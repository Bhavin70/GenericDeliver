using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblOrgOverdueHistoryDAO
    {
        String SqlSelectQuery();
        List<TblOrgOverdueHistoryTO> SelectAllTblOrgOverdueHistory();
        List<TblOrgOverdueHistoryTO> SelectAllTblOrgOverdueHistory(SqlConnection conn, SqlTransaction tran);
        List<TblOrgOverdueHistoryTO> ConvertDTToList(SqlDataReader tblOrgOverdueHistoryTODT);
        int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO);
        int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlCommand cmdInsert);
        int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO);
        int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlCommand cmdUpdate);
        int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory);
        int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgOverdueHistory, SqlCommand cmdDelete);

    }
}