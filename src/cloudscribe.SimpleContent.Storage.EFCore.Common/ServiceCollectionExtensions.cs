// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-02
// Last Modified:			2016-11-09
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace cloudscribe.SimpleContent.Storage.EFCore.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribeSimpleContentEFStorageCommon(this IServiceCollection services)
        {
            
            services.TryAddScoped<ISimpleContentTableNames, SimpleContentTableNames>();

            services.TryAddScoped<IPageQueries, PageQueries>();
            services.TryAddScoped<IPageCommands, PageCommands>();

            services.TryAddScoped<IPostQueries, PostQueries>();
            services.TryAddScoped<IPostCommands, PostCommands>();

            services.TryAddScoped<IProjectQueries, ProjectQueries>();
            services.TryAddScoped<IProjectCommands, ProjectCommands>();
            

            return services;
        }

        

    }
}
