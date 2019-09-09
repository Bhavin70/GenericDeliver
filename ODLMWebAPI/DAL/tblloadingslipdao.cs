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
    public class TblLoadingSlipDAO : ITblLoadingSlipDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblLoadingSlipDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tempLoadingSlip.* ,tempLoadSlipdtl.loadingQty, " +
                                  " cnfOrg.firmName as cnfOrgName ,dimStat.statusName ,tempLoadSlipdtl.bookingId,tblOrganization.firmName+', '+ CASE WHEN cnfOrg.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL  THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerOrgName" +
                                  " ,loading.modbusRefId,loading.gateId,loading.isDBup,gate.portNumber,gate.IoTUrl,gate.machineIP "+
                                  " FROM tempLoadingSlip" +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = tempLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId " +
                                   " Left Join tempLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = tempLoadingSlip.idLoadingSlip " +
                                   "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId" +   //Aniket K [03-Jan-2019] Added to show village name against dealer
                                   " LEFT JOIN temploading loading on loading.idloading = tempLoadingSlip.loadingId "+
                                   " LEFT JOIN tblGate gate on gate.idGate=loading.gateId "+

                                   // Vaibhav [09-Jan-2018] Added to select from  finalLoadingSlip

            " UNION ALL " +

                                  " SELECT finalLoadingSlip.* , tempLoadSlipdtl.loadingQty, " +
                                  " cnfOrg.firmName as cnfOrgName, dimStat.statusName, tempLoadSlipdtl.bookingId,tblOrganization.firmName+', '+ CASE WHEN cnfOrg.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL  THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerOrgName" +
                                  " , loading.modbusRefId,loading.gateId,loading.isDBup,gate.portNumber,gate.IoTUrl,gate.machineIP "+
                                  " FROM finalLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = finalLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg" +
                                  " ON cnfOrg.idOrganization = finalLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = finalLoadingSlip.statusId " +
                                   " Left Join finalLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = finalLoadingSlip.idLoadingSlip " +
                                   "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId " +  //Aniket K [03-Jan-2019] Added to show village name against dealer
                                    " LEFT JOIN temploading loading on loading.idloading = finalLoadingSlip.loadingId " +
                                   " LEFT JOIN tblGate gate on gate.idGate=loading.gateId ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection
        //Aniket [22-8-2019] added for IoT, to get PortNumber and machine IP
        public  Dictionary<Int32, TblLoadingTO> SelectModbusRefIdByLoadingSlipIdDCT(string loadingSlipNos)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader tblLoadingSlipTODT = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                String sqlSelectQry = " SELECT idLoadingSlip, modbusRefId,gateId, tblgate.iotUrl,tblgate.portnumber,tblgate.machineIP FROM temploadingslip " +
                              " INNER JOIN tempLoading tempLoading ON idLoading = loadingId " +
                              "  left join tblgate tblgate on tblgate.idGate = tempLoading.gateId " +
                              "   WHERE idLoadingSlip IN(" + loadingSlipNos + ") AND modbusRefId IS NOT NULL";

                cmdSelect.CommandText = sqlSelectQry;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblLoadingSlipTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //   Dictionary<int, int> DCT = new Dictionary<int, int>();
                //  List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();

                Dictionary<Int32, TblLoadingTO> tblLoadingTODct = new Dictionary<int, TblLoadingTO>();

                //TblLoadingTO tblLoadingTO = new TblLoadingTO();
                if (tblLoadingSlipTODT != null)
                {
                    while (tblLoadingSlipTODT.Read())
                    {
                        TblLoadingTO tblLoadingTO = new TblLoadingTO();
                        int loadingslipId = 0, modRefId = 0;
                        if (tblLoadingSlipTODT["idLoadingSlip"] != DBNull.Value)
                            loadingslipId = Convert.ToInt32(tblLoadingSlipTODT["idLoadingSlip"].ToString());
                        if (tblLoadingSlipTODT["modbusRefId"] != DBNull.Value)
                            tblLoadingTO.ModbusRefId = Convert.ToInt32(tblLoadingSlipTODT["modbusRefId"].ToString());
                        if (tblLoadingSlipTODT["iotUrl"] != DBNull.Value)
                            tblLoadingTO.IoTUrl = tblLoadingSlipTODT["iotUrl"].ToString();
                        if (tblLoadingSlipTODT["portnumber"] != DBNull.Value)
                            tblLoadingTO.PortNumber = tblLoadingSlipTODT["portnumber"].ToString();
                        if (tblLoadingSlipTODT["machineIP"] != DBNull.Value)
                            tblLoadingTO.MachineIP = tblLoadingSlipTODT["machineIP"].ToString();
                        if (tblLoadingSlipTODT["gateId"] != DBNull.Value)
                            tblLoadingTO.GateId = Convert.ToInt32(tblLoadingSlipTODT["gateId"].ToString());

                        if (!tblLoadingTODct.ContainsKey(loadingslipId))
                        {
                            tblLoadingTODct.Add(loadingslipId, tblLoadingTO);
                        }


                    }
                }
                //return list;
                return tblLoadingTODct;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblLoadingSlipTODT != null)
                    tblLoadingSlipTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public List<TblLoadingSlipTO> SelectAllTblLoadingSlip()
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
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingSlipTO> SelectAllTblLoadingSlip(int loadingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingId=" + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingSlipTO> SelectAllTblLoadingSlip(int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE loadingId=" + loadingId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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

        public TblLoadingSlipTO SelectTblLoadingSlip(Int32 idLoadingSlip)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoadingSlip = " + idLoadingSlip + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
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

        public TblLoadingSlipTO SelectTblLoadingSlip(int idLoadingSlip, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idLoadingSlip = " + idLoadingSlip + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
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
                cmdSelect.Dispose();
            }
        }

        public Dictionary<int, string> SelectRegMobileNoDCTForLoadingDealers(String loadingIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            Dictionary<int, string> DCT = null;
            try
            {
                cmdSelect.CommandText = " SELECT distinct dealerOrgId,registeredMobileNos FROM tempLoadingSlip slips " +
                                        " INNER JOIN tblOrganization org ON slips.dealerOrgId = org.idOrganization " +
                                        " WHERE loadingId IN(" + loadingIds + ") " +

                                        // Vaibhav [10-Jan-2018] Added to select form finalLoadingSlip
                                        " UNION ALL " +

                                        " SELECT distinct dealerOrgId,registeredMobileNos FROM finalLoadingSlip slips " +
                                        " INNER JOIN tblOrganization org ON slips.dealerOrgId = org.idOrganization " +
                                        " WHERE loadingId IN(" + loadingIds + ") ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (rdr != null)
                {
                    DCT = new Dictionary<int, string>();
                    while (rdr.Read())
                    {
                        Int32 orgId = 0;
                        string regMobNos = string.Empty;
                        if (rdr["dealerOrgId"] != DBNull.Value)
                            orgId = Convert.ToInt32(rdr["dealerOrgId"].ToString());
                        if (rdr["registeredMobileNos"] != DBNull.Value)
                            regMobNos = Convert.ToString(rdr["registeredMobileNos"].ToString());

                        if (orgId > 0 && !string.IsNullOrEmpty(regMobNos))
                        {
                            DCT.Add(orgId, regMobNos);
                        }
                    }

                    return DCT;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }


        public Int64 SelectCountOfLoadingSlips(DateTime date, Int32 isConfirmed, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT COUNT(*) as slipCount FROM tempLoadingSlip WHERE ISNULL(isConfirmed,0)=" + isConfirmed + " AND DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year;

                // Vaibhav [20-Nov-2017] comment and added to select from finalLoadingSlip
                cmdSelect.CommandText = " SELECT (SELECT COUNT(*) FROM tempLoadingSlip WHERE ISNULL(isConfirmed,0)=" + isConfirmed + " AND DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + ")" +
                                        " + (SELECT COUNT(*) FROM finalLoadingSlip WHERE ISNULL(isConfirmed,0)=" + isConfirmed + " AND DAY(createdOn)=" + date.Day + " AND MONTH(createdOn)=" + date.Month + " AND YEAR(createdOn)=" + date.Year + ") AS slipCount";

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

        //Sudhir[27-02-2018]
        public List<TblLoadingSlipTO> SelectAllTblLoadingSlipByDate(DateTime fromDate,DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            //vijaymala[04-04-2018]modify the code to display records acc to role
            int isConfEn = 0;
            int userId = 0;
            string areConfJoin = string.Empty;
            string cnfwhereCond = string.Empty;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {
                //vijaymala[04-04-2018]modify the code to display records acc to role

                String sqlSelectQry = " SELECT tempLoadingSlip.* , cnfOrg.firmName as cnfOrgName , tempLoadSlipdtl.loadingQty, dimStat.statusName,tempLoadSlipdtl.bookingId,tblOrganization.firmName+', '+ CASE WHEN cnfOrg.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerOrgName " +
                                  
                                  " FROM tempLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = tempLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId " +
                                  " Left Join tempLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = tempLoadingSlip.idLoadingSlip " +
                                  "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId" +
                                  // Vaibhav [09-Jan-2018] Added to select from  finalLoadingSlip

                                  " UNION ALL " +

                                  " SELECT finalLoadingSlip.* , cnfOrg.firmName as cnfOrgName,  tempLoadSlipdtl.loadingQty,dimStat.statusName, tempLoadSlipdtl.bookingId,tblOrganization.firmName+', '+ CASE WHEN cnfOrg.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerOrgName " +
                                  
                                  " FROM finalLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = finalLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg" +
                                  " ON cnfOrg.idOrganization = finalLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = finalLoadingSlip.statusId "+
                                  " Left Join finalLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = finalLoadingSlip.idLoadingSlip "+
                                  "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId";

                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                               " ( " +
                               "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                               "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                               "   INNER JOIN " +
                               "   ( " +
                               "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               "  ) addrDtl  ON idOrganization = organizationId " +
                               "   INNER JOIN tblUserAreaAllocation areaConf ON " +
                               " ( addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId and  areaConf.isActive=1  ) " +
                               " Or  (areaConf.cnfOrgId=tblCnfDealers.cnfOrgId and Isnull(areaConf.districtId,0)=0 and  areaConf.isActive=1  )   " +
                               "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                               " ) AS userAreaDealer On " +
                               "(userAreaDealer.cnfOrgId = sq1.cnfOrgId AND sq1.dealerOrgId = userAreaDealer.idOrganization)";
                          
                    //  areConfJoin = " INNER JOIN ( SELECT DISTINCT cnfOrgId FROM tblUserAreaAllocation WHERE isActive=1 AND userId=" + userId + ") areaConf ON  areaConf.cnfOrgId = sq1.cnfOrgId ";
                }




                string whereCond = " WHERE CAST(sq1.statusDate AS DATE)  BETWEEN @fromDate AND @toDate";

                if (cnfId > 0)
                    cnfwhereCond = whereCond + " AND sq1.cnfOrgId=" + cnfId;
                else
                    cnfwhereCond = whereCond;

               
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + sqlSelectQry + ")sq1 " + areConfJoin + cnfwhereCond;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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

        //Sudhir
        public List<TblLoadingSlipTO> SelectAllLoadingSlipListByVehicleNo(string vehicleNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE vehicleNo LIKE '%" + vehicleNo + "%' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
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

        //Sudhir
        public List<TblLoadingSlipTO> SelectTblLoadingTOByLoadingSlipIdForSupport(String loadingSlipNo)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipNo LIKE '%" + loadingSlipNo + "%' ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
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

        public List<TblLoadingSlipTO> ConvertDTToList(SqlDataReader tblLoadingSlipTODT)
        {
            List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();
            if (tblLoadingSlipTODT != null)
            {
                while (tblLoadingSlipTODT.Read())
                {
                    TblLoadingSlipTO tblLoadingSlipTONew = new TblLoadingSlipTO();
                    if (tblLoadingSlipTODT["idLoadingSlip"] != DBNull.Value)
                        tblLoadingSlipTONew.IdLoadingSlip = Convert.ToInt32(tblLoadingSlipTODT["idLoadingSlip"].ToString());
                    if (tblLoadingSlipTODT["dealerOrgId"] != DBNull.Value)
                        tblLoadingSlipTONew.DealerOrgId = Convert.ToInt32(tblLoadingSlipTODT["dealerOrgId"].ToString());
                    if (tblLoadingSlipTODT["isJointDelivery"] != DBNull.Value)
                        tblLoadingSlipTONew.IsJointDelivery = Convert.ToInt32(tblLoadingSlipTODT["isJointDelivery"].ToString());
                    if (tblLoadingSlipTODT["noOfDeliveries"] != DBNull.Value)
                        tblLoadingSlipTONew.NoOfDeliveries = Convert.ToInt32(tblLoadingSlipTODT["noOfDeliveries"].ToString());
                    if (tblLoadingSlipTODT["statusId"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusId = Convert.ToInt32(tblLoadingSlipTODT["statusId"].ToString());
                    if (tblLoadingSlipTODT["createdBy"] != DBNull.Value)
                        tblLoadingSlipTONew.CreatedBy = Convert.ToInt32(tblLoadingSlipTODT["createdBy"].ToString());
                    if (tblLoadingSlipTODT["statusDate"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusDate = Convert.ToDateTime(tblLoadingSlipTODT["statusDate"].ToString());
                    if (tblLoadingSlipTODT["loadingDatetime"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingDatetime = Convert.ToDateTime(tblLoadingSlipTODT["loadingDatetime"].ToString());
                    if (tblLoadingSlipTODT["createdOn"] != DBNull.Value)
                        tblLoadingSlipTONew.CreatedOn = Convert.ToDateTime(tblLoadingSlipTODT["createdOn"].ToString());
                    if (tblLoadingSlipTODT["cdStructure"] != DBNull.Value)
                        tblLoadingSlipTONew.CdStructure = Convert.ToDouble(tblLoadingSlipTODT["cdStructure"].ToString());
                    if (tblLoadingSlipTODT["statusReason"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusReason = Convert.ToString(tblLoadingSlipTODT["statusReason"].ToString());
                    if (tblLoadingSlipTODT["vehicleNo"] != DBNull.Value)
                        tblLoadingSlipTONew.VehicleNo = Convert.ToString(tblLoadingSlipTODT["vehicleNo"].ToString().ToUpper());
                    if (tblLoadingSlipTODT["dealerOrgName"] != DBNull.Value)
                        tblLoadingSlipTONew.DealerOrgName = Convert.ToString(tblLoadingSlipTODT["dealerOrgName"].ToString());
                    if (tblLoadingSlipTODT["statusName"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusName = Convert.ToString(tblLoadingSlipTODT["statusName"].ToString());

                    if (tblLoadingSlipTODT["loadingId"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingId = Convert.ToInt32(tblLoadingSlipTODT["loadingId"].ToString());
                    if (tblLoadingSlipTODT["statusReasonId"] != DBNull.Value)
                        tblLoadingSlipTONew.StatusReasonId = Convert.ToInt32(tblLoadingSlipTODT["statusReasonId"].ToString());
                    if (tblLoadingSlipTODT["loadingSlipNo"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingSlipNo = Convert.ToString(tblLoadingSlipTODT["loadingSlipNo"].ToString());

                    if (tblLoadingSlipTODT["isConfirmed"] != DBNull.Value)
                        tblLoadingSlipTONew.IsConfirmed = Convert.ToInt32(tblLoadingSlipTODT["isConfirmed"].ToString());

                    if (tblLoadingSlipTODT["comment"] != DBNull.Value)
                        tblLoadingSlipTONew.Comment = Convert.ToString(tblLoadingSlipTODT["comment"].ToString());
                    if (tblLoadingSlipTODT["contactNo"] != DBNull.Value)
                        tblLoadingSlipTONew.ContactNo = Convert.ToString(tblLoadingSlipTODT["contactNo"].ToString());
                    if (tblLoadingSlipTODT["driverName"] != DBNull.Value)
                        tblLoadingSlipTONew.DriverName = Convert.ToString(tblLoadingSlipTODT["driverName"].ToString());
                    if (tblLoadingSlipTODT["cdStructureId"] != DBNull.Value)
                        tblLoadingSlipTONew.CdStructureId = Convert.ToInt32(tblLoadingSlipTODT["cdStructureId"].ToString());

                    //Vijaymala [2018-26-02]
                    if (tblLoadingSlipTODT["poNo"] != DBNull.Value)
                        tblLoadingSlipTONew.PoNo = Convert.ToString(tblLoadingSlipTODT["poNo"].ToString());

                    //Vijaymla[26-02-2018]added
                    if (tblLoadingSlipTODT["poDate"] != DBNull.Value)
                        tblLoadingSlipTONew.PoDate = Convert.ToDateTime(tblLoadingSlipTODT["poDate"].ToString());

                    //Priyanka [10-03-2018]
                    if (tblLoadingSlipTODT["orcAmt"] != DBNull.Value)
                        tblLoadingSlipTONew.OrcAmt = Convert.ToDouble(tblLoadingSlipTODT["orcAmt"].ToString());

                    //Priyanka [10-03-2018]
                    if (tblLoadingSlipTODT["orcMeasure"] != DBNull.Value)
                        tblLoadingSlipTONew.OrcMeasure = Convert.ToString(tblLoadingSlipTODT["orcMeasure"].ToString());


                    if (tblLoadingSlipTODT["cnfOrgId"] != DBNull.Value)
                        tblLoadingSlipTONew.CnfOrgId = Convert.ToInt32(tblLoadingSlipTODT["cnfOrgId"].ToString());

                    if (tblLoadingSlipTODT["cnfOrgName"] != DBNull.Value)
                        tblLoadingSlipTONew.CnfOrgName = Convert.ToString(tblLoadingSlipTODT["cnfOrgName"].ToString());

                    if (tblLoadingSlipTODT["orcPersonName"] != DBNull.Value)
                        tblLoadingSlipTONew.ORCPersonName = Convert.ToString(tblLoadingSlipTODT["orcPersonName"]);
                

                    if (tblLoadingSlipTODT["freightAmt"] != DBNull.Value)    //Vijaymala [25-04-2018] added
                        tblLoadingSlipTONew.FreightAmt = Convert.ToDouble(tblLoadingSlipTODT["freightAmt"].ToString());
                    if (tblLoadingSlipTODT["isFreightIncluded"] != DBNull.Value) //Vijaymala [25-04-2018] added
                        tblLoadingSlipTONew.IsFreightIncluded = Convert.ToInt32(tblLoadingSlipTODT["isFreightIncluded"].ToString());

                    //Priyanka [04-06-2018] 
                    if (tblLoadingSlipTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipTODT["loadingQty"].ToString());

                    //vijaymala added
                    if (tblLoadingSlipTODT["forAmount"] != DBNull.Value)
                        tblLoadingSlipTONew.ForAmount = Convert.ToDouble(tblLoadingSlipTODT["forAmount"]);
                    if (tblLoadingSlipTODT["isForAmountIncluded"] != DBNull.Value)
                        tblLoadingSlipTONew.IsForAmountIncluded = Convert.ToInt32(tblLoadingSlipTODT["isForAmountIncluded"].ToString());

                    //Priyanka [05-07-2018]
                    if (tblLoadingSlipTODT["addDiscAmt"] != DBNull.Value)
                        tblLoadingSlipTONew.AddDiscAmt = Convert.ToDouble(tblLoadingSlipTODT["addDiscAmt"]);

                    //Priyanka [05-09-2018]
                    if (tblLoadingSlipTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipTONew.BookingId = Convert.ToInt32(tblLoadingSlipTODT["bookingId"].ToString());

                    if (tblLoadingSlipTODT["modbusRefId"] != DBNull.Value)
                        tblLoadingSlipTONew.ModbusRefId = Convert.ToInt32(tblLoadingSlipTODT["modbusRefId"]);

                    if (tblLoadingSlipTODT["gateId"] != DBNull.Value)
                        tblLoadingSlipTONew.GateId = Convert.ToInt32(tblLoadingSlipTODT["gateId"]);
                    if (tblLoadingSlipTODT["portNumber"] != DBNull.Value)
                        tblLoadingSlipTONew.PortNumber = Convert.ToString(tblLoadingSlipTODT["portNumber"]);
                    if (tblLoadingSlipTODT["ioTUrl"] != DBNull.Value)
                        tblLoadingSlipTONew.IotUrl = Convert.ToString(tblLoadingSlipTODT["ioTUrl"]);
                    if (tblLoadingSlipTODT["machineIP"] != DBNull.Value)
                        tblLoadingSlipTONew.MachineIP = Convert.ToString(tblLoadingSlipTODT["machineIP"]);
                    if (tblLoadingSlipTODT["isDBup"] != DBNull.Value)
                        tblLoadingSlipTONew.IsDBup = Convert.ToInt32(tblLoadingSlipTODT["isDBup"]);
                    tblLoadingSlipTOList.Add(tblLoadingSlipTONew);
                }
            }
            return tblLoadingSlipTOList;
        }

        /// <summary>
        /// /// Vijaymala [17-04-2018]:added to get loading slip lisy by loading slip id
        /// </summary>
        /// <param name="idLoadingSlip"></param>
        /// <returns></returns>
        public List<TblLoadingSlipTO> SelectAllTblLoadingSlipListByloadingId(string idLoadingSlip)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlConnection conn = new SqlConnection(sqlConnStr);
          
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoadingSlip in (" + idLoadingSlip + ") ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
                else return null;
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
        /// <summary>
        /// /// Vijaymala [17-04-2018]:added to get loading slip list by vehicle number
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>

        public List<TblLoadingSlipTO> SelectAllLoadingSlipListByVehicleNo(string vehicleNo,DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                // Vaibhav modify
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE vehicleNo LIKE '%" + vehicleNo + "%'" +
                    "And CAST(sq1.statusDate AS DATE)  BETWEEN @fromDate AND @toDate"  ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);
                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null)
                    return list;
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

        /// <summary>
        /// Vijaymala [08-05-2018] added to get notified loading list withiin period 
        /// </summary>
        /// <returns></returns>
        public List<TblLoadingSlipTO> SelectAllNotifiedTblLoadingList(DateTime fromDate, DateTime toDate,Int32 callFlag)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader sqlReader = null;
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();

                String whereCondition = "WHERE CAST(sq1.statusDate AS Date) BETWEEN @fromDate AND @toDate ";

                whereCondition += " AND ISNULL(sq1.callFlag,0) = " + callFlag;

                String sqlSelectQry = " SELECT tempLoadingSlip.* ,cnfOrg.firmName as cnfOrgName , " +
                                  " tempLoadSlipdtl.bookingId, tempLoadSlipdtl.loadingQty, dimStat.statusName,tempLoading.callFlag,tblOrganization.firmName+', '+CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerOrgName " +
                                  " FROM tempLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = tempLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId " +
                                  " LEFT JOIN tempLoading " +
                                  " ON tempLoading.idLoading = tempLoadingSlip.loadingId " +
                                   " Left Join tempLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = tempLoadingSlip.idLoadingSlip " +
                                   "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId" + // Aniket [4-jan-2019] added for villageName agaist dealername


                // Vaibhav [09-Jan-2018] Added to select from  finalLoadingSlip

                " UNION ALL " +

                                  " SELECT finalLoadingSlip.* , cnfOrg.firmName as cnfOrgName," +
                                  " tempLoadSlipdtl.bookingId, tempLoadSlipdtl.loadingQty, dimStat.statusName,finalLoading.callFlag,tblOrganization.firmName+', '+CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerOrgName " +
                                  " FROM finalLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = finalLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg" +
                                  " ON cnfOrg.idOrganization = finalLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = finalLoadingSlip.statusId " +
                                  " LEFT JOIN finalLoading " +
                                  "  ON finalLoading.idLoading = finalLoadingSlip.loadingId " +
                                  "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId"+
                                  " Left Join finalLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = finalLoadingSlip.idLoadingSlip ";


                cmdSelect.CommandText = "SELECT * FROM(" + sqlSelectQry + ")sq1 " + whereCondition;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat); ;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat); ;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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
        /// Priyanka [01-06-2018]: Added to get loading slip list
        /// </summary>

        public List<TblLoadingSlipTO> SelectAllTblLoadingSlipList(TblUserRoleTO tblUserRoleTO, int cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm, Int32 brandId,Int32 superwisorId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            string whereCond = " WHERE CONVERT (DATE,sq1.createdOn,103) BETWEEN @fromDate AND @toDate";
            string areConfJoin = string.Empty;
            string whereCondFin = string.Empty;
            string whereSupCond = string.Empty;
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
                    areConfJoin = " INNER JOIN ( SELECT DISTINCT cnfOrgId FROM tblUserAreaAllocation WHERE isActive=1 AND userId=" + userId + ") areaConf ON  areaConf.cnfOrgId = sq1.cnfOrgId ";
                }

                conn.Open();
                if (loadingStatusId > 0)
                    whereCond += " AND sq1.statusId=" + loadingStatusId;
               // whereCondFin += " AND finloadingSlip.statusId=" + loadingStatusId;

                String wherecnfIdTemp = String.Empty;
                String wherecnfIdFinal = String.Empty;


                String whereisConTemp = String.Empty;
                String whereisConFinal = String.Empty;

                if (cnfId > 0)
                {
                    wherecnfIdTemp += " AND sq1.cnfOrgId = " + cnfId;
                //    wherecnfIdFinal += " AND finloadingSlip.cnfOrgId = " + cnfId;
                }

                if (dealerId > 0)
                {
                    wherecnfIdTemp += " AND sq1.dealerOrgId = " + dealerId;
                 //   wherecnfIdFinal += " AND finloadingSlip.dealerOrgId = " + dealerId;
                }

                if (superwisorId >0)
                {
                    whereSupCond += " AND sq1.superwisorId=" + superwisorId;
                 

                }

                if (loadingTypeId > 0)
                {
                    whereCond += " AND sq1.loadingType=" + loadingTypeId;
                }

                //Priyanka [31-05-2018] : Added to show the confirm and non-confirm loading slip.
                if (isConfirm == 0 || isConfirm == 1)
                {
                    whereisConTemp += " AND sq1.isConfirmed = " + isConfirm;
                //    whereisConFinal += " AND finloadingSlip.isConfirmed = " + isConfirm;
                }

                String whereisConTemp1 = String.Empty;
                String whereisConFinal1 = String.Empty;
                if (brandId > 0)
                {
                    whereisConTemp1 += " WHERE  tblLoadingSlip.idLoadingSlip IN (select loadingSlipId from tempLoadingSlipExt where brandId = " + brandId + " ) ";
                    whereisConFinal1 += " WHERE tblLoadingSlip.idLoadingSlip IN (select loadingSlipId from tempLoadingSlipExt where brandId = " + brandId + " ) ";
                }

                String sqlQuery = " SELECT tblLoadingSlip.* ,org.digitalSign, loading.loadingType, loading.superwisorId,org.firmName as cnfOrgName, tblLoadSlipdtl.loadingQty, tblOrganization.firmName " +
                                  " + ',' +  CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN address.villageName IS NOT NULL " +
                                  " THEN address.villageName ELSE CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName " +
                                  " ELSE CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE address.stateName" +
                                  " END END END END AS  dealerOrgName,transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName, tblLoadSlipdtl.bookingId " +
                                   " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                   " , loading.modbusRefId, loading.gateId,loading.isDBup "+
                                  " FROM  tempLoadingSlip tblLoadingSlip " +
                                  " Left Join tempLoadingSlipDtl tblLoadSlipdtl ON tblLoadSlipdtl.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  " LEFT JOIN tblOrganization ON tblOrganization.idOrganization = tblLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tempLoading loading ON loading.idLoading = tblLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = tblLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = tblLoadingSlip.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                  " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  " LEFT JOIN vAddressDetails address ON address.idAddr = tblOrganization.addrId  " + whereisConTemp1 +

                                  " UNION ALL " +

                                  " SELECT tblLoadingSlip.* ,org.digitalSign,loading.loadingType,loading.superwisorId, org.firmName as cnfOrgName, tblLoadSlipdtl.loadingQty, tblOrganization.firmName " +
                                   " + ',' +  CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN address.villageName IS NOT NULL " +
                                  " THEN address.villageName ELSE CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName " +
                                  " ELSE CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE address.stateName" +
                                  " END END END END AS  dealerOrgName , " +
                                  " transOrg.firmName as transporterOrgName ,dimStat.statusName ,ISNULL(person.firstName,'') + ' ' + ISNULL(person.lastName,'') AS superwisorName    " +
                                  " ,tblUser.userDisplayName, tblLoadSlipdtl.bookingId " +
                                  " , tblGate.portNumber, tblGate.IoTUrl, tblGate.machineIP " +
                                   " , loading.modbusRefId, loading.gateId,loading.isDBup " +
                                  " FROM finalLoadingSlip tblLoadingSlip  " +
                                  " Left Join finalLoadingSlipDtl tblLoadSlipdtl ON tblLoadSlipdtl.loadingSlipId = tblLoadingSlip.idLoadingSlip " +
                                  " LEFT JOIN tblOrganization ON tblOrganization.idOrganization = tblLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN finalLoading loading ON loading.idLoading = tblLoadingSlip.loadingId " +
                                  " LEFT JOIN tblOrganization org ON org.idOrganization = tblLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = tblLoadingSlip.statusId " +
                                  " LEFT JOIN tblSupervisor superwisor ON superwisor.idSupervisor=loading.superwisorId " +
                                  " LEFT JOIN tblPerson person ON superwisor.personId = person.idPerson" +
                                  " LEFT JOIN tblOrganization transOrg ON transOrg.idOrganization = loading.transporterOrgId " +
                                  " LEFT JOIN tblUser ON idUser=loading.createdBy " +
                                   " LEFT JOIN tblGate tblGate ON tblGate.idGate = loading.gateId " +
                                  " LEFT JOIN vAddressDetails address ON address.idAddr = tblOrganization.addrId " + whereisConFinal1;


                cmdSelect.CommandText = "Select * from ("+ sqlQuery + " )sq1" + areConfJoin + whereCond + wherecnfIdTemp + whereisConTemp +whereSupCond;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = ConvertDTToList(sqlReader);
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




        //Priyanka [08-05-2018] : For showing ORC Report from booking and loading.
        public List<TblORCReportTO> SelectORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            String sqlSelectQueryTemp = String.Empty;
            String sqlSelectQueryFinal = String.Empty;
            String FinalSqlSelectQuery = String.Empty;

            String whereCon = String.Empty;
            try
            {
                conn.Open();
                if (fromDate != null && toDate != null)
                {
                    if (flag == 0)
                    {
                        sqlSelectQueryTemp = " Select dimStatus.statusName, cnfOrg.firmName As cnfName, tblBookings.idBooking, tempLoadSlipdtl.bookingId," +
                         " tempLoadSlipdtl.loadingQty, loading.callFlag,tblOrganization.firmName + ', ' + CASE WHEN tblOrganization.addrId IS NULL THEN '' " +
                         " Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN " +
                         " vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  Dealer, tblLoadingSlip.createdOn,  " +
                         " tblBookings.bookingQty, tblBookings.bookingRate,  tblLoadingSlip.comment,tblLoadingSlip.orcAmt, tblLoadingSlip.orcMeasure,  " +
                         " tblLoadingSlipExt.rateCalcDesc from tempLoadingSlip tblLoadingSlip " +
                         " LEFT Join  tblOrganization tblOrganization ON tblLoadingSlip.dealerOrgId = tblOrganization.idOrganization " +
                         " LEFT Join  tempLoading loading ON loading.idLoading = tblLoadingSlip.loadingId " +
                         " LEFT Join  tblOrganization cnfOrg ON loading.cnfOrgId = cnfOrg.idOrganization " +
                         " LEFT Join dimStatus dimStatus on tblLoadingSlip.statusId = dimStatus.idStatus " +
                         " LEFT JOIN tempLoadingSlipExt tblLoadingSlipExt ON tblLoadingSlip.idLoadingSlip = tblLoadingSlipExt.loadingSlipId " +
                         " LEFT Join tblBookings tblBookings ON tblBookings.idBooking = tblLoadingSlipExt.bookingId " +
                         " LEFT JOIN tempLoadingSlipDtl tempLoadSlipdtl ON tblLoadingSlip.idLoadingSlip = tempLoadSlipdtl.loadingSlipId " +
                         " LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId"; //Aniket K[4-jan-19] added for to display villagename agaist Dealer

                        sqlSelectQueryFinal = " Select dimStatus.statusName, cnfOrg.firmName AS cnfName, tblBookings.idBooking, tblOrganization.firmName+', '+ finalLoadSlipdtl.bookingId, finalLoadSlipdtl.loadingQty, loading.callFlag, " +
                        " CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName "+
                        " ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  Dealer, tblLoadingSlip.createdOn, tblBookings.bookingQty,tblBookings.bookingRate, "+
		                " tblLoadingSlip.comment,tblLoadingSlip.orcAmt, tblLoadingSlip.orcMeasure,  tblLoadingSlipExt.rateCalcDesc from finalLoadingSlip tblLoadingSlip "+
                        " LEFT JOIN  tblOrganization tblOrganization ON tblLoadingSlip.dealerOrgId = tblOrganization.idOrganization "+
                        " LEFT JOIN  finalLoading loading ON loading.idLoading = tblLoadingSlip.loadingId "+
                        " LEFT JOIN  tblOrganization cnfOrg ON loading.cnfOrgId = cnfOrg.idOrganization "+
                        " LEFT JOIN dimStatus dimStatus on tblLoadingSlip.statusId = dimStatus.idStatus "+
                        " LEFT JOIN finalLoadingSlipExt tblLoadingSlipExt ON tblLoadingSlip.idLoadingSlip = tblLoadingSlipExt.loadingSlipId "+
                        " LEFT JOIN tblBookings tblBookings ON tblBookings.idBooking = tblLoadingSlipExt.bookingId "+
                        " LEFT JOIN finalLoadingSlipDtl finalLoadSlipdtl ON tblLoadingSlip.idLoadingSlip = finalLoadSlipdtl.loadingSlipId "+
                        " LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId"; //Aniket K[4-jan-19] added for to display villagename agaist Dealer

                        whereCon = " Where CAST(tblLoadingSlip.createdOn AS DATE) BETWEEN @fromDate AND @toDate " +
                                              "AND tblLoadingSlip.statusId NOT IN (" + (int)Constants.TranStatusE.LOADING_CANCEL + " ) AND tblLoadingSlip.orcAmt > 0";

                        FinalSqlSelectQuery ="select DISTINCT * from (" + sqlSelectQueryTemp + whereCon + "UNION ALL" + sqlSelectQueryFinal + whereCon + ") As sq1";
                    }
                    else
                    {
                        FinalSqlSelectQuery = "Select dimStatus.statusName ,cnfOrg.firmName As cnfName,tblOrganization.firmName+', '+CASE WHEN tblOrganization.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  Dealer ," +
                                          " tblBookings.idBooking,tblBookings.createdOn, tblBookings.bookingQty, " +
                                          " tblBookings.bookingRate,tblBookings.comments AS comment,tblBookings.orcAmt , " +
                                          " tblBookings.orcMeasure, '' AS rateCalcDesc from tblBookings tblBookings " +
                                          " LEFT Join tblOrganization tblOrganization ON tblBookings.dealerOrgId = tblOrganization.idOrganization " +
                                          " LEFT Join  tblOrganization cnfOrg ON tblbookings.cnfOrgId = cnfOrg.idOrganization " +
                                          " LEFT Join dimStatus dimStatus on tblBookings.statusId = dimStatus.idStatus " +
                                          " LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = tblOrganization.addrId" + //Aniket K[4-jan-19] added for to display villagename agaist Dealer
                                          " Where CAST(tblBookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate " +
                                          " AND tblBookings.statusId NOT IN (" + (int)Constants.TranStatusE.LOADING_CANCEL + " ) AND tblBookings.orcAmt > 0";

                    }
                }
                else
                {
                    cmdSelect.CommandText = null;
                }
                cmdSelect.CommandText = FinalSqlSelectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblORCReportTO> list = ConvertDTToListForORC(sqlReader);
                if (sqlReader != null)
                    sqlReader.Dispose();
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectORCReportDetailsList");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        //Priyanka [11-05-2018] : Added this ConvertDTToList for ORC report- from loading, from booking

        public List<TblORCReportTO> ConvertDTToListForORC(SqlDataReader tblORCReportTODT)
        {
            List<TblORCReportTO> tblORCReportTOList = new List<TblORCReportTO>();
            if (tblORCReportTODT != null)
            {
                while (tblORCReportTODT.Read())
                {
                    TblORCReportTO tblORCReportTONew = new TblORCReportTO();
                    if (tblORCReportTODT["dealer"] != DBNull.Value)
                        tblORCReportTONew.Dealer = Convert.ToString(tblORCReportTODT["dealer"].ToString());
                    if (tblORCReportTODT["cnfName"] != DBNull.Value)
                        tblORCReportTONew.CnfName = Convert.ToString(tblORCReportTODT["cnfName"].ToString());
                    if (tblORCReportTODT["statusName"] != DBNull.Value)
                        tblORCReportTONew.StatusName = Convert.ToString(tblORCReportTODT["statusName"].ToString());
                    if (tblORCReportTODT["idBooking"] != DBNull.Value)
                        tblORCReportTONew.IdBooking = Convert.ToInt32(tblORCReportTODT["idBooking"].ToString());
                    if (tblORCReportTODT["createdOn"] != DBNull.Value)
                        tblORCReportTONew.CreatedOn = Convert.ToDateTime(tblORCReportTODT["createdOn"].ToString());
                    if (tblORCReportTODT["bookingQty"] != DBNull.Value)
                        tblORCReportTONew.BookingQty = Convert.ToDouble(tblORCReportTODT["bookingQty"].ToString());
                    if (tblORCReportTODT["bookingRate"] != DBNull.Value)
                        tblORCReportTONew.BookingRate = Convert.ToDouble(tblORCReportTODT["bookingRate"].ToString());

                    if (tblORCReportTODT["comment"] != DBNull.Value)
                        tblORCReportTONew.Comment = Convert.ToString(tblORCReportTODT["comment"].ToString());

                    if (tblORCReportTODT["orcAmt"] != DBNull.Value)
                        tblORCReportTONew.OrcAmt = Convert.ToDouble(tblORCReportTODT["orcAmt"].ToString());

                    if (tblORCReportTODT["orcMeasure"] != DBNull.Value)
                        tblORCReportTONew.OrcMeasure = Convert.ToString(tblORCReportTODT["orcMeasure"].ToString());
                    if (tblORCReportTODT["rateCalcDesc"] != DBNull.Value)
                        tblORCReportTONew.RateCalcDesc = Convert.ToString(tblORCReportTODT["rateCalcDesc"].ToString());

                    tblORCReportTOList.Add(tblORCReportTONew);
                }
            }
            return tblORCReportTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipTO, cmdInsert);
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

        public int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlip]( " +
                            "  [dealerOrgId]" +
                            " ,[isJointDelivery]" +
                            " ,[noOfDeliveries]" +
                            " ,[statusId]" +
                            " ,[createdBy]" +
                            " ,[statusDate]" +
                            " ,[loadingDatetime]" +
                            " ,[createdOn]" +
                            " ,[cdStructure]" +
                            " ,[statusReason]" +
                            " ,[vehicleNo]" +
                            " ,[loadingId]" +
                            " ,[statusReasonId]" +
                            " ,[loadingSlipNo]" +
                            " ,[isConfirmed]" +
                            " ,[comment]" +
                            " ,[contactNo]" +
                            " ,[driverName]" +
                            " ,[cdStructureId]" +
                            " ,[poNo]" +
                            " ,[poDate]" + //Vijaymala[2018-02-26]Added
                            " ,[cnfOrgId]" +
                            " ,[orcAmt]"+
                            " ,[orcMeasure]"+
                            " ,[orcPersonName]" +
                            " ,[freightAmt]" +//Vijaymala[2018-04-25]Added
                            " ,[isFreightIncluded]" +//Vijaymala[2018-04-25]Added
                            " ,[forAmount]" +
                            " ,[isForAmountIncluded]" +
                            " ,[addDiscAmt]" +          //Priyanka [05-07-2018]
                            " )" +
                " VALUES (" +
                            "  @DealerOrgId " +
                            " ,@IsJointDelivery " +
                            " ,@NoOfDeliveries " +
                            " ,@StatusId " +
                            " ,@CreatedBy " +
                            " ,@StatusDate " +
                            " ,@LoadingDatetime " +
                            " ,@CreatedOn " +
                            " ,@CdStructure " +
                            " ,@StatusReason " +
                            " ,@VehicleNo " +
                            " ,@LoadingId " +
                            " ,@statusReasonId " +
                            " ,@loadingSlipNo " +
                            " ,@isConfirmed " +
                            " ,@comment " +
                            " ,@contactNo " +
                            " ,@driverName " +
                            " ,@cdStructureId " +
                            " ,@poNo " +
                            " ,@poDate" + //Vijaymala[2018-02-26]Added
                            " ,@CnfOrgId " +
                            " ,@orcAmt"+
                            " ,@orcMeasure"+
                            " ,@ORCPersonName" +
                            " ,@freightAmt " +//Vijaymala[2018-04-25]Added
                            " ,@isFreightIncluded " +//Vijaymala[2018-04-25]Added
                            " ,@forAmount" +
                            " ,@isForAmountIncluded" +
                            " ,@addDiscAmt" +           //Priyanka [05-07-2018]
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.DealerOrgId;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.CreatedBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.StatusDate;
            cmdInsert.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingDatetime);
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.CreatedOn;
            cmdInsert.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipTO.CdStructure;
            cmdInsert.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReason);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.VehicleNo);
            cmdInsert.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.LoadingId;
            cmdInsert.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.StatusReasonId);
            cmdInsert.Parameters.Add("@loadingSlipNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingSlipNo);
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsConfirmed;
            cmdInsert.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.Comment);
            cmdInsert.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ContactNo);
            cmdInsert.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.DriverName);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.CdStructureId);
            cmdInsert.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.PoNo);
            cmdInsert.Parameters.Add("@poDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.PoDate);//Vijaymala [2018-02-26] Added
            cmdInsert.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.CnfOrgId);
            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcAmt); //Priyanka [05-03-2018] 
            cmdInsert.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcMeasure); //Priyanka [06-03-2018] 
            cmdInsert.Parameters.Add("@ORCPersonName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ORCPersonName);
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.FreightAmt);//Vijaymala[2018-04-25]Added
            cmdInsert.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.IsFreightIncluded);//Vijaymala[2018-04-25]Added
            cmdInsert.Parameters.Add("@forAmount", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ForAmount);
            cmdInsert.Parameters.Add("@isForAmountIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.IsForAmountIncluded);
            cmdInsert.Parameters.Add("@addDiscAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.AddDiscAmt); //Priyanka [05-07-2018] 
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblLoadingSlipTO.IdLoadingSlip = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipTO, cmdUpdate);
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

        public int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipTO, cmdUpdate);
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

        public int UpdateTblLoadingSlip(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempLoadingSlip] SET " +
                            "  [statusId]= @StatusId" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[vehicleNo]= @VehicleNo" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[statusReason]= @StatusReason" +
                            " ,[statusReasonId]= @statusReasonId" +
                            " WHERE [loadingId]= @LoadingId ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusId;
                cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingTO.StatusDate;
                cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.LoadingDatetime);
                cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.StatusReason);
                cmdUpdate.Parameters.Add("@LoadingId", System.Data.SqlDbType.Int).Value = tblLoadingTO.IdLoading;
                cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = tblLoadingTO.StatusReasonId;
                cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingTO.VehicleNo);
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

        public int ExecuteUpdationCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlip] SET " +
                            "  [dealerOrgId]= @DealerOrgId" +
                            " ,[isJointDelivery]= @IsJointDelivery" +
                            " ,[noOfDeliveries]= @NoOfDeliveries" +
                            " ,[statusId]= @StatusId" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[loadingDatetime]= @LoadingDatetime" +
                            " ,[cdStructure]= @CdStructure" +
                            " ,[statusReason]= @StatusReason" +
                            " ,[statusReasonId]= @statusReasonId" +
                            " ,[vehicleNo] = @VehicleNo" +
                            " ,[isConfirmed] = @isConfirmed" +
                            " ,[comment] = @comment" +
                            " ,[contactNo] = @contactNo" +
                            " ,[driverName] = @driverName" +
                            " ,[cdStructureId] = @cdStructureId" +
                            " ,[poNo] = @poNo" + //Vijaymala [2018-02-26] Added
                            " ,[poDate]=@poDate " +   //Vijaymala [2018-02-26] Added
                            " ,[orcAmt]=@orcAmt" +//Priyanka [2018-03-13] Added
                            " ,[orcMeasure]=@orcMeasure" +//Priyanka [2018-03-13] Added
                            " ,[cnfOrgId] = @CnfOrgId" +
                            " ,[freightAmt] = @freightAmt" +//Vijaymala[2018-04-25]Added
                            " ,[isFreightIncluded] = @isFreightIncluded" +//Vijaymala[2018-04-25]Added
                            " ,[forAmount] = @forAmount " +
                            " ,[isForAmountIncluded] = @isForAmountIncluded" +
                            " ,[addDiscAmt] = @addDiscAmt" +                //Priyanka [05-07-2018]
                            " WHERE [idLoadingSlip] = @IdLoadingSlip ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.DealerOrgId;
            cmdUpdate.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsJointDelivery;
            cmdUpdate.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.NoOfDeliveries;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusId;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblLoadingSlipTO.StatusDate;
            cmdUpdate.Parameters.Add("@LoadingDatetime", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.LoadingDatetime);
            cmdUpdate.Parameters.Add("@CdStructure", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipTO.CdStructure;
            cmdUpdate.Parameters.Add("@StatusReason", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipTO.StatusReason;
            cmdUpdate.Parameters.Add("@statusReasonId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.StatusReasonId;
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.VarChar).Value = tblLoadingSlipTO.VehicleNo;
            cmdUpdate.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.Comment);
            cmdUpdate.Parameters.Add("@contactNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ContactNo);
            cmdUpdate.Parameters.Add("@driverName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.DriverName);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.CdStructureId;
            cmdUpdate.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.PoNo);
            cmdUpdate.Parameters.Add("@poDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.PoDate);

            cmdUpdate.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcAmt); //Priyanka [10-03-2018] 
            cmdUpdate.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.OrcMeasure); //Priyanka [10-03-2018] 

            cmdUpdate.Parameters.Add("@CnfOrgId", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.CnfOrgId;
            cmdUpdate.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.FreightAmt); //Vijaymala [25-04-2018] added
            cmdUpdate.Parameters.Add("@isFreightIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.IsFreightIncluded); //Vijaymala [25-04-2018] added
            cmdUpdate.Parameters.Add("@forAmount", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.ForAmount);
            cmdUpdate.Parameters.Add("@isForAmountIncluded", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.IsForAmountIncluded);
            cmdUpdate.Parameters.Add("@addDiscAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipTO.AddDiscAmt); //Priyanka [05-07-2018] 
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblLoadingSlip(Int32 idLoadingSlip)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadingSlip, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblLoadingSlip(Int32 idLoadingSlip, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadingSlip, cmdDelete);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idLoadingSlip, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlip] " +
            " WHERE idLoadingSlip = " + idLoadingSlip + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadingSlip", System.Data.SqlDbType.Int).Value = tblLoadingSlipTO.IdLoadingSlip;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
