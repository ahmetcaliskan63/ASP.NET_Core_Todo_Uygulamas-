using Microsoft.Extensions.Caching.Memory;
using System;

namespace ToDoUygulaması.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            _memoryCache.TryGetValue(key, out T value);
            return value;
        }

        public bool Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new MemoryCacheEntryOptions();

            if (expiry.HasValue)
            {
                options.SetAbsoluteExpiration(expiry.Value);
            }
            else
            {
                options.SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Varsayılan 10 dakika
            }

            _memoryCache.Set(key, value, options);
            return true;
        }

        public bool Remove(string key)
        {
            _memoryCache.Remove(key);
            return true;
        }
    }
}