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
    public class InitTemplatedPageHandler : IRequestHandler<InitTemplatedPage, CommandResult<IPage>>
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

        public async Task<CommandResult<IPage>> Handle(InitTemplatedPage request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var errors = new List<string>();
            try
            {
                var serializer = GetSerializer(request.Template.SerializerName);
                var type = Type.GetType(request.Template.ModelType);
                var model = Activator.CreateInstance(type);
                var typedModel = Convert.ChangeType(model, type);
                var page = new Page();
                page.TemplateKey = request.Template.Key;
                page.Title = request.ViewModel.PageTitle;
                page.Serializer = serializer.Name;
                page.DraftSerializedModel = serializer.Serialize(request.Template.ModelType, model);
                page.Slug = ContentUtils.CreateSlug(page.Title);

                page.DraftContent = await _viewRenderer.RenderViewAsString("EmailTemplates/ConfirmAccountTextEmail", model).ConfigureAwait(false);


                var result = new CommandResult<IPage>(page, false, new List<string>());

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
