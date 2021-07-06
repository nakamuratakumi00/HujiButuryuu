using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Macss.Areas.Fdass.Common;
using Macss.Models;

namespace Macss.Repositories
{
    public class ControlRepository : IControlRepository
    {
        #region 定数

        /// <summary>
        /// MControlのセクションリスト
        /// </summary>
        public static class MControlSection
        {

            public const string Group = "Group";
            public const string LogFunction = "LogFunction";
            public const string MasterMaintenance = "MasterMaintenance";
            public const string LoginCount = "LoginCount";
            public const string Password = "Password";
            public const string Information = "Information";

            public const string Login = "A0001";              // ログイン

            // 末締処理
            public const string MatujimeStatus = "MatujimeStatus";              // 実行ステータス
            public const string MatujimeFile = "MatujimeFile";                  // 末締ファイル読込場所

            // 単価マスタ読込
            public const string TankaFile = "TankaFile";                        // 単価ファイル読込場所

            // 見積印刷
            public const string MitumoriFile = "MitumoriFile";                 // 見積書Excel一時保存場所

            // "保管請求拠点別データ"
            public const string KyotenFile = "KyotenFile";                     // 拠点別Excel一時保存場所


            // アカウントマスタ
            public const string AccountBumon = "AccountBumon";                  // 部門
            public const string AccountGroup = "AccountGroup";                  // 照会グループ

            //コントロールマスタ
            public const string KenCod = "KenCod";                                  // 県コード

            // 品名マスタ
            public const string HinmeiExtraction = "HinmeiExtraction";          // 抽出フラグ

            // 出荷注文書
            public const string UFutanExtraction = "UFutan";                    // 運賃負担
            public const string TokuteiShiteiExtraction = "TokuteiShitei";      // 特別指定コード
        }

        /// <summary>
        /// MControlの処理内容IDリスト
        /// </summary>
        public static class MControlFunctionKbn
        {
            public const string U1 = "U1";  // 更新（新規）
            public const string U2 = "U2";  // 更新（修正）
            public const string U3 = "U3";  // 更新（削除）
            public const string S1 = "S1";  // 照会（詳細）
            public const string S2 = "S2";  // 照会（一覧）
            public const string O1 = "O1";  // ファイル出力
            public const string I1 = "I1";  // ファイル取り込み
            public const string C1 = "C1";  // ファイル連携
            public const string E1 = "E1";  // エラー
            public const string L = "L";    // ログイン
        }

        #endregion

        private readonly ApplicationDB dbContext;

        public ControlRepository(ApplicationDB db)
        {
            this.dbContext = db;
        }

        public async Task<IEnumerable<MControl>> GetDataBySectionAsync(string section)
        {
            return await dbContext.MControl.Where(x => x.Section == section).OrderBy(x => x.SortNo).ToListAsync();
        }

        public async Task UpdateDataValueAsync(string section, string kbn, string value1, string value2, string value3,
            object nValue1, object nValue2, object nValue3, string userID)
        {
            var controlData = await dbContext.MControl.FirstOrDefaultAsync(x => x.Section == section && x.Kbn == kbn);
            if (controlData == null) { return; }

            //if (!string.IsNullOrEmpty(value1)) controlData.Value1 = value1;
            //if (!string.IsNullOrEmpty(value2)) controlData.Value2 = value2;
            //if (!string.IsNullOrEmpty(value3)) controlData.Value3 = value3;

            controlData.Value1 = value1;
            controlData.Value2 = value2;
            controlData.Value3 = value3;

            if (decimal.TryParse((string)nValue1, out decimal val1)) controlData.NumericValue1 = val1;
            if (decimal.TryParse((string)nValue2, out decimal val2)) controlData.NumericValue2 = val2;
            if (decimal.TryParse((string)nValue3, out decimal val3)) controlData.NumericValue3 = val3;

            try
            {
                controlData.UpdateId = userID;
                controlData.UpdateDate = DateTime.Now;
                await dbContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                DataUtil.PrintEntityValidationErrors(dbEx.EntityValidationErrors);
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return;
        }
    }
}