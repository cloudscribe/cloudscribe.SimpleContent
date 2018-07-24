using cloudscribe.SimpleContent.Models;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class PageEditContext
    {
        public PageEditContext(
            IProjectSettings projectSettings,
            IPage currentPage,
            ContentHistory contentHistory,
            bool canEdit,
            bool didReplaceDraft,
            bool didRestoreDeleted,
            int rootPageCount = -1
            )
        {
            Project = projectSettings;
            CurrentPage = currentPage;
            History = contentHistory;
            CanEdit = canEdit;
            DidReplaceDraft = didReplaceDraft;
            DidRestoreDeleted = didRestoreDeleted;
            RootPageCount = rootPageCount;
        }

        public IProjectSettings Project { get; private set; } = null;
        public IPage CurrentPage { get; private set; } = null;
        public bool CanEdit { get; private set; }
        public ContentHistory History { get; private set; } = null;
        public bool DidReplaceDraft { get; private set; }
        public bool DidRestoreDeleted { get; private set; }
        public int RootPageCount { get; private set; } = -1;
        public bool IsValidRequest
        {
            get
            {
                return CanEdit && Project != null;
            }
        }
    }
}
