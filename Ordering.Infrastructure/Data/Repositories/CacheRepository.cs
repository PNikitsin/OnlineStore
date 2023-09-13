using Microsoft.Extensions.Caching.Distributed;
using Ordering.Domain.Interfaces;
using System.Text.Json;

namespace Ordering.Infrastructure.Data.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _cache;

        public CacheRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetDataAsync<T>(string cacheKey)
        {
            var data = await _cache.GetAsync(cacheKey);

            if (data == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task SetDataAsync<T>(string cacheKey, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(300),
                SlidingExpiration = TimeSpan.FromSeconds(300)
            };

            var jsonData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(cacheKey, jsonData, options);
        }

        public async Task RemoveAsync(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }
    }
}