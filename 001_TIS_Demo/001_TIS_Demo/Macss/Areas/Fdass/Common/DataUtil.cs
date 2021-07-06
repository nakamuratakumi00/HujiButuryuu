using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Macss.Areas.Fdass.Common
{
    public static class DataUtil
    {
        public static decimal GetDecimal(string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch
            {
                return 0;
            }
        }

        public static decimal GetDecimal(decimal? value)
        {

            if (value == null)
            {
                return 0;
            }
            return (decimal)value;

        }

        public static DateTime? GetDate(string value)
        {
            if (DateTime.TryParseExact(value, "yyyyMMdd", null, DateTimeStyles.None, out DateTime dt))
            {
                return dt.Date;
            }
            else
            {
                return null;
            }
        }

        public static string SubstringEx(string value, int start, int cnt)
        {

            if (value == null)
            {
                return string.Empty;
            }

            try
            {
                return value.Substring(start, cnt);
            }
            catch
            {
                return string.Empty;
            }

        }

        public static int intParseEx(string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch
            {
                return 0;
            }
        }

        public static string PrintEntityValidationErrors(IEnumerable<DbEntityValidationResult> entityValidationErrors)
        {
            foreach (var er in entityValidationErrors)
            {
                foreach (var item in er.ValidationErrors)
                {
                    Console.WriteLine("{0}:{1}", item.PropertyName, item.ErrorMessage);
                    return item.ErrorMessage;
                }
            }
            return string.Empty;
        }

        public static void GetSqlDatta(string log, ref string sql)
        {

            if (log.IndexOf("SELECT") >= 0) { 
                sql = log;
            }
        }

    }
}