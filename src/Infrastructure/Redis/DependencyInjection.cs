using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Redis.Services;
using StackExchange.Redis;

namespace Infrastructure.Redis
{
    public static class DependencyInjection
    {
        public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(config =>
                ConnectionMultiplexer.Connect(configuration["RedisConnection"])
            );
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
    }
}