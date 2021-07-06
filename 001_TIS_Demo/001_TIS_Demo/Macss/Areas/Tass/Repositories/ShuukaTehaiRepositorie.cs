using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using Macss.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Macss.Areas.Tass.ViewModels.ShuukaTehaiViewModels;

namespace Macss.Areas.Tass.Repositories
{
    public class ShuukaTehaiRepositorie : IShuukaTehaiRepositorie
    {

        private readonly ApplicationDB dbContext;
        private TableLockRepository tableLockRepository;
        private TodokesakiRepositorie todokesakiRepositorie;

        public ShuukaTehaiRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
            tableLockRepository = new TableLockRepository(dbContext);
            todokesakiRepositorie = new TodokesakiRepositorie(dbContext);
        }

        // 一覧 表示
        public async Task<IEnumerable<ShuukaTehaiInformation>> GetShuukaInformationAsync(ShuukaTehaiSerch serch, bool ReplicationFlg)
        {

            DateTime skymdF = DateTime.MinValue;
            DateTime skymdT = DateTime.MinValue;

            if (serch.SykymdFrom != null)
            {
                skymdF = DateTime.Parse(serch.SykymdFrom);
            }
            if (serch.SykymdTo != null)
            {
                skymdT = DateTime.Parse(serch.SykymdTo);
            }


            bool syutufF = false;
            List<string> syutuf = new List<string>();
            if (serch.Tehai)
            {
                syutuf.Add("1");
                syutufF = true;
            }

            if (serch.Tehai1)
            {
                syutuf.Add("2");
                syutufF = true;
            }

            if (serch.Tehai2)
            {
                syutuf.Add("3");
                syutufF = true;
            }

            string[] sSyutuf;
            if (syutufF)
            {
                sSyutuf = syutuf.ToArray();

            } else
            {
                sSyutuf = new string[1];

            }

            VUnsouShuukaTehai vUnsouShuukaTehai = new VUnsouShuukaTehai();
            var shuukaTehaiList = await dbContext.VUnsouShuukaTehai
                        // アカウントマスタ
                        .GroupJoin(dbContext.MAccount, a => a.CRTCOD, b => b.Id, (a, b) => new { a, b })
                        .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a.a, b })
                        // 取引先マスタ_得意先
                        .GroupJoin(dbContext.MTorihikisaki, ab => ab.a.TOKCOD, c => c.Torcod, (ab, c) => new { ab, c })
                        .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new { ab.ab, c })
                        //// 届先マスタ
                        //.GroupJoin(dbContext.VUnsouTodokesaki, abc => abc.ab.a.TDKCOD, d => d.TDKCOD, (abc, d) => new { abc, d })
                        //.SelectMany(x => x.d.DefaultIfEmpty(), (abc, d) => new { abc.abc, d })
                        .Where(x => (serch.Syukno == null ? true : x.ab.a.SYUKNO.StartsWith(serch.Syukno)))
                        .Where(x => (serch.Sybcod == null ? true : x.ab.a.SYBCOD == serch.Sybcod))
                        .Where(x => (serch.Crtnam == null ? true : x.ab.b.UserName.StartsWith(serch.Crtnam)))
                        .Where(x => (serch.SykymdFrom == null ? true : x.ab.a.SYKYMD >= skymdF))
                        .Where(x => (serch.SykymdTo == null ? true : x.ab.a.SYKYMD <= skymdT))
                        .Where(x => (serch.Fsykno == null ? true : x.ab.a.FSYKNO.Contains(serch.Fsykno)))
                        .Where(x => (serch.Keifno == null ? true : x.ab.a.KEIFNO.Contains(serch.Keifno)))
                        .Where(x => (serch.Tokcod == null ? true : x.ab.a.TOKCOD.StartsWith(serch.Tokcod)))
                        .Where(x => (serch.Toknam == null ? true : x.c.Tornam.Contains(serch.Toknam)))
                        .Where(x => (serch.Tdkcod == null ? true : x.ab.a.TDKCOD.StartsWith(serch.Tdkcod)))
                        .Where(x => (serch.Tdknam == null ? true : x.ab.a.TDKNAM.Contains(serch.Tdknam)))
                        .Where(x => (serch.Tdsnam == null ? true : x.ab.a.TDSNAM.Contains(serch.Tdsnam)))
                        .Where(x => (serch.Tdbnam == null ? true : x.ab.a.TDBNAM.Contains(serch.Tdbnam)))
                        .Where(x => (serch.Tdktan == null ? true : x.ab.a.TDKTAN.Contains(serch.Tdktan)))
                        .Where(x => (serch.Group == "G000" ? true : x.c.Seco06 == serch.Group))
                        .Where(x => (syutufF == false ? true : sSyutuf.Contains(x.ab.a.SYUTUF)))
                        .Where(x => (serch.Entry == false ? true : x.ab.a.CTLF19 == "A"))
                        //.Where(x => (serch.Ktdkcd == null ? true : x.d.KTDKCD.StartsWith(serch.Ktdkcd)))
                        .Where(x => (serch.Dell == "1" ? true : x.ab.a.DELFLG != "1"))
                        .Select(x => new ShuukaTehaiInformation()
                        {
                            Taisho = x.ab.a.SYUTUF == "1" ? "手" : x.ab.a.SYUTUF == "2" ? "当月" : x.ab.a.SYUTUF == "3" ? "過去" : "",
                            Tdkcod = x.ab.a.TDKCOD,
                            SykymdDt = x.ab.a.SYKYMD,
                            Syukno = x.ab.a.SYUKNO,
                            Keifno = x.ab.a.KEIFNO,
                            Fsykno = x.ab.a.FSYKNO,
                            Tdknam = x.ab.a.TDKNAM,
                            Tdsnam = x.ab.a.TDSNAM,
                            Tdbnam = x.ab.a.TDBNAM,
                            Tdktan = x.ab.a.TDKTAN,
                            Dhinnam = x.ab.a.HINNAM,
                            Felflg = x.ab.a.DELFLG,
                            Sybcod = x.ab.a.SYBCOD,
                            Tokcod = x.ab.a.TOKCOD,
                            Toknam = x.c.Tornam,
                            Dataym = x.ab.a.DATAYM

                        })
                        .OrderByDescending(x => x.Dataym)
                        .ThenBy(x => x.Syukno)
                        .ThenBy(x => x.SykymdDt)
                        .ToListAsync();

            var list2 = shuukaTehaiList
                        .Select(x => new ShuukaTehaiInformation()
                        {
                            Taisho = x.Felflg == "1" ?  x.Taisho + "削" : x.Taisho,
                            Sykymd = x.SykymdDt.ToString("yyyy/MM/dd"),
                            Syukno = x.Syukno,
                            Keifno = x.Keifno,
                            Fsykno = x.Fsykno,
                            Tdkcod = x.Tdkcod,
                            Tdknam = x.Tdknam.Trim() + " " + x.Tdsnam.Trim() + " " + x.Tdbnam.Trim() + " " + x.Tdktan.Trim(),
                            Dhinnam = x.Dhinnam,
                            Sybcod = x.Sybcod,
                            Tokcod = x.Tokcod,
                            Toknam = x.Toknam,
                            Dataym = x.Dataym
                        });


            // レプリケーションチェック
            if (ReplicationFlg || serch.Ktdkcd == null)
            {
                return list2;
            }
            else
            {
                TodokesakiSerch todokesakiSerch = new TodokesakiSerch();
                todokesakiSerch.Ktdkcd = serch.Ktdkcd;
                todokesakiSerch.Del = false;
                var todokesaki = await todokesakiRepositorie.GetTodokesakisAsync(todokesakiSerch);

                var list3 = list2
                        // 届先マスタ
                        .Join(todokesaki, x => x.Tdkcod, x => x.Tdkcod, (a, d) => new { a, d })
                        .Select(x => new ShuukaTehaiInformation()
                        {
                            Taisho = x.a.Taisho,
                            Sykymd = x.a.Sykymd,
                            Syukno = x.a.Syukno,
                            Keifno = x.a.Keifno,
                            Fsykno = x.a.Fsykno,
                            Tdkcod = x.a.Tdkcod,
                            Tdknam = x.a.Tdknam,
                            Dhinnam = x.a.Dhinnam,
                            Sybcod = x.a.Sybcod,
                            Tokcod = x.a.Tokcod,
                            Toknam = x.a.Toknam,
                            Dataym = x.a.Dataym
                        }).ToArray();
                return list3;
            }

        }


        // 一覧 表示
        public async Task<IEnumerable<UnsouShuukaTehaiAll>> GetShuukaAllAsync(ShuukaTehaiSerch serch)
        {

            DateTime skymdF = DateTime.MinValue;
            DateTime skymdT = DateTime.MinValue;

            if (serch.SykymdFrom != null)
            {
                skymdF = DateTime.Parse(serch.SykymdFrom);
            }
            if (serch.SykymdTo != null)
            {
                skymdT = DateTime.Parse(serch.SykymdTo);
            }


            bool syutufF = false;
            List<string> syutuf = new List<string>();
            if (serch.Tehai)
            {
                syutuf.Add("1");
                syutufF = true;
            }

            if (serch.Tehai1)
            {
                syutuf.Add("2");
                syutufF = true;
            }

            if (serch.Tehai2)
            {
                syutuf.Add("3");
                syutufF = true;
            }

            string[] sSyutuf;
            if (syutufF)
            {
                sSyutuf = syutuf.ToArray();

            }
            else
            {
                sSyutuf = new string[1];

            }

            VUnsouShuukaTehai vUnsouShuukaTehai = new VUnsouShuukaTehai();
            var shuukaTehaiList = await dbContext.VUnsouShuukaTehai
                        // アカウントマスタ
                        .GroupJoin(dbContext.MAccount, a => a.CRTCOD, b => b.Id, (a, b) => new { a, b })
                        .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a.a, b })
                        // 取引先マスタ_得意先
                        .GroupJoin(dbContext.MTorihikisaki, ab => ab.a.TOKCOD, c => c.Torcod, (ab, c) => new { ab, c })
                        .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new { ab.ab, c })
                        // 届先マスタ
                        //.GroupJoin(dbContext.VUnsouTodokesaki, abc => abc.ab.a.TDKCOD, d => d.TDKCOD, (abc, d) => new { abc, d })
                        //.SelectMany(x => x.d.DefaultIfEmpty(), (abc, d) => new { abc.abc, d })
                        .Where(x => (serch.Syukno == null ? true : x.ab.a.SYUKNO.StartsWith(serch.Syukno)))
                        .Where(x => (serch.Sybcod == null ? true : x.ab.a.SYBCOD == serch.Sybcod))
                        .Where(x => (serch.Crtnam == null ? true : x.ab.b.UserName.StartsWith(serch.Crtnam)))
                        .Where(x => (serch.SykymdFrom == null ? true : x.ab.a.SYKYMD >= skymdF))
                        .Where(x => (serch.SykymdTo == null ? true : x.ab.a.SYKYMD <= skymdT))
                        .Where(x => (serch.Fsykno == null ? true : x.ab.a.FSYKNO.Contains(serch.Fsykno)))
                        .Where(x => (serch.Keifno == null ? true : x.ab.a.KEIFNO.Contains(serch.Keifno)))
                        .Where(x => (serch.Tokcod == null ? true : x.ab.a.TOKCOD.StartsWith(serch.Tokcod)))
                        .Where(x => (serch.Toknam == null ? true : x.c.Tornam.Contains(serch.Toknam)))
                        .Where(x => (serch.Tdkcod == null ? true : x.ab.a.TDKCOD.StartsWith(serch.Tdkcod)))
                        .Where(x => (serch.Tdknam == null ? true : x.ab.a.TDKNAM.Contains(serch.Tdknam)))
                        .Where(x => (serch.Tdsnam == null ? true : x.ab.a.TDSNAM.Contains(serch.Tdsnam)))
                        .Where(x => (serch.Tdbnam == null ? true : x.ab.a.TDBNAM.Contains(serch.Tdbnam)))
                        .Where(x => (serch.Tdktan == null ? true : x.ab.a.TDKTAN.Contains(serch.Tdktan)))
                        .Where(x => (serch.Group == "G000" ? true : x.c.Seco06 == serch.Group))
                        .Where(x => (syutufF == false ? true : sSyutuf.Contains(x.ab.a.SYUTUF)))
                        .Where(x => (serch.Entry == false ? true : x.ab.a.CTLF19 == "A"))
                        //.Where(x => (serch.Ktdkcd == null ? true : x.d.KTDKCD.StartsWith(serch.Ktdkcd)))
                        .Where(x => (serch.Dell == "1" ? true : x.ab.a.DELFLG != "1"))
                        .Select(x => new UnsouShuukaTehaiAll()
                        {
                            Syukno = x.ab.a.SYUKNO,
                            Dataym = x.ab.a.DATAYM,
                            Sykymd = x.ab.a.SYKYMD,
                            Kisyu = x.ab.a.KISYU,
                            Keifno = x.ab.a.KEIFNO,
                            Fsykno = x.ab.a.FSYKNO,
                            Tancod = x.ab.a.TANCOD,
                            Tannam = x.ab.a.TANNAM,
                            Tdkcod = x.ab.a.TDKCOD,
                            Tdkjyu = x.ab.a.TDKJYU,
                            Tdknam = x.ab.a.TDKNAM,
                            Tdsnam = x.ab.a.TDSNAM,
                            Tdbnam = x.ab.a.TDBNAM,
                            Tdktan = x.ab.a.TDKTAN,
                            Tdktel = x.ab.a.TDKTEL,
                            Tdkyub = x.ab.a.TDKYUB,
                            Hincod = x.ab.a.HINCOD,
                            Hinnam = x.ab.a.HINNAM,
                            Unscod = x.ab.a.UNSCOD,
                            Unscrs = x.ab.a.UNSCRS,
                            Sircod = x.ab.a.SIRCOD,
                            Sgenno = x.ab.a.SGENNO,
                            Unskbn = x.ab.a.UNSKBN,
                            Sybcod = x.ab.a.SYBCOD,
                            Tokcod = x.ab.a.TOKCOD,
                            Seicod = x.ab.a.SEICOD,
                            Denkbn = x.ab.a.DENKBN,
                            Denmsu = x.ab.a.DENMSU,
                            Tkjiko = x.ab.a.TKJIKO,
                            Hososu = x.ab.a.HOSOSU,
                            Nfdate = x.ab.a.NFDATE,
                            Daihno = x.ab.a.DAIHNO,
                            Daihnoym = x.ab.a.DAIHNOYM,
                            Jyuryo = x.ab.a.JYURYO,
                            Hosos3 = x.ab.a.HOSOS3,
                            Jyury3 = x.ab.a.JYURY3,
                            Ufutan = x.ab.a.UFUTAN,
                            Yusono = x.ab.a.YUSONO,
                            Delflg = x.ab.a.DELFLG,
                            Ctlf19 = x.ab.a.CTLF19,
                            Ctlf28 = x.ab.a.CTLF28,
                            Ctlf29 = x.ab.a.CTLF29,
                            Mehkbn = x.ab.a.MEHKBN,
                            Jskkbn = x.ab.a.JSKKBN,
                            Tocymd = x.ab.a.TOCYMD,
                            Taksiz = x.ab.a.TAKSIZ,
                            Seikyu = x.ab.a.SEIKYU,
                            Geppou = x.ab.a.GEPPOU,
                            Pccod = x.ab.a.PCCOD,
                            Sgenn2 = x.ab.a.SGENN2,
                            Stoucd = x.ab.a.STOUCD,
                            Kencod = x.ab.a.KENCOD,
                            Jiscod = x.ab.a.JISCOD,
                            Tensir = x.ab.a.TENSIR,
                            Hatcod = x.ab.a.HATCOD,
                            Sgen35 = x.ab.a.SGEN35,
                            Ecoflg = x.ab.a.ECOFLG,
                            Syuksu = x.ab.a.SYUKSU,
                            Syutuf = x.ab.a.SYUTUF,
                            Crtcod = x.ab.a.CRTCOD,
                            Crtymd = x.ab.a.CRTYMD,
                            Updcod = x.ab.a.UPDCOD,
                            Updymd = x.ab.a.UPDYMD
                        })
                        .OrderByDescending(x => x.Dataym)
                        .ThenBy(x => x.Syukno)
                        .ThenBy(x => x.Dataym)
                        .ToListAsync();

            // レプリケーションチェック
            if (await tableLockRepository.CheckTableLock("m_unsou_todokesaki") || serch.Ktdkcd == null)
            {
                return shuukaTehaiList;
            }
            else
            {

                TodokesakiSerch todokesakiSerch = new TodokesakiSerch();
                todokesakiSerch.Ktdkcd = serch.Ktdkcd;
                todokesakiSerch.Del = false;
                var todokesaki = await todokesakiRepositorie.GetTodokesakisAsync(todokesakiSerch);

                var shuukaTehaiList2 = shuukaTehaiList
                        // 届先マスタ
                        .Join(todokesaki, x => x.Tdkcod, x => x.Tdkcod, (a, d) => new { a, d })
                        .Select(x => new UnsouShuukaTehaiAll()
                        {
                            Syukno = x.a.Syukno,
                            Dataym = x.a.Dataym,
                            Sykymd = x.a.Sykymd,
                            Kisyu = x.a.Kisyu,
                            Keifno = x.a.Keifno,
                            Fsykno = x.a.Fsykno,
                            Tancod = x.a.Tancod,
                            Tannam = x.a.Tannam,
                            Tdkcod = x.a.Tdkcod,
                            Tdkjyu = x.a.Tdkjyu,
                            Tdknam = x.a.Tdknam,
                            Tdsnam = x.a.Tdsnam,
                            Tdbnam = x.a.Tdbnam,
                            Tdktan = x.a.Tdktan,
                            Tdktel = x.a.Tdktel,
                            Tdkyub = x.a.Tdkyub,
                            Hincod = x.a.Hincod,
                            Hinnam = x.a.Hinnam,
                            Unscod = x.a.Unscod,
                            Unscrs = x.a.Unscrs,
                            Sircod = x.a.Sircod,
                            Sgenno = x.a.Sgenno,
                            Unskbn = x.a.Unskbn,
                            Sybcod = x.a.Sybcod,
                            Tokcod = x.a.Tokcod,
                            Seicod = x.a.Seicod,
                            Denkbn = x.a.Denkbn,
                            Denmsu = x.a.Denmsu,
                            Tkjiko = x.a.Tkjiko,
                            Hososu = x.a.Hososu,
                            Nfdate = x.a.Nfdate,
                            Daihno = x.a.Daihno,
                            Daihnoym = x.a.Daihnoym,
                            Jyuryo = x.a.Jyuryo,
                            Hosos3 = x.a.Hosos3,
                            Jyury3 = x.a.Jyury3,
                            Ufutan = x.a.Ufutan,
                            Yusono = x.a.Yusono,
                            Delflg = x.a.Delflg,
                            Ctlf19 = x.a.Ctlf19,
                            Ctlf28 = x.a.Ctlf28,
                            Ctlf29 = x.a.Ctlf29,
                            Mehkbn = x.a.Mehkbn,
                            Jskkbn = x.a.Jskkbn,
                            Tocymd = x.a.Tocymd,
                            Taksiz = x.a.Taksiz,
                            Seikyu = x.a.Seikyu,
                            Geppou = x.a.Geppou,
                            Pccod = x.a.Pccod,
                            Sgenn2 = x.a.Sgenn2,
                            Stoucd = x.a.Stoucd,
                            Kencod = x.a.Kencod,
                            Jiscod = x.a.Jiscod,
                            Tensir = x.a.Tensir,
                            Hatcod = x.a.Hatcod,
                            Sgen35 = x.a.Sgen35,
                            Ecoflg = x.a.Ecoflg,
                            Syuksu = x.a.Syuksu,
                            Syutuf = x.a.Syutuf,
                            Crtcod = x.a.Crtcod,
                            Crtymd = x.a.Crtymd,
                            Updcod = x.a.Updcod,
                            Updymd = x.a.Updymd
                        }).ToArray();

                return shuukaTehaiList2;

            }
        }

        // 明細 表示
        public async Task<ShuukaTehaiViewModels> GetShuukaMeisaiAsync(string Syukno, string Taisho, string Dataym)
        {
            string taishoCd = "";
            if (Taisho == "手" || Taisho == "手削")
            {
                taishoCd = "1";
            } else if (Taisho == "当月" || Taisho == "当月削")
            {
                taishoCd = "2";
            } else if (Taisho == "過去" || Taisho == "過去削")
            {
                taishoCd = "3";
            }

            List<ShuukaTehaiViewModels> shuukaTehaiList = await dbContext.VUnsouShuukaTehai
                    // 取引先マスタ_得意先
                    .GroupJoin(dbContext.MTorihikisaki, x => x.TOKCOD, x => x.Torcod, (sh, to) => new { sh, to })
                    .SelectMany(x => x.to.DefaultIfEmpty(), (sh, to) => new { sh.sh, to })
                    // 取引先マスタ_仕入先
                    .GroupJoin(dbContext.MTorihikisaki, x => x.sh.SIRCOD, x => x.Torcod, (sh, si) => new { sh, sh.to, si })
                    .SelectMany(x => x.si.DefaultIfEmpty(), (sh, si) => new { sh.sh.sh, sh.sh.to, si })
                    // 運送方法マスタ
                    .GroupJoin(dbContext.MUnsouUnsouhouhou, x => x.sh.UNSCOD, x => x.Unscod, (sh, uh) => new {sh, sh.to, sh.si, uh })
                    .SelectMany(x => x.uh.DefaultIfEmpty(), (sh, uh) => new { sh.sh.sh, sh.to, sh.si, uh })
                    // 運送区分マスタ
                    .GroupJoin(dbContext.MUnsouUnsoukubun, x => x.sh.UNSKBN, x => x.Unskbn, (sh, uk) => new { sh, sh.to, sh.si, sh.uh, uk })
                    .SelectMany(x => x.uk.DefaultIfEmpty(), (sh, uk) => new { sh.sh.sh, sh.to, sh.si, sh.uh, uk })
                    // 出荷場所マスタ
                    .GroupJoin(dbContext.MShukkabasho, x => x.sh.SYBCOD, x => x.Sybcod, (sh, sb) => new { sh, sh.to, sh.si, sh.uh, sh.uk, sb })
                    .SelectMany(x => x.sb.DefaultIfEmpty(), (sh, sb) => new { sh.sh.sh, sh.to, sh.si, sh.uh, sh.uk, sb })
                    .Where(x => x.sh.SYUKNO == Syukno)
                    .Where(x => x.sh.DATAYM == Dataym)
                    .Where(x => x.sh.SYUTUF == taishoCd)
                    .Select(x => new ShuukaTehaiViewModels()
                    {
                        Syukno = x.sh.SYUKNO,
                        Denkbn = x.sh.DENKBN,
                        TocymdDt = x.sh.TOCYMD,
                        Syutuf = x.sh.SYUTUF == "1" ? "出荷手配" : x.sh.SYUTUF == "2" ? "出荷実績（当月）" : x.sh.SYUTUF == "3" ? "出荷実績（過去）" : "",
                        Fsykno = x.sh.FSYKNO,
                        Tokcod = x.sh.TOKCOD,
                        Tokcnm = x.to.Tornam,
                        SykymdDt = x.sh.SYKYMD,
                        Unscod = x.sh.UNSCOD,
                        Unscnm = x.uh.Unsnam,
                        Unscrs = x.sh.UNSCRS,
                        Sircod = x.sh.SIRCOD,
                        Sircnm = x.si.Tornam,
                        Unskbn = x.sh.UNSKBN,
                        Unskbnnm = x.uk.Unsnam,
                        Sybcod = x.sh.SYBCOD,
                        Sybcnm = x.sb.Sybnam,
                        Ufutan = x.sh.UFUTAN == "1" ? x.sh.UFUTAN + ":着払" : x.sh.UFUTAN == "2" ? x.sh.UFUTAN + ":元払" : "",
                        Keifno = x.sh.KEIFNO,
                        Kisyua = x.sh.KISYU.Substring(0, 2),    // 機種A
                        Kisyub = x.sh.KISYU.Substring(2, 6),    // 機種B
                        Tancod = x.sh.TANCOD,
                        Tannam = x.sh.TANNAM,
                        Ninusi = x.sh.CTLF28 + x.sh.CTLF29,
                        Tdkcod = x.sh.TDKCOD,
                        Tdkyub = x.sh.TDKYUB,
                        Tdktel = x.sh.TDKTEL,
                        Tdkjyu = x.sh.TDKJYU,
                        Tdknam = x.sh.TDKNAM,
                        Tdsnam = x.sh.TDSNAM,
                        Tdbnam = x.sh.TDBNAM,
                        Tdktan = x.sh.TDKTAN,
                        Hincod = x.sh.HINCOD,
                        Hinnam = x.sh.HINNAM,
                        Syuksu = x.sh.SYUKSU,
                        Tkjiko1 = x.sh.TKJIKO.Substring(0, 20),   // 特記事項
                        Tkjiko2 = x.sh.TKJIKO.Substring(20, 20),  // 特記事項
                        Yusono = x.sh.YUSONO.Substring(0, 3),
                        Daihno = x.sh.DAIHNO,
                        Hososu = x.sh.HOSOSU,
                        Jyuryo = x.sh.JYURYO,
                        Taksiz = x.sh.TAKSIZ,
                        Sgen35 = x.sh.SGEN35,
                        Sgenno = x.sh.SGENNO,
                        Stoucd = x.sh.STOUCD,
                        Hatcod = x.sh.HATCOD,
                        Jiscod = x.sh.JISCOD,
                        Sgenn2 = x.sh.SGENN2,
                        Pccod = x.sh.PCCOD,
                        Mehkbn = x.sh.MEHKBN,
                        Tensir = x.sh.TENSIR,
                        Ecoflg = x.sh.ECOFLG,
                        Delflg = x.sh.DELFLG
                    })
                    .ToListAsync();

            ShuukaTehaiViewModels shuukaTehaiData = shuukaTehaiList.FirstOrDefault();

            shuukaTehaiData.Tocymd = shuukaTehaiData.TocymdDt.ToString("yyyy/MM/dd");
            DateTime dt = DateTime.Parse("2001/01/01");
            if (dt > shuukaTehaiData.TocymdDt)
            {
                shuukaTehaiData.Tocymd = "";
            }
            shuukaTehaiData.Sykymd = shuukaTehaiData.SykymdDt.ToString("yyyy/MM/dd");
            shuukaTehaiData.SyuksuS = shuukaTehaiData.Syuksu.ToString();
            shuukaTehaiData.HososuS = shuukaTehaiData.Hososu.ToString();
            shuukaTehaiData.JyuryoS = shuukaTehaiData.Jyuryo.ToString();

            var shuukaTehaiSyuknoList = await dbContext.VUnsouShuukaTehai
            .Where(x => x.SYUKNO != Syukno)
            .Where(x => x.DAIHNO == Syukno)
            .Where(x => x.DAIHNOYM == Dataym)
            .Where(x => x.SYUTUF == taishoCd)
            .Select(x => new SyuknoData
            {
                Syukno1 = x.SYUKNO.Substring(0, 10)
            })
            .OrderBy(x => x.Syukno1)
            .ToListAsync();

            int count = 0;
            List<SyuknoData> SyuknoList = new List<SyuknoData>();
            SyuknoData SyuknoData = new SyuknoData();
            foreach (SyuknoData shuukaTehaiSyukno in shuukaTehaiSyuknoList)
            {
                count++;
                if (count == 1)
                {
                    SyuknoData = new SyuknoData();
                    SyuknoData.Syukno1 = shuukaTehaiSyukno.Syukno1;
                }
                else if (count == 2)
                {
                    SyuknoData.Syukno2 = shuukaTehaiSyukno.Syukno1;
                }
                else if (count == 3)
                {
                    SyuknoData.Syukno3 = shuukaTehaiSyukno.Syukno1;
                }
                else if (count == 4)
                {
                    SyuknoData.Syukno4 = shuukaTehaiSyukno.Syukno1;
                }
                else if (count == 5)
                {
                    SyuknoData.Syukno5 = shuukaTehaiSyukno.Syukno1;
                }
                else if (count == 6)
                {
                    SyuknoData.Syukno6 = shuukaTehaiSyukno.Syukno1;
                    SyuknoList.Add(SyuknoData);
                    count = 0;
                }
            }

            if (SyuknoData.Syukno1 != null )
            {
                SyuknoList.Add(SyuknoData);
            }

            shuukaTehaiData.SyuknoList = SyuknoList;

            return shuukaTehaiData;
        }
    }
}