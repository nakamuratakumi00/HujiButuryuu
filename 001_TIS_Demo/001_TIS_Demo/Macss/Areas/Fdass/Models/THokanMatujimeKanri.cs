using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{
    [Table("t_hokan_matujime_kanri")]
    public class THokanMatujimeKanri
    {

        [Description("実施年月")]
        [Column("MONTH", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Month { get; set; }

        [Description("開始日時")]
        [Column("STARTT")]
        public DateTime? Startt { get; set; }

        [Description("終了日時")]
        [Column("ENDT")]
        public DateTime? Endt { get; set; }

        [Description("実行ステータス")]
        [Column("STATUS")]
        [MaxLength(1)]
        public string Status { get; set; }

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