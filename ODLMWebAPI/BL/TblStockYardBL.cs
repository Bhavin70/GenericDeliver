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
    public class TblStockYardBL : ITblStockYardBL
    {
        private readonly ITblStockYardDAO _iTblStockYardDAO;
        public TblStockYardBL(ITblStockYardDAO iTblStockYardDAO)
        {
            _iTblStockYardDAO = iTblStockYardDAO;
        }
        #region Selection

        public List<TblStockYardTO> SelectAllTblStockYardList()
        {
            return _iTblStockYardDAO.SelectAllTblStockYard();
        }

        public TblStockYardTO SelectTblStockYardTO(Int32 idStockYard)
        {
            return _iTblStockYardDAO.SelectTblStockYard(idStockYard);
        }

       

        #endregion
        
        #region Insertion
        public int InsertTblStockYard(TblStockYardTO tblStockYardTO)
        {
            return _iTblStockYardDAO.InsertTblStockYard(tblStockYardTO);
        }

        public int InsertTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockYardDAO.InsertTblStockYard(tblStockYardTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStockYard(TblStockYardTO tblStockYardTO)
        {
            return _iTblStockYardDAO.UpdateTblStockYard(tblStockYardTO);
        }

        public int UpdateTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockYardDAO.UpdateTblStockYard(tblStockYardTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStockYard(Int32 idStockYard)
        {
            return _iTblStockYardDAO.DeleteTblStockYard(idStockYard);
        }

        public int DeleteTblStockYard(Int32 idStockYard, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockYardDAO.DeleteTblStockYard(idStockYard, conn, tran);
        }

        #endregion
        
    }
}
