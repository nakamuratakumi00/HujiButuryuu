using Macss.Database.Entity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MacssWeb.Areas.Tass.Controllers
{
    [Authorize]
    public class DossDataController : Controller
    {
        protected List<DummyDossData> CreateSampleData(int count = 10)
        {
            var list = new List<DummyDossData>();
            var rand = new Random();

            var formatKitakantoFileName = "KIT-MATSUMOTO_{0}_25Z.csv";
            var formatAllFileName = "07010020_{1}_{0}.csv";

            var mailSend = true;
            for (int i = 0; i < count; i++)
            {
                var doss = new DummyDossData();

                doss.Id = i;

                var type = rand.Next(1, 10);
                doss.DossDataType = (DummyDossData.DossDataTypes)Enum.ToObject(
                                        typeof(DummyDossData.DossDataTypes), (type % 2) + 1);

                var ymd = rand.Next(0, 10);
                doss.DataCreateYMD = DateTime.Now.Subtract(new TimeSpan(ymd, 0, 0, 0));

                doss.MailSend = !mailSend;

                mailSend = doss.MailSend.Value;

                doss.FileName = String.Format(formatKitakantoFileName, doss.DataCreateYMD.ToString("yyyyMMddHHmm"));

                doss.Location = "-";
                if (doss.DossDataType == DummyDossData.DossDataTypes.All)
                {
                    doss.MailSend = null;
                    var loc = rand.Next(1, 10);
                    doss.Location = "半導体";
                    var locCode = "B3";
                    if (loc % 2 == 0)
                    {
                        doss.Location = "丸子出張所";
                        locCode = "S2";
                    }
                    doss.FileName = String.Format(formatAllFileName, doss.DataCreateYMD.ToString("yyyyMMddHHmm"), locCode);
                }




                list.Add(doss);
            }

            return list;
        }
        protected List<DummyDossDataForAll> CreateDossForAllData(int count = 10)
        {
            var list = new List<DummyDossDataForAll>();
            var rand = new Random();


            for (int i = 0; i < count; i++)
            {
                var doss = new DummyDossDataForAll();

                doss.Id = i;

                doss.OutYMD = DateTime.Now;

                doss.Location = "S3";

                doss.LocationCode = "704";

                doss.OutNumber = String.Format("{0}   {1}", "12014525", "210129");

                doss.RouteCode = "705161";

                doss.RouteName = "松本信州埼玉京浜";

                doss.VehicleCode = "16431";

                doss.DestinationCode = "130024901";

                doss.DestinationName = "富士電機リテイルサービス（株）";

                doss.DestinationAddress = "神奈川県松本市深志区深志神社１－１";

                doss.DestinationZipCode = "3900123";

                doss.DestinationPhoneNumber = "026-333-2222";

                doss.ProductName = "ＣＳＢ７７Ｆ　２０３－５３９１２";

                doss.OutQuantity = rand.Next(1, 99);

                doss.InquiryNumber = "S123456789-99";

                doss.PackageQuantity = rand.Next(1, 9);

                doss.Weight = rand.Next(1, 500);

                doss.ClientCode = "S2256A41";

                doss.ClientShortName = "信州富士電機（株）";

                doss.PcCode = "8080";

                doss.WMSDestinationCode = "FRVA07G52";

                doss.WMSDestinationName = "富士電機リテイルサービス株式会社川崎営業所";

                doss.WMSDestinationAddress = "神奈川県松本市深志区深志神社１－１";

                doss.Note1 = "S2 43711   25  -110000701";

                doss.Note2 = "信州富士電機（株）　サービス　（流通機器）";

                doss.Note3 = "";

                list.Add(doss);
            }

            return list;
        }
        protected List<DummyDossDataForKitaKanto> CreateDossForKitaKantoData(int count = 10)
        {
            var list = new List<DummyDossDataForKitaKanto>();
            var rand = new Random();


            for (int i = 0; i < count; i++)
            {
                var doss = new DummyDossDataForKitaKanto();

                doss.Id = i;

                doss.PackingYMD = DateTime.Now;

                doss.ShippingName = "ｷﾀ-9";

                doss.ShippingCode = "25";

                doss.FBNO = "";

                doss.InvoiceNumber = "ｷﾀ-9";

                doss.DeliverQuantity = rand.Next(1, 9);

                doss.DeliverWeight = rand.Next(1, 500);

                doss.DeliveryYMD = DateTime.Now;

                doss.DestinationZipCode = "3900123";

                doss.DestinationPhoneNumber = "026-333-2222";

                doss.DestinationCode = "";

                doss.DestinationName = "届先名称（株）";

                doss.DestinationAddress = "神奈川県松本市深志区深志神社１－１";

                doss.Note1 = "";

                doss.Note2 = "25   S228037 1 11202";

                doss.Note3 = "300003";

                doss.DataRegistrationDate = DateTime.Now;

                doss.PeriodStart = DateTime.Now;

                doss.PeriodEnd = DateTime.Now;

                list.Add(doss);
            }

            return list;
        }

        // GET: Tass/DossData
        public ActionResult Index()
        {
            var list = CreateSampleData();


            TempData["SampleList"] = list;

            return View(list);
        }

        public ActionResult DetailsForKitaKanto(int? id = null)
        {
            var list = CreateDossForKitaKantoData();

            ViewBag.DataCount = list.Count;

            TempData["DossForKitaKantoList"] = list;

            return View(list);
        }
        public ActionResult DetailsForAll(int? id = null)
        {
            var list = CreateDossForAllData();

            ViewBag.DataCount = list.Count;
            ViewBag.UniqueKey = String.Format("{0}-{1}", list[0].Location, list[0].OutYMD.ToString("yyMMddHHmmss"));
            ViewBag.ProcName = String.Format("松本-{0}-{1}-{2}"
                , list[0].Location, list[0].OutYMD.ToString("MMdd"), list[0].OutYMD.ToString("HHmm"));

            TempData["DossForAllList"] = list;

            return View(list);
        }

        // GET: Tass/DossData/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirm(DummyDossData dossData)
        {
            var str = String.Empty;
            if (dossData != null)
            {
                if (dossData.DossDataAll && dossData.DossDataKitaKanto)
                {
                    str = String.Format("{0}, {1}", Resources.Tass.DossDataForAll, Resources.Tass.DossDataForKitaKanto);
                }
                else
                {
                    if (dossData.DossDataAll)
                    {
                        str = Resources.Tass.DossDataForAll;
                    }
                    if (dossData.DossDataKitaKanto)
                    {
                        str = Resources.Tass.DossDataForKitaKanto;
                    }
                }
            }

            dossData.TargetDossData = str;

            ViewBag.ConfirmMessage = "以下の内容で実行します。";

            return View(dossData);
        }

        public ActionResult CreateResult()
        {
            return View();
        }

        // POST: Tass/DossData/Create
        [HttpPost]
        public ActionResult Create(DummyDossData dossData)
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

        // GET: Tass/DossData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tass/DossData/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tass/DossData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tass/DossData/Delete/5
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
