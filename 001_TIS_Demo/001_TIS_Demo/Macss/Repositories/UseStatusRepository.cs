using Macss.Models;
using Macss.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Macss.Repositories
{
    public class UseStatusRepository : IUseStatusRepository
    {

        private readonly ApplicationDB dbContext;

        public UseStatusRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<TUseStatus>> GetSelectUseStatusAsync(string SessionId, string AccountId)
        {
            return await dbContext.TUseStatus.Where(x => x.SessionId == SessionId && x.AccountId == AccountId).ToListAsync();
        }

        public async Task<bool> UpdateUseStatusAsync(string SessionId, string AccountId, string TitleName, string ActionName, string ControllerName)
        {

            DateTime oldDate = DateTime.Now.AddHours(-1);
            var oldUseStatus = await dbContext.TUseStatus.Where(x => x.UpdateDate <= oldDate).ToListAsync();
            foreach (var use in oldUseStatus)
            {
                dbContext.TUseStatus.Remove(use);
                await dbContext.SaveChangesAsync();
            }

            var useStatus = await dbContext.TUseStatus.Where(x => x.SessionId == SessionId && x.AccountId == AccountId).ToListAsync();

            // 新規の場合
            if (useStatus.Count == 0)
            {
                var newUseStatus = new TUseStatus();

                newUseStatus.SessionId = SessionId;
                newUseStatus.AccountId = AccountId;
                newUseStatus.TitleName = TitleName;
                newUseStatus.ActionName = ActionName;
                newUseStatus.ControllerName = ControllerName;
                newUseStatus.UpdateDate = DateTime.Now;
                dbContext.TUseStatus.Add(newUseStatus);
                await dbContext.SaveChangesAsyncEx();
            }
            // 更新の場合
            else
            {
                var updUseStatus = useStatus.First();
                updUseStatus.TitleName = TitleName;
                updUseStatus.ActionName = ActionName;
                updUseStatus.ControllerName = ControllerName;
                updUseStatus.UpdateDate = DateTime.Now;
                await dbContext.SaveChangesAsyncEx();
            }

            return true;
        }

        public async Task<bool> DeleteUseStatusAsync(string SessionId, string AccountId)
        {
            if (!string.IsNullOrEmpty(AccountId))
            {
                var useStatus = await dbContext.TUseStatus.Where(x => x.SessionId == SessionId && x.AccountId == AccountId).ToListAsync();
                foreach (var use in useStatus)
                {
                    dbContext.TUseStatus.Remove(use);
                    await dbContext.SaveChangesAsync();
                }
            } else
            {
                var useStatus = await dbContext.TUseStatus.Where(x => x.SessionId == SessionId).ToListAsync();
                foreach (var use in useStatus)
                {
                    dbContext.TUseStatus.Remove(use);
                    await dbContext.SaveChangesAsync();
                }
            }

            DateTime oldDate = DateTime.Now.AddHours(-1);
            var oldUseStatus = await dbContext.TUseStatus.Where(x => x.UpdateDate <= oldDate).ToListAsync();
            foreach (var use in oldUseStatus)
            {
                dbContext.TUseStatus.Remove(use);
                await dbContext.SaveChangesAsync();
            }

            return true;
        }

        private async Task<bool> DeleteUseStatusAsync()
        {
            DateTime oldDate = DateTime.Now.AddHours(-1);
            var useStatus = await dbContext.TUseStatus.Where(x => x.UpdateDate <= oldDate).ToListAsync();
            foreach(var use in useStatus)
            {
                dbContext.TUseStatus.Remove(use);
                await dbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<IEnumerable<UseStatusViewModel>> GetAllUseStatusAsync()
        {

            var useStatusList = await dbContext.TUseStatus
                            .GroupJoin(dbContext.MAccount, x => x.AccountId, x => x.Id, (u, a) => new { u, a })
                            .Join(dbContext.MControl, x => x.a.FirstOrDefault().BumonCd, x => x.Kbn, (a, c1) => new { u = a.u, a = a.a, c1 })
                            .Where(x => x.c1.Section == ControlRepository.MControlSection.AccountBumon)
                            .Select(x => new UseStatusViewModel()
                            {
                                AccountId = x.u.AccountId,
                                AccountName = x.a.FirstOrDefault().UserName,
                                BumonCd = x.c1.Value1,
                                TitleName = x.u.TitleName,
                                UpdateDateTime = x.u.UpdateDate

                            }).ToListAsync();

            var useStatusData = useStatusList
                    .Select(x => new UseStatusViewModel()
                    {
                        AccountId = x.AccountId,
                        AccountName = x.AccountName,
                        BumonCd = x.BumonCd,
                        TitleName = x.TitleName,
                        UpdateDate = x.UpdateDateTime.ToString("yyyy/MM/dd HH:mm:ss")
                    })
                    .OrderByDescending(x => x.UpdateDate)
                    .ToList();

            return useStatusData;
        }

    }
}