using System.Threading.Tasks;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Models
{
    public interface IProjectEmailService
    {
        Task SendCommentNotificationEmailAsync(
            ProjectSettings project, 
            Post post, 
            Comment comment, 
            string postUrl, 
            string approveUrl, 
            string deleteUrl);
    }
}