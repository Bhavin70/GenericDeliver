using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblOrgOverdueHistoryBL : ITblOrgOverdueHistoryBL
    {
        private readonly ITblOrgOverdueHistoryDAO _iTblOrgOverdueHistoryDAO;
        public TblOrgOverdueHistoryBL(ITblOrgOverdueHistoryDAO iTblOrgOverdueHistoryDAO)
        {
            _iTblOrgOverdueHistoryDAO = iTblOrgOverdueHistoryDAO;
        }
        #region Selection
        public List<TblOrgOverdueHistoryTO> SelectAllTblOrgOverdueHistory()
        {
            return _iTblOrgOverdueHistoryDAO.SelectAllTblOrgOverdueHistory();
        }

        #endregion
        
        #region Insertion
        public int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO)
        {
            return _iTblOrgOverdueHistoryDAO.InsertTblOrgOverdueHistory(tblOrgOverdueHistoryTO);
        }

        public int InsertTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgOverdueHistoryDAO.InsertTblOrgOverdueHistory(tblOrgOverdueHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO)
        {
            return _iTblOrgOverdueHistoryDAO.UpdateTblOrgOverdueHistory(tblOrgOverdueHistoryTO);
        }

        public int UpdateTblOrgOverdueHistory(TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgOverdueHistoryDAO.UpdateTblOrgOverdueHistory(tblOrgOverdueHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory)
        {
            return _iTblOrgOverdueHistoryDAO.DeleteTblOrgOverdueHistory(idOrgOverdueHistory);
        }

        public int DeleteTblOrgOverdueHistory(Int32 idOrgOverdueHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgOverdueHistoryDAO.DeleteTblOrgOverdueHistory(idOrgOverdueHistory, conn, tran);
        }

        #endregion
        
    }
}
