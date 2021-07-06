using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Macss.Models;
using Macss.ViewModels;

namespace Macss.Repositories
{
    public class LogService
    {
        internal static System.IO.MemoryStream Output(IEnumerable<LogViewModel> outputData)
        {
            var fs = new System.IO.MemoryStream();

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.AddWorksheet("sheet1");

                using (var ws = wb.Worksheets.First())
                {
                    OutputDetali(outputData, ws);
                    ws.ColumnsUsed().AdjustToContents();
                    using (fs)
                    {
                        wb.SaveAs(fs);
                        fs.Position = 0;

                        return fs;
                    }
                }
            }
        }

        internal static void OutputDetali(IEnumerable<LogViewModel> list, IXLWorksheet ws)
        {
            ws.Cell(1, 1).Value = "処理日時";
            ws.Cell(1, 2).Value = "処理ユーザーID";
            ws.Cell(1, 3).Value = "氏名";
            ws.Cell(1, 4).Value = "出力対象区分";
            ws.Cell(1, 5).Value = "出力対象機能";
            ws.Cell(1, 6).Value = "処理内容";
            ws.Cell(1, 7).Value = "処理件数";

            int row = 2;
            int col = 1;
            foreach (var data in list)
            {
                col = 1;
                // 処理日時
                ws.Cell(row, col).Style.NumberFormat.SetFormat("yyyy/mm/dd hh:mm:ss");
                ws.Cell(row, col).Value = data.ExcectionDate;
                col++;
                // 処理ユーザID
                ws.Cell(row, col).Value = data.UserId;
                col++;
                // 氏名
                ws.Cell(row, col).Value = data.UserName;
                col++;
                // 出力対象区分
                ws.Cell(row, col).Value = data.MenuName;
                col++;
                // 出力対象機能
                ws.Cell(row, col).Value = data.RoleName;
                col++;
                // 処理内容
                ws.Cell(row, col).Value = data.FunctionName;
                col++;
                // 処理件数
                //ws.Cell(row, col).Value = data.Purpose1;
                ws.Cell(row, col).Value = data.Purpose2;
                col++;
                row++;
            }
        }

        public IEnumerable<(string field, string message)> CreateLogHistory(
            string loginUser, string processingId, string function, string purpose1, string purpose2, ILogRepository logRepository)
        {
            var errors = new List<(string field, string message)>();
            TLogHistory logFunc = new TLogHistory
            {
                CreateId = loginUser,
                CreateDate = DateTime.Now,
                UpdateId = loginUser,
                UpdateDate = DateTime.Now,
                ExcectionDate = DateTime.Now,
                UserId = loginUser,
                ProcessingId = processingId,
                Function = function,
                Purpose1 = purpose1,
                Purpose2 = purpose2
            };
            logRepository.CreateLogHisory(logFunc);

            return errors;
        }
    }
}