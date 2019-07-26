using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingStatusHistoryBL
    {
        List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistoryList(Int32 loadingId);
        List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistoryList(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        TblLoadingStatusHistoryTO SelectTblLoadingStatusHistoryTO(Int32 idLoadingHistory);
        int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO);
        int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO);
        int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory);
        int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran);
    }
}