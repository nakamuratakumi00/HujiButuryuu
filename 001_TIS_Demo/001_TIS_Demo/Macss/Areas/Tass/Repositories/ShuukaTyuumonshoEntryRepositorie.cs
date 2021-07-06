using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Macss.Areas.Tass.Repositories
{
    public class ShuukaTyuumonshoEntryRepositorie : IShuukaTyuumonshoEntryRepositorie

    {
        private readonly ApplicationDB dbContext;

        public ShuukaTyuumonshoEntryRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
        }

        // 一覧 表示
        public async Task<IEnumerable<EntryInformation>> GetShuukaTyuumonshosAsync(EntrySerch Search)
        {
            DateTime skymdF = DateTime.MinValue;
            DateTime skymdT = DateTime.MinValue;

            if (Search.SykymdFrom != null)
            {
                skymdF = DateTime.Parse(Search.SykymdFrom);
            }
            if (Search.SykymdTo != null)
            {
                skymdT = DateTime.Parse(Search.SykymdTo);
            }

            var list = await dbContext.TUnsouShuukaTyuumonshoTehai
                        // アカウントマスタ
                        .GroupJoin(dbContext.MAccount, a => a.Crtcod, b => b.Id, (a, b) => new { a, b })
                        .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a.a, b })
                        // 取引先マスタ_得意先
                        .GroupJoin(dbContext.MTorihikisaki, ab => ab.a.Tokcod, c => c.Torcod, (ab, c) => new { ab, c })
                        .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new { ab.ab, c })

                        .Where(x => Search.Syukno == null ? true : x.ab.a.Syukno.StartsWith(Search.Syukno))
                        .Where(x => Search.Sybcod == null ? true : x.ab.a.Sybcod == Search.Sybcod)
                        .Where(x => Search.Fsykno == null ? true : x.ab.a.Fsykno.StartsWith(Search.Fsykno))
                        .Where(x => Search.Crtnam == null ? true : x.ab.b.UserName.StartsWith(Search.Crtnam))
                        .Where(x => Search.SykymdFrom == null ? true : x.ab.a.Sykymd >= skymdF)
                        .Where(x => Search.SykymdTo == null ? true : x.ab.a.Sykymd <= skymdT)
                        .Where(x => Search.Tokcod == null ? true : x.ab.a.Tokcod.StartsWith(Search.Tokcod))
                        .Where(x => Search.Toknam == null ? true : x.c.Tornam.Contains(Search.Toknam))
                        .Where(x => Search.Tdkcod == null ? true : x.ab.a.Tdkcod.StartsWith(Search.Tdkcod))
                        .Where(x => Search.Tdknam == null ? true : x.ab.a.Tdknam.Contains(Search.Tdknam))
                        .Where(x => Search.Tdsnam == null ? true : x.ab.a.Tdsnam.Contains(Search.Tdsnam))
                        .Where(x => Search.Tdbnam == null ? true : x.ab.a.Tdbnam.Contains(Search.Tdbnam))
                        .Where(x => Search.Tdktan == null ? true : x.ab.a.Tdktan.Contains(Search.Tdktan))
                        .Where(x => (Search.Denfn == false & Search.Denfy == true) ? (x.ab.a.Denf == "A" | x.ab.a.Denf == "S") : true) 
                        .Where(x => (Search.Denfn == true & Search.Denfy == false) ? (x.ab.a.Denf == null  | x.ab.a.Denf == string.Empty): true)
                        .Where(x => Search.GroupCd == "G000" ? true : x.c.Seco06 == Search.GroupCd)

                        .Select(x => new EntryInformation()
                        {
                            Mukounam = x.ab.a.Mukoukbn == "1" ? "無効" : "",
                            Denf = x.ab.a.Denf == "S" ? "再" : x.ab.a.Denf == "A" ? "済" : "未",
                            Sykymd = string.Empty,
                            Sybcod = x.ab.a.Sybcod,
                            Syukno = x.ab.a.Syukno,

                            Tdknam = (x.ab.a.Tdknam ?? string.Empty)
                                     + " " + (x.ab.a.Tdsnam ?? string.Empty)
                                     + " " + (x.ab.a.Tdbnam ?? string.Empty)
                                     + " " + (x.ab.a.Tdktan ?? string.Empty),
                            Dhinnam = x.ab.a.Dhinnam,
                            Tokcod = x.ab.a.Tokcod,
                            Toknam = x.c.Tornam,
                            Crtnam = x.ab.b.UserName,
                            Cdate = x.ab.a.Cdate,
                            TSykymd = (DateTime)x.ab.a.Sykymd                            
                        })
                        .ToListAsync();

            // DateTime?文字列変換用
            var list2 = list
                        .Select(x => new EntryInformation()
                        {
                            Mukounam = x.Mukounam,
                            Denf = x.Denf,
                            Sykymd = x.TSykymd.ToString("yyyy/MM/dd"),
                            Sybcod = x.Sybcod,
                            Syukno = Common.DataUtil.GetSyukno(x.Syukno),
                            Tdknam = x.Tdknam,
                            Dhinnam = x.Dhinnam,
                            Tokcod = x.Tokcod,
                            Toknam = x.Toknam,
                            Crtnam = x.Crtnam,
                            Cdate = x.Cdate,
                            TSykymd = x.TSykymd
                        })
                        //出荷日の降順、出荷Noの降順
                        .OrderByDescending(x => x.TSykymd.ToString("yyyy/MM/dd"))
                        .ThenBy(x => x.Syukno);

            return list2;
        }

        // 登録 表示
        public async Task<IEnumerable<ShuukaTyuumonshoEntryViewModel>> GetShuukaTyuumonshoAsync(string Syukno, string Cdate)
        {
            var ShuukaTyuumonsho = await dbContext.TUnsouShuukaTyuumonshoTehai
                                    //コントロールマスタ_部門
                                    .GroupJoin(dbContext.MControl, 
                                               a => new { key1 = Macss.Repositories.ControlRepository.MControlSection.AccountBumon, key2 = a.Pccod.Substring(0, 4) }, 
                                               b => new { key1 = b.Section, key2 = b.Kbn },
                                               (a, b) => new { a, b })
                                    .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a.a, b })
                                    // 取引先マスタ_得意先
                                    .GroupJoin(dbContext.MTorihikisaki, ab => ab.a.Tokcod, c => c.Torcod, (ab, c) => new { ab, c })
                                    .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new { ab.ab, c })
                                    // 取引先マスタ＿仕入先
                                    .GroupJoin(dbContext.MTorihikisaki, abc => abc.ab.a.Sircod, d => d.Torcod, (abc, d) => new { abc, d })
                                    .SelectMany(x => x.d.DefaultIfEmpty(), (abc, d) => new { abc.abc, d })
                                    // 出荷場所マスタ
                                    .GroupJoin(dbContext.MShukkabasho, abcd => abcd.abc.ab.a.Sybcod, e => e.Sybcod, (abcd, e) => new { abcd, e })
                                    .SelectMany(x => x.e.DefaultIfEmpty(), (abcd, e) => new { abcd.abcd, e })
                                    // 運送方法マスタ
                                    .GroupJoin(dbContext.MUnsouUnsouhouhou, abcde => abcde.abcd.abc.ab.a.Unscod, f => f.Unscod, (abcde, f) => new { abcde, f })
                                    .SelectMany(x => x.f.DefaultIfEmpty(), (abcde, f) => new { abcde.abcde, f })
                                    // 運送区分マスタ
                                    .GroupJoin(dbContext.MUnsouUnsoukubun, abcdef => abcdef.abcde.abcd.abc.ab.a.Unskbn, g => g.Unskbn, (abcdef, g) => new { abcdef, g })
                                    .SelectMany(x => x.g.DefaultIfEmpty(), (abcdef, g) => new { abcdef.abcdef, g })

                                    .Where(x => x.abcdef.abcde.abcd.abc.ab.a.Syukno == Syukno)
                                    .Where(x => x.abcdef.abcde.abcd.abc.ab.a.Cdate == Cdate)

                                    .Select(x => new ShuukaTyuumonshoEntryViewModel()
                                    {
                                        SyuknoMae = "000", //保存チェック用ダミー
                                        Syukno = x.abcdef.abcde.abcd.abc.ab.a.Syukno,
                                        VSyukno = string.Empty,
                                        Inscod = x.abcdef.abcde.abcd.abc.ab.a.Inscod,
                                        Pccod1 = x.abcdef.abcde.abcd.abc.ab.a.Pccod == null ? string.Empty : x.abcdef.abcde.abcd.abc.ab.a.Pccod.Substring(0,4),
                                        Pccod1n = x.abcdef.abcde.abcd.abc.ab.b.Value1,
                                        Htynam = x.abcdef.abcde.abcd.abc.ab.a.Htynam,
                                        Htykah = x.abcdef.abcde.abcd.abc.ab.a.Htykah,
                                        Tancod = x.abcdef.abcde.abcd.abc.ab.a.Tancod,
                                        Tannam = x.abcdef.abcde.abcd.abc.ab.a.Tannam,
                                        Htytel = x.abcdef.abcde.abcd.abc.ab.a.Htytel,
                                        Basyo = x.abcdef.abcde.abcd.abc.ab.a.Basyo,
                                        Fsykno = x.abcdef.abcde.abcd.abc.ab.a.Fsykno,
                                        Tokcod = x.abcdef.abcde.abcd.abc.ab.a.Tokcod,
                                        Toknam = x.abcdef.abcde.abcd.abc.c.Tornam,
                                        Sybcod = x.abcdef.abcde.abcd.abc.ab.a.Sybcod,
                                        Sybnam = x.abcdef.abcde.e.Sybnam,
                                        Sykymd = string.Empty,
                                        Ufutan = x.abcdef.abcde.abcd.abc.ab.a.Ufutan,
                                        Keifno = x.abcdef.abcde.abcd.abc.ab.a.Keifno,
                                        Tdkcod = x.abcdef.abcde.abcd.abc.ab.a.Tdkcod,
                                        Tdkyub = x.abcdef.abcde.abcd.abc.ab.a.Tdkyub,
                                        Tdkjyu = x.abcdef.abcde.abcd.abc.ab.a.Tdkjyu,
                                        Tdknam = x.abcdef.abcde.abcd.abc.ab.a.Tdknam,
                                        Tdsnam = x.abcdef.abcde.abcd.abc.ab.a.Tdsnam,
                                        Tdbnam = x.abcdef.abcde.abcd.abc.ab.a.Tdbnam,
                                        Tdktan = x.abcdef.abcde.abcd.abc.ab.a.Tdktan,
                                        Tdktel = x.abcdef.abcde.abcd.abc.ab.a.Tdktel,
                                        Tricod = x.abcdef.abcde.abcd.abc.ab.a.Dhincod,
                                        Trinam = x.abcdef.abcde.abcd.abc.ab.a.Dhinnam,
                                        Dsyuksu = string.Empty,
                                        Tkjiko1 = x.abcdef.abcde.abcd.abc.ab.a.Tkjiko.Length > 20 ? x.abcdef.abcde.abcd.abc.ab.a.Tkjiko.Substring(0,20) : x.abcdef.abcde.abcd.abc.ab.a.Tkjiko,
                                        Tkjiko2 = x.abcdef.abcde.abcd.abc.ab.a.Tkjiko.Length <= 20 ? string.Empty : x.abcdef.abcde.abcd.abc.ab.a.Tkjiko.Substring(20),
                                        Yusono = x.abcdef.abcde.abcd.abc.ab.a.Yusono,
                                        Coment = x.abcdef.abcde.abcd.abc.ab.a.Coment,
                                        Kisyua = x.abcdef.abcde.abcd.abc.ab.a.Kisyu.Length > 2 ? x.abcdef.abcde.abcd.abc.ab.a.Kisyu.Substring(0, 2) : x.abcdef.abcde.abcd.abc.ab.a.Kisyu,
                                        Kisyub = x.abcdef.abcde.abcd.abc.ab.a.Kisyu.Length <= 2 ? string.Empty : x.abcdef.abcde.abcd.abc.ab.a.Kisyu.Substring(2),
                                        Unscod = x.abcdef.abcde.abcd.abc.ab.a.Unscod,
                                        Unscodnam = x.abcdef.f.Unsnam,
                                        Unscrs = x.abcdef.abcde.abcd.abc.ab.a.Unscrs,
                                        Sircod = x.abcdef.abcde.abcd.abc.ab.a.Sircod,
                                        Sirnam = x.abcdef.abcde.abcd.d.Tornam,
                                        Unskbn = x.abcdef.abcde.abcd.abc.ab.a.Unskbn,
                                        Unskbnnam = x.g.Unsnam,
                                        Yubflg = x.abcdef.abcde.abcd.abc.ab.a.Yubflg == "0" ? true : false,
                                        Mukoukbn = x.abcdef.abcde.abcd.abc.ab.a.Mukoukbn == "1" ? true : false,
                                        Mukouriyuu = x.abcdef.abcde.abcd.abc.ab.a.Mukouriyuu,
                                        Cdate = x.abcdef.abcde.abcd.abc.ab.a.Cdate,
                                        Denf = x.abcdef.abcde.abcd.abc.ab.a.Denf,
                                        TSykymd = x.abcdef.abcde.abcd.abc.ab.a.Sykymd.Value,
                                        TDsyuksu = x.abcdef.abcde.abcd.abc.ab.a.Dsyuksu.Value
                                    })
                                    .ToListAsync();

            var ShuukaTyuumonsho2 = ShuukaTyuumonsho
                                    .Select(x => new ShuukaTyuumonshoEntryViewModel()
                                    {
                                        SyuknoMae = x.SyuknoMae,
                                        Syukno = x.Syukno,
                                        VSyukno = Common.DataUtil.GetSyukno(x.Syukno),
                                        Inscod = x.Inscod,
                                        Pccod1 = x.Pccod1,
                                        Pccod1n = x.Pccod1n,
                                        Htynam = x.Htynam,
                                        Htykah = x.Htykah,
                                        Tancod = x.Tancod,
                                        Tannam = x.Tannam,
                                        Htytel = x.Htytel,
                                        Basyo = x.Basyo,
                                        Fsykno = x.Fsykno,
                                        Tokcod = x.Tokcod,
                                        Toknam = x.Toknam,
                                        Sybcod = x.Sybcod,
                                        Sybnam = x.Sybnam,
                                        Sykymd = x.TSykymd.ToString("yyyy/MM/dd"),
                                        Ufutan = x.Ufutan,
                                        Keifno = x.Keifno,
                                        Tdkcod = x.Tdkcod,
                                        Tdkyub = x.Tdkyub,
                                        Tdkjyu = x.Tdkjyu,
                                        Tdknam = x.Tdknam,
                                        Tdsnam = x.Tdsnam,
                                        Tdbnam = x.Tdbnam,
                                        Tdktan = x.Tdktan,
                                        Tdktel = x.Tdktel,
                                        Tricod = x.Tricod,
                                        Trinam = x.Trinam,
                                        Dsyuksu = x.TDsyuksu.ToString(),
                                        Tkjiko1 = x.Tkjiko1.Trim(),
                                        Tkjiko2 = x.Tkjiko2,
                                        Yusono = x.Yusono,
                                        Coment = x.Coment,
                                        Kisyua = x.Kisyua,
                                        Kisyub = x.Kisyub,
                                        Unscod = x.Unscod,
                                        Unscodnam = x.Unscodnam,
                                        Unscrs = x.Unscrs,
                                        Sircod = x.Sircod,
                                        Sirnam = x.Sirnam,
                                        Unskbn = x.Unskbn,
                                        Unskbnnam = x.Unskbnnam,
                                        Yubflg = x.Yubflg,
                                        Mukoukbn = x.Mukoukbn,
                                        Mukouriyuu = x.Mukouriyuu,
                                        Cdate = x.Cdate,
                                        Denf = x.Denf,
                                        TSykymd = x.TSykymd,
                                        TDsyuksu = x.TDsyuksu
                                    });

            return ShuukaTyuumonsho2;
        }

        // 登録(明細) 表示
        public async Task<IEnumerable<ShuukaTyuumonshoEntryMeisaiViewModel>> GetShuukaTyuumonshoMeisaiAsync(string Syukno, string Cdate)
        {
            var ShuukaTyuumonshoMeisai = await dbContext.TUnsouShuukaTyuumonshoTehaiMeisai
                                            .Where(x => x.Syukno == Syukno)
                                            .Where(x => x.Cdate == Cdate)
                                            .Select(x => new ShuukaTyuumonshoEntryMeisaiViewModel()
                                            {
                                                Renban = x.Renban,
                                                No = x.Renban.ToString(),
                                                Hincod = x.Hincod,
                                                Hinnam = x.Hinnam,
                                                Syuksu = string.Empty,
                                                TSyuksu = x.Syuksu
                                            })
                                            .OrderBy(x => x.Renban)
                                            .ToListAsync();

            // 空行追加
            int? maxNo = ShuukaTyuumonshoMeisai.Max(x => x.Renban);
            var ShuukaTyuumonshoMeisai2 = new List<ShuukaTyuumonshoEntryMeisaiViewModel>();
            if (maxNo != null)
            {
                for (int row = 1; row < maxNo + 1; row++)
                {
                    var data = ShuukaTyuumonshoMeisai.Where(x => x.No == row.ToString());
                    if (data != null && data.Count() != 0)
                    {
                        var add = data.First();
                        ShuukaTyuumonshoMeisai2.Add(new ShuukaTyuumonshoEntryMeisaiViewModel()
                        {
                            Hincod = add.Hincod,
                            Hinnam = add.Hinnam,
                            Syuksu = add.TSyuksu.Value.ToString(),
                            TSyuksu = add.TSyuksu,
                        });
                    } else
                    {
                        ShuukaTyuumonshoMeisai2.Add(new ShuukaTyuumonshoEntryMeisaiViewModel()
                        {
                            Hincod = string.Empty,
                            Hinnam = string.Empty,
                            Syuksu = string.Empty,
                            TSyuksu = 0
                        });
                    }
                }
            }

            // 空行追加
            var addRow = new List<ShuukaTyuumonshoEntryMeisaiViewModel>();
            for (int row = ShuukaTyuumonshoMeisai2.Count(); row < 5; row++)
            {
                addRow.Add(new ShuukaTyuumonshoEntryMeisaiViewModel()
                {
                    Hincod = string.Empty,
                    Hinnam = string.Empty,
                    Syuksu = string.Empty,
                    TSyuksu = 0
                });
            }

            var ShuukaTyuumonshoMeisai3 = ShuukaTyuumonshoMeisai2.Concat(addRow);

            //var ShuukaTyuumonshoMeisai2 = ShuukaTyuumonshoMeisai
            //                                .Select(x => new ShuukaTyuumonshoEntryMeisaiViewModel()
            //                                {
            //                                    Hincod = x.Hincod,
            //                                    Hinnam = x.Hinnam,
            //                                    Syuksu = x.TSyuksu.Value.ToString(),
            //                                    TSyuksu = x.TSyuksu
            //                                })
            //                                .Concat(addRow);

            return ShuukaTyuumonshoMeisai3;
        }

        // 登録 運送コースマスタ取得
        public async Task<IEnumerable<MUnsouUnsoukousu>> GetMUnsoukousuAsync()
        {
            return await dbContext.MUnsouUnsoukousu.OrderBy(x => x.Unscrs).ToListAsync();
        }

        // 登録 採番用
        public decimal GetSyknoAto(string SyuknoMae)
        {
            var seq_name = "seq_" + SyuknoMae + "_no";
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

        // 登録 出荷注文書パターンマスタ取得
        public async Task<IEnumerable<ShuukaTyuumonshoEntryViewModel>> GetShuukaTyuumonshoPattern(string Sykno2, string Inscod)
        {
            var ShuukaTyuumonshoPattern = await dbContext.MUnsouShuukaTyuumonshoPattern
                                            // 取引先マスタ_得意先
                                            .GroupJoin(dbContext.MTorihikisaki, a => a.Tokcod, b => b.Torcod, (a, b) => new { a, b })
                                            .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a.a, b })
                                            // 出荷場所マスタ
                                            .GroupJoin(dbContext.MShukkabasho, ab => ab.a.Sybcod, c => c.Sybcod, (ab, c) => new { ab, c })
                                            .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new { ab.ab, c })
                                            // 運送方法マスタ
                                            .GroupJoin(dbContext.MUnsouUnsouhouhou, abc => abc.ab.a.Unscod, d => d.Unscod, (abc, d) => new { abc, d })
                                            .SelectMany(x => x.d.DefaultIfEmpty(), (abc, d) => new { abc.abc, d })

                                            .Where(x => x.abc.ab.a.Sykno2 == Sykno2)
                                            .Where(x => x.abc.ab.a.Inscod == Inscod)

                                            .Select(x => new ShuukaTyuumonshoEntryViewModel()
                                            {
                                                Inscod = x.abc.ab.a.Inscod,
                                                Htynam = x.abc.ab.a.Htynam,
                                                Htykah = x.abc.ab.a.Ktykho,
                                                Tancod = x.abc.ab.a.Tancod,
                                                Tannam = x.abc.ab.a.Tannam,
                                                Htytel = x.abc.ab.a.Ktytel,
                                                Basyo = x.abc.ab.a.Basyo,
                                                Tokcod = x.abc.ab.a.Tokcod,
                                                Toknam = x.abc.ab.b.Tornam,
                                                Sybcod = x.abc.ab.a.Sybcod,
                                                Sybnam = x.abc.c.Sybnam,
                                                Ufutan = x.abc.ab.a.Ufutan,
                                                Keifno = x.abc.ab.a.Keifno,
                                                Tdkcod = x.abc.ab.a.Tdkcod,
                                                Tdkyub = x.abc.ab.a.Tdkyub,
                                                Tdkjyu = x.abc.ab.a.Jdkjyu,
                                                Tdknam = x.abc.ab.a.Tdknam,
                                                Tdsnam = x.abc.ab.a.Tdsnam,
                                                Tdbnam = x.abc.ab.a.Tdbnam,
                                                Tdktan = x.abc.ab.a.Tdktan,
                                                Tdktel = x.abc.ab.a.Tdktel,
                                                Hincd1 = x.abc.ab.a.Hincd1,
                                                Hincd2 = x.abc.ab.a.Hincd2,
                                                Hincd3 = x.abc.ab.a.Hincd3,
                                                Hincd4 = x.abc.ab.a.Hincd4,
                                                Hincd5 = x.abc.ab.a.Hincd5,
                                                Hinnm1 = x.abc.ab.a.Hinnm1,
                                                Hinnm2 = x.abc.ab.a.Hinnm2,
                                                Hinnm3 = x.abc.ab.a.Hinnm3,
                                                Hinnm4 = x.abc.ab.a.Hinnm4,
                                                Hinnm5 = x.abc.ab.a.Hinnm5,
                                                Syksu1 = x.abc.ab.a.Syksu1,
                                                Syksu2 = x.abc.ab.a.Syksu2,
                                                Syksu3 = x.abc.ab.a.Syksu3,
                                                Syksu4 = x.abc.ab.a.Syksu4,
                                                Syksu5 = x.abc.ab.a.Syksu5,
                                                Tkjiko2 = x.abc.ab.a.Tkjiko,
                                                Unscod = x.abc.ab.a.Unscod,
                                                Unscodnam = x.d.Unsnam,
                                                Unscrs = x.abc.ab.a.Unscrs
                                            })
                                            .ToListAsync();

            IEnumerable<ShuukaTyuumonshoEntryViewModel> ViewModel = ShuukaTyuumonshoPattern
                .Select(x => new ShuukaTyuumonshoEntryViewModel()
                {
                    Htynam = x.Htynam == null ? string.Empty : x.Htynam.Trim(),
                    Htykah = x.Htykah == null ? string.Empty : x.Htykah.Trim(),
                    Tancod = x.Tancod == null ? string.Empty : x.Tancod.Trim(),
                    Tannam = x.Tannam == null ? string.Empty : x.Tannam.Trim(),
                    Htytel = x.Htytel == null ? string.Empty : x.Htytel.Trim(),
                    Basyo = x.Basyo == null ? string.Empty : x.Basyo.Trim(),
                    Tokcod = x.Tokcod == null ? string.Empty : x.Tokcod.Trim(),
                    Toknam = x.Toknam == null ? string.Empty : x.Toknam.Trim(),
                    Sybcod = x.Sybcod == null ? string.Empty : x.Sybcod.Trim(),
                    Sybnam = x.Sybnam == null ? string.Empty : x.Sybnam.Trim(),
                    Ufutan = x.Ufutan == null ? string.Empty : x.Ufutan.Trim(),
                    Keifno = x.Keifno == null ? string.Empty : x.Keifno.Trim(),
                    Tdkcod = x.Tdkcod == null ? string.Empty : x.Tdkcod.Trim(),
                    Tdkyub = x.Tdkyub == null ? string.Empty : x.Tdkyub.Trim(),
                    Tdkjyu = x.Tdkjyu == null ? string.Empty : x.Tdkjyu.Trim(),
                    Tdknam = x.Tdknam == null ? string.Empty : x.Tdknam.Trim(),
                    Tdsnam = x.Tdsnam == null ? string.Empty : x.Tdsnam.Trim(),
                    Tdbnam = x.Tdbnam == null ? string.Empty : x.Tdbnam.Trim(),
                    Tdktan = x.Tdktan == null ? string.Empty : x.Tdktan.Trim(),
                    Tdktel = x.Tdktel == null ? string.Empty : x.Tdktel.Trim(),
                    Hincd1 = x.Hincd1 == null ? string.Empty : x.Hincd1.Trim(),
                    Hincd2 = x.Hincd2 == null ? string.Empty : x.Hincd2.Trim(),
                    Hincd3 = x.Hincd3 == null ? string.Empty : x.Hincd3.Trim(),
                    Hincd4 = x.Hincd4 == null ? string.Empty : x.Hincd4.Trim(),
                    Hincd5 = x.Hincd5 == null ? string.Empty : x.Hincd5.Trim(),
                    Hinnm1 = x.Hinnm1 == null ? string.Empty : x.Hinnm1.Trim(),
                    Hinnm2 = x.Hinnm2 == null ? string.Empty : x.Hinnm2.Trim(),
                    Hinnm3 = x.Hinnm3 == null ? string.Empty : x.Hinnm3.Trim(),
                    Hinnm4 = x.Hinnm4 == null ? string.Empty : x.Hinnm4.Trim(),
                    Hinnm5 = x.Hinnm5 == null ? string.Empty : x.Hinnm5.Trim(),
                    Syksu1 = x.Syksu1,
                    Syksu2 = x.Syksu2,
                    Syksu3 = x.Syksu3,
                    Syksu4 = x.Syksu4,
                    Syksu5 = x.Syksu5,
                    Tkjiko1 = x.Tkjiko2 == null ? string.Empty : x.Tkjiko2.Trim().Length > 20 ? x.Tkjiko2.Trim().Substring(0, 20) : x.Tkjiko2.Trim(),
                    Tkjiko2 = x.Tkjiko2 == null ? string.Empty : x.Tkjiko2.Trim().Length <= 20 ? string.Empty : x.Tkjiko2.Trim().Substring(20),
                    Unscod = x.Unscod == null ? x.Unscod : x.Unscod.Trim(),
                    Unscodnam = x.Unscodnam == null ? string.Empty : x.Unscodnam.Trim(),
                    Unscrs = x.Unscrs == null ? string.Empty : x.Unscrs.Trim()
                });

            //if (ShuukaTyuumonshoPattern.Count() != 0)
            //{
            //    var pattern = ShuukaTyuumonshoPattern.First();
            //    if (pattern.Inscod != Inscod)
            //    {
            //        IEnumerable<ShuukaTyuumonshoEntryViewModel> model = ShuukaTyuumonshoPattern
            //            .Select(x => new ShuukaTyuumonshoEntryViewModel() { });

            //        return model;
            //    }
            //}
            return ViewModel;
        }

        // 登録 取引先マスタ取得
        public async Task<IEnumerable<MTorihikisaki>> GetMTorihikisakiAsync()
        {
            return await dbContext.MTorihikisaki.OrderBy(x => x.Torcod).ToListAsync();
        }

        // 登録 届先マスタ取得
        public async Task<IEnumerable<ShuukaTyuumonshoEntryViewModel>> GetVTodokesakiAsync()
        {
            VUnsouTodokesaki vUnsouTodokesaki = new VUnsouTodokesaki();
            var UnsouTodokesaki = await vUnsouTodokesaki.v_unsou_todokesaki
                            .Where(x => x.DELFLG != "1")
                            .OrderBy(x => x.TDKCOD)
                            .Select(x => new ShuukaTyuumonshoEntryViewModel()
                            {
                                Tdkcod = x.TDKCOD,
                                Tdkjyu = x.JYUSYO,
                                Tdknam = x.TDKNAM,
                                Tdsnam = x.TDKNMS,
                                Tdbnam = x.TDBNAM,
                                Tdktan = x.TDKTAN,
                                Tdktel = x.TDKTEL,
                                Tdkyub = x.YUBINN
                            })
                            .ToListAsync();

            return UnsouTodokesaki
                .Select(x => new ShuukaTyuumonshoEntryViewModel()
                {
                    Tdkcod = x.Tdkcod == null ? string.Empty : x.Tdkcod.Trim(),
                    Tdkjyu = x.Tdkjyu == null ? string.Empty : x.Tdkjyu.Trim(),
                    Tdknam = x.Tdknam == null ? string.Empty : x.Tdknam.Trim(),
                    Tdsnam = x.Tdsnam == null ? string.Empty : x.Tdsnam.Trim(),
                    Tdbnam = x.Tdbnam == null ? string.Empty : x.Tdbnam.Trim(),
                    Tdktan = x.Tdktan == null ? string.Empty : x.Tdktan.Trim(),
                    Tdktel = x.Tdktel == null ? string.Empty : x.Tdktel.Trim(),
                    Tdkyub = x.Tdkyub == null ? string.Empty : x.Tdkyub.Trim()
                });
        }

        // 登録 郵便番号マスタ取得
        public async Task<IEnumerable<MUnsouPostalcode>> GetMUnsouPostalcodeAsync()
        {
            return await dbContext.MUnsouPostalcode.OrderBy(x => x.Jyusy1).ThenBy(x => x.Jyusy2).ToListAsync();
        }

        // 登録 品名マスタ取得
        public async Task<IEnumerable<ShuukaTyuumonshoEntryMeisaiViewModel>> GetHinmeiAsync()
        {
            VUnsouHinmei vUnsouHinmei = new VUnsouHinmei();
            var UnsouHinmei = await vUnsouHinmei.v_unsou_hinmei
                            .Where(x => x.DELFLG != "1")
                            .OrderBy(x => x.HINCOD)
                            .Select(x => new ShuukaTyuumonshoEntryMeisaiViewModel()
                            {
                                Hincod = x.HINCOD,
                                Hinnam = x.HINNAM,
                                Syuksu = string.Empty,
                                TSyuksu = 0
                            })
                            .ToListAsync();

            return UnsouHinmei
                .Select(x => new ShuukaTyuumonshoEntryMeisaiViewModel()
                {
                    Hincod = x.Hincod == null ? string.Empty : x.Hincod.Trim(),
                    Hinnam = x.Hinnam == null ? string.Empty : x.Hinnam.Trim(),
                    Syuksu = string.Empty,
                    TSyuksu = 0
                });
        }

        // 登録 Fe機種マスタ取得
        public async Task<IEnumerable<MKishu>> GetMKishuAsync()
        {
            return await dbContext.MKishu.OrderBy(x => x.Kisyua).ToListAsync();
        }

        // 登録 運送方法マスタ取得
        public async Task<IEnumerable<MUnsouUnsouhouhou>> GetMUnsouUnsouhouhouAsync()
        {
            return await dbContext.MUnsouUnsouhouhou.OrderBy(x => x.Unscod).ToListAsync();
        }

        // 登録 運送区分マスタ取得
        public async Task<IEnumerable<MUnsouUnsoukubun>> GetMUnsouUnsoukubunAsync()
        {
            return await dbContext.MUnsouUnsoukubun.OrderBy(x => x.Unskbn).ToListAsync();
        }

        // 登録 DB更新
        public async Task UpdateShuukaTyuumonshoAsync(string mode, ShuukaTyuumonshoEntryViewModel entry, List<ShuukaTyuumonshoEntryMeisaiViewModel> meisai, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var udate = DateTime.Now;
                    var syukno = entry.Syukno;
                    var cdate = entry.Cdate;

                    switch (mode)
                    {
                        case null:
                            // 新規
                            await InsertTehai(entry, syukno, cdate, loginUser, udate);
                            await InsertMeisai(meisai, syukno, cdate, loginUser, udate);
                            break;

                        case "2":  
                            // 更新 
                            await UptadeTehai(entry, syukno, cdate, loginUser, udate);
                            //await DeleteMeisai(syukno, cdate);
                            //await InsertMeisai(meisai, syukno, cdate, loginUser, udate);
                            await UpdateMeisai(meisai, syukno, cdate, loginUser, udate);
                            await UptadeShuuka(entry, syukno, cdate, loginUser, udate);
                            break;

                        case "3":  
                            // 削除
                            await DeleteTehai(syukno, cdate);
                            await DeleteMeisai(syukno, cdate);
                            break;
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        // 新規
        private async Task InsertTehai(ShuukaTyuumonshoEntryViewModel entry, string syukno, string cdate, string loginUser, DateTime udate)
        {
            // 得意先コードから請求先コード・顧客コードを取得
            var Tokuisaki = (await dbContext.MTorihikisaki.ToListAsync()).Where(x => x.Torcod == entry.Tokcod).FirstOrDefault();
            // 運送区分コードから事業区分コードを取得
            var Jgykbn = "    ";
            if (!string.IsNullOrEmpty(entry.Unskbn))
            {
                var Unsoukubun = (await dbContext.MUnsouUnsoukubun.ToListAsync()).Where(x => x.Unskbn == entry.Unskbn).FirstOrDefault();
                if (Unsoukubun != null)
                {
                    Jgykbn = Unsoukubun.Jgykbn;
                }
            }

            var insTehai = new TUnsouShuukaTyuumonshoTehai()
            {
                Syukno = syukno,
                Cdate = cdate,
                Sykymd = DateTime.Parse(entry.Sykymd),
                Kisyu = entry.Kisyua + entry.Kisyub,
                Keifno = entry.Keifno,
                Fsykno = entry.Fsykno,
                Sybcod = entry.Sybcod,
                Tokcod = entry.Tokcod,
                Seicod = Tokuisaki.Seco01,
                Htynam = entry.Htynam,
                Htykah = entry.Htykah,
                Tancod = entry.Tancod,
                Tannam = entry.Tannam,
                Htytel = entry.Htytel,
                Basyo = entry.Basyo,
                Tdkcod = entry.Tdkcod,
                Tdkyub = entry.Tdkyub,
                Tdkjyu = entry.Tdkjyu,
                Tdknam = entry.Tdknam,
                Tdsnam = entry.Tdsnam,
                Tdbnam = entry.Tdbnam,
                Tdktan = entry.Tdktan,
                Tdktel = entry.Tdktel,
                Dhincod = entry.Tricod,
                Dhinnam = entry.Trinam,
                Dsyuksu = decimal.Parse(entry.Dsyuksu),
                Tkjiko = String.Format("{0,-20}", entry.Tkjiko1) + entry.Tkjiko2,

                //Tkjiko = entry.Tkjiko1 + entry.Tkjiko2,
                Coment = entry.Coment,
                Unscod = entry.Unscod,
                Unscrs = entry.Unscrs,
                Sircod = entry.Sircod,
                Unskbn = entry.Unskbn,
                Denkbn = "I",
                Denmsu = 1,
                Ufutan = entry.Ufutan,
                Yusono = entry.Yusono,
                Denf = null,
                Hkuymd = null,
                Hkucod = string.Empty,
                Pccod = entry.Pccod1 + Jgykbn + Tokuisaki.Kokcod,
                Yubflg = entry.Yubflg == true ? "0" : "1",
                Mukoukbn = "0",
                Mukouriyuu = string.Empty,
                Inscod = entry.Inscod,
                Crtcod = loginUser,
                Crtymd = udate,
                Updcod = loginUser,
                Updymd = udate
            };

            // 追加
            dbContext.TUnsouShuukaTyuumonshoTehai.Add(insTehai);
            await dbContext.SaveChangesAsync();
        }

        // 新規(明細)
        private async Task InsertMeisai(List<ShuukaTyuumonshoEntryMeisaiViewModel> meisai, string syukno, string cdate, string loginUser, DateTime udate)
        {
            int renban = 1;
            foreach (var row in meisai)
            {
                if (!string.IsNullOrEmpty(row.Hinnam))
                {
                    if (string.IsNullOrEmpty(row.Syuksu))
                    {
                        row.Syuksu = "0";
                    }

                    var insMeisai = new TUnsouShuukaTyuumonshoTehaiMeisai()
                    {
                        Syukno = syukno,
                        Cdate = cdate,
                        Renban = renban,
                        Hincod = row.Hincod,
                        Hinnam = row.Hinnam,
                        Syuksu = decimal.Parse(row.Syuksu),
                        Crtcod = loginUser,
                        Crtymd = udate,
                        Updcod = loginUser,
                        Updymd = udate
                    };

                    // 追加
                    dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Add(insMeisai);
                    await dbContext.SaveChangesAsync();
                    renban++;
                }
            }
        }

        private async Task UpdateMeisai(List<ShuukaTyuumonshoEntryMeisaiViewModel> meisai, string syukno, string cdate, string loginUser, DateTime udate)
        {
            // 削除処理
            var dbList = await dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Where(x => x.Syukno == syukno).Where(x => x.Cdate == cdate).ToListAsync();
            foreach (TUnsouShuukaTyuumonshoTehaiMeisai db in dbList)
            {
                var check = meisai.Where(x => x.No == db.Renban.ToString());
                if (check == null || check.Count() == 0)
                {
                    // 削除
                    dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Remove(db);
                    await dbContext.SaveChangesAsync();
                } else
                {
                    var work = check.First();
                    if (string.IsNullOrEmpty(work.Hinnam))
                    {
                        // 削除
                        dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Remove(db);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            // 登録・更新処理
            foreach (var row in meisai)
            {
                if (!string.IsNullOrEmpty(row.Hinnam))
                {
                    if (string.IsNullOrEmpty(row.Syuksu))
                    {
                        row.Syuksu = "0";
                    }
                    var dList = await dbContext.TUnsouShuukaTyuumonshoTehaiMeisai
                            .Where(x => x.Syukno == syukno).Where(x => x.Cdate == cdate).Where(x => x.Renban.ToString() == row.No).ToListAsync();
                    if (dList == null || dList.Count() == 0)
                    {
                        int no = int.Parse(row.No);
                        var insMeisai = new TUnsouShuukaTyuumonshoTehaiMeisai()
                        {
                            Syukno = syukno,
                            Cdate = cdate,
                            Renban = no,
                            Hincod = row.Hincod,
                            Hinnam = row.Hinnam,
                            Syuksu = decimal.Parse(row.Syuksu),
                            Crtcod = loginUser,
                            Crtymd = udate,
                            Updcod = loginUser,
                            Updymd = udate
                        };

                        // 追加
                        dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Add(insMeisai);

                    } else
                    {
                        var data = dList.First();
                        if (data.Hincod != row.Hincod || data.Hinnam != row.Hinnam || data.Syuksu != decimal.Parse(row.Syuksu))
                        {
                            // 更新
                            data.Hincod = row.Hincod;
                            data.Hinnam = row.Hinnam;
                            data.Syuksu = decimal.Parse(row.Syuksu);
                            data.Updcod = loginUser;
                            data.Updymd = udate;

                        }

                    }
                }
            }
            await dbContext.SaveChangesAsync();

        }

        // 更新(出荷注文書手配データ)
        private async Task UptadeTehai(ShuukaTyuumonshoEntryViewModel entry, string syukno, string cdate, string loginUser, DateTime udate)
        {
            // 得意先コードから請求先コード・顧客コードを取得
            var Tokuisaki = (await dbContext.MTorihikisaki.ToListAsync()).Where(x => x.Torcod == entry.Tokcod).FirstOrDefault();
            // 運送区分コードから事業区分コードを取得
            var Jgykbn = "    ";
            if (!string.IsNullOrEmpty(entry.Unskbn))
            {
                var Unsoukubun = (await dbContext.MUnsouUnsoukubun.ToListAsync()).Where(x => x.Unskbn == entry.Unskbn).FirstOrDefault();
                if (Unsoukubun != null)
                {
                    Jgykbn = Unsoukubun.Jgykbn;
                }
            }

            // 更新データを取得
            var upd = (await dbContext.TUnsouShuukaTyuumonshoTehai.Where(x => x.Syukno == syukno).Where(x => x.Cdate == cdate).ToListAsync()).FirstOrDefault();

            upd.Sykymd = DateTime.Parse(entry.Sykymd);
            upd.Kisyu = entry.Kisyua + entry.Kisyub;
            upd.Keifno = entry.Keifno;
            upd.Fsykno = entry.Fsykno;
            upd.Sybcod = entry.Sybcod;
            upd.Tokcod = entry.Tokcod;
            upd.Seicod = Tokuisaki.Seco01;
            upd.Htynam = entry.Htynam;
            upd.Htykah = entry.Htykah;
            upd.Tancod = entry.Tancod;
            upd.Tannam = entry.Tannam;
            upd.Htytel = entry.Htytel;
            upd.Basyo = entry.Basyo;
            upd.Tdkcod = entry.Tdkcod;
            upd.Tdkyub = entry.Tdkyub;
            upd.Tdkjyu = entry.Tdkjyu;
            upd.Tdknam = entry.Tdknam;
            upd.Tdsnam = entry.Tdsnam;
            upd.Tdbnam = entry.Tdbnam;
            upd.Tdktan = entry.Tdktan;
            upd.Tdktel = entry.Tdktel;
            upd.Dhincod = entry.Tricod;
            upd.Dhinnam = entry.Trinam;
            upd.Dsyuksu = decimal.Parse(entry.Dsyuksu);
            //upd.Tkjiko = entry.Tkjiko1 + entry.Tkjiko2;
            upd.Tkjiko = String.Format("{0,-20}", entry.Tkjiko1) + entry.Tkjiko2;
            upd.Coment = entry.Coment;
            upd.Unscod = entry.Unscod;
            upd.Unscrs = entry.Unscrs;
            upd.Sircod = entry.Sircod;
            upd.Unskbn = entry.Unskbn;
            upd.Denkbn = "I";
            upd.Denmsu = 1;
            upd.Ufutan = entry.Ufutan;
            upd.Yusono = entry.Yusono;
            upd.Pccod = entry.Pccod1 + Jgykbn + Tokuisaki.Kokcod;
            upd.Yubflg = entry.Yubflg == true ? "0" : "1";
            upd.Mukoukbn = entry.Mukoukbn == true ? "1" : "0";
            upd.Mukouriyuu = entry.Mukouriyuu;
            upd.Inscod = entry.Inscod;
            upd.Updcod = loginUser;
            upd.Updymd = udate;

            // 更新
            await dbContext.SaveChangesAsync();
        }

        // 更新(出荷手配データ)
        private async Task UptadeShuuka(ShuukaTyuumonshoEntryViewModel entry, string syukno, string cdate, string loginUser, DateTime udate)
        {
            // 更新データを取得
            var upd = (await dbContext.TUnsouShuukatehai.Where(x => x.Syukno == syukno).Where(x => x.Cdate == cdate).ToListAsync()).FirstOrDefault();

            if (upd != null)
            {

                string Mkcod = entry.Mukoukbn == true ? "1" : "0";
                if (upd.Mkcod == "0" && Mkcod == "1")
                {
                    // 削除に変更
                    upd.Updcod = loginUser;
                    upd.Updymd = udate;

                }
                upd.Mkriyu = entry.Mukouriyuu ?? string.Empty;
                upd.Mkcod = Mkcod;
                // 更新
                await dbContext.SaveChangesAsyncEx();
            }
        }

        // 削除
        public async Task DeleteTehai(string syukno, string cdate)
        {
            // 削除データを取得
            var del = await dbContext.TUnsouShuukaTyuumonshoTehai.Where(x => x.Syukno == syukno).Where(x => x.Cdate == cdate).SingleOrDefaultAsync();

            // 削除
            dbContext.TUnsouShuukaTyuumonshoTehai.Remove(del);
            await dbContext.SaveChangesAsync();
        }

        // 削除(明細)
        private async Task DeleteMeisai(string syukno, string cdate)
        {
            // 削除データを取得
            var delList = await dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Where(x => x.Syukno == syukno).Where(x => x.Cdate == cdate).ToListAsync();

            // 削除
            foreach(TUnsouShuukaTyuumonshoTehaiMeisai del in delList)
            {
                dbContext.TUnsouShuukaTyuumonshoTehaiMeisai.Remove(del);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}