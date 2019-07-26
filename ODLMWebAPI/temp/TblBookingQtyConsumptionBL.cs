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
    public class TblBookingQtyConsumptionBL : ITblBookingQtyConsumptionBL
    {
        #region Selection

        public List<TblBookingQtyConsumptionTO> SelectAllTblBookingQtyConsumptionList()
        {
            return  TblBookingQtyConsumptionDAO.SelectAllTblBookingQtyConsumption();
        }

        public TblBookingQtyConsumptionTO SelectTblBookingQtyConsumptionTO(Int32 idBookQtyConsuption)
        {
            return  TblBookingQtyConsumptionDAO.SelectTblBookingQtyConsumption(idBookQtyConsuption);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            return TblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumptionTO);
        }

        public int InsertTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingQtyConsumptionDAO.InsertTblBookingQtyConsumption(tblBookingQtyConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO)
        {
            return TblBookingQtyConsumptionDAO.UpdateTblBookingQtyConsumption(tblBookingQtyConsumptionTO);
        }

        public int UpdateTblBookingQtyConsumption(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingQtyConsumptionDAO.UpdateTblBookingQtyConsumption(tblBookingQtyConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption)
        {
            return TblBookingQtyConsumptionDAO.DeleteTblBookingQtyConsumption(idBookQtyConsuption);
        }

        public int DeleteTblBookingQtyConsumption(Int32 idBookQtyConsuption, SqlConnection conn, SqlTransaction tran)
        {
            return TblBookingQtyConsumptionDAO.DeleteTblBookingQtyConsumption(idBookQtyConsuption, conn, tran);
        }

        #endregion
        
    }
}
