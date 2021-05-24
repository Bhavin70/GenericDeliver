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
using ODLMWebAPI.DashboardModels;

namespace ODLMWebAPI.DAL
{
    public class TblLoadingDAO : ITblLoadingDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        public TblLoadingDAO(IConnectionString iConnectionString, ITblConfigParamsDAO iTblConfigParamsDAO)
        {
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            //Pandurang [2018-03-21] commented digitalSign as discussed with Saket
            String sqlSelectQry = " SELECT loading.* " +
                                  //",org.digitalSign" +
                                  ", org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName " +
                                   " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                   " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +

                                    // Vaibhav [09-Jan-2018] added to select from finalLoading
                                    " UNION ALL " +

                                  " SELECT loading.* " +
                                  //",org.digitalSign" +
                                  ", org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName"+
                                   " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM finalLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy" +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId "; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public tblUserMachineMappingTo SelectUserMachineTo(int userId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM tblusermachinemapping where userId = " + userId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                tblUserMachineMappingTo MachineMappingTo = new tblUserMachineMappingTo();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["idUserMachineMapping"] != DBNull.Value)
                            MachineMappingTo.IdUserMachineMapping = Convert.ToInt32(sqlReader["idUserMachineMapping"].ToString());
                        if (sqlReader["userId"] != DBNull.Value)
                            MachineMappingTo.UserId = Convert.ToInt32(sqlReader["userId"].ToString());
                        if (sqlReader["gateId"] != DBNull.Value)
                            MachineMappingTo.GateId = Convert.ToInt32(sqlReader["gateId"].ToString());
                    }
                }
                return MachineMappingTo;
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

        //Hrushikesh Added for IOT for non multi tenancy Config
        //this method is purposely kept static refer GeModRefMaxData() for its non static version.
         public static List<int> GeModRefMaxDataNonMulti()
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 255 modbusRefId FROM tempLoading WHERE modbusRefId IS NOT NULL ORDER BY modbusRefId DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> list = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        int modRefId = 0;
                        if (sqlReader["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(sqlReader["modbusRefId"].ToString());
                        if (modRefId > 0)
                            list.Add(modRefId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        } 


        //Aniket [30-7-2019] added for IOT
        public List<int> GeModRefMaxData()
        {
            SqlCommand cmdSelect = new SqlCommand();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT TOP 255 modbusRefId FROM tempLoading WHERE modbusRefId IS NOT NULL ORDER BY modbusRefId DESC";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<int> list = new List<int>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        int modRefId = 0;
                        if (sqlReader["modbusRefId"] != DBNull.Value)
                            modRefId = Convert.ToInt32(sqlReader["modbusRefId"].ToString());
                        if (modRefId > 0)
                            list.Add(modRefId);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


       
        public List<TblLoadingTO> SelectAllTblLoading()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
      
        //Priyanka [11-05-2018]
        public List<TblLoadingTO> SelectAllTblLoadingListForConvertNCToC()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "Select * from ("+ SqlSelectQuery()  + " )As sq1 where sq1.statusId IN (" + (int)Constants.TranStatusE.LOADING_GATE_IN + "," + (int)Constants.TranStatusE.LOADING_COMPLETED +")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public TblLoadingTO SelectTblLoadingTOByModBusRefId(Int32 modBusRefId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoading in ( select loadingId from tempLoadingSlip where modbusRefId =" + modBusRefId + ") " +

                                      // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlip

                                      " UNION ALL " + " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoading in ( select loadingId from finalLoadingSlip where modbusRefId =" + modBusRefId + ") ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingTO> SelectAllTblloadingList(DateTime fromDate, DateTime toDate,string selectedOrgStr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = "SELECT * FROM(" + SqlSelectQuery() + ")sq1 WHERE (sq1.idLoading " +
                    "IN (SELECT loadingId FROM tempWeighingMeasures where createdOn BETWEEN @fromDate AND @toDate) OR " +
                    " sq1.idLoading  IN (SELECT loadingId FROM finalWeighingMeasures where createdOn BETWEEN @fromDate AND @toDate)) ";

                if(!string.IsNullOrEmpty(selectedOrgStr))
                {
                    cmdSelect.CommandText += " And sq1.fromOrgId in("+ selectedOrgStr + ")";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        
        public List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId(Int32 parentLoadingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.parentLoadingId=" + parentLoadingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


   



        public List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId, SqlConnection conn, SqlTransaction tran,Int32 gateId=0)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                if (gateId == 0)
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId IN(" + statusId + ")";
                else
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.statusId IN(" + statusId + ") AND sq1.gateId = " + gateId; 

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
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


        public List<TblLoadingTO> SelectAllTblLoading(TblUserRoleTO tblUserRoleTO, int cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId,string selectedOrgStr, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId,Int32 superwisorId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string whereSupCond = string.Empty;

            string whereCond = " WHERE CONVERT (DATE,loading.createdOn,103) BETWEEN @fromDate AND @toDate";
            string areConfJoin = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                if (isConfEn == 1)
                {
                    areConfJoin = " INNER JOIN ( SELECT DISTINCT cnfOrgId FROM tblUserAreaAllocation WHERE isActive=1 AND userId=" + userId + ") areaConf ON  areaConf.cnfOrgId = loading.cnfOrgId ";
                }
                

                conn.Open();
                //if (cnfId == 0 && loadingStatusId > 0)
                //    whereCond += " AND loading.statusId=" + loadingStatusId;
                //else if (cnfId > 0 && loadingStatusId > 0)
                //    whereCond += " AND loading.cnfOrgId=" + cnfId + " AND loading.statusId=" + loadingStatusId;
                //else if (cnfId > 0 && loadingStatusId == 0)
                //    whereCond += " AND loading.cnfOrgId=" + cnfId;


                String wherecnfIdTemp = String.Empty;
                String wherecnfIdFinal = String.Empty;


                String whereisConTemp = String.Empty;
                String whereisConFinal = String.Empty;

                if (cnfId > 0)
                {
                    wherecnfIdTemp += " AND loading.idLoading IN ( SELECT loadingId from tempLoadingSlip where cnfOrgId = " + cnfId + " ) " ;
                    wherecnfIdFinal += " AND loading.idLoading IN ( SELECT loadingId from finalLoadingSlip where cnfOrgId = " + cnfId + " ) ";
                }

                if(dealerId > 0)
                {
                    wherecnfIdTemp += " AND loading.idLoading IN ( SELECT loadingId from tempLoadingSlip where dealerOrgId = " + dealerId + " ) ";
                    wherecnfIdFinal += " AND loading.idLoading IN ( SELECT loadingId from finalLoadingSlip where dealerOrgId = " + dealerId + " ) ";
                }

                if (brandId > 0)
                {
                    wherecnfIdTemp += " AND loading.idLoading IN ( SELECT loadingId from tempLoadingSlip where idLoadingSlip IN (select loadingSlipId from tempLoadingSlipExt where brandId = " + brandId + " ) ) ";
                    wherecnfIdFinal += " AND loading.idLoading IN ( SELECT loadingId from finalLoadingSlip where idLoadingSlip IN (select loadingSlipId from finalLoadingSlipExt where brandId = " + brandId + ") ) ";
                }

               
                //Priyanka [18-08-2018] : Added for navigate through alert on specific loading slip 
                if (loadingNavigateId > 0)
                {
                    whereCond = " WHERE loading.idLoading =" + loadingNavigateId;
                }
                if (loadingTypeId > 0)
                {
                    whereCond += " AND loading.loadingType=" + loadingTypeId;
                }

                if (superwisorId > 0)
                {
                    whereSupCond += " AND loading.superwisorId=" + superwisorId;

                }

                if (loadingStatusId > 0)
                    whereCond += " AND loading.statusId=" + loadingStatusId;

                if (isConfEn == 0 && (!string.IsNullOrEmpty(selectedOrgStr)))
                {
                    whereCond += " AND isnull(loading.fromOrgId,0) in(" + selectedOrgStr+")";
                }

                    //Priyanka [31-05-2018] : Added to show the confirm and non-confirm loading slip.
                if (isConfirm == 0 || isConfirm == 1)
                {
                    whereisConTemp += " AND ISNULL(loadingSlip.isConfirmed,0) = " + isConfirm;
                    whereisConFinal += " AND ISNULL(loadingSlip.isConfirmed,0) = " + isConfirm;

                    //    whereisConTemp += " AND loading.idLoading IN ( select loadingId from tempLoadingSlip where ISNULL(isConfirmed,0) = " + isConfirm + ")";
                    //    whereisConFinal += " AND loading.idLoading IN ( select loadingId from finalLoadingSlip where ISNULL(isConfirmed,0) = " + isConfirm + ")";
                }
                //cmdSelect.CommandText = SqlSelectQuery() + areConfJoin + whereCond;

                // Vaibhav [09-Jan-2018] Commented and added to select from finalLoading
                String sqlQuery = " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName  " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP, " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " tblOrganization.firmName  + ',' +  CASE WHEN tblOrganization.addrId IS NULL THEN '' ELSE " +
                                  " CASE WHEN address.villageName IS NOT NULL  THEN address.villageName ELSE " +
                                  " CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName  ELSE " +
                                  " CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE " +
                                  " address.stateName END END END END AS  dealerOrgName " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " LEFT JOIN tempLoadingSlip loadingSlip ON loadingSlip.loadingId = loading.idLoading " +
                                  " LEFT JOIN tblOrganization ON tblOrganization.idOrganization = loadingSlip.dealerOrgId " +
                                  " LEFT JOIN vAddressDetails address ON address.idAddr = tblOrganization.addrId " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " + areConfJoin + whereCond + wherecnfIdTemp + whereisConTemp + whereSupCond+
                                  
                                  " UNION ALL " +

                                  " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP, " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " tblOrganization.firmName  + ',' +  CASE WHEN tblOrganization.addrId IS NULL THEN '' ELSE " +
                                  " CASE WHEN address.villageName IS NOT NULL  THEN address.villageName ELSE " +
                                  " CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName  ELSE " +
                                  " CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE " +
                                  " address.stateName END END END END AS  dealerOrgName " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " FROM finalLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " LEFT JOIN finalLoadingSlip loadingSlip ON loadingSlip.loadingId = loading.idLoading " +
                                  " LEFT JOIN tblOrganization ON tblOrganization.idOrganization = loadingSlip.dealerOrgId " +
                                  " LEFT JOIN vAddressDetails address ON address.idAddr = tblOrganization.addrId " +
                                  //Added by minal 12 May 2021 For show dealer to approval screen
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " + areConfJoin + whereCond + wherecnfIdFinal + whereisConFinal  +whereSupCond;


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToListForDealer(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        } 
        /// <summary>
        /// @kira 12-12-2017 for get loading slip agians dealer
        /// </summary>

        public List<TblLoadingTO> SelectAllTblLoadingLinkList(TblUserRoleTO tblUserRoleTO, int dearlerOrgId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string whereCond = " WHERE idloading IN ((SELECT distinct loadingId FROM temploadingslip WHERE dealerOrgId="+ dearlerOrgId  + " AND statusId IN("+ loadingStatusId + ")))" +
            "AND vehicleNo is NULL AND CONVERT (DATE,loading.createdOn,103) BETWEEN @fromDate AND @toDate";
            

            string areConfJoin = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                conn.Open();
                //cmdSelect.CommandText = SqlSelectQuery() + whereCond;

                // Vaibhav [09-Jan-2018] Commented and added to select from finalLoading

                String sqlQuery = " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                   " ,tblUser.userDisplayName " +
                                   " FROM tempLoading loading " +
                                   " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                   " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                   " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                   " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                   " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                   " LEFT JOIN tblUser ON idUser=loading.createdBy " +

                                   " WHERE idloading IN ((SELECT distinct loadingId FROM temploadingslip WHERE dealerOrgId=" + dearlerOrgId + " AND statusId IN(" + loadingStatusId + ")))" +
                                   " AND vehicleNo is NULL AND CONVERT (DATE,loading.createdOn,103) BETWEEN @fromDate AND @toDate" +

                                   " UNION ALL " +

                                   " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                   " ,tblUser.userDisplayName " +
                                   " FROM finalLoading loading " +
                                   " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                   " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                   " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                   " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                   " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                   " LEFT JOIN tblUser ON idUser=loading.createdBy " +

                                   " WHERE idloading IN ((SELECT distinct loadingId FROM finalloadingslip WHERE dealerOrgId=" + dearlerOrgId + " AND statusId IN(" + loadingStatusId + ")))" +
                                   " AND vehicleNo is NULL AND CONVERT (DATE,loading.createdOn,103) BETWEEN @fromDate AND @toDate";


                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
          
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblLoadingTO> SelectAllTblLoading(int cnfId, String loadingStatusIdIn, DateTime loadingDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string whereCond = " WHERE DAY (loading.createdOn)=" + loadingDate.Day + " AND MONTH(loading.createdOn)=" + loadingDate.Month + " AND YEAR(loading.createdOn)=" + loadingDate.Year;
            try
            {
                conn.Open();
                if (cnfId == 0)
                    whereCond += " AND statusId IN(" + loadingStatusIdIn + ")";
                else if (cnfId > 0)
                    whereCond += " AND cnfOrgId=" + cnfId + " AND statusId IN(" + loadingStatusIdIn + ")";

                cmdSelect.CommandText = SqlSelectQuery() + whereCond;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Aniket [22-8-2019] added for without connnection trans
        //requirement for IoT
        public TblLoadingTO SelectTblLoading(Int32 idLoading)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoading = " + idLoading + " ";
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Connection = conn;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public TblLoadingTO SelectTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoading = " + idLoading + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public TblLoadingTO SelectTblLoadingByLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE idLoading in ( select loadingId from temploadingslip where idLoadingSlip =" + loadingSlipId + ") ";

                // Vaibhav [09-Jan-2018] Commented and added to select from finalLoading

                  string sqlQuery=" SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  " WHERE idLoading in ( select loadingId from temploadingslip where idLoadingSlip =" + loadingSlipId + ") " +

                                  " UNION ALL " +

                                  " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName " +
                                   " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM finalLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  " WHERE idLoading in ( select loadingId from finalloadingslip where idLoadingSlip =" + loadingSlipId + ") ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(reader);
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
                if (reader != null) reader.Dispose();
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public Int64 SelectCountOfLoadingSlips(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT COUNT(*) as slipCount FROM tempLoading WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year;

                // Vaibhav [09-Jan-2018] Commented and add to select form finalLoading
                String sqlQuery = "Select  (SELECT COUNT(*)  FROM tempLoading WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + " ) " +
                                 "  + " +
                                 " (SELECT COUNT(*)  FROM finalLoading WHERE DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + " ) AS slipCount";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                Int64 slipCount = 0;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (reader["slipCount"] != DBNull.Value)
                        slipCount = Convert.ToInt64(reader["slipCount"].ToString());
                }

                return slipCount;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public LoadingInfo SelectDashboardLoadingInfo(TblUserRoleTO tblUserRoleTO, Int32 orgId, DateTime sysDate , Int32 loadingType)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            String whereCond = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                //DimRoleTypeTO roleTypeTO = BL.DimRoleTypeBL.SelectDimRoleTypeTO((int)Constants.SystemRoleTypeE.C_AND_F_AGENT);
                //if (roleTypeTO != null)
                //{
                //    string temp = roleTypeTO.RoleId;
                //    List<string> list = temp.Split(',').ToList();

                    //if (list.Contains(tblUserRoleTO.RoleId.ToString()))
                    // if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                    if (orgId >0)
                    {
                        whereCond = " AND slip.cnfOrgId=" + orgId;
                    }
                //}

                //if (loadingType > 0)
                //{
                //    whereCond += " AND loadingType =  " + loadingType;
                //}

                conn.Open();
                //[05-09-2018]Vijaymala added to get loading status  for other or regular 
                if (isConfEn == 1)
                {
                    string areaConfStr = " INNER JOIN " +
                                                " ( " +
                                                " SELECT areaConf.cnfOrgId, idOrganization " +
                                                " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                                " INNER JOIN " +
                                                " ( " +
                                                "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                                "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                                "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                                "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                                 "   UNION ALL " +
                                                "    SELECT areaConf.cnfOrgId, idOrganization = 0 FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + "   AND areaConf.isActive = 1 " +
                                                " ) AS userAreaDealer On " +
                                                " (userAreaDealer.cnfOrgId = tempLoading.cnFOrgid AND temploadingslip.dealerOrgId = userAreaDealer.idOrganization)" +
                                                " Or (userAreaDealer.cnfOrgId = tempLoading.cnFOrgid )" +
                                                " WHERE DAY(tempLoading.createdOn) = " + sysDate.Day + " AND MONTH(tempLoading.createdOn) = " + sysDate.Month + " AND YEAR(tempLoading.createdOn) = " + sysDate.Year + " " + whereCond;
                   

                    string finalAreaConf = " INNER JOIN " +
                                            " ( " +
                                            " SELECT areaConf.cnfOrgId, idOrganization " +
                                            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                            " INNER JOIN " +
                                            " ( " +
                                            "    SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1   ) addrDtl ON idOrganization = organizationId " +
                                            "    INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                            "    WHERE tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                            "   UNION ALL " +
                                            "    SELECT areaConf.cnfOrgId, idOrganization = 0 FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + "   AND areaConf.isActive = 1 " +
                                            " ) AS userAreaDealer On " +
                                            " ( userAreaDealer.cnfOrgId = finalLoading.cnFOrgid AND finalloadingslip.dealerOrgId = userAreaDealer.idOrganization )" +
                                            " Or ( userAreaDealer.cnfOrgId = finalLoading.cnFOrgid)" +
                                            " WHERE DAY(finalLoading.createdOn) = " + sysDate.Day + " AND MONTH(finalLoading.createdOn) = " + sysDate.Month + " AND YEAR(finalLoading.createdOn) = " + sysDate.Year + " " + whereCond;


                    if (loadingType == 1)
                    {
                        
                        cmdSelect.CommandText = " SELECT  totalLoadingQty=0 , " +
                                                " SUM(CASE WHEN tempLoading.statusId IN(7, 14, 15, 16, 17, 20) THEN temploadingslipExt.loadingQty ELSE 0 END) AS confimedLoadingQty " +
                                                " , SUM(CASE WHEN tempLoading.statusId IN(4, 5, 6) THEN temploadingslipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty " +
                                                " , SUM(CASE WHEN tempLoading.statusId IN(17) THEN temploadingslipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                                " FROM temploadingslipExt  " +
                                                " INNER JOIN temploadingslip ON temploadingslip.idLoadingSlip = temploadingslipExt.loadingSlipId " +
                                                " And  ISNULL(temploadingslipExt.prodItemId,0) = 0" +
                                                " INNER JOIN tempLoading ON tempLoading.idLoading = temploadingslip.loadingId " +
                                                //" INNER JOIN tempLoadingSlipDtl ON temploadingslip.idLoadingSlip = tempLoadingSlipDtl.loadingSlipId " +
                                                  areaConfStr +

                                                // Vaibhav [10-Jan-2018] Added to select from finalLoading
                                                " UNION ALL " +

                                                " SELECT  totalLoadingQty = 0, " +
                                                " SUM(CASE WHEN finalLoading.statusId IN(7, 14, 15, 16, 17, 20) THEN loadingQty ELSE 0 END) AS confimedLoadingQty " +
                                                " , SUM(CASE WHEN finalLoading.statusId IN(4, 5, 6) THEN loadingQty ELSE 0 END) AS notconfimedLoadingQty " +
                                                " , SUM(CASE WHEN finalLoading.statusId IN(17) THEN loadingQty ELSE 0 END) AS deliveredQty " +
                                                " FROM finalloadingslipExt " +
                                                 " INNER JOIN finalloadingslip ON finalloadingslip.idLoadingSlip = finalloadingslipExt.loadingSlipId " +
                                                " and ISNULL(finalloadingslipExt.prodItemId,0)= 0 " +
                                                " INNER JOIN finalLoading ON finalLoading.idLoading = finalloadingslip.loadingId " +
                                                //" INNER JOIN finalLoadingSlipDtl ON finalloadingslip.idLoadingSlip = finalLoadingSlipDtl.loadingSlipId " +
                                                  finalAreaConf;
                    }
                    else if (loadingType == 2)
                    {
                        cmdSelect.CommandText = " SELECT  totalLoadingQty = 0 , " +
                                               " SUM(CASE WHEN tempLoading.statusId IN(7, 14, 15, 16, 17, 20) THEN slipExt.loadingQty ELSE 0 END) AS confimedLoadingQty " +
                                               " , SUM(CASE WHEN tempLoading.statusId IN(4, 5, 6) THEN slipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty " +
                                               " , SUM(CASE WHEN tempLoading.statusId IN(17) THEN  slipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                               " FROM tempLoadingSlipExt slipExt  " +
                                               " INNER JOIN temploadingslip ON temploadingslip.idLoadingSlip = slipExt.loadingSlipId " +
                                               " and ISNULL(slipExt.prodItemId,0)!=0 " +
                                               " INNER JOIN tempLoading ON tempLoading.idLoading = temploadingslip.loadingId " +
                                                 areaConfStr +

                                               // Vaibhav [10-Jan-2018] Added to select from finalLoading
                                               " UNION ALL " +

                                               " SELECT totalLoadingQty =0 , " +
                                               " SUM(CASE WHEN finalLoading.statusId IN(7, 14, 15, 16, 17, 20) THEN slipExt.loadingQty ELSE 0 END) AS confimedLoadingQty " +
                                               " , SUM(CASE WHEN finalLoading.statusId IN(4, 5, 6) THEN slipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty " +
                                               " , SUM(CASE WHEN finalLoading.statusId IN(17) THEN slipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                               " FROM finalLoadingSlipExt  slipExt " +
                                               " INNER JOIN finalloadingslip ON finalloadingslip.idLoadingSlip = slipExt.loadingSlipId" +
                                               " and ISNULL(slipExt.prodItemId,0) != 0 " +
                                               " INNER JOIN  finalLoading  ON finalLoading.idLoading = finalloadingslip.loadingId " +
                                               //" INNER JOIN finalLoadingSlipDtl ON finalloadingslip.idLoadingSlip = finalLoadingSlipDtl.loadingSlipId " +
                                                 finalAreaConf;


                    }
                }
                else
                {
                    if (loadingType == 1)
                    {
                        cmdSelect.CommandText = "SELECT SUM(totalLoadingQty) totalLoadingQty,SUM(confimedLoadingQty) confimedLoadingQty " +
                                        " ,SUM(notconfimedLoadingQty) notconfimedLoadingQty,SUM(deliveredQty) deliveredQty " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT ISNULL(totalLoadingQty, 0) totalLoadingQty, ISNULL(confimedLoadingQty, 0) confimedLoadingQty, " +
                                        " ISNULL(notconfimedLoadingQty, 0) notconfimedLoadingQty, ISNULL(deliveredQty, 0) deliveredQty " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT totalLoadingQty=0, " +
                                        " SUM(CASE WHEN slip.statusId IN(7, 14, 15, 16, 17, 20) THEN slipExt.loadingQty ELSE 0 END) AS confimedLoadingQty, " +
                                        " SUM(CASE WHEN slip.statusId IN(4, 5, 6) THEN slipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                                        " SUM(CASE WHEN slip.statusId IN(17) THEN slipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                        " FROM temploadingslipExt  slipExt " +
                                        " INNER JOIN temploadingslip slip ON slip.idLoadingSlip = slipExt.loadingSlipId " +
                                        " and ISNULL(slipExt.prodItemId,0)= 0 " +
                                        " INNER JOIN tempLoading loading ON loading.idLoading = slip.loadingId " +
                                        //" INNER JOIN tempLoadingSlipDtl slipDtl  On slip.idLoadingSlip = slipDtl.loadingSlipId " +
                                        " WHERE DAY(loading.createdOn)= " + sysDate.Day + " AND MONTH(loading.createdOn)= " + sysDate.Month + " AND YEAR(loading.createdOn)= " + sysDate.Year + whereCond +
                                        " ) as tempRes " +

                                        " UNION ALL " +

                                        " SELECT ISNULL(totalLoadingQty, 0) totalLoadingQty, ISNULL(confimedLoadingQty, 0) confimedLoadingQty, " +
                                        " ISNULL(notconfimedLoadingQty, 0) notconfimedLoadingQty, ISNULL(deliveredQty, 0) deliveredQty " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT  totalLoadingQty = 0, " +
                                        " SUM(CASE WHEN slip.statusId IN(7, 14, 15, 16, 17, 20) THEN slipExt.loadingQty ELSE 0 END) AS confimedLoadingQty, " +
                                        " SUM(CASE WHEN slip.statusId IN(4, 5, 6) THEN slipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                                        " SUM(CASE WHEN slip.statusId IN(17) THEN slipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                        " FROM finalloadingslipExt  slipExt " +
                                        " INNER JOIN finalLoadingslip slip ON slip.idLoadingSlip = slipExt.loadingSlipId " +
                                        " and ISNULL(slipExt.prodItemId,0)=0" +
                                        " INNER JOIN finalLoading loading ON loading.idLoading = slip.loadingId " +
                                       // " INNER JOIN finalLoadingSlipDtl slipDtl  On slip.idLoadingSlip = slipDtl.loadingSlipId  " +
                                        " WHERE DAY(loading.createdOn)= " + sysDate.Day + " AND MONTH(loading.createdOn)= " + sysDate.Month + " AND YEAR(loading.createdOn)= " + sysDate.Year + whereCond +
                                        " ) AS finRes " +
                                        " ) AS completeRes";

                        //if (loadingType == (int)Constants.LoadingTypeE.REGULAR)
                        //{
                        //    cmdSelect.CommandText = " SELECT SUM(totalLoadingQty) totalLoadingQty , " +
                        //                       " SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20) THEN totalLoadingQty ELSE 0 END) AS confimedLoadingQty, " +
                        //                       " SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                        //                       " SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END) AS deliveredQty " +
                        //                       " FROM tempLoading " +
                        //                       " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=1"+

                        //                       // Vaibhav [10-Jan-2018] Added to select from finalLoading
                        //                       " UNION ALL " +

                        //                       " SELECT SUM(totalLoadingQty) totalLoadingQty , " +
                        //                       " SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20) THEN totalLoadingQty ELSE 0 END) AS confimedLoadingQty, " +
                        //                       " SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                        //                       " SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END) AS deliveredQty " +
                        //                       " FROM finalLoading " +
                        //                       " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=1";

                        //}
                        //else if (loadingType == (int)Constants.LoadingTypeE.OTHER)
                        //{
                        //    cmdSelect.CommandText = " SELECT SUM(totalLoadingQty) totalLoadingQty , " +
                        //                     " SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20) THEN totalLoadingQty ELSE 0 END) AS confimedLoadingQty, " +
                        //                     " SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                        //                     " SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END) AS deliveredQty " +
                        //                     " FROM tempLoading " +
                        //                     " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=2"+

                        //                     // Vaibhav [10-Jan-2018] Added to select from finalLoading
                        //                     " UNION ALL " +

                        //                     " SELECT SUM(totalLoadingQty) totalLoadingQty , " +
                        //                     " SUM(CASE WHEN statusId IN(7, 14, 15, 16, 17,20) THEN totalLoadingQty ELSE 0 END) AS confimedLoadingQty, " +
                        //                     " SUM(CASE WHEN statusId IN(4, 5, 6) THEN totalLoadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                        //                     " SUM(CASE WHEN statusId IN(17) THEN totalLoadingQty ELSE 0 END) AS deliveredQty " +
                        //                     " FROM finalLoading " +
                        //                     " WHERE DAY(createdOn)= " + sysDate.Day + " AND MONTH(createdOn)= " + sysDate.Month + " AND YEAR(createdOn)= " + sysDate.Year + whereCond + "AND loadingType=2";


                        //}
                    }
                    else if(loadingType==2)
                    {
                        cmdSelect.CommandText = "SELECT SUM(totalLoadingQty) totalLoadingQty,SUM(confimedLoadingQty) confimedLoadingQty " +
                                      " ,SUM(notconfimedLoadingQty) notconfimedLoadingQty,SUM(deliveredQty) deliveredQty " +
                                      " FROM " +
                                      " ( " +
                                      " SELECT  totalLoadingQty=0, ISNULL(confimedLoadingQty, 0) confimedLoadingQty, " +
                                      " ISNULL(notconfimedLoadingQty, 0) notconfimedLoadingQty, ISNULL(deliveredQty, 0) deliveredQty " +
                                      " FROM " +
                                      " ( " +
                                      " SELECT  totalLoadingQty =0, " +
                                      " SUM(CASE WHEN slip.statusId IN(7, 14, 15, 16, 17, 20) THEN slipExt.loadingQty ELSE 0 END) AS confimedLoadingQty, " +
                                      " SUM(CASE WHEN slip.statusId IN(4, 5, 6) THEN slipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                                      " SUM(CASE WHEN slip.statusId IN(17) THEN slipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                      " FROM temploadingslipExt slipExt" +
                                      " INNER JOIN tempLoadingslip slip ON slip.idLoadingSlip = slipExt.loadingSlipId " +
                                      " and ISNULL(slipExt.prodItemId,0) != 0 " +
                                      " INNER JOIN tempLoading  loading   ON loading.idLoading = slip.loadingId " +
                                      " WHERE DAY(loading.createdOn)= " + sysDate.Day + " AND MONTH(loading.createdOn)= " + sysDate.Month + " AND YEAR(loading.createdOn)= " + sysDate.Year + whereCond +
                                      " ) as tempRes " +

                                      " UNION ALL " +

                                      " SELECT ISNULL(totalLoadingQty, 0) totalLoadingQty, ISNULL(confimedLoadingQty, 0) confimedLoadingQty, " +
                                      " ISNULL(notconfimedLoadingQty, 0) notconfimedLoadingQty, ISNULL(deliveredQty, 0) deliveredQty " +
                                      " FROM " +
                                      " ( " +
                                      " SELECT  totalLoadingQty=0, " +
                                      " SUM(CASE WHEN slip.statusId IN(7, 14, 15, 16, 17, 20) THEN slipExt.loadingQty ELSE 0 END) AS confimedLoadingQty, " +
                                      " SUM(CASE WHEN slip.statusId IN(4, 5, 6) THEN slipExt.loadingQty ELSE 0 END) AS notconfimedLoadingQty, " +
                                      " SUM(CASE WHEN slip.statusId IN(17) THEN slipExt.loadingQty ELSE 0 END) AS deliveredQty " +
                                      " FROM finalLoadingSlipExt  slipExt " +
                                      " INNER JOIN finalLoadingSlip slip ON slip.idLoadingSlip = slipExt.loadingSlipId " +
                                      " and ISNULL(slipExt.prodItemId,0)!=0 " +
                                      " INNER JOIN finalLoading loading ON loading.idLoading = slip.loadingId " +
                                      " WHERE DAY(loading.createdOn)= " + sysDate.Day + " AND MONTH(loading.createdOn)= " + sysDate.Month + " AND YEAR(loading.createdOn)= " + sysDate.Year + whereCond +
                                      " ) AS finRes " +
                                      " ) AS completeRes";

                    }

                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {
                    ODLMWebAPI.DashboardModels.LoadingInfo tblLoadingTONew = new ODLMWebAPI.DashboardModels.LoadingInfo();
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());
                    if (tblLoadingTODT["confimedLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalConfirmedLoadingQty = Convert.ToDouble(tblLoadingTODT["confimedLoadingQty"].ToString());
                    if (tblLoadingTODT["notconfimedLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.NotconfimedLoadingQty = Convert.ToDouble(tblLoadingTODT["notconfimedLoadingQty"].ToString());
                    if (tblLoadingTODT["deliveredQty"] != DBNull.Value)
                        tblLoadingTONew.TotalDeliveredQty = Convert.ToDouble(tblLoadingTODT["deliveredQty"].ToString());

                    tblLoadingTONew.TotalPendingQty = tblLoadingTONew.TotalConfirmedLoadingQty - tblLoadingTONew.TotalDeliveredQty;

                    return tblLoadingTONew;
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

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime loadingDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND DAY(loading.createdOn)=" + loadingDate.Day +
                                        " AND MONTH(loading.createdOn) = " + loadingDate.Month + " AND YEAR(loading.createdOn)=" + loadingDate.Year;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'";
                                        
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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


        public List<TblLoadingTO> SelectLoadingTOWithDetailsByLoadingNoForSupport(string loadingSlipNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipNo ='" + loadingSlipNo + "'";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading,int loadingId,SqlConnection conn,SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            String strStatusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            string statusIdsForIn = (int)Constants.TranStatusE.LOADING_GATE_IN + "," + (int)Constants.TranStatusE.LOADING_CONFIRM;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {
                //sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND loading.statusId NOT IN(" + statusIds + ")";
                //Aniket [13-8-2019] Commented for change vehical number condition
                // sqlQuery = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'";
                //           " AND loading.statusId = " + statusIds;
                //Aniket [16-8-2019] Added for change vehical number condition to lodingId
                if (loadingId == 0)
                {
                    sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'"; //sq1.idLoading = 35834--
                }
                else
                {
                    sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.idLoading = " + loadingId;//--
                }

                if (isAllowNxtLoading)
                {
                    sqlQuery += " AND sq1.statusId NOT IN(" + strStatusIds + ")";
                                //" AND loading.isAllowNxtLoading = 0";
                }
                
                else
                {
                    sqlQuery += " AND sq1.statusId IN (" + statusIdsForIn + ")";
                    /*GJ@20170830 : Below Conditions for to check Supervisor is assigned or not*/
                    //sqlQuery += " AND ISNULL(loading.superwisorId ,0)> 0";
                        
                                
                }
                    
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction= tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(string vehicleNo, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            string statusIdsForIn = (int)Constants.TranStatusE.LOADING_COMPLETED + "";
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {
                //sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND loading.statusId NOT IN(" + statusIds + ")";
                sqlQuery = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'"+
                           " AND sq1.statusId = " + (int)Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH + "";



                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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

        //Aniket [19-8-2019] added for IOT
        public List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string sqlQuery = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            string statusIdsForIn = (int)Constants.TranStatusE.LOADING_COMPLETED + "";
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {
                //sqlQuery = SqlSelectQuery() + " WHERE loading.vehicleNo ='" + vehicleNo + "'" + " AND loading.statusId NOT IN(" + statusIds + ")";
                sqlQuery = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.idLoading ='" + loadingId + "'" +
                           " AND sq1.statusId = " + (int)Constants.TranStatusE.LOADING_COMPLETED + "";



                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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


        public List<TblLoadingTO> SelectAllInLoadingListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : START*/
            //String statusIds = (int)Constants.TranStatusE.LOADING_CANCEL + "," + (int)Constants.TranStatusE.LOADING_DELIVERED;
            int statusIds = (int)Constants.TranStatusE.LOADING_GATE_IN;
            /*GJ@20170822 : changes in Status Ids and allow generate loading slip against completed Loading Slips : END*/
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.vehicleNo ='" + vehicleNo + "'" + " AND sq1.statusId = " + statusIds ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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

        public Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisor(DateTime date, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                Dictionary<Int32, Int32> DCT = new Dictionary<int, Int32>();
                cmdSelect.CommandText = " SELECT superwisorId ,COUNT(*) as slipCount FROM tempLoading WHERE statusId =" + (int)Constants.TranStatusE.LOADING_GATE_IN + " AND DAY(updatedOn)=" + date.Day + " AND MONTH(updatedOn)=" + date.Month + " AND YEAR(updatedOn)=" + date.Year +
                                        " GROUP BY superwisorId";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                Int32 slipCount = 0;
                Int32 superwisorId = 0;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (reader["superwisorId"] != DBNull.Value)
                        superwisorId = Convert.ToInt32(reader["superwisorId"].ToString());
                    if (reader["slipCount"] != DBNull.Value)
                        slipCount = Convert.ToInt32(reader["slipCount"].ToString());

                    DCT.Add(superwisorId, slipCount);
                }

                return DCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingTO> ConvertDTToList(SqlDataReader tblLoadingTODT)
        {
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            if (tblLoadingTODT != null)
            {
                while (tblLoadingTODT.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (tblLoadingTODT["idLoading"] != DBNull.Value)
                        tblLoadingTONew.IdLoading = Convert.ToInt32(tblLoadingTODT["idLoading"].ToString());
                    if (tblLoadingTODT["isJointDelivery"] != DBNull.Value)
                        tblLoadingTONew.IsJointDelivery = Convert.ToInt32(tblLoadingTODT["isJointDelivery"].ToString());
                    if (tblLoadingTODT["noOfDeliveries"] != DBNull.Value)
                        tblLoadingTONew.NoOfDeliveries = Convert.ToInt32(tblLoadingTODT["noOfDeliveries"].ToString());
                    if (tblLoadingTODT["statusId"] != DBNull.Value)
                        tblLoadingTONew.StatusId = Convert.ToInt32(tblLoadingTODT["statusId"].ToString());
                    if (tblLoadingTODT["createdBy"] != DBNull.Value)
                        tblLoadingTONew.CreatedBy = Convert.ToInt32(tblLoadingTODT["createdBy"].ToString());
                    if (tblLoadingTODT["updatedBy"] != DBNull.Value)
                        tblLoadingTONew.UpdatedBy = Convert.ToInt32(tblLoadingTODT["updatedBy"].ToString());
                    if (tblLoadingTODT["statusDate"] != DBNull.Value)
                        tblLoadingTONew.StatusDate = Convert.ToDateTime(tblLoadingTODT["statusDate"].ToString());
                    if (tblLoadingTODT["loadingDatetime"] != DBNull.Value)
                        tblLoadingTONew.LoadingDatetime = Convert.ToDateTime(tblLoadingTODT["loadingDatetime"].ToString());
                    if (tblLoadingTODT["createdOn"] != DBNull.Value)
                        tblLoadingTONew.CreatedOn = Convert.ToDateTime(tblLoadingTODT["createdOn"].ToString());
                    if (tblLoadingTODT["updatedOn"] != DBNull.Value)
                        tblLoadingTONew.UpdatedOn = Convert.ToDateTime(tblLoadingTODT["updatedOn"].ToString());
                    if (tblLoadingTODT["loadingSlipNo"] != DBNull.Value)
                        tblLoadingTONew.LoadingSlipNo = Convert.ToString(tblLoadingTODT["loadingSlipNo"].ToString());
                    if (tblLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingTONew.VehicleNo = Convert.ToString(tblLoadingTODT["vehicleNo"].ToString().ToUpper());
                    if (tblLoadingTODT["statusReason"] != DBNull.Value)
                        tblLoadingTONew.StatusReason = Convert.ToString(tblLoadingTODT["statusReason"].ToString());

                    if (tblLoadingTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgId = Convert.ToInt32(tblLoadingTODT["cnfOrgId"].ToString());
                    if (tblLoadingTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgName = Convert.ToString(tblLoadingTODT["cnfOrgName"].ToString());
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());

                    if (tblLoadingTODT["statusName"] != DBNull.Value)
                        tblLoadingTONew.StatusDesc = Convert.ToString(tblLoadingTODT["statusName"].ToString());
                    if (tblLoadingTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingTONew.StatusReasonId = Convert.ToInt32(tblLoadingTODT["statusReasonId"].ToString());
                    if (tblLoadingTODT["transporterOrgId"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgId = Convert.ToInt32(tblLoadingTODT["transporterOrgId"].ToString());
                    if (tblLoadingTODT["freightAmt"] != DBNull.Value)
                        tblLoadingTONew.FreightAmt = Convert.ToDouble(tblLoadingTODT["freightAmt"].ToString());

                    if (tblLoadingTODT["transporterOrgName"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgName = Convert.ToString(tblLoadingTODT["transporterOrgName"].ToString());

                    if (tblLoadingTODT["superwisorId"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorId = Convert.ToInt32(tblLoadingTODT["superwisorId"].ToString());
                    if (tblLoadingTODT["superwisorName"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorName = Convert.ToString(tblLoadingTODT["superwisorName"].ToString());
                    if (tblLoadingTODT["isFreightIncluded"] != DBNull.Value)
                        tblLoadingTONew.IsFreightIncluded = Convert.ToInt32(tblLoadingTODT["isFreightIncluded"].ToString());

                    if (tblLoadingTODT["contactNo"] != DBNull.Value)
                        tblLoadingTONew.ContactNo = Convert.ToString(tblLoadingTODT["contactNo"].ToString());
                    if (tblLoadingTODT["driverName"] != DBNull.Value)
                        tblLoadingTONew.DriverName = Convert.ToString(tblLoadingTODT["driverName"].ToString());

                    //Pandurang [2018-03-21] commented as discussed with Saket(Multiple CnF into single loading and query optimization)
                    //if (tblLoadingTODT["digitalSign"] != DBNull.Value)
                    //    tblLoadingTONew.DigitalSign = Convert.ToString(tblLoadingTODT["digitalSign"].ToString());
                    if (tblLoadingTODT["userDisplayName"] != DBNull.Value)
                        tblLoadingTONew.CreatedByUserName = Convert.ToString(tblLoadingTODT["userDisplayName"].ToString());
                    if (tblLoadingTODT["parentLoadingId"] != DBNull.Value)
                        tblLoadingTONew.ParentLoadingId = Convert.ToInt32(tblLoadingTODT["parentLoadingId"].ToString());
                    if (tblLoadingTODT["callFlag"] != DBNull.Value)
                        tblLoadingTONew.CallFlag = Convert.ToInt32(tblLoadingTODT["callFlag"].ToString());
                    if (tblLoadingTODT["flagUpdatedOn"] != DBNull.Value)
                        tblLoadingTONew.FlagUpdatedOn = Convert.ToDateTime(tblLoadingTODT["flagUpdatedOn"].ToString());
                    if (tblLoadingTODT["isAllowNxtLoading"] != DBNull.Value)
                        tblLoadingTONew.IsAllowNxtLoading = Convert.ToInt32(tblLoadingTODT["isAllowNxtLoading"].ToString());
                    if (tblLoadingTODT["loadingType"] != DBNull.Value)
                        tblLoadingTONew.LoadingType = Convert.ToInt32(tblLoadingTODT["loadingType"]);
                    if (tblLoadingTODT["currencyId"] != DBNull.Value)
                        tblLoadingTONew.CurrencyId = Convert.ToInt32(tblLoadingTODT["currencyId"]);
                    if (tblLoadingTODT["currencyRate"] != DBNull.Value)
                        tblLoadingTONew.CurrencyRate = Convert.ToDouble(tblLoadingTODT["currencyRate"]);

                    if (tblLoadingTODT["maxWeighingOty"] != DBNull.Value)
                        tblLoadingTONew.MaxWeighingOty = Convert.ToDouble(tblLoadingTODT["maxWeighingOty"]);
                    if (tblLoadingTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingTONew.ModbusRefId = Convert.ToInt32(tblLoadingTODT["modbusRefId"]);
                    if (tblLoadingTODT["gateId"] != DBNull.Value)
                        tblLoadingTONew.GateId = Convert.ToInt32(tblLoadingTODT["gateId"]);
                    if (tblLoadingTODT["portNumber"] != DBNull.Value)
                        tblLoadingTONew.PortNumber = Convert.ToString(tblLoadingTODT["portNumber"]);
                    if (tblLoadingTODT["ioTUrl"] != DBNull.Value)
                        tblLoadingTONew.IoTUrl = Convert.ToString(tblLoadingTODT["ioTUrl"]);
                    if (tblLoadingTODT["machineIP"] != DBNull.Value)
                        tblLoadingTONew.MachineIP = Convert.ToString(tblLoadingTODT["machineIP"]);
                    if (tblLoadingTODT["isDBup"] != DBNull.Value)
                        tblLoadingTONew.IsDBup = Convert.ToInt32(tblLoadingTODT["isDBup"]);
                    if (tblLoadingTODT["ignoreGrossWt"] != DBNull.Value)
                        tblLoadingTONew.IgnoreGrossWt = Convert.ToInt32(tblLoadingTODT["ignoreGrossWt"]);
                    if (tblLoadingTODT["fromOrgId"] != DBNull.Value)
                        tblLoadingTONew.FromOrgId = Convert.ToInt32(tblLoadingTODT["fromOrgId"]);
                    //Added by minal 12 may 2021 for show dealer name in approval screen
                    //if (tblLoadingTODT["dealerOrgName"] != DBNull.Value)
                    //    tblLoadingTONew.DealerOrgName = Convert.ToString(tblLoadingTODT["dealerOrgName"]);
                    //Added by minal 12 may 2021
                    tblLoadingTOList.Add(tblLoadingTONew);
                }
            }
            return tblLoadingTOList;
        }

        public List<TblLoadingTO> ConvertDTToListForDealer(SqlDataReader tblLoadingTODT)
        {
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            if (tblLoadingTODT != null)
            {
                while (tblLoadingTODT.Read())
                {
                    TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                    if (tblLoadingTODT["idLoading"] != DBNull.Value)
                        tblLoadingTONew.IdLoading = Convert.ToInt32(tblLoadingTODT["idLoading"].ToString());
                    if (tblLoadingTODT["isJointDelivery"] != DBNull.Value)
                        tblLoadingTONew.IsJointDelivery = Convert.ToInt32(tblLoadingTODT["isJointDelivery"].ToString());
                    if (tblLoadingTODT["noOfDeliveries"] != DBNull.Value)
                        tblLoadingTONew.NoOfDeliveries = Convert.ToInt32(tblLoadingTODT["noOfDeliveries"].ToString());
                    if (tblLoadingTODT["statusId"] != DBNull.Value)
                        tblLoadingTONew.StatusId = Convert.ToInt32(tblLoadingTODT["statusId"].ToString());
                    if (tblLoadingTODT["createdBy"] != DBNull.Value)
                        tblLoadingTONew.CreatedBy = Convert.ToInt32(tblLoadingTODT["createdBy"].ToString());
                    if (tblLoadingTODT["updatedBy"] != DBNull.Value)
                        tblLoadingTONew.UpdatedBy = Convert.ToInt32(tblLoadingTODT["updatedBy"].ToString());
                    if (tblLoadingTODT["statusDate"] != DBNull.Value)
                        tblLoadingTONew.StatusDate = Convert.ToDateTime(tblLoadingTODT["statusDate"].ToString());
                    if (tblLoadingTODT["loadingDatetime"] != DBNull.Value)
                        tblLoadingTONew.LoadingDatetime = Convert.ToDateTime(tblLoadingTODT["loadingDatetime"].ToString());
                    if (tblLoadingTODT["createdOn"] != DBNull.Value)
                        tblLoadingTONew.CreatedOn = Convert.ToDateTime(tblLoadingTODT["createdOn"].ToString());
                    if (tblLoadingTODT["updatedOn"] != DBNull.Value)
                        tblLoadingTONew.UpdatedOn = Convert.ToDateTime(tblLoadingTODT["updatedOn"].ToString());
                    if (tblLoadingTODT["loadingSlipNo"] != DBNull.Value)
                        tblLoadingTONew.LoadingSlipNo = Convert.ToString(tblLoadingTODT["loadingSlipNo"].ToString());
                    if (tblLoadingTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingTONew.VehicleNo = Convert.ToString(tblLoadingTODT["vehicleNo"].ToString().ToUpper());
                    if (tblLoadingTODT["statusReason"] != DBNull.Value)
                        tblLoadingTONew.StatusReason = Convert.ToString(tblLoadingTODT["statusReason"].ToString());

                    if (tblLoadingTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgId = Convert.ToInt32(tblLoadingTODT["cnfOrgId"].ToString());
                    if (tblLoadingTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingTONew.CnfOrgName = Convert.ToString(tblLoadingTODT["cnfOrgName"].ToString());
                    if (tblLoadingTODT["totalLoadingQty"] != DBNull.Value)
                        tblLoadingTONew.TotalLoadingQty = Convert.ToDouble(tblLoadingTODT["totalLoadingQty"].ToString());

                    if (tblLoadingTODT["statusName"] != DBNull.Value)
                        tblLoadingTONew.StatusDesc = Convert.ToString(tblLoadingTODT["statusName"].ToString());
                    if (tblLoadingTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingTONew.StatusReasonId = Convert.ToInt32(tblLoadingTODT["statusReasonId"].ToString());
                    if (tblLoadingTODT["transporterOrgId"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgId = Convert.ToInt32(tblLoadingTODT["transporterOrgId"].ToString());
                    if (tblLoadingTODT["freightAmt"] != DBNull.Value)
                        tblLoadingTONew.FreightAmt = Convert.ToDouble(tblLoadingTODT["freightAmt"].ToString());

                    if (tblLoadingTODT["transporterOrgName"] != DBNull.Value)
                        tblLoadingTONew.TransporterOrgName = Convert.ToString(tblLoadingTODT["transporterOrgName"].ToString());

                    if (tblLoadingTODT["superwisorId"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorId = Convert.ToInt32(tblLoadingTODT["superwisorId"].ToString());
                    if (tblLoadingTODT["superwisorName"] != DBNull.Value)
                        tblLoadingTONew.SuperwisorName = Convert.ToString(tblLoadingTODT["superwisorName"].ToString());
                    if (tblLoadingTODT["isFreightIncluded"] != DBNull.Value)
                        tblLoadingTONew.IsFreightIncluded = Convert.ToInt32(tblLoadingTODT["isFreightIncluded"].ToString());

                    if (tblLoadingTODT["contactNo"] != DBNull.Value)
                        tblLoadingTONew.ContactNo = Convert.ToString(tblLoadingTODT["contactNo"].ToString());
                    if (tblLoadingTODT["driverName"] != DBNull.Value)
                        tblLoadingTONew.DriverName = Convert.ToString(tblLoadingTODT["driverName"].ToString());

                    //Pandurang [2018-03-21] commented as discussed with Saket(Multiple CnF into single loading and query optimization)
                    //if (tblLoadingTODT["digitalSign"] != DBNull.Value)
                    //    tblLoadingTONew.DigitalSign = Convert.ToString(tblLoadingTODT["digitalSign"].ToString());
                    if (tblLoadingTODT["userDisplayName"] != DBNull.Value)
                        tblLoadingTONew.CreatedByUserName = Convert.ToString(tblLoadingTODT["userDisplayName"].ToString());
                    if (tblLoadingTODT["parentLoadingId"] != DBNull.Value)
                        tblLoadingTONew.ParentLoadingId = Convert.ToInt32(tblLoadingTODT["parentLoadingId"].ToString());
                    if (tblLoadingTODT["callFlag"] != DBNull.Value)
                        tblLoadingTONew.CallFlag = Convert.ToInt32(tblLoadingTODT["callFlag"].ToString());
                    if (tblLoadingTODT["flagUpdatedOn"] != DBNull.Value)
                        tblLoadingTONew.FlagUpdatedOn = Convert.ToDateTime(tblLoadingTODT["flagUpdatedOn"].ToString());
                    if (tblLoadingTODT["isAllowNxtLoading"] != DBNull.Value)
                        tblLoadingTONew.IsAllowNxtLoading = Convert.ToInt32(tblLoadingTODT["isAllowNxtLoading"].ToString());
                    if (tblLoadingTODT["loadingType"] != DBNull.Value)
                        tblLoadingTONew.LoadingType = Convert.ToInt32(tblLoadingTODT["loadingType"]);
                    if (tblLoadingTODT["currencyId"] != DBNull.Value)
                        tblLoadingTONew.CurrencyId = Convert.ToInt32(tblLoadingTODT["currencyId"]);
                    if (tblLoadingTODT["currencyRate"] != DBNull.Value)
                        tblLoadingTONew.CurrencyRate = Convert.ToDouble(tblLoadingTODT["currencyRate"]);

                    if (tblLoadingTODT["maxWeighingOty"] != DBNull.Value)
                        tblLoadingTONew.MaxWeighingOty = Convert.ToDouble(tblLoadingTODT["maxWeighingOty"]);
                    if (tblLoadingTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingTONew.ModbusRefId = Convert.ToInt32(tblLoadingTODT["modbusRefId"]);
                    if (tblLoadingTODT["gateId"] != DBNull.Value)
                        tblLoadingTONew.GateId = Convert.ToInt32(tblLoadingTODT["gateId"]);
                    if (tblLoadingTODT["portNumber"] != DBNull.Value)
                        tblLoadingTONew.PortNumber = Convert.ToString(tblLoadingTODT["portNumber"]);
                    if (tblLoadingTODT["ioTUrl"] != DBNull.Value)
                        tblLoadingTONew.IoTUrl = Convert.ToString(tblLoadingTODT["ioTUrl"]);
                    if (tblLoadingTODT["machineIP"] != DBNull.Value)
                        tblLoadingTONew.MachineIP = Convert.ToString(tblLoadingTODT["machineIP"]);
                    if (tblLoadingTODT["isDBup"] != DBNull.Value)
                        tblLoadingTONew.IsDBup = Convert.ToInt32(tblLoadingTODT["isDBup"]);
                    if (tblLoadingTODT["ignoreGrossWt"] != DBNull.Value)
                        tblLoadingTONew.IgnoreGrossWt = Convert.ToInt32(tblLoadingTODT["ignoreGrossWt"]);
                    if (tblLoadingTODT["fromOrgId"] != DBNull.Value)
                        tblLoadingTONew.FromOrgId = Convert.ToInt32(tblLoadingTODT["fromOrgId"]);
                    //Added by minal 12 may 2021 for show dealer name in approval screen
                    if (tblLoadingTODT["dealerOrgName"] != DBNull.Value)
                        tblLoadingTONew.DealerOrgName = Convert.ToString(tblLoadingTODT["dealerOrgName"]);
                    //Added by minal 12 may 2021
                    tblLoadingTOList.Add(tblLoadingTONew);
                }
            }
            return tblLoadingTOList;
        }

        public List<VehicleNumber> SelectAllVehicles()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            int isHideSuggestion = 0;
            TblConfigParamsTO tblConfigParamTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_HIDE_VEHICLE_LIST_SUGGESTION);
            if(tblConfigParamTO!=null)
            {
                if(tblConfigParamTO.ConfigParamVal=="1")
                {
                    isHideSuggestion = 1;
                }
            }

            SqlCommand cmdSelect = new SqlCommand();
            if(isHideSuggestion==0)
            {
                try
                {
                    conn.Open();
                    cmdSelect.CommandText = "SELECT distinct vehicleNo FROM tempLoading";
                    cmdSelect.Connection = conn;
                    cmdSelect.CommandType = System.Data.CommandType.Text;

                    sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                    List<VehicleNumber> list = new List<VehicleNumber>();
                    if (sqlReader != null)
                    {
                        while (sqlReader.Read())
                        {
                            String vehicleNo = string.Empty;
                            if (sqlReader["vehicleNo"] != DBNull.Value)
                                vehicleNo = Convert.ToString(sqlReader["vehicleNo"].ToString());

                            if (!string.IsNullOrEmpty(vehicleNo))
                            {
                                String[] vehNoPart = vehicleNo.Split(' ');
                                if (vehNoPart.Length == 4)
                                {
                                    VehicleNumber vehicleNumber = new VehicleNumber();
                                    for (int i = 0; i < vehNoPart.Length; i++)
                                    {
                                        if (i == 0)
                                        {
                                            vehicleNumber.StateCode = vehNoPart[i].ToUpper();
                                        }
                                        if (i == 1)
                                        {
                                            vehicleNumber.DistrictCode = vehNoPart[i].ToUpper();
                                        }
                                        if (i == 2)
                                        {
                                            vehicleNumber.UniqueLetters = vehNoPart[i];
                                            if (vehicleNumber.UniqueLetters == "undefined")
                                                vehicleNumber.UniqueLetters = "";
                                            else
                                                vehicleNumber.UniqueLetters = vehicleNumber.UniqueLetters.ToUpper();
                                        }
                                        if (i == 3)
                                        {
                                            if (Constants.IsInteger(vehNoPart[i]))
                                            {
                                                vehicleNumber.VehicleNo = Convert.ToInt32(vehNoPart[i]);
                                            }
                                            else break;
                                        }
                                    }

                                    if (vehicleNumber.VehicleNo > 0)
                                        list.Add(vehicleNumber);
                                }
                            }
                        }
                    }

                    return list;
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    sqlReader.Dispose();
                    conn.Close();
                    cmdSelect.Dispose();
                }
            }
           else
            {
                return null;
            }
        }

        public List<DropDownTO> SelectAllVehiclesListByStatus(int statusId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav [15-Sep-2017] Added condition for displaying new unloading records.
                if (statusId != (int)Constants.TranStatusE.UNLOADING_NEW)
                {
                    //cmdSelect.CommandText = "SELECT idLoading, vehicleNo FROM tblLoading WHERE statusId =" + statusId;
                    cmdSelect.CommandText = "SELECT idLoading, vehicleNo FROM tempLoading WHERE statusId = " + (int)Constants.TranStatusE.LOADING_GATE_IN +
                                            " AND ISNULL(isAllowNxtLoading,0) = 0" +
                                            " UNION ALL " +
                                            " SELECT distinct tbl.idLoading, tbl.vehicleNo from tempLoading tbl" +
                                            " LEFT OUTER JOIN tempLoading tbl1 ON tbl.vehicleNo = tbl1.vehicleNo" +
                                            //" AND tbl1.statusId =" + (int)Constants.TranStatusE.LOADING_COMPLETED + 
                                            " AND tbl1.isAllowNxtLoading = 1 " +
                                            " AND tbl.statusId =" + (int)Constants.TranStatusE.LOADING_CONFIRM +
                                            " WHERE tbl1.vehicleNo IS NOT NULL" +
                                            " AND ISNULL(tbl.isAllowNxtLoading,0) = 0";
                    //cmdSelect.CommandText = "SELECT * FROM (SELECT idLoading, vehicleNo , superwisorId FROM tblLoading " +
                    //                         "WHERE statusId = 15 AND ISNULL(isAllowNxtLoading,0) = 0 " +
                    //                         "UNION ALL SELECT tbl.idLoading, tbl.vehicleNo, tbl.superwisorId " +
                    //                         "FROM tblLoading tbl LEFT OUTER JOIN tblLoading tbl1 ON tbl.vehicleNo = tbl1.vehicleNo " +
                    //                         "AND tbl1.isAllowNxtLoading = 1  " +
                    //                         "AND tbl.statusId ="+ (int)Constants.TranStatusE.LOADING_CONFIRM + "WHERE tbl1.vehicleNo IS NOT NULL) as b " +
                    //                         "where 1 = 1 AND ISNULL(superwisorId ,0)> 0";
                }
                else
                {
                    cmdSelect.CommandText = " SELECT vehicleNo,idUnLoading AS 'idLoading' ,org.firmName AS orgName FROM tblUnLoading tblunload " +
                                            " INNER JOIN tblOrganization org ON org.idOrganization=tblunload.SupplierOrgId " +
                                            " WHERE statusId = " + (int)Constants.TranStatusE.UNLOADING_NEW + " ORDER BY tblunload.createdOn DESC";
                }
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> list = new List<DropDownTO>();
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        DropDownTO dropDownTo = new DropDownTO();
                        String vehicleNo = string.Empty;
                        if (sqlReader["vehicleNo"] != DBNull.Value)
                            dropDownTo.Text = Convert.ToString(sqlReader["vehicleNo"].ToString());
                        if (sqlReader["idLoading"] != DBNull.Value)
                            dropDownTo.Value = Convert.ToInt32(sqlReader["idLoading"].ToString());
                        if ((sqlReader.FieldCount > 2))
                            {
                            if (sqlReader["orgName"] != DBNull.Value)
                                dropDownTo.Tag = sqlReader["orgName"].ToString();
                            }
                        list.Add(dropDownTo);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        // Vaibhav [08-Jan-2018] Added to select all temp loading details.
        public List<TblLoadingTO> SelectAllTempLoading(SqlConnection conn, SqlTransaction tran, DateTime migrateBeforeDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                String sqlQuery = " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ," +
                                  " dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,createdUser.userDisplayName " +
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +                                  
                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy WHERE loading.statusId IN " +
                                  " ( " + (int)Constants.TranStatusE.LOADING_DELIVERED + "," + (int)Constants.TranStatusE.LOADING_CANCEL + ")" +
                                  " AND  CONVERT (DATE,statusDate,103) <= @StatusDate " +
                                  " ORDER BY idloading ASC ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = migrateBeforeDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
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
        //Pandurang[2018-09-25]Added to select all temp loading on status details.
        public List<TblLoadingTO> SelectAllTempLoadingOnStatus(SqlConnection conn, SqlTransaction tran, DateTime migrateBeforeDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                String sqlQuery = " SELECT loading.* ,org.digitalSign, org.firmName as cnfOrgName,transOrg.firmName as transporterOrgName ," +
                                  " dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,createdUser.userDisplayName " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                  " FROM tempLoading loading " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = loading.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate=loading.gateId " +
                                  " LEFT JOIN tblUser createdUser ON createdUser.idUser=loading.createdBy WHERE loading.statusId IN " +
                                  " ( " + (int)Constants.TranStatusE.LOADING_COMPLETED + "," + (int)Constants.TranStatusE.LOADING_DELIVERED + "," + (int)Constants.TranStatusE.LOADING_CANCEL + ")" +
                                  " AND  CONVERT (DATE,statusDate,103) <= @StatusDate " +
                                  " ORDER BY idloading ASC ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@StatusDate", SqlDbType.DateTime).Value = migrateBeforeDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
                return list;
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

        /// <summary>
        /// /// Vijaymala [17-04-2018]:added to get loading list by vehicle number
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public List<TblLoadingTO> SelectLoadingListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loading.vehicleNo ='" + vehicleNo ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingTO> list = ConvertDTToList(sqlReader);
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

  
        #endregion

        #region Insertion
        public int InsertTblLoading(TblLoadingTO tblLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingTO, cmdInsert);
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

        public int InsertTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoading]( " +
                                "  [isJointDelivery]" +
                                " ,[noOfDeliveries]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[statusDate]" +
                                " ,[loadingDatetime]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[loadingSlipNo]" +
                                " ,[vehicleNo]" +
                                " ,[statusReason]" +
                                " ,[cnfOrgId]" +
                                " ,[totalLoadingQty]" +
                                " ,[statusReasonId]" +
                                " ,[transporterOrgId]" +
                                " ,[freightAmt]" +
                                " ,[superwisorId]" +
                                " ,[isFreightIncluded]" +
                                " ,[contactNo]" +
                                " ,[driverName]" +
                                " ,[parentLoadingId]" +
                                " ,[callFlag]" +
                                " ,[flagUpdatedOn]" +
                                " ,[isAllowNxtLoading]" +
                                " ,[loadingType]" +
                                " ,[currencyId]" +
                                " ,[currencyRate]" +
                                " ,[maxWeighingOty]" +
                                  ",[modbusRefId]" +
                                ",[gateId]" +
                                ",[ignoreGrossWt]" +
                                " ,[fromOrgId]" +
                                " )" +
                    " VALUES (" +
                                "  @IsJointDelivery " +
                                " ,@NoOfDeliveries " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@StatusDate " +
                                " ,@LoadingDatetime " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@LoadingSlipNo " +
                                " ,@VehicleNo " +
                                " ,@StatusReason " +
                                " ,@cnfOrgId " +
                                " ,@totalLoadingQty " +
                                " ,@statusReasonId " +
                                " ,@transporterOrgId " +
                                " ,@freightAmt " +
                                " ,@superwisorId " +
                                " ,@isFreightIncluded " +
                                " ,@contactNo " +
                                " ,@driverName " +
                                " ,@parentLoadingId " +
                                " ,@callFlag " +
                                " ,@flagUpdatedOn " +
                                " ,@isAllowNxtLoading " +
                                " ,@loadingType " +
                                " ,@currencyId " +
                                " ,@currencyRate " +
                                " ,@maxWeighingOty"+
                                  " ,@ModbusRefId" +
                                " ,@GateId" +
                                " ,@IgnoreGrossWt" +
                                " ,@fromOrgId " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.UpdatedBy);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
            cmdInsert.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.UpdatedOn);
            cmdInsert.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.VarChar).Value = tblLoadingTO.LoadingSlipNo;
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.VehicleNo); //Saket [2017-12-21] Added GetSqlDataValueNullForBaseValue.
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
            cmdInsert.Parameters.Add("@cnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CnfOrgId);
            cmdInsert.Parameters.Add("@totalLoadingQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TotalLoadingQty);
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
            cmdInsert.Parameters.Add("@transporterOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TransporterOrgId);
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FreightAmt);
            cmdInsert.Parameters.Add("@superwisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.SuperwisorId);
            cmdInsert.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsFreightIncluded);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.DriverName);
            cmdInsert.Parameters.Add("@parentLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ParentLoadingId);
            cmdInsert.Parameters.Add("@callFlag", System.Data.SqlDbType.Int).Value = tblLoadingTO.CallFlag;
            cmdInsert.Parameters.Add("@flagUpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FlagUpdatedOn);
            cmdInsert.Parameters.Add("@isAllowNxtLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsAllowNxtLoading;
            cmdInsert.Parameters.Add("@loadingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingType);
            cmdInsert.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyId);
            cmdInsert.Parameters.Add("@currencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyRate);

            cmdInsert.Parameters.Add("@maxWeighingOty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.MaxWeighingOty);
            cmdInsert.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ModbusRefId);
            cmdInsert.Parameters.Add("@GateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.GateId);
            cmdInsert.Parameters.Add("@IgnoreGrossWt", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IgnoreGrossWt);
            cmdInsert.Parameters.Add("@fromOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FromOrgId);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingTO.IdLoading = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation

        public int UpdateTblLoadingIgnoreGrossWTFlag(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                //return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
                cmdUpdate.CommandText = "UPDATE tempLoading SET " +
                                        "[ignoreGrossWt]=@IgnoreGrossWt" +
                                        " WHERE [idLoading] = @IdLoading ";
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@IgnoreGrossWt", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IgnoreGrossWt);

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


        public int UpdateTblLoading(TblLoadingTO tblLoadingTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
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

        public int UpdateTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
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

        public int updateLaodingToCallFlag(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoading] SET " +
                            "  [isJointDelivery]= @IsJointDelivery" +
                            " ,[noOfDeliveries]= @NoOfDeliveries" +
                            " ,[statusId]= @StatusId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[loadingSlipNo]= @LoadingSlipNo" +
                            " ,[vehicleNo]= @VehicleNo" +
                            " ,[statusReason] = @StatusReason" +
                            " ,[statusReasonId] = @statusReasonId" +
                            " ,[totalLoadingQty] = @totalLoadingQty" +
                            " ,[transporterOrgId] = @transporterOrgId" +
                            " ,[freightAmt] = @freightAmt" +
                            " ,[superwisorId] = @superwisorId" +
                            " ,[isFreightIncluded] = @isFreightIncluded" +
                            " ,[contactNo] = @contactNo" +
                            " ,[driverName] = @driverName" +
                            " ,[parentLoadingId] = @parentLoadingId" +
                            " ,[callFlag] = @callFlag" +
                            " ,[flagUpdatedOn] = @flagUpdatedOn" +
                            " ,[isAllowNxtLoading] = @isAllowNxtLoading" +
                            " ,[loadingType] = @loadingType " +
                            " ,[currencyId] = @currencyId " +
                            " ,[currencyRate] = @currencyRate " +
                            ",[modbusRefId]=@ModbusRefId" +
                            ",[gateId]=@GateId" +
                            ",[isDBup]=@IsDBup" +
                            ",[ignoreGrossWt]=@IgnoreGrossWt" +
                            " WHERE [idLoading] = @IdLoading ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            cmdUpdate.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsJointDelivery;
            cmdUpdate.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingTO.NoOfDeliveries;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblLoadingTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
            cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@LoadingSlipNo", System.Data.SqlDbType.VarChar).Value = tblLoadingTO.LoadingSlipNo;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.VehicleNo);
            cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
            cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReasonId);
            cmdUpdate.Parameters.Add("@totalLoadingQty", System.Data.SqlDbType.Decimal).Value = tblLoadingTO.TotalLoadingQty;
            cmdUpdate.Parameters.Add("@transporterOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.TransporterOrgId);
            cmdUpdate.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FreightAmt);
            cmdUpdate.Parameters.Add("@superwisorId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.SuperwisorId);
            cmdUpdate.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsFreightIncluded);
            cmdUpdate.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ContactNo);
            cmdUpdate.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.DriverName);
            cmdUpdate.Parameters.Add("@parentLoadingId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ParentLoadingId);
            cmdUpdate.Parameters.Add("@callFlag", System.Data.SqlDbType.Int).Value = tblLoadingTO.CallFlag;
            cmdUpdate.Parameters.Add("@flagUpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.FlagUpdatedOn);
            cmdUpdate.Parameters.Add("@isAllowNxtLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IsAllowNxtLoading;
            cmdUpdate.Parameters.Add("@loadingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingType);
            cmdUpdate.Parameters.Add("@currencyId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyId);
            cmdUpdate.Parameters.Add("@currencyRate", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.CurrencyRate);
            cmdUpdate.Parameters.Add("@ModbusRefId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.ModbusRefId);
            cmdUpdate.Parameters.Add("@GateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.GateId);
            cmdUpdate.Parameters.Add("@IsDBup", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IsDBup);
            cmdUpdate.Parameters.Add("@IgnoreGrossWt", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.IgnoreGrossWt);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblLoading(Int32 idLoading)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoading, cmdDelete);
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

        public int DeleteTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoading, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idLoading, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = " DELETE FROM [tempLoading] " +
                                    " WHERE idLoading = " + idLoading + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoading", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
