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
    public class TempLoadingSlipInvoiceDAO : ITempLoadingSlipInvoiceDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TempLoadingSlipInvoiceDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tempLoadingSlipInvoice]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoice()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idLoadingSlipInvoice = " + idLoadingSlipInvoice +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
            }
            catch(Exception ex)
            {
                  
                   
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoice(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                  
                   
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }


        public List<TempLoadingSlipInvoiceTO> ConvertDTToList(SqlDataReader tempLoadingSlipInvoiceTODT)
        {
            List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = new List<TempLoadingSlipInvoiceTO>();
            if (tempLoadingSlipInvoiceTODT != null)
            {
                while (tempLoadingSlipInvoiceTODT.Read())
                {
                    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTONew = new TempLoadingSlipInvoiceTO();
                    if (tempLoadingSlipInvoiceTODT["idLoadingSlipInvoice"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.IdLoadingSlipInvoice = Convert.ToInt32(tempLoadingSlipInvoiceTODT["idLoadingSlipInvoice"].ToString());
                    if (tempLoadingSlipInvoiceTODT["loadingSlipId"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.LoadingSlipId = Convert.ToInt32(tempLoadingSlipInvoiceTODT["loadingSlipId"].ToString());
                    if (tempLoadingSlipInvoiceTODT["invoiceId"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.InvoiceId = Convert.ToInt32(tempLoadingSlipInvoiceTODT["invoiceId"].ToString());
                    if (tempLoadingSlipInvoiceTODT["createdBy"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.CreatedBy = Convert.ToInt32(tempLoadingSlipInvoiceTODT["createdBy"].ToString());
                    if (tempLoadingSlipInvoiceTODT["updatedBy"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.UpdatedBy = Convert.ToInt32(tempLoadingSlipInvoiceTODT["updatedBy"].ToString());
                    if (tempLoadingSlipInvoiceTODT["createdOn"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.CreatedOn = Convert.ToDateTime(tempLoadingSlipInvoiceTODT["createdOn"].ToString());
                    if (tempLoadingSlipInvoiceTODT["updatedOn"] != DBNull.Value)
                        tempLoadingSlipInvoiceTONew.UpdatedOn = Convert.ToDateTime(tempLoadingSlipInvoiceTODT["updatedOn"].ToString());
                    tempLoadingSlipInvoiceTOList.Add(tempLoadingSlipInvoiceTONew);
                }
            }
            return tempLoadingSlipInvoiceTOList;
        }

        public List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoiceList(String loadingSlipIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " Where loadingSlipId IN (" + loadingSlipIds  + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {


                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE invoiceId = " + invoiceId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(reader);
                
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOByInvoiceId(Int32 invoiceId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE invoiceId = " + invoiceId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }


        public TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOListByLoadingSlip(int loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipId = " + loadingSlipId ;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOList(int loadingSlipId, int invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipId = "+ loadingSlipId + " AND invoiceId = " + invoiceId + " "; 
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TempLoadingSlipInvoiceTO> list= ConvertDTToList(reader);
                if (list != null && list.Count==1)
                    return list[0];
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tempLoadingSlipInvoiceTO, cmdInsert);
            }
            catch(Exception ex)
            {
                  
                   
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tempLoadingSlipInvoiceTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlipInvoice]( " + 
            "  [loadingSlipId]" +
            " ,[invoiceId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            "  @LoadingSlipId " +
            " ,@InvoiceId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.InvoiceId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tempLoadingSlipInvoiceTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tempLoadingSlipInvoiceTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tempLoadingSlipInvoiceTO.UpdatedOn);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tempLoadingSlipInvoiceTO.IdLoadingSlipInvoice = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tempLoadingSlipInvoiceTO, cmdUpdate);
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

        public int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tempLoadingSlipInvoiceTO, cmdUpdate);
            }
            catch(Exception ex)
            {
               
                   
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlipInvoice] SET " +

            "  [loadingSlipId]= @LoadingSlipId" +
            " ,[invoiceId]= @InvoiceId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE   [idLoadingSlipInvoice] = @IdLoadingSlipInvoice";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingSlipInvoice", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.IdLoadingSlipInvoice;
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.LoadingSlipId;
            cmdUpdate.Parameters.Add("@InvoiceId", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.InvoiceId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tempLoadingSlipInvoiceTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tempLoadingSlipInvoiceTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingSlipInvoice, cmdDelete);
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

        public int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingSlipInvoice, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idLoadingSlipInvoice, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipInvoice] " +
            " WHERE idLoadingSlipInvoice = " + idLoadingSlipInvoice +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlipInvoice", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.IdLoadingSlipInvoice;
            return cmdDelete.ExecuteNonQuery();
        }


        public int DeleteTempLoadingSlipInvoiceByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommandByInvoiceId(invoiceId, cmdDelete);
            }
            catch (Exception ex)
            {


                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipInvoice] " +
            " WHERE invoiceId = " + invoiceId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlipInvoice", System.Data.SqlDbType.Int).Value = tempLoadingSlipInvoiceTO.IdLoadingSlipInvoice;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
