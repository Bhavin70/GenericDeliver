using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using Microsoft.Extensions.Logging;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblTranActionsBL : ITblTranActionsBL
    {
        private readonly ITblTranActionsDAO _iTblTranActionsDAO;
        public TblTranActionsBL(ITblTranActionsDAO iTblTranActionsDAO)
        {
            _iTblTranActionsDAO = iTblTranActionsDAO;
        }
        #region Selection
        public List<TblTranActionsTO> SelectAllTblTranActions()
        {
            return _iTblTranActionsDAO.SelectAllTblTranActions();
        }

        public List<TblTranActionsTO> SelectAllTblTranActionsList(TblTranActionsTO tblTranActionsTO)
        {
            return _iTblTranActionsDAO.SelectAllTblTranActionsList(tblTranActionsTO);
        }

        public TblTranActionsTO SelectTblTranActionsTO(Int32 idTranActions)
        {
            return _iTblTranActionsDAO.SelectTblTranActions(idTranActions);
        }

       
        #endregion
        
        #region Insertion
        public int InsertTblTranActions(TblTranActionsTO tblTranActionsTO)
        {
            return _iTblTranActionsDAO.InsertTblTranActions(tblTranActionsTO);
        }

        public int InsertTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTranActionsDAO.InsertTblTranActions(tblTranActionsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO)
        {
            return _iTblTranActionsDAO.UpdateTblTranActions(tblTranActionsTO);
        }

        public int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTranActionsDAO.UpdateTblTranActions(tblTranActionsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblTranActions(Int32 idTranActions)
        {
            return _iTblTranActionsDAO.DeleteTblTranActions(idTranActions);
        }

        public int DeleteTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblTranActionsDAO.DeleteTblTranActions(idTranActions, conn, tran);
        }

        #endregion
        
    }
}
