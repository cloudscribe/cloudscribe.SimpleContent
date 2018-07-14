using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace cloudscribe.SimpleContent.Web.Services.Blog
{
    public class UpdateTemplatedPostRequest : IRequest<CommandResult<IPost>>
    {
        public UpdateTemplatedPostRequest(
            string projectId,
            string userName,
            PostEditWithTemplateViewModel viewModel,
            ContentTemplate template,
            IPost post,
            IFormCollection form,
            ModelStateDictionary modelState
            )
        {
            ProjectId = projectId;
            UserName = userName;
            ViewModel = viewModel;
            Template = template;
            Post = post;
            Form = form;
            ModelState = modelState;

        }

        public string ProjectId { get; private set; }
        public string UserName { get; private set; }

        public PostEditWithTemplateViewModel ViewModel { get; private set; }
        public ContentTemplate Template { get; private set; }
        public IPost Post { get; private set; }

        public IFormCollection Form { get; private set; }
        public ModelStateDictionary ModelState { get; private set; }


    }
}
