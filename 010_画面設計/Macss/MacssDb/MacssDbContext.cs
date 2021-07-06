using Macss.Database.Entity;
using System.Data.Entity;

namespace Macss.Database
{
    public class MacssDbContext : MacssDbContextBase
    {
        public MacssDbContext()
            : base("name=MacssDB")
        { }

        public virtual DbSet<AccountMaster> m_account { get; set; }
        public virtual DbSet<AccountRoleMaster> m_account_role { get; set; }
        public virtual DbSet<m_calendar> m_calendar { get; set; }
        public virtual DbSet<m_control> m_control { get; set; }
        public virtual DbSet<m_group> m_group { get; set; }
        public virtual DbSet<m_hokan_bumon> m_hokan_bumon { get; set; }
        public virtual DbSet<m_hokan_jouken> m_hokan_jouken { get; set; }
        public virtual DbSet<m_hokan_keiyaku> m_hokan_keiyaku { get; set; }
        public virtual DbSet<m_hokan_rireki_bumon> m_hokan_rireki_bumon { get; set; }
        public virtual DbSet<m_hokan_rireki_jouken> m_hokan_rireki_jouken { get; set; }
        public virtual DbSet<m_hokan_rireki_keiyaku> m_hokan_rireki_keiyaku { get; set; }
        public virtual DbSet<m_hokan_rireki_seikyuusaki_change> m_hokan_rireki_seikyuusaki_change { get; set; }
        public virtual DbSet<m_hokan_rireki_tanka> m_hokan_rireki_tanka { get; set; }
        public virtual DbSet<m_hokan_seihin> m_hokan_seihin { get; set; }
        public virtual DbSet<m_hokan_seikyuusaki_change> m_hokan_seikyuusaki_change { get; set; }
        public virtual DbSet<m_hokan_tanka> m_hokan_tanka { get; set; }
        public virtual DbSet<m_kishu> m_kishu { get; set; }
        public virtual DbSet<m_menu> m_menu { get; set; }
        public virtual DbSet<m_menu_role> m_menu_role { get; set; }
        public virtual DbSet<m_role> m_role { get; set; }
        public virtual DbSet<m_seikyusaki> m_seikyusaki { get; set; }
        public virtual DbSet<m_shukkabasho> m_shukkabasho { get; set; }
        public virtual DbSet<m_torihikisaki> m_torihikisaki { get; set; }
        public virtual DbSet<m_unsou_hinmei> m_unsou_hinmei { get; set; }
        public virtual DbSet<m_unsou_hinmei_koyuu> m_unsou_hinmei_koyuu { get; set; }
        public virtual DbSet<m_unsou_jiscode> m_unsou_jiscode { get; set; }
        public virtual DbSet<m_unsou_postalcode> m_unsou_postalcode { get; set; }
        public virtual DbSet<m_unsou_shuuka_tyuumonsho_pattern> m_unsou_shuuka_tyuumonsho_pattern { get; set; }
        public virtual DbSet<m_unsou_todokesaki> m_unsou_todokesaki { get; set; }
        public virtual DbSet<m_unsou_todokesaki_koyuu> m_unsou_todokesaki_koyuu { get; set; }
        public virtual DbSet<m_unsou_unsouhouhou> m_unsou_unsouhouhou { get; set; }
        public virtual DbSet<m_unsou_unsoukousu> m_unsou_unsoukousu { get; set; }
        public virtual DbSet<m_unsou_unsoukubun> m_unsou_unsoukubun { get; set; }
        public virtual DbSet<t_hokan_denpyokensu> t_hokan_denpyokensu { get; set; }
        public virtual DbSet<t_hokan_denpyokensu_kurikosi> t_hokan_denpyokensu_kurikosi { get; set; }
        public virtual DbSet<t_hokan_matujime_kanri> t_hokan_matujime_kanri { get; set; }
        public virtual DbSet<t_hokan_nyuushuuko> t_hokan_nyuushuuko { get; set; }
        public virtual DbSet<t_hokan_nyuushuuko_kurikosi> t_hokan_nyuushuuko_kurikosi { get; set; }
        public virtual DbSet<t_hokan_rireki_denpyokensu> t_hokan_rireki_denpyokensu { get; set; }
        public virtual DbSet<t_hokan_rireki_denpyokensu_kurikosi> t_hokan_rireki_denpyokensu_kurikosi { get; set; }
        public virtual DbSet<t_hokan_rireki_nyuushuuko> t_hokan_rireki_nyuushuuko { get; set; }
        public virtual DbSet<t_hokan_rireki_nyuushuuko_kurikosi> t_hokan_rireki_nyuushuuko_kurikosi { get; set; }
        public virtual DbSet<t_hokan_rireki_seikyu> t_hokan_rireki_seikyu { get; set; }
        public virtual DbSet<t_hokan_rireki_seikyu_b> t_hokan_rireki_seikyu_b { get; set; }
        public virtual DbSet<t_hokan_rireki_seikyu_kyoten> t_hokan_rireki_seikyu_kyoten { get; set; }
        public virtual DbSet<t_hokan_seikyu> t_hokan_seikyu { get; set; }
        public virtual DbSet<t_hokan_seikyu_b> t_hokan_seikyu_b { get; set; }
        public virtual DbSet<t_hokan_seikyu_kyoten> t_hokan_seikyu_kyoten { get; set; }
        public virtual DbSet<t_log_history> t_log_history { get; set; }
        public virtual DbSet<t_password_history> t_password_history { get; set; }
        public virtual DbSet<t_unsou_rireki_shuuka_jiseki> t_unsou_rireki_shuuka_jiseki { get; set; }
        public virtual DbSet<t_unsou_shuuka_jiseki1> t_unsou_shuuka_jiseki1 { get; set; }
        public virtual DbSet<t_unsou_shuuka_jiseki2> t_unsou_shuuka_jiseki2 { get; set; }
        public virtual DbSet<t_unsou_shuuka_tehai_all> t_unsou_shuuka_tehai_all { get; set; }
        public virtual DbSet<t_unsou_shuuka_tyuumonsho_tehai> t_unsou_shuuka_tyuumonsho_tehai { get; set; }
        public virtual DbSet<t_unsou_shuuka_tyuumonsho_tehai_k> t_unsou_shuuka_tyuumonsho_tehai_k { get; set; }
        public virtual DbSet<t_unsou_shuuka_tyuumonsho_tehai_meisai> t_unsou_shuuka_tyuumonsho_tehai_meisai { get; set; }
        public virtual DbSet<t_unsou_shuuka_tyuumonsho_tehai_mk> t_unsou_shuuka_tyuumonsho_tehai_mk { get; set; }
        public virtual DbSet<t_unsou_shuukatehai> t_unsou_shuukatehai { get; set; }
        public virtual DbSet<t_use_status> t_use_status { get; set; }
        public virtual DbSet<w_hokan_denpyokensu> w_hokan_denpyokensu { get; set; }
        public virtual DbSet<w_hokan_nyuushuuko> w_hokan_nyuushuuko { get; set; }
        public virtual DbSet<w_hokan_seihin> w_hokan_seihin { get; set; }
        public virtual DbSet<w_unsou_shuuka_tyuumonsho_tehai_kouho> w_unsou_shuuka_tyuumonsho_tehai_kouho { get; set; }
        public virtual DbSet<w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho> w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho { get; set; }
        public virtual DbSet<v_hokan_denpyokensu> v_hokan_denpyokensu { get; set; }
        public virtual DbSet<v_hokan_nyuushuuko> v_hokan_nyuushuuko { get; set; }
        public virtual DbSet<v_hokan_tanka> v_hokan_tanka { get; set; }
        public virtual DbSet<v_table_lock> v_table_lock { get; set; }
        public virtual DbSet<v_unsou_hinmei> v_unsou_hinmei { get; set; }
        public virtual DbSet<v_unsou_shuuka_tehai> v_unsou_shuuka_tehai { get; set; }
        public virtual DbSet<v_unsou_shuukatehai> v_unsou_shuukatehai { get; set; }
        public virtual DbSet<v_unsou_todokesaki> v_unsou_todokesaki { get; set; }


        public virtual DbSet<DummyStock> t_stock { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountMaster>()
                .HasMany(e => e.m_account_role)
                .WithRequired(e => e.m_account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.HNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.NIEANT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.NIEKYT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.NNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.OJYUKR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.OSYJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.HJYUKR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_keiyaku>()
                .Property(e => e.HSYJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.HNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.NIEANT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.NIEKYT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.NNEBIR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.OJYUKR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.OSYJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.HJYUKR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_keiyaku>()
                .Property(e => e.HSYJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.JYUKAR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.SYUJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.FPTANK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.NIEKIT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.OFPTNK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.OHOKAT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_rireki_tanka>()
                .Property(e => e.ONIEKT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_seihin>()
                .Property(e => e.FPTANK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<m_hokan_seihin>()
                .Property(e => e.ZAIKSU)
                .HasPrecision(11, 2);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.JYUKAR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.SYUJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.FPTANK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.NIEKIT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.OFPTNK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.OHOKAT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_hokan_tanka>()
                .Property(e => e.ONIEKT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<m_menu>()
                .HasMany(e => e.m_menu_role)
                .WithRequired(e => e.m_menu)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<m_role>()
                .HasMany(e => e.m_account_role)
                .WithRequired(e => e.m_role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<m_role>()
                .HasMany(e => e.m_menu_role)
                .WithRequired(e => e.m_role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<m_unsou_shuuka_tyuumonsho_pattern>()
                .Property(e => e.SYKSU1)
                .HasPrecision(11, 0);

            modelBuilder.Entity<m_unsou_shuuka_tyuumonsho_pattern>()
                .Property(e => e.SYKSU2)
                .HasPrecision(11, 0);

            modelBuilder.Entity<m_unsou_shuuka_tyuumonsho_pattern>()
                .Property(e => e.SYKSU3)
                .HasPrecision(11, 0);

            modelBuilder.Entity<m_unsou_shuuka_tyuumonsho_pattern>()
                .Property(e => e.SYKSU4)
                .HasPrecision(11, 0);

            modelBuilder.Entity<m_unsou_shuuka_tyuumonsho_pattern>()
                .Property(e => e.SYKSU5)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_denpyokensu>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_denpyokensu>()
                .Property(e => e.NSKOSU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_denpyokensu_kurikosi>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_denpyokensu_kurikosi>()
                .Property(e => e.NSKOSU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.ZANSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.ZANKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.SIKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.SIKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.KAISUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.KAIKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.SYKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.SYKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko>()
                .Property(e => e.TOUZAN)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.ZANSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.ZANKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.SIKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.SIKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.KAISUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.KAIKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.SYKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.SYKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_nyuushuuko_kurikosi>()
                .Property(e => e.TOUZAN)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_denpyokensu>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_rireki_denpyokensu>()
                .Property(e => e.NSKOSU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_denpyokensu_kurikosi>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_rireki_denpyokensu_kurikosi>()
                .Property(e => e.NSKOSU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.ZANSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.ZANKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.SIKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.SIKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.KAISUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.KAIKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.SYKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.SYKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko>()
                .Property(e => e.TOUZAN)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.ZANSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.ZANKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.SIKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.SIKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.KAISUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.KAIKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.SYKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.SYKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_hokan_rireki_nyuushuuko_kurikosi>()
                .Property(e => e.TOUZAN)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.ZANSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.NYUKSU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.SYKSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.SEKISU)
                .HasPrecision(10, 2);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.HOKANK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.HOKANR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.ATUKAI)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.DENSUU)
                .HasPrecision(8, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.NIEKIT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.NIEKIK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu>()
                .Property(e => e.NIEKIKR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.ZANSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.NYUKSU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.SYKSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.SEKISU)
                .HasPrecision(10, 2);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.HOKANK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.HOKANKR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.ATUKAI)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.DENSUU)
                .HasPrecision(8, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.NIEKIT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.NIEKIK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_b>()
                .Property(e => e.NIEKIR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.ZANSUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.ZANKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.SIKSUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.SIKKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.KAISUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.KAIKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.NYUKSU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.NYUKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.SYKSUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.SYKKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.ZAIKSU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.ZAIKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.DENSKY)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.HOKANK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.NIEKIK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_rireki_seikyu_kyoten>()
                .Property(e => e.NIEKYJ)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.ZANSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.NYUKSU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.SYKSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.SEKISU)
                .HasPrecision(10, 2);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.HOKANK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.HOKANKR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.ATUKAI)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.DENSUU)
                .HasPrecision(8, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.NIEKIT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.NIEKIK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu>()
                .Property(e => e.NIEKIKR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.ZANSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.NYUKSU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.SYKSUU)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.SEKISU)
                .HasPrecision(10, 2);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.HOKANK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.HOKANKR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.ATUKAI)
                .HasPrecision(10, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.DENSUU)
                .HasPrecision(8, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.NIEKIT)
                .HasPrecision(9, 4);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.NIEKIK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_b>()
                .Property(e => e.NIEKIKR)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.ZANSUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.ZANKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.SIKSUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.SIKKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.KAISUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.KAIKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.NYUKSU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.NYUKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.SYKSUU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.SYKKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.ZAIKSU)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.ZAIKIN)
                .HasPrecision(12, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.DENSKY)
                .HasPrecision(6, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.HOKANK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.NIEKIK)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_hokan_seikyu_kyoten>()
                .Property(e => e.NIEKYJ)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_unsou_rireki_shuuka_jiseki>()
                .Property(e => e.HOSOSU)
                .HasPrecision(7, 0);

            modelBuilder.Entity<t_unsou_rireki_shuuka_jiseki>()
                .Property(e => e.JYURYO)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_rireki_shuuka_jiseki>()
                .Property(e => e.HOSOS3)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_unsou_rireki_shuuka_jiseki>()
                .Property(e => e.JYURY3)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_rireki_shuuka_jiseki>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_jiseki1>()
                .Property(e => e.HOSOSU)
                .HasPrecision(7, 0);

            modelBuilder.Entity<t_unsou_shuuka_jiseki1>()
                .Property(e => e.JYURYO)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_shuuka_jiseki1>()
                .Property(e => e.HOSOS3)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_unsou_shuuka_jiseki1>()
                .Property(e => e.JYURY3)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_shuuka_jiseki1>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_jiseki2>()
                .Property(e => e.HOSOSU)
                .HasPrecision(7, 0);

            modelBuilder.Entity<t_unsou_shuuka_jiseki2>()
                .Property(e => e.JYURYO)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_shuuka_jiseki2>()
                .Property(e => e.HOSOS3)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_unsou_shuuka_jiseki2>()
                .Property(e => e.JYURY3)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_shuuka_jiseki2>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_tehai_all>()
                .Property(e => e.HOSOSU)
                .HasPrecision(7, 0);

            modelBuilder.Entity<t_unsou_shuuka_tehai_all>()
                .Property(e => e.JYURYO)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_shuuka_tehai_all>()
                .Property(e => e.HOSOS3)
                .HasPrecision(9, 0);

            modelBuilder.Entity<t_unsou_shuuka_tehai_all>()
                .Property(e => e.JYURY3)
                .HasPrecision(9, 2);

            modelBuilder.Entity<t_unsou_shuuka_tehai_all>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_tyuumonsho_tehai>()
                .Property(e => e.DSYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_tyuumonsho_tehai_k>()
                .Property(e => e.DSYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_tyuumonsho_tehai_meisai>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuuka_tyuumonsho_tehai_mk>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<t_unsou_shuukatehai>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_denpyokensu>()
                .Property(e => e.ID)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_denpyokensu>()
                .Property(e => e.DENSUU)
                .HasPrecision(6, 0);

            modelBuilder.Entity<w_hokan_denpyokensu>()
                .Property(e => e.NSKOSU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.ID)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.ZANSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.ZANKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.SIKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.SIKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.KAISUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.KAIKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.SYKSUU)
                .HasPrecision(9, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.SYKKIN)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_nyuushuuko>()
                .Property(e => e.TOUZAN)
                .HasPrecision(9, 0);

            modelBuilder.Entity<w_hokan_seihin>()
                .Property(e => e.ID)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_hokan_seihin>()
                .Property(e => e.FPTANK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<w_hokan_seihin>()
                .Property(e => e.ZAIKSU)
                .HasPrecision(11, 2);

            modelBuilder.Entity<w_unsou_shuuka_tyuumonsho_tehai_kouho>()
                .Property(e => e.ACKYMD)
                .HasPrecision(17, 0);

            modelBuilder.Entity<w_unsou_shuuka_tyuumonsho_tehai_kouho>()
                .Property(e => e.DSYUKSU)
                .HasPrecision(11, 0);

            modelBuilder.Entity<w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho>()
                .Property(e => e.ACKYMD)
                .HasPrecision(17, 0);

            modelBuilder.Entity<w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);

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

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.JYUKAR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.SYUJYR)
                .HasPrecision(5, 3);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.FPTANK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.HOKANT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.NIEKIT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.OFPTNK)
                .HasPrecision(10, 2);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.OHOKAT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<v_hokan_tanka>()
                .Property(e => e.ONIEKT)
                .HasPrecision(10, 5);

            modelBuilder.Entity<v_table_lock>()
                .Property(e => e.ProcessName)
                .IsFixedLength();

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

            modelBuilder.Entity<v_unsou_shuukatehai>()
                .Property(e => e.SYUKSU)
                .HasPrecision(11, 0);
        }
    }

}
