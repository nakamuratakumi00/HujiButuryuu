using Macss.Database.Entity;
using MacssWeb.Controllers;
using System;
using System.Collections.Generic;

namespace MacssWeb.Areas.Dcos.Controllers
{
    public class StockBaseController : BaseController
    {
        protected List<DummyStock> CreateSampleData(int count = 10)
        {
            var list = new List<DummyStock>();

            var clientCode = "71281";
            var clientName = "サンプル商事(株)";
            var productCode = "1550001";
            var productName = "ＮＯ３７０Ｇ ５×８００００１２ Ａ１２３４５６７８９０";

            var rand = new Random();

            for (int i = 0; i < count; i++)
            {
                var stock = new DummyStock();

                stock.Id = i;
                stock.InOutYMD = DateTime.Now.AddDays(-i);
                stock.InYMD = DateTime.Now.AddDays(-i * 7);
                stock.OutYMD = DateTime.Now.AddDays(-i);
                stock.OutOrderYMD = DateTime.Now.AddDays(-i);
                var tmp1 = rand.Next(1, 99);
                stock.WarehouseCode = tmp1.ToString("00");
                stock.WarehouseName = String.Format("(株)サンプル{0}倉庫", tmp1);
                stock.ClientCode = clientCode;
                stock.ClientName = clientName;
                stock.ProductCode = productCode;
                stock.ProductName = productName;
                stock.Standards = "KIKAKU12";
                stock.Unit = "ｃｓ";
                stock.RackNumber = rand.Next(60, 63).ToString();

                var lot = String.Empty;
                var tmp2 = rand.Next(3, 14);
                if (tmp2 % 3 == 0 || tmp2 % 2 == 0)
                {
                    lot = "W58451";
                }
                else if (tmp2 % 7 == 0)
                {
                    lot = "W58453";
                }

                stock.LotNumber = lot;
                stock.LotNumberQuantity = 100;

                stock.PreviousMonthRemaining = rand.Next(0, 100);
                stock.PresentMonthInQuantity = rand.Next(0, 100);
                stock.PresentMonthOutQuantity = rand.Next(0, 100);
                stock.PresentMonthRemaining = stock.PreviousMonthRemaining + stock.PresentMonthInQuantity - stock.PresentMonthOutQuantity;

                stock.StockQuantity = rand.Next(0, 100);
                stock.AllocationQuantity = rand.Next(0, 30);
                stock.OutOrderQuantity = rand.Next(0, 5);

                if (i % 3 == 0 || i % 2 == 0)
                {
                    stock.BeforeInOutYMD = DateTime.Now;
                    stock.BeforeRackNumber = rand.Next(60, 63).ToString();
                }

                if (i % 7 == 0)
                {
                    stock.StockLimitYM = DateTime.Now;
                }

                var picking = rand.Next(0, 3);
                if (picking == 0)
                {
                    stock.PickingListOutput = DummyStock.PickingListOutputStatus.Nothing;
                }
                else if (picking == 1)
                {
                    stock.PickingListOutput = DummyStock.PickingListOutputStatus.Output;
                }
                else
                {
                    stock.PickingListOutput = DummyStock.PickingListOutputStatus.NoOutput;
                }

                stock.Status = (DummyStock.StockStatus)Enum.ToObject(typeof(DummyStock.StockStatus), ((i % 2) + 1));

                list.Add(stock);
            }

            return list;
        }
    }
}