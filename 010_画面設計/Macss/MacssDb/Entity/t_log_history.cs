namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_log_history
    {
        [Key]
        [Column(Order = 0)]
        public DateTime excection_date { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string user_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(32)]
        public string processing_id { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(4)]
        public string function { get; set; }

        [StringLength(500)]
        public string purpose1 { get; set; }

        [StringLength(500)]
        public string purpose2 { get; set; }

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
