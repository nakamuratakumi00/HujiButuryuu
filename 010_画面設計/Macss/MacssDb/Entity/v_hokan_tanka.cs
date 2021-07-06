namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_hokan_tanka
    {
        [StringLength(8)]
        public string MONTH { get; set; }

        [Key]
        [StringLength(8)]
        public string HINCOD { get; set; }

        [StringLength(2)]
        public string SYBCOD { get; set; }

        [StringLength(20)]
        public string HINNMK { get; set; }

        [StringLength(2)]
        public string KISYUA { get; set; }

        [StringLength(6)]
        public string KISYUB { get; set; }

        [StringLength(1)]
        public string TFULAG { get; set; }

        [StringLength(1)]
        public string FRIKAE { get; set; }

        public decimal? JYUKAR { get; set; }

        public decimal? SYUJYR { get; set; }

        public decimal? FPTANK { get; set; }

        public decimal? HOKANT { get; set; }

        public DateTime? HOKYMD { get; set; }

        [StringLength(8)]
        public string HOKTAN { get; set; }

        public decimal? NIEKIT { get; set; }

        public DateTime? NIEYMD { get; set; }

        [StringLength(8)]
        public string NIETAN { get; set; }

        [StringLength(2)]
        public string OSYBCOD { get; set; }

        [StringLength(20)]
        public string OHINNMK { get; set; }

        [StringLength(2)]
        public string OKISYUA { get; set; }

        [StringLength(6)]
        public string OKISYUB { get; set; }

        [StringLength(1)]
        public string OFRIKAE { get; set; }

        public decimal? OFPTNK { get; set; }

        public decimal? OHOKAT { get; set; }

        public decimal? ONIEKT { get; set; }

        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime? CRTYMD { get; set; }

        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime? UPDYMD { get; set; }
    }
}
