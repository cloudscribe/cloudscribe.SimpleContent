using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.SimpleContent.Storage.EFCore.SQLite
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<SimpleContentDbContext>
    {
        public SimpleContentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleContentDbContext>();
            builder.UseSqlite("Data Source=cloudscribe.db");
            return new SimpleContentDbContext(builder.Options);
        }

    }
}
