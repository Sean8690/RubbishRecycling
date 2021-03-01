using System;
using System.IO;
using InfoTrack.Common.Api.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

#pragma warning disable 1591

namespace RubbishRecyclingAU
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly, check the application's WebHost configuration.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            /*
             UseSerilog registers Serilog using the application configuration settings enriched with exception details, application names, and log context.
             Optionally, add configuration "DatadogApiKey" to enable Datadog logging
            */
            return Host.CreateDefaultBuilder(args)
                .UseSerilog(SerilogConfiguration.ConfigureLogger)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseSerilog()
                        .UseKestrel()
                        .UseUrls("http://*:5000")
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            var env = builderContext.HostingEnvironment;

                            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables();
                        });
                });
        }
    }
}
