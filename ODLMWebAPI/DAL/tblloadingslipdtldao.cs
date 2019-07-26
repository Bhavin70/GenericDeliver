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
    public class TblLoadingSlipDtlDAO : ITblLoadingSlipDtlDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblLoadingSlipDtlDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT tempLoadingSlipDtl.*,tblBookings.bookingRate ,tblBookings.comments FROM tempLoadingSlipDtl " +
                                  " LEFT JOIN tblBookings ON bookingId = idBooking " +

                                   // Vaibhav [20-Nov-2017] Added to select from finalLoadingSlipDtl
                                   " UNION ALL " +

                                   " SELECT finalLoadingSlipDtl.*,tblBookings.bookingRate ,tblBookings.comments FROM finalLoadingSlipDtl " +
                                   " LEFT JOIN tblBookings ON bookingId = idBooking ";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblLoadingSlipDtlTO> SelectAllTblLoadingSlipDtl()
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
                List<TblLoadingSlipDtlTO> list = ConvertDTToList(sqlReader);
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

        public TblLoadingSlipDtlTO SelectTblLoadingSlipDtl(Int32 idLoadSlipDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE idLoadSlipDtl = " + idLoadSlipDtl + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipDtlTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId = " + loadingSlipId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipDtlTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE loadingSlipId = " + loadingSlipId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipDtlTO> list = ConvertDTToList(reader);
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
                if (reader != null)
                    reader.Dispose();
                cmdSelect.Dispose();
            }
        }

        public List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromLoadingId(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                //cmdSelect.CommandText = SqlSelectQuery() + " WHERE loadingSlipId IN (SELECT idLoadingSlip FROM tempLoadingSlip WHERE loadingId=" + loadingId + ")";

                // vaibhav [09-jan-2018] Commented and added to select from finalLoadingSlipDtl
                cmdSelect.CommandText = " SELECT tempLoadingSlipDtl.*,tblBookings.bookingRate ,tblBookings.comments FROM tempLoadingSlipDtl " +
                                        " LEFT JOIN tblBookings ON bookingId = idBooking " +
                                        " WHERE loadingSlipId IN (SELECT idLoadingSlip FROM tempLoadingSlip WHERE loadingId=" + loadingId + ")" +

                                        " UNION ALL " +

                                        " SELECT finalLoadingSlipDtl.*,tblBookings.bookingRate ,tblBookings.comments FROM finalLoadingSlipDtl " +
                                        " LEFT JOIN tblBookings ON bookingId = idBooking " +
                                        " WHERE loadingSlipId IN (SELECT idLoadingSlip FROM finalLoadingSlip WHERE loadingId=" + loadingId + ")";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipDtlTO> list = ConvertDTToList(sqlReader);
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

        public List<TblLoadingSlipDtlTO> ConvertDTToList(SqlDataReader tblLoadingSlipDtlTODT)
        {
            List<TblLoadingSlipDtlTO> tblLoadingSlipDtlTOList = new List<TblLoadingSlipDtlTO>();
            if (tblLoadingSlipDtlTODT != null)
            {
                while (tblLoadingSlipDtlTODT.Read())
                {
                    TblLoadingSlipDtlTO tblLoadingSlipDtlTONew = new TblLoadingSlipDtlTO();
                    if (tblLoadingSlipDtlTODT["idLoadSlipDtl"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.IdLoadSlipDtl = Convert.ToInt32(tblLoadingSlipDtlTODT["idLoadSlipDtl"].ToString());
                    if (tblLoadingSlipDtlTODT["loadingSlipId"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.LoadingSlipId = Convert.ToInt32(tblLoadingSlipDtlTODT["loadingSlipId"].ToString());
                    if (tblLoadingSlipDtlTODT["bookingId"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.BookingId = Convert.ToInt32(tblLoadingSlipDtlTODT["bookingId"].ToString());
                    if (tblLoadingSlipDtlTODT["bookingExtId"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.BookingExtId = Convert.ToInt32(tblLoadingSlipDtlTODT["bookingExtId"].ToString());
                    if (tblLoadingSlipDtlTODT["loadingQty"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.LoadingQty = Convert.ToDouble(tblLoadingSlipDtlTODT["loadingQty"].ToString());
                    if (tblLoadingSlipDtlTODT["bookingRate"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.BookingRate = Convert.ToDouble(tblLoadingSlipDtlTODT["bookingRate"].ToString());
                    if (tblLoadingSlipDtlTODT["comments"] != DBNull.Value)
                        tblLoadingSlipDtlTONew.SpecialNote = Convert.ToString(tblLoadingSlipDtlTODT["comments"].ToString());

                    //Priyanka [26-07-2018] : Added to set IdBooking; 
                    tblLoadingSlipDtlTONew.IdBooking = tblLoadingSlipDtlTONew.BookingId;
                    tblLoadingSlipDtlTOList.Add(tblLoadingSlipDtlTONew);
                }
            }
            return tblLoadingSlipDtlTOList;
        }

        public Dictionary<int, Dictionary<int, Double>> SelectCnfAndDealerWiseLoadingQtyDCT(Int32 cnfId, DateTime loadingDate)
        {
            Dictionary<int, Dictionary<int, Double>> cnfDealerDCT = new Dictionary<int, Dictionary<int, double>>();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            String statusIn = (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_COMPLETED + "," +
                            (int)Constants.TranStatusE.LOADING_DELIVERED + "," + (int)Constants.TranStatusE.LOADING_GATE_IN + "," +
                            (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING + "," + (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN;
            try
            {
                conn.Open();
                if (cnfId == 0)
                    cmdSelect.CommandText = " SELECT cnfOrgId,dealerOrgId, SUM(loadingQty) loadingQty " +
                                           " FROM tempLoadingSlipDtl slipDtl " +
                                           " INNER JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                           " INNER JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                           " WHERE loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                           " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                           " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                           " GROUP BY cnfOrgId,dealerOrgId" +

                                           // Vaibhav [10-Jan-2018] Added to select from finalLoadingSlipDtl
                                           " UNION ALL " +

                                           " SELECT cnfOrgId,dealerOrgId, SUM(loadingQty) loadingQty " +
                                           " FROM finalLoadingSlipDtl slipDtl " +
                                           " INNER JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                           " INNER JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                           " WHERE loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                           " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                           " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                           " GROUP BY cnfOrgId,dealerOrgId";


                else
                    cmdSelect.CommandText = " SELECT cnfOrgId,dealerOrgId, SUM(loadingQty) loadingQty " +
                                      " FROM tempLoadingSlipDtl slipDtl " +
                                      " INNER JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                      " INNER JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                      " WHERE loading.cnfOrgId=" + cnfId + "AND loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                      " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                      " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                      " GROUP BY cnfOrgId,dealerOrgId" +

                                      // Vaibhav [10-Jan-2018] Added to select from finalLoadingSlipDtl
                                      " UNION ALL " +

                                      " SELECT cnfOrgId,dealerOrgId, SUM(loadingQty) loadingQty " +
                                      " FROM finalLoadingSlipDtl slipDtl " +
                                      " INNER JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                      " INNER JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                      " WHERE loading.cnfOrgId=" + cnfId + "AND loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                      " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                      " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                      " GROUP BY cnfOrgId,dealerOrgId";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        int cnfOrgId = Convert.ToInt32(reader["cnfOrgId"].ToString());
                        if (cnfDealerDCT.ContainsKey(cnfOrgId))
                        {
                            int dealerId = Convert.ToInt32(reader["dealerOrgId"].ToString());
                            double loadingQty = Convert.ToDouble(reader["loadingQty"].ToString());
                            Dictionary<int, Double> tempDCT = cnfDealerDCT[cnfOrgId];
                            tempDCT.Add(dealerId, loadingQty);
                            //cnfDealerDCT[cnfId].Add(dealerId, loadingQty);
                        }
                        else
                        {
                            int dealerId = Convert.ToInt32(reader["dealerOrgId"].ToString());
                            double loadingQty = Convert.ToDouble(reader["loadingQty"].ToString());
                            Dictionary<int, Double> tempDCT = new Dictionary<int, double>();
                            tempDCT.Add(dealerId, loadingQty);
                            cnfDealerDCT.Add(cnfOrgId, tempDCT);
                        }
                    }
                }

                return cnfDealerDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public Dictionary<int, Dictionary<int, Double>> SelectCnfAndDealerWiseLoadedAndBookingDeleteQtyDCT(Int32 cnfId, DateTime loadingDate)
        {
            Dictionary<int, Dictionary<int, Double>> cnfDealerDCT = new Dictionary<int, Dictionary<int, double>>();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            String statusIn = (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_COMPLETED + "," +
                            (int)Constants.TranStatusE.LOADING_DELIVERED + "," + (int)Constants.TranStatusE.LOADING_GATE_IN + "," +
                            (int)Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING + "," + (int)Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN;
            try
            {
                conn.Open();
                if (cnfId == 0)
                    cmdSelect.CommandText = " SELECT cnfOrgId,dealerOrgId, SUM(slipDtl.loadingQty) loadingQty " +
                                           " FROM tempLoadingSlipDtl slipDtl " +
                                           " INNER JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                           " INNER JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                           " INNER JOIN " +
                                           " ( SELECT ext.* FROM tempLoadingSlipExt ext " +
                                           "  INNER JOIN tblBookingQtyConsumption qtyCons ON qtyCons.bookingId = ext.bookingId " +
                                           "  WHERE DAY(createdOn)= " + loadingDate.Day + " AND MONTH(createdOn)= " + loadingDate.Month + " AND YEAR(createdOn)= " + loadingDate.Year + " " +
                                           " ) AS todaysLoading ON todaysLoading.loadingSlipId =loadingSlip.idLoadingSlip " +
                                           " WHERE loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                           " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                           " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                           " GROUP BY cnfOrgId,dealerOrgId" +

                                           // Vaibhav [10-Jan-2018] Added to select from finalLoadingSlipDtl
                                           " UNION ALL " +

                                           " SELECT cnfOrgId,dealerOrgId, SUM(slipDtl.loadingQty) loadingQty " +
                                           " FROM finalLoadingSlipDtl slipDtl " +
                                           " INNER JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                           " INNER JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                           " INNER JOIN " +
                                           " ( SELECT ext.* FROM finalLoadingSlipExt ext " +
                                           "  INNER JOIN tblBookingQtyConsumption qtyCons ON qtyCons.bookingId = ext.bookingId " +
                                           "  WHERE DAY(createdOn)= " + loadingDate.Day + " AND MONTH(createdOn)= " + loadingDate.Month + " AND YEAR(createdOn)= " + loadingDate.Year + " " +
                                           " ) AS todaysLoading ON todaysLoading.loadingSlipId =loadingSlip.idLoadingSlip " +
                                           " WHERE loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                           " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                           " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                           " GROUP BY cnfOrgId,dealerOrgId";
                else
                    cmdSelect.CommandText = " SELECT cnfOrgId,dealerOrgId, SUM(slipDtl.loadingQty) loadingQty " +
                                      " FROM tempLoadingSlipDtl slipDtl " +
                                      " INNER JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                      " INNER JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                      " INNER JOIN " +
                                      " ( SELECT ext.* FROM tempLoadingSlipExt ext " +
                                      "  INNER JOIN tblBookingQtyConsumption qtyCons ON qtyCons.bookingId = ext.bookingId " +
                                      "  WHERE DAY(createdOn)= " + loadingDate.Day + " AND MONTH(createdOn)= " + loadingDate.Month + " AND YEAR(createdOn)= " + loadingDate.Year + " " +
                                      " ) AS todaysLoading ON todaysLoading.loadingSlipId =loadingSlip.idLoadingSlip " +
                                      " WHERE loading.cnfOrgId=" + cnfId + "AND loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                      " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                      " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                      " GROUP BY cnfOrgId,dealerOrgId" +

                                      // Vaibhav [10-Jan-2018] Added to select from finalLoadingSlipDtl
                                      " UNION ALL " +

                                      " SELECT cnfOrgId,dealerOrgId, SUM(slipDtl.loadingQty) loadingQty " +
                                      " FROM finalLoadingSlipDtl slipDtl " +
                                      " INNER JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                      " INNER JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                      " INNER JOIN " +
                                      " ( SELECT ext.* FROM finalLoadingSlipExt ext " +
                                      "  INNER JOIN tblBookingQtyConsumption qtyCons ON qtyCons.bookingId = ext.bookingId " +
                                      "  WHERE DAY(createdOn)= " + loadingDate.Day + " AND MONTH(createdOn)= " + loadingDate.Month + " AND YEAR(createdOn)= " + loadingDate.Year + " " +
                                      " ) AS todaysLoading ON todaysLoading.loadingSlipId =loadingSlip.idLoadingSlip " +
                                      " WHERE loading.cnfOrgId=" + cnfId + "AND loading.statusId IN(" + statusIn + ") AND DAY(loading.createdOn)=" + loadingDate.Day +
                                      " AND MONTH(loading.createdOn)=" + loadingDate.Month +
                                      " AND YEAR(loading.createdOn)=" + loadingDate.Year +
                                      " GROUP BY cnfOrgId,dealerOrgId";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        int cnfOrgId = Convert.ToInt32(reader["cnfOrgId"].ToString());
                        if (cnfDealerDCT.ContainsKey(cnfOrgId))
                        {
                            int dealerId = Convert.ToInt32(reader["dealerOrgId"].ToString());
                            double loadingQty = Convert.ToDouble(reader["loadingQty"].ToString());
                            cnfDealerDCT[cnfId].Add(dealerId, loadingQty);
                        }
                        else
                        {
                            int dealerId = Convert.ToInt32(reader["dealerOrgId"].ToString());
                            double loadingQty = Convert.ToDouble(reader["loadingQty"].ToString());
                            Dictionary<int, Double> tempDCT = new Dictionary<int, double>();
                            tempDCT.Add(dealerId, loadingQty);
                            cnfDealerDCT.Add(cnfOrgId, tempDCT);
                        }
                    }
                }

                return cnfDealerDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public Dictionary<int, Double> SelectBookingWiseLoadingQtyDCT(DateTime loadingDate, Boolean isDeletedOnly,Int32 brandId)
        {
            Dictionary<int, Double> loadingQtyDCT = new Dictionary<int, double>();
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            String brandIdFilter=null; // [15-jan-2019]added by aniket to filter based on brand
            String statusIn = (int)Constants.TranStatusE.LOADING_CONFIRM + "," + (int)Constants.TranStatusE.LOADING_NOT_CONFIRM;

            String delStatusIn = (int)Constants.TranStatusE.LOADING_CANCEL + "";
            // [15-jan-2019]added by aniket to filter based on brand
            if (brandId != 0)
                brandIdFilter = " AND brandId=" + brandId;
            else
                brandIdFilter = "";
            try
            {
                conn.Open();
                if (!isDeletedOnly)
                    cmdSelect.CommandText = " SELECT bookingId, SUM(loadingQty) loadingQty " +
                                            " FROM tempLoadingSlipDtl slipDtl " +
                                            " INNER JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                            " INNER JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                            " WHERE loading.idLoading IN(SELECT DISTINCT loadingId FROM tempLoadingStatusHistory WHERE " +
                                            " DAY(createdOn)=" + loadingDate.Day +
                                            " AND MONTH(createdOn)=" + loadingDate.Month +
                                            " AND YEAR(createdOn)=" + loadingDate.Year + " AND statusId IN(" + statusIn + "))" + brandIdFilter +
                                            " GROUP BY bookingId" +

                                            // Vaibhav [10-Jan-2018] Added to select from finalLoadingSlipDtl
                                            " UNION ALL " +

                                            " SELECT bookingId, SUM(loadingQty) loadingQty " +
                                            " FROM finalLoadingSlipDtl slipDtl " +
                                            " INNER JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                            " INNER JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                            " WHERE loading.idLoading IN(SELECT DISTINCT loadingId FROM finalLoadingStatusHistory WHERE " +
                                            " DAY(createdOn)=" + loadingDate.Day +
                                            " AND MONTH(createdOn)=" + loadingDate.Month +
                                            " AND YEAR(createdOn)=" + loadingDate.Year + " AND statusId IN(" + statusIn + "))" +
                                            " GROUP BY bookingId";
                else
                    cmdSelect.CommandText = " SELECT bookingId, SUM(loadingQty) loadingQty " +
                                            " FROM tempLoadingSlipDtl slipDtl " +
                                            " INNER JOIN tempLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                            " INNER JOIN tempLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                            " WHERE loading.idLoading IN(SELECT DISTINCT loadingId FROM tempLoadingStatusHistory WHERE " +
                                            " DAY(createdOn)=" + loadingDate.Day +
                                            " AND MONTH(createdOn)=" + loadingDate.Month +
                                            " AND YEAR(createdOn)=" + loadingDate.Year + " AND statusId IN(" + delStatusIn + "))" + brandIdFilter +
                                            " GROUP BY bookingId" +

                                            // Vaibhav [10-Jan-2018] Added to select from finalLoadingSlipDtl
                                            " UNION ALL " +

                                            " SELECT bookingId, SUM(loadingQty) loadingQty " +
                                            " FROM finalLoadingSlipDtl slipDtl " +
                                            " INNER JOIN finalLoadingSlip loadingSlip ON loadingSlip.idLoadingSlip = slipDtl.loadingSlipId " +
                                            " INNER JOIN finalLoading loading ON loading.idLoading = loadingSlip.loadingId " +
                                            " WHERE loading.idLoading IN(SELECT DISTINCT loadingId FROM finalLoadingStatusHistory WHERE " +
                                            " DAY(createdOn)=" + loadingDate.Day +
                                            " AND MONTH(createdOn)=" + loadingDate.Month +
                                            " AND YEAR(createdOn)=" + loadingDate.Year + " AND statusId IN(" + delStatusIn + "))" + brandIdFilter +
                                            " GROUP BY bookingId";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        int bookingId = Convert.ToInt32(reader["bookingId"].ToString());
                        Double loadingQty = Convert.ToDouble(reader["loadingQty"].ToString());

                        if (loadingQtyDCT.ContainsKey(bookingId))
                        {
                            loadingQtyDCT[bookingId] += loadingQty;
                        }
                        else
                        {
                            loadingQtyDCT.Add(bookingId, loadingQty);
                        }
                    }
                }

                return loadingQtyDCT;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Vijaymala added [24-04-2018]:added to get loading slip details from bookingId
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                cmdSelect.CommandText = " SELECT * FROM (" + SqlSelectQuery() + ")sq1 WHERE bookingId=" + bookingId ;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblLoadingSlipDtlTO> list = ConvertDTToList(sqlReader);
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
        #endregion

        #region Insertion
        public int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblLoadingSlipDtlTO, cmdInsert);
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

        public int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblLoadingSlipDtlTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tempLoadingSlipDtl]( " +
                            "  [loadingSlipId]" +
                            " ,[bookingId]" +
                            " ,[bookingExtId]" +
                            " ,[loadingQty]" +
                            " )" +
                " VALUES (" +
                            "  @LoadingSlipId " +
                            " ,@BookingId " +
                            " ,@BookingExtId " +
                            " ,@LoadingQty " +
                            " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdLoadSlipDtl", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.IdLoadSlipDtl;
            cmdInsert.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.LoadingSlipId;
            cmdInsert.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.BookingId;
            cmdInsert.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipDtlTO.BookingExtId);
            cmdInsert.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipDtlTO.LoadingQty;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblLoadingSlipDtlTO.IdLoadSlipDtl = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion

        #region Updation
        public int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblLoadingSlipDtlTO, cmdUpdate);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblLoadingSlipDtlTO, cmdUpdate);
            }
            catch (Exception ex)
            {


                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tempLoadingSlipDtl] SET " +
           // "  [idLoadSlipDtl] = @IdLoadSlipDtl" +
            " [loadingSlipId]= @LoadingSlipId" +
            " ,[bookingId]= @BookingId" +
            " ,[bookingExtId]= @BookingExtId" +
            " ,[loadingQty] = @LoadingQty" +
            " WHERE [idLoadSlipDtl] = @IdLoadSlipDtl ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdLoadSlipDtl", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.IdLoadSlipDtl;
            cmdUpdate.Parameters.Add("@LoadingSlipId", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.LoadingSlipId;
            cmdUpdate.Parameters.Add("@BookingId", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.BookingId;
            cmdUpdate.Parameters.Add("@BookingExtId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblLoadingSlipDtlTO.BookingExtId);
            cmdUpdate.Parameters.Add("@LoadingQty", System.Data.SqlDbType.NVarChar).Value = tblLoadingSlipDtlTO.LoadingQty;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idLoadSlipDtl, cmdDelete);
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

        public int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idLoadSlipDtl, cmdDelete);
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

        public int DeleteTblLoadingSlipDtlNew(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
          //  SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipDtl] " +
           " WHERE loadingSlipId = " + loadingSlipId + "";
                return cmdDelete.ExecuteNonQuery();
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

        public int ExecuteDeletionCommand(Int32 idLoadSlipDtl, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tempLoadingSlipDtl] " +
            " WHERE idLoadSlipDtl = " + idLoadSlipDtl + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idLoadSlipDtl", System.Data.SqlDbType.Int).Value = tblLoadingSlipDtlTO.IdLoadSlipDtl;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
