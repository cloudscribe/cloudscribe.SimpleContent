using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.SimpleContent.Storage.EFCore.PostgreSql
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<SimpleContentDbContext>
    {
        public SimpleContentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleContentDbContext>();
            builder.UseNpgsql("server=yourservername;UID=yourdatabaseusername;PWD=yourdatabaseuserpassword;database=yourdatabasename");

            return new SimpleContentDbContext(builder.Options);
        }
    }
}
