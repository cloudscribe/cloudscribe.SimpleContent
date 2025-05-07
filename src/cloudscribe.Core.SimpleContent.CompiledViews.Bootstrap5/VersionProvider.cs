using System;
using System.Reflection;
using cloudscribe.Versioning;
using cloudscribe.Web.Common;

namespace cloudscribe.Core.SimpleContent.CompiledViews.Bootstrap5
{
    public class VersionProvider : IVersionProvider
    {
        public string Name { get { return "cloudscribe.Core.SimpleContent.CompiledViews.Bootstrap5"; } }

        public Guid ApplicationId { get { return new Guid("f93067b4-919d-4910-acd1-4b3b1c210ecf"); } }

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
