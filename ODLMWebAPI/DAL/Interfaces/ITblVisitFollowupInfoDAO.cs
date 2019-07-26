using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitFollowupInfoDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblVisitFollowupInfo();
        DataTable SelectTblVisitFollowupInfo(Int32 idProjectFollowUpInfo);
        DataTable SelectAllTblVisitFollowupInfo(SqlConnection conn, SqlTransaction tran);
        TblVisitFollowupInfoTO SelectVisitFollowupInfo(Int32 visitId);
        List<TblVisitFollowupInfoTO> ConvertDTToList(SqlDataReader visitFollowupInfoDT);
        int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO);
        int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlCommand cmdInsert);
        int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO);
        int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlCommand cmdUpdate);
        int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo);
        int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProjectFollowUpInfo, SqlCommand cmdDelete);

    }
}