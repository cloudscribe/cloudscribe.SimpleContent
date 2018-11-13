using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.SimpleContent.MetaWeblog
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(MetaWeblogService).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("a4eb54de-39e9-4af6-ab55-1077db6f2ce3"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}
