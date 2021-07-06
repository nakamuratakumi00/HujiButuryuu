using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{
    [Table("m_hokan_jouken")]
    public class MHokanJouken
    {

        [Description("識別コード")]
        [Column("SIKCOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(10)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Sikcod { get; set; }

        [Description("条件")]
        [Column("JYOKEN", Order = 1)]
        [Required]
        [Key]
        [MaxLength(10)]
        [RegularExpression(@"[A-Z0-9 -/:-@\[-`{-~｡-ﾟ]+")]
        public string Jyoken { get; set; }

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