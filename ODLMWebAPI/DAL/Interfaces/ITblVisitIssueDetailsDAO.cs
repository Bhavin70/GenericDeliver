using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitIssueDetailsDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblVisitIssueDetails();
        DataTable SelectTblVisitIssueDetails(Int32 idIssue);
        DataTable SelectAllTblVisitIssueDetails(SqlConnection conn, SqlTransaction tran);
        List<TblVisitIssueDetailsTO> SelectVisitIssueDetailsList(Int32 visitId);
        List<TblVisitIssueDetailsTO> ConvertDTToList(SqlDataReader visitIssueDetailsDT);
        int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO);
        int InsertTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlCommand cmdInsert);
        int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO);
        int UpdateTblVisitIssueDetails(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitIssueDetailsTO tblVisitIssueDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblVisitIssueDetails(Int32 idIssue);
        int DeleteTblVisitIssueDetails(Int32 idIssue, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idIssue, SqlCommand cmdDelete);

    }
}