using Macss.Attributes.Custom;
using Macss.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_hinmei_koyuu")]
    public class MUnsouHinmeiKoyuu
    {

        [Description("品名コード")]
        [Column("HINCOD", Order = 0)]
        [Required]
        [FileDoubleRequired]
        [Key]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hincod { get; set; }

        [Description("品名")]
        [Column("HINNAM")]
        [Required]
        [FileRequired]
        [MaxLength(80)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Hinnam { get; set; }

        [Description("品名カナ")]
        [Column("HINNMK")]
        [Required]
        [FileRequired]
        [MaxLength(80)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Hinnmk { get; set; }

        [Description("得意先コード")]
        [Column("TORCOD")]
        [Required]
        [MaxLength(9)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Torcod { get; set; }

        [Description("Ｆｅ機種Ａ")]
        [Column("KISYUA")]
        [Required]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Kisyua { get; set; }

        [Description("Ｆｅ機種Ｂ")]
        [Column("KISYUB")]
        [Required]
        [MaxLength(6)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Kisyub { get; set; }

        [Description("データ管理元")]
        [Column("DTMOTO")]
        [Required]
        [FileDoubleRequired]
        [MaxLength(1)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Dtmoto { get; set; }

        [Description("単価コード")]
        [Column("TKCOD")]
        [Required]
        [MaxLength(5)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Tkcod { get; set; }

        [Description("種別")]
        [Column("SYUBTU")]
        [Required]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Syubtu { get; set; }

        [Description("抽出フラグ")]
        [Column("CTLFL1")]
        [Required]
        [MaxLength(2)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Ctlfl1 { get; set; }

        [Description("顧客品名コード")]
        [Column("KHINCD")]
        [Required]
        [MaxLength(15)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Khincd { get; set; }

        [Description("削除フラグ")]
        [Column("DELFLG")]
        [Required]
        [MaxLength(1)]
        public string Delflg { get; set; }

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