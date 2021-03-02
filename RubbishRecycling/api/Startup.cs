
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RubbishRecyclingAU.ControllerModels.Authentication;
using RubbishRecyclingAU.Services;

#pragma warning disable 1591

namespace RubbishRecyclingAU
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddControllers(options =>
                {
                    options.Filters.Add<AuthenticationWrapperFilter>();
                    options.Filters.Add<RequestAccessorFilter>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;  //to prevent reference loops from happening
                });

            var writerConnStr = Configuration.GetConnectionString("RubbishRecyclingDb");
            if (!string.IsNullOrEmpty(writerConnStr))
            {
                // The connection string will be null when migration are being applied. See
                // MigrationContextFactory.
                //services.AddSingleton(new DbContextOptionsBuilder<RubbishRecyclingContext>()
                //    .UseMySql(writerConnStr)
                //    .Options);

                services.AddDbContext<RubbishRecyclingContext>(options => options.UseMySql(writerConnStr));
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.ExpireTimeSpan = AuthenticationConstants.AuthenticationCookieLifetime;
                        options.SlidingExpiration = true;
                        options.Cookie.Name = AuthenticationConstants.AuthenticationCookieName;
                        // We don't set MaxAge of the cookie because we want it to be a session cookie.
                    });

            services.AddHealthChecks();
            services
                    .AddSingleton(s => new HealthCheckOptions
                    {
                        Predicate = _ => true, // allow all health checks
                        AllowCachingResponses = false,
                    });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<RubbishRecyclingContext>();
            services.AddScoped<IRegisterService, RegisterService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IAuthenticationWrapper, AuthenticationWrapper>();
            services.AddScoped<IAntiforgeryService, AntiforgeryService>();
            services.AddScoped<IRequestAccessor, RequestAccessor>();

            // Register the Swagger services
            services.AddSwaggerDocument();

            services.AddSwaggerGen();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*
             Configuring enables the following:
             * Problem Details
             * HTTPS
             * SwaggerUI
             * Healthchecks (at /health endpoint)
            */
            app.ApplyLoggingMiddleware();
            app.UseRouting(); // required to support /api/{route} routing
            app.UseEndpoints(endpoints => // required to support /api/{route} routing
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseOpenApi();
            app.UseSwaggerUi3();
        }
    }
}
