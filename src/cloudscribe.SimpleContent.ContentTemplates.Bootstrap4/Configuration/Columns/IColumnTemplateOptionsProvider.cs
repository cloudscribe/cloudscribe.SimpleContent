using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.ContentTemplates.Configuration
{
    public interface IColumnTemplateOptionsProvider
    {
        Task<ColumnTemplateOptions> ResolveColumnTemplateOptions(
            string hostName,
            string path,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
