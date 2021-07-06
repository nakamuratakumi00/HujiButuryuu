using System.Web.Mvc;

namespace MacssWeb.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Logger.DebugFormat("{0}", "Index");
            Logger.InfoFormat("{0}", "Index");
            Logger.WarnFormat("{0}", "Index");
            Logger.ErrorFormat("{0}", "Index");
            Logger.FatalFormat("{0}", "Index");

            ViewBag.ImportantInfoHtml = GetInformationText("ImportantInformation.txt");
            ViewBag.UptimeInfoHtml = GetInformationText("UptimeInformation.txt");
            ViewBag.InternalInfoHtml = GetInformationText("InternalInformation.txt");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}