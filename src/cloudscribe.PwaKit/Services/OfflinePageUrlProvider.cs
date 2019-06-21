using cloudscribe.PwaKit.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace cloudscribe.PwaKit.Services
{
    public class OfflinePageUrlProvider : IOfflinePageUrlProvider
    {
        public OfflinePageUrlProvider(
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IPwaRouteNameProvider pwaRouteNameProvider
            )
        {
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _pwaRouteNameProvider = pwaRouteNameProvider;
        }

        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccesor;
        private readonly IPwaRouteNameProvider _pwaRouteNameProvider;

        public string GetOfflineUrl()
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            return urlHelper.RouteUrl(_pwaRouteNameProvider.GetOfflinePageRouteName());

        }


    }
}
