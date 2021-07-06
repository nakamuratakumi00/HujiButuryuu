namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_seikyusaki
    {
        [Key]
        [StringLength(9)]
        public string SEICOD { get; set; }

        [Required]
        [StringLength(40)]
        public string SEINAM { get; set; }

        [Required]
        [StringLength(20)]
        public string BUKNAM { get; set; }

        [Required]
        [StringLength(10)]
        public string TANNAM { get; set; }

        [Required]
        [StringLength(20)]
        public string TELNO { get; set; }

        [Required]
        [StringLength(8)]
        public string YUBNO { get; set; }

        [Required]
        [StringLength(60)]
        public string JYSYO { get; set; }

        [Required]
        [StringLength(1)]
        public string LSTFLG { get; set; }

        [Required]
        [StringLength(8)]
        public string CRTCOD { get; set; }

        public DateTime CRTYMD { get; set; }

        [Required]
        [StringLength(8)]
        public string UPDCOD { get; set; }

        public DateTime UPDYMD { get; set; }
    }
}
