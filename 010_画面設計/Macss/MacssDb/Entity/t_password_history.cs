namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_password_history
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(32)]
        public string account_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string account_password { get; set; }

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
