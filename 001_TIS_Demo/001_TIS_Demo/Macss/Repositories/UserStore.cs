using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Macss.Models;

namespace Macss.Repositories
{
    public class UserStore :
        IUserStore<MAccount>,
        IUserStore<MAccount, string>,
        IUserPasswordStore<MAccount, string>
    {
        private ApplicationDB dbContext;

        internal UserStore(ApplicationDB dbContext)
        {
            this.dbContext = dbContext;
        }

        public static UserStore Create(IdentityFactoryOptions<UserStore> options, IOwinContext context)
        {
            return new UserStore(context.Get<ApplicationDB>());
        }

        /// <summary>
        /// ユーザーを作成する
        /// </summary>
        public async Task CreateAsync(MAccount user)
        {
            Debug.WriteLine(nameof(CreateAsync));
            dbContext.MAccount.Add(user);
            await dbContext.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// ユーザーを削除する
        /// </summary>
        public async Task DeleteAsync(MAccount user)
        {
            Debug.WriteLine(nameof(DeleteAsync));
            dbContext.MAccount.Remove(dbContext.MAccount.First(x => x.Id == user.Id));
            await dbContext.SaveChangesAsync();
            return;
        }

        /// <summary>
        /// 何か後始末（DbContextとかDBのコネクションとか作ってたら後始末をする）
        /// </summary>
        public void Dispose()
        {
            Debug.WriteLine(nameof(Dispose));
        }

        /// <summary>
        /// ユーザーをId指定で取得する
        /// </summary>
        public Task<MAccount> FindByIdAsync(string userId)
        {
            Debug.WriteLine(nameof(FindByIdAsync));
            return dbContext.MAccount.FirstOrDefaultAsync(x => x.Id == userId);
        }

        /// <summary>
        /// ユーザーをユーザー名で取得する（仕様により、Idから取得に変更）
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<MAccount> FindByNameAsync(string userId)
        {
            Debug.WriteLine(nameof(FindByNameAsync));
            return dbContext.MAccount.FirstOrDefaultAsync(x => x.Id == userId);
        }

#pragma warning disable 1998
        /// <summary>
        /// ユーザーからパスワードのハッシュを取得する
        /// </summary>
        public async Task<string> GetPasswordHashAsync(MAccount user)
        {
            Debug.WriteLine(nameof(GetPasswordHashAsync));
            return user.Password;
        }
#pragma warning restore 1998

        /// <summary>
        /// パスワードを持ってるか
        /// </summary>
        public Task<bool> HasPasswordAsync(MAccount user)
        {
            Debug.WriteLine(nameof(HasPasswordAsync));
            return Task.FromResult(user.Password != null);
        }

        /// <summary>
        /// ユーザーにハッシュ化されたパスワードを設定する
        /// </summary>
        public Task SetPasswordHashAsync(MAccount user, string passwordHash)
        {
            Debug.WriteLine(nameof(SetPasswordHashAsync));
            user.Password = passwordHash;
            user.AccountPasswordDate = DateTime.Now;
            return Task.FromResult(default(object));
        }

        /// <summary>
        /// ユーザー情報を更新する
        /// </summary>
        public async Task UpdateAsync(MAccount user)
        {
            Debug.WriteLine(nameof(UpdateAsync));
            var r = await dbContext.MAccount.FirstOrDefaultAsync(x => x.Id == user.Id);
            if (r == null) { return; }
            r.UserName = user.UserName;
            r.Password = user.Password;
            r.UpdateId = user.Id;
            r.UpdateDate = DateTime.Now;
            await dbContext.SaveChangesAsync();
            return;
        }
    }
}