// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-02
// Last Modified:			2016-09-02
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.SimpleContent.Storage.EFCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {

        public static IServiceCollection AddCloudscribeCoreEFStorage(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<SimpleContentDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );

            //services.AddDbContext<CoreDbContext>(options =>
            //{
            //    options.UseSqlServer(connectionString) ;
            //});

            services.AddScoped<ISimpleContentModelMapper, SqlServerSimpleContentModelMapper>();
            
            //services.AddScoped<ISiteCommands, SiteCommands>();
            //services.AddScoped<ISiteQueries, SiteQueries>();

            //services.AddScoped<IUserCommands, UserCommands>();
            //services.AddScoped<IUserQueries, UserQueries>();

            //services.AddScoped<IGeoCommands, GeoCommands>();
            //services.AddScoped<IGeoQueries, GeoQueries>();

            services.TryAddScoped<SimpleContentTableNames, SimpleContentTableNames>();

            return services;
        }

    }
}
