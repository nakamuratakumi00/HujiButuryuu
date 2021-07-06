namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class w_hokan_denpyokensu
    {
        public decimal ID { get; set; }

        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(3)]
        public string KISYUB { get; set; }

        [StringLength(2)]
        public string BASYO { get; set; }

        [StringLength(2)]
        public string DENKUB { get; set; }

        public decimal? DENSUU { get; set; }

        [StringLength(9)]
        public string SEIKYU { get; set; }

        public DateTime? INPYMD { get; set; }

        public DateTime? KEIYMD { get; set; }

        [StringLength(8)]
        public string HINCOD { get; set; }

        public decimal? NSKOSU { get; set; }

        [StringLength(2)]
        public string EIGSOK { get; set; }

        [StringLength(1)]
        public string DTMOTO { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
