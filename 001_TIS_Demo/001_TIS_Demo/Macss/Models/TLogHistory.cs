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
    [Table("t_log_history")]
    public class TLogHistory
    {
        [Description("処理日時")]
        [Column("excection_date",Order = 0)]
        [Required]
        [Key]
        [Index]
        public DateTime ExcectionDate { get; set; }

        [Description("ユーザID")]
        [Column("user_id", Order = 1)]
        [Required]
        [Key]
        [Index]
        [MaxLength(32)]
        public string UserId { get; set; }

        [Description("メニューID")]
        [Column("processing_id",Order = 2)]
        [Required]
        [Key]
        [Index]
        [MaxLength(32)]
        public string ProcessingId { get; set; }

        [Description("処理区分ID")]
        [Column("function",Order = 3)]
        [Required]
        [Key]
        [Index]
        [MaxLength(4)]
        public string Function { get; set; }

        [Description("汎用1")]
        [Column("purpose1")]
        [MaxLength(500)]
        public string Purpose1 { get; set; }

        [Description("汎用2")]
        [Column("purpose2")]
        [MaxLength(500)]
        public string Purpose2 { get; set; }

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