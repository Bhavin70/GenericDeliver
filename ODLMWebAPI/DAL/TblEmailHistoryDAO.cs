using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblEmailHistoryDAO : ITblEmailHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblEmailHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblEmailHistory]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        //public DataTable SelectAllTblEmailHistory()
        //{
        //    String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSelectQuery();
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idEmailHistory", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.IdEmailHistory;
        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //        String userName = System.Windows.Forms.SystemInformation.UserName;
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        //public DataTable SelectTblEmailHistory(Int32 idEmailHistory)
        //{
        //    String sqlConnStr = Masters.GlobalConnectionString.ActiveConnectionString;
        //    SqlConnection conn = new SqlConnection(sqlConnStr);
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        conn.Open();
        //        cmdSelect.CommandText = SqlSelectQuery() + " WHERE idEmailHistory = " + idEmailHistory + " ";
        //        cmdSelect.Connection = conn;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idEmailHistory", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.IdEmailHistory;
        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //        String userName = System.Windows.Forms.SystemInformation.UserName;
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //        cmdSelect.Dispose();
        //    }
        //}

        //public DataTable SelectAllTblEmailHistory(SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdSelect = new SqlCommand();
        //    try
        //    {
        //        cmdSelect.CommandText = SqlSelectQuery();
        //        cmdSelect.Connection = conn;
        //        cmdSelect.Transaction = tran;
        //        cmdSelect.CommandType = System.Data.CommandType.Text;

        //        //cmdSelect.Parameters.Add("@idEmailHistory", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.IdEmailHistory;
        //        SqlDataAdapter da = new SqlDataAdapter(cmdSelect);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        String computerName = System.Windows.Forms.SystemInformation.ComputerName;
        //        String userName = System.Windows.Forms.SystemInformation.UserName;
        //        return null;
        //    }
        //    finally
        //    {
        //        cmdSelect.Dispose();
        //    }
        //}

        #endregion

        #region Insertion
        public int InsertTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblEmailHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblEmailHistoryTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblEmailHistoryTO tblEmailHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblEmailHistory]( " +
           // "  [idEmailHistory]" +
            " [createdBy]" +
            " ,[invoiceId]" +
            " ,[sendOn]" +
            " ,[sendBy]" +
            " ,[sendTo]" +
            " ,[cc]" +
            " )" +
" VALUES (" +
            //"  @IdEmailHistory " +
            " @CreatedBy " +
            " ,@InvoiceId " +
            " ,@SendOn " +
            " ,@SendBy " +
            " ,@SendTo " +
            " ,@Cc " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdEmailHistory", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.IdEmailHistory;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.InvoiceId;
            cmdInsert.Parameters.Add("@SendOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblEmailHistoryTO.SendOn);
            cmdInsert.Parameters.Add("@SendBy", System.Data.SqlDbType.NVarChar).Value = tblEmailHistoryTO.SendBy;
            cmdInsert.Parameters.Add("@SendTo", System.Data.SqlDbType.NVarChar).Value = tblEmailHistoryTO.SendTo;
            cmdInsert.Parameters.Add("@Cc", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblEmailHistoryTO.Cc);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblEmailHistoryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblEmailHistoryTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblEmailHistoryTO tblEmailHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblEmailHistory] SET " +
            "  [idEmailHistory] = @IdEmailHistory" +
            " ,[createdBy]= @CreatedBy" +
            " ,[invoiceId]= @InvoiceId" +
            " ,[sendOn]= @SendOn" +
            " ,[sendBy]= @SendBy" +
            " ,[sendTo]= @SendTo" +
            " ,[cc] = @Cc" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdEmailHistory", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.IdEmailHistory;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.InvoiceId;
            cmdUpdate.Parameters.Add("@SendOn", System.Data.SqlDbType.DateTime).Value = tblEmailHistoryTO.SendOn;
            cmdUpdate.Parameters.Add("@SendBy", System.Data.SqlDbType.NVarChar).Value = tblEmailHistoryTO.SendBy;
            cmdUpdate.Parameters.Add("@SendTo", System.Data.SqlDbType.NVarChar).Value = tblEmailHistoryTO.SendTo;
            cmdUpdate.Parameters.Add("@Cc", System.Data.SqlDbType.NVarChar).Value = tblEmailHistoryTO.Cc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblEmailHistory(Int32 idEmailHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idEmailHistory, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblEmailHistory(Int32 idEmailHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idEmailHistory, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idEmailHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblEmailHistory] " +
            " WHERE idEmailHistory = " + idEmailHistory + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idEmailHistory", System.Data.SqlDbType.Int).Value = tblEmailHistoryTO.IdEmailHistory;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
