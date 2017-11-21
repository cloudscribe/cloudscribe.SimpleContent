// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2017-11-20
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.NoDb;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoDbStorageForSimpleContent(this IServiceCollection services)
        {
            services.AddScoped<PostXmlSerializer>();
            services.AddScoped<PostMarkdownSerializer>();

            //services.TryAddScoped<IStringSerializer<Post>, PostXmlSerializer>();
            services.TryAddScoped<IStringSerializer<Post>, CompositePostSerializer>(); 
            services.TryAddScoped<IStoragePathResolver<Post>, PostStoragePathResolver>();
            services.TryAddScoped<IKeyGenerator, DefaultKeyGenerator>();
          
            services.AddNoDbProjectStorage();
            services.AddNoDbPostStorage();
            services.AddNoDbPageStorage();
            services.AddScoped<IStorageInfo, StorageInfo>();

            return services;
        }

        public static IServiceCollection AddNoDbPageStorage(this IServiceCollection services)
        {
            services.AddNoDb<Page>();
            services.TryAddScoped<IPageQueries, PageQueries>();
            services.TryAddScoped<IPageCommands, PageCommands>();

            return services;
        }

        public static IServiceCollection AddNoDbPostStorage(this IServiceCollection services)
        {
            services.AddNoDb<Post>();
            services.TryAddScoped<IPostQueries, PostQueries>();
            services.TryAddScoped<IPostCommands, PostCommands>();
            services.TryAddScoped<PostCache, PostCache>();

            return services;
        }

        public static IServiceCollection AddNoDbProjectStorage(this IServiceCollection services)
        {
            services.AddNoDb<ProjectSettings>();
            services.TryAddScoped<IProjectQueries, ProjectQueries>();
            services.TryAddScoped<IProjectCommands, ProjectCommands>();

            return services;
        }
    }
}
