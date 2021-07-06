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

    [Table("m_seikyusaki")]
    public class MSeikyusaki
    {

        [Description("請求先コード")]
        [Column("SEICOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(9)]
        public string Seicod { get; set; }

        [Description("請求先名称")]
        [Column("SEINAM")]
        [Required]
        [MaxLength(40)]
        public string Seinam { get; set; }

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

        [Description("郵便番号")]
        [Column("YUBNO")]
        [Required]
        [MaxLength(8)]
        public string Yubno { get; set; }

        [Description("住所")]
        [Column("JYSYO")]
        [Required]
        [MaxLength(60)]
        public string Jysyo { get; set; }

        [Description("帳票出力フラグ")]
        [Column("LSTFLG")]
        [Required]
        [MaxLength(1)]
        public string Lstflg { get; set; }

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