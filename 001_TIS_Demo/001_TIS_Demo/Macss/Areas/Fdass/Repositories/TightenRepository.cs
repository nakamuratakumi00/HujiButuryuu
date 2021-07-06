using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Macss.Models;
using Macss.Repositories;
using Macss.Areas.Fdass.ViewModels;
using Macss.Areas.Fdass.Models;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Data.Entity.Validation;
using Macss.Areas.Fdass.Common;

namespace Macss.Areas.Fdass.Repositories
{
    public class TightenRepository : ITightenRepository
    {
        #region 定数
        public const string StRun = "1";            // 1:実行中
        public const string StEnd = "2";            // 2:完了
        public const string StBak = "3";            // 3:退避完了
        public const string StErr = "9";            // 9:異常終了

        public const string MANS = "MFB107";        // FITC松本入出庫データ
        public const string MINS = "YCD7105";       // FITC三重入出庫データ
        public const string MADP = "MFB108";        // FITC松本伝票件数データ
        public const string MIDP = "YCD7106";       // FITC三重伝票件数データ

        public const string DTMOTOMA = "2";         // 2:松本工場
        public const string DTMOTOME = "6";         // 6:三重工場

        public const string NOT_KISYUB = "XXX";     // 空白Key項目 機種B
        public const string NOT_HOKCOD = "XX";      // 空白Key項目 保管場所

        #endregion

        // デバッグ用
        bool csvFlag = true;

        IList<string> error = new List<string>();
        private readonly ApplicationDB dbContext;
        private IControlRepository controlRepository;
        private IPrintRepository printRepository;
        private IMatujimeKanriRepository matujimeKanriRepository;
        private IMaintenanceRepository maintenanceRepository;
        private LogService logService;
        private ILogRepository logRepository;

        public TightenRepository(ApplicationDB db)
        {
            this.dbContext = db;
            controlRepository = new ControlRepository(dbContext);
            printRepository = new PrintRepository(dbContext);
            matujimeKanriRepository = new MatujimeKanriRepository(dbContext);
            maintenanceRepository = new MaintenanceRepository(dbContext);
            logRepository = new LogRepository(dbContext);
            logService = new LogService();

        }

        public async Task<TightenViewModel> GetDispData(string yyyyMm)
        {
            MatujimeKanriViewModel prntData = await matujimeKanriRepository.GetDispData(yyyyMm);
            TightenViewModel dispData = new TightenViewModel();
            dispData.MatujimeKanri = prntData;
            return dispData;

        }

        // ステータスチェック
        public async Task<bool> IsMatujimeKanriStatus(string month, System.Web.Mvc.ModelStateDictionary error)
        {

            // 指定月の前月ステータスチェック
            string workMonth = month + "/01";
            DateTime lastMonth = DateTime.Parse(workMonth);
            lastMonth = lastMonth.AddMonths(-1);
            string wMonth = lastMonth.ToString("yyyyMM");
            var matujime = (await matujimeKanriRepository.FindByMonthAsync(wMonth)).FirstOrDefault();
            if (matujime == null || (matujime != null && !matujime.Status.Equals(StBak)))
            {
                error.AddModelError(String.Empty, String.Format(Resources.Message.CE100));
                return false;
            }

            // 指定月ステータスチェック
            bool errorFlag = false;
            wMonth = month.Replace("/", "");
            matujime = (await matujimeKanriRepository.FindByMonthAsync(wMonth)).FirstOrDefault();
            if (matujime != null)
            {
                if (matujime.Status.Equals(StRun))
                {
                    error.AddModelError(String.Empty, String.Format(Resources.Message.CE101));
                    errorFlag = true;
                }
                else if (matujime.Status.Equals(StBak))
                {
                    error.AddModelError(String.Empty, String.Format(Resources.Message.CE102));
                    errorFlag = true;
                }

                if (errorFlag)
                {
                    return false;
                }
            }

            return true;

        }

        // 受信ファイル読み込み
        public async Task<bool> ReadFile(string month, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            IList<string> nFList = new List<string>();
            IList<string> dFList = new List<string>();

            // 末締処理管理テーブル　ステータス変更
            string wMonth = month.Replace("/", "");
            await SetStatus(wMonth, StRun, loginUser, error);

            using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
            {

                // ファイル読み込み
                if (!await ImportFileData(loginUser, nFList, dFList, error))
                {
                    transaction.Rollback();
                    return false;
                }
                transaction.Commit();

            }

            // ファイル退避
            try
            {
                var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.MatujimeFile)).Where(x => x.Kbn == "1").First();
                foreach (string file in nFList)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    System.IO.File.Move(@controls.Value1 + "/" + fileName, @controls.Value2 + "/" + fileName);
                }

                foreach (string file in dFList)
                {
                    string fileName = System.IO.Path.GetFileName(file);
                    System.IO.File.Move(@controls.Value1 + "/" + fileName, @controls.Value2 + "/" + fileName);
                }

            }
            catch (Exception)
            {
                error.AddModelError(String.Empty, String.Format(Resources.Message.CE104));
                return false;
                //throw ex;
            }

            return true;

        }

        // 末締処理管理テーブル　ステータス変更
        public async Task<bool> SetStatus(string month, string status, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            // 末締処理管理テーブル　ステータス変更
            var matujimes = await matujimeKanriRepository.FindByMonthAsync(month);
            if (matujimes.Count() == 0)
            {
                THokanMatujimeKanri mokanMatujimeKanri = new THokanMatujimeKanri
                {
                    Month = month,
                    Status = status,
                    Startt = DateTime.Now,
                    Endt = DateTime.Now,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now
                };
                if (status == StRun)
                {
                    mokanMatujimeKanri.Startt = DateTime.Now;
                }
                else
                {
                    mokanMatujimeKanri.Endt = DateTime.Now;
                }
                dbContext.THokanMatujimeKanri.Add(mokanMatujimeKanri);
                await dbContext.SaveChangesAsync();

            }
            else {
                THokanMatujimeKanri matujime = matujimes.First();
                if (status == StRun)
                {
                    matujime.Startt = DateTime.Now;
                } else
                {
                    matujime.Endt = DateTime.Now;
                }
                matujime.Status = status;
                matujime.Updcod = loginUser;
                matujime.Updymd = DateTime.Now;
                await dbContext.SaveChangesAsync();
            }
            return true;

        }

        // 請求データ作成
        public async Task<bool> SetSeikyuData(string month, string prossesingId, string menuName, string loginUser, System.Web.Mvc.ModelStateDictionary modelError)
        {

            try { 
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    dbContext.Configuration.ValidateOnSaveEnabled = false;

                    // 入出庫データ・伝票件数データ作成
                    await SetSeikyuMotoData(month, loginUser, modelError);

                    // 請求データ作成 保管料
                    await SetSeikyuHokanData(loginUser, modelError);

                    // 倉庫使用料
                    await SetSeikyuSoukoData(loginUser, modelError);

                    // 請求データ作成 荷役料
                    await SetSeikyuNiekiData(loginUser, modelError);

                    // 拠点別データ作成
                    await SetKyotenData(month, loginUser, modelError);

                    // 末締処理管理テーブル　ステータス変更
                    string wMonth = month.Replace("/", "");
                    await SetStatus(wMonth, StEnd, loginUser, modelError);

                    UpdateSQL();

                    transaction.Commit();

                }
            } finally
            {
                dbContext.Configuration.ValidateOnSaveEnabled = true;
            }

            if (error.Count() != 0)
            {
                bool fFlag = true;
                StringBuilder errorMmsg= new StringBuilder();
                foreach (string buf in error)
                {
                    if (fFlag)
                    errorMmsg.Append(buf);
                }

                string errorMsg = string.Empty;
                if (errorMmsg.Length > 500)
                {
                    errorMsg = errorMmsg.ToString().Substring(0, 500);
                }
                else
                {
                    errorMsg = errorMmsg.ToString();
                }

                logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.E1, menuName, errorMsg, logRepository);
            }

            StringBuilder count = new StringBuilder();
            var seikyu = dbContext.THokanSeikyu.OrderBy(x => x.Hincod);
            var seikyuB = dbContext.THokanSeikyuB.OrderBy(x => x.Hincod);
            var kyoten = dbContext.THokanSeikyuKyoten.OrderBy(x => x.Kisyua);
            count.Append("Fe保管請求請求データ " + seikyu.Count() + "件作成\n");
            count.Append("Fe保管請求請求データB " + seikyuB.Count() + "件作成\n");
            count.Append("Fe保管請求拠点別データ " + kyoten.Count() + "件作成\n");
            logService.CreateLogHistory(loginUser, prossesingId, ControlRepository.MControlFunctionKbn.I1, menuName, count.ToString(), logRepository);

            return true;

        }

        // 入出庫データ・伝票件数データ作成
        private async Task<bool> SetSeikyuMotoData(string month, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            try
            {
                // 入出庫データ・伝票件数データ削除
                var tNyuushuuko = dbContext.THokanNyuushuuko.OrderBy(x => x.Hincod);
                dbContext.THokanNyuushuuko.RemoveRange(tNyuushuuko);
                var tDenpyokensu = dbContext.THokanDenpyokensu.OrderBy(x => x.Hincod);
                dbContext.THokanDenpyokensu.RemoveRange(tDenpyokensu);

                // 繰越データは前月分以外を削除
                //DateTime dt = DateTime.Parse(month + "/01");
                //dt = dt.AddMonths(-1);
                //string yymmdd = dt.ToString("yyyyMMdd");

                string yymmdd = month + "/01";
                DateTime theDay = DateTime.Parse(yymmdd);
                //theDay = theDay.AddMonths(1);
                theDay = theDay.AddDays(-1);
                string yymmddlast = theDay.ToString("yyyyMMdd");

                var NyuushuukoKurikosi = dbContext.THokanNyuushuukoKurikosi.Where(x => x.Kurymd != yymmddlast);
                dbContext.THokanNyuushuukoKurikosi.RemoveRange(NyuushuukoKurikosi);
                var DenpyokensuKurikosi = dbContext.THokanDenpyokensuKurikosi.Where(x => x.Kurymd != yymmddlast);
                dbContext.THokanDenpyokensuKurikosi.RemoveRange(DenpyokensuKurikosi);


                // 入出庫データデータ取り込み
                await ReadNyuushuukoData(loginUser);

                // 伝票件数データ取り込み
                await ReadDenpyokensu(loginUser);

                // 入出庫繰越データ取り込み
                await ReadNyuushuukoKurikosiData(loginUser);

                // 伝票件数繰越データ取り込み
                await ReadDenpyokensuKurikosiData(loginUser);

                // 入出庫繰越データ・伝票件数繰越データ削除
                IOrderedQueryable<THokanNyuushuukoKurikosi> tNyuushuukoKurikosi = dbContext.THokanNyuushuukoKurikosi.OrderBy(x => x.Hincod);
                dbContext.THokanNyuushuukoKurikosi.RemoveRange(tNyuushuukoKurikosi);
                IOrderedQueryable<THokanDenpyokensuKurikosi> tDenpyokensuKurikosi = dbContext.THokanDenpyokensuKurikosi.OrderBy(x => x.Hincod);
                dbContext.THokanDenpyokensuKurikosi.RemoveRange(tDenpyokensuKurikosi);

                // 入出庫繰越データ・伝票件数繰越データ作成
                await SetKurikosiData(month, loginUser);


                // 前回データ削除
                StringBuilder sql = new StringBuilder();
                CreateDeleteSeikyu(ref sql);
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
                await dbContext.SaveChangesAsync();

                //IOrderedQueryable<THokanSeikyu> tSeikyu = dbContext.THokanSeikyu.OrderBy(x => x.Hincod);
                //await dbContext.THokanSeikyu.RemoveRange(tSeikyu);
                //IOrderedQueryable<THokanSeikyuB> tSeikyuB = dbContext.THokanSeikyuB.OrderBy(x => x.Hincod);
                //await dbContext.THokanSeikyuB.RemoveRange(tSeikyuB);
                //IOrderedQueryable<THokanSeikyuKyoten> tSeikyuKyoten = dbContext.THokanSeikyuKyoten.OrderBy(x => x.Kisyua);
                //await dbContext.THokanSeikyuKyoten.RemoveRange(tSeikyuKyoten);

            }
            catch (DbEntityValidationException dbEx)
            {
                DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                throw dbEx;
            } catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }


        // 請求データ作成 保管料
        private async Task<bool> SetSeikyuHokanData(string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            try
            {
                // 請求データB　品番単位
                await SetSeikyuBHinban(loginUser);

                // 請求データ　品番単位
                await SetSeikyuHinban(loginUser);

                // 請求データ 機種A、物流請求№、
                //await SetSeikyuKisyuA(loginUser);

            }
            catch (DbEntityValidationException dbEx)
            {
                DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                throw dbEx;
            }
            catch (Exception ex) {
                throw ex;
            }

            return true;

        }

        // 請求データ作成 倉庫使用料
        private async Task<bool> SetSeikyuSoukoData(string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            try
            {
                // 倉庫使用料
                await SetSeikyuSouko(loginUser);

            }
            catch (DbEntityValidationException ex)
            {
                DataUtil.PrintEntityValidationErrors(ex.EntityValidationErrors);
                throw ex;
            }

            return true;

        }


        // 請求データ作成 荷役料
        private async Task<bool> SetSeikyuNiekiData(string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            try
            {
                // 請求データ作成 入出庫件数荷役料　品番コード単位
                await SetSeikyuNiekiHinbanN(loginUser);

                // 請求データ作成 入出庫件数荷役料　PCコード単位
                await SetSeikyuNiekiPcN(loginUser);

                // 請求データ作成 入出庫件数荷役料
                await SetSeikyuNieki(loginUser);

                // 請求データ作成 入出庫件数荷役料　PCコード単位
                //await SetSeikyuNiekiPc(loginUser);

            }
            catch (DbEntityValidationException vEx)
            {
                DataUtil.PrintEntityValidationErrors(vEx.EntityValidationErrors);
                throw vEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }


        private async Task<bool> ImportFileData(string loginUser, IList<string> nFList, IList<string> dFList, System.Web.Mvc.ModelStateDictionary error)
        {

            // ファイル存在チェック
            var controls = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.MatujimeFile)).Where(x => x.Kbn == "1").First();

            string[] files = files = System.IO.Directory.GetFiles(controls.Value1, "*.RCV", System.IO.SearchOption.AllDirectories);
            if (csvFlag)
            {
                files = System.IO.Directory.GetFiles(controls.Value1, "*.csv", System.IO.SearchOption.AllDirectories);
            }
            bool fFlag1 = false;
            bool fFlag2 = false;
            bool fFlag3 = false;
            bool fFlag4 = false;
            foreach (string file in files)
            {
                if (file.Contains(MANS))
                {
                    nFList.Add(file);
                    fFlag1 = true;
                }

                if (file.Contains(MINS))
                {
                    nFList.Add(file);
                    fFlag2 = true;
                }

                if (file.Contains(MADP))
                {
                    dFList.Add(file);
                    fFlag3 = true;
                }

                if (file.Contains(MIDP))
                {
                    dFList.Add(file);
                    fFlag4 = true;
                }
            }
            if (!fFlag1 || !fFlag2 || !fFlag3 || !fFlag4)
            {
                error.AddModelError(string.Empty, String.Format(Resources.Message.CE103));
                return false;
            }

            try
            {
                // ワークデータ削除
                //var nyuushuuko = dbContext.WHokanNyuushuuko.OrderBy(x => x.Id);
                //dbContext.WHokanNyuushuuko.RemoveRange(nyuushuuko);
                //var denpyokensu = dbContext.WHokanDenpyokensu.OrderBy(x => x.Id);
                //dbContext.WHokanDenpyokensu.RemoveRange(denpyokensu);

                StringBuilder sql = new StringBuilder();
                CreateDeleteSQLN(ref sql);
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
                dbContext.SaveChanges();

                sql = new StringBuilder();
                CreateDeleteSQLD(ref sql);
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
                dbContext.SaveChanges();

                // ファイル読み込み
                List<WHokanNyuushuuko> nyuushuukoList = new List<WHokanNyuushuuko>();
                foreach (string file in nFList)
                {
                    ReadNyuushuukoFile(file, nyuushuukoList, loginUser);
                }

                List<WHokanDenpyokensu> denpyokensuList = new List<WHokanDenpyokensu>();
                foreach (string file in dFList)
                {
                    ReadDenpyokensuFile(file, denpyokensuList, loginUser);
                }

                // データ登録
                sql = new StringBuilder();
                foreach (WHokanNyuushuuko nData in nyuushuukoList) {

                    CreateInsertSQLN(ref sql, nData);
                    //dbContext.WHokanNyuushuuko.Add(nData);
                }
                //await dbContext.SaveChangesAsync();
                if (sql.Length != 0) { 
                    dbContext.Database.ExecuteSqlCommand(sql.ToString());
                    dbContext.SaveChanges();
                }
                sql = new StringBuilder();
                foreach (WHokanDenpyokensu dData in denpyokensuList)
                {
                    CreateInsertSQLD(ref sql, dData);
                    //dbContext.WHokanDenpyokensu.Add(dData);
                }
                if (sql.Length != 0)
                {
                    //await dbContext.SaveChangesAsync();
                    dbContext.Database.ExecuteSqlCommand(sql.ToString());
                    dbContext.SaveChanges();
                }

            }
            catch (DbEntityValidationException vex)
            {
                DataUtil.PrintEntityValidationErrors(vex.EntityValidationErrors);
                throw vex;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return true;

        }


        private List<WHokanNyuushuuko> ReadNyuushuukoFile(string path, List<WHokanNyuushuuko> dataList, string loginUser)
        {
            string moto = DTMOTOMA;
            if (path.Contains(MINS))
            {
                moto = DTMOTOME;
            }

            // ファイルを開く
            TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding("Shift_JIS"));
            using (parser)
            {
                // CSV
                if (csvFlag)
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                }
                else
                {
                    // 固定長
                    parser.TextFieldType = FieldType.FixedWidth;
                    parser.TrimWhiteSpace = false;
                    parser.FieldWidths = new int[] { 2, 3, 2, 8, 10, 12, 10, 12, 10, 12, 10, 12, 1, 7, 1, 10 };
                }
                decimal idx = dataList.Count + 1;
                while (!parser.EndOfData)
                {
                    WHokanNyuushuuko nyuushuuko = new WHokanNyuushuuko();
                    string[] cols = parser.ReadFields();
                    nyuushuuko.Id = idx;
                    nyuushuuko.Kisyua = cols[0];
                    nyuushuuko.Kisyub = cols[1];
                    nyuushuuko.Hokcod = cols[2];
                    nyuushuuko.Hincod = cols[3];
                    nyuushuuko.Zansuu = DataUtil.GetDecimal(cols[4]);
                    nyuushuuko.Zankin = DataUtil.GetDecimal(cols[5]);
                    nyuushuuko.Siksuu = DataUtil.GetDecimal(cols[6]);
                    nyuushuuko.Sikkin = DataUtil.GetDecimal(cols[7]);
                    nyuushuuko.Kaisuu = DataUtil.GetDecimal(cols[8]);
                    nyuushuuko.Kaikin = DataUtil.GetDecimal(cols[9]);
                    nyuushuuko.Syksuu = DataUtil.GetDecimal(cols[10]);
                    nyuushuuko.Sykkin = DataUtil.GetDecimal(cols[11]);
                    nyuushuuko.Dkbn = cols[12];
                    nyuushuuko.Seikyu = cols[13];
                    nyuushuuko.Baitai = cols[14];
                    nyuushuuko.Touzan = DataUtil.GetDecimal(cols[15]);
                    nyuushuuko.Dtmoto = moto;
                    nyuushuuko.Crtcod = loginUser;
                    nyuushuuko.Crtymd = DateTime.Now;
                    nyuushuuko.Updcod = loginUser;
                    nyuushuuko.Updymd = DateTime.Now;
                    dataList.Add(nyuushuuko);
                    idx++;
                }

            }
            return dataList;
        }

        private List<WHokanDenpyokensu> ReadDenpyokensuFile(string path, List<WHokanDenpyokensu> dataList, string loginUser)
        {
            string moto = DTMOTOMA;
            if (path.Contains(MIDP))
            {
                moto = DTMOTOME;
            }

            // ファイルを開く
            TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding("Shift_JIS"));
            using (parser)
            {
                // CSV
                if (csvFlag)
                {
                    parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    parser.SetDelimiters(",");
                } else
                {
                    // 固定長
                    parser.TextFieldType = FieldType.FixedWidth;
                    parser.TrimWhiteSpace = false;
                    parser.FieldWidths = new int[] { 2, 3, 2, 2, 6, 7, 8, 8, 8, 10, 2 };
                }
                int idx = dataList.Count + 1;
                while (!parser.EndOfData)
                {
                    WHokanDenpyokensu denpyokensu = new WHokanDenpyokensu();
                    string[] cols = parser.ReadFields();
                    denpyokensu.Id = idx;
                    denpyokensu.Kisyua = cols[0];
                    denpyokensu.Kisyub = cols[1];
                    denpyokensu.Basyo = cols[2];
                    denpyokensu.Denkub = cols[3];
                    denpyokensu.Densuu = DataUtil.GetDecimal(cols[4]);
                    denpyokensu.Seikyu = cols[5];
                    denpyokensu.Inpymd = DataUtil.GetDate(cols[6]);
                    denpyokensu.Keiymd = DataUtil.GetDate(cols[7]);
                    denpyokensu.Hincod = cols[8];
                    denpyokensu.Nskosu = DataUtil.GetDecimal(cols[9]);
                    denpyokensu.Eigsok = cols[10];
                    denpyokensu.Dtmoto = moto;
                    denpyokensu.Crtcod = loginUser;
                    denpyokensu.Crtymd = DateTime.Now;
                    denpyokensu.Updcod = loginUser;
                    denpyokensu.Updymd = DateTime.Now;
                    dataList.Add(denpyokensu);
                    idx++;
                }

            }
            return dataList;
        }

        // ワークテーブルから入出庫データ取り込み
        private async Task<bool> ReadNyuushuukoData(string loginUser)
        {

            // 入出庫データ取り込み(松本除外データ)
            var matumotoWNyuushuuko = await dbContext.WHokanNyuushuuko
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => x.n.Dtmoto == "2" && x.k.Ctlf03 == "1")
                                    .Select( x => new { Hincod = x.n.Hincod })
                                    .ToListAsync();

            // エラーログに出力
            bool fFlag = true;
            StringBuilder hError = new StringBuilder();
            foreach (var matumotoN in matumotoWNyuushuuko)
            {
                if (fFlag)
                {
                    hError.Append("入出庫データ 松本除外データ 品番コード：");
                    fFlag = false;
                }
                hError.Append(" " + matumotoN.Hincod + " ");
            }

            // 入出庫データ取り込み(三重除外データ)
            var mieWNyuushuuko = await dbContext.WHokanNyuushuuko
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => x.n.Dtmoto == "6" && x.k.Ctlf03 == "2")
                                    .Select(x => new { Hincod = x.n.Hincod })
                                    .ToListAsync();

            // エラーログに出力
            fFlag = true;
            foreach (var nieN in mieWNyuushuuko)
            {
                if (fFlag)
                {
                    if (hError.Length != 0) hError.Append("\n");
                    hError.Append("入出庫データ 三重除外データ 品番コード：");
                    fFlag = false;
                }
                hError.Append(" " + nieN.Hincod + " ");
            }

            if (hError.Length != 0)
            {
                error.Add(hError.ToString());
            }

            // 入出庫データ取り込み(対象データ)
            var nyuushuuko = await dbContext.WHokanNyuushuuko
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => (x.n.Dtmoto == "2" && x.k.Ctlf03 != "1") || (x.n.Dtmoto == "6" && x.k.Ctlf03 != "2"))
                                    .GroupBy(x => new { x.n.Kisyua, x.n.Kisyub, x.n.Hokcod, x.n.Hincod, x.n.Dkbn, x.n.Seikyu, x.n.Baitai })
                                    .Select(x => new
                                    {
                                        Kisyua = x.Key.Kisyua,
                                        Kisyub = x.Key.Kisyub,
                                        Hokcod = x.Key.Hokcod,
                                        Hincod = x.Key.Hincod,
                                        Dkbn = x.Key.Dkbn,
                                        Seikyu = x.Key.Seikyu,
                                        Baitai = x.Key.Baitai,
                                        Zansuu = x.Sum(y => y.n.Zansuu),
                                        Zankin = x.Sum(y => y.n.Zankin),
                                        Siksuu = x.Sum(y => y.n.Siksuu),
                                        Sikkin = x.Sum(y => y.n.Sikkin),
                                        Kaisuu = x.Sum(y => y.n.Kaisuu),
                                        Kaikin = x.Sum(y => y.n.Kaikin),
                                        Syksuu = x.Sum(y => y.n.Syksuu),
                                        Sykkin = x.Sum(y => y.n.Sykkin),
                                        Touzan = x.Sum(y => y.n.Touzan)
                                    })
                                    .ToListAsync();

            // データ登録
            foreach (var nData in nyuushuuko)
            {

                THokanNyuushuuko addData = new THokanNyuushuuko()
                {
                    Kisyua = nData.Kisyua,
                    Kisyub = nData.Kisyub,
                    Hokcod = nData.Hokcod,
                    Hincod = nData.Hincod,
                    Dkbn = nData.Dkbn,
                    Seikyu = nData.Seikyu,
                    Baitai = nData.Baitai,
                    Zansuu = nData.Zansuu,
                    Zankin = nData.Zankin,
                    Siksuu = nData.Siksuu,
                    Sikkin = nData.Sikkin,
                    Kaisuu = nData.Kaisuu,
                    Kaikin = nData.Kaikin,
                    Syksuu = nData.Syksuu,
                    Sykkin = nData.Sykkin,
                    Touzan = nData.Touzan,
                    Crtymd = DateTime.Now,
                    Crtcod = loginUser,
                    Updymd = DateTime.Now,
                    Updcod = loginUser
                };
                dbContext.THokanNyuushuuko.Add(addData);
                await dbContext.SaveChangesAsync();
            }
            return true;

        }

        // ワークテーブルから伝票件数データ取り込み
        private async Task<bool> ReadDenpyokensu(string loginUser)
        {

            // 伝票件数データ取り込み(松本分 除外データ)
            var matumotoWDenpyokensu = await dbContext.WHokanDenpyokensu
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => x.n.Dtmoto == "2" && x.k.Ctlf03 == "1")
                                    .Select(x => new { Hincod = x.n.Hincod })
                                    .ToListAsync();


            // エラーログに出力
            bool fFlag = true;
            StringBuilder hError = new StringBuilder();
            foreach (var matumotoD in matumotoWDenpyokensu)
            {
                if (fFlag)
                {
                    if (error.Count != 0) error.Add("\n");
                    hError.Append("伝票件数データ 松本除外データ 品番コード： ");
                    fFlag = false;
                }
                hError.Append(" " + matumotoD.Hincod + " ");
            }

            // 伝票件数データ取り込み(三重分 除外データ)
            var mieWDenpyokensu = await dbContext.WHokanDenpyokensu
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => x.n.Dtmoto == "6" && x.k.Ctlf03 == "2")
                                    .Select(x => new { Hincod = x.n.Hincod })
                                    .ToListAsync();

            // エラーログに出力
            fFlag = true;
            foreach (var mieD in mieWDenpyokensu)
            {
                if (fFlag)
                {
                    if (error.Count != 0) error.Add("\n");
                    hError.Append("伝票件数データ 三重除外データ 品番コード：");
                    fFlag = false;
                }
                hError.Append(" " + mieD.Hincod + " ");
            }

            if (hError.Length != 0)
            {
                error.Add(hError.ToString());
            }

            // 伝票件数データ取り込み(松本分 対象データ)
            var denpyokensu = await dbContext.WHokanDenpyokensu
                                    .Join(dbContext.MKishu, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .Where(x => (x.n.Dtmoto == "2" && x.k.Ctlf03 != "1") || (x.n.Dtmoto == "6" && x.k.Ctlf03 != "2"))
                                    .GroupBy(x => new { x.n.Kisyua, x.n.Kisyub, x.n.Basyo, x.n.Denkub, x.n.Seikyu, x.n.Inpymd, x.n.Keiymd, x.n.Hincod, x.n.Eigsok })
                                    .Select(x => new
                                    {
                                        Kisyua = x.Key.Kisyua,
                                        Kisyub = x.Key.Kisyub,
                                        Basyo = x.Key.Basyo,
                                        Denkub = x.Key.Denkub,
                                        Seikyu = x.Key.Seikyu,
                                        Inpymd = x.Key.Inpymd,
                                        Keiymd = x.Key.Keiymd,
                                        Hincod = x.Key.Hincod,
                                        Eigsok = x.Key.Eigsok,
                                        Densuu = x.Sum(y => y.n.Densuu),
                                        Nskosu = x.Sum(y => y.n.Nskosu)
                                    })
                                    .ToListAsync();

            // データ登録
            foreach (var dData in denpyokensu)
            {
                THokanDenpyokensu addData = new THokanDenpyokensu()
                {
                    Kisyua = dData.Kisyua,
                    Kisyub = dData.Kisyub,
                    Basyo = dData.Basyo,
                    Denkub = dData.Denkub,
                    Seikyu = dData.Seikyu,
                    Inpymd = dData.Inpymd,
                    Keiymd = dData.Keiymd,
                    Hincod = dData.Hincod,
                    Eigsok = dData.Eigsok,
                    Densuu = dData.Densuu,
                    Nskosu = dData.Nskosu,
                    Crtymd = DateTime.Now,
                    Crtcod = loginUser,
                    Updymd = DateTime.Now,
                    Updcod = loginUser
                };
                dbContext.THokanDenpyokensu.Add(addData);
                await dbContext.SaveChangesAsync();

            }

            return true;

        }

        // 入出庫繰越データ取り込み
        private async Task<bool> ReadNyuushuukoKurikosiData(string loginUser)
        {

            // 入出庫繰越データ取り込み
            var nyuushuukoKurikosi = await dbContext.THokanNyuushuukoKurikosi.OrderBy(x => x.Kurymd).ToListAsync();
            foreach (THokanNyuushuukoKurikosi nKurikosi in nyuushuukoKurikosi)
            {

                var nyuushuuko = await dbContext.THokanNyuushuuko
                                        .Where(x => x.Kisyua == nKurikosi.Kisyua && 
                                                    x.Kisyub == nKurikosi.Kisyub && 
                                                    x.Hokcod == nKurikosi.Hokcod && 
                                                    x.Hincod == nKurikosi.Hincod).ToListAsync();
                if (nyuushuuko.Count == 0)
                {
                    //　データが無い場合追加
                    THokanNyuushuuko addNyuushuuko = new THokanNyuushuuko()
                    {
                        Kisyua = nKurikosi.Kisyua,
                        Kisyub = nKurikosi.Kisyub,
                        Hokcod = nKurikosi.Hokcod,
                        Hincod = nKurikosi.Hincod,
                        Zansuu = nKurikosi.Zansuu,
                        Zankin = nKurikosi.Zankin,
                        Siksuu = nKurikosi.Siksuu,
                        Sikkin = nKurikosi.Sikkin,
                        Kaisuu = nKurikosi.Kaisuu,
                        Kaikin = nKurikosi.Kaikin,
                        Syksuu = nKurikosi.Syksuu,
                        Sykkin = nKurikosi.Sykkin,
                        Dkbn = nKurikosi.Dkbn,
                        Seikyu = nKurikosi.Seikyu,
                        Baitai = nKurikosi.Baitai,
                        Touzan = nKurikosi.Touzan,
                        Crtcod = loginUser,
                        Crtymd = DateTime.Now,
                        Updcod = loginUser,
                        Updymd = DateTime.Now
                    };
                    dbContext.THokanNyuushuuko.Add(addNyuushuuko);
                    await dbContext.SaveChangesAsync();

                }
                else
                {
                    //　データがある場合更新
                    var nData = nyuushuuko.First();
                    nData.Zansuu = nData.Zansuu + nKurikosi.Zansuu;
                    nData.Zankin = nData.Zankin + nKurikosi.Zankin;
                    nData.Siksuu = nData.Siksuu + nKurikosi.Siksuu;
                    nData.Sikkin = nData.Sikkin + nKurikosi.Sikkin;
                    nData.Kaisuu = nData.Kaisuu + nKurikosi.Kaisuu;
                    nData.Kaikin = nData.Kaikin + nKurikosi.Kaikin;
                    nData.Syksuu = nData.Syksuu + nKurikosi.Syksuu;
                    nData.Sykkin = nData.Sykkin + nKurikosi.Sykkin;
                    nData.Touzan = nData.Touzan + nKurikosi.Touzan;
                    nData.Updcod = loginUser;
                    nData.Updymd = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                }

            }
            return true;

        }

        // 伝票件数繰越データ取り込み
        private async Task<bool> ReadDenpyokensuKurikosiData(string loginUser)
        {

            // 伝票件数繰越データ取り込み
            var denpyokensuKurikosi = await dbContext.THokanDenpyokensuKurikosi.OrderBy(x => x.Kurymd).ToListAsync();
            foreach (THokanDenpyokensuKurikosi nKurikosi in denpyokensuKurikosi)
            {

                var denpyokensu = await dbContext.THokanDenpyokensu
                                            .Where(x => x.Kisyua == nKurikosi.Kisyua && 
                                                        x.Kisyub == nKurikosi.Kisyub && 
                                                        x.Basyo == nKurikosi.Basyo && 
                                                        x.Denkub == nKurikosi.Denkub &&
                                                        x.Inpymd == nKurikosi.Inpymd &&
                                                        x.Hincod == nKurikosi.Hincod &&
                                                        x.Keiymd == nKurikosi.Keiymd).ToListAsync();
                if (denpyokensu.Count == 0)
                {
                    //　データが無い場合追加
                    THokanDenpyokensu addDenpyokensu = new THokanDenpyokensu()
                    {
                        Kisyua = nKurikosi.Kisyua,
                        Kisyub = nKurikosi.Kisyub,
                        Basyo = nKurikosi.Basyo,
                        Denkub = nKurikosi.Denkub,
                        Densuu = nKurikosi.Densuu,
                        Seikyu = nKurikosi.Seikyu,
                        Inpymd = nKurikosi.Inpymd,
                        Keiymd = nKurikosi.Keiymd,
                        Hincod = nKurikosi.Hincod,
                        Nskosu = nKurikosi.Nskosu,
                        Eigsok = nKurikosi.Eigsok,
                        Crtcod = loginUser,
                        Crtymd = DateTime.Now,
                        Updcod = loginUser,
                        Updymd = DateTime.Now
                    };
                    dbContext.THokanDenpyokensu.Add(addDenpyokensu);
                    await dbContext.SaveChangesAsync();

                }
                else
                {
                    //　データがある場合更新
                    var nData = denpyokensu.First();
                    nData.Densuu = nData.Densuu + nKurikosi.Densuu;
                    nData.Nskosu = nData.Nskosu + nKurikosi.Nskosu;
                    nData.Updcod = loginUser;
                    nData.Updymd = DateTime.Now;
                    await dbContext.SaveChangesAsync();
                }
            }

            return true;
        }


        // 入出庫繰越データ・伝票件数繰越データ作成
        private async Task<bool> SetKurikosiData(string yyyymm, string loginUser)
        {

            string yymmdd = yyyymm + "/01";
            DateTime theDay = DateTime.Parse(yymmdd);
            theDay = theDay.AddMonths(1);
            theDay = theDay.AddDays(-1);
            string yymmddlast = theDay.ToString("yyyyMMdd");
            //string yymm = yyyymm.Replace("/", "");
            // 入出庫繰越データ・伝票件数繰越データ作成
            var nyuushuuko = await dbContext.THokanNyuushuuko
                                    .Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (n, k) => new { n, k })
                                    .GroupJoin(dbContext.MHokanSeihin, x => x.n.Hincod, x => x.Hincod, (n, s) => new { n = n.n, k = n.k, s })
                                    .SelectMany(x => x.s.DefaultIfEmpty(), (n, s) => new { n = n.n, k = n.k, s })
                                    .Select(x => new
                                    {
                                        Kisyua = x.n.Kisyua,
                                        Kisyub = x.n.Kisyub,
                                        Hokcod = x.n.Hokcod,
                                        Hincod = x.n.Hincod,
                                        Zansuu = x.n.Zansuu,
                                        Zankin = x.n.Zankin,
                                        Siksuu = x.n.Siksuu,
                                        Sikkin = x.n.Sikkin,
                                        Kaisuu = x.n.Kaisuu,
                                        Kaikin = x.n.Kaikin,
                                        Syksuu = x.n.Syksuu,
                                        Sykkin = x.n.Sykkin,
                                        Dkbn = x.n.Dkbn,
                                        Seikyu = x.n.Seikyu,
                                        Baitai = x.n.Baitai,
                                        Touzan = x.n.Touzan,
                                        Dtmoto = x.n.Dtmoto,
                                        Seitai = x.k.Seitai,
                                        Hokant = x.k.Hokant,
                                        Nieant = x.k.Nieant,
                                        Hokflg2 = x.k.Hokflg2,
                                        Hokflg4 = x.k.Hokflg4,
                                        Nieflg2 = x.k.Nieflg2,
                                        Hinnmk = x.s.Hinnmk
                                    })
                                    .ToListAsync();


            foreach (var nData in nyuushuuko)
            {

                // 請求契約マスタ.保管請求フラグ2 ＝ "C"  または
                // 請求契約マスタ.荷役請求フラグ2 ＝ "C" 
                if (!nData.Hokflg2.Equals("C") && !nData.Nieflg2.Equals("C"))
                {
                    continue;
                }

                // 除外データ
                // 前月残数、仕入倉入数、改造倉入数、 出庫数がすべてゼロ
                if (nData.Zansuu == 0 && nData.Siksuu == 0 && nData.Syksuu == 0)
                {
                    continue;
                }

                // 通貨のカストマ品
                // 請求契約マスタ.保管請求対象フラグ4 = "Y" かつ 製品マスタ.品番型式(カナ)先頭の3桁 = "Z41"
                if (nData.Hokflg4.Equals("Y") && DataUtil.SubstringEx(nData.Hinnmk, 0, 3).Equals("Z41"))
                {
                    continue;
                }

                // SGの内製基板(EDG)
                // 機種A = "SG" かつ 品番コードの先頭3桁 = "EDG" 
                if (nData.Kisyua.Equals("SG") && DataUtil.SubstringEx(nData.Hincod, 0, 3).Equals("EDG"))
                {
                    continue;
                }

                // 請求対象
                if (nData.Seitai == "" || nData.Seitai == "N")
                {
                    continue;
                }

                // 対象外データ
                string hokan = nData.Hokcod;
                string hokan1 = DataUtil.SubstringEx(nData.Hokcod, 0, 1);
                string kisyub2 = DataUtil.SubstringEx(nData.Kisyub, 0, 2);
                var joukenHokan = await dbContext.MHokanJouken.Where(x => x.Jyoken == hokan).ToArrayAsync();
                var joukenHokan1 = await dbContext.MHokanJouken.Where(x => x.Jyoken == hokan1).ToArrayAsync();
                var joukenKisyub2 = await dbContext.MHokanJouken.Where(x => x.Jyoken == kisyub2).ToArrayAsync();

                var jhokcod = joukenHokan.Where(x => x.Sikcod == "HOKCOD").ToArray();
                var jhokcod2 = joukenHokan1.Where(x => x.Sikcod == "HOKCOD2").ToArray();
                var jhokan = joukenHokan1.Where(x => x.Sikcod == "HOKAN").ToArray();

                var jniecod = joukenHokan.Where(x => x.Sikcod == "NIECOD").ToArray();
                var jniecod2 = joukenHokan1.Where(x => x.Sikcod == "NIECOD2").ToArray();
                var jnieki = joukenHokan1.Where(x => x.Sikcod == "NIEKI").ToArray();

                if (((nData.Hokflg2 == "A" && nData.Hokant == 0) || (jhokcod.Length != 0) || (jhokcod2.Length != 0) || (jhokan.Length != 0)) &&
                    ((nData.Nieflg2 == "A" && nData.Nieant == 0) || (jniecod.Length != 0) || (jniecod2.Length != 0) || (jnieki.Length != 0)))
                {
                    continue;
                }

                var mtanka = await dbContext.MHokanTanka
                                            .Where(x => x.Hincod == nData.Hincod).ToArrayAsync();

                if (mtanka.Count() == 0)
                {
                    // 入出庫繰越データ
                    var nyuushuukoKurikosi = await dbContext.THokanNyuushuukoKurikosi.Where(x => x.Kurymd == yymmddlast &&
                                                                                            x.Kisyua == nData.Kisyua && 
                                                                                            //x.Kisyub == nData.Kisyub && 
                                                                                            x.Hincod == nData.Hincod &&
                                                                                            x.Dkbn == nData.Dkbn &&
                                                                                            x.Hokcod == nData.Hokcod).ToArrayAsync();
                    if (nyuushuukoKurikosi.Count() == 0)
                    {
                        // 登録
                        THokanNyuushuukoKurikosi addNyuushuukoKurikosi = new THokanNyuushuukoKurikosi()
                        {
                            Kurymd = yymmddlast,
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
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now

                        };
                        dbContext.THokanNyuushuukoKurikosi.Add(addNyuushuukoKurikosi);
                        await dbContext.SaveChangesAsync();

                    }
                    else
                    {
                        // 更新
                        var update = nyuushuukoKurikosi.First();
                        update.Zansuu = update.Zansuu + nData.Zansuu;
                        update.Zankin = update.Zankin + nData.Zankin;
                        update.Siksuu = update.Siksuu + nData.Siksuu;
                        update.Sikkin = update.Sikkin + nData.Sikkin;
                        update.Kaisuu = update.Kaisuu + nData.Kaisuu;
                        update.Kaikin = update.Kaikin + nData.Kaikin;
                        update.Syksuu = update.Syksuu + nData.Syksuu;
                        update.Sykkin = update.Sykkin + nData.Sykkin;
                        update.Touzan = update.Touzan + nData.Touzan;
                        update.Updcod = loginUser;
                        update.Updymd = DateTime.Now;
                        await dbContext.SaveChangesAsync();
                    }

                    // 伝票件数データ繰越
                    var denpyokensus = await dbContext.THokanDenpyokensu.Where(x => x.Hincod == nData.Hincod).ToArrayAsync();
                    if (denpyokensus.Count() != 0)
                    {

                        foreach (var denpyokensu in denpyokensus)
                        {

                            //var denpyokensu = denpyokensus.First();
                            var denpyokensuKurikosis = await dbContext.THokanDenpyokensuKurikosi.Where(x => x.Kisyua == denpyokensu.Kisyua &&
                                                                                                        x.Hincod == denpyokensu.Hincod &&
                                                                                                        x.Basyo == denpyokensu.Basyo &&
                                                                                                        x.Denkub == denpyokensu.Denkub &&
                                                                                                        x.Inpymd == denpyokensu.Inpymd &&
                                                                                                        x.Keiymd == denpyokensu.Keiymd &&
                                                                                                        x.Eigsok == denpyokensu.Eigsok).ToArrayAsync();
                            if (denpyokensuKurikosis.Count() == 0)
                            {
                                // 登録
                                THokanDenpyokensuKurikosi insert = new THokanDenpyokensuKurikosi()
                                {
                                    Kurymd = yymmddlast,
                                    Kisyua = denpyokensu.Kisyua,
                                    Kisyub = denpyokensu.Kisyub,
                                    Basyo = denpyokensu.Basyo,
                                    Denkub = denpyokensu.Denkub,
                                    Densuu = denpyokensu.Densuu,
                                    Seikyu = denpyokensu.Seikyu,
                                    Inpymd = denpyokensu.Inpymd,
                                    Keiymd = denpyokensu.Keiymd,
                                    Hincod = denpyokensu.Hincod,
                                    Nskosu = denpyokensu.Nskosu,
                                    Eigsok = denpyokensu.Eigsok,
                                    Crtcod = loginUser,
                                    Crtymd = DateTime.Now,
                                    Updcod = loginUser,
                                    Updymd = DateTime.Now
                                };
                                dbContext.THokanDenpyokensuKurikosi.Add(insert);
                                await dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                // 更新
                                var update = denpyokensuKurikosis.First();
                                update.Densuu = update.Densuu + denpyokensu.Densuu;
                                update.Nskosu = update.Nskosu + denpyokensu.Nskosu;
                                update.Updcod = loginUser;
                                update.Updymd = DateTime.Now;
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            return true;

        }

        // 請求データB　品番単位
        private async Task<bool> SetSeikyuBHinban(string loginUser)
        {

            // 請求データB　品番単位
            //VHokanNyuushuuko vHokanNyuushuuko = new VHokanNyuushuuko();
            //var nyuushuukoList = await vHokanNyuushuuko.v_hokan_nyuushuuko
            var nyuushuukoList = await dbContext.VHokanNyuushuuko
                                        .Where(x => x.HOKANT > 0 && x.HOKANS == "Y")
                                        .Where(x => x.HOKFLG1 == "C" && x.HOKFLG4 == "Y")
                                        .GroupBy(x => new {
                                            x.SEIKYU,
                                            x.KISYUA,
                                            x.KISYUB,
                                            x.KISNAM,
                                            x.HINCOD,
                                            x.HOKCOD,
                                            x.HOKANT,
                                            x.HNEBIR,
                                            x.HINNMK,
                                            x.HOKFLG1,
                                            x.HOKFLG2,
                                            x.HOKFLG3,
                                            x.HOKFLG4,
                                            x.HOKFLG5,
                                            x.NIEFLG1,
                                            x.NIEFLG2,
                                            x.NIEFLG3,
                                            x.NIEFLG4,
                                            x.NIEFLG5,
                                            x.PCHOK
                                        })
                                        .Select(x => new
                                        {
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISYUB = x.Key.KISYUB,              // 機種B
                                            KISNAM = x.Key.KISNAM,              // 機種名
                                            HINCOD = x.Key.HINCOD,              // 品番コード
                                            HOKCOD = x.Key.HOKCOD,              // 保管場所
                                            HINNMK = x.Key.HINNMK,              // 品名形式
                                            HOKANT = x.Key.HOKANT,              // 保管単価
                                            HNEBIR = x.Key.HNEBIR,              // 保管値引率
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            PCHOK = x.Key.PCHOK,                // PCコード保管
                                            SIKSUU = x.Sum(y => y.SIKSUU),      // 仕込倉入数
                                            KAISUU = x.Sum(y => y.KAISUU),      // 改造倉入数
                                            DSYKSUU = x.Sum(y => y.DSYKSUU),    // 出庫数
                                            ZANSUU = x.Sum(y => y.ZANSUU)       // 前月残数
                                        }).ToListAsync();


            foreach (var nyuushuuko in nyuushuukoList)
            {
                // 存在チェック
                var check = await dbContext.THokanSeikyuB
                                            .Where(x => x.Seicod == nyuushuuko.SEIKYU &&
                                                    x.Kisyua == nyuushuuko.KISYUA &&
                                                    x.Kisyub == nyuushuuko.KISYUB &&
                                                    x.Hincod == nyuushuuko.HINCOD &&
                                                    x.Hokcod == nyuushuuko.HOKCOD &&
                                                    x.Pccod == nyuushuuko.PCHOK).ToArrayAsync();
                bool fFlag = true;
                if (check.Count() != 0)
                {
                    // エラー表示
                    if (fFlag)
                    {
                        if (error.Count != 0) error.Add("\n");
                        error.Add(string.Format("Fe保管請求請求データＢにすでにデータがあります。品番コード：{0} 保管場所：{1}", nyuushuuko.HOKCOD, nyuushuuko.HINCOD));
                        fFlag = false;
                    } else
                    {
                        error.Add(string.Format(" 品番コード：{0} 保管場所：{1}", nyuushuuko.HOKCOD, nyuushuuko.HINCOD));
                    }
                    continue;
                }

                // 入庫数 = 仕込倉入数 + 改造倉入数
                decimal NukoSu = (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;

                // 扱い数 = 入庫数 + 出庫数
                decimal AtukaiSu = NukoSu + (decimal)nyuushuuko.DSYKSUU;

                // 積数 = 前月残数 * 保管請求フラグ3(数値変換) + 入庫数
                decimal SekiSu = (decimal)nyuushuuko.ZANSUU * DataUtil.intParseEx(nyuushuuko.HOKFLG3) + NukoSu;

                // 保管料 = 積数 * 保管単価（四捨五入）
                decimal HokanR = Math.Round(SekiSu * (decimal)nyuushuuko.HOKANT);

                // 値引
                decimal Nebiki = 0;
                if (HokanR > 0 && nyuushuuko.HNEBIR > 0)
                {
                    Nebiki = Math.Round(HokanR * (decimal)nyuushuuko.HNEBIR);
                }

                THokanSeikyuB hokanSeikyuB = new THokanSeikyuB()
                {
                    Seicod = nyuushuuko.SEIKYU,
                    Kisyua = nyuushuuko.KISYUA,
                    Kisyub = nyuushuuko.KISYUB.Trim() == string.Empty ? NOT_KISYUB : nyuushuuko.KISYUB,
                    Kisnam = nyuushuuko.KISNAM,
                    Hincod = nyuushuuko.HINCOD,
                    Hokcod = nyuushuuko.HOKCOD,
                    Hinnmk = nyuushuuko.HINNMK,
                    Zansuu = nyuushuuko.ZANSUU,
                    Nyuksu = NukoSu,
                    Syksuu = nyuushuuko.DSYKSUU,
                    Sekisu = SekiSu,
                    Hokant = nyuushuuko.HOKANT,
                    Hokank = HokanR,
                    Hokankr = HokanR - Nebiki,
                    Atukai = AtukaiSu,
                    Hokflg1 = nyuushuuko.HOKFLG1,
                    Hokflg2 = nyuushuuko.HOKFLG2,
                    Hokflg3 = nyuushuuko.HOKFLG3,
                    Hokflg4 = nyuushuuko.HOKFLG4,
                    Hokflg5 = nyuushuuko.HOKFLG5,
                    Nieflg1 = nyuushuuko.NIEFLG1,
                    Nieflg2 = nyuushuuko.NIEFLG2,
                    Nieflg3 = nyuushuuko.NIEFLG3,
                    Nieflg4 = nyuushuuko.NIEFLG4,
                    Nieflg5 = nyuushuuko.NIEFLG5,
                    Pccod = nyuushuuko.PCHOK,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now

                };
                dbContext.THokanSeikyuB.Add(hokanSeikyuB);
                await dbContext.SaveChangesAsyncEx();
            }

            // 値引計算
            var seikyuListB = await dbContext.THokanSeikyuB
                                    .Where(x => x.Kisyub != "999" && x.Hokank != 0)
                                                .GroupBy(x => new {
                                                            x.Seicod,
                                                            x.Kisyua,
                                                            x.Kisnam,
                                                            x.Hokflg1,
                                                            x.Hokflg2,
                                                            x.Hokflg3,
                                                            x.Hokflg4,
                                                            x.Hokflg5,
                                                            x.Nieflg1,
                                                            x.Nieflg2,
                                                            x.Nieflg3,
                                                            x.Nieflg4,
                                                            x.Nieflg5,
                                                            x.Pccod
                                                })
                                                .Select(x => new
                                                {
                                                    SEICOD = x.Key.Seicod,              
                                                    KISYUA = x.Key.Kisyua,              // 機種A
                                                    KISNAM = x.Key.Kisnam,              // 機種名
                                                    HOKFLG1 = x.Key.Hokflg1,            // 保管請求フラグ1
                                                    HOKFLG2 = x.Key.Hokflg2,            // 保管請求フラグ2
                                                    HOKFLG3 = x.Key.Hokflg3,            // 保管請求フラグ3
                                                    HOKFLG4 = x.Key.Hokflg4,            // 保管請求フラグ4
                                                    HOKFLG5 = x.Key.Hokflg5,            // 保管請求フラグ5
                                                    NIEFLG1 = x.Key.Nieflg1,            // 荷役請求フラグ1
                                                    NIEFLG2 = x.Key.Nieflg2,            // 荷役請求フラグ2
                                                    NIEFLG3 = x.Key.Nieflg3,            // 荷役請求フラグ3
                                                    NIEFLG4 = x.Key.Nieflg4,            // 荷役請求フラグ4
                                                    NIEFLG5 = x.Key.Nieflg5,            // 荷役請求フラグ5
                                                    PCHOK = x.Key.Pccod,                // PCコード保管
                                                    HOKANK = x.Sum(y => y.Hokank),      // 仕込倉入数
                                                }).ToListAsync();



            foreach (var seikyu in seikyuListB)
            {
                decimal Nebiki = 0;
                // 値引率取得
                var vNyuushuukoList = await dbContext.VHokanNyuushuuko
                            .Where(x => x.KISYUA == seikyu.KISYUA)
                            .Select(x => new
                            {
                                KISYUA = x.KISYUA,              // 機種A
                                HNEBIR = x.HNEBIR,              // 保管値引率
                            }).ToListAsync();
                if (vNyuushuukoList.Count() != 0)
                {
                    var nyuushuuko = vNyuushuukoList.First();
                    Nebiki = Math.Round((decimal)nyuushuuko.HNEBIR * (decimal)seikyu.HOKANK);
                }
                else
                {
                    continue;
                }

                THokanSeikyuB nebiki = new THokanSeikyuB()
                {
                    Seicod = seikyu.SEICOD,
                    Kisyua = seikyu.KISYUA,
                    Kisyub = "999",
                    Kisnam = seikyu.KISNAM,
                    Hincod = seikyu.SEICOD,
                    Hokcod = "NE",
                    Hokank = Nebiki,
                    Pccod = seikyu.PCHOK,
                    Hokflg1 = seikyu.HOKFLG1,
                    Hokflg2 = seikyu.HOKFLG2,
                    Hokflg3 = seikyu.HOKFLG3,
                    Hokflg4 = seikyu.HOKFLG4,
                    Hokflg5 = seikyu.HOKFLG5,
                    Nieflg1 = seikyu.NIEFLG1,
                    Nieflg2 = seikyu.NIEFLG2,
                    Nieflg3 = seikyu.NIEFLG3,
                    Nieflg4 = seikyu.NIEFLG4,
                    Nieflg5 = seikyu.NIEFLG5,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now

                };
                dbContext.THokanSeikyuB.Add(nebiki);
                await dbContext.SaveChangesAsyncEx();
            }

            return true;

        }

        // 請求データ　品番単位
        private async Task<bool> SetSeikyuHinban(string loginUser)
        {

            // 請求データ　品番単位
            //VHokanNyuushuuko vHokanNyuushuuko = new VHokanNyuushuuko();
            //var nyuushuukoList = await vHokanNyuushuuko.v_hokan_nyuushuuko
            var nyuushuukoList = await dbContext.VHokanNyuushuuko
                                        .Where(x => x.HOKANT > 0 && x.HOKANS == "Y")
                                        .Where(x => x.HOKFLG1 == "C" && x.HOKFLG4 != "Y")
                                        .GroupBy(x => new {
                                            x.SEIKYU,
                                            x.KISYUA,
                                            x.KISYUB,
                                            x.KISNAM,
                                            x.HINCOD,
                                            x.HOKCOD,
                                            x.HOKANT,
                                            x.HNEBIR,
                                            x.HINNMK,
                                            x.HOKFLG1,
                                            x.HOKFLG2,
                                            x.HOKFLG3,
                                            x.HOKFLG4,
                                            x.HOKFLG5,
                                            x.NIEFLG1,
                                            x.NIEFLG2,
                                            x.NIEFLG3,
                                            x.NIEFLG4,
                                            x.NIEFLG5,
                                            x.PCHOK
                                        })
                                        .Select(x => new
                                        {
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISYUB = x.Key.KISYUB,              // 機種B
                                            KISNAM = x.Key.KISNAM,              // 機種名
                                            HINCOD = x.Key.HINCOD,              // 品番コード
                                            HOKCOD = x.Key.HOKCOD,              // 品番場所
                                            HINNMK = x.Key.HINNMK,              // 品名形式
                                            HOKANT = x.Key.HOKANT,              // 保管単価
                                            HNEBIR = x.Key.HNEBIR,              // 保管値引率
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            PCHOK = x.Key.PCHOK,                // PCコード保管
                                            SIKSUU = x.Sum(y => y.SIKSUU),      // 仕込倉入数
                                            KAISUU = x.Sum(y => y.KAISUU),      // 改造倉入数
                                            DSYKSUU = x.Sum(y => y.DSYKSUU),    // 出庫数
                                            ZANSUU = x.Sum(y => y.ZANSUU),      // 前月残数
                                        }).ToListAsync();


            foreach (var nyuushuuko in nyuushuukoList)
            {

                // 存在チェック
                var check = await dbContext.THokanSeikyu
                                            .Where(x => x.Seicod == nyuushuuko.SEIKYU &&
                                                    x.Kisyua == nyuushuuko.KISYUA &&
                                                    x.Kisyub == nyuushuuko.KISYUB &&
                                                    x.Hincod == nyuushuuko.HINCOD &&
                                                    x.Hokcod == nyuushuuko.HOKCOD &&
                                                    x.Pccod == nyuushuuko.PCHOK).ToArrayAsync();
                bool fFlag = true;
                if (check.Count() != 0)
                {
                    // エラー表示
                    if (fFlag)
                    {
                        if (error.Count != 0) error.Add("\n");
                        error.Add(string.Format("Fe保管請求請求データにすでにデータがあります。品番コード：{0} 保管場所：{1}", nyuushuuko.HOKCOD, nyuushuuko.HINCOD));
                        fFlag = false;
                    }
                    else
                    {
                        error.Add(string.Format(" 品番コード：{0} 保管場所：{1}", nyuushuuko.HOKCOD, nyuushuuko.HINCOD));
                    }
                    continue;
                }

                // 入庫数 = 仕込倉入数 + 改造倉入数
                decimal NukoSu = (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;

                // 扱い数 = 入庫数 + 出庫数
                decimal AtukaiSu = NukoSu + (decimal)nyuushuuko.DSYKSUU;

                // 積数 = 前月残数 * 保管請求フラグ3(数値変換) + 入庫数
                decimal SekiSu = (decimal)nyuushuuko.ZANSUU * DataUtil.intParseEx(nyuushuuko.HOKFLG3) + NukoSu;

                // 保管料 = 積数 * 保管単価（四捨五入）
                decimal HokanR = Math.Round(SekiSu * (decimal)nyuushuuko.HOKANT);

                // 値引
                decimal Nebiki = 0;
                if (HokanR > 0 && nyuushuuko.HNEBIR > 0)
                {
                    Nebiki = Math.Round(HokanR * (decimal)nyuushuuko.HNEBIR);
                }

                THokanSeikyu hokanSeikyu = new THokanSeikyu()
                {
                    Seicod = nyuushuuko.SEIKYU,
                    Kisyua = nyuushuuko.KISYUA,
                    Kisyub = nyuushuuko.KISYUB.Trim() == string.Empty ? NOT_KISYUB : nyuushuuko.KISYUB,
                    Kisnam = nyuushuuko.KISNAM,
                    Hincod = nyuushuuko.HINCOD,
                    Hokcod = nyuushuuko.HOKCOD,
                    Hinnmk = nyuushuuko.HINNMK,
                    Zansuu = nyuushuuko.ZANSUU,
                    Nyuksu = NukoSu,
                    Syksuu = nyuushuuko.DSYKSUU,
                    Sekisu = SekiSu,
                    Hokant = nyuushuuko.HOKANT,
                    Hokank = HokanR,
                    Hokankr = HokanR - Nebiki,
                    Atukai = AtukaiSu,
                    Hokflg1 = nyuushuuko.HOKFLG1,
                    Hokflg2 = nyuushuuko.HOKFLG2,
                    Hokflg3 = nyuushuuko.HOKFLG3,
                    Hokflg4 = nyuushuuko.HOKFLG4,
                    Hokflg5 = nyuushuuko.HOKFLG5,
                    Nieflg1 = nyuushuuko.NIEFLG1,
                    Nieflg2 = nyuushuuko.NIEFLG2,
                    Nieflg3 = nyuushuuko.NIEFLG3,
                    Nieflg4 = nyuushuuko.NIEFLG4,
                    Nieflg5 = nyuushuuko.NIEFLG5,
                    Pccod = nyuushuuko.PCHOK,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now

                };
                dbContext.THokanSeikyu.Add(hokanSeikyu);
                await dbContext.SaveChangesAsyncEx();
            }

            return true;

        }

        // 機種A、物流請求№、PCコード毎の集計
        private async Task<bool> SetSeikyuKisyuA(string loginUser)
        {

            // 機種A、物流請求№、PCコード毎の集計
            //VHokanNyuushuuko vHokanNyuushuuko = new VHokanNyuushuuko();
            //var nyuushuukoList = await vHokanNyuushuuko.v_hokan_nyuushuuko
            var nyuushuukoList = await dbContext.VHokanNyuushuuko
                                        .Where(x => x.HOKANT > 0 && x.HOKANS == "Y")
                                        //.Where(x => x.HOKFLG1 == "C")
                                        .GroupBy(x => new
                                        {
                                            x.SEIKYU,
                                            x.KISYUA,
                                            x.KISNAM,
                                            x.HOKCOD,
                                            x.HNEBIR,
                                            x.HOKANT,
                                            x.HOKFLG1,
                                            x.HOKFLG2,
                                            x.HOKFLG3,
                                            x.HOKFLG4,
                                            x.HOKFLG5,
                                            x.NIEFLG1,
                                            x.NIEFLG2,
                                            x.NIEFLG3,
                                            x.NIEFLG4,
                                            x.NIEFLG5,
                                            x.PCHOK
                                        })
                                        .Select(x => new
                                        {
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISNAM = x.Key.KISNAM,              // 機種名
                                            HOKCOD = x.Key.HOKCOD,              // 品番場所
                                            HOKANT = x.Key.HOKANT,              // 保管単価
                                            HNEBIR = x.Key.HNEBIR,              // 保管値引率
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            PCHOK = x.Key.PCHOK,                // PCコード保管
                                            SIKSUU = x.Sum(y => y.SIKSUU),      // 仕込倉入数
                                            KAISUU = x.Sum(y => y.KAISUU),      // 改造倉入数
                                            DSYKSUU = x.Sum(y => y.DSYKSUU),    // 出庫数
                                            ZANSUU = x.Sum(y => y.ZANSUU),      // 前月残数
                                        }).ToListAsync();


            foreach (var nyuushuuko in nyuushuukoList)
            {


                // 入庫数 = 仕込倉入数 + 改造倉入数
                decimal NukoSu = (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;

                // 扱い数 = 入庫数 + 出庫数
                decimal AtukaiSu = NukoSu + (decimal)nyuushuuko.DSYKSUU;

                // 積数 = 前月残数 * 保管請求フラグ3(数値変換) + 入庫数
                decimal SekiSu = (decimal)nyuushuuko.ZANSUU * DataUtil.intParseEx(nyuushuuko.HOKFLG3) + NukoSu;

                // 保管料 = 積数 * 保管単価（四捨五入）
                decimal HokanR = Math.Round(SekiSu * (decimal)nyuushuuko.HOKANT);

                // 値引
                decimal Nebiki = 0;
                if (HokanR > 0 && nyuushuuko.HNEBIR > 0)
                {
                    Nebiki = Math.Round(HokanR * (decimal)nyuushuuko.HNEBIR);
                }

                // 値引後保管料
                decimal HokanRN = HokanR - Nebiki;

                // 値引登録
                if (Nebiki > 0 && nyuushuuko.HOKFLG4 == "Y")
                {

                    // 存在チェック
                    THokanSeikyuB[] check = await dbContext.THokanSeikyuB
                                                .Where(x => x.Seicod == nyuushuuko.SEIKYU &&
                                                        x.Kisyua == nyuushuuko.KISYUA &&
                                                        x.Kisyub == "999" &&
                                                        x.Kisnam == nyuushuuko.KISNAM &&
                                                        x.Hincod == nyuushuuko.SEIKYU &&
                                                        x.Hokcod == "NE" &&
                                                        x.Pccod == nyuushuuko.PCHOK).ToArrayAsync();
                    if (check.Count() != 0)
                    {

                        THokanSeikyuB nebikiBUp = check.First();
                        nebikiBUp.Hokank = nebikiBUp.Hokank + Nebiki;
                        await dbContext.SaveChangesAsyncEx();

                    }
                    else
                    {

                        THokanSeikyuB nebikiB = new THokanSeikyuB()
                        {
                            Seicod = nyuushuuko.SEIKYU,
                            Kisyua = nyuushuuko.KISYUA,
                            Kisyub = "999",
                            Kisnam = nyuushuuko.KISNAM,
                            Hincod = nyuushuuko.SEIKYU,
                            Hokcod = "NE",
                            Hokank = Nebiki,
                            Pccod = nyuushuuko.PCHOK,
                            Hokflg1 = nyuushuuko.HOKFLG1,
                            Hokflg2 = nyuushuuko.HOKFLG2,
                            Hokflg3 = nyuushuuko.HOKFLG3,
                            Hokflg4 = nyuushuuko.HOKFLG4,
                            Hokflg5 = nyuushuuko.HOKFLG5,
                            Nieflg1 = nyuushuuko.NIEFLG1,
                            Nieflg2 = nyuushuuko.NIEFLG2,
                            Nieflg3 = nyuushuuko.NIEFLG3,
                            Nieflg4 = nyuushuuko.NIEFLG4,
                            Nieflg5 = nyuushuuko.NIEFLG5,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now

                        };
                        dbContext.THokanSeikyuB.Add(nebikiB);
                        await dbContext.SaveChangesAsyncEx();
                    }
                }
                
                //if (Nebiki > 0)
                //{

                //    // 存在チェック
                //    THokanSeikyu[] check3 = await dbContext.THokanSeikyu
                //                                .Where(x => x.Seicod == nyuushuuko.SEIKYU &&
                //                                        x.Kisyua == nyuushuuko.KISYUA &&
                //                                        x.Kisyub == "999" &&
                //                                        x.Kisnam == nyuushuuko.KISNAM &&
                //                                        x.Hincod == nyuushuuko.SEIKYU &&
                //                                        x.Hokcod == "NE" &&
                //                                        x.Pccod == nyuushuuko.PCHOK).ToArrayAsync();
                //    if (check3.Count() != 0)
                //    {

                //        THokanSeikyu nebikiUp = check3.First();
                //        nebikiUp.Hokank = nebikiUp.Hokank + Nebiki;
                //        await dbContext.SaveChangesAsyncEx();
                //    }
                //    else
                //    {

                //        THokanSeikyu nebiki = new THokanSeikyu()
                //        {
                //            Seicod = nyuushuuko.SEIKYU,
                //            Kisyua = nyuushuuko.KISYUA,
                //            Kisyub = "999",
                //            Kisnam = nyuushuuko.KISNAM,
                //            Hincod = nyuushuuko.SEIKYU,
                //            Hokcod = "NE",
                //            Hokank = Nebiki,
                //            Pccod = nyuushuuko.PCHOK,
                //            Hokflg1 = nyuushuuko.HOKFLG1,
                //            Hokflg2 = nyuushuuko.HOKFLG2,
                //            Hokflg3 = nyuushuuko.HOKFLG3,
                //            Hokflg4 = nyuushuuko.HOKFLG4,
                //            Hokflg5 = nyuushuuko.HOKFLG5,
                //            Nieflg1 = nyuushuuko.NIEFLG1,
                //            Nieflg2 = nyuushuuko.NIEFLG2,
                //            Nieflg3 = nyuushuuko.NIEFLG3,
                //            Nieflg4 = nyuushuuko.NIEFLG4,
                //            Nieflg5 = nyuushuuko.NIEFLG5,
                //            Crtcod = loginUser,
                //            Crtymd = DateTime.Now,
                //            Updcod = loginUser,
                //            Updymd = DateTime.Now

                //        };
                //        dbContext.THokanSeikyu.Add(nebiki);
                //        await dbContext.SaveChangesAsyncEx();
                //    }
                //}

                #region
                // PCコード集計行は不要 
                //if (HokanRN > 0)
                //{
                //    var seikyuList = await dbContext.THokanSeikyu
                //        .Where(x => x.Seicod == nyuushuuko.SEIKYU && x.Kisyua == nyuushuuko.KISYUA &&
                //                         x.Kisyub == "999" && x.Hokcod == "P1" &&
                //                         //x.Kisnam == nyuushuuko.HOKCOD &&
                //                         x.Hincod == nyuushuuko.SEIKYU && x.Pccod == nyuushuuko.PCHOK).ToListAsync();

                //    if (seikyuList.Count() == 0)
                //    {
                //        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                //        {
                //            Seicod = nyuushuuko.SEIKYU,
                //            Kisyua = nyuushuuko.KISYUA,
                //            Kisyub = "999",
                //            Kisnam = nyuushuuko.HOKCOD,
                //            Hincod = nyuushuuko.SEIKYU,
                //            Hokcod = "P1",
                //            Hokank = HokanRN,
                //            Hokflg1 = nyuushuuko.HOKFLG1,
                //            Hokflg2 = nyuushuuko.HOKFLG2,
                //            Hokflg3 = nyuushuuko.HOKFLG3,
                //            Hokflg4 = nyuushuuko.HOKFLG4,
                //            Hokflg5 = nyuushuuko.HOKFLG5,
                //            Nieflg1 = nyuushuuko.NIEFLG1,
                //            Nieflg2 = nyuushuuko.NIEFLG2,
                //            Nieflg3 = nyuushuuko.NIEFLG3,
                //            Nieflg4 = nyuushuuko.NIEFLG4,
                //            Nieflg5 = nyuushuuko.NIEFLG5,
                //            Pccod = nyuushuuko.PCHOK,
                //            Crtcod = loginUser,
                //            Crtymd = DateTime.Now,
                //            Updcod = loginUser,
                //            Updymd = DateTime.Now
                //        };
                //        dbContext.THokanSeikyu.Add(hokanSeikyu);
                //        await dbContext.SaveChangesAsyncEx();
                //    }
                //    else
                //    {
                //        var seikyu = seikyuList.First();
                //        seikyu.Hokank = seikyu.Hokank + HokanRN;
                //        await dbContext.SaveChangesAsyncEx();
                //    }

                //}
                #endregion

            }

            return true;

        }


        // 倉庫使用料
        private async Task<bool> SetSeikyuSouko(string loginUser)
        {
            // 倉庫使用料
            var seikyuBList = await dbContext.THokanSeikyuB
                                    .Join(dbContext.MHokanKeiyaku, x => x.Kisyua, x => x.Kisyua, (s, k) => new { s, k })
                                    .Where(x => (x.s.Kisyub != "999") &&
                                                x.k.Hokflg4 == "Y")
                                    .GroupBy(x => new {
                                        x.s.Seicod,
                                        x.s.Kisyua,
                                        x.s.Kisnam,
                                        x.s.Pccod,
                                        x.k.Hnebir,
                                        x.k.Hokflg1,
                                        x.k.Hokflg2,
                                        x.k.Hokflg3,
                                        x.k.Hokflg4,
                                        x.k.Hokflg5,
                                        x.k.Nieflg1,
                                        x.k.Nieflg2,
                                        x.k.Nieflg3,
                                        x.k.Nieflg4,
                                        x.k.Nieflg5,

                                    })
                                    .Select(x => new
                                    {
                                        Seicod = x.Key.Seicod,
                                        Kisyua = x.Key.Kisyua,
                                        Hnebir = x.Key.Hnebir,
                                        Kisnam = x.Key.Kisnam,
                                        Pccod = x.Key.Pccod,
                                        Hokflg1 = x.Key.Hokflg1,
                                        Hokflg2 = x.Key.Hokflg2,
                                        Hokflg3 = x.Key.Hokflg3,
                                        Hokflg4 = x.Key.Hokflg4,
                                        Nieflg1 = x.Key.Nieflg1,
                                        Nieflg2 = x.Key.Nieflg2,
                                        Nieflg3 = x.Key.Nieflg3,
                                        Nieflg4 = x.Key.Nieflg4,
                                        Zansuu = x.Sum(y => y.s.Zansuu),
                                        Nyuksu = x.Sum(y => y.s.Nyuksu),
                                        Hokank = x.Sum(y => y.s.Hokank),
                                        Hokankr = x.Sum(y => y.s.Hokankr)
                                    })
                                    .ToListAsync();
            foreach (var seikyuB in seikyuBList)
            {

                // 存在チェック
                THokanSeikyu[] check = await dbContext.THokanSeikyu
                                            .Where(x => x.Seicod == seikyuB.Seicod &&
                                                    x.Kisyua == seikyuB.Kisyua &&
                                                    x.Kisyub == "996" &&
                                                    x.Kisnam == seikyuB.Kisnam &&
                                                    x.Hincod == seikyuB.Seicod &&
                                                    x.Hokcod == "HO" &&
                                                    x.Pccod == seikyuB.Pccod).ToArrayAsync();
                bool fFlag = true;
                if (check.Count() != 0)
                {
                    // エラー表示
                    if (fFlag)
                    {
                        if (error.Count != 0) error.Add("\n");
                        error.Add(string.Format("Fe保管請求請求データにすでにデータがあります。請求先コード：{0} 保管場所{1} ", seikyuB.Seicod, seikyuB.Kisyua));
                        fFlag = false;
                    }
                    else
                    {
                        error.Add(string.Format(" 請求先コード：{0} 保管場所{1} ", seikyuB.Seicod, seikyuB.Kisyua));
                    }
                    continue;
                }

                // 面積
                //decimal Menseki = (decimal)seikyuB.Hokank / 2500;
                //decimal Menseki = Math.Round((decimal)seikyuB.Hokank / 2500, 2, MidpointRounding.AwayFromZero);
                decimal work = (decimal)seikyuB.Hokank / 2500;
                decimal Menseki = Math.Truncate(work * 100) / 100;

                // 保管料
                decimal HokanR = Math.Round(Menseki * 2500);

                // 面積(値引)
                decimal Mensekir = (decimal)seikyuB.Hokankr / 2500;
                //decimal Mensekir = Math.Round((decimal)seikyuB.Hokankr / 2500, 2, MidpointRounding.AwayFromZero);

                // 保管料(値引)
                decimal HokanRr = Math.Round(Mensekir * 2500);

                // 値引
                decimal Nebiki = 0;
                if (seikyuB.Hnebir > 0 && seikyuB.Hokank > 0)
                {
                    Nebiki = Math.Round((decimal)seikyuB.Hokank * (decimal)seikyuB.Hnebir);
                }

                THokanSeikyu hokanSeikyu = new THokanSeikyu()
                {
                    Seicod = seikyuB.Seicod,
                    Kisyua = seikyuB.Kisyua,
                    Kisyub = "996",
                    Kisnam = seikyuB.Kisnam,
                    Hincod = seikyuB.Seicod,
                    Hokcod = "HO",
                    Zansuu = seikyuB.Zansuu,
                    Nyuksu = seikyuB.Nyuksu,
                    Sekisu = Menseki,
                    Hokant = 2500,
                    Hokank = HokanR,
                    Hokankr = HokanRr,
                    Hokflg1 = seikyuB.Hokflg1,
                    Hokflg2 = seikyuB.Hokflg2,
                    Hokflg3 = seikyuB.Hokflg3,
                    Hokflg4 = seikyuB.Hokflg4,
                    Nieflg1 = seikyuB.Nieflg1,
                    Nieflg2 = seikyuB.Nieflg2,
                    Nieflg3 = seikyuB.Nieflg3,
                    Nieflg4 = seikyuB.Nieflg4,
                    Pccod = seikyuB.Pccod,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now
                };
                dbContext.THokanSeikyu.Add(hokanSeikyu);
                await dbContext.SaveChangesAsyncEx();

            }

            // 値引計算
            List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                                    .Where(x => (x.Kisyub == "996") &&
                                                x.Hokcod == "HO").ToListAsync();

            foreach (var seikyu in seikyuList)
            {

                decimal Nebiki = 0;
                // 値引率取得
                var nyuushuukoList = await dbContext.VHokanNyuushuuko
                            .Where(x => x.KISYUA == seikyu.Kisyua)
                            .Select(x => new
                            {
                                KISYUA = x.KISYUA,              // 機種A
                                HNEBIR = x.HNEBIR,              // 保管値引率
                            }).ToListAsync();
                if (nyuushuukoList.Count() != 0)
                {
                    var nyuushuuko = nyuushuukoList.First();
                    Nebiki = Math.Round((decimal)nyuushuuko.HNEBIR * (decimal)seikyu.Hokank);
                } else
                {
                    continue;
                }

                THokanSeikyu nebiki = new THokanSeikyu()
                {
                    Seicod = seikyu.Seicod,
                    Kisyua = seikyu.Kisyua,
                    Kisyub = "999",
                    Kisnam = seikyu.Kisnam,
                    Hincod = seikyu.Seicod,
                    Hokcod = "NE",
                    Hokank = Nebiki,
                    Pccod = seikyu.Pccod,
                    Hokflg1 = seikyu.Hokflg1,
                    Hokflg2 = seikyu.Hokflg2,
                    Hokflg3 = seikyu.Hokflg3,
                    Hokflg4 = seikyu.Hokflg4,
                    Hokflg5 = seikyu.Hokflg5,
                    Nieflg1 = seikyu.Nieflg1,
                    Nieflg2 = seikyu.Nieflg2,
                    Nieflg3 = seikyu.Nieflg3,
                    Nieflg4 = seikyu.Nieflg4,
                    Nieflg5 = seikyu.Nieflg5,
                    Crtcod = loginUser,
                    Crtymd = DateTime.Now,
                    Updcod = loginUser,
                    Updymd = DateTime.Now

                };
                dbContext.THokanSeikyu.Add(nebiki);
                await dbContext.SaveChangesAsyncEx();

                seikyu.Hokankr = seikyu.Hokank - Nebiki;
                await dbContext.SaveChangesAsyncEx();

            }

            return true;

        }


        // 請求データ作成 入出庫件数荷役料　品番コード単位
        private async Task<bool> SetSeikyuNiekiHinbanN(string loginUser)
        {

            //VHokanNyuushuuko vHokanNyuushuuko = new VHokanNyuushuuko();
            //var nyuushuukoList = await vHokanNyuushuuko.v_hokan_nyuushuuko
            var nyuushuukoList = await dbContext.VHokanNyuushuuko
                                        .Where(x => x.NIEKIT > 0 && x.NIEKIS == "Y" && x.NIEFLG3 == "N")
                                        .Select(x => new
                                        {
                                            SEIKYU = x.SEIKYU,              // 物流請求No
                                            KISYUA = x.KISYUA,              // 機種A
                                            KISYUB = x.KISYUB,              // 機種B
                                            KISNAM = x.KISNAM,              // 機種名
                                            HINCOD = x.HINCOD,              // 品番コード
                                            HOKCOD = x.HOKCOD,              // 保管場所
                                            HINNMK = x.HINNMK,              // 品名形式
                                            NIEKIT = x.NIEKIT,              // 荷役単価
                                            NNEBIR = x.NNEBIR,              // 荷役値引率
                                            HOKFLG1 = x.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.NIEFLG5,            // 荷役請求フラグ5
                                            PCNIE = x.PCNIE,                // PCコード荷役
                                            SIKSUU = x.SIKSUU,              // 仕込倉入数
                                            KAISUU = x.KAISUU,              // 改造倉入数
                                            DSYKSUU = x.DSYKSUU,            // 出庫数
                                            ZANSUU = x.ZANSUU               // 前月残数
                                        }).ToListAsync();

            foreach (var nyuushuuko in nyuushuukoList)
            {

                // 入庫数
                decimal NukoSu = (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;

                // 扱い数
                decimal AtukaiSu = NukoSu + (decimal)nyuushuuko.DSYKSUU;

                // 荷役料
                decimal NiekiR = Math.Round(AtukaiSu * (decimal)nyuushuuko.NIEKIT);

                // 値引
                decimal Nebiki = 0;
                if (nyuushuuko.NNEBIR > 0 && NiekiR > 0)
                {
                    Nebiki = Math.Round(NiekiR * (decimal)nyuushuuko.NNEBIR);
                }

                if (NiekiR > 0 && nyuushuuko.NIEFLG1 == "C")
                {

                    List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                            .Where(x => x.Seicod == nyuushuuko.SEIKYU && x.Kisyua == nyuushuuko.KISYUA &&
                                             x.Kisyub == nyuushuuko.KISYUB && x.Hincod == nyuushuuko.HINCOD &&
                                             x.Kisnam == nyuushuuko.KISNAM && x.Pccod == nyuushuuko.PCNIE &&
                                             x.Hokcod == nyuushuuko.HOKCOD).ToListAsync();

                    if (seikyuList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = nyuushuuko.SEIKYU,
                            Kisyua = nyuushuuko.KISYUA,
                            Kisyub = nyuushuuko.KISYUB,
                            Kisnam = nyuushuuko.KISNAM,
                            Hincod = nyuushuuko.HINCOD,
                            Hokcod = nyuushuuko.HOKCOD,
                            Zansuu = nyuushuuko.ZANSUU,
                            Nyuksu = NukoSu,
                            Syksuu = nyuushuuko.DSYKSUU,
                            Atukai = AtukaiSu,
                            Niekit = nyuushuuko.NIEKIT,
                            Niekik = NiekiR,
                            Niekikr = NiekiR - Nebiki,
                            Hokflg1 = nyuushuuko.HOKFLG1,
                            Hokflg2 = nyuushuuko.HOKFLG2,
                            Hokflg3 = nyuushuuko.HOKFLG3,
                            Hokflg4 = nyuushuuko.HOKFLG4,
                            Hokflg5 = nyuushuuko.HOKFLG5,
                            Nieflg1 = nyuushuuko.NIEFLG1,
                            Nieflg2 = nyuushuuko.NIEFLG2,
                            Nieflg3 = nyuushuuko.NIEFLG3,
                            Nieflg4 = nyuushuuko.NIEFLG4,
                            Nieflg5 = nyuushuuko.NIEFLG5,
                            Pccod = nyuushuuko.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now

                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();
                    }
                    else
                    {
                        var seikyu = seikyuList.First();
                        seikyu.Kisnam = nyuushuuko.KISNAM;
                        seikyu.Zansuu = nyuushuuko.ZANSUU;
                        seikyu.Nyuksu = NukoSu;
                        seikyu.Densuu = nyuushuuko.DSYKSUU;
                        seikyu.Atukai = AtukaiSu;
                        seikyu.Niekit = nyuushuuko.NIEKIT;
                        seikyu.Niekik = NiekiR;
                        seikyu.Niekikr = NiekiR - Nebiki;
                        await dbContext.SaveChangesAsyncEx();
                    }
                }

            }
            return true;

        }

        // 請求データ作成 入出庫件数荷役料　PCコード単位
        private async Task<bool> SetSeikyuNiekiPcN(string loginUser)
        {

            //VHokanNyuushuuko vHokanNyuushuuko = new VHokanNyuushuuko();
            //var nyuushuukoList = await vHokanNyuushuuko.v_hokan_nyuushuuko
            var nyuushuukoList = await dbContext.VHokanNyuushuuko
                                        .Where(x => x.NIEKIT > 0 && x.NIEKIS == "Y" && x.NIEFLG3 == "N")
                                        .GroupBy(x => new { x.KISYUA, x.KISNAM, x.SEIKYU, x.PCNIE, x.HOKCOD, x.NIEKIT, x.NNEBIR,
                                            x.HOKFLG1, x.HOKFLG2, x.HOKFLG3, x.HOKFLG4, x.HOKFLG5,
                                            x.NIEFLG1, x.NIEFLG2, x.NIEFLG3, x.NIEFLG4, x.NIEFLG5 })
                                        .Select(x => new
                                        {
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISNAM = x.Key.KISNAM,              // 機種名
                                            HOKCOD = x.Key.HOKCOD,              // 保管場所
                                            NIEKIT = x.Key.NIEKIT,              // 荷役単価
                                            NNEBIR = x.Key.NNEBIR,              // 荷役値引率
                                            PCNIE = x.Key.PCNIE,                // PCコード荷役
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            ZANSUU = x.Sum(y => y.ZANSUU),      // 前月残数
                                            SIKSUU = x.Sum(y => y.SIKSUU),      // 仕込倉入数
                                            KAISUU = x.Sum(y => y.KAISUU),      // 改造倉入数
                                            DSYKSUU = x.Sum(y => y.DSYKSUU)     // 出庫数
                                        }).ToListAsync();

            foreach (var nyuushuuko in nyuushuukoList)
            {

                // 入庫数
                decimal NukoSu = (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;

                // 扱い数
                decimal AtukaiSu = NukoSu + (decimal)nyuushuuko.DSYKSUU;

                // 荷役料
                decimal NiekiR = Math.Round(AtukaiSu * (decimal)nyuushuuko.NIEKIT);

                // 値引
                decimal Nebiki = 0;
                if (NiekiR > 0 && nyuushuuko.NNEBIR > 0)
                {
                    Nebiki = Math.Round(NiekiR * (decimal)nyuushuuko.NNEBIR);
                }

                // 値引後荷役料
                decimal NiekiRN = NiekiR - Nebiki;

                #region 
                // PCコード集計j行は不要
                //if (NiekiRN != 0)
                //{
                //    var seikyuList = await dbContext.THokanSeikyu
                //                            .Where(x => x.Seicod == nyuushuuko.SEIKYU && x.Kisyua == nyuushuuko.KISYUA &&
                //                                             x.Kisyub == "999" && x.Hokcod == "P2" &&
                //                                             //x.Kisnam == nyuushuuko.HOKCOD &&
                //                                             x.Hincod == nyuushuuko.SEIKYU && x.Pccod == nyuushuuko.PCNIE).ToListAsync();

                //    if (seikyuList.Count() == 0)
                //    {
                //        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                //        {
                //            Seicod = nyuushuuko.SEIKYU,
                //            Kisyua = nyuushuuko.KISYUA,
                //            Kisyub = "999",
                //            Kisnam = nyuushuuko.HOKCOD,
                //            Hincod = nyuushuuko.SEIKYU,
                //            Hokcod = "P2",
                //            Zansuu = nyuushuuko.ZANSUU,
                //            Nyuksu = NukoSu,
                //            Syksuu = nyuushuuko.DSYKSUU,
                //            Niekit = nyuushuuko.NIEKIT,
                //            Niekik = NiekiRN,
                //            Atukai = AtukaiSu,
                //            Hokflg1 = nyuushuuko.HOKFLG1,
                //            Hokflg2 = nyuushuuko.HOKFLG2,
                //            Hokflg3 = nyuushuuko.HOKFLG3,
                //            Hokflg4 = nyuushuuko.HOKFLG4,
                //            Hokflg5 = nyuushuuko.HOKFLG5,
                //            Nieflg1 = nyuushuuko.NIEFLG1,
                //            Nieflg2 = nyuushuuko.NIEFLG2,
                //            Nieflg3 = nyuushuuko.NIEFLG3,
                //            Nieflg4 = nyuushuuko.NIEFLG4,
                //            Nieflg5 = nyuushuuko.NIEFLG5,
                //            Pccod = nyuushuuko.PCNIE,
                //            Crtcod = loginUser,
                //            Crtymd = DateTime.Now,
                //            Updcod = loginUser,
                //            Updymd = DateTime.Now
                //        };
                //        dbContext.THokanSeikyu.Add(hokanSeikyu);
                //        await dbContext.SaveChangesAsyncEx();
                //    }
                //    else
                //    {
                //        var seikyu = seikyuList.First();
                //        seikyu.Zansuu = nyuushuuko.ZANSUU;
                //        seikyu.Nyuksu = seikyu.Nyuksu + NukoSu;
                //        seikyu.Densuu = seikyu.Densuu + nyuushuuko.DSYKSUU;
                //        seikyu.Atukai = seikyu.Atukai + AtukaiSu;
                //        seikyu.Niekit = nyuushuuko.NIEKIT;
                //        seikyu.Niekik = seikyu.Niekik + NiekiRN;
                //        await dbContext.SaveChangesAsyncEx();

                //    }
                //}
                #endregion

                // 値引
                if (Nebiki > 0)
                {

                    var seikyuNebikiList = await dbContext.THokanSeikyu
                        .Where(x => x.Seicod == nyuushuuko.SEIKYU && x.Kisyua == nyuushuuko.KISYUA &&
                                         x.Kisyub == "999" && x.Hincod == nyuushuuko.SEIKYU &&
                                         x.Hokcod == "NE" && x.Pccod == nyuushuuko.PCNIE).ToListAsync();

                    if (seikyuNebikiList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = nyuushuuko.SEIKYU,
                            Kisyua = nyuushuuko.KISYUA,
                            Kisyub = "999",
                            Kisnam = nyuushuuko.KISNAM,
                            Hincod = nyuushuuko.SEIKYU,
                            Hokcod = "NE",
                            Niekik = Nebiki,
                            Hokflg1 = nyuushuuko.HOKFLG1,
                            Hokflg2 = nyuushuuko.HOKFLG2,
                            Hokflg3 = nyuushuuko.HOKFLG3,
                            Hokflg4 = nyuushuuko.HOKFLG4,
                            Hokflg5 = nyuushuuko.HOKFLG5,
                            Nieflg1 = nyuushuuko.NIEFLG1,
                            Nieflg2 = nyuushuuko.NIEFLG2,
                            Nieflg3 = nyuushuuko.NIEFLG3,
                            Nieflg4 = nyuushuuko.NIEFLG4,
                            Nieflg5 = nyuushuuko.NIEFLG5,
                            Pccod = nyuushuuko.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();

                    } else
                    {
                        var seikyu = seikyuNebikiList.First();
                        seikyu.Niekik = Nebiki;
                        await dbContext.SaveChangesAsyncEx();
                    }
                }
            }

            return true;

        }


        // 請求データ作成 入出庫件数荷役料
        private async Task<bool> SetSeikyuNieki(string loginUser)
        {

            bool fFlag = true;
            bool fFlagB = true;
            //VHokanDenpyokensu vHokanDenpyokensu = new VHokanDenpyokensu();
            //var denpyokensuList = await vHokanDenpyokensu.v_hokan_denpyokensu
            var denpyokensuList = await dbContext.VHokanDenpyokensu
                                        .Where(x => x.NIEANT > 0 && x.NIEKIS == "Y" && x.NIEFLG3 == "D")
                                        .GroupBy(x => new
                                        {
                                            x.KISYUA,
                                            x.KISNAM,
                                            x.SEIKYU,
                                            x.KYUJIT,       // 休日識別
                                            x.NNEBIR,       // 荷役値引率
                                            x.NIEANT,       // 荷役単価
                                            x.NIEKYT,       // 荷役単価（休日用）
                                            x.PCNIE,
                                            x.HOKFLG1,
                                            x.HOKFLG2,
                                            x.HOKFLG3,
                                            x.HOKFLG4,
                                            x.HOKFLG5,
                                            x.NIEFLG1,
                                            x.NIEFLG2,
                                            x.NIEFLG3,
                                            x.NIEFLG4,
                                            x.NIEFLG5
                                        })
                                        .Select(x => new
                                        {
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISNAM = x.Key.KISNAM,              // 機種名
                                            KYUJIT = x.Key.KYUJIT,              // 休日識別
                                            NNEBIR = x.Key.NNEBIR,              // 荷役値引率
                                            NIEKIT = x.Key.NIEANT,              // 荷役単価
                                            NIEKYT = x.Key.NIEKYT,              // 荷役単価（休日用）
                                            PCNIE = x.Key.PCNIE,                // PCコード荷役
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            DENSUU = x.Sum(y => y.DENSUU)       // 伝票件数
                                        }).ToListAsync();

            foreach (var denpyokensu in denpyokensuList)
            {

                // 荷役料(休日)
                decimal NiekiRK = 0;
                if (denpyokensu.KYUJIT == "Y" && denpyokensu.NIEKYT > 0 && denpyokensu.DENSUU > 0)
                {
                    NiekiRK = Math.Round((decimal)denpyokensu.DENSUU * (decimal)denpyokensu.NIEKYT);
                }

                // 荷役料
                decimal NiekiR = 0;
                if (denpyokensu.KYUJIT != "Y" && denpyokensu.NIEKIT > 0 && denpyokensu.DENSUU > 0)
                {
                    NiekiR = Math.Round((decimal)denpyokensu.DENSUU * (decimal)denpyokensu.NIEKIT);
                }

                // 値引
                decimal Nebiki = 0;
                if (denpyokensu.NNEBIR > 0 && NiekiR > 0)
                {
                    Nebiki = Math.Round((decimal)denpyokensu.NNEBIR * NiekiR);
                }

                // 値引荷役料
                decimal NebikiNR = NiekiR - Nebiki;

                if (denpyokensu.NIEFLG1 == "A" && denpyokensu.HOKFLG1 == "A" && NiekiR > 0)
                {
                    List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                                                .Where(x => x.Seicod == denpyokensu.SEIKYU && 
                                                            x.Kisyua == denpyokensu.KISYUA &&
                                                            x.Kisyub == NOT_KISYUB && 
                                                            x.Hokcod == NOT_HOKCOD &&
                                                            x.Hincod == denpyokensu.SEIKYU && 
                                                            x.Pccod == denpyokensu.PCNIE).ToListAsync();

                    if (seikyuList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = NOT_KISYUB,
                            Kisnam = denpyokensu.KISNAM,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = NOT_HOKCOD,
                            Densuu = denpyokensu.DENSUU,
                            Niekit = denpyokensu.NIEKIT,
                            Niekik = NiekiR,
                            Niekikr = NebikiNR,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();

                    } else
                    {

                        var seikyu = seikyuList.First();
                        seikyu.Kisnam = denpyokensu.KISNAM;
                        seikyu.Densuu = denpyokensu.DENSUU;
                        seikyu.Niekit = denpyokensu.NIEKIT;
                        seikyu.Niekik = NiekiR;
                        seikyu.Niekikr = NebikiNR;
                        await dbContext.SaveChangesAsyncEx();

                    }

                }

                if (denpyokensu.NIEFLG1 == "A" && denpyokensu.HOKFLG1 == "C" && NiekiR > 0) {

                    List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                            .Where(x => x.Seicod == denpyokensu.SEIKYU && 
                                        x.Kisyua == denpyokensu.KISYUA &&
                                        x.Kisyub == "997" && 
                                        x.Hokcod == "NI" &&
                                        x.Hincod == denpyokensu.SEIKYU && 
                                        x.Pccod == denpyokensu.PCNIE).ToListAsync();

                    if (seikyuList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = "997",
                            Kisnam = denpyokensu.KISNAM,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = "NI",
                            Densuu = denpyokensu.DENSUU,
                            Niekit = denpyokensu.NIEKIT,
                            Niekik = NiekiR,
                            Niekikr = NebikiNR,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();
                    } else
                    {
                        if (fFlag)
                        {
                            // エラーログ
                            if (error.Count != 0) error.Add("\n");
                            error.Add(string.Format("Fe保管請求請求データにすでにデータがあります。機種A：{0} 請求先コード{1} ", denpyokensu.KISYUA, denpyokensu.SEIKYU));
                            fFlag = false;
                        } else
                        {
                            error.Add(string.Format("{0} 請求先コード{1}", denpyokensu.KISYUA, denpyokensu.SEIKYU));
                        }
                        continue;
                    }

                }

                if (denpyokensu.NIEFLG1 == "A" && denpyokensu.HOKFLG1 == "C" && NiekiR > 0 && denpyokensu.HOKFLG4 == "Y")
                {

                    List<THokanSeikyuB> seikyuBList = await dbContext.THokanSeikyuB
                            .Where(x => x.Seicod == denpyokensu.SEIKYU && 
                                        x.Kisyua == denpyokensu.KISYUA &&
                                        x.Kisyub == "997" && 
                                        x.Hokcod == "NI" &&
                                        x.Hincod == denpyokensu.SEIKYU && 
                                        x.Pccod == denpyokensu.PCNIE).ToListAsync();

                    if (seikyuBList.Count() == 0)
                    {
                        THokanSeikyuB hokanSeikyuB = new THokanSeikyuB()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = "997",
                            Kisnam = denpyokensu.KISNAM,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = "NI",
                            Densuu = denpyokensu.DENSUU,
                            Niekit = denpyokensu.NIEKIT,
                            Niekik = NiekiR,
                            Niekikr = NebikiNR,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyuB.Add(hokanSeikyuB);
                        await dbContext.SaveChangesAsyncEx();

                    } else
                    {
                        if (fFlagB)
                        {
                            // エラーログ
                            if (error.Count != 0) error.Add("\n");
                            error.Add(string.Format("Fe保管請求請求データBにすでにデータがあります。機種A：{0} 請求先コード{1} ", denpyokensu.KISYUA, denpyokensu.SEIKYU));
                            fFlagB = false;
                        }
                        else
                        {
                            error.Add(string.Format("{0} 請求先コード{1}", denpyokensu.KISYUA, denpyokensu.SEIKYU));
                        }

                        continue;
                    }

                }

                // 休日分
                if (NiekiRK > 0)
                {
                    List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                                            .Where(x => x.Seicod == denpyokensu.SEIKYU && 
                                                        x.Kisyua == denpyokensu.KISYUA &&
                                                        x.Kisyub == "998" && 
                                                        x.Hokcod == "NI" &&
                                                        x.Hincod == denpyokensu.SEIKYU && 
                                                        x.Pccod == denpyokensu.PCNIE).ToListAsync();
                    if (seikyuList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = "998",
                            Kisnam = denpyokensu.KISNAM,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = "NI",
                            Densuu = denpyokensu.DENSUU,
                            Niekit = denpyokensu.NIEKYT,
                            Niekik = NiekiRK,
                            Niekikr = NiekiRK - Nebiki,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();

                    } else
                    {
                        if (fFlag)
                        {
                            // エラーログ
                            if (error.Count != 0) error.Add("\n");
                            error.Add(string.Format("Fe保管請求請求データにすでにデータがあります。機種A：{0} 請求先コード{1} ", denpyokensu.KISYUA, denpyokensu.SEIKYU));
                            fFlag = false;
                        }
                        else
                        {
                            error.Add(string.Format("{0} 請求先コード{1}", denpyokensu.KISYUA, denpyokensu.SEIKYU));
                        }
                        continue;
                    }
                }

                // 値引
                if (Nebiki > 0 && denpyokensu.HOKFLG4 == "Y")
                {

                    List<THokanSeikyuB> seikyuBList = await dbContext.THokanSeikyuB
                                        .Where(x => x.Seicod == denpyokensu.SEIKYU && 
                                                    x.Kisyua == denpyokensu.KISYUA &&
                                                    x.Kisyub == "999" && x.Hokcod == "NE" &&
                                                    x.Hincod == denpyokensu.SEIKYU && 
                                                    x.Pccod == denpyokensu.PCNIE).ToListAsync();

                    if (seikyuBList.Count() == 0)
                    {
                        THokanSeikyuB hokanSeikyuB = new THokanSeikyuB()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = "999",
                            Kisnam = denpyokensu.KISNAM,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = "NE",
                            Niekik = Nebiki,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyuB.Add(hokanSeikyuB);
                        await dbContext.SaveChangesAsyncEx();

                    } else
                    {
                        var seikyuB = seikyuBList.First();
                        seikyuB.Niekik = Nebiki;
                        await dbContext.SaveChangesAsyncEx();

                    }

                }

                if (Nebiki > 0)
                {

                    List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                                            .Where(x => x.Seicod == denpyokensu.SEIKYU && 
                                                        x.Kisyua == denpyokensu.KISYUA &&
                                                        x.Kisyub == "999" && 
                                                        x.Hokcod == "NE" &&
                                                        x.Hincod == denpyokensu.SEIKYU && 
                                                        x.Pccod == denpyokensu.PCNIE).ToListAsync();

                    if (seikyuList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = "999",
                            Kisnam = denpyokensu.KISNAM,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = "NE",
                            Niekik = Nebiki,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();
                    }
                    else
                    {
                        THokanSeikyu seikyu = seikyuList.First();
                        seikyu.Niekik = Nebiki;
                        await dbContext.SaveChangesAsyncEx();

                    }

                }

            }
             return true;

        }


        // 請求データ作成 入出庫件数荷役料 PCコード単位
        private async Task<bool> SetSeikyuNiekiPc(string loginUser)
        {

            //VHokanDenpyokensu vHokanDenpyokensu = new VHokanDenpyokensu();
            //var denpyokensuList = await vHokanDenpyokensu.v_hokan_denpyokensu
            var denpyokensuList = await dbContext.VHokanDenpyokensu
                                        .Where(x => x.NIEANT > 0 && x.NIEKIS == "Y" && x.NIEFLG3 == "D")
                                        .GroupBy(x => new
                                        {
                                            x.KISYUA,
                                            x.KISNAM,
                                            x.SEIKYU,
                                            x.BASYO,
                                            x.KYUJIT,       // 休日識別
                                            x.NNEBIR,       // 荷役値引率
                                            x.NIEANT,       // 荷役単価
                                            x.NIEKYT,       // 荷役単価（休日用）
                                            x.PCNIE,
                                            x.HOKFLG1,
                                            x.HOKFLG2,
                                            x.HOKFLG3,
                                            x.HOKFLG4,
                                            x.HOKFLG5,
                                            x.NIEFLG1,
                                            x.NIEFLG2,
                                            x.NIEFLG3,
                                            x.NIEFLG4,
                                            x.NIEFLG5
                                        })
                                        .Select(x => new
                                        {
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISNAM = x.Key.KISNAM,              // 機種名
                                            BASYO = x.Key.BASYO,                // 保管場所
                                            KYUJIT = x.Key.KYUJIT,              // 休日識別
                                            NNEBIR = x.Key.NNEBIR,              // 荷役値引率
                                            NIEKIT = x.Key.NIEANT,              // 荷役単価
                                            NIEKYT = x.Key.NIEKYT,              // 荷役単価（休日用）
                                            PCNIE = x.Key.PCNIE,                // PCコード荷役
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            DENSUU = x.Sum(y => y.DENSUU)       // 伝票件数
                                        }).ToListAsync();

            foreach (var denpyokensu in denpyokensuList)
            {

                // 荷役料(休日)
                decimal NiekiRK = 0;
                if (denpyokensu.KYUJIT == "Y" && denpyokensu.NIEKYT > 0 && denpyokensu.DENSUU > 0)
                {
                    NiekiRK = Math.Round((decimal)denpyokensu.DENSUU * (decimal)denpyokensu.NIEKYT);
                }

                // 荷役料
                decimal NiekiR = 0;
                if (denpyokensu.KYUJIT != "Y" && denpyokensu.NIEKIT > 0 && denpyokensu.DENSUU > 0)
                {
                    NiekiR = Math.Round((decimal)denpyokensu.DENSUU * (decimal)denpyokensu.NIEKIT);
                }

                // 値引
                decimal Nebiki = 0;
                if (denpyokensu.NNEBIR > 0 && NiekiR > 0)
                {
                    Nebiki = Math.Round((decimal)denpyokensu.NNEBIR * NiekiR);
                }

                // 値引荷役料
                decimal NebikiNR = (NiekiR + NiekiRK) - Nebiki;

                if (NebikiNR != 0)
                {
                    List<THokanSeikyu> seikyuList = await dbContext.THokanSeikyu
                                                .Where(x => x.Seicod == denpyokensu.SEIKYU && x.Kisyua == denpyokensu.KISYUA &&
                                                            x.Kisyub == "999" && x.Hokcod == "P2" &&
                                                            //x.Kisnam == denpyokensu.BASYO && 
                                                            x.Hincod == denpyokensu.SEIKYU && x.Pccod == denpyokensu.PCNIE).ToListAsync();

                    if (seikyuList.Count() == 0)
                    {
                        THokanSeikyu hokanSeikyu = new THokanSeikyu()
                        {
                            Seicod = denpyokensu.SEIKYU,
                            Kisyua = denpyokensu.KISYUA,
                            Kisyub = "999",
                            Kisnam = denpyokensu.BASYO,
                            Hincod = denpyokensu.SEIKYU,
                            Hokcod = "P2",
                            Densuu = denpyokensu.DENSUU,
                            Niekit = denpyokensu.NIEKIT,
                            Niekik = NebikiNR,
                            Hokflg1 = denpyokensu.HOKFLG1,
                            Hokflg2 = denpyokensu.HOKFLG2,
                            Hokflg3 = denpyokensu.HOKFLG3,
                            Hokflg4 = denpyokensu.HOKFLG4,
                            Hokflg5 = denpyokensu.HOKFLG5,
                            Nieflg1 = denpyokensu.NIEFLG1,
                            Nieflg2 = denpyokensu.NIEFLG2,
                            Nieflg3 = denpyokensu.NIEFLG3,
                            Nieflg4 = denpyokensu.NIEFLG4,
                            Nieflg5 = denpyokensu.NIEFLG5,
                            Pccod = denpyokensu.PCNIE,
                            Crtcod = loginUser,
                            Crtymd = DateTime.Now,
                            Updcod = loginUser,
                            Updymd = DateTime.Now
                        };
                        dbContext.THokanSeikyu.Add(hokanSeikyu);
                        await dbContext.SaveChangesAsyncEx();

                    }
                    else
                    {
                        var seikyu = seikyuList.First();
                        seikyu.Densuu = seikyu.Densuu + denpyokensu.DENSUU;
                        seikyu.Niekit = denpyokensu.NIEKIT;
                        seikyu.Niekik = seikyu.Niekik + NebikiNR;
                        await dbContext.SaveChangesAsyncEx();
                    }
                }
            }

            return true;
        }

        private async Task<bool> SetKyotenData(string yyyyMm, string loginUser, System.Web.Mvc.ModelStateDictionary error)
        {

            try
            {
                // 拠点別データ作成 入出庫データ
                await SetSeikyuKyotenbetuN(yyyyMm, loginUser);

                // 拠点別データ作成 伝票件数
                await SetSeikyuKyotenbetuD(yyyyMm, loginUser);

                // 差額調整
                await SetSeikyuKyotenbetuCheck(loginUser);

            }
            catch (DbEntityValidationException vEx)
            {
                DataUtil.PrintEntityValidationErrors(vEx.EntityValidationErrors);
                throw vEx;
            } catch (Exception ex)
            {
                throw ex;
            }

            return true;

        }


        // 拠点別データ作成 入出庫データ
        private async Task<bool> SetSeikyuKyotenbetuN(string yyyyMm, string loginUser)
        {
            // 場所単位集計
            var nyuushuukoList = await dbContext.VHokanNyuushuuko
                                        .GroupBy(x => new
                                        {
                                            x.HOKCOD,                           // 保管場所
                                            x.KISYUA,                           // 機種A
                                            x.KISYUB,                           // 機種B
                                            x.SEIKYU,                           // 物流請求№
                                            x.HOKANS,                           // 保管請求対象
                                            x.HNEBIR,                           // 保管値引率
                                            x.HOKANT,                           // 保管単価
                                            x.NIEKIS,                           // 荷役請求対象
                                            x.NNEBIR,                           // 荷役値引率
                                            x.NIEKIT,                           // 荷役単価
                                            x.PCHOK,
                                            x.PCNIE,
                                            x.HOKFLG1,
                                            x.HOKFLG2,
                                            x.HOKFLG3,
                                            x.HOKFLG4,
                                            x.HOKFLG5,
                                            x.NIEFLG1,
                                            x.NIEFLG2,
                                            x.NIEFLG3,
                                            x.NIEFLG4,
                                            x.NIEFLG5
                                        })
                                        .Select(x => new
                                        {
                                            HOKCOD = x.Key.HOKCOD,              // 保管場所
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISYUB = x.Key.KISYUB,              // 機種B
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            HOKANS = x.Key.HOKANS,              // 保管請求対象
                                            HOKANT = x.Key.HOKANT,              // 保管単価
                                            HNEBIR = x.Key.HNEBIR,              // 保管値引率
                                            NIEKIS = x.Key.NIEKIS,              // 荷役請求対象
                                            NNEBIR = x.Key.NNEBIR,              // 荷役値引率
                                            NIEKIT = x.Key.NIEKIT,              // 荷役単価
                                            PCHOK = x.Key.PCHOK,                // PCコード保管
                                            PCNIE = x.Key.PCNIE,                // PCコード荷役
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            ZANSUU = x.Sum(y => y.ZANSUU),      // 前月残数
                                            ZANKIN = x.Sum(y => y.ZANKIN),      // 前月残金額                                            
                                            SIKSUU = x.Sum(y => y.SIKSUU),      // 仕込倉入数
                                            SIKKIN = x.Sum(y => y.SIKKIN),      // 仕込倉入金額
                                            KAISUU = x.Sum(y => y.KAISUU),      // 改造倉入数
                                            KAIKIN = x.Sum(y => y.KAIKIN),      // 改造倉入金額
                                            DSYKSUU = x.Sum(y => y.DSYKSUU),    // 出庫数
                                            SYKKIN = x.Sum(y => y.SYKKIN)       // 出庫金額
                                        })
                                        .OrderBy(x => x.HOKCOD)
                                        .ThenBy(x => x.KISYUA)
                                        .ThenBy(x => x.KISYUB)
                                        .ThenBy(x => x.SEIKYU)
                                        .ToListAsync();

            bool fFlag = true;
            string hokcod = string.Empty;
            string kisyua = string.Empty;
            string kisyub = string.Empty;
            string seikyu = string.Empty;
            string pchok = string.Empty;
            string pcnie = string.Empty;

            // 入庫数
            decimal Nuko = 0;
            // 入庫金額
            decimal NukoK = 0;
            // 在庫数
            decimal Zaiko = 0;
            // 在庫金額
            decimal ZaikoR = 0;
            // 保管料
            decimal HokanR = 0;
            // 荷役料
            decimal NiekiR = 0;

            decimal Zansuu = 0;
            decimal Zankin = 0;
            decimal Siksuu = 0;
            decimal Sikkin = 0;
            decimal Kaisuu = 0;
            decimal Kaikin = 0;
            decimal Dsyksuu = 0;
            decimal Sykkin = 0;

            int last = nyuushuukoList.Count;
            int idx = 0;
            THokanSeikyuKyoten back = new THokanSeikyuKyoten();
            foreach (var nyuushuuko in nyuushuukoList)
            {

                idx++;
                if (fFlag)
                {
                    hokcod = nyuushuuko.HOKCOD;
                    kisyua = nyuushuuko.KISYUA;
                    kisyub = nyuushuuko.KISYUB;
                    seikyu = nyuushuuko.SEIKYU;
                    pchok = nyuushuuko.PCHOK;
                    pcnie = nyuushuuko.PCNIE;
                    fFlag = false;
                }

                if (hokcod != nyuushuuko.HOKCOD || kisyua != nyuushuuko.KISYUA || kisyub != nyuushuuko.KISYUB || seikyu != nyuushuuko.SEIKYU)
                {
                    if (nyuushuuko.ZANSUU > 0 || Nuko > 0 || nyuushuuko.DSYKSUU > 0)
                    {
                        string sybnam = string.Empty;
                        MShukkabasho[] mBasho = await dbContext.MShukkabasho.Where(x => x.Sybcod == hokcod).ToArrayAsync();
                        if (mBasho.Count() != 0)
                        {
                            MShukkabasho basho = mBasho.First();
                            sybnam = basho.Sybnam;
                        }
                        string yymm = yyyyMm.Replace("/", "");
                        yymm = DataUtil.SubstringEx(yymm, 2, 4);

                        // 在庫数
                        Zaiko = Zansuu + Nuko - Dsyksuu;

                        // 在庫金額
                        ZaikoR = Zankin + NukoK - Sykkin;

                        // 保管料
                        decimal Menseki = 0;
                        if (nyuushuuko.HOKFLG4 == "Y" && HokanR > 0)
                        {
                            Menseki = HokanR / 2500;
                            HokanR = Math.Round(Menseki * 2500, 0, MidpointRounding.AwayFromZero);
                        }

                        // 値引
                        decimal NebikiH = 0;
                        if (nyuushuuko.HNEBIR > 0 && HokanR > 0)
                        {
                            NebikiH = Math.Round(HokanR * (decimal)nyuushuuko.HNEBIR, 0, MidpointRounding.AwayFromZero);
                            // 値引保管料
                            HokanR = HokanR - NebikiH;
                        }

                        HokanR = Math.Round(HokanR, 0, MidpointRounding.AwayFromZero);

                        if (HokanR != 0 || NiekiR != 0 || Zansuu != 0 || Zankin != 0 || Sikkin != 0 || Kaisuu != 0 || Kaikin != 0 || Dsyksuu != 0 || Sykkin != 0)
                        {
                            if (HokanR == 0)
                            {
                                pchok = string.Empty;
                            }

                            if (NiekiR == 0)
                            {
                                pcnie = string.Empty;
                            }

                            THokanSeikyuKyoten hokanSeikyuKyoten = new THokanSeikyuKyoten()
                            {
                                Basyo = hokcod,
                                Basnam = sybnam,
                                Kisyua = kisyua,
                                Kisyub = kisyub.Trim() == string.Empty ? NOT_KISYUB : kisyub,
                                Kisybn = kisyub,
                                Seicod = seikyu,
                                Zansuu = Zansuu,
                                Zankin = Zankin,
                                Siksuu = Siksuu,
                                Sikkin = Sikkin,
                                Kaisuu = Kaisuu,
                                Kaikin = Kaikin,
                                Nyuksu = Nuko,
                                Nyukin = NukoK,
                                Syksuu = Dsyksuu,
                                Sykkin = Sykkin,
                                Zaiksu = Zaiko,
                                Zaikin = ZaikoR,
                                Densuu = 0,
                                Densky = 0,
                                Hokank = HokanR,
                                Niekik = NiekiR,
                                Niekyj = 0,
                                Pccodh = pchok,
                                Pccodn = pcnie,
                                Dataym = yymm,
                                Crtcod = loginUser,
                                Crtymd = DateTime.Now,
                                Updcod = loginUser,
                                Updymd = DateTime.Now
                            };
                            dbContext.THokanSeikyuKyoten.Add(hokanSeikyuKyoten);
                            await dbContext.SaveChangesAsync();
                        }
                    }

                    hokcod = nyuushuuko.HOKCOD;
                    kisyua = nyuushuuko.KISYUA;
                    kisyub = nyuushuuko.KISYUB;
                    seikyu = nyuushuuko.SEIKYU;
                    pchok = nyuushuuko.PCHOK;
                    pcnie = nyuushuuko.PCNIE;
                    Nuko = 0;
                    NukoK = 0;
                    Zaiko = 0;
                    ZaikoR = 0;
                    HokanR = 0;
                    NiekiR = 0;
                    Zansuu = 0;
                    Zankin = 0;
                    Siksuu = 0;
                    Sikkin = 0;
                    Kaisuu = 0;
                    Kaikin = 0;
                    Dsyksuu = 0;
                    Sykkin = 0;
                }

                Zansuu += (decimal)nyuushuuko.ZANSUU;
                Zankin += (decimal)nyuushuuko.ZANKIN;
                Siksuu += (decimal)nyuushuuko.SIKSUU;
                Sikkin += (decimal)nyuushuuko.SIKKIN;
                Kaisuu += (decimal)nyuushuuko.KAISUU;
                Kaikin += (decimal)nyuushuuko.KAIKIN;
                Dsyksuu += (decimal)nyuushuuko.DSYKSUU;
                Sykkin += (decimal)nyuushuuko.SYKKIN;

                // 入庫数
                Nuko += (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;

                // 入庫金額
                NukoK += (decimal)nyuushuuko.SIKKIN + (decimal)nyuushuuko.KAIKIN;

                // 扱い数
                decimal Atukai = Nuko + (decimal)nyuushuuko.DSYKSUU;

                // 積数
                decimal Sekisu = 0;

                // 保管料                
                if (nyuushuuko.HOKANT > 0 && nyuushuuko.HOKANS == "Y")
                {
                    // 積数
                    Sekisu = (decimal)nyuushuuko.ZANSUU * DataUtil.intParseEx(nyuushuuko.HOKFLG3) + (decimal)nyuushuuko.SIKSUU + (decimal)nyuushuuko.KAISUU;
                    if (Sekisu > 0)
                    {
                        // 保管料
                        HokanR += Sekisu * (decimal)nyuushuuko.HOKANT;
                    }
                }

                // 荷役料
                if (nyuushuuko.NIEFLG3 == "N" && nyuushuuko.NIEKIT > 0 && nyuushuuko.NIEKIS == "Y")
                {
                    if (Atukai > 0)
                    {
                        // 荷役料
                        NiekiR = Math.Round(Atukai * (decimal)nyuushuuko.NIEKIT);
                    }
                }

                if (idx == last)
                {
                    if (nyuushuuko.ZANSUU > 0 || Nuko > 0 || nyuushuuko.DSYKSUU > 0)
                    {
                        string sybnam = string.Empty;
                        MShukkabasho[] mBasho = await dbContext.MShukkabasho.Where(x => x.Sybcod == hokcod).ToArrayAsync();
                        if (mBasho.Count() != 0)
                        {
                            MShukkabasho basho = mBasho.First();
                            sybnam = basho.Sybnam;
                        }

                        // 保管料
                        decimal Menseki = 0;
                        if (nyuushuuko.HOKFLG4 == "Y" && HokanR > 0)
                        {
                            Menseki = HokanR / 2500;
                            HokanR = Math.Round(Menseki * 2500, 0, MidpointRounding.AwayFromZero);
                        }

                        // 値引
                        decimal NebikiH = 0;
                        if (nyuushuuko.HNEBIR > 0 && HokanR > 0)
                        {
                            NebikiH = Math.Round(HokanR * (decimal)nyuushuuko.HNEBIR, 0, MidpointRounding.AwayFromZero);
                            // 値引保管料
                            HokanR = HokanR - NebikiH;
                        }

                        // 在庫数
                        Zaiko = Zansuu + Nuko - Dsyksuu;

                        // 在庫金額
                        ZaikoR = Zankin + NukoK - Sykkin;

                        if (HokanR != 0 || NiekiR != 0 || Zansuu != 0 || Zankin != 0 || Sikkin != 0 || Kaisuu != 0 || Kaikin != 0 || Dsyksuu != 0 || Sykkin != 0)
                        {

                            if (HokanR == 0)
                            {
                                pchok = string.Empty;
                            }

                            if (NiekiR == 0)
                            {
                                pcnie = string.Empty;
                            }

                            HokanR = Math.Round(HokanR, 0, MidpointRounding.AwayFromZero);
                            string yymm = yyyyMm.Replace("/", "");
                            yymm = DataUtil.SubstringEx(yymm, 2, 4);
                            THokanSeikyuKyoten hokanSeikyuKyoten = new THokanSeikyuKyoten()
                            {
                                Basyo = hokcod,
                                Basnam = sybnam,
                                Kisyua = kisyua,
                                Kisyub = kisyub.Trim() == string.Empty ? NOT_KISYUB : kisyub,
                                Kisybn = kisyub,
                                Seicod = seikyu,
                                Zansuu = Zansuu,
                                Zankin = Zankin,
                                Siksuu = Siksuu,
                                Sikkin = Sikkin,
                                Kaisuu = Kaisuu,
                                Kaikin = Kaikin,
                                Nyuksu = Nuko,
                                Nyukin = NukoK,
                                Syksuu = Dsyksuu,
                                Sykkin = Sykkin,
                                Zaiksu = Zaiko,
                                Zaikin = ZaikoR,
                                Densuu = 0,
                                Densky = 0,
                                Hokank = HokanR,
                                Niekik = NiekiR,
                                Niekyj = 0,
                                Pccodh = pchok,
                                Pccodn = pcnie,
                                Dataym = yymm,
                                Crtcod = loginUser,
                                Crtymd = DateTime.Now,
                                Updcod = loginUser,
                                Updymd = DateTime.Now
                            };
                            dbContext.THokanSeikyuKyoten.Add(hokanSeikyuKyoten);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }

           return true;

        }

        // 拠点別データ作成 伝票件数
        private async Task<bool> SetSeikyuKyotenbetuD(string yyyyMm, string loginUser)
        {
            string yymm = yyyyMm.Replace("/", "");
            yymm = DataUtil.SubstringEx(yymm, 2, 4);

            // 場所単位集計
            var denpyokensuList = await dbContext.VHokanDenpyokensu
                                        .Join(dbContext.MShukkabasho, x => x.BASYO, x => x.Sybcod, (d, s) => new { d, s })
                                        .Where(x => x.d.NIEKIS == "Y")
                                        .GroupBy(x => new
                                        {
                                            x.d.BASYO,                            // 回収場所
                                            x.d.KISYUA,                           // 機種A
                                            x.d.KISYUB,                           // 機種B
                                            x.d.SEIKYU,                           // 物流請求№
                                            x.d.HOKANS,                           // 保管請求対象
                                            x.d.NIEKIS,                           // 荷役請求対象
                                            x.d.NNEBIR,                           // 荷役値引率
                                            x.d.NIEANT,                           // 荷役単価
                                            x.d.NIEKYT,                           // 荷役単価（休日用）
                                            x.d.KYUJIT,                           // 休日識別
                                            x.d.PCNIE,
                                            x.d.HOKFLG1,
                                            x.d.HOKFLG2,
                                            x.d.HOKFLG3,
                                            x.d.HOKFLG4,
                                            x.d.HOKFLG5,
                                            x.d.NIEFLG1,
                                            x.d.NIEFLG2,
                                            x.d.NIEFLG3,
                                            x.d.NIEFLG4,
                                            x.d.NIEFLG5
                                        })
                                        .Select(x => new
                                        {
                                            BASYO = x.Key.BASYO,                // 回収場所
                                            KISYUA = x.Key.KISYUA,              // 機種A
                                            KISYUB = x.Key.KISYUB,              // 機種B
                                            SEIKYU = x.Key.SEIKYU,              // 物流請求No
                                            HOKANS = x.Key.HOKANS,              // 保管請求対象
                                            NIEKIS = x.Key.NIEKIS,              // 荷役請求対象
                                            NNEBIR = x.Key.NNEBIR,              // 荷役値引率
                                            NIEANT = x.Key.NIEANT,              // 荷役単価
                                            NIEKYT = x.Key.NIEKYT,              // 荷役単価（休日用）
                                            KYUJIT = x.Key.KYUJIT,              // 休日識別
                                            PCNIE = x.Key.PCNIE,                // PCコード荷役
                                            HOKFLG1 = x.Key.HOKFLG1,            // 保管請求フラグ1
                                            HOKFLG2 = x.Key.HOKFLG2,            // 保管請求フラグ2
                                            HOKFLG3 = x.Key.HOKFLG3,            // 保管請求フラグ3
                                            HOKFLG4 = x.Key.HOKFLG4,            // 保管請求フラグ4
                                            HOKFLG5 = x.Key.HOKFLG5,            // 保管請求フラグ5
                                            NIEFLG1 = x.Key.NIEFLG1,            // 荷役請求フラグ1
                                            NIEFLG2 = x.Key.NIEFLG2,            // 荷役請求フラグ2
                                            NIEFLG3 = x.Key.NIEFLG3,            // 荷役請求フラグ3
                                            NIEFLG4 = x.Key.NIEFLG4,            // 荷役請求フラグ4
                                            NIEFLG5 = x.Key.NIEFLG5,            // 荷役請求フラグ5
                                            DENSUU = x.Sum(y => y.d.DENSUU)       // 伝票件数
                                        })
                                        .OrderBy(x => x.KISYUA)
                                        .ThenBy(x => x.SEIKYU)
                                        .ToListAsync();

            int last = denpyokensuList.Count;
            int idx = 0;
            bool fFlag = true;
            string basyo = string.Empty;
            string kisyua = string.Empty;
            string kisyub = string.Empty;
            string seikyu = string.Empty;
            string pcnie = string.Empty;

            // 荷役料（平日）
            decimal NiekiR = 0;
            // 荷役料（休日）
            decimal NiekiRK = 0;

            decimal Densuu = 0;
            decimal Densky = 0;

            foreach (var denpyokensu in denpyokensuList)
            {

                idx++;
                if (fFlag)
                {
                    basyo = denpyokensu.BASYO;
                    kisyua = denpyokensu.KISYUA;
                    kisyub = denpyokensu.KISYUB;
                    seikyu = denpyokensu.SEIKYU;
                    pcnie = denpyokensu.PCNIE;
                    fFlag = false;
                }

                if (basyo != denpyokensu.BASYO || kisyua != denpyokensu.KISYUA || kisyub != denpyokensu.KISYUB || seikyu != denpyokensu.SEIKYU)
                {

                    string sybnam = string.Empty;
                    MShukkabasho[] mBasho = await dbContext.MShukkabasho.Where(x => x.Sybcod == basyo).ToArrayAsync();
                    if (mBasho.Count() != 0)
                    {
                        MShukkabasho basho = mBasho.First();
                        sybnam = basho.Sybnam;
                    }

                    string kisyubw = kisyub;
                    if (kisyub.Trim() == string.Empty)
                    {
                        kisyubw = NOT_KISYUB;
                    }
                    if (NiekiR != 0 || NiekiRK != 0 || Densuu != 0 || Densky != 0)
                    {
                        List<THokanSeikyuKyoten> kyoten = await dbContext.THokanSeikyuKyoten.Where(x => x.Basyo == basyo && x.Kisyua == kisyua && x.Kisyub == kisyubw).ToListAsync();
                        if (kyoten.Count() == 0)
                        {
                            THokanSeikyuKyoten hokanSeikyuKyoten = new THokanSeikyuKyoten()
                            {
                                Basyo = basyo,
                                Basnam = sybnam,
                                Kisyua = kisyua,
                                Kisyub = kisyub.Trim() == string.Empty ? NOT_KISYUB : kisyub,
                                Kisybn = kisyub,
                                Seicod = seikyu,
                                Zansuu = 0,
                                Zankin = 0,
                                Siksuu = 0,
                                Sikkin = 0,
                                Kaisuu = 0,
                                Kaikin = 0,
                                Nyuksu = 0,
                                Nyukin = 0,
                                Syksuu = 0,
                                Sykkin = 0,
                                Zaiksu = 0,
                                Zaikin = 0,
                                Densuu = Densuu,
                                Densky = Densky,
                                Hokank = 0,
                                Niekik = NiekiR,
                                Niekyj = NiekiRK,
                                Pccodn = pcnie,
                                Dataym = yymm,
                                Crtcod = loginUser,
                                Crtymd = DateTime.Now,
                                Updcod = loginUser,
                                Updymd = DateTime.Now
                            };
                            dbContext.THokanSeikyuKyoten.Add(hokanSeikyuKyoten);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            THokanSeikyuKyoten upKyoten = kyoten.First();

                            upKyoten.Densuu = Densuu;
                            upKyoten.Densky = Densky;
                            upKyoten.Niekik = NiekiR;
                            upKyoten.Niekyj = NiekiRK;
                            upKyoten.Pccodn = pcnie;
                            upKyoten.Updcod = loginUser;
                            upKyoten.Updymd = DateTime.Now;
                            await dbContext.SaveChangesAsync();
                        }
                    }
                    basyo = denpyokensu.BASYO;
                    kisyua = denpyokensu.KISYUA;
                    kisyub = denpyokensu.KISYUB;
                    seikyu = denpyokensu.SEIKYU;
                    pcnie = denpyokensu.PCNIE;
                    NiekiR = 0;
                    NiekiRK = 0;
                    Densuu = 0;
                    Densky = 0;
                }

                
                // 荷役料（平日）
                if (denpyokensu.NIEFLG3 == "D" && denpyokensu.NIEANT > 0 && denpyokensu.DENSUU > 0 && denpyokensu.KYUJIT != "Y")
                {
                    NiekiR += Math.Round((decimal)denpyokensu.DENSUU * (decimal)denpyokensu.NIEANT);
                    Densuu += (decimal)denpyokensu.DENSUU;
                }

                // 値引
                if (denpyokensu.NNEBIR > 0 && NiekiR > 0)
                {
                    decimal Nebiki = Math.Round(NiekiR * (decimal)denpyokensu.NNEBIR);
                    NiekiR = NiekiR - Nebiki;
                }

                // 荷役料（休日）
                if (denpyokensu.NIEFLG3 == "D" && denpyokensu.NIEKYT > 0 && denpyokensu.DENSUU > 0 && denpyokensu.KYUJIT == "Y")
                {
                    NiekiRK += Math.Round((decimal)denpyokensu.DENSUU * (decimal)denpyokensu.NIEKYT);
                    Densky += (decimal)denpyokensu.DENSUU;
                }

                // 値引
                if (denpyokensu.NNEBIR > 0 && NiekiRK > 0)
                {
                    decimal Nebiki = Math.Round(NiekiRK * (decimal)denpyokensu.NNEBIR);
                    NiekiRK = NiekiRK - Nebiki;
                }


                if (idx == last) {

                    string sybnam = string.Empty;
                    MShukkabasho[] mBasho = await dbContext.MShukkabasho.Where(x => x.Sybcod == basyo).ToArrayAsync();
                    if (mBasho.Count() != 0)
                    {
                        MShukkabasho basho = mBasho.First();
                        sybnam = basho.Sybnam;
                    }

                    string kisyubw = kisyub;
                    if (kisyub.Trim() == string.Empty)
                    {
                        kisyubw = NOT_KISYUB;
                    }

                    if (NiekiR != 0 || NiekiRK != 0 || Densuu != 0 || Densky != 0)
                    {
                        List<THokanSeikyuKyoten> kyoten = await dbContext.THokanSeikyuKyoten.Where(x => x.Basyo == basyo && x.Kisyua == kisyua && x.Kisyub == kisyubw).ToListAsync();
                        if (kyoten.Count() == 0)
                        {
                            THokanSeikyuKyoten hokanSeikyuKyoten = new THokanSeikyuKyoten()
                            {
                                Basyo = basyo,
                                Basnam = sybnam,
                                Kisyua = kisyua,
                                Kisyub = kisyub.Trim() == string.Empty ? NOT_KISYUB : kisyub,
                                Kisybn = kisyub,
                                Seicod = seikyu,
                                Zansuu = 0,
                                Zankin = 0,
                                Siksuu = 0,
                                Sikkin = 0,
                                Kaisuu = 0,
                                Kaikin = 0,
                                Nyuksu = 0,
                                Nyukin = 0,
                                Syksuu = 0,
                                Sykkin = 0,
                                Zaiksu = 0,
                                Zaikin = 0,
                                Densuu = Densuu,
                                Densky = Densky,
                                Hokank = 0,
                                Niekik = NiekiR,
                                Niekyj = NiekiRK,
                                Pccodn = pcnie,
                                Dataym = yymm,
                                Crtcod = loginUser,
                                Crtymd = DateTime.Now,
                                Updcod = loginUser,
                                Updymd = DateTime.Now
                            };
                            dbContext.THokanSeikyuKyoten.Add(hokanSeikyuKyoten);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            THokanSeikyuKyoten upKyoten = kyoten.First();

                            upKyoten.Densuu = Densuu;
                            upKyoten.Densky = Densky;
                            upKyoten.Niekik = NiekiR;
                            upKyoten.Niekyj = NiekiRK;
                            upKyoten.Pccodn = pcnie;
                            upKyoten.Updcod = loginUser;
                            upKyoten.Updymd = DateTime.Now;
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }

            }
            return true;

        }

        // 拠点別データ 金額調整
        private async Task<bool> SetSeikyuKyotenbetuCheck(string loginUser)
        {

            var seikyuList1 = await dbContext.THokanSeikyu
                                        .GroupBy(x => new
                                        {
                                            x.Kisyua,                           // 機種A
                                            x.Kisyub,                           // 機種B
                                            x.Seicod,                           // 請求先コード
                                            x.Pccod                             // PCコード
                                        })
                                        .Select(x => new
                                        {
                                            Kisyua = x.Key.Kisyua,              // 機種A
                                            Kisyub = x.Key.Kisyub,              // 機種B
                                            Seicod = x.Key.Seicod,              // 請求先コード
                                            Pccod = x.Key.Pccod,                // PCコード
                                            Hokankr = x.Sum(y => y.Hokankr),    // 値引後保管料
                                            Niekikr = x.Sum(y => y.Niekikr)     // 値引後荷役料
                                        })
                                        .Where(x => x.Kisyub != "999")
                                        .OrderBy(x => x.Kisyua)
                                        .ToListAsync();

            var seikyuList2 = seikyuList1
                                .GroupBy(x => new
                                {
                                    x.Kisyua,                           // 機種A
                                    x.Seicod,                           // 請求先コード
                                    x.Pccod                             // PCコード
                                })
                                .Select(x => new
                                {
                                    Kisyua = x.Key.Kisyua,              // 機種A
                                    Seicod = x.Key.Seicod,              // 請求先コード
                                    Pccod = x.Key.Pccod,                // PCコード
                                    Hokankr = x.Sum(y => y.Hokankr),    // 値引後保管料
                                    Niekikr = x.Sum(y => y.Niekikr)     // 値引後荷役料
                                })
                                .OrderBy(x => x.Kisyua);

            foreach (var seikyu in seikyuList2)
            {
                var seikyuKyotenList = await dbContext.THokanSeikyuKyoten
                            .GroupBy(x => new
                            {
                                x.Kisyua,                           // 機種A
                                x.Seicod,                           // 請求先コード
                                x.Pccodh,                           // PCコード 保管
                                x.Pccodn                            // PCコード 荷役
                            })
                            .Select(x => new
                            {
                                Kisyua = x.Key.Kisyua,              // 機種A
                                Seicod = x.Key.Seicod,              // 請求先コード
                                Pccodh = x.Key.Pccodh == null ? string.Empty: x.Key.Pccodh,  // PCコード 保管
                                Pccodn = x.Key.Pccodn == null ? string.Empty: x.Key.Pccodn,  // PCコード 荷役
                                Hokank = x.Sum(y => y.Hokank),    　// 値引後保管料
                                Niekik = x.Sum(y => y.Niekik),      // 値引後荷役料
                                Niekyj = x.Sum(y => y.Niekyj)       // 荷役料 休日
                            })
                            .Where(x => x.Kisyua == seikyu.Kisyua)
                            .Where(x => x.Seicod == seikyu.Seicod)
                            .Where(x => x.Pccodh == seikyu.Pccod || x.Pccodn == seikyu.Pccod)
                            .OrderBy(x => x.Kisyua)
                            .ToListAsync();

                if (seikyuKyotenList.Count() != 0)
                {
                    var checData = seikyuKyotenList
                            .GroupBy(x => new { x.Kisyua, x.Pccodh, x.Pccodn })
                            .Select(x => new
                            {
                                Kisyua = x.Key.Kisyua,
                                Pccodh = x.Key.Pccodh,
                                Pccodn = x.Key.Pccodn,
                                Hokank = x.Sum(y => y.Hokank),
                                Niekik = x.Sum(y => y.Niekik),
                                Niekyj = x.Sum(y => y.Niekyj)
                            }).FirstOrDefault();

                    if ((checData.Hokank != seikyu.Hokankr) && seikyu.Pccod == checData.Pccodh)
                    {
                        decimal? check = seikyu.Hokankr - checData.Hokank;
                        var kyotenList = await dbContext.THokanSeikyuKyoten
                                                        .Where(x => x.Kisyua == seikyu.Kisyua)
                                .Where(x => x.Seicod == seikyu.Seicod)
                                .Where(x => x.Pccodh == seikyu.Pccod)
                                .OrderBy(x => x.Kisyua)
                                .ToListAsync();
                        if (kyotenList.Count() != 0)
                        {
                            var kyoten = kyotenList.First();
                            kyoten.Hokank = kyoten.Hokank + check;
                            await dbContext.SaveChangesAsync();
                        }

                    }

                    if (((checData.Niekik + checData.Niekyj) != seikyu.Niekikr) && seikyu.Pccod == checData.Pccodn)
                    {

                        decimal? check = seikyu.Niekikr - (checData.Niekik + checData.Niekyj);
                        var kyotenList = await dbContext.THokanSeikyuKyoten
                                .Where(x => x.Kisyua == seikyu.Kisyua)
                                .Where(x => x.Seicod == seikyu.Seicod)
                                .Where(x => x.Pccodn == seikyu.Pccod)
                                .OrderByDescending(x => x.Niekik)
                                .ToListAsync();

                        if (kyotenList.Count() != 0)
                        {
                            var kyoten = kyotenList.First();
                            kyoten.Niekik = kyoten.Niekik + check;
                            await dbContext.SaveChangesAsync();
                        }

                    }
                }
            }
            return true;

        }


        private static void CreateDeleteSQLN(ref StringBuilder sql)
        {
            sql.Append(" DELETE FROM w_hokan_nyuushuuko; ");
        }

        private static void CreateDeleteSQLD(ref StringBuilder sql)
        {
            sql.Append(" DELETE FROM w_hokan_denpyokensu; ");
        }


        private static void CreateDeleteSeikyu(ref StringBuilder sql)
        {
            sql.Append(" DELETE FROM t_hokan_seikyu; ");
            sql.Append(" DELETE FROM t_hokan_seikyu_b; ");
            sql.Append(" DELETE FROM t_hokan_seikyu_kyoten; ");
        }

        private static void CreateInsertSQLN(ref StringBuilder sql, WHokanNyuushuuko nyuushuuko)
        {

            sql.Append(" INSERT INTO w_hokan_nyuushuuko (");
            sql.Append(" ID, ");
            sql.Append(" KISYUA,");
            sql.Append(" KISYUB,");
            sql.Append(" HOKCOD,");
            sql.Append(" HINCOD,");
            sql.Append(" ZANSUU,");
            sql.Append(" ZANKIN,");
            sql.Append(" SIKSUU,");
            sql.Append(" SIKKIN,");
            sql.Append(" KAISUU,");
            sql.Append(" KAIKIN,");
            sql.Append(" SYKSUU,");
            sql.Append(" SYKKIN,");
            sql.Append(" DKBN,");
            sql.Append(" SEIKYU,");
            sql.Append(" BAITAI,");
            sql.Append(" TOUZAN,");
            sql.Append(" DTMOTO,");
            sql.Append(" CRTCOD,");
            sql.Append(" CRTYMD,");
            sql.Append(" UPDCOD,");
            sql.Append(" UPDYMD");
            sql.Append(") VALUES (");
            sql.Append("N'" + nyuushuuko.Id + "',");
            sql.Append("N'" + nyuushuuko.Kisyua + "',");
            sql.Append("N'" + nyuushuuko.Kisyub + "',");
            sql.Append("N'" + nyuushuuko.Hokcod + "',");
            sql.Append("N'" + nyuushuuko.Hincod + "',");
            sql.Append("N'" + nyuushuuko.Zansuu + "',");
            sql.Append("N'" + nyuushuuko.Zankin + "',");
            sql.Append("N'" + nyuushuuko.Siksuu + "',");
            sql.Append("N'" + nyuushuuko.Sikkin + "',");
            sql.Append("N'" + nyuushuuko.Kaisuu + "',");
            sql.Append("N'" + nyuushuuko.Kaikin + "',");
            sql.Append("N'" + nyuushuuko.Syksuu + "',");
            sql.Append("N'" + nyuushuuko.Sykkin + "',");
            sql.Append("N'" + nyuushuuko.Dkbn + "',");
            sql.Append("N'" + nyuushuuko.Seikyu + "',");
            sql.Append("N'" + nyuushuuko.Baitai + "',");
            sql.Append("N'" + nyuushuuko.Touzan + "',");
            sql.Append("N'" + nyuushuuko.Dtmoto + "',");
            sql.Append("N'" + nyuushuuko.Crtcod + "',");
            sql.Append("N'" + nyuushuuko.Crtymd + "',");
            sql.Append("N'" + nyuushuuko.Updcod + "',");
            sql.Append("N'" + nyuushuuko.Updymd + "'");
            sql.AppendLine(");");

        }

        private static void CreateInsertSQLD(ref StringBuilder sql, WHokanDenpyokensu denpyokensu)
        {

            sql.Append(" INSERT INTO w_hokan_denpyokensu (");
            sql.Append(" ID,");
            sql.Append(" KISYUA,");
            sql.Append(" KISYUB,");
            sql.Append(" BASYO, ");    
            sql.Append(" DENKUB,");
            sql.Append(" DENSUU,");
            sql.Append(" SEIKYU,");
            sql.Append(" INPYMD,");
            sql.Append(" KEIYMD,");
            sql.Append(" HINCOD,");
            sql.Append(" NSKOSU,");
            sql.Append(" EIGSOK,");
            sql.Append(" DTMOTO,");
            sql.Append(" CRTCOD,");
            sql.Append(" CRTYMD,");
            sql.Append(" UPDCOD,");
            sql.Append(" UPDYMD");
            sql.Append(") VALUES (");
            sql.Append("N'" + denpyokensu.Id + "',");
            sql.Append("N'" + denpyokensu.Kisyua + "',");
            sql.Append("N'" + denpyokensu.Kisyub + "',");
            sql.Append("N'" + denpyokensu.Basyo + "',");
            sql.Append("N'" + denpyokensu.Denkub + "',");
            sql.Append("N'" + denpyokensu.Densuu + "',");
            sql.Append("N'" + denpyokensu.Seikyu + "',");
            sql.Append("N'" + denpyokensu.Inpymd + "',");
            sql.Append("N'" + denpyokensu.Keiymd + "',");
            sql.Append("N'" + denpyokensu.Hincod + "',");
            sql.Append("N'" + denpyokensu.Nskosu + "',");
            sql.Append("N'" + denpyokensu.Eigsok + "',");
            sql.Append("N'" + denpyokensu.Dtmoto + "',");
            sql.Append("N'" + denpyokensu.Crtcod + "',");
            sql.Append("N'" + denpyokensu.Crtymd + "',");
            sql.Append("N'" + denpyokensu.Updcod + "',");
            sql.Append("N'" + denpyokensu.Updymd + "'");
            sql.AppendLine(");");

        }

        private void UpdateSQL()
        {

            StringBuilder sql = new StringBuilder();
            sql.Append(" UPDATE t_hokan_seikyu SET KISYUB = ''");
            sql.Append(" WHERE KISYUB = 'XXX'");

            dbContext.Database.ExecuteSqlCommand(sql.ToString());
            dbContext.SaveChanges();

            sql = new StringBuilder();
            sql.Append(" UPDATE t_hokan_seikyu SET HOKCOD = ''");
            sql.Append(" WHERE HOKCOD = 'XX'");

            dbContext.Database.ExecuteSqlCommand(sql.ToString());
            dbContext.SaveChanges();

            sql = new StringBuilder();
            sql.Append(" UPDATE t_hokan_seikyu_b SET KISYUB = ''");
            sql.Append(" WHERE KISYUB = 'XXX'");

            dbContext.Database.ExecuteSqlCommand(sql.ToString());
            dbContext.SaveChanges();

            sql = new StringBuilder();
            sql.Append(" UPDATE t_hokan_seikyu_b SET HOKCOD = ''");
            sql.Append(" WHERE HOKCOD = 'XX'");

            dbContext.Database.ExecuteSqlCommand(sql.ToString());
            dbContext.SaveChanges();

            sql = new StringBuilder();
            sql.Append(" UPDATE t_hokan_seikyu_kyoten SET KISYUB = ''");
            sql.Append(" WHERE KISYUB = 'XXX'");

            dbContext.Database.ExecuteSqlCommand(sql.ToString());
            dbContext.SaveChanges();

            sql = new StringBuilder();
            sql.Append(" UPDATE t_hokan_seikyu_kyoten SET KISYBN = ''");
            sql.Append(" WHERE KISYBN = 'XXX'");

            dbContext.Database.ExecuteSqlCommand(sql.ToString());
            dbContext.SaveChanges();

            //sql = new StringBuilder();
            //sql.Append(" UPDATE t_hokan_nyuushuuko SET KISYUB = ''");
            //sql.Append(" WHERE KISYUB = 'XXX'");

            //dbContext.Database.ExecuteSqlCommand(sql.ToString());
            //dbContext.SaveChanges();

            //sql = new StringBuilder();
            //sql.Append(" UPDATE t_hokan_denpyokensu SET KISYUB = ''");
            //sql.Append(" WHERE KISYUB = 'XXX'");

            //dbContext.Database.ExecuteSqlCommand(sql.ToString());
            //dbContext.SaveChanges();

            //sql = new StringBuilder();
            //sql.Append(" UPDATE t_hokan_nyuushuuko_kurikosi SET KISYUB = ''");
            //sql.Append(" WHERE KISYUB = 'XXX'");

            //dbContext.Database.ExecuteSqlCommand(sql.ToString());
            //dbContext.SaveChanges();

            //sql = new StringBuilder();
            //sql.Append(" UPDATE t_hokan_denpyokensu_kurikosi SET KISYUB = ''");
            //sql.Append(" WHERE KISYUB = 'XXX'");

            //dbContext.Database.ExecuteSqlCommand(sql.ToString());
            //dbContext.SaveChanges();

        }

    }
}