using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SmartHotel.Services.Bookings.Extensions;
using SmartHotel.Services.Bookings.Data;
namespace SmartHotel.Services.Bookings
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
    }
}
