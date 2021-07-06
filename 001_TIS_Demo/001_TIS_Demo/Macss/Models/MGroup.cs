using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Models
{
    [Table("m_group")]
    public class MGroup
    {
        [Description("グループコード")]
        [Column("group_cd")]
        [Required]
        [Key]
        [Index(IsUnique = true)]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string GroupCd { get; set; }

        [Description("グループ名")]
        [Column("group_name")]
        [Required]
        [MaxLength(64)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string GroupName { get; set; }

        [Description("上位グループコード")]
        [Column("upper_class_cd")]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string UpperClassCd { get; set; }

        [Description("区分")]
        [Column("kbn")]
        [Required]
        [MaxLength(4)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string Kbn { get; set; }

        [Description("作成者ID")]
        [Column("create_id")]
        [Required]
        [MaxLength(32)]
        public string CreateId { get; set; }

        [Description("作成日")]
        [Column("create_date")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Description("更新者ID")]
        [Column("update_id")]
        [Required]
        [MaxLength(32)]
        public string UpdateId { get; set; }

        [Description("更新日")]
        [Column("update_date")]
        [Required]
        public DateTime UpdateDate { get; set; }
    }
}