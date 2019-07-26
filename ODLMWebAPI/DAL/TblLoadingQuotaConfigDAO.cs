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
    public class TblLoadingQuotaConfigDAO : ITblLoadingQuotaConfigDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblLoadingQuotaConfigDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT config.* ,org.firmName as cnfOrgName, material.materialSubType as materialDesc " +
                                  " FROM tblLoadingQuotaConfig config " +
                                  " LEFT JOIN tblOrganization org " +
                                  " ON config.cnfOrgId = org.idOrganization " +
                                  " LEFT JOIN tblMaterial material " +
                                  " ON material.idMaterial = config.materialId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblLoadingQuotaConfigTO> SelectAllTblLoadingQuotaConfig()
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
                List<TblLoadingQuotaConfigTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingQuotaConfigTO> SelectLatestLoadingQuotaConfig(Int32 prodCatId, Int32 prodSpecId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT idOrganization AS cnfOrgId , firmName as cnfOrgName ,idMaterial as materialId,  " +
                                        " materialSubType as materialDesc ,lastConfig.idLoadQuotaConfig,lastConfig.isActive, " +
                                        " lastConfig.allocPct,lastConfig.remark,lastConfig.createdBy,lastConfig.createdOn " +
                                        " , prodCat.prodCateDesc as prodCatDesc , prodSpec.prodSpecDesc " +
                                        " , prodCat.idProdCat as prodCatId , prodSpec.idProdSpec AS prodSpecId " +
                                        " FROM tblOrganization " +
                                        " FULL OUTER JOIN tblMaterial " +
                                        " ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdCat prodCat" +
                                        " ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec prodSpec" +
                                        " ON 1 = 1 " +
                                        " LEFT JOIN " +
                                        " ( " +
                                        "    SELECT config.*, material.materialSubType as materialDesc FROM tblLoadingQuotaConfig config " +
                                        "    INNER JOIN " +
                                        "   ( " +
                                        "       SELECT cnfOrgId,prodCatId,prodSpecId, materialId, MAX(createdOn) createdOn " +
                                        "        FROM tblLoadingQuotaConfig loadingQuota " +
                                        "        WHERE prodCatId=" + prodCatId + "AND prodSpecId=" + prodSpecId +
                                        "        GROUP BY cnfOrgId,prodCatId,prodSpecId, materialId " +
                                        "   ) latestConfig " +
                                        "    ON config.cnfOrgId = latestConfig.cnfOrgId AND config.materialId = latestConfig.materialId " +
                                        "    AND config.createdOn = latestConfig.createdOn " +
                                        "    AND config.prodCatId = latestConfig.prodCatId " +
                                        "    AND config.prodSpecId = latestConfig.prodSpecId " +
                                        "    LEFT JOIN tblMaterial material " +
                                        "    ON material.idMaterial = config.materialId " +
                                        "     WHERE config.prodCatId=" + prodCatId + "AND config. prodSpecId=" + prodSpecId +
                                        " ) lastConfig " +
                                        " ON lastConfig.cnfOrgId = tblOrganization.idOrganization " +
                                        " AND lastConfig.materialId = tblMaterial.idMaterial " +
                                        " WHERE tblOrganization.isActive=1 AND orgTypeId = " + (int)Constants.OrgTypeE.C_AND_F_AGENT +
                                        " AND prodCat.idProdCat=" + prodCatId + "AND prodSpec.idProdSpec=" + prodSpecId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaConfigTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingQuotaConfigTO> SelectEmptyLoadingQuotaConfig(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT 0 AS cnfOrgId , '' as cnfOrgName ,idMaterial as materialId,  " +
                                        " materialSubType as materialDesc ,0 AS idLoadQuotaConfig,1 AS isActive, " +
                                        " 0 AS allocPct, '' AS remark,tblMaterial.createdBy,tblMaterial.createdOn " +
                                        " , prodCat.prodCateDesc as prodCatDesc , prodSpec.prodSpecDesc " +
                                        " , prodCat.idProdCat as prodCatId , prodSpec.idProdSpec AS prodSpecId " +
                                        " FROM  tblMaterial " +
                                        " FULL OUTER JOIN dimProdCat prodCat" +
                                        " ON 1 = 1 " +
                                        " FULL OUTER JOIN dimProdSpec prodSpec" +
                                        " ON 1 = 1 " +
                                        " WHERE prodCat.isActive=1 AND prodSpec.isActive=1";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaConfigTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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
                cmdSelect.Dispose();
            }
        }

        public TblLoadingQuotaConfigTO SelectTblLoadingQuotaConfig(Int32 idLoadQuotaConfig)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idLoadQuotaConfig = " + idLoadQuotaConfig + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingQuotaConfigTO> list = ConvertDTToList(reader);
                reader.Dispose();
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

        public List<TblLoadingQuotaConfigTO> ConvertDTToList(SqlDataReader tblLoadingQuotaConfigTODT)
        {
            List<TblLoadingQuotaConfigTO> tblLoadingQuotaConfigTOList = new List<TblLoadingQuotaConfigTO>();
            if (tblLoadingQuotaConfigTODT != null)
            {
                while (tblLoadingQuotaConfigTODT.Read())
                {
                    TblLoadingQuotaConfigTO tblLoadingQuotaConfigTONew = new TblLoadingQuotaConfigTO();
                    if (tblLoadingQuotaConfigTODT["idLoadQuotaConfig"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.IdLoadQuotaConfig = Convert.ToInt32(tblLoadingQuotaConfigTODT["idLoadQuotaConfig"].ToString());
                    if (tblLoadingQuotaConfigTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.CnfOrgId = Convert.ToInt32(tblLoadingQuotaConfigTODT["cnfOrgId"].ToString());
                    if (tblLoadingQuotaConfigTODT["materialId"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.MaterialId = Convert.ToInt32(tblLoadingQuotaConfigTODT["materialId"].ToString());
                    if (tblLoadingQuotaConfigTODT["isActive"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.IsActive = Convert.ToInt32(tblLoadingQuotaConfigTODT["isActive"].ToString());
                    if (tblLoadingQuotaConfigTODT["createdBy"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.CreatedBy = Convert.ToInt32(tblLoadingQuotaConfigTODT["createdBy"].ToString());
                    if (tblLoadingQuotaConfigTODT["createdOn"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.CreatedOn = Convert.ToDateTime(tblLoadingQuotaConfigTODT["createdOn"].ToString());
                    if (tblLoadingQuotaConfigTODT["allocPct"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.AllocPct = Convert.ToDouble(tblLoadingQuotaConfigTODT["allocPct"].ToString());
                    if (tblLoadingQuotaConfigTODT["remark"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.Remark = Convert.ToString(tblLoadingQuotaConfigTODT["remark"].ToString());
                    if (tblLoadingQuotaConfigTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.CnfOrgName = Convert.ToString(tblLoadingQuotaConfigTODT["cnfOrgName"].ToString());
                    if (tblLoadingQuotaConfigTODT["materialDesc"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.MaterialDesc = Convert.ToString(tblLoadingQuotaConfigTODT["materialDesc"].ToString());
                    if (tblLoadingQuotaConfigTODT["prodCatId"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.ProdCatId = Convert.ToInt32(tblLoadingQuotaConfigTODT["prodCatId"].ToString());
                    if (tblLoadingQuotaConfigTODT["prodSpecId"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.ProdSpecId = Convert.ToInt32(tblLoadingQuotaConfigTODT["prodSpecId"].ToString());
                    if (tblLoadingQuotaConfigTODT["prodCatDesc"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.ProdCatDesc = Convert.ToString(tblLoadingQuotaConfigTODT["prodCatDesc"].ToString());
                    if (tblLoadingQuotaConfigTODT["prodSpecDesc"] != DBNull.Value)
                        tblLoadingQuotaConfigTONew.ProdSpecDesc = Convert.ToString(tblLoadingQuotaConfigTODT["prodSpecDesc"].ToString());

                    tblLoadingQuotaConfigTOList.Add(tblLoadingQuotaConfigTONew);
                }
            }
            return tblLoadingQuotaConfigTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingQuotaConfigTO, cmdInsert);
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

        public int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingQuotaConfigTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblLoadingQuotaConfig]( " +
                                "  [cnfOrgId]" +
                                " ,[materialId]" +
                                " ,[isActive]" +
                                " ,[createdBy]" +
                                " ,[createdOn]" +
                                " ,[allocPct]" +
                                " ,[remark]" +
                                " ,[prodCatId]" +
                                " ,[prodSpecId]" +
                                " )" +
                    " VALUES (" +
                                "  @CnfOrgId " +
                                " ,@MaterialId " +
                                " ,@IsActive " +
                                " ,@CreatedBy " +
                                " ,@CreatedOn " +
                                " ,@AllocPct " +
                                " ,@Remark " +
                                " ,@prodCatId " +
                                " ,@prodSpecId " +
                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadQuotaConfig", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.IdLoadQuotaConfig;
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.CnfOrgId;
            cmdInsert.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.MaterialId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaConfigTO.CreatedOn;
            cmdInsert.Parameters.Add("@AllocPct", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConfigTO.AllocPct;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingQuotaConfigTO.Remark);
            cmdInsert.Parameters.Add("@prodCatId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.ProdCatId;
            cmdInsert.Parameters.Add("@prodSpecId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.ProdSpecId;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingQuotaConfigTO.IdLoadQuotaConfig = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingQuotaConfigTO, cmdUpdate);
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

        public int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingQuotaConfigTO, cmdUpdate);
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

        public int DeactivateLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblLoadingQuotaConfig] SET " +
                                 "  [isActive]= @IsActive" +
                                 " ,[deactivatedOn]= @deactivatedOn" +
                                 " ,[deactivatedBy] = @deactivatedBy" +
                                 " WHERE [idLoadQuotaConfig] = @IdLoadQuotaConfig";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdLoadQuotaConfig", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.IdLoadQuotaConfig;
                //cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.CnfOrgId;
                //cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.MaterialId;
                cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@deactivatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaConfigTO.DeactivatedOn;
                cmdUpdate.Parameters.Add("@deactivatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.DeactivatedBy;
                return cmdUpdate.ExecuteNonQuery();
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

        public int ExecuteUpdationCommand(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblLoadingQuotaConfig] SET " + 
            "  [idLoadQuotaConfig] = @IdLoadQuotaConfig" +
            " ,[cnfOrgId]= @CnfOrgId" +
            " ,[materialId]= @MaterialId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[allocPct]= @AllocPct" +
            " ,[remark] = @Remark" +
            " WHERE 1 = 2 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadQuotaConfig", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.IdLoadQuotaConfig;
            cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.CnfOrgId;
            cmdUpdate.Parameters.Add("@MaterialId", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.MaterialId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingQuotaConfigTO.CreatedOn;
            cmdUpdate.Parameters.Add("@AllocPct", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConfigTO.AllocPct;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = tblLoadingQuotaConfigTO.Remark;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadQuotaConfig, cmdDelete);
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

        public int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadQuotaConfig, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idLoadQuotaConfig, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblLoadingQuotaConfig] " +
            " WHERE idLoadQuotaConfig = " + idLoadQuotaConfig +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadQuotaConfig", System.Data.SqlDbType.Int).Value = tblLoadingQuotaConfigTO.IdLoadQuotaConfig;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
