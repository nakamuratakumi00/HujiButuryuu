using MacssWeb.Common;
using Sic.Common.Attribute;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Pats.Controllers
{
    public class D006Controller : Controller
    {
        // GET: Pats/D006
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 月次集計データ作成
        /// </summary>
        /// <returns></returns>
        public ActionResult D006_01()
        {
            ViewBag.Title = Resources.Pats.MonthlyData + "作成";
            ViewBag.PageMainTitle = Resources.Pats.MonthlyData + "作成";
            ViewBag.ScreenId = "PATS-D006-01";

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ResultMessage = $"選択された月次集計データを作成します。よろしいですか？";

            return View();
        }

        /// <summary>
        /// 月次集計データ作成完了
        /// </summary>
        /// <returns></returns>
        public ActionResult D006_02()
        {
            ViewBag.Title = Resources.Pats.MonthlyData + "作成完了";
            ViewBag.PageMainTitle = Resources.Pats.MonthlyData + "作成完了";
            ViewBag.ScreenId = "PATS-D006-02";

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Success);
            ViewBag.ResultMessage = $"作成した月次集計データを共有フォルダーへ保存しました。";
            ViewBag.TargetCount = "100 件";

            return View();
        }
    }
}