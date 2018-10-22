using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartHotels.Services.Notifications.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace SmartHotels.Services.Notifications
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
            services.AddTransient<NotificationsService>();
            if (!string.IsNullOrEmpty(Configuration["k8sname"]))
            {
                services.EnableKubernetes();
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Notifications Api", Version = "v1" });
            });

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

            app.UseByPassAuth();
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var b2cConfig = Configuration.GetSection("b2c");
                var path = string.IsNullOrEmpty(pbase) || pbase == "/" ? "/" : $"{pbase}/";
                c.InjectOnCompleteJavaScript($"{path}swagger-ui-b2c.js");
                c.SwaggerEndpoint($"{path}swagger/v1/swagger.json", "Notifications Api");
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
