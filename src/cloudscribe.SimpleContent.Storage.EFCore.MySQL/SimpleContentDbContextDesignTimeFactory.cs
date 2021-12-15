using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace cloudscribe.SimpleContent.Storage.EFCore.MySQL
{
    public class SimpleContentDbContextDesignTimeFactory : IDesignTimeDbContextFactory<SimpleContentDbContext>
    {
        public SimpleContentDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SimpleContentDbContext>();
            var conn = "Server=yourserver;Database=yourdb;Uid=youruser;Pwd=yourpassword;Charset=utf8;";
            builder.UseMySql(conn, ServerVersion.AutoDetect(conn)); // breaking change in Net5.0

            return new SimpleContentDbContext(builder.Options);
        }
    }
}
