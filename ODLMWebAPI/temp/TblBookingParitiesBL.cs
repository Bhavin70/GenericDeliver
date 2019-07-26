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
    public class TblBookingParitiesBL : ITblBookingParitiesBL
    {
        #region Selection

        public List<TblBookingParitiesTO> SelectAllTblBookingParitiesList()
        {
           return TblBookingParitiesDAO.SelectAllTblBookingParities();
           
        }

        public TblBookingParitiesTO SelectTblBookingParitiesTO(Int32 idBookingParity)
        {
            return  TblBookingParitiesDAO.SelectTblBookingParities(idBookingParity);
        }

        public List<TblBookingParitiesTO> SelectTblBookingParitiesTOListByBookingId (Int32 bookingId, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingParitiesDAO.SelectTblBookingParitiesByBookingId(bookingId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO)
        {
            return TblBookingParitiesDAO.InsertTblBookingParities(tblBookingParitiesTO);
        }

        public int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingParitiesDAO.InsertTblBookingParities(tblBookingParitiesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO)
        {
            return TblBookingParitiesDAO.UpdateTblBookingParities(tblBookingParitiesTO);
        }

        public int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingParitiesDAO.UpdateTblBookingParities(tblBookingParitiesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingParities(Int32 idBookingParity)
        {
            return TblBookingParitiesDAO.DeleteTblBookingParities(idBookingParity);
        }

        public int DeleteTblBookingParities(Int32 idBookingParity, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingParitiesDAO.DeleteTblBookingParities(idBookingParity, conn, tran);
        }

        #endregion
        
    }
}
