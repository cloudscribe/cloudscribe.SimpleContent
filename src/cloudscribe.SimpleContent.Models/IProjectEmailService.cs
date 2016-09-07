using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectEmailService
    {
        Task SendCommentNotificationEmailAsync(
            ProjectSettings project, 
            IPost post, 
            Comment comment, 
            string postUrl, 
            string approveUrl, 
            string deleteUrl);
    }
}