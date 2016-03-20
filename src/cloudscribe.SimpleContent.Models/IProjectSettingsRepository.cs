


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectSettingsRepository
    {
        Task<ProjectSettings> GetProjectSettings(
            string blogId,
            CancellationToken cancellationToken
            );

        Task<List<ProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken
            );
    }
}
