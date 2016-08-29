using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class MediaFolderHelper
    {
        public MediaFolderHelper(IHostingEnvironment env)
        {
            environment = env;
        }

        private IHostingEnvironment environment;

        public void EnsureMediaFolderExists(string[] segments)
        {
            var p = environment.WebRootPath;
            for(int i = 0; i < segments.Length; i++)
            {
                p = Path.Combine(p, segments[i]);
                if(!Directory.Exists(p))
                {
                    Directory.CreateDirectory(p);
                }
            }

        }
    }
}
