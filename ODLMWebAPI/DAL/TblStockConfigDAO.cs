using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{
    public class TblStockConfigDAO : ITblStockConfigDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblStockConfigDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " select stockConfig.*,brand.brandName,prodCat.prodCateDesc as prodCatName, " +
                                  " prodSpec.prodSpecDesc as prodSpecName,material.materialSubType as materialName from tblStockConfig stockConfig " +
                                  " INNER JOIN dimBrand brand ON brand.idBrand = stockConfig.brandId " +
                                  " INNER JOIN dimProdCat prodCat ON prodCat.idProdCat = stockConfig.prodCatId " +
                                  " INNER JOIN dimProdSpec prodSpec ON prodSpec.idProdSpec = stockConfig.prodSpecId " +
                                  " INNER JOIN tblMaterial material ON material.idMaterial = stockConfig.materialId "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblStockConfigTO> SelectAllTblStockConfigTOList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + "where stockConfig.isItemizedStock = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockConfigTO> list = ConvertDTToList(sqlReader);
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

        public TblStockConfigTO SelectTblStockConfigTO(Int32 idStockConfig)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idStockConfig = " + idStockConfig +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockConfigTO> list = ConvertDTToList(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();

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

        public List<TblStockConfigTO> SelectAllTblStockConfigTOList(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblStockConfigTO> list = ConvertDTToList(sqlReader);
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
                cmdSelect.Dispose();
            }
        }

        public List<TblStockConfigTO> ConvertDTToList(SqlDataReader tblStockConfigTODT)
        {
            List<TblStockConfigTO> tblStockConfigTOList = new List<TblStockConfigTO>();
            if (tblStockConfigTODT != null)
            {
                while (tblStockConfigTODT.Read())
                {
                    TblStockConfigTO tblStockConfigTONew = new TblStockConfigTO();
                    if (tblStockConfigTODT["idStockConfig"] != DBNull.Value)
                        tblStockConfigTONew.IdStockConfig = Convert.ToInt32(tblStockConfigTODT["idStockConfig"].ToString());
                    if (tblStockConfigTODT["brandId"] != DBNull.Value)
                        tblStockConfigTONew.BrandId = Convert.ToInt32(tblStockConfigTODT["brandId"].ToString());
                    if (tblStockConfigTODT["prodCatId"] != DBNull.Value)
                        tblStockConfigTONew.ProdCatId = Convert.ToInt32(tblStockConfigTODT["prodCatId"].ToString());
                    if (tblStockConfigTODT["prodSpecId"] != DBNull.Value)
                        tblStockConfigTONew.ProdSpecId = Convert.ToInt32(tblStockConfigTODT["prodSpecId"].ToString());
                    if (tblStockConfigTODT["materialId"] != DBNull.Value)
                        tblStockConfigTONew.MaterialId = Convert.ToInt32(tblStockConfigTODT["materialId"].ToString());
                    if (tblStockConfigTODT["isItemizedStock"] != DBNull.Value) 
                        tblStockConfigTONew.IsItemizedStock = Convert.ToInt32(tblStockConfigTODT["isItemizedStock"].ToString());
                    if (tblStockConfigTODT["brandName"] != DBNull.Value)
                        tblStockConfigTONew.BrandName= Convert.ToString(tblStockConfigTODT["brandName"]);
                    if (tblStockConfigTODT["prodCatName"] != DBNull.Value)
                        tblStockConfigTONew.ProdCatName = Convert.ToString(tblStockConfigTODT["prodCatName"]);
                    if (tblStockConfigTODT["prodSpecName"] != DBNull.Value)
                        tblStockConfigTONew.ProdSpecName = Convert.ToString(tblStockConfigTODT["prodSpecName"]);
                    if (tblStockConfigTODT["materialName"] != DBNull.Value)
                        tblStockConfigTONew.MaterialName = Convert.ToString(tblStockConfigTODT["materialName"]);
                    tblStockConfigTOList.Add(tblStockConfigTONew);
                }
            }
            return tblStockConfigTOList;
        }


        #endregion

        #region Insertion
        public int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblStockConfigTO, cmdInsert);
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

        public int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblStockConfigTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblStockConfigTO tblStockConfigTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblStockConfig]( " + 
            //"  [idStockConfig]" +
            " [brandId]" +
            " ,[prodCatId]" +
            " ,[prodSpecId]" +
            " ,[materialId]" +
            " ,[isItemizedStock]" +
            " )" +
" VALUES (" +
            //"  @IdStockConfig " +
            " @BrandId " +
            " ,@ProdCatId " +
            " ,@ProdSpecId " +
            " ,@MaterialId " +
            " ,@IsItemizedStock " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdStockConfig", System.Data.SqlDbType.Int).Value = tblStockConfigTO.IdStockConfig;
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.BrandId;
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.ProdCatId;
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.ProdSpecId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.MaterialId;
            cmdInsert.Parameters.Add("@IsItemizedStock", System.Data.SqlDbType.Int).Value = tblStockConfigTO.IsItemizedStock;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblStockConfigTO, cmdUpdate);
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

        public int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblStockConfigTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblStockConfigTO tblStockConfigTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblStockConfig] SET " + 
            "  [brandId]= @BrandId" +
            " ,[prodCatId]= @ProdCatId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[materialId]= @MaterialId" +
            " ,[isItemizedStock] = @IsItemizedStock" +
            "  WHERE [idStockConfig] = @IdStockConfig" ; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdStockConfig", System.Data.SqlDbType.Int).Value = tblStockConfigTO.IdStockConfig;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.BrandId;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.ProdCatId;
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.ProdSpecId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblStockConfigTO.MaterialId;
            cmdUpdate.Parameters.Add("@IsItemizedStock", System.Data.SqlDbType.Int).Value = tblStockConfigTO.IsItemizedStock;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblStockConfig(Int32 idStockConfig)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idStockConfig, cmdDelete);
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

        public int DeleteTblStockConfig(Int32 idStockConfig, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idStockConfig, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idStockConfig, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblStockConfig] " +
            " WHERE idStockConfig = " + idStockConfig +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idStockConfig", System.Data.SqlDbType.Int).Value = tblStockConfigTO.IdStockConfig;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
