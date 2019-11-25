using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblQuotaDeclarationDAO : ITblQuotaDeclarationDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblQuotaDeclarationDAO(ICommon iCommon, IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT quota.*,rate.rate as declaredRate,brand.brandName,brand.idBrand as brandId  FROM [tblQuotaDeclaration] quota" +
                                  " LEFT JOIN [tblGlobalRate] rate ON rate.idGlobalRate=quota.globalRateId" +
                                  " LEFT JOIN [dimBrand] brand ON rate.brandId=brand.idBrand";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclaration()
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

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(tblGlobalRateTODT);

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

        public List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclaration(Int32 globalRateId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblGlobalRateTODT = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE globalRateId=" + globalRateId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                return ConvertDTToList(tblGlobalRateTODT);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblGlobalRateTODT != null) tblGlobalRateTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Aniket [30-4-2019]
        public TblQuotaDeclarationTO ConvertDT(SqlDataReader tblQuotaDeclarationTODT)
        {
            TblQuotaDeclarationTO tblQuotaDeclarationTONew = new TblQuotaDeclarationTO();
            if (tblQuotaDeclarationTODT != null)
            {
                while (tblQuotaDeclarationTODT.Read())
                {
                    
                    if (tblQuotaDeclarationTODT["idQuotaDeclaration"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IdQuotaDeclaration = Convert.ToInt32(tblQuotaDeclarationTODT["idQuotaDeclaration"].ToString());
                    if (tblQuotaDeclarationTODT["orgId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.OrgId = Convert.ToInt32(tblQuotaDeclarationTODT["orgId"].ToString());
                    if (tblQuotaDeclarationTODT["globalRateId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.GlobalRateId = Convert.ToInt32(tblQuotaDeclarationTODT["globalRateId"].ToString());
                    if (tblQuotaDeclarationTODT["createdBy"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CreatedBy = Convert.ToInt32(tblQuotaDeclarationTODT["createdBy"].ToString());
                    if (tblQuotaDeclarationTODT["quotaAllocDate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.QuotaAllocDate = Convert.ToDateTime(tblQuotaDeclarationTODT["quotaAllocDate"].ToString());
                    if (tblQuotaDeclarationTODT["createdOn"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CreatedOn = Convert.ToDateTime(tblQuotaDeclarationTODT["createdOn"].ToString());
                    if (tblQuotaDeclarationTODT["rate_band"] != DBNull.Value)
                        tblQuotaDeclarationTONew.RateBand = Convert.ToDouble(tblQuotaDeclarationTODT["rate_band"].ToString());
                    if (tblQuotaDeclarationTODT["alloc_qty"] != DBNull.Value)
                        tblQuotaDeclarationTONew.AllocQty = Convert.ToDouble(tblQuotaDeclarationTODT["alloc_qty"].ToString());
                    if (tblQuotaDeclarationTODT["balance_qty"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BalanceQty = Convert.ToDouble(tblQuotaDeclarationTODT["balance_qty"].ToString());
                    if (tblQuotaDeclarationTODT["calculatedRate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CalculatedRate = Convert.ToDouble(tblQuotaDeclarationTODT["calculatedRate"].ToString());
                    if (tblQuotaDeclarationTODT["validUpto"] != DBNull.Value)
                        tblQuotaDeclarationTONew.ValidUpto = Convert.ToInt32(tblQuotaDeclarationTODT["validUpto"].ToString());
                    if (tblQuotaDeclarationTODT["isActive"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IsActive = Convert.ToInt32(tblQuotaDeclarationTODT["isActive"].ToString());
                    if (tblQuotaDeclarationTODT["updatedBy"] != DBNull.Value)
                        tblQuotaDeclarationTONew.UpdatedBy = Convert.ToInt32(tblQuotaDeclarationTODT["updatedBy"].ToString());
                    if (tblQuotaDeclarationTODT["updatedOn"] != DBNull.Value)
                        tblQuotaDeclarationTONew.UpdatedOn = Convert.ToDateTime(tblQuotaDeclarationTODT["updatedOn"].ToString());
       
                  
                   
                }
            }
            return tblQuotaDeclarationTONew;
        }
        public List<TblQuotaDeclarationTO> ConvertDTToList(SqlDataReader tblQuotaDeclarationTODT)
        {
            List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();
            if (tblQuotaDeclarationTODT != null)
            {
                while (tblQuotaDeclarationTODT.Read())
                {
                    TblQuotaDeclarationTO tblQuotaDeclarationTONew = new TblQuotaDeclarationTO();
                    if (tblQuotaDeclarationTODT["idQuotaDeclaration"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IdQuotaDeclaration = Convert.ToInt32(tblQuotaDeclarationTODT["idQuotaDeclaration"].ToString());
                    if (tblQuotaDeclarationTODT["orgId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.OrgId = Convert.ToInt32(tblQuotaDeclarationTODT["orgId"].ToString());
                    if (tblQuotaDeclarationTODT["globalRateId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.GlobalRateId = Convert.ToInt32(tblQuotaDeclarationTODT["globalRateId"].ToString());
                    if (tblQuotaDeclarationTODT["createdBy"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CreatedBy = Convert.ToInt32(tblQuotaDeclarationTODT["createdBy"].ToString());
                    if (tblQuotaDeclarationTODT["quotaAllocDate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.QuotaAllocDate = Convert.ToDateTime(tblQuotaDeclarationTODT["quotaAllocDate"].ToString());
                    if (tblQuotaDeclarationTODT["createdOn"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CreatedOn = Convert.ToDateTime(tblQuotaDeclarationTODT["createdOn"].ToString());
                    if (tblQuotaDeclarationTODT["rate_band"] != DBNull.Value)
                        tblQuotaDeclarationTONew.RateBand = Convert.ToDouble(tblQuotaDeclarationTODT["rate_band"].ToString());
                    if (tblQuotaDeclarationTODT["alloc_qty"] != DBNull.Value)
                        tblQuotaDeclarationTONew.AllocQty = Convert.ToDouble(tblQuotaDeclarationTODT["alloc_qty"].ToString());
                    if (tblQuotaDeclarationTODT["balance_qty"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BalanceQty = Convert.ToDouble(tblQuotaDeclarationTODT["balance_qty"].ToString());
                    if (tblQuotaDeclarationTODT["calculatedRate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CalculatedRate = Convert.ToDouble(tblQuotaDeclarationTODT["calculatedRate"].ToString());
                    if (tblQuotaDeclarationTODT["validUpto"] != DBNull.Value)
                        tblQuotaDeclarationTONew.ValidUpto = Convert.ToInt32(tblQuotaDeclarationTODT["validUpto"].ToString());
                    if (tblQuotaDeclarationTODT["isActive"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IsActive = Convert.ToInt32(tblQuotaDeclarationTODT["isActive"].ToString());
                    if (tblQuotaDeclarationTODT["updatedBy"] != DBNull.Value)
                        tblQuotaDeclarationTONew.UpdatedBy = Convert.ToInt32(tblQuotaDeclarationTODT["updatedBy"].ToString());
                    if (tblQuotaDeclarationTODT["updatedOn"] != DBNull.Value)
                        tblQuotaDeclarationTONew.UpdatedOn = Convert.ToDateTime(tblQuotaDeclarationTODT["updatedOn"].ToString());
                    if (tblQuotaDeclarationTODT["declaredRate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.DeclaredRate = Convert.ToDouble(tblQuotaDeclarationTODT["declaredRate"].ToString());
                    if (tblQuotaDeclarationTODT["brandName"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BrandName = Convert.ToString(tblQuotaDeclarationTODT["brandName"].ToString());
                    if (tblQuotaDeclarationTODT["brandId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BrandId = Convert.ToInt32(tblQuotaDeclarationTODT["brandId"].ToString());
                    tblQuotaDeclarationTOList.Add(tblQuotaDeclarationTONew);
                }
            }
            return tblQuotaDeclarationTOList;
        }


        public List<TblQuotaDeclarationTO> ConvertDTToListQouta(SqlDataReader tblQuotaDeclarationTODT)
        {
            List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = new List<TblQuotaDeclarationTO>();
            if (tblQuotaDeclarationTODT != null)
            {
                while (tblQuotaDeclarationTODT.Read())
                {
                    TblQuotaDeclarationTO tblQuotaDeclarationTONew = new TblQuotaDeclarationTO();
                    if (tblQuotaDeclarationTODT["idQuotaDeclaration"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IdQuotaDeclaration = Convert.ToInt32(tblQuotaDeclarationTODT["idQuotaDeclaration"].ToString());
                    if (tblQuotaDeclarationTODT["orgId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.OrgId = Convert.ToInt32(tblQuotaDeclarationTODT["orgId"].ToString());
                    if (tblQuotaDeclarationTODT["globalRateId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.GlobalRateId = Convert.ToInt32(tblQuotaDeclarationTODT["globalRateId"].ToString());
                    if (tblQuotaDeclarationTODT["createdBy"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CreatedBy = Convert.ToInt32(tblQuotaDeclarationTODT["createdBy"].ToString());
                    if (tblQuotaDeclarationTODT["quotaAllocDate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.QuotaAllocDate = Convert.ToDateTime(tblQuotaDeclarationTODT["quotaAllocDate"].ToString());
                    if (tblQuotaDeclarationTODT["createdOn"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CreatedOn = Convert.ToDateTime(tblQuotaDeclarationTODT["createdOn"].ToString());
                    if (tblQuotaDeclarationTODT["rate_band"] != DBNull.Value)
                        tblQuotaDeclarationTONew.RateBand = Convert.ToDouble(tblQuotaDeclarationTODT["rate_band"].ToString());
                    if (tblQuotaDeclarationTODT["alloc_qty"] != DBNull.Value)
                        tblQuotaDeclarationTONew.AllocQty = Convert.ToDouble(tblQuotaDeclarationTODT["alloc_qty"].ToString());
                    if (tblQuotaDeclarationTODT["balance_qty"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BalanceQty = Convert.ToDouble(tblQuotaDeclarationTODT["balance_qty"].ToString());
                    if (tblQuotaDeclarationTODT["calculatedRate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.CalculatedRate = Convert.ToDouble(tblQuotaDeclarationTODT["calculatedRate"].ToString());
                    if (tblQuotaDeclarationTODT["validUpto"] != DBNull.Value)
                        tblQuotaDeclarationTONew.ValidUpto = Convert.ToInt32(tblQuotaDeclarationTODT["validUpto"].ToString());
                    if (tblQuotaDeclarationTODT["isActive"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IsActive = Convert.ToInt32(tblQuotaDeclarationTODT["isActive"].ToString());
                    if (tblQuotaDeclarationTODT["updatedBy"] != DBNull.Value)
                        tblQuotaDeclarationTONew.UpdatedBy = Convert.ToInt32(tblQuotaDeclarationTODT["updatedBy"].ToString());
                    if (tblQuotaDeclarationTODT["updatedOn"] != DBNull.Value)
                        tblQuotaDeclarationTONew.UpdatedOn = Convert.ToDateTime(tblQuotaDeclarationTODT["updatedOn"].ToString());
                    if (tblQuotaDeclarationTODT["declaredRate"] != DBNull.Value)
                        tblQuotaDeclarationTONew.DeclaredRate = Convert.ToDouble(tblQuotaDeclarationTODT["declaredRate"].ToString());
                    if (tblQuotaDeclarationTODT["brandName"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BrandName = Convert.ToString(tblQuotaDeclarationTODT["brandName"].ToString());
                    if (tblQuotaDeclarationTODT["brandId"] != DBNull.Value)
                        tblQuotaDeclarationTONew.BrandId = Convert.ToInt32(tblQuotaDeclarationTODT["brandId"].ToString());
                    if (tblQuotaDeclarationTODT["isDefualtBrnd"] != DBNull.Value)
                        tblQuotaDeclarationTONew.IsDefualtBrand = Convert.ToInt32(tblQuotaDeclarationTODT["isDefualtBrnd"].ToString());
                    tblQuotaDeclarationTOList.Add(tblQuotaDeclarationTONew);
                }
            }
            return tblQuotaDeclarationTOList;
        }

        public TblQuotaDeclarationTO SelectTblQuotaDeclaration(Int32 idQuotaDeclaration)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idQuotaDeclaration = " + idQuotaDeclaration + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaDeclarationTO> list = ConvertDTToList(tblGlobalRateTODT);
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

        public TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration, Int32 cnfOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idQuotaDeclaration IN(SELECT TOP 1 idQuotaDeclaration  FROM tblQuotaDeclaration quota " +
                                        " WHERE orgId = " + cnfOrgId + " AND idQuotaDeclaration NOT IN(" + idQuotaDeclaration + ") ORDER BY idQuotaDeclaration DESC)";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaDeclarationTO> list = ConvertDTToList(tblGlobalRateTODT);
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

        public TblQuotaDeclarationTO SelectTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idQuotaDeclaration = " + idQuotaDeclaration + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
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
                sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idQuotaDeclaration IN(SELECT TOP 1 idQuotaDeclaration FROM tblQuotaDeclaration ORDER BY createdOn DESC) ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaDeclarationTO> list = ConvertDTToList(sqlReader);
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
                if (sqlReader != null)
                    sqlReader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblQuotaDeclarationTO> SelectLatestQuotaDeclaration(Int32 orgId,DateTime date,Boolean isQuotaDeclaration)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                //cmdSelect.CommandText = " SELECT TOP 1 quota.*,rate.rate as declaredRate FROM tblQuotaDeclaration quota " +
                //                        " LEFT JOIN [tblGlobalRate] rate ON rate.idGlobalRate=quota.globalRateId" +
                //                        " WHERE orgId=" + orgId + " ORDER BY quotaAllocDate DESC";

                //cmdSelect.CommandText = " SELECT TOP 2 quota.*,rate.rate as declaredRate FROM tblQuotaDeclaration quota " +
                //                        " LEFT JOIN [tblGlobalRate] rate ON rate.idGlobalRate=quota.globalRateId" +
                //                        " WHERE DAY(quota.createdOn)=" + date.Day + " AND MONTH(quota.createdOn)=" + date.Month + " AND YEAR(quota.createdOn)= " + date.Year +
                //                        " AND orgId=" + orgId + " AND isActive=1 ORDER BY quotaAllocDate DESC";

                //cmdSelect.CommandText = " SELECT quota.*,latestRateInfo.rate as declaredRate,latestRateInfo.brandName,latestRateInfo.idBrand as brandId  FROM tblQuotaDeclaration quota " +
                //                        " LEFT JOIN (SELECT rateDtl.*,brand.brandName,brand.idBrand FROM tblGlobalRate rateDtl " +
                //                        " INNER JOIN(SELECT brandId, MAX(idGlobalRate) idGlobalRate  FROM tblGlobalRate GROUP BY brandId) AS latestRate " +
                //                        " ON rateDtl.brandId = latestRate.brandId AND rateDtl.idGlobalRate = latestRate.idGlobalRate 	" +
                //                        " LEFT JOIN dimBrand brand on brand.idBrand=latestRate.brandId) AS latestRateInfo ON latestRateInfo.idGlobalRate = quota.globalRateId WHERE";
                // Above query commented by Aniket [4-7-2019] as discussed with saket, and written below new query
                // new query has been written due to validUp mins issues while rate declaration
                cmdSelect.CommandText= " SELECT quota.*,g.rate as declaredRate,b.brandName,b.idBrand as brandId,b.isDefault as isDefualtBrnd " + 
                                        " FROM tblQuotaDeclaration quota "+
                                        " LEFT JOIN tblGlobalRate g ON quota.globalRateId = g.idGlobalRate "+
                                        " left join dimBrand b ON b.idBrand = g.brandId WHERE";
                cmdSelect.CommandText += "  orgId=" + orgId  ;

                if (isQuotaDeclaration)
                {
                    cmdSelect.CommandText += " AND DAY(quota.createdOn)=" + date.Day + " AND MONTH(quota.createdOn)=" + date.Month + " AND YEAR(quota.createdOn)= " + date.Year;
                }
                cmdSelect.CommandText += " AND b.isActive = 1 and quota.isActive = 1 ORDER BY quotaAllocDate DESC";




                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader tblGlobalRateTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblQuotaDeclarationTO> list = ConvertDTToListQouta(tblGlobalRateTODT);
                if (tblGlobalRateTODT != null)
                    tblGlobalRateTODT.Dispose();
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

        public ODLMWebAPI.DashboardModels.QuotaAndRateInfo SelectDashboardQuotaAndRateInfo(Int32 roletypeId, Int32 orgId, DateTime sysDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            String whereCond = string.Empty;
            try
            {

                //DimRoleTypeTO roleTypeTO = BL.DimRoleTypeBL.SelectDimRoleTypeTO((int)Constants.SystemRoleTypeE.C_AND_F_AGENT);
                //if (roleTypeTO != null)
                //{
                //    string temp = roleTypeTO.RoleId;
                //    List<string> list = temp.Split(',').ToList();

                //    if (list.Contains(roleId.ToString()))
                //if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                if (roletypeId == (int)Constants.SystemRoleTypeE.C_AND_F_AGENT)
                {
                    whereCond = " AND orgId=" + orgId;
                    }
                //}

                //if (roleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                //{
                //    whereCond = " AND orgId=" + orgId;
                //}

                conn.Open();
                cmdSelect.CommandText = " SELECT MAX(declaredRate.rate) latestRate,SUM(alloc_qty) AS totalQuota ,AVG(alloc_qty) AS avgRateBand" +
                                        " FROM tblQuotaDeclaration quota " +
                                        " INNER JOIN tblGlobalRate declaredRate " +
                                        " ON quota.globalRateId = declaredRate.idGlobalRate " +
                                        " WHERE declaredRate.idGlobalRate = (SELECT MAX(idGlobalRate) idGlobalRate " +
                                        " FROM tblGlobalRate WHERE DAY(createdOn) = " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + ")" + whereCond;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {
                    ODLMWebAPI.DashboardModels.QuotaAndRateInfo quotaAndRateInfoNew = new ODLMWebAPI.DashboardModels.QuotaAndRateInfo();
                    if (tblLoadingTODT["totalQuota"] != DBNull.Value)
                        quotaAndRateInfoNew.TotalQuota = Convert.ToDouble(tblLoadingTODT["totalQuota"].ToString());
                    if (tblLoadingTODT["latestRate"] != DBNull.Value)
                        quotaAndRateInfoNew.DeclaredRate = Convert.ToDouble(tblLoadingTODT["latestRate"].ToString());
                    if (tblLoadingTODT["avgRateBand"] != DBNull.Value)
                        quotaAndRateInfoNew.AvgRateBand = Convert.ToDouble(tblLoadingTODT["avgRateBand"].ToString());

                    return quotaAndRateInfoNew;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<ODLMWebAPI.DashboardModels.QuotaAndRateInfo> SelectDashboardQuotaAndRateInfoList(Int32 roleId, Int32 orgId, DateTime sysDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            String whereCond = string.Empty;
            try
            {
                //if (roleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                //{
                //    whereCond = " AND orgId=" + orgId;
                //}
                conn.Open();
                //cmdSelect.CommandText = " SELECT MAX(declaredRate.rate) latestRate,SUM(alloc_qty) AS totalQuota ,AVG(alloc_qty) AS avgRateBand" +
                //                        " FROM tblQuotaDeclaration quota " +
                //                        " INNER JOIN tblGlobalRate declaredRate " +
                //                        " ON quota.globalRateId = declaredRate.idGlobalRate " +
                //                        " WHERE declaredRate.idGlobalRate = (SELECT MAX(idGlobalRate) idGlobalRate " +
                //                        " FROM tblGlobalRate WHERE DAY(createdOn) = " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + ")" + whereCond;


                cmdSelect.CommandText = " SELECT tblGlobalRate.*,dimBrand.brandName,tblGroup.groupName FROM tblGlobalRate tblGlobalRate " +
                                        " LEFT JOIN dimBrand ON dimBrand.idBrand = tblGlobalRate.brandId " +
                                        " LEFT JOIN tblGroup ON tblGroup.idGroup = tblGlobalRate.groupId " +
                                        " where tblGlobalRate.createdOn = (select top 1 createdOn from tblGlobalRate order by createdOn desc) ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                List<ODLMWebAPI.DashboardModels.QuotaAndRateInfo> quotaAndRateInfoNewList = new List<DashboardModels.QuotaAndRateInfo>();

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {
                    ODLMWebAPI.DashboardModels.QuotaAndRateInfo quotaAndRateInfoNew = new ODLMWebAPI.DashboardModels.QuotaAndRateInfo();
                    //if (tblLoadingTODT["totalQuota"] != DBNull.Value)
                    //    quotaAndRateInfoNew.TotalQuota = Convert.ToDouble(tblLoadingTODT["totalQuota"].ToString());
                    //if (tblLoadingTODT["latestRate"] != DBNull.Value)
                    //    quotaAndRateInfoNew.DeclaredRate = Convert.ToDouble(tblLoadingTODT["latestRate"].ToString());
                    //if (tblLoadingTODT["avgRateBand"] != DBNull.Value)
                    //    quotaAndRateInfoNew.AvgRateBand = Convert.ToDouble(tblLoadingTODT["avgRateBand"].ToString());

                    if (tblLoadingTODT["rate"] != DBNull.Value)
                        quotaAndRateInfoNew.DeclaredRate = Convert.ToDouble(tblLoadingTODT["rate"].ToString());

                    if (tblLoadingTODT["brandName"] != DBNull.Value)
                        quotaAndRateInfoNew.DisplayName = Convert.ToString(tblLoadingTODT["brandName"].ToString());
                    if (tblLoadingTODT["groupName"] != DBNull.Value)
                        quotaAndRateInfoNew.DisplayName = Convert.ToString(tblLoadingTODT["groupName"].ToString());

                    quotaAndRateInfoNewList.Add(quotaAndRateInfoNew);
                }

                return quotaAndRateInfoNewList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblQuotaDeclarationTO, cmdInsert);
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

        public int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblQuotaDeclarationTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblQuotaDeclaration]( " +
                            "  [orgId]" +
                            " ,[globalRateId]" +
                            " ,[createdBy]" +
                            " ,[quotaAllocDate]" +
                            " ,[createdOn]" +
                            " ,[rate_band]" +
                            " ,[alloc_qty]" +
                            " ,[balance_qty]" +
                            " ,[calculatedRate]" +
                            " ,[isActive]" +
                            " ,[updatedBy]" +
                            " ,[updatedOn]" +
                            " ,[validUpto]"+
                            " )" +
                " VALUES (" +
                            "  @OrgId " +
                            " ,@GlobalRateId " +
                            " ,@CreatedBy " +
                            " ,@QuotaAllocDate " +
                            " ,@CreatedOn " +
                            " ,@RateBand " +
                            " ,@AllocQty " +
                            " ,@BalanceQty " +
                            " ,@CalculatedRate " +
                            " ,@isActive " +
                            " ,@updatedBy " +
                            " ,@updatedOn " +
                            " ,@ValidUpto " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.OrgId;
            cmdInsert.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.GlobalRateId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.CreatedBy;
            cmdInsert.Parameters.Add("@QuotaAllocDate", System.Data.SqlDbType.DateTime).Value = tblQuotaDeclarationTO.QuotaAllocDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblQuotaDeclarationTO.CreatedOn;
            cmdInsert.Parameters.Add("@RateBand", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.RateBand;
            cmdInsert.Parameters.Add("@AllocQty", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.AllocQty;
            cmdInsert.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.BalanceQty;
            cmdInsert.Parameters.Add("@CalculatedRate", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.CalculatedRate;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.IsActive;
            cmdInsert.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQuotaDeclarationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblQuotaDeclarationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@ValidUpto", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblQuotaDeclarationTO.ValidUpto);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblQuotaDeclarationTO.IdQuotaDeclaration = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            DateTime serverDateTime = _iCommon.ServerDateTime;
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                if(tblQuotaDeclarationTO.UpdatedOn==DateTime.MinValue)
                {
                    tblQuotaDeclarationTO.UpdatedOn = serverDateTime;
                }
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblQuotaDeclarationTO, cmdUpdate);
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

        public int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblQuotaDeclarationTO, cmdUpdate);
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

        public int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblQuotaDeclaration] SET " +
                                  "  [isActive] = @isActive " +
                                  " ,[updatedBy] = @updatedBy " +
                                  " ,[validUpto] = @validUpto " +
                                  " ,[updatedOn] = @updatedOn ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = 0;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = updatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = _iCommon.ServerDateTime;

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

        public int UpdateQuotaDeclarationValidity(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tblQuotaDeclaration] SET " +
                                  "  [validUpto] = @validUpto " +
                                  " ,[isActive] = @isActive " +
                                  " ,[updatedBy] = @updatedBy " +
                                  " ,[updatedOn] = @updatedOn " +
                             " WHERE  [idQuotaDeclaration] = @IdQuotaDeclaration AND [orgId]=@OrgId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdQuotaDeclaration", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.IdQuotaDeclaration;
                cmdUpdate.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.OrgId;
                cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.ValidUpto;
                cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.IsActive;
                cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblQuotaDeclarationTO.UpdatedOn;

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

        public int ExecuteUpdationCommand(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblQuotaDeclaration] SET " +
                            "  [orgId]= @OrgId" +
                            " ,[globalRateId]= @GlobalRateId" +
                            " ,[quotaAllocDate]= @QuotaAllocDate" +
                            " ,[rate_band]= @RateBand" +
                            " ,[alloc_qty]= @AllocQty" +
                            " ,[balance_qty]= @BalanceQty" +
                            " ,[calculatedRate] = @CalculatedRate" +
                            ", [validUpto] = @validUpto" +
                            ", [isActive] = @isActive" +
                            ", [updatedBy] = @updatedBy" +
                            ", [updatedOn] = @updatedOn" +
                            " WHERE[idQuotaDeclaration] = @IdQuotaDeclaration";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdQuotaDeclaration", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.IdQuotaDeclaration;
            cmdUpdate.Parameters.Add("@OrgId", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.OrgId;
            cmdUpdate.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.GlobalRateId;
            cmdUpdate.Parameters.Add("@QuotaAllocDate", System.Data.SqlDbType.DateTime).Value = tblQuotaDeclarationTO.QuotaAllocDate;
            cmdUpdate.Parameters.Add("@RateBand", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.RateBand;
            cmdUpdate.Parameters.Add("@AllocQty", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.AllocQty;
            cmdUpdate.Parameters.Add("@BalanceQty", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.BalanceQty;
            cmdUpdate.Parameters.Add("@CalculatedRate", System.Data.SqlDbType.NVarChar).Value = tblQuotaDeclarationTO.CalculatedRate;
            cmdUpdate.Parameters.Add("@validUpto", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.ValidUpto;
            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.IsActive;
            cmdUpdate.Parameters.Add("@updatedBy", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@updatedOn", System.Data.SqlDbType.DateTime).Value = tblQuotaDeclarationTO.UpdatedOn;

            return cmdUpdate.ExecuteNonQuery();
        }

        //Aniket [29-4-2019]
        public TblQuotaDeclarationTO GetBookingQuotaAgainstCNF(Int32 cnfOrgId, Int32 brandId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            try
            {
                TblQuotaDeclarationTO tblQuotaDeclarationTO = new TblQuotaDeclarationTO();
                SqlCommand updateCommand = new SqlCommand();
                DateTime serverDate = _iCommon.ServerDateTime;
                updateCommand.Connection = conn;
                conn.Open();
                updateCommand.CommandText = "select * from tblQuotaDeclaration  where orgId= @cnfOrgId " +
                    "and globalRateId= ( select idGlobalRate from tblGlobalRate where cast(createdOn as date) = cast( @currentDate as date) and brandId = @brandId)";
               
                updateCommand.Parameters.AddWithValue("@currentDate", DbType.DateTime).Value = serverDate;
                updateCommand.Parameters.AddWithValue("@brandId", DbType.Int32).Value = brandId;
                updateCommand.Parameters.AddWithValue("@cnfOrgId", DbType.Int32).Value = cnfOrgId;
                SqlDataReader dataReader = updateCommand.ExecuteReader();
                tblQuotaDeclarationTO = ConvertDT(dataReader);
               if(tblQuotaDeclarationTO!=null)
                {
                    return tblQuotaDeclarationTO;
                }
               else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;  
            }
            finally
            {
                conn.Close();
            }
            // return resultMessage;
        }
        #endregion

        #region Deletion
        public int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idQuotaDeclaration, cmdDelete);
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

        public int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idQuotaDeclaration, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idQuotaDeclaration, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblQuotaDeclaration] " +
            " WHERE idQuotaDeclaration = " + idQuotaDeclaration +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idQuotaDeclaration", System.Data.SqlDbType.Int).Value = tblQuotaDeclarationTO.IdQuotaDeclaration;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
