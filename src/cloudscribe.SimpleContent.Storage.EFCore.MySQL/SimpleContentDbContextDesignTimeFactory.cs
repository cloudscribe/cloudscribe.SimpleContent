using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL
{
    public class SimpleContentDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SimpleContentDbContext>
    {
        public SimpleContentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleContentDbContext>();
            builder.UseMySql("Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;");

            return new SimpleContentDbContext(builder.Options);
        }
    }
}
