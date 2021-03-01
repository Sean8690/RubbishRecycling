using System.Linq;
using System.Reflection;
using InfoTrack.Cdd.Api.Middleware;
using InfoTrack.Cdd.Application;
using InfoTrack.Cdd.Infrastructure;
using InfoTrack.Common.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Swashbuckle.AspNetCore.Filters;

#pragma warning disable 1591

namespace InfoTrack.Cdd.Api
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
            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);

            /*
             Adding the InfoTrack.Common.API registers the following:
              * UserService and CoreIdentity
              * Authentication - configured by the AuthOptions:Authority and AuthOptions:Audience values
              * Authorisation Policies - Client = resource owner (user specific tokens) & Admin = client credentials (server to server tokens)
              * Problem Detail maps - exceptions automatically mapped to HTTP status codes and common response format
              * CORS - Specify origin, headers, and methods
              * Options for rancher prefixes
              * 
            */
            services.AddCommonApi(Configuration, HostEnvironment);

            // Enable exposing health checks via a controller, so that they can appear in swagger documentation
            // (This is not necessary, it's just nice-to-have for all endpoints to appear in the swagger documentation. If this causes any unexpected problems, it can just be removed.)
            services
                .AddSingleton(s => new HealthCheckOptions
                {
                    Predicate = _ => true, // allow all health checks
                    AllowCachingResponses = false,
                });

            ConfigureOpenApiServices(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApplication(env);
            app.ApplyOAuthInjection();
            app.UseInfrastructure(env);
            /*
             Configuring InfoTrack.Common.API enables the following:
             * Problem Details
             * HTTPS
             * Authentication
             * Authorisation
             * SwaggerUI
             * Healthchecks (at /health endpoint)
            */
            app.ConfigureCommonApi(env);

            app.UsePathBase("/service/cdd/api");
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
        private static string SwaggerDocumentName = $"InfoTrack.Cdd.Api.{SwaggerVersion}";

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
                configure.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
                configure.AddSecurity(
                    "JWT Token",
                    Enumerable.Empty<string>(),
                    new NSwag.OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Copy this into the value field: Bearer {token}",
                    });
            });

            // The default System.Text.Json.Serialization cannot serialize Enums to strings and causes doc generation to fail.
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerExamples();
            services.AddControllers().AddNewtonsoftJson();
        }

        private static void ConfigureOpenApi(IApplicationBuilder app)
        {
            app.UseOpenApi(/*configure => configure.PostProcess = (document, _) => document.Schemes = new []
            {    
                // Forcing HTTPS in swagger, doesn't work locally until we setup HTTPS, will do it later
                OpenApiSchema.Https
            }*/);
            //app.UseSwaggerUi3();
            //app.UseReDoc(settings =>
            //{
            //    settings.Path = "/ReDoc";
            //});

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/service/cdd/api/swagger/{SwaggerDocumentName}/swagger.json", SwaggerDocumentName);
            });
        }
    }
}
