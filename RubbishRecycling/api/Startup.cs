
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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

            services.AddControllers().AddNewtonsoftJson();

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

            services.AddHealthChecks();
            services
                    .AddSingleton(s => new HealthCheckOptions
                    {
                        Predicate = _ => true, // allow all health checks
                        AllowCachingResponses = false,
                    });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<RubbishRecyclingContext>();

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
