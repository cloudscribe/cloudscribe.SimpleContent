using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Web.Design
{
    public class SimpleContentThemeSettings
    {
        public SimpleContentThemeSettings()
        {
            Icons = new IconCssClasses();
        }

        public string ThemeName { get; set; } = "default";
        public string IconSetId { get; set; } = "fontawesome4x";

        public IconCssClasses Icons { get; set; }
    }
}
