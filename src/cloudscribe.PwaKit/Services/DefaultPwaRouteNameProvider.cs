using cloudscribe.PwaKit.Interfaces;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultPwaRouteNameProvider : IPwaRouteNameProvider
    {
        public string GetServiceWorkerRouteName()
        {
            return "serviceworker";
        }

        public string GetServiceWorkerScope()
        {
            return "./";
        }

        public string GetOfflinePageRouteName()
        {
            return "offlinepage";
        }
    }
}
