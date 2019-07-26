using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblSessionHistoryBL : ITblSessionHistoryBL
    {
        private readonly ITblSessionHistoryDAO _iTblSessionHistoryDAO;
        public TblSessionHistoryBL(ITblSessionHistoryDAO iTblSessionHistoryDAO)
        {
            _iTblSessionHistoryDAO = iTblSessionHistoryDAO;
        }
        #region Selection
        public TblSessionHistoryTO SelectAllTblSessionHistory()
        {
            return _iTblSessionHistoryDAO.SelectAllTblSessionHistory();
        }

        public List<TblSessionHistoryTO> SelectAllTblSessionHistoryList()
        {
            return _iTblSessionHistoryDAO.SelectAllTblSessionHistoryData();
        }

        public List<TblSessionHistoryTO> SelectTblSessionHistoryTO(Int32 idSessionHistory)
        {
           return _iTblSessionHistoryDAO.SelectTblSessionHistory(idSessionHistory);
        }

        #endregion
        
        #region Insertion
        public int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO)
        {
            return _iTblSessionHistoryDAO.InsertTblSessionHistory(tblSessionHistoryTO);
        }

        public int InsertTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSessionHistoryDAO.InsertTblSessionHistory(tblSessionHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO)
        {
            return _iTblSessionHistoryDAO.UpdateTblSessionHistory(tblSessionHistoryTO);
        }

        public int UpdateTblSessionHistory(TblSessionHistoryTO tblSessionHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSessionHistoryDAO.UpdateTblSessionHistory(tblSessionHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblSessionHistory(Int32 idSessionHistory)
        {
            return _iTblSessionHistoryDAO.DeleteTblSessionHistory(idSessionHistory);
        }
        public int DeleteTblSessionHistory()
        {
            return _iTblSessionHistoryDAO.DeleteTblSessionHistory();
        }
        public int DeleteTblSessionHistory(Int32 idSessionHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSessionHistoryDAO.DeleteTblSessionHistory(idSessionHistory, conn, tran);
        }

        #endregion
        
    }
}
