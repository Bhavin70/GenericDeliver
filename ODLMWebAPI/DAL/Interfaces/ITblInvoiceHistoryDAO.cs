using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblInvoiceHistoryDAO
    {
        String SqlSelectQuery();
        List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistory();
        TblInvoiceHistoryTO SelectTblInvoiceHistory(Int32 idInvHistory);
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTORateByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryById(Int32 byId, Boolean isByInvoiceId);
        List<TblInvoiceHistoryTO> ConvertDTToList(SqlDataReader tblInvoiceHistoryTODT);
        List<TblInvoiceHistoryTO> SelectTempInvoiceHistory(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTOCdByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceHistoryTO SelectTblInvoiceHistoryTOByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO);
        int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdInsert);
        int InsertTblInvoiceHistoryForFinal(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommandForFinal(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdInsert);
        int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO);
        int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdUpdate);
        int UpdateTblInvoiceHistoryById(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommandById(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblInvoiceHistory(Int32 idInvHistory);
        int DeleteTblInvoiceHistory(Int32 idInvHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvHistory, SqlCommand cmdDelete);
        int DeleteTblInvoiceHistoryByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete);
        int DeleteTblInvoiceHistoryByItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByInvoiceItemId(Int32 invoiceItemId, SqlCommand cmdDelete);

    }
}