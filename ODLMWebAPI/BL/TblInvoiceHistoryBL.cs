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
    public class TblInvoiceHistoryBL : ITblInvoiceHistoryBL
    {
        private readonly ITblInvoiceHistoryDAO _iTblInvoiceHistoryDAO;
        public TblInvoiceHistoryBL(ITblInvoiceHistoryDAO iTblInvoiceHistoryDAO)
        {
            _iTblInvoiceHistoryDAO = iTblInvoiceHistoryDAO;
        }
        #region Selection

        public List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryList()
        {
            return  _iTblInvoiceHistoryDAO.SelectAllTblInvoiceHistory();
        }
    

        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTO(Int32 idInvHistory)
        {
            return  _iTblInvoiceHistoryDAO.SelectTblInvoiceHistory(idInvHistory);
        }

        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTORateByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.SelectTblInvoiceHistoryTORateByInvoiceItemId(invoiceItemId,conn,tran);
        }

        public List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryById(Int32 byId, Boolean isByInvoiceId = false)
        {
            return _iTblInvoiceHistoryDAO.SelectAllTblInvoiceHistoryById(byId, isByInvoiceId);
        }

        // Vaibhav [08-Jan-2018] added to select invoice history data
        public List<TblInvoiceHistoryTO> SelectTempInvoiceHistory(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.SelectTempInvoiceHistory(invoiceId, conn, tran);
        }

        //Vijaymala [24/01/2018] : Added To Get Invoice History When Cd Change
        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTOCdByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.SelectTblInvoiceHistoryTOCdByInvoiceItemId(invoiceItemId, conn, tran);
        }
        /// <summary>
        /// Vijaymala [17-04-2018]:added to get invoice history by invoice id
        /// </summary>
        /// <param name="invoiceItemId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTOByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.SelectTblInvoiceHistoryTOByInvoiceItemId(invoiceItemId, conn, tran);

        }
        #endregion

        #region Insertion
        public int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            return _iTblInvoiceHistoryDAO.InsertTblInvoiceHistory(tblInvoiceHistoryTO);
        }

        public int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.InsertTblInvoiceHistory(tblInvoiceHistoryTO, conn, tran);
        }

        public int InsertTblInvoiceHistoryForFinal(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.InsertTblInvoiceHistoryForFinal(tblInvoiceHistoryTO, conn, tran);
        }
        #endregion

        #region Updation
        public int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            return _iTblInvoiceHistoryDAO.UpdateTblInvoiceHistory(tblInvoiceHistoryTO);
        }

        public int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.UpdateTblInvoiceHistory(tblInvoiceHistoryTO, conn, tran);
        }
        /// <summary>
        ///  Vijaymala [17-04-2018]:added to update invoice history by invoice id
        /// </summary>
        /// <param name="tblInvoiceHistoryTO"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int UpdateTblInvoiceHistoryById(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.UpdateTblInvoiceHistoryById(tblInvoiceHistoryTO, conn, tran);
        }


        #endregion

        #region Deletion
        public int DeleteTblInvoiceHistory(Int32 idInvHistory)
        {
            return _iTblInvoiceHistoryDAO.DeleteTblInvoiceHistory(idInvHistory);
        }

        public int DeleteTblInvoiceHistory(Int32 idInvHistory, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.DeleteTblInvoiceHistory(idInvHistory, conn, tran);
        }

        /// <summary>
        /// Vijaymala [17-04-2018]:added to delete invoice history by invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteTblInvoiceHistoryByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.DeleteTblInvoiceHistoryByInvoiceId(invoiceId, conn, tran);
        }

        /// <summary>
        /// Vijaymala [25-07-2018]:added to delete invoice history by invoice id
        /// </summary>
        /// <param name="invoiceItemId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public int DeleteTblInvoiceHistoryByItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceHistoryDAO.DeleteTblInvoiceHistoryByItemId(invoiceItemId, conn, tran);
        }
        #endregion

    }
}
