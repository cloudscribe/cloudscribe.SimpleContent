using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cloudscribe.SimpleContent.Web.Services.Blog
{
    public class CreateOrUpdatePostRequest : IRequest<CommandResult<IPost>>
    {
        public CreateOrUpdatePostRequest(
            string projectId,
            string userName,
            PostEditViewModel viewModel,
            IPost post,
            ModelStateDictionary modelState
            )
        {
            ProjectId = projectId;
            UserName = userName;
            ViewModel = viewModel;
            Post = post;
            ModelState = modelState;
        }

        public string ProjectId { get; private set; }
        public string UserName { get; private set; }

        public PostEditViewModel ViewModel { get; private set; }

        public IPost Post { get; private set; }

        public ModelStateDictionary ModelState { get; private set; }

    }
}
