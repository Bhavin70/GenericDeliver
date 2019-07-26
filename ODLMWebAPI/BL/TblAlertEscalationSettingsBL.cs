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
    public class TblAlertEscalationSettingsBL : ITblAlertEscalationSettingsBL
    {
        private readonly ITblAlertEscalationSettingsDAO _iTblAlertEscalationSettingsDAO;
        public TblAlertEscalationSettingsBL(ITblAlertEscalationSettingsDAO iTblAlertEscalationSettingsDAO)
        {
            _iTblAlertEscalationSettingsDAO = iTblAlertEscalationSettingsDAO;
        }
        #region Selection

        public List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettingsList()
        {
           return  _iTblAlertEscalationSettingsDAO.SelectAllTblAlertEscalationSettings();
        }

        public TblAlertEscalationSettingsTO SelectTblAlertEscalationSettingsTO(Int32 idEscalationSetting)
        {
            return  _iTblAlertEscalationSettingsDAO.SelectTblAlertEscalationSettings(idEscalationSetting);
        }

        #endregion
        
        #region Insertion
        public int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return _iTblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return _iTblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting)
        {
            return _iTblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting);
        }

        public int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting, conn, tran);
        }

        #endregion
        
    }
}
