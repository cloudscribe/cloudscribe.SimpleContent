using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL
{
    public class SimpleContentDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SimpleContentDbContext>
    {
        public SimpleContentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleContentDbContext>();
            builder.UseSqlServer("Server=(local);Database=DATABASENAME;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new SimpleContentDbContext(builder.Options);
        }
    }
}
