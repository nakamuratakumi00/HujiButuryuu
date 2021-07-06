namespace Macss.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_table_lock
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(60)]
        public string type { get; set; }

        public long? entity_id { get; set; }

        [StringLength(128)]
        public string object_name { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(60)]
        public string request_mode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(60)]
        public string request_type { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(60)]
        public string request_status { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Session_id { get; set; }

        [StringLength(128)]
        public string ProcessName { get; set; }
    }
}
