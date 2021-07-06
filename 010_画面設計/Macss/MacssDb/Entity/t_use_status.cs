namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_use_status
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string session_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(32)]
        public string account_id { get; set; }

        [StringLength(128)]
        public string title_name { get; set; }

        [StringLength(32)]
        public string action_name { get; set; }

        [StringLength(32)]
        public string controller_name { get; set; }

        public DateTime update_date { get; set; }
    }
}
