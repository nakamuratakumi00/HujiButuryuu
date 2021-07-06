using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Repositories;
using Macss.ViewModels;
using System.Collections.Generic;
using Macss.Models.Service;

namespace Macss.Controllers
{
    public class RoleController : Controller
    {

        public static class ExportType
        {
            public const string Excel = "excel";
            public const string CSV = "csv";
        }

        private IRoleRepository roleRepository;
        private IMenuRepository menuRepository;
        private ILogRepository logRepository;
        private LogService logService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            roleRepository = new RoleRepository(dbContext);
            menuRepository = new MenuRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
        }

        // GET: Role
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
            return Json(await roleRepository.GetAllRolesAsync());
        }

        // GET: Role/Details/5
        [AuthorityActionFilter]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(string id)
        {
            ViewBag.AllMenu = (await menuRepository.GetAllMenuAsync()).OrderBy(x => x.Id).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });
            ViewBag.SetMenu = (await menuRepository.GetMenuByRoleAsync(id)).OrderBy(x => x.Id).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });
            var role = new MRole();

            if (String.IsNullOrEmpty(id))
            {
                ViewBag.Mode = 0;    // 0:新規
            }
            else
            {
                role = (await roleRepository.GetSelectRolesAsync(id)).FirstOrDefault();
                if(role == null)
                {
                    ViewBag.Mode = 0;    // 0:新規
                    ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE019));
                }
                else
                {
                    ViewBag.Mode = 1;    // 1:編集
                   //// ログ履歴作成
                   // var loginUser = Session.GetUserID();
                   // var prossesingId = Session.GetProcessingID();
                   // logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.S1, String.Empty, String.Empty, logRepository);
                }
            }
            return View(new RoleViewModel(role));
        }

        // POST: Role/Details
        [HttpPost]
        [AuthorityActionFilter]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(RoleViewModel role)
        {
            string id = role.Id;
            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();

            // 画面への設定値
            ViewBag.AllMenu = (await menuRepository.GetAllMenuAsync()).OrderBy(x => x.Id).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });
            ViewBag.SetMenu = (await menuRepository.GetAllMenuAsync()).Where(x => role.SetMenu.Contains(x.Id)).OrderBy(x => x.Id).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });
            ViewBag.Mode = role.Mode;

            try
            {
                var errors = await roleRepository.UpdateRoleAsync(id, role, loginUser);
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(field, message);
                    }
                    return View(role);
                }

                // ロール更新後、取得しなおす
                ViewBag.SetMenu = (await menuRepository.GetMenuByRoleAsync(role.Id)).Select(x => new SelectListItem() { Text = x.Name, Value = x.Id });

                if (!ModelState.IsValid)
                {
                    return View(role);
                }

                // ログ履歴作成
                if (role.Mode == 0)
                {
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U1, String.Empty, String.Empty, logRepository);
                }
                else
                {
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U2, String.Empty, String.Empty, logRepository);
                }

                ViewBag.Mode = 1; // 1:編集

                // 完了メッセージ
                ViewBag.Message = String.Format(Resources.Message.CI003);

                return View(role);
            }
            catch(Exception ex)
            {
                // エラー画面へ遷移
                throw ex;
            }
        }

        // POST: Role/Delete
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            // ログ履歴作成
            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as System.Collections.Generic.List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Role").Where(x => x.ActionName == "Details").FirstOrDefault();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U3, String.Empty, String.Empty, logRepository);

            var result = await roleRepository.DeleteRolesAsync(id);
            return Json(new { succsess = result == String.Empty ? 1 : 0, errMsg = result == String.Empty ? String.Format(Resources.Message.CI004) : result });
        }

        public async Task<ActionResult> Export(List<RoleViewModel> paramList, string extention)
        {
            // ログ履歴作成
            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as System.Collections.Generic.List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Account").Where(x => x.ActionName == "Index").FirstOrDefault();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.O1, String.Empty, String.Empty, logRepository);

            // 出力処理
            if (paramList == null)
            {
                paramList = new List<RoleViewModel>();
            }
            var roleIds = paramList.Select(x => x.Id).ToList();
            var allData = await roleRepository.GetAllRolesInfoAsync();
            var list = allData.Where(x => roleIds.Contains(x.Id));
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

    }
}
