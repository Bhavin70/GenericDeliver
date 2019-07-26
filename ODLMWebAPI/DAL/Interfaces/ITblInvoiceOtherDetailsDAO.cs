using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblInvoiceOtherDetailsDAO
    {
        String SqlSelectQuery();
        List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails();
        TblInvoiceOtherDetailsTO SelectTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails);
        List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails(SqlConnection conn, SqlTransaction tran);
        List<TblInvoiceOtherDetailsTO> ConvertDTToList(SqlDataReader tblInvoiceOtherDetailsTODT);
        List<TblInvoiceOtherDetailsTO> SelectInvoiceOtherDetails(Int32 organizationId);
        int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO);
        int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlCommand cmdInsert);
        int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO);
        int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails);
        int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idInvoiceOtherDetails, SqlCommand cmdDelete);

    }
}