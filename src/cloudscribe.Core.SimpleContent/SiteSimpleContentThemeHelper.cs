using cloudscribe.Core.Models;
using cloudscribe.SimpleContent.Web.Design;
using Microsoft.Extensions.Options;

namespace cloudscribe.Core.SimpleContent
{
    public class SiteSimpleContentThemeHelper : ISimpleContentThemeHelper
    {
        public SiteSimpleContentThemeHelper(
            SiteContext currentSite,
            IOptions<SimpleContentThemeConfig> themeConfigAccessor,
            IOptions<SimpleContentIconConfig> iconConfigOptionsAccessor
            )
        {
            _currentSite = currentSite;
            _themeConfig = themeConfigAccessor.Value;
            _iconConfig = iconConfigOptionsAccessor.Value;
        }

        private SiteContext _currentSite;
        private SimpleContentThemeConfig _themeConfig;
        private SimpleContentIconConfig _iconConfig;

        public SimpleContentThemeSettings GetThemeSettings()
        {
            SimpleContentThemeSettings result = null;
            foreach (var ts in _themeConfig.ThemeSettings)
            {
                if (ts.ThemeName == _currentSite.Theme)
                {
                    result = ts;
                    break;
                }
            }

            if(result == null)
            {
                foreach (var ts in _themeConfig.ThemeSettings)
                {
                    if (ts.ThemeName == "default")
                    {
                        result = ts;
                        break;
                    }
                }
            }
            
            if (result == null)
            {
                result = new SimpleContentThemeSettings();
            }
            result.Icons = _iconConfig.GetIcons(result.IconSetId);


            return result;
        }

    }
}
