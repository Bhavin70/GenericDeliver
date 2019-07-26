using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblOrgOverdueHistoryDAO : ITblOrgOverdueHistoryDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblOrgOverdueHistoryDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblOrgOverdueHistory]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblOrgOverdueHistoryTO>SelectAllTblOrgOverdueHistory()
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
                List<TblOrgOverdueHistoryTO> list = ConvertDTToList(reader);
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

        public List<TblOrgOverdueHistoryTO> SelectAllTblOrgOverdueHistory(SqlConnection conn, SqlTransaction tran)
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
                List<TblOrgOverdueHistoryTO> list = ConvertDTToList(reader);
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

        public List<TblOrgOverdueHistoryTO> ConvertDTToList(SqlDataReader tblOrgOverdueHistoryTODT)
        {
            List<TblOrgOverdueHistoryTO> tblOrgOverdueHistoryTOList = new List<TblOrgOverdueHistoryTO>();
            if (tblOrgOverdueHistoryTODT != null)
            {
                while (tblOrgOverdueHistoryTODT.Read())
                {
                    TblOrgOverdueHistoryTO tblOrgOverdueHistoryTONew = new TblOrgOverdueHistoryTO();
                    if (tblOrgOverdueHistoryTODT["idOrgOverdueHistory"] != DBNull.Value)
                        tblOrgOverdueHistoryTONew.IdOrgOverdueHistory = Convert.ToInt32(tblOrgOverdueHistoryTODT["idOrgOverdueHistory"].ToString());
                    if (tblOrgOverdueHistoryTODT["organizationId"] != DBNull.Value)
                        tblOrgOverdueHistoryTONew.OrganizationId = Convert.ToInt32(tblOrgOverdueHistoryTODT["organizationId"].ToString());
                    if (tblOrgOverdueHistoryTODT["isOverdueExist"] != DBNull.Value)
                        tblOrgOverdueHistoryTONew.IsOverdueExist = Convert.ToInt32(tblOrgOverdueHistoryTODT["isOverdueExist"].ToString());
                    if (tblOrgOverdueHistoryTODT["CreatedBy"] != DBNull.Value)
                        tblOrgOverdueHistoryTONew.CreatedBy = Convert.ToInt32(tblOrgOverdueHistoryTODT["CreatedBy"].ToString());
                    if (tblOrgOverdueHistoryTODT["CreatedOn"] != DBNull.Value)
                        tblOrgOverdueHistoryTONew.CreatedOn = Convert.ToDateTime(tblOrgOverdueHistoryTODT["CreatedOn"].ToString());
                    if (tblOrgOverdueHistoryTODT["bookingId"] != DBNull.Value)
                        tblOrgOverdueHistoryTONew.BookingId = Convert.ToInt32(tblOrgOverdueHistoryTODT["bookingId"].ToString());
                    tblOrgOverdueHistoryTOList.Add(tblOrgOverdueHistoryTONew);
                }
            }
            return tblOrgOverdueHistoryTOList;
        }
        #endregion

        #region Insertion
        public int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblOrgOverdueHistoryTO, cmdInsert);
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

        public int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblOrgOverdueHistoryTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblOrgOverdueHistory]( " + 
           
            " [organizationId]" +
            " ,[isOverdueExist]" +
            " ,[CreatedBy]" +
            " ,[CreatedOn]" +
            " ,[bookingId]" +
            " )" +
" VALUES (" +
           
            " @OrganizationId " +
            " ,@IsOverdueExist " +
            " ,@CreatedBy " +
            " ,@CreatedOn " + 
            " ,@BookingId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

          //  cmdInsert.Parameters.Add("@IdOrgOverdueHistory", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.IdOrgOverdueHistory;
            cmdInsert.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.OrganizationId;
            cmdInsert.Parameters.Add("@IsOverdueExist", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.IsOverdueExist;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgOverdueHistoryTO.CreatedOn;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgOverdueHistoryTO.BookingId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblOrgOverdueHistoryTO.IdOrgOverdueHistory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO)
        {
            String sqlConnStr =_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblOrgOverdueHistoryTO, cmdUpdate);
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

        public int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblOrgOverdueHistoryTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblOrgOverdueHistory] SET " + 
            "  [idOrgOverdueHistory] = @IdOrgOverdueHistory" +
            " ,[organizationId]= @OrganizationId" +
            " ,[isOverdueExist]= @IsOverdueExist" +
            " ,[CreatedBy]= @CreatedBy" +
            " ,[CreatedOn] = @CreatedOn" +
            " ,[BookingId] = @BookingId" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdOrgOverdueHistory", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.IdOrgOverdueHistory;
            cmdUpdate.Parameters.Add("@OrganizationId", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.OrganizationId;
            cmdUpdate.Parameters.Add("@IsOverdueExist", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.IsOverdueExist;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblOrgOverdueHistoryTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblOrgOverdueHistoryTO.CreatedOn;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblOrgOverdueHistoryTO.BookingId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idOrgOverdueHistory, cmdDelete);
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

        public int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idOrgOverdueHistory, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idOrgOverdueHistory, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblOrgOverdueHistory] " +
            " ";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
