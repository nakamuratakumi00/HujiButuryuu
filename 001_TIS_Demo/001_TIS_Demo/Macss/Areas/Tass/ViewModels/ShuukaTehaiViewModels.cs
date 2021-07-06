using Macss.Areas.Tass.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.Areas.Tass.ViewModels
{
    public class ShuukaTehaiViewModels
    {

        public ShuukaTehaiViewModels() : this(new v_unsou_shuuka_tehai())
        {
        }

        public ShuukaTehaiViewModels(v_unsou_shuuka_tehai enrty)
        {
            Model = enrty;
            if (enrty == null)
            {
                Model = new v_unsou_shuuka_tehai();
            }
        }

        [Display(Name = "出荷No")]
        public string Syukno { get; set; }

        [Display(Name = "伝票区分")]
        public string Denkbn { get; set; }

        [Display(Name = "到着日")]
        public string Tocymd { get; set; }
        public DateTime TocymdDt { get; set; }

        [Display(Name = "出力対象")]
        public string Syutuf { get; set; }

        [Display(Name = "振替出荷No")]
        public string Fsykno { get; set; }

        [Display(Name = "得意先")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        public string Tokcnm { get; set; }

        [Display(Name = "出荷日")]
        public string Sykymd { get; set; }
        public DateTime SykymdDt { get; set; }

        [Display(Name = "運送方法")]
        public string Unscod { get; set; }

        [Display(Name = "運送方法名")]
        public string Unscnm { get; set; }

        [Display(Name = "運送コース")]
        public string Unscrs { get; set; }

        [Display(Name = "仕入先")]
        public string Sircod { get; set; }

        [Display(Name = "仕入先名")]
        public string Sircnm { get; set; }

        [Display(Name = "運送区分")]
        public string Unskbn { get; set; }

        [Display(Name = "運送区分名")]
        public string Unskbnnm { get; set; }

        [Display(Name = "出荷場所")]
        public string Sybcod { get; set; }

        [Display(Name = "出荷場所名")]
        public string Sybcnm { get; set; }

        [Display(Name = "運賃負担")]
        public string Ufutan { get; set; }

        [Display(Name = "経費負担No")]
        public string Keifno { get; set; }

        [Display(Name = "機種A")]
        public string Kisyua { get; set; }

        [Display(Name = "機種B")]
        public string Kisyub { get; set; }

        [Display(Name = "担当者")]
        public string Tancod { get; set; }

        [Display(Name = "担当者名")]
        public string Tannam { get; set; }

        [Display(Name = "荷主")]
        public string Ninusi { get; set; }

        [Display(Name = "届先コード")]
        public string Tdkcod { get; set; }

        [Display(Name = "郵便番号")]
        public string Tdkyub { get; set; }

        [Display(Name = "電話番号")]
        public string Tdktel { get; set; }

        [Display(Name = "住所")]
        public string Tdkjyu { get; set; }

        [Display(Name = "社名")]
        public string Tdknam { get; set; }

        [Display(Name = "支店名")]
        public string Tdsnam { get; set; }

        [Display(Name = "部課名")]
        public string Tdbnam { get; set; }

        [Display(Name = "担当者名")]
        public string Tdktan { get; set; }

        [Display(Name = "品名")]
        public string Hincod { get; set; }

        [Display(Name = "品名")]
        public string Hinnam { get; set; }

        [Display(Name = "出荷数")]
        public decimal Syuksu { get; set; }
        public string SyuksuS { get; set; }

        [Display(Name = "特記事項１")]
        public string Tkjiko1 { get; set; }

        [Display(Name = "特記事項２")]
        public string Tkjiko2 { get; set; }

        [Display(Name = "特別指定コード")]
        public string Yusono { get; set; }

        [Display(Name = "詰合せ代表出荷No")]
        public string Daihno { get; set; }

        [Display(Name = "包装数")]
        public decimal Hososu { get; set; }
        public string HososuS { get; set; }

        [Display(Name = "重量")]
        public decimal Jyuryo { get; set; }
        public string JyuryoS { get; set; }

        [Display(Name = "サイズ")]
        public string Taksiz { get; set; }

        [Display(Name = "仕入先原票No")]
        public string Sgen35 { get; set; }

        [Display(Name = "出荷括りNo")]
        public string Sgenno { get; set; }

        [Display(Name = "出荷統合コード")]
        public string Stoucd { get; set; }

        [Display(Name = "仕分コード")]
        public string Hatcod { get; set; }

        [Display(Name = "JISコード")]
        public string Jiscod { get; set; }

        [Display(Name = "FB発行仕入先原票No")]
        public string Sgenn2 { get; set; }
        
        [Display(Name = "PCコード")]
        public string Pccod { get; set; }

        [Display(Name = "明細区分")]
        public string Mehkbn { get; set; }

        [Display(Name = "振替得意先コード")]
        public string Tensir { get; set; }

        [Display(Name = "エネ外")]
        public string Ecoflg { get; set; }

        public string Delflg { get; set; }

        public List<SyuknoData> SyuknoList { get; set; }

        [ScriptIgnore]
        public v_unsou_shuuka_tehai Model { get; set; }

    }

    public class SyuknoData
    {
        [Display(Name = "出荷No")]
        public string Syukno1 { get; set; }
        public string Syukno2 { get; set; }
        public string Syukno3 { get; set; }
        public string Syukno4 { get; set; }
        public string Syukno5 { get; set; }
        public string Syukno6 { get; set; }
    }

    public class ShuukaTehaiData
    {
        public ShuukaTehaiSerch Serch { get; set; }
        public ShuukaTehaiInformation Information { get; set; }
    }

    public class ShuukaTehaiSerch
    {

        [Display(Name = "出荷No")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Syukno { get; set; }

        [Display(Name = "出荷場所コード")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Sybcod { get; set; }

        [Display(Name = "登録担当者名")]
        //[RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Crtnam { get; set; }

        [Display(Name = "出荷日")]
        public string SykymdFrom { get; set; }
        public string SykymdTo { get; set; }

        [Display(Name = "振替出荷No")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Fsykno { get; set; }

        [Display(Name = "経費負担No")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Keifno { get; set; }

        [Display(Name = "得意先コード")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        //[RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Toknam { get; set; }

        [Display(Name = "届先コード")]
        //[RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tdkcod { get; set; }

        [Display(Name = "届先社名")]
        //[RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdknam { get; set; }

        [Display(Name = "届先支店名")]
        //[RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdsnam { get; set; }

        [Display(Name = "届先部課名")]
        //[RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdbnam { get; set; }

        [Display(Name = "届先担当者名")]
        //[RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdktan { get; set; }

        [Display(Name = "顧客届先コード")]
        public string Ktdkcd { get; set; }

        [Display(Name = "出力条件1")]
        public bool Jyoken1 { get; set; }

        [Display(Name = "出力条件2")]
        public bool Jyoken2 { get; set; }

        [Display(Name = "出力条件3")]
        public bool Jyoken3 { get; set; }

        [Display(Name = "出荷手配")]
        public bool Tehai { get; set; }

        [Display(Name = "出荷実績（当月）")]
        public bool Tehai1 { get; set; }

        [Display(Name = "出荷実績（過去）")]
        public bool Tehai2 { get; set; }

        [Display(Name = "出荷注文書手配エントリで登録したものに絞る")]
        public bool Entry { get; set; }

        public string Group { get; set; }

        public int DetaileReturnFlg { get; set; }

        [Display(Name = "削除データを含めない")]
        public bool Dell1 { get; set; }

        [Display(Name = "削除データを含める")]
        public bool Dell2 { get; set; }

        public string Dell { get; set; }

    }

    public class ShuukaTehaiInformation
    {

        [Display(Name = "対")]
        public string Taisho { get; set; }

        [Display(Name = "出荷日")]
        public string Sykymd { get; set; }
        public DateTime SykymdDt { get; set; }

        [Display(Name = "出荷No")]
        public string Syukno { get; set; }

        [Display(Name = "経費負担No")]
        public string Keifno { get; set; }

        [Display(Name = "振替出荷No")]
        public string Fsykno { get; set; }

        [Display(Name = "届先名")]
        public string Tdknam { get; set; }

        [Display(Name = "届先支店名")]
        public string Tdsnam { get; set; }

        [Display(Name = "届先部課名")]
        public string Tdbnam { get; set; }

        [Display(Name = "届先担当者名")]
        public string Tdktan { get; set; }

        [Display(Name = "品名")]
        public string Dhinnam { get; set; }

        [Display(Name = "場")]
        public string Sybcod { get; set; }

        [Display(Name = "得意先C")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        public string Toknam { get; set; }

        [Display(Name = "データ年月")]
        public string Dataym { get; set; }

        public string Felflg { get; set; }

        public string Tdkcod { get; set; }

    }

    public class UnsouShuukaTehaiAll
    {

        public string Syukno { get; set; }

        public string Dataym { get; set; }

        public DateTime? Sykymd { get; set; }

        public string Kisyu { get; set; }

        public string Keifno { get; set; }

        public string Fsykno { get; set; }

        public string Tancod { get; set; }

        public string Tannam { get; set; }

        public string Tdkcod { get; set; }

        public string Tdkjyu { get; set; }

        public string Tdknam { get; set; }

        public string Tdsnam { get; set; }

        public string Tdbnam { get; set; }

        public string Tdktan { get; set; }

        public string Tdktel { get; set; }

        public string Tdkyub { get; set; }

        public string Hincod { get; set; }

        public string Hinnam { get; set; }

        public string Unscod { get; set; }

        public string Unscrs { get; set; }

        public string Sircod { get; set; }

        public string Sgenno { get; set; }

        public string Unskbn { get; set; }

        public string Sybcod { get; set; }

        public string Tokcod { get; set; }

        public string Seicod { get; set; }

        public string Denkbn { get; set; }

        public int? Denmsu { get; set; }

        public string Tkjiko { get; set; }

        public decimal? Hososu { get; set; }

        public DateTime? Nfdate { get; set; }

        public string Daihno { get; set; }

        public string Daihnoym { get; set; }

        public decimal? Jyuryo { get; set; }

        public decimal? Hosos3 { get; set; }

        public decimal? Jyury3 { get; set; }

        public string Ufutan { get; set; }

        public string Yusono { get; set; }

        public string Delflg { get; set; }

        public string Ctlf19 { get; set; }

        public string Ctlf28 { get; set; }

        public string Ctlf29 { get; set; }

        public string Mehkbn { get; set; }

        public string Jskkbn { get; set; }

        [Required]
        public DateTime? Tocymd { get; set; }

        public string Taksiz { get; set; }

        public string Seikyu { get; set; }

        public string Geppou { get; set; }

        public string Pccod { get; set; }

        public string Sgenn2 { get; set; }

        public string Stoucd { get; set; }

        public string Kencod { get; set; }

        public string Jiscod { get; set; }

        public string Tensir { get; set; }

        public string Hatcod { get; set; }

        public string Sgen35 { get; set; }

        public string Ecoflg { get; set; }

        public decimal? Syuksu { get; set; }

        public string Syutuf { get; set; }

        public string Crtcod { get; set; }

        public DateTime? Crtymd { get; set; }

        public string Updcod { get; set; }

        public DateTime? Updymd { get; set; }

    }

}