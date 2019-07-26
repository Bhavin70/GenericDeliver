using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblItemTallyRefDtlsDAO : ITblItemTallyRefDtlsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblItemTallyRefDtlsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblItemTallyRefDtls]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtls()
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

                //cmdSelect.Parameters.Add("@idItemTallyRef", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IdItemTallyRef;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemTallyRefDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblItemTallyRefDtlsTO SelectTblItemTallyRefDtls(Int32 idItemTallyRef)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idItemTallyRef = " + idItemTallyRef +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idItemTallyRef", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IdItemTallyRef;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemTallyRefDtlsTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblItemTallyRefDtlsTO> SelectAllTblItemTallyRefDtls(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idItemTallyRef", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IdItemTallyRef;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemTallyRefDtlsTO> list = ConvertDTToList(reader);
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }



        public List<TblItemTallyRefDtlsTO> ConvertDTToList(SqlDataReader tblItemTallyRefDtlsTODT)
        {
            List<TblItemTallyRefDtlsTO> tblItemTallyRefDtlsTOList = new List<TblItemTallyRefDtlsTO>();
            if (tblItemTallyRefDtlsTODT != null)
            {
                while (tblItemTallyRefDtlsTODT.Read())
                {
                    TblItemTallyRefDtlsTO tblItemTallyRefDtlsTONew = new TblItemTallyRefDtlsTO();
                    if (tblItemTallyRefDtlsTODT["idItemTallyRef"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.IdItemTallyRef = Convert.ToInt32(tblItemTallyRefDtlsTODT["idItemTallyRef"].ToString());
                    if (tblItemTallyRefDtlsTODT["prodCatId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdCatId = Convert.ToInt32(tblItemTallyRefDtlsTODT["prodCatId"].ToString());
                    if (tblItemTallyRefDtlsTODT["prodSpecId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdSpecId = Convert.ToInt32(tblItemTallyRefDtlsTODT["prodSpecId"].ToString());
                    if (tblItemTallyRefDtlsTODT["materialId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.MaterialId = Convert.ToInt32(tblItemTallyRefDtlsTODT["materialId"].ToString());
                    if (tblItemTallyRefDtlsTODT["prodItemId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdItemId = Convert.ToInt32(tblItemTallyRefDtlsTODT["prodItemId"].ToString());
                    if (tblItemTallyRefDtlsTODT["brandId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.BrandId = Convert.ToInt32(tblItemTallyRefDtlsTODT["brandId"].ToString());
                    if (tblItemTallyRefDtlsTODT["overdueTallyRefId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.OverdueTallyRefId = Convert.ToString(tblItemTallyRefDtlsTODT["overdueTallyRefId"].ToString());
                    if (tblItemTallyRefDtlsTODT["enquiryTallyRefId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.EnquiryTallyRefId = Convert.ToString(tblItemTallyRefDtlsTODT["enquiryTallyRefId"].ToString());
                    if (tblItemTallyRefDtlsTODT ["isActive"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.IsActive = Convert.ToInt32(tblItemTallyRefDtlsTODT ["isActive"].ToString());
                    if (tblItemTallyRefDtlsTODT ["createdBy"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.CreatedBy = Convert.ToInt32(tblItemTallyRefDtlsTODT ["createdBy"].ToString());
                    if (tblItemTallyRefDtlsTODT ["updatedBy"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.UpdatedBy = Convert.ToInt32(tblItemTallyRefDtlsTODT ["updatedBy"].ToString());
                    if (tblItemTallyRefDtlsTODT ["createdOn"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.CreatedOn = Convert.ToDateTime(tblItemTallyRefDtlsTODT ["createdOn"].ToString());
                    if (tblItemTallyRefDtlsTODT ["updatedOn"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.UpdatedOn = Convert.ToDateTime(tblItemTallyRefDtlsTODT ["updatedOn"].ToString());
                    tblItemTallyRefDtlsTOList.Add(tblItemTallyRefDtlsTONew);
                }
            }
            return tblItemTallyRefDtlsTOList;
        }

        public List<TblItemTallyRefDtlsTO> SelectEmptyTblItemTallyRefDtlsTOTemplate(Int32 brandId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                   
                    cmdSelect.CommandText = " SELECT idProdCat AS prodCatId, prodCateDesc, prodSpec.idProdSpec AS prodSpecId,prodSpecDesc, idMaterial AS materialId,materialSubType  " +
                                            " ,brand.idBrand AS brandId, brand.brandName" +
                                            " FROM dimProdSpec prodSpec " +
                                            " FULL OUTER JOIN tblMaterial material ON 1 = 1 AND material.isActive = 1 " +
                                            " FULL OUTER JOIN dimProdCat prodCat ON 1 = 1 " +
                                            " FULL OUTER JOIN dimBrand brand ON 1 = 1 " +
                                            //" LEFT JOIN tblStockConfig tblStockConfig  " +
                                            //" ON prodSpec.idProdSpec = tblStockConfig.prodSpecId AND material.idMaterial = tblStockConfig.materialId " +
                                            //" AND tblStockConfig.brandId = brand.idBrand  AND tblStockConfig.prodCatId = prodCat.idProdCat " +
                                            " WHERE idProdSpec <> 0 AND idProdCat <> 0 AND idBrand <> 0 " +
                                            " AND idBrand =" + brandId + 
                                            //" AND tblStockConfig.isItemizedStock = 1 " +
                                            " AND prodSpec.isActive = 1  " +
                                            " ORDER BY prodSpec.displaySequence";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemTallyRefDtlsTO> list = ConvertDTToListEmty(sqlReader);
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

        public List<TblItemTallyRefDtlsTO> ConvertDTToListEmty(SqlDataReader tblItemTallyRefDtlsTODT)
        {
            List<TblItemTallyRefDtlsTO> tblItemTallyRefDtlsTOList = new List<TblItemTallyRefDtlsTO>();
            if (tblItemTallyRefDtlsTODT != null)
            {
                while (tblItemTallyRefDtlsTODT.Read())
                {
                    TblItemTallyRefDtlsTO tblItemTallyRefDtlsTONew = new TblItemTallyRefDtlsTO();
                    if (tblItemTallyRefDtlsTODT["prodCatId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdCatId = Convert.ToInt32(tblItemTallyRefDtlsTODT["prodCatId"].ToString());
                    if (tblItemTallyRefDtlsTODT["prodSpecId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdSpecId = Convert.ToInt32(tblItemTallyRefDtlsTODT["prodSpecId"].ToString());
                    if (tblItemTallyRefDtlsTODT["materialId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.MaterialId = Convert.ToInt32(tblItemTallyRefDtlsTODT["materialId"].ToString());
                    if (tblItemTallyRefDtlsTODT["brandId"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.BrandId = Convert.ToInt32(tblItemTallyRefDtlsTODT["brandId"].ToString());

                    if (tblItemTallyRefDtlsTODT["prodSpecDesc"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdSpecDesc = Convert.ToString(tblItemTallyRefDtlsTODT["prodSpecDesc"].ToString());
                    if (tblItemTallyRefDtlsTODT["prodCateDesc"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.ProdCatDesc = Convert.ToString(tblItemTallyRefDtlsTODT["prodCateDesc"].ToString());
                    if (tblItemTallyRefDtlsTODT["materialSubType"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.MaterialDesc = Convert.ToString(tblItemTallyRefDtlsTODT["materialSubType"].ToString());
                    if (tblItemTallyRefDtlsTODT["brandName"] != DBNull.Value)
                        tblItemTallyRefDtlsTONew.BrandName = Convert.ToString(tblItemTallyRefDtlsTODT["brandName"].ToString());

                    tblItemTallyRefDtlsTONew.DisplayName = tblItemTallyRefDtlsTONew.MaterialDesc + " - " + tblItemTallyRefDtlsTONew.ProdCatDesc + " " + tblItemTallyRefDtlsTONew.ProdSpecDesc + " (" + tblItemTallyRefDtlsTONew.BrandName + ")";

                    tblItemTallyRefDtlsTOList.Add(tblItemTallyRefDtlsTONew);
                }
            }
            return tblItemTallyRefDtlsTOList;
        }



        public List<TblItemTallyRefDtlsTO> SelectExistingAllItemTallyByRefIds( String overdueTallyRefId, String enquiryTallyRefId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery();


                if (!String.IsNullOrEmpty(overdueTallyRefId))
                {
                    cmdSelect.CommandText += "WHERE tblItemTallyRefDtls.overdueTallyRefId = '" + overdueTallyRefId + "'";
                }

                if (!String.IsNullOrEmpty(enquiryTallyRefId))
                {

                    cmdSelect.CommandText += "WHERE tblItemTallyRefDtls.enq_ref_id = '" + enquiryTallyRefId + "'";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemTallyRefDtlsTO> list = ConvertDTToList(reader);
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

        public TblItemTallyRefDtlsTO SelectExistingAllTblItemRef(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + "WHERE ISNULL(tblItemTallyRefDtls.materialId,0) = " + tblItemTallyRefDtlsTO.MaterialId + " AND ISNULL(tblItemTallyRefDtls.prodSpecId,0) =" + tblItemTallyRefDtlsTO.ProdSpecId
                    + " AND ISNULL(tblItemTallyRefDtls.prodCatId,0) =" + tblItemTallyRefDtlsTO.ProdCatId + " AND ISNULL(tblItemTallyRefDtls.brandId,0) =" + tblItemTallyRefDtlsTO.BrandId
                    + "  AND tblItemTallyRefDtls.isActive =1 "; 

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblItemTallyRefDtlsTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
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

        #endregion

        #region Insertion
        public int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblItemTallyRefDtlsTO, cmdInsert);
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

        public int InsertTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblItemTallyRefDtlsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblItemTallyRefDtls]( " + 
     
            " [prodCatId]" +
            " ,[prodSpecId]" +
            " ,[materialId]" +
            " ,[prodItemId]" +
            " ,[brandId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[overdueTallyRefId]" +
            " ,[enquiryTallyRefId]" +
            " ,[isActive]" +
            " )" +
" VALUES (" +
           
            " @ProdCatId " +
            " ,@ProdSpecId " +
            " ,@MaterialId " +
            " ,@ProdItemId " +
            " ,@BrandId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@OverdueTallyRefId " +
            " ,@EnquiryTallyRefId " +
            " ,@isActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

           
            cmdInsert.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.ProdCatId;
            cmdInsert.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.ProdSpecId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.MaterialId;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.ProdItemId);
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.BrandId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblItemTallyRefDtlsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@OverdueTallyRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.OverdueTallyRefId);
            cmdInsert.Parameters.Add("@EnquiryTallyRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.EnquiryTallyRefId);
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IsActive;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblItemTallyRefDtlsTO.IdItemTallyRef = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblItemTallyRefDtlsTO, cmdUpdate);
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

        public int UpdateTblItemTallyRefDtls(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblItemTallyRefDtlsTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblItemTallyRefDtlsTO tblItemTallyRefDtlsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblItemTallyRefDtls] SET " + 
     
            "  [prodCatId]= @ProdCatId" +
            " ,[prodSpecId]= @ProdSpecId" +
            " ,[materialId]= @MaterialId" +
            " ,[prodItemId]= @ProdItemId" +
            " ,[brandId]= @BrandId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[overdueTallyRefId]= @OverdueTallyRefId" +
            " ,[enquiryTallyRefId] = @EnquiryTallyRefId" +
            " ,[isActive] = @isActive" +
             " WHERE [idItemTallyRef] = @IdItemTallyRef";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdItemTallyRef", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IdItemTallyRef;
            cmdUpdate.Parameters.Add("@ProdCatId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.ProdCatId;
            cmdUpdate.Parameters.Add("@ProdSpecId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.ProdSpecId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.MaterialId;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.ProdItemId);
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.BrandId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblItemTallyRefDtlsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@OverdueTallyRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.OverdueTallyRefId);
            cmdUpdate.Parameters.Add("@EnquiryTallyRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblItemTallyRefDtlsTO.EnquiryTallyRefId);
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IsActive;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idItemTallyRef, cmdDelete);
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

        public int DeleteTblItemTallyRefDtls(Int32 idItemTallyRef, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idItemTallyRef, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idItemTallyRef, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblItemTallyRefDtls] " +
            " WHERE idItemTallyRef = " + idItemTallyRef +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idItemTallyRef", System.Data.SqlDbType.Int).Value = tblItemTallyRefDtlsTO.IdItemTallyRef;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
