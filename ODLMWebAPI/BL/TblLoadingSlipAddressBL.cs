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
    public class TblLoadingSlipAddressBL : ITblLoadingSlipAddressBL
    {
        private readonly ITblLoadingSlipAddressDAO _iTblLoadingSlipAddressDAO;
        private readonly IConnectionString _iConnectionString;
        public TblLoadingSlipAddressBL(IConnectionString iConnectionString, ITblLoadingSlipAddressDAO iTblLoadingSlipAddressDAO)
        {
            _iTblLoadingSlipAddressDAO = iTblLoadingSlipAddressDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddressList(Int32 loadingSlipId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblLoadingSlipAddressDAO.SelectAllTblLoadingSlipAddress(loadingSlipId,conn,tran);
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

        public List<TblLoadingSlipAddressTO> SelectAllTblLoadingSlipAddressList(Int32 loadingSlipId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblLoadingSlipAddressDAO.SelectAllTblLoadingSlipAddress(loadingSlipId,conn,tran);
        }

        public TblLoadingSlipAddressTO SelectTblLoadingSlipAddressTO(Int32 idLoadSlipAddr)
        {
            return _iTblLoadingSlipAddressDAO.SelectTblLoadingSlipAddress(idLoadSlipAddr);
        }

        #endregion
        
        #region Insertion
        public int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO)
        {
            return _iTblLoadingSlipAddressDAO.InsertTblLoadingSlipAddress(tblLoadingSlipAddressTO);
        }

        public int InsertTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipAddressDAO.InsertTblLoadingSlipAddress(tblLoadingSlipAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO)
        {
            return _iTblLoadingSlipAddressDAO.UpdateTblLoadingSlipAddress(tblLoadingSlipAddressTO);
        }

        public int UpdateTblLoadingSlipAddress(TblLoadingSlipAddressTO tblLoadingSlipAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipAddressDAO.UpdateTblLoadingSlipAddress(tblLoadingSlipAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr)
        {
            return _iTblLoadingSlipAddressDAO.DeleteTblLoadingSlipAddress(idLoadSlipAddr);
        }

        public int DeleteTblLoadingSlipAddress(Int32 idLoadSlipAddr, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblLoadingSlipAddressDAO.DeleteTblLoadingSlipAddress(idLoadSlipAddr, conn, tran);
        }

        #endregion
        
    }
}
