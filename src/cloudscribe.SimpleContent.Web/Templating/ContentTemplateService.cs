// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-21
// Last Modified:           2018-06-21
// 

using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            IEnumerable<IModelSerializer> serializers,
            ILogger<ContentTemplateService> logger
             )
        {
            _templateProvider = templateProvider;
            _pageService = pageService;
            _serializers = serializers;
            _log = logger;
        }

        private readonly IContentTemplateProvider _templateProvider;
        private readonly IPageService _pageService;
        private readonly IEnumerable<IModelSerializer> _serializers;
        private readonly ILogger _log;

        private IModelSerializer GetSerializer(string name)
        {
            foreach (var s in _serializers)
            {
                if (s.Name == name) return s;
            }

            return _serializers.FirstOrDefault();
        }

        public object DesrializeTemplateModel(IPage page, ContentTemplate template)
        {
            string modelString;
            if(!string.IsNullOrWhiteSpace(page.DraftSerializedModel))
            {
                modelString = page.DraftSerializedModel;
            }
            else
            {
                modelString = page.SerializedModel;
            }
            if(string.IsNullOrWhiteSpace(modelString))
            {
                _log.LogError($"could not deserialize model from empty string on page {page.Title}");
                return null;
            }
            var serializer = GetSerializer(template.SerializerName);

            try
            {
                return serializer.Deserialize(template.ModelType, modelString);
            }
            catch(Exception ex)
            {
                _log.LogError($"Failed to deserialize model for page {page.Title} returning null. Exception was {ex.Message}:{ex.StackTrace}");
                return null;
            }
            
            
        }


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
