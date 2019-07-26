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
    public class TblInvoiceAddressBL : ITblInvoiceAddressBL
    {
        #region Selection
        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList()
        {
            return  TblInvoiceAddressDAO.SelectAllTblInvoiceAddress();
        }

        public TblInvoiceAddressTO SelectTblInvoiceAddressTO(Int32 idInvoiceAddr)
        {
            return  TblInvoiceAddressDAO.SelectTblInvoiceAddress(idInvoiceAddr);
        }

        public List<TblInvoiceAddressTO> SelectAllTblInvoiceAddressList(Int32 invoiceId)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblInvoiceAddressDAO.SelectAllTblInvoiceAddress(invoiceId,conn,tran);
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
            return TblInvoiceAddressDAO.SelectAllTblInvoiceAddress(invoiceId, conn, tran);
        }
        #endregion

        #region Insertion
        public int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            return TblInvoiceAddressDAO.InsertTblInvoiceAddress(tblInvoiceAddressTO);
        }

        public int InsertTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.InsertTblInvoiceAddress(tblInvoiceAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO)
        {
            return TblInvoiceAddressDAO.UpdateTblInvoiceAddress(tblInvoiceAddressTO);
        }

        public int UpdateTblInvoiceAddress(TblInvoiceAddressTO tblInvoiceAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.UpdateTblInvoiceAddress(tblInvoiceAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblInvoiceAddress(Int32 idInvoiceAddr)
        {
            return TblInvoiceAddressDAO.DeleteTblInvoiceAddress(idInvoiceAddr);
        }

        public int DeleteTblInvoiceAddress(Int32 idInvoiceAddr, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.DeleteTblInvoiceAddress(idInvoiceAddr, conn, tran);
        }
        public int DeleteTblInvoiceAddressByinvoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran)
        {
            return TblInvoiceAddressDAO.DeleteTblInvoiceAddressByinvoiceId(invoiceId, conn, tran);
        }
        #endregion

    }
}
