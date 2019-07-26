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
    public class TblLoadingVehDocExtDAO : ITblLoadingVehDocExtDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblLoadingVehDocExtDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tempLoadingVehDocExt.*,dimVehDocType.vehDocTypeName,dimVehDocType.sequenceNo FROM [tempLoadingVehDocExt] tempLoadingVehDocExt" +
                                  " LEFT JOIN [dimVehDocType] dimVehDocType" +
                                  " ON tempLoadingVehDocExt.vehDocTypeId = dimVehDocType.idVehDocType"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExt()
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
                List<TblLoadingVehDocExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExtList(Int32 loadingId, Int32 ActiveYnAll)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tempLoadingVehDocExt.loadingId = " + loadingId;
                if (ActiveYnAll == 1)
                {
                    cmdSelect.CommandText += " AND tempLoadingVehDocExt.isActive = 1";
                }
                else if (ActiveYnAll == 2)
                {
                    cmdSelect.CommandText += " AND ISNULL(tempLoadingVehDocExt.isActive, 0) = 1";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingVehDocExtTO> list = ConvertDTToList(sqlReader);
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


        public TblLoadingVehDocExtTO SelectTblLoadingVehDocExt(Int32 idLoadingVehDocExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idLoadingVehDocExt = " + idLoadingVehDocExt +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingVehDocExtTO> list = ConvertDTToList(sqlReader);
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public List<TblLoadingVehDocExtTO> SelectAllTblLoadingVehDocExt(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingVehDocExtTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingVehDocExtTO> ConvertDTToList(SqlDataReader tblLoadingVehDocExtTODT)
        {
            List<TblLoadingVehDocExtTO> tblLoadingVehDocExtTOList = new List<TblLoadingVehDocExtTO>();
            if (tblLoadingVehDocExtTODT != null)
            {
                while (tblLoadingVehDocExtTODT.Read())
                {
                    TblLoadingVehDocExtTO tblLoadingVehDocExtTONew = new TblLoadingVehDocExtTO();
                    if (tblLoadingVehDocExtTODT["idLoadingVehDocExt"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.IdLoadingVehDocExt = Convert.ToInt32(tblLoadingVehDocExtTODT["idLoadingVehDocExt"].ToString());
                    if (tblLoadingVehDocExtTODT["vehDocTypeId"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.VehDocTypeId = Convert.ToInt32(tblLoadingVehDocExtTODT["vehDocTypeId"].ToString());
                    if (tblLoadingVehDocExtTODT["isActive"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.IsActive = Convert.ToInt32(tblLoadingVehDocExtTODT["isActive"].ToString());
                    if (tblLoadingVehDocExtTODT["createdBy"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.CreatedBy = Convert.ToInt32(tblLoadingVehDocExtTODT["createdBy"].ToString());
                    if (tblLoadingVehDocExtTODT["updatedBy"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.UpdatedBy = Convert.ToInt32(tblLoadingVehDocExtTODT["updatedBy"].ToString());
                    if (tblLoadingVehDocExtTODT["createdOn"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.CreatedOn = Convert.ToDateTime(tblLoadingVehDocExtTODT["createdOn"].ToString());
                    if (tblLoadingVehDocExtTODT["updatedOn"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.UpdatedOn = Convert.ToDateTime(tblLoadingVehDocExtTODT["updatedOn"].ToString());
                    if (tblLoadingVehDocExtTODT["vehDocNo"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.VehDocNo = Convert.ToString(tblLoadingVehDocExtTODT["vehDocNo"].ToString());
                    if (tblLoadingVehDocExtTODT["loadingId"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.LoadingId = Convert.ToInt32(tblLoadingVehDocExtTODT["loadingId"].ToString());
                    if (tblLoadingVehDocExtTODT["vehDocTypeName"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.VehDocTypeName = Convert.ToString(tblLoadingVehDocExtTODT["vehDocTypeName"].ToString());

                    if (tblLoadingVehDocExtTODT["sequenceNo"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.SequenceNo = Convert.ToInt32(tblLoadingVehDocExtTODT["sequenceNo"].ToString());
                    if (tblLoadingVehDocExtTODT["isAvailable"] != DBNull.Value)
                        tblLoadingVehDocExtTONew.IsAvailable = Convert.ToInt32(tblLoadingVehDocExtTODT["isAvailable"].ToString());

                    tblLoadingVehDocExtTOList.Add(tblLoadingVehDocExtTONew);
                }
            }
            return tblLoadingVehDocExtTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingVehDocExtTO, cmdInsert);
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

        public int InsertTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingVehDocExtTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingVehDocExt]( " + 
            //"  [idLoadingVehDocExt]" +
            " [vehDocTypeId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[vehDocNo]" +
            " ,[loadingId]" +
            " ,[isAvailable] " +
            " )" +
" VALUES (" +
            //"  @IdLoadingVehDocExt " +
            " @VehDocTypeId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VehDocNo " +
            " ,@LoadingId " +
            " ,@IsAvailable " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdLoadingVehDocExt", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IdLoadingVehDocExt;
            cmdInsert.Parameters.Add("@VehDocTypeId", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.VehDocTypeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblLoadingVehDocExtTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingVehDocExtTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblLoadingVehDocExtTO.UpdatedOn);
            cmdInsert.Parameters.Add("@VehDocNo", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblLoadingVehDocExtTO.VehDocNo);
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.LoadingId;
            cmdInsert.Parameters.Add("@IsAvailable", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IsAvailable;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingVehDocExtTO.IdLoadingVehDocExt = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblLoadingVehDocExtActiveYn(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction= tran;

                String sqlQuery = @" UPDATE [tempLoadingVehDocExt] SET " +
                " [isActive]= @IsActive" +
                " ,[updatedBy]= @UpdatedBy" +
                " ,[updatedOn]= @UpdatedOn" +
                " WHERE [loadingId] = @LoadingId AND isActive = 1";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IsActive;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingVehDocExtTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.LoadingId;

                return cmdUpdate.ExecuteNonQuery();
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

        public int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingVehDocExtTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblLoadingVehDocExt(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingVehDocExtTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblLoadingVehDocExtTO tblLoadingVehDocExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingVehDocExt] SET " + 
            "  [idLoadingVehDocExt] = @IdLoadingVehDocExt" +
            " ,[vehDocTypeId]= @VehDocTypeId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[vehDocNo] = @VehDocNo" +
            " ,[loadingId] = @LoadingId" +
            " ,[isAvailable] = @IsAvailable" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingVehDocExt", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IdLoadingVehDocExt;
            cmdUpdate.Parameters.Add("@VehDocTypeId", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.VehDocTypeId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingVehDocExtTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingVehDocExtTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@VehDocNo", System.Data.SqlDbType.NVarChar).Value = tblLoadingVehDocExtTO.VehDocNo;
            cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.LoadingId;
            cmdUpdate.Parameters.Add("@IsAvailable", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IsAvailable;

            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingVehDocExt, cmdDelete);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblLoadingVehDocExt(Int32 idLoadingVehDocExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingVehDocExt, cmdDelete);
            }
            catch(Exception ex)
            {
                
                
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idLoadingVehDocExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingVehDocExt] " +
            " WHERE idLoadingVehDocExt = " + idLoadingVehDocExt +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingVehDocExt", System.Data.SqlDbType.Int).Value = tblLoadingVehDocExtTO.IdLoadingVehDocExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
