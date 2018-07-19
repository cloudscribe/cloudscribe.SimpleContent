// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-07-14
// Last Modified:           2018-07-14
// 

using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common.Razor;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class InitTemplatedPostHandler : IRequestHandler<InitTemplatedPostRequest, CommandResult<IPost>>
    {
        public InitTemplatedPostHandler(
            IBlogService blogService,
            IEnumerable<IModelSerializer> serializers,
            ViewRenderer viewRenderer,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            ILogger<InitTemplatedPageHandler> logger
            )
        {
            _blogService = blogService;
            _serializers = serializers;
            _viewRenderer = viewRenderer;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IBlogService _blogService;
        private readonly IEnumerable<IModelSerializer> _serializers;
        private readonly ViewRenderer _viewRenderer;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger _log;

        private IModelSerializer GetSerializer(string name)
        {
            foreach (var s in _serializers)
            {
                if (s.Name == name) return s;
            }

            return _serializers.FirstOrDefault();
        }

        public async Task<CommandResult<IPost>> Handle(InitTemplatedPostRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            // initialize an unpublished post based on the template

            var errors = new List<string>();
            try
            {
                var serializer = GetSerializer(request.Template.SerializerName);
                var type = Type.GetType(request.Template.ModelType);
                var model = Activator.CreateInstance(type);

                var post = new Post
                {
                    BlogId = request.ProjectId,
                    CreatedByUser = request.CreatedByUserName,
                    DraftAuthor = request.Author,
                    LastModifiedByUser = request.CreatedByUserName,
                    TemplateKey = request.Template.Key,
                    Title = request.ViewModel.Title,
                    Serializer = serializer.Name,
                    DraftSerializedModel = serializer.Serialize(request.Template.ModelType, model),
                    Content = await _viewRenderer.RenderViewAsString(request.Template.RenderView, model).ConfigureAwait(false),
                    IsPublished = false
                };
                
                await _blogService.Create(post);
                
                var result = new CommandResult<IPost>(post, true, errors);

                return result;
            }
            catch (Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");

                errors.Add(_localizer["Initializing a new post from a content template failed. An error has been logged."]);

                return new CommandResult<IPost>(null, false, errors);
            }

        }
    }
}
