using System.Reflection;
using InfoTrack.Common.Application;
using InfoTrack.Orders.Api.Client.DependencyInjection;
using InfoTrack.Parties.Api.Client.DependencyInjection;
using InfoTrack.Services.Api.Client.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

#pragma warning disable 1591

namespace InfoTrack.Cdd.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            /*
             Adding InfoTrack.Common.Application registers the following:
             * AutoMapper and all classes implementing IMapFrom<>
             * Mediatr
             * FluentValidation - Add an AbstractValidator<T> where T is ICommand or IQuery to implement validation
             * Authorisers - Add an IAuthoriser<T> where T is ICommand or IQuery to implement authorisation
             * Mediatr pipeline logging
             * Mediatr pipeline exception handling
             * 
             Specify a list of assemblies if your Mediatr implementations are spread across multiple for the automatic registration to work
            */
            services.AddCommonApplication(Assembly.GetExecutingAssembly());

            services.AddOrdersApiClient(options => configuration.GetSection("OrdersApi").Bind(options))
                .AddHttpAuthentication();

            services.AddServicesApiClient(options => configuration.GetSection("ServicesApi").Bind(options))
                .AddHttpAuthentication();

            services.AddPartiesApiClient(options => configuration.GetSection("PartiesApi").Bind(options))
                .AddHttpAuthentication();

            return services;
        }

        public static IApplicationBuilder UseApplication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app;
        }
    }
}
