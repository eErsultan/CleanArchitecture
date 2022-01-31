using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio.AspNetCore;
using Infrastructure.MinIO.Service.Services;
using Application.Interfaces;

namespace Infrastructure.MinIO.Service
{
    public static class DependencyInjection
    {
        public static void AddMinioInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMinio(options =>
            {
                options.Endpoint = configuration["MinIOSettings:Url"];
                options.AccessKey = configuration["MinIOSettings:AccessKey"];
                options.SecretKey = configuration["MinIOSettings:SecretKey"];
                //options.OnClientConfiguration = client =>
                //{
                //    client.WithSSL();
                //};
            });

            services.AddScoped<IMinIOService, MinIOService>();
        }
    }
}
