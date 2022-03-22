using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.IoT.Interfaces;
using static ODLMWebAPI.StaticStuff.Constants;
using QRCoder;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using OfficeOpenXml;
using System.IO;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ODLMWebAPI.Controllers
{
    
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private readonly ITblInvoiceBL _iTblInvoiceBL;
        private readonly ITblInvoiceAddressBL _iTblInvoiceAddressBL;
        private readonly ITblInvoiceItemDetailsBL _iTblInvoiceItemDetailsBL;
        private readonly ITblInvoiceItemTaxDtlsBL _iTblInvoiceItemTaxDtlsBL;
        private readonly ITblProdGstCodeDtlsBL _iTblProdGstCodeDtlsBL;
        private readonly ITblGstCodeDtlsBL _iTblGstCodeDtlsBL;
        private readonly ITblTaxRatesBL _iTblTaxRatesBL;
        private readonly ITblInvoiceHistoryBL _iTblInvoiceHistoryBL;
        private readonly ITblUserBL _iTblUserBL;
        private readonly IDimensionBL _iDimensionBL;
        private readonly ITempInvoiceDocumentDetailsBL _iTempInvoiceDocumentDetailsBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblLoadingBL _iTblLoadingBL;
        private readonly ICommon _iCommon;
        private readonly ITblLoadingSlipBL _iTblLoadingSlipBL;
        private readonly IIotCommunication _iIotCommunication;

        public InvoiceController(IIotCommunication iIotCommunication,ITblLoadingSlipBL iTblLoadingSlipBL,ITblLoadingBL iTblLoadingBL, ITblConfigParamsBL iTblConfigParamsBL, ITempInvoiceDocumentDetailsBL iTempInvoiceDocumentDetailsBL, IDimensionBL iDimensionBL, ITblUserBL iTblUserBL, ITblInvoiceHistoryBL iTblInvoiceHistoryBL, ITblTaxRatesBL iTblTaxRatesBL, ITblGstCodeDtlsBL iTblGstCodeDtlsBL, ITblProdGstCodeDtlsBL iTblProdGstCodeDtlsBL, ITblInvoiceItemTaxDtlsBL iTblInvoiceItemTaxDtlsBL, ITblInvoiceItemDetailsBL iTblInvoiceItemDetailsBL, ITblInvoiceAddressBL iTblInvoiceAddressBL, ICommon iCommon, ITblInvoiceBL iTblInvoiceBL)
        {
            _iTblInvoiceBL = iTblInvoiceBL;
            _iTblInvoiceAddressBL = iTblInvoiceAddressBL;
            _iTblInvoiceItemDetailsBL = iTblInvoiceItemDetailsBL;
            _iTblInvoiceItemTaxDtlsBL = iTblInvoiceItemTaxDtlsBL;
            _iTblProdGstCodeDtlsBL = iTblProdGstCodeDtlsBL;
            _iTblGstCodeDtlsBL = iTblGstCodeDtlsBL;
            _iTblTaxRatesBL = iTblTaxRatesBL;
            _iTblInvoiceHistoryBL = iTblInvoiceHistoryBL;
            _iTblUserBL = iTblUserBL;
            _iDimensionBL = iDimensionBL;
            _iTempInvoiceDocumentDetailsBL = iTempInvoiceDocumentDetailsBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblLoadingBL = iTblLoadingBL;
            _iCommon = iCommon;
            _iTblLoadingSlipBL = iTblLoadingSlipBL;
            _iIotCommunication = iIotCommunication;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
        [Route("GetInvoiceList")]
        [HttpGet]
        public List<TblInvoiceTO> GetInvoiceList(string fromDate, string toDate, int isConfirm, Int32 cnfId,  Int32 dealerID, String userRoleTOList, Int32 brandId = 0, Int32 invoiceId = 0, Int32 statusId = 0,String internalOrgId="")
       {
            try
            {

                DateTime frmDt = DateTime.MinValue;
                DateTime toDt = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                {
                    frmDt = Convert.ToDateTime(fromDate);
                }
                if (Constants.IsDateTime(toDate))
                {
                    toDt = Convert.ToDateTime(toDate);
                }

                if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                    frmDt = _iCommon.ServerDateTime.Date;
                if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                    toDt = _iCommon.ServerDateTime.Date;

                List<TblUserRoleTO> tblUserRoleTOList = JsonConvert.DeserializeObject<List<TblUserRoleTO>>(userRoleTOList);
                //if(internalOrgId >0)
                //{
                //return _iTblInvoiceBL.SelectAllTblInvoiceList(frmDt, toDt, isConfirm, cnfId, dealerID, tblUserRoleTOList, brandId, invoiceId,statusId)
                //.Where(e=>e.InvFromOrgId==internalOrgId).ToList();   
                //}
                return _iTblInvoiceBL.SelectAllTblInvoiceList(frmDt, toDt, isConfirm, cnfId, dealerID, tblUserRoleTOList, brandId, invoiceId, statusId, internalOrgId);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        [Route("GetInvoiceDetails")]
        [HttpGet]
        public TblInvoiceTO GetInvoiceDetails(Int32 invoiceId)
        {
            try
            {
                string historyDetails= string.Empty;
                TblInvoiceTO invoiceTO = _iTblInvoiceBL.SelectTblInvoiceTO(invoiceId);
                if (invoiceTO != null)
                {
                    invoiceTO.InvoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);
                    List<TblInvoiceItemDetailsTO> itemList = _iTblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceId);
                    invoiceTO.InvoiceItemDetailsTOList = itemList;
                    if (itemList != null)
                    {
                        for (int i = 0; i < itemList.Count; i++)
                        {
                            itemList[i].InvoiceItemTaxDtlsTOList = _iTblInvoiceItemTaxDtlsBL.SelectAllTblInvoiceItemTaxDtlsList(itemList[i].IdInvoiceItem);

                        }

                        if (invoiceTO.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                        {

                            //TblLoadingSlipTO tblLoadingSlipTO = _iTblLoadingSlipBL.SelectTblLoadingSlipTO(invoiceTO.LoadingSlipId);
                            //tblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetails(tblLoadingSlipTO.IdLoadingSlip);

                            //int configId = _iTblConfigParamsBL.IotSetting();

                            
                            ////add Iot settings


                            //// _iIotCommunication.GetItemDataFromIotForGivenLoadingSlip(tblLoadingSlipTO);
                            //// invoiceTO.VehicleNo = tblLoadingSlipTO.VehicleNo;

                            //for (int i = 0; i < itemList.Count; i++)
                            //{
                            //    if (tblLoadingSlipTO != null && tblLoadingSlipTO.LoadingSlipExtTOList != null)
                            //    {
                            //        for (int j = 0; j < tblLoadingSlipTO.LoadingSlipExtTOList.Count; j++)
                            //        {
                            //            if (itemList[i].LoadingSlipExtId == tblLoadingSlipTO.LoadingSlipExtTOList[j].IdLoadingSlipExt)
                            //            {
                            //                itemList[i].Bundles = tblLoadingSlipTO.LoadingSlipExtTOList[j].Bundles.ToString();
                            //                itemList[i].InvoiceQty = Math.Round(tblLoadingSlipTO.LoadingSlipExtTOList[j].LoadedWeight * 0.001, 3);

                            //            }
                            //        }
                            //    }
                            //}
                            /*GJ@20170929 : To get the History Details for Approval and Acceptance*/
                            //if (invoiceTO.StatusId == (int)Constants.InvoiceStatusE.PENDING_FOR_AUTHORIZATION || invoiceTO.StatusId == (int)Constants.InvoiceStatusE.PENDING_FOR_ACCEPTANCE)
                            //{
                            if (invoiceTO.InvoiceModeE != InvoiceModeE.MANUAL_INVOICE)
                            {
                                //Sanjay [30-May-2019] Conditions added for auth invoice. If type is firm and auth then data wil be written to DB else on IoT
                                if (invoiceTO.IsConfirmed == 0)
                                    _iTblInvoiceBL.SetGateAndWeightIotData(invoiceTO, 0);
                                else if (invoiceTO.InvoiceStatusE != InvoiceStatusE.AUTHORIZED && invoiceTO.InvoiceStatusE != InvoiceStatusE.CANCELLED)
                                    _iTblInvoiceBL.SetGateAndWeightIotData(invoiceTO, 0);

                            }
                        }

                        //invoiceTO.InvoiceItemDetailsTOList = itemList;

                        //Saket [2017-11-21]
                        String strProdGstCode = String.Join(",", invoiceTO.InvoiceItemDetailsTOList.Select(s => s.ProdGstCodeId.ToString()).ToArray());

                        List<TblProdGstCodeDtlsTO> tblProdGstCodeDtlsTOList = _iTblProdGstCodeDtlsBL.SelectTblProdGstCodeDtlsTOList(strProdGstCode);

                        //List <TblGstCodeDtlsTO> tblGstCodeDtlsTOList = BL._iTblGstCodeDtlsBL.SelectTblGstCodeDtlsTOList(strProdGstCode);

                        for (int p = 0; p < itemList.Count; p++)
                        {

                            TblProdGstCodeDtlsTO tblProdGstCodeDtlsTO = tblProdGstCodeDtlsTOList.Where(w => w.IdProdGstCode == itemList[p].ProdGstCodeId).FirstOrDefault();
                            if (tblProdGstCodeDtlsTO != null)
                            {
                                if (tblProdGstCodeDtlsTO.GstCodeId > 0)
                                {
                                    itemList[p].GstCodeDtlsTO = _iTblGstCodeDtlsBL.SelectTblGstCodeDtlsTO(tblProdGstCodeDtlsTO.GstCodeId);
                                    itemList[p].GstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesBL.SelectAllTblTaxRatesList(tblProdGstCodeDtlsTO.GstCodeId);
                                }
                            }
                        }

                        List<TblInvoiceHistoryTO> InvoiceHistoryTOList = new List<TblInvoiceHistoryTO>();
                        InvoiceHistoryTOList = _iTblInvoiceHistoryBL.SelectAllTblInvoiceHistoryById(invoiceTO.IdInvoice, true);
                        if (InvoiceHistoryTOList != null && InvoiceHistoryTOList.Count > 0)
                        {
                            for (int i = 0; i < InvoiceHistoryTOList.Count; i++)
                            {
                                string editedBy = string.Empty;
                                TblInvoiceHistoryTO element = InvoiceHistoryTOList[i];
                                TblUserTO tblUserTo = _iTblUserBL.SelectTblUserTO(element.CreatedBy);
                                if (tblUserTo != null)
                                {
                                    editedBy = tblUserTo.UserDisplayName;
                                }
                                if (element.OldBillingAddr != null && element.NewBillingAddr != null)
                                {
                                    if (string.IsNullOrEmpty(historyDetails))
                                    {
                                        historyDetails = "Billing Address " + "!" + editedBy + "!" + element.OldBillingAddr + "!" + element.NewBillingAddr;
                                    }
                                    else
                                    {
                                        historyDetails += "::" + "Billing Address " + "!" + editedBy + "!" + element.OldBillingAddr + "!" + element.NewBillingAddr;
                                    }
                                }
                                if (element.OldConsinAddr != null && element.NewConsinAddr != null)
                                {
                                    if (string.IsNullOrEmpty(historyDetails))
                                    {
                                        historyDetails = "Consignee Address " + "!" + editedBy + "!" + element.OldConsinAddr + "!" + element.NewConsinAddr;
                                    }
                                    else
                                    {
                                        historyDetails += "::" + "Consignee Address " + "!" + editedBy + "!" + element.OldConsinAddr + "!" + element.NewConsinAddr;
                                    }
                                }
                            }

                            for (int i = 0; i < itemList.Count; i++)
                            {
                                //InvoiceHistoryTOList = _iTblInvoiceHistoryBL.SelectAllTblInvoiceHistoryById(itemList[i].IdInvoiceItem);
                                List<TblInvoiceHistoryTO> filteredInvoiceList = InvoiceHistoryTOList.Where(p => p.InvoiceItemId == itemList[i].IdInvoiceItem).ToList();

                                if (filteredInvoiceList != null && filteredInvoiceList.Count > 0)
                                {
                                    for (int j = 0; j < filteredInvoiceList.Count; j++)
                                    {
                                        itemList[i].ChangeIn = "Rate";
                                        TblInvoiceHistoryTO element = filteredInvoiceList[j];
                                        string editedBy = string.Empty;
                                        TblUserTO tblUserTo = _iTblUserBL.SelectTblUserTO(element.CreatedBy);
                                        if (tblUserTo != null)
                                        {
                                            editedBy = tblUserTo.UserDisplayName;
                                        }
                                        //historyDetails = string.IsNullOrEmpty(historyDetails)? 
                                        if (element.OldUnitRate != 0 && element.NewUnitRate != 0)
                                        {
                                            itemList[i].ChangeIn = itemList[i].ChangeIn == "" || string.IsNullOrEmpty(itemList[i].ChangeIn) ? "Rate" : itemList[i].ChangeIn + "|" + "Rate";
                                            if (string.IsNullOrEmpty(historyDetails))
                                            {
                                                historyDetails = itemList[i].ProdItemDesc + " (Rate)" + "!" + editedBy + "!" + element.OldUnitRate + "!" + element.NewUnitRate;
                                            }
                                            else
                                            {
                                                historyDetails += "::" + itemList[i].ProdItemDesc + " (Rate)" + "!" + editedBy + "!" + element.OldUnitRate + "!" + element.NewUnitRate;
                                            }
                                        }
                                        if (element.OldCdStructureId != 0 && element.NewCdStructureId != 0)
                                        {
                                            List<DropDownTO> cdStructureList = _iDimensionBL.SelectCDStructureForDropDown(0);
                                            var vOldRes = cdStructureList.Where(p => p.Value == element.OldCdStructureId).ToList();
                                            var vNewRes = cdStructureList.Where(p => p.Value == element.NewCdStructureId).ToList();
                                            itemList[i].ChangeIn = itemList[i].ChangeIn == "" || string.IsNullOrEmpty(itemList[i].ChangeIn) ? "CD" : itemList[i].ChangeIn + "|" + "CD";

                                            if (string.IsNullOrEmpty(historyDetails))
                                            {
                                                historyDetails = itemList[i].ProdItemDesc + " (CD)" + "!" + editedBy + "!" + (vOldRes.Count > 0 ? vOldRes[0].Text : "0") + "!" + (vNewRes.Count > 0 ? vNewRes[0].Text : "0");
                                            }
                                            else
                                            {
                                                historyDetails += "::" + itemList[i].ProdItemDesc + " (CD)" + "!" + editedBy + "!" + (vOldRes.Count > 0 ? vOldRes[0].Text : "0") + "!" + (vNewRes.Count > 0 ? vNewRes[0].Text : "0");
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        // }
                    }

                    invoiceTO.HistoryDetails = historyDetails;
                }

                return invoiceTO;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Ramdas.W:@22092017:API This method is used to Get List of Invoice By Status
        /// </summary>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        [Route("GetInvoiceListByStatus")]
        [HttpGet]
        public List<TblInvoiceTO> GetInvoiceListByStatus(int statusId, int distributorOrgId, int invoiceId,int isConfirm=2)
        {
            try
            {
                return _iTblInvoiceBL.SelectTblInvoiceByStatus(statusId, distributorOrgId, invoiceId, isConfirm);
               
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }

        }


        /// <summary>
        /// Vijaymala[15-09-2017] Added To Get Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>

        [Route("GetRptInvoiceList")]
        [HttpGet]
        public ResultMessage  GetRptInvoiceList(string fromDate, string toDate, int isConfirm,int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            return  _iTblInvoiceBL.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
        }

        [Route("GetRptNCInvoiceList")]
        [HttpGet]
        public ResultMessage GetRptNCInvoiceList(int isConfirm, int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue; 
            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date .AddDays (-1);
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            frmDt = frmDt.AddMinutes(530);
            toDt = toDt .AddMinutes(530);

            return _iTblInvoiceBL.SelectAllRptInvoiceList(frmDt, toDt, isConfirm, fromOrgId);
        }

        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>

        [Route("GetInvoiceExportList")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetInvoiceExportList(string fromDate, string toDate, int isConfirm,int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;


            return _iTblInvoiceBL.SelectInvoiceExportList(frmDt, toDt, isConfirm, fromOrgId);
        }

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate HSN Excel
        /// </summary>
        /// <returns></returns>

        [Route("GetHsnExportList")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetHsnExportList(string fromDate, string toDate, int isConfirm,int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            return _iTblInvoiceBL.SelectHsnExportList(frmDt, toDt, isConfirm, fromOrgId);
        }


        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>

        [Route("GetSalesInvoiceListForReport")]
        [HttpGet]
        public List<TblInvoiceRptTO> GetSalesInvoiceListForReport(string fromDate, string toDate, int isConfirm, int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            return _iTblInvoiceBL.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm, fromOrgId);
        }

        //For Metaroll changes in Item Wise sales export C report
        [Route("GetItemWiseSalesExportCListForReport")]
        [HttpGet]
        public ResultMessage GetItemWiseSalesExportCListForReport(string fromDate, string toDate, int isConfirm, int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            return _iTblInvoiceBL.SelectItemWiseSalesExportCListForReport(frmDt, toDt, isConfirm, fromOrgId);
        }

        [Route("PrintSaleReport")]
        [HttpGet]
        public ResultMessage PrintSaleReport(string fromDate, string toDate, int isConfirm, string selectedOrg, int isFromPurchase)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            if (string.IsNullOrEmpty(selectedOrg))
            {
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (tblConfigParamsTO != null)
                {
                    selectedOrg = Convert.ToString(tblConfigParamsTO.ConfigParamVal) + ",0";
                }
            }
            return _iTblInvoiceBL.PrintSaleReport(frmDt, toDt, isConfirm, selectedOrg, isFromPurchase);
        }



        [Route("GetOtherItemListForReport")]
        [HttpGet]
        public List<TblOtherTaxRpt> GetOtherItemListForReport(string fromDate, string toDate, int isConfirm, int otherTaxId,int fromOrgId)
        {
            DateTime frmDt = DateTime.MinValue;
            DateTime toDt = DateTime.MinValue;
            if (Constants.IsDateTime(fromDate))
            {
                frmDt = Convert.ToDateTime(fromDate);

            }
            if (Constants.IsDateTime(toDate))
            {
                toDt = Convert.ToDateTime(toDate);
            }

            if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                frmDt = _iCommon.ServerDateTime.Date;
            if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                toDt = _iCommon.ServerDateTime.Date;
            return _iTblInvoiceBL.SelectOtherTaxDetailsReport(frmDt, toDt, isConfirm, otherTaxId, fromOrgId);
        }


        /// <summary>
        /// Sudhir Added For get InvoiceTO based on loadingSlipId
        /// </summary>
        /// <param name="loadingSlipId"></param>
        /// <returns></returns>
        [Route("GetInvoiceDetailsByLoadingSlipId")]
        [HttpGet]
        public List<TblInvoiceTO> GetInvoiceDetailsByLoadingSlipId(Int32 loadingSlipId)
        {
            return _iTblInvoiceBL.SelectInvoiceTOListFromLoadingSlipId(loadingSlipId);
        }

        /// <summary>
        /// Vijaymala[12-04-2018] : Added to get invoice list by using vehicle number
        /// </summary>
        /// <returns></returns>
        [Route("GetAllInvoiceListByVehicleNo")]
        [HttpGet]
        public List<TblInvoiceTO> GetAllInvoiceListByVehicleNo(string vehicleNo,string fromDate, string toDate)
        {
            try
            {
                DateTime frmDt = DateTime.MinValue;
                DateTime toDt = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                {
                    frmDt = Convert.ToDateTime(fromDate);

                }
                if (Constants.IsDateTime(toDate))
                {
                    toDt = Convert.ToDateTime(toDate);
                }

                if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                    frmDt = _iCommon.ServerDateTime.Date;
                if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                    toDt = _iCommon.ServerDateTime.Date;

                return _iTblLoadingBL.SelectAllInvoiceListByVehicleNo(vehicleNo, frmDt , toDt);
               
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Route("GetInvoiceAddressList")]
        [HttpGet]
        public List<TblInvoiceAddressTO> GetInvoiceAddressList(Int32 invoiceId)
        {
            try
            {
                List<TblInvoiceAddressTO> invoiceAddressTOList = new List<TblInvoiceAddressTO>();

                if (invoiceId != null)
                {
                    invoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);
                }
                return invoiceAddressTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
        /// <summary>
        /// Vijaymala added[09-05-2018] :To get notified invoice list
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="isConfirm"></param>
        /// <returns></returns>

        [Route("GetAllTNotifiedblInvoiceList")]
        [HttpGet]
        public List<TblInvoiceTO> GetAllTNotifiedblInvoiceList(string fromDate, string toDate,  int isConfirm, int fromOrgId)
        {
            try
            {
                DateTime frmDt = DateTime.MinValue;
                DateTime toDt = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                {
                    frmDt = Convert.ToDateTime(fromDate);
                }
                if (Constants.IsDateTime(toDate))
                {
                    toDt = Convert.ToDateTime(toDate);
                }

                if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                    frmDt = _iCommon.ServerDateTime.Date;
                if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                    toDt = _iCommon.ServerDateTime.Date;

                return _iTblInvoiceBL.SelectAllTNotifiedblInvoiceList(frmDt, toDt,isConfirm, fromOrgId);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        /// <summary>
        /// AmolG[2022-Feb-14] For Invoice Size wise Report
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [Route("GetAllInvoices")]
        [HttpGet]
        public List<InvoiceReportTO> GetAllInvoices(string fromDate, string toDate)
        {
            try
            {
                DateTime frmDt = DateTime.MinValue;
                DateTime toDt = DateTime.MinValue;
                if (Constants.IsDateTime(fromDate))
                {
                    frmDt = Convert.ToDateTime(fromDate);
                }
                if (Constants.IsDateTime(toDate))
                {
                    toDt = Convert.ToDateTime(toDate);
                }

                if (Convert.ToDateTime(frmDt) == DateTime.MinValue)
                    frmDt = _iCommon.ServerDateTime.Date;
                if (Convert.ToDateTime(toDt) == DateTime.MinValue)
                    toDt = _iCommon.ServerDateTime.Date;

                String errorMsg = "";
                return _iTblInvoiceBL.GetAllInvoices(frmDt, toDt, ref errorMsg);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        [Route("GetInvoiceDocumentDetails")]
        [HttpGet]
        public List<TempInvoiceDocumentDetailsTO> GetInvoiceDocumentDetails(Int32 invoiceId)
        {
            try
            {
                return _iTempInvoiceDocumentDetailsBL.SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(invoiceId);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }
        [HttpGet]
        [Route("GenerateInvoiceNumberForManual")]
        public TblEntityRangeTO GenerateInvoiceNumberForManual(Int32 idInvoice)
        {
            return _iTblInvoiceBL.GenerateInvoiceNumberFromEntityRange(idInvoice);
        }

     //Aniket [22-4-2019]
     [HttpGet]
     [Route("SelectAllInvoiceAddrById")]
     public List<TblInvoiceAddressTO> SelectAllInvoiceAddrById(Int32 dealerId, String addrSrcTypeString)
        {
            return _iTblInvoiceBL.SelectTblInvoiceAddressByDealerId(dealerId, addrSrcTypeString);
        }
             

        /// <summary>
        /// Ramdas.W:14092017:API This method is used to Added new Invoice 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostInvoice")]
        [HttpPost]
        public ResultMessage PostInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    tblInvoiceTO.CreatedBy = Convert.ToInt32(loginUserId);
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "loginUserId Found 0";
                    return resultMessage;
                }
                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblInvoiceTO.CreatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.CreatedOn = serverDate;
                    tblInvoiceTO.InvoiceDate = serverDate;
                    tblInvoiceTO.StatusDate = serverDate;
                    tblInvoiceTO.InvoiceStatusE = Constants.InvoiceStatusE.NEW;
                    //tblInvoiceTO.InvoiceModeE = Constants.InvoiceModeE.MANUAL_INVOICE;

                    tblInvoiceTO.DeliveredOn = tblInvoiceTO.StatusDate;//viaymala added

                    // tblInvoiceTO.IsActive = 1;

                    return _iTblInvoiceBL.InsertTblInvoice(tblInvoiceTO);
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Invoice ";
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Result = -1;
                resultMessage.Exception = ex;
                resultMessage.Text = "Exception Error IN API Call PostNewInvoice";
                return resultMessage;
            }


        }

       

        [Route("PostEditInvoice")]
        [HttpPost]
        public ResultMessage PostEditInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;

                    return _iTblInvoiceBL.SaveUpdatedInvoice(tblInvoiceTO);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoice");
                return resultMessage;
            }

        }


        [Route("PostexchangeInvoice")]
        [HttpPost]
        public ResultMessage exchangeInvoice(int invoiceId,int invGenerateModeId,int fromOrgId,int toOrgId=0, int isCalculateWithBaseRate = 0)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                return _iTblInvoiceBL.exchangeInvoice(invoiceId,invGenerateModeId,fromOrgId,toOrgId, isCalculateWithBaseRate);
            }
            catch(Exception e)
            {
                 resultMessage.DefaultExceptionBehaviour(e, "exchangeIvoice");
                return resultMessage;
            }
        }


        [Route("PostGenerateInvoiceNumber")]
        [HttpPost]
        public ResultMessage PostGenerateInvoiceNumber([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var invoiceId = data["invoiceId"].ToString();
                var invGenerateModeId = data["invGenerateModeId"].ToString();
                var taxInvoiceNumber  = data["taxInvoiceNumber"].ToString();
               
                String invComment = data["invComment"].ToString();
        
                int fromOrgId =0;
                int toOrgId =0;
                // try
                // {
                //      fromOrgId  = Convert.ToInt32(data["fromOrgId"].ToString());
           
                // }
                // catch(Exception e)
                // {
                // fromOrgId =0;
                // }
                

                // try
                // {
                //    toOrgId  = Convert.ToInt32(data["toOrgId"].ToString());
           
                // }
                // catch(Exception e)
                // {
                //   toOrgId =0   
                // }
                Int32 manualinvoiceno = Convert.ToInt32(data["ManualInvoiceNo"]);
                if (Convert.ToInt32(loginUserId) <= 0)
                { 
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (Convert.ToInt32(invoiceId) <= 0)
                {
                    resultMessage.DefaultBehaviour("invoiceId Not Found");
                    return resultMessage;
                }
               Int32 isConfirm = 1;
                return _iTblLoadingBL.GenerateInvoiceNumber(Convert.ToInt32(invoiceId), Convert.ToInt32(loginUserId), isConfirm, Convert.ToInt32(invGenerateModeId),fromOrgId,toOrgId,Convert.ToString(taxInvoiceNumber), manualinvoiceno,invComment);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostGenerateInvoiceNumber");
                return resultMessage;
            }
        }


        [Route("PostEditInvoiceForNonCommercialDtls")]
        [HttpPost]
        public ResultMessage PostEditInvoiceForNonCommercialDtls([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;

                    return _iTblInvoiceBL.UpdateInvoiceNonCommercialDetails(tblInvoiceTO);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForNonCommercialDtls");
                return resultMessage;
            }

        }

        /// <summary>
        /// add PostInvoiceFromMail to send mail @Kiran
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        [Route("PostInvoiceFromMail")]
        [HttpPost]
        public ResultMessage PostInvoiceFromMail([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();

            try
            {
                SendMail SendMailTo = JsonConvert.DeserializeObject<SendMail>(data["mailInformationTo"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (SendMailTo != null)
                {
                    int result = 0;
                    DateTime serverDate = _iCommon.ServerDateTime;
                    SendMailTo.CreatedBy = Convert.ToInt32(loginUserId);
                    result = _iTblInvoiceBL.sendInvoiceFromMail(SendMailTo);
                    if(result == 1){
                         resultMessage.DefaultSuccessBehaviour();
                        return resultMessage;
                    }
                    else{
                        resultMessage.DefaultBehaviour("Error in send mail");
                    }
                   
                }
                else{
                    resultMessage.DefaultBehaviour("SendMailTo Found NULL"); 
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostInvoiceFromMail");
                return resultMessage;
            }

        }

        /// <summary>
        /// GJ@20171001 : Post Edit ConfirmNonConfirm status with calculation
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("PostUpdateInvoiceConfirmNonConfirmDetails")]
        [HttpPost]
        public ResultMessage PostUpdateInvoiceConfirmNonConfirmDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                    tblInvoiceTO.UpdatedOn = serverDate;
                    
                    return _iTblLoadingBL.UpdateInvoiceConfrimNonConfirmDetails(tblInvoiceTO, tblInvoiceTO.UpdatedBy);
                }
                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostEditInvoiceForStatusConversion");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vijaymala [13-04-2017] :Added to combine invoices
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("PostComposeInvoice")]
        [HttpPost]
        public ResultMessage PostComposeInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                var loginUserId = data["loginUserId"].ToString();
                //var invoiceId = data["invoiceIdList"].ToString();

                List<Int32> invoiceIdsList = JsonConvert.DeserializeObject<List<Int32>>(data["invoiceIdsList"].ToString());


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (invoiceIdsList != null && invoiceIdsList.Count >0)
                {
                    return _iTblInvoiceBL.ComposeInvoice(invoiceIdsList, Convert.ToInt32(loginUserId));

                }
                else
                {
                    resultMessage.DefaultBehaviour("invoiceIdList Not Found");
                    return resultMessage;
                }

              
                return resultMessage;
                    
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostGenerateInvoiceNumber");
                return resultMessage;
            }
        }

        [Route("PostDecomposeInvoice")]
        [HttpPost]
        public ResultMessage PostDecomposeInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {

                var loginUserId = data["loginUserId"].ToString();
                //var invoiceId = data["invoiceIdList"].ToString();

                List<Int32> invoiceIdsList = JsonConvert.DeserializeObject<List<Int32>>(data["invoiceIdsList"].ToString());


                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (invoiceIdsList != null && invoiceIdsList.Count > 0)
                {
                    return _iTblInvoiceBL.DecomposeInvoice(invoiceIdsList, Convert.ToInt32(loginUserId));

                }
                else
                {
                    resultMessage.DefaultBehaviour("invoiceIdList Not Found");
                    return resultMessage;
                }


                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostGenerateInvoiceNumber");
                return resultMessage;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

     


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {


        }

        /// <summary>
        /// Vaibhav [29-Nov-2017] Added to separate transactional data.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Route("PostExtractEnquiryData")]
        [HttpPost]
        public ResultMessage PostExtractEnquiryData()
        {
            ResultMessage resultMessage = new ResultMessage();
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DATA_EXTRACTION_TYPE);
            if (tblConfigParamsTO == null)
            {
                resultMessage.DefaultBehaviour("Error tblConfigParamsTO is null");
                return null;
            }
            try
            {
                if (tblConfigParamsTO.ConfigParamVal == Constants.DataExtractionTypeE.IsRegular.ToString())
                {
                    return _iTblLoadingBL.ExtractEnquiryData();
                }
                else
                {
                    return _iTblLoadingBL.DeleteDispatchData();
                }
                
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostExtractEnquiryData");
                return resultMessage;
            }
        }

        /// <summary>
        /// Vaibhav [12-April-2018] Added to update invoice date.
        /// </summary>
        /// <returns></returns>
        
        [Route("PostUpdateInvoiceDate")]
        [HttpPost]
        public ResultMessage PostUpdateInvoiceDate([FromBody] JObject data)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                var loginUserId = data["loginUserId"].ToString();

                tblInvoiceTO.UpdatedBy = Convert.ToInt32(loginUserId);
                tblInvoiceTO.UpdatedOn = _iCommon.ServerDateTime;

                return _iTblInvoiceBL.UpdateInvoiceDate(tblInvoiceTO);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateInvoiceDate");
                return resultMessage;
            }
        }




        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("PostInvoiceDocumentDetails")]
        [HttpPost]
        public ResultMessage PostInvoiceDocumentDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TblInvoiceTO tblInvoiceTO = JsonConvert.DeserializeObject<TblInvoiceTO>(data["invoiceTO"].ToString());
                List<TblDocumentDetailsTO> tblDocumentDetailsTOList = JsonConvert.DeserializeObject<List<TblDocumentDetailsTO>>(data["invoiceDocumentDetailsTOList"].ToString());

                var loginUserId = data["loginUserId"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tblDocumentDetailsTOList == null && tblDocumentDetailsTOList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Error : Invoice Document Details List Found Empty Or Null");
                    return resultMessage;
                }

                if (tblInvoiceTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    return _iTblInvoiceBL.SaveInvoiceDocumentDetails(tblInvoiceTO, tblDocumentDetailsTOList, Convert.ToInt32(loginUserId));
                }

                else
                {
                    resultMessage.DefaultBehaviour("tblInvoiceTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostInvoiceDocumentDetails");
                return resultMessage;
            }

        }

        /// <summary>
        /// Vijaymala[22-05-2018] : Added To save invoice document details.
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("PostDeactivateInvoiceDocumentDetails")]
        [HttpPost]
        public ResultMessage PostDeactivateInvoiceDocumentDetails([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO = JsonConvert.DeserializeObject<TempInvoiceDocumentDetailsTO>(data["invoiceDocumentDetailsTO"].ToString());

                var loginUserId = data["loginUserId"].ToString();

                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }

                if (tempInvoiceDocumentDetailsTO != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    return _iTblInvoiceBL.DeactivateInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, Convert.ToInt32(loginUserId));
                }

                else
                {
                    resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found NULL");
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostDeactivateInvoiceDocumentDetails");
                return resultMessage;
            }

        }


        [Route("PrintInvoiceDetails")]
        [HttpPost]
        public ResultMessage PrintInvoiceDetails([FromBody] JObject data)
        {
            try
            {
                ResultMessage resultMessage = new StaticStuff.ResultMessage();
                var invoiceId = data["invoiceId"].ToString();
                var isPrinted =Convert.ToBoolean(data["isPrinted"]);
                // var firmNameId = data["firmId"].ToString();
                
                if (invoiceId != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    return _iTblInvoiceBL.PrintReport(Convert.ToInt32(invoiceId), isPrinted);
                        //, Convert.ToInt32(firmNameId));
                }

                else
                {
                    resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found NULL");
                    return resultMessage;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        [Route("PrintWeighingDetails")]
        [HttpPost]
        public ResultMessage PrintWeighingDetails([FromBody] JObject data)
        {
            try
            {
                ResultMessage resultMessage = new StaticStuff.ResultMessage();
                var invoiceId = data["invoiceId"].ToString();
                var reportType = data["reportType"].ToString();
                if (invoiceId != null)
                {
                    DateTime serverDate = _iCommon.ServerDateTime;
                    return _iTblInvoiceBL.PrintWeighingReport(Convert.ToInt32(invoiceId),false,reportType.ToString());
                }

                else
                {
                    resultMessage.DefaultBehaviour("tempInvoiceDocumentDetailsTO Found NULL");
                    return resultMessage;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }

        [Route("SendInvoiceEmail")]
        [HttpPost]
        public ResultMessage SendInvoiceEmail([FromBody] JObject data)
        {
            try
            {
                ResultMessage resultMessage = new StaticStuff.ResultMessage();

                SendMail mailInformationTo = JsonConvert.DeserializeObject<SendMail>(data["mailInformationTo"].ToString());

                if(mailInformationTo == null)
                {
                    resultMessage.DefaultBehaviour("mailInformationTo Found NULL");
                    return resultMessage;
                }
               
             return _iTblInvoiceBL.SendInvoiceEmail(mailInformationTo);
               
            }

            catch (Exception ex)
            {
                return null;
            }
            finally
            {

            }
        }


        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        [Route("GenerateEInvoice")]
        [HttpPost]
        public ResultMessage GenerateEInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                Int32 eInvoiceCreationType = Convert.ToInt32(data["generateEInvoiceTypeE"].ToString());
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                if (eInvoiceCreationType == 0)
                {
                    resultMessage.DefaultBehaviour("E-Invoice Creation type not found");
                    return resultMessage;
                }
                
                List<TblInvoiceAddressTO> tblInvoiceAddressTOList = JsonConvert.DeserializeObject<List<TblInvoiceAddressTO>>(data["invoiceAddressTOList"].ToString());
                resultMessage = _iTblInvoiceBL.UpdateInvoiceAddress(tblInvoiceAddressTOList);
                if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                {
                    return resultMessage;
                }

                if (eInvoiceCreationType == (Int32)Constants.EGenerateEInvoiceCreationType.UPDATE_ONLY_ADDRESS)
                {
                    return resultMessage;
                }
                
                return _iTblInvoiceBL.GenerateEInvoice(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice), Convert.ToInt32(eInvoiceCreationType));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[19-11-2020] : Added To Cancel eInvvoice.
        /// </summary>
        [Route("CancelEInvoice")]
        [HttpPost]
        public ResultMessage CancelEInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                return _iTblInvoiceBL.CancelEInvoice(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get And Update eInvvoice.
        /// </summary>
        [Route("GetAndUpdateEInvoice")]
        [HttpPost]
        public ResultMessage GetAndUpdateEInvoice([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                
                return _iTblInvoiceBL.GetAndUpdateEInvoice(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate EWayBill.
        /// </summary>
        [Route("GenerateEWayBill")]
        [HttpPost]
        public ResultMessage GenerateEWayBill([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                var distanceInKM = data["distanceInKM"].ToString();
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                return _iTblInvoiceBL.GenerateEWayBill(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice), Convert.ToDecimal(distanceInKM));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

        /// <summary>
        /// Dhananjay[19-11-2020] : Added To Cancel EWayBill.
        /// </summary>
        [Route("CancelEWayBill")]
        [HttpPost]
        public ResultMessage CancelEWayBill([FromBody] JObject data)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                var loginUserId = data["loginUserId"].ToString();
                var idInvoice = data["idInvoice"].ToString();
                //Int32 idInvoice = 29194;
                if (Convert.ToInt32(loginUserId) <= 0)
                {
                    resultMessage.DefaultBehaviour("loginUserId Not Found");
                    return resultMessage;
                }
                return _iTblInvoiceBL.CancelEWayBill(Convert.ToInt32(loginUserId), Convert.ToInt32(idInvoice));
            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = ex.Message;
                return resultMessage;
            }
            finally
            {

            }
        }

      


    }
}
