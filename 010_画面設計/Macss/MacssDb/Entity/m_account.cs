namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("m_account")]
    public partial class AccountMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AccountMaster()
        {
            m_account_role = new HashSet<AccountRoleMaster>();
        }

        [Key]
        [StringLength(32)]
        [DisplayName("アカウントID")]
        public string account_id { get; set; }

        [Required]
        [StringLength(32)]
        [DisplayName("アカウント名")]
        public string account_name { get; set; }

        [StringLength(32)]
        [DisplayName("アカウントカナ名")]
        public string account_name_kana { get; set; }

        [Required]
        [StringLength(256)]
        [DisplayName("パスワード")]
        [DataType(DataType.Password)]
        public string account_password { get; set; }

        [Required]
        [StringLength(6)]
        [DisplayName("照会グループ")]
        public string group_cd { get; set; }

        [DisplayName("ログイン回数")]
        public int login_count { get; set; }

        [DisplayName("ログイン連続失敗回数")]
        public int login_failure_count { get; set; }

        [DisplayName("最終ログイン日時")]
        public DateTime? last_login_date { get; set; }

        [DisplayName("削除フラグ")]
        public int delete_flg { get; set; }

        [NotMapped]
        public bool DeleteFlgBool
        {
            get
            {
                return this.delete_flg == 1;
            }
            set
            {
                this.delete_flg = (value ? 1 : 0);
            }
        }

        [Required]
        [StringLength(32)]
        [DisplayName("作成者ID")]
        public string create_id { get; set; }

        [DisplayName("作成日")]
        public DateTime create_date { get; set; }

        [Required]
        [StringLength(32)]
        [DisplayName("更新者ID")]
        public string update_id { get; set; }

        [DisplayName("更新日")]
        public DateTime update_date { get; set; }

        [Required]
        [StringLength(4)]
        [DisplayName("部門コード")]
        public string bumon_cd { get; set; }

        [StringLength(2)]
        [DisplayName("出荷場所コード")]
        public string basyo_cd { get; set; }

        [DisplayName("パスワード変更日時")]
        public DateTime account_password_date { get; set; }

        [StringLength(1)]
        [DisplayName("使用区分_顧")]
        public string sdek12 { get; set; }

        [StringLength(2)]
        [DisplayName("抽出フラグ")]
        public string ctlfl1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountRoleMaster> m_account_role { get; set; }
    }
}
