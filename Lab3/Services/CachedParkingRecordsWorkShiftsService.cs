using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedParkingRecordsWorkShiftsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<ParkingRecordsWorkShift>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<ParkingRecordsWorkShift> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<ParkingRecordsWorkShift> GetAll()
        {
            return _kursachContext.ParkingRecordsWorkShifts
                .Include(x => x.WorkShift).ThenInclude(workShift => workShift.Employee)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.Car).ThenInclude(car => car.Client)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.ParkingSpace)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Tariff)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Discount);
        }

        public IEnumerable<ParkingRecordsWorkShift> GetByCount(int countRows = 20)
        {
            return _kursachContext.ParkingRecordsWorkShifts.Take(countRows)
                .Include(x => x.WorkShift).ThenInclude(workShift => workShift.Employee)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.Car).ThenInclude(car => car.Client)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.ParkingSpace)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Tariff)
                .Include(x => x.ParkingRecord).ThenInclude(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Discount);
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<ParkingRecordsWorkShift> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
