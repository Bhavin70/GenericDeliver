using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblLoadingBL : ITblLoadingBL
    {
        #region Selection


        public List<TblLoadingTO> SelectAllTblLoadingList()
        {
            return TblLoadingDAO.SelectAllTblLoading();
        }
        //Priyanka [11-05-2018] : Added to get all loading slip list whose 
        //                        vehicle status is Gate In Or Loading Completed.
        public List<TblLoadingTO> SelectAllTblLoadingListForConvertNCToC()
        {
            return TblLoadingDAO.SelectAllTblLoadingListForConvertNCToC();
        }
        public List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId(int parentLoadingId)
        {
            return TblLoadingDAO.SelectAllLoadingsFromParentLoadingId(parentLoadingId);
        }

        public List<TblLoadingTO> SelectAllTblloadingList(DateTime fromDate, DateTime toDate)
        {
            return TblLoadingDAO.SelectAllTblloadingList(fromDate, toDate);
        }

        public List<TblLoadingTO> SelectAllTblLoadingList(List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId,Int32 superwisorId)
        {
            //Priyanka [12-12-2018]
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }

            List<TblLoadingTO> tblLoadingTOList = TblLoadingDAO.SelectAllTblLoading(tblUserRoleTO, cnfId, loadingStatusId, fromDate, toDate, loadingTypeId, dealerId, isConfirm, brandId, loadingNavigateId, superwisorId);

            if (cnfId > 0)
            {
                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
                {
                    String name = TblOrganizationDAO.SelectFirmNameOfOrganiationById(cnfId);
                    if (!String.IsNullOrEmpty(name))
                    {
                        tblLoadingTOList.ForEach(c => c.CnfOrgName = name);
                    }
                }
            }

            return tblLoadingTOList;

        } 
        /// <summary>
        /// @Kiran 19-04-2018
        /// </summary>
        /// <param name="TblLoadingTO"></param>
        /// <returns></returns>
        public List<TblLoadingTO> GetLoadingDetailsForReport(DateTime fromDate, DateTime toDate)
        {
            List<TblLoadingTO> tblLoadingToList = new List<TblLoadingTO>();
            TblLoadingTO tblLoadingTO = new TblLoadingTO();
            List<TblLoadingTO> tblLoadingTOList = BL.TblLoadingBL.SelectAllTblloadingList(fromDate, toDate);//.FindAll(ele => ele.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT);

            if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
            {
                List<DropDownTO> MaterialList = BL.TblMaterialBL.SelectAllMaterialListForDropDown();
                for (int i = 0; i < tblLoadingTOList.Count; i++)
                {
                    tblLoadingTO = tblLoadingTOList[i];

                    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading);

                    tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(Convert.ToInt32(tblLoadingTO.IdLoading));
                    if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0)
                    {

                        List<TblLoadingSlipTO> groupByVehicalDealer = tblLoadingTO.LoadingSlipList.GroupBy(g => g.DealerOrgId).Select(s => s.FirstOrDefault()).ToList();
                        for (int j = 0; j < groupByVehicalDealer.Count; j++)
                        {
                            List<TblLoadingSlipExtTO> LoadingSlipExtList = new List<TblLoadingSlipExtTO>();
                            TblLoadingSlipTO temp = groupByVehicalDealer[j];
                            List<TblLoadingSlipTO> gropByList = tblLoadingTO.LoadingSlipList.Where(w => w.DealerOrgId == temp.DealerOrgId).ToList();
                            if (gropByList != null && gropByList.Count > 0)
                            {
                                List<TblLoadingSlipAddressTO> addressList = new List<TblLoadingSlipAddressTO>();
                                gropByList.ForEach(f => addressList.AddRange(f.DeliveryAddressTOList));
                                addressList = addressList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).OrderBy(o => o.LoadingLayerId).ToList();
                                temp.DeliveryAddressTOList = addressList;
                                foreach (var extentionItem in gropByList)
                                {
                                    LoadingSlipExtList.AddRange(extentionItem.LoadingSlipExtTOList);
                                }
                                if (LoadingSlipExtList != null && LoadingSlipExtList.Count > 0)
                                {
                                    string quntity = "";
                                    double totalSumLoadingQty = 0;
                                    double totalloadedQuantity = 0;


                                    double todaysTotalSumLoadingQty = 0;
                                    double todaysTotalloadedQuantity = 0;

                                    Dictionary<string, string> materialDictionary = new Dictionary<string, string>();
                                    Dictionary<string, string> todaysMaterialDictionary = new Dictionary<string, string>();

                                    foreach (var item in MaterialList)
                                    {
                                        List<TblLoadingSlipExtTO> tempExtList = new List<TblLoadingSlipExtTO>();
                                        tempExtList = LoadingSlipExtList.Where(w => w.MaterialId == item.Value).ToList();
                                        double sum = tempExtList.Sum(s => s.LoadingQty);
                                        double loadedQuantity = tempExtList.Sum(s => s.LoadedWeight);
                                        if (loadedQuantity != 0)
                                        {
                                            loadedQuantity = Math.Round(loadedQuantity / 1000, 3);
                                        }
                                        totalSumLoadingQty += sum;
                                        totalloadedQuantity += loadedQuantity;
                                        quntity = loadedQuantity + "/" + sum;
                                        materialDictionary.Add(item.Text, quntity);


                                        //Todays
                                        tempExtList = LoadingSlipExtList.Where(w => w.MaterialId == item.Value).ToList();
                                        tempExtList = tempExtList.Where(w => w.UpdatedOn >= fromDate && w.UpdatedOn <= toDate).ToList();

                                        double todaysSum = tempExtList.Sum(s => s.LoadingQty);
                                        double todaysloadedQuantity = tempExtList.Sum(s => s.LoadedWeight);
                                        if (todaysloadedQuantity != 0)
                                        {
                                            todaysloadedQuantity = Math.Round(todaysloadedQuantity / 1000, 3);
                                        }

                                        todaysTotalSumLoadingQty += sum;
                                        todaysTotalloadedQuantity += todaysloadedQuantity;
                                        String todaysQuntity = todaysloadedQuantity + "/" + sum;
                                        todaysMaterialDictionary.Add(item.Text, todaysQuntity);


                                    }


                                    materialDictionary.Add("Todays Total", todaysTotalloadedQuantity + "/" + todaysTotalSumLoadingQty);
                                    //temp.Dictionary.Add(materialDictionary);

                                    materialDictionary.Add("Total", totalloadedQuantity + "/" + totalSumLoadingQty);
                                    temp.Dictionary.Add(materialDictionary);
                                    materialDictionary = null;

                                    todaysMaterialDictionary.Add("Todays Total", todaysTotalloadedQuantity + "/" + todaysTotalSumLoadingQty);
                                    //temp.TodaysDictionary.Add(todaysMaterialDictionary);

                                    todaysMaterialDictionary.Add("Total", totalloadedQuantity + "/" + totalSumLoadingQty);
                                    temp.TodaysDictionary.Add(todaysMaterialDictionary);
                                }


                            }
                        }
                        tblLoadingTO.LoadingSlipList = groupByVehicalDealer;
                        tblLoadingToList.Add(tblLoadingTO);
                    }
                }
            }
            return tblLoadingToList;
        }
        /// <summary>
        /// @Kiran 11-12-2017
        /// </summary>
        /// <param name="tblUserRoleTO"></param>
        /// <returns></returns>
        public List<TblLoadingTO> SelectAllTblLoadingLinkList(List<TblUserRoleTO> tblUserRoleTOList, Int32 dearlerOrgId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate)
        {

            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return TblLoadingDAO.SelectAllTblLoadingLinkList(tblUserRoleTO, dearlerOrgId, loadingStatusId, fromDate, toDate);
        }

        public List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingDAO.SelectAllLoadingListByStatus(statusId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblLoadingTO SelectTblLoadingTO(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingDAO.SelectTblLoading(idLoading, conn, tran);
        }

        public TblLoadingTO SelectTblLoadingTO(Int32 idLoading)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingDAO.SelectTblLoading(idLoading, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblLoadingTO SelectTblLoadingTOByLoadingSlipId(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingDAO.SelectTblLoadingByLoadingSlipId(loadingSlipId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblLoadingTO> SelectLoadingTOListWithDetails(string idLoadings)
        {
            try
            {
                string[] arrLoadingIds = null;
                List<TblLoadingTO> tblLoadingToList = new List<TblLoadingTO>();
                if (idLoadings.Contains(','))
                {
                    arrLoadingIds = idLoadings.Split(',');
                }
                else
                {
                    arrLoadingIds = new string[] { idLoadings };
                }
                foreach (string loadingId in arrLoadingIds)
                {
                    TblLoadingTO tblLoadingTO = SelectTblLoadingTO(Convert.ToInt32(loadingId));
                    tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(Convert.ToInt32(loadingId));
                    tblLoadingToList.Add(tblLoadingTO);
                }

                return tblLoadingToList;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public TblLoadingTO SelectLoadingTOWithDetails(Int32 idLoading)
        {
            try
            {
                TblLoadingTO tblLoadingTO = SelectTblLoadingTO(idLoading);
                tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(idLoading);
                return tblLoadingTO;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// Saket [2018-04-25] Added.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public TblLoadingTO SelectLoadingTOWithDetailsByInvoiceId(Int32 invoiceId)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblLoadingTO tblLoadingTO = null;

                //List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();

                List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = TempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId, conn, tran);

                for (int i = 0; i < tempLoadingSlipInvoiceTOList.Count; i++)
                {
                    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = tempLoadingSlipInvoiceTOList[i];

                    TblLoadingSlipTO tblLoadingSlipTOTemp = TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tempLoadingSlipInvoiceTO.LoadingSlipId, conn, tran);

                    if (tblLoadingTO == null)
                    {
                        tblLoadingTO = TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTOTemp.LoadingId, conn, tran);
                        if (tblLoadingTO == null)
                        {
                            return new TblLoadingTO();
                        }

                        if (tblLoadingTO.LoadingSlipList == null)
                        {
                            tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();
                        }
                        tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTOTemp);

                    }
                    else
                    {
                        if (tblLoadingTO.LoadingSlipList == null)
                        {
                            tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();
                        }

                        tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTOTemp);

                    }

                }


                return tblLoadingTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblLoadingTO SelectLoadingTOWithDetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            try
            {
                TblLoadingTO tblLoadingTO = SelectTblLoadingTOByLoadingSlipId(loadingSlipId);
                tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading);
                return tblLoadingTO;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        //Priyanka [25-07-2018]
        public TblLoadingTO SelectTblLoadingByLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
                return TblLoadingDAO.SelectTblLoadingByLoadingSlipId(loadingSlipId, conn, tran);

        }


        public List<VehicleNumber> SelectAllVehicles()
        {
            return TblLoadingDAO.SelectAllVehicles();
        }
        public List<DropDownTO> SelectAllVehiclesByStatus(int statusId)
        {
            return TblLoadingDAO.SelectAllVehiclesListByStatus(statusId);
        }
        public ODLMWebAPI.DashboardModels.LoadingInfo SelectDashboardLoadingInfo(List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, DateTime sysDate, Int32 loadingType)
        {
            try
            {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                Boolean isPriorityOther = true;
                if (tblUserRoleTOList!=null && tblUserRoleTOList.Count >0)
                {
                     tblUserRoleTO = BL.TblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
                     isPriorityOther = BL.TblUserRoleBL.selectRolePriorityForOther(tblUserRoleTOList);
                }
                return TblLoadingDAO.SelectDashboardLoadingInfo(tblUserRoleTO, orgId, sysDate, loadingType, isPriorityOther);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime loadingDate)
        {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, loadingDate);
        }

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, isAllowNxtLoading, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, isAllowNxtLoading, conn, tran);
        }


        public List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(string vehicleNo, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut(vehicleNo, conn, tran);
        }

        public List<TblLoadingTO> SelectAllInLoadingListByVehicleNo(string vehicleNo)
        {
            return TblLoadingDAO.SelectAllInLoadingListByVehicleNo(vehicleNo);
        }

        public Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisorDCT(DateTime date)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblLoadingDAO.SelectCountOfLoadingsOfSuperwisor(date, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblLoadingTO> SelectAllTblLoading(int cnfId, String loadingStatusIdIn, DateTime loadingDate)
        {
            return TblLoadingDAO.SelectAllTblLoading(cnfId, loadingStatusIdIn, loadingDate);
        }

        // Vaibhav [08-Jan-2018] Added to select all temp loading details.
        public List<TblLoadingTO> SelectAllTempLoading(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_MIGRATE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null)
            {
                resultMessage.DefaultBehaviour("Error tblConfigParamsTO is null");
                return null;
            }

            DateTime statusDate = Constants.ServerDateTime.AddDays(-Convert.ToInt32(tblConfigParamsTO.ConfigParamVal));
            //DateTime statusDate = Constants.ServerDateTime.AddDays(-25);

            try
            {
                return TblLoadingDAO.SelectAllTempLoading(conn, tran, statusDate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTempLoading");
                return null;
            }
        }
        //Pandurang[2018-09-25] Added to select all temp loading on status details.
        public List<TblLoadingTO> SelectAllTempLoadingOnStatus(SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELETE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null)
            {
                resultMessage.DefaultBehaviour("Error tblConfigParamsTO is null");
                return null;
            }

            DateTime statusDate = Constants.ServerDateTime.AddDays(-Convert.ToInt32(tblConfigParamsTO.ConfigParamVal));
            //DateTime statusDate = Constants.ServerDateTime.AddDays(-25);

            try
            {
                return TblLoadingDAO.SelectAllTempLoadingOnStatus(conn, tran, statusDate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTempLoading");
                return null;
            }
        }

        //Vijaymala [12-04-2018] added to get all loading list by vehicle number
        public List<TblLoadingTO> SelectLoadingListByVehicleNo(string vehicleNo)
        {
            return TblLoadingDAO.SelectLoadingListByVehicleNo(vehicleNo);
        }
        /// <summary>
        /// Vijaymala[24-04-2018] added to get loading details by using booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public List<TblLoadingTO> SelectAllTblLoadingByBookingId(Int32 bookingId)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblLoadingSlipDtlTO> tblLoadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromBookingId(bookingId, conn, tran);
                if (tblLoadingSlipDtlTOList != null && tblLoadingSlipDtlTOList.Count > 0)
                {

                    for (int i = 0; i < tblLoadingSlipDtlTOList.Count; i++)
                    {

                        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipDtlTOList[i];

                        TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblLoadingSlipDtlTO.LoadingSlipId, conn, tran);

                        if (tblLoadingSlipTO != null)
                        {

                            TblLoadingTO tblLoadingTO = TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId, conn, tran);

                            TblLoadingTO tblLoadingTOAlready = tblLoadingTOList.Where(w => w.IdLoading == tblLoadingTO.IdLoading).FirstOrDefault();

                            if (tblLoadingTOAlready != null)
                            {
                                if (tblLoadingTOAlready.LoadingSlipList == null)
                                {
                                    tblLoadingTOAlready.LoadingSlipList = new List<TblLoadingSlipTO>();
                                }
                                tblLoadingTOAlready.LoadingSlipList.Add(tblLoadingSlipTO);
                            }
                            else
                            {

                                if (tblLoadingTO.LoadingSlipList == null)
                                {
                                    tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();
                                }
                                tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);
                                tblLoadingTOList.Add(tblLoadingTO);
                            }

                        }

                    }
                }
                return tblLoadingTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }


        }

        /// <summary>
        /// Vijaymala[19-09-2018] added to get loading slip list by using booking
        /// </summary>
        /// <param name="idBooking"></param>
        /// <returns></returns>

        public TblLoadingTO SelectLoadingTOWithDetailsByBooking(String tempBookingsIdsList,String tempScheduleIdsList)
        {
            try
            {
                TblLoadingTO tblLoadingTO = new TblLoadingTO();
                List<Int32> bookingsIdsList=new List<Int32>();
                List<Int32> scheduleIdsList = new List<Int32>();
                List<TblBookingExtTO> tblAllBookingExtTOList = new List<TblBookingExtTO>();
                //get bookingIds List
                if(!String.IsNullOrEmpty(tempBookingsIdsList))
                {
                    bookingsIdsList = tempBookingsIdsList.Split(',').Select(int.Parse).ToList();
                }
                //get ScheduleIds List
                if(!String.IsNullOrEmpty(tempScheduleIdsList ))
                {
                    scheduleIdsList= tempScheduleIdsList.Split(',').Select(int.Parse).ToList();
                }
                TblBookingsTO tblBookingTO = new TblBookingsTO();
                if (bookingsIdsList != null && bookingsIdsList.Count > 0)
                {

                    for (int s = 0; s < bookingsIdsList.Count; s++)
                    {
                        Int32 bookingId = bookingsIdsList[s];
                        tblBookingTO = BL.TblBookingsBL.SelectBookingsTOWithDetails(bookingId);
                        if (tblBookingTO != null)
                        {
                            tblLoadingTO.VehicleNo = tblBookingTO.VehicleNo;
                            tblLoadingTO.FreightAmt = tblBookingTO.FreightAmt;
                            tblLoadingTO.CnfOrgId = tblBookingTO.CnFOrgId;
                            tblLoadingTO.CnfOrgName = tblBookingTO.CnfName;
                           
                            tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();
                             if (tblBookingTO.BookingScheduleTOLst != null && tblBookingTO.BookingScheduleTOLst.Count > 0)
                            {
                                List<TblBookingScheduleTO> temptblBookingScheduleTOList = tblBookingTO.BookingScheduleTOLst;
                                List<TblBookingScheduleTO> tblBookingScheduleTOList = new List<TblBookingScheduleTO>();

                                //get schedule list
                                if (scheduleIdsList!=null && scheduleIdsList.Count > 0)
                                {
                                    for(int p=0;p< scheduleIdsList.Count;p++)
                                    {
                                        Int32 scheduleId = scheduleIdsList[p];
                                        TblBookingScheduleTO tempTblBookingScheduleTO = new TblBookingScheduleTO();
                                        tempTblBookingScheduleTO = temptblBookingScheduleTOList.Where(c => c.IdSchedule == scheduleId).FirstOrDefault();
                                        tblBookingScheduleTOList.Add(tempTblBookingScheduleTO);
                                    }
                                }
                                //get all extensionlist
                                if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0)
                                {
                                    for (int i = 0; i < tblBookingScheduleTOList.Count; i++)
                                    {

                                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[i];
                                        tblAllBookingExtTOList.AddRange(tblBookingScheduleTO.OrderDetailsLst);
                                    }
                                }
                                //get distinct schedule list
                                List<TblBookingScheduleTO> distinctBookingScheduleList= tblBookingScheduleTOList.GroupBy(w => w.LoadingLayerId).Select(x => x.FirstOrDefault()).ToList();
                                if (distinctBookingScheduleList != null && distinctBookingScheduleList.Count > 0)
                                {
                                    for (int m = 0; m < distinctBookingScheduleList.Count; m++)
                                    {
                                        TblBookingScheduleTO tempBookingScheduleTO = distinctBookingScheduleList[m];
                                        tempBookingScheduleTO.OrderDetailsLst = new List<TblBookingExtTO>();
                                        List<TblBookingExtTO> distinctBookingExtList = tblAllBookingExtTOList.GroupBy(w => w.LoadingLayerId).Select(x => x.FirstOrDefault()).ToList();
                                        if (distinctBookingExtList != null && distinctBookingExtList.Count > 0)
                                        {
                                            for (int n = 0; n < distinctBookingExtList.Count; n++)
                                            {
                                                List<TblBookingExtTO> tempOrderList = tblAllBookingExtTOList.Where(oi => oi.LoadingLayerId == distinctBookingExtList[n].LoadingLayerId).ToList();
                                                for (int k = 0; k < tempOrderList.Count; k++)
                                                {
                                                    TblBookingExtTO tempBookingExtTO = tempOrderList[k];
                                                    if (tempBookingScheduleTO.LoadingLayerId == tempBookingExtTO.LoadingLayerId)
                                                    {
                                                        tempBookingScheduleTO.OrderDetailsLst.Add(tempBookingExtTO);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    Double bookQty = tblBookingTO.PendingQty;
                                   

                                    for (int p = 0; p < distinctBookingScheduleList.Count; p++)
                                    {
                                        TblBookingScheduleTO distBookingScheduleTO = distinctBookingScheduleList[p];
                                        var listToCheck= distBookingScheduleTO.OrderDetailsLst.GroupBy(a => new {  a.ProdSpecId, a.ProdCatId, a.MaterialId, a.ProdItemId,a.BrandId, a.ProdCatDesc, a.ProdSpecDesc, a.MaterialSubType, a.BrandDesc, a.DisplayName }).
                                            Select(a => new { ProdCatId = a.Key.ProdCatId, ProdItemId = a.Key.ProdItemId, ProdSpecId = a.Key.ProdSpecId, BrandId = a.Key.BrandId, MaterialId = a.Key.MaterialId, ProdCatDesc = a.Key.ProdCatDesc,
                                                ProdSpecDesc = a.Key.ProdSpecDesc,BrandDesc = a.Key.BrandDesc,MaterialSubType = a.Key.MaterialSubType,
                                                DisplayName = a.Key.DisplayName,BalanceQty = a.Sum(acs => acs.BalanceQty) }).ToList();

                                        distBookingScheduleTO.OrderDetailsLst = new List<TblBookingExtTO>();
                                        for (int l = 0; l < listToCheck.Count; l++)
                                        {
                                            var listTo = listToCheck[l];
                                            TblBookingExtTO tblBookingExtTO = new TblBookingExtTO();
                                            tblBookingExtTO.MaterialId = listTo.MaterialId;
                                            tblBookingExtTO.ProdCatId = listTo.ProdCatId;
                                            tblBookingExtTO.ProdSpecId = listTo.ProdSpecId;
                                            tblBookingExtTO.ProdItemId = listTo.ProdItemId;
                                            tblBookingExtTO.BrandId = listTo.BrandId;
                                            tblBookingExtTO.BookedQty = listTo.BalanceQty;
                                            tblBookingExtTO.BalanceQty = listTo.BalanceQty;
                                            tblBookingExtTO.MaterialSubType = listTo.MaterialSubType;
                                            tblBookingExtTO.ProdCatDesc = listTo.ProdCatDesc;
                                            tblBookingExtTO.ProdSpecDesc = listTo.ProdSpecDesc;
                                            tblBookingExtTO.BrandDesc = listTo.BrandDesc;
                                            tblBookingExtTO.DisplayName = listTo.DisplayName;
                                            distBookingScheduleTO.OrderDetailsLst.Add(tblBookingExtTO);
                                        }

                                        TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO(tblBookingTO);

                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;
                                        if (distBookingScheduleTO.DeliveryAddressLst != null && distBookingScheduleTO.DeliveryAddressLst.Count > 0)
                                        {
                                            List<TblBookingDelAddrTO> tblBookingDelAddrTOList = distBookingScheduleTO.DeliveryAddressLst;
                                                //.Where(ele => ele.ScheduleId == distBookingScheduleTO.IdSchedule).ToList();
                                            TblBookingDelAddrTO tblBookingDelAddrTO = new TblBookingDelAddrTO();
                                            for (int j = 0; j < tblBookingDelAddrTOList.Count; j++)
                                            {
                                                TblLoadingSlipAddressTO tblLoadingSlipAddressTO = new TblLoadingSlipAddressTO();
                                                tblBookingDelAddrTO = tblBookingDelAddrTOList[j];
                                                tblLoadingSlipAddressTO.BillingName = tblBookingDelAddrTO.BillingName;
                                                tblLoadingSlipAddressTO.GstNo = tblBookingDelAddrTO.GstNo;
                                                tblLoadingSlipAddressTO.PanNo = tblBookingDelAddrTO.PanNo;
                                                tblLoadingSlipAddressTO.AadharNo = tblBookingDelAddrTO.AadharNo;
                                                tblLoadingSlipAddressTO.ContactNo = tblBookingDelAddrTO.ContactNo;
                                                tblLoadingSlipAddressTO.Address = tblBookingDelAddrTO.Address;
                                                tblLoadingSlipAddressTO.VillageName = tblBookingDelAddrTO.VillageName;
                                                tblLoadingSlipAddressTO.TalukaName = tblBookingDelAddrTO.TalukaName;
                                                tblLoadingSlipAddressTO.DistrictName = tblBookingDelAddrTO.DistrictName;
                                                tblLoadingSlipAddressTO.StateId = tblBookingDelAddrTO.StateId;
                                                tblLoadingSlipAddressTO.State = tblBookingDelAddrTO.State;
                                                tblLoadingSlipAddressTO.Country = tblBookingDelAddrTO.Country;
                                                tblLoadingSlipAddressTO.Pincode = tblBookingDelAddrTO.Pincode.ToString();
                                                tblLoadingSlipAddressTO.TxnAddrTypeId = tblBookingDelAddrTO.TxnAddrTypeId;
                                                tblLoadingSlipAddressTO.AddrSourceTypeId = tblBookingDelAddrTO.AddrSourceTypeId;
                                                tblLoadingSlipAddressTO.LoadingLayerId = tblBookingDelAddrTO.LoadingLayerId;
                                                tblLoadingSlipAddressTO.BillingOrgId = tblBookingDelAddrTO.BillingOrgId;

                                                //tblLoadingSlipAddressTO.Country = tblBookingDelAddrTO.Country;
                                                tblLoadingSlipTO.DeliveryAddressTOList.Add(tblLoadingSlipAddressTO);
                                            }

                                        }
                                       
                                        if (distBookingScheduleTO.OrderDetailsLst != null && distBookingScheduleTO.OrderDetailsLst.Count > 0)
                                        {
                                            List<TblBookingExtTO> tblBookingExtTOList = distBookingScheduleTO.OrderDetailsLst;
                                                //.Where(ele => ele.ScheduleId == distBookingScheduleTO.IdSchedule).ToList();
                                            TblBookingExtTO tblBookingExtTO = new TblBookingExtTO();
                                            Double totLayerQty = 0;
                                            for (int k = 0; k < tblBookingExtTOList.Count; k++)
                                            {
                                                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                                 tblBookingExtTO = tblBookingExtTOList[k];
                                                tblLoadingSlipExtTO.BalanceQty = tblBookingExtTO.BalanceQty;
                                                tblLoadingSlipExtTO.LoadingQty = tblBookingExtTO.BalanceQty;
                                                tblLoadingSlipExtTO.MaterialId = tblBookingExtTO.MaterialId;
                                                tblLoadingSlipExtTO.ProdCatId = tblBookingExtTO.ProdCatId;
                                                tblLoadingSlipExtTO.ProdSpecId = tblBookingExtTO.ProdSpecId;
                                                tblLoadingSlipExtTO.BrandId = tblBookingExtTO.BrandId;
                                                tblLoadingSlipExtTO.ProdItemId = tblBookingExtTO.ProdItemId;
                                                tblLoadingSlipExtTO.DisplayName = tblBookingExtTO.DisplayName;
                                                totLayerQty += tblBookingExtTO.BalanceQty;
                                                // tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty += tblBookingExtTO.BalanceQty;
                                                tblLoadingSlipExtTO.LoadingLayerid = distBookingScheduleTO.LoadingLayerId;
                                                tblLoadingSlipExtTO.MaterialDesc = tblBookingExtTO.MaterialSubType;
                                                tblLoadingSlipExtTO.ProdCatDesc = tblBookingExtTO.ProdCatDesc;
                                                tblLoadingSlipExtTO.ProdSpecDesc = tblBookingExtTO.ProdSpecDesc;
                                                tblLoadingSlipExtTO.BrandDesc = tblBookingExtTO.BrandDesc;
                                                tblLoadingSlipExtTO.BookingId = tblBookingTO.IdBooking;
                                                tblLoadingSlipExtTO.LoadingLayerDesc = distBookingScheduleTO.LoadingLayerDesc;
                                                tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                                            }
                                            if (bookQty >= totLayerQty)
                                            {
                                                bookQty = bookQty - totLayerQty;
                                                
                                            }
                                            else
                                            {
                                                totLayerQty = bookQty;
                                                bookQty = 0;
                                            }
                                             tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty =totLayerQty;


                                        }
                                        else
                                        {
                                            //tblLoadingSlipTO.NoOfDeliveries = 1;
                                            TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                            tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                            tblLoadingSlipExtTO.LoadingLayerid = (int)Constants.LoadingLayerE.BOTTOM;
                                            tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);


                                        }
                                        tblLoadingTO.NoOfDeliveries = distinctBookingScheduleList.Count;
                                        tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);
                                    }


                                }

                            }
                            else
                            {
                                TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO(tblBookingTO);
                                tblLoadingTO.NoOfDeliveries = 1;
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;
                                tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                tblLoadingSlipExtTO.LoadingLayerid = (int)Constants.LoadingLayerE.BOTTOM;
                                tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                                tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);

                            }
                        }
                    }
                }
                return tblLoadingTO;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private static TblLoadingSlipTO selectLoadingSlipTO(TblBookingsTO tblBookingTO)
        {
            TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
            tblLoadingSlipTO.VehicleNo = tblBookingTO.VehicleNo;
            tblLoadingSlipTO.CnfOrgId = tblBookingTO.CnFOrgId;
            tblLoadingSlipTO.CnfOrgName = tblBookingTO.CnfName;
            tblLoadingSlipTO.DealerOrgId = tblBookingTO.DealerOrgId;
            tblLoadingSlipTO.DealerOrgName = tblBookingTO.DealerName;
            tblLoadingSlipTO.BookingId = tblBookingTO.IdBooking;
            tblLoadingSlipTO.CdStructureId = tblBookingTO.CdStructureId;
            tblLoadingSlipTO.CdStructure = tblBookingTO.CdStructure;
            tblLoadingSlipTO.IsConfirmed = tblBookingTO.IsConfirmed;
            tblLoadingSlipTO.DealerOrgId = tblBookingTO.DealerOrgId;
            tblLoadingSlipTO.DealerOrgName = tblBookingTO.DealerName;
            tblLoadingSlipTO.FreightAmt= tblBookingTO.FreightAmt;
            tblLoadingSlipTO.OrcAmt = tblBookingTO.OrcAmt;
            tblLoadingSlipTO.OrcMeasure = tblBookingTO.OrcMeasure;
            tblLoadingSlipTO.ORCPersonName = tblBookingTO.ORCPersonName;
            tblLoadingSlipTO.Comment = tblBookingTO.Comments;
            tblLoadingSlipTO.TblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();
            tblLoadingSlipTO.DeliveryAddressTOList = new List<TblLoadingSlipAddressTO>();
            tblLoadingSlipTO.LoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
            return tblLoadingSlipTO;
        }

        #endregion

        #region Insertion
        public int InsertTblLoading(TblLoadingTO tblLoadingTO)
        {
            return TblLoadingDAO.InsertTblLoading(tblLoadingTO);
        }

        public int InsertTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingDAO.InsertTblLoading(tblLoadingTO, conn, tran);
        }

        public ResultMessage CalculateLoadingValuesRate(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            //resultMessage.Tag = tblLoadingTO;
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                Double freightPerMT = 0;
                //Vijaymala added[26-04-2018]:commented that code to get freight from loading slip layerwise
                //if (tblLoadingTO.IsFreightIncluded == 1)
                //{
                //    freightPerMT = tblLoadingTO.FreightAmt;// CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                //    //freightPerMT = CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                //    if (freightPerMT < 0)
                //    {
                //        tran.Rollback();
                //        resultMessage.MessageType = ResultMessageE.Error;
                //        resultMessage.Text = "Error : Freight Calculations is less than 0. Please check the calculations immediatly";
                //        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                //        return resultMessage;
                //    }
                //}

                //Vijaymala[13-11-2018]commented the code .Tax inclusive/exclusive getting from brand
                //Sanjay [2018-07-04] Tax Calculations Inclusive Of Taxes Or Exclusive Of Taxes. Reported From Customer Shivangi Rolling Mills.By default it will be 0 i.e. Tax Exclusive
                Int32 isTaxInclusiveWithTaxes = 0;
                Boolean isSez = false;
                TblConfigParamsTO rateCalcConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_RATE_CALCULATIONS_TAX_INCLUSIVE, conn, tran);
                if (rateCalcConfigParamsTO != null)
                {
                    isTaxInclusiveWithTaxes = Convert.ToInt32(rateCalcConfigParamsTO.ConfigParamVal);
                }

                List<TblConfigParamsTO> tblConfigParamsTOList = BL.TblConfigParamsBL.SelectAllTblConfigParamsList();
                Boolean isRateRounded = false;
                TblConfigParamsTO roundRateConfig = new TblConfigParamsTO();
                if (tblConfigParamsTOList!=null && tblConfigParamsTOList.Count >0)
                {
                    roundRateConfig = tblConfigParamsTOList.Where(ele => ele.ConfigParamName == Constants.CP_IS_INVOICE_RATE_ROUNDED).FirstOrDefault();
                    if(roundRateConfig!=null && Convert.ToInt32(roundRateConfig.ConfigParamVal)==1 )
                    {
                        isRateRounded = true;
                    }
                }

                Double forAmtPerMT = 0; //Vijaymala added[22-06-2018]
                for (int i = 0; i < tblLoadingTO.LoadingSlipList.Count; i++)
                {
                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[i];

                    //Vijaymala added[26-04-2018]:to done calculation using  freight from loading slip 
                    if (tblLoadingSlipTO.IsFreightIncluded == 1)
                    {
                        freightPerMT = tblLoadingSlipTO.FreightAmt;// CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingSlipTO.FreightAmt);
                                                                   //freightPerMT = CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingSlipTO.FreightAmt);
                        //if (freightPerMT < 0)
                        //{
                        //    tran.Rollback();
                        //    resultMessage.MessageType = ResultMessageE.Error;
                        //    resultMessage.Text = "Error : Freight Calculations is less than 0. Please check the calculations immediatly";
                        //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        //    return resultMessage;
                        //}
                    }

                    //Vijaymala added[21-06-2018]for new For amount calculation
                    if (tblLoadingSlipTO.IsForAmountIncluded == 1)
                    {

                        if (tblLoadingSlipTO.ForAmount > 0)
                        {
                            forAmtPerMT = tblLoadingSlipTO.ForAmount;
                            freightPerMT = forAmtPerMT + freightPerMT;
                        }
                    }
                  //  freightPerMT = Math.Abs(freightPerMT);

                    if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                    {
                        if (tblLoadingTO.LoadingType == (int)Constants.LoadingTypeE.OTHER)
                        {

                        }
                        else
                        {

                            TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipTO.TblLoadingSlipDtlTO;
                            if(tblLoadingSlipDtlTO.IdBooking > 0)
                            {
                                tblLoadingSlipDtlTO.BookingId = tblLoadingSlipDtlTO.IdBooking;

                            }
                            TblBookingsTO tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);
                            if (tblBookingsTO == null)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }

                            if(tblBookingsTO.IsSez==1)
                            {
                                isSez = true;
                            }

                            String parityIds = String.Empty;
                            List<TblBookingParitiesTO> tblBookingParitiesTOList = TblBookingParitiesBL.SelectTblBookingParitiesTOListByBookingId(tblBookingsTO.IdBooking, conn, tran);


                            if (tblBookingParitiesTOList != null && tblBookingParitiesTOList.Count > 0)
                            {
                                parityIds = String.Join(",", tblBookingParitiesTOList.Select(s => s.ParityId.ToString()).ToArray());
                            }

                            if (String.IsNullOrEmpty(parityIds))
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : ParityTO Not Found";
                                resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                                return resultMessage;
                            }

                            //Sudhir[23-MARCH-2018] Commented For New Parity Logic.
                            // List<TblParityDetailsTO> parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(parityIds, 0, conn, tran);

                            for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++)
                            {
                              
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];

                                Int32 isTaxInclusive = 0;
                                DimBrandTO dimBrandTO = BL.DimBrandBL.SelectDimBrandTO(tblLoadingSlipExtTO.BrandId);
                               
                                if (dimBrandTO != null)
                                {
                                    isTaxInclusive = dimBrandTO.IsTaxInclusive;
                                }
                                if (tblLoadingSlipExtTO.LoadingQty > 0)
                                {
                                    
                                    #region Calculate Actual Price From Booking and Parity Settings

                                    Double orcAmtPerTon = 0;
                                    if (tblLoadingSlipTO.OrcMeasure == "Rs/MT")  //Need to change
                                    {
                                        orcAmtPerTon = tblLoadingSlipTO.OrcAmt;
                                    }
                                    else
                                    {
                                        if (tblLoadingSlipTO.OrcAmt > 0)
                                            orcAmtPerTon = tblLoadingSlipTO.OrcAmt / tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;
                                    }

                                    //String rateCalcDesc = string.Empty;
                                    //rateCalcDesc = "B.R : " + tblBookingsTO.BookingRate + "|";
                                    //Double bookingPrice = tblBookingsTO.BookingRate;

                                    TblBookingParitiesTO tblBookingParitiesTO = tblBookingParitiesTOList.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();
                                    if (tblBookingParitiesTO == null || tblBookingParitiesTO.BookingRate == 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.Text = "Error : Rate not found against brand - " + tblLoadingSlipExtTO.BrandDesc;
                                        resultMessage.DisplayMessage = "Error : Rate not found against brand - " + tblLoadingSlipExtTO.BrandDesc;
                                        return resultMessage;
                                    }


                                    String rateCalcDesc = string.Empty;
                                    Double bookingPrice = tblBookingParitiesTO.BookingRate;

                                   // rateCalcDesc = "B.R : " + tblBookingParitiesTO.BookingRate + "|";
                                   if(isTaxInclusive==1 && isTaxInclusiveWithTaxes==0)
                                    {
                                        bookingPrice = bookingPrice / 1.18;
                                        bookingPrice = Math.Round(bookingPrice, 2);
                                    }
                                    rateCalcDesc = "B.R : " + bookingPrice + "|";
                                    Double parityAmt = 0;
                                    Double priceSetOff = 0;
                                    Double paritySettingAmt = 0;
                                    Double bvcAmt = 0;
                                    //TblParitySummaryTO parityTO = null; Sudhir[23-MARCH-2018] Commented Code
                                    TblParityDetailsTO parityDtlTO = null;
                                    if (true)
                                    {
                                        //Sudhir[23-MARCH-2018] Commented for New Parity Logic.
                                        /*var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                                && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                 && m.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();*/

                                        //Get Latest To Based On -materialId, Date And Time Check Condition Actual TIme < = First Object.
                                        TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                                        parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, addrTO.StateId, tblBookingsTO.BookingDatetime);
                                        if (parityDtlTO != null)
                                        {
                                            parityAmt = parityDtlTO.ParityAmt;
                                            if (tblLoadingSlipTO.IsConfirmed != 1)
                                                priceSetOff = parityDtlTO.NonConfParityAmt;
                                            else
                                                priceSetOff = 0;

                                            tblLoadingSlipExtTO.ParityDtlId = parityDtlTO.IdParityDtl;
                                        }
                                        else
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.Text = "Error : ParityTO Not Found";
                                            string mateDesc = tblLoadingSlipExtTO.DisplayName;
                                            //[05-09-2018] : Vijaymala commented code to set display  name for item for other and regular
                                            //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                            resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                            return resultMessage;
                                        }

                                        #region Sudhir[23-MARCH-2018] Commented Code for New PArity Logic
                                        //parityTO = BL.TblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                                        //if (parityTO == null)
                                        //{
                                        //    tran.Rollback();
                                        //    resultMessage.DefaultBehaviour();
                                        //    resultMessage.Text = "Error : ParityTO Not Found";
                                        //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        //    return resultMessage;
                                        //}

                                        //paritySettingAmt = parityTO.BaseValCorAmt + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                        //bvcAmt = parityTO.BaseValCorAmt;
                                        //rateCalcDesc += "BVC Amt :" + parityTO.BaseValCorAmt + "|" + "Exp Amt :" + parityTO.ExpenseAmt + "|" + " Other :" + parityTO.OtherAmt + "|";

                                        #endregion

                                        //[23-MARCH-2018] Added For New Parity Setting Logic
                                        paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                                        bvcAmt = parityDtlTO.BaseValCorAmt;
                                        rateCalcDesc += "BVC Amt :" + parityDtlTO.BaseValCorAmt + "|" + "Exp Amt :" + parityDtlTO.ExpenseAmt + "|" + " Other :" + parityDtlTO.OtherAmt + "|";
                                    }
                                    else
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.Text = "Error : ParityTO Not Found";
                                        resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                                        return resultMessage;
                                    }

                                    Double cdApplicableAmt = 0;
                                    cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                                    //if (tblLoadingSlipTO.IsConfirmed == 1)
                                    //    cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt;

                                    if (tblLoadingSlipTO.IsConfirmed == 1)
                                        cdApplicableAmt += parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                                    Double cdAmt = 0;
                                    Double orgCdAmt = 0;
                                    //Vijaymala added[22-06-2018]
                                    DropDownTO dropDownTO = BL.DimensionBL.SelectCDDropDown(tblLoadingSlipTO.CdStructureId);

                                    //Priyanka [23-07-2018] Added if cdstructure is 0
                                    if(tblLoadingSlipTO.CdStructure >= 0)
                                    {
                                        Int32 isRsValue = Convert.ToInt32(dropDownTO.Text);
                                        if (isRsValue ==(int)Constants.CdType.IsRs)
                                        {

                                            orgCdAmt=cdAmt = tblLoadingSlipTO.CdStructure;
                                            cdAmt = cdAmt + tblLoadingSlipTO.AddDiscAmt;        //Priyanka [09-07-18]
                                        }
                                        else
                                        {

                                            orgCdAmt=cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;
                                            cdAmt = cdAmt + tblLoadingSlipTO.AddDiscAmt;        //Priyanka [09-07-18]
                                        }
                                    }                                    
                                   

                                    rateCalcDesc += "CD :" + Math.Round(cdAmt, 2) + "|";
                                    Double basicRateTaxIncl = cdApplicableAmt  -cdAmt + freightPerMT;
                                    Double rateAfterCD = cdApplicableAmt - cdAmt;

                                    Double gstApplicableAmt = 0;
                                    Double gstAmt = 0;
                                    Double finalRate = 0;

                                    TblGstCodeDtlsTO gstCodeDtlsTO = BL.TblGstCodeDtlsBL.SelectGstCodeDtlsTO(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdItemId, conn, tran);
                                    if (gstCodeDtlsTO == null)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.Text = "Error : GST Code Not Found";
                                        string mateDesc = tblLoadingSlipExtTO.DisplayName;
                                        //[05-09-2018] : Vijaymala commented code to set display  name for item for other and regular
                                        //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                        resultMessage.DisplayMessage = "Warning : GST Code Is Not Defined For " + mateDesc + " Please contact BackOffice";
                                        return resultMessage;
                                    }

                                    if (isTaxInclusiveWithTaxes == 0 || isTaxInclusive == 0)
                                    {
                                        if (tblLoadingSlipTO.IsConfirmed == 1)
                                            //gstApplicableAmt = rateAfterCD + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                            gstApplicableAmt = rateAfterCD + freightPerMT;
                                        else
                                            gstApplicableAmt = rateAfterCD;
                                        if (isSez)
                                        {
                                            gstCodeDtlsTO.TaxPct = 0;
                                        }

                                        gstAmt = (gstApplicableAmt * gstCodeDtlsTO.TaxPct) / 100;
                                        gstAmt = Math.Round(gstAmt, 2);

                                        if (tblLoadingSlipTO.IsConfirmed == 1)
                                            finalRate = gstApplicableAmt + gstAmt;
                                        //else
                                        //finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityTO.ExpenskeAmt + parityTO.OtherAmt; Sudhir[23-MARCH-2018] Commented
                                        else
                                            finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                                    }
                                    else
                                    {
                                        if (isSez)
                                        {
                                            gstCodeDtlsTO.TaxPct = 0;
                                        }
                                        Double taxToDivide = 100 + gstCodeDtlsTO.TaxPct;

                                        gstAmt = basicRateTaxIncl - ((basicRateTaxIncl / taxToDivide) * 100);
                                        gstAmt = Math.Round(gstAmt, 2);

                                        gstApplicableAmt = basicRateTaxIncl - gstAmt;
                                        finalRate = basicRateTaxIncl;
                                        cdApplicableAmt = gstApplicableAmt + cdAmt;
                                    }


                                    tblLoadingSlipExtTO.TaxableRateMT = gstApplicableAmt;
                                    tblLoadingSlipExtTO.RatePerMT = finalRate;
                                    if(isRateRounded)
                                    {
                                        cdApplicableAmt= Math.Round(cdApplicableAmt);
                                    }
                                    tblLoadingSlipExtTO.CdApplicableAmt = cdApplicableAmt;
                                    //tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt; Sudhir[23-MARCH-2018] Commented
                                    tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;



                                    TblConfigParamsTO tblConfigParamsTempTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_HIDE_NOT_CONFIRM_OPTION);

                                    Int32 isHideCorNC = 0;
                                    if (tblConfigParamsTempTO != null)
                                    {
                                        isHideCorNC = Convert.ToInt32(tblConfigParamsTempTO.ConfigParamVal);
                                    }

                                    string isNCAmt = string.Empty;
                                    if (isHideCorNC == 0)
                                    {
                                        isNCAmt = " NC Amt :" + priceSetOff + "|";
                                    }
                                    rateCalcDesc += " ORC :" + orcAmtPerTon + "|" + " Parity :" + parityAmt + "|" + isNCAmt + " Freight :" + freightPerMT + "|" + " GST :" + gstAmt + "|";
                                    tblLoadingSlipExtTO.RateCalcDesc = rateCalcDesc;
                                    #endregion


                                }
                            }
                        }
                    }
                }

                //tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;

                resultMessage.Text = "Calculated Successfully";
                resultMessage.DisplayMessage = "Calculated Successfully";

                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;


            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method CalculateLoadingValuesRate";
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public void CalculateActualPriceInclusiveOfTaxes()
        {

        }

        /// <summary>
        /// Priyanka [11-05-2018] : Added for convert NC to C loading slip.
        /// </summary>
        /// <returns></returns>
        public ResultMessage UpdateNCToCLoadingSlip(Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            String erroMsg = String.Empty;
            DateTime txnDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
               
                List<TblLoadingTO> tblLoadingTOList = BL.TblLoadingBL.SelectAllTblLoadingListForConvertNCToC();
                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingTOList.Count; i++)
                    {
                        TblLoadingTO tblLoadingTO = tblLoadingTOList[i];
                        List<TblLoadingSlipTO> tblLoadingSlipTOList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                        if(tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0)
                        {
                            List<TblLoadingSlipTO> tblNCLoadingSlipTOList = tblLoadingSlipTOList.Where(t => t.IsConfirmed == 0).ToList();
                            if(tblNCLoadingSlipTOList != null && tblNCLoadingSlipTOList.Count > 0)
                            {
                                //If loading slip in confirm then change the status.
                                for (int t = 0; t < tblNCLoadingSlipTOList.Count; t++)
                                {
                                    TblLoadingSlipTO tblLoadingSlipTO = tblNCLoadingSlipTOList[t];
                                    List<TblInvoiceTO> tblInvoiceTOList = BL.TblInvoiceBL.SelectInvoiceTOListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip);

                                    if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                                    {
                                        List<TblInvoiceTO> tblInvoiceTOListInvoiceNoNull = tblInvoiceTOList.Where(e => e.InvoiceNo == null && e.IsConfirmed == 0).ToList();

                                        if (tblInvoiceTOListInvoiceNoNull != null && tblInvoiceTOListInvoiceNoNull.Count > 0)
                                        {
                                            for (int p = 0; p < tblInvoiceTOListInvoiceNoNull.Count; p++)
                                            {
                                                TblInvoiceTO tblInvoiceTONew = tblInvoiceTOListInvoiceNoNull[p];
                                                if (tblInvoiceTONew.IsConfirmed == 0)
                                                {
                                                    tblInvoiceTONew.UpdatedBy = Convert.ToInt32(loginUserId);
                                                    tblInvoiceTONew.UpdatedOn = txnDateTime;
                                                    tblInvoiceTONew.IsConfirmed = 1;
                                                    resultMessage = BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTONew, loginUserId);
                                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                                    {
                                                        erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            resultMessage = BL.TblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(tblLoadingSlipTO, loginUserId);
                                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                            {
                                                erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        resultMessage = BL.TblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(tblLoadingSlipTO, loginUserId);
                                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                        {
                                            erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // If loading slip is confirm and its invoice is not confirm then change the status.
                                for (int t = 0; t < tblLoadingSlipTOList.Count; t++)
                                {
                                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingSlipTOList[t];
                                    List<TblInvoiceTO> tblInvoiceTOList = BL.TblInvoiceBL.SelectInvoiceTOListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip);
                                    if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                                    {
                                        List<TblInvoiceTO> tblInvoiceTOListInvoiceNoNull = tblInvoiceTOList.Where(e => e.InvoiceNo == null && e.IsConfirmed == 0).ToList();

                                        if (tblInvoiceTOListInvoiceNoNull != null && tblInvoiceTOListInvoiceNoNull.Count > 0)
                                        {
                                            for (int p = 0; p < tblInvoiceTOListInvoiceNoNull.Count; p++)
                                            {
                                                
                                                TblInvoiceTO tblInvoiceTONew = tblInvoiceTOListInvoiceNoNull[p];
                                                if(tblInvoiceTONew.IsConfirmed == 0)
                                                {
                                                    tblInvoiceTONew.UpdatedBy = Convert.ToInt32(loginUserId);
                                                    tblInvoiceTONew.UpdatedOn = txnDateTime;
                                                    tblInvoiceTONew.IsConfirmed = 1;
                                                    resultMessage = BL.TblInvoiceBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTONew, loginUserId);
                                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                                    {
                                                        erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                                    }
                                                }
                                               
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                            }

                        }

                    }

                    resultMessage.DefaultSuccessBehaviour();

                    if (!String.IsNullOrEmpty(erroMsg))
                    {
                        erroMsg = erroMsg.TrimEnd(',');
                        resultMessage.DisplayMessage += "\n Error While Updating - " + erroMsg;

                    }
                    
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblLoadingTOList Found NULL");
                    return resultMessage;
                }
               
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateNCToCLoadingSlip");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage SaveNewLoadingSlip(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save New Main Loading Slip

                //Int64 earlierCount = TblLoadingDAO.SelectCountOfLoadingSlips(tblLoadingTO.CreatedOn,conn, tran);
                //earlierCount++;
                //String loadingSlipNo = tblLoadingTO.CreatedOn.Year +""+ tblLoadingTO.CreatedOn.Month+"" + tblLoadingTO.CreatedOn.Day + "/" + earlierCount;

                // Vaibhav [30-Jan-2018] Commented and added to generate loading count.
                TblEntityRangeTO loadingEntityRangeTO = SelectEntityRangeForLoadingCount(Constants.ENTITY_RANGE_LOADING_COUNT, conn, tran);
                if (loadingEntityRangeTO == null)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : loadingEntityRangeTO is null";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                String loadingSlipNo = tblLoadingTO.CreatedOn.Day + "" + tblLoadingTO.CreatedOn.Month + "" + tblLoadingTO.CreatedOn.Year + "/" + loadingEntityRangeTO.EntityPrevValue;

                loadingEntityRangeTO.EntityPrevValue++;
                result = BL.TblEntityRangeBL.UpdateTblEntityRange(loadingEntityRangeTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : While UpdateTblEntityRange";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }


                //Vijaymala [2018-06-20] Added 
                Int32 isAutoGateInVehicle = 0;
                TblConfigParamsTO tblConfigParamsTOAutoGateIn = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_AUTO_GATE_IN_VEHICLE, conn, tran);
                if (tblConfigParamsTOAutoGateIn != null)
                {
                    isAutoGateInVehicle = Convert.ToInt32(tblConfigParamsTOAutoGateIn.ConfigParamVal);
                }

                Boolean isBoyondLoadingQuota = false;
                Double finalLoadQty = 0;
                tblLoadingTO.LoadingSlipNo = loadingSlipNo;
                //Vijaymala added[22-06-2018]
                if (isAutoGateInVehicle==1)
                {
                    tblLoadingTO.StatusId= (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                    tblLoadingTO.StatusReason = "Vehicle Entered In The Premises"; 
                }
                else
                {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                    tblLoadingTO.StatusReason = "Loading Scheduled";
                }

                //Vijaymala added[22-06-2018]
                tblLoadingTO.StatusDate = StaticStuff.Constants.ServerDateTime;
                tblLoadingTO.CreatedOn = Constants.ServerDateTime;
                result = InsertTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error in InsertTblLoading";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                #endregion

                #region 2. Save Individual Loading Slips and Its Qty Details

                if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : LoadingSlipList Found Empty Or Null";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                //Vijaymala added[26-04-2018]:commented that code to get freight from loading slip layerwise

                Double freightPerMT = 0;

                //Vijaymala added[26-04-2018]:commented that code to get freight from loading slip layerwise
                //if (tblLoadingTO.IsFreightIncluded == 1)
                //{
                //    freightPerMT = tblLoadingTO.FreightAmt;// CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                //    //freightPerMT = CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                //    if(freightPerMT < 0)
                //    {
                //        tran.Rollback();
                //        resultMessage.MessageType = ResultMessageE.Error;
                //        resultMessage.Text = "Error : Freight Calculations is less than 0. Please check the calculations immediatly";
                //        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                //        return resultMessage;
                //    }
                //}

                #region Splitting of loadingslip itemwise

                //Saket [2018-02-13] Added 

                Int32 isBrandWiseLoading = 1;

                TblConfigParamsTO tblConfigParamsTOBrandWise = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BRAND_WISE_INVOICE, conn, tran);
                if (tblConfigParamsTOBrandWise != null)
                {
                    isBrandWiseLoading = Convert.ToInt32(tblConfigParamsTOBrandWise.ConfigParamVal);
                }

                if (isBrandWiseLoading == 1)
                {
                    if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0)
                    {
                        List<TblLoadingSlipTO> splitLoadingSlipList = new List<TblLoadingSlipTO>();

                        for (int i = 0; i < tblLoadingTO.LoadingSlipList.Count; i++)
                        {
                            TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[i];
                            if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                            {

                                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTO.LoadingSlipExtTOList;

                                List<TblLoadingSlipExtTO> distinctBrandList = tblLoadingSlipExtTOList.GroupBy(w => w.BrandId).Select(s => s.FirstOrDefault()).ToList();

                                if (distinctBrandList != null && distinctBrandList.Count > 1)  //Greater than 1 condtion if brand is distinct then only separate the loading slip.
                                {
                                    for (int k = 0; k < distinctBrandList.Count; k++)
                                    {
                                        TblLoadingSlipExtTO tblLoadingSlipExtTOBrand = distinctBrandList[k];
                                        if (k == 0)
                                        {
                                            tblLoadingSlipTO.LoadingSlipExtTOList = tblLoadingSlipExtTOList.Where(w => w.BrandId == tblLoadingSlipExtTOBrand.BrandId).ToList();
                                        }
                                        else
                                        {

                                            TblLoadingSlipTO tblLoadingSlipTOTemp = tblLoadingSlipTO.DeepCopy(); // Create Clone
                                            tblLoadingSlipTOTemp.LoadingSlipExtTOList = tblLoadingSlipExtTOList.Where(w => w.BrandId == tblLoadingSlipExtTOBrand.BrandId).ToList();
                                            splitLoadingSlipList.Add(tblLoadingSlipTOTemp);
                                        }
                                    }
                                }


                            }
                        }

                        tblLoadingTO.LoadingSlipList.AddRange(splitLoadingSlipList);
                    }

                }

                #endregion

                for (int i = 0; i < tblLoadingTO.LoadingSlipList.Count; i++)
                {
                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[i];
                    tblLoadingSlipTO.LoadingId = tblLoadingTO.IdLoading;
                    tblLoadingSlipTO.VehicleNo = tblLoadingTO.VehicleNo;
                    //Vijaymala added[22-06-2018]
                    if (isAutoGateInVehicle == 1)
                    {
                        tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                        tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;

                    }
                    else
                    {
                        tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;

                    }
                    tblLoadingSlipTO.CreatedBy = tblLoadingTO.CreatedBy;
                    tblLoadingSlipTO.CreatedOn = tblLoadingTO.CreatedOn;
                    tblLoadingSlipTO.NoOfDeliveries = tblLoadingTO.NoOfDeliveries;
                    tblLoadingSlipTO.StatusDate = tblLoadingTO.StatusDate;
                    tblLoadingSlipTO.StatusReason = tblLoadingTO.StatusReason;
                    tblLoadingSlipTO.ContactNo = tblLoadingTO.ContactNo;
                    tblLoadingSlipTO.DriverName = tblLoadingTO.DriverName;
                    tblLoadingSlipTO.AddDiscAmt = tblLoadingSlipTO.AddDiscAmt;
                    //tblLoadingSlipTO.OrcAmt = tblLoadingTO.OrcAmt;


                    //Vijaymala added[26-04-2018]:to done calculation using  freight from loading slip 
                    if (tblLoadingSlipTO.IsFreightIncluded == 1)
                    {
                        freightPerMT = tblLoadingSlipTO.FreightAmt;// CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                                                                   //freightPerMT = CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
                        //if (freightPerMT < 0)
                        //{
                        //    tran.Rollback();
                        //    resultMessage.MessageType = ResultMessageE.Error;
                        //    resultMessage.Text = "Error : Freight Calculations is less than 0. Please check the calculations immediatly";
                        //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        //    return resultMessage;
                        //}
                    }

                    Int64 slipCnt = TblLoadingSlipDAO.SelectCountOfLoadingSlips(tblLoadingTO.CreatedOn, tblLoadingSlipTO.IsConfirmed, conn, tran);
                    slipCnt++;
                    String slipNo = string.Empty;
                    if (tblLoadingSlipTO.IsConfirmed == 1)
                    {
                        //slipNo = tblLoadingTO.CreatedOn.Year.ToString() + "" + tblLoadingTO.CreatedOn.Month.ToString() + "" + tblLoadingTO.CreatedOn.Day.ToString() + "/" + slipCnt;

                        // Vaibhav [30-Jan-2018] Commented and added to generate confirm loading slip count.
                        TblEntityRangeTO entityRangeTO = SelectEntityRangeForLoadingCount(Constants.ENTITY_RANGE_C_LOADINGSLIP_COUNT, conn, tran);
                        if (entityRangeTO == null)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : entityRangeTO is null";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        slipNo = tblLoadingTO.CreatedOn.Day.ToString() + "" + tblLoadingTO.CreatedOn.Month.ToString() + "" + tblLoadingTO.CreatedOn.Year.ToString()+ "/" + entityRangeTO.EntityPrevValue;

                        entityRangeTO.EntityPrevValue++;
                        result = BL.TblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While UpdateTblEntityRange";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                    }
                    else
                    {
                        //slipNo = tblLoadingTO.CreatedOn.Year.ToString() + "" + tblLoadingTO.CreatedOn.Month.ToString() + "" + tblLoadingTO.CreatedOn.Day.ToString() + "" + "NC/" + slipCnt;

                        // Vaibhav [10-Jan-2018] Commented and added to generate nc loading slip count.
                        TblEntityRangeTO entityRangeTO = SelectEntityRangeForLoadingCount(Constants.ENTITY_RANGE_NC_LOADINGSLIP_COUNT, conn, tran);
                        if (entityRangeTO == null)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : entityRangeTO is null";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        slipNo = tblLoadingTO.CreatedOn.Day.ToString() + ""+ tblLoadingTO.CreatedOn.Month.ToString() + "" + tblLoadingTO.CreatedOn.Year.ToString() + "" +  "NC/" + entityRangeTO.EntityPrevValue;

                        entityRangeTO.EntityPrevValue++;
                        result = BL.TblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While UpdateTblEntityRange";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                    }
                    tblLoadingSlipTO.LoadingSlipNo = slipNo;
                    result = BL.TblLoadingSlipBL.InsertTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : While inserting into InsertTblLoadingSlip";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }


                    #region Loading Slip Order And Qty Details

                    TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();

                    List<TblBookingExtTO> tblBookingExtTOList = new List<TblBookingExtTO>();

                    if (tblLoadingTO.LoadingType != (int)Constants.LoadingTypeE.OTHER)
                    {
                        if (tblLoadingSlipTO.TblLoadingSlipDtlTO == null)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : LoadingSlipDtlTOList Found Empty Or Null";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }


                        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipTO.TblLoadingSlipDtlTO;
                        tblLoadingSlipDtlTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;
                        tblLoadingSlipDtlTO.BookingId = tblLoadingSlipDtlTO.IdBooking;
                        result = TblLoadingSlipDtlBL.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While inserting into InsertTblLoadingSlipDtl";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        finalLoadQty += tblLoadingSlipDtlTO.LoadingQty;

                        //Call to update pending booking qty for loading

                        tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);
                        if (tblBookingsTO == null)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        if (tblBookingsTO.DealerOrgId != tblLoadingSlipTO.DealerOrgId)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : Loading Slip Dealer and Respective Booking Dealer Not Matches";
                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Dealer Info from booking and Loading not matches";
                            return resultMessage;
                        }

                        tblBookingsTO.IdBooking = tblLoadingSlipDtlTO.BookingId;
                        tblBookingsTO.PendingQty = tblBookingsTO.PendingQty - tblLoadingSlipDtlTO.LoadingQty;
                        tblBookingsTO.UpdatedBy = tblLoadingSlipTO.CreatedBy;
                        tblBookingsTO.UpdatedOn = Constants.ServerDateTime;

                        if (tblBookingsTO.PendingQty < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Pending Qty Of Selected Booking #" + tblBookingsTO.IdBooking + " is less then loading Qty" + Environment.NewLine + " Please recreate the loading slip";
                            return resultMessage;
                        }

                        //Check for Weight Tolerance . If pending Qty is within weight tolerance then mark the booking status as closed.
                        TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_WEIGHT_TOLERANCE_IN_KGS, conn, tran);
                        if (tblConfigParamsTO != null)
                        {
                            Double wtToleranceKgs = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);
                            Double pendingQtyInKgs = tblBookingsTO.PendingQty * 1000;
                            if (pendingQtyInKgs > 0 && pendingQtyInKgs <= wtToleranceKgs)
                            {
                                TblBookingQtyConsumptionTO bookingQtyConsumptionTO = new TblBookingQtyConsumptionTO();
                                bookingQtyConsumptionTO.BookingId = tblBookingsTO.IdBooking;
                                bookingQtyConsumptionTO.ConsumptionQty = tblBookingsTO.PendingQty;
                                bookingQtyConsumptionTO.CreatedBy = tblBookingsTO.UpdatedBy;
                                bookingQtyConsumptionTO.CreatedOn = tblBookingsTO.UpdatedOn;
                                bookingQtyConsumptionTO.StatusId = (int)tblBookingsTO.TranStatusE;
                                bookingQtyConsumptionTO.WeightTolerance = tblConfigParamsTO.ConfigParamVal + " KGs";
                                bookingQtyConsumptionTO.Remark = "Booking Pending Qty is Within Weight Tolerance Limit and Auto Closed";

                                result = BL.TblBookingQtyConsumptionBL.InsertTblBookingQtyConsumption(bookingQtyConsumptionTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While InsertTblBookingQtyConsumption";
                                    resultMessage.Tag = bookingQtyConsumptionTO;
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }

                                tblBookingsTO.PendingQty = 0;
                            }
                        }


                        result = BL.TblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }

                        tblBookingExtTOList = TblBookingExtBL.SelectAllTblBookingExtList(tblBookingsTO.IdBooking, conn, tran);
                        if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                        {
                            tblBookingExtTOList = tblBookingExtTOList.OrderBy(o => o.ScheduleDate).ToList();
                        }
                    }
                    
                    #endregion

                    #region LoadingSlip Layer Material Details.


                    resultMessage = InsertLoadingExtDetails(tblLoadingTO, conn, tran, ref isBoyondLoadingQuota, ref finalLoadQty, tblLoadingSlipTO, tblBookingsTO, tblBookingExtTOList);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }

                    #endregion

                    #region Save Loading Slip Layerwise Adress Details

                    if (tblLoadingSlipTO.DeliveryAddressTOList != null && tblLoadingSlipTO.DeliveryAddressTOList.Count > 0)
                    {
                        for (int a = 0; a < tblLoadingSlipTO.DeliveryAddressTOList.Count; a++)
                        {
                            TblLoadingSlipAddressTO deliveryAddressTO = tblLoadingSlipTO.DeliveryAddressTOList[a];
                            if (deliveryAddressTO.LoadingLayerId > 0)
                            {
                                deliveryAddressTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;

                                if (string.IsNullOrEmpty(deliveryAddressTO.Country))
                                    deliveryAddressTO.Country = Constants.DefaultCountry;

                                result = BL.TblLoadingSlipAddressBL.InsertTblLoadingSlipAddress(deliveryAddressTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While InsertTblLoadingSlipAddress Against LoadingSlip";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Delivery Address will not be compulsory while loading
                        //tran.Rollback();
                        //resultMessage.MessageType = ResultMessageE.Error;
                        //resultMessage.Text = "Error : LoadingSlipAddressTOList(Loading Address Details) Found Null Or Empty";
                        //return resultMessage;
                    }

                    #endregion
                }

                #endregion

                #region 3. Prepare A History Record

                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = tblLoadingTO.GetLoadingStatusHistoryTO();
                //Sanjay [2017-07-28] Condition Added As Proper history were not getting maintain
                //if (isBoyondLoadingQuota)
                //{
                //    tblLoadingStatusHistoryTO.TranStatusE= Constants.TranStatusE.LOADING_NOT_CONFIRM;
                //    tblLoadingStatusHistoryTO.StatusRemark= "Apporval Needed : Loading Beyond Loading Quota";
                //}
                //else
                //{
                //    tblLoadingStatusHistoryTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                //    tblLoadingStatusHistoryTO.StatusRemark = "Loading Scheduled & Confirmed";
                //}

                tblLoadingStatusHistoryTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                tblLoadingStatusHistoryTO.StatusRemark = "Apporval Needed";

                result = BL.TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error in InsertTblLoadingStatusHistory";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                #endregion

                #region 4. Finally Update the Total Loading Qty And Its Status based on loading quota consumption

                tblLoadingTO.TotalLoadingQty = finalLoadQty;
                tblLoadingTO.UpdatedBy = tblLoadingTO.CreatedBy;
                tblLoadingTO.UpdatedOn = tblLoadingTO.CreatedOn;
                //if (isBoyondLoadingQuota)
                //{
                //    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                //    tblLoadingTO.StatusReason = "Apporval Needed : Loading Beyond Loading Quota";

                //}
                //else
                //{
                //    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                //    tblLoadingTO.StatusReason = "Loading Scheduled & Confirmed";
                //}
                //Vijaymala added[22-06-2018]
                if (isAutoGateInVehicle == 1)
                {
                    tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                    tblLoadingTO.StatusReason = "Vehicle Entered In The Premises";
                }
                else
                {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                    tblLoadingTO.StatusReason = "Apporval Needed";
                }

                result = UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method SaveNewLoadingSlip";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                #endregion

                #region 5. Notifications For Approval Or Information
                //if (!isBoyondLoadingQuota)
                //Vijaymala added[03-05-2018]to change loading slip notification with party name
                String dealerOrgNames = String.Empty;
                if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 1)
                {
                    List<TblLoadingSlipTO> loadingSlipList = tblLoadingTO.LoadingSlipList;
                    List<TblLoadingSlipTO> distinctLoadingSlipList = loadingSlipList.GroupBy(w => w.DealerOrgId).Select(s => s.FirstOrDefault()).ToList();
                    if (distinctLoadingSlipList != null && distinctLoadingSlipList.Count > 0)
                    {
                        distinctLoadingSlipList.ForEach(f=> f.DealerOrgName = f.DealerOrgName.Replace(',', ' '));
                        dealerOrgNames = String.Join(" , ", distinctLoadingSlipList.Select(s => s.DealerOrgName.ToString()).ToArray());
                    }

                }
                TblConfigParamsTO dealerNameConfTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);
                Int32 dealerNameActive = 0;
                if (dealerNameConfTO != null)
                    dealerNameActive = Convert.ToInt32(dealerNameConfTO.ConfigParamVal);

                if (true)
                {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED;
                    tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CONFIRMATION_REQUIRED";
                    tblAlertInstanceTO.AlertComment = "Not confirmed loading slip  " + tblLoadingTO.LoadingSlipNo + "  is awaiting for confirmation";
                    if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                    {
                        tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                        tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                    }
                    tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(12);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CONFIRMATION_REQUIRED";
                    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                    tblAlertInstanceTO.RaisedBy = tblLoadingTO.CreatedBy;
                    tblAlertInstanceTO.RaisedOn = tblLoadingTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 0;


                    ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While SaveNewAlertInstance";
                        resultMessage.Tag = tblAlertInstanceTO;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }

                }
                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;

                resultMessage.Text = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated but approval needed.";
                resultMessage.DisplayMessage = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated but approval needed.";
                //Vijaymala added[22-06-2018]
                if (isAutoGateInVehicle == 1)
                {
                    resultMessage.Text = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated and ready for weighing.";
                    resultMessage.DisplayMessage = "Success, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated and ready for weighing.";
                }
                else
                { 

                    //Saket [2018-02-13] Added 
                    TblConfigParamsTO tblConfigParamsTOApproval = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SKIP_LOADING_APPROVAL, conn, tran);
                    if (tblConfigParamsTOApproval != null)
                    {
                        Int32 skiploadingApproval = Convert.ToInt32(tblConfigParamsTOApproval.ConfigParamVal);
                        if (skiploadingApproval == 1)
                        {
                            tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                            tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                            tblLoadingTO.StatusReason = "Loading Slip Auto Approved";

                            tblLoadingTO.UpdatedBy = tblLoadingTO.CreatedBy;
                            tblLoadingTO.StatusDate = Constants.ServerDateTime;
                            tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                            return BL.TblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO);
                        }
                    }

                   //if (isBoyondLoadingQuota)
                    //{
                    //    resultMessage.Text = "Sucess, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated but approval needed. Loading Quota Exceeded";
                    //    resultMessage.DisplayMessage = "Sucess, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated but approval needed. Loading Quota Exceeded";
                    //}
                    //else
                    //{
                    //    resultMessage.Text = "Sucess, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated and approved.";
                    //    resultMessage.DisplayMessage = "Sucess, New Loading Slip # - " + tblLoadingTO.LoadingSlipNo + " is generated and approved.";
                    //}
                }
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveNewLoadingSlip";
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        private static ResultMessage InsertLoadingExtDetails(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran, ref bool isBoyondLoadingQuota, ref double finalLoadQty, TblLoadingSlipTO tblLoadingSlipTO, TblBookingsTO tblBookingsTO, List<TblBookingExtTO> tblBookingExtTOList)
        {
            string loadingSlipNo = tblLoadingTO.LoadingSlipNo;
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();

            if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
            {
                if (tblLoadingTO.LoadingType == (int)Constants.LoadingTypeE.OTHER)
                {
                    for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++)
                    {

                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                        finalLoadQty += tblLoadingSlipExtTO.LoadingQty;
                    }//Vijaymala[23-Aug-2018] Commented for New Changes.
                }
                if (false)
                {

                    List<TblStockDetailsTO> stockDetailsList = TblStockDetailsBL.SelectAllTblStockDetailsList();
                    List<TblProductItemTO> productItemlist = TblProductItemBL.SelectAllTblProductItemList();
                    stockDetailsList = stockDetailsList.Where(ele => ele.ProdItemId > 0).ToList();

                    for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++)
                    {

                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                        tblLoadingSlipExtTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;

                        //Sudhir[15-Jan-2018] Added for get the productitemTo for checking otheritemstock update is require or not.
                        TblProductItemTO productItemTO = productItemlist.Where(ele => ele.IdProdItem == tblLoadingSlipExtTO.ProdItemId).FirstOrDefault();
                        if (productItemTO != null)
                        {
                            if (productItemTO.IsStockRequire == 1)
                            {
                                // Vaibhav [09-April-2018] Added to select compartment wise stock.
                                List<TblStockDetailsTO> stockDetailsFilterList = null;
                                if (tblLoadingSlipExtTO.CompartmentId == 0)
                                {
                                    stockDetailsFilterList = stockDetailsList.Where(x => x.ProdItemId == tblLoadingSlipExtTO.ProdItemId && x.BrandId == tblLoadingSlipExtTO.BrandId).ToList();
                                }
                                else
                                {
                                    stockDetailsFilterList = stockDetailsList.Where(x => x.ProdItemId == tblLoadingSlipExtTO.ProdItemId && x.BrandId == tblLoadingSlipExtTO.BrandId && x.LocationId == tblLoadingSlipExtTO.CompartmentId).ToList();
                                }

                                if (stockDetailsFilterList != null)
                                {
                                    stockDetailsFilterList = stockDetailsFilterList.Where(w => w.TotalStock > 0).ToList();
                                    if (stockDetailsFilterList == null || stockDetailsFilterList.Count == 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error : stockList Found NULL ";

                                        if (tblLoadingSlipExtTO.CompartmentId > 0)
                                        {
                                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock For the Size " + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " at Compartment " + tblLoadingSlipExtTO.CompartmentName + " not found";
                                        }
                                        else
                                        {
                                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock For the Size " + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " not found";
                                        }
                                        resultMessage.Result = 0;
                                        return resultMessage;
                                    }
                                    else
                                    {
                                        //tblLoadingSlipExtTO.Tag = stockDetailsFilterList;
                                        // To Use in Stock consumption , Wrt Loading Quota Availability Update Master Stock
                                        var totalAvailStock = stockDetailsFilterList.Sum(s => s.TotalStock);
                                        if (totalAvailStock >= tblLoadingSlipExtTO.LoadingQty)
                                        {
                                            List<TblStockDetailsTO> stockList = stockDetailsFilterList;

                                            // Create Stock Consumption History Record
                                            //var stkConsList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                            //                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                            //                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId).ToList();

                                            var stkConsList = stockList;

                                            Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                            for (int s = 0; s < stkConsList.Count; s++)
                                            {

                                                if (totalLoadingQty > 0)
                                                {
                                                    resultMessage = UpdateStockAndConsumptionHistory(tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, null, conn, tran);
                                                    if (resultMessage.MessageType != ResultMessageE.Information)
                                                    {
                                                        tran.Rollback();
                                                        resultMessage.DefaultBehaviour();
                                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                        resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip";
                                                        return resultMessage;
                                                    }
                                                }
                                            }

                                        }
                                        else
                                        {
                                            String errorMsg = tblLoadingSlipExtTO.ProdItemDesc;
                                            tran.Rollback();
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.Text = "Error - Stock Is Not Available for item :" + errorMsg;
                                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved.  Stock Is Not Available for item :" + errorMsg;
                                            resultMessage.Result = 0;
                                            resultMessage.Tag = tblLoadingSlipExtTO;
                                            return resultMessage;
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {

                        }
                        result = BL.TblLoadingSlipExtBL.InsertTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error : While InsertTblLoadingSlipExt Against other LoadingSlip";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                        finalLoadQty += tblLoadingSlipExtTO.LoadingQty;
                        isBoyondLoadingQuota = false;
                    }
                }
                else
                {
                    Int32 isAllowLoading = 0;
                    //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
                    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
                    if (tblConfigParamsTO != null)
                    {
                        isAllowLoading = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                    }
                    //[27-08-2018]Vijaymala commented code parity details from parity table  
                    //String parityIds = String.Empty;
                    //List<TblBookingParitiesTO> tblBookingParitiesTOList = TblBookingParitiesBL.SelectTblBookingParitiesTOListByBookingId(tblBookingsTO.IdBooking, conn, tran);
                    //if (tblBookingParitiesTOList != null && tblBookingParitiesTOList.Count > 0)
                    //{
                    //    parityIds = String.Join(",", tblBookingParitiesTOList.Select(s => s.ParityId.ToString()).ToArray());
                    //}

                    //if (String.IsNullOrEmpty(parityIds))
                    //{
                    //    tran.Rollback();
                    //    resultMessage.DefaultBehaviour();
                    //    resultMessage.Text = "Error : ParityTO Not Found";
                    //    resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                    //    return resultMessage;
                    //}
                    //commented end

                    // List<TblParityDetailsTO> parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(parityIds, 0, conn, tran);

                    //List<TblParityDetailsTO> parityDetailsTOList = null;
                    //if (tblBookingsTO.ParityId > 0)
                    //    parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(tblBookingsTO.ParityId, 0, conn, tran);



                    //List<TblLoadingQuotaDeclarationTO> quotaList = TblLoadingQuotaDeclarationBL.SelectLoadingQuotaListForCnfAndDate(tblLoadingTO.CnfOrgId, tblLoadingTO.CreatedOn, conn, tran);

                    List<TblProductInfoTO> productConfgList = TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);
                    if (productConfgList == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error : productConfgList Found NULL ";
                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Product Master Configuration is not completed.";
                        return resultMessage;
                    }

                    List<TblStockConfigTO> tblStockConfigTOList = BL.TblStockConfigBL.SelectAllTblStockConfigTOList(conn, tran);
                    if (tblStockConfigTOList == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error : StockConfigTOList Found NULL ";
                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock Configurator not found is not completed.";
                        return resultMessage;
                    }

                    tblStockConfigTOList = tblStockConfigTOList.Where(w => w.IsItemizedStock == 1).ToList();


                    Int32 isConsolidateStk = BL.TblConfigParamsBL.GetStockConfigIsConsolidate();


                    #region Check Stock,Loading Quota,Validate and Not Save 
                    //[05-09-2018] : Vijaymala added code to get product item list
                    List<TblProductItemTO> stockRequireProductItemList = TblProductItemBL.SelectProductItemListStockUpdateRequire(1);
                    Boolean isStockRequie = false;
                    for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++)
                    {
                        TblLoadingQuotaDeclarationTO loadingQuotaTOLive = null;

                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];
                        if (tblLoadingSlipExtTO.LoadingQty > 0)
                        {
                            tblLoadingSlipExtTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;


                            #region Calculate Bundles from Loading Qty and Product Configuration
                            if (tblLoadingSlipExtTO.ProdItemId == 0)//Vijaymala added [05-08-2018]
                            {
                                var prodConfgTO = productConfgList.Where(p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                                     p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                                     p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                     && p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();

                                if (prodConfgTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialDesc + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatDesc + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecDesc;
                                    resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                    return resultMessage;
                                }

                                //Product Configuration is per bundles and has avg Bundle Wtin Kg
                                //Hence convert loading qty(MT) to KG
                                Double noOfBundles = (tblLoadingSlipExtTO.LoadingQty * 1000) / prodConfgTO.AvgBundleWt;
                                tblLoadingSlipExtTO.Bundles = Math.Round(noOfBundles, 0);
                            }
                            else
                            {
                                //[05-09-2018]Vijaymala aadded code to set other item bundles
                                tblLoadingSlipExtTO.Bundles = tblLoadingSlipExtTO.LoadingQty;
                            }
                            #endregion

                            #region Stock Availability Calculations and Validations

                            //Check If Stock exist Or Not
                            //List<TblStockDetailsTO> stockList = TblStockDetailsDAO.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingTO.CreatedOn, conn, tran);

                            List<TblStockDetailsTO> stockList = new List<TblStockDetailsTO>();

                            String isItemized = "Itemized";
                           
                            if (isConsolidateStk == 1)
                            {
                                isItemized = "Consolidate";

                                TblStockConfigTO tblStockConfigTO = tblStockConfigTOList.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId
                                                                    && w.MaterialId == tblLoadingSlipExtTO.MaterialId && w.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                    && w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.IsItemizedStock == 1).FirstOrDefault();

                                if (tblStockConfigTO != null)  //Get Itemized Stock
                                {
                                    stockList = TblStockDetailsBL.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, new DateTime(), tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, conn, tran);
                                    isItemized = "Itemized";


                                    stockList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                                                     && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                                                     && l.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                                                                     && l.BrandId == tblLoadingSlipExtTO.BrandId
                                                                                                     && l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList();


                                    stockList = stockList.Where(w => w.TotalStock > 0).ToList();


                                }
                                else //Get consolidate stock brand wise
                                {
                                    stockList = TblStockDetailsBL.SelectAllTblStockDetailsListConsolidated(1, tblLoadingSlipExtTO.BrandId, conn, tran);
                                    stockList = stockList.Where(w => w.TotalStock > 0).ToList();
                                }
                            }
                            else
                            {
                                
                                //stockList = TblStockDetailsBL.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, new DateTime(), tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, conn, tran);
                                stockList = TblStockDetailsDAO.SelectAllTblStockDetailsOther(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime(), conn, tran);

                                isItemized = "Itemized";

                                stockList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                                                                 && l.BrandId == tblLoadingSlipExtTO.BrandId
                                                                                                 && l.ProdItemId == tblLoadingSlipExtTO.ProdItemId
                                                                                                 && l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList();

                                if (tblLoadingSlipExtTO.ProdItemId == 0)
                                {
                                    //Check is item boughtout.
                                    TblStockConfigTO tblStockConfigTO = tblStockConfigTOList.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId
                                                                        && w.MaterialId == tblLoadingSlipExtTO.MaterialId && w.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                        && w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.IsItemizedStock == 1).FirstOrDefault();

                                    if (tblStockConfigTO != null)  //Get Itemized Stock
                                    {
                                        Double upQty = tblLoadingSlipExtTO.LoadingQty;
                                        Double existingQtyInMt = 0;
                                        Double newQtyInMt = 0;
                                        Int32 stockDtlId = 0;
                                        List<TblLocationTO> tblLocationTOList = BL.TblLocationBL.SelectAllTblLocationList();

                                        Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                        if (stockList == null || stockList.Count == 0)
                                        {
                                            resultMessage = InsertItemIntoStockDtlAndSummary(tblLoadingTO, totalLoadingQty, conn, tran, tblLoadingSlipExtTO, tblLoadingTO.CreatedBy, tblLocationTOList);
                                            if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(TblStockDetailsTO))
                                            {
                                                TblStockDetailsTO tempStockDetailsTO = new TblStockDetailsTO();

                                                tempStockDetailsTO = (TblStockDetailsTO)resultMessage.Tag;
                                                stockDtlId = tempStockDetailsTO.IdStockDtl;
                                                stockList = new List<TblStockDetailsTO>();
                                                stockList.Add(tempStockDetailsTO);

                                            }
                                            //  return InsertItemIntoStockDtlAndSummary()
                                        }

                                        if (stockList != null && stockList.Count > 0)
                                        {
                                            TblStockDetailsTO tblStockDetailsTO = stockList[0];
                                            stockList = new List<TblStockDetailsTO>();
                                            stockList.Add(tblStockDetailsTO);

                                            existingQtyInMt = tblStockDetailsTO.TotalStock;

                                            tblStockDetailsTO.TotalStock += upQty;
                                            tblStockDetailsTO.BalanceStock += upQty;
                                            tblStockDetailsTO.NoOfBundles += tblLoadingSlipExtTO.Bundles;

                                            newQtyInMt = tblStockDetailsTO.TotalStock;
                                            stockDtlId = tblStockDetailsTO.IdStockDtl;

                                            tblStockDetailsTO.UpdatedOn = Constants.ServerDateTime;
                                            tblStockDetailsTO.UpdatedBy = tblLoadingTO.CreatedBy;

                                            //Update Stock details

                                            result = TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
                                            if (result != 1)
                                            {
                                                tran.Rollback();
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                resultMessage.Text = "Error : While updating the stock details";
                                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while updating the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                                                   // + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " not found";
                                                resultMessage.Result = 0;
                                                return resultMessage;
                                            }
                                        }
                                        else
                                        {
                                            if(isAllowLoading == 0)
                                            {
                                                tran.Rollback();
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                resultMessage.Text = "Error : Stock is not taken";
                                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " Stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                                                //+ tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " not found";
                                                resultMessage.Result = 0;
                                                return resultMessage;
                                            }
                                           
                                            //TblStockDetailsTO tblStockDetailsTO = new TblStockDetailsTO();
                                            //stockList = new List<TblStockDetailsTO>();
                                            //stockList.Add(tblStockDetailsTO);
                                            //tblStockDetailsTO.BrandId = tblLoadingSlipExtTO.BrandId;
                                            //tblStockDetailsTO.MaterialId = tblLoadingSlipExtTO.MaterialId;
                                            //tblStockDetailsTO.ProdCatId= tblLoadingSlipExtTO.ProdCatId;
                                            //tblStockDetailsTO.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
                                            //tblStockDetailsTO.LocationId = 1;
                                            //tblStockDetailsTO.StockSummaryId = 1;

                                            //tblStockDetailsTO.NoOfBundles = 0;
                                            //tblStockDetailsTO.ProductId = 0;


                                            //Insert


                                        }



                                        //Insert Into tbl Consumption
                                        #region Insert In Consumption

                                        TblStockConsumptionTO tblStockConsumptionTO = new TblStockConsumptionTO();

                                        tblStockConsumptionTO.BeforeStockQty = existingQtyInMt;
                                        tblStockConsumptionTO.AfterStockQty = newQtyInMt;
                                        tblStockConsumptionTO.LoadingSlipExtId = 0;
                                        tblStockConsumptionTO.CreatedBy = tblLoadingSlipTO.CreatedBy;
                                        tblStockConsumptionTO.CreatedOn = Constants.ServerDateTime;
                                        tblStockConsumptionTO.StockDtlId = stockDtlId;

                                        tblStockConsumptionTO.TxnQty = Math.Round(upQty, 2);

                                        if (tblStockConsumptionTO.TxnQty > 0)
                                        {
                                            tblStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                                            tblStockConsumptionTO.Remark = tblStockConsumptionTO.TxnQty + " Qty is added against bought out item";

                                        }
                                        else
                                        {
                                            tblStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
                                            tblStockConsumptionTO.Remark = tblStockConsumptionTO.TxnQty + " Qty is consumed against bought out item";

                                        }


                                        if (tblStockConsumptionTO.TxnQty != 0)
                                        {
                                            result = BL.TblStockConsumptionBL.InsertTblStockConsumption(tblStockConsumptionTO, conn, tran);
                                            if (result != 1)
                                            {
                                                tran.Rollback();
                                                resultMessage.Text = "Error While InsertTblStockConsumption : Method UpdateDailyStock";
                                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                                resultMessage.Result = 0;
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                return resultMessage;
                                            }
                                        }
                                        #endregion


                                    }

                                    stockList = stockList.Where(w => w.TotalStock > 0).ToList();
                                }

                            }

                            if ((stockList == null || stockList.Count == 0) && isAllowLoading ==0)
                            {
                              

                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = " Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock For that item is not found ";

                                if (tblLoadingSlipExtTO.CompartmentId > 0)
                                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock For the Size " + tblLoadingSlipExtTO.DisplayName
                                //+ tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")"
                                + " at Compartment " + tblLoadingSlipExtTO.CompartmentName + " not found";
                                else
                                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock For the Size " + tblLoadingSlipExtTO.DisplayName
                                //+ tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")"
                                    + " not found";

                                resultMessage.Result = 0;
                                return resultMessage;
                            }

                            tblLoadingSlipExtTO.Tag = stockList;

                            //[05-09-2018]Vijaymala added to check stock require for item or not
                            if (tblLoadingSlipExtTO.ProdItemId > 0)
                            {
                                isStockRequie = stockRequireProductItemList.Where(ele => ele.IdProdItem == tblLoadingSlipExtTO.ProdItemId).
                                                                            Select(x => x.IsStockRequire == 1).FirstOrDefault();

                            }
                            else
                            {
                                isStockRequie = true;
                            }


                            if(isStockRequie)
                            {
                                
                                // To Use in Stock consumption , Wrt Loading Quota Availability Update Master Stock
                                var totalAvailStock = stockList.Sum(s => s.TotalStock);

                                if (totalAvailStock >= tblLoadingSlipExtTO.LoadingQty)
                                {

                                }
                                else
                                {
                                    if(isAllowLoading == 0)
                                    {
                                        String errorMsg = tblLoadingSlipExtTO.DisplayName;
                                        //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.ProdSpecDesc + ")";
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error - Stock Is Not Available for item :" + errorMsg;
                                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock Is Not Available for item :" + errorMsg;
                                        resultMessage.Result = 0;
                                        resultMessage.Tag = tblLoadingSlipExtTO;
                                        return resultMessage;
                                    }
                                    
                                }
                            }
                               

                            //TblLoadingQuotaDeclarationTO loadingQuotaTO = quotaList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                            //                                                               && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                            //                                                               && l.MaterialId == tblLoadingSlipExtTO.MaterialId).FirstOrDefault();

                            //Double quotaBforeLoading = 0;
                            //Double quotaAfterLoading = 0;

                            //if (loadingQuotaTO != null)
                            //{
                            //    loadingQuotaTOLive = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaTO.IdLoadingQuota, conn, tran);
                            //    if (loadingQuotaTOLive == null)
                            //    {
                            //        tran.Rollback();
                            //        resultMessage.MessageType = ResultMessageE.Error;
                            //        resultMessage.Text = "Error : loadingQuotaTOLive Found NULL ";
                            //        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            //        resultMessage.Result = 0;
                            //        return resultMessage;
                            //    }

                            //    quotaBforeLoading = loadingQuotaTOLive.BalanceQuota;
                            //    quotaAfterLoading = quotaBforeLoading - tblLoadingSlipExtTO.LoadingQty;

                            //    tblLoadingSlipExtTO.QuotaBforeLoading = quotaBforeLoading;
                            //    tblLoadingSlipExtTO.QuotaAfterLoading = quotaAfterLoading;
                            //    tblLoadingSlipExtTO.LoadingQuotaId = loadingQuotaTOLive.IdLoadingQuota;
                            //}
                            //else
                            //{
                            //    isBoyondLoadingQuota = true;
                            //}
                            //if (!isBoyondLoadingQuota)
                            //{
                            //    if (tblLoadingSlipExtTO.QuotaAfterLoading < 0)
                            //        isBoyondLoadingQuota = true;
                            //}
                            #endregion

                            #region Calculate Actual Price From Booking and Parity Settings
                            /*
                            Double orcAmtPerTon = 0;
                            if (tblBookingsTO.OrcMeasure == "Rs/MT")
                            {
                                orcAmtPerTon = tblBookingsTO.OrcAmt;
                            }
                            else
                            {
                                if (tblBookingsTO.OrcAmt > 0)
                                    orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                            }

                            //String rateCalcDesc = string.Empty;
                            //rateCalcDesc = "B.R : " + tblBookingsTO.BookingRate + "|";
                            //Double bookingPrice = tblBookingsTO.BookingRate;

                            TblBookingParitiesTO tblBookingParitiesTO = tblBookingParitiesTOList.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();
                            if (tblBookingParitiesTO == null || tblBookingParitiesTO.BookingRate == 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : Rate not found against brand - " + tblLoadingSlipExtTO.BrandDesc;
                                resultMessage.DisplayMessage = "Error : Rate not found against brand - " + tblLoadingSlipExtTO.BrandDesc;
                                return resultMessage;
                            }


                            String rateCalcDesc = string.Empty;
                            rateCalcDesc = "B.R : " + tblBookingParitiesTO.BookingRate + "|";
                            Double bookingPrice = tblBookingParitiesTO.BookingRate;

                            Double parityAmt = 0;
                            Double priceSetOff = 0;
                            Double paritySettingAmt = 0;
                            Double bvcAmt = 0;
                            TblParitySummaryTO parityTO = null;
                            if (parityDetailsTOList != null)
                            {
                                var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                        && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                        && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                         && m.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();
                                if (parityDtlTO != null)
                                {
                                    parityAmt = parityDtlTO.ParityAmt;
                                    if (tblLoadingSlipTO.IsConfirmed != 1)
                                        priceSetOff = parityDtlTO.NonConfParityAmt;
                                    else
                                        priceSetOff = 0;

                                    tblLoadingSlipExtTO.ParityDtlId = parityDtlTO.IdParityDtl;
                                }
                                else
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : ParityTO Not Found";
                                    string mateDesc = tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                                    resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                                    return resultMessage;
                                }

                                parityTO = BL.TblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                                if (parityTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : ParityTO Not Found";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }

                                paritySettingAmt = parityTO.BaseValCorAmt + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                bvcAmt = parityTO.BaseValCorAmt;
                                rateCalcDesc += "BVC Amt :" + parityTO.BaseValCorAmt + "|" + "Exp Amt :" + parityTO.ExpenseAmt + "|" + " Other :" + parityTO.OtherAmt + "|";
                            }
                            else
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : ParityTO Not Found";
                                resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                                return resultMessage;
                            }

                            Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                            if (tblLoadingSlipTO.IsConfirmed == 1)
                                cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt;

                            Double cdAmt = 0;
                            if (tblLoadingSlipTO.CdStructure > 0)
                                cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;

                            rateCalcDesc += "CD :" + Math.Round(cdAmt, 2) + "|";
                            Double rateAfterCD = cdApplicableAmt - cdAmt;

                            Double gstApplicableAmt = 0;
                            if (tblLoadingSlipTO.IsConfirmed == 1)
                                //gstApplicableAmt = rateAfterCD + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;
                                gstApplicableAmt = rateAfterCD + freightPerMT;
                            else
                                gstApplicableAmt = rateAfterCD;

                            Double gstAmt = (gstApplicableAmt * 18) / 100;
                            gstAmt = Math.Round(gstAmt, 2);

                            Double finalRate = 0;
                            if (tblLoadingSlipTO.IsConfirmed == 1)
                                finalRate = gstApplicableAmt + gstAmt;
                            else
                                finalRate = gstApplicableAmt + gstAmt + freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;

                            tblLoadingSlipExtTO.TaxableRateMT = gstApplicableAmt;
                            tblLoadingSlipExtTO.RatePerMT = finalRate;
                            tblLoadingSlipExtTO.CdApplicableAmt = cdApplicableAmt;
                            tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt;

                            rateCalcDesc += " ORC :" + orcAmtPerTon + "|" + " Parity :" + parityAmt + "|" + " NC Amt :" + priceSetOff + "|" + " Freight :" + freightPerMT + "|" + " GST :" + gstAmt + "|";
                            tblLoadingSlipExtTO.RateCalcDesc = rateCalcDesc;
                            */
                            #endregion



                            Int32 bookingExtId = tblLoadingSlipExtTO.BookingExtId;
                            tblLoadingSlipExtTO.BookingExtId = 0;
                            result = BL.TblLoadingSlipExtBL.InsertTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                            tblLoadingSlipExtTO.BookingExtId = bookingExtId;

                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "Error : While InsertTblLoadingSlipExt Against LoadingSlip";
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }

                            if (tblLoadingSlipExtTO.LoadingQuotaId > 0) // It means loading quota is not allocated. This request is beyond quota
                            {
                                //Create Loading Quota Consumption History Record
                                Models.TblLoadingQuotaConsumptionTO quotaConsumptionTO = new TblLoadingQuotaConsumptionTO();
                                quotaConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                                quotaConsumptionTO.QuotaQty = -tblLoadingSlipExtTO.LoadingQty;
                                quotaConsumptionTO.AvailableQuota = tblLoadingSlipExtTO.QuotaBforeLoading;
                                quotaConsumptionTO.BalanceQuota = tblLoadingSlipExtTO.QuotaAfterLoading;
                                quotaConsumptionTO.LoadingQuotaId = tblLoadingSlipExtTO.LoadingQuotaId;
                                quotaConsumptionTO.Remark = "Quota Consumed Against Loading Slip No :" + loadingSlipNo;
                                quotaConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
                                quotaConsumptionTO.CreatedBy = tblLoadingTO.CreatedBy;
                                quotaConsumptionTO.CreatedOn = tblLoadingTO.CreatedOn;

                                result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(quotaConsumptionTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }

                                //Update Balance Quota for Declared Loading Quota
                                loadingQuotaTOLive.BalanceQuota = loadingQuotaTOLive.BalanceQuota - tblLoadingSlipExtTO.LoadingQty;
                                loadingQuotaTOLive.UpdatedBy = tblLoadingTO.CreatedBy;
                                loadingQuotaTOLive.UpdatedOn = tblLoadingTO.CreatedOn;
                                loadingQuotaTOLive.Remark = tblLoadingSlipExtTO.LoadingQty + " Qty is consumed against Loading Slip : " + loadingSlipNo;
                                result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(loadingQuotaTOLive, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : While UpdateTblLoadingQuotaDeclaration Against LoadingSlip ";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }

                            #region Adjust Balance Qty

                            List<TblBookingExtTO> tblBookingExtTOListAdj = new List<TblBookingExtTO>();

                            if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                            {
                                if (tblLoadingSlipExtTO.BookingExtId > 0)
                                {
                                    TblBookingExtTO tblBookingExtTO = tblBookingExtTOList.Where(w => w.IdBookingExt == tblLoadingSlipExtTO.BookingExtId).FirstOrDefault();

                                    tblBookingExtTOListAdj.Add(tblBookingExtTO);


                                }

                                List<TblBookingExtTO> temp = tblBookingExtTOList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                             && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                             && l.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                             && l.BrandId == tblLoadingSlipExtTO.BrandId
                                                             &&l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList();

                                if (tblLoadingSlipExtTO.BookingExtId > 0)
                                {
                                    temp = temp.Where(w => w.IdBookingExt != tblLoadingSlipExtTO.BookingExtId).ToList();
                                }


                                if (temp != null && temp.Count > 0)
                                {
                                    tblBookingExtTOListAdj.AddRange(temp);
                                }

                                Double qtyToAdjust = tblLoadingSlipExtTO.LoadingQty;

                                for (int a = 0; a < tblBookingExtTOListAdj.Count; a++)
                                {
                                    if (qtyToAdjust > 0)
                                    {
                                        TblBookingExtTO tblBookingExtTO = tblBookingExtTOListAdj[a];
                                        if (tblBookingExtTO.BalanceQty > 0)
                                        {
                                            if (qtyToAdjust <= tblBookingExtTO.BalanceQty)
                                            {
                                                tblBookingExtTO.BalanceQty = tblBookingExtTO.BalanceQty - qtyToAdjust;
                                                qtyToAdjust = 0;
                                            }
                                            else
                                            {
                                                tblBookingExtTO.BalanceQty = 0;
                                                qtyToAdjust -= tblBookingExtTO.BalanceQty;
                                            }

                                            result = TblBookingExtBL.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
                                            if (result != 1)
                                            {
                                                tran.Rollback();
                                                resultMessage.DefaultBehaviour();
                                                resultMessage.Text = "Error : While UpdateTblLoadingQuotaDeclaration Against LoadingSlip ";
                                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                return resultMessage;
                                            }

                                        }
                                    }

                                }

                            }
                            #endregion

                        }
                    }

                    #endregion

                    #region Wrt Loading Quota Availability Update Master Stock

                    //If Loading Quota is expired then do not give master stock effects. Discussed with Nitin K
                    if (!isBoyondLoadingQuota)
                    {

                        for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++)
                        {

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];

                            //Check If Stock exist Or Not
                            if (tblLoadingSlipExtTO.Tag == null && isAllowLoading ==0)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error : stockList Found NULL ";
                                resultMessage.Result = 0;
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }
                            TblProductInfoTO prodConfgTO = null;
                            if (tblLoadingSlipExtTO.ProdItemId == 0)
                            {
                                 prodConfgTO = productConfgList.Where(p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                                     p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                                     p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                     && p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();

                                if (prodConfgTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialId + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatId + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecId;
                                    resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                    return resultMessage;
                                }
                            }





                            List<TblStockDetailsTO> stockList = (List<TblStockDetailsTO>)tblLoadingSlipExtTO.Tag;

                            // Create Stock Consumption History Record
                            //var stkConsList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                            //                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                            //                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId).ToList();

                            var stkConsList = stockList;

                            Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;

                            for (int s = 0; s < stkConsList.Count; s++)
                            {

                                if (totalLoadingQty > 0)
                                {
                                    resultMessage = UpdateStockAndConsumptionHistory(tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, prodConfgTO, conn, tran);
                                    if (resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip";
                                        return resultMessage;
                                    }
                                }
                            }
                            if (isAllowLoading == 1)
                            {
                                TblStockConfigTO tblStockConfigTO = new TblStockConfigTO();
                                Double upQty = tblLoadingSlipExtTO.LoadingQty;
                                Double existingQtyInMt = 0;
                                Double newQtyInMt = 0;
                                Int32 stockDtlId = 0;
                                String isItemized = "Itemized";
                                isAllowLoading = 1;
                                tblStockConfigTO.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
                                tblStockConfigTO.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
                                tblStockConfigTO.BrandId = tblLoadingSlipExtTO.BrandId;
                                tblStockConfigTO.MaterialId = tblLoadingSlipExtTO.MaterialId;
                                List<TblLocationTO> tblLocationTOList = BL.TblLocationBL.SelectAllTblLocationList();

                                if (totalLoadingQty > 0)
                                {
                                    resultMessage = UpdateStockAgainstItem(tblLoadingTO,totalLoadingQty, conn, tran, tblLoadingSlipExtTO, tblLoadingTO.CreatedBy, tblLocationTOList);
                                    // return ConfigureAllowStockUpdate(tblLoadingTO, conn, tran, ref result, resultMessage, tblLoadingSlipExtTO, ref stockList, isItemized, ref isAllowLoading, tblStockConfigTO, upQty, ref existingQtyInMt, ref stockDtlId, tblLocationTOList);
                                    // return resultMessage;
                                    if (resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.Text = "Error : While UpdateStockAgainstItem Against LoadingSlip";
                                        return resultMessage;
                                    }
                                }
                            }
                            else
                            {
                                if (totalLoadingQty > 0)
                                {
                                    String isItemized = String.Empty;
                                    String errorMsg = tblLoadingSlipExtTO.DisplayName;
                                    // tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.ProdSpecDesc + ")";
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error - Stock Is Not Available for item :" + errorMsg;
                                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock Is Not Available for item :" + errorMsg;
                                    resultMessage.Result = 0;
                                    resultMessage.Tag = tblLoadingSlipExtTO;
                                    return resultMessage;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            else
            {
                tran.Rollback();
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error : LoadingSlipExtTOList(Loading Layer Details) Found Null Or Empty";
                resultMessage.DisplayMessage = "Error 01 : No Loading Layer Found To Save";
                return resultMessage;
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }
        //private static ResultMessage ConfigureAllowStockUpdate(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran, ref int result, ResultMessage resultMessage, TblLoadingSlipExtTO tblLoadingSlipExtTO, ref List<TblStockDetailsTO> stockList, string isItemized, ref Int32 isAllowLoading, TblStockConfigTO tblStockConfigTO, Double upQty, ref Double existingQtyInMt, ref int stockDtlId, List<TblLocationTO> tblLocationTOList)
        //{

        //    //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
        //    TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
        //    if (tblConfigParamsTO != null)
        //    {
        //        isAllowLoading = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
        //    }
        //    if (isAllowLoading == 1)
        //    {
        //        stockList = TblStockDetailsDAO.SelectAllTblStockDetailsOther(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime(), conn, tran);
        //         isItemized = "Itemized";

        //        stockList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
        //                                                                         && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
        //                                                                         && l.MaterialId == tblLoadingSlipExtTO.MaterialId
        //                                                                         && l.BrandId == tblLoadingSlipExtTO.BrandId
        //                                                                         && l.ProdItemId == tblLoadingSlipExtTO.ProdItemId
        //                                                                         && l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList();

        //        Double totalLoadingQty = 0 - tblLoadingSlipExtTO.LoadingQty;
        //        TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();

        //        List<TblProductInfoTO> productList = BL.TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);

        //        if (stockList.Count == 0)
        //        {
        //            totalLoadingQty = 0 - tblLoadingSlipExtTO.LoadingQty;
        //            stockList = new List<TblStockDetailsTO>();

        //            //Insert Stock Summary
        //            TblStockSummaryTO tblStockSummaryTO = new TblStockSummaryTO();
        //            tblStockSummaryTO.StockDate = Constants.ServerDateTime;
        //            tblStockSummaryTO.NoOfBundles = 0;
        //            tblStockSummaryTO.TotalStock = 0;
        //            tblStockSummaryTO.CreatedBy = tblLoadingTO.CreatedBy;
        //            tblStockSummaryTO.CreatedOn = Constants.ServerDateTime;

        //            result = TblStockSummaryBL.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMessage.MessageType = ResultMessageE.Error;
        //                resultMessage.Text = "Error : While insert the stock summary";
        //                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
        //                resultMessage.Result = 0;
        //                return resultMessage;
        //            }

        //            // For weight and Stock in MT calculations
        //            tblStockDetailsTONew.TotalStock = tblStockDetailsTONew.LoadedStock - upQty;
        //            tblStockDetailsTONew.LoadedStock = tblStockDetailsTONew.LoadedStock - upQty;
        //            tblStockDetailsTONew.BalanceStock = tblStockDetailsTONew.BalanceStock - upQty;
        //            tblStockDetailsTONew.NoOfBundles = tblStockDetailsTONew.NoOfBundles - tblLoadingSlipExtTO.Bundles;
        //            tblStockDetailsTONew.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
        //            tblStockDetailsTONew.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
        //            tblStockDetailsTONew.BrandId = tblLoadingSlipExtTO.BrandId;
        //            tblStockDetailsTONew.MaterialId = tblLoadingSlipExtTO.MaterialId;
        //            tblStockDetailsTONew.CreatedOn = Constants.ServerDateTime;
        //            tblStockDetailsTONew.CreatedBy = tblLoadingTO.CreatedBy;

        //            TblProductInfoTO productInfoTO = productList.Where(p => p.MaterialId == tblStockDetailsTONew.MaterialId
        //                                            && p.ProdCatId == tblStockDetailsTONew.ProdCatId && p.ProdSpecId == tblStockDetailsTONew.ProdSpecId
        //                                            && p.BrandId == tblStockDetailsTONew.BrandId).FirstOrDefault();

        //            tblStockDetailsTONew.ProductId = productInfoTO.IdProduct;
        //            tblStockDetailsTONew.IsInMT = 1;
        //            tblStockDetailsTONew.StockSummaryId = tblStockSummaryTO.IdStockSummary;
        //            tblStockDetailsTONew.LocationId = tblLocationTOList[0].IdLocation;

        //            existingQtyInMt = tblStockDetailsTONew.TotalStock;

        //            result = TblStockDetailsBL.InsertTblStockDetails(tblStockDetailsTONew, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMessage.MessageType = ResultMessageE.Error;
        //                resultMessage.Text = "Error : While insert the stock details";
        //                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
        //                resultMessage.Result = 0;
        //                //  return resultMessage;
        //            }
        //            stockList.Add(tblStockDetailsTONew);
        //            stockDtlId = tblStockDetailsTONew.IdStockDtl;

        //            //Update Stock Summary

        //            tblStockSummaryTO.NoOfBundles = stockList[0].NoOfBundles;
        //            tblStockSummaryTO.TotalStock = stockList[0].TotalStock;
        //            tblStockSummaryTO.UpdatedBy = tblLoadingTO.CreatedBy;
        //            tblStockSummaryTO.UpdatedOn = Constants.ServerDateTime;

        //            result = TblStockSummaryBL.UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMessage.MessageType = ResultMessageE.Error;
        //                resultMessage.Text = "Error : While insert the stock summary";
        //                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
        //                resultMessage.Result = 0;
        //                //return resultMessage;
        //            }
        //            return resultMessage;
        //        }
        //        else
        //        {

        //            List<TblProductInfoTO> productConfgList = TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);
        //            if (productConfgList == null)
        //            {
        //                tran.Rollback();
        //                resultMessage.DefaultBehaviour();
        //                resultMessage.Text = "Error : productConfgList Found NULL ";
        //                resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Product Master Configuration is not completed.";
        //                return resultMessage;
        //            }
        //            var prodConfgTO = productConfgList.Where(p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
        //                                                                  p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
        //                                                                  p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
        //                                                                  && p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();

        //            if (prodConfgTO == null)
        //            {
        //                tran.Rollback();
        //                resultMessage.DefaultBehaviour();
        //                resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialId + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatId + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecId;
        //                resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
        //                return resultMessage;
        //            }
        //            for (int s = 0; s < stockList.Count; s++)
        //            {
        //                resultMessage = UpdateStockAndConsumptionHistory(tblLoadingSlipExtTO, tblLoadingTO, stockList[s].IdStockDtl, ref totalLoadingQty, prodConfgTO, conn, tran);
        //                if (resultMessage.MessageType != ResultMessageE.Information)
        //                {
        //                    tran.Rollback();
        //                    resultMessage.DefaultBehaviour();
        //                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
        //                    resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip";
        //                    return resultMessage;
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {

        //        tran.Rollback();
        //        resultMessage.MessageType = ResultMessageE.Error;
        //        resultMessage.Text = "Error : Stock is not taken";
        //        resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " Stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
        //        //+ tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " not found";
        //        resultMessage.Result = 0;
        //        return resultMessage;

        //    }
        //    return resultMessage;

        //}

        private static ResultMessage UpdateStockAgainstItem( TblLoadingTO tblLoadingTO, Double totalLoadingQty, SqlConnection conn, SqlTransaction tran, TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 loginUserId, List<TblLocationTO> tblLocationTOList)
        {
            Int32 isAllowLoading = 0;
            Int32 stockDtlId = 0;
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            try
            {

            //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
            TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
            if (tblConfigParamsTO != null)
            {
                isAllowLoading = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }
            if (isAllowLoading == 1)
            {
                List<TblStockDetailsTO> stockList = TblStockDetailsDAO.SelectAllTblStockDetailsOther(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime(), conn, tran);
                String isItemized = "Itemized";

                stockList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                                                 && l.BrandId == tblLoadingSlipExtTO.BrandId
                                                                                 && l.ProdItemId == tblLoadingSlipExtTO.ProdItemId
                                                                                 && l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList();

                List<TblProductInfoTO> productList = BL.TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);
                    TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();

                    if (stockList.Count == 0)
                    {
                        // For weight and Stock in MT calculations
                        tblStockDetailsTONew.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
                        tblStockDetailsTONew.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
                        tblStockDetailsTONew.BrandId = tblLoadingSlipExtTO.BrandId;
                        tblStockDetailsTONew.MaterialId = tblLoadingSlipExtTO.MaterialId;
                        tblStockDetailsTONew.CreatedOn = Constants.ServerDateTime;
                        tblStockDetailsTONew.CreatedBy = loginUserId;
                        tblStockDetailsTONew.ProdItemId = tblLoadingSlipExtTO.ProdItemId;
                        
                        if (tblLoadingSlipExtTO.CompartmentId > 0)
                        {
                            tblStockDetailsTONew.LocationId = tblLoadingSlipExtTO.CompartmentId;
                        }
                        else
                        {
                            Int32 tempLocatId = 0;

                            var tempLocationLst = tblLocationTOList.Where(w => w.ParentLocId > 0).ToList();
                            if (tempLocationLst != null && tempLocationLst.Count > 0)
                            {
                                tempLocatId = tempLocationLst[0].IdLocation;
                            }

                            tblStockDetailsTONew.LocationId = tempLocatId;
                        }


                        TblStockSummaryTO tblStockSummaryTO = BL.TblStockSummaryBL.SelectTblStockSummaryTO(new DateTime());
                        if (tblStockSummaryTO == null)
                        {
                            tblStockSummaryTO = new TblStockSummaryTO();
                            tblStockSummaryTO.StockDate = Constants.ServerDateTime;
                            tblStockSummaryTO.NoOfBundles = 0;
                            tblStockSummaryTO.TotalStock = 0;
                            tblStockSummaryTO.CreatedBy = loginUserId;
                            tblStockSummaryTO.CreatedOn = Constants.ServerDateTime;

                            result = TblStockSummaryBL.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error : While insert the stock summary";
                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }

                        tblStockDetailsTONew.StockSummaryId = tblStockSummaryTO.IdStockSummary;


                        result = TblStockDetailsBL.InsertTblStockDetails(tblStockDetailsTONew, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While insert the stock details";
                            resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                    else
                    {
                        tblStockDetailsTONew = stockList[0];
                    }

                    Double adjustedBundles = 0;

                    tblStockDetailsTONew.TotalStock = tblStockDetailsTONew.BalanceStock - totalLoadingQty;

                    tblStockDetailsTONew.LoadedStock += totalLoadingQty;
                    tblStockDetailsTONew.BalanceStock = tblStockDetailsTONew.BalanceStock - totalLoadingQty;

                    if(tblStockDetailsTONew.ProdItemId == 0)
                    {
                        TblProductInfoTO productInfoTO = productList.Where(p => p.MaterialId == tblStockDetailsTONew.MaterialId
                                                       && p.ProdCatId == tblStockDetailsTONew.ProdCatId && p.ProdSpecId == tblStockDetailsTONew.ProdSpecId
                                                       && p.BrandId == tblStockDetailsTONew.BrandId).FirstOrDefault();

                        tblStockDetailsTONew.ProductId = productInfoTO.IdProduct;

                        adjustedBundles = totalLoadingQty * 1000 / productInfoTO.AvgBundleWt;

                    }

                    else
                    {
                        adjustedBundles = totalLoadingQty;
                    }


                    tblStockDetailsTONew.NoOfBundles -= adjustedBundles;
                    tblStockDetailsTONew.IsInMT = 1;

                    if (tblStockDetailsTONew.IdStockDtl > 0)
                    {
                        tblStockDetailsTONew.UpdatedBy = loginUserId;
                        tblStockDetailsTONew.UpdatedOn = Constants.ServerDateTime;
                        result = TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTONew, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While update the stock details";
                            resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while update the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                    else
                    {
                        return resultMessage;
                    }


                    //TblStockSummaryTO mainSummaryTO = TblStockSummaryBL.SelectTblStockSummaryTO(tblStockDetailsTONew.StockSummaryId, conn, tran);
                    //mainSummaryTO.NoOfBundles -= adjustedBundles;
                    //mainSummaryTO.TotalStock -= totalLoadingQty;
                    //mainSummaryTO.UpdatedBy = loginUserId;
                    //mainSummaryTO.UpdatedOn = Constants.ServerDateTime;

                    //result = TblStockSummaryBL.UpdateTblStockSummary(mainSummaryTO, conn, tran);
                    //if (result != 1)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error : While insert the stock summary";
                    //    resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
                    //    resultMessage.Result = 0;
                    //    return resultMessage;
                    //}

                    //stockList = new List<TblStockDetailsTO>();
                    //Insert Stock Summary



                    //List<TblStockDetailsTO> tblStockDetailsTOList = BL.TblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);
                    //if (tblStockDetailsTOList == null)
                    if (true)
                    {

                        //    stockList.Add(tblStockDetailsTONew);
                        //    stockDtlId = tblStockDetailsTONew.IdStockDtl;
                        //}
                        //else
                        //{
                        //    result = TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTONew, conn, tran);
                        //    if (result != 1)
                        //    {
                        //        tran.Rollback();
                        //        resultMessage.MessageType = ResultMessageE.Error;
                        //        resultMessage.Text = "Error : While update the stock details";
                        //        resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while update the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                        //        resultMessage.Result = 0;
                        //        return resultMessage;
                        //    }
                        //}
                        //Update Stock Summary



                        //Insert Into the TblStockConsumption.

                        TblStockConsumptionTO stockConsumptionTO = new TblStockConsumptionTO();
                        stockConsumptionTO.BeforeStockQty = tblStockDetailsTONew.BalanceStock + totalLoadingQty;
                        stockConsumptionTO.AfterStockQty = tblStockDetailsTONew.BalanceStock;
                        stockConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                        stockConsumptionTO.CreatedBy = loginUserId;
                        stockConsumptionTO.CreatedOn = Constants.ServerDateTime;
                        stockConsumptionTO.Remark = totalLoadingQty + " Qty is consumed against Loading Slip : " + tblLoadingTO.LoadingSlipNo;
                        stockConsumptionTO.StockDtlId = tblStockDetailsTONew.IdStockDtl;
                        stockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
                        stockConsumptionTO.TxnQty = -totalLoadingQty;

                        result = BL.TblStockConsumptionBL.InsertTblStockConsumption(stockConsumptionTO, conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour();
                            resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                            return resultMessage;
                        }

                    }
            }
             resultMessage.DefaultSuccessBehaviour();
             return resultMessage;
            }

            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method UpdateStockAgainstItem";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                //conn.Close();
            }
        }

        private static ResultMessage InsertItemIntoStockDtlAndSummary(TblLoadingTO tblLoadingTO, Double totalLoadingQty, SqlConnection conn, SqlTransaction tran, TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 loginUserId, List<TblLocationTO> tblLocationTOList)
        {
            Int32 isAllowLoading = 0;
            Int32 stockDtlId = 0;
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            try
            {
                TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();

                //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
                if (tblConfigParamsTO != null)
                {
                    isAllowLoading = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                }
                if (isAllowLoading == 1)
                {
                    List<TblStockDetailsTO> stockList = TblStockDetailsDAO.SelectAllTblStockDetailsOther(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime(), conn, tran);
                    String isItemized = "Itemized";

                    stockList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                                     && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                                     && l.MaterialId == tblLoadingSlipExtTO.MaterialId
                                                                                     && l.BrandId == tblLoadingSlipExtTO.BrandId
                                                                                     && l.ProdItemId == tblLoadingSlipExtTO.ProdItemId
                                                                                     && l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList();

                    List<TblProductInfoTO> productList = BL.TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);

                    if (stockList.Count == 0)
                    {
                        // For weight and Stock in MT calculations
                        tblStockDetailsTONew.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
                        tblStockDetailsTONew.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
                        tblStockDetailsTONew.BrandId = tblLoadingSlipExtTO.BrandId;
                        tblStockDetailsTONew.MaterialId = tblLoadingSlipExtTO.MaterialId;
                        tblStockDetailsTONew.CreatedOn = Constants.ServerDateTime;
                        tblStockDetailsTONew.CreatedBy = loginUserId;
                        tblStockDetailsTONew.ProdItemId = tblLoadingSlipExtTO.ProdItemId;

                        if (tblLoadingSlipExtTO.CompartmentId > 0)
                        {
                            tblStockDetailsTONew.LocationId = tblLoadingSlipExtTO.CompartmentId;
                        }
                        else
                        {
                            Int32 tempLocatId = 0;

                            var tempLocationLst = tblLocationTOList.Where(w => w.ParentLocId > 0).ToList();
                            if (tempLocationLst != null && tempLocationLst.Count > 0)
                            {
                                tempLocatId = tempLocationLst[0].IdLocation;
                            }

                            tblStockDetailsTONew.LocationId = tempLocatId;
                        }


                        TblStockSummaryTO tblStockSummaryTO = BL.TblStockSummaryBL.SelectTblStockSummaryTO(new DateTime());
                        if (tblStockSummaryTO == null)
                        {
                            tblStockSummaryTO = new TblStockSummaryTO();
                            tblStockSummaryTO.StockDate = Constants.ServerDateTime;
                            tblStockSummaryTO.NoOfBundles = 0;
                            tblStockSummaryTO.TotalStock = 0;
                            tblStockSummaryTO.CreatedBy = loginUserId;
                            tblStockSummaryTO.CreatedOn = Constants.ServerDateTime;

                            result = TblStockSummaryBL.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error : While insert the stock summary";
                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }

                        tblStockDetailsTONew.StockSummaryId = tblStockSummaryTO.IdStockSummary;


                        result = TblStockDetailsBL.InsertTblStockDetails(tblStockDetailsTONew, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While insert the stock details";
                            resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    }
                    else
                    {
                        tblStockDetailsTONew = stockList[0];
                    }

                }
                resultMessage.Tag = tblStockDetailsTONew;
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }

            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method UpdateStockAgainstItem";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                //conn.Close();
            }
        }




        private static Double CalculateFreightAmtPerTon(List<TblLoadingSlipTO> list, Double totalFreightAmt)
        {
            try
            {
                Double freightAmt = 0;
                Double totalQtyInMT = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    totalQtyInMT += list[i].TblLoadingSlipDtlTO.LoadingQty;
                }

                freightAmt = totalFreightAmt / totalQtyInMT;
                freightAmt = Math.Round(freightAmt, 2);
                return freightAmt;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {

            }
        }

        public ResultMessage UpdateStockAndConsumptionHistory(TblLoadingSlipExtTO tblLoadingSlipExtTO, TblLoadingTO tblLoadingTO, int stockDtlId, ref Double totalLoadingQty, TblProductInfoTO prodConfgTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            int result = 0;
            Double stockQty = 0;

            //Checked from DB To Get Latest Stock Details
            TblStockDetailsTO stockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO(stockDtlId, conn, tran);
            if (stockDetailsTO.BalanceStock >= totalLoadingQty)
            {
                stockQty = totalLoadingQty;
            }
            else
            {
                stockQty = stockDetailsTO.BalanceStock;
            }

            TblStockConsumptionTO stockConsumptionTO = new TblStockConsumptionTO();
            stockConsumptionTO.BeforeStockQty = stockDetailsTO.BalanceStock;
            stockConsumptionTO.AfterStockQty = stockDetailsTO.BalanceStock - stockQty;
            stockConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
            stockConsumptionTO.CreatedBy = tblLoadingTO.CreatedBy;
            stockConsumptionTO.CreatedOn = tblLoadingTO.CreatedOn;
            stockConsumptionTO.Remark = stockQty + " Qty is consumed against Loading Slip : " + tblLoadingTO.LoadingSlipNo;
            stockConsumptionTO.StockDtlId = stockDetailsTO.IdStockDtl;
            stockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;
            stockConsumptionTO.TxnQty = -stockQty;

            result = BL.TblStockConsumptionBL.InsertTblStockConsumption(stockConsumptionTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                return resultMessage;
            }

            //Update Stock Balance Qty
            stockDetailsTO.BalanceStock = stockConsumptionTO.AfterStockQty;

            stockDetailsTO.TotalStock = stockConsumptionTO.AfterStockQty;

            if (stockDetailsTO.IsConsolidatedStock == 1)
            {
                stockDetailsTO.NoOfBundles = stockDetailsTO.TotalStock;
            }
            else
            {
                if (stockDetailsTO.TotalStock <= 0)
                {
                    stockDetailsTO.NoOfBundles = 0;
                }
                else
                {
                    Double totalStkInMT = stockDetailsTO.TotalStock;
                    totalStkInMT = totalStkInMT * 1000;
                    if (prodConfgTO == null)
                    {
                        stockDetailsTO.NoOfBundles = stockDetailsTO.TotalStock;
                    }
                    else
                    {
                        Double noOfBundles = Math.Round(totalStkInMT / prodConfgTO.NoOfPcs / prodConfgTO.AvgSecWt / prodConfgTO.StdLength, 2);
                        stockDetailsTO.NoOfBundles = noOfBundles;
                    }

                }
            }
            stockDetailsTO.LoadedStock += stockQty;
            stockDetailsTO.UpdatedBy = tblLoadingTO.CreatedBy;
            stockDetailsTO.UpdatedOn = tblLoadingTO.CreatedOn;
            result = BL.TblStockDetailsBL.UpdateTblStockDetails(stockDetailsTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.Text = "Error : While UpdateTblStockDetails Against LoadingSlip";
                return resultMessage;
            }

            totalLoadingQty = totalLoadingQty - stockQty;
            resultMessage.MessageType = ResultMessageE.Information;
            resultMessage.Text = "Stock consumption marked Sucessfully";
            return resultMessage;
        }
        #endregion

        #region Updation
        public int UpdateTblLoading(TblLoadingTO tblLoadingTO)
        {
            return TblLoadingDAO.UpdateTblLoading(tblLoadingTO);
        }

        public int UpdateTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingDAO.UpdateTblLoading(tblLoadingTO, conn, tran);
        }
        public ResultMessage UpdateDeliverySlipConfirmations(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage = UpdateDeliverySlipConfirmations(tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return resultMessage;

                }
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateDeliverySlipConfirmations");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        public ResultMessage UpdateDeliverySlipConfirmations(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {

                List<TblProductInfoTO> productConfgList = TblProductInfoBL.SelectAllTblProductInfoList(conn, tran);
                if (productConfgList == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error : productConfgList Found NULL ";
                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Product Master Configuration is not completed.";
                    return resultMessage;
                }


                TblLoadingTO existingLoadingTO = SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);
                if (existingLoadingTO == null)
                {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : existingLoadingTO Found NULL ";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)
                {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record could not be updated as selected Loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.DisplayMessage = "Record could not be updated as selected loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region While Delivery OUT check for invoices generated or not.

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)
                {
                    resultMessage = BL.TblWeighingMeasuresBL.CheckInvoiceNoGeneratedByVehicleNo(tblLoadingTO.VehicleNo, conn, tran, true);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        //tran.Rollback();
                        return resultMessage;
                    }

                    // Vijaymala [30-03-2018] added:to update invoice deliveredOn date after loading slip out
                    resultMessage = BL.TblInvoiceBL.UpdateInvoiceAfterloadingSlipOut(tblLoadingTO.IdLoading, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        //tran.Rollback();
                        return resultMessage;
                    }
                }

                #endregion

                #region 0. If User Is Confirming Then Check it can be approve or Not

                //No need to update stock for ODLMS
                //if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM)
                if (false)
                {
                    resultMessage = CanGivenLoadingSlipBeApproved(tblLoadingTO, conn, tran);
                    if (resultMessage.MessageType == ResultMessageE.Information
                        && resultMessage.Result == 1)
                    {
                        // Give Stock Effects
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblLoadingSlipExtTO>))
                        {
                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = (List<TblLoadingSlipExtTO>)resultMessage.Tag;

                            for (int stk = 0; stk < loadingSlipExtTOList.Count; stk++)
                            {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = loadingSlipExtTOList[stk];


                                //Check If Stock exist Or Not
                                List<TblStockDetailsTO> stockList = TblStockDetailsDAO.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingTO.CreatedOn, conn, tran);

                                if (stockList == null)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : stockList Found NULL ";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }

                                var prodConfgTO = productConfgList.Where(p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                                            p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                                            p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                            && p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();

                                if (prodConfgTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialId + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatId + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecId;
                                    resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                    return resultMessage;
                                }

                                // Create Stock Consumption History Record
                                var stkConsList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId).ToList();

                                Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                for (int s = 0; s < stkConsList.Count; s++)
                                {

                                    if (totalLoadingQty > 0)
                                    {
                                        resultMessage = UpdateStockAndConsumptionHistory(tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, prodConfgTO, conn, tran);
                                        if (resultMessage.MessageType != ResultMessageE.Information)
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip Confirmation";
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            return resultMessage;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }

                #endregion

                #region 1. Stock Calculations If Cancelling Loading
                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                {

                    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
                    tblWeighingMeasuresTOList = TblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading);

                    if (tblWeighingMeasuresTOList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;

                        //Priyanka [16-04-2018] : Added for if the tare weight taken allow to cancel loading slip.

                        Int32 allowToCancelIfTareWT = 0;

                        TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ALLOW_TO_CANCEL_LOADING_IF_TARE_WT_TAKEN, conn, tran);
                        if (tblConfigParamsTO != null)
                        {
                            allowToCancelIfTareWT = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                        }

                        if (allowToCancelIfTareWT == 1)
                        {
                            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOListTare = tblWeighingMeasuresTOList.Where(w => w.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT).ToList();

                            if (tblWeighingMeasuresTOList.Count > tblWeighingMeasuresTOListTare.Count)
                            {
                                resultMessage.Text = "Vehicle Weighing already done can not Cancel";
                                return resultMessage;
                            }

                        }
                        else
                        {

                            if (tblWeighingMeasuresTOList.Count == 1)
                            {
                                resultMessage.Text = "Vehicle Tare weight already done can not Cancel";
                            }
                            else if (tblWeighingMeasuresTOList.Count > 1)
                            {
                                resultMessage.Text = "Vehicle Weighing already done can not Cancel";
                            }
                            return resultMessage;
                        }
                    }



                    //if (tblWeighingMeasuresTOList.Count == 0)
                    if (true)
                    {
                        // tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);

                        if (tblLoadingTO.LoadingTypeE != Constants.LoadingTypeE.OTHER)
                        {
                            #region 2.1 Reverse Booking Pending Qty
                            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0)
                            {
                                //tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                resultMessage.Text = "loadingSlipDtlTOList found null";
                                return resultMessage;
                            }

                            var distinctBookings = loadingSlipDtlTOList.GroupBy(b => b.BookingId).ToList();
                            for (int i = 0; i < distinctBookings.Count; i++)
                            {
                                Int32 bookingId = distinctBookings[i].Key;
                                Double bookingQty = loadingSlipDtlTOList.Where(b => b.BookingId == bookingId).Sum(l => l.LoadingQty);

                                //Call to update pending booking qty for loading
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(bookingId, conn, tran);
                                if (tblBookingsTO == null)
                                {
                                    //tran.Rollback();
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                    return resultMessage;
                                }

                                tblBookingsTO.IdBooking = bookingId;
                                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                if (tblBookingsTO.PendingQty < 0)
                                {
                                    //tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                                    return resultMessage;
                                }

                                result = BL.TblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                                if (result != 1)
                                {
                                    //tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }
                            #endregion

                            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0)
                            {
                                //tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                resultMessage.Text = "loadingSlipExtTOList found null";
                                return resultMessage;
                            }

                            //TblLoadingTO existingLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);

                            for (int i = 0; i < loadingSlipExtTOList.Count; i++)
                            {
                                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                                //TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
                                //if (tblLoadingQuotaDeclarationTO == null)
                                //{
                                //    //tran.Rollback();
                                //    resultMessage.DefaultBehaviour();
                                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                //    resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                                //    return resultMessage;
                                //}

                                //// Update Loading Quota For Balance Qty
                                //Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                //tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                                //tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                //tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                //result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                                //if (result != 1)
                                //{
                                //    resultMessage.DefaultBehaviour();
                                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                //    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                                //    return resultMessage;
                                //}

                                ////History Record For Loading Quota consumptions
                                //TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO();
                                //consumptionTO.AvailableQuota = balanceQuota;
                                //consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                //consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                                //consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                                //consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                                //consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                //consumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                                //consumptionTO.QuotaQty = quotaQty;
                                //consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                                //result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
                                //if (result != 1)
                                //{
                                //    resultMessage.DefaultBehaviour();
                                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                //    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                                //    return resultMessage;
                                //}

                                // Update Stock i.e reverse stock. If It is confirmed loading slips

                                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_NOT_CONFIRM
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)

                                {

                                    List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList(loadingSlipExtId, (int)Constants.TxnOperationTypeE.OUT, conn, tran);
                                    if (tblStockConsumptionTOList == null)
                                    {
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                                        return resultMessage;
                                    }

                                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++)
                                    {
                                        Double qtyToReverse = Math.Abs(tblStockConsumptionTOList[s].TxnQty);
                                        TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO(tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                                        if (tblStockDetailsTO == null)
                                        {
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                                        tblStockDetailsTO.TotalStock = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                        result = BL.TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
                                        if (result != 1)
                                        {
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        // Insert Stock Consumption History Record
                                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO();
                                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                                        reversedStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;

                                        result = BL.TblStockConsumptionBL.InsertTblStockConsumption(reversedStockConsumptionTO, conn, tran);
                                        if (result != 1)
                                        {
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }
                                    }
                                }
                            }

                            #endregion

                        }
                    }


                }


                #endregion
                #region 2. Update Loading Slip Status
                //Update LoadingTO Status First
                result = UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                if (result <= 0)
                {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                    return resultMessage;
                }
                #endregion

                #region 3. Create History Record

                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO();
                tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                if (result != 1)
                {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                    return resultMessage;
                }

                #endregion

                #region 4. Notifications For Approval Or Information
                //Vijaymala added[03-05-2018]to change  notification with party name
                TblConfigParamsTO dealerNameConfTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);
                Int32 dealerNameActive = 0;
                if (dealerNameConfTO != null)
                    dealerNameActive = Convert.ToInt32(dealerNameConfTO.ConfigParamVal);

                String dealerOrgNames = String.Empty;
                List<TblLoadingSlipTO> tblLoadingSlipTOList = BL.TblLoadingSlipBL.SelectAllTblLoadingSlip(tblLoadingTO.IdLoading, conn, tran);
                if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 1)
                {
                    List<TblLoadingSlipTO> distinctLoadingSlipList = tblLoadingSlipTOList.GroupBy(w => w.DealerOrgId).Select(s => s.FirstOrDefault()).ToList();
                    if (distinctLoadingSlipList != null && distinctLoadingSlipList.Count > 0)
                    {
                        distinctLoadingSlipList.ForEach(f => f.DealerOrgName = f.DealerOrgName.Replace(',', ' '));
                        dealerOrgNames = String.Join(" , ", distinctLoadingSlipList.Select(s => s.DealerOrgName.ToString()).ToArray());
                    }

                }

             


                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED||

                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN)
                {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                    List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblLoadingTO.CnfOrgId, conn, tran);
                    if (cnfUserList != null && cnfUserList.Count > 0)
                    {
                        for (int a = 0; a < cnfUserList.Count; a++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }

                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMED;
                        tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CONFIRMED";
                        tblAlertInstanceTO.AlertComment = "Not confirmed loading slip  " + tblLoadingTO.LoadingSlipNo + "  For Vehicle No :" + tblLoadingTO.VehicleNo + "  is approved";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CONFIRMED";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                        if (cnfDCT != null)
                        {
                            foreach (var item in cnfDCT.Keys)
                            {
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = cnfDCT[item];
                                smsTO.SourceTxnDesc = "LOADING_SLIP_CONFIRMED";
                                smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                                tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            }
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                        if (result < 0)
                        {
                            //tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }

                    }
                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                        tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                        tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is cancelled due to " + tblLoadingTO.StatusReason;

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                        //SMS is not required for loading slip cancellation. Notification is already sent
                        //Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                        //if (cnfDCT != null)
                        //{
                        //    foreach (var item in cnfDCT.Keys)
                        //    {
                        //        TblSmsTO smsTO = new TblSmsTO();
                        //        smsTO.MobileNo = cnfDCT[item];
                        //        smsTO.SourceTxnDesc = "LOADING_SLIP_CANCELLED";
                        //        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                        //        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        //    }
                        //}

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                        if (result < 0)
                        {
                            //tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    }
                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_OUT_FOR_DELIVERY;
                        tblAlertInstanceTO.AlertAction = "VEHICLE_OUT_FOR_DELIVERY";
                        tblAlertInstanceTO.AlertComment = "Your Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  of Vehicle No " + tblLoadingTO.VehicleNo + " is out for delivery";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "VEHICLE_OUT_FOR_DELIVERY";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();


                        //SMS to Dealer
                        Dictionary<int, string> dealerDCT = BL.TblLoadingSlipBL.SelectRegMobileNoDCTForLoadingDealers(tblLoadingTO.IdLoading.ToString(), conn, tran);
                        if (dealerDCT != null)
                        {
                            foreach (var item in dealerDCT.Keys)
                            {
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = dealerDCT[item];
                                smsTO.SourceTxnDesc = "VEHICLE_OUT_FOR_DELIVERY";
                                smsTO.SmsTxt = "Your Loading Slip Ref. " + tblLoadingTO.LoadingSlipNo + " is out for delivery";
                                tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            }
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                    }

                    //Priyanka [09-10-2018] - Added to send notifications to persons about vehicle status like vehicle
                    //                        gate in, reported, clearance to send in for loading. 

                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_GATE_IN;
                        tblAlertInstanceTO.AlertAction = "LOADING_GATE_IN";
                        tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is gate in for loading.";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_GATE_IN";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }

                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_REPORTED_FOR_LOADING;
                        tblAlertInstanceTO.AlertAction = "VEHICLE_REPORTED_FOR_LOADING";
                        tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is reported for loading.";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "VEHICLE_REPORTED_FOR_LOADING";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }

                    
                     else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_VEHICLE_CLEARANCE_TO_SEND_IN;
                        tblAlertInstanceTO.AlertAction = "LOADING_VEHICLE_CLEARANCE_TO_SEND_IN";
                        tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is clear to send in for loading.";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_VEHICLE_CLEARANCE_TO_SEND_IN";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }
                    tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                    tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;
                    ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Tag = tblAlertInstanceTO;
                        return resultMessage;
                    }

                }

                #endregion


                #region 5.Insert Vehicle Ext Details

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN)
                {
                    resultMessage = InsertLoadingVehDocExtDetailsAgainstLoading(tblLoadingTO, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }

                }

                #endregion


                //tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Loading Slip Approved Sucessfully";
                resultMessage.DisplayMessage = "Loading Slip Approved Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod UpdateDeliverySlipConfirmations";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }

        }


        public ResultMessage InsertLoadingVehDocExtDetailsAgainstLoading(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage = InsertLoadingVehDocExtDetailsAgainstLoading(tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return resultMessage;

                }
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "InsertLoadingVehDocExtDetailsAgainstLoading");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        private static ResultMessage InsertLoadingVehDocExtDetailsAgainstLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result;


            if (tblLoadingTO.LoadingVehDocExtTOList != null && tblLoadingTO.LoadingVehDocExtTOList.Count > 0)
            {
                tblLoadingTO.LoadingVehDocExtTOList[0].IsActive = 0;

                tblLoadingTO.LoadingVehDocExtTOList[0].LoadingId = tblLoadingTO.IdLoading;
                tblLoadingTO.LoadingVehDocExtTOList[0].CreatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingTO.LoadingVehDocExtTOList[0].CreatedOn = tblLoadingTO.UpdatedOn;
                tblLoadingTO.LoadingVehDocExtTOList[0].UpdatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingTO.LoadingVehDocExtTOList[0].UpdatedOn = tblLoadingTO.UpdatedOn;

                result = TblLoadingVehDocExtBL.UpdateTblLoadingVehDocExtActiveYn(tblLoadingTO.LoadingVehDocExtTOList[0], conn, tran);
                if (result == -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While updating the UpdateTblLoadingVehDocExtActiveYn";
                    resultMessage.DisplayMessage = "Error 02:" + resultMessage.Text;
                    return resultMessage;
                }

                for (int v = 0; v < tblLoadingTO.LoadingVehDocExtTOList.Count; v++)
                {
                    tblLoadingTO.LoadingVehDocExtTOList[v].LoadingId = tblLoadingTO.IdLoading;
                    tblLoadingTO.LoadingVehDocExtTOList[v].CreatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingTO.LoadingVehDocExtTOList[v].CreatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingTO.LoadingVehDocExtTOList[v].UpdatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingTO.LoadingVehDocExtTOList[v].UpdatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingTO.LoadingVehDocExtTOList[v].IsActive = 1;
                }

                tblLoadingTO.LoadingVehDocExtTOList = tblLoadingTO.LoadingVehDocExtTOList.Where(w => w.IsAvailable == 1).ToList();

                result = TblLoadingVehDocExtBL.InsertTblLoadingVehDocExt(tblLoadingTO.LoadingVehDocExtTOList, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Error While Inserting the InsertTblLoadingVehDocExt";
                    resultMessage.DisplayMessage = "Error 03:" + resultMessage.Text;
                    return resultMessage;
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage RestorePreviousStatusForLoading(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                DimStatusTO statusTO = DAL.DimStatusDAO.SelectDimStatus(tblLoadingTO.StatusId, conn, tran);
                if (statusTO == null || statusTO.PrevStatusId == 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error...statusTO Found NULL In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }

                tblLoadingTO.StatusId = statusTO.PrevStatusId;
                #region 2. Update Loading Slip Status
                //Update LoadingTO Status First
                result = UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }
                #endregion

                #region 3. Create History Record

                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO();
                tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason + " Reversed";
                result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod RestorePreviousStatusForLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage CancelAllNotConfirmedLoadingSlips()
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SYTEM_ADMIN_USER_ID, conn, tran);
                Int32 sysAdminUserId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;

                #region 1. Loading Slip Cancellation

                TblConfigParamsTO cancelConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS, conn, tran);
                List<TblLoadingTO> loadingTOListToCancel = TblLoadingDAO.SelectAllLoadingListByStatus(cancelConfigParamsTO.ConfigParamVal, conn, tran);

                if (loadingTOListToCancel != null)
                {

                    for (int ic = 0; ic < loadingTOListToCancel.Count; ic++)
                    {
                        TblLoadingTO tblLoadingTO = loadingTOListToCancel[ic];
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CANCEL;
                        tblLoadingTO.UpdatedBy = sysAdminUserId;
                        tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                        tblLoadingTO.StatusDate = tblLoadingTO.UpdatedOn;
                        tblLoadingTO.StatusReason = "No Actions - Auto Cancelled";

                        #region 1. Stock Calculations If Cancelling Loading
                        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                        {
                            #region 2.1 Reverse Booking Pending Qty

                            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "loadingSlipDtlTOList found null";
                                return resultMessage;
                            }

                            var distinctBookings = loadingSlipDtlTOList.GroupBy(b => b.BookingId).ToList();
                            for (int i = 0; i < distinctBookings.Count; i++)
                            {
                                Int32 bookingId = distinctBookings[i].Key;
                                Double bookingQty = loadingSlipDtlTOList.Where(b => b.BookingId == bookingId).Sum(l => l.LoadingQty);

                                //Call to update pending booking qty for loading
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(bookingId, conn, tran);
                                if (tblBookingsTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                    return resultMessage;
                                }

                                tblBookingsTO.IdBooking = bookingId;
                                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                if (tblBookingsTO.PendingQty < 0)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                                    return resultMessage;
                                }

                                result = BL.TblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                                    return resultMessage;
                                }
                            }

                            #endregion

                            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.Text = "loadingSlipExtTOList found null";
                                return resultMessage;
                            }

                            TblLoadingTO existingLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);

                            for (int i = 0; i < loadingSlipExtTOList.Count; i++)
                            {
                                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                                TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
                                if (tblLoadingQuotaDeclarationTO == null)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                                    return resultMessage;
                                }

                                // Update Loading Quota For Balance Qty
                                Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                                tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                                    return resultMessage;
                                }

                                //History Record For Loading Quota consumptions
                                TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO();
                                consumptionTO.AvailableQuota = balanceQuota;
                                consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                                consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                                consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                                consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                consumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                                consumptionTO.QuotaQty = quotaQty;
                                consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                                result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour();
                                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                                    return resultMessage;
                                }

                                // Update Stock i.e reverse stock. If It is confirmed loading slips

                                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING
                                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN
                                    )
                                {

                                    List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList(loadingSlipExtId, (int)Constants.TxnOperationTypeE.OUT, conn, tran);
                                    if (tblStockConsumptionTOList == null)
                                    {
                                        resultMessage.DefaultBehaviour();
                                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                                        return resultMessage;
                                    }

                                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++)
                                    {
                                        Double qtyToReverse = Math.Abs(tblStockConsumptionTOList[s].TxnQty);
                                        TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO(tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                                        if (tblStockDetailsTO == null)
                                        {
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                        result = BL.TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
                                        if (result != 1)
                                        {
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        // Insert Stock Consumption History Record
                                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO();
                                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                                        reversedStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;

                                        result = BL.TblStockConsumptionBL.InsertTblStockConsumption(reversedStockConsumptionTO, conn, tran);
                                        if (result != 1)
                                        {
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        #endregion

                        #region 2. Update Loading Slip Status
                        //Update LoadingTO Status First
                        result = UpdateTblLoading(tblLoadingTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }

                        //Update Individual Loading Slip statuses
                        result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                        if (result <= 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }
                        #endregion

                        #region 3. Create History Record

                        TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO();
                        tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                        tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                        tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                        tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                        tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                        tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                        result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }

                        #endregion

                        #region 4. Notifications For Approval Or Information
                        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                        {
                            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                            List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                            List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblLoadingTO.CnfOrgId, conn, tran);
                            if (cnfUserList != null && cnfUserList.Count > 0)
                            {
                                for (int a = 0; a < cnfUserList.Count; a++)
                                {
                                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                    tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                                    tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                                }
                            }


                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                            tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                            tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is auto cancelled ";
                            tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                            //SMS Not required in auto cancellation. Discussed in meeting 
                            //Dictionary<int, string> cnfDCT = BL.TblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                            //if (cnfDCT != null)
                            //{
                            //    foreach (var item in cnfDCT.Keys)
                            //    {
                            //        TblSmsTO smsTO = new TblSmsTO();
                            //        smsTO.MobileNo = cnfDCT[item];
                            //        smsTO.SourceTxnDesc = "LOADING_SLIP_CANCELLED";
                            //        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                            //        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            //    }
                            //}

                            tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                            result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                            if (result < 0)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While Reseting Prev Alert";
                                return resultMessage;
                            }

                            tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                            tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                            tblAlertInstanceTO.IsActive = 1;
                            tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                            tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                            tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                            tblAlertInstanceTO.IsAutoReset = 1;
                            ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                            if (rMessage.MessageType != ResultMessageE.Information)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                                resultMessage.Tag = tblAlertInstanceTO;
                                return resultMessage;
                            }
                        }

                        #endregion
                    }
                }

                #endregion

                //#region 2. Loading Slip Auto Postpone

                //TblConfigParamsTO postponeConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID, conn, tran);
                //List<TblLoadingTO> loadingTOListToPostpone = TblLoadingDAO.SelectAllLoadingListByStatus(postponeConfigParamsTO.ConfigParamVal, conn, tran);

                //if (loadingTOListToPostpone == null)
                //{

                //    for (int ic = 0; ic < loadingTOListToPostpone.Count; ic++)
                //    {
                //        TblLoadingTO tblLoadingTO = loadingTOListToPostpone[ic];
                //        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_POSTPONED;
                //        tblLoadingTO.UpdatedBy = sysAdminUserId;
                //        tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                //        tblLoadingTO.StatusDate = tblLoadingTO.UpdatedOn;
                //        tblLoadingTO.StatusReason = "No Actions - Auto Postponed For Tommorow";

                //        #region 1. Stock Calculations If Cancelling Loading
                //        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                //        {
                //            #region 2.1 Reverse Booking Pending Qty

                //            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                //            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.DefaultBehaviour();
                //                resultMessage.Text = "loadingSlipDtlTOList found null";
                //                return resultMessage;
                //            }

                //            var distinctBookings = loadingSlipDtlTOList.GroupBy(b => b.BookingId).ToList();
                //            for (int i = 0; i < distinctBookings.Count; i++)
                //            {
                //                Int32 bookingId = distinctBookings[i].Key;
                //                Double bookingQty = loadingSlipDtlTOList.Where(b => b.BookingId == bookingId).Sum(l => l.LoadingQty);

                //                //Call to update pending booking qty for loading
                //                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                //                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(bookingId, conn, tran);
                //                if (tblBookingsTO == null)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.MessageType = ResultMessageE.Error;
                //                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                //                    return resultMessage;
                //                }

                //                tblBookingsTO.IdBooking = bookingId;
                //                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                //                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                //                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                //                if (tblBookingsTO.PendingQty < 0)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.MessageType = ResultMessageE.Error;
                //                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                //                    return resultMessage;
                //                }

                //                result = BL.TblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                //                if (result != 1)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.MessageType = ResultMessageE.Error;
                //                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                //                    return resultMessage;
                //                }
                //            }

                //            #endregion

                //            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                //            List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                //            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.DefaultBehaviour();
                //                resultMessage.Text = "loadingSlipExtTOList found null";
                //                return resultMessage;
                //            }

                //            TblLoadingTO existingLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);

                //            for (int i = 0; i < loadingSlipExtTOList.Count; i++)
                //            {
                //                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                //                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                //                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                //                TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL.TblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
                //                if (tblLoadingQuotaDeclarationTO == null)
                //                {
                //                    tran.Rollback();
                //                    resultMessage.DefaultBehaviour();
                //                    resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                //                    return resultMessage;
                //                }

                //                // Update Loading Quota For Balance Qty
                //                Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                //                tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                //                tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                //                tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                //                result = BL.TblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
                //                if (result != 1)
                //                {
                //                    resultMessage.DefaultBehaviour();
                //                    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                //                    return resultMessage;
                //                }

                //                //History Record For Loading Quota consumptions
                //                TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO();
                //                consumptionTO.AvailableQuota = balanceQuota;
                //                consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                //                consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                //                consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                //                consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                //                consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                //                consumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;
                //                consumptionTO.QuotaQty = quotaQty;
                //                consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                //                result = BL.TblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
                //                if (result != 1)
                //                {
                //                    resultMessage.DefaultBehaviour();
                //                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                //                    return resultMessage;
                //                }

                //                // Update Stock i.e reverse stock. If It is confirmed loading slips

                //                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN
                //                    || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING)
                //                {

                //                    List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList(loadingSlipExtId, (int)Constants.TxnOperationTypeE.OUT, conn, tran);
                //                    if (tblStockConsumptionTOList == null)
                //                    {
                //                        resultMessage.DefaultBehaviour();
                //                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                //                        return resultMessage;
                //                    }

                //                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++)
                //                    {
                //                        Double qtyToReverse = Math.Abs(tblStockConsumptionTOList[s].TxnQty);
                //                        TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO(tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                //                        if (tblStockDetailsTO == null)
                //                        {
                //                            resultMessage.DefaultBehaviour();
                //                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                //                            return resultMessage;
                //                        }

                //                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                //                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                //                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                //                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                //                        result = BL.TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
                //                        if (result != 1)
                //                        {
                //                            resultMessage.DefaultBehaviour();
                //                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                //                            return resultMessage;
                //                        }

                //                        // Insert Stock Consumption History Record
                //                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO();
                //                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                //                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                //                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                //                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                //                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                //                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                //                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                //                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                //                        reversedStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;

                //                        result = BL.TblStockConsumptionBL.InsertTblStockConsumption(reversedStockConsumptionTO, conn, tran);
                //                        if (result != 1)
                //                        {
                //                            resultMessage.DefaultBehaviour();
                //                            resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                //                            return resultMessage;
                //                        }
                //                    }
                //                }
                //            }

                //            #endregion
                //        }

                //        #endregion

                //        #region 2. Update Loading Slip Status
                //        //Update LoadingTO Status First
                //        result = UpdateTblLoading(tblLoadingTO, conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                //            return resultMessage;
                //        }

                //        //Update Individual Loading Slip statuses
                //        result = TblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                //        if (result <= 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                //            return resultMessage;
                //        }
                //        #endregion

                //        #region 3. Create History Record

                //        TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO();
                //        tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                //        tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                //        tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                //        tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                //        tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                //        tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                //        result = TblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                //            return resultMessage;
                //        }

                //        #endregion

                //        #region 4. Notifications For Approval Or Information
                //        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                //        {
                //            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                //            List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();

                //            List<TblUserTO> cnfUserList = BL.TblUserBL.SelectAllTblUserList(tblLoadingTO.CnfOrgId, conn, tran);
                //            if (cnfUserList != null && cnfUserList.Count > 0)
                //            {
                //                for (int a = 0; a < cnfUserList.Count; a++)
                //                {
                //                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                //                    tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                //                    tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                //                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                //                }
                //            }

                //            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                //            tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                //            tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is auto cancelled ";
                //            tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                //            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                //            tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                //            result = BL.TblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, conn, tran);
                //            if (result < 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                resultMessage.Text = "Error While Reseting Prev Alert";
                //                return resultMessage;
                //            }

                //            tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                //            tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                //            tblAlertInstanceTO.IsActive = 1;
                //            tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                //            tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                //            tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                //            tblAlertInstanceTO.IsAutoReset = 1;
                //            ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //            if (rMessage.MessageType != ResultMessageE.Information)
                //            {
                //                tran.Rollback();
                //                resultMessage.MessageType = ResultMessageE.Error;
                //                resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                //                resultMessage.Tag = tblAlertInstanceTO;
                //                return resultMessage;
                //            }
                //        }

                //        #endregion
                //    }
                //}

                //#endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod CancelAllNotConfirmedLoadingSlips";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResultMessage CanGivenLoadingSlipBeApproved(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return CanGivenLoadingSlipBeApproved(tblLoadingTO, conn, tran);
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Text = "Loading Slip Can Not Be Approve";
                resultMessage.Exception = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage CanGivenLoadingSlipBeApproved(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                if (tblLoadingSlipExtTOList == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error. Loading Material Not Found";
                    return resultMessage;
                }

                var loadingSlipExtIds = string.Join(",", tblLoadingSlipExtTOList.Select(p => p.IdLoadingSlipExt.ToString()));
                loadingSlipExtIds = loadingSlipExtIds.TrimEnd(',');
                List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList = BL.TblLoadingQuotaDeclarationBL.SelectAllLoadingQuotaDeclListFromLoadingExt(loadingSlipExtIds, conn, tran);
                if (loadingQuotaDeclarationTOList == null)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error. Loading Quota Not Found";
                    return resultMessage;
                }

                var loadingQuotaIds = string.Join(",", loadingQuotaDeclarationTOList.Select(p => p.IdLoadingQuota.ToString()));
                loadingQuotaIds = loadingQuotaIds.TrimEnd(',');

                var listToCheck = tblLoadingSlipExtTOList.Where(q => q.QuotaAfterLoading < 0).ToList().GroupBy(a => new { a.ProdCatId, a.ProdCatDesc, a.ProdSpecId, a.ProdSpecDesc, a.MaterialId, a.MaterialDesc }).Select(a => new { ProdCatId = a.Key.ProdCatId, ProdCatDesc = a.Key.ProdCatDesc, ProdSpecId = a.Key.ProdSpecId, ProdSpecDesc = a.Key.ProdSpecDesc, MaterialId = a.Key.MaterialId, MaterialDesc = a.Key.MaterialDesc, LoadingQty = a.Sum(acs => acs.LoadingQty) }).ToList();

                if (listToCheck != null || listToCheck.Count > 0)
                {
                    Dictionary<Int32, Double> loadingQtyDCT = new Dictionary<int, double>();

                    loadingQtyDCT = BL.TblLoadingSlipExtBL.SelectLoadingQuotaWiseApprovedLoadingQtyDCT(loadingQuotaIds, conn, tran);
                    Boolean isAllowed = true;
                    String reason = "Not Enough Quota For Following Items" + Environment.NewLine;
                    for (int i = 0; i < listToCheck.Count; i++)
                    {
                        //TblLoadingSlipExtTO tblLoadingSlipExtTO = listToCheck[i];

                        var loadingQuotaDeclarationTO = loadingQuotaDeclarationTOList.Where(l => l.ProdCatId == listToCheck[i].ProdCatId
                                                         && l.ProdSpecId == listToCheck[i].ProdSpecId
                                                         && l.MaterialId == listToCheck[i].MaterialId).FirstOrDefault();

                        if (loadingQuotaDeclarationTO.IsActive == 0)
                        {
                            if (isAllowed)
                            {
                                isAllowed = false;
                            }
                            reason += listToCheck[i].MaterialDesc + " " + listToCheck[i].ProdCatDesc + "-" + listToCheck[i].ProdSpecDesc + " R.Q. :" + listToCheck[i].LoadingQty + " has inactive loading quota" + Environment.NewLine;

                        }

                        Double approvedLoadingQty = 0;
                        Double transferedQty = loadingQuotaDeclarationTO.TransferedQuota;
                        Double totalAvailableQty = loadingQuotaDeclarationTO.AllocQuota + loadingQuotaDeclarationTO.ReceivedQuota;
                        if (loadingQtyDCT != null && loadingQtyDCT.ContainsKey(loadingQuotaDeclarationTO.IdLoadingQuota))
                            approvedLoadingQty = loadingQtyDCT[loadingQuotaDeclarationTO.IdLoadingQuota];

                        Double pendingQty = totalAvailableQty - transferedQty - approvedLoadingQty;

                        if (listToCheck[i].LoadingQty > pendingQty)
                        {
                            if (isAllowed)
                            {
                                isAllowed = false;
                            }
                            reason += listToCheck[i].MaterialDesc + " " + listToCheck[i].ProdCatDesc + "-" + listToCheck[i].ProdSpecDesc + " R.Q. :" + listToCheck[i].LoadingQty + " AND A.Q. :" + pendingQty + Environment.NewLine;
                        }
                    }

                    if (!isAllowed)
                    {
                        resultMessage.MessageType = ResultMessageE.Information;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Loading Slip Can Not Be Approve";
                        resultMessage.Tag = reason;
                        return resultMessage;
                    }
                }

                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Result = 1;
                resultMessage.Text = "Loading Slip Can Be Approve";
                resultMessage.Tag = tblLoadingSlipExtTOList;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Text = "Loading Slip Can Not Be Approve";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        public ResultMessage updateLaodingToCallFlag(TblLoadingTO tblLoadingTO)
        {

            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SYTEM_ADMIN_USER_ID, conn, tran);
                Int32 sysAdminUserId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;
                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod RestorePreviousStatusForLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// GJ@20171107 : check the Vehicle is complete the all material weight
        /// </summary>
        /// <param name="tblLoadingTO"></param>
        /// <returns></returns>
        public ResultMessage IsVehicleWaitingForGross(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {

            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            List<TblLoadingTO> loadingToList = new List<TblLoadingTO>();

            try
            {

                loadingToList = TblLoadingDAO.SelectAllLoadingListByVehicleNo(tblLoadingTO.VehicleNo, false, conn, tran);
                if (loadingToList != null && loadingToList.Count > 0)
                {
                    loadingToList.OrderByDescending(p => p.IdLoading);
                    if (loadingToList[0].IdLoading != tblLoadingTO.IdLoading)
                    {
                        resultMessage.DefaultBehaviour("Not able to Remove the Allow one more Loading.");
                        return resultMessage;
                    }
                    TblLoadingTO eleLoadingTo = SelectLoadingTOWithDetails(loadingToList[0].IdLoading);
                    for (int j = 0; j < eleLoadingTo.LoadingSlipList.Count; j++)
                    {
                        TblLoadingSlipTO eleLoadingslipTo = eleLoadingTo.LoadingSlipList[j];
                        for (int k = 0; k < eleLoadingslipTo.LoadingSlipExtTOList.Count; k++)
                        {
                            if (eleLoadingslipTo.LoadingSlipExtTOList[k].WeightMeasureId == 0)
                            {
                                resultMessage.DefaultBehaviour("Weight not loaded for all material");
                                return resultMessage;
                            }
                        }
                    }

                }
                else
                {
                    resultMessage.DefaultBehaviour("Loading Slip List found Null");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod IsVehicleWaitingForGross";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }


        }


        /// <summary>
        /// GJ@20171107 : Get the Last Weighing weight measurement and submit as gross weight
        /// </summary>
        /// <param name="idLoading"></param>
        /// <returns></returns>
        /// 
        public TblWeighingMeasuresTO getWeighingGrossTo(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {

            try
            {
                List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO>();
                // TblWeighingMeasuresTO tblWeighingMeasureTo = new TblWeighingMeasuresTO();
                weighingMeasuresToList = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading, conn, tran);
                if (weighingMeasuresToList.Count > 0)
                {
                    weighingMeasuresToList = weighingMeasuresToList.OrderByDescending(p => p.CreatedOn).ToList();
                    return weighingMeasuresToList[0];
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
        }

        /// <summary>
        /// GJ@20171107 : Remove Allow one more Loading generation flag if required
        /// </summary>
        /// <param name="idLoading"></param>
        /// <returns></returns>
        /// 
        public ResultMessage removeIsAllowOneMoreLoading(TblLoadingTO tblLoadingTO, int loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = BL.TblLoadingBL.IsVehicleWaitingForGross(tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return resultMessage;
                }
                //Insert the Weighing measure Gross To
                TblWeighingMeasuresTO weighingMeasureTo = new TblWeighingMeasuresTO();
                weighingMeasureTo = getWeighingGrossTo(tblLoadingTO, conn, tran);
                if (weighingMeasureTo == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Last weighing weight not found againest selected Loading");
                    return resultMessage;
                }
                weighingMeasureTo.IdWeightMeasure = 0;
                weighingMeasureTo.CreatedOn = Constants.ServerDateTime;
                weighingMeasureTo.UpdatedOn = Constants.ServerDateTime;
                weighingMeasureTo.WeightMeasurTypeId = (int)Constants.TransMeasureTypeE.GROSS_WEIGHT;

                #region 1. Save the Weighing Machine Mesurement 
                result = DAL.TblWeighingMeasuresDAO.InsertTblWeighingMeasures(weighingMeasureTo, conn, tran);
                if (result < 0)
                {
                    tran.Rollback();
                    resultMessage.Text = "";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #endregion

                //Updating Loading Slip flag status
                tblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                //result = BL.TblLoadingBL.UpdateTblLoading(tblLoadingTO);

                TblConfigParamsTO tblConfigParamsTO = BL.TblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
                if (tblConfigParamsTO != null)
                {
                    if (tblConfigParamsTO.ConfigParamVal == "1")
                    {
                        tblLoadingTO.StatusId = (int)Constants.TranStatusE.LOADING_COMPLETED;
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_COMPLETED;
                        tblLoadingTO.StatusReason = "Loading Completed";
                    }
                }

                resultMessage = BL.TblLoadingBL.UpdateDeliverySlipConfirmations(tblLoadingTO, conn, tran);

                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While UpdateTblLoading");
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod removeIsAllowOneMoreLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage UpdateLoadingTransportDetails(TblTransportSlipTO tblTransportSlipTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;

                #region 1.Update Loading Details
                TblLoadingTO tblLoadingTO = SelectTblLoadingTO(tblTransportSlipTO.LoadingId, conn, tran);
                if (tblLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("tblLoadingTO found null");
                    return resultMessage;
                }

                tblLoadingTO.UpdatedBy = tblTransportSlipTO.UpdatedBy;
                tblLoadingTO.UpdatedOn = tblTransportSlipTO.UpdatedOn;
                tblLoadingTO.VehicleNo = tblTransportSlipTO.VehicleNo;
                tblLoadingTO.DriverName = tblTransportSlipTO.DriverName;
                tblLoadingTO.ContactNo = tblTransportSlipTO.ContactNo;
                tblLoadingTO.TransporterOrgId = tblTransportSlipTO.TransporterOrgId;
                result = UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while update UpdateTblLoading");
                    return resultMessage;
                }
                #endregion

                #region 2. Update loading slip details
                List<TblLoadingSlipTO> loadindingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                if (loadindingSlipList == null || loadindingSlipList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while update UpdateTblLoading");
                    return resultMessage;
                }
                foreach (var loadindingSlip in loadindingSlipList)
                {
                    loadindingSlip.VehicleNo = tblTransportSlipTO.VehicleNo;
                    loadindingSlip.DriverName = tblTransportSlipTO.DriverName;
                    loadindingSlip.ContactNo = tblTransportSlipTO.ContactNo;
                    result = BL.TblLoadingSlipBL.UpdateTblLoadingSlip(loadindingSlip, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("error while update loadindingSlip");
                        return resultMessage;
                    }
                }

                #endregion

                #region 3. Update TransportSlip Details
                result = BL.TblTransportSlipBL.UpdateTblTransportSlip(tblTransportSlipTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while Update TblTransportSlip");
                    return resultMessage;
                }

                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateLoadingTransportDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        #endregion


        /// <summary>
        /// Priyanka [18-04-2018]
        /// </summary>
        /// <param name="LoadingTO"></param>
        /// <returns></returns>
        public ResultMessage UpdateVehicleDetails(TblLoadingTO LoadingTO)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;

                #region 1.Update Vehicle Number In Loading Details
                TblLoadingTO tblLoadingTO = SelectTblLoadingTO(LoadingTO.IdLoading, conn, tran);
                if (tblLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("tblLoadingTO found null");
                    return resultMessage;
                }

                tblLoadingTO.UpdatedBy = LoadingTO.UpdatedBy;
                tblLoadingTO.UpdatedOn = LoadingTO.UpdatedOn;
                tblLoadingTO.VehicleNo = LoadingTO.VehicleNo;

                result = UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while update UpdateVehicleDetails");
                    return resultMessage;
                }
                #endregion

                #region 2. Update Vehicle In loading slip details
                List<TblLoadingSlipTO> loadindingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                if (loadindingSlipList == null || loadindingSlipList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while update UpdateTblLoading");
                    return resultMessage;
                }
                foreach (var loadindingSlip in loadindingSlipList)
                {
                    loadindingSlip.VehicleNo = LoadingTO.VehicleNo;

                    result = BL.TblLoadingSlipBL.UpdateTblLoadingSlip(loadindingSlip, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("error while update loadindingSlip");
                        return resultMessage;
                    }
                }
                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in UpdateVehicleDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResultMessage AllocateSuperwisor(TblLoadingTO tblLoadingTO, string loginUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                TblLoadingTO existingTblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingTO.IdLoading,conn,tran);
                if (existingTblLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "existingTblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                existingTblLoadingTO.UpdatedBy = Convert.ToInt32(loginUserId);
                existingTblLoadingTO.UpdatedOn = Constants.ServerDateTime;
                existingTblLoadingTO.SuperwisorId = tblLoadingTO.SuperwisorId;
                int result = BL.TblLoadingBL.UpdateTblLoading(existingTblLoadingTO,conn,tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #region Notifications & SMS

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                //List<TblUserTO> superwisorList = BL.TblUserBL.SelectAllTblUserList(existingTblLoadingTO.SuperwisorId, conn, tran);
                //if (superwisorList != null && superwisorList.Count > 0)
                //{
                //    for (int a = 0; a < superwisorList.Count; a++)
                //    {
                //        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                //        tblAlertUsersTO.UserId = superwisorList[a].IdUser;
                //        tblAlertUsersTO.DeviceId = superwisorList[a].RegisteredDeviceId;
                //        tblAlertUsersTOList.Add(tblAlertUsersTO);
                //    }
                //}

                TblUserTO userTO = TblUserBL.SelectTblUserTO(existingTblLoadingTO.SuperwisorId, conn, tran);
                if (userTO != null)
                {
                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                    tblAlertUsersTO.UserId = userTO.IdUser;
                    tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                }


                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.SUPERWISOR_ALLOCATION_FOR_VEHICLE;
                tblAlertInstanceTO.AlertAction = "SUPERWISOR_ALLOCATION_FOR_VEHICLE";
                tblAlertInstanceTO.AlertComment = "Vehicle Number " + tblLoadingTO.VehicleNo + "  is allocated ";
                tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(12);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "SUPERWISOR_ALLOCATION_FOR_VEHICLE";
                tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                tblAlertInstanceTO.IsAutoReset = 0;
                tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;


                ResultMessage rMessage = BL.TblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {

                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                #endregion

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch(Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "AllocateSuperwisor");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #region Deletion
        public int DeleteTblLoading(Int32 idLoading)
        {
            return TblLoadingDAO.DeleteTblLoading(idLoading);
        }

        public int DeleteTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran)
        {
            return TblLoadingDAO.DeleteTblLoading(idLoading, conn, tran);
        }

        #endregion

        #region Methods

        // Vaibhav [30-Jan-2018] Added to update entity range for loading and loadingslip count.
        private static TblEntityRangeTO SelectEntityRangeForLoadingCount(string entityName, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                TblEntityRangeTO entityRangeTO = BL.TblEntityRangeBL.SelectTblEntityRangeTOByEntityName(entityName, Constants.FinYear, conn, tran);
                if (entityRangeTO == null)
                {
                    return null;
                }

                if (Constants.ServerDateTime.Date != entityRangeTO.CreatedOn.Date)
                {
                    entityRangeTO.CreatedOn = Constants.ServerDateTime;
                    entityRangeTO.EntityPrevValue = 1;

                    int result = BL.TblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                    if (result != 1)
                    {
                        return null;
                    }
                }

                return entityRangeTO;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        /// <summary>
        /// Priyanka [19-04-2018] : Added to remove the item whose weight is not taken.(Edit Loading Slip)
        /// </summary>
        /// <param name="tblLoadingSlipExtTO"></param>
        /// <param name="txnUserId"></param>
        /// <returns></returns>
        public ResultMessage RemoveItemFromLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime txnDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipDAO.SelectTblLoadingSlip(tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblLoadingSlipTO found null");
                    return resultMessage;
                }

                resultMessage = RemoveItemFromLoadingSlip(tblLoadingSlipExtTO, 0, txnUserId, conn, tran);
                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                {

                    #region Check Final Item

                    resultMessage = CheckLoadingStatusAndGenerateInvoice(tblLoadingSlipTO.LoadingId, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }

                    #endregion

                    tran.Commit();
                }
                return resultMessage;
            }

            catch (Exception ex)
            {

                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In MEthod RemoveItemFromLoadingSlip");
                return resultMessage;


            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage RemoveItemFromLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 isForUpdate, Int32 txnUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime txnDateTime = Constants.ServerDateTime;
            try
            {
                int result = 0;


                TblLoadingSlipExtTO existingTblLoadingSlipExtTO = TblLoadingSlipExtBL.SelectTblLoadingSlipExtTO(tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (existingTblLoadingSlipExtTO == null)
                {
                    throw new Exception("existingTblLoadingSlipExtTO == null for IdLoadingSlipExt - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                }

                tblLoadingSlipExtTO = existingTblLoadingSlipExtTO;

                #region 1.Mark Deletion History in tblLoadingSlipRemovedItems

                TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO = tblLoadingSlipExtTO.GetTblLoadingSlipRemovedItemsTO();
                tblLoadingSlipRemovedItemsTO.UpdatedBy = txnUserId;
                tblLoadingSlipRemovedItemsTO.UpdatedOn = txnDateTime;

                result = BL.TblLoadingSlipRemovedItemsBL.InsertTblLoadingSlipRemovedItems(tblLoadingSlipRemovedItemsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Inserting History Record For Deleted Items");
                    return resultMessage;
                }
                #endregion

                #region 2.Delete Item From TblLoadingSlipExtHistoryTO

                result = BL.TblLoadingSlipExtHistoryBL.DeleteLoadingSlipExtHistoryForItem(tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (result <= -1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Deleting Loading Slip History");
                    return resultMessage;
                }
                #endregion

                #region 3. Reverse Booking Pending Qty
                Double bookingQty = tblLoadingSlipExtTO.LoadingQty;

                //Call to update pending booking qty for loading
                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
                tblBookingsTO = BL.TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipExtTO.BookingId, conn, tran);
                if (tblBookingsTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error :tblBookingsTO Found NUll Or Empty");
                    return resultMessage;
                }

                tblBookingsTO.IdBooking = tblLoadingSlipExtTO.BookingId;
                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                tblBookingsTO.UpdatedBy = txnUserId;
                tblBookingsTO.UpdatedOn = txnDateTime;

                if (tblBookingsTO.PendingQty < 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblBookingsTO.PendingQty gone less than 0");
                    return resultMessage;
                }

                result = BL.TblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : While UpdateBookingPendingQty Against Booking");
                    return resultMessage;
                }

                #endregion

                #region 4. Update the stock back and Add new record in tblStockConsumption for reverse stock entry
                TblLoadingTO tblLoadingTO = new TblLoadingTO();
                tblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTOByLoadingSlipId(tblLoadingSlipExtTO.LoadingSlipId);

                List<TblStockConsumptionTO> tblStockConsumptionTOList = BL.TblStockConsumptionBL.SelectAllStockConsumptionList(tblLoadingSlipExtTO.IdLoadingSlipExt, (int)Constants.TxnOperationTypeE.OUT, conn, tran);
                if (tblStockConsumptionTOList == null)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error :tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                    return resultMessage;
                }

                for (int s = 0; s < tblStockConsumptionTOList.Count; s++)
                {
                    tblStockConsumptionTOList[s].LoadingSlipExtId = 0;
                    result = BL.TblStockConsumptionBL.UpdateTblStockConsumption(tblStockConsumptionTOList[s], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblStockConsumption Against LoadingSlip Cancellation";
                        return resultMessage;
                    }


                    Double qtyToReverse = Math.Abs(tblStockConsumptionTOList[s].TxnQty);
                    TblStockDetailsTO tblStockDetailsTO = BL.TblStockDetailsBL.SelectTblStockDetailsTO(tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                    if (tblStockDetailsTO == null)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error : tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                        return resultMessage;
                    }

                    double prevStockQty = tblStockDetailsTO.BalanceStock;
                    tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                    tblStockDetailsTO.TotalStock = tblStockDetailsTO.BalanceStock;
                    tblStockDetailsTO.UpdatedBy = txnUserId;
                    tblStockDetailsTO.UpdatedOn = Constants.ServerDateTime;

                    result = BL.TblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                        return resultMessage;
                    }

                    // Insert Stock Consumption History Record 
                    TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO();
                    reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                    reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                    reversedStockConsumptionTO.CreatedBy = txnUserId;
                    reversedStockConsumptionTO.CreatedOn = Constants.ServerDateTime;
                    reversedStockConsumptionTO.LoadingSlipExtId = tblStockConsumptionTOList[s].LoadingSlipExtId;
                    reversedStockConsumptionTO.Remark = "Item is removed by " + tblLoadingTO.CreatedByUserName + " and Stock is reversed against loadingslip no." + tblLoadingTO.LoadingSlipNo;
                    reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                    reversedStockConsumptionTO.TxnQty = qtyToReverse;
                    reversedStockConsumptionTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.IN;

                    result = BL.TblStockConsumptionBL.InsertTblStockConsumption(reversedStockConsumptionTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                        return resultMessage;
                    }
                }
                #endregion

                #region 5. Recalculate the total loading qty and update it in tempLoading
                //TblLoadingTO tblLoadingTO = new TblLoadingTO();
                //tblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTOByLoadingSlipId(tblLoadingSlipExtTO.LoadingSlipId);

                if (tblLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblLoadingTo found null");
                    return resultMessage;
                }

                tblLoadingTO.TotalLoadingQty = tblLoadingTO.TotalLoadingQty - tblLoadingSlipExtTO.LoadingQty;
                tblLoadingTO.UpdatedBy = txnUserId;
                tblLoadingTO.UpdatedOn = Constants.ServerDateTime;

                result = BL.TblLoadingBL.UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading Against LoadingSlip Cancellation";
                    return resultMessage;
                }
                #endregion

                # region 6. Recalculate the total loading qty and update it in tempLoadingSlipDtl

                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();
                tblLoadingSlipDtlTO = TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipDtlTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblLoadingTo found null");
                    return resultMessage;
                }
                tblLoadingSlipDtlTO.LoadingQty = tblLoadingSlipDtlTO.LoadingQty - tblLoadingSlipExtTO.LoadingQty;

                result = BL.TblLoadingSlipDtlBL.UpdateTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading Against LoadingSlip Cancellation";
                    return resultMessage;
                }

                if (tblLoadingSlipDtlTO.LoadingQty == 0)
                {

                }
                #endregion
                //If the loading slip contains a single item in it then it will remove complete loading slip after item removal.

                #region 7.Delete Item From TblLoadingSlipExtTO

                TblLoadingSlipExtTO tblLoadingSlipExtTOExist = TblLoadingSlipExtBL.SelectTblLoadingSlipExtTO(tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (tblLoadingSlipExtTOExist == null)
                {
                    throw new Exception("tblLoadingSlipExtTOExist  IdLoadingSlipExt -" + tblLoadingSlipExtTOExist.IdLoadingSlipExt);
                }

                if (tblLoadingSlipExtTOExist.LoadedWeight > 0)
                {
                    tran.Rollback();
                    resultMessage.DisplayMessage = "Weighing is already done for this items";
                    resultMessage.Text = "Weighing is already done for this items";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                result = BL.TblLoadingSlipExtBL.DeleteTblLoadingSlipExt(tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Ext");
                    return resultMessage;
                }
                #endregion

                #region 8 Delete the record from loading slip

                if (isForUpdate == 0)
                {

                    TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();
                    tblLoadingSlipTO = BL.TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                    if (tblLoadingSlipTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error : tblLoadingSlipTO found null");
                        return resultMessage;
                    }
                    if (tblLoadingSlipDtlTO.LoadingQty == 0)
                    {
                        #region Delete Slip

                        result = BL.TblLoadingSlipDtlBL.DeleteTblLoadingSlipDtl(tblLoadingSlipDtlTO.IdLoadSlipDtl, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Details.");
                            return resultMessage;
                        }

                        //Delete Address

                        List<TblLoadingSlipAddressTO> tblLoadingSlipAddressTOList = TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(tblLoadingSlipDtlTO.LoadingSlipId, conn, tran);
                        if (tblLoadingSlipAddressTOList != null && tblLoadingSlipAddressTOList.Count > 0)
                        {
                            for (int u = 0; u < tblLoadingSlipAddressTOList.Count; u++)
                            {
                                result = TblLoadingSlipAddressBL.DeleteTblLoadingSlipAddress(tblLoadingSlipAddressTOList[u].IdLoadSlipAddr, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Address Details for IdLoadSlipAddr = " + tblLoadingSlipAddressTOList[u].IdLoadSlipAddr);
                                    return resultMessage;
                                }
                            }

                        }
                        result = BL.TblLoadingSlipBL.DeleteTblLoadingSlip(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting Loading Slip Details");
                            return resultMessage;
                        }

                        #endregion
                    }

                    //2. Delete record from loading.
                    if (tblLoadingTO.TotalLoadingQty == 0)
                    {
                        #region Delete Loading

                        //3. Delete record from loading status history
                        List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO>();
                        tblLoadingStatusHistoryTOList = BL.TblLoadingStatusHistoryBL.SelectAllTblLoadingStatusHistoryList(tblLoadingTO.IdLoading, conn, tran);
                        if (tblLoadingStatusHistoryTOList == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("tblLoadingStatusHistoryTOList found null");
                            return resultMessage;
                        }
                        if (tblLoadingSlipDtlTO.LoadingQty == 0)
                        {

                            foreach (var tblLoadingStatusHistoryTO in tblLoadingStatusHistoryTOList)
                            {

                                result = BL.TblLoadingStatusHistoryBL.DeleteTblLoadingStatusHistory(tblLoadingStatusHistoryTO.IdLoadingHistory, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("Error while delete loadindingSlip status history");
                                    return resultMessage;
                                }

                            }
                        }


                        //4. Delete from Weighing measures.
                        List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
                        tblWeighingMeasuresTOList = BL.TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(tblLoadingTO.IdLoading, conn, tran);
                        if (tblWeighingMeasuresTOList == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("tblWeighingMeasuresTOList found null");
                            return resultMessage;
                        }

                        foreach (var tblWeighingMeasuresTO in tblWeighingMeasuresTOList)
                        {
                            result = BL.TblWeighingMeasuresBL.DeleteTblWeighingMeasures(tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Deleting tblWeighingMeasuresTOList ");
                                return resultMessage;
                            }
                        }



                        result = BL.TblLoadingBL.DeleteTblLoading(tblLoadingTO.IdLoading, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting Loading Slip ");
                            return resultMessage;
                        }

                        #endregion

                    }
                }
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }

            catch (Exception ex)
            {

                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In MEthod RemoveItemFromLoadingSlip");
                return resultMessage;


            }
            finally
            {
                //conn.Close();
            }
        }

        private static ResultMessage CheckLoadingStatusAndGenerateInvoice(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblLoadingTO loadingTOFinal = BL.TblLoadingBL.SelectTblLoadingTO(loadingId, conn, tran);
            if (loadingTOFinal != null)
            {
                Boolean skipInvoiceProcess = false;
                List<TblLoadingSlipTO> loadingSlipTOList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(loadingTOFinal.IdLoading, conn, tran);
                for (int q = 0; q < loadingSlipTOList.Count; q++)
                {
                    TblLoadingSlipTO tblLoadingSlipTOTemp = loadingSlipTOList[q];
                    if (tblLoadingSlipTOTemp.LoadingSlipExtTOList == null || tblLoadingSlipTOTemp.LoadingSlipExtTOList.Count == 0)
                    {
                        skipInvoiceProcess = true;
                        break;
                    }
                    else
                    {
                        List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where(w => w.WeightMeasureId == 0).ToList();
                        if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                        {
                            skipInvoiceProcess = true;
                            break;
                        }
                    }

                }

                if (!skipInvoiceProcess)
                {
                    resultMessage = BL.TblInvoiceBL.PrepareAndSaveNewTaxInvoice(loadingTOFinal, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }
                }
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        /// <summary>
        /// Saket [2018-04-26] Added to add new item in loading slip.
        /// </summary>
        /// <param name="tblLoadingSlipExtTO"></param>
        /// <param name="txnUserId"></param>
        /// <returns></returns>
        public ResultMessage AddItemInLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime txnDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                Int32 isForUpdate = 0;

                //First remove item then add
                if (tblLoadingSlipExtTO.IdLoadingSlipExt > 0)
                {

                    isForUpdate = 1;

                    TblLoadingSlipExtTO temp = tblLoadingSlipExtTO.DeepCopy();

                    resultMessage = RemoveItemFromLoadingSlip(temp, 1,txnUserId, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }
                }
                //Add Item
                resultMessage = AddItemInLoadingSlip(tblLoadingSlipExtTO, txnUserId, isForUpdate, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                tran.Commit();
                return resultMessage;
            }

            catch (Exception ex)
            {

                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In Method AddItemInLoadingSlip");
                return resultMessage;


            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Saket [2018-04-26] Added to add new item in loading slip.
        /// </summary>
        /// <param name="tblLoadingSlipExtTO"></param>
        /// <param name="txnUserId"></param>
        /// <returns></returns>
        public ResultMessage AddItemInLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId, Int32 isForUpdate, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result = 0;

                TblBookingsTO tblBookingsTO = null;

                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();
                if (tblLoadingSlipExtTO.LoadingSlipId > 0)
                {

                    tblLoadingSlipDtlTO = TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                    if (tblLoadingSlipDtlTO == null)
                    {
                        throw new Exception("tblLoadingSlipDtlTO == null for LoadingSlipId - " + tblLoadingSlipExtTO.LoadingSlipId);
                    }

                    tblBookingsTO = TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);

                    if (tblBookingsTO == null)
                    {
                        throw new Exception("tblBookingsTO == null for BookingId -" + tblLoadingSlipDtlTO.BookingId);
                    }

                }
                else
                {
                    tblBookingsTO = TblBookingsBL.SelectTblBookingsTO(tblLoadingSlipExtTO.BookingId, conn, tran);
                    if (tblBookingsTO == null)
                    {
                        throw new Exception("tblBookingsTO == null for BookingId -" + tblLoadingSlipExtTO.BookingId);
                    }
                }



                if (tblBookingsTO == null)
                {
                    throw new Exception("tblBookingsTO == null");
                }

                if (tblLoadingSlipExtTO.LoadingQty > tblBookingsTO.PendingQty)
                {
                    String errorMsg = "Loading qty (" + tblLoadingSlipExtTO.LoadingQty + ") is greater than booking pending qty (" + tblBookingsTO.PendingQty + ")";
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                    return resultMessage;
                }

                TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipDAO.SelectTblLoadingSlip(tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    throw new Exception("tblLoadingSlipTO == null For LoadingSlipId - " + tblLoadingSlipExtTO.LoadingSlipId);
                }


                TblLoadingTO tblLoadingTO = TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId, conn, tran);
                if (tblLoadingTO == null)
                {
                    throw new Exception("tblLoadingTO == null For LoadingId - " + tblLoadingSlipTO.LoadingId);
                }


                #region Validation

                Int32 currentLayerId = tblLoadingSlipExtTO.LoadingLayerid;

                TblLoadingTO loadingTO = TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId, conn, tran);
                if (loadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Error Booking loadingTO is null");
                    return resultMessage;
                }

                // Select temp loading slip details.
                loadingTO.LoadingSlipList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingSlipTO.LoadingId, conn, tran);

                //Already item is added in the loading slip

                TblLoadingSlipTO existingTblLoadingSlipTO = loadingTO.LoadingSlipList.Where(w => w.IdLoadingSlip == tblLoadingSlipExtTO.LoadingSlipId).FirstOrDefault();
                if (existingTblLoadingSlipTO == null)
                {
                    resultMessage.DefaultBehaviour("existingTblLoadingSlipTO == null for LoadingSlipId - " + tblLoadingSlipExtTO.LoadingSlipId);
                    return resultMessage;
                }

                var alreadySpecItemTO = existingTblLoadingSlipTO.LoadingSlipExtTOList.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId &&
                                    w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    w.MaterialId == tblLoadingSlipExtTO.MaterialId
                                    && w.ProdItemId == tblLoadingSlipExtTO.ProdItemId).FirstOrDefault();

                if (alreadySpecItemTO != null)
                {
                    resultMessage.DefaultBehaviour("Item" + tblLoadingSlipExtTO.DisplayName + "is already added into loading slip");
                    return resultMessage;
                }

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOListAll = new List<TblLoadingSlipExtTO>();

                for (int r = 0; r < loadingTO.LoadingSlipList.Count; r++)
                {
                    if (loadingTO.LoadingSlipList[r].LoadingSlipExtTOList != null && loadingTO.LoadingSlipList[r].LoadingSlipExtTOList.Count > 0)
                    {
                        tblLoadingSlipExtTOListAll.AddRange(loadingTO.LoadingSlipList[r].LoadingSlipExtTOList);
                    }
                }

                if (isForUpdate == 1)
                {
                    var currentLayerExtList = tblLoadingSlipExtTOListAll.Where(w => w.LoadingLayerid == currentLayerId).ToList();
                    //if (currentLayerExtList != null && currentLayerExtList.Count == 0)  //These Condition while update item from loading slip with single item.
                    //{
                    //    resultMessage.DefaultBehaviour("Current Layer Ext List not found for layer Id - " + currentLayerId);
                    //    return resultMessage;

                    //}
                    if (currentLayerExtList == null)
                    {
                        currentLayerExtList = new List<TblLoadingSlipExtTO>();
                    }

                    //These Condition while update item from loading slip with single item.
                    if (tblLoadingSlipExtTOListAll != null && tblLoadingSlipExtTOListAll.Count > 0)
                    {
                        var currentLayerExtListWtNotDone = currentLayerExtList.Where(s => s.WeightMeasureId == 0).ToList();
                        if (currentLayerExtListWtNotDone != null && currentLayerExtListWtNotDone.Count > 0)
                        {

                            var currentLayerExtListWtDone = currentLayerExtList.Where(s => s.WeightMeasureId > 0).ToList();

                            if (currentLayerExtListWtDone != null && currentLayerExtListWtDone.Count > 0)
                            {
                                var tempAlreadyWtMeasure = currentLayerExtListWtDone.Where(w => w.BrandId == tblLoadingSlipExtTO.BrandId &&
                                                w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                                w.MaterialId == tblLoadingSlipExtTO.MaterialId).FirstOrDefault();

                                if (tempAlreadyWtMeasure != null)
                                {
                                    resultMessage.DefaultBehaviour("These item weighing is already done againt these layer ");
                                    return resultMessage;
                                }
                            }

                        }
                        else
                        {

                            //Get next loading Ids
                            var nextLayerIdTO = tblLoadingSlipExtTOListAll.Where(w => w.LoadingLayerid > currentLayerId).FirstOrDefault();
                            if (nextLayerIdTO == null)
                            {
                                resultMessage.DefaultBehaviour("Loading is already done against all layer");
                                return resultMessage;
                            }
                            else
                            {
                                Int32 nextLayerId = nextLayerIdTO.LoadingLayerid;

                                var nextLayerExtToList = tblLoadingSlipExtTOListAll.Where(w => w.LoadingLayerid == nextLayerId).ToList();

                                if (nextLayerExtToList != null && nextLayerExtToList.Count > 0)
                                {
                                    nextLayerExtToList = nextLayerExtToList.Where(s => s.WeightMeasureId > 0).ToList();
                                    if (nextLayerExtToList != null && nextLayerExtToList.Count > 0)
                                    {
                                        resultMessage.DefaultBehaviour("Next Layer item weighing is done");
                                        return resultMessage;
                                    }
                                }
                                else
                                {
                                    resultMessage.DefaultBehaviour("Loading is already done against all layer");
                                    return resultMessage;
                                }
                            }

                        }
                    }
                }

                #endregion

                tblLoadingSlipDtlTO.IdBooking = tblLoadingSlipDtlTO.BookingId;
                tblLoadingSlipTO.TblLoadingSlipDtlTO = tblLoadingSlipDtlTO;

                tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();
                tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);

                tblLoadingSlipTO.LoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);




                resultMessage = CalculateLoadingValuesRate(tblLoadingTO);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                Boolean isBoyondLoadingQuota = false;
                Double finalLoadQty = 0;
                resultMessage = InsertLoadingExtDetails(tblLoadingTO, conn, tran, ref isBoyondLoadingQuota, ref finalLoadQty, tblLoadingSlipTO, tblBookingsTO, new List<TblBookingExtTO>());
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                //Update Qty in temploading

                tblLoadingTO = BL.TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTO.LoadingId, conn, tran);
                if (tblLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblLoadingTO found null");
                    return resultMessage;
                }
                tblLoadingTO.TotalLoadingQty = tblLoadingTO.TotalLoadingQty + tblLoadingSlipExtTO.LoadingQty;
                result = BL.TblLoadingBL.UpdateTblLoading(tblLoadingTO, conn, tran);

                //Update Qty in temploadingslipdtl

                tblLoadingSlipDtlTO = TblLoadingSlipDtlBL.SelectLoadingSlipDtlTO(tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipDtlTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblLoadingTo found null");
                    return resultMessage;
                }
                tblLoadingSlipDtlTO.LoadingQty = tblLoadingSlipDtlTO.LoadingQty + tblLoadingSlipExtTO.LoadingQty;

                result = BL.TblLoadingSlipDtlBL.UpdateTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading Against LoadingSlip Cancellation";
                    return resultMessage;
                }

                //Update Qty in booking
                List<TblBookingsTO> tblBookingsTOList = BL.TblBookingsBL.SelectAllBookingsListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                if (tblBookingsTOList == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error : tblBookingsTOList found null");
                }
                foreach (var tblBookingsTONew in tblBookingsTOList)
                {
                    tblBookingsTONew.PendingQty = tblBookingsTONew.PendingQty - tblLoadingSlipExtTO.LoadingQty;
                    result = BL.TblBookingsBL.UpdateTblBookings(tblBookingsTONew, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblBookings ";
                        return resultMessage;
                    }

                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }

            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In Method AddItemInLoadingSlip");
                return resultMessage;
            }
            finally
            {

            }
        }

        public ResultMessage ReverseWeighingProcessAgainstLoading(Int32 loadingId, Int32 txnUserId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            DateTime txnDateTime = Constants.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                //Add Item
                resultMessage = ReverseWeighingProcessAgainstLoading(loadingId, txnUserId, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                tran.Commit();
                return resultMessage;
            }

            catch (Exception ex)
            {

                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In Method ReverseWeighingProcessAgainstLoading");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResultMessage ReverseWeighingProcessAgainstLoading(Int32 loadingId, Int32 txnUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int result = 0;

                TblLoadingTO tblLoadingTO = TblLoadingBL.SelectTblLoadingTO(loadingId, conn, tran);
                if (tblLoadingTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("tblLoadingTO == null for loading Id - " + loadingId);
                    return resultMessage;
                }

                if (tblLoadingTO.StatusId != (int)Constants.TranStatusE.LOADING_GATE_IN)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Loading status in not gate in");
                    return resultMessage;
                }

                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = TblWeighingMeasuresBL.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);

                if (tblWeighingMeasuresTOList == null || tblWeighingMeasuresTOList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Weighing not found against loading");
                    return resultMessage;
                }

                tblWeighingMeasuresTOList = tblWeighingMeasuresTOList.OrderBy(w => w.IdWeightMeasure).ToList();

                TblWeighingMeasuresTO latestTblWeighingMeasuresTO = tblWeighingMeasuresTOList[tblWeighingMeasuresTOList.Count - 1];

                if (latestTblWeighingMeasuresTO.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.GROSS_WEIGHT)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Gross Weight is done against loading");
                    return resultMessage;
                }

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtListByWeighingMeasureId(latestTblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingSlipExtTOList.Count; i++)
                    {
                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipExtTOList[i];

                        //Check invoice is generated against item
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = TblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId(tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                        if (tblInvoiceItemDetailsTO != null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Invoice is generated against last weighing item");
                            return resultMessage;
                        }

                        tblLoadingSlipExtTO.WeightMeasureId = 0;
                        tblLoadingSlipExtTO.LoadedWeight = 0;
                        tblLoadingSlipExtTO.LoadedBundles = 0;
                        tblLoadingSlipExtTO.CalcTareWeight = 0;

                        result = TblLoadingSlipExtBL.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While updating Item loadingSlipExtId = " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                            return resultMessage;
                        }

                    }

                }

                result = TblWeighingMeasuresBL.DeleteTblWeighingMeasures(latestTblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While deleting tare weight against weightMeasureId = " + latestTblWeighingMeasuresTO.IdWeightMeasure);
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }

            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In Method ReverseWeighingProcessAgainstLoading");
                return resultMessage;
            }
            finally
            {

            }
        }
        #endregion
    }
}



