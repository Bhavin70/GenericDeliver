using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingSlipExtHistoryBL
    {
        List<TblLoadingSlipExtHistoryTO> SelectAllTblLoadingSlipExtHistoryList();
        TblLoadingSlipExtHistoryTO SelectTblLoadingSlipExtHistoryTO(Int32 idConfirmHistory);
        List<TblLoadingSlipExtHistoryTO> SelectTempLoadingSlipExtHistoryTOList(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO);
        int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO);
        int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory);
        int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory, SqlConnection conn, SqlTransaction tran);
        int DeleteLoadingSlipExtHistoryForItem(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
    }
}