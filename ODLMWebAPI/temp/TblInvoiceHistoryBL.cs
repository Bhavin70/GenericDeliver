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
namespace ODLMWebAPI.BL
{
    public class TblInvoiceHistoryBL : ITblInvoiceHistoryBL
    {
        #region Selection
       
        public List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryList()
        {
            return  TblInvoiceHistoryDAO.SelectAllTblInvoiceHistory();
        }


        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTO(Int32 idInvHistory)
        {
            return  TblInvoiceHistoryDAO.SelectTblInvoiceHistory(idInvHistory);
        }

        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTORateByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.SelectTblInvoiceHistoryTORateByInvoiceItemId(invoiceItemId,conn,tran);
        }

        public List<TblInvoiceHistoryTO> SelectAllTblInvoiceHistoryById(Int32 byId, Boolean isByInvoiceId = false)
        {
            return TblInvoiceHistoryDAO.SelectAllTblInvoiceHistoryById(byId, isByInvoiceId);
        }

        // Vaibhav [08-Jan-2018] added to select invoice history data
        public List<TblInvoiceHistoryTO> SelectTempInvoiceHistory(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.SelectTempInvoiceHistory(invoiceId, conn, tran);
        }

        //Vijaymala [24/01/2018] : Added To Get Invoice History When Cd Change
        public TblInvoiceHistoryTO SelectTblInvoiceHistoryTOCdByInvoiceItemId(Int32 invoiceItemId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.SelectTblInvoiceHistoryTOCdByInvoiceItemId(invoiceItemId, conn, tran);
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
            return TblInvoiceHistoryDAO.SelectTblInvoiceHistoryTOByInvoiceItemId(invoiceItemId, conn, tran);

        }
        #endregion

        #region Insertion
        public int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            return TblInvoiceHistoryDAO.InsertTblInvoiceHistory(tblInvoiceHistoryTO);
        }

        public int InsertTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.InsertTblInvoiceHistory(tblInvoiceHistoryTO, conn, tran);
        }

        public int InsertTblInvoiceHistoryForFinal(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.InsertTblInvoiceHistoryForFinal(tblInvoiceHistoryTO, conn, tran);
        }
        #endregion

        #region Updation
        public int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO)
        {
            return TblInvoiceHistoryDAO.UpdateTblInvoiceHistory(tblInvoiceHistoryTO);
        }

        public int UpdateTblInvoiceHistory(TblInvoiceHistoryTO tblInvoiceHistoryTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.UpdateTblInvoiceHistory(tblInvoiceHistoryTO, conn, tran);
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
            return TblInvoiceHistoryDAO.UpdateTblInvoiceHistoryById(tblInvoiceHistoryTO, conn, tran);
        }


        #endregion

        #region Deletion
        public int DeleteTblInvoiceHistory(Int32 idInvHistory)
        {
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistory(idInvHistory);
        }

        public int DeleteTblInvoiceHistory(Int32 idInvHistory, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistory(idInvHistory, conn, tran);
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
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistoryByInvoiceId(invoiceId, conn, tran);
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
            return TblInvoiceHistoryDAO.DeleteTblInvoiceHistoryByItemId(invoiceItemId, conn, tran);
        }
        #endregion

    }
}
