using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAlertSubscribersDAO
    {
        String SqlSelectQuery();
        List<TblAlertSubscribersTO> SelectAllTblAlertSubscribers();
        List<TblAlertSubscribersTO> SelectTblAlertSubscribersByAlertDefId(Int32 alertDefId);
        List<TblAlertSubscribersTO> SelectAllTblAlertSubscribers(Int32 alertDefId, SqlConnection conn, SqlTransaction tran);
        TblAlertSubscribersTO SelectTblAlertSubscribers(Int32 idSubscription);
        List<TblAlertSubscribersTO> ConvertDTToList(SqlDataReader tblAlertSubscribersTODT);
        int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertSubscribersTO tblAlertSubscribersTO, SqlCommand cmdInsert);
        int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertSubscribersTO tblAlertSubscribersTO, SqlCommand cmdUpdate);
        int DeleteTblAlertSubscribers(Int32 idSubscription);
        int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSubscription, SqlCommand cmdDelete);

    }
}