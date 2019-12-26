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
using static ODLMWebAPI.StaticStuff.Constants;

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
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblAddressBL _iTblAddressBL;
        private readonly ITblLoadingSlipBL _iTblLoadingSlipBL;
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
        private readonly ITblAlertDefinitionDAO _iTblAlertDefinitionDAO;
        public TblInvoiceBL(ITblAlertDefinitionDAO iTblAlertDefinitionDAO, ITblInvoiceChangeOrgHistoryDAO iTblInvoiceChangeOrgHistoryDAO, ITblConfigParamsDAO iTblConfigParamsDAO, ITblPaymentTermOptionRelationBL iTblPaymentTermOptionRelationBL, ITblPaymentTermsForBookingBL iTblPaymentTermsForBookingBL, ITblPaymentTermOptionRelationDAO iTblPaymentTermOptionRelationDAO, IDimBrandDAO iDimBrandDAO, ITblDocumentDetailsBL iTblDocumentDetailsBL, ITblBookingsBL iTblBookingsBL, ITblOrganizationBL iTblOrganizationBL, ITblInvoiceHistoryBL iTblInvoiceHistoryBL, IDimReportTemplateBL iDimReportTemplateBL, ITblAlertInstanceBL iTblAlertInstanceBL, ISendMailBL iSendMailBL, ICircularDependencyBL iCircularDependencyBL, ICommon iCommon, IConnectionString iConnectionString, ITblEmailHistoryDAO iTblEmailHistoryDAO, IRunReport iRunReport, ITblPersonDAO iTblPersonDAO, ITblBookingParitiesDAO iTblBookingParitiesDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblUserDAO iTblUserDAO, ITblInvoiceAddressDAO iTblInvoiceAddressDAO, ITblInvoiceOtherDetailsDAO iTblInvoiceOtherDetailsDAO, ITblInvoiceBankDetailsDAO iTblInvoiceBankDetailsDAO, ITblOtherTaxesDAO iTblOtherTaxesDAO, ITempInvoiceDocumentDetailsDAO iTempInvoiceDocumentDetailsDAO, ITblOrgLicenseDtlDAO iTblOrgLicenseDtlDAO, ITblTaxRatesDAO iTblTaxRatesDAO, ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO, ITblProdGstCodeDtlsDAO iTblProdGstCodeDtlsDAO, ITblProductItemDAO iTblProductItemDAO, ITblParitySummaryDAO iTblParitySummaryDAO, ITblWeighingMeasuresDAO iTblWeighingMeasuresDAO, ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO, ITblStockConfigDAO iTblStockConfigDAO, ITblLoadingSlipExtDAO iTblLoadingSlipExtDAO, IDimensionBL iDimensionBL, ITblLoadingDAO iTblLoadingDAO, ITempLoadingSlipInvoiceBL iTempLoadingSlipInvoiceBL, ITblLoadingSlipBL iTblLoadingSlipBL, ITblAddressBL iTblAddressBL, ITblInvoiceAddressBL iTblInvoiceAddressBL, ITblConfigParamsBL iTblConfigParamsBL, ITblInvoiceDAO iTblInvoiceDAO, ITblUserRoleBL iTblUserRoleBL, ITblInvoiceItemDetailsBL iTblInvoiceItemDetailsBL, ITblInvoiceItemTaxDtlsBL iTblInvoiceItemTaxDtlsBL, IDimensionDAO iDimensionDAO)
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
            _iTblAlertDefinitionDAO = iTblAlertDefinitionDAO;
            _iDimensionDAO = iDimensionDAO;
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
        public List<TblInvoiceTO> SelectAllTblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerID, List<TblUserRoleTO> tblUserRoleTOList, Int32 brandId, Int32 invoiceId, Int32 statusId,String internalOrgId)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return _iTblInvoiceDAO.SelectAllTblInvoice(frmDt, toDt, isConfirm, cnfId, dealerID, tblUserRoleTO, brandId, invoiceId, statusId, internalOrgId);
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
                return _iTblInvoiceDAO.SelectTblInvoiceByStatus(statusId, distributorOrgId, invoiceId, conn, tran, isConfirm);
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
        public List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm)
        {
            return _iTblInvoiceDAO.SelectAllRptInvoiceList(frmDt, toDt, isConfirm);
        }

        /// <summary>
        /// Vijaymala[06-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm)
        {
            return _iTblInvoiceDAO.SelectInvoiceExportList(frmDt, toDt, isConfirm);
        }

        /// <summary>
        /// Vijaymala[07-10-2017] Added To Get Invoice List To Generate Invoice Excel
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm)
        {
            return _iTblInvoiceDAO.SelectHsnExportList(frmDt, toDt, isConfirm);
        }

        /// <summary>
        /// Vijaymala[11-01-2018] Added To Get Sales Invoice List To Generate Report
        /// </summary>
        /// <returns></returns>
        public List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm)
        {
            return _iTblInvoiceDAO.SelectSalesInvoiceListForReport(frmDt, toDt, isConfirm);
        }

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
        public List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm)
        {
            return _iTblInvoiceDAO.SelectAllTNotifiedblInvoiceList(frmDt, toDt, isConfirm);
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

        public ResultMessage PrepareAndSaveNewTaxInvoice(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage resultMsg = new ResultMessage();
            string entityRangeName = string.Empty;

            List<DropDownTO> districtList = new List<DropDownTO>();
            List<DropDownTO> talukaList = new List<DropDownTO>();
            try
            {
                List<TblLoadingSlipTO> loadingSlipTOList = _iCircularDependencyBL.SelectAllLoadingSlipListWithDetails(loadingTO.IdLoading, conn, tran);
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

        private ResultMessage CreateInvoiceAgainstLoadingSlips(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran, List<TblLoadingSlipTO> loadingSlipTOList, Int32 skipMergeSetting = 0)
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
                    List<TblLoadingSlipExtTO> tblLoadingSlipExtTOList = tblLoadingSlipTOTemp.LoadingSlipExtTOList.Where(w => w.WeightMeasureId == 0).ToList();
                    if (tblLoadingSlipExtTOList != null && tblLoadingSlipExtTOList.Count > 0)
                    {
                        remove = true;
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
                String loadingSlipIds = String.Join(',', loadingSlipTOList.Select(s => s.IdLoadingSlip.ToString()).ToArray());
                if (!String.IsNullOrEmpty(loadingSlipIds))
                {
                    List<TblInvoiceTO> tblInvoiceTOListTemp = SelectInvoiceListFromLoadingSlipIds(loadingSlipIds, conn, tran);
                    if (tblInvoiceTOListTemp != null && tblInvoiceTOListTemp.Count > 0)
                    {
                        for (int t = 0; t < tblInvoiceTOListTemp.Count; t++)
                        {
                            loadingSlipTOList = loadingSlipTOList.Where(w => w.IdLoadingSlip != tblInvoiceTOListTemp[t].LoadingSlipId).ToList();
                        }
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
                TblInvoiceTO tblInvoiceTO = PrepareInvoiceAgainstLoadingSlip(loadingTO, conn, tran, internalOrgId, ofcAddrTO, rcmConfigParamsTO, invoiceDateConfigTO, loadingSlipTo);
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

        public TblInvoiceTO PrepareInvoiceAgainstLoadingSlip(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran, int internalOrgId, TblAddressTO ofcAddrTO, TblConfigParamsTO rcmConfigParamsTO, TblConfigParamsTO invoiceDateConfigTO, TblLoadingSlipTO loadingSlipTo)
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
            double taxableTotal = 0;
            Boolean isSez = false;
            Int32 result = 0;
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
                tblInvoiceTO.Narration = String.Empty;


            //tblInvoiceTO.InvFromOrgId = internalOrgId; //No need to aasign from loading Only use for BMM
            tblInvoiceTO.InvFromOrgId = internalOrgId;  //For 
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

                    //[05-03-2018]Vijaymala:Changes the code to change prodItemDesc as per Kalika and SRJ requirement 
                    Int32 a = 0;
                    // Int32 isHide = 0;
                    TblConfigParamsTO regulartemp = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DISPLAY_BRAND_ON_INVOICE, conn, tran);
                    if (regulartemp != null)
                    {
                        a = Convert.ToInt32(regulartemp.ConfigParamVal);

                    }
                    //Aniket [18-9-2019] commented the code
                    //TblConfigParamsTO isHideBrandNameOnNC = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.HIDE_BRAND_NAME_ON_NC_INVOICE, conn, tran);
                    //if(isHideBrandNameOnNC!=null)
                    //{
                    //    isHide = Convert.ToInt32(isHideBrandNameOnNC.ConfigParamVal);
                    //}
                    //[05-09-2018] : Vijaymala added to set product item display for other booking in invoice
                    if (loadingSlipExtTo.ProdItemId == 0)
                    {
                        if (a == 1)
                        {
                            tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.BrandDesc + " " + loadingSlipExtTo.ProdCatDesc + " " + loadingSlipExtTo.ProdSpecDesc + " " + loadingSlipExtTo.MaterialDesc;
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
                        tblInvoiceItemDetailsTO.ProdItemDesc = loadingSlipExtTo.ProdItemDesc; // Aniket [4-02-2019]

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

                tblInvoiceItemDetailsTO.BasicTotal = Math.Round((loadingSlipExtTo.LoadedWeight * conversionFactor * tblInvoiceItemDetailsTO.Rate), 2);
                basicTotal += tblInvoiceItemDetailsTO.BasicTotal;
                //Vijaymala added[22-06-2018]
                DropDownTO dropDownTO = _iDimensionBL.SelectCDDropDown(tblInvoiceItemDetailsTO.CdStructureId);
                if (tblInvoiceItemDetailsTO.CdStructure >= 0)
                {
                    Int32 isRsValue = Convert.ToInt32(dropDownTO.Text);

                    if (isRsValue == (int)Constants.CdType.IsRs)
                    {
                        tblInvoiceItemDetailsTO.CdAmt = tblInvoiceItemDetailsTO.CdStructure * loadingSlipExtTo.LoadedWeight * conversionFactor;
                    }
                    else
                    {
                        tblInvoiceItemDetailsTO.CdAmt = Math.Round(tblInvoiceItemDetailsTO.BasicTotal * tblInvoiceItemDetailsTO.CdStructure) / 100;
                    }
                    //Priyanka [10-07-2018] : Added for additional discount SHIVANGI.
                    tblInvoiceItemDetailsTO.CdAmt += loadingSlipTo.AddDiscAmt * loadingSlipExtTo.LoadedWeight * conversionFactor;
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
                discountTotal += tblInvoiceItemDetailsTO.CdAmt;
                Double taxbleAmt = 0;
                //Double totalFreExpOtherAmt = loadingSlipExtTo.LoadedWeight * conversionFactor * loadingSlipExtTo.FreExpOtherAmt;

                if (loadingSlipTo.IsConfirmed == 1)
                    taxbleAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;// + totalFreExpOtherAmt;
                else
                    taxbleAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;

                tblInvoiceItemDetailsTO.TaxableAmt = taxbleAmt;
                itemGrandTotal += taxbleAmt;
                taxableTotal += tblInvoiceItemDetailsTO.TaxableAmt;
                tblProdGstCodeDtlsTO = _iTblProdGstCodeDtlsDAO.SelectTblProdGstCodeDtls(loadingSlipExtTo.ProdCatId, loadingSlipExtTo.ProdSpecId, loadingSlipExtTo.MaterialId, loadingSlipExtTo.ProdItemId, 0, conn, tran);
                if (tblProdGstCodeDtlsTO == null)
                {
                    resultMsg.DefaultBehaviour("ProdGSTCodeDetails found null against loadingSlipExtId is : " + loadingSlipExtTo.IdLoadingSlipExt + ".");
                    resultMsg.DisplayMessage = "GSTIN Not Defined for Item :" + tblInvoiceItemDetailsTO.ProdItemDesc;
                    ////return resultMsg;
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

                #region 4 Added Invoice Item Tax details

                foreach (var taxRateTo in gstCodeDtlsTO.TaxRatesTOList)
                {
                    TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = new TblInvoiceItemTaxDtlsTO();
                    tblInvoiceItemTaxDtlsTO.TaxRateId = taxRateTo.IdTaxRate;
                    tblInvoiceItemTaxDtlsTO.TaxPct = taxRateTo.TaxPct;
                    tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                    tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                    if (isSez)
                    {
                        tblInvoiceItemTaxDtlsTO.TaxRatePct = 0;
                        tblInvoiceItemTaxDtlsTO.TaxableAmt = 0;
                    }
                    tblInvoiceItemTaxDtlsTO.TaxAmt = (tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
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


                grandTotal += itemGrandTotal;
                tblInvoiceItemDetailsTO.GrandTotal = itemGrandTotal;
                tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
                tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
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

            RoundOffInvoiceValuesBySetting(tblInvoiceTO);

            //double finalGrandTotal = Math.Round(grandTotal);
            //tblInvoiceTO.GrandTotal = finalGrandTotal;
            //tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);
            


            tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
            return tblInvoiceTO;
            #endregion
        }

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
                List<TblInvoiceItemDetailsTO> invoiceItemTOList = _iTblInvoiceItemDetailsBL.SelectAllTblInvoiceItemDetailsList(invoiceId, conn, tran);
                List<TblInvoiceItemTaxDtlsTO> invoiceItemTaxTOList = _iTblInvoiceItemTaxDtlsBL.SelectInvoiceItemTaxDtlsListByInvoiceId(invoiceId, conn, tran);
                List<TblInvoiceAddressTO> invoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId, conn, tran);

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

                TblConfigParamsTO tblConfigParamsTO = null;
                DateTime serverDateTime = _iCommon.ServerDateTime;
                Int32 billingStateId = 0;

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

                    tblInvoiceItemDetailsTO.InvoiceQty = invoiceItemTOList.Sum(s => s.InvoiceQty);
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



                        bookingRateTemp -= 400;
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
                    tblInvoiceItemDetailsTO.BasicTotal = tblInvoiceItemDetailsTO.Rate * tblInvoiceItemDetailsTO.InvoiceQty;
                    tblInvoiceItemDetailsTO.TaxableAmt = tblInvoiceItemDetailsTO.BasicTotal - tblInvoiceItemDetailsTO.CdAmt;
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
                }

                foreach (var existingInvItemTO in invoiceItemTOList)
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
                        tblInvoiceItemTaxDtlsTO.TaxRatePct = (gstCodeDtlsTO.TaxPct * taxRateTo.TaxPct) / 100;
                        tblInvoiceItemTaxDtlsTO.TaxableAmt = tblInvoiceItemDetailsTO.TaxableAmt;
                        tblInvoiceItemTaxDtlsTO.TaxAmt = (tblInvoiceItemTaxDtlsTO.TaxableAmt * tblInvoiceItemTaxDtlsTO.TaxRatePct) / 100;
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
                    taxableTotal += existingInvItemTO.TaxableAmt;
                    discountTotal += existingInvItemTO.CdAmt;

                    itemGrandTotal += existingInvItemTO.TaxableAmt;

                    grandTotal += itemGrandTotal;
                    tblInvoiceItemDetailsTO.GrandTotal = itemGrandTotal;
                    tblInvoiceItemDetailsTO.InvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList;
                    tblInvoiceItemDetailsTOList.Add(tblInvoiceItemDetailsTO);
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

                //double finalGrandTotal = Math.Round(grandTotal);
                //tblInvoiceTO.GrandTotal = finalGrandTotal;
                //tblInvoiceTO.RoundOffAmt = Math.Round(finalGrandTotal - grandTotal, 2);

                RoundOffInvoiceValuesBySetting(tblInvoiceTO);

                tblInvoiceTO.InvoiceItemDetailsTOList = tblInvoiceItemDetailsTOList;
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
            TblConfigParamsTO tblConfigParamsTOR = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.ROUND_OFF_TAX_INVOICE_VALUES);
            if (tblConfigParamsTOR != null)
            {
                roundOffValue = Convert.ToInt32(tblConfigParamsTOR.ConfigParamVal);
            }
            //if (roundOffValue > 0) Saket [2019-12-05] not needed these condition 
            if (true)
            {
                tblInvoiceTO.TaxableAmt = Math.Round(tblInvoiceTO.TaxableAmt, roundOffValue);
                tblInvoiceTO.DiscountAmt = Math.Round(tblInvoiceTO.DiscountAmt, roundOffValue);
                tblInvoiceTO.IgstAmt = Math.Round(tblInvoiceTO.IgstAmt, roundOffValue);
                tblInvoiceTO.CgstAmt = Math.Round(tblInvoiceTO.CgstAmt, roundOffValue);
                tblInvoiceTO.SgstAmt = Math.Round(tblInvoiceTO.SgstAmt, roundOffValue);
                tblInvoiceTO.BasicAmt = Math.Round(tblInvoiceTO.BasicAmt, roundOffValue);
            }

            Double grandTotal = tblInvoiceTO.TaxableAmt + tblInvoiceTO.IgstAmt + tblInvoiceTO.CgstAmt + tblInvoiceTO.SgstAmt;

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


                            //To calculate tare,gross and net weight
                            var minTareWt = tblInvoiceTOList.Min(minEle => minEle.TareWeight);
                            finalInvoiceTO.TareWeight = minTareWt;
                            var maxGrossWt = finalInvoiceItemDetailsTOList.Sum(maxEle => maxEle.InvoiceQty);
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
        public ResultMessage PrintReport(Int32 invoiceId,Boolean isPrinted=false)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {

                DataSet printDataSet = new DataSet();

                //headerDT
                DataTable headerDT = new DataTable();
                DataTable addressDT = new DataTable();
                DataTable invoiceDT = new DataTable();
                DataTable invoiceItemDT = new DataTable();
                DataTable itemFooterDetailsDT = new DataTable();
                DataTable commercialDT = new DataTable();
                DataTable hsnItemTaxDT = new DataTable();
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
                TblConfigParamsTO configParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.CP_DEFAULT_MATE_COMP_ORGID);
                if (configParamsTO != null)
                {
                    defaultCompOrgId = Convert.ToInt16(configParamsTO.ConfigParamVal);
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
                invoiceDT.Columns.Add("areaName");
                invoiceDT.Columns.Add("district");
                invoiceDT.Columns.Add("pinCode");


                invoiceDT.Columns.Add("orgFirmName");
                invoiceDT.Columns.Add("hsnNo");
                invoiceDT.Columns.Add("panNo");
                invoiceDT.Columns.Add("paymentTerm");
                invoiceDT.Columns.Add("poNo");
                invoiceDT.Columns.Add("poDateStr");

                invoiceDT.Columns.Add("TotalTaxAmt");
                invoiceDT.Columns.Add("TotalTaxAmtWordStr");


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
                TblInvoiceTO tblInvoiceTO = SelectTblInvoiceTOWithDetails(invoiceId);

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
                        invoiceItemDT.Columns.Add("hsn");
                        for (int i = 0; i < invoiceItemlist.Count; i++)
                        {
                            TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO = invoiceItemlist[i];
                            invoiceItemDT.Rows.Add();
                            Int32 invoiceItemDTCount = invoiceItemDT.Rows.Count - 1;
                            invoiceItemDT.Rows[invoiceItemDTCount]["srNo"] = i + 1;
                            invoiceItemDT.Rows[invoiceItemDTCount]["prodItemDesc"] = tblInvoiceItemDetailsTO.ProdItemDesc;
                            invoiceItemDT.Rows[invoiceItemDTCount]["bundles"] = tblInvoiceItemDetailsTO.Bundles;
                            if (isMathRoundoff == 1)
                            {
                                invoiceItemDT.Rows[invoiceItemDTCount]["invoiceQty"] = tblInvoiceItemDetailsTO.InvoiceQty;
                                invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = tblInvoiceItemDetailsTO.Rate;
                                invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = tblInvoiceItemDetailsTO.BasicTotal;
                            }
                            else
                            {
                                invoiceItemDT.Rows[invoiceItemDTCount]["invoiceQty"] = Math.Round(tblInvoiceItemDetailsTO.InvoiceQty, 3);
                                invoiceItemDT.Rows[invoiceItemDTCount]["rate"] = Math.Round(tblInvoiceItemDetailsTO.Rate, 2);
                                invoiceItemDT.Rows[invoiceItemDTCount]["basicTotal"] = Math.Round(tblInvoiceItemDetailsTO.BasicTotal, 2);
                            }

                            invoiceItemDT.Rows[invoiceItemDTCount]["hsn"] = tblInvoiceItemDetailsTO.GstinCodeNo;
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
                    invoiceDT.Rows[0]["poNo"] = tblInvoiceTO.PoNo;
                    if (!String.IsNullOrEmpty(tblInvoiceTO.PoDateStr))
                    {
                        DateTime poDate = Convert.ToDateTime(tblInvoiceTO.PoDateStr);
                        addressDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");
                        invoiceDT.Rows[0]["poDateStr"] = poDate.ToString("dd/MM/yyyy");
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
                printDataSet.Tables.Add(invoiceDT);
                printDataSet.Tables.Add(invoiceItemDT);
                printDataSet.Tables.Add(addressDT);
                printDataSet.Tables.Add(itemFooterDetailsDT);
                printDataSet.Tables.Add(commercialDT);
                printDataSet.Tables.Add(hsnItemTaxDT);
                printDataSet.Tables.Add(multipleInvoiceCopyDT);
                // printDataSet.Tables.Add(shippingAddressDT);
                //creating template'''''''''''''''''

                
                string templateName = "";
                if (isPrinted)
                {
                    int val = 0;
                    TblConfigParamsTO configParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MULTIPLE_TEMPLATE_FOR_PRINTED_INVOICE);
                    if (configParamsTOTemp != null)
                    {
                        val = Convert.ToInt16(configParamsTOTemp.ConfigParamVal);
                    }

                    if (val == 0)
                    {
                        templateName = "InvoiceVoucherPrinted";
                    }
                    else
                    {
                        templateName = "InvoiceVoucherPrePrinted_" + organizationTO.IdOrganization;
                    }

                }
                else {
                    int val = 0;
                    TblConfigParamsTO configParamsTOTemp = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MULTIPLE_TEMPLATE_FOR_PLAIN_INVOICE);
                    if (configParamsTOTemp != null)
                    {
                        val = Convert.ToInt16(configParamsTOTemp.ConfigParamVal);
                    }

                    if (val == 0)
                    {
                        templateName = "InvoiceVoucherPlain";
                    }
                    else
                    {
                        templateName = "InvoiceVoucherPlain_" + organizationTO.IdOrganization;
                    }
                }
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



                    //driveName + path;
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
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }



        public ResultMessage PrintWeighingReport(Int32 invoiceId)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                if (invoiceId != null)
                {
                    TblLoadingSlipTO TblLoadingSlipTO = _iTblLoadingSlipBL.SelectAllLoadingSlipWithDetailsByInvoice(invoiceId);
                    List<TblInvoiceAddressTO> invoiceAddressTOList = _iTblInvoiceAddressBL.SelectAllTblInvoiceAddressList(invoiceId);


                    if (TblLoadingSlipTO != null)
                    {
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
                        headerDT.Columns.Add("FirmName");
                        headerDT.Columns.Add("dealername");
                        headerDT.Columns.Add("VehicleNo");
                        headerDT.Columns.Add("LoadingSlipId");
                        headerDT.Columns.Add("loadingLayerDesc");
                        headerDT.Columns.Add("Date");
                        headerDT.Columns.Add("TotalBundles");
                        headerDT.Columns.Add("TotalNetWt");

                        loadingItemDTForGatePass.Columns.Add("DisplayName");
                        loadingItemDTForGatePass.Columns.Add("MaterialDesc");
                        loadingItemDTForGatePass.Columns.Add("ProdItemDesc");
                        loadingItemDTForGatePass.Columns.Add("LoadingQty");
                        loadingItemDTForGatePass.Columns.Add("Bundles");
                        loadingItemDTForGatePass.Columns.Add("LoadedWeight");
                        loadingItemDTForGatePass.Columns.Add("MstLoadedBundles");
                        loadingItemDTForGatePass.Columns.Add("LoadedBundles");
                        loadingItemDTForGatePass.Columns.Add("GrossWt");
                        loadingItemDTForGatePass.Columns.Add("TareWt");
                        loadingItemDTForGatePass.Columns.Add("NetWt");
                        loadingItemDTForGatePass.Columns.Add("BrandDesc");
                        loadingItemDTForGatePass.Columns.Add("ProdSpecDesc");
                        loadingItemDTForGatePass.Columns.Add("ProdcatDesc");
                        loadingItemDTForGatePass.Columns.Add("ItemName");
                        loadingItemDTForGatePass.Columns.Add("UpdatedOn");
                        loadingItemDTForGatePass.Columns.Add("DisplayField");
                        loadingItemDTForGatePass.Columns.Add("LoadingSlipId");


                        loadingItemDT.Columns.Add("DisplayName");
                        loadingItemDT.Columns.Add("MaterialDesc");
                        loadingItemDT.Columns.Add("ProdItemDesc");
                        loadingItemDT.Columns.Add("LoadingQty");
                        loadingItemDT.Columns.Add("Bundles");
                        loadingItemDT.Columns.Add("LoadedWeight");
                        loadingItemDT.Columns.Add("MstLoadedBundles");
                        loadingItemDT.Columns.Add("LoadedBundles");
                        loadingItemDT.Columns.Add("GrossWt");
                        loadingItemDT.Columns.Add("TareWt");
                        loadingItemDT.Columns.Add("NetWt");
                        loadingItemDT.Columns.Add("BrandDesc");
                        loadingItemDT.Columns.Add("ProdSpecDesc");
                        loadingItemDT.Columns.Add("ProdcatDesc");
                        loadingItemDT.Columns.Add("ItemName");
                        loadingItemDT.Columns.Add("UpdatedOn");
                        loadingItemDT.Columns.Add("DisplayField");
                        loadingItemDT.Columns.Add("LoadingSlipId");


                        #endregion
                        if (TblLoadingSlipTO != null )
                        {
                            headerDT.Rows.Add();
                            double totalBundle = 0;
                            double totalNetWt = 0;
                            headerDT.Rows[0]["FirmName"] = TblLoadingSlipTO.DealerOrgName;
                            headerDT.Rows[0]["dealername"] = TblLoadingSlipTO.DealerOrgName;
                            headerDT.Rows[0]["VehicleNo"] = TblLoadingSlipTO.VehicleNo;
                            headerDT.Rows[0]["loadingLayerDesc"] = TblLoadingSlipTO.LoadingSlipExtTOList[0].LoadingLayerDesc;
                            headerDT.Rows[0]["LoadingSlipId"] = TblLoadingSlipTO.IdLoadingSlip;
                            headerDT.Rows[0]["Date"] = TblLoadingSlipTO.CreatedOnStr;


                            for (int j = 0; j < TblLoadingSlipTO.LoadingSlipExtTOList.Count; j++)
                                {
                                    TblLoadingSlipExtTO tblLoadingSlipExtTO = TblLoadingSlipTO.LoadingSlipExtTOList[j];
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
                                    totalBundle +=tblLoadingSlipExtTO.Bundles;
                                    loadingItemDT.Rows[loadItemDTCount]["TareWt"] = (tblLoadingSlipExtTO.CalcTareWeight / 1000);
                                    loadingItemDT.Rows[loadItemDTCount]["GrossWt"] = (tblLoadingSlipExtTO.CalcTareWeight + tblLoadingSlipExtTO.LoadedWeight)/ 1000;
                                    loadingItemDT.Rows[loadItemDTCount]["NetWt"] = tblLoadingSlipExtTO.LoadedWeight / 1000;
                                totalNetWt += (tblLoadingSlipExtTO.LoadedWeight / 1000);
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedWeight"] = tblLoadingSlipExtTO.LoadedWeight;
                                    loadingItemDT.Rows[loadItemDTCount]["MstLoadedBundles"] = tblLoadingSlipExtTO.MstLoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadedBundles"] = tblLoadingSlipExtTO.LoadedBundles;
                                    loadingItemDT.Rows[loadItemDTCount]["LoadingSlipId"] = tblLoadingSlipExtTO.LoadingSlipId;

                                }
                            headerDT.Rows[0]["TotalBundles"] =totalBundle;
                            headerDT.Rows[0]["TotalNetWt"] = totalNetWt;


                        }


                        //headerDT = loadingDT.Copy();
                        headerDT.TableName = "headerDT";
                        printDataSet.Tables.Add(headerDT);

                        loadingItemDT.TableName = "loadingItemDT";
                        printDataSet.Tables.Add(loadingItemDT);

                        loadingItemDTForGatePass = loadingItemDT.Copy();

                        loadingItemDT.TableName = "loadingItemDTForGatePass";
                        printDataSet.Tables.Add(loadingItemDTForGatePass);


                        string templateName = "WeighingSlip";
                        //creating template'''''''''''''''''
                        if (TblLoadingSlipTO.IsConfirmed != 1)
                        {
                            templateName = "WeighingSlipNonConfirm";
                        }
                       
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
            if(tblInvoiceTO.DiscountAmt >0)
            {
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
              
                    commercialValueDT.Rows[commercialDTCount]["Name"] = "Discount (" + Math.Round(getDiscountPerct(tblInvoiceTO), 2) + "%)";
                    commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(tblInvoiceTO.DiscountAmt, 2);
                
                
            }
            
            var freightResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.FREIGHT).FirstOrDefault();
            if (freightResTO != null && freightResTO.TaxableAmt >0)
            {
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                commercialValueDT.Rows[commercialDTCount]["Name"] = "Freight/Other";
               
                    commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(freightResTO.TaxableAmt, 2);
             
                
            }
            var pfResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.PF).FirstOrDefault();
            if (pfResTO != null && pfResTO.TaxableAmt >0)
            {
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                commercialValueDT.Rows[commercialDTCount]["Name"] = "PF";
              
                    commercialValueDT.Rows[commercialDTCount]["Value"] = Math.Round(pfResTO.TaxableAmt, 2);
             
               

               
            }
            var cessResTO = tblInvoiceTO.InvoiceItemDetailsTOList.Where(ele => ele.OtherTaxId == (Int32)Constants.OtherTaxTypeE.CESS).FirstOrDefault();
            if (cessResTO != null && cessResTO.TaxableAmt >0)
            {
                double cessPercentage = cessResTO.TaxPct;
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                if(cessPercentage>0)
                commercialValueDT.Rows[commercialDTCount]["Name"] = cessResTO.ProdItemDesc+"@"+cessPercentage+"%";
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
            if (afterCessResTO != null && afterCessResTO.TaxableAmt >0)
            {
                double taxPercentage = afterCessResTO.TaxPct;
                commercialValueDT.Rows.Add();
                commercialDTCount = commercialValueDT.Rows.Count - 1;
                if(taxPercentage>0)
                {
                    commercialValueDT.Rows[commercialDTCount]["Name"] = afterCessResTO.ProdItemDesc+"@"+ taxPercentage + "%";
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

            int isMathRounfOff = 0;
            TblConfigParamsTO tblConfigParams = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.IS_ROUND_OFF_TAX_ON_PRINT_INVOICE);
            if(tblConfigParams != null)
            {
                if(tblConfigParams.ConfigParamVal=="1")
                {
                    isMathRounfOff = 1;
                }
            }
            List<TblInvoiceItemTaxDtlsTO> tblInvoiceItemTaxDtlsTOList = _iTblInvoiceItemTaxDtlsBL.SelectInvoiceItemTaxDtlsListByInvoiceId(tblInvoiceTO.IdInvoice);
            if (tblInvoiceItemTaxDtlsTOList != null && tblInvoiceItemTaxDtlsTOList.Count > 0)
            {

                //Aniket [12-03-2019]
                tblInvoiceItemTaxDtlsTOList = tblInvoiceItemTaxDtlsTOList.Where(x => x.IsAfter == 0).ToList();

                List<TblInvoiceItemTaxDtlsTO> distinctHsnItemTaxList = tblInvoiceItemTaxDtlsTOList.GroupBy(w => w.GstinCodeNo).Select(s => s.FirstOrDefault()).ToList();
                if (distinctHsnItemTaxList != null && distinctHsnItemTaxList.Count > 0)
                {
                    
                    Double cgstAmt = 0, sgstAmt = 0, igstAmt = 0, taxTotal = 0, hsntaxableAmt = 0 ;
                    for (int m = 0; m < distinctHsnItemTaxList.Count; m++)
                    {
                        cgstAmt = 0; sgstAmt = 0; taxTotal = 0;
                
                        List<TblInvoiceItemTaxDtlsTO> tempInvoiceTaxList = tblInvoiceItemTaxDtlsTOList.Where(oi => oi.GstinCodeNo == distinctHsnItemTaxList[m].GstinCodeNo).ToList();
                     
                            for (int n = 0; n < tempInvoiceTaxList.Count; n++)
                            {
                                TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO = tempInvoiceTaxList[n];
                                 if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.CGST)
                                {
                                    cgstAmt += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    taxTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;


                                }
                                if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.SGST)
                                {
                                    sgstAmt += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                    taxTotal += tblInvoiceItemTaxDtlsTO.TaxAmt;
                                }
                                if (tblInvoiceItemTaxDtlsTO.TaxTypeId == (int)Constants.TaxTypeE.IGST)
                                {
                                    igstAmt += tblInvoiceItemTaxDtlsTO.TaxAmt;
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

                }

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
                            if(!string.IsNullOrEmpty(tblAlertDefinitionTO.DefaultAlertTxt))
                            {
                                tempTxt = tblAlertDefinitionTO.DefaultAlertTxt;
                                tempTxt = tempTxt.Replace("@InvoiceIdStr",tblInvoiceTO.IdInvoice.ToString());
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
                            tempTxt = tempTxt.Replace("@DealerNameStr", "");

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
                            List<DropDownTO> dropDownTOList = _iDimensionBL.SelectCDStructureForDropDown();
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
                if(tblConfigParams!=null)
                {
                    if(tblConfigParams.ConfigParamVal=="1")
                    {
                        isMathRoundOff = 1;
                    }
                }
                if (tblInvoiceItemDetailsTO.CdStructure > 0)
                {
                    Int32 isRsValue = Convert.ToInt32(cdDropDownTO.Text);
                    if (isRsValue == (int)Constants.CdType.IsRs)
                    {

                        tblInvoiceItemDetailsTO.CdAmt = tblInvoiceItemDetailsTO.CdStructure;
                    }
                    else
                    {
                        if(isMathRoundOff==1)
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
                tblInvoiceTo.DiscountAmt += tblInvoiceItemDetailsTO.CdAmt;
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

        public ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm, Int32 invGenModeId,int fromOrgId,int toOrgId,String taxInvoiceNumber="",Int32 manualinvoiceno=0,String invComment="")
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
                        resultMessage = PrepareAndSaveInternalTaxInvoices(invoiceTO,invGenModeId,fromOrgId,toOrgId,0, changeHisTO,conn, tran);
                 
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

                    if (invoiceTO.IsConfirmed == 1)
                    {
                        TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID, conn, tran);
                        // Aniket [05-02-2019] check manual branswise invoice no generate or not
                        TblConfigParamsTO tblConfigParamsTOForBrand = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.GENERATE_MANUALLY_BRANDWISE_INVOICENO, conn, tran);
                        if (tblConfigParamsTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Internal Self Organization Not Found in Configuration.");
                            return resultMessage;
                        }
                        Int32 defualtOrgId = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
                        TblEntityRangeTO entityRangeTO = null;
                        if (Convert.ToInt32(tblConfigParamsTOForBrand.ConfigParamVal) == 1)
                        {
                            DimBrandTO dimBrandTO = _iDimBrandDAO.SelectDimBrand(invoiceTO.BrandId);
                            entityRangeString += dimBrandTO.IdBrand.ToString();
                            entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityRangeString, invoiceTO.FinYearId, conn, tran);
                        }
                        //Hrushikesh added get entity Range organizationwise
                        else if (invoiceTO.InvFromOrgId != defualtOrgId)
                        {
                            string  orgstr = Constants.ENTITY_RANGE_REGULAR_TAX_INTERNALORG + invoiceTO.InvFromOrgId;
                            entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(orgstr, invoiceTO.FinYearId, conn, tran);
                        }
                        else
                            entityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(invoiceTO.InvoiceTypeId, invoiceTO.FinYearId, conn, tran);

                        if (entityRangeTO == null)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("entityRangeTO Found NULL. Entity Range Not Defined"); return resultMessage;
                        }
                        Int32 entityPrevVal=0;
                        bool isInvoicePresent=false;
                        int newTaxNumber=0;
                        //Aniket [17-jan-2019] to check invoice number should generate manually or automatically
                        if (String.IsNullOrEmpty(taxInvoiceNumber))
                        {
                             
                            entityPrevVal = entityRangeTO.EntityPrevValue;
                            entityPrevVal++;
                            if(String.IsNullOrEmpty(entityRangeTO.Suffix))
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

                            TblInvoiceTO tblInvoiceTO = _iTblInvoiceDAO.SelectAllTblInvoice(taxInvoiceNumber,invoiceTO.FinYearId);
                           // List<TblInvoiceTO> tempList = list.Where(x => x.FinYearId == invoiceTO.FinYearId).ToList();
                           // List<TblInvoiceTO> existInvoiceList = new List<TblInvoiceTO>();
                           // if (tempList != null && tempList.Count > 0)
                          //  {
                             //   existInvoiceList = tempList.Where(ele => ele.InvoiceNo == taxInvoiceNumber).ToList();
                                if (!String.IsNullOrEmpty(tblInvoiceTO.InvoiceNo))
                                {
                                
                                    isInvoicePresent = true;
                                    resultMessage.DefaultBehaviour( taxInvoiceNumber+ " Invoice number has been already generated against Id  "+ tblInvoiceTO.IdInvoice);
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

                            result = UpdateTblInvoice(invoiceTO, conn, tran);
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
                        result = UpdateTblInvoice(invoiceTO, conn, tran);
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
                    result = _iTblInvoiceHistoryBL.InsertTblInvoiceHistory(invHistoryTO, conn, tran);
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

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.DisplayMessage = "Success..Invoice authorized and #" + invoiceTO.InvoiceNo + " is generated";
                resultMessage.Tag = invoiceTO;
                return resultMessage;
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

        //Aniket [22-4-2019]
        public List<TblInvoiceAddressTO> SelectTblInvoiceAddressByDealerId(Int32 dealerOrgId, String txnAddrTypeId)
        {
            Int32 cnt = 0;
            String txnAddrTypeIdtemp = String.Empty;

            if (!String.IsNullOrEmpty(txnAddrTypeId))
            {
                txnAddrTypeIdtemp = txnAddrTypeId;
            }

            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_EXISTING_ADDRESS_COUNT_FOR_BOOKING);
            if (tblConfigParamsTO != null)
            {
                cnt = Convert.ToInt32(tblConfigParamsTO.ConfigParamVal);
            }

            List<TblInvoiceAddressTO> tblInvoiceDelAddrTOList = new List<TblInvoiceAddressTO>();
            try
            {
                List<TblInvoiceAddressTO> tblInvoiceDelAddrTOListtemp = new List<TblInvoiceAddressTO>();
                 tblInvoiceDelAddrTOList= _iTblInvoiceAddressDAO.SelectTblInvoiceAddressByDealerId(dealerOrgId, txnAddrTypeIdtemp);
                //tblBookingDelAddrTOList = _iTblBookingDelAddrDAO.SelectTblBookingsByDealerOrgId(dealerOrgId, txnAddrTypeIdtemp);
                //tblBookingDelAddrTOList = tblBookingDelAddrTOList.Where(ele => ele.BillingName != null || ele.Address != null).ToList();
                //tblBookingDelAddrTOList = tblBookingDelAddrTOList.GroupBy(c => new { c.BillingName, c.Address }).Select(s => s.FirstOrDefault()).ToList();
                if (cnt > tblInvoiceDelAddrTOList.Count)
                {
                    tblInvoiceDelAddrTOListtemp = tblInvoiceDelAddrTOList;
                }
                else
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        TblInvoiceAddressTO tblInvoiceDelAddrTO = tblInvoiceDelAddrTOList[i];
                        tblInvoiceDelAddrTOListtemp.Add(tblInvoiceDelAddrTO);
                    }
                }
                return tblInvoiceDelAddrTOListtemp;
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
                if (sendMail.IsWeighmentSlipAttach == true && sendMail.IsInvoiceAttach==true)
                {
                    sendMail.Subject = "Invoice-"+sendMail.InvoiceNumber+" and Weighment Slip";
                }
                else if (sendMail.IsWeighmentSlipAttach == false && sendMail.IsInvoiceAttach == true)
                {
                    sendMail.Subject = "Invoice-" + sendMail.InvoiceNumber;
                }
                else
                {
                    sendMail.Subject = "Weighment Slip";
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
                saleOrderDocument = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
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

        public List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId)
        {
            return _iTblInvoiceDAO.SelectOtherTaxDetailsReport(frmDt, toDt, isConfirm, otherTaxId);
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

    }
}
