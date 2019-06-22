using cloudscribe.Web.Navigation;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Integration.Navigation
{
    public interface INavigationNodeServiceWorkerFilter
    {
        Task<bool> ShouldRenderNode(NavigationNode node);
    }
}
