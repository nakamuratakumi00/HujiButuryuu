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
        [DisplayName("�A�J�E���gID")]
        public string account_id { get; set; }

        [Required]
        [StringLength(32)]
        [DisplayName("�A�J�E���g��")]
        public string account_name { get; set; }

        [StringLength(32)]
        [DisplayName("�A�J�E���g�J�i��")]
        public string account_name_kana { get; set; }

        [Required]
        [StringLength(256)]
        [DisplayName("�p�X���[�h")]
        [DataType(DataType.Password)]
        public string account_password { get; set; }

        [Required]
        [StringLength(6)]
        [DisplayName("�Ɖ�O���[�v")]
        public string group_cd { get; set; }

        [DisplayName("���O�C����")]
        public int login_count { get; set; }

        [DisplayName("���O�C���A�����s��")]
        public int login_failure_count { get; set; }

        [DisplayName("�ŏI���O�C������")]
        public DateTime? last_login_date { get; set; }

        [DisplayName("�폜�t���O")]
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
        [DisplayName("�쐬��ID")]
        public string create_id { get; set; }

        [DisplayName("�쐬��")]
        public DateTime create_date { get; set; }

        [Required]
        [StringLength(32)]
        [DisplayName("�X�V��ID")]
        public string update_id { get; set; }

        [DisplayName("�X�V��")]
        public DateTime update_date { get; set; }

        [Required]
        [StringLength(4)]
        [DisplayName("����R�[�h")]
        public string bumon_cd { get; set; }

        [StringLength(2)]
        [DisplayName("�o�׏ꏊ�R�[�h")]
        public string basyo_cd { get; set; }

        [DisplayName("�p�X���[�h�ύX����")]
        public DateTime account_password_date { get; set; }

        [StringLength(1)]
        [DisplayName("�g�p�敪_��")]
        public string sdek12 { get; set; }

        [StringLength(2)]
        [DisplayName("���o�t���O")]
        public string ctlfl1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountRoleMaster> m_account_role { get; set; }
    }
}
