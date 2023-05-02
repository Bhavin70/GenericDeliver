using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceItemDetailsBL
    {
        List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList();
        TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTO(Int32 idInvoiceItem);
        TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTOByInvoice(Int32 idInvoice);
        List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId);
        List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceItemDetailsTO SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO);
        int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO);
        int UpdateTestCertiOfInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO,SqlConnection conn,SqlTransaction tran);
        int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem);
        int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceItemDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
    }
}
