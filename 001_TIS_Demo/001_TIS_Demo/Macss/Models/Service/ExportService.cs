using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Macss.Models.Service
{
    public class ExportService
    {
        public static System.IO.MemoryStream Export<T>(IEnumerable<T> list,  string extention)
        {
            var fs = new System.IO.MemoryStream();

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.AddWorksheet("Sheet1");
                using (var ws = wb.Worksheets.First())
                {
                    OutputDetail(list, ws);
                    ws.ColumnsUsed().AdjustToContents();
                
                    using (fs)
                    {
                        // Excel
                        if (extention == "excel")
                        {
                            wb.SaveAs(fs);
                            fs.Position = 0;
                            return fs;
                        }
                        // CSV
                        else
                        {
                            var lastCellAddress = wb.Worksheets.First().RangeUsed().LastCell().Address;
                            var csvWriter = new StreamWriter(fs, System.Text.Encoding.GetEncoding("shift-jis"));
                            var test = wb.Worksheets.First().Rows(1, lastCellAddress.RowNumber)
                                .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                                    .Select(cell =>
                                    {
                                        var cellValue = cell.GetValue<string>();
                                        return cellValue.Contains(",") ? $"\"{cellValue}\"" : "\"" + cellValue + "\"";
                                    })));
                            for (int i = 0; i < test.ToArray().Length; i++)
                            {
                                csvWriter.Write(test.ElementAt(i));
                                csvWriter.Write("\r\n");
                            }
                            csvWriter.Flush();
                            return fs;
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }

        private static void OutputDetail<T>(IEnumerable<T> list, IXLWorksheet ws)
        {
            int col = 1;
            var properties = GetProperties(typeof(T));

            // 1行目（項目名）
            foreach (var pi in properties)
            {
                SetHeader(ws, 1, ref col, 0, pi);
            }

            int row = 1;

            // 2行目以降（データ）
            foreach (var data in list)
            {
                row++;
                col = 1;
                foreach (var pi in properties)
                {
                    SetRowData(ws, row, ref col, pi, data);
                }
            }
        }

        private static void SetRowData(IXLWorksheet ws, int row, ref int col, PropertyInfo pi, object data)
        {
            var props = GetProperties(pi);
            if (props.Any())
            {
                foreach (var p in props)
                {
                    SetRowData(ws, row, ref col, p, pi.GetValue(data));
                }
            }
            else
            {
                ws.Cell(row, col).SetValue(pi.GetValue(data));
                col++;
            }
        }

        private static void SetHeader(IXLWorksheet ws, int row, ref int col, int height, PropertyInfo pi)
        {
            if (GetProperties(pi).Any())
            {
                int width = GetWidth(pi) - 1;
                if (width > 0)
                {
                    ws.Range(row, col, row, col + width).Merge();
                }
                ws.Cell(row, col).Value = GetDisplayName(pi);

                foreach (var p in GetProperties(pi.PropertyType))
                {
                    SetHeader(ws, row + 1, ref col, height - 1, p);
                }
            }
            else
            {
                if (height > 0)
                {
                    ws.Range(row, col, row + height, col).Merge();
                }
                ws.Cell(row, col).Value = GetDisplayName(pi);
                col++;
            }
        }

        private static List<PropertyInfo> GetProperties(Type type)
        {
            return type.GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(DisplayAttribute)))
                                       .ToList();
        }

        private static List<PropertyInfo> GetProperties(PropertyInfo pi)
        {
            return GetProperties(pi.PropertyType);
        }

        private static string GetDisplayName(PropertyInfo pi)
        {
            var display = System.Attribute.GetCustomAttribute(pi, typeof(DisplayAttribute)) as DisplayAttribute;
            if (display != null)
            {
                return String.IsNullOrEmpty(display.ShortName) ? display.Name : display.ShortName;
            }
            else
            {
                return pi.Name;
            }
        }

        private static int GetWidth(PropertyInfo prop)
        {
            var children = GetProperties(prop.PropertyType);
            if (children.Any())
            {
                return children.Select(x => GetWidth(x)).Sum();
            }
            else
            {
                return 1;
            }
        }
    }
}