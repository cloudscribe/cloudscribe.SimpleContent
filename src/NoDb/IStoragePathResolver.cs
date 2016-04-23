

namespace NoDb
{
    public interface IStoragePathResolver
    {
        string ResolvePath(StoragePathOptions pathOptions, string type, string key = "", bool ensureFoldersExist = false);
    }
}
