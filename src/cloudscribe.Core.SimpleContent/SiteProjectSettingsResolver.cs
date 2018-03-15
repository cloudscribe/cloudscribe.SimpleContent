// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-07-11
// Last Modified:           2017-09-24
// 

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent.Integration
{
    public class SiteProjectSettingsResolver : IProjectSettingsResolver
    {
        public SiteProjectSettingsResolver(
            SiteContext currentSite,
            IProjectQueries projectQueries,
            IProjectCommands projectCommands,
            IOptions<ContentSettingsUIConfig> uiOptionsAccessor
            )
        {
            this.currentSite = currentSite;
            this.projectQueries = projectQueries;
            this.projectCommands = projectCommands;
            uiOptions = uiOptionsAccessor.Value;
        }

        private SiteContext currentSite;
        private IProjectQueries projectQueries;
        private IProjectCommands projectCommands;
        private ContentSettingsUIConfig uiOptions;

        public async Task<IProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            var settings = await projectQueries.GetProjectSettings(currentSite.Id.ToString(), cancellationToken).ConfigureAwait(false);

            //ensure existence of settings
            if(settings == null)
            {
                settings = new ProjectSettings();
                settings.Id = currentSite.Id.ToString();
                if(!uiOptions.ShowBlogMenuOptions)
                {
                    settings.AddBlogToPagesTree = false;
                    settings.BlogMenuLinksToNewestPost = false;
                }
                
                await projectCommands.Create(settings.Id, settings, cancellationToken).ConfigureAwait(false);
            }
            
            if (string.IsNullOrEmpty(settings.RecaptchaPublicKey))
            {
                settings.RecaptchaPublicKey = currentSite.RecaptchaPublicKey;
                settings.RecaptchaPrivateKey = currentSite.RecaptchaPrivateKey;
            }

            if (!uiOptions.ShowBlogMenuOptions)
            {
                settings.AddBlogToPagesTree = false;        
            }

            //settings.EmailFromAddress = currentSite.DefaultEmailFromAddress;
            //settings.SmtpPassword = currentSite.SmtpPassword;
            //settings.SmtpPort = currentSite.SmtpPort;
            //settings.SmtpRequiresAuth = currentSite.SmtpRequiresAuth;
            //settings.SmtpServer = currentSite.SmtpServer;
            //settings.SmtpUser = currentSite.SmtpUser;
            //settings.SmtpUseSsl = currentSite.SmtpUseSsl;
            settings.TimeZoneId = currentSite.TimeZoneId;
            
            return settings;
        }

    }
}
