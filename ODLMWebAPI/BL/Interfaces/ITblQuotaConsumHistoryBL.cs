using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblQuotaConsumHistoryBL
    {
        List<TblQuotaConsumHistoryTO> SelectAllTblQuotaConsumHistoryList();
        TblQuotaConsumHistoryTO SelectTblQuotaConsumHistoryTO(Int32 idQuotaConsmHIstory);
        int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO);
        int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO);
        int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory);
        int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory, SqlConnection conn, SqlTransaction tran);
    }
}