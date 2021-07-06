namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public m_menu()
        {
            m_menu_role = new HashSet<m_menu_role>();
        }

        [Key]
        [StringLength(32)]
        public string menu_id { get; set; }

        [Required]
        [StringLength(64)]
        public string menu_name { get; set; }

        [StringLength(128)]
        public string role_name { get; set; }

        [StringLength(128)]
        public string title_name { get; set; }

        [StringLength(32)]
        public string action_name { get; set; }

        [StringLength(32)]
        public string controller_name { get; set; }

        [Required]
        [StringLength(32)]
        public string parent_id { get; set; }

        public int menu_kbn { get; set; }

        public int role_kbn { get; set; }

        public int? menu_sort { get; set; }

        [StringLength(512)]
        public string comment { get; set; }

        [Required]
        [StringLength(32)]
        public string create_id { get; set; }

        public DateTime create_date { get; set; }

        [Required]
        [StringLength(32)]
        public string update_id { get; set; }

        public DateTime update_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<m_menu_role> m_menu_role { get; set; }
    }
}
