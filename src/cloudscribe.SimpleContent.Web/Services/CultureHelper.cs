using cloudscribe.SimpleContent.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class CultureHelper
    {
        public CultureHelper(
            IOptions<RequestLocalizationOptions> localizationOptionsAccessor,
            IOptions<ContentLocalizationOptions> contentLocalizationOptionsAccessor
            )
        {
            _options = localizationOptionsAccessor.Value;
            _contentOptions = contentLocalizationOptionsAccessor.Value;
        }

        private readonly RequestLocalizationOptions _options;
        private readonly ContentLocalizationOptions _contentOptions;

        public bool IsDefaultCulture()
        {
            return _options.DefaultRequestCulture.UICulture.Name == CultureInfo.CurrentUICulture.Name;
        }

        public string CurrentUICultureName()
        {
            return CultureInfo.CurrentUICulture.Name;
        }

        public bool UseCultureProjectIds()
        {
            
            return _contentOptions.UseCultureProjectIds;
        }

        public bool UseCultureRoutes()
        {
            
            return _contentOptions.UseCultureRoutes;
        }
    }
}
