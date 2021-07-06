using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_unsouhouhou")]
    public class MUnsouUnsouhouhou
    {

        [Description("運送方法コード")]
        [Column("UNSCOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Unscod { get; set; }

        [Description("運送方法名")]
        [Column("UNSNAM")]
        [Required]
        [MaxLength(20)]
        public string Unsnam { get; set; }

        [Description("略名")]
        [Column("RYKNAM")]
        [Required]
        [MaxLength(6)]
        public string Ryknam { get; set; }

        [Description("取引先コード")]
        [Column("TORCOD")]
        [Required]
        [MaxLength(9)]
        public string Torcod { get; set; }

        [Description("運送区分")]
        [Column("UNSKBN")]
        [Required]
        [MaxLength(5)]
        public string Unskbn { get; set; }

        [Description("出荷関係Ｆ２")]
        [Column("SCTL02")]
        [Required]
        [MaxLength(1)]
        public string Sctl02 { get; set; }

        [Description("出荷関係Ｆ３")]
        [Column("SCTL03")]
        [Required]
        [MaxLength(1)]
        public string Sctl03 { get; set; }

        [Description("出荷関係Ｆ６")]
        [Column("SCTL06")]
        [Required]
        [MaxLength(1)]
        public string Sctl06 { get; set; }

        [Description("出荷関係Ｆ８")]
        [Column("SCTL08")]
        [Required]
        [MaxLength(1)]
        public string Sctl08 { get; set; }

        [Description("Ｐ制御フラグ５")]
        [Column("PCTL05")]
        [Required]
        [MaxLength(1)]
        public string Pctl05 { get; set; }

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