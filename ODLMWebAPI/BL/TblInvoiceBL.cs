using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using System.IO;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.IoT.Interfaces;
using static ODLMWebAPI.StaticStuff.Constants;
using ODLMWebAPI.IoT;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;
using ODLMWebAPI.DAL;
using OfficeOpenXml;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;


namespace ODLMWebAPI.BL
{
    public class TblInvoiceBL : ITblInvoiceBL
    {
        private readonly ITblInvoiceChangeOrgHistoryDAO _iTblInvoiceChangeOrgHistoryDAO;
        private readonly ITblInvoiceDAO _iTblInvoiceDAO;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblInvoiceItemDetailsBL _iTblInvoiceItemDetailsBL;
        private readonly ITblInvoiceItemTaxDtlsBL _iTblInvoiceItemTaxDtlsBL;
        private readonly ITblInvoiceAddressBL _iTblInvoiceAddressBL;
        private readonly ITblMaterialDAO _iTblMaterialDAO;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly ITblLoadingSlipBL _iTblLoadingSlipBL;
        private readonly ITblLoadingStatusHistoryDAO _iTblLoadingStatusHistoryDAO;
        private readonly ITempLoadingSlipInvoiceBL _iTempLoadingSlipInvoiceBL;
        private readonly ITblLoadingDAO _iTblLoadingDAO;
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITblLoadingSlipExtDAO _iTblLoadingSlipExtDAO;
        private readonly ITblStockConfigDAO _iTblStockConfigDAO;
        private readonly ITblBookingsBL _iTblBookingsBL;
        private readonly ITblLoadingSlipDtlDAO _iTblLoadingSlipDtlDAO;
        private readonly ITblWeighingMeasuresDAO _iTblWeighingMeasuresDAO;
        private readonly ITblParitySummaryDAO _iTblParitySummaryDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblProductItemDAO _iTblProductItemDAO;
        private readonly ITblProdGstCodeDtlsDAO _iTblProdGstCodeDtlsDAO;
        private readonly ITblGstCodeDtlsDAO _iTblGstCodeDtlsDAO;
        private readonly ITblTaxRatesDAO _iTblTaxRatesDAO;
        private readonly ITblOrganizationBL _iTblOrganizationBL;
        private readonly ITblOrgLicenseDtlDAO _iTblOrgLicenseDtlDAO;
        private readonly ITempInvoiceDocumentDetailsDAO _iTempInvoiceDocumentDetailsDAO;
        private readonly ITblAddonsFunDtlsDAO _iTblAddonsFunDtlsDAO;
        private readonly ITblOtherTaxesDAO _iTblOtherTaxesDAO;
        private readonly ITblInvoiceHistoryBL _iTblInvoiceHistoryBL;
        private readonly ITblDocumentDetailsBL _iTblDocumentDetailsBL;
        private readonly ITblInvoiceBankDetailsDAO _iTblInvoiceBankDetailsDAO;
        private readonly ITblInvoiceOtherDetailsDAO _iTblInvoiceOtherDetailsDAO;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly ITblInvoiceAddressDAO _iTblInvoiceAddressDAO;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly ITblAlertInstanceBL _iTblAlertInstanceBL;
        private readonly ITblEntityRangeDAO _iTblEntityRangeDAO;
        private readonly ITblBookingParitiesDAO _iTblBookingParitiesDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ISendMailBL _iSendMailBL;
        private readonly ITblEmailHistoryDAO _iTblEmailHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        private readonly IRunReport _iRunReport;
        private readonly ICircularDependencyBL _iCircularDependencyBL;
        private readonly IDimBrandDAO _iDimBrandDAO;
        private readonly ITblPaymentTermOptionRelationDAO _iTblPaymentTermOptionRelationDAO;
        private readonly ITblPaymentTermsForBookingBL _iTblPaymentTermsForBookingBL;
        private readonly ITblPaymentTermOptionRelationBL _iTblPaymentTermOptionRelationBL;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IIotCommunication _iIotCommunication;
        private readonly IGateCommunication _iGateCommunication;
        private readonly ITblLoadingSlipDAO _iTblLoadingSlipDAO;
        private readonly ITblAlertDefinitionDAO _iTblAlertDefinitionDAO;
        private readonly ITblEInvoiceApiDAO _iTblEInvoiceApiDAO;
        private readonly ITblEInvoiceApiResponseDAO _iTblEInvoiceApiResponseDAO;
        private readonly ITblEInvoiceSessionApiResponseDAO _iTblEInvoiceSessionApiResponseDAO;
        private readonly ITblOrgLicenseDtlBL _iTblOrgLicenseDtlBL;
        private readonly ITblProductItemBL _iTblProductItemBL;
        private readonly ITblProdGstCodeDtlsBL _iTblProdGstCodeDtlsBL;
        private readonly ITempLoadingSlipInvoiceDAO _iTempLoadingSlipInvoiceDAO;

        public TblInvoiceBL(ITblAlertDefinitionDAO iTblAlertDefinitionDAO, ITblLoadingSlipDAO iTblLoadingSlipDAO, IIotCommunication iIotCommunication, ITblInvoiceChangeOrgHistoryDAO iTblInvoiceChangeOrgHistoryDAO, IGateCommunication iGateCommunication, ITblConfigParamsDAO iTblConfigParamsDAO, ITblPaymentTermOptionRelationBL iTblPaymentTermOptionRelationBL, ITblPaymentTermsForBookingBL iTblPaymentTermsForBookingBL, ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, IDimBrandDAO iDimBrandDAO, ITblDocumentDetailsBL iTblDocumentDetailsBL, ITblBookingsBL iTblBookingsBL, ITblOrganizationBL iTblOrganizationBL, ITblInvoiceHistoryBL iTblInvoiceHistoryBL, IDimReportTemplateBL iDimReportTemplateBL, ITblAlertInstanceBL iTblAlertInstanceBL, ISendMailBL iSendMailBL, ICircularDependencyBL iCircularDependencyBL, ICommon iCommon, IConnectionString iConnectionString, ITblEmailHistoryDAO iTblEmailHistoryDAO, IRunReport iRunReport, ITblPersonDAO iTblPersonDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblUserDAO iTblUserDAO, ITblInvoiceAddressDAO iTblInvoiceAddressDAO, ITblInvoiceOtherDetailsDAO iTblInvoiceOtherDetailsDAO, ITblInvoiceBankDetailsDAO iTblInvoiceBankDetailsDAO, ITblOtherTaxesDAO iTblOtherTaxesDAO, ITempInvoiceDocumentDetailsDAO iTempInvoiceDocumentDetailsDAO, ITblOrgLicenseDtlDAO iTblOrgLicenseDtlDAO, ITblTaxRatesDAO iTblTaxRatesDAO, ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO, ITblProdGstCodeDtlsDAO iTblProdGstCodeDtlsDAO, ITblProductItemDAO iTblProductItemDAO, ITblParitySummaryDAO iTblParitySummaryDAO, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblStockConfigDAO iTblStockConfigDAO, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, IDimensionBL iDimensionBL, ITblLoadingDAO iTblLoadingDAO, ITempLoadingSlipInvoiceBL iTempLoadingSlipInvoiceBL, ITblLoadingSlipBL iTblLoadingSlipBL, ITblAddressBL iTblAddressBL, ITblInvoiceAddressBL iTblInvoiceAddressBL, ITblConfigParamsBL iTblConfigParamsBL, ITblInvoiceDAO iTblInvoiceDAO, ITblUserRoleBL iTblUserRoleBL, ITblInvoiceItemDetailsBL iTblInvoiceItemDetailsBL, ITblInvoiceItemTaxDtlsBL iTblInvoiceItemTaxDtlsBL, IDimensionDAO iDimensionDAO, ITblEInvoiceApiDAO iTblEInvoiceApiDAO, ITblEInvoiceApiResponseDAO iTblEInvoiceApiResponseDAO, ITblEInvoiceSessionApiResponseDAO iTblEInvoiceSessionApiResponseDAO, ITblOrgLicenseDtlBL iTblOrgLicenseDtlBL, ITblProductItemBL iTblProductItemBL, ITblProdGstCodeDtlsBL iTblProdGstCodeDtlsBL, ITblMaterialDAO iTblMaterialDAO
            , ITempLoadingSlipInvoiceDAO iTempLoadingSlipInvoiceDAO, ITblAddonsFunDtlsDAO iTblAddonsFunDtlsDAO, ITblLoadingStatusHistoryDAO iTblLoadingStatusHistoryDAO)
        // public TblInvoiceBL(ITblAlertDefinitionDAO iTblAlertDefinitionDAO,ITblInvoiceChangeOrgHistoryDAO iTblInvoiceChangeOrgHistoryDAO, ITblConfigParamsDAO iTblConfigParamsDAO, ITblPaymentTermOptionRelationBL iTblPaymentTermOptionRelationBL, ITblPaymentTermsForBookingBL iTblPaymentTermsForBookingBL, ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, IDimBrandDAO iDimBrandDAO, ITblDocumentDetailsBL iTblDocumentDetailsBL, ITblBookingsBL iTblBookingsBL, ITblOrganizationBL iTblOrganizationBL, ITblInvoiceHistoryBL iTblInvoiceHistoryBL, IDimReportTemplateBL iDimReportTemplateBL, ITblAlertInstanceBL iTblAlertInstanceBL, ISendMailBL iSendMailBL, ICircularDependencyBL iCircularDependencyBL, ICommon iCommon, IConnectionString iConnectionString, ITblEmailHistoryDAO iTblEmailHistoryDAO, IRunReport iRunReport, ITblPersonDAO iTblPersonDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblUserDAO iTblUserDAO, ITblInvoiceAddressDAO iTblInvoiceAddressDAO, ITblInvoiceOtherDetailsDAO iTblInvoiceOtherDetailsDAO, ITblInvoiceBankDetailsDAO iTblInvoiceBankDetailsDAO, ITblOtherTaxesDAO iTblOtherTaxesDAO, ITempInvoiceDocumentDetailsDAO iTempInvoiceDocumentDetailsDAO, ITblOrgLicenseDtlDAO iTblOrgLicenseDtlDAO, ITblTaxRatesDAO iTblTaxRatesDAO, ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO, ITblProdGstCodeDtlsDAO iTblProdGstCodeDtlsDAO, ITblProductItemDAO iTblProductItemDAO, ITblParitySummaryDAO iTblParitySummaryDAO, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblStockConfigDAO iTblStockConfigDAO, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, IDimensionBL iDimensionBL, ITblLoadingDAO iTblLoadingDAO, ITempLoadingSlipInvoiceBL iTempLoadingSlipInvoiceBL, ITblLoadingSlipBL iTblLoadingSlipBL, ITblAddressBL iTblAddressBL, ITblInvoiceAddressBL iTblInvoiceAddressBL, ITblConfigParamsBL iTblConfigParamsBL, ITblInvoiceDAO iTblInvoiceDAO, ITblUserRoleBL iTblUserRoleBL, ITblInvoiceItemDetailsBL iTblInvoiceItemDetailsBL, ITblInvoiceItemTaxDtlsBL iTblInvoiceItemTaxDtlsBL)
        {
            _iTblInvoiceChangeOrgHistoryDAO = iTblInvoiceChangeOrgHistoryDAO;
            _iTblInvoiceDAO = iTblInvoiceDAO;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblInvoiceItemDetailsBL = iTblInvoiceItemDetailsBL;
            _iTblInvoiceItemTaxDtlsBL = iTblInvoiceItemTaxDtlsBL;
            _iTblInvoiceAddressBL = iTblInvoiceAddressBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblAddressBL = iTblAddressBL;
            _iTblLoadingSlipBL = iTblLoadingSlipBL;
            _iTempLoadingSlipInvoiceBL = iTempLoadingSlipInvoiceBL;
            _iTblLoadingDAO = iTblLoadingDAO;
            _iDimensionBL = iDimensionBL;
            _iTblLoadingSlipExtDAO = iTblLoadingSlipExtDAO;
            _iTblStockConfigDAO = iTblStockConfigDAO;
            _iTblBookingsBL = iTblBookingsBL;
            _iTblLoadingSlipDtlDAO = iTblLoadingSlipDtlDAO;
            _iTblWeighingMeasuresDAO = iTblWeighingMeasuresDAO;
            _iTblParitySummaryDAO = iTblParitySummaryDAO;
            _iTblProductItemDAO = iTblProductItemDAO;
            _iTblProdGstCodeDtlsDAO = iTblProdGstCodeDtlsDAO;
            _iTblGstCodeDtlsDAO = iTblGstCodeDtlsDAO;
            _iTblTaxRatesDAO = iTblTaxRatesDAO;
            _iTblOrganizationBL = iTblOrganizationBL;
            _iTblOrgLicenseDtlDAO = iTblOrgLicenseDtlDAO;
            _iTempInvoiceDocumentDetailsDAO = iTempInvoiceDocumentDetailsDAO;
            _iTblOtherTaxesDAO = iTblOtherTaxesDAO;
            _iTblInvoiceHistoryBL = iTblInvoiceHistoryBL;
            _iTblDocumentDetailsBL = iTblDocumentDetailsBL;
            _iTblInvoiceBankDetailsDAO = iTblInvoiceBankDetailsDAO;
            _iTblInvoiceOtherDetailsDAO = iTblInvoiceOtherDetailsDAO;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iTblInvoiceAddressDAO = iTblInvoiceAddressDAO;
            _iTblUserDAO = iTblUserDAO;
            _iTblAlertInstanceBL = iTblAlertInstanceBL;
            _iTblEntityRangeDAO = iTblEntityRangeDAO;
            _iTblBookingParitiesDAO = iTblBookingParitiesDAO;
            _iTblPersonDAO = iTblPersonDAO;
            _iSendMailBL = iSendMailBL;
            _iTblEmailHistoryDAO = iTblEmailHistoryDAO;
            _iRunReport = iRunReport;
            _iCircularDependencyBL = iCircularDependencyBL;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iDimBrandDAO = iDimBrandDAO;
            _iTblPaymentTermOptionRelationDAO = iTblPaymentTermOptionRelationDAO;
            _iTblPaymentTermsForBookingBL = iTblPaymentTermsForBookingBL;
            _iTblPaymentTermOptionRelationBL = iTblPaymentTermOptionRelationBL;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iIotCommunication = iIotCommunication;
            _iGateCommunication = iGateCommunication;
            _iTblLoadingSlipDAO = iTblLoadingSlipDAO;
            _iTblAlertDefinitionDAO = iTblAlertDefinitionDAO;
            _iDimensionDAO = iDimensionDAO;
            _iTblEInvoiceApiDAO = iTblEInvoiceApiDAO;
            _iTblEInvoiceApiResponseDAO = iTblEInvoiceApiResponseDAO;
            _iTblEInvoiceSessionApiResponseDAO = iTblEInvoiceSessionApiResponseDAO;
            _iTblOrgLicenseDtlBL = iTblOrgLicenseDtlBL;
            _iTblProductItemBL = iTblProductItemBL;
            _iTblProdGstCodeDtlsBL = iTblProdGstCodeDtlsBL;
            _iTblMaterialDAO = iTblMaterialDAO;
            _iTempLoadingSlipInvoiceDAO = iTempLoadingSlipInvoiceDAO;
            _iTblAddonsFunDtlsDAO = iTblAddonsFunDtlsDAO;
            _iTblLoadingStatusHistoryDAO = iTblLoadingStatusHistoryDAO;
        }
        #region Selection

        public List<TblInvoiceTO> SelectAllTblInvoiceList()
        {
            return _iTblInvoiceDAO.SelectAllTblInvoice();
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
        public List<TblInvoiceTO> SelectAllTblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerID, List<TblUserRoleTO> tblUserRoleTOList, Int32 brandId, Int32 invoiceId, Int32 statusId, String internalOrgId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            List<TblInvoiceTO> list = _iTblInvoiceDAO.SelectAllTblInvoice(frmDt, toDt, isConfirm, cnfId, dealerID, tblUserRoleTO, brandId, invoiceId, statusId, internalOrgId);
            if (isConfirm == 1)
            {
                var nonAuthList = list.Where(n => n.InvoiceStatusE != InvoiceStatusE.AUTHORIZED).ToList();
                SetGateIotDataToInvoiceTOV2(nonAuthList);
            }
            else
            {
                var nonAuthList = list.Where(n => n.LoadingStatusId != (int)TranStatusE.LOADING_DELIVERED).ToList();
                SetGateIotDataToInvoiceTOV2(nonAuthList);
            }
            if (configId == (int)Constants.WeighingDataSourceE.IoT)
            {
                list = list.Where(n => !String.IsNullOrEmpty(n.VehicleNo)).ToList();
            }
            return list;
        }
        //Aniket [22-8-2019] added for IoT
        public void SetGateIotDataToInvoiceTOV2(List<TblInvoiceTO> list)
        {
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == (int)Constants.WeighingDataSourceE.IoT)
            {
                var nonAuthList = list;
                if (nonAuthList != null && nonAuthList.Count > 0)
                {
                    string commSepSlipIds = string.Empty;
                    for (int i = 0; i < nonAuthList.Count; i++)
                    {
                        commSepSlipIds += nonAuthList[i].LoadingSlipId + ",";
                    }
                    commSepSlipIds = commSepSlipIds.TrimEnd(',');
                    //dict of loadingslipId and ModRefId
                    //Dictionary<int, int> loadingSlipModBusRefDCT = DAL.TblLoadingSlipDAO.SelectModbusRefIdWrtLoadingSlipIdDCT(commSepSlipIds);

                    Dictionary<Int32, TblLoadingTO> loadingSlipModBusRefDCT = _iTblLoadingSlipDAO.SelectModbusRefIdByLoadingSlipIdDCT(commSepSlipIds);

                    if (loadingSlipModBusRefDCT != null)
                    {
                        List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();
                        foreach (var item in loadingSlipModBusRefDCT)
                        {
                            tblLoadingTOList.Add(item.Value);
                        }

                        List<TblLoadingTO> distGate = tblLoadingTOList.GroupBy(g => g.GateId).Select(s => s.FirstOrDefault()).ToList();

                        GateIoTResult gateIoTResult = new GateIoTResult();

                        for (int g = 0; g < distGate.Count; g++)
                        {
                            TblLoadingTO tblLoadingTOTemp = distGate[g];
                            TblGateTO tblGateTO = new TblGateTO(tblLoadingTOTemp.GateId, tblLoadingTOTemp.IoTUrl, tblLoadingTOTemp.MachineIP, tblLoadingTOTemp.PortNumber);
                            GateIoTResult temp = _iIotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId("102", tblGateTO);

                            if (temp != null && temp.Data != null && temp.Data.Count > 0)
                            {
                                gateIoTResult.Data.AddRange(temp.Data);
                            }
                        }


                        //GateIoTResult gateIoTResult = IoT.IotCommunication.GetLoadingSlipsByStatusFromIoTByStatusId("102");

                        if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count > 0)
                        {
                            foreach (var item in loadingSlipModBusRefDCT.Keys)
                            {
                                //int modRefId = loadingSlipModBusRefDCT[item];

                                TblLoadingTO tblLoadingTO = loadingSlipModBusRefDCT[item];
                                int modRefId = tblLoadingTO.ModbusRefId;

                                var data = gateIoTResult.Data.Where(w => Convert.ToInt32(w[0]) == modRefId).FirstOrDefault();
                                if (data == null)
                                    continue;
                                //  string vehicleNo = (string)data[(int)IoTConstants.GateIoTColE.VehicleNo];
                                string vehicleNo = _iIotCommunication.GetVehicleNumbers((string)data[(int)IoTConstants.GateIoTColE.VehicleNo], true);//chetan[11-feb-2020] added for formatting old vehicle number.
                                //int transporterOrgId = Convert.ToInt32(data[(int)IoTConstants.GateIoTColE.TransportorId]);
                                //String transporterName = TblOrganizationBL.GetFirmNameByOrgId(transporterOrgId);

                                var invoiceList = nonAuthList.Where(inv => inv.LoadingSlipId == item).ToList();
                                if (invoiceList != null)
                                {
                                    //invoiceList.ForEach(f => { f.VehicleNo = vehicleNo; f.TransportOrgId = transporterOrgId; f.TransporterName = transporterName; });
                                    invoiceList.ForEach(f => { f.VehicleNo = vehicleNo; });
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetGateAndWeightIotData(TblInvoiceTO tblInvoiceTO, int IsExtractionAllowed)
        {
            if (tblInvoiceTO != null)
            {
                List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
                tblInvoiceTOList.Add(tblInvoiceTO);
                SetGateAndWeightIotData(tblInvoiceTOList, IsExtractionAllowed);
            }
        }


        public void SetGateAndWeightIotData(List<TblInvoiceTO> tblInvoiceTOList, int IsExtractionAllowed)
        {
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (IsExtractionAllowed == 0)
                {
                    SetGateIotDataToInvoiceTO(tblInvoiceTOList);
                }
                for (int i = 0; i < tblInvoiceTOList.Count; i++)
                {
                    SetWeightIotDateToInvoiceTO(tblInvoiceTOList[i], IsExtractionAllowed);
                }
            }
        }

        public void SetGateIotDataToInvoiceTO(List<TblInvoiceTO> list)
        {
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                var nonAuthList = list;//.Where(x => x.InvoiceStatusE != InvoiceStatusE.AUTHORIZED).ToList();
                if (nonAuthList != null)
                {
                    string commSepSlipIds = string.Empty;
                    for (int i = 0; i < nonAuthList.Count; i++)
                    {

                        TblInvoiceTO tblInvoiceTOTemp = nonAuthList[i];

                        //Sakeet [2020-03-02] not need as Composition should be against same loading on IOT mode.
                        //List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOByInvoiceId(tblInvoiceTOTemp.IdInvoice);

                        //if(tempLoadingSlipInvoiceTOList != null && tempLoadingSlipInvoiceTOList.Count > 0)
                        //{
                        //    for (int p = 0; p < tempLoadingSlipInvoiceTOList.Count; p++)
                        //    {
                        //        commSepSlipIds += tempLoadingSlipInvoiceTOList[p].LoadingSlipId + ",";
                        //    }

                        //}

                        commSepSlipIds += tblInvoiceTOTemp.LoadingSlipId + ",";

                    }
                    commSepSlipIds = commSepSlipIds.TrimEnd(',');
                    Dictionary<Int32, TblLoadingTO> loadingSlipModBusRefDCT = _iTblLoadingSlipDAO.SelectModbusRefIdByLoadingSlipIdDCT(commSepSlipIds);
                    if (loadingSlipModBusRefDCT != null)
                    {
                        foreach (var item in loadingSlipModBusRefDCT.Keys)
                        {
                            //Added By Vipul
                            TblLoadingTO tblLoadingTO = loadingSlipModBusRefDCT[item];

                            //End
                            //GateIoTResult gateIoTResult = IotCommunication.GetLoadingStatusHistoryDataFromGateIoT(loadingSlipModBusRefDCT[item]);
                            GateIoTResult gateIoTResult = _iGateCommunication.GetLoadingStatusHistoryDataFromGateIoT(tblLoadingTO);

                            if (gateIoTResult != null && gateIoTResult.Data != null && gateIoTResult.Data.Count > 0)
                            {
                                // string vehicleNo = (string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo];
                                string vehicleNo = _iIotCommunication.GetVehicleNumbers((string)gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.VehicleNo], true);//chetan[12-feb-2020] added for formating old vehicle
                                int transporterOrgId = Convert.ToInt32(gateIoTResult.Data[0][(int)IoTConstants.GateIoTColE.TransportorId]);
                                String transporterName = _iTblOrganizationBL.GetFirmNameByOrgId(transporterOrgId);

                                var invoiceList = nonAuthList.Where(inv => inv.LoadingSlipId == item).ToList();
                                if (invoiceList != null)
                                {
                                    invoiceList.ForEach(f => { f.VehicleNo = vehicleNo; f.TransportOrgId = transporterOrgId; f.TransporterName = transporterName; });
                                }
                            }
                        }
                    }
                }
            }
        }
        public ResultMessage SetWeightIotDateToInvoiceTO(TblInvoiceTO tblInvoice, int IsExtractionAllowed)
        {
            ResultMessage resultMessage = new ResultMessage();
            double conversionFactor = 1000;
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (tblInvoice.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                {
                    if (tblInvoice.LoadingSlipId != 0)
                    {
                        TblLoadingSlipTO tblLoadingSlipTO = new TblLoadingSlipTO();

                        List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOByInvoiceId(tblInvoice.IdInvoice);

                        for (int k = 0; k < tempLoadingSlipInvoiceTOList.Count; k++)
                        {
                            TblLoadingSlipTO tblLoadingSlipTO1 = new TblLoadingSlipTO();

                            TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = tempLoadingSlipInvoiceTOList[k];
                            if (tempLoadingSlipInvoiceTO.LoadingSlipId > 0)
                            {
                                if (IsExtractionAllowed == 0)
                                {
                                    //tblLoadingSlipTO1 = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblInvoice.LoadingSlipId);
                                    tblLoadingSlipTO1 = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tempLoadingSlipInvoiceTO.LoadingSlipId);
                                }
                                else
                                {
                                    //tblLoadingSlipTO1 = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetailsForExtract(tblInvoice.LoadingSlipId);
                                    tblLoadingSlipTO1 = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetailsForExtract(tempLoadingSlipInvoiceTO.LoadingSlipId);
                                }

                                if (k == 0)
                                {
                                    tblLoadingSlipTO = tblLoadingSlipTO1;
                                }
                                else
                                {
                                    tblLoadingSlipTO.LoadingSlipExtTOList.AddRange(tblLoadingSlipTO1.LoadingSlipExtTOList);
                                }
                            }
                        }
                        for (int i = 0; i < tblInvoice.InvoiceItemDetailsTOList.Count; i++)
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoice.InvoiceItemDetailsTOList[i];

                            if (tblInvoiceItemDetailsTO.LoadingSlipExtId > 0)
                            {
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipTO.LoadingSlipExtTOList.Where(w => w.IdLoadingSlipExt == tblInvoiceItemDetailsTO.LoadingSlipExtId).FirstOrDefault();
                                if (tblLoadingSlipExtTO != null)
                                {
                                    tblInvoiceItemDetailsTO.Bundles = tblLoadingSlipExtTO.LoadedBundles.ToString();
                                    if (tblLoadingSlipExtTO.LoadedWeight > 0)
                                        tblInvoiceItemDetailsTO.InvoiceQty = tblLoadingSlipExtTO.LoadedWeight / conversionFactor;
                                }
                            }
                        }

                        //Saket [2019-05-27] Added round off and added LoadingSlipExtId > 0 conidtion.
                        tblInvoice.NetWeight = tblInvoice.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId == 0).Sum(s => s.InvoiceQty);
                        tblInvoice.NetWeight = tblInvoice.NetWeight * conversionFactor;
                        tblInvoice.NetWeight = Math.Round(tblInvoice.NetWeight, 2);

                        tblInvoice.GrossWeight = tblInvoice.TareWeight + tblInvoice.NetWeight;
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;

        }

        public TblInvoiceTO SelectTblInvoiceTO(Int32 idInvoice)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceDAO.SelectTblInvoice(idInvoice, conn, tran);
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
        /// Ramdas.W:@22092017:API This method is used to Get List of Invoice By Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <param name="distributorOrgId"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectTblInvoiceByStatus(int statusId, int distributorOrgId, int invoiceId, int isConfirm)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                List<TblInvoiceTO> tblInvoiceTOList = _iTblInvoiceDAO.SelectTblInvoiceByStatus(statusId, distributorOrgId, invoiceId, conn, tran, isConfirm);
                SetGateIotDataToInvoiceTOV2(tblInvoiceTOList);
                return tblInvoiceTOList;
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

        public TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return SelectTblInvoiceTOWithDetails(idInvoice, conn, tran);
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

        public String SelectresponseForPhotoInReport(Int32 idInvoice, Int32 ApiId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceDAO.SelectresponseForPhotoInReport(idInvoice, ApiId);
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



        public TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            TblInvoiceTO invoiceTO = _iTblInvoiceDAO.SelectTblInvoice(idInvoice, conn, tran);
            if (invoiceTO != null)
            {
                invoiceTO.InvoiceItemDetailsTOList = _iTblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceTO.IdInvoice, conn, tran);
                for (int i = 0; i < invoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    invoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList = _iTblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(invoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem, conn, tran);
                }
                invoiceTO.InvoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceTO.IdInvoice, conn, tran);
                //invoiceTO.InvoiceTaxesTOList = BL.TblInvoiceTaxesBL.SelectAllTblInvoiceTaxesList(invoiceTO.IdInvoice, conn, tran);
            }

            if (invoiceTO.IsConfirmed == 0 || invoiceTO.InvoiceStatusE != InvoiceStatusE.AUTHORIZED)
            {
                SetGateAndWeightIotData(invoiceTO, 0);
            }
            //SetGateAndWeightIotData(invoiceTO, 0);
            return invoiceTO;
        }

        //Aniket [25-01-2019]
        public TblEntityRangeTO GenerateInvoiceNumberFromEntityRange(Int32 idInvoice)
        {
            ResultMessage resultMsg = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            TblEntityRangeTO entityRangeTO = null;
            string entityRangeString = "REGULAR_TAX_INVOICE_";
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);
                if (tblConfigParamsTO == null)
                {
                    return null;

                }
                TblConfigParamsTO tblConfigParamsTOForBrand = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.GENERATE_MANUALLY_BRANDWISE_INVOICENO, conn, tran);

                Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                TblInvoiceTO invoiceTO = _iTblInvoiceDAO.SelectTblInvoice(idInvoice, conn, tran);
                //Aniket [05-02-2019]
                if (Convert.ToInt32(tblConfigParamsTOForBrand.ConfigParamVal) == 1)
                {
                    DimBrandTO dimBrandTO = _iDimBrandDAO.SelectDimBrand(invoiceTO.BrandId);
                    entityRangeString += dimBrandTO.IdBrand.ToString();
                    entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityRangeString, invoiceTO.FinYearId, conn, tran);
                }
                else if (invoiceTO.InvFromOrgId == internalOrgId)
                    entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(Constants.ENTITY_RANGE_REGULAR_TAX_INVOICE_BMM, invoiceTO.FinYearId, conn, tran);
                else
                    entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(invoiceTO.InvoiceTypeId, invoiceTO.FinYearId, conn, tran);


                tran.Commit();
                return entityRangeTO;

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
        public List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceDAO.SelectInvoiceTOFromLoadingSlipId(loadingSlipId, conn, tran);
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

        public List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.SelectInvoiceTOFromLoadingSlipId(loadingSlipId, conn, tran);
        }


        public List<TblInvoiceTO> SelectInvoiceTOListFromLoadingSlipId(Int32 loadingSlipId)
        {
            try
            {
                return _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(loadingSlipId);
            }
            catch (Exception ex)
            {
                return null;
            }
            //SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            //SqlTransaction tran = null;
            //try
            //{
            //    conn.Open();
            //    tran = conn.BeginTransaction();
            //    return _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(loadingSlipId, conn, tran);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
            //finally
            //{
            //    conn.Close();
            //}
        }

        public List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipId(loadingSlipId, conn, tran);
        }

        /// <summary>
        /// Saket [2018-02-15] Added
        /// </summary>
        /// <param name="loadingSlipIds"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.SelectInvoiceListFromLoadingSlipIds(loadingSlipIds, conn, tran);
        }

        /// <summary>
        /// Vijaymala[15-09-2017] Added To Get Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public ResultMessage SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            //Reshma Added
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblInvoiceRptTO> TblInvoiceRptTOList = new List<TblInvoiceRptTO>();
                List<TblInvoiceRptTO> TblInvoiceRptTOListByInvoiceItemId = new List<TblInvoiceRptTO>();
                TblInvoiceRptTOList = _iTblInvoiceDAO.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
                if (TblInvoiceRptTOList != null && TblInvoiceRptTOList.Count > 0)
                {
                    ExcelPackage excelPackage = new ExcelPackage();
                    int cellRow = 2;
                    int invoiceId = 0;
                    excelPackage = new ExcelPackage();
                    string minDate = TblInvoiceRptTOList.Min(ele => ele.InvoiceDate).ToString("ddMMyy");
                    string maxDate = TblInvoiceRptTOList.Max(ele => ele.InvoiceDate).ToString("ddMMyy");

                    TblInvoiceRptTOListByInvoiceItemId = TblInvoiceRptTOList.GroupBy(ele => ele.InvoiceItemId).Select(ele => ele.FirstOrDefault()).ToList();

                    foreach (var items in TblInvoiceRptTOListByInvoiceItemId)
                    {
                        List<TblInvoiceRptTO> TblInvoiceRptTOListNew = TblInvoiceRptTOList.Where(ele => ele.InvoiceItemId == items.InvoiceItemId).Select(ele => ele).ToList();

                        foreach (var item in TblInvoiceRptTOListNew)
                        {
                            if (item.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                            {
                                items.IgstTaxAmt = item.TaxAmt;
                                items.IgstPct = item.TaxRatePct;
                            }

                            if (item.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                            {
                                items.CgstTaxAmt = item.TaxAmt;
                                items.CgstPct = item.TaxRatePct;
                            }
                            if (item.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                            {
                                items.SgstTaxAmt = item.TaxAmt;
                                items.SgstPct = item.TaxRatePct;
                            }
                        }
                    }
                    #region Excel Column Prepareration
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                    // Add By Samadhan 9 jan 2023
                    Int32 ShowNewColNCRptMetaroll = 0;

                    TblConfigParamsTO tblConfigParamsTONewColNCRptshow = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.cp_is_show_NC_report_col_for_metaroll);
                    if (tblConfigParamsTONewColNCRptshow != null)
                    {
                        ShowNewColNCRptMetaroll = Convert.ToInt32(tblConfigParamsTONewColNCRptshow.ConfigParamVal);
                    }                   

                  
                    if (ShowNewColNCRptMetaroll == 1)
                    {
                        excelWorksheet.Cells[1, 1].Value = "Vehicle No";
                        excelWorksheet.Cells[1, 2].Value = "Transaction Date";                  
                        excelWorksheet.Cells[1, 3].Value = "Party Name";
                        excelWorksheet.Cells[1, 4].Value = "Distributor";
                        excelWorksheet.Cells[1, 5].Value = "Booking Rate";                    
                        excelWorksheet.Cells[1, 6].Value = "Size";
                        excelWorksheet.Cells[1, 7].Value = "Size Bundle";
                        excelWorksheet.Cells[1, 8].Value = "Gross Weight";
                        excelWorksheet.Cells[1, 9].Value = "Tare Weight";
                        excelWorksheet.Cells[1, 10].Value = "Net Weight";
                        excelWorksheet.Cells[1, 11].Value = "Size Rate";
                        excelWorksheet.Cells[1, 12].Value = "CD (%)"; 
                        excelWorksheet.Cells[1, 13].Value = "SIZE WEIGHT";
                        excelWorksheet.Cells[1, 14].Value = "BASIC SALE VALUE";                      
                        excelWorksheet.Cells[1, 15].Value = "GROSS VALUE";
                        excelWorksheet.Cells[1, 16].Value = "CD VALUE";
                        excelWorksheet.Cells[1, 17].Value = "PARTY RECEIVABLE";
                        excelWorksheet.Cells[1, 18].Value = "Narration";
                        excelWorksheet.Cells[1, 19].Value = "DealerIDOrganization";
                        

                        excelWorksheet.Cells[1, 1, 1, 19].Style.Font.Bold = true;
                      
                        for (int i = 0; i < TblInvoiceRptTOListByInvoiceItemId.Count; i++)
                        {
                            if (invoiceId != 0 && invoiceId != TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice)
                            {
                                List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                                excelWorksheet.Cells[cellRow, 2].Value = "Total";
                                excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListnew.Select(ele => ele.CdStructure);                              
                                excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListnew.Sum(ele => ele.InvoiceQty);
                                excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.TaxableAmt), 2);
                              
                                excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);
                                excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CdAmt), 2);
                                excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);

                                excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListnew.Select(ele => ele.GrossWeight / 1000);
                                excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListnew.Select(ele => ele.TareWeight / 1000);
                                excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListnew.Select(ele => ele.NetWeight / 1000);

                                excelWorksheet.Cells[cellRow, 1, cellRow, 19].Style.Font.Bold = true;                               
                                cellRow++;

                            }

                            excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].VehicleNo;
                            excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                            excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                            excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;
                            excelWorksheet.Cells[cellRow, 5].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].BookingRate, 2);
                            excelWorksheet.Cells[cellRow, 6].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProdItemDesc;
                            excelWorksheet.Cells[cellRow, 7].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                            //excelWorksheet.Cells[cellRow, 8].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrossWeight, 2);
                           // excelWorksheet.Cells[cellRow, 9].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].TareWeight, 2);
                            //excelWorksheet.Cells[cellRow, 10].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].NetWeight, 2);
                            excelWorksheet.Cells[cellRow, 11].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].Rate, 2);
                            excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;                           
                            excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                            excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt, 2);
                            excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal, 2);
                            excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CdAmt, 2);
                            excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal, 2);                           
                            excelWorksheet.Cells[cellRow, 18].Value = TblInvoiceRptTOListByInvoiceItemId[i].DealerData;
                            excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListByInvoiceItemId[i].DealerIDOrganization;
                          
                            invoiceId = TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice;
                            cellRow++;

                            // For last record.
                            if (i == (TblInvoiceRptTOListByInvoiceItemId.Count - 1))
                            {
                                List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                                excelWorksheet.Cells[cellRow, 2].Value = "Total";
                                excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListnew.Select(ele => ele.CdStructure);                               
                                excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListnew.Sum(ele => ele.InvoiceQty);
                                excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.TaxableAmt), 2);
                                excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);
                                excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CdAmt), 2);
                                excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);

                                excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListnew.Select(ele => ele.GrossWeight / 1000);
                                excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListnew.Select(ele => ele.TareWeight / 1000);
                                excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListnew.Select(ele => ele.NetWeight / 1000);

                                excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;
                                cellRow++;

                                // For final total.
                                excelWorksheet.Cells[cellRow, 9].Value = "Grand Total";
                                excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.InvoiceQty);
                                excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.TaxableAmt), 2);

                                excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);
                                excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CdAmt), 2);
                                excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);

                                excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;
                              

                                using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 19])
                                {
                                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium); 
                                    range.Style.Font.Name = "Times New Roman";
                                    range.Style.Font.Size = 10;
                                }
                            }
                        }


                    }
                    else
                    { 

                    excelWorksheet.Cells[1, 1].Value = "Vehicle No";
                    excelWorksheet.Cells[1, 2].Value = "Invoice No";
                    excelWorksheet.Cells[1, 3].Value = "Transaction Date";
                    excelWorksheet.Cells[1, 4].Value = "Tally Ref Id";
                    excelWorksheet.Cells[1, 5].Value = "Party Name";
                    excelWorksheet.Cells[1, 6].Value = "Distributor";
                    excelWorksheet.Cells[1, 7].Value = "Booking Rate";
                    excelWorksheet.Cells[1, 8].Value = "Product Code";
                    excelWorksheet.Cells[1, 9].Value = "Size";
                    excelWorksheet.Cells[1, 10].Value = "Size Bundle";
                    excelWorksheet.Cells[1, 11].Value = "Gross Weight";
                    excelWorksheet.Cells[1, 12].Value = "Tare Weight";
                    excelWorksheet.Cells[1, 13].Value = "Net Weight";
                    excelWorksheet.Cells[1, 14].Value = "Size Rate";
                    excelWorksheet.Cells[1, 15].Value = "CD (%)";
                    excelWorksheet.Cells[1, 16].Value = "CGST (%)";
                    excelWorksheet.Cells[1, 17].Value = "SGST(%)";
                    excelWorksheet.Cells[1, 18].Value = "IGST(%)";
                    excelWorksheet.Cells[1, 19].Value = "SIZE WEIGHT";
                    excelWorksheet.Cells[1, 20].Value = "BASIC SALE VALUE";
                    excelWorksheet.Cells[1, 21].Value = "CGST VALUE";
                    excelWorksheet.Cells[1, 22].Value = "SGST VALUE";
                    excelWorksheet.Cells[1, 23].Value = "IGST VALUE";
                    excelWorksheet.Cells[1, 24].Value = "GROSS VALUE";
                    excelWorksheet.Cells[1, 25].Value = "CD VALUE";
                    excelWorksheet.Cells[1, 26].Value = "PARTY RECEIVABLE";
                    excelWorksheet.Cells[1, 27].Value = "Narration";

                        excelWorksheet.Cells[1, 1, 1, 24].Style.Font.Bold = true;
                        for (int i = 0; i < TblInvoiceRptTOListByInvoiceItemId.Count; i++)
                        {
                            if (invoiceId != 0 && invoiceId != TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice)
                            {
                                List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                                excelWorksheet.Cells[cellRow, 2].Value = "Total";
                                excelWorksheet.Cells[cellRow, 25].Value = TblInvoiceRptTOListnew.Select(ele => ele.CdStructure);
                                excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListnew.Select(ele => ele.CgstPct);
                                excelWorksheet.Cells[cellRow, 17].Value = TblInvoiceRptTOListnew.Select(ele => ele.SgstPct);
                                excelWorksheet.Cells[cellRow, 18].Value = TblInvoiceRptTOListnew.Select(ele => ele.IgstPct);
                                excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListnew.Sum(ele => ele.InvoiceQty);
                                excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.TaxableAmt), 2);

                                excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 22].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.SgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 23].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.IgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 24].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);
                                excelWorksheet.Cells[cellRow, 25].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CdAmt), 2);
                                excelWorksheet.Cells[cellRow, 26].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);

                                excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListnew.Select(ele => ele.GrossWeight / 1000);
                                excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListnew.Select(ele => ele.TareWeight / 1000);
                                excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListnew.Select(ele => ele.NetWeight / 1000);

                                excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;
                                cellRow++;

                            }

                            excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].VehicleNo;
                            excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNoWrtDate;
                            excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                            excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                            excelWorksheet.Cells[cellRow, 6].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;

                            excelWorksheet.Cells[cellRow, 7].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].BookingRate, 2);
                            excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProdItemDesc;
                            excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                            excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].Rate, 2);
                            excelWorksheet.Cells[cellRow, 15].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;

                            excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListByInvoiceItemId[i].CgstPct;
                            excelWorksheet.Cells[cellRow, 17].Value = TblInvoiceRptTOListByInvoiceItemId[i].SgstPct;
                            excelWorksheet.Cells[cellRow, 18].Value = TblInvoiceRptTOListByInvoiceItemId[i].IgstPct;
                            excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                            excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt, 2);

                            excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CgstTaxAmt, 2);
                            excelWorksheet.Cells[cellRow, 22].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].SgstTaxAmt, 2);
                            excelWorksheet.Cells[cellRow, 23].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].IgstTaxAmt, 2);
                            excelWorksheet.Cells[cellRow, 24].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal, 2);
                            excelWorksheet.Cells[cellRow, 25].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].CdAmt, 2);
                            excelWorksheet.Cells[cellRow, 26].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal, 2);
                            excelWorksheet.Cells[cellRow, 27].Value = TblInvoiceRptTOListByInvoiceItemId[i].Narration;

                            invoiceId = TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice;
                            cellRow++;

                            // For last record.
                            if (i == (TblInvoiceRptTOListByInvoiceItemId.Count - 1))
                            {
                                List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                                excelWorksheet.Cells[cellRow, 2].Value = "Total";
                                excelWorksheet.Cells[cellRow, 15].Value = TblInvoiceRptTOListnew.Select(ele => ele.CdStructure);
                                excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListnew.Select(ele => ele.CgstPct);
                                excelWorksheet.Cells[cellRow, 17].Value = TblInvoiceRptTOListnew.Select(ele => ele.SgstPct);
                                excelWorksheet.Cells[cellRow, 18].Value = TblInvoiceRptTOListnew.Select(ele => ele.IgstPct);
                                excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListnew.Sum(ele => ele.InvoiceQty);
                                excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.TaxableAmt), 2);

                                excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 22].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.SgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 23].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.IgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 24].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);
                                excelWorksheet.Cells[cellRow, 25].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.CdAmt), 2);
                                excelWorksheet.Cells[cellRow, 26].Value = Math.Round(TblInvoiceRptTOListnew.Sum(ele => ele.GrandTotal), 2);

                                excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListnew.Select(ele => ele.GrossWeight / 1000);
                                excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListnew.Select(ele => ele.TareWeight / 1000);
                                excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListnew.Select(ele => ele.NetWeight / 1000);

                                excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;
                                cellRow++;

                                // For final total.
                                excelWorksheet.Cells[cellRow, 9].Value = "Grand Total";
                                excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.InvoiceQty);
                                excelWorksheet.Cells[cellRow, 20].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.TaxableAmt), 2);

                                excelWorksheet.Cells[cellRow, 21].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 22].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.SgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 23].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.IgstTaxAmt), 2);
                                excelWorksheet.Cells[cellRow, 24].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);
                                excelWorksheet.Cells[cellRow, 25].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CdAmt), 2);
                                excelWorksheet.Cells[cellRow, 26].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);

                                excelWorksheet.Cells[cellRow, 1, cellRow, 21].Style.Font.Bold = true;

                                using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 21])
                                {
                                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                                    range.Style.Font.Name = "Times New Roman";
                                    range.Style.Font.Size = 10;
                                }
                            }
                        }


                    }

                  
                    #endregion
                   
                    excelWorksheet.Protection.IsProtected = true;
                    excelPackage.Workbook.Protection.LockStructure = true;
                    #region Upload File to Azure

                    // Create azure storage  account connection.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Retrieve reference to a target container.
                    CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                    String fileName = Constants.ExcelFileName + _iCommon.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-" + minDate + "-" + maxDate + "-R" + ".xlsx";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    var fileStream = excelPackage.GetAsByteArray();

                    Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                    excelPackage.Dispose();
                    #endregion
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                resultMessage.DefaultBehaviour();
                //return _iTblInvoiceDAO.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempInvoiceExcel");
            }
            return resultMessage;
        }

        public ResultMessage GetRptInvoiceNCListForVasudha(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            //Reshma Added
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblInvoiceRptTO> TblInvoiceRptTOList = new List<TblInvoiceRptTO>();
                List<TblInvoiceRptTO> TblInvoiceRptTOListByInvoiceItemId = new List<TblInvoiceRptTO>();
                // return _iTblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, fromOrgId);
                TblInvoiceRptTOList = _iTblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, fromOrgId);
                if (TblInvoiceRptTOList != null && TblInvoiceRptTOList.Count > 0)
                {
                    ExcelPackage excelPackage = new ExcelPackage();
                    int cellRow = 2;
                    int invoiceId = 0;
                    excelPackage = new ExcelPackage();
                    string minDate = TblInvoiceRptTOList.Min(ele => ele.InvoiceDate).ToString("ddMMyy");
                    string maxDate = TblInvoiceRptTOList.Max(ele => ele.InvoiceDate).ToString("ddMMyy");

                    List<TblInvoiceRptTO>  TblInvoiceRptTOListByInvoiceItemDistictId = TblInvoiceRptTOList.GroupBy(ele => ele.IdInvoice).Select(ele => ele.FirstOrDefault()).ToList();

                    foreach (var items in TblInvoiceRptTOListByInvoiceItemDistictId)
                    {
                        List<TblInvoiceRptTO> TblInvoiceRptTOListNew = TblInvoiceRptTOList.Where(ele => ele.InvoiceItemId == items.InvoiceItemId).Select(ele => ele).ToList();

                        foreach (var item in TblInvoiceRptTOListNew)
                        {
                            if (item.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                            {
                                items.IgstTaxAmt = item.TaxAmt;
                                items.IgstPct = item.TaxRatePct;
                            }

                            if (item.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                            {
                                items.CgstTaxAmt = item.TaxAmt;
                                items.CgstPct = item.TaxRatePct;
                            }
                            if (item.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                            {
                                items.SgstTaxAmt = item.TaxAmt;
                                items.SgstPct = item.TaxRatePct;
                            }
                        }
                    }
                    #region Excel Column Prepareration
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                    // Add By Samadhan 9 jan 2023
                    Int32 ShowNewColNCRptMetaroll = 0;

                    // if (ShowNewColNCRptMetaroll == 1)
                    if (TblInvoiceRptTOListByInvoiceItemDistictId != null && TblInvoiceRptTOListByInvoiceItemDistictId.Count > 0)
                    {
                        #region Excel column
                        excelWorksheet.Cells[1, 1].Value = "Invoice No.";
                        excelWorksheet.Cells[1, 2].Value = "Transaction Date";
                        excelWorksheet.Cells[1, 3].Value = "Party Name";
                        excelWorksheet.Cells[1, 4].Value = "Buyer Address 1";
                        excelWorksheet.Cells[1, 5].Value = "Buyer Address 2";
                        excelWorksheet.Cells[1, 6].Value = "Buyer Address 3";
                        excelWorksheet.Cells[1, 7].Value = "Buyer Address 4";
                        excelWorksheet.Cells[1, 8].Value = "Buyers State";
                        excelWorksheet.Cells[1, 9].Value = "Country";
                        excelWorksheet.Cells[1, 10].Value = "PinCode";
                        excelWorksheet.Cells[1, 11].Value = "Buyers GSTIN / UIN";
                        excelWorksheet.Cells[1, 12].Value = "Distributor";
                        excelWorksheet.Cells[1, 13].Value = "Consignee Name";
                        excelWorksheet.Cells[1, 14].Value = "Consignee Address 1";
                        excelWorksheet.Cells[1, 15].Value = "Consignee Address 2";
                        excelWorksheet.Cells[1, 16].Value = "Consignee Address 3";
                        excelWorksheet.Cells[1, 17].Value = "Consignee Address 4";
                        excelWorksheet.Cells[1, 18].Value = "State";
                        excelWorksheet.Cells[1, 19].Value = "Country";
                        excelWorksheet.Cells[1, 20].Value = "PinCode";
                        excelWorksheet.Cells[1, 21].Value = "Buyers GSTIN / UIN";
                        excelWorksheet.Cells[1, 22].Value = "Delivery Location to";
                        excelWorksheet.Cells[1, 23].Value = "Item Description";
                        excelWorksheet.Cells[1, 24].Value = "GST %";
                        excelWorksheet.Cells[1, 25].Value = "HSN";
                        excelWorksheet.Cells[1, 26].Value = "Size";
                        excelWorksheet.Cells[1, 27].Value = "Size Bundle";
                        excelWorksheet.Cells[1, 28].Value = "SIZE WEIGHT";
                        excelWorksheet.Cells[1, 29].Value = "Booking Rate";
                        excelWorksheet.Cells[1, 30].Value = "Size Rate";
                        excelWorksheet.Cells[1, 31].Value = "CD";
                        excelWorksheet.Cells[1, 32].Value = "BASIC VALUE";
                        excelWorksheet.Cells[1, 33].Value = "Taxable Amt.";
                        excelWorksheet.Cells[1, 34].Value = "Sales Ledger";
                        excelWorksheet.Cells[1, 35].Value = "Adv. Freight Amt";
                        excelWorksheet.Cells[1, 36].Value = "Insurance GL";
                        excelWorksheet.Cells[1, 37].Value = "CGST OUTPUT";
                        excelWorksheet.Cells[1, 38].Value = "SGST OUTPUT";
                        excelWorksheet.Cells[1, 39].Value = "IGST OUTPUT";
                        excelWorksheet.Cells[1, 40].Value = "TCS GL";
                        excelWorksheet.Cells[1, 41].Value = "TCS 1% F.Y. 2017-18 (PAYABLE)";
                        excelWorksheet.Cells[1, 42].Value = "Round Off";
                        excelWorksheet.Cells[1, 43].Value = "PARTY RECEIVABLE";
                        excelWorksheet.Cells[1, 44].Value = "Acknowledgment No.";
                        excelWorksheet.Cells[1, 45].Value = "Ack Date";
                        excelWorksheet.Cells[1, 46].Value = "E-way bill No";
                        excelWorksheet.Cells[1, 47].Value = "Eway bill Date";
                        excelWorksheet.Cells[1, 48].Value = "Dispatched Through/ Transporter";
                        excelWorksheet.Cells[1, 49].Value = "Motor Vehicle No";
                        excelWorksheet.Cells[1, 50].Value = "Bill of Lading / LR-RR No";
                        excelWorksheet.Cells[1, 51].Value = "LR Date";
                        excelWorksheet.Cells[1, 52].Value = "Customer Phone NO";
                        excelWorksheet.Cells[1, 53].Value = "Driver Phone No";
                        excelWorksheet.Cells[1, 54].Value = "Narration";

                        excelWorksheet.Cells[1, 1, 1, 19].Style.Font.Bold = true;
                        #endregion
                        for (int k = 0; k < TblInvoiceRptTOListByInvoiceItemDistictId.Count; k++)
                        {
                            invoiceId = 0;
                            TblInvoiceRptTOListByInvoiceItemId = TblInvoiceRptTOList.
                                Where(w => w.IdInvoice == TblInvoiceRptTOListByInvoiceItemDistictId[k].IdInvoice).ToList();
                            for (int i = 0; i < TblInvoiceRptTOListByInvoiceItemId.Count; i++)
                            {
                                if (invoiceId != 0)// && invoiceId != TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice)
                                {

                                    excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNo;
                                    excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                                    excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                                    excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerAddress;
                                    excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerDistict;
                                    excelWorksheet.Cells[cellRow, 6].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerTaluka;
                                    excelWorksheet.Cells[cellRow, 7].Value = "";
                                    excelWorksheet.Cells[cellRow, 8].Value = "";
                                    excelWorksheet.Cells[cellRow, 9].Value = "";
                                    excelWorksheet.Cells[cellRow, 10].Value = "";
                                    excelWorksheet.Cells[cellRow, 11].Value = "";
                                    excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;
                                    excelWorksheet.Cells[cellRow, 13].Value = "";
                                    excelWorksheet.Cells[cellRow, 14].Value = "";
                                    excelWorksheet.Cells[cellRow, 15].Value = "";
                                    excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeTaluka;
                                    excelWorksheet.Cells[cellRow, 17].Value = "";
                                    excelWorksheet.Cells[cellRow, 18].Value = "";
                                    excelWorksheet.Cells[cellRow, 19].Value = "";
                                    excelWorksheet.Cells[cellRow, 20].Value = "";
                                    excelWorksheet.Cells[cellRow, 21].Value = "";

                                    excelWorksheet.Cells[cellRow, 22].Value = "";
                                    excelWorksheet.Cells[cellRow, 23].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProDesc;
                                    excelWorksheet.Cells[cellRow, 24].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxPCT;
                                    excelWorksheet.Cells[cellRow, 25].Value = TblInvoiceRptTOListByInvoiceItemId[i].GstCodeNo;
                                    excelWorksheet.Cells[cellRow, 26].Value = TblInvoiceRptTOListByInvoiceItemId[i].MaterialName;
                                    excelWorksheet.Cells[cellRow, 27].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                                    excelWorksheet.Cells[cellRow, 28].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                                    excelWorksheet.Cells[cellRow, 29].Value = TblInvoiceRptTOListByInvoiceItemId[i].BookingRate;
                                    excelWorksheet.Cells[cellRow, 30].Value = TblInvoiceRptTOListByInvoiceItemId[i].Rate;
                                    excelWorksheet.Cells[cellRow, 31].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;
                                    excelWorksheet.Cells[cellRow, 32].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt;

                                    excelWorksheet.Cells[cellRow, 33].Value = "";
                                    excelWorksheet.Cells[cellRow, 34].Value = TblInvoiceRptTOListByInvoiceItemId[i].SalesLedger;
                                    excelWorksheet.Cells[cellRow, 35].Value = "";
                                    excelWorksheet.Cells[cellRow, 36].Value = "";
                                    excelWorksheet.Cells[cellRow, 37].Value = "";
                                    excelWorksheet.Cells[cellRow, 38].Value = "";
                                    excelWorksheet.Cells[cellRow, 39].Value = "";
                                    excelWorksheet.Cells[cellRow, 40].Value = "";
                                    excelWorksheet.Cells[cellRow, 41].Value = "";
                                    excelWorksheet.Cells[cellRow, 42].Value = TblInvoiceRptTOListByInvoiceItemId[i].roundOffAmt;
                                    excelWorksheet.Cells[cellRow, 43].Value = "";
                                    excelWorksheet.Cells[cellRow, 44].Value = "";

                                    excelWorksheet.Cells[cellRow, 45].Value = "";
                                    excelWorksheet.Cells[cellRow, 46].Value = "";
                                    excelWorksheet.Cells[cellRow, 47].Value = "";
                                    excelWorksheet.Cells[cellRow, 48].Value = "";
                                    excelWorksheet.Cells[cellRow, 49].Value = "";
                                    excelWorksheet.Cells[cellRow, 50].Value = "";
                                    excelWorksheet.Cells[cellRow, 51].Value = "";
                                    excelWorksheet.Cells[cellRow, 52].Value = "";
                                    excelWorksheet.Cells[cellRow, 53].Value = "";
                                    excelWorksheet.Cells[cellRow, 54].Value = "";

                                  //  excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;
                                    cellRow++;

                                }
                                else
                                {
                                    excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNo;
                                    excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                                    excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                                    excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerAddress;
                                    excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerDistict;
                                    excelWorksheet.Cells[cellRow, 6].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerTaluka;
                                    excelWorksheet.Cells[cellRow, 7].Value = "";
                                    excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerState;
                                    excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyercountryName;
                                    excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerPincode;
                                    excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerGstNo;
                                    excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;
                                    excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId[i].Consignee;
                                    excelWorksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeAddress;
                                    excelWorksheet.Cells[cellRow, 15].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeDistict;
                                    excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeTaluka;
                                    excelWorksheet.Cells[cellRow, 17].Value = "";
                                    excelWorksheet.Cells[cellRow, 18].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeState;
                                    excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListByInvoiceItemId[i].consigneecountryName;
                                    excelWorksheet.Cells[cellRow, 20].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneePinCode;
                                    excelWorksheet.Cells[cellRow, 21].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeGstNo;

                                    excelWorksheet.Cells[cellRow, 22].Value = TblInvoiceRptTOListByInvoiceItemId[i].DeliveryLocation;
                                    excelWorksheet.Cells[cellRow, 23].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProDesc;
                                    excelWorksheet.Cells[cellRow, 24].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxPCT;
                                    excelWorksheet.Cells[cellRow, 25].Value = TblInvoiceRptTOListByInvoiceItemId[i].GstCodeNo;
                                    excelWorksheet.Cells[cellRow, 26].Value = TblInvoiceRptTOListByInvoiceItemId[i].MaterialName;
                                    excelWorksheet.Cells[cellRow, 27].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                                    excelWorksheet.Cells[cellRow, 28].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                                    excelWorksheet.Cells[cellRow, 29].Value = TblInvoiceRptTOListByInvoiceItemId[i].BookingRate;
                                    excelWorksheet.Cells[cellRow, 30].Value = TblInvoiceRptTOListByInvoiceItemId[i].Rate;
                                    excelWorksheet.Cells[cellRow, 31].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;
                                    excelWorksheet.Cells[cellRow, 32].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt;

                                    excelWorksheet.Cells[cellRow, 33].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceTaxableAmt;
                                    excelWorksheet.Cells[cellRow, 34].Value = TblInvoiceRptTOListByInvoiceItemId[i].SalesLedger;
                                    excelWorksheet.Cells[cellRow, 35].Value = TblInvoiceRptTOListByInvoiceItemId[i].Freight_GL;
                                    excelWorksheet.Cells[cellRow, 36].Value = TblInvoiceRptTOListByInvoiceItemId[i].FreightAmt;
                                    excelWorksheet.Cells[cellRow, 37].Value = TblInvoiceRptTOListByInvoiceItemId[i].CgstTaxAmt;
                                    excelWorksheet.Cells[cellRow, 38].Value = TblInvoiceRptTOListByInvoiceItemId[i].SgstTaxAmt;
                                    excelWorksheet.Cells[cellRow, 39].Value = TblInvoiceRptTOListByInvoiceItemId[i].IgstTaxAmt;
                                    excelWorksheet.Cells[cellRow, 40].Value = TblInvoiceRptTOListByInvoiceItemId[i].TCS_GL;
                                    excelWorksheet.Cells[cellRow, 41].Value = TblInvoiceRptTOListByInvoiceItemId[i].TcsAmt;
                                    excelWorksheet.Cells[cellRow, 42].Value = TblInvoiceRptTOListByInvoiceItemId[i].roundOffAmt;
                                    excelWorksheet.Cells[cellRow, 43].Value = TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal;
                                    excelWorksheet.Cells[cellRow, 44].Value = TblInvoiceRptTOListByInvoiceItemId[i].AckNo;

                                    excelWorksheet.Cells[cellRow, 45].Value = TblInvoiceRptTOListByInvoiceItemId[i].AckDate;
                                    excelWorksheet.Cells[cellRow, 46].Value = TblInvoiceRptTOListByInvoiceItemId[i].EwbNo;
                                    excelWorksheet.Cells[cellRow, 47].Value = TblInvoiceRptTOListByInvoiceItemId[i].EwbDate;
                                    excelWorksheet.Cells[cellRow, 48].Value = TblInvoiceRptTOListByInvoiceItemId[i].TransporterName;
                                    excelWorksheet.Cells[cellRow, 49].Value = TblInvoiceRptTOListByInvoiceItemId[i].VehicleNo;
                                    excelWorksheet.Cells[cellRow, 50].Value = TblInvoiceRptTOListByInvoiceItemId[i].LrNumber;
                                    excelWorksheet.Cells[cellRow, 51].Value = "";
                                    excelWorksheet.Cells[cellRow, 52].Value = TblInvoiceRptTOListByInvoiceItemId[i].DealerMobNo;
                                    excelWorksheet.Cells[cellRow, 53].Value = TblInvoiceRptTOListByInvoiceItemId[i].ContactNo;
                                    excelWorksheet.Cells[cellRow, 54].Value = TblInvoiceRptTOListByInvoiceItemId[i].NarrationConcat;


                                    invoiceId = TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice;
                                    excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;
                                    cellRow++;
                                   
                                }
                            }
                            // For last record.
                            //if (i == (TblInvoiceRptTOListByInvoiceItemId.Count - 1))

                        }

                        // List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                        excelWorksheet.Cells[cellRow, 1].Value = "Grand Total";
                        excelWorksheet.Cells[cellRow, 28].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.InvoiceQty),2);
                        excelWorksheet.Cells[cellRow, 32].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.TaxableAmt),2);
                        excelWorksheet.Cells[cellRow, 33].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.InvoiceTaxableAmt), 2);
                        excelWorksheet.Cells[cellRow, 37].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.CgstTaxAmt), 2);
                        excelWorksheet.Cells[cellRow, 38].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.SgstTaxAmt), 2);
                        excelWorksheet.Cells[cellRow, 39].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.IgstTaxAmt), 2);
                        excelWorksheet.Cells[cellRow, 43].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.GrandTotal), 2);

                        //excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListnew.Select(ele => ele.GrossWeight / 1000);
                        //excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListnew.Select(ele => ele.TareWeight / 1000);
                        //excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListnew.Select(ele => ele.NetWeight / 1000);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;
                        //cellRow++;

                        // For final total.
                        //excelWorksheet.Cells[cellRow, 9].Value = "Grand Total";
                        //excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.InvoiceQty);
                        //excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.TaxableAmt), 2);

                        //excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);
                        //excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CdAmt), 2);
                        //excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;


                        using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 54])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                            range.Style.Font.Name = "Times New Roman";
                            range.Style.Font.Size = 10;
                        }

                    }
                    

                    #endregion

                    excelWorksheet.Protection.IsProtected = true;
                    excelPackage.Workbook.Protection.LockStructure = true;
                    #region Upload File to Azure

                    // Create azure storage  account connection.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Retrieve reference to a target container.
                    CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                    String fileName = Constants.ExcelFileName + _iCommon.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-" + minDate + "-" + maxDate + "-R" + ".xlsx";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    var fileStream = excelPackage.GetAsByteArray();

                    Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                    excelPackage.Dispose();
                    #endregion
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                resultMessage.DefaultBehaviour();
                //return _iTblInvoiceDAO.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempInvoiceExcel");
            }
            return resultMessage;
        }

        public ResultMessage GetRptInvoiceNCListForGajkesri(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            //Reshma Added
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblInvoiceRptTO> TblInvoiceRptTOList = new List<TblInvoiceRptTO>();
                List<TblInvoiceRptTO> TblInvoiceRptTOListByInvoiceItemId = new List<TblInvoiceRptTO>();
                // return _iTblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, fromOrgId);
                TblInvoiceRptTOList = _iTblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, fromOrgId);
                if (TblInvoiceRptTOList != null && TblInvoiceRptTOList.Count > 0)
                {
                    ExcelPackage excelPackage = new ExcelPackage();
                    int cellRow = 2;
                    int invoiceId = 0;
                    excelPackage = new ExcelPackage();
                    string minDate = TblInvoiceRptTOList.Min(ele => ele.InvoiceDate).ToString("ddMMyy");
                    string maxDate = TblInvoiceRptTOList.Max(ele => ele.InvoiceDate).ToString("ddMMyy");

                    List<TblInvoiceRptTO> TblInvoiceRptTOListByInvoiceItemDistictId = TblInvoiceRptTOList.GroupBy(ele => ele.IdInvoice).Select(ele => ele.FirstOrDefault()).ToList();

                    foreach (var items in TblInvoiceRptTOListByInvoiceItemDistictId)
                    {
                        List<TblInvoiceRptTO> TblInvoiceRptTOListNew = TblInvoiceRptTOList.Where(ele => ele.InvoiceItemId == items.InvoiceItemId).Select(ele => ele).ToList();

                        foreach (var item in TblInvoiceRptTOListNew)
                        {
                            if (item.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                            {
                                items.IgstTaxAmt = item.TaxAmt;
                                items.IgstPct = item.TaxRatePct;
                            }

                            if (item.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                            {
                                items.CgstTaxAmt = item.TaxAmt;
                                items.CgstPct = item.TaxRatePct;
                            }
                            if (item.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                            {
                                items.SgstTaxAmt = item.TaxAmt;
                                items.SgstPct = item.TaxRatePct;
                            }
                        }
                    }
                    #region Excel Column Prepareration
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                    // Add By Samadhan 9 jan 2023
                    Int32 ShowNewColNCRptMetaroll = 0;

                    // if (ShowNewColNCRptMetaroll == 1)
                    if (TblInvoiceRptTOListByInvoiceItemDistictId != null && TblInvoiceRptTOListByInvoiceItemDistictId.Count > 0)
                    {
                        #region Excel column
                        excelWorksheet.Cells[1, 1].Value = "Invoice No.";
                        excelWorksheet.Cells[1, 2].Value = "Transaction Date";
                        excelWorksheet.Cells[1, 3].Value = "Party Name";
                        excelWorksheet.Cells[1, 4].Value = "Buyer Address 1";
                        excelWorksheet.Cells[1, 5].Value = "Buyer Address 2";
                        excelWorksheet.Cells[1, 6].Value = "Buyer Address 3";
                        excelWorksheet.Cells[1, 7].Value = "Buyer Address 4";
                        excelWorksheet.Cells[1, 8].Value = "Buyers State";
                        excelWorksheet.Cells[1, 9].Value = "Country";
                        excelWorksheet.Cells[1, 10].Value = "PinCode";
                        excelWorksheet.Cells[1, 11].Value = "Buyers GSTIN / UIN";
                        excelWorksheet.Cells[1, 12].Value = "Distributor";
                        excelWorksheet.Cells[1, 13].Value = "Consignee Name";
                        excelWorksheet.Cells[1, 14].Value = "Consignee Address 1";
                        excelWorksheet.Cells[1, 15].Value = "Consignee Address 2";
                        excelWorksheet.Cells[1, 16].Value = "Consignee Address 3";
                        excelWorksheet.Cells[1, 17].Value = "Consignee Address 4";
                        excelWorksheet.Cells[1, 18].Value = "State";
                        excelWorksheet.Cells[1, 19].Value = "Country";
                        excelWorksheet.Cells[1, 20].Value = "PinCode";
                        excelWorksheet.Cells[1, 21].Value = "Buyers GSTIN / UIN";
                        excelWorksheet.Cells[1, 22].Value = "Delivery Location to";
                        excelWorksheet.Cells[1, 23].Value = "Item Description";
                        excelWorksheet.Cells[1, 24].Value = "GST %";
                        excelWorksheet.Cells[1, 25].Value = "HSN";
                        excelWorksheet.Cells[1, 26].Value = "Size";
                        excelWorksheet.Cells[1, 27].Value = "Size Bundle";
                        excelWorksheet.Cells[1, 28].Value = "SIZE WEIGHT";
                        excelWorksheet.Cells[1, 29].Value = "Booking Rate";
                        excelWorksheet.Cells[1, 30].Value = "Size Rate";
                        excelWorksheet.Cells[1, 31].Value = "CD";
                        excelWorksheet.Cells[1, 32].Value = "BASIC VALUE";
                        excelWorksheet.Cells[1, 33].Value = "Taxable Amt.";
                        excelWorksheet.Cells[1, 34].Value = "Sales Ledger";
                        excelWorksheet.Cells[1, 35].Value = "Adv. Freight Amt";
                        excelWorksheet.Cells[1, 36].Value = "Insurance GL";
                        excelWorksheet.Cells[1, 37].Value = "CGST OUTPUT";
                        excelWorksheet.Cells[1, 38].Value = "SGST OUTPUT";
                        excelWorksheet.Cells[1, 39].Value = "IGST OUTPUT";
                        excelWorksheet.Cells[1, 40].Value = "TCS GL";
                        excelWorksheet.Cells[1, 41].Value = "TCS 1% F.Y. 2017-18 (PAYABLE)";
                        excelWorksheet.Cells[1, 42].Value = "Round Off";
                        excelWorksheet.Cells[1, 43].Value = "PARTY RECEIVABLE";
                        excelWorksheet.Cells[1, 44].Value = "Acknowledgment No.";
                        excelWorksheet.Cells[1, 45].Value = "Ack Date";
                        excelWorksheet.Cells[1, 46].Value = "E-way bill No";
                        excelWorksheet.Cells[1, 47].Value = "Eway bill Date";
                        excelWorksheet.Cells[1, 48].Value = "Dispatched Through/ Transporter";
                        excelWorksheet.Cells[1, 49].Value = "Motor Vehicle No";
                        excelWorksheet.Cells[1, 50].Value = "Bill of Lading / LR-RR No";
                        excelWorksheet.Cells[1, 51].Value = "LR Date";
                        excelWorksheet.Cells[1, 52].Value = "Customer Phone NO";
                        excelWorksheet.Cells[1, 53].Value = "Driver Phone No";
                        excelWorksheet.Cells[1, 54].Value = "Narration";

                        excelWorksheet.Cells[1, 1, 1, 19].Style.Font.Bold = true;
                        #endregion
                        for (int k = 0; k < TblInvoiceRptTOListByInvoiceItemDistictId.Count; k++)
                        {
                            invoiceId = 0;
                            TblInvoiceRptTOListByInvoiceItemId = TblInvoiceRptTOList.
                                Where(w => w.IdInvoice == TblInvoiceRptTOListByInvoiceItemDistictId[k].IdInvoice).ToList();
                            for (int i = 0; i < TblInvoiceRptTOListByInvoiceItemId.Count; i++)
                            {
                                if (invoiceId != 0)// && invoiceId != TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice)
                                {

                                    excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNo;
                                    excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                                    excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                                    excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerAddress;
                                    excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerDistict;
                                    excelWorksheet.Cells[cellRow, 6].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerTaluka;
                                    excelWorksheet.Cells[cellRow, 7].Value = "";
                                    excelWorksheet.Cells[cellRow, 8].Value = "";
                                    excelWorksheet.Cells[cellRow, 9].Value = "";
                                    excelWorksheet.Cells[cellRow, 10].Value = "";
                                    excelWorksheet.Cells[cellRow, 11].Value = "";
                                    excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;
                                    excelWorksheet.Cells[cellRow, 13].Value = "";
                                    excelWorksheet.Cells[cellRow, 14].Value = "";
                                    excelWorksheet.Cells[cellRow, 15].Value = "";
                                    excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeTaluka;
                                    excelWorksheet.Cells[cellRow, 17].Value = "";
                                    excelWorksheet.Cells[cellRow, 18].Value = "";
                                    excelWorksheet.Cells[cellRow, 19].Value = "";
                                    excelWorksheet.Cells[cellRow, 20].Value = "";
                                    excelWorksheet.Cells[cellRow, 21].Value = "";

                                    excelWorksheet.Cells[cellRow, 22].Value = "";
                                    excelWorksheet.Cells[cellRow, 23].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProDesc;
                                    excelWorksheet.Cells[cellRow, 24].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxPCT;
                                    excelWorksheet.Cells[cellRow, 25].Value = TblInvoiceRptTOListByInvoiceItemId[i].GstCodeNo;
                                    excelWorksheet.Cells[cellRow, 26].Value = TblInvoiceRptTOListByInvoiceItemId[i].MaterialName;
                                    excelWorksheet.Cells[cellRow, 27].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                                    excelWorksheet.Cells[cellRow, 28].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                                    excelWorksheet.Cells[cellRow, 29].Value = TblInvoiceRptTOListByInvoiceItemId[i].BookingRate;
                                    excelWorksheet.Cells[cellRow, 30].Value = TblInvoiceRptTOListByInvoiceItemId[i].Rate;
                                    excelWorksheet.Cells[cellRow, 31].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;
                                    excelWorksheet.Cells[cellRow, 32].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt;

                                    excelWorksheet.Cells[cellRow, 33].Value = "";
                                    excelWorksheet.Cells[cellRow, 34].Value = TblInvoiceRptTOListByInvoiceItemId[i].SalesLedger;
                                    excelWorksheet.Cells[cellRow, 35].Value = "";
                                    excelWorksheet.Cells[cellRow, 36].Value = "";
                                    excelWorksheet.Cells[cellRow, 37].Value = "";
                                    excelWorksheet.Cells[cellRow, 38].Value = "";
                                    excelWorksheet.Cells[cellRow, 39].Value = "";
                                    excelWorksheet.Cells[cellRow, 40].Value = "";
                                    excelWorksheet.Cells[cellRow, 41].Value = "";
                                    excelWorksheet.Cells[cellRow, 42].Value = TblInvoiceRptTOListByInvoiceItemId[i].roundOffAmt;
                                    excelWorksheet.Cells[cellRow, 43].Value = "";
                                    excelWorksheet.Cells[cellRow, 44].Value = "";

                                    excelWorksheet.Cells[cellRow, 45].Value = "";
                                    excelWorksheet.Cells[cellRow, 46].Value = "";
                                    excelWorksheet.Cells[cellRow, 47].Value = "";
                                    excelWorksheet.Cells[cellRow, 48].Value = "";
                                    excelWorksheet.Cells[cellRow, 49].Value = "";
                                    excelWorksheet.Cells[cellRow, 50].Value = "";
                                    excelWorksheet.Cells[cellRow, 51].Value = "";
                                    excelWorksheet.Cells[cellRow, 52].Value = "";
                                    excelWorksheet.Cells[cellRow, 53].Value = "";
                                    excelWorksheet.Cells[cellRow, 54].Value = "";

                                    //  excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;
                                    cellRow++;

                                }
                                else
                                {
                                    excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceNo;
                                    excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceDateStr;
                                    excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOListByInvoiceItemId[i].PartyName;
                                    excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerAddress;
                                    excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerDistict;
                                    excelWorksheet.Cells[cellRow, 6].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerTaluka;
                                    excelWorksheet.Cells[cellRow, 7].Value = "";
                                    excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerState;
                                    excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyercountryName;
                                    excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerPincode;
                                    excelWorksheet.Cells[cellRow, 11].Value = TblInvoiceRptTOListByInvoiceItemId[i].BuyerGstNo;
                                    excelWorksheet.Cells[cellRow, 12].Value = TblInvoiceRptTOListByInvoiceItemId[i].CnfName;
                                    excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId[i].Consignee;
                                    excelWorksheet.Cells[cellRow, 14].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeAddress;
                                    excelWorksheet.Cells[cellRow, 15].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeDistict;
                                    excelWorksheet.Cells[cellRow, 16].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeTaluka;
                                    excelWorksheet.Cells[cellRow, 17].Value = "";
                                    excelWorksheet.Cells[cellRow, 18].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeState;
                                    excelWorksheet.Cells[cellRow, 19].Value = TblInvoiceRptTOListByInvoiceItemId[i].consigneecountryName;
                                    excelWorksheet.Cells[cellRow, 20].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneePinCode;
                                    excelWorksheet.Cells[cellRow, 21].Value = TblInvoiceRptTOListByInvoiceItemId[i].ConsigneeGstNo;

                                    excelWorksheet.Cells[cellRow, 22].Value = TblInvoiceRptTOListByInvoiceItemId[i].DeliveryLocation;
                                    excelWorksheet.Cells[cellRow, 23].Value = TblInvoiceRptTOListByInvoiceItemId[i].ProDesc;
                                    excelWorksheet.Cells[cellRow, 24].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxPCT;
                                    excelWorksheet.Cells[cellRow, 25].Value = TblInvoiceRptTOListByInvoiceItemId[i].GstCodeNo;
                                    excelWorksheet.Cells[cellRow, 26].Value = TblInvoiceRptTOListByInvoiceItemId[i].MaterialName;
                                    excelWorksheet.Cells[cellRow, 27].Value = TblInvoiceRptTOListByInvoiceItemId[i].Bundles;
                                    excelWorksheet.Cells[cellRow, 28].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceQty;
                                    excelWorksheet.Cells[cellRow, 29].Value = TblInvoiceRptTOListByInvoiceItemId[i].BookingRate;
                                    excelWorksheet.Cells[cellRow, 30].Value = TblInvoiceRptTOListByInvoiceItemId[i].Rate;
                                    excelWorksheet.Cells[cellRow, 31].Value = TblInvoiceRptTOListByInvoiceItemId[i].CdStructure;
                                    excelWorksheet.Cells[cellRow, 32].Value = TblInvoiceRptTOListByInvoiceItemId[i].TaxableAmt;

                                    excelWorksheet.Cells[cellRow, 33].Value = TblInvoiceRptTOListByInvoiceItemId[i].InvoiceTaxableAmt;
                                    excelWorksheet.Cells[cellRow, 34].Value = TblInvoiceRptTOListByInvoiceItemId[i].SalesLedger;
                                    excelWorksheet.Cells[cellRow, 35].Value = TblInvoiceRptTOListByInvoiceItemId[i].Freight_GL;
                                    excelWorksheet.Cells[cellRow, 36].Value = TblInvoiceRptTOListByInvoiceItemId[i].FreightAmt;
                                    excelWorksheet.Cells[cellRow, 37].Value = TblInvoiceRptTOListByInvoiceItemId[i].CgstTaxAmt;
                                    excelWorksheet.Cells[cellRow, 38].Value = TblInvoiceRptTOListByInvoiceItemId[i].SgstTaxAmt;
                                    excelWorksheet.Cells[cellRow, 39].Value = TblInvoiceRptTOListByInvoiceItemId[i].IgstTaxAmt;
                                    excelWorksheet.Cells[cellRow, 40].Value = TblInvoiceRptTOListByInvoiceItemId[i].TCS_GL;
                                    excelWorksheet.Cells[cellRow, 41].Value = TblInvoiceRptTOListByInvoiceItemId[i].TcsAmt;
                                    excelWorksheet.Cells[cellRow, 42].Value = TblInvoiceRptTOListByInvoiceItemId[i].roundOffAmt;
                                    excelWorksheet.Cells[cellRow, 43].Value = TblInvoiceRptTOListByInvoiceItemId[i].GrandTotal;
                                    excelWorksheet.Cells[cellRow, 44].Value = TblInvoiceRptTOListByInvoiceItemId[i].AckNo;

                                    excelWorksheet.Cells[cellRow, 45].Value = TblInvoiceRptTOListByInvoiceItemId[i].AckDate;
                                    excelWorksheet.Cells[cellRow, 46].Value = TblInvoiceRptTOListByInvoiceItemId[i].EwbNo;
                                    excelWorksheet.Cells[cellRow, 47].Value = TblInvoiceRptTOListByInvoiceItemId[i].EwbDate;
                                    excelWorksheet.Cells[cellRow, 48].Value = TblInvoiceRptTOListByInvoiceItemId[i].TransporterName;
                                    excelWorksheet.Cells[cellRow, 49].Value = TblInvoiceRptTOListByInvoiceItemId[i].VehicleNo;
                                    excelWorksheet.Cells[cellRow, 50].Value = TblInvoiceRptTOListByInvoiceItemId[i].LrNumber;
                                    excelWorksheet.Cells[cellRow, 51].Value = "";
                                    excelWorksheet.Cells[cellRow, 52].Value = TblInvoiceRptTOListByInvoiceItemId[i].DealerMobNo;
                                    excelWorksheet.Cells[cellRow, 53].Value = TblInvoiceRptTOListByInvoiceItemId[i].ContactNo;
                                    excelWorksheet.Cells[cellRow, 54].Value = TblInvoiceRptTOListByInvoiceItemId[i].NarrationConcat;


                                    invoiceId = TblInvoiceRptTOListByInvoiceItemId[i].IdInvoice;
                                    excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;
                                    cellRow++;

                                }
                            }
                            // For last record.
                            //if (i == (TblInvoiceRptTOListByInvoiceItemId.Count - 1))

                        }

                        // List<TblInvoiceRptTO> TblInvoiceRptTOListnew = TblInvoiceRptTOListByInvoiceItemId.Where(ele => ele.IdInvoice == invoiceId).Select(ele => ele).ToList();

                        excelWorksheet.Cells[cellRow, 1].Value = "Grand Total";
                        excelWorksheet.Cells[cellRow, 28].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.InvoiceQty), 2);
                        excelWorksheet.Cells[cellRow, 32].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.TaxableAmt), 2);
                        excelWorksheet.Cells[cellRow, 33].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.InvoiceTaxableAmt), 2);
                        excelWorksheet.Cells[cellRow, 37].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.CgstTaxAmt), 2);
                        excelWorksheet.Cells[cellRow, 38].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.SgstTaxAmt), 2);
                        excelWorksheet.Cells[cellRow, 39].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.IgstTaxAmt), 2);
                        excelWorksheet.Cells[cellRow, 43].Value = Math.Round(TblInvoiceRptTOList.Sum(ele => ele.GrandTotal), 2);

                        //excelWorksheet.Cells[cellRow, 8].Value = TblInvoiceRptTOListnew.Select(ele => ele.GrossWeight / 1000);
                        //excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOListnew.Select(ele => ele.TareWeight / 1000);
                        //excelWorksheet.Cells[cellRow, 10].Value = TblInvoiceRptTOListnew.Select(ele => ele.NetWeight / 1000);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;
                        //cellRow++;

                        // For final total.
                        //excelWorksheet.Cells[cellRow, 9].Value = "Grand Total";
                        //excelWorksheet.Cells[cellRow, 13].Value = TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.InvoiceQty);
                        //excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.TaxableAmt), 2);

                        //excelWorksheet.Cells[cellRow, 15].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);
                        //excelWorksheet.Cells[cellRow, 16].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.CdAmt), 2);
                        //excelWorksheet.Cells[cellRow, 17].Value = Math.Round(TblInvoiceRptTOListByInvoiceItemId.Sum(ele => ele.GrandTotal), 2);

                        excelWorksheet.Cells[cellRow, 1, cellRow, 54].Style.Font.Bold = true;


                        using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 54])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                            range.Style.Font.Name = "Times New Roman";
                            range.Style.Font.Size = 10;
                        }

                    }


                    #endregion

                    excelWorksheet.Protection.IsProtected = true;
                    excelPackage.Workbook.Protection.LockStructure = true;
                    #region Upload File to Azure

                    //IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUWest1);
                    //IAmazonS3 client = Amazon.S3.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUWest1);

                    Amazon.Runtime.AWSCredentials credentials = new Amazon.Runtime.StoredProfileAWSCredentials("development");
                    Amazon.S3.IAmazonS3 s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USWest2);

                    // Create azure storage  account connection.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Retrieve reference to a target container.
                    CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                    String fileName = Constants.ExcelFileName + _iCommon.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-" + minDate + "-" + maxDate + "-R" + ".xlsx";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    var fileStream = excelPackage.GetAsByteArray();

                    Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                    excelPackage.Dispose();
                    #endregion
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                resultMessage.DefaultBehaviour();
                //return _iTblInvoiceDAO.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempInvoiceExcel");
            }
            return resultMessage;
        }
        
        public ResultMessage SelectAllRptNCList(DateTime frmDt, DateTime toDt)
        {

            ResultMessage resultMessage = new ResultMessage();
            int Lresult = 0;
            try
            {
                List<TblInvoiceRptTO> TblInvoiceRptTOList = new List<TblInvoiceRptTO>();

              //  Lresult = _iTblInvoiceDAO.InsertNCReportLog("SelectAllRptNCList", "before select Data");
                TblInvoiceRptTOList = _iTblInvoiceDAO.SelectAllRptNCList(frmDt, toDt);
               // Lresult = _iTblInvoiceDAO.InsertNCReportLog("SelectAllRptNCList", "After select Data");
                if (TblInvoiceRptTOList != null && TblInvoiceRptTOList.Count > 0)
                {
                   // Lresult = _iTblInvoiceDAO.InsertNCReportLog("SelectAllRptNCList", "TblInvoiceRptTOList not null and Count > 0 ");
                    ExcelPackage excelPackage = new ExcelPackage();
                    int cellRow = 2;
                    excelPackage = new ExcelPackage();
                    string minDate = TblInvoiceRptTOList.Min(ele => ele.InvDate).ToString("ddMMyy");
                    string maxDate = TblInvoiceRptTOList.Max(ele => ele.InvDate).ToString("ddMMyy");



                    #region Excel Column Prepareration
                    ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(Constants.ExcelSheetName);

                    excelWorksheet.Cells[1, 1].Value = "SrNo";
                    excelWorksheet.Cells[1, 2].Value = "Date";
                    excelWorksheet.Cells[1, 3].Value = "Dealer Name";
                    excelWorksheet.Cells[1, 4].Value = "vehicleNo";
                    excelWorksheet.Cells[1, 5].Value = "Size";
                    excelWorksheet.Cells[1, 6].Value = "Net Wt.";
                    excelWorksheet.Cells[1, 7].Value = "Tare Wt.";
                    excelWorksheet.Cells[1, 8].Value = "Gross Wt.";
                    excelWorksheet.Cells[1, 9].Value = "Bundle";
                    excelWorksheet.Cells[1, 10].Value = "BASIC RATE";
                    excelWorksheet.Cells[1, 11].Value = "C.D.";
                    excelWorksheet.Cells[1, 12].Value = "Difference";
                    excelWorksheet.Cells[1, 13].Value = "FinalRate";
                    excelWorksheet.Cells[1, 14].Value = "FinalAmt";
                    excelWorksheet.Cells[1, 15].Value = "Remark";

                    excelWorksheet.Cells[1, 1, 1, 24].Style.Font.Bold = true;
                    #endregion
                    for (int i = 0; i < TblInvoiceRptTOList.Count; i++)
                    {
                        excelWorksheet.Cells[cellRow, 1].Value = TblInvoiceRptTOList[i].SrNo;
                        excelWorksheet.Cells[cellRow, 2].Value = TblInvoiceRptTOList[i].Date;
                        excelWorksheet.Cells[cellRow, 3].Value = TblInvoiceRptTOList[i].DealerName;
                        excelWorksheet.Cells[cellRow, 4].Value = TblInvoiceRptTOList[i].VehicleNo;
                        excelWorksheet.Cells[cellRow, 5].Value = TblInvoiceRptTOList[i].Size;
                        excelWorksheet.Cells[cellRow, 6].Value = Math.Round(TblInvoiceRptTOList[i].NetWt, 2);
                        excelWorksheet.Cells[cellRow, 7].Value = Math.Round(TblInvoiceRptTOList[i].TareWt, 2);
                        excelWorksheet.Cells[cellRow, 8].Value = Math.Round(TblInvoiceRptTOList[i].GrossWt, 2);
                        excelWorksheet.Cells[cellRow, 9].Value = TblInvoiceRptTOList[i].Bundle;
                        excelWorksheet.Cells[cellRow, 10].Value = Math.Round(TblInvoiceRptTOList[i].BASICRATE, 2);
                        excelWorksheet.Cells[cellRow, 11].Value = Math.Round(TblInvoiceRptTOList[i].CD, 2);
                        excelWorksheet.Cells[cellRow, 12].Value = Math.Round(TblInvoiceRptTOList[i].Difference, 2);
                        excelWorksheet.Cells[cellRow, 13].Value = Math.Round(TblInvoiceRptTOList[i].FinalRate, 0);
                        excelWorksheet.Cells[cellRow, 14].Value = Math.Round(TblInvoiceRptTOList[i].FinalAmt, 0);
                        excelWorksheet.Cells[cellRow, 15].Value = TblInvoiceRptTOList[i].Remark;
                        cellRow++;


                        using (ExcelRange range = excelWorksheet.Cells[1, 1, cellRow, 21])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                            range.Style.Font.Name = "Times New Roman";
                            range.Style.Font.Size = 10;
                        }
                    }

                    excelWorksheet.Protection.IsProtected = true;
                    excelPackage.Workbook.Protection.LockStructure = true;
                    #region Upload File to Azure

                    // Create azure storage  account connection.
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_iConnectionString.GetConnectionString(Constants.AZURE_CONNECTION_STRING));

                    // Create the blob client.
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                    // Retrieve reference to a target container.
                    CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureSourceContainerName);

                    String fileName = Constants.ExcelFileNameForNCRpt + _iCommon.ServerDateTime.ToString("ddMMyyyyHHmmss") + "-" + minDate + "-" + maxDate + "-R" + ".xlsx";
                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

                    var fileStream = excelPackage.GetAsByteArray();

                    Task t1 = blockBlob.UploadFromByteArrayAsync(fileStream, 0, fileStream.Length);

                    excelPackage.Dispose();
                    #endregion
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;

                }
                resultMessage.DefaultBehaviour();
                
            }
            catch (Exception ex)
            {
                Lresult = _iTblInvoiceDAO.InsertNCReportLog("SelectAllRptNCList", ex.ToString());
                resultMessage.DefaultExceptionBehaviour(ex, "CreateTempInvoiceExcel");
            }
            return resultMessage;
        }

        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            return _iTblInvoiceDAO.SelectInvoiceExportList(frmDt, toDt, isConfirm, fromOrgId);
        }

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            return _iTblInvoiceDAO.SelectHsnExportList(frmDt, toDt, isConfirm, fromOrgId);
        }

        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            return _iTblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, fromOrgId);
        }

        //Added by minal 06 Aug 2021 

        public ResultMessage SelectItemWiseSalesExportCListForReport(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            ResultMessage resultMessage = new ResultMessage();
            List<Object> resultMessageMulti = new List<Object>();

            List<TblInvoiceRptTO> itemWiseSalesExportCList = _iTblInvoiceDAO.SelectItemWiseSalesExportCListForReport(frmDt, toDt, isConfirm, fromOrgId);

            if (itemWiseSalesExportCList == null || itemWiseSalesExportCList.Count == 0)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.DisplayMessage = "No record found";
                return resultMessage;
            }

            var summuryGroupList = itemWiseSalesExportCList.ToLookup(p => p.IdInvoice).ToList();
            if (summuryGroupList != null)
            {
                TblInvoiceRptTO tblInvoiceRptTotalTO = new TblInvoiceRptTO();
                for (int i = 0; i < summuryGroupList.Count; i++)
                {
                    String narration1 = String.Empty, narration2 = String.Empty, narration3 = String.Empty, narration4 = String.Empty, prevProdCateDesc = String.Empty;

                    narration1 = "Invoice No. " + summuryGroupList[i].FirstOrDefault().InvoiceNo + "  ";
                    narration4 = "  Vehicle No. " + summuryGroupList[i].FirstOrDefault().VehicleNo + " ( " + summuryGroupList[i].FirstOrDefault().TotalItemQty + "  in mt )" +
                                 //" ,Basic Rate : " + summuryGroupList[i].FirstOrDefault().BasicRate + "  ,DO Date : " + summuryGroupList[i].FirstOrDefault().LoadingSlipDate;
                                 " ,Basic Rate : " + summuryGroupList[i].FirstOrDefault().BookingRate + "  ,DO Date : " + summuryGroupList[i].FirstOrDefault().LoadingSlipDate;
                    foreach (var item in summuryGroupList[i])
                    {
                        if (item.InvoiceNo.ToString() == "26".ToString())
                        {

                        }

                        if (!String.IsNullOrEmpty(prevProdCateDesc))
                        {
                            if (prevProdCateDesc != item.ProdCateDesc)
                            {
                                string desc = item.ProdCateDesc;
                                if (String.IsNullOrEmpty(desc))
                                {
                                    desc = item.ProdItemDesc;
                                }

                                //narration2 =  " For " + item.ProdCateDesc + "  ";
                                narration2 = " For " + desc + "  ";
                                narration3 = narration3 + narration2;
                                //prevProdCateDesc = item.ProdCateDesc;
                                prevProdCateDesc = desc;
                            }
                        }
                        else
                        {
                            string desc = item.ProdCateDesc;
                            if (String.IsNullOrEmpty(desc))
                            {
                                desc = item.ProdItemDesc;
                            }

                            //narration2 = " For " + item.ProdCateDesc + "  ";
                            narration2 = " For " + desc + "  ";
                            narration3 = narration3 + narration2;
                            //prevProdCateDesc = item.ProdCateDesc;
                            prevProdCateDesc = desc;
                        }
                        narration3 += item.MaterialName + " ( " + item.InvoiceQty + " mt* " + item.Rate + "  ),";
                    }
                    narration1 = narration1 + narration3 + narration4;
                    foreach (var item in summuryGroupList[i])
                    {
                        item.NarrationConcat = narration1;
                    }
                    tblInvoiceRptTotalTO.InvoiceNo = "Grand Total";
                    tblInvoiceRptTotalTO.InvoiceQty += summuryGroupList[i].Sum(a => a.InvoiceQty);
                    tblInvoiceRptTotalTO.TaxableAmt += summuryGroupList[i].Sum(a => a.TaxableAmt);
                    tblInvoiceRptTotalTO.InvoiceTaxableAmt += summuryGroupList[i].FirstOrDefault().InvoiceTaxableAmt;
                    tblInvoiceRptTotalTO.FreightAmt += summuryGroupList[i].FirstOrDefault().FreightAmt;
                    tblInvoiceRptTotalTO.CgstTaxAmt += summuryGroupList[i].FirstOrDefault().CgstTaxAmt;
                    tblInvoiceRptTotalTO.SgstTaxAmt += summuryGroupList[i].FirstOrDefault().SgstTaxAmt;
                    tblInvoiceRptTotalTO.IgstTaxAmt += summuryGroupList[i].FirstOrDefault().IgstTaxAmt;
                    tblInvoiceRptTotalTO.GrandTotal += summuryGroupList[i].FirstOrDefault().GrandTotal;


                }
                itemWiseSalesExportCList.Add(tblInvoiceRptTotalTO);
            }

            DataSet printDataSet = new DataSet();
            DataTable itemWiseSalesExportCListDT = new DataTable();
            if (itemWiseSalesExportCList != null && itemWiseSalesExportCList.Count > 0)
            {
                itemWiseSalesExportCListDT = Common.ToDataTable(itemWiseSalesExportCList);
            }
            itemWiseSalesExportCListDT.TableName = "itemWiseSalesExportCListDT";
            printDataSet.Tables.Add(itemWiseSalesExportCListDT);
            String ReportTemplateName = "ItemWiseSalesExportCRpt";
            String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
            String fileName = "Doc-" + DateTime.Now.Ticks;
            String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
            Boolean IsProduction = true;
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
            if (tblConfigParamsTO != null)
            {
                if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                {
                    IsProduction = false;
                }
            }
            resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
            if (resultMessage.MessageType == ResultMessageE.Information)
            {
                String filePath = String.Empty;
                if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                {
                    filePath = resultMessage.Tag.ToString();
                }
                //driveName + path;
                int returnPath = 0;
                if (returnPath != 1)
                {
                    String fileName1 = Path.GetFileName(saveLocation);
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null && bytes.Length > 0)
                    {
                        resultMessage.Tag = bytes;

                        string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        string directoryName;
                        directoryName = Path.GetDirectoryName(saveLocation);
                        string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                        string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

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
                        resultMessageMulti.Add(resultMessage.Tag);
                        //return resultMessage;
                    }
                }

            }
            else
            {
                resultMessage.Text = "Something wents wrong please try again";
                resultMessage.DisplayMessage = "Something wents wrong please try again";
                resultMessage.Result = 0;
            }

            resultMessage.DefaultSuccessBehaviour();
            //resultMessage.TagList = resultMessageMulti;
            return resultMessage;
        }

        public ResultMessage PrintSaleReport(DateTime frmDt, DateTime toDt, int isConfirm, string selectedOrg, int isFromPurchase = 0)
        {
            Int32 defualtOrgId = 0;
            ResultMessage resultMessage = new ResultMessage();

            List<TblInvoiceRptTO> TblReportsTOList = _iTblInvoiceDAO.SelectSalesPurchaseListForReport(frmDt, toDt, isConfirm, selectedOrg, defualtOrgId, isFromPurchase);

            if (TblReportsTOList != null && TblReportsTOList.Count > 0)
            {
                DataSet printDataSet = new DataSet();
                DataTable headerDT = new DataTable();
                if (TblReportsTOList != null && TblReportsTOList.Count > 0)
                {
                    headerDT = Common.ToDataTable(TblReportsTOList);
                }
                headerDT.TableName = "headerDT";

                printDataSet.Tables.Add(headerDT);
                String ReportTemplateName = "SalesReportRpt";
                if (isFromPurchase == 1)
                {
                    ReportTemplateName = "PurchaseReportRpt";
                }

                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
                String fileName = "Doc-" + DateTime.Now.Ticks;
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {
                        filePath = resultMessage.Tag.ToString();
                    }
                    //driveName + path;
                    int returnPath = 0;
                    if (returnPath != 1)
                    {
                        String fileName1 = Path.GetFileName(saveLocation);
                        Byte[] bytes = File.ReadAllBytes(filePath);
                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = bytes;

                            string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                            string directoryName;
                            directoryName = Path.GetDirectoryName(saveLocation);
                            string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                            string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

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
                            return resultMessage;
                        }
                    }

                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
            }
            else
            {
                resultMessage.DefaultBehaviour();
                return resultMessage;

            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        //Added by minal 06 Aug 2021
        // Vaibhav [14-Nov-2017] added to select invoice details by loading id
        public List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId)
        {
            return _iTblInvoiceDAO.SelectAllTempInvoice(loadingSlipId);
        }
        // Vaibhav [14-Nov-2017] added to select invoice details by loading id
        public List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.SelectAllTempInvoice(loadingSlipId, conn, tran);
        }

        /// <summary>
        /// Vijaymla : Added to check invoices details according to region
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public Boolean CheckInvoiceDetailsAccToState(TblInvoiceTO tblInvoiceTO, ref String errorMsg)
        {
            if (tblInvoiceTO != null)
            {
                Int32 organizationId = 0;
                if (tblInvoiceTO.InvFromOrgId > 0)
                {
                    organizationId = tblInvoiceTO.InvFromOrgId;

                }
                else
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (configParamsTO != null)
                    {
                        organizationId = Convert.ToInt32(configParamsTO.ConfigParamVal);
                    }
                }

                //Get Address Of Organization
                Constants.AddressTypeE addressTypeE = Constants.AddressTypeE.OFFICE_ADDRESS;
                TblAddressTO tblOrgAddressTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(organizationId, addressTypeE);
                if (tblOrgAddressTO == null)
                {
                    errorMsg = "Organization Address Not Found";
                    return false;
                }
                List<TblInvoiceAddressTO> tblInvoiceAddressTOList = new List<TblInvoiceAddressTO>();

                if (tblInvoiceTO.InvoiceAddressTOList == null || tblInvoiceTO.InvoiceAddressTOList.Count == 0)
                {
                    tblInvoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(tblInvoiceTO.IdInvoice);
                }
                else
                {
                    tblInvoiceAddressTOList = tblInvoiceTO.InvoiceAddressTOList;
                }
                if (tblInvoiceAddressTOList == null || tblInvoiceAddressTOList.Count == 0)
                {
                    errorMsg = "Invoice Delivery Address Not Found";
                    return false;
                }

                TblInvoiceAddressTO tblInvoiceAddressTO = tblInvoiceAddressTOList.Where(ele => ele.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                if (tblInvoiceAddressTO != null)
                {
                    //Aniket [9-9-2019] added to check StateId is null or not.. if null do not allow to create or udate the invoice
                    // issue was if stateId not found automatically convert  CGST invoice calc to IGST
                    if (tblInvoiceAddressTO.StateId == 0)
                    {
                        errorMsg = "State not found. Kindly select State from list in billing address ";
                        return false;
                    }
                    if (tblOrgAddressTO.StateId == tblInvoiceAddressTO.StateId)
                    {
                        if (tblInvoiceTO.IgstAmt > 0)
                        {
                            errorMsg = "Bill is within state, IGST Applied,Please check once and try again";
                            return false;
                        }

                    }
                    else
                    {
                        if (tblInvoiceTO.CgstAmt > 0)
                        {
                            errorMsg = "Bill is out of state, CGST Applied,Please check once and try again";

                            return false;
                        }
                        if (tblInvoiceTO.SgstAmt > 0)
                        {
                            errorMsg = "Bill is out of state, SGST Applied,Please check once and try again";
                            return false;
                        }
                    }

                }
            }
            return true;

        }

        /// <summary>
        /// Vijaymala[12-04-2017] :Added to get invoice list by using vehicle number
        /// </summary>
        /// <param name="vehicleNo"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectAllInvoiceListByVehicleNo(string vehicleNo, DateTime frmDt, DateTime toDt)
        {
            List<TblLoadingSlipTO> tblLoadingSlipTOList = _iTblLoadingSlipBL.SelectAllLoadingListByVehicleNo(vehicleNo, frmDt, toDt);
            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
            if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0)
            {
                String strLoadingSlipIds = String.Join(",", tblLoadingSlipTOList.Select(s => s.IdLoadingSlip.ToString()).ToArray());
                if (!String.IsNullOrEmpty(strLoadingSlipIds))
                {
                    List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceBL.SelectAllTempLoadingSlipInvoiceList(strLoadingSlipIds);
                    String strInvoiceIds = String.Join(",", tempLoadingSlipInvoiceTOList.Select(s => s.InvoiceId.ToString()).ToArray());

                    if (!String.IsNullOrEmpty(strInvoiceIds))
                    {
                        tblInvoiceTOList = SelectInvoiceListFromInvoiceIds(strInvoiceIds);
                        if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                        {
                            return tblInvoiceTOList;
                        }
                    }

                }
            }
            return tblInvoiceTOList;

        }


        /// <summary>
        /// Vijaymala [2018-02-15] Added:to get invoice list fron loading slip
        /// </summary>
        /// <param name="invoiceIds"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectInvoiceListFromInvoiceIds(String invoiceIds)
        {
            return _iTblInvoiceDAO.SelectInvoiceListFromInvoiceIds(invoiceIds);
        }

        /// <summary>
        /// Vaibhav [23-April-2018] Added to select final invoice list.
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectAllFinalInvoiceList(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.SelectAllFinalInvoice(conn, tran);
        }

        public List<TblLoadingSlipTO> SelectLoadingSlipDetailsByInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.SelectLoadingDetailsByInvoiceId(invoiceId, conn, tran);
        }

        /// <summary>
        /// Vijaymala added [09-05-2018]:To get notified invoices list
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId)
        {
            return _iTblInvoiceDAO.SelectAllTNotifiedblInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
        }

        /// <summary>
        /// AmolG[2022-Feb-14] This function is used for Size wise report
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public List<InvoiceReportTO> GetAllInvoices(DateTime fromDate, DateTime toDate, ref String errorMsg)
        {
            try
            {
                List<InvoiceReportTO> invoiceReportTOListFinal = new List<InvoiceReportTO>();
                List<InvoiceReportTO> invoiceReportTOList = _iTblInvoiceDAO.SelectAllInvoices(fromDate, toDate);
                Dictionary<string, string> ProdMaterialQtyDCT = new Dictionary<string, string>();
                if (invoiceReportTOList != null && invoiceReportTOList.Count > 0)
                {
                    List<int> invoiceIdList = invoiceReportTOList.Select(x => x.InvoiceId).Distinct().ToList();
                    Double grandTotal = 0;
                    for (int i = 0; i < invoiceIdList.Count; i++)
                    {
                        int invoiceId = invoiceIdList[i];
                        InvoiceReportTO invoiceReportTO = new InvoiceReportTO();
                        invoiceReportTO.ProdMaterialQtyDCT = new Dictionary<string, string>();

                        List<InvoiceReportTO> invoiceReportTOListLocal = invoiceReportTOList.Where(x => x.InvoiceId == invoiceId).ToList();
                        //Find the Party and Consinee name suing invoice id

                        List<TblInvoiceAddressTO> tblInvoiceAddressTOList = _iTblInvoiceAddressBL.SelectTblInvoice(invoiceId);

                        if (tblInvoiceAddressTOList != null && tblInvoiceAddressTOList.Count > 0)
                        {
                            //Party Address
                            var exist = from lst in tblInvoiceAddressTOList
                                        where lst.TxnAddrTypeId == 1
                                        select lst;
                            if (exist != null && exist.Any())
                            {
                                invoiceReportTO.PartyName = exist.FirstOrDefault().BillingName;
                            }

                            var exisCon = from lst in tblInvoiceAddressTOList
                                          where lst.TxnAddrTypeId == 2
                                          select lst;
                            if (exisCon != null && exisCon.Any())
                            {
                                invoiceReportTO.ConsigneeName = exisCon.FirstOrDefault().BillingName;
                            }
                        }

                        if (invoiceReportTOListLocal != null && invoiceReportTOListLocal.Count > 0)
                        {
                            int idInvoie = invoiceReportTOListLocal[0].InvoiceId;
                            if (idInvoie == 949)
                            {
                            }
                            invoiceReportTO.InvoiceNo = invoiceReportTOListLocal[0].InvoiceNo;
                            invoiceReportTO.InvoiceDate = invoiceReportTOListLocal[0].InvoiceDate;
                            invoiceReportTO.InvoiceId = invoiceReportTOListLocal[0].InvoiceId;
                            invoiceReportTO.SuperwiserName = invoiceReportTOListLocal[0].SuperwiserName;
                            invoiceReportTO.VehicleNo = invoiceReportTOListLocal[0].VehicleNo;//Reshma added FOr Gajkesari report
                            invoiceReportTO.SaleEngineer = invoiceReportTOListLocal[0].SaleEngineer;
                            invoiceReportTO.BVCAmt = invoiceReportTOListLocal[0].BVCAmt;
                            invoiceReportTO.ItemDecscription  = invoiceReportTOListLocal[0].ItemDecscription;
                            invoiceReportTO.Rate  = invoiceReportTOListLocal[0].Rate ;
                            invoiceReportTO.CDPct  = invoiceReportTOListLocal[0].CDPct;
                            invoiceReportTO.TotalAmt = invoiceReportTOListLocal[0].TotalAmt ;
                            invoiceReportTO.Freight = invoiceReportTOListLocal[0].Freight;
                            invoiceReportTO.OtherAmt = invoiceReportTOListLocal[0].OtherAmt;
                            invoiceReportTO.TCS = invoiceReportTOListLocal[0].TCS;
                            invoiceReportTO.ParityAmt  = invoiceReportTOListLocal[0].ParityAmt;
                            invoiceReportTO.NonconfParityAmt = invoiceReportTOListLocal[0].NonconfParityAmt;
                            invoiceReportTO.ProdCat = invoiceReportTOListLocal[0].ProdCat;
                            Double total = 0;
                            for (int iC = 0; iC < invoiceReportTOListLocal.Count; iC++)
                            {
                                String colName = invoiceReportTOListLocal[iC].MaterialSubType;
                                //+ "-" + invoiceReportTOListLocal[iC].ProdCat;
                                total += (Double)invoiceReportTOListLocal[iC].InvoiceQty;
                                if (invoiceReportTO.ProdMaterialQtyDCT.ContainsKey(colName))
                                {
                                    //Reshma Added
                                    double Qty = Convert.ToDouble(invoiceReportTO.ProdMaterialQtyDCT[colName]);
                                    Qty += invoiceReportTOListLocal[iC].InvoiceQty;
                                    //invoiceReportTO.ProdMaterialQtyDCT[colName] += invoiceReportTOListLocal[iC].InvoiceQty;
                                    invoiceReportTO.ProdMaterialQtyDCT[colName] = Qty.ToString(); ;

                                }
                                else
                                {
                                    invoiceReportTO.ProdMaterialQtyDCT.Add(colName, invoiceReportTOListLocal[iC].InvoiceQty.ToString());
                                }
                                if (ProdMaterialQtyDCT.ContainsKey(colName))
                                {
                                    double value = Convert.ToDouble(ProdMaterialQtyDCT[colName]);
                                    value = value + invoiceReportTOListLocal[iC].InvoiceQty;
                                    ProdMaterialQtyDCT[colName] = value.ToString(); ;
                                }
                                else
                                {
                                    ProdMaterialQtyDCT.Add(colName, invoiceReportTOListLocal[iC].InvoiceQty.ToString());
                                }
                            }

                            invoiceReportTO.TotalQty = Math.Round(total, 2);
                        }

                        grandTotal += invoiceReportTO.TotalQty;
                        invoiceReportTO.GrandTotalQty = Math.Round(grandTotal, 2);
                        invoiceReportTOListFinal.Add(invoiceReportTO);

                    }
                    //Reshma Added
                    List<DropDownTO> MaterialList = _iTblMaterialDAO.SelectAllMaterialForDropDown();
                    if (invoiceReportTOListFinal != null && invoiceReportTOListFinal.Count > 0)
                    {
                        InvoiceReportTO invoiceReportTO = new InvoiceReportTO();
                        invoiceReportTO.InvoiceNo = "Total";
                        invoiceReportTO.TotalQty = Math.Round(grandTotal, 2);
                        invoiceReportTO.ProdMaterialQtyDCT = ProdMaterialQtyDCT;
                        invoiceReportTO.TCS = Math.Round(Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.TCS)),2);
                        invoiceReportTO.TotalAmt = Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.TotalAmt));
                        invoiceReportTO.Freight = Math.Round(Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.Freight)),2);
                        invoiceReportTO.BVCAmt = Math.Round(Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.BVCAmt)),2);
                        invoiceReportTO.OtherAmt = Math.Round(Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.OtherAmt)),2);
                        invoiceReportTO.ParityAmt  = Math.Round(Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.ParityAmt)), 2);
                        invoiceReportTO.NonconfParityAmt = Math.Round(Convert.ToDouble(invoiceReportTOListFinal.Sum(s => s.NonconfParityAmt)), 2);

                        invoiceReportTOListFinal.Add(invoiceReportTO);

                    }
                    return invoiceReportTOListFinal;
                }
                else
                {
                    errorMsg = "No Invoice Found";
                    return null;
                }
            }
            catch (Exception ex)
            {

                errorMsg = "Exception When GetInvoices. Ex : " + ex.GetBaseException().ToString() + ". StackTrace : " + ex.StackTrace.ToString();
                return null;
            }
        }
        #endregion

        #region Insertion


        //public ResultMessage CreateIntermediateInvoiceAgainstLoading(String loadingIds, Int32 userId)
        //{
        //    List<TblLoadingTO> tblLoadingTOList = _iTblLoadingBL.SelectLoadingTOListWithDetails(loadingIds);

        //    List<TblInvoiceTO> newTblInvoiceTOList = new List<TblInvoiceTO>();

        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    int result = 0;
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    resultMessage.MessageType = ResultMessageE.None;
        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();

        //        if (tblLoadingTOList != null && tblLoadingTOList.Count > 0)
        //        {
        //            for (int i = 0; i < tblLoadingTOList.Count; i++)
        //            {
        //                tblLoadingTOList[i].CreatedBy = userId;
        //                resultMessage = PrepareAndSaveNewTaxInvoice(tblLoadingTOList[i], conn, tran);
        //                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
        //                {
        //                    return resultMessage;
        //                }
        //                else
        //                {
        //                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblInvoiceTO>))
        //                    {
        //                        if (((List<TblInvoiceTO>)resultMessage.Tag).Count > 0)
        //                        {
        //                            newTblInvoiceTOList.AddRange((List<TblInvoiceTO>)resultMessage.Tag);
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        tran.Commit();
        //        resultMessage.MessageType = ResultMessageE.Information;

        //        if (newTblInvoiceTOList != null)
        //        {
        //            resultMessage.Text = "Invoice Saved Sucessfully (" + newTblInvoiceTOList.Count + ")";
        //        }
        //        else
        //        {
        //            resultMessage.Text = "Invoice Saved Sucessfully";
        //        }
        //        resultMessage.Result = 1;
        //        return resultMessage;

        //    }
        //    catch (Exception ex)
        //    {

        //        resultMessage.Text = "Exception Error While Record Save : SaveNewWeighinMachineMeasurement";
        //        resultMessage.MessageType = ResultMessageE.Error;
        //        resultMessage.Exception = ex;
        //        resultMessage.Result = -1;
        //        return resultMessage;
        //    }
        //    finally
        //    {

        //    }

        //}


        /// <summary>
        /// RW:14092017:API This Methos is used to Add new Invoice 
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <returns></returns>
        public ResultMessage InsertTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                /*GJ@20170927 : For get RCM and pass to Invoice*/
                TblConfigParamsTO rcmConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_REVERSE_CHARGE_MECHANISM, conn, tran);
                if (rcmConfigParamsTO == null)
                {
                    tran.Commit();
                    resultMessage.DefaultBehaviour("RCM value Not Found in Configuration.");
                    return resultMessage;
                }


                //Saket [2018-03-17] Comment.
                //Sudhir[15-MAR-2018] Added for Round Off the amount only for Grand Total - 1Rs Diff.
                //double finalGrandTotal = Math.Round(tblInvoiceTO.GrandTotal);
                //tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - tblInvoiceTO.GrandTotal, 2);
                //tblInvoiceTO.GrandTotal = finalGrandTotal;


                resultMessage = SaveNewInvoice(tblInvoiceTO, conn, tran);
                if (resultMessage.MessageType == ResultMessageE.Information && resultMessage.Result == 1)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();

                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                if (tran.Connection.State == ConnectionState.Open)
                    tran.Rollback();

                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }


        }

        public ResultMessage SaveNewInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            try
            {
                //Vijaymala[23-03-2016]added to check invoice details of igst,cgst,sgst taxes
                #region To check invoice details is valid or not
                string errorMsg = string.Empty;
                Boolean isValidInvoice = CheckInvoiceDetailsAccToState(tblInvoiceTO, ref errorMsg);
                if (!isValidInvoice)
                {
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.Text = errorMsg;
                    return resultMessage;
                }
                #endregion

                #region 1. Save the New Invoice

                DimFinYearTO curFinYearTO = _iDimensionBL.GetCurrentFinancialYear(tblInvoiceTO.CreatedOn, conn, tran);
                if (curFinYearTO == null)
                {
                    resultMessage.DefaultBehaviour("Current Fin Year Object Not Found");
                    return resultMessage;
                }

                tblInvoiceTO.FinYearId = curFinYearTO.IdFinYear;
                if (tblInvoiceTO.Narration == null || tblInvoiceTO.Narration == "")
                {
                    tblInvoiceTO.Narration = tblInvoiceTO.DistributorName;
                }
                if (tblInvoiceTO.InvoiceModeE == Constants.InvoiceModeE.MANUAL_INVOICE)
                {
                    tblInvoiceTO.IsConfirmed = 1;
                }
                //[04-01-2018]Vijaymala:Added to auto notify invoice of brought out item
                if (tblInvoiceTO.InvoiceItemDetailsTOList != null || tblInvoiceTO.InvoiceItemDetailsTOList.Count != 0)
                {
                    String strLoadingSlipExtIds = String.Join(",", tblInvoiceTO.InvoiceItemDetailsTOList.Select(s => s.LoadingSlipExtId.ToString()).ToArray());
                    if (!String.IsNullOrEmpty(strLoadingSlipExtIds))
                    {

                        List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExtByIds(strLoadingSlipExtIds, conn, tran);
                        List<TblStockConfigTO> tblStockConfigTOList = _iTblStockConfigDAO.SelectAllTblStockConfigTOList();
                        if (tblLoadingSlipExtTOList != null && tblStockConfigTOList != null)
                        {
                            for (int p = 0; p < tblLoadingSlipExtTOList.Count; p++)
                            {
                                TblLoadingSlipExtTO tblLoadingSlipExtTO = tblLoadingSlipExtTOList[p];
                                Boolean res = tblStockConfigTOList.Exists(ele => ele.ProdCatId == tblLoadingSlipExtTO.ProdCatId && ele.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId
                                && ele.MaterialId == tblLoadingSlipExtTO.MaterialId && ele.BrandId == tblLoadingSlipExtTO.BrandId);


                                if (res)
                                {
                                    tblInvoiceTO.StatusId = (int)Constants.InvoiceStatusE.NEW;
                                    tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
                                    tblInvoiceTO.InvoiceModeId = (int)Constants.InvoiceModeE.AUTO_INVOICE_EDIT;
                                    tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE_EDIT;
                                    break;
                                }

                            }
                        }
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error : Invoce Item Det List Found Empty Or Null");
                    return resultMessage;
                }


                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO paramTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                    if (paramTO != null)
                        tblInvoiceTO.InvFromOrgId = Convert.ToInt32(paramTO.ConfigParamVal);
                }
                //Saket [2018-02-23] Added
                if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                {
                    tblInvoiceTO.BrandId = tblInvoiceTO.InvoiceItemDetailsTOList[0].BrandId;
                }



                result = InsertTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in Insert InvoiceTbl");
                    return resultMessage;
                }
                #endregion

                #region Priyanka [23-01-2019] Save commercial details

                if (tblInvoiceTO.PaymentTermOptionRelationTOLst != null && tblInvoiceTO.PaymentTermOptionRelationTOLst.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceTO.PaymentTermOptionRelationTOLst.Count; i++)
                    {
                        TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = tblInvoiceTO.PaymentTermOptionRelationTOLst[i];
                        tblPaymentTermOptionRelationTO.InvoiceId = tblInvoiceTO.IdInvoice;
                        tblPaymentTermOptionRelationTO.CreatedBy = tblInvoiceTO.CreatedBy;
                        tblPaymentTermOptionRelationTO.LoadingId = 0;
                        tblPaymentTermOptionRelationTO.CreatedOn = _iCommon.ServerDateTime;
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

                //Vijaymala added[11-04-2018]
                #region To map invoice and loading slip
                if (tblInvoiceTO.InvoiceModeE == Constants.InvoiceModeE.AUTO_INVOICE || tblInvoiceTO.InvoiceModeE == Constants.InvoiceModeE.AUTO_INVOICE_EDIT)

                {
                    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = new TempLoadingSlipInvoiceTO();
                    tempLoadingSlipInvoiceTO.InvoiceId = tblInvoiceTO.IdInvoice;
                    tempLoadingSlipInvoiceTO.LoadingSlipId = tblInvoiceTO.LoadingSlipId;
                    tempLoadingSlipInvoiceTO.CreatedBy = tblInvoiceTO.CreatedBy;
                    tempLoadingSlipInvoiceTO.CreatedOn = tblInvoiceTO.CreatedOn;
                    result = _iTempLoadingSlipInvoiceBL.InsertTempLoadingSlipInvoice(tempLoadingSlipInvoiceTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InsertTempLoadingSlipInvoice");
                        return resultMessage;
                    }
                }
                #endregion

                #region 2. Save the Address Details 
                if (tblInvoiceTO.InvoiceAddressTOList == null || tblInvoiceTO.InvoiceAddressTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoce Item Address Det List Found Empty Or Null");
                    return resultMessage;
                }
                for (int i = 0; i < tblInvoiceTO.InvoiceAddressTOList.Count; i++)
                {
                    tblInvoiceTO.InvoiceAddressTOList[i].InvoiceId = tblInvoiceTO.IdInvoice;
                    result = _iTblInvoiceAddressBL.InsertTblInvoiceAddress(tblInvoiceTO.InvoiceAddressTOList[i], conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceAddressDetailTbl");
                        return resultMessage;
                    }

                }
                #endregion


                #region 3. Save the Invoice Item Details
                if (tblInvoiceTO.InvoiceItemDetailsTOList == null || tblInvoiceTO.InvoiceItemDetailsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoce Item Det List Found Empty Or Null");
                    return resultMessage;
                }
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = new TblInvoiceItemDetailsTO();
                    tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                    tblInvoiceItemDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;

                    result = _iTblInvoiceItemDetailsBL.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceItemDetailTbl");
                        return resultMessage;
                    }
                    #region 1. Save the Invoice Tax Item Details             
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList == null
                    || tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count == 0)
                    {
                        if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.REGULAR_TAX_INVOICE
                            || tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITH_DUTY)
                        {
                            resultMessage.DefaultBehaviour("Error : Invoice Item Det Tax List Found Empty Or Null");
                            return resultMessage;
                        }
                    }


                    //harshala

                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList != null)
                    {
                        for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count; j++)
                        {
                            tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j].InvoiceItemId = tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem;
                            result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                return resultMessage;
                            }
                        }
                    }
                    #endregion

                }
                #endregion

                #region 4. Save the invoice data to SAP
                //resultMessage = PostSalesInvoiceToSAP(tblInvoiceTO);
                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {

            }

        }

        public int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {

            return _iTblInvoiceDAO.InsertTblInvoice(tblInvoiceTO, conn, tran);
        }

        public ResultMessage PrepareAndSaveNewTaxInvoice(TblLoadingTO loadingTO, List<TblLoadingSlipExtTO> lastItemList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMsg = new ResultMessage();
            string entityRangeName = string.Empty;

            List<DropDownTO> districtList = new List<DropDownTO>();
            List<DropDownTO> talukaList = new List<DropDownTO>();
            try
            {
                List<TblLoadingSlipTO> loadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(loadingTO.IdLoading, conn, tran);
                loadingTO.LoadingSlipList = loadingSlipTOList;
                int configId = _iTblConfigParamsDAO.IoTSetting();


                //Aniket [19-8-2019] added for IOT
                if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    _iIotCommunication.GetItemDataFromIotAndMerge(loadingTO, true);

                    List<TblLoadingSlipExtTO> allItem = new List<TblLoadingSlipExtTO>();

                    for (int j = 0; j < loadingSlipTOList.Count; j++)
                    {
                        allItem.AddRange(loadingSlipTOList[j].LoadingSlipExtTOList);
                    }

                    for (int j = 0; j < lastItemList.Count; j++)
                    {
                        TblLoadingSlipExtTO item = lastItemList[j];
                        TblLoadingSlipExtTO var = allItem.Where(w => w.IdLoadingSlipExt == item.IdLoadingSlipExt).FirstOrDefault();

                        var.LoadedBundles = item.LoadedBundles;
                        var.LoadedWeight = item.LoadedWeight;
                        var.CalcTareWeight = item.CalcTareWeight;
                    }

                    //var emptyItem = lastItemList.Where(w => w.LoadedWeight <= 0).ToList();
                    //if (emptyItem != null && emptyItem.Count > 0)
                    //{
                    //    resultMsg.DefaultBehaviour("Weight Not Found Against " + emptyItem.Count + " Item ");
                    //    return resultMsg;
                    //}

                    //Saket [2020-07-10] Check all item weight are completed on final invoice generation step.
                    var emptyItem = allItem.Where(w => w.LoadedWeight <= 0).ToList();
                    if (emptyItem != null && emptyItem.Count > 0)
                    {
                        resultMsg.DefaultBehaviour("Weight Not Found Against " + emptyItem.Count + " Item ");
                        return resultMsg;
                    }


                }
                resultMsg = CreateInvoiceAgainstLoadingSlips(loadingTO, conn, tran, loadingSlipTOList);
                // resultMsg.DefaultSuccessBehaviour();
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PrepareAndSaveNewTaxInvoice");
                return resultMsg;
            }
        }

        public ResultMessage CreateInvoiceAgainstLoadingSlips(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran, List<TblLoadingSlipTO> loadingSlipTOList, Int32 skipMergeSetting = 0)
        {

            ResultMessage resultMsg = new ResultMessage();

            if (loadingSlipTOList == null && loadingSlipTOList.Count == 0)
            {
                resultMsg.DefaultBehaviour("Loading Slip list Found Null.");
                return resultMsg;
            }

            DateTime serverDateTime = _iCommon.ServerDateTime;

            #region Check Weighing is done against all items

            for (int q = 0; q < loadingSlipTOList.Count; q++)
            {
                Boolean remove = false;
                TblLoadingSlipTO tblLoadingSlipTOTemp = loadingSlipTOList[q];
                if (tblLoadingSlipTOTemp.LoadingSlipExtTOList == null || tblLoadingSlipTOTemp.LoadingSlipExtTOList.Count == 0)
                {
                    remove = true;
                }

                else
                {
                    int weighingSourceId = _iTblConfigParamsDAO.IoTSetting();
                    if (weighingSourceId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                    {
                        //List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where(w => w.LoadedBundles == 0).ToList();
                        List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where(w => w.LoadedWeight == 0).ToList();
                        if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                        {
                            remove = true;
                        }
                    }
                    else
                    {
                        //14/05/2020 - Yogesh - We are Added this for Skip Wehing Skip Functionality after loding slip approval and commented below code
                        List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where(w => w.LoadedWeight == 0).ToList();
                        //List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where(w => w.WeightMeasureId == 0).ToList();
                        if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                        {
                            remove = true;
                        }
                    }

                }
                if (remove)
                {
                    loadingSlipTOList.Remove(tblLoadingSlipTOTemp);
                    q--;
                }

            }

            #endregion

            #region Check if invoice is already generated

            //Saket [2018-02-15] Added

            if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
            {
                //String loadingSlipIds = String.Join(',', loadingSlipTOList.Select(s => s.IdLoadingSlip.ToString()).ToArray());
                //if (!String.IsNullOrEmpty(loadingSlipIds))
                //{
                //    List<TblInvoiceTO> tblInvoiceTOListTemp = SelectInvoiceListFromLoadingSlipIds(loadingSlipIds, conn, tran);
                //    if (tblInvoiceTOListTemp != null && tblInvoiceTOListTemp.Count > 0)
                //    {
                //        for (int t = 0; t < tblInvoiceTOListTemp.Count; t++)
                //        {
                //            loadingSlipTOList = loadingSlipTOList.Where(w => w.IdLoadingSlip != tblInvoiceTOListTemp[t].LoadingSlipId).ToList();
                //        }
                //    }

                //}

                //List<TempLoadingSlipInvoiceTO> TempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOListByLoadingSlip()
                for (int r = 0; r < loadingSlipTOList.Count; r++)
                {
                    List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOListByLoadingSlip(loadingSlipTOList[r].IdLoadingSlip, conn, tran);
                    if (tempLoadingSlipInvoiceTOList != null && tempLoadingSlipInvoiceTOList.Count > 0)
                    {
                        loadingSlipTOList.RemoveAt(r);
                        r--;
                    }
                }

            }

            #endregion

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
            if (tblConfigParamsTO == null)
            {
                resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                return resultMsg;
            }
            Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            TblAddressTO ofcAddrTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
            if (ofcAddrTO == null)
            {
                resultMsg.DefaultBehaviour("Address Not Found For Self Organization.");
                return resultMsg;
            }
            /*GJ@20170927 : For get RCM and pass to Invoice*/
            TblConfigParamsTO rcmConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_REVERSE_CHARGE_MECHANISM, conn, tran);
            if (rcmConfigParamsTO == null)
            {
                resultMsg.DefaultBehaviour("RCM value Not Found in Configuration.");
                return resultMsg;
            }



            #region Prepare List Of Invoices To Save

            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();

            // Vaibhav [11-April-2018] Added to select invoice date configuration.
            TblConfigParamsTO invoiceDateConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TARE_WEIGHT_DATE_AS_INV_DATE, conn, tran);

            Int32 dontShowCdOnInvoice = 0;

            TblConfigParamsTO tblConfigParamsTOCdDoNotshow = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DO_NOT_SHOW_CD_ON_INOVICE);
            if (tblConfigParamsTOCdDoNotshow != null)
            {
                dontShowCdOnInvoice = Convert.ToInt32(tblConfigParamsTOCdDoNotshow.ConfigParamVal);
            }

            if (dontShowCdOnInvoice == 1)
            {
                loadingSlipTOList.ForEach(f => { f.CdStructure = 0; f.CdStructureId = 0; });
            }

            foreach (var loadingSlipTo in loadingSlipTOList)
            {
                TblInvoiceTO tblInvoiceTO = PrepareInvoiceAgainstLoadingSlip(loadingTO, conn, tran, internalOrgId, ofcAddrTO, rcmConfigParamsTO, invoiceDateConfigTO, loadingSlipTo, loadingSlipTo.DealerOrgId);
                Double tdsTaxPct = 0;
                if (tblInvoiceTO != null)
                {
                    if (tblInvoiceTO.IsTcsApplicable == 0)
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
                tblInvoiceTO.TdsAmt = 0;
                if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                {
                    tblInvoiceTO.TdsAmt = (CalculateTDS(tblInvoiceTO) * tdsTaxPct) / 100;
                    tblInvoiceTO.TdsAmt = Math.Ceiling(tblInvoiceTO.TdsAmt);
                }
                tblInvoiceTOList.Add(tblInvoiceTO);
            }

            #endregion


            #region Call To Save Invoice

            if (tblInvoiceTOList != null)
            {
                for (int i = 0; i < tblInvoiceTOList.Count; i++)
                {

                    resultMsg = SaveNewInvoice(tblInvoiceTOList[i], conn, tran);
                    if (resultMsg.MessageType != ResultMessageE.Information)
                    {
                        return resultMsg;
                    }
                }

                if (skipMergeSetting != 1)
                {
                    #region Merge Invoice

                    Int32 mergeInvoice = 0;

                    TblConfigParamsTO mergeInvoiceTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_AUTO_MERGE_INVOICE, conn, tran);
                    if (mergeInvoiceTO != null)
                    {
                        mergeInvoice = Convert.ToInt32(mergeInvoiceTO.ConfigParamVal);
                    }

                    Dictionary<String, List<Int32>> commonInvoice = new Dictionary<string, List<int>>();

                    if (mergeInvoice == 1)
                    {
                        for (int p = 0; p < tblInvoiceTOList.Count; p++)
                        {

                            TblInvoiceTO temp = tblInvoiceTOList[p];
                            String keyStr = temp.DealerOrgId + "***";

                            keyStr += temp.DistributorOrgId + "***";

                            keyStr += temp.IsConfirmed + "***";

                            if (temp.InvoiceAddressTOList != null && temp.InvoiceAddressTOList.Count > 0)
                            {
                                for (int q = 0; q < temp.InvoiceAddressTOList.Count; q++)
                                {
                                    TblInvoiceAddressTO tblInvoiceAddressTO = temp.InvoiceAddressTOList[q];

                                    if (tblInvoiceAddressTO.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                                    {
                                        keyStr += tblInvoiceAddressTO.StateId + "***";
                                        keyStr += tblInvoiceAddressTO.BillingOrgId + "***";
                                        keyStr += tblInvoiceAddressTO.PanNo + "***";
                                        keyStr += tblInvoiceAddressTO.AadharNo + "***";
                                        keyStr += tblInvoiceAddressTO.BillingName + "***";
                                        keyStr += tblInvoiceAddressTO.GstinNo + "***";
                                        keyStr += tblInvoiceAddressTO.Address + "***";
                                        keyStr += tblInvoiceAddressTO.Taluka + "***";
                                        keyStr += tblInvoiceAddressTO.District + "***";
                                    }

                                }
                            }

                            if (commonInvoice.ContainsKey(keyStr))
                            {
                                List<Int32> listI = commonInvoice[keyStr];
                                listI.Add(temp.IdInvoice);
                                commonInvoice[keyStr] = listI;
                            }
                            else
                            {
                                List<Int32> listI = new List<int>();
                                listI.Add(temp.IdInvoice);

                                commonInvoice.Add(keyStr, listI);
                            }
                        }

                        foreach (KeyValuePair<String, List<Int32>> temp in commonInvoice)
                        {
                            List<Int32> InvoiceIdList = temp.Value;

                            if (InvoiceIdList != null && InvoiceIdList.Count > 1)
                            {
                                resultMsg = ComposeInvoice(InvoiceIdList, loadingTO.CreatedBy, conn, tran);
                                if (resultMsg.MessageType != ResultMessageE.Information)
                                {
                                    return resultMsg;
                                }
                            }
                        }

                    }

                    #endregion
                }
            }

            #endregion
            resultMsg.Tag = tblInvoiceTOList;
            return resultMsg;
        }
        public Double CalculateTDS(TblInvoiceTO tblInvoiceTO)
        {
            Double TdsAmt = 0;
            List<TblOtherTaxesTO> TblOtherTaxesTOList = _iTblOtherTaxesDAO.SelectAllTblOtherTaxes();
            if (tblInvoiceTO != null)
            {
                if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                {
                    tblInvoiceTO.InvoiceItemDetailsTOList.ForEach(element =>
                    {
                        if (element.OtherTaxId == 0)
                        {
                            TdsAmt += element.TaxableAmt;
                        }
                        else if (element.OtherTaxId > 0 && TblOtherTaxesTOList != null && TblOtherTaxesTOList.Count > 0)
                        {
                            var matchTO = TblOtherTaxesTOList.Where(w => w.IdOtherTax == element.OtherTaxId).FirstOrDefault();
                            if (matchTO != null)
                            {
                                if (matchTO.IsBefore == 1)
                                {
                                    TdsAmt += element.TaxableAmt;
                                }
                            }
                        }
                    });
                }
            }
            return TdsAmt;
        }
        public TblInvoiceTO PrepareInvoiceAgainstLoadingSlip(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran, int internalOrgId, TblAddressTO ofcAddrTO, TblConfigParamsTO rcmConfigParamsTO, TblConfigParamsTO invoiceDateConfigTO, TblLoadingSlipTO loadingSlipTo, int InvoiceDealerOrgId)
        {
            /*GJ@20170915 :  Default Weighing is done in  KG UOM , hence we need to convert it MT while we assign this*/
            double conversionFactor = 0.001;

            Int32 billingStateId = 0;
            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            ResultMessage resultMsg = new ResultMessage();
            tblInvoiceTO.InvoiceAddressTOList = new List<TblInvoiceAddressTO>();
            tblInvoiceTO.InvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
            double grandTotal = 0;
            double discountTotal = 0;
            double igstTotal = 0;
            double cgstTotal = 0;
            double sgstTotal = 0;
            double basicTotal = 0;
            double basicTotalall = 0;
            double taxableTotal = 0;
            double taxableTotalall = 0;

            Double otherTaxAmt = 0;
            Boolean isSez = false;
            Int32 result = 0;
            Boolean isPanNoPresent = false;
            TblBookingsTO tblBookingsTO = new TblBookingsTO();
            List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

            #region 1 Preparing main InvoiceTO
            tblInvoiceTO.RcmFlag = Convert.ToInt32(rcmConfigParamsTO.ConfigParamVal);
            tblInvoiceTO.CurrencyId = loadingTO.CurrencyId;
            tblInvoiceTO.CurrencyRate = loadingTO.CurrencyRate;
            if (loadingTO.CurrencyId == 0 || loadingTO.CurrencyRate == 0)
            {
                tblInvoiceTO.CurrencyId = Constants.DefaultCurrencyID;
                tblInvoiceTO.CurrencyRate = Constants.DefaultCurrencyRate;
            }

            //It will be based on confirm not confirm. Hence commented and added at the end
            //tblInvoiceTO.FreightAmt = loadingTO.FreightAmt;
            tblInvoiceTO.VehicleNo = loadingTO.VehicleNo;
            // tblInvoiceTO.InvFromOrgId = loadingTO.FromOrgId;
            //Saket [2018-02-01] Added.
            //tblInvoiceTO.Narration = loadingTO.CnfOrgName;
            Int32 cnfNameInNarration = 0;

            TblConfigParamsTO temp = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ADD_CNF_AGENT_IN_INVOICE, conn, tran);
            if (temp == null)
            {
                resultMsg.DefaultBehaviour("Sales Engineer name in narration setting not found");
                ////return resultMsg;
            }

            cnfNameInNarration = Convert.ToInt32(temp.ConfigParamVal);

            if (cnfNameInNarration == 1)
                tblInvoiceTO.Narration = loadingTO.CnfOrgName;
            else
            {
                // tblInvoiceTO.Narration = String.Empty;Reshma Comment
                TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipDAO.SelectTblLoadingSlip(loadingSlipTo.IdLoadingSlip, conn, tran);
                if (tblLoadingSlipTO != null)
                {
                    tblInvoiceTO.Narration = tblLoadingSlipTO.Comment;
                }
            }


            //tblInvoiceTO.InvFromOrgId = internalOrgId; //No need to aasign from loading Only use for BMM
            tblInvoiceTO.InvFromOrgId = loadingTO.FromOrgId;  //For 
            tblInvoiceTO.CreatedOn = _iCommon.ServerDateTime;
            tblInvoiceTO.CreatedBy = loadingTO.CreatedBy;
            //tblInvoiceTO.DistributorOrgId = loadingTO.CnfOrgId;
            tblInvoiceTO.DistributorOrgId = loadingSlipTo.CnfOrgId;
            tblInvoiceTO.DealerOrgId = loadingSlipTo.DealerOrgId;

            //Aniket [06-02-2019]
            tblInvoiceTO.GrossWtTakenDate = _iCommon.ServerDateTime;
            tblInvoiceTO.PreparationDate = _iCommon.ServerDateTime;
            Boolean isRegular = true;
            TblLoadingSlipDtlTO tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(loadingSlipTo.IdLoadingSlip, conn, tran);
            if (tblLoadingSlipDtlTO != null)
            {
                tblBookingsTO = _iTblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);
                if (tblBookingsTO.IsSez == 1)
                {
                    isSez = true;
                }
                tblInvoiceTO.BrokerName = tblBookingsTO.BrokerName;
                tblInvoiceTO.BookingCommentCategoryId  = tblBookingsTO.BookingCommentCategoryId;
                tblInvoiceTO.BookingTaxCategoryId  = tblBookingsTO.BookingTaxCategoryId;


            }
            else
            {
                isRegular = false;

            }

            #region Priyanka [22-01-2019] : Added to save the commercial details against loadingslip

            // List<TblPaymentTermsForBookingTO> tblPaymentTermForBookingTOList = BL.TblPaymentTermsForBookingBL.SelectAllTblPaymentTermsForBookingFromBookingId(tblBookingsTO.IdBooking, 0);

            if (loadingSlipTo.IdLoadingSlip > 0)
            {
                // List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOList = new List<TblPaymentTermOptionRelationTO>();
                List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOList = _iTblPaymentTermOptionRelationDAO.SelectTblPaymentTermOptionRelationByLoadingId(loadingSlipTo.IdLoadingSlip);
                if (tblPaymentTermOptionRelationTOList != null && tblPaymentTermOptionRelationTOList.Count > 0)
                {
                    List<TblPaymentTermOptionRelationTO> tblPaymentTermOptionRelationTOListNew = new List<TblPaymentTermOptionRelationTO>();

                    for (int i = 0; i < tblPaymentTermOptionRelationTOList.Count; i++)
                    {
                        TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = tblPaymentTermOptionRelationTOList[i];
                        tblPaymentTermOptionRelationTO.CreatedBy = tblInvoiceTO.CreatedBy;
                        tblPaymentTermOptionRelationTO.CreatedOn = _iCommon.ServerDateTime;
                        tblPaymentTermOptionRelationTO.BookingId = 0;
                        //tblPaymentTermOptionRelationTO.InvoiceId = tblInvoiceTO.IdInvoice;
                        tblPaymentTermOptionRelationTOListNew.Add(tblPaymentTermOptionRelationTO);
                    }
                    tblInvoiceTO.PaymentTermOptionRelationTOLst = tblPaymentTermOptionRelationTOListNew;
                }
            }

            #endregion

            if (isSez)
            {
                tblInvoiceTO.InvoiceTypeE = Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY;
            }
            else
            {
                tblInvoiceTO.InvoiceTypeE = Constants.InvoiceTypeE.REGULAR_TAX_INVOICE;
            }
            if (invoiceDateConfigTO == null || invoiceDateConfigTO.ConfigParamVal == "0")
            {
                tblInvoiceTO.InvoiceDate = _iCommon.ServerDateTime;
            }
            else
            {
                List<TblWeighingMeasuresTO> tblWeighingMeasuresTOList = new List<TblWeighingMeasuresTO>();
                tblWeighingMeasuresTOList = _iTblWeighingMeasuresDAO.SelectAllTblWeighingMeasuresListByLoadingId(loadingTO.IdLoading, conn, tran);
                if (tblWeighingMeasuresTOList.Count > 0)
                {
                    tblWeighingMeasuresTOList.OrderByDescending(p => p.CreatedOn);
                }

                if (tblWeighingMeasuresTOList != null && tblWeighingMeasuresTOList.Count > 0)
                {
                    TblWeighingMeasuresTO tblWeighingMeasuresTO = tblWeighingMeasuresTOList.FindAll(ele => ele.WeightMeasurTypeId == (int)Constants.TransMeasureTypeE.TARE_WEIGHT).FirstOrDefault();
                    if (tblWeighingMeasuresTO != null)
                    {
                        tblInvoiceTO.InvoiceDate = tblWeighingMeasuresTO.CreatedOn;
                    }
                    else
                    {
                        resultMsg.DefaultBehaviour("tblWeighingMeasuresTO is null");
                        ////return resultMsg;
                    }
                }
                else
                {
                    resultMsg.DefaultBehaviour("tblWeighingMeasuresTOList is null");
                    ////return resultMsg;
                }
            }

            tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE;
            tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
            tblInvoiceTO.LoadingSlipId = loadingSlipTo.IdLoadingSlip;
            tblInvoiceTO.StatusDate = tblInvoiceTO.InvoiceDate;
            tblInvoiceTO.TransportOrgId = loadingTO.TransporterOrgId;
            //tblInvoiceTO.InvoiceTypeE = Constants.InvoiceTypeE.REGULAR_TAX_INVOICE;
            tblInvoiceTO.IsConfirmed = loadingSlipTo.IsConfirmed;
            if (loadingTO.CallFlag == 1)
                tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE_EDIT;

            //Auto Invoice edit facility setting
            TblConfigParamsTO tblConfigParamsTOAutoEditYn = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EVERY_AUTO_INVOICE_WITH_EDIT, conn, tran);
            if (tblConfigParamsTOAutoEditYn != null)
            {
                if (tblConfigParamsTOAutoEditYn.ConfigParamVal == "1")
                    tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE_EDIT;
            }


            tblInvoiceTO.PoNo = loadingSlipTo.PoNo;
            tblInvoiceTO.PoDate = loadingSlipTo.PoDate;
            #endregion

            #region 2 Added Invoice Address Details
            if (loadingSlipTo.DeliveryAddressTOList != null && loadingSlipTo.DeliveryAddressTOList.Count > 0)
            {
                foreach (var deliveryAddrTo in loadingSlipTo.DeliveryAddressTOList)
                {
                    TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
                    tblInvoiceAddressTo.AadharNo = deliveryAddrTo.AadharNo;
                    tblInvoiceAddressTo.GstinNo = deliveryAddrTo.GstNo;
                    tblInvoiceAddressTo.PanNo = deliveryAddrTo.PanNo;
                    tblInvoiceAddressTo.StateId = deliveryAddrTo.StateId;
                    tblInvoiceAddressTo.State = deliveryAddrTo.State;
                    tblInvoiceAddressTo.Taluka = deliveryAddrTo.TalukaName;
                    tblInvoiceAddressTo.VillageName = deliveryAddrTo.VillageName;
                    tblInvoiceAddressTo.District = deliveryAddrTo.DistrictName;
                    tblInvoiceAddressTo.BillingName = deliveryAddrTo.BillingName;
                    tblInvoiceAddressTo.ContactNo = deliveryAddrTo.ContactNo;
                    tblInvoiceAddressTo.PinCode = deliveryAddrTo.Pincode;
                    tblInvoiceAddressTo.TxnAddrTypeId = deliveryAddrTo.TxnAddrTypeId;
                    tblInvoiceAddressTo.Address = deliveryAddrTo.Address;
                    tblInvoiceAddressTo.AddrSourceTypeId = deliveryAddrTo.AddrSourceTypeId;
                    if (deliveryAddrTo.AddrSourceTypeId == (int)Constants.AddressSourceTypeE.FROM_CNF)
                    {
                        tblInvoiceAddressTo.BillingOrgId = loadingTO.CnfOrgId;
                    }

                    //Priyanka [16-04-2018]
                    else if (deliveryAddrTo.AddrSourceTypeId == (int)Constants.AddressSourceTypeE.NEW_ADDRESS)
                    {
                        tblInvoiceAddressTo.BillingOrgId = deliveryAddrTo.BillingOrgId;
                    }
                    else
                    {
                        tblInvoiceAddressTo.BillingOrgId = loadingSlipTo.DealerOrgId;
                    }
                    //Aniket [18-9-2019]
                    int showDeliveryLocation = 0;
                    TblConfigParamsTO tblConfigParamTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.SHOW_DELIVERY_LOCATION_ON_INVOICE);
                    if (tblConfigParamTO != null)
                    {
                        showDeliveryLocation = Convert.ToInt32(tblConfigParamTO.ConfigParamVal);
                    }

                    if (deliveryAddrTo.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                    {
                        billingStateId = deliveryAddrTo.StateId;
                        if (showDeliveryLocation == 1)
                        {
                            if (string.IsNullOrEmpty(deliveryAddrTo.VillageName))
                            {
                                if (string.IsNullOrEmpty(deliveryAddrTo.TalukaName))
                                {
                                    tblInvoiceTO.DeliveryLocation = deliveryAddrTo.DistrictName;
                                }
                                tblInvoiceTO.DeliveryLocation = deliveryAddrTo.TalukaName;
                            }
                            else
                                tblInvoiceTO.DeliveryLocation = deliveryAddrTo.VillageName;
                        }

                        //Harshala
                        isPanNoPresent = IsPanOrGstPresent(deliveryAddrTo.PanNo, deliveryAddrTo.GstNo);
                        //
                    }


                    tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);
                }
            }

            if (billingStateId == 0)
            {
                resultMsg.DefaultBehaviour("Billing State Not Found");
                ////return resultMsg;
            }
            #endregion

            #region 3 Added Invoice Item details

            Double totalInvQty = 0;
            Double totalNCExpAmt = 0;
            Double totalNCOtherAmt = 0;
            Int32 isForItemWiseRoundup = 2;
            //chetan[2020 - june - 08] added
            TblConfigParamsTO cPisForItemWiseRoundup = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.ITEM_GRAND_TOTAL_ROUNDUP_VALUE, conn, tran);
            if (cPisForItemWiseRoundup != null)
            {
                isForItemWiseRoundup = Convert.ToInt32(cPisForItemWiseRoundup.ConfigParamVal);
            }
            #region GJ@20170922 : Find the Minium Weight from LoadingSlipExtTo to Know Tare wt for that Loading Slip
            if (loadingSlipTo.LoadingSlipExtTOList != null && loadingSlipTo.LoadingSlipExtTOList.Count > 0)
            {
                var minCalcTareWt = loadingSlipTo.LoadingSlipExtTOList.Aggregate((curMin, x) => (curMin == null || (x.CalcTareWeight) < curMin.CalcTareWeight ? x : curMin)).CalcTareWeight;
                tblInvoiceTO.TareWeight = minCalcTareWt;
                tblInvoiceTO.GrossWeight = minCalcTareWt;
            }
            #endregion
            #region Regular Invoice Items i.e from Loading
            foreach (var loadingSlipExtTo in loadingSlipTo.LoadingSlipExtTOList)
            {
                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = new TblInvoiceItemDetailsTO();
                List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                TblProductItemTO tblProductItemTO = new TblProductItemTO();
                Double itemGrandTotal = 0;

                //Saket [2018-02-23] Added by default 0 brand id
                if (tblInvoiceItemDetailsTOList != null && tblInvoiceItemDetailsTOList.Count == 0)
                {
                    tblInvoiceTO.BrandId = loadingSlipExtTo.BrandId;
                }


                //if (loadingTO.LoadingType == (int)Constants.LoadingTypeE.OTHER)
                if (!isRegular)
                {
                    tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdItemDesc;
                    tblInvoiceItemDetailsTO.CdStructure = loadingSlipExtTo.CdStructure;
                    tblInvoiceItemDetailsTO.CdStructureId = loadingSlipExtTo.CdStructureId; //Saket [2018-02-06] Added.
                    tblInvoiceItemDetailsTO.Rate = loadingSlipExtTo.RatePerMT;

                }
                else
                {
                    tblInvoiceItemDetailsTO.LoadingSlipExtId = loadingSlipExtTo.IdLoadingSlipExt;
                    tblInvoiceItemDetailsTO.CdStructure = loadingSlipTo.CdStructure;
                    tblInvoiceItemDetailsTO.CdStructureId = loadingSlipTo.CdStructureId; //Saket [2018-02-06] 
                    tblInvoiceItemDetailsTO.Rate = loadingSlipExtTo.CdApplicableAmt;
                    if (tblBookingsTO.BookingTaxCategoryId == (int)Constants.BookingTaxCategory.Excluding)
                        tblInvoiceItemDetailsTO.Rate = loadingSlipExtTo.TaxableRateMT;
                      //[05-03-2018]Vijaymala:Changes the code to change prodItemDesc as per Kalika and SRJ requirement 
                      //cnetan[20-feb-2020] added for display brand name in report on setting base.
                    Int32 isCPDisplayBrandOnInvoice = 0;
                    Int32 isHideBrandNameOnNC = 0;
                    TblConfigParamsTO cPDisplayBrandOnInvoiceTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DISPLAY_BRAND_ON_INVOICE, conn, tran);
                    if (cPDisplayBrandOnInvoiceTO != null)
                    {
                        isCPDisplayBrandOnInvoice = Convert.ToInt32(cPDisplayBrandOnInvoiceTO.ConfigParamVal);
                    }
                    //Aniket [18-9-2019] commented the code
                    TblConfigParamsTO hideBrandNameOnNCInvoiceTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.HIDE_BRAND_NAME_ON_NC_INVOICE, conn, tran);
                    if (hideBrandNameOnNCInvoiceTO != null)
                    {
                        isHideBrandNameOnNC = Convert.ToInt32(hideBrandNameOnNCInvoiceTO.ConfigParamVal);
                    }
                    //[05-09-2018] : Vijaymala added to set product item display for other booking in invoice
                    if (loadingSlipExtTo.ProdItemId == 0)
                    {
                        if (isCPDisplayBrandOnInvoice == 1)
                        {
                            if (tblInvoiceTO.IsConfirmed == 0 && isHideBrandNameOnNC == 1)
                            {
                                tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + " " + loadingSlipExtTo.MaterialDesc;
                            }
                            else
                            {
                                tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.BrandDesc + " " + loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + " " + loadingSlipExtTo.MaterialDesc;
                            }
                        }
                        else
                        {
                            tblInvoiceItemDetailsTO.ProdItemDesc = tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + " " + loadingSlipExtTo.MaterialDesc;
                        }
                        // tblInvoiceItemDetailsTO.ProdItemDesc=loadingSlipExtTo.BrandDesc + " " + loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + " " + loadingSlipExtTo.MaterialDesc;
                    }
                    else
                    {
                        //tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.DisplayName; // commented by Aniket As we need to display only ItemName
                        //tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdItemDesc; // Aniket [4-02-2019]
                        tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ItemName;
                        //tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + " " + loadingSlipExtTo.MaterialDesc;
                    }

                    tblInvoiceItemDetailsTO.BrandId = loadingSlipExtTo.BrandId;

                    if (loadingSlipTo.IsConfirmed == 0)
                    {
                        TblParitySummaryTO parityTO = _iTblParitySummaryDAO.SelectParitySummaryFromParityDtlId(loadingSlipExtTo.ParityDtlId, conn, tran);
                        if (parityTO != null)
                        {
                            totalNCExpAmt += parityTO.ExpenseAmt * Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor, 2);
                            totalNCOtherAmt += parityTO.OtherAmt * Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor, 2);
                        }
                    }
                }
                if (loadingSlipExtTo.ProdItemId > 0)
                {
                    tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(loadingSlipExtTo.ProdItemId, conn, tran);
                    if (tblProductItemTO == null)
                    {
                        resultMsg.DefaultBehaviour("Product conversion Factor Found Null againest the Product Item :  " + loadingSlipExtTo.ProdItemId + ".");
                        ////return resultMsg;
                    }
                    //Commented by Aniket [12-6-2019]
                    //conversionFactor = tblProductItemTO.ConversionFactor;
                }
                if (loadingSlipExtTo.MaterialId > 0)
                {
                    SizeTestingDtlTO SizeTestingDtlTO = _iTblParitySummaryDAO.SelectTestCertificateDdtlofMaterial(loadingSlipExtTo.MaterialId, conn, tran);
                    if (SizeTestingDtlTO != null)
                    {
                        tblInvoiceItemDetailsTO.SizeTestingDtlId = SizeTestingDtlTO.IdTestDtl;
                    }
                }
                tblInvoiceTO.NetWeight += loadingSlipExtTo.LoadedWeight;
                tblInvoiceTO.GrossWeight += loadingSlipExtTo.LoadedWeight;
                tblInvoiceItemDetailsTO.InvoiceQty = Math.Round(loadingSlipExtTo.LoadedWeight * conversionFactor, 3);
                totalInvQty += tblInvoiceItemDetailsTO.InvoiceQty;
                //tblInvoiceItemDetailsTO.CdStructureId = loadingSlipTo.CdStructureId; //Saket [2018-02-06] Commented and assgin above

                //Priyanka [29-05-2018] : for adding master bundles while weighing.
                //if (loadingSlipExtTo.MstLoadedBundles == 0)
                //{
                //    tblInvoiceItemDetailsTO.Bundles =  loadingSlipExtTo.LoadedBundles.ToString();
                //}
                //else if(loadingSlipExtTo.LoadedBundles == 0 )
                //{
                //    tblInvoiceItemDetailsTO.Bundles = loadingSlipExtTo.MstLoadedBundles.ToString();
                //}
                //else
                //{
                //    tblInvoiceItemDetailsTO.Bundles = loadingSlipExtTo.MstLoadedBundles.ToString() + ',' + loadingSlipExtTo.LoadedBundles.ToString();
                //}

                if (loadingSlipExtTo.MstLoadedBundles > 0)
                    tblInvoiceItemDetailsTO.Bundles += loadingSlipExtTo.MstLoadedBundles.ToString() + ',';
                if (loadingSlipExtTo.LoadedBundles > 0)
                    tblInvoiceItemDetailsTO.Bundles += loadingSlipExtTo.LoadedBundles.ToString() + ',';

                if (String.IsNullOrEmpty(tblInvoiceItemDetailsTO.Bundles))
                {
                    tblInvoiceItemDetailsTO.Bundles = String.Empty;
                }
                tblInvoiceItemDetailsTO.Bundles = tblInvoiceItemDetailsTO.Bundles.TrimEnd(',');
                //chean[2020-june-08]
                tblInvoiceItemDetailsTO.BasicTotal = Math.Round((loadingSlipExtTo.LoadedWeight * conversionFactor * tblInvoiceItemDetailsTO.Rate), isForItemWiseRoundup);

                basicTotal += tblInvoiceItemDetailsTO.BasicTotal;
                //basicTotalall = basicTotal;
                //Vijaymala added[22-06-2018]
                DropDownTO dropDownTO = _iDimensionBL.SelectCDDropDown(tblInvoiceItemDetailsTO.CdStructureId);
                if (tblInvoiceItemDetailsTO.CdStructure >= 0)
                {
                    Int32 isRsValue = Convert.ToInt32(dropDownTO.Text);

                    if (isRsValue == (int)Constants.CdType.IsRs)
                    {
                        tblInvoiceItemDetailsTO.CdAmt = (tblInvoiceItemDetailsTO.CdStructure * loadingSlipExtTo.LoadedWeight * conversionFactor);
                    }
                    else
                    {
                        tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                    }
                    //Priyanka [10-07-2018] : Added for additional discount SHIVANGI.
                    tblInvoiceItemDetailsTO.CdAmt += (loadingSlipTo.AddDiscAmt * loadingSlipExtTo.LoadedWeight * conversionFactor);
                }
                else
                {
                    tblInvoiceItemDetailsTO.CdAmt = 0;
                }

                //Vijaymala commented that code [22-06-2018]
                //if (tblInvoiceItemDetailsTO.CdStructure > 0)
                //{
                //    tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                //}
                //else
                //{
                //    tblInvoiceItemDetailsTO.CdAmt = 0;
                //}
                //Add By Samadhan 18 May 2022
                string isRoundOffCD = "";
                TblConfigParamsTO ConfigParamsData = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_CD_ON_Rate_Calculation_Details);
                if (ConfigParamsData != null && !String.IsNullOrEmpty(ConfigParamsData.ConfigParamVal))
                {
                    isRoundOffCD = ConfigParamsData.ConfigParamVal;
                }
                if (isRoundOffCD != "")
                {
                    discountTotal += Math.Round(tblInvoiceItemDetailsTO.CdAmt, Convert.ToInt32(isRoundOffCD));
                }
                else
                {
                    discountTotal += tblInvoiceItemDetailsTO.CdAmt;
                }
                Double taxbleAmt = 0;
                //Double totalFreExpOtherAmt = loadingSlipExtTo.LoadedWeight * conversionFactor * loadingSlipExtTo.FreExpOtherAmt;

                if (loadingSlipTo.IsConfirmed == 1)
                    taxbleAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;// + totalFreExpOtherAmt;
                else
                    taxbleAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;

                tblInvoiceItemDetailsTO.TaxableAmt = Math.Round(taxbleAmt, isForItemWiseRoundup);
                itemGrandTotal += taxbleAmt;
                taxableTotal += tblInvoiceItemDetailsTO.TaxableAmt;
                tblProdGstCodeDtlsTO = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(loadingSlipExtTo.ProdCatId, loadingSlipExtTo.ProdSpecId, loadingSlipExtTo.MaterialId, loadingSlipExtTo.ProdItemId, 0, conn, tran);
                if (tblProdGstCodeDtlsTO == null)
                {
                    resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against loadingSlipExtId is : " + loadingSlipExtTo.IdLoadingSlipExt + ".");
                    resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                    //return resultMsg;
                }
                tblInvoiceItemDetailsTO.ProdGstCodeId = tblProdGstCodeDtlsTO.IdProdGstCode;
                TblGstCodeDtlsTO gstCodeDtlsTO = _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                if (gstCodeDtlsTO != null)
                {
                    gstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesDAO.SelectAllTblTaxRates(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                }
                if (gstCodeDtlsTO == null)
                {
                    resultMsg.DefaultBehaviour("GST code details found null againest loadingSlipExtId is : " + loadingSlipExtTo.IdLoadingSlipExt + ".");
                    resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                    ////return resultMsg;
                }

                tblInvoiceItemDetailsTO.GstinCodeNo = gstCodeDtlsTO.CodeNumber;

                double grandtotal = 0;
                double invoiceAmt = 0;
                if (tblBookingsTO.BookingTaxCategoryId == (int)Constants.BookingTaxCategory.Excluding)
                {
                   //  grandtotal = tblInvoiceItemDetailsTO.TaxableAmt;
                   //  invoiceAmt = (grandtotal * gstCodeDtlsTO.TaxPct) / 100;

                   // tblInvoiceItemDetailsTO.TaxableAmt = grandtotal - invoiceAmt;
                   // tblInvoiceItemDetailsTO.BasicTotal = tblInvoiceItemDetailsTO.TaxableAmt;
                   // tblInvoiceItemDetailsTO.Rate = tblInvoiceItemDetailsTO.BasicTotal / tblInvoiceItemDetailsTO.InvoiceQty;

                   // tblInvoiceItemDetailsTO.Rate = loadingSlipExtTo.TaxableRateMT;
                   // tblInvoiceItemDetailsTO.TaxableAmt = tblInvoiceItemDetailsTO.Rate * tblInvoiceItemDetailsTO.InvoiceQty;
                   // tblInvoiceItemDetailsTO.BasicTotal = tblInvoiceItemDetailsTO.TaxableAmt;
                   //taxableTotalall += tblInvoiceItemDetailsTO.BasicTotal;
                   // basicTotalall += tblInvoiceItemDetailsTO.BasicTotal;
                }

                #region 4 Added Invoice Item Tax details

                foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
                {
                   
                    TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
                    tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
                    tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
                    tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                    //if (tblBookingsTO.BookingTaxCategoryId == (int)Constants.BookingTaxCategory.Excluding)
                    //{
                    //     grandtotal = tblInvoiceItemDetailsTO.TaxableAmt;
                    //     invoiceAmt = (grandtotal * gstCodeDtlsTO.TaxPct)  / 100;
                    //    tblInvoiceItemTaxDtlsTO.TaxableAmt = grandtotal - invoiceAmt;
                    //    //tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;

                    //}
                    //else
                        tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                    if (isSez)
                    {
                        tblInvoiceItemTaxDtlsTO.TaxRatePct = 0;
                        tblInvoiceItemTaxDtlsTO.TaxableAmt = 0;
                    }
                    //if (tblBookingsTO.BookingTaxCategoryId == (int)Constants.BookingTaxCategory.Excluding)
                    //    tblInvoiceItemTaxDtlsTO.TaxAmt = (grandtotal * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
                    //else
                        tblInvoiceItemTaxDtlsTO.TaxAmt = ((tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100);
                    tblInvoiceItemTaxDtlsTO.TaxTypeId = taxRateTo.TaxTypeId;
                    if (billingStateId == ofcAddrTO.StateId)
                    {
                        if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                        {
                            cgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                            //if (tblBookingsTO.BookingTaxCategoryId != (int)Constants.BookingTaxCategory.Excluding)
                                itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                            tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                        }
                        else if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                        {
                            sgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                           // if (tblBookingsTO.BookingTaxCategoryId != (int)Constants.BookingTaxCategory.Excluding)
                                itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                            tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                        }
                        else continue;
                    }
                    else
                    {
                        if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                        {
                            igstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                            //if (tblBookingsTO.BookingTaxCategoryId != (int)Constants.BookingTaxCategory.Excluding)
                                itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                            tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                        }
                        else continue;
                    }
                }
                #endregion

                //if (tblBookingsTO.BookingTaxCategoryId != (int)Constants.BookingTaxCategory.Excluding)
                //    grandTotal += itemGrandTotal;
                //else
                //    grandTotal = itemGrandTotal;

                grandTotal += itemGrandTotal;

                tblInvoiceItemDetailsTO.GrandTotal = Math.Round(itemGrandTotal, isForItemWiseRoundup);
                tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
                tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
            }
            if (tblInvoiceItemDetailsTOList != null && tblInvoiceItemDetailsTOList.Count > 0)
            {
                List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOListTemp  = tblInvoiceItemDetailsTOList.Where(w => w.SizeTestingDtlId > 0).ToList();
                if (tblInvoiceItemDetailsTOListTemp != null && tblInvoiceItemDetailsTOListTemp.Count > 0)
                {
                    tblInvoiceTO.IsTestCertificate = 1;
                }
                
            }
            if (basicTotalall > 0 && taxableTotalall > 0)
            {
                basicTotal = basicTotalall;
                taxableTotal = taxableTotalall;
            }

            #endregion

            #region Freight , Expenses or Other Charges if Applicable while loading

            if (loadingSlipTo.IsConfirmed == 1)
            {
                //Vijaymala added[27-04-2018]:commented that code to get freight from loading slip layerwise
                //if (loadingTO.IsFreightIncluded == 1 && loadingTO.FreightAmt > 0)
                //{
                //Vijaymala added[22-06-2018]
                Double freightPerMT = 0;
                if (loadingSlipTo.IsFreightIncluded == 1 && loadingSlipTo.FreightAmt > 0)
                {
                    freightPerMT = loadingSlipTo.FreightAmt;

                    TblConfigParamsTO otherFreighConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_FRIEGHT_OTHER_TAX_ID, conn, tran);
                    if (otherFreighConfigParamsTO == null)
                    {
                        resultMsg.DefaultBehaviour("Other Tax id is not configured for freight");
                        ////return resultMsg;
                    }
                    Int32 freightOtherTaxId = Convert.ToInt32(otherFreighConfigParamsTO.ConfigParamVal);
                    Double freightGrandTotal = 0;
                    TblInvoiceItemDetailsTO freightItemTO = new TblInvoiceItemDetailsTO();
                    freightItemTO.OtherTaxId = freightOtherTaxId;
                    freightItemTO.InvoiceQty = totalInvQty;

                    Double forAmtPerMT = 0;
                    //Vijaymala added[21-06-2018]for new For amount calculation
                    if (loadingSlipTo.IsForAmountIncluded == 1)
                    {

                        if (loadingSlipTo.ForAmount > 0)
                        {
                            forAmtPerMT = loadingSlipTo.ForAmount;
                            freightPerMT = forAmtPerMT - freightPerMT;
                        }
                        //if (tblLoadingSlipTO.ForAmount < 0)
                        //{
                        //    tran.Rollback();
                        //    resultMessage.MessageType = ResultMessageE.Error;
                        //    resultMessage.Text = "Error : For Amount Calculations is less than 0. Please check the calculations immediatly";
                        //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        //    return resultMessage;
                        //}

                    }

                    //Priyanka [11-07-2018] : Added for freight.
                    // freightPerMT = Math.Abs(freightPerMT);

                    freightItemTO.Rate = freightPerMT;//loadingSlipTo.FreightAmt;//loadingTO.FreightAmt; //Vijaymala added[27 - 04 - 2018]:commented that code to get freight from loading slip layerwise
                    freightItemTO.TaxableAmt = freightItemTO.BasicTotal = freightPerMT * totalInvQty; //loadingSlipTo.FreightAmt * totalInvQty; //loadingTO.FreightAmt * totalInvQty;
                    freightGrandTotal += freightItemTO.TaxableAmt;
                    var maxTaxableItemTO = tblInvoiceItemDetailsTOList.OrderByDescending(m => m.TaxableAmt).FirstOrDefault();

                    freightItemTO.ProdGstCodeId = maxTaxableItemTO.ProdGstCodeId;
                    freightItemTO.ProdItemDesc = "Freight Charges";
                    freightItemTO.GstinCodeNo = maxTaxableItemTO.GstinCodeNo;

                    basicTotal += freightItemTO.BasicTotal;
                    taxableTotal += freightItemTO.TaxableAmt;

                    if (maxTaxableItemTO.InvoiceItemTaxDtlsTOList != null && maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                    {
                        for (int ti = 0; ti < maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count; ti++)
                        {
                            TblInvoiceItemTaxDtlsTO taxDtlTO = maxTaxableItemTO.InvoiceItemTaxDtlsTOList[ti].DeepCopy();
                            taxDtlTO.TaxableAmt = freightItemTO.TaxableAmt;
                            if (isSez)
                            {
                                taxDtlTO.TaxRatePct = 0;
                                taxDtlTO.TaxableAmt = 0;
                            }
                            taxDtlTO.TaxAmt = (taxDtlTO.TaxableAmt * taxDtlTO.TaxRatePct) / 100;
                            freightGrandTotal += taxDtlTO.TaxAmt;

                            if (taxDtlTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                            {
                                cgstTotal += taxDtlTO.TaxAmt;
                            }
                            if (taxDtlTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                            {
                                sgstTotal += taxDtlTO.TaxAmt;
                            }
                            if (taxDtlTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                            {
                                igstTotal += taxDtlTO.TaxAmt;
                            }

                            if (freightItemTO.InvoiceItemTaxDtlsTOList == null)
                                freightItemTO.InvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                            freightItemTO.InvoiceItemTaxDtlsTOList.Add(taxDtlTO);
                        }
                    }

                    freightItemTO.GrandTotal = freightGrandTotal;

                    tblInvoiceItemDetailsTOList.Add(freightItemTO);
                    grandTotal += freightGrandTotal;
                }

                Int32 BillingOrgId = InvoiceDealerOrgId;
                if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                {
                    var matchTO = tblInvoiceTO.InvoiceAddressTOList.Where(w => w.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                    if (matchTO != null && matchTO.BillingOrgId > 0)
                    {
                        BillingOrgId = matchTO.BillingOrgId;
                    }
                }
                TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(BillingOrgId);
                if (tblOrganizationTO == null)
                {
                    resultMsg.DefaultBehaviour("Failed to get dealer org details");
                }
                if (tblOrganizationTO != null && tblOrganizationTO.IsTcsApplicable == 1)
                {
                    tblInvoiceTO.IsTcsApplicable = tblOrganizationTO.IsTcsApplicable;
                    resultMsg = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal, ref taxableTotal, ref basicTotal, isPanNoPresent, tblInvoiceItemDetailsTOList, tblInvoiceTO, ref otherTaxAmt, tblOrganizationTO.IsDeclarationRec);
                    if (resultMsg == null || resultMsg.MessageType != ResultMessageE.Information)
                    {
                        resultMsg.DefaultBehaviour(resultMsg.Text);
                        //return resultMsg;
                    }
                }
                TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_IS_INCLUDE_Loading_Charges_TO_AUTO_INVOICE, conn, tran);
                if (configParamsTO != null)
                {
                    resultMsg = AddLoadingChargesTOInTaxItemDtls(conn, tran, ref grandTotal, ref taxableTotal, ref basicTotal, isPanNoPresent, tblInvoiceItemDetailsTOList, tblInvoiceTO, ref otherTaxAmt, tblOrganizationTO.IsDeclarationRec);
                    if (resultMsg == null || resultMsg.MessageType != ResultMessageE.Information)
                    {
                        resultMsg.DefaultBehaviour(resultMsg.Text);
                        //return resultMsg;
                    }
                    tblInvoiceTO.GrandTotal = grandTotal;
                    tblInvoiceTO.TaxableAmt = taxableTotal;
                    tblInvoiceTO.BasicAmt = basicTotal;
                }
            }
            else
            {
                tblInvoiceTO.ExpenseAmt = totalNCExpAmt;
                tblInvoiceTO.OtherAmt = totalNCOtherAmt;

                if (loadingSlipTo.IsFreightIncluded == 1)//if (loadingTO.IsFreightIncluded == 1)
                    tblInvoiceTO.FreightAmt = totalInvQty * loadingSlipTo.FreightAmt;//loadingTO.FreightAmt;
                                                                                     //Priyanka [20-07-2018] : Added for SHIVANGI.
                                                                                     //Sanjay [218-07-04] Tax Calculations Inclusive Of Taxes Or Exclusive Of Taxes. Reported From Customer Shivangi Rolling Mills.By default it will be 0 i.e. Tax Exclusive
                Int32 isToIncludeFreight = 0;
                TblConfigParamsTO freightCalcConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DISPLAY_FREIGHT_ON_INVOICE, conn, tran);
                if (freightCalcConfigParamsTO != null)
                {
                    isToIncludeFreight = Convert.ToInt32(freightCalcConfigParamsTO.ConfigParamVal);
                }

                if (isToIncludeFreight == 1)
                    grandTotal += totalNCExpAmt + totalNCOtherAmt;
                else
                    grandTotal += totalNCExpAmt + totalNCOtherAmt + tblInvoiceTO.FreightAmt;

            }
            #endregion

            #endregion

            #region 5 Save main Invoice
            //int roundOffValue = 2;
            ////Aniket [16-9-2-19] added to round of the invoice values
            //
            //TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.ROUND_OFF_TAX_INVOICE_VALUES);
            //if (tblConfigParamsTO != null)
            //{
            //    roundOffValue = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            //}
            ////if (roundOffValue > 0) Saket [2019-12-05] not needed these condition 
            //if(true)
            //{
            //    taxableTotal = Math.Round(taxableTotal, roundOffValue);
            //    discountTotal = Math.Round(discountTotal, roundOffValue);
            //    igstTotal = Math.Round(igstTotal, roundOffValue);
            //    cgstTotal = Math.Round(cgstTotal, roundOffValue);
            //    sgstTotal = Math.Round(sgstTotal, roundOffValue);
            //    basicTotal = Math.Round(basicTotal, roundOffValue);
            //}

            tblInvoiceTO.TaxableAmt = taxableTotal;
            tblInvoiceTO.DiscountAmt = discountTotal;
            tblInvoiceTO.IgstAmt = igstTotal;
            tblInvoiceTO.CgstAmt = cgstTotal;
            tblInvoiceTO.SgstAmt = sgstTotal;
            tblInvoiceTO.BasicAmt = basicTotal;
            tblInvoiceTO.OtherTaxAmt = otherTaxAmt;
            RoundOffInvoiceValuesBySetting(tblInvoiceTO);

            //double finalGrandTotal = Math.Round(grandTotal);
            //tblInvoiceTO.GrandTotal = finalGrandTotal;
            //tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);



            tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                RemoveIotFieldsFromDB(tblInvoiceTO);
            }
            return tblInvoiceTO;

            #endregion
        }

        public Boolean IsPanOrGstPresent(String panNo, String gstNo)
        {
            if ((!String.IsNullOrEmpty(panNo) && panNo != "") || (!String.IsNullOrEmpty(gstNo) && gstNo != ""))
            {

                return true;
            }
            else
                return false;
        }

        //Harshala[30/09/3030] added to calculate TCS 
        private ResultMessage AddTcsTOInTaxItemDtls(SqlConnection conn, SqlTransaction tran, ref double grandTotal, ref double taxableTotal, ref double basicTotal, bool isPanNoPresent, List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList, TblInvoiceTO tblInvoiceTo, ref double otherTaxAmt, Int32 isDeclarationRec)
        {

            ResultMessage resultMessage = new ResultMessage();

            if (tblInvoiceTo.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
            {
                TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_IS_INCLUDE_TCS_TO_AUTO_INVOICE, conn, tran);

                if (configParamsTO != null)
                {
                    if (configParamsTO.ConfigParamVal == "1")
                    {

                        TblConfigParamsTO tcsConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);

                        if (tcsConfigParamsTO == null)
                        {
                            resultMessage.DefaultBehaviour("Other Tax id is not configured for freight");
                            return resultMessage;
                        }


                        Int32 isForItemWiseRoundup = 2;
                        TblConfigParamsTO cPisForItemWiseRoundup = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.ITEM_GRAND_TOTAL_ROUNDUP_VALUE, conn, tran);
                        if (cPisForItemWiseRoundup != null)
                        {
                            isForItemWiseRoundup = Convert.ToInt32(cPisForItemWiseRoundup.ConfigParamVal);
                        }


                        TblConfigParamsTO tcsPercentConfigParamsTO = new TblConfigParamsTO();
                        Int32 IsCheckDeclarationRec = 0;
                        TblConfigParamsTO declarationRecConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CHECK_IS_DECLARATION_RECEIVED, conn, tran);
                        if (declarationRecConfigParamsTO != null)
                        {
                            if (!String.IsNullOrEmpty(declarationRecConfigParamsTO.ConfigParamVal))
                            {
                                IsCheckDeclarationRec = Convert.ToInt32(declarationRecConfigParamsTO.ConfigParamVal);
                            }
                        }
                        if (IsCheckDeclarationRec == 1)
                        {
                            if (isDeclarationRec == 1)
                            {
                                if (isPanNoPresent)
                                    tcsPercentConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_PAN_PRESENT, conn, tran);
                                else
                                    tcsPercentConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT, conn, tran);
                            }
                            else
                            {
                                tcsPercentConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_DECLARATION_NOT_RECEIVED, conn, tran);
                            }
                        }
                        else
                        {
                            if (isPanNoPresent)
                                tcsPercentConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_PAN_PRESENT, conn, tran);
                            else
                                tcsPercentConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.DEFAULT_TCS_PERCENT_IF_PAN_NOT_PRESENT, conn, tran);
                        }



                        if (tcsPercentConfigParamsTO != null)
                        {
                            if (tcsPercentConfigParamsTO.ConfigParamVal != "0" && tcsPercentConfigParamsTO.ConfigParamVal != null)
                            {
                                Int32 tcsOtherTaxId = Convert.ToInt32(tcsConfigParamsTO.ConfigParamVal);
                                Double tcsGrandTotal = 0;
                                TblInvoiceItemDetailsTO tcsItemTO = new TblInvoiceItemDetailsTO();
                                tcsItemTO.OtherTaxId = tcsOtherTaxId;
                                TblOtherTaxesTO otherTaxesTO = _iTblOtherTaxesDAO.SelectTblOtherTaxes(tcsOtherTaxId);
                                if (otherTaxesTO != null)
                                    tcsItemTO.ProdItemDesc = otherTaxesTO.TaxName;
                                else
                                    tcsItemTO.ProdItemDesc = "TCS";

                                var maxTaxableItemTO = tblInvoiceItemDetailsTOList.OrderByDescending(m => m.TaxableAmt).FirstOrDefault();


                                tcsItemTO.ProdGstCodeId = maxTaxableItemTO.ProdGstCodeId;
                                tcsItemTO.GstinCodeNo = maxTaxableItemTO.GstinCodeNo;
                                Double tcsTaxPercent = Convert.ToDouble(tcsPercentConfigParamsTO.ConfigParamVal);
                                tcsItemTO.TaxPct = tcsTaxPercent;
                                tcsItemTO.TaxableAmt = (grandTotal * tcsTaxPercent) / 100;
                                //tcsItemTO.TaxableAmt = Math.Round(tcsItemTO.TaxableAmt, 2);
                                tcsItemTO.TaxableAmt = Math.Round(tcsItemTO.TaxableAmt, isForItemWiseRoundup);
                                tcsGrandTotal += tcsItemTO.TaxableAmt;
                                tcsItemTO.GrandTotal = tcsGrandTotal;
                                // taxableTotal += tcsItemTO.TaxableAmt;


                                if (maxTaxableItemTO.InvoiceItemTaxDtlsTOList != null && maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                                {
                                    for (int ti = 0; ti < maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count; ti++)
                                    {
                                        TblInvoiceItemTaxDtlsTO taxDtlTO = maxTaxableItemTO.InvoiceItemTaxDtlsTOList[ti].DeepCopy();
                                        taxDtlTO.TaxableAmt = tcsItemTO.TaxableAmt;
                                        taxDtlTO.TaxAmt = 0;
                                        taxDtlTO.TaxRatePct = 0.00;

                                        if (tcsItemTO.InvoiceItemTaxDtlsTOList == null)
                                            tcsItemTO.InvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                                        tcsItemTO.InvoiceItemTaxDtlsTOList.Add(taxDtlTO);
                                    }
                                }
                                tblInvoiceItemDetailsTOList.Add(tcsItemTO);

                                grandTotal += tcsGrandTotal;
                                otherTaxAmt += tcsGrandTotal;
                            }
                        }
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;

        }
        //Reshma Added For ddition of Loading charges
        private ResultMessage AddLoadingChargesTOInTaxItemDtls(SqlConnection conn, SqlTransaction tran, ref double grandTotal, ref double taxableTotal, ref double basicTotal, bool isPanNoPresent, List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList, TblInvoiceTO tblInvoiceTo, ref double otherTaxAmt, Int32 isDeclarationRec)
        {

            ResultMessage resultMessage = new ResultMessage();

            if (tblInvoiceTo.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
            {
                TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_IS_INCLUDE_Loading_Charges_TO_AUTO_INVOICE, conn, tran);

                if (configParamsTO != null)
                {
                    if (configParamsTO.ConfigParamVal == "1")
                    {
                        TblConfigParamsTO tcsConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_CHARGES_OTHER_TAX_ID, conn, tran);
                        if (tcsConfigParamsTO == null)
                        {
                            resultMessage.DefaultBehaviour("Other Tax id is not configured for Loading Charges");
                            return resultMessage;
                        }
                        Int32 isForItemWiseRoundup = 2;
                        TblConfigParamsTO cPisForItemWiseRoundup = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.ITEM_GRAND_TOTAL_ROUNDUP_VALUE, conn, tran);
                        if (cPisForItemWiseRoundup != null)
                        {
                            isForItemWiseRoundup = Convert.ToInt32(cPisForItemWiseRoundup.ConfigParamVal);
                        }
                        TblConfigParamsTO tcsPercentConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_LOADING_CHARGES_AMT, conn, tran);

                        if (tcsPercentConfigParamsTO == null)
                        {
                            resultMessage.DefaultBehaviour("Loading Charges Tax Pct is not defined.");
                            return resultMessage;
                        }

                        if (tcsPercentConfigParamsTO != null)
                        {
                            if (tcsPercentConfigParamsTO.ConfigParamVal != "0" && tcsPercentConfigParamsTO.ConfigParamVal != null)
                            {
                                Int32 tcsOtherTaxId = Convert.ToInt32(tcsConfigParamsTO.ConfigParamVal);
                                Double tcsGrandTotal = 0;
                                TblInvoiceItemDetailsTO tcsItemTO = new TblInvoiceItemDetailsTO();
                                tcsItemTO.OtherTaxId = tcsOtherTaxId;
                                TblOtherTaxesTO otherTaxesTO = _iTblOtherTaxesDAO.SelectTblOtherTaxes(tcsOtherTaxId);
                                if (otherTaxesTO != null)
                                    tcsItemTO.ProdItemDesc = otherTaxesTO.TaxName;
                                else
                                    tcsItemTO.ProdItemDesc = "Loading Charges";

                                var maxTaxableItemTO = tblInvoiceItemDetailsTOList.OrderByDescending(m => m.TaxableAmt).FirstOrDefault();

                                double invoiceQty = tblInvoiceItemDetailsTOList.Where(w => w.OtherTaxId == 0).Sum(s => s.InvoiceQty);
                                tcsItemTO.ProdGstCodeId = maxTaxableItemTO.ProdGstCodeId;
                                tcsItemTO.GstinCodeNo = maxTaxableItemTO.GstinCodeNo;
                                Double tcsTaxPercent = Convert.ToDouble(tcsPercentConfigParamsTO.ConfigParamVal);
                                tcsItemTO.TaxPct = tcsTaxPercent;
                                tcsItemTO.TaxableAmt = ((invoiceQty* tcsTaxPercent)/118)*100;//118= 100 is loading charges and 18 is tax
                                //tcsItemTO.TaxableAmt = Math.Round(tcsItemTO.TaxableAmt, 2);
                                tcsItemTO.TaxableAmt = Math.Round(tcsItemTO.TaxableAmt, isForItemWiseRoundup);
                                tcsGrandTotal += tcsItemTO.TaxableAmt;
                                tcsItemTO.GrandTotal = tcsGrandTotal;
                                // taxableTotal += tcsItemTO.TaxableAmt;

                                if (maxTaxableItemTO.InvoiceItemTaxDtlsTOList != null && maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                                {
                                    for (int ti = 0; ti < maxTaxableItemTO.InvoiceItemTaxDtlsTOList.Count; ti++)
                                    {
                                        TblInvoiceItemTaxDtlsTO taxDtlTO = maxTaxableItemTO.InvoiceItemTaxDtlsTOList[ti].DeepCopy();
                                        taxDtlTO.TaxableAmt = tcsItemTO.TaxableAmt;
                                        taxDtlTO.TaxAmt = 0;
                                        taxDtlTO.TaxRatePct = 0.00;

                                        if (tcsItemTO.InvoiceItemTaxDtlsTOList == null)
                                            tcsItemTO.InvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                                        tcsItemTO.InvoiceItemTaxDtlsTOList.Add(taxDtlTO);
                                    }
                                }
                                tblInvoiceItemDetailsTOList.Add(tcsItemTO);

                                grandTotal += tcsGrandTotal;
                                otherTaxAmt += tcsGrandTotal;
                            }
                        }
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;

        }
        private void RemoveIotFieldsFromDB(TblInvoiceTO tblInvoiceTO)
        {
            int configId = _iTblConfigParamsDAO.IoTSetting();
            if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
            {
                if (tblInvoiceTO.InvoiceStatusE != InvoiceStatusE.AUTHORIZED && tblInvoiceTO.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                {
                    if (tblInvoiceTO.LoadingSlipId > 0)
                    {
                        tblInvoiceTO.VehicleNo = String.Empty;
                        tblInvoiceTO.TransportOrgId = 0;

                        tblInvoiceTO.GrossWeight = 0;
                        tblInvoiceTO.NetWeight = 0;
                        //tblInvoiceTO.TareWeight = 0;
                        if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                        {
                            for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList.Count; j++)
                            {
                                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[j];
                                if (tblInvoiceItemDetailsTO.LoadingSlipExtId > 0)
                                {
                                    tblInvoiceItemDetailsTO.InvoiceQty = 0;
                                    tblInvoiceItemDetailsTO.Bundles = "";
                                }
                            }
                        }
                    }
                }
            }

        }
        //public ResultMessage PrepareAndSaveInternalTaxInvoices(TblInvoiceTO invoiceTO, SqlConnection conn, SqlTransaction tran)
        public ResultMessage PrepareAndSaveInternalTaxInvoices(TblInvoiceTO invoiceTO, int invoiceGenerateModeE, int fromOrgId, int toOrgId, int isCalculateWithBaseRate, TblInvoiceChangeOrgHistoryTO changeHisTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMsg = new ResultMessage();
            string entityRangeName = string.Empty;
            Int32 result = 0;
            List<DropDownTO> districtList = new List<DropDownTO>();
            List<DropDownTO> talukaList = new List<DropDownTO>();
            try
            {
                DateTime serverDateTime = _iCommon.ServerDateTime;
                Int32 invoiceId = invoiceTO.IdInvoice;
                int configId = _iTblConfigParamsDAO.IoTSetting();
                List<TblInvoiceItemDetailsTO> invoiceItemTOList = new List<TblInvoiceItemDetailsTO>();
                List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxTOList = new List<TblInvoiceItemTaxDtlsTO>();
                List<TblInvoiceAddressTO> invoiceAddressTOList = new List<TblInvoiceAddressTO>();
                //Added By Kiran For IoT related Invoice data 24/09/2019
                if (configId == Convert.ToInt32(Constants.WeighingDataSourceE.IoT))
                {
                    invoiceTO = SelectTblInvoiceTOWithDetails(invoiceId, conn, tran);
                    invoiceItemTOList = invoiceTO.InvoiceItemDetailsTOList;
                    invoiceTO.InvoiceItemDetailsTOList.ForEach(f => { invoiceItemTaxTOList.AddRange(f.InvoiceItemTaxDtlsTOList); });
                    invoiceAddressTOList = invoiceTO.InvoiceAddressTOList;
                }
                else
                {
                    invoiceItemTOList = _iTblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceId, conn, tran);
                    invoiceItemTaxTOList = _iTblInvoiceItemTaxDtlsBL.SelectInvoiceItemTaxDtlsListByInvoiceId(invoiceId, conn, tran);
                    invoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId, conn, tran);
                }
                #region 1 BRM TO BM Invoice

                //pass changed from org ()    
                List<TblInvoiceItemDetailsTO> invoiceItemChangeList = new List<TblInvoiceItemDetailsTO>();
                invoiceItemTOList.ForEach(d =>
                {
                    invoiceItemChangeList.Add(d.DeepCopy());
                }
                );
                resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, invoiceGenerateModeE, fromOrgId, toOrgId, 0, conn, tran, 0);
                if (resultMsg.MessageType == ResultMessageE.Information)
                {
                    if (resultMsg.Tag != null && resultMsg.Tag.GetType() == typeof(List<TblInvoiceTO>))
                    {
                        List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMsg.Tag;
                        if (tblInvoiceTOList != null)
                        {
                            //Update Existing Invoice
                            TblInvoiceTO invToUpdateTO = tblInvoiceTOList[0]; //Taken 0th Object as it will always go for single invoice at a time.List is return as existing code is used.
                            //Double tdsTaxPct = 0;
                            //if (invToUpdateTO != null)
                            //{
                            //    if (invToUpdateTO.IsTcsApplicable == 0)
                            //    {
                            //        TblConfigParamsTO tdsConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT);
                            //        if (tdsConfigParamsTO != null)
                            //        {
                            //            if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                            //            {
                            //                tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                            //            }
                            //        }
                            //    }
                            //}
                            //invToUpdateTO.TdsAmt = 0;
                            //if (invoiceTO.IsConfirmed == 1 && invoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                            //{
                            //    invToUpdateTO.TdsAmt = (CalculateTDS(invToUpdateTO) * tdsTaxPct) / 100;
                            //    invToUpdateTO.TdsAmt = Math.Ceiling(invToUpdateTO.TdsAmt);
                            //}
                            //Delete existing invoice item taxes details
                            result = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(invToUpdateTO.IdInvoice, conn, tran);
                            if (result <= -1)
                            {
                                resultMsg.DefaultBehaviour("Error While DeleteInvoiceItemTaxDtlsByInvId");
                                return resultMsg;
                            }

                            //Update Invoice Object
                            invToUpdateTO.UpdatedBy = invoiceTO.CreatedBy;
                            invToUpdateTO.UpdatedOn = _iCommon.ServerDateTime;
                            invToUpdateTO.InvFromOrgFreeze = 1;

                            RemoveIotFieldsFromDB(invToUpdateTO);

                            result = UpdateTblInvoice(invToUpdateTO, conn, tran);
                            if (result != 1)
                            {
                                resultMsg.DefaultBehaviour("Error While UpdateTblInvoice");
                                return resultMsg;
                            }

                            // Added for insert new created item list and tex details for change invoice mode and calculate with base rate @Kiran

                            //if(isCalculateWithBaseRate == 1 && invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE)
                            //{
                            //    for (int invI = 0; invI < invToUpdateTO.InvoiceItemDetailsTOList.Count; invI++)
                            //    {
                            //        TblInvoiceItemDetailsTO itemTO = invToUpdateTO.InvoiceItemDetailsTOList[invI];

                            //        result = _iTblInvoiceItemDetailsBL.InsertTblInvoiceItemDetails(itemTO, conn, tran);
                            //        if (result != 1)
                            //        {
                            //            resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemDetails");
                            //            return resultMsg;
                            //        }
                            //        if (itemTO.InvoiceItemTaxDtlsTOList != null && itemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                            //        {
                            //            for (int t = 0; t < itemTO.InvoiceItemTaxDtlsTOList.Count; t++)
                            //            {
                            //                itemTO.InvoiceItemTaxDtlsTOList[t].InvoiceItemId = itemTO.IdInvoiceItem;
                            //                result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(itemTO.InvoiceItemTaxDtlsTOList[t], conn, tran);
                            //                if (result != 1)
                            //                {
                            //                    resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
                            //                    return resultMsg;
                            //                }
                            //            }
                            //        }
                            //    }
                            //}else
                            //{
                            // Update Invoice Item Details
                            for (int invI = 0; invI < invToUpdateTO.InvoiceItemDetailsTOList.Count; invI++)
                            {
                                TblInvoiceItemDetailsTO itemTO = invToUpdateTO.InvoiceItemDetailsTOList[invI];
                                result = _iTblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(itemTO, conn, tran);
                                if (result != 1)
                                {
                                    resultMsg.DefaultBehaviour("Error While UpdateTblInvoiceItemDetails");
                                    return resultMsg;
                                }

                                if (itemTO.InvoiceItemTaxDtlsTOList != null && itemTO.InvoiceItemTaxDtlsTOList.Count > 0)
                                {
                                    for (int t = 0; t < itemTO.InvoiceItemTaxDtlsTOList.Count; t++)
                                    {
                                        itemTO.InvoiceItemTaxDtlsTOList[t].InvoiceItemId = itemTO.IdInvoiceItem;
                                        result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(itemTO.InvoiceItemTaxDtlsTOList[t], conn, tran);
                                        if (result != 1)
                                        {
                                            resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
                                            return resultMsg;
                                        }
                                    }
                                }
                            }
                            // }

                            result = _iTblInvoiceAddressBL.DeleteTblInvoiceAddressByinvoiceId(invToUpdateTO.IdInvoice, conn, tran);
                            if (result == -1)
                            {
                                resultMsg.DefaultBehaviour("Error While DeleteTblInvoiceItemTaxDtls");
                                return resultMsg;
                            }
                            //Update Existing Address Details
                            for (int ac = 0; ac < invToUpdateTO.InvoiceAddressTOList.Count; ac++)
                            {
                                invToUpdateTO.InvoiceAddressTOList[ac].InvoiceId = invToUpdateTO.IdInvoice;
                                result = _iTblInvoiceAddressBL.InsertTblInvoiceAddress(invToUpdateTO.InvoiceAddressTOList[ac], conn, tran);
                                if (result != 1)
                                {
                                    resultMsg.DefaultBehaviour("Error While InsertTblInvoiceItemTaxDtls");
                                    return resultMsg;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return resultMsg;
                }

                #endregion

                //duplicate   
                #region 2 BM TO Actual Customer Invoice
                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE)
                {
                    //pass second org (org->cust)
                    //resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemTOList, invoiceAddressTOList, invoiceGenerateModeE,fromOrgId,toOrgId, 0,conn, tran);
                    //Change method Parameter for new colne @Kiran
                    resultMsg = PrepareNewInvoiceObjectList(invoiceTO, invoiceItemChangeList, invoiceAddressTOList, invoiceGenerateModeE, fromOrgId, toOrgId, isCalculateWithBaseRate, conn, tran);
                    if (resultMsg.MessageType == ResultMessageE.Information)
                    {
                        if (resultMsg.Tag != null && resultMsg.Tag.GetType() == typeof(List<TblInvoiceTO>))
                        {
                            List<TblInvoiceTO> tblInvoiceTOList = (List<TblInvoiceTO>)resultMsg.Tag;
                            if (tblInvoiceTOList != null)
                            {
                                for (int i = 0; i < tblInvoiceTOList.Count; i++)
                                {
                                    tblInvoiceTOList[i].InvFromOrgFreeze = 1;

                                    RemoveIotFieldsFromDB(tblInvoiceTOList[i]);
                                    Double tdsTaxPct = 0;
                                    if (tblInvoiceTOList[i] != null)
                                    {
                                        if (tblInvoiceTOList[i].IsTcsApplicable == 0)
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
                                    tblInvoiceTOList[i].TdsAmt = 0;
                                    if (invoiceTO.IsConfirmed == 1 && invoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                                    {
                                        tblInvoiceTOList[i].TdsAmt = (CalculateTDS(tblInvoiceTOList[i]) * tdsTaxPct) / 100;
                                        tblInvoiceTOList[i].TdsAmt = Math.Ceiling(tblInvoiceTOList[i].TdsAmt);
                                    }

                                    resultMsg = SaveNewInvoice(tblInvoiceTOList[i], conn, tran);
                                    if (resultMsg.MessageType != ResultMessageE.Information)
                                    {
                                        return resultMsg;
                                    }
                                    else
                                    {
                                        changeHisTO.DupInvoiceId = tblInvoiceTOList[i].IdInvoice;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return resultMsg;
                    }
                }
                #endregion

                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PrepareAndSaveNewTaxInvoice");
                return resultMsg;
            }
        }

        public ResultMessage PrepareNewInvoiceObjectList(TblInvoiceTO invoiceTO, List<TblInvoiceItemDetailsTO> invoiceItemTOList, List<TblInvoiceAddressTO> invoiceAddressTOList, int invoiceGenerateModeE, int fromOrgId, int toOrgId, int isCalculateWithBaseRate, SqlConnection conn, SqlTransaction tran, int swap = 1)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                #region Prepare List Of Invoices To Save

                List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();

                TblInvoiceTO tblInvoiceTO = invoiceTO.DeepCopy();
                tblInvoiceTO.InvoiceAddressTOList = new List<TblInvoiceAddressTO>();
                tblInvoiceTO.InvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
                double grandTotal = 0;
                double discountTotal = 0;
                double igstTotal = 0;
                double cgstTotal = 0;
                double sgstTotal = 0;
                double basicTotal = 0;
                double taxableTotal = 0;
                double otherTaxAmount = 0;
                TblConfigParamsTO tblConfigParamsTO = null;
                DateTime serverDateTime = _iCommon.ServerDateTime;
                Int32 billingStateId = 0;
                Int32 isForItemWiseRoundup = 2;
                //chetan[2020 - june - 08] added
                TblConfigParamsTO cPisForItemWiseRoundup = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.ITEM_GRAND_TOTAL_ROUNDUP_VALUE, conn, tran);
                if (cPisForItemWiseRoundup != null)
                {
                    isForItemWiseRoundup = Convert.ToInt32(cPisForItemWiseRoundup.ConfigParamVal);
                }

                //Hrushikesh Need to change here
                // if (invoiceGenerateModeE == (int)Constants.InvoiceGenerateModeE.Duplicate)
                // {
                //     //org1 id (org1->org2)
                //     tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                // }
                // else
                //     //changed from org                
                //     tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);

                // if (tblConfigParamsTO == null)
                // {
                //     resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                //     return resultMsg;
                // }
                // //Hrushikesh Need to change here 
                // Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                Int32 internalOrgId = fromOrgId;

                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && swap == 0)
                    internalOrgId = toOrgId;
                TblOrganizationTO orgTO = _iTblOrganizationBL.SelectTblOrganizationTO(internalOrgId, conn, tran);
                TblAddressTO ofcAddrTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                if (ofcAddrTO == null)
                {
                    resultMsg.DefaultBehaviour("Address Not Found For Self Organization.");
                    return resultMsg;
                }



                List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

                #region 1 Preparing main InvoiceTO
                // if(tblInvoiceTO.InvFromOrgId==0)
                tblInvoiceTO.InvFromOrgId = internalOrgId;
                tblInvoiceTO.CreatedOn = _iCommon.ServerDateTime;
                tblInvoiceTO.CreatedBy = invoiceTO.CreatedBy;
                tblInvoiceTO.InvoiceDate = tblInvoiceTO.CreatedOn;
                tblInvoiceTO.StatusDate = tblInvoiceTO.InvoiceDate;

                #endregion

                #region 2 Added Invoice Address Details
                if (invoiceGenerateModeE == (int)Constants.InvoiceGenerateModeE.DUPLICATE && swap == 1)
                {
                    //tblInvoiceTO.Narration = "To Bhagylaxmi Metal";

                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_INTERNALTXFER_INVOICE_ORG_ID, conn, tran);

                    if (configParamsTO == null)
                    {
                        resultMsg.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                        return resultMsg;
                    }
                    //Hrushikesh Need to change here 
                    //org2 id (org2->cust)
                    // Int32 internalBMOrgId =   Convert.ToInt32(configParamsTO.ConfigParamVal);
                    Int32 internalBMOrgId = toOrgId;
                    TblOrganizationTO bmOrgTO = _iTblOrganizationBL.SelectTblOrganizationTO(internalBMOrgId, conn, tran);
                    TblAddressTO bmOfcAddrTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(internalBMOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                    if (bmOfcAddrTO == null)
                    {
                        resultMsg.DefaultBehaviour("Address Not Found For BM Organization.");
                        return resultMsg;
                    }

                    tblInvoiceTO.DealerOrgId = internalBMOrgId;  //BMM AS dealer.
                    tblInvoiceTO.IsTcsApplicable = bmOrgTO.IsTcsApplicable;
                    tblInvoiceTO.IsDeclarationRec = bmOrgTO.IsDeclarationRec;
                    List<TblOrgLicenseDtlTO> licenseList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(internalBMOrgId, conn, tran);
                    String aadharNo = string.Empty;
                    String gstNo = string.Empty;
                    String panNo = string.Empty;

                    if (licenseList != null)
                    {
                        var panNoObj = licenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                        if (panNoObj != null)
                            panNo = panNoObj.LicenseValue;
                        var gstinObj = licenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                        if (gstinObj != null)
                            gstNo = gstinObj.LicenseValue;
                    }

                    TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
                    tblInvoiceAddressTo.GstinNo = gstNo;
                    tblInvoiceAddressTo.PanNo = panNo;
                    tblInvoiceAddressTo.StateId = bmOfcAddrTO.StateId;
                    tblInvoiceAddressTo.State = bmOfcAddrTO.StateName;
                    tblInvoiceAddressTo.Taluka = bmOfcAddrTO.TalukaName;
                    tblInvoiceAddressTo.District = bmOfcAddrTO.DistrictName;
                    tblInvoiceAddressTo.BillingName = bmOrgTO.FirmName;
                    tblInvoiceAddressTo.ContactNo = bmOrgTO.RegisteredMobileNos;
                    tblInvoiceAddressTo.PinCode = bmOfcAddrTO.Pincode.ToString();
                    tblInvoiceAddressTo.TxnAddrTypeId = (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS;
                    tblInvoiceAddressTo.Address = bmOfcAddrTO.PlotNo + bmOfcAddrTO.StreetName;
                    tblInvoiceAddressTo.AddrSourceTypeId = (int)Constants.AddressSourceTypeE.FROM_CNF;
                    tblInvoiceAddressTo.BillingOrgId = bmOrgTO.IdOrganization;
                    tblInvoiceAddressTo.VillageName = bmOfcAddrTO.VillageName;

                    billingStateId = bmOfcAddrTO.StateId;
                    if (string.IsNullOrEmpty(bmOfcAddrTO.VillageName))
                    {
                        if (string.IsNullOrEmpty(bmOfcAddrTO.TalukaName))
                        {
                            tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.DistrictName;
                        }
                        tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.TalukaName;
                    }
                    else
                        tblInvoiceTO.DeliveryLocation = bmOfcAddrTO.VillageName;


                    tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);

                    TblInvoiceAddressTO consigneeInvoiceAddressTo = tblInvoiceAddressTo.DeepCopy();
                    consigneeInvoiceAddressTo.TxnAddrTypeId = (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS;
                    tblInvoiceTO.InvoiceAddressTOList.Add(consigneeInvoiceAddressTo);

                }
                else if (invoiceGenerateModeE == (int)Constants.InvoiceGenerateModeE.CHANGEFROM || swap == 0)
                {
                    foreach (var deliveryAddrTo in invoiceAddressTOList)
                    {
                        TblInvoiceAddressTO tblInvoiceAddressTo = new TblInvoiceAddressTO();
                        tblInvoiceAddressTo.AadharNo = deliveryAddrTo.AadharNo;
                        tblInvoiceAddressTo.GstinNo = deliveryAddrTo.GstinNo;
                        tblInvoiceAddressTo.PanNo = deliveryAddrTo.PanNo;
                        tblInvoiceAddressTo.StateId = deliveryAddrTo.StateId;
                        tblInvoiceAddressTo.State = deliveryAddrTo.State;
                        tblInvoiceAddressTo.Taluka = deliveryAddrTo.Taluka;
                        tblInvoiceAddressTo.District = deliveryAddrTo.District;
                        tblInvoiceAddressTo.BillingName = deliveryAddrTo.BillingName;
                        tblInvoiceAddressTo.ContactNo = deliveryAddrTo.ContactNo;
                        tblInvoiceAddressTo.PinCode = deliveryAddrTo.PinCode;
                        tblInvoiceAddressTo.TxnAddrTypeId = deliveryAddrTo.TxnAddrTypeId;
                        tblInvoiceAddressTo.Address = deliveryAddrTo.Address;
                        tblInvoiceAddressTo.AddrSourceTypeId = deliveryAddrTo.AddrSourceTypeId;
                        tblInvoiceAddressTo.BillingOrgId = deliveryAddrTo.BillingOrgId;
                        tblInvoiceAddressTo.VillageName = deliveryAddrTo.VillageName;

                        if (deliveryAddrTo.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                            billingStateId = deliveryAddrTo.StateId;

                        tblInvoiceTO.InvoiceAddressTOList.Add(tblInvoiceAddressTo);
                    }
                }

                if (billingStateId == 0)
                {
                    resultMsg.DefaultBehaviour("Billing State Not Found");
                    return resultMsg;
                }

                #endregion

                #region 3 Added Invoice Item details


                #region Regular Invoice Items i.e from Loading
                //Added By Kiran new changes for invoice calculate using base rate
                if (isCalculateWithBaseRate == 1 && invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE)
                {
                    List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOsList = new List<TblInvoiceItemDetailsTO>();
                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceItemTOList[0];

                    tblInvoiceItemDetailsTO.InvoiceQty = invoiceItemTOList.Where(w => w.OtherTaxId == 0).ToList().Sum(s => s.InvoiceQty);
                    tblInvoiceItemDetailsTO.CdStructure = 0;
                    tblInvoiceItemDetailsTO.CdAmt = 0;
                    tblInvoiceItemDetailsTO.LoadingSlipExtId = 0;
                    tblInvoiceItemDetailsTO.CdStructureId = 0;

                    Double sum = 0;
                    //var resultDelInvoice = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTO.IdInvoice, conn, tran);
                    //if (resultDelInvoice <= -1)
                    //{
                    //    resultMsg.DefaultBehaviour("Error While DeleteInvoiceItemTaxDtlsByInvId");
                    //    return resultMsg;
                    //}
                    for (int i = 0; i < invoiceItemTOList.Count; i++)
                    {
                        Double bundles = 0;
                        bool result = double.TryParse(invoiceItemTOList[i].Bundles, out bundles);
                        if (result)
                        {
                            sum += bundles;
                        }
                        //var resultItem = _iTblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetails(invoiceItemTOList[i].IdInvoiceItem, conn, tran);
                        //if (resultItem != 1)
                        //{
                        //    resultMsg.DefaultBehaviour("Error While DeleteTblInvoiceItemDetails");
                        //    return resultMsg;
                        //}

                    }

                    tblInvoiceItemDetailsTO.Bundles = Convert.ToString(sum);
                    TblBookingsTO TblBookingsTO = _iTblBookingsBL.SelectBookingsDetailsFromInVoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                    if (TblBookingsTO == null)
                    {
                        resultMsg.DefaultBehaviour("Booking details not found");
                        resultMsg.DisplayMessage = "Booking Details Not Found for invoice Id " + tblInvoiceTO.IdInvoice;
                        return resultMsg;
                    }

                    TblBookingsTO = _iTblBookingsBL.SelectTblBookingsTO(TblBookingsTO.IdBooking, conn, tran);
                    if (TblBookingsTO == null)
                    {
                        resultMsg.DefaultBehaviour("Booking details not found");
                        resultMsg.DisplayMessage = "Booking Details Not Found for invoice Id " + tblInvoiceTO.IdInvoice;
                        return resultMsg;
                    }
                    //Int32 isTaxInclusiveWithTaxes = 0;
                    //TblConfigParamsTO rateCalcConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_RATE_CALCULATIONS_TAX_INCLUSIVE, conn, tran);
                    //if (rateCalcConfigParamsTO != null)
                    //{
                    //    isTaxInclusiveWithTaxes = Convert.ToInt32(rateCalcConfigParamsTO.ConfigParamVal);
                    //}
                    Int32 isTaxInclusive = 0;
                    DimBrandTO dimBrandTO = _iDimBrandDAO.SelectDimBrand(TblBookingsTO.BrandId);

                    //To get INTERNAL DEFAULT ITEM
                    TblConfigParamsTO tblConfigParamForInternalItem = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.INTERNAL_DEFAULT_ITEM, conn, tran);

                    if (tblConfigParamForInternalItem == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Internal INTERNAL DEFAULT ITEM Not Found in Configuration.");
                        return resultMsg;
                    }

                    Int32 prodItemId = Convert.ToInt32(tblConfigParamForInternalItem.ConfigParamVal);
                    if (prodItemId == 0)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Internal INTERNAL DEFAULT ITEM Not Found in Configuration.");
                        return resultMsg;
                    }

                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTOTemp = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(0, 0, 0, prodItemId, 0, conn, tran);

                    if (tblProdGstCodeDtlsTOTemp == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Please define GST code for item Id - " + prodItemId);
                        return resultMsg;
                    }
                    TblGstCodeDtlsTO gstCodeDtlsTO = _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(tblProdGstCodeDtlsTOTemp.GstCodeId, conn, tran);
                    if (gstCodeDtlsTO == null)
                    {
                        resultMsg.DefaultBehaviour("GST code details found null : " + tblInvoiceItemDetailsTO.ProdItemDesc + ".");
                        resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                        return resultMsg;
                    }

                    if (dimBrandTO != null)
                    {
                        isTaxInclusive = dimBrandTO.IsTaxInclusive;
                    }
                    //if (isTaxInclusive == 1 && isTaxInclusiveWithTaxes == 0)
                    if (isTaxInclusive == 1)
                    {

                        List<TblLoadingSlipTO> TblLoadingSlipTOList = SelectLoadingSlipDetailsByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                        if (TblLoadingSlipTOList == null || TblLoadingSlipTOList.Count == 0)
                        {
                            resultMsg.DefaultBehaviour("Loading Slip not found against invoice - " + tblInvoiceTO.IdInvoice);
                            resultMsg.DisplayMessage = "Loading Slip not found against invoice - " + tblInvoiceTO.IdInvoice;
                            return resultMsg;
                        }

                        TblLoadingSlipTO tblLoadingSlipTO = TblLoadingSlipTOList[0];

                        Double bookingRateTemp = TblBookingsTO.BookingRate;
                        Double cdAmt = 0;
                        if (tblLoadingSlipTO.CdStructure >= 0)
                        {
                            DropDownTO dropDownTO = _iDimensionDAO.SelectCDDropDown(tblLoadingSlipTO.CdStructureId);

                            //Priyanka [23-07-2018] Added if cdstructure is 0
                            Int32 isRsValue = Convert.ToInt32(dropDownTO.Text);
                            if (isRsValue == (int)Constants.CdType.IsRs)
                            {
                                cdAmt = tblLoadingSlipTO.CdStructure;
                            }
                            else
                            {

                                cdAmt = (TblBookingsTO.BookingRate * tblLoadingSlipTO.CdStructure) / 100;
                            }

                        }
                        Double orcPerMt = 0;
                        if (tblLoadingSlipTO.OrcAmt > 0)
                        {
                            if (tblLoadingSlipTO.OrcMeasure == "Rs/MT")
                            {
                                orcPerMt = tblLoadingSlipTO.OrcAmt;
                            }
                            else
                            {
                                orcPerMt = tblLoadingSlipTO.OrcAmt / tblLoadingSlipTO.LoadingQty;
                            }
                        }
                        Double baseRateDiff = 0;
                        TblConfigParamsTO tblConfigParamForInternalItemBaseRate = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.INTERNAL_DEFAULT_ITEM_BASE_RATE_DIFF_AMT, conn, tran);

                        if (tblConfigParamForInternalItemBaseRate != null)
                        {
                            if (tblConfigParamForInternalItemBaseRate.ConfigParamVal != null)
                            {
                                baseRateDiff = Convert.ToDouble(tblConfigParamForInternalItemBaseRate.ConfigParamVal);
                            }
                        }

                        bookingRateTemp -= baseRateDiff;
                        //bookingRateTemp -= 400;
                        bookingRateTemp -= cdAmt;
                        bookingRateTemp -= orcPerMt;

                        Double divisor = 100 + gstCodeDtlsTO.TaxPct;
                        divisor = divisor / 100;
                        bookingRateTemp = bookingRateTemp / divisor;

                        //[2019-12-13 Pandurang]Commented for A One Round Off issues suggested by Saket. 
                        //TblBookingsTO.BookingRate = Math.Round(bookingRateTemp, 2); 
                        TblBookingsTO.BookingRate = Math.Round(bookingRateTemp);

                    }
                    tblInvoiceItemDetailsTO.Rate = TblBookingsTO.BookingRate;
                    tblInvoiceItemDetailsTO.BasicTotal = Math.Round(tblInvoiceItemDetailsTO.Rate * tblInvoiceItemDetailsTO.InvoiceQty, isForItemWiseRoundup);
                    tblInvoiceItemDetailsTO.TaxableAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt, isForItemWiseRoundup);
                    invoiceTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;


                    TblProductItemTO tblProductItemTO = _iTblProductItemDAO.SelectTblProductItem(prodItemId);
                    if (tblProductItemTO == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Internal INTERNAL DEFAULT ITEM Not Found in Configuration.");
                        return resultMsg;
                    }

                    tblInvoiceItemDetailsTO.ProdItemDesc = tblProductItemTO.ItemName;

                    //tblInvoiceItemDetailsTO.ProdGstCodeId = Convert.ToInt32(tblConfigParamForInternalItem.ConfigParamVal);

                    tblInvoiceItemDetailsTO.ProdGstCodeId = tblProdGstCodeDtlsTOTemp.IdProdGstCode;

                    tblInvoiceItemDetailsTOsList.Add(tblInvoiceItemDetailsTO);

                    invoiceItemTOList = tblInvoiceItemDetailsTOsList;


                    //Harshala added 
                    Double grandTotal1 = tblInvoiceTO.GrandTotal;
                    Double taxableAmt = tblInvoiceTO.TaxableAmt;
                    Double basicTotalAmt = tblInvoiceTO.BasicAmt;
                    Boolean isPanPresent = false;
                    Double otherTaxAmt = 0;
                    ResultMessage message = new ResultMessage();
                    if (tblInvoiceTO.IsConfirmed == 1)
                    {
                        tblInvoiceTO.InvoiceAddressTOList.ForEach(element =>
                        {
                            if (element.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                            {
                                isPanPresent = IsPanOrGstPresent(element.PanNo, element.GstinNo);

                            }
                        });
                        TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(tblInvoiceTO.DealerOrgId);
                        if (tblOrganizationTO == null)
                        {
                            resultMsg.DefaultBehaviour("Failed to get dealer org details");
                            return resultMsg;
                        }
                        if (tblOrganizationTO.IsTcsApplicable == 1)
                        {
                            tblInvoiceTO.IsTcsApplicable = tblOrganizationTO.IsTcsApplicable;
                            message = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal1, ref taxableAmt, ref basicTotalAmt, isPanPresent, invoiceItemTOList, tblInvoiceTO, ref otherTaxAmt, tblOrganizationTO.IsDeclarationRec);
                        }
                        TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_IS_INCLUDE_Loading_Charges_TO_AUTO_INVOICE, conn, tran);

                        if (configParamsTO != null)
                        {
                            message=AddLoadingChargesTOInTaxItemDtls(conn, tran, ref grandTotal1, ref taxableAmt, ref basicTotalAmt, isPanPresent, invoiceItemTOList, tblInvoiceTO, ref otherTaxAmt, tblOrganizationTO.IsDeclarationRec);
                        }
                        tblInvoiceTO.GrandTotal = grandTotal1;
                        tblInvoiceTO.TaxableAmt = taxableAmt;
                        tblInvoiceTO.BasicAmt = basicTotalAmt;

                    }
                    //

                }
                Int32 tcsOtherTaxId = 0;
                //added by harshala to skip adding tcs value in taxable amt.
                TblConfigParamsTO configParamTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_TCS_OTHER_TAX_ID);
                if (configParamTO != null)
                {
                    tcsOtherTaxId = Convert.ToInt32(configParamTO.ConfigParamVal);
                }
                //
                Int32 tcsId = 5;
                TblConfigParamsTO tcsConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TCS_OTHER_TAX_ID, conn, tran);
                if (tcsConfigParamsTO != null)
                {
                    tcsId = Convert.ToInt32(tcsConfigParamsTO.ConfigParamVal);
                }
                foreach (var existingInvItemTO in invoiceItemTOList)
                {
                    Boolean ItsTcs = false;
                    if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && existingInvItemTO.OtherTaxId == tcsId && swap == 1)
                    {
                        ItsTcs = true;
                    }
                    if (ItsTcs == false)
                    {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = existingInvItemTO.DeepCopy();
                        List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();
                        TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = new TblProdGstCodeDtlsTO();
                        TblProductItemTO tblProductItemTO = new TblProductItemTO();
                        Double itemGrandTotal = 0;

                        tblProdGstCodeDtlsTO = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(tblInvoiceItemDetailsTO.ProdGstCodeId, conn, tran);
                        if (tblProdGstCodeDtlsTO == null)
                        {
                            resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against IdInvoiceItem is : " + tblInvoiceItemDetailsTO.IdInvoiceItem + ".");
                            resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                            return resultMsg;
                        }
                        TblGstCodeDtlsTO gstCodeDtlsTO = _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                        if (gstCodeDtlsTO != null)
                        {
                            gstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesDAO.SelectAllTblTaxRates(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                        }
                        if (gstCodeDtlsTO == null)
                        {
                            resultMsg.DefaultBehaviour("GST code details found null : " + tblInvoiceItemDetailsTO.ProdItemDesc + ".");
                            resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                            return resultMsg;
                        }

                        tblInvoiceItemDetailsTO.GstinCodeNo = gstCodeDtlsTO.CodeNumber;

                        #region 4 Added Invoice Item Tax details

                        foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
                        {
                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
                            tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
                            tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
                            //Harshala added
                            if (existingInvItemTO.OtherTaxId > 0)
                            {
                                TblOtherTaxesTO tblOtherTaxesTO = _iTblOtherTaxesDAO.SelectTblOtherTaxes(existingInvItemTO.OtherTaxId);
                                if (tblOtherTaxesTO != null && tblOtherTaxesTO.IsAfter == 1)
                                {
                                    taxRateTo.TaxPct = 0;
                                    tblInvoiceItemTaxDtlsTO.TaxPct = 0;
                                }
                            }
                            //
                            tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                            tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;

                            //Saket [2020-02-18] Added SEZ conditons.
                            if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                            {
                                tblInvoiceItemTaxDtlsTO.TaxRatePct = 0;
                                tblInvoiceItemTaxDtlsTO.TaxableAmt = 0;
                            }

                            tblInvoiceItemTaxDtlsTO.TaxAmt = ((tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100);
                            tblInvoiceItemTaxDtlsTO.TaxTypeId = taxRateTo.TaxTypeId;



                            if (billingStateId == ofcAddrTO.StateId)
                            {
                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                {
                                    cgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                {
                                    sgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else continue;
                            }
                            else
                            {
                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                {
                                    igstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                }
                                else continue;
                            }
                        }
                        #endregion

                        basicTotal += existingInvItemTO.BasicTotal;

                        if (tcsOtherTaxId == existingInvItemTO.OtherTaxId)  //added by harshala 
                            otherTaxAmount += existingInvItemTO.TaxableAmt;
                        else
                            taxableTotal += existingInvItemTO.TaxableAmt;

                        discountTotal += existingInvItemTO.CdAmt;

                        itemGrandTotal += existingInvItemTO.TaxableAmt;

                        grandTotal += itemGrandTotal;
                        tblInvoiceItemDetailsTO.GrandTotal = Math.Round(itemGrandTotal, isForItemWiseRoundup);
                        tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
                        tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
                    }
                }
                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && swap == 1 && tblInvoiceTO.IsTcsApplicable == 1)
                {
                    //Harshala added 
                    Double grandTotal1 = tblInvoiceTO.GrandTotal;
                    Double taxableAmt = taxableTotal;
                    Double basicTotalAmt = basicTotal;
                    Boolean isPanPresent = false;
                    Double otherTaxAmt = otherTaxAmount;
                    ResultMessage message = new ResultMessage();
                    if (tblInvoiceTO.IsConfirmed == 1)
                    {
                        tblInvoiceTO.InvoiceAddressTOList.ForEach(element =>
                        {
                            if (element.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                            {
                                isPanPresent = IsPanOrGstPresent(element.PanNo, element.GstinNo);

                            }
                        });
                        message = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal1, ref taxableAmt, ref basicTotalAmt, isPanPresent, tblInvoiceItemDetailsTOList, tblInvoiceTO, ref otherTaxAmt, tblInvoiceTO.IsDeclarationRec);
                        tblInvoiceTO.GrandTotal = grandTotal1;
                        taxableTotal = taxableAmt;
                        basicTotal = basicTotalAmt;
                        otherTaxAmount = otherTaxAmt;
                    }
                }
                #endregion


                #endregion

                #region 5 Save main Invoice


                tblInvoiceTO.TaxableAmt = taxableTotal;
                tblInvoiceTO.DiscountAmt = discountTotal;
                tblInvoiceTO.IgstAmt = igstTotal;
                tblInvoiceTO.CgstAmt = cgstTotal;
                tblInvoiceTO.SgstAmt = sgstTotal;
                tblInvoiceTO.BasicAmt = basicTotal;
                tblInvoiceTO.OtherTaxAmt = otherTaxAmount;
                //double finalGrandTotal = Math.Round(grandTotal);
                //tblInvoiceTO.GrandTotal = finalGrandTotal;
                //tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);

                RoundOffInvoiceValuesBySetting(tblInvoiceTO);

                tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
                if (invoiceGenerateModeE == (int)InvoiceGenerateModeE.DUPLICATE && swap == 1)
                {
                    tblInvoiceTO.TdsAmt = 0;
                    if (tblInvoiceTO.IsTcsApplicable == 0)
                    {
                        Double tdsTaxPct = 0;
                        TblConfigParamsTO tdsConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                        if (tdsConfigParamsTO != null)
                        {
                            if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                            {
                                tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                            }
                        }
                        if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                        {
                            tblInvoiceTO.TdsAmt = (CalculateTDS(tblInvoiceTO) * tdsTaxPct) / 100;
                            tblInvoiceTO.TdsAmt = Math.Ceiling(tblInvoiceTO.TdsAmt);
                        }
                    }
                }
                #endregion

                tblInvoiceTOList.Add(tblInvoiceTO);

                resultMsg.DefaultSuccessBehaviour();
                resultMsg.Tag = tblInvoiceTOList;
                return resultMsg;

                #endregion
            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PrepareNewInvoiceObjectList");
                return resultMsg;
            }
            finally
            {

            }
        }


        public void RoundOffInvoiceValuesBySetting(TblInvoiceTO tblInvoiceTO)
        {

            int roundOffValue = 0;
            string roundOffValueCD = "";
            TblConfigParamsTO tblConfigParamsTOR = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.ROUND_OFF_TAX_INVOICE_VALUES);
            if (tblConfigParamsTOR != null)
            {
                roundOffValue = Convert.ToInt32(tblConfigParamsTOR.ConfigParamVal);
            }

            TblConfigParamsTO tblConfigParamsCD = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_CD_ON_Rate_Calculation_Details);
            if (tblConfigParamsCD != null)
            {
                roundOffValueCD = tblConfigParamsCD.ConfigParamVal;
            }

           


            //if (roundOffValue > 0) Saket [2019-12-05] not needed these condition 
            if (true)
            {
                if (roundOffValueCD != "")
                {
                    tblInvoiceTO.DiscountAmt = Math.Round(tblInvoiceTO.DiscountAmt, Convert.ToInt32(roundOffValueCD));
                }
                else
                {
                    tblInvoiceTO.DiscountAmt = Math.Round(tblInvoiceTO.DiscountAmt, roundOffValue);
                }
                tblInvoiceTO.TaxableAmt = Math.Round(tblInvoiceTO.TaxableAmt, roundOffValue);
                
                tblInvoiceTO.IgstAmt = Math.Round(tblInvoiceTO.IgstAmt, roundOffValue);
                tblInvoiceTO.CgstAmt = Math.Round(tblInvoiceTO.CgstAmt, roundOffValue);
                tblInvoiceTO.SgstAmt = Math.Round(tblInvoiceTO.SgstAmt, roundOffValue);
                tblInvoiceTO.BasicAmt = Math.Round(tblInvoiceTO.BasicAmt, roundOffValue);

            }

            Double grandTotal = tblInvoiceTO.TaxableAmt + tblInvoiceTO.IgstAmt + tblInvoiceTO.CgstAmt + tblInvoiceTO.SgstAmt + tblInvoiceTO.OtherTaxAmt;

            double finalGrandTotal = Math.Round(grandTotal);
            tblInvoiceTO.GrandTotal = finalGrandTotal;
            tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);
        }

        /// <summary>
        /// Vijaymala[13-04-2018]:added to merge invoices
        /// </summary>
        /// <param name="invoiceIdsList"></param>
        /// <param name="loginUserId"></param>
        /// <returns></returns>
        public ResultMessage ComposeInvoice(List<Int32> invoiceIdsList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage = ComposeInvoice(invoiceIdsList, loginUserId, conn, tran);

                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Success..Invoice composed";
                    resultMessage.Tag = null;
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "mergeInvoices");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        private ResultMessage ComposeInvoice(List<int> invoiceIdsList, int loginUserId, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();
            DateTime serverDate = _iCommon.ServerDateTime;
            int result = 0;
            List<TblInvoiceItemDetailsTO> finalInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();

            double grandTotal = 0;
            double discountTotal = 0;
            double igstTotal = 0;
            double cgstTotal = 0;
            double sgstTotal = 0;
            double basicTotal = 0;
            double taxableTotal = 0;


            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
            List<TblInvoiceAddressTO> tblInvoiceAddressTOList = new List<TblInvoiceAddressTO>();
            List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList = new List<TempLoadingSlipInvoiceTO>();
            List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = new List<TblInvoiceItemDetailsTO>();
            TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = new TempLoadingSlipInvoiceTO();
            List<TempInvoiceDocumentDetailsTO> tempInvoiceDocumentDetailsTOList = new List<TempInvoiceDocumentDetailsTO>();
            Boolean isWithinState = false;
            Int32 tcsOtherTaxId = 0;
            TblConfigParamsTO configParamTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_TCS_OTHER_TAX_ID);
            if (configParamTO != null)
            {
                tcsOtherTaxId = Convert.ToInt32(configParamTO.ConfigParamVal);
            }
            if (tcsOtherTaxId == 0)
            {
                tran.Rollback();
                resultMessage.DefaultBehaviour("TCS Tax Id Not Found");
                return resultMessage;
            }
            try
            {
                #region 1.To get data to combine invoices
                if (invoiceIdsList != null && invoiceIdsList.Count > 1)
                {
                    for (int i = 0; i < invoiceIdsList.Count; i++)
                    {
                        Int32 invoiceId = invoiceIdsList[i];

                        #region 1.1  to get invoices list


                        //existing invoiceTO with item details

                        TblInvoiceTO invoiceTO = SelectTblInvoiceTOWithDetails(invoiceId, conn, tran);

                        if (invoiceTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("invoiceTO Found NULL");
                            return resultMessage;
                        }
                        invoiceTO.IsTcsApplicable = 0;
                        if (invoiceTO.InvoiceItemDetailsTOList != null && invoiceTO.InvoiceItemDetailsTOList.Count > 0)
                        {
                            var matchTO = invoiceTO.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId == tcsOtherTaxId).FirstOrDefault();
                            if (matchTO != null)
                            {
                                invoiceTO.IsTcsApplicable = 1;
                            }
                        }


                        tblInvoiceTOList.Add(invoiceTO);
                        //all item list
                        finalInvoiceItemDetailsTOList.AddRange(invoiceTO.InvoiceItemDetailsTOList);
                        #endregion

                        #region 1.2 To get Invoice loading slip mapping list 

                        List<TempLoadingSlipInvoiceTO> tempLoadingSlipInvoiceTOList1 = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId, conn, tran);

                        tempLoadingSlipInvoiceTOList.AddRange(tempLoadingSlipInvoiceTOList1);

                        #endregion

                        #region 1.3 to get invoice document list

                        List<TempInvoiceDocumentDetailsTO> newTempInvoiceDocumentTOList = _iTempInvoiceDocumentDetailsDAO.SelectTempInvoiceDocumentDetailsByInvoiceId(invoiceId, conn, tran);

                        tempInvoiceDocumentDetailsTOList.AddRange(newTempInvoiceDocumentTOList);

                        #endregion

                    }

                }
                #endregion

                #region 2. Processing on data to merge invoices
                if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                {
                    //TblInvoiceTO finalInvoiceTO = new TblInvoiceTO();

                    for (int f = 0; f < tblInvoiceTOList.Count; f++)
                    {
                        #region 3. Delete Previous Tax Details
                        result = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTOList[f].IdInvoice, conn, tran);
                        if (result == -1)
                        {
                            resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                            return resultMessage;
                        }
                        #endregion
                    }

                    for (int f = 0; f < tblInvoiceTOList.Count; f++)
                    {
                        #region 3. Delete Previous Tax Details
                        result = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTOList[f].IdInvoice, conn, tran);
                        if (result == -1)
                        {
                            resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                            return resultMessage;
                        }
                        #endregion

                        if (f == 0)
                        {
                            TblInvoiceTO finalInvoiceTO = tblInvoiceTOList[0];
                            if (finalInvoiceTO.IsTcsApplicable == 0)
                            {
                                finalInvoiceItemDetailsTOList = finalInvoiceItemDetailsTOList.Where(w => w.OtherTaxId != tcsOtherTaxId).ToList();
                            }

                            //To calculate tare,gross and net weight
                            var minTareWt = tblInvoiceTOList.Min(minEle => minEle.TareWeight);
                            finalInvoiceTO.TareWeight = minTareWt;
                            var maxGrossWt = finalInvoiceItemDetailsTOList.Where (w=>w.OtherTaxId == 0 ).Sum(maxEle => maxEle.InvoiceQty);
                            finalInvoiceTO.NetWeight = (maxGrossWt * 1000);
                            finalInvoiceTO.GrossWeight = finalInvoiceTO.TareWeight + finalInvoiceTO.NetWeight;
                            var expenseAmt = tblInvoiceTOList.Sum(o => o.ExpenseAmt);
                            finalInvoiceTO.ExpenseAmt = expenseAmt;
                            var otherAmt = tblInvoiceTOList.Sum(o => o.OtherAmt);
                            finalInvoiceTO.OtherAmt = otherAmt;

                            //billing details 
                            Int32 billingStateId = 0;

                            tblInvoiceAddressTOList = finalInvoiceTO.InvoiceAddressTOList;
                            if (tblInvoiceAddressTOList != null && tblInvoiceAddressTOList.Count > 0)
                            {
                                TblInvoiceAddressTO tblBillingInvoiceAddressTO = tblInvoiceAddressTOList.Where(b => b.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                                billingStateId = tblBillingInvoiceAddressTO.StateId;

                            }

                            //To get Office address
                            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);

                            if (tblConfigParamsTO == null)
                            {
                                tran.Rollback();
                                resultMessage.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                                return resultMessage;
                            }

                            Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                            TblAddressTO ofcAddrTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
                            if (ofcAddrTO == null)
                            {
                                resultMessage.DefaultBehaviour("Address Not Found For Self Organization.");
                                return resultMessage;
                            }
                            if (billingStateId == ofcAddrTO.StateId)
                            {
                                isWithinState = true;
                            }

                            #region 2.1 To update invoices item details
                            if (finalInvoiceItemDetailsTOList != null && finalInvoiceItemDetailsTOList.Count > 0)
                            {

                                List<TblInvoiceItemDetailsTO> otherItemList = finalInvoiceItemDetailsTOList.Where(o => o.OtherTaxId != 0).ToList();
                                if (otherItemList != null && otherItemList.Count > 0)
                                {
                                    //TblInvoiceItemDetailsTO tblFreightInvoiceItemTO = new TblInvoiceItemDetailsTO();

                                    List<TblInvoiceItemDetailsTO> distinctOtherItemList = otherItemList.GroupBy(w => w.OtherTaxId).Select(s => s.FirstOrDefault()).ToList();
                                    if (distinctOtherItemList != null && distinctOtherItemList.Count > 0)
                                    {
                                        for (int m = 0; m < distinctOtherItemList.Count; m++)
                                        {

                                            List<TblInvoiceItemDetailsTO> tempInvoiceOtherList = otherItemList.Where(oi => oi.OtherTaxId == distinctOtherItemList[m].OtherTaxId).ToList();
                                            for (int n = 0; n < tempInvoiceOtherList.Count; n++)
                                            {
                                                TblInvoiceItemDetailsTO tblFreightInvoiceItemTO = tempInvoiceOtherList[n];

                                                if (n == 0)
                                                {
                                                    for (int j = 1; j < tempInvoiceOtherList.Count; j++)
                                                    {
                                                        TblInvoiceItemDetailsTO tblOtherItemTO = tempInvoiceOtherList[j];
                                                        tblFreightInvoiceItemTO.InvoiceQty += tblOtherItemTO.InvoiceQty;
                                                        if (tblOtherItemTO.InvoiceQty <= 1)
                                                            tblFreightInvoiceItemTO.Rate += tblOtherItemTO.Rate;

                                                        tblFreightInvoiceItemTO.TaxableAmt += tblOtherItemTO.TaxableAmt;

                                                    }
                                                    result = _iTblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(tblFreightInvoiceItemTO, conn, tran);
                                                    if (result != 1)
                                                    {
                                                        resultMessage.DefaultBehaviour("Error While UpdateTblInvoiceItemDetails");
                                                        return resultMessage;
                                                    }

                                                }
                                                else
                                                {
                                                    //Remove Item From List

                                                    finalInvoiceItemDetailsTOList = finalInvoiceItemDetailsTOList.Where(w => w.IdInvoiceItem != tblFreightInvoiceItemTO.IdInvoiceItem).ToList();

                                                    result = _iTblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetails(tblFreightInvoiceItemTO.IdInvoiceItem, conn, tran);
                                                    if (result == -1)
                                                    {
                                                        resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceItemDetails");
                                                        return resultMessage;
                                                    }


                                                }
                                            }

                                        }
                                    }
                                }

                                finalInvoiceTO.InvoiceItemDetailsTOList = finalInvoiceItemDetailsTOList;
                                RemoveIotFieldsFromDB(finalInvoiceTO);

                                for (int j = 0; j < finalInvoiceItemDetailsTOList.Count; j++)
                                {
                                    Double itemGrandTotal = 0;
                                    TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = finalInvoiceItemDetailsTOList[j];
                                    tblInvoiceItemDetailsTO.InvoiceId = finalInvoiceTO.IdInvoice;
                                    result = _iTblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                                    if (result != 1)
                                    {
                                        resultMessage.DefaultBehaviour("Error While UpdateTblInvoiceItemDetails");
                                        return resultMessage;
                                    }

                                    #region 2.1.1 to calculate invoice total

                                    basicTotal += tblInvoiceItemDetailsTO.BasicTotal;
                                    discountTotal += tblInvoiceItemDetailsTO.CdAmt;
                                    taxableTotal += tblInvoiceItemDetailsTO.TaxableAmt;
                                    itemGrandTotal += tblInvoiceItemDetailsTO.TaxableAmt;
                                    #endregion

                                    #region 2.1.2 delete previous item tax details
                                    List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOListTemp = tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList;
                                    //= _iTblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);
                                    if (tblInvoiceItemTaxDtlsTOListTemp != null && tblInvoiceItemTaxDtlsTOListTemp.Count > 0)
                                    {
                                        for (int invTax = 0; invTax < tblInvoiceItemTaxDtlsTOListTemp.Count; invTax++)
                                        {
                                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTOtemp = tblInvoiceItemTaxDtlsTOListTemp[invTax];
                                            result = _iTblInvoiceItemTaxDtlsBL.DeleteTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTOtemp.IdInvItemTaxDtl, conn, tran);
                                            if (result == -1)
                                            {
                                                resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                                return resultMessage;
                                            }
                                        }

                                    }
                                    #endregion

                                    #region 2.1.3 To added new  invoice tax details

                                    Boolean calcTax = true;

                                    if (tblInvoiceItemDetailsTO.OtherTaxId > 0)
                                    {
                                        TblOtherTaxesTO tblOtherTaxesTO = _iTblOtherTaxesDAO.SelectTblOtherTaxes(tblInvoiceItemDetailsTO.OtherTaxId, conn, tran);
                                        if (tblOtherTaxesTO != null)
                                        {
                                            if (tblOtherTaxesTO.IsAfter == 1)
                                            {
                                                calcTax = false;
                                            }
                                        }
                                    }

                                    List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = new List<TblInvoiceItemTaxDtlsTO>();


                                    if (calcTax)
                                    {
                                        TblGstCodeDtlsTO gstCodeDtlsTO = new TblGstCodeDtlsTO();
                                        TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(tblInvoiceItemDetailsTO.ProdGstCodeId, conn, tran);
                                        if (tblProdGstCodeDtlsTO != null)
                                        {
                                            gstCodeDtlsTO = _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                                            if (gstCodeDtlsTO != null)
                                            {
                                                gstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesDAO.SelectAllTblTaxRates(tblProdGstCodeDtlsTO.GstCodeId, conn, tran);
                                            }
                                        }



                                        foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
                                        {
                                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
                                            tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
                                            tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
                                            tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                                            tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;

                                            //Saket [2020-02-18] Added SEZ conditons.
                                            if (finalInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                                            {
                                                tblInvoiceItemTaxDtlsTO.TaxRatePct = 0;
                                                tblInvoiceItemTaxDtlsTO.TaxableAmt = 0;
                                            }

                                            tblInvoiceItemTaxDtlsTO.TaxAmt = (tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
                                            tblInvoiceItemTaxDtlsTO.TaxTypeId = taxRateTo.TaxTypeId;
                                            if (isWithinState)
                                            {
                                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                                {
                                                    cgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                                }
                                                else if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                                {
                                                    sgstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                                }

                                            }
                                            else
                                            {

                                                if (taxRateTo.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                                {
                                                    igstTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                                    itemGrandTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                                    tblInvoiceItemTaxDtlsTOList.Add(tblInvoiceItemTaxDtlsTO);
                                                }
                                            }

                                        }
                                    }
                                    grandTotal += itemGrandTotal;

                                    for (int k = 0; k < tblInvoiceItemTaxDtlsTOList.Count; k++)
                                    {
                                        TblInvoiceItemTaxDtlsTO tempInvoiceItemTaxDtlsTo = tblInvoiceItemTaxDtlsTOList[k];
                                        tempInvoiceItemTaxDtlsTo.InvoiceItemId = tblInvoiceItemDetailsTO.IdInvoiceItem;
                                        result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tempInvoiceItemTaxDtlsTo, conn, tran);
                                        if (result != 1)
                                        {
                                            resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                            return resultMessage;
                                        }
                                    }
                                    #endregion

                                    #region 2.1.4 To update invoice history

                                    TblInvoiceHistoryTO tblInvoiceHistoryTO = _iTblInvoiceHistoryBL.SelectTblInvoiceHistoryTOByInvoiceItemId(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);
                                    if (tblInvoiceHistoryTO != null)
                                    {
                                        tblInvoiceHistoryTO.InvoiceId = finalInvoiceTO.IdInvoice;
                                        result = _iTblInvoiceHistoryBL.UpdateTblInvoiceHistoryById(tblInvoiceHistoryTO, conn, tran);
                                        if (result != 1)
                                        {
                                            resultMessage.DefaultBehaviour("Error While UpdateTblInvoiceHistoryById");
                                            return resultMessage;
                                        }
                                    }


                                    #endregion
                                }


                            }
                            #endregion


                            #region 2.2 To update invoices loading mapping 

                            if (tempLoadingSlipInvoiceTOList != null && tempLoadingSlipInvoiceTOList.Count > 0)
                            {
                                for (int k = 0; k < tempLoadingSlipInvoiceTOList.Count; k++)
                                {
                                    TempLoadingSlipInvoiceTO loadingSlipInvoiceTO = tempLoadingSlipInvoiceTOList[k];
                                    loadingSlipInvoiceTO.InvoiceId = finalInvoiceTO.IdInvoice;
                                    loadingSlipInvoiceTO.UpdatedBy = loginUserId;
                                    loadingSlipInvoiceTO.UpdatedOn = serverDate;
                                    result = _iTempLoadingSlipInvoiceBL.UpdateTempLoadingSlipInvoice(loadingSlipInvoiceTO, conn, tran);
                                    if (result != 1)
                                    {
                                        resultMessage.DefaultBehaviour("Error While UpdateTempLoadingSlipInvoice");
                                        return resultMessage;
                                    }

                                }
                            }
                            #endregion


                            #region 2.2 To update invoices document details

                            if (tempInvoiceDocumentDetailsTOList != null && tempInvoiceDocumentDetailsTOList.Count > 0)
                            {
                                for (int k = 0; k < tempInvoiceDocumentDetailsTOList.Count; k++)
                                {
                                    TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = tempInvoiceDocumentDetailsTOList[k];
                                    tempInvoiceDocumentDetailsTO.InvoiceId = finalInvoiceTO.IdInvoice;
                                    tempInvoiceDocumentDetailsTO.UpdatedBy = loginUserId;
                                    tempInvoiceDocumentDetailsTO.UpdatedOn = serverDate;
                                    result = _iTempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
                                    if (result != 1)
                                    {
                                        resultMessage.DefaultBehaviour("Error While UpdateTempInvoiceDocumentDetails");
                                        return resultMessage;
                                    }

                                }
                            }
                            #endregion

                            #region 2.3 Update main Invoice
                            finalInvoiceTO.TaxableAmt = taxableTotal;
                            finalInvoiceTO.DiscountAmt = discountTotal;
                            finalInvoiceTO.IgstAmt = igstTotal;
                            finalInvoiceTO.CgstAmt = cgstTotal;
                            finalInvoiceTO.SgstAmt = sgstTotal;
                            finalInvoiceTO.BasicAmt = basicTotal;

                            //double finalGrandTotal = Math.Round(grandTotal);
                            //finalInvoiceTO.GrandTotal = finalGrandTotal;
                            //finalInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);

                            RoundOffInvoiceValuesBySetting(finalInvoiceTO);

                            finalInvoiceTO.UpdatedBy = loginUserId;
                            finalInvoiceTO.UpdatedOn = serverDate;
                            finalInvoiceTO.StatusId = (int)Constants.InvoiceStatusE.NEW;
                            finalInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.AUTO_INVOICE_EDIT;
                            finalInvoiceTO.InvoiceModeId = (int)Constants.InvoiceModeE.AUTO_INVOICE_EDIT;
                            if (finalInvoiceTO.TdsAmt != null && finalInvoiceTO.TdsAmt > 0 && finalInvoiceTO.IsConfirmed == 1 && finalInvoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                            {
                                Double tdsTaxPct = 0;
                                TblConfigParamsTO tdsConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DELIVER_INVOICE_TDS_TAX_PCT, conn, tran);
                                if (tdsConfigParamsTO != null)
                                {
                                    if (!String.IsNullOrEmpty(tdsConfigParamsTO.ConfigParamVal))
                                    {
                                        tdsTaxPct = Convert.ToDouble(tdsConfigParamsTO.ConfigParamVal);
                                    }
                                }
                                finalInvoiceTO.TdsAmt = (CalculateTDS(finalInvoiceTO) * tdsTaxPct) / 100;
                                finalInvoiceTO.TdsAmt = Math.Ceiling(finalInvoiceTO.TdsAmt);
                            }
                            result = UpdateTblInvoice(finalInvoiceTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error While UpdateTblInvoice");
                                return resultMessage;
                            }

                            #endregion

                        }

                        else
                        {
                            #region 2.4 To delete previous invoices
                            resultMessage = DeleteTblInvoiceDetails(tblInvoiceTOList[f], conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return resultMessage;
                            }
                            #endregion

                        }

                    }
                }
                #endregion


                resultMessage.DefaultSuccessBehaviour();
                resultMessage.DisplayMessage = "Success..Invoice combined";
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ComposeInvoice");
                return resultMessage;
            }

        }

        public ResultMessage DecomposeInvoice(List<Int32> invoiceIdsList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMessage = DecomposeInvoice(invoiceIdsList, loginUserId, conn, tran);

                if (resultMessage != null && resultMessage.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.DisplayMessage = "Success..Invoice decomposed";
                    resultMessage.Tag = null;
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "mergeInvoices");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        private ResultMessage DecomposeInvoice(List<int> invoiceIdsList, int loginUserId, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage resultMessage = new ResultMessage();

            List<TblInvoiceTO> tblInvoiceTOList = new List<TblInvoiceTO>();
            List<TempInvoiceDocumentDetailsTO> tempInvoiceDocumentDetailsTOList = new List<TempInvoiceDocumentDetailsTO>();
            #region 1.Get data
            if (invoiceIdsList != null && invoiceIdsList.Count > 0)
            {
                for (int i = 0; i < invoiceIdsList.Count; i++)
                {
                    Int32 invoiceId = invoiceIdsList[i];

                    //invoice document details list

                    List<TempInvoiceDocumentDetailsTO> newTempInvoiceDocumentTOList = _iTempInvoiceDocumentDetailsDAO.SelectTempInvoiceDocumentDetailsByInvoiceId(invoiceId, conn, tran);

                    tempInvoiceDocumentDetailsTOList.AddRange(newTempInvoiceDocumentTOList);

                    //existing invoiceTO with item details

                    TblInvoiceTO invoiceTO = SelectTblInvoiceTOWithDetails(invoiceId, conn, tran);

                    if (invoiceTO == null)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("invoiceTO Found NULL");
                        return resultMessage;
                    }
                    tblInvoiceTOList.Add(invoiceTO);

                    invoiceTO.TempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId, conn, tran);

                    #region Validate Invoice to Decompose

                    if (invoiceTO.TempLoadingSlipInvoiceTOList == null || invoiceTO.TempLoadingSlipInvoiceTOList.Count <= 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("No different loading slip found to decompose invoice");
                        return resultMessage;
                    }

                    List<TempLoadingSlipInvoiceTO> tempDist = invoiceTO.TempLoadingSlipInvoiceTOList.GroupBy(g => g.LoadingSlipId).Select(s => s.FirstOrDefault()).ToList();
                    if (tempDist == null || tempDist.Count <= 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("No different loading slip found to decompose invoice");
                        return resultMessage;
                    }



                    #endregion

                }

            }
            #endregion





            #region 2. Processing on data to merge invoices
            if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
            {
                //Delete all Invoice first.
                for (int i = 0; i < tblInvoiceTOList.Count; i++)
                {

                    TblInvoiceTO tblInvoiceTO = tblInvoiceTOList[i];

                    resultMessage = DeleteTblInvoiceDetails(tblInvoiceTO, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }

                    List<TblLoadingTO> tblLoadingTOList = new List<TblLoadingTO>();


                    for (int j = 0; j < tblInvoiceTO.TempLoadingSlipInvoiceTOList.Count; j++)
                    {

                        TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = tblInvoiceTO.TempLoadingSlipInvoiceTOList[j];

                        TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tempLoadingSlipInvoiceTO.LoadingSlipId, conn, tran);
                        if (tblLoadingSlipTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("tblLoadingSlipTO == null  for loadingSlipId = " + tempLoadingSlipInvoiceTO.LoadingSlipId);
                            return resultMessage;
                        }


                        TblLoadingTO tblLoadingTO = _iTblLoadingDAO.SelectTblLoading(tblLoadingSlipTO.LoadingId, conn, tran);
                        if (tblLoadingTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("tblLoadingTO == null  for LoadingId = " + tblLoadingSlipTO.LoadingId);
                            return resultMessage;
                        }

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

                    int result = 0;
                    for (int k = 0; k < tblLoadingTOList.Count; k++)
                    {
                        TblLoadingTO tblLoadingTO = tblLoadingTOList[k];

                        tblLoadingTO.CallFlag = 1;

                        resultMessage = CreateInvoiceAgainstLoadingSlips(tblLoadingTO, conn, tran, tblLoadingTO.LoadingSlipList, 1);
                        if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                        else
                        {
                            if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblInvoiceTO>))
                            {
                                List<TblInvoiceTO> tblinvoiceTOList = (List<TblInvoiceTO>)resultMessage.Tag;
                                if (tblinvoiceTOList != null && tblinvoiceTOList.Count > 0)
                                {
                                    TblInvoiceTO tempInvoiceTO = tblinvoiceTOList[0];
                                    if (tempInvoiceDocumentDetailsTOList != null && tempInvoiceDocumentDetailsTOList.Count > 0)
                                    {
                                        for (int t = 0; t < tempInvoiceDocumentDetailsTOList.Count; t++)
                                        {
                                            TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = tempInvoiceDocumentDetailsTOList[t];
                                            tempInvoiceDocumentDetailsTO.InvoiceId = tempInvoiceTO.IdInvoice;
                                            result = _iTempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
                                            if (result != 1)
                                            {
                                                resultMessage.DefaultBehaviour("Error While UpdateTempInvoiceDocumentDetails");
                                                return resultMessage;
                                            }

                                        }
                                    }
                                }

                            }

                        }
                    }

                }




            }
            #endregion


            resultMessage.DefaultSuccessBehaviour();
            resultMessage.DisplayMessage = "Success..Invoice Decomposed";
            return resultMessage;
        }


        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        public ResultMessage SaveInvoiceDocumentDetails(TblInvoiceTO invoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMSg = InsertInvoiceDocumentDetails(invoiceTO, tblDocumentDetailsTOList, loginUserId, conn, tran);
                if (resultMSg.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMSg.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();
                }
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        public ResultMessage InsertInvoiceDocumentDetails(TblInvoiceTO tblInvoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            try
            {
                #region 1. Save the Document Details

                resultMessage = _iTblDocumentDetailsBL.UploadDocumentWithConnTran(tblDocumentDetailsTOList, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    resultMessage.DefaultBehaviour("Error To Upload Documenrt");
                    return resultMessage;
                }

                #endregion

                #region 2. Save the Invoice Document Linking 
                if (tblInvoiceTO == null)
                {
                    resultMessage.DefaultBehaviour("Error : InvoiceTO Found Empty Or Null");
                    return resultMessage;
                }
                TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = new TempInvoiceDocumentDetailsTO();
                if (resultMessage != null && resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblDocumentDetailsTO>))
                {
                    List<TblDocumentDetailsTO> tblDocumentDetailsTOListTemp = (List<TblDocumentDetailsTO>)resultMessage.Tag;
                    if (tblDocumentDetailsTOListTemp == null && tblDocumentDetailsTOListTemp.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Error : Document List Found Empty Or Null");
                        return resultMessage;
                    }

                    DateTime serverDateTime = _iCommon.ServerDateTime;
                    tempInvoiceDocumentDetailsTO.DocumentId = tblDocumentDetailsTOListTemp[0].IdDocument;
                    tempInvoiceDocumentDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;
                    tempInvoiceDocumentDetailsTO.CreatedBy = loginUserId;
                    tempInvoiceDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                    tempInvoiceDocumentDetailsTO.IsActive = 1;
                    result = _iTempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour("Error in Insert InvoiceTbl");
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultBehaviour("Error To Upload Documenrt");
                    return resultMessage;
                }



                #endregion



                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblInvoice");
                return resultMessage;
            }
            finally
            {

            }

        }

        //Vijaymala [2018-10-15] Print invoice details
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public ResultMessage PrintReport(Int32 invoiceId, Boolean isPrinted = false, Boolean isSendEmailForInvoice = false, Boolean isFileDelete = true)
        {
            ResultMessage resultMessage = new ResultMessage();
            String response = String.Empty;
            String signedQRCode = String.Empty;
            Int32 apiId = (int)EInvoiceAPIE.GENERATE_EINVOICE;
            byte[] PhotoCodeInBytes = null;
            try
            {
               // resultMessage = PrintReportV2(invoiceId, true, false, false);

                TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTOWithDetails(invoiceId);

                DataSet printDataSet = new DataSet();

                //headerDT
                DataTable headerDT = new DataTable();
                DataTable addressDT = new DataTable();
                DataTable invoiceDT = new DataTable();
                DataTable invoiceItemDT = new DataTable();
                DataTable itemFooterDetailsDT = new DataTable();
                DataTable commercialDT = new DataTable();
                DataTable hsnItemTaxDT = new DataTable();
                DataTable qrCodeDT = new DataTable();
                // DataTable shippingAddressDT = new DataTable();
                //Aniket [1-02-2019] added to create multiple copy of tax invoice
                DataTable multipleInvoiceCopyDT = new DataTable();
                headerDT.TableName = "headerDT";
                invoiceDT.TableName = "invoiceDT";
                addressDT.TableName = "addressDT";
                invoiceItemDT.TableName = "invoiceItemDT";
                itemFooterDetailsDT.TableName = "itemFooterDetailsDT";
                hsnItemTaxDT.TableName = "hsnItemTaxDT";
                // shippingAddressDT.TableName = "shippingAddressDT";
                multipleInvoiceCopyDT.TableName = "multipleInvoiceCopyDT";
                qrCodeDT.TableName = "QRCodeDT";
                //HeaderDT 
                multipleInvoiceCopyDT.Columns.Add("idInvoiceCopy");
                multipleInvoiceCopyDT.Columns.Add("invoiceCopyName");

                //Aniket [13-02-2019] to display payment term option on print invoice
                string paymentTermAllCommaSeparated = "";
                List<DropDownTO> multipleInvoiceCopyList = _iDimensionBL.SelectInvoiceCopyList();

                if (multipleInvoiceCopyList != null)
                {
                    for (int i = 0; i < multipleInvoiceCopyList.Count; i++)
                    {
                        DropDownTO multipleInvoiceCopyTO = multipleInvoiceCopyList[i];
                        multipleInvoiceCopyDT.Rows.Add();
                        Int32 invoiceCopyDTCount = multipleInvoiceCopyDT.Rows.Count - 1;
                        multipleInvoiceCopyDT.Rows[invoiceCopyDTCount]["idInvoiceCopy"] = multipleInvoiceCopyTO.Value;
                        multipleInvoiceCopyDT.Rows[invoiceCopyDTCount]["invoiceCopyName"] = multipleInvoiceCopyTO.Text;

                    }
                }

                int defaultCompOrgId = 0;

                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (configParamsTO != null)
                    {
                        defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
                    }
                }
                else
                {
                    defaultCompOrgId = tblInvoiceTO.InvFromOrgId;
                }
                TblOrganizationTO organizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(defaultCompOrgId);

                //Aniket [06-03-2019] added to check whether Math.Round() function should include in tax calculation or not
                int isMathRoundoff = 0;
                TblConfigParamsTO tblconfigParamForMathRound = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_TAX_ON_PRINT_INVOICE);
                if (tblconfigParamForMathRound != null)
                {
                    if (tblconfigParamForMathRound.ConfigParamVal == "1")
                    {
                        isMathRoundoff = 1;
                    }
                }

                qrCodeDT.Columns.Add("QRCode", typeof(System.Byte[]));
                qrCodeDT.Columns.Add("QRCodeForPrint", typeof(System.Byte[]));

                qrCodeDT.Rows.Add();
                string AckNo = "";
                response = _iTblInvoiceDAO.SelectresponseForPhotoInReport(invoiceId, apiId);
                if (!String.IsNullOrEmpty(response))
                {
                    JObject json = JObject.Parse(response);

                    if (json.ContainsKey("data"))
                    {
                        JObject jsonData = JObject.Parse(json["data"].ToString());

                        if (jsonData.ContainsKey("SignedQRCode"))
                        {
                            signedQRCode = (string)jsonData["SignedQRCode"];
                        }
                        if (jsonData.ContainsKey("AckNo"))
                        {
                            AckNo = (string)jsonData["AckNo"];
                        }
                    }
                }

                if (!String.IsNullOrEmpty(signedQRCode))
                {
                    PhotoCodeInBytes = _iCommon.convertQRStringToByteArray(signedQRCode);
                }

                if (PhotoCodeInBytes != null)
                    qrCodeDT.Rows[0]["QRCode"] = PhotoCodeInBytes;

                //Reshma Added For QR Code Image
                String QrCodeImageFilePath = "";
                TblConfigParamsTO tblconfigParamForQRCodeImage = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.Invoice_payment_QR_CODE_Image_file_Path);
                if (tblconfigParamForQRCodeImage != null)
                {
                    if (!string .IsNullOrEmpty ( tblconfigParamForQRCodeImage.ConfigParamVal))
                    {
                        QrCodeImageFilePath = tblconfigParamForQRCodeImage.ConfigParamVal;
                    }
                }
                byte[] imageByteArray = null;
                if (!string.IsNullOrEmpty(QrCodeImageFilePath))
                {
                    //byte[] imageByteArray = null;
                    FileStream fileStream = new FileStream(QrCodeImageFilePath, FileMode.Open, FileAccess.Read);
                    using (BinaryReader reader = new BinaryReader(fileStream))
                    {
                        imageByteArray = new byte[reader.BaseStream.Length];
                        for (int i = 0; i < reader.BaseStream.Length; i++)
                            imageByteArray[i] = reader.ReadByte();
                    }
                    if(imageByteArray !=null)
                        qrCodeDT.Rows[0]["QRCodeForPrint"] = imageByteArray;
                }
             
                //HeaderDT 
                //headerDT.Columns.Add("orgFirmName");
                invoiceDT.Columns.Add("orgVillageNm");
                invoiceDT.Columns.Add("orgPhoneNo");
                invoiceDT.Columns.Add("orgFaxNo");
                invoiceDT.Columns.Add("orgEmailAddr");
                invoiceDT.Columns.Add("orgWebsite");
                invoiceDT.Columns.Add("orgAddr");
                invoiceDT.Columns.Add("orgCinNo");
                invoiceDT.Columns.Add("orgGstinNo");
                invoiceDT.Columns.Add("orgPanNo");   //Twicie
                invoiceDT.Columns.Add("orgState");
                invoiceDT.Columns.Add("orgStateCode");

                invoiceDT.Columns.Add("plotNo");
                invoiceDT.Columns.Add("streetName");

                invoiceDT.Columns.Add("areaName");
                invoiceDT.Columns.Add("district");
                invoiceDT.Columns.Add("pinCode");


                invoiceDT.Columns.Add("orgFirmName");
                invoiceDT.Columns.Add("hsnNo");
                invoiceDT.Columns.Add("panNo");
                invoiceDT.Columns.Add("paymentTerm");
                invoiceDT.Columns.Add("poNo");
                invoiceDT.Columns.Add("poDateStr");

                headerDT.Columns.Add("poNo");
                headerDT.Columns.Add("poDateStr");
                invoiceDT.Columns.Add("DeliveryNoteNo");
                invoiceDT.Columns.Add("DispatchDocNo");

                invoiceDT.Columns.Add("TotalTaxAmt", typeof(double));
                invoiceDT.Columns.Add("TotalTaxAmtWordStr");
                //chetan[14-feb-2020] added
                invoiceDT.Columns.Add("BookingCDPct", typeof(double));
                invoiceDT.Columns.Add("BookingBasicRate", typeof(double));
                headerDT.Columns.Add("AckNo");
                invoiceDT.Columns.Add("AckNo");
                headerDT.Columns.Add("BrokerName");
                invoiceDT.Columns.Add("BrokerName");

                headerDT.Columns.Add("loadingCharges");
                invoiceDT.Columns.Add("loadingCharges");
                TblAddressTO tblAddressTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(organizationTO.IdOrganization, Constants.AddressTypeE.OFFICE_ADDRESS);
                List<DropDownTO> stateList = _iDimensionBL.SelectStatesForDropDown(0);
                if (organizationTO != null)
                {
                    headerDT.Rows.Add();
                    invoiceDT.Rows.Add();
                    //headerDT.Rows[0]["orgFirmName"] = organizationTO.FirmName;
                    invoiceDT.Rows[0]["orgFirmName"] = organizationTO.FirmName;

                    invoiceDT.Rows[0]["orgPhoneNo"] = organizationTO.PhoneNo;
                    invoiceDT.Rows[0]["orgFaxNo"] = organizationTO.FaxNo;
                    invoiceDT.Rows[0]["orgWebsite"] = organizationTO.Website;
                    invoiceDT.Rows[0]["orgEmailAddr"] = organizationTO.EmailAddr;
                }
                //if (string.IsNullOrEmpty(AckNo))//Reshma[24-06-2022] Added Acknowledgement Number DT creation to print on simpli invoice
                    headerDT.Rows[0]["AckNo"] = AckNo;
                    invoiceDT.Rows[0]["AckNo"] = AckNo;

                //chetan[14-feb-2020]
                TblBookingsTO tblBookingsTO = _iTblBookingsBL.SelectBookingsDetailsFromInVoiceId(tblInvoiceTO.IdInvoice);
                if (tblBookingsTO != null)
                {
                    invoiceDT.Rows[0]["BookingCDPct"] = tblBookingsTO.CdStructure;
                    invoiceDT.Rows[0]["BookingBasicRate"] = tblBookingsTO.BookingRate;
                }
                List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = _iTblPaymentTermsForBookingBL.SelectAllTblPaymentTermsForBookingFromBookingId(0, invoiceId);
                if (tblPaymentTermsForBookingTOList != null)
                {
                    foreach (var item in tblPaymentTermsForBookingTOList)
                    {

                        invoiceDT.Columns.Add(item.PaymentTerm);
                        //headerDT.Columns.Add(item.PaymentTerm);

                        if (item.PaymentTermOptionList != null && item.PaymentTermOptionList.Count > 0)
                        {
                            foreach (var x in item.PaymentTermOptionList)
                            {
                                if (x.IsSelected == 1)
                                {

                                    String tempPayment = x.PaymentTermOption;
                                    if (x.IsDescriptive == 1)
                                    {
                                        tempPayment = x.PaymentTermsDescription;
                                    }


                                    paymentTermAllCommaSeparated += tempPayment + ",";

                                    invoiceDT.Rows[0][item.PaymentTerm] = tempPayment;
                                    //headerDT.Rows[0][item.PaymentTerm] = x.PaymentTermOption;
                                }
                            }

                        }


                    }
                }

                if (!String.IsNullOrEmpty(paymentTermAllCommaSeparated))
                {
                    paymentTermAllCommaSeparated = paymentTermAllCommaSeparated.TrimEnd(',');
                }

                if (tblAddressTO != null)
                {
                    String orgAddrStr = String.Empty;
                    if (!String.IsNullOrEmpty(tblAddressTO.PlotNo))
                    {
                        orgAddrStr += tblAddressTO.PlotNo;
                        invoiceDT.Rows[0]["plotNo"] = tblAddressTO.PlotNo;
                    }

                    if (!String.IsNullOrEmpty(tblAddressTO.StreetName))
                    {
                        orgAddrStr += " " + tblAddressTO.StreetName;
                        invoiceDT.Rows[0]["streetName"] = tblAddressTO.StreetName;
                    }

                    if (!String.IsNullOrEmpty(tblAddressTO.AreaName))
                    {
                        orgAddrStr += " " + tblAddressTO.AreaName;
                        invoiceDT.Rows[0]["areaName"] = tblAddressTO.AreaName;
                    }
                    if (!String.IsNullOrEmpty(tblAddressTO.DistrictName))
                    {
                        orgAddrStr += " " + tblAddressTO.DistrictName;
                        invoiceDT.Rows[0]["district"] = tblAddressTO.DistrictName;

                    }
                    if (tblAddressTO.Pincode > 0)
                    {
                        orgAddrStr += "-" + tblAddressTO.Pincode;
                        invoiceDT.Rows[0]["pinCode"] = tblAddressTO.Pincode;

                    }
                    invoiceDT.Rows[0]["orgVillageNm"] = tblAddressTO.VillageName + "-" + tblAddressTO.Pincode;
                    invoiceDT.Rows[0]["orgAddr"] = orgAddrStr;
                    invoiceDT.Rows[0]["orgState"] = tblAddressTO.StateName;

                    if (stateList != null && stateList.Count > 0)
                    {
                        DropDownTO stateTO = stateList.Where(ele => ele.Value == tblAddressTO.StateId).FirstOrDefault();
                        if (stateTO != null)
                        {

                            invoiceDT.Rows[0]["orgStateCode"] = stateTO.Tag;
                        }
                    }



                }
                List<TblOrgLicenseDtlTO> orgLicenseList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(defaultCompOrgId);

                if (orgLicenseList != null && orgLicenseList.Count > 0)
                {

                    //CIN Number
                    var cinNo = orgLicenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.CIN_NO).FirstOrDefault();
                    if (cinNo != null)
                    {
                        invoiceDT.Rows[invoiceDT.Rows.Count - 1]["orgCinNo"] = cinNo.LicenseValue;
                    }
                    //GSTIN Number
                    var gstinNo = orgLicenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (gstinNo != null)
                    {
                        invoiceDT.Rows[invoiceDT.Rows.Count - 1]["orgGstinNo"] = gstinNo.LicenseValue;
                    }
                    //PAN Number
                    var panNo = orgLicenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    if (panNo != null)
                    {
                        invoiceDT.Rows[invoiceDT.Rows.Count - 1]["orgPanNo"] = panNo.LicenseValue;
                        invoiceDT.Rows[0]["panNo"] = panNo.LicenseValue;

                    }
                }

                //InvoiceDT

                if (tblInvoiceTO != null)
                {

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        tblInvoiceTO.VehicleNo = tblInvoiceTO.VehicleNo.ToUpper();
                    }

                    invoiceDT.Columns.Add("invoiceNo");
                    invoiceDT.Columns.Add("invoiceDateStr");
                    //headerDT.Columns.Add("deliveryLocation");
                    invoiceDT.Columns.Add("EWayBillNo"); //Aniket [26-6-2019]

                    invoiceDT.Rows[0]["invoiceNo"] = tblInvoiceTO.InvoiceNo;
                    invoiceDT.Rows[0]["invoiceDateStr"] = tblInvoiceTO.InvoiceDateStr;
                    if (!string.IsNullOrEmpty(tblInvoiceTO.ElectronicRefNo))
                        invoiceDT.Rows[0]["EWayBillNo"] = tblInvoiceTO.ElectronicRefNo;

                    //Dhananjay [25-11-2020]
                    invoiceDT.Columns.Add("IrnNo");
                    if (!string.IsNullOrEmpty(tblInvoiceTO.IrnNo))
                        invoiceDT.Rows[0]["IrnNo"] = tblInvoiceTO.IrnNo;

                    addressDT.Columns.Add("poNo");
                    addressDT.Columns.Add("poDateStr");
                    addressDT.Columns.Add("electronicRefNo");


                    commercialDT = getCommercialDT(tblInvoiceTO); //for SRJ
                    hsnItemTaxDT = getHsnItemTaxDT(tblInvoiceTO); //for Parameshwar
                    commercialDT.TableName = "commercialDT";



                    invoiceDT.Columns.Add("discountAmt", typeof(double));
                    invoiceDT.Columns.Add("discountAmtStr");

                    invoiceDT.Columns.Add("freightAmt", typeof(double));
                    invoiceDT.Columns.Add("pfAmt", typeof(double));
                    invoiceDT.Columns.Add("cessAmt", typeof(double));
                    invoiceDT.Columns.Add("afterCessAmt", typeof(double));
                    invoiceDT.Columns.Add("insuranceAmt", typeof(double));


                    invoiceDT.Columns.Add("taxableAmt", typeof(double));
                    invoiceDT.Columns.Add("taxableAmtStr");
                    invoiceDT.Columns.Add("cgstAmt", typeof(double));
                    invoiceDT.Columns.Add("sgstAmt", typeof(double));
                    invoiceDT.Columns.Add("igstAmt", typeof(double));
                    invoiceDT.Columns.Add("grandTotal", typeof(double));

                    invoiceDT.Columns.Add("cgstTotalStr");//r
                    invoiceDT.Columns.Add("sgstTotalStr");//r
                    invoiceDT.Columns.Add("igstTotalStr");//r
                    invoiceDT.Columns.Add("grandTotalStr");//r

                    invoiceDT.Columns.Add("grossWeight", typeof(double));
                    invoiceDT.Columns.Add("tareWeight", typeof(double));
                    invoiceDT.Columns.Add("netWeight", typeof(double));


                    //headerDT.Columns.Add("vehicleNo");
                    //headerDT.Columns.Add("lrNumber");
                    invoiceDT.Columns.Add("vehicleNo");
                    invoiceDT.Columns.Add("transporterName");
                    invoiceDT.Columns.Add("Narration");
                    headerDT.Columns.Add("Narration");

                    invoiceDT.Columns.Add("distributorName");
                    headerDT.Columns.Add("distributorName");

                    invoiceDT.Columns.Add("freightCategory");
                    headerDT.Columns.Add("freightCategory");

                    invoiceDT.Columns.Add("deliveryLocation");
                    invoiceDT.Columns.Add("lrNumber");
                    invoiceDT.Columns.Add("disPer", typeof(double));
                    invoiceDT.Columns.Add("roundOff", typeof(double));
                    invoiceDT.Columns.Add("taxTotal", typeof(double));
                    invoiceDT.Columns.Add("totalQty", typeof(double));
                    invoiceDT.Columns.Add("totalBundles");
                    invoiceDT.Columns.Add("totalBasicAmt", typeof(double));
                    invoiceDT.Columns.Add("bankName");
                    invoiceDT.Columns.Add("accountNo");
                    invoiceDT.Columns.Add("branchName");
                    invoiceDT.Columns.Add("ifscCode");
                    invoiceDT.Columns.Add("taxTotalStr");//r
                    invoiceDT.Columns.Add("declaration");//r
                    invoiceDT.Columns.Add("grossWtTakenDate");
                    invoiceDT.Columns.Add("preparationDate");

                    Double totalTaxAmt, cgstAmt, sgstAmt, igstAmt = 0;

                    cgstAmt = Math.Round(tblInvoiceTO.CgstAmt, 2);
                    sgstAmt = Math.Round(tblInvoiceTO.SgstAmt, 2);
                    igstAmt = Math.Round(tblInvoiceTO.IgstAmt, 2);

                    totalTaxAmt = Math.Round(cgstAmt + sgstAmt + igstAmt, 2);

                    invoiceDT.Rows[0]["TotalTaxAmt"] = totalTaxAmt;
                    invoiceDT.Rows[0]["TotalTaxAmtWordStr"] = currencyTowords(totalTaxAmt, tblInvoiceTO.CurrencyId); ;

                    invoiceDT.Rows[0]["DeliveryNoteNo"] = tblInvoiceTO.DeliveryNoteNo;
                    invoiceDT.Rows[0]["DispatchDocNo"] = tblInvoiceTO.DispatchDocNo;
                    //if (isMathRoundoff == 1)
                    if (isMathRoundoff == 1)  //Not applicable as each value will round off upto 2
                    {
                        invoiceDT.Rows[0]["discountAmt"] = tblInvoiceTO.DiscountAmt;
                        invoiceDT.Rows[0]["discountAmtStr"] = currencyTowords(tblInvoiceTO.DiscountAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["taxableAmt"] = tblInvoiceTO.TaxableAmt;
                        invoiceDT.Rows[0]["taxableAmtStr"] = currencyTowords(tblInvoiceTO.TaxableAmt, tblInvoiceTO.CurrencyId);


                        invoiceDT.Rows[0]["cgstAmt"] = tblInvoiceTO.CgstAmt;
                        invoiceDT.Rows[0]["cgstTotalStr"] = currencyTowords(tblInvoiceTO.CgstAmt, tblInvoiceTO.CurrencyId);
                        invoiceDT.Rows[0]["sgstAmt"] = tblInvoiceTO.SgstAmt;
                        invoiceDT.Rows[0]["sgstTotalStr"] = currencyTowords(tblInvoiceTO.SgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["igstAmt"] = tblInvoiceTO.IgstAmt;

                        invoiceDT.Rows[0]["igstTotalStr"] = currencyTowords(tblInvoiceTO.IgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["grandTotal"] = tblInvoiceTO.GrandTotal;
                        invoiceDT.Rows[0]["grandTotalStr"] = currencyTowords(tblInvoiceTO.GrandTotal, tblInvoiceTO.CurrencyId);


                        //invoiceDT.Rows[0]["grossWeight"] = tblInvoiceTO.GrossWeight / 1000;
                        //invoiceDT.Rows[0]["tareWeight"] = tblInvoiceTO.TareWeight / 1000;
                        //invoiceDT.Rows[0]["netWeight"] = tblInvoiceTO.NetWeight / 1000;
                        //if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                        //{
                        //    headerDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //    invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //}

                        //invoiceDT.Rows[0]["transporterName"] = tblInvoiceTO.TransporterName;
                        //invoiceDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //headerDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //invoiceDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        //headerDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        invoiceDT.Rows[0]["disPer"] = getDiscountPerct(tblInvoiceTO);
                        invoiceDT.Rows[0]["roundOff"] = tblInvoiceTO.RoundOffAmt;
                    }
                    else
                    {
                        invoiceDT.Rows[0]["discountAmt"] = Math.Round(tblInvoiceTO.DiscountAmt, 2);
                        invoiceDT.Rows[0]["discountAmtStr"] = currencyTowords(tblInvoiceTO.DiscountAmt, tblInvoiceTO.CurrencyId);
                        invoiceDT.Rows[0]["taxableAmt"] = Math.Round(tblInvoiceTO.TaxableAmt, 2);
                        invoiceDT.Rows[0]["taxableAmtStr"] = currencyTowords(tblInvoiceTO.TaxableAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["cgstAmt"] = Math.Round(tblInvoiceTO.CgstAmt, 2);
                        invoiceDT.Rows[0]["cgstTotalStr"] = currencyTowords(tblInvoiceTO.CgstAmt, tblInvoiceTO.CurrencyId);
                        invoiceDT.Rows[0]["sgstAmt"] = Math.Round(tblInvoiceTO.SgstAmt, 2);
                        invoiceDT.Rows[0]["sgstTotalStr"] = currencyTowords(tblInvoiceTO.SgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["igstAmt"] = Math.Round(tblInvoiceTO.IgstAmt, 2);

                        invoiceDT.Rows[0]["igstTotalStr"] = currencyTowords(tblInvoiceTO.IgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["grandTotal"] = Math.Round(tblInvoiceTO.GrandTotal, 2);
                        invoiceDT.Rows[0]["grandTotalStr"] = currencyTowords(tblInvoiceTO.GrandTotal, tblInvoiceTO.CurrencyId);


                        //invoiceDT.Rows[0]["grossWeight"] = Math.Round(tblInvoiceTO.GrossWeight / 1000, 3);
                        //invoiceDT.Rows[0]["tareWeight"] = Math.Round(tblInvoiceTO.TareWeight / 1000, 3);
                        //invoiceDT.Rows[0]["netWeight"] = Math.Round(tblInvoiceTO.NetWeight / 1000, 3);
                        //if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                        //{
                        //    headerDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //    invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //}
                        //invoiceDT.Rows[0]["transporterName"] = tblInvoiceTO.TransporterName;
                        //invoiceDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //headerDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //invoiceDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        //headerDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        invoiceDT.Rows[0]["disPer"] = Math.Round(getDiscountPerct(tblInvoiceTO), 2);
                        invoiceDT.Rows[0]["roundOff"] = Math.Round(tblInvoiceTO.RoundOffAmt, 2);

                    }

                    invoiceDT.Rows[0]["grossWeight"] = Math.Round(tblInvoiceTO.GrossWeight / 1000, 3);
                    invoiceDT.Rows[0]["tareWeight"] = Math.Round(tblInvoiceTO.TareWeight / 1000, 3);
                    invoiceDT.Rows[0]["netWeight"] = Math.Round(tblInvoiceTO.NetWeight / 1000, 3);

                    invoiceDT.Rows[0]["transporterName"] = tblInvoiceTO.TransporterName;
                    invoiceDT.Rows[0]["Narration"] = tblInvoiceTO.Narration;
                    invoiceDT.Rows[0]["distributorName"] = tblInvoiceTO.DistributorName;
                    invoiceDT.Rows[0]["freightCategory"] = tblInvoiceTO.CommentCategoryName;

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                    }
                    invoiceDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                    invoiceDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;

                    //Aniket [8-02-2019] added
                    //headerDT.Columns.Add("preparationDate");
                    if (tblInvoiceTO.GrossWtTakenDate != DateTime.MinValue)
                        invoiceDT.Rows[0]["grossWtTakenDate"] = tblInvoiceTO.GrossWtDateStr;
                    if (tblInvoiceTO.PreparationDate != DateTime.MinValue)
                    {
                        invoiceDT.Rows[0]["preparationDate"] = tblInvoiceTO.PreparationDateStr;
                        //headerDT.Rows[0]["preparationDate"] = tblInvoiceTO.PreparationDateStr;
                    }
                    //Aniket [8-02-2019] added
                    //headerDT.Columns.Add("paymentTerm");
                    if (!string.IsNullOrEmpty(paymentTermAllCommaSeparated))
                    {
                        invoiceDT.Rows[0]["paymentTerm"] = paymentTermAllCommaSeparated;
                        //headerDT.Rows[0]["paymentTerm"] = paymentTermAllCommaSeparated;
                    }
                    Double taxTotal = 0;
                    if (tblInvoiceTO.CgstAmt > 0 && tblInvoiceTO.SgstAmt > 0)
                    {
                        taxTotal = tblInvoiceTO.CgstAmt + tblInvoiceTO.SgstAmt;
                    }
                    else if (tblInvoiceTO.IgstAmt > 0)
                    {
                        taxTotal = tblInvoiceTO.IgstAmt;
                    }

                    if (isMathRoundoff == 1)
                    {
                        invoiceDT.Rows[0]["taxTotal"] = taxTotal;
                        invoiceDT.Rows[0]["taxTotalStr"] = currencyTowords(taxTotal, tblInvoiceTO.CurrencyId);
                    }
                    else
                    {
                        invoiceDT.Rows[0]["taxTotal"] = Math.Round(taxTotal, 2);
                        invoiceDT.Rows[0]["taxTotalStr"] = currencyTowords(Math.Round(taxTotal, 2), tblInvoiceTO.CurrencyId);
                    }


                    //invoiceItemDT

                    //Int32 finalItemCount = 15;
                    if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                    {
                        tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                        List<TblInvoiceItemDetailsTO> invoiceItemlist = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == 0).ToList();
                        invoiceItemDT.Columns.Add("srNo");
                        invoiceItemDT.Columns.Add("prodItemDesc");
                        invoiceItemDT.Columns.Add("bundles");
                        invoiceItemDT.Columns.Add("invoiceQty", typeof(double));
                        invoiceItemDT.Columns.Add("rate", typeof(double));
                        invoiceItemDT.Columns.Add("basicTotal", typeof(double));
                        //chetan[18-feb-2020] added for display GrandTotal on template
                        invoiceItemDT.Columns.Add("GrandTotal", typeof(double));
                        invoiceItemDT.Columns.Add("RateWithTax", typeof(double));
                        invoiceItemDT.Columns.Add("IGSTAmt", typeof(double));
                        invoiceItemDT.Columns.Add("CGSTAmt", typeof(double));
                        invoiceItemDT.Columns.Add("SGSTAmt", typeof(double));

                        invoiceItemDT.Columns.Add("IGSTPct", typeof(double));
                        invoiceItemDT.Columns.Add("CGSTPct", typeof(double));
                        invoiceItemDT.Columns.Add("SGSTPct", typeof(double));
                        invoiceItemDT.Columns.Add("hsn");

                        //Prajakta[2021-03-31] Added to show GST code upto given digits
                        Int32 gstCodeUptoDigits = 0;
                        TblConfigParamsTO gstCodeUptoDigitTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.SHOW_GST_CODE_UPTO_DIGITS);
                        if (gstCodeUptoDigitTO != null && gstCodeUptoDigitTO.ConfigParamVal != null
                            && gstCodeUptoDigitTO.ConfigParamVal.ToString() != "")
                        {
                            gstCodeUptoDigits = Convert.ToInt32(gstCodeUptoDigitTO.ConfigParamVal);
                        }

                        for (int i = 0; i < invoiceItemlist.Count; i++)
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceItemlist[i];
                            invoiceItemDT.Rows.Add();
                            Int32 invoiceItemDTCount = invoiceItemDT.Rows.Count - 1;
                            invoiceItemDT.Rows[invoiceItemDTCount]["srNo"] = i + 1;
                            invoiceItemDT.Rows[invoiceItemDTCount]["prodItemDesc"] = tblInvoiceItemDetailsTO.ProdItemDesc;
                            invoiceItemDT.Rows[invoiceItemDTCount]["bundles"] = tblInvoiceItemDetailsTO.Bundles;
                            invoiceItemDT.Rows[invoiceItemDTCount]["invoiceQty"] = Math.Round(tblInvoiceItemDetailsTO.InvoiceQty, 3);

                            if (isMathRoundoff == 1)
                            {
                                //invoiceItemDT.Rows[invoiceItemDTCount]["invoiceQty"] = tblInvoiceItemDetailsTO.InvoiceQty;
                                invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = Math.Round(tblInvoiceItemDetailsTO.Rate);
                                invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = Math.Round(tblInvoiceItemDetailsTO.BasicTotal);
                                invoiceItemDT.Rows[invoiceItemDTCount]["GrandTotal"] = Math.Round(tblInvoiceItemDetailsTO.GrandTotal);
                                invoiceItemDT.Rows[invoiceItemDTCount]["RateWithTax"] = Math.Round((tblInvoiceItemDetailsTO.GrandTotal / tblInvoiceItemDetailsTO.InvoiceQty));
                                //  invoiceItemDT.Rows[invoiceItemDTCount]["RateWithTax"] = tblInvoiceItemDetailsTO.GrandTotal/ tblInvoiceItemDetailsTO.InvoiceQty;
                            }
                            else
                            {
                                invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = Math.Round(tblInvoiceItemDetailsTO.Rate, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = Math.Round(tblInvoiceItemDetailsTO.BasicTotal, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["GrandTotal"] = Math.Round(tblInvoiceItemDetailsTO.GrandTotal, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["RateWithTax"] = Math.Round((tblInvoiceItemDetailsTO.GrandTotal / tblInvoiceItemDetailsTO.InvoiceQty), 2);
                            }
                            TblConfigParamsTO tblConfigParamsTOForGajkesari = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("CP_SHOW_GAJKESARI_INVOICE_PRINT_CHANGES");
                            if (tblConfigParamsTOForGajkesari != null)
                            {
                                if (tblConfigParamsTOForGajkesari.ConfigParamVal  == 1.ToString () && tblInvoiceItemDetailsTO.LoadingSlipExtId>0)
                                {
                                    TblLoadingSlipExtTO TblLoadingSlipExtTO = _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExt(tblInvoiceItemDetailsTO.LoadingSlipExtId);
                                    if (tblInvoiceItemDetailsTO != null)
                                    {
                                        string itemDesc = "M.S. TMT BARS " + TblLoadingSlipExtTO.MaterialDesc;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["prodItemDesc"] = itemDesc;
                                        double CdRateAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;
                                        double Cdrate = tblInvoiceItemDetailsTO.CdAmt / tblInvoiceItemDetailsTO.InvoiceQty;
                                        double invoiceRate = Math.Round(CdRateAmt ,2) / Math.Round(tblInvoiceItemDetailsTO.InvoiceQty, 2);
                                        invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = Math.Round( invoiceRate,2);// Math.Round(tblInvoiceItemDetailsTO.TaxableAmt, 2);
                                        invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = Math.Round(tblInvoiceItemDetailsTO.TaxableAmt, 2);
                                    }
                                }
                            }

                            if (gstCodeUptoDigits > 0)
                            {
                                if (!String.IsNullOrEmpty(tblInvoiceItemDetailsTO.GstinCodeNo))
                                {
                                    if (gstCodeUptoDigits > tblInvoiceItemDetailsTO.GstinCodeNo.Length)
                                    {
                                        gstCodeUptoDigits = tblInvoiceItemDetailsTO.GstinCodeNo.Length;
                                    }
                                    tblInvoiceItemDetailsTO.GstinCodeNo = tblInvoiceItemDetailsTO.GstinCodeNo.Substring(0, gstCodeUptoDigits);
                                }
                            }
                            invoiceItemDT.Rows[invoiceItemDTCount]["hsn"] = tblInvoiceItemDetailsTO.GstinCodeNo;


                            if (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList != null && tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 0)
                            {
                                for (int c = 0; c < tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count; c++)
                                {
                                    TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[c];
                                    if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                    {
                                        invoiceItemDT.Rows[invoiceItemDTCount]["IGSTPct"] = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["IGSTAmt"] = tblInvoiceItemTaxDtlsTO.TaxAmt;

                                    }
                                    else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                    {
                                        invoiceItemDT.Rows[invoiceItemDTCount]["CGSTAmt"] = tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["CGSTPct"] = tblInvoiceItemTaxDtlsTO.TaxRatePct;

                                    }
                                    else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                    {
                                        invoiceItemDT.Rows[invoiceItemDTCount]["SGSTAmt"] = tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["SGSTPct"] = tblInvoiceItemTaxDtlsTO.TaxRatePct;

                                    }
                                }
                            }
                        }
                        //if(invoiceItemDT.Rows.Count <finalItemCount)
                        //{
                        //    int ii= invoiceItemDT.Rows.Count  ;
                        //    for (int i=ii; i < finalItemCount;i++)
                        //    {
                        //        invoiceItemDT.Rows.Add();
                        //    }

                        //}
                        var freightResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.FREIGHT).FirstOrDefault();
                        if (freightResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["freightAmt"] = freightResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["freightAmt"] = Math.Round(freightResTO.TaxableAmt, 2);
                            }

                        }
                        //if (Convert.ToDouble(invoiceDT.Rows[0]["freightAmt"]) == 0)
                        else
                        {
                            invoiceDT.Rows[0]["freightAmt"] = tblInvoiceTO.FreightAmt;
                        }
                        var pfResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.PF).FirstOrDefault();
                        if (pfResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["pfAmt"] = pfResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["pfAmt"] = Math.Round(pfResTO.TaxableAmt, 2);
                            }

                        }
                        var cessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.CESS).FirstOrDefault();
                        if (cessResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["cessAmt"] = cessResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["cessAmt"] = Math.Round(cessResTO.TaxableAmt, 2);
                            }

                        }

                        var afterCessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.AFTERCESS).FirstOrDefault();
                        if (afterCessResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["afterCessAmt"] = afterCessResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["afterCessAmt"] = Math.Round(afterCessResTO.TaxableAmt, 2);
                            }

                        }
                        var insuranceTo = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.INSURANCE_ON_SALE).FirstOrDefault();
                        if (insuranceTo != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["insuranceAmt"] = insuranceTo.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["insuranceAmt"] = Math.Round(insuranceTo.TaxableAmt, 2);
                            }

                        }
                        var loadingCharges = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.Loading_Charges).FirstOrDefault();
                        if (loadingCharges != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["loadingCharges"] = loadingCharges.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["loadingCharges"] = Math.Round(loadingCharges.TaxableAmt, 2);
                            }

                        }
                        itemFooterDetailsDT.Rows.Add();
                        itemFooterDetailsDT.Columns.Add("totalQty", typeof(double));
                        itemFooterDetailsDT.Columns.Add("totalBundles");
                        itemFooterDetailsDT.Columns.Add("totalBasicAmt", typeof(double));
                        itemFooterDetailsDT.Columns.Add("EWayBillNo");
                        itemFooterDetailsDT.Rows[0]["EWayBillNo"] = tblInvoiceTO.ElectronicRefNo;
                        var totalQtyResTO = invoiceItemlist.Where(ele => ele.OtherTaxId == 0).ToList();
                        if (totalQtyResTO != null && totalQtyResTO.Count > 0)
                        {
                            Double totalQty = 0;
                            totalQty = totalQtyResTO.Sum(s => s.InvoiceQty);
                            if (isMathRoundoff == 1)
                            {
                                itemFooterDetailsDT.Rows[0]["totalQty"] = totalQty;
                                invoiceDT.Rows[0]["totalQty"] = totalQty;
                            }
                            else
                            {
                                itemFooterDetailsDT.Rows[0]["totalQty"] = Math.Round(totalQty, 3);
                                invoiceDT.Rows[0]["totalQty"] = Math.Round(totalQty, 3);
                            }

                        }
                        bool result;
                        Double sum = 0;
                        // commented by Aniket 
                        //bundles = invoiceItemlist.Sum(s => Convert.ToDouble(s.Bundles));
                        //Aniket [30-8-2019] added if bundles is null or empty string
                        for (int i = 0; i < invoiceItemlist.Count; i++)
                        {
                            Double bundles = 0;
                            result = double.TryParse(invoiceItemlist[i].Bundles, out bundles);
                            if (result)
                            {
                                sum += bundles;
                            }

                        }
                        itemFooterDetailsDT.Rows[0]["totalBundles"] = sum;
                        //Reshma comented below line for showing invoice data as on print.
                        //tblInvoiceTO.BasicAmt = invoiceItemlist.Sum(s => Convert.ToInt32(s.BasicTotal));//added code to sum of items basic total
                        itemFooterDetailsDT.Rows[0]["totalBasicAmt"] = Math.Round(tblInvoiceTO.BasicAmt, 2);
                        invoiceDT.Rows[0]["totalBundles"] = sum;
                        if (isMathRoundoff == 1)
                        {
                            invoiceDT.Rows[0]["totalBasicAmt"] = tblInvoiceTO.BasicAmt;
                        }
                        else
                        {
                            invoiceDT.Rows[0]["totalBasicAmt"] = Math.Round(tblInvoiceTO.BasicAmt, 2);
                        }


                    }

                    string strMobNo = "Mob No:";
                    string strStateCode = String.Empty;
                    string strGstin = String.Empty;
                    strGstin = "GSTIN:";
                    strStateCode = "State & Code:";
                    //if ((firmId == (Int32)Constants.FirmNameE.SRJ) )
                    //{
                    //    strGstin = "GSTIN:";
                    //    strStateCode = "State & Code:";
                    //}
                    //else if(firmId == (Int32)Constants.FirmNameE.Parameshwar)
                    //{
                    //   strGstin = "GSTIN/UIN:";
                    //   strStateCode = "State";
                    //    strCode = ",Code";

                    //}
                    string strPanNo = "PAN No:";


                    addressDT.Columns.Add("lblMobNo");
                    addressDT.Columns.Add("lblStateCode");
                    addressDT.Columns.Add("lblGstin");
                    addressDT.Columns.Add("lblPanNo");


                    addressDT.Columns.Add("strMobNo");
                    addressDT.Columns.Add("billingNm");
                    addressDT.Columns.Add("billingAddr");
                    addressDT.Columns.Add("billingGstNo");
                    addressDT.Columns.Add("billingPanNo");
                    addressDT.Columns.Add("billingState");
                    addressDT.Columns.Add("billingStateCode");
                    addressDT.Columns.Add("billingMobNo");


                    addressDT.Columns.Add("consigneeNm");
                    addressDT.Columns.Add("consigneeAddr");
                    addressDT.Columns.Add("consigneeGstNo");
                    addressDT.Columns.Add("consigneePanNo");
                    addressDT.Columns.Add("consigneeMobNo");
                    addressDT.Columns.Add("consigneeState");
                    addressDT.Columns.Add("consigneeStateCode");


                    addressDT.Columns.Add("lblConMobNo");
                    addressDT.Columns.Add("lblConStateCode");
                    addressDT.Columns.Add("lblConGstin");
                    addressDT.Columns.Add("lblConPanNo");


                    invoiceDT.Columns.Add("shippingNm");
                    invoiceDT.Columns.Add("shippingAddr");
                    invoiceDT.Columns.Add("shippingGstNo");
                    invoiceDT.Columns.Add("shippingPanNo");
                    invoiceDT.Columns.Add("shippingMobNo");
                    invoiceDT.Columns.Add("shippingState");
                    invoiceDT.Columns.Add("shippingStateCode");

                    invoiceDT.Columns.Add("lblShippingMobNo");
                    invoiceDT.Columns.Add("lblShippingStateCode");
                    invoiceDT.Columns.Add("lblShippingGstin");
                    invoiceDT.Columns.Add("lblShippingPanNo");
                    addressDT.Rows.Add();
                    addressDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;
                    headerDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;
                    invoiceDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;

                    headerDT.Rows[0]["BrokerName"] = tblInvoiceTO.BrokerName;
                    invoiceDT.Rows[0]["BrokerName"] = tblInvoiceTO.BrokerName;
                    if (!String.IsNullOrEmpty(tblInvoiceTO.PoDateStr))
                    {
                        DateTime poDate = Convert.ToDateTime(tblInvoiceTO.PoDateStr);
                        addressDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");
                        invoiceDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");
                        headerDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");

                    }
                    addressDT.Rows[0]["electronicRefNo"] = tblInvoiceTO.ElectronicRefNo;
                    string finalAddr = "", addr1 = "";
                    if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                    {
                        TblInvoiceAddressTO tblBillingInvoiceAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                        if (tblBillingInvoiceAddressTO != null)
                        {
                            addressDT.Rows[0]["lblMobNo"] = strMobNo;
                            addressDT.Rows[0]["lblStateCode"] = strStateCode;
                            addressDT.Rows[0]["lblGstin"] = strGstin;
                            addressDT.Rows[0]["lblPanNo"] = strPanNo;
                            addressDT.Rows[0]["billingNm"] = tblBillingInvoiceAddressTO.BillingName;

                            String addressStr = tblBillingInvoiceAddressTO.Address;

                            // if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka)&& !String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                            // {
                            //    if (tblBillingInvoiceAddressTO.Taluka.ToLower()==tblBillingInvoiceAddressTO.District.ToLower())
                            //     {
                            //             addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address + " " + tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;
                            //     }
                            //     else
                            //     {
                            //         addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address + ", " + tblBillingInvoiceAddressTO.Taluka
                            //                                       + tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;
                            //     }
                            // }
                            //else if(!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                            // {
                            //     addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address +" " + tblBillingInvoiceAddressTO.Taluka
                            //                                       + ", " + tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;
                            // }
                            // else if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                            // {
                            //     addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address +" "
                            //                                      +  tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;

                            // }
                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.VillageName))
                            {
                                addressStr += " " + tblBillingInvoiceAddressTO.VillageName;
                            }
                            //Aniket [6-9-2019] added PinCode in address
                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.PinCode) && tblBillingInvoiceAddressTO.PinCode != "0")
                            {
                                addressStr += "-" + tblBillingInvoiceAddressTO.PinCode;
                            }

                            //if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                            //{
                            //    addressStr += ", "+ tblBillingInvoiceAddressTO.Taluka;
                            //}

                            //if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                            //{
                            //    if (String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                            //        tblBillingInvoiceAddressTO.Taluka = String.Empty;

                            //    if (tblBillingInvoiceAddressTO.Taluka.ToLower().Trim() != tblBillingInvoiceAddressTO.District.ToLower().Trim())
                            //        addressStr += ",Dist-" + tblBillingInvoiceAddressTO.District;
                            //}


                            if (String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                                tblBillingInvoiceAddressTO.District = String.Empty;

                            if (String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                                tblBillingInvoiceAddressTO.Taluka = String.Empty;

                            String districtNameWithLabel = String.Empty;
                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                                districtNameWithLabel = ",Dist-" + tblBillingInvoiceAddressTO.District;

                            if (tblBillingInvoiceAddressTO.Taluka.ToLower().Trim() == tblBillingInvoiceAddressTO.District.ToLower().Trim())
                            {

                                addressStr += districtNameWithLabel;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                                {
                                    addressStr += ", " + tblBillingInvoiceAddressTO.Taluka;
                                }

                                addressStr += districtNameWithLabel;
                            }


                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.State))
                            {
                                //addressStr += ", " + tblBillingInvoiceAddressTO.State;
                            }


                            addressDT.Rows[0]["billingAddr"] = addressStr;

                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.GstinNo))
                                addressDT.Rows[0]["billingGstNo"] = tblBillingInvoiceAddressTO.GstinNo.ToUpper();

                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.PanNo))
                                addressDT.Rows[0]["billingPanNo"] = tblBillingInvoiceAddressTO.PanNo.ToUpper();
                            //Aniket [9-9-2-2019]
                            if (tblInvoiceTO.IsConfirmed == 1)
                                addressDT.Rows[0]["billingMobNo"] = tblBillingInvoiceAddressTO.ContactNo;
                            else
                            {
                                addressDT.Rows[0]["billingMobNo"] = String.Empty;
                                addressDT.Rows[0]["lblMobNo"] = String.Empty;
                            }




                            if (stateList != null && stateList.Count > 0)
                            {
                                DropDownTO stateTO = stateList.Where(ele => ele.Value == tblBillingInvoiceAddressTO.StateId).FirstOrDefault();
                                addressDT.Rows[0]["billingState"] = tblBillingInvoiceAddressTO.State;
                                if (stateTO != null)
                                {
                                    //Saket [2019-04-12] Can be manage from template - Change for A1.s
                                    //addressDT.Rows[0]["billingStateCode"] = stateTO.Text + " " + stateTO.Tag;
                                    addressDT.Rows[0]["billingStateCode"] = stateTO.Tag;
                                }
                            }



                        }
                        Boolean IsDisplayConsignee = true;
                        TblInvoiceAddressTO tblConsigneeInvoiceAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS).FirstOrDefault();
                        if (tblConsigneeInvoiceAddressTO != null)
                        {
                            if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.BillingName))
                            {

                                TblConfigParamsTO tempConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName("DISPLAY_CONSIGNEE_ADDRESS_ON_PRINTABLE_INVOICE");
                                if (tempConfigParamsTO != null)
                                {
                                    if (Convert.ToInt32(tempConfigParamsTO.ConfigParamVal) == 1)
                                    {
                                        IsDisplayConsignee = true;
                                    }
                                    else
                                    {
                                        IsDisplayConsignee = false;
                                    }
                                }
                                if (!IsDisplayConsignee)
                                {
                                    if (tblConsigneeInvoiceAddressTO.BillingName.Trim() == tblBillingInvoiceAddressTO.BillingName.Trim())
                                    {
                                        if (tblConsigneeInvoiceAddressTO.Address.Trim() == tblBillingInvoiceAddressTO.Address.Trim())
                                            IsDisplayConsignee = false;
                                        else
                                            IsDisplayConsignee = true;
                                    }
                                    else
                                    {
                                        IsDisplayConsignee = true;
                                    }
                                }
                                //if(tblConsigneeInvoiceAddressTO.BillingName.Trim() != tblBillingInvoiceAddressTO.BillingName.Trim())
                                //{
                                //    IsDisplayConsignee = true;
                                //}

                                if (IsDisplayConsignee)
                                {
                                    addressDT.Rows[0]["lblConMobNo"] = strMobNo;
                                    addressDT.Rows[0]["lblConStateCode"] = strStateCode;
                                    addressDT.Rows[0]["lblConGstin"] = strGstin;
                                    addressDT.Rows[0]["lblConPanNo"] = strPanNo;
                                    addressDT.Rows[0]["consigneeNm"] = tblConsigneeInvoiceAddressTO.BillingName;

                                    String consigneeAddr = tblConsigneeInvoiceAddressTO.Address;
                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka) && !String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                    //{
                                    //    if (tblConsigneeInvoiceAddressTO.Taluka.ToLower() == tblConsigneeInvoiceAddressTO.District.ToLower())
                                    //    {
                                    //        addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + " " + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;
                                    //    }
                                    //    else
                                    //    {
                                    //        addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + ", " + tblConsigneeInvoiceAddressTO.Taluka
                                    //                                      + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;
                                    //    }
                                    //}
                                    //else if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                    //{
                                    //    addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + " " + tblConsigneeInvoiceAddressTO.Taluka
                                    //                                      + ", " + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;
                                    //}
                                    //else if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                    //{
                                    //    addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + " "
                                    //                                     + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;

                                    //}

                                    //addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + "," + tblConsigneeInvoiceAddressTO.Taluka
                                    //                                    + " ," + tblConsigneeInvoiceAddressTO.District + "," + tblConsigneeInvoiceAddressTO.State;
                                    //Aniket [6-9-2019] added PinCode in address
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.VillageName))
                                    {
                                        consigneeAddr += " " + tblConsigneeInvoiceAddressTO.VillageName;
                                    }
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.PinCode) && tblConsigneeInvoiceAddressTO.PinCode != "0")
                                    {
                                        consigneeAddr += "- " + tblConsigneeInvoiceAddressTO.PinCode;
                                    }
                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                    //{
                                    //    consigneeAddr += ", " + tblConsigneeInvoiceAddressTO.Taluka;
                                    //}

                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                    //{
                                    //    if (String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                    //        tblConsigneeInvoiceAddressTO.Taluka = String.Empty;

                                    //    if (tblConsigneeInvoiceAddressTO.Taluka.ToLower() != tblConsigneeInvoiceAddressTO.District.ToLower())
                                    //        consigneeAddr += ",Dist-" + tblConsigneeInvoiceAddressTO.District;
                                    //}


                                    if (String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                        tblConsigneeInvoiceAddressTO.District = String.Empty;

                                    if (String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                        tblConsigneeInvoiceAddressTO.Taluka = String.Empty;

                                    String districtNameWithLabel = String.Empty;
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                        districtNameWithLabel = ",Dist-" + tblConsigneeInvoiceAddressTO.District;

                                    if (tblConsigneeInvoiceAddressTO.Taluka.ToLower().Trim() == tblConsigneeInvoiceAddressTO.District.ToLower().Trim())
                                    {
                                        consigneeAddr += districtNameWithLabel;
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                        {
                                            consigneeAddr += ", " + tblConsigneeInvoiceAddressTO.Taluka;
                                        }

                                        consigneeAddr += districtNameWithLabel;
                                    }




                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.State))
                                    //{
                                    //    consigneeAddr += ", " + tblConsigneeInvoiceAddressTO.State;
                                    //}
                                    //if(!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.PinCode) && tblConsigneeInvoiceAddressTO.PinCode!="0")
                                    //{
                                    //    consigneeAddr += "- " + tblConsigneeInvoiceAddressTO.PinCode;
                                    //}
                                    addressDT.Rows[0]["consigneeAddr"] = consigneeAddr;
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.GstinNo))
                                        addressDT.Rows[0]["consigneeGstNo"] = tblConsigneeInvoiceAddressTO.GstinNo.ToUpper();

                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.PanNo))
                                        addressDT.Rows[0]["consigneePanNo"] = tblConsigneeInvoiceAddressTO.PanNo.ToUpper();
                                    //Aniket [9-9-2-2019]
                                    if (tblInvoiceTO.IsConfirmed == 1)
                                        addressDT.Rows[0]["consigneeMobNo"] = tblConsigneeInvoiceAddressTO.ContactNo;
                                    else
                                    {
                                        addressDT.Rows[0]["consigneeMobNo"] = String.Empty;
                                        addressDT.Rows[0]["lblMobNo"] = String.Empty;
                                    }


                                    if (stateList != null && stateList.Count > 0)
                                    {
                                        DropDownTO stateTO = stateList.Where(ele => ele.Value == tblConsigneeInvoiceAddressTO.StateId).FirstOrDefault();
                                        addressDT.Rows[0]["consigneeState"] = tblConsigneeInvoiceAddressTO.State;
                                        if (stateTO != null)
                                        {
                                            addressDT.Rows[0]["consigneeStateCode"] = stateTO.Tag;
                                        }
                                    }
                                }
                            }



                        }

                        TblInvoiceAddressTO tblShippingAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.SHIPPING_ADDRESS).FirstOrDefault();
                        if (tblShippingAddressTO != null)
                        {
                            if (!String.IsNullOrEmpty(tblShippingAddressTO.Address))
                            {
                                if (tblShippingAddressTO.Address.Trim() != tblBillingInvoiceAddressTO.Address.Trim())
                                {
                                    //headerDT.Rows.Add();
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.ContactNo))
                                    {
                                        invoiceDT.Rows[0]["lblShippingMobNo"] = strMobNo;
                                    }
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.State))
                                    {
                                        invoiceDT.Rows[0]["lblShippingStateCode"] = strStateCode;
                                    }
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.GstinNo))
                                    {
                                        invoiceDT.Rows[0]["lblShippingGstin"] = strGstin;
                                    }
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.PanNo))
                                    {
                                        invoiceDT.Rows[0]["lblShippingPanNo"] = strPanNo;
                                    }



                                    invoiceDT.Rows[0]["shippingNm"] = tblShippingAddressTO.BillingName;
                                    invoiceDT.Rows[0]["shippingAddr"] = tblShippingAddressTO.Address + "," + tblShippingAddressTO.Taluka
                                                                        + " ," + tblShippingAddressTO.District + "," + tblShippingAddressTO.State;
                                    invoiceDT.Rows[0]["shippingGstNo"] = tblShippingAddressTO.GstinNo;
                                    invoiceDT.Rows[0]["shippingPanNo"] = tblShippingAddressTO.PanNo;
                                    //Aniket [9-9-2-2019]
                                    if (tblInvoiceTO.IsConfirmed == 1)
                                        invoiceDT.Rows[0]["shippingMobNo"] = tblShippingAddressTO.ContactNo;
                                    else
                                        invoiceDT.Rows[0]["shippingMobNo"] = String.Empty;


                                }
                            }

                        }

                        //get org bank details
                        List<TblInvoiceBankDetailsTO> tblInvoiceBankDetailsTOList = _iTblInvoiceBankDetailsDAO.SelectInvoiceBankDetails(organizationTO.IdOrganization);
                        if (tblInvoiceBankDetailsTOList != null && tblInvoiceBankDetailsTOList.Count > 0)
                        {
                            TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO = tblInvoiceBankDetailsTOList.Where(c => c.IsPriority == 1).FirstOrDefault();
                            invoiceDT.Rows[0]["bankName"] = tblInvoiceBankDetailsTO.BankName;
                            invoiceDT.Rows[0]["accountNo"] = tblInvoiceBankDetailsTO.AccountNo;
                            invoiceDT.Rows[0]["branchName"] = tblInvoiceBankDetailsTO.Branch;
                            invoiceDT.Rows[0]["ifscCode"] = tblInvoiceBankDetailsTO.IfscCode;
                        }

                        //get org declaration and terms condition details
                        List<TblInvoiceOtherDetailsTO> tblInvoiceOtherDetailsTOList = _iTblInvoiceOtherDetailsDAO.SelectInvoiceOtherDetails(organizationTO.IdOrganization);
                        if (tblInvoiceOtherDetailsTOList != null && tblInvoiceOtherDetailsTOList.Count > 0)
                        {
                            TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO = tblInvoiceOtherDetailsTOList.Where(c => c.DetailTypeId == (int)Constants.invoiceOtherDetailsTypeE.DESCRIPTION).FirstOrDefault();
                            invoiceDT.Rows[0]["declaration"] = tblInvoiceOtherDetailsTO.Description;
                        }

                    }

                }

                //Saket [2019-08-11] To keep the both DT columns same
                //If two same columns with different values then 2 rows will be created while merging DT.
                //invoiceDT.Merge(headerDT);

                headerDT = invoiceDT.Copy();
                headerDT.TableName = "headerDT";

                printDataSet.Tables.Add(headerDT);
                printDataSet.Tables.Add(qrCodeDT);
                printDataSet.Tables.Add(invoiceDT);
                printDataSet.Tables.Add(invoiceItemDT);
                printDataSet.Tables.Add(addressDT);
                printDataSet.Tables.Add(itemFooterDetailsDT);
                printDataSet.Tables.Add(commercialDT);
                printDataSet.Tables.Add(hsnItemTaxDT);
                printDataSet.Tables.Add(multipleInvoiceCopyDT);
                // printDataSet.Tables.Add(shippingAddressDT);
                //creating template'''''''''''''''''

                int isMultipleTemplateByCorNc = 0;

                string templateName = "";
                if (isPrinted)
                {
                    int val = 0;
                    //int isMultipleTemplateByCorNc = 0;
                    TblConfigParamsTO configParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE);
                    TblConfigParamsTO configParamsTOTempC_NC = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE_BY_CONFIRM);

                    if (configParamsTOTemp != null)
                    {
                        val = Convert.ToInt16(configParamsTOTemp.ConfigParamVal);
                    }
                    if (configParamsTOTempC_NC != null)
                    {
                        isMultipleTemplateByCorNc = Convert.ToInt16(configParamsTOTempC_NC.ConfigParamVal);
                    }
                    if (val == 0)
                    {
                        templateName = "InvoiceVoucherPrinted";
                    }
                    else
                    {
                        templateName = "InvoiceVoucherPrePrinted_" + tblInvoiceTO.InvFromOrgId;
                    }
                    if (isMultipleTemplateByCorNc == 1)
                    {
                        string IsConfirmedC_NC = null;
                        if (tblInvoiceTO.IsConfirmed == 1)
                        {
                            IsConfirmedC_NC = "_C";
                        }
                        else
                        {
                            IsConfirmedC_NC = "_NC";
                        }
                        templateName += IsConfirmedC_NC;
                    }
                }
                else
                {
                    int val = 0;
                    TblConfigParamsTO configParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE);
                    TblConfigParamsTO configParamsTOTempC_NC = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE_BY_CONFIRM);

                    if (configParamsTOTemp != null)
                    {
                        val = Convert.ToInt16(configParamsTOTemp.ConfigParamVal);
                    }
                    if (configParamsTOTempC_NC != null)
                    {
                        isMultipleTemplateByCorNc = Convert.ToInt16(configParamsTOTempC_NC.ConfigParamVal);
                    }

                    if (val == 0)
                    {
                        templateName = "InvoiceVoucherPlain";
                    }
                    else
                    {
                        templateName = "InvoiceVoucherPlain_" + tblInvoiceTO.InvFromOrgId;
                    }
                    if (isMultipleTemplateByCorNc == 1)
                    {
                        string IsConfirmedC_NC = null;
                        if (tblInvoiceTO.IsConfirmed == 1)
                        {
                            IsConfirmedC_NC = "_C";
                        }
                        // tblInvoiceTO.IsConfirmedC_NC = "C";
                        else
                        {
                            IsConfirmedC_NC = "_NC";
                        }
                        templateName += IsConfirmedC_NC;
                    }
                }
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                // templateFilePath = @"C:\Deliver Templates\SER INVOICE Template.xls";
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
                    //String driveName = BL.RunReport.GetDriveNameNotOSDrive();
                    //String objFilePath = string.Empty;
                    //TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO("TEMP_REPORT_PATH");
                    //if (tblConfigParamsTO != null)
                    //{
                    //    objFilePath = tblConfigParamsTO.ConfigParamVal;
                    //}
                    //String path = objFilePath.ToString();
                    //if (!Directory.Exists(path))
                    //{
                    //    Directory.CreateDirectory(path);
                    //}

                    String filePath = String.Empty;

                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {
                        filePath = resultMessage.Tag.ToString();
                    }
                    if (isFileDelete)
                    {
                        //driveName + path;
                        Byte[] bytes = DeleteFile(saveLocation, filePath);

                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = Convert.ToBase64String(bytes);
                        }
                    }
                    else
                        resultMessage.Tag = filePath;
                    //List<IFormFile> files = new List<IFormFile>();
                    //TblDocumentDetailsTO tblDocumentDetailsTO = new TblDocumentDetailsTO();
                    //tblDocumentDetailsTO.IsActive = 1;
                    //tblDocumentDetailsTO.ModuleId = 1;
                    //tblDocumentDetailsTO.DocumentDesc = "Inv" + tblInvoiceTO.IdInvoice;
                    //tblDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                    //tblDocumentDetailsTO.CreatedBy = 1;
                    //tblDocumentDetailsTO.FileTypeId = 2;
                    //tblDocumentDetailsTO.FileData = bytes;
                    //tblDocumentDetailsTO.Extension = "pdf";
                    //List<TblDocumentDetailsTO> tblDocumentDetailsTOList = new List<TblDocumentDetailsTO>();
                    //tblDocumentDetailsTOList.Add(tblDocumentDetailsTO);
                    //resultMessage = BL._iTblDocumentDetailsBL.UploadDocument(tblDocumentDetailsTOList);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {

                        //    string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        //    string directoryName;



                        //    directoryName = Path.GetDirectoryName(saveLocation);
                        //    string[] fileEntries = Directory.GetFiles(directoryName, "*Bill*");
                        //    string[] filesList = Directory.GetFiles(directoryName, "*Bill*");

                        //    foreach (string file in filesList)
                        //    {
                        //        //if (file.ToUpper().Contains(resFname.ToUpper()))
                        //        {
                        //            File.Delete(file);
                        //        }
                        //    }

                        //    string fl1 = Path.GetFileName(tblDocumentDetailsTO.Path);

                        //    String fNm = tblDocumentDetailsTO.DocumentDesc + "." + tblDocumentDetailsTO.Extension;
                        //    //BL._iTblDocumentDetailsBL.DeleteFromBlob(fl1);
                        //    //if ((System.IO.File.Exists(saveLocation)))
                        //    //{
                        //    //    System.IO.File.Delete(saveLocation);
                        //    //}

                        resultMessage.DefaultSuccessBehaviour();
                    }

                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
                //if (isFileDelete)
                //{
                //    //Reshma Added FOr WhatsApp integration.
                //      resultMessage  = SendFileOnWhatsAppAfterEwayBillGeneration(tblInvoiceTO.IdInvoice);
                //}
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        //Reshma Added FOr test Certificate Print
        public ResultMessage PrintTestCertificateInvoiceDetails(Int32 invoiceId)
        {
            ResultMessage resultMessage = new ResultMessage();
            String response = String.Empty;
            String signedQRCode = String.Empty;
            Int32 apiId = (int)EInvoiceAPIE.GENERATE_EINVOICE;
            byte[] PhotoCodeInBytes = null;
            try
            {
                TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTOWithDetails(invoiceId);


                DataSet printDataSet = new DataSet();
                DataTable headerDT = new DataTable();
                DataTable invoiceDT = new DataTable();
                DataTable invoiceItemDT = new DataTable();
                DataTable addressDT = new DataTable();

                addressDT.TableName = "addressDT";
                //headerDT.TableName = "headerDT";
                invoiceDT.TableName = "headerDT";
                invoiceItemDT.TableName = "invoiceItemDT";
                int defaultCompOrgId = 0;

                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (configParamsTO != null)
                    {
                        defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
                    }
                }
                else
                {
                    defaultCompOrgId = tblInvoiceTO.InvFromOrgId;
                }
                TblOrganizationTO organizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(defaultCompOrgId);

                if (tblInvoiceTO != null)
                {

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        tblInvoiceTO.VehicleNo = tblInvoiceTO.VehicleNo.ToUpper();
                    }
                    invoiceDT.Columns.Add("invoiceNo");
                    invoiceDT.Columns.Add("invoiceDateStr");
                    invoiceDT.Columns.Add("vehicleNo");
                    invoiceDT.Columns.Add("poNo");
                    invoiceDT.Columns.Add("poDateStr");

                    invoiceDT.Rows.Add();
                    invoiceDT.Rows[0]["invoiceNo"] = tblInvoiceTO.InvoiceNo;
                    invoiceDT.Rows[0]["invoiceDateStr"] = tblInvoiceTO.InvoiceDateStr;

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                    }
                    addressDT.Columns.Add("billingNm");
                    addressDT.Columns.Add("consigneeNm");
                    if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                    {
                        addressDT.Rows.Add();
                        TblInvoiceAddressTO tblBillingInvoiceAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                        if (tblBillingInvoiceAddressTO != null)
                        {
                            addressDT.Rows[0]["billingNm"] = tblBillingInvoiceAddressTO.BillingName;
                        }
                        TblInvoiceAddressTO tblBillingInvoiceAddressTOV2 = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS).FirstOrDefault();
                        if (tblBillingInvoiceAddressTOV2 != null)
                        {
                            addressDT.Rows[0]["consigneeNm"] = tblBillingInvoiceAddressTOV2.BillingName;
                        }
                    }
                    //Int32 finalItemCount = 15;
                    if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                    {
                        tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                        List<TblInvoiceItemDetailsTO> invoiceItemlist = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == 0).ToList();
                        if (invoiceItemlist != null && invoiceItemlist.Count > 0)
                        {
                            invoiceItemDT.Columns.Add("srNo");
                            invoiceItemDT.Columns.Add("prodItemDesc");
                            invoiceItemDT.Columns.Add("bundles");
                            invoiceItemDT.Columns.Add("invoiceQty", typeof(double));
                            invoiceItemDT.Columns.Add("TestingDate");
                            invoiceItemDT.Columns.Add("ChemC", typeof(double));
                            invoiceItemDT.Columns.Add("ChemS", typeof(double));
                            invoiceItemDT.Columns.Add("ChemP", typeof(double));
                            invoiceItemDT.Columns.Add("MechProof", typeof(double));
                            invoiceItemDT.Columns.Add("MechTen", typeof(double));
                            invoiceItemDT.Columns.Add("MechElon", typeof(double));
                            invoiceItemDT.Columns.Add("MechTEle", typeof(double));
                            invoiceItemDT.Columns.Add("ChemCE", typeof(double));
                            invoiceItemDT.Columns.Add("ChemT", typeof(double));
                            invoiceItemDT.Columns.Add("RatioTS", typeof(double));

                            invoiceItemDT.Columns.Add("CastNo");
                            invoiceItemDT.Columns.Add("Grade");
                            invoiceItemDT.Columns.Add("BendTest");
                            invoiceItemDT.Columns.Add("RebandTest");
                            invoiceItemDT.Columns.Add("GradeOfSteel");
                            invoiceItemDT.Columns.Add("Remark");

                            int bookingId = 0;
                            for (int x = 0; x < invoiceItemlist.Count; x++)
                            {
                                invoiceItemDT.Rows.Add();
                                int count = invoiceItemDT.Rows.Count - 1;
                                TblInvoiceItemDetailsTO TblInvoiceItemDetailsTO = invoiceItemlist[x];
                                if (TblInvoiceItemDetailsTO.LoadingSlipExtId > 0)
                                {
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExt(TblInvoiceItemDetailsTO.LoadingSlipExtId);
                                    SizeTestingDtlTO sizeTestingDtlTO = _iTblParitySummaryDAO.SelectTestCertificateDdtl(TblInvoiceItemDetailsTO.SizeTestingDtlId);
                                    if (tblLoadingSlipExtTO != null)
                                    {
                                        if (tblLoadingSlipExtTO.BookingId >0 && bookingId==0)
                                            bookingId = tblLoadingSlipExtTO.BookingId;
                                        invoiceItemDT.Rows[count]["srNo"] = x + 1;
                                        invoiceItemDT.Rows[count]["prodItemDesc"] = tblLoadingSlipExtTO.MaterialDesc;
                                        invoiceItemDT.Rows[count]["invoiceQty"] = TblInvoiceItemDetailsTO.InvoiceQty;
                                        if (sizeTestingDtlTO != null)
                                        {
                                            invoiceItemDT.Rows[count]["TestingDate"] = sizeTestingDtlTO.TestingDate.ToString("dd-MM-yyyy");
                                            invoiceItemDT.Rows[count]["ChemC"] = sizeTestingDtlTO.ChemC;
                                            invoiceItemDT.Rows[count]["ChemS"] = sizeTestingDtlTO.ChemS;
                                            invoiceItemDT.Rows[count]["ChemP"] = sizeTestingDtlTO.ChemP;
                                            invoiceItemDT.Rows[count]["MechProof"] = sizeTestingDtlTO.MechProof;
                                            invoiceItemDT.Rows[count]["MechTen"] = sizeTestingDtlTO.MechTen;
                                            invoiceItemDT.Rows[count]["MechElon"] = sizeTestingDtlTO.MechElon;
                                            invoiceItemDT.Rows[count]["MechTEle"] = sizeTestingDtlTO.MechTEle;

                                            invoiceItemDT.Rows[count]["ChemCE"] = sizeTestingDtlTO.ChemCE;
                                            invoiceItemDT.Rows[count]["ChemT"] = sizeTestingDtlTO.ChemT;
                                            invoiceItemDT.Rows[count]["RatioTS"] =Math.Round ( sizeTestingDtlTO.MechTen/ sizeTestingDtlTO.MechProof,2);

                                            invoiceItemDT.Rows[count]["CastNo"] = sizeTestingDtlTO.CastNo;
                                            invoiceItemDT.Rows[count]["Grade"] = sizeTestingDtlTO.Grade;
                                            invoiceItemDT.Rows[count]["BendTest"] = "OK";
                                            invoiceItemDT.Rows[count]["RebandTest"] = "OK";
                                            invoiceItemDT.Rows[count]["GradeOfSteel"] = "OK";
                                            invoiceItemDT.Rows[count]["Remark"] = "OK";
                                        }

                                    }
                                }
                            }

                            if (bookingId > 0)
                            {
                                TblBookingsTO tblBookingsTO = _iTblBookingsBL.SelectTblBookingsTO(bookingId);
                                if (tblBookingsTO != null)
                                {
                                    invoiceDT.Rows[0]["poNo"] = tblBookingsTO.PoNo;
                                    invoiceDT.Rows[0]["poDateStr"] = tblBookingsTO.PoDateStr;
                                }
                            }
                        }

                    }
                    //headerDT = invoiceDT.Clone();
                    printDataSet.Tables.Add(addressDT);
                    printDataSet.Tables.Add(invoiceDT);
                    printDataSet.Tables.Add(invoiceItemDT);
                    //printDataSet.Tables.Add(headerDT);

                    String templateFilePath = _iDimReportTemplateBL.SelectReportFullName("InvoiceTestCertificate");
                    // templateFilePath = @"C:\Deliver Templates\SER INVOICE Template.xls";
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
                        //driveName + path;
                        Byte[] bytes = DeleteFile(saveLocation, filePath);

                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = Convert.ToBase64String(bytes);
                        }

                        else
                            resultMessage.Tag = filePath;

                        resultMessage.DefaultSuccessBehaviour();
                    }
                    else
                    {
                        resultMessage.Text = "Something wents wrong please try again";
                        resultMessage.DisplayMessage = "Something wents wrong please try again";
                        resultMessage.Result = 0;
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
        public ResultMessage PostUpdateInvoiceStatus(TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                tblInvoiceTO.UpdatedOn = _iCommon.ServerDateTime;
                result = _iTblInvoiceDAO.PostUpdateInvoiceStatus(tblInvoiceTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While Update tempInvoice");
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateInvoiceStatus");
                return resultMessage;
            }
            finally
            {

            }
        }
        public Byte[] DeleteFile(string saveLocation, string filePath)
        {
            String fileName1 = Path.GetFileName(saveLocation);
            Byte[] bytes = File.ReadAllBytes(filePath);
            if (bytes != null && bytes.Length > 0)
            {

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
            return bytes;
        }

        int  MargeMultiplePDF(string[] PDFfileNames, string OutputFile)
        {
            // Create document object  
            int result = 0;
            iTextSharp.text.Document PDFdoc = new iTextSharp.text.Document();
            try
            {
                // Create a object of FileStream which will be disposed at the end  
                using (System.IO.FileStream MyFileStream = new System.IO.FileStream(OutputFile, System.IO.FileMode.Create))
                {
                    // Create a PDFwriter that is listens to the Pdf document  
                    iTextSharp.text.pdf.PdfCopy PDFwriter = new iTextSharp.text.pdf.PdfCopy(PDFdoc, MyFileStream);
                    if (PDFwriter == null)
                    {
                        return 0;
                    }
                    // Open the PDFdocument  
                    PDFdoc.Open();
                    foreach (string fileName in PDFfileNames)
                    {
                        // Create a PDFreader for a certain PDFdocument  
                        iTextSharp.text.pdf.PdfReader PDFreader = new iTextSharp.text.pdf.PdfReader(fileName);
                        PDFreader.ConsolidateNamedDestinations();
                        // Add content  
                        for (int i = 1; i <= PDFreader.NumberOfPages; i++)
                        {
                            iTextSharp.text.pdf.PdfImportedPage page = PDFwriter.GetImportedPage(PDFreader, i);
                            PDFwriter.AddPage(page);
                        }
                        iTextSharp.text.pdf.PRAcroForm form = PDFreader.AcroForm;
                        if (form != null)
                        {
                            PDFwriter.CopyAcroForm(PDFreader);
                        }
                        // Close PDFreader  
                        PDFreader.Close();
                    }
                    // Close the PDFdocument and PDFwriter  
                    PDFwriter.Close();
                    PDFdoc.Close();
                    return 1;
                }// Disposes the Object of FileStream  
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }

        public ResultMessage SendInvoiceEmail(SendMail mailInformationTo)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (mailInformationTo.IsSendEmailForInvoice)
                {
                    resultMessage = PrintReport(mailInformationTo.InvoiceId, false, true);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(string))
                        {
                            mailInformationTo.IsInvoiceAttach = true;
                            mailInformationTo.Message = resultMessage.Tag.ToString();
                        }
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }

                if (mailInformationTo.IsSendEmailForWeighment)
                {
                    resultMessage = PrintWeighingReport(mailInformationTo.InvoiceId, true, Constants.WeighmentSlip);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(string))
                        {
                            mailInformationTo.IsWeighmentSlipAttach = true;
                            mailInformationTo.WeighmentSlip = resultMessage.Tag.ToString();
                        }
                    }
                    else
                    {
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }

                Int32 result = sendInvoiceFromMail(mailInformationTo);
                if (result <= 0)
                {
                    resultMessage.DefaultBehaviour();
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in  SendInvoiceEmail(SendMail mailInformationTo)");
                return resultMessage;


            }
        }

       
        public ResultMessage PrintWeighingReport(Int32 invoiceId, Boolean isSendEmailForWeighment = false, String reportType = null, Boolean isFileDelete = true)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (invoiceId != 0)
                {
                    TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTOWithDetails(invoiceId);
                    TblLoadingSlipTO TblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetailsByInvoice(invoiceId);
                    List<TblInvoiceAddressTO> invoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);

                    if (TblLoadingSlipTO != null)
                    {
                        List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOList = _iTblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory(TblLoadingSlipTO.LoadingId);
                        DataSet printDataSet = new DataSet();

                        //headerDT
                        DataTable headerDT = new DataTable();
                        DataTable loadingItemDT = new DataTable();
                        DataTable loadingItemDTForGatePass = new DataTable();


                        DataTable multipleInvoiceCopyDT = new DataTable();
                        headerDT.TableName = "headerDT";
                        loadingItemDT.TableName = "loadingItemDT";
                        loadingItemDTForGatePass.TableName = "loadingItemDTForGatePass";

                        #region Add Columns
                        headerDT.Columns.Add("InvoiceId");
                        headerDT.Columns.Add("FirmName");
                        headerDT.Columns.Add("dealername");
                        headerDT.Columns.Add("ContactNo"); //[2021-10-13] Dhananjay Added
                        headerDT.Columns.Add("VillageName"); //[2021-10-20] Dhananjay Added
                        headerDT.Columns.Add("VehicleNo");
                        headerDT.Columns.Add("DriverContactNo"); //[2021-10-29] Dhananjay Added
                        headerDT.Columns.Add("LoadingSlipId");
                        headerDT.Columns.Add("loadingLayerDesc");
                        headerDT.Columns.Add("DateTime");
                        headerDT.Columns.Add("Date");
                        headerDT.Columns.Add("TotalBundles");
                        headerDT.Columns.Add("TotalNetWt", typeof(double));
                        headerDT.Columns.Add("TotalTareWt", typeof(double));
                        headerDT.Columns.Add("TotalGrossWt", typeof(double));
                        headerDT.Columns.Add("invoiceNo");
                        //Prajakta[2020-07-14] Added
                        headerDT.Columns.Add("orgFirmName");
                        headerDT.Columns.Add("orgPhoneNo");
                        headerDT.Columns.Add("orgFaxNo");
                        headerDT.Columns.Add("orgWebsite");
                        headerDT.Columns.Add("orgEmailAddr");
                        headerDT.Columns.Add("plotNo");
                        headerDT.Columns.Add("areaName");
                        headerDT.Columns.Add("district");
                        headerDT.Columns.Add("pinCode");
                        headerDT.Columns.Add("orgVillageNm");
                        headerDT.Columns.Add("orgAddr");
                        headerDT.Columns.Add("orgState");
                        headerDT.Columns.Add("orgStateCode");
                        headerDT.Columns.Add("VehicleInTime");
                        headerDT.Columns.Add("VehicleOutTime");
                        headerDT.Columns.Add("FinalWeight");

                        loadingItemDTForGatePass.Columns.Add("SrNo");
                        loadingItemDTForGatePass.Columns.Add("DisplayName");
                        loadingItemDTForGatePass.Columns.Add("MaterialDesc");
                        loadingItemDTForGatePass.Columns.Add("ProdItemDesc");
                        loadingItemDTForGatePass.Columns.Add("LoadingQty", typeof(double));
                        loadingItemDTForGatePass.Columns.Add("Bundles");
                        loadingItemDTForGatePass.Columns.Add("LoadedWeight", typeof(double));
                        loadingItemDTForGatePass.Columns.Add("MstLoadedBundles");
                        loadingItemDTForGatePass.Columns.Add("LoadedBundles");
                        loadingItemDTForGatePass.Columns.Add("GrossWt", typeof(double));
                        loadingItemDTForGatePass.Columns.Add("TareWt", typeof(double));
                        loadingItemDTForGatePass.Columns.Add("NetWt", typeof(double));
                        loadingItemDTForGatePass.Columns.Add("BrandDesc");
                        loadingItemDTForGatePass.Columns.Add("ProdSpecDesc");
                        loadingItemDTForGatePass.Columns.Add("ProdcatDesc");
                        loadingItemDTForGatePass.Columns.Add("ItemName");
                        loadingItemDTForGatePass.Columns.Add("UpdatedOn");
                        loadingItemDTForGatePass.Columns.Add("DisplayField");
                        loadingItemDTForGatePass.Columns.Add("LoadingSlipId");


                        loadingItemDT.Columns.Add("SrNo");
                        loadingItemDT.Columns.Add("DisplayName");
                        loadingItemDT.Columns.Add("MaterialDesc");
                        loadingItemDT.Columns.Add("ProdItemDesc");
                        loadingItemDT.Columns.Add("LoadingQty", typeof(double));
                        loadingItemDT.Columns.Add("Bundles");
                        loadingItemDT.Columns.Add("LoadedWeight", typeof(double));
                        loadingItemDT.Columns.Add("MstLoadedBundles");
                        loadingItemDT.Columns.Add("LoadedBundles");
                        loadingItemDT.Columns.Add("GrossWt", typeof(double));
                        loadingItemDT.Columns.Add("TareWt", typeof(double));
                        loadingItemDT.Columns.Add("NetWt", typeof(double));
                        loadingItemDT.Columns.Add("BrandDesc");
                        loadingItemDT.Columns.Add("ProdSpecDesc");
                        loadingItemDT.Columns.Add("ProdcatDesc");
                        loadingItemDT.Columns.Add("ItemName");
                        loadingItemDT.Columns.Add("UpdatedOn");
                        loadingItemDT.Columns.Add("DisplayField");
                        loadingItemDT.Columns.Add("LoadingSlipId");




                        #endregion
                        if (TblLoadingSlipTO != null)
                        {
                            headerDT.Rows.Add();
                            double totalBundle = 0;
                            double totalNetWt = 0;
                            //double 
                            headerDT.Rows[0]["InvoiceId"] = invoiceId;
                            //prajkta[25-06-2020]added
                            headerDT.Rows[0]["invoiceNo"] = tblInvoiceTO.InvoiceNo;
                            headerDT.Rows[0]["FinalWeight"] = tblInvoiceTO.GrossWeight;
                            if (invoiceAddressTOList != null && invoiceAddressTOList.Count > 0)
                            {
                                TblInvoiceAddressTO tblInvoiceAddressTO = invoiceAddressTOList.Where(w => w.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                                headerDT.Rows[0]["FirmName"] = tblInvoiceAddressTO.BillingName;
                                headerDT.Rows[0]["dealername"] = tblInvoiceAddressTO.BillingName;
                                headerDT.Rows[0]["ContactNo"] = tblInvoiceAddressTO.ContactNo; //[2021-10-13] Dhananjay Added
                                headerDT.Rows[0]["VillageName"] = tblInvoiceAddressTO.VillageName; //[2021-10-20] Dhananjay Added
                            }
                            headerDT.Rows[0]["VehicleNo"] = TblLoadingSlipTO.VehicleNo;
                            headerDT.Rows[0]["DriverContactNo"] = TblLoadingSlipTO.ContactNo; //[2021-10-29] Dhananjay Added
                            headerDT.Rows[0]["loadingLayerDesc"] = TblLoadingSlipTO.LoadingSlipExtTOList[0].LoadingLayerDesc;
                            headerDT.Rows[0]["LoadingSlipId"] = TblLoadingSlipTO.IdLoadingSlip;
                            //headerDT.Rows[0]["Date"] = TblLoadingSlipTO.CreatedOnStr;
                            //headerDT.Rows[0]["Date"] = TblLoadingSlipTO.CreatedOnStr;
                            //headerDT.Rows[0]["Date"] = tblInvoiceTO.CreatedOnStr;
                            if (tblInvoiceTO != null && tblInvoiceTO.CreatedOn != new DateTime())
                            {
                                //string dtStr = tblInvoiceTO.CreatedOn.ToShortDateString();
                                string dtStr = tblInvoiceTO.InvoiceDateStr ;
                                headerDT.Rows[0]["DateTime"] = tblInvoiceTO.CreatedOnStr;
                                headerDT.Rows[0]["Date"] = dtStr;
                            }
                            else
                            {
                                string dtStr = TblLoadingSlipTO.CreatedOn.ToString("dd-MM-yyyy"); ;
                                headerDT.Rows[0]["DateTime"] = TblLoadingSlipTO.CreatedOnStr;
                                headerDT.Rows[0]["Date"] = dtStr;
                            }

                            if (TblLoadingSlipTO.LoadingSlipExtTOList != null && TblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                            {
                                TblLoadingSlipTO.LoadingSlipExtTOList = TblLoadingSlipTO.LoadingSlipExtTOList.OrderBy(o => o.CalcTareWeight).ToList();

                                for (int j = 0; j < TblLoadingSlipTO.LoadingSlipExtTOList.Count; j++)
                                {
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = TblLoadingSlipTO.LoadingSlipExtTOList[j];
                                    loadingItemDT.Rows.Add();
                                    Int32 loadItemDTCount = loadingItemDT.Rows.Count - 1;

                                    loadingItemDT.Rows[loadItemDTCount]["SrNo"] = loadItemDTCount + 1;
                                    string displayName = tblLoadingSlipExtTO.ProdCatDesc + " " + tblLoadingSlipExtTO.ProdSpecDesc + " " + tblLoadingSlipExtTO.MaterialDesc;
                                    if (displayName == "  ")
                                        displayName = tblLoadingSlipExtTO.ItemName;
                                    loadingItemDT.Rows[loadItemDTCount]["DisplayName"] = displayName;// tblLoadingSlipExtTO.DisplayName;

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
                                    totalBundle += tblLoadingSlipExtTO.LoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["TareWt"] = (tblLoadingSlipExtTO.CalcTareWeight / 1000);
                                    loadingItemDT.Rows[loadItemDTCount]["GrossWt"] = (tblLoadingSlipExtTO.CalcTareWeight + tblLoadingSlipExtTO.LoadedWeight) / 1000;
                                    loadingItemDT.Rows[loadItemDTCount]["NetWt"] = tblLoadingSlipExtTO.LoadedWeight / 1000;
                                    totalNetWt += (tblLoadingSlipExtTO.LoadedWeight / 1000);
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedWeight"] = tblLoadingSlipExtTO.LoadedWeight;
                                    loadingItemDT.Rows[loadItemDTCount]["MstLoadedBundles"] = tblLoadingSlipExtTO.MstLoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadingSlipId"] = tblLoadingSlipExtTO.LoadingSlipId;

                                }
                            }
                            headerDT.Rows[0]["TotalBundles"] = totalBundle;
                            headerDT.Rows[0]["TotalNetWt"] = totalNetWt;
                            headerDT.Rows[0]["TotalTareWt"] = (tblInvoiceTO.TareWeight / 1000);
                            headerDT.Rows[0]["TotalGrossWt"] = (tblInvoiceTO.GrossWeight / 1000);
                            headerDT.Rows[0]["TotalNetWt"] = (tblInvoiceTO.NetWeight / 1000);

                            if (tblLoadingStatusHistoryTOList != null && tblLoadingStatusHistoryTOList.Count > 0)
                            {
                                List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryTOTempInList = tblLoadingStatusHistoryTOList.Where(w => w.StatusId == 20).ToList();
                                if (tblLoadingStatusHistoryTOTempInList != null && tblLoadingStatusHistoryTOTempInList.Count > 0)
                                {
                                    headerDT.Rows[0]["VehicleInTime"] = tblLoadingStatusHistoryTOTempInList[0].StatusDate;
                                }
                                //List<TblLoadingStatusHistoryTO> tblLoadingStatusHistoryOutTempInList = tblLoadingStatusHistoryTOList.Where(w => w.StatusId == 20).ToList();
                                if (TblLoadingSlipTO.StatusId == 17)
                                {
                                    headerDT.Rows[0]["VehicleOutTime"] = TblLoadingSlipTO.StatusDate;
                                }
                            }
                        }


                        //Prajakta[2020-07-14] Added to show orgFirmName and address details 
                        int defaultCompOrgId = 0;

                        if (tblInvoiceTO.InvFromOrgId == 0)
                        {
                            TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                            if (configParamsTO != null)
                            {
                                defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
                            }
                        }
                        else
                        {
                            defaultCompOrgId = tblInvoiceTO.InvFromOrgId;
                        }
                        TblOrganizationTO organizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(defaultCompOrgId);
                        TblAddressTO tblAddressTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(organizationTO.IdOrganization, Constants.AddressTypeE.OFFICE_ADDRESS);
                        List<DropDownTO> stateList = _iDimensionBL.SelectStatesForDropDown(0);
                        if (organizationTO != null)
                        {
                            //headerDT.Rows.Add();
                            //headerDT.Rows.Add();
                            headerDT.Rows[0]["orgFirmName"] = organizationTO.FirmName;

                            headerDT.Rows[0]["orgPhoneNo"] = organizationTO.PhoneNo;
                            headerDT.Rows[0]["orgFaxNo"] = organizationTO.FaxNo;
                            headerDT.Rows[0]["orgWebsite"] = organizationTO.Website;
                            headerDT.Rows[0]["orgEmailAddr"] = organizationTO.EmailAddr;
                        }


                        if (tblAddressTO != null)
                        {
                            String orgAddrStr = String.Empty;
                            if (!String.IsNullOrEmpty(tblAddressTO.PlotNo))
                            {
                                orgAddrStr += tblAddressTO.PlotNo;
                                headerDT.Rows[0]["plotNo"] = tblAddressTO.PlotNo;
                            }
                            if (!String.IsNullOrEmpty(tblAddressTO.AreaName))
                            {
                                orgAddrStr += " " + tblAddressTO.AreaName;
                                headerDT.Rows[0]["areaName"] = tblAddressTO.AreaName;
                            }
                            if (!String.IsNullOrEmpty(tblAddressTO.DistrictName))
                            {
                                orgAddrStr += " " + tblAddressTO.DistrictName;
                                headerDT.Rows[0]["district"] = tblAddressTO.DistrictName;

                            }
                            if (tblAddressTO.Pincode > 0)
                            {
                                orgAddrStr += "-" + tblAddressTO.Pincode;
                                headerDT.Rows[0]["pinCode"] = tblAddressTO.Pincode;

                            }
                            headerDT.Rows[0]["orgVillageNm"] = tblAddressTO.VillageName + "-" + tblAddressTO.Pincode;
                            headerDT.Rows[0]["orgAddr"] = orgAddrStr;
                            headerDT.Rows[0]["orgState"] = tblAddressTO.StateName;

                            if (stateList != null && stateList.Count > 0)
                            {
                                DropDownTO stateTO = stateList.Where(ele => ele.Value == tblAddressTO.StateId).FirstOrDefault();
                                if (stateTO != null)
                                {

                                    headerDT.Rows[0]["orgStateCode"] = stateTO.Tag;
                                }
                            }
                        }


                        //headerDT = loadingDT.Copy();
                        headerDT.TableName = "headerDT";
                        printDataSet.Tables.Add(headerDT);

                        loadingItemDT.TableName = "loadingItemDT";
                        printDataSet.Tables.Add(loadingItemDT);

                        loadingItemDTForGatePass = loadingItemDT.Copy();

                        loadingItemDT.TableName = "loadingItemDTForGatePass";
                        printDataSet.Tables.Add(loadingItemDTForGatePass);


                        string templateName = "";

                        if (reportType == Constants.WeighmentSlip)
                        {
                            //templateName = "WeighingSlip";
                            //[2021-10-20] Dhananjay Commented templateName = "WeighmentSlip";
                            templateName = "WeighingSlipNonConfirm"; //[2021-10-20] Dhananjay Added
                        }
                        else if (reportType == Constants.GatePassSlip)
                        {
                            templateName = "GatePassSlip";
                            //[2021-10-20] Dhananjay Commented following condition
                            ////[2021-10-13] Dhananjay Added
                            //if (TblLoadingSlipTO.IsConfirmed != 1)
                            //{
                            //    templateName = "WeighingSlipNonConfirm";
                            //}
                        }

                        //[2021-10-13] Dhananjay commented
                        //if (TblLoadingSlipTO.IsConfirmed != 1)
                        //{
                        //    templateName = "WeighingSlipNonConfirm";
                        //}

                        //creating template'''''''''''''''''
                        String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                        String fileName = "Bill-" + DateTime.Now.Ticks;

                        //if(isSendEmailForWeighment)
                        //{
                        //    templateName = "WeighmentSlip";                      
                        //    templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                        //    fileName = "Bill-" + DateTime.Now.Ticks;
                        //}

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

                            if (isFileDelete)
                            {
                                //driveName + path;
                                Byte[] bytes = DeleteFile(saveLocation, filePath);

                                if (bytes != null && bytes.Length > 0)
                                {
                                    resultMessage.Tag = Convert.ToBase64String(bytes);
                                }
                            }
                            else
                                resultMessage.Tag = filePath;

                            //Reshma Cooment for send WatsMsg
                            //Byte[] bytes = DeleteFile(saveLocation, filePath);
                            //if (bytes != null && bytes.Length > 0)
                            //{
                            //    resultMessage.Tag = Convert.ToBase64String(bytes);
                            //}

                            if (resultMessage.MessageType == ResultMessageE.Information)
                            {
                                resultMessage.DefaultSuccessBehaviour();
                            }
                        }
                    }
                    return resultMessage;

                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PrintWeighingReport");
                return resultMessage;
            }
            finally
            {

            }
        }

        public String currencyTowords(Double amount, Int32 currencyId)
        {

            String currency = "";
            if (amount > 0)
            {
                double m = Convert.ToInt64(Math.Floor(amount));
                double l = amount;

                double j = (l - m) * 100;


                var beforefloating = ConvertNumbertoWords(Convert.ToInt64(m));


                var afterfloating = ConvertNumbertoWords(Convert.ToInt64(j));

                string paiseWord = "Only";

                if (j > 0 && currencyId == (Int32)Constants.CurrencyE.INR)
                {
                    paiseWord = afterfloating + ' ' + " Paise Only";

                }


                switch (currencyId)
                {
                    case (Int32)Constants.CurrencyE.INR:
                        //if (firmNameId == (Int32)Constants.FirmNameE.BHAGYALAXMI)
                        //{
                        //    currency = beforefloating + ' ' + " Rupee" + paiseWord;
                        //}
                        //if (firmNameId == (Int32)Constants.FirmNameE.KALIKA)
                        //{
                        //    currency = " Rs " + beforefloating + paiseWord;
                        //}
                        //if (firmNameId == (Int32)Constants.FirmNameE.SRJ || firmNameId == (Int32)Constants.FirmNameE.Parameshwar)
                        //{
                        currency = " Rs " + beforefloating + " " + paiseWord;
                        //}


                        break;
                    case (Int32)Constants.CurrencyE.USD:

                        //if (firmNameId == (Int32)Constants.FirmNameE.BHAGYALAXMI)
                        //{
                        //    currency = beforefloating + " Dollar " + " Only";
                        //}
                        //if (firmNameId == (Int32)Constants.FirmNameE.KALIKA)
                        //{
                        //    currency = " Dollar " + beforefloating + paiseWord;
                        //}
                        //if ((firmNameId == (Int32)Constants.FirmNameE.SRJ) || (firmNameId== (Int32)Constants.FirmNameE.Parameshwar))
                        //{
                        currency = " Dollar " + beforefloating + paiseWord;
                        // }

                        break;

                }


            }


            return currency;
        }


        public string ConvertNumbertoWords(long number)
        {

            if (number == 0) return "Zero";
            if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";



            if ((number / 100000000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000000) + " Arab ";
                number %= 100000000;
            }

            if ((number / 10000000) > 0)
            {
                words += ConvertNumbertoWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " Hundred ";
                number %= 100;
            }
            //if ((number / 10) > 0)  
            //{  
            // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
            // number %= 10;  
            //}  
            if (number > 0)
            {
                if (words != "") words += " And ";
                var unitsMap = new[]
                {
            "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
        };
                var tensMap = new[]
                {
            "Zero", "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
        };
                if (number < 20) words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }



        public Double getDiscountPerct(TblInvoiceTO resInvoiceTO)
        {
            Double discountPer = 0;
            if (resInvoiceTO.DiscountAmt > 0 && resInvoiceTO.BasicAmt > 0)
            {
                discountPer = resInvoiceTO.DiscountAmt * 100 / resInvoiceTO.BasicAmt;
                discountPer = Math.Round(discountPer, 2);
            }
            return discountPer;

        }

        public Boolean GetTaxPctAgaintInvoice(TblInvoiceTO tblInvoiceTO, out Double igstPct, out Double cgstPct, out Double sgstPct)
        {
            sgstPct = 0;
            cgstPct = 0;
            igstPct = 0;

            List<TblInvoiceItemDetailsTO> groupByTaxPct = tblInvoiceTO.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId == 0).GroupBy(g => g.InvoiceItemTaxDtlsTOList[0].TaxRatePct).Select(s => s.FirstOrDefault()).ToList();
            if (groupByTaxPct == null || groupByTaxPct.Count == 0)
            {
                return true;
            }
            if (groupByTaxPct.Count > 1)
            {
                return true;
            }

            if (groupByTaxPct.Count == 1)
            {
                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = groupByTaxPct[0];
                if (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList == null || tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count == 0)
                {
                    return true;
                }
                TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Where(w => w.TaxTypeId == (int)Constants.TaxTypeE.IGST).FirstOrDefault();
                if (tblInvoiceItemTaxDtlsTO != null)
                {
                    igstPct = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                }
                tblInvoiceItemTaxDtlsTO = tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Where(w => w.TaxTypeId == (int)Constants.TaxTypeE.CGST).FirstOrDefault();
                if (tblInvoiceItemTaxDtlsTO != null)
                {
                    cgstPct = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                }
                tblInvoiceItemTaxDtlsTO = tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Where(w => w.TaxTypeId == (int)Constants.TaxTypeE.SGST).FirstOrDefault();
                if (tblInvoiceItemTaxDtlsTO != null)
                {
                    sgstPct = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                }

                if (igstPct == 0)
                {
                    igstPct = cgstPct + sgstPct;
                }
                if (cgstPct == 0)
                {
                    if (igstPct > 0)
                    {
                        cgstPct = igstPct / 2;
                        sgstPct = cgstPct;
                    }
                }
            }

            return true;
        }

        public DataTable getCommercialDT(TblInvoiceTO tblInvoiceTO)
        {
            DataTable commercialValueDT = new DataTable();
            commercialValueDT.TableName = "commercialDT";
            commercialValueDT.Columns.Add("Name");
            commercialValueDT.Columns.Add("Value", typeof(double));
            Int32 commercialDTCount = 0;
            if (tblInvoiceTO.DiscountAmt > 0)
            {
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;

                commercialValueDT.Rows[commercialDTCount]["Name"] = "Discount (" + Math.Round(getDiscountPerct(tblInvoiceTO), 2) + "%)";
                commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(tblInvoiceTO.DiscountAmt, 2);


            }

            var freightResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.FREIGHT).FirstOrDefault();
            if (freightResTO != null && freightResTO.TaxableAmt > 0)
            {
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                commercialValueDT.Rows[commercialDTCount]["Name"] = "Freight/Other";

                commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(freightResTO.TaxableAmt, 2);


            }
            var pfResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.PF).FirstOrDefault();
            if (pfResTO != null && pfResTO.TaxableAmt > 0)
            {
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                commercialValueDT.Rows[commercialDTCount]["Name"] = "PF";

                commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(pfResTO.TaxableAmt, 2);




            }
            var cessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.CESS).FirstOrDefault();
            if (cessResTO != null && cessResTO.TaxableAmt > 0)
            {
                double cessPercentage = cessResTO.TaxPct;
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                if (cessPercentage > 0)
                    commercialValueDT.Rows[commercialDTCount]["Name"] = cessResTO.ProdItemDesc + "@" + cessPercentage + "%";
                else
                    commercialValueDT.Rows[commercialDTCount]["Name"] = cessResTO.ProdItemDesc + "@";

                commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(cessResTO.TaxableAmt, 2);



            }

            commercialValueDT.Rows.Add();
            commercialDTCount = commercialValueDT.Rows.Count - 1;
            commercialValueDT.Rows[commercialDTCount]["Name"] = "Taxable Value";

            commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(tblInvoiceTO.TaxableAmt, 2);


            Double igstPct, cgstPct, sgstPct = 0;
            GetTaxPctAgaintInvoice(tblInvoiceTO, out igstPct, out cgstPct, out sgstPct);



            commercialValueDT.Rows.Add();
            commercialDTCount = commercialValueDT.Rows.Count - 1;

            commercialValueDT.Rows[commercialDTCount]["Name"] = "CGST";
            if (cgstPct > 0)
                commercialValueDT.Rows[commercialDTCount]["Name"] = "CGST @ " + cgstPct + "%";

            commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(tblInvoiceTO.CgstAmt, 2);



            commercialValueDT.Rows.Add();
            commercialDTCount = commercialValueDT.Rows.Count - 1;
            commercialValueDT.Rows[commercialDTCount]["Name"] = "SGST";
            if (sgstPct > 0)
                commercialValueDT.Rows[commercialDTCount]["Name"] = "SGST @ " + sgstPct + "%";

            commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(tblInvoiceTO.SgstAmt, 2);


            commercialValueDT.Rows.Add();
            commercialDTCount = commercialValueDT.Rows.Count - 1;
            commercialValueDT.Rows[commercialDTCount]["Name"] = "IGST";
            if (igstPct > 0)
                commercialValueDT.Rows[commercialDTCount]["Name"] = "IGST @ " + igstPct + "%";

            commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(tblInvoiceTO.IgstAmt, 2);


            var afterCessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.AFTERCESS).FirstOrDefault();
            if (afterCessResTO != null && afterCessResTO.TaxableAmt > 0)
            {
                double taxPercentage = afterCessResTO.TaxPct;
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                if (taxPercentage > 0)
                {
                    commercialValueDT.Rows[commercialDTCount]["Name"] = afterCessResTO.ProdItemDesc + "@" + taxPercentage + "%";
                }
                else
                {
                    commercialValueDT.Rows[commercialDTCount]["Name"] = afterCessResTO.ProdItemDesc + "@";
                }

                commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(afterCessResTO.TaxableAmt, 2);


            }

            return commercialValueDT;
        }

        public DataTable getHsnItemTaxDT(TblInvoiceTO tblInvoiceTO)
        {
            DataTable hsnItemTaxDT = new DataTable();
            hsnItemTaxDT.TableName = "hsnItemTaxDT";
            hsnItemTaxDT.Columns.Add("hsnNo");
            hsnItemTaxDT.Columns.Add("hsntaxableAmt", typeof(double));
            hsnItemTaxDT.Columns.Add("cgstAmt", typeof(double));
            hsnItemTaxDT.Columns.Add("sgstAmt", typeof(double));
            hsnItemTaxDT.Columns.Add("igstAmt", typeof(double));
            hsnItemTaxDT.Columns.Add("taxTotal", typeof(double));
            hsnItemTaxDT.Columns.Add("cgstPct", typeof(double));
            hsnItemTaxDT.Columns.Add("sgstPct", typeof(double));
            hsnItemTaxDT.Columns.Add("igstPct", typeof(double));
            int isMathRounfOff = 0;
            TblConfigParamsTO tblConfigParams = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_TAX_ON_PRINT_INVOICE);
            if (tblConfigParams != null)
            {
                if (tblConfigParams.ConfigParamVal == "1")
                {
                    isMathRounfOff = 1;
                }
            }
            List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = _iTblInvoiceItemTaxDtlsBL.SelectInvoiceItemTaxDtlsListByInvoiceId(tblInvoiceTO.IdInvoice);
            if (tblInvoiceItemTaxDtlsTOList != null && tblInvoiceItemTaxDtlsTOList.Count > 0)
            {

                //Aniket [12-03-2019]
                tblInvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList.Where(x => x.IsAfter == 0).ToList();
                 
                List<TblInvoiceItemTaxDtlsTO> distinctHsnItemTaxListV1 = tblInvoiceItemTaxDtlsTOList.GroupBy(w => w.GstinCodeNo).Select(s => s.FirstOrDefault()).ToList();
                if (distinctHsnItemTaxListV1 != null && distinctHsnItemTaxListV1.Count > 0)
                {
                    List<TblInvoiceItemTaxDtlsTO> distinctHsnItemTaxList = distinctHsnItemTaxListV1.GroupBy(w => w.TaxRatePct).Select(s => s.FirstOrDefault()).ToList();
                    {
                        if (distinctHsnItemTaxList != null && distinctHsnItemTaxList.Count > 0)
                        {
                            Double cgstAmt = 0, sgstAmt = 0, igstAmt = 0, taxTotal = 0, hsntaxableAmt = 0;
                            Double cgstPct = 0, sgstPct = 0, igstPct = 0;
                            for (int m = 0; m < distinctHsnItemTaxList.Count; m++)
                            {
                                cgstAmt = 0; sgstAmt = 0; taxTotal = 0; hsntaxableAmt = 0; ;
                                cgstPct = 0; sgstPct = 0; igstPct = 0;
                                List<TblInvoiceItemTaxDtlsTO> tempInvoiceTaxList = tblInvoiceItemTaxDtlsTOList.Where(oi => oi.GstinCodeNo == distinctHsnItemTaxList[m].GstinCodeNo).ToList();

                                for (int n = 0; n < tempInvoiceTaxList.Count; n++)
                                {
                                    TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = tempInvoiceTaxList[n];
                                    if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                    {
                                        cgstAmt += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        taxTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        cgstPct = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                                    }
                                    if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                    {
                                        sgstAmt += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        taxTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        sgstPct  = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                                    }
                                    if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                    {
                                        igstAmt += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        igstPct  = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                                        //taxTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    }


                                }



                                hsnItemTaxDT.Rows.Add();
                                Int32 invoiceItemDTCount = hsnItemTaxDT.Rows.Count - 1;
                                List<TblInvoiceItemTaxDtlsTO> distinctItemList = tempInvoiceTaxList.GroupBy(w => w.InvoiceItemId).Select(s => s.FirstOrDefault()).ToList();
                                if (distinctItemList != null && distinctItemList.Count > 0)
                                {
                                    for (int p = 0; p < distinctItemList.Count; p++)
                                    {
                                        TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = distinctItemList[p];
                                        hsntaxableAmt += tblInvoiceItemTaxDtlsTO.TaxableAmt;

                                    }

                                    hsnItemTaxDT.Rows[invoiceItemDTCount]["hsntaxableAmt"] = Math.Round(hsntaxableAmt, 2);


                                }

                                hsnItemTaxDT.Rows[invoiceItemDTCount]["hsnNo"] = distinctHsnItemTaxList[m].GstinCodeNo;
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["cgstAmt"] = Math.Round(cgstAmt, 2);
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["sgstAmt"] = Math.Round(sgstAmt, 2);
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["igstAmt"] = Math.Round(igstAmt, 2);
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["taxtotal"] = Math.Round(taxTotal, 2);
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["cgstPct"] = cgstPct ;
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["sgstPct"] = sgstPct;
                                hsnItemTaxDT.Rows[invoiceItemDTCount]["igstPct"] =igstPct;


                            }
                        }
                    }
                }
            }
            Console.WriteLine(hsnItemTaxDT);
            return hsnItemTaxDT;

        }



        #endregion

        #region Updation
        public int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO)
        {
            return _iTblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO);
        }

        public int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO, conn, tran);
        }
        public int UpdateMappedSAPInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.UpdateMappedSAPInvoiceNo(tblInvoiceTO, conn, tran);
        }
        public ResultMessage SaveUpdatedInvoice(TblInvoiceTO invoiceTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMSg = UpdateInvoice(invoiceTO, conn, tran);
                if (resultMSg.MessageType == ResultMessageE.Information)
                {
                    tran.Commit();
                    resultMSg.DefaultSuccessBehaviour();
                }
                else
                {
                    tran.Rollback();
                }
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage UpdateInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered In The Loop";
            Boolean isAddrChanged = false;
            Boolean isCdChanged = false;
            Boolean isRateChange = false;
            String changeIn = string.Empty;
            DateTime serverDateTime = _iCommon.ServerDateTime;
            try
            {
                //Vijaymala[23-03-2016]added to check invoice details of igst,cgst,sgst taxes
                #region To check invoice details is valid or not
                string errorMsg = string.Empty;
                Boolean isValidInvoice = CheckInvoiceDetailsAccToState(tblInvoiceTO, ref errorMsg);
                if (!isValidInvoice)
                {
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                    return resultMessage;
                }
                #endregion

                #region 1. Update the Invoice
                List<TblInvoiceHistoryTO> invHistoryList = new List<TblInvoiceHistoryTO>();
                TblInvoiceTO existingInvoiceTO = SelectTblInvoiceTOWithDetails(tblInvoiceTO.IdInvoice, conn, tran);
                if (existingInvoiceTO == null)
                {
                    resultMessage.DefaultBehaviour("existingInvoiceTO Object Not Found");
                    return resultMessage;
                }
                //TblInvoiceTO invoiceTO = BL.TblInvoiceBL.SelectTblInvoiceTO(invoiceId);
                if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                {
                    TblInvoiceTO existStatusinvoiceTO = SelectTblInvoiceTO(tblInvoiceTO.IdInvoice);
                    if (existStatusinvoiceTO.StatusId == tblInvoiceTO.StatusId)
                    {
                        tblInvoiceTO.StatusId = (int)Constants.InvoiceStatusE.NEW;
                    }
                }

                /*GJ@20170926: Update the Rate if Status is Rejected By Director Status*/
                if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR)
                {

                    existingInvoiceTO = updateInvoiceToCalc(existingInvoiceTO, conn, tran);
                    tblInvoiceTO.BasicAmt = existingInvoiceTO.BasicAmt;
                    tblInvoiceTO.TaxableAmt = existingInvoiceTO.TaxableAmt;
                    tblInvoiceTO.DiscountAmt = existingInvoiceTO.DiscountAmt;
                    tblInvoiceTO.IgstAmt = existingInvoiceTO.IgstAmt;
                    tblInvoiceTO.CgstAmt = existingInvoiceTO.CgstAmt;
                    tblInvoiceTO.SgstAmt = existingInvoiceTO.SgstAmt;

                    //double finalGrandTotal = Math.Round(existingInvoiceTO.GrandTotal);
                    //tblInvoiceTO.GrandTotal = finalGrandTotal;
                    //tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - existingInvoiceTO.GrandTotal, 2);

                    RoundOffInvoiceValuesBySetting(tblInvoiceTO);

                    //tblInvoiceTO.GrandTotal = existingInvoiceTO.GrandTotal;

                    tblInvoiceTO.InvoiceItemDetailsTOList = existingInvoiceTO.InvoiceItemDetailsTOList;
                    Double tdsTaxPct = 0;
                    if (existingInvoiceTO != null)
                    {
                        if (existingInvoiceTO.IsTcsApplicable == 0)
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
                    tblInvoiceTO.TdsAmt = 0;
                    if (existingInvoiceTO.IsConfirmed == 1 && existingInvoiceTO.InvoiceTypeE != Constants.InvoiceTypeE.SEZ_WITHOUT_DUTY)
                    {
                        tblInvoiceTO.TdsAmt = (CalculateTDS(tblInvoiceTO) * tdsTaxPct) / 100;
                        tblInvoiceTO.TdsAmt = Math.Ceiling(tblInvoiceTO.TdsAmt);
                    }
                }

                RemoveIotFieldsFromDB(tblInvoiceTO);
                //Saket [2018-04-03] Added. 

                Int32 skiploadingApproval = 0;


                TblConfigParamsTO tblConfigParamsTOApproval = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_SKIP_INVOICE_APPROVAL, conn, tran);
                if (tblConfigParamsTOApproval != null)
                {
                    skiploadingApproval = Convert.ToInt32(tblConfigParamsTOApproval.ConfigParamVal);
                    if (tblInvoiceTO.CheckSkipApprovalCondition == 0)
                    {
                        if (skiploadingApproval == 1)
                        {
                            if (tblInvoiceTO.StatusId == (Int32)Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR)
                            {
                                tblInvoiceTO.StatusId = Convert.ToInt32(Constants.InvoiceStatusE.NEW);
                            }
                            else
                                tblInvoiceTO.StatusId = Convert.ToInt32(Constants.InvoiceStatusE.ACCEPTED_BY_DISTRIBUTOR);
                        }
                    }
                }
                result = UpdateTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in UpdateTblInvoice InvoiceTbl");
                    return resultMessage;
                }
                #endregion
                #region 2. Save the Address Details
                if (tblInvoiceTO.StatusId == Convert.ToInt32(Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR) && tblInvoiceTO.InvoiceItemDetailsTOList == null && tblInvoiceTO.InvoiceAddressTOList == null)
                {
                    tblInvoiceTO.InvoiceAddressTOList = _iTblInvoiceAddressDAO.SelectAllTblInvoiceAddress(tblInvoiceTO.IdInvoice, conn, tran);
                    List<TblInvoiceItemDetailsTO> itemList = _iTblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(tblInvoiceTO.IdInvoice);
                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].InvoiceItemTaxDtlsTOList = _iTblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(itemList[i].IdInvoiceItem);
                        }
                        tblInvoiceTO.InvoiceItemDetailsTOList = itemList;
                    }
                }

                List<string> addrChangedPropertiesList = new List<string>();
                // If condition added by Aniket [8-7-2019] as suggested by Saket
                if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceTO.InvoiceAddressTOList.Count; i++)
                    {
                        TblInvoiceAddressTO newAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                        TblInvoiceAddressTO addrTO = existingInvoiceTO.InvoiceAddressTOList.Where(a => a.IdInvoiceAddr == tblInvoiceTO.InvoiceAddressTOList[i].IdInvoiceAddr).FirstOrDefault();
                        if (addrTO != null)
                        {
                            addrChangedPropertiesList = Constants.GetChangedProperties(addrTO, tblInvoiceTO.InvoiceAddressTOList[i]);
                            result = _iTblInvoiceAddressBL.UpdateTblInvoiceAddress(tblInvoiceTO.InvoiceAddressTOList[i], conn, tran);
                        }
                        else
                        {

                            result = _iTblInvoiceAddressBL.InsertTblInvoiceAddress(tblInvoiceTO.InvoiceAddressTOList[i], conn, tran);
                        }
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error in Insert UpdateTblInvoiceAddress");
                            return resultMessage;
                        }

                        if (addrChangedPropertiesList != null && addrChangedPropertiesList.Count > 0)
                        {
                            TblInvoiceHistoryTO addrHistoryTO = new TblInvoiceHistoryTO();
                            addrHistoryTO.InvoiceId = tblInvoiceTO.IdInvoice;
                            addrHistoryTO.BookingCommentCategoryId = tblInvoiceTO.BookingCommentCategoryId;
                            addrHistoryTO.BookingTaxCategoryId  = tblInvoiceTO.BookingTaxCategoryId;

                            if (addrTO.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                            {
                                addrHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                                addrHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                                addrHistoryTO.OldBillingAddr = addrTO.BillingName + "|" + addrTO.BillingOrgId + "|" + addrTO.GstinNo + "|" + addrTO.PanNo + "|" + addrTO.AadharNo + "|" + addrTO.ContactNo + "|" + addrTO.Address + "|" + addrTO.Taluka + "|" + addrTO.TalukaId + "|" + addrTO.District + "|" + addrTO.DistrictId + "|" + addrTO.State + "|" + addrTO.StateId + "|" + addrTO.PinCode + "|" + addrTO.CountryId;
                                addrHistoryTO.NewBillingAddr = newAddrTO.BillingName + "|" + newAddrTO.BillingOrgId + "|" + newAddrTO.GstinNo + "|" + newAddrTO.PanNo + "|" + newAddrTO.AadharNo + "|" + newAddrTO.ContactNo + "|" + newAddrTO.Address + "|" + newAddrTO.Taluka + "|" + newAddrTO.TalukaId + "|" + newAddrTO.District + "|" + newAddrTO.DistrictId + "|" + newAddrTO.State + "|" + newAddrTO.StateId + "|" + newAddrTO.PinCode + "|" + newAddrTO.CountryId;
                                addrHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                                addrHistoryTO.StatusId = tblInvoiceTO.StatusId;
                            }
                            else
                            {
                                addrHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                                addrHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                                addrHistoryTO.OldConsinAddr = addrTO.BillingName + "|" + addrTO.BillingOrgId + "|" + addrTO.GstinNo + "|" + addrTO.PanNo + "|" + addrTO.AadharNo + "|" + addrTO.ContactNo + "|" + addrTO.Address + "|" + addrTO.Taluka + "|" + addrTO.TalukaId + "|" + addrTO.District + "|" + addrTO.DistrictId + "|" + addrTO.State + "|" + addrTO.StateId + "|" + addrTO.PinCode + "|" + addrTO.CountryId;
                                addrHistoryTO.NewConsinAddr = newAddrTO.BillingName + "|" + newAddrTO.BillingOrgId + "|" + newAddrTO.GstinNo + "|" + newAddrTO.PanNo + "|" + newAddrTO.AadharNo + "|" + newAddrTO.ContactNo + "|" + newAddrTO.Address + "|" + newAddrTO.Taluka + "|" + newAddrTO.TalukaId + "|" + newAddrTO.District + "|" + newAddrTO.DistrictId + "|" + newAddrTO.State + "|" + newAddrTO.StateId + "|" + newAddrTO.PinCode + "|" + newAddrTO.CountryId;
                                addrHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                                addrHistoryTO.StatusId = tblInvoiceTO.StatusId;
                            }

                            isAddrChanged = true;
                            changeIn += "ADDRESS|";
                            invHistoryList.Add(addrHistoryTO);
                        }
                    }
                }



                #endregion


                #region 3. Save the Invoice Item Details

                if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                {
                    #region Delete Previous Tax Details


                    //Saket [2017-11-22] Added For Edit Option in for state.
                    result = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTO.IdInvoice, conn, tran);
                    if (result == -1)
                    {
                        resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                        return resultMessage;
                    }

                    #endregion

                    //if (tblInvoiceTO.InvoiceItemDetailsTOList == null || tblInvoiceTO.InvoiceItemDetailsTOList.Count == 0)
                    //{
                    //    resultMessage.DefaultBehaviour("Error : Invoice Item Det List Found Empty Or Null");
                    //    return resultMessage;
                    //}

                    //Ramdas [2017-12-14]  Delete Record if exist.
                    for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                    {
                        TblInvoiceItemDetailsTO tblInvoiceItemDetailsTOResult = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                        if (tblInvoiceItemDetailsTOResult != null && tblInvoiceItemDetailsTOResult.OtherTaxId != 0)
                        {

                            List<TblInvoiceItemDetailsTO> temp = tblInvoiceTO.InvoiceItemDetailsTOList.Where(w => w.OtherTaxId == tblInvoiceItemDetailsTOResult.OtherTaxId).ToList();
                            if (temp != null && temp.Count > 1)
                            {
                                resultMessage.DefaultBehaviour("Other Tax type is double - " + tblInvoiceItemDetailsTOResult.ProdItemDesc);
                                return resultMessage;
                            }
                        }
                    }
                    for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                    {

                        var exInvItemTO = existingInvoiceTO.InvoiceItemDetailsTOList.Where(ei => ei.IdInvoiceItem == tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem).FirstOrDefault();
                        if (exInvItemTO == null)
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = new TblInvoiceItemDetailsTO();
                            tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                            tblInvoiceItemDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;


                            result = _iTblInvoiceItemDetailsBL.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert InvoiceItemDetailTbl");
                                return resultMessage;
                            }
                            #region 1. Save the Invoice Tax Item Details
                            if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList == null
                                || tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count == 0)
                            {
                                if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.REGULAR_TAX_INVOICE
                                    || tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.SEZ_WITH_DUTY)
                                {
                                    resultMessage.DefaultBehaviour("Error : Invoice Item Det Tax List Found Empty Or Null");
                                    return resultMessage;
                                }
                            }

                            for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count; j++)
                            {
                                tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j].InvoiceItemId = tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem;
                                result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error in Insert InvoiceItemTaxDetailTbl");
                                    return resultMessage;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceTO.InvoiceItemDetailsTOList[i];
                            tblInvoiceItemDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;

                            result = _iTblInvoiceItemDetailsBL.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in update UpdateTblInvoiceItemDetails");
                                return resultMessage;
                            }
                            #region 1. Save the Invoice Tax Item Details                       

                            for (int j = 0; j < tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count; j++)
                            {
                                tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j].InvoiceItemId = tblInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem;

                                //Saket [2017-11-22] Added For Edit Option in for state.
                                //result = _iTblInvoiceItemTaxDtlsBL.UpdateTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                                result = _iTblInvoiceItemTaxDtlsBL.InsertTblInvoiceItemTaxDtls(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[j], conn, tran);
                                if (result != 1)
                                {
                                    resultMessage.DefaultBehaviour("Error in Insert UpdateTblInvoiceItemTaxDtls");
                                    return resultMessage;
                                }
                            }

                            if ((exInvItemTO.Rate != tblInvoiceItemDetailsTO.Rate)
                                || (exInvItemTO.CdStructure != tblInvoiceItemDetailsTO.CdStructure))
                            {
                                TblInvoiceHistoryTO historyTO = new TblInvoiceHistoryTO();
                                historyTO.InvoiceId = tblInvoiceTO.IdInvoice;
                                historyTO.InvoiceItemId = tblInvoiceItemDetailsTO.IdInvoiceItem;
                                if (exInvItemTO.Rate != tblInvoiceItemDetailsTO.Rate)
                                {
                                    historyTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                                    historyTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                                    historyTO.OldUnitRate = exInvItemTO.Rate;
                                    historyTO.NewUnitRate = tblInvoiceItemDetailsTO.Rate;
                                    historyTO.StatusDate = tblInvoiceTO.UpdatedOn;
                                    historyTO.StatusId = tblInvoiceTO.StatusId;
                                    isRateChange = true;
                                    changeIn += "RATE|";

                                }

                                if (exInvItemTO.CdStructure != tblInvoiceItemDetailsTO.CdStructure)
                                {
                                    historyTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                                    historyTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                                    historyTO.OldCdStructureId = exInvItemTO.CdStructureId;
                                    historyTO.NewCdStructureId = tblInvoiceItemDetailsTO.CdStructureId;
                                    historyTO.StatusDate = tblInvoiceTO.UpdatedOn;
                                    historyTO.StatusId = tblInvoiceTO.StatusId;
                                    isCdChanged = true;
                                    changeIn += "CD|";
                                }

                                invHistoryList.Add(historyTO);
                            }

                        }

                        #endregion
                    }



                    for (int i = 0; i < existingInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                    {
                        var exInvItemTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ei => ei.IdInvoiceItem == existingInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem).FirstOrDefault();
                        if (exInvItemTO == null)
                        {
                            result = _iTblInvoiceHistoryBL.DeleteTblInvoiceHistoryByItemId(existingInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem, conn, tran);
                            if (result == -1)
                            {
                                resultMessage.DefaultBehaviour("Error in Delete DeleteTblInvoiceHistoryByItemId");
                                return resultMessage;
                            }

                            result = _iTblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetails(existingInvoiceTO.InvoiceItemDetailsTOList[i].IdInvoiceItem, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Delete DeleteTblInvoiceItemDetails");
                                return resultMessage;
                            }
                        }

                    }



                }
                #endregion

                if (invHistoryList != null && invHistoryList.Count > 0)
                {
                    for (int i = 0; i < invHistoryList.Count; i++)
                    {
                        result = _iTblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryList[i], conn, tran);
                        if (result != 1)
                        {
                            resultMessage.DefaultBehaviour("Error while InsertTblInvoiceHistory");
                            return resultMessage;
                        }
                    }
                }

                #region Priyanka [22-01-2019] added To Save commercial Terms

                if (tblInvoiceTO.PaymentTermOptionRelationTOLst != null && tblInvoiceTO.PaymentTermOptionRelationTOLst.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceTO.PaymentTermOptionRelationTOLst.Count; i++)
                    {
                        TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTO = _iTblPaymentTermOptionRelationBL.SelectTblPaymentTermOptionRelationTOByInvoiceId(tblInvoiceTO.PaymentTermOptionRelationTOLst[i].InvoiceId, tblInvoiceTO.PaymentTermOptionRelationTOLst[i].PaymentTermId);
                        if (tblPaymentTermOptionRelationTO != null)
                        {
                            tblPaymentTermOptionRelationTO.IsActive = 0;
                            tblPaymentTermOptionRelationTO.UpdatedBy = tblInvoiceTO.UpdatedBy;
                            tblPaymentTermOptionRelationTO.UpdatedOn = _iCommon.ServerDateTime;
                            result = _iTblPaymentTermOptionRelationBL.UpdateTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Text = "Error While UpdateTblPaymentTermOptionRelation";
                                return resultMessage;
                            }
                        }
                    }
                    //}
                    //if (tblBookingsTO.PaymentTermOptionRelationTOLst != null && tblBookingsTO.PaymentTermOptionRelationTOLst.Count > 0)
                    //{
                    for (int j = 0; j < tblInvoiceTO.PaymentTermOptionRelationTOLst.Count; j++)
                    {
                        TblPaymentTermOptionRelationTO tblPaymentTermOptionRelationTONew = tblInvoiceTO.PaymentTermOptionRelationTOLst[j];
                        tblPaymentTermOptionRelationTONew.CreatedBy = tblInvoiceTO.CreatedBy;
                        tblPaymentTermOptionRelationTONew.CreatedOn = _iCommon.ServerDateTime;

                        result = _iTblPaymentTermOptionRelationBL.InsertTblPaymentTermOptionRelation(tblPaymentTermOptionRelationTONew, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.MessageType = ResultMessageE.Error;
                            resultMessage.Text = "Error While InsertTblPaymentTermOptionRelation";
                            return resultMessage;
                        }
                    }
                }
                #endregion

                #region Notifications & SMSs

                if (tblInvoiceTO.CheckSkipApprovalCondition == 1 && skiploadingApproval == 1)
                {
                    resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                    if (resultMessage.MessageType != ResultMessageE.Information)
                    {
                        return resultMessage;
                    }

                    goto exitNotification;
                }

                //Aniket [6-8-2019] added to send alert and sms notifications dynamically
                List<TblAlertDefinitionTO> tblAlertDefinitionTOList = _iTblAlertDefinitionDAO.SelectAllTblAlertDefinition();

                //Vijaymala added[03-05-2018]to change  notification with party name
                TblConfigParamsTO dealerNameConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_ADD_DEALER_IN_NOTIFICATION, conn, tran);
                Int32 dealerNameActive = 0;
                if (dealerNameConfTO != null)
                    dealerNameActive = Convert.ToInt32(dealerNameConfTO.ConfigParamVal);

                if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.NEW
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.ACCEPTED_BY_DISTRIBUTOR
                    || tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)

                {

                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                    List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                    TblUserTO userTO = _iTblUserDAO.SelectTblUser(tblInvoiceTO.CreatedBy, conn, tran);
                    if (userTO != null)
                    {
                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                        tblAlertUsersTO.UserId = userTO.IdUser;
                        tblAlertUsersTO.DeviceId = userTO.RegisteredDeviceId;
                        tblAlertUsersTOList.Add(tblAlertUsersTO);
                    }

                    if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.NEW)
                    {
                        tblInvoiceTO.ChangeIn = changeIn;

                        //[24-01-2018] Vijaymala:Added cd change condition to update invoice to invoice approval
                        if (isRateChange || isCdChanged)
                        {
                            var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.INVOICE_APPROVAL_REQUIRED);
                            string tempTxt = "";

                            tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION;
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_APPROVAL_REQUIRED;
                            tblAlertInstanceTO.AlertAction = "INVOICE_APPROVAL_REQUIRED";
                            if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                            {
                                tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                                tempTxt = tempTxt.Replace("@InvoiceIdStr", tblInvoiceTO.IdInvoice.ToString());
                                tempTxt = tempTxt.Replace("@DealerNameStr", "");

                                tblAlertInstanceTO.AlertComment = tempTxt;
                            }
                            else
                                tblAlertInstanceTO.AlertComment = "Approval Required For Invoice #" + tblInvoiceTO.IdInvoice;

                            if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                            {
                                tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);
                                tblAlertInstanceTO.SmsComment = tempTxt;
                                // tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                            }
                            resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return resultMessage;
                            }
                        }
                        //else if (!isRateChange && (isCdChanged || isAddrChanged))
                        else
                        {
                            List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(tblInvoiceTO.DistributorOrgId, conn, tran);
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
                            var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTANCE_REQUIRED);
                            string tempTxt = "";

                            tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE;
                            tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTANCE_REQUIRED;
                            tblAlertInstanceTO.AlertAction = "INVOICE_ACCEPTANCE_REQUIRED";
                            if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                            {
                                tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                                tempTxt = tempTxt.Replace("@InvoiceIdStr", tblInvoiceTO.IdInvoice.ToString());
                                tempTxt = tempTxt.Replace("@DealerNameStr", "");

                                tblAlertInstanceTO.AlertComment = tempTxt;
                            }
                            else
                                tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " is awaiting for acceptance";

                            if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                            {
                                tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);
                                tblAlertInstanceTO.SmsComment = tempTxt;
                                // tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                            }
                            resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                return resultMessage;
                            }

                        }
                        //else
                        //{
                        //    goto exitNotification;
                        //}
                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.AUTHORIZED_BY_DIRECTOR)
                    {
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.INVOICE_APPROVED_BY_DIRECTOR);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_APPROVED_BY_DIRECTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_APPROVED_BY_DIRECTOR";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@InvoiceIdStr", tblInvoiceTO.IdInvoice.ToString());
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Approved.";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            //tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                        }
                        resultMessage = CheckAndUpdateForInvoiceAcceptanceStatus(existingInvoiceTO, tblInvoiceTO, tblAlertInstanceTO, tblAlertUsersTOList, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                            return resultMessage;
                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR)
                    {
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DIRECTOR);
                        string tempTxt = "";

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DIRECTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_REJECTED_BY_DIRECTOR";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@InvoiceIdStr", tblInvoiceTO.IdInvoice.ToString());
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Rejected by Director";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            //tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                        }
                        resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                        //resultMessage = CheckAndUpdateForInvoiceAcceptanceStatus(existingInvoiceTO, tblInvoiceTO, tblAlertInstanceTO, tblAlertUsersTOList, conn, tran);
                        //if (resultMessage.MessageType != ResultMessageE.Information)
                        //{
                        //    return resultMessage;
                        //}

                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                    {
                        //Aniket [6-8-2019] commented as NotificationsE should be INVOICE_REJECTED_BY_DISTRIBUTOR
                        // tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DIRECTOR;
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DISTRIBUTOR);
                        string tempTxt = "";

                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DISTRIBUTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_REJECTED_BY_DISTRIBUTOR";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@InvoiceIdStr", tblInvoiceTO.IdInvoice.ToString());
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Rejected by Distributer";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            // tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                        }
                        resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                        //resultMessage = CheckAndUpdateForInvoiceAcceptanceStatus(existingInvoiceTO, tblInvoiceTO, tblAlertInstanceTO, tblAlertUsersTOList, conn, tran);
                        //if (resultMessage.MessageType != ResultMessageE.Information)
                        //{
                        //    return resultMessage;
                        //}

                    }
                    else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.ACCEPTED_BY_DISTRIBUTOR)
                    {
                        var tblAlertDefinitionTO = tblAlertDefinitionTOList.Find(x => x.IdAlertDef == (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTED_BY_DISTRIBUTOR);
                        string tempTxt = "";
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTED_BY_DISTRIBUTOR;
                        tblAlertInstanceTO.AlertAction = "INVOICE_ACCEPTED_BY_DISTRIBUTOR";
                        if (!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                        {
                            tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                            tempTxt = tempTxt.Replace("@InvoiceIdStr", tblInvoiceTO.IdInvoice.ToString());
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);

                            tblAlertInstanceTO.AlertComment = tempTxt;
                        }
                        else
                            tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is accecpted By Distributor.";

                        if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                        {
                            tempTxt = tempTxt.Replace("@DealerNameStr", tblInvoiceTO.DealerName);
                            tblAlertInstanceTO.SmsComment = tempTxt;
                            // tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                        }
                        resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            return resultMessage;
                        }
                    }
                    //Aniket [6-8-2019] commented as REJECTED_BY_DISTRIBUTOR is already written above
                    //else if (tblInvoiceTO.InvoiceStatusE == Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                    //{
                    //    tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_REJECTED_BY_DISTRIBUTOR;
                    //    tblAlertInstanceTO.AlertAction = "INVOICE_REJECTED_BY_DISTRIBUTOR";
                    //    tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " Is Rejected by Distributor.";
                    //    if (dealerNameActive == 1)//Vijaymala added[03-05-2018]
                    //    {
                    //        tblAlertInstanceTO.SmsComment = tblAlertInstanceTO.AlertComment;
                    //        tblAlertInstanceTO.AlertComment += " (" + tblInvoiceTO.DealerName + ").";
                    //    }
                    //}


                    tblAlertInstanceTO.AlertUsersTOList = tblAlertUsersTOList;

                    tblAlertInstanceTO.EffectiveFromDate = tblInvoiceTO.UpdatedOn;
                    tblAlertInstanceTO.EffectiveToDate = tblAlertInstanceTO.EffectiveFromDate.AddHours(10);
                    tblAlertInstanceTO.IsActive = 1;
                    tblAlertInstanceTO.SourceEntityId = tblInvoiceTO.IdInvoice;
                    tblAlertInstanceTO.RaisedBy = tblInvoiceTO.UpdatedBy;
                    tblAlertInstanceTO.RaisedOn = tblInvoiceTO.UpdatedOn;
                    tblAlertInstanceTO.IsAutoReset = 1;

                    ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                    if (rMessage.MessageType != ResultMessageE.Information)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour();
                        resultMessage.Text = "Error While Generating Notification";
                        return resultMessage;
                    }
                }

                exitNotification:

                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateInvoice");
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// GJ@20170926 : Update the Invoice To calculation
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TblInvoiceTO updateInvoiceToCalc(TblInvoiceTO tblInvoiceTo, SqlConnection conn, SqlTransaction tran, Boolean isCheckHist = true)
        {
            tblInvoiceTo.BasicAmt = 0;
            tblInvoiceTo.DiscountAmt = 0;
            tblInvoiceTo.TaxableAmt = 0;
            tblInvoiceTo.IgstAmt = 0;
            tblInvoiceTo.SgstAmt = 0;
            tblInvoiceTo.CgstAmt = 0;
            tblInvoiceTo.GrandTotal = 0;
            int isMathRoundOff = 0;
            string isMathRoundOff«D = "";
            for (int i = 0; i < tblInvoiceTo.InvoiceItemDetailsTOList.Count; i++)
            {
                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = tblInvoiceTo.InvoiceItemDetailsTOList[i];
                TblInvoiceHistoryTO historyInvoiceTo = new TblInvoiceHistoryTO();
                if (isCheckHist)
                {
                    // historyInvoiceTo = TblInvoiceHistoryBL.SelectTblInvoiceHistoryTORateByInvoiceItemId(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);

                    //[24/01/2018]Vijaymala Added :To get previous cd structure and rate
                    historyInvoiceTo = _iTblInvoiceHistoryBL.SelectTblInvoiceHistoryTORateByInvoiceItemId(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);
                    if (historyInvoiceTo != null)
                    {
                        tblInvoiceItemDetailsTO.Rate = historyInvoiceTo.OldUnitRate;
                        //tblInvoiceItemDetailsTO.CdStructureId = historyInvoiceTo.OldCdStructureId;
                        //resultMessage.DefaultBehaviour("Invoice History Rate Object Not Found");
                        //return resultMessage;
                    }
                    historyInvoiceTo = _iTblInvoiceHistoryBL.SelectTblInvoiceHistoryTOCdByInvoiceItemId(tblInvoiceItemDetailsTO.IdInvoiceItem, conn, tran);
                    if (historyInvoiceTo != null)
                    {
                        // tblInvoiceItemDetailsTO.Rate = historyInvoiceTo.OldUnitRate;
                        tblInvoiceItemDetailsTO.CdStructureId = historyInvoiceTo.OldCdStructureId;
                        if (tblInvoiceItemDetailsTO.CdStructureId > 0)
                        {
                            List<DropDownTO> dropDownTOList = _iDimensionBL.SelectCDStructureForDropDown(0);
                            if (dropDownTOList != null && dropDownTOList.Count > 0)
                            {
                                DropDownTO dropDownTO = dropDownTOList.Where(w => w.Value == tblInvoiceItemDetailsTO.CdStructureId).FirstOrDefault();
                                if (dropDownTO != null)
                                {
                                    tblInvoiceItemDetailsTO.CdStructure = Convert.ToDouble(dropDownTO.Text);
                                }
                            }
                        }
                        //resultMessage.DefaultBehaviour("Invoice History Rate Object Not Found");
                        //return resultMessage;
                    }

                }
                else
                {
                    historyInvoiceTo = null;
                }
                if (historyInvoiceTo != null)
                {
                    // Vaibhav [13-Mar-2018] Added to check for CD structure only.
                    if (historyInvoiceTo.OldUnitRate > 0)
                        tblInvoiceItemDetailsTO.Rate = historyInvoiceTo.OldUnitRate;
                    //resultMessage.DefaultBehaviour("Invoice History Rate Object Not Found");
                    //return resultMessage;
                }
                tblInvoiceItemDetailsTO.BasicTotal = tblInvoiceItemDetailsTO.Rate * tblInvoiceItemDetailsTO.InvoiceQty;
                tblInvoiceTo.BasicAmt += tblInvoiceItemDetailsTO.BasicTotal;
                DropDownTO cdDropDownTO = _iDimensionBL.SelectCDDropDown(tblInvoiceItemDetailsTO.CdStructureId);
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_TAX_ON_PRINT_INVOICE);
                if (tblConfigParams != null)
                {
                    if (tblConfigParams.ConfigParamVal == "1")
                    {
                        isMathRoundOff = 1;
                    }
                }
                // Add By Samadhan 18 May 2022
                TblConfigParamsTO tblConfigParamsValue = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_CD_ON_Rate_Calculation_Details);
                if (tblConfigParamsValue != null  )
                {
                    isMathRoundOff«D = tblConfigParamsValue.ConfigParamVal;
                    
                }
                
                //

                if (tblInvoiceItemDetailsTO.CdStructure > 0)
                {
                    Int32 isRsValue = Convert.ToInt32(cdDropDownTO.Text);
                    if (isRsValue == (int)Constants.CdType.IsRs)
                    {

                        tblInvoiceItemDetailsTO.CdAmt = tblInvoiceItemDetailsTO.CdStructure;
                    }
                    else
                    {
                        if (isMathRoundOff == 1)
                        {
                            tblInvoiceItemDetailsTO.CdAmt = (tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                        }
                        else
                        {
                            tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                        }


                    }
                }
                else
                {
                    tblInvoiceItemDetailsTO.CdAmt = 0;
                }


                //Vijaymala[22-06-2018] commented the code to add cd

                //if (tblInvoiceItemDetailsTO.CdStructure > 0)
                //{
                //    tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                //}
                //else
                //{
                //    tblInvoiceItemDetailsTO.CdAmt = 0;
                //}

                if (isMathRoundOff«D != "")
                {                   
                    tblInvoiceTo.DiscountAmt += Math.Round(tblInvoiceItemDetailsTO.CdAmt, Convert.ToInt32(isMathRoundOff«D));
                }
                else
                {
                    tblInvoiceTo.DiscountAmt += tblInvoiceItemDetailsTO.CdAmt;
                }
               
                tblInvoiceItemDetailsTO.TaxableAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;
                tblInvoiceTo.TaxableAmt += tblInvoiceItemDetailsTO.TaxableAmt;

                tblInvoiceItemDetailsTO.GrandTotal = tblInvoiceItemDetailsTO.TaxableAmt;

                foreach (var itemTaxDet in tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList)
                {
                    itemTaxDet.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                    itemTaxDet.TaxAmt = (itemTaxDet.TaxableAmt * itemTaxDet.TaxRatePct) / 100;
                    tblInvoiceItemDetailsTO.GrandTotal += itemTaxDet.TaxAmt;
                }
                tblInvoiceTo.GrandTotal += tblInvoiceItemDetailsTO.GrandTotal;
                if (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count == 1)
                {
                    tblInvoiceTo.IgstAmt += tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[0].TaxAmt;
                }
                else
                {
                    tblInvoiceTo.CgstAmt += tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[0].TaxAmt;
                    tblInvoiceTo.SgstAmt += tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 1 ?
                        tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[1].TaxAmt : 0;
                }
            }


            Double grandTotal = tblInvoiceTo.GrandTotal;
            Double taxableAmt = tblInvoiceTo.TaxableAmt;
            Double basicTotalAmt = tblInvoiceTo.BasicAmt;
            Boolean isPanPresent = false;
            Double otherTaxAmt = 0;
            ResultMessage message = new ResultMessage();
            if (tblInvoiceTo.IsConfirmed == 1)
            {
                Int32 BillingOrgId = tblInvoiceTo.DealerOrgId;
                if (tblInvoiceTo.InvoiceAddressTOList != null && tblInvoiceTo.InvoiceAddressTOList.Count > 0)
                {
                    tblInvoiceTo.InvoiceAddressTOList.ForEach(element =>
                    {
                        if (element.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                        {
                            isPanPresent = IsPanOrGstPresent(element.PanNo, element.GstinNo);
                            if (element.BillingOrgId > 0)
                            {
                                BillingOrgId = element.BillingOrgId;
                            }
                        }
                    });
                }

                TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(BillingOrgId);
                if (tblOrganizationTO != null && tblOrganizationTO.IsTcsApplicable == 1)
                {
                    tblInvoiceTo.IsTcsApplicable = tblOrganizationTO.IsTcsApplicable;
                    message = AddTcsTOInTaxItemDtls(conn, tran, ref grandTotal, ref taxableAmt, ref basicTotalAmt, isPanPresent, tblInvoiceTo.InvoiceItemDetailsTOList, tblInvoiceTo, ref otherTaxAmt, tblOrganizationTO.IsDeclarationRec);
                }
                tblInvoiceTo.GrandTotal = grandTotal;
                tblInvoiceTo.TaxableAmt = taxableAmt;
                tblInvoiceTo.BasicAmt = basicTotalAmt;

            }
            return tblInvoiceTo;

        }

        private ResultMessage InvoiceStatusUpdate(TblInvoiceTO tblInvoiceTO, Int32 statusId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                //Update Status
                tblInvoiceTO.StatusId = statusId;
                int result = UpdateTblInvoice(tblInvoiceTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error in UpdateTblInvoice InvoiceTbl");
                    return resultMessage;
                }

                //Generate inv history record
                TblInvoiceHistoryTO invHistoryTO = new TblInvoiceHistoryTO();
                invHistoryTO.InvoiceId = tblInvoiceTO.IdInvoice;
                invHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                invHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                invHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                invHistoryTO.BookingCommentCategoryId = tblInvoiceTO.BookingCommentCategoryId;
                invHistoryTO.BookingTaxCategoryId = tblInvoiceTO.BookingTaxCategoryId;
                invHistoryTO.StatusId = statusId;
                if (statusId == (int)Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is pending for authorization";
                else if (statusId == (int)Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is pending for Acceptance";
                if (statusId == (int)Constants.InvoiceStatusE.REJECTED_BY_DIRECTOR)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is Rejected By Director";
                if (statusId == (int)Constants.InvoiceStatusE.REJECTED_BY_DISTRIBUTOR)
                    invHistoryTO.StatusRemark = "Invoice #" + tblInvoiceTO.IdInvoice + " is Rejected By Distributer";
                result = _iTblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error While InsertTblInvoiceHistory"); return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InvoiceStatusUpdate");
                return resultMessage;
            }
        }

        private ResultMessage CheckAndUpdateForInvoiceAcceptanceStatus(TblInvoiceTO existingInvoiceTO, TblInvoiceTO tblInvoiceTO, TblAlertInstanceTO tblAlertInstanceTO, List<TblAlertUsersTO> tblAlertUsersTOList, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                //Boolean isSentForAcceptance = false;
                //if (!string.IsNullOrEmpty(existingInvoiceTO.ChangeIn))
                //{
                //    string[] changedAttrs = existingInvoiceTO.ChangeIn.Split('|');
                //    if (changedAttrs.Length > 0)
                //    {
                //        for (int k = 0; k < changedAttrs.Length; k++)
                //        {
                //            if (changedAttrs[k] == "ADDRESS" || changedAttrs[k] == "CD")
                //            {
                //                isSentForAcceptance = true;
                //                break;
                //            }
                //        }
                //    }
                //}

                //if (isSentForAcceptance)
                //{
                List<TblUserTO> cnfUserList = _iTblUserDAO.SelectAllTblUser(tblInvoiceTO.DistributorOrgId, conn, tran);
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

                tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE;
                tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.INVOICE_ACCEPTANCE_REQUIRED;
                tblAlertInstanceTO.AlertAction = "INVOICE_ACCEPTANCE_REQUIRED";
                tblAlertInstanceTO.AlertComment = "Invoice #" + tblInvoiceTO.IdInvoice + " is awaiting for acceptance";

                resultMessage = InvoiceStatusUpdate(tblInvoiceTO, tblInvoiceTO.StatusId, conn, tran);
                if (resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }
                //}

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InvoiceAccetanceStatus");
                return resultMessage;
            }
        }


        public ResultMessage exchangeInvoice(Int32 invoiceId, Int32 invGenModeId, int fromOrgId, int toOrgId, int isCalculateWithBaseRate)
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

                TblInvoiceTO invoiceTO = _iTblInvoiceDAO.SelectTblInvoice(invoiceId, conn, tran);

                if (invoiceTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("invoiceTO Found NULL"); return resultMessage;
                }

                if (invoiceTO.InvFromOrgFreeze == 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.Text = "Invoie Already Converted";
                    return resultMessage;
                }

                //Vijaymala[23-03-2016]added to check invoice details of igst,cgst,sgst taxes
                #region To check invoice details is valid or not
                string errorMsg = string.Empty;
                Boolean isValidInvoice = CheckInvoiceDetailsAccToState(invoiceTO, ref errorMsg);
                if (!isValidInvoice)
                {
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                    return resultMessage;
                }
                #endregion

                TblInvoiceChangeOrgHistoryTO invHistTO = new TblInvoiceChangeOrgHistoryTO();
                invHistTO.InvoiceId = invoiceTO.IdInvoice;
                invHistTO.CreatedOn = _iCommon.ServerDateTime;
                invHistTO.CreatedBy = 1;
                if (invGenModeId == (int)Constants.InvoiceGenerateModeE.DUPLICATE)
                {
                    invHistTO.ActionDesc = "Duplicate";
                }
                if (invGenModeId == (int)Constants.InvoiceGenerateModeE.CHANGEFROM)
                {
                    invHistTO.ActionDesc = "Change Organization";
                }
                resultMessage = PrepareAndSaveInternalTaxInvoices(invoiceTO, invGenModeId, fromOrgId, toOrgId, isCalculateWithBaseRate, invHistTO, conn, tran);
                int res = _iTblInvoiceChangeOrgHistoryDAO.InsertTblInvoiceChangeOrgHistory(invHistTO, conn, tran);

                if (res != 1)
                {
                    tran.Rollback();
                    resultMessage.Text = "failed to save History in exchangeInvoice";
                    return resultMessage;
                }
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

                TblInvoiceTO invoiceTO = _iTblInvoiceDAO.SelectTblInvoice(invoiceId, conn, tran);
                if (invoiceTO == null)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("invoiceTO Found NULL"); return resultMessage;
                }
                invoiceTO.InvComment = invComment;
                //Vijaymala[23-03-2016]added to check invoice details of igst,cgst,sgst taxes
                #region To check invoice details is valid or not
                string errorMsg = string.Empty;
                Boolean isValidInvoice = CheckInvoiceDetailsAccToState(invoiceTO, ref errorMsg);
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
                        resultMessage = PrepareAndSaveInternalTaxInvoices(invoiceTO, invGenModeId, fromOrgId, toOrgId, 0, changeHisTO, conn, tran);
                        TblConfigParamsTO tblConfigParamsTOVasudha = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_NOTIFICATIONS);

                        if (tblConfigParamsTOVasudha != null && !String.IsNullOrEmpty(tblConfigParamsTOVasudha.ConfigParamVal))
                        {
                            Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOVasudha.ConfigParamVal);
                            if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                            {
                                TblConfigParamsTO MessageTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_SEND_SMS_AS_PER_VASUDHA");
                                if (MessageTO != null && !String.IsNullOrEmpty(MessageTO.ConfigParamVal))
                                {
                                    string AlertComment = "";
                                    TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                                    if (invoiceTO != null )
                                    {
                                        TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(invoiceTO.DealerOrgId);
                                        if (tblOrganizationTO == null)
                                        {
                                            resultMessage.DefaultBehaviour("tblOrganizationTO is null in send Vasudha SMS Msg");
                                            return resultMessage;
                                        }
                                        TblConfigParamsTO MessageTOV2 = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("CP_DELIVER_SEND_VEHICLE_OUT_MSG_FOR_VASUDHA");
                                        if (MessageTOV2 != null && !String.IsNullOrEmpty(MessageTOV2.ConfigParamVal))
                                        {
                                            AlertComment = MessageTOV2.ConfigParamVal;
                                          
                                            TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipDAO.SelectTblLoadingSlip(invoiceTO.LoadingSlipId,conn,tran);
                                            if(tblLoadingSlipTO !=null)
                                                AlertComment = AlertComment.Replace("@mobileNo", tblLoadingSlipTO.ContactNo);
                                            else
                                                AlertComment = AlertComment.Replace("@mobileNo","--");
                                        }
                                        List<TblAlertUsersTO> tblAlertUsersTOList = new List<TblAlertUsersTO>();
                                        TblAlertUsersTO tblAlertUsersTO = new TblAlertUsersTO();
                                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.VEHICLE_OUT_FOR_DELIVERY;
                                        tblAlertInstanceTO.AlertAction = "Vehicle OUT";
                                        tblAlertInstanceTO.AlertComment = AlertComment;
                                        TblSmsTO smsTO = new TblSmsTO();
                                        tblAlertInstanceTO.SmsTOList = new List<TblSmsTO>();

                                        smsTO.MobileNo = tblOrganizationTO.RegisteredMobileNos;
                                        smsTO.SourceTxnDesc = AlertComment;
                                        string confirmMsg = string.Empty;

                                        smsTO.SmsTxt = AlertComment;
                                        tblAlertInstanceTO.SmsTOList.Add(smsTO);


                                        ResultMessage rMessage = _iTblAlertInstanceBL.SaveNewAlertInstance(tblAlertInstanceTO, conn, tran);
                                        if (rMessage.MessageType != ResultMessageE.Information)
                                        {
                                            tran.Rollback();
                                            resultMessage.DefaultBehaviour();
                                            resultMessage.Text = "Error While Generating Notification For Send SMS in Vasudha";
                                            // return resultMessage;
                                        }
                                    }
                                }
                            }

                        }
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
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
            return resultMessage;
        }


        //Aniket [22-4-2019]
        public List<TblInvoiceAddressTO> SelectTblInvoiceAddressByDealerId(Int32 dealerOrgId, String txnAddrTypeId)
        {
            Int32 topRecordcnt = 0;
            String txnAddrTypeIdtemp = String.Empty;

            if (!String.IsNullOrEmpty(txnAddrTypeId))
            {
                txnAddrTypeIdtemp = txnAddrTypeId;
            }

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING);
            if (tblConfigParamsTO != null)
            {
                topRecordcnt = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }

            //List<TblInvoiceAddressTO> tblInvoiceDelAddrTOList = new List<TblInvoiceAddressTO>();
            try
            {
                //List<TblInvoiceAddressTO> tblInvoiceDelAddrTOListtemp = new List<TblInvoiceAddressTO>();
                List<TblInvoiceAddressTO> tblInvoiceDelAddrTOList = _iTblInvoiceAddressDAO.SelectTblInvoiceAddressByDealerId(dealerOrgId, txnAddrTypeIdtemp, topRecordcnt);
                //tblBookingDelAddrTOList = _iTblBookingDelAddrDAO.SelectTblBookingsByDealerOrgId(dealerOrgId, txnAddrTypeIdtemp);
                //tblBookingDelAddrTOList = tblBookingDelAddrTOList.Where(ele => ele.BillingName != null || ele.Address != null).ToList();
                //tblBookingDelAddrTOList = tblBookingDelAddrTOList.GroupBy(c => new { c.BillingName, c.Address }).Select(s => s.FirstOrDefault()).ToList();
                //if (cnt > tblInvoiceDelAddrTOList.Count)
                //{
                //    tblInvoiceDelAddrTOListtemp = tblInvoiceDelAddrTOList;
                //}
                //else
                //{
                //    for (int i = 0; i < cnt; i++)
                //    {
                //        TblInvoiceAddressTO tblInvoiceDelAddrTO = tblInvoiceDelAddrTOList[i];
                //        tblInvoiceDelAddrTOListtemp.Add(tblInvoiceDelAddrTO);
                //    }
                //}
                return tblInvoiceDelAddrTOList;
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public ResultMessage UpdateInvoiceNonCommercialDetails(TblInvoiceTO tblInvoiceTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                TblInvoiceTO exiInvoiceTO = _iTblInvoiceDAO.SelectTblInvoice(tblInvoiceTO.IdInvoice, conn, tran);
                if (exiInvoiceTO == null)
                {
                    tran.Rollback();
                    resultMSg.DefaultBehaviour("exiInvoiceTO Found NULL");
                    return resultMSg;
                }
                // Vaibhav [25-April-2018] Added to update from finaltables after data extarction.
                if (tblInvoiceTO.TranTableType == (int)Constants.TranTableType.TEMP)
                {
                    result = _iTblInvoiceDAO.UpdateInvoiceNonCommercDtls(tblInvoiceTO, conn, tran);
                }
                else
                {
                    result = _iTblInvoiceDAO.UpdateInvoiceNonCommercDtlsForFinal(tblInvoiceTO, conn, tran);
                }

                if (result != 1)
                {
                    tran.Rollback();
                    resultMSg.DefaultBehaviour("Error While UpdateInvoiceNonCommercDtls");
                    return resultMSg;
                }

                //Generate inv history record
                TblInvoiceHistoryTO invHistoryTO = new TblInvoiceHistoryTO();
                invHistoryTO.InvoiceId = tblInvoiceTO.IdInvoice;
                invHistoryTO.CreatedOn = tblInvoiceTO.UpdatedOn;
                invHistoryTO.CreatedBy = tblInvoiceTO.UpdatedBy;
                invHistoryTO.StatusDate = tblInvoiceTO.UpdatedOn;
                invHistoryTO.StatusId = exiInvoiceTO.StatusId;
                invHistoryTO.OldEwayBillNo = exiInvoiceTO.ElectronicRefNo;
                invHistoryTO.NewEwayBillNo = tblInvoiceTO.ElectronicRefNo;
                invHistoryTO.StatusRemark = "Non-Commercial Details Update";

                if (tblInvoiceTO.TranTableType == (int)Constants.TranTableType.TEMP)
                {
                    result = _iTblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
                }
                else
                {
                    result = _iTblInvoiceHistoryBL.InsertTblInvoiceHistoryForFinal(invHistoryTO, conn, tran);
                }

                if (result != 1)
                {
                    tran.Rollback();
                    resultMSg.DefaultBehaviour("Error While InsertTblInvoiceHistory"); return resultMSg;
                }

                tran.Commit();
                resultMSg.DefaultSuccessBehaviour();
                return resultMSg;
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// GJ@20171001 : update the Invoice ConfirmNonConfirm Status with Calculation
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        /// 
        //public ResultMessage UpdateInvoiceConfrimNonConfirmDetails(TblInvoiceTO tblInvoiceTO, Int32 loginUserId)
        //{
        //    SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction tran = null;
        //    ResultMessage resultMessage = new ResultMessage();
        //    Double totalInvQty = 0;
        //    Double totalNCExpAmt = 0;
        //    Double totalNCOtherAmt = 0;
        //    double conversionFactor = 0.001;
        //    try
        //    {
        //        conn.Open();
        //        tran = conn.BeginTransaction();

        //        TblInvoiceTO exiInvoiceTO = SelectTblInvoiceTOWithDetails(tblInvoiceTO.IdInvoice, conn, tran);
        //        if (exiInvoiceTO == null)
        //        {
        //            tran.Rollback();
        //            resultMessage.DefaultBehaviour("exiInvoiceTO Found NULL");
        //            return resultMessage;
        //        }
        //        exiInvoiceTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
        //        exiInvoiceTO.UpdatedBy = tblInvoiceTO.UpdatedBy;
        //        exiInvoiceTO.UpdatedOn = tblInvoiceTO.UpdatedOn;

        //        //Call to get the Loading Slip detail againest Loading Slip
        //        TblLoadingSlipDtlTO tblLoadingSlipDtlTO = new TblLoadingSlipDtlTO();
        //        TblLoadingSlipTO loadingSlipTO = new TblLoadingSlipTO();
        //        if (tblInvoiceTO.LoadingSlipId != 0)
        //        {
        //            loadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblInvoiceTO.LoadingSlipId, conn, tran);
        //            if (loadingSlipTO == null)
        //            {
        //                resultMessage.DefaultBehaviour("loadingSlipTO Found NULL");
        //                return resultMessage;
        //            }
        //            //loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
        //            tblLoadingSlipDtlTO = _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(tblInvoiceTO.LoadingSlipId, conn, tran);
        //            //if (tblLoadingSlipDtlTO == null)
        //            //{
        //            //    tran.Rollback();
        //            //    resultMessage.MessageType = ResultMessageE.Error;
        //            //    resultMessage.Text = "Error :tblLoadingSlipDtlTO Found NUll Or Empty";
        //            //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
        //            //    return resultMessage;
        //            //}
        //        }
        //        if (tblInvoiceTO.LoadingSlipId == 0 || tblLoadingSlipDtlTO == null)
        //        {
        //            int result = 0;
        //            result = _iTblInvoiceDAO.UpdateTblInvoice(tblInvoiceTO, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMessage.DefaultBehaviour("Error While UpdateInvoiceConfrimNonConfirmDetails");
        //                return resultMessage;
        //            }
        //            if (tblInvoiceTO.LoadingSlipId != 0)
        //            {
        //             loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;

        //            //    resultMessage = _iTblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(loadingSlipTO, tblInvoiceTO.UpdatedBy, conn, tran);
        //            //    if (resultMessage.MessageType != ResultMessageE.Information)
        //            //    {
        //            //        tran.Rollback();
        //            //        resultMessage.DefaultBehaviour("Error While ChangeLoadingSlipConfirmationStatus");
        //            //        return resultMessage;
        //            //    }
        //            }


        //            tran.Commit();

        //            resultMessage.DefaultSuccessBehaviour();
        //            if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.StatusId == Convert.ToInt32(Constants.InvoiceStatusE.AUTHORIZED))
        //            {
        //                Int32 isconfirm = 0;
        //                GenerateInvoiceNumber(tblInvoiceTO.IdInvoice, loginUserId, isconfirm, (int)Constants.InvoiceGenerateModeE.REGULAR);

        //            }
        //            return resultMessage;
        //        }

        //        //Call to get the TblBooking for Parity Id
        //        TblBookingsTO tblBookingsTO = new Models.TblBookingsTO();
        //        tblBookingsTO = _iTblBookingsBL.SelectTblBookingsTO(tblLoadingSlipDtlTO.BookingId, conn, tran);
        //        if (tblBookingsTO == null)
        //        {
        //            tran.Rollback();
        //            resultMessage.MessageType = ResultMessageE.Error;
        //            resultMessage.Text = "Error :tblBookingsTO Found NUll Or Empty";
        //            resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
        //            return resultMessage;
        //        }
        //        if (exiInvoiceTO.InvoiceItemDetailsTOList != null && exiInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
        //        {
        //            List<TblParityDetailsTO> parityDetailsTOList = null;
        //            //if (tblBookingsTO.ParityId > 0)
        //            //    parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(tblBookingsTO.ParityId, 0, conn, tran);


        //            String parityIds = String.Empty;
        //            List<TblBookingParitiesTO> tblBookingParitiesTOList = _iTblBookingParitiesDAO.SelectTblBookingParitiesByBookingId(tblBookingsTO.IdBooking, conn, tran);
        //            if (tblBookingParitiesTOList != null && tblBookingParitiesTOList.Count > 0)
        //            {
        //                parityIds = String.Join(",", tblBookingParitiesTOList.Select(s => s.ParityId.ToString()).ToArray());
        //            }

        //            if (String.IsNullOrEmpty(parityIds))
        //            {
        //                tran.Rollback();
        //                resultMessage.DefaultBehaviour();
        //                resultMessage.Text = "Error : ParityTO Not Found";
        //                resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
        //                return resultMessage;
        //            }

        //            //Sudhir[30-APR-2018] Commented For New Parity Logic.
        //            //parityDetailsTOList = BL.TblParityDetailsBL.SelectAllTblParityDetailsList(parityIds, 0, conn, tran);


        //            for (int e = 0; e < exiInvoiceTO.InvoiceItemDetailsTOList.Count; e++)
        //            {
        //                TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = exiInvoiceTO.InvoiceItemDetailsTOList[e];
        //                if (tblInvoiceItemDetailsTO.OtherTaxId == 0)
        //                {

        //                    //TblLoadingSlipExtTO tblLoadingSlipExtTO = _iTblLoadingSlipExtBL.SelectTblLoadingSlipExtTO(tblInvoiceItemDetailsTO.LoadingSlipExtId, conn, tran);

        //                    //if (tblLoadingSlipExtTO != null && tblLoadingSlipExtTO.LoadingQty > 0)
        //                    //{
        //                    //    #region Calculate Actual Price From Booking and Parity Settings

        //                    //    Double orcAmtPerTon = 0;
        //                    //    if (loadingSlipTO.OrcMeasure == "Rs/MT")
        //                    //    {
        //                    //        //orcAmtPerTon = tblBookingsTO.OrcAmt; Sudhir[30-APR-2018] ORC From Loading Slip Instead of Booking.
        //                    //        orcAmtPerTon = loadingSlipTO.OrcAmt;
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        //if (tblBookingsTO.OrcAmt > 0)
        //                    //        //    orcAmtPerTon = tblBookingsTO.OrcAmt / tblBookingsTO.BookingQty;
        //                    //        if (loadingSlipTO.OrcAmt > 0)
        //                    //            orcAmtPerTon = loadingSlipTO.OrcAmt / tblLoadingSlipDtlTO.LoadingQty;
        //                    //    }

        //                    //    Double bookingPrice = tblBookingsTO.BookingRate;
        //                    //    Double parityAmt = 0;
        //                    //    Double priceSetOff = 0;
        //                    //    Double paritySettingAmt = 0;
        //                    //    Double bvcAmt = 0;
        //                    //    // TblParitySummaryTO parityTO = null; Sudhir[30-APR-2018] Commented for New Parity Logic.
        //                    //    TblParityDetailsTO parityDtlTO = null;
        //                    //    if (true)
        //                    //    {
        //                    //        //Sudhir[30-APR-2018] Commented for New Parity Logic.
        //                    //        /*var parityDtlTO = parityDetailsTOList.Where(m => m.MaterialId == tblLoadingSlipExtTO.MaterialId
        //                    //                                        && m.ProdCatId == tblLoadingSlipExtTO.ProdCatId
        //                    //                                        && m.ProdSpecId == tblLoadingSlipExtTO.ProdSpecId 
        //                    //                                        && m.BrandId == tblLoadingSlipExtTO.BrandId).FirstOrDefault();*/

        //                    //        //Get Latest To Based On -materialId, Date And Time Check Condition Actual TIme < = First Object.
        //                    //        TblAddressTO addrTO =_iTblAddressBL.SelectOrgAddressWrtAddrType(tblBookingsTO.DealerOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);

        //                    //        //SUdhir[30-APR-2018] Added for the Get Parity Details List based on Material Id,ProdCat Id,ProdSpec Id ,State Id ,Brand Id and Booking Date.
        //                    //        parityDtlTO = BL.TblParityDetailsBL.SelectParityDetailToListOnBooking(tblLoadingSlipExtTO.MaterialId, tblLoadingSlipExtTO.ProdCatId, tblLoadingSlipExtTO.ProdSpecId, tblLoadingSlipExtTO.ProdItemId, tblLoadingSlipExtTO.BrandId, addrTO.StateId, tblBookingsTO.BookingDatetime);

        //                    //        if (parityDtlTO != null)
        //                    //        {
        //                    //            parityAmt = parityDtlTO.ParityAmt;
        //                    //            if (tblInvoiceTO.IsConfirmed != 1)
        //                    //                priceSetOff = parityDtlTO.NonConfParityAmt;
        //                    //            else
        //                    //                priceSetOff = 0;

        //                    //        }
        //                    //        else
        //                    //        {
        //                    //            tran.Rollback();
        //                    //            resultMessage.DefaultBehaviour();
        //                    //            resultMessage.Text = "Error : ParityTO Not Found";
        //                    //            string mateDesc = tblLoadingSlipExtTO.MaterialDesc + " " + tblLoadingSlipExtTO.ProdCatDesc + "-" + tblLoadingSlipExtTO.ProdSpecDesc;
        //                    //            resultMessage.DisplayMessage = "Warning : Parity Details Not Found For " + mateDesc + " Please contact BackOffice";
        //                    //            return resultMessage;
        //                    //        }

        //                    //        #region Sudhir[30-APR-2018] Commented for New Parity Logic
        //                    //        //parityTO = _iTblParitySummaryBL.SelectTblParitySummaryTO(parityDtlTO.ParityId, conn, tran);
        //                    //        //if (parityTO == null)
        //                    //        //{
        //                    //        //    tran.Rollback();
        //                    //        //    resultMessage.DefaultBehaviour();
        //                    //        //    resultMessage.Text = "Error : ParityTO Not Found";
        //                    //        //    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
        //                    //        //    return resultMessage;
        //                    //        //}
        //                    //        //paritySettingAmt = parityTO.BaseValCorAmt + parityTO.ExpenseAmt + parityTO.OtherAmt;
        //                    //        //bvcAmt = parityTO.BaseValCorAmt;
        //                    //        #endregion

        //                    //        //[30-APR-2018] Added For New Parity Setting Logic
        //                    //        paritySettingAmt = parityDtlTO.BaseValCorAmt + parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
        //                    //        bvcAmt = parityDtlTO.BaseValCorAmt;
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        tran.Rollback();
        //                    //        resultMessage.DefaultBehaviour();
        //                    //        resultMessage.Text = "Error : ParityTO Not Found";
        //                    //        resultMessage.DisplayMessage = "Warning : Parity Details Not Found, Please contact BackOffice";
        //                    //        return resultMessage;
        //                    //    }
        //                    //    Double cdApplicableAmt = (bookingPrice + orcAmtPerTon + parityAmt + priceSetOff + bvcAmt);
        //                    //    //if (tblInvoiceTO.IsConfirmed == 1)
        //                    //        // cdApplicableAmt += parityTO.ExpenseAmt + parityTO.OtherAmt; Sudhir[30-APR-2018] Commented.
        //                    //   if (tblInvoiceTO.IsConfirmed == 1)
        //                    //         cdApplicableAmt += parityDtlTO.ExpenseAmt + parityDtlTO.OtherAmt;
        //                    //    tblInvoiceItemDetailsTO.Rate = cdApplicableAmt;

        //                    //    #endregion
        //                    //}
        //                    //else
        //                    //{
        //                    //    tran.Rollback();
        //                    //    resultMessage.DefaultBehaviour("Error : tblLoadingSlipExtTO Found Null Or Empty");
        //                    //    return resultMessage;
        //                    //}
        //                }
        //            }
        //        }
        //        else
        //        {
        //            tran.Rollback();
        //            resultMessage.DefaultBehaviour();
        //            resultMessage.Text = "Error : InvoiceItemDetailsTOList(Invoice Item Details) Found Null Or Empty";
        //            resultMessage.DisplayMessage = "Error 01 : No Items found to change the Status.";
        //            return resultMessage;
        //        }
        //        TblLoadingTO tblLoadingTONew = new TblLoadingTO();
        //        tblLoadingTONew = _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId(loadingSlipTO.IdLoadingSlip, conn, tran);
        //        if (tblLoadingTONew == null)
        //        {
        //            resultMessage.DefaultBehaviour("tblLoadingTONew  found NULL");
        //            return resultMessage;
        //        }

        //        //Slip To

        //        //loadingSlipTO.LoadingSlipExtTOList;
        //        //loadingSlipTO.TblLoadingSlipDtlTO;
        //        loadingSlipTO.TblLoadingSlipDtlTO = tblLoadingSlipDtlTO;
        //        loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;
        //        tblLoadingTONew.LoadingSlipList = new List<TblLoadingSlipTO>();

        //        tblLoadingTONew.LoadingSlipList.Add(loadingSlipTO);

        //        resultMessage = _iTblLoadingBL.CalculateLoadingValuesRate(tblLoadingTONew);
        //        if (resultMessage.MessageType != ResultMessageE.Information)
        //        {
        //            tran.Rollback();
        //            resultMessage.DefaultBehaviour("Error While CalculateLoadingValuesRate");
        //            return resultMessage;
        //        }

        //        Int32 lastConfirmationStatus = tblLoadingTONew.LoadingSlipList[0].IsConfirmed;
        //        if (lastConfirmationStatus == 1)
        //            tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 0;
        //        else
        //            tblLoadingTONew.LoadingSlipList[0].IsConfirmed = 1;

        //        // tblLoadingTONew.LoadingSlipList[0].IsConfirmed= exiInvoiceTO.IsConfirmed;
        //        resultMessage = _iTblLoadingSlipBL.ChangeLoadingSlipConfirmationStatus(tblLoadingTONew.LoadingSlipList[0], tblInvoiceTO.UpdatedBy, conn, tran);
        //        // resultMessage = _iTblLoadingSlipBL.OldChangeLoadingSlipConfirmationStatus(loadingSlipTO, tblInvoiceTO.UpdatedBy, conn, tran);
        //        if (resultMessage.MessageType != ResultMessageE.Information)
        //        {
        //            tran.Rollback();
        //            resultMessage.DefaultBehaviour("Error While ChangeLoadingSlipConfirmationStatus");
        //            return resultMessage;
        //        }

        //        //Priyanka [25-07-2018] : Added

        //        TblLoadingTO tblLoadingTO = new TblLoadingTO();
        //        tblLoadingTO = _iTblLoadingDAO.SelectTblLoadingByLoadingSlipId(loadingSlipTO.IdLoadingSlip, conn, tran);
        //        if (tblLoadingTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("Address Not Found For Self Organization.");
        //            return resultMessage;
        //        }
        //        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
        //        if (tblConfigParamsTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
        //            return resultMessage;
        //        }
        //        Int32 internalOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
        //        TblAddressTO ofcAddrTO =_iTblAddressBL.SelectOrgAddressWrtAddrType(internalOrgId, Constants.AddressTypeE.OFFICE_ADDRESS, conn, tran);
        //        if (ofcAddrTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("Address Not Found For Self Organization.");
        //            return resultMessage;
        //        }
        //        /*GJ@20170927 : For get RCM and pass to Invoice*/
        //        TblConfigParamsTO rcmConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_REVERSE_CHARGE_MECHANISM, conn, tran);
        //        if (rcmConfigParamsTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("RCM value Not Found in Configuration.");
        //            return resultMessage;
        //        }
        //        TblConfigParamsTO invoiceDateConfigTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_TARE_WEIGHT_DATE_AS_INV_DATE, conn, tran);
        //        if (invoiceDateConfigTO == null || invoiceDateConfigTO.ConfigParamVal == "0")
        //        {
        //            tblInvoiceTO.InvoiceDate = _iCommon.ServerDateTime;
        //        }
        //       loadingSlipTO.IsConfirmed = tblInvoiceTO.IsConfirmed;

        //        Int32 billingStateId = 0;
        //        TblInvoiceTO calculatedInvoiceTO = PrepareInvoiceAgainstLoadingSlip(tblLoadingTO, conn, tran, internalOrgId, ofcAddrTO,rcmConfigParamsTO, invoiceDateConfigTO, loadingSlipTO);



        //        if (calculatedInvoiceTO == null)
        //        {
        //            resultMessage.DefaultBehaviour("calculatedInvoiceTO  found NULL");
        //            return resultMessage;
        //        }

        //        Int32 invoiceId = exiInvoiceTO.IdInvoice;
        //        //// calculatedInvoiceTO = exiInvoiceTO;
        //        // calculatedInvoiceTO.IdInvoice = invoiceId;
        //        // calculatedInvoiceTO.InvoiceAddressTOList.ForEach(f => f.InvoiceId = invoiceId);
        //        // //calculatedInvoiceTO.InvoiceItemDetailsTOList.ForEach(f => f.InvoiceId = invoiceId);
        //        // calculatedInvoiceTO.TempLoadingSlipInvoiceTOList.ForEach(f => f.InvoiceId = invoiceId);
        //        //// calculatedInvoiceTO.InvoiceDocumentDetailsTOList.ForEach(f => f.InvoiceId = invoiceId);

        //        // calculatedInvoiceTO.InvoiceAddressTOList = exiInvoiceTO.InvoiceAddressTOList;
        //        // calculatedInvoiceTO.InvoiceDocumentDetailsTOList = exiInvoiceTO.InvoiceDocumentDetailsTOList;
        //        // calculatedInvoiceTO.InvoiceItemDetailsTOList = exiInvoiceTO.InvoiceItemDetailsTOList;

        //        #region 5 Save main Invoice
        //        exiInvoiceTO.TaxableAmt = calculatedInvoiceTO.TaxableAmt;
        //        exiInvoiceTO.DiscountAmt = calculatedInvoiceTO.DiscountAmt;
        //        exiInvoiceTO.IgstAmt = calculatedInvoiceTO.IgstAmt;
        //        exiInvoiceTO.CgstAmt = calculatedInvoiceTO.CgstAmt;
        //        exiInvoiceTO.SgstAmt = calculatedInvoiceTO.SgstAmt;

        //        exiInvoiceTO.IsConfirmed = calculatedInvoiceTO.IsConfirmed;
        //        exiInvoiceTO.GrandTotal = calculatedInvoiceTO.GrandTotal;
        //        exiInvoiceTO.RoundOffAmt = calculatedInvoiceTO.RoundOffAmt;
        //        exiInvoiceTO.BasicAmt = calculatedInvoiceTO.BasicAmt;
        //        calculatedInvoiceTO.InvoiceItemDetailsTOList.ForEach(ele => ele.InvoiceId = invoiceId);

        //        exiInvoiceTO.InvoiceItemDetailsTOList = calculatedInvoiceTO.InvoiceItemDetailsTOList;

        //        #endregion


        //        //exiInvoiceTO = updateInvoiceToCalc(exiInvoiceTO, conn, tran, false);
        //        if (tblInvoiceTO.IsConfirmed == 0)
        //        {
        //            //for (int i = 0; i < loadingSlipTO.LoadingSlipExtTOList.Count; i++)
        //            //{
        //            //    TblLoadingSlipExtTO tblLoadingSlipExt = loadingSlipTO.LoadingSlipExtTOList[i];
        //            //    //TblParitySummaryTO parityTO = _iTblParitySummaryBL.SelectParitySummaryTOFromParityDtlId(tblLoadingSlipExt.ParityDtlId, conn, tran);
        //            //    TblParityDetailsTO parityTO = new TblParityDetailsTO();
        //            //    if (tblLoadingSlipExt.ParityDtlId > 0)
        //            //        parityTO = BL.TblParityDetailsBL.SelectTblParityDetailsTO(tblLoadingSlipExt.ParityDtlId, conn, tran);
        //            //    if (parityTO != null)
        //            //    {
        //            //        totalNCExpAmt += parityTO.ExpenseAmt * Math.Round(tblLoadingSlipExt.LoadedWeight * conversionFactor, 2);
        //            //        totalNCOtherAmt += parityTO.OtherAmt * Math.Round(tblLoadingSlipExt.LoadedWeight * conversionFactor, 2);
        //            //    }
        //            //}
        //            //exiInvoiceTO.ExpenseAmt = totalNCExpAmt;
        //            //exiInvoiceTO.OtherAmt = totalNCOtherAmt;
        //            //exiInvoiceTO.GrandTotal += totalNCExpAmt + totalNCOtherAmt;

        //        }
        //        resultMessage = UpdateInvoice(exiInvoiceTO, conn, tran);
        //        if (resultMessage.MessageType != ResultMessageE.Information)
        //        {
        //            tran.Rollback();
        //            resultMessage.DefaultBehaviour("Error While UpdateInvoiceConfrimNonConfirmDetails");
        //            return resultMessage;
        //        }
        //        //Update the Loading Slip To Details
        //        if (tblInvoiceTO.IsConfirmed == 0)
        //        {
        //            loadingSlipTO.IsConfirmed = 1;
        //        }
        //        else
        //        {
        //            loadingSlipTO.IsConfirmed = 0;
        //        }


        //        tran.Commit();
        //        resultMessage.DefaultSuccessBehaviour();

        //        if (tblInvoiceTO.IsConfirmed == 1 && tblInvoiceTO.StatusId == Convert.ToInt32(Constants.InvoiceStatusE.AUTHORIZED))
        //        {
        //            Int32 isconfirm = 0;
        //            GenerateInvoiceNumber(tblInvoiceTO.IdInvoice, loginUserId, isconfirm, (int)Constants.InvoiceGenerateModeE.REGULAR);

        //        }
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMessage.DefaultExceptionBehaviour(ex, "UpdateInvoiceConfrimNonConfirmDetails");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        /// <summary>
        /// Vijaymala [30-03-2018] added:to update invoice deliveredOn date after loading slip out
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        public ResultMessage UpdateInvoiceAfterloadingSlipOut(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            #region [30-03-2018]Vijaymala Added :To update invoice delivered on date

            List<TblLoadingSlipTO> loadingSlipTOList = new List<TblLoadingSlipTO>();
            loadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(loadingId, conn, tran);
            if (loadingSlipTOList == null || loadingSlipTOList.Count == 0)
            {
                resultMessage.DefaultBehaviour("Loading Slip List Found Null againest Loading Id");
                return resultMessage;
            }
            DateTime deliveredOn = _iCommon.ServerDateTime;
            for (int i = 0; i < loadingSlipTOList.Count; i++)
            {
                List<TblInvoiceTO> invoiceToList = new List<TblInvoiceTO>();
                invoiceToList = SelectInvoiceListFromLoadingSlipId(loadingSlipTOList[i].IdLoadingSlip, conn, tran);
                if (invoiceToList == null || invoiceToList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Invoice List Found Null");
                    return resultMessage;
                }
                for (int j = 0; j < invoiceToList.Count; j++)
                {
                    TblInvoiceTO tblInvoiceTO = invoiceToList[j];
                    tblInvoiceTO.DeliveredOn = deliveredOn;
                    result = UpdateTblInvoice(tblInvoiceTO, conn, tran);

                    if (result != 1)
                    {
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                        resultMessage.Text = "Error While UpdateTblInvoice While updating Loading Slip Status";
                        return resultMessage;
                    }

                }


            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;




            #endregion
        }


        public ResultMessage UpdateInvoiceDate(TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {

                result = _iTblInvoiceDAO.UpdateInvoiceDate(tblInvoiceTO);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While UpdateInvoiceDate");
                    return resultMessage;
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateInvoiceDate");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vijaymala[22-05-2018] : Added To deacivate invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        public ResultMessage DeactivateInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            if (tempInvoiceDocumentDetailsTO == null)
            {
                resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found Null againest document Id");
                return resultMessage;
            }
            DateTime serverDateTime = _iCommon.ServerDateTime;
            tempInvoiceDocumentDetailsTO.IsActive = 0;
            tempInvoiceDocumentDetailsTO.UpdatedOn = serverDateTime;
            tempInvoiceDocumentDetailsTO.UpdatedBy = loginUserId;
            result = _iTempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour();
                resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                resultMessage.Text = "Error While UpdateTempInvoiceDocumentDetails While updating invoice document details";
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public ResultMessage UpdateIsTestCertificateInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            if (tempInvoiceDocumentDetailsTO == null)
            {
                resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found Null againest document Id");
                return resultMessage;
            }
            TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTO(tempInvoiceDocumentDetailsTO.InvoiceId);
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                DateTime serverDateTime = _iCommon.ServerDateTime;
                tempInvoiceDocumentDetailsTO.UpdatedOn = serverDateTime;
                tempInvoiceDocumentDetailsTO.UpdatedBy = loginUserId;
                result = _iTempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour();
                    resultMessage.DisplayMessage = Constants.DefaultErrorMsg;
                    resultMessage.Text = "Error While UpdateTempInvoiceDocumentDetails While updating invoice document details";
                    return resultMessage;
                }
                tran.Commit();
                conn.Close();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateIsTestCertificateInvoiceDocumentDetails");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region Deletion
        public int DeleteTblInvoice(Int32 idInvoice)
        {
            return _iTblInvoiceDAO.DeleteTblInvoice(idInvoice);
        }

        public int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceDAO.DeleteTblInvoice(idInvoice, conn, tran);
        }

        /// <summary>
        /// Vijaymala[16-04-2018]:added to delete invoices and details
        /// </summary>
        /// <param name="tblInvoiceTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>

        public ResultMessage DeleteTblInvoiceDetails(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;

            if (tblInvoiceTO != null)
            {

                #region 1. To delete Invoices and loading slip mapping
                result = _iTempLoadingSlipInvoiceBL.DeleteTempLoadingSlipInvoiceByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoice");
                    return resultMessage;
                }
                #endregion

                #region 2. To delete Invoices History
                result = _iTblInvoiceHistoryBL.DeleteTblInvoiceHistoryByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceHistoryByInvoiceId");
                    return resultMessage;
                }
                #endregion

                #region 3. To delete Invoices Document Details
                result = _iTempInvoiceDocumentDetailsDAO.DeleteTblInvoiceDocumentByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceDocumentByInvoiceId");
                    return resultMessage;
                }
                #endregion

                #region 4.To delete Invoice Address
                List<TblInvoiceAddressTO> tblInvoiceAddressList = tblInvoiceTO.InvoiceAddressTOList;
                result = _iTblInvoiceAddressBL.DeleteTblInvoiceAddressByinvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceAddressByinvoiceId");
                    return resultMessage;
                }
                #endregion



                #region 5. Delete Previous Tax Details
                result = _iTblInvoiceItemTaxDtlsBL.DeleteInvoiceItemTaxDtlsByInvId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error in DeleteTblInvoiceItemTaxDtls");
                    return resultMessage;
                }

                #endregion

                #region 6.To delete Invoice Item

                result = _iTblInvoiceItemDetailsBL.DeleteTblInvoiceItemDetailsByInvoiceId(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoiceItemDetails");
                    return resultMessage;
                }

                #endregion


                #region 7. To delete Invoices
                result = DeleteTblInvoice(tblInvoiceTO.IdInvoice, conn, tran);
                if (result == -1)
                {
                    resultMessage.DefaultBehaviour("Error While DeleteTblInvoice");
                    return resultMessage;
                }

                #endregion

            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        #endregion

        #region ExtractEnquiryData

        //public ResultMessage ExtractEnquiryData()
        //{
        //    SqlConnection bookingConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction bookingTran = null;
        //    SqlConnection enquiryConn = new SqlConnection(Startup.NewConnectionString);
        //    SqlTransaction enquiryTran = null;
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    List<TblLoadingTO> tempLoadingTOList = new List<TblLoadingTO>();
        //    List<TblInvoiceRptTO> tblInvoiceRptTOList = new List<TblInvoiceRptTO>();
        //    Dictionary<int, int> invoiceIdsList = new Dictionary<int, int>();

        //    List<int> processedLoadings = new List<int>();

        //    int result = 0;
        //    int loadingCount = 0;
        //    int totalLoading = 0;
        //    List<int> loadingIdList = new List<int>();

        //    try
        //    {

        //        if (bookingConn.State == ConnectionState.Closed)
        //        {
        //            bookingConn.Open();
        //            bookingTran = bookingConn.BeginTransaction();
        //        }
        //        TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_MIGRATE_ENQUIRY_DATA);

        //        if (configParamsTO.ConfigParamVal == "1")
        //        {
        //            if (enquiryConn.State == ConnectionState.Closed)
        //            {
        //                try
        //                {
        //                    enquiryConn.Open();
        //                    enquiryTran = enquiryConn.BeginTransaction();
        //                }
        //                catch (Exception ex)
        //                {
        //                    resultMessage.MessageType = ResultMessageE.Error;
        //                    resultMessage.DefaultBehaviour(ex.Message);
        //                    return resultMessage;
        //                }
        //            }
        //        }


        //        // Select temp loading data.
        //        tempLoadingTOList = _iTblLoadingBL.SelectAllTempLoading(bookingConn, bookingTran);
        //        if (tempLoadingTOList == null || tempLoadingTOList.Count <= 0)
        //        {
        //            resultMessage.DefaultBehaviour("Record not found!! ");
        //            resultMessage.MessageType = ResultMessageE.Information;
        //            return resultMessage;
        //        }

        //        // Select temp invoice data for creating excel file.
        //        tblInvoiceRptTOList = _iFinalBookingData.SelectTempInvoiceData(bookingConn, bookingTran);


        //        if (tempLoadingTOList != null && tempLoadingTOList.Count > 0)
        //        {

        //            foreach (var tempLoadingTO in tempLoadingTOList.ToList())
        //            {

        //                if (bookingConn.State == ConnectionState.Closed)
        //                {
        //                    bookingConn.Open();
        //                    bookingTran = bookingConn.BeginTransaction();
        //                }

        //                // Vaibhav [23-April-2018] For new changes - Single invoice against multiple loadingslip. To check all loading slip are delivered.
        //                // Select temp loading slip details.
        //                List<TblLoadingSlipTO> loadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(tempLoadingTO.IdLoading, bookingConn, bookingTran);
        //                int undeliveredLoadingSlipCount = 0;
        //                List<TblLoadingSlipTO> loadingSlipDataByInvoiceId = null;

        //                if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
        //                {
        //                    foreach (var loadingSlip in loadingSlipTOList)
        //                    {
        //                        TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOListByLoadingSlip(loadingSlip.IdLoadingSlip, bookingConn, bookingTran);

        //                        if (tempLoadingSlipInvoiceTO != null)
        //                        {
        //                            loadingSlipDataByInvoiceId = SelectLoadingSlipDetailsByInvoiceId(tempLoadingSlipInvoiceTO.InvoiceId, bookingConn, bookingTran);
        //                            if (loadingSlipDataByInvoiceId != null)
        //                            {
        //                                undeliveredLoadingSlipCount += loadingSlipDataByInvoiceId.FindAll(ele => ele.StatusId != (int)Constants.TranStatusE.LOADING_DELIVERED && ele.StatusId != (int)Constants.TranStatusE.LOADING_CANCEL).Count();
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                    }
        //                }
        //                if (undeliveredLoadingSlipCount > 0)
        //                {
        //                    tempLoadingTOList.RemoveAll(ele => ele.IdLoading == tempLoadingTO.IdLoading);
        //                    goto creatFile;
        //                }

        //                processedLoadings.Clear();
        //                if (loadingSlipDataByInvoiceId != null && loadingSlipDataByInvoiceId.Count > 0)
        //                    processedLoadings.AddRange(loadingSlipDataByInvoiceId.Select(ele => ele.LoadingId).Distinct().ToList());


        //                List<TblLoadingTO> newLoadingTOList = new List<TblLoadingTO>();

        //                if (processedLoadings != null && processedLoadings.Count > 0)
        //                {
        //                    foreach (var processedLoading in processedLoadings)
        //                    {
        //                        newLoadingTOList.AddRange(tempLoadingTOList.FindAll(e => e.IdLoading == processedLoading));
        //                    }


        //                    foreach (var newLoadingTO in newLoadingTOList)
        //                    {
        //                        loadingIdList.Add(newLoadingTO.IdLoading);

        //                        #region Handle Connection
        //                        loadingCount = loadingCount + 1;
        //                        totalLoading = totalLoading + 1;

        //                        if (bookingConn.State == ConnectionState.Closed)
        //                        {
        //                            bookingConn.Open();
        //                            bookingTran = bookingConn.BeginTransaction();
        //                        }

        //                        if (configParamsTO.ConfigParamVal == "1")
        //                        {
        //                            if (enquiryConn.State == ConnectionState.Closed)
        //                            {
        //                                enquiryConn.Open();
        //                                enquiryTran = enquiryConn.BeginTransaction();
        //                            }
        //                        }
        //                        #endregion

        //                        #region Insert Booking Data
        //                        resultMessage = _iFinalBookingData.InsertFinalBookingData(newLoadingTO.IdLoading, bookingConn, bookingTran, ref invoiceIdsList);
        //                        if (resultMessage.MessageType != ResultMessageE.Information)
        //                        {
        //                            bookingTran.Rollback();
        //                            enquiryTran.Rollback();
        //                            _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                            UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //                            resultMessage.MessageType = ResultMessageE.Error;
        //                            resultMessage.Text = "Error while InsertFinalBookingData";
        //                            return resultMessage;
        //                        }
        //                        #endregion

        //                        #region Insert Enquiry Data

        //                        //TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_MIGRATE_ENQUIRY_DATA);

        //                        if (configParamsTO.ConfigParamVal == "1")
        //                        {
        //                            resultMessage = _iFinalEnquiryData.InsertFinalEnquiryData(newLoadingTO.IdLoading, bookingConn, bookingTran, enquiryConn, enquiryTran, ref invoiceIdsList);
        //                            if (resultMessage.MessageType != ResultMessageE.Information)
        //                            {
        //                                bookingTran.Rollback();
        //                                enquiryTran.Rollback();
        //                                _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                                UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //                                resultMessage.MessageType = ResultMessageE.Error;
        //                                resultMessage.Text = "Error while InsertFinalEnquiryData";
        //                                return resultMessage;
        //                            }
        //                        }

        //                        #endregion

        //                    }
        //                    #region Delete transactional data

        //                    foreach (var newLoadingTO in newLoadingTOList)
        //                    {
        //                        result = _iFinalBookingData.DeleteTempLoadingData(newLoadingTO.IdLoading, bookingConn, bookingTran);
        //                        if (result < 0)
        //                        {
        //                            bookingTran.Rollback();
        //                            enquiryTran.Rollback();
        //                            _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                            UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //                            resultMessage.MessageType = ResultMessageE.Error;
        //                            resultMessage.Text = "Error while DeleteTempLoadingData";
        //                            return resultMessage;
        //                        }

        //                        tempLoadingTOList.RemoveAll(ele => ele.IdLoading == newLoadingTO.IdLoading);
        //                        totalLoading = totalLoading - 1;
        //                    }
        //                }
        //                #endregion



        //                #region Create Excel File. Delete Stock & Quota. Reset SQL Connection.
        //                creatFile:
        //                if (loadingCount == Constants.LoadingCountForDataExtraction || totalLoading == tempLoadingTOList.Count)
        //                {
        //                    #region Create Excel File  

        //                    TblConfigParamsTO createFileConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_CREATE_NC_DATA_FILE);

        //                    if (createFileConfigParamsTO.ConfigParamVal == "1")
        //                    {
        //                        if (tblInvoiceRptTOList != null && tblInvoiceRptTOList.Count > 0)
        //                        {
        //                            List<TblInvoiceRptTO> enquiryInvoiceList = new List<TblInvoiceRptTO>();

        //                            if (loadingIdList != null && loadingIdList.Count > 0)
        //                            {
        //                                foreach (var loadingId in loadingIdList)
        //                                {
        //                                    enquiryInvoiceList.AddRange(tblInvoiceRptTOList.FindAll(ele => ele.LoadingId == loadingId));
        //                                }
        //                            }

        //                            if (enquiryInvoiceList != null && enquiryInvoiceList.Count > 0)
        //                            {
        //                                result = _iFinalBookingData.CreateTempInvoiceExcel(enquiryInvoiceList, bookingConn, bookingTran);

        //                                if (result != 1)
        //                                {
        //                                    bookingTran.Rollback();
        //                                    enquiryTran.Rollback();
        //                                    _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                                    UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //                                    resultMessage.MessageType = ResultMessageE.Error;
        //                                    resultMessage.Text = "Error while creating excel file.";
        //                                    return resultMessage;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            resultMessage.MessageType = ResultMessageE.Information;
        //                            resultMessage.Text = "Information : tblInvoiceRptTOList is null. Excel file is not created.";
        //                            //return resultMessage;
        //                        }
        //                    }
        //                    #endregion

        //                    #region Delete Stock And Quota
        //                    result = _iFinalBookingData.DeleteYesterdaysStock(bookingConn, bookingTran);
        //                    if (result < 0)
        //                    {
        //                        bookingTran.Rollback();
        //                        enquiryTran.Rollback();
        //                        _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                        UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //                        resultMessage.MessageType = ResultMessageE.Error;
        //                        resultMessage.Text = "Error while DeleteYesterdaysStock";
        //                        return resultMessage;
        //                    }

        //                    result = _iFinalBookingData.DeleteYesterdaysLoadingQuotaDeclaration(bookingConn, bookingTran);
        //                    if (result < 0)
        //                    {
        //                        bookingTran.Rollback();
        //                        enquiryTran.Rollback();
        //                        _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                        UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //                        resultMessage.MessageType = ResultMessageE.Error;
        //                        resultMessage.Text = "Error while DeleteYesterdaysQuotaDeclaration";
        //                        return resultMessage;
        //                    }

        //                    #endregion


        //                    bookingTran.Commit();
        //                    bookingConn.Close();
        //                    bookingTran.Dispose();

        //                    if (configParamsTO.ConfigParamVal == "1")
        //                    {
        //                        enquiryTran.Commit();
        //                        enquiryConn.Close();
        //                        enquiryTran.Dispose();
        //                    }

        //                    loadingCount = 0;
        //                    loadingIdList.Clear();
        //                }
        //                #endregion
        //            }
        //        }

        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        bookingTran.Rollback();
        //        _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);

        //        if (enquiryTran.Connection.State == ConnectionState.Open)
        //        {
        //            enquiryTran.Rollback();
        //            UpdateIdentityFinalTables(enquiryConn, enquiryTran);
        //        }
        //        resultMessage.DefaultExceptionBehaviour(ex, "ExtractEnquiryData");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        bookingConn.Close();
        //        enquiryConn.Close();
        //    }
        //}

        #endregion

        #region DeleteEnquiryData
        //shifted in tblLoadingBL - YA_2019_02_11
        //public ResultMessage DeleteDispatchData()
        //{
        //    ResultMessage resultMessage = new StaticStuff.ResultMessage();
        //    SqlConnection bookingConn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //    SqlTransaction bookingTran = null;
        //    List<TblLoadingTO> tempLoadingTOList = new List<TblLoadingTO>();
        //    List<int> processedLoadings = new List<int>();

        //    int result = 0;
        //    int loadingCount = 0;
        //    int totalLoading = 0;
        //    List<int> loadingIdList = new List<int>();
        //    List<int> bookingIdList = new List<int>();
        //    String bookingIds = String.Empty;


        //    try
        //    {

        //        if (bookingConn.State == ConnectionState.Closed)
        //        {
        //            bookingConn.Open();
        //            bookingTran = bookingConn.BeginTransaction();
        //        }

        //        // Select temp loading data.
        //        tempLoadingTOList = _iTblLoadingBL.SelectAllTempLoadingOnStatus(bookingConn, bookingTran);
        //        if (tempLoadingTOList == null || tempLoadingTOList.Count <= 0)
        //        {
        //            resultMessage.DefaultBehaviour("Record not found!! ");
        //            resultMessage.MessageType = ResultMessageE.Information;
        //            return resultMessage;
        //        }
        //        if (tempLoadingTOList != null && tempLoadingTOList.Count > 0)
        //        {

        //            foreach (var tempLoadingTO in tempLoadingTOList.ToList())
        //            {

        //                if (bookingConn.State == ConnectionState.Closed)
        //                {
        //                    bookingConn.Open();
        //                    bookingTran = bookingConn.BeginTransaction();
        //                }

        //                //List<TblLoadingSlipTO> loadingSlipTOList = TblLoadingSlipBL.SelectAllLoadingSlipListWithDetails(tempLoadingTO.IdLoading, bookingConn, bookingTran);

        //                List<TblLoadingSlipTO> loadingSlipDataByInvoiceId = null;

        //                //if (loadingSlipTOList != null && loadingSlipTOList.Count > 0)
        //                //{

        //                //foreach (var loadingSlip in loadingSlipTOList)
        //                //{
        //                //   // bookingIds += "," + loadingSlip.BookingId;
        //                //    //bookingIds = bookingIds.Trim(',');
        //                //     bookingIdList.Add(loadingSlip.BookingId);
        //                //    TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO = _iTempLoadingSlipInvoiceBL.SelectTempLoadingSlipInvoiceTOListByLoadingSlip(loadingSlip.IdLoadingSlip, bookingConn, bookingTran);

        //                //    if (tempLoadingSlipInvoiceTO != null)
        //                //    {
        //                //        loadingSlipDataByInvoiceId = BL.TblInvoiceBL.SelectLoadingSlipDetailsByInvoiceId(tempLoadingSlipInvoiceTO.InvoiceId, bookingConn, bookingTran);
        //                //    }
        //                //}
        //                //}

        //                processedLoadings.Clear();
        //                if (loadingSlipDataByInvoiceId != null && loadingSlipDataByInvoiceId.Count > 0)
        //                    processedLoadings.AddRange(loadingSlipDataByInvoiceId.Select(ele => ele.LoadingId).Distinct().ToList());


        //                processedLoadings.Add(tempLoadingTO.IdLoading);

        //                List<TblLoadingTO> newLoadingTOList = new List<TblLoadingTO>();

        //                if (processedLoadings != null && processedLoadings.Count > 0)
        //                {
        //                    //foreach (var processedLoading in processedLoadings)
        //                    //{
        //                    //    newLoadingTOList.AddRange(tempLoadingTOList.FindAll(e => e.IdLoading == processedLoading));
        //                    //}
        //                    //foreach (var newLoadingTO in newLoadingTOList)
        //                    //{
        //                    //    loadingIdList.Add(newLoadingTO.IdLoading);
        //                    //    loadingCount = loadingCount + 1;
        //                    //    totalLoading = totalLoading + 1;

        //                    //    if (bookingConn.State == ConnectionState.Closed)
        //                    //    {
        //                    //        bookingConn.Open();
        //                    //        bookingTran = bookingConn.BeginTransaction();
        //                    //    }
        //                    //}

        //                    if (bookingConn.State == ConnectionState.Closed)
        //                    {
        //                        bookingConn.Open();
        //                        bookingTran = bookingConn.BeginTransaction();
        //                    }

        //                    newLoadingTOList.Add(tempLoadingTO);

        //                    #region Delete transactional data

        //                    foreach (var newLoadingTO in newLoadingTOList)
        //                    {
        //                        result = _iFinalBookingData.DeleteDispatchTempLoadingData(newLoadingTO.IdLoading, bookingConn, bookingTran);
        //                        if (result < 0)
        //                        {
        //                            bookingTran.Rollback();
        //                            _iFinalBookingData.UpdateIdentityFinalTables(bookingConn, bookingTran);
        //                            resultMessage.MessageType = ResultMessageE.Error;
        //                            resultMessage.Text = "Error while DeleteDispatchTempLoadingData";
        //                            return resultMessage;
        //                        }

        //                        tempLoadingTOList.RemoveAll(ele => ele.IdLoading == newLoadingTO.IdLoading);
        //                        totalLoading = totalLoading - 1;

        //                    }


        //                    bookingTran.Commit();
        //                    bookingConn.Close();

        //                }
        //                #endregion
        //            }


        //            if (bookingConn.State == ConnectionState.Closed)
        //            {
        //                bookingConn.Open();
        //                bookingTran = bookingConn.BeginTransaction();
        //            }

        //            resultMessage = _iTblBookingsBL.DeleteAllBookings(bookingIdList, bookingConn, bookingTran);
        //            if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
        //            {
        //                bookingTran.Rollback();
        //                resultMessage.DefaultBehaviour("Error while Deleting BookingDispatchData");
        //                return resultMessage;
        //            }

        //            bookingTran.Commit();
        //            bookingConn.Close();
        //            bookingTran.Dispose();
        //            loadingCount = 0;
        //            loadingIdList.Clear();
        //        }

        //        resultMessage.DefaultSuccessBehaviour();
        //        return resultMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        bookingTran.Rollback();
        //        resultMessage.DefaultExceptionBehaviour(ex, "DeleteDispatchData");
        //        return resultMessage;
        //    }
        //    finally
        //    {
        //        bookingConn.Close();
        //    }
        //}

        #endregion

        //#regionDelete Invoice and Reverse Weighing Dtl

        public ResultMessage DeleteWeighingDtlData(int loadingslipid)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            List<TblLoadingSlipTO> loadingSlipTOList = null;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                List<TblLoadingTO> TblLoadingTOList = new List<TblLoadingTO>();
                List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();
                TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(loadingslipid);
                if (tblLoadingSlipTO != null)
                {
                    TblLoadingTO tblLoadingTO = _iTblLoadingDAO.SelectTblLoading(tblLoadingSlipTO.LoadingId);
                    if (tblLoadingTO != null)
                    {
                        tblLoadingSlipTOList.Add(tblLoadingSlipTO);
                        TblLoadingTOList.Add(tblLoadingTO);
                    }
                }
                if (tblLoadingSlipTO != null && TblLoadingTOList.Count > 0)
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                }
                List<TblLoadingSlipTO> loadingSlipTOListTempData = tblLoadingSlipTOList.GroupBy(g => g.LoadingId).FirstOrDefault().ToList();
                for (int k = 0; k < loadingSlipTOListTempData.Count; k++)
                {
                    result = DeleteDispatchTempData(DelTranTablesE.tempWeighingMeasures.ToString(), loadingSlipTOListTempData[k].LoadingId, conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while Deleting TempWeighingMeasures");
                        return resultMessage;
                    }

                    result = _iTblLoadingSlipDAO.UpdateTblLoadingSlipStatus(loadingSlipTOListTempData[k], conn, tran);
                    if (result < 0)
                    {
                        resultMessage.DefaultBehaviour("Error while UpdateTblLoadingSlipStatus");
                        return resultMessage;
                    }

                    for (int i = 0; i < tblLoadingSlipTOList.Count; i++)
                    {
                        result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExtForWeghing(tblLoadingSlipTOList[i].IdLoadingSlip,0, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while UpdateTblLoadingSlipExtForWeghing ");
                            return resultMessage;
                        }
                    }
                    if (result > 0)
                    {
                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, ex.Source.ToString());

            }
            finally
            {
                conn.Close();
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }


        public ResultMessage ReverseWeighingDtlData(int invoiceId, int userId)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            List<TblLoadingSlipTO> loadingSlipTOList = null;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                #region Delete Loading Slip Data
                List<TblLoadingTO> TblLoadingTOList = new List<TblLoadingTO>();
                List<TblLoadingSlipTO> tblLoadingSlipTOList = new List<TblLoadingSlipTO>();
                List<TempLoadingSlipInvoiceTO> TempLoadingSlipInvoiceTOList = _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId);
                if (TempLoadingSlipInvoiceTOList != null && TempLoadingSlipInvoiceTOList.Count > 0)
                {
                    //Reshma Added
                    TempLoadingSlipInvoiceTOList[0].UpdatedBy = userId;
                    TempLoadingSlipInvoiceTOList[0].UpdatedOn = _iCommon.ServerDateTime;
                    int res = _iTempLoadingSlipInvoiceDAO.InsertTempLoadingSlipInvoiceHistory(TempLoadingSlipInvoiceTOList[0]);
                    if (res != 1)
                    {
                        resultMessage.DefaultBehaviour("Error while InsertTempLoadingSlipInvoiceHistory");
                        return resultMessage;
                    }
                    List<TempLoadingSlipInvoiceTO> distinctLoadingSlipIdList = TempLoadingSlipInvoiceTOList.GroupBy(g => g.LoadingSlipId).FirstOrDefault().ToList();
                    if (distinctLoadingSlipIdList != null && distinctLoadingSlipIdList.Count > 0)
                    {
                        for (int i = 0; i < distinctLoadingSlipIdList.Count; i++)
                        {
                            TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(distinctLoadingSlipIdList[i].LoadingSlipId);
                            if (tblLoadingSlipTO != null)
                            {
                                TblLoadingTO tblLoadingTO = _iTblLoadingDAO.SelectTblLoading(tblLoadingSlipTO.LoadingId);
                                if (tblLoadingTO != null)
                                {
                                    tblLoadingSlipTOList.Add(tblLoadingSlipTO);
                                    TblLoadingTOList.Add(tblLoadingTO);
                                }
                            }
                        }
                    }
                }
                if (tblLoadingSlipTOList != null && tblLoadingSlipTOList.Count > 0 && TblLoadingTOList != null && TblLoadingTOList.Count > 0)
                {
                    List<TblInvoiceTO> tblInvoiceTOList = _iTblInvoiceDAO.SelectInvoiceListFromInvoiceIds(invoiceId.ToString());
                    if (tblInvoiceTOList != null && tblInvoiceTOList.Count > 0)
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        result = DeleteDispatchTempData(DelTranTablesE.tempLoadingSlipInvoice.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlipInvoice");
                            return resultMessage;
                        }
                        result = DeleteDispatchTempData(DelTranTablesE.tempInvoiceHistory.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceHistory");
                            return resultMessage;
                        }


                        //result = DeleteTempData(DelTranTablesE.tempInvoiceItemTaxDtls.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                        result = DeleteDispatchTempData(DelTranTablesE.tempInvoiceItemTaxDtls.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceItemTaxDtls");
                            return resultMessage;
                        }


                        //result = DeleteTempData(DelTranTablesE.tempInvoiceItemDetails.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                        result = DeleteDispatchTempData(DelTranTablesE.tempInvoiceItemDetails.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceItemDetails");
                            return resultMessage;
                        }

                        //result = DeleteTempData(DelTranTablesE.tempInvoiceAddress.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                        result = DeleteDispatchTempData(DelTranTablesE.tempInvoiceAddress.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceAddress");
                            return resultMessage;
                        }

                        //[28-05-2018]:Vijaymala added to delete tempinvoicedocumentdetails.
                        result = DeleteDispatchTempData(DelTranTablesE.tempInvoiceDocumentDetails.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempInvoiceDocumentDetails");
                            return resultMessage;
                        }

                        result = DeleteDispatchTempData(DelTranTablesE.tempEInvoiceApiResponse.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempEInvoiceApiResponse");
                            return resultMessage;
                        }

                        result = DeleteDispatchTempData(DelTranTablesE.tempInvoice.ToString(), invoiceId, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempInvoice");
                            return resultMessage;
                        }

                        //result = DeleteDispatchTempData(DelTranTablesE.tempLoadingSlipExtHistory.ToString(), loadingSlipTO.IdLoadingSlip, conn, tran);
                        if (result < 0)
                        {
                            resultMessage.DefaultBehaviour("Error while Deleting TempLoadingSlipExtHistory");
                            return resultMessage;
                        }
                        List<TblLoadingSlipTO> loadingSlipTOListTempData = tblLoadingSlipTOList.GroupBy(g => g.LoadingId).FirstOrDefault().ToList();
                        for (int k = 0; k < loadingSlipTOListTempData.Count; k++)
                        {
                            result = DeleteDispatchTempData(DelTranTablesE.tempWeighingMeasures.ToString(), loadingSlipTOListTempData[k].LoadingId, conn, tran);
                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error while Deleting TempWeighingMeasures");
                                return resultMessage;
                            }
                            result = _iTblLoadingSlipDAO.UpdateTblLoadingSlipStatus(loadingSlipTOListTempData[k], conn, tran);
                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error while UpdateTblLoadingSlipStatus");
                                return resultMessage;
                            }
                            result = _iTblLoadingDAO.UpdateTblLoadingWeighingDetails(loadingSlipTOListTempData[k].LoadingId, conn, tran);
                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error while UpdateTblLoadingWeighingDetails");
                                return resultMessage;
                            }
                        }
                        for (int k = 0; k < tblLoadingSlipTOList.Count; k++)
                        {
                            result = _iTblLoadingSlipExtDAO.UpdateTblLoadingSlipExtForWeghing(tblLoadingSlipTOList[k].IdLoadingSlip,userId, conn, tran);
                            if (result < 0)
                            {
                                resultMessage.DefaultBehaviour("Error while UpdateTblLoadingSlipExtForWeghing ");
                                return resultMessage;
                            }
                        }

                        if (result > 0)
                        {
                            tran.Commit();
                        }
                    }
                }

                #endregion
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }

            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "ReverseWeighingDtlData");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public enum DelTranTablesE
        {
            tempLoadingSlipInvoice,
            tempInvoiceHistory,
            tempInvoiceItemTaxDtls,
            tempInvoiceItemDetails,
            tempInvoiceAddress,
            tempInvoice,
            tempLoadingSlipExtHistory,
            tempLoadingSlipExt,
            tempLoadingSlipDtl,
            tempLoadingSlipAddress,
            tempLoadingSlip,
            tempWeighingMeasures,
            tempLoadingStatusHistory,
            tempLoading,
            tempInvoiceDocumentDetails,
            tblBookingBeyondQuota,
            tblBookingExt,
            tblBookingQtyConsumption,
            tblBookingDelAddr,
            tblBookingSchedule,
            tblQuotaConsumHistory,
            tblBookingOpngBal,
            tblLoadingSlipRemovedItems,
            tblBookings,
            tblBookingParities,
            tempEInvoiceApiResponse //Dhananjay added [23-12-2020]
        }
        private int DeleteDispatchTempData(String delTableName, int delId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;

                String sqlQuery = null;

                //Saket [2018-05-11] Added.
                //String strWhereCondtion = "select invoiceId FROM tempLoadingSlipInvoice WHERE loadingSlipId = " + delId + "";
                String strWhereCondtion = delId.ToString();

                switch ((DelTranTablesE)Enum.Parse(typeof(DelTranTablesE), delTableName))
                {
                    case DelTranTablesE.tempInvoice:
                        //sqlQuery = " DELETE FROM tempInvoice WHERE loadingSlipId = " + delId;
                        sqlQuery = " DELETE FROM tempInvoice WHERE idInvoice IN (" + strWhereCondtion + ")";
                        break;

                    case DelTranTablesE.tempInvoiceAddress:
                        //sqlQuery = " DELETE FROM tempInvoiceAddress WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        sqlQuery = " DELETE FROM tempInvoiceAddress WHERE invoiceId IN ( " + strWhereCondtion + " ) ";
                        break;

                    //[28-05-2018]:Vijaymala added to delete tempinvoicedocumentdetails.
                    case DelTranTablesE.tempInvoiceDocumentDetails:
                        sqlQuery = " DELETE FROM tempInvoiceDocumentDetails WHERE invoiceId IN ( " + strWhereCondtion + " ) ";
                        break;

                    case DelTranTablesE.tempInvoiceHistory:
                        //sqlQuery = " DELETE FROM tempInvoiceHistory WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        sqlQuery = " DELETE FROM tempInvoiceHistory WHERE invoiceId IN ( " + strWhereCondtion + " ) ";
                        break;

                    case DelTranTablesE.tempInvoiceItemDetails:
                        //sqlQuery = " DELETE FROM tempInvoiceItemDetails WHERE invoiceId IN (SELECT idInvoice FROM tempInvoice WHERE loadingSlipId = " + delId + " ) ";
                        sqlQuery = " DELETE FROM tempInvoiceItemDetails WHERE invoiceId IN ( " + strWhereCondtion + " ) ";
                        break;

                    case DelTranTablesE.tempInvoiceItemTaxDtls:
                        //sqlQuery = " DELETE FROM tempInvoiceItemTaxDtls WHERE invoiceItemId IN " +
                        //          " (SELECT idInvoiceItem FROM tempInvoiceItemDetails invoiceItemDetails " +
                        //          " INNER JOIN tempInvoice invoice ON invoiceItemDetails.invoiceId = invoice.idInvoice " +
                        //          " WHERE invoice.loadingSlipId = " + delId + " )";

                        sqlQuery = " DELETE FROM tempInvoiceItemTaxDtls WHERE invoiceItemId IN " +
                                 " (SELECT idInvoiceItem FROM tempInvoiceItemDetails invoiceItemDetails " +
                                 " INNER JOIN tempInvoice invoice ON invoiceItemDetails.invoiceId = invoice.idInvoice " +
                                 " WHERE invoice.IdInvoice IN ( " + strWhereCondtion + " ) )";

                        break;

                    case DelTranTablesE.tempLoading:
                        sqlQuery = " DELETE FROM tempLoading WHERE idLoading = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlip:
                        sqlQuery = " DELETE FROM tempLoadingSlip WHERE idLoadingSlip = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipAddress:
                        sqlQuery = " DELETE FROM tempLoadingSlipAddress WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipDtl:
                        sqlQuery = " DELETE FROM tempLoadingSlipDtl WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipExt:
                        sqlQuery = " DELETE FROM tempLoadingSlipExt WHERE loadingSlipId = " + delId;
                        break;

                    case DelTranTablesE.tempLoadingSlipExtHistory:
                        sqlQuery = " DELETE FROM tempLoadingSlipExtHistory WHERE loadingSlipExtId IN " +
                                  " (SELECT idLoadingSlipExt FROM tempLoadingSlipExt loadingSlipExt " +
                                  " INNER JOIN tempLoadingSlipExtHistory loadingSlipExtHistory ON " +
                                  " loadingSlipExt.idLoadingSlipExt = loadingSlipExtHistory.loadingSlipExtId" +
                                  " WHERE loadingSlipExt.loadingSlipId = " + delId + ")";
                        break;

                    case DelTranTablesE.tempLoadingStatusHistory:
                        sqlQuery = " DELETE FROM tempLoadingStatusHistory WHERE loadingId = " + delId;
                        break;

                    case DelTranTablesE.tempWeighingMeasures:
                        sqlQuery = " DELETE FROM tempWeighingMeasures WHERE loadingId = " + delId;
                        break;
                    case DelTranTablesE.tempEInvoiceApiResponse:
                        sqlQuery = " DELETE FROM tempEInvoiceApiResponse WHERE invoiceId IN (" + strWhereCondtion + ") ";
                        break;
                    case DelTranTablesE.tempLoadingSlipInvoice:
                        sqlQuery = " DELETE FROM tempLoadingSlipInvoice WHERE invoiceId = " + delId;
                        break;
                }

                if (sqlQuery != null)
                {
                    cmdDelete.CommandText = sqlQuery;
                    return cmdDelete.ExecuteNonQuery();
                }
                else
                    return -1;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeleteTempData");
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }
        //#endregion
        #region Send Invoce
        public int sendInvoiceFromMail(SendMail sendMail)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                if (sendMail.IsUpdatePerson)
                {
                    result = _iTblPersonDAO.UpdateTblPerson(sendMail.PersonInfo, conn, tran);

                    if (result != 1)
                    {
                        tran.Rollback();
                        return -1;
                    }
                }


                #region Commented OLD Code
                //List<TblPersonTO> mailToList = BL._iTblPersonBL.SelectAllPersonListByOrganization(sendMail.SenderId);
                //if (mailToList != null)
                //{
                //    foreach (var person in mailToList)
                //    {
                //        if (person.PrimaryEmail != null)
                //        {
                //            sendMail.To = person.PrimaryEmail;
                //            break;
                //        }
                //        else
                //        {
                //            if (person.AlternateEmail != null)
                //            {
                //                sendMail.To = person.AlternateEmail;
                //                break;
                //            }
                //        }
                //    }
                //}
                #endregion

                if (sendMail.To == null)
                {
                    return -1;
                }
                if (sendMail.IsWeighmentSlipAttach == true && sendMail.IsInvoiceAttach == true)
                {
                    sendMail.Subject = "Invoice and Weighment Slip -" + sendMail.InvoiceId + "-" + sendMail.InvoiceNumber + " ";
                }
                else if (sendMail.IsWeighmentSlipAttach == false && sendMail.IsInvoiceAttach == true)
                {
                    sendMail.Subject = "Invoice-" + sendMail.InvoiceId + "-" + sendMail.InvoiceNumber;
                }
                else
                {
                    sendMail.Subject = "Weighment Slip-" + sendMail.InvoiceId + "-" + sendMail.InvoiceNumber;
                }
                result = _iSendMailBL.SendEmail(sendMail);
                if (result != 1)
                {
                    tran.Rollback();
                    return -1;
                }
                TblEmailHistoryTO tblEmailHistoryTO = new TblEmailHistoryTO();
                tblEmailHistoryTO.SendBy = sendMail.From;
                tblEmailHistoryTO.SendTo = sendMail.To;
                tblEmailHistoryTO.SendOn = _iCommon.ServerDateTime;
                tblEmailHistoryTO.CreatedBy = sendMail.CreatedBy;
                tblEmailHistoryTO.InvoiceId = sendMail.InvoiceId;

                result = _iTblEmailHistoryDAO.InsertTblEmailHistory(tblEmailHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    return -1;
                }

                tran.Commit();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region Post Invoice to SAP
        public ResultMessage PostSalesInvoiceToSAP(TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour("SAP Company Object Found NULL");
                    return resultMessage;
                }

                #region 1. Create Sale Order Against Invoice

                SAPbobsCOM.Documents saleOrderDocument;
                saleOrderDocument = (SAPbobsCOM.Documents)Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                saleOrderDocument.CardCode = tblInvoiceTO.DealerOrgId.ToString();
                saleOrderDocument.CardName = tblInvoiceTO.DealerName;
                saleOrderDocument.DocDate = _iCommon.ServerDateTime;
                saleOrderDocument.DocDueDate = saleOrderDocument.DocDate;
                saleOrderDocument.EndDeliveryDate = saleOrderDocument.DocDate;
                Dictionary<string, double> itemQtyDCT = new Dictionary<string, double>();
                int itemCount = 0;
                for (int i = 0; i < tblInvoiceTO.InvoiceItemDetailsTOList.Count; i++)
                {
                    //Freight Or Other taxes to ignore from this
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].OtherTaxId > 0)
                        continue;

                    saleOrderDocument.Lines.SetCurrentLine(itemCount);

                    #region Get Item code Details from RM to FG Configuration and Prod Gst Code Number
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].ProdGstCodeId == 0)
                    {
                        resultMessage.DefaultBehaviour("ProdGstCodeId - GSTINCode/HSN Code not found for this item :" + tblInvoiceTO.InvoiceItemDetailsTOList[i].ProdItemDesc);
                        return resultMessage;
                    }

                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtlsTO(tblInvoiceTO.InvoiceItemDetailsTOList[i].ProdGstCodeId);
                    if (tblProdGstCodeDtlsTO == null)
                    {
                        resultMessage.DefaultBehaviour("tblProdGstCodeDtlsTO Found NULL. Hence Mapped Sap Item Can not be found");
                        return resultMessage;
                    }

                    Int64 productItemId = _iDimensionDAO.GetProductItemIdFromGivenRMDetails(tblProdGstCodeDtlsTO.ProdCatId, tblProdGstCodeDtlsTO.ProdSpecId, tblProdGstCodeDtlsTO.MaterialId, 0, tblProdGstCodeDtlsTO.ProdItemId);
                    if (productItemId <= 0)
                    {
                        resultMessage.DefaultBehaviour("Invoice Item and FG Item Linkgae Not Found in configuration: Check tblProductItemRmToFGConfig");
                        return resultMessage;
                    }

                    saleOrderDocument.Lines.ItemCode = productItemId.ToString();
                    itemQtyDCT.Add(saleOrderDocument.Lines.ItemCode, tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceQty);

                    #endregion

                    #region Quantity, Price and discount details

                    saleOrderDocument.Lines.Quantity = tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceQty;
                    saleOrderDocument.Lines.UnitPrice = tblInvoiceTO.InvoiceItemDetailsTOList[i].Rate;
                    saleOrderDocument.Lines.DiscountPercent = tblInvoiceTO.InvoiceItemDetailsTOList[i].CdStructure;

                    #endregion

                    #region Get Tax Details

                    string sapTaxCode = string.Empty;
                    if (tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList != null
                        && tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList.Count > 0)
                    {
                        TblTaxRatesTO tblTaxRatesTO = _iTblTaxRatesDAO.SelectTblTaxRates(tblInvoiceTO.InvoiceItemDetailsTOList[i].InvoiceItemTaxDtlsTOList[0].TaxRateId);
                        if (tblTaxRatesTO != null)
                            sapTaxCode = tblTaxRatesTO.SapTaxCode;
                    }
                    saleOrderDocument.Lines.TaxCode = sapTaxCode;

                    #endregion

                    saleOrderDocument.Lines.Add();
                    itemCount++;
                }

                int result = saleOrderDocument.Add();
                if (result != 0)
                {
                    string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorMsg);
                    resultMessage.DisplayMessage = errorMsg;
                }
                else
                {
                    string TxnId = Startup.CompanyObject.GetNewObjectKey();
                    tblInvoiceTO.SapMappedSalesOrderNo = TxnId;
                    resultMessage.DefaultSuccessBehaviour();
                }
                #endregion

                #region 2. Do Stock Adjustment Against Invoice Items

                #endregion

                #region 3. Create Sale Invoice Against Above Order Ref

                #endregion

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostSalesInvoiceToSAP");
                return resultMessage;
            }
        }

        #endregion

        #region Reports

        public List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId, int fromOrgId)
        {
            return _iTblInvoiceDAO.SelectOtherTaxDetailsReport(frmDt, toDt, isConfirm, otherTaxId, fromOrgId);
        }

        public int UpdateIdentityFinalTables(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                cmdUpdate.CommandTimeout = 0;

                String sqlQuery = null;

                foreach (FinalBookingData.DelTranTablesE tableName in Enum.GetValues(typeof(FinalBookingData.DelTranTablesE)))
                {
                    switch (tableName)
                    {
                        case FinalBookingData.DelTranTablesE.tempInvoice:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoice) FROM enquiryInvoice),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoice, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceAddress:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceAddr) FROM enquiryInvoiceAddress),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceAddress, RESEED, @id) ";
                            break;

                        //[28-05-2018]:Vijaymala added to update identity of enquiryInvoicedocumentdetails
                        case FinalBookingData.DelTranTablesE.tempInvoiceDocumentDetails:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceDocument) FROM enquiryInvoicedocumentdetails),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoicedocumentdetails, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvHistory) FROM enquiryInvoiceHistory),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceHistory, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceItemDetails:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvoiceItem) FROM enquiryInvoiceItemDetails),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceItemDetails, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempInvoiceItemTaxDtls:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idInvItemTaxDtl) FROM enquiryInvoiceItemTaxDtls),0) " +
                                       " DBCC CHECKIDENT(enquiryInvoiceItemTaxDtls, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoading:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoading) FROM enquiryLoading),0) " +
                                       " DBCC CHECKIDENT(enquiryLoading, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlip:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingSlip) FROM enquiryLoadingSlip),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlip, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipAddress:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadSlipAddr) FROM enquiryLoadingSlipAddress),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipAddress, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipDtl:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadSlipDtl) FROM enquiryLoadingSlipDtl),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipDtl, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipExt:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingSlipExt) FROM enquiryLoadingSlipExt),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipExt, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingSlipExtHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idConfirmHistory) FROM enquiryLoadingSlipExtHistory),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingSlipExtHistory, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempLoadingStatusHistory:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idLoadingHistory) FROM enquiryLoadingStatusHistory),0) " +
                                       " DBCC CHECKIDENT(enquiryLoadingStatusHistory, RESEED, @id) ";
                            break;

                        case FinalBookingData.DelTranTablesE.tempWeighingMeasures:
                            sqlQuery = " DECLARE @id int " +
                                       " SET @id = ISNULL((SELECT MAX(idWeightMeasure) FROM enquiryWeighingMeasures),0) " +
                                       " DBCC CHECKIDENT(enquiryWeighingMeasures, RESEED, @id)";
                            break;
                    }

                    if (sqlQuery != null)
                    {
                        cmdUpdate.CommandText = sqlQuery;
                        result = cmdUpdate.ExecuteNonQuery();
                    }
                    else
                        result = -1;
                }
                return result;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateIdentityFinalTables");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }
        #endregion

        #region eInvoice

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        public ResultMessage GenerateEInvoice(Int32 loginUserId, Int32 idInvoice, Int32 eInvoiceCreationType, bool forceToGetToken = false)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                string sellerGstin = "27AACCK4472B1ZS";
                TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
                tblInvoiceTO = SelectTblInvoiceTOWithDetails(idInvoice);
                if (tblInvoiceTO == null)
                {
                    throw new Exception("InvoiceTO is null");
                }
                List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
                if (TblOrgLicenseDtlTOList != null)
                {
                    for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                    {
                        if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                        {
                            sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                            break;
                        }
                    }
                }

                string access_token_OauthToken = null;
                resultMsg = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_OauthToken");
                }

                access_token_OauthToken = resultMsg.Tag.ToString();
                if (access_token_OauthToken == null)
                {
                    throw new Exception("access_token_OauthToken is null");
                }

                string access_token_Authentication = null;
                resultMsg = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }

                access_token_Authentication = resultMsg.Tag.ToString();
                if (access_token_Authentication != null)
                {
                    resultMsg = EInvoice_Generate(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin, eInvoiceCreationType);
                    Int32 IS_SEND_CUSTOM_NOTIFICATION = 0;
                    TblConfigParamsTO tblConfigParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_WhatsApp_Msg);
                    if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                    {
                        IS_SEND_CUSTOM_NOTIFICATION = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                    }
                    if (resultMsg.Result == 1 && IS_SEND_CUSTOM_NOTIFICATION == 1)//Reshma Added
                    {
                        // //Reshma Added FOr WhatsApp integration.
                        resultMsg = SendFileOnWhatsAppAfterEwayBillGeneration(tblInvoiceTO.IdInvoice);
                    }

                    return resultMsg;
                }
                return resultMsg;
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Text = ex.Message;
                return resultMsg;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Get Oauth Token for eInvvoice.
        /// </summary>
        public ResultMessage EInvoice_OauthToken(Int32 loginUserId, string sellerGstin, bool forceToGetToken, Int32 OrgId)
        {
            string access_token_OauthToken = null;
            DateTime tokenExpiresAt = _iCommon.ServerDateTime;
            ResultMessage resultMsg = new ResultMessage();

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.OAUTH_TOKEN);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            if (forceToGetToken == false)
            {
                if (tblEInvoiceApiTO.SessionExpiresAt > _iCommon.ServerDateTime)
                {
                    resultMsg.DefaultSuccessBehaviour();
                    resultMsg.Tag = tblEInvoiceApiTO.AccessToken;
                    return resultMsg;
                }
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            JObject json = JObject.Parse(response.Content);
            access_token_OauthToken = (string)json["access_token"];

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMsg = InsertIntoTblEInvoiceSessionApiResponse(response, tblEInvoiceApiTO.IdApi, loginUserId, OrgId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (access_token_OauthToken == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                int expires_in = (int)json["expires_in"];
                tokenExpiresAt = _iCommon.ServerDateTime.AddSeconds(expires_in - secsToBeDeductededFromTokenExpTime);

                resultMsg = UpdateTblEInvoiceApi(tblEInvoiceApiTO.IdApi, access_token_OauthToken, tokenExpiresAt, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                tran.Commit();
                resultMsg.Tag = access_token_OauthToken;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_OauthToken");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Get Authentication for eInvvoice.
        /// </summary>
        public ResultMessage EInvoice_Authentication(Int32 loginUserId, string access_token_OauthToken, string sellerGstin, bool forceToGetToken, int OrgId)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_OauthToken == "")
            {
                resultMsg = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, OrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_OauthToken");
                }
                else
                {
                    access_token_OauthToken = resultMsg.Tag.ToString();
                }
            }
            string access_Token_Authentication = null;
            DateTime tokenExpiresAt = _iCommon.ServerDateTime;

            //List<TblEInvoiceApiTO> tblEInvoiceApiTOList = _iTblEInvoiceApiDAO.SelectAllTblEInvoiceApi((int)EInvoiceAPIE.EINVOICE_AUTHENTICATE);
            List<TblEInvoiceApiTO> tblEInvoiceApiTOList = _iTblEInvoiceApiDAO.SelectTblEInvoiceApi(Constants.EINVOICE_AUTHENTICATE, OrgId);
            if (tblEInvoiceApiTOList == null)
            {
                resultMsg.DefaultExceptionBehaviour(new Exception("EInvoiceApiTOList is null"), "EInvoice_Authentication");
                return resultMsg;
            }
            if (tblEInvoiceApiTOList.Count == 0)
            {
                resultMsg.DefaultExceptionBehaviour(new Exception("EInvoiceApiTOList not found"), "EInvoice_Authentication");
                return resultMsg;
            }
            if (forceToGetToken == false)
            {
                if (tblEInvoiceApiTOList[0].SessionExpiresAt > _iCommon.ServerDateTime)
                {
                    resultMsg.DefaultSuccessBehaviour();
                    resultMsg.Tag = tblEInvoiceApiTOList[0].AccessToken;
                    return resultMsg;
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = tblEInvoiceApiTOList[0];
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_OauthToken);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            JObject json = JObject.Parse(response.Content);
            access_Token_Authentication = (string)json["access_token"];

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                resultMsg = InsertIntoTblEInvoiceSessionApiResponse(response, tblEInvoiceApiTO.IdApi, loginUserId, OrgId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (access_Token_Authentication == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                int expires_in = (int)json["expires_in"];
                tokenExpiresAt = _iCommon.ServerDateTime.AddSeconds(expires_in - secsToBeDeductededFromTokenExpTime);

                resultMsg = UpdateTblEInvoiceApi(tblEInvoiceApiTO.IdApi, access_Token_Authentication, tokenExpiresAt, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }
                resultMsg.Tag = access_Token_Authentication;
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_Authentication");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        private string padRight(string str, char pad = '-', int totalWidth = 3)
        {
            str = RemoveSpecialChars(str);
            return str.PadRight(totalWidth, pad);
        }

        private string GetValidVehichleNumber(string vehicleNumber)
        {
            vehicleNumber = RemoveSpecialChars(vehicleNumber).Replace(" ", "").Replace("-", "");
            int n, nOut;
            string last4Digit = "";
            if (vehicleNumber.Length >= 4)
            {
                for (n = 4; n > 0; n--)
                {
                    last4Digit = vehicleNumber.Substring(vehicleNumber.Length - n, n);
                    if (int.TryParse(last4Digit, out nOut) == true)
                    {
                        if (n == 4) break;
                    }
                    else
                    {
                        last4Digit = last4Digit.PadLeft(4, char.Parse("0"));
                        vehicleNumber = vehicleNumber.Substring(0, vehicleNumber.Length - n) + last4Digit;
                        break;
                    }
                }
            }
            return vehicleNumber;
        }
        private string RemoveSpecialChars(string str)
        {
            if (String.IsNullOrEmpty(str)) return "";

            str = str.Replace("\r\n", "");
            str = str.Replace("\t", "");
            str = str.Replace("\n", "");
            str = str.Trim();

            return str;
        }

        private string GetAreaFromAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            string area = "";
            if (tblInvoiceAddressTO.VillageName != null && tblInvoiceAddressTO.VillageName != "")
            {
                area += tblInvoiceAddressTO.VillageName + ", ";
            }
            else if (tblInvoiceAddressTO.Taluka != null && tblInvoiceAddressTO.Taluka != "")
            {
                area += tblInvoiceAddressTO.Taluka + ", ";
            }
            else if (tblInvoiceAddressTO.District != null && tblInvoiceAddressTO.District != "")
            {
                area += tblInvoiceAddressTO.District + ", ";
            }

            if (String.IsNullOrEmpty(area))
            {
                area = "---";
            }
            else
            {
                area = area.Trim().TrimEnd(',');
            }

            area = RemoveSpecialChars(area);
            if (area.Length < 3)
            {
                area = padRight(area);
            }
            return area;
        }

        /// <summary>
        /// Dhananjay[06-01-2021] : Added To Generate eInvvoice.
        /// </summary>
        public TblEInvoiceApiTO GetTblEInvoiceApiTO(int idAPI)
        {
            List<TblEInvoiceApiTO> tblEInvoiceApiTOList = _iTblEInvoiceApiDAO.SelectAllTblEInvoiceApi(idAPI);
            if (tblEInvoiceApiTOList == null)
            {
                return null;
            }
            if (tblEInvoiceApiTOList.Count == 0)
            {
                return null;
            }
            TblEInvoiceApiTO tblEInvoiceApiTO = tblEInvoiceApiTOList[0];
            return tblEInvoiceApiTO;
        }
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        public ResultMessage EInvoice_Generate(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_Token_Authentication, string sellerGstin, Int32 eInvoiceCreationType)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                if (access_Token_Authentication == "")
                {
                    resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                    if (resultMsg.Result != 1)
                    {
                        throw new Exception("Error in EInvoice_Authentication");
                    }
                    else
                    {
                        access_Token_Authentication = resultMsg.Tag.ToString();
                    }
                }

                TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GENERATE_EINVOICE);
                if (tblEInvoiceApiTO == null)
                {
                    throw new Exception("EInvoiceApiTO is null");
                }
                /*TblEInvoiceApiTO tblEInvoiceApiTOEWayBill = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GENERATE_EWAYBILL);
                if (tblEInvoiceApiTOEWayBill == null)
                {
                    throw new Exception("EInvoiceApiTO for eWayBill is null");
                }*/

                tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
                tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_Token_Authentication);
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@invoiceType", "INV");
                if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)//Reshma Addded
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@supTyp", "EXPWP");
                else
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@supTyp", "B2B");

                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@invoiceNo", tblInvoiceTO.InvoiceNo); //"AUG070720-19"
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@invoiceDate", tblInvoiceTO.InvoiceDate.Day.ToString("00") + "/" + tblInvoiceTO.InvoiceDate.Month.ToString("00") + "/" + tblInvoiceTO.InvoiceDate.Year.ToString("0000")); //"12/08/2020"

                int invFromOrgId = tblInvoiceTO.InvFromOrgId;
                TblOrganizationTO organizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(invFromOrgId);
                TblInvoiceAddressTO billingAddrTO = null;
                TblInvoiceAddressTO consigneeAddrTO = null;
                TblInvoiceAddressTO shippingAddrTO = null;
                TblAddressTO sellerAddressTO = null;
                if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceTO.InvoiceAddressTOList.Count; i++)
                    {
                        if (tblInvoiceTO.InvoiceAddressTOList[i].TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS)
                        {
                            billingAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                        }
                        else if (tblInvoiceTO.InvoiceAddressTOList[i].TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS)
                        {
                            consigneeAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                        }
                        else if (tblInvoiceTO.InvoiceAddressTOList[i].TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.SHIPPING_ADDRESS)
                        {
                            shippingAddrTO = tblInvoiceTO.InvoiceAddressTOList[i];
                        }
                    }
                    if (shippingAddrTO == null)
                    {
                        if (consigneeAddrTO == null)
                        {
                            shippingAddrTO = billingAddrTO; //if shipping and consignee address not available
                        }
                        else
                        {
                            shippingAddrTO = consigneeAddrTO; //if shipping address not available
                        }
                    }
                }
                string sellerName = "";
                string sellerEmailAddr = "test@einv.com";
                string sellerPhoneNo = "9100000000";
                TblOrganizationTO tblSellerOrgTO = _iTblOrganizationBL.SelectTblOrganizationTO(invFromOrgId);
                if (tblSellerOrgTO != null)
                {
                    sellerName = tblSellerOrgTO.FirmName;
                    if (tblSellerOrgTO.EmailAddr != null)
                    {
                        if (tblSellerOrgTO.EmailAddr.Length >= 3 && tblSellerOrgTO.EmailAddr.Length <= 100)
                        {
                            sellerEmailAddr = tblSellerOrgTO.EmailAddr;
                        }
                    }
                    if (tblSellerOrgTO.PhoneNo != null)
                    {
                        if (tblSellerOrgTO.PhoneNo.Length >= 6 && tblSellerOrgTO.PhoneNo.Length <= 12)
                        {
                            sellerPhoneNo = tblSellerOrgTO.PhoneNo;
                        }
                    }
                    List<TblAddressTO> tblAddressTOList = _iTblAddressBL.SelectOrgAddressList(invFromOrgId);
                    if (tblAddressTOList != null)
                    {
                        if (tblAddressTOList.Count > 0)
                        {
                            sellerAddressTO = tblAddressTOList[0];
                        }
                    }
                }
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerGstIn", RemoveSpecialChars(sellerGstin));
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerName", RemoveSpecialChars(sellerName));
                if (sellerAddressTO != null)
                {
                    string sellerAddr1 = RemoveSpecialChars(sellerAddressTO.PlotNo);
                    string sellerAddr2 = RemoveSpecialChars(sellerAddressTO.StreetName);
                    string sellerArea = "";
                    if (sellerAddressTO.AreaName != null && sellerAddressTO.AreaName != "")
                    {
                        sellerArea += sellerAddressTO.AreaName + ", ";
                    }
                    if (sellerAddressTO.VillageName != null && sellerAddressTO.VillageName != "")
                    {
                        sellerArea += sellerAddressTO.VillageName + ", ";
                    }
                    if (sellerAddressTO.TalukaName != null && sellerAddressTO.TalukaName != "")
                    {
                        sellerArea += sellerAddressTO.TalukaName + ", ";
                    }
                    if (sellerAddressTO.DistrictName != null && sellerAddressTO.DistrictName != "")
                    {
                        sellerArea += sellerAddressTO.DistrictName + ", ";
                    }

                    if (String.IsNullOrEmpty(sellerArea))
                    {
                        sellerArea = "---";
                    }
                    else
                    {
                        sellerArea = sellerArea.Trim().TrimEnd(',');
                    }

                    sellerArea = RemoveSpecialChars(sellerArea);
                    if (sellerArea.Length < 3)
                    {
                        sellerArea = padRight(sellerArea);
                    }
                    if (sellerAddr1 == null || sellerAddr1 == "")
                    {
                        sellerAddr1 = sellerArea;
                    }

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerAddr1", padRight(sellerAddr1));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerAddr2", padRight(sellerAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerLocation", padRight(sellerArea));
                    /*if (sellerAddressTO.Pincode == 0)
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerPincode", "110001");
                    }
                    else
                    {*/
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerPincode", RemoveSpecialChars(sellerAddressTO.Pincode.ToString()));
                    //}
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerStateCode", RemoveSpecialChars(sellerAddressTO.StateOrUTCode));

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerPhone", RemoveSpecialChars(sellerPhoneNo));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sellerEMail", RemoveSpecialChars(sellerEmailAddr));

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromAddr1", padRight(sellerAddr1));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromAddr2", padRight(sellerAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromLocation", padRight(sellerArea));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromPincode", RemoveSpecialChars(sellerAddressTO.Pincode.ToString()));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@dispFromStateCode", RemoveSpecialChars(sellerAddressTO.StateOrUTCode));
                }
                string buyerName = tblInvoiceTO.DealerName;
                string buyerEmailAddr = "";
                TblOrganizationTO tblBuyerOrgTO = _iTblOrganizationBL.SelectTblOrganizationTO(tblInvoiceTO.DealerOrgId);
                if (tblBuyerOrgTO != null)
                {
                    buyerName = tblBuyerOrgTO.FirmName;
                    buyerEmailAddr = tblBuyerOrgTO.EmailAddr;
                }
                if (billingAddrTO != null)
                {
                    //Saket Added
                    buyerName = billingAddrTO.BillingName;

                    string billingAddr2 = GetAreaFromAddress(billingAddrTO);
                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerGstIn", "URP");
                    else
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerGstIn", RemoveSpecialChars(billingAddrTO.GstinNo.ToUpper()));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerName", RemoveSpecialChars(buyerName));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerAddr1", padRight(billingAddrTO.Address));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerAddr2", padRight(billingAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerLocation", padRight(billingAddr2));
                    /*if (billingAddrTO.PinCode == "0")
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPincode", "110001");
                    }
                    else
                    {*/
                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPincode", "999999");
                    else
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPincode", RemoveSpecialChars(billingAddrTO.PinCode));
                    //}
                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerStateCode", "96");
                    else
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerStateCode", RemoveSpecialChars(billingAddrTO.StateOrUTCode));
                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerSupplyStateCode", "96");
                    else
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerSupplyStateCode", RemoveSpecialChars(billingAddrTO.StateOrUTCode));
                    string contactNo = "9100000000";
                    if (billingAddrTO.ContactNo != null)
                    {
                        if (billingAddrTO.ContactNo.Length >= 6 && billingAddrTO.ContactNo.Length <= 12)
                        {
                            contactNo = billingAddrTO.ContactNo;
                        }
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerPhone", RemoveSpecialChars(contactNo));
                    if (buyerEmailAddr != null)
                    {
                        if (buyerEmailAddr.Length >= 3 && buyerEmailAddr.Length <= 100)
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerEMail", RemoveSpecialChars(buyerEmailAddr));
                        }
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@buyerEMail", "test@einv.com"); //if buyerEmailAddr is not available
                }

                if (shippingAddrTO != null)
                {
                    string shippingAddr2 = GetAreaFromAddress(shippingAddrTO);
                    if (string.IsNullOrEmpty(shippingAddrTO.BillingName))
                    {
                        shippingAddrTO.BillingName = buyerName;
                    }

                    if (String.IsNullOrEmpty(billingAddrTO.BillingName) || String.IsNullOrEmpty(billingAddrTO.Address) || String.IsNullOrEmpty(billingAddrTO.GstinNo) || String.IsNullOrEmpty(shippingAddrTO.BillingName) || String.IsNullOrEmpty(shippingAddrTO.Address) || String.IsNullOrEmpty(shippingAddrTO.GstinNo))
                    {
                        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EINVOICE_SHIPPING_ADDRESS);
                        if (tblConfigParamsTO == null)
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", "");
                        }
                        else
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", tblConfigParamsTO.ConfigParamVal);
                        }
                    }
                    else
                    {
                        //[2021-02-26] Dhananjay added
                        if (shippingAddrTO.BillingName.Trim() == billingAddrTO.BillingName.Trim() && shippingAddrTO.Address.Trim() == billingAddrTO.Address.Trim() && shippingAddrTO.GstinNo.Trim() == billingAddrTO.GstinNo.Trim())
                        {
                            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", "");
                        }
                        else
                        {
                            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EINVOICE_SHIPPING_ADDRESS);
                            if (tblConfigParamsTO == null)
                            {
                                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", "");
                            }
                            else
                            {
                                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingAddr", tblConfigParamsTO.ConfigParamVal);
                            }
                        }
                        //[2021-02-26] Dhananjay end
                    }

                    if (string.IsNullOrEmpty(shippingAddrTO.GstinNo))
                    {
                        //shippingAddrTO.GstinNo = billingAddrTO.GstinNo;
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingName", RemoveSpecialChars(shippingAddrTO.BillingName));

                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)//Reshma Added For Export
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingGstIn", "URP");
                    else if (string.IsNullOrEmpty(shippingAddrTO.GstinNo))
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("\"gstin\": \"@shippingGstIn\",", "");
                    }
                    else
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shippingGstIn", RemoveSpecialChars(shippingAddrTO.GstinNo.ToUpper()));
                    }
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToAddr1", padRight(shippingAddrTO.Address));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToAddr2", padRight(shippingAddr2));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToLocation", padRight(shippingAddr2));
                    /*if (shippingAddrTO.PinCode == "0")
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToPincode", "110001");
                    }
                    else
                    {*/
                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToPincode", "999999");
                    else
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToPincode", RemoveSpecialChars(shippingAddrTO.PinCode));

                    //}
                    if (tblInvoiceTO.InvoiceTypeE == Constants.InvoiceTypeE.EXPORT_INVOICE)//Reshma Added
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToStateCode", "96");
                    else
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@shipToStateCode", RemoveSpecialChars(shippingAddrTO.StateOrUTCode));

                }

                //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@othChrg", tblInvoiceTO.FreightAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@cgstAmt", tblInvoiceTO.CgstAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@sgstAmt", tblInvoiceTO.SgstAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@igstAmt", tblInvoiceTO.IgstAmt.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@grandTotal", tblInvoiceTO.GrandTotal.ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@roundOffAmt", tblInvoiceTO.RoundOffAmt.ToString());

                if (eInvoiceCreationType == (Int32)Constants.EGenerateEInvoiceCreationType.GENERATE_INVOICE_ONLY)
                {
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", "");
                }
                else if (eInvoiceCreationType == (Int32)Constants.EGenerateEInvoiceCreationType.INVOICE_WITH_EWAY_BILL)
                {
                    //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("\r\n\t\t@ewbDtls", ",\r\n        \"EwbDtls\":{\r\n        \"VehType\": \"R\",\r\n        \"VehNo\": \"@vehicleNo\",\r\n        \"TransMode\": \"1\",\r\n        \"Distance\": \"@distanceInKM\"\r\n        }");
                    //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@vehicleNo", GetValidVehichleNumber(tblInvoiceTO.VehicleNo));
                    //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@distanceInKM", RemoveSpecialChars(tblInvoiceTO.DistanceInKM.ToString()));

                    /*tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("{\"action\": \"GENERATEEWB\",", "");
                    tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("data", "EwbDtls");
                    tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("\"Irn\": \"@IrnNo\",", "");
                    tblEInvoiceApiTOEWayBill.BodyParam = tblEInvoiceApiTOEWayBill.BodyParam.Replace("}\r\n}", "}");

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", ",\r\n        " + tblEInvoiceApiTOEWayBill.BodyParam);*/

                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_EINVOICE_EWAY_BILL);
                    if (tblConfigParamsTO == null)
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", "");
                    }
                    else
                    {
                        tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbDtls", tblConfigParamsTO.ConfigParamVal);
                    }

                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@VehNo", GetValidVehichleNumber(tblInvoiceTO.VehicleNo));
                    tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@DistanceinKm", RemoveSpecialChars(tblInvoiceTO.DistanceInKM.ToString()));
                }
                else
                {
                    resultMsg.DefaultBehaviour("Wrong eInvoiceType");
                    return resultMsg;
                }
                int nStartIdx = tblEInvoiceApiTO.BodyParam.IndexOf("slNo");
                int nEndIdx = tblEInvoiceApiTO.BodyParam.IndexOf("@idInvoiceItem");
                string itemList = tblEInvoiceApiTO.BodyParam.Substring(nStartIdx - 20, ((nEndIdx - nStartIdx) + "@idInvoiceItem".Length + 36));
                string itemListReplacedWithValue = "";
                List<TblInvoiceItemDetailsTO> InvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                Int32 nSrNo = 0;
                double otherCharges = 0;
                double taxableAmt = 0;
                for (int i = 0; i <= InvoiceItemDetailsTOList.Count - 1; i++)
                {
                    TblInvoiceItemDetailsTO InvoiceItemDetailsTO = InvoiceItemDetailsTOList[i];
                    string itemListTobeReplaced = itemList;

                    String unit = "MTS";

                    TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = _iTblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTO(InvoiceItemDetailsTO.ProdGstCodeId);
                    if (tblProdGstCodeDtlsTO == null)
                    {
                        resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against IdInvoiceItem is : " + InvoiceItemDetailsTO.IdInvoiceItem + ".");
                        resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + InvoiceItemDetailsTO.ProdItemDesc;
                        return resultMsg;
                    }
                    TblProductItemTO tblProductItemTO = null;
                    if (tblProdGstCodeDtlsTO.ProdItemId > 0)
                    {
                        tblProductItemTO = _iTblProductItemBL.SelectTblProductItemTO(tblProdGstCodeDtlsTO.ProdItemId);

                        if (tblProductItemTO != null)
                        {
                            if (!string.IsNullOrEmpty(tblProductItemTO.MappedEInvoiceUOM))
                            {
                                unit = tblProductItemTO.MappedEInvoiceUOM;
                            }
                        }
                    }
                    if (InvoiceItemDetailsTO.OtherTaxId > 0)
                    {
                        TblOtherTaxesTO otherTaxesTO = _iTblOtherTaxesDAO.SelectTblOtherTaxes(InvoiceItemDetailsTO.OtherTaxId);
                        if (otherTaxesTO != null)
                        {
                            if (otherTaxesTO.IsBefore == 1)
                            {
                                //itemListTobeReplaced = itemListTobeReplaced.Replace("@srNo", "-");
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@isServc", "Y");
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@gstinCodeNo", "9965");
                            }
                            else if (otherTaxesTO.IsAfter == 1)
                            {
                                otherCharges += InvoiceItemDetailsTO.GrandTotal;
                                continue;
                            }
                        }
                    }
                    if (InvoiceItemDetailsTO.InvoiceQty == 0)
                    {
                        itemListTobeReplaced = itemListTobeReplaced.Replace("@basicTotal", InvoiceItemDetailsTO.TaxableAmt.ToString());
                    }
                    taxableAmt += InvoiceItemDetailsTO.TaxableAmt;
                    nSrNo++;
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@srNo", nSrNo.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@prodItemDesc", padRight(InvoiceItemDetailsTO.ProdItemDesc, char.Parse(" ")));
                    if (tblInvoiceTO.InvoiceTypeId == (int)Constants.InvoiceTypeE.SERVICE_INVOICE)
                        itemListTobeReplaced = itemListTobeReplaced.Replace("@isServc", "Y");
                    else
                        itemListTobeReplaced = itemListTobeReplaced.Replace("@isServc", "N");

                    itemListTobeReplaced = itemListTobeReplaced.Replace("@gstinCodeNo", padRight(InvoiceItemDetailsTO.GstinCodeNo, char.Parse("-"), 4));
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@invoiceQty", string.Format("{0:0.000}", InvoiceItemDetailsTO.InvoiceQty));
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@unit", unit);
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@rate", InvoiceItemDetailsTO.Rate.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@basicTotal", InvoiceItemDetailsTO.BasicTotal.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@discount", InvoiceItemDetailsTO.CdAmt.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@pretaxAmt", InvoiceItemDetailsTO.TaxableAmt.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@assAmt", InvoiceItemDetailsTO.TaxableAmt.ToString());

                    Double TaxRatePct = 0;
                    if (InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList != null && InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 0)
                    {
                        for (int nTax = 0; nTax <= InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count - 1; nTax++)
                        {
                            TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = InvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[nTax];
                            //itemListTobeReplaced = itemListTobeReplaced.Replace("@taxRatePct", tblInvoiceItemTaxDtlsTO.TaxRatePct.ToString());

                            TaxRatePct += tblInvoiceItemTaxDtlsTO.TaxRatePct;
                            if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                            {
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@igstItemAmt", tblInvoiceItemTaxDtlsTO.TaxAmt.ToString());
                            }
                            else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                            {
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@cgstItemAmt", tblInvoiceItemTaxDtlsTO.TaxAmt.ToString());
                            }
                            else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                            {
                                itemListTobeReplaced = itemListTobeReplaced.Replace("@sgstItemAmt", tblInvoiceItemTaxDtlsTO.TaxAmt.ToString());
                            }
                        }
                    }
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@taxRatePct", TaxRatePct.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@igstItemAmt", "0");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@cgstItemAmt", "0");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@sgstItemAmt", "0");
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@totItemVal", InvoiceItemDetailsTO.GrandTotal.ToString());
                    itemListTobeReplaced = itemListTobeReplaced.Replace("@idInvoiceItem", InvoiceItemDetailsTO.IdInvoiceItem.ToString());

                    if (i == 0)
                    {
                        itemListReplacedWithValue += itemListTobeReplaced;
                    }
                    else
                    {
                        itemListReplacedWithValue += ",\r\n            " + itemListTobeReplaced;
                    }
                }

                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace(itemList, itemListReplacedWithValue);
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@taxableAmt", Math.Round(taxableAmt, 2).ToString());
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@othChrg", Math.Round(otherCharges, 2).ToString());

                IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

                resultMsg = ProcessEInvoiceAPIResponse(tblInvoiceTO, (int)EInvoiceAPIE.GENERATE_EINVOICE, loginUserId, response, true);
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Text = ex.Message;
                return resultMsg;
            }
            finally
            {

            }
            return resultMsg;
        }

        private ResultMessage ProcessEInvoiceAPIResponse(TblInvoiceTO tblInvoiceTO, Int32 IdAPI, Int32 loginUserId, IRestResponse response, bool bNewInvoice)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                string IrnNo = null;
                string EwayBillNo = null;
                JObject json = JObject.Parse(response.Content);
                if (json.ContainsKey("data"))
                {
                    JObject jsonData = JObject.Parse(json["data"].ToString());
                    IrnNo = (string)jsonData["Irn"];
                    if (jsonData.ContainsKey("EwbNo"))
                    {
                        EwayBillNo = (string)jsonData["EwbNo"];
                    }
                }
                if (json.ContainsKey("error"))
                {
                    JArray arrError = JArray.Parse(json["error"].ToString());
                    foreach (var err in arrError)
                    {
                        JObject jsonError = JObject.Parse(err.ToString());
                        string errorCodes = (string)jsonError["errorCodes"];
                        string errorMsg = (string)jsonError["errorMsg"];
                        //if (errorCodes == "1005" && errorMsg == "Invalid Token")
                        //{
                        //    GenerateEInvoice(loginUserId, tblInvoiceTO.IdInvoice, eInvoiceCreationType, true);
                        //    return null;
                        //}
                    }
                }
                SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
                SqlTransaction tran = null;
                try
                {
                    conn.Open();
                    tran = conn.BeginTransaction();

                    resultMsg = InsertIntoTblEInvoiceApiResponse(IdAPI, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);
                    if (resultMsg.Result != 1)
                    {
                        tran.Rollback();
                        return resultMsg;
                    }

                    if (IrnNo == null)
                    {
                        tran.Commit();
                        resultMsg.DefaultBehaviour(json.ToString());
                        resultMsg.DisplayMessage = json.ToString();
                        resultMsg.Text = resultMsg.DisplayMessage;
                        return resultMsg;
                    }

                    tblInvoiceTO.IrnNo = IrnNo;
                    tblInvoiceTO.IsEInvGenerated = 1;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);

                    resultMsg = UpdateTempInvoiceForEInvoice(tblInvoiceTO, conn, tran);
                    if (resultMsg.Result == 0)
                    {
                        tran.Rollback();
                        return resultMsg;
                    }

                    resultMsg.DisplayMessage = "eInvoice generated successfully;";
                    if (bNewInvoice == false)
                    {
                        resultMsg.DisplayMessage = "eInvoice details updated successfully;";
                    }

                    if (EwayBillNo != null)
                    {
                        tblInvoiceTO.ElectronicRefNo = EwayBillNo;
                        tblInvoiceTO.IsEWayBillGenerated = 1;
                        tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);

                        resultMsg = UpdateTempInvoiceForEWayBill(tblInvoiceTO, conn, tran);
                        if (resultMsg.Result == 0)
                        {
                            tran.Rollback();
                            return resultMsg;
                        }
                        resultMsg.DisplayMessage = "eInvoice and eWayBill generated successfully;";
                        if (bNewInvoice == false)
                        {
                            resultMsg.DisplayMessage = "eInvoice and eWayBill details updated successfully;";
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_Generate");
                    return resultMsg;
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                resultMsg.MessageType = ResultMessageE.Error;
                resultMsg.Text = ex.Message;
                return resultMsg;
            }
            finally
            {

            }
            return resultMsg;
        }

        private IRestResponse CallRestAPIs(string ApiBaseUri, string ApiMethod, string HeaderParam, string BodyParam)
        {
            var client = new RestClient(ApiBaseUri);
            client.Timeout = -1;
            Method method = Method.POST;
            if (ApiMethod.ToUpper() == "POST")
            {
                method = Method.POST;
            }
            else if (ApiMethod.ToUpper() == "GET")
            {
                method = Method.GET;
            }
            else if (ApiMethod.ToUpper() == "PUT")
            {
                method = Method.PUT;
            }
            var request = new RestRequest(method);

            JObject oHeaderParam = JObject.Parse(HeaderParam);
            foreach (var token in oHeaderParam)
            {
                request.AddHeader(token.Key, token.Value.ToString());
            }

            if (oHeaderParam["Content-Type"].ToString() == "application/x-www-form-urlencoded")
            {
                JObject oBodyParam = JObject.Parse(BodyParam);
                foreach (var token in oBodyParam)
                {
                    request.AddParameter(token.Key, token.Value.ToString());
                }
            }
            else if (oHeaderParam["Content-Type"].ToString() == "application/json")
            {
                request.AddParameter("application/json", BodyParam, ParameterType.RequestBody);
            }

            return client.Execute(request);
        }

        private ResultMessage InsertIntoTblEInvoiceSessionApiResponse(IRestResponse response, Int32 apiId, Int32 loginUserId, Int32 OrgId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblEInvoiceSessionApiResponseTO tblEInvoiceSessionApiResponseTO = new TblEInvoiceSessionApiResponseTO();
            tblEInvoiceSessionApiResponseTO.ApiId = apiId;
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                JObject json = JObject.Parse(response.Content);
                string access_token = (string)json["access_token"];
                tblEInvoiceSessionApiResponseTO.AccessToken = access_token;
                tblEInvoiceSessionApiResponseTO.Response = response.Content;
            }
            else
            {
                tblEInvoiceSessionApiResponseTO.Response = response.ErrorMessage;
            }
            tblEInvoiceSessionApiResponseTO.ResponseStatus = response.ResponseStatus.ToString();

            DateTime serverDate = _iCommon.ServerDateTime;
            tblEInvoiceSessionApiResponseTO.CreatedBy = Convert.ToInt32(loginUserId);
            tblEInvoiceSessionApiResponseTO.CreatedOn = serverDate;
            tblEInvoiceSessionApiResponseTO.OrgId = OrgId;
            result = _iTblEInvoiceSessionApiResponseDAO.InsertTblEInvoiceSessionApiResponse(tblEInvoiceSessionApiResponseTO, conn, tran);
            if (result != 1)
            {
                resultMessage.Text = "Sorry..Record Could not be saved.";
                resultMessage.DisplayMessage = "Error while insert into TblEInvoiceSessionApiResponse";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private ResultMessage UpdateTblEInvoiceApi(int IdApi, string access_token, DateTime tokenExpiresAt, int loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblEInvoiceApiTO tblEInvoiceApiTO = new TblEInvoiceApiTO();
            tblEInvoiceApiTO.IdApi = IdApi;
            tblEInvoiceApiTO.IsSession = 1;
            tblEInvoiceApiTO.AccessToken = access_token;
            tblEInvoiceApiTO.SessionExpiresAt = tokenExpiresAt;
            DateTime serverDate1 = _iCommon.ServerDateTime;
            tblEInvoiceApiTO.UpdatedBy = Convert.ToInt32(loginUserId);
            tblEInvoiceApiTO.UpdatedOn = serverDate1;
            result = _iTblEInvoiceApiDAO.UpdateTblEInvoiceApiSession(tblEInvoiceApiTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update TblEInvoiceApi");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private ResultMessage InsertIntoTblEInvoiceApiResponse(Int32 apiId, Int32 InvoiceId, IRestResponse response, Int32 loginUserId, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            TblEInvoiceApiResponseTO tblEInvoiceApiResponseTO = new TblEInvoiceApiResponseTO();
            tblEInvoiceApiResponseTO.ApiId = apiId;
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                tblEInvoiceApiResponseTO.InvoiceId = InvoiceId;
                tblEInvoiceApiResponseTO.Response = response.Content;
            }
            else
            {
                tblEInvoiceApiResponseTO.Response = response.ErrorMessage;
            }
            tblEInvoiceApiResponseTO.ResponseStatus = response.ResponseStatus.ToString();

            DateTime serverDate = _iCommon.ServerDateTime;
            tblEInvoiceApiResponseTO.CreatedBy = Convert.ToInt32(loginUserId);
            tblEInvoiceApiResponseTO.CreatedOn = serverDate;
            result = _iTblEInvoiceApiResponseDAO.InsertTblEInvoiceApiResponse(tblEInvoiceApiResponseTO, conn, tran);
            if (result != 1)
            {
                resultMessage.Text = "Sorry..Record Could not be saved.";
                resultMessage.DisplayMessage = "Error while insert into TempEInvoiceApiResponse";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private ResultMessage UpdateTempInvoiceForEInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();

            int result = 0;
            DateTime serverDate1 = _iCommon.ServerDateTime;
            tblInvoiceTO.UpdatedOn = serverDate1;
            result = _iTblInvoiceDAO.UpdateEInvoicNo(tblInvoiceTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update tempInvoice for EInvoice");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private ResultMessage UpdateTempInvoiceForEWayBill(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            DateTime serverDate1 = _iCommon.ServerDateTime;
            tblInvoiceTO.UpdatedOn = serverDate1;
            result = _iTblInvoiceDAO.UpdateEWayBill(tblInvoiceTO, conn, tran);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update tempInvoice for EWaybill");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        private ResultMessage UpdateTempInvoiceDistanceInKM(TblInvoiceTO tblInvoiceTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            DateTime serverDate1 = _iCommon.ServerDateTime;
            tblInvoiceTO.UpdatedOn = serverDate1;
            result = _iTblInvoiceDAO.UpdateTempInvoiceDistanceInKM(tblInvoiceTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While Update tempInvoice DistanceInKM");
            }
            else
            {
                resultMessage.DefaultSuccessBehaviour();
            }
            return resultMessage;
        }

        public ResultMessage UpdateInvoiceAddress(List<TblInvoiceAddressTO> tblInvoiceAddressTOList)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (tblInvoiceAddressTOList != null && tblInvoiceAddressTOList.Count > 0)
                {
                    for (int i = 0; i < tblInvoiceAddressTOList.Count; i++)
                    {
                        result = _iTblInvoiceAddressBL.UpdateTblInvoiceAddress(tblInvoiceAddressTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error in Insert UpdateTblInvoiceAddress");
                            return resultMessage;
                        }
                    }
                }
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        public ResultMessage CancelEInvoice(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AACCK4472B1ZS";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTO(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_Cancel(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        public ResultMessage EInvoice_Cancel(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.CANCEL_EINVOICE);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@IrnNo", tblInvoiceTO.IrnNo);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            string IrnNo = null;
            JObject json = JObject.Parse(response.Content);
            if (json.ContainsKey("data"))
            {
                JObject jsonData = JObject.Parse(json["data"].ToString());
                IrnNo = (string)jsonData["Irn"];
            }
            if (json.ContainsKey("error"))
            {
                JArray arrError = JArray.Parse(json["error"].ToString());
                foreach (var err in arrError)
                {
                    JObject jsonError = JObject.Parse(err.ToString());
                    string errorCodes = (string)jsonError["errorCodes"];
                    string errorMsg = (string)jsonError["errorMsg"];
                    if (errorCodes == "1005" && errorMsg == "Invalid Token")
                    {
                        CancelEInvoice(loginUserId, tblInvoiceTO.IdInvoice, true);
                        return null;
                    }
                    if (errorCodes == "2270")
                    {
                        response.Content = response.Content.Replace("The allowed cancellation time limit is crossed, you cannot cancel the IRN", "You cannot cancel eInvoice after 24 hrs of generation.");
                        json = JObject.Parse(response.Content);
                    }
                }
            }

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMsg = InsertIntoTblEInvoiceApiResponse(tblEInvoiceApiTO.IdApi, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (IrnNo == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                tblInvoiceTO.IrnNo = "";
                tblInvoiceTO.IsEInvGenerated = 0;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                resultMsg = UpdateTempInvoiceForEInvoice(tblInvoiceTO, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }
                resultMsg.DisplayMessage = "eInvoice cancelled successfully;";
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_Cancel");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get and Update eInvvoice.
        /// </summary>
        public ResultMessage GetAndUpdateEInvoice(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AACCK4472B1ZS";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTO(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_GetAndUpdate(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get and Update eInvvoice.
        /// </summary>
        public ResultMessage EInvoice_GetAndUpdate(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GET_EINVOICE);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            string ApiFunctionName = tblEInvoiceApiTO.ApiFunctionName.Replace("IrnNo", tblInvoiceTO.IrnNo);
            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            resultMsg = ProcessEInvoiceAPIResponse(tblInvoiceTO, (int)EInvoiceAPIE.GENERATE_EINVOICE, loginUserId, response, false);
            return resultMsg;
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate ewaybill.
        /// </summary>
        public ResultMessage GenerateEWayBill(Int32 loginUserId, Int32 idInvoice, decimal distanceInKM, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AACCK4472B1ZS";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTOWithDetails(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            tblInvoiceTO.DistanceInKM = distanceInKM;
            tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
            resultMessage = UpdateTempInvoiceDistanceInKM(tblInvoiceTO);
            if (resultMessage.Result != 1)
            {
                return resultMessage;
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_EWayBill(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        public ResultMessage EInvoice_EWayBill(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.GENERATE_EWAYBILL);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@IrnNo", tblInvoiceTO.IrnNo);
            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@VehNo", GetValidVehichleNumber(tblInvoiceTO.VehicleNo));
            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@DistanceinKm", tblInvoiceTO.DistanceInKM.ToString());

            if ((tblInvoiceTO.TransporterName) == null)
            {
                tblInvoiceTO.TransporterName = "   ";
            }
            if ((tblInvoiceTO.TransporterName) != null && (tblEInvoiceApiTO.BodyParam.Contains("@TransName") == true))
            {
                tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@TransName", tblInvoiceTO.TransporterName.ToString());
            }
            //tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@TransName", tblInvoiceTO.TransporterName.ToString());

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            string EwayBillNo = null;
            JObject json = JObject.Parse(response.Content);
            if (json.ContainsKey("data"))
            {
                JObject jsonData = JObject.Parse(json["data"].ToString());
                EwayBillNo = (string)jsonData["EwbNo"];
            }
            if (json.ContainsKey("error"))
            {
                JArray arrError = JArray.Parse(json["error"].ToString());
                foreach (var err in arrError)
                {
                    JObject jsonError = JObject.Parse(err.ToString());
                    string errorCodes = (string)jsonError["errorCodes"];
                    string errorMsg = (string)jsonError["errorMsg"];
                    if (errorCodes == "1005" && errorMsg == "Invalid Token")
                    {
                        GenerateEWayBill(loginUserId, tblInvoiceTO.IdInvoice, tblInvoiceTO.DistanceInKM, true);
                        return null;
                    }
                }
            }

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMsg = InsertIntoTblEInvoiceApiResponse(tblEInvoiceApiTO.IdApi, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (EwayBillNo == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                tblInvoiceTO.ElectronicRefNo = EwayBillNo;
                tblInvoiceTO.IsEWayBillGenerated = 1;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);

                resultMsg = UpdateTempInvoiceForEWayBill(tblInvoiceTO, conn, tran);
                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                resultMsg.DisplayMessage = "eWayBill generated successfully;";
                tran.Commit();
               // //Reshma Added FOr WhatsApp integration.
               //resultMsg   = SendFileOnWhatsAppAfterEwayBillGeneration(tblInvoiceTO.IdInvoice);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_EWayBill");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }

        public ResultMessage SendFileOnWhatsAppAfterEwayBillGeneration(int invoiceId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMSg = new ResultMessage();
            int result = 0;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                ExcelPackage excelPackage = new ExcelPackage();
                TblConfigParamsTO tblConfigParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DELIVER_IS_SEND_CUSTOM_WhatsApp_Msg);
                if (tblConfigParamsTOTemp != null && !String.IsNullOrEmpty(tblConfigParamsTOTemp.ConfigParamVal))
                {
                    Int32 IS_SEND_CUSTOM_NOTIFICATIONS = Convert.ToInt32(tblConfigParamsTOTemp.ConfigParamVal);
                    if (IS_SEND_CUSTOM_NOTIFICATIONS == 1)
                    {
                        resultMessage = PrintReportV2(invoiceId, true, false, false);

                        //ResultMessage resuMsg = PrintWeighingReport(invoiceId, false, Constants.WeighmentSlip, false);
                        TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTOWithDetails(invoiceId);
                        if (tblInvoiceTO == null)
                        {
                            resultMessage.DefaultBehaviour("tblInvoiceTO is NULL in Send whatsApp Msg");
                            return resultMessage;
                        }
                        String fileName = resultMessage.Tag.ToString();


                        Byte[] bytes = File.ReadAllBytes(fileName);
                        TblDocumentDetailsTO tblDocumentDetailsT0 = new TblDocumentDetailsTO();
                        tblDocumentDetailsT0.FileData = bytes;
                        tblDocumentDetailsT0.FileTypeId = 2;
                        tblDocumentDetailsT0.ModuleId = 1;
                        tblDocumentDetailsT0.CreatedBy = 1;
                        tblDocumentDetailsT0.CreatedOn = _iCommon.ServerDateTime;

                        tblDocumentDetailsT0.IsActive = 1;
                        tblDocumentDetailsT0.Extension = "pdf";
                        tblDocumentDetailsT0.DocumentDesc = invoiceId.ToString() + ".pdf";
                        List<TblDocumentDetailsTO> tblDocumentDetailsTOList = new List<TblDocumentDetailsTO>();
                        tblDocumentDetailsTOList.Add(tblDocumentDetailsT0);


                        conn.Open();
                        tran = conn.BeginTransaction();
                        resultMessage = _iTblDocumentDetailsBL.UploadDocumentWithConnTran(tblDocumentDetailsTOList, conn, tran);
                        if (resultMessage.MessageType != ResultMessageE.Information)
                        {
                            resultMessage.DefaultBehaviour("Error To Upload Documenrt");
                            return resultMessage;
                        }
                        #region 2. Save the Invoice Document Linking 
                        if (tblInvoiceTO == null)
                        {
                            resultMessage.DefaultBehaviour("Error : InvoiceTO Found Empty Or Null");
                            return resultMessage;
                        }
                        TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = new TempInvoiceDocumentDetailsTO();
                        if (resultMessage != null && resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(List<TblDocumentDetailsTO>))
                        {
                            List<TblDocumentDetailsTO> tblDocumentDetailsTOListTemp = (List<TblDocumentDetailsTO>)resultMessage.Tag;
                            if (tblDocumentDetailsTOListTemp == null && tblDocumentDetailsTOListTemp.Count == 0)
                            {
                                resultMessage.DefaultBehaviour("Error : Document List Found Empty Or Null");
                                return resultMessage;
                            }

                            DateTime serverDateTime = _iCommon.ServerDateTime;
                            tempInvoiceDocumentDetailsTO.DocumentId = tblDocumentDetailsTOListTemp[0].IdDocument;
                            tempInvoiceDocumentDetailsTO.InvoiceId = tblInvoiceTO.IdInvoice;
                            tempInvoiceDocumentDetailsTO.CreatedBy = 1;
                            tempInvoiceDocumentDetailsTO.CreatedOn = _iCommon.ServerDateTime;
                            tempInvoiceDocumentDetailsTO.IsActive = 1;
                            result = _iTempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                resultMessage.DefaultBehaviour("Error in Insert InvoiceTbl");
                                return resultMessage;
                            }
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour("Error To Upload Documenrt");
                            return resultMessage;
                        }
                        #endregion
                        if (resultMessage.MessageType == ResultMessageE.Information)
                        {
                            tran.Commit();
                            resultMSg.DefaultSuccessBehaviour();
                        }
                        else
                        {
                            tran.Rollback();
                        }
                        //return resultMessage;
                        List<TblDocumentDetailsTO> tblDocumentDetailsTOListtemp = (List<TblDocumentDetailsTO>)resultMessage.Tag;
                        if (tblDocumentDetailsTOListtemp == null)
                        {
                            resultMessage.DefaultBehaviour("tblDocumentDetailsTOListtemp is null in send WhatsApp Msg");
                            return resultMessage;
                        }
                        String uploadedFileName = tblDocumentDetailsTOListtemp[0].Path;
                        String fileName1 =  Path.GetFileName(uploadedFileName);
                        TblOrganizationTO tblOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(tblInvoiceTO.DealerOrgId);
                        if (tblOrganizationTO == null)
                        {
                            resultMessage.DefaultBehaviour("tblOrganizationTO is null in send WhatsApp Msg");
                            return resultMessage;
                        }
                     
                        TblOrganizationTO tblSalesOrganizationTO = _iTblOrganizationBL.SelectTblOrganizationTOForCNF(tblInvoiceTO.DistributorOrgId);
                        if (tblSalesOrganizationTO == null)
                        {
                            resultMessage.DefaultBehaviour("tblSalesOrganizationTO is null in send WhatsApp Msg");
                            return resultMessage;
                        }
                        double invoiceCnt = 0;

                        
                        List<TblInvoiceItemDetailsTO> tblInvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                        if (tblInvoiceItemDetailsTOList != null && tblInvoiceItemDetailsTOList.Count > 0)
                        {
                            invoiceCnt = tblInvoiceItemDetailsTOList.Where(w => w.LoadingSlipExtId > 0).Sum(w => w.InvoiceQty);
                        }
                        TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipBL.SelectTblLoadingSlipTO(tblInvoiceTO.LoadingSlipId);

                        string whatsAppMsgTOStr = "";
                        TblConfigParamsTO tblConfigParamsTOForRate = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.WHATS_APP_SEND_MESSAGE_REQUEST_JSON_FOR_INVOICE_MSG_SEND);
                        if (tblConfigParamsTOForRate != null && !String.IsNullOrEmpty(tblConfigParamsTOForRate.ConfigParamVal))
                        {
                            List<TblOrganizationTO> tblOrganizationTOTemplist = new List<TblOrganizationTO>();
                            if (tblOrganizationTO != null && tblSalesOrganizationTO != null)
                            {
                                tblOrganizationTOTemplist.Add(tblOrganizationTO);
                                tblOrganizationTOTemplist.Add(tblSalesOrganizationTO);
                            }
                            if (tblOrganizationTOTemplist != null && tblOrganizationTOTemplist.Count > 0)
                            {
                                for (int k = 0; k < tblOrganizationTOTemplist.Count; k++)
                                {
                                    TblOrganizationTO TblOrganizationTOTemp = tblOrganizationTOTemplist[k];
                                    whatsAppMsgTOStr = tblConfigParamsTOForRate.ConfigParamVal;
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@mobileNo", TblOrganizationTOTemp.RegisteredMobileNos);
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment1", tblInvoiceTO.InvoiceNo);
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment2", tblInvoiceTO.CreatedOnStr);
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment3", invoiceCnt.ToString());
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment4", tblInvoiceTO.VehicleNo);
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment5", tblInvoiceTO.GrandTotal.ToString());

                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@url", uploadedFileName);
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@fileName", fileName1);
                                    TblConfigParamsTO WhatsAppConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.WHATS_APP_SEND_MESSAGE_INTEGRATION_API, conn, tran);
                                    String WhatsAppIntegrationAPI = "";
                                    string WhatsAppMsgRequestHeaderStr = "";
                                    string WhatsAppKey = "";
                                    if (WhatsAppConfTO != null && !String.IsNullOrEmpty(WhatsAppConfTO.ConfigParamVal))
                                    {
                                        TblConfigParamsTO WhatsAppHeaderConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.WHATS_APP_SEND_MESSAGE_REQUEST_HEADER_JSON, conn, tran);
                                        if (WhatsAppHeaderConfTO != null && !String.IsNullOrEmpty(WhatsAppHeaderConfTO.ConfigParamVal))
                                        {
                                            WhatsAppMsgRequestHeaderStr = WhatsAppHeaderConfTO.ConfigParamVal;
                                            TblConfigParamsTO WhatsAppKeyConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.WHATS_APP_API_KEY, conn, tran);
                                            if (WhatsAppKeyConfTO != null && !String.IsNullOrEmpty(WhatsAppKeyConfTO.ConfigParamVal))
                                            {
                                                WhatsAppKey = WhatsAppKeyConfTO.ConfigParamVal;
                                            }
                                            if (!String.IsNullOrEmpty(WhatsAppMsgRequestHeaderStr) && !String.IsNullOrEmpty(WhatsAppKey))
                                            {
                                                WhatsAppMsgRequestHeaderStr = WhatsAppMsgRequestHeaderStr.Replace("@API_KEY", WhatsAppKey);
                                            }

                                        }
                                        WhatsAppIntegrationAPI = WhatsAppConfTO.ConfigParamVal;
                                        _iCommon.SendWhatsAppMsg(whatsAppMsgTOStr, WhatsAppIntegrationAPI, WhatsAppMsgRequestHeaderStr);
                                    }
                                }
                            }
                            //tran = conn.BeginTransaction();
                            //TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                            //tblAlertInstanceTO.EffectiveFromDate = _iCommon.ServerDateTime;
                            //tblAlertInstanceTO.EffectiveToDate = _iCommon.ServerDateTime;
                            //tblAlertInstanceTO.AlertComment = whatsAppMsgTOStr;
                            //tblAlertInstanceTO.RaisedOn = _iCommon.ServerDateTime;
                            //tblAlertInstanceTO.RaisedBy = tblInvoiceTO.UpdatedBy;
                            //tblAlertInstanceTO.SourceEntityId = tblInvoiceTO.IdInvoice;
                            //tblAlertInstanceTO.SourceDisplayId = NotificationConstants.NotificationsE.Send_Invoice_Msg.ToString();
                            //tblAlertInstanceTO.IsAutoReset = 1;
                            //tblAlertInstanceTO.AlertAction = NotificationConstants.NotificationsE.Send_Invoice_Msg.ToString();
                            //tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.Send_Invoice_Msg;
                            //tblAlertInstanceTO.IsActive = 1;
                            //tblAlertInstanceTO.WhatsAppComment = whatsAppMsgTOStr;
                            //int result = _iTblAlertInstanceBL.InsertTblAlertInstance(tblAlertInstanceTO, conn, tran);
                            //if (result != 1)
                            //{
                            //    tran.Rollback();
                            //    resultMessage.DefaultBehaviour("Error While InsertTblAlertInstance");
                            //    return resultMessage;
                            //}
                            //tblInvoiceTO.IsSendWhatsAppMsg = true;
                            //result = _iTblInvoiceDAO.UpdateWhatsAppMsgSendInvoiceNo(tblInvoiceTO, conn, tran);
                            //if (result != 1)
                            //{
                            //    tran.Rollback();
                            //    resultMessage.DefaultBehaviour("Error While UpdateWhatsAppMsgSendInvoiceNo");
                            //    return resultMessage;
                            //}
                            //else
                            //    tran.Commit();

                            //resultMessage.DefaultSuccessBehaviour();
                            //return resultMessage;


                        }

                        TblConfigParamsTO tblConfigParamsTOForRateForMetaroll = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.WHATS_APP_SEND_MESSAGE_REQUEST_JSON_FOR_INVOICE_MSG_SEND_FOR_METAROLL);
                        if (tblConfigParamsTOForRateForMetaroll != null && !String.IsNullOrEmpty(tblConfigParamsTOForRateForMetaroll.ConfigParamVal))
                        {
                            whatsAppMsgTOStr = tblConfigParamsTOForRateForMetaroll.ConfigParamVal;
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@mobileNo", tblOrganizationTO.RegisteredMobileNos);
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment1", tblInvoiceTO.DealerName);
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment2", tblInvoiceTO.InvoiceNo);
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment3", tblInvoiceTO.CreatedOnStr.ToString ());
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment4", tblInvoiceTO.GrandTotal .ToString ());
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment5", Convert.ToString( Math .Round( invoiceCnt,3)));
                            DateTime eWayBillExpiry=new DateTime();
                            string  response = _iTblInvoiceDAO.SelectresponseForPhotoInReport(invoiceId,(int)EInvoiceAPIE.GENERATE_EWAYBILL);
                            if (!String.IsNullOrEmpty(response))
                            {
                                JObject json = JObject.Parse(response);

                                if (json.ContainsKey("data"))
                                {
                                    JObject jsonData = JObject.Parse(json["data"].ToString());

                                    if (jsonData.ContainsKey("EwbValidTill"))
                                    {
                                        eWayBillExpiry = (DateTime)jsonData["EwbValidTill"];
                                        whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment6", eWayBillExpiry.ToString(("MM-dd-yyyy")));
                                    }
                                    
                                }
                            }
                            else
                                whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment6", "--");
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment7", tblInvoiceTO.VehicleNo);
                            if (tblLoadingSlipTO != null)
                            {
                                if (!string.IsNullOrEmpty(tblLoadingSlipTO.DriverName))
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment8", tblLoadingSlipTO.DriverName);
                                else
                                    whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment8", "--");
                            }
                            if(!string.IsNullOrEmpty(tblInvoiceTO.LrNumber))
                                 whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment9", tblInvoiceTO.LrNumber);
                            else
                                whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment9", "--");

                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@comment", tblInvoiceTO.TransporterName);

                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@url", uploadedFileName);
                            whatsAppMsgTOStr = whatsAppMsgTOStr.Replace("@fileName", fileName1);
                            TblConfigParamsTO WhatsAppConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.WHATS_APP_SEND_MESSAGE_INTEGRATION_API, conn, tran);
                            String WhatsAppIntegrationAPI = "";
                            string WhatsAppMsgRequestHeaderStr = "";
                            string WhatsAppKey = "";
                            if (WhatsAppConfTO != null && !String.IsNullOrEmpty(WhatsAppConfTO.ConfigParamVal))
                            {
                                TblConfigParamsTO WhatsAppHeaderConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.WHATS_APP_SEND_MESSAGE_REQUEST_HEADER_JSON, conn, tran);
                                if (WhatsAppHeaderConfTO != null && !String.IsNullOrEmpty(WhatsAppHeaderConfTO.ConfigParamVal))
                                {
                                    WhatsAppMsgRequestHeaderStr = WhatsAppHeaderConfTO.ConfigParamVal;
                                    TblConfigParamsTO WhatsAppKeyConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.WHATS_APP_API_KEY, conn, tran);
                                    if (WhatsAppKeyConfTO != null && !String.IsNullOrEmpty(WhatsAppKeyConfTO.ConfigParamVal))
                                    {
                                        WhatsAppKey = WhatsAppKeyConfTO.ConfigParamVal;
                                    }
                                    if (!String.IsNullOrEmpty(WhatsAppMsgRequestHeaderStr) && !String.IsNullOrEmpty(WhatsAppKey))
                                    {
                                        WhatsAppMsgRequestHeaderStr = WhatsAppMsgRequestHeaderStr.Replace("@API_KEY", WhatsAppKey);
                                    }

                                }
                                WhatsAppIntegrationAPI = WhatsAppConfTO.ConfigParamVal;
                                _iCommon.SendWhatsAppMsg(whatsAppMsgTOStr, WhatsAppIntegrationAPI, WhatsAppMsgRequestHeaderStr);
                            }
                        }
                        tran = conn.BeginTransaction();
                        TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
                        tblAlertInstanceTO.EffectiveFromDate = _iCommon.ServerDateTime;
                        tblAlertInstanceTO.EffectiveToDate = _iCommon.ServerDateTime;
                        tblAlertInstanceTO.AlertComment = whatsAppMsgTOStr;
                        tblAlertInstanceTO.RaisedOn = _iCommon.ServerDateTime;
                        tblAlertInstanceTO.RaisedBy = tblInvoiceTO.UpdatedBy;
                        tblAlertInstanceTO.SourceEntityId = tblInvoiceTO.IdInvoice;
                        tblAlertInstanceTO.SourceDisplayId = NotificationConstants.NotificationsE.Send_Invoice_Msg.ToString();
                        tblAlertInstanceTO.IsAutoReset = 1;
                        tblAlertInstanceTO.AlertAction = NotificationConstants.NotificationsE.Send_Invoice_Msg.ToString();
                        tblAlertInstanceTO.AlertDefinitionId = (int)NotificationConstants.NotificationsE.Send_Invoice_Msg;
                        tblAlertInstanceTO.IsActive = 1;
                        tblAlertInstanceTO.WhatsAppComment = whatsAppMsgTOStr;
                        result = _iTblAlertInstanceBL.InsertTblAlertInstance(tblAlertInstanceTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While InsertTblAlertInstance");
                            return resultMessage;
                        }
                        tblInvoiceTO.IsSendWhatsAppMsg = true;
                        result = _iTblInvoiceDAO.UpdateWhatsAppMsgSendInvoiceNo(tblInvoiceTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Error While UpdateWhatsAppMsgSendInvoiceNo");
                            return resultMessage;
                        }
                        else
                            tran.Commit();

                        resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;

                    }
                }
            }
            catch (Exception ex)
            {
                resultMSg.DefaultExceptionBehaviour(ex, "");
                return resultMSg;
            }
            finally
            {
                conn.Close();
            }
            return resultMessage;
        }

        public ResultMessage PrintReportV2(Int32 invoiceId, Boolean isPrinted = false, Boolean isSendEmailForInvoice = false, Boolean isFileDelete = true)
        {
            ResultMessage resultMessage = new ResultMessage();
            String response = String.Empty;
            String signedQRCode = String.Empty;
            Int32 apiId = (int)EInvoiceAPIE.GENERATE_EINVOICE;
            byte[] PhotoCodeInBytes = null;
            try
            {

                TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTOWithDetails(invoiceId);
                TblLoadingSlipTO TblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetailsByInvoice(invoiceId);
                List<TblInvoiceAddressTO> invoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);
                DataSet printDataSet = new DataSet();

                //headerDT
                DataTable headerDT = new DataTable();
                DataTable addressDT = new DataTable();
                DataTable invoiceDT = new DataTable();
                DataTable invoiceItemDT = new DataTable();
                DataTable itemFooterDetailsDT = new DataTable();
                DataTable commercialDT = new DataTable();
                DataTable hsnItemTaxDT = new DataTable();
                DataTable qrCodeDT = new DataTable();
                // DataTable shippingAddressDT = new DataTable();
                //Aniket [1-02-2019] added to create multiple copy of tax invoice
                DataTable multipleInvoiceCopyDT = new DataTable();
                headerDT.TableName = "headerDT";
                invoiceDT.TableName = "invoiceDT";
                addressDT.TableName = "addressDT";
                invoiceItemDT.TableName = "invoiceItemDT";
                itemFooterDetailsDT.TableName = "itemFooterDetailsDT";
                hsnItemTaxDT.TableName = "hsnItemTaxDT";
                // shippingAddressDT.TableName = "shippingAddressDT";
                multipleInvoiceCopyDT.TableName = "multipleInvoiceCopyDT";
                qrCodeDT.TableName = "QRCodeDT";
                //HeaderDT 
                multipleInvoiceCopyDT.Columns.Add("idInvoiceCopy");
                multipleInvoiceCopyDT.Columns.Add("invoiceCopyName");

                //Aniket [13-02-2019] to display payment term option on print invoice
                string paymentTermAllCommaSeparated = "";
                List<DropDownTO> multipleInvoiceCopyList = _iDimensionBL.SelectInvoiceCopyList();

                if (multipleInvoiceCopyList != null)
                {
                    for (int i = 0; i < multipleInvoiceCopyList.Count; i++)
                    {
                        DropDownTO multipleInvoiceCopyTO = multipleInvoiceCopyList[i];
                        multipleInvoiceCopyDT.Rows.Add();
                        Int32 invoiceCopyDTCount = multipleInvoiceCopyDT.Rows.Count - 1;
                        multipleInvoiceCopyDT.Rows[invoiceCopyDTCount]["idInvoiceCopy"] = multipleInvoiceCopyTO.Value;
                        multipleInvoiceCopyDT.Rows[invoiceCopyDTCount]["invoiceCopyName"] = multipleInvoiceCopyTO.Text;

                    }
                }

                int defaultCompOrgId = 0;

                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (configParamsTO != null)
                    {
                        defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
                    }
                }
                else
                {
                    defaultCompOrgId = tblInvoiceTO.InvFromOrgId;
                }
                TblOrganizationTO organizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(defaultCompOrgId);

                //Aniket [06-03-2019] added to check whether Math.Round() function should include in tax calculation or not
                int isMathRoundoff = 0;
                TblConfigParamsTO tblconfigParamForMathRound = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_TAX_ON_PRINT_INVOICE);
                if (tblconfigParamForMathRound != null)
                {
                    if (tblconfigParamForMathRound.ConfigParamVal == "1")
                    {
                        isMathRoundoff = 1;
                    }
                }

                qrCodeDT.Columns.Add("QRCode", typeof(System.Byte[]));
                qrCodeDT.Columns.Add("QRCodeForPrint", typeof(System.Byte[]));

                qrCodeDT.Rows.Add();
                string AckNo = "";
                response = _iTblInvoiceDAO.SelectresponseForPhotoInReport(invoiceId, apiId);
                if (!String.IsNullOrEmpty(response))
                {
                    JObject json = JObject.Parse(response);

                    if (json.ContainsKey("data"))
                    {
                        JObject jsonData = JObject.Parse(json["data"].ToString());

                        if (jsonData.ContainsKey("SignedQRCode"))
                        {
                            signedQRCode = (string)jsonData["SignedQRCode"];
                        }
                        if (jsonData.ContainsKey("AckNo"))
                        {
                            AckNo = (string)jsonData["AckNo"];
                        }
                    }
                }

                if (!String.IsNullOrEmpty(signedQRCode))
                {
                    PhotoCodeInBytes = _iCommon.convertQRStringToByteArray(signedQRCode);
                }

                if (PhotoCodeInBytes != null)
                    qrCodeDT.Rows[0]["QRCode"] = PhotoCodeInBytes;

                //Reshma Added For QR Code Image
                String QrCodeImageFilePath = "";
                TblConfigParamsTO tblconfigParamForQRCodeImage = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.Invoice_payment_QR_CODE_Image_file_Path);
                if (tblconfigParamForQRCodeImage != null)
                {
                    if (!string.IsNullOrEmpty(tblconfigParamForQRCodeImage.ConfigParamVal))
                    {
                        QrCodeImageFilePath = tblconfigParamForQRCodeImage.ConfigParamVal;
                    }
                }
                byte[] imageByteArray = null;
                if (!string.IsNullOrEmpty(QrCodeImageFilePath))
                {
                    //byte[] imageByteArray = null;
                    FileStream fileStream = new FileStream(QrCodeImageFilePath, FileMode.Open, FileAccess.Read);
                    using (BinaryReader reader = new BinaryReader(fileStream))
                    {
                        imageByteArray = new byte[reader.BaseStream.Length];
                        for (int i = 0; i < reader.BaseStream.Length; i++)
                            imageByteArray[i] = reader.ReadByte();
                    }
                    if (imageByteArray != null)
                        qrCodeDT.Rows[0]["QRCodeForPrint"] = imageByteArray;
                }

                //HeaderDT 
                //headerDT.Columns.Add("orgFirmName");
                invoiceDT.Columns.Add("orgVillageNm");
                invoiceDT.Columns.Add("orgPhoneNo");
                invoiceDT.Columns.Add("orgFaxNo");
                invoiceDT.Columns.Add("orgEmailAddr");
                invoiceDT.Columns.Add("orgWebsite");
                invoiceDT.Columns.Add("orgAddr");
                invoiceDT.Columns.Add("orgCinNo");
                invoiceDT.Columns.Add("orgGstinNo");
                invoiceDT.Columns.Add("orgPanNo");   //Twicie
                invoiceDT.Columns.Add("orgState");
                invoiceDT.Columns.Add("orgStateCode");

                invoiceDT.Columns.Add("plotNo");
                invoiceDT.Columns.Add("streetName");

                invoiceDT.Columns.Add("areaName");
                invoiceDT.Columns.Add("district");
                invoiceDT.Columns.Add("pinCode");


                invoiceDT.Columns.Add("orgFirmName");
                invoiceDT.Columns.Add("hsnNo");
                invoiceDT.Columns.Add("panNo");
                invoiceDT.Columns.Add("paymentTerm");
                invoiceDT.Columns.Add("poNo");
                invoiceDT.Columns.Add("poDateStr");

                headerDT.Columns.Add("poNo");
                headerDT.Columns.Add("poDateStr");
                invoiceDT.Columns.Add("DeliveryNoteNo");
                invoiceDT.Columns.Add("DispatchDocNo");

                invoiceDT.Columns.Add("TotalTaxAmt", typeof(double));
                invoiceDT.Columns.Add("TotalTaxAmtWordStr");
                //chetan[14-feb-2020] added
                invoiceDT.Columns.Add("BookingCDPct", typeof(double));
                invoiceDT.Columns.Add("BookingBasicRate", typeof(double));
                headerDT.Columns.Add("AckNo");
                invoiceDT.Columns.Add("AckNo");
                headerDT.Columns.Add("BrokerName");
                invoiceDT.Columns.Add("BrokerName");
                invoiceDT.Columns.Add("freightCategory");
                headerDT.Columns.Add("freightCategory");

                TblAddressTO tblAddressTO = _iTblAddressBL.SelectOrgAddressWrtAddrType(organizationTO.IdOrganization, Constants.AddressTypeE.OFFICE_ADDRESS);
                List<DropDownTO> stateList = _iDimensionBL.SelectStatesForDropDown(0);
                if (organizationTO != null)
                {
                    headerDT.Rows.Add();
                    invoiceDT.Rows.Add();
                    //headerDT.Rows[0]["orgFirmName"] = organizationTO.FirmName;
                    invoiceDT.Rows[0]["orgFirmName"] = organizationTO.FirmName;

                    invoiceDT.Rows[0]["orgPhoneNo"] = organizationTO.PhoneNo;
                    invoiceDT.Rows[0]["orgFaxNo"] = organizationTO.FaxNo;
                    invoiceDT.Rows[0]["orgWebsite"] = organizationTO.Website;
                    invoiceDT.Rows[0]["orgEmailAddr"] = organizationTO.EmailAddr;
                }
                //if (string.IsNullOrEmpty(AckNo))//Reshma[24-06-2022] Added Acknowledgement Number DT creation to print on simpli invoice
                headerDT.Rows[0]["AckNo"] = AckNo;
                invoiceDT.Rows[0]["AckNo"] = AckNo;

                //chetan[14-feb-2020]
                TblBookingsTO tblBookingsTO = _iTblBookingsBL.SelectBookingsDetailsFromInVoiceId(tblInvoiceTO.IdInvoice);
                if (tblBookingsTO != null)
                {
                    invoiceDT.Rows[0]["BookingCDPct"] = tblBookingsTO.CdStructure;
                    invoiceDT.Rows[0]["BookingBasicRate"] = tblBookingsTO.BookingRate;
                }
                List<TblPaymentTermsForBookingTO> tblPaymentTermsForBookingTOList = _iTblPaymentTermsForBookingBL.SelectAllTblPaymentTermsForBookingFromBookingId(0, invoiceId);
                if (tblPaymentTermsForBookingTOList != null)
                {
                    foreach (var item in tblPaymentTermsForBookingTOList)
                    {

                        invoiceDT.Columns.Add(item.PaymentTerm);
                        //headerDT.Columns.Add(item.PaymentTerm);

                        if (item.PaymentTermOptionList != null && item.PaymentTermOptionList.Count > 0)
                        {
                            foreach (var x in item.PaymentTermOptionList)
                            {
                                if (x.IsSelected == 1)
                                {

                                    String tempPayment = x.PaymentTermOption;
                                    if (x.IsDescriptive == 1)
                                    {
                                        tempPayment = x.PaymentTermsDescription;
                                    }


                                    paymentTermAllCommaSeparated += tempPayment + ",";

                                    invoiceDT.Rows[0][item.PaymentTerm] = tempPayment;
                                    //headerDT.Rows[0][item.PaymentTerm] = x.PaymentTermOption;
                                }
                            }

                        }


                    }
                }

                if (!String.IsNullOrEmpty(paymentTermAllCommaSeparated))
                {
                    paymentTermAllCommaSeparated = paymentTermAllCommaSeparated.TrimEnd(',');
                }

                if (tblAddressTO != null)
                {
                    String orgAddrStr = String.Empty;
                    if (!String.IsNullOrEmpty(tblAddressTO.PlotNo))
                    {
                        orgAddrStr += tblAddressTO.PlotNo;
                        invoiceDT.Rows[0]["plotNo"] = tblAddressTO.PlotNo;
                    }

                    if (!String.IsNullOrEmpty(tblAddressTO.StreetName))
                    {
                        orgAddrStr += " " + tblAddressTO.StreetName;
                        invoiceDT.Rows[0]["streetName"] = tblAddressTO.StreetName;
                    }

                    if (!String.IsNullOrEmpty(tblAddressTO.AreaName))
                    {
                        orgAddrStr += " " + tblAddressTO.AreaName;
                        invoiceDT.Rows[0]["areaName"] = tblAddressTO.AreaName;
                    }
                    if (!String.IsNullOrEmpty(tblAddressTO.DistrictName))
                    {
                        orgAddrStr += " " + tblAddressTO.DistrictName;
                        invoiceDT.Rows[0]["district"] = tblAddressTO.DistrictName;

                    }
                    if (tblAddressTO.Pincode > 0)
                    {
                        orgAddrStr += "-" + tblAddressTO.Pincode;
                        invoiceDT.Rows[0]["pinCode"] = tblAddressTO.Pincode;

                    }
                    invoiceDT.Rows[0]["orgVillageNm"] = tblAddressTO.VillageName + "-" + tblAddressTO.Pincode;
                    invoiceDT.Rows[0]["orgAddr"] = orgAddrStr;
                    invoiceDT.Rows[0]["orgState"] = tblAddressTO.StateName;

                    if (stateList != null && stateList.Count > 0)
                    {
                        DropDownTO stateTO = stateList.Where(ele => ele.Value == tblAddressTO.StateId).FirstOrDefault();
                        if (stateTO != null)
                        {

                            invoiceDT.Rows[0]["orgStateCode"] = stateTO.Tag;
                        }
                    }



                }
                List<TblOrgLicenseDtlTO> orgLicenseList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(defaultCompOrgId);

                if (orgLicenseList != null && orgLicenseList.Count > 0)
                {

                    //CIN Number
                    var cinNo = orgLicenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.CIN_NO).FirstOrDefault();
                    if (cinNo != null)
                    {
                        invoiceDT.Rows[invoiceDT.Rows.Count - 1]["orgCinNo"] = cinNo.LicenseValue;
                    }
                    //GSTIN Number
                    var gstinNo = orgLicenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (gstinNo != null)
                    {
                        invoiceDT.Rows[invoiceDT.Rows.Count - 1]["orgGstinNo"] = gstinNo.LicenseValue;
                    }
                    //PAN Number
                    var panNo = orgLicenseList.Where(a => a.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    if (panNo != null)
                    {
                        invoiceDT.Rows[invoiceDT.Rows.Count - 1]["orgPanNo"] = panNo.LicenseValue;
                        invoiceDT.Rows[0]["panNo"] = panNo.LicenseValue;

                    }
                }

                //InvoiceDT

                if (tblInvoiceTO != null)
                {

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        tblInvoiceTO.VehicleNo = tblInvoiceTO.VehicleNo.ToUpper();
                    }

                    invoiceDT.Columns.Add("invoiceNo");
                    invoiceDT.Columns.Add("invoiceDateStr");
                    //headerDT.Columns.Add("deliveryLocation");
                    invoiceDT.Columns.Add("EWayBillNo"); //Aniket [26-6-2019]

                    invoiceDT.Rows[0]["invoiceNo"] = tblInvoiceTO.InvoiceNo;
                    invoiceDT.Rows[0]["invoiceDateStr"] = tblInvoiceTO.InvoiceDateStr;
                    if (!string.IsNullOrEmpty(tblInvoiceTO.ElectronicRefNo))
                        invoiceDT.Rows[0]["EWayBillNo"] = tblInvoiceTO.ElectronicRefNo;

                    //Dhananjay [25-11-2020]
                    invoiceDT.Columns.Add("IrnNo");
                    if (!string.IsNullOrEmpty(tblInvoiceTO.IrnNo))
                        invoiceDT.Rows[0]["IrnNo"] = tblInvoiceTO.IrnNo;

                    addressDT.Columns.Add("poNo");
                    addressDT.Columns.Add("poDateStr");
                    addressDT.Columns.Add("electronicRefNo");


                    commercialDT = getCommercialDT(tblInvoiceTO); //for SRJ
                    hsnItemTaxDT = getHsnItemTaxDT(tblInvoiceTO); //for Parameshwar
                    commercialDT.TableName = "commercialDT";



                    invoiceDT.Columns.Add("discountAmt", typeof(double));
                    invoiceDT.Columns.Add("discountAmtStr");

                    invoiceDT.Columns.Add("freightAmt", typeof(double));
                    invoiceDT.Columns.Add("pfAmt", typeof(double));
                    invoiceDT.Columns.Add("cessAmt", typeof(double));
                    invoiceDT.Columns.Add("afterCessAmt", typeof(double));
                    invoiceDT.Columns.Add("insuranceAmt", typeof(double));


                    invoiceDT.Columns.Add("taxableAmt", typeof(double));
                    invoiceDT.Columns.Add("taxableAmtStr");
                    invoiceDT.Columns.Add("cgstAmt", typeof(double));
                    invoiceDT.Columns.Add("sgstAmt", typeof(double));
                    invoiceDT.Columns.Add("igstAmt", typeof(double));
                    invoiceDT.Columns.Add("grandTotal", typeof(double));

                    invoiceDT.Columns.Add("cgstTotalStr");//r
                    invoiceDT.Columns.Add("sgstTotalStr");//r
                    invoiceDT.Columns.Add("igstTotalStr");//r
                    invoiceDT.Columns.Add("grandTotalStr");//r

                    invoiceDT.Columns.Add("grossWeight", typeof(double));
                    invoiceDT.Columns.Add("tareWeight", typeof(double));
                    invoiceDT.Columns.Add("netWeight", typeof(double));


                    //headerDT.Columns.Add("vehicleNo");
                    //headerDT.Columns.Add("lrNumber");
                    invoiceDT.Columns.Add("vehicleNo");
                    invoiceDT.Columns.Add("transporterName");
                    invoiceDT.Columns.Add("Narration");
                    invoiceDT.Columns.Add("deliveryLocation");
                    invoiceDT.Columns.Add("lrNumber");
                    invoiceDT.Columns.Add("disPer", typeof(double));
                    invoiceDT.Columns.Add("roundOff", typeof(double));
                    invoiceDT.Columns.Add("taxTotal", typeof(double));
                    invoiceDT.Columns.Add("totalQty", typeof(double));
                    invoiceDT.Columns.Add("totalBundles");
                    invoiceDT.Columns.Add("totalBasicAmt", typeof(double));
                    invoiceDT.Columns.Add("bankName");
                    invoiceDT.Columns.Add("accountNo");
                    invoiceDT.Columns.Add("branchName");
                    invoiceDT.Columns.Add("ifscCode");
                    invoiceDT.Columns.Add("taxTotalStr");//r
                    invoiceDT.Columns.Add("declaration");//r
                    invoiceDT.Columns.Add("grossWtTakenDate");
                    invoiceDT.Columns.Add("preparationDate");

                    Double totalTaxAmt, cgstAmt, sgstAmt, igstAmt = 0;

                    cgstAmt = Math.Round(tblInvoiceTO.CgstAmt, 2);
                    sgstAmt = Math.Round(tblInvoiceTO.SgstAmt, 2);
                    igstAmt = Math.Round(tblInvoiceTO.IgstAmt, 2);

                    totalTaxAmt = Math.Round(cgstAmt + sgstAmt + igstAmt, 2);

                    invoiceDT.Rows[0]["TotalTaxAmt"] = totalTaxAmt;
                    invoiceDT.Rows[0]["TotalTaxAmtWordStr"] = currencyTowords(totalTaxAmt, tblInvoiceTO.CurrencyId); ;

                    invoiceDT.Rows[0]["DeliveryNoteNo"] = tblInvoiceTO.DeliveryNoteNo;
                    invoiceDT.Rows[0]["DispatchDocNo"] = tblInvoiceTO.DispatchDocNo;

                    //if (isMathRoundoff == 1)
                    if (isMathRoundoff == 1)  //Not applicable as each value will round off upto 2
                    {
                        invoiceDT.Rows[0]["discountAmt"] = tblInvoiceTO.DiscountAmt;
                        invoiceDT.Rows[0]["discountAmtStr"] = currencyTowords(tblInvoiceTO.DiscountAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["taxableAmt"] = tblInvoiceTO.TaxableAmt;
                        invoiceDT.Rows[0]["taxableAmtStr"] = currencyTowords(tblInvoiceTO.TaxableAmt, tblInvoiceTO.CurrencyId);


                        invoiceDT.Rows[0]["cgstAmt"] = tblInvoiceTO.CgstAmt;
                        invoiceDT.Rows[0]["cgstTotalStr"] = currencyTowords(tblInvoiceTO.CgstAmt, tblInvoiceTO.CurrencyId);
                        invoiceDT.Rows[0]["sgstAmt"] = tblInvoiceTO.SgstAmt;
                        invoiceDT.Rows[0]["sgstTotalStr"] = currencyTowords(tblInvoiceTO.SgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["igstAmt"] = tblInvoiceTO.IgstAmt;

                        invoiceDT.Rows[0]["igstTotalStr"] = currencyTowords(tblInvoiceTO.IgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["grandTotal"] = tblInvoiceTO.GrandTotal;
                        invoiceDT.Rows[0]["grandTotalStr"] = currencyTowords(tblInvoiceTO.GrandTotal, tblInvoiceTO.CurrencyId);


                        //invoiceDT.Rows[0]["grossWeight"] = tblInvoiceTO.GrossWeight / 1000;
                        //invoiceDT.Rows[0]["tareWeight"] = tblInvoiceTO.TareWeight / 1000;
                        //invoiceDT.Rows[0]["netWeight"] = tblInvoiceTO.NetWeight / 1000;
                        //if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                        //{
                        //    headerDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //    invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //}

                        //invoiceDT.Rows[0]["transporterName"] = tblInvoiceTO.TransporterName;
                        //invoiceDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //headerDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //invoiceDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        //headerDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        invoiceDT.Rows[0]["disPer"] = getDiscountPerct(tblInvoiceTO);
                        invoiceDT.Rows[0]["roundOff"] = tblInvoiceTO.RoundOffAmt;
                    }
                    else
                    {
                        invoiceDT.Rows[0]["discountAmt"] = Math.Round(tblInvoiceTO.DiscountAmt, 2);
                        invoiceDT.Rows[0]["discountAmtStr"] = currencyTowords(tblInvoiceTO.DiscountAmt, tblInvoiceTO.CurrencyId);
                        invoiceDT.Rows[0]["taxableAmt"] = Math.Round(tblInvoiceTO.TaxableAmt, 2);
                        invoiceDT.Rows[0]["taxableAmtStr"] = currencyTowords(tblInvoiceTO.TaxableAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["cgstAmt"] = Math.Round(tblInvoiceTO.CgstAmt, 2);
                        invoiceDT.Rows[0]["cgstTotalStr"] = currencyTowords(tblInvoiceTO.CgstAmt, tblInvoiceTO.CurrencyId);
                        invoiceDT.Rows[0]["sgstAmt"] = Math.Round(tblInvoiceTO.SgstAmt, 2);
                        invoiceDT.Rows[0]["sgstTotalStr"] = currencyTowords(tblInvoiceTO.SgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["igstAmt"] = Math.Round(tblInvoiceTO.IgstAmt, 2);

                        invoiceDT.Rows[0]["igstTotalStr"] = currencyTowords(tblInvoiceTO.IgstAmt, tblInvoiceTO.CurrencyId);

                        invoiceDT.Rows[0]["grandTotal"] = Math.Round(tblInvoiceTO.GrandTotal, 2);
                        invoiceDT.Rows[0]["grandTotalStr"] = currencyTowords(tblInvoiceTO.GrandTotal, tblInvoiceTO.CurrencyId);


                        //invoiceDT.Rows[0]["grossWeight"] = Math.Round(tblInvoiceTO.GrossWeight / 1000, 3);
                        //invoiceDT.Rows[0]["tareWeight"] = Math.Round(tblInvoiceTO.TareWeight / 1000, 3);
                        //invoiceDT.Rows[0]["netWeight"] = Math.Round(tblInvoiceTO.NetWeight / 1000, 3);
                        //if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                        //{
                        //    headerDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //    invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                        //}
                        //invoiceDT.Rows[0]["transporterName"] = tblInvoiceTO.TransporterName;
                        //invoiceDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //headerDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                        //invoiceDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        //headerDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;
                        invoiceDT.Rows[0]["disPer"] = Math.Round(getDiscountPerct(tblInvoiceTO), 2);
                        invoiceDT.Rows[0]["roundOff"] = Math.Round(tblInvoiceTO.RoundOffAmt, 2);

                    }

                    invoiceDT.Rows[0]["grossWeight"] = Math.Round(tblInvoiceTO.GrossWeight / 1000, 3);
                    invoiceDT.Rows[0]["tareWeight"] = Math.Round(tblInvoiceTO.TareWeight / 1000, 3);
                    invoiceDT.Rows[0]["netWeight"] = Math.Round(tblInvoiceTO.NetWeight / 1000, 3);

                    invoiceDT.Rows[0]["transporterName"] = tblInvoiceTO.TransporterName;
                    invoiceDT.Rows[0]["Narration"] = tblInvoiceTO.Narration;

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        invoiceDT.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                    }
                    invoiceDT.Rows[0]["deliveryLocation"] = tblInvoiceTO.DeliveryLocation;
                    invoiceDT.Rows[0]["lrNumber"] = tblInvoiceTO.LrNumber;

                    //Aniket [8-02-2019] added
                    //headerDT.Columns.Add("preparationDate");
                    if (tblInvoiceTO.GrossWtTakenDate != DateTime.MinValue)
                        invoiceDT.Rows[0]["grossWtTakenDate"] = tblInvoiceTO.GrossWtDateStr;
                    if (tblInvoiceTO.PreparationDate != DateTime.MinValue)
                    {
                        invoiceDT.Rows[0]["preparationDate"] = tblInvoiceTO.PreparationDateStr;
                        //headerDT.Rows[0]["preparationDate"] = tblInvoiceTO.PreparationDateStr;
                    }
                    //Aniket [8-02-2019] added
                    //headerDT.Columns.Add("paymentTerm");
                    if (!string.IsNullOrEmpty(paymentTermAllCommaSeparated))
                    {
                        invoiceDT.Rows[0]["paymentTerm"] = paymentTermAllCommaSeparated;
                        //headerDT.Rows[0]["paymentTerm"] = paymentTermAllCommaSeparated;
                    }
                    Double taxTotal = 0;
                    if (tblInvoiceTO.CgstAmt > 0 && tblInvoiceTO.SgstAmt > 0)
                    {
                        taxTotal = tblInvoiceTO.CgstAmt + tblInvoiceTO.SgstAmt;
                    }
                    else if (tblInvoiceTO.IgstAmt > 0)
                    {
                        taxTotal = tblInvoiceTO.IgstAmt;
                    }

                    if (isMathRoundoff == 1)
                    {
                        invoiceDT.Rows[0]["taxTotal"] = taxTotal;
                        invoiceDT.Rows[0]["taxTotalStr"] = currencyTowords(taxTotal, tblInvoiceTO.CurrencyId);
                    }
                    else
                    {
                        invoiceDT.Rows[0]["taxTotal"] = Math.Round(taxTotal, 2);
                        invoiceDT.Rows[0]["taxTotalStr"] = currencyTowords(Math.Round(taxTotal, 2), tblInvoiceTO.CurrencyId);
                    }


                    //invoiceItemDT

                    //Int32 finalItemCount = 15;
                    if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                    {
                        tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                        List<TblInvoiceItemDetailsTO> invoiceItemlist = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == 0).ToList();
                        invoiceItemDT.Columns.Add("srNo");
                        invoiceItemDT.Columns.Add("prodItemDesc");
                        invoiceItemDT.Columns.Add("bundles");
                        invoiceItemDT.Columns.Add("invoiceQty", typeof(double));
                        invoiceItemDT.Columns.Add("rate", typeof(double));
                        invoiceItemDT.Columns.Add("basicTotal", typeof(double));
                        //chetan[18-feb-2020] added for display GrandTotal on template
                        invoiceItemDT.Columns.Add("GrandTotal", typeof(double));
                        invoiceItemDT.Columns.Add("RateWithTax", typeof(double));
                        invoiceItemDT.Columns.Add("IGSTAmt", typeof(double));
                        invoiceItemDT.Columns.Add("CGSTAmt", typeof(double));
                        invoiceItemDT.Columns.Add("SGSTAmt", typeof(double));

                        invoiceItemDT.Columns.Add("IGSTPct", typeof(double));
                        invoiceItemDT.Columns.Add("CGSTPct", typeof(double));
                        invoiceItemDT.Columns.Add("SGSTPct", typeof(double));
                        invoiceItemDT.Columns.Add("hsn");

                        //Prajakta[2021-03-31] Added to show GST code upto given digits
                        Int32 gstCodeUptoDigits = 0;
                        TblConfigParamsTO gstCodeUptoDigitTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.SHOW_GST_CODE_UPTO_DIGITS);
                        if (gstCodeUptoDigitTO != null && gstCodeUptoDigitTO.ConfigParamVal != null
                            && gstCodeUptoDigitTO.ConfigParamVal.ToString() != "")
                        {
                            gstCodeUptoDigits = Convert.ToInt32(gstCodeUptoDigitTO.ConfigParamVal);
                        }

                        for (int i = 0; i < invoiceItemlist.Count; i++)
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceItemlist[i];
                            invoiceItemDT.Rows.Add();
                            Int32 invoiceItemDTCount = invoiceItemDT.Rows.Count - 1;
                            invoiceItemDT.Rows[invoiceItemDTCount]["srNo"] = i + 1;
                            invoiceItemDT.Rows[invoiceItemDTCount]["prodItemDesc"] = tblInvoiceItemDetailsTO.ProdItemDesc;
                            invoiceItemDT.Rows[invoiceItemDTCount]["bundles"] = tblInvoiceItemDetailsTO.Bundles;
                            invoiceItemDT.Rows[invoiceItemDTCount]["invoiceQty"] = Math.Round(tblInvoiceItemDetailsTO.InvoiceQty, 3);

                            if (isMathRoundoff == 1)
                            {
                                //invoiceItemDT.Rows[invoiceItemDTCount]["invoiceQty"] = tblInvoiceItemDetailsTO.InvoiceQty;
                                invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = Math.Round(tblInvoiceItemDetailsTO.Rate);
                                invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = Math.Round(tblInvoiceItemDetailsTO.BasicTotal);
                                invoiceItemDT.Rows[invoiceItemDTCount]["GrandTotal"] = Math.Round(tblInvoiceItemDetailsTO.GrandTotal);
                                invoiceItemDT.Rows[invoiceItemDTCount]["RateWithTax"] = Math.Round((tblInvoiceItemDetailsTO.GrandTotal / tblInvoiceItemDetailsTO.InvoiceQty));
                                //  invoiceItemDT.Rows[invoiceItemDTCount]["RateWithTax"] = tblInvoiceItemDetailsTO.GrandTotal/ tblInvoiceItemDetailsTO.InvoiceQty;
                            }
                            else
                            {
                                invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = Math.Round(tblInvoiceItemDetailsTO.Rate, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = Math.Round(tblInvoiceItemDetailsTO.BasicTotal, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["GrandTotal"] = Math.Round(tblInvoiceItemDetailsTO.GrandTotal, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["RateWithTax"] = Math.Round((tblInvoiceItemDetailsTO.GrandTotal / tblInvoiceItemDetailsTO.InvoiceQty), 2);
                            }

                            if (gstCodeUptoDigits > 0)
                            {
                                if (!String.IsNullOrEmpty(tblInvoiceItemDetailsTO.GstinCodeNo))
                                {
                                    if (gstCodeUptoDigits > tblInvoiceItemDetailsTO.GstinCodeNo.Length)
                                    {
                                        gstCodeUptoDigits = tblInvoiceItemDetailsTO.GstinCodeNo.Length;
                                    }
                                    tblInvoiceItemDetailsTO.GstinCodeNo = tblInvoiceItemDetailsTO.GstinCodeNo.Substring(0, gstCodeUptoDigits);
                                }
                            }
                            invoiceItemDT.Rows[invoiceItemDTCount]["hsn"] = tblInvoiceItemDetailsTO.GstinCodeNo;


                            if (tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList != null && tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count > 0)
                            {
                                for (int c = 0; c < tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList.Count; c++)
                                {
                                    TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList[c];
                                    if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                    {
                                        invoiceItemDT.Rows[invoiceItemDTCount]["IGSTPct"] = tblInvoiceItemTaxDtlsTO.TaxRatePct;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["IGSTAmt"] = tblInvoiceItemTaxDtlsTO.TaxAmt;

                                    }
                                    else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                    {
                                        invoiceItemDT.Rows[invoiceItemDTCount]["CGSTAmt"] = tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["CGSTPct"] = tblInvoiceItemTaxDtlsTO.TaxRatePct;

                                    }
                                    else if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                    {
                                        invoiceItemDT.Rows[invoiceItemDTCount]["SGSTAmt"] = tblInvoiceItemTaxDtlsTO.TaxAmt;
                                        invoiceItemDT.Rows[invoiceItemDTCount]["SGSTPct"] = tblInvoiceItemTaxDtlsTO.TaxRatePct;

                                    }
                                }
                            }
                        }
                        //if(invoiceItemDT.Rows.Count <finalItemCount)
                        //{
                        //    int ii= invoiceItemDT.Rows.Count  ;
                        //    for (int i=ii; i < finalItemCount;i++)
                        //    {
                        //        invoiceItemDT.Rows.Add();
                        //    }

                        //}
                        var freightResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.FREIGHT).FirstOrDefault();
                        if (freightResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["freightAmt"] = freightResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["freightAmt"] = Math.Round(freightResTO.TaxableAmt, 2);
                            }

                        }
                        //if (Convert.ToDouble(invoiceDT.Rows[0]["freightAmt"]) == 0)
                        else
                        {
                            invoiceDT.Rows[0]["freightAmt"] = tblInvoiceTO.FreightAmt;
                        }
                        var pfResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.PF).FirstOrDefault();
                        if (pfResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["pfAmt"] = pfResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["pfAmt"] = Math.Round(pfResTO.TaxableAmt, 2);
                            }

                        }
                        var cessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.CESS).FirstOrDefault();
                        if (cessResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["cessAmt"] = cessResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["cessAmt"] = Math.Round(cessResTO.TaxableAmt, 2);
                            }

                        }

                        var afterCessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.AFTERCESS).FirstOrDefault();
                        if (afterCessResTO != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["afterCessAmt"] = afterCessResTO.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["afterCessAmt"] = Math.Round(afterCessResTO.TaxableAmt, 2);
                            }

                        }
                        var insuranceTo = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.INSURANCE_ON_SALE).FirstOrDefault();
                        if (insuranceTo != null)
                        {
                            if (isMathRoundoff == 1)
                            {
                                invoiceDT.Rows[0]["insuranceAmt"] = insuranceTo.TaxableAmt;
                            }
                            else
                            {
                                invoiceDT.Rows[0]["insuranceAmt"] = Math.Round(insuranceTo.TaxableAmt, 2);
                            }

                        }
                        itemFooterDetailsDT.Rows.Add();
                        itemFooterDetailsDT.Columns.Add("totalQty", typeof(double));
                        itemFooterDetailsDT.Columns.Add("totalBundles");
                        itemFooterDetailsDT.Columns.Add("totalBasicAmt", typeof(double));
                        itemFooterDetailsDT.Columns.Add("EWayBillNo");
                        itemFooterDetailsDT.Rows[0]["EWayBillNo"] = tblInvoiceTO.ElectronicRefNo;
                        var totalQtyResTO = invoiceItemlist.Where(ele => ele.OtherTaxId == 0).ToList();
                        if (totalQtyResTO != null && totalQtyResTO.Count > 0)
                        {
                            Double totalQty = 0;
                            totalQty = totalQtyResTO.Sum(s => s.InvoiceQty);
                            if (isMathRoundoff == 1)
                            {
                                itemFooterDetailsDT.Rows[0]["totalQty"] = totalQty;
                                invoiceDT.Rows[0]["totalQty"] = totalQty;
                            }
                            else
                            {
                                itemFooterDetailsDT.Rows[0]["totalQty"] = Math.Round(totalQty, 3);
                                invoiceDT.Rows[0]["totalQty"] = Math.Round(totalQty, 3);
                            }

                        }
                        bool result;
                        Double sum = 0;
                        // commented by Aniket 
                        //bundles = invoiceItemlist.Sum(s => Convert.ToDouble(s.Bundles));
                        //Aniket [30-8-2019] added if bundles is null or empty string
                        for (int i = 0; i < invoiceItemlist.Count; i++)
                        {
                            Double bundles = 0;
                            result = double.TryParse(invoiceItemlist[i].Bundles, out bundles);
                            if (result)
                            {
                                sum += bundles;
                            }

                        }
                        itemFooterDetailsDT.Rows[0]["totalBundles"] = sum;
                        tblInvoiceTO.BasicAmt = invoiceItemlist.Sum(s => Convert.ToInt32(s.BasicTotal));//added code to sum of items basic total
                        itemFooterDetailsDT.Rows[0]["totalBasicAmt"] = Math.Round(tblInvoiceTO.BasicAmt, 2);
                        invoiceDT.Rows[0]["totalBundles"] = sum;
                        if (isMathRoundoff == 1)
                        {
                            invoiceDT.Rows[0]["totalBasicAmt"] = tblInvoiceTO.BasicAmt;
                        }
                        else
                        {
                            invoiceDT.Rows[0]["totalBasicAmt"] = Math.Round(tblInvoiceTO.BasicAmt, 2);
                        }


                    }

                    string strMobNo = "Mob No:";
                    string strStateCode = String.Empty;
                    string strGstin = String.Empty;
                    strGstin = "GSTIN:";
                    strStateCode = "State & Code:";
                    //if ((firmId == (Int32)Constants.FirmNameE.SRJ) )
                    //{
                    //    strGstin = "GSTIN:";
                    //    strStateCode = "State & Code:";
                    //}
                    //else if(firmId == (Int32)Constants.FirmNameE.Parameshwar)
                    //{
                    //   strGstin = "GSTIN/UIN:";
                    //   strStateCode = "State";
                    //    strCode = ",Code";

                    //}
                    string strPanNo = "PAN No:";


                    addressDT.Columns.Add("lblMobNo");
                    addressDT.Columns.Add("lblStateCode");
                    addressDT.Columns.Add("lblGstin");
                    addressDT.Columns.Add("lblPanNo");


                    addressDT.Columns.Add("strMobNo");
                    addressDT.Columns.Add("billingNm");
                    addressDT.Columns.Add("billingAddr");
                    addressDT.Columns.Add("billingGstNo");
                    addressDT.Columns.Add("billingPanNo");
                    addressDT.Columns.Add("billingState");
                    addressDT.Columns.Add("billingStateCode");
                    addressDT.Columns.Add("billingMobNo");


                    addressDT.Columns.Add("consigneeNm");
                    addressDT.Columns.Add("consigneeAddr");
                    addressDT.Columns.Add("consigneeGstNo");
                    addressDT.Columns.Add("consigneePanNo");
                    addressDT.Columns.Add("consigneeMobNo");
                    addressDT.Columns.Add("consigneeState");
                    addressDT.Columns.Add("consigneeStateCode");


                    addressDT.Columns.Add("lblConMobNo");
                    addressDT.Columns.Add("lblConStateCode");
                    addressDT.Columns.Add("lblConGstin");
                    addressDT.Columns.Add("lblConPanNo");


                    invoiceDT.Columns.Add("shippingNm");
                    invoiceDT.Columns.Add("shippingAddr");
                    invoiceDT.Columns.Add("shippingGstNo");
                    invoiceDT.Columns.Add("shippingPanNo");
                    invoiceDT.Columns.Add("shippingMobNo");
                    invoiceDT.Columns.Add("shippingState");
                    invoiceDT.Columns.Add("shippingStateCode");

                    invoiceDT.Columns.Add("lblShippingMobNo");
                    invoiceDT.Columns.Add("lblShippingStateCode");
                    invoiceDT.Columns.Add("lblShippingGstin");
                    invoiceDT.Columns.Add("lblShippingPanNo");
                    addressDT.Rows.Add();
                    addressDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;
                    headerDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;
                    invoiceDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;

                    headerDT.Rows[0]["BrokerName"] = tblInvoiceTO.BrokerName;
                    invoiceDT.Rows[0]["BrokerName"] = tblInvoiceTO.BrokerName;

                    invoiceDT.Rows[0]["freightCategory"] = tblInvoiceTO.CommentCategoryName;
                    if (!String.IsNullOrEmpty(tblInvoiceTO.PoDateStr))
                    {
                        DateTime poDate = Convert.ToDateTime(tblInvoiceTO.PoDateStr);
                        addressDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");
                        invoiceDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");
                        headerDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");

                    }
                    addressDT.Rows[0]["electronicRefNo"] = tblInvoiceTO.ElectronicRefNo;
                    string finalAddr = "", addr1 = "";
                    if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                    {
                        TblInvoiceAddressTO tblBillingInvoiceAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                        if (tblBillingInvoiceAddressTO != null)
                        {
                            addressDT.Rows[0]["lblMobNo"] = strMobNo;
                            addressDT.Rows[0]["lblStateCode"] = strStateCode;
                            addressDT.Rows[0]["lblGstin"] = strGstin;
                            addressDT.Rows[0]["lblPanNo"] = strPanNo;
                            addressDT.Rows[0]["billingNm"] = tblBillingInvoiceAddressTO.BillingName;

                            String addressStr = tblBillingInvoiceAddressTO.Address;

                            // if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka)&& !String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                            // {
                            //    if (tblBillingInvoiceAddressTO.Taluka.ToLower()==tblBillingInvoiceAddressTO.District.ToLower())
                            //     {
                            //             addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address + " " + tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;
                            //     }
                            //     else
                            //     {
                            //         addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address + ", " + tblBillingInvoiceAddressTO.Taluka
                            //                                       + tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;
                            //     }
                            // }
                            //else if(!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                            // {
                            //     addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address +" " + tblBillingInvoiceAddressTO.Taluka
                            //                                       + ", " + tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;
                            // }
                            // else if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                            // {
                            //     addressDT.Rows[0]["billingAddr"] = tblBillingInvoiceAddressTO.Address +" "
                            //                                      +  tblBillingInvoiceAddressTO.District + ", " + tblBillingInvoiceAddressTO.State;

                            // }
                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.VillageName))
                            {
                                addressStr += " " + tblBillingInvoiceAddressTO.VillageName;
                            }
                            //Aniket [6-9-2019] added PinCode in address
                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.PinCode) && tblBillingInvoiceAddressTO.PinCode != "0")
                            {
                                addressStr += "-" + tblBillingInvoiceAddressTO.PinCode;
                            }

                            //if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                            //{
                            //    addressStr += ", "+ tblBillingInvoiceAddressTO.Taluka;
                            //}

                            //if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                            //{
                            //    if (String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                            //        tblBillingInvoiceAddressTO.Taluka = String.Empty;

                            //    if (tblBillingInvoiceAddressTO.Taluka.ToLower().Trim() != tblBillingInvoiceAddressTO.District.ToLower().Trim())
                            //        addressStr += ",Dist-" + tblBillingInvoiceAddressTO.District;
                            //}


                            if (String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                                tblBillingInvoiceAddressTO.District = String.Empty;

                            if (String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                                tblBillingInvoiceAddressTO.Taluka = String.Empty;

                            String districtNameWithLabel = String.Empty;
                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.District))
                                districtNameWithLabel = ",Dist-" + tblBillingInvoiceAddressTO.District;

                            if (tblBillingInvoiceAddressTO.Taluka.ToLower().Trim() == tblBillingInvoiceAddressTO.District.ToLower().Trim())
                            {

                                addressStr += districtNameWithLabel;
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.Taluka))
                                {
                                    addressStr += ", " + tblBillingInvoiceAddressTO.Taluka;
                                }

                                addressStr += districtNameWithLabel;
                            }


                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.State))
                            {
                                //addressStr += ", " + tblBillingInvoiceAddressTO.State;
                            }


                            addressDT.Rows[0]["billingAddr"] = addressStr;

                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.GstinNo))
                                addressDT.Rows[0]["billingGstNo"] = tblBillingInvoiceAddressTO.GstinNo.ToUpper();

                            if (!String.IsNullOrEmpty(tblBillingInvoiceAddressTO.PanNo))
                                addressDT.Rows[0]["billingPanNo"] = tblBillingInvoiceAddressTO.PanNo.ToUpper();
                            //Aniket [9-9-2-2019]
                            if (tblInvoiceTO.IsConfirmed == 1)
                                addressDT.Rows[0]["billingMobNo"] = tblBillingInvoiceAddressTO.ContactNo;
                            else
                            {
                                addressDT.Rows[0]["billingMobNo"] = String.Empty;
                                addressDT.Rows[0]["lblMobNo"] = String.Empty;
                            }




                            if (stateList != null && stateList.Count > 0)
                            {
                                DropDownTO stateTO = stateList.Where(ele => ele.Value == tblBillingInvoiceAddressTO.StateId).FirstOrDefault();
                                addressDT.Rows[0]["billingState"] = tblBillingInvoiceAddressTO.State;
                                if (stateTO != null)
                                {
                                    //Saket [2019-04-12] Can be manage from template - Change for A1.s
                                    //addressDT.Rows[0]["billingStateCode"] = stateTO.Text + " " + stateTO.Tag;
                                    addressDT.Rows[0]["billingStateCode"] = stateTO.Tag;
                                }
                            }



                        }
                        Boolean IsDisplayConsignee = true;
                        TblInvoiceAddressTO tblConsigneeInvoiceAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS).FirstOrDefault();
                        if (tblConsigneeInvoiceAddressTO != null)
                        {
                            if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.BillingName))
                            {

                                TblConfigParamsTO tempConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName("DISPLAY_CONSIGNEE_ADDRESS_ON_PRINTABLE_INVOICE");
                                if (tempConfigParamsTO != null)
                                {
                                    if (Convert.ToInt32(tempConfigParamsTO.ConfigParamVal) == 1)
                                    {
                                        IsDisplayConsignee = true;
                                    }
                                    else
                                    {
                                        IsDisplayConsignee = false;
                                    }
                                }
                                if (!IsDisplayConsignee)
                                {
                                    if (tblConsigneeInvoiceAddressTO.BillingName.Trim() == tblBillingInvoiceAddressTO.BillingName.Trim())
                                    {
                                        if (tblConsigneeInvoiceAddressTO.Address.Trim() == tblBillingInvoiceAddressTO.Address.Trim())
                                            IsDisplayConsignee = false;
                                        else
                                            IsDisplayConsignee = true;
                                    }
                                    else
                                    {
                                        IsDisplayConsignee = true;
                                    }
                                }
                                //if(tblConsigneeInvoiceAddressTO.BillingName.Trim() != tblBillingInvoiceAddressTO.BillingName.Trim())
                                //{
                                //    IsDisplayConsignee = true;
                                //}

                                if (IsDisplayConsignee)
                                {
                                    addressDT.Rows[0]["lblConMobNo"] = strMobNo;
                                    addressDT.Rows[0]["lblConStateCode"] = strStateCode;
                                    addressDT.Rows[0]["lblConGstin"] = strGstin;
                                    addressDT.Rows[0]["lblConPanNo"] = strPanNo;
                                    addressDT.Rows[0]["consigneeNm"] = tblConsigneeInvoiceAddressTO.BillingName;

                                    String consigneeAddr = tblConsigneeInvoiceAddressTO.Address;
                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka) && !String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                    //{
                                    //    if (tblConsigneeInvoiceAddressTO.Taluka.ToLower() == tblConsigneeInvoiceAddressTO.District.ToLower())
                                    //    {
                                    //        addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + " " + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;
                                    //    }
                                    //    else
                                    //    {
                                    //        addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + ", " + tblConsigneeInvoiceAddressTO.Taluka
                                    //                                      + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;
                                    //    }
                                    //}
                                    //else if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                    //{
                                    //    addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + " " + tblConsigneeInvoiceAddressTO.Taluka
                                    //                                      + ", " + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;
                                    //}
                                    //else if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                    //{
                                    //    addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + " "
                                    //                                     + tblConsigneeInvoiceAddressTO.District + ", " + tblConsigneeInvoiceAddressTO.State;

                                    //}

                                    //addressDT.Rows[0]["consigneeAddr"] = tblConsigneeInvoiceAddressTO.Address + "," + tblConsigneeInvoiceAddressTO.Taluka
                                    //                                    + " ," + tblConsigneeInvoiceAddressTO.District + "," + tblConsigneeInvoiceAddressTO.State;
                                    //Aniket [6-9-2019] added PinCode in address
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.VillageName))
                                    {
                                        consigneeAddr += " " + tblConsigneeInvoiceAddressTO.VillageName;
                                    }
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.PinCode) && tblConsigneeInvoiceAddressTO.PinCode != "0")
                                    {
                                        consigneeAddr += "- " + tblConsigneeInvoiceAddressTO.PinCode;
                                    }
                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                    //{
                                    //    consigneeAddr += ", " + tblConsigneeInvoiceAddressTO.Taluka;
                                    //}

                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                    //{
                                    //    if (String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                    //        tblConsigneeInvoiceAddressTO.Taluka = String.Empty;

                                    //    if (tblConsigneeInvoiceAddressTO.Taluka.ToLower() != tblConsigneeInvoiceAddressTO.District.ToLower())
                                    //        consigneeAddr += ",Dist-" + tblConsigneeInvoiceAddressTO.District;
                                    //}


                                    if (String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                        tblConsigneeInvoiceAddressTO.District = String.Empty;

                                    if (String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                        tblConsigneeInvoiceAddressTO.Taluka = String.Empty;

                                    String districtNameWithLabel = String.Empty;
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.District))
                                        districtNameWithLabel = ",Dist-" + tblConsigneeInvoiceAddressTO.District;

                                    if (tblConsigneeInvoiceAddressTO.Taluka.ToLower().Trim() == tblConsigneeInvoiceAddressTO.District.ToLower().Trim())
                                    {
                                        consigneeAddr += districtNameWithLabel;
                                    }
                                    else
                                    {
                                        if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.Taluka))
                                        {
                                            consigneeAddr += ", " + tblConsigneeInvoiceAddressTO.Taluka;
                                        }

                                        consigneeAddr += districtNameWithLabel;
                                    }




                                    //if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.State))
                                    //{
                                    //    consigneeAddr += ", " + tblConsigneeInvoiceAddressTO.State;
                                    //}
                                    //if(!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.PinCode) && tblConsigneeInvoiceAddressTO.PinCode!="0")
                                    //{
                                    //    consigneeAddr += "- " + tblConsigneeInvoiceAddressTO.PinCode;
                                    //}
                                    addressDT.Rows[0]["consigneeAddr"] = consigneeAddr;
                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.GstinNo))
                                        addressDT.Rows[0]["consigneeGstNo"] = tblConsigneeInvoiceAddressTO.GstinNo.ToUpper();

                                    if (!String.IsNullOrEmpty(tblConsigneeInvoiceAddressTO.PanNo))
                                        addressDT.Rows[0]["consigneePanNo"] = tblConsigneeInvoiceAddressTO.PanNo.ToUpper();
                                    //Aniket [9-9-2-2019]
                                    if (tblInvoiceTO.IsConfirmed == 1)
                                        addressDT.Rows[0]["consigneeMobNo"] = tblConsigneeInvoiceAddressTO.ContactNo;
                                    else
                                    {
                                        addressDT.Rows[0]["consigneeMobNo"] = String.Empty;
                                        addressDT.Rows[0]["lblMobNo"] = String.Empty;
                                    }


                                    if (stateList != null && stateList.Count > 0)
                                    {
                                        DropDownTO stateTO = stateList.Where(ele => ele.Value == tblConsigneeInvoiceAddressTO.StateId).FirstOrDefault();
                                        addressDT.Rows[0]["consigneeState"] = tblConsigneeInvoiceAddressTO.State;
                                        if (stateTO != null)
                                        {
                                            addressDT.Rows[0]["consigneeStateCode"] = stateTO.Tag;
                                        }
                                    }
                                }
                            }



                        }

                        TblInvoiceAddressTO tblShippingAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.SHIPPING_ADDRESS).FirstOrDefault();
                        if (tblShippingAddressTO != null)
                        {
                            if (!String.IsNullOrEmpty(tblShippingAddressTO.Address))
                            {
                                if (tblShippingAddressTO.Address.Trim() != tblBillingInvoiceAddressTO.Address.Trim())
                                {
                                    //headerDT.Rows.Add();
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.ContactNo))
                                    {
                                        invoiceDT.Rows[0]["lblShippingMobNo"] = strMobNo;
                                    }
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.State))
                                    {
                                        invoiceDT.Rows[0]["lblShippingStateCode"] = strStateCode;
                                    }
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.GstinNo))
                                    {
                                        invoiceDT.Rows[0]["lblShippingGstin"] = strGstin;
                                    }
                                    if (!String.IsNullOrEmpty(tblShippingAddressTO.PanNo))
                                    {
                                        invoiceDT.Rows[0]["lblShippingPanNo"] = strPanNo;
                                    }



                                    invoiceDT.Rows[0]["shippingNm"] = tblShippingAddressTO.BillingName;
                                    invoiceDT.Rows[0]["shippingAddr"] = tblShippingAddressTO.Address + "," + tblShippingAddressTO.Taluka
                                                                        + " ," + tblShippingAddressTO.District + "," + tblShippingAddressTO.State;
                                    invoiceDT.Rows[0]["shippingGstNo"] = tblShippingAddressTO.GstinNo;
                                    invoiceDT.Rows[0]["shippingPanNo"] = tblShippingAddressTO.PanNo;
                                    //Aniket [9-9-2-2019]
                                    if (tblInvoiceTO.IsConfirmed == 1)
                                        invoiceDT.Rows[0]["shippingMobNo"] = tblShippingAddressTO.ContactNo;
                                    else
                                        invoiceDT.Rows[0]["shippingMobNo"] = String.Empty;


                                }
                            }

                        }

                        //get org bank details
                        List<TblInvoiceBankDetailsTO> tblInvoiceBankDetailsTOList = _iTblInvoiceBankDetailsDAO.SelectInvoiceBankDetails(organizationTO.IdOrganization);
                        if (tblInvoiceBankDetailsTOList != null && tblInvoiceBankDetailsTOList.Count > 0)
                        {
                            TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO = tblInvoiceBankDetailsTOList.Where(c => c.IsPriority == 1).FirstOrDefault();
                            invoiceDT.Rows[0]["bankName"] = tblInvoiceBankDetailsTO.BankName;
                            invoiceDT.Rows[0]["accountNo"] = tblInvoiceBankDetailsTO.AccountNo;
                            invoiceDT.Rows[0]["branchName"] = tblInvoiceBankDetailsTO.Branch;
                            invoiceDT.Rows[0]["ifscCode"] = tblInvoiceBankDetailsTO.IfscCode;
                        }

                        //get org declaration and terms condition details
                        List<TblInvoiceOtherDetailsTO> tblInvoiceOtherDetailsTOList = _iTblInvoiceOtherDetailsDAO.SelectInvoiceOtherDetails(organizationTO.IdOrganization);
                        if (tblInvoiceOtherDetailsTOList != null && tblInvoiceOtherDetailsTOList.Count > 0)
                        {
                            TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO = tblInvoiceOtherDetailsTOList.Where(c => c.DetailTypeId == (int)Constants.invoiceOtherDetailsTypeE.DESCRIPTION).FirstOrDefault();
                            invoiceDT.Rows[0]["declaration"] = tblInvoiceOtherDetailsTO.Description;
                        }

                    }

                }

                //Saket [2019-08-11] To keep the both DT columns same
                //If two same columns with different values then 2 rows will be created while merging DT.
                //invoiceDT.Merge(headerDT);


                DataTable WheaderDT = new DataTable();
                DataTable loadingItemDT = new DataTable();
                DataTable loadingItemDTForGatePass = new DataTable();


                WheaderDT.TableName = "WheaderDT";
                loadingItemDT.TableName = "loadingItemDT";
                loadingItemDTForGatePass.TableName = "loadingItemDTForGatePass";

                #region Add Columns
                WheaderDT.Columns.Add("InvoiceId");
                WheaderDT.Columns.Add("FirmName");
                WheaderDT.Columns.Add("dealername");
                WheaderDT.Columns.Add("ContactNo"); //[2021-10-13] Dhananjay Added
                WheaderDT.Columns.Add("VillageName"); //[2021-10-20] Dhananjay Added
                WheaderDT.Columns.Add("VehicleNo");
                WheaderDT.Columns.Add("DriverContactNo"); //[2021-10-29] Dhananjay Added
                WheaderDT.Columns.Add("LoadingSlipId");
                WheaderDT.Columns.Add("loadingLayerDesc");
                WheaderDT.Columns.Add("DateTime");
                WheaderDT.Columns.Add("Date");
                WheaderDT.Columns.Add("TotalBundles");
                WheaderDT.Columns.Add("TotalNetWt", typeof(double));
                WheaderDT.Columns.Add("TotalTareWt", typeof(double));
                WheaderDT.Columns.Add("TotalGrossWt", typeof(double));
                WheaderDT.Columns.Add("invoiceNo");
                //Prajakta[2020-07-14] Added
                WheaderDT.Columns.Add("orgFirmName");
                WheaderDT.Columns.Add("orgPhoneNo");
                WheaderDT.Columns.Add("orgFaxNo");
                WheaderDT.Columns.Add("orgWebsite");
                WheaderDT.Columns.Add("orgEmailAddr");
                WheaderDT.Columns.Add("plotNo");
                WheaderDT.Columns.Add("areaName");
                WheaderDT.Columns.Add("district");
                WheaderDT.Columns.Add("pinCode");
                WheaderDT.Columns.Add("orgVillageNm");
                WheaderDT.Columns.Add("orgAddr");
                WheaderDT.Columns.Add("orgState");
                WheaderDT.Columns.Add("orgStateCode");


                loadingItemDTForGatePass.Columns.Add("SrNo");
                loadingItemDTForGatePass.Columns.Add("DisplayName");
                loadingItemDTForGatePass.Columns.Add("MaterialDesc");
                loadingItemDTForGatePass.Columns.Add("ProdItemDesc");
                loadingItemDTForGatePass.Columns.Add("LoadingQty", typeof(double));
                loadingItemDTForGatePass.Columns.Add("Bundles");
                loadingItemDTForGatePass.Columns.Add("LoadedWeight", typeof(double));
                loadingItemDTForGatePass.Columns.Add("MstLoadedBundles");
                loadingItemDTForGatePass.Columns.Add("LoadedBundles");
                loadingItemDTForGatePass.Columns.Add("GrossWt", typeof(double));
                loadingItemDTForGatePass.Columns.Add("TareWt", typeof(double));
                loadingItemDTForGatePass.Columns.Add("NetWt", typeof(double));
                loadingItemDTForGatePass.Columns.Add("BrandDesc");
                loadingItemDTForGatePass.Columns.Add("ProdSpecDesc");
                loadingItemDTForGatePass.Columns.Add("ProdcatDesc");
                loadingItemDTForGatePass.Columns.Add("ItemName");
                loadingItemDTForGatePass.Columns.Add("UpdatedOn");
                loadingItemDTForGatePass.Columns.Add("DisplayField");
                loadingItemDTForGatePass.Columns.Add("LoadingSlipId");


                loadingItemDT.Columns.Add("SrNo");
                loadingItemDT.Columns.Add("DisplayName");
                loadingItemDT.Columns.Add("MaterialDesc");
                loadingItemDT.Columns.Add("ProdItemDesc");
                loadingItemDT.Columns.Add("LoadingQty", typeof(double));
                loadingItemDT.Columns.Add("Bundles");
                loadingItemDT.Columns.Add("LoadedWeight", typeof(double));
                loadingItemDT.Columns.Add("MstLoadedBundles");
                loadingItemDT.Columns.Add("LoadedBundles");
                loadingItemDT.Columns.Add("GrossWt", typeof(double));
                loadingItemDT.Columns.Add("TareWt", typeof(double));
                loadingItemDT.Columns.Add("NetWt", typeof(double));
                loadingItemDT.Columns.Add("BrandDesc");
                loadingItemDT.Columns.Add("ProdSpecDesc");
                loadingItemDT.Columns.Add("ProdcatDesc");
                loadingItemDT.Columns.Add("ItemName");
                loadingItemDT.Columns.Add("UpdatedOn");
                loadingItemDT.Columns.Add("DisplayField");
                loadingItemDT.Columns.Add("LoadingSlipId");




                #endregion

                if (TblLoadingSlipTO != null)
                {
                    WheaderDT.Rows.Add();
                    double totalBundle = 0;
                    double totalNetWt = 0;
                    //double 
                    WheaderDT.Rows[0]["InvoiceId"] = invoiceId;
                    //prajkta[25-06-2020]added
                    WheaderDT.Rows[0]["invoiceNo"] = tblInvoiceTO.InvoiceNo;
                    if (invoiceAddressTOList != null && invoiceAddressTOList.Count > 0)
                    {
                        TblInvoiceAddressTO tblInvoiceAddressTO = invoiceAddressTOList.Where(w => w.TxnAddrTypeId == (Int32)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                        WheaderDT.Rows[0]["FirmName"] = tblInvoiceAddressTO.BillingName;
                        WheaderDT.Rows[0]["dealername"] = tblInvoiceAddressTO.BillingName;
                        WheaderDT.Rows[0]["ContactNo"] = tblInvoiceAddressTO.ContactNo; //[2021-10-13] Dhananjay Added
                        WheaderDT.Rows[0]["VillageName"] = tblInvoiceAddressTO.VillageName; //[2021-10-20] Dhananjay Added
                    }
                    WheaderDT.Rows[0]["VehicleNo"] = TblLoadingSlipTO.VehicleNo;
                    WheaderDT.Rows[0]["DriverContactNo"] = TblLoadingSlipTO.ContactNo; //[2021-10-29] Dhananjay Added
                    WheaderDT.Rows[0]["loadingLayerDesc"] = TblLoadingSlipTO.LoadingSlipExtTOList[0].LoadingLayerDesc;
                    WheaderDT.Rows[0]["LoadingSlipId"] = TblLoadingSlipTO.IdLoadingSlip;
                    //headerDT.Rows[0]["Date"] = TblLoadingSlipTO.CreatedOnStr;
                    //headerDT.Rows[0]["Date"] = TblLoadingSlipTO.CreatedOnStr;
                    //headerDT.Rows[0]["Date"] = tblInvoiceTO.CreatedOnStr;
                    if (tblInvoiceTO != null && tblInvoiceTO.CreatedOn != new DateTime())
                    {
                        string dtStr = tblInvoiceTO.CreatedOn.ToShortDateString();
                        WheaderDT.Rows[0]["DateTime"] = tblInvoiceTO.CreatedOnStr;
                        WheaderDT.Rows[0]["Date"] = dtStr;
                    }
                    else
                    {
                        string dtStr = TblLoadingSlipTO.CreatedOn.ToShortDateString();
                        WheaderDT.Rows[0]["DateTime"] = TblLoadingSlipTO.CreatedOnStr;
                        WheaderDT.Rows[0]["Date"] = dtStr;
                    }

                    if (TblLoadingSlipTO.LoadingSlipExtTOList != null && TblLoadingSlipTO.LoadingSlipExtTOList.Count > 0)
                    {
                        TblLoadingSlipTO.LoadingSlipExtTOList = TblLoadingSlipTO.LoadingSlipExtTOList.OrderBy(o => o.CalcTareWeight).ToList();

                        for (int j = 0; j < TblLoadingSlipTO.LoadingSlipExtTOList.Count; j++)
                        {
                            TblLoadingSlipExtTO tblLoadingSlipExtTO = TblLoadingSlipTO.LoadingSlipExtTOList[j];
                            loadingItemDT.Rows.Add();
                            Int32 loadItemDTCount = loadingItemDT.Rows.Count - 1;

                            loadingItemDT.Rows[loadItemDTCount]["SrNo"] = loadItemDTCount + 1;
                            string displayName = tblLoadingSlipExtTO.ProdCatDesc + " " + tblLoadingSlipExtTO.ProdSpecDesc + " " + tblLoadingSlipExtTO.MaterialDesc;
                            if (displayName == "  ")
                                displayName = tblLoadingSlipExtTO.ItemName;
                            loadingItemDT.Rows[loadItemDTCount]["DisplayName"] = displayName;// tblLoadingSlipExtTO.DisplayName;

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
                            totalBundle += tblLoadingSlipExtTO.LoadedBundles;
                            loadingItemDT.Rows[loadItemDTCount]["TareWt"] = (tblLoadingSlipExtTO.CalcTareWeight / 1000);
                            loadingItemDT.Rows[loadItemDTCount]["GrossWt"] = (tblLoadingSlipExtTO.CalcTareWeight + tblLoadingSlipExtTO.LoadedWeight) / 1000;
                            loadingItemDT.Rows[loadItemDTCount]["NetWt"] = tblLoadingSlipExtTO.LoadedWeight / 1000;
                            totalNetWt += (tblLoadingSlipExtTO.LoadedWeight / 1000);
                            loadingItemDT.Rows[loadItemDTCount]["LoadedWeight"] = tblLoadingSlipExtTO.LoadedWeight;
                            loadingItemDT.Rows[loadItemDTCount]["MstLoadedBundles"] = tblLoadingSlipExtTO.MstLoadedBundles;
                            loadingItemDT.Rows[loadItemDTCount]["LoadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                            loadingItemDT.Rows[loadItemDTCount]["LoadingSlipId"] = tblLoadingSlipExtTO.LoadingSlipId;

                        }
                    }
                    WheaderDT.Rows[0]["TotalBundles"] = totalBundle;
                    WheaderDT.Rows[0]["TotalNetWt"] = totalNetWt;
                    WheaderDT.Rows[0]["TotalTareWt"] = (tblInvoiceTO.TareWeight / 1000);
                    WheaderDT.Rows[0]["TotalGrossWt"] = (tblInvoiceTO.GrossWeight / 1000);
                    WheaderDT.Rows[0]["TotalNetWt"] = (tblInvoiceTO.NetWeight / 1000);
                }


                //Prajakta[2020-07-14] Added to show orgFirmName and address details 

                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (configParamsTO != null)
                    {
                        defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
                    }
                }
                else
                {
                    defaultCompOrgId = tblInvoiceTO.InvFromOrgId;
                }
                if (organizationTO != null)
                {
                    //headerDT.Rows.Add();
                    //headerDT.Rows.Add();
                    WheaderDT.Rows[0]["orgFirmName"] = organizationTO.FirmName;

                    WheaderDT.Rows[0]["orgPhoneNo"] = organizationTO.PhoneNo;
                    WheaderDT.Rows[0]["orgFaxNo"] = organizationTO.FaxNo;
                    WheaderDT.Rows[0]["orgWebsite"] = organizationTO.Website;
                    WheaderDT.Rows[0]["orgEmailAddr"] = organizationTO.EmailAddr;
                }


                if (tblAddressTO != null)
                {
                    String orgAddrStr = String.Empty;
                    if (!String.IsNullOrEmpty(tblAddressTO.PlotNo))
                    {
                        orgAddrStr += tblAddressTO.PlotNo;
                        WheaderDT.Rows[0]["plotNo"] = tblAddressTO.PlotNo;
                    }
                    if (!String.IsNullOrEmpty(tblAddressTO.AreaName))
                    {
                        orgAddrStr += " " + tblAddressTO.AreaName;
                        WheaderDT.Rows[0]["areaName"] = tblAddressTO.AreaName;
                    }
                    if (!String.IsNullOrEmpty(tblAddressTO.DistrictName))
                    {
                        orgAddrStr += " " + tblAddressTO.DistrictName;
                        WheaderDT.Rows[0]["district"] = tblAddressTO.DistrictName;

                    }
                    if (tblAddressTO.Pincode > 0)
                    {
                        orgAddrStr += "-" + tblAddressTO.Pincode;
                        WheaderDT.Rows[0]["pinCode"] = tblAddressTO.Pincode;

                    }
                    WheaderDT.Rows[0]["orgVillageNm"] = tblAddressTO.VillageName + "-" + tblAddressTO.Pincode;
                    WheaderDT.Rows[0]["orgAddr"] = orgAddrStr;
                    WheaderDT.Rows[0]["orgState"] = tblAddressTO.StateName;

                    if (stateList != null && stateList.Count > 0)
                    {
                        DropDownTO stateTO = stateList.Where(ele => ele.Value == tblAddressTO.StateId).FirstOrDefault();
                        if (stateTO != null)
                        {
                            WheaderDT.Rows[0]["orgStateCode"] = stateTO.Tag;
                        }
                    }
                }


                //headerDT = loadingDT.Copy();
                WheaderDT.TableName = "WheaderDT";
                printDataSet.Tables.Add(WheaderDT);

                loadingItemDT.TableName = "loadingItemDT";
                printDataSet.Tables.Add(loadingItemDT);

                loadingItemDTForGatePass = loadingItemDT.Copy();

                loadingItemDT.TableName = "loadingItemDTForGatePass";
                printDataSet.Tables.Add(loadingItemDTForGatePass);

                headerDT = invoiceDT.Copy();
                headerDT.TableName = "headerDT";

                printDataSet.Tables.Add(headerDT);
                printDataSet.Tables.Add(qrCodeDT);
                printDataSet.Tables.Add(invoiceDT);
                printDataSet.Tables.Add(invoiceItemDT);
                printDataSet.Tables.Add(addressDT);
                printDataSet.Tables.Add(itemFooterDetailsDT);
                printDataSet.Tables.Add(commercialDT);
                printDataSet.Tables.Add(hsnItemTaxDT);
                printDataSet.Tables.Add(multipleInvoiceCopyDT);

                DataTable headerDTV2 = new DataTable();
                DataTable invoiceDTV2 = new DataTable();
                DataTable invoiceItemDTV2 = new DataTable();
                DataTable addressDTV2 = new DataTable();

                addressDTV2.TableName = "addressDTV2";
                //headerDT.TableName = "headerDT";
                invoiceDTV2.TableName = "headerDTV2";
                invoiceItemDTV2.TableName = "invoiceItemDTV2";

                if (tblInvoiceTO.InvFromOrgId == 0)
                {
                    TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                    if (configParamsTO != null)
                    {
                        defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
                    }
                }
                else
                {
                    defaultCompOrgId = tblInvoiceTO.InvFromOrgId;
                }
                //TblOrganizationTO organizationTO = _iTblOrganizationBL.SelectTblOrganizationTO(defaultCompOrgId);

                if (tblInvoiceTO != null)
                {

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        tblInvoiceTO.VehicleNo = tblInvoiceTO.VehicleNo.ToUpper();
                    }
                    invoiceDTV2.Columns.Add("invoiceNo");
                    invoiceDTV2.Columns.Add("invoiceDateStr");
                    invoiceDTV2.Columns.Add("vehicleNo");
                    invoiceDTV2.Columns.Add("poNo");
                    invoiceDTV2.Columns.Add("poDateStr");

                    invoiceDTV2.Rows.Add();
                    invoiceDTV2.Rows[0]["invoiceNo"] = tblInvoiceTO.InvoiceNo;
                    invoiceDTV2.Rows[0]["invoiceDateStr"] = tblInvoiceTO.InvoiceDateStr;

                    if (!String.IsNullOrEmpty(tblInvoiceTO.VehicleNo))
                    {
                        invoiceDTV2.Rows[0]["vehicleNo"] = tblInvoiceTO.VehicleNo;
                    }
                    addressDTV2.Columns.Add("billingNm");
                    addressDTV2.Columns.Add("consigneeNm");
                    if (tblInvoiceTO.InvoiceAddressTOList != null && tblInvoiceTO.InvoiceAddressTOList.Count > 0)
                    {
                        addressDTV2.Rows.Add();
                        TblInvoiceAddressTO tblBillingInvoiceAddressTO = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.BILLING_ADDRESS).FirstOrDefault();
                        if (tblBillingInvoiceAddressTO != null)
                        {
                            addressDTV2.Rows[0]["billingNm"] = tblBillingInvoiceAddressTO.BillingName;
                        }
                        TblInvoiceAddressTO tblBillingInvoiceAddressTOV2 = tblInvoiceTO.InvoiceAddressTOList.Where(eleA => eleA.TxnAddrTypeId == (int)Constants.TxnDeliveryAddressTypeE.CONSIGNEE_ADDRESS).FirstOrDefault();
                        if (tblBillingInvoiceAddressTOV2 != null)
                        {
                            addressDTV2.Rows[0]["consigneeNm"] = tblBillingInvoiceAddressTOV2.BillingName;
                        }
                    }
                    //Int32 finalItemCount = 15;
                    if (tblInvoiceTO.InvoiceItemDetailsTOList != null && tblInvoiceTO.InvoiceItemDetailsTOList.Count > 0)
                    {
                        tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceTO.InvoiceItemDetailsTOList;
                        List<TblInvoiceItemDetailsTO> invoiceItemlist = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == 0).ToList();
                        if (invoiceItemlist != null && invoiceItemlist.Count > 0)
                        {
                            invoiceItemDTV2.Columns.Add("srNo");
                            invoiceItemDTV2.Columns.Add("prodItemDesc");
                            invoiceItemDTV2.Columns.Add("bundles");
                            invoiceItemDTV2.Columns.Add("invoiceQty", typeof(double));
                            invoiceItemDTV2.Columns.Add("TestingDate");
                            invoiceItemDTV2.Columns.Add("ChemC", typeof(double));
                            invoiceItemDTV2.Columns.Add("ChemS", typeof(double));
                            invoiceItemDTV2.Columns.Add("ChemP", typeof(double));
                            invoiceItemDTV2.Columns.Add("MechProof", typeof(double));
                            invoiceItemDTV2.Columns.Add("MechTen", typeof(double));
                            invoiceItemDTV2.Columns.Add("MechElon", typeof(double));
                            invoiceItemDTV2.Columns.Add("MechTEle", typeof(double));
                            invoiceItemDTV2.Columns.Add("ChemCE", typeof(double));
                            invoiceItemDTV2.Columns.Add("ChemT", typeof(double));
                            invoiceItemDTV2.Columns.Add("RatioTS", typeof(double));

                            invoiceItemDTV2.Columns.Add("CastNo");
                            invoiceItemDTV2.Columns.Add("Grade");
                            invoiceItemDTV2.Columns.Add("BendTest");
                            invoiceItemDTV2.Columns.Add("RebandTest");
                            invoiceItemDTV2.Columns.Add("GradeOfSteel");
                            invoiceItemDTV2.Columns.Add("Remark");

                            int bookingId = 0;
                            for (int x = 0; x < invoiceItemlist.Count; x++)
                            {
                                invoiceItemDTV2.Rows.Add();
                                int count = invoiceItemDTV2.Rows.Count - 1;
                                TblInvoiceItemDetailsTO TblInvoiceItemDetailsTO = invoiceItemlist[x];
                                if (TblInvoiceItemDetailsTO.LoadingSlipExtId > 0)
                                {
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = _iTblLoadingSlipExtDAO.SelectTblLoadingSlipExt(TblInvoiceItemDetailsTO.LoadingSlipExtId);
                                    SizeTestingDtlTO sizeTestingDtlTO = _iTblParitySummaryDAO.SelectTestCertificateDdtl(TblInvoiceItemDetailsTO.SizeTestingDtlId);
                                    if (tblLoadingSlipExtTO != null)
                                    {
                                        if (tblLoadingSlipExtTO.BookingId > 0 && bookingId == 0)
                                            bookingId = tblLoadingSlipExtTO.BookingId;
                                        invoiceItemDTV2.Rows[count]["srNo"] = x + 1;
                                        invoiceItemDTV2.Rows[count]["prodItemDesc"] = tblLoadingSlipExtTO.MaterialDesc;
                                        invoiceItemDTV2.Rows[count]["invoiceQty"] = TblInvoiceItemDetailsTO.InvoiceQty;
                                        if (sizeTestingDtlTO != null)
                                        {
                                            invoiceItemDTV2.Rows[count]["TestingDate"] = sizeTestingDtlTO.TestingDate.ToString("dd-MM-yyyy");
                                            invoiceItemDTV2.Rows[count]["ChemC"] = sizeTestingDtlTO.ChemC;
                                            invoiceItemDTV2.Rows[count]["ChemS"] = sizeTestingDtlTO.ChemS;
                                            invoiceItemDTV2.Rows[count]["ChemP"] = sizeTestingDtlTO.ChemP;
                                            invoiceItemDTV2.Rows[count]["MechProof"] = sizeTestingDtlTO.MechProof;
                                            invoiceItemDTV2.Rows[count]["MechTen"] = sizeTestingDtlTO.MechTen;
                                            invoiceItemDTV2.Rows[count]["MechElon"] = sizeTestingDtlTO.MechElon;
                                            invoiceItemDTV2.Rows[count]["MechTEle"] = sizeTestingDtlTO.MechTEle;

                                            invoiceItemDTV2.Rows[count]["ChemCE"] = sizeTestingDtlTO.ChemCE;
                                            invoiceItemDTV2.Rows[count]["ChemT"] = sizeTestingDtlTO.ChemT;
                                            invoiceItemDTV2.Rows[count]["RatioTS"] = Math.Round(sizeTestingDtlTO.MechTen / sizeTestingDtlTO.MechProof, 2);

                                            invoiceItemDTV2.Rows[count]["CastNo"] = sizeTestingDtlTO.CastNo;
                                            invoiceItemDTV2.Rows[count]["Grade"] = sizeTestingDtlTO.Grade;
                                            invoiceItemDTV2.Rows[count]["BendTest"] = "OK";
                                            invoiceItemDTV2.Rows[count]["RebandTest"] = "OK";
                                            invoiceItemDTV2.Rows[count]["GradeOfSteel"] = "OK";
                                            invoiceItemDTV2.Rows[count]["Remark"] = "OK";
                                        }

                                    }
                                }
                            }

                            if (bookingId > 0)
                            {
                                TblBookingsTO tblBookingsTOV2 = _iTblBookingsBL.SelectTblBookingsTO(bookingId);
                                if (tblBookingsTO != null)
                                {
                                    headerDT.Rows[0]["poNo"] = tblBookingsTOV2.PoNo;
                                    headerDT.Rows[0]["poDateStr"] = tblBookingsTOV2.PoDateStr;
                                }
                            }
                        }

                    }
                }
                //headerDT = invoiceDT.Clone();
                printDataSet.Tables.Add(addressDTV2);
                printDataSet.Tables.Add(invoiceDTV2);
                printDataSet.Tables.Add(invoiceItemDTV2);

                int isMultipleTemplateByCorNc = 0;

                string templateName = "WhatsAppInvoiceReport";
                String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(templateName);
                // templateFilePath = @"C:\Deliver Templates\SER INVOICE Template.xls";
                String fileName = "Bill-" + DateTime.Now.Ticks;

                //download location for rewrite  template file
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                // RunReport runReport = new RunReport();
                Boolean IsProduction = true;
                //WriteLog("ConsoleLog", String.Format("{0} @ {1}", "Log is Created at", DateTime.Now));
                //WriteLog(" ConsoleLog", "\\n Log is Written Successfully !!!");
                //Console.ReadLine();

                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                //ResultMessage resultMsg = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    try
                    {
                        //WriteLog(" ConsoleLog", "\\n Old File is created");
                        if (tblInvoiceTO != null && tblInvoiceTO.BookingTaxCategoryId > 0)
                        {
                            //WriteLog(" ConsoleLog", "\\n  tblInvoiceTO.BookingTaxCategoryId ");
                            string templateNameTeV2 = "WhatsAppInvoiceTestCertificateAone";
                            String templateFilePathV2 = _iDimReportTemplateBL.SelectReportFullName(templateNameTeV2);
                            //WriteLog(" ConsoleLog", "\\n templateFilePathV2 " + templateFilePathV2);

                            // templateFilePath = @"C:\Deliver Templates\SER INVOICE Template.xls";
                            String fileNameV2 = "TestCertificate-" + DateTime.Now.Ticks;

                            //download location for rewrite  template file
                            String saveLocationV2 = AppDomain.CurrentDomain.BaseDirectory + fileNameV2 + ".xls";
                            // RunReport runReport = new RunReport();
                            Boolean IsProductionV2 = true;

                            TblConfigParamsTO tblConfigParamsTOV2 = _iTblConfigParamsBL.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                            if (tblConfigParamsTOV2 != null)
                            {
                                if (Convert.ToInt32(tblConfigParamsTOV2.ConfigParamVal) == 0)
                                {
                                    IsProductionV2 = false;
                                }
                            }

                            //WriteLog(" ConsoleLog", "\\n  Before Test Certificate ");
                            ResultMessage resultMsg = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePathV2, saveLocationV2, Constants.ReportE.PDF_DONT_OPEN, IsProductionV2);
                            //WriteLog(" ConsoleLog", "\\n After Test Certificate " + resultMsg.Result );
                            if (resultMessage.MessageType == ResultMessageE.Information)
                            {
                                string[] PDFfileNames = new string[2];
                                PDFfileNames[1] = resultMsg.Tag.ToString();
                                PDFfileNames[0] = resultMessage.Tag.ToString();
                                //download location for rewrite  template file
                                String saveLocationFinal = AppDomain.CurrentDomain.BaseDirectory + fileNameV2 + "_V2" + ".pdf";
                                //WriteLog(" ConsoleLog", "\\n  saveLocationFinal " + saveLocationFinal);
                                //string OutputFile = "E:\\Simpli Work Mask BC\\Simpli Deliver Generic\\ODLMAPICode\\ODLMWebAPI\\bin\\Debug\\netcoreapp2.0\\Test.pdf";
                                DirectoryInfo dirInfo = Directory.GetParent(saveLocationFinal);
                                if (!Directory.Exists(dirInfo.FullName))
                                {
                                    Directory.CreateDirectory(dirInfo.FullName);
                                }
                                //WriteLog(" ConsoleLog", "\\n  Before Merge PDF ");
                                int pdfResult = MargeMultiplePDF(PDFfileNames, saveLocationFinal);
                                //WriteLog(" ConsoleLog", "\\n  After Merge PDF Result " + pdfResult);
                                if (pdfResult != 1)
                                {
                                    //WriteLog(" ConsoleLog", "\\n  After Merge PDF Result in result " + pdfResult);
                                    resultMessage.Text = "Error in MargeMultiplePDF()";
                                    resultMessage.DisplayMessage = "Error in MargeMultiplePDF()";
                                    resultMessage.Result = 0;
                                }
                                //WriteLog(" ConsoleLog", "\\n saveLocationFinal  " + saveLocationFinal);
                                resultMessage.Tag = saveLocationFinal;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //WriteLog(" ConsoleLog", "\\n saveLocationFinal Exception " + ex.GetBaseException().ToString ());
                        resultMessage.Text = "Something wents wrong please try again";
                        resultMessage.DisplayMessage = "Something wents wrong please try again";
                        resultMessage.Result = 0;
                    }
                    String filePath = String.Empty;
                    //WriteLog(" ConsoleLog", "\\n resultMessage  " + resultMessage.Tag.ToString());

                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {
                        filePath = resultMessage.Tag.ToString();
                    }
                    if (isFileDelete)
                    {
                        //driveName + path;
                        Byte[] bytes = DeleteFile(saveLocation, filePath);

                        if (bytes != null && bytes.Length > 0)
                        {
                            resultMessage.Tag = Convert.ToBase64String(bytes);
                        }
                    }
                    else
                        resultMessage.Tag = filePath;
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.DefaultSuccessBehaviour();
                    }

                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
                //if (isFileDelete)
                //{
                //    //Reshma Added FOr WhatsApp integration.
                //      resultMessage  = SendFileOnWhatsAppAfterEwayBillGeneration(tblInvoiceTO.IdInvoice);
                //}
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }
        public  bool WriteLog(string strFileName, string strMessage)
        {
            try
            {
                FileStream objFilestream = new FileStream(string.Format("{0}\\{1}", Path.GetTempPath(), strFileName), FileMode.Append, FileAccess.Write);
                StreamWriter objStreamWriter = new StreamWriter((Stream)objFilestream);
                objStreamWriter.Write(strMessage);
                objStreamWriter.Close();
                objFilestream.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Cancel ewaybill.
        /// </summary>
        public ResultMessage CancelEWayBill(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false)
        {
            ResultMessage resultMessage = new ResultMessage();
            string sellerGstin = "27AACCK4472B1ZS";

            TblInvoiceTO tblInvoiceTO = new TblInvoiceTO();
            tblInvoiceTO = SelectTblInvoiceTO(idInvoice);
            if (tblInvoiceTO == null)
            {
                throw new Exception("InvoiceTO is null");
            }

            if (tblInvoiceTO.IsEInvGenerated != 1)
            {
                resultMessage.Text = "EInvoice is not generated for this invoice.";
                resultMessage.DisplayMessage = "EInvoice is not generated for this invoice.";
                resultMessage.Result = 0;
                resultMessage.MessageType = ResultMessageE.Error;
                return resultMessage;
            }

            List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = _iTblOrgLicenseDtlBL.SelectAllTblOrgLicenseDtlList(tblInvoiceTO.InvFromOrgId);
            if (TblOrgLicenseDtlTOList != null)
            {
                for (int i = 0; i <= TblOrgLicenseDtlTOList.Count - 1; i++)
                {
                    if (TblOrgLicenseDtlTOList[i].LicenseId == (Int32)CommercialLicenseE.IGST_NO)
                    {
                        sellerGstin = TblOrgLicenseDtlTOList[i].LicenseValue.ToUpper();
                        break;
                    }
                }
            }

            string access_token_OauthToken = null;
            resultMessage = EInvoice_OauthToken(loginUserId, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_OauthToken");
            }

            access_token_OauthToken = resultMessage.Tag.ToString();
            if (access_token_OauthToken == null)
            {
                throw new Exception("access_token_OauthToken is null");
            }

            string access_token_Authentication = null;
            resultMessage = EInvoice_Authentication(loginUserId, access_token_OauthToken, sellerGstin, forceToGetToken, tblInvoiceTO.InvFromOrgId);
            if (resultMessage.Result != 1)
            {
                throw new Exception("Error in EInvoice_Authentication");
            }

            access_token_Authentication = resultMessage.Tag.ToString();
            if (access_token_Authentication == null)
            {
                throw new Exception("access_token_Authentication is null");
            }

            return EInvoice_CancelEWayBill(tblInvoiceTO, loginUserId, access_token_Authentication, sellerGstin);
        }

        public ResultMessage EInvoice_CancelEWayBill(TblInvoiceTO tblInvoiceTO, Int32 loginUserId, string access_token_Authentication, string sellerGstin)
        {
            ResultMessage resultMsg = new ResultMessage();
            if (access_token_Authentication == "")
            {
                resultMsg = EInvoice_Authentication(loginUserId, "", sellerGstin, false, tblInvoiceTO.InvFromOrgId);
                if (resultMsg.Result != 1)
                {
                    throw new Exception("Error in EInvoice_Authentication");
                }
                else
                {
                    access_token_Authentication = resultMsg.Tag.ToString();
                }
            }

            TblEInvoiceApiTO tblEInvoiceApiTO = GetTblEInvoiceApiTO((int)EInvoiceAPIE.CANCEL_EWAYBILL);
            if (tblEInvoiceApiTO == null)
            {
                throw new Exception("EInvoiceApiTO is null");
            }

            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@gstin", sellerGstin);
            tblEInvoiceApiTO.HeaderParam = tblEInvoiceApiTO.HeaderParam.Replace("@token", access_token_Authentication);

            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@irnNo", tblInvoiceTO.IrnNo);
            tblEInvoiceApiTO.BodyParam = tblEInvoiceApiTO.BodyParam.Replace("@ewbNo", tblInvoiceTO.ElectronicRefNo);

            IRestResponse response = CallRestAPIs(tblEInvoiceApiTO.ApiBaseUri + tblEInvoiceApiTO.ApiFunctionName, tblEInvoiceApiTO.ApiMethod, tblEInvoiceApiTO.HeaderParam, tblEInvoiceApiTO.BodyParam);

            string EwayBillNo = null;
            bool bPeriodLapsed = false;
            JObject json = JObject.Parse(response.Content);
            if (json.ContainsKey("data"))
            {
                JObject jsonData = JObject.Parse(json["data"].ToString());
                EwayBillNo = (string)jsonData["ewayBillNo"];
            }
            if (json.ContainsKey("error"))
            {
                JArray arrError = JArray.Parse(json["error"].ToString());
                foreach (var err in arrError)
                {
                    JObject jsonError = JObject.Parse(err.ToString());
                    string errorCodes = (string)jsonError["errorCodes"];
                    string errorMsg = (string)jsonError["errorMsg"];
                    if (errorCodes == "1005" && errorMsg == "Invalid Token")
                    {
                        CancelEWayBill(loginUserId, tblInvoiceTO.IdInvoice, true);
                        return null;
                    }
                    else if (errorCodes == "315")
                    {
                        bPeriodLapsed = true;
                    }
                }
            }

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                resultMsg = InsertIntoTblEInvoiceApiResponse(tblEInvoiceApiTO.IdApi, tblInvoiceTO.IdInvoice, response, loginUserId, conn, tran);

                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                if (bPeriodLapsed == true)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = "You cannot cancel eWayBill after 24 hrs of generation.";
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }
                if (EwayBillNo == null)
                {
                    tran.Commit();
                    resultMsg.DefaultBehaviour(json.ToString());
                    resultMsg.DisplayMessage = json.ToString();
                    resultMsg.Text = resultMsg.DisplayMessage;
                    return resultMsg;
                }

                tblInvoiceTO.ElectronicRefNo = "";
                tblInvoiceTO.IsEWayBillGenerated = 0;
                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                resultMsg = UpdateTempInvoiceForEWayBill(tblInvoiceTO, conn, tran);

                if (resultMsg.Result != 1)
                {
                    tran.Rollback();
                    return resultMsg;
                }

                resultMsg.DisplayMessage = "eWayBill cancelled successfully;";
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "EInvoice_CancelEwayBill");
            }
            finally
            {
                conn.Close();
            }
            return resultMsg;
        }
        #endregion

    }
}

