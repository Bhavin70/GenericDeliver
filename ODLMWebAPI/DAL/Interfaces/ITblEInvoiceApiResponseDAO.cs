using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblEInvoiceApiResponseDAO
    {
        String SqlSelectQuery();
        List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponse();
        List<TblEInvoiceApiResponseTO> SelectAllTblEInvoiceApiResponse(Int32 apiId);
        List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseList(int idResponse);
        List<TblEInvoiceApiResponseTO> SelectTblEInvoiceApiResponseListForInvoiceId(int invoiceId);
        List<TblEInvoiceApiResponseTO> ConvertDTToList(SqlDataReader TblEInvoiceApiResponseTODT);
        int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO);
        int InsertTblEInvoiceApiResponse(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEInvoiceApiResponseTO TblEInvoiceApiResponseTO, SqlCommand cmdInsert);
        int DeleteTblEInvoiceApiResponse(Int32 idApi);
        int DeleteTblEInvoiceApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete);
    }
}