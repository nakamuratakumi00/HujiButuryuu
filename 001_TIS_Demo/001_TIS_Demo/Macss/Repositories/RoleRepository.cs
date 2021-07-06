using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDB dbContext;

        public RoleRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRolesAsync()
        {
            return await dbContext.MRole.Select(x => new RoleViewModel() { Id = x.RoleId, Name = x.RoleName, RoleCmt =x.RoleCmt }).ToListAsync();
        }

        public async Task<IEnumerable<RoleInformation>> GetAllRolesInfoAsync()
        {
            return await dbContext.MRole.Select(x => new RoleInformation() { Id = x.RoleId, Name = x.RoleName, RoleCmt = x.RoleCmt }).ToListAsync();
        }

        public async Task<IEnumerable<MRole>> GetSelectRolesAsync(string roleId)
        {
            return await dbContext.MRole.Where(x => x.RoleId == roleId).ToListAsync();
        }
        public async Task<IEnumerable<(string field, string message)>> UpdateRoleAsync(string id, RoleViewModel role, string loginUser)
        {
            var errors = new List<(string field, string message)>();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var appRole = await dbContext.MRole.FirstOrDefaultAsync(x => x.RoleId == id);
                //Microsoft.AspNet.Identity.IdentityResult ret = new Microsoft.AspNet.Identity.IdentityResult();

                // 新規の場合
                if (role.Mode == 0)
                {
                    // ID重複確認
                    if (appRole != null)
                    {
                        errors.Add((String.Empty, String.Format(Resources.Message.CE057, "ロールID")));
                        return errors;
                    }
                    appRole = role.Model;
                    appRole.CreateId = loginUser;
                    appRole.CreateDate = DateTime.Now;
                    appRole.UpdateId = loginUser;
                    appRole.UpdateDate = DateTime.Now;
                    dbContext.MRole.Add(appRole);
                    await dbContext.SaveChangesAsyncEx();
                }
                // 更新の場合
                else
                {
                    appRole.RoleName = role.Name;
                    appRole.RoleCmt = role.RoleCmt;
                    appRole.UpdateId = loginUser;
                    appRole.UpdateDate = DateTime.Now;
                    await dbContext.SaveChangesAsyncEx();
                }

                // メニューロールマスタ更新
                var except = appRole.MMenuRole.Where(x => !role.SetMenu.Contains(x.MenuId)).Select(x => x.MenuId).ToArray();
                var expectData = dbContext.MMenuRole.Where(x => x.RoleId == id).Where(x => except.Contains(x.MenuId));
                dbContext.MMenuRole.RemoveRange(expectData);
                await dbContext.SaveChangesAsync();

                var addMenuId = role.SetMenu.Except(appRole.MMenuRole.Select(x => x.MenuId)).ToArray();
                List<MMenuRole> addList = new List<MMenuRole>();
                for (int i = 0; i < addMenuId.Length; i++)
                {
                    var add = new MMenuRole();
                    add.MenuId = addMenuId[i];
                    add.RoleId = id;
                    add.CreateId = loginUser;
                    add.UpdateId = loginUser;
                    add.CreateDate = DateTime.Now;
                    add.UpdateDate = DateTime.Now;
                    addList.Add(add);
                }
                dbContext.MMenuRole.AddRange(addList);
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
        
        public async Task<List<string>> GetUserRoleAsync(string userId)
        {
            return await dbContext.MAccountRole.Where(x => x.AccountId == userId).Select(x => x.RoleId).ToListAsync();
        }

        public async Task<string> DeleteRolesAsync(string roleId)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var appRole = await dbContext.MRole.Where(x => x.RoleId == roleId).SingleOrDefaultAsync();
                var appAccount = await dbContext.MAccountRole.Where(x => x.RoleId == roleId).ToListAsync();
                //var appMRole = await dbContext.MMenuRole.Where(x => x.RoleId == roleId).SingleOrDefaultAsync();
                var appMRoles = await dbContext.MMenuRole.Where(x => x.RoleId == roleId).ToArrayAsync();

                //対象のグループがDBにあるかチェック
                if (appRole == null)
                {
                    return String.Format(Resources.Message.CE019);
                }

                //ロールに紐付くアカウントがあるかチェック
                if (appAccount.Count != 0)
                {
                    return String.Format(Resources.Message.CE050, "対象のロール", "アカウント");
                }

                //メニューロールの削除
                //dbContext.MMenuRole.Remove(appMRole);
                foreach (var appMRole in appMRoles)
                {
                    dbContext.MMenuRole.Remove(appMRole);
                }
                await dbContext.SaveChangesAsync();

                //ロールの削除
                dbContext.MRole.Remove(appRole);
                await dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return String.Empty;
        }
    }
}