using Macss.Database.Entity;
using MacssWeb.Areas.Tass.Models;
using MacssWeb.Common;
using MacssWeb.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Controllers
{
    class LocationInformation
    {
        public LocationInformation(string name, string jisCode, string zipCode
            , string address, string adressDetail, string phoneNumber, string department, string staff)
        {
            this.Name = name;
            this.JisCode = jisCode;
            this.ZipCode = zipCode;
            this.Address = address;
            this.AddressDetail = adressDetail;
            this.PhoneNumber = phoneNumber;
            this.Department = department;
            this.Staff = staff;
        }

        public string Name { get; set; }
        public string JisCode { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string AddressDetail { get; set; }
        public string PhoneNumber { get; set; }

        public string Department { get; set; }
        public string Staff { get; set; }
    }

    [Authorize]
    public class TMSFixedDataController : BaseController
    {
        protected TMSFixedDataListViewModel CreateSampleData(int count = 10)
        {
            var m = new TMSFixedDataListViewModel();

            var req = CreateRequestSampleData(count);
            m.TMSFixedDataRequests = req;

            var bil = CreateBillingSampleData(count);
            m.TMSFixedDataBillings = bil;

            var pay = CreatePaymentSampleData(count);
            m.TMSFixedDataPayments = pay;

            return m;
        }
        protected List<DummyTMSFixedDataRequest> CreateRequestSampleData(int count = 10)
        {
            var list = new List<DummyTMSFixedDataRequest>();
            var rand = new Random();

            var acceptNumberStart = 2160172587700;
            var orderNumberStart = 2160172569900;

            var customerList = new Dictionary<string, string>()
            {
                {"35771", "ＦＥ）松本工場　（松本・ＧＱ・原価）" },
                {"40211", "富士電機㈱営業本部　営業企画室　総務部　営業助成課" },
                {"27001", "ＦＥ）営業）パワ）第三）営業一）第一Ｇｒ" },
                {"16401", "ＦＥ）半導体営業　（松本・ＧＣ）" },
                {"37971", "ＦＥ）松本工場　（松本・ＤＲ・原価）" },
                {"43111", "ＦＥ）電子）開統）パッケージ実装開発部" },
                {"55001", "（株）富士電機フロンティア　松本事業所" },
                {"11201", "富士電機エフテック（株）" },
                {"51061", "富士電機パワーセミコンダクタ（株）【大町】" },
                {"51011", "富士電機パワーセミコンダクタ（株）【北陸】" },
            };
            var operationList = new Dictionary<string, string>()
            {
                {"6017000002951", "ＦＢ松本自社便「個口配送料（路線便）_Ｆe用」20170601" },
                {"6017000001868", "貸切便" },
                {"6017000000028", "ファナック便" },
                {"6017900000505", "宅配便" },
                {" ", " " },
                {"6017000003310", "路線" },
                {"6017000002966", "宅配" },
            };

            var transModeList = new List<string>()
            {
                "路線便",
                "貸切便",
                "宅配便/個建",
            };

            var custSlipNumList = new List<string>()
            {
                "I/WS212487324",
                "I/A5923",
                "381121",
                "1102222+02",
                "S212489130",
                "",
            };

            var noteList = new List<string>()
            {
                "!CC04614348! #333594889692#",
                "NVNK90   01 ",
                "",
                "NVXC48   01  ",
                "NXPR71   01  ",
            };

            var unloadNoteList = new List<string>()
            {
                "１／７必着",
                "１／１２必着",
                "",
                "在庫補充",
                "最短納期",
                "１／７（木）着！！　　　　　　　　　　　＜割れ物につき取扱注意＞",
                "１／７　ＡＭ１０：００必着　＜天地無用＞タイムサービス　割れ物につき取扱注意！！",
            };


            var vehicleClassList = new Dictionary<string, string>()
            {
                {"1", "軽トラ" },
                {"2", "1t" },
                {"5", "4t" },
                {"13", "8t" },
                {"15", "10t" },
                {"21", "13t" },
                {"44", "3t" },
                {" ", " " },
            };

            var equipmentsList = new Dictionary<string, string>()
            {
                {"1", "W" },
                {"2", "PG" },
                {" ", " " },
            };

            var invoNumList = new List<string>()
            {
                "843600003329",
                "843600003330",
                "843600003331",
                "",
            };

            var productList = new List<string>()
            {
                "RB105-DE/054",
                "AC09-GX0/7L3B02/0013",
                "RZ4N",
                "車載品",
                "パワトラ",
                "AH165-ZWE3(CCC)",
                "２ＭＢＩ３００ＸＧＥ１２０－５１　ＮＥＴ",
                "Ｓｉウェハ",
                "形状①②（２０Ｃ０８６４　他一式）",
                "仕様書",
                "サンプル",
                "ＦＣＳ２５５４ＴＫＸ－２０Ｂ－３５９，他",
                "ＲＰＮＥ２４５０－Ａ１－ＺＡＭ／ＵＬ",
                "ポリ容器（黄箱：１分析用サンプル通い箱・空パレット",
            };

            var clientShipmentSizeList = new Dictionary<string, string>()
            {
                {"140020", "FB松本（ヤマト）5kgまで：80ｻｲｽﾞ　" },
                {"140100", "FB松本（15kgまで）　" },
                {"140060", "FB松本（ヤマト）25kgまで：160ｻｲｽﾞ　" },
                {"140050", "FB松本（ヤマト）20kgまで：140ｻｲｽﾞ　" },
                {"140070", "FB松本（2kgまで）　" },
                {" ", " " },
            };

            var partnerShipmentSizeList = new Dictionary<string, string>()
            {
                {"90010030", "100サイズ (10Kgまで)ヤマト" },
                {"90570040", "（15Kgまで）FB松本" },
                {"90010040", "120サイズ (15Kgまで)ヤマト" },
                {"90010060", "160サイズ (25Kgまで)ヤマト" },
                {" ", " " },
            };

            var partnerList = new Dictionary<string, string>()
            {
                {"4100090401", "富士物流（株）　松本支社　松本物流センター（９０４０１）" },
                {"Y1019A", "ヤマト運輸（株）　松本主管支店" },
                {"P70010", "富士物流㈱基幹配送便（京浜支社）" },
                {"S2030B", "信州名鉄運輸（株）" },
                {"J73801", "東海西濃運輸（株）　松本支店" },
                {"C84801", "赤帽長野県軽自動車運送協同組合　上田配車センター" },
            };


            var driverList = new List<string>()
            {
                "5号車（68-08）",
                "1号車（66-21）",
                ""
            };

            var loadLocationList = new List<LocationInformation>
            {
                new LocationInformation("富士物流株式会社　松本物流センター"
                , "20202","3990033","長野県松本市", "大字笹賀7941", "0263-26-2626", "", "EA"),
                new LocationInformation("Ｂ０富士物流㈱　松本物流センター"
                , "20202","3990033","長野県松本市", "大字笹賀7941", "0263-26-2626", "", "B0"),
                new LocationInformation("富士電機㈱山梨製作所"
                , "19208","","山梨県南アルプス市", "飯野221-1", "00552851111", "", "K1"),
                new LocationInformation("富士物流（株）構内分室"
                , "20202","3900821","長野県松本市筑摩４－１８－１", "", "0263-26-2626", "", "B1"),
                new LocationInformation("富士物流（株）松本物流センター"
                , "20202","3990033","長野県松本市笹賀７９４１", "", "0263-26-2626", "", "E1"),

            };

            var unloadLocationList = new List<LocationInformation>
            {
                new LocationInformation("Ｂ１富士物流㈱　ＦＥＳ構内分室"
                , "20202","3990033","長野県松本市", "筑摩４－１８－１", "0263-26-2626", "", "B1"),
                new LocationInformation("ファナック（株）"
                , "19424","","山梨県南都留郡忍野村", "忍草字古馬場3580", "", "", ""),
                new LocationInformation("富士電機㈱松本工場"
                , "20202","","長野県松本市", "筑摩４－１８－１", "0263-26-2626", "", "B1"),
                new LocationInformation("沖電気工業（株）　北関東試験センター"
                , "20202","3900821","長野県松本市筑摩４－１８－１", "", "0263-26-2626", "", "B1"),
                new LocationInformation("富士電機株式会社　中部支社商品センター"
                , "20202","3990033","愛知県名古屋市中川区愛知町５番１号富士物流（株）内２Ｆ", "", "0263-26-2626", "", ""),
                new LocationInformation("富士電機パワーセミコンダクタ（株）　東部材倉庫"
                , "20202","3990033","長野県大町市常盤６９０９", "", "0263-26-2626", "", ""),

            };

            var statusList = new Dictionary<string, string>()
            {
                {"40", "計上済" },
            };

            var billPayList = new List<string>()
            {
                "請求",
                "支払",
                "支払"
            };
            var importList = new List<string>()
            {
                "済",
                ""
            };


            for (int i = 0; i < count; i++)
            {
                var data = new DummyTMSFixedDataRequest();

                data.Id = i;

                data.ImportedIndex = i + 1;
                data.ImportedAt = DateTime.Now;
                data.UploadErrorMessage = "すでに取込済みのデータです。";
                data.ErrorCode = "123";
                data.ImportedErrorMessage = "出荷場所が未指定です。";
                data.RecordIdCategory = "11";

                data.AcceptanceNumber = (acceptNumberStart + i).ToString();

                var cust = rand.Next(0, 9);
                data.CustomerCode = customerList.Keys.ToArray()[cust];
                data.CustomerName = customerList[data.CustomerCode];

                data.AcceptanceYMD = DateTime.Now;

                data.ProgressCode = "12";
                data.ProgressName = "配送済";

                var ope = rand.Next(0, 6);
                data.OperationNumber = operationList.Keys.ToArray()[ope];
                data.OperationName = operationList[data.OperationNumber];

                var trn1 = rand.Next(0, 2);
                data.TransportationModeForWorkRequest = transModeList[trn1];

                var cs = rand.Next(0, 5);
                data.CustomerSlipNumber = custSlipNumList[cs];

                data.ShipingCostTypeForWorkRequest = "1";
                data.ShipingCostForWorkRequest = null;

                data.ShipmentCategoryCode = String.Empty;
                data.ShipmentCategoryName = String.Empty;

                data.ShipmentPackingStyleCode = String.Empty;
                data.ShipmentPackingStyleName = String.Empty;

                var weight = rand.Next(1, 9999);
                data.ShipmentWeight = ((decimal)weight);

                if (i % 2 == 0)
                {
                    data.ShipmentBulk = null;
                }
                else
                {
                    var bulk = rand.Next(30, 800);
                    data.ShipmentBulk = bulk;
                }

                data.StaffTimeCategory = "0";
                data.NumberOfStaff = null;

                var note = rand.Next(0, 4);
                data.NoteForWorkRequest = noteList[note];

                var vc = rand.Next(0, 7);
                data.VehicleClassCodeForWorkRequest = vehicleClassList.Keys.ToArray()[vc];
                data.VehicleClassNameForWorkRequest = vehicleClassList[data.VehicleClassCodeForWorkRequest];

                var eq = rand.Next(0, 2);
                data.EquipmentsCodeForWorkRequest = equipmentsList.Keys.ToArray()[eq];
                data.EquipmentsNameForWorkRequest = equipmentsList[data.EquipmentsCodeForWorkRequest];

                var invo = rand.Next(0, 3);
                data.InvoiceNumber = invoNumList[invo];

                var q = rand.Next(1, 800);
                data.Quantity = q;

                data.TimeAppointed = String.Empty;

                var product = rand.Next(0, 14);
                data.ProductName = productList[product];

                var pq = rand.Next(1, 800);
                data.PackageQuantity = pq;

                var css = rand.Next(0, 5);
                data.ClientShipmentSizeCode = clientShipmentSizeList.Keys.ToArray()[css];
                data.ClientShipmentSizeName = clientShipmentSizeList[data.ClientShipmentSizeCode];

                var pss = rand.Next(0, 4);
                data.PartnerShipmentSizeCode = partnerShipmentSizeList.Keys.ToArray()[pss];
                data.PartnerShipmentSizeName = partnerShipmentSizeList[data.PartnerShipmentSizeCode];

                data.OrderNumber = (orderNumberStart + i).ToString();

                data.OrderYMD = DateTime.Now;

                var ptn = rand.Next(0, 5);
                data.PartnerCode = partnerList.Keys.ToArray()[ptn];
                data.PartnerName = partnerList[data.PartnerCode];

                data.VehicleClassCodeForOrder = data.VehicleClassCodeForWorkRequest;
                data.VehicleClassNameForOrder = data.VehicleClassNameForWorkRequest;
                data.EquipmentsCodeForOrder = data.EquipmentsCodeForWorkRequest;
                data.EquipmentsNameForOrder = data.EquipmentsNameForWorkRequest;

                data.VehicleNumber = "44289";

                var dr = rand.Next(0, 2);
                data.Driver = driverList[dr];

                data.DriverContactInformation = String.Empty;

                data.TransportationModeForOrder = data.TransportationModeForWorkRequest;
                data.ShipingCostTypeForOrder = data.ShipingCostTypeForWorkRequest;
                data.ShipingCostForOrder = data.ShipingCostForWorkRequest;

                var ymd = rand.Next(0, 10);
                DateTime loadFromYMD = DateTime.Now.Subtract(new TimeSpan(ymd, 0, 0, 0));
                data.LoadingAtFrom = loadFromYMD;
                data.LoadingAtTo = null;

                var ldLoc = rand.Next(0, 4);
                var loadLoc = loadLocationList[ldLoc];
                data.LoadingName = loadLoc.Name;
                data.LoadingLocationJIS = loadLoc.JisCode;
                data.LoadingLocationZipCode = loadLoc.ZipCode;
                data.LoadingLocation = loadLoc.Address;
                data.LoadingLocationDetail = loadLoc.AddressDetail;
                data.LoadingLocationPhoneNumber = loadLoc.PhoneNumber;
                data.LoadingLocationDepartment = loadLoc.Department;
                data.LoadingLocationStaff = loadLoc.Staff;

                data.LoadingNote = data.NoteForWorkRequest;

                data.LoadingSiteFlag = "0";

                var loadCat = rand.Next(1, 4);
                data.LoadingCategory = loadCat.ToString();

                DateTime unloadFromYMD = loadFromYMD.AddDays(1);
                data.UnloadingAtFrom = null;
                data.UnloadingAtTo = unloadFromYMD;


                var unldLoc = rand.Next(0, 4);
                var unloadLoc = unloadLocationList[unldLoc];
                data.UnloadingName = unloadLoc.Name;
                data.UnloadingLocationJIS = unloadLoc.JisCode;
                data.UnloadingLocationZipCode = unloadLoc.ZipCode;
                data.UnloadingLocation = unloadLoc.Address;
                data.UnloadingLocationDetail = unloadLoc.AddressDetail;
                data.UnloadingLocationPhoneNumber = unloadLoc.PhoneNumber;
                data.UnloadingLocationDepartment = unloadLoc.Department;
                data.UnloadingLocationStaff = unloadLoc.Staff;

                var unloadNote = rand.Next(0, 6);
                data.UnloadingNote = unloadNoteList[note];

                data.UnloadingSiteFlag = "0";

                var bilStatus = 0;
                data.BillingRecordedStatusCode1 = statusList.Keys.ToArray()[bilStatus];
                data.BillingRecordedStatusName1 = statusList[data.BillingRecordedStatusCode1];
                data.BillingRecordedStatusCode2 = String.Empty;
                data.BillingRecordedStatusName2 = String.Empty;

                var payStatus = 0;
                data.PaymentRecordedStatusCode1 = statusList.Keys.ToArray()[payStatus];
                data.PaymentRecordedStatusName1 = statusList[data.PaymentRecordedStatusCode1];
                data.PaymentRecordedStatusCode2 = String.Empty;
                data.PaymentRecordedStatusName2 = String.Empty;

                var billDist = rand.Next(10, 300);
                data.BillingDistance = billDist;

                var payDist = rand.Next(10, 300);
                data.PaymentDistance = payDist;

                var cat = rand.Next(3, 12);
                var catNum = cat % 3;
                data.Category = billPayList[catNum];

                var imp = rand.Next(0, 1);
                data.Imported = importList[imp];

                //data.ErrorMessage = "仕入先原票No.が空白です";


                list.Add(data);
            }

            return list;
        }

        protected List<DummyTMSFixedDataBilling> CreateBillingSampleData(int count = 10)
        {
            var list = new List<DummyTMSFixedDataBilling>();
            var rand = new Random();

            var acceptNumberStart = 2160172587700;

            var customerList = new Dictionary<string, string>()
            {
                {"11201", "富士電機エフテック（株）" },
                {"11311", "富士電機（株）　食品流通事業　三重工場　（通貨機器・信州分）" },
                {"15321", "ＦＥ）半導体営業　（北陸・ＤＹ）" },
                {"15571", "ＦＥ）松本工場　（松本・ＤＬ）" },
                {"15821", "ＦＥ）半導体営業　（北陸・ＤＵ）" },
                {"21311", "富士電機（株）　食品流通事業　（通貨機器・信州分）" },
                {"27201", "ＦＥ）半導体営業" },
                {"44801", "ＦＥ）電子）生統）松本）チップ製造技術部" },
                {"51061", "富士電機パワーセミコンダクタ（株）【大町】" },
                {"73821", "トピー実業（株）　自動車部品事業部　松本営業所" },
            };

            var custonerGroupList = new List<string>()
            {
                "B59706",
                "B87049",
                "B13037",
                "B13037",
                "B13037",
                "H3003A41",
                "B13038",
                "C24480",
                "F41903",
                "E93501",

            };

            var accountTitleList = new Dictionary<string, string>()
            {
                {"1000", "基本運賃" },
                {"1007", "その他(外税)" },
                {"4001", "中継料" },
            };

            var custSlipNumList = new List<string>()
            {
                "I/WS212487324",
                "I/A5923",
                "381121",
                "1102222+02",
                "S212489130",
                "",
            };

            var statusList = new Dictionary<string, string>()
            {
                {"30", "未計上" },
                {"40", "計上済" },
            };

            for (int i = 0; i < count; i++)
            {
                var data = new DummyTMSFixedDataBilling();

                data.Id = i;
                data.ImportedIndex = i + 1;
                data.ImportedAt = DateTime.Now;
                data.UploadErrorMessage = "すでに取込済みのデータです。";

                var cust = rand.Next(0, 9);
                data.CustomerCode = customerList.Keys.ToArray()[cust];
                data.CustomerName = customerList[data.CustomerCode];
                data.CustomerGroupNumber = custonerGroupList[cust];

                data.OperationGroupNumber = String.Empty;

                data.AcceptanceNumber = (acceptNumberStart + i).ToString();
                data.AcceptanceYMD = DateTime.Now;

                var cs = rand.Next(0, 5);
                data.CustomerSlipNumber = custSlipNumList[cs];

                data.SaleNumber = (acceptNumberStart + 100 + i).ToString();

                data.ProcessYMD = DateTime.Now;

                data.PcCode1 = "8031";
                data.PcCode2 = "1001";
                data.PcCode3 = "1106";

                data.AccountItemCode = "23011";

                var money = rand.Next(1, 60);
                data.Amount = money * 10;

                data.TaxCategory = "0";
                data.TaxRate = 0.1M;

                data.OutputCategory = "0";

                var at = rand.Next(0, 2);
                data.AccountTitleCode = accountTitleList.Keys.ToArray()[at];
                data.AccountTitleName = accountTitleList[data.AccountTitleCode];

                data.AccountingDeadline = 31;

                data.AmountExcludeTax = data.Amount;

                var money2 = rand.Next(1, 150);
                data.TotalAmountExcludeTax = money2 * 10;

                data.RoundedAmount = 0;

                var bilStatus = 0;
                data.BillingRecordedStatusCode1 = statusList.Keys.ToArray()[bilStatus];
                data.BillingRecordedStatusName1 = statusList[data.BillingRecordedStatusCode1];
                data.BillingRecordedStatusCode2 = String.Empty;
                data.BillingRecordedStatusName2 = String.Empty;

                var billDist = rand.Next(10, 300);
                data.BillingDistance = billDist;

                list.Add(data);
            }

            return list;
        }

        protected List<DummyTMSFixedDataPayment> CreatePaymentSampleData(int count = 10)
        {
            var list = new List<DummyTMSFixedDataPayment>();
            var rand = new Random();

            var acceptNumberStart = 2160172587700;
            var orderNumberStart = 2160172569900;

            var partnerList = new Dictionary<string, string>()
            {
                {"4100090401", "富士物流（株）　松本支社　松本物流センター（９０４０１）" },
                {"Y1019A", "ヤマト運輸（株）　松本主管支店" },
                {"P70010", "富士物流㈱基幹配送便（京浜支社）" },
                {"S2030B", "信州名鉄運輸（株）" },
                {"J73801", "東海西濃運輸（株）　松本支店" },
                {"C84801", "赤帽長野県軽自動車運送協同組合　上田配車センター" },
            };


            var accountTitleList = new Dictionary<string, string>()
            {
                {"1000", "基本運賃" },
                {"1007", "その他(外税)" },
                {"4001", "中継料" },
            };

            var custSlipNumList = new List<string>()
            {
                "I/WS212487324",
                "I/A5923",
                "381121",
                "1102222+02",
                "S212489130",
                "",
            };

            var statusList = new Dictionary<string, string>()
            {
                {"30", "未計上" },
                {"40", "計上済" },
            };

            for (int i = 0; i < count; i++)
            {
                var data = new DummyTMSFixedDataPayment();

                data.Id = i;
                data.ImportedIndex = i + 1;
                data.ImportedAt = DateTime.Now;
                data.UploadErrorMessage = "すでに取込済みのデータです。";

                var ptn = rand.Next(0, 5);
                data.PartnerCode = partnerList.Keys.ToArray()[ptn];
                data.PartnerName = partnerList[data.PartnerCode];

                data.OperationGroupNumber = String.Empty;

                data.OrderNumber = (orderNumberStart + i).ToString();
                data.OrderYMD = DateTime.Now;

                data.PurchaseNumber = (orderNumberStart + 100 + i).ToString();

                data.AcceptanceNumber = (acceptNumberStart + i).ToString();
                data.AcceptanceYMD = DateTime.Now;

                data.ProcessYMD = DateTime.Now;

                var cs = rand.Next(0, 5);
                data.CustomerSlipNumber = custSlipNumList[cs];

                data.PcCode1 = "8035";
                data.PcCode2 = "1003";
                data.PcCode3 = "1040";

                data.AccountItemCode = "31110";

                var money = rand.Next(100, 12000);
                data.Amount = money * 10;

                data.TaxCategory = "0";
                data.TaxRate = 0.1M;

                data.OutputCategory = "0";

                var at = rand.Next(0, 2);
                data.AccountTitleCode = accountTitleList.Keys.ToArray()[at];
                data.AccountTitleName = accountTitleList[data.AccountTitleCode];

                data.AccountingDeadline = 31;

                data.AmountExcludeTax = data.Amount;

                var money2 = rand.Next(1, 150);
                data.TotalAmount = data.Amount;

                var payStatus = 0;
                data.PaymentRecordedStatusCode1 = statusList.Keys.ToArray()[payStatus];
                data.PaymentRecordedStatusName1 = statusList[data.PaymentRecordedStatusCode1];
                data.PaymentRecordedStatusCode2 = String.Empty;
                data.PaymentRecordedStatusName2 = String.Empty;

                var payDist = rand.Next(10, 800);
                data.PaymentDistance = payDist;

                list.Add(data);
            }

            return list;
        }

        public ActionResult Index()
        {
            var vm = CreateSampleData();
            var dataReq = vm.TMSFixedDataRequests;

            TempData["IndexData"] = vm;

            return View(dataReq);
        }

        // GET: Tass/TMSFixedData/Details/5
        public ActionResult Details(int id)
        {
            var vm = TempData["IndexData"] as TMSFixedDataListViewModel;

            if (vm == null)
            {
                return RedirectToAction("Index");
            }

            var req = vm.TMSFixedDataRequests.ToList();
            var model = req[id];

            TempData["IndexData"] = vm;

            return View(model);
        }

        // GET: Tass/TMSFixedData/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = TempData["IndexData"] as TMSFixedDataListViewModel;

            if (vm == null)
            {
                return RedirectToAction("Index");
            }

            var req = vm.TMSFixedDataRequests.ToList();
            var model = req[id];

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfirm(DummyTMSFixedDataRequest data)
        {
            return View(data);
        }

        // POST: Tass/TMSFixedData/Edit/5
        [HttpPost]
        public ActionResult Edit(DummyTMSFixedDataRequest data
            , ButtonValues? cmdTopBack, ButtonValues? cmdBtmBack, ButtonValues? cmdBtmExec)
        {
            var ret = ButtonUtil.RetrieveSubmitButton(cmdTopBack, cmdBtmBack, cmdBtmExec);

            if (ret == ButtonValues.Nothing)
            {
                return View(nameof(EditConfirm), data);
            }

            switch (ret)
            {
                case ButtonValues.Back:
                    return View(data);
                case ButtonValues.Exec:
                    //if (!ModelState.IsValid)
                    //{
                    //    return View(data);
                    //}
                    SetResultAlertParams(AlertTypes.Success, Resources.Messages.EditSuccess);
                    return RedirectToAction(nameof(Index));
                default:
                    return View(nameof(EditConfirm), data);
            }
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadConfirm(HttpPostedFileWrapper postedFile1
            , HttpPostedFileWrapper postedFile2, HttpPostedFileWrapper[] postedFile3)
        {
            if (postedFile1 == null)
            {
                ViewBag.ErrorMessage = "ファイルがありません。";
                return View(nameof(Upload));
            }

            var ext = Path.GetExtension(postedFile1.FileName);

            if (String.Compare(ext, ".csv", true) != 0)
            {
                ViewBag.ErrorMessage = "拡張子が許可されていません。";
                return View(nameof(Upload));
            }

            var uploadPath = Path.Combine(
                Server.MapPath("~/App_Data/Uploaded/"), Path.GetFileName(postedFile1.FileName));

            //postedFile.SaveAs(uploadPath);


            //var cntNew = 0;
            //var cntUpd = 0;
            //var cntDel = 0;

            var dataReq = new List<DummyTMSFixedDataRequest>();
            var dataBil = new List<DummyTMSFixedDataBilling>();
            var dataPay = new List<DummyTMSFixedDataPayment>();

            //foreach (var item in model)
            //{
            //    var uploadData = new DummyTMSFixedData(item);

            //    uploadData.Index = ++i;

            //    if (i % 3 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Update;
            //        cntUpd++;
            //    }
            //    else if (i % 7 == 0)
            //    {
            //        uploadData.ProcType = ProcTypes.Delete;
            //        cntDel++;
            //    }
            //    else
            //    {
            //        uploadData.ProcType = ProcTypes.New;
            //        cntNew++;
            //    }

            //    list.Add(uploadData);

            //}
            var vm = CreateSampleData();

            //TempData["UploadedList"] = list;

            ViewBag.ConfirmMessage = Resources.Messages.UploadConfirm;

            ViewBag.UploadFileName1 = String.Empty;
            ViewBag.UploadFileContentsCount1 = String.Format("{0:#,0}", 0);
            ViewBag.UploadFileName2 = String.Empty;
            ViewBag.UploadFileContentsCount2 = String.Format("{0:#,0}", 0);
            ViewBag.UploadFileName3 = String.Empty;
            ViewBag.UploadFileContentsCount3 = String.Format("{0:#,0}", 0);

            if (postedFile1 != null)
            {
                ViewBag.UploadFileName1 = postedFile1.FileName;
                ViewBag.UploadFileContentsCount1 = String.Format("{0:#,0}", 15000);
            }
            if (postedFile2 != null)
            {
                ViewBag.UploadFileName2 = postedFile2.FileName;
                ViewBag.UploadFileContentsCount2 = String.Format("{0:#,0}", 500);
            }
            if (postedFile3 != null && postedFile3.Length > 0 && postedFile3[0] != null)
            {
                var sb = new StringBuilder();
                for (int i = 0; i < postedFile3.Length; i++)
                {
                    if (i != 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(postedFile3[i].FileName);
                }
                ViewBag.UploadFileName3 = sb.ToString();
                ViewBag.UploadFileContentsCount3 = String.Format("{0:#,0}", 500);
            }
            //ViewBag.NewCountText = String.Format("新規：{0} 件", cntNew);
            //ViewBag.UpdCountText = String.Format("更新：{0} 件", cntUpd);
            //ViewBag.DelCountText = String.Format("削除：{0} 件", cntDel);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadResult(string uploadFileName)
        {

            TempData["UploadedConfirm"] = "TMSデータの取込を実行しました。\n"
                + "続けて、TMS確定データの登録を行う場合は、データを確認して、[登録]を実行してください。";


            return RedirectToAction("CreateData");

        }

        public ActionResult CreateData()
        {
            ViewBag.ConfirmMessage = Resources.Messages.CreateConfirm;

            var transferConfirm = TempData["UploadedConfirm"] as string;

            if (!String.IsNullOrWhiteSpace(transferConfirm))
            {
                ViewBag.ConfirmMessage = transferConfirm;
            }

            var vm = CreateSampleData();

            return View(vm);
        }

        [HttpPost]
        public ActionResult CreateData(int? id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateConfirm()
        {

            return View();
        }
        //// GET: Tass/TMSFixedData/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Tass/TMSFixedData/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}
