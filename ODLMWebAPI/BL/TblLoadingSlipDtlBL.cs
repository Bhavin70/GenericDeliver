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
    public class TblLoadingSlipDtlBL : ITblLoadingSlipDtlBL
    {
        private readonly ITblLoadingSlipDtlDAO _iTblLoadingSlipDtlDAO;
        public TblLoadingSlipDtlBL(ITblLoadingSlipDtlDAO iTblLoadingSlipDtlDAO)
        {
            _iTblLoadingSlipDtlDAO = iTblLoadingSlipDtlDAO;
        }
        #region Selection

        public List<TblLoadingSlipDtlTO> SelectAllTblLoadingSlipDtlList()
        {
            return _iTblLoadingSlipDtlDAO.SelectAllTblLoadingSlipDtl();           
        }

        public TblLoadingSlipDtlTO SelectTblLoadingSlipDtlTO(Int32 idLoadSlipDtl)
        {
            return _iTblLoadingSlipDtlDAO.SelectTblLoadingSlipDtl(idLoadSlipDtl);
        }

        public TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId)
        {
            return _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(loadingSlipId);
        }

        public TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.SelectLoadingSlipDtlTO(loadingSlipId,conn,tran); 
        }

        public List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromLoadingId(Int32 loadingId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromLoadingId(loadingId,conn,tran);
        }

        /// <summary>
        /// Vijaymala added [24-04-2018]:added to get loading slip details from bookingId
        /// </summary>
        /// <param name="bookingId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.SelectAllLoadingSlipDtlListFromBookingId(bookingId, conn, tran);
        }
        #endregion

        #region Insertion
        public int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO)
        {
            return _iTblLoadingSlipDtlDAO.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO);
        }

        public int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.InsertTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO)
        {
            return _iTblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl(tblLoadingSlipDtlTO);
        }

        public int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.UpdateTblLoadingSlipDtl(tblLoadingSlipDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl)
        {
            return _iTblLoadingSlipDtlDAO.DeleteTblLoadingSlipDtl(idLoadSlipDtl);
        }

        public int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.DeleteTblLoadingSlipDtl(idLoadSlipDtl, conn, tran);
        }

        //Priyanka [22-04-2018]
        public int DeleteTblLoadingSlipDtlNew(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipDtlDAO.DeleteTblLoadingSlipDtlNew(loadingSlipId, conn, tran);
        }
        #endregion

    }
}
