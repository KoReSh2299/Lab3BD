using Lab2proj.Data;
using Lab2proj.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3.Services
{
    public class CachedParkingRecordsService(IMemoryCache memoryCache, KursachContext kursachContext) : ICachedService<ParkingRecord>
    {
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly KursachContext _kursachContext = kursachContext;

        public void AddIntoCache(string cacheKey, IEnumerable<ParkingRecord> values)
        {
            if(values != null)
            {
                _memoryCache.Set(cacheKey, values, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(254)));
            }
        }

        public IEnumerable<ParkingRecord> GetAll()
        {
            return _kursachContext.ParkingRecords
                .Include(parkingRecord => parkingRecord.Car).ThenInclude(car => car.Client)
                .Include(parkingRecord => parkingRecord.ParkingSpace)
                .Include(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Tariff)
                .Include(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Discount);
        }

        public IEnumerable<ParkingRecord> GetByCount(int countRows = 20)
        {
            return _kursachContext.ParkingRecords.Take(countRows)
                .Include(parkingRecord => parkingRecord.Car).ThenInclude(car => car.Client)
                .Include(parkingRecord => parkingRecord.ParkingSpace)
                .Include(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Tariff)
                .Include(parkingRecord => parkingRecord.Payment).ThenInclude(payment => payment.Discount);
        }

        public bool TryGetFromCache(string cacheKey, out IEnumerable<ParkingRecord> values)
        {
            return _memoryCache.TryGetValue(cacheKey, out values);
        }
    }
}
