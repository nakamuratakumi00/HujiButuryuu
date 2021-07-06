namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_group
    {
        [Key]
        [StringLength(32)]
        public string group_cd { get; set; }

        [Required]
        [StringLength(64)]
        public string group_name { get; set; }

        [StringLength(32)]
        public string upper_class_cd { get; set; }

        [Required]
        [StringLength(4)]
        public string kbn { get; set; }

        [Required]
        [StringLength(32)]
        public string create_id { get; set; }

        public DateTime create_date { get; set; }

        [Required]
        [StringLength(32)]
        public string update_id { get; set; }

        public DateTime update_date { get; set; }
    }
}
