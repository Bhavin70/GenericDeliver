using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITempLoadingSlipInvoiceDAO
    {
        String SqlSelectQuery();
        List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoice();
        TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice);
        List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoice(SqlConnection conn, SqlTransaction tran);
        List<TempLoadingSlipInvoiceTO> ConvertDTToList(SqlDataReader tempLoadingSlipInvoiceTODT);
        List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoiceList(String loadingSlipIds);
        List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOListByLoadingSlip(int loadingSlipId, SqlConnection conn, SqlTransaction tran);
        TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOList(int loadingSlipId, int invoiceId, SqlConnection conn, SqlTransaction tran);
        int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO);
        int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlCommand cmdInsert);
        int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO);
        int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlCommand cmdUpdate);
        int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice);
        int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingSlipInvoice, SqlCommand cmdDelete);
        int DeleteTempLoadingSlipInvoiceByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete);

    }
}