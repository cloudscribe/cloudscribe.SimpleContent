using Microsoft.Extensions.Options;

namespace cloudscribe.SimpleContent.Web.Design
{
    public class DefaultSimpleContentThemeHelper : ISimpleContentThemeHelper
    {
        public DefaultSimpleContentThemeHelper(
            IOptions<SimpleContentThemeConfig> themeConfigAccessor,
            IOptions<SimpleContentIconConfig> iconConfigOptionsAccessor
            )
        {
            _themeConfig = themeConfigAccessor.Value;
            _iconConfig = iconConfigOptionsAccessor.Value;
        }

        private SimpleContentThemeConfig _themeConfig;
        private SimpleContentIconConfig _iconConfig;

        public SimpleContentThemeSettings GetThemeSettings()
        {
            SimpleContentThemeSettings result = null;
            foreach (var ts in _themeConfig.ThemeSettings)
            {
                if (ts.ThemeName == "default")
                {
                    result = ts;
                    break;
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
