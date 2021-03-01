
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
using Swashbuckle.AspNetCore.Filters;

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

            services.AddControllers();

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

            app.UsePathBase("/api");
            app.UseRouting(); // required to support /service/cdd/api/{route} routing
            app.UseEndpoints(endpoints => // required to support /service/cdd/api/{route} routing
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHttpsRedirection();
            }

            ConfigureOpenApi(app);
        }

        private static string SwaggerTitle = "InfoTrack CDD API";
        private static string SwaggerVersion => $"v{Assembly.GetExecutingAssembly().GetName().Version}";
        private static string SwaggerDescription = "Customer Due Diligence (CDD) API";
        private static string SwaggerDocumentName = $"InfoTrack.RubbishRecyclingAU.Api.{SwaggerVersion}";

        private static void ConfigureOpenApiServices(IServiceCollection services)
        {
            /*
             Adds a default swagger document using the NSwag package. Options to specify titles, descriptions, and versions.
            */
            //services.AddCommonSwagger(title, description, versionStr);

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = SwaggerTitle;
                configure.Description = SwaggerDescription;
                configure.Version = SwaggerVersion;
                configure.DocumentName = SwaggerDocumentName;
            });

            // The default System.Text.Json.Serialization cannot serialize Enums to strings and causes doc generation to fail.
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamples();
            services.AddControllers().AddNewtonsoftJson();

            ConfigureOpenApiServices(services);
        }

        private static void ConfigureOpenApi(IApplicationBuilder app)
        {
            app.UseOpenApi();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/api/swagger/{SwaggerDocumentName}/swagger.json", SwaggerDocumentName);
            });
        }
    }
}
