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
    public class TblInvoiceAddressBL : ITblInvoiceAddressBL
    {
        private readonly ITblInvoiceAddressDAO _iTblInvoiceAddressDAO;
        private readonly IConnectionString _iConnectionString;
        public TblInvoiceAddressBL(ITblInvoiceAddressDAO iTblInvoiceAddressDAO, IConnectionString iConnectionString)
        {
            _iTblInvoiceAddressDAO = iTblInvoiceAddressDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList()
        {
            return  _iTblInvoiceAddressDAO.SelectAllTblInvoiceAddress();
        }

        public TblInvoiceAddressTO SelectTblInvoiceAddressTO(Int32 idInvoiceAddr)
        {
            return  _iTblInvoiceAddressDAO.SelectTblInvoiceAddress(idInvoiceAddr);
        }

        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceAddressDAO.SelectAllTblInvoiceAddress(invoiceId,conn,tran);
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

        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblInvoiceAddressDAO.SelectAllTblInvoiceAddress(invoiceId, conn, tran);
        }
        #endregion

        #region Insertion
        public int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            return _iTblInvoiceAddressDAO.InsertTblInvoiceAddress(tblInvoiceAddressTO);
        }

        public int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceAddressDAO.InsertTblInvoiceAddress(tblInvoiceAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            return _iTblInvoiceAddressDAO.UpdateTblInvoiceAddress(tblInvoiceAddressTO);
        }

        public int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceAddressDAO.UpdateTblInvoiceAddress(tblInvoiceAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceAddress(Int32 idInvoiceAddr)
        {
            return _iTblInvoiceAddressDAO.DeleteTblInvoiceAddress(idInvoiceAddr);
        }

        public int DeleteTblInvoiceAddress(Int32 idInvoiceAddr, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceAddressDAO.DeleteTblInvoiceAddress(idInvoiceAddr, conn, tran);
        }
        public int DeleteTblInvoiceAddressByinvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceAddressDAO.DeleteTblInvoiceAddressByinvoiceId(invoiceId, conn, tran);
        }
        #endregion

    }
}
