using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitIssueReasonsDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblVisitIssueReasons();
        DataTable SelectTblVisitIssueReasons(Int32 idVisitIssueReasons);
        DataTable SelectAllTblVisitIssueReasons(SqlConnection conn, SqlTransaction tran);
        List<TblVisitIssueReasonsTO> SelectAllTblVisitIssueReasonsForDropDOwn();
        List<TblVisitIssueReasonsTO> ConvertDTToList(SqlDataReader visitIssueReasonDT);
        int InsertTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO);
        int InsertTblVisitIssueReasons(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(ref TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlCommand cmdInsert);
        int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO);
        int UpdateTblVisitIssueReasons(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitIssueReasonsTO tblVisitIssueReasonsTO, SqlCommand cmdUpdate);
        int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons);
        int DeleteTblVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVisitIssueReasons, SqlCommand cmdDelete);

    }
}