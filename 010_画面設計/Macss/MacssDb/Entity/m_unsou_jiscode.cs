namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class m_unsou_jiscode
    {
        [Key]
        [StringLength(8)]
        public string JISCOD { get; set; }

        [Required]
        [StringLength(10)]
        public string JYUSY1 { get; set; }

        [Required]
        [StringLength(50)]
        public string JYUSY2 { get; set; }

        [Required]
        [StringLength(50)]
        public string JYUSY3 { get; set; }

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
