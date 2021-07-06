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
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDB dbContext;

        public LogRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<LogViewModel>> GetLogByConditionAsync(LogViewModel logModel)
        {
            var excTo = logModel.ExcectionTo;
            var excFrom = logModel.ExcectionFrom;

            Func <TLogHistory, bool> prcssFilter;
            Func<TLogHistory, bool> dateFilter;
            Func<TLogHistory, bool> userFilter;
            Func<TLogHistory, bool> fncFilter;

            if (!String.IsNullOrEmpty(logModel.ProcessingId))
            {
                prcssFilter = (x) =>
                {
                    return x.ProcessingId == logModel.ProcessingId;
                };
            }
            else
            {
                if (logModel.ProcessingIdList.Count() > 0)
                {
                    prcssFilter = (x) =>
                    {
                        return logModel.ProcessingIdList.Contains(x.ProcessingId);
                    };
                }
                else
                {
                    prcssFilter = (x) => { return true; };
                }
            }

            if (!String.IsNullOrEmpty(excFrom) && !String.IsNullOrEmpty(excTo))
            {
                dateFilter = (x) =>
                {
                    var res = x.ExcectionDate.ToString().Substring(0, 10);
                    return (res.CompareTo(excFrom) >= 0) && (res.CompareTo(excTo) <= 0);
                };
            }
            else if (!String.IsNullOrEmpty(excFrom))
            {
                dateFilter = (x) =>
                {
                    var res = x.ExcectionDate.ToString().Substring(0, 10);
                    return res.CompareTo(excFrom) >= 0;
                };
            }
            else if (!String.IsNullOrEmpty(excTo))
            {
                dateFilter = (x) =>
                {
                    var res = x.ExcectionDate.ToString().Substring(0, 10);
                    return res.CompareTo(excTo) <= 0;
                };
            }
            else
            {
                dateFilter = (x) => { return true; };
            }
            if (!String.IsNullOrEmpty(logModel.UserId))
            {
                userFilter = (x) =>
                {
                    return x.UserId == logModel.UserId;
                };
            }
            else
            {
                userFilter = (x) => { return true; };
            }
            if (!String.IsNullOrEmpty(logModel.Function))
            {
                fncFilter = (x) =>
                {
                    return x.Function == logModel.Function;
                };
            }
            else
            {
                fncFilter = (x) => { return true; };
            }

            var log = dbContext.TLogHistory.Where(prcssFilter).Where(dateFilter).Where(userFilter).Where(fncFilter).ToArray();
            var control = await dbContext.MControl.Where(x => x.Section == ControlRepository.MControlSection.LogFunction).ToArrayAsync();
            //IQueryable<TLogHistory> queryableLog = log.AsQueryable();

            //var logData = await queryableLog
            var logData = log
                .Join(dbContext.MMenu, x => x.ProcessingId, x => x.MenuId, (l, m) => new { l, m })
                .Join(dbContext.MAccount, x => x.l.UserId, x => x.Id, (l, a) => new { l.l, l.m, a })
                .Join(control, x => x.l.Function, x => x.Kbn, (l, c) => new { l.l, l.m, l.a, c })
                .Join(dbContext.MMenu, x => x.l.ProcessingId.Substring(0, 1), x => x.MenuId.Substring(0, 1), (l, m2) => new { l.l, l.m, l.a, l.c, m2 })
                .Where(x => x.m2.ParentId == "MAIN")

                .Select(x => new LogViewModel()
                {
                    ExcectionDate = x.l.ExcectionDate.ToString(),
                    UserId = x.l.UserId,
                    UserName = x.a.UserName,
                    //MenuName = x.m.MenuName,
                    MenuName = x.m2.MenuName,
                    RoleName = x.m.RoleName,
                    FunctionName = x.c.Value1,
                    Purpose1 = x.l.Purpose1,
                    Purpose2 = x.l.Purpose2
                })
                //.ToListAsync();
                .ToArray();
            
            return logData;
        }

        public void CreateLogHisory(TLogHistory logFunc)
        {
            dbContext.TLogHistory.Add(logFunc);
            dbContext.SaveChanges();
        }
    }
}
