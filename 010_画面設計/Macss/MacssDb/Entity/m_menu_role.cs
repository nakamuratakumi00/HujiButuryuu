namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_menu_role
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string role_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string menu_id { get; set; }

        [Required]
        [StringLength(32)]
        public string create_id { get; set; }

        public DateTime create_date { get; set; }

        [Required]
        [StringLength(32)]
        public string update_id { get; set; }

        public DateTime update_date { get; set; }

        public virtual m_menu m_menu { get; set; }

        public virtual m_role m_role { get; set; }
    }
}
