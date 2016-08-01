// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-07-11
// Last Modified:           2016-07-11
// 

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteProjectSettingsResolver : IProjectSettingsResolver
    {
        public SiteProjectSettingsResolver(
            SiteSettings currentSite,
            IProjectQueries projectSettingsRepository
            )
        {
            this.currentSite = currentSite;
            projectRepo = projectSettingsRepository;
        }

        private SiteSettings currentSite;
        private IProjectQueries projectRepo;

        public async Task<ProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            var settings = await projectRepo.GetProjectSettings(currentSite.AliasId, cancellationToken).ConfigureAwait(false);
            if (settings != null)
            {
                if (string.IsNullOrEmpty(settings.RecaptchaPublicKey))
                {
                    settings.RecaptchaPublicKey = currentSite.RecaptchaPublicKey;
                    settings.RecaptchaPrivateKey = currentSite.RecaptchaPrivateKey;
                }

                settings.EmailFromAddress = currentSite.DefaultEmailFromAddress;
                settings.SmtpPassword = currentSite.SmtpPassword;
                settings.SmtpPort = currentSite.SmtpPort;
                settings.SmtpRequiresAuth = currentSite.SmtpRequiresAuth;
                settings.SmtpServer = currentSite.SmtpServer;
                settings.SmtpUser = currentSite.SmtpUser;
                settings.SmtpUseSsl = currentSite.SmtpUseSsl;
                settings.TimeZoneId = currentSite.TimeZoneId;
            }

            return settings;
        }

    }
}
