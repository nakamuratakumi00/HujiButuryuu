using Macss.Areas.Fdass.Common;
using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public class ShuukaRuisekiRepositorie : IShuukaRuisekiRepositorie
    {

        private readonly ApplicationDB dbContext;

        public ShuukaRuisekiRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<TUnsouShuukaJiseki2>> CheckShuukaRuisekiAsync(ShuukaRuisekiViewModel entry)
        {
            DateTime stDate = DateTime.Now;
            DateTime edDate = DateTime.Now;
            GetStEdData(entry.YyyyMm, ref stDate, ref edDate);
            string yymm = stDate.ToString("yyMM");
            return await dbContext.TUnsouShuukaJiseki2
                                        .Where(x => x.Dataym == yymm).ToListAsync();

        }

        public async Task UpdateShuukaRuisekiAsync(ShuukaRuisekiViewModel entry, string loginUser)
        {

            DateTime stDate = DateTime.Now;
            DateTime edDate = DateTime.Now;
            GetStEdData(entry.YyyyMm, ref stDate, ref edDate);

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // 当月データ取得
                    var shuukaJisekiList1 = await dbContext.TUnsouShuukaJiseki1

                        .Where(x => x.Sykymd <= edDate)
                        .Where((x => ((x.Geppou == "1" || x.Geppou == "2") || x.Delflg == "1"))).ToListAsync();

                    // 累積データ登録
                    foreach (var jiseki1 in shuukaJisekiList1)
                    {

                        string dataym = stDate.ToString("yyMM");

                        // 重複チェック
                        var sheckList = await dbContext.TUnsouShuukaJiseki2
                                .Where(x => x.Syukno == jiseki1.Syukno)
                                .Where(x => x.Dataym == jiseki1.Dataym)
                                .Where(x => x.Sykymd >= stDate)
                                .ToListAsync();

                        // データ削除
                        dbContext.TUnsouShuukaJiseki2.RemoveRange(sheckList);

                        TUnsouShuukaJiseki2 jiseki2 = new TUnsouShuukaJiseki2
                        {
                            Syukno = jiseki1.Syukno,
                            Dataym = dataym,
                            Sykymd = jiseki1.Sykymd,
                            Kisyu = jiseki1.Kisyu,
                            Keifno = jiseki1.Keifno,
                            Fsykno = jiseki1.Fsykno,
                            Tancod = jiseki1.Tancod,
                            Tannam = jiseki1.Tannam,
                            Tdkcod = jiseki1.Tdkcod,
                            Tdkjyu = jiseki1.Tdkjyu,
                            Tdknam = jiseki1.Tdknam,
                            Tdsnam = jiseki1.Tdsnam,
                            Tdbnam = jiseki1.Tdbnam,
                            Tdktan = jiseki1.Tdktan,
                            Tdktel = jiseki1.Tdktel,
                            Tdkyub = jiseki1.Tdkyub,
                            Hincod = jiseki1.Hincod,
                            Hinnam = jiseki1.Hinnam,
                            Unscod = jiseki1.Unscod,
                            Unscrs = jiseki1.Unscrs,
                            Sircod = jiseki1.Sircod,
                            Sgenno = jiseki1.Sgenno,
                            Unskbn = jiseki1.Unskbn,
                            Sybcod = jiseki1.Sybcod,
                            Tokcod = jiseki1.Tokcod,
                            Seicod = jiseki1.Seicod,
                            Denkbn = jiseki1.Denkbn,
                            Denmsu = jiseki1.Denmsu,
                            Tkjiko = jiseki1.Tkjiko,
                            Hososu = jiseki1.Hososu,
                            Nfdate = jiseki1.Nfdate,
                            Daihno = jiseki1.Daihno,
                            Daihnoym = jiseki1.Daihnoym,
                            Jyuryo = jiseki1.Jyuryo,
                            Hosos3 = jiseki1.Hosos3,
                            Jyury3 = jiseki1.Jyury3,
                            Ufutan = jiseki1.Ufutan,
                            Yusono = jiseki1.Yusono,
                            Delflg = jiseki1.Delflg,
                            Ctlf19 = jiseki1.Ctlf19,
                            Ctlf28 = jiseki1.Ctlf28,
                            Ctlf29 = jiseki1.Ctlf29,
                            Mehkbn = jiseki1.Mehkbn,
                            Jskkbn = jiseki1.Jskkbn,
                            Tocymd = jiseki1.Tocymd,
                            Taksiz = jiseki1.Taksiz,
                            Seikyu = jiseki1.Seikyu,
                            Geppou = jiseki1.Geppou,
                            Pccod = jiseki1.Pccod,
                            Sgenn2 = jiseki1.Sgenn2,
                            Stoucd = jiseki1.Stoucd,
                            Kencod = jiseki1.Kencod,
                            Jiscod = jiseki1.Jiscod,
                            Tensir = jiseki1.Tensir,
                            Hatcod = jiseki1.Hatcod,
                            Sgen35 = jiseki1.Sgen35,
                            Ecoflg = jiseki1.Ecoflg,
                            Syuksu = jiseki1.Syuksu,
                            //Syutuf = jiseki1.Syutuf,
                            Syutuf = "3",
                            Crtcod = jiseki1.Crtcod,
                            Crtymd = jiseki1.Crtymd,
                            Updcod = jiseki1.Updcod,
                            Updymd = jiseki1.Updymd
                        };

                        dbContext.TUnsouShuukaJiseki2.Add(jiseki2);
                        await dbContext.SaveChangesAsyncEx();
                        entry.Insert2++;
                    }

                    // 実績データ削除
                    var jiseki1Del = await dbContext.TUnsouShuukaJiseki1
                                    .Where(x => x.Sykymd <= edDate)
                                    .Where(x => (((x.Seikyu == "" || x.Seikyu == "0") || (x.Geppou == "" || x.Geppou == "0")) && x.Delflg == "1") ||
                                                 ((x.Seikyu != "" && x.Seikyu != "0") && (x.Geppou != "" && x.Geppou != "0")))
                                    .ToListAsync();

                    if (jiseki1Del.Count != 0)
                    {
                        dbContext.TUnsouShuukaJiseki1.RemoveRange(jiseki1Del);
                        entry.Delete1 = jiseki1Del.Count;
                    }

                    // 履歴データ登録
                    string stDate12 = stDate.AddMonths(-12).ToString("yyMM");                    
                    var shuukaJisekiList2 = await dbContext.TUnsouShuukaJiseki2
                        .Where(x => x.Dataym.CompareTo(stDate12) == -1).ToListAsync();

                    var shuukaJisekiListtes = await dbContext.TUnsouShuukaJiseki2
                        .Where(x => x.Dataym == stDate12).ToListAsync();

                    if (shuukaJisekiList2.Count != 0)
                    {
                        // 履歴データ登録
                        foreach (var jiseki2 in shuukaJisekiList2)
                        {
                            TUnsouRirekiShuukaJiseki rireki = new TUnsouRirekiShuukaJiseki
                            {
                                Month = entry.YyyyMm.Replace("/", ""),
                                Syukno = jiseki2.Syukno,
                                Dataym = jiseki2.Dataym,
                                Sykymd = jiseki2.Sykymd,
                                Kisyu = jiseki2.Kisyu,
                                Keifno = jiseki2.Keifno,
                                Fsykno = jiseki2.Fsykno,
                                Tancod = jiseki2.Tancod,
                                Tannam = jiseki2.Tannam,
                                Tdkcod = jiseki2.Tdkcod,
                                Tdkjyu = jiseki2.Tdkjyu,
                                Tdknam = jiseki2.Tdknam,
                                Tdsnam = jiseki2.Tdsnam,
                                Tdbnam = jiseki2.Tdbnam,
                                Tdktan = jiseki2.Tdktan,
                                Tdktel = jiseki2.Tdktel,
                                Tdkyub = jiseki2.Tdkyub,
                                Hincod = jiseki2.Hincod,
                                Hinnam = jiseki2.Hinnam,
                                Unscod = jiseki2.Unscod,
                                Unscrs = jiseki2.Unscrs,
                                Sircod = jiseki2.Sircod,
                                Sgenno = jiseki2.Sgenno,
                                Unskbn = jiseki2.Unskbn,
                                Sybcod = jiseki2.Sybcod,
                                Tokcod = jiseki2.Tokcod,
                                Seicod = jiseki2.Seicod,
                                Denkbn = jiseki2.Denkbn,
                                Denmsu = jiseki2.Denmsu,
                                Tkjiko = jiseki2.Tkjiko,
                                Hososu = jiseki2.Hososu,
                                Nfdate = jiseki2.Nfdate,
                                Daihno = jiseki2.Daihno,
                                Daihnoym = jiseki2.Daihnoym,
                                Jyuryo = jiseki2.Jyuryo,
                                Hosos3 = jiseki2.Hosos3,
                                Jyury3 = jiseki2.Jyury3,
                                Ufutan = jiseki2.Ufutan,
                                Yusono = jiseki2.Yusono,
                                Delflg = jiseki2.Delflg,
                                Ctlf19 = jiseki2.Ctlf19,
                                Ctlf28 = jiseki2.Ctlf28,
                                Ctlf29 = jiseki2.Ctlf29,
                                Mehkbn = jiseki2.Mehkbn,
                                Jskkbn = jiseki2.Jskkbn,
                                Tocymd = jiseki2.Tocymd,
                                Taksiz = jiseki2.Taksiz,
                                Seikyu = jiseki2.Seikyu,
                                Geppou = jiseki2.Geppou,
                                Pccod = jiseki2.Pccod,
                                Sgenn2 = jiseki2.Sgenn2,
                                Stoucd = jiseki2.Stoucd,
                                Kencod = jiseki2.Kencod,
                                Jiscod = jiseki2.Jiscod,
                                Tensir = jiseki2.Tensir,
                                Hatcod = jiseki2.Hatcod,
                                Sgen35 = jiseki2.Sgen35,
                                Ecoflg = jiseki2.Ecoflg,
                                Syuksu = jiseki2.Syuksu,
                                Syutuf = jiseki2.Syutuf,
                                Crtcod = jiseki2.Crtcod,
                                Crtymd = jiseki2.Crtymd,
                                Updcod = jiseki2.Updcod,
                                Updymd = jiseki2.Updymd

                            };
                            dbContext.TUnsouRirekiShuukaJiseki.Add(rireki);
                            await dbContext.SaveChangesAsyncEx();
                            entry.InsertR++;
                        }

                        // 累積データ削除
                        dbContext.TUnsouShuukaJiseki2.RemoveRange(shuukaJisekiList2);
                        entry.Delete2 = shuukaJisekiList2.Count;

                    }

                    // 3月の場合は履歴データ削除処理
                    if (stDate.Month == 3)
                    {
                        string stDate11 = DateTime.Now.AddYears(-11).ToString("yyyy");
                        var rirekiShuukaJiseki = await dbContext.TUnsouRirekiShuukaJiseki
                                    .Where(x => x.Month.StartsWith(stDate11)).ToListAsync();

                        if (rirekiShuukaJiseki.Count() != 0)
                        {
                            dbContext.TUnsouRirekiShuukaJiseki.RemoveRange(rirekiShuukaJiseki);
                            entry.DeleteR = rirekiShuukaJiseki.Count;
                        }
                    }

                    transaction.Commit();

                }
                catch (DbEntityValidationException dbEx)
                {
                    DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                    throw dbEx;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private bool GetStEdData(string value, ref DateTime stDate, ref DateTime edime)
        {

            if (!DateTime.TryParse(value + "/01", out stDate))
            {
                return false;
            }
            edime = stDate.AddMonths(1);
            edime = edime.AddDays(-1);

            return true;

        }
    }
}