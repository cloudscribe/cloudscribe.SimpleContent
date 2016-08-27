using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp
{
    public class SiteProjectSettingsResolver : IProjectSettingsResolver
    {
        public SiteProjectSettingsResolver(
            SiteSettings currentTenant,
            IProjectQueries projectQueries
  
            )
        {
            this.currentTenant = currentTenant;
            this.projectQueries = projectQueries;
            
        }

        private SiteSettings currentTenant;
        private IProjectQueries projectQueries;
        //private IProjectCommands projectCommands;

        public async Task<ProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            var settings = await projectQueries.GetProjectSettings(currentTenant.ContentProjectId, cancellationToken).ConfigureAwait(false);
            if ((settings != null) && (string.IsNullOrEmpty(settings.RecaptchaPublicKey)))
            {
                settings.RecaptchaPublicKey = currentTenant.RecaptchaPublicKey;
                settings.RecaptchaPrivateKey = currentTenant.RecaptchaPrivateKey;
            }

            return settings;
        }
    }
}
