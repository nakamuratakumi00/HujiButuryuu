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
using static Macss.Repositories.MenuRepository;

namespace Macss.Controllers
{
    public class LogController : Controller
    {
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;

        private const string NoneParent = "MAIN";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
        }

        // GET: Log
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            //絞り込み用のデータ
            //var menu = Session.GetMenu();
            var menu = await GetMenuAsync();
            await SetViewDataAsync(menu);
            ViewBag.AllData = menu.Where(x => x.RoleKbn == (int)RoleKbn.Disp)
                                .Select(x => new MenuViewModels()
                                { MenuName = x.MenuName, RoleName = x.RoleName, Group = x.Group, MenuId = x.MenuId });


            LogViewModel model = new LogViewModel();

            DateTime dtNow = DateTime.Now;
            model.ExcectionFrom = dtNow.ToString("yyyy/MM/dd");

            return View(model);
        }

        private async Task SetViewDataAsync(List<MenuViewModels> menu)
        {

            //出力対象区分用のデータ
            ViewBag.MenuName = menu.Where(x => x.ParentId == "MAIN")
                                .Select(x => new SelectListItem() { Text = x.MenuName, Value = x.Group.ToString() });

            //出力対象機能用のデータ
            ViewBag.RoleName = menu.Where(x => x.RoleKbn == (int)RoleKbn.Disp)
                                .Select(x => new SelectListItem() { Text = x.RoleName, Value = x.MenuId.ToString() });

            //出力内容用のデータ
            ViewBag.Control = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.LogFunction))
                                .Select(x => new SelectListItem() { Text = x.Value1, Value = x.Kbn });
        }

        // POST: Log/Index
        [HttpPost]
        [AuthorityActionFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(LogViewModel logModel)
        {
            // 画面設定用
            //var menu = Session.GetMenu();
            var menu = await GetMenuAsync();
            await SetViewDataAsync(menu);
            var allData = menu.Where(x => x.RoleKbn == (int)RoleKbn.Disp)
                                .Select(x => new MenuViewModels()
                                { MenuName = x.MenuName, RoleName = x.RoleName, Group = x.Group, MenuId = x.MenuId });
            ViewBag.AllData = allData;

            // ログデータ検索
            logModel.ProcessingIdList = allData.Where(x => x.Group.ToString() == logModel.MenuName).Select(x => x.MenuId).ToArray();
            var outputData = await logRepository.GetLogByConditionAsync(logModel);

            // 0件の場合、エラーメッセージ
            if (outputData.Count() == 0)
            {
                ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE019));
                return View(logModel);
            }

            // ログ履歴作成
            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.O1, String.Empty, String.Empty, logRepository);

            // Excel出力
            var fs = LogService.Output(outputData);
            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(fs.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "Log" + dataString + ".xlsx");
        }

        // メニュー取得
        private async Task<List<MenuViewModels>> GetMenuAsync()
        {
            // メニュー作成／格納
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            var menuRepository = new MenuRepository(dbContext);
            var roleRepository = new RoleRepository(dbContext);

            var loginUser = Session.GetUserID();
            var menuList = (await menuRepository.GetDispMenuAsync()).ToList();
            var userRole = await roleRepository.GetUserRoleAsync(loginUser);
            var menuRole = await menuRepository.GetMenuRoleAsync(userRole);

            // 表示対象を一覧化
            var dispMenu = new List<MenuViewModels>();
            foreach (string menuId in menuRole)
            {
                var dispRow = menuList.SingleOrDefault(x => x.MenuId == menuId);

                if (dispRow != null)
                {
                    AddDispMenu(menuList, dispRow, dispMenu);
                }
            }

            dispMenu.OrderBy(x => x.MenuSort);

            // 親毎に子レコードを作成（表示用に整列））
            var parentList = dispMenu.Where(x => x.ParentId == NoneParent).ToList();
            var childList = dispMenu.Where(x => x.ParentId != NoneParent).ToList();
            var ret = new List<MenuViewModels>();
            var group = 0;

            foreach (MenuViewModels row in parentList)
            {
                var level = 0;
                var endTagCnt = 0;
                AddMenuRow(row, childList, ret, level, endTagCnt, group, false);
                group++;
            }

            return ret;
        }
        /// <summary>
        /// 表示用のメニュー一覧を作成する（再帰）
        /// </summary>
        /// <param name="menuList">メニュー一覧</param>
        /// <param name="dispRow">処理行</param>
        /// <param name="dispMenu">作成する一覧</param>
        private static void AddDispMenu(List<MenuViewModels> menuList, MenuViewModels dispRow, List<MenuViewModels> dispMenu)
        {

            if (dispMenu.Any(x => x.MenuId == dispRow.MenuId))
            {
                return;
            }

            if (dispRow.ParentId == NoneParent)
            {
                dispMenu.Add(dispRow);
                return;
            }

            dispMenu.Add(dispRow);
            var parentRow = menuList.SingleOrDefault(x => x.MenuId == dispRow.ParentId);
            if (parentRow == null)
            {
                return;
            }

            AddDispMenu(menuList, parentRow, dispMenu);
        }
        /// <summary>
        /// 子の有無を確認しながらレコードを作成（再帰処理）
        /// </summary>
        /// <param name="row">レコード</param>
        /// <param name="childList">子メニュー一覧</param>
        /// <param name="ret">メニュー一覧</param>
        /// <param name="level">階層</param>
        /// <param name="endTagCnt">要素の終了タグ数</param>
        /// <param name="group">所属グループ</param>
        /// <param name="islast">ループの最後の場合はtrue</param>
        private static void AddMenuRow(MenuViewModels row, List<MenuViewModels> childList, List<MenuViewModels> ret, int level, int endTagCnt, int group, bool islast)
        {
            if (childList.Any(x => x.ParentId == row.MenuId))
            {
                // 子がいる場合、自身を追加後に子を追加（再帰）
                var thisLevel = level;
                level++;

                row.IsChild = true;
                row.level = thisLevel;
                row.Group = group;
                ret.Add(row);

                var children = childList.Where(x => x.ParentId == row.MenuId).ToList();

                foreach (MenuViewModels nextRow in children)
                {
                    // 階層の最後の場合、終了タグを作成する
                    var endFlg = false;
                    var endTag = 0;      // 初期化することで、ネストの深さを管理
                    if (children.IndexOf(nextRow) == children.Count - 1)
                    {
                        endTag = ++endTagCnt;
                        endFlg = true;
                    }
                    AddMenuRow(nextRow, childList, ret, level, endTag, group, endFlg);
                }
            }
            else
            {
                // 子がいない場合、自身を追加して終了
                row.IsChild = false;
                row.level = level;
                row.Group = group;
                if (islast)
                {
                    row.CntUlTag = endTagCnt;

                    if (row.level == endTagCnt)
                    {
                        row.IsLast = true;
                    }
                }
                ret.Add(row);
            }
        }

    }
}