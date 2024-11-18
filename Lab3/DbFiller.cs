using Lab3.Data;
using Lab3.Models;

namespace Lab3
{
    public class DbFiller
    {
        private KursachContext _context;

        public DbFiller(KursachContext context)
        {
            _context = context;
        }

        public void InitializeWorkShifts()
        {
            try
            {
                var employes = _context.Employees.ToList();

                if (employes != null)
                {
                    var random = new Random();
                    var startDate = new DateTime(2024, 9, 1, 0, 0, 0);
                    int dHours = 8;
                    int countEntityes = 2000;

                    for (int i = 0; i < countEntityes; i++)
                    {
                        var newShift = new WorkShift()
                        {
                            ShiftStartTime = startDate,
                            ShiftEndTime = startDate.AddHours(dHours),
                            EmployeeId = employes[random.Next(0, employes.Count - 1)].Id
                        };

                        startDate = startDate.AddHours(dHours);

                        _context.WorkShifts.Add(newShift);
                    }

                    _context.SaveChanges();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InitializeParkingSpaces()
        {
            try
            {
                int countEnt = 100;

                for (int i = 0;i < countEnt;i++)
                {
                    bool isPenalty = true;

                    if (i < 70)
                    {
                        isPenalty = false;
                    }

                    var newPlace = new ParkingSpace()
                    {
                        IsPenalty = isPenalty,
                        CarId = null
                    };

                    _context.ParkingSpaces.Add(newPlace);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InitializeTariffs()
        {
            var tariff1 = new Tariff()
            {
                Rate = 10,
                Description = "Часовая оплата за нахождение на основной стоянке"
            };

            var tariff2 = new Tariff()
            {
                Rate = 15,
                Description = "Часовая оплата за нахождение на штрафстоянке"
            };

            _context.Tariffs.AddRange(tariff1, tariff2);

            _context.SaveChanges();
        }

        public void InitializeDiscounts()
        {
            var discount1 = new Discount()
            {
                Percentage = 10,
                Description = "Скидка постоянному клиенту"
            };

            var discount2 = new Discount()
            {
                Percentage = 15,
                Description = "Скидка за нахождение на стоянке более 3-х дней"
            };

            _context.Discounts.AddRange(discount1, discount2);
            _context.SaveChanges();
        }

        public void InitializePaymentsAndParkingRecords()
        {
            try
            {
                var discounts = _context.Discounts.ToList();
                var tariffs = _context.Tariffs.ToList();
                var parkingSpaces = _context.ParkingSpaces.ToList();
                var clients = _context.Clients.ToList();
                var cars = _context.Cars.ToList();

                if(tariffs != null && discounts != null && parkingSpaces != null && clients != null && cars != null)
                {
                    var random = new Random();
                    var freeSpaces = new HashSet<ParkingSpace>();
                    var takedSpaces = new Dictionary<ParkingSpace, Car>();


                    for(int i = 0; i < parkingSpaces.Count(); i++)
                    {
                        if (parkingSpaces[i].IsFree)
                            freeSpaces.Add(parkingSpaces[i]);
                        else
                            takedSpaces.Add(parkingSpaces[i]);
                    }

                    var takedSpacesAndTimeToFree = new Dictionary<ParkingSpace, DateTime>();
                    var carProbabilities = new int[]{ 50, 40, 30, 20, 10 };
                    var timeProbability = 80;

                    var startTime = new DateTime(2024, 9, 1, 0, 0, 0);
                    var endTime = new DateTime(2025, 2, 1, 0, 0, 0);

                    for(var tempTime = startTime; tempTime < endTime; tempTime = tempTime.AddHours(1))
                    {
                        foreach(var entity in takedSpacesAndTimeToFree)
                        {
                            if(tempTime >= entity.Value)
                            {
                                takedSpaces.Remove(entity.Key);
                                freeSpaces.Add(entity.Key);
                            }
                        }

                        foreach(var probability in carProbabilities)
                        {
                            if (freeSpaces.Count > 0)
                            {
                                if (random.Next(0, 99) < probability)
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
