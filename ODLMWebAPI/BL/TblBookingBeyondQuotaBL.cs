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
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
 
namespace ODLMWebAPI.BL
{ 
    public class TblBookingBeyondQuotaBL : ITblBookingBeyondQuotaBL
    {
        private readonly ITblBookingBeyondQuotaDAO _iTblBookingBeyondQuotaDAO;
        public TblBookingBeyondQuotaBL(ITblBookingBeyondQuotaDAO iTblBookingBeyondQuotaDAO)
        {
            _iTblBookingBeyondQuotaDAO = iTblBookingBeyondQuotaDAO;
        }
        #region Selection

        public List<TblBookingBeyondQuotaTO> SelectAllTblBookingBeyondQuotaList()
        {
            return _iTblBookingBeyondQuotaDAO.SelectAllTblBookingBeyondQuota();
            
        }

        public TblBookingBeyondQuotaTO SelectTblBookingBeyondQuotaTO(Int32 idBookingAuth)
        {
           return _iTblBookingBeyondQuotaDAO.SelectTblBookingBeyondQuota(idBookingAuth);
        }

        public List<TblBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
        {
            return _iTblBookingBeyondQuotaDAO.SelectAllStatusHistoryOfBooking(bookingId);

        }

        #endregion

        #region Insertion
        public int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return _iTblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return _iTblBookingBeyondQuotaDAO.UpdateTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingBeyondQuotaDAO.UpdateTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingBeyondQuota(Int32 idBookingAuth)
        {
            return _iTblBookingBeyondQuotaDAO.DeleteTblBookingBeyondQuota(idBookingAuth);
        }

        public int DeleteTblBookingBeyondQuota(Int32 idBookingAuth, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingBeyondQuotaDAO.DeleteTblBookingBeyondQuota(idBookingAuth, conn, tran);
        }

        #endregion
        
    }
}
