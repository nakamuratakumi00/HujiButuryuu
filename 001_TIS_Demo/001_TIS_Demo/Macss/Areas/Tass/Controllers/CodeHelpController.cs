using Macss.Areas.Tass.Repositories;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using Macss.Repositories;
using Microsoft.AspNet.Identity.Owin;
using Macss.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.Attributes.ActionFilter;
using Macss.Areas.Tass.Models;

using GrapeCity.ActiveReports.Export.Pdf.Section;
using System.Text;
using Macss.ViewModels;


using static Macss.Areas.Tass.ViewModels.CodeHelpViewModels;

namespace Macss.Areas.Tass.Controllers
{

    public class CodeHelpController : Controller
    {
        private readonly int SearchLimit = 20000;

        ICodeHelpRepositorie codeHelpConRepositorie;
        private IControlRepository controlRepository;
        private TableLockRepository tableLockRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ApplicationDB dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            codeHelpConRepositorie = new CodeHelpRepositorie(dbContext);
            controlRepository = new ControlRepository(dbContext);
            tableLockRepository = new TableLockRepository(dbContext);
        }

        // GET: Tass/CodeHelp
        public ActionResult Index()
        {
            return View();
        }

        //得意先
        [HttpPost]
        public async Task<ActionResult> TokuisakiSearch(string Tornmk, string Torcod, string Fbtcod, string Tornam, string Buknam, string Jysyo)
        {
            TokuisakiSerch viewModel = new TokuisakiSerch
            {
                Tornmk = Tornmk == string.Empty ? null : Tornmk,
                Torcod = Torcod == string.Empty ? null : Torcod,
                Fbtcod = Fbtcod == string.Empty ? null : Fbtcod,
                Tornam = Tornam == string.Empty ? null : Tornam,
                Buknam = Buknam == string.Empty ? null : Buknam,
                Jysyo = Jysyo == string.Empty ? null : Jysyo,
            };

            IEnumerable<TokuisakiInformation> list = await codeHelpConRepositorie.GetTokuisakiAsync(viewModel);

            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：取引先マスタ　得意先コード");
            //Logwrite(sb.ToString());

            return Json(list);

        }

        //仕入先
        [HttpPost]
        public async Task<ActionResult> SiiresakiSearch(string Tornmk, string Torcod, string Fbtcod, string Tornam, string Buknam, string Jysyo)
        {
            SiiresakiSerch viewModel = new SiiresakiSerch
            {
                TornmkSi = Tornmk == string.Empty ? null : Tornmk,
                TorcodSi = Torcod == string.Empty ? null : Torcod,
                FbtcodSi = Fbtcod == string.Empty ? null : Fbtcod,
                TornamSi = Tornam == string.Empty ? null : Tornam,
                BuknamSi = Buknam == string.Empty ? null : Buknam,
                JysyoSi = Jysyo == string.Empty ? null : Jysyo,
            };

            IEnumerable<SiiresakiInformation> list = await codeHelpConRepositorie.GetSiiresakiAsync(viewModel);

            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：取引先マスタ 仕入先コード");
            //Logwrite(sb.ToString());

            return Json(list);

        }

        //品名
        [HttpPost]
        public async Task<ActionResult> HinmeisakiSearch(string Hinnmk, string Hincod, string Hinnam, string Ctlfl1, string Khincd)
        {
            HinmeisakiSerch viewModel = new HinmeisakiSerch
            {
                Hinnmk = Hinnmk == string.Empty ? null : Hinnmk,
                Hincod = Hincod == string.Empty ? null : Hincod,
                Hinnam = Hinnam == string.Empty ? null : Hinnam,
                Ctlfl1 = Ctlfl1 == string.Empty ? null : Ctlfl1,
                Khincd = Khincd == string.Empty ? null : Khincd,

            };

            // レプリケーションチェック
            if (await tableLockRepository.CheckTableLock("m_unsou_hinmei"))
            {
                return Json(new { succsess = 3 });
            }

            IEnumerable<HinmeisakiInformation> list = await codeHelpConRepositorie.GetHinmeiAsync(viewModel);
            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：品名マスタ");
            //Logwrite(sb.ToString());

            if (list == null || list.Count() <= SearchLimit)
            {
                if (list.Count() == 0)
                {
                    return Json(new { succsess = 2 });
                }
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
                //return Json(new { succsess = 1, data = list });
            }
            else
            {
                return Json(new { succsess = 0 });
            }

        }

        //出荷場所
        [HttpPost]
        public async Task<ActionResult> ShukkabashoSearch(string Sybcod, string Sybnam)
        {
            ShukkabashoSerch viewModel = new ShukkabashoSerch
            {
                SybcodCh = Sybcod == string.Empty ? null : Sybcod,
                SybnamCh = Sybnam == string.Empty ? null : Sybnam
            };

            IEnumerable<ShukkabashoInformation> list = await codeHelpConRepositorie.GetShukkabashoAsync(viewModel);
            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：出荷場所マスタ");
            //Logwrite(sb.ToString());

            return Json(list);

        }

        //郵便番号
        [HttpPost]
        public async Task<ActionResult> YubinbangoSerch(string Jyusy1, string Jyusy2, string Yubinn)
        {
            YubinbangoSerch viewModel = new YubinbangoSerch
            {
                Jyusy1 = Jyusy1 == string.Empty ? null : Jyusy1,
                Jyusy2 = Jyusy2 == string.Empty ? null : Jyusy2,
                Yubinn = Yubinn == string.Empty ? null : Yubinn

            };

            IEnumerable<YubinbangoInformation> list = await codeHelpConRepositorie.GetYubinbangoAsync(viewModel);
            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：郵便番号マスタ");
            //Logwrite(sb.ToString());

            if (list == null || list.Count() <= SearchLimit)
            {
                if (list.Count() == 0)
                {
                    return Json(new { succsess = 2 });
                }
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
                //return Json(new { succsess = 1, data = list });
            }
            else
            {
                return Json(new { succsess = 0 });
            }

        }

        //運送区分
        [HttpPost]
        public async Task<ActionResult> UnsokbnSearch(string Unskbn, string Unsnam)
        {
            UnsokbnSerch viewModel = new UnsokbnSerch
            {
                Unskbn = Unskbn == string.Empty ? null : Unskbn,
                Unsknam = Unsnam == string.Empty ? null : Unsnam
            };

            IEnumerable<UnsokbnInformation> list = await codeHelpConRepositorie.GetUnsokbnAsync(viewModel);
            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：運送区分マスタ");
            //Logwrite(sb.ToString());

            return Json(list);

        }

        //運送方法
        [HttpPost]
        public async Task<ActionResult> UnsohohoSearch(string Unscod, string Unsnam)
        {
            UnsohohoSerch viewModel = new UnsohohoSerch
            {
                Unscod = Unscod == string.Empty ? null : Unscod,
                Unsnam = Unsnam == string.Empty ? null : Unsnam
            };

            IEnumerable<UnsohohoInformation> list = await codeHelpConRepositorie.GetUnsohohoAsync(viewModel);
            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：運送方法マスタ");
            //Logwrite(sb.ToString());

            return Json(list);

        }

        //届先
        [HttpPost]
        public async Task<ActionResult> TodokesakiSearch(string Tdknmk, string Tdkcod, string Tdknam,string Tdknms, string Tdbnam, string Jyusyo, string Ktdkcd, string Tornam,
                                                                                   string Sdek01, string Sdek02, string Sdek03, string Sdek04, string Sdek05, string Sdek06, string Sdek07, string Sdek08,
                                                                                    string Sdek09, string Sdek10, string Sdek11, string Sdek12, string Sdek13, string Sdek14, string Sdek15)           
        {
            TodokesakihelpSerch viewModel = new TodokesakihelpSerch
            {
                Tdknmk = Tdknmk == string.Empty ? null : Tdknmk,
                Tdkcod = Tdkcod == string.Empty ? null : Tdkcod,
                Tdknam = Tdknam == string.Empty ? null : Tdknam,
                Tdknms = Tdknms == string.Empty ? null : Tdknms,
                Tdbnam = Tdbnam == string.Empty ? null : Tdbnam,
                Jyusyo = Jyusyo == string.Empty ? null : Jyusyo,
                Ktdkcd = Ktdkcd == string.Empty ? null : Ktdkcd,
                Tornam = Tornam == string.Empty ? null : Tornam,
                Sdek01 = Sdek01 == string.Empty ? null : Sdek01,
                Sdek02 = Sdek02 == string.Empty ? null : Sdek02,
                Sdek03 = Sdek03 == string.Empty ? null : Sdek03,
                Sdek04 = Sdek04 == string.Empty ? null : Sdek04,
                Sdek05 = Sdek05 == string.Empty ? null : Sdek05,
                Sdek06 = Sdek06 == string.Empty ? null : Sdek06,
                Sdek07 = Sdek07 == string.Empty ? null : Sdek07,
                Sdek08 = Sdek08 == string.Empty ? null : Sdek08,
                Sdek09 = Sdek09 == string.Empty ? null : Sdek09,
                Sdek10 = Sdek10 == string.Empty ? null : Sdek10,
                Sdek11 = Sdek11 == string.Empty ? null : Sdek11,
                Sdek12 = Sdek12 == string.Empty ? null : Sdek12,
                Sdek13 = Sdek13 == string.Empty ? null : Sdek13,
                Sdek14 = Sdek14 == string.Empty ? null : Sdek14,
                Sdek15 = Sdek15 == string.Empty ? null : Sdek15
            };

            // レプリケーションチェック
            if (await tableLockRepository.CheckTableLock("m_unsou_todokesaki"))
            {
                return Json(new
                {
                    succsess = 3
                });
            }

            IEnumerable<TodokesakihelpInformation> list = await codeHelpConRepositorie.GetTodokesakiAsync(viewModel);
            ////ログ出力
            //StringBuilder sb = new StringBuilder();
            //sb.Append("出力：届先マスタ");
            //Logwrite(sb.ToString());

            if (list == null || list.Count() <= SearchLimit)
            {
                if (list.Count() == 0)
                {
                    return Json(new { succsess = 2 });
                }
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
                //return Json(new { succsess = 1, data = list });
            }
            else
            {
                return Json(new { succsess = 0 });
            }

        }


        //届先
        [HttpPost]
        public async Task<ActionResult> PatternSearch(string SyuknoMae)
        {

            if (string.IsNullOrEmpty(SyuknoMae))
            {
                return Json(new { succsess = 2 });
            }

            IEnumerable<TyuumonshoPatternInformation> list = await codeHelpConRepositorie.GetPatternAsync(SyuknoMae);

            if (list != null && list.Count() <= SearchLimit)
            {
                if (list.Count() == 0)
                {
                    return Json(new { succsess = 2 });
                }
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                return Json(new { succsess = 0 });
            }

        }



        ////ログ出力
        //private void Logwrite(string message)
        //{

        //    var loginUser = Session.GetUserID();
        //    var prossesingId = Session.GetProcessingID();
        //    var sessionMenu = Session[SessionExtensions.Field.Menu] as List<MenuViewModels>;
        //    MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
        //    string name = "処理機能：" + menu.TitleName;


        //}
    }

}