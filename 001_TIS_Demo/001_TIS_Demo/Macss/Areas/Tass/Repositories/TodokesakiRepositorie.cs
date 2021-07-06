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
    public class TodokesakiRepositorie : ITodokesakiRepositorie
                                         
    {
        private readonly ApplicationDB dbContext;

        public TodokesakiRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
        }

        // 一覧 表示
        public async Task<IEnumerable<TodokesakiInformation>> GetTodokesakisAsync(TodokesakiSerch todokesaki)
        {

            string cName = null;
            string uName = null;
            DateTime dtCFrom = DateTime.MinValue;
            DateTime dtCTo = DateTime.MaxValue;
            DateTime dtUFrom = DateTime.MinValue;
            DateTime dtUTo = DateTime.MaxValue;

            if (todokesaki.CuCodCh == "1")
            {
                cName = todokesaki.CuName;
            }
            else
            {
                uName = todokesaki.CuName;
            }
            string CuFromDt = todokesaki.CuFrom + " " + DataUtil.GetTimeString(todokesaki.CuTFrom, true);
            string CuToDt = todokesaki.CuTo + " " + DataUtil.GetTimeString(todokesaki.CuTTo, false);

            try
            {
                if (todokesaki.CuDateCh == "1")
                {
                    // 登録日
                    dtCFrom = todokesaki.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtCTo = todokesaki.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
                else
                {
                    // 更新日
                    dtUFrom = todokesaki.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtUTo = todokesaki.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
            } catch
            {
                return new List<TodokesakiInformation>();
            }

            string Del0 = null;
            string Del1 = null;
            if (todokesaki.Del)
            {
                Del1 = "1";
            } else
            {
                Del0 = "1";
            }

            //VUnsouTodokesaki vUnsouTodokesaki = new VUnsouTodokesaki();
            List<TodokesakiInformation> todokesakiList = await dbContext.VUnsouTodokesaki
                        .GroupJoin(dbContext.MAccount, x => x.CRTCOD, x => x.Id, (t, ac) => new { t, ac })
                        .SelectMany(x => x.ac.DefaultIfEmpty(), (t, ac) => new { t = t.t, ac })
                        .GroupJoin(dbContext.MAccount, x => x.t.UPDCOD, x => x.Id, (t, au) => new { t = t.t, ac = t.ac, au })
                        .SelectMany(x => x.au.DefaultIfEmpty(), (t, au) => new { t = t.t, ac = t.ac, au })
                        .Where(x => (todokesaki.Tdkcod == null ? true : x.t.TDKCOD.StartsWith(todokesaki.Tdkcod)))
                        //.Where(x => (todokesaki.Ktdkcd == null ? true : x.t.KTDKCD.Contains(todokesaki.Ktdkcd)))
                        .Where(x => (todokesaki.Ktdkcd == null ? true : x.t.KTDKCD.StartsWith(todokesaki.Ktdkcd)))
                        .Where(x => (todokesaki.Tdknam == null ? true : x.t.TDKNAM.Contains(todokesaki.Tdknam)))
                        .Where(x => (todokesaki.Jyusyo == null ? true : x.t.JYUSYO.Contains(todokesaki.Jyusyo)))
                        .Where(x => (todokesaki.Tdknmk == null ? true : x.t.TDKNMK.Contains(todokesaki.Tdknmk)))
                        .Where(x => (todokesaki.Tdbnam == null ? true : x.t.TDBNAM.Contains(todokesaki.Tdbnam)))
                        .Where(x => (todokesaki.Tdknms == null ? true : x.t.TDKNMS.Contains(todokesaki.Tdknms)))                        
                        .Where(x => (todokesaki.Tdktan == null ? true : x.t.TDKTAN.Contains(todokesaki.Tdktan)))
                        .Where(x => (todokesaki.Sdek01 == null ? true : x.t.SDEK01 == todokesaki.Sdek01))
                        .Where(x => (todokesaki.Sdek02 == null ? true : x.t.SDEK02 == todokesaki.Sdek02))
                        .Where(x => (todokesaki.Sdek03 == null ? true : x.t.SDEK03 == todokesaki.Sdek03))
                        .Where(x => (todokesaki.Sdek04 == null ? true : x.t.SDEK04 == todokesaki.Sdek04))
                        .Where(x => (todokesaki.Sdek05 == null ? true : x.t.SDEK05 == todokesaki.Sdek05))
                        .Where(x => (todokesaki.Sdek06 == null ? true : x.t.SDEK06 == todokesaki.Sdek06))
                        .Where(x => (todokesaki.Sdek07 == null ? true : x.t.SDEK07 == todokesaki.Sdek07))
                        .Where(x => (todokesaki.Sdek08 == null ? true : x.t.SDEK08 == todokesaki.Sdek08))
                        .Where(x => (todokesaki.Sdek09 == null ? true : x.t.SDEK09 == todokesaki.Sdek09))
                        .Where(x => (todokesaki.Sdek10 == null ? true : x.t.SDEK10 == todokesaki.Sdek10))
                        .Where(x => (todokesaki.Sdek11 == null ? true : x.t.SDEK11 == todokesaki.Sdek11))
                        .Where(x => (todokesaki.Sdek12 == null ? true : x.t.SDEK12 == todokesaki.Sdek12))
                        .Where(x => (todokesaki.Sdek13 == null ? true : x.t.SDEK13 == todokesaki.Sdek13))
                        .Where(x => (todokesaki.Sdek14 == null ? true : x.t.SDEK14 == todokesaki.Sdek14))
                        .Where(x => (todokesaki.Sdek15 == null ? true : x.t.SDEK15 == todokesaki.Sdek15))
                        .Where(x => (cName == null ? true : x.ac.UserName.StartsWith(cName)))
                        .Where(x => (uName == null ? true : x.au.UserName.StartsWith(uName)))
                        .Where(x => x.t.CRTYMD >= dtCFrom)
                        .Where(x => x.t.CRTYMD <= dtCTo)
                        .Where(x => x.t.UPDYMD >= dtUFrom)
                        .Where(x => x.t.UPDYMD <= dtUTo)
                        .Where(x => (Del0 == null ? true : x.t.DELFLG != Del0))
                        .Where(x => (Del1 == null ? true : x.t.DELFLG == Del1))
                        .Where(x => (todokesaki.Dtmoto == null ? true : x.t.DTMOTO == todokesaki.Dtmoto))
                        .Select(x => new TodokesakiInformation()
                        {
                            Tdkcod = x.t.TDKCOD,
                            //Ktdkcd = x.KTDKCD == null ? string.Empty : x.KTDKCD.Substring(0, 9),
                            Ktdkcd = x.t.KTDKCD,
                            Tdknam = (x.t.TDKNAM ?? string.Empty),
                            Tdknms = (x.t.TDKNMS ?? string.Empty),
                            Tdbnam = (x.t.TDBNAM ?? string.Empty),
                            Tdktan = (x.t.TDKTAN ?? string.Empty),
                            Jyusyo = x.t.JYUSYO,
                            Tdknmk = x.t.TDKNMK,
                            Dtmoto = x.t.DTMOTO,
                            Del = x.t.DELFLG
                        })
                        //.OrderBy(x => x.Tdknmk)
                        //.ThenBy(x => x.Tdknam)
                        .OrderBy(x => x.Tdknmk)
                        .ThenBy(x => x.Tdknam)
                        .ThenBy(x => x.Tdknms)
                        .ThenBy(x => x.Tdbnam)
                        .ToListAsync();

            foreach (TodokesakiInformation work in todokesakiList)
            {
                work.Tdknam = work.Tdknam.Trim() + " " + work.Tdknms.Trim() + " " + work.Tdbnam.Trim() + " " + work.Tdktan.Trim();
            }

            return todokesakiList;
            //return todokesakiList.OrderBy(x => x.Tdknmk).ThenBy(x => x.Tdknam);

        }

        // 登録 表示
        public async Task<IEnumerable<TodokesakiViewModel>> GetTodokesakiAsync(string tdkCod)
        {
            VUnsouTodokesaki vUnsouTodokesaki = new VUnsouTodokesaki();

            var todokesakiList = await vUnsouTodokesaki.v_unsou_todokesaki
                    .Where(x => x.TDKCOD == tdkCod)
                    .Select(x => new TodokesakiViewModel()
                    {
                        Tdkcod = x.TDKCOD,
                        Tdknam = x.TDKNAM,
                        Tdknms = x.TDKNMS,
                        Tdknmk = x.TDKNMK,
                        Tdbnam = x.TDBNAM,
                        Tdktan = x.TDKTAN,
                        Jyusyo = x.JYUSYO,
                        Tdktel = x.TDKTEL,
                        Sdek01 = x.SDEK01,
                        Sdek02 = x.SDEK02,
                        Sdek03 = x.SDEK03,
                        Sdek04 = x.SDEK04,
                        Sdek05 = x.SDEK05,
                        Sdek06 = x.SDEK06,
                        Sdek07 = x.SDEK07,
                        Sdek08 = x.SDEK08,
                        Sdek09 = x.SDEK09,
                        Sdek10 = x.SDEK10,
                        Sdek11 = x.SDEK11,
                        Sdek12 = x.SDEK12,
                        Sdek13 = x.SDEK13,
                        Sdek14 = x.SDEK14,
                        Sdek15 = x.SDEK15,
                        Tkjiko = x.TKJIKO,
                        Dtmoto = x.DTMOTO,
                        Yubinn = x.YUBINN,
                        Faxno = x.FAXNO,
                        Ktdkcd = x.KTDKCD,
                        Kitdcd = x.KITDCD,
                        Delfkg = x.DELFLG,
                        CrtymdDt = (DateTime)x.CRTYMD,
                        UpdymdDt = (DateTime)x.UPDYMD
                    })
                    .ToListAsync();


            return todokesakiList.Select(x => new TodokesakiViewModel
                    {
                    Tdkcod = x.Tdkcod,
                    Tdknam = x.Tdknam,
                    Tdknms = x.Tdknms,
                    Tdknmk = x.Tdknmk,
                    Tdbnam = x.Tdbnam,
                    Tdktan = x.Tdktan,
                    Jyusyo = x.Jyusyo,
                    Tdktel = x.Tdktel,
                    Sdek01 = x.Sdek01,
                    Sdek02 = x.Sdek02,
                    Sdek03 = x.Sdek03,
                    Sdek04 = x.Sdek04,
                    Sdek05 = x.Sdek05,
                    Sdek06 = x.Sdek06,
                    Sdek07 = x.Sdek07,
                    Sdek08 = x.Sdek08,
                    Sdek09 = x.Sdek09,
                    Sdek10 = x.Sdek10,
                    Sdek11 = x.Sdek11,
                    Sdek12 = x.Sdek12,
                    Sdek13 = x.Sdek13,
                    Sdek14 = x.Sdek14,
                    Sdek15 = x.Sdek15,
                    Tkjiko = x.Tkjiko,
                    Dtmoto = x.Dtmoto,
                    Yubinn = x.Yubinn,
                    Faxno = x.Faxno,
                    Ktdkcd = x.Ktdkcd,
                    Kitdcd = x.Kitdcd,
                    Delfkg = x.Delfkg,
                    Crtymd = x.CrtymdDt.ToString("yyyy/MM/dd HH:mm:ss"),
                    Updymd = x.UpdymdDt.ToString("yyyy/MM/dd HH:mm:ss")
            }).ToList();

        }

        // 採番
        public decimal GetTdkcod()
        {
            var seq_name = "seq_TD_no";
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

        // 郵便番号マスタ取得
        public async Task<IEnumerable<MUnsouPostalcode>> GetMUnsouPostalcodeAsync()
        {
            return await dbContext.MUnsouPostalcode.OrderBy(x => x.Jyusy1).ThenBy(x => x.Jyusy2).ToListAsync();
        }

        // 新規登録
        public async Task InsertTodokesakiAsync(TodokesakiViewModel todokesaki, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    MUnsouTodokesakiKoyuu insert = new MUnsouTodokesakiKoyuu()
                    {
                        Tdkcod = todokesaki.Tdkcod,
                        Tdknam = todokesaki.Tdknam,
                        Tdknms = todokesaki.Tdknms,
                        Tdknmk = todokesaki.Tdknmk,
                        Tdbnam = todokesaki.Tdbnam,
                        Tdktan = todokesaki.Tdktan,
                        Jyusyo = todokesaki.Jyusyo,
                        Tdktel = todokesaki.Tdktel,
                        Sdek01 = todokesaki.Sdek01,
                        Sdek02 = todokesaki.Sdek02,
                        Sdek03 = todokesaki.Sdek03,
                        Sdek04 = todokesaki.Sdek04,
                        Sdek05 = todokesaki.Sdek05,
                        Sdek06 = todokesaki.Sdek06,
                        Sdek07 = todokesaki.Sdek07,
                        Sdek08 = todokesaki.Sdek08,
                        Sdek09 = todokesaki.Sdek09,
                        Sdek10 = todokesaki.Sdek10,
                        Sdek11 = todokesaki.Sdek11,
                        Sdek12 = todokesaki.Sdek12,
                        Sdek13 = todokesaki.Sdek13,
                        Sdek14 = todokesaki.Sdek14,
                        Sdek15 = todokesaki.Sdek15,
                        Tkjiko = todokesaki.Tkjiko,
                        Dtmoto = todokesaki.Dtmoto,
                        Yubinn = todokesaki.Yubinn,
                        Faxno = todokesaki.Faxno,
                        Ktdkcd = todokesaki.Ktdkcd,
                        Kitdcd = todokesaki.Kitdcd,
                        Delflg = "0",
                        Crtcod = loginUser,
                        Crtymd = DateTime.Now,
                        Updcod = loginUser,
                        Updymd = DateTime.Now
                    };
                    dbContext.MUnsouTodokesakiKoyuu.Add(insert);

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
        public async Task UpdateTodokesakiAsync(TodokesakiViewModel todokesaki, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var mtodokesaki = (await dbContext.MUnsouTodokesakiKoyuu.Where(x => x.Tdkcod == todokesaki.Tdkcod).ToListAsync()).FirstOrDefault();

                    mtodokesaki.Tdkcod = todokesaki.Tdkcod;
                    mtodokesaki.Tdknam = todokesaki.Tdknam;
                    mtodokesaki.Tdknms = todokesaki.Tdknms;
                    mtodokesaki.Tdknmk = todokesaki.Tdknmk;
                    mtodokesaki.Tdbnam = todokesaki.Tdbnam;
                    mtodokesaki.Tdktan = todokesaki.Tdktan;
                    mtodokesaki.Jyusyo = todokesaki.Jyusyo;
                    mtodokesaki.Tdktel = todokesaki.Tdktel;
                    mtodokesaki.Sdek01 = todokesaki.Sdek01;
                    mtodokesaki.Sdek02 = todokesaki.Sdek02;
                    mtodokesaki.Sdek03 = todokesaki.Sdek03;
                    mtodokesaki.Sdek04 = todokesaki.Sdek04;
                    mtodokesaki.Sdek05 = todokesaki.Sdek05;
                    mtodokesaki.Sdek06 = todokesaki.Sdek06;
                    mtodokesaki.Sdek07 = todokesaki.Sdek07;
                    mtodokesaki.Sdek08 = todokesaki.Sdek08;
                    mtodokesaki.Sdek09 = todokesaki.Sdek09;
                    mtodokesaki.Sdek10 = todokesaki.Sdek10;
                    mtodokesaki.Sdek11 = todokesaki.Sdek11;
                    mtodokesaki.Sdek12 = todokesaki.Sdek12;
                    mtodokesaki.Sdek13 = todokesaki.Sdek13;
                    mtodokesaki.Sdek14 = todokesaki.Sdek14;
                    mtodokesaki.Sdek15 = todokesaki.Sdek15;
                    mtodokesaki.Tkjiko = todokesaki.Tkjiko;
                    mtodokesaki.Dtmoto = todokesaki.Dtmoto;
                    mtodokesaki.Yubinn = todokesaki.Yubinn;
                    mtodokesaki.Faxno = todokesaki.Faxno;
                    mtodokesaki.Ktdkcd = todokesaki.Ktdkcd;
                    mtodokesaki.Kitdcd = todokesaki.Kitdcd;
                    mtodokesaki.Updcod = loginUser;
                    mtodokesaki.Updymd = DateTime.Now;

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
        public async Task DeleteTodokesakiAsync(TodokesakiViewModel todokesaki, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var mtodokesaki = await dbContext.MUnsouTodokesakiKoyuu.Where(x => x.Tdkcod == todokesaki.Tdkcod).SingleOrDefaultAsync();

                    mtodokesaki.Delflg = "1";
                    mtodokesaki.Updcod = loginUser;
                    mtodokesaki.Updymd = DateTime.Now;

                    //dbContext.MUnsouTodokesakiKoyuu.Remove(mtodokesaki);

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
        public async Task<IEnumerable<v_unsou_todokesaki>> GetTodokesakiExcelAsync(string tdkCod)
        {
            VUnsouTodokesaki vUnsouTodokesaki = new VUnsouTodokesaki();

            var todokesakiList = await vUnsouTodokesaki.v_unsou_todokesaki
                        .Where(x => x.TDKCOD == tdkCod)
                        .ToListAsync();

            return todokesakiList;
        }

        // 一覧 取得
        public async Task<IEnumerable<TodokesakiFileInformation>> GetTodokesakiExcelAsync(TodokesakiSerch todokesaki)
        {

            string cName = null;
            string uName = null;
            DateTime dtCFrom = DateTime.MinValue;
            DateTime dtCTo = DateTime.MaxValue;
            DateTime dtUFrom = DateTime.MinValue;
            DateTime dtUTo = DateTime.MaxValue;

            if (todokesaki.CuCodCh == "1")
            {
                cName = todokesaki.CuName;
            }
            else
            {
                uName = todokesaki.CuName;
            }

            string CuFromDt = todokesaki.CuFrom + " " + DataUtil.GetTimeString(todokesaki.CuTFrom, true);
            string CuToDt = todokesaki.CuTo + " " + DataUtil.GetTimeString(todokesaki.CuTTo, false);
            try
            {
                if (todokesaki.CuDateCh == "1")
                {
                    // 登録日
                    dtCFrom = todokesaki.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtCTo = todokesaki.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
                else
                {
                    // 更新日
                    dtUFrom = todokesaki.CuFrom == null ? DateTime.MinValue : DateTime.Parse(CuFromDt);
                    dtUTo = todokesaki.CuTo == null ? DateTime.MaxValue : DateTime.Parse(CuToDt);
                }
            }
            catch
            {
                return new List<TodokesakiFileInformation>();
            }

            string Del0 = null;
            string Del1 = null;
            if (todokesaki.Del)
            {
                Del1 = "1";
            }
            else
            {
                Del0 = "1";
            }

            //VUnsouTodokesaki vUnsouTodokesaki = new VUnsouTodokesaki();
            List<TodokesakiFileInformation> todokesakiList = await dbContext.VUnsouTodokesaki
                        .GroupJoin(dbContext.MAccount, x => x.CRTCOD, x => x.Id, (t, ac) => new { t, ac })
                        .SelectMany(x => x.ac.DefaultIfEmpty(), (t, ac) => new { t = t.t, ac })
                        .GroupJoin(dbContext.MAccount, x => x.t.UPDCOD, x => x.Id, (t, au) => new { t = t.t, ac = t.ac, au })
                        .SelectMany(x => x.au.DefaultIfEmpty(), (t, au) => new { t = t.t, ac = t.ac, au })
                        .Where(x => (todokesaki.Tdkcod == null ? true : x.t.TDKCOD.StartsWith(todokesaki.Tdkcod)))
                        //.Where(x => (todokesaki.Ktdkcd == null ? true : x.t.KTDKCD.Contains(todokesaki.Ktdkcd)))
                        .Where(x => (todokesaki.Ktdkcd == null ? true : x.t.KTDKCD.StartsWith(todokesaki.Ktdkcd)))
                        .Where(x => (todokesaki.Tdknam == null ? true : x.t.TDKNAM.Contains(todokesaki.Tdknam)))
                        .Where(x => (todokesaki.Jyusyo == null ? true : x.t.JYUSYO.Contains(todokesaki.Jyusyo)))
                        .Where(x => (todokesaki.Tdknmk == null ? true : x.t.TDKNMK.Contains(todokesaki.Tdknmk)))
                        .Where(x => (todokesaki.Sdek01 == null ? true : x.t.SDEK01 == todokesaki.Sdek01))
                        .Where(x => (todokesaki.Sdek02 == null ? true : x.t.SDEK02 == todokesaki.Sdek02))
                        .Where(x => (todokesaki.Sdek03 == null ? true : x.t.SDEK03 == todokesaki.Sdek03))
                        .Where(x => (todokesaki.Sdek04 == null ? true : x.t.SDEK04 == todokesaki.Sdek04))
                        .Where(x => (todokesaki.Sdek05 == null ? true : x.t.SDEK05 == todokesaki.Sdek05))
                        .Where(x => (todokesaki.Sdek06 == null ? true : x.t.SDEK06 == todokesaki.Sdek06))
                        .Where(x => (todokesaki.Sdek07 == null ? true : x.t.SDEK07 == todokesaki.Sdek07))
                        .Where(x => (todokesaki.Sdek08 == null ? true : x.t.SDEK08 == todokesaki.Sdek08))
                        .Where(x => (todokesaki.Sdek09 == null ? true : x.t.SDEK09 == todokesaki.Sdek09))
                        .Where(x => (todokesaki.Sdek10 == null ? true : x.t.SDEK10 == todokesaki.Sdek10))
                        .Where(x => (todokesaki.Sdek11 == null ? true : x.t.SDEK11 == todokesaki.Sdek11))
                        .Where(x => (todokesaki.Sdek12 == null ? true : x.t.SDEK12 == todokesaki.Sdek12))
                        .Where(x => (todokesaki.Sdek13 == null ? true : x.t.SDEK13 == todokesaki.Sdek13))
                        .Where(x => (todokesaki.Sdek14 == null ? true : x.t.SDEK14 == todokesaki.Sdek14))
                        .Where(x => (todokesaki.Sdek15 == null ? true : x.t.SDEK15 == todokesaki.Sdek15))
                        .Where(x => (todokesaki.Dtmoto == null ? true : x.t.DTMOTO == todokesaki.Dtmoto))
                        .Where(x => (cName == null ? true : x.ac.UserName.StartsWith(cName)))
                        .Where(x => (uName == null ? true : x.au.UserName.StartsWith(uName)))
                        .Where(x => x.t.CRTYMD >= dtCFrom)
                        .Where(x => x.t.CRTYMD <= dtCTo)
                        .Where(x => x.t.UPDYMD >= dtUFrom)
                        .Where(x => x.t.UPDYMD <= dtUTo)
                        .Where(x => (Del0 == null ? true : x.t.DELFLG != Del0))
                        .Where(x => (Del1 == null ? true : x.t.DELFLG == Del1))
                        .Select(x => new TodokesakiFileInformation()
                        {
                            TDKCOD = x.t.TDKCOD,
                            TDKNAM = x.t.TDKNAM,
                            TDKNMS = x.t.TDKNMS,
                            TDKNMK = x.t.TDKNMK,
                            TDBNAM = x.t.TDBNAM,
                            TDKTAN = x.t.TDKTAN,
                            JYUSYO = x.t.JYUSYO,
                            TDKTEL = x.t.TDKTEL,
                            SDEK01 = x.t.SDEK01,
                            SDEK02 = x.t.SDEK02,
                            SDEK03 = x.t.SDEK03,
                            SDEK04 = x.t.SDEK04,
                            SDEK05 = x.t.SDEK05,
                            SDEK06 = x.t.SDEK06,
                            SDEK07 = x.t.SDEK07,
                            SDEK08 = x.t.SDEK08,
                            SDEK09 = x.t.SDEK09,
                            SDEK10 = x.t.SDEK10,
                            SDEK11 = x.t.SDEK11,
                            SDEK12 = x.t.SDEK12,
                            SDEK13 = x.t.SDEK13,
                            SDEK14 = x.t.SDEK14,
                            SDEK15 = x.t.SDEK15,
                            TKJIKO = x.t.TKJIKO,
                            DTMOTO = x.t.DTMOTO,
                            YUBINN = x.t.YUBINN,
                            FAXNO = x.t.FAXNO,
                            KTDKCD = x.t.KTDKCD,
                            KITDCD = x.t.KITDCD,
                            DELFLG = x.t.DELFLG,
                            CRTCOD = x.t.CRTCOD,
                            CRTYMD = x.t.CRTYMD,
                            UPDCOD = x.t.UPDCOD,
                            UPDYMD = x.t.UPDYMD
                        })
                        //社名カナ昇順、社名昇順、支店名昇順、部課名昇順
                        .OrderBy(x => x.TDKNMK)
                        .ThenBy(x => x.TDKNAM)
                        .ThenBy(x => x.TDKNMS)
                        .ThenBy(x => x.TDBNAM)
                        .ToListAsync();

            return todokesakiList;
        }

    }
}