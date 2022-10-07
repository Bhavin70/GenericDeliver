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
    public class TblInvoiceDAO : ITblInvoiceDAO
    {
        private readonly ITblLoadingSlipDAO _iTblLoadingSlipDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        public TblInvoiceDAO(ICommon iCommon, ITblLoadingSlipDAO iTblLoadingSlipDAO, IConnectionString iConnectionString)
        {
            _iTblLoadingSlipDAO = iTblLoadingSlipDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
     
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT "+ (int)Constants.TranTableType.TEMP + " AS tableType, invoice.*,loading.isDBup ,dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc,loadingSlip.statusId as loadingStatusId  " +
                                  " FROM tempInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                                  " LEFT JOIN tempLoadingSlip loadingSlip ON idLoadingSlip = invoice.loadingSlipId " +
                                  " LEFT JOIN tempLoading loading ON idLoading = loadingSlip.loadingid " +

                                  // Vaibhav [10-Jan-2018] Added to select from finalInvoice

                                  " UNION ALL " +
                                  " SELECT " + (int)Constants.TranTableType.FINAL + " AS tableType, invoice.*,loading.isDBup ,dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc,loadingSlip.statusId as loadingStatusId  " +
                                  " FROM finalInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId "+
                                  " LEFT JOIN finalLoadingSlip loadingSlip  ON idLoadingSlip = invoice.loadingSlipId " +
                                  " LEFT JOIN finalLoading loading ON idLoading = loadingSlip.loadingid ";

            return sqlSelectQry;
        }
        #endregion

        #region Selection

        public List<TblInvoiceTO> SelectAllTblInvoice()
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

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
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
        //Aniket [28-01-2019] added to filter manual tax invoice number
        public TblInvoiceTO SelectAllTblInvoice(string taxInvoiceNumber,int finYearId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
           
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.finYearId = " + finYearId + "AND sq1.invoiceNo=" +"'"+taxInvoiceNumber+ "'";
                
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                TblInvoiceTO tblInvoiceTO = ConvertDTTo(reader);
                return tblInvoiceTO;
            }
            catch (Exception ex)
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
        /// <summary>
        /// Ramdas.w @24102017 this method is  Get Generated Invoice List
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <param name="cnfId"></param>
        /// <param name="dealerID"></param>
        /// <param name="userRoleTO"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectAllTblInvoice(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerId, TblUserRoleTO tblUserRoleTO, Int32 brandId, Int32 invoiceId,Int32 statusId, String internalOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string strQuery = string.Empty;
            string strCAndFQuery = string.Empty;
            String areConfJoin = String.Empty;
            string strStatusIds = (int)Constants.InvoiceStatusE.AUTHORIZED + "," + (int)Constants.InvoiceStatusE.CANCELLED;
            SqlDataReader reader = null;
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
                // Vaibhav [10-Jan-2018] Added to select from finalInvoice
                String sqlQueryTemp = " SELECT " + (int)Constants.TranTableType.TEMP + " AS tableType, invoice.*,loading.isDBup ,distributor.firmName AS distributorName,dealer.firmName+', '+CASE WHEN dealer.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerName" +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc, loadingSlip.statusId as loadingStatusId " +
                                  " FROM tempInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId "+
                                  "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = dealer.addrId" + // Aniket K[2-Jan-2019] added villageName against dealer
                                   " LEFT JOIN tempLoadingSlip loadingSlip ON idLoadingSlip = invoice.loadingSlipId " + 
                                   " LEFT JOIN temploading loading ON idLoading = loadingSlip.loadingId "; //Aniket [22-8-2019] added for IoT

                String sqlQueryFinal = " SELECT " + (int)Constants.TranTableType.FINAL + " AS tableType, invoice.* ,loading.isDBup ,distributor.firmName AS distributorName,dealer.firmName+', '+CASE WHEN dealer.addrId IS NULL THEN '' Else case WHEN vAddr.villageName IS NOT NULL THEN vAddr.villageName ELSE CASE WHEN vAddr.talukaName IS NOT NULL THEN vAddr.talukaName ELSE CASE WHEN vAddr.districtName IS NOT NULL THEN vAddr.districtName ELSE vAddr.stateName END END END END AS  dealerName" +
                " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc ,loadingSlip.statusId as loadingStatusId " +
                                  " FROM finalInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId "+
                                   "LEFT JOIN vAddressDetails vAddr ON vAddr.idAddr = dealer.addrId"+ // Aniket K[2-Jan-2019] added villageName against dealer
                                    " LEFT JOIN finalLoadingSlip loadingSlip ON idLoadingSlip = invoice.loadingSlipId " +
                               " LEFT JOIN finalloading loading ON idLoading = loadingSlip.loadingId ";


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
                                 "   ( addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId  and  areaConf.isActive=1 )" +
                                 "   Or (areaConf.cnfOrgId=tblCnfDealers.cnfOrgId and Isnull(areaConf.districtId,0)=0 and  areaConf.isActive=1  )  " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER +
                                 "   AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                 // "   UNION ALL " +
                                 //"    SELECT areaConf.cnfOrgId, idOrganization = 0 FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + "   AND areaConf.isActive = 1 " +

                                 " ) AS userAreaDealer On (userAreaDealer.cnfOrgId = invoice.distributorOrgId AND invoice.dealerOrgId = userAreaDealer.idOrganization )";
                        
                }
                //if (cnfId == 0 && dealerId == 0)
                //{
                //    strQuery = SqlSelectQuery() + areConfJoin + " WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND isConfirmed IN(" + isConfirm + ")";
                //}

                //else if (cnfId > 0 && dealerId == 0)
                //{
                //    strQuery = SqlSelectQuery() + areConfJoin + "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND isConfirmed IN(" + isConfirm + ") AND invoice.distributorOrgId='" + cnfId + "'";
                //}

                //else if (cnfId > 0 && dealerId > 0)
                //{
                //    strQuery = SqlSelectQuery() + areConfJoin + "WHERE CAST(invoice.statusDate AS DATE) BETWEEN @fromDate AND @toDate AND isConfirmed IN(" + isConfirm + ")  AND invoice.distributorOrgId='" + cnfId + "' AND invoice.dealerOrgId='" + dealerId + "' ";
                //}

                //Priyanka [17-08-2018]
                String whereCondition = " WHERE CAST(invoice.statusDate AS DATETIME) BETWEEN @fromDate AND @toDate AND invoice.isConfirmed IN(" + isConfirm + ")";
                if (invoiceId > 0)
                {
                    whereCondition = " WHERE invoice.idInvoice =" + invoiceId; 
                }

                //Saket [2018-02-23] Added brand 
                String brandCondition = "";
              
                if (brandId > 0)
                {
                    brandCondition = "  AND invoice.brandId = " + brandId;
                }


                //Saket [2019-12-06] Added.
                String interOrgConition = "";
                if (!String.IsNullOrEmpty(internalOrgId))
                {
                    interOrgConition = "  AND invoice.invFromOrgId IN (" + internalOrgId + ")";
                }


                //Vijaymala [2018-08-10 Added status filter 
                String statusCondition = "";
                if (statusId > 0 )
                {
                    statusCondition = "  AND invoice.statusId = " + statusId;
                }

                // Vaibhav [10-Jan-2018] Commented and added new code
                if (cnfId == 0 && dealerId == 0)
                {
                    strQuery = " SELECT * FROM ( " + sqlQueryTemp + areConfJoin + whereCondition  + brandCondition + interOrgConition + statusCondition +

                               " UNION ALL " + sqlQueryFinal + areConfJoin + whereCondition + brandCondition + interOrgConition + statusCondition + " )sq1 ORDER BY sq1.idInvoice desc";

                }

                else if (cnfId > 0 && dealerId == 0)
                {
                    strQuery = " SELECT * FROM ( " + sqlQueryTemp + areConfJoin + whereCondition + "  AND invoice.distributorOrgId='" + cnfId + "'" + brandCondition + interOrgConition + statusCondition +
                               " UNION ALL " + sqlQueryFinal + areConfJoin + whereCondition + " AND invoice.distributorOrgId='" + cnfId + "' " + brandCondition + interOrgConition + statusCondition + "  )sq1 ORDER BY sq1.idInvoice desc";

                }

                else if (cnfId > 0 && dealerId > 0)
                {
                    strQuery = " SELECT * FROM ( " + sqlQueryTemp + areConfJoin + whereCondition + "  AND invoice.distributorOrgId='" + cnfId + "' AND invoice.dealerOrgId='" + dealerId + "' " + brandCondition + interOrgConition + statusCondition +
                                " UNION ALL " + sqlQueryFinal + areConfJoin + whereCondition + "  AND invoice.distributorOrgId='" + cnfId + "' AND invoice.dealerOrgId='" + dealerId + "' " + brandCondition + interOrgConition + statusCondition + " )sq1 ORDER BY sq1.idInvoice desc";
                }

                cmdSelect.CommandText = strQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
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

        public TblInvoiceTO SelectTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE idInvoice = " + idInvoice + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public TblInvoiceTO SelectTblInvoice(String loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId IN(" + loadingSlipId + ") ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1) return list[0];
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public String SelectresponseForPhotoInReport(Int32 idInvoice,Int32 ApiId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            String response = String.Empty;
            String whereCond = String.Empty;
            String sqlQuery = String.Empty; 
            try
            {
                conn.Open();
                cmdSelect.Connection = conn;

                sqlQuery = "SELECT response FROM tempEInvoiceApiResponse";
                whereCond = " WHERE apiId = " + ApiId + " AND invoiceId = " + idInvoice;
                cmdSelect.CommandText = sqlQuery + whereCond;
                cmdSelect.CommandType = System.Data.CommandType.Text;                
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (reader.Read())
                {
                    // get the results of each column
                    response = (string)reader["response"];
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                conn.Close();
            }
        }

        /// <summary>
        /// Ramdas.W:@22092017:API This method is used to Get List of Invoice By Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <param name="distributorOrgId"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectTblInvoiceByStatus(Int32 statusId, int distributorOrgId,int invoiceId, SqlConnection conn, SqlTransaction tran,int isConfirm)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string whereCondition = String.Empty;
            string isConfirmCondition = String.Empty;
            try
            {
                if(isConfirm!=2)
                {
                    isConfirmCondition = " AND sq1.isConfirmed=" + isConfirm;
                }
                if (invoiceId > 0)
                {
                    whereCondition = " AND sq1.idInvoice = " + invoiceId;
                }
                if (distributorOrgId == 0)
                {
                    cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.statusId = " + statusId + whereCondition+isConfirmCondition;

                }
                else
                {
                    cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.statusId = " + statusId + " AND ISNULL(sq1.distributorOrgId,0)=" + distributorOrgId + isConfirmCondition;

                }
              
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null && list.Count >= 1) return list;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        public List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipId = " + loadingSlipId + " ";
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE  sq1.idInvoice IN (select invoiceId from tempLoadingSlipInvoice where loadingSlipId = " + loadingSlipId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblInvoiceTO> list = ConvertDTToList(reader);
                //if (list != null && list.Count == 1) return list[0];
                //return null;

                return ConvertDTToList(reader);

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
         
        public List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn = null, SqlTransaction tran = null)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Boolean isConnection = false;
            try
            {
                if (conn != null)
                {
                    cmdSelect.Connection = conn;
                    cmdSelect.Transaction = tran;
                    isConnection = true;
                }
                else
                {
                    String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
                    conn = new SqlConnection(sqlConnStr);
                    cmdSelect.Connection = conn;
                    conn.Open();
                }

                //cmdSelect.CommandText = " SELECT * FROM ("+ SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipId = " + loadingSlipId + " ";
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE  sq1.idInvoice IN (select invoiceId from tempLoadingSlipInvoice where loadingSlipId = " + loadingSlipId + ")";
                //cmdSelect.Connection = conn;
                //cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
                if (!isConnection)
                    conn.Close();
            }
        }

        /// <summary>
        /// Saket [2018-02-15] Added.
        /// </summary>
        /// <param name="loadingSlipIds"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.loadingSlipId IN (" + loadingSlipIds + ") ";
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE  sq1.idInvoice IN (select invoiceId from tempLoadingSlipInvoice where loadingSlipId IN ( " + loadingSlipIds + " ))";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }
        //Aniket [28-01-2019]
        public TblInvoiceTO ConvertDTTo(SqlDataReader tblInvoiceTODT)
        {
            TblInvoiceTO tblInvoiceTONew = new TblInvoiceTO();
            if (tblInvoiceTODT != null)
            {
                while (tblInvoiceTODT.Read())
                {
                    
                    if (tblInvoiceTODT["idInvoice"] != DBNull.Value)
                        tblInvoiceTONew.IdInvoice = Convert.ToInt32(tblInvoiceTODT["idInvoice"].ToString());
                    if (tblInvoiceTODT["invoiceTypeId"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceTypeId = Convert.ToInt32(tblInvoiceTODT["invoiceTypeId"].ToString());
                    if (tblInvoiceTODT["transportOrgId"] != DBNull.Value)
                        tblInvoiceTONew.TransportOrgId = Convert.ToInt32(tblInvoiceTODT["transportOrgId"].ToString());
                    if (tblInvoiceTODT["transportModeId"] != DBNull.Value)
                        tblInvoiceTONew.TransportModeId = Convert.ToInt32(tblInvoiceTODT["transportModeId"].ToString());
                    if (tblInvoiceTODT["currencyId"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyId = Convert.ToInt32(tblInvoiceTODT["currencyId"].ToString());
                    if (tblInvoiceTODT["loadingSlipId"] != DBNull.Value)
                        tblInvoiceTONew.LoadingSlipId = Convert.ToInt32(tblInvoiceTODT["loadingSlipId"].ToString());
                    if (tblInvoiceTODT["distributorOrgId"] != DBNull.Value)
                        tblInvoiceTONew.DistributorOrgId = Convert.ToInt32(tblInvoiceTODT["distributorOrgId"].ToString());
                    if (tblInvoiceTODT["dealerOrgId"] != DBNull.Value)
                        tblInvoiceTONew.DealerOrgId = Convert.ToInt32(tblInvoiceTODT["dealerOrgId"].ToString());
                    if (tblInvoiceTODT["finYearId"] != DBNull.Value)
                        tblInvoiceTONew.FinYearId = Convert.ToInt32(tblInvoiceTODT["finYearId"].ToString());
                    if (tblInvoiceTODT["statusId"] != DBNull.Value)
                        tblInvoiceTONew.StatusId = Convert.ToInt32(tblInvoiceTODT["statusId"].ToString());
                    if (tblInvoiceTODT["createdBy"] != DBNull.Value)
                        tblInvoiceTONew.CreatedBy = Convert.ToInt32(tblInvoiceTODT["createdBy"].ToString());
                    if (tblInvoiceTODT["updatedBy"] != DBNull.Value)
                        tblInvoiceTONew.UpdatedBy = Convert.ToInt32(tblInvoiceTODT["updatedBy"].ToString());
                    if (tblInvoiceTODT["invoiceDate"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceDate = Convert.ToDateTime(tblInvoiceTODT["invoiceDate"].ToString());
                    if (tblInvoiceTODT["lrDate"] != DBNull.Value)
                        tblInvoiceTONew.LrDate = Convert.ToDateTime(tblInvoiceTODT["lrDate"].ToString());
                    if (tblInvoiceTODT["statusDate"] != DBNull.Value)
                        tblInvoiceTONew.StatusDate = Convert.ToDateTime(tblInvoiceTODT["statusDate"].ToString());
                    if (tblInvoiceTODT["createdOn"] != DBNull.Value)
                        tblInvoiceTONew.CreatedOn = Convert.ToDateTime(tblInvoiceTODT["createdOn"].ToString());
                    if (tblInvoiceTODT["updatedOn"] != DBNull.Value)
                        tblInvoiceTONew.UpdatedOn = Convert.ToDateTime(tblInvoiceTODT["updatedOn"].ToString());
                    if (tblInvoiceTODT["currencyRate"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyRate = Convert.ToDouble(tblInvoiceTODT["currencyRate"].ToString());
                    if (tblInvoiceTODT["basicAmt"] != DBNull.Value)
                        tblInvoiceTONew.BasicAmt = Convert.ToDouble(tblInvoiceTODT["basicAmt"].ToString());
                    if (tblInvoiceTODT["discountAmt"] != DBNull.Value)
                        tblInvoiceTONew.DiscountAmt = Convert.ToDouble(tblInvoiceTODT["discountAmt"].ToString());
                    if (tblInvoiceTODT["taxableAmt"] != DBNull.Value)
                        tblInvoiceTONew.TaxableAmt = Convert.ToDouble(tblInvoiceTODT["taxableAmt"].ToString());
                    if (tblInvoiceTODT["cgstAmt"] != DBNull.Value)
                        tblInvoiceTONew.CgstAmt = Convert.ToDouble(tblInvoiceTODT["cgstAmt"].ToString());
                    if (tblInvoiceTODT["sgstAmt"] != DBNull.Value)
                        tblInvoiceTONew.SgstAmt = Convert.ToDouble(tblInvoiceTODT["sgstAmt"].ToString());
                    if (tblInvoiceTODT["igstAmt"] != DBNull.Value)
                        tblInvoiceTONew.IgstAmt = Convert.ToDouble(tblInvoiceTODT["igstAmt"].ToString());
                    if (tblInvoiceTODT["freightPct"] != DBNull.Value)
                        tblInvoiceTONew.FreightPct = Convert.ToDouble(tblInvoiceTODT["freightPct"].ToString());
                    if (tblInvoiceTODT["freightAmt"] != DBNull.Value)
                        tblInvoiceTONew.FreightAmt = Convert.ToDouble(tblInvoiceTODT["freightAmt"].ToString());
                    if (tblInvoiceTODT["roundOffAmt"] != DBNull.Value)
                        tblInvoiceTONew.RoundOffAmt = Convert.ToDouble(tblInvoiceTODT["roundOffAmt"].ToString());
                    if (tblInvoiceTODT["grandTotal"] != DBNull.Value)
                        tblInvoiceTONew.GrandTotal = Convert.ToDouble(tblInvoiceTODT["grandTotal"].ToString());
                    if (tblInvoiceTODT["invoiceNo"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceNo = Convert.ToString(tblInvoiceTODT["invoiceNo"].ToString());
                    if (tblInvoiceTODT["electronicRefNo"] != DBNull.Value)
                        tblInvoiceTONew.ElectronicRefNo = Convert.ToString(tblInvoiceTODT["electronicRefNo"].ToString());
                    if (tblInvoiceTODT["vehicleNo"] != DBNull.Value)
                        tblInvoiceTONew.VehicleNo = Convert.ToString(tblInvoiceTODT["vehicleNo"].ToString());
                    if (tblInvoiceTODT["lrNumber"] != DBNull.Value)
                        tblInvoiceTONew.LrNumber = Convert.ToString(tblInvoiceTODT["lrNumber"].ToString());
                    if (tblInvoiceTODT["roadPermitNo"] != DBNull.Value)
                        tblInvoiceTONew.RoadPermitNo = Convert.ToString(tblInvoiceTODT["roadPermitNo"].ToString());
                    if (tblInvoiceTODT["transportorForm"] != DBNull.Value)
                        tblInvoiceTONew.TransportorForm = Convert.ToString(tblInvoiceTODT["transportorForm"].ToString());
                    if (tblInvoiceTODT["airwayBillNo"] != DBNull.Value)
                        tblInvoiceTONew.AirwayBillNo = Convert.ToString(tblInvoiceTODT["airwayBillNo"].ToString());
                    if (tblInvoiceTODT["narration"] != DBNull.Value)
                        tblInvoiceTONew.Narration = Convert.ToString(tblInvoiceTODT["narration"].ToString());
                    if (tblInvoiceTODT["bankDetails"] != DBNull.Value)
                        tblInvoiceTONew.BankDetails = Convert.ToString(tblInvoiceTODT["bankDetails"].ToString());
                    if (tblInvoiceTODT["invoiceModeId"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceModeId = Convert.ToInt32(tblInvoiceTODT["invoiceModeId"]);

                    if (tblInvoiceTODT["dealerName"] != DBNull.Value)
                        tblInvoiceTONew.DealerName = Convert.ToString(tblInvoiceTODT["dealerName"].ToString());
                    if (tblInvoiceTODT["distributorName"] != DBNull.Value)
                        tblInvoiceTONew.DistributorName = Convert.ToString(tblInvoiceTODT["distributorName"].ToString());
                    if (tblInvoiceTODT["transporterName"] != DBNull.Value)
                        tblInvoiceTONew.TransporterName = Convert.ToString(tblInvoiceTODT["transporterName"].ToString());
                    if (tblInvoiceTODT["currencyName"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyName = Convert.ToString(tblInvoiceTODT["currencyName"].ToString());
                    if (tblInvoiceTODT["statusName"] != DBNull.Value)
                        tblInvoiceTONew.StatusName = Convert.ToString(tblInvoiceTODT["statusName"].ToString());
                    if (tblInvoiceTODT["invoiceTypeDesc"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceTypeDesc = Convert.ToString(tblInvoiceTODT["invoiceTypeDesc"].ToString());
                    if (tblInvoiceTODT["deliveryLocation"] != DBNull.Value)
                        tblInvoiceTONew.DeliveryLocation = Convert.ToString(tblInvoiceTODT["deliveryLocation"]);

                    if (tblInvoiceTODT["changeIn"] != DBNull.Value)
                        tblInvoiceTONew.ChangeIn = Convert.ToString(tblInvoiceTODT["changeIn"]);
                    if (tblInvoiceTODT["expenseAmt"] != DBNull.Value)
                        tblInvoiceTONew.ExpenseAmt = Convert.ToDouble(tblInvoiceTODT["expenseAmt"].ToString());
                    if (tblInvoiceTODT["otherAmt"] != DBNull.Value)
                        tblInvoiceTONew.OtherAmt = Convert.ToDouble(tblInvoiceTODT["otherAmt"].ToString());
                    if (tblInvoiceTODT["tareWeight"] != DBNull.Value)
                        tblInvoiceTONew.TareWeight = Convert.ToDouble(tblInvoiceTODT["tareWeight"]);
                    if (tblInvoiceTODT["netWeight"] != DBNull.Value)
                        tblInvoiceTONew.NetWeight = Convert.ToDouble(tblInvoiceTODT["netWeight"]);
                    if (tblInvoiceTODT["grossWeight"] != DBNull.Value)
                        tblInvoiceTONew.GrossWeight = Convert.ToDouble(tblInvoiceTODT["grossWeight"]);
                    if (tblInvoiceTODT["isConfirmed"] != DBNull.Value)
                        tblInvoiceTONew.IsConfirmed = Convert.ToInt32(tblInvoiceTODT["isConfirmed"]);
                    if (tblInvoiceTODT["rcmFlag"] != DBNull.Value)
                        tblInvoiceTONew.RcmFlag = Convert.ToInt32(tblInvoiceTODT["rcmFlag"]);
                    if (tblInvoiceTODT["remark"] != DBNull.Value)
                        tblInvoiceTONew.Remark = Convert.ToString(tblInvoiceTODT["remark"]);

                    if (tblInvoiceTODT["invFromOrgId"] != DBNull.Value)
                        tblInvoiceTONew.InvFromOrgId = Convert.ToInt32(tblInvoiceTODT["invFromOrgId"]);
                    if (tblInvoiceTODT["brandId"] != DBNull.Value)
                        tblInvoiceTONew.BrandId = Convert.ToInt32(tblInvoiceTODT["brandId"]);

                    //Vijaymala [2018-26-02]
                    if (tblInvoiceTODT["poNo"] != DBNull.Value)
                        tblInvoiceTONew.PoNo = Convert.ToString(tblInvoiceTODT["poNo"].ToString());

                    //Vijaymla[26-02-2018]added
                    if (tblInvoiceTODT["poDate"] != DBNull.Value)
                        tblInvoiceTONew.PoDate = Convert.ToDateTime(tblInvoiceTODT["poDate"].ToString());

                    if (tblInvoiceTODT["deliveredOn"] != DBNull.Value)
                        tblInvoiceTONew.DeliveredOn = Convert.ToDateTime(tblInvoiceTODT["deliveredOn"].ToString());

                    if (tblInvoiceTODT["orcPersonName"] != DBNull.Value)
                        tblInvoiceTONew.ORCPersonName = Convert.ToString(tblInvoiceTODT["orcPersonName"]);

                    if (tblInvoiceTODT["tableType"] != DBNull.Value)
                        tblInvoiceTONew.TranTableType = Convert.ToInt16(tblInvoiceTODT["tableType"]);

                   
                }
            }
            return tblInvoiceTONew;
        }

        public List<TblInvoiceTO> ConvertDTToList(SqlDataReader tblInvoiceTODT)
        {
            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
            if (tblInvoiceTODT != null)
            {
                while (tblInvoiceTODT.Read())
                {
                    TblInvoiceTO tblInvoiceTONew = new TblInvoiceTO();
                    if (tblInvoiceTODT["idInvoice"] != DBNull.Value)
                        tblInvoiceTONew.IdInvoice = Convert.ToInt32(tblInvoiceTODT["idInvoice"].ToString());
                    if (tblInvoiceTODT["invoiceTypeId"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceTypeId = Convert.ToInt32(tblInvoiceTODT["invoiceTypeId"].ToString());
                    if (tblInvoiceTODT["transportOrgId"] != DBNull.Value)
                        tblInvoiceTONew.TransportOrgId = Convert.ToInt32(tblInvoiceTODT["transportOrgId"].ToString());
                    if (tblInvoiceTODT["transportModeId"] != DBNull.Value)
                        tblInvoiceTONew.TransportModeId = Convert.ToInt32(tblInvoiceTODT["transportModeId"].ToString());
                    if (tblInvoiceTODT["currencyId"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyId = Convert.ToInt32(tblInvoiceTODT["currencyId"].ToString());
                    if (tblInvoiceTODT["loadingSlipId"] != DBNull.Value)
                        tblInvoiceTONew.LoadingSlipId = Convert.ToInt32(tblInvoiceTODT["loadingSlipId"].ToString());
                    if (tblInvoiceTODT["distributorOrgId"] != DBNull.Value)
                        tblInvoiceTONew.DistributorOrgId = Convert.ToInt32(tblInvoiceTODT["distributorOrgId"].ToString());
                    if (tblInvoiceTODT["dealerOrgId"] != DBNull.Value)
                        tblInvoiceTONew.DealerOrgId = Convert.ToInt32(tblInvoiceTODT["dealerOrgId"].ToString());
                    if (tblInvoiceTODT["finYearId"] != DBNull.Value)
                        tblInvoiceTONew.FinYearId = Convert.ToInt32(tblInvoiceTODT["finYearId"].ToString());
                    if (tblInvoiceTODT["statusId"] != DBNull.Value)
                        tblInvoiceTONew.StatusId = Convert.ToInt32(tblInvoiceTODT["statusId"].ToString());
                    if (tblInvoiceTODT["createdBy"] != DBNull.Value)
                        tblInvoiceTONew.CreatedBy = Convert.ToInt32(tblInvoiceTODT["createdBy"].ToString());
                    if (tblInvoiceTODT["updatedBy"] != DBNull.Value)
                        tblInvoiceTONew.UpdatedBy = Convert.ToInt32(tblInvoiceTODT["updatedBy"].ToString());
                    if (tblInvoiceTODT["invoiceDate"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceDate = Convert.ToDateTime(tblInvoiceTODT["invoiceDate"].ToString());
                    if (tblInvoiceTODT["lrDate"] != DBNull.Value)
                        tblInvoiceTONew.LrDate = Convert.ToDateTime(tblInvoiceTODT["lrDate"].ToString());
                    if (tblInvoiceTODT["statusDate"] != DBNull.Value)
                        tblInvoiceTONew.StatusDate = Convert.ToDateTime(tblInvoiceTODT["statusDate"].ToString());
                    if (tblInvoiceTODT["createdOn"] != DBNull.Value)
                        tblInvoiceTONew.CreatedOn = Convert.ToDateTime(tblInvoiceTODT["createdOn"].ToString());
                    if (tblInvoiceTODT["updatedOn"] != DBNull.Value)
                        tblInvoiceTONew.UpdatedOn = Convert.ToDateTime(tblInvoiceTODT["updatedOn"].ToString());
                    if (tblInvoiceTODT["currencyRate"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyRate = Convert.ToDouble(tblInvoiceTODT["currencyRate"].ToString());
                    if (tblInvoiceTODT["basicAmt"] != DBNull.Value)
                        tblInvoiceTONew.BasicAmt = Convert.ToDouble(tblInvoiceTODT["basicAmt"].ToString());
                    if (tblInvoiceTODT["discountAmt"] != DBNull.Value)
                        tblInvoiceTONew.DiscountAmt = Convert.ToDouble(tblInvoiceTODT["discountAmt"].ToString());
                    if (tblInvoiceTODT["taxableAmt"] != DBNull.Value)
                        tblInvoiceTONew.TaxableAmt = Convert.ToDouble(tblInvoiceTODT["taxableAmt"].ToString());
                    if (tblInvoiceTODT["cgstAmt"] != DBNull.Value)
                        tblInvoiceTONew.CgstAmt = Convert.ToDouble(tblInvoiceTODT["cgstAmt"].ToString());
                    if (tblInvoiceTODT["sgstAmt"] != DBNull.Value)
                        tblInvoiceTONew.SgstAmt = Convert.ToDouble(tblInvoiceTODT["sgstAmt"].ToString());
                    if (tblInvoiceTODT["igstAmt"] != DBNull.Value)
                        tblInvoiceTONew.IgstAmt = Convert.ToDouble(tblInvoiceTODT["igstAmt"].ToString());
                    if (tblInvoiceTODT["freightPct"] != DBNull.Value)
                        tblInvoiceTONew.FreightPct = Convert.ToDouble(tblInvoiceTODT["freightPct"].ToString());
                    if (tblInvoiceTODT["freightAmt"] != DBNull.Value)
                        tblInvoiceTONew.FreightAmt = Convert.ToDouble(tblInvoiceTODT["freightAmt"].ToString());
                    if (tblInvoiceTODT["roundOffAmt"] != DBNull.Value)
                        tblInvoiceTONew.RoundOffAmt = Convert.ToDouble(tblInvoiceTODT["roundOffAmt"].ToString());
                    if (tblInvoiceTODT["grandTotal"] != DBNull.Value)
                        tblInvoiceTONew.GrandTotal = Convert.ToDouble(tblInvoiceTODT["grandTotal"].ToString());
                    if (tblInvoiceTODT["invoiceNo"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceNo = Convert.ToString(tblInvoiceTODT["invoiceNo"].ToString());
                    if (tblInvoiceTODT["electronicRefNo"] != DBNull.Value)
                        tblInvoiceTONew.ElectronicRefNo = Convert.ToString(tblInvoiceTODT["electronicRefNo"].ToString());
                    if (tblInvoiceTODT["vehicleNo"] != DBNull.Value)
                        tblInvoiceTONew.VehicleNo = Convert.ToString(tblInvoiceTODT["vehicleNo"].ToString());
                    if (tblInvoiceTODT["lrNumber"] != DBNull.Value)
                        tblInvoiceTONew.LrNumber = Convert.ToString(tblInvoiceTODT["lrNumber"].ToString());
                    if (tblInvoiceTODT["roadPermitNo"] != DBNull.Value)
                        tblInvoiceTONew.RoadPermitNo = Convert.ToString(tblInvoiceTODT["roadPermitNo"].ToString());
                    if (tblInvoiceTODT["transportorForm"] != DBNull.Value)
                        tblInvoiceTONew.TransportorForm = Convert.ToString(tblInvoiceTODT["transportorForm"].ToString());
                    if (tblInvoiceTODT["airwayBillNo"] != DBNull.Value)
                        tblInvoiceTONew.AirwayBillNo = Convert.ToString(tblInvoiceTODT["airwayBillNo"].ToString());
                    if (tblInvoiceTODT["narration"] != DBNull.Value)
                        tblInvoiceTONew.Narration = Convert.ToString(tblInvoiceTODT["narration"].ToString());
                    if (tblInvoiceTODT["bankDetails"] != DBNull.Value)
                        tblInvoiceTONew.BankDetails = Convert.ToString(tblInvoiceTODT["bankDetails"].ToString());
                    if (tblInvoiceTODT["invoiceModeId"] != DBNull.Value)
                        tblInvoiceTONew.InvoiceModeId = Convert.ToInt32(tblInvoiceTODT["invoiceModeId"]);

                    if (tblInvoiceTODT["dealerName"] != DBNull.Value)
                        tblInvoiceTONew.DealerName = Convert.ToString(tblInvoiceTODT["dealerName"].ToString());
                    if (tblInvoiceTODT["distributorName"] != DBNull.Value)
                        tblInvoiceTONew.DistributorName = Convert.ToString(tblInvoiceTODT["distributorName"].ToString());
                    if (tblInvoiceTODT["transporterName"] != DBNull.Value)
                        tblInvoiceTONew.TransporterName = Convert.ToString(tblInvoiceTODT["transporterName"].ToString());
                    if (tblInvoiceTODT["currencyName"] != DBNull.Value)
                        tblInvoiceTONew.CurrencyName = Convert.ToString(tblInvoiceTODT["currencyName"].ToString());
                    if (tblInvoiceTODT["statusName"] != DBNull.Value)
                        tblInvoiceTONew.StatusName = Convert.ToString(tblInvoiceTODT["statusName"].ToString());
                   
                    if (tblInvoiceTODT["deliveryLocation"] != DBNull.Value)
                        tblInvoiceTONew.DeliveryLocation = Convert.ToString(tblInvoiceTODT["deliveryLocation"]);

                    if (tblInvoiceTODT["changeIn"] != DBNull.Value)
                        tblInvoiceTONew.ChangeIn = Convert.ToString(tblInvoiceTODT["changeIn"]);
                    if (tblInvoiceTODT["expenseAmt"] != DBNull.Value)
                        tblInvoiceTONew.ExpenseAmt = Convert.ToDouble(tblInvoiceTODT["expenseAmt"].ToString());
                    if (tblInvoiceTODT["otherAmt"] != DBNull.Value)
                        tblInvoiceTONew.OtherAmt = Convert.ToDouble(tblInvoiceTODT["otherAmt"].ToString());
                    if (tblInvoiceTODT["tareWeight"] != DBNull.Value)
                        tblInvoiceTONew.TareWeight = Convert.ToDouble(tblInvoiceTODT["tareWeight"]);
                    if (tblInvoiceTODT["netWeight"] != DBNull.Value)
                        tblInvoiceTONew.NetWeight = Convert.ToDouble(tblInvoiceTODT["netWeight"]);
                    if (tblInvoiceTODT["grossWeight"] != DBNull.Value)
                        tblInvoiceTONew.GrossWeight = Convert.ToDouble(tblInvoiceTODT["grossWeight"]);
                    if (tblInvoiceTODT["isConfirmed"] != DBNull.Value)
                        tblInvoiceTONew.IsConfirmed = Convert.ToInt32(tblInvoiceTODT["isConfirmed"]);
                    //Aniket [19-9-2019] modified for C and NC invoice type description
                    if(tblInvoiceTONew.IsConfirmed==1)
                    {
                        if (tblInvoiceTODT["invoiceTypeDesc"] != DBNull.Value)
                            tblInvoiceTONew.InvoiceTypeDesc = Convert.ToString(tblInvoiceTODT["invoiceTypeDesc"].ToString());
                    }
                    else
                    {
                        if (tblInvoiceTODT["invoiceTypeDesc"] != DBNull.Value)
                            tblInvoiceTONew.InvoiceTypeDesc = "Delivery Challan";
                    }

                    if (tblInvoiceTODT["rcmFlag"] != DBNull.Value)
                        tblInvoiceTONew.RcmFlag = Convert.ToInt32(tblInvoiceTODT["rcmFlag"]);
                    if (tblInvoiceTODT["remark"] != DBNull.Value)
                        tblInvoiceTONew.Remark = Convert.ToString(tblInvoiceTODT["remark"]);

                    if (tblInvoiceTODT["invFromOrgId"] != DBNull.Value)
                        tblInvoiceTONew.InvFromOrgId = Convert.ToInt32(tblInvoiceTODT["invFromOrgId"]);
                    if (tblInvoiceTODT["brandId"] != DBNull.Value)
                        tblInvoiceTONew.BrandId = Convert.ToInt32(tblInvoiceTODT["brandId"]);

                    //Vijaymala [2018-26-02]
                    if (tblInvoiceTODT["poNo"] != DBNull.Value)
                        tblInvoiceTONew.PoNo = Convert.ToString(tblInvoiceTODT["poNo"].ToString());

                    //Vijaymla[26-02-2018]added
                    if (tblInvoiceTODT["poDate"] != DBNull.Value)
                        tblInvoiceTONew.PoDate = Convert.ToDateTime(tblInvoiceTODT["poDate"].ToString());

                    if (tblInvoiceTODT["deliveredOn"] != DBNull.Value)
                        tblInvoiceTONew.DeliveredOn = Convert.ToDateTime(tblInvoiceTODT["deliveredOn"].ToString());

                    if (tblInvoiceTODT["orcPersonName"] != DBNull.Value)
                        tblInvoiceTONew.ORCPersonName = Convert.ToString(tblInvoiceTODT["orcPersonName"]);

                    if(tblInvoiceTODT["tableType"]!=DBNull.Value)
                        tblInvoiceTONew.TranTableType = Convert.ToInt16(tblInvoiceTODT["tableType"]);

                    //Aniket [06-02-2019]
                    if (tblInvoiceTODT["grossWtTakenDate"] != DBNull.Value)
                        tblInvoiceTONew.GrossWtTakenDate = Convert.ToDateTime(tblInvoiceTODT["grossWtTakenDate"].ToString());
                    //Aniket [06-02-2019]
                    if (tblInvoiceTODT["preparationDate"] != DBNull.Value)
                        tblInvoiceTONew.PreparationDate = Convert.ToDateTime(tblInvoiceTODT["preparationDate"].ToString());
                    if (tblInvoiceTODT["loadingStatusId"] != DBNull.Value)
                        tblInvoiceTONew.LoadingStatusId = Convert.ToInt32(tblInvoiceTODT["loadingStatusId"]);
                    if (tblInvoiceTODT["isDBup"] != DBNull.Value)
                        tblInvoiceTONew.IsDBup = Convert.ToInt32(tblInvoiceTODT["isDBup"]);

                    if (tblInvoiceTODT["invFromOrgFreeze"] != DBNull.Value)
                        tblInvoiceTONew.InvFromOrgFreeze = Convert.ToInt32(tblInvoiceTODT["invFromOrgFreeze"].ToString());

                    if (tblInvoiceTODT["comment"] != DBNull.Value)
                        tblInvoiceTONew.InvComment = Convert.ToString(tblInvoiceTODT["comment"].ToString());

                    //Dhananajay [19-11-2020]
                    if (tblInvoiceTODT["IrnNo"] != DBNull.Value)
                        tblInvoiceTONew.IrnNo = tblInvoiceTODT["IrnNo"].ToString();
                    if (tblInvoiceTODT["isEInvGenerated"] != DBNull.Value)
                        tblInvoiceTONew.IsEInvGenerated = Convert.ToInt32(tblInvoiceTODT["isEInvGenerated"].ToString());
                    if (tblInvoiceTODT["isEwayBillGenerated"] != DBNull.Value)
                        tblInvoiceTONew.IsEWayBillGenerated = Convert.ToInt32(tblInvoiceTODT["isEwayBillGenerated"].ToString());
                    if (tblInvoiceTODT["distanceInKM"] != DBNull.Value)
                        tblInvoiceTONew.DistanceInKM = Convert.ToDecimal(tblInvoiceTODT["distanceInKM"].ToString());

                    if (tblInvoiceTODT["tdsAmt"] != DBNull.Value)
                        tblInvoiceTONew.TdsAmt = Convert.ToDouble(tblInvoiceTODT["tdsAmt"].ToString());

                    if (tblInvoiceTODT["deliveryNoteNo"] != DBNull.Value)
                        tblInvoiceTONew.DeliveryNoteNo = Convert.ToString(tblInvoiceTODT["deliveryNoteNo"].ToString());

                    if (tblInvoiceTODT["dispatchDocNo"] != DBNull.Value)
                        tblInvoiceTONew.DispatchDocNo = Convert.ToString(tblInvoiceTODT["dispatchDocNo"].ToString());

                    if (tblInvoiceTODT["voucherClassId"] != DBNull.Value)
                        tblInvoiceTONew.VoucherClassId = Convert.ToInt32(tblInvoiceTODT["voucherClassId"].ToString());

                    if (tblInvoiceTODT["salesLedgerId"] != DBNull.Value)
                        tblInvoiceTONew.SalesLedgerId = Convert.ToInt32(tblInvoiceTODT["salesLedgerId"].ToString());
                    if (tblInvoiceTODT["brokerName"] != DBNull.Value)
                        tblInvoiceTONew.BrokerName = Convert.ToString(tblInvoiceTODT["brokerName"]);

                    tblInvoiceTOList.Add(tblInvoiceTONew);
                }
            }
            return tblInvoiceTOList;
        }
        /// <summary>
        /// Vijaymala[15-09-2017] Added To Get Invoice List To Generate Report //for nc
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm,int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                    " Select invoice.netWeight, invoice.tareWeight, invoice.grossWeight, invoice.invFromOrgId,invoice.idInvoice,invoice.invoiceNo,invoice.narration,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn,   " +
                           " invoiceAddress.billingName as partyName, org.firmName cnfName,  " +
                           "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                           "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
                          "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,taxRate.taxTypeId , " +
                            " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal, " +
                            " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,orgInv.enq_ref_id,tblItemTallyRefDtls.enquiryTallyRefId" +
                            " ,invoice.deliveredOn FROM tempInvoice invoice " +
                            " INNER JOIN tempInvoiceAddress invoiceAddress " +
                            " ON invoiceAddress.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN tblOrganization orgInv on orgInv.idOrganization = invoiceAddress.billingOrgId" +
                            " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
                            " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN tblProdGstCodeDtls ON  tblProdGstCodeDtls.idProdGstCode =  itemDetails.prodGstCodeId" +
                            " LEFT JOIN tblItemTallyRefDtls ON ISNULL(tblProdGstCodeDtls.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                            " ISNULL(tblProdGstCodeDtls.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                            " ISNULL(tblProdGstCodeDtls.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                            " ISNULL(tblProdGstCodeDtls.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                            " AND tblitemtallyrefDtls.isActive = 1" +
                            " LEFT JOIN tempLoadingSlipExt lExt " +
                            " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                            " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
                            " LEFT JOIN tempInvoiceItemTaxDtls itemTaxDetails " +
                            " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
                            " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId " +
                          


                // Vaibhav [10-Jan-2018] Added to select from finalInvoice.

                " UNION ALL " +
                            " Select invoice.netWeight, invoice.tareWeight, invoice.grossWeight, invoice.invFromOrgId,invoice.idInvoice,invoice.invoiceNo,invoice.narration,invoice.statusDate ,invoice.vehicleNo,invoice.invoiceDate,invoice.createdOn,   " +
                           " invoiceAddress.billingName as partyName, org.firmName cnfName,  " +
                           "  booking.bookingRate,itemDetails.idInvoiceItem as invoiceItemId,  " +
                           "  itemDetails.prodItemDesc, itemDetails.bundles,itemDetails.rate, " +
                          "   itemDetails.cdStructure,itemDetails.cdAmt,itemDetails.otherTaxId,itemTaxDetails.taxRatePct ,taxRate.taxTypeId , " +
                            " invoice.freightAmt,itemDetails.invoiceQty,itemDetails.basicTotal as taxableAmt  , itemTaxDetails.taxAmt,itemDetails.grandTotal, " +
                            " invoice.isConfirmed,invoiceAddress.txnAddrTypeId,invoice.statusId,orgInv.enq_ref_id,tblItemTallyRefDtls.enquiryTallyRefId " +
                            " ,invoice.deliveredOn  FROM finalInvoice invoice " +
                            " INNER JOIN finalInvoiceAddress invoiceAddress " +
                            " ON invoiceAddress.invoiceId = invoice.idInvoice " +
                            " LEFT JOIN tblOrganization orgInv on orgInv.idOrganization = invoiceAddress.billingOrgId" +
                            " INNER JOIN tblOrganization org   ON org.idOrganization = invoice.distributorOrgId " +
                            " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                             " LEFT JOIN tblProdGstCodeDtls ON  tblProdGstCodeDtls.idProdGstCode =  itemDetails.prodGstCodeId" +
                            " LEFT JOIN tblItemTallyRefDtls ON ISNULL(tblProdGstCodeDtls.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                            " ISNULL(tblProdGstCodeDtls.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                            " ISNULL(tblProdGstCodeDtls.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                            " ISNULL(tblProdGstCodeDtls.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                            " AND tblitemtallyrefDtls.isActive = 1" +
                            " LEFT JOIN finalLoadingSlipExt lExt " +
                            " ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                            " LEFT JOIN tblBookings booking  ON lExt.bookingId = booking.idBooking " +
                            " LEFT JOIN finalInvoiceItemTaxDtls itemTaxDetails " +
                            " ON itemTaxDetails.invoiceItemId = itemDetails.idInvoiceItem " +
                            " LEFT JOIN tblTaxRates taxRate  ON taxRate.idTaxRate = itemTaxDetails.taxRateId ";
                           

                //chetan[13-feb-2020] added for get data from org Id
                String formOrgIdCondtion = String.Empty;
                if (fromOrgId > 0)
                {
                    formOrgIdCondtion = " AND sq1.invFromOrgId = " + fromOrgId;
                }

                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     //" AND CAST(sq1.deliveredOn  AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND CAST(sq1.statusDate  AS DATETIME) BETWEEN @fromDate AND @toDate" +
                     " AND sq1.txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + formOrgIdCondtion+
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED
                     //+ " order by sq1.deliveredOn asc";
                     + " order by sq1.invoiceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
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

        /// <summary>
        /// Vijaymala[15-09-2017] Added This method to convert dt to rpt invoice List
        /// </summary>
        /// <param name="tblInvoiceRptTODT"></param>
        /// <returns></returns>
        public List<TblInvoiceRptTO> ConvertDTToListForRPTInvoice(SqlDataReader tblInvoiceRptTODT)
        {
            List<TblInvoiceRptTO> tblInvoiceRPtTOList = new List<TblInvoiceRptTO>();
            try
            {
                if (tblInvoiceRptTODT != null)
                {

                    while (tblInvoiceRptTODT.Read())
                    {
                        TblInvoiceRptTO tblInvoiceRptTONew = new TblInvoiceRptTO();
                        for (int i = 0; i < tblInvoiceRptTODT.FieldCount; i++)
                        {
                            if (tblInvoiceRptTODT.GetName(i).Equals("mode"))
                            {
                                if (tblInvoiceRptTODT["mode"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceMode = Convert.ToString(tblInvoiceRptTODT["mode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("idInvoice"))
                            {
                                if (tblInvoiceRptTODT["idInvoice"] != DBNull.Value)
                                    tblInvoiceRptTONew.IdInvoice = Convert.ToInt32(tblInvoiceRptTODT["idInvoice"].ToString());
                            }
                         
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceNo"))
                            {
                                if (tblInvoiceRptTODT["invoiceNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceNo = Convert.ToString(tblInvoiceRptTODT["invoiceNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("vehicleNo"))
                            {
                                if (tblInvoiceRptTODT["vehicleNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.VehicleNo = Convert.ToString(tblInvoiceRptTODT["vehicleNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceDate"))
                            {
                                if (tblInvoiceRptTODT["invoiceDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceDate = Convert.ToDateTime(tblInvoiceRptTODT["invoiceDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("electronicRefNo"))
                            {
                                if (tblInvoiceRptTODT["electronicRefNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.ElectronicRefNo = Convert.ToString(tblInvoiceRptTODT["electronicRefNo"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceTypeDesc"))
                            {
                                if (tblInvoiceRptTODT["invoiceTypeDesc"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceTypeDesc = Convert.ToString(tblInvoiceRptTODT["invoiceTypeDesc"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("IrnNo"))
                            {
                                if (tblInvoiceRptTODT["IrnNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.IrnNo = Convert.ToString(tblInvoiceRptTODT["IrnNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("codeNumber"))
                            {
                                if (tblInvoiceRptTODT["codeNumber"] != DBNull.Value)
                                    tblInvoiceRptTONew.CodeNumber = Convert.ToString(tblInvoiceRptTODT["codeNumber"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("GST_Code_No"))
                            {
                                if (tblInvoiceRptTODT["GST_Code_No"] != DBNull.Value)
                                    tblInvoiceRptTONew.GstCodeNo = Convert.ToString(tblInvoiceRptTODT["GST_Code_No"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("partyName"))
                            {
                                if (tblInvoiceRptTODT["partyName"] != DBNull.Value)
                                    tblInvoiceRptTONew.PartyName = Convert.ToString(tblInvoiceRptTODT["partyName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cnfName"))
                            {
                                if (tblInvoiceRptTODT["cnfName"] != DBNull.Value)
                                    tblInvoiceRptTONew.CnfName = Convert.ToString(tblInvoiceRptTODT["cnfName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("bookingRate"))
                            {
                                if (tblInvoiceRptTODT["bookingRate"] != DBNull.Value)
                                    tblInvoiceRptTONew.BookingRate = Convert.ToDouble(tblInvoiceRptTODT["bookingRate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceItemId"))
                            {
                                if (tblInvoiceRptTODT["invoiceItemId"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceItemId = Convert.ToInt32(tblInvoiceRptTODT["invoiceItemId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("prodItemDesc"))
                            {
                                if (tblInvoiceRptTODT["prodItemDesc"] != DBNull.Value)
                                    tblInvoiceRptTONew.ProdItemDesc = Convert.ToString(tblInvoiceRptTODT["prodItemDesc"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("bundles"))
                            {
                                if (tblInvoiceRptTODT["bundles"] != DBNull.Value)
                                    tblInvoiceRptTONew.Bundles = Convert.ToString(tblInvoiceRptTODT["bundles"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("tareWeight"))
                            {
                                if (tblInvoiceRptTODT["tareWeight"] != DBNull.Value)
                                    tblInvoiceRptTONew.TareWeight = Convert.ToDouble(tblInvoiceRptTODT["tareWeight"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("igstPct"))
                            {
                                if (tblInvoiceRptTODT["igstPct"] != DBNull.Value)
                                    tblInvoiceRptTONew.IgstPct = Convert.ToDouble(tblInvoiceRptTODT["igstPct"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("sgstPct"))
                            {
                                if (tblInvoiceRptTODT["sgstPct"] != DBNull.Value)
                                    tblInvoiceRptTONew.SgstPct = Convert.ToDouble(tblInvoiceRptTODT["sgstPct"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cgstPct"))
                            {
                                if (tblInvoiceRptTODT["cgstPct"] != DBNull.Value)
                                    tblInvoiceRptTONew.CgstPct = Convert.ToDouble(tblInvoiceRptTODT["cgstPct"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("grossWeight"))
                            {
                                if (tblInvoiceRptTODT["grossWeight"] != DBNull.Value)
                                    tblInvoiceRptTONew.GrossWeight = Convert.ToDouble(tblInvoiceRptTODT["grossWeight"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("netWeight"))
                            {
                                if (tblInvoiceRptTODT["netWeight"] != DBNull.Value)
                                    tblInvoiceRptTONew.NetWeight = Convert.ToDouble(tblInvoiceRptTODT["netWeight"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("roundOffAmt"))
                            {
                                if (tblInvoiceRptTODT["roundOffAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.RoundOffAmt = Convert.ToDouble(tblInvoiceRptTODT["roundOffAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("rate"))
                            {
                                if (tblInvoiceRptTODT["rate"] != DBNull.Value)
                                    tblInvoiceRptTONew.Rate = Convert.ToDouble(tblInvoiceRptTODT["rate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cdStructure"))
                            {
                                if (tblInvoiceRptTODT["cdStructure"] != DBNull.Value)
                                    tblInvoiceRptTONew.CdStructure = Convert.ToDouble(tblInvoiceRptTODT["cdStructure"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("orcAmt"))
                            {
                                if (tblInvoiceRptTODT["orcAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrcAmt = Convert.ToDouble(tblInvoiceRptTODT["orcAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cdAmt"))
                            {
                                if (tblInvoiceRptTODT["cdAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.CdAmt = Convert.ToDouble(tblInvoiceRptTODT["cdAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxRatePct"))
                            {
                                if (tblInvoiceRptTODT["taxRatePct"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxRatePct = Convert.ToDouble(tblInvoiceRptTODT["taxRatePct"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxTypeId"))
                            {
                                if (tblInvoiceRptTODT["taxTypeId"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxTypeId = Convert.ToInt32(tblInvoiceRptTODT["taxTypeId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("freightAmt"))
                            {
                                if (tblInvoiceRptTODT["freightAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.FreightAmt = Convert.ToDouble(tblInvoiceRptTODT["freightAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("tcsAmt"))
                            {
                                if (tblInvoiceRptTODT["tcsAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TcsAmt = Convert.ToDouble(tblInvoiceRptTODT["tcsAmt"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceQty"))
                            {
                                if (tblInvoiceRptTODT["invoiceQty"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceQty = Convert.ToDecimal(tblInvoiceRptTODT["invoiceQty"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxableAmt"))
                            {
                                if (tblInvoiceRptTODT["taxableAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxableAmt = Convert.ToDouble(tblInvoiceRptTODT["taxableAmt"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("tcsAmt"))
                            {
                                if (tblInvoiceRptTODT["tcsAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TcsAmt = Convert.ToDouble(tblInvoiceRptTODT["tcsAmt"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("taxAmt"))
                            {
                                if (tblInvoiceRptTODT["taxAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxAmt = Convert.ToDouble(tblInvoiceRptTODT["taxAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("billingTypeId"))
                            {
                                if (tblInvoiceRptTODT["billingTypeId"] != DBNull.Value)
                                    tblInvoiceRptTONew.BillingTypeId = Convert.ToInt32(tblInvoiceRptTODT["billingTypeId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyer"))
                            {
                                if (tblInvoiceRptTODT["buyer"] != DBNull.Value)
                                    tblInvoiceRptTONew.Buyer = Convert.ToString(tblInvoiceRptTODT["buyer"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgstateName"))
                            {
                                if (tblInvoiceRptTODT["OrgstateName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgstateName = Convert.ToString(tblInvoiceRptTODT["OrgstateName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgvillageName"))
                            {
                                if (tblInvoiceRptTODT["OrgcountryName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgcountryName = Convert.ToString(tblInvoiceRptTODT["OrgcountryName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgvillageName"))
                            {
                                if (tblInvoiceRptTODT["OrgvillageName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgvillageName = Convert.ToString(tblInvoiceRptTODT["OrgvillageName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgdistrictName"))
                            {
                                if (tblInvoiceRptTODT["OrgdistrictName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgdistrictName = Convert.ToString(tblInvoiceRptTODT["OrgdistrictName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invFromOrgName"))
                            {
                                if (tblInvoiceRptTODT["invFromOrgName"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvFromOrgName = Convert.ToString(tblInvoiceRptTODT["invFromOrgName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgareaName"))
                            {
                                if (tblInvoiceRptTODT["OrgareaName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgareaName = Convert.ToString(tblInvoiceRptTODT["OrgareaName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("Orgpincode"))
                            {
                                if (tblInvoiceRptTODT["Orgpincode"] != DBNull.Value)
                                    tblInvoiceRptTONew.Orgpincode = Convert.ToString(tblInvoiceRptTODT["Orgpincode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("OrgplotNo"))
                            {
                                if (tblInvoiceRptTODT["OrgplotNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgplotNo = Convert.ToString(tblInvoiceRptTODT["OrgplotNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerGstNo"))
                            {
                                if (tblInvoiceRptTODT["buyerGstNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerGstNo = Convert.ToString(tblInvoiceRptTODT["buyerGstNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerAddress"))
                            {
                                if (tblInvoiceRptTODT["buyerAddress"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerAddress = Convert.ToString(tblInvoiceRptTODT["buyerAddress"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerDistrict"))
                            {
                                if (tblInvoiceRptTODT["buyerDistrict"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerDistrict = Convert.ToString(tblInvoiceRptTODT["buyerDistrict"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerPinCode"))
                            {
                                if (tblInvoiceRptTODT["buyerPinCode"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerPinCode = Convert.ToString(tblInvoiceRptTODT["buyerPinCode"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerTaluka"))
                            {
                                if (tblInvoiceRptTODT["buyerTaluka"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerTaluka = Convert.ToString(tblInvoiceRptTODT["buyerTaluka"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeTypeId"))
                            {
                                if (tblInvoiceRptTODT["consigneeTypeId"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeTypeId = Convert.ToInt32(tblInvoiceRptTODT["consigneeTypeId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consignee"))
                            {
                                if (tblInvoiceRptTODT["consignee"] != DBNull.Value)
                                    tblInvoiceRptTONew.Consignee = Convert.ToString(tblInvoiceRptTODT["consignee"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeGstNo"))
                            {
                                if (tblInvoiceRptTODT["consigneeGstNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeGstNo = Convert.ToString(tblInvoiceRptTODT["consigneeGstNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveryLocation"))
                            {
                                if (tblInvoiceRptTODT["deliveryLocation"] != DBNull.Value)
                                    tblInvoiceRptTONew.DeliveryLocation = Convert.ToString(tblInvoiceRptTODT["deliveryLocation"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i) == "basicAmt")
                            {
                                if (tblInvoiceRptTODT["basicAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.BasicAmt = Convert.ToDouble(tblInvoiceRptTODT["basicAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cgstAmt"))
                            {
                                if (tblInvoiceRptTODT["cgstAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.CgstTaxAmt = Convert.ToDouble(tblInvoiceRptTODT["cgstAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("sgstAmt"))
                            {
                                if (tblInvoiceRptTODT["sgstAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.SgstTaxAmt = Convert.ToDouble(tblInvoiceRptTODT["sgstAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("igstAmt"))
                            {
                                if (tblInvoiceRptTODT["igstAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.IgstTaxAmt = Convert.ToDouble(tblInvoiceRptTODT["igstAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("grandTotal"))
                            {
                                if (tblInvoiceRptTODT["grandTotal"] != DBNull.Value)
                                    tblInvoiceRptTONew.GrandTotal = Convert.ToDouble(tblInvoiceRptTODT["grandTotal"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("createdOn"))
                            {
                                if (tblInvoiceRptTODT["createdOn"] != DBNull.Value)
                                    tblInvoiceRptTONew.CreatedOn = Convert.ToDateTime(tblInvoiceRptTODT["createdOn"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("gstinCodeNo"))
                            {
                                if (tblInvoiceRptTODT["gstinCodeNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.GstinCodeNo = Convert.ToInt32(tblInvoiceRptTODT["gstinCodeNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("stateId"))
                            {
                                if (tblInvoiceRptTODT["stateId"] != DBNull.Value)
                                    tblInvoiceRptTONew.StateId = Convert.ToInt32(tblInvoiceRptTODT["stateId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("isConfirmed"))
                            {
                                if (tblInvoiceRptTODT["isConfirmed"] != DBNull.Value)
                                    tblInvoiceRptTONew.IsConfirmed = Convert.ToInt32(tblInvoiceRptTODT["isConfirmed"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("otherTaxId"))
                            {
                                if (tblInvoiceRptTODT["otherTaxId"] != DBNull.Value)
                                    tblInvoiceRptTONew.OtherTaxId = Convert.ToInt32(tblInvoiceRptTODT["otherTaxId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusId"))
                            {
                                if (tblInvoiceRptTODT["statusId"] != DBNull.Value)
                                    tblInvoiceRptTONew.StatusId = Convert.ToInt32(tblInvoiceRptTODT["statusId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("stateOrUTCode"))
                            {
                                if (tblInvoiceRptTODT["stateOrUTCode"] != DBNull.Value)
                                    tblInvoiceRptTONew.StateOrUTCode = Convert.ToInt32(tblInvoiceRptTODT["stateOrUTCode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusDate"))
                            {
                                if (tblInvoiceRptTODT["statusDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.StatusDate = Convert.ToDateTime(tblInvoiceRptTODT["statusDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("narration"))
                            {
                                if (tblInvoiceRptTODT["narration"] != DBNull.Value)
                                    tblInvoiceRptTONew.Narration = Convert.ToString(tblInvoiceRptTODT["narration"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("buyerState"))
                            {
                                if (tblInvoiceRptTODT["buyerState"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerState = Convert.ToString(tblInvoiceRptTODT["buyerState"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeAddress"))
                            {
                                if (tblInvoiceRptTODT["consigneeAddress"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeAddress = Convert.ToString(tblInvoiceRptTODT["consigneeAddress"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeDistict"))
                            {
                                if (tblInvoiceRptTODT["consigneeDistict"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeDistict = Convert.ToString(tblInvoiceRptTODT["consigneeDistict"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneePinCode"))
                            {
                                if (tblInvoiceRptTODT["consigneePinCode"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneePinCode = Convert.ToString(tblInvoiceRptTODT["consigneePinCode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeState"))
                            {
                                if (tblInvoiceRptTODT["consigneeState"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeState = Convert.ToString(tblInvoiceRptTODT["consigneeState"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("materialName"))
                            {
                                if (tblInvoiceRptTODT["materialName"] != DBNull.Value)
                                    tblInvoiceRptTONew.MaterialName = Convert.ToString(tblInvoiceRptTODT["materialName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("transporterName"))
                            {
                                if (tblInvoiceRptTODT["transporterName"] != DBNull.Value)
                                    tblInvoiceRptTONew.TransporterName = Convert.ToString(tblInvoiceRptTODT["transporterName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("contactNo"))
                            {
                                if (tblInvoiceRptTODT["contactNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.ContactNo = Convert.ToString(tblInvoiceRptTODT["contactNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusDate"))
                            {
                                if (tblInvoiceRptTODT["statusDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.StatusDate = Convert.ToDateTime(tblInvoiceRptTODT["statusDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cnfMobNo"))
                            {
                                if (tblInvoiceRptTODT["cnfMobNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.CnfMobNo = Convert.ToString(tblInvoiceRptTODT["cnfMobNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("dealerMobNo"))
                            {
                                if (tblInvoiceRptTODT["dealerMobNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.DealerMobNo = Convert.ToString(tblInvoiceRptTODT["dealerMobNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("lrNumber"))
                            {
                                if (tblInvoiceRptTODT["lrNumber"] != DBNull.Value)
                                    tblInvoiceRptTONew.LrNumber = Convert.ToString(tblInvoiceRptTODT["lrNumber"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("lrDate"))
                            {
                                if (tblInvoiceRptTODT["lrDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.LrDate = Convert.ToDateTime(tblInvoiceRptTODT["lrDate"].ToString());
                            }

                            // Vaibhav [24-Jan-2018]  Select loadingId
                            if (tblInvoiceRptTODT.GetName(i).Equals("loadingId"))
                            {
                                if (tblInvoiceRptTODT["loadingId"] != DBNull.Value)
                                    tblInvoiceRptTONew.LoadingId = Convert.ToInt32(tblInvoiceRptTODT["loadingId"].ToString());
                            }

                            //Vijaymala Added[14-03-2017]
                            if (tblInvoiceRptTODT.GetName(i).Equals("enq_ref_id"))
                            {
                                if (tblInvoiceRptTODT["enq_ref_id"] != DBNull.Value)
                                    tblInvoiceRptTONew.Enq_ref_id = Convert.ToString(tblInvoiceRptTODT["enq_ref_id"].ToString());
                            }

                            //Vijaymala Added[14-03-2017]
                            if (tblInvoiceRptTODT.GetName(i).Equals("overdue_ref_id"))
                            {
                                if (tblInvoiceRptTODT["overdue_ref_id"] != DBNull.Value)
                                    tblInvoiceRptTONew.Overdue_ref_id = Convert.ToString(tblInvoiceRptTODT["overdue_ref_id"].ToString());
                            }
                            

                                 //Vijaymala Added[14-03-2017]
                            if (tblInvoiceRptTODT.GetName(i).Equals("buyer_overdue_ref_id"))
                            {
                                if (tblInvoiceRptTODT["buyer_overdue_ref_id"] != DBNull.Value)
                                    tblInvoiceRptTONew.Buyer_overdue_ref_id = Convert.ToString(tblInvoiceRptTODT["buyer_overdue_ref_id"].ToString());
                            }
                            //Vijaymala Added[16-03-2017]
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceTaxableAmt"))
                            {
                                if (tblInvoiceRptTODT["invoiceTaxableAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceTaxableAmt = Convert.ToDouble(tblInvoiceRptTODT["invoiceTaxableAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceDiscountAmt"))
                            {
                                if (tblInvoiceRptTODT["invoiceDiscountAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.InvoiceDiscountAmt = Convert.ToDouble(tblInvoiceRptTODT["invoiceDiscountAmt"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("enquiryTallyRefId"))
                            {
                                if (tblInvoiceRptTODT["enquiryTallyRefId"] != DBNull.Value)
                                    tblInvoiceRptTONew.EnquiryTallyRefId = Convert.ToString(tblInvoiceRptTODT["enquiryTallyRefId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("overdueTallyRefId"))
                            {
                                if (tblInvoiceRptTODT["overdueTallyRefId"] != DBNull.Value)
                                    tblInvoiceRptTONew.OverdueTallyRefId = Convert.ToString(tblInvoiceRptTODT["overdueTallyRefId"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveredOn"))
                            {
                                if (tblInvoiceRptTODT["deliveredOn"] != DBNull.Value)
                                    tblInvoiceRptTONew.DeliveredOn = Convert.ToDateTime(tblInvoiceRptTODT["deliveredOn"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("salesEngineer"))
                            {
                                if (tblInvoiceRptTODT["salesEngineer"] != DBNull.Value)
                                    tblInvoiceRptTONew.SalesEngineer = Convert.ToString(tblInvoiceRptTODT["salesEngineer"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("loadingQty"))
                            {
                                if (tblInvoiceRptTODT["loadingQty"] != DBNull.Value)
                                    tblInvoiceRptTONew.LoadingQty = Convert.ToDouble(tblInvoiceRptTODT["loadingQty"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("OrcMeasure"))
                            {
                                if (tblInvoiceRptTODT["OrcMeasure"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrcMeasure = Convert.ToString(tblInvoiceRptTODT["OrcMeasure"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("TotalItemQty"))
                            {
                                if (tblInvoiceRptTODT["TotalItemQty"] != DBNull.Value)
                                    tblInvoiceRptTONew.TotalItemQty = Convert.ToDouble(tblInvoiceRptTODT["TotalItemQty"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("firstName"))
                            {
                                if (tblInvoiceRptTODT["firstName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OwnerPersonFirstName = Convert.ToString(tblInvoiceRptTODT["firstName"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("lastName"))
                            {
                                if (tblInvoiceRptTODT["lastName"] != DBNull.Value)
                                    tblInvoiceRptTONew.OwnerPersonLastName = Convert.ToString(tblInvoiceRptTODT["lastName"].ToString());
                            }
                            //Added by minal 06 Aug 2021 for Item wise Sales Export C
                            //if (tblInvoiceRptTODT.GetName(i).Equals("buyerAddress"))
                            //{
                            //    if (tblInvoiceRptTODT["buyerAddress"] != DBNull.Value)
                            //        tblInvoiceRptTONew.BuyersAddressLine1 = Convert.ToString(tblInvoiceRptTODT["buyerAddress"].ToString());
                            //}
                            //if (tblInvoiceRptTODT.GetName(i).Equals("buyerDistrict"))
                            //{
                            //    if (tblInvoiceRptTODT["buyerDistrict"] != DBNull.Value)
                            //        tblInvoiceRptTONew.BuyersAddressLine2 = Convert.ToString(tblInvoiceRptTODT["buyerDistrict"].ToString());
                            //}
                            //if (tblInvoiceRptTODT.GetName(i).Equals("buyerPincode"))
                            //{
                            //    if (tblInvoiceRptTODT["buyerPincode"] != DBNull.Value)
                            //        tblInvoiceRptTONew.BuyersAddressLine3 = Convert.ToString(tblInvoiceRptTODT["buyerPincode"].ToString());
                            //}
                            if (tblInvoiceRptTODT.GetName(i).Equals("panNo"))
                            {
                                if (tblInvoiceRptTODT["panNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.PanNo = Convert.ToString(tblInvoiceRptTODT["panNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("dealers"))
                            {
                                if (tblInvoiceRptTODT["dealers"] != DBNull.Value)
                                    tblInvoiceRptTONew.Dealers = Convert.ToString(tblInvoiceRptTODT["dealers"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("orderNoandDate"))
                            {
                                if (tblInvoiceRptTODT["orderNoandDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrderNoandDate = Convert.ToString(tblInvoiceRptTODT["orderNoandDate"].ToString());
                            }                          
                            if (tblInvoiceRptTODT.GetName(i).Equals("prodCateDesc"))
                            {
                                if (tblInvoiceRptTODT["prodCateDesc"] != DBNull.Value)
                                    tblInvoiceRptTONew.ProdCateDesc = Convert.ToString(tblInvoiceRptTODT["prodCateDesc"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("basicRate"))
                            {
                                if (tblInvoiceRptTODT["basicRate"] != DBNull.Value)
                                    tblInvoiceRptTONew.BasicRate = Convert.ToDouble(tblInvoiceRptTODT["basicRate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("loadingSlipDate"))
                            {
                                if (tblInvoiceRptTODT["loadingSlipDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.LoadingSlipDate = Convert.ToString(tblInvoiceRptTODT["loadingSlipDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("transactionDate"))
                            {
                                if (tblInvoiceRptTODT["transactionDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.TransactionDate = Convert.ToString(tblInvoiceRptTODT["transactionDate"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveryNoteNo"))
                            {
                                if (tblInvoiceRptTODT["deliveryNoteNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.DeliveryNoteAndNo = Convert.ToString(tblInvoiceRptTODT["deliveryNoteNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("dispatchDocNo"))
                            {
                                if (tblInvoiceRptTODT["dispatchDocNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.DispatchDocNo = Convert.ToString(tblInvoiceRptTODT["dispatchDocNo"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("voucherClass"))
                            {
                                if (tblInvoiceRptTODT["voucherClass"] != DBNull.Value)
                                    tblInvoiceRptTONew.VoucherClass = Convert.ToString(tblInvoiceRptTODT["voucherClass"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("paymentTerm"))
                            {
                                if (tblInvoiceRptTODT["paymentTerm"] != DBNull.Value)
                                    tblInvoiceRptTONew.PaymentTerms = Convert.ToString(tblInvoiceRptTODT["paymentTerm"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("termOfDelivery"))
                            {
                                if (tblInvoiceRptTODT["termOfDelivery"] != DBNull.Value)
                                    tblInvoiceRptTONew.TermsofDelivery = Convert.ToString(tblInvoiceRptTODT["termOfDelivery"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("salesLedgerName"))
                            {
                                if (tblInvoiceRptTODT["salesLedgerName"] != DBNull.Value)
                                    tblInvoiceRptTONew.SalesLedger = Convert.ToString(tblInvoiceRptTODT["salesLedgerName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("orgGstNo"))
                            {                                
                                if (tblInvoiceRptTODT["orgGstNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.OrgGstNo = Convert.ToString(tblInvoiceRptTODT["orgGstNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("InsuranceAmt"))
                            {
                                if (tblInvoiceRptTODT["InsuranceAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.InsuranceAmt = Convert.ToDouble(tblInvoiceRptTODT["InsuranceAmt"].ToString());
                            }

                            //Added by Samadhan 30 June 2022 for Item wise Sales Export C
                            if (tblInvoiceRptTODT.GetName(i).Equals("BuyerAddress"))
                            {
                                if (tblInvoiceRptTODT["BuyerAddress"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerAddress = Convert.ToString(tblInvoiceRptTODT["BuyerAddress"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("BuyerTaluka"))
                            {
                                if (tblInvoiceRptTODT["BuyerTaluka"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerTaluka = Convert.ToString(tblInvoiceRptTODT["BuyerTaluka"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("BuyerDistict"))
                            {
                                if (tblInvoiceRptTODT["BuyerDistict"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerDistict = Convert.ToString(tblInvoiceRptTODT["BuyerDistict"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("BuyerPincode"))
                            {
                                if (tblInvoiceRptTODT["BuyerPincode"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyerPincode = Convert.ToString(tblInvoiceRptTODT["BuyerPincode"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("BuyercountryName"))
                            {
                                if (tblInvoiceRptTODT["BuyercountryName"] != DBNull.Value)
                                    tblInvoiceRptTONew.BuyercountryName = Convert.ToString(tblInvoiceRptTODT["BuyercountryName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneecountryName"))
                            {
                                if (tblInvoiceRptTODT["consigneecountryName"] != DBNull.Value)
                                    tblInvoiceRptTONew.consigneecountryName = Convert.ToString(tblInvoiceRptTODT["consigneecountryName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("consigneeTaluka"))
                            {
                                if (tblInvoiceRptTODT["consigneeTaluka"] != DBNull.Value)
                                    tblInvoiceRptTONew.ConsigneeTaluka = Convert.ToString(tblInvoiceRptTODT["consigneeTaluka"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("TaxPCT"))
                            {
                                if (tblInvoiceRptTODT["TaxPCT"] != DBNull.Value)
                                    tblInvoiceRptTONew.TaxPCT = Convert.ToString(tblInvoiceRptTODT["TaxPCT"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("roundOffAmt"))
                            {
                                if (tblInvoiceRptTODT["roundOffAmt"] != DBNull.Value)
                                    tblInvoiceRptTONew.roundOffAmt = Convert.ToString(tblInvoiceRptTODT["roundOffAmt"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("CDType"))
                            {
                                if (tblInvoiceRptTODT["CDType"] != DBNull.Value)
                                    tblInvoiceRptTONew.CDType = Convert.ToString(tblInvoiceRptTODT["CDType"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("AckNo"))
                            {
                                if (tblInvoiceRptTODT["AckNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.AckNo = Convert.ToString(tblInvoiceRptTODT["AckNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("AckDate"))
                            {
                                if (tblInvoiceRptTODT["AckDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.AckDate = Convert.ToString(tblInvoiceRptTODT["AckDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("EwbNo"))
                            {
                                if (tblInvoiceRptTODT["EwbNo"] != DBNull.Value)
                                    tblInvoiceRptTONew.EwbNo = Convert.ToString(tblInvoiceRptTODT["EwbNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("EwbDate"))
                            {
                                if (tblInvoiceRptTODT["EwbDate"] != DBNull.Value)
                                    tblInvoiceRptTONew.EwbDate = Convert.ToString(tblInvoiceRptTODT["EwbDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("ProDesc"))
                            {
                                if (tblInvoiceRptTODT["ProDesc"] != DBNull.Value)
                                    tblInvoiceRptTONew.ProDesc = Convert.ToString(tblInvoiceRptTODT["ProDesc"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("TransactioDateNew"))
                            {
                                if (tblInvoiceRptTODT["TransactioDateNew"] != DBNull.Value)
                                    tblInvoiceRptTONew.TransactioDateNew = Convert.ToString(tblInvoiceRptTODT["TransactioDateNew"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("Condition"))
                            {
                                if (tblInvoiceRptTODT["Condition"] != DBNull.Value)
                                    tblInvoiceRptTONew.Condition = Convert.ToString(tblInvoiceRptTODT["Condition"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("Freight_GL"))
                            {
                                if (tblInvoiceRptTODT["Freight_GL"] != DBNull.Value)
                                    tblInvoiceRptTONew.Freight_GL = Convert.ToString(tblInvoiceRptTODT["Freight_GL"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("Insurance_GL"))
                            {
                                if (tblInvoiceRptTODT["Insurance_GL"] != DBNull.Value)
                                    tblInvoiceRptTONew.Insurance_GL = Convert.ToString(tblInvoiceRptTODT["Insurance_GL"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("TCS_GL"))
                            {
                                if (tblInvoiceRptTODT["TCS_GL"] != DBNull.Value)
                                    tblInvoiceRptTONew.TCS_GL = Convert.ToString(tblInvoiceRptTODT["TCS_GL"].ToString());
                            }



                            tblInvoiceRptTONew.ContactName = ""+tblInvoiceRptTONew.OwnerPersonFirstName +"  "+  tblInvoiceRptTONew.OwnerPersonLastName+"";
                        }

                        tblInvoiceRPtTOList.Add(tblInvoiceRptTONew);

                    }
                }
                // return tblInvoiceTOList;
                return tblInvoiceRPtTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }



        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();

                selectQuery = "select distinct  tblBookings.bookingRate,invoice.invFromOrgId,invoice.dealerOrgId,invoice.idInvoice,invoice.statusDate,invoice.invoiceDate,invoice.invoiceNo," +
                    " invAddrBill.txnAddrTypeId as billingTypeId,  invAddrBill.billingName + CASE WHEN invAddrBill.village IS NULL THEN '-' Else case WHEN invAddrBill.village IS NOT NULL THEN" +
                    " ',' + invAddrBill.village END END as buyer, invAddrBill.gstinNo as buyerGstNo,invoice.firmName as salesEngineer, invAddrBill.stateId,invAddrCons.txnAddrTypeId as consigneeTypeId, invAddrCons.billingName as consignee," +
                    " invAddrCons.gstinNo as consigneeGstNo,invoice.deliveryLocation , invoiceItem.invoiceQty ,basicAmt,discountAmt as cdAmt, invoice.isConfirmed,invoice.statusId,invoice.narration, taxableAmt,cgstAmt,sgstAmt,igstAmt,grandTotal,invoice.vehicleNo,invoice.createdOn," +
                    " freightItem.freightAmt,tcsItem.tcsAmt,dimState.stateOrUTCode, invAddrCons.overdue_ref_id , invAddrBill.overdue_ref_id as buyer_overdue_ref_id,loadingSlip.cdStructure,loadingSlip.orcAmt,loadingSlip.OrcMeasure,loadingSlipDtl.loadingQty,invoice.firstName,invoice.lastName from(select org.firmName,loadingSlipId, person.firstName as firstName, person.lastName as lastName, invoiceDate, idInvoice, statusDate, invoiceNo, deliveryLocation, discountAmt, cgstAmt, sgstAmt," +
                    " igstAmt, grandTotal, vehicleNo, invoice.createdOn, isConfirmed, statusId, narration,invFromOrgId,dealerOrgId from tempInvoice invoice  LEFT JOIN tblOrganization org on org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization orgDealer on orgDealer.idOrganization = invoice.dealerOrgId LEFT JOIN tblPerson person on person.idPerson = orgDealer.firstOwnerPersonId " +
                    "INNER JOIN tempInvoiceAddress invoiceAdd on invoice.idInvoice = invoiceAdd.invoiceId)invoice" +
                    " INNER JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.village, invAddrB.txnAddrTypeId, invAddrB.gstinNo, invAddrB.stateId, orgB.overdue_ref_id from tempInvoiceAddress invAddrB LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId where txnAddrTypeId = 1)invAddrBill" +
                    " inner join(select idState, stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState on invAddrBill.invoiceId = invoice.idInvoice INNER JOIN(select invAddr.invoiceId, invAddr.billingName, invAddr.txnAddrTypeId, invAddr.gstinNo, org.overdue_ref_id" +
                    " from tempInvoiceAddress invAddr LEFT JOIN tblOrganization org on org.idOrganization = invAddr.billingOrgId where txnAddrTypeId = 2)invAddrCons on invAddrCons.invoiceId = invoice.idInvoice INNER JOIN(select invoiceId, sum(invoiceQty)as invoiceQty,sum(basicTotal) as basicAmt, sum(taxableAmt) as taxableAmt" +
                    " from tempInvoiceItemDetails  where otherTaxId is null group by invoiceId)invoiceItem on invoiceItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt from tempInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice" +
                    " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt from tempInvoiceItemDetails where otherTaxId = 4  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +

                    " LEFT JOIN tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId LEFT JOIN tempLoadingSlipDtl loadingSlipDtl on loadingSlip.idLoadingSlip = loadingSlipDtl.loadingSlipId" +
                    " LEFT JOIN tblBookings tblBookings on tblBookings.idBooking = LoadingSlipDtl.bookingId" +

                    " UNION ALL" +
                    " select distinct  tblBookings.bookingRate,invoice.invFromOrgId,invoice.dealerOrgId,invoice.idInvoice,invoice.statusDate,invoice.invoiceDate,invoice.invoiceNo, invAddrBill.txnAddrTypeId as billingTypeId,  invAddrBill.billingName + CASE WHEN invAddrBill.village IS NULL THEN '-' Else case WHEN invAddrBill.village IS NOT NULL THEN ',' + invAddrBill.village END END as buyer,invAddrBill.gstinNo as buyerGstNo,invoice.firmName as salesEngineer," +
                    " invAddrBill.stateId,invAddrCons.txnAddrTypeId as consigneeTypeId, invAddrCons.billingName as consignee, invAddrCons.gstinNo as consigneeGstNo,invoice.deliveryLocation , invoiceItem.invoiceQty ,basicAmt,discountAmt as cdAmt, invoice.isConfirmed,invoice.statusId,invoice.narration, taxableAmt,cgstAmt,sgstAmt,igstAmt,grandTotal,invoice.vehicleNo,invoice.createdOn, freightItem.freightAmt,tcsItem.tcsAmt,dimState.stateOrUTCode,  invAddrCons.overdue_ref_id , invAddrBill.overdue_ref_id as buyer_overdue_ref_id,loadingSlip.cdStructure,loadingSlip.orcAmt,loadingSlip.OrcMeasure,loadingSlipDtl.loadingQty, invoice.firstName,invoice.lastName" +
                    " from(select org.firmName,loadingSlipId,person.firstName as firstName, person.lastName as lastName, invoiceDate, idInvoice, statusDate, invoiceNo, deliveryLocation, discountAmt, cgstAmt, sgstAmt, igstAmt, grandTotal, vehicleNo, invoice.createdOn, isConfirmed, statusId, narration,invFromOrgId,dealerOrgId from finalInvoice invoice  " +
                    " LEFT JOIN tblOrganization org on org.idOrganization = invoice.distributorOrgId LEFT JOIN tblOrganization orgDealer on orgDealer.idOrganization = invoice.dealerOrgId Left Join tblPerson person on person.idPerson = orgDealer.firstOwnerPersonId INNER JOIN finalInvoiceAddress invoiceAdd on invoice.idInvoice = invoiceAdd.invoiceId)invoice" +
                    " INNER JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.village, invAddrB.txnAddrTypeId, invAddrB.gstinNo, invAddrB.stateId, orgB.overdue_ref_id from finalInvoiceAddress invAddrB LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId where txnAddrTypeId = 1)invAddrBill" +
                    " inner join(select idState, stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState on invAddrBill.invoiceId = invoice.idInvoice INNER JOIN(select invAddr.invoiceId, invAddr.billingName, invAddr.txnAddrTypeId, invAddr.gstinNo, org.overdue_ref_id" +
                    " from finalInvoiceAddress invAddr LEFT JOIN tblOrganization org on org.idOrganization = invAddr.billingOrgId where txnAddrTypeId = 2)invAddrCons on invAddrCons.invoiceId = invoice.idInvoice INNER JOIN(select invoiceId, sum(invoiceQty)as invoiceQty,sum(basicTotal) as basicAmt, sum(taxableAmt) as taxableAmt" +
                    " from finalInvoiceItemDetails  where otherTaxId is null group by invoiceId)invoiceItem on invoiceItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt from finalInvoiceItemDetails where otherTaxId = 2  ) freightItem On freightItem.invoiceId = invoice.idInvoice  " +
                    " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt from finalInvoiceItemDetails where otherTaxId = 4  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN finalLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId LEFT JOIN finalLoadingSlipDtl loadingSlipDtl on loadingSlip.idLoadingSlip = loadingSlipDtl.loadingSlipId" +
                    " LEFT JOIN tblBookings tblBookings on tblBookings.idBooking = LoadingSlipDtl.bookingId ";
                //chetan[12-feb-2020] added for get data from org id
                String formOrgIdCondtion = String.Empty;
                if (fromOrgId > 0)
                {
                    formOrgIdCondtion = " AND sq1.invFromOrgId = " + fromOrgId;
                }

                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED + formOrgIdCondtion +
                " AND CAST(sq1.statusDate AS DATETIME) BETWEEN @fromDate AND @toDate" +
                " order by sq1.invoiceNo asc";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
              
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
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

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate HSN Excel //for c
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm,int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                 " select   distinct invoice.invFromOrgId,invoice.idInvoice,invoice.statusDate ,invoice.invoiceDate,invoice.invoiceNo,invAddrBill.txnAddrTypeId as billTypeId,invAddrBill.billingName as buyer, " +
                 " invAddrBill.gstinNo as buyerGstNo,invAddrCons.txnAddrTypeId as consigneeTypeId,invAddrCons.billingName as consignee, " +
                 " invAddrCons.gstinNo as consigneeGstNo,invoiceItem.gstinCodeNo,invoiceItem.prodItemDesc,invoiceItem.invoiceQty,invoiceItem.rate,invoiceItem.basicTotal as basicAmt," +
                 " invoiceItem.taxableAmt,invoiceItem.idInvoiceItem as invoiceItemId,invoiceTax.taxTypeId,invoiceTax.taxAmt,invoiceItem.grandTotal,invoice.createdOn,invoice.vehicleNo," +
                 " freightItem.freightAmt,invoice.statusId,dimState.stateOrUTCode,invoiceItem.otherTaxId,invoice.narration,invoice.isConfirmed,invAddrCons.overdue_ref_id " +
                 " , invAddrBill.overdue_ref_id as buyer_overdue_ref_id,invoiceItem.overdueTallyRefId ,booking.bookingRate from " +

                 " (select  invoiceDate, idInvoice, invoiceNo,vehicleNo , invoice.statusDate  , createdOn,isConfirmed,statusId,narration,invFromOrgId from tempInvoice invoice " +
                 " INNER JOIN tempInvoiceAddress invoiceAdd On invoice.idInvoice = invoiceAdd.invoiceId)invoice " +
                 " INNER JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId, invAddrB.gstinNo" +
                 " ,invAddrB.stateId , orgB.overdue_ref_id from tempInvoiceAddress invAddrB " +
                 " LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId" +
                 " where txnAddrTypeId  = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + " )invAddrBill " +
                 " inner join (select idState ,stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState" +
                 " On invAddrBill.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select invAddr.invoiceId, invAddr.billingName, invAddr.txnAddrTypeId, invAddr.gstinNo,org.overdue_ref_id from tempInvoiceAddress invAddr " +
                 " LEFT JOIN tblOrganization org on org.idOrganization = invAddr.billingOrgId" +
                 " where txnAddrTypeId= " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                 " On invAddrCons.invoiceId = invoice.idInvoice " +
                 " INNER JOIN" +
                 " (select itemDetails.idInvoiceItem, itemDetails.invoiceId, itemDetails.gstinCodeNo, itemDetails.prodItemDesc" +
                 " ,itemDetails.invoiceQty, itemDetails.rate, itemDetails.basicTotal, itemDetails.taxableAmt, itemDetails.grandTotal," +
                 " itemDetails.otherTaxId ,itemDetails.prodGstCodeId ,tblItemTallyRefDtls.overdueTallyRefId ,itemDetails.loadingSlipExtId" +
                 " from tempInvoiceItemDetails itemDetails" +
                 " LEFT JOIN tblProdGstCodeDtls ON  tblProdGstCodeDtls.idProdGstCode =  itemDetails.prodGstCodeId" +
                 " LEFT JOIN tblItemTallyRefDtls ON ISNULL(tblProdGstCodeDtls.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                 " ISNULL(tblProdGstCodeDtls.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                 " ISNULL(tblProdGstCodeDtls.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                 " ISNULL(tblProdGstCodeDtls.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                 " AND tblitemtallyrefDtls.isActive = 1)invoiceItem" +
                 " On invoiceItem.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select itemTaxDetails.idInvItemTaxDtl, itemTaxDetails.invoiceItemId, itemTaxDetails.taxAmt, taxRate.taxTypeId from tempInvoiceItemTaxDtls itemTaxDetails " +
                 " INNER JOIN tblTaxRates taxRate ON taxRate.idTaxRate = itemTaxDetails.taxRateId)as invoiceTax " +
                 " On invoiceTax.invoiceItemId = invoiceItem.idInvoiceItem " +
                 "LEFT JOIN (select invoiceId,taxAmt as freightAmt from tempInvoiceItemDetails where otherTaxId =  2 )freightItem" +
                 " On freightItem.invoiceId = invoice.idInvoice " +

                " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = invoiceItem.loadingSlipExtId " +
                " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +

                //" where DAY(invoice.invoiceDate) = " + sysDate.Day + " AND MONTH(invoice.invoiceDate) = " + sysDate.Month + " AND YEAR(invoice.invoiceDate) = " + sysDate.Year;

                // Vaibhav [10-Jan-2018] Added to select from finalInvoice

                " UNION ALL " +

                " select   distinct invoice.invFromOrgId,invoice.idInvoice,invoice.statusDate ,invoice.invoiceDate,invoice.invoiceNo,invAddrBill.txnAddrTypeId as billTypeId,invAddrBill.billingName as buyer, " +
                 " invAddrBill.gstinNo as buyerGstNo,invAddrCons.txnAddrTypeId as consigneeTypeId,invAddrCons.billingName as consignee, " +
                 " invAddrCons.gstinNo as consigneeGstNo,invoiceItem.gstinCodeNo,invoiceItem.prodItemDesc,invoiceItem.invoiceQty,invoiceItem.rate,invoiceItem.basicTotal as basicAmt," +
                 " invoiceItem.taxableAmt,invoiceItem.idInvoiceItem as invoiceItemId,invoiceTax.taxTypeId,invoiceTax.taxAmt,invoiceItem.grandTotal,invoice.createdOn,invoice.vehicleNo," +
                 " freightItem.freightAmt,invoice.statusId,dimState.stateOrUTCode,invoiceItem.otherTaxId,invoice.narration,invoice.isConfirmed ,invAddrCons.overdue_ref_id " +
                 " , invAddrBill.overdue_ref_id as buyer_overdue_ref_id,invoiceItem.overdueTallyRefId ,booking.bookingRate from " +

                 " (select  invoiceDate, idInvoice, invoiceNo,vehicleNo , invoice.statusDate  , createdOn,isConfirmed,statusId,narration,invFromOrgId from finalInvoice invoice " +
                 " INNER JOIN finalInvoiceAddress invoiceAdd On invoice.idInvoice = invoiceAdd.invoiceId)invoice " +
                 " INNER JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                 " invAddrB.gstinNo,invAddrB.stateId , orgB.overdue_ref_id from finalInvoiceAddress invAddrB " +
                 " LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId" +
                 " where txnAddrTypeId  = " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + " )invAddrBill " +
                 " inner join (select idState ,stateOrUTCode from dimState)dimState on invAddrBill.stateId = dimState.idState" +
                 " On invAddrBill.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select invAddr.invoiceId, invAddr.billingName, invAddr.txnAddrTypeId, invAddr.gstinNo,org.overdue_ref_id  from finalInvoiceAddress invAddr" +
                 " LEFT JOIN tblOrganization org on org.idOrganization = invAddr.billingOrgId" +
                 " where txnAddrTypeId= " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                 " On invAddrCons.invoiceId = invoice.idInvoice " +
                 " INNER JOIN" +
                 " (select itemDetails.idInvoiceItem, itemDetails.invoiceId, itemDetails.gstinCodeNo, itemDetails.prodItemDesc" +
                 " ,itemDetails.invoiceQty, itemDetails.rate, itemDetails.basicTotal, itemDetails.taxableAmt, itemDetails.grandTotal," +
                 " itemDetails.otherTaxId ,itemDetails.prodGstCodeId ,tblItemTallyRefDtls.overdueTallyRefId,itemDetails.loadingSlipExtId" +
                 " from finalInvoiceItemDetails itemDetails" +
                 " LEFT JOIN tblProdGstCodeDtls ON  tblProdGstCodeDtls.idProdGstCode = itemDetails.prodGstCodeId" +
                 " LEFT JOIN tblItemTallyRefDtls ON ISNULL(tblProdGstCodeDtls.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                 " ISNULL(tblProdGstCodeDtls.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                 " ISNULL(tblProdGstCodeDtls.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                 " ISNULL(tblProdGstCodeDtls.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                 " AND tblitemtallyrefDtls.isActive = 1 )invoiceItem  " +
                 " On invoiceItem.invoiceId = invoice.idInvoice " +
                 " INNER JOIN(select itemTaxDetails.idInvItemTaxDtl, itemTaxDetails.invoiceItemId, itemTaxDetails.taxAmt, taxRate.taxTypeId from finalInvoiceItemTaxDtls itemTaxDetails " +
                 " INNER JOIN tblTaxRates taxRate ON taxRate.idTaxRate = itemTaxDetails.taxRateId)as invoiceTax " +
                 " On invoiceTax.invoiceItemId = invoiceItem.idInvoiceItem " +
                 "LEFT JOIN (select invoiceId,taxAmt as freightAmt from finalInvoiceItemDetails where otherTaxId =  2 )freightItem" +
                 " On freightItem.invoiceId = invoice.idInvoice " +


                 " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = invoiceItem.loadingSlipExtId " +
                 " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking ";
                //chetan[13-feb-2020] added for find data from org id
                String formOrgIdCondtion = String.Empty;
                if (fromOrgId > 0)
                {
                    formOrgIdCondtion = " AND sq1.invFromOrgId = " + fromOrgId;
                }


                cmdSelect.CommandText = " SELECT * FROM ("+ selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED + formOrgIdCondtion +
                     " AND CAST(sq1.statusDate AS DATETIME) BETWEEN @fromDate AND @toDate" +
                     " order by sq1.invoiceNo asc"; ;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
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



        public List<TblInvoiceTO> SelectAllTempInvoice(Int32 loadingSlipId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId = " + loadingSlipId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
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

        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                       " Select distinct invoice.idInvoice,invoice.invoiceNo,invoice.narration,CONVERT(VARCHAR(10),invoice.statusDate,103) as TransactioDateNew, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,lExt.loadedWeight netWeight, lExt.calcTareWeight tareWeight, (lExt.calcTareWeight - lExt.loadedWeight) grossWeight,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,(case when isnull(lExt.prodCatId,0)=1 then  mat.materialSubType else itemDetails.prodItemDesc end ) " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.basicTotal " +
                    " as taxableAmt  ,freightItem.freightAmt,Insurance.InsuranceAmt ,totalItemQtyTbl.TotalItemQty,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId, itemDetails.gstinCodeNo as 'GST_Code_No',   " +
                    " invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId, " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo , " +
                    " invoice.grandTotal, invoice.isConfirmed , invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , " +
                    " invoice.lrDate , invoice.lrNumber ,invAddrCons.overdue_ref_id,invAddrBill.overdue_ref_id as buyer_overdue_ref_id ," +
                    " invoice.taxableAmt as invoiceTaxableAmt ,invoice.discountAmt as invoiceDiscountAmt,tblItemTallyRefDtls.overdueTallyRefId" +
                    " ,invoice.deliveredOn ,invoice.IrnNo ,invoice.electronicRefNo,   " +
                    " invAddrBill.BuyerAddress ,invAddrBill.BuyerTaluka,invAddrBill.BuyerDistict,invAddrBill.BuyerPincode,invAddrBill.BuyercountryName,invAddrCons.consigneecountryName,invAddrCons.consigneeTaluka,Tax.TaxPCT,invoice.roundOffAmt," +
                    " (case when  isnull(ItemDetails.cdStructure,0)=0 then 'Regular' " +
                    " when isnull(ItemDetails.cdStructure,0)= 1.5 then 'CD 1.5%' " +
                    " when isnull(ItemDetails.cdStructure,0)= 1 then 'CD 1%' else null  end ) as CDType " +
                    " ,convert(varchar(50),+ '  ' + JSON_VALUE(ack.response, '$.data.AckNo') + '  ') as AckNo ,CONVERT(VARCHAR(10),CONVERT(DATETIME,JSON_VALUE(ack.response,'$.data.AckDt'), 102),103) as AckDate, " +
                    "  convert(varchar(50),+ '  ' + invoice.electronicRefNo + '  ') as EwbNo ,CONVERT(VARCHAR(10),invoice.invoiceDate,103) as EwbDate  " +
                    " ,(case when isnull(lExt.prodCatId,0)=1 then 'TMT BAR'  when isnull(lExt.prodCatId,0)=2 then 'Threaded Bars' else itemDetails.prodItemDesc end ) as ProDesc " +
                    " ,(case when itemDetails.prodItemDesc = 'Crushed Slags Sand' then 'Crushed Slag' " +
                   " when itemDetails.prodItemDesc = 'Ash' then 'Ash 9%' else (case when isnull(invoice.igstAmt,0)= 0 then " +
                   " (case when isnull(lExt.prodCatId, 0) = 1 then 'TMT BAR' when isnull(lExt.prodCatId, 0) = 2 then 'Threaded Bars' " +
                   "else itemDetails.prodItemDesc end) + '(' + 'Intra-State' + ')' else  (case when isnull(lExt.prodCatId,0)= 1 then 'TMT BAR' " +
                   "when isnull(lExt.prodCatId,0)= 2 then 'Threaded Bars' else itemDetails.prodItemDesc end ) +'(' + 'Inter-State' + ')'   end ) end ) as salesLedgerName " +
                     " ,'TAXABLE FREIGHT OUTWARD' as Freight_GL,'Insurance on Sale (HSN 997136)' as Insurance_GL,'TCS 206C(1H)' as TCS_GL " +
                     " FROM tempInvoice invoice " +
                    " LEFT JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName ,orgB.overdue_ref_id,invAddrB.address as BuyerAddress  ,invAddrB.taluka as BuyerTaluka,invAddrB.district as BuyerDistict,Cntry.countryName as  BuyercountryName,invAddrB.pinCode as BuyerPincode  from tempInvoiceAddress invAddrB " +
                    " LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId left join dimState St on invAddrB.stateId=St.idState left join dimCountry Cntry on St.countryId = Cntry.idCountry" +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo, invAddrC.state as stateName,org.overdue_ref_id, Cntry.countryName as  consigneecountryName,invAddrC.taluka as consigneeTaluka  " +
                    " from tempInvoiceAddress invAddrC   " +
                    " LEFT JOIN tblOrganization org on org.idOrganization = invAddrC.billingOrgId left join dimState St on invAddrC.stateId=St.idState left join dimCountry Cntry on St.countryId = Cntry.idCountry" +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                     " LEFT JOIN tempLoadingSlipDtl LoadingSlipDtl on LoadingSlipDtl.loadingSlipId=invoice.loadingSlipId "+
                    " LEFT JOIN tblBookings booking ON LoadingSlipDtl.bookingId = booking.idBooking " +
                    " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN tblItemTallyRefDtls ON ISNULL(prodGstCodeDtl.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                    " ISNULL(prodGstCodeDtl.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                    " ISNULL(prodGstCodeDtl.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                    " ISNULL(prodGstCodeDtl.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                    " AND tblitemtallyrefDtls.isActive = 1" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN( select invoiceId,SUM(invoiceQty) as TotalItemQty " +
                    " from tempInvoiceItemDetails where otherTaxId is null GROUP BY invoiceId )totalItemQtyTbl On totalItemQtyTbl.invoiceId = invoice.idInvoice" +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                    "LEFT JOIN(select invoiceId, taxableAmt as InsuranceAmt  from tempInvoiceItemDetails where otherTaxId = 5  )Insurance " +
                     "   On Insurance.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 4  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN tempLoading loading on loading.idLoading = loadingSlip.loadingId" +
                    " LEFT join dimProdSpec PS on lExt.prodSpecId = PS.idProdSpec " +
                    " left join tempEInvoiceApiResponse Ack on invoice.idInvoice = Ack.invoiceId  and Ack.apiId = 3 " +
                    " left join tempEInvoiceApiResponse EwbNo on invoice.idInvoice = EwbNo.invoiceId  and EwbNo.apiId = 6 " +
                    " left outer join (select A.idInvoiceItem, sum (isnull(B.taxRatePct,0)) as TaxPCT from tempInvoiceItemDetails A " +
                     " left join tempInvoiceItemTaxDtls B on A.idInvoiceItem = B.invoiceItemId group by A.idInvoiceItem " +
                     " )Tax on Tax.idInvoiceItem = itemDetails.idInvoiceItem " +
                    // Vaibhav [17-Jan-2018] To select from final tables.
                    " UNION ALL " +

                    " Select distinct invoice.idInvoice,invoice.invoiceNo,invoice.narration,CONVERT(VARCHAR(10),invoice.statusDate,103) as TransactioDateNew, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,lExt.loadedWeight netWeight, lExt.calcTareWeight tareWeight, (lExt.calcTareWeight - lExt.loadedWeight) grossWeight,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,(case when isnull(lExt.prodCatId,0)=1 then  mat.materialSubType else itemDetails.prodItemDesc end ) " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.basicTotal " +
                    " as taxableAmt  ,freightItem.freightAmt,Insurance.InsuranceAmt ,totalItemQtyTbl.TotalItemQty,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,itemDetails.gstinCodeNo as 'GST_Code_No',  " +
                    " invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId,  " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo, " +
                    " invoice.grandTotal, invoice.isConfirmed ,invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , invoice.lrDate , invoice.lrNumber,invAddrCons.overdue_ref_id" +
                    " ,invAddrBill.overdue_ref_id as buyer_overdue_ref_id , invoice.taxableAmt as invoiceTaxableAmt ,invoice.discountAmt as invoiceDiscountAmt" +
                    " ,tblItemTallyRefDtls.overdueTallyRefId ,invoice.deliveredOn ,invoice.IrnNo ,invoice.electronicRefNo " +
                    " ,invAddrBill.BuyerAddress ,invAddrBill.BuyerTaluka,invAddrBill.BuyerDistict,invAddrBill.BuyerPincode,invAddrBill.BuyercountryName,invAddrCons.consigneecountryName" +
                    ",invAddrCons.consigneeTaluka,Tax.TaxPCT,invoice.roundOffAmt,(case when  isnull(ItemDetails.cdStructure,0)=0 then 'Regular' " +
                     " when isnull(ItemDetails.cdStructure,0)= 1.5 then 'CD 1.5%' " +
                    "  when isnull(ItemDetails.cdStructure,0)= 1 then 'CD 1%' else null  end ) as CDType  " +
                    " ,convert(varchar(50),+ '  ' + JSON_VALUE(ack.response, '$.data.AckNo') + '  ') as AckNo ,CONVERT(VARCHAR(10),CONVERT(DATETIME,JSON_VALUE(ack.response,'$.data.AckDt'), 102),103) as AckDate, " +
                    "convert(varchar(50),+ '  ' + invoice.electronicRefNo + '  ') as EwbNo ,CONVERT(VARCHAR(10),invoice.invoiceDate,103) as EwbDate " +
                    ",(case when isnull(lExt.prodCatId,0)=1 then 'TMT BAR'  when isnull(lExt.prodCatId,0)=2 then 'Threaded Bars' else itemDetails.prodItemDesc end ) as ProDesc " +
                   " ,(case when itemDetails.prodItemDesc = 'Crushed Slags Sand' then 'Crushed Slag' " +
                  " when itemDetails.prodItemDesc = 'Ash' then 'Ash 9%' else (case when isnull(invoice.igstAmt,0)= 0 then " +
                  " (case when isnull(lExt.prodCatId, 0) = 1 then 'TMT BAR' when isnull(lExt.prodCatId, 0) = 2 then 'Threaded Bars' " +
                  "else itemDetails.prodItemDesc end) + '(' + 'Intra-State' + ')' else  (case when isnull(lExt.prodCatId,0)= 1 then 'TMT BAR' " +
                  "when isnull(lExt.prodCatId,0)= 2 then 'Threaded Bars' else itemDetails.prodItemDesc end ) +'(' + 'Inter-State' + ')'   end ) end ) as salesLedgerName " +

                    " ,'TAXABLE FREIGHT OUTWARD' as Freight_GL,'Insurance on Sale (HSN 997136)' as Insurance_GL,'TCS 206C(1H)' as TCS_GL " +
                    " FROM finalInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName ,orgB.overdue_ref_id " +
                     " ,invAddrB.address as BuyerAddress  ,invAddrB.taluka as BuyerTaluka,invAddrB.district as BuyerDistict,Cntry.countryName as   BuyercountryName,invAddrB.pinCode as BuyerPincode from finalInvoiceAddress invAddrB " +
                    " LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId left join dimState St on invAddrB.stateId=St.idState left join dimCountry Cntry on St.countryId = Cntry.idCountry" +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo,invAddrC.state as stateName ,org.overdue_ref_id,Cntry.countryName as   consigneecountryName,invAddrC.taluka as consigneeTaluka  " +
                    " from finalInvoiceAddress invAddrC   " +
                    " LEFT JOIN tblOrganization org on org.idOrganization = invAddrC.billingOrgId left join dimState St on invAddrC.stateId=St.idState left join dimCountry Cntry on St.countryId = Cntry.idCountry" +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN finalLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    "  LEFT JOIN finalLoadingSlipDtl LoadingSlipDtl on LoadingSlipDtl.loadingSlipId = invoice.loadingSlipId" +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN tblItemTallyRefDtls ON ISNULL(prodGstCodeDtl.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                    " ISNULL(prodGstCodeDtl.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                    " ISNULL(prodGstCodeDtl.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                    " ISNULL(prodGstCodeDtl.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                    " AND tblitemtallyrefDtls.isActive = 1" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN( select invoiceId,SUM(invoiceQty) as TotalItemQty " +
                    " from finalInvoiceItemDetails where otherTaxId is null GROUP BY invoiceId )totalItemQtyTbl On totalItemQtyTbl.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +" from finalInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as InsuranceAmt  from finalInvoiceItemDetails where otherTaxId = 5  )Insurance " +
                    " On Insurance.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 4  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN finalLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN finalLoading loading on loading.idLoading = loadingSlip.loadingId" +
                    " LEFT join dimProdSpec PS on lExt.prodSpecId = PS.idProdSpec " +
                 " left join finalEInvoiceApiResponse Ack on invoice.idInvoice = Ack.invoiceId  and Ack.apiId = 3 " +
                    " left join finalEInvoiceApiResponse EwbNo on invoice.idInvoice = EwbNo.invoiceId  and EwbNo.apiId = 6 " +
                    " left outer join (select A.idInvoiceItem, sum (isnull(B.taxRatePct,0)) as TaxPCT from finalInvoiceItemDetails A " +
                " left join finalInvoiceItemTaxDtls B on A.idInvoiceItem = B.invoiceItemId group by A.idInvoiceItem " +
                " )Tax on Tax.idInvoiceItem = itemDetails.idInvoiceItem ";

           //chetan[13-feb-2020] added get data from org id
           String formOrgIdCondtion = String.Empty;
                if (fromOrgId > 0)
                {
                    formOrgIdCondtion = " AND isnull(sq1.invFromOrgId,"+fromOrgId+") = " + fromOrgId;
                }
                cmdSelect.CommandText = "SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     //" AND CAST(sq1.deliveredOn AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND CAST(sq1.statusDate AS DATE) BETWEEN @fromDate AND @toDate" + formOrgIdCondtion +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED +
                     " order by sq1.invoiceNo asc"; ;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
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

        public List<TblInvoiceRptTO> SelectItemWiseSalesExportCListForReport(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                       " Select distinct invoice.idInvoice,invoice.invoiceNo,invoice.narration,invoice.deliveryNoteNo,invoice.dispatchDocNo,dimMasterValue.masterValueDesc AS voucherClass,dimMasterValueForsalesLedger.masterValueDesc AS salesLedgerName,ISNULL(paymentTerm.paymentTermsDescription,paymentTerm.paymentTermOption) as paymentTerm ,ISNULL(termOfDelivery.paymentTermsDescription,termOfDelivery.paymentTermOption) as termOfDelivery," +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " invAddrBill.buyerAddress,invAddrBill.buyerDistrict,invAddrBill.buyerTaluka,invAddrBill.buyerPinCode,invAddrBill.panNo," +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.basicTotal " +
                    " as taxableAmt  ,freightItem.freightAmt,totalItemQtyTbl.TotalItemQty,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,   " +
                    " invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId, " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo , " +
                    " invoice.grandTotal, invoice.isConfirmed , invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , " +
                    " invoice.lrDate , invoice.lrNumber ,invAddrCons.overdue_ref_id,invAddrBill.overdue_ref_id as buyer_overdue_ref_id ," +
                    " invoice.taxableAmt as invoiceTaxableAmt ,invoice.discountAmt as invoiceDiscountAmt,tblItemTallyRefDtls.overdueTallyRefId" +
                    " ,invoice.deliveredOn," +
                    " CASE WHEN brand.brandName = 'Metaroll' THEN 'Meta Dealer' ELSE brand.brandName + ' Dealer' END AS dealers," +
                    " loadingSlip.loadingSlipNo + ' and ' + FORMAT(loadingSlip.createdOn,'dd-MM-yyyy') AS orderNoandDate,prodCat.prodCateDesc ," +
                    " globalRate.rate as basicRate,FORMAT(loadingSlip.createdOn,'dd-MM-yyyy') AS loadingSlipDate," +
                    " FORMAT(invoice.statusDate ,'dd-MMM-yy') AS transactionDate " +
                    " FROM tempInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId,invAddrB.taluka as buyerTaluka, " +
                    " invAddrB.address as buyerAddress,invAddrB.district as buyerDistrict,invAddrB.pinCode as buyerPinCode,invAddrB.panNo," +
                    " invAddrB.gstinNo, invAddrB.state as stateName ,orgB.overdue_ref_id  from tempInvoiceAddress invAddrB " +
                    " LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId" +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo, invAddrC.state as stateName,org.overdue_ref_id " +
                    " from tempInvoiceAddress invAddrC   " +
                    " LEFT JOIN tblOrganization org on org.idOrganization = invAddrC.billingOrgId" +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                     " LEFT JOIN tempLoadingSlipDtl LoadingSlipDtl on LoadingSlipDtl.loadingSlipId=invoice.loadingSlipId " +
                    " LEFT JOIN tblBookings booking ON LoadingSlipDtl.bookingId = booking.idBooking " +
                    " LEFT JOIN tblGlobalRate  globalRate ON globalRate.idGlobalRate = booking.globalRateId " +
                    " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN tblItemTallyRefDtls ON ISNULL(prodGstCodeDtl.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                    " ISNULL(prodGstCodeDtl.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                    " ISNULL(prodGstCodeDtl.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                    " ISNULL(prodGstCodeDtl.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                    " AND tblitemtallyrefDtls.isActive = 1" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN( select invoiceId,SUM(invoiceQty) as TotalItemQty " +
                    " from tempInvoiceItemDetails where otherTaxId is null GROUP BY invoiceId )totalItemQtyTbl On totalItemQtyTbl.invoiceId = invoice.idInvoice" +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 4  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN tempLoading loading on loading.idLoading = loadingSlip.loadingId" +
                    " LEFT JOIN dimBrand brand ON brand.idBrand = invoice.brandId " +
                    " LEFT JOIN dimProdCat prodCat on prodCat.idProdCat = prodGstCodeDtl.prodCatId " +
                    " LEFT JOIN dimMasterValue dimMasterValue on dimMasterValue.idMasterValue = invoice.voucherClassId" +
                    " LEFT JOIN dimMasterValue dimMasterValueForsalesLedger on dimMasterValueForsalesLedger.idMasterValue = invoice.salesLedgerId" +

                    " LEFT JOIN(select invoiceId, paymentTermRelation.isActive, tblPaymentTermOptions.paymentTermOption, " +
                    " paymentTermRelation.paymentTermsDescription from tblPaymentTermOptionRelation paymentTermRelation " +
                    " LEFT JOIN tblPaymentTermOptions tblPaymentTermOptions on tblPaymentTermOptions.idPaymentTermOption = paymentTermRelation.paymentTermOptionId " +
                    " and tblPaymentTermOptions.isActive = 1 where tblPaymentTermOptions.paymentTermId = 1) as paymentTerm " +
                    "  on paymentTerm.invoiceId = invoice.idInvoice and paymentTerm.isActive = 1 " +

                    " LEFT JOIN(select invoiceId, paymentTermRelation.isActive, tblPaymentTermOptions.paymentTermOption, " +
                    " paymentTermRelation.paymentTermsDescription from tblPaymentTermOptionRelation paymentTermRelation " +
                    " LEFT JOIN tblPaymentTermOptions tblPaymentTermOptions on tblPaymentTermOptions.idPaymentTermOption = paymentTermRelation.paymentTermOptionId " +
                    " and tblPaymentTermOptions.isActive = 1 where tblPaymentTermOptions.paymentTermId = 2) as termOfDelivery " +
                    "  on termOfDelivery.invoiceId = invoice.idInvoice and termOfDelivery.isActive = 1 " +

                // Vaibhav [17-Jan-2018] To select from final tables.
                " UNION ALL " +

                    " Select distinct invoice.idInvoice,invoice.invoiceNo,invoice.narration,invoice.deliveryNoteNo,invoice.dispatchDocNo,dimMasterValue.masterValueDesc AS voucherClass,dimMasterValueForsalesLedger.masterValueDesc AS salesLedgerName,ISNULL(paymentTerm.paymentTermsDescription,paymentTerm.paymentTermOption) as paymentTerm ,ISNULL(termOfDelivery.paymentTermsDescription,termOfDelivery.paymentTermOption) as termOfDelivery, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " invAddrBill.buyerAddress,invAddrBill.buyerDistrict,invAddrBill.buyerTaluka,invAddrBill.buyerPinCode,invAddrBill.panNo," +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.basicTotal " +
                    " as taxableAmt  ,freightItem.freightAmt,totalItemQtyTbl.TotalItemQty,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,  " +
                    " invoice.cgstAmt,invoice.igstAmt,invoice.sgstAmt,itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId,  " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo, " +
                    " invoice.grandTotal, invoice.isConfirmed ,invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , invoice.lrDate , invoice.lrNumber,invAddrCons.overdue_ref_id" +
                    " ,invAddrBill.overdue_ref_id as buyer_overdue_ref_id , invoice.taxableAmt as invoiceTaxableAmt ,invoice.discountAmt as invoiceDiscountAmt" +
                    " ,tblItemTallyRefDtls.overdueTallyRefId ,invoice.deliveredOn, " +
                    " CASE WHEN brand.brandName = 'Metaroll' THEN 'Meta Dealer' ELSE brand.brandName + ' Dealer' END AS dealers," +
                    " loadingSlip.loadingSlipNo + ' and ' + FORMAT(loadingSlip.createdOn,'dd-MM-yyyy') AS orderNoandDate,prodCat.prodCateDesc, " +
                    " globalRate.rate as basicRate,FORMAT(loadingSlip.createdOn,'dd-MM-yyyy') AS loadingSlipDate," +
                    " FORMAT(invoice.statusDate ,'dd-MMM-yy') AS transactionDate " +
                    " FROM finalInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId, invAddrB.billingName, invAddrB.txnAddrTypeId,invAddrB.taluka as buyerTaluka, " +
                    " invAddrB.address as buyerAddress,invAddrB.district as buyerDistrict,invAddrB.pinCode as buyerPinCode,invAddrB.panNo," +
                    " invAddrB.gstinNo, invAddrB.state as stateName ,orgB.overdue_ref_id  from finalInvoiceAddress invAddrB " +
                    " LEFT JOIN tblOrganization orgB on orgB.idOrganization = invAddrB.billingOrgId" +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo,invAddrC.state as stateName ,org.overdue_ref_id" +
                    " from finalInvoiceAddress invAddrC   " +
                    " LEFT JOIN tblOrganization org on org.idOrganization = invAddrC.billingOrgId" +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN finalLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    "  LEFT JOIN finalLoadingSlipDtl LoadingSlipDtl on LoadingSlipDtl.loadingSlipId = invoice.loadingSlipId" +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN tblGlobalRate  globalRate ON globalRate.idGlobalRate = booking.globalRateId " +
                    " LEFT JOIN tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN tblItemTallyRefDtls ON ISNULL(prodGstCodeDtl.prodCatId,0) =  ISNULL(tblItemTallyRefDtls.prodCatId,0) AND" +
                    " ISNULL(prodGstCodeDtl.prodSpecId,0) = ISNULL(tblItemTallyRefDtls.prodSpecId,0) AND " +
                    " ISNULL(prodGstCodeDtl.materialId,0) = ISNULL(tblItemTallyRefDtls.materialId,0) AND " +
                    " ISNULL(prodGstCodeDtl.prodItemId,0) = ISNULL(tblitemtallyrefDtls.prodItemId,0) " +
                    " AND tblitemtallyrefDtls.isActive = 1" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN( select invoiceId,SUM(invoiceQty) as TotalItemQty " +
                    " from finalInvoiceItemDetails where otherTaxId is null GROUP BY invoiceId )totalItemQtyTbl On totalItemQtyTbl.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 4  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN finalLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN finalLoading loading on loading.idLoading = loadingSlip.loadingId" +
                    " LEFT JOIN dimBrand brand ON brand.idBrand = invoice.brandId " +
                    " LEFT JOIN dimProdCat prodCat on prodCat.idProdCat = prodGstCodeDtl.prodCatId" +
                    " LEFT JOIN dimMasterValue dimMasterValue on dimMasterValue.idMasterValue = invoice.voucherClassId" +
                    " LEFT JOIN dimMasterValue dimMasterValueForsalesLedger on dimMasterValueForsalesLedger.idMasterValue = invoice.salesLedgerId" +

                    " LEFT JOIN(select invoiceId, paymentTermRelation.isActive, tblPaymentTermOptions.paymentTermOption, " +
                    " paymentTermRelation.paymentTermsDescription from tblPaymentTermOptionRelation paymentTermRelation " +
                    " LEFT JOIN tblPaymentTermOptions tblPaymentTermOptions on tblPaymentTermOptions.idPaymentTermOption = paymentTermRelation.paymentTermOptionId " +
                    " and tblPaymentTermOptions.isActive = 1 where tblPaymentTermOptions.paymentTermId = 1) as paymentTerm " +
                    "  on paymentTerm.invoiceId = invoice.idInvoice and paymentTerm.isActive = 1 " +

                      " LEFT JOIN(select invoiceId, paymentTermRelation.isActive, tblPaymentTermOptions.paymentTermOption, " +
                    " paymentTermRelation.paymentTermsDescription from tblPaymentTermOptionRelation paymentTermRelation " +
                    " LEFT JOIN tblPaymentTermOptions tblPaymentTermOptions on tblPaymentTermOptions.idPaymentTermOption = paymentTermRelation.paymentTermOptionId " +
                    " and tblPaymentTermOptions.isActive = 1 where tblPaymentTermOptions.paymentTermId = 2) as termOfDelivery " +
                    "  on termOfDelivery.invoiceId = invoice.idInvoice and termOfDelivery.isActive = 1 ";


                //chetan[13-feb-2020] added get data from org id
                String formOrgIdCondtion = String.Empty;
                if (fromOrgId > 0)
                {
                    formOrgIdCondtion = " AND isnull(sq1.invFromOrgId," + fromOrgId + ") = " + fromOrgId;
                }
                cmdSelect.CommandText = "SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     //" AND CAST(sq1.deliveredOn AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND CAST(sq1.statusDate AS DATE) BETWEEN @fromDate AND @toDate" + formOrgIdCondtion +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED +
                     " order by sq1.invoiceNo asc"; ;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
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

        public List<TblInvoiceRptTO> SelectSalesPurchaseListForReport(DateTime frmDt, DateTime toDt, int isConfirm, string selectedOrg, int defualtOrg, int isFromPurchase = 0)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            //DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery =
                    " Select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.invoiceNo,invoice.electronicRefNo,invoice.IrnNo,invoice.dealerOrgId " +
                    " ,tblOrgLicenseDtl.licenseValue as orgGstNo,tblGstCodeDtls.codeNumber, invoice.tareWeight,invoice.grossWeight,invoice.netWeight, invoice.roundOffAmt,invoice.narration, invoice.tdsAmt, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.address as buyerAddress," +
                    " invAddrBill.district as buyerDistrict,invAddrBill.pinCode as buyerPinCode," +
                    " invAddrBill.taluka as buyerTaluka,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeTaluka,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt " +
                    " as taxableAmt  ,freightItem.freightAmt,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,   " +
                    " (SELECT itemTax.taxAmt FROM [tempInvoiceItemTaxDtls] itemTax  LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate" +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 2) as cgstAmt," +
                    " (SELECT itemTax.taxAmt FROM[tempInvoiceItemTaxDtls] itemTax  LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 3) as sgstAmt, " +
                    " (SELECT itemTax.taxAmt FROM[tempInvoiceItemTaxDtls] itemTax  LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 1) as igstAmt ," +
                    " (SELECT itemTax.taxRatePct FROM[tempInvoiceItemTaxDtls] itemTax LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 1) as igstPct , " +
                    " (SELECT itemTax.taxRatePct FROM[tempInvoiceItemTaxDtls] itemTax LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 3) as sgstPct , " +
                    " (SELECT itemTax.taxRatePct FROM[tempInvoiceItemTaxDtls] itemTax LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 2) as cgstPct ," +
                    " itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId, " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo , " +
                    " invoice.grandTotal, invoice.isConfirmed , invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , " +
                    " invoice.lrDate , invoice.lrNumber,invoice.deliveredOn, dimInvoiceTypes.invoiceTypeDesc,invFromOrg.firmName As invFromOrgName,tblAddress.areaName as OrgareaName,tblAddress.pincode as Orgpincode " +
                    " ,tblAddress.plotNo as OrgplotNo,tblAddress.villageName as OrgvillageName ,dimDistrict.districtName as OrgdistrictName," +
                    " dimState.stateName as OrgstateName,dimCountry.countryName As OrgcountryName FROM tempInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId,invAddrB.address,invAddrB.district,invAddrB.pinCode,invAddrB.taluka, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName from tempInvoiceAddress invAddrB " +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.taluka as consigneeTaluka,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo, invAddrC.state as stateName " +
                    " from tempInvoiceAddress invAddrC   " +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization invFromOrg  ON invFromOrg.idOrganization = invoice.invFromOrgId " +
                    " LEFT JOIN tblOrgLicenseDtl tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId = invoice.invFromOrgId  and tblOrgLicenseDtl.licenseId = " + (Int32)Constants.CommercialLicenseE.IGST_NO +
                    //" LEFT JOIN tblOrgAddress tblOrgAddress  ON tblOrgAddress.organizationId = invoice.invFromOrgId " +
                    //" and tblOrgAddress.addrTypeId = 5 " +
                " LEFT JOIN tblAddress tblAddress  ON tblAddress.idAddr = invFromOrg.addrId " +
                    " LEFT JOIN dimDistrict dimDistrict  ON dimDistrict.idDistrict = tblAddress.districtId " +
                    " LEFT JOIN dimState dimState  ON dimState.idState = tblAddress.stateId " +
                    " LEFT JOIN dimCountry dimCountry  ON dimCountry.idCountry = tblAddress.countryId  " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN tempInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN tempLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN  tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN  tblGstCodeDtls tblGstCodeDtls on prodGstCodeDtl.gstCodeId = tblGstCodeDtls.idGstCode" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +
                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                     " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from tempInvoiceItemDetails where otherTaxId = 5  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tempLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN tempLoading loading on loading.idLoading = loadingSlip.loadingId  " +
                    " LEFT JOIN dimInvoiceTypes dimInvoiceTypes on dimInvoiceTypes.idInvoiceType = invoice.invoiceTypeId  " +

                    // Vaibhav [17-Jan-2018] To select from final tables.
                    " UNION ALL " +

                    " Select distinct case when invoice.invoiceModeId = " + Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE) + " then 'Yes' else 'No' end as mode,invoice.idInvoice,invoice.invoiceNo,invoice.electronicRefNo,invoice.IrnNo,invoice.dealerOrgId,tblGstCodeDtls.codeNumber" +
                    " ,tblOrgLicenseDtl.licenseValue as orgGstNo,invoice.tareWeight,invoice.grossWeight,invoice.netWeight,invoice.roundOffAmt, invoice.narration, invoice.tdsAmt, " +
                    " invoice.statusDate ,invoice.invoiceDate,invoice.createdOn,invAddrBill.billingName as partyName, " +
                    " invAddrBill.stateName as buyerState ,invAddrBill.gstinNo as buyerGstNo,invAddrBill.address as buyerAddress," +
                    " invAddrBill.district as buyerDistrict,invAddrBill.pinCode as buyerPinCode," +
                    " invAddrBill.taluka as buyerTaluka,invAddrBill.txnAddrTypeId as billingTypeId, " +
                    " org.firmName cnfName, invAddrCons.billingName as consignee,invAddrCons.consigneeAddress,invAddrCons.consigneeTaluka,invAddrCons.consigneeDistict," +
                    " invAddrCons.consigneePinCode,invAddrCons.stateName as consigneeState,invAddrCons.gstinNo as consigneeGstNo, " +
                    " invAddrCons.txnAddrTypeId as consigneeTypeId,booking.bookingRate,itemDetails.prodItemDesc,mat.materialSubType " +
                    " as materialName, itemDetails.bundles, itemDetails.cdStructure,itemDetails.invoiceQty,itemDetails.taxableAmt " +
                    " as taxableAmt  ,freightItem.freightAmt,tcsItem.tcsAmt,itemDetails.idInvoiceItem as invoiceItemId,  " +
                    " (SELECT itemTax.taxAmt FROM [finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 2) as cgstAmt," +
                    " (SELECT itemTax.taxAmt FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 3) as sgstAmt," +
                    " (SELECT itemTax.taxAmt FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate " +
                    " where itemTax.invoiceItemId = itemDetails.idInvoiceItem AND taxRate.taxTypeId = 1) as igstAmt " +
                    "  ,(SELECT itemTax.taxRatePct FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate  where itemTax.invoiceItemId = itemDetails.idInvoiceItem " +
                    " AND taxRate.taxTypeId = 1) as igstPct , " +
                    " (SELECT itemTax.taxRatePct FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate  where itemTax.invoiceItemId = itemDetails.idInvoiceItem " +
                    " AND taxRate.taxTypeId = 3) as sgstPct ," +
                    " (SELECT itemTax.taxRatePct FROM[finalInvoiceItemTaxDtls] itemTax " +
                    " LEFT JOIN tblTaxRates taxRate ON itemTax.taxRateId = taxRate.idTaxRate  where itemTax.invoiceItemId = itemDetails.idInvoiceItem " +
                    " AND taxRate.taxTypeId = 2) as cgstPct ," +
                    " itemDetails.rate,   itemDetails.cdAmt,itemDetails.otherTaxId,  " +
                    " transportOrg.firmName as transporterName,invoice.deliveryLocation,invoice.vehicleNo,transportOrg.registeredMobileNos as contactNo, " +
                    " invoice.grandTotal, invoice.isConfirmed ,invoice.statusId, invoice.invFromOrgId ," +
                    " org.registeredMobileNos as cnfMobNo , dealerOrg.registeredMobileNos as dealerMobNo , invoice.lrDate , invoice.lrNumber" +
                    " ,invoice.deliveredOn,dimInvoiceTypes.invoiceTypeDesc,invFromOrg.firmName As invFromOrgName,tblAddress.areaName as OrgareaName, " +
                    " tblAddress.pincode as Orgpincode ,tblAddress.plotNo as OrgplotNo,tblAddress.villageName as OrgvillageName," +
                    " dimDistrict.districtName as OrgdistrictName,dimState.stateName as OrgstateName,dimCountry.countryName As OrgcountryName FROM finalInvoice invoice " +

                    " LEFT JOIN(select invAddrB.invoiceId,invAddrB.address,invAddrB.district,invAddrB.pinCode,invAddrB.taluka, invAddrB.billingName, invAddrB.txnAddrTypeId, " +
                    " invAddrB.gstinNo, invAddrB.state as stateName from finalInvoiceAddress invAddrB " +
                    " where txnAddrTypeId =  " + (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS + ")invAddrBill " +
                    " on invAddrBill.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN(select invAddrC.invoiceId, invAddrC.billingName, invAddrC.address as consigneeAddress, " +
                    " invAddrC.district as consigneeDistict,invAddrC.taluka as consigneeTaluka,invAddrC.pinCode as consigneePinCode, " +
                    " invAddrC.txnAddrTypeId, invAddrC.gstinNo,invAddrC.state as stateName " +
                    " from finalInvoiceAddress invAddrC   " +
                    " where txnAddrTypeId = " + (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS + ")invAddrCons " +
                    " on invAddrCons.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN tblOrganization org  ON org.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization dealerOrg  ON dealerOrg.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization invFromOrg  ON invFromOrg.idOrganization = invoice.invFromOrgId " +
                    " LEFT JOIN tblOrgLicenseDtl tblOrgLicenseDtl on tblOrgLicenseDtl.organizationId = invoice.invFromOrgId  and tblOrgLicenseDtl.licenseId = " + (Int32)Constants.CommercialLicenseE.IGST_NO +
                    //" LEFT JOIN tblOrgAddress tblOrgAddress  ON tblOrgAddress.organizationId = invoice.invFromOrgId " +
                    //" and tblOrgAddress.addrTypeId = 5 " +
                " LEFT JOIN tblAddress tblAddress  ON tblAddress.idAddr = invFromOrg.addrId " +
                    " LEFT JOIN dimDistrict dimDistrict  ON dimDistrict.idDistrict = tblAddress.districtId " +
                    " LEFT JOIN dimState dimState  ON dimState.idState = tblAddress.stateId " +
                    " LEFT JOIN dimCountry dimCountry  ON dimCountry.idCountry = tblAddress.countryId  " +
                    " LEFT JOIN tblOrganization transportOrg ON transportOrg.idOrganization = invoice.transportOrgId " +
                    " INNER JOIN finalInvoiceItemDetails itemDetails  ON itemDetails.invoiceId = invoice.idInvoice " +
                    " AND itemDetails.otherTaxId is  NULL" +
                    " LEFT JOIN finalLoadingSlipExt lExt ON lExt.idLoadingSlipExt = itemDetails.loadingSlipExtId " +
                    " LEFT JOIN tblBookings booking ON lExt.bookingId = booking.idBooking " +
                    " LEFT JOIN tblProdGstCodeDtls prodGstCodeDtl on prodGstCodeDtl.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN  tblGstCodeDtls tblGstCodeDtls on prodGstCodeDtl.gstCodeId = tblGstCodeDtls.idGstCode" +
                    " LEFT JOIN tblMaterial mat on mat.idMaterial = prodGstCodeDtl.materialId " +

                    " LEFT JOIN(select invoiceId, taxableAmt as freightAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 2  )freightItem On freightItem.invoiceId = invoice.idInvoice " +
                     " LEFT JOIN(select invoiceId, taxableAmt as tcsAmt " +
                    " from finalInvoiceItemDetails where otherTaxId = 5  )tcsItem On tcsItem.invoiceId = invoice.idInvoice " +
                    " LEFT JOIN finalLoadingSlip loadingSlip on loadingSlip.idLoadingSlip = invoice.loadingSlipId " +
                    " LEFT JOIN finalLoading loading on loading.idLoading = loadingSlip.loadingId  " +
                    "LEFT JOIN dimInvoiceTypes dimInvoiceTypes on dimInvoiceTypes.idInvoiceType = invoice.invoiceTypeId ";


                String strWhere = String.Empty;
                //if (selectedOrg > 0)
                //{
                //    if(defualtOrg == selectedOrg)
                //    {

                //        strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ",0)";
                //    }
                //    else
                //        strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ")";
                //}
                if (isFromPurchase == 1)
                {
                    strWhere = " AND sq1.dealerOrgId  IN (" + selectedOrg + ")";
                }
                else
                {
                    strWhere = " AND sq1.invFromOrgId  IN (" + selectedOrg + ")";
                }
                cmdSelect.CommandText = " SELECT * FROM (" + selectQuery + ")sq1 WHERE sq1.isConfirmed =" + isConfirm +
                     " AND CAST(sq1.invoiceDate AS DATE) BETWEEN @fromDate AND @toDate" +
                     " AND sq1.statusId = " + (int)Constants.InvoiceStatusE.AUTHORIZED + strWhere + " order by sq1.invoiceNo asc";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceRptTO> list = ConvertDTToListForRPTInvoice(reader);

                return list;
            }
            catch (Exception ex)
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

        public List<TblInvoiceTO> SelectAllTempInvoice(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                //cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId = " + loadingSlipId;
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.idInvoice IN(select invoiceId from tempLoadingSlipInvoice where loadingSlipId ="+ loadingSlipId +")";
                cmdSelect.Connection = conn;  
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Vijaymala [17-04-2018]:added to get invoice list invoice id
        /// </summary>
        /// <param name="loadingSlipIds"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectInvoiceListFromInvoiceIds(String invoiceIds)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE sq1.idInvoice IN (" + invoiceIds + ") ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
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


        public List<TblInvoiceTO> SelectAllFinalInvoice( SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText =
                                  " SELECT invoice.* ,dealer.firmName as dealerName,distributor.firmName AS distributorName " +
                                  " , transport.firmName AS transporterName,currencyName,statusName,invoiceTypeDesc " +
                                  " FROM finalInvoice invoice " +
                                  " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                                  " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                                  " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                                  " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                                  " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                                  " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public double GetTareWeightFromInvoice(String lodingSlipIds, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = "SELECT MIN(tareWeight) FROM tempinvoice  WHERE loadingSlipId IN (" + lodingSlipIds + " )";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return Convert.ToDouble(cmdSelect.ExecuteScalar());

            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingSlipTO> SelectLoadingDetailsByInvoiceId(int invoiceId,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText =

                    " SELECT tempLoadingSlip.* ,tmploading.modbusRefId,tmploading.gateId,tblgate.portNumber,tblgate.ioTUrl,tblgate.machineIP,tmploading.isDBup,tblOrganization.firmName as dealerOrgName,tblBookings.bookingDisplayNo, tempLoadSlipdtl.bookingId,tempLoadSlipdtl.loadingQty, cnfOrg.firmName as cnfOrgName ,dimStat.statusName ,tempLoadingSlip.comment 'comments'" +
                                  " FROM tempLoadingSlip " +
                                  " LEFT JOIN tblOrganization " +
                                  " ON tblOrganization.idOrganization = tempLoadingSlip.dealerOrgId " +
                                  " LEFT JOIN tblOrganization AS cnfOrg " +
                                  " ON cnfOrg.idOrganization = tempLoadingSlip.cnfOrgId " +
                                  " LEFT JOIN dimStatus dimStat " +
                                  " ON dimStat.idStatus = tempLoadingSlip.statusId "+
                                  " INNER JOIN tempLoadingSlipInvoice loadingSlipInvoice ON tempLoadingSlip.idLoadingSlip = loadingSlipInvoice.loadingSlipId " +
                                  //Saket [2018-06-09] Added.
                                  " LEFT JOIN tempLoading tmploading ON  tempLoadingSlip.loadingId = tmploading.idLoading" +
                                  " LEFT JOIN tblGate tblgate ON tmploading.gateId = tblgate.idGate " +
                                  " LEFT JOIN tempLoadingSlipDtl tempLoadSlipdtl ON tempLoadSlipdtl.loadingSlipId = tempLoadingSlip.idLoadingSlip " +
                                   " Left Join tblBookings tblBookings ON tempLoadSlipdtl.bookingId = tblBookings.idbooking " +
                                  " WHERE loadingSlipInvoice.invoiceId = " + invoiceId; 

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipTO> list = _iTblLoadingSlipDAO.ConvertDTToList(reader);
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Vijaymala added [09-05-2018]:To get notified invoices list
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt,int isConfirm,int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                String whereCondition = " Where sq1.invoiceModeId = " + (int)Constants.InvoiceModeE.AUTO_INVOICE_EDIT +
                    " AND CAST(sq1.statusDate AS Date) BETWEEN @fromDate AND @toDate";

                whereCondition += " AND ISNULL(sq1.isConfirmed,0) = " + isConfirm;
                if(fromOrgId>0)
                {
                    whereCondition +=" and invFromOrgId= " + fromOrgId;
                }
                conn.Open();
                cmdSelect.CommandText = "Select * From (" + SqlSelectQuery() + ")sq1 " + whereCondition;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblInvoiceTO> list = ConvertDTToList(reader);
                return list;
            }
            catch (Exception ex)
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


        public List<InvoiceReportTO> SelectAllInvoices(DateTime frmDt, DateTime toDt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataAdapter dq = null;
            DataTable dt = new DataTable();
            List<InvoiceReportTO> invoiceReportTOList = new List<InvoiceReportTO>();
            try
            {

                String query = "select tempInvoice.idInvoice, dimProdCat.prodCateDesc,dimProdSpec.prodSpecDesc,tblMaterial.materialSubType " +
                    " ,tempInvoice.invoiceNo, tempInvoice.invoiceDate,tempInvoiceItemDetails.invoiceQty,  tempLoadingSlipExt.* ,tblUser.userDisplayName as 'SuperWise Name' " +
                    " ,tempInvoice.vehicleNo   ,tblBookings.bookingRate,salesEngMaster.firmName  as SaleEngineer , [tempInvoiceItemDetails].cdStructure as 'CDStructure1',tblParityDetails.baseValCorAmt ,tempInvoice .grandTotal  " +//Reshma Added For Sizewise report changes for Gajkesari
                    " ,TCS.grandTotal as TCSAmt  ,Freight .taxableAmt as FreightAmt,tempLoadingSlip.orcAmt as OtherAmount " +
                    " from[dbo].[tempInvoiceItemDetails] " +
                    " Join tempInvoice On tempInvoice.idInvoice = tempInvoiceItemDetails.invoiceId " +
                    " Join tempLoadingSlipExt On tempLoadingSlipExt.idLoadingSlipExt = tempInvoiceItemDetails.loadingSlipExtId " +
                    " Left Join dimProdCat On dimProdCat.idProdCat = tempLoadingSlipExt.prodCatId " +
                    " Left Join dimProdSpec On dimProdSpec.idProdSpec = tempLoadingSlipExt.prodSpecId " +
                    " Left Join tblMaterial On tblMaterial.idMaterial = tempLoadingSlipExt.materialId "+
                    " Left join tempLoadingSlip  on tempInvoice.loadingSlipId  =tempLoadingSlip.idLoadingSlip  "+
                    " Left join tempLoading on tempLoadingSlip.loadingId = tempLoading.idLoading "+
                    " Left join tblUser on tblUser.idUser = tempLoading.superwisorId "+
                    "   left join tblBookings on tblBookings .idBooking =tempLoadingSlipExt.bookingId  " +
                    "  left join tblOrganization salesEngMaster on salesEngMaster.idOrganization =tblBookings.cnFOrgId " +
                    "  left join tblParityDetails on tblParityDetails .idParityDtl =tempLoadingSlipExt.parityDtlId " +
                    "  left join [tempInvoiceItemDetails] TCS on tempInvoice.idInvoice =TCS.invoiceId and TCS.otherTaxId = "+ (Int32)Constants.OtherTaxTypeE.AFTERCESS +
                    "  left join [tempInvoiceItemDetails] Freight on tempInvoice.idInvoice =Freight.invoiceId and Freight.otherTaxId =" + (Int32)Constants.OtherTaxTypeE.FREIGHT +
                    " Where  CAST(tempInvoice.invoiceDate AS Date) BETWEEN @fromDate AND @toDate  Order by tempInvoice.invoiceDate";
                conn.Open();
                cmdSelect.CommandText = query;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;

                dq = new SqlDataAdapter(cmdSelect);
                dq.Fill(dt);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        InvoiceReportTO invoiceReportTO = new InvoiceReportTO();
                        invoiceReportTO.InvoiceId = Convert.ToInt32(dt.Rows[i]["idInvoice"]);
                        invoiceReportTO.ProdCat = Convert.ToString(dt.Rows[i]["prodCateDesc"]);
                        invoiceReportTO.MaterialSubType = Convert.ToString(dt.Rows[i]["materialSubType"]);
                        invoiceReportTO.InvoiceDate = Convert.ToDateTime(dt.Rows[i]["invoiceDate"]);
                        invoiceReportTO.InvoiceQty = Convert.ToDouble(dt.Rows[i]["invoiceQty"]);
                        invoiceReportTO.InvoiceNo = Convert.ToString(dt.Rows[i]["invoiceNo"]);
                        invoiceReportTO.SuperwiserName = Convert.ToString(dt.Rows[i]["SuperWise Name"]);
                        invoiceReportTO.VehicleNo = Convert.ToString(dt.Rows[i]["vehicleNo"]);
                        invoiceReportTO.ItemDecscription = Convert.ToString(dt.Rows[i]["prodSpecDesc"]);
                        if (dt.Rows[i]["bookingRate"] != DBNull.Value)
                            invoiceReportTO.Rate = Convert.ToDouble(dt.Rows[i]["bookingRate"].ToString());
                        if (dt.Rows[i]["SaleEngineer"] != DBNull.Value)
                            invoiceReportTO.SaleEngineer = Convert.ToString(dt.Rows[i]["SaleEngineer"].ToString());
                        if (dt.Rows[i]["CDStructure1"] != DBNull.Value)
                            invoiceReportTO.CDPct = Convert.ToDouble(dt.Rows[i]["CDStructure1"].ToString());

                        if (dt.Rows[i]["grandTotal"] != DBNull.Value)
                            invoiceReportTO.TotalAmt = Convert.ToDouble(dt.Rows[i]["grandTotal"].ToString());
                        if (dt.Rows[i]["baseValCorAmt"] != DBNull.Value)
                            invoiceReportTO.BVCAmt = Convert.ToDouble(dt.Rows[i]["baseValCorAmt"].ToString());
                        if (dt.Rows[i]["TCSAmt"] != DBNull.Value)
                            invoiceReportTO.TCS = Convert.ToDouble(dt.Rows[i]["TCSAmt"].ToString());
                        if (dt.Rows[i]["FreightAmt"] != DBNull.Value)
                            invoiceReportTO.Freight = Convert.ToDouble(dt.Rows[i]["FreightAmt"].ToString());
                        if (dt.Rows[i]["OtherAmount"] != DBNull.Value)
                            invoiceReportTO.OtherAmt = Convert.ToDouble(dt.Rows[i]["OtherAmount"].ToString());

                        invoiceReportTOList.Add(invoiceReportTO);

                    }
                }
                
                return invoiceReportTOList;
              
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
        public int InsertTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblInvoiceTO, cmdInsert);
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

        public int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblInvoiceTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempInvoice]( " +
                                "  [invoiceTypeId]" +
                                " ,[transportOrgId]" +
                                " ,[transportModeId]" +
                                " ,[currencyId]" +
                                " ,[loadingSlipId]" +
                                " ,[distributorOrgId]" +
                                " ,[dealerOrgId]" +
                                " ,[finYearId]" +
                                " ,[statusId]" +
                                " ,[createdBy]" +
                                " ,[updatedBy]" +
                                " ,[invoiceDate]" +
                                " ,[lrDate]" +
                                " ,[statusDate]" +
                                " ,[createdOn]" +
                                " ,[updatedOn]" +
                                " ,[currencyRate]" +
                                " ,[basicAmt]" +
                                " ,[discountAmt]" +
                                " ,[taxableAmt]" +
                                " ,[cgstAmt]" +
                                " ,[sgstAmt]" +
                                " ,[igstAmt]" +
                                " ,[freightPct]" +
                                " ,[freightAmt]" +
                                " ,[roundOffAmt]" +
                                " ,[grandTotal]" +
                                " ,[invoiceNo]" +
                                " ,[electronicRefNo]" +
                                " ,[vehicleNo]" +
                                " ,[lrNumber]" +
                                " ,[roadPermitNo]" +
                                " ,[transportorForm]" +
                                " ,[airwayBillNo]" +
                                " ,[narration]" +
                                " ,[bankDetails]" +
                                " ,[invoiceModeId]" +
                                " ,[deliveryLocation]" +
                                  " ,[tareWeight]" +
                                " ,[netWeight]" +
                                " ,[grossWeight]" +
                                 " ,[changeIn]" +
                                " ,[expenseAmt]" +
                                " ,[otherAmt]" +
                                " ,[isConfirmed]" +
                                " ,[rcmFlag]" +
                                " ,[remark]" +
                                " ,[invFromOrgId]" +
                                " ,[brandId]" +
                                " ,[poNo]" +//Vijaymala[2018-02-26]Added
                                " ,[poDate]" + //Vijaymala[2018-02-26]Added
                                " ,[deliveredOn]" +
                                " ,[orcPersonName]" +
                                " ,[grossWtTakenDate]" +
                                " ,[preparationDate]" +
                                " ,[invFromOrgFreeze]" +
                                " ,[comment]" +
                                " ,[tdsAmt]" +
                                " ,[deliveryNoteNo]" +
                                " ,[dispatchDocNo]" +
                                " ,[voucherClassId]" +
                                " ,[salesLedgerId]" +
                                  ", [brokerName]" +
                                " )" +
                    " VALUES (" +
                                "  @InvoiceTypeId " +
                                " ,@TransportOrgId " +
                                " ,@TransportModeId " +
                                " ,@CurrencyId " +
                                " ,@LoadingSlipId " +
                                " ,@DistributorOrgId " +
                                " ,@DealerOrgId " +
                                " ,@FinYearId " +
                                " ,@StatusId " +
                                " ,@CreatedBy " +
                                " ,@UpdatedBy " +
                                " ,@InvoiceDate " +
                                " ,@LrDate " +
                                " ,@StatusDate " +
                                " ,@CreatedOn " +
                                " ,@UpdatedOn " +
                                " ,@CurrencyRate " +
                                " ,@BasicAmt " +
                                " ,@DiscountAmt " +
                                " ,@TaxableAmt " +
                                " ,@CgstAmt " +
                                " ,@SgstAmt " +
                                " ,@IgstAmt " +
                                " ,@FreightPct " +
                                " ,@FreightAmt " +
                                " ,@RoundOffAmt " +
                                " ,@GrandTotal " +
                                " ,@InvoiceNo " +
                                " ,@ElectronicRefNo " +
                                " ,@VehicleNo " +
                                " ,@LrNumber " +
                                " ,@RoadPermitNo " +
                                " ,@TransportorForm " +
                                " ,@AirwayBillNo " +
                                " ,@Narration " +
                                " ,@BankDetails " +
                                " ,@invoiceModeId " +
                                " ,@deliveryLocation " +
                                 " ,@tareWeight " +
                                " ,@netWeight " +
                                " ,@grossWeight " +
                                 " ,@changeIn " +
                                " ,@expenseAmt " +
                                " ,@otherAmt " +
                                " ,@isConfirmed " +
                                " ,@rcmFlag " +
                                " ,@remark " +
                                " ,@invFromOrgId " +
                                " ,@BrandId " +
                                " ,@poNo " +
                                " ,@poDate" + //Vijaymala[2018-02-26]Added
                                " ,@deliveredOn " +
                                " ,@ORCPersonName " +
                                " ,@grossWtTakenDate" +
                                " ,@preparationDate" +
                                " ,@InvFromOrgFreeze" +
                                " ,@comment" +
                                " ,@tdsAmt" +
                                " ,@DeliveryNoteNo" +
                                " ,@DispatchDocNo" +
                                " ,@VoucherClassId" +
                                " ,@SalesLedgerId" +
                                ",@brokerName" +
                                 " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            cmdInsert.Parameters.Add("@InvoiceTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceTypeId;
            cmdInsert.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportOrgId);
            cmdInsert.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
            cmdInsert.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CurrencyId;
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LoadingSlipId);
            cmdInsert.Parameters.Add("@DistributorOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DistributorOrgId);
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DealerOrgId);
            cmdInsert.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.FinYearId;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.StatusId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.UpdatedBy);
            cmdInsert.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.InvoiceDate;
            cmdInsert.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.StatusDate;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.UpdatedOn);
            cmdInsert.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CurrencyRate;
            cmdInsert.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.BasicAmt;
            cmdInsert.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.DiscountAmt;
            cmdInsert.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.TaxableAmt;
            cmdInsert.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CgstAmt;
            cmdInsert.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SgstAmt;
            cmdInsert.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IgstAmt;
            cmdInsert.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightPct;
            cmdInsert.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightAmt;
            cmdInsert.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.RoundOffAmt;
            cmdInsert.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.GrandTotal;
            cmdInsert.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
            cmdInsert.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
            cmdInsert.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VehicleNo);
            cmdInsert.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
            cmdInsert.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
            cmdInsert.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
            cmdInsert.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
            cmdInsert.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
            cmdInsert.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
            cmdInsert.Parameters.Add("@invoiceModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceModeId);
            cmdInsert.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
            cmdInsert.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
            cmdInsert.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.NetWeight);
            cmdInsert.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);
            cmdInsert.Parameters.Add("@changeIn", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ChangeIn);
            cmdInsert.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ExpenseAmt;
            cmdInsert.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.OtherAmt;
            cmdInsert.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsConfirmed;
            cmdInsert.Parameters.Add("@rcmFlag", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RcmFlag);
            cmdInsert.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Remark);
            cmdInsert.Parameters.Add("@invFromOrgId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgId;
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BrandId);
            cmdInsert.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.PoNo);
            cmdInsert.Parameters.Add("@poDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.PoDate);//Vijaymala [2018-02-26] Added
            cmdInsert.Parameters.Add("@deliveredOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveredOn);
            cmdInsert.Parameters.Add("@ORCPersonName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ORCPersonName);
            cmdInsert.Parameters.Add("@grossWtTakenDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.GrossWtTakenDate;
            cmdInsert.Parameters.Add("@preparationDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.PreparationDate;
            cmdInsert.Parameters.Add("@InvFromOrgFreeze", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgFreeze;
            cmdInsert.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvComment);
            cmdInsert.Parameters.Add("@tdsAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TdsAmt);
            cmdInsert.Parameters.Add("@DispatchDocNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DispatchDocNo);
            cmdInsert.Parameters.Add("@DeliveryNoteNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryNoteNo);
            cmdInsert.Parameters.Add("@VoucherClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VoucherClassId);
            cmdInsert.Parameters.Add("@SalesLedgerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.SalesLedgerId);
            cmdInsert.Parameters.Add("@brokerName", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BrokerName);
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblInvoiceTO.IdInvoice = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }

            return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblInvoiceTO, cmdUpdate);
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

        public int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblInvoiceTO, cmdUpdate);
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

        public int UpdateInvoiceNonCommercDtls(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +
                            "  [transportModeId]= @TransportModeId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[lrDate]= @LrDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[electronicRefNo]= @ElectronicRefNo" +
                            " ,[lrNumber]= @LrNumber" +
                            " ,[roadPermitNo]= @RoadPermitNo" +
                            " ,[transportorForm]= @TransportorForm" +
                            " ,[airwayBillNo]= @AirwayBillNo" +
                            " ,[narration]= @Narration" +
                            " ,[bankDetails] = @BankDetails" +
                            " ,[deliveryLocation] = @deliveryLocation " +
                            " ,[deliveryNoteNo] = @DeliveryNoteNo " +
                            " ,[dispatchDocNo] = @DispatchDocNo " +
                            " WHERE [idInvoice] = @IdInvoice";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
                cmdUpdate.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
                cmdUpdate.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
                cmdUpdate.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
                cmdUpdate.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
                cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
                cmdUpdate.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
                cmdUpdate.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
                cmdUpdate.Parameters.Add("@DispatchDocNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DispatchDocNo);
                cmdUpdate.Parameters.Add("@DeliveryNoteNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryNoteNo);

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

        public int UpdateMappedSAPInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = "UPDATE tempInvoice SET sapMappedSalesOrderNo = @SapMappedSalesOrderNo, sapMappedSalesInvoiceNo = @SapMappedSalesInvoiceNo WHERE idInvoice = @IdInvoice";
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@SapMappedSalesInvoiceNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SapMappedSalesInvoiceNo;
                cmdUpdate.Parameters.Add("@SapMappedSalesOrderNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SapMappedSalesOrderNo;
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


        public int UpdateWhatsAppMsgSendInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandText = "UPDATE tempInvoice SET isSendWhatsAppMsg = @isSendWhatsAppMsg  WHERE idInvoice = @IdInvoice";
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int  ).Value = tblInvoiceTO.IdInvoice ;
                cmdUpdate.Parameters.Add("@isSendWhatsAppMsg", System.Data.SqlDbType.Bit ).Value = tblInvoiceTO.IsSendWhatsAppMsg; 
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
        public int UpdateInvoiceNonCommercDtlsForFinal(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [finalInvoice] SET " +
                            "  [transportModeId]= @TransportModeId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[lrDate]= @LrDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[electronicRefNo]= @ElectronicRefNo" +
                            " ,[lrNumber]= @LrNumber" +
                            " ,[roadPermitNo]= @RoadPermitNo" +
                            " ,[transportorForm]= @TransportorForm" +
                            " ,[airwayBillNo]= @AirwayBillNo" +
                            " ,[narration]= @Narration" +
                            " ,[bankDetails] = @BankDetails" +
                            " ,[deliveryLocation] = @deliveryLocation " +
                            " WHERE [idInvoice] = @IdInvoice";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
                cmdUpdate.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
                cmdUpdate.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
                cmdUpdate.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
                cmdUpdate.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
                cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
                cmdUpdate.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
                cmdUpdate.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);

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
        public int ExecuteUpdationCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempInvoice] SET " +
                            "  [invoiceTypeId]= @InvoiceTypeId" +
                            " ,[transportOrgId]= @TransportOrgId" +
                            " ,[transportModeId]= @TransportModeId" +
                            " ,[currencyId]= @CurrencyId" +
                            " ,[loadingSlipId]= @LoadingSlipId" +
                            " ,[distributorOrgId]= @DistributorOrgId" +
                            " ,[dealerOrgId]= @DealerOrgId" +
                            " ,[finYearId]= @FinYearId" +
                            " ,[statusId]= @StatusId" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[invoiceDate]= @InvoiceDate" +
                            " ,[lrDate]= @LrDate" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[currencyRate]= @CurrencyRate" +
                            " ,[basicAmt]= @BasicAmt" +
                            " ,[discountAmt]= @DiscountAmt" +
                            " ,[taxableAmt]= @TaxableAmt" +
                            " ,[cgstAmt]= @CgstAmt" +
                            " ,[sgstAmt]= @SgstAmt" +
                            " ,[igstAmt]= @IgstAmt" +
                            " ,[freightPct]= @FreightPct" +
                            " ,[freightAmt]= @FreightAmt" +
                            " ,[roundOffAmt]= @RoundOffAmt" +
                            " ,[grandTotal]= @GrandTotal" +
                            " ,[invoiceNo]= @InvoiceNo" +
                            " ,[electronicRefNo]= @ElectronicRefNo" +
                            " ,[vehicleNo]= @VehicleNo" +
                            " ,[lrNumber]= @LrNumber" +
                            " ,[roadPermitNo]= @RoadPermitNo" +
                            " ,[transportorForm]= @TransportorForm" +
                            " ,[airwayBillNo]= @AirwayBillNo" +
                            " ,[narration]= @Narration" +
                            " ,[bankDetails] = @BankDetails" +
                            " ,[invoiceModeId] = @invoiceModeId " +
                            " ,[deliveryLocation] = @deliveryLocation " +
                             " ,[tareWeight] = @tareWeight " +
                            " ,[netWeight] = @netWeight " +
                            " ,[grossWeight] = @grossWeight " +
                             " ,[changeIn] = @changeIn " +
                            " ,[expenseAmt] = @expenseAmt " +
                            " ,[otherAmt] = @otherAmt " +
                            " ,[isConfirmed] = @isConfirmed " +
                            " ,[rcmFlag] = @rcmFlag " +
                            " ,[remark] = @remark " +
                            " ,[invFromOrgId] = @invFromOrgId " +
                            " ,[poNo] = @poNo" + //Vijaymala [2018-02-26] Added
                            " ,[poDate]=@poDate " +   //Vijaymala [2018-02-26] Added
                            " ,[deliveredOn]=@deliveredOn " +

                             " ,[orcPersonName]=@ORCPersonName " +
                             " ,[grossWtTakenDate]=@grossWtTakenDate " +
                             " ,[preparationDate]=@preparationDate " +
                             " ,[invFromOrgFreeze]=@InvFromOrgFreeze " +
                             " ,[comment]=@comment " +
                             " ,[tdsAmt]=@tdsAmt " +
                             " ,[deliveryNoteNo]=@DeliveryNoteNo " +
                             " ,[dispatchDocNo]=@DispatchDocNo " +
                             " ,[voucherClassId]=@VoucherClassId " +
                             " ,[salesLedgerId]=@SalesLedgerId " +
                             " ,[brokerName]=@brokerName "+
                             " WHERE [idInvoice] = @IdInvoice";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            cmdUpdate.Parameters.Add("@InvoiceTypeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceTypeId;
            cmdUpdate.Parameters.Add("@TransportOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportOrgId);
            cmdUpdate.Parameters.Add("@TransportModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportModeId);
            cmdUpdate.Parameters.Add("@CurrencyId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.CurrencyId;
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LoadingSlipId);
            cmdUpdate.Parameters.Add("@DistributorOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DistributorOrgId);
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DealerOrgId);
            cmdUpdate.Parameters.Add("@FinYearId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.FinYearId;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.StatusId;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.InvoiceDate;
            cmdUpdate.Parameters.Add("@LrDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrDate);
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.StatusDate;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@CurrencyRate", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CurrencyRate;
            cmdUpdate.Parameters.Add("@BasicAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.BasicAmt;
            cmdUpdate.Parameters.Add("@DiscountAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.DiscountAmt;
            cmdUpdate.Parameters.Add("@TaxableAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.TaxableAmt;
            cmdUpdate.Parameters.Add("@CgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.CgstAmt;
            cmdUpdate.Parameters.Add("@SgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.SgstAmt;
            cmdUpdate.Parameters.Add("@IgstAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IgstAmt;
            cmdUpdate.Parameters.Add("@FreightPct", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightPct;
            cmdUpdate.Parameters.Add("@FreightAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.FreightAmt;
            cmdUpdate.Parameters.Add("@RoundOffAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.RoundOffAmt;
            cmdUpdate.Parameters.Add("@GrandTotal", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.GrandTotal;
            cmdUpdate.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
            cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
            cmdUpdate.Parameters.Add("@VehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VehicleNo);
            cmdUpdate.Parameters.Add("@LrNumber", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.LrNumber);
            cmdUpdate.Parameters.Add("@RoadPermitNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RoadPermitNo);
            cmdUpdate.Parameters.Add("@TransportorForm", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TransportorForm);
            cmdUpdate.Parameters.Add("@AirwayBillNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.AirwayBillNo);
            cmdUpdate.Parameters.Add("@Narration", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Narration);
            cmdUpdate.Parameters.Add("@BankDetails", System.Data.SqlDbType.NChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BankDetails);
            cmdUpdate.Parameters.Add("@invoiceModeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceModeId);
            cmdUpdate.Parameters.Add("@deliveryLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryLocation);
            cmdUpdate.Parameters.Add("@tareWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TareWeight);
            cmdUpdate.Parameters.Add("@netWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.NetWeight);
            cmdUpdate.Parameters.Add("@grossWeight", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.GrossWeight);
            cmdUpdate.Parameters.Add("@changeIn", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ChangeIn);
            cmdUpdate.Parameters.Add("@expenseAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ExpenseAmt;
            cmdUpdate.Parameters.Add("@otherAmt", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.OtherAmt;
            cmdUpdate.Parameters.Add("@isConfirmed", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@rcmFlag", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.RcmFlag);
            cmdUpdate.Parameters.Add("@remark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.Remark);
            cmdUpdate.Parameters.Add("@invFromOrgId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgId;
            cmdUpdate.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.PoNo);
            cmdUpdate.Parameters.Add("@poDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.PoDate);
            cmdUpdate.Parameters.Add("@deliveredOn", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveredOn);
            cmdUpdate.Parameters.Add("@ORCPersonName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ORCPersonName);
            cmdUpdate.Parameters.Add("@grossWtTakenDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.GrossWtTakenDate;
            cmdUpdate.Parameters.Add("@preparationDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.PreparationDate;
            cmdUpdate.Parameters.Add("@InvFromOrgFreeze", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvFromOrgFreeze;
            cmdUpdate.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvComment);
            cmdUpdate.Parameters.Add("@tdsAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.TdsAmt);
            cmdUpdate.Parameters.Add("@DeliveryNoteNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DeliveryNoteNo);
            cmdUpdate.Parameters.Add("@DispatchDocNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DispatchDocNo);
            cmdUpdate.Parameters.Add("@VoucherClassId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.VoucherClassId);
            cmdUpdate.Parameters.Add("@SalesLedgerId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.SalesLedgerId);
            cmdUpdate.Parameters.Add("@brokerName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.BrokerName);


            return cmdUpdate.ExecuteNonQuery();
        }

        public int UpdateInvoiceDate(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tempInvoice] SET "+
                
                 "  [invoiceDate] = @InvoiceDate "+
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@InvoiceDate", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.InvoiceDate;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

                return cmdUpdate.ExecuteNonQuery();
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

        public int UpdateEInvoicNo(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 "  [IrnNo] = @IrnNo " +
                 " ,[isEInvGenerated]= @IsEInvGenerated" +
                 " ,[distanceInKM]= @DistanceInKM" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IrnNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IrnNo;
                cmdUpdate.Parameters.Add("@IsEInvGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEInvGenerated;
                cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

                return cmdUpdate.ExecuteNonQuery();
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

        public int UpdateEInvoicNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 "  [IrnNo] = @IrnNo " +
                 " ,[isEInvGenerated]= @IsEInvGenerated" +
                 //" ,[distanceInKM]= @DistanceInKM" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IrnNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.IrnNo;
                cmdUpdate.Parameters.Add("@IsEInvGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEInvGenerated;
                //cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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

        public int UpdateEWayBill(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 " [isEwayBillGenerated]= @IsEwayBillGenerated" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IsEwayBillGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEWayBillGenerated;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

                return cmdUpdate.ExecuteNonQuery();
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

        public int PostUpdateInvoiceStatus(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = Startup.ConnectionString;
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                String sqlQuery = @" UPDATE [tempInvoice] SET " +
                 "  [invoiceModeId] = @InvoiceModeId " +
                 " ,[statusId]= @StatusId" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " ,[IrnNo]= @IrnNo" +
                 " ,[electronicRefNo]= @ElectronicRefNo" +
                 " ,[isEInvGenerated]= @IsEInvGenerated" +
                 " ,[isEwayBillGenerated]= @IsEwayBillGenerated" +
                 " ,[invoiceNo]= @InvoiceNo" +
                 " ,[distanceInKM]= @DistanceInKM" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.StatusId;
                cmdUpdate.Parameters.Add("@InvoiceModeId", System.Data.SqlDbType.Int).Value = tblInvoiceTO.InvoiceModeId;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@IrnNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.IrnNo);
                cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.ElectronicRefNo);
                cmdUpdate.Parameters.Add("@IsEInvGenerated", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.IsEInvGenerated);
                cmdUpdate.Parameters.Add("@IsEwayBillGenerated", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.IsEWayBillGenerated);
                cmdUpdate.Parameters.Add("@InvoiceNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.InvoiceNo);
                cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblInvoiceTO.DistanceInKM);

                return cmdUpdate.ExecuteNonQuery();
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

        public int UpdateEWayBill(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();

            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;

                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 " [isEwayBillGenerated]= @IsEwayBillGenerated" +
                 " ,[electronicRefNo]= @ElectronicRefNo" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@IsEwayBillGenerated", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IsEWayBillGenerated;
                cmdUpdate.Parameters.Add("@ElectronicRefNo", System.Data.SqlDbType.NVarChar).Value = tblInvoiceTO.ElectronicRefNo;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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

        public int UpdateTempInvoiceDistanceInKM(TblInvoiceTO tblInvoiceTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                
                String sqlQuery = @" UPDATE [tempInvoice] SET " +

                 " [distanceInKM]= @DistanceInKM" +
                 " ,[updatedBy]= @UpdatedBy" +
                 " ,[updatedOn]= @UpdatedOn" +
                 " WHERE [idInvoice] = @IdInvoice ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@IdInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
                cmdUpdate.Parameters.Add("@DistanceInKM", System.Data.SqlDbType.Decimal).Value = tblInvoiceTO.DistanceInKM;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblInvoiceTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblInvoiceTO.UpdatedOn;

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
        #endregion

        #region Deletion
        public int DeleteTblInvoice(Int32 idInvoice)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idInvoice, cmdDelete);
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

        public int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idInvoice, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idInvoice, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempInvoice] " +
            " WHERE idInvoice = " + idInvoice + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idInvoice", System.Data.SqlDbType.Int).Value = tblInvoiceTO.IdInvoice;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        #region Reports


        public List<TblOtherTaxRpt> SelectOtherTaxDetailsReport (DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId,int fromOrgId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DateTime sysDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();

                String whereCondition = "WHERE CAST(invoice.statusDate AS DATETIME) BETWEEN @fromDate AND @toDate ";

                whereCondition += " AND ISNULL(isConfirmed,0) = " + isConfirm;

                if (otherTaxId > 0)
                {
                    whereCondition += "AND otherTaxId = " + otherTaxId;
                }
                else
                {
                    whereCondition += "AND otherTaxId > 0 ";
                }
                //chetan[13-feb-2020] added for fillteron from org id
                if(fromOrgId > 0)
                {
                    whereCondition += "AND invFromOrgId= "+ fromOrgId;
                }

                selectQuery = " SELECT " +
                    " invoice.idInvoice " +
                    " ,invoice.invoiceNo " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.netWeight " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.invoiceDate " +
                    " ,Invoice.deliveryLocation " +
                    " ,invoice.isConfirmed " +
                    " ,invoice.statusId " +
                    " ,Invoice.statusDate " +
                    " ,InvoiceItemDetails.basicTotal " +
                    " ,InvoiceItemDetails.grandTotal " +
                    " ,dealer.firmName as partyName " +
                    " ,distributor.firmName AS cnfName " +
                    " ,transport.firmName AS transporterName " +
                    " ,InvoiceItemDetails.idInvoiceItem " +
                    " ,InvoiceItemDetails.prodItemDesc " +
                    " ,InvoiceItemDetails.taxableAmt " +
                    " ,InvoiceItemDetails.taxAmt " +
                    " ,InvoiceItemDetails.otherTaxId " +
                    " ,currencyName " +
                    " ,statusName " +
                    " ,invoiceTypeDesc " +
                    " FROM tempInvoiceItemDetails InvoiceItemDetails " +
                    " LEFT JOIN tempInvoice Invoice ON InvoiceItemDetails.invoiceId = Invoice.idInvoice " +
                    " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                    " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                    " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                    " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                    " " + whereCondition +
                    " UNION ALL " +
                    " SELECT " +
                     " invoice.idInvoice " +
                    " ,invoice.invoiceNo " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.netWeight " +
                    " ,invoice.vehicleNo " +
                    " ,invoice.invoiceDate " +
                    " ,Invoice.deliveryLocation " +
                    " ,invoice.isConfirmed " +
                    " ,invoice.statusId " +
                    " ,Invoice.statusDate " +
                    " ,InvoiceItemDetails.basicTotal " +
                    " ,InvoiceItemDetails.grandTotal " +
                    " ,dealer.firmName as partyName " +
                    " ,distributor.firmName AS cnfName " +
                    " ,transport.firmName AS transporterName " +
                    " ,InvoiceItemDetails.idInvoiceItem " +
                    " ,InvoiceItemDetails.prodItemDesc " +
                    " ,InvoiceItemDetails.taxableAmt " +
                    " ,InvoiceItemDetails.taxAmt " +
                    " ,InvoiceItemDetails.otherTaxId " +
                    " ,currencyName " +
                    " ,statusName " +
                    " ,invoiceTypeDesc " +
                    " FROM finalInvoiceItemDetails InvoiceItemDetails " +
                    " LEFT JOIN finalInvoice Invoice ON InvoiceItemDetails.invoiceId = Invoice.idInvoice " +
                    " LEFT JOIN tblOrganization dealer ON dealer.idOrganization = invoice.dealerOrgId " +
                    " LEFT JOIN tblOrganization distributor ON distributor.idOrganization = invoice.distributorOrgId " +
                    " LEFT JOIN tblOrganization transport ON transport.idOrganization = invoice.transportOrgId " +
                    " LEFT JOIN dimInvoiceStatus ON idInvStatus = invoice.statusId " +
                    " LEFT JOIN dimInvoiceTypes ON idInvoiceType = invoice.invoiceTypeId " +
                    " LEFT JOIN dimCurrency ON idCurrency = invoice.currencyId " +
                    " " + whereCondition;


                cmdSelect.Connection = conn;
                cmdSelect.CommandText = selectQuery;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblOtherTaxRpt> list = ConvertDTToOtherTaxRptTOList(reader);
                return list;

            }
            catch (Exception ex)
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
        public List<TblOtherTaxRpt> ConvertDTToOtherTaxRptTOList(SqlDataReader tblInvoiceRptTODT)
        {
            List<TblOtherTaxRpt> tblOtherTaxRptList = new List<TblOtherTaxRpt>();
            try
            {
                if (tblInvoiceRptTODT != null)
                {

                    while (tblInvoiceRptTODT.Read())
                    {
                        TblOtherTaxRpt tblOtherTaxRptNew = new TblOtherTaxRpt();
                        for (int i = 0; i < tblInvoiceRptTODT.FieldCount; i++)
                        {
                            if (tblInvoiceRptTODT.GetName(i).Equals("idInvoice"))
                            {
                                if (tblInvoiceRptTODT["idInvoice"] != DBNull.Value)
                                    tblOtherTaxRptNew.IdInvoice = Convert.ToInt32(tblInvoiceRptTODT["idInvoice"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceNo"))
                            {
                                if (tblInvoiceRptTODT["invoiceNo"] != DBNull.Value)
                                    tblOtherTaxRptNew.InvoiceNo = Convert.ToString(tblInvoiceRptTODT["invoiceNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("vehicleNo"))
                            {
                                if (tblInvoiceRptTODT["vehicleNo"] != DBNull.Value)
                                    tblOtherTaxRptNew.VehicleNo = Convert.ToString(tblInvoiceRptTODT["vehicleNo"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceDate"))
                            {
                                if (tblInvoiceRptTODT["invoiceDate"] != DBNull.Value)
                                    tblOtherTaxRptNew.InvoiceDate = Convert.ToDateTime(tblInvoiceRptTODT["invoiceDate"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("netWeight"))
                            {
                                if (tblInvoiceRptTODT["netWeight"] != DBNull.Value)
                                    tblOtherTaxRptNew.NetWeight = Convert.ToDouble(tblInvoiceRptTODT["netWeight"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i) == "basicTotal")
                            {
                                if (tblInvoiceRptTODT["basicTotal"] != DBNull.Value)
                                    tblOtherTaxRptNew.BasicAmt = Convert.ToDouble(tblInvoiceRptTODT["basicTotal"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("partyName"))
                            {
                                if (tblInvoiceRptTODT["partyName"] != DBNull.Value)
                                    tblOtherTaxRptNew.PartyName = Convert.ToString(tblInvoiceRptTODT["partyName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("cnfName"))
                            {
                                if (tblInvoiceRptTODT["cnfName"] != DBNull.Value)
                                    tblOtherTaxRptNew.CnfName = Convert.ToString(tblInvoiceRptTODT["cnfName"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("transporterName"))
                            {
                                if (tblInvoiceRptTODT["transporterName"] != DBNull.Value)
                                    tblOtherTaxRptNew.TransporterName = Convert.ToString(tblInvoiceRptTODT["transporterName"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("invoiceItemId"))
                            {
                                if (tblInvoiceRptTODT["invoiceItemId"] != DBNull.Value)
                                    tblOtherTaxRptNew.InvoiceItemId = Convert.ToInt32(tblInvoiceRptTODT["invoiceItemId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("prodItemDesc"))
                            {
                                if (tblInvoiceRptTODT["prodItemDesc"] != DBNull.Value)
                                    tblOtherTaxRptNew.ProdItemDesc = Convert.ToString(tblInvoiceRptTODT["prodItemDesc"].ToString());
                            }

                            if (tblInvoiceRptTODT.GetName(i).Equals("grandTotal"))
                            {
                                if (tblInvoiceRptTODT["grandTotal"] != DBNull.Value)
                                    tblOtherTaxRptNew.GrandTotal = Convert.ToDouble(tblInvoiceRptTODT["grandTotal"].ToString());
                            }


                            if (tblInvoiceRptTODT.GetName(i).Equals("taxableAmt"))
                            {
                                if (tblInvoiceRptTODT["taxableAmt"] != DBNull.Value)
                                    tblOtherTaxRptNew.TaxableAmt = Convert.ToDouble(tblInvoiceRptTODT["taxableAmt"].ToString());
                            }
                           
                            if (tblInvoiceRptTODT.GetName(i).Equals("deliveryLocation"))
                            {
                                if (tblInvoiceRptTODT["deliveryLocation"] != DBNull.Value)
                                    tblOtherTaxRptNew.DeliveryLocation = Convert.ToString(tblInvoiceRptTODT["deliveryLocation"].ToString());
                            }
                            
                            if (tblInvoiceRptTODT.GetName(i).Equals("taxAmt"))
                            {
                                if (tblInvoiceRptTODT["taxAmt"] != DBNull.Value)
                                    tblOtherTaxRptNew.TaxAmt = Convert.ToDouble(tblInvoiceRptTODT["taxAmt"].ToString());
                            }
                            
                           
                            if (tblInvoiceRptTODT.GetName(i).Equals("isConfirmed"))
                            {
                                if (tblInvoiceRptTODT["isConfirmed"] != DBNull.Value)
                                    tblOtherTaxRptNew.IsConfirmed = Convert.ToInt32(tblInvoiceRptTODT["isConfirmed"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("otherTaxId"))
                            {
                                if (tblInvoiceRptTODT["otherTaxId"] != DBNull.Value)
                                    tblOtherTaxRptNew.OtherTaxId = Convert.ToInt32(tblInvoiceRptTODT["otherTaxId"].ToString());
                            }
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusId"))
                            {
                                if (tblInvoiceRptTODT["statusId"] != DBNull.Value)
                                    tblOtherTaxRptNew.StatusId = Convert.ToInt32(tblInvoiceRptTODT["statusId"].ToString());
                            }
                            
                            if (tblInvoiceRptTODT.GetName(i).Equals("statusDate"))
                            {
                                if (tblInvoiceRptTODT["statusDate"] != DBNull.Value)
                                    tblOtherTaxRptNew.StatusDate = Convert.ToDateTime(tblInvoiceRptTODT["statusDate"].ToString());
                            }
                            
                            

                            if (tblInvoiceRptTODT.GetName(i).Equals("statusName"))
                            {
                                if (tblInvoiceRptTODT["statusName"] != DBNull.Value)
                                    tblOtherTaxRptNew.StatusName = Convert.ToString(tblInvoiceRptTODT["statusName"].ToString());
                            }


                        }

                        tblOtherTaxRptList.Add(tblOtherTaxRptNew);

                    }
                }
                // return tblInvoiceTOList;
                return tblOtherTaxRptList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion

    }
}
