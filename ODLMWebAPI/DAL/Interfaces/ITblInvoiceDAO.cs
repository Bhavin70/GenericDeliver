using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblInvoiceDAO
    { 
        String SqlSelectQuery();
        String SelectresponseForPhotoInReport(Int32 idInvoice,Int32 ApiId);
        List<TblInvoiceTO> SelectAllTblInvoice();
        List<TblInvoiceTO> SelectAllTblInvoice(DateTime frmDt, DateTime toDt, int isConfirm, Int32 cnfId, Int32 dealerId, TblUserRoleTO tblUserRoleTO, Int32 brandId, Int32 invoiceId, Int32 statusId, String internalOrgId, Int32 districtId , Int32 stateId);
        TblInvoiceTO SelectTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectTblInvoiceByStatus(Int32 statusId, int distributorOrgId, int invoiceId, SqlConnection conn, SqlTransaction tran,int isConfirm);
        List<TblInvoiceTO> SelectInvoiceTOFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn = null, SqlTransaction tran = null);
        List<TblInvoiceTO> SelectInvoiceListFromLoadingSlipIds(String loadingSlipIds, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> ConvertDTToList(SqlDataReader tblInvoiceTODT);
        List<TblInvoiceRptTO> SelectAllRptInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm,int fromOrgId);
        List<TblInvoiceRptTO> SelectAllRptNCList(DateTime frmDt, DateTime toDt);
        List<TblInvoiceRptTO>  GetDistictWiseDispatchData(DateTime frmDt, DateTime toDt,int reportDataType);

        List<TblInvoiceRptTO>  GetDistictWiseDispatchDataAll(DateTime frmDt, DateTime toDt);
        List<TblInvoiceRptTO> GetDispatchReportAll(DateTime frmDt, DateTime toDt, Int32 cnfId, Int32 dealerId,
            Int32 stateId, Int32 districtId, Int32 talukaId);
        List<TblInvoiceRptTO> GetDistictWiseDispatchDataAllV2(DateTime frmDt, DateTime toDt);
        List<TblInvoiceRptTO> GetDistictWiseDispatchData(DateTime frmDt, DateTime toDt);
        List<TblInvoiceRptTO> GetDistictWiseDispatchDataV2(DateTime frmDt, DateTime toDt);
        List<TblInvoiceRptTO> ConvertDTToListForRPTInvoice(SqlDataReader tblInvoiceRptTODT);
        List<TblInvoiceRptTO> SelectInvoiceExportList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        List<TblInvoiceRptTO> SelectHsnExportList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        List<TblInvoiceTO> SelectAllTempInvoice(Int32 loadingSlipId);
        List<TblInvoiceRptTO> SelectSalesInvoiceListForReport(DateTime frmDt, DateTime toDt, int isConfirm,int fromOrgId);
        List<TblInvoiceTO> SelectAllTempInvoice(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectInvoiceListFromInvoiceIds(String invoiceIds);
        List<TblInvoiceTO> SelectAllFinalInvoice(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipTO> SelectLoadingDetailsByInvoiceId(int invoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceTO> SelectAllTNotifiedblInvoiceList(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);
        int InsertTblInvoice(TblInvoiceTO tblInvoiceTO);
        int InsertTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdInsert);
        int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO);
        int UpdateTblInvoice(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateInvoiceNonCommercDtls(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateInvoiceNonCommercDtlsForFinal(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceTO tblInvoiceTO, SqlCommand cmdUpdate);
        int UpdateInvoiceDate(TblInvoiceTO tblInvoiceTO);
        int DeleteTblInvoice(Int32 idInvoice);
        int DeleteTblInvoice(Int32 idInvoice, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvoice, SqlCommand cmdDelete);
        List<TblOtherTaxRpt> SelectOtherTaxDetailsReport(DateTime frmDt, DateTime toDt, int isConfirm, Int32 otherTaxId,int fromOrgId);
        List<TblOtherTaxRpt> ConvertDTToOtherTaxRptTOList(SqlDataReader tblInvoiceRptTODT);
        TblInvoiceTO SelectAllTblInvoice(string taxInvoiceNumber, int finYearId);

        double GetTareWeightFromInvoice(String lodingSlipIds, SqlConnection conn, SqlTransaction tran);
        int UpdateMappedSAPInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);

        int UpdateEInvoicNo(TblInvoiceTO tblInvoiceTO);
        int UpdateEInvoicNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateEWayBill(TblInvoiceTO tblInvoiceTO);
        int UpdateEWayBill(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTempInvoiceDistanceInKM(TblInvoiceTO tblInvoiceTO);

        List<TblInvoiceRptTO> SelectItemWiseSalesExportCListForReport(DateTime frmDt, DateTime toDt, int isConfirm, int fromOrgId);

        List<TblInvoiceRptTO> SelectSalesPurchaseListForReport(DateTime frmDt, DateTime toDt,int isConfirm, string selectedOrg,int defualtOrgId,int isFromPurchase);

        List<InvoiceReportTO> SelectAllInvoices(DateTime frmDt, DateTime toDt);
        TblInvoiceTO SelectTblInvoice(String loadingSlipId, SqlConnection conn, SqlTransaction tran);
        int PostUpdateInvoiceStatus(TblInvoiceTO tblInvoiceTO);
        int UpdateWhatsAppMsgSendInvoiceNo(TblInvoiceTO tblInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int InsertNCReportLog(string FunName, String ErrName);
    }
}