using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class StorageInfo : IStorageInfo
    {
        public string StoragePlatform { get { return "NoDb file system storage"; } }
    }
}
