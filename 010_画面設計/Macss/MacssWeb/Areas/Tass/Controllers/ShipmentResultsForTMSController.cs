using Macss.Database.Entity;
using MacssWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Controllers
{
    [Authorize]
    public class ShipmentResultsForTMSController : Controller
    {
        protected List<DummyShipmentResults> CreateSampleData(int count = 10)
        {
            var list = new List<DummyShipmentResults>();
            var rand = new Random();

            var number = 11500571;

            var fileNameFormat = "NGN-OFFCOM{0}";

            var custList = new Dictionary<string, string>()
            {
                {"11201", "富士電機エフテック（株）" },
                {"14101", "富士電機メーター（株）" },
                {"16601", "ＦＥ）半導体営業　（松本・ＧＥ）" },
                {"21711", "富士電機（株）　食品流通事業　（流通機器" },
                {"23101", "富士電機メーター（株）ＳＷ電源事業部（松" },
                {"27201", "ＦＥ）半導体営業" },
                {"40211", "富士電機（株）営業本部　営業統括室　業務" },
                {"41931", "ＦＥ）電子）生統）松本）チップ製造部" },
                {"43711", "信州富士電機（株）　サービス　（流通機器" },
                {"73821", "トピー実業（株）　自動車部品事業部　松本" },
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
            var transList = new Dictionary<string, string>()
            {
                {"12700112100", "路線便　東海西濃運輸（株）" },
                {"15700422100", "路線便　北陸名鉄運輸（株）富山支店" },
                {"21700412100", "路線便　信州名鉄運輸（株）（トピー用）" },
                {"22700112100", "路線便　西濃運輸（株）（トピー用）" },
                {"24700412100", "路線便　信州名鉄運輸（株）　路線便限定" },
                {"25802112100", "幹線便・埼玉便" },
                {"26803312100", "幹線便・京浜支社便" },
                {"31702112101", "宅配便　ヤマト運輸（株）" },
                {"38702122101", "宅配便　ヤマト運輸（株）（東京堂（株））" },
                {"80702112400", "定期便　その他" },
            };

            var slipList = new List<string>()
            {
                "1960790",
                "47679",
                "57838+05",
                "64923+03",
                "8506414",
                "9506732",
                "3264390",
                "4735691",
                "6507003",
                "88020+01",
                "9960463",
                "72012506",
                "776884",
                "775470",
                "7094394+02",
            };

            var productList = new List<string>()
            {
                "１４Ｘ５Ｊ Ｐ－Ａ／Ｃ６ＨＡ３８．　５　ＭＶ６Ａ",
                "Ｆ７ＨＦ－Ｒ，２００Ｖ，４００／５Ａ，５０ＨＺ＜ＳＨＪ，ケンツキ",
                "洗浄部品（メタル一式）",
                "ＦＨ０８００ＨＶ／ＲＦ",
                "ＣＳＣ７７７－Ｘ２ＮＫＳ０３０６",
                "ＫＯＮＡ自転車",
                "４５７４８１４１２６０３",
                "ＣＳＣ７７７１９８－０３８５４Ｎ",
                "６ＭＢＰ１５０ＶＣＣ０６０－５１",
                "ＹＧ９７１Ｓ８ＲＳＣ",
                "デンソーデバイス向け導電コンテナ一式",
                "トピー　タンケツ　ＴＦ３５８　１２０Ｘ　４００　４　１００　Ｓ",
                "ＰＲＤ　ＴＳ７　１７７０　３８　５ＣＭＳ　　４２０００９",
                "到着便",
                "通い箱・空パレット",
            };

            var accNumList = new List<string>()
            {
                "RS2010",
                "RS2010",
                "RS2010",
                "RS2010",
                "NMNB89  01",
                "DG91287  01",
                "NRHL08  01",
                "NNXC85  01",
                "DG90984  01",
                "DG90763  03",
            };

            var billList = new List<string>()
            {
                "45901",
                "90401"
            };

            var tcList = new List<string>()
            {
                "2301",
                "2101"
            };

            for (int i = 0; i < count; i++)
            {
                var data = new DummyShipmentResults();

                data.Id = i;

                var type = rand.Next(1, 10);
                data.DataType = (DummyShipmentResults.DataTypes)Enum.ToObject(
                                        typeof(DummyShipmentResults.DataTypes), (type % 2) + 1);

                var num = rand.Next(-500, 1000);
                data.MainShipmentNumber = (number + num).ToString();

                var ymd = rand.Next(0, 10);
                DateTime shipYMD = DateTime.Now.Subtract(new TimeSpan(ymd, 0, 0, 0));
                data.ShipmentYMD2 = shipYMD;

                var cust = rand.Next(0, 9);
                var custKey = custList.Keys.ToArray()[cust];
                data.CustomerCode = custKey;
                data.CustomerName = custList[custKey];

                var ware = rand.Next(0, 9);
                var wareKey = wareHouseList.Keys.ToArray()[ware];
                data.WarehouseCode = wareKey;
                data.WarehouseName = wareHouseList[wareKey];

                var trans = rand.Next(0, 9);
                var transKey = transList.Keys.ToArray()[trans];
                data.TransportationCode = transKey;
                data.TransportationName = transList[transKey];

                var slip = rand.Next(0, 14);
                data.CustomerSlipNumber = slipList[slip];

                var acc = rand.Next(0, 9);
                data.AccountingControlNumber = accNumList[acc];

                data.ShipmentYMD = data.ShipmentYMD2;

                data.DeliveryPeriod = null;

                var weight = rand.Next(111, 9999);
                data.ShipmentTotalWeight = ((decimal)weight) / 100;

                data.ShipmentBulk = 0;

                data.Note = "";

                data.LoadingLocationName = "富士物流（株）松本物流センター";

                data.LoadingLocationAddress = "長野県松本市笹賀７９４１";

                data.LoadingLocationAddressDetail = "";

                data.LoadingLocationZipCode = "3990033";

                data.LoadingLocationPhoneNumber = "0263-99-9999";

                data.LoadingLocationDepartment = "";
                data.LoadingLocationStaff = "B3";

                data.LoadingLocationNote = data.AccountingControlNumber;

                data.DestinationName = "富士物流（株）丸子出張所製品倉庫";

                data.DestinationAddress = "長野県松本市笹賀７９４１ - 1";

                data.DestinationAddressDetail = "";

                data.DestinationZipCode = "3990033";

                data.DestinationPhoneNumber = "0263-99-9999";

                data.DestinationDepartment = "";
                data.DestinationStaff = "";

                data.DestinationNote = "";

                var invoice = rand.Next(-9999, 9999);
                data.CarrierInvoiceNumber = (467248096416 + invoice).ToString();

                var product = rand.Next(0, 14);
                data.ProductName = productList[product];

                var num1 = rand.Next(1, 500);
                data.NumberOfShipments = num1;
                var num2 = rand.Next(1, 200);
                data.NumberOfShipmentCases = num2;

                data.PcCode = "8031";

                var day = rand.Next(0, 10);
                data.ArrivalYMD = DateTime.Now.AddDays(day);

                data.RegistrationYMD = DateTime.Now;
                data.RegistrationUser = "*UDB601";
                data.RegistrationWSName = "EDP";

                data.DeliveryYMD = DateTime.Now;

                data.ReceivingYMD = null;


                data.TMSFileName = String.Format(fileNameFormat, data.PcCode);

                var bill = rand.Next(0, 1);
                data.Billing = billList[bill];

                data.TMSPartner = "S4001H";

                data.Supplier = "70011 試験運輸㈱ 松本本社";

                data.CreatedBy = "DU15";

                var tc = rand.Next(0, 1);
                data.TransportationCategory = tcList[tc];

                data.RegistrationUserName = "富士　太郎";

                data.FormCategory = "1";

                data.ErrorMessage = "ＴＭＳに業務登録がされていません";

                list.Add(data);
            }

            return list;
        }

        // GET: Tass/ShipmentResultsForTMS
        public ActionResult Index()
        {
            var list = CreateSampleData();


            TempData["SampleList"] = list;

            return View(list);
        }

        // GET: Tass/ShipmentResultsForTMS/Details/5
        public ActionResult Details(int? id)
        {
            var list = TempData["SampleList"] as List<DummyShipmentResults>;

            if (!id.HasValue || list == null || list.Count <= 0)
            {
                return RedirectToAction("Index");
            }

            var model = list[id.Value];
            return View(model);
        }

        // GET: Tass/ShipmentResultsForTMS/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirm(DummyShipmentResults data)
        {
            var str = String.Empty;

            ViewBag.ConfirmMessage = "以下の内容で実行します。";

            return View(data);
        }

        // POST: Tass/DossData/Create
        [HttpPost]
        public ActionResult Create(DummyShipmentResults data
            , ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            try
            {
                var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

                switch (ret)
                {
                    case ButtonValues.Back:
                        return View(data);
                    case ButtonValues.Exec:
                        if (data.CreateType == DummyShipmentResults.CreateTypes.All)
                        {
                            return RedirectToAction(nameof(CreateResult));
                        }
                        return RedirectToAction(nameof(Index));
                    default:
                        return View(nameof(CreateConfirm), data);
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateResult()
        {
            var list = CreateSampleData();

            var cnt = 6;

            var errList = new List<DummyShipmentResults>(cnt);
            for (int i = 0; i < cnt; i++)
            {
                errList.Add(list[i]);
            }

            ViewBag.ErrorCount = cnt;
            return View(errList);
        }
    }
}
