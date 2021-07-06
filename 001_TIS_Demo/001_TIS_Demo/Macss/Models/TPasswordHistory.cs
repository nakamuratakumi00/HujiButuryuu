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
    [Table("t_password_history")]
    public class TPasswordHistory
    {
        [Description("アカウントID")]
        [Column("account_id", Order = 0)]
        [Required]
        [Key]
        [MaxLength(32)]
        public string Id { get; set; }

        [Description("パスワード")]
        [Column("account_password", Order = 1)]
        [Required]
        [Key]
        [MaxLength(256)]
        public string Password { get; set; }

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