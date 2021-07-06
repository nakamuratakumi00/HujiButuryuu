using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Models.Service;
using Macss.Repositories;
using Macss.ViewModels;
using System.Collections.Generic;
using System.Text;

namespace Macss.Controllers
{
    public class MaintenanceController : Controller
    {
        private IControlRepository controlRepository;
        private IMaintenanceRepository maintenanceRepository;
        private ILogRepository logRepository;
        private LogService logService;

        #region 定数

        public static class Extention
        {
            public const string Excel = ".xlsx";
            public const string CSV = ".csv";
        }

        #endregion

        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            // マスタリストを設定
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            ViewBag.MasterList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.MasterMaintenance))
                                    .Select(x => new SelectListItem() { Text = x.Value1, Value = x.Kbn });

            return View();
        }

        [HttpPost]
        [AuthorityActionFilter]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(HttpPostedFileWrapper uploadFile, MaintenanceViewModels data, string import, string output, int format)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            try
            {
                var extention = format; // 0:Excel、1:csv
                var loginUser = Session.GetUserID();
                var prossesingId = Session.GetProcessingID();
                logService = new LogService();
                logRepository = new LogRepository(dbContext);
                var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
                MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
                string purpose1 = "処理機能：" + menu.TitleName;
                string purpose2 = "検索条件： ";

                var masterList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.MasterMaintenance));
                ViewBag.MasterList = masterList.Select(x => new SelectListItem() { Text = x.Value1, Value = x.Kbn });

                var targetData = masterList.FirstOrDefault(x => x.Kbn == data.Master);
                maintenanceRepository = new MaintenanceRepository(dbContext);
                var user = Session.GetUserID();

                if (!string.IsNullOrEmpty(output))
                {

                    // 出力処理
                    string where = string.Empty;
                    if (data.Hincod != null)
                    {
                        where = " WHERE HINCOD = '" + data.Hincod + "'";
                        purpose2 = purpose2 + data.Hincod;
                    }

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.O1, purpose1, targetData.Value1 + "：" + purpose2, logRepository);

                    var fs = MaintenanceService.Output(targetData, where,  maintenanceRepository, extention);
                    //対象データが存在しない場合
                    if (fs == null)
                    {
                        ModelState.AddModelError(string.Empty, Resources.Message.CE019);  //対象のデータがありません。
                        return View();

                    }
                    var tableName = targetData.Value2;
                    if (tableName == "m_hokan_keiyaku")
                    {
                        tableName = "FM_keiyaku_" + loginUser;
                    }
                    else if (tableName == "m_hokan_bumon")
                    {
                        tableName = "FM_bumon_" + loginUser;
                    }
                    else if (tableName == "m_hokan_jouken")
                    {
                        tableName = "FM_jyoken_" + loginUser;
                    }
                    else if (tableName == "m_hokan_tanka")
                    {
                        tableName = "FM_tanka_" + loginUser;
                    }
                    else if (tableName == "m_hokan_seikyuusaki_change")
                    {
                        tableName = "FM_seikyuhenko_" + loginUser;
                    }
                    else if (tableName == "m_hokan_seihin")
                    {
                        tableName = "FM_seihin_" + loginUser;
                    }

                    string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                    if (extention == 0)
                    {
                        return File(fs.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        tableName + dataString + Extention.Excel);
                    }
                    else
                    {

                        return File(fs.ToArray(),
                        "text/csv",
                        tableName + dataString + Extention.CSV);
                    }
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
                            errorMmsg.Append(targetData.Value1 + "：取込エラー：");
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
                            fKbn = ControlRepository.MControlFunctionKbn.I1;
                            purpose2 = targetData.Value1 + "：" + MakeMessage(model);
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

            return sb.ToString();
        }
    }
}