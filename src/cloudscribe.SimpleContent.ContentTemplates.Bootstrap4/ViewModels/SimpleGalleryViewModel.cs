using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class SimpleGalleryViewModel : ListWithContentModel
    {
        public string Layout { get; set; } = "cards";
    }
}
