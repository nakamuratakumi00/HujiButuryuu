using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Macss.Areas.Tass.Common
{
    public static class DataUtil
    {
        // 出荷No分割 '-'付与
        public static string GetSyukno(string value)
        {
            if (value.Length <= 5)
            {
                return value;
            }
            else
            {
                return value.Substring(0, value.Length - 5) + "-" + value.Substring(value.Length - 5);
            }
        }

        // 数値 文字位置を返却
        public static int GetNumIndex(string value)
        {
            var maxLen = value.Length;
            for (int i = 0; i < maxLen; i++)
            {
                if (char.IsNumber(value, i))
                {
                    return i;
                }
            }
            return maxLen;
        }

        // 得意先コードと経費負担Noの組み合わせチェック
        public static bool CheckTokuisakiKeifno(string tokuisaki, string keifno)
        {

            if (tokuisaki == null || tokuisaki.Length == 0 || tokuisaki.Length < 4)
            {
                return true;
            }
            string tokuisaki1 = tokuisaki.Substring(0, 2);
            string tokuisaki4 = tokuisaki.Substring(3, 1);
            string tokuisaki14 = tokuisaki.Substring(0, 4);

            if (keifno == null || keifno.Length == 0 || keifno.Length < 5)
            {
                return true;
            }
            string keifno4 = keifno.Substring(3, 2);
            string keifno3 = keifno.Substring(2, 3);
            string keifno15 = keifno.Substring(0, 5);

            if (tokuisaki1 == "15" && tokuisaki4 == "7" && keifno4 == "03")
            {
                return false;
            }

            if (tokuisaki1 == "16" && tokuisaki4 == "7" && keifno4 == "03")
            {
                return false;
            }

            if (tokuisaki1 == "17" && tokuisaki4 == "7" && keifno4 == "03")
            {
                return false;
            }

            if (tokuisaki1 == "15" && tokuisaki4 == "0" && keifno4 == "02")
            {
                return false;
            }

            if (tokuisaki1 == "16" && tokuisaki4 == "0" && keifno4 == "02")
            {
                return false;
            }

            if (tokuisaki1 == "17" && tokuisaki4 == "0" && keifno4 == "02")
            {
                return false;
            }

            if (tokuisaki1 == "35" && tokuisaki4 == "7" && keifno3 == "301")
            {
                return false;
            }

            if (tokuisaki1 == "37" && tokuisaki4 == "7" && keifno3 == "301")
            {
                return false;
            }

            if (tokuisaki1 == "35" && tokuisaki4 == "8" && keifno3 == "005")
            {
                return false;
            }

            if (tokuisaki1 == "37" && tokuisaki4 == "8" && keifno3 == "005")
            {
                return false;
            }

            if (tokuisaki14 == "3570" && keifno15 == "GQ005")
            {
                return false;
            }

            if (tokuisaki14 == "3790" && keifno15 == "DR005")
            {
                return false;
            }

            return true;
        }

        public static string GetCsvString(ClosedXML.Excel.XLWorkbook wb)
        {

            int lastRow = wb.Worksheets.First().LastRowUsed().RowNumber();
            var lastColumn = wb.Worksheets.First().LastColumnUsed().ColumnNumber();
            var sb = new System.Text.StringBuilder();
            for (int row = 1; row <= lastRow; row++)
            {
                bool fFlag = true;
                for (int col = 1; col <= lastColumn; col++)
                {
                    string cell = wb.Worksheets.First().Cell(row, col).Value?.ToString();
                    if (fFlag)
                    {
                        sb.Append("\"" + cell + "\"");
                        fFlag = false;
                    }
                    else
                    {
                        sb.Append(",\"" + cell + "\"");
                    }
                }
                sb.Append("\r\n");
            }

            return sb.ToString();

        }

        public static void SetExelString(ClosedXML.Excel.IXLCell cell, object value)
        {
            if (value.GetType() == typeof(string))
            {
                cell.SetValue(value).SetDataType(XLDataType.Text);
            }
            else
            {
                cell.SetValue(value);
            }

        }

        public static string GetTimeString(string value, bool fromFlg)
        {

            string TimeString = "00:00";
            if (!fromFlg)
            {
                TimeString = "23:59";
            }
            if (value != null)
            {
                TimeString = value;
                if (TimeString.Length == 3)
                {
                    TimeString = "0" + TimeString;
                }
                if (TimeString.Length == 4)
                {
                    TimeString = TimeString.Substring(0, 2) + ":" + TimeString.Substring(2, 2);
                }
            }

            return TimeString;
        }
    }
}