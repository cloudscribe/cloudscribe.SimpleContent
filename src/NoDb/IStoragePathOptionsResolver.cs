using System.Threading.Tasks;

namespace NoDb
{
    public interface IStoragePathOptionsResolver
    {
        Task<StoragePathOptions> Resolve(string projectId);
    }
}
