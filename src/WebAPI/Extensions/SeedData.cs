using Application.Interfaces.Repositories;
using Infrastructure.Identity.Models;
using Infrastructure.Identity.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class SeedData
    {
        public static async Task InitializeBaseUser(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

                await DefaultRoles.SeedAsync(roleManager);
                await SuperAdmin.SeedAsync(userManager);
            }
        }
    }
}
