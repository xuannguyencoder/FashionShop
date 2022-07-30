using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(FashionShop.Startup))]

namespace FashionShop
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                LoginPath = new PathString("/dang-nhap"),
                SlidingExpiration = true
            });

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "670352473666-ik72v6krpe2cfhpl24oqj2l861god8ru.apps.googleusercontent.com",
                ClientSecret = "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"
            });
        }
    }
}
