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
    public class TblKYCDetailsBL : ITblKYCDetailsBL
    {
        #region Selection
        public List<TblKYCDetailsTO> SelectAllTblKYCDetails()
        {
            return TblKYCDetailsDAO.SelectAllTblKYCDetails();
        }

        public List<TblKYCDetailsTO> SelectTblKYCDetailsTOByOrgId(Int32 organizationId)
        {
            return TblKYCDetailsDAO.SelectTblKYCDetailsTOByOrgId(organizationId);
        }
        public TblKYCDetailsTO SelectTblKYCDetailsTO(Int32 idKYCDetails)
        {
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblKYCDetailsDAO.SelectTblKYCDetails(idKYCDetails, conn, tran);
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
            SqlConnection conn = new SqlConnection(Startup.ConnectionString);
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return TblKYCDetailsDAO.SelectTblKYCDetailsTOByOrgId(organizationId, conn, tran);
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
            return TblKYCDetailsDAO.InsertTblKYCDetails(tblKYCDetailsTO);
        }

        public int InsertTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblKYCDetailsDAO.InsertTblKYCDetails(tblKYCDetailsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO)
        {
            return TblKYCDetailsDAO.UpdateTblKYCDetails(tblKYCDetailsTO);
        }

        public int UpdateTblKYCDetails(TblKYCDetailsTO tblKYCDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            return TblKYCDetailsDAO.UpdateTblKYCDetails(tblKYCDetailsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblKYCDetails(Int32 idKYCDetails)
        {
            return TblKYCDetailsDAO.DeleteTblKYCDetails(idKYCDetails);
        }

        public int DeleteTblKYCDetails(Int32 idKYCDetails, SqlConnection conn, SqlTransaction tran)
        {
            return TblKYCDetailsDAO.DeleteTblKYCDetails(idKYCDetails, conn, tran);
        }

        #endregion
        
    }
}
