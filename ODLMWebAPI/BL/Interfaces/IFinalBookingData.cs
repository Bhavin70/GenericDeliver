using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IFinalBookingData
    { 
        ResultMessage InsertFinalBookingData(int loadingId, SqlConnection conn, SqlTransaction tran, ref Dictionary<int, int> invoiceIdsList);
        int InsertFinalInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionFinalInvoiceDocumentDetailsCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdInsert);
        List<TblInvoiceRptTO> SelectTempInvoiceData(SqlConnection conn, SqlTransaction tran);
        int DeleteTempLoadingData(int loadingId, SqlConnection conn, SqlTransaction tran);
        int DeleteDispatchTempLoadingData(int loadingId, SqlConnection conn, SqlTransaction tran);
        //int DeleteDispatchBookingData(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
        int DeleteYesterdaysStock(SqlConnection conn, SqlTransaction tran);
        int DeleteYesterdaysLoadingQuotaDeclaration(SqlConnection conn, SqlTransaction tran);
        int CreateTempInvoiceExcel(List<TblInvoiceRptTO> tblInvoiceRptTOList, SqlConnection conn, SqlTransaction tran);
        int UpdateIdentityFinalTables(SqlConnection conn, SqlTransaction tran);
    }
}
