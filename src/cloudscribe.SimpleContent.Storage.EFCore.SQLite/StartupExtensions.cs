using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Storage.EFCore.Common;
using cloudscribe.SimpleContent.Storage.EFCore.SQLite;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCloudscribeSimpleContentEFStorageSQLite(
            this IServiceCollection services,
            string connectionString
            )
        {
            services.AddDbContext<SimpleContentDbContext>(options =>
                    options.UseSqlite(connectionString));
            
            services.AddScoped<ISimpleContentDbContext, SimpleContentDbContext>();

            services.AddCloudscribeSimpleContentEFStorageCommon();
            services.AddScoped<IStorageInfo, StorageInfo>();

            return services;
        }
    }
}
