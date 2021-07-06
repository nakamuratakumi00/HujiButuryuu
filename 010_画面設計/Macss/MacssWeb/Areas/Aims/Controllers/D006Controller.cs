using MacssWeb.Common;
using Sic.Common.Attribute;
using System.Web.Mvc;

namespace MacssWeb.Areas.Aims.Controllers
{
    public class D006Controller : Controller
    {
        // GET: Aims/D006
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Exec()
        {
            ViewBag.Date = System.DateTime.Today;
            ViewBag.AlertType = "alert-info";
            ViewBag.ResultMessage = "請求売上・仕入確定を実行します。よろしいですか？";

            return View();
        }

        public ActionResult ExecComplete()
        {
            ViewBag.Date = System.DateTime.Today;
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "請求売上・仕入確定が完了しました。";

            return View();
        }
    }
}