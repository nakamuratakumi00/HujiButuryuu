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
using GrapeCity.ActiveReports.Export.Pdf.Section;
using System.Text;
using Macss.ViewModels;
using System.Data.Entity.Validation;
using Macss.Areas.Fdass.Common;
using GrapeCity.ActiveReports;
using System.Data.Entity;
using System.Data;

namespace Macss.Areas.Tass.Controllers
{
    public class ShuukaTyuumonshoPrintController : Controller
    {
        private IControlRepository controlRepository;
        private LogService logService;
        private ILogRepository logRepository;
        IShuukaTyuumonshoPrintRepositorie printRepositorie;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            printRepositorie = new ShuukaTyuumonshoPrintRepositorie(dbContext);
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        // GET: Print/Index
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            string Id = Session.GetUserID();
            DateTime dtNow = DateTime.Now;
            string dtStr = dtNow.ToString("yyyy/MM/dd");
            DateTime result = DateTime.Parse(dtStr);
            var data = await printRepositorie.GetDispData(Id);
            ShuukaTyuumonshoPrintViewData resultData = new ShuukaTyuumonshoPrintViewData
            {
                DateFrom = dtStr,
                GroupCd = data.GroupCd
            };
            resultData.Serch = new ShuukaTyuumonshoPrintSerch();
            resultData.Serch.Sybcod = data.Sybcod;

            ViewBag.Mode = "1";

            Session.SetGroupCd(data.GroupCd);

            return View(resultData);

        }

        //検索
        [HttpPost]
        //public async Task<ActionResult> Search(string Hkukbn, string Sybcod, string Tannam, string DateFrom,
        //                                                                string DateTo, string Syukno, string Tokcod, string PrintFrom, string PrintTo, string UserName)
        public async Task<ActionResult> Search(string Hkukbn, string Sybcod, string Tannam, string DateFrom,
                                                                        string DateTo, string Syukno, string Tokcod, string PrintFrom, string PrintTo, string UserName)

        {

            ShuukaTyuumonshoPrintSerch viewModel = new ShuukaTyuumonshoPrintSerch
            {
                //Mihakkou = Mihakkou == false ? false : true,
                //Hakousumi = Hakousumi == false ? false : true,
                Hkukbn = Hkukbn == string.Empty ? null : Hkukbn,
                Sybcod = Sybcod == string.Empty ? null : Sybcod,
                Tannam = Tannam == string.Empty ? null : Tannam,
                DateFrom = DateFrom == string.Empty ? null : DateFrom,
                DateTo = DateTo == string.Empty ? null : DateTo,
                Syukno = Syukno == string.Empty ? null : Syukno,
                Tokcod = Tokcod == string.Empty ? null : Tokcod,
                PrintFrom = PrintFrom == string.Empty ? null : PrintFrom,
                PrintTo = PrintTo == string.Empty ? null : PrintTo,
                UserName = UserName == string.Empty ? null : UserName
            };

            string groupcd = Session.GetGroupCd();
            IEnumerable<ShuukaTyuumonshoPrintInformation> list = await printRepositorie.GetSearchAsync(viewModel, groupcd);

            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：取引先マスタ　得意先コード");
            //Logwrite(sb.ToString());

            if (list != null & list.Count() > 0)
            {
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                return Json(new { succsess = 0 });
            }

            //return Json(list);

        }

        public async Task<ActionResult> Check(string syukNo, string cdate)
        {
            syukNo = syukNo.Replace("-", "");
            string[] sNo = syukNo.Split(',');
            string[] sDt = cdate.Split(',');

            StringBuilder errorMsg = new StringBuilder();
            int cnt = 0;
            foreach (var Syukno in sNo)
            {
                string cksts = string.Empty;
                string cdt = sDt[cnt];
                var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
                var statusList = await dbContext.TUnsouShuukaTyuumonshoTehai
                                     .Where(x => x.Syukno == Syukno)
                                     .Where(x => x.Cdate == cdt).ToListAsync();

                if (statusList.Count() != 0)
                {
                    var status = statusList.First();
                    // 未発行
                    if (status.Denf == null)
                    {
                        // 出荷日
                        DateTime workDate = (DateTime)status.Sykymd;
                        if (workDate.Date < DateTime.Now.Date)
                        {
                            errorMsg.Append("【" + status.Syukno + "】");
                        }
                    }
                }
                cnt++;
            }

            if (errorMsg.Length != 0)
            {
                return Json(new { succsess = 0, msg = "出荷日が過去日付で未発行のデータが選択されました　出荷No:" + errorMsg.ToString() });
            }
            return Json(new { succsess = 1 });

        }

        //印刷・更新
        public async Task<ActionResult> UPDPrint(string syukNo, string cdate)
        {

            var loginUser = Session.GetUserID();
            syukNo = syukNo.Replace("-", "");
            string[] sNo = syukNo.Split(',');
            string[] sDt = cdate.Split(',');

            //更新
            await printRepositorie.UPDShuukaTyuumonsho(sNo, sDt, loginUser);
            //try
            //{
            //    //帳票
            //    var aaaaa = await printRepositorie.ShuukaTyuumonshoPrint(sNo, sDt, loginUser);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //帳票
            var result = await printRepositorie.ShuukaTyuumonshoPrint(sNo, sDt, loginUser);

            SectionReport rpt = new SectionReport();
            System.Xml.XmlTextReader xtr = new System.Xml.XmlTextReader(Server.MapPath("~") + @"\Areas\Tass\Report\ShuukaTyuumonsho.rpx");
            rpt.LoadLayout(xtr);
            xtr.Close();
            rpt.DataSource = result.ToArray();            
            PrintPdf(rpt);

            //ログ出力
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("追加キー：{0}", sNo));
            Logwrite(sb.ToString());

            return null;
        }
        protected void PrintPdf(SectionReport rpt)
        {

            try
            {
                rpt.PageSettings.Margins.Right = 0;
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
            //rpt.PageSettings.Orientation = GrapeCity.ActiveReports.Document.Section.PageOrientation.Landscape;
            // PDFの出力用のメモリストリームを作成します。
            System.IO.MemoryStream memStream = new System.IO.MemoryStream();
            // レポートをPDFにエクスポートします。
            pdf.Export(rpt.Document, memStream);
            // 出力ストリームにPDFストリームを出力します。
            Response.BinaryWrite(memStream.ToArray());
            // バッファーされているすべての内容をクライアントへ送信します。
            Response.End();
        }


        private void Logwrite(string message)
        {
            var loginUser = Session.GetUserID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
            MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Tass/ShuukaTyuumonshoPrint").Where(x => x.ActionName == "Index").FirstOrDefault();
            //string name = "機能名：" + menu.MenuName;
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.S1, "処理機能：" + menu.TitleName, message, logRepository);

        }
    }
}