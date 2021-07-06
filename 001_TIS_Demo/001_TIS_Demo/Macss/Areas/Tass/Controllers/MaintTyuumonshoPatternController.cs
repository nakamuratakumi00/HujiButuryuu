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
using System.Text;
using Macss.ViewModels;

namespace Macss.Areas.Tass.Controllers
{
    public class MaintTyuumonshoPatternController : Controller
    {

        public static class Extention
        {
            public const string Excel = ".xlsx";
            public const string CSV = ".csv";
        }

        private IMaintenanceRepository maintenanceRepository;
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
        }

        // GET: Tass/MaintTyuumonshoPattern
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AuthorityActionFilter]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(HttpPostedFileWrapper uploadFile, MaintTyuumonshoPatternViewModel data, string import, string output)
        {

            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            try
            {
                var extention = 0; // 0:Excel、1:csv
                var loginUser = Session.GetUserID();
                var prossesingId = Session.GetProcessingID();
                logService = new LogService();
                var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
                MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
                string purpose1 = "処理機能：" + menu.TitleName;
                string purpose2 = "検索条件： ";

                ViewBag.Ctlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Value1, Value = x.Kbn });

                var targetData = new MControl
                {
                    Value2 = "m_unsou_shuuka_tyuumonsho_pattern",
                    Value3 = "Macss.Areas.Tass.Models.MUnsouShuukaTyuumonshoPattern"
                };
                StringBuilder sb = new StringBuilder();
                if (data.Sykno2 != null)
                {
                    sb.Append("SYKNO2 LIKE '" + data.Sykno2 + "%'");
                }
                targetData.Value1 = sb.ToString();
                maintenanceRepository = new MaintenanceRepository(dbContext);
                var user = Session.GetUserID();

                if (!string.IsNullOrEmpty(output))
                {
                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.O1, purpose1, purpose2 + targetData.Value1, logRepository);

                    // 出力処理
                    var fs = MaintenanceService.OutputUnsou(targetData, maintenanceRepository, extention);
                    //対象データが存在しない場合
                    if (fs == null)
                    {
                        ModelState.AddModelError(string.Empty, Resources.Message.CE075);  //検索結果がありません。
                        return View();

                    }
                    //var tableName = targetData.Value2;
                    var tableName = "TM_shkachumnpatan_" + loginUser;
                    string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    return File(fs.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    tableName + dataString + Extention.Excel);

                }
                else
                {
                    // 取込処理
                    if (uploadFile != null)
                    {
                        var model = new MaintenanceViewModels();
                        var errors = MaintenanceService.Import(dbContext, targetData, uploadFile, maintenanceRepository, user, ref model);
                        var fKbn = string.Empty;
                        StringBuilder errorMmsg = new StringBuilder();

                        if (errors.Count > 0)
                        {
                            errorMmsg.Append("取込エラー：");
                            foreach (var error in errors)
                            {
                                ModelState.AddModelError(String.Empty, error);
                                errorMmsg.Append(error);
                            }
                            fKbn = ControlRepository.MControlFunctionKbn.E1;
                            if (errorMmsg.Length > 500)
                            {
                                purpose2 = errorMmsg.ToString().Substring(0, 500);
                            }
                            else
                            {
                                purpose2 = errorMmsg.ToString();
                            }
                        }
                        else
                        {
                            // 処理完了
                            ViewBag.Success = 1;
                            ModelState.AddModelError(String.Empty, model.Insert + "件の登録を行いました");
                            ModelState.AddModelError(String.Empty, model.Update + "件の更新を行いました");
                            ModelState.AddModelError(String.Empty, model.Delete + "件の削除を行いました");
                            ModelState.AddModelError(String.Empty, model.Invalid + "件　対象外");
                            //ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CI003));
                            fKbn = ControlRepository.MControlFunctionKbn.I1;
                            purpose2 = MakeMessage(model);
                        }

                        // ログ履歴作成
                        logService.CreateLogHistory(loginUser, prossesingId, fKbn, purpose1, purpose2, logRepository);

                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, String.Format(Resources.Message.CE054));
                    }
                }

                return View(data);
            }
            catch (Exception ex)
            {
                // エラー画面に遷移
                throw ex;
            }
        }

        private string MakeMessage(MaintenanceViewModels model)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("追加件数：" + model.Insert + "件");
            sb.AppendLine("更新件数：" + model.Update + "件");
            sb.Append("削除件数：" + model.Delete + "件");
            sb.Append("無効件数：" + model.Invalid + "件");

            return sb.ToString();
        }
    }
}