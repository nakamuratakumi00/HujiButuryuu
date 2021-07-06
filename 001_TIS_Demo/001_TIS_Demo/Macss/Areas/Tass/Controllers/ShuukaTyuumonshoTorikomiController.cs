using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Repositories;
using Macss.Controllers;
using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.Repositories;
using Macss.Areas.Tass.ViewModels;

namespace Macss.Areas.Tass.Controllers
{
    public class ShuukaTyuumonshoTorikomiController : Controller
    {
        IShuukaTyuumonshoTorikomiRepositorie torikomiRepositorie;
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            torikomiRepositorie = new ShuukaTyuumonshoTorikomiRepositorie(dbContext);
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
        }

        // 初回表示
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            Session.RemoveViewModel();
            WShuukaTyuumonshoTorikomiData TorikomiData = new WShuukaTyuumonshoTorikomiData();
            TorikomiSerch serch = new TorikomiSerch
            {
                Actcod = Session.GetUserID(),
                Ackymd = decimal.Parse(System.DateTime.Now.ToString("yyyyMMddHHmmssfff"))
            };
            TorikomiData.Serch = serch;

            int insRowCnt = torikomiRepositorie.GetTorikomiKouho(serch);

            return View(TorikomiData);
        }

        // 表示
        [HttpPost]
        public async Task<ActionResult> Search(string Actcod, string Ackymd)
        {
            TorikomiSerch serch = new TorikomiSerch
            {
                Actcod = Actcod,
                Ackymd = decimal.Parse(Ackymd)
            };

            var list = await torikomiRepositorie.DispTorikomiKouhoAsync(serch);

            return Json(list);
        }

        // データ取込
        [HttpPost]
        [AuthorityActionFilter]
        public async Task<ActionResult> Index(WShuukaTyuumonshoTorikomiData TorikomiData)
        {
            var list = await torikomiRepositorie.DispTorikomiKouhoAsync(TorikomiData.Serch);
            if (list.Count() == 0)
            {
                // 取込データなし 再表示
                return this.Redirect("ShuukaTyuumonshoTorikomi/Index");
            }

            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();

            int insRowCnt = torikomiRepositorie.InsertTorikomiData(TorikomiData.Serch);

            if (insRowCnt == 0)
            {
                string message = "既に取込されているデータが含まれています。";

                // ログ履歴作成
                logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.E1, "処理機能：" + menu.TitleName, string.Format("エラー：{0}", message), logRepository);

                // エラー表示
                ModelState.AddModelError(String.Empty, message);
                return View();
            }
            else
            {
                // ログ履歴作成
                logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.U1, "処理機能：" + menu.TitleName, string.Format("追加件数：{0}件", insRowCnt), logRepository);

                // 新規表示
                return this.Redirect("ShuukaTyuumonshoTorikomi/Index");
            }
        }
    }
}