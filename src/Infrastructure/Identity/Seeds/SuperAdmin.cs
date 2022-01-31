using Application.Constants;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class SuperAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var userName = "admin";
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = userName
                };

                await userManager.CreateAsync(admin, "u_-9SA3ss3umZ5b:{ZKD");
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }
        }
    }
}
