using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{  
    public class TblStockConsumptionBL : ITblStockConsumptionBL
    {
        private readonly ITblStockConsumptionDAO _iTblStockConsumptionDAO;
        public TblStockConsumptionBL(ITblStockConsumptionDAO iTblStockConsumptionDAO)
        {
            _iTblStockConsumptionDAO = iTblStockConsumptionDAO;
        }
        #region Selection

        public List<TblStockConsumptionTO> SelectAllTblStockConsumptionList()
        {
            return  _iTblStockConsumptionDAO.SelectAllTblStockConsumption();
        }

        public TblStockConsumptionTO SelectTblStockConsumptionTO(Int32 idStockConsumption)
        {
            return  _iTblStockConsumptionDAO.SelectTblStockConsumption(idStockConsumption);
        }

        public List<TblStockConsumptionTO> SelectAllStockConsumptionList(Int32 loadingSlipExtId,Int32 txnOpTypeId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblStockConsumptionDAO.SelectAllStockConsumptionList(loadingSlipExtId,txnOpTypeId,conn,tran);
        }

        #endregion

        #region Insertion
        public int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO)
        {
            return _iTblStockConsumptionDAO.InsertTblStockConsumption(tblStockConsumptionTO);
        }

        public int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConsumptionDAO.InsertTblStockConsumption(tblStockConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO)
        {
            return _iTblStockConsumptionDAO.UpdateTblStockConsumption(tblStockConsumptionTO);
        }

        public int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConsumptionDAO.UpdateTblStockConsumption(tblStockConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStockConsumption(Int32 idStockConsumption)
        {
            return _iTblStockConsumptionDAO.DeleteTblStockConsumption(idStockConsumption);
        }

        public int DeleteTblStockConsumption(Int32 idStockConsumption, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockConsumptionDAO.DeleteTblStockConsumption(idStockConsumption, conn, tran);
        }

        #endregion
        
    }
}
