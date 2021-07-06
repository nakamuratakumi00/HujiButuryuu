using Macss.Database.Entity;
using MacssWeb.Common;
using MacssWeb.Models;
using MacssWeb.Controllers;
using Sic.Common.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MacssWeb.Areas.Tass.Controllers
{
    public class D021Controller : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(string id, string transAction) // 01 包装費実績明細詳細確認
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult DetailsKobetsu(string id, string transAction) // 02 包装費実績明細詳細確認（個別単価）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult DetailsShiire(string id, string transAction) // 03 包装費実績明細詳細確認（仕入）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult Create(string id, string transAction) // 04 包装費実績明細登録（出荷実績あり）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult CreateConfirm(string id, string transAction) // 05 包装費実績明細登録確認（出荷実績あり）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult CreateKobetsu(string id, string transAction) // 06 包装費実績明細登録（出荷実績あり）（個別単価）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult CreateKobetsuConfirm(string id, string transAction) // 07 包装費実績明細登録確認（出荷実績あり）（個別単価）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult CreateNew() // 08 包装費実績明細登録（出荷実績なし）
        {
            return View();
        }
        public ActionResult CreateNewConfirm() // 09 包装費実績明細登録確認（出荷実績なし）
        {
            return View();
        }
        public ActionResult CreateNewKobetsu() // 10 包装費実績明細登録（出荷実績なし）（個別単価）
        {
            return View();
        }
        public ActionResult CreateNewKobetsuConfirm() // 11 包装費実績明細登録確認（出荷実績なし）（個別単価）
        {
            return View();
        }
        public ActionResult Edit(string id, string transAction) // 12 包装費実績明細更新
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditConfirm(string id, string transAction) // 13 包装費実績明細更新確認
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditKobetsu(string id, string transAction) // 14 包装費実績明細更新（個別単価）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditKobetsuConfirm(string id, string transAction) // 15 包装費実績明細更新確認（個別単価）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditShiire(string id, string transAction) // 16 包装費実績明細更新（仕入）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult EditShiireConfirm(string id, string transAction) // 17 包装費実績明細更新確認（仕入）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult Delete(string id, string transAction) // 18 包装費実績明細削除
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult DeleteKobetsu(string id, string transAction) // 19 包装費実績明細削除（個別単価）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
        public ActionResult DeleteShiire(string id, string transAction) // 20 包装費実績明細削除（仕入）
        {
            ViewBag.myId = id;
            ViewBag.TransAction = transAction;
            return View();
        }
    }
}