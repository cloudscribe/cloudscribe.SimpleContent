using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class CreateOrUpdatePageRequest : IRequest<CommandResult<IPage>>
    {
        public CreateOrUpdatePageRequest(
            string projectId,
            string userName,
            PageEditViewModel viewModel,
            IPage page,
            ModelStateDictionary modelState
            )
        {
            ProjectId = projectId;
            UserName = userName;
            ViewModel = viewModel;
            Page = page;
            ModelState = modelState;
        }

        public string ProjectId { get; private set; }
        public string UserName { get; private set; }

        public PageEditViewModel ViewModel { get; private set; }
      
        public IPage Page { get; private set; }

        public ModelStateDictionary ModelState { get; private set; }

    }
}
