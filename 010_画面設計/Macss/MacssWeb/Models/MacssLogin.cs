using Macss.Database.Entity;
using MacssWeb.Models.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MacssWeb.Models
{

    /// <summary>
    /// 認証用にAccountMasterを継承したモデルクラス
    /// </summary>
    public class MacssLoginAccount : IUser<string>
    {
        public MacssLoginAccount() { }

        public MacssLoginAccount(AccountMaster accont)
        {
            this.Account = accont;
        }

        public AccountMaster Account { get; private set; }

        public string Id
        {
            get
            {
                if (this.Account == null)
                {
                    return null;
                }
                return this.Account.account_id;
            }
        }

        public string UserName
        {
            get
            {
                if (this.Account == null)
                {
                    return null;
                }
                return this.Account.account_name;
            }
            set
            {
                if (this.Account == null)
                {
                    return;
                }
                this.Account.account_name = value;
            }
        }

        public string PasswordHash
        {
            get
            {
                if (this.Account == null)
                {
                    return null;
                }
                return this.Account.account_password;
            }
        }

        public bool HasAccount
        {
            get
            {
                return (this.Account != null && !String.IsNullOrWhiteSpace(this.Id));
            }
        }
    }

    public class MacssUserEntity
    {
        public MacssUserEntity(DbContext context)
        {
            this.DbContext = context;
            this.DbEntitySet = context.Set<AccountMaster>();
        }

        private DbContext DbContext { get; set; }
        public DbSet<AccountMaster> DbEntitySet { get; private set; }

        public async Task<MacssLoginAccount> GetByIdAsync(string id)
        {
            var ret = await DbEntitySet.FindAsync(id);
            return new MacssLoginAccount(ret);
        }

        public async Task<MacssLoginAccount> FirstOrDefaultAsync(Expression<Func<AccountMaster, bool>> filter)
        {
            var ret = await DbEntitySet.FirstOrDefaultAsync(filter);
            return new MacssLoginAccount(ret);
        }
    }

    public class MacssUserStore
        : IUserStore<MacssLoginAccount>, IUserStore<MacssLoginAccount, string>, IUserPasswordStore<MacssLoginAccount, string>
    {
        private MacssUserStore(MacssWebAccountDbContext context)
        {
            this.DbContext = context;
            this.UserEntity = new MacssUserEntity(this.DbContext);
        }

        public static MacssUserStore Create(IdentityFactoryOptions<MacssUserStore> options, IOwinContext context)
        {
            return new MacssUserStore(context.Get<MacssWebAccountDbContext>());
        }

        private MacssWebAccountDbContext DbContext { get; set; }

        private MacssUserEntity UserEntity { get; set; }

        #region IUserStore実装

        public void Dispose()
        {
        }

        public Task<MacssLoginAccount> FindByIdAsync(string userId)
        {
            return this.UserEntity.GetByIdAsync(userId);
        }

        public Task<MacssLoginAccount> FindByNameAsync(string userName)
        {
            return this.UserEntity.FirstOrDefaultAsync(x => x.account_id == userName);
        }

        public Task CreateAsync(MacssLoginAccount user)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(MacssLoginAccount user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(MacssLoginAccount user)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region IUserPasswordStore実装

        public Task SetPasswordHashAsync(MacssLoginAccount user, string passwordHash)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(MacssLoginAccount user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(MacssLoginAccount user)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

}