using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class DefaultTimeZoneIdResolver : ITimeZoneIdResolver
    {
        public DefaultTimeZoneIdResolver(
            IProjectSettingsResolver projectSettingsResolver
            )
        {
            _projectSettingsResolver = projectSettingsResolver;
        }

        private IProjectSettingsResolver _projectSettingsResolver;

        public async Task<string> GetUserTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
        {
            var project = await _projectSettingsResolver.GetCurrentProjectSettings(cancellationToken);
            return project.TimeZoneId;

        }

        public async Task<string> GetSiteTimeZoneId(CancellationToken cancellationToken = default(CancellationToken))
        {
            var project = await _projectSettingsResolver.GetCurrentProjectSettings(cancellationToken);
            return project.TimeZoneId;
        }

    }
}
