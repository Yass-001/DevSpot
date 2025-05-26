using Microsoft.AspNetCore.Identity;
using DevSpot.Constants;

namespace DevSpot.Data
{
    public class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await CreateUserWithRole(userManager, "admin@devspot.com", "Admin@123", Roles.ADMIN);
            await CreateUserWithRole(userManager, "jobseeker@devspot.com", "Jobseeker@123", Roles.JOB_SEEKER);
            await CreateUserWithRole(userManager, "employer@devspot.com", "Employer@123", Roles.EMPLOYER);
        }

        private static async Task CreateUserWithRole(
            UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };
                // Create the user with a password
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    throw new Exception($"Failed to create user wit email: {user.Email}. Error: {string.Join(", ", result.Errors)}");
                    // result.Errors.Select(e => e.Description)
                }
            }
        }
    }
}