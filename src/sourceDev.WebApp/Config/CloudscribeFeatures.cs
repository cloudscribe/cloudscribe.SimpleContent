using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using cloudscribe.SimpleContent.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {

        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment env
            )
        {
            var storage = config["DevOptions:DbPlatform"].ToLowerInvariant();
            var efProvider = config["DevOptions:EFProvider"].ToLowerInvariant();
            string connectionString;

            switch (storage)
            {
                case "efcore":
                    switch (efProvider.ToLower())
                    {
                        case "mysql":
                            connectionString = config.GetConnectionString("MySqlEntityFrameworkConnection");
                            services.AddCloudscribeCoreEFStorageMySql(connectionString);
                            services.AddCloudscribeLoggingEFStorageMySQL(connectionString);
                            services.AddCloudscribeSimpleContentEFStorageMySQL(connectionString);
                            break;

                        case "pgsql":
                            connectionString = config.GetConnectionString("PostgreSqlEntityFrameworkConnection");
                            services.AddCloudscribeCorePostgreSqlStorage(connectionString);
                            services.AddCloudscribeLoggingPostgreSqlStorage(connectionString);
                            services.AddCloudscribeSimpleContentPostgreSqlStorage(connectionString);
                            break;

                        case "sqlite":
                            var dbName = config.GetConnectionString("SQLiteDbName");
                            var dbPath = Path.Combine(env.ContentRootPath, dbName);
                            connectionString = $"Data Source={dbPath}";
                            services.AddCloudscribeCoreEFStorageSQLite(connectionString);
                            services.AddCloudscribeLoggingEFStorageSQLite(connectionString);
                            services.AddCloudscribeSimpleContentEFStorageSQLite(connectionString);
                            break;

                        case "mssql":
                        default:
                            connectionString = config.GetConnectionString("EntityFrameworkConnection");
                            services.AddCloudscribeCoreEFStorageMSSQL(connectionString);
                            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);
                            services.AddCloudscribeSimpleContentEFStorageMSSQL(connectionString);
                            break;
                    }
                    break;

                case "nodb":
                default:
                    var useSingletons = true;
                    services.AddCloudscribeCoreNoDbStorage(useSingletons);
                    services.AddCloudscribeLoggingNoDbStorage(config);
                    services.AddNoDbStorageForSimpleContent(useSingletons);
                    break;
            }

            return services;
        }

        public static IServiceCollection SetupCloudscribeFeatures(
            this IServiceCollection services,
            IConfiguration config
            )
        {

            services.AddCloudscribeLogging(config);

            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();
            services.AddCloudscribeCoreMvc(config);

            //The template should not have put this here
            // var storage = config["DevOptions:DbPlatform"].ToLowerInvariant();
            // if (storage == "efcore")
            // {
            //     services.AddScoped<IQueryTool, QueryTool>();
            // }

            services.Configure<List<ProjectSettings>>(config.GetSection("ContentProjects"));

            services.AddCloudscribeCoreIntegrationForSimpleContent(config);

            // for testing that teasers can be disabled
            //services.AddScoped<ITeaserService, cloudscribe.SimpleContent.Web.Services.TeaserServiceDisabled>();

            services.AddSimpleContentMvc(config);
            services.AddContentTemplatesForSimpleContent(config);

            services.AddMetaWeblogForSimpleContent(config.GetSection("MetaWeblogApiOptions"));

            services.Configure<cloudscribe.FileManager.Web.Models.AutomaticUploadOptions>(config.GetSection("AutomaticUploadOptions"));

            services.AddSimpleContentRssSyndiction();


            return services;
        }

    }
}
