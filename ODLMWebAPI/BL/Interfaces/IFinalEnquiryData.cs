using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface IFinalEnquiryData
    {
        ResultMessage InsertFinalEnquiryData(int loadingId, SqlConnection bookingConn, SqlTransaction bookingTran, SqlConnection enquiryConn, SqlTransaction enquiryTran, ref Dictionary<int, int> invoiceIdsList);
        int InsertFinalInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionFinalInvoiceItemDetailsCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdInsert);
        int InsertFinalInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionFinalInvoiceItemTaxDtlsCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdInsert);
        int InsertFinalInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionFinalInvoiceHistoryCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdInsert);
        int InsertEnquiryInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionEnquiryInvoiceDocumentDetailsCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdInsert);
        //int UpdateIdentityFinalTables(SqlConnection conn, SqlTransaction tran);
    }
}
