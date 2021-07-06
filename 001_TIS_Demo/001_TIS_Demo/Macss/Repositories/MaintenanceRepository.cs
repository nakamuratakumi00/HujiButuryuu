using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Macss.Models;

namespace Macss.Repositories
{
    public class MaintenanceRepository : IMaintenanceRepository
    {
        private readonly ApplicationDB dbContext;

        public MaintenanceRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public DataTable GetMasterData(string tableName, string where, string order)
        {

            var connectionString = ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString;
            var table = new DataTable();
            try
            {
                //IEnumerable<MAccount> results = await
                //    //await dbContext.Database.SqlQuery<DbSet>(@"SELECT * FROM " + tableName).ToArrayAsync();
                // dbContext.Database.SqlQuery<MAccount>(@"SELECT * FROM m_account").ToArrayAsync();
                using (var connection = new SqlConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    // SQLの設定
                    command.CommandText = @"SELECT * FROM " + tableName;
                    if (where != "")
                    {
                        command.CommandText = command.CommandText + where;
                    }
                    if (order != "")
                    {
                        command.CommandText = command.CommandText + order;
                    }

                    // SQLの実行
                    var adapter = new SqlDataAdapter(command);
                    adapter.Fill(table);

                    }
                return  table;
            }
            catch
            {
                throw; 
            }
        }
        
        public void ExecuteCommand(string sql)
        {
            dbContext.Database.ExecuteSqlCommand(sql);
            dbContext.SaveChanges();
        }

        public int ExecuteCommandExt(string sql)
        {
            int ret = dbContext.Database.ExecuteSqlCommand(sql);
            if (ret == 0)
            {
                return ret;
            }
            dbContext.SaveChanges();
            return ret;
        }

    }
}
