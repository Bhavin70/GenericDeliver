using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblEInvoiceApiBL
    {
        List<TblEInvoiceApiTO> SelectAllTblEInvoiceApiList();
        List<TblEInvoiceApiTO> SelectAllTblEInvoiceApiList(Int32 idApi);
        List<TblEInvoiceApiTO> SelectTblEInvoiceApiList(string apiName);
        int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO);
        int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO);
        int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblEInvoiceApi(Int32 idApi);
        int DeleteTblEInvoiceApi(Int32 idApi, SqlConnection conn, SqlTransaction tran);
    }
}