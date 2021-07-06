using ClosedXML.Excel;
using Macss.Areas.Tass.Common;
using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.Repositories;
using Macss.Areas.Tass.ViewModels;
using Macss.Attributes.ActionFilter;
using Macss.Controllers;
using Macss.Models;
using Macss.Models.Service;
using Macss.Repositories;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Macss.Areas.Tass.Controllers.MaintHinmeiController;

namespace Macss.Areas.Tass.Controllers
{
    public class ShuukaTehaiController : Controller
    {
        private readonly int SearchLimit = 20000;

        private IShuukaTehaiRepositorie shuukaTehaiRepositorie;
        private IAccountRepository accountRepository;
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;
        private TableLockRepository tableLockRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            shuukaTehaiRepositorie = new ShuukaTehaiRepositorie(dbContext);
            accountRepository = new AccountRepository(dbContext);
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
            tableLockRepository = new TableLockRepository(dbContext);
        }

        // GET: 
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];
            object viewModel = Session.GetViewModel();
            if (viewModel != null)
            {
                if (typeof(ShuukaTehaiSerch) == viewModel.GetType() && (Referer != null && -1 != Referer.IndexOf("/ShuukaTehai")))
                {
                    ShuukaTehaiData model = new ShuukaTehaiData();
                    ShuukaTehaiSerch SerchModel = (ShuukaTehaiSerch)viewModel;
                    SerchModel.DetaileReturnFlg = 1;
                    model.Serch = SerchModel;
                    Session.RemoveViewModel();
                    return View(model);
                } else { 
                    Session.RemoveViewModel();
                }
            }

            return View();
        }

        // 一覧 明細取得
        public async Task<ActionResult> Search(string Syukno, string Sybcod, string Crtnam,
                                               string SykymdFrom, string SykymdTo,
                                               string Fsykno, string Keifno,
                                               string Tokcod, string Toknam, string Tdkcod,
                                               string Tdknam, string Tdsnam, string Tdbnam, string Tdktan, string Ktdkcd,
                                               bool Tehai, bool Tehai1, bool Tehai2, bool Entry, string Dell)

        {
            if (!Tehai && !Tehai1 && !Tehai2)
            {
                return Json(new { succsess = 2 });
            }

            IEnumerable<MAccount> dbAccount = await accountRepository.FindByCodeAsync(Session.GetUserID());
            MAccount accountData = dbAccount.First();
            IEnumerable<MControl> controlGDatas = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountGroup);
            IEnumerable<MControl> controlG = controlGDatas.Where(x => x.Kbn == accountData.GroupCd);
            MControl mControl = new MControl();
            if (controlG.Count() != 0)
            {
                mControl = controlG.First();
            }

            ShuukaTehaiSerch SerchModel = new ShuukaTehaiSerch
            {
                Syukno = Syukno == null ? null : Syukno == string.Empty ? null : Syukno,
                Sybcod = Sybcod == null ? null : Sybcod == string.Empty ? null : Sybcod,
                Crtnam = Crtnam == null ? null : Crtnam == string.Empty ? null : Crtnam,
                SykymdFrom = SykymdFrom == null ? null : SykymdFrom == string.Empty ? null : SykymdFrom,
                SykymdTo = SykymdTo == null ? null : SykymdTo == string.Empty ? null : SykymdTo,
                Fsykno = Fsykno == null ? null : Fsykno == string.Empty ? null : Fsykno,
                Keifno = Keifno == null ? null : Keifno == string.Empty ? null : Keifno,
                Tokcod = Tokcod == null ? null : Tokcod == string.Empty ? null : Tokcod,
                Toknam = Toknam == null ? null : Toknam == string.Empty ? null : Toknam,
                Tdkcod = Tdkcod == null ? null : Tdkcod == string.Empty ? null : Tdkcod,
                Tdknam = Tdknam == null ? null : Tdknam == string.Empty ? null : Tdknam,
                Tdsnam = Tdsnam == null ? null : Tdsnam == string.Empty ? null : Tdsnam,
                Tdbnam = Tdbnam == null ? null : Tdbnam == string.Empty ? null : Tdbnam,
                Tdktan = Tdktan == null ? null : Tdktan == string.Empty ? null : Tdktan,
                Ktdkcd = Ktdkcd == null ? null : Ktdkcd == string.Empty ? null : Ktdkcd,
                Tehai = Tehai,
                Tehai1 = Tehai1,
                Tehai2 = Tehai2,
                Entry = Entry,
                Group = accountData.GroupCd,
                Dell = Dell
            };

            Session.SetViewModel(SerchModel);

            // レプリケーションチェック
            bool SReplicationFlg = false;
            if (await tableLockRepository.CheckTableLock("t_unsou_shuuka_tehai_all"))
            {
                SReplicationFlg = true;
            } else if (await tableLockRepository.CheckTableLock("t_unsou_shuuka_jiseki1"))
            {
                SReplicationFlg = true;
            }
            if (SReplicationFlg)
            {
                return Json(new { succsess = 2, SReplicationFlg });
            }
            bool TReplicationFlg = await tableLockRepository.CheckTableLock("m_unsou_todokesaki");

            IEnumerable<ShuukaTehaiInformation> list = await shuukaTehaiRepositorie.GetShuukaInformationAsync(SerchModel, TReplicationFlg);

            //// ログ
            //CreateLogHistory();

            if (list == null || list.Count() <= SearchLimit)
            {
                if (list.Count() == 0)
                {
                    return Json(new { succsess = 2, TReplicationFlg });
                }
                var json = Json(new { succsess = 1, data = list, TReplicationFlg });
                json.MaxJsonLength = int.MaxValue;
                return json;
                //return Json(new { succsess = 1, data = list });
            }
            else
            {
                return Json(new { succsess = 0, TReplicationFlg });
            }
        }

        [AuthorityActionFilter]
        public async Task<ActionResult> Details(string no, string ts, string da)
        {
            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            ShuukaTehaiViewModels model = await shuukaTehaiRepositorie.GetShuukaMeisaiAsync(no, ts, da);
            ViewBag.Meisai = model.SyuknoList;
            if (model.Delflg == "1")
            {
                ViewBag.Delflg = 1;
                ModelState.AddModelError(String.Empty, "削除データです");
            }

            // ログ履歴作成
            var loginUser = Session.GetUserID();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.S1, "処理機能：" + menu.TitleName, string.Format("照会キー：{0}, {1}", no, da), logRepository);

            return View(model);
        }

        // csv出力
        public async Task<ActionResult> Csv()
        {
            var dbAccount = await accountRepository.FindByCodeAsync(Session.GetUserID());
            var targetData = dbAccount.First();
            var gFlg = targetData.GroupCd == "G000" ? true : false;

            //string tableName = "出荷情報一覧";
            string tableName = "TD_syukajyoho_" + Session.GetUserID();
            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            ShuukaTehaiSerch serchModel = (ShuukaTehaiSerch)Session.GetViewModel();
            if (serchModel == null)
            {
                return View("Index");
            }

            // ログ
            CreateLogHistory();

            IEnumerable<UnsouShuukaTehaiAll> list = await shuukaTehaiRepositorie.GetShuukaAllAsync(serchModel);
            var fs = Output(list, 1, gFlg);
            return File(fs.ToArray(),
                        "text/csv",
                        tableName + dataString + ".csv");

        }

        // excel出力
        public async Task<ActionResult> Excel()
        {
            var dbAccount = await accountRepository.FindByCodeAsync(Session.GetUserID());
            var targetData = dbAccount.First();
            var gFlg = targetData.GroupCd == "G000" ? true : false;

            //string tableName = "出荷情報一覧";
            string tableName = "TD_syukajyoho_" + Session.GetUserID();
            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            ShuukaTehaiSerch serchModel = (ShuukaTehaiSerch)Session.GetViewModel();
            if (serchModel == null)
            {
                return View("Index");
            }

            // ログ
            CreateLogHistory();

            IEnumerable<UnsouShuukaTehaiAll> list = await shuukaTehaiRepositorie.GetShuukaAllAsync(serchModel);
            var fs = Output(list, 0, gFlg);
            return File(fs.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    tableName + dataString + Extention.Excel);

        }

        internal static System.IO.MemoryStream Output(IEnumerable<UnsouShuukaTehaiAll> outputData, int extention, bool gFlg)
        {
            var fs = new System.IO.MemoryStream();

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.AddWorksheet("sheet1");

                using (var ws = wb.Worksheets.FirstOrDefault())
                {
                    OutputDetali(outputData, ws, gFlg);
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


        internal static void OutputDetali(IEnumerable<UnsouShuukaTehaiAll> list, ClosedXML.Excel.IXLWorksheet ws, bool gFlg)
        {
            int col = 1;
            ws.Cell(1, col++).Value = "出荷Ｎｏ";
            ws.Cell(1, col++).Value = "データ年月";
            ws.Cell(1, col++).Value = "出荷日";
            ws.Cell(1, col++).Value = "Ｆｅ機種";
            ws.Cell(1, col++).Value = "経費負担Ｎｏ";
            ws.Cell(1, col++).Value = "振替出荷Ｎｏ";
            if (gFlg) ws.Cell(1, col++).Value = "担当者コード";
            ws.Cell(1, col++).Value = "担当者名";
            ws.Cell(1, col++).Value = "届先コード";
            ws.Cell(1, col++).Value = "届先住所";
            ws.Cell(1, col++).Value = "届先社名";
            ws.Cell(1, col++).Value = "届先支店名";
            ws.Cell(1, col++).Value = "届先部課名";
            ws.Cell(1, col++).Value = "届先担当者名";
            ws.Cell(1, col++).Value = "届先電話番号";
            ws.Cell(1, col++).Value = "届先郵便番号";
            ws.Cell(1, col++).Value = "品名コード";
            ws.Cell(1, col++).Value = "品名";
            ws.Cell(1, col++).Value = "運送方法コード";
            ws.Cell(1, col++).Value = "運送コースコード";
            ws.Cell(1, col++).Value = "仕入先コード";
            ws.Cell(1, col++).Value = "出荷括りＮｏ";
            if (gFlg) ws.Cell(1, col++).Value = "運送区分コード";
            ws.Cell(1, col++).Value = "出荷場所コード";
            ws.Cell(1, col++).Value = "得意先コード";
            ws.Cell(1, col++).Value = "請求先コード";
            ws.Cell(1, col++).Value = "伝票区分コード";
            ws.Cell(1, col++).Value = "伝票枚数";
            ws.Cell(1, col++).Value = "特記事項";
            ws.Cell(1, col++).Value = "包装数";
            ws.Cell(1, col++).Value = "荷札発行日時";
            ws.Cell(1, col++).Value = "詰合せ代表出荷Ｎｏ";
            if (gFlg) ws.Cell(1, col++).Value = "詰合せ代表出荷Ｎｏデータ年月";
            ws.Cell(1, col++).Value = "重量";
            ws.Cell(1, col++).Value = "詰合せ包装数";
            ws.Cell(1, col++).Value = "詰合せ重量";
            ws.Cell(1, col++).Value = "運賃負担";
            if (gFlg) ws.Cell(1, col++).Value = "輸送作業伝票Ｎｏ";
            if (gFlg) ws.Cell(1, col++).Value = "制御フラグ１９";
            if (gFlg) ws.Cell(1, col++).Value = "制御フラグ２８";
            if (gFlg) ws.Cell(1, col++).Value = "制御フラグ２９";
            if (gFlg) ws.Cell(1, col++).Value = "運賃明細書発行区分";
            if (gFlg) ws.Cell(1, col++).Value = "実績作成区分";
            ws.Cell(1, col++).Value = "到着日";
            ws.Cell(1, col++).Value = "サイズ";
            if (gFlg) ws.Cell(1, col++).Value = "請求区分";
            if (gFlg) ws.Cell(1, col++).Value = "月報区分";
            if (gFlg) ws.Cell(1, col++).Value = "ＰＣコード";
            ws.Cell(1, col++).Value = "仕入先原票Ｎｏ２";
            if (gFlg) ws.Cell(1, col++).Value = "出荷統合コード";
            ws.Cell(1, col++).Value = "県コード";
            ws.Cell(1, col++).Value = "ＪＩＳコード";
            if (gFlg) ws.Cell(1, col++).Value = "振替得意先コード";
            if (gFlg) ws.Cell(1, col++).Value = "仕分コード";
            ws.Cell(1, col++).Value = "運送会社仕入先原票Ｎｏ";
            if (gFlg) ws.Cell(1, col++).Value = "エネルギー対象判断Ｆ";
            ws.Cell(1, col++).Value = "出荷数";
            if (gFlg) ws.Cell(1, col++).Value = "出力対象フラグ";
            ws.Cell(1, col++).Value = "削除フラグ";
            if (gFlg) ws.Cell(1, col++).Value = "登録担当";
            ws.Cell(1, col++).Value = "登録日";
            if (gFlg) ws.Cell(1, col++).Value = "更新担当";
            ws.Cell(1, col++).Value = "更新日";

            int row = 2;
            col = 1;
            foreach (var data in list)
            {
                col = 1;
                // 出荷Ｎｏ
                DataUtil.SetExelString(ws.Cell(row, col), data.Syukno);
                col++;
                // データ年月
                ws.Cell(row, col).Value = data.Dataym;
                col++;
                // 出荷日
                if (data.Sykymd != null)
                {
                    if (data.Sykymd.ToString() != "2000/01/01 0:00:00")
                    {
                        DateTime Sykymd = (DateTime)data.Sykymd;
                        //ws.Cell(row, col).Value = Sykymd.ToString("yyyy/MM/dd");
                        ws.Cell(row, col).SetValue(Sykymd.ToString("yyyy/MM/dd"));
                    }
                }
                col++;
                // Ｆｅ機種
                DataUtil.SetExelString(ws.Cell(row, col), data.Kisyu);
                col++;
                // 経費負担Ｎｏ
                DataUtil.SetExelString(ws.Cell(row, col), data.Keifno);
                col++;
                // 振替出荷Ｎｏ
                DataUtil.SetExelString(ws.Cell(row, col), data.Fsykno);
                col++;
                if (gFlg)
                {
                    // 担当者コード
                    DataUtil.SetExelString(ws.Cell(row, col), data.Tancod);
                    col++;
                }
                // 担当者名
                ws.Cell(row, col).Value = data.Tannam;
                col++;
                // 届先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Tdkcod);
                col++;
                // 届先住所
                ws.Cell(row, col).Value = data.Tdkjyu;
                col++;
                // 届先社名
                ws.Cell(row, col).Value = data.Tdknam;
                col++;
                // 届先支店名
                ws.Cell(row, col).Value = data.Tdsnam;
                col++;
                // 届先部課名
                ws.Cell(row, col).Value = data.Tdbnam;
                col++;
                // 届先担当者名
                ws.Cell(row, col).Value = data.Tdktan;
                col++;
                // 届先電話番号
                DataUtil.SetExelString(ws.Cell(row, col), data.Tdktel);
                col++;
                // 届先郵便番号
                DataUtil.SetExelString(ws.Cell(row, col), data.Tdkyub);
                col++;
                // 品名コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Hincod);
                col++;
                // 品名
                ws.Cell(row, col).Value = data.Hinnam;
                col++;
                // 運送方法コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Unscod);
                col++;
                // 運送コースコード
                DataUtil.SetExelString(ws.Cell(row, col), data.Unscrs);
                col++;
                // 仕入先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Sircod);
                col++;
                // 出荷括りＮｏ
                DataUtil.SetExelString(ws.Cell(row, col), data.Sgenno);
                col++;
                if (gFlg)
                {
                    // 運送区分コード
                    DataUtil.SetExelString(ws.Cell(row, col), data.Unskbn);
                    col++;
                }
                // 出荷場所コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Sybcod);
                col++;
                // 得意先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Tokcod);
                col++;
                // 請求先コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Seicod);
                col++;
                // 伝票区分コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Denkbn);
                col++;
                // 伝票枚数
                ws.Cell(row, col).Value = data.Denmsu;
                col++;
                // 特記事項
                ws.Cell(row, col).Value = data.Tkjiko;
                col++;
                // 包装数
                ws.Cell(row, col).Value = data.Hososu;
                col++;
                // 荷札発行日時
                if (data.Nfdate != null)
                {
                    if (data.Nfdate.ToString() != "2000/01/01 0:00:00")
                    {
                        DateTime Nfdate = (DateTime)data.Nfdate;
                        //ws.Cell(row, col).Value = Sykymd.ToString("yyyy/MM/dd");
                        ws.Cell(row, col).SetValue(Nfdate.ToString("yyyy/MM/dd HH:mm:ss"));
                    }
                }
                //ws.Cell(row, col).Value = data.Nfdate;
                col++;
                // 詰合せ代表出荷Ｎｏ
                DataUtil.SetExelString(ws.Cell(row, col), data.Daihno);
                col++;
                if (gFlg)
                {

                    // 詰合せ代表出荷Ｎｏデータ年月
                    ws.Cell(row, col).Value = data.Daihnoym;
                    col++;
                }
                // 重量
                ws.Cell(row, col).Value = data.Jyuryo;
                col++;
                // 詰合せ包装数
                ws.Cell(row, col).Value = data.Hosos3;
                col++;
                // 詰合せ重量
                ws.Cell(row, col).Value = data.Jyury3;
                col++;
                // 運賃負担
                ws.Cell(row, col).Value = data.Ufutan;
                col++;
                if (gFlg)
                {
                    // 輸送作業伝票Ｎｏ
                    DataUtil.SetExelString(ws.Cell(row, col), data.Yusono);
                    col++;
                    // 制御フラグ１９
                    ws.Cell(row, col).Value = data.Ctlf19;
                    col++;
                    // 制御フラグ２８
                    ws.Cell(row, col).Value = data.Ctlf28;
                    col++;
                    // 制御フラグ２９
                    ws.Cell(row, col).Value = data.Ctlf29;
                    col++;
                    // 運賃明細書発行区分
                    ws.Cell(row, col).Value = data.Mehkbn;
                    col++;
                    // 実績作成区分
                    ws.Cell(row, col).Value = data.Jskkbn;
                    col++;
                }
                // 到着日
                if (data.Tocymd != null)
                {
                    if (data.Tocymd.ToString() != "2000/01/01 0:00:00")
                    {
                        DateTime Tocymd = (DateTime)data.Tocymd;
                        //ws.Cell(row, col).Value = Tocymd.ToString("yyyy/MM/dd");
                        ws.Cell(row, col).SetValue(Tocymd.ToString("yyyy/MM/dd"));
                    }
                }
                col++;
                // サイズ
                ws.Cell(row, col).Value = data.Taksiz;
                col++;
                if (gFlg)
                {
                    // 請求区分
                    DataUtil.SetExelString(ws.Cell(row, col), data.Seikyu);
                    col++;
                    // 月報区分
                    DataUtil.SetExelString(ws.Cell(row, col), data.Geppou);
                    col++;
                    // ＰＣコード
                    DataUtil.SetExelString(ws.Cell(row, col), data.Pccod);
                    col++;
                }
                // 仕入先原票Ｎｏ２
                DataUtil.SetExelString(ws.Cell(row, col), data.Sgenn2);
                col++;
                if (gFlg)
                {
                    // 出荷統合コード
                    DataUtil.SetExelString(ws.Cell(row, col), data.Stoucd);
                    col++;
                }
                // 県コード
                DataUtil.SetExelString(ws.Cell(row, col), data.Kencod);
                col++;
                // ＪＩＳコード
                DataUtil.SetExelString(ws.Cell(row, col), data.Jiscod);
                col++;
                if (gFlg)
                {
                    // 振替得意先コード
                    DataUtil.SetExelString(ws.Cell(row, col), data.Tensir);
                    col++;
                    // 仕分コード
                    DataUtil.SetExelString(ws.Cell(row, col), data.Hatcod);
                    col++;
                }
                // 運送会社仕入先原票Ｎｏ
                DataUtil.SetExelString(ws.Cell(row, col), data.Sgen35);
                col++;
                if (gFlg)
                {
                    // エネルギー対象判断Ｆ
                    ws.Cell(row, col).Value = data.Ecoflg;
                    col++;
                }
                // 出荷数
                ws.Cell(row, col).Value = data.Syuksu;
                col++;
                if (gFlg)
                {
                    // 出力対象フラグ
                    ws.Cell(row, col).Value = data.Syutuf;
                    col++;
                }
                // 削除フラグ
                ws.Cell(row, col).Value = data.Delflg;
                col++;
                if (gFlg)
                {
                    // 登録担当
                    DataUtil.SetExelString(ws.Cell(row, col), data.Crtcod);
                    col++;
                }
                // 登録日
                if (data.Crtymd != null)
                {
                    if (data.Crtymd.ToString() != "2000/01/01 0:00:00")
                    {
                        ws.Cell(row, col).Style.NumberFormat.SetFormat("yyyy/mm/dd HH:mm:ss");
                        ws.Cell(row, col).Value = data.Crtymd;
                    }
                }
                col++;
                if (gFlg)
                {
                    // 更新担当
                    DataUtil.SetExelString(ws.Cell(row, col), data.Updcod);
                    col++;
                }
                // 更新日
                if (data.Updymd != null)
                {
                    if (data.Updymd.ToString() != "2000/01/01 0:00:00")
                    {
                        ws.Cell(row, col).Style.NumberFormat.SetFormat("yyyy/mm/dd HH:mm:ss");
                        ws.Cell(row, col).Value = data.Updymd;
                    }
                }
                col++;
                row++;
            }
        }

        private void CreateLogHistory()
        {

            ShuukaTehaiSerch serchModel = (ShuukaTehaiSerch)Session.GetViewModel();
            if (serchModel == null) return;
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(" 出荷No：{0} ,", serchModel.Syukno));
            sb.Append(string.Format(" 出荷場所コード：{0} ,", serchModel.Sybcod));
            sb.Append(string.Format(" 登録担当者名：{0} ,", serchModel.Crtnam));
            sb.Append(string.Format(" 出荷日（From）：{0} ,", serchModel.SykymdFrom));
            sb.Append(string.Format(" 出荷日（to）：{0} ,", serchModel.SykymdTo));
            sb.Append(string.Format(" 振替出荷No：{0} ,", serchModel.Fsykno));
            sb.Append(string.Format(" 経費負担No：{0} ,", serchModel.Keifno));
            sb.Append(string.Format(" 得意先コード：{0} ,", serchModel.Tokcod));
            sb.Append(string.Format(" 得意先名：{0} ,", serchModel.Toknam));
            sb.Append(string.Format(" 届先コード：{0} ,", serchModel.Tdkcod));
            sb.Append(string.Format(" 届先支店名：{0} ,", serchModel.Tdsnam));
            sb.Append(string.Format(" 届先部課名：{0} ,", serchModel.Tdbnam));
            sb.Append(string.Format(" 届先担当者名：{0} ,", serchModel.Tdktan));
            sb.Append(string.Format(" 出荷手配：{0} ,", serchModel.Tehai.ToString()));
            sb.Append(string.Format(" 出荷実績（当月）：{0} ,", serchModel.Tehai1.ToString()));
            sb.Append(string.Format(" 出荷実績（前々月～前月）：{0} ,", serchModel.Tehai2.ToString()));
            sb.Append(string.Format(" 出力対象２：{0} ", serchModel.Entry.ToString()));

            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Tass/ShuukaTehai").Where(x => x.ActionName == "Index").FirstOrDefault();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.O1, "処理機能：" + menu.TitleName, string.Format("検索条件：{0}", sb.ToString()), logRepository);

        }

        // レプリケーションチェック
        [HttpPost]
        public async Task<ActionResult> CheckReplication()
        {
            bool Replication = false;
            if (await tableLockRepository.CheckTableLock("t_unsou_shuuka_tehai_all"))
            {
                Replication = true;
            }
            else if (await tableLockRepository.CheckTableLock("t_unsou_shuuka_jiseki1"))
            {
                Replication = true;
            }

            return Json(new { Replication });
        }
    }
}