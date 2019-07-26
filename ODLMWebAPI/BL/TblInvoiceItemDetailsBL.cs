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
    public class TblInvoiceItemDetailsBL : ITblInvoiceItemDetailsBL
    {
        private readonly ITblInvoiceItemDetailsDAO _iTblInvoiceItemDetailsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblInvoiceItemDetailsBL(IConnectionString iConnectionString, ITblInvoiceItemDetailsDAO iTblInvoiceItemDetailsDAO)
        {
            _iTblInvoiceItemDetailsDAO = iTblInvoiceItemDetailsDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList()
        {
            return  _iTblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails();
        }

        public TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTO(Int32 idInvoiceItem)
        {
            return  _iTblInvoiceItemDetailsDAO.SelectTblInvoiceItemDetails(idInvoiceItem);
        }
        /// <summary>
        /// Ramdas.w @14-12-2017 : get other Tax by Invoice 
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        public TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTOByInvoice(Int32 idInvoice)
        {
            return _iTblInvoiceItemDetailsDAO.SelectTblInvoiceItemDetailsTOByInvoice(idInvoice);
        }

        public List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails(invoiceId,conn,tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails(invoiceId, conn, tran);
        }

        /// <summary>
        /// Saket [2018-05-04] Added
        /// </summary>
        /// <param name="loadingSlipExtId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TblInvoiceItemDetailsTO SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId(Int32 loadingSlipExtId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId(loadingSlipExtId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            return _iTblInvoiceItemDetailsDAO.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO);
        }

        public int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemDetailsDAO.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            return _iTblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO);
        }

        public int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem)
        {
            return _iTblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetails(idInvoiceItem);
        }

        public int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetails(idInvoiceItem, conn, tran);
        }
        /// <summary>
        /// Vijaymala [17-04-2018]:added to delete invoice item by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteTblInvoiceItemDetailsByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetailsByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
