using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.Repositories;
using Macss.Areas.Fdass.ViewModels;

namespace Macss.Areas.Fdass.Repositories
{
    public class SeihinResultRepository : ISeihinResultRepository
    {


        private readonly ApplicationDB dbContext;

        public SeihinResultRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<SeihinResultVierModel> GetDispData()
        {

            var statusList = await dbContext.THokanMatujimeKanri
                .Join(dbContext.MAccount, x => x.Crtcod, x => x.Id, (h, a) => new { h, a })
                .Join(dbContext.MControl, x => x.h.Status, x => x.Kbn, (h, c) => new { h = h.h, a = h.a, c = c })
                .Where(x => x.c.Section == ControlRepository.MControlSection.MatujimeStatus)
                .Select(x => new SeihinResultVierModel()
                {
                    Month = x.h.Month,
                    Status = x.h.Status + ":" + x.c.Value1,
                    CrtName = x.a.UserName,
                    StartDateTime = (DateTime)x.h.Startt
                })

                .ToListAsync();

            var statusData = statusList
                                .Select(x => new SeihinResultVierModel()
                                {
                                    Month = x.Month.Insert(4, "/"),
                                    Status = x.Status,
                                    CrtName = x.CrtName,
                                    StartTt = x.StartDateTime.ToString("yyyy/MM/dd HH:ss"),

                                })
                                .OrderBy(x => x.Month).FirstOrDefault();
            return statusData;

        }

        public async Task<IEnumerable<TankaAutoSetList>> TankaAutoSetList(string dtfrom, string dtto)
        {
            DateTime dtFrom;
            if (!DateTime.TryParse(dtfrom, out dtFrom))
            {                 
                dtFrom = DateTime.MinValue;
            }
            string dtFromS = dtFrom.ToString("yyyy/MM/dd") + " 00:00:00";
            dtFrom = DateTime.Parse(dtFromS);

            DateTime dtTo;
            if (!DateTime.TryParse(dtto, out dtTo))
            {
                dtTo = DateTime.MaxValue;
            }
            string dtToS = dtTo.ToString("yyyy/MM/dd") + " 23:59:59";
            dtTo = DateTime.Parse(dtToS);

            //DateTime dtFrom = dtfrom == null ? DateTime.MaxValue :  DateTime.Parse(dtfrom);
            //DateTime dtTo = dtto == null ? DateTime.MinValue : DateTime.Parse(dtto);

            //対象全て
            var allData = await dbContext.MHokanTanka
                .Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (a, c1) => new { a, c1 })
                .Join(dbContext.MShukkabasho, x => x.a.Sybcod, x => x.Sybcod, (a, c2) => new { a = a.a, c1 = a.c1, c2 })
                .Where(x =>  (x.a.Updymd >= dtFrom) && (x.a.Updymd <= dtTo))

                .Select(x => new TankaAutoSetList()
                {
                    Tanbsy = x.c1.Tanbsy,
                    Syori = string.Empty,
                    Hincod = x.a.Hincod,
                    Sybcod = x.a.Sybcod,
                    Sybnam = x.c2.Sybnam,
                    Kisyua = x.a.Kisyua,
                    Kisyub = x.a.Kisyub,
                    Hinnmk = x.a.Hinnmk,
                    Frikae = x.a.Frikae,
                    Fptank = x.a.Fptank,
                    Hokant = x.a.Hokant,
                    OSybcod = x.a.Osybcod,
                    OHinnmk = x.a.Ohinnmk,
                    OKisyua = x.a.Okisyua,
                    OKisyub = x.a.Okisyub,
                    OFrikae = x.a.Ofrikae,
                    OFptank = x.a.Ofptnk,
                    OHokant = x.a.Ohokat,
                    Crtymd = x.a.Crtymd,
                    Updymd = x.a.Updymd
                })
                .OrderBy(x => x.Hincod)
                .ThenBy(x => x.Sybcod)
                .ThenBy(x => x.Kisyua)
                .ThenBy(x => x.Kisyub)
                .ThenBy(x => x.Hinnmk)
                .ThenBy(x => x.Frikae)
                .ThenBy(x => x.Updymd)
                .ToListAsync();

            List<TankaAutoSetList> resultData = new List<TankaAutoSetList>();
            foreach (TankaAutoSetList status in allData)
            {
                //新規
                if (status.Crtymd == status.Updymd)
                {
                    TankaAutoSetList work = new TankaAutoSetList();
                    work.Tanbsy = status.Tanbsy;
                    work.Syori = "新　規";
                    work.Hincod = status.Hincod;
                    work.Sybcod = status.Sybcod;
                    work.Sybnam = status.Sybnam;
                    work.Kisyua = status.Kisyua;
                    work.Kisyub = status.Kisyub;
                    work.Hinnmk = status.Hinnmk;
                    work.Frikae = status.Frikae;
                    work.Fptank = status.Fptank;
                    work.Hokant = status.Hokant;
                    work.Crtymd = status.Crtymd;
                    work.Updymd = status.Updymd;
                    resultData.Add(work);
                }
                //更新
                if (status.Crtymd != status.Updymd)
                {
                    TankaAutoSetList work1 = new TankaAutoSetList();
                    work1.Syori = "修正前";
                    work1.Tanbsy = status.Tanbsy;
                    work1.Hincod = status.Hincod;
                    work1.Sybcod = status.OSybcod;
                    work1.Sybnam = status.Sybnam;
                    work1.Kisyua = status.OKisyua;
                    work1.Kisyub = status.OKisyub;
                    work1.Hinnmk = status.OHinnmk;
                    work1.Frikae = status.OFrikae;
                    work1.Fptank = status.OFptank;
                    work1.Hokant = status.OHokant;
                    work1.Crtymd = status.Crtymd;
                    work1.Updymd = status.Updymd;
                    resultData.Add(work1);

                    TankaAutoSetList work2 = new TankaAutoSetList();
                    work2.Syori = "修正後";
                    work2.Tanbsy = status.Tanbsy;
                    work2.Hincod = status.Hincod;
                    work2.Sybcod = status.Sybcod;
                    work2.Sybnam = status.Sybnam;
                    work2.Kisyua = status.Kisyua;
                    work2.Kisyub = status.Kisyub;
                    work2.Hinnmk = status.Hinnmk;
                    work2.Frikae = status.Frikae;
                    work2.Fptank = status.Fptank;
                    work2.Hokant = status.Hokant;
                    work2.Crtymd = status.Crtymd;
                    work2.Updymd = status.Updymd;
                    resultData.Add(work2);
                }                
            }
            resultData.OrderBy(a => a.Hincod)
               .ThenBy(a => a.Sybcod)
               .ThenBy(a => a.Kisyua)
               .ThenBy(a => a.Kisyub)
               .ThenBy(a => a.Hinnmk)
               .ThenBy(a => a.Frikae)
               .ThenBy(a => a.Updymd);

            return resultData;
        }

    }

}