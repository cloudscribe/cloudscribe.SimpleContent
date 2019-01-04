using System.Collections.Generic;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class ListWithContentModel
    {
        public ListWithContentModel()
        {
            Items = new List<ListItemModel>();
        }

        public string ContentAbove { get; set; }
        public string ContentBelow { get; set; }

        public List<ListItemModel> Items { get; set; }
    }

    public class ListItemModel
    {
        public string FullSizeUrl { get; set; }
        public string ResizedUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string LinkUrl { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int Sort { get; set; } = 3;

        public bool OpensInNewWindow { get; set; }
    }
}
