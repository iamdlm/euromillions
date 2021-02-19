using EuromillionsCore.Interfaces;
using EuromillionsCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace EuromillionsCore
{
    class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider provider;

        // Access the built service pipeline
        public IServiceProvider Provider => provider;

        // Access the built configuration
        public IConfiguration Configuration => configuration;

        public Startup()
        {
            // Create service collection

            IServiceCollection services = new ServiceCollection();

            // Build configuration

            BuildConfiguration(services);

            // Register services

            ConfigureServices(services);

            // Build the pipeline

            provider = services.BuildServiceProvider();
        }

        private static void BuildConfiguration(IServiceCollection services)
        {
            // Build config

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .AddJsonFile($"appsettings.{environment}.json", optional: true)
                            .AddEnvironmentVariables()
                            .Build();

            services.AddSingleton<IConfiguration>(configuration);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<INunofcService, NunofcService>();
        }
    }
}