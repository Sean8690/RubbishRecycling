using System;
using InfoTrack.Cdd.Application.Common.Interfaces;
using InfoTrack.Cdd.Infrastructure.Api;
using InfoTrack.Cdd.Infrastructure.Api.Platform;
using InfoTrack.Cdd.Infrastructure.Config;
using InfoTrack.Cdd.Infrastructure.HealthCheck;
using InfoTrack.Cdd.Infrastructure.Service;
using InfoTrack.Cdd.Infrastructure.Templates;
using InfoTrack.Cdd.Infrastructure.Utils;
using InfoTrack.Common.Infrastructure;
using InfoTrack.Storage;
using InfoTrack.Storage.Contracts.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace InfoTrack.Cdd.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Enable this for as-required data seeding locally
            // services.AddSingleton<IDbInitialiser, DbInitialiser>();

            /*
             Adding InfoTrack.Common.Infrastructure registers the following: 
             * IDateTimeProvider implementation (use for DateTimeOffset capable dates and timezone handling) - Add InfoTrackApiOptions:TimeZoneId config
             * UserService implementation
             * HealthChecks - overload accepts application specific health checks
            */
            services.AddCommonInfrastructure(configuration);

            //Health Check
            services.AddHealthChecks()
                .AddCheck<FrankieHealthCheck>("Authority")
                .AddCheck<MongoDbHealthCheck>("Database");

            var frankieApiConfig = configuration.GetSection("FrankieApi").Get<FrankieApiConfig>();
            services.AddSingleton(frankieApiConfig);

            var platformServicesConfig = configuration.GetSection("PlatformServices").Get<PlatformServicesConfig>();
            services.AddSingleton(platformServicesConfig);

            services.AddTransient<IMongoClient>(s => new MongoClient(configuration["Mongo:ConnectionString"]));
            var mongoConfig = configuration.GetSection("Mongo").Get<Mongo>();
            services.AddSingleton(mongoConfig);
            services.AddScoped<IDatabaseService, DatabaseService>();

            services.AddScoped<ITestableOrganisationClient, FrankieClient>();
            services.AddScoped<ITestablePersonClient, FrankieClient>();
            services.AddScoped<IOrganisationService, FrankieGetOrganisationService>();
            services.AddScoped<IPersonsService, FrankieGetPersonsService>();
            services.AddScoped<IServiceFacade, ServiceFacade>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IServicesService, ServiceAndFeeService>();
            services.AddScoped<IQuoteService, ServiceAndFeeService>();
            services.AddScoped<IPartiesService, PartiesService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ITemplateBuilder, TemplateBuilder>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IUserRequestContext, UserRequestContext>();

            services.AddScoped<IStorageApi, InfoTrackStorageApiClient>(s =>
            {
                var authenticationService = s.GetRequiredService<IUserRequestContext>();
                return new InfoTrackStorageApiClient(configuration["PlatformServices:storageServiceUrl"], authenticationService.AccessToken ?? "anonymous");
            });

            In8nHelper.Init(configuration);

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    // Hackily seed the database
            //    var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            //    using var scope = scopeFactory.CreateScope();
            //    var dbInitializer = scope.ServiceProvider.GetService<IDbInitialiser>();
            //    dbInitializer.SeedCountries().GetAwaiter().GetResult();
            //    //dbInitializer.UpdateFlags_20201221().GetAwaiter().GetResult();
            //}

            return app;
        }
    }
}
