using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.Repositories;
using Macss.Areas.Fdass.ViewModels;
using Macss.Areas.Fdass.Common;
using Macss.Areas.Fdass.Models;

namespace Macss.Areas.Fdass.Repositories
{
    public class PrintRepository : IPrintRepository
    {
        #region 定数

        #endregion

        private readonly ApplicationDB dbContext;

        public PrintRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<PrintViewModel> GetDispData()
        {

            var statusList = await dbContext.THokanMatujimeKanri
                .Join(dbContext.MAccount, x => x.Crtcod, x => x.Id, (h, a) => new { h, a })
                .Join(dbContext.MControl, x => x.h.Status, x => x.Kbn, (h, c) => new { h = h.h, a = h.a, c = c })
                .Where(x => x.c.Section == ControlRepository.MControlSection.MatujimeStatus)
                .Select(x => new PrintViewModel()
                {
                    Month = x.h.Month,
                    Status = x.h.Status + ":" + x.c.Value1,
                    CrtName = x.a.UserName,
                    StartDateTime = (DateTime)x.h.Startt
                })

                .ToListAsync();

            var statusData = statusList
                                .Select(x => new PrintViewModel()
                                {
                                    Month = x.Month.Insert(4, "/"),
                                    Status = x.Status,
                                    CrtName = x.CrtName,
                                    StartTt = x.StartDateTime.ToString("yyyy/MM/dd HH:ss"),

                                })
                                .OrderByDescending(x => x.Month).FirstOrDefault();

            return statusData;

        }

        //Fe保管請求PCコードデータ作成エラーリスト
        public async Task<IEnumerable<PcCodeErrorListPrint>> PcCodeErrorListPrint()
        {

            // PC設定エラー
            List<PcCodeErrorListPrint> errorList = new List<PcCodeErrorListPrint>();
            await SetpcErrorAsync(errorList, true);
            await SetpcErrorAsync(errorList, false);
            return errorList;
        }

        private async Task SetpcErrorAsync(List<PcCodeErrorListPrint> errorList, bool processing)
        {

            string pcError = processing == true ? "ＰＣ設定エラー（部門）": "ＰＣ設定エラー（顧客）";

            var seicodList = await dbContext.THokanSeikyu
                .GroupBy(x => new { x.Seicod, x.Pccod, x.Hokcod })
                .Select(x => new
                {
                    Seicod = x.Key.Seicod,
                    Hokcod = x.Key.Hokcod,
                    Pccod = x.Key.Pccod,
                })
                .Where(x => x.Hokcod != "P1" && x.Hokcod != "P2" && x.Hokcod != "NE")
                .Where(x => processing == true ? x.Pccod.StartsWith("8027"): x.Pccod.Substring(8, 1) == string.Empty)
                .OrderBy(x => x.Seicod)
                .ToListAsync();

            var seicodList2 = seicodList.GroupBy(x => new { x.Seicod })
                .Select(x => new
                {
                    x.Key.Seicod
                });

            foreach (var seicod in seicodList2)
            {
                var pcCodeList = await dbContext.THokanSeikyu
                            .Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (x, k) => new { x = x, k })
                            .GroupBy(x => new { x.x.Seicod, x.x.Pccod, x.x.Kisyua, x.x.Kisyub, x.x.Hokcod, x.k.Hokflg4 })
                            .Select(x => new
                            {
                                Kisyua = x.Key.Kisyua,
                                Kisyub = x.Key.Kisyub,
                                Seicod = x.Key.Seicod,
                                Hokcod = x.Key.Hokcod,
                                Pccod = x.Key.Pccod,
                                Hokflg4 = x.Key.Hokflg4,
                                Hokankr = x.Sum(y => y.x.Hokankr),
                                Niekikr = x.Sum(y => y.x.Niekikr)
                            })
                            .Where(x => x.Hokcod != "P1" && x.Hokcod != "P2")
                            .Where(x => x.Kisyub != "999")
                            .Where(x => x.Seicod == seicod.Seicod)
                        .OrderBy(x => x.Seicod)
                        .OrderBy(x => x.Kisyua)
                        .ToListAsync();

                #region 保管料
                var pcCodeListFH = pcCodeList.GroupBy(x => new { x.Seicod, x.Pccod, x.Kisyua, x.Hokcod, x.Hokflg4 })
                    .Select(x => new
                    {
                        Seicod = x.Key.Seicod,
                        Pccod = x.Key.Pccod,
                        Kisyua = x.Key.Kisyua,
                        Hokcod = x.Key.Hokcod,
                        Hokflg4 = x.Key.Hokflg4,
                        Hokankr = x.Sum(y => y.Hokankr),
                        Niekikr = x.Sum(y => y.Niekikr)
                    })
                    .Where(x => x.Hokankr != 0);

                var hocTableh = new Dictionary<string, string>();
                foreach (var hocList in pcCodeListFH)
                {
                    if (!hocTableh.ContainsKey(hocList.Pccod + hocList.Hokflg4))
                    {
                        hocTableh.Add(hocList.Pccod + hocList.Hokflg4, hocList.Hokcod);
                    }
                }

                if (pcCodeListFH.Count() != 0)
                {

                    bool fFlag = true;
                    var pcCodeFH = pcCodeListFH.First();
                    string Seicod = pcCodeFH.Seicod;
                    string Kisyua = pcCodeFH.Kisyua;
                    pcError = processing == true ? "ＰＣ設定エラー（部門）" : "ＰＣ設定エラー（顧客）";
                    var SeicodList = errorList.Where(x => x.Seicod == pcCodeFH.Seicod && x.Errorstr == pcError).ToArray();
                    if (SeicodList.Count() != 0)
                    {
                        Seicod = string.Empty;
                        Kisyua = string.Empty;
                        pcError = string.Empty;
                    }

                    var pcCodeListFH2 = pcCodeListFH.GroupBy(x => new { x.Seicod, x.Pccod, x.Hokflg4 })
                        .Select(x => new
                        {
                            Seicod = x.Key.Seicod,
                            Pccod = x.Key.Pccod,
                            Hokflg4 = x.Key.Hokflg4,
                            Hokankr = x.Sum(y => y.Hokankr),
                            Niekikr = x.Sum(y => y.Niekikr)
                        });


                    foreach (var pcCodeFH2 in pcCodeListFH2)
                    {

                        if (!fFlag)
                        {
                            Seicod = string.Empty;
                            Kisyua = string.Empty;
                            pcError = string.Empty;
                        }
                        string check1 = DataUtil.SubstringEx(pcCodeFH2.Pccod, 0, 4);
                        string check2 = DataUtil.SubstringEx(pcCodeFH2.Pccod, 8, 1);
                        string check3 = DataUtil.SubstringEx(pcCodeFH2.Pccod, 4, 4);
                        string sikibetu = string.Empty;
                        decimal kingaku = 0;
                        if (check3 == "3101")
                        {
                            sikibetu = "寄託保管料";
                            if (pcCodeFH2.Hokflg4 == "Y")
                            {
                                sikibetu = "倉庫使用料";
                            }
                            kingaku = (decimal)pcCodeFH2.Hokankr;
                        }
                        else
                        {
                            sikibetu = "寄託荷役料";
                            kingaku = (decimal)pcCodeFH2.Niekikr;
                        }

                        PcCodeErrorListPrint PcCodeError = new PcCodeErrorListPrint
                        {
                            // 請求先
                            Seicod = Seicod,
                            // 機種A
                            Kisyua = Kisyua,
                            // PCコード
                            Pccod = pcCodeFH2.Pccod,
                            // 識別名
                            Shikibetu = sikibetu,
                            // 金額
                            Kingaku = kingaku,
                            // 場所
                            Hokcod = hocTableh[pcCodeFH2.Pccod + pcCodeFH2.Hokflg4],
                            // エラー
                            Errorstr = pcError
                        };
                        errorList.Add(PcCodeError);
                        fFlag = false;
                    }
                }
                #endregion

                #region 荷役料                
                var pcCodeListFN = pcCodeList.GroupBy(x => new { x.Seicod, x.Pccod, x.Kisyua, x.Hokcod })
                    .Select(x => new
                    {
                        Seicod = x.Key.Seicod,
                        Pccod = x.Key.Pccod,
                        Kisyua = x.Key.Kisyua,
                        Hokcod = x.Key.Hokcod,
                        Hokankr = x.Sum(y => y.Hokankr),
                        Niekikr = x.Sum(y => y.Niekikr)
                    })
                    .Where(x => x.Niekikr != 0)
                    .OrderBy(x => x.Pccod)
                    ;

                var hocTableN = new Dictionary<string, string>();
                foreach (var hocList in pcCodeListFN)
                {
                    if (!hocTableN.ContainsKey(hocList.Pccod))
                    {
                        hocTableN.Add(hocList.Pccod, hocList.Hokcod);
                    }
                }

                if (pcCodeListFN.Count() != 0)
                {

                    bool fFlag = true;
                    var pcCodeFH = pcCodeListFN.First();
                    string Seicod = pcCodeFH.Seicod;
                    string Kisyua = pcCodeFH.Kisyua;
                    pcError = processing == true ? "ＰＣ設定エラー（部門）" : "ＰＣ設定エラー（顧客）";
                    var SeicodList = errorList.Where(x => x.Seicod == pcCodeFH.Seicod && x.Errorstr == pcError).ToArray();
                    if (SeicodList.Count() != 0)
                    {
                        Seicod = string.Empty;
                        Kisyua = string.Empty;
                        pcError = string.Empty;
                    }

                    var pcCodeListFN2 = pcCodeListFN.GroupBy(x => new { x.Seicod, x.Pccod })
                        .Select(x => new
                        {
                            Seicod = x.Key.Seicod,
                            Pccod = x.Key.Pccod,
                            Hokankr = x.Sum(y => y.Hokankr),
                            Niekikr = x.Sum(y => y.Niekikr)
                        });


                    foreach (var pcCodeFN2 in pcCodeListFN2)
                    {

                        if (!fFlag)
                        {
                            Seicod = string.Empty;
                            Kisyua = string.Empty;
                            pcError = string.Empty;
                        }
                        string check1 = DataUtil.SubstringEx(pcCodeFN2.Pccod, 0, 4);
                        string check2 = DataUtil.SubstringEx(pcCodeFN2.Pccod, 8, 1);
                        string check3 = DataUtil.SubstringEx(pcCodeFN2.Pccod, 4, 4);
                        string sikibetu = string.Empty;
                        decimal kingaku = 0;
                        if (check3 == "3101")
                        {
                            sikibetu = "寄託保管料";
                            kingaku = (decimal)pcCodeFN2.Hokankr;
                        }
                        else
                        {
                            sikibetu = "寄託荷役料";
                            kingaku = (decimal)pcCodeFN2.Niekikr;
                        }

                        PcCodeErrorListPrint PcCodeError = new PcCodeErrorListPrint
                        {
                            // 請求先
                            Seicod = Seicod,
                            // 機種A
                            Kisyua = Kisyua,
                            // PCコード
                            Pccod = pcCodeFN2.Pccod,
                            // 識別名
                            Shikibetu = sikibetu,
                            // 金額
                            Kingaku = kingaku,
                            // 場所
                            Hokcod = hocTableN[pcCodeFN2.Pccod],
                            // エラー
                            Errorstr = pcError
                        };
                        errorList.Add(PcCodeError);
                        fFlag = false;
                    }
                }
                #endregion

            }

        }


        // FD寄託保管請求請求漏機種確認リスト
        public async Task<IEnumerable<KisyuMoreList>> KisyuMoreList()
        {
            var statusList = await dbContext.THokanNyuushuuko
                 //.Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (a, c1) => new { a, c1 })
                 .GroupJoin(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (a, c1) => new { a, c1 })
                 .SelectMany(x => x.c1.DefaultIfEmpty(), (x, c1) => new { x = x.a, c1 })
                 .Join(dbContext.MHokanSeihin, x => x.x.Hincod, x => x.Hincod, (a, c2) => new { a = a.x, c1 = a.c1, c2 })
                .Where(x => (x.a.Zansuu != 0 || x.a.Siksuu != 0 || x.a.Kaisuu != 0 || x.a.Syksuu != 0)
                    && (x.c1.Hokflg4 != "Y" || x.c2.Hinnmk.Substring(0, 3) != "Z41")
                    && (x.a.Kisyua != "SG" || x.a.Hincod.Substring(0, 3) != "EDG")
                    && (x.c1.Seitai == null || x.c1.Seitai == string.Empty))

            .Select(x => new KisyuMoreList()
            {
                Kisyua = x.a.Kisyua,
                Kisyub = x.a.Kisyub,
                Hincod = x.a.Hincod,
                Hokcod = x.a.Hokcod,
                Zansuu = x.a.Zansuu,
                Siksuu = x.a.Siksuu,
                Kaisuu = x.a.Kaisuu,
                Syksuu = x.a.Syksuu
            })

            .OrderBy(x => x.Kisyua)
            .ThenBy(x => x.Kisyub)
            .ThenBy(x => x.Hokcod)
            .ToListAsync();

            var statusData = statusList
                .Select(x => new KisyuMoreList()
                {
                    Kisyua = x.Kisyua,
                    Kisyub = x.Kisyub,
                    Hincod = x.Hincod,
                    Hokcod = x.Hokcod,
                    Zansuu = x.Zansuu,
                    Siksuu = x.Siksuu,
                    Kaisuu = x.Kaisuu,
                    Syksuu = x.Syksuu
                })
                .ToList();

            return statusData;
        }

        // FD寄託保管請求入出庫繰越データリスト
        public async Task<IEnumerable<NyuSyukoKurikosiList>> NyuSyukoKurikosiList()
        {

            //var kurikosiList = await dbContext.THokanNyuushuukoKurikosi
            //.Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (a, c1) => new { a, c1 })
            //.Join(dbContext.MShukkabasho, x => x.a.Hokcod, x => x.Sybcod, (a, c2) => new { a = a.a, c1 = a.c1, c2 })
            //.Join(dbContext.THokanDenpyokensuKurikosi, x => x.a.Kisyua, x => x.Kisyua, (a, b) => new { a = a.a, c1 = a.c1, c2 = a.c2, b })
            //.Where(x => x.c1.Seitai != "N" && (x.c1.Hokflg2 == "C" || x.c1.Nieflg2 == "C"))
            //.GroupBy(x => new { x.a.Kisyua, x.c1.Tanbsy, x.a.Hincod, x.a.Hokcod, x.c2.Sybnam, x.a.Kisyub, x.a.Kurymd })
            //.Select(x => new NyuSyukoKurikosiList()
            //{

            //    Tanbsy = x.Key.Tanbsy,
            //    Kisyua = x.Key.Kisyua,
            //    Hincod = x.Key.Hincod,
            //    Hokcod = x.Key.Hokcod,
            //    Sybnam = x.Key.Sybnam,
            //    Kisyub = x.Key.Kisyub,

            //    Zansuu = x.Sum(y => y.a.Zansuu <= 0 ? 0 : y.a.Zansuu),
            //    Siksuu = x.Sum(y => y.a.Siksuu <= 0 ? 0 : y.a.Siksuu),
            //    Kaisuu = x.Sum(y => y.a.Kaisuu <= 0 ? 0 : y.a.Kaisuu),
            //    Nskosu = x.Sum(y => y.b.Nskosu <= 0 ? 0 : y.b.Nskosu),
            //    Kurymd = x.Key.Kurymd
            //})
            //.OrderBy(x => x.Hincod)
            //.ThenBy(x => x.Hokcod)
            //.ThenBy(x => x.Kisyub)
            //.ThenBy(x => x.Kurymd)
            //.ToListAsync();

            //var strList = new List<NyuSyukoKurikosiList>();
            //foreach (var sData in kurikosiList)
            //{
            //    var jouken1 = await dbContext.MHokanJouken
            //        .Where(x => x.Jyoken == sData.Hokcod && x.Sikcod == "HOKCOD").ToListAsync();
            //    var jouken2 = await dbContext.MHokanJouken
            //        .Where(x => x.Jyoken == sData.Hokcod.Substring(0, 1) && x.Sikcod == "HOKCOD2").ToListAsync();

            //    var jouken3 = await dbContext.MHokanJouken
            //        .Where(x => x.Jyoken == sData.Hokcod && x.Sikcod == "NIECOD").ToListAsync();
            //    var jouken4 = await dbContext.MHokanJouken
            //        .Where(x => x.Jyoken == sData.Hokcod.Substring(0, 1) && x.Sikcod == "NIECOD2").ToListAsync();
            //    var tankaCK = await dbContext.MHokanTanka
            //        .Where(x => x.Hincod == sData.Hincod).ToListAsync();

            //    if (tankaCK.Count() == 0 && (jouken1.Count() != 0 || jouken2.Count() != 0 || jouken3.Count() != 0 || jouken4.Count() != 0))
            //    {
            //        strList.Add(sData);
            //    }

            //}


            var kurikosiList = await dbContext.THokanNyuushuukoKurikosi
                            .Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (a, c1) => new { a, c1 })
                            .Join(dbContext.MShukkabasho, x => x.a.Hokcod, x => x.Sybcod, (a, c2) => new { a = a.a, c1 = a.c1, c2 })
//                            .Join(dbContext.THokanDenpyokensuKurikosi, x => x.a.Kisyua, x => x.Kisyua, (a, b) => new { a = a.a, c1 = a.c1, c2 = a.c2, b })
                            //.Join(dbContext.THokanDenpyokensuKurikosi, x => new { x.a.Kurymd, x.a.Kisyua, x.a.Kisyub, x.a.Hincod },
                            //                                           x => new { x.Kurymd,   x.Kisyua,   x.Kisyub,   x.Hincod }
                            //                                            , (a, b) => new { a = a.a, c1 = a.c1, c2 = a.c2, b })
                            .GroupJoin(dbContext.THokanDenpyokensuKurikosi, x => new { x.a.Kurymd, x.a.Kisyua, x.a.Kisyub, x.a.Hincod },
                                                                            x => new { x.Kurymd, x.Kisyua, x.Kisyub, x.Hincod }
                                                                          , (a, b) => new { a = a.a, c1 = a.c1, c2 = a.c2, b })
                            .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a = a.a, a.c1, a.c2, b })
                            //.Where(x =>  x.b.Denkub == "54" || x.b.Denkub == "56" || x.b.Denkub == "57" || x.b.Denkub == "58" || x.b.Denkub == "59")
                            .GroupBy(x => new { x.a.Kisyua, x.c1.Tanbsy, x.a.Hincod, x.a.Hokcod, x.c2.Sybnam, x.a.Kisyub, x.a.Kurymd, x.a.Zansuu, x.a.Siksuu, x.a.Kaisuu })
                            .Select(x => new NyuSyukoKurikosiList()
                            {

                                Tanbsy = x.Key.Tanbsy,
                                Kisyua = x.Key.Kisyua,
                                Hincod = x.Key.Hincod,
                                Hokcod = x.Key.Hokcod,
                                Sybnam = x.Key.Sybnam,
                                Kisyub = x.Key.Kisyub,
                                Zansuu = x.Key.Zansuu,
                                Siksuu = x.Key.Siksuu,
                                Kaisuu = x.Key.Kaisuu,
                                //Zansuu = x.Sum(y => y.a.Zansuu <= 0 ? 0 : y.a.Zansuu),
                                //Siksuu = x.Sum(y => y.a.Siksuu <= 0 ? 0 : y.a.Siksuu),
                                //Kaisuu = x.Sum(y => y.a.Kaisuu <= 0 ? 0 : y.a.Kaisuu),
                                //Nskosu = x.Sum(y => y.b.Nskosu <= 0 ? 0 : y.b.Nskosu),
                                Nskosu = x.Sum(y => (y.b.Denkub == "54" || y.b.Denkub == "56" || y.b.Denkub == "57" || y.b.Denkub == "58" || y.b.Denkub == "59") && y.b.Nskosu > 0 ? y.b.Nskosu : 0),
                                Kurymd = x.Key.Kurymd
                            })
                            .OrderBy(x => x.Hincod)
                            .ThenBy(x => x.Hokcod)
                            .ThenBy(x => x.Kisyub)
                            .ThenBy(x => x.Kurymd)
                            .ToListAsync();

            return kurikosiList;

        }

        // FD保管請求請求先別集計表
        public async Task<IEnumerable<SekiyusakiShukeiList>> SekiyusakiShukeiList()
        {
            var statusList = await dbContext.THokanSeikyu
                .Where(x => x.Hokcod != "P1")
                .Where(x => x.Hokcod != "P2")
                .GroupBy(x => new { x.Seicod, x.Kisyua, x.Kisnam })
                .Select(x => new SekiyusakiShukeiList()
             {
                 Seicod = x.Key.Seicod,
                 Kisyua = x.Key.Kisyua,
                 Kisnam = x.Key.Kisnam,
                 //値引前保管料
                 HOKANSUM = x.Sum(a => (a.Kisyub != "999" || a.Hokcod != "NE") && a.Hokank != null ? a.Hokank: 0),
                 //値引前荷役料
                 NIYAKUSUM = x.Sum(a => (a.Kisyub != "999" || a.Hokcod != "NE") && a.Niekik != null ? a.Niekik : 0),
                 //値引前合計
                 beNEBIKISUM = x.Sum(a => (a.Kisyub != "999" || a.Hokcod != "NE") && a.Hokank != null ? a.Hokank : 0) + 
                                            x.Sum(a => (a.Kisyub != "999" || a.Hokcod != "NE") && a.Niekik != null ? a.Niekik : 0),

                 //保管料値引
                 Hokannbk = x.Sum(a => (a.Kisyub == "999" && a.Hokcod == "NE") && a.Hokank != null ? a.Hokank * -1: 0),
                 //荷役料値引
                 Niekinbk = x.Sum(a => (a.Kisyub == "999" && a.Hokcod == "NE") && a.Niekik != null ? a.Niekik * -1 : 0),

                //値引合計
                afNEBIKISUM = x.Sum(a => (a.Kisyub == "999" && a.Hokcod == "NE") && a.Hokank != null ? a.Hokank * -1 : 0) +  
                                         x.Sum(a => (a.Kisyub == "999" && a.Hokcod == "NE") && a.Niekik != null ? a.Niekik * -1 : 0),
                 //保管料
                 Hokank = 0,
                //荷役料
                Niekik = 0,
                //値引後合計
                goukei = 0

             })
             .OrderBy(x => x.Seicod)
             .ThenBy(x => x.Kisyua)
             .ThenBy(x => x.Kisnam)
             .ToListAsync();

            List<SekiyusakiShukeiList> resultData = new List<SekiyusakiShukeiList>();
            #region 合計用変数
            decimal? maehokk = 0;
            decimal? maeniekk = 0;
            decimal? nbkihokk = 0;
            decimal? nbkiniekk = 0;
            decimal? hokk = 0;
            decimal? niekk = 0;
            decimal? gokei = 0;
            #endregion

            foreach (SekiyusakiShukeiList status in statusList)
            {
                status.Hokank = status.HOKANSUM + status.Hokannbk;
                status.Niekik = status.NIYAKUSUM + status.Niekinbk;
                status.goukei = status.Hokank + status.Niekik;
                //合計行算出
                maehokk += status.HOKANSUM;
                maeniekk += status.NIYAKUSUM;
                nbkihokk += status.Hokannbk;
                nbkiniekk += status.Niekinbk;
                hokk += status.HOKANSUM + status.Hokannbk;
                niekk += status.NIYAKUSUM + status.Niekinbk;
                gokei += status.Hokank + status.Niekik;

                resultData.Add(status);
            }
            //合計行
            var gokeidata = new SekiyusakiShukeiList();
            gokeidata.Seicod = string.Empty;
            gokeidata.Kisyua = "合計";
            gokeidata.Kisnam = string.Empty;
            gokeidata.HOKANSUM = maehokk;
            gokeidata.NIYAKUSUM = maeniekk;
            gokeidata.beNEBIKISUM = maehokk + maeniekk;
            gokeidata.Hokannbk = nbkihokk;
            gokeidata.Niekinbk = nbkiniekk;
            gokeidata.afNEBIKISUM = nbkihokk + nbkiniekk;
            gokeidata.Hokank = hokk;
            gokeidata.Niekik = niekk;
            gokeidata.goukei = hokk + niekk;
            resultData.Add(gokeidata);

            return resultData;
        }

        // 倉庫明細書(機種A単位)
        public async Task<IEnumerable<SoukoKisyuAList>> SoukoKisyuAList()
        {
            //機種Aごとの合計データ取得
            var statusList = await dbContext.THokanSeikyu
            .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
            .Where(x => x.a.Hokflg1 == "A" && x.a.Nieflg1 == "A")
            .Where(x => x.a.Hokflg4 != "Y")
            .Where(x => x.a.Kisyub != "999" || x.a.Hokcod != "P1")
            .Where(x => x.a.Kisyub != "999" || x.a.Hokcod != "P2")
            .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3 })
            .Select(x => new SoukoKisyuAList()
            {
                //得意先コード
                Torcod = x.Key.Fbtcod,
                //取引先名
                Tornam1 = x.Key.Tornam,
                //請求先コード
                Seicod = x.Key.Seicod,
                //機種A
                Kisyua = x.Key.Kisyua,
                //機種名
                Kisnam = x.Key.Kisnam,
                //数量説明
                Suryoum = x.Key.Nieflg3 == "N" ? "扱い数" : "伝票数",
                //機種Aデータ
                //SeimaxCnt = maxCnt
            })
            .OrderBy(x => x.Seicod)
            .ThenBy(x => x.Kisyua)
            .ThenBy(x => x.Kisnam)
            .ToListAsync();

            var seiList = statusList.GroupBy(x => x.Seicod)
                            .Where(g => g.Count() > 1)
                            .Select(g => new { seiCod = g.Key, maxCnt = g.Count() }).ToList();
            //重複データList 作成
            List<string> setList = new List<string>();
            List<int> cnt = seiList.Select(a => a.maxCnt).ToList();
            if (seiList.Count() != 0)
            {
                foreach(var data in seiList)
                {
                    int index = seiList.FindIndex(x => x.seiCod == data.seiCod);
                    if (index >= 0)
                    {
                        for (int i = 0; i < cnt[index]; i++)
                        {
                            setList.Add(data.seiCod);
                        }
                    }

                }

            }

            //var pgMax = statusList.GroupBy(x => x.Seicod).Count();
            var pgMax = 1;
            var setCnt = 0;
            var ck = 0;

            var pgCnt = 0;

            string seicod = setList.Count() != 0 ? setList[0] : string.Empty;
            string seicodCP = string.Join("&", statusList.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(0).Take(1));

            List<SoukoKisyuAList> resultData = new List<SoukoKisyuAList>();
            foreach (SoukoKisyuAList status in statusList)
            {
                decimal? hogokei = 0;
                decimal? nigoukei = 0;
                decimal? sugoukei = 0;
                ck += 1;

                #region 頁数取得
                //現在ページ数取得
                if(setList.Count() != 0)
                {
                    if (seicod != status.Seicod)
                    {
                        pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                    }
                    else
                    {
                        if (seicodCP != status.Seicod)
                        {
                            pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                            seicodCP = string.Join("&", statusList.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(ck - 1).Take(1));
                        }
                        else
                        {
                            pgCnt = pgCnt == 0 ? 1 : pgCnt;
                        }
                        setCnt += 1;
                        if (setCnt < setList.Count())
                        {
                            seicod = setList[setCnt];
                        }
                    }
                }
                else
                {
                    pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                }
                #endregion

                //取引先文字算出
                string tornam = status.Tornam1;
                status.Tornam1 = tornam.Length >= 20 ? tornam.Substring(20) : tornam;
                status.Tornam2 = tornam.Length >= 20 ? tornam.Substring(0,20) : string.Empty;

                //機種A明細
                var meisaiData = await dbContext.THokanSeikyu
                    .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                    .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                    .Where(x => x.a.Hokflg4 != "Y")
                    .Where(x => x.a.Seicod == status.Seicod)
                    .Where(x => x.a.Kisyua == status.Kisyua)
                    .Where(x => x.a.Kisyub != "998")
                    .Where(x => x.a.Hokcod != "NI")
                    .Where(x => x.a.Kisyub != "999")
                    .Where(x => x.a.Hokcod != "NE")
                    .Where(x => x.a.Hokcod != "P1")
                     .Where(x => x.a.Hokcod != "P2")
                    //.GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3, x.a.Kisyub, x.a.Hokant, x.a.Niekit })
                    .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Hokant, x.a.Niekit})
                    .Select(x => new SoukoKisyuAList()
                    {
                        //得意先コード
                        Torcod = x.Key.Fbtcod,
                        //取引先名
                        Tornam1 = status.Tornam1,
                        Tornam2 = status.Tornam2,
                        //請求先コード
                        Seicod = x.Key.Seicod,
                        //機種A
                        Kisyua = x.Key.Kisyua,
                        //機種名
                        Kisnam = x.Key.Kisnam,
                        //数量説明
                        Suryoum = status.Suryoum,
                        //前月残数
                        Zansuu = x.Sum(a => a.a.Zansuu),
                        //入庫数
                        Nyuksu = x.Sum(a => a.a.Nyuksu),
                        //積数
                        Sekisu = x.Sum(a => a.a.Sekisu),
                        //保管料
                        Hokank = x.Sum(a => a.a.Hokank),
                        //保管単価
                        Hokant = x.Key.Hokant,
                        //荷役料
                        Niekik = x.Sum(a => a.a.Niekik),
                        //荷役単価
                        //Niekit = x.Sum(a => a.a.Niekit),
                        Niekit = x.Key.Niekit,
                        //数量
                        Suryou = status.Suryoum == "扱い数" ? x.Sum(a => a.a.Atukai) : x.Sum(a => a.a.Densuu),
                        //Suryou = x.Sum(a => a.a.Nieflg3 == "N" ? a.a.Atukai : a.a.Densuu),

                    //品番コード説明
                    Hinnam = string.Empty,
                            //合計
                            Goukei = x.Sum(a => a.a.Hokank + a.a.Niekik),
                            Maxpg = pgMax,
                            //Cntpg = pgCnt
                            Cntpg = 1
                    }).ToListAsync();

                if (meisaiData.Count() != 0)
                {
                    var meisai = meisaiData.First();
                    hogokei += meisai.Hokank;
                    nigoukei += meisai.Niekik;
                    sugoukei += meisai.Suryou;
                    resultData.Add(meisai);
                    //hogokei += meisaiData.Sum(x => x.Hokank);
                    //nigoukei += meisaiData.Sum(x => x.Niekik);
                    //sugoukei += meisaiData.Sum(x => x.Suryou);
                    //resultData.AddRange(meisaiData);
                }

                //荷役料
                var niyakuData = await dbContext.THokanSeikyu
                    .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                    .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                    .Where(x => x.a.Hokflg4 != "Y")
                    .Where(x => x.a.Seicod == status.Seicod)
                    .Where(x => x.a.Kisyua == status.Kisyua)
                    .Where(x => x.a.Kisyub == "997" )
                    .Where(x => x.a.Hokcod == "NI")
                    .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3, x.a.Kisyub, x.a.Niekit })
                    .Select(x => new SoukoKisyuAList()
                    {
                        //得意先コード
                        Torcod = x.Key.Fbtcod,
                        //取引先名
                        Tornam1 = status.Tornam1,
                        Tornam2 = status.Tornam2,
                        //請求先コード
                        Seicod = x.Key.Seicod,
                        //機種A
                        Kisyua = x.Key.Kisyua,
                        //機種名
                        Kisnam = x.Key.Kisnam,
                        //数量説明
                        Suryoum = status.Suryoum,
                        //前月残数
                        Zansuu = null,
                        //入庫数
                        Nyuksu = null,
                        //積数
                        Sekisu = null,
                        //保管料
                        Hokank = null,
                        //保管単価
                        Hokant = null,
                        //荷役料
                        Niekik = x.Sum(a => a.a.Niekik),
                        //荷役単価
                        Niekit = x.Key.Niekit,
                        //数量
                        Suryou = x.Sum(a => a.a.Nieflg3 == "N" ? a.a.Atukai : a.a.Densuu),
                        //品番コード説明
                        Hinnam = "《　荷役料　》",
                        //合計
                        Goukei = x.Sum(a => a.a.Niekik),
                        Maxpg = pgMax,
                        //Cntpg = pgCnt
                        Cntpg = 1
                    }).ToListAsync();
                if (niyakuData.Count() != 0)
                {
                    var niyk = niyakuData.First();
                    nigoukei += niyk.Niekik;
                    sugoukei += niyk.Suryou;
                    resultData.Add(niyk);
                }


                //休日分
                var kyuzituData = await dbContext.THokanSeikyu
                .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                .Where(x => x.a.Hokflg4 != "Y")
                .Where(x => x.a.Seicod == status.Seicod)
                .Where(x => x.a.Kisyua == status.Kisyua)
                .Where(x => x.a.Kisyub == "998" )
                .Where(x => x.a.Hokcod == "NI")
                .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3, x.a.Kisyub, x.a.Niekit })
                .Select(x => new SoukoKisyuAList()
                {
                    //得意先コード
                    Torcod = x.Key.Fbtcod,
                    //取引先名
                    Tornam1 = status.Tornam1,
                    Tornam2 = status.Tornam2,
                    //請求先コード
                    Seicod = x.Key.Seicod,
                    //機種A
                    Kisyua = x.Key.Kisyua,
                    //機種名
                    Kisnam = x.Key.Kisnam,
                    //数量説明
                    Suryoum = status.Suryoum,
                    //前月残数
                    Zansuu = null,
                    //入庫数
                    Nyuksu = null,
                    //積数
                    Sekisu = null,
                    //保管料
                    Hokank = null,
                    //保管単価
                    Hokant = null,
                    //荷役料
                    Niekik = x.Sum(a => a.a.Niekik),
                    //荷役単価
                    Niekit = x.Key.Niekit,
                    //数量
                    Suryou = x.Sum(a => a.a.Nieflg3 == "N" ? a.a.Atukai : a.a.Densuu),
                    //品番コード説明
                    Hinnam = "《　休日分　》",
                    //合計
                    Goukei = x.Sum(a => a.a.Niekik),
                    Maxpg = pgMax,
                    //Cntpg = pgCnt
                    Cntpg = 1
                }).ToListAsync();
                if (kyuzituData.Count() != 0)
                {
                    var kyuzitu = kyuzituData.First();
                    nigoukei += kyuzitu.Niekik;
                    sugoukei += kyuzitu.Suryou;
                    resultData.Add(kyuzitu);
                }

                //値引分
                var nebikiData = await dbContext.THokanSeikyu
                .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                .Where(x => x.a.Hokflg4 != "Y")
                .Where(x => x.a.Seicod == status.Seicod)
                .Where(x => x.a.Kisyua == status.Kisyua)
                .Where(x => x.a.Kisyub == "999")
                .Where(x => x.a.Hokcod == "NE")
                .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3, x.a.Kisyub })
                .Select(x => new SoukoKisyuAList()
                {
                    //得意先コード
                    Torcod = x.Key.Fbtcod,
                    //取引先名
                    Tornam1 = status.Tornam1,
                    Tornam2 = status.Tornam2,
                    //請求先コード
                    Seicod = x.Key.Seicod,
                    //機種A
                    Kisyua = x.Key.Kisyua,
                    //機種名
                    Kisnam = x.Key.Kisnam,
                    //数量説明
                    Suryoum = status.Suryoum,
                    //前月残数
                    Zansuu = null,
                    //入庫数
                    Nyuksu = null,
                    //積数
                    Sekisu = null,
                    //保管料
                    Hokank = x.Sum(a => a.a.Hokank * -1),
                    //保管単価
                    Hokant = null,
                    //荷役料
                    Niekik = x.Sum(a => a.a.Niekik * -1),
                    //荷役単価
                    Niekit = null,
                    //数量
                    Suryou = null,
                    //品番コード説明
                    Hinnam = "《　値引き　》",
                    //合計
                    Goukei = x.Sum(a => a.a.Hokank + a.a.Niekik * -1),
                    Maxpg = pgMax,
                    //Cntpg = pgCnt
                    Cntpg = 1
                }).ToListAsync();
                if (nebikiData.Count() != 0)
                {
                    #region nullデータ行作成
                    var nullData = await dbContext.THokanSeikyu
                        .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                        .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                        .Where(x => x.a.Hokflg4 != "Y")
                        .Where(x => x.a.Seicod == status.Seicod)
                        .Where(x => x.a.Kisyua == status.Kisyua)
                        .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3, x.a.Kisyub })
                        .Select(x => new SoukoKisyuAList()
                        {
                            //得意先コード
                            Torcod = x.Key.Fbtcod,
                            //取引先名
                            Tornam1 = status.Tornam1,
                            Tornam2 = status.Tornam2,
                            //請求先コード
                            Seicod = x.Key.Seicod,
                            //機種A
                            Kisyua = x.Key.Kisyua,
                            //機種名
                            Kisnam = x.Key.Kisnam,
                            //数量説明
                            Suryoum = status.Suryoum,
                            //前月残数
                            Zansuu = null,
                            //入庫数
                            Nyuksu = null,
                            //積数
                            Sekisu = null,
                            //保管料
                            Hokank = null,
                            //保管単価
                            Hokant = null,
                            //荷役料
                            Niekik = null,
                            //荷役単価
                            Niekit = null,
                            //数量
                            Suryou = null,
                            //品番コード説明
                            Hinnam = string.Empty,
                            //合計
                            Goukei = null,
                            Maxpg = pgMax,
                            Cntpg = pgCnt
                        }).FirstOrDefaultAsync();
                    resultData.Add(nullData);
                    #endregion

                    var nebiki = nebikiData.First();
                    resultData.Add(nebiki);
                    hogokei += nebiki.Hokank;
                    nigoukei += nebiki.Niekik;

                }
                if (meisaiData.Count() != 0 | niyakuData.Count() != 0 | kyuzituData.Count() != 0 | nebikiData.Count() != 0)
                {
                    #region nullデータ行作成
                    var nullData = await dbContext.THokanSeikyu
                        .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                        .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                        .Where(x => x.a.Hokflg4 != "Y")
                        .Where(x => x.a.Seicod == status.Seicod)
                        .Where(x => x.a.Kisyua == status.Kisyua)
                        .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3, x.a.Kisyub })
                        .Select(x => new SoukoKisyuAList()
                        {
                            //得意先コード
                            Torcod = x.Key.Fbtcod,
                            //取引先名
                            Tornam1 = status.Tornam1,
                            Tornam2 = status.Tornam2,
                            //請求先コード
                            Seicod = x.Key.Seicod,
                            //機種A
                            Kisyua = x.Key.Kisyua,
                            //機種名
                            Kisnam = x.Key.Kisnam,
                            //数量説明
                            Suryoum = status.Suryoum,
                            //前月残数
                            Zansuu = null,
                            //入庫数
                            Nyuksu = null,
                            //積数
                            Sekisu = null,
                            //保管料
                            Hokank = null,
                            //保管単価
                            Hokant = null,
                            //荷役料
                            Niekik = null,
                            //荷役単価
                            Niekit = null,
                            //数量
                            Suryou = null,
                            //品番コード説明
                            Hinnam = string.Empty,
                            //合計
                            Goukei = null,
                            Maxpg = pgMax,
                            Cntpg = pgCnt
                        }).FirstOrDefaultAsync();
                    resultData.Add(nullData);
                    #endregion
                    //合計行
                    var goukeiData = await dbContext.THokanSeikyu
                    .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                    .Where(x => x.a.Hokflg1 == "A" | x.a.Nieflg1 == "A")
                    .Where(x => x.a.Hokflg4 != "Y")
                    .Where(x => x.a.Seicod == status.Seicod)
                    .Where(x => x.a.Kisyua == status.Kisyua)
                    .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3 })
                    .Select(x => new SoukoKisyuAList()
                    {
                    //得意先コード
                    Torcod = x.Key.Fbtcod,
                    //取引先名
                    Tornam1 = status.Tornam1,
                    Tornam2 = status.Tornam2,

                    //請求先コード
                    Seicod = x.Key.Seicod,
                    //機種A
                    Kisyua = x.Key.Kisyua,
                    //機種名
                    Kisnam = x.Key.Kisnam,
                    //数量説明
                    Suryoum = status.Suryoum,
                    //【合　計】
                    Hinnam = "【合　　計】",
                    //前月残数
                    Zansuu = x.Sum(a => a.a.Zansuu),
                    //入庫数
                    Nyuksu = x.Sum(a => a.a.Nyuksu),
                    //積数
                    Sekisu = x.Sum(a => a.a.Sekisu),
                    //保管料
                    Hokank = hogokei,
                    //保管単価
                    Hokant = null,
                    //荷役料
                    Niekik = nigoukei,
                    //荷役単価
                    Niekit = null,
                    //数量
                    Suryou = sugoukei,
                    //合計
                    Goukei = hogokei + nigoukei,
                        Maxpg = pgMax,
                        //Cntpg = pgCnt
                        Cntpg = 1
                    })
                    .FirstOrDefaultAsync();
                    resultData.Add(goukeiData);
                }
            }
            return resultData;
        }

        // 倉庫明細書(品番コード単位)
        public async Task<IEnumerable<SoukoHinCodList>> SoukoHinCodList()
        {
            List<SoukoHinCodList> result = new List<SoukoHinCodList>();

            //機種Aごとのデータ取得
            var status = await dbContext.THokanSeikyu
            .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
            .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 != "Y")
            .Where(x => x.a.Hokcod != "P1")
            .Where(x => x.a.Hokcod != "P2")
            .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3 })
            .Select(x => new SoukoHinCodList()
            {
                Torcod = x.Key.Fbtcod,
                Tornam1 = x.Key.Tornam,
                Seicod = x.Key.Seicod,
                Kisyua = x.Key.Kisyua,
                Kisnam = x.Key.Kisnam,
                Suryoum = x.Key.Nieflg3 == "N" ? "扱い数" : "伝票数",
            })
            .OrderBy(x => x.Seicod)
            .ThenBy(x => x.Kisyua)
            .ThenBy(x => x.Kisnam)
            .ToListAsync();

            var seiList = status.GroupBy(x => x.Seicod)
                            .Where(g => g.Count() > 1)
                            .Select(g => new { seiCod = g.Key, maxCnt = g.Count() }).ToList();
            //重複データList 作成
            List<string> setList = new List<string>();
            List<int> cnt = seiList.Select(a => a.maxCnt).ToList();
            if (seiList.Count() != 0)
            {
                foreach (var data in seiList)
                {
                    int index = seiList.FindIndex(x => x.seiCod == data.seiCod);
                    if (index >= 0)
                    {
                        for (int i = 0; i < cnt[index]; i++)
                        {
                            setList.Add(data.seiCod);
                        }
                    }

                }

            }

            int pgCnt = 0;
            var setCnt = 0;
            var ck = 0;
            var maxCnt = status.GroupBy(x => x.Seicod).Count();
            string seicod = setList.Count() != 0 ? setList[0] : string.Empty;
            string seicodCP = string.Join("&", status.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(0).Take(1));

            //機種Aごとにloop 機種Bごとデータ取得
            foreach (var sdata in status)
            {
                #region 合計用変数
                decimal? zankei = 0;
                decimal? nyukei = 0;
                decimal? sekikei = 0;
                decimal? hokkei = 0;
                decimal? niekei = 0;
                decimal? sukei = 0;
                #endregion
                ck += 1;

                #region 頁数取得
                //現在ページ数取得
                if (setList.Count() != 0)
                {
                    if (seicod != sdata.Seicod)
                    {
                        pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                    }
                    else
                    {
                        if (seicodCP != sdata.Seicod)
                        {
                            pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                            seicodCP = string.Join("&", status.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(ck - 1).Take(1));
                        }
                        else
                        {
                            pgCnt = pgCnt == 0 ? 1 : pgCnt;
                        }
                        setCnt += 1;
                        if (setCnt < setList.Count())
                        {
                            seicod = setList[setCnt];
                        }
                    }
                }
                else
                {
                    pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                }
                #endregion

                //取引先文字算出
                string tornam = sdata.Tornam1;
                sdata.Tornam1 = tornam.Length >= 20 ? tornam.Substring(20) : tornam;
                sdata.Tornam2 = tornam.Length >= 20 ? tornam.Substring(0, 20) : string.Empty;

                //明細取得(機種Bnull以外)
                var data1 = await dbContext.THokanSeikyu
                    .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                    .Where(x => x.a.Seicod == sdata.Seicod)
                    .Where(x => x.a.Kisyua == sdata.Kisyua)
                    .Where(x => x.a.Kisyub != string.Empty)
                    .Where(x => x.a.Kisyub != null)
                    .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 != "Y")
                    .Where(x => x.a.Hokcod != "P1")
                    .Where(x => x.a.Hokcod != "P2")
                    .Where(x => (x.a.Kisyub != "997" | x.a.Hokcod != "NI"))  //荷役料以外
                    .Where(x => (x.a.Kisyub != "998" | x.a.Hokcod != "NI"))  //休日分以外
                    .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "NE")) //値引以外
                    .Select(x => new SoukoHinCodList()
                    {
                        Torcod = sdata.Torcod,
                        Tornam1 = sdata.Tornam1,
                        Tornam2 = sdata.Tornam2,
                        Seicod = sdata.Seicod,
                        Kisyua = sdata.Kisyua,
                        Kisnam = sdata.Kisnam,
                        Kisyub = x.a.Kisyub,
                        Suryoum = sdata.Suryoum,
                        Hinnmk = x.a.Hinnmk,
                        Hincod = x.a.Hincod,
                        Zansuu = x.a.Zansuu,
                        Nyuksu = x.a.Nyuksu,
                        Sekisu = x.a.Sekisu,
                        Hokank = x.a.Hokank,
                        Niekik = null,
                        Hokant = x.a.Hokant,
                        Niekit = null,
                        Suryou = null,
                        Goukei = x.a.Niekik + x.a.Hokank,
                        Cntpg = pgCnt,
                        Maxpg = maxCnt
                    })
                    .OrderBy(x => x.Kisyub)
                    .ThenBy(x => x.Hincod)
                    .ToListAsync();


                if (data1.Count() != 0)
                {
                    zankei = data1.Sum(x => x.Zansuu);
                    nyukei = data1.Sum(x => x.Nyuksu);
                    sekikei = data1.Sum(x => x.Sekisu);
                    sukei = data1.Sum(x => x.Suryou);
                    hokkei = data1.Sum(x => x.Hokank);
                    niekei = data1.Sum(x => x.Niekik);
                    var kiB = data1.GroupBy(x => x.Kisyub).ToList();

                    if (kiB.Count() > 1)
                    {
                        int ckcnt = 0;
                        string kib = string.Join("&", data1.Select(item => string.Format("{1}", item.Kisyub, item.Kisyub)).Skip(0).Take(1));
                        foreach (var data in data1)
                        {
                            ckcnt += 1;
                            if (data.Kisyub != kib)
                            {
                                //小計取得
                                var st = await dbContext.THokanSeikyu
                                    .Where(x => x.Seicod == sdata.Seicod)
                                    .Where(x => x.Kisyua == sdata.Kisyua)
                                    .Where(x => x.Kisyub == kib)
                                    .Where(x => x.Hokcod != "P1")
                                    .Where(x => x.Hokcod != "P2")
                                    .GroupBy(x => new
                                    {
                                        x.Seicod,
                                        x.Kisyua,
                                        x.Kisyub
                                    })
                                    .Select(x => new SoukoHinCodList()
                                    {
                                        Torcod = sdata.Torcod,
                                        Tornam1 = sdata.Tornam1,
                                        Tornam2 = sdata.Tornam2,
                                        Seicod = sdata.Seicod,
                                        Kisyua = sdata.Kisyua,
                                        Kisnam = sdata.Kisnam,
                                        Kisyub = null,
                                        Suryoum = sdata.Suryoum,
                                        Hinnmk = null,
                                        Hincod = "《　小　計　》",
                                        Zansuu =null,
                                        Nyuksu = null,
                                        Sekisu =null,
                                        Hokank = x.Sum( a =>  a.Hokank),
                                        Niekik = x.Sum(a => a.Niekik),
                                        Hokant = null,
                                        Niekit = null,
                                        Suryou = null,
                                        Goukei = x.Sum(a => a.Hokank + a.Niekik),
                                        Cntpg = pgCnt,
                                        Maxpg = maxCnt
                                    }).FirstOrDefaultAsync();
                                result.Add(st);
                                kib = string.Join("&", data1.Select(item => string.Format("{1}", item.Kisyub, item.Kisyub)).Skip(ckcnt).Take(1));
                                //if(result.Count() % 15 != 0)
                                //{
                                //    #region nullデータ行作成
                                //var nulls = new SoukoHinCodList();
                                //nulls.Torcod = sdata.Torcod;
                                //nulls.Tornam1 = sdata.Tornam1;
                                //nulls.Tornam2 = sdata.Tornam2;
                                //nulls.Seicod = sdata.Seicod;
                                //nulls.Kisyua = sdata.Kisyua;
                                //nulls.Kisnam = sdata.Kisnam;
                                //nulls.Hincod = null;
                                //nulls.Zansuu = null;
                                //nulls.Nyuksu = null;
                                //nulls.Sekisu = null;
                                //nulls.Hokank = null;
                                //nulls.Niekik = null;
                                //nulls.Goukei = null;
                                //result.Add(nulls);
                                //    #endregion
                                //}
                            }
                            result.Add(data);

                        }
                        //小計取得
                        var st1 = await dbContext.THokanSeikyu
                                    .Where(x => x.Seicod == sdata.Seicod)
                                    .Where(x => x.Kisyua == sdata.Kisyua)
                                    .Where(x => x.Kisyub == kib)
                                    .Where(x => x.Hokcod != "P1")
                                    .Where(x => x.Hokcod != "P2")
                                    .GroupBy(x => new
                                    {
                                        x.Seicod,
                                        x.Kisyua,
                                        x.Kisyub
                                    })
                                    .Select(x => new SoukoHinCodList()
                                    {
                                        Torcod = sdata.Torcod,
                                        Tornam1 = sdata.Tornam1,
                                        Tornam2 = sdata.Tornam2,
                                        Seicod = sdata.Seicod,
                                        Kisyua = sdata.Kisyua,
                                        Kisnam = sdata.Kisnam,
                                        Kisyub = null,
                                        Suryoum = sdata.Suryoum,
                                        Hinnmk = null,
                                        Hincod = "《　小　計　》",
                                        Zansuu = null,
                                        Nyuksu = null,
                                        Sekisu = null,
                                        Hokank = x.Sum(a => a.Hokank),
                                        Niekik = x.Sum(a => a.Niekik),
                                        Hokant = null,
                                        Niekit = null,
                                        Suryou = null,
                                        Goukei = x.Sum(a => a.Hokank + a.Niekik),
                                        Cntpg = pgCnt,
                                        Maxpg = maxCnt
                                    }).FirstOrDefaultAsync();
                                result.Add(st1);
                                //#region nullデータ行作成
                                //var nulls1 = new SoukoHinCodList();
                                //nulls1.Torcod = sdata.Torcod;
                                //nulls1.Tornam1 = sdata.Tornam1;
                                //nulls1.Tornam2 = sdata.Tornam2;
                                //nulls1.Seicod = sdata.Seicod;
                                //nulls1.Kisyua = sdata.Kisyua;
                                //nulls1.Kisnam = sdata.Kisnam;
                                //nulls1.Hincod = null;
                                //nulls1.Zansuu = null;
                                //nulls1.Nyuksu = null;
                                //nulls1.Sekisu = null;
                                //nulls1.Hokank = null;
                                //nulls1.Niekik = null;
                                //nulls1.Goukei = null;
                                //result.Add(nulls1);
                                //#endregion
                    }
                    else
                    {
                        result.AddRange(data1);
                    }
                }

                //機種Bが登録されていないものを取得
                var data6 = await dbContext.THokanSeikyu
                    .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                    .Where(x => x.a.Seicod == sdata.Seicod)
                    .Where(x => x.a.Kisyua == sdata.Kisyua)
                    .Where(x => x.a.Kisyub == string.Empty | x.a.Kisyub == null)
                    .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 != "Y")
                    .Where(x => x.a.Hokcod != "P1")
                    .Where(x => x.a.Hokcod != "P2")
                    .Where(x => (x.a.Kisyub != "997" | x.a.Hokcod != "NI"))  //荷役料以外
                    .Where(x => (x.a.Kisyub != "998" | x.a.Hokcod != "NI"))  //休日分以外
                    .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "NE")) //値引以外
                    .GroupBy(x => new
                    {
                        x.c1.Fbtcod,
                        x.c1.Tornam,
                        x.a.Seicod,
                        x.a.Kisyua,
                        x.a.Kisyub,
                        x.a.Kisnam,
                        x.a.Hinnmk,
                        x.a.Hincod
                    })
                    .Select(x => new SoukoHinCodList()
                    {
                        Torcod = x.Key.Fbtcod,
                        Tornam1 = sdata.Tornam1,
                        Tornam2 = sdata.Tornam2,
                        Seicod = x.Key.Seicod,
                        Kisyua = x.Key.Kisyua,
                        Kisnam = x.Key.Kisnam,
                        Kisyub = x.Key.Kisyub,
                        Suryoum = sdata.Suryoum,
                        Hinnmk = x.Key.Hinnmk,
                        Hincod = x.Key.Hincod,
                        Zansuu = x.Sum(a => a.a.Zansuu),
                        Nyuksu = x.Sum(a => a.a.Nyuksu),
                        Sekisu = x.Sum(a => a.a.Sekisu),
                        Hokank = x.Sum(a => a.a.Hokank),
                        Hokant = x.Sum(a => a.a.Hokant),
                        Goukei = x.Sum(a => a.a.Niekik + a.a.Hokank),
                        Cntpg = pgCnt,
                        Maxpg = maxCnt
                    })
                    .OrderBy(x => x.Kisyub)
                    .ThenBy(x => x.Hincod)
                    .ToListAsync();

                if (data6.Count() != 0)
                {
                    result.AddRange(data6);
                    zankei = zankei + data6.Sum(x => x.Zansuu);
                    nyukei = nyukei + data6.Sum(x => x.Nyuksu);
                    sekikei = sekikei + data6.Sum(x => x.Sekisu);
                    sukei = sukei + data6.Sum(x => x.Suryou);
                    hokkei = hokkei + data6.Sum(x => x.Hokank);
                    niekei = niekei + data6.Sum(x => x.Niekik);
                }

            //荷役料取得
            var data3 = await dbContext.THokanSeikyu
                    .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                    .Where(x => x.a.Seicod == sdata.Seicod)
                    .Where(x => x.a.Kisyua == sdata.Kisyua)
                    .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 != "Y")
                    .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "P1"))
                    .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "P2"))
                    .Where(x => x.a.Kisyub == "997")
                    .Where(x => x.a.Hokcod == "NI")
                    .GroupBy(x => new
                    {
                        x.c1.Fbtcod,
                        x.c1.Tornam,
                        x.a.Seicod,
                        x.a.Kisyua,
                        x.a.Kisyub,
                        x.a.Kisnam
                    })
                    .Select(x => new SoukoHinCodList()
                    {
                        Torcod = x.Key.Fbtcod,
                        Tornam1 = sdata.Tornam1,
                        Tornam2 = sdata.Tornam2,
                        Seicod = x.Key.Seicod,
                        Kisyua = x.Key.Kisyua,
                        Kisnam = x.Key.Kisnam,
                        Kisyub = string.Empty,
                        Suryoum = sdata.Suryoum,
                        Hinnmk = string.Empty,
                        Hincod = "《　荷役料　》",
                        Zansuu = null,
                        Nyuksu = null,
                        Sekisu = null,
                        Hokank = null,
                        Niekik = x.Sum(a => a.a.Niekik),
                        Hokant = null,
                        Niekit = x.Sum(a => a.a.Niekit),
                        Suryou = x.Sum(a => sdata.Suryoum == "扱い数" ? a.a.Atukai : a.a.Densuu),
                        Goukei = x.Sum(a => a.a.Niekik),
                        Cntpg = pgCnt,
                        Maxpg = maxCnt
                    }).ToListAsync();
                if (data3.Count() != 0)
                {
                    result.AddRange(data3);
                    zankei = zankei + data3.Sum(x => x.Zansuu);
                    nyukei = nyukei + data3.Sum(x => x.Nyuksu);
                    sekikei = sekikei + data3.Sum(x => x.Sekisu);
                    sukei = sukei + data3.Sum(x => x.Suryou);
                    hokkei = hokkei + data3.Sum(x => x.Hokank);
                    niekei = niekei + data3.Sum(x => x.Niekik);

                    //#region nullデータ行作成
                    //var nulldata = new SoukoHinCodList();
                    //nulldata.Torcod = sdata.Torcod;
                    //nulldata.Tornam1 = sdata.Tornam1;
                    //nulldata.Tornam2 = sdata.Tornam2;
                    //nulldata.Seicod = sdata.Seicod;
                    //nulldata.Kisyua = sdata.Kisyua;
                    //nulldata.Kisnam = sdata.Kisnam;
                    //nulldata.Hincod = string.Empty;
                    //nulldata.Zansuu = null;
                    //nulldata.Nyuksu = null;
                    //nulldata.Sekisu = null;
                    //nulldata.Hokank = null;
                    //nulldata.Niekik = null;
                    //nulldata.Goukei = null;
                    //result.Add(nulldata);
                    //#endregion
                }
                //休日分
                var data4 = await dbContext.THokanSeikyu
                        .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                        .Where(x => x.a.Seicod == sdata.Seicod)
                        .Where(x => x.a.Kisyua == sdata.Kisyua)
                        .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "P1"))
                        .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "P2"))
                        .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 != "Y")
                        .Where(x => x.a.Kisyub == "998")
                        .Where(x => x.a.Hokcod == "NI")
                        .GroupBy(x => new
                        {
                            x.c1.Fbtcod,
                            x.c1.Tornam,
                            x.a.Seicod,
                            x.a.Kisyua,
                            x.a.Kisyub,
                            x.a.Kisnam
                        })
                        .Select(x => new SoukoHinCodList()
                        {
                            Torcod = x.Key.Fbtcod,
                            Tornam1 = sdata.Tornam1,
                            Tornam2 = sdata.Tornam2,
                            Seicod = x.Key.Seicod,
                            Kisyua = x.Key.Kisyua,
                            Kisnam = x.Key.Kisnam,
                            Kisyub = string.Empty,
                            Suryoum = sdata.Suryoum,
                            Hinnmk = string.Empty,
                            Hincod = "《　休日分　》",
                            Zansuu = null,
                            Nyuksu = null,
                            Sekisu = null,
                            Hokank = null,
                            Niekik = x.Sum(a => a.a.Niekik),
                            Hokant = null,
                            Niekit = x.Sum(a => a.a.Niekit),
                            Suryou = x.Sum(a => sdata.Suryoum == "扱い数" ? a.a.Atukai : a.a.Densuu),
                            Goukei = x.Sum(a => a.a.Niekik),
                            Cntpg = pgCnt,
                            Maxpg = maxCnt
                        }).ToListAsync();
                if (data4.Count() != 0)
                {
                    result.AddRange(data4);
                    zankei = zankei + data4.Sum(x => x.Zansuu);
                    nyukei = nyukei + data4.Sum(x => x.Nyuksu);
                    sekikei = sekikei + data4.Sum(x => x.Sekisu);
                    sukei = sukei + data4.Sum(x => x.Suryou);
                    hokkei = hokkei + data4.Sum(x => x.Hokank);
                    niekei = niekei + data4.Sum(x => x.Niekik);

                    //#region nullデータ行作成
                    //var nulldata = new SoukoHinCodList();
                    //nulldata.Torcod = sdata.Torcod;
                    //nulldata.Tornam1 = sdata.Tornam1;
                    //nulldata.Tornam2 = sdata.Tornam2;
                    //nulldata.Seicod = sdata.Seicod;
                    //nulldata.Kisyua = sdata.Kisyua;
                    //nulldata.Kisnam = sdata.Kisnam;
                    //nulldata.Hincod = string.Empty;
                    //nulldata.Zansuu = null;
                    //nulldata.Nyuksu = null;
                    //nulldata.Sekisu = null;
                    //nulldata.Hokank = null;
                    //nulldata.Niekik = null;
                    //nulldata.Goukei = null;
                    //result.Add(nulldata);
                    //#endregion
                }
                //値引料
                var data5 = await dbContext.THokanSeikyu
                        .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                        .Where(x => x.a.Seicod == sdata.Seicod)
                        .Where(x => x.a.Kisyua == sdata.Kisyua)
                        .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 != "Y")
                        .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "P1"))
                        .Where(x => (x.a.Kisyub != "999" | x.a.Hokcod != "P2"))
                        .Where(x => x.a.Kisyub == "999")
                        .Where(x => x.a.Hokcod == "NE")
                        .GroupBy(x => new
                        {
                            x.c1.Fbtcod,
                            x.c1.Tornam,
                            x.a.Seicod,
                            x.a.Kisyua,
                            x.a.Kisyub,
                            x.a.Kisnam
                        })
                        .Select(x => new SoukoHinCodList()
                        {
                            Torcod = x.Key.Fbtcod,
                            Tornam1 = sdata.Tornam1,
                            Tornam2 = sdata.Tornam2,
                            Seicod = x.Key.Seicod,
                            Kisyua = x.Key.Kisyua,
                            Kisnam = x.Key.Kisnam,
                            Kisyub = string.Empty,
                            Suryoum = sdata.Suryoum,
                            Hinnmk = string.Empty,
                            Hincod = "《　値引き　》",
                            Zansuu = null,
                            Nyuksu = null,
                            Sekisu = null,
                            Hokank = x.Sum(a => a.a.Hokank * -1),
                            Niekik = x.Sum(a => a.a.Niekik * -1),
                            Hokant = null,
                            Niekit = null,
                            Suryou = null,
                            Goukei = x.Sum(a => a.a.Hokank * -1 + a.a.Niekik * -1),
                            Cntpg = pgCnt,
                            Maxpg = maxCnt
                        }).ToListAsync();
                if (data5.Count() != 0)
                {
                    #region nullデータ行作成
                    var nulldata = new SoukoHinCodList();
                    nulldata.Torcod = sdata.Torcod;
                    nulldata.Tornam1 = sdata.Tornam1;
                    nulldata.Tornam2 = sdata.Tornam2;
                    nulldata.Seicod = sdata.Seicod;
                    nulldata.Kisyua = sdata.Kisyua;
                    nulldata.Kisnam = sdata.Kisnam;
                    nulldata.Hincod = string.Empty;
                    nulldata.Zansuu = null;
                    nulldata.Nyuksu = null;
                    nulldata.Sekisu = null;
                    nulldata.Hokank = null;
                    nulldata.Niekik = null;
                    nulldata.Goukei = null;
                    nulldata.Cntpg = pgCnt;
                    nulldata.Maxpg = maxCnt;
                    result.Add(nulldata);
                    #endregion
                    result.AddRange(data5);
                    zankei = zankei + data5.Sum(x => x.Zansuu);
                    nyukei = nyukei + data5.Sum(x => x.Nyuksu);
                    sekikei = sekikei + data5.Sum(x => x.Sekisu);
                    sukei = sukei + data5.Sum(x => x.Suryou);
                    hokkei = hokkei + data5.Sum(x => x.Hokank);
                    niekei = niekei + data5.Sum(x => x.Niekik);
                }
                //合計(機種Aごと)
                #region nullデータ行作成
                var nulls = new SoukoHinCodList();
                nulls.Torcod = sdata.Torcod;
                nulls.Tornam1 = sdata.Tornam1;
                nulls.Tornam2 = sdata.Tornam2;
                nulls.Seicod = sdata.Seicod;
                nulls.Kisyua = sdata.Kisyua;
                nulls.Kisnam = sdata.Kisnam;
                nulls.Hincod = string.Empty;
                nulls.Zansuu = null;
                nulls.Nyuksu = null;
                nulls.Sekisu = null;
                nulls.Hokank = null;
                nulls.Niekik = null;
                nulls.Goukei = null;
                nulls.Cntpg = pgCnt;
                nulls.Maxpg = maxCnt;
                result.Add(nulls);
                #endregion
                var gokei = new SoukoHinCodList();
                gokei.Torcod = sdata.Torcod;
                gokei.Tornam1 = sdata.Tornam1;
                gokei.Tornam2 = sdata.Tornam2;
                gokei.Seicod = sdata.Seicod;
                gokei.Kisyua = sdata.Kisyua;
                gokei.Kisnam = sdata.Kisnam;
                gokei.Hincod = "【　合　計　】";
                gokei.Zansuu = zankei;
                gokei.Nyuksu = nyukei;
                gokei.Sekisu = sekikei;
                gokei.Hokank = hokkei;
                gokei.Niekik = niekei;
                gokei.Suryou = sukei;
                gokei.Goukei = hokkei + niekei;
                gokei.Cntpg = pgCnt;
                gokei.Maxpg = maxCnt;
                result.Add(gokei);
            }

            result.OrderBy(a => a.Seicod)
               .ThenBy(a => a.Kisyua)
               .ThenBy(a => a.Kisnam)
               .ThenBy(a => a.Hinnmk);
            return result;
        }

        //倉庫明細書(倉庫使用料単位)
        public async Task<IEnumerable<SoukoSiyouryoList>> SoukoSiyouryoList()
        {
            List<SoukoSiyouryoList> result = new List<SoukoSiyouryoList>();

            //機種Aごとのデータ取得
            var status = await dbContext.THokanSeikyu
            .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
            .Where(x => (x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C") && x.a.Hokflg4 == "Y")
            .Where(x => x.a.Hokcod != "P1")
            .Where(x => x.a.Hokcod != "P2")
            .Where(x => x.a.Kisyub == "996" | x.a.Kisyub == "997" | x.a.Kisyub == "998" | x.a.Kisyub == "999")
            .Where(x => x.a.Hokcod == "HO" | x.a.Hokcod == "NI" | x.a.Hokcod == "NI" | x.a.Hokcod == "NE")
            .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam, x.a.Nieflg3 })
            .Select(x => new SoukoSiyouryoList()
            {
                Torcod = x.Key.Fbtcod,
                Tornam1 = x.Key.Tornam,
                Seicod = x.Key.Seicod,
                Kisyua = x.Key.Kisyua,
                Kisnam = x.Key.Kisnam,
                Suryoum = x.Key.Nieflg3 == "N" ? "扱い数" : "伝票数",
            })
            .OrderBy(x => x.Seicod)
            .ThenBy(x => x.Kisyua)
            .ThenBy(x => x.Kisnam)
            .ToListAsync();

            var seiList = status.GroupBy(x => x.Seicod)
                .Where(g => g.Count() > 1)
                .Select(g => new { seiCod = g.Key, maxCnt = g.Count() }).ToList();
            //重複データList 作成
            List<string> setList = new List<string>();
            List<int> cnt = seiList.Select(a => a.maxCnt).ToList();
            if (seiList.Count() != 0)
            {
                foreach (var data in seiList)
                {
                    int index = seiList.FindIndex(x => x.seiCod == data.seiCod);
                    if (index >= 0)
                    {
                        for (int i = 0; i < cnt[index]; i++)
                        {
                            setList.Add(data.seiCod);
                        }
                    }

                }

            }
            var pgMax = status.GroupBy(x => x.Seicod).Count();
            var setCnt = 0;
            var ck = 0;
            var pgCnt = 0;

            string seicod = setList.Count() != 0 ? setList[0] : string.Empty;
            string seicodCP = string.Join("&", status.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(0).Take(1));

            foreach (var sdata in status)
            {
                #region 合計用変数
                decimal? zankei = 0;
                decimal? nyukei = 0;
                decimal? sekikei = 0;
                decimal? hokkei = 0;
                decimal? niekei = 0;
                decimal? sukei = 0;
                #endregion
                ck += 1;

                #region 頁数取得
                //現在ページ数取得
                if (setList.Count() != 0)
                {
                    if (seicod != sdata.Seicod)
                    {
                        pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                    }
                    else
                    {
                        if (seicodCP != sdata.Seicod)
                        {
                            pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                            seicodCP = string.Join("&", status.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(ck - 1).Take(1));
                        }
                        else
                        {
                            pgCnt = pgCnt == 0 ? 1 : pgCnt;
                        }
                        setCnt += 1;
                        if (setCnt < setList.Count())
                        {
                            seicod = setList[setCnt];
                        }

                    }
                }
                else
                {
                    pgCnt = pgCnt == 0 ? 1 : pgCnt + 1;
                }
                #endregion


                //取引先文字算出
                string tornam = sdata.Tornam1;
                sdata.Tornam1 = tornam.Length >= 20 ? tornam.Substring(20) : tornam;
                sdata.Tornam2 = tornam.Length >= 20 ? tornam.Substring(0, 20) : string.Empty;

                //倉庫使用料明細
                var misai = await dbContext.THokanSeikyu
                .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                .Where(x => x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C")
                .Where(x => x.a.Hokflg4 == "Y")
                .Where(x => x.a.Kisyub == "996" )
                .Where(x => x.a.Hokcod == "HO")
                .Where(x => x.a.Seicod == sdata.Seicod)
               .Where(x => x.a.Kisyua == sdata.Kisyua)
                .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam })
                .Select(x => new SoukoSiyouryoList()
                {
                    Torcod = x.Key.Fbtcod,
                    Tornam1 = sdata.Tornam1,
                    Tornam2 = sdata.Tornam2,
                    Seicod = x.Key.Seicod,
                    Kisyua = x.Key.Kisyua,
                    Kisnam = x.Key.Kisnam,
                    Suryoum =sdata.Suryoum,
                    Zansuu = x.Sum(a => a.a.Zansuu),
                    Nyuksu = x.Sum(a => a.a.Nyuksu),
                    Sekisu = x.Sum(a => a.a.Sekisu),
                    Hokant = x.Sum(a => a.a.Hokant),
                    Hokank = x.Sum(a => a.a.Hokank),
                    Niekit = null,
                    Niekik = null,
                    Suryou = null,
                    HincodStr = "《　倉庫使用料　》",
                    Goukei = x.Sum(a => a.a.Hokank),
                    //Cntpg = pgCnt,
                    Cntpg = 1,
                    //Maxpg = pgMax
                    Maxpg = 1

                }).ToListAsync();

                if (misai.Count() != 0)
                {
                    result.AddRange(misai);
                    zankei += misai.Sum(x => x.Zansuu);
                    nyukei += misai.Sum(x => x.Nyuksu);
                    sukei += misai.Sum(x => x.Suryou);
                    sekikei += misai.Sum(x => x.Sekisu);
                    hokkei += misai.Sum(x => x.Hokank);
                    niekei += misai.Sum(x => x.Niekik);
                }

                //荷役料
                var nieki = await dbContext.THokanSeikyu
                .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                .Where(x => x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C")
                .Where(x => x.a.Hokflg4 == "Y")
                .Where(x => x.a.Kisyub == "997")
                .Where(x => x.a.Hokcod == "NI")
                .Where(x => x.a.Seicod == sdata.Seicod)
               .Where(x => x.a.Kisyua == sdata.Kisyua)
                .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam })
                .Select(x => new SoukoSiyouryoList()
                {
                    Torcod = x.Key.Fbtcod,
                    Tornam1 = sdata.Tornam1,
                    Tornam2 = sdata.Tornam2,
                    Seicod = x.Key.Seicod,
                    Kisyua = x.Key.Kisyua,
                    Kisnam = x.Key.Kisnam,
                    Suryoum = sdata.Suryoum,
                    Zansuu = null,
                    Nyuksu = null,
                    Sekisu = null,
                    Hokant = null,
                    Hokank = null,
                    Niekit = x.Sum(a => a.a.Niekit),
                    Niekik = x.Sum(a => a.a.Niekik),
                    Suryou = x.Sum(a => sdata.Suryoum == "扱い数" ? a.a.Atukai : a.a.Densuu),
                    HincodStr = "《　荷役料　》",
                    Goukei = x.Sum(a => a.a.Niekik),
                    //Cntpg = pgCnt,
                    Cntpg = 1,
                    //Maxpg = pgMax
                    Maxpg = 1
                }).ToListAsync();

                if (nieki.Count() != 0)
                {
                    result.AddRange(nieki);
                    zankei += nieki.Sum(x => x.Zansuu);
                    nyukei += nieki.Sum(x => x.Nyuksu);
                    sukei += nieki.Sum(x => x.Suryou);
                    sekikei += nieki.Sum(x => x.Sekisu);
                    hokkei += nieki.Sum(x => x.Hokank);
                    niekei += nieki.Sum(x => x.Niekik);

                }

                //休日分
                var kyzitu = await dbContext.THokanSeikyu
                .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                .Where(x => x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C")
                .Where(x => x.a.Hokflg4 == "Y")
                .Where(x => x.a.Kisyub == "998")
                .Where(x => x.a.Hokcod == "NI")
                .Where(x => x.a.Seicod == sdata.Seicod)
               .Where(x => x.a.Kisyua == sdata.Kisyua)
                .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam })
                .Select(x => new SoukoSiyouryoList()
                {
                    Torcod = x.Key.Fbtcod,
                    Tornam1 = sdata.Tornam1,
                    Tornam2 = sdata.Tornam2,
                    Seicod = x.Key.Seicod,
                    Kisyua = x.Key.Kisyua,
                    Kisnam = x.Key.Kisnam,
                    Suryoum = sdata.Suryoum,
                    Zansuu = null,
                    Nyuksu = null,
                    Sekisu = null,
                    Hokant = null,
                    Hokank = null,
                    Niekit = x.Sum(a => a.a.Niekit),
                    Niekik = x.Sum(a => a.a.Niekik),
                    Suryou = x.Sum(a => sdata.Suryoum == "扱い数" ? a.a.Atukai : a.a.Densuu),
                    HincodStr = "《　休日分　》",
                    Goukei = x.Sum(a => a.a.Niekik),
                    //Cntpg = pgCnt,
                    Cntpg = 1,
                    //Maxpg = pgMax
                    Maxpg = 1
                }).ToListAsync();

                if (kyzitu.Count() != 0)
                {
                    result.AddRange(kyzitu);
                    zankei += kyzitu.Sum(x => x.Zansuu);
                    nyukei += kyzitu.Sum(x => x.Nyuksu);
                    sukei += kyzitu.Sum(x => x.Suryou);
                    sekikei += kyzitu.Sum(x => x.Sekisu);
                    hokkei += kyzitu.Sum(x => x.Hokank);
                    niekei += kyzitu.Sum(x => x.Niekik);
                }

                //値引き
                var nbiki = await dbContext.THokanSeikyu
                .Join(dbContext.MTorihikisaki, x => x.Seicod, x => x.Torcod, (a, c1) => new { a, c1 })
                .Where(x => x.a.Hokflg1 == "C" | x.a.Nieflg1 == "C")
                .Where(x => x.a.Hokflg4 == "Y")
                .Where(x => x.a.Kisyub == "999")
                .Where(x => x.a.Hokcod == "NE")
                .Where(x => x.a.Seicod == sdata.Seicod)
               .Where(x => x.a.Kisyua == sdata.Kisyua)
                .GroupBy(x => new { x.c1.Fbtcod, x.c1.Tornam, x.a.Seicod, x.a.Kisyua, x.a.Kisnam })
                .Select(x => new SoukoSiyouryoList()
                {
                    Torcod = x.Key.Fbtcod,
                    Tornam1 = sdata.Tornam1,
                    Tornam2 = sdata.Tornam2,
                    Seicod = x.Key.Seicod,
                    Kisyua = x.Key.Kisyua,
                    Kisnam = x.Key.Kisnam,
                    Suryoum = sdata.Suryoum,
                    Zansuu = null,
                    Nyuksu = null,
                    Sekisu = null,
                    Hokant = null,
                    Hokank = x.Sum(a => a.a.Hokank * -1),
                    Niekit = null,
                    Niekik = x.Sum(a => a.a.Niekik * -1),
                    Suryou = null,
                    HincodStr = "《　値引き　》",
                    Goukei = x.Sum(a => a.a.Hokank * -1 + a.a.Niekik * -1),
                    //Cntpg = pgCnt,
                    Cntpg = 1,
                    //Maxpg = pgMax
                    Maxpg = 1
                }).ToListAsync();

                if (nbiki.Count() != 0)
                {
                    #region nullデータ作成
                    var nulldata = await dbContext.THokanSeikyu
                        .Select(x => new SoukoSiyouryoList()
                        {
                            Torcod = sdata.Torcod,
                            Tornam1 = sdata.Tornam1,
                            Tornam2 = sdata.Tornam2,
                            Seicod = sdata.Seicod,
                            Kisyua = sdata.Kisyua,
                            Kisnam = sdata.Kisnam,
                            Suryoum = sdata.Suryoum,
                            Zansuu = null,
                            Nyuksu = null,
                            Sekisu = null,
                            Hokant = null,
                            Hokank = null,
                            Niekit = null,
                            Niekik = null,
                            Suryou = null,
                            HincodStr = string.Empty,
                            Goukei = null,
                            Cntpg = pgCnt,
                            Maxpg = pgMax
                        }).FirstOrDefaultAsync();
                    result.Add(nulldata);
                    #endregion
                    result.AddRange(nbiki);
                    hokkei += nbiki.Sum(x => x.Hokank);
                    niekei += nbiki.Sum(x => x.Niekik);
                }
                if (misai.Count() != 0 | nieki.Count() != 0 | kyzitu.Count() != 0 | nbiki.Count() != 0)
                {
                    #region nullデータ作成
                    var nulldata = await dbContext.THokanSeikyu
                        .Select(x => new SoukoSiyouryoList()
                        {
                            Torcod = sdata.Torcod,
                            Tornam1 = sdata.Tornam1,
                            Tornam2 = sdata.Tornam2,
                            Seicod = sdata.Seicod,
                            Kisyua = sdata.Kisyua,
                            Kisnam = sdata.Kisnam,
                            Suryoum = sdata.Suryoum,
                            Zansuu = null,
                            Nyuksu = null,
                            Sekisu = null,
                            Hokant = null,
                            Hokank = null,
                            Niekit = null,
                            Niekik = null,
                            Suryou = null,
                            HincodStr = string.Empty,
                            Goukei = null,
                            Cntpg = pgCnt,
                            Maxpg = pgMax
                        }).FirstOrDefaultAsync();
                    result.Add(nulldata);
                    #endregion

                    //合計(機種Aごと)
                    var gokei = new SoukoSiyouryoList();
                    gokei.Torcod = sdata.Torcod;
                    gokei.Tornam1 = sdata.Tornam1;
                    gokei.Tornam2 = sdata.Tornam2;
                    gokei.Seicod = sdata.Seicod;
                    gokei.Kisyua = sdata.Kisyua;
                    gokei.Kisnam = sdata.Kisnam;
                    gokei.HincodStr = "【合　　計】";
                    gokei.Zansuu = zankei;
                    gokei.Nyuksu = nyukei;
                    gokei.Sekisu = sekikei;
                    gokei.Hokank = hokkei;
                    gokei.Niekik = niekei;
                    gokei.Suryou = sukei;
                    gokei.Goukei = hokkei + niekei;
                    //gokei.Cntpg = pgCnt;
                    gokei.Cntpg = 1;
                    //gokei.Maxpg = pgMax;
                    gokei.Maxpg = 1;
                    result.Add(gokei);
                }
            }
            result.OrderBy(a => a.Seicod)
                    .ThenBy(a => a.Kisyua)
                    .ThenBy(a => a.Kisnam);
            return result;

        }

        // Fe保管請求拠点別データ
        public async Task<IEnumerable<KyotenbetuList>> KyotenbetuList()
        {
            var kyotenList = await dbContext.THokanSeikyuKyoten
                                        .Select(x => new KyotenbetuList
                                        {
                                            Basyo = x.Basyo,
                                            Basnam = x.Basnam,
                                            Kisyua = x.Kisyua,
                                            Kisyub = x.Kisyub,
                                            Kisybn = x.Kisybn,
                                            Seicod = x.Seicod,
                                            Zansuu = x.Zansuu,
                                            Zankin = x.Zankin,
                                            Nyuksu = x.Nyuksu,
                                            Nyukin = x.Nyukin,
                                            Syksuu = x.Syksuu,
                                            Sykkin = x.Sykkin,
                                            Zaiksu = x.Zaiksu,
                                            Zaikin = x.Zaikin,
                                            Densuu = x.Densuu,
                                            Densky = x.Densky,
                                            Hokank = x.Hokank,
                                            Niekik = x.Niekik,
                                            Niekyj = x.Niekyj,
                                            Pccodh = x.Pccodh,
                                            Pccodn = x.Pccodn,
                                            Dataym = x.Dataym,
                                        })
                                        .OrderBy(x => x.Basyo)
                                        .ThenBy(x => x.Kisyua)
                                        .ThenBy(x => x.Kisyub)
                                        .ToListAsync();

            #region 変数
            int cnt = 0;
            string basyo = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Basyo, item.Basyo)).Skip(0).Take(1));
            string kisyua = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Kisyua, item.Kisyua)).Skip(0).Take(1));
            string basnam = string.Empty;
            string seicod = string.Empty;
            string pchkh = string.Empty;
            string pchkn = string.Empty;
            string nentuki = string.Empty;
            #endregion

            List<KyotenbetuList> result = new List<KyotenbetuList>();
            foreach (var status in kyotenList)
            {
                basnam = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Basnam, item.Basnam)).Skip(cnt - 1).Take(1));
                seicod = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(cnt - 1).Take(1));
                pchkh = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Pccodh, item.Pccodh)).Skip(cnt - 1).Take(1));
                pchkn = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Pccodn, item.Pccodn)).Skip(cnt - 1).Take(1));
                nentuki = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Dataym, item.Dataym)).Skip(cnt - 1).Take(1));

                //機種計
                if (status.Kisyua != kisyua)
                {

                    // 機種Aが変わった場合でPCコードが入っていなかった場合
                    // あるまでさかのぼってセットする
                    if (pchkh == "" || pchkn == "")
                    {
                        var pcCodes = await dbContext.THokanSeikyuKyoten
                                .Where(x => x.Basyo == basyo)
                                .Where(x => x.Kisyua == kisyua)
                                .Where(x => x.Pccodn != "" || x.Pccodh != "")
                                .GroupBy(x => new { x.Pccodh, x.Pccodn })
                                .Select(x => new
                                {
                                    Pccodh = x.Key.Pccodh,
                                    Pccodn = x.Key.Pccodn
                                }).ToArrayAsync();
                        if (pcCodes.Count() != 0)
                        {
                            var pcCode = pcCodes.First();
                            if (pchkh == "")
                            {
                                pchkh = pcCode.Pccodh;
                            }
                            if (pchkn == "")
                            {
                                pchkn = pcCode.Pccodn;
                            }
                        }
                    }

                    var kisyulist = await dbContext.THokanSeikyuKyoten
                        .Where(x => x.Basyo == basyo)
                        .Where(x => x.Kisyua == kisyua)
                        .GroupBy(x => new {x.Kisyua })
                        .Select( x => new KyotenbetuList
                        {
                            Basyo = basyo,
                            Basnam = basnam,
                            Kisyua = kisyua,
                            Kisyub = "機種計",
                            Kisybn = string.Empty,
                            Seicod = seicod,
                            Zansuu = x.Sum(a => a.Zansuu),
                            Zankin = x.Sum(a => a.Zankin),
                            Nyuksu = x.Sum(a => a.Nyuksu),
                            Nyukin = x.Sum(a => a.Nyukin),
                            Syksuu = x.Sum(a => a.Syksuu),
                            Sykkin = x.Sum(a => a.Sykkin),
                            Zaiksu = x.Sum(a => a.Zaiksu),
                            Zaikin = x.Sum(a => a.Zaikin),
                            Densuu = x.Sum(a => a.Densuu),
                            Densky = x.Sum(a => a.Densky),
                            Hokank = x.Sum(a => a.Hokank),
                            Niekik = x.Sum(a => a.Niekik),
                            Niekyj = x.Sum(a => a.Niekyj),
                            Pccodh = pchkh,
                            Pccodn = pchkn,
                            Dataym = nentuki,
                        }).ToListAsync();
                    if(kisyulist.Count() != 0)
                    {
                        var data1 = kisyulist.First();
                        result.Add(data1);
                    }
                    kisyua = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Kisyua, item.Kisyua)).Skip(cnt).Take(1));
                }
                if (status.Basyo != basyo)
                {
                    var basyolist = await dbContext.THokanSeikyuKyoten
                        .Where(x => x.Basyo == basyo)
                        .GroupBy(x => new { x.Basyo })
                        .Select(x => new KyotenbetuList
                        {
                            Basyo = basyo,
                            Basnam = basnam,
                            Kisyua = "ZZ",
                            Kisyub = "場所計",
                            Kisybn = string.Empty,
                            Seicod = string.Empty,
                            Zansuu = x.Sum(a => a.Zansuu),
                            Zankin = x.Sum(a => a.Zankin),
                            Nyuksu = x.Sum(a => a.Nyuksu),
                            Nyukin = x.Sum(a => a.Nyukin),
                            Syksuu = x.Sum(a => a.Syksuu),
                            Sykkin = x.Sum(a => a.Sykkin),
                            Zaiksu = x.Sum(a => a.Zaiksu),
                            Zaikin = x.Sum(a => a.Zaikin),
                            Densuu = x.Sum(a => a.Densuu),
                            Densky = x.Sum(a => a.Densky),
                            Hokank = x.Sum(a => a.Hokank),
                            Niekik = x.Sum(a => a.Niekik),
                            Niekyj = x.Sum(a => a.Niekyj),
                            Pccodh = string.Empty,
                            Pccodn = string.Empty,
                            Dataym = nentuki,
                        }).ToListAsync();
                    if(basyolist.Count() != 0)
                    {
                        var data2 = basyolist.First();
                        result.Add(data2);
                    }
                    basyo = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Basyo, item.Basyo)).Skip(cnt).Take(1));
                }
                result.Add(status);
                cnt += 1;
            }
            cnt -= 1;
            //最終データの合計
            basyo = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Basyo, item.Basyo)).Skip(cnt).Take(1));
            kisyua = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Kisyua, item.Kisyua)).Skip(cnt).Take(1));
            basnam = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Basnam, item.Basnam)).Skip(cnt ).Take(1));
            seicod = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Seicod, item.Seicod)).Skip(cnt).Take(1));
            pchkh = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Pccodh, item.Pccodh)).Skip(cnt).Take(1));
            pchkn = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Pccodn, item.Pccodn)).Skip(cnt).Take(1));
            nentuki = string.Join("&", kyotenList.Select(item => string.Format("{1}", item.Dataym, item.Dataym)).Skip(cnt ).Take(1));
            var kisyulist2 = await dbContext.THokanSeikyuKyoten
                        .Where(x => x.Basyo == basyo)
                        .Where(x => x.Kisyua == kisyua)
                        .GroupBy(x => new {x.Kisyua })
                        .Select( x => new KyotenbetuList
                        {
                            Basyo = basyo,
                            Basnam = basnam,
                            Kisyua = kisyua,
                            Kisyub = "機種計",
                            Kisybn = string.Empty,
                            Seicod = seicod,
                            Zansuu = x.Sum(a => a.Zansuu),
                            Zankin = x.Sum(a => a.Zankin),
                            Nyuksu = x.Sum(a => a.Nyuksu),
                            Nyukin = x.Sum(a => a.Nyukin),
                            Syksuu = x.Sum(a => a.Syksuu),
                            Sykkin = x.Sum(a => a.Sykkin),
                            Zaiksu = x.Sum(a => a.Zaiksu),
                            Zaikin = x.Sum(a => a.Zaikin),
                            Densuu = x.Sum(a => a.Densuu),
                            Densky = x.Sum(a => a.Densky),
                            Hokank = x.Sum(a => a.Hokank),
                            Niekik = x.Sum(a => a.Niekik),
                            Niekyj = x.Sum(a => a.Niekyj),
                            Pccodh = pchkh,
                            Pccodn = pchkn,
                            Dataym = nentuki,
                        }).ToListAsync();
            if(kisyulist2.Count() != 0)
            {
                var data1 = kisyulist2.First();
                result.Add(data1);
            }
            var basyolist2 = await dbContext.THokanSeikyuKyoten
                .Where(x => x.Basyo == basyo)
                .GroupBy(x => new { x.Basyo })
                .Select(x => new KyotenbetuList
                {
                    Basyo = basyo,
                    Basnam = basnam,
                    Kisyua = "ZZ",
                    Kisyub = "場所計",
                    Kisybn = string.Empty,
                    Seicod = string.Empty,
                    Zansuu = x.Sum(a => a.Zansuu),
                    Zankin = x.Sum(a => a.Zankin),
                    Nyuksu = x.Sum(a => a.Nyuksu),
                    Nyukin = x.Sum(a => a.Nyukin),
                    Syksuu = x.Sum(a => a.Syksuu),
                    Sykkin = x.Sum(a => a.Sykkin),
                    Zaiksu = x.Sum(a => a.Zaiksu),
                    Zaikin = x.Sum(a => a.Zaikin),
                    Densuu = x.Sum(a => a.Densuu),
                    Densky = x.Sum(a => a.Densky),
                    Hokank = x.Sum(a => a.Hokank),
                    Niekik = x.Sum(a => a.Niekik),
                    Niekyj = x.Sum(a => a.Niekyj),
                    Pccodh = string.Empty,
                    Pccodn = string.Empty,
                    Dataym = nentuki,
                }).ToListAsync();
            if(basyolist2.Count() != 0)
            {
                var data2 = basyolist2.First();
                result.Add(data2);
            }

            return result;
        }

        // FPS様保管請求拠点別データ
        public async Task<IEnumerable<THokanSeikyuKyoten>> FpsKyotenExcel()
        {

            List<THokanSeikyuKyoten> kyotenL = new List<THokanSeikyuKyoten>();

            List<THokanSeikyuKyoten> kyotenList = await dbContext.THokanSeikyuKyoten
                        .OrderBy(x => x.Basyo)
                        .ThenBy(x => x.Kisyua)
                        .ThenBy(x => x.Kisyub)
                        .ToListAsync();

            foreach(THokanSeikyuKyoten kyoten in kyotenList)
            {

                string pdcodeh = DataUtil.SubstringEx(kyoten.Pccodh, 8, 4);
                string pdcoden = DataUtil.SubstringEx(kyoten.Pccodn, 8, 4);

                if (pdcodeh == "1158" || pdcoden == "1158")
                {
                    kyotenL.Add(kyoten);
                }
            }

            return kyotenL;
        }

    } 
}