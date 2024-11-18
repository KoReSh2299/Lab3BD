using Lab3.Data;
using Lab3.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedClientsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<Client>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<Client> clients)
        {
            if(clients !=  null)
            {
                _memoryCache.Set(cacheKey, clients, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<Client> GetAll()
        {
            return _kursachContext.Clients.ToList();
        }

        public IEnumerable<Client> GetByCount(int countRows = 20)
        {
            return _kursachContext.Clients.Take(countRows).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<Client> clients)
        {
            if(_memoryCache.TryGetValue(cacheKey, out clients))
            {
                return true;
            }

            return false;
        }
    }
}
