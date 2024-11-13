using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedTariffsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<Tariff>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<Tariff> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<Tariff> GetAll()
        {
            return _kursachContext.Tariffs.ToList();
        }

        public IEnumerable<Tariff> GetByCount(int countRows = 20)
        {
            return _kursachContext.Tariffs.Take(countRows).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<Tariff> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
