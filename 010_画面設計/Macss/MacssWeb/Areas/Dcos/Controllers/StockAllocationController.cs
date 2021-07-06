using Macss.Database.Entity;
using MacssWeb.Areas.Dcos.Helper;
using MacssWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MacssWeb.Areas.Dcos.Controllers
{
    [Authorize]
    public class StockAllocationController : StockBaseController
    {
        // GET: Dcos/StockAllocation
        public ActionResult Index()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationIndex;

            var list = CreateSampleData();

            ViewBag.SumAlloc = list.Sum(x => x.AllocationQuantity);
            ViewBag.SumImpossibleAlloc = list.Sum((x) =>
            {
                var total = 0;
                if (x.FixStockRemaining < 0)
                {
                    total += System.Math.Abs(x.FixStockRemaining);
                }
                return total;

            });

            TempData["SampleList"] = list;

            return View(list);
        }

        public ActionResult AutoAllocation()
        {
            return View();
        }

        public ActionResult AutoAllocationConfirm()
        {
            ViewBag.StockScreen = StockScreens.StockAutoAllocation;

            ViewBag.ConfirmMessage = "自動引当を実行します。よろしいですか？";

            var list = CreateSampleData();

            ViewBag.AllocCount = String.Format("引当対象件数：{0} 件", list.Sum(x => x.OutOrderQuantity));
            ViewBag.ImpossibleAllocCount = String.Format("引当不能件数：{0} 件", list.Sum((x) =>
            {
                var total = 0;
                if (x.FixStockRemaining < 0)
                {
                    total += System.Math.Abs(x.FixStockRemaining);
                }
                return total;

            }));

            TempData["SampleList"] = list;

            return View(list);
        }
        public ActionResult AutoAllocationResult()
        {
            ViewBag.StockScreen = StockScreens.StockAutoAllocationResult;

            var list = TempData["SampleList"] as List<DummyStock>;

            ViewBag.AllocCount = String.Format("{0} 件", list.Sum(x => x.AllocationQuantity));
            ViewBag.ImpossibleAllocCount = String.Format("{0} 件", list.Sum((x) =>
            {
                var total = 0;
                if (x.FixStockRemaining < 0)
                {
                    total += System.Math.Abs(x.FixStockRemaining);
                }
                return total;

            }));

            TempData["SampleList"] = list;

            ViewBag.ResultMessage = "在庫引当計画データを作成しました。";

            return View(list);
        }

        //public ActionResult AllocationExec()
        //{
        //    ViewBag.StockScreen = StockScreens.StockAllocationExec;

        //    ViewBag.ConfirmMessage = "以下のデータを計画実施にします。よろしいですか？";

        //    var list = CreateSampleData();

        //    ViewBag.AllocCount = String.Format("引当対象件数：{0} 件", list.Sum(x => x.OutOrderQuantity));
        //    ViewBag.ImpossibleAllocCount = String.Format("引当不能件数：{0} 件", list.Sum((x) =>
        //    {
        //        var total = 0;
        //        if (x.FixStockRemaining < 0)
        //        {
        //            total += System.Math.Abs(x.FixStockRemaining);
        //        }
        //        return total;

        //    }));

        //    TempData["SampleList"] = list;
        //public ActionResult AllocationExecConfirm()
        //{
        //    ViewBag.StockScreen = StockScreens.StockAllocationExec;

        //    ViewBag.ConfirmMessage = "在庫自動引当を実行します。よろしいですか？";

        //    var list = CreateSampleData();

        //    ViewBag.AllocCount = String.Format("引当対象件数：{0} 件", 10);
        //    ViewBag.ImpossibleAllocCount = String.Format("引当不能件数：{0} 件", 1);
        //    //ViewBag.AllocCount = String.Format("引当対象件数：{0} 件", list.Sum(x => x.OutOrderQuantity));
        //    //ViewBag.ImpossibleAllocCount = String.Format("引当不能件数：{0} 件", list.Sum((x) =>
        //    //{
        //    //    var total = 0;
        //    //    if (x.FixStockRemaining < 0)
        //    //    {
        //    //        total += System.Math.Abs(x.FixStockRemaining);
        //    //    }
        //    //    return total;

        //    //}));

        //    //TempData["SampleList"] = list;

        //    //return View(list);

        //    return View();
        ////}
        //    return View(list);
        //}

        public ActionResult AllocationExecResult()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationExec;

            var list = TempData["SampleList"] as List<DummyStock>;

            ViewBag.AllocCount = String.Format("{0} 件", list.Sum(x => x.AllocationQuantity));
            ViewBag.ImpossibleAllocCount = String.Format("{0} 件", list.Sum((x) =>
            {
                var total = 0;
                if (x.FixStockRemaining < 0)
                {
                    total += System.Math.Abs(x.FixStockRemaining);
                }
                return total;

            }));

            TempData["SampleList"] = list;

            ViewBag.ResultMessage = "引当計画実施済になりました。";

            return View(list);
        }
        public ActionResult PickingListOutput()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPickingList;

            ViewBag.ConfirmMessage = "以下のデータでピッキングリストの出力対象データについてピッキングリストを出力します。";

            var list = CreateSampleData();

            ViewBag.PickingTargetCount = String.Format("ピッキングリスト対象データ件数：{0} 件", list.Sum((x) =>
            {
                var total = 0;
                if (x.PickingListOutput != DummyStock.PickingListOutputStatus.Nothing)
                {
                    total += 1;
                }
                return total;

            }));
            ViewBag.AllocCount = String.Format("引当件数（計画引当件数）：{0} 件", list.Sum(x => x.OutOrderQuantity));
            ViewBag.ImpossibleAllocCount = String.Format("引当不能件数：{0} 件", list.Sum((x) =>
            {
                var total = 0;
                if (x.FixStockRemaining < 0)
                {
                    total += System.Math.Abs(x.FixStockRemaining);
                }
                return total;

            }));

            TempData["SampleList"] = list;

            return View(list);
        }
        public ActionResult AllocationPlanCancelConfirm()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPlanCancel;

            ViewBag.ConfirmMessage = "以下のデータの引当計画実施済を解除します。よろしいですか？";

            var list = CreateSampleData();

            ViewBag.AllocCount = String.Format("引当件数（計画実施済引当件数）：{0} 件", list.Sum(x => x.AllocationExecQuantity));
            //ViewBag.ImpossibleAllocCount = String.Format("引当不能数：{0} 件", list.Sum((x) =>
            //{
            //    var total = 0;
            //    if (x.FixStockRemaining < 0)
            //    {
            //        total += System.Math.Abs(x.FixStockRemaining);
            //    }
            //    return total;

            //}));

            TempData["SampleList"] = list;

            return View(list);
        }
        public ActionResult AllocationPlanCancelResult()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPlanCancel;

            var list = TempData["SampleList"] as List<DummyStock>;
            if (list == null || list.Count <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllocCount = String.Format("{0} 件", list.Sum(x => x.AllocationQuantity));
            //ViewBag.ImpossibleAllocCount = String.Format("{0} 件", list.Sum((x) =>
            //{
            //    var total = 0;
            //    if (x.FixStockRemaining < 0)
            //    {
            //        total += System.Math.Abs(x.FixStockRemaining);
            //    }
            //    return total;

            //}));

            TempData["SampleList"] = list;

            ViewBag.ResultMessage = "引当計画実施済を解除しました。";

            return View(list);
        }
        public ActionResult AllocationPlanListFixConfirm()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPlanFix;

            ViewBag.ConfirmMessage = "以下のデータの引当計画を確定します。よろしいですか？";

            var list = CreateSampleData();

            ViewBag.AllocCount = String.Format("引当件数（計画実施済引当件数）：{0} 件", list.Sum(x => x.AllocationExecQuantity));
            //ViewBag.ImpossibleAllocCount = String.Format("引当不能数：{0} 件", list.Sum((x) =>
            //{
            //    var total = 0;
            //    if (x.FixStockRemaining < 0)
            //    {
            //        total += System.Math.Abs(x.FixStockRemaining);
            //    }
            //    return total;

            //}));

            TempData["SampleList"] = list;

            return View(list);
        }
        public ActionResult AllocationPlanListFixResult()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPlanFix;

            var list = TempData["SampleList"] as List<DummyStock>;
            if (list == null || list.Count <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllocCount = String.Format("{0} 件", list.Sum(x => x.AllocationQuantity));
            //ViewBag.ImpossibleAllocCount = String.Format("{0} 件", list.Sum((x) =>
            //{
            //    var total = 0;
            //    if (x.FixStockRemaining < 0)
            //    {
            //        total += System.Math.Abs(x.FixStockRemaining);
            //    }
            //    return total;

            //}));

            TempData["SampleList"] = list;

            ViewBag.ResultMessage = "引当計画を確定しました。";

            return View(list);
        }
        public ActionResult AllocationPlanListFixCancelConfirm()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPlanFixCancel;

            ViewBag.ConfirmMessage = "以下のデータの引当計画確定済を解除します。よろしいですか？";

            var list = CreateSampleData();

            ViewBag.AllocCount = String.Format("引当件数（確定済引当件数）：{0} 件", list.Sum(x => x.AllocationExecQuantity));
            //ViewBag.ImpossibleAllocCount = String.Format("引当不能数：{0} 件", list.Sum((x) =>
            //{
            //    var total = 0;
            //    if (x.FixStockRemaining < 0)
            //    {
            //        total += System.Math.Abs(x.FixStockRemaining);
            //    }
            //    return total;

            //}));

            TempData["SampleList"] = list;

            return View(list);
        }
        public ActionResult AllocationPlanListFixCancelResult()
        {
            ViewBag.StockScreen = StockScreens.StockAllocationPlanFixCancel;

            var list = TempData["SampleList"] as List<DummyStock>;
            if (list == null || list.Count <= 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AllocCount = String.Format("{0} 件", list.Sum(x => x.AllocationQuantity));
            //ViewBag.ImpossibleAllocCount = String.Format("{0} 件", list.Sum((x) =>
            //{
            //    var total = 0;
            //    if (x.FixStockRemaining < 0)
            //    {
            //        total += System.Math.Abs(x.FixStockRemaining);
            //    }
            //    return total;

            //}));

            TempData["SampleList"] = list;

            ViewBag.ResultMessage = "確定済の引当を解除しました。";

            return View(list);
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult EditConfirm()
        {
            return View();
        }

        public ActionResult DeleteConfirm()
        {
            return View();
        }

        //実施
        public ActionResult AllocationExecConfirm()
        {
            return View();
        }
        //確定
        public ActionResult AllocationPlanFixConfirm()
        {
            return View();
        }
        //実施解除
        public ActionResult AllocationExecCancelConfirm()
        {
            return View();
        }
        //確定解除
        public ActionResult AllocationPlanFixCancelConfirm()
        {
            return View();
        }
        
        //引当状態
        public ActionResult ReserveStatus()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirm(DummyStock stock)
        {
            ViewBag.StockScreen = StockScreens.StockAllocationShow;
            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,
            DummyStock stock, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            try
            {
                var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

                if (ret == ButtonValues.Nothing)
                {
                    return View(nameof(EditConfirm), stock);
                }

                switch (ret)
                {
                    case ButtonValues.Back:
                        TempData["BackData"] = stock;
                        return RedirectToAction(nameof(Edit), id);
                    case ButtonValues.Exec:
                        //if (!ModelState.IsValid)
                        //{
                        //    return View(stock);
                        //}
                        SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                        return RedirectToAction(nameof(Index));
                    default:
                        return View(nameof(EditConfirm), stock);
                }
            }
            catch
            {
                return View(stock);
            }
        }


        //public ActionResult AllocationPlanFixConfirm(long id)
        //{
        //    ViewBag.StockScreen = StockScreens.StockAllocationPlanFix;
        //    var list = TempData["SampleList"] as List<DummyStock>;
        //    if (list == null || list.Count <= 0)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }


        //    var stock = list.Find(x => x.Id == id);
        //    if (stock == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    TempData["SampleList"] = list;


        //    return View(stock);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllocationPlanFix(int id,
            DummyStock stock, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            try
            {
                var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

                if (ret == ButtonValues.Nothing)
                {
                    return View(nameof(EditConfirm), stock);
                }

                switch (ret)
                {
                    case ButtonValues.Back:
                        TempData["BackData"] = stock;
                        return RedirectToAction(nameof(Edit), id);
                    case ButtonValues.Exec:
                        //if (!ModelState.IsValid)
                        //{
                        //    return View(stock);
                        //}
                        SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                        return RedirectToAction(nameof(Index));
                    default:
                        return View(nameof(EditConfirm), stock);
                }
            }
            catch
            {
                return View(stock);
            }
        }
        //public ActionResult AllocationPlanFixCancelConfirm(long id)
        //{
        //    ViewBag.StockScreen = StockScreens.StockAllocationPlanFixCancel;

        //    var list = TempData["SampleList"] as List<DummyStock>;
        //    if (list == null || list.Count <= 0)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }


        //    var stock = list.Find(x => x.Id == id);
        //    if (stock == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    TempData["SampleList"] = list;


        //    return View(stock);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AllocationPlanFixCancel(int id,
            DummyStock stock, ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            try
            {
                var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

                if (ret == ButtonValues.Nothing)
                {
                    return View(nameof(EditConfirm), stock);
                }

                switch (ret)
                {
                    case ButtonValues.Back:
                        TempData["BackData"] = stock;
                        return RedirectToAction(nameof(Edit), id);
                    case ButtonValues.Exec:
                        //if (!ModelState.IsValid)
                        //{
                        //    return View(stock);
                        //}
                        SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                        return RedirectToAction(nameof(Index));
                    default:
                        return View(nameof(EditConfirm), stock);
                }
            }
            catch
            {
                return View(stock);
            }
        }

        // GET: Dcos/StockAllocation/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dcos/StockAllocation/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dcos/StockAllocation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dcos/StockAllocation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
