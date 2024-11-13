using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedWorkShiftsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<WorkShift>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<WorkShift> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<WorkShift> GetAll()
        {
            return _kursachContext.WorkShifts.Include(workShift => workShift.Employee).ToList();
        }

        public IEnumerable<WorkShift> GetByCount(int countRows = 20)
        {
            return _kursachContext.WorkShifts.Take(countRows).Include(workShift => workShift.Employee).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<WorkShift> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
