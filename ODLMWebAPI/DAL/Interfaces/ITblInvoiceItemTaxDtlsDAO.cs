using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblInvoiceItemTaxDtlsDAO
    {
        String SqlSelectQuery();
        List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtls();
        List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtls(Int32 invItemId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        TblInvoiceItemTaxDtlsTO SelectTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl);
        List<TblInvoiceItemTaxDtlsTO> ConvertDTToList(SqlDataReader tblInvoiceItemTaxDtlsTODT);
        int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO);
        int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdInsert);
        int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO);
        int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlCommand cmdUpdate);
        int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl);
        int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteInvoiceItemTaxDtlsByInvId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvItemTaxDtl, SqlCommand cmdDelete);

    }
}