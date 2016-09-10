using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectEmailService
    {
        Task SendCommentNotificationEmailAsync(
            IProjectSettings project, 
            IPost post, 
            IComment comment, 
            string postUrl, 
            string approveUrl, 
            string deleteUrl);
    }
}