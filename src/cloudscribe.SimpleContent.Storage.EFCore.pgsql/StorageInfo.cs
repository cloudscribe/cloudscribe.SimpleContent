using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Storage.EFCore.pgsql
{
    public class StorageInfo : IStorageInfo
    {
        public string StoragePlatform { get { return "Entity Framework with PostgreSql"; } }
    }
}
