﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-02
// Last Modified:			2018-10-09
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using cloudscribe.SimpleContent.Storage.EFCore.MySQL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SimpleContentEFMSSQLServiceCollectionExtensions
    {

        public static IServiceCollection AddCloudscribeSimpleContentEFStorageMySQL(
            this IServiceCollection services,
            string connectionString,
            int maxConnectionRetryCount = 0,
            int maxConnectionRetryDelaySeconds = 30,
            ICollection<int> transientSqlErrorNumbersToAdd = null
            )
        {
           
            services.AddEntityFrameworkMySql()
                .AddDbContext<SimpleContentDbContext>(optionsBuilder =>
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), // breaking change in Net5.0
                    mySqlOptionsAction: sqlOptions =>
                    {
                        if (maxConnectionRetryCount > 0)
                        {
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: maxConnectionRetryCount,
                                maxRetryDelay: TimeSpan.FromSeconds(maxConnectionRetryDelaySeconds),
                                errorNumbersToAdd: transientSqlErrorNumbersToAdd);
                        }


                    }),
                     optionsLifetime: ServiceLifetime.Singleton
                    );


            services.AddSingleton<ISimpleContentDbContextFactory, SimpleContentDbContextFactory>();

            services.AddScoped<ISimpleContentDbContext, SimpleContentDbContext>();

            services.AddCloudscribeSimpleContentEFStorageCommon();
            services.AddScoped<IStorageInfo, StorageInfo>();

            return services;
        }

    }
}
