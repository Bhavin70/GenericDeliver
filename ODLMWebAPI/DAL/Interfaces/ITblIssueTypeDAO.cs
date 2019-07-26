using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblIssueTypeDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblIssueType();
        DataTable SelectTblIssueType(Int32 idIssueType);
        DataTable SelectAllTblIssueType(SqlConnection conn, SqlTransaction tran);
        int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO);
        int InsertTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblIssueTypeTO tblIssueTypeTO, SqlCommand cmdInsert);
        int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO);
        int UpdateTblIssueType(TblIssueTypeTO tblIssueTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblIssueTypeTO tblIssueTypeTO, SqlCommand cmdUpdate);
        int DeleteTblIssueType(Int32 idIssueType);
        int DeleteTblIssueType(Int32 idIssueType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idIssueType, SqlCommand cmdDelete);

    }
}