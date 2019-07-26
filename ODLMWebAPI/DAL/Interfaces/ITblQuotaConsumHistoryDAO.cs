using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblQuotaConsumHistoryDAO
    {
        String SqlSelectQuery();
        List<TblQuotaConsumHistoryTO> SelectAllTblQuotaConsumHistory();
        TblQuotaConsumHistoryTO SelectTblQuotaConsumHistory(Int32 idQuotaConsmHIstory);
        List<TblQuotaConsumHistoryTO> ConvertDTToList(SqlDataReader tblQuotaConsumHistoryTODT);
        int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO);
        int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlCommand cmdInsert);
        int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO);
        int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory);
        int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idQuotaConsmHIstory, SqlCommand cmdDelete);

    }
}