using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Macss.Models
{
    [Table("m_control")]
    public class MControl
    {
        [Description("セクション")]
        [Column("section" ,Order = 0)]
        [Required]
        [Key]
        [Index]
        [MaxLength(100)]
        public string Section { get; set; }

        [Description("区分")]
        [Column("kbn", Order = 1)]
        [Required]
        [Key]
        [Index]
        [MaxLength(4)]
        public string Kbn { get; set; }

        [Description("値1")]
        [Column("value1")]
        [MaxLength(256)]
        public string Value1 { get; set; }

        [Description("値2")]
        [Column("value2")]
        [MaxLength(256)]
        public string Value2 { get; set; }

        [Description("値3")]
        [Column("value3")]
        [MaxLength(256)]
        public string Value3 { get; set; }

        [Description("数値1")]
        [Column("numeric_value1")]
        public decimal? NumericValue1 { get; set; }

        [Description("数値2")]
        [Column("numeric_value2")]
        public decimal? NumericValue2 { get; set; }

        [Description("数値3")]
        [Column("numeric_value3")]
        public decimal? NumericValue3 { get; set; }

        [Description("表示順")]
        [Column("sort_no")]
        public int? SortNo { get; set; }

        [Description("作成者ID")]
        [Column("create_id")]
        [Required]
        [MaxLength(32)]
        public string CreateId { get; set; }

        [Description("作成日")]
        [Column("create_date")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Description("更新者ID")]
        [Column("update_id")]
        [Required]
        [MaxLength(32)]
        public string UpdateId { get; set; }

        [Description("更新日")]
        [Column("update_date")]
        [Required]
        public DateTime UpdateDate { get; set; }
    }
}