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
    public class TblLoadingQuotaConsumptionBL : ITblLoadingQuotaConsumptionBL
    {
        private readonly ITblLoadingQuotaConsumptionDAO _iTblLoadingQuotaConsumptionDAO;
        public TblLoadingQuotaConsumptionBL(ITblLoadingQuotaConsumptionDAO iTblLoadingQuotaConsumptionDAO)
        {
            _iTblLoadingQuotaConsumptionDAO = iTblLoadingQuotaConsumptionDAO;
        }
        #region Selection

        public List<TblLoadingQuotaConsumptionTO> SelectAllTblLoadingQuotaConsumptionList()
        {
           return  _iTblLoadingQuotaConsumptionDAO.SelectAllTblLoadingQuotaConsumption();
        }

        public TblLoadingQuotaConsumptionTO SelectTblLoadingQuotaConsumptionTO()
        {
            return  _iTblLoadingQuotaConsumptionDAO.SelectTblLoadingQuotaConsumption();
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO)
        {
            return _iTblLoadingQuotaConsumptionDAO.InsertTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO);
        }

        public int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaConsumptionDAO.InsertTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO)
        {
            return _iTblLoadingQuotaConsumptionDAO.UpdateTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO);
        }

        public int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaConsumptionDAO.UpdateTblLoadingQuotaConsumption(tblLoadingQuotaConsumptionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingQuotaConsumption()
        {
            return _iTblLoadingQuotaConsumptionDAO.DeleteTblLoadingQuotaConsumption();
        }

        public int DeleteTblLoadingQuotaConsumption(SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingQuotaConsumptionDAO.DeleteTblLoadingQuotaConsumption(conn, tran);
        }

        #endregion
        
    }
}
