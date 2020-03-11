﻿using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static ODLMWebAPI.StaticStuff.Constants;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceBL
    {
        List<TblInvoiceTO> SelectAllTblInvoiceList();
        List<TblInvoiceTO> SelectAllTblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerID, List<TblUserRoleTO> tblUserRoleTOList, Int32 brandId, Int32 invoiceId, Int32 statusId);
        TblInvoiceTO SelectTblInvoiceTO(Int32 idInvoice);
        List<TblInvoiceTO> SelectTblInvoiceByStatus(int statusId, int distributorOrgId, int invoiceId);
        TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice);
        TblInvoiceTO SelectTblInvoiceTOWithDetails(Int32 idInvoice, SqlConnection conn, SqlTransaction tran);
        TblInvoiceTO SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId);
        TblInvoiceTO SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceTOListFromLoadingSlipId(Int32 loadingSlipId);
        List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm);
        List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm);
        List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm);
        List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm);
        List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId);
        List<TblInvoiceTO> SelectTempInvoiceTOList(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        Boolean CheckInvoiceDetailsAccToState(TblInvoiceTO tblInvoiceTO, ref String errorMsg);
        List<TblInvoiceTO> SelectAllInvoiceListByVehicleNo(string vehicleNo, DateTime frmDt, DateTime toDt);
        List<TblInvoiceTO> SelectInvoiceListFromInvoiceIds(String invoiceIds);
        List<TblInvoiceTO> SelectAllFinalInvoiceList(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipTO> SelectLoadingSlipDetailsByInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm);
        ResultMessage CreateIntermediateInvoiceAgainstLoading(String loadingIds, Int32 userId);
        ResultMessage InsertTblInvoice(TblInvoiceTO tblInvoiceTO);
        ResultMessage SaveNewInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrepareAndSaveNewTaxInvoice(TblLoadingTO loadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrepareAndSaveInternalTaxInvoices(TblInvoiceTO invoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrepareNewInvoiceObjectList(TblInvoiceTO invoiceTO, List<TblInvoiceItemDetailsTO> invoiceItemTOList, List<TblInvoiceAddressTO> invoiceAddressTOList, InvoiceGenerateModeE invoiceGenerateModeE, SqlConnection conn, SqlTransaction tran);
        ResultMessage ComposeInvoice(List<Int32> invoiceIdsList, Int32 loginUserId);
        ResultMessage DecomposeInvoice(List<Int32> invoiceIdsList, Int32 loginUserId);
        ResultMessage SaveInvoiceDocumentDetails(TblInvoiceTO invoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId);
        ResultMessage InsertInvoiceDocumentDetails(TblInvoiceTO tblInvoiceTO, List<TblDocumentDetailsTO> tblDocumentDetailsTOList, Int32 loginUserId, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrintReport(Int32 invoiceId);
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
        ResultMessage GenerateInvoiceNumber(Int32 invoiceId, Int32 loginUserId, Int32 isconfirm, Int32 invGenModeId);
        ResultMessage UpdateInvoiceNonCommercialDetails(TblInvoiceTO tblInvoiceTO);
        ResultMessage UpdateInvoiceConfrimNonConfirmDetails(TblInvoiceTO tblInvoiceTO, Int32 loginUserId);
        ResultMessage UpdateInvoiceAfterloadingSlipOut(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateInvoiceDate(TblInvoiceTO tblInvoiceTO);
        ResultMessage DeactivateInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, Int32 loginUserId);
        int DeleteTblInvoice(Int32 idInvoice);
        int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeleteTblInvoiceDetails(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage ExtractEnquiryData();
        ResultMessage DeleteDispatchData();
        int sendInvoiceFromMail(SendMail sendMail);
        List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId);
    }
}