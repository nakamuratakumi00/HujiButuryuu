using MacssWeb.Common;
using Sic.Common.Attribute;
using System.Web.Mvc;

namespace MacssWeb.Areas.Pats.Controllers
{
    public class D005Controller : Controller
    {
        // GET: Pats/D005
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 請求・仕入データ作成
        /// </summary>
        /// <returns></returns>
        public ActionResult D005_01()
        {
            ViewBag.Title = Resources.Pats.BillingPurchasing + "データ作成";
            ViewBag.PageMainTitle = Resources.Pats.BillingPurchasing + "データ作成";
            ViewBag.ScreenId = "PATS-D005-01";

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ResultMessage = "請求データを作成します。よろしいですか？";

            return View();
        }

        /// <summary>
        /// 請求・仕入データ作成完了
        /// </summary>
        /// <returns></returns>
        public ActionResult D005_02()
        {
            ViewBag.Title = Resources.Pats.BillingPurchasing + "データ作成完了";
            ViewBag.PageMainTitle = Resources.Pats.BillingPurchasing + "データ作成完了";
            ViewBag.ScreenId = "PATS-D005-02";

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = "作成が完了しました。";

            return View();
        }
    }
}