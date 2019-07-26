using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblConfigParamHistoryBL : ITblConfigParamHistoryBL
    {
        private readonly ITblConfigParamHistoryDAO _iTblConfigParamHistoryDAO;
        public TblConfigParamHistoryBL(ITblConfigParamHistoryDAO iTblConfigParamHistoryDAO)
        {
            _iTblConfigParamHistoryDAO = iTblConfigParamHistoryDAO;
        }
        #region Selection

        public List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistoryList()
        {
            return _iTblConfigParamHistoryDAO.SelectAllTblConfigParamHistory();
        }

        public TblConfigParamHistoryTO SelectTblConfigParamHistoryTO(Int32 idParamHistory)
        {
            return  _iTblConfigParamHistoryDAO.SelectTblConfigParamHistory(idParamHistory);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return _iTblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return _iTblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblConfigParamHistory(Int32 idParamHistory)
        {
            return _iTblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory);
        }

        public int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory, conn, tran);
        }

        #endregion
        
    }
}
