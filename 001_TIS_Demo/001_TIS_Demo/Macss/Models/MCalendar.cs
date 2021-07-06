using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Models
{
    [Table("m_calendar")]
    public class MCalendar
    {

        [Description("出荷場所コード")]
        [Column("SYBCOD", Order = 0)]
        [Required]
        [Key]
        [MaxLength(2)]
        public string Sybcod { get; set; }

        [Description("年月日")]
        [Column("YYMMDD", Order = 1)]
        [Required]
        [Key]
        public DateTime? Yymmdd { get; set; }

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