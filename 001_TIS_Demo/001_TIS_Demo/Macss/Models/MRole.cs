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
    [Table("m_role")]
    public class MRole
    {
        [Description("ロールID")]
        [Column("role_id")]
        [Required]
        [Key]
        [Index(IsUnique = true)]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string RoleId { get; set; }

        [Description("ロール名")]
        [Column("role_name")]
        [Required]
        [MaxLength(64)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string RoleName { get; set; }

        [Description("ロール説明")]
        [Column("role_cmt")]
        [MaxLength(256)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string RoleCmt { get; set; }

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

        public virtual ICollection<MMenuRole> MMenuRole { get; set; } = new List<MMenuRole>();
        
        public virtual ICollection<MAccountRole> MAccountRole { get; set; } = new List<MAccountRole>();
    }
}