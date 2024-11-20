using Lab3.Data;
using Lab3.Models;
using Lab3.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Lab3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            services.AddDbContext<KursachContext>();

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddScoped<ICachedService<Car>, CachedCarsService>();
            services.AddScoped<ICachedService<Employee>, CachedEmployeesService>();
            services.AddScoped<ICachedService<Client>, CachedClientsService>();
            services.AddScoped<ICachedService<Discount>, CachedDiscountsService>();
            services.AddScoped<ICachedService<WorkShiftsPayment>, CachedWorkShiftsPaymentService>();
            services.AddScoped<ICachedService<ParkingSpace>, CachedParkingSpacesService>();
            services.AddScoped<ICachedService<Payment>, CachedPaymentsService>();
            services.AddScoped<ICachedService<Tariff>, CachedTariffsService>();
            services.AddScoped<ICachedService<WorkShift>, CachedWorkShiftsService>();

            //var optionsBuilder = new DbContextOptionsBuilder<KursachContext>();
            //optionsBuilder.UseSqlServer("Data Source=LCR\\SQLEXPRESS;Database=Kursach;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

            //using var dbContext = new KursachContext(optionsBuilder.Options);

            //var dbFiller = new DbFiller(dbContext);
            //dbFiller.InitializeWorkShifts();
            //dbFiller.InitializeParkingSpaces();
            //dbFiller.InitializeTariffs();
            //dbFiller.InitializeDiscounts();
            //dbFiller.InitializePayments();
            //dbFiller.InitializeWorkShiftsPayments();

            var app = builder.Build();

            app.UseSession();

            app.Map("/info", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Информация:</H1>";
                    strResponse += "<BR> Сервер: " + context.Request.Host;
                    strResponse += "<BR> Путь: " + context.Request.PathBase;
                    strResponse += "<BR> Протокол: " + context.Request.Protocol;
                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";

                    await context.Response.WriteAsync(strResponse);
                });
            });

            app.Map("/Cars", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<Car>>();

                    if (!cachedCarsService.TryGetFromCache("Cars", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("Cars", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Автомобили</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список автомобилей</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Брэнд</TH>";
                    HtmlString += "<TH>Номер</TH>";
                    HtmlString += "<TH>ФИО клиента</TH>";
                    HtmlString += "</TR>";
                    foreach (var car in values)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + car.Brand + "</TD>";
                        HtmlString += "<TD>" + car.Number + "</TD>";
                        HtmlString += "<TD>" + $"{car.Client.Surname} {car.Client.Name} {car.Client.MiddleName}" + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/Clients", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<Client>>();

                    if (!cachedCarsService.TryGetFromCache("Clients", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("Clients", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Клиенты</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список клиентов</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ФИО</TH>";
                    HtmlString += "<TH>Номер телефона</TH>";
                    HtmlString += "<TH>Является ли постоянным клиентом</TH>";
                    HtmlString += "</TR>";
                    foreach (var client in values)
                    {
                        var isRegular = client.IsRegularClient ? "Да" : "Нет";
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + $"{client.Surname} {client.Name} {client.MiddleName}" + "</TD>";
                        HtmlString += "<TD>" + client.Telephone + "</TD>";
                        HtmlString += "<TD>" + isRegular + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/Discounts", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<Discount>>();

                    if (!cachedCarsService.TryGetFromCache("Discounts", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("Discounts", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Скидки</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Виды скидок</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Описание</TH>";
                    HtmlString += "<TH>Скидочный процент</TH>";
                    HtmlString += "</TR>";
                    foreach (var discount in values)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + discount.Description + "</TD>";
                        HtmlString += "<TD>" + discount.Percentage + "%" + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/Employees", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<Employee>>();

                    if (!cachedCarsService.TryGetFromCache("Employees", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("Employees", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Сотрудники</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список сотрудников</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>ФИО сотрудника</TH>";
                    HtmlString += "</TR>";
                    foreach (var employee in values)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + $"{employee.Surname} {employee.Name} {employee.MiddleName}" + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/ParkingSpaces", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<ParkingSpace>>();

                    if (!cachedCarsService.TryGetFromCache("ParkingSpaces", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("ParkingSpaces", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Парковочные места</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список парковочных мест</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Номер места</TH>";
                    HtmlString += "<TH>Вид парковочного места</TH>";
                    HtmlString += "<TH>Кем занято</TH>";
                    HtmlString += "</TR>";
                    foreach (var parkingSpace in values)
                    {
                        var typePlace = parkingSpace.IsPenalty ? "Штрафная стоянка" : "Основная стоянка";
                        var car = parkingSpace.Car == null ? "Свободно" : parkingSpace.Car.Number;
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + parkingSpace.Id + "</TD>";
                        HtmlString += "<TD>" + typePlace + "</TD>";
                        HtmlString += "<TD>" + car + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/Tariffs", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<Tariff>>();

                    if (!cachedCarsService.TryGetFromCache("Tariffs", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("Tariffs", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Тарифы</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список тарифов</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Сумма</TH>";
                    HtmlString += "<TH>Описание</TH>";
                    HtmlString += "</TR>";
                    foreach (var tariff in values)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + tariff.Rate + "</TD>";
                        HtmlString += "<TD>" + tariff.Description + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/WorkShifts", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<WorkShift>>();

                    if (!cachedCarsService.TryGetFromCache("WorkShifts", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("WorkShifts", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Рабочие смены</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список рабочих смен</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Время начала смены</TH>";
                    HtmlString += "<TH>Время конца смены</TH>";
                    HtmlString += "<TH>ФИО сотрудника</TH>";
                    HtmlString += "</TR>";
                    foreach (var workShift in values)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + $"{workShift.ShiftStartTime.ToShortDateString()} {workShift.ShiftStartTime.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + $"{workShift.ShiftEndTime.ToShortDateString()} {workShift.ShiftEndTime.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + $"{workShift.Employee.Surname} {workShift.Employee.Name} {workShift.Employee.MiddleName}" + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/Payments", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<Payment>>();

                    if (!cachedCarsService.TryGetFromCache("Payments", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("Payments", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Платежи</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список платежей</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Сумма платежа</TH>";
                    HtmlString += "<TH>Тариф</TH>";
                    HtmlString += "<TH>Скидка</TH>";
                    HtmlString += "<TH>Дата платежа</TH>";
                    HtmlString += "<TH>Дата въезда автомобиля на стоянку</TH>";
                    HtmlString += "<TH>Дата выезда автомобиля со стоянки</TH>";
                    HtmlString += "<TH>Номер парковочного места</TH>";
                    HtmlString += "</TR>";
                    foreach (var payment in values)
                    {
                        var discount = payment.Discount == null ? "Отсутствует" : payment.Discount.Description;
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + payment.Amount + "</TD>";
                        HtmlString += "<TD>" + payment.Tariff.Description + "</TD>";
                        HtmlString += "<TD>" + discount + "</TD>";
                        HtmlString += "<TD>" + $"{payment.PaymentDate.ToShortDateString()} {payment.PaymentDate.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + $"{payment.TimeIn.ToShortDateString()} {payment.TimeIn.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + $"{payment.TimeOut.ToShortDateString()} {payment.TimeOut.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + payment.ParkingSpaceId + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/WorkShiftsPayments", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var cachedCarsService = context.RequestServices.GetService<ICachedService<WorkShiftsPayment>>();

                    if (!cachedCarsService.TryGetFromCache("WorkShiftsPayments", out var values))
                    {
                        values = cachedCarsService.GetByCount(20);
                        cachedCarsService.AddIntoCache("WorkShiftsPayments", values);
                    }

                    string HtmlString = "<HTML><HEAD><TITLE>Платежи и смены</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><H1>Список платежей и смен</H1>" +
                    "<TABLE BORDER=1>";
                    HtmlString += "<TR>";
                    HtmlString += "<TH>Сумма платежа</TH>";
                    HtmlString += "<TH>Дата платежа</TH>";
                    HtmlString += "<TH>Дата въезда автомобиля на стоянку</TH>";
                    HtmlString += "<TH>Дата выезда автомобиля со стоянки</TH>";
                    HtmlString += "<TH>Номер парковочного места</TH>";
                    HtmlString += "<TH>ФИО сотрудника</TH>";
                    HtmlString += "</TR>";
                    foreach (var workShiftsPayment in values)
                    {
                        HtmlString += "<TR>";
                        HtmlString += "<TD>" + workShiftsPayment.Payment.Amount + "</TD>";
                        HtmlString += "<TD>" + $"{workShiftsPayment.Payment.PaymentDate.ToShortDateString()} {workShiftsPayment.Payment.PaymentDate.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + $"{workShiftsPayment.Payment.TimeIn.ToShortDateString()} {workShiftsPayment.Payment.TimeIn.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + $"{workShiftsPayment.Payment.TimeOut.ToShortDateString()} {workShiftsPayment.Payment.TimeOut.ToShortTimeString()}" + "</TD>";
                        HtmlString += "<TD>" + workShiftsPayment.Payment.ParkingSpaceId + "</TD>";
                        HtmlString += "<TD>" + $"{workShiftsPayment.WorkShift.Employee.Surname} {workShiftsPayment.WorkShift.Employee.Name} {workShiftsPayment.WorkShift.Employee.MiddleName}" + "</TD>";
                        HtmlString += "</TR>";
                    }
                    HtmlString += "</TABLE>";
                    HtmlString += "<BR><A href='/'>Главная</A></BR>";
                    HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
                    HtmlString += "</BODY></HTML>";

                    await context.Response.WriteAsync(HtmlString);
                });
            });

            app.Map("/searchform1", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    
                    var payment = context.Session.Get<Payment>("Payments") ?? new Payment();

                    var cachedPaymentsService = context.RequestServices.GetService<ICachedService<Payment>>();
                    if (!cachedPaymentsService.TryGetFromCache("Payments", out var payments20))
                    {
                        payments20 = cachedPaymentsService.GetByCount(20);
                        cachedPaymentsService.AddIntoCache("Payments", payments20);
                    }
                    var payments = cachedPaymentsService.GetAll().ToList();

                    var cachedTariffsService = context.RequestServices.GetService<ICachedService<Tariff>>();
                    if (!cachedTariffsService.TryGetFromCache("Tariffs", out var tariffs))
                    {
                        tariffs = cachedTariffsService.GetByCount(20);
                        cachedTariffsService.AddIntoCache("Tariffs", tariffs);
                    }

                    string strResponse = "<HTML><HEAD><TITLE>Платёж</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/searchform1' method='GET'>" +
                    "Ограничение по сумме платежа:<BR><INPUT type='text' name='AmountRestriction' value='" + payment.Amount + "'><BR>" +
                    "Тарифы:<BR><SELECT name='TariffId'>";
                    foreach (var tariff in tariffs)
                    {
                        bool isSelected = payment.Tariff == tariff;
                        strResponse += $"<OPTION value='{tariff.Id}' {(isSelected ? "selected" : "")}>{tariff.Description}</OPTION>";
                    }
                    strResponse += "</SELECT><BR>" +

                    "<BR><INPUT type='submit' value='Сохранить в Session и найти в базе данных'></FORM>";

                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";


                    if (!string.IsNullOrEmpty(context.Request.Query["AmountRestriction"]))
                        payment.Amount = int.Parse(context.Request.Query["AmountRestriction"]);
                    else
                        payment.Amount = decimal.MaxValue;

                    if (int.TryParse(context.Request.Query["TariffId"], out int tariffId))
                    {
                        payment.TariffId = tariffId;
                    }

                    var results = new List<Payment>();

                    if (payment != default(Payment))
                    {
                        results = payments.Where(p => p.Amount <= payment.Amount && p.TariffId == payment.TariffId).ToList();
                    }

                    var html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Поиск из бд</title></head><body>";
                    html += "<h1>Найденные результаты</h1>";

                    if (results.Count > 0)
                    {
                        html += "<table border='1' style='border-collapse:collapse'>";
                        html += "<tr><th>Сумма платежа</th><th>Тариф</th><th>Скидка</th><th>Дата платежа</th><th>Дата въезда автомобиля на стоянку</th><th>Дата выезда автомобиля со стоянки</th><th>Номер парковочного места</th></tr>";
                        foreach (var pmnt in results)
                        {
                            var discount = pmnt.Discount == null ? "Отсутствует" : pmnt.Discount.Description;
                            html += "<TR>";
                            html += "<TD>" + pmnt.Amount + "</TD>";
                            html += "<TD>" + pmnt.Tariff.Description + "</TD>";
                            html += "<TD>" + discount + "</TD>";
                            html += "<TD>" + $"{pmnt.PaymentDate.ToShortDateString()} {pmnt.PaymentDate.ToShortTimeString()}" + "</TD>";
                            html += "<TD>" + $"{pmnt.TimeIn.ToShortDateString()} {pmnt.TimeIn.ToShortTimeString()}" + "</TD>";
                            html += "<TD>" + $"{pmnt.TimeOut.ToShortDateString()} {pmnt.TimeOut.ToShortTimeString()}" + "</TD>";
                            html += "<TD>" + pmnt.ParkingSpaceId + "</TD>";
                            html += "</TR>";
                        }
                        html += "</table>";
                    }
                    else
                    {
                        html += "<p>Ничего не найдено.</p>";
                    }
                    html += "</body></html>";

                    context.Session.Set<Payment>("Payment", payment);

                    await context.Response.WriteAsync(strResponse + html);
                });
            });

            app.Map("/searchform2", (appBuilder) =>
            {
                appBuilder.Run(async (context) =>
                {
                    var payment = new Payment();

                    if (context.Request.Cookies.ContainsKey("AmountDestriction"))
                    {
                        payment.Amount = int.Parse(context.Request.Cookies["AmountDestriction"]);
                    }

                    if (context.Request.Cookies.ContainsKey("TariffId"))
                    {
                        payment.TariffId = int.Parse(context.Request.Cookies["TariffId"]);
                    }

                    var paymentsService = context.RequestServices.GetService<ICachedService<Payment>>();
                    var payments = paymentsService.GetAll().ToList();

                    var tariffsService = context.RequestServices.GetService<ICachedService<Tariff>>();
                    var tariffs = tariffsService.GetAll().ToList();

                    string strResponse = "<HTML><HEAD><TITLE>Платёж</TITLE></HEAD>" +
                    "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<BODY><FORM action ='/searchform1' method='GET'>" +
                    "Ограничение по сумме платежа:<BR><INPUT type='text' name='AmountRestriction' value='" + payment.Amount + "'><BR>" +
                    "Тарифы:<BR><SELECT name='TariffId'>";
                    foreach (var tariff in tariffs)
                    {
                        bool isSelected = payment.Tariff == tariff;
                        strResponse += $"<OPTION value='{tariff.Id}' {(isSelected ? "selected" : "")}>{tariff.Description}</OPTION>";
                    }
                    strResponse += "</SELECT><BR>" +

                    "<BR><INPUT type='submit' value='Сохранить в Session и найти в базе данных'></FORM>";

                    strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";



                    if (!string.IsNullOrEmpty(context.Request.Query["AmountRestriction"]))
                        payment.Amount = int.Parse(context.Request.Query["AmountRestriction"]);
                    else
                        payment.Amount = decimal.MaxValue;

                    if (int.TryParse(context.Request.Query["TariffId"], out int tariffId))
                    {
                        payment.TariffId = tariffId;
                    }

                    var results = new List<Payment>();

                    if (payment != default(Payment))
                    {
                        results = payments.Where(p => p.Amount <= payment.Amount && p.TariffId == payment.TariffId).ToList();
                    }

                    var html = "<!DOCTYPE html><html><head><meta charset='UTF-8'><title>Поиск из бд</title></head><body>";
                    html += "<h1>Найденные результаты</h1>";

                    if (results.Count > 0)
                    {
                        html += "<table border='1' style='border-collapse:collapse'>";
                        html += "<tr><th>Сумма платежа</th><th>Тариф</th><th>Скидка</th><th>Дата платежа</th><th>Дата въезда автомобиля на стоянку</th><th>Дата выезда автомобиля со стоянки</th><th>Номер парковочного места</th></tr>";
                        foreach (var pmnt in results)
                        {
                            var discount = pmnt.Discount == null ? "Отсутствует" : pmnt.Discount.Description;
                            html += "<TR>";
                            html += "<TD>" + pmnt.Amount + "</TD>";
                            html += "<TD>" + pmnt.Tariff.Description + "</TD>";
                            html += "<TD>" + discount + "</TD>";
                            html += "<TD>" + $"{pmnt.PaymentDate.ToShortDateString()} {pmnt.PaymentDate.ToShortTimeString()}" + "</TD>";
                            html += "<TD>" + $"{pmnt.TimeIn.ToShortDateString()} {pmnt.TimeIn.ToShortTimeString()}" + "</TD>";
                            html += "<TD>" + $"{pmnt.TimeOut.ToShortDateString()} {pmnt.TimeOut.ToShortTimeString()}" + "</TD>";
                            html += "<TD>" + pmnt.ParkingSpaceId + "</TD>";
                            html += "</TR>";
                        }
                        html += "</table>";
                    }
                    else
                    {
                        html += "<p>Ничего не найдено.</p>";
                    }
                    html += "</body></html>";

                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddSeconds(254)
                    };

                    context.Response.Cookies.Append("AmountRestriction", payment.Amount.ToString(), cookieOptions);
                    context.Response.Cookies.Append("TariffId", payment.TariffId.ToString(), cookieOptions);

                    await context.Response.WriteAsync(strResponse + html);
                });
            });

            app.Run();
        }
    }
}
