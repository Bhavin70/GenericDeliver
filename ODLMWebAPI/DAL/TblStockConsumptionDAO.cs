using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblStockConsumptionDAO : ITblStockConsumptionDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblStockConsumptionDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblStockConsumption]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblStockConsumptionTO> SelectAllTblStockConsumption()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockConsumptionTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblStockConsumptionTO> SelectAllStockConsumptionList(Int32 loadingSlipExtId, Int32 txnOpTypeId, SqlConnection conn, SqlTransaction tran)
        {
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipExtId=" + loadingSlipExtId + " AND txnOpTypeId=" + txnOpTypeId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockConsumptionTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public Int64 GetLastIdStockConsumption(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "select isnull(max(isnull(idStockConsumption,0)),0) +  1  as Id from tblStockConsumption";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return Convert.ToInt64(cmdSelect.ExecuteScalar());

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public TblStockConsumptionTO SelectTblStockConsumption(Int32 idStockConsumption)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idStockConsumption = " + idStockConsumption + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockConsumptionTO> list = ConvertDTToList(reader);
                if (reader != null)
                    reader.Dispose();

                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
     

        public List<TblStockConsumptionTO> ConvertDTToList(SqlDataReader tblStockConsumptionTODT)
        {
            List<TblStockConsumptionTO> tblStockConsumptionTOList = new List<TblStockConsumptionTO>();
            if (tblStockConsumptionTODT != null)
            {
                while (tblStockConsumptionTODT.Read())
                {
                    TblStockConsumptionTO tblStockConsumptionTONew = new TblStockConsumptionTO();
                    if (tblStockConsumptionTODT["idStockConsumption"] != DBNull.Value)
                        tblStockConsumptionTONew.IdStockConsumption = Convert.ToInt32(tblStockConsumptionTODT["idStockConsumption"].ToString());
                    if (tblStockConsumptionTODT["stockDtlId"] != DBNull.Value)
                        tblStockConsumptionTONew.StockDtlId = Convert.ToInt32(tblStockConsumptionTODT["stockDtlId"].ToString());
                    if (tblStockConsumptionTODT["loadingSlipExtId"] != DBNull.Value)
                        tblStockConsumptionTONew.LoadingSlipExtId = Convert.ToInt32(tblStockConsumptionTODT["loadingSlipExtId"].ToString());
                    if (tblStockConsumptionTODT["transferNoteId"] != DBNull.Value)
                        tblStockConsumptionTONew.TransferNoteId = Convert.ToInt32(tblStockConsumptionTODT["transferNoteId"].ToString());
                    if (tblStockConsumptionTODT["txnOpTypeId"] != DBNull.Value)
                        tblStockConsumptionTONew.TxnOpTypeId = Convert.ToInt32(tblStockConsumptionTODT["txnOpTypeId"].ToString());
                    if (tblStockConsumptionTODT["createdBy"] != DBNull.Value)
                        tblStockConsumptionTONew.CreatedBy = Convert.ToInt32(tblStockConsumptionTODT["createdBy"].ToString());
                    if (tblStockConsumptionTODT["createdOn"] != DBNull.Value)
                        tblStockConsumptionTONew.CreatedOn = Convert.ToDateTime(tblStockConsumptionTODT["createdOn"].ToString());
                    if (tblStockConsumptionTODT["beforeStockQty"] != DBNull.Value)
                        tblStockConsumptionTONew.BeforeStockQty = Convert.ToDouble(tblStockConsumptionTODT["beforeStockQty"].ToString());
                    if (tblStockConsumptionTODT["afterStockQty"] != DBNull.Value)
                        tblStockConsumptionTONew.AfterStockQty = Convert.ToDouble(tblStockConsumptionTODT["afterStockQty"].ToString());
                    if (tblStockConsumptionTODT["txnQty"] != DBNull.Value)
                        tblStockConsumptionTONew.TxnQty = Convert.ToDouble(tblStockConsumptionTODT["txnQty"].ToString());
                    if (tblStockConsumptionTODT["remark"] != DBNull.Value)
                        tblStockConsumptionTONew.Remark = Convert.ToString(tblStockConsumptionTODT["remark"].ToString());
                    tblStockConsumptionTOList.Add(tblStockConsumptionTONew);
                }
            }
            return tblStockConsumptionTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockConsumptionTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockConsumptionTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblStockConsumptionTO tblStockConsumptionTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockConsumption]( " +
                           
                            " [stockDtlId]" +
                            " ,[loadingSlipExtId]" +
                            " ,[transferNoteId]" +
                            " ,[txnOpTypeId]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[beforeStockQty]" +
                            " ,[afterStockQty]" +
                            " ,[txnQty]" +
                            " ,[remark]" +

                            " ,[tranId]" +
                            " ,[tranTypeId]" +
                            " )" +
                " VALUES (" +
                          
                            " @StockDtlId " +
                            " ,@LoadingSlipExtId " +
                            " ,@TransferNoteId " +
                            " ,@TxnOpTypeId " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@BeforeStockQty " +
                            " ,@AfterStockQty " +
                            " ,@TxnQty " +
                            " ,@Remark " +

                            " ,@TranId " +
                            " ,@TranTypeId " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStockConsumption", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.IdStockConsumption;
            cmdInsert.Parameters.Add("@StockDtlId", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.StockDtlId;
            cmdInsert.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockConsumptionTO.LoadingSlipExtId);
            cmdInsert.Parameters.Add("@TransferNoteId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockConsumptionTO.TransferNoteId);
            cmdInsert.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.TxnOpTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockConsumptionTO.CreatedOn;
            cmdInsert.Parameters.Add("@BeforeStockQty", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.BeforeStockQty;
            cmdInsert.Parameters.Add("@AfterStockQty", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.AfterStockQty;
            cmdInsert.Parameters.Add("@TxnQty", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.TxnQty;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockConsumptionTO.Remark);


            cmdInsert.Parameters.Add("@TranId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockConsumptionTO.TranId);
            cmdInsert.Parameters.Add("@TranTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockConsumptionTO.TranTypeId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                //cmdInsert.CommandText = Constants.IdentityColumnQuery;
                //tblStockConsumptionTO.IdStockConsumption = Convert.ToInt32(cmdInsert.ExecuteScalar());
                cmdInsert.CommandText = Constants.SQL_SELECT_IDENTITY_QUERY;
                tblStockConsumptionTO.IdStockConsumption = Convert.ToInt64(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockConsumptionTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockConsumptionTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                
               
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblStockConsumptionTO tblStockConsumptionTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockConsumption] SET " + 
             
            " [stockDtlId]= @StockDtlId" +
            " ,[loadingSlipExtId]= @LoadingSlipExtId" +
            " ,[transferNoteId]= @TransferNoteId" +
            " ,[txnOpTypeId]= @TxnOpTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[beforeStockQty]= @BeforeStockQty" +
            " ,[afterStockQty]= @AfterStockQty" +
            " ,[txnQty]= @TxnQty" +
            " ,[remark] = @Remark" +
            " WHERE [idStockConsumption] = @IdStockConsumption"  ; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStockConsumption", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.IdStockConsumption;
            cmdUpdate.Parameters.Add("@StockDtlId", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.StockDtlId;
            cmdUpdate.Parameters.Add("@LoadingSlipExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblStockConsumptionTO.LoadingSlipExtId);
            cmdUpdate.Parameters.Add("@TransferNoteId", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.TransferNoteId;
            cmdUpdate.Parameters.Add("@TxnOpTypeId", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.TxnOpTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblStockConsumptionTO.CreatedOn;
            cmdUpdate.Parameters.Add("@BeforeStockQty", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.BeforeStockQty;
            cmdUpdate.Parameters.Add("@AfterStockQty", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.AfterStockQty;
            cmdUpdate.Parameters.Add("@TxnQty", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.TxnQty;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblStockConsumptionTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblStockConsumption(Int32 idStockConsumption)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStockConsumption, cmdDelete);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblStockConsumption(Int32 idStockConsumption, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStockConsumption, cmdDelete);
            }
            catch(Exception ex)
            {
                
               
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idStockConsumption, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockConsumption] " +
            " WHERE idStockConsumption = " + idStockConsumption +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStockConsumption", System.Data.SqlDbType.Int).Value = tblStockConsumptionTO.IdStockConsumption;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
