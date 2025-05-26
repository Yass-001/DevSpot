using Microsoft.AspNetCore.Identity;
using DevSpot.Constants;

namespace DevSpot.Data
{
    public class UserSeeder
    {
        public async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (await userManager.FindByEmailAsync("admin@devspot.com") == null)
            {
                var user = new IdentityUser
                {
                    Email = "admin@devspot.com",
                    UserName = "admin@devspot.com",
                    EmailConfirmed = true
                };
                // Create the user with a password
                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.ADMIN);
                };
            }
        }
    }
}