using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.App_Start;
using Macss.Models;
using Macss.ViewModels;
using System.Data.Entity;

namespace Macss.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDB dbContext;

        public GroupRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<GroupViewModel>> GetAllGroupAsync()
        {
            return await dbContext.MGroup.Select(x => new GroupViewModel() { GroupCd = x.GroupCd, GroupName = x.GroupName, UpperClassCd = x.UpperClassCd }).OrderBy(x => x.GroupCd).ToListAsync();
        }
        public async Task<IEnumerable<MGroup>> GetSelectGroupAsync(string groupCd)
        {
            return await dbContext.MGroup.Where(x => x.GroupCd == groupCd).ToListAsync();
        }
        public async Task<IEnumerable<MGroup>> GetSelectUpperGroupAsync(string upperGroupCd)
        {
            return await dbContext.MGroup.Where(x => x.UpperClassCd == upperGroupCd).ToListAsync();
        }
        public async Task UpdateGroupAsync(MGroup appGroup)
        {
            await dbContext.SaveChangesAsync();
            return;
        }
        public async Task CreateGroupAsync(MGroup appGroup)
        {
            dbContext.MGroup.Add(appGroup);
            await dbContext.SaveChangesAsync();
            return;
        }
        public async Task<string> DeleteGroupAsync(string cd)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                var appGroup = await dbContext.MGroup.Where(x => x.GroupCd == cd).SingleOrDefaultAsync();
                var appAccount = await dbContext.MAccount.Where(x => x.GroupCd == cd).ToListAsync();
                var appLowerGroup = await dbContext.MGroup.Where(x => x.UpperClassCd == cd).ToListAsync();

                //対象のグループがDBにあるかチェック
                if (appGroup == null)
                {
                    return String.Format(Resources.Message.CE019);
                }

                //対象のグループに紐付くアカウントがあるかチェック
                if (appAccount.Count != 0)
                {
                    return String.Format(Resources.Message.CE050, "対象グループ", "アカウント");
                }

                //対象のグループに紐付く配下グループがあるかチェック
                if (appLowerGroup.Count != 0)
                {
                    return String.Format(Resources.Message.CE050, "対象グループの配下", "グループ");
                }

                //グループの削除
                dbContext.MGroup.Remove(appGroup);
                await dbContext.SaveChangesAsync();
                transaction.Commit();
            }
            return String.Empty;
        }
    }
}