using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using System.Linq;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DashboardModels;

namespace ODLMWebAPI.DAL
{ 
    public class TblBookingsDAO : ITblBookingsDAO
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ICommon _iCommon;
        public TblBookingsDAO(ICommon iCommon, IConnectionString iConnectionString, ITblConfigParamsBL iTblConfigParamsBL)
        {
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
        }
        #region Methods
        public String SqlSelectQuery(Int32 loginUserId = 0)
        {
            
            String sqlSelectQry = "SELECT bookings.*,dimStat.statusName as dealerCat,dimStat.colorCode,userCreatedBy.userDisplayName As createdByName,userUpdatedBy.userDisplayName As updatedByName, "+
                                  "orgCnf.firmName as cnfName,orgDealer.isOverdueExist  as isOrgOverDue, tblTranAction.tranActionTypeId As tranActionTypeId," +
                                  " orgDealer.firmName + ',' + " +
                                  " CASE WHEN orgDealer.addrId IS NULL THEN '' Else case WHEN address.villageName IS NOT NULL THEN address.villageName " +
                                  " ELSE CASE WHEN address.talukaName IS NOT NULL THEN address.talukaName ELSE CASE WHEN address.districtName IS NOT NULL THEN address.districtName ELSE address.stateName END END END END AS dealerName," +
                                  " CONCAT (dimStatus.statusName,'-', ISNULL(userStatusBy.userDisplayName,'') ) AS statusName , brandDtl.brandName, address.stateId FROM tblbookings bookings LEFT JOIN tblOrganization orgCnf  ON bookings.cnfOrgId = orgCnf.idOrganization" +

                                  " LEFT JOIN tblTranActions tblTranAction ON tblTranAction.transId = bookings.idBooking AND tblTranAction.userId = "+ loginUserId +
                                  " AND tblTranAction.tranActionTypeId = "+ (Int32)Constants.TranActionTypeE.READ +
                                  " AND tblTranAction.transTypeId = "+ (Int32)Constants.TransactionTypeE.BOOKING +
                                  " LEFT JOIN tblUser userCreatedBy ON userCreatedBy.idUser = bookings.createdBy " +
                                  " LEFT JOIN tblUser userUpdatedBy ON userUpdatedBy.idUser = bookings.updatedBy "+
                                  " LEFT JOIN tblUser userStatusBy ON userStatusBy.idUser = bookings.statusBy " +
                                  " LEFT JOIN tblOrganization orgDealer  ON bookings.dealerOrgId = orgDealer.idOrganization " +
                                  " LEFT JOIN dimStatus ON dimStatus.idStatus = bookings.statusId "+
                                  " LEFT JOIN dimStatus dimStat ON dimStat.idStatus = orgDealer.orgStatusId" +
                                  " LEFT JOIN dimBrand brandDtl ON brandDtl.idBrand = bookings.brandId " +
                                  //" LEFT JOIN tblUserAreaAllocation userAreaAlloc on userAreaAlloc.cnfOrgId = bookings.cnFOrgId "+
                                  //" AND userAreaAlloc.userId = "+ RMId +
                                  " LEFT JOIN vAddressDetails address ON address.idAddr = orgDealer.addrId ";


            //String sqlSelectQry = " SELECT bookings.*, orgCnf.firmName as cnfName,orgDealer.firmName as dealerName, dimStatus.statusName" +
            //                        " ,brandDtl.brandName" +
            //                        " FROM tblbookings bookings " +
            //                        " LEFT JOIN tblOrganization orgCnf " +
            //                        " ON bookings.cnfOrgId = orgCnf.idOrganization " +
            //                        " LEFT JOIN tblOrganization orgDealer " +
            //                        " ON bookings.dealerOrgId = orgDealer.idOrganization " +
            //                        " LEFT JOIN dimStatus ON dimStatus.idStatus=bookings.statusId" +
            //                        " LEFT JOIN dimBrand brandDtl ON brandDtl.idBrand=bookings.brandId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblBookingsTO> SelectAllBookingDateWise(DateTime fromDate, DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            try
            {
                sqlQuery = SqlSelectQuery() +
                    " WHERE CAST(bookingDatetime AS DATE) BETWEEN @fromDate AND @toDate ";
                cmdSelect.CommandText = sqlQuery;
                conn.Open();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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
        public List<TblBookingsTO> SelectAllTblBookings()
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
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public Double SelectTotalPendingBookingQty(DateTime sysDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblLoadingTODT = null;
            int skipOtherQty = 0;
            try
            {

                TblConfigParamsTO tblConfigParams = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.IS_OTHER_MATERIAL_QTY_HIDE_ON_DASHBOARD);
                conn.Open();

                String ignorStatusIds = (Int32)Constants.TranStatusE.BOOKING_DELETE + "";
                if(tblConfigParams!=null)
                {
                    if(tblConfigParams.ConfigParamVal=="1")
                    {
                        skipOtherQty = 1;
                    }
                }
                if(skipOtherQty==1)
                {
                    cmdSelect.CommandText = "SELECT SUM(pendingQty) As bookingQty from tblBookings where bookingType not IN(2) and statusId NOT IN( " + ignorStatusIds + ") and bookingDatetime <= @bookingDate";
                }
                else
                {
                    cmdSelect.CommandText = "SELECT SUM(pendingQty) As bookingQty from tblBookings where statusId NOT IN( " + ignorStatusIds + ") and bookingDatetime <= @bookingDate";
                }


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@bookingDate", System.Data.SqlDbType.DateTime).Value = sysDate;

                tblLoadingTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (tblLoadingTODT.Read())
                {

                    return Convert.ToDouble(tblLoadingTODT["bookingQty"].ToString()); ;
                }

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (tblLoadingTODT != null)
                    tblLoadingTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblBookingsTO> SelectAllBookingsListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idBooking IN(SELECT DISTINCT bookingId FROM tempLoadingSlipDtl WHERE loadingSlipId=" + loadingSlipId + ")" +
                                        // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipDtl

                                        " UNION ALL " +
                                        " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idBooking IN(SELECT DISTINCT bookingId FROM finalLoadingSlipDtl WHERE loadingSlipId=" + loadingSlipId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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

        public TblBookingsTO SelectBookingsDetailsFromInVoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader tblBookingsTODT = null;
            try
            {
                cmdSelect.CommandText = "select booking.bookingRate,booking.brandId  from tempLoadingSlipDtl detl " +
                    " left join tempInvoice invoice ON detl.loadingSlipId = invoice.loadingSlipId" +
                    " left join tblBookings booking ON detl.bookingId = booking.idbooking " +
                    "where idInvoice =" + invoiceId;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                TblBookingsTO tblBookingsTONew = new TblBookingsTO();
                if (tblBookingsTODT != null)
                {
                    while (tblBookingsTODT.Read())
                    {
                        if (tblBookingsTODT["bookingRate"] != DBNull.Value)
                            tblBookingsTONew.BookingRate = Convert.ToDouble(tblBookingsTODT["bookingRate"].ToString());
                        if (tblBookingsTODT["brandId"] != DBNull.Value)
                            tblBookingsTONew.BrandId = Convert.ToInt32(tblBookingsTODT["brandId"].ToString());
                    }
                 
                }
                return tblBookingsTONew;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblBookingsTODT != null) tblBookingsTODT.Dispose();
                cmdSelect.Dispose();
            }
        }
       
        public List<TblBookingsTO> SelectAllBookingsListForApproval(Int32 isConfirmed, Int32 idBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_NEW + "";
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();
                sqlQuery = SqlSelectQuery() + " WHERE IsWithinQuotaLimit=0 AND statusId IN(" + statusIds + ")"; 

                if (isConfirmed == (int)Constants.bookingFilterTypeE.CONFIRMED)
                {
                    sqlQuery += " AND bookings.isConfirmed=1";
                }
                else if (isConfirmed == (int)Constants.bookingFilterTypeE.NOTCONFIRMED)
                {
                    sqlQuery += " AND ISNULL(bookings.isConfirmed,0) = 0";
                }
                
                if (idBrand > 0)
                {
                    sqlQuery += "AND brandDtl.idBrand =" + idBrand; 
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingsTO> SelectAllBookingsListForAcceptance(Int32 cnfId, TblUserRoleTO tblUserRoleTO, Int32 isConfirmed)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            //Vijaymala[20-12-2017] Added:to change status as BOOKING_PENDING_FOR_DIRECTOR_APPROVAL
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL + "";
            //(Int32)Constants.TranStatusE.BOOKING_APPROVED_FINANCE + "";

            String areConfJoin = String.Empty;
            String sqlQuery = String.Empty;
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

                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization,brandId=0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER +
                                 "   AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                 "   Union All " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization = 0,areaConf.brandId  FROM tblUserAreaAllocation areaConf WHERE" +
                                 "   areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                 " ) AS userAreaDealer On (userAreaDealer.cnfOrgId = bookings.cnFOrgId ) AND" +
                                 " ( bookings.dealerOrgId = userAreaDealer.idOrganization  Or bookings.brandId = userAreaDealer.brandId  )";
                }

                if (cnfId == 0)
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE IsWithinQuotaLimit=0 AND statusId IN(" + statusIds + ")";
                else
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE IsWithinQuotaLimit=0 AND statusId IN(" + statusIds + ")" + " AND bookings.cnFOrgId=" + cnfId;

                if (isConfirmed == (int)Constants.bookingFilterTypeE.CONFIRMED)
                {
                    sqlQuery += " AND bookings.isConfirmed=1";
                }
                else if (isConfirmed == (int)Constants.bookingFilterTypeE.NOTCONFIRMED)
                {
                    sqlQuery += " AND ISNULL(bookings.isConfirmed,0) = 0";
                }

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

      //  public List<TblBookingsTO> SelectAllPendingBookingsList(Int32 cnfId, string dateCondOper, Boolean onlyPendingYn, int isTransporterScopeYn,int isConfirmed, DateTime asOnDate,TblUserRoleTO tblUserRoleTO)
        public List<TblBookingsTO> SelectAllPendingBookingsList(Int32 cnfId, string dateCondOper, Boolean onlyPendingYn, int isTransporterScopeYn,int isConfirmed, DateTime asOnDate,int brandId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
            string whereCond = String.Empty;

            try
            {
                conn.Open();
                int isConfEn = 0;
                int userId = 0;
                String areConfJoin = String.Empty;
                if (tblUserRoleTO != null)
                {
                    isConfEn = tblUserRoleTO.EnableAreaAlloc;
                    userId = tblUserRoleTO.UserId;
                }
                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization,brandId=0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER +
                                 "   AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                 "   Union All " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization = 0,areaConf.brandId  FROM tblUserAreaAllocation areaConf WHERE" +
                                 "   areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                 " ) AS userAreaDealer On (userAreaDealer.cnfOrgId = bookings.cnFOrgId ) AND" +
                                 " ( bookings.dealerOrgId = userAreaDealer.idOrganization  Or bookings.brandId = userAreaDealer.brandId  )";
                }

                if (onlyPendingYn)
                {
                    if (cnfId > 0)
                        cmdSelect.CommandText = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE)" + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.pendingQty > 0 AND bookings.cnFOrgId=" + cnfId;
                    else
                        cmdSelect.CommandText = SqlSelectQuery() + areConfJoin + " WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.pendingQty > 0  ";
                }
                else
                {
                    if (cnfId > 0)
                        cmdSelect.CommandText = SqlSelectQuery() + areConfJoin  + " WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")" + " AND bookings.cnFOrgId=" + cnfId;
                    else
                        cmdSelect.CommandText = SqlSelectQuery() + areConfJoin +" WHERE CAST(bookings.createdOn AS DATE) " + dateCondOper + "@asOnDate AND bookings.statusId IN(" + statusIds + ")";
                }

                // Vaibhav [21-Mar-2018] Added to filter isTransporterScopeYn and isConfirmed
                if (isTransporterScopeYn != 2)
                    cmdSelect.CommandText += " AND bookings.transporterScopeYn=" + isTransporterScopeYn;

                if (isConfirmed != 2)
                    cmdSelect.CommandText += " AND bookings.isConfirmed=" + isConfirmed;

                // added by aniket'
                if (brandId != 0)
                    cmdSelect.CommandText += " AND bookings.brandId=" + brandId;
                

                // Vaibhav [16-APril-2018] Added date wise filter.
                cmdSelect.CommandText += whereCond;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@asOnDate", System.Data.SqlDbType.Date).Value = asOnDate.ToString(Constants.AzureDateFormat);
                //cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.DateTime).Value = fromDate.Date.ToString(Constants.AzureDateFormat);
                //cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.DateTime).Value = toDate.Date.ToString(Constants.AzureDateFormat);


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingsTO> SelectTodayLoadedAndDeletedBookingsList(Int32 cnfId, DateTime asOnDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;

            try
            {
                conn.Open();


                if (cnfId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM tempLoadingSlipExt LEFT JOIN tempLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) " +

                                            // Vaibhav [10-Jan-2018] Added to select from final table

                                            " UNION ALL " +

                                            SqlSelectQuery() + " WHERE bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM finalLoadingSlipExt LEFT JOIN finalLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) ";
                else
                    cmdSelect.CommandText = SqlSelectQuery() + " WHERE bookings.cnfOrgId=" + cnfId + " AND bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM tempLoadingSlipExt LEFT JOIN tempLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) " +

                                            // Vaibhav [10-Jan-2018] Added to select from final table

                                            " UNION ALL " +

                                            SqlSelectQuery() + " WHERE bookings.cnfOrgId=" + cnfId + " AND bookings.idBooking IN(SELECT bookingId FROM tblBookingQtyConsumption WHERE bookingId IN( " +
                                            " SELECT DISTINCT bookingId FROM finalLoadingSlipExt LEFT JOIN finalLoadingSlip ON idLoadingSlip = loadingSlipId " +
                                            " WHERE DAY(createdOn)= " + asOnDate.Day + " AND MONTH(createdOn)=" + asOnDate.Month + " AND YEAR(createdOn)= " + asOnDate.Year + ") " +
                                            " AND weightTolerance IS NULL) ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingsTO> SelectAllTodaysBookingsWithOpeningBalance(Int32 cnfId, DateTime asOnDate,int isTransporterScopeYn,int isConfirmed,int brandId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();

            try
            {
                conn.Open();


                if (cnfId == 0)
                    cmdSelect.CommandText = SqlSelectQuery() + " INNER JOIN tblBookingOpngBal ON bookings.idBooking = tblBookingOpngBal.bookingId " +
                                            "WHERE DAY(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Day + " AND MONTH(tblBookingOpngBal.balAsOnDate)=" + asOnDate.Month + " AND YEAR(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Year ;

                else
                    cmdSelect.CommandText = SqlSelectQuery() + " INNER JOIN tblBookingOpngBal ON bookings.idBooking = tblBookingOpngBal.bookingId " +
                                             "WHERE bookings.cnfOrgId=" + cnfId + " AND DAY(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Day + " AND MONTH(tblBookingOpngBal.balAsOnDate)=" + asOnDate.Month + " AND YEAR(tblBookingOpngBal.balAsOnDate)= " + asOnDate.Year ;


                // Vaibhav [21-Mar-2018] Added to filter isTransporterScopeYn and isConfirmed
                if (isTransporterScopeYn != 2)
                    cmdSelect.CommandText += " AND bookings.transporterScopeYn=" + isTransporterScopeYn;

                if (isConfirmed != 2)
                    cmdSelect.CommandText += " AND bookings.isConfirmed=" + isConfirmed;
                // added by aniket
                if (brandId != 0)
                    cmdSelect.CommandText += "AND bookings.brandId=" + brandId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;            

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords, Boolean pendingYn , Int32 bookingId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            String statusIds = (int)Constants.TranStatusE.BOOKING_APPROVED + "," + (int)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
            if (pendingYn)
                whereCond = " AND pendingQty > 0";
            try
            {
                conn.Open();

                if (bookingId > 0)
                {
                    whereCond += " AND bookings.idBooking !=  " + bookingId;
                }

                cmdSelect.CommandText = " SELECT TOP " + lastNRecords + " bookings.*, userCreatedBy.userDisplayName As createdByName, " +
                                         "userUpdatedBy.userDisplayName As updatedByName, orgDealer.isOverdueExist  as isOrgOverDue, " +
                                         //" tblTranAction.tranActionTypeId As tranActionTypeId ," +
                                        " orgCnf.firmName as cnfName,orgDealer.firmName as dealerName ,dimStatus.statusName" +
                                        " ,brandDtl.brandName " +
                                        " FROM tblbookings bookings " +
                                        " LEFT JOIN tblOrganization orgCnf " +
                                        " ON bookings.cnfOrgId = orgCnf.idOrganization " +
                                        " LEFT JOIN tblOrganization orgDealer " +
                                        " ON bookings.dealerOrgId = orgDealer.idOrganization " +
                                         //" LEFT JOIN tblTranActions tblTranAction ON tblTranAction.transId = bookings.idBooking "+
                                        " LEFT JOIN tblUser userCreatedBy ON userCreatedBy.idUser = bookings.createdBy " +
                                        " LEFT JOIN tblUser userUpdatedBy ON userUpdatedBy.idUser = bookings.updatedBy " +
                                        " LEFT JOIN dimStatus ON dimStatus.idStatus=bookings.statusId" +
                                        " LEFT JOIN dimBrand brandDtl ON brandDtl.idBrand=bookings.brandId" +
                                        " WHERE dealerOrgId=" + dealerId +
                                        " AND bookings.statusId IN(" + statusIds + ") " + whereCond +
                                        " ORDER BY bookings.createdOn DESC ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToListForBookingHistory(sqlReader);
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

        public List<TblBookingsTO> SelectAllBookingList(Int32 cnfId, Int32 dealerId, TblUserRoleTO tblUserRoleTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
            String areConfJoin = String.Empty;
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

                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization,brandId=0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                
                                 "   UNION ALL " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization = 0, areaConf.brandId FROM tblUserAreaAllocation areaConf where  areaConf.userId = " + userId + " " +
                                 "   AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND " +
                                 " ( userAreaDealer.brandId = bookings.brandId or bookings.dealerOrgId = userAreaDealer.idOrganization )";
                }

                if (cnfId == 0)
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE bookings.dealerOrgId=" + dealerId + " AND pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";
                else
                    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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
        
        public List<TblBookingsTO> GetOrderwiseDealerList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR + "," +
                (Int32)Constants.TranStatusE.BOOKING_NEW;
           // String areConfJoin = String.Empty;
          //  int isConfEn = 0;
          //  int userId = 0;
            //if (tblUserRoleTO != null)
            //{
            //    isConfEn = tblUserRoleTO.EnableAreaAlloc;
            //    userId = tblUserRoleTO.UserId;
            //}

            try
            {
                conn.Open();

                //if (isConfEn == 1)
                //{

                //    areConfJoin = " INNER JOIN " +
                //                 " ( " +
                //                 "   SELECT areaConf.cnfOrgId, idOrganization  FROM tblOrganization " +
                //                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                //                 "   INNER JOIN " +
                //                 "   ( " +
                //                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                //                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                //                 "  ) addrDtl  ON idOrganization = organizationId " +
                //                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                //                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                //                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND bookings.dealerOrgId = userAreaDealer.idOrganization ";
                //}

                //if (cnfId == 0)
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE bookings.dealerOrgId=" + dealerId + " AND pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";
                //else
                //    sqlQuery = SqlSelectQuery() + areConfJoin + " WHERE bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";
                
                cmdSelect.CommandText = SqlSelectQuery() + " Where pendingQty > 0 AND bookings.statusId IN(" + statusIds + ")";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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
        //Aniket [16-Jan-2019] added to view cnFList against confirm and not confirmbooking
        public List<CnFWiseReportTO> SelectCnfCNCBookingReport(DateTime fromDate,DateTime toDate)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
          
            string configval = "";
          TblConfigParamsTO tblConfigParamsTO= _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CNF_BOOKING_REPORT_EXCLUDE_STATUSID);
            if(tblConfigParamsTO!=null)
            {
                 configval = tblConfigParamsTO.ConfigParamVal;
              
            }
            try
            {
                sqlQuery = "select SUM(case b.isConfirmed when 1 then b.bookingQty else null end) as confirmed, SUM(case b.isConfirmed when 0 then b.bookingQty else null end) as notConfirmed,o.firmName " +
                    " from tblBookings b JOIN tblOrganization o on o.idOrganization = b.cnFOrgId "+ 
                    " where o.orgTypeId = 1 AND CAST(b.createdOn as date) BETWEEN CAST ( @fromDate as date)"+
                    " AND CAST( @toDate as date) AND b.statusId NOT IN(" + configval + ")"+
                    " GROUP BY cnFOrgId,firmName order by o.firmName";
                cmdSelect.CommandText = sqlQuery;
                conn.Open();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<CnFWiseReportTO> list = ConvertDTCNCcnFBookingReport(sqlReader);
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
        public List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, Int32 statusId, DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 confirm, Int32 isPendingQty, Int32 bookingId, Int32 isViewAllPendingEnq ,Int32 RMId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String areConfJoin = String.Empty;
            String notDelStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "";
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            if (RMId > 0)
            {
                isConfEn = 1;
                userId = RMId;
            }
            try
            {
                conn.Open();

                if (isConfEn == 1)
                {
                    //areConfJoin += " AND bookings.dealerOrgId IN(SELECT distinct idOrganization " +
                    //            " FROM tblOrganization INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                    //            " INNER JOIN " +
                    //            " ( " +
                    //            "    SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                    //            "    INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                    //            " ) addrDtl " +
                    //            " ON idOrganization = organizationId " +
                    //            " INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId=areaConf.districtId" +
                    //            " AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId " +
                    //            " WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ) ";

                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId,  idOrganization, brandId = 0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                  "   UNION ALL " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization = 0, areaConf.brandId FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + "   AND areaConf.isActive = 1 " +
                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND " +
                                 " ( userAreaDealer.brandId = bookings.brandId or bookings.dealerOrgId = userAreaDealer.idOrganization )";

                }
                
                Boolean isWhereAddded = true;
                String whereCondtionStr = string.Empty;
                whereCondtionStr = "WHERE CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate";

                if (bookingId > 0)
                {
                    whereCondtionStr = " WHERE bookings.idBooking =" + bookingId;
                }

                //Priyanka [07-09-2018] : Added to skip date filter in view pendiing enquiries.
                if (isViewAllPendingEnq > 0)
                {
                    fromDate = new DateTime(2001, 01, 01);
                    whereCondtionStr = "WHERE CAST(bookings.createdOn AS DATE) > @fromDate  ";
                }
              
                if (cnfId == 0 && dealerId == 0 && statusId == 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId == 0 && dealerId == 0 && statusId > 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.statusId IN(" + statusId + ")";
                else if (cnfId == 0 && dealerId > 0 && statusId > 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.dealerOrgId=" + dealerId + "AND bookings.statusId IN(" + statusId + ")";
                else if (cnfId == 0 && dealerId > 0 && statusId == 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.dealerOrgId=" + dealerId + "AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId > 0 && dealerId == 0 && statusId == 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.cnFOrgId=" + cnfId + "AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId > 0 && dealerId > 0 && statusId == 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId NOT IN(" + notDelStatus + ")";
                else if (cnfId > 0 && dealerId == 0 && statusId > 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.cnFOrgId=" + cnfId + " AND bookings.statusId IN(" + statusId + ")";
                else if (cnfId > 0 && dealerId > 0 && statusId > 0)
                    sqlQuery = SqlSelectQuery(userId) + areConfJoin + whereCondtionStr + " AND bookings.cnFOrgId=" + cnfId + " AND bookings.dealerOrgId=" + dealerId + " AND bookings.statusId IN(" + statusId + ")";
                else
                {
                    isWhereAddded = false;
                }

                //Pandurang [2018-02-22] Added for Confirm and non Confirm View booking filter. 

                if (confirm > 0)
                {
                    if (confirm == 1)
                    {
                        if (isWhereAddded)
                            sqlQuery += " AND bookings.isConfirmed = 1";
                        else
                            sqlQuery += " WHERE bookings.isConfirmed = 1";
                    }
                    else
                    {
                        if (isWhereAddded)
                            sqlQuery += " AND ISNULL(bookings.isConfirmed, 0) = 0";
                        else
                            sqlQuery += " WHERE ISNULL(bookings.isConfirmed,0) = 0";
                    }
                }

               
                //Priyanka [14-08-2018] : Added flag for view pending enquiry.
                if (isPendingQty == 1)
                {
                   sqlQuery += " AND bookings.pendingQty > 0";
                }

                //Priyanka [14-08-2018] : Added flag for finance approval.
                if(isPendingQty == 2)
                {
                    sqlQuery += " AND bookings.statusId IN ( " + (int)Constants.TranStatusE.BOOKING_NEW +"," + (int)Constants.TranStatusE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR + ") ";
                    sqlQuery += " AND bookings.isWithinQuotaLimit=0";
                }
          
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);               
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

        /// <summary>
        /// Priyanka [14-03-2018] : Added for booking summary report.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="typeId"></param>
        /// <param name="masterId"></param>
        /// <returns></returns>

        public List<TblBookingSummaryTO> SelectBookingSummaryList(Int32 typeId, Int32 masterId, DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            string statusIds = String.Empty;

           // string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;

            string ids = string.Empty;
            ResultMessage resultMessage = new ResultMessage(); ;

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ENQUIRY_STATISTICS_REPORT_STATUS);
            if (tblConfigParamsTO != null)
            {
                ids = tblConfigParamsTO.ConfigParamVal;
            }
            String distStateJoin = String.Empty;
            String selectSqlQuery = String.Empty;
            String dateJoin = String.Empty;
           // String notDelStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "";
            String areConfJoin = String.Empty;
            int isConfEn = 0;
            int userId = 0;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }
            try
            {

                if (cnfId > 0)
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND cnfOrgId=" + cnfId;
                    }
                    else
                    {
                        //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                    " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*,organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON" +
                                   " ( addrDtl.districtId=areaConf.districtId AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId ) " +
                                    " Or (areaConf.cnfOrgId=tblCnfDealers.cnfOrgId )" +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.cnfOrgId=" + cnfId + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }
                else
                {
                    if (isConfEn == 0)
                    {
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                               " FROM tblOrganization " +
                               " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                 " INNER JOIN " +
                               " ( " +
                               " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                               " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                               " ) addrDtl " +
                               " ON idOrganization = organizationId " +
                               " WHERE tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER;
                    }
                    else
                    {
                        //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                        sqlQuery = " SELECT DISTINCT idOrganization " +
                                   " FROM tblOrganization " +
                                   " INNER JOIN tblCnfDealers ON dealerOrgId=idOrganization" +
                                   " INNER JOIN " +
                                   " ( " +
                                   " SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                   " INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                   " ) addrDtl " +
                                   " ON idOrganization = organizationId " +
                                   " INNER JOIN tblUserAreaAllocation areaConf ON " +
                                   " ( addrDtl.districtId=areaConf.districtId  AND areaConf.cnfOrgId=tblCnfDealers.cnfOrgId )" +
                                    " Or (areaConf.cnfOrgId=tblCnfDealers.cnfOrgId )" +
                                   "WHERE  tblOrganization.isActive=1 AND tblCnfDealers.isActive=1 AND orgTypeId=" + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId=" + userId + " AND areaConf.isActive=1 ";

                    }
                }


                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization,brandId=0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " " +
                                 "   AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                  "   UNION ALL " +
                                 "    SELECT areaConf.cnfOrgId, idOrganization = 0, areaConf.brandId FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + "   AND areaConf.isActive = 1 " +

                                 " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid AND " +
                                 " ( bookings.dealerOrgId = userAreaDealer.idOrganization Or  bookings.brandId = userAreaDealer.brandId )";
                }

                conn.Open();
                distStateJoin = " LEFT JOIN tblOrganization orgCnf on orgCnf.idOrganization = bookings.cnFOrgId " +
                            " LEFT JOIN tblOrganization orgDealer on orgDealer.idOrganization = bookings.dealerOrgId " +
                            " LEFT JOIN tblAddress tblAddress on tblAddress.idAddr = orgDealer.addrId " +
                            " LEFT join dimDistrict dist on dist.idDistrict = tblAddress.districtId " +
                            " LEFT join dimState statelist  on statelist.idState = tblAddress.stateId " +
                            " where CAST(bookings.createdOn AS DATE) BETWEEN @fromDate AND @toDate AND bookings.statusId NOT IN (" + ids + ") ";

                String commonSelect = " bookings.bookingQty,bookings.createdOn as timeView from tblbookings bookings " + areConfJoin + distStateJoin;

                if (typeId == (int)Constants.SelectTypeE.DISTRICT)
                {
                    selectSqlQuery = " select dist.districtName as displayName, " + commonSelect + " AND bookings.dealerOrgId IN (" + sqlQuery + ")  AND tblAddress.districtId IN (" + masterId + ")";
                }
                else if (typeId == (int)Constants.SelectTypeE.STATE)
                {
                    selectSqlQuery = " select statelist.stateName as displayName, " + commonSelect + " AND bookings.dealerOrgId IN (" + sqlQuery + ")  AND tblAddress.stateId IN(" + masterId + ")";
                }
                else if (typeId == (int)Constants.SelectTypeE.CNF)
                {
                    selectSqlQuery = " select orgCnf.firmName as displayName, " + commonSelect + " AND bookings.cnFOrgId IN (" + masterId + ")";
                }
                else if (typeId == (int)Constants.SelectTypeE.DEALER)
                {
                    selectSqlQuery = " select orgDealer.firmName as displayName, " + commonSelect + " AND bookings.dealerOrgId IN (" + masterId + ")";

                }

                cmdSelect.CommandText = selectSqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingSummaryTO> list = ConvertDTToListForBookingSummaryRpt(sqlReader);
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
        
        /// <summary>
        ///  Priyanka [02-03-2018]
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="statusId"></param>
        /// <param name="activeUserId"></param>
        /// <returns></returns>

        public List<TblBookingsTO> SelectUserwiseBookingList(DateTime fromDate, DateTime toDate, Int32 statusId,Int32 activeUserId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String areConfJoin = String.Empty;
            String userStatusJoin = String.Empty;
            String notDelStatus = (int)Constants.TranStatusE.BOOKING_DELETE + "";
            int isConfEn = 0;
            
            try
            {
                conn.Open();

                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization ,brandId=0 FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + "  AND areaConf.isActive = 1 " +

                                   "   UNION ALL " +
                                 "    SELECT areaConf.cnfOrgId, idOrganization = 0, areaConf.brandId FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + activeUserId + " " + " " +
                                 "    AND areaConf.isActive = 1 " +

                                 " ) AS (userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid )" +
                                 "  AND ( bookings.dealerOrgId = userAreaDealer.idOrganization Or userAreaDealer.brandId = bookings.brandId )";
                                
                }


                userStatusJoin = " WHERE bookings.idBooking IN (SELECT DISTINCT bookingId from tblBookingBeyondQuota tblBookingBeyondQuota " +
                                  " WHERE(CAST(tblBookingBeyondQuota.createdOn AS DATE) BETWEEN @fromDate AND @toDate)";

                sqlQuery = SqlSelectQuery() + areConfJoin + userStatusJoin;


                if (statusId > 0)
                    sqlQuery += " AND bookings.statusId IN(" + statusId + ")";
                // Commented by Aniket [19-4-2019] while applying Status filter of New 
                // Accepted by Admin bookings also visible
                // sqlQuery += " AND tblBookingBeyondQuota.statusId IN(" + statusId + ")";

                if (activeUserId > 0)
                    sqlQuery += " AND tblBookingBeyondQuota.createdBy IN(" + activeUserId + ")";

                sqlQuery += " )";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = fromDate;//.ToString(Constants.AzureDateFormat);
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDate;//.ToString(Constants.AzureDateFormat);
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(sqlReader);
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

        public TblBookingsTO SelectTblBookings(Int32 idBooking)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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

        public TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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

        public TblBookingsTO SelectTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idBooking = " + idBooking + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingsTO> list = ConvertDTToList(reader);
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
                reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<BookingInfo> SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, int orgId,Int32 dealerId, DateTime date,string ids,Int32 isHideCorNC)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            string whereCond = string.Empty;
            string areConfJoin = string.Empty;
            SqlDataReader tblBookingsTODT = null;
            int isConfEn = 0;
            int userId = 0;
            string statusIds = string.Empty;
            string isConfirm = string.Empty;

            ResultMessage resultMessage = new ResultMessage(); ;

          
            if(!String.IsNullOrEmpty(ids))
            {
                statusIds = " AND statusId IN ("+ ids +") ";
            }

            if (isHideCorNC == 1)
            {
                isConfirm = " AND isConfirmed =" + isHideCorNC;
            }
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

            try
            {
                conn.Open();

                //DimRoleTypeTO roleTypeTO = BL.DimRoleTypeBL.SelectDimRoleTypeTO((int)Constants.SystemRoleTypeE.C_AND_F_AGENT);
                //if (roleTypeTO != null)
                //{
                //    string temp = roleTypeTO.RoleId;
                //    List<string> list = temp.Split(',').ToList();

                //if (list.Contains(tblUserRoleTO.RoleId.ToString()))
                //if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
              
                if (orgId>0)
                {
                    whereCond = " AND cnfOrgId=" + orgId;
                }
                if( dealerId >0)
                {
                    whereCond += " AND dealerOrgId=" + dealerId;
                }
               // }

                //if (tblUserRoleTO.RoleId == (int)Constants.SystemRolesE.C_AND_F_AGENT)
                //{
                //    whereCond = " AND cnFOrgId=" + orgId;
                //}

                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization,brandId=0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +
                                
                                 "    UNION ALL " +
                                 "    SELECT areaConf.cnfOrgId, idOrganization = 0, areaConf.brandId FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + " " +
                                 "    AND areaConf.isActive = 1 " +

                                 " ) AS userAreaDealer On ( userAreaDealer.cnfOrgId = tblBookings.cnFOrgid )" +
                                 "  AND ( tblBookings.dealerOrgId = userAreaDealer.idOrganization Or userAreaDealer.brandId = tblBookings.brandId )";
                }


                //cmdSelect.CommandText = " SELECT SUM(bookingQty) bookingQty, sum(COST) totalCost ,sum(COST)/SUM(bookingQty) avgPrice " +
                //                        " FROM " +
                //                        " ( " +
                //                        " SELECT bookingQty, bookingRate, (bookingQty * bookingRate) AS cost FROM tblBookings " + areConfJoin +
                //                        " WHERE statusId IN(2,3,9,11) AND DAY(bookingDatetime) = " + date.Day + " AND MONTH(bookingDatetime) = " + date.Month + " AND YEAR(bookingDatetime) = " + date.Year + whereCond +
                //                        //" AND globalRateId = (SELECT TOP 1 idGlobalRate FROM tblGlobalRate ORDER BY createdOn DESC )" +  //Saket [2018-02-08] Commented and added new condition for rate.
                //                        " AND globalRateId IN ( SELECT idGlobalRate FROM tblGlobalRate WHERE createdOn = (SELECT top 1 createdOn FROM tblGlobalRate ORDER BY createdOn DESC) )" +
                //                        " ) AS qryRes";



                cmdSelect.CommandText = " SELECT bookingType,bookingQty, COST as totalCost ,COST / bookingQty avgPrice, isConfirmed,brandName,shortNm " +
                                        " FROM " +
                                        " ( " +
                                        " SELECT sum(bookingQty) AS bookingQty, dimBrand.brandName, isConfirmed, sum((bookingQty * bookingRate))" +
                                        " AS cost,bookingType,dimBrand.shortNm FROM tblBookings " + areConfJoin +
                                        " LEFT JOIN dimBrand On tblBookings.brandId = dimBrand.idBrand " + 
                                        " WHERE DAY(bookingDatetime) = " + date.Day + " AND MONTH(bookingDatetime) = " + date.Month +
                                        " AND YEAR(bookingDatetime) = " + date.Year + statusIds + whereCond + isConfirm +
                                        " GROUP BY isConfirmed, brandName,bookingType,dimBrand.shortNm)AS qryRes  order by bookingType,brandName asc  ";


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                tblBookingsTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<ODLMWebAPI.DashboardModels.BookingInfo> tblBookingsTOList = new List<ODLMWebAPI.DashboardModels.BookingInfo>();
                 if (tblBookingsTODT != null)
                {
                    while (tblBookingsTODT.Read())
                    {
                        ODLMWebAPI.DashboardModels.BookingInfo tblBookingsTONew = new ODLMWebAPI.DashboardModels.BookingInfo();
                        if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                            tblBookingsTONew.BookingQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                        if (tblBookingsTODT["totalCost"] != DBNull.Value)
                            tblBookingsTONew.TotalCost = Convert.ToDouble(tblBookingsTODT["totalCost"].ToString());
                        if (tblBookingsTODT["avgPrice"] != DBNull.Value)
                            tblBookingsTONew.AvgPrice = Convert.ToDouble(tblBookingsTODT["avgPrice"].ToString());
                        if (tblBookingsTODT["isConfirmed"] != DBNull.Value)
                            tblBookingsTONew.IsConfirmed = Convert.ToInt32(tblBookingsTODT["isConfirmed"].ToString());
                        if (tblBookingsTODT["brandName"] != DBNull.Value)
                            tblBookingsTONew.BrandName = Convert.ToString(tblBookingsTODT["brandName"].ToString());
                        if (tblBookingsTODT["bookingType"] != DBNull.Value)
                            tblBookingsTONew.BookingType = Convert.ToInt32(tblBookingsTODT["bookingType"].ToString());
                        if (tblBookingsTODT["shortNm"] != DBNull.Value)
                            tblBookingsTONew.ShortNm = Convert.ToString(tblBookingsTODT["shortNm"].ToString());
                        if (tblBookingsTONew.BookingType==(int)Constants.BookingType.IsOther)
                        {
                            tblBookingsTONew.BrandName = "Others";
                        }
                        if (tblBookingsTONew.BookingType == (int)Constants.BookingType.IsOther)
                        {
                            tblBookingsTONew.ShortNm = "Others";
                        }
                        tblBookingsTOList.Add(tblBookingsTONew);
                        
                    }
                }
                return tblBookingsTOList;
              
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (tblBookingsTODT != null)
                    tblBookingsTODT.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        //Aniket [16-Jan-2019] added to view cnFList against confirm and not confirmbooking
        public List<CnFWiseReportTO> ConvertDTCNCcnFBookingReport(SqlDataReader CnFWiseReportTOList)
        {
            List<CnFWiseReportTO> cnFWiseReportTOList = new List<CnFWiseReportTO>();
            if(CnFWiseReportTOList!=null)
            {
                while(CnFWiseReportTOList.Read())
                {
                    CnFWiseReportTO cnfWiseReportTONew = new CnFWiseReportTO();
                    if (CnFWiseReportTOList["firmName"] != DBNull.Value)
                        cnfWiseReportTONew.CnfName = CnFWiseReportTOList["firmName"].ToString();
                    if (CnFWiseReportTOList["confirmed"] != DBNull.Value)
                        cnfWiseReportTONew.ConfirmBooking =Convert.ToInt32( CnFWiseReportTOList["confirmed"]);
                    if (CnFWiseReportTOList["notConfirmed"] != DBNull.Value)
                        cnfWiseReportTONew.NotConfirmBooking = Convert.ToInt32(CnFWiseReportTOList["notConfirmed"]);
                    cnFWiseReportTOList.Add(cnfWiseReportTONew);

                }
            }
            return cnFWiseReportTOList;
        }
        //Aniket [29-8-2019] added for Last 4 bookings enquires convertDTList
        public List<TblBookingsTO> ConvertDTToListForBookingHistory(SqlDataReader tblBookingsTODT)
        {
            List<TblBookingsTO> tblBookingsTOList = new List<TblBookingsTO>();
            if (tblBookingsTODT != null)
            {
                while (tblBookingsTODT.Read())
                {
                    TblBookingsTO tblBookingsTONew = new TblBookingsTO();
                    if (tblBookingsTODT["idBooking"] != DBNull.Value)
                        tblBookingsTONew.IdBooking = Convert.ToInt32(tblBookingsTODT["idBooking"].ToString());
                    if (tblBookingsTODT["cnFOrgId"] != DBNull.Value)
                        tblBookingsTONew.CnFOrgId = Convert.ToInt32(tblBookingsTODT["cnFOrgId"].ToString());
                    if (tblBookingsTODT["dealerOrgId"] != DBNull.Value)
                        tblBookingsTONew.DealerOrgId = Convert.ToInt32(tblBookingsTODT["dealerOrgId"].ToString());
                    if (tblBookingsTODT["deliveryDays"] != DBNull.Value)
                        tblBookingsTONew.DeliveryDays = Convert.ToInt32(tblBookingsTODT["deliveryDays"].ToString());
                    if (tblBookingsTODT["noOfDeliveries"] != DBNull.Value)
                        tblBookingsTONew.NoOfDeliveries = Convert.ToInt32(tblBookingsTODT["noOfDeliveries"].ToString());
                    if (tblBookingsTODT["isConfirmed"] != DBNull.Value)
                        tblBookingsTONew.IsConfirmed = Convert.ToInt32(tblBookingsTODT["isConfirmed"].ToString());
                    if (tblBookingsTODT["isJointDelivery"] != DBNull.Value)
                        tblBookingsTONew.IsJointDelivery = Convert.ToInt32(tblBookingsTODT["isJointDelivery"].ToString());
                    if (tblBookingsTODT["isSpecialRequirement"] != DBNull.Value)
                        tblBookingsTONew.IsSpecialRequirement = Convert.ToInt32(tblBookingsTODT["isSpecialRequirement"].ToString());
                    if (tblBookingsTODT["cdStructure"] != DBNull.Value)
                        tblBookingsTONew.CdStructure = Convert.ToDouble(tblBookingsTODT["cdStructure"].ToString());
                    if (tblBookingsTODT["statusId"] != DBNull.Value)
                        tblBookingsTONew.StatusId = Convert.ToInt32(tblBookingsTODT["statusId"].ToString());
                    if (tblBookingsTODT["isWithinQuotaLimit"] != DBNull.Value)
                        tblBookingsTONew.IsWithinQuotaLimit = Convert.ToInt32(tblBookingsTODT["isWithinQuotaLimit"].ToString());
                    if (tblBookingsTODT["globalRateId"] != DBNull.Value)
                        tblBookingsTONew.GlobalRateId = Convert.ToInt32(tblBookingsTODT["globalRateId"].ToString());
                    if (tblBookingsTODT["quotaDeclarationId"] != DBNull.Value)
                        tblBookingsTONew.QuotaDeclarationId = Convert.ToInt32(tblBookingsTODT["quotaDeclarationId"].ToString());
                    if (tblBookingsTODT["quotaQtyBforBooking"] != DBNull.Value)
                        tblBookingsTONew.QuotaQtyBforBooking = Convert.ToInt32(tblBookingsTODT["quotaQtyBforBooking"].ToString());
                    if (tblBookingsTODT["quotaQtyAftBooking"] != DBNull.Value)
                        tblBookingsTONew.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTODT["quotaQtyAftBooking"].ToString());
                    if (tblBookingsTODT["createdBy"] != DBNull.Value)
                        tblBookingsTONew.CreatedBy = Convert.ToInt32(tblBookingsTODT["createdBy"].ToString());
                    if (tblBookingsTODT["createdOn"] != DBNull.Value)
                        tblBookingsTONew.CreatedOn = Convert.ToDateTime(tblBookingsTODT["createdOn"].ToString());
                    if (tblBookingsTODT["updatedBy"] != DBNull.Value)
                        tblBookingsTONew.UpdatedBy = Convert.ToInt32(tblBookingsTODT["updatedBy"].ToString());
                    if (tblBookingsTODT["bookingDatetime"] != DBNull.Value)
                        tblBookingsTONew.BookingDatetime = Convert.ToDateTime(tblBookingsTODT["bookingDatetime"].ToString());
                    if (tblBookingsTODT["statusDate"] != DBNull.Value)
                        tblBookingsTONew.StatusDate = Convert.ToDateTime(tblBookingsTODT["statusDate"].ToString());
                    if (tblBookingsTODT["updatedOn"] != DBNull.Value)
                        tblBookingsTONew.UpdatedOn = Convert.ToDateTime(tblBookingsTODT["updatedOn"].ToString());
                    if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                        tblBookingsTONew.BookingQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                    if (tblBookingsTODT["bookingRate"] != DBNull.Value)
                        tblBookingsTONew.BookingRate = Convert.ToDouble(tblBookingsTODT["bookingRate"].ToString());
                    if (tblBookingsTODT["comments"] != DBNull.Value)
                        tblBookingsTONew.Comments = Convert.ToString(tblBookingsTODT["comments"].ToString());

                    if (tblBookingsTODT["cnfName"] != DBNull.Value)
                        tblBookingsTONew.CnfName = Convert.ToString(tblBookingsTODT["cnfName"].ToString());
                    if (tblBookingsTODT["dealerName"] != DBNull.Value)
                        tblBookingsTONew.DealerName = Convert.ToString(tblBookingsTODT["dealerName"].ToString());

                    if (tblBookingsTODT["statusName"] != DBNull.Value)
                        tblBookingsTONew.Status = Convert.ToString(tblBookingsTODT["statusName"].ToString());

                    if (tblBookingsTODT["pendingQty"] != DBNull.Value)
                        tblBookingsTONew.PendingQty = Convert.ToDouble(tblBookingsTODT["pendingQty"].ToString());

                    if (tblBookingsTODT["authReasons"] != DBNull.Value)
                        tblBookingsTONew.AuthReasons = Convert.ToString(tblBookingsTODT["authReasons"].ToString());
                    if (tblBookingsTODT["cdStructureId"] != DBNull.Value)
                        tblBookingsTONew.CdStructureId = Convert.ToInt32(tblBookingsTODT["cdStructureId"].ToString());

                    if (tblBookingsTODT["parityId"] != DBNull.Value)
                        tblBookingsTONew.ParityId = Convert.ToInt32(tblBookingsTODT["parityId"].ToString());
                    //CommonDAO.SetDateStandards(tblBookingsTONew);

                    if (tblBookingsTODT["orcAmt"] != DBNull.Value)
                        tblBookingsTONew.OrcAmt = Convert.ToDouble(tblBookingsTODT["orcAmt"].ToString());
                    if (tblBookingsTODT["orcMeasure"] != DBNull.Value)
                        tblBookingsTONew.OrcMeasure = Convert.ToString(tblBookingsTODT["orcMeasure"].ToString());
                    if (tblBookingsTODT["billingName"] != DBNull.Value)
                        tblBookingsTONew.BillingName = Convert.ToString(tblBookingsTODT["billingName"].ToString());

                    //Sanjay [2017-06-06]
                    if (tblBookingsTODT["poNo"] != DBNull.Value)
                        tblBookingsTONew.PoNo = Convert.ToString(tblBookingsTODT["poNo"].ToString());

                    //Saket [2017-11-10] Added.
                    if (tblBookingsTODT["transporterScopeYn"] != DBNull.Value)
                        tblBookingsTONew.TransporterScopeYn = Convert.ToInt32(tblBookingsTODT["transporterScopeYn"].ToString());

                    if (tblBookingsTODT["brandId"] != DBNull.Value)
                        tblBookingsTONew.BrandId = Convert.ToInt32(tblBookingsTODT["brandId"].ToString());
                    if (tblBookingsTODT["brandName"] != DBNull.Value)
                        tblBookingsTONew.BrandName = Convert.ToString(tblBookingsTODT["brandName"].ToString());
                    if (tblBookingsTODT["vehicleNo"] != DBNull.Value)
                        tblBookingsTONew.VehicleNo = Convert.ToString(tblBookingsTODT["vehicleNo"].ToString());
                    if (tblBookingsTODT["freightAmt"] != DBNull.Value)
                        tblBookingsTONew.FreightAmt = Convert.ToDouble(tblBookingsTODT["freightAmt"].ToString());
                    if (tblBookingsTODT["poFileBase64"] != DBNull.Value)
                        tblBookingsTONew.PoFileBase64 = Convert.ToString(tblBookingsTODT["poFileBase64"].ToString());

                    if (tblBookingsTODT["projectName"] != DBNull.Value)
                        tblBookingsTONew.ProjectName = Convert.ToString(tblBookingsTODT["projectName"].ToString());

                    //Vijaymla[26-02-2018]added
                    if (tblBookingsTODT["poDate"] != DBNull.Value)
                        tblBookingsTONew.PoDate = Convert.ToDateTime(tblBookingsTODT["poDate"].ToString());

                    if (tblBookingsTODT["orcPersonName"] != DBNull.Value)
                        tblBookingsTONew.ORCPersonName = Convert.ToString(tblBookingsTODT["orcPersonName"]);

                    //Priyanka [18-06-2018] Added
                    if (tblBookingsTODT["createdByName"] != DBNull.Value)
                        tblBookingsTONew.CreatedByName = Convert.ToString(tblBookingsTODT["createdByName"]);

                    if (tblBookingsTODT["updatedByName"] != DBNull.Value)
                        tblBookingsTONew.UpdatedByName = Convert.ToString(tblBookingsTODT["updatedByName"]);

                    //Priyakna [08-06-2018] : Added for SHIVANGI.
                    if (tblBookingsTODT["isOverdueExist"] != DBNull.Value)
                        tblBookingsTONew.IsOverdueExist = Convert.ToInt32(tblBookingsTODT["isOverdueExist"].ToString());

                    //Priyanka [21-06-2018] : Added for SHIVANGI.
                    if (tblBookingsTODT["sizesQty"] != DBNull.Value)
                        tblBookingsTONew.SizesQty = Convert.ToDouble(tblBookingsTODT["sizesQty"].ToString());

                    //Priyanka [25-06-2018] : Added for Director Remark while adding booking
                    if (tblBookingsTODT["directorRemark"] != DBNull.Value)
                        tblBookingsTONew.DirectorRemark = Convert.ToString(tblBookingsTODT["directorRemark"].ToString());

                    if (tblBookingsTODT["isOrgOverDue"] != DBNull.Value)
                        tblBookingsTONew.IsOrgOverDue = Convert.ToInt32(tblBookingsTODT["isOrgOverDue"].ToString());

                    if (tblBookingsTODT["statusBy"] != DBNull.Value)
                        tblBookingsTONew.StatusBy = Convert.ToInt32(tblBookingsTODT["statusBy"].ToString());

                    //if (tblBookingsTODT["tranActionTypeId"] != DBNull.Value)
                    //    tblBookingsTONew.TranActionTypeId = Convert.ToInt32(tblBookingsTODT["tranActionTypeId"].ToString());

                    //[05-09-2018]Vijaymala added for booking type like other or regular
                    if (tblBookingsTODT["bookingType"] != DBNull.Value)
                        tblBookingsTONew.BookingType = Convert.ToInt32(tblBookingsTODT["bookingType"].ToString());

                    if (tblBookingsTONew.BookingType == (int)Constants.BookingType.IsOther)
                    {
                        tblBookingsTONew.BrandName = "Others";
                    }
                    if (tblBookingsTODT["isSez"] != DBNull.Value)
                        tblBookingsTONew.IsSez = Convert.ToInt32(tblBookingsTODT["isSez"].ToString());

                    //Aniket [13-6-2019]
                    if (tblBookingsTODT["uomQty"] != DBNull.Value)
                        tblBookingsTONew.UomQty = Convert.ToDouble(tblBookingsTODT["uomQty"]);
                    if (tblBookingsTODT["pendingUomQty"] != DBNull.Value)
                        tblBookingsTONew.PendingUomQty = Convert.ToDouble(tblBookingsTODT["pendingUomQty"]);
                    if (tblBookingsTODT["isInUom"] != DBNull.Value)
                        tblBookingsTONew.IsInUom = Convert.ToInt32(tblBookingsTODT["isInUom"]);
                    if (tblBookingsTODT["isItemized"] != DBNull.Value)
                        tblBookingsTONew.IsItemized = Convert.ToInt32(tblBookingsTODT["isItemized"]);
                   
                    tblBookingsTOList.Add(tblBookingsTONew);
                }
            }
            return tblBookingsTOList;
        }
        public List<TblBookingsTO> ConvertDTToList(SqlDataReader tblBookingsTODT)
        {
            List<TblBookingsTO> tblBookingsTOList = new List<TblBookingsTO>();
            if (tblBookingsTODT != null)
            {
                while (tblBookingsTODT.Read())
                {
                    TblBookingsTO tblBookingsTONew = new TblBookingsTO();
                    if (tblBookingsTODT["idBooking"] != DBNull.Value)
                        tblBookingsTONew.IdBooking = Convert.ToInt32(tblBookingsTODT["idBooking"].ToString());
                    if (tblBookingsTODT["cnFOrgId"] != DBNull.Value)
                        tblBookingsTONew.CnFOrgId = Convert.ToInt32(tblBookingsTODT["cnFOrgId"].ToString());
                    if (tblBookingsTODT["dealerOrgId"] != DBNull.Value)
                        tblBookingsTONew.DealerOrgId = Convert.ToInt32(tblBookingsTODT["dealerOrgId"].ToString());
                    if (tblBookingsTODT["deliveryDays"] != DBNull.Value)
                        tblBookingsTONew.DeliveryDays = Convert.ToInt32(tblBookingsTODT["deliveryDays"].ToString());
                    if (tblBookingsTODT["noOfDeliveries"] != DBNull.Value)
                        tblBookingsTONew.NoOfDeliveries = Convert.ToInt32(tblBookingsTODT["noOfDeliveries"].ToString());
                    if (tblBookingsTODT["isConfirmed"] != DBNull.Value)
                        tblBookingsTONew.IsConfirmed = Convert.ToInt32(tblBookingsTODT["isConfirmed"].ToString());
                    if (tblBookingsTODT["isJointDelivery"] != DBNull.Value)
                        tblBookingsTONew.IsJointDelivery = Convert.ToInt32(tblBookingsTODT["isJointDelivery"].ToString());
                    if (tblBookingsTODT["isSpecialRequirement"] != DBNull.Value)
                        tblBookingsTONew.IsSpecialRequirement = Convert.ToInt32(tblBookingsTODT["isSpecialRequirement"].ToString());
                    if (tblBookingsTODT["cdStructure"] != DBNull.Value)
                        tblBookingsTONew.CdStructure = Convert.ToDouble(tblBookingsTODT["cdStructure"].ToString());
                    if (tblBookingsTODT["statusId"] != DBNull.Value)
                        tblBookingsTONew.StatusId = Convert.ToInt32(tblBookingsTODT["statusId"].ToString());
                    if (tblBookingsTODT["isWithinQuotaLimit"] != DBNull.Value)
                        tblBookingsTONew.IsWithinQuotaLimit = Convert.ToInt32(tblBookingsTODT["isWithinQuotaLimit"].ToString());
                    if (tblBookingsTODT["globalRateId"] != DBNull.Value)
                        tblBookingsTONew.GlobalRateId = Convert.ToInt32(tblBookingsTODT["globalRateId"].ToString());
                    if (tblBookingsTODT["quotaDeclarationId"] != DBNull.Value)
                        tblBookingsTONew.QuotaDeclarationId = Convert.ToInt32(tblBookingsTODT["quotaDeclarationId"].ToString());
                    if (tblBookingsTODT["quotaQtyBforBooking"] != DBNull.Value)
                        tblBookingsTONew.QuotaQtyBforBooking = Convert.ToInt32(tblBookingsTODT["quotaQtyBforBooking"].ToString());
                    if (tblBookingsTODT["quotaQtyAftBooking"] != DBNull.Value)
                        tblBookingsTONew.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTODT["quotaQtyAftBooking"].ToString());
                    if (tblBookingsTODT["createdBy"] != DBNull.Value)
                        tblBookingsTONew.CreatedBy = Convert.ToInt32(tblBookingsTODT["createdBy"].ToString());
                    if (tblBookingsTODT["createdOn"] != DBNull.Value)
                        tblBookingsTONew.CreatedOn = Convert.ToDateTime(tblBookingsTODT["createdOn"].ToString());
                    if (tblBookingsTODT["updatedBy"] != DBNull.Value)
                        tblBookingsTONew.UpdatedBy = Convert.ToInt32(tblBookingsTODT["updatedBy"].ToString());
                    if (tblBookingsTODT["bookingDatetime"] != DBNull.Value)
                        tblBookingsTONew.BookingDatetime = Convert.ToDateTime(tblBookingsTODT["bookingDatetime"].ToString());
                    if (tblBookingsTODT["statusDate"] != DBNull.Value)
                        tblBookingsTONew.StatusDate = Convert.ToDateTime(tblBookingsTODT["statusDate"].ToString());
                    if (tblBookingsTODT["updatedOn"] != DBNull.Value)
                        tblBookingsTONew.UpdatedOn = Convert.ToDateTime(tblBookingsTODT["updatedOn"].ToString());
                    if (tblBookingsTODT["bookingQty"] != DBNull.Value)
                        tblBookingsTONew.BookingQty = Convert.ToDouble(tblBookingsTODT["bookingQty"].ToString());
                    if (tblBookingsTODT["bookingRate"] != DBNull.Value)
                        tblBookingsTONew.BookingRate = Convert.ToDouble(tblBookingsTODT["bookingRate"].ToString());
                    if (tblBookingsTODT["comments"] != DBNull.Value)
                        tblBookingsTONew.Comments = Convert.ToString(tblBookingsTODT["comments"].ToString());

                    if (tblBookingsTODT["cnfName"] != DBNull.Value)
                        tblBookingsTONew.CnfName = Convert.ToString(tblBookingsTODT["cnfName"].ToString());
                    if (tblBookingsTODT["dealerName"] != DBNull.Value)
                        tblBookingsTONew.DealerName = Convert.ToString(tblBookingsTODT["dealerName"].ToString());

                    if (tblBookingsTODT["statusName"] != DBNull.Value)
                        tblBookingsTONew.Status = Convert.ToString(tblBookingsTODT["statusName"].ToString());

                    if (tblBookingsTODT["pendingQty"] != DBNull.Value)
                        tblBookingsTONew.PendingQty = Convert.ToDouble(tblBookingsTODT["pendingQty"].ToString());

                    if (tblBookingsTODT["authReasons"] != DBNull.Value)
                        tblBookingsTONew.AuthReasons = Convert.ToString(tblBookingsTODT["authReasons"].ToString());
                    if (tblBookingsTODT["cdStructureId"] != DBNull.Value)
                        tblBookingsTONew.CdStructureId = Convert.ToInt32(tblBookingsTODT["cdStructureId"].ToString());

                    if (tblBookingsTODT["parityId"] != DBNull.Value)
                        tblBookingsTONew.ParityId = Convert.ToInt32(tblBookingsTODT["parityId"].ToString());
                    //CommonDAO.SetDateStandards(tblBookingsTONew);

                    if (tblBookingsTODT["orcAmt"] != DBNull.Value)
                        tblBookingsTONew.OrcAmt = Convert.ToDouble(tblBookingsTODT["orcAmt"].ToString());
                    if (tblBookingsTODT["orcMeasure"] != DBNull.Value)
                        tblBookingsTONew.OrcMeasure = Convert.ToString(tblBookingsTODT["orcMeasure"].ToString());
                    if (tblBookingsTODT["billingName"] != DBNull.Value)
                        tblBookingsTONew.BillingName = Convert.ToString(tblBookingsTODT["billingName"].ToString());

                    //Sanjay [2017-06-06]
                    if (tblBookingsTODT["poNo"] != DBNull.Value)
                        tblBookingsTONew.PoNo = Convert.ToString(tblBookingsTODT["poNo"].ToString());

                    //Saket [2017-11-10] Added.
                    if (tblBookingsTODT["transporterScopeYn"] != DBNull.Value)
                        tblBookingsTONew.TransporterScopeYn = Convert.ToInt32(tblBookingsTODT["transporterScopeYn"].ToString());

                    if (tblBookingsTODT["brandId"] != DBNull.Value)
                        tblBookingsTONew.BrandId = Convert.ToInt32(tblBookingsTODT["brandId"].ToString());
                    if (tblBookingsTODT["brandName"] != DBNull.Value)
                        tblBookingsTONew.BrandName = Convert.ToString(tblBookingsTODT["brandName"].ToString());
                    if (tblBookingsTODT["vehicleNo"] != DBNull.Value)
                        tblBookingsTONew.VehicleNo = Convert.ToString(tblBookingsTODT["vehicleNo"].ToString());
                    if (tblBookingsTODT["freightAmt"] != DBNull.Value)
                        tblBookingsTONew.FreightAmt = Convert.ToDouble(tblBookingsTODT["freightAmt"].ToString());
                    if (tblBookingsTODT["poFileBase64"] != DBNull.Value)
                        tblBookingsTONew.PoFileBase64 = Convert.ToString(tblBookingsTODT["poFileBase64"].ToString());

                    if (tblBookingsTODT["projectName"] != DBNull.Value)
                        tblBookingsTONew.ProjectName = Convert.ToString(tblBookingsTODT["projectName"].ToString());

                    //Vijaymla[26-02-2018]added
                    if (tblBookingsTODT["poDate"] != DBNull.Value)
                        tblBookingsTONew.PoDate = Convert.ToDateTime(tblBookingsTODT["poDate"].ToString());

                    if (tblBookingsTODT["orcPersonName"] != DBNull.Value)
                        tblBookingsTONew.ORCPersonName = Convert.ToString(tblBookingsTODT["orcPersonName"]);

                    //Priyanka [18-06-2018] Added
                    if (tblBookingsTODT["createdByName"] != DBNull.Value)
                        tblBookingsTONew.CreatedByName = Convert.ToString(tblBookingsTODT["createdByName"]);

                    if (tblBookingsTODT["updatedByName"] != DBNull.Value)
                        tblBookingsTONew.UpdatedByName = Convert.ToString(tblBookingsTODT["updatedByName"]);

                    //Priyakna [08-06-2018] : Added for SHIVANGI.
                    if (tblBookingsTODT["isOverdueExist"] != DBNull.Value)
                        tblBookingsTONew.IsOverdueExist = Convert.ToInt32(tblBookingsTODT["isOverdueExist"].ToString());

                    //Priyanka [21-06-2018] : Added for SHIVANGI.
                    if (tblBookingsTODT["sizesQty"] != DBNull.Value)
                        tblBookingsTONew.SizesQty = Convert.ToDouble(tblBookingsTODT["sizesQty"].ToString());

                    //Priyanka [25-06-2018] : Added for Director Remark while adding booking
                    if (tblBookingsTODT["directorRemark"] != DBNull.Value)
                        tblBookingsTONew.DirectorRemark = Convert.ToString(tblBookingsTODT["directorRemark"].ToString());

                    if (tblBookingsTODT["isOrgOverDue"] != DBNull.Value)
                        tblBookingsTONew.IsOrgOverDue = Convert.ToInt32(tblBookingsTODT["isOrgOverDue"].ToString());

                    if (tblBookingsTODT["statusBy"] != DBNull.Value)
                        tblBookingsTONew.StatusBy = Convert.ToInt32(tblBookingsTODT["statusBy"].ToString());

                    if (tblBookingsTODT["tranActionTypeId"] != DBNull.Value)
                        tblBookingsTONew.TranActionTypeId = Convert.ToInt32(tblBookingsTODT["tranActionTypeId"].ToString());
                    
                    //[05-09-2018]Vijaymala added for booking type like other or regular
                    if (tblBookingsTODT["bookingType"] != DBNull.Value)
                        tblBookingsTONew.BookingType = Convert.ToInt32(tblBookingsTODT["bookingType"].ToString());

                    if(tblBookingsTONew.BookingType==(int)Constants.BookingType.IsOther)
                    {
                        tblBookingsTONew.BrandName = "Others";
                    }
                    if (tblBookingsTODT["isSez"] != DBNull.Value)
                        tblBookingsTONew.IsSez = Convert.ToInt32(tblBookingsTODT["isSez"].ToString());

                    //Aniket [13-6-2019]
                    if (tblBookingsTODT["uomQty"] != DBNull.Value)
                        tblBookingsTONew.UomQty = Convert.ToDouble(tblBookingsTODT["uomQty"]);
                    if (tblBookingsTODT["pendingUomQty"] != DBNull.Value)
                        tblBookingsTONew.PendingUomQty = Convert.ToDouble(tblBookingsTODT["pendingUomQty"]);
                    if (tblBookingsTODT["isInUom"] != DBNull.Value)
                        tblBookingsTONew.IsInUom = Convert.ToInt32(tblBookingsTODT["isInUom"]);
                    if (tblBookingsTODT["isItemized"] != DBNull.Value)
                        tblBookingsTONew.IsItemized = Convert.ToInt32(tblBookingsTODT["isItemized"]);
                    if (tblBookingsTODT["stateId"] != DBNull.Value)
                        tblBookingsTONew.StateId = Convert.ToInt32(tblBookingsTODT["stateId"]);
                             if (tblBookingsTODT["dealerCat"] != DBNull.Value)
                        tblBookingsTONew.DealerCat = Convert.ToString(tblBookingsTODT["dealerCat"]);
                             if (tblBookingsTODT["colorCode"] != DBNull.Value)
                        tblBookingsTONew.ColorCode = Convert.ToString(tblBookingsTODT["colorCode"]);
                    tblBookingsTOList.Add(tblBookingsTONew);
                }
            }
            return tblBookingsTOList;
        }

        /// <summary>
        /// Priyanka [15-03-2018]: Added ConvertDTTOList For Booking Summary Report
        /// </summary>
        /// <param name="tblBookingsSummaryTODT"></param>
        /// <returns></returns>
        public List<TblBookingSummaryTO> ConvertDTToListForBookingSummaryRpt(SqlDataReader tblBookingsSummaryTODT)
        {
            List<TblBookingSummaryTO> tblBookingsSummaryTOList = new List<TblBookingSummaryTO>();
            if (tblBookingsSummaryTODT != null)
            {
                while (tblBookingsSummaryTODT.Read())
                {
                    TblBookingSummaryTO tblBookingsSummaryTONew = new TblBookingSummaryTO();
                   
                    if (tblBookingsSummaryTODT["displayName"] != DBNull.Value)
                        tblBookingsSummaryTONew.DisplayName = Convert.ToString(tblBookingsSummaryTODT["displayName"].ToString());

                    if (tblBookingsSummaryTODT["bookingQty"] != DBNull.Value)
                        tblBookingsSummaryTONew.BookingQty = Convert.ToDouble(tblBookingsSummaryTODT["bookingQty"].ToString());

                    if (tblBookingsSummaryTODT["timeView"] != DBNull.Value)
                        tblBookingsSummaryTONew.TimeView = Convert.ToDateTime(tblBookingsSummaryTODT["timeView"].ToString());

                        tblBookingsSummaryTOList.Add(tblBookingsSummaryTONew);
                }
            }
            return tblBookingsSummaryTOList;
        }
    
        public Dictionary<Int32, Double> SelectBookingsPendingQtyDCT(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            Dictionary<Int32, Double> pendingQtyDCT = new Dictionary<int, double>();

            // String statusIds = (int)Constants.TranStatusE.BOOKING_APPROVED + "," + (int)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
            String statusIds = String.Empty;

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_STATUS_TO_CALCULATE_ENQUIRY_OPENING_BALANCE);
            if (tblConfigParamsTO != null)
            {
                statusIds = tblConfigParamsTO.ConfigParamVal;
            }

            try
            {
                cmdSelect.CommandText = " SELECT idBooking,pendingQty FROM tblBookings " +
                                        " WHERE statusId IN(" + statusIds + " ) AND pendingQty > 0";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        Int32 bookingId = 0;
                        Double pendingQty = 0;
                        if (reader["idBooking"] != DBNull.Value)
                            bookingId = Convert.ToInt32(reader["idBooking"].ToString());
                        if (reader["pendingQty"] != DBNull.Value)
                            pendingQty = Convert.ToDouble(reader["pendingQty"].ToString());

                        if (bookingId > 0 && pendingQty > 0)
                            pendingQtyDCT.Add(bookingId, pendingQty);
                    }
                }

                return pendingQtyDCT;
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

        /// <summary>
        /// Vijaymala [2017-09-11] added to get booking list to generate booking graph
        /// </summary>
        public List<BookingGraphRptTO> SelectBookingListForGraph(Int32 OrganizationId, TblUserRoleTO tblUserRoleTO,Int32 dealerId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String sqlQuery = String.Empty;
            String SelectQuery = String.Empty;
            string statusIds = (Int32)Constants.TranStatusE.BOOKING_APPROVED + "," + (Int32)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
            String areConfJoin = String.Empty;
            string whereCond = string.Empty;
            int isConfEn = 0;
            int userId = 0;
            DateTime sysDate = _iCommon.ServerDateTime;
            if (tblUserRoleTO != null)
            {
                isConfEn = tblUserRoleTO.EnableAreaAlloc;
                userId = tblUserRoleTO.UserId;
            }

           // List<string> roleTypelist = new List<string>();
            //DimRoleTypeTO roleTypeTO = BL.DimRoleTypeBL.SelectDimRoleTypeTO((int)Constants.SystemRoleTypeE.C_AND_F_AGENT);
            //if (roleTypeTO != null)
            //{
            //    string temp = roleTypeTO.RoleId;
            //    roleTypelist = temp.Split(',').ToList();               
            //}

            try
            {
                conn.Open();
                SelectQuery = " Select bookings.cnFOrgId,sum(bookings.bookingQty)bookingQty,org.firmName cnfName,  " +
                "  sum(bookings.bookingQty * bookings.bookingRate) / sum(bookings.bookingQty)avgPrice from tblBookings bookings  " +
                "  Inner Join tblOrganization org On bookings.cnFOrgId = org.idOrganization  " +
                "  Inner Join dimStatus ON dimStatus.idStatus = bookings.statusId  ";


                if (isConfEn == 1)
                {
                    //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]

                    areConfJoin = " INNER JOIN " +
                                 " ( " +
                                 "   SELECT areaConf.cnfOrgId, idOrganization,brandId=0  FROM tblOrganization " +
                                 "   INNER JOIN tblCnfDealers ON dealerOrgId = idOrganization " +
                                 "   INNER JOIN " +
                                 "   ( " +
                                 "       SELECT tblAddress.*, organizationId FROM tblOrgAddress " +
                                 "       INNER JOIN tblAddress ON idAddr = addressId WHERE addrTypeId = 1 " +
                                 "  ) addrDtl  ON idOrganization = organizationId " +
                                 "   INNER JOIN tblUserAreaAllocation areaConf ON addrDtl.districtId = areaConf.districtId AND areaConf.cnfOrgId = tblCnfDealers.cnfOrgId " +
                                 "   WHERE  tblOrganization.isActive = 1 AND tblCnfDealers.isActive = 1  AND orgTypeId = " + (int)Constants.OrgTypeE.DEALER + " AND areaConf.userId = " + userId + "  AND areaConf.isActive = 1 " +

                                  "    UNION ALL " +
                                 "    SELECT areaConf.cnfOrgId, idOrganization = 0, areaConf.brandId FROM tblUserAreaAllocation  areaConf where  areaConf.userId = " + userId + " " + " " +
                                 "    AND areaConf.isActive = 1 " +

                                  " ) AS userAreaDealer On userAreaDealer.cnfOrgId = bookings.cnFOrgid " +
                                 "  AND bookings.dealerOrgId = userAreaDealer.idOrganization Or userAreaDealer.brandId = bookings.brandId ";

                }

                //if (tblUserRoleTO.RoleId != (int)Constants.SystemRolesE.C_AND_F_AGENT)
                //if (roleTypelist.Contains(tblUserRoleTO.RoleId.ToString()))

                if (OrganizationId > 0)
                {
                    whereCond = " AND bookings.cnFOrgId=" + OrganizationId;
                }
                if (dealerId > 0)
                {
                    whereCond += " AND bookings.dealerOrgId=" + dealerId;

                }
                sqlQuery = SelectQuery + areConfJoin + " WHERE   bookings.bookingQty > 0 AND bookings.statusId IN(" + statusIds + ") AND DAY(bookings.bookingDatetime) = " + sysDate.Day + " AND MONTH(bookings.bookingDatetime) = " + sysDate.Month + " AND YEAR(bookings.bookingDatetime) = " + sysDate.Year + whereCond +
                    " group by bookings.cnFOrgId,org.firmName";

                //if (OrganizationId > 0)
                //{
                //    sqlQuery = SelectQuery + areConfJoin + " WHERE bookings.cnFOrgId=" + OrganizationId + " AND  bookings.bookingQty > 0 AND bookings.statusId IN(" + statusIds + ") AND DAY(bookings.bookingDatetime) = " + sysDate.Day + " AND MONTH(bookings.bookingDatetime) = " + sysDate.Month + " AND YEAR(bookings.bookingDatetime) = " + sysDate.Year + " group by bookings.cnFOrgId,org.firmName";
                //}
                //else
                //{
                //    sqlQuery = SelectQuery + areConfJoin + "  WHERE  bookings.bookingQty > 0 AND bookings.statusId IN(" + statusIds + ") AND DAY(bookings.bookingDatetime) = " + sysDate.Day + " AND MONTH(bookings.bookingDatetime) = " + sysDate.Month + " AND YEAR(bookings.bookingDatetime) = " + sysDate.Year + " group by bookings.cnFOrgId,org.firmName";
                //}
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<BookingGraphRptTO> list = ConvertDTToListForGraph(sqlReader);
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
        /// <summary>
        /// Vijaymala [2017-09-11] added to convert dt to list to generate booking graph
        /// </summary>
        public List<BookingGraphRptTO> ConvertDTToListForGraph(SqlDataReader tblBookingsGraphRptTODT)
        {
            List<BookingGraphRptTO> bookingGraphRptTOList = new List<BookingGraphRptTO>();
            if (tblBookingsGraphRptTODT != null)
            {
                while (tblBookingsGraphRptTODT.Read())
                {
                    BookingGraphRptTO tblBookingsGraphRptTONew = new BookingGraphRptTO();

                    //if (tblBookingsGraphRptTODT["bookingId"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.BookingId = Convert.ToInt32(tblBookingsGraphRptTODT["bookingId"].ToString());
                    if (tblBookingsGraphRptTODT["cnFOrgId"] != DBNull.Value)
                        tblBookingsGraphRptTONew.CnFOrgId = Convert.ToInt32(tblBookingsGraphRptTODT["cnFOrgId"].ToString());
                    //if (tblBookingsGraphRptTODT["dealerOrgId"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.DealerOrgId = Convert.ToInt32(tblBookingsGraphRptTODT["dealerOrgId"].ToString());
                    if (tblBookingsGraphRptTODT["cnfName"] != DBNull.Value)
                        tblBookingsGraphRptTONew.CnfName = Convert.ToString(tblBookingsGraphRptTODT["cnfName"].ToString());
                    //if (tblBookingsGraphRptTODT["dealerName"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.DealerName = Convert.ToString(tblBookingsGraphRptTODT["dealerName"].ToString());
                    if (tblBookingsGraphRptTODT["bookingQty"] != DBNull.Value)
                        tblBookingsGraphRptTONew.BookingQty = Convert.ToDouble(tblBookingsGraphRptTODT["bookingQty"].ToString());
                    //if (tblBookingsGraphRptTODT["bookingRate"] != DBNull.Value)
                    //    tblBookingsGraphRptTONew.BookingRate = Convert.ToDouble(tblBookingsGraphRptTODT["bookingRate"].ToString());
                    if (tblBookingsGraphRptTODT["avgPrice"] != DBNull.Value)
                        tblBookingsGraphRptTONew.AvgPrice = Convert.ToDouble(tblBookingsGraphRptTODT["avgPrice"].ToString());
                    bookingGraphRptTOList.Add(tblBookingsGraphRptTONew);
                }
            }
            return bookingGraphRptTOList;
        }

       
        
        #endregion

        #region Insertion
        public int InsertTblBookings(TblBookingsTO tblBookingsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblBookingsTO, cmdInsert);
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

        public int InsertTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingsTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblBookingsTO tblBookingsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookings]( " +
                            "  [cnFOrgId]" +
                            " ,[dealerOrgId]" +
                            " ,[deliveryDays]" +
                            " ,[noOfDeliveries]" +
                            " ,[isConfirmed]" +
                            " ,[isJointDelivery]" +
                            " ,[isSpecialRequirement]" +
                            " ,[cdStructure]" +
                            " ,[statusId]" +
                            " ,[isWithinQuotaLimit]" +
                            " ,[globalRateId]" +
                            " ,[quotaDeclarationId]" +
                            " ,[quotaQtyBforBooking]" +
                            " ,[quotaQtyAftBooking]" +
                            " ,[createdBy]" +
                            " ,[createdOn]" +
                            " ,[updatedBy]" +
                            " ,[bookingDatetime]" +
                            " ,[statusDate]" +
                            " ,[updatedOn]" +
                            " ,[bookingQty]" +
                            " ,[bookingRate]" +
                            " ,[comments]" +
                            " ,[pendingQty]" +
                            " ,[authReasons]" +
                            " ,[cdStructureId]" +
                            " ,[parityId]" +
                            " ,[orcAmt]" +
                            " ,[orcMeasure]" +
                            " ,[billingName]" +
                            " ,[poNo]" +
                            " ,[transporterScopeYn]" +   //Saket [2017-11-10] Added
                            " ,[vehicleNo]" +   //Sanjay [2017-11-23] Added
                            " ,[brandId]" +   //Sanjay [2017-11-23] Added
                            " ,[freightAmt]" +
                            " ,[poFileBase64]" +
                            " ,[poDate]" + //Vijaymala[2018-02-26]Added
                            " ,[orcPersonName]" +
                            " ,[isOverdueExist]" +      //Priyanka [13-06-2018] Added for SHIVANGI.
                            " ,[sizesQty]" +            //Priyanka [21-06-2018] Added for SHIVANGI.
                            " ,[directorRemark]" +      //Priyanka [25-06-2018] 
                            " ,[statusBy]" +            //Priyanka [27-07-2018] 
                            " ,[bookingType]" +
                            " ,[isSez]" +
                            " ,[uomQty]" +  // Aniket [4-6-2019]
                            " ,[pendingUomQty]" +  // Aniket [4-6-2019]
                            " ,[isInUom]" +  //Aniket[13-6-2019]
                            " ,[isItemized]" + //Aniket[13-6-2019]
                            " )" +
                " VALUES (" +
                            "  @CnFOrgId " +
                            " ,@DealerOrgId " +
                            " ,@DeliveryDays " +
                            " ,@NoOfDeliveries " +
                            " ,@IsConfirmed " +
                            " ,@IsJointDelivery " +
                            " ,@IsSpecialRequirement " +
                            " ,@CdStructure " +
                            " ,@StatusId " +
                            " ,@IsWithinQuotaLimit " +
                            " ,@GlobalRateId " +
                            " ,@QuotaDeclarationId " +
                            " ,@QuotaQtyBforBooking " +
                            " ,@QuotaQtyAftBooking " +
                            " ,@CreatedBy " +
                            " ,@CreatedOn " +
                            " ,@UpdatedBy " +
                            " ,@BookingDatetime " +
                            " ,@StatusDate " +
                            " ,@UpdatedOn " +
                            " ,@BookingQty " +
                            " ,@BookingRate " +
                            " ,@Comments " +
                            " ,@PendingQty " +
                            " ,@AuthReasons " +
                            " ,@cdStructureId " +
                            " ,@parityId " +
                            " ,@orcAmt " +
                            " ,@orcMeasure " +
                            " ,@billingName " +
                            " ,@poNo " +
                            " ,@TransporterScopeYn " +   //Saket [2017-11-10] Added
                            " ,@vehicleNo " +   //Sanjay [2017-11-23] Added.
                            " ,@brandId " +   //Saket [2017-11-10] Added
                            " ,@freightAmt " +   //Vijaymala [2017-12-05] Added
                            " ,@poFileBase64 " +   //Vijaymala [2017-12-05] Added
                            " ,@poDate" + //Vijaymala[2018-02-26]Added
                            " ,@ORCPersonName" +
                            " ,@IsOverdueExist" +       //Priyanka [13-06-2018] Added for SHIVANGI.
                            " ,@SizesQty" +             //Priyanka [21-06-2018] Added for SHIVANGI.
                            " ,@DirectorRemark" +       //Priyanka [25-06-2018]
                            " ,@StatusBy" +             //Priyanka [27-07-2018]
                            " ,@BookingType" +
                            " ,@IsSez" +
                            " ,@uomQty" +
                            " ,@pendingUomQty" +
                            " ,@isInUom" +
                            " ,@isItemized" +
                             " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            String sqlSelectIdentityQry = "Select @@Identity";

            //cmdInsert.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
            cmdInsert.Parameters.Add("@CnFOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CnFOrgId);
            cmdInsert.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.DealerOrgId);
            cmdInsert.Parameters.Add("@DeliveryDays", System.Data.SqlDbType.Int).Value = tblBookingsTO.DeliveryDays;
            cmdInsert.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblBookingsTO.NoOfDeliveries;
            cmdInsert.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsConfirmed;
            cmdInsert.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsJointDelivery;
            cmdInsert.Parameters.Add("@IsSpecialRequirement", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsSpecialRequirement;
            cmdInsert.Parameters.Add("@CdStructure", System.Data.SqlDbType.Decimal).Value = tblBookingsTO.CdStructure;
            cmdInsert.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingsTO.StatusId;
            cmdInsert.Parameters.Add("@IsWithinQuotaLimit", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsWithinQuotaLimit;
            cmdInsert.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.GlobalRateId);
            cmdInsert.Parameters.Add("@QuotaDeclarationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaDeclarationId);
            cmdInsert.Parameters.Add("@QuotaQtyBforBooking", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaQtyBforBooking);
            cmdInsert.Parameters.Add("@QuotaQtyAftBooking", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaQtyAftBooking);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.UpdatedBy);
            cmdInsert.Parameters.Add("@BookingDatetime", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.BookingDatetime;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.StatusDate;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.UpdatedOn);
            cmdInsert.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingQty;
            cmdInsert.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingRate;
            cmdInsert.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.Comments);
            cmdInsert.Parameters.Add("@PendingQty", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PendingQty);
            cmdInsert.Parameters.Add("@AuthReasons", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.AuthReasons);
            cmdInsert.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CdStructureId);
            cmdInsert.Parameters.Add("@parityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.ParityId);
            cmdInsert.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcAmt);
            cmdInsert.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcMeasure);
            cmdInsert.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BillingName);
            cmdInsert.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoNo);
            cmdInsert.Parameters.Add("@TransporterScopeYn", System.Data.SqlDbType.Int).Value = tblBookingsTO.TransporterScopeYn;  //Saket [2017-11-10] Added
            cmdInsert.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.VehicleNo);  //Sanjay [2017-11-23] Added
            cmdInsert.Parameters.Add("@brandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BrandId);  //Sanjay [2017-11-23] Added
            cmdInsert.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.FreightAmt);//Vijaymala [2017-12-05] Added
            cmdInsert.Parameters.Add("@poFileBase64", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoFileBase64);  //Vijaymala [2017-12-05] Added
            cmdInsert.Parameters.Add("@poDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoDate);//Vijaymala [2018-02-26] Added
            cmdInsert.Parameters.Add("@ORCPersonName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.ORCPersonName);
            cmdInsert.Parameters.Add("@isOverdueExist", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsOverdueExist);     //Priyanka [13-06-2018] Added for SHIVANGI.
            cmdInsert.Parameters.Add("@sizesQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.SizesQty);    //PRiyanka [21-06-2018] Added for SHIVANGI. 
            cmdInsert.Parameters.Add("@DirectorRemark", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.DirectorRemark);    //Priyanka [25-06-2018]

            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.StatusBy);        //Priyanka [27-07-2018]
            cmdInsert.Parameters.Add("@BookingType", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BookingType);        //Priyanka [27-07-2018]
            cmdInsert.Parameters.Add("@IsSez", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsSez);
            cmdInsert.Parameters.Add("@uomQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.UomQty);
            cmdInsert.Parameters.Add("@pendingUomQty", System.Data.SqlDbType.Decimal).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PendingUomQty);
            cmdInsert.Parameters.Add("@isInUom", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsInUom);
            cmdInsert.Parameters.Add("@isItemized", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsItemized);

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = sqlSelectIdentityQry;
                tblBookingsTO.IdBooking = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblBookings(TblBookingsTO tblBookingsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblBookingsTO, cmdUpdate);
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

        public int UpdateTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblBookingsTO, cmdUpdate);
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

        public int UpdateBookingPendingQty(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblBookings] SET " +
                                "  [updatedBy]= @UpdatedBy" +
                                " ,[updatedOn]= @UpdatedOn" +
                                " ,[pendingQty] = @PendingQty" +
                                " ,[pendingUomQty] = @pendingUomQty" +
                                " WHERE idBooking = @IdBooking ";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;
                cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
                cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.UpdatedBy;
                cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.UpdatedOn;
                cmdUpdate.Parameters.Add("@PendingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.PendingQty;
                //Aniket [13-6-2019]
                cmdUpdate.Parameters.Add("@pendingUomQty", System.Data.SqlDbType.Decimal).Value = tblBookingsTO.PendingUomQty;

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

        public int ExecuteUpdationCommand(TblBookingsTO tblBookingsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblBookings] SET " +
                            "  [cnFOrgId]= @CnFOrgId" +
                            " ,[dealerOrgId]= @DealerOrgId" +
                            " ,[deliveryDays]= @DeliveryDays" +
                            " ,[noOfDeliveries]= @NoOfDeliveries" +
                            " ,[isConfirmed]= @IsConfirmed" +
                            " ,[isJointDelivery]= @IsJointDelivery" +
                            " ,[isSpecialRequirement]= @IsSpecialRequirement" +
                            " ,[cdStructure]= @CdStructure" +
                            " ,[statusId]= @StatusId" +
                            " ,[isWithinQuotaLimit]= @IsWithinQuotaLimit" +
                            " ,[globalRateId]= @GlobalRateId" +
                            " ,[quotaDeclarationId]= @QuotaDeclarationId" +
                            " ,[quotaQtyBforBooking]= @QuotaQtyBforBooking" +
                            " ,[quotaQtyAftBooking]= @QuotaQtyAftBooking" +
                            " ,[updatedBy]= @UpdatedBy" +
                            " ,[bookingDatetime]= @BookingDatetime" +
                            " ,[statusDate]= @StatusDate" +
                            " ,[updatedOn]= @UpdatedOn" +
                            " ,[bookingQty]= @BookingQty" +
                            " ,[bookingRate]= @BookingRate" +
                            " ,[comments] = @Comments" +
                            " ,[pendingQty] = @pendingQty" +
                            " ,[cdStructureId] = @cdStructureId" +
                            " ,[parityId] = @parityId" +
                            " ,[orcAmt] = @orcAmt" +
                            " ,[orcMeasure] = @orcMeasure" +
                            " ,[billingName] = @billingName" +
                            " ,[poNo] = @poNo" +
                            " ,[transporterScopeYn] = @TransporterScopeYn" +  //Saket [2017-11-10] Added.
                            " ,[vehicleNo] = @vehicleNo" +  //Sanjay [2017-11-23] Added.
                            " ,[brandId] = @brandId" +  //Sanjay [2017-11-23] Added.
                            " ,[freightAmt]=@freightAmt " +   //Vijaymala [2017-12-05] Added
                            " ,[poFileBase64]=@poFileBase64 " +   //Vijaymala [2017-12-05] Added
                            " ,[poDate]=@poDate " +   //Vijaymala [2018-02-26] Added
                            " ,[orcPersonName]=@ORCPersonName " +

                            " ,[isOverdueExist]= @IsOverdueExist" +           //Priyanka [11-06-2018] : Added for SHIVANGI.
                            " ,[sizesQty]= @SizesQty" +                       //Priyanka [21-06-2018] : Added for SHIVANGI.
                            " ,[directorRemark]= @DirectorRemark" +           //Priyanka [25-06-2018] 
                            " ,[statusBy] = @StatusBy" +                      //Priyanka [27-07-2018]
                            " ,[bookingType] = @BookingType" +
                            " ,[isSez] = @IsSez" +
                            " ,[uomQty] = @uomQty" +
                            " ,[pendingUomQty] = @pendingUomQty" +
                            " ,[isInUom] = @isInUom" +
                            " ,[isItemized] = @isItemized" +
                            " WHERE  [idBooking] = @IdBooking";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
            cmdUpdate.Parameters.Add("@CnFOrgId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CnFOrgId);
            cmdUpdate.Parameters.Add("@DealerOrgId", System.Data.SqlDbType.Int).Value = tblBookingsTO.DealerOrgId;
            cmdUpdate.Parameters.Add("@DeliveryDays", System.Data.SqlDbType.Int).Value = tblBookingsTO.DeliveryDays;
            cmdUpdate.Parameters.Add("@NoOfDeliveries", System.Data.SqlDbType.Int).Value = tblBookingsTO.NoOfDeliveries;
            cmdUpdate.Parameters.Add("@IsConfirmed", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsConfirmed;
            cmdUpdate.Parameters.Add("@IsJointDelivery", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsJointDelivery;
            cmdUpdate.Parameters.Add("@IsSpecialRequirement", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsSpecialRequirement;
            cmdUpdate.Parameters.Add("@CdStructure", System.Data.SqlDbType.Decimal).Value = tblBookingsTO.CdStructure;
            cmdUpdate.Parameters.Add("@StatusId", System.Data.SqlDbType.Int).Value = tblBookingsTO.StatusId;
            cmdUpdate.Parameters.Add("@IsWithinQuotaLimit", System.Data.SqlDbType.Int).Value = tblBookingsTO.IsWithinQuotaLimit;
            cmdUpdate.Parameters.Add("@GlobalRateId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.GlobalRateId);
            cmdUpdate.Parameters.Add("@QuotaDeclarationId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaDeclarationId);
            cmdUpdate.Parameters.Add("@QuotaQtyBforBooking", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaQtyBforBooking);
            cmdUpdate.Parameters.Add("@QuotaQtyAftBooking", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.QuotaQtyAftBooking);
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblBookingsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@BookingDatetime", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.BookingDatetime;
            cmdUpdate.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.StatusDate;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblBookingsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@BookingQty", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingQty;
            cmdUpdate.Parameters.Add("@BookingRate", System.Data.SqlDbType.NVarChar).Value = tblBookingsTO.BookingRate;
            cmdUpdate.Parameters.Add("@pendingQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PendingQty);
            cmdUpdate.Parameters.Add("@Comments", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.Comments);
            cmdUpdate.Parameters.Add("@cdStructureId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.CdStructureId);
            cmdUpdate.Parameters.Add("@parityId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.ParityId);
            cmdUpdate.Parameters.Add("@orcAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcAmt);
            cmdUpdate.Parameters.Add("@orcMeasure", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.OrcMeasure);
            cmdUpdate.Parameters.Add("@billingName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BillingName);
            cmdUpdate.Parameters.Add("@poNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoNo);
            cmdUpdate.Parameters.Add("@TransporterScopeYn", System.Data.SqlDbType.Int).Value = tblBookingsTO.TransporterScopeYn;  //Saket [2017-11-10] Added
            cmdUpdate.Parameters.Add("@vehicleNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.VehicleNo);
            cmdUpdate.Parameters.Add("@brandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BrandId);
            cmdUpdate.Parameters.Add("@freightAmt", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.FreightAmt);//Vijaymala [2017-12-05] Added
            cmdUpdate.Parameters.Add("@poFileBase64", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoFileBase64);//Vijaymala [2017-12-05] Added
            cmdUpdate.Parameters.Add("@poDate", System.Data.SqlDbType.DateTime).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PoDate);
            cmdUpdate.Parameters.Add("@ORCPersonName", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.ORCPersonName);
            cmdUpdate.Parameters.Add("@isOverdueExist", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsOverdueExist);   //Priyanka [11-06-2018] : Added for SHIVANGI.
            cmdUpdate.Parameters.Add("@sizesQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.SizesQty);    //Priyanka [21-06-2018]
            cmdUpdate.Parameters.Add("@DirectorRemark", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.DirectorRemark); //PRiyanka [25-06-2018]

            cmdUpdate.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.StatusBy);            //Priyanka [27-07-2018]
            cmdUpdate.Parameters.Add("@BookingType", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.BookingType);
            cmdUpdate.Parameters.Add("@IsSez", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsSez);
            //Aniket [13-6-2019]
            cmdUpdate.Parameters.Add("@uomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.UomQty);
            cmdUpdate.Parameters.Add("@pendingUomQty", System.Data.SqlDbType.Decimal).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.PendingUomQty);
            cmdUpdate.Parameters.Add("@isInUom", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsInUom);
            cmdUpdate.Parameters.Add("@isItemized", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblBookingsTO.IsItemized);


            return cmdUpdate.ExecuteNonQuery();
        }
       
        #endregion

        #region Deletion
        public int DeleteTblBookings(Int32 idBooking)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idBooking, cmdDelete);
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

        public int DeleteTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idBooking, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idBooking, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblBookings] " +
            " WHERE idBooking = " + idBooking + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idBooking", System.Data.SqlDbType.Int).Value = tblBookingsTO.IdBooking;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
