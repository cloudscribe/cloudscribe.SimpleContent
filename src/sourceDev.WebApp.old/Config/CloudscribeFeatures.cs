﻿using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CloudscribeFeatures
    {
        public static IServiceCollection SetupDataStorage(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            //services.AddScoped<cloudscribe.Core.Models.Setup.ISetupTask, cloudscribe.Core.Web.Components.EnsureInitialDataSetupTask>();

            var storage = config["DevOptions:DbPlatform"];
            var efProvider = config["DevOptions:EFProvider"];
            var useMiniProfiler = config.GetValue<bool>("DevOptions:EnableMiniProfiler");

            switch (storage)
            {
                case "NoDb":
                    var useSingletons = true;
                    services.AddCloudscribeCoreNoDbStorage(useSingletons);
                    // only needed if using cloudscribe logging with NoDb storage
                    services.AddCloudscribeLoggingNoDbStorage(config);
                    services.AddNoDbStorageForSimpleContent(useSingletons);

                    if (useMiniProfiler)
                    {
                        //services.AddMiniProfiler();
                    }

                    break;

                case "ef":
                default:

                    if (useMiniProfiler)
                    {
                        //services.AddMiniProfiler()
                        //    .AddEntityFramework();
                    }

                    switch (efProvider)
                    {
                        case "sqlite":
                            var slConnection = config.GetConnectionString("SQLiteEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageSQLite(slConnection);
                            services.AddCloudscribeLoggingEFStorageSQLite(slConnection);
                            services.AddCloudscribeSimpleContentEFStorageSQLite(slConnection);

                            break;

                        case "pgsql-old":
                            var pgConnection = config.GetConnectionString("PostgreSqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeLoggingEFStoragePostgreSql(pgConnection);
                            services.AddCloudscribeSimpleContentEFStoragePostgreSql(pgConnection);

                            break;

                        case "pgsql":
                            var pgsConnection = config.GetConnectionString("PostgreSqlConnectionString");
                            services.AddCloudscribeCorePostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeLoggingPostgreSqlStorage(pgsConnection);
                            services.AddCloudscribeSimpleContentPostgreSqlStorage(pgsConnection);

                            break;

                        case "MySql":
                            var mysqlConnection = config.GetConnectionString("MySqlEntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMySql(mysqlConnection);
                            services.AddCloudscribeLoggingEFStorageMySQL(mysqlConnection);
                            services.AddCloudscribeSimpleContentEFStorageMySQL(mysqlConnection);

                            break;

                        case "MSSQL":
                        default:
                            var connectionString = config.GetConnectionString("EntityFrameworkConnectionString");
                            services.AddCloudscribeCoreEFStorageMSSQL(connectionString);

                            // only needed if using cloudscribe logging with EF storage
                            services.AddCloudscribeLoggingEFStorageMSSQL(connectionString);

                            services.AddCloudscribeSimpleContentEFStorageMSSQL(connectionString);


                            break;
                    }




                    break;
            }

            return services;
        }

        public static IServiceCollection SetupCloudscribeFeatures(
            this IServiceCollection services,
            IConfiguration config
            )
        {
            services.AddCloudscribeLogging();

            //services.AddScoped<cloudscribe.Web.Navigation.Caching.ITreeCache, cloudscribe.Web.Navigation.Caching.NotCachedTreeCache>();

            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.Web.Navigation.NavigationNodePermissionResolver>();
            services.AddScoped<cloudscribe.Web.Navigation.INavigationNodePermissionResolver, cloudscribe.SimpleContent.Web.Services.PagesNavigationNodePermissionResolver>();

            services.AddCloudscribeCoreMvc(config);

            services.Configure<List<ProjectSettings>>(config.GetSection("ContentProjects"));

            services.AddCloudscribeCoreIntegrationForSimpleContent(config);

            // for testing that teasers can be disabled
            //services.AddScoped<ITeaserService, cloudscribe.SimpleContent.Web.Services.TeaserServiceDisabled>();

            services.AddSimpleContentMvc(config);
            services.AddContentTemplatesForSimpleContent(config);

            services.AddMetaWeblogForSimpleContent(config.GetSection("MetaWeblogApiOptions"));

            services.Configure<cloudscribe.FileManager.Web.Models.AutomaticUploadOptions>(config.GetSection("AutomaticUploadOptions"));

            services.AddSimpleContentRssSyndiction();

            //services.AddPwaKit(config);
            //services.AddPwaKitCloudscribeCoreIntegration(config);
            //services.AddPwaKitNavigationIntegration(config);
            //services.AddPwaKitSimpleContentIntegration(config);


            return services;
        }

    }
}
