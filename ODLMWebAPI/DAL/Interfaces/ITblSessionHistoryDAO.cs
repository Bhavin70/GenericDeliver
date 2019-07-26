using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSessionHistoryDAO
    {
        String SqlSelectQuery();
        TblSessionHistoryTO SelectAllTblSessionHistory();
        List<TblSessionHistoryTO> SelectAllTblSessionHistoryData();
        List<TblSessionHistoryTO> SelectTblSessionHistory(Int32 idSessionHistory);
        DataTable SelectAllTblSessionHistory(SqlConnection conn, SqlTransaction tran);
        int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO);
        int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSessionHistoryTO tblSessionHistoryTO, SqlCommand cmdInsert);
        int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO);
        int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSessionHistoryTO tblSessionHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblSessionHistory(Int32 idSessionHistory);
        int DeleteTblSessionHistory();
        int DeleteTblSessionHistory(Int32 idSessionHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSessionHistory, SqlCommand cmdDelete);
        int ExecuteDeletionCommand(SqlCommand cmdDelete);
        List<TblSessionHistoryTO> ConvertDTToList(SqlDataReader tblSessionHistoryTODT);

    }
}