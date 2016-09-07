using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class ProjectEmailServiceFake : IProjectEmailService
    {
        public Task SendCommentNotificationEmailAsync(
            ProjectSettings project, 
            IPost post, 
            Comment comment, 
            string postUrl, 
            string approveUrl, 
            string deleteUrl)
        {
            return Task.FromResult(0);
        }
    }
}
