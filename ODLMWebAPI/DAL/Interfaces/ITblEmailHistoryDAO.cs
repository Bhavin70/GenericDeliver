using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblEmailHistoryDAO
    {
        String SqlSelectQuery();
        int InsertTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO);
        int InsertTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblEmailHistoryTO tblEmailHistoryTO, SqlCommand cmdInsert);
        int UpdateTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO);
        int UpdateTblEmailHistory(TblEmailHistoryTO tblEmailHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblEmailHistoryTO tblEmailHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblEmailHistory(Int32 idEmailHistory);
        int DeleteTblEmailHistory(Int32 idEmailHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idEmailHistory, SqlCommand cmdDelete);

    }
}