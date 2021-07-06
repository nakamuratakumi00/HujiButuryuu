using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("t_unsou_shuukatehai")]
    public class TUnsouShuukatehai
    {

        [Description("出荷Ｎｏ")]
        [Column("SYUKNO", Order = 0)]
        [Required]
        [Key]
        [MaxLength(20)]
        public string Syukno { get; set; }

        [Description("データ作成日")]
        [Column("CDATE", Order = 1)]
        [Required]
        [Key]
        [MaxLength(6)]
        public string Cdate { get; set; }

        [Description("出荷日")]
        [Column("SYKYMD")]
        [Required]
        public DateTime? Sykymd { get; set; }

        [Description("Ｆｅ機種")]
        [Column("KISYU")]
        [Required]
        [MaxLength(8)]
        public string Kisyu { get; set; }

        [Description("経費負担Ｎｏ")]
        [Column("KEIFNO")]
        [Required]
        [MaxLength(30)]
        public string Keifno { get; set; }

        [Description("振替出荷Ｎｏ")]
        [Column("FSYKNO")]
        [Required]
        [MaxLength(20)]
        public string Fsykno { get; set; }

        [Description("担当者コード")]
        [Column("TANCOD")]
        [Required]
        [MaxLength(8)]
        public string Tancod { get; set; }

        [Description("担当者名")]
        [Column("TANNAM")]
        [Required]
        [MaxLength(10)]
        public string Tannam { get; set; }

        [Description("届先コード")]
        [Column("TDKCOD")]
        [Required]
        [MaxLength(15)]
        public string Tdkcod { get; set; }

        [Description("届先郵便番号")]
        [Column("TDKYUB")]
        [Required]
        [MaxLength(10)]
        public string Tdkyub { get; set; }

        [Description("届先住所")]
        [Column("TDKJYU")]
        [Required]
        [MaxLength(60)]
        public string Tdkjyu { get; set; }

        [Description("届先社名")]
        [Column("TDKNAM")]
        [Required]
        [MaxLength(20)]
        public string Tdknam { get; set; }

        [Description("届先支店名")]
        [Column("TDSNAM")]
        [Required]
        [MaxLength(20)]
        public string Tdsnam { get; set; }

        [Description("届先部課名")]
        [Column("TDBNAM")]
        [Required]
        [MaxLength(20)]
        public string Tdbnam { get; set; }

        [Description("届先担当者名")]
        [Column("TDKTAN")]
        [Required]
        [MaxLength(20)]
        public string Tdktan { get; set; }

        [Description("届先電話番号")]
        [Column("TDKTEL")]
        [Required]
        [MaxLength(20)]
        public string Tdktel { get; set; }

        [Description("品名コード")]
        [Column("HINCOD")]
        [Required]
        [MaxLength(15)]
        public string Hincod { get; set; }

        [Description("品名")]
        [Column("HINNAM")]
        [Required]
        [MaxLength(80)]
        public string Hinnam { get; set; }

        [Description("運送方法コード")]
        [Column("UNSCOD")]
        [Required]
        [MaxLength(2)]
        public string Unscod { get; set; }

        [Description("運送コースコード")]
        [Column("UNSCRS")]
        [Required]
        [MaxLength(2)]
        public string Unscrs { get; set; }

        [Description("仕入先コード")]
        [Column("SIRCOD")]
        [Required]
        [MaxLength(9)]
        public string Sircod { get; set; }

        [Description("運送区分コード")]
        [Column("UNSKBN")]
        [Required]
        [MaxLength(5)]
        public string Unskbn { get; set; }

        [Description("出荷場所コード")]
        [Column("SYBCOD")]
        [Required]
        [MaxLength(2)]
        public string Sybcod { get; set; }

        [Description("得意先コード")]
        [Column("TOKCOD")]
        [Required]
        [MaxLength(9)]
        public string Tokcod { get; set; }

        [Description("請求先コード")]
        [Column("SEICOD")]
        [Required]
        [MaxLength(9)]
        public string Seicod { get; set; }

        [Description("伝票区分コード")]
        [Column("DENKBN")]
        [Required]
        [MaxLength(2)]
        public string Denkbn { get; set; }

        [Description("伝票枚数")]
        [Column("DENMSU")]
        [Required]
        public int? Denmsu { get; set; }

        [Description("特記事項")]
        [Column("TKJIKO")]
        [Required]
        [MaxLength(40)]
        public string Tkjiko { get; set; }

        [Description("運賃負担")]
        [Column("UFUTAN")]
        [Required]
        [MaxLength(1)]
        public string Ufutan { get; set; }

        [Description("輸送作業伝票Ｎｏ")]
        [Column("YUSONO")]
        [Required]
        [MaxLength(7)]
        public string Yusono { get; set; }

        [Description("伝票出力フラグ")]
        [Column("DENF")]
        [Required]
        [MaxLength(1)]
        public string Denf { get; set; }

        [Description("制御フラグ１９")]
        [Column("CTLF19")]
        [Required]
        [MaxLength(1)]
        public string Ctlf19 { get; set; }

        [Description("運賃明細書発行区分")]
        [Column("MEHKBN")]
        [Required]
        [MaxLength(1)]
        public string Mehkbn { get; set; }

        [Description("ＰＣコード")]
        [Column("PCCOD")]
        [Required]
        [MaxLength(12)]
        public string Pccod { get; set; }

        [Description("県コード")]
        [Column("KENCOD")]
        [Required]
        [MaxLength(2)]
        public string Kencod { get; set; }

        [Description("識別コード")]
        [Column("SIKCOD")]
        [Required]
        [MaxLength(4)]
        public string Sikcod { get; set; }

        [Description("届先分割住所１")]
        [Column("TDBNJ1")]
        [Required]
        [MaxLength(6)]
        public string Tdbnj1 { get; set; }

        [Description("届先分割住所２")]
        [Column("TDBNJ2")]
        [Required]
        [MaxLength(20)]
        public string Tdbnj2 { get; set; }

        [Description("届先分割住所３")]
        [Column("TDBNJ3")]
        [Required]
        [MaxLength(50)]
        public string Tdbnj3 { get; set; }

        [Description("ＪＩＳコード")]
        [Column("JISCOD")]
        [Required]
        [MaxLength(8)]
        public string Jiscod { get; set; }

        [Description("振替得意先コード")]
        [Column("TENSIR")]
        [Required]
        [MaxLength(9)]
        public string Tensir { get; set; }

        [Description("出荷数")]
        [Column("SYUKSU")]
        [Required]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Syuksu { get; set; }

        [Description("無効区分")]
        [Column("MKCOD")]
        [Required]
        [MaxLength(1)]
        public string Mkcod { get; set; }

        [Description("無効理由")]
        [Column("MKRIYU")]
        [Required]
        [MaxLength(20)]
        public string Mkriyu { get; set; }

        [Description("郵便番号必須フラグ")]
        [Column("YUBFLG")]
        [Required]
        [MaxLength(1)]
        public string Yubflg { get; set; }

        [Description("登録担当")]
        [Column("CRTCOD")]
        [Required]
        [MaxLength(8)]
        public string Crtcod { get; set; }

        [Description("登録日")]
        [Column("CRTYMD")]
        [Required]
        public DateTime? Crtymd { get; set; }

        [Description("更新担当")]
        [Column("UPDCOD")]
        [Required]
        [MaxLength(8)]
        public string Updcod { get; set; }

        [Description("更新日")]
        [Column("UPDYMD")]
        [Required]
        public DateTime? Updymd { get; set; }

    }

}