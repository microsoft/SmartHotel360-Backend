using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmartHotel.Services.Discount.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHotel.Services.Discount
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }       

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (!string.IsNullOrEmpty(Configuration["k8sname"]))
            {
                services.EnableKubernetes();
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Hotels Api", Version = "v1" });
            });

            services.AddTransient<LoyaltyService>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<LoyaltyService>>();
                return new LoyaltyService(Configuration["profilesvc"], logger);
            });
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();
            loggerFactory.AddApplicationInsights(app.ApplicationServices, LogLevel.Trace);

            var pbase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pbase))
            {
                app.UsePathBase(pbase);
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var path = string.IsNullOrEmpty(pbase) || pbase == "/" ? "/" : $"{pbase}/";
                c.SwaggerEndpoint($"{path}swagger/v1/swagger.json", "Discounts Api");
            });

            app.UseMvc();
        }
    }
}
