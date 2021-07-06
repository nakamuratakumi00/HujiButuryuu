using Macss.Areas.Tass.ViewModels;
using Macss.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Macss.Areas.Tass.Repositories
{
    public class ShuukaTyuumonshoTorikomiRepositorie : IShuukaTyuumonshoTorikomiRepositorie

    {
        private readonly ApplicationDB dbContext;

        public ShuukaTyuumonshoTorikomiRepositorie(ApplicationDB db)
        {
            this.dbContext = db;
        }

        // 表示(WORKテーブル出力)
        public int GetTorikomiKouho(TorikomiSerch torikomi)
        {
            int insRowCnt = 0;

            // 未取込情報を抽出 -> WORKテーブルへ出力
            // 複数テーブルが対象、且つ、WORKテーブルを使用する為、直接SQL発行
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString;
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                System.Data.SqlClient.SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {

                    command.CommandText = InsertWorkTableSQL();

                    System.Data.SqlClient.SqlParameter param = command.CreateParameter();
                    param.ParameterName = "@Actcod";
                    param.SqlDbType = System.Data.SqlDbType.NChar;
                    param.Direction = System.Data.ParameterDirection.Input;
                    param.Value = torikomi.Actcod;
                    command.Parameters.Add(param);

                    param = command.CreateParameter();
                    param.ParameterName = "@Ackymd";
                    param.SqlDbType = System.Data.SqlDbType.Decimal;
                    param.Direction = System.Data.ParameterDirection.Input;
                    param.Value = torikomi.Ackymd;
                    command.Parameters.Add(param);

                    insRowCnt = command.ExecuteNonQuery();

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    insRowCnt = 0;
                    throw ex;
                }
            }

            return insRowCnt;
        }
        private string InsertWorkTableSQL()
        {
            var query = @" INSERT INTO w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho 
                           (      ACTCOD 
                                , ACKYMD 
                                , SYUKNO 
                                , CDATE  
                                , RENBAN 
                                , HINCOD 
                                , HINNAM 
                                , SYUKSU 
                                , CRTCOD 
                                , CRTYMD 
                                , UPDCOD 
                                , UPDYMD 
                           ) 
                           SELECT @Actcod 
                                , @Ackymd 
                                , SYUKNO
                                , CDATE
                                , RENBAN
                                , HINCOD
                                , HINNAM
                                , SYUKSU
                                , CRTCOD
                                , CRTYMD
                                , UPDCOD
                                , UPDYMD
                             FROM t_unsou_shuuka_tyuumonsho_tehai_mk moto
                            WHERE NOT EXISTS ( 
                                               SELECT 1
                                                 FROM t_unsou_shuuka_tyuumonsho_tehai saki
                                                WHERE saki.SYUKNO = moto.SYUKNO
                                                  AND saki.CDATE = moto.CDATE
                                             ) 
                         ;

                         INSERT INTO w_unsou_shuuka_tyuumonsho_tehai_kouho
                         (      ACTCOD
                              , ACKYMD
                              , SYUKNO
                              , CDATE
                              , SYKYMD
                              , KISYU
                              , KEIFNO
                              , FSYKNO
                              , SYBCOD
                              , TOKCOD
                              , SEICOD
                              , HTYNAM
                              , HTYKAH
                              , TANCOD
                              , TANNAM
                              , HTYTEL
                              , BASYO
                              , TDKCOD
                              , TDKYUB
                              , TDKJYU
                              , TDKNAM
                              , TDSNAM
                              , TDBNAM
                              , TDKTAN
                              , TDKTEL
                              , DHINCOD
                              , DHINNAM
                              , DSYUKSU
                              , TKJIKO
                              , COMENT
                              , UNSCOD
                              , UNSCRS
                              , SIRCOD
                              , UNSKBN
                              , DENKBN
                              , DENMSU
                              , UFUTAN
                              , YUSONO
                              , PCCOD
                              , CRTCOD
                              , CRTYMD
                              , UPDCOD
                              , UPDYMD
                              , TORNAM
                         )
                         SELECT @Actcod
                              , @Ackymd
                              , SYUKNO
                              , CDATE
                              , SYKYMD
                              , KISYU
                              , KEIFNO
                              , FSYKNO
                              , SYBCOD
                              , TOKCOD
                              , SEICOD
                              , HTYNAM
                              , HTYKAH
                              , TANCOD
                              , moto.TANNAM
                              , HTYTEL
                              , BASYO
                              , TDKCOD
                              , TDKYUB
                              , TDKJYU
                              , TDKNAM
                              , TDSNAM
                              , TDBNAM
                              , TDKTAN
                              , TDKTEL
                              , DHINCOD
                              , DHINNAM
                              , DSYUKSU
                              , TKJIKO
                              , COMENT
                              , UNSCOD
                              , UNSCRS
                              , SIRCOD
                              , UNSKBN
                              , DENKBN
                              , DENMSU
                              , UFUTAN
                              , YUSONO
                              , PCCOD
                              , moto.CRTCOD
                              , moto.CRTYMD
                              , moto.UPDCOD
                              , moto.UPDYMD
                              , TORNAM
                           FROM t_unsou_shuuka_tyuumonsho_tehai_k moto
                           LEFT JOIN m_torihikisaki
                             ON TORCOD = TOKCOD
                          WHERE NOT EXISTS (
                                             SELECT 1
                                               FROM t_unsou_shuuka_tyuumonsho_tehai saki
                                              WHERE saki.SYUKNO = moto.SYUKNO
                                                AND saki.CDATE = moto.CDATE
                                         )
                       ; ";

            return query;
        }

        // 表示
        public async Task<IEnumerable<TorikomiInformation>> DispTorikomiKouhoAsync(TorikomiSerch torikomi)
        {
            var torikomiKouhoList = await dbContext.WUnsouShuukaTyuumonshoTehaiKouho
                .Where(x => x.Actcod == torikomi.Actcod &&
                            x.Ackymd == torikomi.Ackymd)
                .Select(x => new TorikomiInformation()
                {
                    Sykymd = String.Empty,
                    Syukno = x.Syukno,
                    KeFsno = x.Keifno + Environment.NewLine + x.Fsykno,
                    Tdknam = x.Tdknam + " " + x.Tdsnam + " " + x.Tdbnam + " " + x.Tdktan,
                    Dhinnam = x.Dhinnam,
                    Sybcod = x.Sybcod,
                    Tokcod = x.Tokcod,
                    Toknam = x.Tornam,
                    Cdate = x.Cdate,
                    TSykymd = (DateTime)x.Sykymd
                })
                .ToListAsync();

            // DateTime?文字列変換用
            var torikomiKouhoList2 = torikomiKouhoList
                .Select(x => new TorikomiInformation()
                {
                    Sykymd = x.TSykymd.ToString("yyyy/MM/dd"),
                    Syukno = Common.DataUtil.GetSyukno(x.Syukno),
                    KeFsno = x.KeFsno,
                    Tdknam = x.Tdknam,
                    Dhinnam = x.Dhinnam,
                    Sybcod = x.Sybcod,
                    Tokcod = x.Tokcod,
                    Toknam = x.Toknam,
                    Cdate = x.Cdate,
                    TSykymd = x.TSykymd
                })
                .OrderByDescending(x => x.TSykymd.ToString("yyyy/MM/dd"))
                .ThenByDescending(x => x.Syukno);

            return torikomiKouhoList2;
        }

        // データ取込
        public int InsertTorikomiData(TorikomiSerch torikomi)
        {
            int insRowCnt = 0;

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationDB"].ConnectionString;
            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                System.Data.SqlClient.SqlTransaction transaction = connection.BeginTransaction();
                command.Transaction = transaction;
                try
                {
                    System.Data.SqlClient.SqlParameter param = command.CreateParameter();
                    param.ParameterName = "@Actcod";
                    param.SqlDbType = System.Data.SqlDbType.NChar;
                    param.Direction = System.Data.ParameterDirection.Input;
                    param.Value = torikomi.Actcod;
                    command.Parameters.Add(param);

                    param = command.CreateParameter();
                    param.ParameterName = "@Ackymd";
                    param.SqlDbType = System.Data.SqlDbType.Decimal;
                    param.Direction = System.Data.ParameterDirection.Input;
                    param.Value = torikomi.Ackymd;
                    command.Parameters.Add(param);

                    // Insert ヘッダ
                    command.CommandText = InsertTyuumonshoTableSQL();
                    insRowCnt = command.ExecuteNonQuery();

                    // Insert 明細
                    command.CommandText = InsertTyuumonshoMeisaiTableSQL();
                    command.ExecuteNonQuery();

                    // Delete WORKテーブル
                    command.CommandText = DeleteWorkTableSQL();
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    insRowCnt = 0;
                }
            }

            return insRowCnt;
        }
        private string InsertTyuumonshoTableSQL()
        {
            var query = @" INSERT INTO t_unsou_shuuka_tyuumonsho_tehai
                           (      SYUKNO
                                , CDATE
                                , SYKYMD
                                , KISYU
                                , KEIFNO
                                , FSYKNO
                                , SYBCOD
                                , TOKCOD
                                , SEICOD
                                , HTYNAM
                                , HTYKAH
                                , TANCOD
                                , TANNAM
                                , HTYTEL
                                , BASYO
                                , TDKCOD
                                , TDKYUB
                                , TDKJYU
                                , TDKNAM
                                , TDSNAM
                                , TDBNAM
                                , TDKTAN
                                , TDKTEL
                                , DHINCOD
                                , DHINNAM
                                , DSYUKSU
                                , TKJIKO
                                , COMENT
                                , UNSCOD
                                , UNSCRS
                                , SIRCOD
                                , UNSKBN
                                , DENKBN
                                , DENMSU
                                , UFUTAN
                                , YUSONO
                                , DENF
                                , HKUYMD
                                , HKUCOD
                                , PCCOD
                                , YUBFLG
                                , MUKOUKBN
                                , MUKOURIYUU
                                , INSCOD
                                , CRTCOD
                                , CRTYMD
                                , UPDCOD
                                , UPDYMD
                           )
                           SELECT SYUKNO
                                , CDATE
                                , SYKYMD
                                , KISYU
                                , KEIFNO
                                , FSYKNO
                                , SYBCOD
                                , TOKCOD
                                , SEICOD
                                , HTYNAM
                                , HTYKAH
                                , TANCOD
                                , TANNAM
                                , HTYTEL
                                , BASYO
                                , TDKCOD
                                , TDKYUB
                                , TDKJYU
                                , TDKNAM
                                , TDSNAM
                                , TDBNAM
                                , TDKTAN
                                , TDKTEL
                                , DHINCOD
                                , DHINNAM
                                , DSYUKSU
                                , TKJIKO
                                , COMENT
                                , UNSCOD
                                , UNSCRS
                                , SIRCOD
                                , UNSKBN
                                , DENKBN
                                , DENMSU
                                , UFUTAN
                                , YUSONO
                                , NULL
                                , NULL
                                , NULL
                                , PCCOD
                                , NULL
                                , 0
                                , NULL
                                , ''
                                , @Actcod
                                , CURRENT_TIMESTAMP
                                , @Actcod
                                , CURRENT_TIMESTAMP
                             FROM w_unsou_shuuka_tyuumonsho_tehai_kouho moto
                            WHERE ACTCOD = @Actcod
                              AND ACKYMD = @Ackymd
                           ; ";

            return query;
        }
        private string InsertTyuumonshoMeisaiTableSQL()
        {
            var query = @" INSERT INTO t_unsou_shuuka_tyuumonsho_tehai_meisai 
                           (      SYUKNO 
                                , CDATE  
                                , RENBAN 
                                , HINCOD 
                                , HINNAM 
                                , SYUKSU 
                                , CRTCOD 
                                , CRTYMD 
                                , UPDCOD 
                                , UPDYMD 
                           ) 
                           SELECT SYUKNO
                                , CDATE
                                , RENBAN
                                , HINCOD
                                , HINNAM
                                , SYUKSU
                                , @Actcod
                                , CURRENT_TIMESTAMP
                                , @Actcod
                                , CURRENT_TIMESTAMP
                             FROM w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho moto
                            WHERE ACTCOD = @Actcod
                              AND ACKYMD = @Ackymd
                           ; ";

            return query;
        }
        private string DeleteWorkTableSQL()
        {
            var query = @" DELETE FROM w_unsou_shuuka_tyuumonsho_tehai_kouho 
                            WHERE ACTCOD =  @Actcod
                              AND ACKYMD <= @Ackymd
                           ;

                           DELETE FROM w_unsou_shuuka_tyuumonsho_tehai_meisai_kouho 
                            WHERE ACTCOD =  @Actcod
                              AND ACKYMD <= @Ackymd
                           ; ";

            return query;
        }
    }
}