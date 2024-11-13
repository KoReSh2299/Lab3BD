using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedParkingSpacesService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<ParkingSpace>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<ParkingSpace> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<ParkingSpace> GetAll()
        {
            return _kursachContext.ParkingSpaces.ToList();
        }

        public IEnumerable<ParkingSpace> GetByCount(int countRows = 20)
        {
            return _kursachContext.ParkingSpaces.Take(countRows).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<ParkingSpace> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
