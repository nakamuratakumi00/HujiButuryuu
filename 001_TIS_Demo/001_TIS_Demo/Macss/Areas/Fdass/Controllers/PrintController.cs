using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Macss.Models;
using Macss.Areas.Fdass.Models;
using Macss.Areas.Fdass.ViewModels;
using Macss.Repositories;
using Macss.Areas.Fdass.Repositories;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using System.Text;
using Macss.Controllers;
using Macss.ViewModels;
using Macss.Attributes.ActionFilter;
using ClosedXML.Excel;
using System.IO;

namespace Macss.Areas.Fdass.Controllers
{
    public class PrintController : Controller
    {
        private IControlRepository controlRepository;
        private IPrintRepository printRepository;
        private LogService logService;
        private ILogRepository logRepository;
        //private static readonly string eOutputDir = "~/Report";


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            printRepository = new PrintRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        // GET: Print/Index
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            var outputData = await printRepository.GetDispData();
            ViewBag.Mode = "1";

            return View(outputData);
        }

        //Fe保管請求PCコードデータ作成エラーリスト
        public async Task<ActionResult> PcCodeErrorListPrint()
        {
            // Fe保管請求PCコードデータ作成エラーリスト
          var result = await printRepository.PcCodeErrorListPrint();
            var sysDate = await printRepository.GetDispData();


                SectionReport rpt = new SectionReport();
                System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\PcCodeErrorList.rpx");
                rpt.LoadLayout(xtr);
                xtr.Close();

            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Nen"])).Text = sysDate.Month.Substring(0, 4);
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Tuki"])).Text = sysDate.Month.Substring(5);

            rpt.DataSource = result.ToArray();
                PrintPdf(rpt, false);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：Fe保管請求PCコードデータ作成エラーリスト、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());

            return null;

        }

        // FD寄託保管請求請求漏機種確認リスト
        public async Task<ActionResult> KisyuMoreList()
        {
            // FD寄託保管請求請求漏機種確認リスト
            var result = await printRepository.KisyuMoreList();
            var sysDate = await printRepository.GetDispData();

            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\KisyuMoreList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();

            rpt.DataSource = result.ToArray();
            PrintPdf(rpt, false);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：Fe保管請求請求漏れ機種確認リスト、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());
            return null;

        }

        // FD寄託保管請求入出庫繰越データリスト
        public async Task<ActionResult> NyuSyukoKurikosiList()
        {
            // FD寄託保管請求入出庫繰越データリスト
            var result = await printRepository.NyuSyukoKurikosiList();
            var sysDate = await printRepository.GetDispData();

            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\NyuSyukoKurikosiList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();
            rpt.DataSource = result.ToArray();
            PrintPdf(rpt, false);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：Fe入出庫繰越データリスト、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());
            return null;

        }

        // FD保管請求請求先別集計表
        public async Task<ActionResult> SekiyusakiShukeiList()
        {
            // FD保管請求請求先別集計表
            var result = await printRepository.SekiyusakiShukeiList();
            var sysDate = await printRepository.GetDispData();


            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\SekiyusakiShukeiList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();

            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
            (rpt.Sections["PageHeader"].Controls["Nen"])).Text = sysDate.Month.Substring(0, 4);
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Tuki"])).Text = sysDate.Month.Substring(5);
            rpt.DataSource = result.ToArray();
            PrintPdf(rpt, false);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：Fe保管請求請求先別集計表、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());
            return null;

        }

        // 倉庫明細書(機種A単位)
        public async Task<ActionResult> SoukoKisyuAList()
        {
            // 倉庫明細書(機種A単位)
            var result = await printRepository.SoukoKisyuAList();
            var sysDate = await printRepository.GetDispData();
            SectionReport rpt = new SectionReport();

            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\SoukoKisyuAList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
            (rpt.Sections["PageHeader"].Controls["Nen"])).Text = sysDate.Month.Substring(0, 4);
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Tuki"])).Text = sysDate.Month.Substring(5);

            rpt.DataSource = result.ToArray();
            PrintPdf(rpt, true);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：倉庫料明細書（機種A単位）、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());
            return null;

        }

        // 倉庫明細書(品番コード単位)
        public async Task<ActionResult> SoukoHinCodList()
        {
            var result = await printRepository.SoukoHinCodList();
            var sysDate = await printRepository.GetDispData();
            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\SoukoHinCodList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
            (rpt.Sections["PageHeader"].Controls["Nen"])).Text = sysDate.Month.Substring(0, 4);
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Tuki"])).Text = sysDate.Month.Substring(5);
            rpt.DataSource = result.ToArray();
            PrintPdf(rpt, true);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：倉庫料明細書（品番コード単位）、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());
            return null;
        }

        // 倉庫明細書(倉庫使用料単位)
        public async Task<ActionResult> SoukoSiyouryoList()
        {
            var result = await printRepository.SoukoSiyouryoList();
            var sysDate = await printRepository.GetDispData();

            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\SoukoSiyouryoList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
            (rpt.Sections["PageHeader"].Controls["Nen"])).Text = sysDate.Month.Substring(0, 4);
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Tuki"])).Text = sysDate.Month.Substring(5);
            rpt.DataSource = result.ToArray();
            PrintPdf(rpt, true);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：倉庫料明細書（倉庫使用料単位）、出力：" + result.Count() + "件");

            Logwrite(sb.ToString());
            return null;
        }        

        protected void PrintPdf(SectionReport rpt, bool flg)
        {

            try
            {
                if(flg)
                {
                    rpt.Run();
                }
                else
                {
                    rpt.Run(false);
                }
            }
            catch (ReportException eRunReport)
            {
                // レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
                Response.Clear();

                // エラー画面へ遷移
                throw eRunReport;
            }
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();

            //2部出力の場合はprintプロパティを設定
            if (flg)
            {
                ////PDF用の設定を定義します。
                GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport p = new GrapeCity.ActiveReports.Export.Pdf.Section.PdfExport();
                p.Version = GrapeCity.ActiveReports.Export.Pdf.Section.PdfVersion.Pdf17;
                ////PrinPresetsクラスを使用してデフォルトの印刷設定を設定します。
                p.PrintPresets.NumberOfCopies = GrapeCity.ActiveReports.Export.Pdf.Enums.NumberOfCopies.Two;
                p.Export(rpt.Document, Server.MapPath("~") + "\\PrintPresets.pdf");

                p.Export(rpt.Document, memStream);
                Response.BinaryWrite(memStream.ToArray());
                // バッファーされているすべての内容をクライアントへ送信します。
                Response.End();
            }
            else
            {
                // ブラウザに対してPDFドキュメントの適切なビューワを使用するように指定します。
                // エクスポート形式別にコンテンツタイプを
                // 以下のように変更できます。
                //	ExportType  ContentType
                //	PDF	   "application/pdf"  （小文字）
                //	RTF	   "application/rtf"
                //	TIFF	  "image/tiff"	   （ブラウザとは別のビューワで表示される）
                //	HTML	  "message/rfc822"   （画像を含む圧縮されたHTMLページに適用する）
                //	Excel	 "application/vnd.ms-excel"
                //	Excel	 "application/excel" （いずれかが動作される）
                //	Text	  "text/plain"  
                //Response.ContentType = "application/pdf";
                Response.ContentType = "image/tiff";
                Response.AddHeader("content-disposition", "inline; filename=MyPDF.PDF");
                // PDFエクスポートオブジェクトを作成します。
                PdfExport pdf = new PdfExport();
                // PDFの出力用のメモリストリームを作成します。
                // レポートをPDFにエクスポートします。
                pdf.Export(rpt.Document, memStream);
            }
                // 出力ストリームにPDFストリームを出力します。
                Response.BinaryWrite(memStream.ToArray());
                // バッファーされているすべての内容をクライアントへ送信します。
                Response.End();

        }
        // Fe保管請求拠点別データ
        public async Task<ActionResult> KyotenExcel()
        {
            string fileNm = "Fe保管請求拠点別データ";
            int row = 2;
            int col = 1;
            var result = await printRepository.KyotenbetuList();
            var wkBk = new XLWorkbook(Server.MapPath("~") + @"\Areas\Fdass\Excel\KyotenData.xlsx");
            var wkSt = wkBk.Worksheets.First();
            foreach (var data in result)
            {
                col = 1;
                
                wkSt.Cell(row, col++).Value = data.Basyo;
                wkSt.Cell(row, col++).Value = data.Basnam;
                wkSt.Cell(row, col++).Value = data.Kisyua;
                wkSt.Cell(row, col++).Value = data.Kisyub;
                wkSt.Cell(row, col++).Value = data.Zansuu;
                wkSt.Cell(row, col++).Value = data.Zankin;
                wkSt.Cell(row, col++).Value = data.Nyuksu;
                wkSt.Cell(row, col++).Value = data.Nyukin;
                wkSt.Cell(row, col++).Value = data.Syksuu;
                wkSt.Cell(row, col++).Value = data.Sykkin;
                wkSt.Cell(row, col++).Value = data.Zaiksu;
                wkSt.Cell(row, col++).Value = data.Zaikin;
                wkSt.Cell(row, col++).Value = data.Densuu;
                wkSt.Cell(row, col++).Value = data.Densky;
                wkSt.Cell(row, col++).Value = data.Hokank;
                wkSt.Cell(row, col++).Value = data.Niekik;
                wkSt.Cell(row, col++).Value = data.Niekyj;
                wkSt.Cell(row, col++).Value = data.Seicod;
                wkSt.Cell(row, col).Style.DateFormat.Format = "###0";
                wkSt.Cell(row, col++).Value = data.Pccodh;
                wkSt.Cell(row, col).Style.DateFormat.Format = "###0";
                wkSt.Cell(row, col++).Value = data.Pccodn;
                wkSt.Cell(row, col++).Value = data.Dataym;
                row++;

            }

            wkSt.Columns().AdjustToContents();

            PrintViewModel outputData = await printRepository.GetDispData();
            string month = outputData.Month.Replace("/", "");
            string dataString = month.Substring(2, 4);

            System.IO.MemoryStream fs = await MakeStream(fileNm, wkBk, result.Count());

            return File(fs.ToArray(),
                     "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    dataString + fileNm + ".xlsx");

        }

        // FPS様保管請求拠点別データ
        public async Task<ActionResult> FpsKyotenExcel()
        {

            string fileNm = "FPS様保管請求拠点別データ";
            int row = 2;
            int col = 1;
            var result = await printRepository.FpsKyotenExcel();
            XLWorkbook wkBk = new XLWorkbook(Server.MapPath("~") + @"\Areas\Fdass\Excel\FpsKyotenData.xlsx");
            IXLWorksheet wkSt = wkBk.Worksheets.First();
            foreach (THokanSeikyuKyoten data in result)
            {
                col = 1;
                wkSt.Cell(row, col++).Value = data.Basyo;
                wkSt.Cell(row, col++).Value = data.Basnam;
                wkSt.Cell(row, col++).Value = data.Kisyua;
                wkSt.Cell(row, col++).Value = data.Kisyub;
                wkSt.Cell(row, col++).Value = data.Densuu;
                wkSt.Cell(row, col++).Value = data.Densky;
                wkSt.Cell(row, col++).Value = data.Hokank;
                wkSt.Cell(row, col++).Value = data.Niekik;
                wkSt.Cell(row, col++).Value = data.Niekyj;
                wkSt.Cell(row, col++).Value = data.Seicod;
                wkSt.Cell(row, col++).Value = data.Dataym;
                row++;
            }

            PrintViewModel outputData = await printRepository.GetDispData();
            string month = outputData.Month.Replace("/", "");
            string dataString = month.Substring(2, 4);

            System.IO.MemoryStream fs = await MakeStream(fileNm, wkBk, result.Count());

            return File(fs.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    dataString + fileNm + ".xlsx");

        }

        private async Task<System.IO.MemoryStream> MakeStream(string fileNm, XLWorkbook wkBk, int count)
        {

            var loginUser = Session.GetUserID();
            //if (System.IO.File.Exists(loginUser + "_" + fileNm)) {
            //    await DeleteFileAsync(loginUser + "_" + fileNm);
            //}
            await DeleteFileAsync(loginUser + "_" + fileNm);

            IEnumerable<MControl> enumerable = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.KyotenFile);
            MControl controls = enumerable.Where(x => x.Kbn == "1").First();
            DateTime dt = DateTime.Now;
            string dtStr = dt.ToString("yyyyMMddHHmmss");
            string strName = controls.Value1 + "/" + loginUser + "_" + fileNm + "_" + dtStr + ".xlsx";

            wkBk.Style.Font.FontName = "游ゴシック";
            wkBk.SaveAs(strName);

            StringBuilder sb = new StringBuilder();
            sb.Append("帳票名：" + fileNm + "、出力：" + count + "件"); 
            Logwrite(sb.ToString());

            string dataString = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var fs = new System.IO.MemoryStream();
            var wb = new ClosedXML.Excel.XLWorkbook(strName);
            wb.Worksheets.First().ColumnsUsed().AdjustToContents();
            wb.SaveAs(fs);
            fs.Position = 0;

            return fs;

        }

        private async Task DeleteFileAsync(string fileNm)
        {

            IEnumerable<MControl> enumerable = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.KyotenFile);
            MControl controls = enumerable.Where(x => x.Kbn == "1").First();

            string[] files = System.IO.Directory.GetFiles(controls.Value1, fileNm + "_*.xlsx", System.IO.SearchOption.AllDirectories);

            foreach (string file in files)
            {
                System.IO.File.Delete(@file);
            }

        }

        private void Logwrite(string message)
        {

            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Fdass/Print").Where(x => x.ActionName == "Index").FirstOrDefault();
            string name = "処理機能：" + menu.TitleName;
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.O1, name, message, logRepository);

        }
    }
}