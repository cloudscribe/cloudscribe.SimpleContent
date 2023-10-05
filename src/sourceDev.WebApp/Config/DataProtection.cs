using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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

            // **** VERY IMPORTANT *****
            // https://www.cloudscribe.com/docs/configuring-data-protection
            // data protection keys are used to encrypt the auth token in the cookie
            // and also to encrypt social auth secrets and smtp password in the data storage
            // therefore we need keys to be persistent in order to be able to decrypt
            // if you move an app to different hosting and the keys change then you would have
            // to update those settings again from the Administration UI

            // for IIS hosting you should use a powershell script to create a keyring in the registry
            // per application pool and use a different application pool per app
            // https://docs.microsoft.com/en-us/aspnet/core/publishing/iis#data-protection
            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?tabs=aspnetcore2x
            if (environment.IsProduction())
            {
                // If you are hosted in Azure you should add the package Microsoft.AspNetCore.DataProtection.AzureStorage to the .csproj file
                // and uncomment/configure the code below as per documentation here: 
                // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-2.1&tabs=aspnetcore2x

                // If using Azure for production the uri with sas token could be stored in azure as environment variable or using key vault
                // but the keys go in azure blob storage per docs https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers
                // this is false by default you should set it to true in azure environment variables
                //var useBlobStorageForDataProtection = config.GetValue<bool>("AppSettings:UseAzureBlobForDataProtection");
                // best to put this in azure environment variables instead of appsettings.json
                //var storageConnectionString = config["AppSettings:DataProtectionBlobStorageConnectionString"];
                // if (useBlobStorageForDataProtection && !string.IsNullOrWhiteSpace(storageConnectionString))
                //{
                //    var storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(storageConnectionString);
                //    var client = storageAccount.CreateCloudBlobClient();
                //    var container = client.GetContainerReference("key-container");
                   // The container must exist before calling the DataProtection APIs.
                    // The specific file within the container does not have to exist,
                    // as it will be created on-demand.
                //    container.CreateIfNotExistsAsync().GetAwaiter().GetResult();
                //    services.AddDataProtection()
                //        .PersistKeysToAzureBlobStorage(container, "keys.xml")
                //        .ProtectKeysWithAzureKeyVault("<keyIdentifier>", "<clientId>", "<clientSecret>")
                //        ;

                // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-2.2
                //latest docs show it like this:
                //services.AddDataProtection()
                //        .PersistKeysToAzureBlobStorage(new Uri("<blobUriWithSasToken>"))
                //        .ProtectKeysWithAzureKeyVault("<keyIdentifier>", "<clientId>", "<clientSecret>");

                //}
                //else
                //{
                    services.AddDataProtection();
                //}
            }
            else
            {
                // dp_Keys folder should be added to .gitignore so the keys don't go into source control
                // ie add a line with: **/dp_keys/**
                // to your .gitignore file
                string pathToCryptoKeys = Path.Combine(environment.ContentRootPath, "dp_keys");
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(pathToCryptoKeys))
                    ;
            }

            return services;
        }

    }
}
