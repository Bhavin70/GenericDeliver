using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblAlertSubscribersBL
    {
        List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList();
        TblAlertSubscribersTO SelectTblAlertSubscribersTO(Int32 idSubscription);
        List<TblAlertSubscribersTO> SelectTblAlertSubscribersByAlertDefId(Int32 alertDefId);
        List<TblAlertSubscribersTO> SelectAllTblAlertSubscribersList(Int32 alertDefId, SqlConnection conn, SqlTransaction tran);
        int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int InsertTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO);
        int UpdateTblAlertSubscribers(TblAlertSubscribersTO tblAlertSubscribersTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertSubscribers(Int32 idSubscription);
        int DeleteTblAlertSubscribers(Int32 idSubscription, SqlConnection conn, SqlTransaction tran);
    }
}
