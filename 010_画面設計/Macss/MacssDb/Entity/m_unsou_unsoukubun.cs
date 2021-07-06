namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_unsoukubun
    {
        [Key]
        [StringLength(5)]
        public string UNSKBN { get; set; }

        [Required]
        [StringLength(20)]
        public string UNSNAM { get; set; }

        [Required]
        [StringLength(4)]
        public string JGYKBN { get; set; }

        [Required]
        [StringLength(4)]
        public string JGYKB2 { get; set; }

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
