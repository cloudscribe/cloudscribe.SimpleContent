// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-23
// Last Modified:           2016-02-23
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonPageStorage(this IServiceCollection services)
        {
            services.TryAddScoped<IPageRepository, JsonPageRepository>();
            services.TryAddScoped<ProjectFilePathResolver, ProjectFilePathResolver>();
            services.TryAddScoped<IJsonPersister, JsonFileSystemPersister>();
            services.TryAddScoped<IJsonFileSystemOptionsResolver, DefaultJsonFileSystemOptionsResolver>();

            return services;
        }
    }
}
