using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class UpdateTemplatedPageRequest : IRequest<CommandResult<IPage>>
    {
        public UpdateTemplatedPageRequest(
            string projectId,
            string modifiedByUserName,
            PageEditWithTemplateViewModel viewModel,
            ContentTemplate template,
            IPage page,
            IFormCollection form,
            ModelStateDictionary modelState
            )
        {
            ProjectId = projectId;
            ModifiedByUserName = modifiedByUserName;
            ViewModel = viewModel;
            Template = template;
            Page = page;
            Form = form;
            ModelState = modelState;

        }

        public string ProjectId { get; private set; }
        public string ModifiedByUserName { get; private set; }

        public PageEditWithTemplateViewModel ViewModel { get; private set; }
        public ContentTemplate Template { get; private set; }
        public IPage Page { get; private set; }
        
        public IFormCollection Form { get; private set; }
        public ModelStateDictionary ModelState { get; private set; }


    }
}
