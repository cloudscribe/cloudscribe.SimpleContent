using cloudscribe.PwaKit.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cloudscribe.PwaKit.Interfaces
{
    public interface IPreCacheItemProvider
    {
        Task<List<PreCacheItem>> GetItems();
    }
}
