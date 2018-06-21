using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common.Razor;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class InitTemplatedPageHandler : IRequestHandler<InitTemplatedPageRequest, CommandResult<IPage>>
    {
        public InitTemplatedPageHandler(
            IPageService pageService,
            IEnumerable<IModelSerializer> serializers,
            ViewRenderer viewRenderer,
            ILogger<InitTemplatedPageHandler> logger
            )
        {
            _pageService = pageService;
            _serializers = serializers;
            _viewRenderer = viewRenderer;
            _log = logger;
        }

        private readonly IPageService _pageService;
        private readonly IEnumerable<IModelSerializer> _serializers;
        private readonly ViewRenderer _viewRenderer;
        private readonly ILogger _log;

        private IModelSerializer GetSerializer(string name)
        {
            foreach(var s in _serializers)
            {
                if (s.Name == name) return s;
            }

            return _serializers.FirstOrDefault();

        }


        public async Task<CommandResult<IPage>> Handle(InitTemplatedPageRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            
            // initialize an unpublished page based on the template


            var errors = new List<string>();
            try
            {
                var serializer = GetSerializer(request.Template.SerializerName);
                var type = Type.GetType(request.Template.ModelType);
                var model = Activator.CreateInstance(type);
                var typedModel = Convert.ChangeType(model, type);
                var page = new Page
                {
                    ProjectId = request.ProjectId,
                    CreatedByUser = request.CreatedByUserName,
                    LastModifiedByUser = request.CreatedByUserName,
                    TemplateKey = request.Template.Key,
                    Title = request.ViewModel.PageTitle,
                    Serializer = serializer.Name,
                    DraftSerializedModel = serializer.Serialize(request.Template.ModelType, model),
                    ParentSlug = request.ViewModel.ParentSlug,
                    DraftContent = await _viewRenderer.RenderViewAsString(request.Template.RenderView, model).ConfigureAwait(false),
                    IsPublished = false
                };
                
                page.Content = page.DraftContent;

                if (!string.IsNullOrEmpty(request.ViewModel.ParentSlug))
                {
                    var parentPage = await _pageService.GetPageBySlug(request.ViewModel.ParentSlug);
                    if (parentPage != null)
                    {
                        if (parentPage.Id != page.ParentId)
                        {
                            page.ParentId = parentPage.Id;
                            page.ParentSlug = parentPage.Slug;
                           
                        }

                    }
                }
                else
                {
                    // empty means root level
                    page.ParentSlug = string.Empty;
                    page.ParentId = "0";
                }


                await _pageService.Create(page, false);
                _pageService.ClearNavigationCache();

                var result = new CommandResult<IPage>(page, true, new List<string>());

                return result;
            }
            catch(Exception ex)
            {
                errors.Add($"{ex.Message}:{ex.StackTrace}");
                return new CommandResult<IPage>(null, false, errors);
            }
            


            
        }
    }
}
