using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITempInvoiceDocumentDetailsDAO
    {
        String SqlSelectQuery();
        List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails();
        TempInvoiceDocumentDetailsTO SelectTempInvoiceDocumentDetails(Int32 idInvoiceDocument);
        List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails(SqlConnection conn, SqlTransaction tran);
        List<TempInvoiceDocumentDetailsTO> ConvertDTToList(SqlDataReader tempInvoiceDocumentDetailsTODT);
        List<TempInvoiceDocumentDetailsTO> SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(Int32 invoiceId);
        List<TempInvoiceDocumentDetailsTO> SelectTempInvoiceDocumentDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO);
        int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdInsert);
        int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO);
        int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlCommand cmdUpdate);
        int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument);
        int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvoiceDocument, SqlCommand cmdDelete);
        int DeleteTblInvoiceDocumentByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByInvoiceId(Int32 invoiceId, SqlCommand cmdDelete);

    }
}