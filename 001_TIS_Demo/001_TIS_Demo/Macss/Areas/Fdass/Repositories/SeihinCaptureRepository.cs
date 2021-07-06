using Macss.Areas.Fdass.Common;
using Macss.Areas.Fdass.Models;
using Macss.Areas.Fdass.ViewModels;
using Macss.Models;
using Macss.Repositories;
using Macss.Controllers;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Macss.Areas.Fdass.Repositories
{

    public class SeihinCaptureRepository : ISeihinCaptureRepository
    {

        #region 定数

        public const string MATANKA = "MFB106";         // 松本工場分
        public const string MITANKA = "YCD7104";        // 三重工場分

        public const string DTMOTOMA = "2";             // 2:松本工場
        public const string DTMOTOME = "6";             // 6:三重工場

        #endregion

        private readonly ApplicationDB dbContext;
        private IControlRepository controlRepository;

        public SeihinCaptureRepository(ApplicationDB db)
        {
            this.dbContext = db;
            controlRepository = new ControlRepository(dbContext);

        }

        public async Task<bool> GetSeihin(SeihinCaptureViewModel model, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            // ファイル読み込み
            IList<string> tFList = new List<string>();
            if (!await ImportFileData(model, tFList, loginUser, error))
            {
                return false;
            }

            // ファイル退避
            if (!await BackUpFile(tFList, error))
            {
                return false;
            }

            // 重複チェック
            if (!await CheckDuplication(error))
            {
                return false;
            }

            bool fFlag1 = false;
            bool fFlag2 = false;
            foreach (string fileName in tFList)
            {
                if (fileName.Contains(MATANKA))
                {
                    fFlag1 = true;
                }

                if (fileName.Contains(MITANKA))
                {
                    fFlag2 = true;
                }
            }

            try
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {

                    //dbContext.Configuration.AutoDetectChangesEnabled = false;
                    dbContext.Configuration.ValidateOnSaveEnabled = false;

                    // 製品マスタ更新 松本分
                    if (fFlag1)
                    {
                        await SetSeihinData(model, "2", "1", loginUser, error);
                    }


                    // 製品マスタ更新 三重分
                    if (fFlag2)
                    {
                        await SetSeihinData(model, "6", "2", loginUser, error);
                    }

                    // 単価マスタ更新
                    await SetTankaData(model, loginUser, error);
                    transaction.Commit();

                }
            }
            catch (DbEntityValidationException dbEx)
            {
                DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //dbContext.Configuration.AutoDetectChangesEnabled = true;
                dbContext.Configuration.ValidateOnSaveEnabled = true;

            }

            return true;

        }


        private async Task<bool> ImportFileData(SeihinCaptureViewModel model, IList<string> tFList, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            // ファイル存在チェック
            var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TankaFile)).Where(x => x.Kbn == "1").First();
            string[] files = System.IO.Directory.GetFiles(controls.Value1, "*.csv", System.IO.SearchOption.AllDirectories);
            bool fFlag1 = false;
            bool fFlag2 = false;
            foreach (string file in files)
            {
                if (file.Contains(MATANKA))
                {
                    tFList.Add(file);
                    fFlag1 = true;
                }

                if (file.Contains(MITANKA))
                {
                    tFList.Add(file);
                    fFlag2 = true;
                }

            }
            if (!fFlag1 && !fFlag2)
            {
                error.AddModelError(String.Empty, String.Format(Resources.Message.CE103));
                return false;
            }

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                // ワークデータ削除
                //var wSeihin = dbContext.WHokanSeihin.OrderBy(x => x.Hincod);
                //dbContext.WHokanSeihin.RemoveRange(wSeihin);

                StringBuilder sql = new StringBuilder();
                CreateDeleteSQL(ref sql);
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
                await dbContext.SaveChangesAsync();

                // ファイル読み込み
                List<WHokanSeihin> wSeihinList = new List<WHokanSeihin>();
                foreach (string file in tFList)
                {
                    ReadSeihinFile(file, wSeihinList, loginUser);                    
                }
                model.Moto = wSeihinList.Count();

                // データ登録
                //decimal idx = 1;
                //foreach (WHokanSeihin nData in wSeihinList)
                //{
                //    nData.Id = idx;
                //    dbContext.WHokanSeihin.Add(nData);
                //    idx++;
                //}
                //await dbContext.SaveChangesAsync();


                decimal idx = 1;
                sql = new StringBuilder();
                foreach (WHokanSeihin nData in wSeihinList)
                {
                    nData.Id = idx;
                    CreateInsertSQL(ref sql, nData);
                    idx++;
                }
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
                await dbContext.SaveChangesAsync();

                transaction.Commit();
            }

            return true;

        }

        private List<WHokanSeihin> ReadSeihinFile(string path, List<WHokanSeihin> dataList, string loginUser)
        {
            string moto = DTMOTOMA;
            if (path.Contains(MITANKA))
            {
                moto = DTMOTOME;
            }

            // ファイルを開く
            TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding("Shift_JIS"));
            using (parser)
            {
                // CSV
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");
                decimal idx = dataList.Count + 1;
                while (!parser.EndOfData)
                {
                    WHokanSeihin seihin = new WHokanSeihin();
                    string[] cols = parser.ReadFields();
                    seihin.Hincod = cols[0];
                    seihin.Hinnam = cols[1];
                    seihin.Hinnmk = cols[2];
                    seihin.Kisyua = cols[3];
                    seihin.Kisyub = cols[4];
                    seihin.Sybcod = cols[5];
                    seihin.Fptank = DataUtil.GetDecimal(cols[6]);
                    seihin.Mentbi = DataUtil.GetDate(cols[7]);
                    seihin.Zantei = cols[8];
                    seihin.Zaiksu = DataUtil.GetDecimal(cols[9]);
                    seihin.Hokcod = cols[10];
                    seihin.Frikae = cols[11];
                    seihin.Dtmoto = moto;
                    seihin.Crtcod = loginUser;
                    seihin.Crtymd = DateTime.Now;
                    seihin.Updcod = loginUser;
                    seihin.Updymd = DateTime.Now;
                    dataList.Add(seihin);
                    idx++;
                }

            }
            return dataList;
        }


        private async Task<bool> CheckDuplication(System.Web.Mvc.ModelStateDictionary error)
        {
            
            var checks = await dbContext.WHokanSeihin
                        .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (s, k) => new { s, k })
                        .Where(x => (x.s.Dtmoto == "2" && x.k.Ctlf03 != "1") || (x.s.Dtmoto == "6" && x.k.Ctlf03 != "2"))
                        .GroupBy(x => x.s.Hincod).Where(x => x.Count() > 1).Select(x => x.Key).ToListAsync();

            StringBuilder sb = new StringBuilder();
            bool fFlag = true;
            foreach (var check in checks)
            {
                if (fFlag)
                {
                    sb.Append(check);
                    fFlag = false;
                } else
                {
                    sb.Append("," + check);
                }
            }

            if (!fFlag)
            {
                error.AddModelError(string.Empty, String.Format(Resources.Message.CE105, sb.ToString()));
                return false;
            }

            return true;

        }

        private async Task<bool> SetSeihinData(SeihinCaptureViewModel model, string dtmoto, string ctlf3, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            //var seihin = await dbContext.MHokanSeihin
            //        .Where(x => (x.Dtmoto == dtmoto))
            //        .ToListAsync();

            //if (seihin.Count() != 0)
            //{
            //    dbContext.MHokanSeihin.RemoveRange(seihin);
            //}

            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM m_hokan_seihin WHERE DTMOTO = N'" + dtmoto + "'");
            dbContext.Database.ExecuteSqlCommand(sql.ToString());

            // 製品データ取り込み(対象データ)
            var wseihin = await dbContext.WHokanSeihin
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => (x.n.Dtmoto == dtmoto && x.k.Ctlf03 != ctlf3))
                                    .Select(x => new 
                                    {
                                        Hincod = x.n.Hincod,
                                        Hinnam = x.n.Hinnam,
                                        Hinnmk = x.n.Hinnmk,
                                        Kisyua = x.n.Kisyua,
                                        Kisyub = x.n.Kisyub,
                                        Sybcod = x.n.Sybcod,
                                        Fptank = x.n.Fptank,
                                        Mentbi = x.n.Mentbi,
                                        Zantei = x.n.Zantei,
                                        Zaiksu = x.n.Zaiksu,
                                        Hokcod = x.n.Hokcod,
                                        Frikae = x.n.Frikae,
                                        Dtmoto = x.n.Dtmoto,
                                        Crtcod = x.n.Crtcod,
                                        Crtymd = x.n.Crtymd,
                                        Updcod = x.n.Updcod,
                                        Updymd = x.n.Updymd
                                    })
                                    .ToListAsync();

            // データ登録
            foreach (var wData in wseihin)
            {

                //var seihin = await dbContext.MHokanSeihin
                //        .Where(x => (x.Hincod == wData.Hincod))
                //        .ToListAsync();

                //if (seihin.Count() != 0)
                //{
                //    dbContext.MHokanSeihin.RemoveRange(seihin);
                //}

                MHokanSeihin mHokanSeihin = new MHokanSeihin()
                {
                    Hincod = wData.Hincod,
                    Hinnam = wData.Hinnam,
                    Hinnmk = wData.Hinnmk,
                    Kisyua = wData.Kisyua,
                    Kisyub = wData.Kisyub,
                    Sybcod = wData.Sybcod,
                    Fptank = wData.Fptank,
                    Mentbi = wData.Mentbi,
                    Zantei = wData.Zantei,
                    Zaiksu = wData.Zaiksu,
                    Hokcod = wData.Hokcod,
                    Frikae = wData.Frikae == "0" ? string.Empty : wData.Frikae,
                    Dtmoto = wData.Dtmoto,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now
                };

                dbContext.MHokanSeihin.Add(mHokanSeihin);
            }
            await dbContext.SaveChangesAsync();

            return true;

        }


        private async Task<bool> SetTankaData(SeihinCaptureViewModel model, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            DateTime oldDate = new DateTime(1900, 1, 1);
            // 製品データ取り込み(対象データ)
            var seihin = await dbContext.MHokanSeihin
                                    .Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (s, k) => new { s, k })
                                    .Where(x => (x.k.Seitai == "Y" && x.k.Hokflg2 == "C"))
                                    .Select(x => new
                                    {
                                        Hincod = x.s.Hincod,
                                        Hinnam = x.s.Hinnam,
                                        Hinnmk = x.s.Hinnmk,
                                        Kisyua = x.s.Kisyua,
                                        Kisyub = x.s.Kisyub,
                                        Sybcod = x.s.Sybcod,
                                        Fptank = x.s.Fptank,
                                        Mentbi = x.s.Mentbi,
                                        Zantei = x.s.Zantei,
                                        Zaiksu = x.s.Zaiksu,
                                        Hokcod = x.s.Hokcod,
                                        Frikae = x.s.Frikae,
                                        Dtmoto = x.s.Dtmoto,
                                        Crtcod = x.s.Crtcod,
                                        Crtymd = x.s.Crtymd,
                                        Updcod = x.s.Updcod,
                                        Updymd = x.s.Updymd,
                                        Ojyukr = x.k.Ojyukr,
                                        Osyjyr = x.k.Osyjyr,
                                        Hjyukr = x.k.Hjyukr,
                                        Hsyjyr = x.k.Hsyjyr
                                    })
                                    .ToListAsync();

            // データ登録
            foreach (var sData in seihin)
            {

                // 請求対象外の機種コードB
                string kisyuB = DataUtil.SubstringEx(sData.Kisyub, 0, 2);
                var joukenB = await dbContext.MHokanJouken
                        .Where(x => (x.Sikcod == "HOKAN" && x.Jyoken == kisyuB)).ToListAsync();

                if (joukenB.Count() != 0)
                {
                    continue;
                }

                // 雑コードの製品
                string work = string.Empty;
                int len = sData.Hincod.Length;
                if (len >= 4)
                {
                    work = DataUtil.SubstringEx(sData.Hincod, len - 4, 4);
                }

                if (work == "9999" || DataUtil.SubstringEx(sData.Hinnmk, 0, 3) == "Z41")
                {
                    continue;
                }

                if (sData.Hinnam.IndexOf("ザツ　") != -1)
                {
                    continue;
                }

                // 単価マスタ検索
                string Status = string.Empty;
                List<MHokanTanka> tankas = await dbContext.MHokanTanka
                                        .Where(x => x.Hincod == sData.Hincod)
                                        .ToListAsync();

                decimal Fptank = 0;
                MHokanTanka tanka = new MHokanTanka();
                if (tankas.Count() == 0)
                {
                    // 新規
                    Status = "N";
                    Fptank = (decimal)sData.Fptank;
                }
                else
                {
                    tanka = tankas.First();

                    string Hinnmk = sData.Hinnmk;
                    if (Hinnmk.Length > 20)
                    {
                        Hinnmk = DataUtil.SubstringEx(Hinnmk, 0, 20);
                    }
                    // 品名型式が異なる場合 若しくは、FP単価が０の場合
                    if (tanka.Hinnmk != Hinnmk || (tanka.Fptank == 0 && sData.Fptank != 0))
                    {
                        // 修正
                        Status = "U";
                        Fptank = (decimal)sData.Fptank;


                    }
                    else if (tanka.Sybcod != sData.Hokcod || tanka.Frikae.Trim() != sData.Frikae.Trim())
                    {
                        // 保管場所、振替区分が変わった製品
                        // 修正
                        Status = "U";
                        Fptank = (decimal)tanka.Fptank;
                    }
                    else
                    {
                        // 保管場所。振替区分が変わらない製品
                        if (tanka.Kisyua != sData.Kisyua || tanka.Kisyub.Trim() != sData.Kisyub.Trim())
                        {
                            // 機種更新のみ
                            Status = "R";
                        }
                    }
                }

                // 保管単価の設定
                decimal? hTanka = tanka.Hokant == null ? 0 : tanka.Hokant;
                decimal? jyukar = tanka.Jyukar == null ? 0 : tanka.Jyukar;
                decimal? syujyr = tanka.Syujyr == null ? 0 : tanka.Syujyr;

                if (Status == "N" || Status == "U")
                {
                    // 「新規」または「修正」の場合に設定
                    string hok = DataUtil.SubstringEx(sData.Hokcod, 0, 1);
                    var jouken1 = await dbContext.MHokanJouken
                            .Where(x => (x.Sikcod == "HOKCOD" && x.Jyoken == sData.Hokcod)).ToListAsync();

                    var jouken2 = await dbContext.MHokanJouken
                            .Where(x => (x.Sikcod == "HOKCOD2" && x.Jyoken == hok)).ToListAsync();

                    var jouken3 = await dbContext.MHokanJouken
                            .Where(x => (x.Sikcod == "OTU" && x.Jyoken == sData.Hokcod)).ToListAsync();

                    if ((sData.Frikae != null && sData.Frikae.Trim() != string.Empty) || jouken1.Count() != 0 || jouken2.Count() != 0)
                    {
                        hTanka = 0;
                    }
                    else
                    {

                        if (jouken3.Count() != 0)
                        {
                            // 乙
                            jyukar = (decimal)sData.Ojyukr;
                            syujyr = (decimal)sData.Osyjyr;
                        }
                        else
                        {
                            // 丙
                            jyukar = (decimal)sData.Hjyukr;
                            syujyr = (decimal)sData.Hsyjyr;
                        }

                        hTanka = Fptank * jyukar * syujyr / (decimal)1000 + (decimal)0.000005;
                    }

                }

                string hinnmk = sData.Hinnmk;
                if (sData.Hinnmk.Length > 20) {
                    hinnmk = DataUtil.SubstringEx(sData.Hinnmk, 0, 20);
                }
                if (Status == "N")
                {
                    // 新規
                    MHokanTanka mHokanTanka = new MHokanTanka()
                    {
                        Hincod = sData.Hincod,
                        Sybcod = sData.Hokcod,
                        Hinnmk = hinnmk,
                        Kisyua = sData.Kisyua,
                        Kisyub = sData.Kisyub,
                        Frikae = sData.Frikae,
                        Jyukar = jyukar,
                        Syujyr = syujyr,
                        Fptank = sData.Fptank,
                        Hokant = hTanka,
                        Niekit = 0,
                        Nieymd = oldDate,
                        Ofptnk = 0,
                        Ohokat = 0,
                        Oniekt = 0,
                        Hokymd = DateTime.Now,
                        Hoktan = loginUser,
                        Crtcod = loginUser,
                        Crtymd = DateTime.Now,
                        Updcod = loginUser,
                        Updymd = DateTime.Now
                    };
                    dbContext.MHokanTanka.Add(mHokanTanka);
                    await dbContext.SaveChangesAsync();
                    model.Torikomi += 1;
                    model.Insert += 1;

                }
                else if (Status == "U")
                {
                    DateTime wDate = (DateTime)tanka.Hokymd;
                    if (tanka.Hokant != hTanka)
                    {
                        wDate = DateTime.Now;
                    }

                    string OSybcod = tanka.Sybcod;
                    string OHinnmk = tanka.Hinnmk;
                    string OKisyua = tanka.Kisyua;
                    string OKisyub = tanka.Kisyub;
                    string OFrikae = tanka.Frikae;

                    decimal? OFptank = tanka.Fptank;
                    decimal? OHokant = tanka.Hokant;

                    // 修正
                    tanka.Sybcod = sData.Hokcod;
                    tanka.Hinnmk = hinnmk;
                    tanka.Kisyua = sData.Kisyua;
                    tanka.Kisyub = sData.Kisyub;
                    tanka.Frikae = sData.Frikae;
                    tanka.Jyukar = jyukar;
                    tanka.Syujyr = syujyr;
                    tanka.Fptank = sData.Fptank;
                    tanka.Hokant = hTanka;
                    tanka.Hokymd = wDate;
                    tanka.Osybcod = OSybcod;
                    tanka.Ohinnmk = OHinnmk;
                    tanka.Okisyua = OKisyua;
                    tanka.Okisyub = OKisyub;
                    tanka.Ofrikae = OFrikae;
                    tanka.Ofptnk = OFptank;
                    tanka.Ohokat = OHokant;
                    tanka.Updcod = loginUser;
                    tanka.Updymd = DateTime.Now;
                    dbContext.ChangeTracker.DetectChanges();
                    await dbContext.SaveChangesAsync();
                    model.Torikomi += 1;
                    model.Update += 1;
                }
                else if (Status == "R")
                {

                    string OSybcod = tanka.Sybcod;
                    string OHinnmk = tanka.Hinnmk;
                    string OKisyua = tanka.Kisyua;
                    string OKisyub = tanka.Kisyub;
                    string OFrikae = tanka.Frikae;
                    decimal? OFptank = tanka.Fptank;
                    decimal? OHokant = tanka.Hokant;

                    // 機種変更
                    tanka.Kisyua = sData.Kisyua;
                    tanka.Kisyub = sData.Kisyub;
                    tanka.Osybcod = OSybcod;
                    tanka.Ohinnmk = OHinnmk;
                    tanka.Okisyua = OKisyua;
                    tanka.Okisyub = OKisyub;
                    tanka.Ofrikae = OFrikae;
                    tanka.Ofptnk = OFptank;
                    tanka.Ohokat = OHokant;
                    tanka.Updcod = loginUser;
                    tanka.Updymd = DateTime.Now;
                    dbContext.ChangeTracker.DetectChanges();
                    await dbContext.SaveChangesAsync();
                    model.Torikomi += 1;
                    model.Update += 1;
                }

            }
            return true;

        }

        private async Task<bool> BackUpFile(IList<string> tFList, System.Web.Mvc.ModelStateDictionary error)
        {

            // ファイル退避
            try
            {
                var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.TankaFile)).Where(x => x.Kbn == "1").First();
                foreach (string file in tFList)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    System.IO.File.Move(@controls.Value1 + "/" + fileName, @controls.Value2 + "/" + fileName);
                }

            }
            catch
            {
                error.AddModelError(String.Empty, String.Format(Resources.Message.CE104));
                return false;
            }

            return true;
        }

        private static void CreateDeleteSQL(ref StringBuilder sql)
        {
            sql.Append(" DELETE FROM w_hokan_seihin; ");
        }

        private static void CreateInsertSQL(ref StringBuilder sql, WHokanSeihin seihin)
        {

            sql.Append(" INSERT INTO w_hokan_seihin (");
            sql.Append(" ID,");
            sql.Append(" HINCOD,");
            sql.Append(" HINNAM,");
            sql.Append(" HINNMK,");
            sql.Append(" KISYUA,");
            sql.Append(" KISYUB,");
            sql.Append(" SYBCOD,");
            sql.Append(" FPTANK,");
            sql.Append(" MENTBI,");
            sql.Append(" ZANTEI,");
            sql.Append(" ZAIKSU,");
            sql.Append(" HOKCOD,");
            sql.Append(" FRIKAE,");
            sql.Append(" DTMOTO,");
            sql.Append(" CRTCOD,");
            sql.Append(" CRTYMD,");
            sql.Append(" UPDCOD,");
            sql.Append(" UPDYMD");
            sql.Append(") VALUES (");

            sql.Append("N'" + seihin.Id + "',");
            sql.Append("N'" + seihin.Hincod + "',");
            sql.Append("N'" + seihin.Hinnam + "',");
            sql.Append("N'" + seihin.Hinnmk + "',");
            sql.Append("N'" + seihin.Kisyua + "',");
            sql.Append("N'" + seihin.Kisyub + "',");
            sql.Append("N'" + seihin.Sybcod + "',");
            sql.Append("N'" + seihin.Fptank + "',");
            sql.Append("N'" + seihin.Mentbi + "',");
            sql.Append("N'" + seihin.Zantei + "',");
            sql.Append("N'" + seihin.Zaiksu + "',");
            sql.Append("N'" + seihin.Hokcod + "',");
            sql.Append("N'" + seihin.Frikae + "',");
            sql.Append("N'" + seihin.Dtmoto + "',");
            sql.Append("N'" + seihin.Crtcod + "',");
            sql.Append("N'" + seihin.Crtymd + "',");
            sql.Append("N'" + seihin.Updcod + "',");
            sql.Append("N'" + seihin.Updymd + "'");
            sql.AppendLine(");");
        }

    }
}