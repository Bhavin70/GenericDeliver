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
namespace ODLMWebAPI.BL
{
    public class TblBookingBeyondQuotaBL : ITblBookingBeyondQuotaBL
    {
        #region Selection
       
        public List<TblBookingBeyondQuotaTO> SelectAllTblBookingBeyondQuotaList()
        {
            return TblBookingBeyondQuotaDAO.SelectAllTblBookingBeyondQuota();
            
        }

        public TblBookingBeyondQuotaTO SelectTblBookingBeyondQuotaTO(Int32 idBookingAuth)
        {
           return TblBookingBeyondQuotaDAO.SelectTblBookingBeyondQuota(idBookingAuth);
        }

        public List<TblBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId)
        {
            return TblBookingBeyondQuotaDAO.SelectAllStatusHistoryOfBooking(bookingId);

        }

        #endregion

        #region Insertion
        public int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return TblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingBeyondQuotaDAO.InsertTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO)
        {
            return TblBookingBeyondQuotaDAO.UpdateTblBookingBeyondQuota(tblBookingBeyondQuotaTO);
        }

        public int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingBeyondQuotaDAO.UpdateTblBookingBeyondQuota(tblBookingBeyondQuotaTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingBeyondQuota(Int32 idBookingAuth)
        {
            return TblBookingBeyondQuotaDAO.DeleteTblBookingBeyondQuota(idBookingAuth);
        }

        public int DeleteTblBookingBeyondQuota(Int32 idBookingAuth, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingBeyondQuotaDAO.DeleteTblBookingBeyondQuota(idBookingAuth, conn, tran);
        }

        #endregion
        
    }
}
