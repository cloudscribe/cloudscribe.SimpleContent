// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-20
// Last Modified:           2018-07-10
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class ConfigContentTemplateProvider : IContentTemplateProvider
    {
        public ConfigContentTemplateProvider(IOptions<ContentTemplateConfig> configAccessor)
        {
            _configuredTemplates = configAccessor.Value;
        }

        private ContentTemplateConfig _configuredTemplates;

        public Task<List<ContentTemplate>> GetAllTemplates()
        {
            var result = _configuredTemplates.Templates.ToList();

            return Task.FromResult(result);

        }
        
    }
}
