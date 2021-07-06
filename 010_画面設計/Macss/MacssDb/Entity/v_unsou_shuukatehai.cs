namespace Macss.Database.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class v_unsou_shuukatehai
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string SYUKNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string CDATE { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime SYKYMD { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(8)]
        public string KISYU { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(30)]
        public string KEIFNO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string FSYKNO { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(8)]
        public string TANCOD { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(10)]
        public string TANNAM { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(15)]
        public string TDKCOD { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(10)]
        public string TDKYUB { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(60)]
        public string TDKJYU { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(20)]
        public string TDKNAM { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(20)]
        public string TDSNAM { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(20)]
        public string TDBNAM { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(20)]
        public string TDKTAN { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(20)]
        public string TDKTEL { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(15)]
        public string HINCOD { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(80)]
        public string HINNAM { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(2)]
        public string UNSCOD { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(2)]
        public string UNSCRS { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(9)]
        public string SIRCOD { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(5)]
        public string UNSKBN { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(2)]
        public string SYBCOD { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(9)]
        public string TOKCOD { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(9)]
        public string SEICOD { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(2)]
        public string DENKBN { get; set; }

        [Key]
        [Column(Order = 26)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DENMSU { get; set; }

        [Key]
        [Column(Order = 27)]
        [StringLength(40)]
        public string TKJIKO { get; set; }

        [Key]
        [Column(Order = 28)]
        [StringLength(1)]
        public string UFUTAN { get; set; }

        [Key]
        [Column(Order = 29)]
        [StringLength(7)]
        public string YUSONO { get; set; }

        [Key]
        [Column(Order = 30)]
        [StringLength(1)]
        public string DENF { get; set; }

        [Key]
        [Column(Order = 31)]
        [StringLength(1)]
        public string CTLF19 { get; set; }

        [Key]
        [Column(Order = 32)]
        [StringLength(1)]
        public string MEHKBN { get; set; }

        [Key]
        [Column(Order = 33)]
        [StringLength(12)]
        public string PCCOD { get; set; }

        [Key]
        [Column(Order = 34)]
        [StringLength(2)]
        public string KENCOD { get; set; }

        [Key]
        [Column(Order = 35)]
        [StringLength(4)]
        public string SIKCOD { get; set; }

        [Key]
        [Column(Order = 36)]
        [StringLength(6)]
        public string TDBNJ1 { get; set; }

        [Key]
        [Column(Order = 37)]
        [StringLength(20)]
        public string TDBNJ2 { get; set; }

        [Key]
        [Column(Order = 38)]
        [StringLength(50)]
        public string TDBNJ3 { get; set; }

        [Key]
        [Column(Order = 39)]
        [StringLength(8)]
        public string JISCOD { get; set; }

        [Key]
        [Column(Order = 40)]
        [StringLength(9)]
        public string TENSIR { get; set; }

        [Key]
        [Column(Order = 41)]
        public decimal SYUKSU { get; set; }

        [Key]
        [Column(Order = 42)]
        [StringLength(1)]
        public string MKCOD { get; set; }

        [Key]
        [Column(Order = 43)]
        [StringLength(20)]
        public string MKRIYU { get; set; }

        [Key]
        [Column(Order = 44)]
        [StringLength(1)]
        public string YUBFLG { get; set; }

        [Key]
        [Column(Order = 45)]
        [StringLength(8)]
        public string CRTCOD { get; set; }

        [Key]
        [Column(Order = 46)]
        public DateTime CRTYMD { get; set; }

        [Key]
        [Column(Order = 47)]
        [StringLength(8)]
        public string UPDCOD { get; set; }

        [Key]
        [Column(Order = 48)]
        public DateTime UPDYMD { get; set; }
    }
}
