using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SmartHotel.Services.Bookings.Settings;
using SmartHotel.Services.Bookings.Data;
using SmartHotel.Services.Bookings.Queries;
using Swashbuckle.AspNetCore.Swagger;
using SmartHotel.Services.Bookings.Commands;
using SmartHotel.Services.Bookings.Data.Repositories;
using SmartHotel.Services.Bookings.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SmartHotel.Services.Bookings
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                     .SetBasePath(env.ContentRootPath)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            if (!string.IsNullOrEmpty(Configuration["k8sname"]))
            {
                services.EnableKubernetes();
            }
            services.AddMvc();

            services.AddTransient<UserBookingQuery>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<BookingRepository>();

            services.AddTransient<UserBookingQuery>();
            services.AddTransient<CreateBookingCommand>();

            services.AddTransient<OccupancyQuery>(sp => new OccupancyQuery(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Bookings Api", Version = "v1" });
            });

            services.Configure<AppSettings>(Configuration);
            services.Configure<SecuritySettings>(Configuration.GetSection("b2c"));

            services.ConfigureSwaggerGen(swaggerGen =>
            {
                var b2cConfig = Configuration.GetSection("b2c");
                var authority = string.Format("https://login.microsoftonline.com/{0}/oauth2/v2.0", b2cConfig["Tenant"]);
                swaggerGen.AddSecurityDefinition("Swagger", new OAuth2Scheme
                {
                    AuthorizationUrl = authority + "/authorize",
                    Flow = "implicit",
                    TokenUrl = authority + "/connect/token",
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid","User offline" },
                    }
                });

            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(jwtOpt =>
            {
                var b2cConfig = Configuration.GetSection("b2c");
                jwtOpt.Authority = string.Format("https://login.microsoftonline.com/tfp/{0}/{1}/v2.0/", b2cConfig["tenant"], b2cConfig["policy"]);

                jwtOpt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudiences = b2cConfig["audiences"].Split(',')
                };
                jwtOpt.Events = new JwtBearerEvents();
            });

            services.AddDbContext<BookingsDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            return services.BuildServiceProvider();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
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


            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseByPassAuth();
            app.UseAuthentication();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                var b2cConfig = Configuration.GetSection("b2c");
                var path = string.IsNullOrEmpty(pbase) || pbase == "/" ? "/" : $"{pbase}/";
                c.InjectOnCompleteJavaScript($"{path}swagger-ui-b2c.js");
                c.SwaggerEndpoint($"{path}swagger/v1/swagger.json", "Bookings Api");
                c.ConfigureOAuth2(b2cConfig["ClientId"], "y", "z", "z", "",
                    new
                    {
                        p = "B2C_1_SignUpInPolicy",
                        prompt = "login",
                        nonce = "defaultNonce"
                    });
            });

            app.UseMvc();
        }
    }
}
