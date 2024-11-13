using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedCarsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<Car>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<Car> cars)
        {
            if(cars != null)
            {
                _memoryCache.Set(cacheKey, cars, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(254)
                });
            }
        }

        public IEnumerable<Car> GetAll()
        {
            return _kursachContext.Cars.Include(a => a.Client).ToList();
        }

        public IEnumerable<Car> GetByCount(int countRows = 20)
        {
            return _kursachContext.Cars.Take(countRows).Include(a => a.Client).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<Car> cars)
        {
            if(_memoryCache.TryGetValue(cacheKey, out cars))
            {
                return true;
            }

            return false;
        }
    }
}
