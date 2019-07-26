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
    public class TblInvoiceItemDetailsBL : ITblInvoiceItemDetailsBL
    {
        #region Selection
        public List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList()
        {
            return  TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails();
        }

        public TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTO(Int32 idInvoiceItem)
        {
            return  TblInvoiceItemDetailsDAO.SelectTblInvoiceItemDetails(idInvoiceItem);
        }
        /// <summary>
        /// Ramdas.w @14-12-2017 : get other Tax by Invoice 
        /// </summary>
        /// <param name="idInvoice"></param>
        /// <returns></returns>
        public TblInvoiceItemDetailsTO SelectTblInvoiceItemDetailsTOByInvoice(Int32 idInvoice)
        {
            return TblInvoiceItemDetailsDAO.SelectTblInvoiceItemDetailsTOByInvoice(idInvoice);
        }

        public List<TblInvoiceItemDetailsTO> SelectAllTblInvoiceItemDetailsList(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails(invoiceId,conn,tran);
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
            return TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetails(invoiceId, conn, tran);
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
            return TblInvoiceItemDetailsDAO.SelectAllTblInvoiceItemDetailsTOByloadingSlipExtId(loadingSlipExtId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            return TblInvoiceItemDetailsDAO.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO);
        }

        public int InsertTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.InsertTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO)
        {
            return TblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO);
        }

        public int UpdateTblInvoiceItemDetails(TblInvoiceItemDetailsTO tblInvoiceItemDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.UpdateTblInvoiceItemDetails(tblInvoiceItemDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem)
        {
            return TblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetails(idInvoiceItem);
        }

        public int DeleteTblInvoiceItemDetails(Int32 idInvoiceItem, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetails(idInvoiceItem, conn, tran);
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
            return TblInvoiceItemDetailsDAO.DeleteTblInvoiceItemDetailsByInvoiceId(invoiceId, conn, tran);
        }

        #endregion

    }
}
