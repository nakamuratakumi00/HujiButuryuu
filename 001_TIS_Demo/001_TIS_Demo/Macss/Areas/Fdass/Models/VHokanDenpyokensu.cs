namespace Macss.Areas.Fdass.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VHokanDenpyokensu : DbContext
    {
        public VHokanDenpyokensu()
            : base("name=VHokanDenpyokensu")
        {
        }

        public virtual DbSet<v_hokan_denpyokensu> v_hokan_denpyokensu { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.KYUJIT)
                .IsUnicode(false);

            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.HOKANS)
                .IsUnicode(false);

            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.NNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.NIEANT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.NIEKYT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<v_hokan_denpyokensu>()
                .Property(e => e.NIEKIS)
                .IsUnicode(false);
        }
    }
}
