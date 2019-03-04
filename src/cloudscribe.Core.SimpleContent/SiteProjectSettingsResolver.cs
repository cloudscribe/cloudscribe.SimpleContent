// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-07-11
// Last Modified:           2019-03-04
// 

using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
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
            IOptions<ContentSettingsUIConfig> uiOptionsAccessor,
            CultureHelper cultureHelper
            )
        {
            _currentSite = currentSite;
            _projectQueries = projectQueries;
            _projectCommands = projectCommands;
            _uiOptions = uiOptionsAccessor.Value;
            _cultureHelper = cultureHelper;
        }

        private readonly SiteContext _currentSite;
        private readonly IProjectQueries _projectQueries;
        private readonly IProjectCommands _projectCommands;
        private readonly ContentSettingsUIConfig _uiOptions;
        private readonly CultureHelper _cultureHelper;

        public async Task<IProjectSettings> GetCurrentProjectSettings(CancellationToken cancellationToken)
        {
            IProjectSettings settings;
            if (_cultureHelper.UseCultureProjectIds() && !_cultureHelper.IsDefaultCulture())
            {
                var settingsKey = _currentSite.Id.ToString() + "~" + _cultureHelper.CurrentUICultureName();
                settings = await _projectQueries.GetProjectSettings(settingsKey, cancellationToken).ConfigureAwait(false);

            }
            else
            {
                settings = await _projectQueries.GetProjectSettings(_currentSite.Id.ToString(), cancellationToken).ConfigureAwait(false);
            }
            
            //ensure existence of settings
            if(settings == null)
            {
                settings = new ProjectSettings();
                settings.Id = _currentSite.Id.ToString();
                if(!_uiOptions.ShowBlogMenuOptions)
                {
                    settings.AddBlogToPagesTree = false;
                    settings.BlogMenuLinksToNewestPost = false;
                }

                if (_cultureHelper.UseCultureProjectIds() && !_cultureHelper.IsDefaultCulture())
                {
                    settings.Id = settings.Id + "~" + _cultureHelper.CurrentUICultureName();
                }

                await _projectCommands.Create(settings.Id, settings, cancellationToken).ConfigureAwait(false);
            }
            
            if (string.IsNullOrEmpty(settings.RecaptchaPublicKey))
            {
                settings.RecaptchaPublicKey = _currentSite.RecaptchaPublicKey;
                settings.RecaptchaPrivateKey = _currentSite.RecaptchaPrivateKey;
            }

            if (!_uiOptions.ShowBlogMenuOptions)
            {
                settings.AddBlogToPagesTree = false;        
            }

            
            settings.TimeZoneId = _currentSite.TimeZoneId;

            
            
            return settings;
        }

    }
}
