namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class t_hokan_matujime_kanri
    {
        [Key]
        [StringLength(8)]
        public string MONTH { get; set; }

        public DateTime? STARTT { get; set; }

        public DateTime? ENDT { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
