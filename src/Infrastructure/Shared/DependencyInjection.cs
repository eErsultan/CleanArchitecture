using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Shared.Services;
using Application.Interfaces;
using Domain.Settings;

namespace Infrastructure.Shared
{
    public static class DependencyInjection
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        }
    }
}
