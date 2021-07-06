using MacssWeb.Common;
using Sic.Common.Attribute;
using System;
using System.Web.Mvc;

namespace MacssWeb.Areas.Pats.Controllers
{
    public class D004Controller : Controller
    {
        // GET: Pats/D004
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 在庫一覧
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_01()
        {
            ViewBag.Title = Resources.Pats.Inventory + "一覧";
            ViewBag.PageMainTitle = Resources.Pats.Inventory + "一覧";
            ViewBag.ScreenId = "PATS-D004-01";

            return View();
        }

        /// <summary>
        /// 在庫詳細確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_02()
        {
            ViewBag.Title = Resources.Pats.Inventory + "詳細確認";
            ViewBag.PageMainTitle = Resources.Pats.Inventory + "詳細確認";
            ViewBag.ScreenId = "PATS-D004-02";

            return View();
        }

        /// <summary>
        /// 棚卸表作成
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_03()
        {
            ViewBag.Title = "棚卸表 作成";
            ViewBag.PageMainTitle = "棚卸表 作成";
            ViewBag.ScreenId = "PATS-D004-03";

            return View();
        }

        /// <summary>
        /// 棚卸表作成確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_04(string syubetsu)
        {
            ViewBag.Title = $"棚卸表({syubetsu}) 作成確認";
            ViewBag.PageMainTitle = $"棚卸表({syubetsu}) 作成確認";
            ViewBag.ScreenId = "PATS-D004-04";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.TargetCount = string.Format("{0} 件", 30);
            ViewBag.Syubetsu = syubetsu;

            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.ResultMessage = $"棚卸表({syubetsu})を作成します。よろしいですか？";

            return View();
        }

        /// <summary>
        /// 貯蔵品勘定決算書/在庫管理表 出力
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_05()
        {
            ViewBag.Title = "貯蔵品勘定決算書/在庫管理表 出力";
            ViewBag.PageMainTitle = "貯蔵品勘定決算書/在庫管理表 出力";
            ViewBag.ScreenId = "PATS-D004-04";

            return View();
        }

        /// <summary>
        /// 貯蔵品勘定決算書/在庫管理表 出力確認
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_06()
        {
            ViewBag.Title = "貯蔵品勘定決算書/在庫管理表 出力確認";
            ViewBag.PageMainTitle = "貯蔵品勘定決算書/在庫管理表 出力確認";
            ViewBag.ScreenId = "PATS-D004-06";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.TargetCount = string.Format("{0} 件", 30);

            return View();
        }

        /// <summary>
        /// PC別払出表/PC別在庫表 出力
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_07()
        {
            ViewBag.Title = "PC別払出表/PC別在庫表 出力";
            ViewBag.PageMainTitle = "PC別払出表/PC別在庫表 出力";
            ViewBag.ScreenId = "PATS-D004-07";

            return View();
        }

        /// <summary>
        /// PC別払出表/PC別在庫表 出力
        /// </summary>
        /// <returns></returns>
        public ActionResult D004_08()
        {
            ViewBag.Title = "PC別払出表/PC別在庫表 出力確認";
            ViewBag.PageMainTitle = "PC別払出表/PC別在庫表 出力確認";
            ViewBag.ScreenId = "PATS-D004-08";
            ViewBag.AlertType = ValueAttribute.GetValue(AlertTypes.Primary);
            ViewBag.TargetCount = string.Format("{0} 件", 30);

            return View();
        }
    }
}