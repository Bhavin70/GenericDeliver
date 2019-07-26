using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUserPwdHistoryDAO
    {
        String SqlSelectQuery();
        List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory();
        TblUserPwdHistoryTO SelectTblUserPwdHistory(Int32 idUserPwdHistory);
        List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory(SqlConnection conn, SqlTransaction tran);
        List<TblUserPwdHistoryTO> ConvertDTToList(SqlDataReader tblUserPwdHistoryTODT);
        int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO);
        int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlCommand cmdInsert);
        int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO);
        int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblUserPwdHistory(Int32 idUserPwdHistory);
        int DeleteTblUserPwdHistory(Int32 idUserPwdHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUserPwdHistory, SqlCommand cmdDelete);

    }
}