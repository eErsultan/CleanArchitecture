using Application.Constants;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            var roles = new string[] { Roles.Admin, Roles.Developer };
            foreach (var role in roles)
            {
                await AddRole(roleManager, role);
            }
        }

        private static async Task AddRole(RoleManager<IdentityRole<int>> roleManager, string role)
        {
            var adminRole = await roleManager.FindByNameAsync(role);
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }
}
