using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITempInvoiceDocumentDetailsBL
    {
        List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails();
        TempInvoiceDocumentDetailsTO SelectTempInvoiceDocumentDetailsTO(Int32 idInvoiceDocument);
        List<TempInvoiceDocumentDetailsTO> SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(Int32 invoiceId);
        List<TempInvoiceDocumentDetailsTO> SelectTempInvoiceDocumentDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO);
        int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO);
        int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument);
        int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceDocumentByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
    }
}