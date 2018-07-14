// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-06-21
// Last Modified:           2018-07-14
// 

using cloudscribe.Pagination.Models;
using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class ContentTemplateService : IContentTemplateService
    {
        public ContentTemplateService(
            IEnumerable<IContentTemplateProvider> templateProviders,
            IEnumerable<IModelSerializer> serializers,
            ILogger<ContentTemplateService> logger
             )
        {
            _templateProviders = templateProviders;
            _serializers = serializers;
            _log = logger;
            
        }

        private readonly IEnumerable<IContentTemplateProvider> _templateProviders;
        private readonly IEnumerable<IModelSerializer> _serializers;
        private readonly ILogger _log;

        private List<ContentTemplate> _aggregateTemplates = null;

        private async Task EnsureAggregateTemplates()
        {
            if(_aggregateTemplates != null) { return; }

            _aggregateTemplates = new List<ContentTemplate>();
            foreach (var provider in _templateProviders)
            {
                var templates = await provider.GetAllTemplates().ConfigureAwait(false);
                _aggregateTemplates.AddRange(templates);
            }
        }

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
                var result = serializer.Deserialize(template.ModelType, modelString);
                return result;
            }
            catch(Exception ex)
            {
                _log.LogError($"Failed to deserialize model for page {page.Title} returning null. Exception was {ex.Message}:{ex.StackTrace}");
                return null;
            }
            
            
        }

        public object DesrializeTemplateModel(IPost post, ContentTemplate template)
        {
            string modelString;
            if (!string.IsNullOrWhiteSpace(post.DraftSerializedModel))
            {
                modelString = post.DraftSerializedModel;
            }
            else
            {
                modelString = post.SerializedModel;
            }
            if (string.IsNullOrWhiteSpace(modelString))
            {
                _log.LogError($"could not deserialize model from empty string on page {post.Title}");
                return null;
            }
            var serializer = GetSerializer(template.SerializerName);

            try
            {
                var result = serializer.Deserialize(template.ModelType, modelString);
                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"Failed to deserialize model for page {post.Title} returning null. Exception was {ex.Message}:{ex.StackTrace}");
                return null;
            }


        }


        public async Task<int> GetCountOfTemplates(
            string projectId,
            string forFeature
            )
        {
            await EnsureAggregateTemplates();

            return _aggregateTemplates.Where(x =>
                 (x.ProjectId == "*" || x.ProjectId == projectId)
                  && (true.Equals(x.Enabled))
                  && (x.AvailbleForFeature == "*" || x.AvailbleForFeature == forFeature)
                  )
                .Count();
        }


        public async Task<PagedResult<ContentTemplate>> GetTemplates(
            string projectId, 
            string forFeature, 
            string query,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            await EnsureAggregateTemplates();

            int offset = (pageSize * pageNumber) - pageSize;

            var result = new PagedResult<ContentTemplate>()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var list = _aggregateTemplates.Where(x =>
                 (x.ProjectId == "*" || x.ProjectId == projectId)
                  && (true.Equals(x.Enabled))
                  && (x.AvailbleForFeature == "*" || x.AvailbleForFeature == forFeature)
                  && ((query == null) || x.Title.ToUpper().Contains(query.ToUpper()))
                )
                .ToList();

            result.TotalItems = list.Count;
            result.Data = list.OrderBy(x => x.Title)
                .Skip(offset)
                .Take(pageSize)
                .ToList()
                ;


            return result;

        }

        public async Task<ContentTemplate> GetTemplate(
            string projectId, 
            string key, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            await EnsureAggregateTemplates();
            
            return _aggregateTemplates.Where(x => 
                    (x.ProjectId == "*" || x.ProjectId == projectId) 
                    && x.Key == key
                )
                .FirstOrDefault();
        }
    }
}
