using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceItemTaxDtlsBL
    {
        List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList();
        TblInvoiceItemTaxDtlsTO SelectTblInvoiceItemTaxDtlsTO(Int32 idInvItemTaxDtl);
        List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId);
        List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId);
        int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO);
        int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO);
        int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl);
        int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteInvoiceItemTaxDtlsByInvId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
    }
}
