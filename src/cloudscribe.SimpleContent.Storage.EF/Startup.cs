// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-09-03
// Last Modified:			2016-09-03
// 

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace cloudscribe.SimpleContent.Storage.EF
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");

            // this file name is ignored by gitignore
            // so you can create it and use on your local dev machine
            // remember last config source added wins if it has the same settings
            builder.AddJsonFile("appsettings.local.overrides.json", optional: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<ISimpleContentModelMapper, SqlServerSimpleContentModelMapper>();

            services.AddEntityFrameworkSqlServer()
              .AddDbContext<SimpleContentDbContext>((serviceProvider, options) =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                       .UseInternalServiceProvider(serviceProvider)
                       );

            //services.AddDbContext<CoreDbContext>(options =>
            //        options.UseSqlServer(Configuration["Data:EF7ConnectionOptions:ConnectionString"])
            //        );
        }


        public void Configure(IApplicationBuilder app)
        {
        }

    }
}
