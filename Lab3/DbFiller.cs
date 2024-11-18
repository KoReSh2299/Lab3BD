using Lab3.Data;
using Lab3.Models;
using System.Collections.Generic;

namespace Lab3
{
    public class DbFiller(KursachContext context)
    {
        private readonly KursachContext _context = context;

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

        public void InitializePayments()
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
                    var freeSpaces = new List<ParkingSpace>();
                    var takedSpaces = new List<ParkingSpace>();
                    var freeCars = new List<Car>();
                    var carsInParkingSpace = new List<Car>();


                    for(int i = 0; i < parkingSpaces.Count; i++)
                    {
                        if (parkingSpaces[i].Car == null)
                        {
                            freeSpaces.Add(parkingSpaces[i]);
                        }
                        else
                        {
                            takedSpaces.Add(parkingSpaces[i]);
                            carsInParkingSpace.Add(parkingSpaces[i].Car);
                        }
                    }

                    foreach(var car in cars)
                    {
                        if(!carsInParkingSpace.Contains(car))
                        {
                            freeCars.Add(car);
                        }
                    }

                    var takedSpacesAndTimeToFree = new Dictionary<ParkingSpace, DateTime>();
                    var carProbabilities = new int[]{ 50, 40, 30, 20, 10 };
                    var timeProbability = 80;

                    var startTime = new DateTime(2024, 9, 1, 0, 0, 0);
                    var endTime = new DateTime(2025, 2, 1, 0, 0, 0);
                    int lowCountHours = 24;
                    int highCountHours = 144;

                    for(var tempTime = startTime; tempTime < endTime; tempTime = tempTime.AddHours(1))
                    {
                        for(int i = 0; i < takedSpacesAndTimeToFree.Count; i++) 
                        {
                            var takedSpace = takedSpacesAndTimeToFree.ElementAt(i);

                            if (tempTime >= takedSpace.Value)
                            {
                                freeCars.Add(takedSpace.Key.Car);
                                carsInParkingSpace.Remove(takedSpace.Key.Car);
                                takedSpaces.Remove(takedSpace.Key);
                                takedSpace.Key.Car = null;
                                freeSpaces.Add(takedSpace.Key);
                                takedSpacesAndTimeToFree.Remove(takedSpace.Key);
                                i--;
                            }
                        }

                        foreach(var probability in carProbabilities)
                        {
                            if (freeSpaces.Count > 0)
                            {
                                if (random.Next(0, 101) < probability)
                                {
                                    if(random.Next(0, 101) < timeProbability)
                                    {
                                        int countHours = random.Next(1, lowCountHours);
                                        var randomCar = freeCars[random.Next(0, freeCars.Count)];
                                        var randomPlace = freeSpaces[random.Next(0, freeSpaces.Count)];


                                        var payment = new Payment()
                                        {
                                            ParkingSpace = randomPlace,
                                            PaymentDate = tempTime,
                                            TimeIn = tempTime,
                                            TimeOut = tempTime.AddHours(countHours)
                                        };


                                        if (randomPlace.IsPenalty)
                                            payment.Tariff = tariffs[1];
                                        else
                                            payment.Tariff = tariffs[0];

                                        if(randomCar.Client.IsRegularClient)
                                        {
                                            payment.Discount = discounts[0];
                                            payment.Amount = countHours * payment.Tariff.Rate * (decimal)(payment.Discount.Percentage / 100.0);
                                        }
                                        else
                                        {
                                            payment.Discount = null;
                                            payment.Amount = countHours * payment.Tariff.Rate;
                                        }

                                        _context.Payments.Add(payment);

                                        freeCars.Remove(randomCar);
                                        carsInParkingSpace.Add(randomCar);
                                        freeSpaces.Remove(randomPlace);
                                        randomPlace.Car = randomCar;
                                        takedSpaces.Add(randomPlace);
                                        takedSpacesAndTimeToFree.Add(randomPlace, tempTime.AddHours(countHours));
                                    }
                                    else
                                    {
                                        int countHours = random.Next(lowCountHours, highCountHours);
                                        var randomCar = freeCars[random.Next(0, freeCars.Count)];
                                        var randomPlace = freeSpaces[random.Next(0, freeSpaces.Count)];

                                        var payment = new Payment()
                                        {
                                            ParkingSpace = randomPlace,
                                            PaymentDate = tempTime,
                                            TimeIn = tempTime,
                                            TimeOut = tempTime.AddHours(countHours)
                                        };


                                        if (randomPlace.IsPenalty)
                                            payment.Tariff = tariffs[1];
                                        else
                                            payment.Tariff = tariffs[0];

                                        if (countHours >= 72)
                                        {
                                            payment.Discount = discounts[1];
                                            payment.Amount = countHours * payment.Tariff.Rate * (decimal)(payment.Discount.Percentage / 100.0);
                                        }
                                        else if (randomCar.Client.IsRegularClient) 
                                        {
                                            payment.Discount = discounts[0];
                                            payment.Amount = countHours * payment.Tariff.Rate * (decimal)(payment.Discount.Percentage / 100.0);
                                        }
                                        else
                                        {
                                         
                                            payment.Discount = null;
                                            payment.Amount = countHours * payment.Tariff.Rate;
                                        }

                                        _context.Payments.Add(payment);

                                        freeCars.Remove(randomCar);
                                        carsInParkingSpace.Add(randomCar);
                                        freeSpaces.Remove(randomPlace);
                                        randomPlace.Car = randomCar;
                                        takedSpaces.Add(randomPlace);
                                        takedSpacesAndTimeToFree.Add(randomPlace, tempTime.AddHours(countHours));
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void InitializeWorkShiftsPayments()
        {
            try
            {
                var workShifts = _context.WorkShifts.ToList();
                var payments = _context.Payments.ToList();

                if(payments != null && workShifts != null)
                {
                    for(int i = 0; i < workShifts.Count; i++)
                    {
                        for(int j = 0; j < payments.Count; j++)
                        {
                            if ((payments[j].TimeIn >= workShifts[i].ShiftStartTime && payments[j].TimeIn < workShifts[i].ShiftEndTime)
                             || (payments[j].TimeOut > workShifts[i].ShiftStartTime && payments[j].TimeOut <= workShifts[i].ShiftEndTime)
                             || (payments[j].TimeIn <= workShifts[i].ShiftStartTime && payments[j].TimeOut >= workShifts[i].ShiftEndTime))
                            {
                                var workShiftPayment = new WorkShiftsPayment()
                                {
                                    WorkShift = workShifts[i],
                                    Payment = payments[j]
                                };

                                _context.WorkShiftsPayments.Add(workShiftPayment);
                            }
                            else if (payments[j].TimeOut <= workShifts[i].ShiftStartTime)
                            {
                                continue;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
