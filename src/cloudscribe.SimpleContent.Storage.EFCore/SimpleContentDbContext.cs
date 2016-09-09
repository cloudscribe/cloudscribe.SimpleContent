// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-09
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace cloudscribe.SimpleContent.Storage.EFCore
{
    public class SimpleContentDbContext : DbContext
    {
        public SimpleContentDbContext(DbContextOptions<SimpleContentDbContext> options):base(options)
        {

        }

        public DbSet<ProjectSettings> Projects { get; set; }

        public DbSet<PostEntity> Posts { get; set; }

        
        public DbSet<PostComment> Comments { get; set; }

       // public DbSet<Tag> Tags { get; set; }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<PageEntity> Pages { get; set; }

        public DbSet<PageComment> PageComments { get; set; }

        public DbSet<PageCategory> PageCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ISimpleContentModelMapper mapper = this.GetService<ISimpleContentModelMapper>();
            if (mapper == null)
            {
                mapper = new SqlServerSimpleContentModelMapper();
            }


            modelBuilder.Entity<ProjectSettings>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<PostEntity>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<PostComment>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<PostCategory>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<PageEntity>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<PageComment>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<PageCategory>(entity =>
            {
                mapper.Map(entity);
            });



            // should this be called before or after we do our thing?

            base.OnModelCreating(modelBuilder);
        }

    }
}
