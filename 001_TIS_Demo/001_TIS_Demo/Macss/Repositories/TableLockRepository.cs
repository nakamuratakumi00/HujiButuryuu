using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.App_Start;
using Macss.Models;
using Macss.ViewModels;
using System.Data.Entity;

namespace Macss.Repositories
{
    public class TableLockRepository : ITableLockRepository
    {
        private readonly ApplicationDB dbContext;

        public TableLockRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<bool> CheckTableLock(string tableName)
        {
            List<v_table_lock> tableLocks = await dbContext.VHTableLock.Where(x => x.object_name == tableName).ToListAsync();

            if (tableLocks != null && tableLocks.Count != 0)
            {
                var tableLock = tableLocks.First();
                if (tableLock.request_mode == "X") return true;
            }

            return false;
        }
    }
}