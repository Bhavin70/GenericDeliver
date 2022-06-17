using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblEInvoiceSessionApiResponseDAO
    {
        String SqlSelectQuery();
        List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponse();
        List<TblEInvoiceSessionApiResponseTO> SelectAllTblEInvoiceSessionApiResponse(Int32 apiId);
        List<TblEInvoiceSessionApiResponseTO> SelectTblEInvoiceSessionApiResponse(Int32 idResponse);
        List<TblEInvoiceSessionApiResponseTO> ConvertDTToList(SqlDataReader TblEInvoiceSessionApiResponseTODT);
        int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO);
        int InsertTblEInvoiceSessionApiResponse(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEInvoiceSessionApiResponseTO TblEInvoiceSessionApiResponseTO, SqlCommand cmdInsert);
        int DeleteTblEInvoiceSessionApiResponse(Int32 idApi);
        int DeleteTblEInvoiceSessionApiResponse(Int32 idApi, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete);
    }
}