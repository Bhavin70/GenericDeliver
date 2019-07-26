using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceHistoryBL
    {
        List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryList();
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTO(Int32 idInvHistory);
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTORateByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryById(Int32 byId, Boolean isByInvoiceId = false);
        List<TblInvoiceHistoryTO> SelectTempInvoiceHistory(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTOCdByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTOByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO);
        int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO);
        int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceHistoryById(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceHistory(Int32 idInvHistory);
        int DeleteTblInvoiceHistory(Int32 idInvHistory, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceHistoryByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceHistoryByItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoiceHistoryForFinal(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
    }
}
