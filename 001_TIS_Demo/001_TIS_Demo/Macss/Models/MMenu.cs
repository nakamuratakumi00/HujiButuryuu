using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Macss.Attributes.Custom;

namespace Macss.Models
{
    [Table("m_menu")]
    public class MMenu
    {
        [Description("メニューID")]
        [Column("menu_id")]
        [Required]
        [Key]
        [Index(IsUnique = true)]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string MenuId { get; set; }

        [Description("メニュー表記")]
        [Column("menu_name")]
        [Required]
        [MaxLength(64)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string MenuName { get; set; }

        [Description("ロール表記")]
        [Column("role_name")]
        [MaxLength(128)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string RoleName { get; set; }

        [Description("タイトル表記")]
        [Column("title_name")]
        [MaxLength(128)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string TitleName { get; set; }

        [Description("アクション名")]
        [Column("action_name")]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string ActionName { get; set; }

        [Description("コントローラ名")]
        [Column("controller_name")]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string ControllerName { get; set; }

        [Description("親メニューID")]
        [Column("parent_id")]
        [Required]
        [MaxLength(32)]
        [RegularExpression(@"[ -~｡-ﾟ]+")]
        public string ParentId { get; set; }

        [Description("メニュー区分")]
        [Column("menu_kbn")]
        [Required]
        [Default(Value = "0")]
        public int MenuKbn { get; set; }

        [Description("ロール区分")]
        [Column("role_kbn")]
        [Required]
        [Default(Value = "0")]
        public int RoleKbn { get; set; }

        [Description("表示順")]
        [Column("menu_sort")]
        public int? MenuSort { get; set; }

        [Description("コメント")]
        [Column("comment")]
        [MaxLength(512)]
        [RegularExpression(@"[^ -~｡-ﾟ]+")]
        public string Comment { get; set; }

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

    }
}