using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class ContentTemplate
    {
        public ContentTemplate()
        {
            EditScripts = new List<EditScript>();
            EditCss = new List<EditStyle>();

        }

        public string ProjectId { get; set; } = "*";
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ModelType { get; set; }
        public string EditView { get; set; }
        public string RenderView { get; set; }

        public List<EditScript> EditScripts { get; set; }
        public List<EditStyle> EditCss { get; set; }
    }
}
