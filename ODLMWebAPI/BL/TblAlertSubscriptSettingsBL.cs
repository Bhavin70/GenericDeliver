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
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{   
    public class TblAlertSubscriptSettingsBL : ITblAlertSubscriptSettingsBL
    {
        private readonly ITblAlertSubscriptSettingsDAO _iTblAlertSubscriptSettingsDAO;
        public TblAlertSubscriptSettingsBL(ITblAlertSubscriptSettingsDAO iTblAlertSubscriptSettingsDAO)
        {
            _iTblAlertSubscriptSettingsDAO = iTblAlertSubscriptSettingsDAO;
        }
        #region Selection

        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList()
        {
            return  _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings();
        }

        public TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsTO(Int32 idSubscriSettings)
        {
            return  _iTblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettings(idSubscriSettings);
        }

        //Priyanka [24-09-18] : Added to get the tblAlertSubscriptSetting TO from subscriptionId.
        public TblAlertSubscriptSettingsTO SelectTblAlertSubscriptSettingsFromNotifyId(Int32 NotificationTypeId, Int32 SubscriptionId, Int32 AlertDefId)
        {
            return _iTblAlertSubscriptSettingsDAO.SelectTblAlertSubscriptSettingsFromNotifyId(NotificationTypeId, SubscriptionId, AlertDefId);
        }
        
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId,conn,tran);
        }

        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsList(int subscriptionId)
        {
            return _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettings(subscriptionId);
        }

        //Priyanka [25-09-2018] : Added to get the alert subscription setting list by alert defination Id.
        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId)
        {
            return _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId);
        }

        public List<TblAlertSubscriptSettingsTO> SelectAllTblAlertSubscriptSettingsListByAlertDefId(int alertDefId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.SelectAllTblAlertSubscriptSettingsByAlertDefId(alertDefId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return _iTblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public int InsertTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.InsertTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO)
        {
            return _iTblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO);
        }

        public int UpdateTblAlertSubscriptSettings(TblAlertSubscriptSettingsTO tblAlertSubscriptSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.UpdateTblAlertSubscriptSettings(tblAlertSubscriptSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings)
        {
            return _iTblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings);
        }

        public int DeleteTblAlertSubscriptSettings(Int32 idSubscriSettings, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertSubscriptSettingsDAO.DeleteTblAlertSubscriptSettings(idSubscriSettings, conn, tran);
        }

        #endregion
        
    }
}
