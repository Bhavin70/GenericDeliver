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
namespace ODLMWebAPI.BL
{
    public class TblInvoiceOtherDetailsBL : ITblInvoiceOtherDetailsBL
    {
        #region Selection
        public List<TblInvoiceOtherDetailsTO> SelectAllTblInvoiceOtherDetails()
        {
            return TblInvoiceOtherDetailsDAO.SelectAllTblInvoiceOtherDetails();
        }


        public TblInvoiceOtherDetailsTO SelectTblInvoiceOtherDetailsTO(Int32 idInvoiceOtherDetails)
        {
            return TblInvoiceOtherDetailsDAO.SelectTblInvoiceOtherDetails(idInvoiceOtherDetails);
        }
        public List<TblInvoiceOtherDetailsTO> SelectInvoiceOtherDetails(Int32 organizationId)
        {
            return TblInvoiceOtherDetailsDAO.SelectInvoiceOtherDetails(organizationId);
        }


        #endregion

        #region Insertion
        public int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            return TblInvoiceOtherDetailsDAO.InsertTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO);
        }

        public int InsertTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceOtherDetailsDAO.InsertTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO)
        {
            return TblInvoiceOtherDetailsDAO.UpdateTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO);
        }

        public int UpdateTblInvoiceOtherDetails(TblInvoiceOtherDetailsTO tblInvoiceOtherDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceOtherDetailsDAO.UpdateTblInvoiceOtherDetails(tblInvoiceOtherDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails)
        {
            return TblInvoiceOtherDetailsDAO.DeleteTblInvoiceOtherDetails(idInvoiceOtherDetails);
        }

        public int DeleteTblInvoiceOtherDetails(Int32 idInvoiceOtherDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceOtherDetailsDAO.DeleteTblInvoiceOtherDetails(idInvoiceOtherDetails, conn, tran);
        }

        #endregion
        
    }
}
