using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace Macss.Models
{

    [Table("m_torihikisaki")]
    public class MTorihikisaki
    {

        [Description("取引先コード")]
        [Column("TORCOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(9)]
        public string Torcod { get; set; }

        [Description("取引先名")]
        [Column("TORNAM")]
        [Required]
        [MaxLength(40)]
        public string Tornam { get; set; }

        [Description("取引先名（カナ）")]
        [Column("TORNMK")]
        [Required]
        [MaxLength(80)]
        public string Tornmk { get; set; }

        [Description("部課名")]
        [Column("BUKNAM")]
        [Required]
        [MaxLength(20)]
        public string Buknam { get; set; }

        [Description("担当者名")]
        [Column("TANNAM")]
        [Required]
        [MaxLength(10)]
        public string Tannam { get; set; }

        [Description("電話番号")]
        [Column("TELNO")]
        [Required]
        [MaxLength(20)]
        public string Telno { get; set; }

        [Description("ＦＡＸ番号")]
        [Column("FAXNO")]
        [Required]
        [MaxLength(20)]
        public string Faxno { get; set; }

        [Description("郵便番号")]
        [Column("YUBINN")]
        [Required]
        [MaxLength(8)]
        public string Yubinn { get; set; }

        [Description("住所")]
        [Column("JYSYO")]
        [Required]
        [MaxLength(60)]
        public string Jysyo { get; set; }

        [Description("請求締日")]
        [Column("SIMDAY")]
        [Required]
        [MaxLength(2)]
        public string Simday { get; set; }

        [Description("請求先コード１")]
        [Column("SECO01")]
        [Required]
        [MaxLength(9)]
        public string Seco01 { get; set; }

        [Description("請求先コード６")]
        [Column("SECO06")]
        [Required]
        [MaxLength(9)]
        public string Seco06 { get; set; }

        [Description("本社Ｃ松本")]
        [Column("FBTCDM")]
        [Required]
        [MaxLength(9)]
        public string Fbtcdm { get; set; }

        [Description("Ｆｅ機種Ａ")]
        [Column("KISYUA")]
        [Required]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("帳票出力フラグ")]
        [Column("LSTFLG")]
        [Required]
        [MaxLength(1)]
        public string Lstflg { get; set; }

        [Description("ＦＢ本社得意先コード")]
        [Column("FBTCOD")]
        [Required]
        [MaxLength(9)]
        public string Fbtcod { get; set; }

        [Description("顧客コード")]
        [Column("KOKCOD")]
        [Required]
        [MaxLength(4)]
        public string Kokcod { get; set; }

        [Description("運賃明細書発行区分")]
        [Column("MEHK01")]
        [Required]
        [MaxLength(1)]
        public string Mehk01 { get; set; }

        [Description("ＦＢ本社仕入先コード")]
        [Column("FBTCDS")]
        [Required]
        [MaxLength(9)]
        public string Fbtcds { get; set; }

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