using Macss.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Controllers
{
    [Authorize]
    public class ShippingCostForTMSController : Controller
    {
        protected List<DummyShippingCost> CreateSampleData(int count = 10)
        {
            var list = new List<DummyShippingCost>();
            var rand = new Random();

            var supplierList = new List<string>()
            {
                "70011",
                "70041",
                "70042",
            };

            var shipmentNumList = new List<string>()
            {
                "SS0067348",
                "S822114030",
                "1115740",
                "11700415",
                "2268787",
                "386949",
                "11500622",
                "7311443",
            };

            var tcList = new List<string>()
            {
                "2100",
                "2101",
            };

            var slipNumList = new List<string>()
            {
                "5573811938",
                "5573811992",
                "850371922",
                "850371885",
                "7751406025",
                "7751406121",
            };

            var addressList = new List<string>()
            {
                "大阪府吹田市",
                "京都府京都市中京区",
                "神奈川県川崎市川崎区",
                "兵庫県明石市",
                "長野県松本市",
                "京都府久世郡久御山町",
                "長野県安曇野市",
                "静岡県静岡市駿河区",
                "奈良県大和郡山市",
                "埼玉県さいたま市中央区",
            };

            var clientList = new List<string>()
            {
                "53501",
                "27201",
                "42051",
                "70701",
                "11201",
                "15321",
            };
            var wareHouseList = new Dictionary<string, string>()
            {
                {"S2R", "富士物流（株）　丸子出張所" },
                {"S1", "信州富士電機（株）" },
                {"B1R", "富士物流（株）　構内分室" },
                {"53R", "富士物流（株）　二子物流センター（トピー" },
                {"A1", "富士電機メーター（株）" },
                {"E1R", "営業在庫　半導体営業在庫（Ｅ１）" },
                {"B0R", "富士物流（株）　二子物流センター" },
                {"H1R", "ＦＰＳ北陸工場" },
                {"B8R", "富士物流（株）ＳＷ電源ＦＭＣ倉庫" },
                {"B3R", "ＦＢ半導体出荷センター" },
            };

            for (int i = 0; i < count; i++)
            {
                var data = new DummyShippingCost();

                data.Id = i;

                var supp = rand.Next(0, 2);
                data.Supplier = supplierList[supp];

                var num = rand.Next(0, 7);
                data.ShipmentNumber = shipmentNumList[num];

                var tc = rand.Next(0, 1);
                data.TransportationCategory = tcList[tc];

                var ymd = rand.Next(0, 10);
                DateTime shipYMD = DateTime.Now.Subtract(new TimeSpan(ymd, 0, 0, 0));
                data.ShipmentYMD = shipYMD;

                var slip = rand.Next(0, 5);
                data.SupplierSlipNumber = slipNumList[slip];

                var addr = rand.Next(0, 9);
                data.DeliveryAddress = addressList[addr];

                var cli = rand.Next(0, 5);
                data.Client = clientList[cli];

                var num1 = rand.Next(1, 50);
                data.PackingQuantity = num1;

                var weight = rand.Next(111, 9999);
                data.Weight = ((decimal)weight);

                var taxCat = rand.Next(0, 2);
                data.TaxCategory = taxCat.ToString();

                var fee = rand.Next(50, 1500);
                data.Insurance = fee * 10;

                var ins = rand.Next(0, 3);
                data.Insurance = ins * 1000;

                var exp = rand.Next(0, 10);
                data.RelayExpense = exp * 10;

                var relay = rand.Next(0, 50);
                data.RelayExpense = relay * 10;

                data.RegistrationUser = "6103248";

                data.RegistrationWSName = "";

                data.ErrorMessage = "仕入先原票No.が空白です";

                var ware = rand.Next(0, 9);
                var wareKey = wareHouseList.Keys.ToArray()[ware];
                data.OutLocation = wareKey;

                list.Add(data);
            }

            return list;
        }

        public ActionResult Index()
        {
            var list = CreateSampleData();


            TempData["SampleList"] = list;

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DummyShippingCost data)
        {
            try
            {
                return RedirectToAction(nameof(CreateResult));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirm()
        {
            var str = String.Empty;

            ViewBag.ConfirmMessage = "以下の内容で実行します。";

            return View();
        }

        public ActionResult CreateResult()
        {
            var list = CreateSampleData();

            var cnt = 6;

            var errList = new List<DummyShippingCost>(cnt);
            for (int i = 0; i < cnt; i++)
            {
                errList.Add(list[i]);
            }

            ViewBag.ErrorCount = cnt;
            return View(errList);
        }
    }
}
