using cloudscribe.Pagination.Models;
using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class ContentHistoryViewModel
    {
        public ContentHistoryViewModel()
        {
            History = new PagedResult<ContentHistory>();
        }

        public PagedResult<ContentHistory> History { get; set; }

        public int SortMode { get; set; } = 0;
        public string ContentId { get; set; }
        public string ContentSource { get; set; }

        public string Editor { get; set; }

        public string ContentTitle { get; set; }
        public string ContentSlug { get; set; }

        public bool CanEditPages { get; set; }
        public bool CanEditPosts { get; set; }


    }
}
