using cloudscribe.PwaKit.Interfaces;

namespace cloudscribe.PwaKit.Services
{
    public class DefaultServiceWorkerRouteNameProvider : IServiceWorkerRouteNameProvider
    {
        public string GetRouteName()
        {
            return "serviceworker";
        }

        public string GetScope()
        {
            return "./";
        }
    }
}
