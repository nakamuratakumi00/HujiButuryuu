using Macss.Areas.Tass.Repositories;
using Macss.Areas.Tass.ViewModels;
using Macss.Attributes.ActionFilter;
using Macss.Controllers;
using Macss.Models;
using Macss.Repositories;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Macss.Areas.Tass.Controllers
{
    public class ShuukaRuisekiController : Controller
    {
        private IShuukaRuisekiRepositorie shuukaRuisekiRepositorie;
        private ILogRepository logRepository;
        private LogService logService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            shuukaRuisekiRepositorie = new ShuukaRuisekiRepositorie(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
        }

        // GET: Tass/ShuukaRuiseki
        public ActionResult Index()
        {
            ShuukaRuisekiViewModel data = new ShuukaRuisekiViewModel();
            data.YyyyMm = DateTime.Now.AddMonths(-1).ToString("yyyy/MM");
            return View(data);
        }

        [HttpPost]
        [AuthorityActionFilter]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(ShuukaRuisekiViewModel viewModel)
        {

            string loginUser = Session.GetUserID();

            DateTime stDate;
            if (!DateTime.TryParse(viewModel.YyyyMm + "/01", out stDate))
            {
                ModelState.AddModelError(String.Empty, "年月を入力してください");
                return View();
            }

            // 実行チェック
            var jiseki2 = await shuukaRuisekiRepositorie.CheckShuukaRuisekiAsync(viewModel);
            if (jiseki2.Count() != 0)
            {
                ModelState.AddModelError(String.Empty, "実行済みです");
                return View();
            }

            // 累積データ更新処理
            try
            {
                await shuukaRuisekiRepositorie.UpdateShuukaRuisekiAsync(viewModel, loginUser);
            } catch (Exception) {
                ModelState.AddModelError(String.Empty, "エラーが発生しました");
                return View();
            }

            ViewBag.Success = 1;
            ModelState.AddModelError(String.Empty, "実績データ削除件数　" + viewModel.Delete1 + "件");
            ModelState.AddModelError(String.Empty, "累積データ登録件数　" + viewModel.Insert2 + "件");
            ModelState.AddModelError(String.Empty, "累積データ削除件数　" + viewModel.Delete2 + "件");
            ModelState.AddModelError(String.Empty, "履歴データ登録件数　" + viewModel.InsertR + "件");
            ModelState.AddModelError(String.Empty, "履歴データ削除件数　" + viewModel.DeleteR + "件");

            // ログ履歴作成
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.S1, "処理機能：" + menu.TitleName, string.Format("月次期間終了年月：{0}", viewModel.YyyyMm), logRepository);

            return View(viewModel);

        }
    }
}