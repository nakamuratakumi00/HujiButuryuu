using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public interface IMaintenanceRepository
    {
        DataTable GetMasterData(string selectSql, string where, string order);

        void ExecuteCommand(string sql);

        int ExecuteCommandExt(string sql);
    }
}