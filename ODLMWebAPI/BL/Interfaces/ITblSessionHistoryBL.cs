using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSessionHistoryBL
    {
        TblSessionHistoryTO SelectAllTblSessionHistory();
        List<TblSessionHistoryTO> SelectAllTblSessionHistoryList();
        List<TblSessionHistoryTO> SelectTblSessionHistoryTO(Int32 idSessionHistory);
        int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO);
        int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO);
        int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSessionHistory(Int32 idSessionHistory);
        int DeleteTblSessionHistory();
        int DeleteTblSessionHistory(Int32 idSessionHistory, SqlConnection conn, SqlTransaction tran);
    }
}