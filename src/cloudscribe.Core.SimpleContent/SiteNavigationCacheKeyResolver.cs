using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.Navigation.Caching;
using System.Threading.Tasks;

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

        public Task<string> GetCacheKey(INavigationTreeBuilder builder)
        {
            if(_cultureHelper.UseCultureRoutesAndProjects() && !_cultureHelper.IsDefaultCulture())
            {
                return Task.FromResult(builder.Name + _currentSite.Id.ToString() + _cultureHelper.CurrentUICultureName());
            }

            return Task.FromResult(builder.Name + _currentSite.Id.ToString());
        }

    }
}
