using cloudscribe.Versioning;
using System;
using System.Reflection;

namespace cloudscribe.SimpleContent.Syndication
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(RssChannelProvider).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("4284486d-6573-4a99-868d-56fec9f6af48"); } }

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
