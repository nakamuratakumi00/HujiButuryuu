namespace Macss.Areas.Tass.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VUnsouShuukaTehai : DbContext
    {
        public VUnsouShuukaTehai()
            //: base("name=VUnsouShuukaTehai")
            : base("name=ApplicationDB")
            
        {
        }

        public virtual DbSet<v_unsou_shuuka_tehai> v_unsou_shuuka_tehai { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<v_unsou_shuuka_tehai>()
                .Property(e => e.HOSOSU)
                .HasPrecision(7, 0);

            modelBuilder.Entity<v_unsou_shuuka_tehai>()
                .Property(e => e.JYURYO)
                .HasPrecision(9, 2);

            modelBuilder.Entity<v_unsou_shuuka_tehai>()
                .Property(e => e.HOSOS3)
                .HasPrecision(9, 0);

            modelBuilder.Entity<v_unsou_shuuka_tehai>()
                .Property(e => e.JYURY3)
                .HasPrecision(9, 2);

            modelBuilder.Entity<v_unsou_shuuka_tehai>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);
        }
    }
}
