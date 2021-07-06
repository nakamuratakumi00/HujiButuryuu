using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Tass.Models
{

    [Table("m_unsou_postalcode")]
    public class MUnsouPostalcode
    {

        [Description("郵便番号")]
        [Column("YUBINN", Order = 0)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Yubinn { get; set; }

        [Description("分割住所１")]
        [Column("JYUSY1", Order = 1)]
        [Required]
        [Key]
        [MaxLength(10)]
        public string Jyusy1 { get; set; }

        [Description("分割住所２")]
        [Column("JYUSY2", Order = 2)]
        [Required]
        [Key]
        [MaxLength(40)]
        public string Jyusy2 { get; set; }

        [Description("分割住所３")]
        [Column("JYUSY3", Order = 3)]
        [Required]
        [Key]
        [MaxLength(40)]
        public string Jyusy3 { get; set; }

        [Description("住所カナ")]
        [Column("JYKANA", Order = 4)]
        [Required]
        [Key]
        [MaxLength(50)]
        public string Jykana { get; set; }

        [Description("検索用住所１")]
        [Column("KENJY1", Order = 5)]
        [Required]
        [Key]
        [MaxLength(10)]
        public string Kenjy1 { get; set; }

        [Description("検索用住所２")]
        [Column("KENJY2", Order = 6)]
        [Required]
        [Key]
        [MaxLength(40)]
        public string Kenjy2 { get; set; }

        [Description("検索用住所３")]
        [Column("KENJY3", Order = 7)]
        [Required]
        [Key]
        [MaxLength(40)]
        public string Kenjy3 { get; set; }

        [Description("ＪＩＳコード")]
        [Column("JISCOD")]
        [Required]
        [MaxLength(8)]
        public string Jiscod { get; set; }

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