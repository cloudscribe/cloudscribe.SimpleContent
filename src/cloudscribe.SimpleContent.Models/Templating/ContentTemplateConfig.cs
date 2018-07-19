using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Models
{
    public class ContentTemplateConfig
    {
        public ContentTemplateConfig()
        {
            Templates = new List<ContentTemplate>();
        }

        public List<ContentTemplate> Templates { get; set; }
    }
}
