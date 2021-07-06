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
    public class EstimatesRepository : IEstimatesRepository
    {
        #region 定数

        #endregion

        private readonly ApplicationDB dbContext;

        public EstimatesRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }
        public async Task <IEnumerable<EstimatesViewModel>> GetAllEstimatesAsync(string id)
        {
            return  await dbContext.MSeikyusaki
            .Join(dbContext.MHokanKeiyaku, x => x.Seicod, x => x.Fbtcod, (a, c1) => new { a, c1 })
            .Where(x => x.a.Seicod == id)
            .Select(x => new EstimatesViewModel() { Seinam = x.a.Seinam }).ToListAsync();
        }

        // 帳票判別処理
        public async Task<EstimatesCKViewModel> EstimatesCKViewModel(string id)
        {
            var statusList = await dbContext.MHokanKeiyaku
            .Where(x => x.Fbtcod == id)
            .Select(x => new EstimatesCKViewModel()
            {
                Fbtcod = x.Fbtcod,
                Hokflg1 = x.Hokflg1,
                Nieflg1 = x.Nieflg1
            })
            .ToListAsync();

            var statusData = statusList
                .Select(x => new EstimatesCKViewModel()
                {
                    Fbtcod = x.Fbtcod,
                    Hokflg1 = x.Hokflg1,
                    Nieflg1 = x.Nieflg1
                })
                .FirstOrDefault();

            return statusData;
        }

        #region 機種A
        //機種A
        public async Task<IEnumerable<KisyuAExcelViewModel>> KisyuAExcelViewModel(string id)
        {

            var status = await dbContext.MHokanKeiyaku
            .Where(x => x.Fbtcod == id && x.Seitai == "Y" && x.Hokflg2 == "A")
            .Select(x => new KisyuAExcelViewModel()
            {
                Fbtcod = x.Fbtcod,
                Kisyua = x.Kisyua,
                Kisnam = x.Kisnam == null ? string.Empty : x.Kisnam,
                Seinam = x.Seinam == null ? string.Empty : x.Seinam,
                HoksTani = x.Hokflg1 == "A" ? "機種A" : x.Hokflg1 == "C" ? "品番コード" : string.Empty,
                NiesTani = x.Nieflg1 == "A" ? "機種A" : x.Nieflg1 == "C" ? "品番コード" : string.Empty,
                HoktTani = x.Hokflg2 == "A" ? "機種A" : x.Hokflg2 == "C" ? "品番コード" : string.Empty,
                NietTani = x.Nieflg2 == "A" ? "機種A" : x.Nieflg2 == "C" ? "品番コード" : string.Empty,
                HoksTik = x.Hokflg3 == "1" ? "１期計算" : x.Hokflg3 == "2" ? "２期計算" : x.Hokflg3 == "3" ? "３期計算" : string.Empty,
                NiesTik = x.Nieflg3 == "N" ? "荷役用エリア" : x.Nieflg3 == "D" ? "伝票件数" : string.Empty,
                Hnebir = x.Hnebir == 0 ? null : x.Hnebir,
                Nnebir = x.Nnebir == 0 ? null : x.Nnebir,
                Hokant = x.Hokant,
                Nieant = x.Nieant ,
                Niekyt = x.Niekyt
            })
            .OrderBy(x => x.Kisyua)
            .ThenBy(x => x.Kisnam)
            .ThenBy(x => x.Seinam)
            .ToListAsync();

            //Excel出力用にデータ変換
            List<KisyuAExcelViewModel> result = new List<KisyuAExcelViewModel>();
            foreach(var sdata in status)
            {
                //保管行
                var hokdata = new KisyuAExcelViewModel();
                hokdata.Fbtcod = sdata.Fbtcod;
                hokdata.Kisyua = string.Empty;
                hokdata.Kisnam = string.Empty;
                hokdata.Seitai = sdata.Seitai;
                hokdata.Seinam = sdata.Seinam.Length >= 20 ? sdata.Seinam.Substring(0, 20) : sdata.Seinam;
                hokdata.Seiktani = sdata.HoksTani;
                hokdata.Tanktani = sdata.HoktTani;
                hokdata.Seiktaik = sdata.HoksTik;
                hokdata.nebk = sdata.Hnebir;
                hokdata.Tanka = sdata.Hokant;
                hokdata.Niekyt = null;
                result.Add(hokdata);

                //荷役行
                var niedata = new KisyuAExcelViewModel();
                niedata.Fbtcod = sdata.Fbtcod;
                niedata.Kisyua = sdata.Kisyua;
                niedata.Kisnam = sdata.Kisnam;
                niedata.Seitai = sdata.Seitai;
                niedata.Seinam = sdata.Seinam.Length >= 20 ? sdata.Seinam.Substring(20) : string.Empty;
                niedata.Seiktani = sdata.NiesTani;
                niedata.Tanktani = sdata.NietTani;
                niedata.Seiktaik = sdata.NiesTik;
                niedata.nebk = sdata.Nnebir;
                niedata.Tanka = sdata.Nieant;
                niedata.Niekyt = sdata.Niekyt;
                result.Add(niedata);
            }
            return result;
        }
        #endregion

        #region 品番コード
        //品番コード
        public async Task<IEnumerable<HinCodExcelViewModel>> HinCodExcelViewModel(string id)

        {
            var statusList = await dbContext.MHokanKeiyaku
            .Join(dbContext.MHokanTanka, x => x.Kisyua, x => x.Kisyua, (a, c1) => new { a, c1 })
            .Join(dbContext.MSeikyusaki, x => x.a.Fbtcod, x => x.Seicod, (a, c2) => new { a = a.a, c1 = a.c1, c2 })
            .Join(dbContext.MShukkabasho, x => x.c1.Sybcod, x => x.Sybcod, (a, c2) => new { a = a.a, c1 = a.c1, c2 })
            .Where(x => x.a.Fbtcod == id && x.a.Seitai == "Y" && x.a.Hokflg2 == "C")
            .Select(x => new HinCodExcelViewModel()
            {
                Kisyua = x.a.Kisyua,
                Kisnam = x.a.Kisnam == null ? string.Empty : x.a.Kisnam,
                Seinam = x.a.Seinam == null ? string.Empty : x.a.Seinam,
                Hokflg1 = x.a.Hokflg1 == "A" ? "機種A" : x.a.Hokflg1 == "C" ? "品番コード" : string.Empty,
                Nieflg1 = x.a.Nieflg1 == "A" ? "機種A" : x.a.Nieflg1 == "C" ? "品番コード" : string.Empty,
                Hokflg2 = x.a.Hokflg2 == "A" ? "機種A" : x.a.Hokflg2 == "C" ? "品番コード" : string.Empty,
                Nieflg2 = x.a.Nieflg2 == "A" ? "機種A" : x.a.Nieflg2 == "C" ? "品番コード" : string.Empty,
                Hokflg3 = x.a.Hokflg3 == "1" ? "１期計算" : x.a.Hokflg3 == "2" ? "2期計算" : x.a.Hokflg3 == "3" ? "3期計算" : string.Empty,
                //Nieflg3 = x.a.Nieflg3 == "N" ? "荷役用エリア" : x.a.Nieflg3 == "D" ? "入出庫数" : string.Empty,
                Nieflg3 = x.a.Nieflg3 == "N" ? "入出庫数" : x.a.Nieflg3 == "D" ? "伝票件数" : string.Empty,
                Hnebir = x.a.Hnebir == null ? '0' : x.a.Hnebir,
                Nnebir = x.a.Nnebir == null ? '0' : x.a.Nnebir,
                Nieant = x.a.Nieant == null ? '0' : x.a.Nieant,
                Ojyukr = x.a.Ojyukr == null ? '0' : x.a.Ojyukr,
                Hjyukr = x.a.Hjyukr == null ? '0' : x.a.Hjyukr,
                Osyjyr = x.a.Osyjyr == null ? '0' : x.a.Osyjyr,
                Hsyjyr = x.a.Osyjyr == null ? '0' : x.a.Hsyjyr,
                //明細
                Hincod = x.c1.Hincod == null ? string.Empty : x.c1.Hincod, 
                Hinnmk = x.c1.Hinnmk == null ? string.Empty: x.c1.Hinnmk,
                Kisyub = x.c1.Kisyub == null ? string.Empty : x.c1.Kisyub,
                Sybcod = x.c1.Sybcod == null ? string.Empty : x.c1.Sybcod,
                Sybnam = x.c2.Sybnam == null ? string.Empty : x.c2.Sybnam,
                Frikae = x.c1.Frikae == null ? string.Empty : x.c1.Frikae,
                Fptank = x.c1.Fptank == null ? '0' : x.c1.Fptank,
                Niekit = x.c1.Niekit == null ? '0' : x.c1.Niekit,
                Hokant = x.c1.Hokant == null ? '0' : x.c1.Hokant
            })
            .OrderBy(x => x.Kisyua)
            .ThenBy(x => x.Kisyub)
            .ThenBy(x => x.Hincod)
            .ThenBy(x => x.Hinnmk)
            .ToListAsync();

                var statusData = statusList
                .Select(x => new HinCodExcelViewModel()
                {
                    Kisyua = x.Kisyua,
                    Kisnam = x.Kisnam,
                    Seinam = x.Seinam,
                    Hokflg1 = x.Hokflg1,
                    Nieflg1 = x.Nieflg1,
                    Hokflg2 = x.Hokflg2,
                    Nieflg2 = x.Nieflg2,
                    Hokflg3 = x.Hokflg3,
                    Nieflg3 = x.Nieflg3,
                    Hnebir = x.Hnebir,
                    Nnebir = x.Nnebir,
                    Hokant = x.Hokant,
                    Nieant = x.Nieant,
                    Ojyukr = x.Ojyukr,
                    Hjyukr = x.Hjyukr,
                    Osyjyr = x.Osyjyr,
                    Hsyjyr = x.Hsyjyr,
                    Hincod = x.Hincod,
                    Hinnmk = x.Hinnmk,
                    Kisyub = x.Kisyub,
                    Sybcod = x.Sybcod,
                    Sybnam = x.Sybnam,
                    Frikae = x.Frikae,
                    Fptank = x.Fptank,
                    Niekit = x.Niekit
                })
             .ToList();

            return statusData;
        }
        #endregion
}
    }