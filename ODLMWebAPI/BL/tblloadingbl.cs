using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using ODLMWebAPI.BL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DashboardModels;
using ODLMWebAPI.DAL;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.IoT;
using ODLMWebAPI.IoT.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.IO;
using Newtonsoft.Json;

namespace ODLMWebAPI.BL {
    public class TblLoadingBL : ITblLoadingBL {
        private IModbusRefConfig _iModbusRefConfig;
        private readonly ITblLoadingDAO _iTblLoadingDAO;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblOrganizationDAO _iTblOrganizationDAO;
        private readonly ITblMaterialBL _iTblMaterialBL;
        private readonly ITblLoadingSlipBL _iTblLoadingSlipBL;
        private readonly ITempLoadingSlipInvoiceDAO _iTempLoadingSlipInvoiceDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblLoadingSlipDtlDAO _iTblLoadingSlipDtlDAO;
        private readonly ITblBookingsDAO _iTblBookingsDAO;
        private readonly ITblBookingParitiesDAO _iTblBookingParitiesDAO;
        private readonly IDimBrandDAO _iDimBrandDAO;
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly ITblParityDetailsBL _iTblParityDetailsBL;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblGstCodeDtlsDAO _iTblGstCodeDtlsDAO;
        private readonly ITblInvoiceBL _iTblInvoiceBL; // working
        private readonly ITblEntityRangeDAO _iTblEntityRangeDAO;
        private readonly ITblLoadingSlipDAO _iTblLoadingSlipDAO;
        private readonly ITblBookingQtyConsumptionDAO _iTblBookingQtyConsumptionDAO;
        private readonly ITblBookingExtDAO _iTblBookingExtDAO;
        private readonly ITblLoadingStatusHistoryDAO _iTblLoadingStatusHistoryDAO;
        private readonly ITblLoadingSlipAddressDAO _iTblLoadingSlipAddressDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly ITblLoadingSlipExtDAO _iTblLoadingSlipExtDAO;
        private readonly ITblProductInfoDAO _iTblProductInfoDAO;
        private readonly ITblStockConfigDAO _iTblStockConfigDAO;
        private readonly ITblStockDetailsDAO _iTblStockDetailsDAO;
        private readonly ITblLocationDAO _iTblLocationDAO;
        private readonly ITblStockConsumptionDAO _iTblStockConsumptionDAO;
        private readonly ITblLoadingQuotaConsumptionDAO _iTblLoadingQuotaConsumptionDAO;
        private readonly ITblLoadingQuotaDeclarationDAO _iTblLoadingQuotaDeclarationDAO;
        private readonly ITblWeighingMeasuresDAO _iTblWeighingMeasuresDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblLoadingVehDocExtBL _iTblLoadingVehDocExtBL;
        private readonly IDimStatusDAO _iDimStatusDAO;
        private readonly ITblTransportSlipDAO _iTblTransportSlipDAO;
        private readonly ITblLoadingSlipRemovedItemsDAO _iTblLoadingSlipRemovedItemsDAO;
        private readonly ITblLoadingSlipExtHistoryDAO _iTblLoadingSlipExtHistoryDAO;
        private readonly ITblInvoiceItemDetailsDAO _iTblInvoiceItemDetailsDAO;
        private readonly ITblStockSummaryDAO _iTblStockSummaryDAO;
        private readonly ITblInvoiceDAO _iTblInvoiceDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        private readonly ICircularDependencyBL _iCircularDependencyBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly IFinalBookingData _iFinalBookingData;
        private readonly IFinalEnquiryData _iFinalEnquiryData;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblPaymentTermOptionRelationDAO _iTblPaymentTermOptionRelationDAO;
        private readonly ITblGateBL _iTblGateBL;
        private readonly IIotCommunication _iIotCommunication;
        private readonly IGateCommunication _iGateCommunication;
        private readonly ITblWeighingMachineDAO _iTblWeighingMachineDAO;
        private readonly IWeighingCommunication _iWeighingCommunication;
        private readonly ITblInvoiceHistoryDAO _iTblInvoiceHistoryDAO;
        private readonly ITblBookingDelAddrDAO _iTblBookingDelAddrDAO;
        private readonly ITblAlertDefinitionDAO _iTblAlertDefinitionDAO;
        private readonly ITblGroupItemDAO _iTblGroupItemDAO;
        private readonly ITblGlobalRateDAO _iTblGlobalRateDAO;
        private readonly IRunReport _iRunReport;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblPaymentTermsForBookingDAO _iTblPaymentTermsForBookingDAO;
        private readonly IDimStatusBL _iDimStatusBL;
        //AmolG[2020-Feb-25] added for changing booking schedule qty
        private readonly ITblBookingScheduleDAO _iTblBookingScheduleDAO;

        //Saket [2020-03-26] Locker object added.
        private static readonly object generateInvoiceNoLock = new object();

        public TblLoadingBL(ITblAlertDefinitionDAO iTblAlertDefinitionDAO, IWeighingCommunication iWeighingCommunication, IModbusRefConfig iModbusRefConfig,ITblBookingDelAddrDAO iTblBookingDelAddrDAO, ITblInvoiceHistoryDAO iTblInvoiceHistoryDAO, ITblWeighingMachineDAO iTblWeighingMachineDAO, IGateCommunication iGateCommunication, IIotCommunication iIotCommunication, ITblGateBL iTblGateBL
            , ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, ITblInvoiceBL iTblInvoiceBL, IFinalEnquiryData iFinalEnquiryData, IFinalBookingData iFinalBookingData, ITblConfigParamsDAO iTblConfigParamsDAO, ITblAddressBL iTblAddressBL, ITblInvoiceDAO iTblInvoiceDAO, ITblStockSummaryDAO iTblStockSummaryDAO, ITblBookingsDAO iTblBookingsDAO, ITblInvoiceItemDetailsDAO iTblInvoiceItemDetailsDAO
            , ITblLoadingSlipExtHistoryDAO iTblLoadingSlipExtHistoryDAO, ITblLoadingSlipRemovedItemsDAO iTblLoadingSlipRemovedItemsDAO, ITblTransportSlipDAO iTblTransportSlipDAO, IDimStatusDAO iDimStatusDAO, ITblLoadingVehDocExtBL iTblLoadingVehDocExtBL, ITblUserDAO iTblUserDAO, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO, ITblLoadingQuotaDeclarationDAO iTblLoadingQuotaDeclarationDAO, ITblLoadingQuotaConsumptionDAO iTblLoadingQuotaConsumptionDAO
            , ITblStockConsumptionDAO iTblStockConsumptionDAO, ITblStockConfigDAO iTblStockConfigDAO, ITblProductInfoDAO iTblProductInfoDAO, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblProductItemDAO iTblProductItemDAO, ITblLocationDAO iTblLocationDAO, ITblStockDetailsDAO iTblStockDetailsDAO, ITblAlertInstanceBL iTblAlertInstanceBL, ITblLoadingSlipAddressDAO iTblLoadingSlipAddressDAO, ITblLoadingStatusHistoryDAO iTblLoadingStatusHistoryDAO
            , ITblBookingExtDAO iTblBookingExtDAO, ITblBookingQtyConsumptionDAO iTblBookingQtyConsumptionDAO, ITblLoadingSlipDAO iTblLoadingSlipDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO, IDimensionDAO iDimensionDAO, ITblParityDetailsBL iTblParityDetailsBL, ITblAddressDAO iTblAddressDAO, IDimBrandDAO iDimBrandDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO
            , ITblConfigParamsBL iTblConfigParamsBL, ITempLoadingSlipInvoiceDAO iTempLoadingSlipInvoiceDAO, ITblLoadingSlipBL iTblLoadingSlipBL, ITblMaterialBL iTblMaterialBL, ITblOrganizationDAO iTblOrganizationDAO, ICircularDependencyBL iCircularDependencyBL, ICommon iCommon, IConnectionString iConnectionString, ITblLoadingDAO iTblLoadingDAO, ITblUserRoleBL iTblUserRoleBL
            , ITblGroupItemDAO iTblGroupItemDAO, ITblGlobalRateDAO iTblGlobalRateDAO, IRunReport iRunReport, IDimReportTemplateBL iDimReportTemplateBL, ITblPaymentTermsForBookingDAO iTblPaymentTermsForBookingDAO, IDimStatusBL iDimStatusBL
            , ITblBookingScheduleDAO iTblBookingScheduleDAO)
        
       // public TblLoadingBL(ITblAlertDefinitionDAO iTblAlertDefinitionDAO,ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, ITblInvoiceBL iTblInvoiceBL, IFinalEnquiryData iFinalEnquiryData, IFinalBookingData iFinalBookingData, ITblConfigParamsDAO iTblConfigParamsDAO, ITblAddressBL iTblAddressBL, ITblInvoiceDAO iTblInvoiceDAO, ITblStockSummaryDAO iTblStockSummaryDAO, ITblBookingsDAO iTblBookingsDAO, ITblInvoiceItemDetailsDAO iTblInvoiceItemDetailsDAO, ITblLoadingSlipExtHistoryDAO iTblLoadingSlipExtHistoryDAO, ITblLoadingSlipRemovedItemsDAO iTblLoadingSlipRemovedItemsDAO, ITblTransportSlipDAO iTblTransportSlipDAO, IDimStatusDAO iDimStatusDAO, ITblLoadingVehDocExtBL iTblLoadingVehDocExtBL, ITblUserDAO iTblUserDAO, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO, ITblLoadingQuotaDeclarationDAO iTblLoadingQuotaDeclarationDAO, ITblLoadingQuotaConsumptionDAO iTblLoadingQuotaConsumptionDAO, ITblStockConsumptionDAO iTblStockConsumptionDAO, ITblStockConfigDAO iTblStockConfigDAO, ITblProductInfoDAO iTblProductInfoDAO, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, ITblProductItemDAO iTblProductItemDAO, ITblLocationDAO iTblLocationDAO, ITblStockDetailsDAO iTblStockDetailsDAO, ITblAlertInstanceBL iTblAlertInstanceBL, ITblLoadingSlipAddressDAO iTblLoadingSlipAddressDAO, ITblLoadingStatusHistoryDAO iTblLoadingStatusHistoryDAO, ITblBookingExtDAO iTblBookingExtDAO, ITblBookingQtyConsumptionDAO iTblBookingQtyConsumptionDAO, ITblLoadingSlipDAO iTblLoadingSlipDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO, IDimensionDAO iDimensionDAO, ITblParityDetailsBL iTblParityDetailsBL, ITblAddressDAO iTblAddressDAO, IDimBrandDAO iDimBrandDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblConfigParamsBL iTblConfigParamsBL, ITempLoadingSlipInvoiceDAO iTempLoadingSlipInvoiceDAO, ITblLoadingSlipBL iTblLoadingSlipBL, ITblMaterialBL iTblMaterialBL, ITblOrganizationDAO iTblOrganizationDAO, ICircularDependencyBL iCircularDependencyBL, ICommon iCommon, IConnectionString iConnectionString, ITblLoadingDAO iTblLoadingDAO, ITblUserRoleBL iTblUserRoleBL)
        {
            _iTblLoadingDAO = iTblLoadingDAO;
            _iRunReport = iRunReport;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iTblPaymentTermsForBookingDAO = iTblPaymentTermsForBookingDAO;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblOrganizationDAO = iTblOrganizationDAO;
            _iTblMaterialBL = iTblMaterialBL;
            _iTblLoadingSlipBL = iTblLoadingSlipBL;
            _iTempLoadingSlipInvoiceDAO = iTempLoadingSlipInvoiceDAO;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblLoadingSlipDtlDAO = iTblLoadingSlipDtlDAO;
            _iTblBookingsDAO = iTblBookingsDAO;
            _iTblBookingParitiesDAO = iTblBookingParitiesDAO;
            _iDimBrandDAO = iDimBrandDAO;
            _iTblAddressDAO = iTblAddressDAO;
            _iTblParityDetailsBL = iTblParityDetailsBL;
            _iDimensionDAO = iDimensionDAO;
            _iTblGstCodeDtlsDAO = iTblGstCodeDtlsDAO;
            _iTblInvoiceBL = iTblInvoiceBL;
            _iTblEntityRangeDAO = iTblEntityRangeDAO;
            _iTblLoadingSlipDAO = iTblLoadingSlipDAO;
            _iTblBookingQtyConsumptionDAO = iTblBookingQtyConsumptionDAO;
            _iTblBookingExtDAO = iTblBookingExtDAO;
            _iTblLoadingStatusHistoryDAO = iTblLoadingStatusHistoryDAO;
            _iTblLoadingSlipAddressDAO = iTblLoadingSlipAddressDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblProductItemDAO = iTblProductItemDAO;
            _iTblLoadingSlipExtDAO = iTblLoadingSlipExtDAO;
            _iTblProductInfoDAO = iTblProductInfoDAO;
            _iTblStockConfigDAO = iTblStockConfigDAO;
            _iTblStockDetailsDAO = iTblStockDetailsDAO;
            _iTblLocationDAO = iTblLocationDAO;
            _iTblStockConsumptionDAO = iTblStockConsumptionDAO;
            _iTblLoadingQuotaConsumptionDAO = iTblLoadingQuotaConsumptionDAO;
            _iTblLoadingQuotaDeclarationDAO = iTblLoadingQuotaDeclarationDAO;
            _iTblWeighingMeasuresDAO = iTblWeighingMeasuresDAO;
            _iTblUserDAO = iTblUserDAO;
            _iTblLoadingVehDocExtBL = iTblLoadingVehDocExtBL;
            _iDimStatusDAO = iDimStatusDAO;
            _iTblTransportSlipDAO = iTblTransportSlipDAO;
            _iTblLoadingSlipRemovedItemsDAO = iTblLoadingSlipRemovedItemsDAO;
            _iTblLoadingSlipExtHistoryDAO = iTblLoadingSlipExtHistoryDAO;
            _iTblInvoiceItemDetailsDAO = iTblInvoiceItemDetailsDAO;
            _iTblStockSummaryDAO = iTblStockSummaryDAO;
            _iTblInvoiceDAO = iTblInvoiceDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iCircularDependencyBL = iCircularDependencyBL;
            _iTblAddressBL = iTblAddressBL;
            _iFinalBookingData = iFinalBookingData;
            _iFinalEnquiryData = iFinalEnquiryData;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblPaymentTermOptionRelationDAO = iTblPaymentTermOptionRelationDAO;
            _iTblGateBL = iTblGateBL;
            _iIotCommunication = iIotCommunication;
            _iGateCommunication = iGateCommunication;
            _iTblWeighingMachineDAO = iTblWeighingMachineDAO;
            _iWeighingCommunication = iWeighingCommunication;
            _iTblInvoiceHistoryDAO = iTblInvoiceHistoryDAO;
            _iTblBookingDelAddrDAO = iTblBookingDelAddrDAO;
            _iModbusRefConfig  = iModbusRefConfig;
            _iTblAlertDefinitionDAO = iTblAlertDefinitionDAO;
            _iTblGroupItemDAO = iTblGroupItemDAO;
            _iTblGlobalRateDAO = iTblGlobalRateDAO;
            _iDimStatusBL = iDimStatusBL;
            _iTblBookingScheduleDAO = iTblBookingScheduleDAO;
        }
        #region Selection



        public void GetWeighingMeasuresFromIoT(string loadingId, bool isUnloading, List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                string[] loadingIdsList = loadingId.Split(',');
                if (loadingIdsList != null && loadingIdsList.Length > 0)
                {
                    //List<TblWeighingMachineTO> tblWeighingMachineList = BL.TblWeighingMachineBL.SelectAllTblWeighingMachineList();
                    for (int i = 0; i < loadingIdsList.Length; i++) {
                        TblLoadingTO tblLoadingTO = SelectLoadingTOWithDetails (Convert.ToInt32 (loadingIdsList[i]));
                        //tblLoadingTO = TblLoadingBL.getDataFromIotAndMerge(tblLoadingTO);
                        //NodeJsResult itemList = GetLoadingLayerData(tblLoadingTO.ModbusRefId, 0);
                        // List<int[]> weighingDataList = new List<int[]>();
                        //if (itemList != null)
                        //{
                        //    List<int[]> defaultResultList = new List<int[]>();
                        //    if (itemList.Data != null && itemList.Data.Count > 0)
                        //    {
                        //        defaultResultList = itemList.Data.Where(w => w[(int)IoTConstants.WeightIotColE.WeighTypeId] == (Int32)Constants.TransMeasureTypeE.TARE_WEIGHT || w[(int)IoTConstants.WeightIotColE.WeighTypeId] == (Int32)Constants.TransMeasureTypeE.GROSS_WEIGHT).ToList();
                        //    }
                        //    weighingDataList.AddRange(defaultResultList);
                        //}
                        if (tblLoadingTO.DynamicItemListDCT != null && tblLoadingTO.DynamicItemListDCT.Count > 0) {
                            foreach (KeyValuePair<int, List<int[]>> pair in tblLoadingTO.DynamicItemListDCT) {
                                foreach (var item in pair.Value) {
                                    TblWeighingMeasuresTO measuresTO = new TblWeighingMeasuresTO ();
                                    measuresTO.LoadingId = tblLoadingTO.IdLoading;
                                    measuresTO.WeightMeasurTypeId = item[(int) IoTConstants.WeightIotColE.WeighTypeId];
                                    measuresTO.WeightMT = item[(int) IoTConstants.WeightIotColE.Weight];
                                    measuresTO.VehicleNo = tblLoadingTO.VehicleNo;
                                    measuresTO.UnLoadingId = Convert.ToInt32 (isUnloading);
                                    measuresTO.WeighingMachineId = pair.Key;
                                    int dateTimeVal = item[(int) IoTConstants.WeightIotColE.TimeStamp];
                                    string dateTimeValTemp = dateTimeVal.ToString ();
                                    if (dateTimeValTemp.Length <= 5) {
                                        dateTimeValTemp = "0" + dateTimeVal;
                                    }
                                    string hrsMin = dateTimeValTemp.Substring (2, 4);
                                    string hrs = hrsMin.Substring (0, 2).ToString ();
                                    string min = hrsMin.Substring (2, 2).ToString ();
                                    string date = dateTimeValTemp.Replace (hrsMin, "");

                                    DateTime dateTime = new DateTime (_iCommon.ServerDateTime.Year, _iCommon.ServerDateTime.Month, Convert.ToInt32 (date), Convert.ToInt32 (hrs), Convert.ToInt32 (min), 0);
                                    measuresTO.CreatedOn = dateTime;
                                    tblWeighingMeasuresTOList.Add (measuresTO);
                                }
                                //Console.WriteLine("{0}, {1}", pair.Key, pair.Value);
                            }
                        }
                        //if (tblLoadingTO.DynamicItemList != null && tblLoadingTO.DynamicItemList.Count > 0)
                        //    weighingDataList.AddRange(tblLoadingTO.DynamicItemList);
                        //for (int wd = 0; wd < weighingDataList.Count; wd++)
                        //{
                        //    TblWeighingMeasuresTO measuresTO = new TblWeighingMeasuresTO();
                        //    measuresTO.LoadingId = tblLoadingTO.IdLoading;
                        //    measuresTO.WeightMeasurTypeId = weighingDataList[wd][(int)IoTConstants.WeightIotColE.WeighTypeId];
                        //    measuresTO.WeightMT = weighingDataList[wd][(int)IoTConstants.WeightIotColE.Weight];
                        //    measuresTO.VehicleNo = tblLoadingTO.VehicleNo;
                        //    measuresTO.UnLoadingId = Convert.ToInt32(isUnloading);
                        //    measuresTO.WeighingMachineId = tblLoadingTO.DynamicItemListDCT.Keys[];
                        //    // measuresTO.CreatedBy = 1;
                        //    tblWeighingMeasuresTOList.Add(measuresTO);
                        //}
                    }
                }
            } catch (Exception ex) {
                return;
            }
        }

        public List<TblLoadingTO> SelectAllTblLoadingList () {
            return _iTblLoadingDAO.SelectAllTblLoading ();
        }
        //Priyanka [11-05-2018] : Added to get all loading slip list whose 
        //                        vehicle status is Gate In Or Loading Completed.
        public List<TblLoadingTO> SelectAllTblLoadingListForConvertNCToC () {
            return _iTblLoadingDAO.SelectAllTblLoadingListForConvertNCToC ();
        }
        public List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId (int parentLoadingId) {
            return _iTblLoadingDAO.SelectAllLoadingsFromParentLoadingId (parentLoadingId);
        }

        public List<TblLoadingTO> SelectAllTblloadingList (DateTime fromDate, DateTime toDate,string selectedOrgStr) {
            return _iTblLoadingDAO.SelectAllTblloadingList (fromDate, toDate, selectedOrgStr);
        }

        public List<TblLoadingTO> SelectAllTblLoadingList(List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgStr, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId, Int32 superwisorId)
        {
            //Aniket [30-7-2019] added for IOT

            var checkIotFlag = loadingStatusId;
            int configId = _iTblConfigParamsDAO.IoTSetting ();
            if (configId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT)) {
                checkIotFlag = 0;
            }

            //Priyanka [12-12-2018]
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO ();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0) {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority (tblUserRoleTOList);
            }

            List<TblLoadingTO> tblLoadingTOList = _iTblLoadingDAO.SelectAllTblLoading (tblUserRoleTO, cnfId, loadingStatusId, fromDate, toDate, loadingTypeId, dealerId,selectedOrgStr, isConfirm, brandId, loadingNavigateId, superwisorId);

            if (cnfId > 0) {
                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0) {
                    String name = _iTblOrganizationDAO.SelectFirmNameOfOrganiationById (cnfId);
                    if (!String.IsNullOrEmpty (name)) {
                        tblLoadingTOList.ForEach (c => c.CnfOrgName = name);
                    }
                }
            }
            List<TblLoadingTO> list = new List<TblLoadingTO> ();
            List<TblLoadingTO> finalList = new List<TblLoadingTO> ();
            if (configId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT)) {
                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0) {
                    var deliverList = tblLoadingTOList.Where (s => s.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || s.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || s.TranStatusE == Constants.TranStatusE.LOADING_NOT_CONFIRM).ToList ();
                    // var deliverList = tblLoadingTOList.Where(s => s.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED || s.TranStatusE == Constants.TranStatusE.LOADING_CANCEL).ToList();
                    string finalStatusId = _iIotCommunication.GetIotEncodedStatusIdsForGivenStatus (loadingStatusId.ToString ());
                    list = SetLoadingStatusData (finalStatusId.ToString (), true, configId, tblLoadingTOList);
                    if (deliverList != null)
                        finalList.AddRange (deliverList);
                    if (list != null)
                        finalList.AddRange (list);
                }

                if (finalList != null && finalList.Count > 0) {
                    if (loadingStatusId > 0) {
                        finalList = finalList.Where (w => w.StatusId == loadingStatusId).ToList ();
                    }
                }
                return finalList;
            } else {
                return tblLoadingTOList;
            }


        }

        //Aniket [30-7-2019] added for IOT
        public List<TblLoadingTO> SetLoadingStatusData (String loadingStatusId, bool isEncoded, int configId, List<TblLoadingTO> tblLoadingTOList) {
            if (configId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT)) {
                List<DimStatusTO> statusList = _iDimStatusDAO.SelectAllDimStatus ((Int32)Constants.TransactionTypeE.LOADING);
                //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(loadingStatusId.ToString());

                List<TblLoadingTO> distGate = tblLoadingTOList.GroupBy (g => g.GateId).Select (s => s.FirstOrDefault ()).ToList ();

                GateIoTResult gateIoTResult = new GateIoTResult ();

                for (int g = 0; g < distGate.Count; g++) {
                    TblLoadingTO tblLoadingTOTemp = distGate[g];
                    TblGateTO tblGateTO = new TblGateTO (tblLoadingTOTemp.GateId, tblLoadingTOTemp.IoTUrl, tblLoadingTOTemp.MachineIP, tblLoadingTOTemp.PortNumber);
                    GateIoTResult temp = _iIotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId (loadingStatusId.ToString (), tblGateTO);

                    if (temp != null && temp.Data != null) {
                        gateIoTResult.Data.AddRange (temp.Data);
                    }
                }

                if (gateIoTResult != null && gateIoTResult.Data != null) {
                    for (int d = 0; d < tblLoadingTOList.Count; d++) {
                        var data = gateIoTResult.Data.Where (w => Convert.ToInt32 (w[0]) == tblLoadingTOList[d].ModbusRefId).FirstOrDefault ();
                        if (data != null) {
                         //   tblLoadingTOList[d].VehicleNo = Convert.ToString (data[(int) IoTConstants.GateIoTColE.VehicleNo]);
                         //chetan[10-feb-2020] added add old vehicle on IOT
                            tblLoadingTOList[d].VehicleNo = _iIotCommunication.GetVehicleNumbers(Convert.ToString(data[(int)IoTConstants.GateIoTColE.VehicleNo]), true);
                            if (data.Length > 3)
                                tblLoadingTOList[d].TransporterOrgId = Convert.ToInt32 (data[(int) IoTConstants.GateIoTColE.TransportorId]);
                            DimStatusTO dimStatusTO = statusList.Where (w => w.IotStatusId == Convert.ToInt32 (data[(int) IoTConstants.GateIoTColE.StatusId])).FirstOrDefault ();
                            if (dimStatusTO != null) {
                                tblLoadingTOList[d].StatusId = dimStatusTO.IdStatus;
                                tblLoadingTOList[d].StatusDesc = dimStatusTO.StatusName;
                            }

                        } else {
                            tblLoadingTOList.RemoveAt (d);
                            d--;
                        }
                    }
                    if (!String.IsNullOrEmpty (loadingStatusId)) {
                        string statusIdList = string.Empty;
                        if (isEncoded)
                            statusIdList = _iIotCommunication.GetIotDecodedStatusIdsForGivenStatus (loadingStatusId);

                        var statusIds = statusIdList.Split (',').ToList ();

                        if (statusIds.Count == 1 && statusIds[0] == "0")
                            return tblLoadingTOList;

                        tblLoadingTOList = tblLoadingTOList.Where (w => statusIds.Contains (Convert.ToString (w.StatusId))).ToList ();

                        //tblLoadingTOList = tblLoadingTOList.Where(w => w.StatusId == loadingStatusId).ToList();
                    }
                } else {
                    tblLoadingTOList = new List<TblLoadingTO> ();
                }
            }

            return tblLoadingTOList;
        }

        public ResultMessage CheckIotConnectivity()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                String statusStr = Convert.ToString((Int32)Constants.TranStatusE.LOADING_DELIVERED);
                String resultMsg = String.Empty;

                int configId = _iTblConfigParamsDAO.IoTSetting();
                if (configId != Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "IOT setting is OFF";
                    return resultMessage;
                }

                #region Gate

                List<TblGateTO> tblGateTOList = _iTblGateBL.SelectAllTblGateList(Constants.ActiveSelectionTypeE.Active);
                tblGateTOList = tblGateTOList.Where(w => w.ModuleId == Constants.DefaultModuleID).ToList();
                DateTime serverDate = _iCommon.ServerDateTime;

                for (int g = 0; g < tblGateTOList.Count; g++)
                {
                    TblGateTO tblGateTO = tblGateTOList[g];
                    TblLoadingTO tblLoadingTO = new TblLoadingTO();
                    tblLoadingTO.ModbusRefId = 1;
                    tblLoadingTO.PortNumber = tblGateTO.PortNumber;
                    tblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    tblLoadingTO.IoTUrl = tblGateTO.IoTUrl;

                    resultMessage = PingIOTDevice(tblLoadingTO);
                    if (resultMessage != null && resultMessage.Exception != null)
                    {
                        resultMessage = PingIOTDevice(tblLoadingTO);
                        if (resultMessage != null && resultMessage.Exception != null)
                        {
                            string tempPortNo = tblGateTO.PortNumber;
                            string tempIp = tblGateTO.MachineIP;

                            tblGateTO.PortNumber = tblGateTO.AltPortNo;
                            tblGateTO.MachineIP = tblGateTO.AltMachineIP;

                            tblGateTO.AltPortNo = tempPortNo;
                            tblGateTO.AltMachineIP = tempIp;
                            tblGateTO.UpdatedOn = _iCommon.ServerDateTime;
                            tblGateTO.UpdatedBy = 1;

                            Int32 result = _iTblGateBL.UpdateTblGatePortAndIp(tblGateTO);
                            if (result == -1)
                            {
                                resultMessage.DefaultBehaviour();
                                return resultMessage;

                            }
                            resultMsg = " GATE Changeover done successfully";
                        }
                    }

                }

                #endregion

                #region Weight
                Int32 loadingId = 1;
                List<TblWeighingMachineTO> tblWeighingMachineTOList = _iTblWeighingMachineDAO.SelectAllTblWeighingMachine();
                if (tblWeighingMachineTOList != null && tblWeighingMachineTOList.Count > 0)
                {
                    tblWeighingMachineTOList = tblWeighingMachineTOList.Where(a => a.ModuleId == Constants.DefaultModuleID).ToList();

                    for (int k = 0; k < tblWeighingMachineTOList.Count; k++)
                    {
                        TblWeighingMachineTO tblWeighingMachineTO = tblWeighingMachineTOList[k];

                        NodeJsResult itemList = _iWeighingCommunication.GetLoadingLayerData(loadingId, 1, tblWeighingMachineTO);
                        if (itemList != null && itemList.Exception != null)
                        {
                            itemList = _iWeighingCommunication.GetLoadingLayerData(loadingId, 1, tblWeighingMachineTO);
                            if (itemList != null && itemList.Exception != null)
                            {
                                string tempPortNo = tblWeighingMachineTO.PortNumber;
                                string tempIp = tblWeighingMachineTO.MachineIP;

                                tblWeighingMachineTO.PortNumber = tblWeighingMachineTO.AltPortNo;
                                tblWeighingMachineTO.MachineIP = tblWeighingMachineTO.AltMachineIP;

                                tblWeighingMachineTO.AltPortNo = tempPortNo;
                                tblWeighingMachineTO.AltMachineIP = tempIp;
                                tblWeighingMachineTO.UpdatedBy = 1;
                                tblWeighingMachineTO.UpdatedOn = _iCommon.ServerDateTime;

                                Int32 result = _iTblWeighingMachineDAO.UpdateTblWeighingMachinePortAndIp(tblWeighingMachineTO);
                                if (result == -1)
                                {
                                    resultMessage.DefaultBehaviour();
                                    return resultMessage;
                                }
                                resultMsg += " WEIGHT Changeover done successfully";
                            }
                        }

                    }
                }

                #endregion


                resultMessage.DefaultSuccessBehaviour();
                resultMessage.DisplayMessage = resultMsg;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            } 
        }

        public ResultMessage PingIOTDevice(TblLoadingTO tblLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                tblLoadingTO = _iIotCommunication.GetItemDataFromIotAndMerge(tblLoadingTO, false, false, 0);

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }
        

        public ResultMessage RemoveDatFromIotDevice()
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            String statusStr = Convert.ToString((Int32)Constants.TranStatusE.LOADING_DELIVERED);

            List<TblGateTO> tblGateTOList = _iTblGateBL.SelectAllTblGateList(Constants.ActiveSelectionTypeE.Active);

            tblGateTOList = tblGateTOList.Where(w => w.ModuleId == Constants.DefaultModuleID).ToList();
            DateTime serverDate = _iCommon.ServerDateTime;

            for (int g = 0; g < tblGateTOList.Count; g++)
            {

                TblGateTO tblGateTO = tblGateTOList[g];

                GateIoTResult gateIoTResult = _iIotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(statusStr, tblGateTO);
                if (gateIoTResult != null && gateIoTResult.Data != null)
                {
                    for (int i = 0; i < gateIoTResult.Data.Count; i++)
                    {
                        if (gateIoTResult.Data[i] != null)
                        {
                            Int32 modBusLoadingRefId = Convert.ToInt32(gateIoTResult.Data[i][(int)IoTConstants.GateIoTColE.LoadingId]);

                            TblLoadingTO tblLoadingTO = SelectTblLoadingTOByModBusRefId(modBusLoadingRefId);

                            if (tblLoadingTO == null)
                            {
                                continue;
                            }

                                //vipul[18/4/19] check allowed to remove or not
                            if (tblLoadingTO == null || tblLoadingTO.IsDBup == 0)
                            {

                               #region Saket [2020-10-27] Add vehicle Out Entry if backup not done against vehicle

                                String statusDate = (String)gateIoTResult.Data[i][(int)IoTConstants.GateIoTColE.StatusDate];

                                DateTime statusDateTime = _iIotCommunication.IoTDateTimeStringToDate(statusDate);

                                TimeSpan ts = serverDate - statusDateTime;

                                if (ts.TotalMinutes > 60)
                                {
                                    _iIotCommunication.GetItemDataFromIotAndMerge(tblLoadingTO, false, true);
                                    if (tblLoadingTO.LoadingStatusHistoryTOList != null && tblLoadingTO.LoadingStatusHistoryTOList.Count > 0)
                                    {
                                        var deliverTOList = tblLoadingTO.LoadingStatusHistoryTOList.Where(w => w.StatusId == (Int32)StaticStuff.Constants.TranStatusE.LOADING_DELIVERED).ToList();

                                        if (deliverTOList != null && deliverTOList.Count > 0)
                                        {

                                            var distinctDate = deliverTOList.GroupBy(g1 => g1.StatusDate).Select(s => s.FirstOrDefault()).ToList();

                                            if (distinctDate.Count >= 3)
                                            {
                                                continue;
                                            }
                                        }

                                        //Write Deliver status
                                        resultMessage = UpdateLoadingStatusToGateIoT(tblLoadingTO, null, null);
                                        if (resultMessage.MessageType != ResultMessageE.Information)
                                        {
                                            continue;

                                        }

                                    }
                                }
                                #endregion

                                continue;
                            }
                            //end
                            if (tblLoadingTO != null)
                            {
                                tblLoadingTO.ModbusRefId = modBusLoadingRefId;
                                resultMessage = MarkDeliverAndRemoveModBusRefs(tblLoadingTO.IdLoading);

                            }
                        }
                    }
                }
            }
            //Startup.AvailableModbusRefList = DAL.TblLoadingDAO.GeModRefMaxData();
            //Added for remove or update current modbus ref List
            //List<int> list = _iTblLoadingDAO.GeModRefMaxData();
            //if (list == null)
            //    throw new Exception("Failed to get ModbusRefList");
            //_iModbusRefConfig.setModbusRefList(list);
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        public TblLoadingTO SelectTblLoadingTOByModBusRefId(Int32 modBusRefId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblLoadingTOByModBusRefId(modBusRefId, conn, tran);
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


        public ResultMessage MarkDeliverAndRemoveModBusRefs(Int32 loadingId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMessage = MarkDeliverAndRemoveModBusRefs(loadingId, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage MarkDeliverAndRemoveModBusRefs(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {

            DateTime serverDate = _iCommon.ServerDateTime;

            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int configId = _iTblConfigParamsDAO.IoTSetting();
                if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    if (loadingId == 0)
                    {
                        throw new Exception("loadingId == 0");
                    }
                    TblLoadingTO tblLoadingTO = SelectTblLoadingTO(loadingId, conn, tran);
                    tblLoadingTO.LoadingSlipList = _iTblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading);
                    //TblLoadingTO tblLoadingTO = SelectLoadingTOWithDetails(loadingId, conn, tran);
                    if (tblLoadingTO == null)
                    {
                        throw new Exception("tblLoadingTO == null");
                    }
                    if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                    {
                        throw new Exception("tblLoadingTO.LoadingSlipList == 0");
                    }
                    string loadingids = String.Join(",", tblLoadingTO.LoadingSlipList.Select(w => w.IdLoadingSlip).ToArray());
                    Double TareWeight = _iTblInvoiceDAO.GetTareWeightFromInvoice(loadingids, conn, tran);//tempExtList.Min(m => m.CalcTareWeight);
                    Double invoiceTareWt = 0;

                    for (int p = 0; p < tblLoadingTO.LoadingSlipList.Count; p++)
                    {
                        TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[p];

                        if (tblLoadingSlipTO.IsConfirmed == 1)
                        {
                            for (int s = 0; s < tblLoadingSlipTO.LoadingSlipExtTOList.Count; s++)
                            {
                                tblLoadingSlipTO.LoadingSlipExtTOList[s].CalcTareWeight = TareWeight;
                                TareWeight += tblLoadingSlipTO.LoadingSlipExtTOList[s].LoadedWeight;
                            }

                            invoiceTareWt = tblLoadingSlipTO.LoadingSlipExtTOList.Min(m => m.CalcTareWeight);

                        }

                        List<TblInvoiceTO> tblInvoiceTOList = _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                        
                        if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                        {
                            for (int k = 0; k < tblInvoiceTOList.Count; k++)
                            {
                                TblInvoiceTO tblInvoiceTO = tblInvoiceTOList[k];

                                //RemoveCommercialDataFromInvoice(conn, tran, tblInvoiceTO);
                                if (tblInvoiceTO.IsConfirmed == 0)
                                {
                                    resultMessage = _iTblInvoiceBL.DeleteTblInvoiceDetails(tblInvoiceTO, conn, tran);
                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                    {
                                        throw new Exception("Error While Deleting Invoice for idInvoice - " + tblInvoiceTO.IdInvoice);
                                    }
                                }
                                else
                                {
                                    tblInvoiceTO.DeliveredOn = serverDate;

                                    tblInvoiceTO.TareWeight = invoiceTareWt;
                                    tblInvoiceTO.GrossWeight = tblInvoiceTO.TareWeight + tblInvoiceTO.NetWeight;

                                    if (tblInvoiceTO.UpdatedOn == new DateTime())
                                    {
                                        tblInvoiceTO.UpdatedOn = _iCommon.ServerDateTime;
                                    }
                                    if (tblInvoiceTO.UpdatedBy == 0)
                                    {
                                        tblInvoiceTO.UpdatedBy = tblInvoiceTO.CreatedBy;
                                    }
                                    Int32 result = _iTblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO, conn, tran);
                                    if (result == -1)
                                    {
                                        throw new Exception("Error While Deleting Invoice for idInvoice - " + tblInvoiceTO.IdInvoice);
                                    }
                                }
                            }
                        }

                        if (tblLoadingSlipTO.IsConfirmed == 0)
                        {
                            //Delete loadingSlip
                            Double sum = tblInvoiceTOList.Where(w => w.IsConfirmed == 0).ToList().Count;

                            if (sum == tblInvoiceTOList.Count)
                            {
                                resultMessage = _iTblLoadingSlipBL.DeleteLoadingSlipWithDetails(tblLoadingTO, tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    throw new Exception("Error While Deleting Invoice for idInvoice - " + tblLoadingSlipTO.IdLoadingSlip);
                                }
                                tblLoadingTO.TotalLoadingQty = tblLoadingTO.TotalLoadingQty - tblLoadingSlipTO.LoadingSlipExtTOList.Sum(s => s.LoadingQty);
                                tblLoadingTO.NoOfDeliveries = tblLoadingTO.NoOfDeliveries - 1;
                                tblLoadingTO.LoadingSlipList.Remove(tblLoadingSlipTO);
                                p--;
                            }
                        }
                    }


                    //tblLoadingTO.LoadingSlipList = BL.TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                    if (tblLoadingTO.LoadingSlipList == null)
                    {
                        throw new Exception("Error While fetching Loading Slip's against loadingId - " + loadingId);
                    }
                    if (tblLoadingTO.LoadingSlipList.Count == 0)
                    {
                        //Int32
                        resultMessage = DeleteLoadingData(loadingId, conn, tran);
                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                        {
                            throw new Exception("Error While deleting Loading data for loadingId - " + loadingId);
                        }
                    }
                    int tempmodbusId = tblLoadingTO.ModbusRefId;
                    if (tblLoadingTO.LoadingSlipList.Count > 0)
                    {
                        //Write DATA
                        tblLoadingTO.StatusId = (int)Constants.TranStatusE.LOADING_DELIVERED;
                        tblLoadingTO.StatusReason = "Delivered";
                        tblLoadingTO.StatusDesc = "Delivered";
                        tblLoadingTO.ModbusRefId = 0;
                        TblLoadingStatusHistoryTO tblLodingStatusHistory = new TblLoadingStatusHistoryTO();
                        tblLodingStatusHistory.LoadingId = tblLoadingTO.IdLoading;
                        tblLodingStatusHistory.StatusId = tblLoadingTO.StatusId;
                        tblLodingStatusHistory.CreatedBy = 1;
                        tblLodingStatusHistory.CreatedOn = _iCommon.ServerDateTime;
                        tblLodingStatusHistory.StatusDate = tblLodingStatusHistory.CreatedOn;
                        tblLodingStatusHistory.StatusRemark = tblLodingStatusHistory.StatusRemark;
                        int res = _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLodingStatusHistory, conn, tran);

                        if (res != 1)
                        {
                            throw new Exception("Error While inserting InsertTblLoadingStatusHistory for loadingId - " + tblLoadingTO.IdLoading);
                        }
                        Int32 result = UpdateTblLoading(tblLoadingTO, conn, tran);
                        if (result != 1)
                        {
                            throw new Exception("Error While updating Loading status for loadingId - " + tblLoadingTO.IdLoading);
                        }

                        for (int j = 0; j < tblLoadingTO.LoadingSlipList.Count; j++)
                        {
                            TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[j];

                            tblLoadingSlipTO.StatusId = tblLoadingTO.StatusId;
                            tblLoadingSlipTO.StatusReason = "Delivered";

                            result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                            if (result != 1)
                            {
                                throw new Exception("Error While updating LoadingSlip status For loadingslipId - " + tblLoadingSlipTO.IdLoadingSlip);
                            }


                            for (int k = 0; k < tblLoadingSlipTO.LoadingSlipExtTOList.Count; k++)
                            {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[k];

                                tblLoadingSlipExtTO.ModbusRefId = 0;

                                
                                Int32 tempResult = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);

                                if (tempResult != 1)
                                {
                                    throw new Exception("Error While updating LoadingSlip Ext status for Ext Id - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                                }
                            }


                        }
                    }
                    tblLoadingTO.ModbusRefId = tempmodbusId;
                    int result1 = RemoveDateFromGateAndWeightIOT(tblLoadingTO);
                    if (result1 != 1)
                    {
                        throw new Exception("Error While RemoveDateFromGateAndWeightIOT ");
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }


        public ResultMessage DeleteLoadingData(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                #region Delete Slip


                Int32 result = 0;
                List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO>();
                tblLoadingStatusHistoryTOList = _iTblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory(loadingId, conn, tran);
                if (tblLoadingStatusHistoryTOList == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("tblLoadingStatusHistoryTOList found null");
                    return resultMessage;
                }


                foreach (var tblLoadingStatusHistoryTO in tblLoadingStatusHistoryTOList)
                {
                    
                    result = _iTblLoadingStatusHistoryDAO.DeleteTblLoadingStatusHistory(tblLoadingStatusHistoryTO.IdLoadingHistory, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error while delete loadindingSlip status history");
                        return resultMessage;
                    }

                }

                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
                tblWeighingMeasuresTOList = SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);
                if (tblWeighingMeasuresTOList == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("tblWeighingMeasuresTOList found null");
                    return resultMessage;
                }

                foreach (var tblWeighingMeasuresTO in tblWeighingMeasuresTOList)
                {
                    if (tblWeighingMeasuresTO.IdWeightMeasure > 0)
                    {
                       
                        result = _iTblWeighingMeasuresDAO.DeleteTblWeighingMeasures(tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While Deleting tblWeighingMeasuresTOList ");
                            return resultMessage;
                        }
                    }
                }


                result = DeleteTblLoading(loadingId, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While Deleting Loading Details");
                    return resultMessage;
                }

                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage = new ResultMessage();
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteLoadingSlipWithDetails");
                return resultMessage;
            }
        }


        public List<TblWeighingMeasuresTO> SelectAllTblWeighingMeasuresListByLoadingId(int loadingId, SqlConnection conn, SqlTransaction tran)
        {

            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
            //int confiqId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingId, conn, tran);
                if (tblWeighingMeasuresTOList.Count > 0)
                {
                    tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
                }
            }
            else
            {
                GetWeighingMeasuresFromIoT(Convert.ToString(loadingId), false, tblWeighingMeasuresTOList, conn, tran);

            }
            return tblWeighingMeasuresTOList;
        }

        public TblLoadingTO SelectTblLoadingTOByModBusRefId(Int32 modBusRefId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingDAO.SelectTblLoadingTOByModBusRefId(modBusRefId, conn, tran);
        }
        /// <summary>
        /// @Kiran 19-04-2018
        /// </summary>
        /// <param name="TblLoadingTO"></param>
        /// <returns></returns>
        public List<TblLoadingTO> GetLoadingDetailsForReport (DateTime fromDate, DateTime toDate,string selectedOrgStr) {
            List<TblLoadingTO> tblLoadingToList = new List<TblLoadingTO> ();
            TblLoadingTO tblLoadingTO = new TblLoadingTO ();
            List<TblLoadingTO> tblLoadingTOList = SelectAllTblloadingList (fromDate, toDate, selectedOrgStr); //.FindAll(ele => ele.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT);

            if (tblLoadingTOList != null && tblLoadingTOList.Count > 0) {
                List<DropDownTO> MaterialList = _iTblMaterialBL.SelectAllMaterialListForDropDown ();
                for (int i = 0; i < tblLoadingTOList.Count; i++) {
                    tblLoadingTO = tblLoadingTOList[i];

                    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = _iCircularDependencyBL.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading);

                    tblLoadingTO.LoadingSlipList = _iTblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (Convert.ToInt32 (tblLoadingTO.IdLoading));
                    if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0) {

                        List<TblLoadingSlipTO> groupByVehicalDealer = tblLoadingTO.LoadingSlipList.GroupBy (g => g.DealerOrgId).Select (s => s.FirstOrDefault ()).ToList ();
                        for (int j = 0; j < groupByVehicalDealer.Count; j++) {
                            List<TblLoadingSlipExtTO> LoadingSlipExtList = new List<TblLoadingSlipExtTO> ();
                            TblLoadingSlipTO temp = groupByVehicalDealer[j];
                            List<TblLoadingSlipTO> gropByList = tblLoadingTO.LoadingSlipList.Where (w => w.DealerOrgId == temp.DealerOrgId).ToList ();
                            if (gropByList != null && gropByList.Count > 0) {
                                List<TblLoadingSlipAddressTO> addressList = new List<TblLoadingSlipAddressTO> ();
                                gropByList.ForEach (f => addressList.AddRange (f.DeliveryAddressTOList));
                                addressList = addressList.Where (w => w.TxnAddrTypeId == (int) Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).OrderBy (o => o.LoadingLayerId).ToList ();
                                temp.DeliveryAddressTOList = addressList;
                                foreach (var extentionItem in gropByList) {
                                    LoadingSlipExtList.AddRange (extentionItem.LoadingSlipExtTOList);
                                }
                                if (LoadingSlipExtList != null && LoadingSlipExtList.Count > 0) {
                                    string quntity = "";
                                    double totalSumLoadingQty = 0;
                                    double totalloadedQuantity = 0;

                                    double todaysTotalSumLoadingQty = 0;
                                    double todaysTotalloadedQuantity = 0;

                                    Dictionary<string, string> materialDictionary = new Dictionary<string, string> ();
                                    Dictionary<string, string> todaysMaterialDictionary = new Dictionary<string, string> ();

                                    foreach (var item in MaterialList) {
                                        List<TblLoadingSlipExtTO> tempExtList = new List<TblLoadingSlipExtTO> ();
                                        tempExtList = LoadingSlipExtList.Where (w => w.MaterialId == item.Value).ToList ();
                                        double sum = tempExtList.Sum (s => s.LoadingQty);
                                        double loadedQuantity = tempExtList.Sum (s => s.LoadedWeight);
                                        if (loadedQuantity != 0) {
                                            loadedQuantity = Math.Round (loadedQuantity / 1000, 3);
                                        }
                                        totalSumLoadingQty += sum;
                                        totalloadedQuantity += loadedQuantity;
                                        quntity = loadedQuantity + "/" + sum;
                                        materialDictionary.Add (item.Text, quntity);

                                        //Todays
                                        tempExtList = LoadingSlipExtList.Where (w => w.MaterialId == item.Value).ToList ();
                                        tempExtList = tempExtList.Where (w => w.UpdatedOn >= fromDate && w.UpdatedOn <= toDate).ToList ();

                                        double todaysSum = tempExtList.Sum (s => s.LoadingQty);
                                        double todaysloadedQuantity = tempExtList.Sum (s => s.LoadedWeight);
                                        if (todaysloadedQuantity != 0) {
                                            todaysloadedQuantity = Math.Round (todaysloadedQuantity / 1000, 3);
                                        }

                                        todaysTotalSumLoadingQty += sum;
                                        todaysTotalloadedQuantity += todaysloadedQuantity;
                                        String todaysQuntity = todaysloadedQuantity + "/" + sum;
                                        todaysMaterialDictionary.Add (item.Text, todaysQuntity);

                                    }

                                    materialDictionary.Add ("Todays Total", todaysTotalloadedQuantity + "/" + todaysTotalSumLoadingQty);
                                    //temp.Dictionary.Add(materialDictionary);

                                    materialDictionary.Add ("Total", totalloadedQuantity + "/" + totalSumLoadingQty);
                                    temp.Dictionary.Add (materialDictionary);
                                    materialDictionary = null;

                                    todaysMaterialDictionary.Add ("Todays Total", todaysTotalloadedQuantity + "/" + todaysTotalSumLoadingQty);
                                    //temp.TodaysDictionary.Add(todaysMaterialDictionary);

                                    todaysMaterialDictionary.Add ("Total", totalloadedQuantity + "/" + totalSumLoadingQty);
                                    temp.TodaysDictionary.Add (todaysMaterialDictionary);
                                }

                            }
                        }
                        tblLoadingTO.LoadingSlipList = groupByVehicalDealer;
                        tblLoadingToList.Add (tblLoadingTO);
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
        public List<TblLoadingTO> SelectAllTblLoadingLinkList (List<TblUserRoleTO> tblUserRoleTOList, Int32 dearlerOrgId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate) {

            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO ();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0) {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority (tblUserRoleTOList);
            }
            return _iTblLoadingDAO.SelectAllTblLoadingLinkList (tblUserRoleTO, dearlerOrgId, loadingStatusId, fromDate, toDate);
        }

        public List<TblLoadingTO> SelectAllLoadingListByStatus (string statusId, int gateId = 0) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return _iTblLoadingDAO.SelectAllLoadingListByStatus (statusId, conn, tran, gateId);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public TblLoadingTO SelectTblLoadingTO (Int32 idLoading, SqlConnection conn, SqlTransaction tran) {
            return _iTblLoadingDAO.SelectTblLoading (idLoading, conn, tran);
        }

        public TblLoadingTO SelectTblLoadingTO (Int32 idLoading) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return _iTblLoadingDAO.SelectTblLoading (idLoading, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public TblLoadingTO SelectTblLoadingTOByLoadingSlipId (Int32 loadingSlipId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId (loadingSlipId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public List<TblLoadingTO> SelectLoadingTOListWithDetails(string idLoadings)
        {
            try
            {
                string[] arrLoadingIds = null;
                List<TblLoadingTO> tblLoadingToList = new List<TblLoadingTO> ();
                //Aniket [30-7-2019] added for IOT
                int confiqId = _iTblConfigParamsDAO.IoTSetting ();
                if (idLoadings.Contains (',')) {
                    arrLoadingIds = idLoadings.Split (',');
                } else {
                    arrLoadingIds = new string[] { idLoadings };
                }
                foreach (string loadingId in arrLoadingIds) {
                    TblLoadingTO tblLoadingTO = SelectTblLoadingTO (Convert.ToInt32 (loadingId));
                    tblLoadingTO.LoadingSlipList = _iTblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (Convert.ToInt32 (loadingId));

                    //Aniket [30-7-2019] added for IOT
                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        _iIotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, true);
                    }
                    tblLoadingToList.Add (tblLoadingTO);
                }

                return tblLoadingToList;
            } catch (Exception ex) {
                return null;
            }

        }
        public TblLoadingTO SelectLoadingTOWithDetails (Int32 idLoading) {
            try {
                TblLoadingTO tblLoadingTO = SelectTblLoadingTO (idLoading);
                tblLoadingTO.LoadingSlipList = _iTblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (idLoading);
                //Aniket [30-7-2019] added for IOT
                if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_DELIVERED && tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_NOT_CONFIRM) {
                    int confiqId = _iTblConfigParamsDAO.IoTSetting ();
                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        _iIotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, true);
                    }
                }
                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            }

        }

        /// <summary>
        /// Saket [2018-04-25] Added.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public TblLoadingTO SelectLoadingTOWithDetailsByInvoiceId (Int32 invoiceId) {

            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                int confiqId = _iTblConfigParamsDAO.IoTSetting ();
                TblLoadingTO tblLoadingTO = null;

                //List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();

                List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOByInvoiceId (invoiceId, conn, tran);

                for (int i = 0; i < tempLoadingSlipInvoiceTOList.Count; i++) {
                    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = tempLoadingSlipInvoiceTOList[i];

                    TblLoadingSlipTO tblLoadingSlipTOTemp = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails (tempLoadingSlipInvoiceTO.LoadingSlipId, conn, tran);

                    if (tblLoadingTO == null) {
                        tblLoadingTO = SelectTblLoadingTO (tblLoadingSlipTOTemp.LoadingId, conn, tran);
                        if (tblLoadingTO == null) {
                            return new TblLoadingTO ();
                        }

                        if (tblLoadingTO.LoadingSlipList == null) {
                            tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO> ();
                        }

                        tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTOTemp);
                        if ((confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                            confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH) ) && tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_DELIVERED) {
                            _iIotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, false);

                            if (tempLoadingSlipInvoiceTO.LoadingSlipId == tblLoadingSlipTOTemp.IdLoadingSlip)
                                _iIotCommunication.GetItemDataFromIotForGivenLoadingSlip (tblLoadingSlipTOTemp);

                        }

                    } else {
                        if (tblLoadingTO.LoadingSlipList == null) {
                            tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO> ();
                        }

                        tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTOTemp);

                    }

                }

                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public TblLoadingTO SelectLoadingTOWithDetailsByLoadingSlipId (Int32 loadingSlipId) {
            try {
                TblLoadingTO tblLoadingTO = SelectTblLoadingTOByLoadingSlipId (loadingSlipId);
                tblLoadingTO.LoadingSlipList = _iTblLoadingSlipBL.SelectAllLoadingSlipListWithDetails (tblLoadingTO.IdLoading);

                //Aniket [30-7-2019] added for IOT
                if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_DELIVERED) {
                    int confiqId = _iTblConfigParamsDAO.IoTSetting ();
                    if (confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) ||
                        confiqId == Convert.ToInt32 (Constants.WeighingDataSourceE.BOTH)) {
                        _iIotCommunication.GetItemDataFromIotAndMerge (tblLoadingTO, false);
                        foreach (var item in tblLoadingTO.LoadingSlipList) {
                            if (item.IdLoadingSlip == loadingSlipId)
                                _iIotCommunication.GetItemDataFromIotForGivenLoadingSlip (item);
                        }
                    }
                }

                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            }

        }

        //Priyanka [25-07-2018]
        public TblLoadingTO SelectTblLoadingByLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId(loadingSlipId, conn, tran);

        }

        public List<VehicleNumber> SelectAllVehicles () {
            return _iTblLoadingDAO.SelectAllVehicles ();
        }
        public List<DropDownTO> SelectAllVehiclesByStatus (int statusId) {
            #region Get Vhical details from IoT 
            //Added By Kiran 12-12-18
            int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting ();
            if (weightSourceConfigId == Convert.ToInt32 (Constants.WeighingDataSourceE.IoT) && statusId != (int) Constants.TranStatusE.UNLOADING_NEW) {

                String statusIds = statusId.ToString ();
                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Loading List From DB Start ");
                int userId = 1; // temp set userId  NTD
                tblUserMachineMappingTo tblUserMachineMappingTo = SelectUserMachineTo (userId);
                int gateId = 0;
                if (tblUserMachineMappingTo != null && tblUserMachineMappingTo.UserId != 0 && tblUserMachineMappingTo.GateId != 0) {
                    gateId = tblUserMachineMappingTo.GateId;
                }
                List<TblLoadingTO> list = SelectAllLoadingListByStatus (Convert.ToString ((int) Constants.TranStatusE.LOADING_CONFIRM) + "," + Convert.ToString ((int) Constants.TranStatusE.LOADING_GATE_IN), gateId); // LOADING_IN_PROGRESS commented by aniket
                string finalStatusId = _iIotCommunication.GetIotEncodedStatusIdsForGivenStatus (statusIds);

                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Loading List From DB END ");

                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + finalStatusId + " From Gate IoT Start");

                List<TblLoadingTO> distGate = list.GroupBy (g => g.GateId).Select (s => s.FirstOrDefault ()).ToList ();

                GateIoTResult gateIoTResult = new GateIoTResult ();

                for (int g = 0; g < distGate.Count; g++) {
                    TblLoadingTO tblLoadingTOTemp = distGate[g];
                    TblGateTO tblGateTO = new TblGateTO (tblLoadingTOTemp.GateId, tblLoadingTOTemp.IoTUrl, tblLoadingTOTemp.MachineIP, tblLoadingTOTemp.PortNumber);
                    GateIoTResult temp = _iIotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId (finalStatusId, tblGateTO);

                    if (temp != null && temp.Data != null) {
                        gateIoTResult.Data.AddRange (temp.Data);
                    }
                }
                //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId(finalStatusId);

                // Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + finalStatusId + " From Gate IoT END ");

                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Mapping & Processing Start ");

                if (gateIoTResult != null && gateIoTResult.Data != null) {
                    List<DropDownTO> dropDownList = new List<DropDownTO> ();
                    for (int j = 0; j < gateIoTResult.Data.Count; j++) {
                        DropDownTO dropDownTo = new DropDownTO ();
                        var data = list.Where (w => w.ModbusRefId == Convert.ToInt32 (gateIoTResult.Data[j][0])).FirstOrDefault ();
                        if (data != null) {
                            dropDownTo.Value = data.IdLoading;
                            //  dropDownTo.Text =  Convert.ToString (gateIoTResult.Data[j][1]);
                            dropDownTo.Text = _iIotCommunication.GetVehicleNumbers(Convert.ToString(gateIoTResult.Data[j][1]), true);//chetan[11-feb-2020] added for select old vehicle number
                            dropDownList.Add (dropDownTo);
                        }
                    }
                    return dropDownList;
                }
                //Constants.writeLog("GetVehicleNumberListByStauts : For statusId - " + statusId + " Mapping & Processing END ");

            }
            #endregion
            List <DropDownTO> dropDownTOList = _iTblLoadingDAO.SelectAllVehiclesListByStatus(statusId);
            if (dropDownTOList != null && dropDownTOList.Count > 0)
                dropDownTOList = dropDownTOList.Where(w => w.Text != null).ToList();
            return dropDownTOList;
        }

        public tblUserMachineMappingTo SelectUserMachineTo (int userId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return _iTblLoadingDAO.SelectUserMachineTo (userId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }
        //Aniket [30-7-2019] added for IOT
        //public  tblUserMachineMappingTo SelectUserMachineTo(int userId)
        //{
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();
        //        return _iTblLoadingDAO.SelectUserMachineTo(userId, conn, tran);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
        public LoadingInfo SelectDashboardLoadingInfo (List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, DateTime sysDate, Int32 loadingType) {
            try {
                TblUserRoleTO tblUserRoleTO = new TblUserRoleTO ();
                //Boolean isPriorityOther = true;
                if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
                {
                    tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
                    // isPriorityOther = BL.TblUserRoleBL.selectRolePriorityForOther(tblUserRoleTOList);
                }
                return _iTblLoadingDAO.SelectDashboardLoadingInfo (tblUserRoleTO, orgId, sysDate, loadingType);
            } catch (Exception ex) {
                return null;
            }
        }

        //public List<TblLoadingTO> SelectAllLoadingListByVehicleNo (string vehicleNo, DateTime loadingDate) {
        //    return _iTblLoadingDAO.SelectAllLoadingListByVehicleNo (vehicleNo, loadingDate);
        //}

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading, int loadingId)//Aniket [13-6-2019] added loadingId paramater
        {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return _iTblLoadingDAO.SelectAllLoadingListByVehicleNo (vehicleNo, isAllowNxtLoading, loadingId, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading, int loadingId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, isAllowNxtLoading, loadingId, conn, tran);
        }

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut (string vehicleNo, SqlConnection conn, SqlTransaction tran) {
            return _iTblLoadingDAO.SelectAllLoadingListByVehicleNoForDelOut (vehicleNo, conn, tran);
        }

        public List<TblLoadingTO> SelectAllInLoadingListByVehicleNo (string vehicleNo) {
            return _iTblLoadingDAO.SelectAllInLoadingListByVehicleNo (vehicleNo);
        }

        public Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisorDCT (DateTime date) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return _iTblLoadingDAO.SelectCountOfLoadingsOfSuperwisor (date, conn, tran);
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
        }

        public List<TblLoadingTO> SelectAllTblLoading (int cnfId, String loadingStatusIdIn, DateTime loadingDate) {
            return _iTblLoadingDAO.SelectAllTblLoading (cnfId, loadingStatusIdIn, loadingDate);
        }

        // Vaibhav [08-Jan-2018] Added to select all temp loading details.
        public List<TblLoadingTO> SelectAllTempLoading (SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_MIGRATE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null) {
                resultMessage.DefaultBehaviour ("Error tblConfigParamsTO is null");
                return null;
            }

            DateTime statusDate = _iCommon.ServerDateTime.AddDays (-Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal));
            //DateTime statusDate = _iCommon.ServerDateTime.AddDays(-25);

            try {
                return _iTblLoadingDAO.SelectAllTempLoading (conn, tran, statusDate);
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "SelectAllTempLoading");
                return null;
            }
        }
        //Pandurang[2018-09-25] Added to select all temp loading on status details.
        public List<TblLoadingTO> SelectAllTempLoadingOnStatus (SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_DELETE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null) {
                resultMessage.DefaultBehaviour ("Error tblConfigParamsTO is null");
                return null;
            }

            DateTime statusDate = _iCommon.ServerDateTime.AddDays (-Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal));
            //DateTime statusDate = _iCommon.ServerDateTime.AddDays(-25);

            try {
                return _iTblLoadingDAO.SelectAllTempLoadingOnStatus (conn, tran, statusDate);
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "SelectAllTempLoading");
                return null;
            }
        }

        //Vijaymala [12-04-2018] added to get all loading list by vehicle number
        public List<TblLoadingTO> SelectLoadingListByVehicleNo (string vehicleNo) {
            return _iTblLoadingDAO.SelectLoadingListByVehicleNo (vehicleNo);
        }
        /// <summary>
        /// Vijaymala[24-04-2018] added to get loading details by using booking id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public List<TblLoadingTO> SelectAllTblLoadingByBookingId (Int32 bookingId) {
            #region Start
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO> ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                List<TblLoadingSlipDtlTO> tblLoadingSlipDtlTOList = _iTblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromBookingId (bookingId, conn, tran);
                if (tblLoadingSlipDtlTOList != null && tblLoadingSlipDtlTOList.Count > 0) {

                    for (int i = 0; i < tblLoadingSlipDtlTOList.Count; i++) {

                        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipDtlTOList[i];

                        TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails (tblLoadingSlipDtlTO.LoadingSlipId, conn, tran);

                        if (tblLoadingSlipTO != null) {

                            TblLoadingTO tblLoadingTO = SelectTblLoadingTO (tblLoadingSlipTO.LoadingId, conn, tran);

                            TblLoadingTO tblLoadingTOAlready = tblLoadingTOList.Where (w => w.IdLoading == tblLoadingTO.IdLoading).FirstOrDefault ();

                            if (tblLoadingTOAlready != null) {
                                if (tblLoadingTOAlready.LoadingSlipList == null) {
                                    tblLoadingTOAlready.LoadingSlipList = new List<TblLoadingSlipTO> ();
                                }
                                tblLoadingTOAlready.LoadingSlipList.Add (tblLoadingSlipTO);
                            } else {

                                if (tblLoadingTO.LoadingSlipList == null) {
                                    tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO> ();
                                }
                                tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTO);
                                tblLoadingTOList.Add (tblLoadingTO);
                            }

                        }

                    }
                }
                return tblLoadingTOList;
            } catch (Exception ex) {
                return null;
            } finally {
                conn.Close ();
            }
            #endregion

        }

        /// <summary>
        /// Vijaymala[19-09-2018] added to get loading slip list by using booking
        /// </summary>
        /// <param name="idBooking"></param>
        /// <returns></returns>

        public TblLoadingTO SelectLoadingTOWithDetailsByBooking(String tempBookingsIdsList, String tempScheduleIdsList)
        {
            try
            {
                TblLoadingTO tblLoadingTO = new TblLoadingTO();
                List<Int32> bookingsIdsList = new List<Int32>();
                List<Int32> scheduleIdsList = new List<Int32>();
                List<TblBookingExtTO> tblAllBookingExtTOList = new List<TblBookingExtTO>();
                //get bookingIds List
                if (!String.IsNullOrEmpty(tempBookingsIdsList))
                {
                    bookingsIdsList = tempBookingsIdsList.Split(',').Select(int.Parse).ToList();
                }
                //get ScheduleIds List
                if (!String.IsNullOrEmpty(tempScheduleIdsList))
                {
                    scheduleIdsList = tempScheduleIdsList.Split(',').Select(int.Parse).ToList();
                }
                TblBookingsTO tblBookingTO = new TblBookingsTO ();
                if (bookingsIdsList != null && bookingsIdsList.Count > 0) {

                    for (int s = 0; s < bookingsIdsList.Count; s++) {
                        Int32 bookingId = bookingsIdsList[s];
                        tblBookingTO = _iCircularDependencyBL.SelectBookingsTOWithDetails (bookingId);
                        if (tblBookingTO != null) {

                            tblBookingTO.PaymentTermOptionRelationTOLst = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByBookingId(bookingId);

                            tblLoadingTO.VehicleNo = tblBookingTO.VehicleNo;
                            tblLoadingTO.FreightAmt = tblBookingTO.FreightAmt;
                            tblLoadingTO.CnfOrgId = tblBookingTO.CnFOrgId;
                            tblLoadingTO.CnfOrgName = tblBookingTO.CnfName;

                            tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO>();
                            if (tblBookingTO.BookingScheduleTOLst != null && tblBookingTO.BookingScheduleTOLst.Count > 0)
                            {
                                List<TblBookingScheduleTO> temptblBookingScheduleTOList = tblBookingTO.BookingScheduleTOLst;
                                List<TblBookingScheduleTO> tblBookingScheduleTOList = new List<TblBookingScheduleTO> ();

                                //get schedule list
                                if (scheduleIdsList != null && scheduleIdsList.Count > 0)
                                {
                                    for (int p = 0; p < scheduleIdsList.Count; p++)
                                    {
                                        Int32 scheduleId = scheduleIdsList[p];
                                        TblBookingScheduleTO tempTblBookingScheduleTO = new TblBookingScheduleTO ();
                                        tempTblBookingScheduleTO = temptblBookingScheduleTOList.Where (c => c.IdSchedule == scheduleId).FirstOrDefault ();
                                        tblBookingScheduleTOList.Add (tempTblBookingScheduleTO);
                                    }
                                }
                                //get all extensionlist
                                if (tblBookingScheduleTOList != null && tblBookingScheduleTOList.Count > 0) {
                                    for (int i = 0; i < tblBookingScheduleTOList.Count; i++) {

                                        TblBookingScheduleTO tblBookingScheduleTO = tblBookingScheduleTOList[i];
                                        tblAllBookingExtTOList.AddRange (tblBookingScheduleTO.OrderDetailsLst);
                                    }
                                }
                                //get distinct schedule list
                                List<TblBookingScheduleTO> distinctBookingScheduleList = tblBookingScheduleTOList.GroupBy(w => w.LoadingLayerId).Select(x => x.FirstOrDefault()).ToList();
                                if (distinctBookingScheduleList != null && distinctBookingScheduleList.Count > 0)
                                {
                                    for (int m = 0; m < distinctBookingScheduleList.Count; m++)
                                    {
                                        TblBookingScheduleTO tempBookingScheduleTO = distinctBookingScheduleList[m];
                                        tempBookingScheduleTO.OrderDetailsLst = new List<TblBookingExtTO> ();
                                        List<TblBookingExtTO> distinctBookingExtList = tblAllBookingExtTOList.GroupBy (w => w.LoadingLayerId).Select (x => x.FirstOrDefault ()).ToList ();
                                        if (distinctBookingExtList != null && distinctBookingExtList.Count > 0) {
                                            for (int n = 0; n < distinctBookingExtList.Count; n++) {
                                                List<TblBookingExtTO> tempOrderList = tblAllBookingExtTOList.Where (oi => oi.LoadingLayerId == distinctBookingExtList[n].LoadingLayerId).ToList ();
                                                for (int k = 0; k < tempOrderList.Count; k++) {
                                                    TblBookingExtTO tempBookingExtTO = tempOrderList[k];
                                                    if (tempBookingScheduleTO.LoadingLayerId == tempBookingExtTO.LoadingLayerId) {
                                                        tempBookingScheduleTO.OrderDetailsLst.Add (tempBookingExtTO);
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    Double bookQty = tblBookingTO.PendingQty;


                                    for (int p = 0; p < distinctBookingScheduleList.Count; p++)
                                    {
                                        TblBookingScheduleTO distBookingScheduleTO = distinctBookingScheduleList[p];
                                        var listToCheck = distBookingScheduleTO.OrderDetailsLst.GroupBy(a => new { a.ProdSpecId, a.ProdCatId, a.MaterialId, a.ProdItemId, a.BrandId, a.ProdCatDesc, a.ProdSpecDesc, a.MaterialSubType, a.BrandDesc, a.DisplayName }).
                                            Select(a => new { ProdCatId = a.Key.ProdCatId, ProdItemId = a.Key.ProdItemId, ProdSpecId = a.Key.ProdSpecId, BrandId = a.Key.BrandId, MaterialId = a.Key.MaterialId, ProdCatDesc = a.Key.ProdCatDesc,
                                                ProdSpecDesc = a.Key.ProdSpecDesc, BrandDesc = a.Key.BrandDesc, MaterialSubType = a.Key.MaterialSubType,
                                                DisplayName = a.Key.DisplayName, BalanceQty = a.Sum(acs => acs.BalanceQty) }).ToList();

                                        distBookingScheduleTO.OrderDetailsLst = new List<TblBookingExtTO> ();
                                        for (int l = 0; l < listToCheck.Count; l++) {
                                            var listTo = listToCheck[l];
                                            TblBookingExtTO tblBookingExtTO = new TblBookingExtTO ();
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

                                        TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO (tblBookingTO);

                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;
                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingRate = tblBookingTO.BookingRate;
                                        tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
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
                                                tblLoadingSlipAddressTO.Pincode = tblBookingDelAddrTO.Pincode.ToString ();
                                                tblLoadingSlipAddressTO.TxnAddrTypeId = tblBookingDelAddrTO.TxnAddrTypeId;

                                                //Saket [2019-09-27] From pending booking auto loading slip addres src should be booking.
                                                //tblLoadingSlipAddressTO.AddrSourceTypeId = tblBookingDelAddrTO.AddrSourceTypeId;
                                                tblLoadingSlipAddressTO.AddrSourceTypeId = (int)Constants.AddressSourceTypeE.FROM_BOOKINGS;

                                                tblLoadingSlipAddressTO.LoadingLayerId = tblBookingDelAddrTO.LoadingLayerId;
                                                tblLoadingSlipAddressTO.BillingOrgId = tblBookingDelAddrTO.BillingOrgId;

                                                //tblLoadingSlipAddressTO.Country = tblBookingDelAddrTO.Country;
                                                tblLoadingSlipTO.DeliveryAddressTOList.Add (tblLoadingSlipAddressTO);
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
                                                tblLoadingSlipExtTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
                                                tblLoadingSlipTO.LoadingSlipExtTOList.Add (tblLoadingSlipExtTO);
                                            }
                                            if (bookQty >= totLayerQty) {
                                                bookQty = bookQty - totLayerQty;

                                            }
                                            else
                                            {
                                                totLayerQty = bookQty;
                                                bookQty = 0;
                                            }
                                            tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = totLayerQty;


                                        } else {
                                            //tblLoadingSlipTO.NoOfDeliveries = 1;
                                            TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO ();
                                            tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                            tblLoadingSlipExtTO.LoadingLayerid = (int) Constants.LoadingLayerE.BOTTOM;
                                            tblLoadingSlipTO.LoadingSlipExtTOList.Add (tblLoadingSlipExtTO);

                                        }
                                        tblLoadingTO.NoOfDeliveries = distinctBookingScheduleList.Count;
                                        tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTO);
                                    }

                                }
                                else
                                {
                                    TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO(tblBookingTO);
                                    tblLoadingTO.NoOfDeliveries = 1;
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO();
                                    tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;

                                    tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
                                    tblLoadingSlipExtTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;

                                    tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                    tblLoadingSlipExtTO.LoadingLayerid = (int)Constants.LoadingLayerE.BOTTOM;
                                    tblLoadingSlipTO.LoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                                    tblLoadingTO.LoadingSlipList.Add(tblLoadingSlipTO);

                                }

                            } else {
                                TblLoadingSlipTO tblLoadingSlipTO = selectLoadingSlipTO (tblBookingTO);
                                tblLoadingTO.NoOfDeliveries = 1;
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = new TblLoadingSlipExtTO ();
                                tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = tblBookingTO.IdBooking;

                                tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
                                tblLoadingSlipExtTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;

                                tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty = tblBookingTO.PendingQty;
                                tblLoadingSlipExtTO.LoadingLayerid = (int) Constants.LoadingLayerE.BOTTOM;
                                tblLoadingSlipTO.LoadingSlipExtTOList.Add (tblLoadingSlipExtTO);
                                tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTO);

                            }
                        }
                    }
                }
                return tblLoadingTO;
            } catch (Exception ex) {
                return null;
            }

        }

        private TblLoadingSlipTO selectLoadingSlipTO (TblBookingsTO tblBookingTO) {
            TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO ();
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
            tblLoadingSlipTO.FreightAmt = tblBookingTO.FreightAmt;
            if (tblBookingTO.OrcMeasure == "Fix" && tblBookingTO.OrcAmt > 0)  //Need to change Rs/MT
            {
                tblLoadingSlipTO.OrcAmt = Math.Round(tblBookingTO.OrcAmt / tblBookingTO.BookingQty);
                tblLoadingSlipTO.OrcMeasure = "Rs/MT";
            }
            else
            {
                tblLoadingSlipTO.OrcAmt = tblBookingTO.OrcAmt;
                tblLoadingSlipTO.OrcMeasure = tblBookingTO.OrcMeasure;
            }
            //tblLoadingSlipTO.OrcAmt = tblBookingTO.OrcAmt;
            //tblLoadingSlipTO.OrcMeasure = tblBookingTO.OrcMeasure;
            tblLoadingSlipTO.ORCPersonName = tblBookingTO.ORCPersonName;
            tblLoadingSlipTO.Comment = tblBookingTO.Comments;
            tblLoadingSlipTO.TblLoadingSlipDtlTO = new TblLoadingSlipDtlTO ();
            tblLoadingSlipTO.DeliveryAddressTOList = new List<TblLoadingSlipAddressTO> ();
            tblLoadingSlipTO.LoadingSlipExtTOList = new List<TblLoadingSlipExtTO> ();
            tblLoadingSlipTO.PaymentTermOptionRelationTOLst = tblBookingTO.PaymentTermOptionRelationTOLst;
            tblLoadingSlipTO.BookingDisplayNo = tblBookingTO.BookingDisplayNo;
            return tblLoadingSlipTO;
        }

        //public ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm, Int32 invGenModeId, String taxInvoiceNumber = "", Int32 manualinvoiceno = 0)
        public ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm, Int32 invGenModeId, int fromOrgId, int toOrgId, String taxInvoiceNumber = "", Int32 manualinvoiceno = 0, String invComment = "")
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = _iCommon.ServerDateTime;

            string entityRangeString = "REGULAR_TAX_INVOICE_";
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                //Saket [2020-03-26]
                lock (generateInvoiceNoLock)
                {
                    int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();

                    TblInvoiceTO invoiceTO = _iTblInvoiceBL.SelectTblInvoiceTOWithDetails(invoiceId, conn, tran);
                    //TblInvoiceTO invoiceTO = _iTblInvoiceDAO.SelectTblInvoice(invoiceId, conn, tran);
                    if (invoiceTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("invoiceTO Found NULL"); return resultMessage;
                    }

                    invoiceTO.InvComment = invComment;

                    //Vijaymala[23-03-2016]added to check invoice details of igst,cgst,sgst taxes
                    #region To check invoice details is valid or not
                    string errorMsg = string.Empty;
                    Boolean isValidInvoice = _iTblInvoiceBL.CheckInvoiceDetailsAccToState(invoiceTO, ref errorMsg);
                    if (!isValidInvoice)
                    {
                        resultMessage.DefaultBehaviour(errorMsg);
                        return resultMessage;
                    }
                    #endregion

                    if (string.IsNullOrEmpty(invoiceTO.InvoiceNo) || invoiceTO.InvoiceNo == "0")
                    {

                        if (invGenModeId != (int)Constants.InvoiceGenerateModeE.REGULAR)
                        {
                            TblInvoiceChangeOrgHistoryTO changeHisTO = new TblInvoiceChangeOrgHistoryTO();
                            resultMessage = _iTblInvoiceBL.PrepareAndSaveInternalTaxInvoices(invoiceTO, invGenModeId, fromOrgId, toOrgId, 0, changeHisTO, conn, tran);
                            if (resultMessage.MessageType == ResultMessageE.Information)
                            {
                                tran.Commit();
                                resultMessage.Text = "Invoice Converted Successfully";
                                resultMessage.DisplayMessage = "Invoice Converted Successfully";
                                return resultMessage;
                            }
                            else
                            {
                                tran.Rollback();
                                return resultMessage;
                            }
                        }
                        //if (string.IsNullOrEmpty(invoiceTO.ElectronicRefNo))
                        //{
                        //    tran.Rollback();
                        //    resultMessage.DefaultBehaviour("Can Not Continue.EWay Bill No is Not Assign.");
                        //    resultMessage.DisplayMessage = "Not Allowed.EWay Bill No is Not Updated.";
                        //    return resultMessage;
                        //}
                        // Ramdas.W @ 28102017 : chenge InvoiceStatus for new Invoice number genarate  AUTHORIZED And status not conform  
                        if (invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED && isconfirm == 0)
                        {
                            invoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
                        }

                        if (invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION
                            || invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.CANCELLED
                            || invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED
                            )
                        {


                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Can Not Continue.INvoice Status Is -" + invoiceTO.StatusName);
                            resultMessage.DisplayMessage = "Not Allowed.Invoice is :" + invoiceTO.StatusName;
                            return resultMessage;

                        }
                        invoiceTO.StatusDate = serverDate;
                        invoiceTO.UpdatedBy = loginUserId;
                        invoiceTO.UpdatedOn = serverDate;
                        invoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.AUTHORIZED;


                        #region Invoice Authorization Date As Invoice Date
                        //Saket [2018-6-04] Added
                        Int32 invoiceAuthDateAsInvoiceDate = 0;
                        TblConfigParamsTO invoiceAuthDateAsInvoiceDateConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_AUTHORIZATION_DATE_AS_INV_DATE, conn, tran);
                        if (invoiceAuthDateAsInvoiceDateConfigTO != null)
                        {
                            invoiceAuthDateAsInvoiceDate = Convert.ToInt32(invoiceAuthDateAsInvoiceDateConfigTO.ConfigParamVal);
                        }

                        if (invoiceAuthDateAsInvoiceDate == 1)
                        {
                            invoiceTO.InvoiceDate = serverDate;
                        }
                        #endregion

                        Int32 generateNCInvoice = 0;
                        TblConfigParamsTO generateNCInvoiceTblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_GENERATE_INVOICE_NO_FOR_NC, conn, tran);
                        if (generateNCInvoiceTblConfigParamsTO != null)
                        {
                            generateNCInvoice = Convert.ToInt32(generateNCInvoiceTblConfigParamsTO.ConfigParamVal);
                        }



                        if (invoiceTO.IsConfirmed == 1 || generateNCInvoice == 1)
                        {


                            //Added By hrushikesh for default org id 09/10/2019
                            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                            // Aniket [05-02-2019] check manual branswise invoice no generate or not
                            TblConfigParamsTO tblConfigParamsTOForBrand = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.GENERATE_MANUALLY_BRANDWISE_INVOICENO, conn, tran);
                            if (tblConfigParamsTO == null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                                return resultMessage;
                            }

                            Int32 brandWiseInvoiceSetting = Convert.ToInt32(tblConfigParamsTOForBrand.ConfigParamVal);

                            Int32 defualtOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                            TblEntityRangeTO entityRangeTO = null;
                            //if (Convert.ToInt32(tblConfigParamsTOForBrand.ConfigParamVal) == 1)
                            //{
                            //    DimBrandTO dimBrandTO = _iDimBrandDAO.SelectDimBrand(invoiceTO.BrandId);
                            //    entityRangeString += dimBrandTO.IdBrand.ToString();
                            //    entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityRangeString, invoiceTO.FinYearId, conn, tran);
                            //}
                            ////Hrushikesh added get entity Range organizationwise
                            //else if (invoiceTO.InvFromOrgId != defualtOrgId)
                            //{
                            //    string orgstr = Constants.ENTITY_RANGE_REGULAR_TAX_INTERNALORG + invoiceTO.InvFromOrgId;
                            //    entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(orgstr, invoiceTO.FinYearId, conn, tran);
                            //}
                            //else
                            //    entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(invoiceTO.InvoiceTypeId, invoiceTO.FinYearId, conn, tran);


                            String entityName = _iDimensionDAO.SelectInvoiceEntityNameByInvoiceTypeId(invoiceTO.InvoiceTypeId);
                            if (String.IsNullOrEmpty(entityName))
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("entityRangeTO Found NULL. Entity Range Not Defined"); return resultMessage;
                            }
                            if (invoiceTO.InvFromOrgId != defualtOrgId)   //For NC invoice this condition will false.
                            {
                                entityName += "_ORG_" + invoiceTO.InvFromOrgId;
                            }

                            if (brandWiseInvoiceSetting == 1)
                            {
                                entityName += "_BRAND_" + invoiceTO.BrandId;
                            }
                            if (invoiceTO.IsConfirmed == 0)
                            {
                                entityName += "_NC";

                                Int32 generateNCInvoiceDaily = 0;
                                TblConfigParamsTO generateNCInvoiceDailyTblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_GENERATE_INVOICE_NO_FOR_NC_DAILY, conn, tran);
                                if (generateNCInvoiceDailyTblConfigParamsTO != null)
                                {
                                    generateNCInvoiceDaily = Convert.ToInt32(generateNCInvoiceDailyTblConfigParamsTO.ConfigParamVal);
                                }
                                if (generateNCInvoiceDaily == 1)
                                {
                                    entityRangeTO = SelectEntityRangeForLoadingCount(entityName, conn, tran, invoiceTO.FinYearId, 0);
                                }

                            }

                            entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName, invoiceTO.FinYearId, conn, tran);

                            if (entityRangeTO == null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("entityRangeTO Found NULL. Entity Range Not Defined"); return resultMessage;
                            }
                            Int32 entityPrevVal = 0;
                            bool isInvoicePresent = false;
                            int newTaxNumber = 0;
                            //Aniket [17-jan-2019] to check invoice number should generate manually or automatically
                            if (String.IsNullOrEmpty(taxInvoiceNumber))
                            {

                                entityPrevVal = entityRangeTO.EntityPrevValue;
                                entityPrevVal++;
                                if (String.IsNullOrEmpty(entityRangeTO.Suffix))
                                {
                                    invoiceTO.InvoiceNo = entityRangeTO.Prefix + entityPrevVal.ToString();
                                }
                                else
                                {
                                    invoiceTO.InvoiceNo = entityRangeTO.Prefix + entityPrevVal.ToString() + entityRangeTO.Suffix;
                                }

                            }
                            else
                            {

                                TblInvoiceTO tblInvoiceTO = _iTblInvoiceDAO.SelectAllTblInvoice(taxInvoiceNumber, invoiceTO.FinYearId);
                                // List<TblInvoiceTO> tempList = list.Where(x => x.FinYearId == invoiceTO.FinYearId).ToList();
                                // List<TblInvoiceTO> existInvoiceList = new List<TblInvoiceTO>();
                                // if (tempList != null && tempList.Count > 0)
                                //  {
                                //   existInvoiceList = tempList.Where(ele => ele.InvoiceNo == taxInvoiceNumber).ToList();
                                if (!String.IsNullOrEmpty(tblInvoiceTO.InvoiceNo))
                                {

                                    isInvoicePresent = true;
                                    resultMessage.DefaultBehaviour(taxInvoiceNumber + " Invoice number has been already generated against Id  " + tblInvoiceTO.IdInvoice);
                                    return resultMessage;

                                }
                                if (String.IsNullOrEmpty(entityRangeTO.Suffix))
                                {
                                    invoiceTO.InvoiceNo = taxInvoiceNumber;
                                }
                                else
                                {
                                    invoiceTO.InvoiceNo = taxInvoiceNumber + entityRangeTO.Suffix;
                                }


                                newTaxNumber = manualinvoiceno;
                                //  }
                            }

                            result = _iTblInvoiceBL.UpdateTblInvoice(invoiceTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Updating Invoice Number After Entity Range"); return resultMessage;
                            }
                            if (String.IsNullOrEmpty(taxInvoiceNumber))
                            {
                                entityRangeTO.EntityPrevValue = entityPrevVal;
                            }
                            else
                            {
                                entityRangeTO.EntityPrevValue = newTaxNumber;
                            }
                            result = _iTblEntityRangeDAO.UpdateTblEntityRange(entityRangeTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While UpdateTblEntityRange"); return resultMessage;
                            }
                        }

                        else
                        {
                            result = _iTblInvoiceBL.UpdateTblInvoice(invoiceTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Error While Updating Invoice Number After Entity Range"); return resultMessage;
                            }
                        }


                        //Generate inv history record
                        TblInvoiceHistoryTO invHistoryTO = new TblInvoiceHistoryTO();
                        invHistoryTO.InvoiceId = invoiceTO.IdInvoice;
                        invHistoryTO.CreatedOn = serverDate;
                        invHistoryTO.CreatedBy = loginUserId;
                        invHistoryTO.StatusDate = serverDate;
                        invHistoryTO.StatusId = (int)Constants.InvoiceStatusE.AUTHORIZED;
                        invHistoryTO.StatusRemark = "Invoice Authorized With Inv No :" + invoiceTO.InvoiceNo;
                        result = _iTblInvoiceHistoryDAO.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblInvoiceHistory"); return resultMessage;
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Invoice No is already Generated");
                        resultMessage.DisplayMessage = "Invoice No #" + invoiceTO.InvoiceNo + " is already Generated";
                        return resultMessage;
                    }


                    if (invoiceTO.InvoiceModeId != Convert.ToInt32(Constants.InvoiceModeE.MANUAL_INVOICE))
                    {
                        Boolean changeStatusOnGate = false;

                        Int32 count = 0;
                        TblLoadingSlipTO tblLoadingSlipTOselect = _iTblLoadingSlipDAO.SelectTblLoadingSlip(invoiceTO.LoadingSlipId, conn, tran);
                        if (tblLoadingSlipTOselect == null)
                        {
                            tran.Rollback();
                            resultMessage.Text = "";
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                        List<TblLoadingSlipTO> list = _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(tblLoadingSlipTOselect.LoadingId, conn, tran);
                        TblLoadingTO tblLoadingTO = new TblLoadingTO();
                        if (list == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("LoadingSlip Found NULL"); return resultMessage;
                        }
                        else
                        {
                            for (int i = 0; i < list.Count; i++)
                            {

                                List<TblInvoiceTO> invoiceTOselectList = _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(list[i].IdLoadingSlip, conn, tran);

                                if (invoiceTOselectList != null && invoiceTOselectList.Count > 0)
                                {
                                    List<TblInvoiceTO> TblInvoiceTOTemp = invoiceTOselectList.Where(w => w.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED).ToList();

                                    //if (TblInvoiceTOTemp == null || TblInvoiceTOTemp.Count == 0)
                                    if (invoiceTOselectList != null && invoiceTOselectList.Count == TblInvoiceTOTemp.Count)
                                    {
                                        count++;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            //int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
                            if (list.Count == count)
                            {
                                tblLoadingTO = SelectLoadingTOWithDetails(tblLoadingSlipTOselect.LoadingId);
                                if (weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.IoT)
                                {
                                    // tblLoadingTO = TblLoadingBL.SelectTblLoadingTO(tblLoadingSlipTOselect.LoadingId, conn, tran);
                                    if (tblLoadingTO == null || tblLoadingTO.VehicleNo == null || tblLoadingTO.TransporterOrgId == 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.DefaultBehaviour("tblLoadingTO Found NULL"); return resultMessage;
                                    }
                                }
                                tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH);
                                tblLoadingTO.StatusReason = "Invoice Generated and ready for dispach";
                                TblConfigParamsTO tblConfigParamsTOWeighing = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SKIP_WEIGHING, conn, tran);
                                if (tblConfigParamsTOWeighing != null)
                                {
                                    if (Convert.ToInt32(tblConfigParamsTOWeighing.ConfigParamVal) == 1)
                                    {
                                        tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_DELIVERED);
                                        tblLoadingTO.StatusReason = "Delivered";
                                    }
                                }
                                tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                                tblLoadingTO.IdLoading = tblLoadingSlipTOselect.LoadingId;
                                invoiceTO.VehicleNo = tblLoadingTO.VehicleNo;

                                if (weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.IoT)
                                {
                                    //tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_IN_PROGRESS);
                                    tblLoadingTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_CONFIRM);
                                    tblLoadingTO.StatusReason = "Loading Confirmed";
                                }
                                TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);

                                if (weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.BOTH)
                                {

                                    if (configParamsTO != null)
                                    {
                                        if (Convert.ToInt32(configParamsTO.ConfigParamVal) == 1)
                                        {
                                            changeStatusOnGate = true;
                                        }
                                    }
                                }
                                else
                                {
                                    result = _iTblLoadingSlipDAO.UpdateTblLoadingById(tblLoadingTO, conn, tran);
                                    if (result <= 0)
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error While UpdateTblLoading In Method UpdateStatusForLoading";
                                        return resultMessage;
                                    }
                                }

                                resultMessage = RightDataFromIotToDB(tblLoadingTO.IdLoading, tblLoadingTO, conn, tran);
                                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While Writng Data from DB";
                                    return resultMessage;
                                }
                            }
                        }

                        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SPLIT_BOOKING_AGAINST_INVOICE, conn, tran);
                        if (tblConfigParamsTO != null)
                        {
                            if (tblConfigParamsTO.ConfigParamVal == "1")
                            {
                                resultMessage = SpiltBookingAgainstInvoice(invoiceTO, tblLoadingTO, conn, tran);
                                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                {
                                    return resultMessage;
                                }
                            }
                        }



                        #region Write Data to Invoice


                        if (weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.IoT)
                        {
                            if (invoiceTO.IsConfirmed == 1)
                            {
                                //invoiceTO = _iTblInvoiceBL.SelectTblInvoiceTOWithDetails(invoiceTO.IdInvoice, conn, tran);

                                _iTblInvoiceBL.SetGateAndWeightIotData(invoiceTO, 0);

                                if (invoiceTO == null || invoiceTO.VehicleNo == null || invoiceTO.TransportOrgId == 0)
                                {
                                    tran.Rollback();
                                    resultMessage.DefaultBehaviour("invoiceTO Found NULL OR VehicleNo Found NULL OR TransportOrgId Found NULL when write Data to Invoice"); return resultMessage;
                                }

                                var invoiceItemList = invoiceTO.InvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId > 0).ToList();
                                if (invoiceItemList != null && invoiceItemList.Count > 0)
                                {
                                    for (int s = 0; s < invoiceItemList.Count; s++)
                                    {
                                        if (invoiceItemList[s].InvoiceQty <= 0)
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour("Invoice Item Qty found zero when write Data to Invoice");
                                            return resultMessage;
                                        }
                                    }
                                }


                                Int32 statusId = invoiceTO.StatusId;

                                invoiceTO.StatusId = (Int32)Constants.InvoiceStatusE.NEW;

                                // TblInvoiceBL.SetGateAndWeightIotData(invoiceTO);

                                invoiceTO.StatusId = statusId;

                                result = _iTblInvoiceBL.UpdateTblInvoice(invoiceTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error While updating tblInvoiceTO";
                                    return resultMessage;
                                }

                                for (int p = 0; p < invoiceTO.InvoiceItemDetailsTOList.Count; p++)
                                {
                                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceTO.InvoiceItemDetailsTOList[p];

                                    result = _iTblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error While updating tblInvoiceItemTO";
                                        return resultMessage;
                                    }

                                }

                            }

                        }

                        #endregion


                        #region Write Invoice Generate & Ready For dispatch on Device

                        if (changeStatusOnGate)
                        {
                            DimStatusTO statusTO = _iDimStatusDAO.SelectDimStatus(Convert.ToInt16(Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH), conn, tran);
                            if (statusTO == null || statusTO.IotStatusId == 0)
                            {
                                resultMessage.DefaultBehaviour("iot status id not found for loading to pass at gate iot");
                                return resultMessage;
                            }

                            if (tblLoadingTO == null || tblLoadingTO.ModbusRefId == 0)
                            {
                                resultMessage.DefaultBehaviour("ModbusRefId == 0 while marking status invoice generated and ready for dispatch");
                                return resultMessage;
                            }

                            object[] statusframeTO = new object[2] { tblLoadingTO.ModbusRefId, statusTO.IotStatusId };
                            result = _iIotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi(tblLoadingTO, statusframeTO);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error while PostGateAPIDataToModbusTcpApi");
                                return resultMessage;
                            }
                        }

                        #endregion
                    }
                    #region 4. Save the invoice data to SAP
                    if (invoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED)
                    {
                        TblConfigParamsTO tblConfigParams = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_POST_SALES_INVOICE_TO_SAP_DIRECTLY_AFTER_INVOICE_GENERATION);
                        if (tblConfigParams != null)
                        {
                            if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                            {
                                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                                if (tblConfigParamsTOSAPService != null)
                                {
                                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                                    {
                                        if (invoiceTO != null)
                                        {
                                            for (int i = 0; i < invoiceTO.InvoiceItemDetailsTOList.Count; i++)
                                            {
                                                if (invoiceTO.InvoiceItemDetailsTOList[i].Bundles == null)
                                                {
                                                    invoiceTO.InvoiceItemDetailsTOList[i].Bundles = "0";
                                                }
                                            }
                                        }
                                        resultMessage = JsonConvert.DeserializeObject<ResultMessage>(_iCommon.PostSalesInvoiceToSAP(invoiceTO));
                                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                                        {
                                            tran.Rollback();
                                            return resultMessage;
                                        }
                                        TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(resultMessage.data.ToString());
                                        result = _iTblInvoiceBL.UpdateMappedSAPInvoiceNo(tblInvoiceTO, conn, tran);
                                        if (result != 1)
                                        {
                                            tran.Rollback();
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.Text = "Error While updating tblInvoiceTO";
                                            return resultMessage;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    if (invoiceTO.IsConfirmed == 1)
                        resultMessage.DisplayMessage = "Success..Invoice authorized and #" + invoiceTO.InvoiceNo + " is generated";
                    else
                        resultMessage.DisplayMessage = "Success..DC authorized and #" + invoiceTO.InvoiceNo + " is generated";
                    resultMessage.Tag = invoiceTO;
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "GenerateInvoiceNumber");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResultMessage RightDataFromIotToDB(Int32 loadingId, TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
                if (weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.IoT)
                {
                    if (loadingId == 0)
                    {
                        throw new Exception("loadingId == 0");
                    }

                    //TblLoadingTO tblLoadingTO = SelectLoadingTOWithDetails(loadingId, conn, tran);
                    if (tblLoadingTO == null)
                    {
                        throw new Exception("tblLoadingTO == null");
                    }
                    if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                    {
                        throw new Exception("tblLoadingTO.LoadingSlipList == 0");
                    }

                    //Move Confirm Data.
                    //tblLoadingTO.LoadingSlipList = tblLoadingTO.LoadingSlipList.Where(w => w.IsConfirmed == 1).ToList();
                    List<TblLoadingSlipTO> loadingSlipListConfirm = tblLoadingTO.LoadingSlipList.Where(w => w.IsConfirmed == 1).ToList();


                    if (loadingSlipListConfirm.Count > 0)
                    {


                        if (String.IsNullOrEmpty(tblLoadingTO.VehicleNo) || tblLoadingTO.TransporterOrgId == 0)
                        {
                            throw new Exception("tblLoadingTO Found NULL");
                        }

                        //Write DATA

                        Int32 result = UpdateTblLoading(tblLoadingTO, conn, tran);
                        if (result != 1)
                        {
                            throw new Exception("Error While updating Loading status for loadingId - " + tblLoadingTO.IdLoading);
                        }


                        if (tblLoadingTO.LoadingStatusHistoryTOList != null && tblLoadingTO.LoadingStatusHistoryTOList.Count > 0)
                        {
                            for (int t = 0; t < tblLoadingTO.LoadingStatusHistoryTOList.Count; t++)
                            {
                                TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = tblLoadingTO.LoadingStatusHistoryTOList[t];
                                tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                //tblLoadingStatusHistoryTO.StatusDate = CommonDAO.SelectServerDateTime();
                                tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.StatusDate;
                                result = _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                                if (result != 1)
                                {
                                    throw new Exception("Error While inserting status history record  for index " + t + " against loadingId - " + tblLoadingTO.IdLoading);

                                }

                            }
                        }


                        for (int j = 0; j < loadingSlipListConfirm.Count; j++)
                        {
                            TblLoadingSlipTO tblLoadingSlipTO = loadingSlipListConfirm[j];
                            tblLoadingSlipTO.StatusId = Convert.ToInt16(Constants.TranStatusE.LOADING_CONFIRM);
                            tblLoadingSlipTO.StatusReason = "Loading Confirmed";
                            result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                            if (result != 1)
                            {
                                throw new Exception("Error While updating LoadingSlip status For loadingslipId - " + tblLoadingSlipTO.IdLoadingSlip);
                            }


                            for (int k = 0; k < tblLoadingSlipTO.LoadingSlipExtTOList.Count; k++)
                            {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[k];

                                if (tblLoadingSlipExtTO.LoadedWeight <= 0)
                                {
                                    throw new Exception("Loading Wt found zero for ext id IdLoadingSlipExt - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                                }

                                Int32 tempResult = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipExtTO, conn, tran);
                                if (tempResult != 1)
                                {
                                    throw new Exception("Error While updating LoadingSlip Ext status for Ext Id - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                                }
                            }


                        }
                    }

                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        public ResultMessage SpiltBookingAgainstInvoice(TblInvoiceTO tblInvoiceTO, TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                Int32 result = 0;
                if (tblInvoiceTO == null)
                {
                    throw new Exception("InvoiceTO is NULL");
                }

                //if (tblInvoiceTO.IsConfirmed != 0)
                //{
                //    resultMessage.DefaultSuccessBehaviour();
                //    resultMessage.DisplayMessage = "Invoice No - " + tblInvoiceTO.IdInvoice + " Is Confirm";
                //    resultMessage.Text = resultMessage.DisplayMessage;
                //    return resultMessage;
                //}

                if (tblInvoiceTO.LoadingSlipId <= 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }

                //Check if every invoice against loading is confirm
                List<TblInvoiceTO> loadingInvoiceList = _iTblInvoiceBL.SelectInvoiceListFromLoadingSlipId(tblInvoiceTO.LoadingSlipId, conn, tran);
                if (loadingInvoiceList == null || loadingInvoiceList.Count == 0)
                {
                    throw new Exception("Invoice not found against loading slip Id - " + tblInvoiceTO.LoadingSlipId);
                }
                for (int i = 0; i < loadingInvoiceList.Count; i++)
                {
                    if (loadingInvoiceList[i].InvoiceStatusE != Constants.InvoiceStatusE.AUTHORIZED)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                }

                //if (tblLoadingTO.LoadingSlipList == null || tblLoadingTO.LoadingSlipList.Count == 0)
                //{
                //    throw new Exception("LoadingSlipList is null for LoadingSlipId");
                //}
                //TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList.Where(w => w.IdLoadingSlip == tblInvoiceTO.LoadingSlipId).FirstOrDefault();
                
                TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipDAO.SelectTblLoadingSlip(tblInvoiceTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    throw new Exception("LoadingSlipList is null for LoadingSlipId");
                }
                tblLoadingSlipTO.TblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(tblLoadingSlipTO.IdLoadingSlip, conn, tran);

                //AmolG[2020-Feb-25] use existing list. If the list found empty then get data from the database.
                if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0
                    && tblLoadingTO.LoadingSlipList[0].LoadingSlipExtTOList != null && tblLoadingTO.LoadingSlipList[0].LoadingSlipExtTOList.Count > 0)
                    tblLoadingSlipTO.LoadingSlipExtTOList = tblLoadingTO.LoadingSlipList[0].LoadingSlipExtTOList;
                else
                    tblLoadingSlipTO.LoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExt(tblLoadingSlipTO.IdLoadingSlip, conn, tran);

                //tblLoadingSlipTO.LoadingSlipExtTOList = BL.TblLoadingSlipExtBL.SelectAllTblLoadingSlipExtList(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                //tblLoadingSlipTO.DeliveryAddressTOList = BL.TblLoadingSlipAddressBL.SelectAllTblLoadingSlipAddressList(tblLoadingSlipTO.IdLoadingSlip, conn, tran);



                //TblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblInvoiceTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null)
                {
                    throw new Exception("tblLoadingSlipTO is null for LoadingSlipId - " + tblInvoiceTO.LoadingSlipId);
                }


                if (tblLoadingTO.LoadingTypeE == Constants.LoadingTypeE.OTHER)
                {
                    if (tblLoadingSlipTO.TblLoadingSlipDtlTO == null)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                }

                if (tblLoadingSlipTO.TblLoadingSlipDtlTO == null)
                {
                    throw new Exception("TblLoadingSlipDtlTO is null for LoadingSlipId - " + tblInvoiceTO.LoadingSlipId);
                }

                Int32 currentBookingId = tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId;

                if (currentBookingId == 0)
                {
                    throw new Exception("Booking Id is zero for LoadingSlipId - " + tblInvoiceTO.LoadingSlipId);
                }

                TblBookingsTO tblBookingsTO = _iCircularDependencyBL.SelectBookingsTOWithDetails(currentBookingId);
                if (tblBookingsTO == null)
                {
                    throw new Exception("tblBookingsTO == null for bookingId" + currentBookingId);
                }

                if (tblInvoiceTO.IsConfirmed == tblBookingsTO.IsConfirmed)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Booking & Invoice are - " + tblInvoiceTO.IsConfirmed;
                    return resultMessage;
                }

                //if (tblBookingsTO.IsConfirmed == 0)
                //{
                //    resultMessage.DefaultSuccessBehaviour();
                //    resultMessage.DisplayMessage = "Booking is already tentative";
                //    return resultMessage;
                //}

                Double splitQty = tblLoadingSlipTO.TblLoadingSlipDtlTO.LoadingQty;
                #region Update Current Booking

                if (splitQty == tblBookingsTO.BookingQty)
                {
                    if (tblBookingsTO.IsConfirmed == 0)
                        tblBookingsTO.IsConfirmed = 1;
                    else
                        tblBookingsTO.IsConfirmed = 0;


                    result = _iTblBookingsDAO.UpdateTblBookings(tblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        throw new Exception("Error while updating tblBookingsTO for bookingId - " + tblBookingsTO.IdBooking);
                    }

                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;

                }
                else
                {
                    tblBookingsTO.BookingQty -= splitQty;
                }
                result = _iTblBookingsDAO.UpdateTblBookings(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while updating tblBookingsTO for bookingId - " + tblBookingsTO.IdBooking);
                }

                //AmolG[2020-Feb-25] if the booking ext list is empty then get it from database
                if (tblBookingsTO.OrderDetailsLst == null || tblBookingsTO.OrderDetailsLst.Count == 0)
                    tblBookingsTO.OrderDetailsLst = _iTblBookingExtDAO.SelectAllTblBookingExt(tblBookingsTO.IdBooking, conn, tran);

                if (tblBookingsTO.BookingScheduleTOLst == null || tblBookingsTO.BookingScheduleTOLst.Count == 0)
                {
                    tblBookingsTO.BookingScheduleTOLst = _iTblBookingScheduleDAO.SelectAllTblBookingScheduleList(tblBookingsTO.IdBooking, conn, tran);
                }
                
                if (tblBookingsTO.OrderDetailsLst != null && tblBookingsTO.OrderDetailsLst.Count > 0)
                {

                    Double previousBookingItemQty = tblBookingsTO.OrderDetailsLst.Sum(s => s.BookedQty);

                    Double diffQty = previousBookingItemQty - tblBookingsTO.BookingQty;
                    if (diffQty > 0)
                    {
                        //Delete Item which are in loading slip.
                        for (int i = 0; i < tblLoadingSlipTO.LoadingSlipExtTOList.Count; i++)
                        {
                            if (diffQty <= 0)
                            {
                                break;
                            }

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[i];

                            Double adjustedQty = tblLoadingSlipExtTO.LoadingQty;
                            Double adjustedBookQty = tblLoadingSlipExtTO.LoadingQty;

                            List<TblBookingExtTO> tblBookingExtTOList = tblBookingsTO.OrderDetailsLst.Where(w => w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                                            w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                                            w.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList();

                            if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                            {
                                for (int j = 0; j < tblBookingExtTOList.Count; j++)
                                {

                                    if (adjustedQty <= 0 || diffQty <= 0)
                                    {
                                        break;
                                    }

                                    TblBookingExtTO tblBookingExtTO = tblBookingExtTOList[j];
                                    if (tblBookingExtTO.BookedQty >= adjustedQty)
                                    {
                                        diffQty -= adjustedQty;

                                        tblBookingExtTO.BookedQty -= adjustedQty;
                                        
                                        //AmolG[2020-Feb-25] Here upadte the schedule qty
                                        UpdateBookingQty(ref adjustedBookQty, tblBookingsTO, tblBookingExtTO);

                                        adjustedQty = 0;
                                    }
                                    else
                                    {
                                        diffQty -= tblBookingExtTO.BookedQty;

                                        adjustedQty -= tblBookingExtTO.BookedQty;
                                        tblBookingExtTO.BookedQty = 0;
                                        //AmolG[2020-Feb-25] Here upadte the schedule qty
                                        UpdateBookingQty(ref adjustedBookQty, tblBookingsTO, tblBookingExtTO);
                                    }

                                    result = _iTblBookingExtDAO.UpdateTblBookingExt(tblBookingExtTO, conn, tran);
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

                                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                                {
                                    for (int n = 0; n < tblBookingsTO.BookingScheduleTOLst.Count; n++)
                                    {
                                        if (tblBookingsTO.BookingScheduleTOLst[n].IsUpdated)
                                        {

                                            tblBookingsTO.BookingScheduleTOLst[n].Qty = Math.Round(tblBookingsTO.BookingScheduleTOLst[n].Qty, 3);

                                            tblBookingsTO.BookingScheduleTOLst[n].IsUpdated = false;
                                            result = _iTblBookingScheduleDAO.UpdateTblBookingSchedulePendingQty(tblBookingsTO.BookingScheduleTOLst[n], conn, tran);
                                            if (result != 1)
                                            {
                                                tran.Rollback();
                                                resultMessage.Text = "Sorry..Record Could not be saved. Error While Update Booking qty in Schedule table in Function UpdateTblBookingSchedule";
                                                resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                                                resultMessage.Result = 0;
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                return resultMessage;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //AmolG[2020-Mar-03] Update Size Qty. This is used to show the color on UI based on Schedule Qty of booking. 
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    double sizeQty = tblBookingsTO.BookingScheduleTOLst.Sum(s => s.Qty);
                    tblBookingsTO.SizesQty = sizeQty;
                    result = _iTblBookingsDAO.UpdateSizeQuantity(tblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.Text = "Error When Update Size Qty in Booking";
                        resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                        resultMessage.Result = 0;
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                }

                #endregion

                #region New Booking

                tblBookingsTO.BookingRefId = Convert.ToInt32(tblBookingsTO.BookingDisplayNo);
                tblBookingsTO.PendingQty = 0;
                tblBookingsTO.BookingQty = splitQty;
                tblBookingsTO.SizesQty = splitQty; // AmolG[2020-Mar-03] This is used to show the color on UI based on Schedule Qty of booking. 
                tblBookingsTO.IdBooking = 0;
                List<TblBookingsTO> List = _iTblBookingsDAO.SelectTblBookingsRef(tblBookingsTO.BookingRefId, conn, tran);
                tblBookingsTO.BookingDisplayNo = List != null && List.Count > 0 ? tblBookingsTO.BookingDisplayNo + "/" + (Convert.ToInt32(List.Count) + Convert.ToInt32(1)) : tblBookingsTO.BookingDisplayNo + "/1";
                if (tblBookingsTO.IsConfirmed == 0)
                {
                    tblBookingsTO.IsConfirmed = 1;
                }
                else
                {
                    tblBookingsTO.IsConfirmed = 0;
                }

                result = _iTblBookingsDAO.InsertTblBookings(tblBookingsTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while inserting tblBookingsTO for refBookingId- " + tblBookingsTO.BookingRefId);
                }

                Int32 newBookingId = tblBookingsTO.IdBooking;

                //AmolG[2020-Mar-03] Save new Schedule Entry for new booking
                TblBookingScheduleTO tblBookingScheduleTO = null;
                if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
                {
                    tblBookingScheduleTO = tblBookingsTO.BookingScheduleTOLst[0];
                    tblBookingScheduleTO.BookingId = newBookingId;
                    tblBookingScheduleTO.Qty = tblBookingsTO.BookingQty;
                }
                else
                {
                    tblBookingScheduleTO = new TblBookingScheduleTO();
                    tblBookingScheduleTO.BookingId = newBookingId;
                    tblBookingScheduleTO.ScheduleDate = tblBookingsTO.CreatedOn;
                    tblBookingScheduleTO.CreatedBy = tblBookingsTO.CreatedBy;
                    tblBookingScheduleTO.CreatedOn = tblBookingsTO.CreatedOn;
                    tblBookingScheduleTO.Qty = tblBookingsTO.BookingQty;
                }

                result = _iTblBookingScheduleDAO.InsertTblBookingSchedule(tblBookingScheduleTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while inserting InsertTblBookingSchedule for BookingId - " + tblBookingScheduleTO.BookingId);
                }


                if (tblBookingsTO.DeliveryAddressLst != null && tblBookingsTO.DeliveryAddressLst.Count > 0)
                {
                    for (int i = 0; i < tblBookingsTO.DeliveryAddressLst.Count; i++)
                    {
                        if (string.IsNullOrEmpty(tblBookingsTO.DeliveryAddressLst[i].Country))
                            tblBookingsTO.DeliveryAddressLst[i].Country = Constants.DefaultCountry;

                        tblBookingsTO.DeliveryAddressLst[i].BookingId = newBookingId;
                        result = _iTblBookingDelAddrDAO.InsertTblBookingDelAddr(tblBookingsTO.DeliveryAddressLst[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Error While Inserting Booking Del Address in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }

                if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingSlipTO.LoadingSlipExtTOList.Count; i++)
                    {
                        TblBookingExtTO tblBookingExtTO = new TblBookingExtTO(tblLoadingSlipTO.LoadingSlipExtTOList[i]);
                        tblBookingExtTO.BookingId = newBookingId;
                        tblBookingExtTO.Rate = tblBookingsTO.BookingRate;

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
                if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                {
                    for (int i = 0; i < tblLoadingSlipTO.LoadingSlipExtTOList.Count; i++)
                    {
                        tblLoadingSlipTO.LoadingSlipExtTOList[i].BookingId = newBookingId;

                        int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
                        if (weightSourceConfigId == (Int32)Constants.WeighingDataSourceE.IoT)
                        {
                            if (tblLoadingSlipTO.IsConfirmed == 0)
                            {
                                tblLoadingSlipTO.LoadingSlipExtTOList[i].LoadedBundles = 0;
                                tblLoadingSlipTO.LoadingSlipExtTOList[i].LoadedWeight = 0;
                                tblLoadingSlipTO.LoadingSlipExtTOList[i].CalcTareWeight = 0;
                            }
                        }

                        result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(tblLoadingSlipTO.LoadingSlipExtTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.Text = "Sorry..Record Could not be saved. Error While InsertTblLoadingSlipExt in Function SaveNewBooking";
                            resultMessage.DisplayMessage = "Sorry..Record Could not be saved.";
                            resultMessage.Result = 0;
                            resultMessage.MessageType = ResultMessageE.Error;
                            return resultMessage;
                        }
                    }
                }
                #endregion

                tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId = newBookingId;

                result = _iTblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl(tblLoadingSlipTO.TblLoadingSlipDtlTO, conn, tran);
                if (result != 1)
                {
                    throw new Exception("Error while updating tblLoadingSlipTO.TblLoadingSlipDtlTO for IdLoadSlipDtl - " + tblLoadingSlipTO.TblLoadingSlipDtlTO.IdLoadSlipDtl);
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        /// <summary>
        /// AmolG[2020-Feb-25]Update Booking Qty
        /// </summary>
        /// <param name="adjustedBookQty"></param>
        /// <param name="tblBookingsTO"></param>
        /// <param name="tblBookingExtTO"></param>
        public void UpdateBookingQty(ref Double adjustedBookQty, TblBookingsTO tblBookingsTO, TblBookingExtTO tblBookingExtTO)
        {
            //AmolG[2020-Feb-25] Here upadte the schedule qty
            if (tblBookingsTO.BookingScheduleTOLst != null && tblBookingsTO.BookingScheduleTOLst.Count > 0)
            {
                for (int n = 0; n < tblBookingsTO.BookingScheduleTOLst.Count; n++)
                {
                    if (tblBookingsTO.BookingScheduleTOLst[n].IdSchedule == tblBookingExtTO.ScheduleId
                        && tblBookingsTO.BookingScheduleTOLst[n].Qty >= adjustedBookQty)
                    {
                        tblBookingsTO.BookingScheduleTOLst[n].Qty -= adjustedBookQty;

                        tblBookingsTO.BookingScheduleTOLst[n].Qty = Math.Round(tblBookingsTO.BookingScheduleTOLst[n].Qty, 3);

                        tblBookingsTO.BookingScheduleTOLst[n].IsUpdated = true;
                        adjustedBookQty = 0;
                    }
                    else if(tblBookingsTO.BookingScheduleTOLst[n].IdSchedule == tblBookingExtTO.ScheduleId)
                    {
                        adjustedBookQty = tblBookingsTO.BookingScheduleTOLst[n].Qty - adjustedBookQty;
                        tblBookingsTO.BookingScheduleTOLst[n].Qty = 0;
                        tblBookingsTO.BookingScheduleTOLst[n].IsUpdated = true;
                    }
                }
            }
        }

        #endregion

        #region Insertion
        public int InsertTblLoading (TblLoadingTO tblLoadingTO) {
            return _iTblLoadingDAO.InsertTblLoading (tblLoadingTO);
        }

        public int InsertTblLoading (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            return _iTblLoadingDAO.InsertTblLoading (tblLoadingTO, conn, tran);
        }

        public ResultMessage IsLoadingShouldMerge(TblLoadingTO tblLoadingTO)
        {
            ResultMessage resultMessage = new ResultMessage();

            if (tblLoadingTO != null)
            {
                ResultMessage r = IsThisVehicleDelivered(tblLoadingTO.VehicleNo, 1);
                if (r.Tag != null && r.Tag.GetType() == typeof(TblLoadingTO))
                {
                    //tblLoadingTO = ((TblLoadingTO)r.Tag);
                    tblLoadingTO.MergeLoadingId = ((TblLoadingTO)r.Tag).IdLoading;
                    tblLoadingTO.MergeMessage = r.DisplayMessage;
                }

                return r;
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage IsThisVehicleDelivered(String vehicleNo, Int32 checkOnDevice = 0)
        {
            ResultMessage resultMessage = new ResultMessage();

            if (String.IsNullOrEmpty(vehicleNo))
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Vehicle No not found";
                resultMessage.DisplayMessage = "Vehicle No not found";
                resultMessage.Result = 1;
                return resultMessage;
            }

            int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
           
            List<TblLoadingTO> list = SelectAllLoadingListByVehicleNo(vehicleNo, true, 0);
            if (list == null || list.Count == 0)
            {
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Allowed , All Loadings Are Delivered";
                resultMessage.DisplayMessage = "";
                resultMessage.Result = 1;
            }
            else
            {
                var lastObj = list.OrderByDescending(s => s.StatusDate).FirstOrDefault();
                if (lastObj != null && lastObj.IsAllowNxtLoading == 1)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Allowed ,  next Loadings is Allowed";
                    resultMessage.DisplayMessage = "";
                    resultMessage.Result = 1;
                    return resultMessage;

                }
                resultMessage.Result = 0;
                //resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Not Allowed , Selected Vehicle :" + vehicleNo + " is not delivered . Last status is " + lastObj.StatusDesc;
                resultMessage.DisplayMessage = "Selected Vehicle: " + vehicleNo + " is not delivered . Last status is " + lastObj.StatusDesc +
                                               " Current loading slip will merge with existing slip No - " + lastObj.LoadingSlipNo;
                resultMessage.Result = 1;
                resultMessage.Tag = lastObj;
            }
            

            if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
            {
                if (checkOnDevice == 1)
                {

                    List<TblGateTO> tblGateTOList = _iTblGateBL.SelectAllTblGateList(Constants.ActiveSelectionTypeE.Active);

                    GateIoTResult gateIoTResult = new GateIoTResult();

                    if (tblGateTOList != null && tblGateTOList.Count > 0)
                    {
                        tblGateTOList = tblGateTOList.Where(W => W.ModuleId == Constants.DefaultModuleID).ToList();

                        for (int g = 0; g < tblGateTOList.Count; g++)
                        {
                            //TblLoadingTO tblLoadingTOTemp = distGate[g];
                            TblGateTO tblGateTO = tblGateTOList[g];
                            GateIoTResult temp = _iIotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId("101", tblGateTO);
                            if (temp != null && temp.Data != null)
                            {
                                gateIoTResult.Data.AddRange(temp.Data);
                            }

                            temp = _iIotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId("102", tblGateTO);
                            if (temp != null && temp.Data != null)
                            {
                                gateIoTResult.Data.AddRange(temp.Data);
                            }


                        }
                    }
                    if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count > 0)
                    {
                        for (int i = 0; i < gateIoTResult.Data.Count; i++)
                        {
                            if (gateIoTResult.Data[i] != null)
                            {
                                Int32 modBusLoadingRefId = Convert.ToInt32(gateIoTResult.Data[i][(int)IoTConstants.GateIoTColE.LoadingId]);

                                String iotVehicleNo = gateIoTResult.Data[i][(int)IoTConstants.GateIoTColE.VehicleNo].ToString();
                                iotVehicleNo = _iIotCommunication.GetVehicleNumbers(iotVehicleNo, true);
                                vehicleNo = _iIotCommunication.GetVehicleNumbers(vehicleNo, true);
                                if (iotVehicleNo.ToUpper() == vehicleNo.ToUpper())
                                {
                                    TblLoadingTO tblLoadingTO = SelectTblLoadingTOByModBusRefId(modBusLoadingRefId);

                                    if (tblLoadingTO == null)
                                    {
                                        resultMessage.DefaultBehaviour("tblLoadingTO Found null against modBusRefId - " + modBusLoadingRefId);
                                        return resultMessage;
                                    }

                                    Int32 iotStatusId = Convert.ToInt32(gateIoTResult.Data[i][(int)IoTConstants.GateIoTColE.StatusId]);
                                    DimStatusTO dimStatusTO = _iDimStatusBL.SelectDimStatusTOByIotStatusId(iotStatusId, (Int32)StaticStuff.Constants.TransactionTypeE.LOADING);
                                    if (dimStatusTO != null)
                                    {
                                        tblLoadingTO.StatusDesc = dimStatusTO.StatusName;
                                    }

                                    resultMessage.MessageType = ResultMessageE.Information;
                                    resultMessage.Text = "Not Allowed , Selected Vehicle :" + vehicleNo + " is not delivered . Last status is " + tblLoadingTO.StatusDesc;
                                    resultMessage.DisplayMessage = "Selected Vehicle: " + vehicleNo + " is not delivered . Last status is " + tblLoadingTO.StatusDesc +
                                                                   " Current loading slip will merge with existing slip No - " + tblLoadingTO.LoadingSlipNo;
                                    resultMessage.Result = 1;
                                    resultMessage.Tag = tblLoadingTO;
                                    return resultMessage;
                                }
                            }
                        }
                    }

                }
            }

            return resultMessage;
        }

        public ResultMessage CalculateLoadingValuesRate(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
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

                #region Commented code Vijaymala added[26-04-2018]

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

                #endregion


                //Sanjay [2018-07-04] Tax Calculations Inclusive Of Taxes Or Exclusive Of Taxes. Reported From Customer Shivangi Rolling Mills.By default it will be 0 i.e. Tax Exclusive
                //Int32 isTaxInclusiveWithTaxes = 0;
                Int32 isTaxInclusiveWithAllParam = 0; // Less CD,parity,ORC while reverse GSTIN Calculation
                Boolean isSez = false;
                //TblConfigParamsTO rateCalcConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_RATE_CALCULATIONS_TAX_INCLUSIVE, conn, tran);
                //if (rateCalcConfigParamsTO != null)
                //{
                //    isTaxInclusiveWithTaxes = Convert.ToInt32(rateCalcConfigParamsTO.ConfigParamVal);
                //}

                TblConfigParamsTO rateCalcTaxInclConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_RATE_CALCULATIONS_TAX_INCLUSIVE_WITH_ALL_PARAMS_LESS, conn, tran);
                if (rateCalcTaxInclConfigParamsTO != null)
                {
                    isTaxInclusiveWithAllParam = Convert.ToInt32(rateCalcTaxInclConfigParamsTO.ConfigParamVal);
                }

                List<TblConfigParamsTO> tblConfigParamsTOList = _iTblConfigParamsBL.SelectAllTblConfigParamsList();
                Boolean isRateRounded = false;
                Int32 dontShowCdOnInvoice = 0;

                TblConfigParamsTO roundRateConfig = new TblConfigParamsTO();
                if (tblConfigParamsTOList != null && tblConfigParamsTOList.Count > 0)
                {
                    roundRateConfig = tblConfigParamsTOList.Where(ele => ele.ConfigParamName == Constants.CP_IS_INVOICE_RATE_ROUNDED).FirstOrDefault();
                    if (roundRateConfig != null && Convert.ToInt32(roundRateConfig.ConfigParamVal) == 1)
                    {
                        isRateRounded = true;
                    }


                    TblConfigParamsTO temp = tblConfigParamsTOList.Where(ele => ele.ConfigParamName == Constants.CP_DO_NOT_SHOW_CD_ON_INOVICE).FirstOrDefault();
                    if (temp != null)
                    {
                        dontShowCdOnInvoice = Convert.ToInt32(temp.ConfigParamVal);
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

                        //Prajakta [2020-08-17]
                        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = tblLoadingSlipTO.TblLoadingSlipDtlTO;
                        if (tblLoadingSlipDtlTO != null && tblLoadingSlipDtlTO.IdBooking > 0)
                        {
                            tblLoadingSlipDtlTO.BookingId = tblLoadingSlipDtlTO.IdBooking;

                        }

                        //if (tblLoadingTO.LoadingType == (int)Constants.LoadingTypeE.OTHER)
                        //{

                        //}
                        if(tblLoadingSlipDtlTO == null || tblLoadingSlipDtlTO.BookingId == 0)
                        { }
                        else
                        {


                            TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(tblLoadingSlipDtlTO.BookingId, conn, tran);
                            if (tblBookingsTO == null)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }

                            if (tblBookingsTO.IsSez == 1)
                            {
                                isSez = true;
                            }

                            String parityIds = String.Empty;
                            List<TblBookingParitiesTO> tblBookingParitiesTOList = _iTblBookingParitiesDAO.SelectTblBookingParitiesByBookingId(tblBookingsTO.IdBooking, conn, tran);


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
                            // List<TblParityDetailsTO> parityDetailsTOList = BL._iTblParityDetailsBL.SelectAllTblParityDetailsList(parityIds, 0, conn, tran);

                            for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++)
                            {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];

                                Int32 isTaxInclusive = 0;
                                DimBrandTO dimBrandTO = _iDimBrandDAO.SelectDimBrand(tblLoadingSlipExtTO.BrandId);

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
                                    Double bookingPrice;
                                    List<TblBookingExtTO> bookingExtTOList = _iTblBookingExtDAO.SelectAllTblBookingExt(tblBookingParitiesTO.BookingId);
                                    //Aniket [24-9-2019]
                                    TblGlobalRateTO rateTO = null;
                                    TblGroupItemTO tblGroupItemTO = _iTblGroupItemDAO.SelectTblGroupItemDetails(tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.MaterialId);
                                    if (tblGroupItemTO != null)
                                    {
                                        rateTO = new TblGlobalRateTO();
                                        Dictionary<Int32, Int32> rateDCT = _iTblGlobalRateDAO.SelectLatestGroupAndRateDCT(tblBookingsTO.CreatedOn.ToString("yyyy-MM-dd"));
                                        if (rateDCT != null)
                                        {
                                            if (rateDCT.ContainsKey(tblGroupItemTO.GroupId))
                                            {
                                                Int32 rateID = rateDCT[tblGroupItemTO.GroupId];
                                                rateTO = _iTblGlobalRateDAO.SelectTblGlobalRate(rateID);
                                            }
                                        }
                                    }
                                    if (rateTO != null)
                                        bookingPrice = rateTO.Rate;
                                    else
                                        bookingPrice = tblBookingParitiesTO.BookingRate;
                                    // Aniket [18-6-2019]
                                    // added to reduce item wise discount from bookingprice
                                    if (tblBookingsTO.IsItemized == 1)
                                    {
                                        if (bookingExtTOList != null && bookingExtTOList.Count > 0)
                                        {
                                            foreach (TblBookingExtTO item in bookingExtTOList)
                                            {
                                                if (item.ProdCatId == tblLoadingSlipExtTO.ProdCatId && item.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && item.BrandId == tblLoadingSlipExtTO.BrandId && item.MaterialId == tblLoadingSlipExtTO.MaterialId && item.ProdItemId == tblLoadingSlipExtTO.ProdItemId)
                                                {
                                                    bookingPrice = bookingPrice - (item.Discount * item.BookedQty * 1000);
                                                }
                                            }
                                        }
                                    }

                                    TblGstCodeDtlsTO gstCodeDtlsTO = _iTblGstCodeDtlsDAO.SelectGstCodeDtlsTO(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdItemId, conn, tran);
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

                                    // rateCalcDesc = "B.R : " + tblBookingParitiesTO.BookingRate + "|";
                                    //if (isTaxInclusive == 1 && isTaxInclusiveWithTaxes == 0)
                                    if (isTaxInclusive == 1 && isTaxInclusiveWithAllParam == 0)
                                    {
                                        //bookingPrice = bookingPrice / 1.18;

                                        Double divisor = 100 + gstCodeDtlsTO.TaxPct;

                                        divisor = divisor / 100;

                                        bookingPrice = bookingPrice / divisor;
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
                                        TblAddressTO addrTO = _iTblAddressDAO.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                                        if (addrTO == null)
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour("Organization Office Address Details Not Found");
                                            return resultMessage;
                                        }
                                        //02-12-2020 Dhananjay added start
                                        Int32 districtId = 0;
                                        Int32 talukaId = 0;
                                        Int32 parityLevel = 1;
                                        TblConfigParamsTO parityLevelConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_PARITY_LEVEL, conn, tran);

                                        if (parityLevelConfigParamsTO != null)
                                        {
                                            parityLevel = Convert.ToInt32(parityLevelConfigParamsTO.ConfigParamVal);
                                            if(parityLevel == 2)
                                            {
                                                districtId = addrTO.DistrictId;
                                            }
                                            else if (parityLevel == 3)
                                            {
                                                districtId = addrTO.DistrictId;
                                                talukaId = addrTO.TalukaId;
                                            }
                                        }
                                        //02-12-2020 Dhananjay added end
                                        parityDtlTO = _iTblParityDetailsBL.GetParityDetailToOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, addrTO.StateId, tblBookingsTO.BookingDatetime, districtId, talukaId, parityLevel); //29-12-2020 Dhananjay commented parityDtlTO = _iTblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, addrTO.StateId, tblBookingsTO.BookingDatetime, districtId, talukaId);//02-12-2020 Dhananjay added districtId and talukaId
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

                                    }

                                    Double cdApplicableAmt = 0;
                                    cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                                    //if (tblLoadingSlipTO.IsConfirmed == 1)
                                    //    cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt;

                                    if (tblLoadingSlipTO.IsConfirmed == 1)
                                        cdApplicableAmt += parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;


                                    //if (isTaxInclusiveWithAllParam == 1)
                                    //{
                                    //    Double divisor = 100 + gstCodeDtlsTO.TaxPct;

                                    //    divisor = divisor / 100;

                                    //    cdApplicableAmt = cdApplicableAmt / divisor;
                                    //    cdApplicableAmt = Math.Round(cdApplicableAmt, 2);
                                    //}


                                    Double cdAmt = 0;
                                    Double orgCdAmt = 0;
                                    //Vijaymala added[22-06-2018]
                                    DropDownTO dropDownTO = _iDimensionDAO.SelectCDDropDown(tblLoadingSlipTO.CdStructureId);


                                    if (tblLoadingSlipTO.CdStructure >= 0)
                                    {
                                        //Priyanka [23-07-2018] Added if cdstructure is 0
                                        Int32 isRsValue = Convert.ToInt32(dropDownTO.Text);
                                        if (isRsValue == (int)Constants.CdType.IsRs)
                                        {

                                            orgCdAmt = cdAmt = tblLoadingSlipTO.CdStructure;
                                            cdAmt = cdAmt + tblLoadingSlipTO.AddDiscAmt;        //Priyanka [09-07-18]
                                        }
                                        else
                                        {

                                            orgCdAmt = cdAmt = (cdApplicableAmt * tblLoadingSlipTO.CdStructure) / 100;
                                            cdAmt = cdAmt + tblLoadingSlipTO.AddDiscAmt;        //Priyanka [09-07-18]
                                        }
                                    }


                                    rateCalcDesc += "CD :" + Math.Round(cdAmt, 2) + "|";
                                    Double basicRateTaxIncl = cdApplicableAmt - cdAmt + freightPerMT;
                                    //Double basicRateTaxIncl = cdApplicableAmt   + freightPerMT;
                                    Double rateAfterCD = cdApplicableAmt - cdAmt;
                                    //Double rateAfterCD = cdApplicableAmt;// - cdAmt;

                                    Double gstApplicableAmt = 0;
                                    Double gstAmt = 0;
                                    Double finalRate = 0;

                                    //if (isTaxInclusiveWithTaxes == 0 || isTaxInclusive == 0)
                                    if (isTaxInclusive == 0)
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

                                        //Saket [2020-07-10] Parmeshwar  Can be convert this setting all params.
                                        if (isTaxInclusiveWithAllParam == 0)
                                        {
                                            if (isSez)
                                            {
                                                gstCodeDtlsTO.TaxPct = 0;
                                            }
                                            Double taxToDivide = 100 + gstCodeDtlsTO.TaxPct;

                                            Double temp = ((basicRateTaxIncl * taxToDivide) / 100);

                                            gstAmt = basicRateTaxIncl - temp;
                                            gstAmt = Math.Round(Math.Abs(gstAmt), 2);
                                            //gstApplicableAmt = basicRateTaxIncl - gstAmt;
                                            gstApplicableAmt = basicRateTaxIncl;

                                            //finalRate = basicRateTaxIncl;
                                            cdApplicableAmt = gstApplicableAmt + cdAmt;
                                            finalRate = gstApplicableAmt + gstAmt;
                                        }
                                        //Rate Calculation for A1 Ispaat- All param will less from declared rate expect Freight
                                        else if (isTaxInclusiveWithAllParam == 1)
                                        {
                                            if (isSez)
                                            {
                                                gstCodeDtlsTO.TaxPct = 0;
                                            }
                                            Double taxToDivide = 100 + gstCodeDtlsTO.TaxPct;

                                            double reverseGstBasicAmt = (bookingPrice - cdAmt - orcAmtPerTon + parityAmt + priceSetOff + bvcAmt) + freightPerMT;
                                            gstAmt = reverseGstBasicAmt - ((reverseGstBasicAmt / taxToDivide) * 100);
                                            gstAmt = Math.Round(gstAmt, 2);
                                            gstApplicableAmt = reverseGstBasicAmt - gstAmt;
                                            finalRate = reverseGstBasicAmt;
                                            cdApplicableAmt = gstApplicableAmt + cdAmt;

                                            if (dontShowCdOnInvoice == 1)  //For A1
                                            {
                                                cdApplicableAmt = gstApplicableAmt;
                                            }
                                        }

                                    }




                                    tblLoadingSlipExtTO.TaxableRateMT = gstApplicableAmt;
                                    tblLoadingSlipExtTO.RatePerMT = finalRate;
                                    if (isRateRounded)
                                    {
                                        cdApplicableAmt = Math.Round(cdApplicableAmt);
                                    }
                                    tblLoadingSlipExtTO.CdApplicableAmt = cdApplicableAmt;
                                    //tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityTO.ExpenseAmt + parityTO.OtherAmt; Sudhir[23-MARCH-2018] Commented
                                    tblLoadingSlipExtTO.FreExpOtherAmt = freightPerMT + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;

                                    TblConfigParamsTO tblConfigParamsTempTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_HIDE_NOT_CONFIRM_OPTION);

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

        public void CalculateActualPriceInclusiveOfTaxes () {

        }

        /// <summary>
        /// Priyanka [11-05-2018] : Added for convert NC to C loading slip.
        /// </summary>
        /// <returns></returns>
        public ResultMessage UpdateNCToCLoadingSlip (Int32 loginUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage ();
            String erroMsg = String.Empty;
            DateTime txnDateTime = _iCommon.ServerDateTime;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                List<TblLoadingTO> tblLoadingTOList = SelectAllTblLoadingListForConvertNCToC ();
                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0) {
                    for (int i = 0; i < tblLoadingTOList.Count; i++) {
                        TblLoadingTO tblLoadingTO = tblLoadingTOList[i];
                        List<TblLoadingSlipTO> tblLoadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails (tblLoadingTO.IdLoading, conn, tran);
                        if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0) {
                            List<TblLoadingSlipTO> tblNCLoadingSlipTOList = tblLoadingSlipTOList.Where (t => t.IsConfirmed == 0).ToList ();
                            if (tblNCLoadingSlipTOList != null && tblNCLoadingSlipTOList.Count > 0) {
                                //If loading slip in confirm then change the status.
                                for (int t = 0; t < tblNCLoadingSlipTOList.Count; t++) {
                                    TblLoadingSlipTO tblLoadingSlipTO = tblNCLoadingSlipTOList[t];
                                    List<TblInvoiceTO> tblInvoiceTOList = _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId (tblLoadingSlipTO.IdLoadingSlip);

                                    if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0) {
                                        List<TblInvoiceTO> tblInvoiceTOListInvoiceNoNull = tblInvoiceTOList.Where (e => e.InvoiceNo == null && e.IsConfirmed == 0).ToList ();

                                        if (tblInvoiceTOListInvoiceNoNull != null && tblInvoiceTOListInvoiceNoNull.Count > 0) {
                                            for (int p = 0; p < tblInvoiceTOListInvoiceNoNull.Count; p++) {
                                                TblInvoiceTO tblInvoiceTONew = tblInvoiceTOListInvoiceNoNull[p];
                                                if (tblInvoiceTONew.IsConfirmed == 0) {
                                                    tblInvoiceTONew.UpdatedBy = Convert.ToInt32 (loginUserId);
                                                    tblInvoiceTONew.UpdatedOn = txnDateTime;
                                                    tblInvoiceTONew.IsConfirmed = 1;
                                                    resultMessage = UpdateInvoiceConfrimNonConfirmDetails (tblInvoiceTONew, loginUserId);
                                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                                                        erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                                    }
                                                }
                                            }
                                        } else {
                                            resultMessage = ChangeLoadingSlipConfirmationStatus (tblLoadingSlipTO, loginUserId);
                                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                                                erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                            }
                                        }
                                    } else {
                                        resultMessage = ChangeLoadingSlipConfirmationStatus (tblLoadingSlipTO, loginUserId);
                                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                                            erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                        }
                                    }
                                }
                            } else {
                                // If loading slip is confirm and its invoice is not confirm then change the status.
                                for (int t = 0; t < tblLoadingSlipTOList.Count; t++) {
                                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingSlipTOList[t];
                                    List<TblInvoiceTO> tblInvoiceTOList = _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId (tblLoadingSlipTO.IdLoadingSlip);
                                    if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0) {
                                        List<TblInvoiceTO> tblInvoiceTOListInvoiceNoNull = tblInvoiceTOList.Where (e => e.InvoiceNo == null && e.IsConfirmed == 0).ToList ();

                                        if (tblInvoiceTOListInvoiceNoNull != null && tblInvoiceTOListInvoiceNoNull.Count > 0) {
                                            for (int p = 0; p < tblInvoiceTOListInvoiceNoNull.Count; p++) {

                                                TblInvoiceTO tblInvoiceTONew = tblInvoiceTOListInvoiceNoNull[p];
                                                if (tblInvoiceTONew.IsConfirmed == 0) {
                                                    tblInvoiceTONew.UpdatedBy = Convert.ToInt32 (loginUserId);
                                                    tblInvoiceTONew.UpdatedOn = txnDateTime;
                                                    tblInvoiceTONew.IsConfirmed = 1;
                                                    resultMessage = UpdateInvoiceConfrimNonConfirmDetails (tblInvoiceTONew, loginUserId);
                                                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                                                        erroMsg += " Veh No - " + tblLoadingSlipTO.VehicleNo + " LoadingSlipNo - " + tblLoadingSlipTO.LoadingSlipNo + ", ";
                                                    }
                                                }

                                            }
                                        } else { }
                                    }
                                }
                            }

                        }

                    }

                    resultMessage.DefaultSuccessBehaviour ();

                    if (!String.IsNullOrEmpty (erroMsg)) {
                        erroMsg = erroMsg.TrimEnd (',');
                        resultMessage.DisplayMessage += "\n Error While Updating - " + erroMsg;

                    }

                    return resultMessage;
                } else {
                    resultMessage.DefaultBehaviour ("tblLoadingTOList Found NULL");
                    return resultMessage;
                }

            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateNCToCLoadingSlip");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage CheckMergeCondition(TblLoadingTO tblLoadingTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            TblLoadingTO tblLoadingTOTemp = new TblLoadingTO();
            tblLoadingTOTemp.VehicleNo = tblLoadingTO.VehicleNo;

            resultMessage = IsLoadingShouldMerge(tblLoadingTOTemp);
            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
            {
                return resultMessage;
            }
            TblLoadingTO mergeToLoaingTO = null;
            if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(TblLoadingTO))
            {
                mergeToLoaingTO = (TblLoadingTO)resultMessage.Tag;
            }

            if (tblLoadingTOTemp.MergeLoadingId > 0)
            {
                if (tblLoadingTO.MergeLoadingId == 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Current Loading slip will merge with existing please try again.";
                    resultMessage.DisplayMessage = resultMessage.Text;
                    return resultMessage;
                }
                if (tblLoadingTO.MergeLoadingId != tblLoadingTOTemp.MergeLoadingId)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Given Loading slip not match with current existing merge loading slip";
                    resultMessage.DisplayMessage = resultMessage.Text;
                    return resultMessage;
                }

                resultMessage = MergerLoadingToExisting(tblLoadingTO);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Loading Slip Merge to existing- " + mergeToLoaingTO.LoadingSlipNo;
                    resultMessage.Text = resultMessage.DisplayMessage;
                    resultMessage.Result = 2;
                    return resultMessage; ;
                }


            }


            resultMessage = new StaticStuff.ResultMessage();
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;

        }

        public ResultMessage MergerLoadingToExisting(TblLoadingTO tblLoadingTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();

                TblLoadingTO exitingTblLoadingTO = SelectLoadingTOWithDetails(tblLoadingTO.MergeLoadingId);

                if (exitingTblLoadingTO == null)
                {
                    resultMessage.DefaultBehaviour("Loading TO not found against - " + exitingTblLoadingTO);
                    return resultMessage;
                }

                if (exitingTblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || exitingTblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)
                {
                    resultMessage.DefaultBehaviour("Existing loading Id - " + exitingTblLoadingTO.IdLoading + " status is - " + exitingTblLoadingTO.StatusDesc);
                    return resultMessage;
                }

                //exitingTblLoadingTO.IsJointDelivery = 1;

                if (tblLoadingTO.LoadingSlipList != null && tblLoadingTO.LoadingSlipList.Count > 0)
                {
                    for (int u = 0; u < tblLoadingTO.LoadingSlipList.Count; u++)
                    {
                        TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[u];
                        TblLoadingSlipTO temp = exitingTblLoadingTO.LoadingSlipList.Where(w => w.DealerOrgId == tblLoadingSlipTO.DealerOrgId).FirstOrDefault();
                        if (temp == null)
                        {
                            exitingTblLoadingTO.IsJointDelivery = 1;
                        }
                    }

                }

                List<TblLoadingSlipExtTO> allTblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                Int32 itemModBusRefId = 0;

                for (int u = 0; u < exitingTblLoadingTO.LoadingSlipList.Count; u++)
                {
                    allTblLoadingSlipExtTOList.AddRange(exitingTblLoadingTO.LoadingSlipList[u].LoadingSlipExtTOList);
                }
                itemModBusRefId = allTblLoadingSlipExtTOList.Max(s => s.ModbusRefId);

                Double existingLoadingQty = exitingTblLoadingTO.TotalLoadingQty;
                Double finalLoadQty = 0;
                exitingTblLoadingTO.LoadingSlipList = tblLoadingTO.LoadingSlipList;

                tblLoadingTO.TotalLoadingQty = 0;
                for (int c1 = 0; c1 < tblLoadingTO.LoadingSlipList.Count; c1++)
                {
                    TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[c1];

                    tblLoadingSlipTO.IsMerge = 1;

                    for (int c2 = 0; c2 < tblLoadingSlipTO.LoadingSlipExtTOList.Count; c2++)
                    {
                        tblLoadingTO.TotalLoadingQty += tblLoadingSlipTO.LoadingSlipExtTOList[c2].LoadingQty;
                        tblLoadingSlipTO.LoadingSlipExtTOList[c2].LoadingLayerid = (int)Constants.LoadingLayerE.TOP;

                    }
                }

                exitingTblLoadingTO.TotalLoadingQty = existingLoadingQty + tblLoadingTO.TotalLoadingQty;

                Int32 changeStatustoDevice = 0;

                if (exitingTblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED || exitingTblLoadingTO.TranStatusE == Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH)
                {
                    //Update status & delete gross weight entry against loading

                    if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                    {
                        changeStatustoDevice = 1;  //device not supporting connection trancaton so add flag.
                        exitingTblLoadingTO.IgnoreGrossWt = 1;

                    }
                    else
                    {
                        exitingTblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                        exitingTblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                        exitingTblLoadingTO.StatusReason = "Vehicle Entered In The Premises";

                        List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(exitingTblLoadingTO.IdLoading, conn, tran);
                        if (tblWeighingMeasuresTOList != null && tblWeighingMeasuresTOList.Count > 0)
                        {
                            for (int r = 0; r < tblWeighingMeasuresTOList.Count; r++)
                            {
                                TblWeighingMeasuresTO tblWeighingMeasuresTO = tblWeighingMeasuresTOList[r];

                                if (tblWeighingMeasuresTO.WeightMeasurTypeId == (Int32)Constants.TransMeasureTypeE.GROSS_WEIGHT)
                                {
                                    result = _iTblWeighingMeasuresDAO.DeleteTblWeighingMeasures(tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                                    if (result != 1)
                                    {
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error While deleting tblWeighingMeasuresTO.IdWeightMeasure - " + tblWeighingMeasuresTO.IdWeightMeasure;
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        return resultMessage;
                                    }
                                }

                            }
                        }
                    }
                }

                resultMessage = SaveLoadingSlipDetails(exitingTblLoadingTO, conn, tran, weightSourceConfigId, itemModBusRefId);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                {

                    if (exitingTblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_NOT_CONFIRM)
                    {
                        //Write Status On IOT.
                        exitingTblLoadingTO.VehicleNo = "";
                        exitingTblLoadingTO.TransporterOrgId = 0;
                        exitingTblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                        exitingTblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                    }
                    //exitingTblLoadingTO.StatusReason = "Loading Scheduled";
                }

                result = UpdateTblLoading(exitingTblLoadingTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(exitingTblLoadingTO, conn, tran);
                if (result <= 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method SaveNewLoadingSlip";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }

                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                {
                    if (changeStatustoDevice == 1)
                    {
                        exitingTblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                        exitingTblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;

                        DimStatusTO statusTO = _iDimStatusDAO.SelectDimStatus(exitingTblLoadingTO.StatusId, conn, tran);
                        if (statusTO == null || statusTO.IotStatusId == 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("iot status id not found for loading to pass at gate iot");
                            return resultMessage;
                        }
                        exitingTblLoadingTO.StatusReason = statusTO.StatusDesc;

                        List<object[]> frameList = _iIotCommunication.GenerateGateIoTStatusFrameData(exitingTblLoadingTO, statusTO.IotStatusId);
                        if (frameList != null && frameList.Count > 0)
                        {
                            for (int f = 0; f < frameList.Count; f++)
                            {
                                result = _iIotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi(exitingTblLoadingTO, frameList[f]);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error while PostGateAPIDataToModbusTcpApi");
                                    return resultMessage;
                                }
                            }
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour("frameList Found Null Or Empty while PostGateAPIDataToModbusTcpApi");
                            return resultMessage;
                        }
                    }
                }
                tran.Commit();
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveNewLoadingSlip";
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


        }

        public ResultMessage SaveNewLoadingSlip (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                //Saket [2020-05-23] Added default from Org ID
                if (tblLoadingTO.FromOrgId == 0)
                {
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (tblConfigParamsTO == null)
                    {
                        resultMessage.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                        return resultMessage;
                    }
                    Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);

                    tblLoadingTO.FromOrgId = internalOrgId;

                }

                #region Check Merge Condition

                resultMessage = CheckMergeCondition(tblLoadingTO);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
                else if (resultMessage.MessageType == ResultMessageE.Information && resultMessage.Result == 2)
                {
                    resultMessage.Result = 1;
                    return resultMessage;
                }

                #endregion

                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Save New Main Loading Slip

                //Int64 earlierCount = _iTblLoadingDAO.SelectCountOfLoadingSlips(tblLoadingTO.CreatedOn,conn, tran);
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

                String Day = tblLoadingTO.CreatedOn.Day.ToString();
                String Month = tblLoadingTO.CreatedOn.Month.ToString();
                if (Day.Length == 1)
                    Day= Day.Insert(0, "0");

                if (Month.Length == 1)
                    Month = Month.Insert(0, "0");
                String loadingSlipNo = Day + "" + Month + "" + tblLoadingTO.CreatedOn.Year + "/" + loadingEntityRangeTO.EntityPrevValue;
                #region IOT related code added
                //Hrushikesh [30-7-2019] added code for IOT
                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
                if (weightSourceConfigId == (int)StaticStuff.Constants.WeighingDataSourceE.IoT)
                {
                    tblLoadingTO.ModbusRefId = _iCommon.GetNextAvailableModRefIdNew();
                    if (tblLoadingTO.ModbusRefId == 0)
                    {
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : ModbusRef List gretter than 255 or Number not found Or Dublicate number found";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }
                #endregion
                loadingEntityRangeTO.EntityPrevValue++;
                result = _iTblEntityRangeDAO.UpdateTblEntityRange(loadingEntityRangeTO, conn, tran);
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
                TblConfigParamsTO tblConfigParamsTOAutoGateIn = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_AUTO_GATE_IN_VEHICLE, conn, tran);
                if (tblConfigParamsTOAutoGateIn != null)
                {
                    isAutoGateInVehicle = Convert.ToInt32(tblConfigParamsTOAutoGateIn.ConfigParamVal);
                }

                //Boolean isBoyondLoadingQuota = false;
                Double finalLoadQty = 0;
                tblLoadingTO.TotalLoadingQty = 0;
                tblLoadingTO.LoadingSlipNo = loadingSlipNo;
                //Vijaymala added[22-06-2018]
                if (isAutoGateInVehicle == 1)
                {
                    if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                    {
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                        tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                        tblLoadingTO.StatusReason = "Loading Scheduled";
                    }
                    else
                    {
                        tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                        tblLoadingTO.StatusReason = "Vehicle Entered In The Premises";
                    }
                }
                else
                {
                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;
                    tblLoadingTO.StatusReason = "Loading Scheduled";
                }

                //Vijaymala added[22-06-2018]
                tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                tblLoadingTO.CreatedOn = _iCommon.ServerDateTime;

                int transporterId = tblLoadingTO.TransporterOrgId;
                string vehicleNumber = tblLoadingTO.VehicleNo;
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                {
                    tblLoadingTO.TransporterOrgId = 0;
                    tblLoadingTO.VehicleNo = string.Empty;
                }

                #region Assign default Gate

                if (tblLoadingTO.GateId == 0)
                {
                    TblGateTO tblGateTO = _iTblGateBL.GetDefaultTblGateTO();
                    if (tblGateTO != null)
                    {
                        tblLoadingTO.GateId = tblGateTO.IdGate;
                        tblLoadingTO.PortNumber = tblGateTO.PortNumber;
                        tblLoadingTO.IoTUrl = tblGateTO.IoTUrl;
                        tblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    }
                }

                #endregion

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

                resultMessage = SaveLoadingSlipDetails(tblLoadingTO, conn, tran, weightSourceConfigId);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
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

                //tblLoadingStatusHistoryTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                //tblLoadingStatusHistoryTO.StatusRemark = "Apporval Needed";
                tblLoadingStatusHistoryTO.TranStatusE = tblLoadingTO.TranStatusE;
                tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;

                result = _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error in InsertTblLoadingStatusHistory";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                #endregion

                #region to set loading depend on other booking as other loading slip
                if (tblLoadingTO.LoadingType != (int)Constants.LoadingTypeE.OTHER)
                {
                    Int32 changeType = 1;

                    for (int k = 0; k < tblLoadingTO.LoadingSlipList.Count; k++)
                    {
                        TblLoadingSlipTO tempLoadingSlipTo = tblLoadingTO.LoadingSlipList[k];

                        if (tempLoadingSlipTo.BookingType != (int)Constants.BookingType.IsOther)
                        {
                            changeType = 0;
                            break;
                        }
                    }

                    if (changeType == 1)
                    {
                        tblLoadingTO.LoadingType = (int)Constants.LoadingTypeE.OTHER;
                    }

                }

                #endregion

                #region 4. Finally Update the Total Loading Qty And Its Status based on loading quota consumption
                tblLoadingTO.TotalLoadingQty = 0;
                for (int c1 = 0; c1 < tblLoadingTO.LoadingSlipList.Count; c1++)
                {
                    for (int c2 = 0; c2 < tblLoadingTO.LoadingSlipList[c1].LoadingSlipExtTOList.Count; c2++)
                    {
                        tblLoadingTO.TotalLoadingQty += tblLoadingTO.LoadingSlipList[c1].LoadingSlipExtTOList[c2].LoadingQty;
                    }
                } 


                //tblLoadingTO.TotalLoadingQty = finalLoadQty;
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
                    if (weightSourceConfigId != (int)Constants.WeighingDataSourceE.IoT)
                    {
                        tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                        tblLoadingTO.StatusReason = "Vehicle Entered In The Premises";
                    }
                }
                else
                {

                    tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_NOT_CONFIRM;
                    tblLoadingTO.StatusReason = "Apporval Needed";
                }
                //Added by kiran for not confirm loading 
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT && tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_NOT_CONFIRM)
                {
                    tblLoadingTO.VehicleNo = vehicleNumber;
                    tblLoadingTO.TransporterOrgId = transporterId;
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
                result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
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
                        distinctLoadingSlipList.ForEach(f => f.DealerOrgName = f.DealerOrgName.Replace(',', ' '));
                        dealerOrgNames = String.Join(" , ", distinctLoadingSlipList.Select(s => s.DealerOrgName.ToString()).ToArray());
                    }

                }
                TblConfigParamsTO dealerNameConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);
                Int32 dealerNameActive = 0;
                if (dealerNameConfTO != null)
                    dealerNameActive = Convert.ToInt32(dealerNameConfTO.ConfigParamVal);

                if (true)
                {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED;
                    tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CONFIRMATION_REQUIRED";

                    tblAlertInstanceTO.AlertComment = "Loading slip  " + tblLoadingTO.LoadingSlipNo + "  is awaiting for confirmation";
                    if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                    {
                        tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                        tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ")."; //      
                    }
                    tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.CreatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(12);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CONFIRMATION_REQUIRED";
                    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                    tblAlertInstanceTO.RaisedBy = tblLoadingTO.CreatedBy;
                    tblAlertInstanceTO.RaisedOn = tblLoadingTO.CreatedOn;
                    tblAlertInstanceTO.IsAutoReset = 0;

                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
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
                #region @KKM [10-Dec-2018] Call To IoT To write the vehicle details

                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int)Constants.WeighingDataSourceE.BOTH)
                {
                    if (tblLoadingTO.StatusId == (Int32)Constants.TranStatusE.LOADING_CONFIRM || tblLoadingTO.StatusId == (Int32)Constants.TranStatusE.LOADING_NEW)
                    {
                        if (isAutoGateInVehicle == 1)
                        {
                            tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                            tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                            tblLoadingTO.StatusReason = "Vehicle Entered In The Premises";
                        }
                        int res = WriteDataOnIOT(tblLoadingTO, conn, tran, vehicleNumber, transporterId);
                        if (res == 0)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Network Error, Please Try Again";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                    }
                }

                #endregion

                //tran.Commit();

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
                    TblConfigParamsTO tblConfigParamsTOApproval = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SKIP_LOADING_APPROVAL, conn, tran);
                    if (tblConfigParamsTOApproval != null)
                    {
                        Int32 skiploadingApproval = Convert.ToInt32(tblConfigParamsTOApproval.ConfigParamVal);
                        if (skiploadingApproval == 1)
                        {
                            tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                            tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                            tblLoadingTO.StatusReason = "Loading Slip Auto Approved";

                            tblLoadingTO.UpdatedBy = tblLoadingTO.CreatedBy;
                            tblLoadingTO.StatusDate = _iCommon.ServerDateTime;
                            tblLoadingTO.UpdatedOn = tblLoadingTO.StatusDate;

                            //return UpdateDeliverySlipConfirmations(tblLoadingTO);
                            resultMessage = UpdateDeliverySlipConfirmations(tblLoadingTO, conn, tran);
                            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return resultMessage;
                            }

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

                tran.Commit();

                resultMessage.Tag = tblLoadingTO.IdLoading;
                resultMessage.Result = 1;
                return resultMessage;
            }
            catch (Exception ex) {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback ();

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveNewLoadingSlip";
                resultMessage.Tag = ex;
                resultMessage.Result = -1;
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage SaveLoadingSlipDetails(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran, int weightSourceConfigId , Int32 startModBusRefId = 0)
        {

            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage();
            Boolean isBoyondLoadingQuota = false;
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

            TblConfigParamsTO tblConfigParamsTOBrandWise = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_BRAND_WISE_INVOICE, conn, tran);
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

                            if (distinctBrandList != null && distinctBrandList.Count > 1) //Greater than 1 condtion if brand is distinct then only separate the loading slip.
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

            //Aniket [30-7-2019] added for IOT

            int modbusRefIdInc = startModBusRefId;
            for (int i = 0; i < tblLoadingTO.LoadingSlipList.Count; i++)
            {
                TblLoadingSlipTO tblLoadingSlipTO = tblLoadingTO.LoadingSlipList[i];
                tblLoadingSlipTO.FromOrgId = tblLoadingTO.FromOrgId;
                tblLoadingSlipTO.LoadingId = tblLoadingTO.IdLoading;
                //Aniket [30-7-2019] added for IOT
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                {
                    tblLoadingSlipTO.VehicleNo = string.Empty;
                }
                else
                    tblLoadingSlipTO.VehicleNo = tblLoadingTO.VehicleNo;

                //Vijaymala added[22-06-2018]
                //if (isAutoGateInVehicle == 1)
                //{
                //    if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                //    {
                //        tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                //        tblLoadingSlipTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                //        tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                //        tblLoadingTO.StatusReason = "Loading Scheduled";
                //    }
                //    else
                //    {
                //        tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_GATE_IN;
                //        tblLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_GATE_IN;
                //    }

                //}
                //else
                //{
                //    tblLoadingSlipTO.TranStatusE = Constants.TranStatusE.LOADING_NEW;

                //}


                tblLoadingSlipTO.TranStatusE = tblLoadingTO.TranStatusE;
                tblLoadingSlipTO.StatusId = tblLoadingTO.StatusId;

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
                    freightPerMT = tblLoadingSlipTO.FreightAmt; // CalculateFreightAmtPerTon(tblLoadingTO.LoadingSlipList, tblLoadingTO.FreightAmt);
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

                Int64 slipCnt = _iTblLoadingSlipDAO.SelectCountOfLoadingSlips(tblLoadingTO.CreatedOn, tblLoadingSlipTO.IsConfirmed, conn, tran);
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
                    String Day = tblLoadingTO.CreatedOn.Day.ToString();
                    String Month = tblLoadingTO.CreatedOn.Month.ToString();
                    if (Day.Length == 1)
                        Day = Day.Insert(0, "0");
                    if (Month.Length == 1)
                        Month = Month.Insert(0, "0");

                    slipNo = Day + "" + Month + "" + tblLoadingTO.CreatedOn.Year.ToString() + "/" + entityRangeTO.EntityPrevValue;

                    entityRangeTO.EntityPrevValue++;
                    result = _iTblEntityRangeDAO.UpdateTblEntityRange(entityRangeTO, conn, tran);
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

                    String Day = tblLoadingTO.CreatedOn.Day.ToString();
                    String Month = tblLoadingTO.CreatedOn.Month.ToString();
                    if (Day.Length == 1)
                        Day = Day.Insert(0, "0");
                    if (Month.Length == 1)
                        Month = Month.Insert(0, "0");
                    slipNo = Day + "" + Month + "" + tblLoadingTO.CreatedOn.Year.ToString() + "" + "NC/" + entityRangeTO.EntityPrevValue;

                    entityRangeTO.EntityPrevValue++;
                    result = _iTblEntityRangeDAO.UpdateTblEntityRange(entityRangeTO, conn, tran);
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

                result = _iTblLoadingSlipBL.InsertTblLoadingSlip(tblLoadingSlipTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : While inserting into InsertTblLoadingSlip";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                #region Insert PaymentTerm Option Relation
                if (tblLoadingSlipTO.PaymentTermOptionRelationTOLst != null && tblLoadingSlipTO.PaymentTermOptionRelationTOLst.Count > 0)
                {
                    TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = new TblPaymentTermOptionRelationTO();

                    for (int j = 0; j < tblLoadingSlipTO.PaymentTermOptionRelationTOLst.Count; j++)
                    {
                        tblPaymentTermOptionRelationTO = tblLoadingSlipTO.PaymentTermOptionRelationTOLst[j];
                        tblPaymentTermOptionRelationTO.CreatedBy = tblLoadingSlipTO.CreatedBy;
                        tblPaymentTermOptionRelationTO.CreatedOn = _iCommon.ServerDateTime;
                        tblPaymentTermOptionRelationTO.BookingId = 0;
                        tblPaymentTermOptionRelationTO.LoadingId = tblLoadingSlipTO.IdLoadingSlip;

                        result = _iTblPaymentTermOptionRelationDAO.InsertTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO, conn, tran);
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

                #region Loading Slip Order And Qty Details

                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();

                List<TblBookingExtTO> tblBookingExtTOList = new List<TblBookingExtTO>();

                //Prajakta [2020-08-17] Added isReg Condition.
                Boolean isReg = false;
                if (tblLoadingSlipTO.TblLoadingSlipDtlTO != null && tblLoadingSlipTO.TblLoadingSlipDtlTO.BookingId > 0)
                {
                    isReg = true;
                }

                if(isReg)
                //if (tblLoadingTO.LoadingType != (int)Constants.LoadingTypeE.OTHER)
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
                    result = _iTblLoadingSlipDtlDAO.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : While inserting into InsertTblLoadingSlipDtl";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }

                    //finalLoadQty += tblLoadingSlipDtlTO.LoadingQty;

                    //Call to update pending booking qty for loading

                    tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(tblLoadingSlipDtlTO.BookingId, conn, tran);
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
                    double totalLoadedBundles = 0;
                    tblLoadingSlipTO.LoadingSlipExtTOList.ForEach(ele =>
                    {
                        totalLoadedBundles += ele.Bundles;
                    });
                    tblBookingsTO.IdBooking = tblLoadingSlipDtlTO.BookingId;
                    tblBookingsTO.PendingQty = tblBookingsTO.PendingQty - tblLoadingSlipDtlTO.LoadingQty;
                    tblBookingsTO.PendingUomQty = tblBookingsTO.PendingUomQty - totalLoadedBundles;
                    tblBookingsTO.UpdatedBy = tblLoadingSlipTO.CreatedBy;
                    tblBookingsTO.UpdatedOn = _iCommon.ServerDateTime;
                    tblLoadingSlipTO.BookingType = tblBookingsTO.BookingType;
                    if (tblBookingsTO.PendingQty < 0)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Pending Qty Of Selected Booking #" + tblBookingsTO.BookingDisplayNo + " is less then loading Qty" + Environment.NewLine + " Please recreate the loading slip";
                        return resultMessage;
                    }

                    //Check for Weight Tolerance . If pending Qty is within weight tolerance then mark the booking status as closed.
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_WEIGHT_TOLERANCE_IN_KGS, conn, tran);
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

                            result = _iTblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(bookingQtyConsumptionTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While InsertTblBookingQtyConsumption";
                                resultMessage.Tag = bookingQtyConsumptionTO;
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }
                            //commented by aniket [25-6-2019] 
                            //tblBookingsTO.PendingQty = 0;
                        }
                    }

                    result = _iTblBookingsDAO.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }

                    tblBookingExtTOList = _iTblBookingExtDAO.SelectAllTblBookingExt(tblBookingsTO.IdBooking, conn, tran);
                    if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
                    {
                        tblBookingExtTOList = tblBookingExtTOList.OrderBy(o => o.ScheduleDate).ToList();
                    }
                }

                #endregion

                #region LoadingSlip Layer Material Details.
                Double finalLoadQty = 0;
                resultMessage = InsertLoadingExtDetails(tblLoadingTO, conn, tran, ref isBoyondLoadingQuota, ref finalLoadQty, tblLoadingSlipTO, tblBookingsTO, tblBookingExtTOList, ref modbusRefIdInc);
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

                            result = _iTblLoadingSlipAddressDAO.InsertTblLoadingSlipAddress(deliveryAddressTO, conn, tran);
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

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        //Aniket [7-8-2019] added to write data on IOT
        public int WriteDataOnIOT (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran, String vehicleNumber, int transporterId) {
            int result = 0;
            DimStatusTO statusTO = _iDimStatusDAO.SelectDimStatus (tblLoadingTO.StatusId, conn, tran);
            if (statusTO == null || statusTO.IotStatusId == 0) {
                result = 0;
                return result;
            }

            // Call to post data to Gate IoT API
            List<object[]> frameList = _iIotCommunication.GenerateGateIoTFrameData (tblLoadingTO, vehicleNumber, statusTO.IotStatusId, transporterId);
            if (frameList != null && frameList.Count > 0) {
                for (int f = 0; f < frameList.Count; f++) {
                    //Saket [2019-04-11] Keep common call for write data too IOT i.e from approval
                    //result = IoT.IotCommunication.PostGateAPIDataToModbusTcpApiForLoadingSlip(tblLoadingTO, frameList[f]);
                    result = _iIotCommunication.PostGateAPIDataToModbusTcpApi (tblLoadingTO, frameList[f]);
                    if (result != 1) {
                        result = 0;
                        return result;
                    }
                }
            } else {
                result = 0;
            }
            return result;
        }
        //Aniket [30-7-2019] added for IOT
        public ResultMessage UpdateLoadingStatusToGateIoT(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            DimStatusTO statusTO;
            if (conn != null)
            {
                statusTO = _iDimStatusDAO.SelectDimStatus(tblLoadingTO.StatusId, conn, tran);
            }
            else
            {
                statusTO = _iDimStatusDAO.SelectDimStatus(tblLoadingTO.StatusId);
            }
            if (statusTO == null || statusTO.IotStatusId == 0)
            {
                resultMessage.DefaultBehaviour("iot status id not found for loading to pass at gate iot");
                return resultMessage;
            }

            // Call to post data to Gate IoT API
            List<object[]> frameList = _iIotCommunication.GenerateGateIoTStatusFrameData(tblLoadingTO, statusTO.IotStatusId);
            if (frameList != null && frameList.Count > 0)
            {
                for (int f = 0; f < frameList.Count; f++)
                {
                    result = _iIotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi(tblLoadingTO, frameList[f]);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error while PostGateAPIDataToModbusTcpApi");
                        return resultMessage;
                    }
                }
            }
            else
            {
                resultMessage.DefaultBehaviour("frameList Found Null Or Empty while PostGateAPIDataToModbusTcpApi");
                return resultMessage;
            }

            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage PostChangeGateIOTAgainstLoading (TblLoadingTO tblLoadingTO) {

            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                if (tblLoadingTO == null) {
                    throw new Exception ("LoadingTO==null");
                }
                if (tblLoadingTO.GateId == 0) {
                    throw new Exception ("tblLoadingTO.GateId == 0");
                }

                TblLoadingTO existingTblLoadingTO = SelectTblLoadingTO (tblLoadingTO.IdLoading);
                if (existingTblLoadingTO == null) {
                    throw new Exception ("existingTblLoadingTO == null" + tblLoadingTO.IdLoading);
                }

                if (tblLoadingTO.GateId == existingTblLoadingTO.GateId) {
                    throw new Exception ("GateId != existingGateId");
                }
                //previous gate Id
                int previousGateId = existingTblLoadingTO.GateId;
                existingTblLoadingTO.GateId = tblLoadingTO.GateId;
                existingTblLoadingTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                existingTblLoadingTO.UpdatedOn = tblLoadingTO.UpdatedOn;
                Int32 result = 0;
                //Get Data from IOT
                //start vipul[16/04/2019] read existing data from gate
                List<DimStatusTO> dimStatusTOList = _iDimStatusDAO.SelectAllDimStatus ((Int32)Constants.TransactionTypeE.LOADING);
                TblLoadingSlipTO loadingslip = new TblLoadingSlipTO ();
                loadingslip.LoadingId = existingTblLoadingTO.IdLoading;
                existingTblLoadingTO = _iIotCommunication.GetItemDataFromIotAndMerge (existingTblLoadingTO, false, true); //TblLoadingSlipBL.GetVehicalHistoryDataFromIoT(existingTblLoadingTO, loadingslip, dimStatusTOList);
                //end
                //Write Data to IOT
                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting ();
                // if ((weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int)Constants.WeighingDataSourceE.BOTH) && tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM)
                if ((weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int) Constants.WeighingDataSourceE.BOTH)) {
                    TblGateTO tblGateTO = _iTblGateBL.SelectTblGateTO (tblLoadingTO.GateId);
                    if (tblGateTO == null) {
                        throw new Exception ("tblGateTO == null for gateId - " + tblLoadingTO.GateId);
                    }

                    tblLoadingTO.PortNumber = tblGateTO.PortNumber;
                    tblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    tblLoadingTO.IoTUrl = tblGateTO.IoTUrl;

                    //start
                    for (int i = 0; i < existingTblLoadingTO.LoadingStatusHistoryTOList.Count; i++) {

                        DimStatusTO statusTO = _iDimStatusDAO.SelectDimStatus (existingTblLoadingTO.LoadingStatusHistoryTOList[i].StatusId, conn, tran);
                        if (statusTO == null || statusTO.IotStatusId == 0) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("iot status id not found for loading to pass at gate iot");
                            return resultMessage;
                        }
                        // int result = 0;
                        // Call to post data to Gate IoT API
                        if (i == 0) {
                            List<object[]> frameList = _iIotCommunication.GenerateGateIoTFrameData (existingTblLoadingTO, existingTblLoadingTO.VehicleNo, statusTO.IotStatusId, existingTblLoadingTO.TransporterOrgId);
                            if (frameList != null && frameList.Count > 0) {
                                result = _iIotCommunication.PostGateAPIDataToModbusTcpApi (tblLoadingTO, frameList[i]);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                                    return resultMessage;
                                }
                            } else {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ("Error while Generate Gate IoT Frame Data ");
                                return resultMessage;
                            }
                        } else {
                            List<object[]> frameList = _iIotCommunication.GenerateGateIoTStatusFrameData (existingTblLoadingTO, statusTO.IotStatusId);
                            if (frameList != null && frameList.Count > 0) {
                                for (int f = 0; f < frameList.Count; f++) {
                                    result = _iIotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi (tblLoadingTO, frameList[f]);
                                    if (result != 1) {
                                        resultMessage.DefaultBehaviour ("Error while PostGateAPIDataToModbusTcpApi");
                                        return resultMessage;
                                    }
                                }
                            } else {
                                resultMessage.DefaultBehaviour ("frameList Found Null Or Empty while PostGateAPIDataToModbusTcpApi");
                                return resultMessage;
                            }
                        }
                        Thread.Sleep (500);
                    }
                }

                if (result != 1) {
                    resultMessage.DefaultBehaviour ("Error while writing data on gate iot");
                    return resultMessage;
                }
                var result1 = new GateIoTResult ();
                result1.Code = 0;
                int cnt = 0;
                while (cnt < 3) {
                    result1 = _iGateCommunication.DeleteSingleLoadingFromGateIoT (existingTblLoadingTO);
                    if (result1.Code == 1) {
                        break;
                    }
                    cnt++;
                }
                // result1 = IotCommunication.DeleteSingleLoadingFromGateIoT(existingTblLoadingTO);
                if (result1 == null || result1.Code == 0) {
                    //Remove write data from another Gate IOT

                    TblGateTO tblGateTO = _iTblGateBL.SelectTblGateTO (tblLoadingTO.GateId);
                    if (tblGateTO == null) {
                        throw new Exception ("tblGateTO == null for gateId - " + tblLoadingTO.GateId);
                    }

                    existingTblLoadingTO.PortNumber = tblGateTO.PortNumber;
                    existingTblLoadingTO.MachineIP = tblGateTO.MachineIP;
                    existingTblLoadingTO.IoTUrl = tblGateTO.IoTUrl;
                    var result2 = new GateIoTResult ();
                    result2.Code = 0;
                    cnt = 0;
                    while (cnt < 3) {
                        result2 = _iGateCommunication.DeleteSingleLoadingFromGateIoT (existingTblLoadingTO);
                        if (result2.Code == 1) {
                            break;
                        }
                    }

                    throw new Exception ("Error while deleting gate IOT data");

                }

                existingTblLoadingTO.VehicleNo = "";
                existingTblLoadingTO.TransporterOrgId = 0;
                existingTblLoadingTO.StatusId = Convert.ToInt16 (Constants.TranStatusE.LOADING_CONFIRM);
                existingTblLoadingTO.StatusReason = "Loading Scheduled & Confirmed";
                result = UpdateTblLoading (existingTblLoadingTO, conn, tran);
                if (result != 1) {
                    throw new Exception ("Error while updating gateId for loading Id - " + tblLoadingTO.IdLoading);
                }

                tran.Commit ();
                resultMessage.DefaultSuccessBehaviour ();

                return resultMessage;
            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateDeliverySlipConfirmations");
                return resultMessage;
            } finally {
                conn.Close ();
            }

        }

        private ResultMessage InsertLoadingExtDetails (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran, ref bool isBoyondLoadingQuota, ref double finalLoadQty, TblLoadingSlipTO tblLoadingSlipTO, TblBookingsTO tblBookingsTO, List<TblBookingExtTO> tblBookingExtTOList, ref int modbusRefIdInc) {
            string loadingSlipNo = tblLoadingTO.LoadingSlipNo;
            Int32 result = 0;
            ResultMessage resultMessage = new ResultMessage ();
            if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0)
            {
                tblBookingExtTOList = tblBookingExtTOList.Where(w => w.ScheduleId > 0).ToList();
            }
            if (tblLoadingSlipTO.LoadingSlipExtTOList != null && tblLoadingSlipTO.LoadingSlipExtTOList.Count > 0) {
                if (tblLoadingTO.LoadingType == (int) Constants.LoadingTypeE.OTHER) {
                    for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++) {

                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                        finalLoadQty += tblLoadingSlipExtTO.LoadingQty;
                    } //Vijaymala[23-Aug-2018] Commented for New Changes.
                }
                if (false) {

                    List<TblStockDetailsTO> stockDetailsList = _iTblStockDetailsDAO.SelectAllTblStockDetails ();
                    List<TblProductItemTO> productItemlist = _iTblProductItemDAO.SelectAllTblProductItem (0);
                    stockDetailsList = stockDetailsList.Where (ele => ele.ProdItemId > 0).ToList ();

                    for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++) {

                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];
                        tblLoadingSlipExtTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;

                        //Sudhir[15-Jan-2018] Added for get the productitemTo for checking otheritemstock update is require or not.
                        TblProductItemTO productItemTO = productItemlist.Where (ele => ele.IdProdItem == tblLoadingSlipExtTO.ProdItemId).FirstOrDefault ();
                        if (productItemTO != null) {
                            if (productItemTO.IsStockRequire == 1) {
                                // Vaibhav [09-April-2018] Added to select compartment wise stock.
                                List<TblStockDetailsTO> stockDetailsFilterList = null;
                                if (tblLoadingSlipExtTO.CompartmentId == 0) {
                                    stockDetailsFilterList = stockDetailsList.Where (x => x.ProdItemId == tblLoadingSlipExtTO.ProdItemId && x.BrandId == tblLoadingSlipExtTO.BrandId).ToList ();
                                } else {
                                    stockDetailsFilterList = stockDetailsList.Where (x => x.ProdItemId == tblLoadingSlipExtTO.ProdItemId && x.BrandId == tblLoadingSlipExtTO.BrandId && x.LocationId == tblLoadingSlipExtTO.CompartmentId).ToList ();
                                }

                                if (stockDetailsFilterList != null) {
                                    stockDetailsFilterList = stockDetailsFilterList.Where (w => w.TotalStock > 0).ToList ();
                                    if (stockDetailsFilterList == null || stockDetailsFilterList.Count == 0) {
                                        tran.Rollback ();
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error : stockList Found NULL ";

                                        if (tblLoadingSlipExtTO.CompartmentId > 0) {
                                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock For the Size " + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " at Compartment " + tblLoadingSlipExtTO.CompartmentName + " not found";
                                        } else {
                                            resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock For the Size " + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " not found";
                                        }
                                        resultMessage.Result = 0;
                                        return resultMessage;
                                    } else {
                                        //tblLoadingSlipExtTO.Tag = stockDetailsFilterList;
                                        // To Use in Stock consumption , Wrt Loading Quota Availability Update Master Stock
                                        var totalAvailStock = stockDetailsFilterList.Sum (s => s.TotalStock);
                                        if (totalAvailStock >= tblLoadingSlipExtTO.LoadingQty) {
                                            List<TblStockDetailsTO> stockList = stockDetailsFilterList;

                                            // Create Stock Consumption History Record
                                            //var stkConsList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                                            //                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                            //                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId).ToList();

                                            var stkConsList = stockList;

                                            Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                            for (int s = 0; s < stkConsList.Count; s++) {

                                                if (totalLoadingQty > 0) {
                                                    resultMessage = UpdateStockAndConsumptionHistory (tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, null, conn, tran);
                                                    if (resultMessage.MessageType != ResultMessageE.Information) {
                                                        tran.Rollback ();
                                                        resultMessage.DefaultBehaviour ();
                                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                                        resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip";
                                                        return resultMessage;
                                                    }
                                                }
                                            }

                                        } else {
                                            String errorMsg = tblLoadingSlipExtTO.ProdItemDesc;
                                            tran.Rollback ();
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
                        } else {

                        }
                        result = _iTblLoadingSlipExtDAO.InsertTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ();
                            resultMessage.Text = "Error : While InsertTblLoadingSlipExt Against other LoadingSlip";
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            return resultMessage;
                        }
                        finalLoadQty += tblLoadingSlipExtTO.LoadingQty;
                        isBoyondLoadingQuota = false;
                    }
                } else {
                    Int32 isAllowLoading = 0;
                    //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
                    if (tblConfigParamsTO != null) {
                        isAllowLoading = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                    }
                    //[27-08-2018]Vijaymala commented code parity details from parity table  
                    //String parityIds = String.Empty;
                    //List<TblBookingParitiesTO> tblBookingParitiesTOList = _iTblBookingParitiesBL.SelectTblBookingParitiesTOListByBookingId(tblBookingsTO.IdBooking, conn, tran);
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

                    // List<TblParityDetailsTO> parityDetailsTOList = BL._iTblParityDetailsBL.SelectAllTblParityDetailsList(parityIds, 0, conn, tran);

                    //List<TblParityDetailsTO> parityDetailsTOList = null;
                    //if (tblBookingsTO.ParityId > 0)
                    //    parityDetailsTOList = BL._iTblParityDetailsBL.SelectAllTblParityDetailsList(tblBookingsTO.ParityId, 0, conn, tran);

                    //List<TblLoadingQuotaDeclarationTO> quotaList = _iTblLoadingQuotaDeclarationBL.SelectLoadingQuotaListForCnfAndDate(tblLoadingTO.CnfOrgId, tblLoadingTO.CreatedOn, conn, tran);

                    List<TblProductInfoTO> productConfgList = _iTblProductInfoDAO.SelectAllLatestProductInfo (conn, tran);
                    if (productConfgList == null) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ();
                        resultMessage.Text = "Error : productConfgList Found NULL ";
                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Product Master Configuration is not completed.";
                        return resultMessage;
                    }

                    List<TblStockConfigTO> tblStockConfigTOList = _iTblStockConfigDAO.SelectAllTblStockConfigTOList (conn, tran);
                    if (tblStockConfigTOList == null) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ();
                        resultMessage.Text = "Error : StockConfigTOList Found NULL ";
                        resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Stock Configurator not found is not completed.";
                        return resultMessage;
                    }

                    tblStockConfigTOList = tblStockConfigTOList.Where (w => w.IsItemizedStock == 1).ToList ();

                    Int32 isConsolidateStk = _iTblConfigParamsBL.GetStockConfigIsConsolidate ();

                    #region Check Stock,Loading Quota,Validate and Not Save 
                    //[05-09-2018] : Vijaymala added code to get product item list
                    List<TblProductItemTO> stockRequireProductItemList = _iTblProductItemDAO.SelectProductItemListStockUpdateRequire (1);
                    Boolean isStockRequie = false;
                    for (int e = 0; e < tblLoadingSlipTO.LoadingSlipExtTOList.Count; e++) {
                        TblLoadingQuotaDeclarationTO loadingQuotaTOLive = null;
                        tblLoadingSlipTO.LoadingSlipExtTOList[e].ModbusRefId = modbusRefIdInc + 1;
                        modbusRefIdInc++;
                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[e];
                        if (tblLoadingSlipExtTO.LoadingQty > 0) {
                            tblLoadingSlipExtTO.LoadingSlipId = tblLoadingSlipTO.IdLoadingSlip;

                            #region Calculate Bundles from Loading Qty and Product Configuration
                            if (tblLoadingSlipExtTO.ProdItemId == 0) //Vijaymala added [05-08-2018]
                            {
                                var prodConfgTO = productConfgList.Where (p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                    p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault ();

                                if (prodConfgTO == null) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialDesc + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatDesc + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecDesc;
                                    resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                    return resultMessage;
                                }

                                //Product Configuration is per bundles and has avg Bundle Wtin Kg
                                //Hence convert loading qty(MT) to KG
                                Double noOfBundles = (tblLoadingSlipExtTO.LoadingQty * 1000) / prodConfgTO.AvgBundleWt;
                                tblLoadingSlipExtTO.Bundles = Math.Round (noOfBundles, 0);
                            } else {
                                //[05-09-2018]Vijaymala aadded code to set other item bundles
                                if (tblLoadingSlipExtTO.Bundles == 0)
                                    tblLoadingSlipExtTO.Bundles = tblLoadingSlipExtTO.LoadingQty;
                            }
                            #endregion

                            #region Stock Availability Calculations and Validations

                            //Check If Stock exist Or Not
                            //List<TblStockDetailsTO> stockList = _iTblStockDetailsDAO.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingTO.CreatedOn, conn, tran);

                            List<TblStockDetailsTO> stockList = new List<TblStockDetailsTO> ();

                            String isItemized = "Itemized";

                            if (isConsolidateStk == 1) {
                                isItemized = "Consolidate";

                                TblStockConfigTO tblStockConfigTO = tblStockConfigTOList.Where (w => w.BrandId == tblLoadingSlipExtTO.BrandId &&
                                    w.MaterialId == tblLoadingSlipExtTO.MaterialId && w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.IsItemizedStock == 1).FirstOrDefault ();

                                if (tblStockConfigTO != null) //Get Itemized Stock
                                {
                                    stockList = _iTblStockDetailsDAO.SelectAllTblStockDetails (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, new DateTime (), tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, conn, tran);
                                    isItemized = "Itemized";

                                    stockList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                        l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                        l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                        l.BrandId == tblLoadingSlipExtTO.BrandId &&
                                        l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList ();

                                    stockList = stockList.Where (w => w.TotalStock > 0).ToList ();

                                } else //Get consolidate stock brand wise
                                {
                                    stockList = _iTblStockDetailsDAO.SelectAllTblStockDetailsConsolidated (1, tblLoadingSlipExtTO.BrandId, conn, tran);
                                    stockList = stockList.Where (w => w.TotalStock > 0).ToList ();
                                }
                            } else {

                                //stockList = _iTblStockDetailsBL.SelectAllTblStockDetails(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, new DateTime(), tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, conn, tran);
                                stockList = _iTblStockDetailsDAO.SelectAllTblStockDetailsOther (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime (), conn, tran);

                                isItemized = "Itemized";

                                stockList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                    l.BrandId == tblLoadingSlipExtTO.BrandId &&
                                    l.ProdItemId == tblLoadingSlipExtTO.ProdItemId &&
                                    l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList ();

                                if (tblLoadingSlipExtTO.ProdItemId == 0) {
                                    //Check is item boughtout.
                                    TblStockConfigTO tblStockConfigTO = tblStockConfigTOList.Where (w => w.BrandId == tblLoadingSlipExtTO.BrandId &&
                                        w.MaterialId == tblLoadingSlipExtTO.MaterialId && w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                        w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId && w.IsItemizedStock == 1).FirstOrDefault ();

                                    if (tblStockConfigTO != null) //Get Itemized Stock
                                    {
                                        Double upQty = tblLoadingSlipExtTO.LoadingQty;
                                        Double existingQtyInMt = 0;
                                        Double newQtyInMt = 0;
                                        Int32 stockDtlId = 0;
                                        List<TblLocationTO> tblLocationTOList = _iTblLocationDAO.SelectAllTblLocation ();

                                        Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                        if (stockList == null || stockList.Count == 0) {
                                            resultMessage = InsertItemIntoStockDtlAndSummary (tblLoadingTO, totalLoadingQty, conn, tran, tblLoadingSlipExtTO, tblLoadingTO.CreatedBy, tblLocationTOList);
                                            if (resultMessage.Tag != null && resultMessage.Tag.GetType () == typeof (TblStockDetailsTO)) {
                                                TblStockDetailsTO tempStockDetailsTO = new TblStockDetailsTO ();

                                                tempStockDetailsTO = (TblStockDetailsTO) resultMessage.Tag;
                                                stockDtlId = tempStockDetailsTO.IdStockDtl;
                                                stockList = new List<TblStockDetailsTO> ();
                                                stockList.Add (tempStockDetailsTO);

                                            }
                                            //  return InsertItemIntoStockDtlAndSummary()
                                        }

                                        if (stockList != null && stockList.Count > 0) {
                                            TblStockDetailsTO tblStockDetailsTO = stockList[0];
                                            stockList = new List<TblStockDetailsTO> ();
                                            stockList.Add (tblStockDetailsTO);

                                            existingQtyInMt = tblStockDetailsTO.TotalStock;

                                            tblStockDetailsTO.TotalStock += upQty;
                                            tblStockDetailsTO.BalanceStock += upQty;
                                            tblStockDetailsTO.NoOfBundles += tblLoadingSlipExtTO.Bundles;

                                            newQtyInMt = tblStockDetailsTO.TotalStock;
                                            stockDtlId = tblStockDetailsTO.IdStockDtl;

                                            tblStockDetailsTO.UpdatedOn = _iCommon.ServerDateTime;
                                            tblStockDetailsTO.UpdatedBy = tblLoadingTO.CreatedBy;

                                            //Update Stock details

                                            result = _iTblStockDetailsDAO.UpdateTblStockDetails (tblStockDetailsTO, conn, tran);
                                            if (result != 1) {
                                                tran.Rollback ();
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                resultMessage.Text = "Error : While updating the stock details";
                                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while updating the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                                                // + tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")" + " not found";
                                                resultMessage.Result = 0;
                                                return resultMessage;
                                            }
                                        } else {
                                            if (isAllowLoading == 0) {
                                                tran.Rollback ();
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

                                        TblStockConsumptionTO tblStockConsumptionTO = new TblStockConsumptionTO ();

                                        tblStockConsumptionTO.BeforeStockQty = existingQtyInMt;
                                        tblStockConsumptionTO.AfterStockQty = newQtyInMt;
                                        tblStockConsumptionTO.LoadingSlipExtId = 0;
                                        tblStockConsumptionTO.CreatedBy = tblLoadingSlipTO.CreatedBy;
                                        tblStockConsumptionTO.CreatedOn = _iCommon.ServerDateTime;
                                        tblStockConsumptionTO.StockDtlId = stockDtlId;

                                        tblStockConsumptionTO.TxnQty = Math.Round (upQty, 2);

                                        if (tblStockConsumptionTO.TxnQty > 0) {
                                            tblStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;
                                            tblStockConsumptionTO.Remark = tblStockConsumptionTO.TxnQty + " Qty is added against bought out item";

                                        } else {
                                            tblStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.OUT;
                                            tblStockConsumptionTO.Remark = tblStockConsumptionTO.TxnQty + " Qty is consumed against bought out item";

                                        }

                                        if (tblStockConsumptionTO.TxnQty != 0) {
                                            result = _iTblStockConsumptionDAO.InsertTblStockConsumption (tblStockConsumptionTO, conn, tran);
                                            if (result != 1) {
                                                tran.Rollback ();
                                                resultMessage.Text = "Error While InsertTblStockConsumption : Method UpdateDailyStock";
                                                resultMessage.DisplayMessage = "Error.. Records could not be saved";
                                                resultMessage.Result = 0;
                                                resultMessage.MessageType = ResultMessageE.Error;
                                                return resultMessage;
                                            }
                                        }
                                        #endregion

                                    }

                                    stockList = stockList.Where (w => w.TotalStock > 0).ToList ();
                                }

                            }

                            if ((stockList == null || stockList.Count == 0) && isAllowLoading == 0) {

                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = " Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock For that item is not found ";

                                if (tblLoadingSlipExtTO.CompartmentId > 0)
                                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock For the Size " + tblLoadingSlipExtTO.DisplayName
                                    //+ tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")"
                                    +
                                    " at Compartment " + tblLoadingSlipExtTO.CompartmentName + " not found";
                                else
                                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. " + isItemized + " Stock For the Size " + tblLoadingSlipExtTO.DisplayName
                                    //+ tblLoadingSlipExtTO.MaterialDesc + "-" + tblLoadingSlipExtTO.ProdCatDesc + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.BrandDesc + ")"
                                    +
                                    " not found";

                                resultMessage.Result = 0;
                                return resultMessage;
                            }

                            tblLoadingSlipExtTO.Tag = stockList;

                            //[05-09-2018]Vijaymala added to check stock require for item or not
                            if (tblLoadingSlipExtTO.ProdItemId > 0) {
                                isStockRequie = stockRequireProductItemList.Where (ele => ele.IdProdItem == tblLoadingSlipExtTO.ProdItemId).
                                Select (x => x.IsStockRequire == 1).FirstOrDefault ();

                            } else {
                                isStockRequie = true;
                            }

                            if (isStockRequie) {

                                // To Use in Stock consumption , Wrt Loading Quota Availability Update Master Stock
                                var totalAvailStock = stockList.Sum (s => s.TotalStock);

                                if (totalAvailStock >= tblLoadingSlipExtTO.LoadingQty) {

                                } else {
                                    if (isAllowLoading == 0) {
                                        String errorMsg = tblLoadingSlipExtTO.DisplayName;
                                        //tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.ProdSpecDesc + ")";
                                        tran.Rollback ();
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
                            //    loadingQuotaTOLive = BL._iTblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaTO.IdLoadingQuota, conn, tran);
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
                            result = _iTblLoadingSlipExtDAO.InsertTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                            tblLoadingSlipExtTO.BookingExtId = bookingExtId;

                            if (result != 1) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.Text = "Error : While InsertTblLoadingSlipExt Against LoadingSlip";
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }

                            if (tblLoadingSlipExtTO.LoadingQuotaId > 0) // It means loading quota is not allocated. This request is beyond quota
                            {
                                //Create Loading Quota Consumption History Record
                                Models.TblLoadingQuotaConsumptionTO quotaConsumptionTO = new TblLoadingQuotaConsumptionTO ();
                                quotaConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                                quotaConsumptionTO.QuotaQty = -tblLoadingSlipExtTO.LoadingQty;
                                quotaConsumptionTO.AvailableQuota = tblLoadingSlipExtTO.QuotaBforeLoading;
                                quotaConsumptionTO.BalanceQuota = tblLoadingSlipExtTO.QuotaAfterLoading;
                                quotaConsumptionTO.LoadingQuotaId = tblLoadingSlipExtTO.LoadingQuotaId;
                                quotaConsumptionTO.Remark = "Quota Consumed Against Loading Slip No :" + loadingSlipNo;
                                quotaConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.OUT;
                                quotaConsumptionTO.CreatedBy = tblLoadingTO.CreatedBy;
                                quotaConsumptionTO.CreatedOn = tblLoadingTO.CreatedOn;

                                result = _iTblLoadingQuotaConsumptionDAO.InsertTblLoadingQuotaConsumption (quotaConsumptionTO, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }

                                //Update Balance Quota for Declared Loading Quota
                                loadingQuotaTOLive.BalanceQuota = loadingQuotaTOLive.BalanceQuota - tblLoadingSlipExtTO.LoadingQty;
                                loadingQuotaTOLive.UpdatedBy = tblLoadingTO.CreatedBy;
                                loadingQuotaTOLive.UpdatedOn = tblLoadingTO.CreatedOn;
                                loadingQuotaTOLive.Remark = tblLoadingSlipExtTO.LoadingQty + " Qty is consumed against Loading Slip : " + loadingSlipNo;
                                result = _iTblLoadingQuotaDeclarationDAO.UpdateTblLoadingQuotaDeclaration (loadingQuotaTOLive, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : While UpdateTblLoadingQuotaDeclaration Against LoadingSlip ";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }

                            #region Adjust Balance Qty

                            List<TblBookingExtTO> tblBookingExtTOListAdj = new List<TblBookingExtTO> ();

                            if (tblBookingExtTOList != null && tblBookingExtTOList.Count > 0) {
                                if (tblLoadingSlipExtTO.BookingExtId > 0) {
                                    TblBookingExtTO tblBookingExtTO = tblBookingExtTOList.Where (w => w.IdBookingExt == tblLoadingSlipExtTO.BookingExtId).FirstOrDefault ();

                                    tblBookingExtTOListAdj.Add (tblBookingExtTO);

                                }

                                List<TblBookingExtTO> temp = tblBookingExtTOList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                    l.BrandId == tblLoadingSlipExtTO.BrandId &&
                                    l.ProdItemId == tblLoadingSlipExtTO.ProdItemId).ToList ();

                                if (tblLoadingSlipExtTO.BookingExtId > 0) {
                                    temp = temp.Where (w => w.IdBookingExt != tblLoadingSlipExtTO.BookingExtId).ToList ();
                                }

                                if (temp != null && temp.Count > 0) {
                                    tblBookingExtTOListAdj.AddRange (temp);
                                }

                                Double qtyToAdjust = tblLoadingSlipExtTO.LoadingQty;
                                double uomQtytoAdjust = tblLoadingSlipExtTO.Bundles;
                                for (int a = 0; a < tblBookingExtTOListAdj.Count; a++) {
                                    if (qtyToAdjust > 0) {
                                        TblBookingExtTO tblBookingExtTO = tblBookingExtTOListAdj[a];
                                        if (tblBookingExtTO.BalanceQty > 0) {
                                            if (qtyToAdjust <= tblBookingExtTO.BalanceQty) {
                                                tblBookingExtTO.BalanceQty = tblBookingExtTO.BalanceQty - qtyToAdjust;
                                                tblBookingExtTO.PendingUomQty = tblBookingExtTO.PendingUomQty - uomQtytoAdjust;
                                                qtyToAdjust = 0;
                                                uomQtytoAdjust = 0;
                                            } else {

                                                qtyToAdjust -= tblBookingExtTO.BalanceQty;
                                                tblBookingExtTO.BalanceQty = 0;
                                            }

                                            tblBookingExtTO.BalanceQty = Math.Round(tblBookingExtTO.BalanceQty, 3);

                                            result = _iTblBookingExtDAO.UpdateTblBookingExt (tblBookingExtTO, conn, tran);
                                            if (result != 1) {
                                                tran.Rollback ();
                                                resultMessage.DefaultBehaviour ();
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
                    if (!isBoyondLoadingQuota) {

                        for (int stk = 0; stk < tblLoadingSlipTO.LoadingSlipExtTOList.Count; stk++) {

                            TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList[stk];

                            //Check If Stock exist Or Not
                            if (tblLoadingSlipExtTO.Tag == null && isAllowLoading == 0) {
                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error : stockList Found NULL ";
                                resultMessage.Result = 0;
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                return resultMessage;
                            }
                            TblProductInfoTO prodConfgTO = null;
                            if (tblLoadingSlipExtTO.ProdItemId == 0) {
                                prodConfgTO = productConfgList.Where (p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                    p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault ();

                                if (prodConfgTO == null) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialId + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatId + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecId;
                                    resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                    return resultMessage;
                                }
                            }

                            List<TblStockDetailsTO> stockList = (List<TblStockDetailsTO>) tblLoadingSlipExtTO.Tag;

                            // Create Stock Consumption History Record
                            //var stkConsList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                            //                                                                 && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                            //                                                                 && l.MaterialId == tblLoadingSlipExtTO.MaterialId).ToList();

                            var stkConsList = stockList;

                            Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;

                            for (int s = 0; s < stkConsList.Count; s++) {

                                if (totalLoadingQty > 0) {
                                    resultMessage = UpdateStockAndConsumptionHistory (tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, prodConfgTO, conn, tran);
                                    if (resultMessage.MessageType != ResultMessageE.Information) {
                                        tran.Rollback ();
                                        resultMessage.DefaultBehaviour ();
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip";
                                        return resultMessage;
                                    }
                                }
                            }
                            if (isAllowLoading == 1) {
                                TblStockConfigTO tblStockConfigTO = new TblStockConfigTO ();
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
                                List<TblLocationTO> tblLocationTOList = _iTblLocationDAO.SelectAllTblLocation ();

                                if (totalLoadingQty > 0) {
                                    resultMessage = UpdateStockAgainstItem (tblLoadingTO, totalLoadingQty, conn, tran, tblLoadingSlipExtTO, tblLoadingTO.CreatedBy, tblLocationTOList);
                                    // return ConfigureAllowStockUpdate(tblLoadingTO, conn, tran, ref result, resultMessage, tblLoadingSlipExtTO, ref stockList, isItemized, ref isAllowLoading, tblStockConfigTO, upQty, ref existingQtyInMt, ref stockDtlId, tblLocationTOList);
                                    // return resultMessage;
                                    if (resultMessage.MessageType != ResultMessageE.Information) {
                                        tran.Rollback ();
                                        resultMessage.DefaultBehaviour ();
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.Text = "Error : While UpdateStockAgainstItem Against LoadingSlip";
                                        return resultMessage;
                                    }
                                }
                            } else {
                                if (totalLoadingQty > 0) {
                                    String isItemized = String.Empty;
                                    String errorMsg = tblLoadingSlipExtTO.DisplayName;
                                    // tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc + "(" + tblLoadingSlipExtTO.ProdSpecDesc + ")";
                                    tran.Rollback ();
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
            } else {
                tran.Rollback ();
                resultMessage.DefaultBehaviour ();
                resultMessage.Text = "Error : LoadingSlipExtTOList(Loading Layer Details) Found Null Or Empty";
                resultMessage.DisplayMessage = "Error 01 : No Loading Layer Found To Save";
                return resultMessage;
            }

            resultMessage.DefaultSuccessBehaviour ();
            return resultMessage;
        }
        //private ResultMessage ConfigureAllowStockUpdate(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran, ref int result, ResultMessage resultMessage, TblLoadingSlipExtTO tblLoadingSlipExtTO, ref List<TblStockDetailsTO> stockList, string isItemized, ref Int32 isAllowLoading, TblStockConfigTO tblStockConfigTO, Double upQty, ref Double existingQtyInMt, ref int stockDtlId, List<TblLocationTO> tblLocationTOList)
        //{

        //    //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
        //    TblConfigParamsTO tblConfigParamsTO = BL._iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
        //    if (tblConfigParamsTO != null)
        //    {
        //        isAllowLoading = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
        //    }
        //    if (isAllowLoading == 1)
        //    {
        //        stockList = _iTblStockDetailsDAO.SelectAllTblStockDetailsOther(tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime(), conn, tran);
        //         isItemized = "Itemized";

        //        stockList = stockList.Where(l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId
        //                                                                         && l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
        //                                                                         && l.MaterialId == tblLoadingSlipExtTO.MaterialId
        //                                                                         && l.BrandId == tblLoadingSlipExtTO.BrandId
        //                                                                         && l.ProdItemId == tblLoadingSlipExtTO.ProdItemId
        //                                                                         && l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList();

        //        Double totalLoadingQty = 0 - tblLoadingSlipExtTO.LoadingQty;
        //        TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO();

        //        List<TblProductInfoTO> productList = BL._iTblProductInfoBL.SelectAllTblProductInfoList(conn, tran);

        //        if (stockList.Count == 0)
        //        {
        //            totalLoadingQty = 0 - tblLoadingSlipExtTO.LoadingQty;
        //            stockList = new List<TblStockDetailsTO>();

        //            //Insert Stock Summary
        //            TblStockSummaryTO tblStockSummaryTO = new TblStockSummaryTO();
        //            tblStockSummaryTO.StockDate = _iCommon.ServerDateTime;
        //            tblStockSummaryTO.NoOfBundles = 0;
        //            tblStockSummaryTO.TotalStock = 0;
        //            tblStockSummaryTO.CreatedBy = tblLoadingTO.CreatedBy;
        //            tblStockSummaryTO.CreatedOn = _iCommon.ServerDateTime;

        //            result = _iTblStockSummaryBL.InsertTblStockSummary(tblStockSummaryTO, conn, tran);
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
        //            tblStockDetailsTONew.CreatedOn = _iCommon.ServerDateTime;
        //            tblStockDetailsTONew.CreatedBy = tblLoadingTO.CreatedBy;

        //            TblProductInfoTO productInfoTO = productList.Where(p => p.MaterialId == tblStockDetailsTONew.MaterialId
        //                                            && p.ProdCatId == tblStockDetailsTONew.ProdCatId && p.ProdSpecId == tblStockDetailsTONew.ProdSpecId
        //                                            && p.BrandId == tblStockDetailsTONew.BrandId).FirstOrDefault();

        //            tblStockDetailsTONew.ProductId = productInfoTO.IdProduct;
        //            tblStockDetailsTONew.IsInMT = 1;
        //            tblStockDetailsTONew.StockSummaryId = tblStockSummaryTO.IdStockSummary;
        //            tblStockDetailsTONew.LocationId = tblLocationTOList[0].IdLocation;

        //            existingQtyInMt = tblStockDetailsTONew.TotalStock;

        //            result = _iTblStockDetailsBL.InsertTblStockDetails(tblStockDetailsTONew, conn, tran);
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
        //            tblStockSummaryTO.UpdatedOn = _iCommon.ServerDateTime;

        //            result = _iTblStockSummaryBL.UpdateTblStockSummary(tblStockSummaryTO, conn, tran);
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

        //            List<TblProductInfoTO> productConfgList = _iTblProductInfoBL.SelectAllTblProductInfoList(conn, tran);
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

        private ResultMessage UpdateStockAgainstItem (TblLoadingTO tblLoadingTO, Double totalLoadingQty, SqlConnection conn, SqlTransaction tran, TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 loginUserId, List<TblLocationTO> tblLocationTOList) {
            Int32 isAllowLoading = 0;
            Int32 stockDtlId = 0;
            ResultMessage resultMessage = new ResultMessage ();
            Int32 result = 0;
            try {

                //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
                if (tblConfigParamsTO != null) {
                    isAllowLoading = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                }
                if (isAllowLoading == 1) {
                    List<TblStockDetailsTO> stockList = _iTblStockDetailsDAO.SelectAllTblStockDetailsOther (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime (), conn, tran);
                    String isItemized = "Itemized";

                    stockList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                        l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                        l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                        l.BrandId == tblLoadingSlipExtTO.BrandId &&
                        l.ProdItemId == tblLoadingSlipExtTO.ProdItemId &&
                        l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList ();

                    List<TblProductInfoTO> productList = _iTblProductInfoDAO.SelectAllLatestProductInfo (conn, tran);
                    TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO ();

                    if (stockList.Count == 0) {
                        // For weight and Stock in MT calculations
                        tblStockDetailsTONew.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
                        tblStockDetailsTONew.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
                        tblStockDetailsTONew.BrandId = tblLoadingSlipExtTO.BrandId;
                        tblStockDetailsTONew.MaterialId = tblLoadingSlipExtTO.MaterialId;
                        tblStockDetailsTONew.CreatedOn = _iCommon.ServerDateTime;
                        tblStockDetailsTONew.CreatedBy = loginUserId;
                        tblStockDetailsTONew.ProdItemId = tblLoadingSlipExtTO.ProdItemId;

                        if (tblLoadingSlipExtTO.CompartmentId > 0) {
                            tblStockDetailsTONew.LocationId = tblLoadingSlipExtTO.CompartmentId;
                        } else {
                            Int32 tempLocatId = 0;

                            var tempLocationLst = tblLocationTOList.Where (w => w.ParentLocId > 0).ToList ();
                            if (tempLocationLst != null && tempLocationLst.Count > 0) {
                                tempLocatId = tempLocationLst[0].IdLocation;
                            }

                            tblStockDetailsTONew.LocationId = tempLocatId;
                        }

                        TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryDAO.SelectTblStockSummary (new DateTime ());
                        if (tblStockSummaryTO == null) {
                            tblStockSummaryTO = new TblStockSummaryTO ();
                            tblStockSummaryTO.StockDate = _iCommon.ServerDateTime;
                            tblStockSummaryTO.NoOfBundles = 0;
                            tblStockSummaryTO.TotalStock = 0;
                            tblStockSummaryTO.CreatedBy = loginUserId;
                            tblStockSummaryTO.CreatedOn = _iCommon.ServerDateTime;

                            result = _iTblStockSummaryDAO.InsertTblStockSummary (tblStockSummaryTO, conn, tran);
                            if (result != 1) {
                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error : While insert the stock summary";
                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }

                        tblStockDetailsTONew.StockSummaryId = tblStockSummaryTO.IdStockSummary;

                        result = _iTblStockDetailsDAO.InsertTblStockDetails (tblStockDetailsTONew, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While insert the stock details";
                            resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    } else {
                        tblStockDetailsTONew = stockList[0];
                    }

                    Double adjustedBundles = 0;

                    tblStockDetailsTONew.TotalStock = tblStockDetailsTONew.BalanceStock - totalLoadingQty;

                    tblStockDetailsTONew.LoadedStock += totalLoadingQty;
                    tblStockDetailsTONew.BalanceStock = tblStockDetailsTONew.BalanceStock - totalLoadingQty;

                    if (tblStockDetailsTONew.ProdItemId == 0) {
                        TblProductInfoTO productInfoTO = productList.Where (p => p.MaterialId == tblStockDetailsTONew.MaterialId &&
                            p.ProdCatId == tblStockDetailsTONew.ProdCatId && p.ProdSpecId == tblStockDetailsTONew.ProdSpecId &&
                            p.BrandId == tblStockDetailsTONew.BrandId).FirstOrDefault ();

                        tblStockDetailsTONew.ProductId = productInfoTO.IdProduct;

                        adjustedBundles = totalLoadingQty * 1000 / productInfoTO.AvgBundleWt;

                    } else {
                        adjustedBundles = totalLoadingQty;
                    }

                    tblStockDetailsTONew.NoOfBundles -= adjustedBundles;
                    tblStockDetailsTONew.IsInMT = 1;

                    if (tblStockDetailsTONew.IdStockDtl > 0) {
                        tblStockDetailsTONew.UpdatedBy = loginUserId;
                        tblStockDetailsTONew.UpdatedOn = _iCommon.ServerDateTime;
                        result = _iTblStockDetailsDAO.UpdateTblStockDetails (tblStockDetailsTONew, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While update the stock details";
                            resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while update the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    } else {
                        return resultMessage;
                    }

                    //TblStockSummaryTO mainSummaryTO = _iTblStockSummaryBL.SelectTblStockSummaryTO(tblStockDetailsTONew.StockSummaryId, conn, tran);
                    //mainSummaryTO.NoOfBundles -= adjustedBundles;
                    //mainSummaryTO.TotalStock -= totalLoadingQty;
                    //mainSummaryTO.UpdatedBy = loginUserId;
                    //mainSummaryTO.UpdatedOn = _iCommon.ServerDateTime;

                    //result = _iTblStockSummaryBL.UpdateTblStockSummary(mainSummaryTO, conn, tran);
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

                    //List<TblStockDetailsTO> tblStockDetailsTOList = BL._iTblStockDetailsBL.SelectAllTblStockDetailsList(tblStockSummaryTO.IdStockSummary, conn, tran);
                    //if (tblStockDetailsTOList == null)
                    if (true) {

                        //    stockList.Add(tblStockDetailsTONew);
                        //    stockDtlId = tblStockDetailsTONew.IdStockDtl;
                        //}
                        //else
                        //{
                        //    result = _iTblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTONew, conn, tran);
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

                        TblStockConsumptionTO stockConsumptionTO = new TblStockConsumptionTO ();
                        stockConsumptionTO.BeforeStockQty = tblStockDetailsTONew.BalanceStock + totalLoadingQty;
                        stockConsumptionTO.AfterStockQty = tblStockDetailsTONew.BalanceStock;
                        stockConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
                        stockConsumptionTO.CreatedBy = loginUserId;
                        stockConsumptionTO.CreatedOn = _iCommon.ServerDateTime;
                        stockConsumptionTO.Remark = totalLoadingQty + " Qty is consumed against Loading Slip : " + tblLoadingTO.LoadingSlipNo;
                        stockConsumptionTO.StockDtlId = tblStockDetailsTONew.IdStockDtl;
                        stockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.OUT;
                        stockConsumptionTO.TxnQty = -totalLoadingQty;

                        result = _iTblStockConsumptionDAO.InsertTblStockConsumption (stockConsumptionTO, conn, tran);
                        if (result != 1) {
                            resultMessage.DefaultBehaviour ();
                            resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                            return resultMessage;
                        }

                    }
                }
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method UpdateStockAgainstItem";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                //conn.Close();
            }
        }

        private ResultMessage InsertItemIntoStockDtlAndSummary (TblLoadingTO tblLoadingTO, Double totalLoadingQty, SqlConnection conn, SqlTransaction tran, TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 loginUserId, List<TblLocationTO> tblLocationTOList) {
            Int32 isAllowLoading = 0;
            Int32 stockDtlId = 0;
            ResultMessage resultMessage = new ResultMessage ();
            Int32 result = 0;
            try {
                TblStockDetailsTO tblStockDetailsTONew = new TblStockDetailsTO ();

                //Priyanka[29-10-2018] : Added to check the setting of allow to create loading slip without availability of stock.
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_ALLOW_LOADING_WITHOUT_STOCK, conn, tran);
                if (tblConfigParamsTO != null) {
                    isAllowLoading = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                }
                if (isAllowLoading == 1) {
                    List<TblStockDetailsTO> stockList = _iTblStockDetailsDAO.SelectAllTblStockDetailsOther (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, tblLoadingSlipExtTO.CompartmentId, new DateTime (), conn, tran);
                    String isItemized = "Itemized";

                    stockList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                        l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                        l.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                        l.BrandId == tblLoadingSlipExtTO.BrandId &&
                        l.ProdItemId == tblLoadingSlipExtTO.ProdItemId &&
                        l.LocationId == (tblLoadingSlipExtTO.CompartmentId > 0 ? tblLoadingSlipExtTO.CompartmentId : l.LocationId)).ToList ();

                    List<TblProductInfoTO> productList = _iTblProductInfoDAO.SelectAllLatestProductInfo (conn, tran);

                    if (stockList.Count == 0) {
                        // For weight and Stock in MT calculations
                        tblStockDetailsTONew.ProdCatId = tblLoadingSlipExtTO.ProdCatId;
                        tblStockDetailsTONew.ProdSpecId = tblLoadingSlipExtTO.ProdSpecId;
                        tblStockDetailsTONew.BrandId = tblLoadingSlipExtTO.BrandId;
                        tblStockDetailsTONew.MaterialId = tblLoadingSlipExtTO.MaterialId;
                        tblStockDetailsTONew.CreatedOn = _iCommon.ServerDateTime;
                        tblStockDetailsTONew.CreatedBy = loginUserId;
                        tblStockDetailsTONew.ProdItemId = tblLoadingSlipExtTO.ProdItemId;

                        if (tblLoadingSlipExtTO.CompartmentId > 0) {
                            tblStockDetailsTONew.LocationId = tblLoadingSlipExtTO.CompartmentId;
                        } else {
                            Int32 tempLocatId = 0;

                            var tempLocationLst = tblLocationTOList.Where (w => w.ParentLocId > 0).ToList ();
                            if (tempLocationLst != null && tempLocationLst.Count > 0) {
                                tempLocatId = tempLocationLst[0].IdLocation;
                            }

                            tblStockDetailsTONew.LocationId = tempLocatId;
                        }

                        TblStockSummaryTO tblStockSummaryTO = _iTblStockSummaryDAO.SelectTblStockSummary (new DateTime ());
                        if (tblStockSummaryTO == null) {
                            tblStockSummaryTO = new TblStockSummaryTO ();
                            tblStockSummaryTO.StockDate = _iCommon.ServerDateTime;
                            tblStockSummaryTO.NoOfBundles = 0;
                            tblStockSummaryTO.TotalStock = 0;
                            tblStockSummaryTO.CreatedBy = loginUserId;
                            tblStockSummaryTO.CreatedOn = _iCommon.ServerDateTime;

                            result = _iTblStockSummaryDAO.InsertTblStockSummary (tblStockSummaryTO, conn, tran);
                            if (result != 1) {
                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error : While insert the stock summary";
                                resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock summary for the Size " + tblLoadingSlipExtTO.DisplayName;
                                resultMessage.Result = 0;
                                return resultMessage;
                            }
                        }

                        tblStockDetailsTONew.StockSummaryId = tblStockSummaryTO.IdStockSummary;

                        result = _iTblStockDetailsDAO.InsertTblStockDetails (tblStockDetailsTONew, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error : While insert the stock details";
                            resultMessage.DisplayMessage = "Error : Record Could Not Be Saved. " + isItemized + " exception while insert the stock details for the Size " + tblLoadingSlipExtTO.DisplayName;
                            resultMessage.Result = 0;
                            return resultMessage;
                        }
                    } else {
                        tblStockDetailsTONew = stockList[0];
                    }

                }
                resultMessage.Tag = tblStockDetailsTONew;
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method UpdateStockAgainstItem";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                //conn.Close();
            }
        }

        private Double CalculateFreightAmtPerTon (List<TblLoadingSlipTO> list, Double totalFreightAmt) {
            try {
                Double freightAmt = 0;
                Double totalQtyInMT = 0;

                for (int i = 0; i < list.Count; i++) {
                    totalQtyInMT += list[i].TblLoadingSlipDtlTO.LoadingQty;
                }

                freightAmt = totalFreightAmt / totalQtyInMT;
                freightAmt = Math.Round (freightAmt, 2);
                return freightAmt;
            } catch (Exception ex) {
                return -1;
            } finally {

            }
        }

        public ResultMessage UpdateStockAndConsumptionHistory (TblLoadingSlipExtTO tblLoadingSlipExtTO, TblLoadingTO tblLoadingTO, int stockDtlId, ref Double totalLoadingQty, TblProductInfoTO prodConfgTO, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            int result = 0;
            Double stockQty = 0;

            //Checked from DB To Get Latest Stock Details
            TblStockDetailsTO stockDetailsTO = _iTblStockDetailsDAO.SelectTblStockDetails (stockDtlId, conn, tran);
            if (stockDetailsTO.BalanceStock >= totalLoadingQty) {
                stockQty = totalLoadingQty;
            } else {
                stockQty = stockDetailsTO.BalanceStock;
            }

            TblStockConsumptionTO stockConsumptionTO = new TblStockConsumptionTO ();
            stockConsumptionTO.BeforeStockQty = stockDetailsTO.BalanceStock;
            stockConsumptionTO.AfterStockQty = stockDetailsTO.BalanceStock - stockQty;
            stockConsumptionTO.LoadingSlipExtId = tblLoadingSlipExtTO.IdLoadingSlipExt;
            stockConsumptionTO.CreatedBy = tblLoadingTO.CreatedBy;
            stockConsumptionTO.CreatedOn = tblLoadingTO.CreatedOn;
            stockConsumptionTO.Remark = stockQty + " Qty is consumed against Loading Slip : " + tblLoadingTO.LoadingSlipNo;
            stockConsumptionTO.StockDtlId = stockDetailsTO.IdStockDtl;
            stockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.OUT;
            stockConsumptionTO.TxnQty = -stockQty;

            result = _iTblStockConsumptionDAO.InsertTblStockConsumption (stockConsumptionTO, conn, tran);
            if (result != 1) {
                resultMessage.DefaultBehaviour ();
                resultMessage.Text = "Error : While InsertTblStockConsumption Against LoadingSlip";
                return resultMessage;
            }

            //Update Stock Balance Qty
            stockDetailsTO.BalanceStock = stockConsumptionTO.AfterStockQty;

            stockDetailsTO.TotalStock = stockConsumptionTO.AfterStockQty;

            if (stockDetailsTO.IsConsolidatedStock == 1) {
                stockDetailsTO.NoOfBundles = stockDetailsTO.TotalStock;
            } else {
                if (stockDetailsTO.TotalStock <= 0) {
                    stockDetailsTO.NoOfBundles = 0;
                } else {
                    Double totalStkInMT = stockDetailsTO.TotalStock;
                    totalStkInMT = totalStkInMT * 1000;
                    if (prodConfgTO == null) {
                        stockDetailsTO.NoOfBundles = stockDetailsTO.TotalStock;
                    } else {
                        Double noOfBundles = Math.Round (totalStkInMT / prodConfgTO.NoOfPcs / prodConfgTO.AvgSecWt / prodConfgTO.StdLength, 2);
                        stockDetailsTO.NoOfBundles = noOfBundles;
                    }

                }
            }
            stockDetailsTO.LoadedStock += stockQty;
            stockDetailsTO.UpdatedBy = tblLoadingTO.CreatedBy;
            stockDetailsTO.UpdatedOn = tblLoadingTO.CreatedOn;
            result = _iTblStockDetailsDAO.UpdateTblStockDetails (stockDetailsTO, conn, tran);
            if (result != 1) {
                resultMessage.DefaultBehaviour ();
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
        public int UpdateTblLoading (TblLoadingTO tblLoadingTO) {
            return _iTblLoadingDAO.UpdateTblLoading (tblLoadingTO);
        }

        public int UpdateTblLoading (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            return _iTblLoadingDAO.UpdateTblLoading (tblLoadingTO, conn, tran);
        }
        public ResultMessage UpdateDeliverySlipConfirmations (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                resultMessage = UpdateDeliverySlipConfirmations (tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    return resultMessage;

                }
                tran.Commit ();
                return resultMessage;
            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateDeliverySlipConfirmations");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }
        public ResultMessage UpdateDeliverySlipConfirmations (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            int result = 0;
            //For Cancel it is used.
            Int32 modBusRefId = tblLoadingTO.ModbusRefId;

            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                //Kiran [10-Dec-2018] For IoT Implementations
                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();

                List<TblProductInfoTO> productConfgList = _iTblProductInfoDAO.SelectAllLatestProductInfo (conn, tran);
                if (productConfgList == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ();
                    resultMessage.Text = "Error : productConfgList Found NULL ";
                    resultMessage.DisplayMessage = "Error - 01 : Record Could Not Be Saved. Product Master Configuration is not completed.";
                    return resultMessage;
                }

                TblLoadingTO existingLoadingTO = SelectTblLoadingTO (tblLoadingTO.IdLoading, conn, tran);
                if (existingLoadingTO == null) {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error : existingLoadingTO Found NULL ";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED) {
                    //tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Record could not be updated as selected Loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.DisplayMessage = "Record could not be updated as selected loading is already " + existingLoadingTO.StatusDesc;
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                #region While Delivery OUT check for invoices generated or not.

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED) {
                    //Aniket [19-8-2019] commmnedted for IOT as we are passing loadingId to check invoice is generated against VehicleNo
                    //resultMessage = _iCircularDependencyBL.CheckInvoiceNoGeneratedByVehicleNo(tblLoadingTO.VehicleNo, conn, tran, true);
                    if (weightSourceConfigId != (Int32) Constants.WeighingDataSourceE.IoT) {
                        resultMessage = _iCircularDependencyBL.CheckInvoiceNoGeneratedByVehicleNo (tblLoadingTO.VehicleNo, conn, tran, tblLoadingTO.IdLoading, true);
                        if (resultMessage.MessageType != ResultMessageE.Information) {
                            //tran.Rollback();
                            return resultMessage;
                        }
                    }

                    // Vijaymala [30-03-2018] added:to update invoice deliveredOn date after loading slip out
                    resultMessage = _iTblInvoiceBL.UpdateInvoiceAfterloadingSlipOut (tblLoadingTO.IdLoading, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information) {
                        //tran.Rollback();
                        return resultMessage;
                    }

                }

                #endregion

                #region 0. If User Is Confirming Then Check it can be approve or Not

                //No need to update stock for ODLMS
                //if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM)
                if (false) {
                    resultMessage = CanGivenLoadingSlipBeApproved (tblLoadingTO, conn, tran);
                    if (resultMessage.MessageType == ResultMessageE.Information &&
                        resultMessage.Result == 1) {
                        // Give Stock Effects
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType () == typeof (List<TblLoadingSlipExtTO>)) {
                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = (List<TblLoadingSlipExtTO>) resultMessage.Tag;

                            for (int stk = 0; stk < loadingSlipExtTOList.Count; stk++) {

                                TblLoadingSlipExtTO tblLoadingSlipExtTO = loadingSlipExtTOList[stk];

                                //Check If Stock exist Or Not
                                List<TblStockDetailsTO> stockList = _iTblStockDetailsDAO.SelectAllTblStockDetails (tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingTO.CreatedOn, conn, tran);

                                if (stockList == null) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : stockList Found NULL ";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Result = 0;
                                    return resultMessage;
                                }

                                var prodConfgTO = productConfgList.Where (p => p.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                                    p.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    p.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    p.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault ();

                                if (prodConfgTO == null) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : Product Configuration Not Found For MaterialId:" + tblLoadingSlipExtTO.MaterialId + " AND ProdCat : " + tblLoadingSlipExtTO.ProdCatId + " AND Spec :" + tblLoadingSlipExtTO.ProdSpecId;
                                    resultMessage.DisplayMessage = "Error 01 :" + resultMessage.Text;
                                    return resultMessage;
                                }

                                // Create Stock Consumption History Record
                                var stkConsList = stockList.Where (l => l.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    l.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    l.MaterialId == tblLoadingSlipExtTO.MaterialId).ToList ();

                                Double totalLoadingQty = tblLoadingSlipExtTO.LoadingQty;
                                for (int s = 0; s < stkConsList.Count; s++) {

                                    if (totalLoadingQty > 0) {
                                        resultMessage = UpdateStockAndConsumptionHistory (tblLoadingSlipExtTO, tblLoadingTO, stkConsList[s].IdStockDtl, ref totalLoadingQty, prodConfgTO, conn, tran);
                                        if (resultMessage.MessageType != ResultMessageE.Information) {
                                            tran.Rollback ();
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error : While UpdateStockAndConsumptionHistory Against LoadingSlip Confirmation";
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            return resultMessage;
                                        }
                                    }
                                }
                            }
                        }
                    } else {
                        tran.Rollback ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                }

                #endregion

                #region 1. Stock Calculations If Cancelling Loading
                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {

                    List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO> ();

                    //Hrushikesh added for IOT
                    if (weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT) {
                        GetWeighingMeasuresFromIoT (Convert.ToString (tblLoadingTO.IdLoading), false, tblWeighingMeasuresTOList, conn, tran);
                    } else {
                        tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading);
                    }
                    //end

                    if (tblWeighingMeasuresTOList.Count > 0) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;

                        //Priyanka [16-04-2018] : Added for if the tare weight taken allow to cancel loading slip.

                        Int32 allowToCancelIfTareWT = 0;

                        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_ALLOW_TO_CANCEL_LOADING_IF_TARE_WT_TAKEN, conn, tran);
                        if (tblConfigParamsTO != null) {
                            allowToCancelIfTareWT = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                        }

                        if (allowToCancelIfTareWT == 1) {
                            List<TblWeighingMeasuresTO> tblWeighingMeasuresTOListTare = tblWeighingMeasuresTOList.Where (w => w.WeightMeasurTypeId == (int) Constants.TransMeasureTypeE.TARE_WEIGHT).ToList ();

                            if (tblWeighingMeasuresTOList.Count > tblWeighingMeasuresTOListTare.Count) {
                                resultMessage.Text = "Vehicle Weighing already done can not Cancel";
                                return resultMessage;
                            }

                        } else {

                            if (tblWeighingMeasuresTOList.Count == 1) {
                                resultMessage.Text = "Vehicle Tare weight already done can not Cancel";
                            } else if (tblWeighingMeasuresTOList.Count > 1) {
                                resultMessage.Text = "Vehicle Weighing already done can not Cancel";
                            }
                            return resultMessage;
                        }
                    }

                    //if (tblWeighingMeasuresTOList.Count == 0)
                    if (true) {
                        // tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);

                        //if (tblLoadingTO.LoadingTypeE != Constants.LoadingTypeE.OTHER)
                        if (true) {
                            #region 2.1 Reverse Booking Pending Qty
                            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = _iTblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0) {
                                //tran.Rollback();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                resultMessage.Text = "loadingSlipDtlTOList found null";
                                return resultMessage;
                            }

                            //AmolG[2020-Feb-20] Shifted this call from below
                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading.ToString(), conn, tran);
                            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0)
                            {
                                //tran.Rollback();
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                resultMessage.Text = "loadingSlipExtTOList found null";
                                return resultMessage;
                            }

                            var distinctBookings = loadingSlipDtlTOList.GroupBy (b => b.BookingId).ToList ();
                            for (int i = 0; i < distinctBookings.Count; i++) {
                                Int32 bookingId = distinctBookings[i].Key;
                                Double bookingQty = loadingSlipDtlTOList.Where (b => b.BookingId == bookingId).Sum (l => l.LoadingQty);

                                //Call to update pending booking qty for loading
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                                tblBookingsTO = _iTblBookingsDAO.SelectTblBookings (bookingId, conn, tran);
                                if (tblBookingsTO == null) {
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

                                if (tblBookingsTO.PendingQty < 0) {
                                    //tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                                    return resultMessage;
                                }

                                result = _iTblBookingsDAO.UpdateBookingPendingQty (tblBookingsTO, conn, tran);
                                if (result != 1) {
                                    //tran.Rollback();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }

                                //AmolG[2020-Feb-20] Update Booking balance qty in Ext Table
                                resultMessage = UpdateBookingQty(loadingSlipExtTOList, bookingId, conn, tran);
                                if (resultMessage.MessageType == ResultMessageE.Error)
                                {
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingExtPendingQty";
                                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                    return resultMessage;
                                }
                            }
                            #endregion

                            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                            //AmolG[2020-Feb-20] Move this method Before Booking for loop
                            //List<TblLoadingSlipExtTO> loadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId (tblLoadingTO.IdLoading.ToString (), conn, tran);
                            //if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0) {
                            //    //tran.Rollback();
                            //    resultMessage.DefaultBehaviour ();
                            //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            //    resultMessage.Text = "loadingSlipExtTOList found null";
                            //    return resultMessage;
                            //}

                            //TblLoadingTO existingLoadingTO =SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);
                            
                            for (int i = 0; i < loadingSlipExtTOList.Count; i++) {
                                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;
                                
                                

                                //TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL._iTblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
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

                                //result = BL._iTblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
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
                                //result = BL._iTblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
                                //if (result != 1)
                                //{
                                //    resultMessage.DefaultBehaviour();
                                //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                //    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                                //    return resultMessage;
                                //}

                                // Update Stock i.e reverse stock. If It is confirmed loading slips

                                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_NOT_CONFIRM ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)

                                {

                                    List<TblStockConsumptionTO> tblStockConsumptionTOList = _iTblStockConsumptionDAO.SelectAllStockConsumptionList (loadingSlipExtId, (int) Constants.TxnOperationTypeE.OUT, conn, tran);
                                    if (tblStockConsumptionTOList == null) {
                                        resultMessage.DefaultBehaviour ();
                                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                                        return resultMessage;
                                    }

                                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++) {
                                        Double qtyToReverse = Math.Abs (tblStockConsumptionTOList[s].TxnQty);
                                        TblStockDetailsTO tblStockDetailsTO = _iTblStockDetailsDAO.SelectTblStockDetails (tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                                        if (tblStockDetailsTO == null) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                                        tblStockDetailsTO.TotalStock = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                        result = _iTblStockDetailsDAO.UpdateTblStockDetails (tblStockDetailsTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        // Insert Stock Consumption History Record
                                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO ();
                                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                                        reversedStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;

                                        result = _iTblStockConsumptionDAO.InsertTblStockConsumption (reversedStockConsumptionTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
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
                if (weightSourceConfigId != (int) Constants.WeighingDataSourceE.IoT || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM) {

                    //Update LoadingTO Status First
                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                        tblLoadingTO.ModbusRefId = 0;
                        tblLoadingTO.IsDBup = 0;
                    }
                    result = UpdateTblLoading (tblLoadingTO, conn, tran);
                    if (result != 1) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }

                    //Update Individual Loading Slip statuses
                    result = _iTblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingTO, conn, tran);
                    if (result <= 0) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }
                }
                #endregion

                #region 3. Create History Record
                if (weightSourceConfigId != (int) Constants.WeighingDataSourceE.IoT || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                    TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO ();
                    tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                    tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                    tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                    tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                    result = _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                    if (result != 1) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }
                }
                #endregion

                #region 4. Notifications For Approval Or Information
                //Aniket [6-8-2019] added to set alert and sms dynamically
                List<TblAlertDefinitionTO> tblAlertDefinitionTOList = _iTblAlertDefinitionDAO.SelectAllTblAlertDefinition();
                //Vijaymala added[03-05-2018]to change  notification with party name
                TblConfigParamsTO dealerNameConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);
                Int32 dealerNameActive = 0;
                if (dealerNameConfTO != null)
                    dealerNameActive = Convert.ToInt32 (dealerNameConfTO.ConfigParamVal);

                String dealerOrgNames = String.Empty;
                List<TblLoadingSlipTO> tblLoadingSlipTOList = _iTblLoadingSlipBL.SelectAllTblLoadingSlip (tblLoadingTO.IdLoading, conn, tran);
                if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 1) {
                    List<TblLoadingSlipTO> distinctLoadingSlipList = tblLoadingSlipTOList.GroupBy (w => w.DealerOrgId).Select (s => s.FirstOrDefault ()).ToList ();
                    if (distinctLoadingSlipList != null && distinctLoadingSlipList.Count > 0) {
                        distinctLoadingSlipList.ForEach (f => f.DealerOrgName = f.DealerOrgName.Replace (',', ' '));
                        dealerOrgNames = String.Join (" , ", distinctLoadingSlipList.Select (s => s.DealerOrgName.ToString ()).ToArray ());
                    }

                }

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED ||

                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN ||
                    tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN) {
                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO ();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO> ();

                    List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser (tblLoadingTO.CnfOrgId, conn, tran);
                    if (cnfUserList != null && cnfUserList.Count > 0) {
                        for (int a = 0; a < cnfUserList.Count; a++) {
                            TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO ();
                            tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                            tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                            tblAlertUsersTOList.Add (tblAlertUsersTO);
                        }
                    }

                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM)
                    {
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMED);
                        string tempTxt = "", tempSms = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMED;
                        tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CONFIRMED";
                        //Aniket [6-8-2019] 
                        if (!String.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@LoadingSlipStr", tblLoadingTO.LoadingSlipNo);
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");
                            if (!string.IsNullOrEmpty(tblLoadingTO.VehicleNo))
                                tempTxt = tempTxt.Replace("@VehicleNoStr", tblLoadingTO.VehicleNo);
                            else
                                tempTxt = tempTxt.Replace("@VehicleNoStr", "-");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Loading slip  " + tblLoadingTO.LoadingSlipNo + "  For Vehicle No :" + tblLoadingTO.VehicleNo + "  is approved";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", dealerOrgNames);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            //tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CONFIRMED";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        Dictionary<int, string> cnfDCT = _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
                        if (cnfDCT != null) {
                            foreach (var item in cnfDCT.Keys) {
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = cnfDCT[item];
                                smsTO.SourceTxnDesc = "LOADING_SLIP_CONFIRMED";
                                smsTO.SmsTxt = tblAlertInstanceTO.AlertComment;
                                tblAlertInstanceTO.SmsTOList.Add(smsTO);
                            }
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                        if (result < 0) {
                            //tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }

                    }
                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                    {
                        //Aniket [6-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                        tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@LoadingSlipNoStr", tblLoadingTO.LoadingSlipNo);
                            tempTxt = tempTxt.Replace("@ReasonStr", tblLoadingTO.StatusReason);
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is cancelled due to " + tblLoadingTO.StatusReason;

                        if (dealerNameActive == 1) //Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", dealerOrgNames);

                            tblAlertInstanceTO.SmsComment = tempTxt;
                            //tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                        //SMS is not required for loading slip cancellation. Notification is already sent
                        //Dictionary<int, string> cnfDCT = BL._iTblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
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

                        result = _iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                        if (result < 0) {
                            //tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            resultMessage.Text = "Error While Reseting Prev Alert";
                            return resultMessage;
                        }
                    }
                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)
                    {
                        //Aniket [6-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.VEHICLE_OUT_FOR_DELIVERY);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_OUT_FOR_DELIVERY;
                        tblAlertInstanceTO.AlertAction = "VEHICLE_OUT_FOR_DELIVERY";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@LoadingSlipNoStr", tblLoadingTO.LoadingSlipNo);
                            tempTxt = tempTxt.Replace("@VehicleNoStr", tblLoadingTO.VehicleNo);
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Your Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  of Vehicle No " + tblLoadingTO.VehicleNo + " is out for delivery";
                        //Reshma Added For SMS Template Changes.
                        String AlertComment = "";
                        List<TblBookingsTO> TblBookingsTOList = new List<TblBookingsTO>();
                        TblConfigParamsTO tblConfigParamsTOTemp = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_NOTIFICATIONS);
                        if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                        {
                            Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                            if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                                if (tblLoadingTO != null && tblAlertDefinitionTO != null)
                                {
                                    TblLoadingTO tblLoadingToTemp = SelectLoadingTOWithDetails(tblLoadingTO.IdLoading);
                                    TblConfigParamsTO MessageTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_VEHICLE_OUT);
                                    if (MessageTO != null && !String.IsNullOrEmpty(MessageTO.ConfigParamVal))
                                    {
                                        AlertComment = MessageTO.ConfigParamVal; 
                                        AlertComment = AlertComment.Replace("@Dealer_Name", tblLoadingToTemp.LoadingSlipList[0].DealerOrgName);
                                        AlertComment = AlertComment.Replace("@Date", _iCommon.ServerDateTime.ToString("dd MMMM yyyy"));
                                        List<TblLoadingSlipTO> tblLoadingSlipTempTOList = _iTblLoadingSlipBL.SelectAllTblLoadingSlip(tblLoadingTO.IdLoading, conn, tran);
                                        if (tblLoadingSlipTempTOList != null && tblLoadingSlipTempTOList.Count > 0)
                                        {
                                            List<TblLoadingSlipTO> distinctLoadingSlipList = tblLoadingSlipTOList.GroupBy(w => w.IdLoadingSlip).Select(s => s.FirstOrDefault()).ToList();
                                            if (distinctLoadingSlipList != null && distinctLoadingSlipList.Count > 0)
                                            {
                                                for (int x = 0; x < distinctLoadingSlipList.Count; x++)
                                                {
                                                    TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(distinctLoadingSlipList[x].BookingId, conn, tran);
                                                    if (tblBookingsTO != null)
                                                        TblBookingsTOList.Add(tblBookingsTO);
                                                }
                                            }

                                        }
                                        if (TblBookingsTOList != null && TblBookingsTOList.Count > 0)
                                        {
                                            TblBookingsTO TblBookingsTOTemp = TblBookingsTOList.OrderByDescending(o => o.BookingQty).First();
                                            if (TblBookingsTOTemp != null)
                                            {
                                                AlertComment = AlertComment.Replace("@Order_No", TblBookingsTOTemp.BookingDisplayNo);
                                            }
                                        }
                                        TblOrganizationTO organizationTO = _iTblOrganizationDAO.SelectTblOrganizationTO(tblLoadingTO.FromOrgId);
                                        AlertComment = AlertComment.Replace("@Vehicle_No", tblLoadingTO.VehicleNo);
                                        AlertComment = AlertComment.Replace("@Org_Name", organizationTO.FirmName);
                                    }
                                }
                            if (!string.IsNullOrEmpty(AlertComment))
                            {
                                tblAlertInstanceTO.AlertComment = AlertComment;
                            }
                        }
                        if (dealerNameActive == 1) //Vijaymala added[03-05-2018]
                        {
                            //tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                            // tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";// 
                            tempTxt = tempTxt.Replace("@DealerNameStr", dealerOrgNames);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                        }

                        tblAlertInstanceTO.SourceDisplayId = "VEHICLE_OUT_FOR_DELIVERY";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                        //SMS to Dealer
                        //Aniket [31-7-2019] added to set sms text dynamically
                        // TblAlertDefinitionTO tblAlertDefinitionTO = _iTblAlertDefinitionDAO.SelectTblAlertDefinition((int)NotificationConstants.NotificationsE.VEHICLE_OUT_FOR_DELIVERY, conn, tran);

                        Dictionary<int, string> dealerDCT = _iTblLoadingSlipBL.SelectRegMobileNoDCTForLoadingDealers(tblLoadingTO.IdLoading.ToString(), conn, tran);
                        if (dealerDCT != null)
                        {
                            foreach (var item in dealerDCT.Keys)
                            {
                                TblSmsTO smsTO = new TblSmsTO();
                                smsTO.MobileNo = dealerDCT[item];
                                smsTO.SourceTxnDesc = "VEHICLE_OUT_FOR_DELIVERY";
                                if (!String.IsNullOrEmpty(tblAlertDefinitionTO.DefaultSmsTxt))
                                {
                                    string tempSmsString = tblAlertDefinitionTO.DefaultSmsTxt;
                                    tempSmsString = tempSmsString.Replace("@QtyStr", tblLoadingTO.TotalLoadingQty.ToString());
                                    if (!string.IsNullOrEmpty(tblLoadingTO.VehicleNo))
                                        tempSmsString = tempSmsString.Replace("@TruckNoStr", tblLoadingTO.VehicleNo);
                                    else
                                        tempSmsString = tempSmsString.Replace("@TruckNoStr", "-");
                                    if (!string.IsNullOrEmpty(tblLoadingTO.ContactNo))
                                        tempSmsString = tempSmsString.Replace("@NoStr", tblLoadingTO.ContactNo);
                                    else
                                        tempSmsString = tempSmsString.Replace("@NoStr", "-");

                                    if (!string.IsNullOrEmpty(tblLoadingTO.DriverName))
                                        tempSmsString = tempSmsString.Replace("@NameStr", tblLoadingTO.DriverName);
                                    else
                                        tempSmsString = tempSmsString.Replace("@NameStr", "-");

                                    smsTO.SmsTxt = tempSmsString;
                                }
                                else
                                    smsTO.SmsTxt = "Your Loading Slip Ref. " + tblLoadingTO.LoadingSlipNo + " is out for delivery";
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
                        }

                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                    }

                    //Priyanka [09-10-2018] - Added to send notifications to persons about vehicle status like vehicle
                    //                        gate in, reported, clearance to send in for loading. 

                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN)
                    {
                        //Aniket [6-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.LOADING_GATE_IN);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_GATE_IN;
                        tblAlertInstanceTO.AlertAction = "LOADING_GATE_IN";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@VehicleNoStr", tblLoadingTO.VehicleNo);
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is gate in for loading.";
                        //Reshma Added For SMS Template Changes.
                        String AlertComment = "";
                        TblConfigParamsTO tblConfigParamsTOTemp = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_NOTIFICATIONS);
                        if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                        {
                            Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                            if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                            {
                                TblLoadingTO tblLoadingToTemp =SelectLoadingTOWithDetails(tblLoadingTO.IdLoading) ;
                                TblConfigParamsTO MessageTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_VEHICLE_GATE_IN_SMS_STRING);
                                if (MessageTO != null && !String.IsNullOrEmpty(MessageTO.ConfigParamVal))
                                {
                                    List<TblBookingsTO> TblBookingsTOList = new List<TblBookingsTO>();
                                    if (tblLoadingTO != null)
                                    {
                                        AlertComment = MessageTO.ConfigParamVal;
                                        AlertComment = AlertComment.Replace("@Dealer_Name", tblLoadingToTemp.LoadingSlipList [0].DealerOrgName);
                                        AlertComment = AlertComment.Replace("@date", _iCommon.ServerDateTime.ToString("dd MMMM yyyy"));
                                        List<TblLoadingSlipTO> tblLoadingSlipTempTOList = _iTblLoadingSlipBL.SelectAllTblLoadingSlip(tblLoadingTO.IdLoading, conn, tran);
                                        if (tblLoadingSlipTempTOList != null && tblLoadingSlipTempTOList.Count > 0)
                                        {
                                            List<TblLoadingSlipTO> distinctLoadingSlipList = tblLoadingSlipTOList.GroupBy(w => w.IdLoadingSlip).Select(s => s.FirstOrDefault()).ToList();
                                            if (distinctLoadingSlipList != null && distinctLoadingSlipList.Count > 0)
                                            {
                                                for (int x = 0; x < distinctLoadingSlipList.Count; x++)
                                                {
                                                    TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(distinctLoadingSlipList[x].BookingId, conn, tran);
                                                    if (tblBookingsTO != null)
                                                        TblBookingsTOList.Add(tblBookingsTO);
                                                }
                                            }

                                        }
                                        if (TblBookingsTOList != null && TblBookingsTOList.Count > 0)
                                        {
                                            TblBookingsTO TblBookingsTOTemp = TblBookingsTOList.OrderByDescending(o => o.BookingQty).First();
                                            if (TblBookingsTOTemp != null)
                                            {
                                                AlertComment = AlertComment.Replace("@Order_No", TblBookingsTOTemp.BookingDisplayNo);
                                            }
                                        }
                                        TblOrganizationTO organizationTO = _iTblOrganizationDAO.SelectTblOrganizationTO(tblLoadingTO.FromOrgId);
                                        AlertComment = AlertComment.Replace("@Vehicle_No", tblLoadingTO.VehicleNo);
                                        AlertComment = AlertComment.Replace("@Org_Name", organizationTO.FirmName);
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(AlertComment))
                        {
                            tblAlertInstanceTO.AlertComment = AlertComment;

                        }
                        if (dealerNameActive == 1) //Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", dealerOrgNames);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            // tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_GATE_IN";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }

                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING)
                    {
                        //Aniket [6-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.LOADING_GATE_IN);
                        string tempTxt = "";

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_REPORTED_FOR_LOADING;
                        tblAlertInstanceTO.AlertAction = "VEHICLE_REPORTED_FOR_LOADING";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@VehicleNoStr", tblLoadingTO.VehicleNo);
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is reported for loading.";

                        if (dealerNameActive == 1) //Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", dealerOrgNames);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            //tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "VEHICLE_REPORTED_FOR_LOADING";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }


                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN)
                    {
                        //Aniket [6-8-2019]
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.LOADING_VEHICLE_CLEARANCE_TO_SEND_IN);
                        string tempTxt = "";

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_VEHICLE_CLEARANCE_TO_SEND_IN;
                        tblAlertInstanceTO.AlertAction = "LOADING_VEHICLE_CLEARANCE_TO_SEND_IN";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@VehicleNoStr", tblLoadingTO.VehicleNo);
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Vehicle No " + tblLoadingTO.VehicleNo + " is clear to send in for loading.";

                        if (dealerNameActive == 1) //Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", dealerOrgNames);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            // tblAlertInstanceTO.AlertComment += " (" + dealerOrgNames + ").";//      
                        }

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_VEHICLE_CLEARANCE_TO_SEND_IN";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }
                    else if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED)//Reshma Added For Parmeshwar Changes.
                    { 
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.LOADING_VEHICLE_OUT);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.LOADING_VEHICLE_OUT;
                        tblAlertInstanceTO.AlertAction = "LOADING_VEHICLE_OUT";
                        TblLoadingTO tblLoadingToTemp = SelectLoadingTOWithDetails(tblLoadingTO.IdLoading);
                        //Reshma Added For SMS Template Changes.
                        String AlertComment = "";
                        List<TblBookingsTO> TblBookingsTOList = new List<TblBookingsTO>();
                        if (tblLoadingTO != null && tblAlertDefinitionTO !=null)
                        {
                            TblConfigParamsTO MessageTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_VEHICLE_OUT );
                            if (MessageTO != null && !String.IsNullOrEmpty(MessageTO.ConfigParamVal))
                            {
                                AlertComment = MessageTO.ConfigParamVal ;
                                AlertComment = AlertComment.Replace("@Dealer_Name", tblLoadingToTemp.LoadingSlipList[0].DealerOrgName);
                                AlertComment = AlertComment.Replace("@date", _iCommon.ServerDateTime.ToString("dd MMMM yyyy"));
                                List<TblLoadingSlipTO> tblLoadingSlipTempTOList = _iTblLoadingSlipBL.SelectAllTblLoadingSlip(tblLoadingTO.IdLoading, conn, tran);
                                if (tblLoadingSlipTempTOList != null && tblLoadingSlipTempTOList.Count > 1)
                                {
                                    List<TblLoadingSlipTO> distinctLoadingSlipList = tblLoadingSlipTOList.GroupBy(w => w.IdLoadingSlip).Select(s => s.FirstOrDefault()).ToList();
                                    if (distinctLoadingSlipList != null && distinctLoadingSlipList.Count > 0)
                                    {
                                        for (int x = 0; x < distinctLoadingSlipList.Count; x++)
                                        {
                                            TblBookingsTO tblBookingsTO = _iTblBookingsDAO.SelectTblBookings(distinctLoadingSlipList[x].BookingId, conn, tran);
                                            if (tblBookingsTO != null)
                                                TblBookingsTOList.Add(tblBookingsTO);
                                        }
                                    }

                                }
                                if (TblBookingsTOList != null && TblBookingsTOList.Count > 0)
                                {
                                    TblBookingsTO TblBookingsTOTemp = TblBookingsTOList.OrderByDescending(o => o.BookingQty).First();
                                    if (TblBookingsTOTemp != null)
                                    {
                                        AlertComment = AlertComment.Replace("@Order_No", TblBookingsTOTemp.BookingDisplayNo);
                                    }
                                }
                                TblOrganizationTO organizationTO = _iTblOrganizationDAO.SelectTblOrganizationTO(tblLoadingTO.FromOrgId);
                                AlertComment = AlertComment.Replace("@Vehicle_No", tblLoadingTO.VehicleNo);
                                AlertComment = AlertComment.Replace("@Org_Name", organizationTO.FirmName);
                            }
                        } 
                        if (!string.IsNullOrEmpty(AlertComment))
                        {
                            tblAlertInstanceTO.AlertComment = AlertComment; 
                        } 

                        tblAlertInstanceTO.SourceDisplayId = "LOADING_VEHICLE_OUT";
                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();
                        tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;
                    }
                    tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours (10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                    tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;
                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance (tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information) {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While SaveNewAlertInstance In Method UpdateDeliverySlipConfirmations";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Tag = tblAlertInstanceTO;
                        return resultMessage;
                    }

                }

                #endregion

                #region Kiran [10-Dec-2018] Call To IoT To write the vehicle details 
                if ((weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int) Constants.WeighingDataSourceE.BOTH) && tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM) {
                    //string vehicleNumber = tblLoadingTO.VehicleNo;
                    //int trasporterId = tblLoadingTO.TransporterOrgId;
                    //tblLoadingTO.VehicleNo = string.Empty;
                    //tblLoadingTO.TransporterOrgId = 0;
                    int res = WriteDataOnIOT (tblLoadingTO, conn, tran, tblLoadingTO.VehicleNo, tblLoadingTO.TransporterOrgId);
                    if (res == 0) {
                        tran.Rollback ();
                        resultMessage.MessageType = ResultMessageE.Error;
                        return resultMessage;
                    }
                    tblLoadingTO.TransporterOrgId = 0;
                    tblLoadingTO.VehicleNo = string.Empty;
                    result = UpdateTblLoading (tblLoadingTO, conn, tran);
                    if (result != 1) {
                        tran.Rollback ();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While UpdateTblLoading";
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        return resultMessage;
                    }
                    //Update Individual Loading Slip statuses
                    result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
                    if (result <= 0)
                    {
                        //tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }
                }
                #endregion

                #region 5. Status Update and history to Gate IoT

                if (weightSourceConfigId == (int) Constants.WeighingDataSourceE.IoT || weightSourceConfigId == (int) Constants.WeighingDataSourceE.BOTH) {
                    //if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_IN_PROGRESS
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED
                    //    || tblLoadingTO.TranStatusE == Constants.TranStatusE.INVOICE_GENERATED_AND_READY_FOR_DISPACH
                    //    )
                    //{

                    if (tblLoadingTO.TranStatusE != Constants.TranStatusE.LOADING_CANCEL) {
                        resultMessage = UpdateLoadingStatusToGateIoT (tblLoadingTO, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information) {
                            return resultMessage;
                        }
                    }
                    //}

                    if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {

                        if (weightSourceConfigId == (Int32) Constants.WeighingDataSourceE.IoT) {

                            tblLoadingTO.ModbusRefId = modBusRefId;

                            int deleteResult = RemoveDateFromGateAndWeightIOT (tblLoadingTO);
                            if (deleteResult != 1) {
                                throw new Exception ("Error While RemoveDateFromGateAndWeightIOT ");
                            }
                            //  Startup.AvailableModbusRefList = _iTblLoadingDAO.GeModRefMaxData();
                            //Hrushikesh added for Multitenant changes with IOT
                            //List<int> list = _iTblLoadingDAO.GeModRefMaxData ();
                            //if (list == null)
                            //    throw new Exception ("Failed to get ModbusRefList");
                            //_iModbusRefConfig.setModbusRefList (list);
                            tblLoadingTO.ModbusRefId = 0;
                        }
                    }
                }

                #endregion

                #region 6.Insert Vehicle Ext Details

                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN) {
                    resultMessage = InsertLoadingVehDocExtDetailsAgainstLoading (tblLoadingTO, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                        return resultMessage;
                    }

                }

                #endregion
                #region 7.Create Invoice Receipt
                TblConfigParamsTO tblConfigParamsTOWeighing = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SKIP_WEIGHING, conn, tran);
                if (tblConfigParamsTOWeighing != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOWeighing.ConfigParamVal) == 1)
                    {
                        TblLoadingTO tbloadingTOInvoice = SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);
                        if(tbloadingTOInvoice == null)
                        {
                            resultMessage.DefaultBehaviour("Failed to get loading details for invoice creation");
                            return resultMessage;
                        }
                        List<TblLoadingSlipTO> InvoiceloadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(tbloadingTOInvoice.IdLoading, conn, tran);
                        if (InvoiceloadingSlipTOList == null || InvoiceloadingSlipTOList.Count == 0)
                        {
                            resultMessage.DefaultBehaviour("Failed to get loading slip details for invoice creation");
                            return resultMessage;
                        }
                        for (int m = 0; m < InvoiceloadingSlipTOList.Count; m++)
                        {
                            if (InvoiceloadingSlipTOList[m].LoadingSlipExtTOList == null || InvoiceloadingSlipTOList[m].LoadingSlipExtTOList.Count == 0)
                            {
                                resultMessage.DefaultBehaviour("Failed to get loading slip ext details for invoice creation");
                                return resultMessage;
                            }
                            TblLoadingSlipExtTO TblLoadingSlipExtTOInvoice = new TblLoadingSlipExtTO();
                            for (int n = 0; n < InvoiceloadingSlipTOList[m].LoadingSlipExtTOList.Count; n++)
                            {
                                InvoiceloadingSlipTOList[m].LoadingSlipExtTOList[n].LoadedWeight = InvoiceloadingSlipTOList[m].LoadingSlipExtTOList[n].LoadingQty * 1000;
                                TblLoadingSlipExtTOInvoice = InvoiceloadingSlipTOList[m].LoadingSlipExtTOList[n];
                                result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt(TblLoadingSlipExtTOInvoice, conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error : While UpdateTblLoadingSlipExt Against LoadingSlip");
                                    return resultMessage;
                                }
                            }
                        }
                        resultMessage = _iTblInvoiceBL.CreateInvoiceAgainstLoadingSlips(tbloadingTOInvoice, conn, tran, InvoiceloadingSlipTOList);
                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                    }
                }
                #endregion
                //tran.Commit();
                if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled.";
                    resultMessage.DisplayMessage = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled.";
                    resultMessage.Result = 1;
                    resultMessage.Tag = tblLoadingTO;
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Text = "Loading Slip Approved Sucessfully";
                    resultMessage.DisplayMessage = "Loading Slip Approved Sucessfully";
                    resultMessage.Result = 1;
                    resultMessage.Tag = tblLoadingTO;
                    return resultMessage;
                }
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod UpdateDeliverySlipConfirmations";
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            }

        }

        /// <summary>
        /// AmolG[2020-Feb-20] Update Booking pending qty
        /// </summary>
        /// <param name="loadingSlipExtTOList"></param>
        /// <param name="bookingId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private ResultMessage UpdateBookingQty(List<TblLoadingSlipExtTO> loadingSlipExtTOList, int bookingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            try
            {
                resultMessage.MessageType = ResultMessageE.Error;
                int result = 0;
                var localList = from lst in loadingSlipExtTOList
                                where lst.BookingId == bookingId
                                select lst;

                if (localList != null && localList.Any())
                {
                    List<TblLoadingSlipExtTO> loadingSlipExtTOLocalList = localList.ToList();

                    List<TblBookingExtTO> tblBookingExtTOList = _iTblBookingExtDAO.SelectAllTblBookingExt(bookingId, conn, tran);

                    for (int i = 0; i < loadingSlipExtTOLocalList.Count; i++)
                    {
                        var exist = from lst in tblBookingExtTOList
                                    where lst.BrandId == loadingSlipExtTOLocalList[i].BrandId
                                    && lst.ProdCatId == loadingSlipExtTOLocalList[i].ProdCatId
                                    && lst.ProdItemId == loadingSlipExtTOLocalList[i].ProdItemId
                                    && lst.ProdSpecId == loadingSlipExtTOLocalList[i].ProdSpecId
                                    && lst.MaterialId == loadingSlipExtTOLocalList[i].MaterialId
                                    select lst;

                        if (exist != null && exist.Any())
                        {
                            List<TblBookingExtTO> tblBookingExtTOLocalList = null;
                            double toBeAdjQty = loadingSlipExtTOLocalList[i].LoadingQty;
                            if (toBeAdjQty > 0)
                            {
                                tblBookingExtTOLocalList = exist.OrderByDescending(x => x.IdBookingExt).ToList();
                            }
                            else
                                tblBookingExtTOLocalList = exist.OrderBy(x => x.IdBookingExt).ToList();

                            for (int n = 0; n < tblBookingExtTOLocalList.Count; n++)
                            {
                                TblBookingExtTO tblBookingExtTO = tblBookingExtTOLocalList[n];

                                if (toBeAdjQty > 0)
                                {
                                    if (tblBookingExtTO.BalanceQty < tblBookingExtTO.BookedQty)
                                    {
                                        double balQty = tblBookingExtTO.BookedQty - tblBookingExtTO.BalanceQty;

                                        if (balQty <= toBeAdjQty)
                                        {
                                            tblBookingExtTO.BalanceQty = tblBookingExtTO.BookedQty;
                                            toBeAdjQty -= balQty;
                                        }
                                        else
                                        {
                                            tblBookingExtTO.BalanceQty += toBeAdjQty;
                                            toBeAdjQty = 0;
                                        }

                                        result = _iTblBookingExtDAO.UpdateTblBookingExtBalanceQty(tblBookingExtTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While Update Booking Balance Qty In Method UpdateTblBookingExtBalanceQty";
                                            return resultMessage;
                                        }

                                        if (toBeAdjQty == 0)
                                            break;
                                    }
                                }
                                else
                                {
                                    if (tblBookingExtTO.BalanceQty > 0)
                                    {
                                        double absQty = Math.Abs(toBeAdjQty);
                                        if(tblBookingExtTO.BalanceQty > absQty)
                                        {
                                            tblBookingExtTO.BalanceQty += toBeAdjQty;
                                            toBeAdjQty = 0;
                                        }
                                        else
                                        {
                                            toBeAdjQty += tblBookingExtTO.BalanceQty;
                                            tblBookingExtTO.BalanceQty = 0;
                                        }

                                        result = _iTblBookingExtDAO.UpdateTblBookingExtBalanceQty(tblBookingExtTO, conn, tran);
                                        if (result <= 0)
                                        {
                                            resultMessage.MessageType = ResultMessageE.Error;
                                            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                                            resultMessage.Text = "Error While Update Booking Balance Qty In Method UpdateTblBookingExtBalanceQty";
                                            return resultMessage;
                                        }

                                        if (toBeAdjQty == 0)
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        //Aniket [30-7-2019] added for IOT
        private  int RemoveDateFromGateAndWeightIOT(TblLoadingTO tblLoadingTO)
        {
            //Addes by kiran for retry 3 times to delete All Data
            int cnt = 0;
            GateIoTResult result = new GateIoTResult ();
            while (cnt < 3) {
                result = _iGateCommunication.DeleteSingleLoadingFromGateIoT (tblLoadingTO);
                if (result.Code == 1) {
                    break;
                }
                Thread.Sleep (200);
                cnt++;
            }
            if (result.Code != 1) {
                return 0;
            }
            int cnt2 = 0;
            NodeJsResult nodeJsResult = new NodeJsResult ();
            while (cnt2 < 3) {
                nodeJsResult = _iIotCommunication.DeleteSingleLoadingFromWeightIoTByModBusRefId (tblLoadingTO);
                if (nodeJsResult.Code == 1) {
                    break;
                }
                Thread.Sleep (200);
                cnt2++;
            }
            if (nodeJsResult.Code != 1) {
                return 0;
            }
            return 1;
        }

        public ResultMessage InsertLoadingVehDocExtDetailsAgainstLoading (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                resultMessage = InsertLoadingVehDocExtDetailsAgainstLoading (tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    return resultMessage;

                }
                tran.Commit ();
                return resultMessage;
            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "InsertLoadingVehDocExtDetailsAgainstLoading");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        private ResultMessage InsertLoadingVehDocExtDetailsAgainstLoading (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();
            int result;

            if (tblLoadingTO.LoadingVehDocExtTOList != null && tblLoadingTO.LoadingVehDocExtTOList.Count > 0) {
                tblLoadingTO.LoadingVehDocExtTOList[0].IsActive = 0;

                tblLoadingTO.LoadingVehDocExtTOList[0].LoadingId = tblLoadingTO.IdLoading;
                tblLoadingTO.LoadingVehDocExtTOList[0].CreatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingTO.LoadingVehDocExtTOList[0].CreatedOn = tblLoadingTO.UpdatedOn;
                tblLoadingTO.LoadingVehDocExtTOList[0].UpdatedBy = tblLoadingTO.UpdatedBy;
                tblLoadingTO.LoadingVehDocExtTOList[0].UpdatedOn = tblLoadingTO.UpdatedOn;

                result = _iTblLoadingVehDocExtBL.UpdateTblLoadingVehDocExtActiveYn (tblLoadingTO.LoadingVehDocExtTOList[0], conn, tran);
                if (result == -1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ();
                    resultMessage.Text = "Error While updating the UpdateTblLoadingVehDocExtActiveYn";
                    resultMessage.DisplayMessage = "Error 02:" + resultMessage.Text;
                    return resultMessage;
                }

                for (int v = 0; v < tblLoadingTO.LoadingVehDocExtTOList.Count; v++) {
                    tblLoadingTO.LoadingVehDocExtTOList[v].LoadingId = tblLoadingTO.IdLoading;
                    tblLoadingTO.LoadingVehDocExtTOList[v].CreatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingTO.LoadingVehDocExtTOList[v].CreatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingTO.LoadingVehDocExtTOList[v].UpdatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingTO.LoadingVehDocExtTOList[v].UpdatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingTO.LoadingVehDocExtTOList[v].IsActive = 1;
                }

                tblLoadingTO.LoadingVehDocExtTOList = tblLoadingTO.LoadingVehDocExtTOList.Where (w => w.IsAvailable == 1).ToList ();

                result = _iTblLoadingVehDocExtBL.InsertTblLoadingVehDocExt (tblLoadingTO.LoadingVehDocExtTOList, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ();
                    resultMessage.Text = "Error While Inserting the InsertTblLoadingVehDocExt";
                    resultMessage.DisplayMessage = "Error 03:" + resultMessage.Text;
                    return resultMessage;
                }
            }
            resultMessage.DefaultSuccessBehaviour ();
            return resultMessage;
        }

        public ResultMessage RestorePreviousStatusForLoading (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
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
                DimStatusTO statusTO = _iDimStatusDAO.SelectDimStatus(tblLoadingTO.StatusId, conn, tran);
                if (statusTO == null || statusTO.PrevStatusId == 0)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error...statusTO Found NULL In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }

                int configId = _iTblConfigParamsDAO.IoTSetting();
                if (configId == (Int32)Constants.WeighingDataSourceE.IoT)
                {
                    statusTO = _iDimStatusDAO.SelectDimStatus(statusTO.PrevStatusId, conn, tran);
                    if (statusTO == null || statusTO.IotStatusId == 0)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("iot status id not found for loading to pass at gate iot");
                        return resultMessage;
                    }

                    existingLoadingTO.StatusId = (Int32)Constants.TranStatusE.LOADING_CONFIRM;
                    existingLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CONFIRM;
                    existingLoadingTO.StatusReason = "Loading Scheduled";
                    List<object[]> frameList = _iIotCommunication.GenerateGateIoTStatusFrameData(existingLoadingTO, statusTO.IotStatusId);
                    if (frameList != null && frameList.Count > 0)
                    {
                        for (int f = 0; f < frameList.Count; f++)
                        {
                            result = _iIotCommunication.UpdateLoadingStatusOnGateAPIToModbusTcpApi(tblLoadingTO, frameList[f]);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error while PostGateAPIDataToModbusTcpApi");
                                return resultMessage;
                            }
                        }
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour("frameList Found Null Or Empty while PostGateAPIDataToModbusTcpApi");
                        return resultMessage;
                    }
                }
                else
                {
                    existingLoadingTO.StatusId = statusTO.PrevStatusId;
                }
                   
                #region 2. Update Loading Slip Status
                //Update LoadingTO Status First
                result = UpdateTblLoading (existingLoadingTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }

                //Update Individual Loading Slip statuses
                result = _iTblLoadingSlipBL.UpdateTblLoadingSlip (existingLoadingTO, conn, tran);
                if (result <= 0) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoadingSlip In Method RestorePreviousStatusForLoading";
                    return resultMessage;
                }
                #endregion

                #region 3. Create History Record
                if (configId != (Int32)Constants.WeighingDataSourceE.IoT)
                {
                    TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO();
                    tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                    tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                    tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                    tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                    tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                    tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason + " Reversed";
                    result = _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.MessageType = ResultMessageE.Error;
                        resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                        return resultMessage;
                    }
                }

                #endregion

                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod RestorePreviousStatusForLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage CancelAllNotConfirmedLoadingSlips () {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_SYTEM_ADMIN_USER_ID, conn, tran);
                Int32 sysAdminUserId = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;

                #region 1. Loading Slip Cancellation

                TblConfigParamsTO cancelConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_LOADING_SLIPS_AUTO_CANCEL_STATUS_IDS, conn, tran);
                List<TblLoadingTO> loadingTOListToCancel = _iTblLoadingDAO.SelectAllLoadingListByStatus (cancelConfigParamsTO.ConfigParamVal, conn, tran, 0);

                if (loadingTOListToCancel != null) {

                    for (int ic = 0; ic < loadingTOListToCancel.Count; ic++) {
                        TblLoadingTO tblLoadingTO = loadingTOListToCancel[ic];
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_CANCEL;
                        tblLoadingTO.UpdatedBy = sysAdminUserId;
                        tblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                        tblLoadingTO.StatusDate = tblLoadingTO.UpdatedOn;
                        tblLoadingTO.StatusReason = "No Actions - Auto Cancelled";

                        #region 1. Stock Calculations If Cancelling Loading
                        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                            #region 2.1 Reverse Booking Pending Qty

                            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = _iTblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromLoadingId (tblLoadingTO.IdLoading, conn, tran);
                            if (loadingSlipDtlTOList == null || loadingSlipDtlTOList.Count == 0) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.Text = "loadingSlipDtlTOList found null";
                                return resultMessage;
                            }

                            var distinctBookings = loadingSlipDtlTOList.GroupBy (b => b.BookingId).ToList ();
                            for (int i = 0; i < distinctBookings.Count; i++) {
                                Int32 bookingId = distinctBookings[i].Key;
                                Double bookingQty = loadingSlipDtlTOList.Where (b => b.BookingId == bookingId).Sum (l => l.LoadingQty);

                                //Call to update pending booking qty for loading
                                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                                tblBookingsTO = _iTblBookingsDAO.SelectTblBookings (bookingId, conn, tran);
                                if (tblBookingsTO == null) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                                    return resultMessage;
                                }

                                tblBookingsTO.IdBooking = bookingId;
                                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                                tblBookingsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblBookingsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                if (tblBookingsTO.PendingQty < 0) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : tblBookingsTO.PendingQty gone less than 0";
                                    return resultMessage;
                                }

                                result = _iTblBookingsDAO.UpdateBookingPendingQty (tblBookingsTO, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error : While UpdateBookingPendingQty Against Booking";
                                    return resultMessage;
                                }
                            }

                            #endregion

                            #region 2.2 Reverse Loading Quota Consumed , Stock and Mark a history Record

                            List<TblLoadingSlipExtTO> loadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId (tblLoadingTO.IdLoading.ToString (), conn, tran);
                            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ();
                                resultMessage.Text = "loadingSlipExtTOList found null";
                                return resultMessage;
                            }

                            TblLoadingTO existingLoadingTO = SelectTblLoadingTO (tblLoadingTO.IdLoading, conn, tran);

                            for (int i = 0; i < loadingSlipExtTOList.Count; i++) {
                                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                                TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = _iTblLoadingQuotaDeclarationDAO.SelectTblLoadingQuotaDeclaration (loadingQuotaId, conn, tran);
                                if (tblLoadingQuotaDeclarationTO == null) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "tblLoadingQuotaDeclarationTO found null";
                                    return resultMessage;
                                }

                                // Update Loading Quota For Balance Qty
                                Double balanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                tblLoadingQuotaDeclarationTO.BalanceQuota += quotaQty;
                                tblLoadingQuotaDeclarationTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                tblLoadingQuotaDeclarationTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                result = _iTblLoadingQuotaDeclarationDAO.UpdateTblLoadingQuotaDeclaration (tblLoadingQuotaDeclarationTO, conn, tran);
                                if (result != 1) {
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error While UpdateTblLoadingQuotaDeclaration While Cancelling Loading Slip";
                                    return resultMessage;
                                }

                                //History Record For Loading Quota consumptions
                                TblLoadingQuotaConsumptionTO consumptionTO = new Models.TblLoadingQuotaConsumptionTO ();
                                consumptionTO.AvailableQuota = balanceQuota;
                                consumptionTO.BalanceQuota = tblLoadingQuotaDeclarationTO.BalanceQuota;
                                consumptionTO.CreatedBy = tblLoadingQuotaDeclarationTO.UpdatedBy;
                                consumptionTO.CreatedOn = tblLoadingQuotaDeclarationTO.UpdatedOn;
                                consumptionTO.LoadingQuotaId = tblLoadingQuotaDeclarationTO.IdLoadingQuota;
                                consumptionTO.LoadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                                consumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;
                                consumptionTO.QuotaQty = quotaQty;
                                consumptionTO.Remark = "Quota reversed after loading slip is cancelled : - " + tblLoadingTO.LoadingSlipNo;
                                result = _iTblLoadingQuotaConsumptionDAO.InsertTblLoadingQuotaConsumption (consumptionTO, conn, tran);
                                if (result != 1) {
                                    resultMessage.DefaultBehaviour ();
                                    resultMessage.Text = "Error : While InsertTblLoadingQuotaConsumption Against LoadingSlip Cancellation";
                                    return resultMessage;
                                }

                                // Update Stock i.e reverse stock. If It is confirmed loading slips

                                if (existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CONFIRM ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_COMPLETED ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_DELIVERED ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_GATE_IN ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_REPORTED_FOR_LOADING ||
                                    existingLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_VEHICLE_CLERANCE_TO_SEND_IN
                                ) {

                                    List<TblStockConsumptionTO> tblStockConsumptionTOList = _iTblStockConsumptionDAO.SelectAllStockConsumptionList (loadingSlipExtId, (int) Constants.TxnOperationTypeE.OUT, conn, tran);
                                    if (tblStockConsumptionTOList == null) {
                                        resultMessage.DefaultBehaviour ();
                                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                                        return resultMessage;
                                    }

                                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++) {
                                        Double qtyToReverse = Math.Abs (tblStockConsumptionTOList[s].TxnQty);
                                        TblStockDetailsTO tblStockDetailsTO = _iTblStockDetailsDAO.SelectTblStockDetails (tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                                        if (tblStockDetailsTO == null) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        double prevStockQty = tblStockDetailsTO.BalanceStock;
                                        tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                                        tblStockDetailsTO.UpdatedBy = tblLoadingTO.UpdatedBy;
                                        tblStockDetailsTO.UpdatedOn = tblLoadingTO.UpdatedOn;

                                        result = _iTblStockDetailsDAO.UpdateTblStockDetails (tblStockDetailsTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
                                            resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                                            return resultMessage;
                                        }

                                        // Insert Stock Consumption History Record
                                        TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO ();
                                        reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                                        reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                                        reversedStockConsumptionTO.CreatedBy = tblLoadingTO.UpdatedBy;
                                        reversedStockConsumptionTO.CreatedOn = tblLoadingTO.UpdatedOn;
                                        reversedStockConsumptionTO.LoadingSlipExtId = loadingSlipExtId;
                                        reversedStockConsumptionTO.Remark = "Loading Slip No :" + tblLoadingTO.LoadingSlipNo + " is cancelled and Stock is reversed";
                                        reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                                        reversedStockConsumptionTO.TxnQty = qtyToReverse;
                                        reversedStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;

                                        result = _iTblStockConsumptionDAO.InsertTblStockConsumption (reversedStockConsumptionTO, conn, tran);
                                        if (result != 1) {
                                            resultMessage.DefaultBehaviour ();
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
                        result = UpdateTblLoading (tblLoadingTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoading In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }

                        //Update Individual Loading Slip statuses
                        result = _iTblLoadingSlipBL.UpdateTblLoadingSlip (tblLoadingTO, conn, tran);
                        if (result <= 0) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While UpdateTblLoadingSlip In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }
                        #endregion

                        #region 3. Create History Record

                        TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO = new TblLoadingStatusHistoryTO ();
                        tblLoadingStatusHistoryTO.CreatedBy = tblLoadingTO.UpdatedBy;
                        tblLoadingStatusHistoryTO.CreatedOn = tblLoadingTO.UpdatedOn;
                        tblLoadingStatusHistoryTO.LoadingId = tblLoadingTO.IdLoading;
                        tblLoadingStatusHistoryTO.StatusDate = tblLoadingTO.StatusDate;
                        tblLoadingStatusHistoryTO.StatusId = tblLoadingTO.StatusId;
                        tblLoadingStatusHistoryTO.StatusRemark = tblLoadingTO.StatusReason;
                        result = _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory (tblLoadingStatusHistoryTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblLoadingStatusHistory In Method UpdateDeliverySlipConfirmations";
                            return resultMessage;
                        }

                        #endregion

                        #region 4. Notifications For Approval Or Information
                        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL) {
                            TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO ();
                            List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO> ();

                            List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser (tblLoadingTO.CnfOrgId, conn, tran);
                            if (cnfUserList != null && cnfUserList.Count > 0) {
                                for (int a = 0; a < cnfUserList.Count; a++) {
                                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO ();
                                    tblAlertUsersTO.UserId = cnfUserList[a].IdUser;
                                    tblAlertUsersTO.DeviceId = cnfUserList[a].RegisteredDeviceId;
                                    tblAlertUsersTOList.Add (tblAlertUsersTO);
                                }
                            }

                            tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.LOADING_SLIP_CANCELLED;
                            tblAlertInstanceTO.AlertAction = "LOADING_SLIP_CANCELLED";
                            tblAlertInstanceTO.AlertComment = "Your Generated Loading Slip (Ref " + tblLoadingTO.LoadingSlipNo + ")  is auto cancelled ";
                            tblAlertInstanceTO.SourceDisplayId = "LOADING_SLIP_CANCELLED";
                            tblAlertInstanceTO.SmsTOList = new List<TblSmsTO> ();

                            //SMS Not required in auto cancellation. Discussed in meeting 
                            //Dictionary<int, string> cnfDCT = BL._iTblOrganizationBL.SelectRegisteredMobileNoDCT(tblLoadingTO.CnfOrgId.ToString(), conn, tran);
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

                            result = _iTblAlertInstanceBL.ResetAlertInstance ((int) NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, 0, conn, tran);
                            if (result < 0) {
                                tran.Rollback ();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While Reseting Prev Alert";
                                return resultMessage;
                            }

                            tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                            tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours (10);
                            tblAlertInstanceTO.IsActive = 1;
                            tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                            tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                            tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                            tblAlertInstanceTO.IsAutoReset = 1;
                            ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance (tblAlertInstanceTO, conn, tran);
                            if (rMessage.MessageType != ResultMessageE.Information) {
                                tran.Rollback ();
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

                //TblConfigParamsTO postponeConfigParamsTO = BL._iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_SLIPS_AUTO_POSTPONED_STATUS_ID, conn, tran);
                //List<TblLoadingTO> loadingTOListToPostpone = _iTblLoadingDAO.SelectAllLoadingListByStatus(postponeConfigParamsTO.ConfigParamVal, conn, tran);

                //if (loadingTOListToPostpone == null)
                //{

                //    for (int ic = 0; ic < loadingTOListToPostpone.Count; ic++)
                //    {
                //        TblLoadingTO tblLoadingTO = loadingTOListToPostpone[ic];
                //        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_POSTPONED;
                //        tblLoadingTO.UpdatedBy = sysAdminUserId;
                //        tblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                //        tblLoadingTO.StatusDate = tblLoadingTO.UpdatedOn;
                //        tblLoadingTO.StatusReason = "No Actions - Auto Postponed For Tommorow";

                //        #region 1. Stock Calculations If Cancelling Loading
                //        if (tblLoadingTO.TranStatusE == Constants.TranStatusE.LOADING_CANCEL)
                //        {
                //            #region 2.1 Reverse Booking Pending Qty

                //            List<TblLoadingSlipDtlTO> loadingSlipDtlTOList = BL._iTblLoadingSlipDtlBL.SelectAllLoadingSlipDtlListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
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
                //                tblBookingsTO = BL._iTblBookingsBL.SelectTblBookingsTO(bookingId, conn, tran);
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

                //                result = BL._iTblBookingsBL.UpdateBookingPendingQty(tblBookingsTO, conn, tran);
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

                //            List<TblLoadingSlipExtTO> loadingSlipExtTOList = BL._iTblLoadingSlipExtBL.SelectAllLoadingSlipExtListFromLoadingId(tblLoadingTO.IdLoading, conn, tran);
                //            if (loadingSlipExtTOList == null || loadingSlipExtTOList.Count == 0)
                //            {
                //                tran.Rollback();
                //                resultMessage.DefaultBehaviour();
                //                resultMessage.Text = "loadingSlipExtTOList found null";
                //                return resultMessage;
                //            }

                //            TblLoadingTO existingLoadingTO =SelectTblLoadingTO(tblLoadingTO.IdLoading, conn, tran);

                //            for (int i = 0; i < loadingSlipExtTOList.Count; i++)
                //            {
                //                Int32 loadingSlipExtId = loadingSlipExtTOList[i].IdLoadingSlipExt;
                //                Int32 loadingQuotaId = loadingSlipExtTOList[i].LoadingQuotaId;
                //                Double quotaQty = loadingSlipExtTOList[i].LoadingQty;

                //                TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO = BL._iTblLoadingQuotaDeclarationBL.SelectTblLoadingQuotaDeclarationTO(loadingQuotaId, conn, tran);
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

                //                result = BL._iTblLoadingQuotaDeclarationBL.UpdateTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
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
                //                result = BL._iTblLoadingQuotaConsumptionBL.InsertTblLoadingQuotaConsumption(consumptionTO, conn, tran);
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

                //                    List<TblStockConsumptionTO> tblStockConsumptionTOList = BL._iTblStockConsumptionBL.SelectAllStockConsumptionList(loadingSlipExtId, (int)Constants.TxnOperationTypeE.OUT, conn, tran);
                //                    if (tblStockConsumptionTOList == null)
                //                    {
                //                        resultMessage.DefaultBehaviour();
                //                        resultMessage.Text = "tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                //                        return resultMessage;
                //                    }

                //                    for (int s = 0; s < tblStockConsumptionTOList.Count; s++)
                //                    {
                //                        Double qtyToReverse = Math.Abs(tblStockConsumptionTOList[s].TxnQty);
                //                        TblStockDetailsTO tblStockDetailsTO = BL._iTblStockDetailsBL.SelectTblStockDetailsTO(tblStockConsumptionTOList[s].StockDtlId, conn, tran);
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

                //                        result = BL._iTblStockDetailsBL.UpdateTblStockDetails(tblStockDetailsTO, conn, tran);
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

                //                        result = BL._iTblStockConsumptionBL.InsertTblStockConsumption(reversedStockConsumptionTO, conn, tran);
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
                //        result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(tblLoadingTO, conn, tran);
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
                //        result = _iTblLoadingStatusHistoryBL.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
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

                //            List<TblUserTO> cnfUserList = BL._iTblUserBL.SelectAllTblUserList(tblLoadingTO.CnfOrgId, conn, tran);
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

                //            result = BL._iTblAlertInstanceBL.ResetAlertInstance((int)NotificationConstants.NotificationsE.LOADING_SLIP_CONFIRMATION_REQUIRED, tblLoadingTO.IdLoading, conn, tran);
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
                //            ResultMessage rMessage = BL._iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
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

                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod CancelAllNotConfirmedLoadingSlips";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage CanGivenLoadingSlipBeApproved (TblLoadingTO tblLoadingTO) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage ();

            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                return CanGivenLoadingSlipBeApproved (tblLoadingTO, conn, tran);
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Text = "Loading Slip Can Not Be Approve";
                resultMessage.Exception = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage CanGivenLoadingSlipBeApproved (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();

            try {

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllLoadingSlipExtListFromLoadingId (tblLoadingTO.IdLoading.ToString (), conn, tran);
                if (tblLoadingSlipExtTOList == null) {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error. Loading Material Not Found";
                    return resultMessage;
                }

                var loadingSlipExtIds = string.Join (",", tblLoadingSlipExtTOList.Select (p => p.IdLoadingSlipExt.ToString ()));
                loadingSlipExtIds = loadingSlipExtIds.TrimEnd (',');
                List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList = _iTblLoadingQuotaDeclarationDAO.SelectAllLoadingQuotaDeclListFromLoadingExt (loadingSlipExtIds, conn, tran);
                if (loadingQuotaDeclarationTOList == null) {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error. Loading Quota Not Found";
                    return resultMessage;
                }

                var loadingQuotaIds = string.Join (",", loadingQuotaDeclarationTOList.Select (p => p.IdLoadingQuota.ToString ()));
                loadingQuotaIds = loadingQuotaIds.TrimEnd (',');

                var listToCheck = tblLoadingSlipExtTOList.Where (q => q.QuotaAfterLoading < 0).ToList ().GroupBy (a => new { a.ProdCatId, a.ProdCatDesc, a.ProdSpecId, a.ProdSpecDesc, a.MaterialId, a.MaterialDesc }).Select (a => new { ProdCatId = a.Key.ProdCatId, ProdCatDesc = a.Key.ProdCatDesc, ProdSpecId = a.Key.ProdSpecId, ProdSpecDesc = a.Key.ProdSpecDesc, MaterialId = a.Key.MaterialId, MaterialDesc = a.Key.MaterialDesc, LoadingQty = a.Sum (acs => acs.LoadingQty) }).ToList ();

                if (listToCheck != null || listToCheck.Count > 0) {
                    Dictionary<Int32, Double> loadingQtyDCT = new Dictionary<int, double> ();

                    loadingQtyDCT = _iTblLoadingSlipExtDAO.SelectLoadingQuotaWiseApprovedLoadingQtyDCT (loadingQuotaIds, conn, tran);
                    Boolean isAllowed = true;
                    String reason = "Not Enough Quota For Following Items" + Environment.NewLine;
                    for (int i = 0; i < listToCheck.Count; i++) {
                        //TblLoadingSlipExtTO tblLoadingSlipExtTO = listToCheck[i];

                        var loadingQuotaDeclarationTO = loadingQuotaDeclarationTOList.Where (l => l.ProdCatId == listToCheck[i].ProdCatId &&
                            l.ProdSpecId == listToCheck[i].ProdSpecId &&
                            l.MaterialId == listToCheck[i].MaterialId).FirstOrDefault ();

                        if (loadingQuotaDeclarationTO.IsActive == 0) {
                            if (isAllowed) {
                                isAllowed = false;
                            }
                            reason += listToCheck[i].MaterialDesc + " " + listToCheck[i].ProdCatDesc + "-" + listToCheck[i].ProdSpecDesc + " R.Q. :" + listToCheck[i].LoadingQty + " has inactive loading quota" + Environment.NewLine;

                        }

                        Double approvedLoadingQty = 0;
                        Double transferedQty = loadingQuotaDeclarationTO.TransferedQuota;
                        Double totalAvailableQty = loadingQuotaDeclarationTO.AllocQuota + loadingQuotaDeclarationTO.ReceivedQuota;
                        if (loadingQtyDCT != null && loadingQtyDCT.ContainsKey (loadingQuotaDeclarationTO.IdLoadingQuota))
                            approvedLoadingQty = loadingQtyDCT[loadingQuotaDeclarationTO.IdLoadingQuota];

                        Double pendingQty = totalAvailableQty - transferedQty - approvedLoadingQty;

                        if (listToCheck[i].LoadingQty > pendingQty) {
                            if (isAllowed) {
                                isAllowed = false;
                            }
                            reason += listToCheck[i].MaterialDesc + " " + listToCheck[i].ProdCatDesc + "-" + listToCheck[i].ProdSpecDesc + " R.Q. :" + listToCheck[i].LoadingQty + " AND A.Q. :" + pendingQty + Environment.NewLine;
                        }
                    }

                    if (!isAllowed) {
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
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Text = "Loading Slip Can Not Be Approve";
                resultMessage.Exception = ex;
                return resultMessage;
            }
        }

        public ResultMessage updateLaodingToCallFlag (TblLoadingTO tblLoadingTO) {

            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_SYTEM_ADMIN_USER_ID, conn, tran);
                Int32 sysAdminUserId = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                DateTime cancellationDateTime = DateTime.MinValue;
                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Updated Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = tblLoadingTO;
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod RestorePreviousStatusForLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        /// <summary>
        /// GJ@20171107 : check the Vehicle is complete the all material weight
        /// </summary>
        /// <param name="tblLoadingTO"></param>
        /// <returns></returns>
        public ResultMessage IsVehicleWaitingForGross (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {

            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            List<TblLoadingTO> loadingToList = new List<TblLoadingTO> ();

            try {

                loadingToList = _iTblLoadingDAO.SelectAllLoadingListByVehicleNo (tblLoadingTO.VehicleNo, false, 0, conn, tran);
                if (loadingToList != null && loadingToList.Count > 0) {
                    loadingToList.OrderByDescending (p => p.IdLoading);
                    if (loadingToList[0].IdLoading != tblLoadingTO.IdLoading) {
                        resultMessage.DefaultBehaviour ("Not able to Remove the Allow one more Loading.");
                        return resultMessage;
                    }
                    TblLoadingTO eleLoadingTo = SelectLoadingTOWithDetails (loadingToList[0].IdLoading);
                    for (int j = 0; j < eleLoadingTo.LoadingSlipList.Count; j++) {
                        TblLoadingSlipTO eleLoadingslipTo = eleLoadingTo.LoadingSlipList[j];
                        for (int k = 0; k < eleLoadingslipTo.LoadingSlipExtTOList.Count; k++) {
                            if (eleLoadingslipTo.LoadingSlipExtTOList[k].WeightMeasureId == 0) {
                                resultMessage.DefaultBehaviour ("Weight not loaded for all material");
                                return resultMessage;
                            }
                        }
                    }

                } else {
                    resultMessage.DefaultBehaviour ("Loading Slip List found Null");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
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
        public TblWeighingMeasuresTO getWeighingGrossTo (TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran) {

            try {
                List<TblWeighingMeasuresTO> weighingMeasuresToList = new List<TblWeighingMeasuresTO> ();
                // TblWeighingMeasuresTO tblWeighingMeasureTo = new TblWeighingMeasuresTO();
                weighingMeasuresToList = _iCircularDependencyBL.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading, conn, tran);
                if (weighingMeasuresToList.Count > 0) {
                    weighingMeasuresToList = weighingMeasuresToList.OrderByDescending (p => p.CreatedOn).ToList ();
                    return weighingMeasuresToList[0];
                } else {
                    return null;
                }

            } catch (Exception ex) {
                return null;
            }
        }

        /// <summary>
        /// GJ@20171107 : Remove Allow one more Loading generation flag if required
        /// </summary>
        /// <param name="idLoading"></param>
        /// <returns></returns>
        /// 
        public ResultMessage removeIsAllowOneMoreLoading (TblLoadingTO tblLoadingTO, int loginUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                resultMessage = IsVehicleWaitingForGross (tblLoadingTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    return resultMessage;
                }
                //Insert the Weighing measure Gross To
                TblWeighingMeasuresTO weighingMeasureTo = new TblWeighingMeasuresTO ();
                weighingMeasureTo = getWeighingGrossTo (tblLoadingTO, conn, tran);
                if (weighingMeasureTo == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Last weighing weight not found againest selected Loading");
                    return resultMessage;
                }
                weighingMeasureTo.IdWeightMeasure = 0;
                weighingMeasureTo.CreatedOn = _iCommon.ServerDateTime;
                weighingMeasureTo.UpdatedOn = _iCommon.ServerDateTime;
                weighingMeasureTo.WeightMeasurTypeId = (int) Constants.TransMeasureTypeE.GROSS_WEIGHT;

                #region 1. Save the Weighing Machine Mesurement 
                result = _iTblWeighingMeasuresDAO.InsertTblWeighingMeasures (weighingMeasureTo, conn, tran);
                if (result < 0) {
                    tran.Rollback ();
                    resultMessage.Text = "";
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #endregion

                //Updating Loading Slip flag status
                tblLoadingTO.UpdatedBy = Convert.ToInt32 (loginUserId);
                tblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                //result =UpdateTblLoading(tblLoadingTO);

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_DEFAULT_WEIGHING_SCALE, conn, tran);
                if (tblConfigParamsTO != null) {
                    if (tblConfigParamsTO.ConfigParamVal == "1") {
                        tblLoadingTO.StatusId = (int) Constants.TranStatusE.LOADING_COMPLETED;
                        tblLoadingTO.TranStatusE = Constants.TranStatusE.LOADING_COMPLETED;
                        tblLoadingTO.StatusReason = "Loading Completed";
                    }
                }

                resultMessage = UpdateDeliverySlipConfirmations (tblLoadingTO, conn, tran);

                if (resultMessage.MessageType == ResultMessageE.Information) {
                    tran.Commit ();
                    resultMessage.DefaultSuccessBehaviour ();
                    return resultMessage;
                } else {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While UpdateTblLoading");
                    return resultMessage;
                }

            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In MEthod removeIsAllowOneMoreLoading";
                resultMessage.Result = -1;
                resultMessage.Tag = ex;
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage UpdateLoadingTransportDetails (TblTransportSlipTO tblTransportSlipTO) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                int result = 0;

                #region 1.Update Loading Details
                TblLoadingTO tblLoadingTO = SelectTblLoadingTO (tblTransportSlipTO.LoadingId, conn, tran);
                if (tblLoadingTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("tblLoadingTO found null");
                    return resultMessage;
                }

                tblLoadingTO.UpdatedBy = tblTransportSlipTO.UpdatedBy;
                tblLoadingTO.UpdatedOn = tblTransportSlipTO.UpdatedOn;
                tblLoadingTO.VehicleNo = tblTransportSlipTO.VehicleNo;
                tblLoadingTO.DriverName = tblTransportSlipTO.DriverName;
                tblLoadingTO.ContactNo = tblTransportSlipTO.ContactNo;
                tblLoadingTO.TransporterOrgId = tblTransportSlipTO.TransporterOrgId;
                result = UpdateTblLoading (tblLoadingTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("error while update UpdateTblLoading");
                    return resultMessage;
                }
                #endregion

                #region 2. Update loading slip details
                List<TblLoadingSlipTO> loadindingSlipList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails (tblLoadingTO.IdLoading, conn, tran);
                if (loadindingSlipList == null || loadindingSlipList.Count == 0) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("error while update UpdateTblLoading");
                    return resultMessage;
                }
                foreach (var loadindingSlip in loadindingSlipList) {
                    loadindingSlip.VehicleNo = tblTransportSlipTO.VehicleNo;
                    loadindingSlip.DriverName = tblTransportSlipTO.DriverName;
                    loadindingSlip.ContactNo = tblTransportSlipTO.ContactNo;
                    result = _iTblLoadingSlipBL.UpdateTblLoadingSlip (loadindingSlip, conn, tran);
                    if (result != 1) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ("error while update loadindingSlip");
                        return resultMessage;
                    }
                }

                #endregion

                #region 3. Update TransportSlip Details
                result = _iTblTransportSlipDAO.UpdateTblTransportSlip (tblTransportSlipTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("error while Update TblTransportSlip");
                    return resultMessage;
                }

                #endregion

                tran.Commit ();
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "Error in UpdateLoadingTransportDetails");
                return resultMessage;
            } finally {
                conn.Close ();
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
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                //chetan[07-Feb-2020] added for updaate vehicle No on IOT
                string vehicleNumber = LoadingTO.VehicleNo;
                int transporterId = LoadingTO.TransporterOrgId;

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
                int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT && tblLoadingTO.StatusId != (int)Constants.TranStatusE.LOADING_NOT_CONFIRM)
                {

                    LoadingTO.VehicleNo = string.Empty;
                    LoadingTO.TransporterOrgId = 0;
                }
                else
                {
                    tblLoadingTO.VehicleNo = LoadingTO.VehicleNo;
                    tblLoadingTO.TransporterOrgId = LoadingTO.TransporterOrgId;
                }
                tblLoadingTO.UpdatedBy = LoadingTO.UpdatedBy;
                tblLoadingTO.UpdatedOn = LoadingTO.UpdatedOn;

                //chetan[07-feb-2020] added for update vehicle no on IOT
                if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT && tblLoadingTO.StatusId != (int)Constants.TranStatusE.LOADING_NOT_CONFIRM)
                {
                    int res = WriteDataOnIOT(LoadingTO, conn, tran, vehicleNumber, transporterId);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("error while update loadindingSlip");
                        return resultMessage;
                    }
                }

                result = UpdateTblLoading(tblLoadingTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while update UpdateVehicleDetails");
                    return resultMessage;
                }
                #endregion

                #region 2. Update Vehicle In loading slip details
                List<TblLoadingSlipTO> loadindingSlipList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(tblLoadingTO.IdLoading, conn, tran);
                if (loadindingSlipList == null || loadindingSlipList.Count == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("error while update UpdateTblLoading");
                    return resultMessage;
                }
                foreach (var loadindingSlip in loadindingSlipList)
                {
                    loadindingSlip.VehicleNo = LoadingTO.VehicleNo;
                    loadindingSlip.TransporterOrgId = LoadingTO.TransporterOrgId;

                    result = _iTblLoadingSlipBL.UpdateTblLoadingSlip(loadindingSlip, conn, tran);
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

        public ResultMessage AllocateSuperwisor (TblLoadingTO tblLoadingTO, string loginUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();
                TblLoadingTO existingTblLoadingTO = SelectTblLoadingTO (tblLoadingTO.IdLoading, conn, tran);
                if (existingTblLoadingTO == null) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "existingTblLoadingTO Found NULL";
                    resultMessage.Result = 0;
                    return resultMessage;
                }

                existingTblLoadingTO.UpdatedBy = Convert.ToInt32 (loginUserId);
                existingTblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;
                existingTblLoadingTO.SuperwisorId = tblLoadingTO.SuperwisorId;
                int result = UpdateTblLoading (existingTblLoadingTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While UpdateTblLoading";
                    resultMessage.Result = 0;
                    return resultMessage;
                }
                #region Notifications & SMS

                TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO ();
                List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO> ();
                //List<TblUserTO> superwisorList = BL._iTblUserBL.SelectAllTblUserList(existingTblLoadingTO.SuperwisorId, conn, tran);
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

                TblUserTO userTO = _iTblUserDAO.SelectTblUser (existingTblLoadingTO.SuperwisorId, conn, tran);
                if (userTO != null) {
                    TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO ();
                    tblAlertUsersTO.UserId = userTO.IdUser;
                    tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                    tblAlertUsersTOList.Add (tblAlertUsersTO);
                }

                tblAlertInstanceTO.AlertDefinitionId = (int) NotificationConstants.NotificationsE.SUPERWISOR_ALLOCATION_FOR_VEHICLE;
                tblAlertInstanceTO.AlertAction = "SUPERWISOR_ALLOCATION_FOR_VEHICLE";
                tblAlertInstanceTO.AlertComment = "Vehicle Number " + tblLoadingTO.VehicleNo + "  is allocated ";
                tblAlertInstanceTO.EffectiveFromDate = tblLoadingTO.UpdatedOn;
                tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours (12);
                tblAlertInstanceTO.IsActive = 1;
                tblAlertInstanceTO.SourceDisplayId = "SUPERWISOR_ALLOCATION_FOR_VEHICLE";
                tblAlertInstanceTO.SourceEntityId = tblLoadingTO.IdLoading;
                tblAlertInstanceTO.RaisedBy = tblLoadingTO.UpdatedBy;
                tblAlertInstanceTO.RaisedOn = tblLoadingTO.UpdatedOn;
                tblAlertInstanceTO.IsAutoReset = 0;
                tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance (tblAlertInstanceTO, conn, tran);
                if (rMessage.MessageType != ResultMessageE.Information) {

                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While SaveNewAlertInstance";
                    resultMessage.Tag = tblAlertInstanceTO;
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                #endregion

                tran.Commit ();
                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {

                resultMessage.DefaultExceptionBehaviour (ex, "AllocateSuperwisor");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        #region Deletion
        public int DeleteTblLoading (Int32 idLoading) {
            return _iTblLoadingDAO.DeleteTblLoading (idLoading);
        }

        public int DeleteTblLoading (Int32 idLoading, SqlConnection conn, SqlTransaction tran) {
            return _iTblLoadingDAO.DeleteTblLoading (idLoading, conn, tran);
        }

        #endregion

        #region Methods

        // Vaibhav [30-Jan-2018] Added to update entity range for loading and loadingslip count.
        private TblEntityRangeTO SelectEntityRangeForLoadingCount(string entityName, SqlConnection conn, SqlTransaction tran, Int32 finYearId = 0, Int32 newSeq = 1) {
            try {

                if (finYearId == 0)
                {
                    finYearId = Constants.FinYear;
                }

                TblEntityRangeTO entityRangeTO = _iTblEntityRangeDAO.SelectTblEntityRangeByEntityName (entityName, finYearId, conn, tran);
                if (entityRangeTO == null) {
                    return null;
                }

                if (_iCommon.ServerDateTime.Date != entityRangeTO.CreatedOn.Date) {
                    entityRangeTO.CreatedOn = _iCommon.ServerDateTime;
                    entityRangeTO.EntityPrevValue = newSeq;

                    int result = _iTblEntityRangeDAO.UpdateTblEntityRange (entityRangeTO, conn, tran);
                    if (result != 1) {
                        return null;
                    }
                }

                return entityRangeTO;
            } catch (Exception ex) {
                ex.Message.ToString ();
                return null;
            }
        }

        /// <summary>
        /// Priyanka [19-04-2018] : Added to remove the item whose weight is not taken.(Edit Loading Slip)
        /// </summary>
        /// <param name="tblLoadingSlipExtTO"></param>
        /// <param name="txnUserId"></param>
        /// <returns></returns>
        public ResultMessage RemoveItemFromLoadingSlip (TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            DateTime txnDateTime = _iCommon.ServerDateTime;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipDAO.SelectTblLoadingSlip (tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblLoadingSlipTO found null");
                    return resultMessage;
                }

                resultMessage = RemoveItemFromLoadingSlip (tblLoadingSlipExtTO, 0, txnUserId, conn, tran);
                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information) {

                    #region Check Final Item

                    resultMessage = CheckLoadingStatusAndGenerateInvoice (tblLoadingSlipTO.LoadingId, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                        tran.Rollback ();
                        return resultMessage;
                    }

                    #endregion

                    tran.Commit ();
                }
                return resultMessage;
            } catch (Exception ex) {

                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In MEthod RemoveItemFromLoadingSlip");
                return resultMessage;

            } finally {
                conn.Close ();
            }
        }

        public ResultMessage RemoveItemFromLoadingSlip (TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 isForUpdate, Int32 txnUserId, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            DateTime txnDateTime = _iCommon.ServerDateTime;
            try {
                int result = 0;

                TblLoadingSlipExtTO existingTblLoadingSlipExtTO = _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExt (tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (existingTblLoadingSlipExtTO == null) {
                    throw new Exception ("existingTblLoadingSlipExtTO == null for IdLoadingSlipExt - " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                }

                tblLoadingSlipExtTO = existingTblLoadingSlipExtTO;

                #region 1.Mark Deletion History in tblLoadingSlipRemovedItems

                TblLoadingSlipRemovedItemsTO tblLoadingSlipRemovedItemsTO = tblLoadingSlipExtTO.GetTblLoadingSlipRemovedItemsTO ();
                tblLoadingSlipRemovedItemsTO.UpdatedBy = txnUserId;
                tblLoadingSlipRemovedItemsTO.UpdatedOn = txnDateTime;

                result = _iTblLoadingSlipRemovedItemsDAO.InsertTblLoadingSlipRemovedItems (tblLoadingSlipRemovedItemsTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While Inserting History Record For Deleted Items");
                    return resultMessage;
                }
                #endregion

                #region 2.Delete Item From TblLoadingSlipExtHistoryTO

                result = _iTblLoadingSlipExtHistoryDAO.DeleteLoadingSlipExtHistoryForItem (tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (result <= -1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While Deleting Loading Slip History");
                    return resultMessage;
                }
                #endregion

                #region 3. Reverse Booking Pending Qty
                Double bookingQty = tblLoadingSlipExtTO.LoadingQty;

                //Call to update pending booking qty for loading
                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                tblBookingsTO = _iTblBookingsDAO.SelectTblBookings (tblLoadingSlipExtTO.BookingId, conn, tran);
                if (tblBookingsTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error :tblBookingsTO Found NUll Or Empty");
                    return resultMessage;
                }

                tblBookingsTO.IdBooking = tblLoadingSlipExtTO.BookingId;
                tblBookingsTO.PendingQty = tblBookingsTO.PendingQty + bookingQty;
                tblBookingsTO.UpdatedBy = txnUserId;
                tblBookingsTO.UpdatedOn = txnDateTime;

                if (tblBookingsTO.PendingQty < 0) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblBookingsTO.PendingQty gone less than 0");
                    return resultMessage;
                }

                result = _iTblBookingsDAO.UpdateBookingPendingQty (tblBookingsTO, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : While UpdateBookingPendingQty Against Booking");
                    return resultMessage;
                }

                //AmolG[2020-Feb-20] This Function should be used when Remove Item from the loading slip
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                resultMessage = UpdateBookingQty(tblLoadingSlipExtTOList, tblLoadingSlipExtTO.BookingId, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Error)
                {
                    return resultMessage;
                }


                #endregion

                #region 4. Update the stock back and Add new record in tblStockConsumption for reverse stock entry
                TblLoadingTO tblLoadingTO = new TblLoadingTO ();
                tblLoadingTO = SelectTblLoadingTOByLoadingSlipId (tblLoadingSlipExtTO.LoadingSlipId);

                List<TblStockConsumptionTO> tblStockConsumptionTOList = _iTblStockConsumptionDAO.SelectAllStockConsumptionList (tblLoadingSlipExtTO.IdLoadingSlipExt, (int) Constants.TxnOperationTypeE.OUT, conn, tran);
                if (tblStockConsumptionTOList == null) {
                    resultMessage.DefaultBehaviour ();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error :tblStockConsumptionTOList Found Null Against LoadingSlip Cancellation";
                    return resultMessage;
                }

                for (int s = 0; s < tblStockConsumptionTOList.Count; s++) {
                    tblStockConsumptionTOList[s].LoadingSlipExtId = 0;
                    result = _iTblStockConsumptionDAO.UpdateTblStockConsumption (tblStockConsumptionTOList[s], conn, tran);
                    if (result != 1) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblStockConsumption Against LoadingSlip Cancellation";
                        return resultMessage;
                    }

                    Double qtyToReverse = Math.Abs (tblStockConsumptionTOList[s].TxnQty);
                    TblStockDetailsTO tblStockDetailsTO = _iTblStockDetailsDAO.SelectTblStockDetails (tblStockConsumptionTOList[s].StockDtlId, conn, tran);
                    if (tblStockDetailsTO == null) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error : tblStockDetailsTO Found Null Against LoadingSlip Cancellation";
                        return resultMessage;
                    }

                    double prevStockQty = tblStockDetailsTO.BalanceStock;
                    tblStockDetailsTO.BalanceStock = tblStockDetailsTO.BalanceStock + qtyToReverse;
                    tblStockDetailsTO.TotalStock = tblStockDetailsTO.BalanceStock;
                    tblStockDetailsTO.UpdatedBy = txnUserId;
                    tblStockDetailsTO.UpdatedOn = _iCommon.ServerDateTime;

                    result = _iTblStockDetailsDAO.UpdateTblStockDetails (tblStockDetailsTO, conn, tran);
                    if (result != 1) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblStockDetails Against LoadingSlip Cancellation";
                        return resultMessage;
                    }

                    // Insert Stock Consumption History Record 
                    TblStockConsumptionTO reversedStockConsumptionTO = new TblStockConsumptionTO ();
                    reversedStockConsumptionTO.AfterStockQty = tblStockDetailsTO.BalanceStock;
                    reversedStockConsumptionTO.BeforeStockQty = prevStockQty;
                    reversedStockConsumptionTO.CreatedBy = txnUserId;
                    reversedStockConsumptionTO.CreatedOn = _iCommon.ServerDateTime;
                    reversedStockConsumptionTO.LoadingSlipExtId = tblStockConsumptionTOList[s].LoadingSlipExtId;
                    reversedStockConsumptionTO.Remark = "Item is removed by " + tblLoadingTO.CreatedByUserName + " and Stock is reversed against loadingslip no." + tblLoadingTO.LoadingSlipNo;
                    reversedStockConsumptionTO.StockDtlId = tblStockDetailsTO.IdStockDtl;
                    reversedStockConsumptionTO.TxnQty = qtyToReverse;
                    reversedStockConsumptionTO.TxnOpTypeId = (int) Constants.TxnOperationTypeE.IN;

                    result = _iTblStockConsumptionDAO.InsertTblStockConsumption (reversedStockConsumptionTO, conn, tran);
                    if (result != 1) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While InsertTblStockConsumption Against LoadingSlip Cancellation";
                        return resultMessage;
                    }
                }
                #endregion

                #region 5. Recalculate the total loading qty and update it in tempLoading
                //TblLoadingTO tblLoadingTO = new TblLoadingTO();
                //tblLoadingTO =SelectTblLoadingTOByLoadingSlipId(tblLoadingSlipExtTO.LoadingSlipId);

                if (tblLoadingTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblLoadingTo found null");
                    return resultMessage;
                }

                tblLoadingTO.TotalLoadingQty = tblLoadingTO.TotalLoadingQty - tblLoadingSlipExtTO.LoadingQty;
                tblLoadingTO.UpdatedBy = txnUserId;
                tblLoadingTO.UpdatedOn = _iCommon.ServerDateTime;

                result = UpdateTblLoading (tblLoadingTO, conn, tran);
                if (result != 1) {
                    resultMessage.DefaultBehaviour ();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading Against LoadingSlip Cancellation";
                    return resultMessage;
                }
                #endregion

                #region 6. Recalculate the total loading qty and update it in tempLoadingSlipDtl

                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO ();
                tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO (tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipDtlTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblLoadingTo found null");
                    return resultMessage;
                }
                tblLoadingSlipDtlTO.LoadingQty = tblLoadingSlipDtlTO.LoadingQty - tblLoadingSlipExtTO.LoadingQty;

                result = _iTblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl (tblLoadingSlipDtlTO, conn, tran);
                if (result != 1) {
                    resultMessage.DefaultBehaviour ();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading Against LoadingSlip Cancellation";
                    return resultMessage;
                }

                if (tblLoadingSlipDtlTO.LoadingQty == 0) {

                }
                #endregion
                //If the loading slip contains a single item in it then it will remove complete loading slip after item removal.

                #region 7.Delete Item From TblLoadingSlipExtTO

                TblLoadingSlipExtTO tblLoadingSlipExtTOExist = _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExt (tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (tblLoadingSlipExtTOExist == null) {
                    throw new Exception ("tblLoadingSlipExtTOExist  IdLoadingSlipExt -" + tblLoadingSlipExtTOExist.IdLoadingSlipExt);
                }

                if (tblLoadingSlipExtTOExist.LoadedWeight > 0) {
                    tran.Rollback ();
                    resultMessage.DisplayMessage = "Weighing is already done for this items";
                    resultMessage.Text = "Weighing is already done for this items";
                    resultMessage.Result = 0;
                    return resultMessage;
                }


                result = _iTblLoadingSlipExtDAO.DeleteTblLoadingSlipExt (tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While Deleting Loading Slip Ext");
                    return resultMessage;
                }
                #endregion

                #region 8 Delete the record from loading slip

                if (isForUpdate == 0) {

                    TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO ();
                    tblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails (tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                    if (tblLoadingSlipTO == null) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ("Error : tblLoadingSlipTO found null");
                        return resultMessage;
                    }
                    if (tblLoadingSlipDtlTO.LoadingQty == 0) {
                        #region Delete Slip

                        result = _iTblLoadingSlipDtlDAO.DeleteTblLoadingSlipDtl (tblLoadingSlipDtlTO.IdLoadSlipDtl, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("Error While Deleting Loading Slip Details.");
                            return resultMessage;
                        }

                        //Delete Address

                        List<TblLoadingSlipAddressTO> tblLoadingSlipAddressTOList = _iTblLoadingSlipAddressDAO.SelectAllTblLoadingSlipAddress (tblLoadingSlipDtlTO.LoadingSlipId, conn, tran);
                        if (tblLoadingSlipAddressTOList != null && tblLoadingSlipAddressTOList.Count > 0) {
                            for (int u = 0; u < tblLoadingSlipAddressTOList.Count; u++) {
                                result = _iTblLoadingSlipAddressDAO.DeleteTblLoadingSlipAddress (tblLoadingSlipAddressTOList[u].IdLoadSlipAddr, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ("Error While Deleting Loading Slip Address Details for IdLoadSlipAddr = " + tblLoadingSlipAddressTOList[u].IdLoadSlipAddr);
                                    return resultMessage;
                                }
                            }

                        }
                        result = _iTblLoadingSlipBL.DeleteTblLoadingSlip (tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("Error While Deleting Loading Slip Details");
                            return resultMessage;
                        }

                        #endregion
                    }

                    //2. Delete record from loading.
                    if (tblLoadingTO.TotalLoadingQty == 0) {
                        #region Delete Loading

                        //3. Delete record from loading status history
                        List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOList = new List<TblLoadingStatusHistoryTO> ();

                        try {
                            tblLoadingStatusHistoryTOList = _iTblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory (tblLoadingTO.IdLoading, conn, tran);
                        } catch (Exception ex) {
                            tblLoadingStatusHistoryTOList = null;
                        }

                        if (tblLoadingStatusHistoryTOList == null) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("tblLoadingStatusHistoryTOList found null");
                            return resultMessage;
                        }
                        if (tblLoadingSlipDtlTO.LoadingQty == 0) {

                            foreach (var tblLoadingStatusHistoryTO in tblLoadingStatusHistoryTOList) {

                                result = _iTblLoadingStatusHistoryDAO.DeleteTblLoadingStatusHistory (tblLoadingStatusHistoryTO.IdLoadingHistory, conn, tran);
                                if (result != 1) {
                                    tran.Rollback ();
                                    resultMessage.DefaultBehaviour ("Error while delete loadindingSlip status history");
                                    return resultMessage;
                                }

                            }
                        }

                        //4. Delete from Weighing measures.
                        List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO> ();
                        tblWeighingMeasuresTOList = _iCircularDependencyBL.SelectAllTblWeighingMeasuresListByLoadingId (tblLoadingTO.IdLoading, conn, tran);
                        if (tblWeighingMeasuresTOList == null) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("tblWeighingMeasuresTOList found null");
                            return resultMessage;
                        }

                        foreach (var tblWeighingMeasuresTO in tblWeighingMeasuresTOList) {
                            result = _iTblWeighingMeasuresDAO.DeleteTblWeighingMeasures (tblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                            if (result != 1) {
                                tran.Rollback ();
                                resultMessage.DefaultBehaviour ("Error While Deleting tblWeighingMeasuresTOList ");
                                return resultMessage;
                            }
                        }

                        result = DeleteTblLoading (tblLoadingTO.IdLoading, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("Error While Deleting Loading Slip ");
                            return resultMessage;
                        }
                        int configId = _iTblConfigParamsDAO.IoTSetting();
                        if (configId == (Int32)Constants.WeighingDataSourceE.IoT)
                        {

                            int deleteResult = RemoveDateFromGateAndWeightIOT(tblLoadingTO);
                            if (deleteResult != 1)
                            {
                                throw new Exception("Error While RemoveDateFromGateAndWeightIOT ");
                            }
                            //  Startup.AvailableModbusRefList = _iTblLoadingDAO.GeModRefMaxData();
                            //Hrushikesh added for Multitenant changes with IOT
                            //List<int> list = _iTblLoadingDAO.GeModRefMaxData();
                            //if (list == null)
                            //    throw new Exception("Failed to get ModbusRefList");
                            //_iModbusRefConfig.setModbusRefList(list);
                        }

                        #endregion

                    }
                }
                #endregion

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {

                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In MEthod RemoveItemFromLoadingSlip");
                return resultMessage;

            } finally {
                //conn.Close();
            }
        }

        private ResultMessage CheckLoadingStatusAndGenerateInvoice (Int32 loadingId, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            TblLoadingTO loadingTOFinal = SelectTblLoadingTO (loadingId, conn, tran);
            if (loadingTOFinal != null) {
                Boolean skipInvoiceProcess = false;
                List<TblLoadingSlipTO> loadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails (loadingTOFinal.IdLoading, conn, tran);
                for (int q = 0; q < loadingSlipTOList.Count; q++) {
                    TblLoadingSlipTO tblLoadingSlipTOTemp = loadingSlipTOList[q];
                    if (tblLoadingSlipTOTemp.LoadingSlipExtTOList == null || tblLoadingSlipTOTemp.LoadingSlipExtTOList.Count == 0) {
                        skipInvoiceProcess = true;
                        break;
                    } else {
                        List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where (w => w.WeightMeasureId == 0).ToList ();
                        if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0) {
                            skipInvoiceProcess = true;
                            break;
                        }
                    }

                }
                //Added By kiran for passing Item Extenstion List For PrepareAndSaveNewTaxInvoice method
                List<TblLoadingSlipExtTO> TblLoadingSlipExtList = new List<TblLoadingSlipExtTO>();
                loadingSlipTOList.ForEach(d =>
                {
                    TblLoadingSlipExtList.AddRange(d.LoadingSlipExtTOList);
                });

                if (!skipInvoiceProcess) {
                    resultMessage = _iTblInvoiceBL.PrepareAndSaveNewTaxInvoice (loadingTOFinal, TblLoadingSlipExtList, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information) {
                        tran.Rollback ();
                        return resultMessage;
                    }
                }
            }

            resultMessage.DefaultSuccessBehaviour ();
            return resultMessage;
        }

        /// <summary>
        /// Saket [2018-04-26] Added to add new item in loading slip.
        /// </summary>
        /// <param name="tblLoadingSlipExtTO"></param>
        /// <param name="txnUserId"></param>
        /// <returns></returns>
        public ResultMessage AddItemInLoadingSlip (TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            DateTime txnDateTime = _iCommon.ServerDateTime;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                Int32 isForUpdate = 0;

                //First remove item then add


                if (tblLoadingSlipExtTO.IdLoadingSlipExt > 0) {

                    isForUpdate = 1;
                    TblLoadingSlipExtTO temp = tblLoadingSlipExtTO.DeepCopy ();
                    resultMessage = RemoveItemFromLoadingSlip (temp, 1, txnUserId, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                        return resultMessage;
                    }
                }
                //Add Item
                resultMessage = AddItemInLoadingSlip (tblLoadingSlipExtTO, txnUserId, isForUpdate, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                    return resultMessage;
                }

                tran.Commit ();
                return resultMessage;
            } catch (Exception ex) {

                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In Method AddItemInLoadingSlip");
                return resultMessage;

            } finally {
                conn.Close ();
            }
        }

        /// <summary>
        /// Saket [2018-04-26] Added to add new item in loading slip.
        /// </summary>
        /// <param name="tblLoadingSlipExtTO"></param>
        /// <param name="txnUserId"></param>
        /// <returns></returns>
        public ResultMessage AddItemInLoadingSlip (TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId, Int32 isForUpdate, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                int result = 0;

                TblBookingsTO tblBookingsTO = null;

                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO ();
                if (tblLoadingSlipExtTO.LoadingSlipId > 0) {

                    tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO (tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                    if (tblLoadingSlipDtlTO == null) {
                        throw new Exception ("tblLoadingSlipDtlTO == null for LoadingSlipId - " + tblLoadingSlipExtTO.LoadingSlipId);
                    }

                    tblBookingsTO = _iTblBookingsDAO.SelectTblBookings (tblLoadingSlipDtlTO.BookingId, conn, tran);

                    if (tblBookingsTO == null) {
                        throw new Exception ("tblBookingsTO == null for BookingId -" + tblLoadingSlipDtlTO.BookingId);
                    }

                } else {
                    tblBookingsTO = _iTblBookingsDAO.SelectTblBookings (tblLoadingSlipExtTO.BookingId, conn, tran);
                    if (tblBookingsTO == null) {
                        throw new Exception ("tblBookingsTO == null for BookingId -" + tblLoadingSlipExtTO.BookingId);
                    }
                }

                if (tblBookingsTO == null) {
                    throw new Exception ("tblBookingsTO == null");
                }

                if (tblLoadingSlipExtTO.LoadingQty > tblBookingsTO.PendingQty) {
                    String errorMsg = "Loading qty (" + tblLoadingSlipExtTO.LoadingQty + ") is greater than booking pending qty (" + tblBookingsTO.PendingQty + ")";
                    resultMessage.DefaultBehaviour (errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                    return resultMessage;
                }

                TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipDAO.SelectTblLoadingSlip (tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipTO == null) {
                    throw new Exception ("tblLoadingSlipTO == null For LoadingSlipId - " + tblLoadingSlipExtTO.LoadingSlipId);
                }

                TblLoadingTO tblLoadingTO = SelectTblLoadingTO (tblLoadingSlipTO.LoadingId, conn, tran);
                if (tblLoadingTO == null) {
                    throw new Exception ("tblLoadingTO == null For LoadingId - " + tblLoadingSlipTO.LoadingId);
                }

                #region Validation

                Int32 currentLayerId = tblLoadingSlipExtTO.LoadingLayerid;

                TblLoadingTO loadingTO = SelectTblLoadingTO (tblLoadingSlipTO.LoadingId, conn, tran);
                if (loadingTO == null) {
                    resultMessage.DefaultBehaviour ("Error Booking loadingTO is null");
                    return resultMessage;
                }

                // Select temp loading slip details.
                loadingTO.LoadingSlipList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails (tblLoadingSlipTO.LoadingId, conn, tran);

                //Already item is added in the loading slip

                TblLoadingSlipTO existingTblLoadingSlipTO = loadingTO.LoadingSlipList.Where (w => w.IdLoadingSlip == tblLoadingSlipExtTO.LoadingSlipId).FirstOrDefault ();
                if (existingTblLoadingSlipTO == null) {
                    resultMessage.DefaultBehaviour ("existingTblLoadingSlipTO == null for LoadingSlipId - " + tblLoadingSlipExtTO.LoadingSlipId);
                    return resultMessage;
                }

                var alreadySpecItemTO = existingTblLoadingSlipTO.LoadingSlipExtTOList.Where (w => w.BrandId == tblLoadingSlipExtTO.BrandId &&
                    w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                    w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                    w.MaterialId == tblLoadingSlipExtTO.MaterialId &&
                    w.ProdItemId == tblLoadingSlipExtTO.ProdItemId).FirstOrDefault ();

                if (alreadySpecItemTO != null) {
                    resultMessage.DefaultBehaviour ("Item" + tblLoadingSlipExtTO.DisplayName + "is already added into loading slip");
                    return resultMessage;
                }

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOListAll = new List<TblLoadingSlipExtTO> ();

                for (int r = 0; r < loadingTO.LoadingSlipList.Count; r++) {
                    if (loadingTO.LoadingSlipList[r].LoadingSlipExtTOList != null && loadingTO.LoadingSlipList[r].LoadingSlipExtTOList.Count > 0) {
                        tblLoadingSlipExtTOListAll.AddRange (loadingTO.LoadingSlipList[r].LoadingSlipExtTOList);
                    }
                }

                if (isForUpdate == 1) {
                    var currentLayerExtList = tblLoadingSlipExtTOListAll.Where (w => w.LoadingLayerid == currentLayerId).ToList ();
                    //if (currentLayerExtList != null && currentLayerExtList.Count == 0)  //These Condition while update item from loading slip with single item.
                    //{
                    //    resultMessage.DefaultBehaviour("Current Layer Ext List not found for layer Id - " + currentLayerId);
                    //    return resultMessage;

                    //}
                    if (currentLayerExtList == null) {
                        currentLayerExtList = new List<TblLoadingSlipExtTO> ();
                    }

                    //These Condition while update item from loading slip with single item.
                    if (tblLoadingSlipExtTOListAll != null && tblLoadingSlipExtTOListAll.Count > 0) {
                        var currentLayerExtListWtNotDone = currentLayerExtList.Where (s => s.WeightMeasureId == 0).ToList ();
                        if (currentLayerExtListWtNotDone != null && currentLayerExtListWtNotDone.Count > 0) {

                            var currentLayerExtListWtDone = currentLayerExtList.Where (s => s.WeightMeasureId > 0).ToList ();

                            if (currentLayerExtListWtDone != null && currentLayerExtListWtDone.Count > 0) {
                                var tempAlreadyWtMeasure = currentLayerExtListWtDone.Where (w => w.BrandId == tblLoadingSlipExtTO.BrandId &&
                                    w.ProdCatId == tblLoadingSlipExtTO.ProdCatId &&
                                    w.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId &&
                                    w.MaterialId == tblLoadingSlipExtTO.MaterialId).FirstOrDefault ();

                                if (tempAlreadyWtMeasure != null) {
                                    resultMessage.DefaultBehaviour ("These item weighing is already done againt these layer ");
                                    return resultMessage;
                                }
                            }

                        } else {

                            //Get next loading Ids
                            var nextLayerIdTO = tblLoadingSlipExtTOListAll.Where (w => w.LoadingLayerid > currentLayerId).FirstOrDefault ();
                            if (nextLayerIdTO == null) {
                                resultMessage.DefaultBehaviour ("Loading is already done against all layer");
                                return resultMessage;
                            } else {
                                Int32 nextLayerId = nextLayerIdTO.LoadingLayerid;

                                var nextLayerExtToList = tblLoadingSlipExtTOListAll.Where (w => w.LoadingLayerid == nextLayerId).ToList ();

                                if (nextLayerExtToList != null && nextLayerExtToList.Count > 0) {
                                    nextLayerExtToList = nextLayerExtToList.Where (s => s.WeightMeasureId > 0).ToList ();
                                    if (nextLayerExtToList != null && nextLayerExtToList.Count > 0) {
                                        resultMessage.DefaultBehaviour ("Next Layer item weighing is done");
                                        return resultMessage;
                                    }
                                } else {
                                    resultMessage.DefaultBehaviour ("Loading is already done against all layer");
                                    return resultMessage;
                                }
                            }

                        }
                    }
                }

                #endregion

                tblLoadingSlipDtlTO.IdBooking = tblLoadingSlipDtlTO.BookingId;
                tblLoadingSlipTO.TblLoadingSlipDtlTO = tblLoadingSlipDtlTO;

                tblLoadingTO.LoadingSlipList = new List<TblLoadingSlipTO> ();
                tblLoadingTO.LoadingSlipList.Add (tblLoadingSlipTO);

                tblLoadingSlipTO.LoadingSlipExtTOList = new List<TblLoadingSlipExtTO> ();
                tblLoadingSlipTO.LoadingSlipExtTOList.Add (tblLoadingSlipExtTO);

                resultMessage = CalculateLoadingValuesRate (tblLoadingTO);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                    return resultMessage;
                }

                Boolean isBoyondLoadingQuota = false;
                Double finalLoadQty = 0;
                int modbusRefIdInc = 0;
                if (tblLoadingSlipExtTOListAll != null && tblLoadingSlipExtTOListAll.Count > 0)
                {
                    modbusRefIdInc = tblLoadingSlipExtTOListAll.Max(w => w.ModbusRefId);
                }
                resultMessage = InsertLoadingExtDetails(tblLoadingTO, conn, tran, ref isBoyondLoadingQuota, ref finalLoadQty, tblLoadingSlipTO, tblBookingsTO, new List<TblBookingExtTO>(),ref modbusRefIdInc);//added by Aniket last parameter modbusRefIdInc is modbusRefId needs to change logic 
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                //Update Qty in temploading

                tblLoadingTO = SelectTblLoadingTO (tblLoadingSlipTO.LoadingId, conn, tran);
                if (tblLoadingTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblLoadingTO found null");
                    return resultMessage;
                }
                tblLoadingTO.TotalLoadingQty = tblLoadingTO.TotalLoadingQty + tblLoadingSlipExtTO.LoadingQty;
                result = UpdateTblLoading (tblLoadingTO, conn, tran);

                //Update Qty in temploadingslipdtl

                tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO (tblLoadingSlipExtTO.LoadingSlipId, conn, tran);
                if (tblLoadingSlipDtlTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblLoadingTo found null");
                    return resultMessage;
                }
                tblLoadingSlipDtlTO.LoadingQty = tblLoadingSlipDtlTO.LoadingQty + tblLoadingSlipExtTO.LoadingQty;

                result = _iTblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl (tblLoadingSlipDtlTO, conn, tran);
                if (result != 1) {
                    resultMessage.DefaultBehaviour ();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTblLoading Against LoadingSlip Cancellation";
                    return resultMessage;
                }

                //Update Qty in booking
                List<TblBookingsTO> tblBookingsTOList = _iTblBookingsDAO.SelectAllBookingsListFromLoadingSlipId (tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                if (tblBookingsTOList == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error : tblBookingsTOList found null");
                }
                foreach (var tblBookingsTONew in tblBookingsTOList) {
                    tblBookingsTONew.PendingQty = tblBookingsTONew.PendingQty - tblLoadingSlipExtTO.LoadingQty;
                    result = _iTblBookingsDAO.UpdateTblBookings (tblBookingsTONew, conn, tran);
                    if (result != 1) {
                        resultMessage.DefaultBehaviour ();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblBookings ";
                        return resultMessage;
                    }

                }

                //AmolG[2020-Feb-24]
                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = new List<TblLoadingSlipExtTO>();
                tblLoadingSlipExtTOList.Add(tblLoadingSlipExtTO);
                tblLoadingSlipExtTO.LoadingQty *= -1;
                resultMessage = UpdateBookingQty(tblLoadingSlipExtTOList, tblLoadingSlipExtTO.BookingId, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Error)
                {
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;

            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In Method AddItemInLoadingSlip");
                return resultMessage;
            } finally {

            }
        }

        public ResultMessage ReverseWeighingProcessAgainstLoading (Int32 loadingId, Int32 txnUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            DateTime txnDateTime = _iCommon.ServerDateTime;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                //Add Item
                resultMessage = ReverseWeighingProcessAgainstLoading (loadingId, txnUserId, conn, tran);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                    return resultMessage;
                }

                tran.Commit ();
                return resultMessage;
            } catch (Exception ex) {

                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In Method ReverseWeighingProcessAgainstLoading");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }

        public ResultMessage ReverseWeighingProcessAgainstLoading (Int32 loadingId, Int32 txnUserId, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            try {
                int result = 0;

                TblLoadingTO tblLoadingTO = SelectTblLoadingTO (loadingId, conn, tran);
                if (tblLoadingTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("tblLoadingTO == null for loading Id - " + loadingId);
                    return resultMessage;
                }

                if (tblLoadingTO.StatusId != (int) Constants.TranStatusE.LOADING_GATE_IN) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Loading status in not gate in");
                    return resultMessage;
                }

                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = _iCircularDependencyBL.SelectAllTblWeighingMeasuresListByLoadingId (loadingId, conn, tran);

                if (tblWeighingMeasuresTOList == null || tblWeighingMeasuresTOList.Count == 0) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Weighing not found against loading");
                    return resultMessage;
                }

                tblWeighingMeasuresTOList = tblWeighingMeasuresTOList.OrderBy (w => w.IdWeightMeasure).ToList ();

                TblWeighingMeasuresTO latestTblWeighingMeasuresTO = tblWeighingMeasuresTOList[tblWeighingMeasuresTOList.Count - 1];

                if (latestTblWeighingMeasuresTO.WeightMeasurTypeId == (int) Constants.TransMeasureTypeE.GROSS_WEIGHT) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Gross Weight is done against loading");
                    return resultMessage;
                }

                List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectAllTblLoadingSlipExtByWeighingMeasureId (latestTblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0) {
                    for (int i = 0; i < tblLoadingSlipExtTOList.Count; i++) {
                        TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipExtTOList[i];

                        //Check invoice is generated against item
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = _iTblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId (tblLoadingSlipExtTO.IdLoadingSlipExt, conn, tran);
                        if (tblInvoiceItemDetailsTO != null) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("Invoice is generated against last weighing item");
                            return resultMessage;
                        }

                        tblLoadingSlipExtTO.WeightMeasureId = 0;
                        tblLoadingSlipExtTO.LoadedWeight = 0;
                        tblLoadingSlipExtTO.LoadedBundles = 0;
                        tblLoadingSlipExtTO.CalcTareWeight = 0;

                        result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExt (tblLoadingSlipExtTO, conn, tran);
                        if (result != 1) {
                            tran.Rollback ();
                            resultMessage.DefaultBehaviour ("Error While updating Item loadingSlipExtId = " + tblLoadingSlipExtTO.IdLoadingSlipExt);
                            return resultMessage;
                        }

                    }

                }

                result = _iTblWeighingMeasuresDAO.DeleteTblWeighingMeasures (latestTblWeighingMeasuresTO.IdWeightMeasure, conn, tran);
                if (result != 1) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While deleting tare weight against weightMeasureId = " + latestTblWeighingMeasuresTO.IdWeightMeasure);
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;

            } catch (Exception ex) {
                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In Method ReverseWeighingProcessAgainstLoading");
                return resultMessage;
            } finally {

            }
        }


        /// <summary>
        /// Vijaymala[12-04-2017] :Added to get invoice list by using vehicle number
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectAllInvoiceListByVehicleNo(string vehicleNo, DateTime frmDt, DateTime toDt)
        {
            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();

            int weightSourceConfigId = _iTblConfigParamsDAO.IoTSetting();
            List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();

            //DB
            if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.DB)
            {
                tblLoadingSlipTOList = _iTblLoadingSlipBL.SelectAllLoadingListByVehicleNo(vehicleNo, frmDt, toDt);
            }
            else
            {
                Int32 loadingId = 0;
                ResultMessage r = IsThisVehicleDelivered(vehicleNo, 1);
                if (r.Tag != null && r.Tag.GetType() == typeof(TblLoadingTO))
                {
                    //tblLoadingTO = ((TblLoadingTO)r.Tag);
                    loadingId = ((TblLoadingTO)r.Tag).IdLoading;
                    //tblLoadingTO.MergeMessage = r.DisplayMessage;
                }
                else
                {
                    return tblInvoiceTOList;
                }

                tblLoadingSlipTOList = _iTblLoadingSlipDAO.SelectAllTblLoadingSlip(loadingId);

                tblLoadingSlipTOList.ForEach(f => f.VehicleNo = vehicleNo);

            }

            if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0)
            {
                String strLoadingSlipIds = String.Join(",", tblLoadingSlipTOList.Select(s => s.IdLoadingSlip.ToString()).ToArray());
                if (!String.IsNullOrEmpty(strLoadingSlipIds))
                {
                    List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceDAO.SelectAllTempLoadingSlipInvoiceList(strLoadingSlipIds);
                    String strInvoiceIds = String.Join(",", tempLoadingSlipInvoiceTOList.Select(s => s.InvoiceId.ToString()).ToArray());

                    if (!String.IsNullOrEmpty(strInvoiceIds))
                    {
                        tblInvoiceTOList = _iTblInvoiceBL.SelectInvoiceListFromInvoiceIds(strInvoiceIds);
                        if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                        {

                            if (weightSourceConfigId == (int)Constants.WeighingDataSourceE.IoT)
                            {
                                tblInvoiceTOList.ForEach(f => f.VehicleNo = vehicleNo);
                            }

                            return tblInvoiceTOList;
                        }
                    }

                }
            }
            return tblInvoiceTOList;

        }



        public ResultMessage CreateIntermediateInvoiceAgainstLoading (String loadingIds, Int32 userId) {
            List<TblLoadingTO> tblLoadingTOList = SelectLoadingTOListWithDetails (loadingIds);

            List<TblInvoiceTO> newTblInvoiceTOList = new List<TblInvoiceTO> ();

            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            resultMessage.MessageType = ResultMessageE.None;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                if (tblLoadingTOList != null && tblLoadingTOList.Count > 0) {

                    //Added By kiran for passing Item Extenstion List For PrepareAndSaveNewTaxInvoice method
                    List<TblLoadingSlipExtTO> TblLoadingSlipExtList = new List<TblLoadingSlipExtTO>();
                    tblLoadingTOList.ForEach(c => c.LoadingSlipList.ForEach(d =>
                    {
                        TblLoadingSlipExtList.AddRange(d.LoadingSlipExtTOList);
                    }));

                    for (int i = 0; i < tblLoadingTOList.Count; i++) {
                        tblLoadingTOList[i].CreatedBy = userId;
                        resultMessage = _iTblInvoiceBL.PrepareAndSaveNewTaxInvoice (tblLoadingTOList[i], TblLoadingSlipExtList, conn, tran);
                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                            return resultMessage;
                        } else {
                            if (resultMessage.Tag != null && resultMessage.Tag.GetType () == typeof (List<TblInvoiceTO>)) {
                                if (((List<TblInvoiceTO>) resultMessage.Tag).Count > 0) {
                                    newTblInvoiceTOList.AddRange ((List<TblInvoiceTO>) resultMessage.Tag);
                                }
                            }
                        }

                    }
                }
                tran.Commit ();
                resultMessage.MessageType = ResultMessageE.Information;

                if (newTblInvoiceTOList != null) {
                    resultMessage.Text = "Invoice Saved Sucessfully (" + newTblInvoiceTOList.Count + ")";
                } else {
                    resultMessage.Text = "Invoice Saved Sucessfully";
                }
                resultMessage.Result = 1;
                return resultMessage;

            } catch (Exception ex) {

                resultMessage.Text = "Exception Error While Record Save : SaveNewWeighinMachineMeasurement";
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Exception = ex;
                resultMessage.Result = -1;
                return resultMessage;
            } finally {

            }

        }


        public ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();



                TblLoadingTO tblLoadingTONew = new TblLoadingTO();
                tblLoadingTONew = _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId(tblLoadingSlipTO.IdLoadingSlip, conn, tran);
                if (tblLoadingTONew == null)
                {
                    resultMessage.DefaultBehaviour("tblLoadingTONew  found NULL");
                    return resultMessage;
                }

                //Slip To

                //loadingSlipTO.LoadingSlipExtTOList;
                //loadingSlipTO.TblLoadingSlipDtlTO;

                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(tblLoadingSlipTO.IdLoadingSlip, conn, tran);

                tblLoadingSlipTO.TblLoadingSlipDtlTO = tblLoadingSlipDtlTO;
                tblLoadingTONew.LoadingSlipList = new List<TblLoadingSlipTO>();

                tblLoadingTONew.LoadingSlipList.Add(tblLoadingSlipTO);

                Int32 lastConfirmationStatus = tblLoadingTONew.LoadingSlipList[0].IsConfirmed;
                if (lastConfirmationStatus == 1)
                    tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 0;
                else
                    tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 1;

                resultMessage = CalculateLoadingValuesRate(tblLoadingTONew);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error While CalculateLoadingValuesRate");
                    return resultMessage;
                }

                lastConfirmationStatus = tblLoadingTONew.LoadingSlipList[0].IsConfirmed;
                if (lastConfirmationStatus == 1)
                    tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 0;
                else
                    tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 1;




                resultMessage = _iTblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(tblLoadingSlipTO, loginUserId, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    tran.Rollback();
                    return null;
                }
                //Priyanka [15-05-2018] added to commit the transaction.
                tran.Commit();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ChangeLoadingSlipConfirmationStatus");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }



        }





        public ResultMessage UpdateInvoiceConfrimNonConfirmDetails (TblInvoiceTO tblInvoiceTO, Int32 loginUserId) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage ();
            Double totalInvQty = 0;
            Double totalNCExpAmt = 0;
            Double totalNCOtherAmt = 0;
            double conversionFactor = 0.001;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                TblInvoiceTO exiInvoiceTO = _iTblInvoiceBL.SelectTblInvoiceTOWithDetails (tblInvoiceTO.IdInvoice, conn, tran);
                if (exiInvoiceTO == null) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("exiInvoiceTO Found NULL");
                    return resultMessage;
                }
                exiInvoiceTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
                exiInvoiceTO.UpdatedBy = tblInvoiceTO.UpdatedBy;
                exiInvoiceTO.UpdatedOn = tblInvoiceTO.UpdatedOn;

                //Call to get the Loading Slip detail againest Loading Slip
                TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO ();
                TblLoadingSlipTO loadingSlipTO = new TblLoadingSlipTO ();
                if (tblInvoiceTO.LoadingSlipId != 0) {
                    loadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails (tblInvoiceTO.LoadingSlipId, conn, tran);
                    if (loadingSlipTO == null) {
                        resultMessage.DefaultBehaviour ("loadingSlipTO Found NULL");
                        return resultMessage;
                    }
                    //loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
                    tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO (tblInvoiceTO.LoadingSlipId, conn, tran);
                    //if (tblLoadingSlipDtlTO == null)
                    //{
                    //    tran.Rollback();
                    //    resultMessage.MessageType = ResultMessageE.Error;
                    //    resultMessage.Text = "Error :tblLoadingSlipDtlTO Found NUll Or Empty";
                    //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    //    return resultMessage;
                    //}
                }
                if (tblInvoiceTO.LoadingSlipId == 0 || tblLoadingSlipDtlTO == null) {
                    int result = 0;
                    result = _iTblInvoiceDAO.UpdateTblInvoice (tblInvoiceTO, conn, tran);
                    if (result != 1) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ("Error While UpdateInvoiceConfrimNonConfirmDetails");
                        return resultMessage;
                    }
                    if (tblInvoiceTO.LoadingSlipId != 0) {
                        loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;

                        //    resultMessage = _iTblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(loadingSlipTO, tblInvoiceTO.UpdatedBy, conn, tran);
                        //    if (resultMessage.MessageType != ResultMessageE.Information)
                        //    {
                        //        tran.Rollback();
                        //        resultMessage.DefaultBehaviour("Error While ChangeLoadingSlipConfirmationStatus");
                        //        return resultMessage;
                        //    }
                    }

                    tran.Commit ();

                    resultMessage.DefaultSuccessBehaviour ();
                    if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.StatusId == Convert.ToInt32 (Constants.InvoiceStatusE.AUTHORIZED)) {
                        Int32 isconfirm = 0;
                        GenerateInvoiceNumber(tblInvoiceTO.IdInvoice, loginUserId, isconfirm, (int)Constants.InvoiceGenerateModeE.REGULAR,0,0);

                    }
                    return resultMessage;
                }

                //Call to get the TblBooking for Parity Id
                TblBookingsTO tblBookingsTO = new Models.TblBookingsTO ();
                tblBookingsTO = _iTblBookingsDAO.SelectTblBookings (tblLoadingSlipDtlTO.BookingId, conn, tran);
                if (tblBookingsTO == null) {
                    tran.Rollback ();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    return resultMessage;
                }
                if (exiInvoiceTO.InvoiceItemDetailsTOList != null && exiInvoiceTO.InvoiceItemDetailsTOList.Count > 0) {
                    List<TblParityDetailsTO> parityDetailsTOList = null;
                    //if (tblBookingsTO.ParityId > 0)
                    //    parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(tblBookingsTO.ParityId, 0, conn, tran);

                    String parityIds = String.Empty;
                    List<TblBookingParitiesTO> tblBookingParitiesTOList = _iTblBookingParitiesDAO.SelectTblBookingParitiesByBookingId (tblBookingsTO.IdBooking, conn, tran);
                    if (tblBookingParitiesTOList != null && tblBookingParitiesTOList.Count > 0) {
                        parityIds = String.Join (",", tblBookingParitiesTOList.Select (s => s.ParityId.ToString ()).ToArray ());
                    }

                    if (String.IsNullOrEmpty (parityIds)) {
                        tran.Rollback ();
                        resultMessage.DefaultBehaviour ();
                        resultMessage.Text = "Error : ParityTO Not Found";
                        resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                        return resultMessage;
                    }

                    //Sudhir[30-APR-2018] Commented For New Parity Logic.
                    //parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(parityIds, 0, conn, tran);

                    //Harshala
                    if (tblInvoiceTO.IsConfirmed == 0)
                    {
                        Int32 tcsTaxId = 0;
                        TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);
                        if (configParamsTO != null)
                        {
                            tcsTaxId = Convert.ToInt32(configParamsTO.ConfigParamVal);
                        }
                        if (tcsTaxId > 0)
                            exiInvoiceTO.InvoiceItemDetailsTOList = exiInvoiceTO.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId != tcsTaxId).ToList();
                    }
                    //

                    for (int e = 0; e < exiInvoiceTO.InvoiceItemDetailsTOList.Count; e++) {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = exiInvoiceTO.InvoiceItemDetailsTOList[e];
                        if (tblInvoiceItemDetailsTO.OtherTaxId == 0) {

                            //TblLoadingSlipExtTO tblLoadingSlipExtTO = _iTblLoadingSlipExtBL.SelectTblLoadingSlipExtTO(tblInvoiceItemDetailsTO.LoadingSlipExtId, conn, tran);

                            //if (tblLoadingSlipExtTO != null && tblLoadingSlipExtTO.LoadingQty > 0)
                            //{
                            //    #region Calculate Actual Price From Booking and Parity Settings

                            //    Double orcAmtPerTon = 0;
                            //    if (loadingSlipTO.OrcMeasure == "Rs/MT")
                            //    {
                            //        //orcAmtPerTon = tblBookingsTO.OrcAmt; Sudhir[30-APR-2018] ORC From Loading Slip Instead of Booking.
                            //        orcAmtPerTon = loadingSlipTO.OrcAmt;
                            //    }
                            //    else
                            //    {
                            //        //if (tblBookingsTO.OrcAmt > 0)
                            //        //    orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
                            //        if (loadingSlipTO.OrcAmt > 0)
                            //            orcAmtPerTon = loadingSlipTO.OrcAmt / tblLoadingSlipDtlTO.LoadingQty;
                            //    }

                            //    Double bookingPrice = tblBookingsTO.BookingRate;
                            //    Double parityAmt = 0;
                            //    Double priceSetOff = 0;
                            //    Double paritySettingAmt = 0;
                            //    Double bvcAmt = 0;
                            //    // TblParitySummaryTO parityTO = null; Sudhir[30-APR-2018] Commented for New Parity Logic.
                            //    TblParityDetailsTO parityDtlTO = null;
                            //    if (true)
                            //    {
                            //        //Sudhir[30-APR-2018] Commented for New Parity Logic.
                            //        /*var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
                            //                                        && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
                            //                                        && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId 
                            //                                        && m.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();*/

                            //        //Get Latest To Based On -materialId, Date And Time Check Condition Actual TIme < = First Object.
                            //        TblAddressTO addrTO =_iTblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

                            //        //SUdhir[30-APR-2018] Added for the Get Parity Details List based on Material Id,ProdCat Id,ProdSpec Id ,State Id ,Brand Id and Booking Date.
                            //        parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, addrTO.StateId, tblBookingsTO.BookingDatetime);

                            //        if (parityDtlTO != null)
                            //        {
                            //            parityAmt = parityDtlTO.ParityAmt;
                            //            if (tblInvoiceTO.IsConfirmed != 1)
                            //                priceSetOff = parityDtlTO.NonConfParityAmt;
                            //            else
                            //                priceSetOff = 0;

                            //        }
                            //        else
                            //        {
                            //            tran.Rollback();
                            //            resultMessage.DefaultBehaviour();
                            //            resultMessage.Text = "Error : ParityTO Not Found";
                            //            string mateDesc = tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
                            //            resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
                            //            return resultMessage;
                            //        }

                            //        #region Sudhir[30-APR-2018] Commented for New Parity Logic
                            //        //parityTO = _iTblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
                            //        //if (parityTO == null)
                            //        //{
                            //        //    tran.Rollback();
                            //        //    resultMessage.DefaultBehaviour();
                            //        //    resultMessage.Text = "Error : ParityTO Not Found";
                            //        //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                            //        //    return resultMessage;
                            //        //}
                            //        //paritySettingAmt = parityTO.BaseValCorAmt + parityTO.ExpenseAmt + parityTO.OtherAmt;
                            //        //bvcAmt = parityTO.BaseValCorAmt;
                            //        #endregion

                            //        //[30-APR-2018] Added For New Parity Setting Logic
                            //        paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                            //        bvcAmt = parityDtlTO.BaseValCorAmt;
                            //    }
                            //    else
                            //    {
                            //        tran.Rollback();
                            //        resultMessage.DefaultBehaviour();
                            //        resultMessage.Text = "Error : ParityTO Not Found";
                            //        resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
                            //        return resultMessage;
                            //    }
                            //    Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
                            //    //if (tblInvoiceTO.IsConfirmed == 1)
                            //        // cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt; Sudhir[30-APR-2018] Commented.
                            //   if (tblInvoiceTO.IsConfirmed == 1)
                            //         cdApplicableAmt += parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
                            //    tblInvoiceItemDetailsTO.Rate = cdApplicableAmt;

                            //    #endregion
                            //}
                            //else
                            //{
                            //    tran.Rollback();
                            //    resultMessage.DefaultBehaviour("Error : tblLoadingSlipExtTO Found Null Or Empty");
                            //    return resultMessage;
                            //}
                        }
                    }
                } else {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ();
                    resultMessage.Text = "Error : InvoiceItemDetailsTOList(Invoice Item Details) Found Null Or Empty";
                    resultMessage.DisplayMessage = "Error 01 : No Items found to change the Status.";
                    return resultMessage;
                }
                TblLoadingTO tblLoadingTONew = new TblLoadingTO ();
                tblLoadingTONew = _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId (loadingSlipTO.IdLoadingSlip, conn, tran);
                if (tblLoadingTONew == null) {
                    resultMessage.DefaultBehaviour ("tblLoadingTONew  found NULL");
                    return resultMessage;
                }

                //Slip To

                //loadingSlipTO.LoadingSlipExtTOList;
                //loadingSlipTO.TblLoadingSlipDtlTO;
                loadingSlipTO.TblLoadingSlipDtlTO = tblLoadingSlipDtlTO;
                loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
                tblLoadingTONew.LoadingSlipList = new List<TblLoadingSlipTO> ();

                tblLoadingTONew.LoadingSlipList.Add (loadingSlipTO);

                resultMessage = CalculateLoadingValuesRate (tblLoadingTONew);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While CalculateLoadingValuesRate");
                    return resultMessage;
                }

                Int32 lastConfirmationStatus = tblLoadingTONew.LoadingSlipList[0].IsConfirmed;
                if (lastConfirmationStatus == 1)
                    tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 0;
                else
                    tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 1;

                // tblLoadingTONew.LoadingSlipList[0].IsConfirmed= exiInvoiceTO.IsConfirmed;
                resultMessage = _iTblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus (tblLoadingTONew.LoadingSlipList[0], tblInvoiceTO.UpdatedBy, conn, tran);
                // resultMessage = _iTblLoadingSlipBL.OldChangeLoadingSlipConfirmationStatus(loadingSlipTO, tblInvoiceTO.UpdatedBy, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While ChangeLoadingSlipConfirmationStatus");
                    return resultMessage;
                }

                //Priyanka [25-07-2018] : Added

                TblLoadingTO tblLoadingTO = new TblLoadingTO ();
                tblLoadingTO = _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId (loadingSlipTO.IdLoadingSlip, conn, tran);
                if (tblLoadingTO == null) {
                    resultMessage.DefaultBehaviour ("Address Not Found For Self Organization.");
                    return resultMessage;
                }
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                if (tblConfigParamsTO == null) {
                    resultMessage.DefaultBehaviour ("Internal Self Organization Not Found in Configuration.");
                    return resultMessage;
                }
                Int32 internalOrgId = Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal);
                TblAddressTO ofcAddrTO = _iTblAddressBL.SelectOrgAddressWrtAddrType (internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (ofcAddrTO == null) {
                    resultMessage.DefaultBehaviour ("Address Not Found For Self Organization.");
                    return resultMessage;
                }
                /*GJ@20170927 : For get RCM and pass to Invoice*/
                TblConfigParamsTO rcmConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_REVERSE_CHARGE_MECHANISM, conn, tran);
                if (rcmConfigParamsTO == null) {
                    resultMessage.DefaultBehaviour ("RCM value Not Found in Configuration.");
                    return resultMessage;
                }
                TblConfigParamsTO invoiceDateConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO (Constants.CP_TARE_WEIGHT_DATE_AS_INV_DATE, conn, tran);
                if (invoiceDateConfigTO == null || invoiceDateConfigTO.ConfigParamVal == "0") {
                    tblInvoiceTO.InvoiceDate = _iCommon.ServerDateTime;
                }
                loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;

                Int32 billingStateId = 0;
                TblInvoiceTO calculatedInvoiceTO = _iTblInvoiceBL.PrepareInvoiceAgainstLoadingSlip (tblLoadingTO, conn, tran, internalOrgId, ofcAddrTO, rcmConfigParamsTO, invoiceDateConfigTO, loadingSlipTO, exiInvoiceTO.DealerOrgId);

                if (calculatedInvoiceTO == null) {
                    resultMessage.DefaultBehaviour ("calculatedInvoiceTO  found NULL");
                    return resultMessage;
                }

                Int32 invoiceId = exiInvoiceTO.IdInvoice;
                //// calculatedInvoiceTO = exiInvoiceTO;
                // calculatedInvoiceTO.IdInvoice = invoiceId;
                // calculatedInvoiceTO.InvoiceAddressTOList.ForEach(f => f.InvoiceId = invoiceId);
                // //calculatedInvoiceTO.InvoiceItemDetailsTOList.ForEach(f => f.InvoiceId = invoiceId);
                // calculatedInvoiceTO.TempLoadingSlipInvoiceTOList.ForEach(f => f.InvoiceId = invoiceId);
                //// calculatedInvoiceTO.InvoiceDocumentDetailsTOList.ForEach(f => f.InvoiceId = invoiceId);

                // calculatedInvoiceTO.InvoiceAddressTOList = exiInvoiceTO.InvoiceAddressTOList;
                // calculatedInvoiceTO.InvoiceDocumentDetailsTOList = exiInvoiceTO.InvoiceDocumentDetailsTOList;
                // calculatedInvoiceTO.InvoiceItemDetailsTOList = exiInvoiceTO.InvoiceItemDetailsTOList;

                #region 5 Save main Invoice
                exiInvoiceTO.TaxableAmt = calculatedInvoiceTO.TaxableAmt;
                exiInvoiceTO.DiscountAmt = calculatedInvoiceTO.DiscountAmt;
                exiInvoiceTO.IgstAmt = calculatedInvoiceTO.IgstAmt;
                exiInvoiceTO.CgstAmt = calculatedInvoiceTO.CgstAmt;
                exiInvoiceTO.SgstAmt = calculatedInvoiceTO.SgstAmt;

                exiInvoiceTO.IsConfirmed = calculatedInvoiceTO.IsConfirmed;
                exiInvoiceTO.GrandTotal = calculatedInvoiceTO.GrandTotal;
                Double tdsTaxPct = 0;
                if (calculatedInvoiceTO != null)
                {
                    if (calculatedInvoiceTO.IsTcsApplicable == 0)
                    {
                        TblConfigParamsTO tdsConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT);
                        if (tdsConfigParamsTO != null)
                        {
                            if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                            {
                                tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                            }
                        }
                    }
                }
               
                exiInvoiceTO.RoundOffAmt = calculatedInvoiceTO.RoundOffAmt;
                exiInvoiceTO.BasicAmt = calculatedInvoiceTO.BasicAmt;
                calculatedInvoiceTO.InvoiceItemDetailsTOList.ForEach(ele => ele.InvoiceId = invoiceId);
                calculatedInvoiceTO.InvoiceAddressTOList.ForEach(ele => ele.InvoiceId = invoiceId);

                if (calculatedInvoiceTO.InvoiceAddressTOList != null && calculatedInvoiceTO.InvoiceAddressTOList.Count > 0)
                {
                    if (exiInvoiceTO.InvoiceAddressTOList != null && exiInvoiceTO.InvoiceAddressTOList.Count > 0)
                    {
                        for (int a = 0; a < calculatedInvoiceTO.InvoiceAddressTOList.Count; a++)
                        {
                            if (exiInvoiceTO.InvoiceAddressTOList.Count > a)
                            {
                                calculatedInvoiceTO.InvoiceAddressTOList[a].IdInvoiceAddr = exiInvoiceTO.InvoiceAddressTOList[a].IdInvoiceAddr;
                            }
                        }
                    }
                }
                //Saket [2019-09-30] Error while validating Invoice ,i.e CST applied for inter state invoice.
                exiInvoiceTO.InvoiceAddressTOList = calculatedInvoiceTO.InvoiceAddressTOList;

                exiInvoiceTO.InvoiceItemDetailsTOList = calculatedInvoiceTO.InvoiceItemDetailsTOList;

                exiInvoiceTO.TdsAmt = 0;
                if (calculatedInvoiceTO.IsConfirmed == 1 && calculatedInvoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                {
                    exiInvoiceTO.TdsAmt = (_iTblInvoiceBL.CalculateTDS(exiInvoiceTO) * tdsTaxPct) / 100;
                    exiInvoiceTO.TdsAmt = Math.Ceiling(exiInvoiceTO.TdsAmt);
                }

                #endregion

                //exiInvoiceTO = updateInvoiceToCalc(exiInvoiceTO, conn, tran, false);
                if (tblInvoiceTO.IsConfirmed == 0) {
                    //for (int i = 0; i < loadingSlipTO.LoadingSlipExtTOList.Count; i++)
                    //{
                    //    TblLoadingSlipExtTO tblLoadingSlipExt = loadingSlipTO.LoadingSlipExtTOList[i];
                    //    //TblParitySummaryTO parityTO = _iTblParitySummaryBL.SelectParitySummaryTOFromParityDtlId(tblLoadingSlipExt.ParityDtlId, conn, tran);
                    //    TblParityDetailsTO parityTO = new TblParityDetailsTO();
                    //    if (tblLoadingSlipExt.ParityDtlId > 0)
                    //        parityTO = BL.TblParityDetailsBL.SelectTblParityDetailsTO(tblLoadingSlipExt.ParityDtlId, conn, tran);
                    //    if (parityTO != null)
                    //    {
                    //        totalNCExpAmt += parityTO.ExpenseAmt * Math.Round(tblLoadingSlipExt.LoadedWeight * conversionFactor, 2);
                    //        totalNCOtherAmt += parityTO.OtherAmt * Math.Round(tblLoadingSlipExt.LoadedWeight * conversionFactor, 2);
                    //    }
                    //}
                    //exiInvoiceTO.ExpenseAmt = totalNCExpAmt;
                    //exiInvoiceTO.OtherAmt = totalNCOtherAmt;
                    //exiInvoiceTO.GrandTotal += totalNCExpAmt + totalNCOtherAmt;

                }
                resultMessage = _iTblInvoiceBL.UpdateInvoice (exiInvoiceTO, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information) {
                    tran.Rollback ();
                    resultMessage.DefaultBehaviour ("Error While UpdateInvoiceConfrimNonConfirmDetails");
                    return resultMessage;
                }
                //Update the Loading Slip To Details
                if (tblInvoiceTO.IsConfirmed == 0) {
                    loadingSlipTO.IsConfirmed = 1;
                } else {
                    loadingSlipTO.IsConfirmed = 0;
                }

                tran.Commit ();
                resultMessage.DefaultSuccessBehaviour ();

                if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.StatusId == Convert.ToInt32 (Constants.InvoiceStatusE.AUTHORIZED)) {
                    Int32 isconfirm = 0;
                    GenerateInvoiceNumber(tblInvoiceTO.IdInvoice, loginUserId, isconfirm, (int)Constants.InvoiceGenerateModeE.REGULAR,0,0);

                }
                return resultMessage;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "UpdateInvoiceConfrimNonConfirmDetails");
                return resultMessage;
            } finally {
                conn.Close ();
            }
        }
        public ResultMessage ExtractEnquiryData () {
            SqlConnection bookingConn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction bookingTran = null;
            SqlConnection enquiryConn = new SqlConnection (Startup.NewConnectionString);
            SqlTransaction enquiryTran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            List<TblLoadingTO> tempLoadingTOList = new List<TblLoadingTO> ();
            List<TblInvoiceRptTO> tblInvoiceRptTOList = new List<TblInvoiceRptTO> ();
            Dictionary<int, int> invoiceIdsList = new Dictionary<int, int> ();

            List<int> processedLoadings = new List<int> ();

            int result = 0;
            int loadingCount = 0;
            int totalLoading = 0;
            List<int> loadingIdList = new List<int> ();

            try {

                if (bookingConn.State == ConnectionState.Closed) {
                    bookingConn.Open ();
                    bookingTran = bookingConn.BeginTransaction ();
                }
                TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName (Constants.CP_MIGRATE_ENQUIRY_DATA);

                if (configParamsTO.ConfigParamVal == "1") {
                    if (enquiryConn.State == ConnectionState.Closed) {
                        try {
                            enquiryConn.Open ();
                            enquiryTran = enquiryConn.BeginTransaction ();
                        } catch (Exception ex) {
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.DefaultBehaviour (ex.Message);
                            return resultMessage;
                        }
                    }
                }

                // Select temp loading data.
                tempLoadingTOList = SelectAllTempLoading (bookingConn, bookingTran);
                if (tempLoadingTOList == null || tempLoadingTOList.Count <= 0) {
                    resultMessage.DefaultBehaviour ("Record not found!! ");
                    resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;
                }

                // Select temp invoice data for creating excel file.
                tblInvoiceRptTOList = _iFinalBookingData.SelectTempInvoiceData (bookingConn, bookingTran);

                if (tempLoadingTOList != null && tempLoadingTOList.Count > 0) {

                    foreach (var tempLoadingTO in tempLoadingTOList.ToList ()) {

                        if (bookingConn.State == ConnectionState.Closed) {
                            bookingConn.Open ();
                            bookingTran = bookingConn.BeginTransaction ();
                        }
                        loadingIdList.Add(tempLoadingTO.IdLoading);//Reshma Added
                        loadingCount++;
                        totalLoading++;
                        #region Insert Booking Data
                        resultMessage = _iFinalBookingData .InsertFinalBookingData(tempLoadingTO.IdLoading, bookingConn, bookingTran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            bookingTran.Rollback();
                            enquiryTran.Rollback();
                            _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                            _iFinalBookingData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error while InsertFinalBookingData";
                            return resultMessage;
                        }
                        #endregion
                        resultMessage = _iFinalBookingData.InsertFinalEnquiryData(tempLoadingTO.IdLoading, bookingConn, bookingTran, enquiryConn, enquiryTran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            bookingTran.Rollback();
                            enquiryTran.Rollback();
                            _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                            _iFinalBookingData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error while InsertFinalEnquiryData";
                            return resultMessage;
                        }

                        #region Delete transactional data
                        result =_iFinalBookingData .DeleteTempLoadingData (tempLoadingTO.IdLoading, bookingConn, bookingTran);
                        if (result < 0)
                        {
                            bookingTran.Rollback();
                            enquiryTran.Rollback();
                            _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                            _iFinalBookingData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error while DeleteTempLoadingData";
                            return resultMessage;
                        }
                        #endregion

                        #region Create Excel File. Delete Stock & Quota. Reset SQL Connection.
                        if (loadingCount == Constants.LoadingCountForDataExtraction || totalLoading == tempLoadingTOList.Count)
                        {
                            #region Create Excel File  

                            if (tblInvoiceRptTOList != null && tblInvoiceRptTOList.Count > 0)
                            {
                                List<TblInvoiceRptTO> enquiryInvoiceList = new List<TblInvoiceRptTO>();

                                if (loadingIdList != null && loadingIdList.Count > 0)
                                {
                                    foreach (var loadingId in loadingIdList)
                                    {
                                        enquiryInvoiceList.AddRange(tblInvoiceRptTOList.FindAll(ele => ele.LoadingId == loadingId));
                                    }
                                }

                                if (enquiryInvoiceList != null && enquiryInvoiceList.Count > 0)
                                {
                                    result = _iFinalBookingData.CreateTempInvoiceExcel(enquiryInvoiceList, bookingConn, bookingTran);

                                    if (result != 1)
                                    {
                                        bookingTran.Rollback();
                                        enquiryTran.Rollback();
                                        _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                        _iFinalBookingData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                        resultMessage.MessageType = ResultMessageE.Error;
                                        resultMessage.Text = "Error while creating excel file.";
                                        return resultMessage;
                                    }
                                }
                            }
                            else
                            {
                                resultMessage.MessageType = ResultMessageE.Information;
                                resultMessage.Text = "Information : tblInvoiceRptTOList is null. Excel file is not created.";
                                //return resultMessage;
                            }
                            #endregion

                            #region Delete Stock And Quota
                            result = _iFinalBookingData.DeleteYesterdaysStock(bookingConn, bookingTran);
                            if (result < 0)
                            {
                                bookingTran.Rollback();
                                enquiryTran.Rollback();
                                _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                _iFinalBookingData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error while DeleteYesterdaysStock";
                                return resultMessage;
                            }

                            result = _iFinalBookingData.DeleteYesterdaysLoadingQuotaDeclaration(bookingConn, bookingTran);
                            if (result < 0)
                            {
                                bookingTran.Rollback();
                                enquiryTran.Rollback();
                                _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
                                _iFinalBookingData.UpdateIdentityFinalTables(enquiryConn, enquiryTran);
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error while DeleteYesterdaysQuotaDeclaration";
                                return resultMessage;
                            }

                            #endregion


                            bookingTran.Commit();
                            bookingConn.Close();
                            bookingTran.Dispose();

                            if (configParamsTO.ConfigParamVal == "1")
                            {
                                enquiryTran.Commit();
                                enquiryConn.Close();
                                enquiryTran.Dispose();
                            }

                            loadingCount = 0;
                            loadingIdList.Clear();
                        }
                        #endregion

                        #region  Old Code


                        //// Vaibhav [23-April-2018] For new changes - Single invoice against multiple loadingslip. To check all loading slip are delivered.
                        //// Select temp loading slip details.
                        //List<TblLoadingSlipTO> loadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails (tempLoadingTO.IdLoading, bookingConn, bookingTran);
                        //int undeliveredLoadingSlipCount = 0;
                        //List<TblLoadingSlipTO> loadingSlipDataByInvoiceId = null;

                        //if (loadingSlipTOList != null && loadingSlipTOList.Count > 0) {
                        //    foreach (var loadingSlip in loadingSlipTOList) {
                        //        List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOListByLoadingSlip(loadingSlip.IdLoadingSlip, bookingConn, bookingTran);
                        //        TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = null;
                        //        if (tempLoadingSlipInvoiceTOList != null && tempLoadingSlipInvoiceTOList.Count > 0)
                        //             tempLoadingSlipInvoiceTO = tempLoadingSlipInvoiceTOList[0];

                        //        if (tempLoadingSlipInvoiceTO != null) {
                        //            loadingSlipDataByInvoiceId = _iTblInvoiceBL.SelectLoadingSlipDetailsByInvoiceId (tempLoadingSlipInvoiceTO.InvoiceId, bookingConn, bookingTran);
                        //            if (loadingSlipDataByInvoiceId != null) {
                        //                undeliveredLoadingSlipCount += loadingSlipDataByInvoiceId.FindAll (ele => ele.StatusId != (int) Constants.TranStatusE.LOADING_DELIVERED && ele.StatusId != (int) Constants.TranStatusE.LOADING_CANCEL).Count ();
                        //            } else {

                        //            }
                        //        }
                        //    }
                        //}
                        //if (undeliveredLoadingSlipCount > 0) {
                        //    tempLoadingTOList.RemoveAll (ele => ele.IdLoading == tempLoadingTO.IdLoading);
                        //    goto creatFile;
                        //}

                        //processedLoadings.Clear ();
                        //if (loadingSlipDataByInvoiceId != null && loadingSlipDataByInvoiceId.Count > 0)
                        //    processedLoadings.AddRange (loadingSlipDataByInvoiceId.Select (ele => ele.LoadingId).Distinct ().ToList ());

                        //List<TblLoadingTO> newLoadingTOList = new List<TblLoadingTO> ();

                        //if (processedLoadings != null && processedLoadings.Count > 0) {
                        //    foreach (var processedLoading in processedLoadings) {
                        //        newLoadingTOList.AddRange (tempLoadingTOList.FindAll (e => e.IdLoading == processedLoading));
                        //    }

                        //    foreach (var newLoadingTO in newLoadingTOList) {
                        //        loadingIdList.Add (newLoadingTO.IdLoading);

                        //        #region Handle Connection
                        //        loadingCount = loadingCount + 1;
                        //        totalLoading = totalLoading + 1;

                        //        if (bookingConn.State == ConnectionState.Closed) {
                        //            bookingConn.Open ();
                        //            bookingTran = bookingConn.BeginTransaction ();
                        //        }

                        //        if (configParamsTO.ConfigParamVal == "1") {
                        //            if (enquiryConn.State == ConnectionState.Closed) {
                        //                enquiryConn.Open ();
                        //                enquiryTran = enquiryConn.BeginTransaction ();
                        //            }
                        //        }
                        //        #endregion

                        //        #region Insert Booking Data
                        //        resultMessage = _iFinalBookingData.InsertFinalBookingData (newLoadingTO.IdLoading, bookingConn, bookingTran, ref invoiceIdsList);
                        //        if (resultMessage.MessageType != ResultMessageE.Information) {
                        //            bookingTran.Rollback ();
                        //            enquiryTran.Rollback ();
                        //            _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                        //            _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                        //            resultMessage.MessageType = ResultMessageE.Error;
                        //            resultMessage.Text = "Error while InsertFinalBookingData";
                        //            return resultMessage;
                        //        }
                        //        #endregion

                        //        #region Insert Enquiry Data

                        //        //TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_MIGRATE_ENQUIRY_DATA);

                        //        if (configParamsTO.ConfigParamVal == "1") {
                        //            resultMessage = _iFinalEnquiryData.InsertFinalEnquiryData (newLoadingTO.IdLoading, bookingConn, bookingTran, enquiryConn, enquiryTran, ref invoiceIdsList);
                        //            if (resultMessage.MessageType != ResultMessageE.Information) {
                        //                bookingTran.Rollback ();
                        //                enquiryTran.Rollback ();
                        //                _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                        //                _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                        //                resultMessage.MessageType = ResultMessageE.Error;
                        //                resultMessage.Text = "Error while InsertFinalEnquiryData";
                        //                return resultMessage;
                        //            }
                        //        }

                        //        #endregion

                        //    }
                        //    #region Delete transactional data

                        //    foreach (var newLoadingTO in newLoadingTOList) {
                        //        result = _iFinalBookingData.DeleteTempLoadingData (newLoadingTO.IdLoading, bookingConn, bookingTran);
                        //        if (result < 0) {
                        //            bookingTran.Rollback ();
                        //            enquiryTran.Rollback ();
                        //            _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                        //            _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                        //            resultMessage.MessageType = ResultMessageE.Error;
                        //            resultMessage.Text = "Error while DeleteTempLoadingData";
                        //            return resultMessage;
                        //        }

                        //        tempLoadingTOList.RemoveAll (ele => ele.IdLoading == newLoadingTO.IdLoading);
                        //        totalLoading = totalLoading - 1;
                        //    }
                        //}
                        //#endregion

                        //#region Create Excel File. Delete Stock & Quota. Reset SQL Connection.
                        //creatFile:
                        //    if (loadingCount == Constants.LoadingCountForDataExtraction || totalLoading == tempLoadingTOList.Count) {
                        //        #region Create Excel File  

                        //        TblConfigParamsTO createFileConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName (Constants.CP_CREATE_NC_DATA_FILE);

                        //        if (createFileConfigParamsTO.ConfigParamVal == "1") {
                        //            if (tblInvoiceRptTOList != null && tblInvoiceRptTOList.Count > 0) {
                        //                List<TblInvoiceRptTO> enquiryInvoiceList = new List<TblInvoiceRptTO> ();

                        //                if (loadingIdList != null && loadingIdList.Count > 0) {
                        //                    foreach (var loadingId in loadingIdList) {
                        //                        enquiryInvoiceList.AddRange (tblInvoiceRptTOList.FindAll (ele => ele.LoadingId == loadingId));
                        //                    }
                        //                }

                        //                if (enquiryInvoiceList != null && enquiryInvoiceList.Count > 0) {
                        //                    result = _iFinalBookingData.CreateTempInvoiceExcel (enquiryInvoiceList, bookingConn, bookingTran);

                        //                    if (result != 1) {
                        //                        bookingTran.Rollback ();
                        //                        enquiryTran.Rollback ();
                        //                        _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                        //                        _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                        //                        resultMessage.MessageType = ResultMessageE.Error;
                        //                        resultMessage.Text = "Error while creating excel file.";
                        //                        return resultMessage;
                        //                    }
                        //                }
                        //            } else {
                        //                resultMessage.MessageType = ResultMessageE.Information;
                        //                resultMessage.Text = "Information : tblInvoiceRptTOList is null. Excel file is not created.";
                        //                //return resultMessage;
                        //            }
                        //        }
                        //        #endregion

                        //        #region Delete Stock And Quota
                        //        result = _iFinalBookingData.DeleteYesterdaysStock (bookingConn, bookingTran);
                        //        if (result < 0) {
                        //            bookingTran.Rollback ();
                        //            enquiryTran.Rollback ();
                        //            _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                        //            _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                        //            resultMessage.MessageType = ResultMessageE.Error;
                        //            resultMessage.Text = "Error while DeleteYesterdaysStock";
                        //            return resultMessage;
                        //        }

                        //        result = _iFinalBookingData.DeleteYesterdaysLoadingQuotaDeclaration (bookingConn, bookingTran);
                        //        if (result < 0) {
                        //            bookingTran.Rollback ();
                        //            enquiryTran.Rollback ();
                        //            _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                        //            _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                        //            resultMessage.MessageType = ResultMessageE.Error;
                        //            resultMessage.Text = "Error while DeleteYesterdaysQuotaDeclaration";
                        //            return resultMessage;
                        //        }

                        //        #endregion

                        //        bookingTran.Commit ();
                        //        bookingConn.Close ();
                        //        bookingTran.Dispose ();

                        //        if (configParamsTO.ConfigParamVal == "1") {
                        //            enquiryTran.Commit ();
                        //            enquiryConn.Close ();
                        //            enquiryTran.Dispose ();
                        //        }

                        //        loadingCount = 0;
                        //        loadingIdList.Clear ();
                        //    }
                        //#endregion

                        #endregion
                    }
                    
                }

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                bookingTran.Rollback ();
                _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);

                if (enquiryTran.Connection.State == ConnectionState.Open) {
                    enquiryTran.Rollback ();
                    _iTblInvoiceBL.UpdateIdentityFinalTables (enquiryConn, enquiryTran);
                }
                resultMessage.DefaultExceptionBehaviour (ex, "ExtractEnquiryData");
                return resultMessage;
            } finally {
                bookingConn.Close ();
                enquiryConn.Close ();
            }
        }
        public ResultMessage DeleteDispatchData () {
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            SqlConnection bookingConn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction bookingTran = null;
            List<TblLoadingTO> tempLoadingTOList = new List<TblLoadingTO> ();
            List<int> processedLoadings = new List<int> ();

            int result = 0;
            int loadingCount = 0;
            int totalLoading = 0;
            List<int> loadingIdList = new List<int> ();
            List<int> bookingIdList = new List<int> ();
            String bookingIds = String.Empty;

            try {

                if (bookingConn.State == ConnectionState.Closed) {
                    bookingConn.Open ();
                    bookingTran = bookingConn.BeginTransaction ();
                }

                // Select temp loading data.
                tempLoadingTOList = SelectAllTempLoadingOnStatus (bookingConn, bookingTran);
                if (tempLoadingTOList == null || tempLoadingTOList.Count <= 0) {
                    resultMessage.DefaultBehaviour ("Record not found!! ");
                    resultMessage.MessageType = ResultMessageE.Information;
                    return resultMessage;
                }

                int configId = _iTblConfigParamsDAO.IoTSetting();
                if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    tempLoadingTOList = tempLoadingTOList.Where(w => w.ModbusRefId == 0).ToList();
                }

                if (tempLoadingTOList != null && tempLoadingTOList.Count > 0) {

                    foreach (var tempLoadingTO in tempLoadingTOList.ToList ()) {

                        if (bookingConn.State == ConnectionState.Closed) {
                            bookingConn.Open ();
                            bookingTran = bookingConn.BeginTransaction ();
                        }

                        //List<TblLoadingSlipTO> loadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tempLoadingTO.IdLoading, bookingConn, bookingTran);

                        List<TblLoadingSlipTO> loadingSlipDataByInvoiceId = null;

                        //if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
                        //{

                        //foreach (var loadingSlip in loadingSlipTOList)
                        //{
                        //   // bookingIds += "," + loadingSlip.BookingId;
                        //    //bookingIds = bookingIds.Trim(',');
                        //     bookingIdList.Add(loadingSlip.BookingId);
                        //    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOListByLoadingSlip(loadingSlip.IdLoadingSlip, bookingConn, bookingTran);

                        //    if (tempLoadingSlipInvoiceTO != null)
                        //    {
                        //        loadingSlipDataByInvoiceId = BL.TblInvoiceBL.SelectLoadingSlipDetailsByInvoiceId(tempLoadingSlipInvoiceTO.InvoiceId, bookingConn, bookingTran);
                        //    }
                        //}
                        //}

                        processedLoadings.Clear ();
                        if (loadingSlipDataByInvoiceId != null && loadingSlipDataByInvoiceId.Count > 0)
                            processedLoadings.AddRange (loadingSlipDataByInvoiceId.Select (ele => ele.LoadingId).Distinct ().ToList ());

                        processedLoadings.Add (tempLoadingTO.IdLoading);

                        List<TblLoadingTO> newLoadingTOList = new List<TblLoadingTO> ();

                        if (processedLoadings != null && processedLoadings.Count > 0) {
                            //foreach (var processedLoading in processedLoadings)
                            //{
                            //    newLoadingTOList.AddRange(tempLoadingTOList.FindAll(e => e.IdLoading == processedLoading));
                            //}
                            //foreach (var newLoadingTO in newLoadingTOList)
                            //{
                            //    loadingIdList.Add(newLoadingTO.IdLoading);
                            //    loadingCount = loadingCount + 1;
                            //    totalLoading = totalLoading + 1;

                            //    if (bookingConn.State == ConnectionState.Closed)
                            //    {
                            //        bookingConn.Open();
                            //        bookingTran = bookingConn.BeginTransaction();
                            //    }
                            //}

                            if (bookingConn.State == ConnectionState.Closed) {
                                bookingConn.Open ();
                                bookingTran = bookingConn.BeginTransaction ();
                            }

                            newLoadingTOList.Add (tempLoadingTO);

                            #region Delete transactional data

                            foreach (var newLoadingTO in newLoadingTOList) {
                                result = _iFinalBookingData.DeleteDispatchTempLoadingData (newLoadingTO.IdLoading, bookingConn, bookingTran);
                                if (result < 0) {
                                    bookingTran.Rollback ();
                                    _iFinalBookingData.UpdateIdentityFinalTables (bookingConn, bookingTran);
                                    resultMessage.MessageType = ResultMessageE.Error;
                                    resultMessage.Text = "Error while DeleteDispatchTempLoadingData";
                                    return resultMessage;
                                }

                                tempLoadingTOList.RemoveAll (ele => ele.IdLoading == newLoadingTO.IdLoading);
                                totalLoading = totalLoading - 1;

                            }

                            bookingTran.Commit ();
                            bookingConn.Close ();

                        }
                        #endregion
                    }

                    if (bookingConn.State == ConnectionState.Closed) {
                        bookingConn.Open ();
                        bookingTran = bookingConn.BeginTransaction ();
                    }

                    resultMessage = DeleteAllBookings (bookingIdList, bookingConn, bookingTran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                        bookingTran.Rollback ();
                        resultMessage.DefaultBehaviour ("Error while Deleting BookingDispatchData");
                        return resultMessage;
                    }

                    bookingTran.Commit ();
                    bookingConn.Close ();
                    bookingTran.Dispose ();
                    loadingCount = 0;
                    loadingIdList.Clear ();
                }

                resultMessage.DefaultSuccessBehaviour ();
                return resultMessage;
            } catch (Exception ex) {
                bookingTran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "DeleteDispatchData");
                return resultMessage;
            } finally {
                bookingConn.Close ();
            }
        }
        public ResultMessage DeleteAllBookings (List<Int32> bookingsIdList) {
            SqlConnection conn = new SqlConnection (_iConnectionString.GetConnectionString (Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new StaticStuff.ResultMessage ();
            DateTime txnDateTime = _iCommon.ServerDateTime;
            try {
                conn.Open ();
                tran = conn.BeginTransaction ();

                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information) {

                    #region Check Final Item

                    resultMessage = DeleteAllBookings (bookingsIdList, conn, tran);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information) {
                        tran.Rollback ();
                        return resultMessage;
                    }

                    #endregion

                    tran.Commit ();
                }
                return resultMessage;
            } catch (Exception ex) {

                tran.Rollback ();
                resultMessage.DefaultExceptionBehaviour (ex, "Exception Error In MEthod RemoveItemFromLoadingSlip");
                return resultMessage;

            } finally {
                conn.Close ();
            }
        }
        public ResultMessage DeleteAllBookings (List<int> bookingsIdsList, SqlConnection conn, SqlTransaction tran) {
            ResultMessage resultMessage = new ResultMessage ();

            List<TblBookingsTO> allBooking = _iTblBookingsDAO.SelectAllTblBookings ();

            allBooking = allBooking.Where (w => w.PendingQty == 0).ToList ();

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams (Constants.CP_DELETE_BEFORE_DAYS, conn, tran);
            if (tblConfigParamsTO == null) {
                resultMessage.DefaultBehaviour ("Error tblConfigParamsTO is null");
                return null;
            }

            DateTime statusDate = _iCommon.ServerDateTime.AddDays (-Convert.ToInt32 (tblConfigParamsTO.ConfigParamVal));

            allBooking = allBooking.Where (w => w.CreatedOn.Date <= statusDate.Date).ToList ();

            for (int i = 0; i < allBooking.Count; i++) {
                TblBookingsTO TblBookingsTOTemp = allBooking[i];

                if (TblBookingsTOTemp.PendingQty <= 0) {
                    List<TblLoadingSlipDtlTO> tblLoadingSlipDtlTOList = _iTblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromBookingId (TblBookingsTOTemp.IdBooking, conn, tran);

                    if (tblLoadingSlipDtlTOList == null || tblLoadingSlipDtlTOList.Count == 0) {
                        Int32 result1 = DeleteDispatchBookingData (TblBookingsTOTemp.IdBooking, conn, tran);
                        if (result1 < 0) {
                            //tran.Rollback();
                            //resultMessage.DefaultBehaviour("Error while Deleting BookingDispatchData");

                        }
                    }
                }
            }

            //if (bookingsIdsList != null && bookingsIdsList.Count > 0)
            //{
            //    for (int s = 0; s < bookingsIdsList.Count; s++)
            //    {
            //        Int32 bookingId = bookingsIdsList[s];
            //        TblBookingsTO tblBookingTO = BL.TblBookingsBL.SelectBookingsTOWithDetails(bookingId);
            //        if (tblBookingTO != null)
            //        {

            //            if (tblBookingTO.PendingQty <= 0)
            //            {
            //                result = BL.FinalBookingData.DeleteDispatchBookingData(tblBookingTO.IdBooking, conn, tran);
            //                if (result < 0)
            //                {
            //                    tran.Rollback();
            //                    resultMessage.DefaultBehaviour("Error while Deleting BookingDispatchData");

            //                }

            //            }
            //        }
            //    }

            //}
            resultMessage.DefaultSuccessBehaviour ();
            return resultMessage;
        }

        public int DeleteDispatchBookingData (Int32 bookingId, SqlConnection conn, SqlTransaction tran) {

            ResultMessage resultMessage = new ResultMessage ();
            int result = 0;

            try {
                #region Delete Booking Data
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingBeyondQuota.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingBeyondQuota");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingExt.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingExt");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingParities.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingParities");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingQtyConsumption.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingQtyConsumption");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingDelAddr.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingDelAddr");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingSchedule.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingSchedule");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblQuotaConsumHistory.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblQuotaConsumHistory");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookingOpngBal.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookingOpngBal");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblLoadingSlipRemovedItems.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblLoadingSlipRemovedItems");
                    return -1;
                }
                result = DeleteDispatchBookingTempData (FinalBookingData.DelTranTablesE.tblBookings.ToString (), bookingId, conn, tran);
                if (result < 0) {
                    resultMessage.DefaultBehaviour ("Error while Deleting tblBookings");
                    return -1;
                }

                #endregion

                resultMessage.DefaultSuccessBehaviour ();
                return 1;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "DeleteDispatchBookingTempData");
                return -1;
            } finally {

            }
        }

        private int DeleteDispatchBookingTempData (String delTableName, Int32 delId, SqlConnection conn, SqlTransaction tran) {
            SqlCommand cmdDelete = new SqlCommand ();
            ResultMessage resultMessage = new ResultMessage ();

            try {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = null;

                //Saket [2018-05-11] Added.
                //String strWhereCondtion = "select invoiceId FROM tempLoadingSlipInvoice WHERE loadingSlipId = " + delId + "";
                // String strWhereCondtion = delId.ToString();

                switch ((FinalBookingData.DelTranTablesE) Enum.Parse (typeof (FinalBookingData.DelTranTablesE), delTableName)) {
                    case FinalBookingData.DelTranTablesE.tblBookingBeyondQuota:
                        //sqlQuery = " DELETE FROM tblBookingBeyondQuota WHERE bookingId = " + delId + "AND pendingQty <= 0 ";
                        sqlQuery = " DELETE FROM tblBookingBeyondQuota WHERE bookingId =" + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookingExt:
                        sqlQuery = " DELETE FROM tblBookingExt WHERE scheduleId IN " +
                            ("(select idSchedule from tblBookingSchedule where bookingId = " + delId + ")") + " OR bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookingParities:
                        sqlQuery = " DELETE FROM tblBookingParities WHERE bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookingQtyConsumption:
                        sqlQuery = " DELETE FROM tblBookingQtyConsumption WHERE bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookingDelAddr:
                        sqlQuery = " DELETE FROM tblBookingDelAddr WHERE scheduleId IN " +
                            ("(select idSchedule from tblBookingSchedule where bookingId = " + delId + ")");
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookingSchedule:
                        sqlQuery = " DELETE FROM tblBookingSchedule WHERE bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblQuotaConsumHistory:
                        sqlQuery = " DELETE FROM tblQuotaConsumHistory WHERE bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookingOpngBal:
                        sqlQuery = " DELETE FROM tblBookingOpngBal WHERE bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblLoadingSlipRemovedItems:
                        sqlQuery = " DELETE FROM tblLoadingSlipRemovedItems WHERE bookingId = " + delId;
                        break;
                    case FinalBookingData.DelTranTablesE.tblBookings:
                        sqlQuery = " DELETE FROM tblBookings WHERE idbooking = " + delId;
                        break;
                }

                if (sqlQuery != null) {
                    cmdDelete.CommandText = sqlQuery;
                    return cmdDelete.ExecuteNonQuery ();
                } else
                    return -1;
            } catch (Exception ex) {
                resultMessage.DefaultExceptionBehaviour (ex, "DeleteTempData");
                return -1;
            } finally {
                cmdDelete.Dispose ();
            }
        }
        #endregion
        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime loadingDate)
        {
            return _iTblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo, loadingDate);
        }

        public List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo)
        {
            return _iTblLoadingDAO.SelectAllLoadingListByVehicleNo(vehicleNo);
        }
        public List<TblLoadingTO> SelectLoadingTOWithDetailsByLoadingNoForSupport(string loadingSlipNo)
        {
            return _iTblLoadingDAO.SelectLoadingTOWithDetailsByLoadingNoForSupport(loadingSlipNo);
        }

        public ResultMessage PrintReport(int idLoading, bool isPrinted)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                TblLoadingTO LoadingTO = SelectLoadingTOWithDetails(idLoading);
                double totalBundleQty = 0;
                double totalLoadingQty = 0;
                double loadindSlipId = 0;
                if (LoadingTO != null)
                {
                    Int32 IS_REQUIRE_DIFFERENT_DT_FOR_6MM_MATERIAL = 0;
                    TblConfigParamsTO ConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_IS_REQUIRE_DIFFERENT_DT_FOR_6MM_MATERIAL);
                    if (ConfigParamsTO != null)
                    {
                        IS_REQUIRE_DIFFERENT_DT_FOR_6MM_MATERIAL = Convert.ToInt32(ConfigParamsTO.ConfigParamVal);
                    }
                    //Reshma Added For Tptal Column
                    int SHOW_LOADING_SLIP_TOTAL_VALUE = 0;
                    TblConfigParamsTO ConfigParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.SHOW_LOADING_SLIP_TOTAL_VALUE) ;
                    if (ConfigParamsTOTemp != null)
                    {
                        SHOW_LOADING_SLIP_TOTAL_VALUE = Convert.ToInt32(ConfigParamsTOTemp.ConfigParamVal);
                        ////return resultMsg;
                    }
                    DataSet printDataSet = new DataSet();

                    //headerDT
                    DataTable headerDT = new DataTable();
                    DataTable addressDT = new DataTable();
                    DataTable loadingDT = new DataTable();
                    DataTable loadingItemDT = new DataTable();
                    DataTable loadingItemTotalDT = new DataTable();

                    DataTable specificItemLoadingDT = new DataTable();
                    DataTable itemFooterDetailsDT = new DataTable();

                    DataTable multipleInvoiceCopyDT = new DataTable();
                    headerDT.TableName = "headerDT";
                    loadingDT.TableName = "loadingDT";
                    addressDT.TableName = "addressDT";
                    loadingItemDT.TableName = "loadingItemDT";
                    loadingItemTotalDT.TableName = "loadingItemTotalDT";

                    specificItemLoadingDT.TableName = "specificItemLoadingDT";
                    itemFooterDetailsDT.TableName = "itemFooterDetailsDT";

                    headerDT.Columns.Add("CreatedOnStr");
                    headerDT.Columns.Add("CnfOrgName");
                    headerDT.Columns.Add("TransporterOrgName");
                    headerDT.Columns.Add("DriverName");
                    headerDT.Columns.Add("ContactNo");
                    headerDT.Columns.Add("LoadingSlipNo");
                    headerDT.Columns.Add("DealerOrgName");
                    headerDT.Columns.Add("VehicleNo");
                    headerDT.Columns.Add("LoadingId");
                    headerDT.Columns.Add("BookingRate");
                    headerDT.Columns.Add("CdStructure");
                    headerDT.Columns.Add("IsConfirmed");
                    headerDT.Columns.Add("FreightAmt");
                    headerDT.Columns.Add("BillingName");
                    headerDT.Columns.Add("ConsigneeName");
                    headerDT.Columns.Add("BillingAddress");
                    headerDT.Columns.Add("ConsigneeAddress");
                    headerDT.Columns.Add("BillingGstNo");
                    headerDT.Columns.Add("ConsigneeGstNo");
                    headerDT.Columns.Add("ShippingTo");
                    headerDT.Columns.Add("ShippingAddress");
                    headerDT.Columns.Add("LoadingSlipId");
                    headerDT.Columns.Add("LoadingLayerDesc");
                    headerDT.Columns.Add("DriverContactNo");
                    headerDT.Columns.Add("PreparedBy");
                    headerDT.Columns.Add("Comment");
                    headerDT.Columns.Add("LayerNo");
                    headerDT.Columns.Add("BrandDesc");
                    


                    loadingItemDT.Columns.Add("DisplayName");
                    loadingItemDT.Columns.Add("MaterialDesc");
                    loadingItemDT.Columns.Add("ProdItemDesc");
                    loadingItemDT.Columns.Add("LoadingQty");
                    loadingItemDT.Columns.Add("Bundles");
                    loadingItemDT.Columns.Add("LoadedWeight");
                    loadingItemDT.Columns.Add("MstLoadedBundles");
                    loadingItemDT.Columns.Add("LoadedBundles");
                    loadingItemDT.Columns.Add("RatePerMT");
                    loadingItemDT.Columns.Add("LoadingSlipId");
                    loadingItemDT.Columns.Add("BrandDesc");
                    loadingItemDT.Columns.Add("ProdSpecDesc");
                    loadingItemDT.Columns.Add("ProdcatDesc");
                    loadingItemDT.Columns.Add("ItemName");
                    loadingItemDT.Columns.Add("DisplayField");

                    specificItemLoadingDT.Columns.Add("DisplayName");
                    specificItemLoadingDT.Columns.Add("MaterialDesc");
                    specificItemLoadingDT.Columns.Add("ProdItemDesc");
                    specificItemLoadingDT.Columns.Add("LoadingQty");
                    specificItemLoadingDT.Columns.Add("Bundles");
                    specificItemLoadingDT.Columns.Add("LoadedWeight");
                    specificItemLoadingDT.Columns.Add("MstLoadedBundles");
                    specificItemLoadingDT.Columns.Add("LoadedBundles");
                    specificItemLoadingDT.Columns.Add("RatePerMT");
                    specificItemLoadingDT.Columns.Add("LoadingSlipId");
                    specificItemLoadingDT.Columns.Add("BrandDesc");
                    specificItemLoadingDT.Columns.Add("ProdSpecDesc");
                    specificItemLoadingDT.Columns.Add("ProdcatDesc");
                    specificItemLoadingDT.Columns.Add("ItemName");
                    specificItemLoadingDT.Columns.Add("DisplayField");

                    if (LoadingTO.LoadingSlipList != null && LoadingTO.LoadingSlipList.Count > 0)
                    {
                        for (int i = 0; i < LoadingTO.LoadingSlipList.Count; i++)
                        {
                            TblLoadingSlipTO loadingSlipTo = LoadingTO.LoadingSlipList[i];
                            headerDT.Rows.Add();
                            Int32 loadHeaderDTCount = headerDT.Rows.Count - 1;
                            totalLoadingQty = 0;
                            totalBundleQty = 0;
                            headerDT.Rows[loadHeaderDTCount]["LayerNo"] = i+1;
                            headerDT.Rows[loadHeaderDTCount]["CreatedOnStr"] = LoadingTO.CreatedOnStr;
                            headerDT.Rows[loadHeaderDTCount]["CnfOrgName"] = LoadingTO.CnfOrgName;
                            headerDT.Rows[loadHeaderDTCount]["TransporterOrgName"] = LoadingTO.TransporterOrgName;
                            headerDT.Rows[loadHeaderDTCount]["DriverName"] = LoadingTO.DriverName;
                            headerDT.Rows[loadHeaderDTCount]["LoadingSlipNo"] = loadingSlipTo.LoadingSlipNo;
                            headerDT.Rows[loadHeaderDTCount]["DealerOrgName"] = loadingSlipTo.DealerOrgName;
                            headerDT.Rows[loadHeaderDTCount]["VehicleNo"] = LoadingTO.VehicleNo;
                            headerDT.Rows[loadHeaderDTCount]["LoadingId"] = loadingSlipTo.LoadingId;
                            headerDT.Rows[loadHeaderDTCount]["BookingRate"] = loadingSlipTo.TblLoadingSlipDtlTO.BookingRate;
                            headerDT.Rows[loadHeaderDTCount]["CdStructure"] = loadingSlipTo.CdStructure;
                            headerDT.Rows[loadHeaderDTCount]["Comment"] = loadingSlipTo.Comment;
                            headerDT.Rows[loadHeaderDTCount]["PreparedBy"] = LoadingTO.CreatedByUserName;
                            headerDT.Rows[loadHeaderDTCount]["LoadingSlipId"] = loadingSlipTo.IdLoadingSlip;
                            headerDT.Rows[loadHeaderDTCount]["DriverContactNo"] = LoadingTO.ContactNo;

                            if (loadingSlipTo.IsConfirmed == 1)
                            {
                                headerDT.Rows[loadHeaderDTCount]["IsConfirmed"] = "Confirmed";

                            }
                            else
                            {
                                headerDT.Rows[loadHeaderDTCount]["IsConfirmed"] = "-";

                            }
                            if (loadingSlipTo.IsFreightIncluded == 1)
                            {
                                headerDT.Rows[loadHeaderDTCount]["FreightAmt"] = loadingSlipTo.FreightAmt + " (Included)";
                            }
                            else
                            {
                                headerDT.Rows[loadHeaderDTCount]["FreightAmt"] = loadingSlipTo.FreightAmt + " (NotIncluded)";
                            }

                            if (loadingSlipTo.DeliveryAddressTOList.Count > 0)
                            {
                                TblLoadingSlipAddressTO addressTO = loadingSlipTo.DeliveryAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                                if (addressTO != null)
                                {
                                    headerDT.Rows[loadHeaderDTCount]["BillingName"] = addressTO.BillingName;
                                    headerDT.Rows[loadHeaderDTCount]["BillingAddress"] = addressTO.Address;
                                    headerDT.Rows[loadHeaderDTCount]["BillingGstNo"] = addressTO.GstNo;
                                    headerDT.Rows[loadHeaderDTCount]["ContactNo"] = addressTO.ContactNo;

                                }

                                TblLoadingSlipAddressTO addressTOC = loadingSlipTo.DeliveryAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS).FirstOrDefault();
                                if (addressTOC != null)
                                {
                                    headerDT.Rows[loadHeaderDTCount]["ConsigneeName"] = addressTOC.BillingName;
                                    headerDT.Rows[loadHeaderDTCount]["ConsigneeAddress"] = addressTOC.Address;
                                    headerDT.Rows[loadHeaderDTCount]["ConsigneeGstNo"] = addressTOC.GstNo;
                                }

                                TblLoadingSlipAddressTO addressTOs = loadingSlipTo.DeliveryAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.SHIPPING_ADDRESS).FirstOrDefault();
                                if (addressTOs != null)
                                {

                                    headerDT.Rows[loadHeaderDTCount]["ShippingTo"] = addressTOs.BillingName;
                                    headerDT.Rows[loadHeaderDTCount]["ShippingAddress"] = addressTOs.Address;
                                }
                            }


                            for (int j = 0; j < loadingSlipTo.LoadingSlipExtTOList.Count; j++)
                            {
                              
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = loadingSlipTo.LoadingSlipExtTOList[j];
                                if(IS_REQUIRE_DIFFERENT_DT_FOR_6MM_MATERIAL == 1 && tblLoadingSlipExtTO.MaterialId == (Int32)Constants.TblMaterialEnum.SIX_MM)
                                {
                                    specificItemLoadingDT.Rows.Add();
                                    Int32 loadItemDTCount = specificItemLoadingDT.Rows.Count - 1;

                                    specificItemLoadingDT.Rows[loadItemDTCount]["DisplayName"] = tblLoadingSlipExtTO.DisplayName;

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.MaterialDesc))
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["DisplayField"] = tblLoadingSlipExtTO.MaterialDesc;
                                    }
                                    else
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["DisplayField"] = tblLoadingSlipExtTO.ItemName;
                                    }
                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.MaterialDesc))
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["MaterialDesc"] = tblLoadingSlipExtTO.MaterialDesc;
                                    }
                                    else
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["MaterialDesc"] = "";
                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ItemName))
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["ItemName"] = tblLoadingSlipExtTO.ItemName;
                                    }
                                    else
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["ItemName"] = "";
                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ProdCatDesc))
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["ProdCatDesc"] = tblLoadingSlipExtTO.ProdCatDesc;
                                    }
                                    else
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["ProdCatDesc"] = "";

                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ProdSpecDesc))
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["ProdSpecDesc"] = tblLoadingSlipExtTO.ProdSpecDesc;
                                    }
                                    else
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["ProdSpecDesc"] = "";

                                    }
                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.BrandDesc))
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["BrandDesc"] = tblLoadingSlipExtTO.BrandDesc;
                                    }
                                    else
                                    {
                                        specificItemLoadingDT.Rows[loadItemDTCount]["BrandDesc"] = "";

                                    }

                                    specificItemLoadingDT.Rows[loadItemDTCount]["ProdItemDesc"] = tblLoadingSlipExtTO.ProdItemDesc;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["LoadingQty"] = tblLoadingSlipExtTO.LoadingQty;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["Bundles"] = tblLoadingSlipExtTO.Bundles;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["LoadedWeight"] = tblLoadingSlipExtTO.LoadedWeight;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["MstLoadedBundles"] = tblLoadingSlipExtTO.MstLoadedBundles;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["LoadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["RatePerMT"] = tblLoadingSlipExtTO.RatePerMT;
                                    specificItemLoadingDT.Rows[loadItemDTCount]["LoadingSlipId"] = tblLoadingSlipExtTO.LoadingSlipId;
                                    
                                }
                                else
                                {
                                    loadingItemDT.Rows.Add();
                                    Int32 loadItemDTCount = loadingItemDT.Rows.Count - 1;

                                    loadingItemDT.Rows[loadItemDTCount]["DisplayName"] = tblLoadingSlipExtTO.DisplayName;

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.MaterialDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["DisplayField"] = tblLoadingSlipExtTO.MaterialDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["DisplayField"] = tblLoadingSlipExtTO.ItemName;
                                    }
                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.MaterialDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["MaterialDesc"] = tblLoadingSlipExtTO.MaterialDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["MaterialDesc"] = "";
                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ItemName))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ItemName"] = tblLoadingSlipExtTO.ItemName;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ItemName"] = "";
                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ProdCatDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdCatDesc"] = tblLoadingSlipExtTO.ProdCatDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdCatDesc"] = "";

                                    }

                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.ProdSpecDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdSpecDesc"] = tblLoadingSlipExtTO.ProdSpecDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["ProdSpecDesc"] = "";

                                    }
                                    if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.BrandDesc))
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["BrandDesc"] = tblLoadingSlipExtTO.BrandDesc;
                                    }
                                    else
                                    {
                                        loadingItemDT.Rows[loadItemDTCount]["BrandDesc"] = "";

                                    }

                                    loadingItemDT.Rows[loadItemDTCount]["ProdItemDesc"] = tblLoadingSlipExtTO.ProdItemDesc;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadingQty"] = tblLoadingSlipExtTO.LoadingQty;
                                    loadingItemDT.Rows[loadItemDTCount]["Bundles"] = tblLoadingSlipExtTO.Bundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedWeight"] = tblLoadingSlipExtTO.LoadedWeight;
                                    loadingItemDT.Rows[loadItemDTCount]["MstLoadedBundles"] = tblLoadingSlipExtTO.MstLoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["RatePerMT"] = tblLoadingSlipExtTO.RatePerMT;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadingSlipId"] = tblLoadingSlipExtTO.LoadingSlipId;
                                    totalBundleQty += tblLoadingSlipExtTO.Bundles;
                                    totalLoadingQty += tblLoadingSlipExtTO.LoadingQty;
                                    loadindSlipId = tblLoadingSlipExtTO.LoadingSlipId;
                                } 
                                headerDT.Rows[loadHeaderDTCount]["LoadingLayerDesc"] = tblLoadingSlipExtTO.LoadingLayerDesc;
                                if (!string.IsNullOrEmpty(tblLoadingSlipExtTO.BrandDesc))
                                {
                                    headerDT.Rows[loadHeaderDTCount]["BrandDesc"] = tblLoadingSlipExtTO.BrandDesc;
                                }
                                else
                                {
                                    headerDT.Rows[loadHeaderDTCount]["BrandDesc"] = "";

                                }
                                
                            }
                            if (SHOW_LOADING_SLIP_TOTAL_VALUE == 1)
                            {
                                loadingItemDT.Rows.Add();
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["LoadingQty"] = totalLoadingQty;
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["Bundles"] = totalBundleQty;
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["ProdSpecDesc"] = "Total";
                                loadingItemDT.Rows[loadingItemDT.Rows.Count - 1]["LoadingSlipId"] = loadindSlipId;
                            }

                        }
                    }

                    loadingItemTotalDT.Columns.Add("LoadingQty");
                    loadingItemTotalDT.Columns.Add("Bundles");
                    loadingItemTotalDT.Columns.Add("ProdSpecDesc");
                    //Reshma Added.
                    if (loadingItemDT != null && loadingItemDT.Rows.Count > 0)
                    {
                        //loadingItemTotalDT.Rows.Add();
                        //loadingItemTotalDT.Rows[loadingItemTotalDT.Rows.Count - 1]["LoadingQty"] = totalLoadingQty;
                        //loadingItemTotalDT.Rows[loadingItemTotalDT.Rows.Count - 1]["Bundles"] = totalBundleQty;
                        //loadingItemTotalDT.Rows[loadingItemTotalDT.Rows.Count - 1]["ProdSpecDesc"] = "Total";

                    }
                    //headerDT = loadingDT.Copy();
                    headerDT.TableName = "headerDT";

                    printDataSet.Tables.Add(headerDT);
                    loadingItemDT.TableName = "loadingItemDT";
                    specificItemLoadingDT.TableName = "specificItemLoadingDT";
                    printDataSet.Tables.Add(loadingItemDT);
                    printDataSet.Tables.Add(specificItemLoadingDT);
                    printDataSet.Tables.Add(loadingItemTotalDT);

                    //creating template'''''''''''''''''
                    string templateName = "LoadingSlip";
                    String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                    String fileName = "Bill-" + DateTime.Now.Ticks;

                    //download location for rewrite  template file
                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    // RunReport runReport = new RunReport();
                    Boolean IsProduction = true;

                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                    if (tblConfigParamsTO != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                        {
                            IsProduction = false;
                        }
                    }
                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {
                            filePath = resultMessage.Tag.ToString();
                        }
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;
                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*Bill*");
                            string[] filesList = Directory.GetFiles(directoryName, "*Bill*");

                            foreach (string file in filesList)
                            {
                                //if (file.ToUpper().Contains(resFname.ToUpper()))
                                {
                                    File.Delete(file);
                                }
                            }
                        }
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                        }
                    }
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }

        }

    }
}