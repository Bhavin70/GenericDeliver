using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAlertSubscriptSettingsDAO
    {
        String SqlSelectQuery();
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings();
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings(int subscriptionId, SqlConnection conn, SqlTransaction tran);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettings(int subscriptionId);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsByAlertDefId(int alertDefId);
        List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsByAlertDefId(int alertDefId, SqlConnection conn, SqlTransaction tran);
        TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettings(Int32 idSubscriSettings);
        TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsFromNotifyId(Int32 NotificationTypeId, Int32 SubscriptionId, Int32 AlertDefId);
        List<TblAlertSubscriptSettingsTO> ConvertDTToList(SqlDataReader tblAlertSubscriptSettingsTODT);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdInsert);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO);
        int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlCommand cmdUpdate);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings);
        int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSubscriSettings, SqlCommand cmdDelete);

    }
}