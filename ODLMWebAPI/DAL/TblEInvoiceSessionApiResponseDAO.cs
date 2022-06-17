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
    public class TblEInvoiceSessionApiResponseDAO : ITblEInvoiceSessionApiResponseDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblEInvoiceSessionApiResponseDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT idResponse, apiId, accessToken, responseStatus, response, createdBy, createdOn " +
                                  " FROM TblEInvoiceSessionApiResponse";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponse()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceSessionApiResponseTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponse(Int32 apiId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE apiId=" + apiId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceSessionApiResponseTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblEInvoiceSessionApiResponseTO> SelectTblEInvoiceSessionApiResponse(Int32 idResponse)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idResponse=" + idResponse;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblEInvoiceSessionApiResponseTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblEInvoiceSessionApiResponseTO> ConvertDTToList(SqlDataReader TblEInvoiceSessionApiResponseTODT)
        {
            List<TblEInvoiceSessionApiResponseTO> TblEInvoiceSessionApiResponseTOList = new List<TblEInvoiceSessionApiResponseTO>();
            if (TblEInvoiceSessionApiResponseTODT != null)
            {
                while(TblEInvoiceSessionApiResponseTODT.Read())
                {
                    TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTONew = new TblEInvoiceSessionApiResponseTO();
                    if (TblEInvoiceSessionApiResponseTODT["idResponse"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.IdResponse = Convert.ToInt32(TblEInvoiceSessionApiResponseTODT["idResponse"].ToString());
                    if (TblEInvoiceSessionApiResponseTODT["apiId"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.ApiId = Convert.ToInt32(TblEInvoiceSessionApiResponseTODT["apiId"].ToString());
                    if (TblEInvoiceSessionApiResponseTODT["accessToken"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.AccessToken = TblEInvoiceSessionApiResponseTODT["accessToken"].ToString();
                    if (TblEInvoiceSessionApiResponseTODT["responseStatus"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.ResponseStatus = TblEInvoiceSessionApiResponseTODT["responseStatus"].ToString();
                    if (TblEInvoiceSessionApiResponseTODT["response"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.Response = TblEInvoiceSessionApiResponseTODT["response"].ToString();
                    if (TblEInvoiceSessionApiResponseTODT["createdBy"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.CreatedBy = Convert.ToInt32(TblEInvoiceSessionApiResponseTODT["createdBy"].ToString());
                    if (TblEInvoiceSessionApiResponseTODT["createdOn"] != DBNull.Value)
                        TblEInvoiceSessionApiResponseTONew.CreatedOn = Convert.ToDateTime(TblEInvoiceSessionApiResponseTODT["createdOn"].ToString());
                    TblEInvoiceSessionApiResponseTOList.Add(TblEInvoiceSessionApiResponseTONew);
                }
            }
            return TblEInvoiceSessionApiResponseTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(TblEInvoiceSessionApiResponseTO, cmdInsert);
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

        public int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(TblEInvoiceSessionApiResponseTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [TblEInvoiceSessionApiResponse]( " +
                                "  [apiId]" +
                                " ,[accessToken]" +
                                " ,[responseStatus]" +
                                " ,[response]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[OrgId]" +
                                " )" +
                    " VALUES (" +
                                "  @ApiId " +
                                " ,@AccessToken " +
                                " ,@ResponseStatus " +
                                " ,@Response " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@OrgId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdResponse", System.Data.SqlDbType.Int).Value = TblEInvoiceSessionApiResponseTO.IdResponse;
            cmdInsert.Parameters.Add("@ApiId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceSessionApiResponseTO.ApiId);
            cmdInsert.Parameters.Add("@AccessToken", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceSessionApiResponseTO.AccessToken);
            cmdInsert.Parameters.Add("@ResponseStatus", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceSessionApiResponseTO.ResponseStatus);
            cmdInsert.Parameters.Add("@Response", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(TblEInvoiceSessionApiResponseTO.Response);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = TblEInvoiceSessionApiResponseTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = TblEInvoiceSessionApiResponseTO.CreatedOn;
            cmdInsert.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = TblEInvoiceSessionApiResponseTO.OrgId;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                TblEInvoiceSessionApiResponseTO.IdResponse = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Deletion
        public int DeleteTblEInvoiceSessionApiResponse(Int32 idResponse)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idResponse, cmdDelete);
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

        public int DeleteTblEInvoiceSessionApiResponse(Int32 idResponse, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idResponse, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idResponse, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [TblEInvoiceSessionApiResponse] " +
            " WHERE idResponse = " + idResponse + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
