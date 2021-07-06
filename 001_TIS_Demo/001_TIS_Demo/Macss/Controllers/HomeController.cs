using Macss.App_Start;
using Macss.Models;
using Macss.Repositories;
using Macss.Service;
using Macss.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Macss.Repositories.AccountRepository;

namespace Macss.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IRoleRepository roleRepository;
        private IMenuRepository menuRepository;
        private IAccountRepository accountRepository;
        private IControlRepository controlRepository;

        private ILogRepository logRepository;
        private LogService logService;

        private IUseStatusRepository useStatusRepository;

        // GET: Home
        public async Task<ActionResult> Index()
        {

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            accountRepository = new AccountRepository(dbContext);
            var dbAccount = await accountRepository.FindByCodeAsync(Session.GetUserID());
            var targetData = dbAccount.First();
            controlRepository = new ControlRepository(dbContext);

            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];
            if ((Referer != null && 0 < Referer.IndexOf("/Login")))
            {
                // パスワード有効期限チェック
                int idigits = 6;
                var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "2").FirstOrDefault();
                if (controls != null)
                {
                    if (!int.TryParse(controls.Value1, out idigits))
                    {
                        idigits = 6;
                    }
                }

                // 期限
                DateTime passwordDate = targetData.AccountPasswordDate.AddMonths(idigits);
                // 警告日
                DateTime workDate = passwordDate.AddDays(-14);
                if (workDate < DateTime.Now)
                {
                    ViewBag.Message = passwordDate.ToString("yyyy年MM月dd日までにパスワードを更新して下さい　");
                }
            }

            // お知らせ設定
            ViewBag.Information = string.Empty;
            if (targetData.GroupCd == "G000")
            {
                // 社内
                var inInfom = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Information)).Where(x => x.Kbn == "1").FirstOrDefault();
                if (inInfom != null)
                {
                    if (CheckInfoDate(inInfom.Value2, inInfom.Value3))
                    {
                        ViewBag.Information = inInfom.Value1 == null ? string.Empty : inInfom.Value1.Replace("\r\n", "<br/>");
                    }
                }
            }
            else
            {
                // 社外
                var outInfom = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Information)).Where(x => x.Kbn == "2").FirstOrDefault();
                if (outInfom != null)
                {
                    if (CheckInfoDate(outInfom.Value2, outInfom.Value3))
                    {
                        ViewBag.Information = outInfom.Value1 == null ? string.Empty : outInfom.Value1.Replace("\r\n", "<br/>");
                    }
                }
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Login()
        {
            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];
            string ReturnUrl = HttpContext.Request.QueryString["ReturnUrl"];
            if (Referer != null && ReturnUrl == null)
            {
                var sessionInfo = Session[SessionExtensions.Field.UserID];
                if (sessionInfo == null)
                {
                    var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
                    useStatusRepository = new UseStatusRepository(dbContext);
                    await useStatusRepository.DeleteUseStatusAsync(Session.SessionID, SessionExtensions.GetUserID(Session));

                    ModelState.AddModelError(String.Empty, "セッションがタイムアウトしました。");
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel parameter, string returnUrl)
        {
            var userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            var user = (await userManager.FindByIdAsync(parameter.Id));
            if (user == null || user.DeleteFlg == (int)DeleteFlg.Hide)
            {
                ModelState.AddModelError(String.Empty, Resources.Message.ME002);
                return View(parameter);
            }

            var loginFailureCount = user.LoginFailureCount;
            var userId = user.Id;

            // ログイン回数取得
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            var failureCount = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.LoginCount)).FirstOrDefault();
            decimal? digits = null;
            if (failureCount != null)
            {
                digits = failureCount.NumericValue1;
            }
            else
            {
                digits = 5;
            }

            logRepository = new LogRepository(dbContext);
            logService = new LogService();

            // 失敗回数判定
            if (loginFailureCount >= digits)
            {
                logService.CreateLogHistory(userId, ControlRepository.MControlSection.Login, ControlRepository.MControlFunctionKbn.L, "処理機能：ログイン", Resources.Message.ME001 + " (" + loginFailureCount + ")", logRepository);
                ModelState.AddModelError(String.Empty, Resources.Message.ME001);
                return View(parameter);
            }

            // 認証
            accountRepository = new AccountRepository(dbContext);
            var dbAccount = await accountRepository.FindByCodeAsync(userId);

            // デモ用 ->
            var allowList = new System.Collections.Generic.List<string>() { "9900097", "TISAdmin" };
            var ignorePwd = (allowList.Find(x => String.Compare(userId, x, true) == 0) != null);
            if (!ignorePwd)
            {
                // <- デモ用
                var userChk = await userManager.FindAsync(user.Id, parameter.Password);
                if (userChk == null)
                {
                    // 認証エラー処理
                    var count = loginFailureCount + 1;
                    if (dbAccount.Count() != 0)
                    {
                        await accountRepository.UpdateFailureCountAsync(userId, count);
                        ModelState.AddModelError(String.Empty, "パスワードが違います。");

                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, Resources.Message.ME002);
                    }

                    logService.CreateLogHistory(userId, ControlRepository.MControlSection.Login, ControlRepository.MControlFunctionKbn.L, "処理機能：ログイン", Resources.Message.ME002 + " (" + count + ")", logRepository);
                    return View(parameter);
                }
                // デモ用 ->
            }
            // <- デモ用
            // 成功時処理
            accountRepository = new AccountRepository(dbContext);
            await accountRepository.UpdateFailureCountAsync(userId, 0);
            var signInManager = this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            await signInManager.SignInAsync(user, false, false);
            //logService.CreateLogHistory(userId, ControlRepository.MControlSection.Login, ControlRepository.MControlFunctionKbn.L, "処理機能：ログイン", "ログイン成功", logRepository);
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "Index";
            }

            // セッション格納
            Session.Timeout = 60;
            //Session.Timeout = 2;
            Session.SetUserID(user.Id);
            Session.SetUserName(user.UserName);

            // メニュー作成／格納
            menuRepository = new MenuRepository(dbContext);
            roleRepository = new RoleRepository(dbContext);

            var menuList = (await menuRepository.GetDispMenuAsync()).ToList();
            var userRole = (await roleRepository.GetUserRoleAsync(user.Id));
            var menuRole = (await menuRepository.GetMenuRoleAsync(userRole));
            Session.SetMenu(MenuService.GetMenuListAsync(menuList, menuRole));

            // パスワード有効期限チェック
            int idigits = 6;
            var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "2").FirstOrDefault();
            if (controls != null)
            {
                if (!int.TryParse(controls.Value1, out idigits))
                {
                    idigits = 6;
                }
            }

            // デモ用 ->
            if (!ignorePwd)
            {
                // <- デモ用
                var targetData = dbAccount.First();
                DateTime workDate = DateTime.Now.AddMonths(-idigits);
                if (workDate > targetData.AccountPasswordDate)
                {
                    return RedirectToAction("ChangePassword", "Account");
                }
                // デモ用 ->
            }
            // <- デモ用

            useStatusRepository = new UseStatusRepository(dbContext);
            await useStatusRepository.UpdateUseStatusAsync(Session.SessionID, SessionExtensions.GetUserID(Session), "ログイン", "Login", "HomeController");

            return parameter.Password == AccountRepository.InitPassword ? RedirectToAction("ChangePassword", "Account") : (ActionResult)Redirect(returnUrl);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LogOff()
        {

            await AccessMonitoringAsync(Session);

            Session.Clear();
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index");
        }

        private async Task AccessMonitoringAsync(HttpSessionStateBase session)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            useStatusRepository = new UseStatusRepository(dbContext);
            await useStatusRepository.DeleteUseStatusAsync(session.SessionID, SessionExtensions.GetUserID(session));

        }

        private bool CheckInfoDate(string from, string to)
        {
            DateTime fromDate;
            if (!DateTime.TryParse(from, out fromDate))
            {
                fromDate = DateTime.MinValue;
            }

            DateTime toDate;
            if (!DateTime.TryParse(to, out toDate))
            {
                toDate = DateTime.MaxValue;
            }
            DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            if (fromDate <= nowDate && nowDate <= toDate)
            {
                return true;
            }
            return false;
        }
    }
}