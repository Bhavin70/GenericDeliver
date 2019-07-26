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
    public class TblInvoiceItemTaxDtlsBL : ITblInvoiceItemTaxDtlsBL
    {
        #region Selection

        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList()
        {
            return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls();
        }

        public TblInvoiceItemTaxDtlsTO SelectTblInvoiceItemTaxDtlsTO(Int32 idInvItemTaxDtl)
        {
            return TblInvoiceItemTaxDtlsDAO.SelectTblInvoiceItemTaxDtls(idInvItemTaxDtl);
        }

        public List<TblInvoiceItemTaxDtlsTO> SelectAllTblInvoiceItemTaxDtlsList(Int32 invItemId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls(invItemId, conn, tran);
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
            return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtls(invItemId, conn, tran);
        }

        public List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtlsByInvoiceId(invoiceId, conn, tran);
        }


        public List<TblInvoiceItemTaxDtlsTO> SelectInvoiceItemTaxDtlsListByInvoiceId(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceItemTaxDtlsDAO.SelectAllTblInvoiceItemTaxDtlsByInvoiceId(invoiceId, conn, tran);
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
            return TblInvoiceItemTaxDtlsDAO.InsertTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO);
        }

        public int InsertTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.InsertTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO)
        {
            return TblInvoiceItemTaxDtlsDAO.UpdateTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO);
        }

        public int UpdateTblInvoiceItemTaxDtls(TblInvoiceItemTaxDtlsTO tblInvoiceItemTaxDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.UpdateTblInvoiceItemTaxDtls(tblInvoiceItemTaxDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl)
        {
            return TblInvoiceItemTaxDtlsDAO.DeleteTblInvoiceItemTaxDtls(idInvItemTaxDtl);
        }

        public int DeleteTblInvoiceItemTaxDtls(Int32 idInvItemTaxDtl, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.DeleteTblInvoiceItemTaxDtls(idInvItemTaxDtl, conn, tran);
        }

        public int DeleteInvoiceItemTaxDtlsByInvId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceItemTaxDtlsDAO.DeleteInvoiceItemTaxDtlsByInvId(invoiceId, conn, tran);

        }
        #endregion

    }
}
