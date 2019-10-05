using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataProtection
    {
        public static IServiceCollection SetupDataProtection(
            this IServiceCollection services,
            IConfiguration config,
            IWebHostEnvironment environment
            )
        {
            string pathToCryptoKeys = Path.Combine(environment.ContentRootPath, "dp_keys");
            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(pathToCryptoKeys));

            return services;
        }

    }
}
