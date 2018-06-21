using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.ViewModels;
using MediatR;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public class InitTemplatedPage : IRequest<CommandResult<IPage>>
    {
        public InitTemplatedPage(NewPageViewModel model, ContentTemplate template)
        {
            ViewModel = model;
            Template = template;
        }

        public NewPageViewModel ViewModel { get; private set; }
        public ContentTemplate Template { get; private set; }
    }
}
