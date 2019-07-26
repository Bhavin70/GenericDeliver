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
    public class TblInvoiceItemTaxDtlsBL : ITblInvoiceItemTaxDtlsBL
    {
        private readonly ITblInvoiceItemTaxDtlsDAO _iTblInvoiceItemTaxDtlsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblInvoiceItemTaxDtlsBL(IConnectionString iConnectionString, ITblInvoiceItemTaxDtlsDAO iTblInvoiceItemTaxDtlsDAO)
        {
            _iTblInvoiceItemTaxDtlsDAO = iTblInvoiceItemTaxDtlsDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList()
        {
            return _iTblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls();
        }

        public TblInvoiceItemTaxDtlsTO SelectTblInvoiceItemTaxDtlsTO(Int32 idInvItemTaxDtl)
        {
            return _iTblInvoiceItemTaxDtlsDAO.SelectTblInvoiceItemTaxDtls(idInvItemTaxDtl);
        }

        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls(invItemId, conn, tran);
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
        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls(invItemId, conn, tran);
        }

        public List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtlsByInvoiceId(invoiceId, conn, tran);
        }


        public List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtlsByInvoiceId(invoiceId, conn, tran);
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
        #endregion

        #region Insertion
        public int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            return _iTblInvoiceItemTaxDtlsDAO.InsertTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO);
        }

        public int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemTaxDtlsDAO.InsertTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            return _iTblInvoiceItemTaxDtlsDAO.UpdateTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO);
        }

        public int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemTaxDtlsDAO.UpdateTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl)
        {
            return _iTblInvoiceItemTaxDtlsDAO.DeleteTblInvoiceItemTaxDtls(idInvItemTaxDtl);
        }

        public int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemTaxDtlsDAO.DeleteTblInvoiceItemTaxDtls(idInvItemTaxDtl, conn, tran);
        }

        public int DeleteInvoiceItemTaxDtlsByInvId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblInvoiceItemTaxDtlsDAO.DeleteInvoiceItemTaxDtlsByInvId(invoiceId, conn, tran);

        }
        #endregion

    }
}
