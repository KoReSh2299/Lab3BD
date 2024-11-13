namespace Lab3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");
            app.MapGet("/Home", () => "ghgfh");

            int x = 2;
            app.Run(async (context) => { x *= 2; await context.Response.WriteAsync(x.ToString()); });
            app.Run();
        }
    }
}
