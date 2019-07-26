using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimVisitIssueReasonsDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllDimVisitIssueReasons();
        DataTable SelectDimVisitIssueReasons(Int32 idVisitIssueReasons);
        DataTable SelectAllDimVisitIssueReasons(SqlConnection conn, SqlTransaction tran);
        int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO);
        int InsertDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlCommand cmdInsert);
        int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO);
        int UpdateDimVisitIssueReasons(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimVisitIssueReasonsTO dimVisitIssueReasonsTO, SqlCommand cmdUpdate);
        int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons);
        int DeleteDimVisitIssueReasons(Int32 idVisitIssueReasons, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVisitIssueReasons, SqlCommand cmdDelete);

    }
}