using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface IWorkboxCacheSuffixProvider
    {
        Task<string> GetWorkboxCacheSuffix();
    }
}
