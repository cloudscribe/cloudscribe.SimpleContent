// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-21
// Last Modified:           2018-06-21
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class ContentTemplateService
    {
        public ContentTemplateService(
            IContentTemplateProvider templateProvider,
            IPageService pageService,
            ILogger<ContentTemplateService> logger
             )
        {
            _templateProvider = templateProvider;
            _pageService = pageService;
            _log = logger;
        }

        private readonly IContentTemplateProvider _templateProvider;
        private readonly IPageService _pageService;
        private readonly ILogger _log;

        public async Task<List<ContentTemplate>> GetAllTemplates(string projectId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _templateProvider.GetAllTemplates(projectId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<ContentTemplate> GetTemplate(string projectId, string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _templateProvider.GetTemplate(projectId, key, cancellationToken).ConfigureAwait(false);
        }
    }
}
