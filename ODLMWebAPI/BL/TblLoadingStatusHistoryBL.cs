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
    public class TblLoadingStatusHistoryBL : ITblLoadingStatusHistoryBL
    {
        private readonly ITblLoadingStatusHistoryDAO _iTblLoadingStatusHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        public TblLoadingStatusHistoryBL(IConnectionString iConnectionString, ITblLoadingStatusHistoryDAO iTblLoadingStatusHistoryDAO)
        {
            _iTblLoadingStatusHistoryDAO = iTblLoadingStatusHistoryDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistoryList(Int32 loadingId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory(loadingId,conn,tran);
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

        public List<TblLoadingStatusHistoryTO> SelectAllTblLoadingStatusHistoryList(Int32 loadingId, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                return _iTblLoadingStatusHistoryDAO.SelectAllTblLoadingStatusHistory(loadingId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TblLoadingStatusHistoryTO SelectTblLoadingStatusHistoryTO(Int32 idLoadingHistory)
        {
            return _iTblLoadingStatusHistoryDAO.SelectTblLoadingStatusHistory(idLoadingHistory);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO)
        {
            return _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO);
        }

        public int InsertTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingStatusHistoryDAO.InsertTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO)
        {
            return _iTblLoadingStatusHistoryDAO.UpdateTblLoadingStatusHistory(tblLoadingStatusHistoryTO);
        }

        public int UpdateTblLoadingStatusHistory(TblLoadingStatusHistoryTO tblLoadingStatusHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingStatusHistoryDAO.UpdateTblLoadingStatusHistory(tblLoadingStatusHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory)
        {
            return _iTblLoadingStatusHistoryDAO.DeleteTblLoadingStatusHistory(idLoadingHistory);
        }

        public int DeleteTblLoadingStatusHistory(Int32 idLoadingHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingStatusHistoryDAO.DeleteTblLoadingStatusHistory(idLoadingHistory, conn, tran);
        }

        #endregion
        
    }
}
