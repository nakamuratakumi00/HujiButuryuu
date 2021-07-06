using Macss.Areas.Tass.Common;
using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Macss.Areas.Tass.Repositories
{
    public class HinmeiRepositorie : IHinmeiRepositorie
    {
        private readonly ApplicationDB dbContext;

        public HinmeiRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
        }

        // 一覧 表示
        public async Task<IEnumerable<HinmeiInformation>> GetHinmeisAsync(HinmeiSerch hinmei)
        {

            string cName = null;
            string uName = null;
            DateTime dtCFrom = DateTime.MinValue;
            DateTime dtCTo = DateTime.MaxValue;
            DateTime dtUFrom = DateTime.MinValue;
            DateTime dtUTo = DateTime.MaxValue;

            if (hinmei.CuCodCh == "1")
            {
                cName = hinmei.CuName;
            }
            else
            {
                uName = hinmei.CuName;
            }
            string CuFromDt = hinmei.CuFrom + " " + DataUtil.GetTimeString(hinmei.CuTFrom, true);
            string CuToDt = hinmei.CuTo + " " + DataUtil.GetTimeString(hinmei.CuTTo, false);

            try
            {
                if (hinmei.CuDateCh == "1")
                {
                    // 登録日
                    dtCFrom = hinmei.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtCTo = hinmei.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
                else
                {
                    // 更新日
                    dtUFrom = hinmei.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtUTo = hinmei.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
            }
            catch
            {
                return new List<HinmeiInformation>();
            }

            string Del0 = null;
            string Del1 = null;
            if (hinmei.Del)
            {
                Del1 = "1";
            }
            else
            {
                Del0 = "1";
            }

            //VUnsouHinmei vUnsouHinmei = new VUnsouHinmei();
            List<HinmeiInformation> hinmeiList = await dbContext.VUnsouHinmei
                        .GroupJoin(dbContext.MAccount, x => x.CRTCOD, x => x.Id, (h, ac) => new {h, ac})
                        .SelectMany(x => x.ac.DefaultIfEmpty(), (h, ac) => new { h = h.h, ac })
                        .GroupJoin(dbContext.MAccount, x => x.h.UPDCOD, x => x.Id, (h, au) => new { h = h.h, ac = h.ac, au })
                        .SelectMany(x => x.au.DefaultIfEmpty(), (h, au) => new { h = h.h, ac = h.ac, au })
                        .Where(x => hinmei.Hinnmk == null ? true : x.h.HINNMK.Contains(hinmei.Hinnmk))
                        .Where(x => hinmei.Hincod == null ? true : x.h.HINCOD.StartsWith(hinmei.Hincod))
                        //.Where(x => hinmei.Khincd == null ? true : x.h.KHINCD.Contains(hinmei.Khincd))
                        .Where(x => hinmei.Khincd == null ? true : x.h.KHINCD.StartsWith(hinmei.Khincd))
                        .Where(x => hinmei.Hinnam == null ? true : x.h.HINNAM.Contains(hinmei.Hinnam))
                        .Where(x => hinmei.Ctlfl1 == null ? true : x.h.CTLFL1 == hinmei.Ctlfl1)
                        .Where(x => hinmei.Dtmoto == null ? true : x.h.DTMOTO == hinmei.Dtmoto)
                        .Where(x => (cName == null ? true : x.ac.UserName.StartsWith(cName)))
                        .Where(x => (uName == null ? true : x.au.UserName.StartsWith(uName)))
                        .Where(x => x.h.CRTYMD >= dtCFrom)
                        .Where(x => x.h.CRTYMD <= dtCTo)
                        .Where(x => x.h.UPDYMD >= dtUFrom)
                        .Where(x => x.h.UPDYMD <= dtUTo)
                        .Where(x => (Del0 == null ? true : x.h.DELFLG != Del0))
                        .Where(x => (Del1 == null ? true : x.h.DELFLG == Del1))
                        .Select(x => new HinmeiInformation()
                        {
                            Ctlfl1 = x.h.CTLFL1,
                            Hincod = x.h.HINCOD,
                            Khincd = x.h.KHINCD,
                            Hinnam = x.h.HINNAM,
                            Hinnmk = x.h.HINNMK,
                            Dtmoto = x.h.DTMOTO,
                            Del = x.h.DELFLG
                        })
                        .OrderBy(x => x.Hinnmk)
                        .ThenBy(x => x.Hinnam)
                        .ToListAsync();

            foreach (HinmeiInformation wHinmei in hinmeiList)
            {
                wHinmei.Hinnam = wHinmei.Hinnam.Trim();
                wHinmei.Hinnmk = HttpUtility.HtmlEncode(wHinmei.Hinnmk.Trim());
            }

            return hinmeiList;
        }

        // 登録 表示
        public async Task<IEnumerable<HinmeiViewModel>> GetHinmeiAsync(string hinmeiCd)
        {
            VUnsouHinmei vUnsouHinmei = new VUnsouHinmei();

            var hinmeiList = await vUnsouHinmei.v_unsou_hinmei
                    .Where(x => x.HINCOD == hinmeiCd)
                    .Select(x => new HinmeiViewModel()
                    {
                        Hincod = x.HINCOD,
                        Khincd = x.KHINCD,
                        Hinnam = x.HINNAM,
                        Hinnmk = x.HINNMK,
                        Torcod = x.TORCOD,
                        Kisyua = x.KISYUA,
                        Kisyub = x.KISYUB,
                        Tkcod = x.TKCOD,
                        Syubtu = x.SYUBTU,
                        Ctlfl1 = x.CTLFL1 == string.Empty ? " ": x.CTLFL1,
                        Dtmoto = x.DTMOTO,
                        Delfkg = x.DELFLG,
                        CrtymdDt = (DateTime)x.CRTYMD,
                        UpdymdDt = (DateTime)x.UPDYMD
                    })
                    .ToListAsync();

            return hinmeiList.Select(x => new HinmeiViewModel
                    {
                        Hincod = x.Hincod,
                        Khincd = x.Khincd,
                        Hinnam = x.Hinnam,
                        Hinnmk = x.Hinnmk,
                        Torcod = x.Torcod,
                        Kisyua = x.Kisyua,
                        Kisyub = x.Kisyub,
                        Tkcod = x.Tkcod,
                        Syubtu = x.Syubtu,
                        Ctlfl1 = x.Ctlfl1,
                        Dtmoto = x.Dtmoto,
                        Delfkg = x.Delfkg,
                        Crtymd = x.CrtymdDt.ToString("yyyy/MM/dd HH:mm:ss"),
                        Updymd = x.UpdymdDt.ToString("yyyy/MM/dd HH:mm:ss")

                    }).ToList();
        }

        // 採番
        public decimal GetHincod()
        {
            var seq_name = "seq_HN_no";
            string seq_no = "";
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString;
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = SelectSequenceSQL(seq_name);
                var reader = command.ExecuteReader();
                var umu_flg = reader.Read();
                reader.Close();
                command.Dispose();

                if (!umu_flg)
                {
                    command.CommandText = CreateSequenceSQL(seq_name);
                    command.ExecuteNonQuery();

                    command.Dispose();
                }

                command.CommandText = GetSequenceSQL(seq_name);
                seq_no = command.ExecuteScalar().ToString();
            }

            return decimal.Parse(seq_no);
        }
        private string SelectSequenceSQL(string seq_name)
        {
            var query = @" SELECT name 
                               FROM sys.sequences 
                              WHERE name = '" + seq_name + @"' ";

            return query;
        }
        private string CreateSequenceSQL(string seq_name)
        {
            var query = @" CREATE SEQUENCE " + seq_name + @" 
                                  START WITH 1 
                                  INCREMENT BY 1 ";

            return query;
        }
        private string GetSequenceSQL(string seq_name)
        {
            var query = @" SELECT NEXT VALUE FOR " + seq_name + @" AS seq_no ";

            return query;
        }

        // 取引先マスタ取得
        public async Task<IEnumerable<MTorihikisaki>> GetMTorihikisakiAsync()
        {
            return await dbContext.MTorihikisaki.OrderBy(x => x.Torcod).ToListAsync();
        }

        // Fe機種マスタ取得
        public async Task<IEnumerable<MKishu>> GetMKishuAsync()
        {
            return await dbContext.MKishu.OrderBy(x => x.Kisyua).ToListAsync();
        }

        // 新規登録
        public async Task InsertHinmeiAsync(HinmeiViewModel hinmei, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    MUnsouHinmeiKoyuu insert = new MUnsouHinmeiKoyuu()
                    {
                        Hincod = hinmei.Hincod,
                        Hinnam = hinmei.Hinnam,
                        Hinnmk = hinmei.Hinnmk,
                        Torcod = hinmei.Torcod,
                        Kisyua = hinmei.Kisyua,
                        Kisyub = hinmei.Kisyub,
                        Dtmoto = hinmei.Dtmoto,
                        Tkcod = hinmei.Tkcod,
                        Syubtu = hinmei.Syubtu,
                        Ctlfl1 = hinmei.Ctlfl1,
                        Khincd = hinmei.Khincd,
                        Delflg = "0",
                        Crtcod = loginUser,
                        Crtymd = DateTime.Now,
                        Updcod = loginUser,
                        Updymd = DateTime.Now
                    };
                    dbContext.MUnsouHinmeiKoyuu.Add(insert);

                    await dbContext.SaveChangesAsyncEx();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        // 更新
        public async Task UpdateHinmeiAsync(HinmeiViewModel hinmei, string loginUser)
        {
            hinmei.Ctlfl1 = hinmei.Ctlfl1.Trim();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var mhinmei = (await dbContext.MUnsouHinmeiKoyuu.Where(x => x.Hincod == hinmei.Hincod).ToListAsync()).FirstOrDefault();

                    mhinmei.Hinnam = hinmei.Hinnam;
                    mhinmei.Hinnmk = hinmei.Hinnmk;
                    mhinmei.Torcod = hinmei.Torcod;
                    mhinmei.Kisyua = hinmei.Kisyua;
                    mhinmei.Kisyub = hinmei.Kisyub;
                    mhinmei.Dtmoto = hinmei.Dtmoto;
                    mhinmei.Tkcod = hinmei.Tkcod;
                    mhinmei.Syubtu = hinmei.Syubtu;
                    mhinmei.Ctlfl1 = hinmei.Ctlfl1;
                    mhinmei.Khincd = hinmei.Khincd;
                    mhinmei.Updcod = loginUser;
                    mhinmei.Updymd = DateTime.Now;

                    await dbContext.SaveChangesAsyncEx();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        // 削除
        public async Task DeleteHinmeiAsync(HinmeiViewModel hinmei, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var mhinmei = await dbContext.MUnsouHinmeiKoyuu.Where(x => x.Hincod == hinmei.Hincod).SingleOrDefaultAsync();

                    mhinmei.Delflg = "1";
                    mhinmei.Updcod = loginUser;
                    mhinmei.Updymd = DateTime.Now;

                    //dbContext.MUnsouHinmeiKoyuu.Remove(mhinmei);

                    await dbContext.SaveChangesAsyncEx();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        // Excel出力
        public async Task<IEnumerable<v_unsou_hinmei>> GetHinmeiExcelAsync(string hinCod)
        {
            VUnsouHinmei vUnsouHinmei = new VUnsouHinmei();

            var hinmeiiList = await vUnsouHinmei.v_unsou_hinmei
                        .Where(x => x.HINCOD == hinCod)
                        .ToListAsync();

            return hinmeiiList;
        }


        public async Task<IEnumerable<HinmeiFileInformation>> GetHinmeiExcelAsync(HinmeiSerch hinmei)
        {
            string cName = null;
            string uName = null;
            DateTime dtCFrom = DateTime.MinValue;
            DateTime dtCTo = DateTime.MaxValue;
            DateTime dtUFrom = DateTime.MinValue;
            DateTime dtUTo = DateTime.MaxValue;

            if (hinmei.CuCodCh == "1")
            {
                cName = hinmei.CuName;
            }
            else
            {
                uName = hinmei.CuName;
            }
            string CuFromDt = hinmei.CuFrom + " " + DataUtil.GetTimeString(hinmei.CuTFrom, true);
            string CuToDt = hinmei.CuTo + " " + DataUtil.GetTimeString(hinmei.CuTTo, false);

            try
            {
                if (hinmei.CuDateCh == "1")
                {
                    // 登録日
                    dtCFrom = hinmei.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtCTo = hinmei.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
                else
                {
                    // 更新日
                    dtUFrom = hinmei.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtUTo = hinmei.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
            }
            catch
            {
                return new List<HinmeiFileInformation>();
            }

            string Del0 = null;
            string Del1 = null;
            if (hinmei.Del)
            {
                Del1 = "1";
            }
            else
            {
                Del0 = "1";
            }

            //VUnsouHinmei vUnsouHinmei = new VUnsouHinmei();
            var hinmeiiList = await dbContext.VUnsouHinmei
                        .GroupJoin(dbContext.MAccount, x => x.CRTCOD, x => x.Id, (h, ac) => new { h, ac })
                        .SelectMany(x => x.ac.DefaultIfEmpty(), (h, ac) => new { h = h.h, ac })
                        .GroupJoin(dbContext.MAccount, x => x.h.UPDCOD, x => x.Id, (h, au) => new { h = h.h, ac = h.ac, au })
                        .SelectMany(x => x.au.DefaultIfEmpty(), (h, au) => new { h = h.h, ac = h.ac, au })
                        .Where(x => hinmei.Hinnmk == null ? true : x.h.HINNMK.Contains(hinmei.Hinnmk))
                        .Where(x => hinmei.Hincod == null ? true : x.h.HINCOD.StartsWith(hinmei.Hincod))
                        //.Where(x => hinmei.Khincd == null ? true : x.h.KHINCD.Contains(hinmei.Khincd))
                        .Where(x => hinmei.Khincd == null ? true : x.h.KHINCD.StartsWith(hinmei.Khincd))
                        .Where(x => hinmei.Hinnam == null ? true : x.h.HINNAM.Contains(hinmei.Hinnam))
                        .Where(x => hinmei.Ctlfl1 == null ? true : x.h.CTLFL1 == hinmei.Ctlfl1)
                        .Where(x => hinmei.Dtmoto == null ? true : x.h.DTMOTO == hinmei.Dtmoto)
                        .Where(x => (cName == null ? true : x.ac.UserName.StartsWith(cName)))
                        .Where(x => (uName == null ? true : x.au.UserName.StartsWith(uName)))
                        .Where(x => x.h.CRTYMD >= dtCFrom)
                        .Where(x => x.h.CRTYMD <= dtCTo)
                        .Where(x => x.h.UPDYMD >= dtUFrom)
                        .Where(x => x.h.UPDYMD <= dtUTo)
                        .Where(x => (Del0 == null ? true : x.h.DELFLG != Del0))
                        .Where(x => (Del1 == null ? true : x.h.DELFLG == Del1))
                        .Select(x => new HinmeiFileInformation()
                        {
                            HINCOD = x.h.HINCOD,
                            HINNAM = x.h.HINNAM,
                            HINNMK = x.h.HINNMK,
                            TORCOD = x.h.TORCOD,
                            KISYUA = x.h.KISYUA,
                            KISYUB = x.h.KISYUB,
                            DTMOTO = x.h.DTMOTO,
                            TKCOD = x.h.TKCOD,
                            SYUBTU = x.h.SYUBTU,
                            CTLFL1 = x.h.CTLFL1,
                            KHINCD = x.h.KHINCD,
                            CRTCOD = x.h.CRTCOD,
                            DELFLG = x.h.DELFLG,
                            CRTYMD = x.h.CRTYMD,
                            UPDCOD = x.h.UPDCOD,
                            UPDYMD = x.h.UPDYMD
                        })
                        .OrderBy(x => x.HINNMK)
                        .ThenBy(x => x.HINNAM)
                        .ToListAsync();

            return hinmeiiList;
        }
    }
}