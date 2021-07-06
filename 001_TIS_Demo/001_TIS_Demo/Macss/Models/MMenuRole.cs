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
    [Table("m_menu_role")]
    public class MMenuRole
    {
        [Description("ロールID")]
        [Column("role_id",Order = 0)]
        [Required]
        [Key]
        [Index]
        [MaxLength(32)]
        public string RoleId { get; set; }

        [Description("メニューID")]
        [Column("menu_id",Order = 1)]
        [Required]
        [Key]
        [Index]
        [MaxLength(32)]
        public string MenuId { get; set; }

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