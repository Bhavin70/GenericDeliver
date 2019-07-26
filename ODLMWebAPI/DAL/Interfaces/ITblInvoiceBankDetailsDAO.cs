using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblInvoiceBankDetailsDAO
    {
        String SqlSelectQuery();
        List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails();
        TblInvoiceBankDetailsTO SelectTblInvoiceBankDetails(Int32 idBank);
        List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails(SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceBankDetailsTO> SelectInvoiceBankDetails(Int32 organizationId);
        List<TblInvoiceBankDetailsTO> ConvertDTToList(SqlDataReader tblInvoiceBankDetailsTODT);
        int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO);
        int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlCommand cmdInsert);
        int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO);
        int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblInvoiceBankDetails(Int32 idBank);
        int DeleteTblInvoiceBankDetails(Int32 idBank, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBank, SqlCommand cmdDelete);

    }
}