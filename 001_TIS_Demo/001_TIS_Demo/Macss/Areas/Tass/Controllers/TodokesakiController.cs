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
using System.Text;
using System.IO;
using Macss.Areas.Tass.Common;
using Macss.App_Start;
using Macss.ViewModels;

namespace Macss.Areas.Tass.Controllers
{
    public class TodokesakiController : Controller
    {
        private readonly int SearchLimit = 20000;

        #region 定数
        public const string StrInquiry = "1";       // 1:照会
        public const string StrEdit = "2";          // 2:更新
        public const string StrDelete = "3";        // 3:削除
        public const string StrCsv = "8";           // 8:Csv
        public const string StrExcel = "9";         // 9:Excel
        #endregion

        ITodokesakiRepositorie todokesakiRepositorie;
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;
        private ApplicationUserManager userManager;
        private TableLockRepository tableLockRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            todokesakiRepositorie = new TodokesakiRepositorie(dbContext);
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
            userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            tableLockRepository = new TableLockRepository(dbContext);
        }

        // 一覧 表示
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];
            TodokesakiData todokesakiData = new TodokesakiData();
            object model = Session.GetViewModel();
            if (model != null && (typeof(TodokesakiSerch) != model.GetType() || (Referer != null && - 1 == Referer.IndexOf("/Todokesaki"))))
            {
                Session.RemoveViewModel();
            }
            else if (model != null && typeof(TodokesakiSerch) == model.GetType())
            {
                TodokesakiSerch serch = (TodokesakiSerch)model;
                todokesakiData.Serch = serch;
                Session.RemoveViewModel();
            }

            return View(todokesakiData);
        }

        // 一覧 明細取得
        [HttpPost]
        public async Task<ActionResult> Search(string Tdknmk, string Tdkcod, string Ktdkcd, string Tdknam, string Tdknms, string Tdbnam, string Tdktan, string Jyusyo, string Dtmoto,
                                               string Sdek01, string Sdek02, string Sdek03, string Sdek04, string Sdek05, string Sdek06, string Sdek07,
                                               string Sdek08, string Sdek09, string Sdek10, string Sdek11, string Sdek12, string Sdek13, string Sdek14, string Sdek15,
                                               string CuCodCh, string CuName, string CuDateCh, string CuFrom, string CuTFrom, 
                                               string CuTo, string CuTTo, bool Del)
        {
            TodokesakiSerch viewModel = new TodokesakiSerch
            {
                Tdknmk = Tdknmk == string.Empty ? null : Tdknmk,
                Tdkcod = Tdkcod == string.Empty ? null : Tdkcod,
                Ktdkcd = Ktdkcd == string.Empty ? null : Ktdkcd,
                Tdknam = Tdknam == string.Empty ? null : Tdknam,
                Tdknms = Tdknms == string.Empty ? null : Tdknms,
                Tdbnam = Tdbnam == string.Empty ? null : Tdbnam,
                Tdktan = Tdktan == string.Empty ? null : Tdktan,
                Jyusyo = Jyusyo == string.Empty ? null : Jyusyo,
                Dtmoto = Dtmoto == string.Empty ? null : Dtmoto,
                Sdek01 = Sdek01 == string.Empty ? null : Sdek01,
                Sdek02 = Sdek02 == string.Empty ? null : Sdek02,
                Sdek03 = Sdek03 == string.Empty ? null : Sdek03,
                Sdek04 = Sdek04 == string.Empty ? null : Sdek04,
                Sdek05 = Sdek05 == string.Empty ? null : Sdek05,
                Sdek06 = Sdek06 == string.Empty ? null : Sdek06,
                Sdek07 = Sdek07 == string.Empty ? null : Sdek07,
                Sdek08 = Sdek08 == string.Empty ? null : Sdek08,
                Sdek09 = Sdek09 == string.Empty ? null : Sdek09,
                Sdek10 = Sdek10 == string.Empty ? null : Sdek10,
                Sdek11 = Sdek11 == string.Empty ? null : Sdek11,
                Sdek12 = Sdek12 == string.Empty ? null : Sdek12,
                Sdek13 = Sdek13 == string.Empty ? null : Sdek13,
                Sdek14 = Sdek14 == string.Empty ? null : Sdek14,
                Sdek15 = Sdek15 == string.Empty ? null : Sdek15,
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
            if (await tableLockRepository.CheckTableLock("m_unsou_todokesaki"))
            {
                return Json(new { succsess = 3 });
            }

            var list = await todokesakiRepositorie.GetTodokesakisAsync(viewModel);

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
        public ActionResult Index(TodokesakiData todokesakiData)
        {
            string proc = todokesakiData.Mode;

            if (proc == StrEdit || proc == StrDelete)
            {
                // エラーチェック
                var errors = new List<(string field, string message)>();

                // データ管理元チェック
                if (todokesakiData.Information.Dtmoto != "4" && todokesakiData.Information.Dtmoto != "7")
                {
                    if (proc == StrEdit)
                    {
                        //errors.Add((String.Empty, "データ管理元が「7」以外の為、更新できません。"));
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "データ管理元", "「4」「7」以外の為、更新できません"))); //{0}：　{1}。

                    }
                    if (proc == StrDelete)
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "データ管理元", "「4」「7」以外の為、削除できません"))); //{0}：　{1}。
                        //errors.Add((String.Empty, "データ管理元が「7」以外の為、削除できません。"));
                    }
                }

                if (todokesakiData.Information.Del == "1")
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

                    return View(todokesakiData);
                }
            }

            // 画面遷移
            switch (proc)
            {
                case StrInquiry:
                    return this.Redirect("Todokesaki/Inquiry?Cd=" + todokesakiData.Information.Tdkcod + "&Proc=1");
                case StrEdit:
                    return this.Redirect("Todokesaki/Edit?Cd=" + todokesakiData.Information.Tdkcod + "&Proc=2");
                case StrDelete:
                    return this.Redirect("Todokesaki/Delete?Cd=" + todokesakiData.Information.Tdkcod + "&Proc=3");
                case StrCsv:
                    return this.Redirect("Todokesaki/OutputCsv");
                case StrExcel:
                    return this.Redirect("Todokesaki/OutputExcel");
                default:
                    return View();
            }
        }

        // 一覧 Excel出力
        public async Task<ActionResult> OutputExcel()
        {

            // データ取得
            IEnumerable<TodokesakiFileInformation> outputData = await getOutputDataAsync(0);

            // Excel出力
            var fs = Output(outputData, 0);
            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(fs.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "TM_TODOKE_" + Session.GetUserID() + dataString + ".xlsx");

        }

        // 一覧 Csv出力
        public async Task<ActionResult> OutputCsv()
        {

            // データ取得
            IEnumerable<TodokesakiFileInformation> outputData = await getOutputDataAsync(1);

            // Csv出力
            var fs = Output(outputData, 1);
            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(fs.ToArray(),
                        "text/csv",
                        "TM_TODOKE_" + Session.GetUserID() + dataString + ".csv");

        }

        private async Task<IEnumerable<TodokesakiFileInformation>> getOutputDataAsync(int extention)
        {

            object model = Session.GetViewModel();
            TodokesakiSerch serch = new TodokesakiSerch();
            if (model != null && typeof(TodokesakiSerch) == model.GetType())
            {
                serch = (TodokesakiSerch)model;
            }

            // ログ履歴作成
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" 社名（カナ）：{0} ,", serch.Tdknmk));
            sb.Append(string.Format(" 届先コード：{0} ,", serch.Tdkcod));
            sb.Append(string.Format(" 顧客届先コード：{0} ,", serch.Ktdkcd));
            sb.Append(string.Format(" 社名：{0} ,", serch.Tdknam));
            sb.Append(string.Format(" 支店名：{0} ,", serch.Tdknms));
            sb.Append(string.Format(" 部課名：{0} ,", serch.Tdbnam));
            sb.Append(string.Format(" 担当者名：{0} ,", serch.Tdktan));
            sb.Append(string.Format(" 住所：{0} ,", serch.Jyusyo));
            sb.Append(string.Format(" データ管理元：{0} ,", serch.Dtmoto));
            sb.Append(string.Format(" 担当者選択：{0} ,", serch.CuCodCh));
            sb.Append(string.Format(" 担当者：{0} ,", serch.CuName));
            sb.Append(string.Format(" 日選択：{0} ,", serch.CuDateCh));
            sb.Append(string.Format(" 日付From：{0} ,", serch.CuFrom));
            sb.Append(string.Format(" 時分From：{0} ,", serch.CuTFrom));
            sb.Append(string.Format(" 日付日To：{0} ,", serch.CuTo));
            sb.Append(string.Format(" 時分To：{0} ,", serch.CuTTo));
            sb.Append(string.Format(" 削除済のみ表示：{0} ,", serch.Del));
            sb.Append(string.Format(" 検索FLG：{0} ,", serch.SearchFlg));

            string funcNm = extention == 0 ? "：Excel出力" : "：CSV出力";
            var loginUser = Session.GetUserID();
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.O1, "処理機能：" + menu.TitleName + funcNm, string.Format("検索条件：{0}", sb.ToString()), logRepository);

            // データ取得
            return await todokesakiRepositorie.GetTodokesakiExcelAsync(serch);

        }

        internal static System.IO.MemoryStream Output(IEnumerable<TodokesakiFileInformation> outputData, int extention)
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
                        if (extention == 0)
                        {
                            wb.SaveAs(fs);
                            fs.Position = 0;

                            return fs;
                        } else
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
        internal static void OutputDetali(IEnumerable<TodokesakiFileInformation> list, ClosedXML.Excel.IXLWorksheet ws)
        {
            ws.Cell(1, 1).Value = "TDKCOD";
            ws.Cell(1, 2).Value = "TDKNAM";
            ws.Cell(1, 3).Value = "TDKNMS";
            ws.Cell(1, 4).Value = "TDKNMK";
            ws.Cell(1, 5).Value = "TDBNAM";
            ws.Cell(1, 6).Value = "TDKTAN";
            ws.Cell(1, 7).Value = "JYUSYO";
            ws.Cell(1, 8).Value = "TDKTEL";
            ws.Cell(1, 9).Value = "SDEK01";
            ws.Cell(1, 10).Value = "SDEK02";
            ws.Cell(1, 11).Value = "SDEK03";
            ws.Cell(1, 12).Value = "SDEK04";
            ws.Cell(1, 13).Value = "SDEK05";
            ws.Cell(1, 14).Value = "SDEK06";
            ws.Cell(1, 15).Value = "SDEK07";
            ws.Cell(1, 16).Value = "SDEK08";
            ws.Cell(1, 17).Value = "SDEK09";
            ws.Cell(1, 18).Value = "SDEK10";
            ws.Cell(1, 19).Value = "SDEK11";
            ws.Cell(1, 20).Value = "SDEK12";
            ws.Cell(1, 21).Value = "SDEK13";
            ws.Cell(1, 22).Value = "SDEK14";
            ws.Cell(1, 23).Value = "SDEK15";
            ws.Cell(1, 24).Value = "TKJIKO";
            ws.Cell(1, 25).Value = "DTMOTO";
            ws.Cell(1, 26).Value = "YUBINN";
            ws.Cell(1, 27).Value = "FAXNO";
            ws.Cell(1, 28).Value = "KTDKCD";
            ws.Cell(1, 29).Value = "KITDCD";
            ws.Cell(1, 30).Value = "DELFLG";
            ws.Cell(1, 31).Value = "CRTCOD";
            ws.Cell(1, 32).Value = "CRTYMD";
            ws.Cell(1, 33).Value = "UPDCOD";
            ws.Cell(1, 34).Value = "UPDYMD";
            ws.Cell(1, 35).Value = "updateType";

            ws.Cell(2, 1).Value = "届先コード**";
            ws.Cell(2, 2).Value = "届先名社名（漢字）*";
            ws.Cell(2, 3).Value = "届先名支店名（漢字）";
            ws.Cell(2, 4).Value = "届先名社名（カナ）*";
            ws.Cell(2, 5).Value = "届先部課名";
            ws.Cell(2, 6).Value = "届先担当者名";
            ws.Cell(2, 7).Value = "届先住所（漢字）*";
            ws.Cell(2, 8).Value = "届先電話番号";
            ws.Cell(2, 9).Value = "使用区分_Ｈ";
            ws.Cell(2, 10).Value = "使用区分_ｉ";
            ws.Cell(2, 11).Value = "使用区分_Ｍ";
            ws.Cell(2, 12).Value = "使用区分_Ｐ";
            ws.Cell(2, 13).Value = "使用区分_集";
            ws.Cell(2, 14).Value = "使用区分_構";
            ws.Cell(2, 15).Value = "使用区分_Ｙ";
            ws.Cell(2, 16).Value = "使用区分_半";
            ws.Cell(2, 17).Value = "使用区分_長";
            ws.Cell(2, 18).Value = "使用区分_在";
            ws.Cell(2, 19).Value = "使用区分_需";
            ws.Cell(2, 20).Value = "使用区分_顧";
            ws.Cell(2, 21).Value = "使用区分_得";
            ws.Cell(2, 22).Value = "使用区分_ブ";
            ws.Cell(2, 23).Value = "使用区分_基";
            ws.Cell(2, 24).Value = "特記事項";
            ws.Cell(2, 25).Value = "データ管理元**";
            ws.Cell(2, 26).Value = "郵便番号";
            ws.Cell(2, 27).Value = "ＦＡＸ番号";
            ws.Cell(2, 28).Value = "顧客届先コード";
            ws.Cell(2, 29).Value = "基幹届先コード";
            ws.Cell(2, 30).Value = "削除フラグ";
            ws.Cell(2, 31).Value = "登録担当";
            ws.Cell(2, 32).Value = "登録日";
            ws.Cell(2, 33).Value = "更新担当";
            ws.Cell(2, 34).Value = "更新日";
            ws.Cell(2, 35).Value = "更新区分";

            int row = 3;
            int col = 1;

            foreach (var data in list)
            {
                col = 1;
                // 届先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.TDKCOD);
                col++;
                // 届先名社名（漢字）
                ws.Cell(row, col).Value = data.TDKNAM;
                col++;
                // 届先名支店名（漢字）
                ws.Cell(row, col).Value = data.TDKNMS;
                col++;
                // 届先名社名（カナ）
                ws.Cell(row, col).Value = data.TDKNMK;
                col++;
                // 届先部課名
                ws.Cell(row, col).Value = data.TDBNAM;
                col++;
                // 届先担当者名
                ws.Cell(row, col).Value = data.TDKTAN;
                col++;
                // 届先住所（漢字）
                ws.Cell(row, col).Value = data.JYUSYO;
                col++;
                // 届先電話番号
                DataUtil.SetExelString(ws.Cell(row, col), data.TDKTEL);
                col++;
                // 使用区分_Ｈ
                ws.Cell(row, col).Value = data.SDEK01;
                col++;
                // 使用区分_ｉ
                ws.Cell(row, col).Value = data.SDEK02;
                col++;
                // 使用区分_Ｍ
                ws.Cell(row, col).Value = data.SDEK03;
                col++;
                // 使用区分_Ｐ
                ws.Cell(row, col).Value = data.SDEK04;
                col++;
                // 使用区分_集
                ws.Cell(row, col).Value = data.SDEK05;
                col++;
                // 使用区分_構
                ws.Cell(row, col).Value = data.SDEK06;
                col++;
                // 使用区分_Ｙ
                ws.Cell(row, col).Value = data.SDEK07;
                col++;
                // 使用区分_半
                ws.Cell(row, col).Value = data.SDEK08;
                col++;
                // 使用区分_長
                ws.Cell(row, col).Value = data.SDEK09;
                col++;
                // 使用区分_在
                ws.Cell(row, col).Value = data.SDEK10;
                col++;
                // 使用区分_需
                ws.Cell(row, col).Value = data.SDEK11;
                col++;
                // 使用区分_顧
                ws.Cell(row, col).Value = data.SDEK12;
                col++;
                // 使用区分_得
                ws.Cell(row, col).Value = data.SDEK13;
                col++;
                // 使用区分_ブ
                ws.Cell(row, col).Value = data.SDEK14;
                col++;
                // 使用区分_基
                ws.Cell(row, col).Value = data.SDEK15;
                col++;
                // 特記事項
                ws.Cell(row, col).Value = data.TKJIKO;
                col++;
                // データ管理元
                ws.Cell(row, col).Value = data.DTMOTO;
                col++;
                // 郵便番号
                DataUtil.SetExelString(ws.Cell(row, col), data.YUBINN);
                col++;
                // ＦＡＸ番号
                DataUtil.SetExelString(ws.Cell(row, col), data.FAXNO);
                col++;
                // 顧客届先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.KTDKCD);
                col++;
                // 基幹届先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.KITDCD);
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
            if (model != null && (typeof(TodokesakiSerch) == model.GetType()))
            {
                Session.RemoveViewModel();
            }

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            var userID = Session.GetUserID();
            var user = await userManager.FindByIdAsync(userID);

            TodokesakiViewModel viewModel = new TodokesakiViewModel
            {
                Dtmoto = "7",
                Mode = proc,
                Sdek12 = user.Sdek12
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

            TodokesakiViewModel viewModel = new TodokesakiViewModel();
            viewModel = (await todokesakiRepositorie.GetTodokesakiAsync(cd)).FirstOrDefault();

            // ログ履歴作成
            var loginUser = Session.GetUserID();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.S1, "処理機能：" + menu.TitleName, string.Format("照会キー：{0}", cd), logRepository);

            if (viewModel.Delfkg == "1")
            {
                ViewBag.Delflg = 1;
                ModelState.AddModelError(String.Empty, "削除データです");
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

            TodokesakiViewModel viewModel = new TodokesakiViewModel();
            viewModel = (await todokesakiRepositorie.GetTodokesakiAsync(cd)).FirstOrDefault();

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

            TodokesakiViewModel viewModel = new TodokesakiViewModel();
            viewModel = (await todokesakiRepositorie.GetTodokesakiAsync(cd)).FirstOrDefault();

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 採番
        [HttpPost]
        public ActionResult GetTdkcod()
        {
            string Tdkcod = string.Empty;
            var max_no = todokesakiRepositorie.GetTdkcod();
            Tdkcod = max_no.ToString("TD000000");

            return Json(new { succsess = Tdkcod != String.Empty ? 1 : 0, Tdkcod });
        }

        // 郵便番号コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetJyusyo(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    Jyusyo = string.Empty
                });
            }
            else
            {
                var list = (await todokesakiRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Yubinn == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    Jyusyo = list == null ? string.Empty : ((list.Jyusy1 ?? string.Empty) + (list.Jyusy2 ?? string.Empty)).Replace("　", "").Replace(" ", "")
                });
            }
        }

        // 新規・修正・削除
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(TodokesakiViewModel todokesaki)
        {
            string mode = todokesaki.Mode;
            string loginUser = Session.GetUserID();

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
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Tass/Todokesaki").Where(x => x.ActionName == actionName).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            try
            {
                // エラーチェック
                var errors = new List<(string field, string message)>();

                if (mode == null)
                {
                    // 新規
                    // 未採番
                    if (todokesaki.NumberingFlg != 1)
                    {
                        // 8桁　かつ　先頭２文字が"TD"の場合はエラー
                        if (todokesaki.NumberingFlg != 1 && todokesaki.Tdkcod.Length == 8 && todokesaki.Tdkcod.StartsWith("TD"))
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "届先コード", "(8桁)の先頭に「TD」は使用できません"))); //{0}：　{1}。
                            //errors.Add((String.Empty, string.Format(Resources.Message.CE033, "届先コード(8桁)の先頭", "「TD」は使用"))); //{0}に{1}できません。
                        }
                        // 重複チェック
                        var chkData = await todokesakiRepositorie.GetTodokesakiAsync(todokesaki.Tdkcod);
                        if (chkData.Count() > 0)
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "届先コード","重複しています"))); //{0}：　{1}。
                            //errors.Add((String.Empty, string.Format(Resources.Message.CE057, "届先コード"))); //{0}が重複しています。
                        }
                    }
                }
                if (mode == null | mode == StrEdit)
                {
                    // 新規・更新
                    // 郵便番号、住所整合性チェック
                    if (!string.IsNullOrEmpty(todokesaki.Yubinn))
                    {
                        var Tdkjyu = todokesaki.Jyusyo.Replace("　", "").Replace(" ", "");
                        //var spIdx = Common.DataUtil.GetNumIndex(Tdkjyu);
                        //var jyusy12 = Tdkjyu.Substring(0, spIdx);
                        //var jyusyo = (await todokesakiRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Jyusy1 + x.Jyusy2 == jyusy12).FirstOrDefault();
                        //var jyusyo = await CheckJyusyAsync(Tdkjyu, todokesaki.Yubinn);
                        //if (jyusyo == null || todokesaki.Yubinn != jyusyo.Yubinn)
                        if (!await CheckJyusyAsync(Tdkjyu, todokesaki.Yubinn))
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "郵便番号・住所","郵便番号と住所が一致しません"))); //{0}：　{1}。
                            //errors.Add((string.Empty, string.Format(Resources.Message.CE035, "郵便番号と住所"))); //{0}が一致しません。

                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Mode = todokesaki.Mode;
                    return View(todokesaki);
                }
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(String.Empty, message);
                    }
                    ViewBag.Mode = todokesaki.Mode;
                    return View(todokesaki);
                }

                // DB更新
                if (mode == null)
                {
                    // 新規
                    await todokesakiRepositorie.InsertTodokesakiAsync(todokesaki, loginUser);

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U1, "処理機能：" + menu.TitleName, string.Format("追加キー：{0}", todokesaki.Tdkcod), logRepository);
                }
                else if (mode.Equals(StrEdit))
                {
                    // 2:更新
                    await todokesakiRepositorie.UpdateTodokesakiAsync(todokesaki, loginUser);

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U2, "処理機能：" + menu.TitleName, string.Format("更新キー：{0}", todokesaki.Tdkcod), logRepository);
                }
                else if (mode.Equals(StrDelete))
                {
                    // 3:削除
                    await todokesakiRepositorie.DeleteTodokesakiAsync(todokesaki, loginUser);

                    // ログ履歴作成
                    logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U3, "処理機能：" + menu.TitleName, string.Format("削除キー：{0}", todokesaki.Tdkcod), logRepository);
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

        private async Task<bool> CheckJyusyAsync(string jyusy, string Yubinn)
        {

            //MUnsouPostalcode jyusyo = null;
            string jyusy3 = jyusy.Length >= 3 ? jyusy.Substring(0, 3) : jyusy;
            var jyusyo1 = (await todokesakiRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Jyusy1.Contains(jyusy3)).OrderBy(x => x.Jyusy2);
            if (jyusyo1.Count() != 0)
            {
                for (int idx = jyusy.Length; idx > 0; idx--)
                {
                    //jyusyo = jyusyo1.Where(x => x.Jyusy1 + x.Jyusy2 == jyusy.Substring(0, idx)).FirstOrDefault();
                    var jyusyos = jyusyo1.Where(x => x.Jyusy1 + x.Jyusy2 == jyusy.Substring(0, idx));
                    foreach (var check in jyusyos)
                    {
                        if (check.Yubinn == Yubinn)
                        {
                            return true;
                        }
                    }
                }

            }
            return false;

        }

        // レプリケーションチェック
        [HttpPost]
        public async Task<ActionResult> CheckReplication()
        {
            bool Replication = false;
            if (await tableLockRepository.CheckTableLock("m_unsou_todokesaki"))
            {
                Replication = true;
            }
            return Json(new { Replication });
        }

    }
}