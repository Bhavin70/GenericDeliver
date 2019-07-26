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
    public class TblInvoiceBankDetailsBL : ITblInvoiceBankDetailsBL
    {
        #region Selection
        public List<TblInvoiceBankDetailsTO> SelectAllTblInvoiceBankDetails()
        {
            return TblInvoiceBankDetailsDAO.SelectAllTblInvoiceBankDetails();
        }

        public TblInvoiceBankDetailsTO SelectTblInvoiceBankDetailsTO(Int32 idBank)
        {
            return TblInvoiceBankDetailsDAO.SelectTblInvoiceBankDetails(idBank);
        }

        public List<TblInvoiceBankDetailsTO> SelectInvoiceBankDetails(Int32 organizationId)
        {
            return TblInvoiceBankDetailsDAO.SelectInvoiceBankDetails(organizationId);
        }



        #endregion

        #region Insertion
        public int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            return TblInvoiceBankDetailsDAO.InsertTblInvoiceBankDetails(tblInvoiceBankDetailsTO);
        }

        public int InsertTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceBankDetailsDAO.InsertTblInvoiceBankDetails(tblInvoiceBankDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO)
        {
            return TblInvoiceBankDetailsDAO.UpdateTblInvoiceBankDetails(tblInvoiceBankDetailsTO);
        }

        public int UpdateTblInvoiceBankDetails(TblInvoiceBankDetailsTO tblInvoiceBankDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceBankDetailsDAO.UpdateTblInvoiceBankDetails(tblInvoiceBankDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceBankDetails(Int32 idBank)
        {
            return TblInvoiceBankDetailsDAO.DeleteTblInvoiceBankDetails(idBank);
        }

        public int DeleteTblInvoiceBankDetails(Int32 idBank, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceBankDetailsDAO.DeleteTblInvoiceBankDetails(idBank, conn, tran);
        }

        #endregion
        
    }
}
