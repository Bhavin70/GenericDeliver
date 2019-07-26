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
    public class TblAlertDefinitionBL : ITblAlertDefinitionBL
    {
        #region Selection
        public List<TblAlertDefinitionTO> SelectAllTblAlertDefinitionList()
        {
            return  TblAlertDefinitionDAO.SelectAllTblAlertDefinition();
        }

        public TblAlertDefinitionTO SelectTblAlertDefinitionTO(Int32 idAlertDef)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
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
            TblAlertDefinitionTO tblAlertDefinitionTO= TblAlertDefinitionDAO.SelectTblAlertDefinition(idAlertDef, conn, tran);
            if (tblAlertDefinitionTO != null)
                tblAlertDefinitionTO.AlertSubscribersTOList = BL.TblAlertSubscribersBL.SelectAllTblAlertSubscribersList(tblAlertDefinitionTO.IdAlertDef, conn, tran);

            return tblAlertDefinitionTO;

        }

        #endregion

        #region Insertion
        public int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return TblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO);
        }

        public int InsertTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertDefinitionDAO.InsertTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO)
        {
            return TblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO);
        }

        public int UpdateTblAlertDefinition(TblAlertDefinitionTO tblAlertDefinitionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertDefinitionDAO.UpdateTblAlertDefinition(tblAlertDefinitionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblAlertDefinition(Int32 idAlertDef)
        {
            return TblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef);
        }

        public int DeleteTblAlertDefinition(Int32 idAlertDef, SqlConnection conn, SqlTransaction tran)
        {
            return TblAlertDefinitionDAO.DeleteTblAlertDefinition(idAlertDef, conn, tran);
        }

        #endregion
        
    }
}
