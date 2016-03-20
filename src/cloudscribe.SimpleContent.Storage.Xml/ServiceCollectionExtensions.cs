// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-10
// Last Modified:           2016-02-17
// 


using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Xml
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddXmlBlogStorage(this IServiceCollection services)
        {
            services.TryAddScoped<IPostRepository, XmlPostRepository>();
            services.TryAddScoped<ProjectFilePathResolver, ProjectFilePathResolver>();
            services.TryAddScoped<IXmlPersister, XmlFileSystemPersister>();
            services.TryAddScoped<IXmlFileSystemOptionsResolver, DefaultXmlFileSystemOptionsResolver>();
            
            return services;
        }
    }
}
