namespace Macss.Models
{
    using Macss.Areas.Fdass.Models;
    using System.Data.Entity;
    using Macss.Attributes.Custom;
    using Macss.Areas.Tass.Models;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Data.Entity.Validation;
    using Macss.Areas.Fdass.Common;
    using log4net;

    public class ApplicationDB : DbContext
    {
        // コンテキストは、アプリケーションの構成ファイル (App.config または Web.config) から 'ApplicationDB' 
        // 接続文字列を使用するように構成されています。既定では、この接続文字列は LocalDb インスタンス上
        // の 'Macss.Models.ApplicationDB' データベースを対象としています。 
        // 
        // 別のデータベースとデータベース プロバイダーまたはそのいずれかを対象とする場合は、
        // アプリケーション構成ファイルで 'ApplicationDB' 接続文字列を変更してください。
        public ApplicationDB()
            : base("name=ApplicationDB")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DefaultValueAttributeConvention());
            modelBuilder.Conventions.Add(new CustomAttributes.DecimalPrecisionAttributeConvention());
            base.OnModelCreating(modelBuilder);
        }


        public Task<int> SaveChangesAsyncEx()
        {
            try
            {
                CheckCurrentValue();
                base.Configuration.ValidateOnSaveEnabled = false;
                return base.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMsg = DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error(errorMsg);
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Configuration.ValidateOnSaveEnabled = true;
            }

        }

        public int SaveChangesEx()
        {
            try
            {
                CheckCurrentValue();
                base.Configuration.ValidateOnSaveEnabled = false;
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string errorMsg = DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                var logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error(errorMsg);
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Configuration.ValidateOnSaveEnabled = true;
            }

        }

        private void CheckCurrentValue()
        {
            DateTime dataTime = DateTime.Parse("2000/01/01");
            foreach (System.Data.Entity.Infrastructure.DbEntityEntry entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var type = entry.Entity.GetType();
                System.Reflection.PropertyInfo[] Properties = type.GetProperties();
                foreach (System.Reflection.PropertyInfo propertie in Properties)
                {
                    if (propertie.PropertyType == typeof(decimal) || propertie.PropertyType == typeof(decimal?))
                    {
                        if (entry.Property(propertie.Name).CurrentValue == null)
                        {
                            entry.Property(propertie.Name).CurrentValue = 0m;
                        }
                    }

                    if (propertie.PropertyType == typeof(int))
                    {
                        if (entry.Property(propertie.Name).CurrentValue == null)
                        {
                            entry.Property(propertie.Name).CurrentValue = 0;
                        }
                    }

                    if (propertie.PropertyType == typeof(string))
                    {
                        if (entry.Property(propertie.Name).CurrentValue == null)
                        {
                            entry.Property(propertie.Name).CurrentValue = "";
                        }
                    }

                    if (propertie.PropertyType == typeof(DateTime) || propertie.PropertyType == typeof(DateTime?))
                    {
                        if (entry.Property(propertie.Name).CurrentValue == null)
                        {
                            entry.Property(propertie.Name).CurrentValue = dataTime;
                        }
                    }
                }
            }
        }


        // モデルに含めるエンティティ型ごとに DbSet を追加します。Code First モデルの構成および使用の
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=390109 を参照してください。

        public virtual DbSet<MAccount> MAccount { get; set; }
        public virtual DbSet<MControl> MControl { get; set; }
        public virtual DbSet<MGroup> MGroup { get; set; }
        public virtual DbSet<MAccountRole> MAccountRole { get; set; }
        public virtual DbSet<MMenu> MMenu { get; set; }
        public virtual DbSet<TLogHistory> TLogHistory { get; set; }
        public virtual DbSet<MRole> MRole { get; set; }
        public virtual DbSet<MMenuRole> MMenuRole { get; set; }
        public virtual DbSet<TPasswordHistory> TPasswordHistory { get; set; }
        public virtual DbSet<TUseStatus> TUseStatus { get; set; }

        // 共通
        public virtual DbSet<MShukkabasho> MShukkabasho { get; set; }
        public virtual DbSet<MTorihikisaki> MTorihikisaki { get; set; }
        public virtual DbSet<MCalendar> MCalendar { get; set; }
        public virtual DbSet<MSeikyusaki> MSeikyusaki { get; set; }
        public virtual DbSet<MKishu> MKishu { get; set; }
        // View　(Viewはマイグレーションで作成しないでください)
        public virtual DbSet<v_table_lock> VHTableLock { get; set; }

        // Fe保管請求システム
        // マスタ
        public virtual DbSet<MHokanKeiyaku> MHokanKeiyaku { get; set; }
        public virtual DbSet<MHokanJouken> MHokanJouken { get; set; }
        public virtual DbSet<MHokanTanka> MHokanTanka { get; set; }
        public virtual DbSet<MHokanSeihin> MHokanSeihin { get; set; }
        public virtual DbSet<MHokanSeikyuusakiChange> MHokanSeikyuusakiChange { get; set; }
        public virtual DbSet<MHokanBumon> MHokanBumon { get; set; }
        // トラン
        public virtual DbSet<THokanMatujimeKanri> THokanMatujimeKanri { get; set; }
        public virtual DbSet<THokanNyuushuuko> THokanNyuushuuko { get; set; }
        public virtual DbSet<THokanNyuushuukoKurikosi> THokanNyuushuukoKurikosi { get; set; }
        public virtual DbSet<THokanDenpyokensu> THokanDenpyokensu { get; set; }
        public virtual DbSet<THokanDenpyokensuKurikosi> THokanDenpyokensuKurikosi { get; set; }
        public virtual DbSet<THokanSeikyu> THokanSeikyu { get; set; }
        public virtual DbSet<THokanSeikyuB> THokanSeikyuB { get; set; }
        public virtual DbSet<THokanSeikyuKyoten> THokanSeikyuKyoten { get; set; }        
        // ワーク
        public virtual DbSet<WHokanSeihin> WHokanSeihin { get; set; }
        public virtual DbSet<WHokanNyuushuuko> WHokanNyuushuuko { get; set; }
        public virtual DbSet<WHokanDenpyokensu> WHokanDenpyokensu { get; set; }
        // View　(Viewはマイグレーションで作成しないでください)
        public virtual DbSet<v_hokan_nyuushuuko> VHokanNyuushuuko { get; set; }
        public virtual DbSet<v_hokan_denpyokensu> VHokanDenpyokensu { get; set; }
        public virtual DbSet<v_unsou_shuuka_tehai> VUnsouShuukaTehai { get; set; }
        public virtual DbSet<v_unsou_hinmei> VUnsouHinmei { get; set; }
        public virtual DbSet<v_unsou_todokesaki> VUnsouTodokesaki { get; set; }
        
        // 退避用テーブル
        public virtual DbSet<THokanRirekiDenpyokensu> THokanRirekiDenpyokensu { get; set; }
        public virtual DbSet<THokanRirekiDenpyokensuKurikosi> THokanRirekiDenpyokensuKurikosi { get; set; }
        public virtual DbSet<THokanRirekiNyuushuuko> THokanRirekiNyuushuuko { get; set; }
        public virtual DbSet<THokanRirekiNyuushuukoKurikosi> THokanRirekiNyuushuukoKurikosi { get; set; }
        public virtual DbSet<THokanRirekiSeikyu> THokanRirekiSeikyu { get; set; }
        public virtual DbSet<THokanRirekiSeikyuB> THokanRirekiSeikyuB { get; set; }
        public virtual DbSet<THokanRirekiSeikyuKyoten> THokanRirekiSeikyuKyoten { get; set; }
        public virtual DbSet<MHokanRirekiJouken> MHokanRirekiJouken { get; set; }
        public virtual DbSet<MHokanRirekiKeiyaku> MHokanRirekiKeiyaku { get; set; }
        public virtual DbSet<MHokanRirekiSeikyuusakiChange> MHokanRirekiSeikyuusakiChange { get; set; }
        public virtual DbSet<MHokanRirekiTanka> MHokanRirekiTanka { get; set; }
        public virtual DbSet<MHokanRirekiBumon> MHokanRirekiBumon { get; set; }        

        // 運送業務システム
        // マスタ
        public virtual DbSet<MUnsouHinmeiKoyuu> MUnsouHinmeiKoyuu { get; set; }
        public virtual DbSet<MUnsouTodokesakiKoyuu> MUnsouTodokesakiKoyuu { get; set; }
        public virtual DbSet<MUnsouShuukaTyuumonshoPattern> MUnsouShuukaTyuumonshoPattern { get; set; }
        public virtual DbSet<MUnsouTodokesaki> MUnsouTodokesaki { get; set; }
        public virtual DbSet<MUnsouHinmei> MUnsouHinmei { get; set; }
        public virtual DbSet<MUnsouUnsoukousu> MUnsouUnsoukousu { get; set; }
        public virtual DbSet<MUnsouUnsouhouhou> MUnsouUnsouhouhou { get; set; }
        public virtual DbSet<MUnsouUnsoukubun> MUnsouUnsoukubun { get; set; }
        public virtual DbSet<MUnsouPostalcode> MUnsouPostalcode { get; set; }
        public virtual DbSet<MUnsouJiscode> MUnsouJiscode { get; set; }
        
        // トラン
        public virtual DbSet<TUnsouShuukaTyuumonshoTehaiKouho> TUnsouShuukaTyuumonshoTehaiKouho { get; set; }
        public virtual DbSet<TUnsouShuukaTyuumonshoTehaiMeisaiKouho> TUnsouShuukaTyuumonshoTehaiMeisaiKouho { get; set; }
        public virtual DbSet<TUnsouShuukaTyuumonshoTehai> TUnsouShuukaTyuumonshoTehai { get; set; }
        public virtual DbSet<TUnsouShuukaTyuumonshoTehaiMeisai> TUnsouShuukaTyuumonshoTehaiMeisai { get; set; }
        public virtual DbSet<TUnsouShuukatehai> TUnsouShuukatehai { get; set; }
        public virtual DbSet<TUnsouShuukaTehaiAll> YUnsouShuukaTehaiAll { get; set; }
        public virtual DbSet<TUnsouShuukaJiseki1> TUnsouShuukaJiseki1 { get; set; }
        public virtual DbSet<TUnsouShuukaJiseki2> TUnsouShuukaJiseki2 { get; set; }
        public virtual DbSet<TUnsouRirekiShuukaJiseki> TUnsouRirekiShuukaJiseki { get; set; }

        // ワーク
        public virtual DbSet<WUnsouShuukaTyuumonshoTehaiKouho> WUnsouShuukaTyuumonshoTehaiKouho { get; set; }
        public virtual DbSet<WUnsouShuukaTyuumonshoTehaiMeisaiKouho> WUnsouShuukaTyuumonshoTehaiMeisaiKouho { get; set; }

    }

}