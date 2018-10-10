// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-24
// Last Modified:           2018-07-02
// 


using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.NoDb;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NoDb;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNoDbStorageForSimpleContent(
            this IServiceCollection services,
            bool useSingletons = false
            )
        {
            if(useSingletons)
            {
                services.TryAddSingleton<IKeyGenerator, DefaultKeyGenerator>();
            }
            else
            {
                services.TryAddScoped<IKeyGenerator, DefaultKeyGenerator>();
            }
            
          
            services.AddNoDbProjectStorage(useSingletons);
            services.AddNoDbPostStorage(useSingletons);
            services.AddNoDbPageStorage(useSingletons);
            services.AddScoped<IStorageInfo, StorageInfo>();

            if (useSingletons)
            {
                services.AddNoDbSingleton<ContentHistory>();
                services.TryAddSingleton<IContentHistoryCommandsSingleton, ContentHistoryCommands>();
                services.TryAddSingleton<IContentHistoryQueriesSingleton, ContentHistoryQueries>();
            }
            else
            {
                services.AddNoDb<ContentHistory>();
                
            }

            services.TryAddScoped<IContentHistoryCommands, ContentHistoryCommands>();
            services.TryAddScoped<IContentHistoryQueries, ContentHistoryQueries>();


            return services;
        }

        public static IServiceCollection AddNoDbPageStorage(
            this IServiceCollection services,
            bool useSingletons = false
            )
        {
            if(useSingletons)
            {
                services.AddSingleton<PageJsonSerializer>();
                services.AddSingleton<PageMarkdownSerializer>();

                services.TryAddSingleton<IStringSerializer<Page>, PageCompositeSerializer>();
                services.TryAddSingleton<IStoragePathResolver<Page>, PageStoragePathResolver>();

                services.AddNoDbSingleton<Page>();
                services.TryAddSingleton<IPageQueriesSingleton, PageQueries>();
                services.TryAddSingleton<IPageCommandsSingleton, PageCommands>();
            }
            else
            {
                services.AddScoped<PageJsonSerializer>();
                services.AddScoped<PageMarkdownSerializer>();

                services.TryAddScoped<IStringSerializer<Page>, PageCompositeSerializer>();
                services.TryAddScoped<IStoragePathResolver<Page>, PageStoragePathResolver>();

                services.AddNoDb<Page>();
                
            }

            services.TryAddScoped<IPageQueries, PageQueries>();
            services.TryAddScoped<IPageCommands, PageCommands>();


            return services;
        }

        public static IServiceCollection AddNoDbPostStorage(
            this IServiceCollection services,
            bool useSingletons = false
            )
        {
            if(useSingletons)
            {
                services.AddSingleton<PostXmlSerializer>();
                services.AddSingleton<PostMarkdownSerializer>();
                
                services.TryAddSingleton<IStringSerializer<Post>, PostCompositeSerializer>();
                services.TryAddSingleton<IStoragePathResolver<Post>, PostStoragePathResolver>();

                services.AddNoDbSingleton<Post>();

                services.TryAddSingleton<IPostQueriesSingleton, PostQueries>();
                services.TryAddSingleton<IPostCommandsSingleton, PostCommands>();

                services.TryAddSingleton<PostCache, PostCache>();
            }
            else
            {
                services.AddScoped<PostXmlSerializer>();
                services.AddScoped<PostMarkdownSerializer>();

                //services.TryAddScoped<IStringSerializer<Post>, PostXmlSerializer>();
                services.TryAddScoped<IStringSerializer<Post>, PostCompositeSerializer>();
                services.TryAddScoped<IStoragePathResolver<Post>, PostStoragePathResolver>();

                services.AddNoDb<Post>();
                
                services.TryAddScoped<PostCache, PostCache>();
            }

            services.TryAddScoped<IPostQueries, PostQueries>();
            services.TryAddScoped<IPostCommands, PostCommands>();

            return services;
        }

        public static IServiceCollection AddNoDbProjectStorage(
            this IServiceCollection services,
            bool useSingletons = false
            )
        {
            if (useSingletons)
            {
                services.AddNoDbSingleton<ProjectSettings>();
                services.TryAddSingleton<IProjectQueries, ProjectQueries>();
                services.TryAddSingleton<IProjectCommands, ProjectCommands>();
            }
            else
            {
                services.AddNoDb<ProjectSettings>();
                services.TryAddScoped<IProjectQueries, ProjectQueries>();
                services.TryAddScoped<IProjectCommands, ProjectCommands>();
            }
            

            return services;
        }
    }
}
