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
            // Notes to myself - capturing my thoughts 2016-08-02

            // there are 2 "project" concepts that are kind of overlayed on top of each other

            // there is the "Content Project" with settings encapsulated in ProjectSettings class
            // this class is for querying those. -part of SimpleContent

            // there is also the concept in NoDb of a projectid which corresponds to the folder where obejcts are stored on disk
            // in some cases the same projectid may be used for both NoDb and ProjectSettings.ProjectId

            // in SimpleContent the ProjectSettings.ProjectId always corresponds to the NoDb projectid aka folder where
            // posts and pages will be stored

            // the current NoDb implementation for cloudscribe Core puts everything in a NoDb projectid/folder named "default"
            // so the NoDb projectid for cloudscribe core is not the same one used for cloudscribe SimpleContent

            // when using cloudscribe core we also have the concept of an AliasId on the site which is a friendlier id than
            // Id which is a guid

            // with integration between cloudscribe Core and SimpleContent, either the Site.AliasId or the Site.Id guid will be used as the 
            // ProjectSettings.ProjectId
            // if a project

            // I am kind of torn on these ideas, flexible but perhaps error prone if AliasId were to change for example
            // ProjectSettings object will get serialized to the cloudscribe core NoDb project ie "default"
            // but SimpleContent Pages and Posts will ALWAYS use ProjectSettings.ProjectId as the NoDb projectid
            // which means they will not be stored in the same NoDb project folder as cloudscribe Core data (ie default)

            // I had thought about making a NoDb ProjectIdResolver for cloudscribe Core that would enable each site/tenant to have its
            // own NoDb project whereas currently all Sites and related data would go into the "default" project
            // however for this to work we would have to resolve the NoDb projectId before resolving the site
            // in order to be able to look in the correct folder location to resolve the site
            // so we would need some extra master list of sites perhaps in the default NoDb project
            // common data such as Country/State list data would also still live in the default project
            // the advantages of this approach are easy to copy/migrate a specific site/tenant data from one install to another
            // and fewer files to iterate over when looking up info for a specific tenant
            // changing to this approach would also allow unification of the same NoDb projectid for SimpleContent as is used for the 
            // site specific cloudscribe Core data
            // typing this info out has helped me reason about this and I think I will go back nnow and try to implement this in 
            // the cloudscribe Core implementation of NoDb

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
