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
    public class DimVehDocTypeDAO : IDimVehDocTypeDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimVehDocTypeDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimVehDocType]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimVehDocTypeTO> SelectAllDimVehDocType()
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
                List<DimVehDocTypeTO> list = ConvertDTToList(sqlReader);
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

        public DimVehDocTypeTO SelectDimVehDocType(Int32 idVehDocType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idVehDocType = " + idVehDocType +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehDocTypeTO> list = ConvertDTToList(sqlReader);
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

        public List<DimVehDocTypeTO> SelectAllDimVehDocType(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimVehDocTypeTO> list = ConvertDTToList(sqlReader);
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



        public List<DimVehDocTypeTO> ConvertDTToList(SqlDataReader dimVehDocTypeTODT)
        {
            List<DimVehDocTypeTO> dimVehDocTypeTOList = new List<DimVehDocTypeTO>();
            if (dimVehDocTypeTODT != null)
            {
                while (dimVehDocTypeTODT.Read())
                {
                    DimVehDocTypeTO dimVehDocTypeTONew = new DimVehDocTypeTO();
                    if (dimVehDocTypeTODT["idVehDocType"] != DBNull.Value)
                        dimVehDocTypeTONew.IdVehDocType = Convert.ToInt32(dimVehDocTypeTODT["idVehDocType"].ToString());
                    if (dimVehDocTypeTODT["isActive"] != DBNull.Value)
                        dimVehDocTypeTONew.IsActive = Convert.ToInt32(dimVehDocTypeTODT["isActive"].ToString());
                    if (dimVehDocTypeTODT["createdBy"] != DBNull.Value)
                        dimVehDocTypeTONew.CreatedBy = Convert.ToInt32(dimVehDocTypeTODT["createdBy"].ToString());
                    if (dimVehDocTypeTODT["updatedBy"] != DBNull.Value)
                        dimVehDocTypeTONew.UpdatedBy = Convert.ToInt32(dimVehDocTypeTODT["updatedBy"].ToString());
                    if (dimVehDocTypeTODT["createdOn"] != DBNull.Value)
                        dimVehDocTypeTONew.CreatedOn = Convert.ToDateTime(dimVehDocTypeTODT["createdOn"].ToString());
                    if (dimVehDocTypeTODT["updatedOn"] != DBNull.Value)
                        dimVehDocTypeTONew.UpdatedOn = Convert.ToDateTime(dimVehDocTypeTODT["updatedOn"].ToString());
                    if (dimVehDocTypeTODT["vehDocTypeName"] != DBNull.Value)
                        dimVehDocTypeTONew.VehDocTypeName = Convert.ToString(dimVehDocTypeTODT["vehDocTypeName"].ToString());
                    if (dimVehDocTypeTODT["vehDocTypeDesc"] != DBNull.Value)
                        dimVehDocTypeTONew.VehDocTypeDesc = Convert.ToString(dimVehDocTypeTODT["vehDocTypeDesc"].ToString());

                    if (dimVehDocTypeTODT["sequenceNo"] != DBNull.Value)
                        dimVehDocTypeTONew.SequenceNo = Convert.ToInt32(dimVehDocTypeTODT["sequenceNo"].ToString());

                    dimVehDocTypeTOList.Add(dimVehDocTypeTONew);
                }
            }
            return dimVehDocTypeTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimVehDocTypeTO, cmdInsert);
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

        public int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimVehDocTypeTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimVehDocTypeTO dimVehDocTypeTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimVehDocType]( " + 
            " [isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[vehDocTypeName]" +
            " ,[vehDocTypeDesc]" +
            " )" +
" VALUES (" +
            " @IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@VehDocTypeName " +
            " ,@VehDocTypeDesc " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdVehDocType", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.IdVehDocType;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimVehDocTypeTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimVehDocTypeTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimVehDocTypeTO.UpdatedOn);
            cmdInsert.Parameters.Add("@VehDocTypeName", System.Data.SqlDbType.NVarChar).Value = dimVehDocTypeTO.VehDocTypeName;
            cmdInsert.Parameters.Add("@VehDocTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimVehDocTypeTO.VehDocTypeDesc;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimVehDocTypeTO.IdVehDocType = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimVehDocTypeTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimVehDocTypeTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimVehDocTypeTO dimVehDocTypeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimVehDocType] SET " + 
            "  [idVehDocType] = @IdVehDocType" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[vehDocTypeName]= @VehDocTypeName" +
            " ,[vehDocTypeDesc] = @VehDocTypeDesc" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdVehDocType", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.IdVehDocType;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimVehDocTypeTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = dimVehDocTypeTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@VehDocTypeName", System.Data.SqlDbType.NVarChar).Value = dimVehDocTypeTO.VehDocTypeName;
            cmdUpdate.Parameters.Add("@VehDocTypeDesc", System.Data.SqlDbType.NVarChar).Value = dimVehDocTypeTO.VehDocTypeDesc;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimVehDocType(Int32 idVehDocType)
        {
             String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idVehDocType, cmdDelete);
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

        public int DeleteDimVehDocType(Int32 idVehDocType, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idVehDocType, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idVehDocType, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimVehDocType] " +
            " WHERE idVehDocType = " + idVehDocType +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idVehDocType", System.Data.SqlDbType.Int).Value = dimVehDocTypeTO.IdVehDocType;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
