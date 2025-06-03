using cloudscribe.Core.SimpleContent.Integration;
using cloudscribe.Versioning;
using cloudscribe.Web.Common;
using System;
using System.Reflection;

namespace cloudscribe.Core.SimpleContent
{
    public class VersionProvider : IVersionProvider
    {
        private Assembly assembly = typeof(AuthorNameResolver).Assembly;

        public string Name
        {
            get { return assembly.GetName().Name; }

        }

        public Guid ApplicationId { get { return new Guid("f94177b4-919d-4910-acd1-4b3b1c210ecf"); } }

        public Version CurrentVersion
        {

            get
            {

                var version = new Version(2, 0, 0, 0);
                var versionString = typeof(CloudscribeCommonResources).Assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    Version.TryParse(versionString, out version);
                }

                return version;
            }
        }
    }
}