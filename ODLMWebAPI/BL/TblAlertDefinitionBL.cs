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
    public class TblAlertDefinitionBL : ITblAlertDefinitionBL
    {
        private readonly ITblAlertDefinitionDAO _iTblAlertDefinitionDAO;
        private readonly ITblAlertSubscribersBL _iTblAlertSubscribersBL;
        private readonly IConnectionString _iConnectionString;
        public TblAlertDefinitionBL(IConnectionString iConnectionString, ITblAlertSubscribersBL iTblAlertSubscribersBL, ITblAlertDefinitionDAO iTblAlertDefinitionDAO)
        {
            _iTblAlertDefinitionDAO = iTblAlertDefinitionDAO;
            _iTblAlertSubscribersBL = iTblAlertSubscribersBL;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblAlertDefinitionTO> SelectAllTblAlertDefinitionList()
        {
            return  _iTblAlertDefinitionDAO.SelectAllTblAlertDefinition();
        }

        public TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef,SqlConnection conn,SqlTransaction tran)
        {
            TblAlertDefinitionTO tblAlertDefinitionTO= _iTblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
            if (tblAlertDefinitionTO != null)
                tblAlertDefinitionTO.AlertSubscribersTOList = _iTblAlertSubscribersBL.SelectAllTblAlertSubscribersList(tblAlertDefinitionTO.IdAlertDef, conn, tran);

            return tblAlertDefinitionTO;

        }

        #endregion

        #region Insertion
        public int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return _iTblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO);
        }

        public int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return _iTblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO);
        }

        public int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertDefinition(Int32 idAlertDef)
        {
            return _iTblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef);
        }

        public int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef, conn, tran);
        }

        #endregion
        
    }
}
