using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using Macss.Models;
using Macss.Repositories;

namespace Macss.App_Start
{
    public class ApplicationUserManager : UserManager<MAccount>
    {
        private ApplicationUserManager(IUserStore<MAccount> store) : base(store)
        {
        }

        public static ApplicationUserManager Create(IUserStore<MAccount> store)
        {
            var manager = new ApplicationUserManager(store);

            // ユーザー名の検証ロジックを設定
            manager.UserValidator = new UserValidator<MAccount>(manager)
            {
                AllowOnlyAlphanumericUserNames = false
            };

            // パスワードの検証ロジックを設定
            manager.PasswordValidator = new PasswordValidator
            {
                //RequiredLength = 6,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // ユーザー ロックアウトの既定値を設定
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            return Create(context.Get<UserStore>());
        }
    }

    public class ApplicationSignInManager : SignInManager<MAccount, string>
    {
        private ApplicationSignInManager(
            UserManager<MAccount, string> userManager,
            IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}