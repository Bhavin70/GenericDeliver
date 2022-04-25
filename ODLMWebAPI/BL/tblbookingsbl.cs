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
        private readonly ITblAlertDefinitionDAO _iTblAlertDefinitionDAO;
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITblEntityRangeBL _iTblEntityRangeBL;

        //Saket [2020-03-26] Locker object added.
        private static readonly object bookingNoLock = new object();

        public TblBookingsBL(ITblAlertDefinitionDAO iTblAlertDefinitionDAO,ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblMaterialDAO iTblMaterialDAO, ITblQuotaDeclarationDAO iTblQuotaDeclarationDAO, IDimensionDAO iDimensionDAO, ITblPaymentTermOptionRelationBL iTblPaymentTermOptionRelationBL, ITblOrgOverdueHistoryDAO iTblOrgOverdueHistoryDAO, ITblUserDAO iTblUserDAO, ITblAlertInstanceBL iTblAlertInstanceBL, ITblQuotaConsumHistoryDAO iTblQuotaConsumHistoryDAO, ITblBookingBeyondQuotaDAO iTblBookingBeyondQuotaDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblSysElementsBL iTblSysElementsBL, IDimBrandDAO iDimBrandDAO, ITblOrganizationDAO iTblOrganizationDAO, ITblGlobalRateDAO iTblGlobalRateDAO, ITblQuotaDeclarationBL iTblQuotaDeclarationBL, ITblBookingActionsDAO iTblBookingActionsDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblBookingQtyConsumptionDAO iTblBookingQtyConsumptionDAO, ITblBookingOpngBalDAO iTblBookingOpngBalDAO, ITblUserAreaAllocationBL iTblUserAreaAllocationBL, ICircularDependencyBL iCircularDependencyBL, ITblConfigParamsDAO iTblConfigParamsDAO, ITblBookingDelAddrDAO iTblBookingDelAddrDAO, ITblBookingExtDAO iTblBookingExtDAO, ITblBookingScheduleDAO iTblBookingScheduleDAO, ITblUserRoleBL iTblUserRoleBL, ITblEnquiryDtlDAO iTblEnquiryDtlDAO, ITblOverdueDtlDAO iTblOverdueDtlDAO, ITblBookingsDAO iTblBookingsDAO, ICommon iCommon, IConnectionString iConnectionString, ITblEntityRangeBL iTblEntityRangeBL, IDimensionBL iDimensionBL)
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
            _iTblAlertDefinitionDAO = iTblAlertDefinitionDAO;
            _iDimensionBL = iDimensionBL;
            _iTblEntityRangeBL = iTblEntityRangeBL;
        }
        #region Selection
        public List<TblBookingPendingRptTO> SelectBookingPendingQryRpt(DateTime fromDate, DateTime toDate, int reportType)
        {

            //fetch all bookings against selected dates
            List<TblBookingsTO> tblBookingTOList = _iTblBookingsDAO.SelectAllBookingDateWise(fromDate, toDate);
            if (reportType == 1 && tblBookingTOList!=null && tblBookingTOList.Count >0)
            {
                tblBookingTOList = tblBookingTOList.Where(x => x.PendingQty > 0 && x.StatusId != (Int32)Constants.TranStatusE.BOOKING_DELETE).ToList();
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
                Dictionary<int, double> globalLoadingDictionary = new Dictionary<int, double> ();
                Dictionary<int, double> globalBookingDictionary = new Dictionary<int, double>();
                for (int i = 0; i < tblBookingTOList.Count; i++)
                {
                    if(tblBookingTOList[i].IdBooking == 111891)
                    {

                    }
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
                    tblBookingPendingRptTO.BookingDisplayNo = tblBookingTOList[i].BookingDisplayNo;
                    tblBookingPendingRptTO.BookingDateStr = tblBookingTOList[i].BookingDatetime.ToShortDateString();
                    tblBookingPendingRptTO.DealerId = tblBookingTOList[i].DealerOrgId;
                    tblBookingPendingRptTO.DealerName = tblBookingTOList[i].DealerName;
                    tblBookingPendingRptTO.TotalBookedQty = tblBookingTOList[i].BookingQty;
                    tblBookingPendingRptTO.PendingQty = tblBookingTOList[i].PendingQty;
                    tblBookingPendingRptTO.CnfName = tblBookingTOList[i].CnfName;

                    //Deepali Added to get all layers qty.[16-06-2021]
                    bookingDictionary = new Dictionary<int, double>();

                    foreach (var bookingSchedule in tblBookingTOList[i].BookingScheduleTOLst)
                    {
                        //Deepali Commented to get all layers qty.[16-06-2021]
                        //bookingDictionary = new Dictionary<int, double>();
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
                            //Reshma Added
                            if (!globalBookingDictionary.ContainsKey(eachMaterial.Value))
                            {
                                globalBookingDictionary.Add(eachMaterial.Value, bookedQty);
                            }
                            else
                            {
                                if (bookedQty > 0)
                                {
                                    double val = Convert.ToDouble(globalBookingDictionary[eachMaterial.Value]);
                                    double bookingQtytemp = bookedQty;
                                    bookingQtytemp += val;
                                    globalBookingDictionary[eachMaterial.Value] = bookingQtytemp;
                                }
                            }
                        }
                        if (loadingDictionary != null)
                        {
                            if (loadingDictionary.ContainsKey(eachMaterial.Value))
                            {
                                loadQty = Convert.ToDouble(loadingDictionary[eachMaterial.Value]);
                                totalLoadQty += loadQty;
                            }
                            //Reshma Added
                            if (!globalLoadingDictionary.ContainsKey(eachMaterial.Value))
                            {
                                globalLoadingDictionary.Add(eachMaterial.Value, loadQty);
                            }
                            else
                            {
                                if (loadQty > 0)
                                {
                                    double val = Convert.ToDouble(globalLoadingDictionary[eachMaterial.Value]);
                                    double loadQtytemp = loadQty;
                                    loadQtytemp += val;
                                    globalLoadingDictionary[eachMaterial.Value] = loadQtytemp;
                                }
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

        public List<TblBookingPendingRptTO> SelectBookingPendingOrderQtyRpt(DateTime fromDate, DateTime toDate, int reportType)
        {

            //fetch all bookings against selected dates
            List<TblBookingsTO> tblBookingTOList = _iTblBookingsDAO.SelectAllBookingDateWise(fromDate, toDate);
            if (reportType == 1 && tblBookingTOList != null && tblBookingTOList.Count > 0)
            {
                tblBookingTOList = tblBookingTOList.Where(x => x.PendingQty > 0 && x.StatusId != (Int32)Constants.TranStatusE.BOOKING_DELETE).ToList();
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
                Dictionary<int, double> globalLoadingDictionary = new Dictionary<int, double>();
                Dictionary<int, double> globalBookingDictionary = new Dictionary<int, double>();
                double totalPendingBookedQty = 0.0; double totalBookedOrderQty = 0.0;
                for (int i = 0; i < tblBookingTOList.Count; i++)
                {
                    if (tblBookingTOList[i].IdBooking == 111891)
                    {

                    }
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
                    tblBookingPendingRptTO.BookingDisplayNo = tblBookingTOList[i].BookingDisplayNo;
                    tblBookingPendingRptTO.BookingDateStr = tblBookingTOList[i].BookingDatetime.ToShortDateString();
                    tblBookingPendingRptTO.DealerId = tblBookingTOList[i].DealerOrgId;
                    tblBookingPendingRptTO.DealerName = tblBookingTOList[i].DealerName;
                    tblBookingPendingRptTO.TotalBookedQty = tblBookingTOList[i].BookingQty;
                    tblBookingPendingRptTO.PendingQty = tblBookingTOList[i].PendingQty;
                    tblBookingPendingRptTO.CnfName = tblBookingTOList[i].CnfName;
                    totalPendingBookedQty = totalPendingBookedQty + tblBookingPendingRptTO.PendingQty;
                    //Deepali Added to get all layers qty.[16-06-2021]
                    bookingDictionary = new Dictionary<int, double>();

                    foreach (var bookingSchedule in tblBookingTOList[i].BookingScheduleTOLst)
                    {
                        //Deepali Commented to get all layers qty.[16-06-2021]
                        //bookingDictionary = new Dictionary<int, double>();
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
                                //totalBookedQty += bookedQty;
                            }
                           
                        }
                        if (loadingDictionary != null)
                        {
                            if (loadingDictionary.ContainsKey(eachMaterial.Value))
                            {
                                loadQty = Convert.ToDouble(loadingDictionary[eachMaterial.Value]);
                                //totalLoadQty += loadQty;
                            }
                           
                        }

                        double pendingQty = bookedQty- loadQty;
                        if (pendingQty < 0)
                            pendingQty = 0;
                        if (pendingQty > 0)
                        {
                            totalBookedQty += bookedQty;
                            totalLoadQty += loadQty;
                            if (bookingDictionary != null && loadingDictionary != null)
                            {
                                //Reshma Added
                                if (!globalBookingDictionary.ContainsKey(eachMaterial.Value))
                                {
                                    globalBookingDictionary.Add(eachMaterial.Value, bookedQty);
                                }
                                else
                                {
                                    if (bookedQty > 0)
                                    {
                                        double val = Convert.ToDouble(globalBookingDictionary[eachMaterial.Value]);
                                        double bookingQtytemp = bookedQty;
                                        bookingQtytemp += val;
                                        globalBookingDictionary[eachMaterial.Value] = bookingQtytemp;
                                    }
                                }
                                //Reshma Added
                                if (!globalLoadingDictionary.ContainsKey(eachMaterial.Value))
                                {
                                    globalLoadingDictionary.Add(eachMaterial.Value, loadQty);
                                }
                                else
                                {
                                    if (loadQty > 0)
                                    {
                                        double val = Convert.ToDouble(globalLoadingDictionary[eachMaterial.Value]);
                                        double loadQtytemp = loadQty;
                                        loadQtytemp += val;
                                        globalLoadingDictionary[eachMaterial.Value] = loadQtytemp;
                                    }
                                }

                            }
                        }
                        finalData = pendingQty.ToString("N1") ;
                        finalBookingpendingQty.Add(eachMaterial.Text, finalData);
                    }
                    double totalPendingQty = totalBookedQty - totalLoadQty;
                    if (totalPendingQty < 0)
                        totalPendingQty = 0;
                    totalQty = totalPendingQty.ToString("N1");
                    tblBookingPendingRptTO.TotalQty = totalQty;

                    totalBookedOrderQty = totalBookedOrderQty + Convert.ToDouble(tblBookingPendingRptTO.TotalQty);
                    tblBookingPendingRptTO.FinalDictionaryList.Add(finalBookingpendingQty);
                    tblBookingPendingRptTOList.Add(tblBookingPendingRptTO);
                }
                //Reshma Added For Sum
                if (tblBookingPendingRptTOList != null && tblBookingPendingRptTOList.Count > 0)
                {
                    TblBookingPendingRptTO tblBookingPendingRptTO = new TblBookingPendingRptTO();
                    double bookedQty = 0; double totalBookedQty = 0; double loadQty = 0; double totalLoadQty = 0;
                    string finalData = "";
                    string totalQty = "";
                    Dictionary<string, string> finalBookingpendingQty = new Dictionary<string, string>();
                    if (MaterialList != null && MaterialList.Count > 0)
                    {
                        //for (int i = 0; i < MaterialList.Count; i++)
                        {
                            foreach (var eachMaterial in MaterialList)
                            {
                                bookedQty = 0;loadQty = 0;
                                if (globalBookingDictionary != null)
                                {
                                    if (globalBookingDictionary.ContainsKey(eachMaterial.Value))
                                    {
                                        bookedQty = Convert.ToDouble(globalBookingDictionary[eachMaterial.Value]);
                                        if(bookedQty >0)
                                            totalBookedQty += bookedQty;
                                    }
                                }
                                if (globalLoadingDictionary != null)
                                {
                                    if (globalLoadingDictionary.ContainsKey(eachMaterial.Value))
                                    {
                                        loadQty = Convert.ToDouble(globalLoadingDictionary[eachMaterial.Value]);
                                        if(loadQty >0)
                                            totalLoadQty += loadQty;
                                    }
                                }
                                double totalPendimngQty = bookedQty - loadQty;
                                if (totalPendimngQty < 0)
                                    totalPendimngQty = 0;
                                finalData = totalPendimngQty.ToString("N1") ;
                                finalBookingpendingQty.Add(eachMaterial.Text, finalData);
                            }
                        }
                    }
                    double totalPenQty = totalBookedQty - totalLoadQty;
                    if (totalPenQty < 0)
                        totalPenQty = 0;
                    totalQty = totalPenQty.ToString("N1");
                    tblBookingPendingRptTO.DealerName = "Total";
                    tblBookingPendingRptTO.TotalQty  =Convert .ToString (Math.Round(totalBookedOrderQty ,2));

                    tblBookingPendingRptTO.PendingQty =Math.Round ( totalPendingBookedQty,2);
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


        public TblBookingsTO SelectBookingsDetailsFromInVoiceId(Int32 inInvoice, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingsDAO.SelectBookingsDetailsFromInVoiceId(inInvoice, conn, tran);
        }
        //chetan[14-feb-2020] added for get booking detai without connection transcation

        public TblBookingsTO SelectBookingsDetailsFromInVoiceId(Int32 inInvoice)
        {
            string sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING); 
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            TblBookingsTO tblBookingsTO = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                tblBookingsTO =  _iTblBookingsDAO.SelectBookingsDetailsFromInVoiceId(inInvoice, conn, tran);
                tran.Commit();
            }
            catch (Exception)
            {
                tran.Rollback();
                return null;
            }    
            finally
            {
                conn.Close();
            }
            return tblBookingsTO;
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


        public List<PendingQtyOrderTypeTo> SelectTotalPendingBookingQty(DateTime sysDate)
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


        public ResultMessage GetBookingAvgQtyDetailsStatus(int dealerOrgId, Int32 bookingId)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {

                Int32 avgBookingQtyDevDays = 0, avgBookingQtyDevPer = 0;

                TblConfigParamsTO tblConfigParamTOForQuota = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.AVERAGE_BOOKING_QTY_DAYS_AND_DEV_PERCENT);
                if (tblConfigParamTOForQuota != null && !String.IsNullOrEmpty(tblConfigParamTOForQuota.ConfigParamVal))
                {
                    List<String> tempList = tblConfigParamTOForQuota.ConfigParamVal.Split(',').ToList();
                    if (tempList != null && tempList.Count >= 2)
                    {
                        avgBookingQtyDevDays = Convert.ToInt32(tempList[0]);
                        avgBookingQtyDevPer = Convert.ToInt32(tempList[1]);
                    }
                }

                //if (avgBookingQtyDevDays == 0)
                //{
                //    resultMessage.Tag = 0;
                //    return resultMessage;
                //}


                DateTime currentDate = _iCommon.ServerDateTime;

                DateTime toDate = currentDate;
                DateTime fromDate = currentDate.AddDays(-avgBookingQtyDevDays);

                fromDate = Constants.GetStartDateTime(fromDate);
                toDate = Constants.GetEndDateTime(toDate);

                List<TblBookingsTO> list = SelectBookingList(0, dealerOrgId, 0, fromDate, toDate, null, -1, 0, 0, 0, 0);

                list = list.Where(w => w.IdBooking != bookingId).ToList();

                list = list.Where(w => w.StatusId == (int)Constants.TranStatusE.BOOKING_APPROVED
                || w.StatusId == (int)Constants.TranStatusE.BOOKING_APPROVED_BY_MARKETING
                || w.StatusId == (int)Constants.TranStatusE.BOOKING_APPROVED_FINANCE
                || w.StatusId == (int)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR).ToList();

                if (list == null || list.Count == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.Tag = 0;
                    return resultMessage;
                }

                var a = list.Count();

                Double totalQtyBetweenDays = list.Sum(g => g.BookingQty);

                Double avgQty = totalQtyBetweenDays / a;

                Double percentageValue = (avgQty * avgBookingQtyDevPer) / 100;

                avgQty += percentageValue;

                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Tag = avgQty;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GetBookingAvgQtyDetailsStatus");
                return resultMessage;
            }
        }


        public ResultMessage SendBookingDueNotification()
        {
            ResultMessage resultMessage = new ResultMessage();


            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                List<TblAlertDefinitionTO> tblAlertDefinitionTOList = _iTblAlertDefinitionDAO.SelectAllTblAlertDefinition();

                DateTime currentDate = _iCommon.ServerDateTime;

                DateTime fromDate = Constants.GetEndDateTime(currentDate.AddDays(-90));
                DateTime toDate = Constants.GetEndDateTime(currentDate);

                List<TblBookingsTO> tblBookingsTOList = SelectBookingList(0, 0, 0, fromDate, toDate, null, -1, 1, 0, 1, 0);

                tblBookingsTOList = tblBookingsTOList.Where(w => w.StatusId == (int)Constants.TranStatusE.BOOKING_APPROVED
                || w.StatusId == (int)Constants.TranStatusE.BOOKING_APPROVED_BY_MARKETING
                || w.StatusId == (int)Constants.TranStatusE.BOOKING_APPROVED_FINANCE
                || w.StatusId == (int)Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR).ToList();


                //List<TblBookingsTO> tblBookingsTOList = SelectAllPendingBookingsForReport();
                //List<TblBookingsTO> tblBookingsTOList = _iTblBookingsDAO.SelectAllTblBookings();

                Int32 dueDays = 0, notificationStartDays = 0;

                TblConfigParamsTO tblConfigParamTODueDays = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.BOOKING_DUE_DAYS_WITH_START_NOTIFICATON_DAYS);
                if (tblConfigParamTODueDays == null)
                {
                    throw new Exception("tblConfigParamTODueDays no found");
                }

                List<String> tempList = tblConfigParamTODueDays.ConfigParamVal.Split(',').ToList();
                if (tempList != null && tempList.Count >= 2)
                {
                    dueDays = Convert.ToInt32(tempList[0]);
                    notificationStartDays = Convert.ToInt32(tempList[1]);
                }

                if (dueDays == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                if (notificationStartDays == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                tblBookingsTOList = tblBookingsTOList.Where(w => w.PendingQty > 0).ToList();
                tblBookingsTOList = tblBookingsTOList.Where(w => w.BookingDatetime.Date.AddDays(dueDays) >= currentDate.Date).ToList();
                tblBookingsTOList = tblBookingsTOList.Where(w => w.BookingDatetime.Date.AddDays(notificationStartDays) <= currentDate.Date).ToList();

                for (int i = 0; i < tblBookingsTOList.Count; i++)
                {
                    TblBookingsTO tblBookingsTO = tblBookingsTOList[i];

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    List<TblUserTO> dealerUserList = _iTblUserDAO.SelectAllTblUser(tblBookingsTO.DealerOrgId, conn, tran);
                    if (dealerUserList != null && dealerUserList.Count > 0)
                    {
                        for (int a = 0; a < dealerUserList.Count; a++)
                        {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                            tblAlertUsersTO.UserId = dealerUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = dealerUserList[a].RegisteredDeviceId;
                            tblAlertUsersTOList.Add(tblAlertUsersTO);
                        }
                    }


                    String dueDateStr = tblBookingsTO.BookingDatetime.Date.AddDays(dueDays).ToString("yyyy-MM-dd");


                    TblAlertDefinitionTO tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_DUE);
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_DUE;
                    tblAlertInstanceTO.AlertAction = "BOOKING_DUE";


                    tblAlertInstanceTO.EffectiveFromDate = currentDate;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(24);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "Booking";
                    tblAlertInstanceTO.SourceEntityId = tblBookingsTO.IdBooking;
                    tblAlertInstanceTO.RaisedBy = 1;
                    tblAlertInstanceTO.RaisedOn = currentDate;
                    tblAlertInstanceTO.IsAutoReset = 1;


                    //tblAlertInstanceTO.AlertComment = "";
                    tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                    if (!String.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                    {
                        string tempSmsString = tblAlertDefinitionTO.DefaultAlertTxt;

                        tempSmsString = tempSmsString.Replace("@QtyStr", tblBookingsTO.BookingQty.ToString());
                        tempSmsString = tempSmsString.Replace("@RateStr", tblBookingsTO.BookingRate.ToString());
                        tempSmsString = tempSmsString.Replace("@BookingNoStr", tblBookingsTO.BookingDisplayNo);
                        tempSmsString = tempSmsString.Replace("@PendingQtyStr", tblBookingsTO.PendingQty.ToString());
                        tempSmsString = tempSmsString.Replace("@DueDateStr", dueDateStr);

                        tblAlertInstanceTO.AlertComment = tempSmsString;
                    }


                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    // SMS to Dealer
                    TblSmsTO smsTO = new TblSmsTO();
                    Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);
                    if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                    {
                        smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                        if (!String.IsNullOrEmpty(tblAlertDefinitionTO.DefaultSmsTxt))
                        {
                            string tempSmsString = tblAlertDefinitionTO.DefaultSmsTxt;

                            tempSmsString = tempSmsString.Replace("@QtyStr", tblBookingsTO.BookingQty.ToString());
                            tempSmsString = tempSmsString.Replace("@RateStr", tblBookingsTO.BookingRate.ToString());
                            tempSmsString = tempSmsString.Replace("@BookingNoStr", tblBookingsTO.BookingDisplayNo);
                            tempSmsString = tempSmsString.Replace("@PendingQtyStr", tblBookingsTO.PendingQty.ToString());
                            tempSmsString = tempSmsString.Replace("@DueDateStr", dueDateStr);

                            smsTO.SmsTxt = tempSmsString;
                        }
                        else
                        {
                            smsTO.SmsTxt = "Your Order Of Pend Qty " + tblBookingsTO.PendingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is due on date - " + dueDateStr + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";
                        }
                        tblAlertInstanceTO.SmsTOList.Add(smsTO);
                    }


                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);

                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        return rMessage;
                    }

                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendBookingDueNotification");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords , Int32 bookingId)
        {
            List<TblBookingsTO> pendingList = _iTblBookingsDAO.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, true, bookingId);
            if (pendingList != null && pendingList.Count < lastNRecords)
            {
                lastNRecords = lastNRecords - pendingList.Count;
                List<TblBookingsTO> list = _iTblBookingsDAO.SelectAllLatestBookingOfDealer(dealerId, lastNRecords, false, bookingId);
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
        public List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, Int32 statusId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 confirm, Int32 isPendingQty, Int32 bookingId, Int32 isViewAllPendingEnq, Int32 RMId,Int32 orderTypeId=0)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();

            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            List<TblBookingsTO> bookingList =  _iTblBookingsDAO.SelectBookingList(cnfId, dealerId, statusId, fromDate, toDate, tblUserRoleTO, confirm, isPendingQty, bookingId, isViewAllPendingEnq, RMId,orderTypeId);
            if(bookingList != null && bookingList.Count > 0)
            {
                //Prajakta[2021-07-06] Added 
                var isCnfChkSelectedList = bookingList.Where(a => a.CnfChkSelected == 1).ToList();
                if(isCnfChkSelectedList != null && isCnfChkSelectedList.Count > 0)
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_INTERNAL_CNF_SELECTION_ON_CHECKBOX_NEW);
                    if(configParamsTO != null && !String.IsNullOrEmpty(configParamsTO.ConfigParamVal) && configParamsTO.ConfigParamVal != "0")
                    {
                        String cnfName = configParamsTO.ConfigParamVal.ToString();

                        isCnfChkSelectedList.ForEach(cc => cc.CnfName = cnfName);
                    }
                }

            }

            return bookingList;

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

                List<ODLMWebAPI.DashboardModels.BookingInfo> tblBookingsTOList = _iTblBookingsDAO.SelectBookingDashboardInfo(tblUserRoleTO, orgId, dealerId, date, ids, isHideCorNC, false);

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

                if (tblUserRoleTO != null)
                {
                    Dictionary<int, string> sysEleAccessDCT = _iTblSysElementsBL.SelectSysElementUserEntitlementDCT(tblUserRoleTO.UserId, tblUserRoleTO.RoleId);

                    if (sysEleAccessDCT != null || sysEleAccessDCT.Count > 0)
                    {
                        if (sysEleAccessDCT.ContainsKey(Convert.ToInt32(Constants.pageElements.CONSUMER_TYPEWISE_ENQUIRY)) && sysEleAccessDCT[Convert.ToInt32(Constants.pageElements.CONSUMER_TYPEWISE_ENQUIRY)] != null
                            && !string.IsNullOrEmpty(sysEleAccessDCT[Convert.ToInt32(Constants.pageElements.CONSUMER_TYPEWISE_ENQUIRY)].ToString()) && sysEleAccessDCT[Convert.ToInt32(Constants.pageElements.CONSUMER_TYPEWISE_ENQUIRY)] == "RW")
                        {
                            List<ODLMWebAPI.DashboardModels.BookingInfo> tblBookingsTOList1 = _iTblBookingsDAO.SelectBookingDashboardInfo(tblUserRoleTO, orgId, dealerId, date, ids, isHideCorNC, true);

                            if (tblBookingsTOList != null && tblBookingsTOList1 != null)
                            {
                                for (int j = 0; j < tblBookingsTOList1.Count; j++)
                                {
                                    tblBookingsTOList1[j].ShortNm = tblBookingsTOList1[j].ConsumerType;
                                    tblBookingsTOList.Add(tblBookingsTOList1[j]);
                                }
                            }
                        }
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
                //List<TblBookingsTO> openingBalBookingList = _iTblBookingsDAO.SelectAllTodaysBookingsWithOpeningBalance(cnfId, serverDate, isTransporterScopeYn, isConfirmed, brandId);
                //List<TblBookingsTO> todaysList = _iTblBookingsDAO.SelectAllPendingBookingsList(cnfId, "=", false, isTransporterScopeYn, isConfirmed, serverDate, brandId, tblUserRoleTO);

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
                        pendingBookingRptTO.BookingDisplayNo = bookingTO.BookingDisplayNo;
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
                        pendingBookingRptTO.TotalAmountOfBookings = ((bookingTO.BookingQty) * (bookingTO.BookingRate));
                        pendingBookingRptTO.BookingQty = bookingTO.BookingQty;
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

        public TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking)
        {
            return _iTblBookingsDAO.SelectBookingsTOWithDetails(idBooking);

        }

        public List<TblBookingAnalysisReportTO> GetGroupByDealerBookingList(List<TblBookingAnalysisReportTO> tblBookingTOList)
        {
            var dist = tblBookingTOList.GroupBy(g => new { g.DealerId, g.DistributorId }).Select(s => s.FirstOrDefault()).ToList();

            for (int i = 0; i < dist.Count; i++)
            {
                TblBookingAnalysisReportTO tblBookingsTO = dist[i];
                List<TblBookingAnalysisReportTO> temp = tblBookingTOList.Where(w => w.DealerId == tblBookingsTO.DealerId && w.DistributorId == tblBookingsTO.DistributorId).ToList();
                //tblBookingsTO.BookingQty = temp.Sum(s => s.BookingQty);

                temp = temp.OrderBy(a => a.CreatedOn).ToList();
                double totalDays = (temp[temp.Count - 1].CreatedOn - temp[0].CreatedOn).TotalDays;
                double sumOfBookingQty = (from x in temp select x.BookingQty).Sum();
                tblBookingsTO.BookingRate = ((from x in temp select x.BookingRate * x.BookingQty).Sum()) / sumOfBookingQty;
                tblBookingsTO.BookingQty = sumOfBookingQty;
                tblBookingsTO.DispatchedQty = (from x in temp select x.DispatchedQty).Sum();
                tblBookingsTO.BookingRate = Math.Round(tblBookingsTO.BookingRate, 2);
                tblBookingsTO.BookingQty = Math.Round(tblBookingsTO.BookingQty, 3);
                tblBookingsTO.AvgBookingFrequency = Math.Round(Math.Round(totalDays) / temp.Count, 3);

            }

            return dist;
        }

        //Deepali added for task 1272 [03-08-2021]
        public List<TblBookingAnalysisReportTO> GetBookingAnalysisReport(DateTime startDate, DateTime endDate, int distributorId, int cOrNcId, int brandId, int skipDate, int isFromProject)
        {
            List<TblBookingAnalysisReportTO> listReturn = new List<TblBookingAnalysisReportTO>();
            List<TblBookingAnalysisReportTO> list = new List<TblBookingAnalysisReportTO>();
            List<TblBookingAnalysisReportTO> listGroupbyCnf = new List<TblBookingAnalysisReportTO>();
            list = _iTblBookingsDAO.GetBookingAnalysisReport(startDate, endDate, distributorId, cOrNcId, brandId, skipDate,isFromProject);
            if (list != null && list.Count > 0)
            {


                list = GetGroupByDealerBookingList(list);

                listGroupbyCnf = list.GroupBy(g => new { g.DistributorId }).Select(s => s.FirstOrDefault()).ToList();
                double totalQty = 0;
                double totalAvgRate = 0;
                totalQty = (from x in list select x.BookingQty).Sum();
                totalAvgRate = ((from x in list select x.BookingRate * x.BookingQty).Sum()) / totalQty;
                totalQty = Math.Round(totalQty, 3);
                totalAvgRate = Math.Round(totalAvgRate, 2);
                for (int i = 0; i < listGroupbyCnf.Count; i++)
                {
                    List<TblBookingAnalysisReportTO> listTemp = new List<TblBookingAnalysisReportTO>();
                    listTemp = list.Where(s => s.DistributorId == listGroupbyCnf[i].DistributorId).ToList();

                    for (int j = 0; j < listTemp.Count; j++)
                    {
                        if (j > 0)
                        {
                            listTemp[j].DistributorName = "";

                        }
                        else
                        {
                            listTemp[j].SrNo = i + 1;
                        }
                    }

                    List<TblBookingAnalysisReportTO> listGroupbyDealer = new List<TblBookingAnalysisReportTO>();
                    listGroupbyDealer = listTemp.GroupBy(g => new { g.DealerId }).Select(s => s.FirstOrDefault()).ToList();
                    for (int dealer = 0; dealer < listGroupbyDealer.Count; dealer++)
                    {
                        
                        double totalDays = 0;

                        List<TblBookingAnalysisReportTO> listTempDealer = new List<TblBookingAnalysisReportTO>();
                        listTempDealer = listTemp.Where(s => s.DealerId == listGroupbyDealer[dealer].DealerId).ToList();
                        
                        //for (int d = 0; d < listTempDealer.Count; d++)
                        //{
                        //    if (d < listTempDealer.Count - 1)
                        //    {
                        //        totalDays += (listTempDealer[d+1].CreatedOn - listTempDealer[d].CreatedOn).TotalDays;
                        //    }
                        //    listReturn.Add(listTempDealer[d]);
                        //}
                        //listTempDealer = listTempDealer.OrderBy(a => a.CreatedOn).ToList();
                        //totalDays = (listTempDealer[listTempDealer.Count - 1].CreatedOn - listTempDealer[0].CreatedOn).TotalDays;

                        

                        listReturn.AddRange(listTempDealer);

                        //listReturn[listReturn.Count - 1].AvgBookingFrequency = Math.Round(totalDays) / listTempDealer.Count;
                        //listReturn[listReturn.Count - 1].AvgBookingFrequency = Math.Round(listReturn[listReturn.Count - 1].AvgBookingFrequency, 3);

                    }


                    TblBookingAnalysisReportTO tblBookingAnalysisReportTO = new TblBookingAnalysisReportTO();
                    tblBookingAnalysisReportTO.DistributorName = "Total";
                    double sumOfBookingQty = (from x in listTemp select x.BookingQty).Sum();
                    tblBookingAnalysisReportTO.BookingQty = sumOfBookingQty;
                    tblBookingAnalysisReportTO.BookingRate = ((from x in listTemp select x.BookingRate * x.BookingQty).Sum()) / sumOfBookingQty;
                    tblBookingAnalysisReportTO.BookingRate = Math.Round(tblBookingAnalysisReportTO.BookingRate, 2);
                    tblBookingAnalysisReportTO.BookingQty = Math.Round(tblBookingAnalysisReportTO.BookingQty, 3);

                    tblBookingAnalysisReportTO.SrNo = -1;
                    listReturn.Add(tblBookingAnalysisReportTO);

                   
                }
                if(listReturn != null && listReturn.Count > 0)
                {
                    listReturn[0].TotalAvgQty = totalQty;
                    listReturn[0].TotalAvgRate = totalAvgRate;
                }
            }
            return listReturn;
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
            Boolean isAddItemWiseRate = false;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();


                #region entity range 

                //Saket [2020-03-26]
                lock (bookingNoLock)
                {

                    DimFinYearTO curFinYearTO = _iDimensionBL.GetCurrentFinancialYear(tblBookingsTO.CreatedOn);
                    if (curFinYearTO == null)
                    {
                        resultMessage.Text = "Current Fin Year Object Not Found";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                    TblEntityRangeTO entityRangeTO = _iTblEntityRangeBL.SelectTblEntityRangeTOByEntityName(Constants.REGULAR_BOOKING, curFinYearTO.IdFinYear, conn, tran);
                    if (entityRangeTO == null)
                    {
                        tran.Rollback();
                        resultMessage.Text = "entity range not found in Function SaveNewBooking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                    entityRangeTO.EntityPrevValue = entityRangeTO.EntityPrevValue + 1;
                    var result1 = _iTblEntityRangeBL.UpdateTblEntityRange(entityRangeTO, conn, tran);
                    if (result1 != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : While UpdateTblEntityRange";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                    tblBookingsTO.BookingDisplayNo = entityRangeTO.EntityPrevValue.ToString();

                    #endregion

                    //Prajakta[2021-04-26] Added to add itemwise rate while booking
                    TblConfigParamsTO addItemWiseRateConfigTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.ADD_ITEMWISE_RATE_WHILE_BOOKING);
                    if(addItemWiseRateConfigTO != null)
                    {
                        if(addItemWiseRateConfigTO.ConfigParamVal == "1")
                        {
                            isAddItemWiseRate = true;
                        }
                        else
                        {
                            isAddItemWiseRate = false;
                        }
                    }


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

                        if (tblBookingsTO.EnquiryId == 0)
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
                        if (tblBookingsTO.EnquiryId == 0)
                            tblBookingsTO.StatusId = statusId;
                        //tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;

                    }

                    if (tblBookingsTO.CdStructure > maxCdStructure)
                    {
                        if (tblBookingsTO.CdStructure > dealerOrgTO.CdStructure)
                        {
                            tblBookingsTO.IsWithinQuotaLimit = 0;
                            //tblBookingsTO.TranStatusE = Constants.TranStatusE.BOOKING_NEW;
                            if (tblBookingsTO.EnquiryId == 0)
                                tblBookingsTO.StatusId = statusId;
                            tblBookingsTO.AuthReasons += "CD|";
                        }
                    }
                    //Aniket [3-7-2019] placed from line no 1265 to 1289 as while applying CD create problem
                    if (tblBookingsTO.IsItemized == 0 && !string.IsNullOrEmpty(tblBookingsTO.AuthReasons))
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
                            if (tblBookingsTO.EnquiryId == 0)
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

                    if (tblBookingsTO.EnquiryId == 0)
                        tblBookingsTO.StatusId = statusId;
                    //Aniket [24-7-2019] added to check scheduled NoOfDeliveries against booking
                    if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                    {
                        //Prajakta[2020-02-19] Commented and added as per disscussion with saket
                        //var res = tblBookingsTO.BookingScheduleTOLst.GroupBy(x => x.ScheduleDate);
                        var res = tblBookingsTO.BookingScheduleTOLst.GroupBy(x => x.ScheduleGroupId);
                        if (res != null)
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
                                    //Prajakta[2021-04-23] Commented as rate will get as per parity from GUI
                                    if(!isAddItemWiseRate)
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
                    #endregion
                    #region Added By Kiran For Bundle Wise Item Details using booking Id
                    if (tblBookingsTO.OrderDetailsLstForItemWise != null && tblBookingsTO.OrderDetailsLstForItemWise.Count > 0)
                    {
                        List<TblBookingExtTO> tblBookingExtTOItemWiseList = tblBookingsTO.OrderDetailsLstForItemWise.Where(w => w.BookedQty > 0).ToList();
                        for (int j = 0; j < tblBookingExtTOItemWiseList.Count; j++)
                        {
                            TblBookingExtTO tblBookingExtTO = tblBookingExtTOItemWiseList[j];
                            tblBookingExtTO.BookingId = tblBookingsTO.IdBooking;
                            tblBookingExtTO.BalanceQty = tblBookingExtTO.BookedQty;
                            //if(isBalajiClient==0)
                            //Prajakta[2021-04-23] Commented as rate will get as per parity from GUI
                            if (!isAddItemWiseRate)
                                tblBookingExtTO.Rate = tblBookingsTO.BookingRate; //For the time being Rate is declare global for the order. i.e. single Rate for All Material

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
                    //Aniket [31-7-2019] added to set sms text dynamically
                    List<TblAlertDefinitionTO> tblAlertDefinitionTOList = _iTblAlertDefinitionDAO.SelectAllTblAlertDefinition();

                    //AmolG[2020-Feb-11]
                    //Check and generate alert if the size is changes when update booking. This is SRJ requirement

                    String errorMsg = String.Empty;
                    AnalyzeDifferentSizes(null, tblBookingsTO, true, tblAlertDefinitionTOList, ref errorMsg, 0, conn, tran);

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

                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_CONFIRMED);
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_CONFIRMED;
                        tblAlertInstanceTO.AlertAction = "BOOKING_CONFIRMED";
                        tblAlertInstanceTO.AlertComment = "";
                        //Reshma Added For Parmeshwar SMS Changes.
                        String AlertComment = "";
                        TblConfigParamsTO tblConfigParamsTOTemp = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_NOTIFICATIONS);
                        if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                        { 
                            Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                            if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                            {
                                TblConfigParamsTO MessageTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_ORDER_CONFIRMATION_SMS_STRING);
                                if (MessageTO != null && !String.IsNullOrEmpty(MessageTO.ConfigParamVal))
                                {
                                    if (tblBookingsTO != null && tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst != null && tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst.Count > 0)
                                    {
                                        AlertComment = MessageTO.ConfigParamVal;
                                        AlertComment = AlertComment.Replace("@DEALER_NAME", tblBookingsTO.DealerName);
                                        String SMS_CONTENT = tblBookingsTO.CreatedOn.ToString("dd-MMMM-yy") + " Rate " + tblBookingsTO.BookingRate + " Size";
                                        for (int i = 0; i< tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst.Count ; i++)
                                        {
                                            if (i == 0)
                                            {
                                                SMS_CONTENT = SMS_CONTENT + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[0].MaterialSubType + " Qty " + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[0].BookedQty + " MT";
                                                 
                                            }
                                            else
                                            {
                                                SMS_CONTENT = SMS_CONTENT + " , " + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[i].MaterialSubType + " Qty " + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[i].BookedQty + " MT";
                                            }
                                        }
                                        AlertComment = AlertComment.Replace("@SMS_CONTENT", SMS_CONTENT);
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(AlertComment))
                        {
                            TblOrganizationTO OrganizationTO = _iTblOrganizationDAO.SelectTblOrganizationTO((int)Constants.DefaultCompanyId);
                            AlertComment = AlertComment.Replace("@Org_Name", OrganizationTO .FirmName);
                            tblAlertInstanceTO.AlertComment = AlertComment;

                        }
                        if (String.IsNullOrEmpty(tblAlertInstanceTO.AlertComment))
                        {
                            tblAlertInstanceTO.AlertComment = "Your Booking #" + tblBookingsTO.BookingDisplayNo + " is confirmed. Rate : " + tblBookingsTO.BookingRate + " AND Qty : " + tblBookingsTO.BookingQty;
                        }
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                        // SMS to Dealer
                        TblSmsTO smsTO = new TblSmsTO();
                        Dictionary<Int32, String> orgMobileNoDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblBookingsTO.DealerOrgId.ToString(), conn, tran);

                        if (!String.IsNullOrEmpty(tblAlertDefinitionTO.DefaultSmsTxt))
                        {
                            string tempSmsString = tblAlertDefinitionTO.DefaultSmsTxt;
                            tempSmsString = tempSmsString.Replace("@QtyStr", tblBookingsTO.BookingQty.ToString());
                            tempSmsString = tempSmsString.Replace("@RateStr", tblBookingsTO.BookingRate.ToString());
                            smsTO.SmsTxt = tempSmsString;
                        }
                        else
                        {
                            if (orgMobileNoDCT != null && orgMobileNoDCT.Count == 1)
                            {
                                tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                                smsTO.MobileNo = orgMobileNoDCT[tblBookingsTO.DealerOrgId];
                                smsTO.SourceTxnDesc = "New Booking";
                                string confirmMsg = string.Empty;
                                if (tblBookingsTO.IsConfirmed == 1)
                                    confirmMsg = "Confirmed";
                                else
                                    confirmMsg = "Not Confirmed";

                                smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";
                                //Reshma Added
                                if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                                {
                                    Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                                    if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                                    {
                                        smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                                    }
                                }
                            }

                        }
                        tblAlertInstanceTO.SmsTOList.Add(smsTO);

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
                                var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED);
                                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                                tblAlertInstanceTO.AlertAction = "booking_approval_required";
                                string tempTxt = "";
                                if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                                {
                                    tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                                    tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                                    tempTxt = tempTxt.Replace("@DealerName", tblBookingsTO.DealerName);
                                    tblAlertInstanceTO.AlertComment = tempTxt;
                                }
                                else
                                    tblAlertInstanceTO.AlertComment = "approval required for booking #" + tblBookingsTO.BookingDisplayNo;
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
                        tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.BookingDisplayNo + " Director has remark -" + tblBookingsTO.DirectorRemark;
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


                }

                resultMessage.MessageType = ResultMessageE.Information;
                if (tblBookingsTO.IsWithinQuotaLimit == 1)
                {
                    resultMessage.Text = "Success, Enquiry # - " + tblBookingsTO.BookingDisplayNo + " is generated Successfully.";
                    resultMessage.DisplayMessage = "Enquiry # - " + tblBookingsTO.BookingDisplayNo + " is generated Successfully.";
                    resultMessage.Tag = tblBookingsTO.IdBooking;
                }
                else
                {

                    if (tblBookingsTO.TranStatusE == Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                    {
                        resultMessage.Text = "Success, Enquiry # - " + tblBookingsTO.BookingDisplayNo + " is accepted";
                        resultMessage.DisplayMessage = "Enquiry # - " + tblBookingsTO.BookingDisplayNo + " is accepted";
                    }
                    else
                    {
                        resultMessage.Text = "Success, Enquiry # - " + tblBookingsTO.BookingDisplayNo + " is generated Successfully But Sent For Approval";
                        resultMessage.DisplayMessage = "Enquiry # - " + tblBookingsTO.BookingDisplayNo + " is generated Successfully But Sent For Approval";
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

        /// <summary>
        /// AmolG[2020-Feb-11]
        /// This function is used to find the changes in sizes when we add new booking to Update existing booking
        /// </summary>
        /// <param name="tblBookingsTOPrev">Old booking TO</param>
        /// <param name="tblBookingsTOCurr">Save/Update booking TO</param>
        /// <param name="isGenerateAlert">True is used to raise the size change alert</param>
        /// <param name="tblAlertDefinitionTOList">ALter defination list</param>
        /// <param name="errorMsg">Return Error message if any exception occures in function</param>
        /// <param name="isFromEditBooking">if 1 then check diff Otherwise send current sizes</param>
        /// <returns></returns>
        private Boolean AnalyzeDifferentSizes(TblBookingsTO tblBookingsTOPrev, TblBookingsTO tblBookingsTOCurr, Boolean isGenerateAlert
            , List<TblAlertDefinitionTO> tblAlertDefinitionTOList, ref string errorMsg, Int32 isFromEditBooking ,SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                //Saket [2020-02-21] As per discussion with Amol Kularni SRJ - No need to send notifcation on approval instead send on add/edit.
                //Saket [2020-02-18] Added conditions
                //if (tblBookingsTOCurr.TranStatusE != Constants.TranStatusE.BOOKING_APPROVED_FINANCE && tblBookingsTOCurr.TranStatusE != Constants.TranStatusE.BOOKING_APPROVED && tblBookingsTOCurr.TranStatusE != Constants.TranStatusE.BOOKING_ACCEPTED_BY_ADMIN_OR_DIRECTOR)
                //{
                //    return false;
                //}

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_SIZE_CHANGE_ALERT_GENERATE);
                if (tblConfigParamsTO == null || Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                {
                    return false;
                }
                if (tblBookingsTOCurr.IdBooking > 0)
                {
                    List<TblBookingExtTO> tblBookingExtTOList = _iTblBookingExtDAO.SelectAllTblBookingExt(tblBookingsTOCurr.IdBooking, conn, tran);
                    if (tblBookingExtTOList != null)
                    {
                        var lstBook = from lst in tblBookingExtTOList
                                      where lst.BalanceQty > 0
                                      select lst;
                        if (lstBook != null && lstBook.Any())
                        {
                            tblBookingExtTOList = lstBook.ToList();
                        }
                        else
                        {
                            tblBookingExtTOList = new List<TblBookingExtTO>();
                        }
                        tblBookingsTOPrev = new TblBookingsTO();
                        tblBookingsTOPrev.IdBooking = tblBookingsTOCurr.IdBooking;
                        tblBookingsTOPrev.BookingScheduleTOLst = new List<TblBookingScheduleTO>();
                        TblBookingScheduleTO tblBookingScheduleTO = new TblBookingScheduleTO();
                        tblBookingsTOPrev.BookingScheduleTOLst.Add(tblBookingScheduleTO);
                        tblBookingScheduleTO.OrderDetailsLst = tblBookingExtTOList;
                    }
                }

                String alertMsg = String.Empty;
                Boolean isSizeChanged = false;

                if (isFromEditBooking == 0)
                {
                    Dictionary<String, Double> materialDCT = GetMaterialList(tblBookingsTOPrev, ref alertMsg);
                    if (materialDCT != null && materialDCT.Count > 0)
                        isSizeChanged = true;
                }
                else
                {
                    if (tblBookingsTOPrev == null || tblBookingsTOPrev.IdBooking == 0)
                    {
                        if (tblBookingsTOCurr != null && tblBookingsTOCurr.BookingScheduleTOLst != null && tblBookingsTOCurr.BookingScheduleTOLst.Count > 0)
                        {
                            Dictionary<String, Double> materialDCT = GetMaterialList(tblBookingsTOCurr, ref alertMsg);
                            if (materialDCT != null && materialDCT.Count > 0)
                                isSizeChanged = true;
                        }
                    }
                    else
                    {
                        String alert = String.Empty;
                        Dictionary<String, Double> materialDCT = GetMaterialList(tblBookingsTOCurr, ref alertMsg);
                        
                        Dictionary<String, Double> materialPrevDCT = GetMaterialList(tblBookingsTOPrev, ref alert);

                        if ((materialDCT == null || materialDCT.Count == 0) && (materialPrevDCT == null || materialPrevDCT.Count == 0))
                        {
                            //Nothing change 
                            return false;
                        }

                        if (materialDCT != null && materialDCT.Count > 0)
                        {
                            alertMsg = String.Empty;

                            foreach (String itemId in materialDCT.Keys)
                            {
                                if (materialPrevDCT.ContainsKey(itemId))
                                {
                                    if (materialPrevDCT[itemId] != materialDCT[itemId])
                                    {
                                        alertMsg += itemId + ", ";
                                        isSizeChanged = true;
                                    }
                                }
                                else
                                {
                                    alertMsg += itemId + ", ";
                                    isSizeChanged = true;
                                }
                            }
                            alertMsg = alertMsg.TrimEnd(',');
                        }
                        else
                        {
                            alertMsg = "(deleted)";
                            isSizeChanged = true;
                            //if (isFromEditBooking == 1)
                            //{
                            //    alertMsg = "(deleted)";
                            //    isSizeChanged = true;
                            //}
                            //else
                            //{
                            //    return false;
                            //}
                        }
                    }
                }
                if (isGenerateAlert && isSizeChanged)
                {
                    if (tblAlertDefinitionTOList == null || tblAlertDefinitionTOList.Count == 0)
                    {
                        TblAlertDefinitionTO tblAlertDefinitionLocalTO = _iTblAlertDefinitionDAO.SelectTblAlertDefinition((int)NotificationConstants.NotificationsE.SIZE_CHANGES_IN_BOOKING, conn, tran);
                        if (tblAlertDefinitionTOList != null)
                            tblAlertDefinitionTOList.Add(tblAlertDefinitionLocalTO);
                        else
                        {
                            tblAlertDefinitionTOList = new List<TblAlertDefinitionTO>();
                            tblAlertDefinitionTOList.Add(tblAlertDefinitionLocalTO);
                        }
                    }
                    //Raise alert here
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.SIZE_CHANGES_IN_BOOKING);
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.SIZE_CHANGES_IN_BOOKING;
                    tblAlertInstanceTO.AlertAction = "Size_Changes_In_Booking";

                    tblAlertInstanceTO.AlertComment = tblAlertDefinitionTO.DefaultAlertTxt.Replace("@SizeStr", alertMsg).Replace("@BookingIdStr", tblBookingsTOCurr.BookingDisplayNo).Replace("@DealerNameStr", tblBookingsTOCurr.DealerName);

                    tblAlertInstanceTO.EffectiveFromDate = _iCommon.ServerDateTime;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "Size Change";
                    tblAlertInstanceTO.SourceEntityId = tblBookingsTOCurr.IdBooking;
                    tblAlertInstanceTO.RaisedBy = tblBookingsTOCurr.CreatedBy;
                    tblAlertInstanceTO.RaisedOn = _iCommon.ServerDateTime;
                    tblAlertInstanceTO.IsAutoReset = 1;


                    //Saket [2020-02-21] As per discussion with Amol Kularni - SRJ no need to reset notifcation.
                    //ResetAlertInstanceTO reset = new ResetAlertInstanceTO();

                    //reset.SourceEntityTxnId = tblBookingsTOCurr.IdBooking;
                    //reset.AlertDefinitionId = tblAlertInstanceTO.AlertDefinitionId;
                    //tblAlertInstanceTO.AlertsToReset = new AlertsToReset();
                    //tblAlertInstanceTO.AlertsToReset.ResetAlertInstanceTOList = new List<ResetAlertInstanceTO>();
                    //tblAlertInstanceTO.AlertsToReset.ResetAlertInstanceTOList.Add(reset);

                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);

                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        errorMsg = "Error While Generating Notification";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMsg = "Ex : " + ex.GetBaseException().ToString() + ". StackTrace : " + ex.StackTrace;
                return false;
            }
        }

        /// <summary>
        /// AmolG[2020-Feb-11]
        /// Return all unique sizes in the given booking 
        /// </summary>
        /// <param name="tblBookingsTO"></param>
        /// <param name="alertMsg"></param>
        /// <returns></returns>
        private Dictionary<String, Double> GetMaterialList(TblBookingsTO tblBookingsTO, ref string alertMsg)
        {
            //List<Int32> materialIdList = new List<int>();
            Dictionary<String, Double> materialDCT = new Dictionary<String, double>();
            try
            {
                if (tblBookingsTO != null && tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    for (int iSch = 0; iSch < tblBookingsTO.BookingScheduleTOLst.Count; iSch++)
                    {
                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[iSch];
                        if (tblBookingScheduleTO.OrderDetailsLst != null && tblBookingScheduleTO.OrderDetailsLst.Count > 0)
                        {
                            for (int iOrder = 0; iOrder < tblBookingScheduleTO.OrderDetailsLst.Count; iOrder++)
                            {
                                if (!materialDCT.ContainsKey(tblBookingScheduleTO.OrderDetailsLst[iOrder].MaterialSubType)
                                    && tblBookingScheduleTO.OrderDetailsLst[iOrder].BalanceQty > 0)
                                {
                                    materialDCT.Add(tblBookingScheduleTO.OrderDetailsLst[iOrder].MaterialSubType, tblBookingScheduleTO.OrderDetailsLst[iOrder].BookedQty);
                                    alertMsg += tblBookingScheduleTO.OrderDetailsLst[iOrder].MaterialSubType + " ,";
                                }
                                else
                                {
                                    materialDCT[tblBookingScheduleTO.OrderDetailsLst[iOrder].MaterialSubType] += tblBookingScheduleTO.OrderDetailsLst[iOrder].BookedQty;
                                }
                            }
                        }
                    }

                    alertMsg = alertMsg.TrimEnd(',');

                }

            }
            catch (Exception ex)
            {
                
            }
            return materialDCT;
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
                //Aniket [23-9-2019] added to update booking rate in bookings parities table 
                int updateBookingparityResult;
                List<TblBookingParitiesTO> tblBookingParitiesTOList = _iTblBookingParitiesDAO.SelectTblBookingParitiesByBookingId(tblBookingsTO.IdBooking, conn, tran);
                TblBookingParitiesTO tblBookingParitiesTO = new TblBookingParitiesTO();
                if(tblBookingParitiesTOList!=null && tblBookingParitiesTOList.Count>0)
                {
                    tblBookingParitiesTOList.ForEach(x =>
                    {
                        if (x.BookingId==tblBookingsTO.IdBooking && x.BrandId==tblBookingsTO.BrandId)
                        {
                            tblBookingParitiesTO = x;
                        }
                    });
                }
                if(tblBookingParitiesTO!=null)
                {
                    tblBookingParitiesTO.BookingRate = tblBookingsTO.BookingRate;
                    updateBookingparityResult =  _iTblBookingParitiesDAO.UpdateTblBookingParities(tblBookingParitiesTO, conn, tran);
                    if(updateBookingparityResult!=1)
                    {
                        tran.Rollback();
                        resultMessage.DisplayMessage = "Error while update in Booking parities ";
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


                //Saket [2020-02-21] As per discussion with Amol Kularni SRJ - No need to send notifcation on approval instead send on add/edit.
                if (false)
                {
                    //Saket [2020-02-18] Added
                    String errorMsg = String.Empty;
                    AnalyzeDifferentSizes(null, tblBookingsTO, true, null, ref errorMsg, 0, conn, tran);
                    if (!String.IsNullOrEmpty(errorMsg))
                    {
                        tran.Rollback();
                        resultMessage.DisplayMessage = errorMsg;
                        return resultMessage;
                    }
                }
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
                //Aniket [5-8-2019] added to set Alert text dynamically
               List<TblAlertDefinitionTO> tblAlertDefinitionTOList = _iTblAlertDefinitionDAO.SelectAllTblAlertDefinition();
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
                        //Aniket [5-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.New_Booking);

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.New_Booking;
                        tblAlertInstanceTO.AlertAction = "New_Booking";
                        if(tblAlertDefinitionTO!=null)
                        {
                            if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                            {
                                string tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                                tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                                tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);
                                tblAlertInstanceTO.AlertComment = tempTxt;
                            }
                        }
                       
                        else
                        tblAlertInstanceTO.AlertComment = "New Booking #" + tblBookingsTO.BookingDisplayNo + " Is Generated(" + tblBookingsTO.DealerName + ").";

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
                       var tblAlertDefinitionTO= tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS);
                        string tempTxt="";
                        string cncStr = "";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                            tempTxt = tempTxt.Replace("@DealerNameStr", "-");
                            tempTxt = tempTxt.Replace("@CNCStr", cncStr);
                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                        {
                            tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.BookingDisplayNo + " is accepted";
                        }

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);
                            // tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                            tblAlertInstanceTO.AlertComment = tempTxt;
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
                            //Aniket [5-8-2019]
                            if(!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultSmsTxt))
                            {
                                string tempSms = tblAlertDefinitionTO.DefaultSmsTxt;
                                tempSms = tempSms.Replace("@QtyStr", tblBookingsTO.BookingQty.ToString());
                                tempSms = tempSms.Replace("@RateStr", tblBookingsTO.BookingRate.ToString());
                                tempSms = tempSms.Replace("@CNCStr", confirmMsg);
                                tempSms = tempSms.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());

                            }
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate(Rs/MT) " + tblBookingsTO.BookingRate.ToString("N2") + " is " + confirmMsg;
                            //smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty + " MT with Rate " + tblBookingsTO.BookingRate.ToString("N2") + " (Rs/MT) is " + confirmMsg + " Your Ref No :" + tblBookingsTO.IdBooking;
                            else
                            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";

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

                        tblAlertInstanceTO.AlertComment = "You Booking #" + tblBookingsTO.BookingDisplayNo + " is Hold By Admin/Director";
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
                            smsTO.SmsTxt = "Your Order Of Qty " + tblBookingsTO.BookingQty.ToString().Trim() + " MT with Rate " + tblBookingsTO.BookingRate + " (Rs/MT) is " + confirmMsg.Trim() + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + "";
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
                        //Aniket [5-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVAL_REQUIRED;
                        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVAL_REQUIRED";
                        if(!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                        tblAlertInstanceTO.AlertComment = "Booking #" + tblBookingsTO.BookingDisplayNo + " is awaiting for your confirmation";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);
                            // tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                            tblAlertInstanceTO.AlertComment = tempTxt;
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
                        //Aniket [5-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_REJECTED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_REJECTED_BY_DIRECTORS";
                        if(!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        //tblAlertInstanceTO.AlertComment = "Your Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is rejected";
                        else
                        tblAlertInstanceTO.AlertComment = "Your Booking #" + tblBookingsTO.BookingDisplayNo + " is rejected";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);
                            // tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                            tblAlertInstanceTO.AlertComment = tempTxt;
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
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS;
                        tblAlertInstanceTO.AlertAction = "BOOKING_APPROVED_BY_DIRECTORS";
                        //tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is accepted by Director";
                        //Aniket [5-8-2019] added
                        string cncStr = string.Empty;
                        if (tblBookingsTO.IsConfirmed == 1)
                            cncStr = " Confirmed ";
                        else
                            cncStr = " Not Confirmd ";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                             tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                            tempTxt = tempTxt.Replace("@CNCStr", cncStr);
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                        tblAlertInstanceTO.AlertComment = "Booking #" + tblBookingsTO.BookingDisplayNo + " is accepted by Director";

                        //Reshma Added For SMS Template Changes.
                        String AlertComment = "";
                        TblConfigParamsTO tblConfigParamsTOTemp = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_NOTIFICATIONS);
                        if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                        {
                            Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                            if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                            { 
                                TblConfigParamsTO MessageTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_ORDER_CONFIRMATION_SMS_STRING);
                                if (MessageTO != null && !String.IsNullOrEmpty(MessageTO.ConfigParamVal))
                                {
                                    if (tblBookingsTO != null && tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                                    {
                                        if (tblBookingsTO != null && tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst != null && tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst.Count > 0)
                                        {
                                            AlertComment = MessageTO.ConfigParamVal;
                                            AlertComment = AlertComment.Replace("@Dealer_Name ", tblBookingsTO.DealerName);
                                            String SMS_CONTENT = tblBookingsTO.CreatedOn.ToString("dd-MMMM-yy") + " Rate " + tblBookingsTO.BookingRate + " Size";
                                            for (int i = 0; i < tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst.Count; i++)
                                            {
                                                if (i == 0)
                                                {
                                                    SMS_CONTENT = SMS_CONTENT + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[0].MaterialSubType + " Qty " + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[0].BookedQty + " MT";
                                                }
                                                else
                                                {
                                                    SMS_CONTENT = SMS_CONTENT + " , " + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[i].MaterialSubType + " Qty " + tblBookingsTO.BookingScheduleTOLst[0].OrderDetailsLst[i].BookedQty + " MT";
                                                }
                                            }
                                            AlertComment = AlertComment.Replace("@SMS_CONTENT", SMS_CONTENT);
                                        }
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(AlertComment))
                        {
                            TblOrganizationTO OrganizationTO = _iTblOrganizationDAO.SelectTblOrganizationTO((int)Constants.DefaultCompanyId);
                            AlertComment = AlertComment.Replace("@Org_Name", OrganizationTO.FirmName);
                            tblAlertInstanceTO.AlertComment = AlertComment; 
                        }

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);
                            // tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                            tblAlertInstanceTO.AlertComment = tempTxt;
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
                                    smsTO.SmsTxt = SMSContent + keyValue + " Your Ref No : " + tblBookingsTO.BookingDisplayNo;
                                else
                                    smsTO.SmsTxt = SMSContent + keyValue + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + " (Other)";
                            }
                            else
                            {
                                if (tblBookingsTO.BookingType == Convert.ToInt32(Constants.BookingType.IsRegular))
                                    smsTO.SmsTxt = SMSContent + " Your Ref No : " + tblBookingsTO.BookingDisplayNo;
                                else
                                    smsTO.SmsTxt = SMSContent + " Your Ref No : " + tblBookingsTO.BookingDisplayNo + " (Other)";
                            }
                            //Reshma Added
                            if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                            {
                                Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                                if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                                {
                                    smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                                }
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
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.BOOKING_APPROVED_BY_DIRECTORS);
                        string tempTxt = "";
                        if(!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@BookingIdStr", tblBookingsTO.BookingDisplayNo.ToString());
                            tempTxt = tempTxt.Replace("@DealerNameStr","");

                        }
                        //tblAlertInstanceTO.AlertComment = "Not Confirmed Booking #" + tblBookingsTO.IdBooking + " is rejected by Admin/Director";
                        else
                        tblAlertInstanceTO.AlertComment = "Booking #" + tblBookingsTO.BookingDisplayNo + " is rejected by Admin/Director";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblBookingsTO.DealerName);
                            // tblAlertInstanceTO.AlertComment += " (" + tblBookingsTO.DealerName + ").";
                            tblAlertInstanceTO.AlertComment = tempTxt;
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

                #region 1.1 For Size Change Alert
                //AmolG[2020-Feb-11]
                //Check and generate alert if the size is changes when update booking. This is SRJ requirement
                //Below method is used without transaction
                string errorMsg = string.Empty;
                AnalyzeDifferentSizes(null, tblBookingsTO, true, null, ref errorMsg, 1, conn, tran);

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
                existingTblBookingsTO.OrderTypeId = tblBookingsTO.OrderTypeId;
                existingTblBookingsTO.OrderTypeName = tblBookingsTO.OrderTypeName;
                existingTblBookingsTO.CnfChkSelected = tblBookingsTO.CnfChkSelected;

                //Aniket [24-7-2019] added to check scheduled NoOfDeliveries against booking
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    var res = tblBookingsTO.BookingScheduleTOLst.GroupBy(x => x.ScheduleDate);
                    existingTblBookingsTO.NoOfDeliveries = res.Count();
                }
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


                #region 2.3 Update Booking Parities

                //Aniket [23-9-2019] added to update booking rate in bookings parities table 
                int updateBookingparityResult;
                List<TblBookingParitiesTO> tblBookingParitiesTOList = _iTblBookingParitiesDAO.SelectTblBookingParitiesByBookingId(tblBookingsTO.IdBooking, conn, tran);
                TblBookingParitiesTO tblBookingParitiesTO = new TblBookingParitiesTO();
                if (tblBookingParitiesTOList != null && tblBookingParitiesTOList.Count > 0)
                {
                    tblBookingParitiesTOList.ForEach(x =>
                    {
                        if (x.BookingId == tblBookingsTO.IdBooking && x.BrandId == tblBookingsTO.BrandId)
                        {
                            tblBookingParitiesTO = x;
                        }
                    });
                }
                if (tblBookingParitiesTO != null)
                {
                    tblBookingParitiesTO.BookingRate = tblBookingsTO.BookingRate;
                    updateBookingparityResult = _iTblBookingParitiesDAO.UpdateTblBookingParities(tblBookingParitiesTO, conn, tran);
                    if (updateBookingparityResult != 1)
                    {
                        tran.Rollback();
                        resultMessage.DisplayMessage = "Error while update in Booking parities ";
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
                        tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.BookingDisplayNo + " Director has remark -" + tblBookingsTO.DirectorRemark;
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
                        tblAlertInstanceTO.AlertComment = "Enquiry No. #" + tblBookingsTO.BookingDisplayNo + " Director has remark -" + tblBookingsTO.DirectorRemark;
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
        public ResultMessage UpdatePendingQuantity(TblBookingQtyConsumptionTO tblBookingQtyConsumption)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = _iCommon.ServerDateTime;

            int result = 0;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();
                TblBookingsTO tblBookingsTO = new TblBookingsTO();

                tblBookingsTO = SelectBookingsTOWithDetails(tblBookingQtyConsumption.BookingId);
                if (tblBookingsTO == null)
                {
                    resultMessage.DefaultBehaviour("booking TO not found against bookingId-" + tblBookingQtyConsumption.BookingId);
                    return resultMessage;
                }

                if (tblBookingsTO.PendingQty == 0)
                {
                    resultMessage.DefaultBehaviour("Quantity Already done");
                    return resultMessage;
                }

                tblBookingQtyConsumption.ConsumptionQty = tblBookingsTO.PendingQty;
                tblBookingsTO.PendingQty = 0;

                result = _iTblBookingsDAO.UpdatePendingQuantity(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error while Updating Quantity");
                    return resultMessage;
                }
                tblBookingQtyConsumption.BookingId = tblBookingsTO.IdBooking;
                tblBookingQtyConsumption.CreatedOn = serverDate;
                tblBookingQtyConsumption.StatusId = (int)tblBookingsTO.TranStatusE;
                tblBookingQtyConsumption.WeightTolerance = null;
                result = _iTblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumption, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error while InsertTblBookingQtyConsumption");
                    return resultMessage;
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostCloseQuantity");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }

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
