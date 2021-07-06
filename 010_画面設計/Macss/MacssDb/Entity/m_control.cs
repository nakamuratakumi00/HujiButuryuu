namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_control
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string section { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(4)]
        public string kbn { get; set; }

        [StringLength(256)]
        public string value1 { get; set; }

        [StringLength(256)]
        public string value2 { get; set; }

        [StringLength(256)]
        public string value3 { get; set; }

        public decimal? numeric_value1 { get; set; }

        public decimal? numeric_value2 { get; set; }

        public decimal? numeric_value3 { get; set; }

        public int? sort_no { get; set; }

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
