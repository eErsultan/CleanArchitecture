using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICacheService
    {
        Task<string> GetCacheValueAsync(string key);
        Task SetCacheValueAsync(string key, string value);
        Task RemoveCacheValueAsync(string key);
    }
}