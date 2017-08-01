using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace cloudscribe.SimpleContent.Storage.EFCore.MSSQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();

            host.Run();
        }
    }
}
