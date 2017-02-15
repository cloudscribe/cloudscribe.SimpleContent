using cloudscribe.Core.Models;
using cloudscribe.FileManager.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.CoreIntegration
{
    public class MediaRootPathResolver : IMediaRootPathResolver
    {
        public MediaRootPathResolver(
            IHostingEnvironment environment,
            SiteContext currentSite
            )
        {
            hosting = environment;
            this.currentSite = currentSite;
        }

        private IHostingEnvironment hosting;
        private SiteContext currentSite;

        public Task<MediaRootPathInfo> Resolve(CancellationToken cancellationToken = default(CancellationToken))
        {
            var virtualPath = "/" + currentSite.AliasId;
            var fsPath = Path.Combine(hosting.WebRootPath, currentSite.AliasId);
            var result = new MediaRootPathInfo(virtualPath, fsPath);
            return Task.FromResult(result);

        }


    }
}
