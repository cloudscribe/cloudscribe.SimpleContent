using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace sourceDev.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            var config = host.Services.GetRequiredService<IConfiguration>();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    EnsureDataStorageIsReady(config, services);

                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

        private static void EnsureDataStorageIsReady(IConfiguration config, IServiceProvider services)
        {
            var storage = config["DevOptions:DbPlatform"];
            switch (storage)
            {
                case "NoDb":
                    CoreNoDbStartup.InitializeDataAsync(services).Wait();
                    break;

                case "ef":
                default:

                    CoreEFStartup.InitializeDatabaseAsync(services).Wait();

                    LoggingEFStartup.InitializeDatabaseAsync(services).Wait();

                    SimpleContentEFStartup.InitializeDatabaseAsync(services).Wait();

                    break;
            }
        }

    }
}
