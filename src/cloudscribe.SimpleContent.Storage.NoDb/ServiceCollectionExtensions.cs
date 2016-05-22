// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2016-05-21
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.NoDb;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoDbPageStorage(this IServiceCollection services)
        {
            services.AddNoDb<Page>();
            services.TryAddScoped<IPageRepository, NoDbPageRepository>();

            return services;
        }

        public static IServiceCollection AddNoDbPostStorage(this IServiceCollection services)
        {
            services.AddNoDb<Post>();
            services.TryAddScoped<IPostRepository, NoDbPostRepository>();

            return services;
        }

        public static IServiceCollection AddNoDbProjectStorage(this IServiceCollection services)
        {
            services.AddScoped<IProjectSettingsRepository, NoDbProjectRepository>();

            return services;
        }
    }
}
