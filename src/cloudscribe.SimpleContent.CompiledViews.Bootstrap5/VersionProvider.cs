using cloudscribe.Versioning;
using cloudscribe.Web.Common;
using System;
using System.Reflection;

namespace cloudscribe.SimpleContent.CompiledViews.Bootstrap5
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.SimpleContent.CompiledViews.Bootstrap5"; } }

        public Guid ApplicationId { get { return new Guid("f94067b4-919d-4910-acd1-4b3b1c210ecf"); } }

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