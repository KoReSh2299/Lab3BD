using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedDiscountsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<Discount>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<Discount> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<Discount> GetAll()
        {
            return _kursachContext.Discounts.ToList();
        }

        public IEnumerable<Discount> GetByCount(int countRows = 20)
        {
            return _kursachContext.Discounts.Take(countRows).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<Discount> values)
        {
            return TryGetFromCache(cacheKey, out values);
        }
    }
}
