using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedEmployeesService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<Employee>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<Employee> values)
        {
            if(values == null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<Employee> GetAll()
        {
            return _kursachContext.Employees.ToList();
        }

        public IEnumerable<Employee> GetByCount(int countRows = 20)
        {
            return _kursachContext.Employees.Take(countRows).ToList();  
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<Employee> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
