using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteNavigationCacheKeys : IPageNavigationCacheKeys
    {
        public SiteNavigationCacheKeys(SiteContext currentSite)
        {
            this.currentSite = currentSite;
        }

        private SiteContext currentSite;

        public string PageTreeCacheKey
        {
            get { return "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder" + currentSite.Id.ToString(); }
        }

        public string XmlTreeCacheKey
        {
            get { return "cloudscribe.Web.Navigation.XmlNavigationTreeBuilder" + currentSite.Id.ToString(); }
        }

        public string JsonTreeCacheKey
        {
            get { return "cloudscribe.Web.Navigation.JsonNavigationTreeBuilder" + currentSite.Id.ToString(); }
        }

    }
}
