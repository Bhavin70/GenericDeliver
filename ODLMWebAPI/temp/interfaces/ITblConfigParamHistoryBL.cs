using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblConfigParamHistoryBL
    {
        List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistoryList();
        TblConfigParamHistoryTO SelectTblConfigParamHistoryTO(Int32 idParamHistory);
        int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO);
        int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO);
        int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblConfigParamHistory(Int32 idParamHistory);
        int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran);
    }
}
