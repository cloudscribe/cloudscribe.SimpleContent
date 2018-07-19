using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore.Common
{
    public interface ISimpleContentDbContext : IDisposable
    {
        DbSet<ProjectSettings> Projects { get; set; }

        DbSet<PostEntity> Posts { get; set; }

        DbSet<PostComment> Comments { get; set; }

        DbSet<PostCategory> PostCategories { get; set; }

        DbSet<PageEntity> Pages { get; set; }

        DbSet<PageComment> PageComments { get; set; }

        DbSet<PageCategory> PageCategories { get; set; }

        DbSet<PageResourceEntity> PageResources { get; set; }

        DbSet<ContentHistory> ContentHistory { get; set; }

        ChangeTracker ChangeTracker { get; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
