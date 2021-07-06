using Macss.Attributes.Custom;
using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_shuuka_tyuumonsho_pattern")]
    public class MUnsouShuukaTyuumonshoPattern
    {

        [Description("出荷Ｎｏ(3桁)")]
        [Column("SYKNO2", Order = 0)]
        [Required]
        [FileRequired]
        [Key]
        [MaxLength(3)]
        [RegularExpression(@"[A-Z0-9]+")]
        public string Sykno2 { get; set; }

        [Description("パターンコード")]
        [Column("INSCOD", Order = 1)]
        [Required]
        [FileRequired]
        [Key]
        [MaxLength(6)]
        [RegularExpression(@"[A-Z0-9]+")]
        public string Inscod { get; set; }

        [Description("配布先名")]
        [Column("HAINAM")]
        [FileRequired]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hainam { get; set; }

        [Description("配布枚数")]
        [Column("MAISUU")]
        [FileRequired]
        public int? Maisuu { get; set; }

        [Description("出荷Ｎｏ印刷フラグ")]
        [Column("NOPRTF")]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Noprtf { get; set; }

        [Description("有効期限印刷フラグ")]
        [Column("KIGENF")]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Kigenf { get; set; }

        [Description("発注元会社名")]
        [Column("HTYNAM")]
        [MaxLength(20)]
        [FileRequired]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Htynam { get; set; }

        [Description("課補係長")]
        [Column("KTYKHO")]
        [MaxLength(10)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Ktykho { get; set; }

        [Description("担当者コード")]
        [Column("TANCOD")]
        [MaxLength(8)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tancod { get; set; }

        [Description("担当者")]
        [Column("TANNAM")]
        [MaxLength(10)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tannam { get; set; }

        [Description("発注元ＴＥＬ")]
        [Column("KTYTEL")]
        [MaxLength(20)]
        [RegularExpression(@"[0-9-() ]+")]
        public string Ktytel { get; set; }

        [Description("現品保管場所")]
        [Column("BASYO")]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Basyo { get; set; }

        [Description("運賃負担")]
        [Column("UFUTAN")]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Ufutan { get; set; }

        [Description("経費負担Ｎｏ")]
        [Column("KEIFNO")]
        [MaxLength(30)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Keifno { get; set; }

        [Description("届先コード")]
        [Column("TDKCOD")]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tdkcod { get; set; }

        [Description("届先電話番号")]
        [Column("TDKTEL")]
        [MaxLength(20)]
        [RegularExpression(@"[0-9-() ]+")]
        public string Tdktel { get; set; }

        [Description("届先住所")]
        [Column("JDKJYU")]
        [MaxLength(60)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Jdkjyu { get; set; }

        [Description("届先社名")]
        [Column("TDKNAM")]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdknam { get; set; }

        [Description("届先支店名")]
        [Column("TDSNAM")]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdsnam { get; set; }

        [Description("届先部課名")]
        [Column("TDBNAM")]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdbnam { get; set; }

        [Description("届先担当者名")]
        [Column("TDKTAN")]
        [MaxLength(20)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tdktan { get; set; }

        [Description("品名コード１")]
        [Column("HINCD1")]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hincd1 { get; set; }

        [Description("品名コード２")]
        [Column("HINCD2")]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hincd2 { get; set; }

        [Description("品名コード３")]
        [Column("HINCD3")]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hincd3 { get; set; }

        [Description("品名コード４")]
        [Column("HINCD4")]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hincd4 { get; set; }

        [Description("品名コード５")]
        [Column("HINCD5")]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hincd5 { get; set; }

        [Description("品名１")]
        [Column("HINNM1")]
        [MaxLength(80)]
        [FileMaxLength(26)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hinnm1 { get; set; }

        [Description("品名２")]
        [Column("HINNM2")]
        [MaxLength(80)]
        [FileMaxLength(26)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hinnm2 { get; set; }

        [Description("品名３")]
        [Column("HINNM3")]
        [MaxLength(80)]
        [FileMaxLength(26)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hinnm3 { get; set; }

        [Description("品名４")]
        [Column("HINNM4")]
        [MaxLength(80)]
        [FileMaxLength(26)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hinnm4 { get; set; }

        [Description("品名５")]
        [Column("HINNM5")]
        [MaxLength(80)]
        [FileMaxLength(26)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hinnm5 { get; set; }

        [Description("出荷数１")]
        [Column("SYKSU1")]
        [FileRequired]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syksu1 { get; set; }

        [Description("出荷数２")]
        [Column("SYKSU2")]
        [FileRequired]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syksu2 { get; set; }

        [Description("出荷数３")]
        [Column("SYKSU3")]
        [FileRequired]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syksu3 { get; set; }

        [Description("出荷数４")]
        [Column("SYKSU4")]
        [FileRequired]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syksu4 { get; set; }

        [Description("出荷数５")]
        [Column("SYKSU5")]
        [FileRequired]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syksu5 { get; set; }

        [Description("特記事項")]
        [Column("TKJIKO")]
        [MaxLength(40)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Tkjiko { get; set; }

        [Description("得意先コード")]
        [Column("TOKCOD")]
        [MaxLength(9)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tokcod { get; set; }

        [Description("運送方法コード")]
        [Column("UNSCOD")]
        [MaxLength(4)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Unscod { get; set; }

        [Description("運送コースコード")]
        [Column("UNSCRS")]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Unscrs { get; set; }

        [Description("出荷場所コード")]
        [Column("SYBCOD")]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sybcod { get; set; }

        [Description("届先郵便番号")]
        [Column("TDKYUB")]
        [MaxLength(8)]
        [RegularExpression(@"[0-9 ]+")]
        public string Tdkyub { get; set; }

        [Description("伝票区分")]
        [Column("DENKBN")]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Denkbn { get; set; }

        [Description("帳票出力フラグ")]
        [Column("LSTFLG")]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Lstflg { get; set; }

        [Description("届先マスタ使用フラグ")]
        [Column("TDKHEN")]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tdkhen { get; set; }

        [Description("登録依頼元")]
        [Column("IRIMOT")]
        [MaxLength(13)]
        [FileRequired]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Irimot { get; set; }

        [Description("登録依頼元ＴＥＬ")]
        [Column("IRITEL")]
        [MaxLength(20)]
        [RegularExpression(@"[0-9-() ]+")]
        public string Iritel { get; set; }

        [Description("登録担当")]
        [Column("CRTCOD")]
        [MaxLength(8)]
        public string Crtcod { get; set; }

        [Description("登録日")]
        [Column("CRTYMD")]
        public DateTime? Crtymd { get; set; }

        [Description("更新担当")]
        [Column("UPDCOD")]
        [MaxLength(8)]
        public string Updcod { get; set; }

        [Description("更新日")]
        [Column("UPDYMD")]
        public DateTime? Updymd { get; set; }

    }

}