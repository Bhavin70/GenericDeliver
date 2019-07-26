using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitPurposeDAO
    {
        String SqlSelectQuery();
        List<TblVisitPurposeTO> SelectAllTblVisitPurpose();
        DataTable SelectTblVisitPurpose(Int32 idVisitPurpose);
        DataTable SelectAllTblVisitPurpose(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectVisitPurposeListForDropDown(int visitTypeId);
        List<TblVisitPurposeTO> ConvertDTToList(SqlDataReader visitPurposeDT);
        int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO);
        int InsertTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(ref TblVisitPurposeTO tblVisitPurposeTO, SqlCommand cmdInsert);
        int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO);
        int UpdateTblVisitPurpose(TblVisitPurposeTO tblVisitPurposeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitPurposeTO tblVisitPurposeTO, SqlCommand cmdUpdate);
        int DeleteTblVisitPurpose(Int32 idVisitPurpose);
        int DeleteTblVisitPurpose(Int32 idVisitPurpose, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVisitPurpose, SqlCommand cmdDelete);

    }
}