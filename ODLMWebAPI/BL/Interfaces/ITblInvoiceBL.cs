﻿using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using static ODLMWebAPI.StaticStuff.Constants;

namespace ODLMWebAPI.BL.Interfaces
{   
    public interface ITblInvoiceBL
    {
        List<TblInvoiceTO> SelectAllTblInvoiceList();
        List<TblInvoiceTO> SelectAllTblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerID, List<TblUserRoleTO> tblUserRoleTOList, Int32 brandId, Int32 invoiceId, Int32 statusId, String internalOrgId);
        TblInvoiceTO SelectTblInvoiceTO(Int32 idInvoice);
        List<TblInvoiceTO> SelectTblInvoiceByStatus(int statusId, int distributorOrgId, int invoiceId,int isConfirm);
        TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice);
        TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId);
        List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceTOListFromLoadingSlipId(Int32 loadingSlipId);
        List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran);
        ResultMessage  SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm,int fromOrgId);
        List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm,int fromOrgId);
        List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId);
        List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        Boolean CheckInvoiceDetailsAccToState(TblInvoiceTO tblInvoiceTO, ref String errorMsg);
        List<TblInvoiceTO> SelectAllInvoiceListByVehicleNo(string vehicleNo, DateTime frmDt, DateTime toDt);
        List<TblInvoiceTO> SelectInvoiceListFromInvoiceIds(String invoiceIds);
        List<TblInvoiceTO> SelectAllFinalInvoiceList(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipTO> SelectLoadingSlipDetailsByInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        //ResultMessage CreateIntermediateInvoiceAgainstLoading(String loadingIds, Int32 userId);
        ResultMessage InsertTblInvoice(TblInvoiceTO tblInvoiceTO);
        ResultMessage SaveNewInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
     
        ResultMessage PrepareAndSaveInternalTaxInvoices(TblInvoiceTO invoiceTO, int invoiceGenerateModeE,int fromOrgId,int toOrgId ,int isCalculateWithBaseRate,TblInvoiceChangeOrgHistoryTO changeHisTO,SqlConnection conn, SqlTransaction tran);
        ResultMessage PrepareNewInvoiceObjectList(TblInvoiceTO invoiceTO, List<TblInvoiceItemDetailsTO> invoiceItemTOList, List<TblInvoiceAddressTO> invoiceAddressTOList, int invoiceGenerateModeE,int fromOrgId,int toOrgId ,int isCalculateWithBaseRate, SqlConnection conn, SqlTransaction tran,int swap=1);
        ResultMessage PrepareAndSaveNewTaxInvoice(TblLoadingTO loadingTO, List<TblLoadingSlipExtTO> lastItemList, SqlConnection conn, SqlTransaction tran);
        ResultMessage ComposeInvoice(List<Int32> invoiceIdsList, Int32 loginUserId);
        ResultMessage DecomposeInvoice(List<Int32> invoiceIdsList, Int32 loginUserId);
        ResultMessage SaveInvoiceDocumentDetails(TblInvoiceTO invoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId);
        ResultMessage InsertInvoiceDocumentDetails(TblInvoiceTO tblInvoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrintReport(Int32 invoiceId,Boolean isPrinted=false,Boolean isSendEmailForInvoice = false);
        ResultMessage PrintWeighingReport(Int32 invoiceId,Boolean isSendEmailForWeighment = false,String reportType=null);
        ResultMessage SendInvoiceEmail(SendMail mailInformationTo);
        String currencyTowords(Double amount, Int32 currencyId);
        string ConvertNumbertoWords(long number);
        Double getDiscountPerct(TblInvoiceTO resInvoiceTO);
        DataTable getCommercialDT(TblInvoiceTO tblInvoiceTO);
        DataTable getHsnItemTaxDT(TblInvoiceTO tblInvoiceTO);
        int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO);
        int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveUpdatedInvoice(TblInvoiceTO invoiceTO);
        ResultMessage UpdateInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        TblInvoiceTO updateInvoiceToCalc(TblInvoiceTO tblInvoiceTo, SqlConnection conn, SqlTransaction tran, Boolean isCheckHist = true);
        ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm,Int32 invGenModeId,int fromOrgId,int toOrgId, String taxInvoiceNumber = "", Int32 manualinvoiceno = 0, String invComment = "");
        ResultMessage exchangeInvoice(Int32 invoiceId,  Int32 invGenModeId,int fromOrgId,int toOrgId , int isCalculateWithBaseRate);
        //ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm, Int32 invGenModeId, String taxInvoiceNumber = "", Int32 manualinvoiceno = 0);
        ResultMessage UpdateInvoiceNonCommercialDetails(TblInvoiceTO tblInvoiceTO);
        //ResultMessage UpdateInvoiceConfrimNonConfirmDetails(TblInvoiceTO tblInvoiceTO, Int32 loginUserId);
        ResultMessage UpdateInvoiceAfterloadingSlipOut(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateInvoiceDate(TblInvoiceTO tblInvoiceTO);
        ResultMessage DeactivateInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, Int32 loginUserId);
        int DeleteTblInvoice(Int32 idInvoice);
        int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteTblInvoiceDetails(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        //ResultMessage ExtractEnquiryData();
        //ResultMessage DeleteDispatchData();
        int sendInvoiceFromMail(SendMail sendMail);
        List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId,int fromOrgId);
        int UpdateIdentityFinalTables(SqlConnection conn, SqlTransaction tran);
        TblInvoiceTO PrepareInvoiceAgainstLoadingSlip(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran, int internalOrgId, TblAddressTO ofcAddrTO, TblConfigParamsTO rcmConfigParamsTO, TblConfigParamsTO invoiceDateConfigTO, TblLoadingSlipTO loadingSlipTo, int InvoiceDealerOrgId);
        TblEntityRangeTO GenerateInvoiceNumberFromEntityRange(Int32 idInvoice);

        //Aniket [22-4-2019]
        List<TblInvoiceAddressTO> SelectTblInvoiceAddressByDealerId(Int32 dealerOrgId, String addrSourceType);
        void SetGateAndWeightIotData(TblInvoiceTO tblInvoiceTO, int IsExtractionAllowed);

        void SetGateAndWeightIotData(List<TblInvoiceTO> tblInvoiceTOList, int IsExtractionAllowed);
        void SetGateIotDataToInvoiceTO(List<TblInvoiceTO> list);

        ResultMessage SetWeightIotDateToInvoiceTO(TblInvoiceTO tblInvoice, int IsExtractionAllowed);
        ResultMessage CreateInvoiceAgainstLoadingSlips(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran, List<TblLoadingSlipTO> loadingSlipTOList, Int32 skipMergeSetting = 0);
        Byte[] DeleteFile(string saveLocation, string filePath);
        int UpdateMappedSAPInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);

        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate eInvvoice.
        /// </summary>
        ResultMessage GenerateEInvoice(Int32 loginUserId, Int32 idInvoice, Int32 eInvoiceCreationType, bool forceToGetToken = false);
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Cancel eInvvoice.
        /// </summary>
        ResultMessage CancelEInvoice(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false);
        /// <summary>
        /// Dhananjay[01-03-2021] : Added To Get And Update eInvvoice.
        /// </summary>
        ResultMessage GetAndUpdateEInvoice(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false);
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Generate EWayBill.
        /// </summary>
        ResultMessage GenerateEWayBill(Int32 loginUserId, Int32 idInvoice, decimal distanceInKM, bool forceToGetToken = false);
        /// <summary>
        /// Dhananjay[18-11-2020] : Added To Cancel EWayBill.
        /// </summary>
        ResultMessage CancelEWayBill(Int32 loginUserId, Int32 idInvoice, bool forceToGetToken = false);
        ResultMessage UpdateInvoiceAddress(List<TblInvoiceAddressTO> tblInvoiceAddressTOList);
        Double CalculateTDS(TblInvoiceTO tblInvoiceTO);

        ResultMessage SelectItemWiseSalesExportCListForReport(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        ResultMessage PrintSaleReport(DateTime frmDt, DateTime toDt, int isConfirm, string selectedOrg, int isFromPurchase = 0);

        List<InvoiceReportTO> GetAllInvoices(DateTime fromDate, DateTime toDate, ref String errorMsg);
        ResultMessage PostUpdateInvoiceStatus(TblInvoiceTO tblInvoiceTO);

        ResultMessage ReverseWeighingDtlData(int InvoiceId);
    }
}
