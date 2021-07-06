using Macss.Attributes.ActionFilter;
using Macss.Models;
using Macss.Repositories;
using Macss.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Macss.Controllers
{
    public class ControlController : Controller
    {
        private IAccountRepository accountRepository;
        private IControlRepository controlRepository;
        private LogService logService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var dbContext = HttpContext.GetOwinContext().Get<ApplicationDB>();
            accountRepository = new AccountRepository(dbContext);
            controlRepository = new ControlRepository(dbContext);
            logService = new LogService();
        }

        // GET: Control
        [AuthorityActionFilter]
        public async Task<ActionResult> Index()
        {
            var viewData = new ControlViewModel();

            // 社内用お知らせ
            var ininfo = await GetcontrolDataAsync("1");
            if (ininfo != null)
            {
                // お知らせ
                viewData.InInformation = ininfo.Value1;
                // 投稿者
                var appUserByCode = await accountRepository.FindByCodeAsync(ininfo.UpdateId);
                if (appUserByCode.Count() != 0)
                {
                    var appUse = appUserByCode.First();
                    viewData.InPostUser = appUse.UserName;
                }
                // 掲載日
                viewData.InPostDate = ininfo.UpdateDate.ToString("yyyy/MM/dd");
                // 掲載期間 From
                viewData.InDateFrom = ininfo.Value2;
                // 掲載期間 To
                viewData.InDateTo = ininfo.Value3;

            }

            // 社外用お知らせ
            var outinfo = await GetcontrolDataAsync("2");
            if (outinfo != null)
            {
                // お知らせ
                viewData.OutInformation = outinfo.Value1;
                // 投稿者
                var appUserByCode = await accountRepository.FindByCodeAsync(outinfo.UpdateId);
                if (appUserByCode.Count() != 0)
                {
                    var appUse = appUserByCode.First();
                    viewData.OutPostUser = appUse.UserName;
                }
                // 掲載日
                viewData.OutPostDate = outinfo.UpdateDate.ToString("yyyy/MM/dd");
                // 掲載期間 From
                viewData.OutDateFrom = outinfo.Value2;
                // 掲載期間 To
                viewData.OutDateTo = outinfo.Value3;

            }


            var passList = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password);
            if (passList != null)
            {
                var passArray = passList.ToArray();

                // パスワード桁数
                if (passArray[0] != null)
                {
                    var val01 = passArray[0].Value1;
                    var val02 = passArray[0].Value2;
                    viewData.PassKetaFrom = val01;
                    viewData.PassKetaTo = val02;
                }

                // パスワード有効期限（月）
                if (passArray[1] != null)
                {
                    var val1 = passArray[1].Value1;
                    viewData.PassMonth = val1;
                }

                // パスワード文字種類（アルファベット大文字・小文字・数字・記号）
                if (passArray[2] != null)
                {
                    char[] charArray = passArray[2].Value1.ToCharArray();
                    viewData.PassType1 = charArray[0] == '1' ? true : false;
                    viewData.PassType2 = charArray[1] == '1' ? true : false;
                    viewData.PassType3 = charArray[2] == '1' ? true : false;
                    viewData.PassType4 = charArray[3] == '1' ? true : false;
                }

                // パスワード世代管理
                if (passArray[3] != null)
                {
                    var val3 = passArray[3].Value1;
                    viewData.PassGene = val3;
                }

                var pass = passList.OrderByDescending(x => x.UpdateDate).First();
                if (pass != null)
                {
                    // 投稿者
                    var appUserByCode = await accountRepository.FindByCodeAsync(pass.UpdateId);
                    if (appUserByCode.Count() != 0)
                    {
                        var appUse = appUserByCode.First();
                        viewData.PsPostUser = appUse.UserName;
                    }
                    // 掲載日
                    viewData.PsPostDate = pass.UpdateDate.ToString("yyyy/MM/dd");
                }
            }

            var loginCounts = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.LoginCount);
            if (loginCounts != null && loginCounts.Count() != 0)
            {
                var loginCount = loginCounts.First();
                int val = Decimal.ToInt32((decimal)loginCount.NumericValue1);
                viewData.PassMiss = val.ToString();

                if (DateTime.Parse(viewData.PsPostDate) < loginCount.UpdateDate)
                {
                    viewData.PsPostDate = loginCount.UpdateDate.ToString("yyyy/MM/dd");
                    viewData.PsPostUser = string.Empty;
                    var appUserByCode = await accountRepository.FindByCodeAsync(loginCount.UpdateId);
                        if (appUserByCode.Count() != 0)
                        {
                            var appUse = appUserByCode.First();
                            viewData.PsPostUser = appUse.UserName;
                        }
                }
            }

            return View(viewData);

        }

        [HttpPost]
        [AuthorityActionFilter]
        [ValidateInput(false)]
        public async Task<ActionResult> Index(ControlViewModel control)
        {
            var userID = Session.GetUserID();

            control.InDateFrom = SetDataString(control.InDateFrom);
            control.InDateTo = SetDataString(control.InDateTo);

            control.OutDateFrom = SetDataString(control.OutDateFrom);
            control.OutDateTo = SetDataString(control.OutDateTo);


            if ((control.InInformation != null && control.InInformation.Length > 256) ||
                (control.OutInformation != null && control.OutInformation.Length > 256))
            {

                ModelState.AddModelError(String.Empty, "お知らせは256文字以内で登録してください。");

                var ininfo = await GetcontrolDataAsync("1");
                if (ininfo != null)
                {
                    control.InPostDate = ininfo.UpdateDate.ToString("yyyy/MM/dd");
                    var appUserByCode = await accountRepository.FindByCodeAsync(ininfo.UpdateId);
                    var appUse = appUserByCode.First();
                    control.InPostUser = appUse.UserName;
                }

                var outinfo = await GetcontrolDataAsync("2");
                if (outinfo != null)
                {
                    control.InPostDate = outinfo.UpdateDate.ToString("yyyy/MM/dd");
                    var appUserByCode = await accountRepository.FindByCodeAsync(outinfo.UpdateId);
                    var appUse = appUserByCode.First();
                    control.OutPostUser = appUse.UserName;
                }

                return View(control);
            }

            try
            {
                // 社内お知らせ
                var ininfo = await GetcontrolDataAsync("1");
                if (ininfo != null)
                {
                    if (ininfo.Value1 != control.InInformation || ininfo.Value2 != control.InDateFrom || ininfo.Value3 != control.InDateTo)
                    {
                        await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.Information, "1", control.InInformation, control.InDateFrom, control.InDateTo, null, null, null, userID);
                    }
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        // VisualStudioの出力に表示
                        System.Diagnostics.Trace.WriteLine(error.ErrorMessage);
                    }
                }
            }

            try
            {
                // 社外お知らせ
                var Outinfo = await GetcontrolDataAsync("2");
                if (Outinfo != null)
                {
                    if (Outinfo.Value1 != control.OutInformation || Outinfo.Value2 != control.OutDateFrom || Outinfo.Value3 != control.OutDateTo)
                    {
                        await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.Information, "2", control.OutInformation, control.OutDateFrom, control.OutDateTo, null, null, null, userID);
                    }
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        // VisualStudioの出力に表示
                        System.Diagnostics.Trace.WriteLine(error.ErrorMessage);
                    }
                }
            }

            var passList = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password);
            if (passList != null)
            {
                var passArray = passList.ToArray();

                // パスワード桁数
                if (passArray[0] != null)
                {

                    if (passArray[0].Value1 != control.PassKetaFrom || passArray[0].Value2 != control.PassKetaTo)
                    {
                        if (!int.TryParse(control.PassKetaFrom, out int ketaFrom))
                        {
                            control.PassKetaFrom = string.Empty;
                        }
                        if (!int.TryParse(control.PassKetaTo, out int ketaTo))
                        {
                            control.PassKetaTo = string.Empty;
                        }
                        await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.Password, "1", control.PassKetaFrom, control.PassKetaTo, "パスワード桁数", null, null, null, userID);
                    }

                }

                // パスワード有効期限（月）
                if (passArray[1] != null)
                {
                    if (passArray[1].Value1 != control.PassMonth)
                    {
                        if (int.TryParse(control.PassMonth, out int month))
                        {
                            await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.Password, "2", control.PassMonth, "パスワード有効期限（月）", "", null, null, null, userID);
                        }
                    }
                }

                // パスワード文字種類（アルファベット大文字・小文字・数字・記号）
                if (passArray[2] != null)
                {
                    char[] charArray = passArray[2].Value1.ToCharArray();

                    var PassType1 = control.PassType1 ? '1' : '0';
                    var PassType2 = control.PassType2 ? '1' : '0';
                    var PassType3 = control.PassType3 ? '1' : '0';
                    var PassType4 = control.PassType4 ? '1' : '0';

                    if (charArray[0] != PassType1 || charArray[1] != PassType2 || charArray[2] != PassType3 || charArray[3] != PassType4)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(control.PassType1 ? "1" : "0");
                        sb.Append(control.PassType2 ? "1" : "0");
                        sb.Append(control.PassType3 ? "1" : "0");
                        sb.Append(control.PassType4 ? "1" : "0");

                        await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.Password, "3", sb.ToString(), "パスワード文字種類（アルファベット大文字・小文字・数字・記号）", "", null, null, null, userID);

                    }

                }

                // パスワード世代管理
                var PassGene = string.Empty;
                if (passArray[3] != null)
                {
                    if (passArray[3].Value1 != control.PassGene)
                    {
                        if (int.TryParse(control.PassGene, out int gene))
                        {
                            await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.Password, "4", control.PassGene, "パスワード履歴回数", "", null, null, null, userID);
                        }
                    }
                }

                var pass = passList.OrderByDescending(x => x.UpdateDate).First();
                if (pass != null)
                {
                    // 投稿者
                    var appUserByCode = await accountRepository.FindByCodeAsync(pass.UpdateId);
                    if (appUserByCode.Count() != 0)
                    {
                        var appUse = appUserByCode.First();
                        control.PsPostUser = appUse.UserName;
                    }
                    // 掲載日
                    control.PsPostDate = pass.UpdateDate.ToString("yyyy/MM/dd");
                }
            }

            // パスワード履歴
            var loginCount = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.LoginCount)).Where(x => x.Kbn == "1").First();
            if (loginCount != null)
            {

                if ((decimal)loginCount.NumericValue1 != decimal.Parse(control.PassMiss))
                {
                    if (int.TryParse(control.PassMiss, out int miss))
                    {
                        await controlRepository.UpdateDataValueAsync(ControlRepository.MControlSection.LoginCount, "1", "", "", "", control.PassMiss, null, null, userID);

                        if (DateTime.Parse(control.PsPostDate) < loginCount.UpdateDate)
                        {
                            control.PsPostDate = loginCount.UpdateDate.ToString("yyyy/MM/dd");
                            control.PsPostUser = string.Empty;
                            var appUserByCode = await accountRepository.FindByCodeAsync(loginCount.UpdateId);
                            if (appUserByCode.Count() != 0)
                            {
                                var appUse = appUserByCode.First();
                                control.PsPostUser = appUse.UserName;
                            }
                        }
                    }
                }
            }

            var info = await GetcontrolDataAsync("1");
            if (info != null)
            {
                control.InPostDate = info.UpdateDate.ToString("yyyy/MM/dd");
            }

            info = await GetcontrolDataAsync("2");
            if (info != null)
            {
                control.OutPostDate = info.UpdateDate.ToString("yyyy/MM/dd");
            }

            // 完了メッセージ
            ViewBag.Message = String.Format(Resources.Message.CI003);

            return View(control);

        }

        private async Task<MControl> GetcontrolDataAsync(string kbn)
        {

            IEnumerable<MControl> ininfos = await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Information);
            if (ininfos != null && ininfos.Count() != 0)
            {
                var list = ininfos.Where(x => x.Kbn == kbn).ToList();
                if (list != null && list.Count() != 0) return list.First();
                else return null;

            }
            else
            {
                return null;
            }

        }

        private string SetDataString(string value)
        {

            DateTime date;
            if (!DateTime.TryParse(value, out date))
            {
                return "";
            }
            else
            {
                return date.ToString("yyyy/MM/dd");
            }
        }
    }
}