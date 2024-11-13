using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedEmployeeMonthlyShiftsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<EmployeeMonthlyShift>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<EmployeeMonthlyShift> values)
        {
            if(values !=  null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<EmployeeMonthlyShift> GetAll()
        {
            return _kursachContext.EmployeeMonthlyShifts.ToList();
        }

        public IEnumerable<EmployeeMonthlyShift> GetByCount(int countRows = 20)
        {
            return _kursachContext.EmployeeMonthlyShifts.Take(countRows).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<EmployeeMonthlyShift> values)
        {
            return TryGetFromCache(cacheKey, out values);
        }
    }
}
