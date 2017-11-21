using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public interface IKeyGenerator
    {
        string GenerateKey(IContentItem item);
    }
}
