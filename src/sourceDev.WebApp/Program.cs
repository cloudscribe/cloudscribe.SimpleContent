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
            var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var env = host.Services.GetRequiredService<IHostingEnvironment>();
            var config = host.Services.GetRequiredService<IConfiguration>();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                ConfigureLogging(env, loggerFactory, services);

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

        private static void ConfigureLogging(
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
            LogLevel minimumLevel;
            if (env.IsProduction())
            {
                minimumLevel = LogLevel.Warning;
            }
            else
            {
                minimumLevel = LogLevel.Information;
            }

            var logRepo = serviceProvider.GetService<cloudscribe.Logging.Web.ILogRepository>();

            // a customizable filter for logging
            // add exclusions to remove noise in the logs
            var excludedLoggers = new List<string>
            {
                "Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware",
                "Microsoft.AspNetCore.Hosting.Internal.WebHost",
            };

            Func<string, LogLevel, bool> logFilter = (string loggerName, LogLevel logLevel) =>
            {
                if (logLevel < minimumLevel)
                {
                    return false;
                }

                if (excludedLoggers.Contains(loggerName))
                {
                    return false;
                }

                return true;
            };

            loggerFactory.AddDbLogger(serviceProvider, logFilter, logRepo);
        }

    }
}
