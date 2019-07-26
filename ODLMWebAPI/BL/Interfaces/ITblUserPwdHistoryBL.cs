using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserPwdHistoryBL
    {
        List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory();
        List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistoryList();
        TblUserPwdHistoryTO SelectTblUserPwdHistoryTO(Int32 idUserPwdHistory);
        int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO);
        int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO);
        int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserPwdHistory(Int32 idUserPwdHistory);
        int DeleteTblUserPwdHistory(Int32 idUserPwdHistory, SqlConnection conn, SqlTransaction tran);
    }
}