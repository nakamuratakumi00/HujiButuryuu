using Macss.Areas.Fdass.Models;
using Macss.Areas.Fdass.ViewModels;
using Macss.Models;
using Macss.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Fdass.Repositories
{
    public class MatujimeKanriRepository : IMatujimeKanriRepository
    {
        private readonly ApplicationDB dbContext;

        public MatujimeKanriRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<THokanMatujimeKanri>> FindByMonthAsync(string month)
        {
            return await dbContext.THokanMatujimeKanri.Where(x => x.Month == month).ToListAsync();
        }

        public async Task<MatujimeKanriViewModel> GetDispData(string yyyyMm)
        {

            var statusList = await dbContext.THokanMatujimeKanri
                .Join(dbContext.MAccount, x => x.Crtcod, x => x.Id, (h, a) => new { h, a })
                .Join(dbContext.MControl, x => x.h.Status, x => x.Kbn, (h, c) => new { h = h.h, a = h.a, c = c })
                .Where(x => x.h.Month == yyyyMm)
                .Where(x => x.c.Section == ControlRepository.MControlSection.MatujimeStatus)
                .Select(x => new MatujimeKanriViewModel()
                {
                    Month = x.h.Month,
                    Status = x.h.Status + ":" + x.c.Value1,
                    CrtName = x.a.UserName,
                    StartDateTime = (DateTime)x.h.Startt,
                    EndDateTime = (DateTime)x.h.Endt
                }).OrderByDescending(x => x.Month)
                .ToListAsync();

            if (statusList.Count() == 0)
            {
                return await GetDispData();
            }

            var statusData = statusList
                                .Select(x => new MatujimeKanriViewModel()
                                {
                                    Month = x.Month.Insert(4, "/"),
                                    Status = x.Status,
                                    CrtName = x.CrtName,
                                    StartTt = x.StartDateTime.ToString("yyyy/MM/dd HH:mm"),
                                    EndTt = x.EndDateTime.ToString("yyyy/MM/dd HH:mm")
                                })
                                .OrderByDescending(x => x.Month).FirstOrDefault();

            return statusData;

        }

        public async Task<MatujimeKanriViewModel> GetDispData()
        {

            List<MatujimeKanriViewModel> statusList = await dbContext.THokanMatujimeKanri
                    .Join(dbContext.MAccount, x => x.Crtcod, x => x.Id, (h, a) => new { h, a })
                    .Join(dbContext.MControl, x => x.h.Status, x => x.Kbn, (h, c) => new { h = h.h, a = h.a, c = c })
                    .Where(x => x.c.Section == ControlRepository.MControlSection.MatujimeStatus)
                    .Select(x => new MatujimeKanriViewModel()
                    {
                        Month = x.h.Month,
                        Status = x.h.Status + ":" + x.c.Value1,
                        CrtName = x.a.UserName,
                        StartDateTime = (DateTime)x.h.Startt,
                        EndDateTime = (DateTime)x.h.Endt
                    }).OrderByDescending(x => x.Month)
                    .ToListAsync();

            MatujimeKanriViewModel statusData = statusList
                                .Select(x => new MatujimeKanriViewModel()
                                {
                                    Month = x.Month.Insert(4, "/"),
                                    Status = x.Status,
                                    CrtName = x.CrtName,
                                    StartTt = x.StartDateTime.ToString("yyyy/MM/dd HH:mm"),
                                    EndTt = x.EndDateTime.ToString("yyyy/MM/dd HH:mm")
                                })
                                .OrderByDescending(x => x.Month).FirstOrDefault();

            return statusData;

        }

    }
}