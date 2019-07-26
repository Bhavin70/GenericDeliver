using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblAlertUsersBL
    {      
        ResultMessage snoozeTblAlert(int alertUserId, int snoozeTime,int tranType);
        List<TblAlertUsersTO> SelectAllTblAlertUsersList();
        TblAlertUsersTO SelectTblAlertUsersTO(Int32 idAlertUser);
        List<TblAlertUsersTO> SelectAllActiveNotAKAlertList(Int32 userId, Int32 roleId);
        List<TblAlertUsersTO> SelectAllActiveAlertList(Int32 userId, List<TblUserRoleTO> tblUserRoleToList,Int32 loginId,Int32 ModuleId);
        int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO);
        int InsertTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO);
        int UpdateTblAlertUsers(TblAlertUsersTO tblAlertUsersTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertUsers(Int32 idAlertUser);
        int DeleteTblAlertUsers(Int32 idAlertUser, SqlConnection conn, SqlTransaction tran);
    }
}
