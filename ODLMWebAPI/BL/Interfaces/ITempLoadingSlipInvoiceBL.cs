using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITempLoadingSlipInvoiceBL
    {
        List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoice();
        TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTO(Int32 idLoadingSlipInvoice);
        List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoiceList(string loadingSlipIds);
        List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOList(int loadingSlipId, int invoiceId, SqlConnection conn, SqlTransaction tran);
        TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOListByLoadingSlip(int loadingSlipId, SqlConnection conn, SqlTransaction tran);
        int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO);
        int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO);
        int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice);
        int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice, SqlConnection conn, SqlTransaction tran);
        int DeleteTempLoadingSlipInvoiceByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
    }
}