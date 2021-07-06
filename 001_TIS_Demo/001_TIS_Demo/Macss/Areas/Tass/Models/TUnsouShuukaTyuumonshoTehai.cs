using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("t_unsou_shuuka_tyuumonsho_tehai")]
    public class TUnsouShuukaTyuumonshoTehai
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
        public DateTime? Sykymd { get; set; }

        [Description("Ｆｅ機種")]
        [Column("KISYU")]
        [MaxLength(8)]
        public string Kisyu { get; set; }

        [Description("経費負担Ｎｏ")]
        [Column("KEIFNO")]
        [MaxLength(30)]
        public string Keifno { get; set; }

        [Description("振替出荷Ｎｏ")]
        [Column("FSYKNO")]
        [MaxLength(20)]
        public string Fsykno { get; set; }

        [Description("出荷場所コード")]
        [Column("SYBCOD")]
        [MaxLength(2)]
        public string Sybcod { get; set; }

        [Description("得意先コード")]
        [Column("TOKCOD")]
        [MaxLength(9)]
        public string Tokcod { get; set; }

        [Description("請求先コード")]
        [Column("SEICOD")]
        [MaxLength(9)]
        public string Seicod { get; set; }

        [Description("発注元")]
        [Column("HTYNAM")]
        [MaxLength(20)]
        public string Htynam { get; set; }

        [Description("発注元課補係長")]
        [Column("HTYKAH")]
        [MaxLength(10)]
        public string Htykah { get; set; }

        [Description("担当者コード")]
        [Column("TANCOD")]
        [MaxLength(8)]
        public string Tancod { get; set; }

        [Description("担当者")]
        [Column("TANNAM")]
        [MaxLength(10)]
        public string Tannam { get; set; }

        [Description("発注元ＴＥＬ")]
        [Column("HTYTEL")]
        [MaxLength(20)]
        public string Htytel { get; set; }

        [Description("現品保管場所")]
        [Column("BASYO")]
        [MaxLength(20)]
        public string Basyo { get; set; }

        [Description("届先コード")]
        [Column("TDKCOD")]
        [MaxLength(15)]
        public string Tdkcod { get; set; }

        [Description("届先郵便番号")]
        [Column("TDKYUB")]
        [MaxLength(10)]
        public string Tdkyub { get; set; }

        [Description("届先住所")]
        [Column("TDKJYU")]
        [MaxLength(60)]
        public string Tdkjyu { get; set; }

        [Description("届先社名")]
        [Column("TDKNAM")]
        [MaxLength(20)]
        public string Tdknam { get; set; }

        [Description("届先支店名")]
        [Column("TDSNAM")]
        [MaxLength(20)]
        public string Tdsnam { get; set; }

        [Description("届先部課名")]
        [Column("TDBNAM")]
        [MaxLength(20)]
        public string Tdbnam { get; set; }

        [Description("届先担当者名")]
        [Column("TDKTAN")]
        [MaxLength(20)]
        public string Tdktan { get; set; }

        [Description("届先電話番号")]
        [Column("TDKTEL")]
        [MaxLength(20)]
        public string Tdktel { get; set; }

        [Description("代表品名コード")]
        [Column("DHINCOD")]
        [MaxLength(15)]
        public string Dhincod { get; set; }

        [Description("代表品名")]
        [Column("DHINNAM")]
        [MaxLength(80)]
        public string Dhinnam { get; set; }

        [Description("代表出荷数")]
        [Column("DSYUKSU")]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Dsyuksu { get; set; }

        [Description("特記事項")]
        [Column("TKJIKO")]
        [MaxLength(40)]
        public string Tkjiko { get; set; }

        [Description("コメント")]
        [Column("COMENT")]
        [MaxLength(20)]
        public string Coment { get; set; }

        [Description("運送方法コード")]
        [Column("UNSCOD")]
        [MaxLength(2)]
        public string Unscod { get; set; }

        [Description("運送コース")]
        [Column("UNSCRS")]
        [MaxLength(2)]
        public string Unscrs { get; set; }

        [Description("仕入先コード")]
        [Column("SIRCOD")]
        [MaxLength(9)]
        public string Sircod { get; set; }

        [Description("運送区分コード")]
        [Column("UNSKBN")]
        [MaxLength(5)]
        public string Unskbn { get; set; }

        [Description("伝票区分コード")]
        [Column("DENKBN")]
        [MaxLength(2)]
        public string Denkbn { get; set; }

        [Description("伝票枚数")]
        [Column("DENMSU")]
        public int? Denmsu { get; set; }

        [Description("運賃負担")]
        [Column("UFUTAN")]
        [MaxLength(1)]
        public string Ufutan { get; set; }

        [Description("輸送作業伝票Ｎｏ")]
        [Column("YUSONO")]
        [MaxLength(7)]
        public string Yusono { get; set; }

        [Description("伝票出力フラグ")]
        [Column("DENF")]
        [MaxLength(1)]
        public string Denf { get; set; }

        [Description("最終発行日時")]
        [Column("HKUYMD")]
        public DateTime? Hkuymd { get; set; }

        [Description("発行者")]
        [Column("HKUCOD")]
        [MaxLength(8)]
        public string Hkucod { get; set; }

        [Description("ＰＣコード")]
        [Column("PCCOD")]
        [MaxLength(12)]
        public string Pccod { get; set; }

        [Description("郵便番号必須フラグ")]
        [Column("YUBFLG")]
        [MaxLength(1)]
        public string Yubflg { get; set; }

        [Description("無効区分")]
        [Column("MUKOUKBN")]
        [MaxLength(1)]
        public string Mukoukbn { get; set; }

        [Description("無効理由")]
        [Column("MUKOURIYUU")]
        [MaxLength(20)]
        public string Mukouriyuu { get; set; }

        [Description("パターンコード")]
        [Column("INSCOD")]
        [MaxLength(6)]
        public string Inscod { get; set; }

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