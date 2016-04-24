// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-24
// Last Modified:           2016-04-24
// 

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NoDb
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddNoDb<TObject>(this IServiceCollection services) where TObject : class
        {
            services.TryAddScoped<IStoragePathOptionsResolver, DefaultStoragePathOptionsResolver>();
            services.TryAddScoped<IStoragePathResolver<TObject>, DefaultStoragePathResolver<TObject>>();

            return services;
        }
    }
}
