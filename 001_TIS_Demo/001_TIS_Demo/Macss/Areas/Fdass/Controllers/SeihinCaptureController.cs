using Macss.Areas.Fdass.Repositories;
using Macss.Areas.Fdass.ViewModels;
using Macss.Attributes.ActionFilter;
using Macss.Controllers;
using Macss.Models;
using Macss.Repositories;
using Macss.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Macss.Areas.Fdass.Controllers
{
    public class SeihinCaptureController : Controller
    {
        private ISeihinCaptureRepository tankaRepository;
        private LogService logService;
        private ILogRepository logRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            tankaRepository = new SeihinCaptureRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
        }

        // GET: Fdass/Tanka
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            ViewBag.Mode = "1";
            return View();

        }

        [HttpPost]
        [AuthorityActionFilter]
        public async Task<ActionResult> Index(string run)
        {
            var loginUser = Session.GetUserID();
            SeihinCaptureViewModel model = new SeihinCaptureViewModel();
            string msg = string.Empty;
            if (!await tankaRepository.GetSeihin(model, loginUser, ModelState))
            {
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        msg = error.ErrorMessage;
                        Logwrite(ControlRepository.MControlFunctionKbn.E1, "エラー：" + error.ErrorMessage);
                    }
                }

                ViewBag.Mode = "1";
                return Json(new { succsess = false, errorMsg = msg });                
            }

            MakeMessage(model);
            ViewBag.Mode = "2";

            StringBuilder sb = new StringBuilder();
            sb.Append("<ul><li>Fe製品マスタ取込処理が完了しました</li>");
            sb.Append("<li>元データ件数 " + model.Moto + " 件</li>");
            sb.Append("<li>取り込み件数 " + model.Torikomi + " 件</li>");
            sb.Append("<li>エラー件数 " + model.Error + " 件</li></ul>");
            return Json(new { succsess = true, successMsg = sb.ToString() });

        }

        private void MakeMessage(SeihinCaptureViewModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("追加件数：" + model.Insert + "件\n");
            sb.Append("更新件数：" + model.Update + "件");

            Logwrite(ControlRepository.MControlFunctionKbn.I1, sb.ToString());
        }

        private void Logwrite(string funcKbn, string message)
        {

            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).First();
            string name = "処理機能：" + menu.TitleName;
            logService.CreateLogHistory(loginUser, prossesingId, funcKbn, name, message, logRepository);

        }
    }

}