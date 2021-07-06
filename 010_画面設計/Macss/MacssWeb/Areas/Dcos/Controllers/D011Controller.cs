using Macss.Database.Entity;
using MacssWeb.Areas.Dcos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MacssWeb.Areas.Dcos.Controllers
{
    [Authorize]
    public class D011Controller : StockBaseController
    {
        public ActionResult Index()
        {
            var list = CreateSampleData();

            ViewBag.SumPrevRem = list.Sum(x => x.PreviousMonthRemaining);
            ViewBag.SumIn = list.Sum(x => x.PresentMonthInQuantity);
            ViewBag.SumOut = list.Sum(x => x.PresentMonthOutQuantity);

            TempData["SampleList"] = list;

            return View(list);
        }

        public ActionResult Update()
        {
            var list = TempData["SampleList"] as List<DummyStock>;

            var vm = new D011UpdateViewModel();

            vm.Stocks = list;
            if (list != null && list.Count > 0)
            {
                vm.ClientCode = list[0].ClientCode;
                vm.ClientName = list[0].ClientName;
                vm.ProductCode = list[0].ProductCode;
                vm.ProductName = list[0].ProductName;
                vm.WarehouseCode = list[0].WarehouseCode;
                vm.WarehouseName = list[0].WarehouseName;
                vm.RackNumber = list[0].RackNumber;
                vm.LotNumber = list[0].LotNumber;
                vm.StockLimitYM = list[0].StockLimitYM;
            }
            TempData["SampleList"] = list;
            return View(vm);
        }

        public ActionResult UpdateConfirm(D011UpdateViewModel data)
        {
            ViewBag.ConfirmMessage = "以下の内容で実行します。";

            var list = TempData["SampleList"] as List<DummyStock>;
            data.Stocks = list;
            TempData["SampleList"] = list;

            return View(data);
        }

        [HttpPost]
        public ActionResult UpdateResult(D011UpdateViewModel data)
        {
            ViewBag.ResultMessage = "倉庫・保存期限を一括変更しました。";

            var list = TempData["SampleList"] as List<DummyStock>;

            var cd = "77";
            var nm = String.Format("(株)サンプル{0}倉庫", cd);

            data.WarehouseCode = cd;
            data.WarehouseName = nm;

            list.ForEach(x =>
            {
                x.StockLimitYM = DateTime.Now;
                x.WarehouseCode = cd;
                x.WarehouseName = nm;
            });


            data.Stocks = list;
            TempData["SampleList"] = list;

            return View(data);
        }
    }
}
