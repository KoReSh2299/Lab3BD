using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedRegularClientsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<RegularClient>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<RegularClient> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<RegularClient> GetAll()
        {
            return _kursachContext.RegularClients.ToList();
        }

        public IEnumerable<RegularClient> GetByCount(int countRows = 20)
        {
            return _kursachContext.RegularClients.Take(countRows).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<RegularClient> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
