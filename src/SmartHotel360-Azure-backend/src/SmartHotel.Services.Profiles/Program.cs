using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartHotel.Services.Profiles.Data;
using SmartHotel.Services.Profiles.Data.Seed;
using SmartHotel.Services.Profiles.Extensions;
using Microsoft.Extensions.DependencyInjection;
using SmartHotel.Services.Profiles.Extensions;

namespace SmartHotel.Services.Profiles
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                  .MigrateDbContext<ProfilesDbContext>((context, services) =>
                  {
                      var db = services.GetService<ProfilesDbContext>();
                      ProfilesDbContextSeed.Seed(db);
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
