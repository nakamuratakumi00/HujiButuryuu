using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Macss.Models;
using Macss.Repositories;
using Macss.Attributes.ActionFilter;
using Macss.Areas.Fdass.Repositories;
using Macss.Areas.Fdass.ViewModels;
using ClosedXML.Excel;
using Macss.Controllers;
using System.IO;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using System.Text;
using Macss.ViewModels;

namespace Macss.Areas.Fdass.Controllers
{
    public class EstimatesController : Controller
    {
        private IControlRepository controlRepository;
        private IEstimatesRepository estimatesRepository;
        private IMenuRepository menuRepository;
        private LogService logService;
        private ILogRepository logRepository;

        #region 定数
        public const string FileA = "Fe保管請求見積書_機種A単位_";
        public const string FileH = "Fe保管請求見積書_品番コード単位_";
        #endregion


        #region 変数
        public string strNam = String.Empty;
        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            estimatesRepository = new EstimatesRepository(dbContext);
            menuRepository = new MenuRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        // GET: Estimates/Index
        [AuthorityActionFilter]
        public ActionResult Index()
        {
            ViewBag.Mode = "1";
            return View();
        }

        // POST: Group/GetTorName
        [HttpPost]
        public async Task<ActionResult> GetTorName(string id)
        {
            string torName = string.Empty;
            var result = await estimatesRepository.GetAllEstimatesAsync(id);
            if (result.Count() != 0)
            {
                var seinam = result.First();
                torName = seinam.Seinam;
            }
            return Json(new { succsess = torName != String.Empty ? 1 : 0, torNam = torName });
        }

   
        public async Task<ActionResult> EdtMitumoriExcelTable(EstimatesViewModel model)
        {
            // ファイル削除
            await DeleteFileAsync();

            string id = model.Seicod;

            var loginUser = Session.GetUserID();

            IEnumerable<EstimatesViewModel> seikyuData = await estimatesRepository.GetAllEstimatesAsync(id);
            EstimatesCKViewModel ckCod = await estimatesRepository.EstimatesCKViewModel(id);

            string torName = string.Empty;
            string fileName = string.Empty;
            if (ckCod.Hokflg1.Equals("A") && ckCod.Nieflg1.Equals("A"))
            {

                string fileNm = await EdtMitumoriAExcelTableAsync(seikyuData, ckCod, id);
                string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                var fs = new System.IO.MemoryStream();
                var wb = new ClosedXML.Excel.XLWorkbook(fileNm);
                wb.Worksheets.First().ColumnsUsed().AdjustToContents();
                wb.SaveAs(fs);
                fs.Position = 0;
                return File(fs.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileA + loginUser + dataString + ".xlsx");

            }
            else
            {
                string fileNm = await EdtMitumoriHExcelTableAsync(seikyuData, ckCod, id);
                string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                var fs = new System.IO.MemoryStream();
                var wb = new ClosedXML.Excel.XLWorkbook(fileNm);
                wb.Worksheets.First().ColumnsUsed().AdjustToContents();
                wb.SaveAs(fs);
                fs.Position = 0;
                return File(fs.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileH + loginUser + dataString + ".xlsx");
            }

        }

        private async Task<string> EdtMitumoriAExcelTableAsync (IEnumerable<EstimatesViewModel> seikyuData, EstimatesCKViewModel ckCod, string id)
        {

            string torCod = ckCod.Fbtcod;
            string torName = seikyuData.Select(x => x.Seinam).First();
            // 機種A
            var kisyuAData = await estimatesRepository.KisyuAExcelViewModel(id);
            List<KisyuAExcelViewModel> kisyuAList = kisyuAData.ToList();
            var wkBk = new XLWorkbook(Server.MapPath("~") + @"\Areas\Fdass\Excel\EstimatesKisyuA.xlsx");
            var wkSt = wkBk.Worksheets.First();
            //顧客名
            var range = wkSt.Range("C2:D2").Merge(false);
            range.Value = torName + "　様";
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            //出力日時
            range = wkSt.Range("I2:J2").Merge(false);
            range.Value = DateTime.Now;

            var rangecp = wkSt.Range("B7:J8");

            bool flg = true;
            int row = 7;
            int col = 2;
            int ck = 1;
            int cnt = 1;
            var cells = wkSt.Cell(row, col);


            foreach (var data in kisyuAList)
            {
                col = 2;
                if (row % 2 != 0)
                {
                    rangecp.CopyTo(cells);
                    wkSt.Range(row, col, row + 1, 10).Style
                        .Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wkSt.Range(row, col, row + 1, 10).Style
                        .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        .Border.SetRightBorder(XLBorderStyleValues.Thin);
                    wkSt.Range(row, 5, row, 9).Style
                        .Border.BottomBorder = XLBorderStyleValues.Dotted;

                    if (flg)
                        wkSt.Range(row, col, row + 1, 10).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                    if (!flg)
                        wkSt.Range(row, col, row + 1, 10).Style.Fill.SetBackgroundColor(XLColor.White);
                    flg = !flg;
                }
                if (ck > 32 * cnt) cnt++; 

                wkSt.Cell(row, col++).Value = data.Kisyua;
                wkSt.Cell(row, col++).Value = data.Kisnam;
                wkSt.Cell(row, col++).Value = data.Seinam;
                wkSt.Cell(row, col++).Value = data.Seiktani;
                wkSt.Cell(row, col++).Value = data.Tanktani;
                wkSt.Cell(row, col++).Value = data.Seiktaik;
                wkSt.Cell(row, col++).Value = data.nebk;
                wkSt.Cell(row, col++).Value = data.Tanka == 0 ? null : data.Tanka;
                wkSt.Cell(row, col).Style.DateFormat.Format = "###0.00000";
                wkSt.Cell(row, col++).Value = data.Niekyt;
                wkSt.Cell(row, col).Style.DateFormat.Format = "###0.0000";
                ck++;
                row++;
            }

        //空行作成
            int zanrow = cnt *32 - (ck-1);
            for (int i = 1; i < zanrow; i++)
            {
                col = 2;
                if (row % 2 != 0)
                {
                    rangecp.CopyTo(cells);
                    wkSt.Range(row, col, row + 1, 10).Style
                        .Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wkSt.Range(row, col, row + 1, 10).Style
                        .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        .Border.SetRightBorder(XLBorderStyleValues.Thin);
                    wkSt.Range(row, 5, row, 9).Style
                        .Border.BottomBorder = XLBorderStyleValues.Dotted;

                    if (flg)
                        wkSt.Range(row, col, row + 1, 10).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                    if (!flg)
                        wkSt.Range(row, col, row + 1, 10).Style.Fill.SetBackgroundColor(XLColor.White);
                    flg = !flg;
                }
                row++;
            }

            #region セル調整
            //セル調整
            range = wkSt.Range(7, 2,row, 2);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            range = wkSt.Range(7, 5, row, 10);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            cells = wkSt.Cell(row, 10);
            // 列幅指定
            wkSt.Column(2).Width = 7.15;
            wkSt.Columns(2, 2).Width = 0.01; // 列幅指定

            wkSt.Column(3).Width = 30;
            wkSt.Column(4).Width = 40;
            wkSt.Column(5).Width = 14.75;
            wkSt.Column(6).Width = 14.75;
            wkSt.Column(7).Width = 14.75;
            wkSt.Column(8).Width = 14.75;
            wkSt.Column(9).Width = 14.75;
            wkSt.Column(10).Width = 14.75;

            #endregion

            //cells.Value = "FDASS_LM1M002a";
            

            //wkSt.Columns().AdjustToContents();
            wkSt.Style.Font.FontName = "游ゴシック";

            string fileNm = await MakeFileName("A");
            wkBk.Style.Font.FontName = "游ゴシック";
            wkBk.SaveAs(@fileNm);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("検索条件：" + id );
            Logwrite(sb.ToString());

            return fileNm;
        }

        private async Task<string> EdtMitumoriHExcelTableAsync(IEnumerable<EstimatesViewModel> seikyuData, EstimatesCKViewModel ckCod, string id)
        {
            //品番コード
            string torCod = ckCod.Fbtcod;
            string torName = seikyuData.Select(x => x.Seinam).First();

            var hinCodData = await estimatesRepository.HinCodExcelViewModel(id);
            //機種ACount取得
            var kisyuA = hinCodData.GroupBy(x => x.Kisyua)
                .Select(g => new { Kisyua = g.Key, maxCnt = g.Count() }).ToList();
            List<string> ksyA = new List<string>();
            List<int> cntA = kisyuA.Select(a => a.maxCnt).ToList();
            foreach (var kA in kisyuA)
            {
                ksyA.Add(kA.Kisyua);
            }
            int indx = 0;
            string stNam = string.Empty;
            List<HinCodExcelViewModel> hinCodList = hinCodData.ToList();
            var wkBk = new XLWorkbook(Server.MapPath("~") + @"\Areas\Fdass\Excel\EstimatesHinCod.xlsx");
            var wkSt = wkBk.Worksheets.First();
            //if(kisyuA.Count() > 1)
            //{
            //    wkSt = wkSt.CopyTo("Fe保管請求見積書_品番コード単位1");
            //}
            //else
            //{
            //    wkSt = wkSt.CopyTo("Fe保管請求見積書_品番コード単位");
            //}
            wkSt = wkSt.CopyTo("Fe保管請求見積書_品番コード単位_" + ksyA[0]);

            wkSt.Style.Font.FontName = "游ゴシック";

            //顧客名            
            var range = wkSt.Range("E2");
            range.Value = torName.Length >= 20 ? torName.Substring(0, 20) : torName + "　様";
            range = wkSt.Range("E2:Q2").Merge(false);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            range = wkSt.Range("E3");
            range.Value = torName.Length >= 20 ? torName.Substring(20) + "様" : string.Empty;
            range = wkSt.Range("E3:Q3").Merge(false);
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

            //出力日時
            range = wkSt.Range("AW2");
            range.Value = DateTime.Now;
            range = wkSt.Range("AW2:BA2").Merge(false);
            var rangeh = wkSt.Range("B2:BA9");
            var rangecp = wkSt.Range("B10:BA10");
            var cellMrg = wkSt.Cell(10,2);

            #region col行
            int colB = 2;
            int colE = 5;
            int colF = 6;
            int colO = 15;
            //int colQ = 17;
            //int colS = 19;
            int colT = 20;
            int colW = 23;
            int colX = 24;
            int colY = 25;
            int colZ = 26;
            int colAA = 27;
            int colAB = 28;
            int colAD = 30;
            int colAG = 33;
            int colAI = 35;
            int colAK = 37;
            int colAL = 38;
            int colAM = 39;
            int colAN = 40;
            int colAO = 41;
            int colAP = 42;
            //int colAQ = 43;
            int colAS = 45;
            int colAT = 46;
            int colAU = 47;
            int colAW = 49;
            int colAX = 50;
            int colAY = 51;
            int colBA = 53;
            #endregion
            bool flg = true;
            int col = 2;
            int row = 10;
            int hrow = 6;
            int nrow = 7;
            int pgcnt = 1;
            int ck = 1;
            int cnt = 1;

            bool firstck = true;
            var rangept = wkSt.Range("A1");

            foreach (var data in hinCodList)
            {
                //一番最初はヘッダー入力
                if(firstck)
                {
                    firstck = false;
                    //機種A
                    wkSt.Cell(hrow, colE).Value = data.Kisyua;
                    range = wkSt.Range(hrow, colE, hrow, colF).Merge(false);
                    wkSt.Cell(nrow, colE).Value = data.Kisnam;
                    range = wkSt.Range(nrow, colE, nrow, colO).Merge(false);
                    //保管
                    wkSt.Cell(hrow, colT).Value = data.Hokflg1;
                    range = wkSt.Range(hrow, colT, hrow, colW).Merge(false);
                    wkSt.Cell(hrow, colY).Value = data.Hokflg2;
                    range = wkSt.Range(hrow, colY, hrow, colAB).Merge(false);
                    wkSt.Cell(hrow, colAD).Value = data.Hokflg3;
                    range = wkSt.Range(hrow, colAD, hrow, colAG).Merge(false);
                    wkSt.Cell(hrow, colAI).Value = data.Hnebir;
                    wkSt.Cell(hrow, colAI).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(hrow, colAI, hrow, colAK).Merge(false);
                    wkSt.Cell(hrow, colAU).Value = data.Ojyukr;
                    wkSt.Cell(hrow, colAU).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(hrow, colAU, hrow, colAW).Merge(false);
                    wkSt.Cell(hrow, colAY).Value = data.Osyjyr;
                    wkSt.Cell(hrow, colAY).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(hrow, colAY, hrow, colBA).Merge(false);
                    //荷役
                    wkSt.Cell(nrow, colT).Value = data.Nieflg1;
                    range = wkSt.Range(nrow, colT, nrow, colW).Merge(false);
                    wkSt.Cell(nrow, colY).Value = data.Nieflg2;
                    range = wkSt.Range(nrow, colY, nrow, colAB).Merge(false);
                    wkSt.Cell(nrow, colAD).Value = data.Nieflg3;
                    range = wkSt.Range(nrow, colAD, nrow, colAG).Merge(false);
                    wkSt.Cell(nrow, colAI).Value = data.Nnebir;
                    wkSt.Cell(nrow, colAI).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(nrow, colAI, nrow, colAK).Merge(false);
                    wkSt.Cell(nrow, colAM).Value = data.Nieant;
                    wkSt.Cell(nrow, colAM).Style.DateFormat.Format = "###0.0000";
                    range = wkSt.Range(nrow, colAM, nrow, colAP).Merge(false);
                    wkSt.Cell(nrow, colAU).Value = data.Hjyukr;
                    wkSt.Cell(nrow, colAU).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(nrow, colAU, nrow, colAW).Merge(false);
                    wkSt.Cell(nrow, colAY).Value = data.Hsyjyr;
                    wkSt.Cell(nrow, colAY).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(nrow, colAY, nrow, colBA).Merge(false);
                }

                if (ksyA[indx] != data.Kisyua)
                {
                    //フッター更新
                    //wkSt.Cell(row, colAQ).Value = "富士物流株式会社　FDASS_LM1M001a";
                    //range = wkSt.Range(row, colAQ, row, colBA).Merge(false);
                    //空行作成
                    int zanrow = cnt * 30 - (ck - 1);
                    for (int i = 1; i < zanrow; i++)
                    {
                        col = 2;
                            wkSt.Range(row, colB, row, colBA).Style
                                .Border.SetTopBorder(XLBorderStyleValues.Thin)
                                .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                                .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                                .Border.SetRightBorder(XLBorderStyleValues.Thin);
                            if (flg)
                                wkSt.Range(row, col, row, colBA).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                            if (!flg)
                                wkSt.Range(row, col, row, colBA).Style.Fill.SetBackgroundColor(XLColor.White);
                            flg = !flg;
                            range = wkSt.Range(row, colB, row, colE).Merge(false);
                            range = wkSt.Range(row, colF, row, colW).Merge(false);
                            range = wkSt.Range(row, colX, row, colZ).Merge(false);
                            range = wkSt.Range(row, colAA, row, colAL).Merge(false);
                            range = wkSt.Range(row, colAM, row, colAN).Merge(false);
                            range = wkSt.Range(row, colAO, row, colAS).Merge(false);
                            range = wkSt.Range(row, colAT, row, colAW).Merge(false);
                            range = wkSt.Range(row, colAX, row, colBA).Merge(false);
                        row++;
                    }

                    //改ページ制御
                    var lrow = row;
                    flg = true;
                    stNam = wkSt.Name;
                    indx += 1;
                    row = 10;
                    pgcnt++;
                    cnt = 1;
                    ck = 1;
                    wkSt = wkSt.CopyTo("Fe保管請求見積書_品番コード単位_" + data.Kisyua);
                    wkSt.Style.Font.FontName = "游ゴシック";
                    // 明細行削除
                    wkSt.Rows(10, lrow).Delete();
                    //機種A
                    wkSt.Cell(hrow, colE).Value = data.Kisyua;
                    range = wkSt.Range(hrow, colE, hrow, colF).Merge(false);
                    wkSt.Cell(nrow, colE).Value = data.Kisnam;
                    range = wkSt.Range(nrow, colE, nrow, colO).Merge(false);
                    //保管
                    wkSt.Cell(hrow, colT).Value = data.Hokflg1;
                    range = wkSt.Range(hrow, colT, hrow, colW).Merge(false);
                    wkSt.Cell(hrow, colY).Value = data.Hokflg2;
                    range = wkSt.Range(hrow, colY, hrow, colAB).Merge(false);
                    wkSt.Cell(hrow, colAD).Value = data.Hokflg3;
                    range = wkSt.Range(hrow, colAD, hrow, colAG).Merge(false);
                    wkSt.Cell(hrow, colAI).Value = data.Hnebir;
                    wkSt.Cell(hrow, colAI).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(hrow, colAI, hrow, colAK).Merge(false);
                    wkSt.Cell(hrow, colAU).Value = data.Ojyukr;
                    wkSt.Cell(hrow, colAU).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(hrow, colAU, hrow, colAW).Merge(false);
                    wkSt.Cell(hrow, colAY).Value = data.Osyjyr;
                    wkSt.Cell(hrow, colAY).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(hrow, colAY, hrow, colBA).Merge(false);
                    //荷役
                    wkSt.Cell(nrow, colT).Value = data.Nieflg1;
                    range = wkSt.Range(nrow, colT, nrow, colW).Merge(false);
                    wkSt.Cell(nrow, colY).Value = data.Nieflg2;
                    range = wkSt.Range(nrow, colY, nrow, colAB).Merge(false);
                    wkSt.Cell(nrow, colAD).Value = data.Nieflg3;
                    range = wkSt.Range(nrow, colAD, nrow, colAG).Merge(false);
                    wkSt.Cell(nrow, colAI).Value = data.Nnebir;
                    wkSt.Cell(nrow, colAI).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(nrow, colAI, nrow, colAK).Merge(false);
                    wkSt.Cell(nrow, colAM).Value = data.Nieant;
                    wkSt.Cell(nrow, colAM).Style.DateFormat.Format = "###0.0000";
                    range = wkSt.Range(nrow, colAM, nrow, colAP).Merge(false);
                    wkSt.Cell(nrow, colAU).Value = data.Hjyukr;
                    wkSt.Cell(nrow, colAU).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(nrow, colAU, nrow, colAW).Merge(false);
                    wkSt.Cell(nrow, colAY).Value = data.Hsyjyr;
                    wkSt.Cell(nrow, colAY).Style.DateFormat.Format = "###0.000";
                    range = wkSt.Range(nrow, colAY, nrow, colBA).Merge(false);

                }

                col = 2;

                wkSt.Range(row, colB, row, colBA).Style
                    .Border.SetTopBorder(XLBorderStyleValues.Thin)
                    .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                    .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                    .Border.SetRightBorder(XLBorderStyleValues.Thin);
                if (flg)
                    wkSt.Range(row, col, row, colBA).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                if (!flg)
                    wkSt.Range(row, col, row, colBA).Style.Fill.SetBackgroundColor(XLColor.White);
                flg = !flg;

                wkSt.Cell(row, colB).Value = data.Hincod;
                range = wkSt.Range(row, colB, row, colE).Merge(false);
                range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                wkSt.Cell(row, colF).Value = data.Hinnmk;
                range = wkSt.Range(row, colF, row, colW).Merge(false);
                wkSt.Cell(row, colX).Value = data.Kisyub;
                range = wkSt.Range(row, colX, row, colZ).Merge(false);
                wkSt.Cell(row, colAA).Value = data.Sybcod + "　" + data.Sybnam;
                range = wkSt.Range(row, colAA, row, colAL).Merge(false);
                wkSt.Cell(row, colAM).Value = data.Frikae;
                range = wkSt.Range(row, colAM, row, colAN).Merge(false);
                wkSt.Cell(row, colAO).Value = data.Fptank;
                wkSt.Cell(row, colAO).Style.DateFormat.Format = "###0.00";
                range = wkSt.Range(row, colAO, row, colAS).Merge(false);
                wkSt.Cell(row, colAT).Value = data.Hokant;
                wkSt.Cell(row, colAT).Style.DateFormat.Format = "###0.00000";
                range = wkSt.Range(row, colAT, row, colAW).Merge(false);
                //wkSt.Cell(row, colAX).Value = data.Nieant;
                wkSt.Cell(row, colAX).Value = 0;
                wkSt.Cell(row, colAX).Style.DateFormat.Format = "###0.00000";
                range = wkSt.Range(row, colAX, row, colBA).Merge(false);
                if (ck > 30 * cnt) cnt++;
                row++;
                ck++;
            }
            //空行作成
            int zanrow2 = cnt * 30 - (ck - 1);
            for (int i = 1; i < zanrow2; i++)
            {
                col = 2;
                    wkSt.Range(row, colB, row, colBA).Style
                        .Border.SetTopBorder(XLBorderStyleValues.Thin)
                        .Border.SetBottomBorder(XLBorderStyleValues.Thin)
                        .Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        .Border.SetRightBorder(XLBorderStyleValues.Thin);
                    if (flg)
                        wkSt.Range(row, col, row, colBA).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                    if (!flg)
                        wkSt.Range(row, col, row, colBA).Style.Fill.SetBackgroundColor(XLColor.White);
                    flg = !flg;
                    range = wkSt.Range(row, colB, row, colE).Merge(false);
                    range = wkSt.Range(row, colF, row, colW).Merge(false);
                    range = wkSt.Range(row, colX, row, colZ).Merge(false);
                    range = wkSt.Range(row, colAA, row, colAL).Merge(false);
                    range = wkSt.Range(row, colAM, row, colAN).Merge(false);
                    range = wkSt.Range(row, colAO, row, colAS).Merge(false);
                    range = wkSt.Range(row, colAT, row, colAW).Merge(false);
                    range = wkSt.Range(row, colAX, row, colBA).Merge(false);
                row++;
            }

            //フッター更新
            //wkSt.Cell(row, colAQ).Value = "富士物流株式会社　FDASS_LM1M001a";
            //range = wkSt.Range(row, colAQ, row, colBA).Merge(false);
            //baseシート非表示
            wkBk.Worksheet("base").Hide();

            XLWorkbook.DefaultStyle.Font.FontName = "游ゴシック";
            string fileNm = await MakeFileName("H");
            //wkBk.Style.Font.FontName = "游ゴシック";
            wkBk.SaveAs(@fileNm);
            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("検索条件：" + id);
            Logwrite(sb.ToString());

            return fileNm;

        }

        private async Task<string> MakeFileName(string Hokflg1)
        {

            var loginUser = Session.GetUserID();
            IEnumerable<MControl> enumerable = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.MitumoriFile);
            MControl controls = enumerable.Where(x => x.Kbn == "1").First();

            string strName = string.Empty;
            strName = Hokflg1 == "A" ? FileA : FileH;

            DateTime dt = DateTime.Now;
            string dtStr = dt.ToString("yyyyMMddHHmmss");

            return controls.Value1 + "/" + loginUser + "_" + strName + dtStr + ".xlsx";
        }

        private async Task DeleteFileAsync()
        {

            var loginUser = Session.GetUserID();
            IEnumerable<MControl> enumerable = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.MitumoriFile);
            MControl controls = enumerable.Where(x => x.Kbn == "1").First();

            string[] files = System.IO.Directory.GetFiles(controls.Value1, loginUser + "_*.xlsx", System.IO.SearchOption.AllDirectories);

            foreach (string file in files)
            {
                System.IO.File.Delete(@file);
            }

        }

        //ログ出力
        private void Logwrite(string message)
        {

            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Fdass/Estimates").Where(x => x.ActionName == "Index").FirstOrDefault();
            string name = "処理機能：" + menu.TitleName;
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.O1, name, message, logRepository);

        }


    }


}