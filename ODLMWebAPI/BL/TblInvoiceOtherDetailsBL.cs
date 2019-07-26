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
    public class TblInvoiceOtherDetailsBL : ITblInvoiceOtherDetailsBL
    {
        private readonly ITblInvoiceOtherDetailsDAO _iTblInvoiceOtherDetailsDAO;
        public TblInvoiceOtherDetailsBL(ITblInvoiceOtherDetailsDAO iTblInvoiceOtherDetailsDAO)
        {
            _iTblInvoiceOtherDetailsDAO = iTblInvoiceOtherDetailsDAO;
        }
        #region Selection
        public List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails()
        {
            return _iTblInvoiceOtherDetailsDAO.SelectAllTblInvoiceOtherDetails();
        }


        public TblInvoiceOtherDetailsTO SelectTblInvoiceOtherDetailsTO(Int32 idInvoiceOtherDetails)
        {
            return _iTblInvoiceOtherDetailsDAO.SelectTblInvoiceOtherDetails(idInvoiceOtherDetails);
        }
        public List<TblInvoiceOtherDetailsTO> SelectInvoiceOtherDetails(Int32 organizationId)
        {
            return _iTblInvoiceOtherDetailsDAO.SelectInvoiceOtherDetails(organizationId);
        }


        #endregion

        #region Insertion
        public int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            return _iTblInvoiceOtherDetailsDAO.InsertTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO);
        }

        public int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceOtherDetailsDAO.InsertTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            return _iTblInvoiceOtherDetailsDAO.UpdateTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO);
        }

        public int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceOtherDetailsDAO.UpdateTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails)
        {
            return _iTblInvoiceOtherDetailsDAO.DeleteTblInvoiceOtherDetails(idInvoiceOtherDetails);
        }

        public int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceOtherDetailsDAO.DeleteTblInvoiceOtherDetails(idInvoiceOtherDetails, conn, tran);
        }

        #endregion
        
    }
}
