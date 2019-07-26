using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblUserPwdHistoryBL : ITblUserPwdHistoryBL
    {
        private readonly ITblUserPwdHistoryDAO _iTblUserPwdHistoryDAO;
        public TblUserPwdHistoryBL(ITblUserPwdHistoryDAO iTblUserPwdHistoryDAO)
        {
            _iTblUserPwdHistoryDAO = iTblUserPwdHistoryDAO;
        }
        #region Selection
        public List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistory()
        {
            return _iTblUserPwdHistoryDAO.SelectAllTblUserPwdHistory();
        }

        public List<TblUserPwdHistoryTO> SelectAllTblUserPwdHistoryList()
        {
            return _iTblUserPwdHistoryDAO.SelectAllTblUserPwdHistory();
        }

        public TblUserPwdHistoryTO SelectTblUserPwdHistoryTO(Int32 idUserPwdHistory)
        {
            return _iTblUserPwdHistoryDAO.SelectTblUserPwdHistory(idUserPwdHistory);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            return _iTblUserPwdHistoryDAO.InsertTblUserPwdHistory(tblUserPwdHistoryTO);
        }

        public int InsertTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserPwdHistoryDAO.InsertTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO)
        {
            return _iTblUserPwdHistoryDAO.UpdateTblUserPwdHistory(tblUserPwdHistoryTO);
        }

        public int UpdateTblUserPwdHistory(TblUserPwdHistoryTO tblUserPwdHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserPwdHistoryDAO.UpdateTblUserPwdHistory(tblUserPwdHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblUserPwdHistory(Int32 idUserPwdHistory)
        {
            return _iTblUserPwdHistoryDAO.DeleteTblUserPwdHistory(idUserPwdHistory);
        }

        public int DeleteTblUserPwdHistory(Int32 idUserPwdHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblUserPwdHistoryDAO.DeleteTblUserPwdHistory(idUserPwdHistory, conn, tran);
        }

        #endregion
        
    }
}
