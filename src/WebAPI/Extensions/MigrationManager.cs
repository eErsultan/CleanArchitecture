using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Persistence.Context;
using System.Threading.Tasks;
using Infrastructure.Identity.Context;

namespace WebAPI.Extensions
{
    public static class MigrationManager
    {
        public static async Task MigrateContexts(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using (var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
                applicationDbContext.Database.Migrate();
            }

            using (var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>())
            {
                identityContext.Database.Migrate();
            }
        }
    }
}
