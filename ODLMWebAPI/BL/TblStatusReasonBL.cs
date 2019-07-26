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
    public class TblStatusReasonBL : ITblStatusReasonBL
    {
        private readonly ITblStatusReasonDAO _iTblStatusReasonDAO;
        public TblStatusReasonBL(ITblStatusReasonDAO iTblStatusReasonDAO)
        {
            _iTblStatusReasonDAO = iTblStatusReasonDAO;
        }
        #region Selection      

        public List<TblStatusReasonTO> SelectAllTblStatusReasonList()
        {
            return _iTblStatusReasonDAO.SelectAllTblStatusReason();
        }

        public TblStatusReasonTO SelectTblStatusReasonTO(Int32 idStatusReason)
        {
            return  _iTblStatusReasonDAO.SelectTblStatusReason(idStatusReason);
        }

        /// <summary>
        /// Sanjay [2017-03-06] To Get All the list of reason for given status
        /// if statusid=0 then returns all reasons
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public List<TblStatusReasonTO> SelectAllTblStatusReasonList(Int32 statusId)
        {
            return _iTblStatusReasonDAO.SelectAllTblStatusReason(statusId);
        }

        #endregion

        #region Insertion
        public int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO)
        {
            return _iTblStatusReasonDAO.InsertTblStatusReason(tblStatusReasonTO);
        }

        public int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStatusReasonDAO.InsertTblStatusReason(tblStatusReasonTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO)
        {
            return _iTblStatusReasonDAO.UpdateTblStatusReason(tblStatusReasonTO);
        }

        public int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStatusReasonDAO.UpdateTblStatusReason(tblStatusReasonTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStatusReason(Int32 idStatusReason)
        {
            return _iTblStatusReasonDAO.DeleteTblStatusReason(idStatusReason);
        }

        public int DeleteTblStatusReason(Int32 idStatusReason, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStatusReasonDAO.DeleteTblStatusReason(idStatusReason, conn, tran);
        }

        #endregion
        
    }
}
