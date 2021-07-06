namespace Macss.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class VTableLock : DbContext
    {
        public VTableLock()
            : base("name=VTableLock")
        {
        }

        public virtual DbSet<v_table_lock> v_table_lock { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<v_table_lock>()
                .Property(e => e.ProcessName)
                .IsFixedLength();
        }
    }
}
