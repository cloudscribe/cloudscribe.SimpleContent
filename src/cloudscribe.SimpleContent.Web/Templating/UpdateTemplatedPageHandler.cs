using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common.Razor;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class UpdateTemplatedPageHandler : IRequestHandler<UpdateTemplatedPageRequest, CommandResult<IPage>>
    {
        public UpdateTemplatedPageHandler(
            IPageService pageService,
            IEnumerable<IModelSerializer> serializers,
            IEnumerable<IParseModelFromForm> formParsers,
            ViewRenderer viewRenderer,
            IStringLocalizer<cloudscribe.SimpleContent.Web.SimpleContent> localizer,
            ILogger<UpdateTemplatedPageHandler> logger
            )
        {
            _pageService = pageService;
            _serializers = serializers;
            _formParsers = formParsers;
            _viewRenderer = viewRenderer;
            _localizer = localizer;
            _log = logger;
        }

        private readonly IPageService _pageService;
        private readonly IEnumerable<IModelSerializer> _serializers;
        private readonly IEnumerable<IParseModelFromForm> _formParsers;
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

        public async Task<CommandResult<IPage>> Handle(UpdateTemplatedPageRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var errors = new List<string>();
            var success = true;
            try
            {
                var page = request.Page;
                var serializer = GetSerializer(request.Template.SerializerName);
                var parser = GetFormParser(request.Template.FormParserName);
                var type = Type.GetType(request.Template.ModelType);
                var model = parser.ParseModel(request.Template.ModelType, request.Form);
                if(model == null)
                {
                    errors.Add(_localizer["Failed to parse custom tempalte model from form."]);
                    success = false;
                }

                if(success)
                {
                    //TODO: validate custom model and update model state
                    var isValid = true;

                    var modelString = serializer.Serialize(request.Template.ModelType, model);
                    var renderedModel = await _viewRenderer.RenderViewAsString(request.Template.RenderView, model).ConfigureAwait(false);

                    if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                    {
                        // remove any bad characters
                        request.ViewModel.Slug = ContentUtils.CreateSlug(request.ViewModel.Slug);
                        if (request.ViewModel.Slug != page.Slug)
                        {
                            var slugIsAvailable = await _pageService.SlugIsAvailable(request.ViewModel.Slug);
                            if (!slugIsAvailable)
                            {
                                errors.Add(_localizer["The page slug was not changed because the requested slug is already in use."]);
                                success = false;
                            }
                        }

                    }

                    if (request.ModelState.IsValid && success)
                    {
                        page.Title = request.ViewModel.Title;
                        page.CorrelationKey = request.ViewModel.CorrelationKey;
                        page.LastModified = DateTime.UtcNow;
                        page.LastModifiedByUser = request.ModifiedByUserName;
                        page.MenuFilters = request.ViewModel.MenuFilters;
                        page.MetaDescription = request.ViewModel.MetaDescription;
                        page.PageOrder = request.ViewModel.PageOrder;
                        if (!string.IsNullOrEmpty(request.ViewModel.Slug))
                        {
                            page.Slug = request.ViewModel.Slug;
                        }
                        page.ViewRoles = request.ViewModel.ViewRoles;

                    }

                    if (isValid && success)
                    {
                        if (request.ShouldPublish)
                        {
                            page.SerializedModel = modelString;
                            page.Content = renderedModel;
                            page.Author = request.ViewModel.Author;
                            page.PubDate = DateTime.UtcNow;
                            page.IsPublished = true;

                            page.DraftAuthor = null;
                            page.DraftContent = null;
                            page.DraftPubDate = null;
                        }
                        else
                        {
                            page.DraftSerializedModel = modelString;
                            page.DraftContent = renderedModel;
                            page.DraftAuthor = request.ViewModel.Author;
                            if (request.ViewModel.PubDate.HasValue)
                            {
                                page.DraftPubDate = request.ViewModel.PubDate.Value;
                            }
                        }

                        await _pageService.Update(page, request.ShouldPublish);
                        _pageService.ClearNavigationCache();
                    }
                    //either way return the parsed model on the viewmodel
                    request.ViewModel.TemplateModel = model;

                }
                else
                {
                    //failed to parse model from form
                    // at least return the original model before changes
                    string pageModelString;
                    if(!string.IsNullOrWhiteSpace(page.DraftSerializedModel))
                    {
                        pageModelString = page.DraftSerializedModel;
                    }
                    else
                    {
                        pageModelString = page.SerializedModel;
                    }
                    if(!string.IsNullOrWhiteSpace(pageModelString))
                    {
                        request.ViewModel.TemplateModel = serializer.Deserialize(request.Template.ModelType, pageModelString);
                    }

                }
                
                var result = new CommandResult<IPage>(page, success, errors);

                return result;
            }
            catch(Exception ex)
            {
                _log.LogError($"{ex.Message}:{ex.StackTrace}");

                errors.Add(_localizer["Updating a page from a content template failed. An error has been logged."]);

                return new CommandResult<IPage>(null, false, errors);
            }



        }


    }
}
