using Macss.Database.Entity;
using MacssWeb.Areas.Dcos.Helper;
using MacssWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Sic.Common.Attribute;

namespace MacssWeb.Areas.Dcos.Controllers
{
    [Authorize]
    public class StockController : StockBaseController 
    {

        // GET: Dcos/Stock
        public ActionResult Index()
        {
            var list = CreateSampleData();

            ViewBag.SumPrevRem = list.Sum(x => x.PreviousMonthRemaining);
            ViewBag.SumIn = list.Sum(x => x.PresentMonthInQuantity);
            ViewBag.SumOut = list.Sum(x => x.PresentMonthOutQuantity);

            TempData["SampleList"] = list;

            return View(list);
        }

        // GET: Dcos/Stock/Details/5
        public ActionResult Details(int? id = null)
        {
            ViewBag.StockScreen = StockScreens.StockDetail;
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = TempData["SampleList"] as List<DummyStock>;

            if (list == null || list.Count <= 0)
            {
                return RedirectToAction(nameof(Index));
            }


            var stock = list.Find(x => x.Id == id.Value);
            if (stock == null)
            {
                return HttpNotFound();
            }

            TempData["SampleList"] = list;
            return View(stock);
        }


        public ActionResult RackChange(int? id = null)
        {
            var list = TempData["SampleList"] as List<DummyStock>;

            DummyStock stock = null;
            if (list == null || list.Count <= 0)
            {
                stock = TempData["BackData"] as DummyStock;
            }
            else
            {
                stock = list.Find(x => x.Id == id.Value);
            }


            stock.MovedAt = DateTime.Now;
            stock.MovedQuantity = stock.LotNumberQuantity;

            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RackChangeConfirm(
            DummyStock stock)
        {
            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RackChange(int id,
            DummyStock stock, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            try
            {
                var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

                if (ret == ButtonValues.Nothing)
                {
                    return View(nameof(RackChangeConfirm), stock);
                }

                switch (ret)
                {
                    case ButtonValues.Back:
                        TempData["BackData"] = stock;
                        return RedirectToAction(nameof(RackChange));
                    case ButtonValues.Exec:
                        //if (!ModelState.IsValid)
                        //{
                        //    return View(stock);
                        //}
                        SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                        return RedirectToAction(nameof(Index));
                    default:
                        return View(nameof(RackChangeConfirm), stock);
                }
            }
            catch
            {
                return View(stock);
            }
        }

        public ActionResult Report()
        {
            return View();
        }
        public ActionResult ReportConfirm()
        {
            return View();
        }
    }
}
