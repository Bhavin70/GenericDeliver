using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL;

namespace ODLMWebAPI.BL
{
    public class TblBookingsBL : ITblBookingsBL
    {
        private readonly ITblBookingsDAO _iTblBookingsDAO;
        private readonly ITblOverdueDtlDAO _iTblOverdueDtlDAO;
        private readonly ITblEnquiryDtlDAO _iTblEnquiryDtlDAO;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblBookingScheduleDAO _iTblBookingScheduleDAO;
        private readonly ITblBookingExtDAO _iTblBookingExtDAO;
        private readonly ITblBookingDelAddrDAO _iTblBookingDelAddrDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblUserAreaAllocationBL _iTblUserAreaAllocationBL;
        private readonly ITblBookingOpngBalDAO _iTblBookingOpngBalDAO;
        private readonly ITblBookingQtyConsumptionDAO _iTblBookingQtyConsumptionDAO;
        private readonly ITblLoadingSlipDtlDAO _iTblLoadingSlipDtlDAO;
        private readonly ITblBookingActionsDAO _iTblBookingActionsDAO;
        private readonly ITblQuotaDeclarationBL _iTblQuotaDeclarationBL;
        private readonly ITblGlobalRateDAO _iTblGlobalRateDAO;
        private readonly ITblOrganizationDAO _iTblOrganizationDAO;
        private readonly IDimBrandDAO _iDimBrandDAO;
        private readonly ITblSysElementsBL _iTblSysElementsBL;
        private readonly ITblBookingParitiesDAO _iTblBookingParitiesDAO;
        private readonly ITblBookingBeyondQuotaDAO _iTblBookingBeyondQuotaDAO;
        private readonly ITblQuotaConsumHistoryDAO _iTblQuotaConsumHistoryDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblOrgOverdueHistoryDAO _iTblOrgOverdueHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        private readonly ICircularDependencyBL _iCircularDependencyBL;
        private readonly ITblPaymentTermOptionRelationBL _iTblPaymentTermOptionRelationBL;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblQuotaDeclarationDAO _iTblQuotaDeclarationDAO;
        private readonly ITblMaterialDAO _iTblMaterialDAO;
        private readonly ITblLoadingSlipExtDAO _iTblLoadingSlipExtDAO;
        public TblBookingsBL(ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblMaterialDAO iTblMaterialDAO, ITblQuotaDeclarationDAO iTblQuotaDeclarationDAO, IDimensionDAO iDimensionDAO, ITblPaymentTermOptionRelationBL iTblPaymentTermOptionRelationBL, ITblOrgOverdueHistoryDAO iTblOrgOverdueHistoryDAO, ITblUserDAO iTblUserDAO, ITblAlertInstanceBL iTblAlertInstanceBL, ITblQuotaConsumHistoryDAO iTblQuotaConsumHistoryDAO, ITblBookingBeyondQuotaDAO iTblBookingBeyondQuotaDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblSysElementsBL iTblSysElementsBL, IDimBrandDAO iDimBrandDAO, ITblOrganizationDAO iTblOrganizationDAO, ITblGlobalRateDAO iTblGlobalRateDAO, ITblQuotaDeclarationBL iTblQuotaDeclarationBL, ITblBookingActionsDAO iTblBookingActionsDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblBookingQtyConsumptionDAO iTblBookingQtyConsumptionDAO, ITblBookingOpngBalDAO iTblBookingOpngBalDAO, ITblUserAreaAllocationBL iTblUserAreaAllocationBL, ICircularDependencyBL iCircularDependencyBL, ITblConfigParamsDAO iTblConfigParamsDAO, ITblBookingDelAddrDAO iTblBookingDelAddrDAO, ITblBookingExtDAO iTblBookingExtDAO, ITblBookingScheduleDAO iTblBookingScheduleDAO, ITblUserRoleBL iTblUserRoleBL, ITblEnquiryDtlDAO iTblEnquiryDtlDAO, ITblOverdueDtlDAO iTblOverdueDtlDAO, ITblBookingsDAO iTblBookingsDAO, ICommon iCommon, IConnectionString iConnectionString)
        {
            _iTblBookingsDAO = iTblBookingsDAO;
            _iTblOverdueDtlDAO = iTblOverdueDtlDAO;
            _iTblEnquiryDtlDAO = iTblEnquiryDtlDAO;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblBookingScheduleDAO = iTblBookingScheduleDAO;
            _iTblBookingExtDAO = iTblBookingExtDAO;
            _iTblBookingDelAddrDAO = iTblBookingDelAddrDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblUserAreaAllocationBL = iTblUserAreaAllocationBL;
            _iTblBookingOpngBalDAO = iTblBookingOpngBalDAO;
            _iTblBookingQtyConsumptionDAO = iTblBookingQtyConsumptionDAO;
            _iTblLoadingSlipDtlDAO = iTblLoadingSlipDtlDAO;
            _iTblBookingActionsDAO = iTblBookingActionsDAO;
            _iTblQuotaDeclarationBL = iTblQuotaDeclarationBL;
            _iTblGlobalRateDAO = iTblGlobalRateDAO;
            _iTblOrganizationDAO = iTblOrganizationDAO;
            _iDimBrandDAO = iDimBrandDAO;
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblBookingParitiesDAO = iTblBookingParitiesDAO;
            _iTblBookingBeyondQuotaDAO = iTblBookingBeyondQuotaDAO;
            _iTblQuotaConsumHistoryDAO = iTblQuotaConsumHistoryDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblUserDAO = iTblUserDAO;
            _iTblOrgOverdueHistoryDAO = iTblOrgOverdueHistoryDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iCircularDependencyBL = iCircularDependencyBL;
            _iTblPaymentTermOptionRelationBL = iTblPaymentTermOptionRelationBL;
            _iDimensionDAO = iDimensionDAO;
            _iTblQuotaDeclarationDAO = iTblQuotaDeclarationDAO;
            _iTblMaterialDAO = iTblMaterialDAO;
            _iTblLoadingSlipExtDAO = iTblLoadingSlipExtDAO;
        }
        #region Selection
        public List<TblBookingPendingRptTO> SelectBookingPendingQryRpt(DateTime fromDate, DateTime toDate, int reportType)
        {

            //fetch all bookings against selected dates
            List<TblBookingsTO> tblBookingTOList = _iTblBookingsDAO.SelectAllBookingDateWise(fromDate, toDate);
            if (reportType == 1)
            {
                tblBookingTOList = tblBookingTOList.Where(x => x.PendingQty > 0).ToList();
            }

            //// created object to get loading details against each booking
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
            List<DropDownTO> MaterialList = _iTblMaterialDAO.SelectAllMaterialForDropDown();
            string configval = "";
            string[] statusId = null;
            int[] myInts = null;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CNF_BOOKING_REPORT_EXCLUDE_STATUSID);
            if (tblConfigParamsTO != null)
            {
                configval = tblConfigParamsTO.ConfigParamVal;

            }
            if (!String.IsNullOrEmpty(configval))
            {
                statusId = configval.Split(',');
                myInts = Array.ConvertAll(statusId, int.Parse);
            }


            // TblBookingsTO tblBookingsTO = new TblBookingsTO();

            List<TblBookingPendingRptTO> tblBookingPendingRptTOList = new List<TblBookingPendingRptTO>();
            if (tblBookingTOList != null && tblBookingTOList.Count > 0)
            {
                Dictionary<int, double> loadingDictionary = null;
                Dictionary<int, double> bookingDictionary = null;
                for (int i = 0; i < tblBookingTOList.Count; i++)
                {

                    // fetch loading qty against each booking consumsion of qty 
                    //  tblLoadingTOList = TblLoadingBL.SelectAllTblLoadingByBookingId(tblBookingTOList[i].IdBooking);
                    List<TblLoadingSlipExtTO> loadingSlipExtList = _iTblLoadingSlipExtDAO.GetAllLoadingExtByBookingId(tblBookingTOList[i].IdBooking, configval);
                    //BookingList with schedule and sizewise qty
                    TblBookingPendingRptTO tblBookingPendingRptTO = new TblBookingPendingRptTO();
                    tblBookingTOList[i].BookingScheduleTOLst = _iCircularDependencyBL.SelectBookingScheduleByBookingId(tblBookingTOList[i].IdBooking);
                    foreach (int a in myInts)
                    {
                        tblBookingTOList.ForEach(x => x.StatusId = a);
                    }
                    tblBookingPendingRptTO.IdBooking = tblBookingTOList[i].IdBooking;
                    tblBookingPendingRptTO.DealerId = tblBookingTOList[i].DealerOrgId;
                    tblBookingPendingRptTO.DealerName = tblBookingTOList[i].DealerName;
                    tblBookingPendingRptTO.TotalBookedQty = tblBookingTOList[i].BookingQty;
                    tblBookingPendingRptTO.PendingQty = tblBookingTOList[i].PendingQty;
                    tblBookingPendingRptTO.CnfName = tblBookingTOList[i].CnfName;

                    foreach (var bookingSchedule in tblBookingTOList[i].BookingScheduleTOLst)
                    {

                        bookingDictionary = new Dictionary<int, double>();
                        foreach (var item in bookingSchedule.OrderDetailsLst)
                        {
                            if (!bookingDictionary.ContainsKey(item.MaterialId))
                            {
                                bookingDictionary.Add(item.MaterialId, item.BookedQty);
                            }
                            else
                            {
                                double val = Convert.ToDouble(bookingDictionary[item.MaterialId]);
                                item.BookedQty += val;
                                bookingDictionary[item.MaterialId] = item.BookedQty;
                            }
                        }

                        tblBookingPendingRptTO.BookingDictionaryList.Add(bookingDictionary);
                    }

                    //if (loadingSlipExtList != null && loadingSlipExtList.Count > 0)
                    //{

                    loadingDictionary = new Dictionary<int, double>();
                    foreach (var loadingSlipExt in loadingSlipExtList)
                    {
                        if (!loadingDictionary.ContainsKey(loadingSlipExt.MaterialId))
                        {
                            loadingDictionary.Add(loadingSlipExt.MaterialId, loadingSlipExt.LoadingQty);
                        }
                        else
                        {
                            double val = Convert.ToDouble(loadingDictionary[loadingSlipExt.MaterialId]);
                            loadingSlipExt.LoadingQty += val;
                            loadingDictionary[loadingSlipExt.MaterialId] = loadingSlipExt.LoadingQty;
                        }
                    }
                    tblBookingPendingRptTO.LoadingDictionaryList.Add(loadingDictionary);
                    //}
                    Dictionary<string, string> finalBookingpendingQty = new Dictionary<string, string>();
                    // use for loop against material and check using containskey
                    double bookedQty = 0.0, totalBookedQty = 0.0;
                    double loadQty = 0.0, totalLoadQty = 0.0;
                    string finalData = "";
                    string totalQty = "";

                    foreach (var eachMaterial in MaterialList)
                    {
                        bookedQty = 0.0;
                        loadQty = 0.0;
                        finalData = "";
                        //    finalTotal = "";
                        // totalQty = "";
                        // totalBookedQty = 0;
                        //totalLoadQty = 0;
                        if (bookingDictionary != null)
                        {
                            if (bookingDictionary.ContainsKey(eachMaterial.Value))
                            {
                                bookedQty = Convert.ToDouble(bookingDictionary[eachMaterial.Value]);
                                totalBookedQty += bookedQty;
                            }
                        }
                        if (loadingDictionary != null)
                        {
                            if (loadingDictionary.ContainsKey(eachMaterial.Value))
                            {
                                loadQty = Convert.ToDouble(loadingDictionary[eachMaterial.Value]);
                                totalLoadQty += loadQty;
                            }
                        }

                        finalData = bookedQty.ToString("N1") + "/" + loadQty.ToString("N1");

                        finalBookingpendingQty.Add(eachMaterial.Text, finalData);
                    }

                    totalQty = totalBookedQty.ToString("N1") + "/" + totalLoadQty.ToString("N1");
                    tblBookingPendingRptTO.TotalQty = totalQty;
                    tblBookingPendingRptTO.FinalDictionaryList.Add(finalBookingpendingQty);
                    tblBookingPendingRptTOList.Add(tblBookingPendingRptTO);
                }


            }
            return tblBookingPendingRptTOList;
            // return tblBookingTOList;
        }
        public List<TblBookingsTO> SelectAllTblBookingsList()
        {
            return _iTblBookingsDAO.SelectAllTblBookings();

        }

        public List<TblBookingsTO> SelectAllBookingsListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.SelectAllBookingsListFromLoadingSlipId(loadingSlipId, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-02-23] Wil  return list of all booking out of quota and rate band
        /// and booking status = New
        /// </summary>
        /// <returns></returns>
        public List<TblBookingsTO> SelectAllBookingsListForApproval(Int32 isConfirmed, Int32 idBrand)
        {
            return _iTblBookingsDAO.SelectAllBookingsListForApproval(isConfirmed, idBrand);
        }


        public Double SelectTotalPendingBookingQty(DateTime sysDate)
        {
            return _iTblBookingsDAO.SelectTotalPendingBookingQty(sysDate);

        }

        public void AssignOverDueAmount(List<TblBookingsTO> tblBookingsTOList)
        {
            if (tblBookingsTOList != null && tblBookingsTOList.Count > 0)
            {
                String strDistDealers = string.Join(",", tblBookingsTOList.Select(d => d.DealerOrgId.ToString()).ToArray());

                if (!string.IsNullOrEmpty(strDistDealers))
                {
                    List<TblOverdueDtlTO> tblOverdueDtlTOList = _iTblOverdueDtlDAO.SelectAllTblOverdueDtl(strDistDealers);
                    if (tblOverdueDtlTOList != null && tblOverdueDtlTOList.Count > 0)
                    {
                        for (int i = 0; i < tblBookingsTOList.Count; i++)
                        {
                            TblOverdueDtlTO tblOverdueDtlTO = tblOverdueDtlTOList.Where(w => w.OrganizationId == tblBookingsTOList[i].DealerOrgId).FirstOrDefault();

                            if (tblOverdueDtlTO != null)
                                tblBookingsTOList[i].OverdueAmt = tblOverdueDtlTO.OverdueAmt;
                        }
                    }
                    List<TblEnquiryDtlTO> tblEnquiryDtlTOList = _iTblEnquiryDtlDAO.SelectAllTblEnquiryDtl(strDistDealers);
                    if (tblEnquiryDtlTOList != null && tblEnquiryDtlTOList.Count > 0)
                    {
                        for (int i = 0; i < tblBookingsTOList.Count; i++)
                        {
                            TblEnquiryDtlTO tblEnquiryDtlTO = tblEnquiryDtlTOList.Where(w => w.OrganizationId == tblBookingsTOList[i].DealerOrgId).FirstOrDefault();

                            if (tblEnquiryDtlTO != null)
                                tblBookingsTOList[i].EnquiryAmt = tblEnquiryDtlTO.EnquiryAmt;
                        }
                    }


                }
            }
        }


        /// <summary>
        /// Sanjay [2017-02-27] Will return list of all bookings of given cnf which are beyond quota and Rate Band
        /// and Approved By Directors. This will be for Acceptance By Cnf
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        public List<TblBookingsTO> SelectAllBookingsListForAcceptance(Int32 cnfId, List<TblUserRoleTO> userRoleTOList, Int32 isConfirmed)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                userRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(userRoleTOList);
            }

            return _iTblBookingsDAO.SelectAllBookingsListForAcceptance(cnfId, userRoleTO, isConfirmed);

        }

        public List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords)
        {
            List<TblBookingsTO> pendingList = _iTblBookingsDAO.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, true);
            if (pendingList != null && pendingList.Count < lastNRecords)
            {
                lastNRecords = lastNRecords - pendingList.Count;
                List<TblBookingsTO> list = _iTblBookingsDAO.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, false);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var isExist = pendingList.Where(e => e.IdBooking == list[i].IdBooking).FirstOrDefault();
                        if (isExist == null)
                            pendingList.Add(list[i]);

                    }
                }
            }

            return pendingList;
        }

        /// <summary>
        /// Sanjay [2017-02-23] This will return list of booking to show for loading
        /// Only Apporved and having Pending Qty > 0 bookings will be returned.
        /// </summary>
        /// <param name="cnfId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public List<TblBookingsTO> SelectAllBookingList(Int32 cnfId, Int32 dealerId, List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                userRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return _iTblBookingsDAO.SelectAllBookingList(cnfId, dealerId, userRoleTO);

        }



        public List<TblBookingsTO> GetOrderwiseDealerList()
        {
            return _iTblBookingsDAO.GetOrderwiseDealerList();

        }
        public List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, Int32 statusId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 confirm, Int32 isPendingQty, Int32 bookingId, Int32 isViewAllPendingEnq, Int32 RMId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return _iTblBookingsDAO.SelectBookingList(cnfId, dealerId, statusId, fromDate, toDate, tblUserRoleTO, confirm, isPendingQty, bookingId, isViewAllPendingEnq, RMId);

        }
        //Aniket [16-Jan-2019] added to view cnFList against confirm and not confirmbooking
        public List<CnFWiseReportTO> SelectCnfCNCBookingReport(DateTime fromDate, DateTime toDate)
        {
            return _iTblBookingsDAO.SelectCnfCNCBookingReport(fromDate, toDate);
        }
        /// <summary>
        /// Priyanka [14-03-2018]
        /// </summary>
        /// <param name="idBooking"></param>
        /// <returns></returns>
        public List<TblBookingSummaryTO> SelectBookingSummaryList(Int32 typeId, Int32 masterId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }


            return _iTblBookingsDAO.SelectBookingSummaryList(typeId, masterId, fromDate, toDate, tblUserRoleTO, cnfId);

        }


        public TblBookingsTO SelectTblBookingsTO(Int32 idBooking)
        {
            return _iTblBookingsDAO.SelectTblBookings(idBooking);

        }

        ///Priyanka [02-03-2018] 
        public List<TblBookingsTO> SelectUserwiseBookingList(DateTime fromDate, DateTime toDate, Int32 statusId, Int32 activeUserId)
        {
            return _iTblBookingsDAO.SelectUserwiseBookingList(fromDate, toDate, statusId, activeUserId);

        }



        ///// <summary>
        ///// Sanjay [2017-03-03] To Get the Details of Given Booking with child details
        ///// </summary>
        ///// <param name="idBooking"></param>
        ///// <returns></returns>
        //public TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking)
        //{
        //    try
        //    {
        //        TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(idBooking);
        //       // tblBookingsTO.DeliveryAddressLst =_iTblBookingDelAddrBL.SelectAllTblBookingDelAddrList(idBooking);
        //        //tblBookingsTO.OrderDetailsLst = _iTblBookingExtBL.SelectAllTblBookingExtList(idBooking);

        //        //[15-12-2017]Vijaymala :Added to  get booking schedule list against booking
        //        List<TblBookingScheduleTO> tblBookingScheduleTOList = _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(idBooking); 
        //        if(tblBookingScheduleTOList!=null && tblBookingScheduleTOList.Count > 0)
        //        {
        //            for (int i = 0; i < tblBookingScheduleTOList.Count; i++)
        //            {

        //                TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[i];
        //                List<TblBookingExtTO> tblBookingExtTOLst = _iTblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);
        //                tblBookingScheduleTO.OrderDetailsLst = tblBookingExtTOLst;
        //                List<TblBookingDelAddrTO> tblBookingDelAddrTOLst = _iTblBookingDelAddrDAO.SelectAllTblBookingDelAddrListBySchedule(tblBookingScheduleTO.IdSchedule);
        //                tblBookingScheduleTO.DeliveryAddressLst = tblBookingDelAddrTOLst;
        //            }


        //        }
        //        tblBookingsTO.BookingScheduleTOLst = tblBookingScheduleTOList;
        //        return tblBookingsTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}

        public List<ODLMWebAPI.DashboardModels.BookingInfo> SelectBookingDashboardInfo(List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, Int32 dealerId, DateTime date)
        {
            try
            {

                string ids = string.Empty;
                string cnfIds = string.Empty;
                string isConfirm = string.Empty;
                Int32 isHideCorNC = 0;
                List<TblConfigParamsTO> tblConfigParamsTOList = _iTblConfigParamsDAO.SelectAllTblConfigParams();
                if (tblConfigParamsTOList != null && tblConfigParamsTOList.Count > 0)
                {


                    TblConfigParamsTO tblConfigParamsTO = tblConfigParamsTOList.Where(ele => ele.ConfigParamName == Constants.CP_DASHBOARD_ENQ_QTY_STATUSES).FirstOrDefault();
                    //BL._iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DASHBOARD_ENQ_QTY_STATUSES);

                    if (tblConfigParamsTO != null)
                    {
                        ids = tblConfigParamsTO.ConfigParamVal;
                    }


                    TblConfigParamsTO tblConfigParamsTempTO = tblConfigParamsTOList.Where(ele => ele.ConfigParamName == Constants.CP_HIDE_NOT_CONFIRM_OPTION).FirstOrDefault();
                    //BL._iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_HIDE_NOT_CONFIRM_OPTION);


                    if (tblConfigParamsTempTO != null)
                    {
                        isHideCorNC = Convert.ToInt32(tblConfigParamsTempTO.ConfigParamVal);
                    }


                }
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                // Boolean isPriorityOther = true;
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
                    //isPriorityOther = BL.TblUserRoleBL.selectRolePriorityForOther(tblUserRoleTOList);
                }

                List<ODLMWebAPI.DashboardModels.BookingInfo> tblBookingsTOList = _iTblBookingsDAO.SelectBookingDashboardInfo(tblUserRoleTO, orgId, dealerId, date, ids, isHideCorNC);


                Double grandTotal = 0;
                Double grandTotalQty = 0;
                Double otherGrandTotal = 0;
                Double otherGrandTotalQty = 0;
                Int32 count = 0, otherCount = 0;
                for (int i = 0; i < tblBookingsTOList.Count; i++)
                {

                    ODLMWebAPI.DashboardModels.BookingInfo bookingInfo = tblBookingsTOList[i];

                    if (bookingInfo.BookingType == (int)Constants.BookingType.IsRegular)
                    {
                        count += 1;
                        grandTotal += bookingInfo.AvgPrice;
                        grandTotalQty += bookingInfo.BookingQty;
                    }
                    else
                    {
                        otherCount += 1;
                        otherGrandTotal += bookingInfo.AvgPrice;
                        otherGrandTotalQty += bookingInfo.BookingQty;

                    }

                }
                if (grandTotal != 0)
                {
                    ODLMWebAPI.DashboardModels.BookingInfo tempBookingInfo = new DashboardModels.BookingInfo();
                    tempBookingInfo.ShortNm = "Total";
                    tempBookingInfo.BrandName = "Grand Total";
                    tempBookingInfo.BookingQty = Math.Round(grandTotalQty);
                    tempBookingInfo.AvgPrice = Math.Round(grandTotal / count);
                    tempBookingInfo.IsConfirmed = 2;
                    tempBookingInfo.BookingType = (int)Constants.BookingType.IsRegular;
                    tblBookingsTOList.Add(tempBookingInfo);
                    tblBookingsTOList = tblBookingsTOList.OrderBy(o => o.BookingType).ToList();
                }
                tblBookingsTOList = tblBookingsTOList.Where(ele => ele.BookingType != (int)Constants.BookingType.IsOther).ToList();

                ODLMWebAPI.DashboardModels.BookingInfo otherBookingInfo = new DashboardModels.BookingInfo();
                if (otherGrandTotalQty > 0)
                {
                    otherBookingInfo.ShortNm = "Other";
                    otherBookingInfo.BrandName = "Other";
                    otherBookingInfo.BookingQty = Math.Round(otherGrandTotalQty);
                    otherBookingInfo.AvgPrice = Math.Round(otherGrandTotal / otherCount);
                    otherBookingInfo.IsConfirmed = 2;
                    otherBookingInfo.BookingType = (int)Constants.BookingType.IsOther;
                    tblBookingsTOList.Add(otherBookingInfo);
                }

                return tblBookingsTOList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TblBookingsTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.SelectTblBookings(idBooking, conn, tran);

        }

        //public  List<PendingBookingRptTO> SelectAllPendingBookingsForReport(Int32 cnfId,Int32 dealerOrgId, List<TblUserRoleTO> tblUserRoleTOList, int isTransporterScopeYn, int isConfirmed, DateTime fromDate, DateTime toDate, Boolean isDateFilter)
        public List<PendingBookingRptTO> SelectAllPendingBookingsForReport(Int32 cnfId, Int32 dealerOrgId, List<TblUserRoleTO> tblUserRoleTOList, int isTransporterScopeYn, int isConfirmed, DateTime fromDate, DateTime toDate, Boolean isDateFilter, Int32 brandId)
        {
            try
            {
                int isConfEn = 0;
                int userId = 0;

                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);

                }
                if (tblUserRoleTO != null)
                {
                    isConfEn = tblUserRoleTO.EnableAreaAlloc;
                    userId = tblUserRoleTO.UserId;
                }

                List<UserAreaCnfDealerDtlTO> userDealerList = null;
                if (userId > 0 && isConfEn == 1)
                    userDealerList = _iTblUserAreaAllocationBL.SelectAllUserAreaCnfDealerList(userId);

                List<PendingBookingRptTO> list = new List<PendingBookingRptTO>();
                DateTime serverDate = _iCommon.ServerDateTime;

                //List<TblBookingsTO> openingBalBookingList = DAL.TblBookingsDAO.SelectAllTodaysBookingsWithOpeningBalance(cnfId, serverDate, isTransporterScopeYn, isConfirmed);
                //List<TblBookingsTO> todaysList = DAL.TblBookingsDAO.SelectAllPendingBookingsList(cnfId, "=", false,isTransporterScopeYn,isConfirmed, serverDate, tblUserRoleTO);
                List<TblBookingsTO> openingBalBookingList = _iTblBookingsDAO.SelectAllTodaysBookingsWithOpeningBalance(cnfId, serverDate, isTransporterScopeYn, isConfirmed, brandId);
                List<TblBookingsTO> todaysList = _iTblBookingsDAO.SelectAllPendingBookingsList(cnfId, "=", false, isTransporterScopeYn, isConfirmed, serverDate, brandId, tblUserRoleTO);
                List<TblBookingOpngBalTO> openingBalQtyList = _iTblBookingOpngBalDAO.SelectAllTblBookingOpngBal(serverDate);
                List<TblBookingQtyConsumptionTO> bookingConsuList = _iTblBookingQtyConsumptionDAO.SelectAllTblBookingQtyConsumption(serverDate);
                Dictionary<int, Double> todayDeletedLoadingQtyDCT = _iTblLoadingSlipDtlDAO.SelectBookingWiseLoadingQtyDCT(serverDate, true, brandId);
                Dictionary<int, Double> todaysLoadingQtyDCT = _iTblLoadingSlipDtlDAO.SelectBookingWiseLoadingQtyDCT(serverDate, false, brandId);

                List<TblBookingsTO> finalList = new List<TblBookingsTO>();
                if (openingBalBookingList != null)
                    finalList.AddRange(openingBalBookingList);
                if (todaysList != null)
                    finalList.AddRange(todaysList);

                if (finalList != null && finalList.Count > 0)
                {
                    List<Int32> bookingIdList = new List<int>();

                    var list1 = finalList.GroupBy(a => a.IdBooking).ToList().Select(a => a.Key).ToList();
                    bookingIdList.AddRange(list1);

                    if (todayDeletedLoadingQtyDCT != null && todayDeletedLoadingQtyDCT.Count > 0)
                    {
                        var list2 = todayDeletedLoadingQtyDCT.ToList().Select(a => a.Key).ToList();
                        bookingIdList.AddRange(list2);
                    }

                    if (todaysLoadingQtyDCT != null && todaysLoadingQtyDCT.Count > 0)
                    {
                        var list3 = todaysLoadingQtyDCT.ToList().Select(a => a.Key).ToList();
                        bookingIdList.AddRange(list3);
                    }

                    var distinctBookings = bookingIdList.Distinct().ToList();
                    foreach (var bookingId in distinctBookings)
                    {

                        PendingBookingRptTO pendingBookingRptTO = new PendingBookingRptTO();

                        var bookingTO = finalList.Where(a => a.IdBooking == bookingId).FirstOrDefault();
                        if (bookingTO == null)
                            bookingTO = SelectTblBookingsTO(bookingId);

                        if (bookingTO == null)
                        {
                            continue;
                        }

                        pendingBookingRptTO.BookingId = bookingId;
                        pendingBookingRptTO.CnfName = bookingTO.CnfName;
                        pendingBookingRptTO.CnfOrgId = bookingTO.CnFOrgId;
                        pendingBookingRptTO.DealerName = bookingTO.DealerName;
                        pendingBookingRptTO.DealerOrgId = bookingTO.DealerOrgId;
                        pendingBookingRptTO.NoOfDayElapsed = (int)serverDate.Subtract(bookingTO.CreatedOn).TotalDays;
                        pendingBookingRptTO.BookingType = bookingTO.BookingType;
                        Int32 cnfOrgId = pendingBookingRptTO.CnfOrgId;
                        Int32 dealerId = pendingBookingRptTO.DealerOrgId;

                        if (userDealerList != null && userDealerList.Count > 0)
                        {
                            // If User has area alloacated then check for allocated Cnf and Area
                            //modified by vijaymala acc to brandwise allocation or districtwise allocation[26-07-2018]
                            var isAllowedTO = userDealerList.Where(u => (u.DealerOrgId == dealerId && u.CnfOrgId == cnfOrgId) || (u.CnfOrgId == cnfOrgId)).FirstOrDefault();
                            if (isAllowedTO == null)
                                continue;
                        }

                        if (cnfId > 0)
                        {
                            if (cnfOrgId != cnfId)
                                continue;
                        }

                        if (dealerOrgId > 0)
                        {
                            if (dealerOrgId != dealerId)
                                continue;
                        }

                        var openingQtyMT = openingBalQtyList.Where(b => b.BookingId == bookingId).Sum(x => x.OpeningBalQty);

                        pendingBookingRptTO.OpeningBalanceMT = openingQtyMT;
                        pendingBookingRptTO.BookingRate = bookingTO.BookingRate;
                        pendingBookingRptTO.BookingDate = bookingTO.CreatedOn;

                        Double todaysBookQtyMT = 0;
                        if (bookingTO.CreatedOn.Day == serverDate.Day && bookingTO.CreatedOn.Month == serverDate.Month && bookingTO.CreatedOn.Year == serverDate.Year)
                        {
                            todaysBookQtyMT = bookingTO.BookingQty;
                            pendingBookingRptTO.TodaysBookingMT = todaysBookQtyMT;
                        }

                        var todaysDelQty = bookingConsuList.Where(d => d.BookingId == bookingId).Sum(s => s.ConsumptionQty);
                        pendingBookingRptTO.TodaysDelBookingQty = todaysDelQty;

                        Double todaysLoadingQty = 0;
                        if (todaysLoadingQtyDCT != null && todaysLoadingQtyDCT.ContainsKey(bookingId))
                        {
                            todaysLoadingQty = todaysLoadingQtyDCT[bookingId];
                        }
                        Double todaysDelLoadingQty = 0;
                        if (todayDeletedLoadingQtyDCT != null && todayDeletedLoadingQtyDCT.ContainsKey(bookingId))
                        {
                            todaysDelLoadingQty = todayDeletedLoadingQtyDCT[bookingId];
                        }

                        Double todaysFinalLoadingQty = todaysLoadingQty - todaysDelLoadingQty;
                        pendingBookingRptTO.TodaysLoadingQtyMT = todaysFinalLoadingQty;

                        Double closingBal = 0;

                        if (openingQtyMT == 0)
                            closingBal = todaysBookQtyMT - (todaysLoadingQty - todaysDelLoadingQty + todaysDelQty);
                        else
                            closingBal = openingQtyMT - (todaysLoadingQty - todaysDelLoadingQty + todaysDelQty);

                        pendingBookingRptTO.ClosingBalance = closingBal;
                        pendingBookingRptTO.TransporterScopeYn = bookingTO.TransporterScopeYn;
                        pendingBookingRptTO.IsConfirmed = bookingTO.IsConfirmed;

                        list.Add(pendingBookingRptTO);
                    }
                }

                list = list.OrderBy(a => a.CnfName).ThenByDescending(b => b.NoOfDayElapsed).ToList();

                // Vaibhav [22-Mar-2018] Added to filter final enquiry list.
                if (isTransporterScopeYn != 2)
                {
                    list = list.FindAll(ele => ele.TransporterScopeYn == isTransporterScopeYn);
                }

                if (isConfirmed != 2)
                {
                    list = list.FindAll(ele => ele.IsConfirmed == isConfirmed);
                }

                // Vaibhav [16-April-2018] Added date wise filter.
                if (fromDate != DateTime.MinValue && fromDate != DateTime.MaxValue && toDate != DateTime.MinValue && toDate != DateTime.MaxValue && isDateFilter == true)
                {
                    list = list.Where(ele => ele.BookingDate.Date >= fromDate.Date && ele.BookingDate.Date <= toDate.Date).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// Vijaymala [2017-09-11] added to get booking list to generate booking graph
        /// </summary>
        /// <returns></returns>
        public List<BookingGraphRptTO> SelectBookingListForGraph(Int32 OrganizationId, List<TblUserRoleTO> userRoleTOList, Int32 dealerId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            //Boolean isPriorityOther = true;
            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(userRoleTOList);
                // isPriorityOther = BL.TblUserRoleBL.selectRolePriorityForOther(userRoleTOList);
            }

            return _iTblBookingsDAO.SelectBookingListForGraph(OrganizationId, tblUserRoleTO, dealerId);

        }
        #endregion

        #region Insertion
        public int InsertTblBookings(TblBookingsTO tblBookingsTO)
        {
            return _iTblBookingsDAO.InsertTblBookings(tblBookingsTO);
        }

        public int InsertTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.InsertTblBookings(tblBookingsTO, conn, tran);
        }
        public List<TblOrganizationTO> SelectSalesAgentListWithBrandAndRate()
        {
            try
            {
                List<TblOrganizationTO> orgList = _iTblOrganizationDAO.SelectSaleAgentOrganizationList();
                if (orgList != null)
                {
                    List<DropDownTO> brandList = _iDimensionDAO.SelectBrandList();
                    Dictionary<Int32, Int32> brandRateDCT = _iTblGlobalRateDAO.SelectLatestBrandAndRateDCT();
                    Dictionary<Int32, List<TblQuotaDeclarationTO>> rateAndBandDCT = new Dictionary<int, List<TblQuotaDeclarationTO>>();
                    List<TblGlobalRateTO> tblGlobalRateTOList = new List<TblGlobalRateTO>();
                    if (brandList == null || brandList.Count == 0)
                        return null;

                    foreach (var item in brandRateDCT.Keys)
                    {
                        Int32 rateID = brandRateDCT[item];
                        TblGlobalRateTO rateTO = _iTblGlobalRateDAO.SelectTblGlobalRate(rateID);
                        if (rateTO != null)
                            tblGlobalRateTOList.Add(rateTO);
                        List<TblQuotaDeclarationTO> rateBandList = _iTblQuotaDeclarationDAO.SelectAllTblQuotaDeclaration(rateID);

                        rateAndBandDCT.Add(rateID, rateBandList);
                    }

                    for (int i = 0; i < orgList.Count; i++)
                    {
                        TblOrganizationTO tblOrganizationTO = orgList[i];
                        tblOrganizationTO.BrandRateDtlTOList = new List<Models.BrandRateDtlTO>();
                        for (int b = 0; b < brandList.Count; b++)
                        {
                            Models.BrandRateDtlTO brandRateDtlTO = new Models.BrandRateDtlTO();
                            brandRateDtlTO.BrandId = brandList[b].Value;
                            brandRateDtlTO.BrandName = brandList[b].Text;

                            if (brandRateDCT != null && brandRateDCT.ContainsKey(brandRateDtlTO.BrandId))
                            {
                                int rateId = brandRateDCT[brandRateDtlTO.BrandId];

                                if (tblGlobalRateTOList != null)
                                {
                                    TblGlobalRateTO rateTO = tblGlobalRateTOList.Where(ri => ri.IdGlobalRate == rateId).FirstOrDefault();
                                    if (rateTO != null)
                                        brandRateDtlTO.Rate = rateTO.Rate;
                                }

                                if (rateAndBandDCT != null && rateAndBandDCT.ContainsKey(rateId))
                                {
                                    List<TblQuotaDeclarationTO> rateBandList = rateAndBandDCT[rateId];
                                    if (rateBandList != null)
                                    {
                                        var rateBandObj = rateBandList.Where(o => o.OrgId == tblOrganizationTO.IdOrganization).FirstOrDefault();
                                        if (rateBandObj != null)
                                        {
                                            brandRateDtlTO.RateBand = rateBandObj.RateBand;
                                            brandRateDtlTO.LastAllocQty = rateBandObj.AllocQty; //Sudhir[25-6-2018] Added For Madhav
                                            brandRateDtlTO.ValidUpto = rateBandObj.ValidUpto; //Sudhir[25-6-2018] Added For Madhav
                                            brandRateDtlTO.BalanceQty = rateBandObj.BalanceQty; //Sudhir[25-6-2018] Added For Madhav
                                            brandRateDtlTO.QuotaDeclarationId = rateBandObj.IdQuotaDeclaration;  //Aniket [4-7-2019]

                                        }
                                    }
                                }
                            }

                            tblOrganizationTO.BrandRateDtlTOList.Add(brandRateDtlTO);
                        }

                    }
                }

                return orgList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
        public ResultMessage SaveNewBooking(TblBookingsTO tblBookingsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;

            int restrictBeyondQuota = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                // Aniket [27-02-2019] added to check whether CNF has sufficient balance quota against current booking
                #region
                TblConfigParamsTO tblConfigParamTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.ANNOUNCE_RATE_WITH_RATEBAND_CURRENT_QUOTA);
                if (tblConfigParamTO != null)
                {
                    TblConfigParamsTO tblConfigParamTOForQuota = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RESTRICT_CNF_BEYOND_BOOKING_QUOTA);
                    if (tblConfigParamTOForQuota != null)
                    {
                        if (tblConfigParamTOForQuota.ConfigParamVal == "1")
                        {
                            restrictBeyondQuota = 1;
                        }
                    }
                }
                if (restrictBeyondQuota == 1)
                {
                    List<TblOrganizationTO> tblOrganizationToList = SelectSalesAgentListWithBrandAndRate();
                    int brandId = 0;
                    double balanceQty = 0;
                    int cnfId = 0;
                    int flag = 0;
                    double allocatedQty = 0.0;
                    foreach (var i in tblOrganizationToList)
                    {
                        if (i.IdOrganization == tblBookingsTO.CnFOrgId)
                        {
                            foreach (var j in i.BrandRateDtlTOList)
                            {
                                if (j.BrandId == tblBookingsTO.BrandId)
                                {
                                    brandId = j.BrandId;
                                    balanceQty = j.BalanceQty;
                                    cnfId = i.IdOrganization;
                                    allocatedQty = j.LastAllocQty;
                                    flag = 1;
                                    break;
                                }
                            }

                        }
                        if (flag == 1)
                        {
                            break;
                        }
                    }

                    if (cnfId == tblBookingsTO.CnFOrgId)
                    {
                        if (allocatedQty > 0)
                        {
                            if (tblBookingsTO.BookingQty > balanceQty && tblBookingsTO.BrandId == brandId)
                            {
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                resultMessage.Text = "Booking Quantity is greater than Balance Quota. Remaining Quota is " + balanceQty.ToString();
                                resultMessage.DisplayMessage = "Your booking Quota limit ends, Kindly contact Administrator";
                                return resultMessage;
                            }
                        }

                    }
                }
                #endregion
                //[05-09-2018] : Vijaymala added to  differentiate other and regular booking
                Boolean isRegular = true;
                #region 0. Check Bookings are Open Or Closed. If Closed Then Do Not Save the request
                if (tblBookingsTO.BookingType == 0)
                {
                    tblBookingsTO.BookingType = (int)Constants.BookingType.IsRegular;
                }
                if (tblBookingsTO.BookingType == (int)Constants.BookingType.IsOther)
                {
                    isRegular = false;
                }

                //Priyanka [22-11-18] : Commented and write common code for booking validation.
                //if (isRegular)
                //{
                //    TblBookingActionsTO bookingStatusTO = BL.TblBookingActionsBL.SelectLatestBookingActionTO(conn, tran);
                //    if (bookingStatusTO == null || bookingStatusTO.BookingStatus == "CLOSE")
                //    {
                //        resultMessage.MessageType = ResultMessageE.Error;
                //        resultMessage.Result = 0;
                //        resultMessage.Text = "Sorry..Record Could not be saved. Bookings are closed";
                //        resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Bookings are closed";
                //        return resultMessage;
                //    }
                //}
                //Priyanka [22-11-2018]
                #region Validation for New Booking

                //1.Checking the status of Booking OPEN/CLOSE.
                TblBookingActionsTO bookingStatusTO = _iTblBookingActionsDAO.SelectLatestBookingActionTO(conn, tran);
                if (bookingStatusTO == null || bookingStatusTO.BookingStatus == "CLOSE")
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Sorry..Record Could not be saved. Bookings are closed";
                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Bookings are closed";
                    return resultMessage;
                }

                //2. Checking the Booking End Time.
                List<TblConfigParamsTO> tblConfigParamsTOList = _iTblConfigParamsDAO.SelectAllTblConfigParams();
                TblQuotaDeclarationTO existingQuotaTO = new TblQuotaDeclarationTO();
                TblConfigParamsTO bookingEndTimeConfigParamsTO = tblConfigParamsTOList.Where(c => c.ConfigParamName == Constants.CP_BOOKING_END_TIME).FirstOrDefault();
                if (bookingEndTimeConfigParamsTO != null)
                {


                    TimeSpan abc = tblBookingsTO.CreatedOn.TimeOfDay;
                    int hrs = abc.Hours;
                    int min = abc.Minutes;

                    string[] endTime = bookingEndTimeConfigParamsTO.ConfigParamVal.Split(':');

                    int endTimeHrs = Convert.ToInt32(endTime[0]);
                    int endTimeMin = 0;
                    if (endTime.Length > 1)
                    {
                        endTimeMin = Convert.ToInt32(endTime[1]);
                    }

                    if (hrs >= endTimeHrs)
                    {
                        if (hrs > endTimeHrs)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Booking time is end";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                        else
                        {
                            if (hrs == endTimeHrs)
                            {
                                if (min > endTimeMin)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Sorry..Record Could not be saved. Booking time is end";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    else
                    {

                    }
                }


                //3.Check the quota declaration date and current date.
                //Prajakta[2019-06-18] Added isRateDeclareForEnqConfigParam condition as per disscussion with aniket
                TblConfigParamsTO isRateDeclareForEnqConfigParam = tblConfigParamsTOList.Where(c => c.ConfigParamName == Constants.CP_RATE_DECLARATION_FOR_ENQUIRY).FirstOrDefault();
                if (isRateDeclareForEnqConfigParam != null)
                {
                    if (isRateDeclareForEnqConfigParam.ConfigParamVal.ToString() == "1" && isRegular)
                    {
                        existingQuotaTO = _iTblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);
                        if (existingQuotaTO != null)
                        {
                            if (existingQuotaTO.CreatedOn.Date != _iCommon.ServerDateTime.Date)
                            {
                                tran.Rollback();
                                resultMessage.Text = "Sorry..Record Could not be saved. Quota Not Found";
                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                            else
                            {

                            }

                        }
                        else
                        {

                        }

                    }
                }


                //Aniket [8-3-2019] added to verify booking should placed against current date and current booking rate
                if (isRegular)
                {
                    int isAllowBooking = 0;
                    DateTime sysDate1 = _iCommon.ServerDateTime;
                    List<TblQuotaDeclarationTO> tblQuotaDeclarationTOList = _iTblQuotaDeclarationBL.SelectLatestQuotaDeclarationTOList(tblBookingsTO.CnFOrgId, sysDate1);
                    for (int i = 0; i < tblQuotaDeclarationTOList.Count; i++)
                    {
                        if (tblQuotaDeclarationTOList[i].IdQuotaDeclaration == tblBookingsTO.QuotaDeclarationId)
                        {
                            isAllowBooking = 1;
                            break;
                        }
                    }
                    if (isAllowBooking == 0)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record could not be saved";
                        resultMessage.DisplayMessage = "Sorry..Booking can not continue, Please refresh your page and try again";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }
                }


                #endregion

                #endregion

                #region 0.1 Check for Parity Chart. Assign Active ParityId For Future Reference
                //Sudhir[23-MARCH-2018] Commented for as per new parity logic no parity need to check or save at the time of Booking.
                /*
                List<DropDownTO> brandList = BL.DimensionBL.SelectBrandList();
                if (brandList == null || brandList.Count == 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Sorry..Brand List not found";
                    resultMessage.DisplayMessage = "Sorry..Brand List not found";
                    return resultMessage;
                }

                
                List<TblParitySummaryTO> activeParityTOList = BL.TblParitySummaryBL.SelectActiveParitySummaryTOList(tblBookingsTO.DealerOrgId, conn, tran);
                if (activeParityTOList != null)
                {
                    //resultMessage.Text = "Sorry..Record Could not be saved. Rate Chart Not Defined For The State";
                    //resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Rate Chart Not Defined For The State";
                    //List<TblBookingParitiesTO> parityList = new List<TblBookingParitiesTO>();
                    //tblBookingsTO.ParityId = activeParityTO.IdParity;

                    String notDefinedParites = String.Empty;

                    for (int u = 0; u < brandList.Count; u++)
                    {
                        TblParitySummaryTO tblParitySummaryTO = activeParityTOList.Where(w => w.BrandId == brandList[u].Value).FirstOrDefault();
                        if (tblParitySummaryTO == null)
                        {
                            notDefinedParites += brandList[u].Text + ", ";
                        }
                    }

                    if (!String.IsNullOrEmpty(notDefinedParites))
                    {
                        notDefinedParites = notDefinedParites.Trim().TrimEnd(',');
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        resultMessage.Text = "Sorry..parity is not define against brand - " + notDefinedParites + ". Kindly update the parity master ";
                        resultMessage.DisplayMessage = "Sorry..parity is not define against brand - " + notDefinedParites + ". Kindly update the parity master ";
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    TblAddressTO addrTO = BL.TblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                    if (addrTO == null)
                    {
                        resultMessage.Text = "Sorry..Record Could not be saved. Address Is Not Defined For The Dealer And Hence Parity Settings For Not found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Address Is Not Defined For The Dealer And Hence Parity Settings Not found";
                    }
                    else
                    {
                        resultMessage.Text = "Sorry..Record Could not be saved. Parity Settings Not defined for the state " + addrTO.StateName;
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved. Parity Settings Not defined for the state " + addrTO.StateName;
                    }
                    return resultMessage;
                }*/
                #endregion

                #region 1. Save Booking Request First
                //Sanjay [2017-02-17] Default pending qty will be booking qty. Used in Loading of the order
                //[05-09-2018] : Vijaymala added to  set global rate only for regular booking

                TblGlobalRateTO globalRateTO = new TblGlobalRateTO();

                Int32 statusId = (Int32)Constants.TranStatusE.BOOKING_NEW;

                TblConfigParamsTO validateConfigTO = new TblConfigParamsTO();
                TblConfigParamsTO statusAfterBookingConfigTO = new TblConfigParamsTO();
                if (tblConfigParamsTOList != null && tblConfigParamsTOList.Count > 0)
                {
                    TblConfigParamsTO bookingStatusConfigParamsTO = tblConfigParamsTOList.Where(c => c.ConfigParamName == Constants.CP_STATUS_AFTER_SAVE_BOOKING).FirstOrDefault();
                    if (bookingStatusConfigParamsTO != null)
                    {
                        statusId = Convert.ToInt32(bookingStatusConfigParamsTO.ConfigParamVal);
                    }
                    validateConfigTO = tblConfigParamsTOList.Where(e => e.ConfigParamName == Constants.CP_VALIDATE_RATEBAND_FOR_BOOKING).FirstOrDefault();
                    statusAfterBookingConfigTO = tblConfigParamsTOList.Where(e => e.ConfigParamName == Constants.CP_STATUS_FOR_SAVE_BOOKING_VALIDATION).FirstOrDefault();
                }
                if (isRegular)
                {
                    if (tblBookingsTO.QuotaDeclarationId == 0)
                    {
                        List<TblQuotaDeclarationTO> rateBandList = _iTblQuotaDeclarationBL.SelectAllTblQuotaDeclarationList(tblBookingsTO.GlobalRateId);
                        if (rateBandList != null && rateBandList.Count > 0)
                        {
                            tblBookingsTO.QuotaDeclarationId = rateBandList.Where(w => w.OrgId == tblBookingsTO.CnFOrgId).FirstOrDefault().IdQuotaDeclaration;
                        }
                    }
                    existingQuotaTO = _iTblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);
                    if (existingQuotaTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved. Quota Not Found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = 0;
                        return resultMessage;
                    }

                    tblBookingsTO.QuotaQtyBforBooking = Convert.ToInt32(existingQuotaTO.BalanceQty);
                    tblBookingsTO.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTO.QuotaQtyBforBooking - tblBookingsTO.BookingQty);

                    globalRateTO = _iTblGlobalRateDAO.SelectTblGlobalRate(existingQuotaTO.GlobalRateId, conn, tran);
                    if (globalRateTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved. Rate Declaration Not Found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Result = 0;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_MAX_ALLOWED_DEL_PERIOD, conn, tran);
                Int32 maxAllowedDelPeriod = 7;

                if (tblConfigParamsTO != null)
                    maxAllowedDelPeriod = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                Double orcAmtPerTon = 0;
                if (tblBookingsTO.OrcMeasure == "Rs/MT")
                {
                    orcAmtPerTon = tblBookingsTO.OrcAmt;
                }
                else orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;

                Double allowedRate = globalRateTO.Rate - existingQuotaTO.RateBand;
                Double bookingRateWithOrcAmt = tblBookingsTO.BookingRate;

                if (orcAmtPerTon > 0)
                    bookingRateWithOrcAmt = tblBookingsTO.BookingRate - existingQuotaTO.RateBand - orcAmtPerTon;

                TblOrganizationTO dealerOrgTO = _iTblOrganizationDAO.SelectTblOrganization(tblBookingsTO.DealerOrgId, conn, tran);
                if (dealerOrgTO == null)
                {
                    tran.Rollback();
                    resultMessage.Text = "Sorry..Record Could not be saved. Dealer Details not found";
                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                    resultMessage.Result = 0;
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }

                tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_MAX_ALLOWED_CD_STRUCTURE, conn, tran);
                Double maxCdStructure = 1.5;

                if (tblConfigParamsTO != null)
                    maxCdStructure = Convert.ToDouble(tblConfigParamsTO.ConfigParamVal);


                if (tblBookingsTO.QuotaQtyAftBooking < 0 || (tblBookingsTO.BookingRate < allowedRate)
                    || (tblBookingsTO.DeliveryDays > maxAllowedDelPeriod)
                    // || (bookingRateWithOrcAmt < globalRateTO.Rate) // Sanjay [2017-06-30] Not required as per discussion in meeting 29/6/17. Nitin K Sir and BRM Team. It will directly added into booking Rate for final Rate Calculation
                    )
                {
                    tblBookingsTO.IsWithinQuotaLimit = 0;
                    // tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                    tblBookingsTO.StatusId = statusId;
                    if (tblBookingsTO.QuotaQtyAftBooking < 0)
                        tblBookingsTO.AuthReasons = "QTY|";
                    if (tblBookingsTO.BookingRate < allowedRate)
                        tblBookingsTO.AuthReasons += "RATE|";
                    if (tblBookingsTO.DeliveryDays > maxAllowedDelPeriod)
                        tblBookingsTO.AuthReasons += "DELIVERY|";
                    //if (bookingRateWithOrcAmt < globalRateTO.Rate)  // Sanjay [2017-06-30] Not required as per discussion in meeting 29/6/17. Nitin K Sir and BRM Team.It will directly added into booking Rate for final Rate Calculation
                    //    tblBookingsTO.AuthReasons += "ORC|";

                    
                }
                else
                {
                    //Sanjay [2017-11-10]
                    //tblBookingsTO.IsWithinQuotaLimit = 1;
                    //tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_APPROVED;

                    tblBookingsTO.StatusId = statusId;
                    //tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;

                }

                if (tblBookingsTO.CdStructure > maxCdStructure)
                {
                    if (tblBookingsTO.CdStructure > dealerOrgTO.CdStructure)
                    {
                        tblBookingsTO.IsWithinQuotaLimit = 0;
                        //tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                        tblBookingsTO.StatusId = statusId;
                        tblBookingsTO.AuthReasons += "CD|";
                    }
                }
                //Aniket [3-7-2019] placed from line no 1265 to 1289 as while applying CD create problem
                if(tblBookingsTO.IsItemized==0 && !string.IsNullOrEmpty(tblBookingsTO.AuthReasons))
                tblBookingsTO.AuthReasons = tblBookingsTO.AuthReasons.TrimEnd('|');
                Int32 skipFinanceApproval = 0;
                //Saket [2018-02-13] Added 
                TblConfigParamsTO tblConfigParamsTOApproval = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_AUTO_FINANCE_APPROVAL_FOR_ENQUIRY, conn, tran);
                if (tblConfigParamsTOApproval != null)
                {
                    skipFinanceApproval = Convert.ToInt32(tblConfigParamsTOApproval.ConfigParamVal);
                    if (skipFinanceApproval == 1)
                    {
                        statusId = Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR);
                        tblBookingsTO.StatusId = statusId;
                        //tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
                        tblBookingsTO.IsWithinQuotaLimit = 1;
                    }
                }
                //[05-09-2018] : Vijaymala added to  set default brand  for other booking

                List<DimBrandTO> brandList = _iDimBrandDAO.SelectAllDimBrand();

                if (brandList != null && brandList.Count > 0)
                {
                    brandList = brandList.Where(ele => ele.IsActive == 1).ToList();
                }
                DimBrandTO dimBrandTO = new DimBrandTO();
                if (!isRegular)
                {
                    if (brandList != null && brandList.Count > 0)
                    {
                        dimBrandTO = brandList.Where(ele => ele.IsDefault == 1).FirstOrDefault();
                        if (dimBrandTO != null)
                        {
                            tblBookingsTO.BrandId = dimBrandTO.IdBrand;
                        }
                    }
                }

                tblBookingsTO.StatusBy = tblBookingsTO.CreatedBy;                    //Priyanka [27-07-2018]



                //Aniket [2018-02-13] Added 
                //int isBalajiClient = 0;
                //TblConfigParamsTO tblConfigParamsTOBalaji = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.IS_BALAJI_CLIENT, conn, tran);
                //if (tblConfigParamsTOBalaji != null)
                //{
                //    isBalajiClient = Convert.ToInt32(tblConfigParamsTOBalaji.ConfigParamVal);
                //}
                //if(isBalajiClient==0)
                tblBookingsTO.PendingQty = tblBookingsTO.BookingQty;         
                Int32 isValidateRateBand = 0;

                if (validateConfigTO != null)
                {
                    isValidateRateBand = Convert.ToInt32(validateConfigTO.ConfigParamVal);
                }
                if (isValidateRateBand == 1 && tblBookingsTO.BookingType == Convert.ToInt32(Constants.BookingType.IsRegular))
                {
                    if (tblBookingsTO.BookingRate < allowedRate)
                    {
                        if (statusAfterBookingConfigTO != null)
                        {
                            statusId = Convert.ToInt32(statusAfterBookingConfigTO.ConfigParamVal);
                        }
                    }
                }

                List<TblUserRoleTO> tblUserRoleTOList = _iTblUserRoleBL.SelectAllActiveUserRoleList(tblBookingsTO.CreatedBy);
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    TblUserRoleTO tblUserRoleTO = tblUserRoleTOList.Where(ele => ele.UserId == tblBookingsTO.CreatedBy).FirstOrDefault();
                    Dictionary<int, string> sysEleAccessDCT = _iTblSysElementsBL.SelectSysElementUserEntitlementDCT(tblUserRoleTO.UserId, tblUserRoleTO.RoleId);

                    if (sysEleAccessDCT != null || sysEleAccessDCT.Count > 0)
                    {
                        if (sysEleAccessDCT.ContainsKey(Convert.ToInt32(Constants.pageElements.SKIP_BOOKING_APPROVAL)) && sysEleAccessDCT[Convert.ToInt32(Constants.pageElements.SKIP_BOOKING_APPROVAL)] != null
                            && !string.IsNullOrEmpty(sysEleAccessDCT[Convert.ToInt32(Constants.pageElements.SKIP_BOOKING_APPROVAL)].ToString()) && sysEleAccessDCT[Convert.ToInt32(Constants.pageElements.SKIP_BOOKING_APPROVAL)] == "RW")
                        {
                            statusId = Convert.ToInt32(Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR);
                        }

                    }
                }


                tblBookingsTO.StatusId = statusId;
                //Aniket [24-7-2019] added to check scheduled NoOfDeliveries against booking
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count>0)
                {
                    var res = tblBookingsTO.BookingScheduleTOLst.GroupBy(x => x.ScheduleDate);
                    tblBookingsTO.NoOfDeliveries = res.Count();
                }
               
                result = InsertTblBookings(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "Sorry..Record Could not be saved.";
                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                    resultMessage.Result = 0;
                    resultMessage.MessageType = ResultMessageE.Error;
                    return resultMessage;
                }
                #endregion

                #region Priyanka [22-01-2019] : Added to save the commercial details against booking

                if (tblBookingsTO.PaymentTermOptionRelationTOLst != null && tblBookingsTO.PaymentTermOptionRelationTOLst.Count > 0)
                {
                    TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = new TblPaymentTermOptionRelationTO();

                    for (int i = 0; i < tblBookingsTO.PaymentTermOptionRelationTOLst.Count; i++)
                    {
                        tblPaymentTermOptionRelationTO = tblBookingsTO.PaymentTermOptionRelationTOLst[i];
                        tblPaymentTermOptionRelationTO.CreatedBy = tblBookingsTO.CreatedBy;
                        tblPaymentTermOptionRelationTO.CreatedOn = _iCommon.ServerDateTime;
                        tblPaymentTermOptionRelationTO.BookingId = tblBookingsTO.IdBooking;

                        result = _iTblPaymentTermOptionRelationBL.InsertTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved.";
                            resultMessage.DisplayMessage = "Error while insert into TblPaymentTermOptionRelation";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }
                #endregion

                #region 1.2 Save Booking Parities 

                DateTime sysDate = _iCommon.ServerDateTime;

                List<TblQuotaDeclarationTO> TblQuotaDeclarationTOList = _iTblQuotaDeclarationBL.SelectLatestQuotaDeclarationTOList(tblBookingsTO.CnFOrgId, sysDate);
                if (TblQuotaDeclarationTOList != null)
                {
                    for (int i = 0; i < TblQuotaDeclarationTOList.Count; i++)
                    {
                        if (TblQuotaDeclarationTOList[i].ValidUpto > 0)
                        {
                            if (!_iTblQuotaDeclarationBL.CheckForValidityAndReset(TblQuotaDeclarationTOList[i]))
                            {
                                TblQuotaDeclarationTOList.RemoveAt(i);
                                i--;
                            }
                        }


                    }
                }

                //Sudhir[20-MARCH-2018] Commented for as per new parity logic no parity need to check or save at the time of Booking.
                /*
                for (int pi = 0; pi < activeParityTOList.Count; pi++)
                {
                    TblBookingParitiesTO bookingParityTO = new TblBookingParitiesTO();

                    if (activeParityTOList[pi].BrandId == tblBookingsTO.BrandId)
                    {
                        bookingParityTO.BookingRate = tblBookingsTO.BookingRate;

                    }
                    else
                    {
                        TblQuotaDeclarationTO TblQuotaDeclarationTO = TblQuotaDeclarationTOList.Where(ele => ele.BrandId == activeParityTOList[pi].BrandId).FirstOrDefault();
                        if (TblQuotaDeclarationTO != null)
                        {
                            bookingParityTO.BookingRate = TblQuotaDeclarationTO.DeclaredRate;
                        }
                    }
                    bookingParityTO.BookingId = tblBookingsTO.IdBooking;
                    bookingParityTO.ParityId = activeParityTOList[pi].IdParity;
                    result = _iTblBookingParitiesBL.InsertTblBookingParities(bookingParityTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While InsertTblBookingParities");
                        return resultMessage;
                    }
                } */
                TblBookingParitiesTO bookingParityTO = new TblBookingParitiesTO();

                //[05-09-2018] : Vijaymala modified code to  save booking rate  for regular or other booking
                if (isRegular)
                {
                    for (int i = 0; i < brandList.Count; i++)
                    {
                        if (brandList[i].IdBrand == tblBookingsTO.BrandId)
                        {
                            bookingParityTO.BookingRate = tblBookingsTO.BookingRate;

                        }
                        else
                        {
                            TblQuotaDeclarationTO TblQuotaDeclarationTO = TblQuotaDeclarationTOList.Where(ele => ele.BrandId == brandList[i].IdBrand).FirstOrDefault();
                            if (TblQuotaDeclarationTO != null)
                            {
                                bookingParityTO.BookingRate = TblQuotaDeclarationTO.DeclaredRate;
                            }
                        }
                        bookingParityTO.BookingId = tblBookingsTO.IdBooking;
                        bookingParityTO.ParityId = 0;
                        bookingParityTO.BrandId = brandList[i].IdBrand;
                        result = _iTblBookingParitiesDAO.InsertTblBookingParities(bookingParityTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblBookingParities");
                            return resultMessage;
                        }
                    }

                }
                //[05-09-2018] : Vijaymala modified code to  save booking rate  for regular or other booking
                else
                {
                    bookingParityTO.BookingId = tblBookingsTO.IdBooking;
                    bookingParityTO.ParityId = 0;
                    bookingParityTO.BrandId = dimBrandTO.IdBrand;
                    bookingParityTO.BookingRate = tblBookingsTO.BookingRate;
                    result = _iTblBookingParitiesDAO.InsertTblBookingParities(bookingParityTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error While InsertTblBookingParities");
                        return resultMessage;
                    }
                }




                #endregion
                #region 2. If Booking Beyond Quota Then Maintain History Of Approvals

                if (tblBookingsTO.IsWithinQuotaLimit == 0)
                {
                    TblBookingBeyondQuotaTO bookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                    bookingBeyondQuotaTO.BookingId = tblBookingsTO.IdBooking;
                    bookingBeyondQuotaTO.CreatedBy = tblBookingsTO.CreatedBy;
                    bookingBeyondQuotaTO.CreatedOn = tblBookingsTO.CreatedOn;
                    bookingBeyondQuotaTO.DeliveryPeriod = tblBookingsTO.DeliveryDays;
                    //   if (isBalajiClient == 0)
                    bookingBeyondQuotaTO.Quantity = tblBookingsTO.BookingQty;
                    //else
                    //bookingBeyondQuotaTO.Quantity = tblBookingsTO.PendingQty;
                    bookingBeyondQuotaTO.Rate = tblBookingsTO.BookingRate;
                    bookingBeyondQuotaTO.CdStructureId = tblBookingsTO.CdStructureId;
                    bookingBeyondQuotaTO.OrcAmt = tblBookingsTO.OrcAmt;
                    bookingBeyondQuotaTO.Remark = tblBookingsTO.AuthReasons;
                    bookingBeyondQuotaTO.Rate = tblBookingsTO.BookingRate;
                    bookingBeyondQuotaTO.StatusDate = tblBookingsTO.CreatedOn;

                    // bookingBeyondQuotaTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                    bookingBeyondQuotaTO.StatusId = statusId;

                    result = _iTblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(bookingBeyondQuotaTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingBeyondQuota in Function SaveNewBooking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }

                #endregion

                #region 3.Save project schedule 
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst.Count; i++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[i];
                        tblBookingScheduleTO.BookingId = tblBookingsTO.IdBooking;
                        tblBookingScheduleTO.CreatedBy = tblBookingsTO.CreatedBy;
                        tblBookingScheduleTO.CreatedOn = tblBookingsTO.CreatedOn;

                        result = _iTblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Record Could not be saved.";
                            resultMessage.DisplayMessage = "Record Could not be saved.";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }


                        #region 3.1. Save Materialwise Qty and Rate
                        if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                        {
                            for (int j = 0; j < tblBookingScheduleTO.OrderDetailsLst.Count; j++)
                            {
                                TblBookingExtTO tblBookingExtTO = tblBookingScheduleTO.OrderDetailsLst[j];
                                tblBookingExtTO.BookingId = tblBookingsTO.IdBooking;
                                //if(isBalajiClient==0)
                                tblBookingExtTO.Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                                tblBookingExtTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                if (!isRegular)
                                {
                                    tblBookingExtTO.BrandId = dimBrandTO.IdBrand;
                                }
                                result = _iTblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Record Could not be saved.";
                                    resultMessage.DisplayMessage = "Record Could not be saved.";
                                    resultMessage.Result = 0;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {

                        }
                        #endregion
                        #region 3.2. Save Order Delivery Addresses

                        if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
                        {
                            for (int k = 0; k < tblBookingScheduleTO.DeliveryAddressLst.Count; k++)
                            {
                                TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingScheduleTO.DeliveryAddressLst[k];
                                if (string.IsNullOrEmpty(tblBookingDelAddrTO.Country))
                                    tblBookingDelAddrTO.Country = Constants.DefaultCountry;

                                tblBookingDelAddrTO.BookingId = tblBookingsTO.IdBooking;
                                tblBookingDelAddrTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                result = _iTblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Record Could not be saved.";
                                    resultMessage.DisplayMessage = "Record Could not be saved.";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            //Sanjay [2017-03-02] These details are not compulsory while entry
                            //tran.Rollback();
                            //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                            //resultMessage.MessageType = ResultMessageE.Error;
                            //return resultMessage;
                        }
                        #endregion
                    }

                }
                else
                {

                }
                #endregion

                //#region 3. Save Materialwise Qty and Rate
                //if (tblBookingsTO.OrderDetailsLst != null && tblBookingsTO.OrderDetailsLst.Count > 0)
                //{
                //    for (int i = 0; i < tblBookingsTO.OrderDetailsLst.Count; i++)
                //    {
                //        tblBookingsTO.OrderDetailsLst[i].BookingId = tblBookingsTO.IdBooking;
                //        tblBookingsTO.OrderDetailsLst[i].Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                //        result = _iTblBookingExtBL.InsertTblBookingExt(tblBookingsTO.OrderDetailsLst[i], conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingExt in Function SaveNewBooking";
                //            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //            resultMessage.Result = 0;
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            return resultMessage;
                //        }
                //    }
                //}
                //else
                //{
                //    //Sanjay [2017-02-23] These details are not compulsory while entry
                //    //tran.Rollback();
                //    //resultMessage.Text = "OrderDetailsLst Not Found  While SaveNewBooking";
                //    //resultMessage.MessageType = ResultMessageE.Error;
                //    //return resultMessage;
                //}
                //#endregion

                //#region 4. Save Order Delivery Addresses

                //if (tblBookingsTO.DeliveryAddressLst != null && tblBookingsTO.DeliveryAddressLst.Count > 0)
                //{
                //    for (int i = 0; i < tblBookingsTO.DeliveryAddressLst.Count; i++)
                //    {
                //        if (string.IsNullOrEmpty(tblBookingsTO.DeliveryAddressLst[i].Country))
                //            tblBookingsTO.DeliveryAddressLst[i].Country = Constants.DefaultCountry;

                //        tblBookingsTO.DeliveryAddressLst[i].BookingId = tblBookingsTO.IdBooking;
                //        result =_iTblBookingDelAddrBL.InsertTblBookingDelAddr(tblBookingsTO.DeliveryAddressLst[i], conn, tran);
                //        if (result != 1)
                //        {
                //            tran.Rollback();
                //            resultMessage.Text = "Error While Inserting Booking Del Address in Function SaveNewBooking";
                //            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            return resultMessage;
                //        }
                //    }
                //}
                //else
                //{
                //    //Sanjay [2017-03-02] These details are not compulsory while entry
                //    //tran.Rollback();
                //    //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                //    //resultMessage.MessageType = ResultMessageE.Error;
                //    //return resultMessage;
                //}
                //#endregion

                #region 5. Update Quota for Balance Qty
                //[05-09-2018] : Vijaymala added to save quota details for regular booking
                if (isRegular)
                {
                    existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty - tblBookingsTO.BookingQty;
                    existingQuotaTO.UpdatedBy = tblBookingsTO.CreatedBy;
                    existingQuotaTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblQuotaDeclarationBL.UpdateTblQuotaDeclaration(existingQuotaTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    #endregion

                    #region 6. Manage Quota Consumption History

                    TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO = new TblQuotaConsumHistoryTO();
                    tblQuotaConsumHistoryTO.AvailableQuota = tblBookingsTO.QuotaQtyBforBooking;
                    tblQuotaConsumHistoryTO.BalanceQuota = tblBookingsTO.QuotaQtyAftBooking;
                    tblQuotaConsumHistoryTO.BookingId = tblBookingsTO.IdBooking;
                    tblQuotaConsumHistoryTO.CreatedBy = tblBookingsTO.CreatedBy;
                    tblQuotaConsumHistoryTO.CreatedOn = tblBookingsTO.CreatedOn;
                    tblQuotaConsumHistoryTO.QuotaDeclarationId = tblBookingsTO.QuotaDeclarationId;
                    tblQuotaConsumHistoryTO.QuotaQty = -tblBookingsTO.BookingQty;
                    tblQuotaConsumHistoryTO.Remark = "New Booking for Dealer :" + tblBookingsTO.DealerName;
                    tblQuotaConsumHistoryTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.OUT;

                    result = _iTblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota InsertTblQuotaConsumHistory in Function SaveNewBooking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }

                #endregion

                #region Notifications & SMS

                // if booking withing quota then send notification to dealer confirming order detail
                // else send notification for approval of booking

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(tblBookingsTO.CnFOrgId, conn, tran);
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
                if (tblBookingsTO.IsWithinQuotaLimit == 1)
                {


                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_CONFIRMED;
                    tblAlertInstanceTO.AlertAction = "BOOKING_CONFIRMED";
                    tblAlertInstanceTO.AlertComment = "Your Booking #" + tblBookingsTO.IdBooking + " is confirmed. Rate : " + tblBookingsTO.BookingRate + " AND Qty : " + tblBookingsTO.BookingQty;
                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    // SMS to Dealer
                    Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                    if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                    {
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        TblSmsTO smsTO = new TblSmsTO();
                        smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                        smsTO.SourceTxnDesc = "New Booking";
                        string confirmMsg = string.Empty;
                        if (tblBookingsTO.IsConfirmed == 1)
                            confirmMsg = "Confirmed";
                        else
                            confirmMsg = "Not Confirmed";

                        smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                    }

                }
                else
                {
                    Boolean isCnfAcceptDirectly = false;
                    Boolean isFromNewBooking = true;
                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE)
                    {
                        isCnfAcceptDirectly = true;
                    }
                    resultMessage = SendNotification(tblBookingsTO, isCnfAcceptDirectly, isFromNewBooking, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        return resultMessage;
                    }

                    //if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL)
                    //{
                    //    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                    //    tblAlertInstanceTO.AlertAction = "BOOKING_APPROVAL_REQUIRED";
                    //    tblAlertInstanceTO.AlertComment = "Booking #" + tblBookingsTO.IdBooking + " is awaiting for your confirmation";
                    //}
                    //else
                    {
                        if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_NEW)
                        {
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                            tblAlertInstanceTO.AlertAction = "booking_approval_required";
                            tblAlertInstanceTO.AlertComment = "approval required for booking #" + tblBookingsTO.IdBooking;
                        }

                    }

                }

                tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.CreatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "New Booking";
                tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                tblAlertInstanceTO.RaisedBy = tblBookingsTO.CreatedBy;
                tblAlertInstanceTO.RaisedOn = tblBookingsTO.CreatedOn;
                tblAlertInstanceTO.IsAutoReset = 1;

                ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                    resultMessage.Text = "Error While Generating Notification";

                    return resultMessage;
                }



                //Vijaymala [06-09-2018] added to send new enquiry notification to role like Loading Person

                //tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.New_Booking;
                //tblAlertInstanceTO.AlertAction = "New_Booking";
                //tblAlertInstanceTO.AlertComment = "New Booking #" + tblBookingsTO.IdBooking + " Is Generated(" + tblBookingsTO.DealerName + ").";

                ResultMessage rMsg = new ResultMessage();


                //Vijaymala[06-09-2018] added to send new enquiry notification to role like RM
                List<TblUserAreaAllocationTO> tblUserAreaAllocationTOlist = _iTblUserAreaAllocationBL.SelectAllBookingUserAreaAllocationList(tblBookingsTO.CnFOrgId, tblBookingsTO.DealerOrgId, tblBookingsTO.BrandId, conn, tran);
                List<TblAlertUsersTO> tblAlertUsersList = new List<TblAlertUsersTO>();
                for (int i = 0; i < tblUserAreaAllocationTOlist.Count; i++)
                {
                    TblUserAreaAllocationTO tblUserAreaAllocationTO = tblUserAreaAllocationTOlist[i];
                    TblUserTO userTO = _iTblUserDAO.SelectTblUser(tblUserAreaAllocationTO.UserId, conn, tran);
                    if (userTO != null)
                    {
                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                        tblAlertUsersTO.UserId = userTO.IdUser;
                        tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                        tblAlertUsersList.Add(tblAlertUsersTO);


                    }
                }
                List<TblAlertUsersTO> distinctUSerList = tblAlertUsersList.GroupBy(w => w.UserId).Select(s => s.FirstOrDefault()).ToList();

                // tblAlertInstanceTO.AlertUsersTOList = distinctUSerList;
                // rMsg = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                // if (rMessage.MessageType != ResultMessageE.Information)
                // {
                //     tran.Rollback();
                //     resultMessage.DefaultBehaviour();
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.Text = "Error While Generating Notification";

                //    return resultMessage;
                // }

                //Priyanka [03-10-2018] Added to send notification to RM.
                //tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.DIRECTOR_REMARK_IN_BOOKING;
                //tblAlertInstanceTO.AlertAction = "DIRECTOR_REMARK_IN_BOOKING";
                //tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.IdBooking + " Director has remark -" + tblBookingsTO.DirectorRemark;
                //tblAlertInstanceTO.AlertUsersTOList = distinctUSerList;
                //rMsg = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //if (rMsg.MessageType != ResultMessageE.Information)
                //{
                //    tran.Rollback();
                //    resultMessage.DefaultBehaviour();
                //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                //    resultMessage.Text = "Error While Generating Notification";

                //    return resultMessage;
                //}


                //Priyanka [04-10-18] Added to send notification to Sales Engineer
                if (!string.IsNullOrEmpty(tblBookingsTO.DirectorRemark))
                {
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.DIRECTOR_REMARK_IN_BOOKING;
                    tblAlertInstanceTO.AlertAction = "DIRECTOR_REMARK_IN_BOOKING";
                    tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.IdBooking + " Director has remark -" + tblBookingsTO.DirectorRemark;
                    tblAlertInstanceTO.SourceDisplayId = "Director Remark for booking";

                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    tblAlertInstanceTO.AlertUsersTOList.AddRange(distinctUSerList);

                    rMsg = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMsg.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Text = "Error While Generating Notification";
                        return resultMessage;
                    }
                }


                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                if (tblBookingsTO.IsWithinQuotaLimit == 1)
                {
                    resultMessage.Text = "Success, Enquiry # - " + tblBookingsTO.IdBooking + " is generated Successfully.";
                    resultMessage.DisplayMessage = "Enquiry # - " + tblBookingsTO.IdBooking + " is generated Successfully.";
                    resultMessage.Tag = tblBookingsTO.IdBooking;
                }
                else
                {

                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                    {
                        resultMessage.Text = "Success, Enquiry # - " + tblBookingsTO.IdBooking + " is accepted";
                        resultMessage.DisplayMessage = "Enquiry # - " + tblBookingsTO.IdBooking + " is accepted";
                    }
                    else
                    {
                        resultMessage.Text = "Success, Enquiry # - " + tblBookingsTO.IdBooking + " is generated Successfully But Sent For Approval";
                        resultMessage.DisplayMessage = "Enquiry # - " + tblBookingsTO.IdBooking + " is generated Successfully But Sent For Approval";
                    }

                    resultMessage.Tag = tblBookingsTO.IdBooking;
                }

                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.Text = "Exception Error While Record Save : SaveNewBooking";
                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblBookings(TblBookingsTO tblBookingsTO)
        {
            return _iTblBookingsDAO.UpdateTblBookings(tblBookingsTO);
        }

        public int UpdateTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.UpdateTblBookings(tblBookingsTO, conn, tran);
        }

        /// <summary>
        /// Sanjay [2017-02-17] To Update the Pending Booking qty after the Loading activity is done
        /// </summary>
        /// <param name="tblBookingsTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int UpdateBookingPendingQty(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
        }

        public ResultMessage UpdateBookingConfirmations(TblBookingsTO tblBookingsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Add Record in tblBookingBeyondQuota For History

                TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                tblBookingBeyondQuotaTO = tblBookingsTO.GetBookingBeyondQuotaTO();

                tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusRemark = tblBookingsTO.StatusRemark;
                result = _iTblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                #endregion

                #region 2. Update Booking Information

                TblBookingsTO existingTblBookingsTO = SelectTblBookingsTO(tblBookingsTO.IdBooking, conn, tran);

                Boolean isCnfAcceptDirectly = false;
                //Saket [2017-11-10] Commented SRJ

                ////if(tblBookingsTO.TranStatusE== Constants.TranStatusE.BOOKING_APPROVED_DIRECTORS  //Saket [2017-11-10] Commented SRJ
                //if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE  
                //    && tblBookingsTO.BookingQty==existingTblBookingsTO.BookingQty
                //    && tblBookingsTO.BookingRate==existingTblBookingsTO.BookingRate
                //    && tblBookingsTO.DeliveryDays==existingTblBookingsTO.DeliveryDays
                //    && tblBookingsTO.CdStructureId==existingTblBookingsTO.CdStructureId
                //    && tblBookingsTO.OrcAmt==existingTblBookingsTO.OrcAmt
                //    )
                //{
                //    isCnfAcceptDirectly = true;
                //}

                //Sanjay [2017-11-13]
                //if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE
                //    && tblBookingsTO.OverdueAmt==0)
                //{
                //    isCnfAcceptDirectly = true;
                //}

                if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE)
                {
                    isCnfAcceptDirectly = true;
                }
                //else if(tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                //{
                //    isCnfAcceptDirectly = true;

                //}

                Double diffQty = 0;
                //if (tblBookingsTO.IsDeleted == 1 || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_CANDF
                //    || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_DIRECTORS
                if (tblBookingsTO.IsDeleted == 1 || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR
                    || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_FINANCE
                    || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_MARKETING)
                    diffQty = existingTblBookingsTO.BookingQty - 0;
                else
                    diffQty = existingTblBookingsTO.BookingQty - tblBookingsTO.BookingQty;

                Double pendBookQtyToUpdate = 0;
                Double pendQuotaQtyToUpdate = 0;
                if (diffQty != 0)
                {
                    if (diffQty < 0)
                    {
                        pendBookQtyToUpdate = Math.Abs(diffQty);
                        pendQuotaQtyToUpdate = diffQty;
                    }
                    else
                    {
                        pendBookQtyToUpdate = -diffQty;
                        pendQuotaQtyToUpdate = diffQty;
                    }
                }

                Double pendingBookingQty = existingTblBookingsTO.PendingQty;
                existingTblBookingsTO.PendingQty = existingTblBookingsTO.PendingQty + pendBookQtyToUpdate;

                if (existingTblBookingsTO.PendingQty < 0)
                    existingTblBookingsTO.PendingQty = 0;

                existingTblBookingsTO.BookingQty = tblBookingsTO.BookingQty;
                existingTblBookingsTO.BookingRate = tblBookingsTO.BookingRate;
                existingTblBookingsTO.DeliveryDays = tblBookingsTO.DeliveryDays;
                existingTblBookingsTO.CdStructureId = tblBookingsTO.CdStructureId;
                existingTblBookingsTO.CdStructure = tblBookingsTO.CdStructure;
                existingTblBookingsTO.OrcAmt = tblBookingsTO.OrcAmt;

                existingTblBookingsTO.StatusId = tblBookingsTO.StatusId;
                existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                existingTblBookingsTO.StatusBy = tblBookingsTO.UpdatedBy;                            //Priyanka [27-07-2018]
                result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblBookings";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                //Sanjay [2017-06-06] if booking is deleted then maintain history of pending qty
                if (tblBookingsTO.IsDeleted == 1 || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_DELETE)
                {
                    TblBookingQtyConsumptionTO bookingQtyConsumptionTO = new TblBookingQtyConsumptionTO();
                    bookingQtyConsumptionTO.BookingId = existingTblBookingsTO.IdBooking;
                    bookingQtyConsumptionTO.ConsumptionQty = pendingBookingQty;
                    bookingQtyConsumptionTO.CreatedBy = tblBookingsTO.UpdatedBy;
                    bookingQtyConsumptionTO.CreatedOn = tblBookingsTO.UpdatedOn;
                    bookingQtyConsumptionTO.StatusId = (int)tblBookingsTO.TranStatusE;
                    bookingQtyConsumptionTO.Remark = "Booking Deleted";

                    result = _iTblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(bookingQtyConsumptionTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblBookingQtyConsumption";
                        resultMessage.Tag = bookingQtyConsumptionTO;
                        return resultMessage;
                    }
                }

                #region 5. Update Quota for Balance Qty

                //[05-09-2018] : Vijaymala added to save quota details for regular booking
                Boolean isRegular = true;
                if (tblBookingsTO.BookingType == (int)Constants.BookingType.IsOther)
                {
                    isRegular = false;
                }
                if (isRegular)
                {
                    TblQuotaDeclarationTO existingQuotaTO = _iTblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);
                    if (existingQuotaTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Existing Quota Not Found In Function UpdateBookingConfirmations";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    //tblBookingsTO.QuotaQtyBforBooking = Convert.ToInt32(existingQuotaTO.BalanceQty);
                    //tblBookingsTO.QuotaQtyAftBooking = Convert.ToInt32(tblBookingsTO.QuotaQtyBforBooking - tblBookingsTO.BookingQty);
                    Double availQuota = existingQuotaTO.BalanceQty;
                    existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty + pendQuotaQtyToUpdate;
                    existingQuotaTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                    existingQuotaTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblQuotaDeclarationBL.UpdateTblQuotaDeclaration(existingQuotaTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    #endregion

                    #region 6. Manage Quota Consumption History

                    TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO = new TblQuotaConsumHistoryTO();
                    tblQuotaConsumHistoryTO.AvailableQuota = availQuota;
                    tblQuotaConsumHistoryTO.BalanceQuota = existingQuotaTO.BalanceQty;
                    tblQuotaConsumHistoryTO.BookingId = existingTblBookingsTO.IdBooking;
                    tblQuotaConsumHistoryTO.CreatedBy = tblBookingsTO.UpdatedBy;
                    tblQuotaConsumHistoryTO.CreatedOn = existingQuotaTO.UpdatedOn;
                    tblQuotaConsumHistoryTO.QuotaDeclarationId = existingTblBookingsTO.QuotaDeclarationId;
                    tblQuotaConsumHistoryTO.QuotaQty = pendQuotaQtyToUpdate;

                    if (tblBookingsTO.IsDeleted == 1)
                        tblQuotaConsumHistoryTO.Remark = "Booking Deleted";
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR)
                        tblQuotaConsumHistoryTO.Remark = "Booking Rejected BY C&F";
                    else
                        tblQuotaConsumHistoryTO.Remark = "Existing Booking Updated for Dealer :" + existingTblBookingsTO.DealerName;

                    tblQuotaConsumHistoryTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.UPDATE;

                    result = _iTblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota InsertTblQuotaConsumHistory in Function SaveNewBooking";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }

                #endregion

                #endregion

                if (isCnfAcceptDirectly)
                {
                    tblBookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                    tblBookingBeyondQuotaTO = tblBookingsTO.GetBookingBeyondQuotaTO();
                    tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                    tblBookingBeyondQuotaTO.TranStatusE = Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR;
                    tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                    tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                    tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                    result = _iTblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                        resultMessage.Tag = tblBookingBeyondQuotaTO;
                        return resultMessage;
                    }

                    existingTblBookingsTO.TranStatusE = tblBookingBeyondQuotaTO.TranStatusE;
                    existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                    existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                    existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                    result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While UpdateTblBookings";
                        resultMessage.Tag = tblBookingBeyondQuotaTO;
                        return resultMessage;
                    }
                }

                //#region Notifications & SMSs
                resultMessage = SendNotification(tblBookingsTO, isCnfAcceptDirectly, false, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return resultMessage;
                }

                ////Vijaymala added[03-05-2018]to change loading slip notification with party name
                //TblConfigParamsTO dealerNameConfTO = BL._iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);
                //Int32 dealerNameActive = 0;
                //if (dealerNameConfTO != null)
                //    dealerNameActive = Convert.ToInt32(dealerNameConfTO.ConfigParamVal);

                //    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE ||
                //    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_FINANCE||
                //    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL ||
                //    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR ||
                //    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR ||
                //    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR)
                //{

                //    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                //    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                //    List<TblUserTO> cnfUserList = _iTblUserBL.SelectAllTblUserList(tblBookingsTO.CnFOrgId, conn, tran);
                //    TblUserTO userTO = TblUserBL.SelectTblUserTO(tblBookingsTO.CreatedBy,conn,tran);
                //    if(userTO!=null)
                //    {
                //        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                //        tblAlertUsersTO.UserId = userTO.IdUser;
                //        tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                //        tblAlertUsersTOList.Add(tblAlertUsersTO);
                //    }

                //    if (cnfUserList != null && cnfUserList.Count > 0)
                //    {
                //        for (int a = 0; a < cnfUserList.Count; a++)
                //        {
                //            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                //            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                //            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                //            if (tblAlertUsersTOList != null && tblAlertUsersTOList.Count > 0)
                //            {
                //                var isExistTO = tblAlertUsersTOList.Where(x => x.UserId == tblAlertUsersTO.UserId).FirstOrDefault();
                //                if (isExistTO == null)
                //                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                //            }
                //            else
                //                tblAlertUsersTOList.Add(tblAlertUsersTO);

                //        }
                //    }
                //    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE && isCnfAcceptDirectly)
                //    {

                //        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS ;
                //        tblAlertInstanceTO.AlertAction = tblBookingsTO.TranStatusE.ToString();
                //        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is accepted";
                //        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                //        {
                //            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                //            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                //        }
                //        // SMS to Dealer
                //        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationBL.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                //        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                //        {
                //            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                //            TblSmsTO smsTO = new TblSmsTO();
                //            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                //            smsTO.SourceTxnDesc = "Booking Approved By Directors";
                //            string confirmMsg = string.Empty;
                //            if (tblBookingsTO.IsConfirmed == 1)
                //                confirmMsg = "Confirmed";
                //            else
                //                confirmMsg = "Not Confirmed";

                //            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                //            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                //            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                //            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                //        }

                //        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                //        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, conn, tran);
                //        if(result<0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While Reseting Prev Alert";
                //            return resultMessage;
                //        }
                //    }

                //    //Priyanka [30-07-2018]

                //    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR)
                //    {

                //        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR;
                //        tblAlertInstanceTO.AlertAction = tblBookingsTO.TranStatusE.ToString();

                //        tblAlertInstanceTO.AlertComment = "You Booking #" + tblBookingsTO.IdBooking + " is Hold By Admin/Director" ;
                //        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                //        {
                //            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                //            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                //        }
                //        // SMS to Dealer
                //        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationBL.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                //        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                //        {
                //            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                //            TblSmsTO smsTO = new TblSmsTO();
                //            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                //            smsTO.SourceTxnDesc = "Booking Hold By Admin/Directors";
                //            string confirmMsg ="Hold";
                //            //if (tblBookingsTO.IsConfirmed == 1)
                //            //    confirmMsg = "Confirmed";
                //            //else
                //            //    confirmMsg = "Not Confirmed";

                //            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                //            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                //            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                //            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                //        }

                //        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                //        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR, tblBookingsTO.IdBooking, conn, tran);
                //        if (result < 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While Reseting Prev Alert";
                //            return resultMessage;
                //        }
                //    }



                //    else if ((tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE && !isCnfAcceptDirectly)
                //        || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL)
                //    {
                //        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                //        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVAL_REQUIRED";
                //        tblAlertInstanceTO.AlertComment = "Booking #" + tblBookingsTO.IdBooking + " is awaiting for your confirmation";
                //        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                //        {
                //            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                //            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                //        }

                //        tblAlertInstanceTO.SourceDisplayId = "Approved By Finance";
                //        //tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                //        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, conn, tran);
                //        if (result < 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While Reseting Prev Alert";
                //            return resultMessage;
                //        }

                //    }
                //    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_FINANCE)
                //    {
                //        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS;
                //        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_DIRECTORS";
                //        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is rejected";
                //        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                //        {
                //            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                //            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                //        }
                //        tblAlertInstanceTO.SourceDisplayId = "BOOKING_REJECTED_BY_DIRECTORS";
                //        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                //        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, conn, tran);
                //        if (result < 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While Reseting Prev Alert";
                //            return resultMessage;
                //        }

                //    }
                //    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                //    {
                //        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS;
                //        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVED_BY_DIRECTORS";
                //        tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is accepted by Director";
                //        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                //        {
                //            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                //            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                //        }
                //        // SMS to Dealer
                //        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationBL.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                //        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                //        {
                //            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                //            TblSmsTO smsTO = new TblSmsTO();
                //            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                //            smsTO.SourceTxnDesc = "BOOKING_APPROVED_BY_DIRECTORS";
                //            string confirmMsg = string.Empty;
                //            if (tblBookingsTO.IsConfirmed == 1)
                //                confirmMsg = "Confirmed";
                //            else
                //                confirmMsg = "Not Confirmed";

                //            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                //            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                //            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                //            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                //        }

                //        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS, tblBookingsTO.IdBooking, conn, tran);
                //        if (result < 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While Reseting Prev Alert";
                //            return resultMessage;
                //        }
                //    }
                //    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR)
                //    {
                //        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS;
                //        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_DIRECTORS";
                //        tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is rejected by Admin/Director";
                //        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                //        {
                //            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                //            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                //        }

                //        tblAlertInstanceTO.SourceDisplayId = "BOOKING_REJECTED_BY_DIRECTORS";

                //        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS, tblBookingsTO.IdBooking, conn, tran);
                //        if (result < 0)
                //        {
                //            tran.Rollback();
                //            resultMessage.MessageType = ResultMessageE.Error;
                //            resultMessage.Text = "Error While Reseting Prev Alert";
                //            return resultMessage;
                //        }
                //    }

                //    tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.UpdatedOn;
                //    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                //    tblAlertInstanceTO.IsActive = 1;
                //    tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                //    tblAlertInstanceTO.RaisedBy = tblBookingsTO.UpdatedBy;
                //    tblAlertInstanceTO.RaisedOn = tblBookingsTO.UpdatedOn;
                //    tblAlertInstanceTO.IsAutoReset = 1;

                //    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                //    if (rMessage.MessageType != ResultMessageE.Information)
                //    {
                //        tran.Rollback();
                //        resultMessage.DefaultBehaviour();
                //        resultMessage.Text = "Error While Generating Notification";
                //        return resultMessage;
                //    }
                //}

                //#endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = resultMessage.DisplayMessage = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblBookingsTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Text = "Exception Error in Method UpdateBookingConfirmations";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// Vijaymala added to send booking notification [29-11-2018]
        /// </summary>
        /// <param name="tblBookingsTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public ResultMessage SendNotification(TblBookingsTO tblBookingsTO, Boolean isCnfAcceptDirectly, Boolean isFromNewBooking, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                #region Notifications & SMSs
                Int32 dealerNameActive = 0;
                Int32 result = 0;
                Int32 smsTemplateForSize = 0;

                //Priyanka [20-12-2018] : Added to check the setting of sms template including sizes or not
                TblConfigParamsTO SMSTemplateForSizeTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_SMS_TEMPLATE_INCLUDING_SIZE, conn, tran);
                if (SMSTemplateForSizeTO != null)
                {
                    smsTemplateForSize = Convert.ToInt32(SMSTemplateForSizeTO.ConfigParamVal);
                }
                //Vijaymala added[03-05-2018]to change loading slip notification with party name
                TblConfigParamsTO dealerNameConfTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);

                if (dealerNameConfTO != null)
                    dealerNameActive = Convert.ToInt32(dealerNameConfTO.ConfigParamVal);

                if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_NEW ||
                    tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE ||
                tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_FINANCE ||
                tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL ||
                tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR ||
                tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR ||
                tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR)
                {

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(tblBookingsTO.CnFOrgId, conn, tran);
                    if (!isFromNewBooking || tblBookingsTO.TranStatusE != Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                    {
                        TblUserTO userTO = _iTblUserDAO.SelectTblUser(tblBookingsTO.CreatedBy, conn, tran);
                        if (userTO != null)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = userTO.IdUser;
                            tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }


                    if (cnfUserList != null && cnfUserList.Count > 0)
                    {
                        for (int a = 0; a < cnfUserList.Count; a++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                            if (tblAlertUsersTOList != null && tblAlertUsersTOList.Count > 0)
                            {
                                var isExistTO = tblAlertUsersTOList.Where(x => x.UserId == tblAlertUsersTO.UserId).FirstOrDefault();
                                if (isExistTO == null)
                                    tblAlertUsersTOList.Add(tblAlertUsersTO);
                            }
                            else
                                tblAlertUsersTOList.Add(tblAlertUsersTO);

                        }
                    }
                    //Vijaymala [06-09-2018] added to send new enquiry notification to role like Loading Person
                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_NEW)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.New_Booking;
                        tblAlertInstanceTO.AlertAction = "New_Booking";
                        tblAlertInstanceTO.AlertComment = "New Booking #" + tblBookingsTO.IdBooking + " Is Generated(" + tblBookingsTO.DealerName + ").";

                        ResultMessage rMsg = new ResultMessage();


                        //Vijaymala[06-09-2018] added to send new enquiry notification to role like RM
                        List<TblUserAreaAllocationTO> tblUserAreaAllocationTOlist = _iTblUserAreaAllocationBL.SelectAllBookingUserAreaAllocationList(tblBookingsTO.CnFOrgId, tblBookingsTO.DealerOrgId, tblBookingsTO.BrandId, conn, tran);
                        List<TblAlertUsersTO> tblAlertUsersList = new List<TblAlertUsersTO>();
                        for (int i = 0; i < tblUserAreaAllocationTOlist.Count; i++)
                        {
                            TblUserAreaAllocationTO tblUserAreaAllocationTO = tblUserAreaAllocationTOlist[i];
                            TblUserTO tempUserTO = _iTblUserDAO.SelectTblUser(tblUserAreaAllocationTO.UserId, conn, tran);
                            if (tempUserTO != null)
                            {
                                TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                tblAlertUsersTO.UserId = tempUserTO.IdUser;
                                tblAlertUsersTO.DeviceId = tempUserTO.RegisteredDeviceId;
                                tblAlertUsersList.Add(tblAlertUsersTO);


                            }
                        }
                        List<TblAlertUsersTO> distinctUSerList = tblAlertUsersList.GroupBy(w => w.UserId).Select(s => s.FirstOrDefault()).ToList();

                        tblAlertInstanceTO.AlertUsersTOList = distinctUSerList;
                        //rMsg = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        //if (rMessage.MessageType != ResultMessageE.Information)
                        //{
                        //    tran.Rollback();
                        //    resultMessage.DefaultBehaviour();
                        //    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        //    resultMessage.Text = "Error While Generating Notification";

                        //    return resultMessage;
                        //}
                    }
                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE && isCnfAcceptDirectly)
                    {

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = tblBookingsTO.TranStatusE.ToString();
                        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is accepted";
                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                        }
                        // SMS to Dealer
                        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                        {
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                            smsTO.SourceTxnDesc = "Booking Approved By Directors";
                            string confirmMsg = string.Empty;
                            if (tblBookingsTO.IsConfirmed == 1)
                                confirmMsg = "Confirmed";
                            else
                                confirmMsg = "Not Confirmed";

                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    }

                    //Priyanka [30-07-2018]

                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR)
                    {

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR;
                        tblAlertInstanceTO.AlertAction = tblBookingsTO.TranStatusE.ToString();

                        tblAlertInstanceTO.AlertComment = "You Booking #" + tblBookingsTO.IdBooking + " is Hold By Admin/Director";
                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                        }
                        // SMS to Dealer
                        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                        {
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                            smsTO.SourceTxnDesc = "Booking Hold By Admin/Directors";
                            string confirmMsg = "Hold";
                            //if (tblBookingsTO.IsConfirmed == 1)
                            //    confirmMsg = "Confirmed";
                            //else
                            //    confirmMsg = "Not Confirmed";

                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.IdBooking + "";
                            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_HOLD_BY_ADMIN_OR_DIRECTOR, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            // return resultMessage;
                        }
                    }



                    else if ((tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_APPROVED_FINANCE && !isCnfAcceptDirectly)
                        || tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_PENDING_FOR_DIRECTOR_APPROVAL
                  )
                    {

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVAL_REQUIRED";
                        tblAlertInstanceTO.AlertComment = "Booking #" + tblBookingsTO.IdBooking + " is awaiting for your confirmation";
                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                        }

                        tblAlertInstanceTO.SourceDisplayId = "Approved By Finance";
                        //tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            //return resultMessage;
                        }

                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_FINANCE)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is rejected";
                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                        }
                        tblAlertInstanceTO.SourceDisplayId = "BOOKING_REJECTED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            // return resultMessage;
                        }

                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is accepted by Director";
                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        // SMS to Dealer
                        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                        if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                        {
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                            smsTO.SourceTxnDesc = "BOOKING_APPROVED_BY_DIRECTORS";
                            string confirmMsg = string.Empty;
                            if (tblBookingsTO.IsConfirmed == 1)
                                confirmMsg = "Confirmed";
                            else
                                confirmMsg = "Not Confirmed";

                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                            // var stringMaterial = String.Empty;
                            var templist = new List<TblBookingExtTO>();
                            List<string> stringMaterial = new List<string>();

                            if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                            {
                                for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst.Count; i++)
                                {
                                    var tblBookingScheduleTO = new TblBookingScheduleTO();
                                    tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[i];

                                    var tblbookingExtTO = new TblBookingExtTO();
                                    if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                                    {
                                        for (int j = 0; j < tblBookingScheduleTO.OrderDetailsLst.Count; j++)
                                        {
                                            tblbookingExtTO = tblBookingScheduleTO.OrderDetailsLst[j];
                                            templist.Add(tblbookingExtTO);
                                        }
                                        templist.Where(ele => ele.BookedQty > 0).ToList();
                                    }

                                    //List<Dictionary<String, double>> a= new List<Dictionary<string, double>>();

                                }
                                var matList = templist.GroupBy(ele => ele.MaterialSubType).Select(c => new { MaterialSubType = c.First().MaterialSubType, BookedQty = c.Sum(s => s.BookedQty) }).ToList();

                                if (tblBookingsTO.BookingType == Convert.ToInt32(Constants.BookingType.IsOther))
                                {
                                    if (matList != null && matList.Count > 0)
                                    {
                                        for (int l = 0; l < matList.Count; l++)
                                        {
                                            if (matList[l].MaterialSubType != null)
                                            {
                                                var k = matList[l].MaterialSubType.Split("-").Last() + " - " + matList[l].BookedQty + " MT ";
                                                stringMaterial.Add(k);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    for (int l = 0; l < matList.Count; l++)
                                    {
                                        var k = matList[l].MaterialSubType + " - " + matList[l].BookedQty + " MT ";
                                        stringMaterial.Add(k);
                                    }
                                }



                            }
                            var keyValue = String.Join(",", stringMaterial).ToString();
                            if (keyValue != null && keyValue != "")
                            {
                                keyValue = " Against Items - " + keyValue;
                            }
                            //for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst.Count; i++)
                            //{
                            //    var tblBookingScheduleTO = new TblBookingScheduleTO();
                            //    tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[i];

                            //    var tblbookingExtTO = new TblBookingExtTO();
                            //    for(int j = 0; j < tblBookingScheduleTO.OrderDetailsLst.Count; j++)
                            //    {
                            //        tblbookingExtTO = tblBookingScheduleTO.OrderDetailsLst[j];
                            //        templist.Add(tblbookingExtTO);
                            //    }
                            //    templist.Where(ele => ele.BookedQty > 0).ToList();
                            //    //List<Dictionary<String, double>> a= new List<Dictionary<string, double>>();

                            //}
                            //var matList = templist.GroupBy(ele => ele.MaterialSubType).Select(c => new { MaterialSubType = c.First().MaterialSubType,  BookedQty =c.Sum(s=>s.BookedQty)}).ToList();

                            //if(tblBookingsTO.BookingType == Convert.ToInt32(Constants.BookingType.IsOther))
                            //{
                            //    for (int l = 0; l < matList.Count; l++)
                            //    {
                            //        if(matList[l].MaterialSubType != null)
                            //        {
                            //            var k = matList[l].MaterialSubType.Split("-").Last() + " - " + matList[l].BookedQty + " MT ";
                            //            stringMaterial.Add(k);
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    for (int l = 0; l < matList.Count; l++)
                            //    {
                            //        var k = matList[l].MaterialSubType + " - " + matList[l].BookedQty + " MT ";
                            //        stringMaterial.Add(k);
                            //    }
                            //}



                            String SMSContent = string.Empty;
                            SMSContent = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim();

                            //Check the sms template for size
                            if (smsTemplateForSize == 1)
                            {
                                if (tblBookingsTO.BookingType == Convert.ToInt32(Constants.BookingType.IsRegular))
                                    smsTO.SmsTxt = SMSContent + keyValue + " Your Ref No : " + tblBookingsTO.IdBooking;
                                else
                                    smsTO.SmsTxt = SMSContent + keyValue + " Your Ref No : " + tblBookingsTO.IdBooking + " (Other)";
                            }
                            else
                            {
                                if (tblBookingsTO.BookingType == Convert.ToInt32(Constants.BookingType.IsRegular))
                                    smsTO.SmsTxt = SMSContent + " Your Ref No : " + tblBookingsTO.IdBooking;
                                else
                                    smsTO.SmsTxt = SMSContent + " Your Ref No : " + tblBookingsTO.IdBooking + " (Other)";
                            }
                            tblAlertInstanceTO.SmsTOList.Add(smsTO);
                        }

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            //return resultMessage;
                        }
                    }
                    else if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_REJECTED_BY_ADMIN_OR_DIRECTOR)
                    {
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_DIRECTORS";
                        tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is rejected by Admin/Director";
                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                        }
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.SourceDisplayId = "BOOKING_REJECTED_BY_DIRECTORS";

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS, tblBookingsTO.IdBooking, 0, conn, tran);
                        if (result < 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            //return resultMessage;
                        }
                    }


                    if (isFromNewBooking)
                    {
                        tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.CreatedOn;
                        tblAlertInstanceTO.SourceDisplayId = "New Booking";
                        tblAlertInstanceTO.RaisedBy = tblBookingsTO.CreatedBy;
                        tblAlertInstanceTO.RaisedOn = tblBookingsTO.CreatedOn;

                    }
                    else
                    {
                        tblAlertInstanceTO.EffectiveFromDate = tblBookingsTO.UpdatedOn;
                        tblAlertInstanceTO.RaisedBy = tblBookingsTO.UpdatedBy;
                        tblAlertInstanceTO.RaisedOn = tblBookingsTO.UpdatedOn;
                    }

                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Generating Notification";
                        // return resultMessage;
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                // resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                //return resultMessage;
            }
            return resultMessage;

            #endregion
        }
        public ResultMessage UpdateBooking(TblBookingsTO tblBookingsTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            string extingDirectorRemark = String.Empty;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Add Record in tblBookingBeyondQuota For History

                TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO = new TblBookingBeyondQuotaTO();
                tblBookingBeyondQuotaTO = tblBookingsTO.GetBookingBeyondQuotaTO();

                tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                tblBookingBeyondQuotaTO.CreatedBy = tblBookingsTO.UpdatedBy;
                tblBookingBeyondQuotaTO.CreatedOn = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusDate = tblBookingsTO.UpdatedOn;
                tblBookingBeyondQuotaTO.StatusRemark = tblBookingsTO.StatusRemark;
                result = _iTblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                #endregion

                #region 2. Update Booking Information

                TblBookingsTO existingTblBookingsTO = SelectTblBookingsTO(tblBookingsTO.IdBooking, conn, tran);
                if (!String.IsNullOrEmpty(existingTblBookingsTO.DirectorRemark))
                {
                    extingDirectorRemark = existingTblBookingsTO.DirectorRemark;
                }
                Double diffQty = existingTblBookingsTO.BookingQty - tblBookingsTO.BookingQty;
                Double pendBookQtyToUpdate = 0;
                Double pendQuotaQtyToUpdate = 0;
                if (diffQty != 0)
                {
                    if (diffQty < 0)
                    {
                        pendBookQtyToUpdate = Math.Abs(diffQty);
                        pendQuotaQtyToUpdate = diffQty;
                    }
                    else
                    {
                        pendBookQtyToUpdate = -diffQty;
                        pendQuotaQtyToUpdate = diffQty;
                    }
                }

                existingTblBookingsTO.PendingQty = existingTblBookingsTO.PendingQty + pendBookQtyToUpdate;

                existingTblBookingsTO.BookingQty = tblBookingsTO.BookingQty;
                existingTblBookingsTO.BookingRate = tblBookingsTO.BookingRate;
                existingTblBookingsTO.DeliveryDays = tblBookingsTO.DeliveryDays;
                existingTblBookingsTO.StatusId = tblBookingsTO.StatusId;
                existingTblBookingsTO.StatusDate = tblBookingsTO.StatusDate;
                existingTblBookingsTO.UpdatedOn = tblBookingsTO.UpdatedOn;
                existingTblBookingsTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                existingTblBookingsTO.NoOfDeliveries = tblBookingsTO.NoOfDeliveries;
                existingTblBookingsTO.IsConfirmed = tblBookingsTO.IsConfirmed;
                existingTblBookingsTO.IsJointDelivery = tblBookingsTO.IsJointDelivery;
                existingTblBookingsTO.IsSpecialRequirement = tblBookingsTO.IsSpecialRequirement;
                existingTblBookingsTO.CdStructure = tblBookingsTO.CdStructure;
                existingTblBookingsTO.CdStructureId = tblBookingsTO.CdStructureId;
                existingTblBookingsTO.OrcAmt = tblBookingsTO.OrcAmt;
                existingTblBookingsTO.OrcMeasure = tblBookingsTO.OrcMeasure;
                existingTblBookingsTO.BillingName = tblBookingsTO.BillingName;
                existingTblBookingsTO.Comments = tblBookingsTO.Comments;
                existingTblBookingsTO.PoNo = tblBookingsTO.PoNo;
                existingTblBookingsTO.TransporterScopeYn = tblBookingsTO.TransporterScopeYn;
                existingTblBookingsTO.VehicleNo = tblBookingsTO.VehicleNo;
                existingTblBookingsTO.FreightAmt = tblBookingsTO.FreightAmt;//Vijaymala[05-12-2017]Added
                existingTblBookingsTO.PoFileBase64 = tblBookingsTO.PoFileBase64;//Vijaymala[05-12-2017]Added
                existingTblBookingsTO.PoDate = tblBookingsTO.PoDate;//Vijaymala[27-02-2018]Added
                existingTblBookingsTO.ORCPersonName = tblBookingsTO.ORCPersonName;

                existingTblBookingsTO.SizesQty = tblBookingsTO.SizesQty;                //Priyanka [21-06-2018] Added
                existingTblBookingsTO.DirectorRemark = tblBookingsTO.DirectorRemark;    //Priyanka [25-06-2018]

                // existingTblBookingsTO.StatusBy = tblBookingsTO.StatusBy;                //PRiyanka [27-07-2018]
                existingTblBookingsTO.IsSez = tblBookingsTO.IsSez;
                existingTblBookingsTO.UomQty = tblBookingsTO.UomQty;
                existingTblBookingsTO.PendingUomQty = tblBookingsTO.PendingUomQty;
                result = UpdateTblBookings(existingTblBookingsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While InsertTblBookingBeyondQuota";
                    resultMessage.Tag = tblBookingBeyondQuotaTO;
                    return resultMessage;
                }

                #region Update commercial terms against booking 

                if (tblBookingsTO.PaymentTermOptionRelationTOLst != null && tblBookingsTO.PaymentTermOptionRelationTOLst.Count > 0)
                {
                    for (int i = 0; i < tblBookingsTO.PaymentTermOptionRelationTOLst.Count; i++)
                    {
                        TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = _iTblPaymentTermOptionRelationBL.SelectTblPaymentTermOptionRelationTOByBookingId(tblBookingsTO.PaymentTermOptionRelationTOLst[i].BookingId, tblBookingsTO.PaymentTermOptionRelationTOLst[i].PaymentTermId);
                        if (tblPaymentTermOptionRelationTO != null)
                        {
                            tblPaymentTermOptionRelationTO.IsActive = 0;
                            tblPaymentTermOptionRelationTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                            tblPaymentTermOptionRelationTO.UpdatedOn = _iCommon.ServerDateTime;
                            result = _iTblPaymentTermOptionRelationBL.UpdateTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While UpdateTblPaymentTermOptionRelation";
                                resultMessage.Tag = tblBookingBeyondQuotaTO;
                                return resultMessage;
                            }
                        }
                    }
                    //}
                    //if (tblBookingsTO.PaymentTermOptionRelationTOLst != null && tblBookingsTO.PaymentTermOptionRelationTOLst.Count > 0)
                    //{
                    for (int j = 0; j < tblBookingsTO.PaymentTermOptionRelationTOLst.Count; j++)
                    {
                        TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTONew = tblBookingsTO.PaymentTermOptionRelationTOLst[j];
                        tblPaymentTermOptionRelationTONew.CreatedBy = tblBookingsTO.CreatedBy;
                        tblPaymentTermOptionRelationTONew.CreatedOn = _iCommon.ServerDateTime;

                        result = _iTblPaymentTermOptionRelationBL.InsertTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTONew, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblPaymentTermOptionRelation";
                            resultMessage.Tag = tblBookingBeyondQuotaTO;
                            return resultMessage;
                        }
                    }
                }

                #endregion

                #region 2.1. Update Quota for Balance Qty


                Boolean isRegular = true;
                //[05-09-2018] : Vijaymala added to differentiate  regular or other booking
                if (tblBookingsTO.BookingType == (int)Constants.BookingType.IsOther)
                {
                    isRegular = false;
                }

                if (isRegular)
                {
                    TblQuotaDeclarationTO existingQuotaTO = _iTblQuotaDeclarationBL.SelectTblQuotaDeclarationTO(tblBookingsTO.QuotaDeclarationId, conn, tran);
                    if (existingQuotaTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Existing Quota Not Found In Function UpdateBookingConfirmations";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    Double availQuota = existingQuotaTO.BalanceQty;
                    existingQuotaTO.BalanceQty = existingQuotaTO.BalanceQty + pendQuotaQtyToUpdate;
                    existingQuotaTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                    existingQuotaTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblQuotaDeclarationBL.UpdateTblQuotaDeclaration(existingQuotaTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota UpdateTblQuotaDeclaration in Function SaveNewBooking";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }

                    #endregion

                    #region 2.2. Manage Quota Consumption History

                    TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO = new TblQuotaConsumHistoryTO();
                    tblQuotaConsumHistoryTO.AvailableQuota = availQuota;
                    tblQuotaConsumHistoryTO.BalanceQuota = existingQuotaTO.BalanceQty;
                    tblQuotaConsumHistoryTO.BookingId = existingTblBookingsTO.IdBooking;
                    tblQuotaConsumHistoryTO.CreatedBy = tblBookingsTO.UpdatedBy;
                    tblQuotaConsumHistoryTO.CreatedOn = existingQuotaTO.UpdatedOn;
                    tblQuotaConsumHistoryTO.QuotaDeclarationId = existingTblBookingsTO.QuotaDeclarationId;
                    tblQuotaConsumHistoryTO.QuotaQty = pendQuotaQtyToUpdate;
                    tblQuotaConsumHistoryTO.Remark = "Existing Booking Updated for Dealer :" + existingTblBookingsTO.DealerName;
                    tblQuotaConsumHistoryTO.TxnOpTypeId = (int)Constants.TxnOperationTypeE.UPDATE;

                    result = _iTblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.Text = "Error While Updating Quota InsertTblQuotaConsumHistory in Function SaveNewBooking";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }

                #endregion

                #endregion


                #region 3. Update Schedule and Materialwise Qty and Rate

                //Delete Previous Records against Booking'


                List<TblBookingScheduleTO> tblBookingScheduleTOList = _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(tblBookingsTO.IdBooking, conn, tran);

                for (int l = 0; l < tblBookingScheduleTOList.Count; l++)
                {

                    //Delete Ext
                    result = _iTblBookingExtDAO.DeleteTblBookingExtBySchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        //resultMessage.Text = "Error While Deliting Booking Delivery Address   in Function UpdateBooking";
                        resultMessage.Text = "Sorry..Record Could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = -1;
                        return resultMessage;
                    }

                    //Delete Address
                    result = _iTblBookingDelAddrDAO.DeleteTblBookingDelAddrByScheduleId(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        // resultMessage.Text = "Error While Deliting Booking Delivery Address   in Function UpdateBooking";
                        resultMessage.Text = "Sorry..Record Could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = -1;
                        return resultMessage;
                    }

                    //Delete Schedule
                    result = _iTblBookingScheduleDAO.DeleteTblBookingSchedule(tblBookingScheduleTOList[l].IdSchedule, conn, tran);
                    if (result == -1)
                    {
                        tran.Rollback();
                        //resultMessage.Text = "Error While Deleting Booking Schedule  in Function UpdateBooking";
                        resultMessage.Text = "Sorry..Record Could not be saved";
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Result = -1;
                        return resultMessage;
                    }

                }

                #region 3.Save project schedule 
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst.Count; i++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[i];
                        tblBookingScheduleTO.BookingId = tblBookingsTO.IdBooking;
                        if (tblBookingScheduleTO.CreatedBy != 0)
                        {
                            tblBookingScheduleTO.UpdatedBy = tblBookingsTO.UpdatedBy;
                            tblBookingScheduleTO.UpdatedOn = tblBookingsTO.UpdatedOn;

                        }
                        else
                        {
                            tblBookingScheduleTO.CreatedBy = tblBookingsTO.UpdatedBy;
                            tblBookingScheduleTO.CreatedOn = tblBookingsTO.UpdatedOn;

                        }



                        result = _iTblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingSchedule in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }


                        #region 3.1. Save Materialwise Qty and Rate
                        //[05-09-2018] : Vijaymala added to get default brand for other booking

                        List<DimBrandTO> brandList = _iDimBrandDAO.SelectAllDimBrand().Where(ele => ele.IsActive == 1).ToList();
                        DimBrandTO dimBrandTO = new DimBrandTO();
                        if (!isRegular)
                        {
                            if (brandList != null && brandList.Count > 0)
                            {
                                dimBrandTO = brandList.Where(ele => ele.IsDefault == 1).FirstOrDefault();
                            }
                        }

                        if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                        {
                            for (int j = 0; j < tblBookingScheduleTO.OrderDetailsLst.Count; j++)
                            {
                                TblBookingExtTO tblBookingExtTO = tblBookingScheduleTO.OrderDetailsLst[j];
                                tblBookingExtTO.BookingId = tblBookingsTO.IdBooking;
                                tblBookingExtTO.Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material
                                tblBookingExtTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                //[05-09-2018] : Vijaymala added to get default brand for other booking
                                if (!isRegular)
                                {
                                    if (dimBrandTO != null)
                                    {
                                        tblBookingExtTO.BrandId = dimBrandTO.IdBrand;
                                    }
                                }

                                result = _iTblBookingExtDAO.InsertTblBookingExt(tblBookingExtTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblBookingExt in Function SaveNewBooking";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.Result = 0;
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {

                        }
                        #endregion
                        #region 3.2. Save Order Delivery Addresses

                        if (tblBookingScheduleTO.DeliveryAddressLst != null && tblBookingScheduleTO.DeliveryAddressLst.Count > 0)
                        {
                            for (int k = 0; k < tblBookingScheduleTO.DeliveryAddressLst.Count; k++)
                            {
                                TblBookingDelAddrTO tblBookingDelAddrTO = tblBookingScheduleTO.DeliveryAddressLst[k];
                                if (string.IsNullOrEmpty(tblBookingDelAddrTO.Country))
                                    tblBookingDelAddrTO.Country = Constants.DefaultCountry;

                                tblBookingDelAddrTO.BookingId = tblBookingsTO.IdBooking;
                                tblBookingDelAddrTO.ScheduleId = tblBookingScheduleTO.IdSchedule;
                                result = _iTblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingDelAddrTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.Text = "Error While Inserting Booking Del Address in Function SaveNewBooking";
                                    resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            //Sanjay [2017-03-02] These details are not compulsory while entry
                            //tran.Rollback();
                            //resultMessage.Text = "Delivery Address Not Found - Function SaveNewBooking";
                            //resultMessage.MessageType = ResultMessageE.Error;
                            //return resultMessage;
                        }
                        #endregion


                    }

                }
                else
                {

                }
                #endregion
                #region Notifications & SMS

                // if booking withing quota then send notification to dealer confirming order detail
                // else send notification for approval of booking



                //Priyanka [04-10-18] Added to send notification to Sales Engineer
                if (!string.IsNullOrEmpty(existingTblBookingsTO.DirectorRemark))
                {
                    if (extingDirectorRemark != null && (existingTblBookingsTO.DirectorRemark.TrimEnd() != extingDirectorRemark.TrimEnd()))
                    {
                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                        List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(existingTblBookingsTO.CnFOrgId, conn, tran);
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


                        tblAlertInstanceTO.EffectiveFromDate = existingTblBookingsTO.UpdatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceEntityId = existingTblBookingsTO.IdBooking;
                        tblAlertInstanceTO.RaisedBy = existingTblBookingsTO.UpdatedBy;
                        tblAlertInstanceTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;

                        ResultMessage rMsg = new ResultMessage();

                        //Vijaymala[06-09-2018] added to send new enquiry notification to role like RM
                        List<TblUserAreaAllocationTO> tblUserAreaAllocationTOlist = _iTblUserAreaAllocationBL.SelectAllBookingUserAreaAllocationList(tblBookingsTO.CnFOrgId, tblBookingsTO.DealerOrgId, tblBookingsTO.BrandId, conn, tran);
                        List<TblAlertUsersTO> tblAlertUsersList = new List<TblAlertUsersTO>();
                        for (int i = 0; i < tblUserAreaAllocationTOlist.Count; i++)
                        {
                            TblUserAreaAllocationTO tblUserAreaAllocationTO = tblUserAreaAllocationTOlist[i];
                            TblUserTO userTO = _iTblUserDAO.SelectTblUser(tblUserAreaAllocationTO.UserId, conn, tran);
                            if (userTO != null)
                            {
                                TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                tblAlertUsersTO.UserId = userTO.IdUser;
                                tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                                tblAlertUsersList.Add(tblAlertUsersTO);


                            }
                        }
                        List<TblAlertUsersTO> distinctUSerList = tblAlertUsersList.GroupBy(w => w.UserId).Select(s => s.FirstOrDefault()).ToList();

                        tblAlertInstanceTO.AlertUsersTOList = distinctUSerList;


                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.DIRECTOR_REMARK_IN_BOOKING;
                        tblAlertInstanceTO.AlertAction = "DIRECTOR_REMARK_IN_BOOKING";
                        tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.IdBooking + " Director has remark -" + tblBookingsTO.DirectorRemark;
                        tblAlertInstanceTO.SourceDisplayId = "Director Remark for booking";

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.AlertUsersTOList.AddRange(distinctUSerList);

                        rMsg = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (rMsg.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Text = "Error While Generating Notification";
                            return resultMessage;
                        }
                    }
                }


                #endregion
                #endregion

                #region Notifications & SMS

                // if booking withing quota then send notification to dealer confirming order detail
                // else send notification for approval of booking



                //Priyanka [04-10-18] Added to send notification to Sales Engineer
                if (!string.IsNullOrEmpty(existingTblBookingsTO.DirectorRemark))
                {
                    if (extingDirectorRemark != null && (existingTblBookingsTO.DirectorRemark.TrimEnd() != extingDirectorRemark.TrimEnd()))
                    {
                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                        List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(existingTblBookingsTO.CnFOrgId, conn, tran);
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


                        tblAlertInstanceTO.EffectiveFromDate = existingTblBookingsTO.UpdatedOn;
                        tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.SourceEntityId = existingTblBookingsTO.IdBooking;
                        tblAlertInstanceTO.RaisedBy = existingTblBookingsTO.UpdatedBy;
                        tblAlertInstanceTO.RaisedOn = existingTblBookingsTO.UpdatedOn;
                        tblAlertInstanceTO.IsAutoReset = 1;

                        ResultMessage rMsg = new ResultMessage();

                        //Vijaymala[06-09-2018] added to send new enquiry notification to role like RM
                        List<TblUserAreaAllocationTO> tblUserAreaAllocationTOlist = _iTblUserAreaAllocationBL.SelectAllBookingUserAreaAllocationList(tblBookingsTO.CnFOrgId, tblBookingsTO.DealerOrgId, tblBookingsTO.BrandId, conn, tran);
                        List<TblAlertUsersTO> tblAlertUsersList = new List<TblAlertUsersTO>();
                        for (int i = 0; i < tblUserAreaAllocationTOlist.Count; i++)
                        {
                            TblUserAreaAllocationTO tblUserAreaAllocationTO = tblUserAreaAllocationTOlist[i];
                            TblUserTO userTO = _iTblUserDAO.SelectTblUser(tblUserAreaAllocationTO.UserId, conn, tran);
                            if (userTO != null)
                            {
                                TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                tblAlertUsersTO.UserId = userTO.IdUser;
                                tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                                tblAlertUsersList.Add(tblAlertUsersTO);


                            }
                        }
                        List<TblAlertUsersTO> distinctUSerList = tblAlertUsersList.GroupBy(w => w.UserId).Select(s => s.FirstOrDefault()).ToList();

                        tblAlertInstanceTO.AlertUsersTOList = distinctUSerList;


                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.DIRECTOR_REMARK_IN_BOOKING;
                        tblAlertInstanceTO.AlertAction = "DIRECTOR_REMARK_IN_BOOKING";
                        tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.IdBooking + " Director has remark -" + tblBookingsTO.DirectorRemark;
                        tblAlertInstanceTO.SourceDisplayId = "Director Remark for booking";

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        tblAlertInstanceTO.AlertUsersTOList.AddRange(distinctUSerList);

                        rMsg = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (rMsg.MessageType != ResultMessageE.Information)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Text = "Error While Generating Notification";
                            return resultMessage;
                        }
                    }
                }


                #endregion

                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Booking Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblBookingsTO;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.Text = "Exception Error in Method UpdateBookingConfirmations";
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        //Priyanka [11-06-2018] : Added for post the overdue status from booking for SHIVANGI.
        public ResultMessage PostUpdateOverdueExistOrNotFromBooking(TblBookingsTO bookingTo, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            DateTime serverDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Update table TblBookings.

                result = UpdateTblBookings(bookingTo, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Update Update TblBookings in Method UpdateTblBookings";
                    return resultMessage;
                }
                #endregion

                #region 2. insert into table TblOrgOverdueHistory.

                TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO = new TblOrgOverdueHistoryTO();
                tblOrgOverdueHistoryTO.CreatedBy = loginUserId;
                tblOrgOverdueHistoryTO.CreatedOn = serverDate;
                tblOrgOverdueHistoryTO.OrganizationId = bookingTo.DealerOrgId;
                tblOrgOverdueHistoryTO.IsOverdueExist = bookingTo.IsOverdueExist;
                tblOrgOverdueHistoryTO.BookingId = bookingTo.IdBooking;
                result = _iTblOrgOverdueHistoryDAO.InsertTblOrgOverdueHistory(tblOrgOverdueHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Update InsertTblOrgOverdueHistory ";
                    return resultMessage;
                }
                #endregion


                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = bookingTo;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateOverdueExistOrNotFromBooking");
                return resultMessage;
            }
            finally
            {
                conn.Close();

            }
        }


        public Int32 MigrateBookingSizesQty()
        {
            List<TblBookingsTO> tblBookingsTOList = _iTblBookingsDAO.SelectAllTblBookings();

            String notUpdateBookings = String.Empty;

            for (int i = 0; i < tblBookingsTOList.Count; i++)
            {
                TblBookingsTO tblBookingsTO = tblBookingsTOList[i];

                List<TblBookingScheduleTO> TblBookingScheduleTOList = _iCircularDependencyBL.SelectBookingScheduleByBookingId(tblBookingsTO.IdBooking);

                Double totalSizes = 0;

                if (TblBookingScheduleTOList != null && TblBookingScheduleTOList.Count > 0)
                {
                    for (int j = 0; j < TblBookingScheduleTOList.Count; j++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = TblBookingScheduleTOList[j];

                        List<TblBookingExtTO> tblBookingExtTOList = _iTblBookingExtDAO.SelectAllTblBookingExtListBySchedule(tblBookingScheduleTO.IdSchedule);

                        if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                        {
                            totalSizes += tblBookingExtTOList.Sum(s => s.BookedQty);
                        }
                    }
                }

                tblBookingsTO.SizesQty = totalSizes;
                if (totalSizes > 0)
                {
                    if (tblBookingsTO.UpdatedBy == 0)
                        tblBookingsTO.UpdatedBy = 1;
                    if (tblBookingsTO.UpdatedOn == new DateTime())
                        tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;

                    //update booing
                    Int32 result = UpdateTblBookings(tblBookingsTO);
                    if (result != 1)
                    {
                        notUpdateBookings += " IdBooking - " + tblBookingsTO.IdBooking + ", ";
                    }
                }

            }

            return 1;
        }


        #endregion

        #region Deletion
        public int DeleteTblBookings(Int32 idBooking)
        {
            return _iTblBookingsDAO.DeleteTblBookings(idBooking);
        }

        public int DeleteTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.DeleteTblBookings(idBooking, conn, tran);
        }

        //Shifted in tblloadingbl.cs YA_2019_02_11
        //public ResultMessage DeleteAllBookings(List<Int32> bookingsIdList)
        //{
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    DateTime txnDateTime = _iCommon.ServerDateTime;
        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();

        //        if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
        //        {

        //            #region Check Final Item

        //            resultMessage = DeleteAllBookings(bookingsIdList, conn, tran);
        //            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
        //            {
        //                tran.Rollback();
        //                return resultMessage;
        //            }

        //            #endregion

        //            tran.Commit();
        //        }
        //        return resultMessage;
        //    }

        //    catch (Exception ex)
        //    {

        //        tran.Rollback();
        //        resultMessage.DefaultExceptionBehaviour(ex, "Exception Error In MEthod RemoveItemFromLoadingSlip");
        //        return resultMessage;


        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
        //Shifted in tblloadingbl.cs YA_2019_02_11
        //public ResultMessage DeleteAllBookings(List<int> bookingsIdsList, SqlConnection conn, SqlTransaction tran)
        //{
        //    ResultMessage resultMessage = new ResultMessage();

        //    List<TblBookingsTO> allBooking = SelectAllTblBookingsList();

        //    allBooking = allBooking.Where(w => w.PendingQty == 0).ToList();

        //    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_DELETE_BEFORE_DAYS, conn, tran);
        //    if (tblConfigParamsTO == null)
        //    {
        //        resultMessage.DefaultBehaviour("Error tblConfigParamsTO is null");
        //        return null;
        //    }

        //    DateTime statusDate = _iCommon.ServerDateTime.AddDays(-Convert.ToInt32(tblConfigParamsTO.ConfigParamVal));

        //    allBooking = allBooking.Where(w => w.CreatedOn.Date <= statusDate.Date).ToList();

        //    for (int i = 0; i < allBooking.Count; i++)
        //    {
        //        TblBookingsTO TblBookingsTOTemp = allBooking[i];

        //        if (TblBookingsTOTemp.PendingQty <= 0)
        //        {
        //            Int32 result1 = DeleteDispatchBookingData(TblBookingsTOTemp.IdBooking, conn, tran);
        //            if (result1 < 0)
        //            {
        //                //tran.Rollback();
        //                //resultMessage.DefaultBehaviour("Error while Deleting BookingDispatchData");

        //            }
        //        }
        //    }


        //    Int32 result = 0;
        //    //if (bookingsIdsList != null && bookingsIdsList.Count > 0)
        //    //{
        //    //    for (int s = 0; s < bookingsIdsList.Count; s++)
        //    //    {
        //    //        Int32 bookingId = bookingsIdsList[s];
        //    //        TblBookingsTO tblBookingTO = BL.TblBookingsBL.SelectBookingsTOWithDetails(bookingId);
        //    //        if (tblBookingTO != null)
        //    //        {

        //    //            if (tblBookingTO.PendingQty <= 0)
        //    //            {
        //    //                result = BL.FinalBookingData.DeleteDispatchBookingData(tblBookingTO.IdBooking, conn, tran);
        //    //                if (result < 0)
        //    //                {
        //    //                    tran.Rollback();
        //    //                    resultMessage.DefaultBehaviour("Error while Deleting BookingDispatchData");

        //    //                }

        //                if (TblBookingsTOTemp.PendingQty <= 0)
        //                {

        //                    List<TblLoadingSlipDtlTO> tblLoadingSlipDtlTOList = BL.TblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromBookingId(TblBookingsTOTemp.IdBooking, conn, tran);

        //                    if (tblLoadingSlipDtlTOList == null || tblLoadingSlipDtlTOList.Count == 0)
        //                    {
        //                        Int32 result1 = BL.FinalBookingData.DeleteDispatchBookingData(TblBookingsTOTemp.IdBooking, conn, tran);
        //                        if (result1< 0)
        //                        {
        //                            //tran.Rollback();
        //                            //resultMessage.DefaultBehaviour("Error while Deleting BookingDispatchData");

        //                        }
        //}
        //                }
        //            }

        //    //}
        //    resultMessage.DefaultSuccessBehaviour();
        //    return resultMessage;


        //}
        //Shifted in tblloadingbl.cs YA_2019_02_11
        //public int DeleteDispatchBookingData(Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        //{

        //    ResultMessage resultMessage = new ResultMessage();
        //    int result = 0;

        //    try
        //    {
        //        #region Delete Booking Data
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingBeyondQuota.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingBeyondQuota");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingExt.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingExt");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingParities.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingParities");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingQtyConsumption.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingQtyConsumption");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingDelAddr.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingDelAddr");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingSchedule.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingSchedule");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblQuotaConsumHistory.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblQuotaConsumHistory");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookingOpngBal.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookingOpngBal");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblLoadingSlipRemovedItems.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblLoadingSlipRemovedItems");
        //            return -1;
        //        }
        //        result = DeleteDispatchBookingTempData(DelTranTablesE.tblBookings.ToString(), bookingId, conn, tran);
        //        if (result < 0)
        //        {
        //            resultMessage.DefaultBehaviour("Error while Deleting tblBookings");
        //            return -1;
        //        }

        //        #endregion


        //        resultMessage.DefaultSuccessBehaviour();
        //        return 1;
        //    }

        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "DeleteDispatchBookingTempData");
        //        return -1;
        //    }
        //    finally
        //    {

        //    }
        //}
        //Shifted in tblloadingbl.cs YA_2019_02_11
        //private int DeleteDispatchBookingTempData(String delTableName, Int32 delId, SqlConnection conn, SqlTransaction tran)
        //{
        //    SqlCommand cmdDelete = new SqlCommand();
        //    ResultMessage resultMessage = new ResultMessage();

        //    try
        //    {
        //        cmdDelete.Connection = conn;
        //        cmdDelete.Transaction = tran;

        //        String sqlQuery = null;

        //        //Saket [2018-05-11] Added.
        //        //String strWhereCondtion = "select invoiceId FROM tempLoadingSlipInvoice WHERE loadingSlipId = " + delId + "";
        //        // String strWhereCondtion = delId.ToString();

        //        switch ((DelTranTablesE)Enum.Parse(typeof(DelTranTablesE), delTableName))
        //        {
        //            case DelTranTablesE.tblBookingBeyondQuota:
        //                //sqlQuery = " DELETE FROM tblBookingBeyondQuota WHERE bookingId = " + delId + "AND pendingQty <= 0 ";
        //                sqlQuery = " DELETE FROM tblBookingBeyondQuota WHERE bookingId =" + delId;
        //                break;
        //            case DelTranTablesE.tblBookingExt:
        //                sqlQuery = " DELETE FROM tblBookingExt WHERE scheduleId IN " +
        //                    ("(select idSchedule from tblBookingSchedule where bookingId = " + delId + ")");
        //                break;
        //            case DelTranTablesE.tblBookingParities:
        //                sqlQuery = " DELETE FROM tblBookingParities WHERE bookingId = " + delId;
        //                break;
        //            case DelTranTablesE.tblBookingQtyConsumption:
        //                sqlQuery = " DELETE FROM tblBookingQtyConsumption WHERE bookingId = " + delId;
        //                break;
        //            case DelTranTablesE.tblBookingDelAddr:
        //                sqlQuery = " DELETE FROM tblBookingDelAddr WHERE scheduleId IN " +
        //                    ("(select idSchedule from tblBookingSchedule where bookingId = " + delId + ")");
        //                break;
        //            case DelTranTablesE.tblBookingSchedule:
        //                sqlQuery = " DELETE FROM tblBookingSchedule WHERE bookingId = " + delId;
        //                break;
        //            case DelTranTablesE.tblQuotaConsumHistory:
        //                sqlQuery = " DELETE FROM tblQuotaConsumHistory WHERE bookingId = " + delId;
        //                break;
        //            case DelTranTablesE.tblBookingOpngBal:
        //                sqlQuery = " DELETE FROM tblBookingOpngBal WHERE bookingId = " + delId;
        //                break;
        //            case DelTranTablesE.tblLoadingSlipRemovedItems:
        //                sqlQuery = " DELETE FROM tblLoadingSlipRemovedItems WHERE bookingId = " + delId;
        //                break;
        //            case DelTranTablesE.tblBookings:
        //                sqlQuery = " DELETE FROM tblBookings WHERE idbooking = " + delId;
        //                break;
        //        }

        //        if (sqlQuery != null)
        //        {
        //            cmdDelete.CommandText = sqlQuery;
        //            return cmdDelete.ExecuteNonQuery();
        //        }
        //        else
        //            return -1;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "DeleteTempData");
        //        return -1;
        //    }
        //    finally
        //    {
        //        cmdDelete.Dispose();
        //    }
        //}

        #endregion

    }
}
