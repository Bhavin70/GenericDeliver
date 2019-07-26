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
    public class TblBookingParitiesBL : ITblBookingParitiesBL
    {
        private readonly ITblBookingParitiesDAO _iTblBookingParitiesDAO;
        public TblBookingParitiesBL(ITblBookingParitiesDAO iTblBookingParitiesDAO)
        {
            _iTblBookingParitiesDAO = iTblBookingParitiesDAO;
        }
        #region Selection

        public List<TblBookingParitiesTO> SelectAllTblBookingParitiesList()
        {
           return _iTblBookingParitiesDAO.SelectAllTblBookingParities();
        }

        public TblBookingParitiesTO SelectTblBookingParitiesTO(Int32 idBookingParity)
        {
            return  _iTblBookingParitiesDAO.SelectTblBookingParities(idBookingParity);
        }

        public List<TblBookingParitiesTO> SelectTblBookingParitiesTOListByBookingId (Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingParitiesDAO.SelectTblBookingParitiesByBookingId(bookingId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO)
        {
            return _iTblBookingParitiesDAO.InsertTblBookingParities(tblBookingParitiesTO);
        }

        public int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingParitiesDAO.InsertTblBookingParities(tblBookingParitiesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO)
        {
            return _iTblBookingParitiesDAO.UpdateTblBookingParities(tblBookingParitiesTO);
        }

        public int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingParitiesDAO.UpdateTblBookingParities(tblBookingParitiesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingParities(Int32 idBookingParity)
        {
            return _iTblBookingParitiesDAO.DeleteTblBookingParities(idBookingParity);
        }

        public int DeleteTblBookingParities(Int32 idBookingParity, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingParitiesDAO.DeleteTblBookingParities(idBookingParity, conn, tran);
        }

        #endregion
        
    }
}
