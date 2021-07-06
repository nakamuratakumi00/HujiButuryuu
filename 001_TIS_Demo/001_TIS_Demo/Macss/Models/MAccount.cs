using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Macss.Attributes.Custom;

namespace Macss.Models
{
    [Table("m_account")]
    public class MAccount : IUser<string>
    {
        [Description("アカウントID")]
        [Column("account_id")]
        [Required]
        [MaxLength(32)]
        public string Id { get; set; } = "";

        [Description("アカウント名")]
        [Column("account_name")]
        [Required]
        [MaxLength(32)]
        public string UserName { get; set; } = "";

        [Description("アカウントカナ名")]
        [Column("account_name_kana")]
        [MaxLength(32)]
        public string AccountNameKana { get; set; } = "";

        [Description("パスワード")]
        [Column("account_password")]
        [Required]
        [MaxLength(256)]
        public string Password { get; set; } = "";

        [Description("ログイン回数")]
        [Column("login_count")]
        [Required]
        [Default(Value = "0")]
        public int LoginCount { get; set; }

        [Description("ログイン連続失敗回数")]
        [Column("login_failure_count")]
        [Required]
        [Default(Value = "0")]
        public int LoginFailureCount { get; set; }

        [Description("最終ログイン日時")]
        [Column("last_login_date")]
        public DateTime? LastLoginDate { get; set; }

        [Description("削除フラグ")]
        [Column("delete_flg")]
        [Required]
        [Default(Value = "0")]
        public int DeleteFlg { get; set; }

        [Description("部門コード")]
        [Column("bumon_cd")]
        [Required]
        [MaxLength(4)]
        public string BumonCd { get; set; } = "";

        [Description("出荷場所コード")]
        [Column("basyo_cd")]
        [MaxLength(2)]
        public string BasyoCd { get; set; } = "";

        [Description("照会グループ")]
        [Column("group_cd")]
        [Required]
        [MaxLength(6)]
        public string GroupCd { get; set; } = "";

        [Description("パスワード変更日時")]
        [Column("account_password_date")]
        [Required]
        public DateTime AccountPasswordDate { get; set; }

        [Description("使用区分_顧")]
        [Column("sdek12")]
        [MaxLength(1)]
        public string Sdek12 { get; set; } = "";

        [Description("抽出フラグ")]
        [Column("ctlfl1")]
        [MaxLength(2)]
        public string Ctlfl1 { get; set; } = "";

        [Description("作成者ID")]
        [Column("create_id")]
        [Required]
        [MaxLength(32)]
        public string CreateId { get; set; } = "";

        [Description("作成日")]
        [Column("create_date")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Description("更新者ID")]
        [Column("update_id")]
        [Required]
        [MaxLength(32)]
        public string UpdateId { get; set; } = "";

        [Description("更新日")]
        [Column("update_date")]
        [Required]
        public DateTime UpdateDate { get; set; }

        //public virtual MGroup MGroup { get; set; }

        [ForeignKey("AccountId")]
        public virtual ICollection<MAccountRole> MAccountRole { get; set; } = new List<MAccountRole>();
    }
}