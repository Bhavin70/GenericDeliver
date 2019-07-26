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
    public class TblQuotaConsumHistoryBL : ITblQuotaConsumHistoryBL
    {
        private readonly ITblQuotaConsumHistoryDAO _iTblQuotaConsumHistoryDAO;
        public TblQuotaConsumHistoryBL(ITblQuotaConsumHistoryDAO iTblQuotaConsumHistoryDAO)
        {
            _iTblQuotaConsumHistoryDAO = iTblQuotaConsumHistoryDAO;
        }
        #region Selection

        public List<TblQuotaConsumHistoryTO> SelectAllTblQuotaConsumHistoryList()
        {
            return _iTblQuotaConsumHistoryDAO.SelectAllTblQuotaConsumHistory();
        }

        public TblQuotaConsumHistoryTO SelectTblQuotaConsumHistoryTO(Int32 idQuotaConsmHIstory)
        {
            return _iTblQuotaConsumHistoryDAO.SelectTblQuotaConsumHistory(idQuotaConsmHIstory);
        }

       

        #endregion
        
        #region Insertion
        public int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO)
        {
            return _iTblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO);
        }

        public int InsertTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaConsumHistoryDAO.InsertTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO)
        {
            return _iTblQuotaConsumHistoryDAO.UpdateTblQuotaConsumHistory(tblQuotaConsumHistoryTO);
        }

        public int UpdateTblQuotaConsumHistory(TblQuotaConsumHistoryTO tblQuotaConsumHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaConsumHistoryDAO.UpdateTblQuotaConsumHistory(tblQuotaConsumHistoryTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory)
        {
            return _iTblQuotaConsumHistoryDAO.DeleteTblQuotaConsumHistory(idQuotaConsmHIstory);
        }

        public int DeleteTblQuotaConsumHistory(Int32 idQuotaConsmHIstory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblQuotaConsumHistoryDAO.DeleteTblQuotaConsumHistory(idQuotaConsmHIstory, conn, tran);
        }

        #endregion
        
    }
}
