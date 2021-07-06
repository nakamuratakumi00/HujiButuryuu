namespace Macss.Areas.Fdass.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Macss.Models;

    public partial class VHokanNyuushuuko : DbContext
    {
        public VHokanNyuushuuko()
            //: base("name=VHokanNyuushuuko")
            : base("name=ApplicationDB")
        {
        }

        public virtual DbSet<v_hokan_nyuushuuko> v_hokan_nyuushuuko { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.ZANSUU)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.ZANKIN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.SIKSUU)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.SIKKIN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.KAISUU)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.KAIKIN)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.DSYKSUU)
                .HasPrecision(38, 0);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.SYKKIN)
                .HasPrecision(38, 2);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.HNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.HOKANS)
                .IsUnicode(false);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.NNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.NIEKIT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<v_hokan_nyuushuuko>()
                .Property(e => e.NIEKIS)
                .IsUnicode(false);
        }
    }
}
