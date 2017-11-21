using cloudscribe.SimpleContent.Models;
using NoDb;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PageStoragePathResolver : IStoragePathResolver<Page>
    {
        public PageStoragePathResolver(IStoragePathOptionsResolver storageOptionsResolver)
        {
            _optionsResolver = storageOptionsResolver;
        }

        private IStoragePathOptionsResolver _optionsResolver;

        public async Task<string> ResolvePath(
            string projectId,
            string key = "",
            string fileExtension = ".json",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(projectId)) throw new ArgumentException("projectId must be provided");

            var pathOptions = await _optionsResolver.Resolve(projectId).ConfigureAwait(false);

            var firstFolderPath = pathOptions.AppRootFolderPath
                + pathOptions.BaseFolderVPath.Replace("/", pathOptions.FolderSeparator);

            if (ensureFoldersExist && !Directory.Exists(firstFolderPath))
            {
                Directory.CreateDirectory(firstFolderPath);
            }
            
            var projectsFolderPath = Path.Combine(firstFolderPath, pathOptions.ProjectsFolderName);

            if (ensureFoldersExist && !Directory.Exists(projectsFolderPath))
            {
                Directory.CreateDirectory(projectsFolderPath);
            }

           
            var projectIdFolderPath = Path.Combine(projectsFolderPath, projectId);

            if (ensureFoldersExist && !Directory.Exists(projectIdFolderPath))
            {
                Directory.CreateDirectory(projectIdFolderPath);
            }

            var type = typeof(Page).Name.ToLowerInvariant();

           

            var typeFolderPath = Path.Combine(projectIdFolderPath, type.ToLowerInvariant().Trim());

            if (ensureFoldersExist && !Directory.Exists(typeFolderPath))
            {
                Directory.CreateDirectory(typeFolderPath);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return typeFolderPath + pathOptions.FolderSeparator;
            }

            var filePath = Path.Combine(typeFolderPath, key + ".md");
            if (File.Exists(filePath)) return filePath;

            return Path.Combine(typeFolderPath, key + fileExtension);
        }


        
        public async Task<string> ResolvePath(
            string projectId,
            string key,
            Page page,
            string fileExtension = ".json",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            
            if(page.Content == "markdown")
            {
                fileExtension = ".md";
            }

            return await ResolvePath(
                projectId,
                key,
                fileExtension,
                ensureFoldersExist,
                cancellationToken).ConfigureAwait(false);

        }

    }
}
