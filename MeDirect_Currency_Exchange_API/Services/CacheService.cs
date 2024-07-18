using MeDirect_Currency_Exchange_API.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace MeDirect_Currency_Exchange_API.Services {
    public class CacheService : ICacheService {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }
        public T Get<T>(string key) {
            _memoryCache.TryGetValue(key, out T item);
            return item;
        }
        public void Set<T>(string key, T item, TimeSpan? expiration) {
            var cacheEntryOptions = new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = expiration
            };
            _memoryCache.Set(key, item, cacheEntryOptions);
        }
        public void Remove(string key) {
            _memoryCache.Remove(key);
        }
    }
}
