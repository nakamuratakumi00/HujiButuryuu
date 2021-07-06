using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Macss.Models
{
    [Table("t_use_status")]
    public class TUseStatus
    {

        [Description("セッションID")]
        [Column("session_id", Order = 0)]
        [Required]
        [Key]
        [MaxLength(100)]
        public string SessionId { get; set; }

        [Description("アカウントID")]
        [Column("account_id", Order = 1)]
        [Required]
        [Key]
        [MaxLength(32)]
        public string AccountId { get; set; }

        [Description("機能")]
        [Column("title_name")]
        [MaxLength(128)]
        public string TitleName { get; set; }

        [Description("アクション名")]
        [Column("action_name")]
        [MaxLength(32)]
        public string ActionName { get; set; }

        [Description("コントローラ名")]
        [Column("controller_name")]
        [MaxLength(32)]
        public string ControllerName { get; set; }

        [Description("更新日")]
        [Column("update_date")]
        [Required]
        public DateTime UpdateDate { get; set; }

    }
}