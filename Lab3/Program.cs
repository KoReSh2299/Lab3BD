using Lab3.Data;
using Lab3.Models;
using Lab3.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace Lab3
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //var builder = WebApplication.CreateBuilder(args);

            //var services = builder.Services;

            //services.AddDbContext<KursachContext>();

            //services.AddMemoryCache();

            //services.AddDistributedMemoryCache();
            //services.AddSession();

            //services.AddScoped<ICachedService<Car>, CachedCarsService>();
            //services.AddScoped<ICachedService<Employee>, CachedEmployeesService>();
            //services.AddScoped<ICachedService<Client>, CachedClientsService>();
            //services.AddScoped<ICachedService<Discount>, CachedDiscountsService>();
            //services.AddScoped<ICachedService<EmployeeMonthlyShift>, CachedEmployeeMonthlyShiftsService>();
            //services.AddScoped<ICachedService<ParkingRecord>, CachedParkingRecordsService>();
            //services.AddScoped<ICachedService<ParkingRecordsWorkShift>, CachedParkingRecordsWorkShiftsService>();
            //services.AddScoped<ICachedService<ParkingSpace>, CachedParkingSpacesService>();
            //services.AddScoped<ICachedService<Payment>, CachedPaymentsService>();
            //services.AddScoped<ICachedService<RegularClient>, CachedRegularClientsService>();
            //services.AddScoped<ICachedService<Tariff>, CachedTariffsService>();
            //services.AddScoped<ICachedService<WorkShift>, CachedWorkShiftsService>();

            var optionsBuilder = new DbContextOptionsBuilder<KursachContext>();
            optionsBuilder.UseSqlServer("Data Source=LCR\\SQLEXPRESS;Database=Kursach;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

            using var dbContext = new KursachContext(optionsBuilder.Options);

            var dbFiller = new DbFiller(dbContext);
            //dbFiller.InitializeWorkShifts();
            //dbFiller.InitializeParkingSpaces();
            //dbFiller.InitializeTariffs();
            dbFiller.InitializeDiscounts();

            //var app = builder.Build();

            //app.Map("/info", (appBuilder) =>
            //{
            //    appBuilder.Run(async (context) =>
            //    {
            //        string strResponse = "<HTML><HEAD><TITLE>Информация</TITLE></HEAD>" +
            //        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            //        "<BODY><H1>Информация:</H1>";
            //        strResponse += "<BR> Сервер: " + context.Request.Host;
            //        strResponse += "<BR> Путь: " + context.Request.PathBase;
            //        strResponse += "<BR> Протокол: " + context.Request.Protocol;
            //        strResponse += "<BR><A href='/'>Главная</A></BODY></HTML>";

            //        await context.Response.WriteAsync(strResponse);
            //    });
            //});

            //app.Map("/Cars", (appBuilder) =>
            //{
            //    appBuilder.Run(async (context) =>
            //    {
            //        var cachedCarsService = context.RequestServices.GetService<ICachedService<Car>>();

            //        if(!cachedCarsService.TryGetFromCache("Cars", out var values))
            //        {
            //            values = cachedCarsService.GetByCount(20);
            //            cachedCarsService.AddIntoCache("Cars", values);
            //        }

            //        string HtmlString = "<HTML><HEAD><TITLE>Автомобили</TITLE></HEAD>" +
            //        "<META http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
            //        "<BODY><H1>Список автомобилей</H1>" +
            //        "<TABLE BORDER=1>";
            //        HtmlString += "<TR>";
            //        HtmlString += "<TH>Брэнд</TH>";
            //        HtmlString += "<TH>Номер</TH>";
            //        HtmlString += "<TH>ФИО клиента</TH>";
            //        HtmlString += "</TR>";
            //        foreach (var car in values)
            //        {
            //            HtmlString += "<TR>";
            //            HtmlString += "<TD>" + car.Brand + "</TD>";
            //            HtmlString += "<TD>" + car.Number + "</TD>";
            //            HtmlString += "<TD>" + $"{ car.Client.Surname} {car.Client.Name} {car.Client.MiddleName}" + "</TD>";
            //            HtmlString += "</TR>";
            //        }
            //        HtmlString += "</TABLE>";
            //        HtmlString += "<BR><A href='/'>Главная</A></BR>";
            //        HtmlString += "<BR><A href='/form'>Данные пользователя</A></BR>";
            //        HtmlString += "</BODY></HTML>";

            //        await context.Response.WriteAsync(HtmlString);
            //    });
            //});

            //app.Run();
        }
    }
}
