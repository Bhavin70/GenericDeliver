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
    public class TblUserBrandDAO : ITblUserBrandDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblUserBrandDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblUserBrand] tblUserBrand"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblUserBrandTO> SelectAllTblUserBrand()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserBrandTO> list = ConvertDTToList(sqlReader);
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

        public List<TblUserBrandTO> SelectAllTblUserBrand(int isActive)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE ISNULL(tblUserBrand.isActive, 0) =  " + isActive;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserBrandTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblUserBrandTO> SelectAllTblUserBrandByCnfId(Int32 cnfId,int categoryType = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblUserBrand.isActive = 1 AND tblUserBrand.cnfOrgId = " + cnfId;
                if(categoryType > 0) 
                {
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE categoryType = " + categoryType;
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserBrandTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblUserBrandTO SelectTblUserBrand(Int32 idUserBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idUserBrand = " + idUserBrand +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserBrandTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblUserBrandTO> SelectAllTblUserBrand(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblUserBrandTO> list = ConvertDTToList(sqlReader);
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
        public List<TblUserBrandTO> ConvertDTToList(SqlDataReader tblUserBrandTODT)
        {
            List<TblUserBrandTO> tblUserBrandTOList = new List<TblUserBrandTO>();
            if (tblUserBrandTODT != null)
            {
                while(tblUserBrandTODT.Read())
                {
                    TblUserBrandTO tblUserBrandTONew = new TblUserBrandTO();
                    if (tblUserBrandTODT["idUserBrand"] != DBNull.Value)
                        tblUserBrandTONew.IdUserBrand = Convert.ToInt32(tblUserBrandTODT["idUserBrand"].ToString());
                    if (tblUserBrandTODT["userId"] != DBNull.Value)
                        tblUserBrandTONew.UserId = Convert.ToInt32(tblUserBrandTODT["userId"].ToString());
                    if (tblUserBrandTODT["brandId"] != DBNull.Value)
                        tblUserBrandTONew.BrandId = Convert.ToInt32(tblUserBrandTODT["brandId"].ToString());
                    if (tblUserBrandTODT["isActive"] != DBNull.Value)
                        tblUserBrandTONew.IsActive = Convert.ToInt32(tblUserBrandTODT["isActive"].ToString());
                    if (tblUserBrandTODT["createdBy"] != DBNull.Value)
                        tblUserBrandTONew.CreatedBy = Convert.ToInt32(tblUserBrandTODT["createdBy"].ToString());
                    if (tblUserBrandTODT["updatedBy"] != DBNull.Value)
                        tblUserBrandTONew.UpdatedBy = Convert.ToInt32(tblUserBrandTODT["updatedBy"].ToString());
                    if (tblUserBrandTODT["createdOn"] != DBNull.Value)
                        tblUserBrandTONew.CreatedOn = Convert.ToDateTime(tblUserBrandTODT["createdOn"].ToString());
                    if (tblUserBrandTODT["updatedOn"] != DBNull.Value)
                        tblUserBrandTONew.UpdatedOn = Convert.ToDateTime(tblUserBrandTODT["updatedOn"].ToString());
                    if (tblUserBrandTODT["cnfOrgId"] != DBNull.Value)
                        tblUserBrandTONew.CnfOrgId = Convert.ToInt32(tblUserBrandTODT["cnfOrgId"].ToString());
                    tblUserBrandTOList.Add(tblUserBrandTONew);
                }
            }
            return tblUserBrandTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblUserBrandTO, cmdInsert);
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

        public int InsertTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblUserBrandTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblUserBrandTO tblUserBrandTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblUserBrand]( " + 
            "  [userId]" +
            " ,[brandId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[cnfOrgId]" +
            " )" +
" VALUES (" +
            " @UserId " +
            " ,@BrandId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " ,@CnfOrgId" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           // cmdInsert.Parameters.Add("@IdUserBrand", System.Data.SqlDbType.Int).Value = tblUserBrandTO.IdUserBrand;
            cmdInsert.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.UserId);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblUserBrandTO.BrandId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserBrandTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblUserBrandTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblUserBrandTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.UpdatedOn);
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.CnfOrgId);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblUserBrandTO, cmdUpdate);
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

        public int UpdateTblUserBrand(TblUserBrandTO tblUserBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblUserBrandTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblUserBrandTO tblUserBrandTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblUserBrand] SET " + 
            //"  [idUserBrand] = @IdUserBrand" +
            " [userId]= @UserId" +
            " ,[brandId]= @BrandId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " ,[cnfOrgId] = @CnfOrgId" +
            " WHERE [idUserBrand] =  @IdUserBrand "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdUserBrand", System.Data.SqlDbType.Int).Value = tblUserBrandTO.IdUserBrand;
            cmdUpdate.Parameters.Add("@UserId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.UserId);
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblUserBrandTO.BrandId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblUserBrandTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.CreatedBy);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblUserBrandTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.CreatedOn);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblUserBrandTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblUserBrandTO.CnfOrgId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblUserBrand(Int32 idUserBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idUserBrand, cmdDelete);
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

        public int DeleteTblUserBrand(Int32 idUserBrand, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idUserBrand, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idUserBrand, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblUserBrand] " +
            " WHERE idUserBrand = " + idUserBrand +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idUserBrand", System.Data.SqlDbType.Int).Value = tblUserBrandTO.IdUserBrand;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
