using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingStatusHistoryDAO
    {
        String SqlSelectQuery();
        List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistory(int loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistory(int loadingId);
        TblLoadingStatusHistoryTO SelectTblLoadingStatusHistory(Int32 idLoadingHistory);
        List<TblLoadingStatusHistoryTO> ConvertDTToList(SqlDataReader tblLoadingStatusHistoryTODT);
        int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO);
        int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlCommand cmdInsert);
        int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO);
        int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory);
        int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingHistory, SqlCommand cmdDelete);

    }
}