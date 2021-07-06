namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public m_role()
        {
            m_account_role = new HashSet<AccountRoleMaster>();
            m_menu_role = new HashSet<m_menu_role>();
        }

        [Key]
        [StringLength(32)]
        public string role_id { get; set; }

        [Required]
        [StringLength(64)]
        public string role_name { get; set; }

        [StringLength(256)]
        public string role_cmt { get; set; }

        [Required]
        [StringLength(32)]
        public string create_id { get; set; }

        public DateTime create_date { get; set; }

        [Required]
        [StringLength(32)]
        public string update_id { get; set; }

        public DateTime update_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountRoleMaster> m_account_role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<m_menu_role> m_menu_role { get; set; }
    }
}
