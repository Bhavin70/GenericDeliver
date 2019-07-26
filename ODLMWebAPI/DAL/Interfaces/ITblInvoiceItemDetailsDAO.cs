using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblInvoiceItemDetailsDAO
    {
        String SqlSelectQuery();
        List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetails();
        TblInvoiceItemDetailsTO SelectTblInvoiceItemDetails(Int32 idInvoiceItem);
        TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTOByInvoice(Int32 idInvoice);
        List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetails(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceItemDetailsTO SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceItemDetailsTO> ConvertDTToList(SqlDataReader tblInvoiceItemDetailsTODT);
        int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO);
        int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdInsert);
        int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO);
        int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem);
        int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvoiceItem, SqlCommand cmdDelete);
        int DeleteTblInvoiceItemDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete);

    }
}