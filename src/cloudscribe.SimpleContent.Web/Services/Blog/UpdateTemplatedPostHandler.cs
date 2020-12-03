// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Author:                  Joe Audette
// Created:                 2018-07-14
// Last Modified:           2019-04-07
// 

using cloudscribe.DateTimeUtils;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Models.Versioning;
using cloudscribe.SimpleContent.Web.Templating;
using cloudscribe.Web.Common.Razor;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class UpdateTemplatedPostHandler : IRequestHandler<UpdateTemplatedPostRequest, CommandResult<IPost>>
    {
        public UpdateTemplatedPostHandler(
            IProjectService projectService,
            IBlogService blogService,
            IOptions<BlogEditOptions> configOptionsAccessor,
            IContentHistoryCommands historyCommands,
            ITimeZoneHelper timeZoneHelper,
            ITimeZoneIdResolver timeZoneIdResolver,
            ITeaserService teaserService,
            IEnumerable<IModelSerializer> serializers,
            IEnumerable<IParseModelFromForm> formParsers,
            IEnumerable<IValidateTemplateModel> modelValidators,
            ViewRenderer viewRenderer,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            ILogger<UpdateTemplatedPostHandler> logger
            )
        {
            _projectService = projectService;
            _blogService = blogService;
            _editOptions = configOptionsAccessor.Value;
            _historyCommands = historyCommands;
            _timeZoneHelper = timeZoneHelper;
            _timeZoneIdResolver = timeZoneIdResolver;
            _teaserService = teaserService;
            _serializers = serializers;
            _formParsers = formParsers;
            _modelValidators = modelValidators;
            _viewRenderer = viewRenderer;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IProjectService _projectService;
        private readonly IBlogService _blogService;
        private readonly BlogEditOptions _editOptions;
        private readonly IContentHistoryCommands _historyCommands;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private readonly ITimeZoneIdResolver _timeZoneIdResolver;
        private readonly ITeaserService _teaserService;
        private readonly IEnumerable<IModelSerializer> _serializers;
        private readonly IEnumerable<IParseModelFromForm> _formParsers;
        private readonly IEnumerable<IValidateTemplateModel> _modelValidators;
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

        private IParseModelFromForm GetFormParser(string name)
        {
            foreach (var s in _formParsers)
            {
                if (s.ParserName == name) return s;
            }

            return _formParsers.FirstOrDefault();
        }

        private IValidateTemplateModel GetValidator(string name)
        {
            foreach (var s in _modelValidators)
            {
                if (s.ValidatorName == name) return s;
            }

            return _modelValidators.FirstOrDefault();
        }

        public async Task<CommandResult<IPost>> Handle(UpdateTemplatedPostRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var errors = new List<string>();
            var customModelIsValid = true;
            try
            {
                var post = request.Post;
                var history = post.CreateHistory(request.UserName);
                var project = await _projectService.GetCurrentProjectSettings();
                var serializer = GetSerializer(request.Template.SerializerName);
                var parser = GetFormParser(request.Template.FormParserName);
                var validator = GetValidator(request.Template.ValidatorName);
                var type = Type.GetType(request.Template.ModelType);
                var model = parser.ParseModel(request.Template.ModelType, request.Form);
                if (model == null)
                {
                    errors.Add(_localizer["Failed to parse custom template model from form."]);
                    customModelIsValid = false;
                    //failed to parse model from form
                    // at least return the original model before changes
                    string pageModelString;
                    if (!string.IsNullOrWhiteSpace(post.DraftSerializedModel))
                    {
                        pageModelString = post.DraftSerializedModel;
                    }
                    else
                    {
                        pageModelString = post.SerializedModel;
                    }
                    if (!string.IsNullOrWhiteSpace(pageModelString))
                    {
                        request.ViewModel.TemplateModel = serializer.Deserialize(request.Template.ModelType, pageModelString);
                    }
                }

                if (customModelIsValid)
                {
                    // we are going to return the parsed model either way
                    request.ViewModel.TemplateModel = model;

                    var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
                    var validationResults = new List<ValidationResult>();

                    customModelIsValid = validator.IsValid(model, validationContext, validationResults);
                    if (!customModelIsValid)
                    {
                        foreach (var item in validationResults)
                        {
                            foreach (var memberName in item.MemberNames)
                            {
                                request.ModelState.AddModelError(memberName, item.ErrorMessage);

                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                {
                    // remove any bad characters
                    request.ViewModel.Slug = ContentUtils.CreateSlug(request.ViewModel.Slug);
                    if (request.ViewModel.Slug != post.Slug)
                    {
                        var slug = request.ViewModel.Slug;
                        var slugIsAvailable = await _blogService.SlugIsAvailable(slug);
                        while (!slugIsAvailable)
                        {
                            slug += "-";
                            slugIsAvailable = await _blogService.SlugIsAvailable(slug);
                            
                        }

                        if (slugIsAvailable)
                        {
                            post.Slug = slug;
                        }
                    }
                }

                if (request.ModelState.IsValid)
                {
                    var modelString = serializer.Serialize(request.Template.ModelType, model);
                    var renderedModel = await _viewRenderer.RenderViewAsString(request.Template.RenderView, model).ConfigureAwait(false);

                    post.Title = request.ViewModel.Title;
                    post.CorrelationKey = request.ViewModel.CorrelationKey;
                    post.ImageUrl = request.ViewModel.ImageUrl;
                    post.ThumbnailUrl = request.ViewModel.ThumbnailUrl;
                    post.IsFeatured = request.ViewModel.IsFeatured;
                    post.TeaserOverride = request.ViewModel.TeaserOverride;
                    post.SuppressTeaser = request.ViewModel.SuppressTeaser;
                    post.ShowComments = request.ViewModel.ShowComments;
                    post.LastModified = DateTime.UtcNow;
                    post.LastModifiedByUser = request.UserName;
                    post.MetaDescription = request.ViewModel.MetaDescription;
                    
                    var categories = new List<string>();
                    if (!string.IsNullOrEmpty(request.ViewModel.Categories))
                    {
                        if (_editOptions.ForceLowerCaseCategories)
                        {
                            categories = request.ViewModel.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim().ToLower())
                            .Where(x =>
                            !string.IsNullOrWhiteSpace(x)
                            && x != ","
                            )
                            .Distinct()
                            .ToList();
                        }
                        else
                        {
                            categories = request.ViewModel.Categories.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim())
                            .Where(x =>
                            !string.IsNullOrWhiteSpace(x)
                            && x != ","
                            )
                            .Distinct()
                            .ToList();
                        }
                    }
                    post.Categories = categories;

                    var tzId = await _timeZoneIdResolver.GetUserTimeZoneId(CancellationToken.None);
                    var shouldFirePublishEvent = false;
                    var saveMode = request.ViewModel.SaveMode;
                    if (saveMode == SaveMode.PublishLater)
                    {
                        if (request.ViewModel.NewPubDate.HasValue)
                        {
                            var newPubDate = _timeZoneHelper.ConvertToUtc(request.ViewModel.NewPubDate.Value, tzId);
                            if (newPubDate < DateTime.UtcNow)
                            {
                                saveMode = SaveMode.PublishNow;
                                post.PubDate = newPubDate;
                            }
                        }
                    }

                    switch (saveMode)
                    {
                        case SaveMode.SaveDraft:

                            post.DraftSerializedModel = modelString;
                            post.DraftContent = renderedModel;
                            post.DraftAuthor = request.ViewModel.Author;
                            if (!post.PubDate.HasValue) { post.IsPublished = false; }

                            break;

                        case SaveMode.PublishLater:
                            post.DraftSerializedModel = modelString;
                            post.DraftContent = renderedModel;
                            post.DraftAuthor = request.ViewModel.Author;
                            if (request.ViewModel.NewPubDate.HasValue)
                            {
                                post.DraftPubDate = _timeZoneHelper.ConvertToUtc(request.ViewModel.NewPubDate.Value, tzId);
                            }
                            if (!post.PubDate.HasValue) { post.IsPublished = false; }

                            break;

                        case SaveMode.PublishNow:

                            post.Author = request.ViewModel.Author;
                            post.Content = renderedModel;
                            post.SerializedModel = modelString;

                            if(!post.PubDate.HasValue)
                            {
                                post.PubDate = DateTime.UtcNow;
                            }
                            
                            post.IsPublished = true;
                            shouldFirePublishEvent = true;

                            post.DraftAuthor = null;
                            post.DraftContent = null;
                            post.DraftPubDate = null;
                            post.DraftSerializedModel = null;

                            break;
                    }

                    if (project.TeaserMode != TeaserMode.Off)
                    {
                        // need to generate the teaser on save
                        var teaserResult = _teaserService.GenerateTeaser(
                            project.TeaserTruncationMode,
                            project.TeaserTruncationLength,
                            post.Content,
                            Guid.NewGuid().ToString(),//cache key
                            post.Slug,
                            project.LanguageCode,
                            false //logWarnings
                            );
                        post.AutoTeaser = teaserResult.Content;
                    }

                    await _historyCommands.Create(request.ProjectId, history).ConfigureAwait(false);

                    await _blogService.Update(post);

                    if (shouldFirePublishEvent)
                    {
                        await _blogService.FirePublishEvent(post).ConfigureAwait(false);
                        await _historyCommands.DeleteDraftHistory(project.Id, post.Id).ConfigureAwait(false);
                    }

                   
                }

                return new CommandResult<IPost>(post, customModelIsValid && request.ModelState.IsValid, errors);

            }
            catch (Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");

                errors.Add(_localizer["Updating a post from a content template failed. An error has been logged."]);

                return new CommandResult<IPost>(null, false, errors);
            }

        }
    }
}
