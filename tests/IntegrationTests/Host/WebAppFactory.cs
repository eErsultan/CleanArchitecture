using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebAPI;
using Infrastructure.Identity.Context;
using Infrastructure.Persistence.Context;

namespace IntegrationTests.Host
{
    public class CrmWebAppFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var applicationDbContext = services.SingleOrDefault(d => 
                    d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (applicationDbContext != null)
                    services.Remove(applicationDbContext);

                var identityContext = services.SingleOrDefault(d => 
                    d.ServiceType == typeof(DbContextOptions<IdentityContext>));

                if (identityContext != null)
                    services.Remove(identityContext);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryApplicationDb");
                });

                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryIdentityDb");
                });
            });
        }

        //protected async Task AuthenticateAsync()
        //{
        //    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await GetJwtAsync());
        //}

        //private async Task<string> GetJwtAsync()
        //{
        //    await _client.PostAsync(ApiRoutes.Account.Register.Replace("{version:apiVersion}", "1"),
        //        new JsonContent(
        //            new RegisterRequest()
        //            {
        //                UserName = "erkon",
        //                Password = "1234567e"
        //            }));

        //    var response = await _client.PostAsync(ApiRoutes.Account.Login.Replace("{version:apiVersion}", "1"),
        //        new JsonContent(
        //            new LoginRequest()
        //            {
        //                UserName = "erkon",
        //                Password = "1234567e"
        //            }));

        //    var stringResponse = await response.Content.ReadAsStringAsync();
        //    var authResponse = JsonConvert.DeserializeObject<Response<AuthResponse>>(stringResponse);

        //    return authResponse.Data.Token;
        //}
    }
}