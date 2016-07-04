using cloudscribe.Web.SimpleAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp
{
    public class SiteAuthSettingsResolver : IAuthSettingsResolver
    {
        public SiteAuthSettingsResolver(SiteSettings tenant)
        {
            this.tenant = tenant;

            authSettings = new SimpleAuthSettings();
            authSettings.AuthenticationScheme = tenant.AuthenticationScheme;
            authSettings.RecaptchaPrivateKey = tenant.RecaptchaPrivateKey;
            authSettings.RecaptchaPublicKey = tenant.RecaptchaPublicKey;
            authSettings.EnablePasswordHasherUi = tenant.EnablePasswordHasherUi;
        }

        private SiteSettings tenant;
        private SimpleAuthSettings authSettings;

        public SimpleAuthSettings GetCurrentAuthSettings()
        {
            return authSettings;
        }
    }
}
