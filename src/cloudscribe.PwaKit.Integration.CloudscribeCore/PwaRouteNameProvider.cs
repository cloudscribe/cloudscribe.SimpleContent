using cloudscribe.Core.Models;
using cloudscribe.PwaKit.Interfaces;
using Microsoft.Extensions.Options;

namespace cloudscribe.PwaKit.Integration.CloudscribeCore
{
    public class PwaRouteNameProvider : IPwaRouteNameProvider
    {
        public PwaRouteNameProvider(
            SiteContext currentSite,
            IOptions<MultiTenantOptions> multiTenantOptionsAccessor
            )
        {
            _currentSite = currentSite;
            _multiTenantOptions = multiTenantOptionsAccessor.Value;
        }

        private readonly SiteContext _currentSite;
        private readonly MultiTenantOptions _multiTenantOptions;


        public string GetServiceWorkerRouteName()
        {
            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                {

                    return "folder-serviceworker";
                }
            }

            return "serviceworker";
        }

        public string GetServiceWorkerScope()
        {
            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                {

                    return "./" + _currentSite.SiteFolderName;
                }
            }

            return "./";
        }

        public string GetOfflinePageRouteName()
        {
            if (_multiTenantOptions.Mode == MultiTenantMode.FolderName)
            {
                if (!string.IsNullOrEmpty(_currentSite.SiteFolderName))
                {

                    return "folder-offlinepage";
                }
            }

            return "offlinepage";
        }



    }
}
