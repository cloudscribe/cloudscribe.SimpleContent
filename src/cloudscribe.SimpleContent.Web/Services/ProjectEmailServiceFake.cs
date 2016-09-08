using cloudscribe.SimpleContent.Models;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class ProjectEmailServiceFake : IProjectEmailService
    {
        public Task SendCommentNotificationEmailAsync(
            IProjectSettings project, 
            IPost post, 
            IComment comment, 
            string postUrl, 
            string approveUrl, 
            string deleteUrl)
        {
            return Task.FromResult(0);
        }
    }
}
