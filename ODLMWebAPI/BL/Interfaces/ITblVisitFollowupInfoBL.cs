using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitFollowupInfoBL
    {
        DataTable SelectAllTblVisitFollowupInfo();
        List<TblVisitFollowupInfoTO> SelectAllTblVisitFollowupInfoList();
        TblVisitFollowupInfoTO SelectTblVisitFollowupInfoTO(Int32 idProjectFollowUpInfo);
        List<TblVisitFollowupInfoTO> ConvertDTToList(DataTable tblVisitFollowupInfoTODT);
        TblVisitFollowupInfoTO SelectVisitFollowupInfoTO(Int32 visitid);
        int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO);
        int InsertTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveVisitFollowUpInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO);
        int UpdateTblVisitFollowupInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateVisitFollowUpInfo(TblVisitFollowupInfoTO tblVisitFollowupInfoTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo);
        int DeleteTblVisitFollowupInfo(Int32 idProjectFollowUpInfo, SqlConnection conn, SqlTransaction tran);
    }
}