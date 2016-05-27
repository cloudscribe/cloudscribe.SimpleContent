

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class CommentViewModel
    {
        public string PostId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string WebSite { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        

    }
}
