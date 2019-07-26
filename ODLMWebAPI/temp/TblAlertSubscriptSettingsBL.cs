using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblAlertSubscriptSettingsBL : ITblAlertSubscriptSettingsBL
    {
        #region Selection
       
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList()
        {
            return  TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings();
        }

        public TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsTO(Int32 idSubscriSettings)
        {
            return  TblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettings(idSubscriSettings);
        }

        //Priyanka [24-09-18] : Added to get the tblAlertSubscriptSetting TO from subscriptionId.
        public TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsFromNotifyId(Int32 NotificationTypeId, Int32 SubscriptionId, Int32 AlertDefId)
        {
            return TblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettingsFromNotifyId(NotificationTypeId, SubscriptionId, AlertDefId);
        }
        
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId,SqlConnection conn,SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId,conn,tran);
        }

        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId);
        }

        //Priyanka [25-09-2018] : Added to get the alert subscription setting list by alert defination Id.
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId);
        }

        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return TblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return TblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            return TblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings);
        }

        public int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings, conn, tran);
        }

        #endregion
        
    }
}
