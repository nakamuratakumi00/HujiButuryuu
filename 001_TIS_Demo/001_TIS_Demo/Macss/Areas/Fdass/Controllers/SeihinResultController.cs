using Macss.Areas.Fdass.Repositories;
using Macss.Areas.Fdass.ViewModels;
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
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Export.Pdf.Section;
using System.Text;
using Macss.Controllers;
using Macss.ViewModels;



namespace Macss.Areas.Fdass.Controllers
{
    public class SeihinResultController : Controller
    {
        private IControlRepository controlRepository;
        private ISeihinResultRepository seihinResultRepository;
        private LogService logService;
        private ILogRepository logRepository;


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            controlRepository = new ControlRepository(dbContext);
            seihinResultRepository = new SeihinResultRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        [AuthorityActionFilter]
        // GET: Print/Index
        public ActionResult Index()
        {
            DateTime dtNow = DateTime.Now;
            string drStr = dtNow.ToString("yyyy/MM/dd");
            DateTime result = DateTime.Parse(drStr);
            SeihinResultVierModel resultDate = new SeihinResultVierModel
            {
                DateFrom = drStr,
                DateTo = drStr
            };
            //ViewBag.Mode = "1";
            return View(resultDate);
        }

        //[HttpPost]
        public async Task<ActionResult> TankaAutoSetList(string DTFROM, string DTTO)
        {
            string dtfrom = DTFROM;
            string dtto = DTTO;

            var result = await seihinResultRepository.TankaAutoSetList(dtfrom, dtto);

            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Fdass\Report\TankaAutoSetList.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();

            DateTime dt = DateTime.Now;
            string dtStr = dt.ToString("yyyy/MM/dd");
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
            (rpt.Sections["PageHeader"].Controls["Nen"])).Text = dtStr.Substring(0, 4);
            ((GrapeCity.ActiveReports.SectionReportModel.TextBox)
                        (rpt.Sections["PageHeader"].Controls["Tuki"])).Text = dtStr.Substring(5, 2);
            rpt.DataSource = result.ToArray();
            PrintPdf(rpt);

            //ログ
            StringBuilder sb = new StringBuilder();
            sb.Append("出力：" + result.Count() + "件");

            Logwrite(sb.ToString());

            return null;


        }


        protected void PrintPdf(SectionReport rpt)
        {

            try
            {
                rpt.Run(false);
            }
            catch (ReportException eRunReport)
            {
                // レポートの作成に失敗した場合、クライアントにエラーメッセージを表示します。
                Response.Clear();

                // エラー画面へ遷移
                throw eRunReport;
            }

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
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            // レポートをPDFにエクスポートします。
            pdf.Export(rpt.Document, memStream);
            // 出力ストリームにPDFストリームを出力します。
            Response.BinaryWrite(memStream.ToArray());
            // バッファーされているすべての内容をクライアントへ送信します。
            Response.End();
        }

        //ログ出力
        private void Logwrite(string message)
        {

            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Fdass/SeihinResult").Where(x => x.ActionName == "Index").FirstOrDefault();
            string name = "処理機能：" + menu.TitleName;
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.O1, name, message, logRepository);

        }


    }

}