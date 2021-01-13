using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblEInvoiceApiResponseBL
    {
        List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponseList();
        List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponseList(Int32 apiId);
        List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseList(Int32 idResponse);
        List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(Int32 invoiceId);
        List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);
        int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO);
        int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblEInvoiceApiResponse(Int32 idApi);
        int DeleteTblEInvoiceApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran);
    }
}