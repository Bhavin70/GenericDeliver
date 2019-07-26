using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOrgOverdueHistoryBL
    {
        List<TblOrgOverdueHistoryTO> SelectAllTblOrgOverdueHistory();
        int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO);
        int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO);
        int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory);
        int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory, SqlConnection conn, SqlTransaction tran);
    }
}