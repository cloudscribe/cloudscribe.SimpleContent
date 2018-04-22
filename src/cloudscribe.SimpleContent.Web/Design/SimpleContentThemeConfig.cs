using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Web.Design
{
    public class SimpleContentThemeConfig
    {
        public SimpleContentThemeConfig()
        {
            ThemeSettings = new List<SimpleContentThemeSettings>();
        }

        public List<SimpleContentThemeSettings> ThemeSettings { get; set; }
    }
}
