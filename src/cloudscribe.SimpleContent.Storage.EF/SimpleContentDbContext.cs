// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-08-31
// Last Modified:			2016-09-04
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace cloudscribe.SimpleContent.Storage.EF
{
    public class SimpleContentDbContext : DbContext
    {
        public SimpleContentDbContext(DbContextOptions<SimpleContentDbContext> options):base(options)
        {

        }

        public DbSet<ProjectSettings> Projects { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Page> Pages { get; set; }

        public DbSet<Comment> Comments { get; set; }

       // public DbSet<Tag> Tags { get; set; }

        public DbSet<TagItem> TagItems { get; set; }

        //public DbSet<PageComment> PageComments { get; set; }

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

            modelBuilder.Entity<Post>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<Page>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                mapper.Map(entity);
            });

            modelBuilder.Entity<TagItem>(entity =>
            {
                mapper.Map(entity);
            });

            // should this be called before or after we do our thing?

            base.OnModelCreating(modelBuilder);
        }

    }
}
