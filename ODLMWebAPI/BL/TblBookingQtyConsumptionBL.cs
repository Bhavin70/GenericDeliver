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
    public class TblBookingQtyConsumptionBL : ITblBookingQtyConsumptionBL
    {
        #region Selection
        private readonly ITblBookingQtyConsumptionDAO _iTblBookingQtyConsumptionDAO;
        public TblBookingQtyConsumptionBL(ITblBookingQtyConsumptionDAO iTblBookingQtyConsumptionDAO)
        {
            _iTblBookingQtyConsumptionDAO = iTblBookingQtyConsumptionDAO;
        }
        public List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumptionList()
        {
            return  _iTblBookingQtyConsumptionDAO.SelectAllTblBookingQtyConsumption();
        }

        public TblBookingQtyConsumptionTO SelectTblBookingQtyConsumptionTO(Int32 idBookQtyConsuption)
        {
            return  _iTblBookingQtyConsumptionDAO.SelectTblBookingQtyConsumption(idBookQtyConsuption);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            return _iTblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumptionTO);
        }

        public int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            return _iTblBookingQtyConsumptionDAO.UpdateTblBookingQtyConsumption(tblBookingQtyConsumptionTO);
        }

        public int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingQtyConsumptionDAO.UpdateTblBookingQtyConsumption(tblBookingQtyConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption)
        {
            return _iTblBookingQtyConsumptionDAO.DeleteTblBookingQtyConsumption(idBookQtyConsuption);
        }

        public int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblBookingQtyConsumptionDAO.DeleteTblBookingQtyConsumption(idBookQtyConsuption, conn, tran);
        }

        #endregion
        
    }
}
