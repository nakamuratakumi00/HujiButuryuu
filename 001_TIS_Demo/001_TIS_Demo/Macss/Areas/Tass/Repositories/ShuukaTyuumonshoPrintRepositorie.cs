using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Macss.App_Start;
using Macss.Models;
using Macss.Areas.Tass.Models;
using Macss.Areas.Tass.ViewModels;
using Macss.Repositories;
using Macss.Areas.Fdass.Common;
using System.Data;

namespace Macss.Areas.Tass.Repositories
{
    public class ShuukaTyuumonshoPrintRepositorie : IShuukaTyuumonshoPrintRepositorie
    {


        private readonly ApplicationDB dbContext;
        private IControlRepository controlRepository;

        public ShuukaTyuumonshoPrintRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
            controlRepository = new ControlRepository(dbContext);

        }
        //初期処理.グループ照会の退避
        public async Task<DispData> GetDispData(string LoginUser)
        {
            var statusList = await dbContext.MAccount
                      .Where(x => x.Id == LoginUser)
                      .Select(x => new DispData()
                      {
                          GroupCd = x.GroupCd,
                          Sybcod = x.BasyoCd
                      })
                      .ToListAsync();

            var statusData = statusList
                                .Select(x => new DispData()
                                {
                                    GroupCd = x.GroupCd,
                                    Sybcod = x.Sybcod
                                })
                                .FirstOrDefault();
            return statusData;
        }

        public async Task<IEnumerable<ShuukaTyuumonshoPrintInformation>> GetSearchAsync(ShuukaTyuumonshoPrintSerch Tyuumonshyo, string groupcd)
        {

            DateTime dtFrom = Tyuumonshyo.DateFrom == null ? DateTime.MinValue : DateTime.Parse(Tyuumonshyo.DateFrom);
            DateTime dtTo = Tyuumonshyo.DateTo == null ? DateTime.MaxValue : DateTime.Parse(Tyuumonshyo.DateTo);
            DateTime? printFrom = Tyuumonshyo.PrintFrom == null ? DateTime.MinValue : DateTime.Parse(Tyuumonshyo.PrintFrom);
            DateTime? printTo = Tyuumonshyo.PrintTo == null ? DateTime.MaxValue : DateTime.Parse(Tyuumonshyo.PrintTo + " 23:59:59");

            //加工用
            var sdata = await dbContext.TUnsouShuukaTyuumonshoTehai
                //出荷手配データ.出荷No, データ作成日
                .GroupJoin(dbContext.TUnsouShuukatehai,
                a => new { key1 = a.Syukno, key2 = a.Cdate },
                b => new { key1 = b.Syukno, key2 = b.Cdate }, 
                (a, b) => new { a, b })
                .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new { a.a, b })
                 //取引先マスタ.取引先コード
                 .GroupJoin(dbContext.MTorihikisaki, ab => ab.a.Tokcod, c => c.Torcod, (ab, c) => new { ab, c })
                 .SelectMany(x => x.c.DefaultIfEmpty(), (ab, c) => new { ab.ab, c })
                 //アカウントマスタ_発行者
                 .GroupJoin(dbContext.MAccount, abc => abc.ab.a.Hkucod, d => d.Id, (abc, d) => new { abc, d })
                 .SelectMany(x => x.d.DefaultIfEmpty(), (abc, d) => new { abc.abc, d })
                 //アカウントマスタ_登録担当者名
                 .GroupJoin(dbContext.MAccount, abcd => abcd.abc.ab.a.Crtcod, e => e.Id, (abcd, e) => new { abcd, e })
                 .SelectMany(x => x.e.DefaultIfEmpty(), (abcd, e) => new { abcd.abcd, e })
                // 出荷注文書手配データ.無効区分 = 0(有効)
                .Where(x => x.abcd.abc.ab.a.Mukoukbn == "0")
                ////画面抽出条件
                .Where(x => x.abcd.abc.ab.a.Sykymd >= dtFrom)
                .Where(x => x.abcd.abc.ab.a.Sykymd <= dtTo)
                .Where(x => Tyuumonshyo.PrintFrom == null ? true : x.abcd.abc.ab.a.Hkuymd >= printFrom)
                .Where(x => Tyuumonshyo.PrintTo == null ? true : x.abcd.abc.ab.a.Hkuymd <= printTo)
                //.Where(x => (Tyuumonshyo.Mihakkou == false & Tyuumonshyo.Hakousumi == true) ? (x.abcd.abc.ab.a.Denf == "A" | x.abcd.abc.ab.a.Denf == "S") : true)
                //.Where(x => (Tyuumonshyo.Mihakkou == true & Tyuumonshyo.Hakousumi == false) ? (x.abcd.abc.ab.a.Denf == null | x.abcd.abc.ab.a.Denf == string.Empty) : true)
                .Where(x => (Tyuumonshyo.Hkukbn == "1" ) ? (x.abcd.abc.ab.a.Denf == "A" | x.abcd.abc.ab.a.Denf == "S") : true)
                .Where(x => (Tyuumonshyo.Hkukbn == "0" ) ? (x.abcd.abc.ab.a.Denf == null | x.abcd.abc.ab.a.Denf == string.Empty) : true)
                .Where(x => Tyuumonshyo.Sybcod == null ? true : x.abcd.abc.ab.a.Sybcod == Tyuumonshyo.Sybcod)
                .Where(x => Tyuumonshyo.Tannam == null ? true : x.e.UserName.StartsWith(Tyuumonshyo.Tannam))
                .Where(x => Tyuumonshyo.Syukno == null ? true : x.abcd.abc.ab.a.Syukno.StartsWith(Tyuumonshyo.Syukno))
                .Where(x => Tyuumonshyo.Tokcod == null ? true : x.abcd.abc.ab.a.Tokcod.StartsWith(Tyuumonshyo.Tokcod))
                .Where(x => Tyuumonshyo.UserName == null ? true : x.abcd.d.UserName.StartsWith(Tyuumonshyo.UserName))

                ////紹介グループによる条件分岐
                .Where(x => groupcd == "G000" ? true : x.abcd.abc.c.Seco06 == groupcd)
                .Select(x => new ShuukaTyuumonshoPrintInformation()
                {
                    Hstatus = x.abcd.abc.ab.a.Denf == "S"? "再" : x.abcd.abc.ab.a.Denf == "A " ? "済" : "未",
                    THkuymd = x.abcd.abc.ab.a.Hkuymd == null ? DateTime.MinValue : x.abcd.abc.ab.a.Hkuymd.Value,
                    Hkuymd = string.Empty,
                    Hknam = x.abcd.d.UserName,
                    TSyukdt = x.abcd.abc.ab.a.Sykymd.Value,
                    Syukdt = string.Empty,
                    Sybcod = x.abcd.abc.ab.a.Sybcod,
                    Syukno = x.abcd.abc.ab.a.Syukno,
                    Cdate = x.abcd.abc.ab.a.Cdate,
                    TTdkjyu = x.abcd.abc.ab.a.Tdkjyu.Trim(),
                    Todonam = x.abcd.abc.ab.a.Tdknam + " " + x.abcd.abc.ab.a.Tdsnam + " " + x.abcd.abc.ab.a.Tdbnam + " " + x.abcd.abc.ab.a.Tdktan,
                    Dhinnam = x.abcd.abc.ab.a.Dhinnam,
                    Tokcod = x.abcd.abc.ab.a.Tokcod,
                    Tornam = x.abcd.abc.c.Tornam,
                    Updcod = x.e.UserName
                }).ToListAsync();
            var statusList2 = sdata
                        .Select(x => new ShuukaTyuumonshoPrintInformation()
                        {
                            Hstatus = x.Hstatus,
                            Hkuymd = x.THkuymd == DateTime.MinValue ? string.Empty : x.THkuymd.ToString("yyyy/MM/dd HH:mm:ss"),
                            Hknam = x.Hknam,
                            Syukdt = x.TSyukdt.ToString("yyyy/MM/dd"),
                            Sybcod = x.Sybcod,
                            Syukno = Common.DataUtil.GetSyukno(x.Syukno),
                            Cdate = x.Cdate,
                            Todonam = x.Todonam,
                            Dhinnam = x.Dhinnam,
                            Tokcod = x.Tokcod,
                            Tornam = x.Tornam,
                            Updcod = x.Updcod
                        })
                        .OrderBy(x => x.Syukno);
                        //.OrderByDescending(x => x.Hkuymd)
                        //.ThenBy(x => x.Syukno);

            //名称の変換&&文字列調整
            List<ShuukaTyuumonshoPrintInformation> result = new List<ShuukaTyuumonshoPrintInformation>();
            foreach (var sData in statusList2)
            {

                //if (sData.Todonam != null)
                //{
                //    sData.Todonam = sData.Todonam.Length >= 20 ? sData.Todonam.Substring(0, 20) : sData.Todonam;
                //}

                if (sData.Tornam != null)
                {
                    sData.Tornam = sData.Tornam.Length >= 20 ? sData.Tornam.Substring(0, 20) : sData.Tornam;
                }

                result.Add(sData);
            }
            return result;
        }

        public async Task UPDShuukaTyuumonsho(string[] sNo, string[] sDt, string loginUser)
        {
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                int cnt = 0;
                foreach (var Syukno in sNo)
                {
                    string cksts = string.Empty;
                    string cdt = sDt[cnt];

                    var statusList = await dbContext.TUnsouShuukaTyuumonshoTehai
                                         .Where(x => x.Syukno == Syukno)
                                         .Where(x => x.Cdate == cdt).ToListAsync();

                    var status = statusList.First();
                    //得意先コードをキーに、取引先マスタの取引先コードを検索し、
                    //該当する「請求先コード１」を設定する
                    var mtorihiki = await dbContext.MTorihikisaki
                        .Where(x => x.Torcod == status.Tokcod).ToListAsync();

                    if (statusList.Count() != 0)
                    {
                        if (mtorihiki.Count() != 0)
                        {
                            var torihiki = mtorihiki.First();
                            status.Seicod = torihiki.Seco01;
                        }
                        else
                        {
                            status.Seicod = status.Seicod;
                        }
                        cksts = status.Denf != string.Empty ? "S" : "A";
                        //status.Denf = status.Denf == string.Empty ? "A" : "S";
                        status.Hkucod = loginUser;
                        status.Hkuymd = DateTime.Now;
                        //status.Updcod = loginUser;
                        //status.Updymd = DateTime.Now;
                        await dbContext.SaveChangesAsync();
                    }

                    // 運賃明細書発行区分を取引先マスタより取得
                    string meikbn = string.Empty;
                    var uKbnList = await dbContext.MTorihikisaki
                                    .Where(x => x.Torcod == status.Tokcod).ToListAsync();
                    if(uKbnList.Count != 0)
                    {
                        var uKbn = uKbnList.First();
                        meikbn = uKbn.Mehk01;
                    }
                    else
                    {
                        meikbn = string.Empty;
                    }
                    //県コードを郵便番号マスタの分割住所1を検索し、検索された分割住所1の県名をキーにコントロールマスタの県コードを設定 Kencod
                    string kencod = string.Empty;
                    var kenList  = await dbContext.MUnsouPostalcode
                                    .Where(x => status.Tdkjyu.StartsWith(x.Kenjy1)).ToListAsync();
                    if(kenList.Count() != 0)
                    {
                        var kencod2 = kenList.First();
                        var ckenList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.KenCod))
                                            .Where(x => x.Value1 == kencod2.Jyusy1).FirstOrDefault();
                        kencod = ckenList.Kbn;
                    }

                    // 分割住所1,2,3
                    bool notF = false;
                    string Tdbnj1 = "";
                    string Tdbnj2 = "";
                    string Tdbnj3 = "";
                    if (status.Tdkyub != null)
                    {
                        var yubinlist = await dbContext.MUnsouPostalcode
                                    .Where(x => status.Tdkyub == x.Yubinn).ToListAsync();
                        if (yubinlist.Count() != 0)
                        {
                            //var yubin = yubinlist.First();
                            //Tdbnj1 = yubin.Jyusy1;
                            //Tdbnj2 = yubin.Jyusy2;
                            notF = true;
                        }
                    }
                    //if (!notF)
                    //{
                    // 届先住所から県を取得
                    
                    if (!string.IsNullOrEmpty(status.Tdkjyu))
                    {
                        var stTdkjyu = status.Tdkjyu.Replace("　", "");
                        var ckenList = (await controlRepository.GetDataBySectionAsync(ControlRepository.MControlSection.KenCod));
                            foreach (var ken in ckenList)
                            {
                                if (stTdkjyu.Length > ken.Value1.Length)
                                {
                                    if (stTdkjyu.Substring(0, ken.Value1.Length) == ken.Value1)
                                    {
                                        Tdbnj1 = ken.Value1;
                                        string sTdbnj2 = stTdkjyu.Substring(ken.Value1.Length);
                                        if (sTdbnj2.Length > 20)
                                        {
                                            Tdbnj2 = sTdbnj2.Substring(0, 20);
                                            Tdbnj3 = sTdbnj2.Substring(20);
                                        } else
                                        {
                                            Tdbnj2 = sTdbnj2;
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    //}

                    //郵便番号マスタを取得し、設定
                    string jiscod = string.Empty;
                    //if (!string.IsNullOrEmpty(status.Tdkyub))
                    if (notF)
                    {
                        var yubinlist = await dbContext.MUnsouPostalcode
                            .Where(x => status.Tdkyub == x.Yubinn).ToListAsync();
                        if(yubinlist.Count() != 0)
                        {
                            var yubin = yubinlist.First();
                            jiscod = yubin.Jiscod;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(Tdbnj1) && !string.IsNullOrEmpty(Tdbnj2))
                        {
                            var jisdata = await dbContext.MUnsouJiscode.Where(x => x.Jyusy1 == Tdbnj1).ToListAsync();
                            foreach (var jis in jisdata)
                            {
                                if (Tdbnj2.Length > jis.Jyusy2.Length)
                                {
                                    if (Tdbnj2.Substring(0, jis.Jyusy2.Length) == jis.Jyusy2)
                                    {
                                        jiscod = jis.Jiscod;
                                        break;
                                    }
                                }
                            }
                        }

                        //var yubin2list = await dbContext.MUnsouPostalcode
                        //    .Where(x => status.Tdkyub == x.Yubinn).ToListAsync();
                        //if(yubin2list.Count() != 0)
                        //{
                        //    var yubin2 = yubin2list.First();
                        //    var jisdata = await dbContext.MUnsouJiscode
                        //        .Where(x => x.Jyusy1 == yubin2.Kenjy1 & x.Jyusy2 == yubin2.Kenjy2).FirstAsync();
                        //    jiscod = jisdata.Jiscod;
                        //}
                    }

                    // 出荷手配データ 更新(未発行)
                    if (status.Denf != "A" && status.Denf != "S")
                    {
                        cksts = "A";
                        status.Denf = "A";
                    }
                    else if (status.Denf == "A" || status.Denf == "S")
                    {
                        cksts = "S";
                        status.Denf = "S";
                    }
                    if (cksts == "A")
                    {
                        var statusList2 = await dbContext.TUnsouShuukatehai
                                                    .Where(x => x.Syukno == Syukno)
                                                    .Where(x => x.Cdate == cdt)
                                                    .ToListAsync();
                        if (statusList2.Count != 0)
                        {
                            //更新
                            var updata = statusList2.First();
                            updata.Syukno = updata.Syukno;
                            updata.Cdate = updata.Cdate;
                            updata.Sykymd = status.Sykymd;
                            updata.Kisyu = status.Kisyu;
                            updata.Keifno = status.Keifno;
                            updata.Fsykno = status.Fsykno;
                            updata.Tancod = status.Tancod;
                            updata.Tannam = status.Tannam;
                            updata.Tdkcod = status.Tdkcod;
                            updata.Tdkyub = status.Tdkyub;
                            updata.Tdkjyu = status.Tdkjyu;
                            updata.Tdknam = status.Tdknam;
                            updata.Tdsnam = status.Tdsnam;
                            updata.Tdbnam = status.Tdbnam;
                            updata.Tdktan = status.Tdktan;
                            updata.Tdktel = status.Tdktel;
                            updata.Hincod = status.Dhincod;
                            updata.Hinnam = status.Dhinnam;
                            updata.Unscod = status.Unscod;
                            updata.Unscrs = status.Unscrs;
                            updata.Sircod = status.Sircod;
                            updata.Unskbn = status.Unskbn;
                            updata.Sybcod = status.Sybcod;
                            updata.Tokcod = status.Tokcod;
                            updata.Seicod = status.Seicod;
                            updata.Denkbn = status.Denkbn;
                            updata.Denmsu = status.Denmsu;
                            updata.Tkjiko = status.Tkjiko;
                            updata.Ufutan = status.Ufutan;
                            updata.Yusono = status.Yusono;
                            updata.Denf = cksts;
                            updata.Ctlf19 = "A";
                            updata.Mehkbn = meikbn;
                            updata.Pccod = status.Pccod;
                            updata.Kencod = kencod;
                            updata.Sikcod = "0000";
                            updata.Tdbnj1 = Tdbnj1;
                            updata.Tdbnj2 = Tdbnj2;
                            updata.Tdbnj3 = Tdbnj3;
                            updata.Tdkjyu = status.Tdkjyu;
                            updata.Jiscod = jiscod;
                            updata.Tensir = string.Empty;
                            updata.Syuksu = status.Dsyuksu;
                            updata.Mkcod = status.Mukoukbn;
                            updata.Mkriyu = string.Empty;
                            updata.Yubflg = status.Yubflg;
                            updata.Crtcod = status.Crtcod;
                            updata.Crtymd = status.Crtymd;
                            updata.Updcod = loginUser;
                            updata.Updymd = DateTime.Now;
                            await dbContext.SaveChangesAsyncEx();
                        }
                        else
                        {
                            //新規作成
                            var crtdata = new TUnsouShuukatehai()
                            {
                                Syukno = status.Syukno,
                                Cdate = status.Cdate,
                                Sykymd = status.Sykymd,
                                Kisyu = status.Kisyu,
                                Keifno = status.Keifno,
                                Fsykno = status.Fsykno,
                                Tancod = status.Tancod,
                                Tannam = status.Tannam,
                                Tdkcod = status.Tdkcod,
                                Tdkyub = status.Tdkyub,
                                Tdkjyu = status.Tdkjyu,
                                Tdknam = status.Tdknam,
                                Tdsnam = status.Tdsnam,
                                Tdbnam = status.Tdbnam,
                                Tdktan = status.Tdktan,
                                Tdktel = status.Tdktel,
                                Hincod = status.Dhincod,
                                Hinnam = status.Dhinnam,
                                Unscod = status.Unscod,
                                Unscrs = status.Unscrs,
                                Sircod = status.Sircod,
                                Unskbn = status.Unskbn,
                                Sybcod = status.Sybcod,
                                Tokcod = status.Tokcod,
                                Seicod = status.Seicod,
                                Denkbn = status.Denkbn,
                                Denmsu = status.Denmsu,
                                Tkjiko = status.Tkjiko,
                                Ufutan = status.Ufutan,
                                Yusono = status.Yusono,
                                Denf = cksts,
                                Ctlf19 = "A",
                                Mehkbn = meikbn,
                                Pccod = status.Pccod,
                                Kencod = kencod,
                                Sikcod = "0000",
                                Tdbnj1 = Tdbnj1,
                                Tdbnj2 = Tdbnj2,
                                Tdbnj3 = Tdbnj3,
                                Jiscod = jiscod,
                                Tensir = string.Empty,
                                Syuksu = status.Dsyuksu,
                                Mkcod = status.Mukoukbn,
                                Mkriyu = string.Empty,
                                Yubflg = status.Yubflg,
                                Crtcod = status.Crtcod,
                                Crtymd = status.Crtymd,
                                Updcod = loginUser,
                                Updymd = DateTime.Now
                            };
                            // 追加
                            dbContext.TUnsouShuukatehai.Add(crtdata);
                            await dbContext.SaveChangesAsyncEx();
                        }
                    }
                    else
                    {
                        //更新
                        var statusList3 = await dbContext.TUnsouShuukatehai
                            .Where(x => x.Syukno == Syukno)
                            .Where(x => x.Cdate == cdt)
                            .ToListAsync();

                        var updata2 = statusList3.First();
                        updata2.Syukno = status.Syukno;
                        updata2.Cdate = updata2.Cdate;
                        updata2.Sykymd = status.Sykymd;
                        updata2.Kisyu = status.Kisyu;
                        updata2.Keifno = status.Keifno;
                        updata2.Fsykno = status.Fsykno;
                        updata2.Tancod = status.Tancod;
                        updata2.Tannam = status.Tannam;
                        updata2.Tdkcod = status.Tdkcod;
                        updata2.Tdkyub = status.Tdkyub;
                        updata2.Tdkjyu = status.Tdkjyu;
                        updata2.Tdknam = status.Tdknam;
                        updata2.Tdsnam = status.Tdsnam;
                        updata2.Tdbnam = status.Tdbnam;
                        updata2.Tdktan = status.Tdktan;
                        updata2.Tdktel = status.Tdktel;
                        updata2.Hincod = status.Dhincod;
                        updata2.Hinnam = status.Dhinnam;
                        updata2.Unscod = status.Unscod;
                        updata2.Unscrs = status.Unscrs;
                        updata2.Sircod = status.Sircod;
                        updata2.Unskbn = status.Unskbn;
                        updata2.Sybcod = status.Sybcod;
                        updata2.Tokcod = status.Tokcod;
                        updata2.Seicod = status.Seicod;
                        updata2.Denkbn = status.Denkbn;
                        updata2.Denmsu = status.Denmsu;
                        updata2.Tkjiko = status.Tkjiko;
                        updata2.Ufutan = status.Ufutan;
                        updata2.Yusono = status.Yusono;
                        updata2.Denf = cksts;
                        updata2.Ctlf19 = "A";
                        updata2.Mehkbn = meikbn;
                        updata2.Pccod = status.Pccod;
                        updata2.Kencod = kencod;
                        updata2.Sikcod = "0000";
                        updata2.Tdbnj1 = Tdbnj1;
                        updata2.Tdbnj2 = Tdbnj2;
                        updata2.Tdbnj3 = Tdbnj3;
                        updata2.Tdkjyu = status.Tdkjyu;
                        updata2.Jiscod = jiscod;
                        updata2.Tensir = string.Empty;
                        updata2.Syuksu = status.Dsyuksu;
                        updata2.Mkcod = status.Mukoukbn;
                        updata2.Mkriyu = string.Empty;
                        updata2.Yubflg = status.Yubflg;
                        updata2.Crtcod = status.Crtcod;
                        updata2.Crtymd = status.Crtymd;
                        updata2.Updcod = loginUser;
                        updata2.Updymd = DateTime.Now;

                        await dbContext.SaveChangesAsyncEx();
                    }
                    cnt += 1;
                }
                transaction.Commit();
            }
        }

        //帳票出力
        public async Task<IEnumerable<ShuukaTyuumonshoPrintData>> ShuukaTyuumonshoPrint(string[] sNo, string[] sDt, string loginUser)
        {
            List<ShuukaTyuumonshoPrintData> result = new List<ShuukaTyuumonshoPrintData>();

            // データソート
            DataTable sData = new DataTable();
            sData.Columns.Add("No");
            sData.Columns.Add("Date");
            int idx = 0;
            foreach (var Syukno in sNo)
            {
                DataRow row = sData.NewRow();
                row["No"] = Syukno;
                row["Date"] = sDt[idx];
                sData.Rows.Add(row);
                idx++;
            }
            DataRow[] dRows = sData.Select("", "No");

            int dtcnt = 0;
            //foreach (var Syukno in sNo)
            foreach (var dRow in dRows)
            {
                //string cdt = sDt[dtcnt];
                string Syukno = dRow["No"].ToString();
                string cdt = dRow["Date"].ToString();

                //出火場所手配データ(加工用)
                var shuukadata = await dbContext.TUnsouShuukaTyuumonshoTehai
                                        .Where(x => x.Syukno == Syukno)
                                        .Where(x => x.Cdate == cdt)
                                        .Select(x => new ShuukaTyuumonshoPrintData
                                        {
                                            Denf = x.Denf == "S" ? "S" : string.Empty,
                                            THkuymd = x.Hkuymd.Value,
                                            Htynam = x.Htynam,
                                            Htykah = x.Htykah,
                                            Tannam = x.Tannam,
                                            Htytel = x.Htytel,
                                            Syukno = x.Syukno,
                                            Bacode = x.Syukno,
                                            Basyo = x.Basyo,
                                            Ufutan = x.Ufutan,
                                            //Ufutanmei = x.c.Value1,
                                            Keifno = x.Keifno,
                                            Fsykno = x.Fsykno,
                                            TSykymd = x.Sykymd.Value,
                                            Tdkyub = x.Tdkyub,
                                            //Tdkyub = x.Tdkyub.Substring(0,3) + "-" + x.Tdkyub.Substring(3),
                                            Tdktel = x.Tdktel,
                                            Tdkcod = x.Tdkcod,
                                            TTdkjyu = x.Tdkjyu,
                                            TTdknam = x.Tdknam + " " + x.Tdsnam + " " + x.Tdbnam + " " + x.Tdktan,
                                            TTkjiko = x.Tkjiko,
                                            Coment = x.Coment,
                                            Tokcod = x.Tokcod,
                                            Sybcod = x.Sybcod,
                                            Unscod = x.Unscod,
                                            Unscrs = x.Unscrs
                                        }).FirstAsync();

                //加工(日付と文字列長さ)
                shuukadata.TTkjiko = shuukadata.TTkjiko.Replace(" ", "");
                if (!string.IsNullOrEmpty(shuukadata.Tdkyub))
                {
                    shuukadata.Tdkyub = shuukadata.Tdkyub.Length == 7 ? shuukadata.Tdkyub.Substring(0, 3) + "-" + shuukadata.Tdkyub.Substring(3) : shuukadata.Tdkyub;
                } else
                {
                    shuukadata.Tdkyub = string.Empty;
                }
                shuukadata.Syukno = Common.DataUtil.GetSyukno(shuukadata.Syukno);
                shuukadata.Hkuymd = shuukadata.THkuymd.ToString("yyyy/MM/dd HH:mm:ss");
                shuukadata.Sykymd = shuukadata.TSykymd.ToString("yyyy-MM-dd");
                shuukadata.Tdkjyu1 = shuukadata.TTdkjyu.Length >= 30 ? shuukadata.TTdkjyu.Substring(0, 30) : shuukadata.TTdkjyu;
                shuukadata.Tdkjyu2 = shuukadata.TTdkjyu.Length >= 30 ? shuukadata.TTdkjyu.Substring(30) : string.Empty;
                shuukadata.Tdknam1 = shuukadata.TTdknam.Length >= 32 ? shuukadata.TTdknam.Substring(0, 32) : shuukadata.TTdknam + "　様";
                shuukadata.Tdknam2 = shuukadata.TTdknam.Length >= 64 ? shuukadata.TTdknam.Substring(32, 32) :
                                                    shuukadata.TTdknam.Length >= 32 ? shuukadata.TTdknam.Substring(32) + "　様" : string.Empty;
                shuukadata.Tdknam3 = shuukadata.TTdknam.Length >= 64 ? shuukadata.TTdknam.Substring(64) + "　様" : string.Empty;
                shuukadata.Tkjiko1 = shuukadata.TTkjiko.Length >= 20 ? shuukadata.TTkjiko.Substring(0, 20) : shuukadata.TTkjiko;
                shuukadata.Tkjiko2 = shuukadata.TTkjiko.Length >= 20 ? shuukadata.TTkjiko.Substring(20) : string.Empty;

                var mControls = await dbContext.MControl
                        .Where(x => x.Kbn == shuukadata.Ufutan && x.Section == ControlRepository.MControlSection.UFutanExtraction).ToListAsync();
                if (mControls.Count() != 0)
                {
                    var mControl = mControls.First();
                    shuukadata.Ufutanmei = mControl.Value1;
                }

                // 請求先マスタより発送者データを取得
                var sedata = await dbContext.MSeikyusaki
                        .Where(x => x.Seicod == shuukadata.Sybcod).ToListAsync();
                if (sedata.Count() != 0)
                {
                    var seidata = sedata.First();
                    shuukadata.Hsatukai = seidata.Seinam;
                    shuukadata.hsyubno = seidata.Yubno.Substring(0, 3) + "-" + seidata.Yubno.Substring(3);
                    shuukadata.hstel = seidata.Telno;
                    shuukadata.hsjyusyo = seidata.Jysyo;
                }
                else
                {
                    var sdata2 = await dbContext.MSeikyusaki
                                            .Where(x => x.Seicod == "B0").FirstAsync();
                    shuukadata.Hsatukai = sdata2.Seinam;
                    shuukadata.hsyubno = sdata2.Yubno.Substring(0, 3) +"-" + sdata2.Yubno.Substring(3);
                    shuukadata.hstel = sdata2.Telno;
                    shuukadata.hsjyusyo = sdata2.Jysyo;
                }
                // 出荷注文書明細データ取得
                List<TUnsouShuukaTyuumonshoTehaiMeisai> misaidata = await dbContext.TUnsouShuukaTyuumonshoTehaiMeisai
                                            .Where(x => x.Syukno == Syukno)
                                            .Where(x => x.Cdate == cdt)
                                            .OrderBy(x => x.Renban)
                                            .ToListAsync();

                int no = 1;
                List<TUnsouShuukaTyuumonshoTehaiMeisai> meidata = new List<TUnsouShuukaTyuumonshoTehaiMeisai>();
                foreach (var mei in misaidata)
                {
                    meidata.Add(new TUnsouShuukaTyuumonshoTehaiMeisai()
                    {
                        Renban = no,
                        Syukno = mei.Syukno,
                        Cdate = mei.Cdate,
                        Hincod = mei.Hincod,
                        Hinnam = mei.Hinnam,
                        Syuksu = mei.Syuksu
                    });
                    no++;
                }

                if (shuukadata.Coment != null && shuukadata.Coment.Length != 0)
                {
                    int? max = meidata.Max(x => x.Renban);
                    if (max != null) {
                        max = max + 5 - max % 5;
                    } else
                    {
                        max = 5;
                    }
                    TUnsouShuukaTyuumonshoTehaiMeisai meisai = new TUnsouShuukaTyuumonshoTehaiMeisai();
                    meisai.Renban = max;
                    meisai.Hincod = "(コメント)";
                    meisai.Hinnam = shuukadata.Coment;
                    meidata.Add(meisai);
                }

                //出荷頁数取得
                var pgmax = Convert.ToInt32(Math.Ceiling((double)meidata.Count() / 5));

                //明細が複数ページの場合
                if (pgmax > 1)
                {
                    var updata = new ShuukaTyuumonshoPrintData();

                    var pgcnt = 1;
                    var cnt = 0;

                    foreach (var mdata in meidata)
                    {
                        if (cnt == 0)
                        {
                            if (mdata.Renban - cnt == 1)
                            {
                                shuukadata.Hincod1 = mdata.Hincod;
                                shuukadata.Hinnam1 = mdata.Hinnam;
                                shuukadata.Syuksu1 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                continue;
                            }
                            if (mdata.Renban - cnt == 2)
                            {
                                shuukadata.Hincod2 = mdata.Hincod;
                                shuukadata.Hinnam2 = mdata.Hinnam;
                                shuukadata.Syuksu2 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                continue;
                            }
                            if (mdata.Renban - cnt == 3)
                            {
                                shuukadata.Hincod3 = mdata.Hincod;
                                shuukadata.Hinnam3 = mdata.Hinnam;
                                shuukadata.Syuksu3 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                continue;
                            }
                            if (mdata.Renban - cnt == 4)
                            {
                                shuukadata.Hincod4 = mdata.Hincod;
                                shuukadata.Hinnam4 = mdata.Hinnam;
                                shuukadata.Syuksu4 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                continue;
                            }
                            if (mdata.Renban - cnt == 5)
                            {
                                shuukadata.Hincod5 = mdata.Hincod;
                                shuukadata.Hinnam5 = mdata.Hinnam;
                                shuukadata.Syuksu5 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                cnt += 5;
                            }
                            shuukadata.Edaban = "-" + pgcnt + "/" + pgmax;
                            result.Add(shuukadata); // 複数ページ出力の１ページ目（５明細全埋）
                            pgcnt += 1;
                            updata = SetPrintData(shuukadata);
                        }
                        else
                        {
                            if (mdata.Renban - cnt == 1)
                            {
                                updata.Hincod1 = mdata.Hincod;
                                updata.Hinnam1 = mdata.Hinnam;
                                updata.Syuksu1 = mdata.Syuksu == 0 ? null : mdata.Syuksu;

                                continue;
                            }
                            if (mdata.Renban - cnt == 2)
                            {
                                updata.Hincod2 = mdata.Hincod;
                                updata.Hinnam2 = mdata.Hinnam;
                                updata.Syuksu2 = mdata.Syuksu == 0 ? null : mdata.Syuksu;

                                continue;
                            }
                            if (mdata.Renban - cnt == 3)
                            {
                                updata.Hincod3 = mdata.Hincod;
                                updata.Hinnam3 = mdata.Hinnam;
                                updata.Syuksu3 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                continue;
                            }
                            if (mdata.Renban - cnt == 4)
                            {
                                updata.Hincod4 = mdata.Hincod;
                                updata.Hinnam4 = mdata.Hinnam;
                                updata.Syuksu4 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                continue;
                            }
                            if (mdata.Renban - cnt == 5)
                            {
                                updata.Hincod5 = mdata.Hincod;
                                updata.Hinnam5 = mdata.Hinnam;
                                updata.Syuksu5 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                                cnt += 5;
                            }
                            updata.Edaban = "-" + pgcnt + "/" + pgmax;
                            result.Add(updata); // 複数ページ出力の２ページ目以降（５明細全埋）
                            pgcnt += 1;
                            updata = SetPrintData(shuukadata);
                        }
                    }
                    if (cnt == 0)
                    {
                        //shuukadata.Edaban = string.Empty;
                        shuukadata.Edaban = "-" + pgcnt + "/" + pgmax;
                        result.Add(shuukadata); // 複数ページ出力の１ページ目（５明細未満）
                    }
                    else if (pgcnt == pgmax)
                    {
                        //updata.Edaban = string.Empty;
                        updata.Edaban = "-" + pgcnt + "/" + pgmax;
                        result.Add(updata); // 複数ページ出力の２ページ目以降（５明細未満）
                    }
                }

                //明細が１ページの場合
                else
                {
                    foreach (var mdata in meidata)
                    {

                        if (mdata.Renban == 1)
                        {
                            shuukadata.Hincod1 = mdata.Hincod;
                            shuukadata.Hinnam1 = mdata.Hinnam;
                            shuukadata.Syuksu1 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                            continue;
                        }
                        if (mdata.Renban == 2)
                        {
                            shuukadata.Hincod2 = mdata.Hincod;
                            shuukadata.Hinnam2 = mdata.Hinnam;
                            shuukadata.Syuksu2 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                            continue;
                        }
                        if (mdata.Renban == 3)
                        {
                            shuukadata.Hincod3 = mdata.Hincod;
                            shuukadata.Hinnam3 = mdata.Hinnam;
                            shuukadata.Syuksu3 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                            continue;
                        }
                        if (mdata.Renban == 4)
                        {
                            shuukadata.Hincod4 = mdata.Hincod;
                            shuukadata.Hinnam4 = mdata.Hinnam;
                            shuukadata.Syuksu4 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                            continue;
                        }
                        if (mdata.Renban == 5)
                        {
                            shuukadata.Hincod5 = mdata.Hincod;
                            shuukadata.Hinnam5 = mdata.Hinnam;
                            shuukadata.Syuksu5 = mdata.Syuksu == 0 ? null : mdata.Syuksu;
                        }
                    }
                    result.Add(shuukadata); // １ページ出力

                }
                dtcnt += 1;
            }

            return result;
        }
        private ShuukaTyuumonshoPrintData SetPrintData(ShuukaTyuumonshoPrintData shuukadata)
        {
            var updata = new ShuukaTyuumonshoPrintData();

            #region shuukadataコピー
            updata.Denf = shuukadata.Denf;
            updata.Htynam = shuukadata.Htynam;
            updata.Htykah = shuukadata.Htykah;
            updata.Tannam = shuukadata.Tannam;
            updata.Htytel = shuukadata.Htytel;
            updata.Syukno = shuukadata.Syukno;
            updata.Bacode = shuukadata.Bacode;
            updata.Basyo = shuukadata.Basyo;
            updata.Ufutan = shuukadata.Ufutan;
            updata.Ufutanmei = shuukadata.Ufutanmei;
            updata.Keifno = shuukadata.Keifno;
            updata.Fsykno = shuukadata.Fsykno;
            updata.Tdkyub = shuukadata.Tdkyub;
            updata.Tdktel = shuukadata.Tdktel;
            updata.Tdkcod = shuukadata.Tdkcod;
            updata.Tokcod = shuukadata.Tokcod;
            updata.Sybcod = shuukadata.Sybcod;
            updata.Unscod = shuukadata.Unscod;
            updata.Unscrs = shuukadata.Unscrs;
            updata.Hsatukai = shuukadata.Hsatukai;
            updata.hsyubno = shuukadata.hsyubno;
            updata.hstel = shuukadata.hstel;
            updata.hsjyusyo = shuukadata.hsjyusyo;
            updata.Hkuymd = shuukadata.Hkuymd;
            updata.Sykymd = shuukadata.Sykymd;
            updata.Tdkjyu1 = shuukadata.Tdkjyu1;
            updata.Tdkjyu2 = shuukadata.Tdkjyu2;
            updata.Tdknam1 = shuukadata.Tdknam1;
            updata.Tdknam2 = shuukadata.Tdknam2;
            updata.Tdknam3 = shuukadata.Tdknam3;
            updata.Tkjiko1 = shuukadata.Tkjiko1;
            updata.Tkjiko2 = shuukadata.Tkjiko2;
            #endregion

            return updata;
        }
    }
}