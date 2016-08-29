// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-07-11
// Last Modified:           2016-08-29
// 

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteProjectSettingsResolver : IProjectSettingsResolver
    {
        public SiteProjectSettingsResolver(
            SiteSettings currentSite,
            MediaFolderHelper folderHelper,
            IProjectQueries projectQueries,
            IProjectCommands projectCommands
            )
        {
            this.currentSite = currentSite;
            this.projectQueries = projectQueries;
            this.projectCommands = projectCommands;
            this.folderHelper = folderHelper;
        }

        private SiteSettings currentSite;
        private MediaFolderHelper folderHelper;
        private IProjectQueries projectQueries;
        private IProjectCommands projectCommands;

        public async Task<ProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            var settings = await projectQueries.GetProjectSettings(currentSite.Id.ToString(), cancellationToken).ConfigureAwait(false);

            //ensure existence of settings
            if(settings == null)
            {
                settings = new ProjectSettings();
                settings.ProjectId = currentSite.Id.ToString();
                if(!string.IsNullOrEmpty(currentSite.AliasId))
                {
                    settings.LocalMediaVirtualPath = "/" + currentSite.AliasId + "/media/images/";
                    // need to create the folder path below wwwroot
                    var segments = new string[3] { currentSite.AliasId, "media", "images" };
                    folderHelper.EnsureMediaFolderExists(segments);
                }
                
                await projectCommands.Create(settings.ProjectId, settings, cancellationToken).ConfigureAwait(false);
            }
            
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
            
            return settings;
        }

    }
}
