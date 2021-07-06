using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Macss.App_Start;
using Macss.Areas.Fdass.Common;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public const string InitPassword = "fb000000";


        private readonly ApplicationDB dbContext;
        private readonly ApplicationUserManager userManager;
        private IControlRepository controlRepository;

        #region 定数

        public enum DeleteFlg
        {
            Disp = 0,
            Hide = 1,
        }

        #endregion

        public AccountRepository(ApplicationDB db, ApplicationUserManager manager)
        {
            this.dbContext = db;
            this.userManager = manager;
            controlRepository = new ControlRepository(dbContext);
        }

        public AccountRepository(ApplicationDB db)
        {
            this.dbContext = db;
            controlRepository = new ControlRepository(dbContext);
        }

        public async Task<IEnumerable<AccountInformation>> GetAllUsersAsync()
        {
            var accountList = await dbContext.MAccount
                            .Join(dbContext.MControl, x => x.BumonCd, x => x.Kbn , (a, c1) => new { a, c1 })
                            .Where(x => x.c1.Section == ControlRepository.MControlSection.AccountBumon)
                            .Join(dbContext.MControl, x => x.a.GroupCd, x => x.Kbn, (a, c2) => new { a = a.a, c1 = a.c1, c2 })
                            .Where(x => x.c2.Section == ControlRepository.MControlSection.AccountGroup)
                            //.Join(dbContext.MShukkabasho, x => x.a.BasyoCd, x => x.Sybcod, (a, b) => new { a = a.a, c1 = a.c1, c2 = a.c1, b })
                            .GroupJoin(dbContext.MShukkabasho, x => x.a.BasyoCd, x => x.Sybcod, (a, b) => new { a = a.a, c1 = a.c1, c2 = a.c2, b })
                            .Select(x => new AccountInformation()
                            {
                                AccountId = x.a.Id,
                                AccountName = x.a.UserName,
                                BumonCd = x.c1.Value1,
                                BasyoCd = x.b.Any() == true ? x.b.FirstOrDefault().Sybnam : "",
                                GroupCd = x.c2.Value1,
                                RockFlg = x.a.LoginFailureCount.ToString(),
                                UpdateDateTime = x.a.UpdateDate,
                                CreateDateTime = x.a.CreateDate,
                                DeleteFlg = x.a.DeleteFlg == (int)DeleteFlg.Disp ? String.Empty : "削除",
                            }).ToListAsync();

            // アカウントロールデータを取得
            var roleList = await dbContext.MAccountRole
                                    .Join(dbContext.MRole, x => x.RoleId, x => x.RoleId, (ar, r) => new { ar, r })
                                    .Select(x => new { x.ar.AccountId, x.r.RoleName }).ToListAsync();

            // ロールデータ、更新日を加工
            var accountData = accountList
                                .Select(x => new AccountInformation()
            {
                                    AccountId = x.AccountId,
                                    AccountName = x.AccountName,
                                    BumonCd = x.BumonCd,
                                    BasyoCd = x.BasyoCd,
                                    GroupCd = x.GroupCd,
                                    RockFlg = x.RockFlg,
                                    UpdateDate = x.UpdateDateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                                    CreateDate = x.CreateDateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                                    DeleteFlg = x.DeleteFlg,
                                    Role = string.Join(", ", roleList.Where(y => y.AccountId == x.AccountId).Select(y => y.RoleName).ToList())
                                }).ToList();

            return accountData;
        }

        public async Task<IEnumerable<AccountRoleViewModel>> GetAccountRolesAsync(string accountId)
        {
            return await dbContext.MAccountRole.Where(x => x.AccountId == accountId)
                .Join(dbContext.MRole, x => x.RoleId, x => x.RoleId, (ar, r) => new { ar, r })
                .Select(x => new AccountRoleViewModel() { AccountId = x.ar.AccountId, RoleId = x.r.RoleId, RoleName = x.r.RoleName }).ToListAsync();
        }


        public async Task<IEnumerable<MAccount>> FindByCodeAsync(string code)
        {
            return await dbContext.MAccount.Where(x => x.Id == code).ToListAsync();
        }

        public async Task<IEnumerable<TPasswordHistory>> GetPasswordHistoryAsync(string accountId, bool orderF)
        {
            if (orderF)
            {
                return await dbContext.TPasswordHistory.Where(x => x.Id == accountId).OrderBy(x => x.CreateDate).ToListAsync();
            } else {
                return await dbContext.TPasswordHistory.Where(x => x.Id == accountId).OrderByDescending(x => x.CreateDate).ToListAsync();
            }
        }

        public async Task<int> GetPasswordControlValueAsync(string kbn, int defaultValue)
        {
            var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == kbn).FirstOrDefault();
            string digits = null;
            if (controls != null)
            {
                digits = controls.Value1;
            }
            if (!int.TryParse(digits, out int idigits))
            {
                idigits = defaultValue;
            }
            return idigits;
        }


        /// <summary>
        /// ユーザー情報のログイン失敗回数を更新する
        /// </summary>
        public async Task UpdateFailureCountAsync(string userID,int count)
        {
            var userData = await dbContext.MAccount.FirstOrDefaultAsync(x => x.Id == userID);
            if (userData == null) { return; }
            userData.LoginFailureCount = count;
            if (count == 0)
            {
                userData.LoginCount = userData.LoginCount + 1;
                userData.LastLoginDate = DateTime.Now;
            }


            try { 
            await dbContext.SaveChangesAsync();

            }
            catch (DbEntityValidationException dbEx)
            {
                DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                throw dbEx;
            } catch (Exception ex)
            {
                throw ex;
            }

            return;
        }

        public async Task<IEnumerable<(string field, string message)>> UpdateUserAsync(string id, AccountViewModel user, string loginUser)
        {
            var errors = new List<(string field, string message)>();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                // アカウントマスタ更新
                var appUser = await userManager.FindByIdAsync(id);

                errors = ConfirmUpdPassword(user.Password, user.Mode);
                if (errors.Any())
                {
                    return errors;
                }

                // 新規の場合
                if (user.Mode == 0)
                {
                    // ID重複確認
                    if (appUser != null)
                    {
                        //errors.Add((String.Empty, String.Format(Resources.Message.CE057, "ユーザID")));
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "ユーザID", "重複しています"))); //{0}：　{1}。
                        return errors;
                    }
                    // コード重複確認
                    var appUserByCode = await FindByCodeAsync(user.AccountId);
                    if (appUserByCode.Count() > 0)
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "担当者コード", "重複しています"))); //{0}：　{1}。

                        //errors.Add((String.Empty, String.Format(Resources.Message.CE057, "担当者コード")));
                        return errors;
                    }

                    appUser = user.Model;
                    appUser.Password = user.Password = userManager.PasswordHasher.HashPassword(user.Password);
                    appUser.DeleteFlg = user.DeleteFlg == true ? (int)DeleteFlg.Hide : (int)DeleteFlg.Disp;
                    appUser.BasyoCd = user.BasyoCd;
                    appUser.BumonCd = user.BumonCd;
                    appUser.GroupCd = user.GroupCd;
                    appUser.Sdek12 = user.Sdek12;
                    appUser.Ctlfl1 = user.Ctlfl1.Trim();
                    appUser.CreateId = loginUser;
                    appUser.CreateDate = DateTime.Now;
                    appUser.UpdateId = loginUser;
                    appUser.UpdateDate = DateTime.Now;
                    appUser.AccountPasswordDate = DateTime.Now;
                    dbContext.MAccount.Add(appUser);

                    //パスワード履歴更新
                    TPasswordHistory pHistory = new TPasswordHistory
                    {
                        Id = id,
                        Password = appUser.Password,
                        CreateId = loginUser,
                        CreateDate = DateTime.Now,
                        UpdateId = loginUser,
                        UpdateDate = DateTime.Now
                    };
                    dbContext.TPasswordHistory.Add(pHistory);

                }
                // 更新の場合
                else
                {
                    //パスワード履歴更新
                    int idigits = await GetPasswordControlValueAsync("4", 1);
                    //var pHs = await GetPasswordHistoryAsync(id, true);
                    //int cnt = 0;
                    //int pHCnt = pHs.Count();
                    //foreach (var pH in pHs)
                    //{
                    //    if (cnt == idigits) break;
                    //    var ret = userManager.PasswordHasher.VerifyHashedPassword(pH.Password, user.Password);
                    //    if (Microsoft.AspNet.Identity.PasswordVerificationResult.Success == ret)
                    //    {
                    //        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "パスワード", "過去に使用したパスワードは設定できません"))); //{0}：　{1}。
                    //        return errors;

                    //    }
                    //    cnt++;
                    //}

                    if (!String.IsNullOrEmpty(user.Password))
                    {
                        appUser.Password = user.Password = userManager.PasswordHasher.HashPassword(user.Password);
                        appUser.AccountPasswordDate = DateTime.Now;
                    }
                    appUser.UserName = user.AccountName;
                    appUser.AccountNameKana = user.AccountNameKana;
                    appUser.BasyoCd = user.BasyoCd;
                    appUser.BumonCd = user.BumonCd;
                    appUser.GroupCd = user.GroupCd;
                    appUser.Sdek12 = user.Sdek12;
                    appUser.Ctlfl1 = user.Ctlfl1.Trim();
                    appUser.DeleteFlg = user.DeleteFlg == true ? (int)DeleteFlg.Hide : (int)DeleteFlg.Disp;
                    if (user.UserLock == false)
                    {
                        appUser.LoginFailureCount = 0;
                    } else
                    {
                        appUser.LoginFailureCount = 1000;
                    }
                    appUser.UpdateId = loginUser;
                    appUser.UpdateDate = DateTime.Now;
                    await dbContext.SaveChangesAsync();

                    ////パスワード履歴更新
                    //var pHs = await GetPasswordHistoryAsync(id, true);
                    //var pHCnt = pHs.Count();
                    //if (pHCnt >= idigits)
                    //{
                    //    foreach (var pH in pHs)
                    //    {
                    //        if (pHCnt == idigits - 1) break;
                    //        dbContext.TPasswordHistory.Remove(pH);
                    //        pHCnt--;
                    //    }
                    //}

                    //TPasswordHistory pHistory = new TPasswordHistory
                    //{
                    //    Id = id,
                    //    Password = appUser.Password,
                    //    CreateId = id,
                    //    CreateDate = DateTime.Now,
                    //    UpdateId = id,
                    //    UpdateDate = DateTime.Now
                    //};
                    //dbContext.TPasswordHistory.Add(pHistory);

                }

                //アカウントロール更新
                var dbRole = (await GetAccountRolesAsync(id)).ToArray();
                var except = dbRole.Where(x => !user.SetRoles.Contains(x.RoleId)).Select(x => x.RoleId).ToArray();
                var expectData = dbContext.MAccountRole.Where(x => x.AccountId == id).Where(x => except.Contains(x.RoleId));
                dbContext.MAccountRole.RemoveRange(expectData);
                await dbContext.SaveChangesAsync();

                //var addRoleId = user.SetRoles.Except(appUser.MAccountRole.Select(x => x.RoleId)).ToArray();
                var addRoleId = user.SetRoles.Except(dbRole.Select(x => x.RoleId).ToArray()).ToArray();
                List<MAccountRole> addList = new List<MAccountRole>();
                for (int i = 0; i < addRoleId.Length; i++)
                {
                    var add = new MAccountRole();
                    add.RoleId = addRoleId[i];
                    add.AccountId = id;
                    add.CreateId = loginUser;
                    add.UpdateId = loginUser;
                    add.CreateDate = DateTime.Now;
                    add.UpdateDate = DateTime.Now;
                    addList.Add(add);
                }
                //ret = await userManager.AddToRolesAsync(id, add);
                dbContext.MAccountRole.AddRange(addList);
                await dbContext.SaveChangesAsync();

                if (errors.Any())
                {
                    transaction.Rollback();
                }
                else
                {
                    transaction.Commit();
                }
            }
            return errors;
        }

        public List<(string field, string message)> ConfirmUpdPassword(string password, int mode)
        {
            var errors = new List<(string field, string message)>();

            // 新規の場合のみ必須
            if (mode == 0 && String.IsNullOrEmpty(password))
            {
                //errors.Add((String.Empty, (String.Format(Resources.Message.CE032, "パスワード", 6, 12))));
                errors.Add((String.Empty, (String.Format(Resources.Message.CE071, "パスワード", 6, 12))));
                return errors;
            }

            if (mode == 0 || (mode != 0 && !String.IsNullOrEmpty(password)))
            {
                if (password.Length < 6 || password.Length > 12)
                {
                    errors.Add((String.Empty, (String.Format(Resources.Message.CE071, "パスワード", 6, 12))));
                    return errors;
                }
                string[] banChar = { "\\", "%", "_", ";" };
                foreach (string s in banChar)
                {
                    if (password.Contains(s))
                    {
                        errors.Add((String.Empty, (String.Format(Resources.Message.CE069, "パスワード", "禁則文字が存在します。登録できません"))));
                        //errors.Add((String.Empty, (String.Format(Resources.Message.CE041, "パスワードに禁則文字", "登録"))));

                        return errors;
                    }
                }

            }
            return errors;
        }
        public async Task<List<(string field, string message)>> ConfirmPassword(string password, int mode)
        {
            var errors = new List<(string field, string message)>();

            int idigits1= await GetPasswordControlValueAsync("1", -1);
            var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "1").FirstOrDefault();
            // 新規の場合のみ必須
            if (mode == 0 && String.IsNullOrEmpty(password))
            {
                //errors.Add((String.Empty, (String.Format(Resources.Message.CE032, "パスワード", 6, 12))));
                if (controls == null)
                {
                    errors.Add((String.Empty, (String.Format(Resources.Message.CE071, "パスワード", 6, 12))));
                } else
                {
                    var val1 = controls.Value1;
                    var val2 = controls.Value2;
                    if (!string.IsNullOrEmpty(val2))
                    {
                        errors.Add((String.Empty, (String.Format(Resources.Message.CE071, "パスワード", val1, val2))));
                    } else
                    {
                        errors.Add((String.Empty, string.Format(Resources.Message.CE069, "パスワード", val1 + "文字で入力してください"))); //{0}：　{1}。
                    }                    
                    //errors.Add((String.Empty, string.Format(Resources.Message.CE069, "パスワード", idigits1.ToString() + "文字で入力してください"))); //{0}：　{1}。

                }
                return errors;
            }

            if (mode == 0 || (mode != 0 && !String.IsNullOrEmpty(password))) {
                if (controls == null) { 
                    if (password.Length < 6 || password.Length > 12)
                    {
                        errors.Add((String.Empty, (String.Format(Resources.Message.CE071, "パスワード", 6, 12))));
                        return errors;
                    }
                } else
                {
                    if (!int.TryParse(controls.Value1, out int val1))
                    {
                        val1 = 0;
                    }
                    if (!int.TryParse(controls.Value2, out int val2))
                    {
                        val2 = 0;
                    }

                    if (val2 != 0 && val1 <= val2)
                    {

                        if (val1 == val2)
                        {
                            if (password.Length != val1)
                            {
                                errors.Add((String.Empty, "パスワード：　" + controls.Value1 + "文字で入力してください。"));
                                return errors;
                            }
                        }
                        if ( password.Length < val1 || password.Length > val2)
                        {
                            errors.Add((String.Empty, (String.Format(Resources.Message.CE071, "パスワード", controls.Value1, controls.Value2))));
                            return errors;
                        }
                    } else
                    {
                        if ( password.Length < val1)
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "パスワード", controls.Value1 + "文字以上で入力してください"))); //{0}：　{1}。
                            return errors;
                        }
                    }

                }
                string[] banChar = { "\\", "%", "_", ";" };
                foreach (string s in banChar)
                {
                    if (password.Contains(s))
                    {
                        errors.Add((String.Empty, (String.Format(Resources.Message.CE069, "パスワード", "禁則文字が存在します。登録できません"))));
                        //errors.Add((String.Empty, (String.Format(Resources.Message.CE041, "パスワードに禁則文字", "登録"))));

                        return errors;
                    }
                }

                var controls3 = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.Password)).Where(x => x.Kbn == "3").FirstOrDefault();
                string digits3 = null;
                if (controls3 != null)
                {
                    digits3 = controls3.Value1;
                    var chTable = new Dictionary<string, string>();
                    chTable.Add("1000", "[A-Z]+");
                    chTable.Add("1100", "[A-Za-z]+");
                    chTable.Add("1110", "[A-Za-z0-9]+");
                    chTable.Add("1111", "[A-Za-z0-9!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]+");
                    chTable.Add("0100", "[a-z]+");
                    chTable.Add("0110", "[a-z0-9]+");
                    chTable.Add("0111", "[a-z0-9!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]+");
                    chTable.Add("0010", "[0-9]+");
                    chTable.Add("0011", "[0-9!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]+");
                    chTable.Add("0001", "[!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]+");
                    string chkStr = chTable[digits3];

                    char[] rgxs = digits3.ToCharArray();
                    var rgx1 = new Regex("[A-Z]+");
                    var rgx2 = new Regex("[a-z]+");
                    var rgx3 = new Regex("[0-9]+");
                    var rgx4 = new Regex("[!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~]+");
                    var rgx5 = new Regex(chkStr);

                    Match match1;
                    Match match2;
                    Match match3;
                    Match match4;
                    Match match5;

                    StringBuilder error = new StringBuilder();
                    bool errorF = false;
                    if (rgxs[0] == '1')
                    {
                        if (error.Length != 0) error.Append("・");
                        error.Append("半角英大文字");
                        match1 = rgx1.Match(password);
                        if (match1.Value.Length == 0)
                        {
                            errorF = true;
                        }
                    }
                    if (rgxs[1] == '1')
                    {
                        if (error.Length != 0) error.Append("・");
                        error.Append("半角英小文字");
                        match2 = rgx2.Match(password);
                        if (match2.Value.Length == 0)
                        {
                            errorF = true;
                        }
                    }
                    if (rgxs[2] == '1')
                    {
                        if (error.Length != 0) error.Append("・");
                        error.Append("数字");
                        match3 = rgx3.Match(password);
                        if (match3.Value.Length == 0)
                        {
                            errorF = true;
                        }
                    }
                    if (rgxs[3] == '1')
                    {
                        if (error.Length != 0) error.Append("・");
                        error.Append("記号");
                        match4 = rgx4.Match(password);
                        if (match4.Value.Length == 0)
                        {
                            errorF = true;
                        }
                    }

                    match5 = rgx5.Match(password);
                    if (match5.Value.Length != password.Length)
                    {
                        errorF = true;
                    }

                    if (errorF)
                    {
                        if (error.ToString().IndexOf("・") > 0)
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "パスワード", error.ToString() + "混在で入力してください"))); //{0}：
                        }
                        else
                        {
                            errors.Add((String.Empty, string.Format(Resources.Message.CE069, "パスワード", error.ToString() + "で入力してください"))); //{0}：
                        }
                        return errors;
                    }
                }
            }
            return errors;
        }
        public async Task<IEnumerable<(string field, string message)>> CreateUserAsync(CreateAccountViewModel user)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var errors = new List<(string field, string message)>();

                var appUser = user.Model;
                var ret = await userManager.CreateAsync(appUser, appUser.Password);
                if (!ret.Succeeded)
                {
                    foreach (var er in ret.Errors)
                    {
                        errors.Add((String.Empty, er));
                    }
                }

                appUser = await userManager.FindByNameAsync(user.AccountName);

                if (appUser != null)
                {
                    ret = await userManager.AddToRolesAsync(appUser.Id, user.Roles.ToArray());
                    if (!ret.Succeeded)
                    {
                        foreach (var er in ret.Errors)
                        {
                            errors.Add((String.Empty, er));
                        }
                    }
                }

                if (errors.Any())
                {
                    transaction.Rollback();
                }
                else
                {
                    transaction.Commit();
                }
                return errors;
            }
        }
    }
}