using Macss.Areas.Tass.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Macss.Areas.Tass.ViewModels
{
    public class ShuukaTyuumonshoEntryViewModel
    {
        public ShuukaTyuumonshoEntryViewModel() : this(new TUnsouShuukaTyuumonshoTehai())
        {

        }

        public ShuukaTyuumonshoEntryViewModel(TUnsouShuukaTyuumonshoTehai enrty)
        {
            Model = enrty;
            if (enrty == null)
            {
                Model = new Models.TUnsouShuukaTyuumonshoTehai();
            }
        }

        [Display(Name = "出荷No")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(3, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE074")]
        [MinLength(3, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE074")]
        [RegularExpression(@"[A-Z0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE116")]
        public string SyuknoMae { get; set; }

        [Display(Name = "出荷No")]
        public string SyuknoAto { get; set; }

        [Display(Name = "出荷No")]
        public string Syukno { get; set; }

        [Display(Name = "出荷No")]    // 表示用
        public string VSyukno { get; set; }

        [Display(Name = "パターン")]
        [MaxLength(6, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        //[RegularExpression(@"[A-Z0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE116")]
        [RegularExpression(@"[a-zA-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE113")]
        public string Inscod { get; set; }

        [Display(Name = "部門")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(4, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Pccod1 { get; set; }

        [Display(Name = "部門名")]
        public string Pccod1n { get; set; }

        [Display(Name = "発注元")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110"), ]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Htynam { get; set; }

        [Display(Name = "課補・係長")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Htykah { get; set; }

        [Display(Name = "担当者")]
        [MaxLength(8, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tancod { get; set; }

        [Display(Name = "担当者名")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE133")]
        public string Tannam { get; set; }

        [Display(Name = "電話番号")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[0-9-()]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE114")]
        public string Htytel { get; set; }

        [Display(Name = "保管場所")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Basyo { get; set; }

        [Display(Name = "振替出荷No")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Fsykno { get; set; }

        [Display(Name = "得意先")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        public string Toknam { get; set; }

        [Display(Name = "出荷場所")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Sybcod { get; set; }

        [Display(Name = "出荷場所名")]
        public string Sybnam { get; set; }

        [Display(Name = "出荷日")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        public string Sykymd { get; set; }

        [Display(Name = "運賃負担")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(1, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Ufutan { get; set; }

        [Display(Name = "経費負担Ｎｏ")]
        [MaxLength(30, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Keifno { get; set; }

        [Display(Name = "届先コード")]
        [MaxLength(15, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Tdkcod { get; set; }
        
        [Display(Name = "届先郵便番号")]
        [RegularExpression(@"[0-9]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE115")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdkyub { get; set; }

        [Display(Name = "届先住所")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(60, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdkjyu { get; set; }

        [Display(Name = "届先社名")]
        [Required(ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE055")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]

        public string Tdknam { get; set; }

        [Display(Name = "届先支店名")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]

        public string Tdsnam { get; set; }

        [Display(Name = "届先部課名")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Tdbnam { get; set; }

        [Display(Name = "届先担当者名")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tdktan { get; set; }

        [Display(Name = "届先電話番号")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[0-9-()]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE114")]

        public string Tdktel { get; set; }

        [Display(Name = "品名")]
        public ShuukaTyuumonshoEntryMeisaiViewModel Meisai { get; set; }

        [Display(Name = "取込名")]
        public string Tricod { get; set; }

        [Display(Name = "取込名")]
        public string Trinam { get; set; }

        [Display(Name = "出荷数")]
        public string Dsyuksu { get; set; }

        [Display(Name = "特記事項１")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tkjiko1 { get; set; }

        [Display(Name = "特記事項２")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]
        public string Tkjiko2 { get; set; }

        [Display(Name = "特別指定コード")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Yusono { get; set; }

        [Display(Name = "コメント")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[^ -~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE110")]

        public string Coment { get; set; }

        [Display(Name = "機種A")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Kisyua { get; set; }

        [Display(Name = "機種B")]
        [MaxLength(6, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Kisyub { get; set; }

        [Display(Name = "運送方法")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Unscod { get; set; }

        [Display(Name = "運送方法")]
        public string Unscodnam { get; set; }

        [Display(Name = "コース")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Unscrs { get; set; }

        [Display(Name = "仕入先")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Sircod { get; set; }

        [Display(Name = "仕入先")]
        public string Sirnam { get; set; }

        [Display(Name = "運送区分")]
        [MaxLength(5, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+", ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE111")]
        public string Unskbn { get; set; }

        [Display(Name = "運送区分")]
        public string Unskbnnam { get; set; }

        [Display(Name = "郵便番号必須フラグ")]
        public bool Yubflg { get; set; }

        [Display(Name = "無効区分")]
        public bool Mukoukbn { get; set; }

        [Display(Name = "無効化理由")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Resources.Message), ErrorMessageResourceName = "CE056")]
        public string Mukouriyuu { get; set; }

        [Display(Name = "データ作成日(非表示)")]
        public string Cdate { get; set; }

        [Display(Name = "伝票出力フラグ(非表示 チェック用)")]
        public string Denf { get; set; }

        [Display(Name = "出荷日(非表示 変換用)")]
        public DateTime TSykymd { get; set; }

        [Display(Name = "出荷数(非表示 変換用)")]
        public decimal TDsyuksu { get; set; }

        [Display(Name = "品名コード１(非表示 変換用)")]
        public string Hincd1 { get; set; }

        [Display(Name = "品名コード２(非表示 変換用)")]
        public string Hincd2 { get; set; }

        [Display(Name = "品名コード３(非表示 変換用)")]
        public string Hincd3 { get; set; }

        [Display(Name = "品名コード４(非表示 変換用)")]
        public string Hincd4 { get; set; }

        [Display(Name = "品名コード５(非表示 変換用)")]
        public string Hincd5 { get; set; }

        [Display(Name = "品名１(非表示 変換用)")]
        public string Hinnm1 { get; set; }

        [Display(Name = "品名２(非表示 変換用)")]
        public string Hinnm2 { get; set; }

        [Display(Name = "品名３(非表示 変換用)")]
        public string Hinnm3 { get; set; }

        [Display(Name = "品名４(非表示 変換用)")]
        public string Hinnm4 { get; set; }

        [Display(Name = "品名５(非表示 変換用)")]
        public string Hinnm5 { get; set; }

        [Display(Name = "出荷数１(非表示 変換用)")]
        public decimal? Syksu1 { get; set; }

        [Display(Name = "出荷数２(非表示 変換用)")]
        public decimal? Syksu2 { get; set; }

        [Display(Name = "出荷数３(非表示 変換用)")]
        public decimal? Syksu3 { get; set; }

        [Display(Name = "出荷数４(非表示 変換用)")]
        public decimal? Syksu4 { get; set; }

        [Display(Name = "出荷数５(非表示 変換用)")]
        public decimal? Syksu5 { get; set; }

        [Display(Name = "操作モード")]
        public string Mode { get; set; }

        [Display(Name = "採番FLG")]
        public int NumberingFlg { get; set; }

        [Display(Name = "明細 保存用")]
        public string MeisaiJson { get; set; }

        [ScriptIgnore]
        public TUnsouShuukaTyuumonshoTehai Model { get; set; }

    }

    public class ShuukaTyuumonshoEntryMeisaiViewModel
    {

        [Display(Name = "Renban")]
        public int? Renban { get; set; }

        [Display(Name = "No")]
        public string No { get; set; }

        [Display(Name = "品名コード")]
        public string Hincod { get; set; }

        [Display(Name = "品名")]
        public string Hinnam { get; set; }

        [Display(Name = "数量")]
        public string Syuksu { get; set; }

        [Display(Name = "数量(非表示 変換用)")]
        public decimal? TSyuksu { get; set; }

    }

    public class ShuukaTyuumonshoEntryData
    {

        public EntrySerch Serch { get; set; }

        public EntryInformation Information { get; set; }

        [Display(Name = "操作モード")]
        public string Mode { get; set; }

    }

    public class EntrySerch
    {

        [Display(Name = "出荷No")]
        public string Syukno { get; set; }

        [Display(Name = "出荷場所コード")]
        public string Sybcod { get; set; }

        [Display(Name = "振替出荷No")]
        public string Fsykno { get; set; }

        [Display(Name = "登録担当者名")]
        public string Crtnam { get; set; }

        [Display(Name = "出荷日")]
        public string SykymdFrom { get; set; }
        public string SykymdTo { get; set; }

        [Display(Name = "得意先コード")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        public string Toknam { get; set; }

        [Display(Name = "届先コード")]
        public string Tdkcod { get; set; }

        [Display(Name = "届先社名")]
        public string Tdknam { get; set; }

        [Display(Name = "届先支店名")]
        public string Tdsnam { get; set; }

        [Display(Name = "届先部課名")]
        public string Tdbnam { get; set; }

        [Display(Name = "届先担当者名")]
        public string Tdktan { get; set; }

        [Display(Name = "未発行")]
        public bool Denfn { get; set; }

        [Display(Name = "発行済")]
        public bool Denfy { get; set; }

        [Display(Name = "照会グループ")]
        public string GroupCd { get; set; }

        [Display(Name = "検索FLG")]
        public int SearchFlg { get; set; }

    }

    public class EntryInformation
    {

        [Display(Name = "無")]
        public string Mukounam { get; set; }

        [Display(Name = "発")]
        public string Denf { get; set; }

        [Display(Name = "出荷日")]
        public string Sykymd { get; set; }

        [Display(Name = "場")]
        public string Sybcod { get; set; }

        [Display(Name = "出荷No")]
        public string Syukno { get; set; }

        [Display(Name = "届先名")]
        public string Tdknam { get; set; }

        [Display(Name = "品名")]
        public string Dhinnam { get; set; }

        [Display(Name = "得意先C")]
        public string Tokcod { get; set; }

        [Display(Name = "得意先名")]
        public string Toknam { get; set; }

        [Display(Name = "登録担当者")]
        public string Crtnam { get; set; }

        [Display(Name = "データ作成日(非表示)")]
        public string Cdate { get; set; }

        [Display(Name = "出荷日(変換用 非表示)")]
        public DateTime TSykymd { get; set; }

    }

}