using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedPaymentsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<Payment>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<Payment> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            return _kursachContext.Payments.Include(payment => payment.Tariff).Include(payment => payment.Discount).ToList();
        }

        public IEnumerable<Payment> GetByCount(int countRows = 20)
        {
            return _kursachContext.Payments.Take(countRows).Include(payment => payment.Tariff).Include(payment => payment.Discount).ToList();
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<Payment> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
