using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Macss.Models.Service
{
    public class UseStatusService
    {
        public static bool UpdateUseStatus(ApplicationDB dbContext, string session_id, string account_id, string title_name, string action_name, string controller_name)
        {
            DateTime oldDate = DateTime.Now.AddHours(-1);
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM t_use_status WHERE update_date <= N'" + oldDate.ToString() + "'");
            dbContext.Database.ExecuteSqlCommand(sql.ToString());

            sql = new StringBuilder();
            sql.Append("SELECT COUNT(*) FROM t_use_status WHERE ");
            sql.Append("session_id = N'" + session_id + "' AND ");
            sql.Append("account_id = N'" + account_id + "'");
            int ret = dbContext.Database.SqlQuery<int>(sql.ToString()).First();
            if (ret == 0)
            {
                sql = new StringBuilder();
                sql.Append(" INSERT INTO t_use_status (");
                sql.Append(" session_id, ");
                sql.Append(" account_id, ");
                sql.Append(" title_name, ");
                sql.Append(" action_name, ");
                sql.Append(" controller_name, ");
                sql.Append(" update_date ");
                sql.Append(") VALUES (");
                sql.Append("N'" + session_id + "',");
                sql.Append("N'" + account_id + "',");
                sql.Append("N'" + title_name + "',");
                sql.Append("N'" + action_name + "',");
                sql.Append("N'" + controller_name + "',");
                sql.Append("N'" + DateTime.Now.ToString() + "'");
                sql.AppendLine(");");
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
            } else
            {
                sql = new StringBuilder();
                sql.Append(" UPDATE t_use_status SET ");
                if (!string.IsNullOrEmpty(title_name)) { 
                    sql.Append(" title_name = N'" + title_name + "',");
                }
                sql.Append(" action_name = N'" + action_name + "',");
                sql.Append(" controller_name = N'" + controller_name + "',");
                sql.Append(" update_date = N'" + DateTime.Now.ToString() + "'");
                sql.Append(" WHERE ");
                sql.Append("session_id = N'" + session_id + "' AND ");
                sql.Append("account_id = N'" + account_id + "'");
                dbContext.Database.ExecuteSqlCommand(sql.ToString());
            }

            return true;
        }

    }
}