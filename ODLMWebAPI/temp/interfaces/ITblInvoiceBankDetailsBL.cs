using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceBankDetailsBL
    {
        List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails();
        TblInvoiceBankDetailsTO SelectTblInvoiceBankDetailsTO(Int32 idBank);
        List<TblInvoiceBankDetailsTO> SelectInvoiceBankDetails(Int32 organizationId);
        int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO);
        int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO);
        int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceBankDetails(Int32 idBank);
        int DeleteTblInvoiceBankDetails(Int32 idBank, SqlConnection conn, SqlTransaction tran);
    }
}
