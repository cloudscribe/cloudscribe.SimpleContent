namespace cloudscribe.SimpleContent.Models
{
    public interface IPageNavigationCacheKeys
    {
        string PageTreeCacheKey { get; }
        string XmlTreeCacheKey { get; }
        string JsonTreeCacheKey { get; }
    }

    public class PageNavigationCacheKeys : IPageNavigationCacheKeys
    {
        public string PageTreeCacheKey
        {
            get { return "cloudscribe.SimpleContent.Services.PagesNavigationTreeBuilder"; }
        } 

        public string XmlTreeCacheKey
        {
            get { return "cloudscribe.Web.Navigation.XmlNavigationTreeBuilder"; }
        }

        public string JsonTreeCacheKey
        {
            get { return "cloudscribe.Web.Navigation.JsonNavigationTreeBuilder"; }
        } 
    }

}
