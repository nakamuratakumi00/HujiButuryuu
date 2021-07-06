using Macss.Areas.Tass.Repositories;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using Macss.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Macss.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Attributes.ActionFilter;
using Macss.Areas.Tass.Models;
using System.Text;
using System.IO;
using Macss.Areas.Tass.Common;
using Macss.App_Start;

namespace Macss.Areas.Tass.Controllers
{
    public class HinmeiController : Controller
    {
        private readonly int SearchLimit = 20000;

        #region 定数
        public const string StrInquiry = "1";         // 1:照会
        public const string StrEdit = "2";            // 2:更新
        public const string StrDelete = "3";          // 3:削除
        public const string StrCsv = "8";           // 8:Csv
        public const string StrExcel = "9";         // 9:Excel
        #endregion

        IHinmeiRepositorie hinmeiRepositorie;
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;
        private ApplicationUserManager userManager;
        private TableLockRepository tableLockRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            hinmeiRepositorie = new HinmeiRepositorie(dbContext);
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
            userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            tableLockRepository = new TableLockRepository(dbContext);
        }

        // 一覧 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];            
            HinmeiData hinmeiData = new HinmeiData();
            object model = Session.GetViewModel();
            if (model != null && (typeof(HinmeiSerch) != model.GetType() || (Referer != null && -1 == Referer.IndexOf("/Hinmei"))))
            {
                Session.RemoveViewModel();
            }
            else if (model != null && typeof(HinmeiSerch) == model.GetType())
            {
                HinmeiSerch serch = (HinmeiSerch)model;
                hinmeiData.Serch = serch;
                Session.RemoveViewModel();
            }

            return View(hinmeiData);
        }

        // 一覧 明細取得
        [HttpPost]
        public async Task<ActionResult> Search(string Hinnmk, string Hincod, string Khincd, string Hinnam, string Ctlfl1, string Dtmoto,
                                               string CuCodCh, string CuName, string CuDateCh, string CuFrom, string CuTFrom,
                                               string CuTo, string CuTTo, bool Del)
        {
            HinmeiSerch viewModel = new HinmeiSerch
            {
                Hinnmk = Hinnmk == string.Empty ? null : Hinnmk,
                Hincod = Hincod == string.Empty ? null : Hincod,
                Khincd = Khincd == string.Empty ? null : Khincd,
                Hinnam = Hinnam == string.Empty ? null : Hinnam,
                Ctlfl1 = Ctlfl1 == string.Empty ? null : Ctlfl1,
                Dtmoto = Dtmoto == string.Empty ? null : Dtmoto,
                CuCodCh = CuCodCh == string.Empty ? null : CuCodCh,
                CuName = CuName == string.Empty ? null : CuName,
                CuDateCh = CuDateCh == string.Empty ? null : CuDateCh,
                CuFrom = CuFrom == string.Empty ? null : CuFrom,
                CuTFrom = CuTFrom == string.Empty ? null : CuTFrom,
                CuTo = CuTo == string.Empty ? null : CuTo,
                CuTTo = CuTTo == string.Empty ? null : CuTTo,
                Del = Del,
                SearchFlg = 1
            };
            Session.SetViewModel(viewModel);

            // レプリケーションチェック
            if (await tableLockRepository.CheckTableLock("m_unsou_hinmei"))
            {
                return Json(new { succsess = 3 });
            }

            var list = await hinmeiRepositorie.GetHinmeisAsync(viewModel);

            if (list == null || list.Count() <= SearchLimit)
            {
                if (list.Count() == 0)
                {
                    return Json(new { succsess = 2 });
                }
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
                //return Json(new { succsess = 1, data = list });
            }
            else
            {
                return Json(new { succsess = 0 });
            }
        }

        // 一覧 操作ボタン押下
        [HttpPost]
        [AuthorityActionFilter]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(HinmeiData hinmeiData)
        {
            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            string proc = hinmeiData.Mode;

            if (proc == StrEdit || proc == StrDelete)
            {
                // エラーチェック
                var errors = new List<(string field, string message)>();

                // データ管理元チェック
                if (hinmeiData.Information.Dtmoto != "4" && hinmeiData.Information.Dtmoto != "7")
                {
                    if (proc == StrEdit)
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "データ管理元", "「4」「7」以外の為、更新できません"))); //{0}：　{1}。
                        //errors.Add((String.Empty, "データ管理元が「7」以外の為、更新できません。"));
                    }
                    if (proc == StrDelete)
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "データ管理元", "「4」「7」以外の為、削除できません"))); //{0}：　{1}。
                        //errors.Add((String.Empty, "データ管理元が「7」以外の為、削除できません。"));
                    }
                }

                if (hinmeiData.Information.Del == "1")
                {
                    if (proc == StrEdit)
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "更新", "削除済の為、更新できません"))); //{0}：　{1}。

                    }
                    if (proc == StrDelete)
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "削除", "削除済の為、削除できません"))); //{0}：　{1}。
                    }
                }

                // エラー表示
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(String.Empty, message);
                    }

                    return View(hinmeiData);
                }
            }

            // 画面遷移
            switch (proc)
            {
                case StrInquiry:
                    return this.Redirect("Hinmei/Inquiry?Cd=" + hinmeiData.Information.Hincod + "&Proc=1");
                case StrEdit:
                    return this.Redirect("Hinmei/Edit?Cd=" + hinmeiData.Information.Hincod + "&Proc=2");
                case StrDelete:
                    return this.Redirect("Hinmei/Delete?Cd=" + hinmeiData.Information.Hincod + "&Proc=3");
                case StrCsv:
                    return this.Redirect("Hinmei/OutputCsv");
                case StrExcel:
                    return this.Redirect("Hinmei/OutputExcel");
                default:
                    return View();
            }
        }

        // 一覧 Excel出力
        public async Task<ActionResult> OutputExcel()
        {

            // データ取得
            var outputData = await GetOutputDataAsync(1);

            // Excel出力
            var fs = Output(outputData, 0);

            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(fs.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "TM_HINMEI_" + Session.GetUserID() + dataString + ".xlsx");

        }

        // 一覧 Csv出力
        public async Task<ActionResult> OutputCsv()
        {

            // データ取得
            var outputData = await GetOutputDataAsync(1);

            // Csv出力
            var fs = Output(outputData, 1);

            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(fs.ToArray(),
                "text/csv",
                "TM_HINMEI_" + Session.GetUserID() + dataString + ".csv");

        }

        private async Task<IEnumerable<HinmeiFileInformation>> GetOutputDataAsync(int extention)
        {


            HinmeiSerch serch = new HinmeiSerch();
            object model = Session.GetViewModel();
            if (model != null && typeof(HinmeiSerch) == model.GetType())
            {
                serch = (HinmeiSerch)model;
            }

            // ログ履歴作成
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" 品名カナ：{0} ,", serch.Hinnmk));
            sb.Append(string.Format(" 品名コード：{0} ,", serch.Hincod));
            sb.Append(string.Format(" 顧客品名コード：{0} ,", serch.Khincd));
            sb.Append(string.Format(" 品名：{0} ,", serch.Hinnam));
            sb.Append(string.Format(" 抽出フラグ：{0} ,", serch.Ctlfl1));
            sb.Append(string.Format(" データ管理元：{0} ,", serch.Dtmoto));
            sb.Append(string.Format(" 検索FLG：{0} ,", serch.SearchFlg));

            string funcNm = extention == 0 ? "：Excel出力" : "：CSV出力";


            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.O1, "処理機能：" + menu.TitleName + funcNm, string.Format("検索条件：{0}", sb.ToString()), logRepository);


            // データ取得
            return await hinmeiRepositorie.GetHinmeiExcelAsync(serch);

        }

        internal static System.IO.MemoryStream Output(IEnumerable<HinmeiFileInformation> outputData, int extention)
        {
            var fs = new System.IO.MemoryStream();

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.AddWorksheet("sheet1");

                using (var ws = wb.Worksheets.FirstOrDefault())
                {
                    OutputDetali(outputData, ws);
                    ws.ColumnsUsed().AdjustToContents();
                    using (fs)
                    {
                        // Excel
                        if (extention == 0)
                        {
                            wb.SaveAs(fs);
                            fs.Position = 0;

                            return fs;
                        }
                        // CSV
                        else
                        {
                            var csvWriter = new StreamWriter(fs, System.Text.Encoding.GetEncoding("shift-jis"));
                            csvWriter.Write(DataUtil.GetCsvString(wb));
                            csvWriter.Flush();
                            return fs;
                        }
                    }
                }
            }
        }
        internal static void OutputDetali(IEnumerable<HinmeiFileInformation> list, ClosedXML.Excel.IXLWorksheet ws)
        {

            ws.Cell(1, 1).Value = "HINCOD";
            ws.Cell(1, 2).Value = "HINNAM";
            ws.Cell(1, 3).Value = "HINNMK";
            ws.Cell(1, 4).Value = "TORCOD";
            ws.Cell(1, 5).Value = "KISYUA";
            ws.Cell(1, 6).Value = "KISYUB";
            ws.Cell(1, 7).Value = "DTMOTO";
            ws.Cell(1, 8).Value = "TKCOD";
            ws.Cell(1, 9).Value = "SYUBTU";
            ws.Cell(1, 10).Value = "CTLFL1";
            ws.Cell(1, 11).Value = "KHINCD";
            ws.Cell(1, 12).Value = "DELFLG";
            ws.Cell(1, 13).Value = "CRTCOD";
            ws.Cell(1, 14).Value = "CRTYMD";
            ws.Cell(1, 15).Value = "UPDCOD";
            ws.Cell(1, 16).Value = "UPDYMD";
            ws.Cell(1, 17).Value = "updateType";

            ws.Cell(2, 1).Value = "品名コード**";
            ws.Cell(2, 2).Value = "品名*";
            ws.Cell(2, 3).Value = "品名カナ*";
            ws.Cell(2, 4).Value = "得意先コード";
            ws.Cell(2, 5).Value = "Ｆｅ機種Ａ";
            ws.Cell(2, 6).Value = "Ｆｅ機種Ｂ";
            ws.Cell(2, 7).Value = "データ管理元**";
            ws.Cell(2, 8).Value = "単価コード";
            ws.Cell(2, 9).Value = "種別";
            ws.Cell(2, 10).Value = "抽出フラグ";
            ws.Cell(2, 11).Value = "顧客品名コード";
            ws.Cell(2, 12).Value = "削除フラグ";
            ws.Cell(2, 13).Value = "登録担当";
            ws.Cell(2, 14).Value = "登録日";
            ws.Cell(2, 15).Value = "更新担当";
            ws.Cell(2, 16).Value = "更新日";
            ws.Cell(2, 17).Value = "更新区分";

            int row = 3;
            int col = 1;
            foreach (var data in list)
            {
                col = 1;
                // 品名コード
                DataUtil.SetExelString(ws.Cell(row, col), data.HINCOD);
                col++;
                // 品名
                ws.Cell(row, col).Value = data.HINNAM;
                col++;
                // 品名カナ
                ws.Cell(row, col).Value = data.HINNMK;
                col++;
                // 得意先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.TORCOD);
                col++;
                // Ｆｅ機種Ａ
                DataUtil.SetExelString(ws.Cell(row, col), data.KISYUA);
                col++;
                // Ｆｅ機種Ｂ
                DataUtil.SetExelString(ws.Cell(row, col), data.KISYUB);
                col++;
                // データ管理元
                ws.Cell(row, col).Value = data.DTMOTO;
                col++;
                // 単価コード
                DataUtil.SetExelString(ws.Cell(row, col), data.TKCOD);
                col++;
                // 種別
                DataUtil.SetExelString(ws.Cell(row, col), data.SYUBTU);
                col++;
                // 抽出フラグ
                DataUtil.SetExelString(ws.Cell(row, col), data.CTLFL1);
                col++;
                // 顧客品名コード
                DataUtil.SetExelString(ws.Cell(row, col), data.KHINCD);
                col++;
                // 削除フラグ
                ws.Cell(row, col).Value = data.DELFLG;
                col++;
                // 登録担当
                ws.Cell(row, col).Value = data.CRTCOD;
                col++;
                // 登録日
                ws.Cell(row, col).Style.NumberFormat.SetFormat("yyyy/mm/dd hh:mm:ss");
                ws.Cell(row, col).Value = data.CRTYMD;
                col++;
                // 更新担当
                ws.Cell(row, col).Value = data.UPDCOD;
                col++;
                // 更新日
                ws.Cell(row, col).Style.NumberFormat.SetFormat("yyyy/mm/dd hh:mm:ss");
                ws.Cell(row, col).Value = data.UPDYMD;
                col++;

                row++;
            }
        }

        // 新規 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Details(string cd, string proc)
        {
            ViewBag.Mode = proc;

            object model = Session.GetViewModel();
            if (model != null && (typeof(HinmeiSerch) == model.GetType()))
            {
                Session.RemoveViewModel();
            }

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            var userID = Session.GetUserID();
            var user = await userManager.FindByIdAsync(userID);

            if (user != null && user.Ctlfl1 != null)
            {
                user.Ctlfl1 = user.Ctlfl1 == string.Empty ? " " : user.Ctlfl1;
            }
            HinmeiViewModel viewModel = new HinmeiViewModel
            {
                Dtmoto = "7",
                Mode = proc,
                Ctlfl1 = user.Ctlfl1
            };

            return View(viewModel);
        }

        // 照会 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Inquiry(string cd, string proc)
        {
            ViewBag.Mode = proc;

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            HinmeiViewModel viewModel = new HinmeiViewModel();
            viewModel = (await hinmeiRepositorie.GetHinmeiAsync(cd)).FirstOrDefault();
            if (!string.IsNullOrEmpty(viewModel.Torcod))
            {
                var tokuisaki = (await hinmeiRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == viewModel.Torcod).FirstOrDefault();
                if (tokuisaki != null)
                {
                    viewModel.Tornam = tokuisaki.Tornam;
                }
            }

            // ログ履歴作成
            var loginUser = Session.GetUserID();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.S1, "処理機能：" + menu.TitleName, string.Format("照会キー：{0}", cd), logRepository);

            if (viewModel.Delfkg == "1")
            {
                ViewBag.Delflg = 1;
                ModelState.AddModelError(String.Empty,"削除データです");
            }

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 更新 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Edit(string cd, string proc)
        {
            ViewBag.Mode = proc;

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            HinmeiViewModel viewModel = new HinmeiViewModel();
            viewModel = (await hinmeiRepositorie.GetHinmeiAsync(cd)).FirstOrDefault();
            if (!string.IsNullOrEmpty(viewModel.Torcod))
            {
                var tokuisaki = (await hinmeiRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == viewModel.Torcod).FirstOrDefault();
                if (tokuisaki != null)
                {
                    viewModel.Tornam = tokuisaki.Tornam;
                }
            }

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 削除 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Delete(string cd, string proc)
        {
            ViewBag.Mode = proc;

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            HinmeiViewModel viewModel = new HinmeiViewModel();
            viewModel = (await hinmeiRepositorie.GetHinmeiAsync(cd)).FirstOrDefault();
            if (!string.IsNullOrEmpty(viewModel.Torcod))
            {
                var tokuisaki = (await hinmeiRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == viewModel.Torcod).FirstOrDefault();
                if (tokuisaki != null)
                {
                    viewModel.Tornam = tokuisaki.Tornam;
                }
            }

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 採番
        [HttpPost]
        public ActionResult GetHincod()
        {
            string Hincod = string.Empty;
            var max_no = hinmeiRepositorie.GetHincod();
            Hincod = max_no.ToString("HN000000");

            return Json(new { succsess = Hincod != String.Empty ? 1 : 0, Hincod });
        }

        // 得意先コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetTorName(string param)
        {
            string torName = string.Empty;
            var result = (await hinmeiRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == param); ;
            if (result.Count() != 0)
            {
                var hinmei = result.FirstOrDefault();
                torName = hinmei.Tornam;
            }
            return Json(new { succsess = torName != String.Empty ? 1 : 0, torNam = torName });
        }

        // 新規・修正・削除
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(HinmeiViewModel hinmei)
        {
            string mode = hinmei.Mode;
            string loginUser = Session.GetUserID();

            if (hinmei.Ctlfl1 != null)
            {
                hinmei.Ctlfl1 = hinmei.Ctlfl1.Trim();
            }
            
            // タイトルセット
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            string actionName = "";
            switch (mode)
            {
                case StrEdit:
                    actionName = "Edit";
                    break;
                case StrDelete:
                    actionName = "Delete";
                    break;
                default:
                    actionName = "Details";
                    break;
            }
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Tass/Hinmei").Where(x => x.ActionName == actionName).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            try
            {
                // エラーチェック
                var errors = new List<(string field, string message)>();

                if (mode == null)
                {
                    // 新規
                    // 未採番
                    if (hinmei.NumberingFlg != 1)
                    {
                        // 8桁　かつ　先頭２文字が"TD"の場合はエラー
                        if (hinmei.NumberingFlg != 1 && hinmei.Hincod.Length == 8 && hinmei.Hincod.StartsWith("HN"))
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "品名コード", "(8桁)の先頭に「HN」は使用できません"))); //{0}：　{0}。
                            //errors.Add((String.Empty, string.Format(Resources.Message.CE033, "品名コード(8桁)の先頭", "「HN」は使用"))); //{0}に{1}できません。
                        }
                        // 重複チェック
                        var chkData = await hinmeiRepositorie.GetHinmeiAsync(hinmei.Hincod);
                        if (chkData.Count() > 0)
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "品名コード", "重複しています"))); //{0}：　{0}。
                            //errors.Add((String.Empty, string.Format(Resources.Message.CE057, "品名コード"))); //{0}が重複しています。
                        }
                    }
                }
                if (mode == null | mode == StrEdit)
                {
                    // 新規・更新
                    // 得意先コード 存在チェック
                    if (!string.IsNullOrEmpty(hinmei.Torcod))
                    {
                        var tokuisaki = (await hinmeiRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == hinmei.Torcod);
                        if (tokuisaki.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "得意先コード" , "得意先コード"))); //{0}：　マスタに存在しない{1}が設定されています。
                        }
                    }
                    // 機種A 存在チェック
                    if (!string.IsNullOrEmpty(hinmei.Kisyua))
                    {
                        var kisyu = (await hinmeiRepositorie.GetMKishuAsync()).Where(x => x.Kisyua == hinmei.Kisyua);
                        if (kisyu.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "機種Aコード", "機種Aコード"))); //{0}：　マスタに存在しない{1}が設定されています。
                        }
                    }
                }

                //if (!ModelState.IsValid)
                //{
                //    ViewBag.Mode = hinmei.Mode;
                //    return View(hinmei);
                //}
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(String.Empty, message);
                    }
                    ViewBag.Mode = hinmei.Mode;
                    return View(hinmei);
                }

                // DB更新
                if (mode == null)
                {
                    // 新規
                    await hinmeiRepositorie.InsertHinmeiAsync(hinmei, loginUser);

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U1, "処理機能：" + menu.TitleName, string.Format("追加キー：{0}", hinmei.Hincod), logRepository);
                }
                else if (mode.Equals(StrEdit))
                {
                    // 2:更新
                    await hinmeiRepositorie.UpdateHinmeiAsync(hinmei, loginUser);

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U2, "処理機能：" + menu.TitleName, string.Format("更新キー：{0}", hinmei.Hincod), logRepository);
                }
                else if (mode.Equals(StrDelete))
                {
                    // 3:削除
                    await hinmeiRepositorie.DeleteHinmeiAsync(hinmei, loginUser);

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U3, "処理機能：" + menu.TitleName, string.Format("削除キー：{0}", hinmei.Hincod), logRepository);
                }

                // 画面遷移
                if (mode == null)
                {
                    // 新規
                    return this.Redirect("Details");
                }
                else
                {
                    // 新規以外
                    return this.Redirect("Index");
                }

            }
            catch (Exception ex)
            {
                // エラー画面へ遷移
                throw ex;
            }
        }

        // レプリケーションチェック
        [HttpPost]
        public async Task<ActionResult> CheckReplication()
        {
            bool Replication = false;
            if (await tableLockRepository.CheckTableLock("m_unsou_hinmei"))
            {
                Replication = true;
            }
            return Json(new { Replication });
        }

    }
}