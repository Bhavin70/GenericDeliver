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
    public class DimBrandDAO : IDimBrandDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimBrandDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimBrand]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimBrandTO> SelectAllDimBrand()
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
                List<DimBrandTO> list = ConvertDTToList(sqlReader);
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

        public List<DimBrandTO> SelectAllDimBrand(DimBrandTO dimBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE brandName = '" + dimBrandTO.BrandName + "'";
                if (dimBrandTO.IdBrand > 0)
                {
                    cmdSelect.CommandText += " AND idBrand != " + dimBrandTO.IdBrand; 
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimBrandTO> list = ConvertDTToList(sqlReader);
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

        public DimBrandTO SelectDimBrand(Int32 idBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBrand = " + idBrand + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimBrandTO> list = ConvertDTToList(sqlReader);
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DimBrandTO SelectDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran)
        {
        
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBrand = " + idBrand + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimBrandTO> list = ConvertDTToList(sqlReader);
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
                if (rdr != null) rdr.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<DimBrandTO> SelectAllDimBrand(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimBrandTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public List<DimBrandTO> ConvertDTToList(SqlDataReader dimBrandTODT)
        {
            List<DimBrandTO> dimBrandTOList = new List<DimBrandTO>();
            if (dimBrandTODT != null)
            {
                while (dimBrandTODT.Read())
                {
                    DimBrandTO dimBrandTONew = new DimBrandTO();
                    if (dimBrandTODT["idBrand"] != DBNull.Value)
                        dimBrandTONew.IdBrand = Convert.ToInt32(dimBrandTODT["idBrand"].ToString());
                    if (dimBrandTODT["isActive"] != DBNull.Value)
                        dimBrandTONew.IsActive = Convert.ToInt32(dimBrandTODT["isActive"].ToString());
                    if (dimBrandTODT["createdOn"] != DBNull.Value)
                        dimBrandTONew.CreatedOn = Convert.ToDateTime(dimBrandTODT["createdOn"].ToString());
                    if (dimBrandTODT["brandName"] != DBNull.Value)
                        dimBrandTONew.BrandName = Convert.ToString(dimBrandTODT["brandName"].ToString());
                    //Vijaymala[05-09-2018] added to det default brand for other item
                    if (dimBrandTODT["isDefault"] != DBNull.Value)
                        dimBrandTONew.IsDefault = Convert.ToInt32(dimBrandTODT["isDefault"].ToString());
                    if (dimBrandTODT["shortNm"] != DBNull.Value)
                        dimBrandTONew.ShortNm = Convert.ToString(dimBrandTODT["shortNm"].ToString());
                    if (dimBrandTODT["isTaxInclusive"] != DBNull.Value)
                        dimBrandTONew.IsTaxInclusive = Convert.ToInt32(dimBrandTODT["isTaxInclusive"].ToString());
                    
                    dimBrandTOList.Add(dimBrandTONew);
                }
            }
            return dimBrandTOList;
        }

        #endregion

        #region Insertion
        public int InsertDimBrand(DimBrandTO dimBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimBrandTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimBrandTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(DimBrandTO dimBrandTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimBrand]( " +
            //"  [idBrand]" +
            " [isActive]" +
            " ,[createdOn]" +
            " ,[brandName]" +
            " ,[isDefault]" +
            " ,[isTaxInclusive]" +
            " )" +
" VALUES (" +
            //"  @IdBrand " +
            " @IsActive " +
            " ,@CreatedOn " +
            " ,@BrandName " +
            " ,@IsDefault " +
            " ,@IsTaxInclusive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@IdBrand", System.Data.SqlDbType.Int).Value = dimBrandTO.IdBrand;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimBrandTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimBrandTO.CreatedOn;
            cmdInsert.Parameters.Add("@BrandName", System.Data.SqlDbType.NVarChar).Value = dimBrandTO.BrandName;
            cmdInsert.Parameters.Add("@IsDefault", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(dimBrandTO.IsDefault);
            cmdInsert.Parameters.Add("@IsTaxInclusive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(dimBrandTO.IsTaxInclusive);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimBrandTO.IdBrand = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;

        }
        #endregion

        #region Updation
        public int UpdateDimBrand(DimBrandTO dimBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimBrandTO, cmdUpdate);
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

        public int UpdateDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimBrandTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(DimBrandTO dimBrandTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimBrand] SET " +
            " [isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[brandName] = @BrandName" +
            " ,[isDefault]= @IsDefault" +
            " ,[isTaxInclusive]= @IsTaxInclusive" +
            " WHERE [idBrand] = @IdBrand ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBrand", System.Data.SqlDbType.Int).Value = dimBrandTO.IdBrand;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimBrandTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimBrandTO.CreatedOn;
            cmdUpdate.Parameters.Add("@BrandName", System.Data.SqlDbType.NVarChar).Value = dimBrandTO.BrandName;
            cmdUpdate.Parameters.Add("@IsDefault", System.Data.SqlDbType.Int).Value = dimBrandTO.IsDefault;
            cmdUpdate.Parameters.Add("@IsTaxInclusive", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(dimBrandTO.IsTaxInclusive);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteDimBrand(Int32 idBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBrand, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBrand, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idBrand, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimBrand] " +
            " WHERE idBrand = " + idBrand + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBrand", System.Data.SqlDbType.Int).Value = dimBrandTO.IdBrand;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
