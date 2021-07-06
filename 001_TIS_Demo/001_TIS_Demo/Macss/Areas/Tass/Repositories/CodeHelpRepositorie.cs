using Macss.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static Macss.Areas.Tass.ViewModels.CodeHelpViewModels;

namespace Macss.Areas.Tass.Repositories
{
    public class CodeHelpRepositorie : ICodeHelpRepositorie
    {

        private readonly ApplicationDB dbContext;


        public CodeHelpRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
        }

        //得意先
        public async Task<IEnumerable<TokuisakiInformation>> GetTokuisakiAsync(TokuisakiSerch tokuisaki)
        {

            List<TokuisakiInformation> tokuisakiList = await dbContext.MTorihikisaki
                                                    .Where(x => x.Lstflg == "0" | x.Lstflg == "2")
                                                    .Where(x => tokuisaki.Tornmk == null ? true : x.Tornmk.Contains(tokuisaki.Tornmk))
                                                    .Where(x => tokuisaki.Torcod == null ? true : x.Torcod.StartsWith(tokuisaki.Torcod))
                                                    .Where(x => tokuisaki.Fbtcod == null ? true : x.Fbtcod.StartsWith(tokuisaki.Fbtcod))
                                                    .Where(x => tokuisaki.Tornam == null ? true : x.Tornam.Contains(tokuisaki.Tornam))
                                                    .Where(x => tokuisaki.Buknam == null ? true : x.Buknam.Contains(tokuisaki.Buknam))
                                                    .Where(x => tokuisaki.Jysyo == null ? true : x.Jysyo.Contains(tokuisaki.Jysyo))
                                                    .Select(x => new TokuisakiInformation()
                                                    {
                                                        Tornmk = x.Tornmk,
                                                        Torcod = x.Torcod,
                                                        Fbtcod = x.Fbtcod,
                                                        Tornam = x.Tornam,
                                                        Buknam = x.Buknam,
                                                        Jysyo = x.Jysyo
                                                    })
                                                    .OrderBy(x => x.Tornmk)
                                                    .ThenBy(x => x.Tornam)
                                                    .ThenBy(x => x.Buknam)
                                                    .ThenBy(x => x.Torcod)
                                                    .ToListAsync();

            return tokuisakiList;
        }

        //品名
        public async Task<IEnumerable<HinmeisakiInformation>> GetHinmeiAsync(HinmeisakiSerch hinmei)
        {
            Models.VUnsouHinmei vUnsouHinmei = new Models.VUnsouHinmei();

            List<HinmeisakiInformation> HinmeiList = await vUnsouHinmei.v_unsou_hinmei
                                                     .Where(x => hinmei.Hinnmk == null ? true : x.HINNMK.Contains(hinmei.Hinnmk))
                                                     .Where(x => hinmei.Hincod == null ? true : x.HINCOD.StartsWith(hinmei.Hincod))
                                                     .Where(x => hinmei.Hinnam == null ? true : x.HINNAM.Contains(hinmei.Hinnam))
                                                     .Where(x => hinmei.Ctlfl1 == null ? true : x.CTLFL1 == hinmei.Ctlfl1)
                                                     .Where(x => hinmei.Khincd == null ? true : x.KHINCD.StartsWith(hinmei.Khincd))
                                                     .Where(x => x.DELFLG != "1")
                                                     .Select(x => new HinmeisakiInformation()
                                                     {
                                                         Hinnmk = x.HINNMK,
                                                         Hincod = x.HINCOD,
                                                         Hinnam = x.HINNAM,
                                                         Ctlfl1 = x.CTLFL1
                                                     })
                                                     .OrderBy(x => x.Hinnmk)
                                                     .ThenBy(x => x.Hinnam)
                                                     .ToListAsync();

            var HinmeiList2 = HinmeiList
                .Select(x => new HinmeisakiInformation()
                {
                    Hinnmk = x.Hinnmk == null ? string.Empty : HttpUtility.HtmlEncode(x.Hinnmk.Trim()),
                    Hincod = x.Hincod == null ? string.Empty : x.Hincod.Trim(),
                    Hinnam = x.Hinnam == null ? string.Empty : x.Hinnam.Trim(),
                    Ctlfl1 = x.Ctlfl1 == null ? string.Empty : x.Ctlfl1.Trim()
                });

            return HinmeiList2;
        }

        //届先
        public async Task<IEnumerable<TodokesakihelpInformation>> GetTodokesakiAsync(TodokesakihelpSerch todokesaki)
        {
            Models.VUnsouTodokesaki vUnsouTodokesaki = new Models.VUnsouTodokesaki();

            List<TodokesakihelpInformation> statusList = await vUnsouTodokesaki.v_unsou_todokesaki
                                                     .Where(x => todokesaki.Tdkcod == null ? true : x.TDKCOD.StartsWith(todokesaki.Tdkcod))
                                                     .Where(x => todokesaki.Tdknmk == null ? true : x.TDKNMK.Contains(todokesaki.Tdknmk))
                                                     .Where(x => todokesaki.Tdknam == null ? true : x.TDKNAM.Contains(todokesaki.Tdknam))
                                                     .Where(x => todokesaki.Tdknms == null ? true : x.TDKNMS.Contains(todokesaki.Tdknms))
                                                     .Where(x => todokesaki.Tdbnam == null ? true : x.TDBNAM.Contains(todokesaki.Tdbnam))
                                                     .Where(x => todokesaki.Ktdkcd == null ? true : x.KTDKCD.StartsWith(todokesaki.Ktdkcd))
                                                     .Where(x => todokesaki.Jyusyo == null ? true : x.JYUSYO.Contains(todokesaki.Jyusyo))
                                                     .Where(x => todokesaki.Sdek01 == null ? true : x.SDEK01 == todokesaki.Sdek01)
                                                     .Where(x => todokesaki.Sdek02 == null ? true : x.SDEK02 == todokesaki.Sdek02)
                                                     .Where(x => todokesaki.Sdek03 == null ? true : x.SDEK03 == todokesaki.Sdek03)
                                                     .Where(x => todokesaki.Sdek04 == null ? true : x.SDEK04 == todokesaki.Sdek04)
                                                     .Where(x => todokesaki.Sdek05 == null ? true : x.SDEK05 == todokesaki.Sdek05)
                                                     .Where(x => todokesaki.Sdek06 == null ? true : x.SDEK06 == todokesaki.Sdek06)
                                                     .Where(x => todokesaki.Sdek07 == null ? true : x.SDEK07 == todokesaki.Sdek07)
                                                     .Where(x => todokesaki.Sdek08 == null ? true : x.SDEK08 == todokesaki.Sdek08)
                                                     .Where(x => todokesaki.Sdek09 == null ? true : x.SDEK09 == todokesaki.Sdek09)
                                                     .Where(x => todokesaki.Sdek10 == null ? true : x.SDEK10 == todokesaki.Sdek10)
                                                     .Where(x => todokesaki.Sdek11 == null ? true : x.SDEK11 == todokesaki.Sdek11)
                                                     .Where(x => todokesaki.Sdek12 == null ? true : x.SDEK12 == todokesaki.Sdek12)
                                                     .Where(x => todokesaki.Sdek13 == null ? true : x.SDEK13 == todokesaki.Sdek13)
                                                     .Where(x => todokesaki.Sdek14 == null ? true : x.SDEK14 == todokesaki.Sdek14)
                                                     .Where(x => todokesaki.Sdek15 == null ? true : x.SDEK15 == todokesaki.Sdek15)
                                                     .Where(x => x.DELFLG != "1")
                                                     .Select(x => new TodokesakihelpInformation()
                                                     {
                                                         Tdknmk = x.TDKNMK,
                                                         Tdkcod = x.TDKCOD,
                                                         Tdknam = x.TDKNAM,
                                                         Tdknms = x.TDKNMS,
                                                         Tdbnam = x.TDBNAM,
                                                         Jyusyo = x.JYUSYO,
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
                                                         Sdek15 = x.SDEK15
                                                     })
                                                     .OrderBy(x => x.Tdknmk)
                                                     .ThenBy(x => x.Tdknam)
                                                     .ThenBy(x => x.Tdknms)
                                                     .ThenBy(x => x.Tdbnam)
                                                     .ThenBy(x => x.Tdkcod)
                                                     .ToListAsync();

            return statusList;

        }

        //仕入先
        public async Task<IEnumerable<SiiresakiInformation>> GetSiiresakiAsync(SiiresakiSerch siiresaki)
        {
            List<SiiresakiInformation> SiiresakiList = await dbContext.MTorihikisaki
                                                .Where(x => x.Lstflg == "0" | x.Lstflg == "1")
                                                .Where(x => siiresaki.TornmkSi == null ? true : x.Tornmk.Contains(siiresaki.TornmkSi))
                                                .Where(x => siiresaki.TorcodSi == null ? true : x.Torcod.StartsWith(siiresaki.TorcodSi))
                                                .Where(x => siiresaki.FbtcodSi == null ? true : x.Fbtcds.StartsWith(siiresaki.FbtcodSi))
                                                .Where(x => siiresaki.TornamSi == null ? true : x.Tornam.Contains(siiresaki.TornamSi))
                                                .Where(x => siiresaki.BuknamSi == null ? true : x.Buknam.Contains(siiresaki.BuknamSi))
                                                .Where(x => siiresaki.JysyoSi == null ? true : x.Jysyo.Contains(siiresaki.JysyoSi))
                                                .Select(x => new SiiresakiInformation()
                                                {
                                                    TornmkSi = x.Tornmk,
                                                    TorcodSi = x.Torcod,
                                                    FbtcodSi = x.Fbtcds,
                                                    TornamSi = x.Tornam,
                                                    BuknamSi = x.Buknam,
                                                    JysyoSi = x.Jysyo
                                                })
                                                .OrderBy(x => x.TornmkSi)
                                                .ThenBy(x => x.TornamSi)
                                                //.OrderBy(x => x.BuknamSi)
                                                .ToListAsync();

            return SiiresakiList;

        }


        //郵便番号
        public async Task<IEnumerable<YubinbangoInformation>> GetYubinbangoAsync(YubinbangoSerch yubinbango)
        {
            List<YubinbangoInformation> YubinbangoList = await dbContext.MUnsouPostalcode
                                    .Where(x => yubinbango.Jyusy1 == null ? true : x.Jyusy1.Contains(yubinbango.Jyusy1))
                                    .Where(x => yubinbango.Jyusy2 == null ? true : x.Jyusy2.Contains(yubinbango.Jyusy2))
                                    .Where(x => yubinbango.Yubinn == null ? true : x.Yubinn.StartsWith(yubinbango.Yubinn))
                                    .Select(x => new YubinbangoInformation()
                                    {
                                        Jyusy1 = x.Jyusy1,
                                        Jyusy2 = x.Jyusy2,
                                        Yubinn = x.Yubinn
                                    })
                                    .OrderBy(x => x.Yubinn)
                                    //.OrderBy(x => x.Jyusy1)
                                    //.ThenBy(x => x.Jyusy2)
                                    .ToListAsync();
            return YubinbangoList;

        }

        //運送方法
        public async  Task<IEnumerable<UnsohohoInformation>> GetUnsohohoAsync(UnsohohoSerch unsohoho)
        {
            List<UnsohohoInformation> UnsohohoList = await dbContext.MUnsouUnsouhouhou
                            .Where(x => unsohoho.Unscod == null ? true : x.Unscod.StartsWith(unsohoho.Unscod))
                            .Where(x => unsohoho.Unsnam == null ? true : x.Unsnam.Contains(unsohoho.Unsnam))
                            .Select(x => new UnsohohoInformation()
                            {
                                Unscod = x.Unscod,
                                Unsnam = x.Unsnam
                            })
                            .OrderBy(x => x.Unscod)
                            .ToListAsync();

            return UnsohohoList;

        }

        //出荷場所
        public async Task<IEnumerable<ShukkabashoInformation>> GetShukkabashoAsync(ShukkabashoSerch shukkabasho)
        {

            List<ShukkabashoInformation> shukkabashoList = await dbContext.MShukkabasho
                                                    .Where(x => shukkabasho.SybcodCh == null ? true : x.Sybcod.StartsWith(shukkabasho.SybcodCh))
                                                    .Where(x => shukkabasho.SybnamCh == null ? true : x.Sybnam.Contains(shukkabasho.SybnamCh))
                                                    .Select(x => new ShukkabashoInformation()
                                                    {
                                                        SybcodCh = x.Sybcod,
                                                        SybnamCh = x.Sybnam
                                                    })
                                                    .OrderBy(x => x.SybcodCh)
                                                    .ToListAsync();

            return shukkabashoList;
        }

        //運送区分
        public async Task<IEnumerable<UnsokbnInformation>> GetUnsokbnAsync(UnsokbnSerch unsokbn)
        {
            List<UnsokbnInformation> UnsokbnList = await dbContext.MUnsouUnsoukubun
                                        .Where(x => unsokbn.Unskbn == null ? true : x.Unskbn.StartsWith(unsokbn.Unskbn))
                                        .Where(x => unsokbn.Unsknam == null ? true : x.Unsnam.Contains(unsokbn.Unsknam))
                                        .Select(x => new UnsokbnInformation()
                                        {
                                            Unskbn = x.Unskbn,
                                            Unsknam = x.Unsnam
                                        })
                                        .OrderBy(x => x.Unskbn)
                                        .ToListAsync();

            return UnsokbnList;

        }

        //届先
        public async Task<IEnumerable<TyuumonshoPatternInformation>> GetPatternAsync(string SyuknoMae)
        {

            List<TyuumonshoPatternInformation> statusList = await dbContext.MUnsouShuukaTyuumonshoPattern
                                                     .Where(x => x.Sykno2 == SyuknoMae)
                                                     .Select(x => new TyuumonshoPatternInformation()
                                                     {
                                                         Sykno2 = x.Sykno2,
                                                         Inscod = x.Inscod,
                                                         Hainam = x.Hainam,
                                                         Maisuu = x.Maisuu,
                                                         Noprtf = x.Noprtf,
                                                         Kigenf = x.Kigenf,
                                                         Htynam = x.Htynam,
                                                         Ktykho = x.Ktykho,
                                                         Tancod = x.Tancod,
                                                         Tannam = x.Tannam,
                                                         Ktytel = x.Ktytel,
                                                         Basyo = x.Basyo,
                                                         Ufutan = x.Ufutan,
                                                         Keifno = x.Keifno,
                                                         Tdkcod = x.Tdkcod,
                                                         Tdktel = x.Tdktel,
                                                         Jdkjyu = x.Jdkjyu,
                                                         Tdknam = x.Tdknam,
                                                         Tdsnam = x.Tdsnam,
                                                         Tdbnam = x.Tdbnam,
                                                         Tdktan = x.Tdktan,
                                                         Hincd1 = x.Hincd1,
                                                         Hincd2 = x.Hincd2,
                                                         Hincd3 = x.Hincd3,
                                                         Hincd4 = x.Hincd4,
                                                         Hincd5 = x.Hincd5,
                                                         Hinnm1 = x.Hinnm1,
                                                         Hinnm2 = x.Hinnm2,
                                                         Hinnm3 = x.Hinnm3,
                                                         Hinnm4 = x.Hinnm4,
                                                         Hinnm5 = x.Hinnm5,
                                                         Syksu1 = x.Syksu1,
                                                         Syksu2 = x.Syksu2,
                                                         Syksu3 = x.Syksu3,
                                                         Syksu4 = x.Syksu4,
                                                         Syksu5 = x.Syksu5,
                                                         Tkjiko = x.Tkjiko,
                                                         Tokcod = x.Tokcod,
                                                         Unscod = x.Unscod,
                                                         Unscrs = x.Unscrs,
                                                         Sybcod = x.Sybcod,
                                                         Tdkyub = x.Tdkyub,
                                                         Denkbn = x.Denkbn,
                                                         Lstflg = x.Lstflg,
                                                         Tdkhen = x.Tdkhen,
                                                         Irimot = x.Irimot,
                                                         Iritel = x.Iritel,
                                                         Crtcod = x.Crtcod,
                                                         Crtymd = x.Crtymd,
                                                         Updcod = x.Updcod,
                                                         Updymd = x.Updymd

                                                     })
                                                     .OrderBy(x => x.Sykno2)
                                                     .ThenBy(x => x.Inscod)
                                                     .ToListAsync();

            return statusList;

        }

    }
}