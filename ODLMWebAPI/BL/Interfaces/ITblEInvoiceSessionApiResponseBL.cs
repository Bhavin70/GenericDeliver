using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblEInvoiceSessionApiResponseBL
    {
        List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponseList();
        List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponseList(Int32 apiId);
        List<TblEInvoiceSessionApiResponseTO> SelectTblEInvoiceSessionApiResponseList(Int32 idResponse);
        int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO);
        int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblEInvoiceSessionApiResponse(Int32 idApi);
        int DeleteTblEInvoiceSessionApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran);
    }
}