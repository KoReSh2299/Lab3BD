using Lab3.Data;
using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedWorkShiftsPaymentService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<WorkShiftsPayment>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<WorkShiftsPayment> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<WorkShiftsPayment> GetAll()
        {
            return _kursachContext.WorkShiftsPayments
                .Include(x => x.WorkShift).ThenInclude(workShift => workShift.Employee)
                .Include(x => x.Payment).ThenInclude(payment => payment.ParkingSpace).ThenInclude(parkingSpace => parkingSpace.Car).ThenInclude(car => car.Client)
                .Include(x => x.Payment).ThenInclude(payment => payment.Tariff)
                .Include(x => x.Payment).ThenInclude(payment => payment.Discount);
        }

        public IEnumerable<WorkShiftsPayment> GetByCount(int countRows = 20)
        {
            return _kursachContext.WorkShiftsPayments.Take(countRows)
                .Include(x => x.WorkShift).ThenInclude(workShift => workShift.Employee)
                .Include(x => x.Payment).ThenInclude(payment => payment.ParkingSpace).ThenInclude(parkingSpace => parkingSpace.Car).ThenInclude(car => car.Client)
                .Include(x => x.Payment).ThenInclude(payment => payment.Tariff)
                .Include(x => x.Payment).ThenInclude(payment => payment.Discount);
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<WorkShiftsPayment> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
