using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblAlertInstanceBL
    {
        List<TblAlertInstanceTO> SelectAllTblAlertInstanceList();
        TblAlertInstanceTO SelectTblAlertInstanceTO(Int32 idAlertInstance);
        List<TblAlertInstanceTO> SelectAllTblAlertInstanceList(Int32 userId, Int32 roleId);
        ResultMessage SaveAlertInstance(TblAlertInstanceTO alertInstanceTO);
        ResultMessage SaveNewAlertInstance(TblAlertInstanceTO alertInstanceTO, SqlConnection conn, SqlTransaction tran);
        void ResetOldAlerts(TblAlertInstanceTO alertInstanceTO);
        int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO);
        int InsertTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO);
        int UpdateTblAlertInstance(TblAlertInstanceTO tblAlertInstanceTO, SqlConnection conn, SqlTransaction tran);
        int ResetAlertInstance(int alertDefId, int sourceEntityId, int userId, SqlConnection conn, SqlTransaction tran);
        int ResetAlertInstanceByDef(string alertDefIds, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertInstance(Int32 idAlertInstance);
        int DeleteTblAlertInstance(Int32 idAlertInstance, SqlConnection conn, SqlTransaction tran);
    }
}
