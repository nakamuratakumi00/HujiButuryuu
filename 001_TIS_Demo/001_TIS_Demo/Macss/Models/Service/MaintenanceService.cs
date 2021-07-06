using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Macss.Properties;
using Macss.Repositories;
using System.Configuration;
using System.Data.SqlClient;
using Macss.ViewModels;
using Macss.Areas.Tass.Common;
using Macss.Areas.Tass.Repositories;
using System.Data.Entity;
using Macss.Attributes.Custom;

namespace Macss.Models.Service
{
    public class MaintenanceService
    {

        #region 定数

        private const string ModelsPath = "Macss.Models.";
        private const string AreaName = "Areas";
        private const string HinmeiName = "Macss.Areas.Tass.Models.MUnsouHinmeiKoyuu";
        private const string TodokesakiName = "Macss.Areas.Tass.Models.MUnsouTodokesakiKoyuu";
        private static readonly string[] ExtensionsForTenderInfo = { ".xlsx", ".csv" };


        private const string ExtensionExcel = ".xlsx";
        private const string ExtensionCsv = ".csv";

        //private const string DeleteFlgTrue = "1";
        //private const string DeleteFlgPhysics = "delete_flg";
        //private const string DeleteFlgLogical = "削除フラグ";
        private const string UpdateTypeD = "D";
        private const string UpdateTypeI = "1";
        private const string UpdateTypeU = "2";
        private const string UpdateType = "updateType";
        private const string UpdateTypeName = "更新区分";

        private const string CreateId = "create_id";
        private const string CreateDate = "create_date";
        private const string UpdateId = "update_id";
        private const string UpdateDate = "update_date";

        private const string DelFlg = "DELFLG";

        private const string Hincod = "HINCOD";
        private const string Tdkcod = "TDKCOD";
        private const string DtMoto = "DTMOTO";
        private const string TdkNam = "TDKNAM";
        private const string TdkNmk = "TDKNMK";
        private const string Jyusyo = "JYUSYO";
        private const string HinNam = "HINNAM";
        private const string HinNmk = "HINNMK";
        private const string HaiNam = "HAINAM";
        private const string Irimot = "IRIMOT";

        private const string Crtcod = "CRTCOD";
        private const string Crtymd = "CRTYMD";
        private const string Updcod = "UPDCOD";
        private const string Updymd = "UPDYMD";

        private const string Update = "更新処理";
        private const string Insert = "追加処理";
        private const string Delete = "削除処理";
        #endregion

        #region 取込処理

        public static List<string> Import(ApplicationDB dbContext, MControl targetData, HttpPostedFileWrapper file, IMaintenanceRepository maintenanceRepository, string user, ref MaintenanceViewModels model)
        {
            List<string> errors = new List<string>();

            // ファイル形式の確認
            var extension = System.IO.Path.GetExtension(file.FileName.ToString());
            int extensionKbn = 0;

            if (extension == ExtensionExcel)
            {
                extensionKbn = 0; // Excel
            }
            else if (extension == ExtensionCsv)
            {
                extensionKbn = 1; // CSV
            }
            else
            {
                errors.Add(string.Format(Resources.Message.CE049)); // フォーマットが正しくありません。
                return errors;
            }

            // 保存先フォルダの取得＆確認
            var destinationFolder = System.Web.HttpContext.Current.Server.MapPath("/");
            destinationFolder = destinationFolder + "\\" + Resource.UploadFiles;

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);

                string addName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_";
                fileName = addName + fileName;

                var path = Path.Combine(destinationFolder, fileName);

                try
                {
                    file.SaveAs(path);

                    if (extensionKbn == 0)
                    {
                        errors = ImportExcel(dbContext, targetData, maintenanceRepository, path, user, ref model);

                    } else if (extensionKbn == 1)
                    {
                        errors = ImportCsv(dbContext, targetData, maintenanceRepository, path, user, ref model);

                    }
                }
                finally
                {
                    File.Delete(path);
                }
            }

            return errors;
        }

        private static List<string> ImportCsv(ApplicationDB dbContext, MControl targetData, IMaintenanceRepository maintenanceRepository, string path, string user, ref MaintenanceViewModels model)
        {

            // 対象テーブルのプロパティをモデルから取得
            var propertyList = GetPropertyList(targetData.Value3);

            List<string> err = new List<string>();

            // csvファイルを開く
            TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding("Shift_JIS"));
            using (parser)
            {
                parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                parser.SetDelimiters(",");

                var headerData = new Dictionary<int, string>();
                var keyData = new Dictionary<int, string>();
                var propertyData = new Dictionary<int, MyProperty>();
                var dataList = new List<string[]>();
                int xCount = 0;
                var date = DateTime.Now.ToString(); // 処理日時

                int index = 1;

                var checkKey = new Dictionary<String, int>();
                var duplicate = new Dictionary<int, String>();

                while (!parser.EndOfData)
                {
                    string[] cols = parser.ReadFields(); // 1行読み込み

                    if (index == 1) // ヘッダー部
                    {
                        // 列数
                        xCount = cols.Length;

                        // ヘッダー＆属性作成
                        for (int n = 0; n < xCount; n++)
                        {
                            var column = cols[n];

                            if (string.IsNullOrEmpty(column))
                            {
                                break;
                            }

                            var propertyInfo = propertyList.FirstOrDefault(x => x.PhysicsName == column);
                            if (propertyInfo != null)
                            {
                                headerData.Add(n + 1, column); // Excel処理とKeyを合わせるため +1する
                                propertyData.Add(n + 1, propertyInfo);

                                if (propertyInfo.Key)
                                {
                                    keyData.Add(n + 1, column);
                                }
                            }
                        }

                        // ヘッダー項目チェック
                        foreach (MyProperty property in propertyList)
                        {
                            if (!headerData.ContainsValue(property.PhysicsName))
                            {
                                //err.Add(String.Format(Resources.Message.CE053, property.PhysicsName)); // {0}：取込項目がありません。
                                err.Add("取込ファイルが異なります。"); //{0}：　{1}。
                                return err;
                            }
                        }

                        if (err.Count() > 0)
                        {
                            break;
                        }
                    }
                    else if (index > 2) // データ部
                    {
                        int key = headerData.First(y => y.Value == UpdateType).Key - 1;

                        // データ作成＆チェック処理
                        var rowData = new string[xCount + 1];

                        string keys = string.Empty;
                        for (int i = 1; i <= xCount; i++)
                        {
                            // 取込対象列のみ処理する
                            if (propertyData.ContainsKey(i))
                            {
                                var value = cols[i - 1]; // Excel処理とKeyを合わせるため -1する
                                var property = propertyData[i];

                                if (cols[key].ToString() == UpdateTypeI ||
                                        cols[key].ToString() == UpdateTypeU ||
                                        cols[key].ToString() == UpdateTypeD)
                                {

                                    // 重複キーチェック
                                    if (keyData.ContainsKey(i))
                                    {
                                        string pKey = keyData[i];
                                        if (!string.IsNullOrEmpty(pKey))
                                        {
                                            if (string.IsNullOrEmpty(keys)) keys = keys + value;
                                            else keys = keys + ":" + value;
                                        }
                                    }

                                    switch (property.PhysicsName)
                                    {
                                        // 配列[0]に削除フラグを格納
                                        /*
                                        case DeleteFlgPhysics:
                                            rowData[0] = value == DeleteFlgTrue ? value : String.Empty;
                                            break;
                                        */

                                        // 配列[i]に列毎の値を格納
                                        case UpdateDate:
                                        case CreateDate:
                                        case Crtymd:
                                        case Updymd:
                                            rowData[i] = date;
                                            break;

                                        case UpdateId:
                                        case CreateId:
                                        case Updcod:
                                        case Crtcod:
                                            rowData[i] = user;
                                            break;
                                        case UpdateType:
                                            rowData[0] = value;
                                            break;
                                        default:
                                            CheckProperty(err, index, value, property);
                                            rowData[i] = value;
                                            break;
                                    }
                                } else
                                {
                                    rowData[i] = value;
                                }
                            }
                        }

                        // 重複データチェック
                        if (cols[key].ToString() == UpdateTypeI ||
                            cols[key].ToString() == UpdateTypeU ||
                            cols[key].ToString() == UpdateTypeD)
                        {
                            CheckKeyString(keys, index, checkKey, ref duplicate);
                        }

                        dataList.Add(rowData);
                    }

                    index++;
                }

                DuplicateErrorString(ref err, duplicate, keyData);

                // SQL作成
                StringBuilder sql = new StringBuilder();

                // 削除フラグは追加項目のため削除
                //var deleteFlgKey = headerData.First(x => x.Value == DeleteFlgPhysics).Key;
                //headerData.Remove(deleteFlgKey);
                var updateType = headerData.First(x => x.Value == UpdateType).Key;
                headerData.Remove(updateType);


                int count = 0;
                foreach (var data in dataList)
                {
                    count++;
                    if (data[0] == UpdateTypeD)
                    {
                        // Delete
                        if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                        {
                            err.Add(ErrorString(Delete, xCount, data, headerData, keyData, null, count));
                        }
                    }
                    else if (data[0] == UpdateTypeI)
                    {
                        // Insert
                        if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) != 0)
                        {
                            err.Add(ErrorString(Insert, xCount, data, headerData, keyData, null, count));
                        }
                    }
                    else if (data[0] == UpdateTypeU)
                    {
                        // Update
                        if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                        {
                            err.Add(ErrorString(Update, xCount, data, headerData, keyData, null, count));
                        }
                    }
                }

                if (err.Count() > 0)
                {
                    return err;
                }

                using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    count = 0;
                    foreach (var data in dataList)
                    {
                        count++;
                        /*
                        if (data[0] != DeleteFlgTrue)
                        {
                            CreateInsertSQL(ref sql, targetData.Value2, xCount, data, headerData, keyData);
                        }
                        else
                        {
                            CreateDeleteSQL(ref sql, targetData.Value2, xCount, data, headerData, keyData);

                        }
                        */

                        if (data[0] == UpdateTypeD)
                        {
                            // Delete
                            if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                            {
                                transaction.Rollback();
                                err.Add(ErrorString(Delete, xCount, data, headerData, keyData, null, count));
                                return err;
                            }
                            StringBuilder DeleteSql = new StringBuilder();
                            CreateDeleteSQL(ref DeleteSql, targetData.Value2, xCount, data, headerData, keyData);
                            dbContext.Database.ExecuteSqlCommand(DeleteSql.ToString());
                            model.Delete++;
                        }
                        else if (data[0] == UpdateTypeI)
                        {
                            // Insert
                            if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) != 0)
                            {
                                transaction.Rollback();
                                err.Add(ErrorString(Insert, xCount, data, headerData, keyData, null, count));
                                return err;
                            }
                            StringBuilder insertSql = new StringBuilder();
                            CreateInsertSQL(ref insertSql, targetData.Value2, xCount, data, headerData, keyData);
                            dbContext.Database.ExecuteSqlCommand(insertSql.ToString());
                            model.Insert++;
                        }
                        else if (data[0] == UpdateTypeU)
                        {
                            // Update
                            if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                            {
                                transaction.Rollback();
                                err.Add(ErrorString(Update, xCount, data, headerData, keyData, null, count));
                                return err;
                            }
                            StringBuilder UpdateSql = new StringBuilder();
                            CreateUpdateSQL(ref UpdateSql, targetData.Value2, xCount, data, headerData, keyData);
                            dbContext.Database.ExecuteSqlCommand(UpdateSql.ToString());
                            model.Update++;
                        } else
                        {
                            model.Invalid++;
                        }
                    }

                    if (model.Delete > 0 || model.Insert > 0 || model.Update > 0)
                    {
                        // SQL実行
                        transaction.Commit();
                        //maintenanceRepository.ExecuteCommand(sql.ToString());
                    }
                }

            }

            return err;
        }
        public static List<string> ImportExcel(ApplicationDB dbContext, MControl targetData, IMaintenanceRepository maintenanceRepository, string path, string user, ref MaintenanceViewModels model)
        {

            bool delFlg = false;

            // 対象テーブルのプロパティをモデルから取得
            var propertyList = GetPropertyList(targetData.Value3);

            List<string> err = new List<string>();

            using (var wb = new XLWorkbook(path))
            {
                // シート取込
                using (var ws = wb.Worksheets.Worksheet(1))
                {
                    // 全データを取得
                    var usedData = ws.RangeUsed();

                    var xCount = ws.RangeUsed().ColumnCount();
                    var yCount = ws.RangeUsed().RowCount();

                    var headerData = new Dictionary<int, string>();
                    var keyData = new Dictionary<int, string>();
                    var keyDataJ = new Dictionary<int, string>();
                    var propertyData = new Dictionary<int, MyProperty>();

                    // ヘッダー項目の取得
                    var firstRow = usedData.Row(1);
                    var firstRowJ = usedData.Row(2);
                    for (int i = 1; i <= xCount; i++)
                    {
                        var column = firstRow.Cell(i).Value.ToString();
                        var columnJ = firstRowJ.Cell(i).Value.ToString();
                        if (string.IsNullOrEmpty(column))
                        {
                            break;
                        }

                        var propertyInfo = propertyList.FirstOrDefault(x => x.PhysicsName == column);
                        if (propertyInfo != null)
                        {
                            headerData.Add(i, column);
                            propertyData.Add(i, propertyInfo);

                            if (propertyInfo.Key)
                            {
                                keyData.Add(i, column);
                                keyDataJ.Add(i, propertyInfo.LogicalName);
                            }
                        }
                    }

                    // ヘッダー項目チェック
                    //foreach (MyProperty property in propertyList.Where(x => x.Required == true)) // 必須項目のみをチェックするかは要件に応じる
                    foreach (MyProperty property in propertyList)
                    {
                        if (!headerData.ContainsValue(property.PhysicsName))
                        {
                            //err.Add(String.Format(Resources.Message.CE053, property.PhysicsName));
                            err.Add("取込ファイルが異なります。"); //{0}：　{1}。
                            return err;
                        }
                    }

                    if (err.Count() > 0)
                    {
                        return err;
                    }

                    // 処理日時
                    var date = DateTime.Now.ToString();

                    // 処理行
                    int index = 3;
                    int idx = index - 1;
                    var checkKey = new Dictionary<String, int>();
                    var duplicate = new Dictionary<int, String>();

                    // データ取込＆チェック
                    var dataList = usedData.Rows().Skip(2).Select(x =>
                    {

                        delFlg = false;
                        var key = headerData.First(y => y.Value == UpdateType).Key;
                        var key2 = headerData.FirstOrDefault(y => y.Value == DtMoto).Key;

                        int delIdx = -1;
                        var rowData = new string[xCount + 1];

                        idx++;
                        // 更新、削除時データ管理元が4または、7以外は対象外
                        if (x.Cell(key).Value.ToString() == UpdateTypeU ||
                            x.Cell(key).Value.ToString() == UpdateTypeD)
                        {
                            if (key2 != 0 && x.Cell(key2).Value.ToString() != "4" && x.Cell(key2).Value.ToString() != "7")
                            {
                                x.Cell(key).Value = "";
                            }
                        }

                        if (x.Cell(key).Value.ToString() == UpdateTypeI ||
                                x.Cell(key).Value.ToString() == UpdateTypeU ||
                                x.Cell(key).Value.ToString() == UpdateTypeD)
                        {
                            string keys = string.Empty;
                            for (int i = 1; i <= xCount; i++)
                            {
                                // 取込対象列のみ処理する
                                if (propertyData.ContainsKey(i))
                                {
                                    var value = x.Cell(i).Value.ToString();
                                    var property = propertyData[i];

                                    // 重複キーチェック
                                    if (keyData.ContainsKey(i)) {
                                        string pKey = keyData[i];
                                        if (!string.IsNullOrEmpty(pKey) && !string.IsNullOrEmpty(value))
                                        {                                            
                                            if (string.IsNullOrEmpty(keys)) keys = keys + value;
                                            else keys = keys + ":" + value;
                                        }
                                    }

                                    switch (property.PhysicsName)
                                    {
                                        // 配列[0]に削除フラグを格納
                                        /*
                                        case DeleteFlgPhysics:
                                            rowData[0] = value == DeleteFlgTrue ? value : String.Empty;
                                            break;
                                        */
                                        case UpdateType:
                                            if ((targetData.Value3 == HinmeiName || targetData.Value3 == TodokesakiName) && value == UpdateTypeD)
                                            {
                                                delFlg = true;
                                            }
                                            rowData[0] = value;
                                            break;

                                        // 配列[i]に列毎の値を格納
                                        case UpdateDate:
                                        case CreateDate:
                                        case Crtymd:
                                        case Updymd:
                                            rowData[i] = date;
                                            break;

                                        case UpdateId:
                                        case CreateId:
                                        case Updcod:
                                        case Crtcod:
                                            rowData[i] = user;
                                            break;

                                        case DelFlg:
                                            delIdx = i;
                                            break;

                                        case Hincod:
                                        case Tdkcod:

                                            if (x.Cell(key).Value.ToString() == UpdateTypeI)
                                            {
                                                // 新規登録
                                                switch (targetData.Value2)
                                                {
                                                    case "m_unsou_hinmei_koyuu":

                                                        if (string.IsNullOrEmpty(value))
                                                        {
                                                            CheckHincod(dbContext, err, index, ref value, property);
                                                            if (string.IsNullOrEmpty(keys)) keys = keys + value;
                                                            else keys = keys + ":" + value;
                                                        } else {
                                                            CheckHincod(dbContext, err, index, ref value, property);
                                                        }
                                                        break;

                                                    case "m_unsou_todokesaki_koyuu":
                                                        if (string.IsNullOrEmpty(value))
                                                        {
                                                            CheckTdkcod(dbContext, err, index, ref value, property);
                                                            if (string.IsNullOrEmpty(keys)) keys = keys + value;
                                                            else keys = keys + ":" + value;
                                                        } else
                                                        {
                                                            CheckTdkcod(dbContext, err, index, ref value, property);
                                                        }
                                                        break;

                                                    default:
                                                        CheckProperty(err, index, value, property);
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                CheckProperty(err, index, value, property);
                                            }

                                            rowData[i] = value;
                                            break;

                                        case DtMoto:
                                            if (x.Cell(key).Value.ToString() == UpdateTypeI)
                                            {
                                                value = "7";
                                            }
                                            rowData[i] = value;
                                            break;

                                        default:
                                            CheckProperty(err, index, value, property);
                                            rowData[i] = value;
                                            break;
                                    }
                                }
                            }

                            // 重複データチェック
                            CheckKeyString(keys, idx, checkKey, ref duplicate);

                            if (delIdx != -1)
                            {
                                if (delFlg)
                                {
                                    rowData[delIdx] = "1";
                                } else
                                {
                                    rowData[delIdx] = "0";
                                }
                            }
                        }

                        index++;
                        return rowData;
                    }).ToList();

                    DuplicateErrorString(ref err, duplicate, keyDataJ);

                    // SQL作成
                    StringBuilder sql = new StringBuilder();

                    // 削除フラグは追加項目のため削除
                    //var deleteFlgKey = headerData.First(x => x.Value == DeleteFlgPhysics).Key;
                    //headerData.Remove(deleteFlgKey);
                    bool logicDel = targetData.Value2 == "m_unsou_hinmei_koyuu" || targetData.Value2 == "m_unsou_todokesaki_koyuu";
                    var updateType = headerData.First(x => x.Value == UpdateType).Key;
                    headerData.Remove(updateType);

                    int count = 0;
                    foreach (var data in dataList)
                    {
                        count++;
                        if (data[0] == UpdateTypeD && targetData.Value2 != "m_unsou_hinmei_koyuu" && targetData.Value2 != "m_unsou_todokesaki_koyuu")
                        {
                            // Delete
                            if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                            {
                                err.Add(ErrorString(Delete, xCount, data, headerData, keyData, keyDataJ, count));
                            }
                        }
                        else if (data[0] == UpdateTypeI)
                        {
                            // Inset
                            if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) != 0)
                            {
                                err.Add(ErrorString(Insert, xCount, data, headerData, keyData, keyDataJ, count));
                            }
                        }
                        else if (data[0] == UpdateTypeU || (data[0] == UpdateTypeD && logicDel))
                        {
                            // Update
                            if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                            {
                                if (data[0] == UpdateTypeD && logicDel)
                                {
                                    err.Add(ErrorString(Delete, xCount, data, headerData, keyData, keyDataJ, count));
                                }
                                else
                                {
                                    err.Add(ErrorString(Update, xCount, data, headerData, keyData, keyDataJ, count));
                                }
                            }
                        }
                    }

                    if (err.Count() > 0)
                    {
                        return err;
                    }

                    using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        count = 0;
                        foreach (var data in dataList)
                        {
                            count++;
                            /*
                            if (data[0] != DeleteFlgTrue)
                            {
                                CreateInsertSQL(ref sql, targetData.Value2, xCount, data, headerData, keyData);
                            }
                            else
                            {
                                CreateDeleteSQL(ref sql, targetData.Value2, xCount, data, headerData, keyData);

                            }
                            */

                            if (data[0] == UpdateTypeD && targetData.Value2 != "m_unsou_hinmei_koyuu" && targetData.Value2 != "m_unsou_todokesaki_koyuu")
                            {
                                // Delete
                                if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                                {
                                    transaction.Rollback();
                                    err.Add(ErrorString(Delete, xCount, data, headerData, keyData, keyDataJ, count));
                                    return err;
                                }
                                StringBuilder DeleteSql = new StringBuilder();
                                CreateDeleteSQL(ref DeleteSql, targetData.Value2, xCount, data, headerData, keyData);
                                dbContext.Database.ExecuteSqlCommand(DeleteSql.ToString());
                                model.Delete++;

                            }
                            else if (data[0] == UpdateTypeI)
                            {
                                // Inset
                                if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) != 0)
                                {
                                    transaction.Rollback();
                                    err.Add(ErrorString(Insert, xCount, data, headerData, keyData, keyDataJ, count));
                                    return err;
                                }
                                StringBuilder inputSql = new StringBuilder();
                                CreateInsertSQL(ref inputSql, targetData.Value2, xCount, data, headerData, keyData);
                                dbContext.Database.ExecuteSqlCommand(inputSql.ToString());
                                model.Insert++;

                            }
                            else if (data[0] == UpdateTypeU || (data[0] == UpdateTypeD && logicDel))
                            {
                                // Update
                                if (CheckDataCountSQL(dbContext, targetData.Value2, xCount, data, headerData, keyData) == 0)
                                {
                                    transaction.Rollback();
                                    if (data[0] == UpdateTypeD && logicDel)
                                    {
                                        err.Add(ErrorString(Delete, xCount, data, headerData, keyData, keyDataJ, count));
                                    }
                                    else
                                    {
                                        err.Add(ErrorString(Update, xCount, data, headerData, keyData, keyDataJ, count));
                                    }
                                    return err;
                                }
                                StringBuilder UpdateSql = new StringBuilder();
                                CreateUpdateSQL(ref UpdateSql, targetData.Value2, xCount, data, headerData, keyData);
                                dbContext.Database.ExecuteSqlCommand(UpdateSql.ToString());
                                if (data[0] == UpdateTypeD && logicDel)
                                {
                                    model.Delete++;
                                }
                                else
                                {
                                    model.Update++;
                                }
                            }
                            else
                            {
                                model.Invalid++;
                            }
                        }

                        if (model.Delete > 0 || model.Insert > 0 || model.Update > 0)
                        {
                            // SQL実行
                            transaction.Commit();
                        }
                    }
                }
            }

            return err;

        }

        private static List<MyProperty> GetPropertyList(string modelName)
        {
            // モデル情報取得
            Assembly asm = typeof(MaintenanceService).Assembly;

            var modelsPath = ModelsPath + modelName;
            if (modelName.IndexOf(AreaName) > 0)
            {
                modelsPath = modelName;
            }
            var tableData = asm.GetType(modelsPath).GetProperties();

            // 項目毎に属性作成
            var propertyList = new List<MyProperty>();
            foreach (PropertyInfo row in tableData)
            {
                var data = new MyProperty();

                if (!(Attribute.GetCustomAttribute(row, typeof(ColumnAttribute)) is ColumnAttribute physicsName))
                {
                    continue;
                }
                var logicalName = Attribute.GetCustomAttribute(row, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var required = Attribute.GetCustomAttribute(row, typeof(RequiredAttribute)) as RequiredAttribute;
                var length = Attribute.GetCustomAttribute(row, typeof(MaxLengthAttribute)) as MaxLengthAttribute;
                var key = Attribute.GetCustomAttribute(row, typeof(KeyAttribute)) as KeyAttribute;
                var RegularExpression = Attribute.GetCustomAttribute(row, typeof(RegularExpressionAttribute)) as RegularExpressionAttribute;
                var fileRequired = Attribute.GetCustomAttribute(row, typeof(FileRequiredAttribute)) as FileRequiredAttribute;
                var fileDoubleRequired = Attribute.GetCustomAttribute(row, typeof(FileDoubleRequiredAttribute)) as FileDoubleRequiredAttribute;
                var filelength = Attribute.GetCustomAttribute(row, typeof(FileMaxLengthAttribute)) as FileMaxLengthAttribute;

                data.Name = row.Name; // 変数名
                data.PhysicsName = physicsName == null ? String.Empty : physicsName.Name;
                data.LogicalName = logicalName == null ? String.Empty : logicalName.Description;   // 論理名
                data.Required = required == null ? false : true;
                data.Length = length == null ? 0 : length.Length;
                data.Key = key == null ? false : true;
                data.Type = Nullable.GetUnderlyingType(row.PropertyType) ?? row.PropertyType;
                data.RegularExpression = RegularExpression == null ? String.Empty : RegularExpression.Pattern;
                data.FileRequired = fileRequired == null ? false : true;
                data.FileDoubleRequired = fileDoubleRequired == null ? false : true;
                data.FileLength = filelength == null ? 0 : filelength.value;

                propertyList.Add(data);
            }

            // 削除フラグを追加
            /*
            if (!propertyList.Any(x => x.PhysicsName == DeleteFlgPhysics))
            {
                var data = new MyProperty();
                data.Name = DeleteFlgLogical;
                data.PhysicsName = DeleteFlgPhysics;
                data.Required = false;
                data.Length = 1;
                data.Key = false;

                propertyList.Add(data);
            }
            */

            if (!propertyList.Any(x => x.PhysicsName == UpdateType))
            {
                var data = new MyProperty
                {
                    Name = UpdateTypeName,
                    PhysicsName = UpdateType,
                    Required = false,
                    Length = 1,
                    Key = false
                };

                propertyList.Add(data);
            }

            return propertyList;
        }

        private static void CheckProperty(List<string> err, int index, string value, MyProperty property)
        {

            // キー項目のみ必須チェックを行うように変更
            if (property.Key)
            {
                // 必須チェック
                if (property.Required)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        //err.Add(String.Format(Resources.Message.CE045, index.ToString() + "行目：" + property.PhysicsName)); // {0}：必須項目を入力してください。
                        err.Add(String.Format(Resources.Message.CE045, index.ToString() + "行目：" + property.LogicalName)); // {0}：必須項目を入力してください。
                        return;
                    }
                }
            }

            // ファイル必須項目チェック
            if (property.FileRequired)
            {
                if (property.Type.Name != "Decimal" && property.Type.Name != "Int" && property.Type.Name != "Int32")
                {                    
                    if (string.IsNullOrEmpty(value.Trim()))
                    {
                        err.Add(String.Format(Resources.Message.CE045, index.ToString() + "行目：" + property.LogicalName)); // {0}：必須項目を入力してください。
                        return;
                    }
                }
            }

            // 型チェック
            try
            {
                Convert.ChangeType(value, property.Type);
            }
            catch
            {
                //err.Add(String.Format(Resources.Message.CE073, index.ToString() + "行目：" + property.PhysicsName)); // {0}：　データベースと型が異なります。

                if (property.Type.Name == "Decimal" || property.Type.Name == "Int" || property.Type.Name == "Int32")
                {
                    if (value == "")
                    {
                        err.Add((string.Format(Resources.Message.CE069, index.ToString() + "行目：" + property.LogicalName, "数値(0～)を入力してください"))); //{0}：　{1}。
                    } else
                    {
                        err.Add((string.Format(Resources.Message.CE069, index.ToString() + "行目：" + property.LogicalName, "数値型に [" + value + "] は入れられません"))); //{0}：　{1}。
                    }
                } else {
                    err.Add(String.Format(Resources.Message.CE073, index.ToString() + "行目：" + property.LogicalName)); // {0}：　データベースと型が異なります。
                }
                return;
            }

            // 桁数チェック
            if (property.Length != 0)
            {
                if (value.Length > property.Length)
                {
                    //err.Add(String.Format(Resources.Message.CE052, index.ToString() + "行目：" + property.PhysicsName, property.Length.ToString())); // {0}：{1}文字以内で入力してください。
                    err.Add(String.Format(Resources.Message.CE052, index.ToString() + "行目：" + property.LogicalName, property.Length.ToString())); // {0}：{1}文字以内で入力してください。
                    return;
                }
            }

            // 桁数チェック(ファイル)
            if (property.FileLength != 0)
            {
                if (value.Length > property.FileLength)
                {
                    err.Add(String.Format(Resources.Message.CE052, index.ToString() + "行目：" + property.LogicalName, property.FileLength.ToString())); // {0}：{1}文字以内で入力してください。
                    return;
                }
            }

            // 正規表現チェック
            if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(property.RegularExpression))
            {
                var rgx = new Regex(property.RegularExpression, RegexOptions.IgnoreCase);
                var match = rgx.Match(value);
                if (value != match.Value)
                {
                    string msgPtn = "";
                    switch (property.RegularExpression)
                    {
                        case "[ -~｡-ﾟ]+":       // 半角
                            msgPtn = Resources.Message.CE064; // {0}：半角文字を入力してください。
                            break;
                        case "[a-zA-Z0-9]+":    // 半角英数字
                            msgPtn = Resources.Message.CE061; // {0}：半角英数字を入力してください。
                            break;
                        case "[｡-ﾟ+]+":         // 半角カタカナ
                            msgPtn = Resources.Message.CE063; // {0}：半角カナを入力してください。
                            break;
                        case "[0-9]+":          // 半角数値
                            msgPtn = Resources.Message.CE043; // {0}：半角数字を入力してください。
                            break;
                        case "[0-9 ]+":          // 半角数値
                            msgPtn = Resources.Message.CE043; // {0}：半角数字を入力してください。
                            break;
                        case "[0-9-]+":        // 電話番号・ＦＡＸ番号
                            msgPtn = Resources.Message.CE062; // {0}：半角数字（ハイフン付き）を入力してください。
                            break;
                        case "[0-9- ]+":        // 電話番号・ＦＡＸ番号
                            msgPtn = Resources.Message.CE062; // {0}：半角数字（ハイフン付き）を入力してください。
                            break;
                        case "[ぁ-ん]+":        // ひらがな
                            msgPtn = Resources.Message.CE048; // {0}ひらがなのみで入力してください。
                            break;
                        case "[^ -~｡-ﾟ]+":      // 全角
                            msgPtn = Resources.Message.CE109; // {0}：全角文字を入力してください。
                            break;
                        case "[A-Z0-9 -/:-@\\[-`{-~｡-ﾟ]+":      // 半角英数字(半角小文字英字不可)
                            msgPtn = Resources.Message.CE072; // {0}：　半角英数字を入力してください。(半角小文字英字不可)
                            break;
                        default:
                            msgPtn = Resources.Message.CE051; // {0}：入力形式が正しくありません。
                            break;
                    }
                    //err.Add(String.Format(msgPtn, index.ToString() + "行目：" + property.PhysicsName));
                    err.Add(String.Format(msgPtn, index.ToString() + "行目：" + property.LogicalName));
                    return;
                }
            }
        }

        private static void CheckHincod(ApplicationDB dbContext, List<string> err, int index, ref string value, MyProperty property)
        {

            if (value != null && value.Trim() == "")
            {
                IHinmeiRepositorie hinmeiRepositorie = new HinmeiRepositorie(dbContext);
                value = string.Empty;
                var max_no = hinmeiRepositorie.GetHincod();
                value = max_no.ToString("HN000000");
                return;
            }

            // 8桁　かつ　先頭２文字が"HN"の場合はエラー
            if (value.Length == 8 && value.StartsWith("HN"))
            {
                err.Add(index.ToString() + "行目：" + property.PhysicsName + "：　新規登録の場合" + value + "のコードは登録できません。");
                return;
            }

            // 上記エラーがなければ通常チェック
            CheckProperty(err, index, value, property);
        }

        private static void CheckTdkcod(ApplicationDB dbContext, List<string> err, int index, ref string value, MyProperty property)
        {
            if (value != null && value.Trim() == "")
            {
                ITodokesakiRepositorie todokesakiRepositorie = new TodokesakiRepositorie(dbContext);
                value = string.Empty;
                var max_no = todokesakiRepositorie.GetTdkcod();
                value = max_no.ToString("TD000000");
                return;
            }

            // 8桁　かつ　先頭２文字が"TD"の場合はエラー
            if (value.Length == 8 && value.StartsWith("TD"))
            {
                err.Add(index.ToString() + "行目：" + property.PhysicsName + "：　新規登録の場合" + value + "のコードは登録できません。");
                return;
            }

            // 上記エラーがなければ通常チェック
            CheckProperty(err, index, value, property);
        }

        /*
        private static void CreateInsertSQL(ref StringBuilder sql, string tableName, int xCount, string[] data, Dictionary<int, string> columns, Dictionary<int, string> keyData)
        {
            sql.Append("MERGE INTO " + tableName + " AS A");
            sql.Append(" USING (SELECT ");

            var setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(",");
                }

                sql.Append("N'" + data[i] + "'" + " AS " + columns[i]);
                setValue = true;
            }
            sql.Append(" ) AS B");
            sql.Append(" ON (");
            setValue = false;

            int index = 1;
            foreach (string key in keyData.Values)
            {
                if (setValue)
                {
                    sql.Append(" AND ");
                }
                sql.Append("A." + key + " = B." + key);

                setValue = true;
                index++;
            }

            sql.Append(") WHEN MATCHED THEN UPDATE SET ");
            setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                var column = columns[i];
                if (column == CreateId || column == CreateDate)
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(",");
                }

                sql.Append(column + " = B." + column);
                setValue = true;
            }

            sql.Append(" WHEN NOT MATCHED THEN INSERT (");
            setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(", ");
                }

                sql.Append(columns[i]);
                setValue = true;
            }

            sql.Append(") VALUES (");
            setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(", ");
                }

                sql.Append("B." + columns[i]);
                setValue = true;
            }

            sql.AppendLine(");");
        }
        */

        private static void CreateInsertSQL(ref StringBuilder sql, string tableName, int xCount, string[] data, Dictionary<int, string> columns, Dictionary<int, string> keyData)
        {

            sql.Append(" INSERT INTO " + tableName + "(");
            var setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(", ");
                }

                sql.Append(columns[i]);
                setValue = true;
            }

            sql.Append(") VALUES (");
            setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(",");
                }

                sql.Append("N'" + data[i] + "'");
                setValue = true;
            }

            sql.AppendLine(");");
        }

        private static int CheckDataCountSQL(ApplicationDB dbContext, string tableName, int xCount, string[] data, Dictionary<int, string> columns, Dictionary<int, string> keyData)
        {

            StringBuilder sql = new StringBuilder();
            var setValue = false;

            sql.Append("SELECT COUNT(*) FROM " + tableName + " WHERE ");
            foreach (string key in keyData.Values)
            {
                if (setValue)
                {
                    sql.Append(" AND ");
                }
                sql.Append(key + " = N'");
                for (int i = 1; i <= xCount; i++)
                {
                    if (key.Equals(columns[i]))
                    {
                        sql.Append(data[i] + "'");
                        break;
                    }
                }

                setValue = true;
            }

            return dbContext.Database.SqlQuery<int>(sql.ToString()).First();


            //var connectionString = ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString;
            //var table = new DataTable();
            //try
            //{
            //    //using (var connection = new SqlConnection(connectionString))
            //    using (var connection = new SqlConnection(connectionString))
            //    using (var command = connection.CreateCommand())
            //    {

            //        // SQLの設定
            //        command.CommandText = @sql.ToString();

            //        // SQLの実行
            //        var adapter = new SqlDataAdapter(command);
            //        adapter.Fill(table);
            //    }
            //}
            //catch
            //{
            //    throw;
            //}

            //return (int)table.Rows[0][0];

        }

        private static void CreateUpdateSQL(ref StringBuilder sql, string tableName, int xCount, string[] data, Dictionary<int, string> columns, Dictionary<int, string> keyData)
        {
            var setValue = false;

            sql.Append(" UPDATE " + tableName + " SET ");
            setValue = false;

            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                var column = columns[i];
                if (column == CreateId || column == CreateDate || column == Crtcod || column == Crtymd)
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(",");
                }

                sql.Append(column + " = B." + column);
                setValue = true;
            }

            setValue = false;
            sql.Append(" FROM (SELECT ");
            for (int i = 1; i <= xCount; i++)
            {
                if (!columns.ContainsKey(i))
                {
                    continue;
                }

                if (setValue)
                {
                    sql.Append(",");
                }

                sql.Append("N'" + data[i] + "'" + " AS " + columns[i]);
                setValue = true;
            }
            sql.Append(" ) AS B");
            sql.Append(" WHERE (");
            setValue = false;

            int index = 1;
            foreach (string key in keyData.Values)
            {
                if (setValue)
                {
                    sql.Append(" AND ");
                }
                sql.Append(tableName + "." + key + " = B." + key);

                setValue = true;
                index++;
            }

            sql.AppendLine(");");
        }

        private static void CreateDeleteSQL(ref StringBuilder sql, string tableName, int xCount, string[] data, Dictionary<int, string> columns, Dictionary<int, string> keyData)
        {
            sql.Append("DELETE FROM " + tableName + " WHERE ");

            int index = 1;
            foreach (string keyValue in keyData.Values)
            {
                var deleteKey = columns.First(x => x.Value == keyValue).Key;

                if (index != 1)
                {
                    sql.Append(" AND ");
                }

                sql.Append(keyValue + " = '" + data[deleteKey] + "'");

                index++;
            }

            sql.AppendLine(";");
        }

        #endregion

        #region 出力処理

        internal static System.IO.MemoryStream Output(MControl targetData, string where, IMaintenanceRepository maintenanceRepository, int extention)
        {
            // 指定されたマスタデータを取得
            var tableName = targetData.Value2;
            var modelName = targetData.Value3;

            string order = string.Empty;
            if (tableName == "v_hokan_tanka")
            {
                order = " ORDER BY MONTH";
            }
            var outputData = maintenanceRepository.GetMasterData(tableName, where, order);

            // モデルプロパティを取得
            Assembly asm = typeof(MaintenanceService).Assembly;
            var modelsPath = ModelsPath + modelName;
            if (modelName.IndexOf(AreaName) > 0)
            {
                modelsPath = modelName;
            }
            Type type = asm.GetType(modelsPath);
            var tableData = asm.GetType(modelsPath).GetProperties();
            //            var tableData = asm.GetType(ModelsPath + modelName).GetProperties();

            var propertyList = new List<MyProperty>();
            foreach (PropertyInfo row in tableData)
            {
                var data = new MyProperty();
                var physicsName = Attribute.GetCustomAttribute(row, typeof(ColumnAttribute)) as ColumnAttribute;
                var logicalName = Attribute.GetCustomAttribute(row, typeof(DescriptionAttribute)) as DescriptionAttribute;

                data.PhysicsName = physicsName == null ? String.Empty : physicsName.Name;     // 物理名
                data.LogicalName = logicalName == null ? String.Empty : logicalName.Description;   // 論理名

                propertyList.Add(data);
            }


            var fs = new System.IO.MemoryStream();
            var columus = outputData.Columns;
            var table = outputData;
            if (table.Rows.Count == 0)
            {
                return null;
            }

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                wb.AddWorksheet(tableName);
                using (var ws = wb.Worksheets.First())
                {
                    OutputDetail(propertyList, columus, table, ws);
                    ws.ColumnsUsed().AdjustToContents();
                    using (fs)
                    {
                        // Excel
                        if (extention == 0)
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
                            csvWriter.Write(sb.ToString());

                            //var test = wb.Worksheets.First().Rows(1, lastCellAddress.RowNumber)
                            //    .Select(r => string.Join(",", r.Cells(1, lastCellAddress.ColumnNumber)
                            //        .Select(cell =>
                            //        {
                            //            var cellValue = cell.GetValue<string>();
                            //            return cellValue.Contains(",") ? $"\"{cellValue}\"" : "\"" + cellValue + "\"";
                            //        })));
                            //for (int i = 0; i < test.ToArray().Length; i++)
                            //{
                            //    csvWriter.Write(test.ElementAt(i));
                            //    csvWriter.Write("\r\n");
                            //}
                            csvWriter.Flush();
                            return fs;
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }

        internal static System.IO.MemoryStream OutputUnsou(MControl targetData, IMaintenanceRepository maintenanceRepository, int extention)
        {
            // 指定されたマスタデータを取得
            var tableName = targetData.Value2;
            var modelName = targetData.Value3;

            string order = string.Empty;
            if (tableName == "m_unsou_todokesaki_koyuu")
            {
                order = " ORDER BY TDKNMK,TDKNAM,TDKNMS,TDBNAM";
            } else if (tableName == "m_unsou_hinmei_koyuu")
            {
                order = " ORDER BY HINNAM,HINNMK";
            }
            var outputData = maintenanceRepository.GetMasterData(tableName, "", order);

            // モデルプロパティを取得
            Assembly asm = typeof(MaintenanceService).Assembly;
            Type type = asm.GetType(modelName);
            var tableData = asm.GetType(modelName).GetProperties();

            var propertyList = new List<MyProperty>();
            foreach (PropertyInfo row in tableData)
            {
                var data = new MyProperty();
                var physicsName = Attribute.GetCustomAttribute(row, typeof(ColumnAttribute)) as ColumnAttribute;
                var logicalName = Attribute.GetCustomAttribute(row, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var fileRequired = Attribute.GetCustomAttribute(row, typeof(FileRequiredAttribute)) as FileRequiredAttribute;
                var fileDoubleRequired = Attribute.GetCustomAttribute(row, typeof(FileDoubleRequiredAttribute)) as FileDoubleRequiredAttribute;
                var pType = Nullable.GetUnderlyingType(row.PropertyType) ?? row.PropertyType;
                var zero = string.Empty;
                if (pType.Name == "Decimal" || pType.Name == "Int" || pType.Name == "Int32")
                {
                    zero = "0";
                }
                var fReq = fileRequired == null ? "" : "*" + zero;
                var fDReq = fileDoubleRequired == null ? "" : "**" + zero;
                data.PhysicsName = physicsName == null ? String.Empty : physicsName.Name;     // 物理名
                data.LogicalName = logicalName == null ? String.Empty : logicalName.Description + fReq + fDReq;   // 論理名
                propertyList.Add(data);
            }

            var fs = new System.IO.MemoryStream();
            var columus = outputData.Columns;
            var table = outputData;
            if (targetData.Value1 != null) {
                DataRow[] selectedRows = outputData.Select(targetData.Value1);
                if (selectedRows.Length == 0)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        row.Delete();
                    }
                    table.AcceptChanges();
                }
                else
                {
                    table = selectedRows.CopyToDataTable();
                }
            }

            //データが存在しない場合はnullを返却
            if (table.Rows.Count == 0)
            {
                return null;
            }

            using (var wb = new ClosedXML.Excel.XLWorkbook())
            {
                String tableNameWork = tableName;
                if (tableName.Length >= 31)
                {
                    //tableNameWork = tableName.Substring(1, 31);
                    tableNameWork = tableName.Substring(0, 31);
                }
                wb.AddWorksheet(tableNameWork);
                using (var ws = wb.Worksheets.First())
                {
                    OutputDetail(propertyList, columus, table, ws);
                    ws.ColumnsUsed().AdjustToContents();
                    using (fs)
                    {
                        // Excel
                        if (extention == 0)
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

        private static void OutputDetail(List<MyProperty> propertyList, DataColumnCollection columus, DataTable table, IXLWorksheet ws)
        {
            // 物理名
            for (int i = 0; i < columus.Count; i++)
            {
                ws.Cell(1, i + 1).Value = columus[i];
            }
            // 論理名
            for (int i = 0; i < columus.Count; i++)
            {
                var logName = propertyList.Find(x => x.PhysicsName == columus[i].ToString()).LogicalName;
                ws.Cell(2, i + 1).Value = logName;
            }

            /*
            if (!propertyList.Any(x => x.PhysicsName == DeleteFlgPhysics))
            {
                ws.Cell(1, columus.Count + 1).Value = DeleteFlgPhysics;
                ws.Cell(2, columus.Count + 1).Value = DeleteFlgLogical;
            }
            */

            if (!propertyList.Any(x => x.PhysicsName == UpdateType))
            {
                ws.Cell(1, columus.Count + 1).Value = UpdateType;
                ws.Cell(2, columus.Count + 1).Value = UpdateTypeName;
            }

            // データ
            int rowNo = 3;
            for (int r = 0; r < table.Rows.Count; r++)
            {
                for (int c = 0; c < columus.Count; c++)
                {
                    string value = table.Rows[r][c].ToString();
                    //ws.Cell(rowNo, c + 1).Value = value;
                    DataUtil.SetExelString(ws.Cell(rowNo, c + 1), value);
                }
                rowNo++;
            }
        }
        #endregion

        private static void CheckKeyString(string keys, int idx, Dictionary<String, int> checkKey, ref Dictionary<int, string> duplicate)
        {
            if (!checkKey.ContainsKey(keys))
            {
                checkKey.Add(keys, idx);
            }
            else
            {
                if (!duplicate.ContainsValue(keys))
                {
                    duplicate.Add(checkKey[keys], keys);
                    duplicate.Add(idx, keys);
                }
                else
                {
                    duplicate.Add(idx, keys);
                }
            }
        }

        private static void DuplicateErrorString(ref List<string> err, Dictionary<int, string> duplicate, Dictionary<int, string> keyData)
        {
            foreach (KeyValuePair<int, string> dup in duplicate)
            {
                string[] keys = dup.Value.Split(':');
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < keys.Length; i++)
                {
                    if (keyData != null) sb.Append("　" + keyData[i + 1] + " : " + keys[i]);

                }
                //err.Add(func + "　"  + dup.Key + "行目" + sb.ToString() + "　は取込データが重複しています。");
                err.Add(dup.Key + "行目" + sb.ToString() + "　は取込データが重複しています。");
            }
        }

        private static void DuplicateDBErrorString(ref List<string> err, ApplicationDB dbContext, MControl targetData, Dictionary<int, string> headerData, Dictionary<int, string> duplicate, Dictionary<int, string> keyData, Dictionary<int, string> keyDataJ, String func)
        {
            foreach (KeyValuePair<int, string> dup in duplicate)
            {
                string[] keys = dup.Value.Split(':');
                int xCount = keys.Count();

                int ret = CheckDataCountSQL(dbContext, targetData.Value2, xCount, keys, headerData, keyData);
                if (func == Insert)
                {
                    if (ret != 0) err.Add(ErrorString(Insert, xCount, keys, headerData, keyData, keyDataJ, dup.Key));
                } else if (func == Update)
                {
                    if (ret == 0) err.Add(ErrorString(Update, xCount, keys, headerData, keyData, keyDataJ, dup.Key));
                } else if (func == Delete)
                {
                    if (ret == 0) err.Add(ErrorString(Delete, xCount, keys, headerData, keyData, keyDataJ, dup.Key));
                }

            }
        }

        private static String ErrorString(String func, int xCount, string[] data, Dictionary<int, string> columns, Dictionary<int, string> keyData, Dictionary<int, string> keyDataJ, int count)
        {
            count = count + 2;
            StringBuilder error = new StringBuilder();
            error.Append(func + "　" + count + "行目　");
            foreach(KeyValuePair<int, string> key in keyData)
            {
                for (int i = 1; i <= xCount; i++)
                {
                    if (key.Value.Equals(columns[i]))
                    {
                        if (keyDataJ != null)
                        {
                            error.Append(keyDataJ[key.Key] + " : " + data[i] + "　");
                        } else
                        {
                            error.Append(key.Value + " : " + data[i] + "　");
                        }
                        break;
                    }
                }
            }
            if (func == Insert)
            {
                return error.ToString() + "は既に登録済みです。";
            } else
            {
                return error.ToString() + "は未登録です。";
            }
        }

        private class MyProperty
        {
            public string Name { get; set; }
            public string PhysicsName { get; set; }
            public string LogicalName { get; set; }
            public bool Required { get; set; }
            public int Length { get; set; }
            public bool Key { get; set; }
            public Type Type { get; set; }
            public string RegularExpression { get; set; }
            public bool FileRequired { get; set; }
            public bool FileDoubleRequired { get; set; }
            public int FileLength { get; set; }
        }

    }
}