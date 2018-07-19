using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class InitTemplatedPageRequest : IRequest<CommandResult<IPage>>
    {
        public InitTemplatedPageRequest(
            string projectId,
            string createdByUserName,
            string author,
            NewContentViewModel model, 
            ContentTemplate template
            )
        {
            ProjectId = projectId;
            CreatedByUserName = createdByUserName;
            Author = author;
            ViewModel = model;
            Template = template;
        }

        public string ProjectId { get; private set; }
        public string CreatedByUserName { get; private set; }

        public string Author { get; private set; }

        public NewContentViewModel ViewModel { get; private set; }
        public ContentTemplate Template { get; private set; }
    }
}
