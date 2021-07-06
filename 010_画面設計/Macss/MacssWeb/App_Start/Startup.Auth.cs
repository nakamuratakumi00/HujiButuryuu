using MacssWeb.Models;
using MacssWeb.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace MacssWeb
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // 1要求につき1インスタンスのみを使用する構成
            //DbContext
            app.CreatePerOwinContext(MacssWebAccountDbContext.Create);
            //UserStore
            app.CreatePerOwinContext<MacssUserStore>(MacssUserStore.Create);
            //UserManager
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //SignInManager
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Cookieを使用したユーザー情報の格納
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}