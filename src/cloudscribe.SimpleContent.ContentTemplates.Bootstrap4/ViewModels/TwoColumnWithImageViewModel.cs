using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.ContentTemplates.ViewModels
{
    public class TwoColumnWithImageViewModel
    {
        public string ColumnOneContent { get; set; }

        public string ColumnTwoContent { get; set; }

        public string ImageUrl { get; set; }
        public string ImageFullSizeUrl { get; set; }
        public string ImageThumbSizeUrl { get; set; }
    }
}
