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
    public class TblLocationBL : ITblLocationBL
    {
        private readonly ITblLocationDAO _iTblLocationDAO;
        public TblLocationBL(ITblLocationDAO iTblLocationDAO)
        {
            _iTblLocationDAO = iTblLocationDAO;
        }
        #region Selection

        public List<TblLocationTO> SelectAllTblLocationList()
        {
           return _iTblLocationDAO.SelectAllTblLocation();
        }

        public List<TblLocationTO> SelectAllCompartmentLocationList(Int32 parentLocationId)
        {
            return _iTblLocationDAO.SelectAllTblLocation(parentLocationId);
        }

        public List<TblLocationTO> SelectAllParentLocation()
        {
            return _iTblLocationDAO.SelectAllParentLocation();
        }

        public TblLocationTO SelectTblLocationTO(Int32 idLocation)
        {
            return  _iTblLocationDAO.SelectTblLocation(idLocation);
        }

        /// <summary>
        /// Sanjay [2017-05-03] To Get All the compartment whose stock for the given date is not taken
        /// </summary>
        /// <param name="stockDate"></param>
        /// <returns></returns>
        public List<TblLocationTO> SelectStkNotTakenCompartmentList(DateTime stockDate)
        {
            return _iTblLocationDAO.SelectStkNotTakenCompartmentList(stockDate);

        }


        #endregion

        #region Insertion
        public int InsertTblLocation(TblLocationTO tblLocationTO)
        {
            return _iTblLocationDAO.InsertTblLocation(tblLocationTO);
        }

        public int InsertTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.InsertTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLocation(TblLocationTO tblLocationTO)
        {
            return _iTblLocationDAO.UpdateTblLocation(tblLocationTO);
        }

        public int UpdateTblLocation(TblLocationTO tblLocationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.UpdateTblLocation(tblLocationTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLocation(Int32 idLocation)
        {
            return _iTblLocationDAO.DeleteTblLocation(idLocation);
        }

        public int DeleteTblLocation(Int32 idLocation, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLocationDAO.DeleteTblLocation(idLocation, conn, tran);
        }

        #endregion
        
    }
}
