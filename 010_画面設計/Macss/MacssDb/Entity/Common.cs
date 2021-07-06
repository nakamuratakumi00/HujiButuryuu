using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Macss.Database.Entity
{
    /// <summary>
    /// 共通の列を定義
    /// </summary>
    public class Common
    {
        public const string CodeNameFormat = "{0}\t\t\t{1}";

        [Key]
        [Column("id", Order = 0)]
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("created_user", Order = 1)]
        public string CreatedUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("created_at", Order = 2)]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("updated_user", Order = 3)]
        public string UpdatedUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [ConcurrencyCheck]
        [Column("updated_at", Order = 4)]
        public DateTime UpdatedAt { get; set; }


    }
}
