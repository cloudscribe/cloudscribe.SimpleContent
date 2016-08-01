


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectQueries
    {
        Task<ProjectSettings> GetProjectSettings(
            string projectId,
            CancellationToken cancellationToken
            );

        Task<List<ProjectSettings>> GetProjectSettingsByUser(
            string userName,
            CancellationToken cancellationToken
            );
    }
}
