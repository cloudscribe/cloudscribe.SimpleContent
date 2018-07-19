using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class UpdateTemplatedPageRequest : IRequest<CommandResult<IPage>>
    {
        public UpdateTemplatedPageRequest(
            string projectId,
            string userName,
            PageEditWithTemplateViewModel viewModel,
            ContentTemplate template,
            IPage page,
            IFormCollection form,
            ModelStateDictionary modelState
            )
        {
            ProjectId = projectId;
            UserName = userName;
            ViewModel = viewModel;
            Template = template;
            Page = page;
            Form = form;
            ModelState = modelState;

        }

        public string ProjectId { get; private set; }
        public string UserName { get; private set; }

        public PageEditWithTemplateViewModel ViewModel { get; private set; }
        public ContentTemplate Template { get; private set; }
        public IPage Page { get; private set; }
        
        public IFormCollection Form { get; private set; }
        public ModelStateDictionary ModelState { get; private set; }


    }
}
