using log4net;
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

namespace Macss.Controllers
{
    public class GroupController : Controller
    {
        private IControlRepository controlRepository;
        private IGroupRepository groupRepository;
        private GroupService groupService;
        private ILogRepository logRepository;
        private LogService logService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            groupRepository = new GroupRepository(dbContext);
            groupService = new GroupService();
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
        }

        // GET: Group
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            // ログ履歴作成
            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.S2, String.Empty, String.Empty, logRepository);

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> All()
        {
            return Json(groupService.GetUpperAsync(await groupRepository.GetAllGroupAsync()));
        }

        // GET: Group/Details/5
        [AuthorityActionFilter]
        public async Task<ActionResult> Details(string id)
        {
            string setUpperGr = String.Empty;
            string setKbn = String.Empty;
            var group = new MGroup();

            if (String.IsNullOrEmpty(id))
            {
                ViewBag.Mode = 0;    // 0:新規
            }
            else
            {
                group = (await groupRepository.GetSelectGroupAsync(id)).FirstOrDefault();
                if (group == null)
                {
                    ViewBag.Mode = 0;    // 0:新規
                    ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE019));                    
                }
                else
                {
                    ViewBag.Mode = 1;    // 1:編集
                    setUpperGr = group.UpperClassCd;
                    setKbn = group.Kbn;
                    // ログ履歴作成
                    var loginUser = Session.GetUserID();
                    var prossesingId = Session.GetProcessingID();
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.S1, String.Empty, String.Empty, logRepository);
                }
            }
            ViewBag.UpperGroup = (await groupRepository.GetAllGroupAsync())
                .Select(x => new SelectListItem() { Text = x.GroupCd + "：" + x.GroupName, Value = x.GroupCd, Selected = (x.GroupCd == setUpperGr ? true : false) });
            ViewBag.GroupKbn = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Group))
                .Select(x => new SelectListItem() { Text = x.Value1, Value = x.Kbn, Selected = (x.Kbn == setKbn ? true : false) });
            return View(new GroupViewModel(group));
        }

        // POST: Group/Details
        [HttpPost]
        [AuthorityActionFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(GroupViewModel group)
        {
            string setUpperGr = group.UpperClassCd;
            string setKbn = group.Kbn;
            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();

            // 画面への設定値
            ViewBag.UpperGroup = (await groupRepository.GetAllGroupAsync())
                .Select(x => new SelectListItem() { Text = x.GroupCd + "：" + x.GroupName, Value = x.GroupCd, Selected = (x.UpperClassCd == setUpperGr ? true : false) });
            ViewBag.GroupKbn = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Group))
                .Select(x => new SelectListItem() { Text = x.Value1, Value = x.Kbn, Selected = (x.Kbn == setKbn ? true : false) });
            ViewBag.Mode = group.Mode;

            try
            {
                var errors = await groupService.UpdatGroupAsync(group, loginUser, groupRepository);

                // グループ更新後、取得しなおす
                ViewBag.UpperGroup = (await groupRepository.GetAllGroupAsync())
                    .Select(x => new SelectListItem() { Text = x.GroupCd + "：" + x.GroupName, Value = x.GroupCd, Selected = (x.UpperClassCd == setUpperGr ? true : false) });

                if (!ModelState.IsValid)
                {
                    return View(group);
                }
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(field, message);
                    }
                    return View(group);
                }

                // ログ履歴作成
                if (group.Mode == 0)
                {
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U1, String.Empty, String.Empty, logRepository);
                }
                else
                {
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U2, String.Empty, String.Empty, logRepository);
                }
                
                // 完了メッセージ
                ViewBag.Message = String.Format(Resources.Message.CI003);

                ViewBag.Mode = 1; // 1:編集
                return View(group);
            }
            catch (Exception ex)
            {
                // エラー画面へ遷移
                throw ex;
            }
        }

        // POST: Group/Delete
        [HttpPost]
        public async Task<ActionResult> Delete(string cd)
        {
            // ログ履歴作成
            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as System.Collections.Generic.List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Group").Where(x => x.ActionName == "Details").FirstOrDefault();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U3, String.Empty, String.Empty, logRepository);

            var result = await groupRepository.DeleteGroupAsync(cd);
            return Json(new { succsess = result == String.Empty ? 1 : 0, errMsg = result == String.Empty ? String.Format(Resources.Message.CI004) : result });
        }
    }
}
