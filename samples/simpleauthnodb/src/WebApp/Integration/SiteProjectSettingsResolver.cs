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
            IProjectSettingsRepository projectSettingsRepository)
        {
            this.currentTenant = currentTenant;
            projectRepo = projectSettingsRepository;
        }

        private SiteSettings currentTenant;
        private IProjectSettingsRepository projectRepo;

        public async Task<ProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            var settings = await projectRepo.GetProjectSettings(currentTenant.ContentProjectId, cancellationToken).ConfigureAwait(false);
            if ((settings != null) && (string.IsNullOrEmpty(settings.RecaptchaPublicKey)))
            {
                settings.RecaptchaPublicKey = currentTenant.RecaptchaPublicKey;
                settings.RecaptchaPrivateKey = currentTenant.RecaptchaPrivateKey;
            }

            return settings;
        }
    }
}
