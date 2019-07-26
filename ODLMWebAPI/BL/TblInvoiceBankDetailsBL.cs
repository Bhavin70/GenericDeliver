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
    public class TblInvoiceBankDetailsBL : ITblInvoiceBankDetailsBL
    {
        private readonly ITblInvoiceBankDetailsDAO _iTblInvoiceBankDetailsDAO;
        public TblInvoiceBankDetailsBL(ITblInvoiceBankDetailsDAO iTblInvoiceBankDetailsDAO)
        {
            _iTblInvoiceBankDetailsDAO = iTblInvoiceBankDetailsDAO;
        }
        #region Selection
        public List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails()
        {
            return _iTblInvoiceBankDetailsDAO.SelectAllTblInvoiceBankDetails();
        }

        public TblInvoiceBankDetailsTO SelectTblInvoiceBankDetailsTO(Int32 idBank)
        {
            return _iTblInvoiceBankDetailsDAO.SelectTblInvoiceBankDetails(idBank);
        }

        public List<TblInvoiceBankDetailsTO> SelectInvoiceBankDetails(Int32 organizationId)
        {
            return _iTblInvoiceBankDetailsDAO.SelectInvoiceBankDetails(organizationId);
        }



        #endregion

        #region Insertion
        public int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            return _iTblInvoiceBankDetailsDAO.InsertTblInvoiceBankDetails(tblInvoiceBankDetailsTO);
        }

        public int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceBankDetailsDAO.InsertTblInvoiceBankDetails(tblInvoiceBankDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            return _iTblInvoiceBankDetailsDAO.UpdateTblInvoiceBankDetails(tblInvoiceBankDetailsTO);
        }

        public int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceBankDetailsDAO.UpdateTblInvoiceBankDetails(tblInvoiceBankDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceBankDetails(Int32 idBank)
        {
            return _iTblInvoiceBankDetailsDAO.DeleteTblInvoiceBankDetails(idBank);
        }

        public int DeleteTblInvoiceBankDetails(Int32 idBank, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceBankDetailsDAO.DeleteTblInvoiceBankDetails(idBank, conn, tran);
        }

        #endregion
        
    }
}
