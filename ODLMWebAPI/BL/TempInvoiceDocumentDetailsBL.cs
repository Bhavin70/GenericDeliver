using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TempInvoiceDocumentDetailsBL : ITempInvoiceDocumentDetailsBL
    {
        private readonly ITempInvoiceDocumentDetailsDAO _iTempInvoiceDocumentDetailsDAO;
        public TempInvoiceDocumentDetailsBL(ITempInvoiceDocumentDetailsDAO iTempInvoiceDocumentDetailsDAO)
        {
            _iTempInvoiceDocumentDetailsDAO = iTempInvoiceDocumentDetailsDAO;
        }
        #region Selection
        public List<TempInvoiceDocumentDetailsTO> SelectAllTempInvoiceDocumentDetails()
        {
            return _iTempInvoiceDocumentDetailsDAO.SelectAllTempInvoiceDocumentDetails();
        }

        public TempInvoiceDocumentDetailsTO SelectTempInvoiceDocumentDetailsTO(Int32 idInvoiceDocument)
        {
            return _iTempInvoiceDocumentDetailsDAO.SelectTempInvoiceDocumentDetails(idInvoiceDocument);
        }
        /// <summary>
        /// Vijaymala[25-05-2018] :Added To get invoice document list by using invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        public List<TempInvoiceDocumentDetailsTO> SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(Int32 invoiceId)
        {
            return _iTempInvoiceDocumentDetailsDAO.SelectALLTempInvoiceDocumentDetailsTOListByInvoiceId(invoiceId);
        }

        /// <summary>
        /// Vijaymala[28-05-2018] :Added To get invoice document list by using invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TempInvoiceDocumentDetailsTO> SelectTempInvoiceDocumentDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempInvoiceDocumentDetailsDAO.SelectTempInvoiceDocumentDetailsByInvoiceId(invoiceId, conn, tran);
        }
        #endregion

        #region Insertion
        public int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO)
        {
            return _iTempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
        }

        public int InsertTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempInvoiceDocumentDetailsDAO.InsertTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO)
        {
            return _iTempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO);
        }

        public int UpdateTempInvoiceDocumentDetails(TempInvoiceDocumentDetailsTO tempInvoiceDocumentDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempInvoiceDocumentDetailsDAO.UpdateTempInvoiceDocumentDetails(tempInvoiceDocumentDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument)
        {
            return _iTempInvoiceDocumentDetailsDAO.DeleteTempInvoiceDocumentDetails(idInvoiceDocument);
        }

        public int DeleteTempInvoiceDocumentDetails(Int32 idInvoiceDocument, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempInvoiceDocumentDetailsDAO.DeleteTempInvoiceDocumentDetails(idInvoiceDocument, conn, tran);
        }

        /// <summary>
        /// Vijaymala [17-04-2018]:added to delete invoice document  by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteTblInvoiceDocumentByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempInvoiceDocumentDetailsDAO.DeleteTblInvoiceDocumentByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
