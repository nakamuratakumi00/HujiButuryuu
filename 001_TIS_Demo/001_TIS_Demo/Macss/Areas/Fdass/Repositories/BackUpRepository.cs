using Macss.Areas.Fdass.Common;
using Macss.Areas.Fdass.Models;
using Macss.Models;
using Macss.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Fdass.Repositories
{
    public class BackUpRepository: IBackUpRepository
    {
        private readonly ApplicationDB dbContext;
        private LogService logService;
        private ILogRepository logRepository;

        public BackUpRepository(ApplicationDB db)
        {
            this.dbContext = db;
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        public async Task<bool> BackUpData(string month, string prossesingId, string menuName, string loginUser, System.Web.Mvc.ModelStateDictionary modelError)
        {

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = false;

                    // 入出庫データ
                    IOrderedQueryable<THokanNyuushuuko> tNyuushuuko = dbContext.THokanNyuushuuko.OrderBy(x => x.Hincod);
                    foreach (THokanNyuushuuko nData in tNyuushuuko)
                    {
                        THokanRirekiNyuushuuko rData = new THokanRirekiNyuushuuko
                        {
                            Month = month,
                            Kisyua = nData.Kisyua,
                            Kisyub = nData.Kisyub,
                            Hokcod = nData.Hokcod,
                            Hincod = nData.Hincod,
                            Zansuu = nData.Zansuu,
                            Zankin = nData.Zankin,
                            Siksuu = nData.Siksuu,
                            Sikkin = nData.Sikkin,
                            Kaisuu = nData.Kaisuu,
                            Kaikin = nData.Kaikin,
                            Syksuu = nData.Syksuu,
                            Sykkin = nData.Sykkin,
                            Dkbn = nData.Dkbn,
                            Seikyu = nData.Seikyu,
                            Baitai = nData.Baitai,
                            Touzan = nData.Touzan,
                            Dtmoto = nData.Dtmoto,
                            Crtcod = nData.Crtcod,
                            Crtymd = nData.Crtymd,
                            Updcod = nData.Updcod,
                            Updymd = nData.Updymd
                        };
                        dbContext.THokanRirekiNyuushuuko.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 入出庫繰越データ
                    IOrderedQueryable<THokanNyuushuukoKurikosi> tNyuushuukoKurikosi = dbContext.THokanNyuushuukoKurikosi.OrderBy(x => x.Hincod);
                    foreach (THokanNyuushuukoKurikosi nData in tNyuushuukoKurikosi)
                    {
                        THokanRirekiNyuushuukoKurikosi rData = new THokanRirekiNyuushuukoKurikosi
                        {
                            Month = month,
                            Kurymd = nData.Kurymd,
                            Kisyua = nData.Kisyua,
                            Kisyub = nData.Kisyub,
                            Hincod = nData.Hincod,
                            Hokcod = nData.Hokcod,
                            Zansuu = nData.Zansuu,
                            Zankin = nData.Zankin,
                            Siksuu = nData.Siksuu,
                            Sikkin = nData.Sikkin,
                            Kaisuu = nData.Kaisuu,
                            Kaikin = nData.Kaikin,
                            Syksuu = nData.Syksuu,
                            Sykkin = nData.Sykkin,
                            Dkbn = nData.Dkbn,
                            Seikyu = nData.Seikyu,
                            Baitai = nData.Baitai,
                            Touzan = nData.Touzan,
                            Crtcod = nData.Crtcod,
                            Crtymd = nData.Crtymd,
                            Updcod = nData.Updcod,
                            Updymd = nData.Updymd
                        };
                        dbContext.THokanRirekiNyuushuukoKurikosi.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 伝票件数データ
                    IOrderedQueryable<THokanDenpyokensu> tDenpyokensu = dbContext.THokanDenpyokensu.OrderBy(x => x.Hincod);
                    foreach (THokanDenpyokensu dData in tDenpyokensu)
                    {
                        THokanRirekiDenpyokensu rData = new THokanRirekiDenpyokensu
                        {
                            Month = month,
                            Kisyua = dData.Kisyua,
                            Kisyub = dData.Kisyub,
                            Basyo = dData.Basyo,
                            Denkub = dData.Denkub,
                            Densuu = dData.Densuu,
                            Seikyu = dData.Seikyu,
                            Inpymd = dData.Inpymd,
                            Keiymd = dData.Keiymd,
                            Hincod = dData.Hincod,
                            Nskosu = dData.Nskosu,
                            Eigsok = dData.Eigsok,
                            Dtmoto = dData.Dtmoto,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.THokanRirekiDenpyokensu.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 伝票件数繰越データ
                    IOrderedQueryable<THokanDenpyokensuKurikosi> tDenpyokensuKurikosi = dbContext.THokanDenpyokensuKurikosi.OrderBy(x => x.Hincod);
                    foreach (THokanDenpyokensuKurikosi dData in tDenpyokensuKurikosi)
                    {
                        THokanRirekiDenpyokensuKurikosi rData = new THokanRirekiDenpyokensuKurikosi
                        {
                            Month = month,
                            Kurymd = dData.Kurymd,
                            Kisyua = dData.Kisyua,
                            Kisyub = dData.Kisyub,
                            Basyo = dData.Basyo,
                            Denkub = dData.Denkub,
                            Densuu = dData.Densuu,
                            Seikyu = dData.Seikyu,
                            Inpymd = dData.Inpymd,
                            Keiymd = dData.Keiymd,
                            Hincod = dData.Hincod,
                            Nskosu = dData.Nskosu,
                            Eigsok = dData.Eigsok,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.THokanRirekiDenpyokensuKurikosi.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求請求データ
                    IOrderedQueryable<THokanSeikyu> tSeikyu = dbContext.THokanSeikyu.OrderBy(x => x.Hincod);
                    foreach (THokanSeikyu dData in tSeikyu)
                    {
                        THokanRirekiSeikyu rData = new THokanRirekiSeikyu
                        {
                            Month = month,
                            Seicod = dData.Seicod,
                            Kisyua = dData.Kisyua,
                            Kisyub = dData.Kisyub,
                            Kisnam = dData.Kisnam,
                            Hincod = dData.Hincod,
                            Hokcod = dData.Hokcod,
                            Hinnmk = dData.Hinnmk,
                            Zansuu = dData.Zansuu,
                            Nyuksu = dData.Nyuksu,
                            Syksuu = dData.Syksuu,
                            Sekisu = dData.Sekisu,
                            Hokant = dData.Hokant,
                            Hokank = dData.Hokank,
                            Hokankr = dData.Hokankr,
                            Atukai = dData.Atukai,
                            Densuu = dData.Densuu,
                            Niekit = dData.Niekit,
                            Niekik = dData.Niekik,
                            Niekikr = dData.Niekikr,
                            Hokflg1 = dData.Hokflg1,
                            Hokflg2 = dData.Hokflg2,
                            Hokflg3 = dData.Hokflg3,
                            Hokflg4 = dData.Hokflg4,
                            Hokflg5 = dData.Hokflg5,
                            Nieflg1 = dData.Nieflg1,
                            Nieflg2 = dData.Nieflg2,
                            Nieflg3 = dData.Nieflg3,
                            Nieflg4 = dData.Nieflg4,
                            Nieflg5 = dData.Nieflg5,
                            Pccod = dData.Pccod,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.THokanRirekiSeikyu.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求請求データB
                    IOrderedQueryable<THokanSeikyuB> tSeikyuB = dbContext.THokanSeikyuB.OrderBy(x => x.Hincod);
                    foreach (THokanSeikyuB dData in tSeikyuB)
                    {
                        THokanRirekiSeikyuB rData = new THokanRirekiSeikyuB
                        {
                            Month = month,
                            Seicod = dData.Seicod,
                            Kisyua = dData.Kisyua,
                            Kisyub = dData.Kisyub,
                            Kisnam = dData.Kisnam,
                            Hincod = dData.Hincod,
                            Hokcod = dData.Hokcod,
                            Hinnmk = dData.Hinnmk,
                            Zansuu = dData.Zansuu,
                            Nyuksu = dData.Nyuksu,
                            Syksuu = dData.Syksuu,
                            Sekisu = dData.Sekisu,
                            Hokant = dData.Hokant,
                            Hokank = dData.Hokank,
                            Hokankr = dData.Hokankr,
                            Atukai = dData.Atukai,
                            Densuu = dData.Densuu,
                            Niekit = dData.Niekit,
                            Niekik = dData.Niekik,
                            Niekikr = dData.Niekikr,
                            Hokflg1 = dData.Hokflg1,
                            Hokflg2 = dData.Hokflg2,
                            Hokflg3 = dData.Hokflg3,
                            Hokflg4 = dData.Hokflg4,
                            Hokflg5 = dData.Hokflg5,
                            Nieflg1 = dData.Nieflg1,
                            Nieflg2 = dData.Nieflg2,
                            Nieflg3 = dData.Nieflg3,
                            Nieflg4 = dData.Nieflg4,
                            Nieflg5 = dData.Nieflg5,
                            Pccod = dData.Pccod,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.THokanRirekiSeikyuB.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求拠点別データ
                    IOrderedQueryable<THokanSeikyuKyoten> tSeikyuKyoten = dbContext.THokanSeikyuKyoten.OrderBy(x => x.Basyo);
                    foreach (THokanSeikyuKyoten dData in tSeikyuKyoten)
                    {
                        THokanRirekiSeikyuKyoten rData = new THokanRirekiSeikyuKyoten
                        {
                            Month = month,
                            Basyo = dData.Basyo,
                            Basnam = dData.Basnam,
                            Kisyua = dData.Kisyua,
                            Kisyub = dData.Kisyub,
                            Kisybn = dData.Kisybn,
                            Seicod = dData.Seicod,
                            Zansuu = dData.Zansuu,
                            Zankin = dData.Zankin,
                            Siksuu = dData.Siksuu,
                            Sikkin = dData.Sikkin,
                            Kaisuu = dData.Kaisuu,
                            Kaikin = dData.Kaikin,
                            Nyuksu = dData.Nyuksu,
                            Nyukin = dData.Nyukin,
                            Syksuu = dData.Syksuu,
                            Sykkin = dData.Sykkin,
                            Zaiksu = dData.Zaiksu,
                            Zaikin = dData.Zaikin,
                            Densuu = dData.Densuu,
                            Densky = dData.Densky,
                            Hokank = dData.Hokank,
                            Niekik = dData.Niekik,
                            Niekyj = dData.Niekyj,
                            Pccodh = dData.Pccodh,
                            Pccodn = dData.Pccodn,
                            Dataym = dData.Dataym,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.THokanRirekiSeikyuKyoten.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求単価マスタ
                    IOrderedQueryable<MHokanTanka> nTanka = dbContext.MHokanTanka.OrderBy(x => x.Hincod);
                    foreach (MHokanTanka dData in nTanka)
                    {
                        MHokanRirekiTanka rData = new MHokanRirekiTanka
                        {
                            Month = month,
                            Hincod = dData.Hincod,
                            Sybcod = dData.Sybcod,
                            Hinnmk = dData.Hinnmk,
                            Kisyua = dData.Kisyua,
                            Kisyub = dData.Kisyub,
                            Tfulag = dData.Tfulag,
                            Frikae = dData.Frikae,
                            Jyukar = dData.Jyukar,
                            Syujyr = dData.Syujyr,
                            Fptank = dData.Fptank,
                            Hokant = dData.Hokant,
                            Hokymd = dData.Hokymd,
                            Hoktan = dData.Hoktan,
                            Niekit = dData.Niekit,
                            Nieymd = dData.Nieymd,
                            Nietan = dData.Nietan,
                            Osybcod = dData.Osybcod,
                            Ohinnmk = dData.Ohinnmk,
                            Okisyua = dData.Okisyua,
                            Okisyub = dData.Okisyub,
                            Ofrikae = dData.Ofrikae,
                            Ofptnk = dData.Ofptnk,
                            Ohokat = dData.Ohokat,
                            Oniekt = dData.Oniekt,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.MHokanRirekiTanka.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求契約マスタ
                    IOrderedQueryable<MHokanKeiyaku> mKeiyaku = dbContext.MHokanKeiyaku.OrderBy(x => x.Kisyua);
                    foreach (MHokanKeiyaku dData in mKeiyaku)
                    {
                        MHokanRirekiKeiyaku rData = new MHokanRirekiKeiyaku
                        {
                            Month = month,
                            Kisyua = dData.Kisyua,
                            Kisnam = dData.Kisnam,
                            Seitai = dData.Seitai,
                            Seinam = dData.Seinam,
                            Tanbsy = dData.Tanbsy,
                            Fbtcod = dData.Fbtcod,
                            Hokflg1 = dData.Hokflg1,
                            Hokflg2 = dData.Hokflg2,
                            Hokflg3 = dData.Hokflg3,
                            Hokflg4 = dData.Hokflg4,
                            Hokflg5 = dData.Hokflg5,
                            Hnebir = dData.Hnebir,
                            Hokant = dData.Hokant,
                            Nieflg1 = dData.Nieflg1,
                            Nieflg2 = dData.Nieflg2,
                            Nieflg3 = dData.Nieflg3,
                            Nieflg4 = dData.Nieflg4,
                            Nieflg5 = dData.Nieflg5,
                            Nieant = dData.Nieant,
                            Niekyt = dData.Niekyt,
                            Nnebir = dData.Nnebir,
                            Ojyukr = dData.Ojyukr,
                            Osyjyr = dData.Osyjyr,
                            Hjyukr = dData.Hjyukr,
                            Hsyjyr = dData.Hsyjyr,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.MHokanRirekiKeiyaku.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求条件マスタ
                    IOrderedQueryable<MHokanJouken> mJouken = dbContext.MHokanJouken.OrderBy(x => x.Sikcod);
                    foreach (MHokanJouken dData in mJouken)
                    {
                        MHokanRirekiJouken rData = new MHokanRirekiJouken
                        {
                            Month = month,
                            Sikcod = dData.Sikcod,
                            Jyoken = dData.Jyoken,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.MHokanRirekiJouken.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求請求先変更マスタ
                    IOrderedQueryable<MHokanSeikyuusakiChange> mSeikyuusakiChange = dbContext.MHokanSeikyuusakiChange.OrderBy(x => x.Seicod);
                    foreach (MHokanSeikyuusakiChange dData in mSeikyuusakiChange)
                    {
                        MHokanRirekiSeikyuusakiChange rData = new MHokanRirekiSeikyuusakiChange
                        {
                            Month = month,
                            Seicod = dData.Seicod,
                            Kisyua = dData.Kisyua,
                            Chgcod = dData.Chgcod,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.MHokanRirekiSeikyuusakiChange.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    // 保管請求部門設定用マスタ
                    IOrderedQueryable<MHokanBumon> mHokanBumon = dbContext.MHokanBumon.OrderBy(x => x.Kisyua);
                    foreach (MHokanBumon dData in mHokanBumon)
                    {
                        MHokanRirekiBumon rData = new MHokanRirekiBumon
                        {
                            Month  = month,
                            Kisyua = dData.Kisyua,
                            Basyo  = dData.Basyo,
                            Bmncod = dData.Bmncod,
                            Crtcod = dData.Crtcod,
                            Crtymd = dData.Crtymd,
                            Updcod = dData.Updcod,
                            Updymd = dData.Updymd
                        };
                        dbContext.MHokanRirekiBumon.Add(rData);
                    }
                    await dbContext.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (DbEntityValidationException dbEx)
                {
                    string errorMsg = DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                    transaction.Rollback();
                    EntityStateDetached();
                    return false;

                }
                catch (Exception ex)
                {
                    string error = ex.InnerException.ToString();
                    if (error.IndexOf("PRIMARY KEY") >= 0)
                    {
                        transaction.Rollback();
                        EntityStateDetached();
                        modelError.AddModelError(String.Empty, String.Format(Resources.Message.CE107));
                        return false;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                finally
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = true;
                }

            }

            StringBuilder count = new StringBuilder();
            var nyuushuuko = dbContext.THokanRirekiNyuushuuko.Where(x => x.Month == month);
            var nyuushuukoKurikosi = dbContext.THokanRirekiNyuushuukoKurikosi.Where(x => x.Month == month);
            var denpyokensu = dbContext.THokanRirekiDenpyokensu.Where(x => x.Month == month);
            var denpyokensuKurikosi = dbContext.THokanRirekiDenpyokensuKurikosi.Where(x => x.Month == month);
            var seikyu = dbContext.THokanRirekiSeikyu.Where(x => x.Month == month);
            var seikyuB = dbContext.THokanRirekiSeikyuB.Where(x => x.Month == month);
            var seikyuKyoten = dbContext.THokanRirekiSeikyuKyoten.Where(x => x.Month == month);
            var tanka = dbContext.MHokanRirekiTanka.Where(x => x.Month == month);
            var keiyaku = dbContext.MHokanRirekiKeiyaku.Where(x => x.Month == month);
            var jouken = dbContext.MHokanRirekiJouken.Where(x => x.Month == month);
            var seikyuusakiChange = dbContext.MHokanRirekiSeikyuusakiChange.Where(x => x.Month == month);
            var bumon = dbContext.MHokanRirekiBumon.Where(x => x.Month == month);


            count.Append("Fe入出庫データ " + nyuushuuko.Count() + "件作成\n");
            count.Append("Fe入出庫繰越データ " + nyuushuukoKurikosi.Count() + "件作成\n");
            count.Append("Fe伝票件数データ " + denpyokensu.Count() + "件作成\n");
            count.Append("Fe伝票件数繰越データ " + denpyokensuKurikosi.Count() + "件作成\n");
            count.Append("Fe保管請求請求データ " + seikyu.Count() + "件作成\n");
            count.Append("Fe保管請求請求データB " + seikyuB.Count() + "件作成\n");
            count.Append("Fe保管請求拠点別データ " + seikyuKyoten.Count() + "件作成\n");
            count.Append("Fe保管請求単価マスタ " + tanka.Count() + "件作成\n");
            count.Append("Fe保管請求契約マスタ " + keiyaku.Count() + "件作成\n");
            count.Append("Fe保管請求条件マスタ " + jouken.Count() + "件作成\n");
            count.Append("Fe保管請求請求先変更マスタ " + seikyuusakiChange.Count() + "件作成\n");
            count.Append("Fe保管請求部門設定用マスタ " + bumon.Count() + "件作成\n");
            logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.I1, menuName, count.ToString(), logRepository);

            return true;

        }

        private void EntityStateDetached()
        {

            List<System.Data.Entity.Infrastructure.DbEntityEntry> entries = dbContext.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
            foreach (System.Data.Entity.Infrastructure.DbEntityEntry dbEntityEntry in entries)
            {
                var entity = dbEntityEntry.Entity;

                if (entity == null) continue;
                if (dbEntityEntry.State == EntityState.Added)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }
        }

        public async Task<bool> Test(string month)
        {

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                dbContext.Configuration.ValidateOnSaveEnabled = false;

                // 入出庫データ
                IOrderedQueryable<THokanNyuushuuko> tNyuushuuko = dbContext.THokanNyuushuuko.OrderBy(x => x.Hincod);
                foreach (THokanNyuushuuko nData in tNyuushuuko)
                {
                    THokanRirekiNyuushuuko rData = new THokanRirekiNyuushuuko
                    {
                        Month = month,
                        Kisyua = nData.Kisyua,
                        Kisyub = nData.Kisyub,
                        Hokcod = nData.Hokcod,
                        Hincod = nData.Hincod,
                        Dkbn = nData.Dkbn,
                    };
                    dbContext.THokanRirekiNyuushuuko.Add(rData);
                    break;
                }
                await dbContext.SaveChangesAsync();

                transaction.Commit();
            }

            return true;
        }


    }
}