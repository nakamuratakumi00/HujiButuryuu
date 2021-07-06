using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.App_Start;
using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Models.Service;
using Macss.Properties;
using Macss.Repositories;
using Macss.ViewModels;
using System.Text;

namespace Macss.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager userManager;
        private IAccountRepository accountRepository;
        private IRoleRepository roleRepository;
        private IGroupRepository groupRepository;
        private ILogRepository logRepository;
        private IControlRepository controlRepository;
        private MShukkabashoRepository mShukkabashoRepository;
        private LogService logService;

        #region 定数

        public static class ExportType
        {
            public const string Excel = "excel";
            public const string CSV = "csv";
        }

        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            accountRepository = new AccountRepository(dbContext, userManager);
            roleRepository = new RoleRepository(dbContext);
            groupRepository = new GroupRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            controlRepository = new ControlRepository(dbContext);
            mShukkabashoRepository = new MShukkabashoRepository(dbContext);
            logService = new LogService();
        }

        // GET: Account
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            //// ログ履歴作成
            //var loginUser = Session.GetUserID();
            //var prossesingId = Session.GetProcessingID();
            //logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.S2, String.Empty, String.Empty, logRepository);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> All()
        {
            return Json(await accountRepository.GetAllUsersAsync());
        }

        // GET: Account/Details/5
        [AuthorityActionFilter]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(string id)
        {
            ViewBag.Role = 0;
            string setGroupCd = String.Empty;
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                setGroupCd = user.GroupCd;
                user.Ctlfl1 = user.Ctlfl1 == string.Empty ? " " : user.Ctlfl1;
            }
            ViewBag.Roles = (await roleRepository.GetAllRolesAsync()).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });
            //ViewBag.Group = (await groupRepository.GetAllGroupAsync()).Select(x => new SelectListItem() { Text = x.GroupName, Value = x.GroupCd, Selected = (x.GroupCd == setGroupCd ? true: false) });
            ViewBag.SetRoles = (await accountRepository.GetAccountRolesAsync(id)).Select(x => new SelectListItem() { Text = x.RoleName, Value = x.RoleId });

            ViewBag.Bumon = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountBumon)).Select(x => new SelectListItem() { Text = x.Kbn + "　" + x.Value1, Value = x.Kbn });
            ViewBag.Group = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountGroup)).Select(x => new SelectListItem() { Text = x.Kbn + "　" + x.Value1, Value = x.Kbn });
            ViewBag.Basyo = (await mShukkabashoRepository.GetAllShukkabashosAsync()).Select(x => new SelectListItem() { Text = x.Sybcod + "　" + x.Sybnam, Value = x.Sybcod });
            ViewBag.Ctlfl = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            if (String.IsNullOrEmpty(id))
            {
                ViewBag.Mode = 0;    // 0:新規
            }
            else
            {
                if(user == null)
                {
                    ViewBag.Mode = 0;    // 0:新規
                    ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE019));
                }
                else
                {
                    ViewBag.Mode = 1;    // 1:編集
                    //// ログ履歴作成
                    //var loginUser = Session.GetUserID();
                    //var prossesingId = Session.GetProcessingID();
                    //logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.S1, String.Empty, String.Empty, logRepository);

                    // 権限チェック
                    //IEnumerable<AccountRoleViewModel> roles = await accountRepository.GetAccountRolesAsync(id);
                    //AccountRoleViewModel role = roles.First();
                    //if ((role.RoleId == "A00001" || role.RoleId == "F00001") && id == Session.GetUserID())
                    //{
                    //    ViewBag.Role = 1;
                    //}
                }
            }
            var viewData = new AccountViewModel(user);

            var failureCount = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.LoginCount));
            viewData.MaxFailureCount = failureCount.Count() == 0 ? 5 : failureCount.First().NumericValue1;

            return View(viewData);
        }

        // POST: Account/Details
        [HttpPost]
        [AuthorityActionFilter]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(AccountViewModel user)
        {
            var failureCount = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.LoginCount));
            user.MaxFailureCount = failureCount.Count() == 0 ? 5 : failureCount.First().NumericValue1;
            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            string id = user.AccountId;

            // 画面への設定値
            ViewBag.Roles = (await roleRepository.GetAllRolesAsync()).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });
            //ViewBag.Group = (await groupRepository.GetAllGroupAsync()).Select(x => new SelectListItem() { Text = x.GroupName, Value = x.GroupCd });
            ViewBag.Mode = user.Mode;
            ViewBag.SetRoles = (await roleRepository.GetAllRolesAsync()).Where(x => user.SetRoles.Contains(x.Id)).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });

            ViewBag.Bumon = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountBumon)).Select(x => new SelectListItem() { Text = x.Kbn + "　" + x.Value1, Value = x.Kbn });
            ViewBag.Group = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountGroup)).Select(x => new SelectListItem() { Text = x.Kbn + "　" + x.Value1, Value = x.Kbn });
            ViewBag.Basyo = (await mShukkabashoRepository.GetAllShukkabashosAsync()).Select(x => new SelectListItem() { Text = x.Sybcod + "　" + x.Sybnam, Value = x.Sybcod });
            ViewBag.Ctlfl = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            try
            {
                var errors = await accountRepository.UpdateUserAsync(id, user, loginUser);
                
                if (user != null && user.Ctlfl1 != null)
                {
                    user.Ctlfl1 = user.Ctlfl1.Trim();
                }

                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(String.Empty, message);
                    }
                    return View(user);
                }

                // ログ履歴作成
                if (user.Mode == 0)
                {
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U1, String.Empty, String.Empty, logRepository);
                }
                else
                {
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U2, String.Empty, String.Empty, logRepository);
                }

                // 完了メッセージ
                ViewBag.Message = String.Format(Resources.Message.CI003);

                // ロール更新後、取得しなおす
                ViewBag.SetRoles = (await accountRepository.GetAccountRolesAsync(user.AccountId)).Select(x => new SelectListItem() { Text = x.RoleName, Value = x.RoleId });
                ViewBag.Mode = 1; // 1:編集
                return View(user);
            }
            catch(Exception ex)
            {
                // エラー画面へ遷移
                throw ex;
            }
        }

        // Get: Account/ChangePassword/5
        [Authorize]
        public async Task<ActionResult> ChangePassword()
        {
            var loginUserId = Session.GetUserID();

            if (String.IsNullOrEmpty(loginUserId))
            {
                return RedirectToAction("Index", "Home");
            }

            // パスワード有効期限チェック
            int idigits = 6;
            ViewBag.Deadline = "0";
            var controls2 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "2").FirstOrDefault();
            if (controls2 != null)
            {
                if (!int.TryParse(controls2.Value1, out idigits))
                {
                    idigits = 6;
                }
            }

            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];
            if ((Referer != null && 0 < Referer.IndexOf("/Login")))
            {
                ViewBag.Deadline = "1";
            }

            StringBuilder msg = new StringBuilder();

            var controls3 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "3").FirstOrDefault();
            string digits3 = null;
            if (controls3 != null)
            {
                digits3 = controls3.Value1;
                char[] rgxs = digits3.ToCharArray();
                if (rgxs[0] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("半角英大文字");
                }
                if (rgxs[1] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("半角英小文字");
                }
                if (rgxs[2] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("数字");
                }
                if (rgxs[3] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("記号");
                }
            }

            var controls1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "1").FirstOrDefault();
            if (controls1 != null)
            {
                if (msg.Length != 0) msg.Append("　");
                
                if (controls1.Value2 != string.Empty)
                {
                    msg.Append(controls1.Value1 + "桁 ～ " + controls1.Value2 + "桁");
                } else
                {
                    msg.Append(controls1.Value1 + "桁");
                }
                msg.Append("で入力してください。");
                ViewBag.Msg = msg.ToString();
            }

            return View(new ChangePasswordViewModel() { Id = loginUserId });
        }

        // POST: Account/ChangePassword/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> ChangePassword(string id, ChangePasswordViewModel model)
        {

            StringBuilder msg = new StringBuilder();
            var controls3 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "3").FirstOrDefault();
            string digits3 = null;
            if (controls3 != null)
            {
                digits3 = controls3.Value1;
                char[] rgxs = digits3.ToCharArray();
                if (rgxs[0] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("半角英大文字");
                }
                if (rgxs[1] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("半角英小文字");
                }
                if (rgxs[2] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("数字");
                }
                if (rgxs[3] == '1')
                {
                    if (msg.Length != 0) msg.Append("・");
                    msg.Append("記号");
                }
            }

            var controls1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "1").FirstOrDefault();
            if (controls1 != null)
            {
                if (msg.Length != 0) msg.Append("　");
                if (controls1.Value2 != string.Empty)
                {
                    msg.Append(controls1.Value1 + "桁 ～ " + controls1.Value2 + "桁");
                }
                else
                {
                    msg.Append(controls1.Value1 + "桁");
                }
                msg.Append("で入力してください。");
                ViewBag.Msg = msg.ToString();
            }

            id = Session.GetUserID();

            if (model.Password != model.ConfirmationPassword)
            {
                ModelState.AddModelError(String.Empty, String.Format(Resources.Message.ME003));
                return View(new ChangePasswordViewModel() { Id = id });
            }

            // 認証
            var userChk = await userManager.FindAsync(id, model.Password);
            if (userChk != null)
            {
                ModelState.AddModelError(String.Empty, Resources.Message.CE117);
                return View(new ChangePasswordViewModel() { Id = id });
            }

            var errors = await accountRepository.ConfirmPassword(model.Password, 0);
            if (errors.Any())
            {
                foreach (var (field, message) in errors)
                {
                    ModelState.AddModelError(field, message);
                }
                return View(new ChangePasswordViewModel() { Id = id });
            }

            //パスワード履歴チェック
            int idigits = await accountRepository.GetPasswordControlValueAsync("4", 1);
            var pHs = await accountRepository.GetPasswordHistoryAsync(id, true);
            int cnt = 0;
            int pHCnt = pHs.Count();
            foreach (var pH in pHs)
            {
                if (cnt == idigits) break;
                var result = userManager.PasswordHasher.VerifyHashedPassword(pH.Password, model.Password);
                if (Microsoft.AspNet.Identity.PasswordVerificationResult.Success == result)
                {
                    ModelState.AddModelError(String.Empty, string.Format(Resources.Message.CE069, "パスワード", "過去に使用したパスワードは設定できません"));
                    return View(new ChangePasswordViewModel() { Id = id });

                }
                cnt++;
            }

            // 現在のパスワードチェック
            var appUserByCode = await accountRepository.FindByCodeAsync(id);
            var appUse = appUserByCode.First();
            string appUsePass = appUse.Password;
            var ret = await userManager.ChangePasswordAsync(id, model.CurrentPassword, model.Password);
            if (!ret.Succeeded)
            {
                foreach (var error in ret.Errors)
                {
                    ModelState.AddModelError(String.Empty, error);
                }
                return View(new ChangePasswordViewModel() { Id = id });
            }

            //パスワード履歴更新
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            pHs = await accountRepository.GetPasswordHistoryAsync(id, true);
            pHCnt = pHs.Count();
            if (pHCnt >= idigits) {
                foreach (var pH in pHs)
                {
                    if (pHCnt == idigits - 1) break;
                    dbContext.TPasswordHistory.Remove(pH);
                    pHCnt--;
                }
            }

            TPasswordHistory pHistory = new TPasswordHistory
            {
                Id = id,
                Password = appUsePass,
                CreateId = id,
                CreateDate = DateTime.Now,
                UpdateId = id,
                UpdateDate = DateTime.Now
            };
            dbContext.TPasswordHistory.Add(pHistory);

            // ログ履歴作成
            TLogHistory logFunc = new TLogHistory
            {
                CreateId = id,
                CreateDate = DateTime.Now,
                UpdateId = id,
                UpdateDate = DateTime.Now,
                ExcectionDate = DateTime.Now,
                UserId = id,
                ProcessingId = "11100",
                Function = ControlRepository.MControlFunctionKbn.U2,
                Purpose1 = "処理機能：パスワード変更",
                Purpose2 = "パスワード変更"
            };
            logRepository.CreateLogHisory(logFunc);

            // 処理完了
            ViewBag.Success = 1;
            ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CI002));
            return View(model);
        }

        public async Task<ActionResult> Export(List<AccountInformation> paramList, string extention)
        {
            // ログ履歴作成
            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as System.Collections.Generic.List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Account").Where(x => x.ActionName == "Index").FirstOrDefault();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.O1, String.Empty, String.Empty, logRepository);

            // 出力処理
            if (paramList == null)
            {
                paramList = new List<AccountInformation>();
            }
            var accountIds = paramList.Select(x => x.AccountId).ToList();
            var allData = await accountRepository.GetAllUsersAsync();
            var list = allData.Where(x => accountIds.Contains(x.AccountId));
            var fs = ExportService.Export(list, extention);

            if (extention == ExportType.Excel)
            {
                return File(fs.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else
            {
                return File(fs.ToArray(), "text/csv");
            }
        }
        #region フレームワーク 未使用機能

        //    // GET: Account/Create
        //    [Authorize(Roles = "admin")]
        //    public async Task<ActionResult> Create()
        //    {
        //        ViewBag.Roles = await roleRepository.GetAllRolesAsync();
        //        ViewBag.Group = await groupRepository.GetAllGroupAsync();
        //        return View();
        //    }

        //    // POST: Account/Create
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    [Authorize(Roles = "admin")]
        //    public async Task<ActionResult> Create(CreateAccountViewModel user)
        //    {
        //        ViewBag.Roles = await roleRepository.GetAllRolesAsync();
        //        ViewBag.Group = await groupRepository.GetAllGroupAsync();
        //        try
        //        {
        //            if (user.ConfirmationPassword != user.Password)
        //            {
        //                ModelState.AddModelError("", Resource.InvalidConfirmationPassword);
        //            }
        //            if (!ModelState.IsValid)
        //            {
        //                user.Password = "";
        //                user.ConfirmationPassword = "";
        //                return View(user);
        //            }

        //            var errors = await accountRepository.CreateUserAsync(user);
        //            if (errors.Any())
        //            {
        //                foreach (var error in errors)
        //                {
        //                    ModelState.AddModelError(error.field, error.message);
        //                }
        //            }

        //            if (!ModelState.IsValid)
        //            {
        //                user.Password = "";
        //                user.ConfirmationPassword = "";
        //                return View(user);
        //            }

        //            ModelState.Clear();

        //            return View(new CreateAccountViewModel());
        //        }
        //        catch
        //        {
        //            user.Password = "";
        //            user.ConfirmationPassword = "";
        //            return View(user);
        //        }
        //    }

        //    // GET: Account/Edit/5
        //    [Authorize]
        //    public async Task<ActionResult> Edit(string id)
        //    {
        //        if(String.IsNullOrEmpty(id))
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        if (!User.IsInRole("admin"))
        //        {
        //            if (User.Identity.GetUserId() != id)
        //            {
        //                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden");
        //            }
        //        }

        //        ViewBag.Roles = await roleRepository.GetAllRolesAsync();

        //        var user = await userManager.FindByIdAsync(id);
        //        user.Password = "";
        //        return View(new AccountViewModel(user));
        //    }

        //    // POST: Account/Edit/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    [Authorize]
        //    public async Task<ActionResult> Edit(string id, AccountViewModel user)
        //    {
        //        var isAdmin = User.IsInRole("admin");
        //        if (!isAdmin)
        //        {
        //            if (User.Identity.GetUserId() != id)
        //            {
        //                throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden");
        //            }
        //        }

        //        ViewBag.Roles = await roleRepository.GetAllRolesAsync();

        //        try
        //        {
        //            var errors = await accountRepository.UpdateUserAsync(id, user,"");
        //            if (errors.Any())
        //            {
        //                foreach (var error in errors)
        //                {
        //                    ModelState.AddModelError(error.field, error.message);
        //                }
        //            }

        //            if (!ModelState.IsValid)
        //            {
        //                return View(user);
        //            }

        //            return View(user);
        //        }
        //        catch
        //        {
        //            return View(user);
        //        }
        //    }

        //    // GET: Account/Delete/5
        //    [Authorize(Roles = "admin")]
        //    public async Task<ActionResult> Delete(string id)
        //    {
        //        if (String.IsNullOrEmpty(id))
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ViewBag.Roles = await roleRepository.GetAllRolesAsync();

        //        var user = await userManager.FindByIdAsync(id);
        //        return View(new AccountViewModel(user));
        //    }

        //    // POST: Account/Delete/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    [Authorize(Roles = "admin")]
        //    public async Task<ActionResult> Delete(string id, AccountViewModel user)
        //    {
        //        try
        //        {
        //            var delU = await userManager.FindByIdAsync(id);
        //            await userManager.DeleteAsync(delU);

        //            return RedirectToAction("Index");
        //        }
        //        catch
        //        {
        //            return View();
        //        }
        //    }

        //    // Get: Account/ChangePasswordByAdmin/5
        //    [Authorize(Roles = "admin")]
        //    public ActionResult ChangePasswordByAdmin(string id)
        //    {
        //        if (String.IsNullOrEmpty(id))
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        return View(new ChangePasswordByAdminViewModel() { Id = id });
        //    }

        //    // POST: Account/ChangePassword/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    [Authorize(Roles = "admin")]
        //    public async Task<ActionResult> ChangePasswordByAdmin(string id, ChangePasswordByAdminViewModel model)
        //    {
        //        var ret = await userManager.PasswordValidator.ValidateAsync(model.Password);
        //        if (!ret.Succeeded)
        //        {
        //            foreach (var error in ret.Errors)
        //            {
        //                ModelState.AddModelError(nameof(model.Password), error);
        //            }
        //        }
        //        if (!ModelState.IsValid)
        //        {
        //            return View(new ChangePasswordByAdminViewModel() { Id = id });
        //        }

        //        var user = await userManager.FindByIdAsync(id);
        //        user.Password = userManager.PasswordHasher.HashPassword(model.Password);
        //        await userManager.UpdateAsync(user);

        //        return RedirectToAction("Index");
        //    }
        #endregion フレームワーク 未使用機能
    }
}
