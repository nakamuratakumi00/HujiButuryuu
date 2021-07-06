using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Macss.App_Start;
using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Repositories;
using Macss.Controllers;
using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.Repositories;
using Macss.Areas.Tass.ViewModels;
using System.Text;
using Macss.Areas.Tass.Common;
using System.Text.RegularExpressions;


namespace Macss.Areas.Tass.Controllers
{
    public class ShuukaTyuumonshoEntryController : Controller
    {
        #region 定数
        public const string StrInquiry = "1";       // 1:照会
        public const string StrEdit = "2";          // 2:更新
        public const string StrDelete = "3";        // 3:削除
        #endregion

        private ApplicationUserManager userManager;
        private IControlRepository controlRepository;
        private ILogRepository logRepository;
        private LogService logService;
        private MShukkabashoRepository mShukkabashoRepository;
        private IShuukaTyuumonshoEntryRepositorie entryRepositorie;
        private IAccountRepository accountRepository;
        private TableLockRepository tableLockRepository;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            controlRepository = new ControlRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();
            mShukkabashoRepository = new MShukkabashoRepository(dbContext);
            entryRepositorie = new ShuukaTyuumonshoEntryRepositorie(dbContext);
            accountRepository = new AccountRepository(dbContext);
            tableLockRepository = new TableLockRepository(dbContext);
        }

        // 一覧 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            string Referer = HttpContext.Request.ServerVariables["HTTP_REFERER"];
            ShuukaTyuumonshoEntryData entryData = new ShuukaTyuumonshoEntryData();
            object model = Session.GetViewModel();
            if (model != null && (typeof(EntrySerch) == model.GetType() && (Referer != null && -1 != Referer.IndexOf("/ShuukaTyuumonshoEntry"))))
            {
                EntrySerch serch = (EntrySerch)model;
                entryData.Serch = serch;
                Session.RemoveViewModel();
            }
            else
            {
                Session.RemoveViewModel();

                // ログインユーザに紐づく、照会グループ取得
                var loginUser = Session.GetUserID();
                var user = await userManager.FindByIdAsync(loginUser);

                EntrySerch serch = new EntrySerch
                {
                    SykymdFrom = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)).ToString("yyyy/MM/dd"),
                    Denfn = true,
                    Denfy = false,
                    GroupCd = user.GroupCd
                };
                entryData.Serch = serch;
            }

            return View(entryData);
        }

        // 一覧 明細取得
        [HttpPost]
        public async Task<ActionResult> Search(string Syukno, string Sybcod, 
                                               string Fsykno, string Crtnam, 
                                               string SykymdFrom, string SykymdTo, 
                                               string Tokcod, string Toknam, 
                                               string Tdkcod, string Tdknam, string Tdsnam,
                                               string Tdbnam, string Tdktan,
                                               bool Denfn, bool Denfy,
                                               string GroupCd)
        {
            if (!Denfn && !Denfy)
            {
                return Json(new { succsess = 0 });
            }

            EntrySerch viewModel = new EntrySerch
            {
                Syukno = Syukno == string.Empty ? null : Syukno,
                Sybcod = Sybcod == string.Empty ? null : Sybcod,
                Fsykno = Fsykno == string.Empty ? null : Fsykno,
                Crtnam = Crtnam == string.Empty ? null : Crtnam,
                SykymdFrom = SykymdFrom == string.Empty ? null : SykymdFrom,
                SykymdTo = SykymdTo == string.Empty ? null : SykymdTo,
                Tokcod = Tokcod == string.Empty ? null : Tokcod,
                Toknam = Toknam == string.Empty ? null : Toknam,
                Tdkcod = Tdkcod == string.Empty ? null : Tdkcod,
                Tdknam = Tdknam == string.Empty ? null : Tdknam,
                Tdsnam = Tdsnam == string.Empty ? null : Tdsnam,
                Tdbnam = Tdbnam == string.Empty ? null : Tdbnam,
                Tdktan = Tdktan == string.Empty ? null : Tdktan,
                Denfn = Denfn,
                Denfy = Denfy,
                GroupCd = GroupCd,
                SearchFlg = 1
            };
            Session.SetViewModel(viewModel);

            var list = await entryRepositorie.GetShuukaTyuumonshosAsync(viewModel);

            //TODO KANAI 検索結果0件の場合の返答
            if (list != null & list.Count() > 0)
            {
                var json = Json(new { succsess = 1, data = list });
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            else
            {
                return Json(new { succsess = 0 });
            }

            //return Json(list);
        }

        // 一覧 操作ボタン押下
        [HttpPost]
        [AuthorityActionFilter]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ShuukaTyuumonshoEntryData entryData)
        {
            string proc = entryData.Mode;

            var no = entryData.Information.Syukno.Replace("-", "");
            var viewModel = (await entryRepositorie.GetShuukaTyuumonshoAsync(no, entryData.Information.Cdate)).FirstOrDefault();

            // エラーチェック
            var errors = new List<(string field, string message)>();
            if (proc == StrInquiry)
            {
                // 照会
                // 存在チェック
                if (viewModel == null)
                {
                    errors.Add((string.Empty, "照会対象データは存在しません。"));
                }
            }
            if (proc == StrEdit)
            {
                // 更新
                // 存在チェック
                if (viewModel == null)
                {
                    errors.Add((string.Empty, Resources.Message.CE059)); //更新対象データは存在しません。
                }
                // 注文書発行済かつ出荷日が30日前未満 チェック
                if (viewModel != null && !string.IsNullOrEmpty(viewModel.Denf) && (viewModel.Denf == "A" | viewModel.Denf == "S") && DateTime.Parse(viewModel.Sykymd) < DateTime.Now.Date.AddDays(-30))
                {
                    errors.Add((string.Empty, "発行済かつ出荷日が現在日より31日以上前の為、更新できません。"));
                }
            }
            if (proc == StrDelete)
            {
                // 削除
                // 存在チェック
                if (viewModel == null)
                {
                    errors.Add((string.Empty, Resources.Message.CE065)); //削除対象データは存在しません。
                }
                // 注文書発行済 チェック
                if (viewModel != null && !string.IsNullOrEmpty(viewModel.Denf) && (viewModel.Denf == "A" | viewModel.Denf == "S"))
                {
                    errors.Add((string.Empty, "発行済の為、削除できません。"));
                }
            }

            // エラー表示
            if (errors.Any())
            {
                foreach (var (field, message) in errors)
                {
                    ModelState.AddModelError(String.Empty, message);
                }

                return View(entryData);
            }

            // 画面遷移
            switch (proc)
            {
                case StrInquiry:
                    return this.Redirect("ShuukaTyuumonshoEntry/Inquiry?No=" + entryData.Information.Syukno + "&Dt=" + entryData.Information.Cdate + "&Proc=1");
                case StrEdit:
                    return this.Redirect("ShuukaTyuumonshoEntry/Edit?No=" + entryData.Information.Syukno + "&Dt=" + entryData.Information.Cdate + "&Proc=2");
                case StrDelete:
                    return this.Redirect("ShuukaTyuumonshoEntry/Delete?No=" + entryData.Information.Syukno + "&Dt=" + entryData.Information.Cdate + "&Proc=3");
                default:
                    return View();
            }
        }

        // 新規 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Details(string no, string dt, string proc)
        {
            ViewBag.Mode = proc;

            object model = Session.GetViewModel();
            if (model != null && (typeof(EntrySerch) == model.GetType()))
            {
                EntrySerch serch = (EntrySerch)model;
                Session.RemoveViewModel();
            }

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.UfutanList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.UFutanExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });
            ViewBag.YusonoList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TokuteiShiteiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn, Value = x.Kbn });
            ViewBag.UnscrsList = (await entryRepositorie.GetMUnsoukousuAsync()).Select(x => new SelectListItem() { Text = x.Unscrs + ":" + x.Crsnam, Value = x.Unscrs });
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            // 操作者に紐付く部門コード、部門名、出荷場所コードを初期表示
            ShuukaTyuumonshoEntryViewModel viewModel = new ShuukaTyuumonshoEntryViewModel();
            var loginUser = Session.GetUserID();
            var user = await userManager.FindByIdAsync(loginUser);
            viewModel.Pccod1 = user.BumonCd;
            viewModel.Sybcod = user.BasyoCd;
            var bumon = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountBumon)).Where(x => x.Kbn == user.BumonCd).FirstOrDefault();
            viewModel.Pccod1n = bumon == null ? string.Empty : bumon.Value1;
            var shukkabasho = (await mShukkabashoRepository.GetAllShukkabashosAsync()).Where(x => x.Sybcod == user.BasyoCd).FirstOrDefault();
            viewModel.Sybnam = shukkabasho == null ? string.Empty : shukkabasho.Sybnam;
            ViewBag.Meisai = await entryRepositorie.GetShuukaTyuumonshoMeisaiAsync(string.Empty, string.Empty);

            viewModel.Mode = proc;
            return View(viewModel);
        }

        // 照会 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Inquiry(string no, string dt, string proc)
        {
            ViewBag.Mode = proc;

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.UfutanList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.UFutanExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });
            ViewBag.YusonoList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TokuteiShiteiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn, Value = x.Kbn });
            ViewBag.UnscrsList = (await entryRepositorie.GetMUnsoukousuAsync()).Select(x => new SelectListItem() { Text = x.Unscrs + ":" + x.Crsnam, Value = x.Unscrs });
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            // 表示
            ShuukaTyuumonshoEntryViewModel viewModel = new ShuukaTyuumonshoEntryViewModel();
            no = no.Replace("-", "");
            viewModel = (await entryRepositorie.GetShuukaTyuumonshoAsync(no, dt)).FirstOrDefault();
            ViewBag.Meisai = await entryRepositorie.GetShuukaTyuumonshoMeisaiAsync(no, dt);

            // 特別指定コードは空白
            //viewModel.Yusono = string.Empty;

            // ログ履歴作成
            var loginUser = Session.GetUserID();
            logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.S1, "処理機能：" + menu.TitleName, string.Format("照会キー：{0}, {1}", no, dt), logRepository);

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 更新 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Edit(string no, string dt, string proc)
        {
            ViewBag.Mode = proc;

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.UfutanList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.UFutanExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });
            ViewBag.YusonoList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TokuteiShiteiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn, Value = x.Kbn });
            ViewBag.UnscrsList = (await entryRepositorie.GetMUnsoukousuAsync()).Select(x => new SelectListItem() { Text = x.Unscrs + ":" + x.Crsnam, Value = x.Unscrs });
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            // 表示
            ShuukaTyuumonshoEntryViewModel viewModel = new ShuukaTyuumonshoEntryViewModel();
            no = no.Replace("-", "");
            viewModel = (await entryRepositorie.GetShuukaTyuumonshoAsync(no, dt)).FirstOrDefault();
            ViewBag.Meisai = await entryRepositorie.GetShuukaTyuumonshoMeisaiAsync(no, dt);

            // 特別指定コードは空白
            //viewModel.Yusono = string.Empty;

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 削除 表示
        [AuthorityActionFilter]
        public async Task<ActionResult> Delete(string no, string dt, string proc)
        {
            ViewBag.Mode = proc;

            // タイトルセット
            var prossesingId = Session.GetProcessingID();
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.MenuId == prossesingId).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.UfutanList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.UFutanExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });
            ViewBag.YusonoList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TokuteiShiteiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn, Value = x.Kbn });
            ViewBag.UnscrsList = (await entryRepositorie.GetMUnsoukousuAsync()).Select(x => new SelectListItem() { Text = x.Unscrs + ":" + x.Crsnam, Value = x.Unscrs });
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            // 表示
            ShuukaTyuumonshoEntryViewModel viewModel = new ShuukaTyuumonshoEntryViewModel();
            no = no.Replace("-", "");
            viewModel = (await entryRepositorie.GetShuukaTyuumonshoAsync(no, dt)).FirstOrDefault();
            ViewBag.Meisai = await entryRepositorie.GetShuukaTyuumonshoMeisaiAsync(no, dt);

            // 特別指定コードは空白
            //viewModel.Yusono = string.Empty;

            viewModel.Mode = proc;
            return View("Details", viewModel);
        }

        // 採番
        [HttpPost]
        public ActionResult GetSyuknoAto(string param)
        {
            // 未入力/3桁以外の場合、採番しない
            if (param.Length != 3)
            {
                return Json(new { succsess = 0, string.Empty });
            }

            // 半角入力が含まれている場合、採番しない
            if (!Regex.IsMatch(param, @"^[A-Z0-9]+$"))
            {
                return Json(new { succsess = 0, string.Empty });
            }

            // 採番
            string SyuknoAto = string.Empty;
            var max_no = entryRepositorie.GetSyknoAto(param);
            SyuknoAto = max_no.ToString("00000");

            return Json(new { succsess = SyuknoAto != string.Empty ? 1 : 0, SyuknoAto });
        }

        // パターン フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetShuukaTyuumonshoPattern(string param1, string param2)
        {
            if (param2.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    Htynam = string.Empty,
                    Htykah = string.Empty,
                    Tancod = string.Empty,
                    Tannam = string.Empty,
                    Htytel = string.Empty,
                    Basyo = string.Empty,
                    Tokcod = string.Empty,
                    Toknam = string.Empty,
                    Sybcod = string.Empty,
                    Sybnam = string.Empty,
                    Ufutan = string.Empty,
                    Keifno = string.Empty,
                    Tdkcod = string.Empty,
                    Tdkyub = string.Empty,
                    Tdkjyu = string.Empty,
                    Tdknam = string.Empty,
                    Tdsnam = string.Empty,
                    Tdbnam = string.Empty,
                    Tdktan = string.Empty,
                    Tdktel = string.Empty,
                    Hincd1 = string.Empty,
                    Hincd2 = string.Empty,
                    Hincd3 = string.Empty,
                    Hincd4 = string.Empty,
                    Hincd5 = string.Empty,
                    Hinnm1 = string.Empty,
                    Hinnm2 = string.Empty,
                    Hinnm3 = string.Empty,
                    Hinnm4 = string.Empty,
                    Hinnm5 = string.Empty,
                    Syksu1 = string.Empty,
                    Syksu2 = string.Empty,
                    Syksu3 = string.Empty,
                    Syksu4 = string.Empty,
                    Syksu5 = string.Empty,
                    Tkjiko1 = string.Empty,
                    Tkjiko2 = string.Empty,
                    Unscod = string.Empty,
                    Unscodnam = string.Empty,
                    Unscrs = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetShuukaTyuumonshoPattern(param1, param2)).FirstOrDefault();

                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    Htynam = list == null ? string.Empty : list.Htynam ?? string.Empty,
                    Htykah = list == null ? string.Empty : list.Htykah ?? string.Empty,
                    Tancod = list == null ? string.Empty : list.Tancod ?? string.Empty,
                    Tannam = list == null ? string.Empty : list.Tannam ?? string.Empty,
                    Htytel = list == null ? string.Empty : list.Htytel ?? string.Empty,
                    Basyo = list == null ? string.Empty : list.Basyo ?? string.Empty,
                    Tokcod = list == null ? string.Empty : list.Tokcod ?? string.Empty,
                    Toknam = list == null ? string.Empty : list.Toknam ?? string.Empty,
                    Sybcod = list == null ? string.Empty : list.Sybcod ?? string.Empty,
                    Sybnam = list == null ? string.Empty : list.Sybnam ?? string.Empty,
                    Ufutan = list == null ? string.Empty : list.Ufutan ?? string.Empty,
                    Keifno = list == null ? string.Empty : list.Keifno ?? string.Empty,
                    Tdkcod = list == null ? string.Empty : list.Tdkcod ?? string.Empty,
                    Tdkyub = list == null ? string.Empty : list.Tdkyub ?? string.Empty,
                    Tdkjyu = list == null ? string.Empty : list.Tdkjyu ?? string.Empty,
                    Tdknam = list == null ? string.Empty : list.Tdknam ?? string.Empty,
                    Tdsnam = list == null ? string.Empty : list.Tdsnam ?? string.Empty,
                    Tdbnam = list == null ? string.Empty : list.Tdbnam ?? string.Empty,
                    Tdktan = list == null ? string.Empty : list.Tdktan ?? string.Empty,
                    Tdktel = list == null ? string.Empty : list.Tdktel ?? string.Empty,
                    Hincd1 = list == null ? string.Empty : list.Hincd1 ?? string.Empty,
                    Hincd2 = list == null ? string.Empty : list.Hincd2 ?? string.Empty,
                    Hincd3 = list == null ? string.Empty : list.Hincd3 ?? string.Empty,
                    Hincd4 = list == null ? string.Empty : list.Hincd4 ?? string.Empty,
                    Hincd5 = list == null ? string.Empty : list.Hincd5 ?? string.Empty,
                    Hinnm1 = list == null ? string.Empty : list.Hinnm1 ?? string.Empty,
                    Hinnm2 = list == null ? string.Empty : list.Hinnm2 ?? string.Empty,
                    Hinnm3 = list == null ? string.Empty : list.Hinnm3 ?? string.Empty,
                    Hinnm4 = list == null ? string.Empty : list.Hinnm4 ?? string.Empty,
                    Hinnm5 = list == null ? string.Empty : list.Hinnm5 ?? string.Empty,
                    Syksu1 = list == null ? string.Empty : list.Hinnm1 == null || list.Hinnm1 == string.Empty ? string.Empty : list.Syksu1 == null ? string.Empty : list.Syksu1.ToString(),
                    Syksu2 = list == null ? string.Empty : list.Hinnm2 == null || list.Hinnm2 == string.Empty ? string.Empty : list.Syksu2 == null ? string.Empty : list.Syksu2.ToString(),
                    Syksu3 = list == null ? string.Empty : list.Hinnm3 == null || list.Hinnm3 == string.Empty ? string.Empty : list.Syksu3 == null ? string.Empty : list.Syksu3.ToString(),
                    Syksu4 = list == null ? string.Empty : list.Hinnm4 == null || list.Hinnm4 == string.Empty ? string.Empty : list.Syksu4 == null ? string.Empty : list.Syksu4.ToString(),
                    Syksu5 = list == null ? string.Empty : list.Hinnm5 == null || list.Hinnm5 == string.Empty ? string.Empty : list.Syksu5 == null ? string.Empty : list.Syksu5.ToString(),
                    Tkjiko1 = list == null ? string.Empty : list.Tkjiko1 ?? string.Empty,
                    Tkjiko2 = list == null ? string.Empty : list.Tkjiko2 ?? string.Empty,
                    Unscod = list == null ? string.Empty : list.Unscod ?? string.Empty,
                    Unscodnam = list == null ? string.Empty : list.Unscodnam ?? string.Empty,
                    Unscrs = list == null ? string.Empty : list.Unscrs ?? string.Empty
                });
            }
        }

        // 部門コード フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetBumonName(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    BumonName = string.Empty
                });
            }
            else
            {
                var list = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountBumon)).Where(x => x.Kbn == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    BumonName = list == null ? string.Empty : list.Value1 ?? string.Empty
                });
            }
        }

        // 担当者コード フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetTantoName(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    TantoName = string.Empty
                });
            }
            else
            {
                var list = (await accountRepository.FindByCodeAsync(param)).FirstOrDefault();
                if (list != null && list.DeleteFlg == 1)
                {
                    list = null;
                }
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    TantoName = list == null ? string.Empty : list.UserName ?? string.Empty
                });
            }
        }

        // 担当者名　整合性チェック
        [HttpPost]
        public async Task<ActionResult> GetMatchTannam(string cd, string name)
        {
            if (name.Length == 0 | cd.Length == 0)
            {
                return Json(new
                {
                    succsess = 0
            });
            }
            else
            {
                var list = (await accountRepository.FindByCodeAsync(cd)).FirstOrDefault();                
                return Json(new
                {
                    succsess = list == null ? 1 : list.UserName != name ? 1 : 0,
                });
            }
        }

        // 得意先・仕入先　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetTorihikisakiName(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    TorihikisakiName = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == param).FirstOrDefault();
                return Json(new { succsess = list == null ? 0 : 1, TorihikisakiName = list == null ? string.Empty : list.Tornam });
            }
        }

        // 出荷場所コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetSyukabashoName(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    SyukabashoName = string.Empty
                });
            }
            else
            {
                var list = (await mShukkabashoRepository.GetAllShukkabashosAsync()).Where(x => x.Sybcod == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    SyukabashoName = list == null ? string.Empty : list.Sybnam ?? string.Empty
                });
            }
        }

        // 届先コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetTodokesaki(string param)
        {

            // レプリケーションチェック
            if (await tableLockRepository.CheckTableLock("m_unsou_todokesaki"))
            {
                return Json(new
                {
                    succsess = 2
                });
            }

            if (param.Length == 0)
            {
                return Json(new {
                    succsess = 0,
                    Tdkjyu = string.Empty,
                    Tdknam = string.Empty,
                    Tdsnam = string.Empty,
                    Tdbnam = string.Empty,
                    Tdktan = string.Empty,
                    Tdktel = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetVTodokesakiAsync()).Where(x => x.Tdkcod == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    Tdkyub = list == null ? string.Empty : list.Tdkyub ?? string.Empty,
                    Tdkjyu = list == null ? string.Empty : list.Tdkjyu ?? string.Empty,
                    Tdknam = list == null ? string.Empty : list.Tdknam ?? string.Empty,
                    Tdsnam = list == null ? string.Empty : list.Tdsnam ?? string.Empty,
                    Tdbnam = list == null ? string.Empty : list.Tdbnam ?? string.Empty,
                    Tdktan = list == null ? string.Empty : list.Tdktan ?? string.Empty,
                    Tdktel = list == null ? string.Empty : list.Tdktel ?? string.Empty
                });
            }
        }

        // 届先郵便番号コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetJyusyo(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    Jyusyo = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Yubinn == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    Jyusyo = list == null ? string.Empty : ((list.Jyusy1 ?? string.Empty) + (list.Jyusy2 ?? string.Empty)).Replace("　", "").Replace(" ", "")
                });
            }
        }

        // 品名コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetHinmei(string param)
        {

            // レプリケーションチェック
            if (await tableLockRepository.CheckTableLock("m_unsou_hinmei"))
            {
                return Json(new
                {
                    succsess = 2,
                    Hinnam = string.Empty
                });
            }

            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    Hinnam = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetHinmeiAsync()).Where(x => x.Hincod == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    Hinnam = list == null ? string.Empty : list.Hinnam.Trim() ?? string.Empty
                });
            }
        }

        // 特別指定コード　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetTkjiko(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    addTkjiko2 = string.Empty,
                });
            }
            else
            {
                var list = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TokuteiShiteiExtraction)).Where(x => x.Kbn == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    addTkjiko2 = list == null ? string.Empty : list.Value1 ?? string.Empty
                });
            }
        }

        // 運送方法　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetUnsonhouhouName(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    Unsnam = string.Empty,
                    Torcod = string.Empty,
                    Unskbn = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetMUnsouUnsouhouhouAsync()).Where(x => x.Unscod == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    Unsnam = list == null ? string.Empty : list.Unsnam ?? string.Empty,
                    Torcod = list == null ? string.Empty : list.Torcod ?? string.Empty,
                    Unskbn = list == null ? string.Empty : list.Unskbn ?? string.Empty
                });
            }
        }

        // 運送区分　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetUnsonkubunName(string param)
        {
            if (param.Length == 0)
            {
                return Json(new
                {
                    succsess = 0,
                    UnsonkubunName = string.Empty
                });
            }
            else
            {
                var list = (await entryRepositorie.GetMUnsouUnsoukubunAsync()).Where(x => x.Unskbn == param).FirstOrDefault();
                return Json(new
                {
                    succsess = list == null ? 0 : 1,
                    UnsonkubunName = list == null ? string.Empty : list.Unsnam ?? string.Empty
                });
            }
        }


        // 住所　フォーカスアウト(変更)時
        [HttpPost]
        public async Task<ActionResult> GetYubinn(string param1, string param2, bool param3)
        {
            if (param1.Length == 0)
            {
                return Json(new
                {
                    succsess = 1,
                    Yubinn = param2
                });
            }
            else
            {
                var Tdkjyu = param1.Replace("　", "").Replace(" ", "");
                //var spIdx = Common.DataUtil.GetNumIndex(Tdkjyu);
                //var jyusy12 = Tdkjyu.Substring(0, spIdx);
                //var jyusyo = (await entryRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Jyusy1 + x.Jyusy2 == jyusy12).FirstOrDefault();
                var jyusyo = await CheckJyusyAsync(Tdkjyu, param2);
                if (param3)
                {
                    // 郵便番号入力済 - 郵便番号を必須としない:On  <= チェックなし
                    if (jyusyo != null && (param2 == ""))
                    {
                        return Json(new
                        {
                            succsess = 1,
                            Yubinn = jyusyo.Yubinn
                        });
                    }

                    return Json(new
                    {
                        succsess = 1,
                        Yubinn = param2
                    });
                }
                else
                {
                    // 郵便番号入力済 - 郵便番号を必須としない:Off <= 郵便番号、住所整合性チェック
                    if (jyusyo == null || (param2 != "" && param2 != jyusyo.Yubinn))
                    {
                        // エラー
                        return Json(new
                        {
                            succsess = 0,
                            Yubinn = param2
                        });
                    } else
                    {
                        return Json(new
                        {
                            succsess = 1,
                            Yubinn = jyusyo.Yubinn
                        });
                    }
                }
            }
        }


        // 新規・修正・削除
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Details(ShuukaTyuumonshoEntryViewModel entry)
        {
            string mode = entry.Mode;
            string loginUser = Session.GetUserID();

            // タイトルセット
            var sessionMenu = Session[SessionExtensions.Field.Menu] as List<Macss.ViewModels.MenuViewModels>;
            string actionName = "";
            switch (mode)
            {
                case StrEdit:
                    actionName = "Edit";
                    break;
                case StrDelete:
                    actionName = "Delete";
                    break;
                default:
                    actionName = "Details";
                    break;
            }
            Macss.ViewModels.MenuViewModels menu = sessionMenu.Where(x => x.ControllerName == "Tass/ShuukaTyuumonshoEntry").Where(x => x.ActionName == actionName).FirstOrDefault();
            ViewBag.TitleName = menu.TitleName;

            // プルダウンリスト作成
            ViewBag.UfutanList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.UFutanExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });
            ViewBag.YusonoList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TokuteiShiteiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn, Value = x.Kbn });
            ViewBag.UnscrsList = (await entryRepositorie.GetMUnsoukousuAsync()).Select(x => new SelectListItem() { Text = x.Unscrs + ":" + x.Crsnam, Value = x.Unscrs });
            ViewBag.SCtlfl1 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.HinmeiExtraction)).Select(x => new SelectListItem() { Text = x.Kbn + ":" + x.Value1, Value = x.Kbn });

            // 明細データ取得
            var meisaiData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShuukaTyuumonshoEntryMeisaiViewModel>>(entry.MeisaiJson);
            ViewBag.Meisai = meisaiData;

            try
            {
                // エラーチェック
                var errors = new List<(string field, string message)>();

                if (mode == null)
                {
                    // 新規
                    // 未採番
                    if (entry.NumberingFlg != 1)
                    {
                        //errors.Add((String.Empty, "採番ボタンを押してください。"));
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "採番", "採番ボタンを押してください"))); //{0}：　{1}。

                    }
                }
                if (mode == null | mode == StrEdit)
                {
                    // 新規・更新
                    // 部門コード 存在チェック
                    var bumon = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.AccountBumon)).Where(x => x.Kbn == entry.Pccod1);
                    if (bumon.Count() == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE067, "部門コード", "部門コード"))); //{0}：　マスタに存在しない{1}が設定されています。
                    }
                    // 担当者コード 存在チェック
                    if (!string.IsNullOrEmpty(entry.Tancod))
                    {
                        var Tantousya = (await accountRepository.FindByCodeAsync(entry.Tancod));
                        if (Tantousya.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "担当者コード", "担当者コード"))); //{0}：　マスタに存在しない{1}が設定されています。
                        }
                        else
                        {
                            var Tan = Tantousya.First();
                            if (Tan.DeleteFlg == 1)
                            {
                                errors.Add((string.Empty, string.Format(Resources.Message.CE067, "担当者コード", "担当者コード"))); //{0}：　マスタに存在しない{1}が設定されています。
                            }
                        }
                    }
                    // 得意先コード 存在チェック
                    var tokuisaki = (await entryRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == entry.Tokcod);
                    if (tokuisaki.Count() == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE067, "得意先コード", "得意先コード")));//{0}：　マスタに存在しない{1}が設定されています。
                    }
                    // 出荷場所コード 存在チェック
                    var shukkabasyo = (await mShukkabashoRepository.GetAllShukkabashosAsync()).Where(x => x.Sybcod == entry.Sybcod);
                    if (shukkabasyo.Count() == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE067, "出荷場所コード", "出荷場所コード"))); //{0}：　マスタに存在しない{1}が設定されています。
                    }
                    // 経費負担No 条件付き必須チェック
                    if (string.IsNullOrEmpty(entry.Keifno))
                    {
                        if (entry.Tokcod.StartsWith("357") | entry.Tokcod.StartsWith("379"))
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "得意先コード・経費負担No", "得意先コード、経費負担Noの組み合わせに誤りがあります"))); //{0}：　{1}。
                            //errors.Add((string.Empty, string.Format(Resources.Message.CE066, "経費負担No"))); //{0}：　入力してください。
                        }
                    }
                    // 経費負担No 整合性チェック
                    if (!string.IsNullOrEmpty(entry.Keifno) && entry.Keifno.StartsWith("MS")) {
                        if (entry.Tokcod.StartsWith("1") | entry.Tokcod.StartsWith("2") | entry.Tokcod.StartsWith("3"))
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "得意先コード・経費負担No", "得意先コード、経費負担Noの組み合わせに誤りがあります"))); //{0}：　{1}。
                            //errors.Add((string.Empty, string.Format(Resources.Message.CE070, "経費負担No"))); //{0}：　誤りがあります。
                        }
                    }

                    // 届先コード 存在チェック
                    if (!string.IsNullOrEmpty(entry.Tdkcod))
                    {

                        // レプリケーションチェック
                        if (await tableLockRepository.CheckTableLock("m_unsou_todokesaki"))
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先コード", "現在届先マスタはレプリケーション中です。　登録・更新する場合は、届先コードを削除してください"))); //{0}：　{1}。
                        }
                        else
                        {

                            var todokesaki = (await entryRepositorie.GetVTodokesakiAsync()).Where(x => x.Tdkcod == entry.Tdkcod);
                            if (todokesaki.Count() == 0)
                            {
                                errors.Add((string.Empty, string.Format(Resources.Message.CE067, "届先コード", "届先コード")));  //{0}：　マスタに存在しない{1}が設定されています。
                            }
                            else
                            {
                                var todoke = todokesaki.First();
                                if ((todoke.Tdkjyu == null ? string.Empty : todoke.Tdkjyu) != (entry.Tdkjyu == null ? string.Empty : entry.Tdkjyu))
                                {
                                    errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先住所", "届先マスタの情報と異なります"))); //{0}：　{1}。
                                }
                                if ((todoke.Tdknam == null ? string.Empty : todoke.Tdknam) != (entry.Tdknam == null ? string.Empty : entry.Tdknam))
                                {
                                    errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先社名", "届先マスタの情報と異なります"))); //{0}：　{1}。
                                }
                                if ((todoke.Tdsnam == null ? string.Empty : todoke.Tdsnam) != (entry.Tdsnam == null ? string.Empty : entry.Tdsnam))
                                {
                                    errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先支店名", "届先マスタの情報と異なります"))); //{0}：　{1}。
                                }
                                if ((todoke.Tdkyub == null ? string.Empty : todoke.Tdkyub) != (entry.Tdkyub == null ? string.Empty : entry.Tdkyub))
                                {
                                    errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先郵便番号", "届先マスタの情報と異なります"))); //{0}：　{1}。
                                }
                                if ((todoke.Tdktel == null ? string.Empty : todoke.Tdktel) != (entry.Tdktel == null ? string.Empty : entry.Tdktel))
                                {
                                    errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先電話番号", "届先マスタの情報と異なります"))); //{0}：　{1}。
                                }
                            }
                        }
                    }

                    // 得意先コード、経費負担No組み合わせチェック
                    if (!DataUtil.CheckTokuisakiKeifno(entry.Tokcod, entry.Keifno))
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "得意先コード・経費負担No", "得意先コード、経費負担Noの組み合わせに誤りがあります"))); //{0}：　{1}。
                    }

                    // 郵便番号、住所チェック
                    var Tdkjyu = entry.Tdkjyu.Replace("　", "").Replace(" ", "");
                    //var spIdx = Common.DataUtil.GetNumIndex(Tdkjyu);
                    //var jyusy12 = Tdkjyu.Substring(0, spIdx);
                    //var jyusyo = (await entryRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Jyusy1 + x.Jyusy2 == jyusy12).FirstOrDefault();
                    var jyusyo = await CheckJyusyAsync(Tdkjyu, entry.Tdkyub);

                    if (string.IsNullOrEmpty(entry.Tdkyub))
                    {
                        if (jyusyo == null)
                        {
                            if (entry.Yubflg)
                            {
                                // 郵便番号未入力 - 住所なし - 郵便番号を必須としない:On  <= チェックなし
                            }
                            else
                            {
                                // 郵便番号未入力 - 住所なし - 郵便番号を必須としない:Off <= 必須チェック
                                errors.Add((string.Empty, string.Format(Resources.Message.CE066, "届先郵便番号"))); //{0}：　入力してください。
                            }
                        }
                        else
                        {
                            // 郵便番号未入力 - 住所あり - 郵便番号セット
                            entry.Tdkyub = jyusyo.Yubinn;
                        }
                    }
                    else
                    {
                        if (entry.Yubflg)
                        {
                            // 郵便番号入力済 - 郵便番号を必須としない:On  <= チェックなし
                        }
                        else
                        {
                            // 郵便番号入力済 - 郵便番号を必須としない:Off <= 郵便番号、住所整合性チェック
                            if (jyusyo == null || entry.Tdkyub != jyusyo.Yubinn)
                            {
                                errors.Add((string.Empty, string.Format(Resources.Message.CE069, "届先郵便番号・届先住所","届先郵便番号と届先住所が一致しません"))); //{0}：　{1}。
                            }
                        }
                    }
                    // 品名欄 チェック
                    bool hinmeiErrDispFlg = true;
                    int hinmeiCnt = 0;
                    int syuksuCnt = 0;
                    foreach (ShuukaTyuumonshoEntryMeisaiViewModel meisai in meisaiData)
                    {                       

                        string eNo = meisai.No + "行目　";
                        // 品名コード 存在/整合性チェック
                        if (!string.IsNullOrEmpty(meisai.Hincod))
                        {
                            // レプリケーションチェック
                            if (await tableLockRepository.CheckTableLock("m_unsou_hinmei"))
                            {
                                errors.Add((string.Empty, string.Format(Resources.Message.CE069, "品名コード", "現在品名マスタはレプリケーション中です。　登録・更新する場合は、品名コードを削除してください"))); //{0}：　{1}。
                                break;
                            }

                            var hinmei = (await entryRepositorie.GetHinmeiAsync()).Where(x => x.Hincod == meisai.Hincod);
                            if (hinmei.Count() == 0)
                            {
                                errors.Add((string.Empty, eNo + string.Format(Resources.Message.CE067, "品名コード", "品名コード"))); //{0}：　マスタに存在しない{1}が設定されています。
                                //break;
                            }
                            else
                            {
                                var hin = hinmei.FirstOrDefault();
                                if (!hin.Hinnam.Trim().StartsWith(meisai.Hinnam))
                                {
                                    errors.Add((string.Empty, eNo + string.Format(Resources.Message.CE069, "品名コード・品名", "品名コードに紐づく品名と、入力した品名一致しません"))); //{0}：　{1}。
                                    //break;
                                }
                            }
                        }
                        // 品名 必須チェック
                        if (string.IsNullOrEmpty(meisai.Hinnam) && (!string.IsNullOrEmpty(meisai.Syuksu) && meisai.Syuksu != "0"))
                        {
                            hinmeiErrDispFlg = false;
                            errors.Add((string.Empty, eNo + string.Format(Resources.Message.CE066, "品名"))); //{0}：　入力してください。
                           //break;
                        }
                        // 品名 文字数チェック
                        if (!string.IsNullOrEmpty(meisai.Hinnam) && meisai.Hinnam.Length > 26)
                        {
                            errors.Add((string.Empty, eNo + string.Format(Resources.Message.CE052, "品名", "26"))); //{0}：　{1}文字以内で入力してください。
                           //break;
                        }
                        //品名 全角チェック
                        Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                        if (sjisEnc.GetByteCount(meisai.Hinnam) != meisai.Hinnam.Length * 2)
                        {
                            errors.Add((string.Empty, eNo + string.Format(Resources.Message.CE109, "品名"))); //{0}：　全角文字を入力してください
                            //break;
                        }
                        // 数量 数値チェック
                        if (!string.IsNullOrEmpty(meisai.Syuksu) && !decimal.TryParse(meisai.Syuksu, out decimal dec))
                        {
                            errors.Add((string.Empty, eNo + string.Format(Resources.Message.CE043, "数量"))); //{0}：半角数値を入力してください。
                            //break;
                        }
                    }
                    // １明細以上(必須)チェック
                    foreach (ShuukaTyuumonshoEntryMeisaiViewModel meisai in meisaiData)
                    {
                        if (!string.IsNullOrEmpty(meisai.Hinnam) | !string.IsNullOrEmpty(meisai.Syuksu))
                        {
                            hinmeiCnt++;
                        }
                        if (!string.IsNullOrEmpty(meisai.Syuksu))
                        {
                            if (decimal.TryParse(meisai.Syuksu, out decimal dec))
                            {
                                if (dec != 0)
                                {
                                    syuksuCnt++;
                                }
                            }
                        }
                    }
                    if (hinmeiErrDispFlg && hinmeiCnt == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE066, "品名"))); //{0}：　入力してください。
                    }
                    if (syuksuCnt == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE066, "数量"))); //{0}：　入力してください。
                    }

                    // 出荷数チェック
                    decimal Dsyuksu = Convert.ToDecimal(entry.Dsyuksu);
                    if (Dsyuksu > 99999999999)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "出荷数", "出荷数は11桁以下にしてください"))); //{0}：　{1}。
                    }

                    // 特別指定コード 整合性チェック
                    if (!string.IsNullOrEmpty(entry.Unscod) && entry.Unscod != "31" && !string.IsNullOrEmpty(entry.Yusono))
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE070, "特別指定コード"))); //{0}：　誤りがあります。
                    }
                    // 機種A 存在チェック
                    if (!string.IsNullOrEmpty(entry.Kisyua))
                    {
                        var kisyu = (await entryRepositorie.GetMKishuAsync()).Where(x => x.Kisyua == entry.Kisyua);
                        if (kisyu.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "機種Aコード", "機種Aコード"))); //{0}：　マスタに存在しない{1}が設定されています。
                        }
                    }
                    // 運送方法 存在チェック
                    if (!string.IsNullOrEmpty(entry.Unscod))
                    {
                        var unsouhouhou = (await entryRepositorie.GetMUnsouUnsouhouhouAsync()).Where(x => x.Unscod == entry.Unscod);
                        if (unsouhouhou.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "運送方法コード", "運送方法コード")));  //{0}：　マスタに存在しない{1}が設定されています。
                        }
                    }
                    // コース 条件付き必須チェック
                    if (!string.IsNullOrEmpty(entry.Unscod) && entry.Unscod == "81" && string.IsNullOrEmpty(entry.Unscrs))
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE066, "コース"))); //{0}：　入力してください。
                    }

                    // コース 条件付きチェック
                    if (entry.Unscod != "81" && !string.IsNullOrEmpty(entry.Unscrs))
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "コース", "削除してください"))); //{0}：　{1}。
                    }

                    // 運送区分 条件付きチェック
                    if (string.IsNullOrEmpty(entry.Unscod) && !string.IsNullOrEmpty(entry.Unskbn))
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "運送区分", "削除してください"))); //{0}：　{1}。
                    }

                    // 仕入先コード 条件付きチェック
                    if (string.IsNullOrEmpty(entry.Unscod) && !string.IsNullOrEmpty(entry.Sircod))
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "仕入先", "削除してください"))); //{0}：　{1}。
                    }

                    // 仕入先コード 存在コードチェック
                    if (!string.IsNullOrEmpty(entry.Sircod))
                    {
                        var shiiresaki = (await entryRepositorie.GetMTorihikisakiAsync()).Where(x => x.Torcod == entry.Sircod);
                        if (shiiresaki.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "仕入先コード", "仕入先コード"))); //{ 0}：　マスタに存在しない{ 1}が設定されています。
                        }
                    }
                    // 運送区分 存在チェック
                    if (!string.IsNullOrEmpty(entry.Unskbn))
                    {
                        var unsoukubun = (await entryRepositorie.GetMUnsouUnsoukubunAsync()).Where(x => x.Unskbn == entry.Unskbn);
                        if (unsoukubun.Count() == 0)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE067, "運送区分コード", "運送区分コード")));  //{ 0}：　マスタに存在しない{ 1}が設定されています。
                        }
                    }

                    if (mode == null)
                    {
                        // 出荷日チェック
                        // 過去日、2か月以上未来日はエラー
                        if (DateTime.Parse(entry.Sykymd).CompareTo(DateTime.Today) == -1)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "出荷日", "過去日は入力できません"))); //{0}：　{1}。
                        }
                        else if (DateTime.Parse(entry.Sykymd).CompareTo(DateTime.Today.AddMonths(2)) == 1)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "出荷日", "２か月以上の未来日は入力できません"))); //{0}：　{1}。
                        }
                    } else if (mode == StrEdit)
                    {
                        if (DateTime.Parse(entry.Sykymd).CompareTo(DateTime.Today.AddMonths(2)) == 1)
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "出荷日", "２か月以上の未来日は入力できません"))); //{0}：　{1}。
                        }
                    }

                }
                if (mode == StrEdit)
                {
                    bool MukouFlg = false;
                    var updData = (await entryRepositorie.GetShuukaTyuumonshoAsync(entry.Syukno, entry.Cdate));
                    if (updData.Count() != 0)
                    {
                        var updDataEnt = updData.FirstOrDefault();
                        if (updDataEnt.Mukoukbn)
                        {
                            if (!entry.Mukoukbn)
                            {
                                // 無効→有効
                                errors.Add((string.Empty, "無効データを有効データには変更できません。"));
                                MukouFlg = true;
                            }
                        }
                    }

                    if (!MukouFlg)
                    {
                        // 更新
                        // 無効区分 チェック
                        if (entry.Mukoukbn)
                        {
                            // ON:  無効理由が入力されていない場合はエラー
                            if (string.IsNullOrEmpty(entry.Mukouriyuu))
                            {
                                errors.Add((string.Empty, string.Format(Resources.Message.CE068, "無効化チェック・無効化理由", "無効化チェック時は必ず無効化理由を入力"))); //{0}：　{1}してください。
                            }
                        }
                        else
                        {
                            // OFF: 無効理由が入力されている場合はエラー
                            if (!string.IsNullOrEmpty(entry.Mukouriyuu))
                            {
                                errors.Add((string.Empty, string.Format(Resources.Message.CE068, "無効化理由", "無効化理由をクリア"))); //{0}：　{1}してください。
                            }
                        }
                    }

                    // 更新対象 存在チェック
                    //var updData = (await entryRepositorie.GetShuukaTyuumonshoAsync(entry.Syukno, entry.Cdate));
                    if (updData.Count() == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "更新対象データ", "更新対象データは存在しません"))); //{0}：　{1}。
                        //errors.Add((string.Empty, Resources.Message.CE059)); //は存在しません。
                    }
                    else
                    {
                        // 注文書発行済かつ出荷日が30日前未満 チェック
                        var updDataEnt = updData.FirstOrDefault();
                        if (!string.IsNullOrEmpty(updDataEnt.Denf) && (updDataEnt.Denf == "A" | updDataEnt.Denf == "S") && DateTime.Parse(updDataEnt.Sykymd) < DateTime.Now.Date.AddDays(-30))
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "発行済・出荷日", "発行済かつ出荷日が現在日より31日以上前の為、更新できません"))); //{0}：　{1}。
                            //errors.Add((string.Empty, "発行済かつ出荷日が現在日より31日以上前の為、更新できません。"));
                        }
                    }
                }
                if (mode == StrDelete)
                {
                    // 削除
                    // 削除対象 存在チェック
                    var delData = (await entryRepositorie.GetShuukaTyuumonshoAsync(entry.Syukno, entry.Cdate));
                    if (delData.Count() == 0)
                    {
                        errors.Add((string.Empty, string.Format(Resources.Message.CE069, "削除対象データ", "削除対象データは存在しません"))); //{0}：　{1}。
                        //errors.Add((string.Empty, Resources.Message.CE065)); //削除対象データは存在しません。
                    }
                    else
                    {
                        // 注文書発行済 チェック
                        var delDataEnt = delData.FirstOrDefault();
                        if (!string.IsNullOrEmpty(delDataEnt.Denf) && (delDataEnt.Denf == "A" | delDataEnt.Denf == "S"))
                        {
                            errors.Add((string.Empty, string.Format(Resources.Message.CE069, "注文書発行済", "発行済の為、削除できません"))); //{0}：　{1}。
                            //errors.Add((string.Empty, "発行済の為、削除できません。"));
                        }
                    }
                }

                //if (!ModelState.IsValid)
                //{
                //    ViewBag.Mode = entry.Mode;
                //    return View(entry);
                //}
                if (errors.Any())
                {
                    foreach (var (field, message) in errors)
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                    ViewBag.Mode = entry.Mode;
                    return View(entry);
                }

                // DB更新
                if (mode == null)
                {
                    entry.Syukno = entry.SyuknoMae + entry.SyuknoAto;
                    entry.Cdate = DateTime.Now.ToString("yyMMdd");
                }
                await entryRepositorie.UpdateShuukaTyuumonshoAsync(mode, entry, meisaiData, loginUser);

                // ログ履歴作成
                switch (mode)
                {
                    case null:
                        logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U1, "処理機能：" + menu.TitleName, string.Format("追加キー：{0}, {1}", entry.Syukno, entry.Cdate), logRepository);
                        break;
                    case StrEdit:
                        logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U2, "処理機能：" + menu.TitleName, string.Format("出荷注文書手配ﾃﾞｰﾀ 更新キー：{0}, {1}" + Environment.NewLine + "出荷手配データ 更新キー：{0}, {1}", entry.Syukno, entry.Cdate), logRepository);
                        break;
                    case StrDelete:
                        logService.CreateLogHistory(loginUser, menu.MenuId, ControlRepository.MControlFunctionKbn.U3, "処理機能：" + menu.TitleName, string.Format("削除キー：{0}, {1}", entry.Syukno, entry.Cdate), logRepository);
                        break;
                }

                // 画面遷移
                if (mode == null)
                {
                    // 新規
                    return this.Redirect("Details");
                }
                else
                {
                    // 新規以外
                    return this.Redirect("Index");
                }

            }
            catch (Exception ex)
            {
                // エラー画面へ遷移
                throw ex;
            }

        }

        private async Task<MUnsouPostalcode> CheckJyusyAsync(string jyusy, string yubinn)
        {

            //MUnsouPostalcode jyusyo = null;
            string jyusy3 = jyusy.Length>= 3 ? jyusy.Substring(0, 3) : jyusy;
            var jyusyo1 = (await entryRepositorie.GetMUnsouPostalcodeAsync()).Where(x => x.Jyusy1.Contains(jyusy3)).OrderBy(x => x.Jyusy2);
            if (jyusyo1.Count() != 0)
            {
                for (int idx = jyusy.Length; idx > 0; idx--)
                {
                    //jyusyo = jyusyo1.Where(x => x.Jyusy1 + x.Jyusy2 == jyusy.Substring(0, idx)).FirstOrDefault();

                    var jyusyos = jyusyo1.Where(x => x.Jyusy1 + x.Jyusy2 == jyusy.Substring(0, idx));
                    if (jyusyos != null && jyusyos.Count() != 0)
                    {
                        if (string.IsNullOrEmpty(yubinn))
                        {
                            return jyusyos.First();
                        }
                        else
                        {
                            foreach (var jyusyo in jyusyos)
                            {
                                if (jyusyo.Yubinn == yubinn)
                                {
                                    return jyusyo;
                                }
                            }
                        }
                    }
                }
            }
            return null;

        }
    }

    
}