using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblInvoiceOtherDetailsBL
    {
        List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails();
        TblInvoiceOtherDetailsTO SelectTblInvoiceOtherDetailsTO(Int32 idInvoiceOtherDetails);
        List<TblInvoiceOtherDetailsTO> SelectInvoiceOtherDetails(Int32 organizationId);
        int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO);
        int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO);
        int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails);
        int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails, SqlConnection conn, SqlTransaction tran);
    }
}
