using MacssWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;

namespace MacssWeb
{
    /// <summary>
    /// UserManager
    /// </summary>
    public class ApplicationUserManager : UserManager<MacssLoginAccount>
    {
        public ApplicationUserManager(IUserStore<MacssLoginAccount> store)
            : base(store)
        { }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(context.Get<MacssUserStore>());

            // ユーザー名の検証ロジックを設定します
            manager.UserValidator = new UserValidator<MacssLoginAccount>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                //RequireUniqueEmail = true
            };

            // パスワードの検証ロジックを設定します
            manager.PasswordValidator = new PasswordValidator
            {
                //RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // ユーザー ロックアウトの既定値を設定します。
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider =
            //        new DataProtectorTokenProvider<MacssAccount>(dataProtectionProvider.Create("ASP.NET Identity"));
            //    //manager.UserTokenProvider =
            //    //    new DataProtectorTokenProvider<MacssAccount>(dataProtectionProvider.Create("ASP.NET Identity"))
            //    //        { TokenLifespan = TimeSpan.FromDays(7) };
            //}
            return manager;
        }
    }

    /// <summary>
    /// SignInManager
    /// </summary>
    public class ApplicationSignInManager : SignInManager<MacssLoginAccount, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(
            IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
