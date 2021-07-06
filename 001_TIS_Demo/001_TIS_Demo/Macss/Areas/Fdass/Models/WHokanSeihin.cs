﻿using Macss.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Areas.Fdass.Models
{

    [Table("w_hokan_seihin")]
    public class WHokanSeihin
    {

        [Description("ID")]
        [Column("ID", Order = 0)]
        [Required]
        [Key]
        [CustomAttributes.DecimalPrecisionAttribute(11, 0)]
        public decimal? Id { get; set; }

        [Description("品番コード")]
        [Column("HINCOD", Order = 1)]
        [Required]
        [Key]
        [MaxLength(8)]
        public string Hincod { get; set; }

        [Description("品名型式（日本語）")]
        [Column("HINNAM")]
        [MaxLength(80)]
        public string Hinnam { get; set; }

        [Description("品名型式（カナ）")]
        [Column("HINNMK")]
        [MaxLength(80)]
        public string Hinnmk { get; set; }

        [Description("機種Ａ")]
        [Column("KISYUA")]
        [MaxLength(2)]
        public string Kisyua { get; set; }

        [Description("機種Ｂ")]
        [Column("KISYUB")]
        [MaxLength(6)]
        public string Kisyub { get; set; }

        [Description("出荷場所")]
        [Column("SYBCOD")]
        [MaxLength(2)]
        public string Sybcod { get; set; }

        [Description("ＦＰ単価")]
        [Column("FPTANK")]
        [CustomAttributes.DecimalPrecisionAttribute(10, 2)]
        public decimal? Fptank { get; set; }

        [Description("最終メンテ日")]
        [Column("MENTBI")]
        public DateTime? Mentbi { get; set; }

        [Description("暫定登録区分")]
        [Column("ZANTEI")]
        [MaxLength(1)]
        public string Zantei { get; set; }

        [Description("在庫数")]
        [Column("ZAIKSU")]
        [CustomAttributes.DecimalPrecisionAttribute(11, 2)]
        public decimal? Zaiksu { get; set; }

        [Description("保管場所")]
        [Column("HOKCOD")]
        [MaxLength(2)]
        public string Hokcod { get; set; }

        [Description("振替区分")]
        [Column("FRIKAE")]
        [MaxLength(1)]
        public string Frikae { get; set; }

        [Description("データ管理元")]
        [Column("DTMOTO")]
        [MaxLength(1)]
        public string Dtmoto { get; set; }

        [Description("登録担当")]
        [Column("CRTCOD")]
        [MaxLength(8)]
        public string Crtcod { get; set; }

        [Description("登録日")]
        [Column("CRTYMD")]
        public DateTime? Crtymd { get; set; }

        [Description("更新担当")]
        [Column("UPDCOD")]
        [MaxLength(8)]
        public string Updcod { get; set; }

        [Description("更新日")]
        [Column("UPDYMD")]
        public DateTime? Updymd { get; set; }

    }


}