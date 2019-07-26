using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceAddressBL
    {
        List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList();
        TblInvoiceAddressTO SelectTblInvoiceAddressTO(Int32 idInvoiceAddr);
        List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId);
        List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO);
        int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO);
        int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceAddress(Int32 idInvoiceAddr);
        int DeleteTblInvoiceAddress(Int32 idInvoiceAddr, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceAddressByinvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
    }
}
