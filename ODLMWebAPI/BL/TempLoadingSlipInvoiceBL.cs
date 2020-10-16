using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{   
    public class TempLoadingSlipInvoiceBL : ITempLoadingSlipInvoiceBL
    {
        private readonly ITempLoadingSlipInvoiceDAO _iTempLoadingSlipInvoiceDAO;
        public TempLoadingSlipInvoiceBL(ITempLoadingSlipInvoiceDAO iTempLoadingSlipInvoiceDAO)
        {
            _iTempLoadingSlipInvoiceDAO = iTempLoadingSlipInvoiceDAO;
        }
        #region Selection
        public List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoice()
        {
            return _iTempLoadingSlipInvoiceDAO.SelectAllTempLoadingSlipInvoice();
        }

        public TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTO(Int32 idLoadingSlipInvoice)
        {
            return _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoice(idLoadingSlipInvoice);
        }

        public List<TempLoadingSlipInvoiceTO> SelectAllTempLoadingSlipInvoiceList(string loadingSlipIds)
        {
            return _iTempLoadingSlipInvoiceDAO.SelectAllTempLoadingSlipInvoiceList(loadingSlipIds);
        }
        /// <summary>
        /// Vijaymala[17-04-2018] :Added To get loading slip and invoice mapping list by using invoice id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOByInvoiceId(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId,conn,tran);
        }

        public List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOByInvoiceId(Int32 invoiceId)
        {
            return _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOByInvoiceId(invoiceId);
        }

        /// <summary>
        /// Vaibhav [24-April-2018] 
        /// </summary>
        /// <param name="loadingSlipId"></param>
        /// <param name="invoiceId"></param>
        /// <param name="conn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TempLoadingSlipInvoiceTO SelectTempLoadingSlipInvoiceTOList(int loadingSlipId,int invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOList(loadingSlipId, invoiceId, conn, tran);
        }

        public List<TempLoadingSlipInvoiceTO> SelectTempLoadingSlipInvoiceTOListByLoadingSlip(int loadingSlipId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.SelectTempLoadingSlipInvoiceTOListByLoadingSlip(loadingSlipId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO)
        {
            return _iTempLoadingSlipInvoiceDAO.InsertTempLoadingSlipInvoice(tempLoadingSlipInvoiceTO);
        }

        public int InsertTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.InsertTempLoadingSlipInvoice(tempLoadingSlipInvoiceTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO)
        {
            return _iTempLoadingSlipInvoiceDAO.UpdateTempLoadingSlipInvoice(tempLoadingSlipInvoiceTO);
        }

        public int UpdateTempLoadingSlipInvoice(TempLoadingSlipInvoiceTO tempLoadingSlipInvoiceTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.UpdateTempLoadingSlipInvoice(tempLoadingSlipInvoiceTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice)
        {
            return _iTempLoadingSlipInvoiceDAO.DeleteTempLoadingSlipInvoice(idLoadingSlipInvoice);
        }

        public int DeleteTempLoadingSlipInvoice(Int32 idLoadingSlipInvoice, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.DeleteTempLoadingSlipInvoice(idLoadingSlipInvoice, conn, tran);
        }


        public int DeleteTempLoadingSlipInvoiceByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTempLoadingSlipInvoiceDAO.DeleteTempLoadingSlipInvoiceByInvoiceId(invoiceId, conn, tran);
        }
        #endregion

    }
}
