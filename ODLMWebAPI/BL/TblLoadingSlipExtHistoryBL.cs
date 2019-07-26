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
    public class TblLoadingSlipExtHistoryBL : ITblLoadingSlipExtHistoryBL
    {
        private readonly ITblLoadingSlipExtHistoryDAO _iTblLoadingSlipExtHistoryDAO;
        public TblLoadingSlipExtHistoryBL(ITblLoadingSlipExtHistoryDAO iTblLoadingSlipExtHistoryDAO)
        {
            _iTblLoadingSlipExtHistoryDAO = iTblLoadingSlipExtHistoryDAO;
        }
        #region Selection
        public List<TblLoadingSlipExtHistoryTO> SelectAllTblLoadingSlipExtHistoryList()
        {
            return  _iTblLoadingSlipExtHistoryDAO.SelectAllTblLoadingSlipExtHistory();
        }

        public TblLoadingSlipExtHistoryTO SelectTblLoadingSlipExtHistoryTO(Int32 idConfirmHistory)
        {
           return  _iTblLoadingSlipExtHistoryDAO.SelectTblLoadingSlipExtHistory(idConfirmHistory);
        }

        // Vaibhav [08-Jan-2018] added to select temp data
        public List<TblLoadingSlipExtHistoryTO> SelectTempLoadingSlipExtHistoryTOList(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtHistoryDAO.SelectTempLoadingSlipExtHistoryList(loadingSlipExtId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO)
        {
            return _iTblLoadingSlipExtHistoryDAO.InsertTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO);
        }

        public int InsertTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtHistoryDAO.InsertTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO)
        {
            return _iTblLoadingSlipExtHistoryDAO.UpdateTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO);
        }

        public int UpdateTblLoadingSlipExtHistory(TblLoadingSlipExtHistoryTO tblLoadingSlipExtHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtHistoryDAO.UpdateTblLoadingSlipExtHistory(tblLoadingSlipExtHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory)
        {
            return _iTblLoadingSlipExtHistoryDAO.DeleteTblLoadingSlipExtHistory(idConfirmHistory);
        }

        public int DeleteTblLoadingSlipExtHistory(Int32 idConfirmHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtHistoryDAO.DeleteTblLoadingSlipExtHistory(idConfirmHistory, conn, tran);
        }


        public int DeleteLoadingSlipExtHistoryForItem(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipExtHistoryDAO.DeleteLoadingSlipExtHistoryForItem(loadingSlipExtId, conn, tran);
        }
        #endregion

    }
}
