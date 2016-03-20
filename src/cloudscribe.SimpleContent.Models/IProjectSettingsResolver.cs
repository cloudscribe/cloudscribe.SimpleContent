
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectSettingsResolver
    {

        Task<ProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken);
    }
}
