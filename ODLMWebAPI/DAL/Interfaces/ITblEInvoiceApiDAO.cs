using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblEInvoiceApiDAO
    {
        String SqlSelectQuery();
        List<TblEInvoiceApiTO> SelectAllTblEInvoiceApi();
        List<TblEInvoiceApiTO> SelectAllTblEInvoiceApi(Int32 idApi);
        List<TblEInvoiceApiTO> SelectTblEInvoiceApi(string apiName, int OrgId = 0);
        List<TblEInvoiceApiTO> ConvertDTToList(SqlDataReader tblEInvoiceApiTODT);
        int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO);
        int InsertTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEInvoiceApiTO tblEInvoiceApiTO, SqlCommand cmdInsert);
        int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO);
        int UpdateTblEInvoiceApi(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblEInvoiceApiTO tblEInvoiceApiTO, SqlCommand cmdUpdate);
        int UpdateTblEInvoiceApiSession(TblEInvoiceApiTO tblEInvoiceApiTO);
        int UpdateTblEInvoiceApiSession(TblEInvoiceApiTO tblEInvoiceApiTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteSessionUpdationCommand(TblEInvoiceApiTO tblEInvoiceApiTO, SqlCommand cmdUpdate);
        int DeleteTblEInvoiceApi(Int32 idApi);
        int DeleteTblEInvoiceApi(Int32 idApi, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLocation, SqlCommand cmdDelete);
    }
}