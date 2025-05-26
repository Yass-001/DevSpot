

using DevSpot.Constants;
using Microsoft.AspNetCore.Identity;

namespace DevSpot.Data
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (! await roleManager.RoleExistsAsync(Roles.ADMIN))
            {
                var role = new IdentityRole(Roles.ADMIN);
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(Roles.JOB_SEEKER))
            {
                var role = new IdentityRole(Roles.JOB_SEEKER);
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync(Roles.EMPLOYER))
            {
                var role = new IdentityRole(Roles.EMPLOYER);
                await roleManager.CreateAsync(role);
            }

            //return Task.CompletedTask;
        }
    }
}
