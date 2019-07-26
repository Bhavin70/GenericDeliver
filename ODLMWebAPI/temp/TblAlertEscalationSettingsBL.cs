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
    public class TblAlertEscalationSettingsBL : ITblAlertEscalationSettingsBL
    {
        #region Selection
       
        public List<TblAlertEscalationSettingsTO> SelectAllTblAlertEscalationSettingsList()
        {
           return  TblAlertEscalationSettingsDAO.SelectAllTblAlertEscalationSettings();
        }

        public TblAlertEscalationSettingsTO SelectTblAlertEscalationSettingsTO(Int32 idEscalationSetting)
        {
            return  TblAlertEscalationSettingsDAO.SelectTblAlertEscalationSettings(idEscalationSetting);
        }

        #endregion
        
        #region Insertion
        public int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return TblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public int InsertTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertEscalationSettingsDAO.InsertTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO)
        {
            return TblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO);
        }

        public int UpdateTblAlertEscalationSettings(TblAlertEscalationSettingsTO tblAlertEscalationSettingsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertEscalationSettingsDAO.UpdateTblAlertEscalationSettings(tblAlertEscalationSettingsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting)
        {
            return TblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting);
        }

        public int DeleteTblAlertEscalationSettings(Int32 idEscalationSetting, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertEscalationSettingsDAO.DeleteTblAlertEscalationSettings(idEscalationSetting, conn, tran);
        }

        #endregion
        
    }
}
