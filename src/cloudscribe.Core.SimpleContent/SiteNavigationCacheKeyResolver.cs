using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;

namespace cloudscribe.Core.SimpleContent
{
    public class SiteNavigationCacheKeyResolver : ITreeCacheKeyResolver
    {
        public SiteNavigationCacheKeyResolver(
            SiteContext currentSite,
            CultureHelper cultureHelper
            )
        {
            _currentSite = currentSite;
            _cultureHelper = cultureHelper;
        }

        private readonly SiteContext _currentSite;
        private readonly CultureHelper _cultureHelper;

        public string GetCacheKey(INavigationTreeBuilder builder)
        {
            if(_cultureHelper.UseCultureRoutes() && !_cultureHelper.IsDefaultCulture())
            {
                return builder.Name + _currentSite.Id.ToString() + _cultureHelper.CurrentUICultureName();
            }

            return builder.Name + _currentSite.Id.ToString();
        }

    }
}
