using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SmartHotel.Services.Hotels.Extensions;
using SmartHotel.Services.Hotels.Data;
using SmartHotel.Services.Hotels.Data.Seed;

namespace SmartHotel.Services.Hotels
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<HotelsDbContext>((context, services) =>
                {
                    var db = services.GetService<HotelsDbContext>();
                    HotelsDbContextSeed.Seed(db);
                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .UseApplicationInsights()
                        .Build();
    }
}
