using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblAlertSubscriptSettingsBL
    {
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList();
        TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsTO(Int32 idSubscriSettings);
        TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsFromNotifyId(Int32 NotificationTypeId, Int32 SubscriptionId, Int32 AlertDefId);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId, SqlConnection conn, SqlTransaction tran);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId, SqlConnection conn, SqlTransaction tran);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran);
    }
}
