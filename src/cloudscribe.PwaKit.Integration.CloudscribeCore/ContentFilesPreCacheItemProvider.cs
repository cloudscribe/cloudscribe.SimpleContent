using cloudscribe.FileManager.Web.Models;
using cloudscribe.PwaKit.Interfaces;
using cloudscribe.PwaKit.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Integration.CloudscribeCore
{
    public class ContentFilesPreCacheItemProvider : IPreCacheItemProvider
    {
        public ContentFilesPreCacheItemProvider(
            IMediaPathResolver mediaPathResolver
            )
        {
            _mediaPathResolver = mediaPathResolver;
        }

        private readonly IMediaPathResolver _mediaPathResolver;
        private MediaRootPathInfo _rootPath = null;

        private async Task EnsureProjectSettings()
        {
            if (_rootPath != null) { return; }
            _rootPath = await _mediaPathResolver.Resolve().ConfigureAwait(false);
            if (_rootPath != null) { return; }
        }


        public async Task<List<PreCacheItem>> GetItems()
        {
            await EnsureProjectSettings();

            var result = new List<PreCacheItem>();

            if (!Directory.Exists(_rootPath.RootFileSystemPath))
            {
                return result;
            }

            string supportedExtensions = "*.jpg,*.gif,*.png,*.jpe,*.jpeg";

            var tmp = GetFileList("*", _rootPath.RootFileSystemPath);

            var imgs = tmp.Where(x => supportedExtensions.Contains(Path.GetExtension(x).ToLower()))
                .Select(x =>
                    new PreCacheItem()
                    {
                        Url = x.Replace(_rootPath.RootFileSystemPath, "").Replace(Path.DirectorySeparatorChar, '/')
                    }
                );

            result.AddRange(imgs);
               
            return result;
        }


        private static IEnumerable<string> GetFileList(string fileSearchPattern, string rootFolderPath)
        {
            Queue<string> pending = new Queue<string>();
            pending.Enqueue(rootFolderPath);
            string[] tmp;
            while (pending.Count > 0)
            {
                rootFolderPath = pending.Dequeue();
                try
                {
                    tmp = Directory.GetFiles(rootFolderPath, fileSearchPattern);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                for (int i = 0; i < tmp.Length; i++)
                {
                    yield return tmp[i];
                }
                tmp = Directory.GetDirectories(rootFolderPath);
                for (int i = 0; i < tmp.Length; i++)
                {
                    pending.Enqueue(tmp[i]);
                }
            }
        }



    }
}
