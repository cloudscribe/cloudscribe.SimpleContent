using cloudscribe.SimpleContent.Storage.EFCore.Common;
using Microsoft.EntityFrameworkCore;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql
{
    public class SimpleContentDbContextFactory : ISimpleContentDbContextFactory
    {
        public SimpleContentDbContextFactory(DbContextOptions<SimpleContentDbContext> options)
        {
            _options = options;
        }

        private readonly DbContextOptions<SimpleContentDbContext> _options;

        public ISimpleContentDbContext CreateContext()
        {
            return new SimpleContentDbContext(_options);
        }
    }
}
