using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Redis.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _multiplexer;

        public RedisCacheService(IConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            var db = _multiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task SetCacheValueAsync(string key, string value)
        {
            var db = _multiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async Task RemoveCacheValueAsync(string keys)
        {
            var removeKeys = keys.Split(",");

            var db = _multiplexer.GetDatabase();
            var server = _multiplexer.GetServer("localhost:6379");
            var redisKeys = server.Keys();
            
            Parallel.ForEach(redisKeys, k =>
                {
                    var redisKey = k.ToString().Split("/");
                    if (removeKeys.Contains(redisKey[^1]))
                    {
                        db.StringGetDeleteAsync(k);
                    }
                });
        }
    }
}