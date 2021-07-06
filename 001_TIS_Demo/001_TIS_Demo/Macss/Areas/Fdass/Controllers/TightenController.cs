using Macss.Models;
using Macss.Repositories;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Areas.Tass.ViewModels;
using Macss.Controllers;
using Macss.Models.Service;
using Macss.Attributes.ActionFilter;
using Macss.Areas.Fdass.Repositories;
using Macss.Areas.Fdass.ViewModels;
//using Macss.Areas.Fdass.Models.Service;
using Macss.ViewModels;
using System.Data.Entity.Validation;
using Macss.Areas.Fdass.Common;

namespace Macss.Areas.Fdass.Controllers
{
    public class TightenController : Controller
    {
        #region 定数
        public const string StRun = "1";        // 1:実行中
        public const string StEnd = "2";        // 2:完了
        public const string StBak = "3";        // 3:退避完了
        public const string StErr = "9";        // 9:異常終了
        #endregion

        private ITightenRepository tightenRepository;
        private LogService logService;
        private ILogRepository logRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            tightenRepository = new TightenRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        // GET: Fdass/Tighten
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddMonths(-1);
            var data = await tightenRepository.GetDispData(dt.ToString("yyyyMM"));
            data.YyyyMm = dt.ToString("yyyy/MM");
            ViewBag.Mode = "1";
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Index(TightenViewModel model)
        {
            DateTime dt = DateTime.Now;
            dt = dt.AddMonths(-1);
            var data = await tightenRepository.GetDispData(dt.ToString("yyyyMM"));
            data.YyyyMm = dt.ToString("yyyy/MM");
            ViewBag.Mode = "1";
            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> Tighten(TightenViewModel data)
        {
            string wMonth = data.YyyyMm.Replace("/", "");
            string check = data.YyyyMm;
            if (!DateTime.TryParse(check, out DateTime dt))
            {
                return Json(new { succsess = false, errorMsg = String.Format(Resources.Message.CE044, "処理対象年月") });
            }

            TightenViewModel outputData = await tightenRepository.GetDispData(data.YyyyMm);
            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Fdass/Tighten").Where(x => x.ActionName == "Index").FirstOrDefault();
            string prossesingId = menu.MenuId;
            string menuName = "処理機能：" + menu.TitleName;

            // 締処理
            // ステータスチェック
            if (!await tightenRepository.IsMatujimeKanriStatus(data.YyyyMm, ModelState))
            {
                string msg = string.Empty;
                foreach (var key in ViewData.ModelState.Keys)
                {
                    foreach (var e in ViewData.ModelState[key].Errors)
                    {
                        msg = e.ErrorMessage;
                        break;
                    }
                }
                return Json(new { succsess = false, errorMsg = msg });
            }

            // 受信ファイル読込
            try
            {
                if (!await tightenRepository.ReadFile(data.YyyyMm, loginUser, ModelState))
                {
                    string msg = string.Empty;
                    foreach (var key in ViewData.ModelState.Keys)
                    {
                        foreach (var e in ViewData.ModelState[key].Errors)
                        {
                            msg = e.ErrorMessage;
                            break;
                        }
                    }
                    await tightenRepository.SetStatus(wMonth, StErr, loginUser, ModelState);
                    return Json(new { succsess = false, errorMsg = msg });

                }

                // 請求データ作成
                await tightenRepository.SetSeikyuData(data.YyyyMm, prossesingId, menuName, loginUser, ModelState);

            }
            catch (DbEntityValidationException dbEx)
            {
                await tightenRepository.SetStatus(wMonth, StErr, loginUser, ModelState);
                DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                throw dbEx;

            } catch (Exception ex)
            {
                await tightenRepository.SetStatus(wMonth, StErr, loginUser, ModelState);
                throw ex;
            }

            string yymm = data.YyyyMm.Replace("/", "");
            outputData = await tightenRepository.GetDispData(yymm);
            ViewBag.Mode = "1";

            return Json(new { succsess = true });

        }

    }
}