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
    public class TblOtherTaxesBL : ITblOtherTaxesBL
    {
        private readonly ITblOtherTaxesDAO _iTblOtherTaxesDAO;
        public TblOtherTaxesBL(ITblOtherTaxesDAO iTblOtherTaxesDAO)
        {
            _iTblOtherTaxesDAO = iTblOtherTaxesDAO;
        }

        #region Selection

        public List<TblOtherTaxesTO> SelectAllTblOtherTaxesList()
        {
           return  _iTblOtherTaxesDAO.SelectAllTblOtherTaxes();
        }

        public TblOtherTaxesTO SelectTblOtherTaxesTO(Int32 idOtherTax)
        {
            return  _iTblOtherTaxesDAO.SelectTblOtherTaxes(idOtherTax);
        }
        /// <summary>
        /// Vijaymala [17-04-2018]:added to get other tax details 
        /// </summary>
        /// <param name="idOtherTax"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TblOtherTaxesTO SelectTblOtherTaxesTO(Int32 idOtherTax,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblOtherTaxesDAO.SelectTblOtherTaxes(idOtherTax,conn,tran);
        }

        #endregion

        #region Insertion
        public int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO)
        {
            return _iTblOtherTaxesDAO.InsertTblOtherTaxes(tblOtherTaxesTO);
        }

        public int InsertTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherTaxesDAO.InsertTblOtherTaxes(tblOtherTaxesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO)
        {
            return _iTblOtherTaxesDAO.UpdateTblOtherTaxes(tblOtherTaxesTO);
        }

        public int UpdateTblOtherTaxes(TblOtherTaxesTO tblOtherTaxesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherTaxesDAO.UpdateTblOtherTaxes(tblOtherTaxesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblOtherTaxes(Int32 idOtherTax)
        {
            return _iTblOtherTaxesDAO.DeleteTblOtherTaxes(idOtherTax);
        }

        public int DeleteTblOtherTaxes(Int32 idOtherTax, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOtherTaxesDAO.DeleteTblOtherTaxes(idOtherTax, conn, tran);
        }

        #endregion
        
    }
}
