namespace Macss.Areas.Tass.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VUnsouHinmei : DbContext
    {
        public VUnsouHinmei()
            : base("name=VUnsouHinmei")
        {
        }

        public virtual DbSet<v_unsou_hinmei> v_unsou_hinmei { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
