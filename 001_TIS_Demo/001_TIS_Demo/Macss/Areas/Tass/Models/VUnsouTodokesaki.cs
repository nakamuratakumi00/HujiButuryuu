namespace Macss.Areas.Tass.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VUnsouTodokesaki : DbContext
    {
        public VUnsouTodokesaki()
            : base("name=VUnsouTodokesaki")
        {
        }

        public virtual DbSet<v_unsou_todokesaki> v_unsou_todokesaki { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
