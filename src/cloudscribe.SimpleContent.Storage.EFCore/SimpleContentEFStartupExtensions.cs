// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-02
// Last Modified:			2016-09-07
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using cloudscribe.SimpleContent.Storage.EFCore;
using cloudscribe.SimpleContent.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleContentEFStartupExtensions
    {

        public static IServiceCollection AddCloudscribeSimpleContentEFStorage(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<SimpleContentDbContext>((serviceProvider, options) =>
                options.UseSqlServer(connectionString)
                       .UseInternalServiceProvider(serviceProvider)
                       );
            
            services.AddScoped<ISimpleContentModelMapper, SqlServerSimpleContentModelMapper>();
            services.TryAddScoped<SimpleContentTableNames, SimpleContentTableNames>();

            services.TryAddScoped<IPageQueries, PageQueries>();
            services.TryAddScoped<IPageCommands, PageCommands>();

            services.TryAddScoped<IPostQueries, PostQueries>();
            services.TryAddScoped<IPostCommands, PostCommands>();

            services.TryAddScoped<IProjectQueries, ProjectQueries>();
            services.TryAddScoped<IProjectCommands, ProjectCommands>();
            

            return services;
        }

        public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
        {

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<SimpleContentDbContext>();

                await db.Database.MigrateAsync();


            }
        }

    }
}
