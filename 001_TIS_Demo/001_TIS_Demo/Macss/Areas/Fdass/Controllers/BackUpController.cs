using Macss.Areas.Fdass.Repositories;
using Macss.Areas.Fdass.ViewModels;
using Macss.Attributes.ActionFilter;
using Macss.Controllers;
using Macss.Models;
using Macss.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Macss.Areas.Fdass.Controllers
{
    public class BackUpController : Controller
    {
        #region 定数
        public const string StRun = "1";        // 1:実行中
        public const string StEnd = "2";        // 2:完了
        public const string StBak = "3";        // 3:退避完了
        public const string StErr = "9";        // 9:異常終了
        #endregion

        private IMatujimeKanriRepository matujimeKanriRepository;
        private IBackUpRepository backUpRepository;
        private ITightenRepository tightenRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            matujimeKanriRepository = new MatujimeKanriRepository(dbContext);
            backUpRepository = new BackUpRepository(dbContext);
            tightenRepository = new TightenRepository(dbContext);
        }

        // GET: Fdass/BackUp
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            BackUpViewModel model = new BackUpViewModel();
            MatujimeKanriViewModel dispData = await matujimeKanriRepository.GetDispData();
            model.MatujimeKanri = dispData;
            ViewBag.Mode = "1";
            return View(model);

        }

        [HttpPost]
        [AuthorityActionFilter]
        public async Task<ActionResult> Index(BackUpViewModel vModel)
        {
            // ステータスチェック
            ViewBag.Mode = "1";
            BackUpViewModel model = new BackUpViewModel();
            MatujimeKanriViewModel dispData = await matujimeKanriRepository.GetDispData();
            model.MatujimeKanri = dispData;
            string[] status = dispData.Status.Split(':');
            if (status[0] == StRun)
            {
                ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE106));
                return View(model);
            }
            else if (status[0] == StBak)
            {
                ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE107));
                return View(model);
            }
            else if (status[0] == StErr)
            {
                ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE108));
                return View(model);
            }

            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            string menuName = "処理機能：" + menu.TitleName;

            string yyyymm = dispData.Month.Replace("/", "");
            await tightenRepository.SetStatus(yyyymm, StRun, loginUser, ModelState);
            if (!await backUpRepository.BackUpData(yyyymm, prossesingId, menuName, loginUser, ModelState))
            {
                await tightenRepository.SetStatus(yyyymm, StErr, loginUser, ModelState);

                dispData = await matujimeKanriRepository.GetDispData();
                model.MatujimeKanri = dispData;
                ViewBag.Mode = "1";
                return View(model);
            }

            await tightenRepository.SetStatus(yyyymm, StBak, loginUser, ModelState);

            ViewBag.Mode = "2";
            ViewBag.Success = "1";
            ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CI007));
            dispData = await matujimeKanriRepository.GetDispData();
            model.MatujimeKanri = dispData;
            return View(model);

        }
    }
}