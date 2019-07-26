using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
namespace ODLMWebAPI.BL
{
    public class TblConfigParamHistoryBL : ITblConfigParamHistoryBL
    {
        #region Selection

        public List<TblConfigParamHistoryTO> SelectAllTblConfigParamHistoryList()
        {
            return TblConfigParamHistoryDAO.SelectAllTblConfigParamHistory();
        }

        public TblConfigParamHistoryTO SelectTblConfigParamHistoryTO(Int32 idParamHistory)
        {
            return  TblConfigParamHistoryDAO.SelectTblConfigParamHistory(idParamHistory);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return TblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public int InsertTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamHistoryDAO.InsertTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO)
        {
            return TblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO);
        }

        public int UpdateTblConfigParamHistory(TblConfigParamHistoryTO tblConfigParamHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamHistoryDAO.UpdateTblConfigParamHistory(tblConfigParamHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblConfigParamHistory(Int32 idParamHistory)
        {
            return TblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory);
        }

        public int DeleteTblConfigParamHistory(Int32 idParamHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblConfigParamHistoryDAO.DeleteTblConfigParamHistory(idParamHistory, conn, tran);
        }

        #endregion
        
    }
}
