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
    public class TblKYCDetailsBL : ITblKYCDetailsBL
    {
        private readonly ITblKYCDetailsDAO _iTblKYCDetailsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblKYCDetailsBL(IConnectionString iConnectionString, ITblKYCDetailsDAO iTblKYCDetailsDAO)
        {
            _iTblKYCDetailsDAO = iTblKYCDetailsDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public List<TblKYCDetailsTO> SelectAllTblKYCDetails()
        {
            return _iTblKYCDetailsDAO.SelectAllTblKYCDetails();
        }

        public List<TblKYCDetailsTO> SelectTblKYCDetailsTOByOrgId(Int32 organizationId)
        {
            return _iTblKYCDetailsDAO.SelectTblKYCDetailsTOByOrgId(organizationId);
        }
        public TblKYCDetailsTO SelectTblKYCDetailsTO(Int32 idKYCDetails)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblKYCDetailsDAO.SelectTblKYCDetails(idKYCDetails, conn, tran);
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
        public TblKYCDetailsTO SelectTblKYCDetailsTOByOrg(Int32 organizationId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblKYCDetailsDAO.SelectTblKYCDetailsTOByOrgId(organizationId, conn, tran);
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
        public int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            return _iTblKYCDetailsDAO.InsertTblKYCDetails(tblKYCDetailsTO);
        }

        public int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblKYCDetailsDAO.InsertTblKYCDetails(tblKYCDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            return _iTblKYCDetailsDAO.UpdateTblKYCDetails(tblKYCDetailsTO);
        }

        public int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblKYCDetailsDAO.UpdateTblKYCDetails(tblKYCDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblKYCDetails(Int32 idKYCDetails)
        {
            return _iTblKYCDetailsDAO.DeleteTblKYCDetails(idKYCDetails);
        }

        public int DeleteTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblKYCDetailsDAO.DeleteTblKYCDetails(idKYCDetails, conn, tran);
        }

        #endregion
        
    }
}
