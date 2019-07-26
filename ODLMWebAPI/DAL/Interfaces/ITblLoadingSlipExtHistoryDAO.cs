using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipExtHistoryDAO
    {
        String SqlSelectQuery();
        List<TblLoadingSlipExtHistoryTO> SelectAllTblLoadingSlipExtHistory();
        TblLoadingSlipExtHistoryTO SelectTblLoadingSlipExtHistory(Int32 idConfirmHistory);
        List<TblLoadingSlipExtHistoryTO> ConvertDTToList(SqlDataReader tblLoadingSlipExtHistoryTODT);
        List<TblLoadingSlipExtHistoryTO> SelectTempLoadingSlipExtHistoryList(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO);
        int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlCommand cmdInsert);
        int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO);
        int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory);
        int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idConfirmHistory, SqlCommand cmdDelete);
        int DeleteLoadingSlipExtHistoryForItem(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran);

    }
}