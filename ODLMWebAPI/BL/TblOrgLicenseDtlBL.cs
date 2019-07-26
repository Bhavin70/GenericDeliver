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
    public class TblOrgLicenseDtlBL : ITblOrgLicenseDtlBL
    {
        private readonly ITblOrgLicenseDtlDAO _iTblOrgLicenseDtlDAO;
        public TblOrgLicenseDtlBL(ITblOrgLicenseDtlDAO iTblOrgLicenseDtlDAO)
        {
            _iTblOrgLicenseDtlDAO = iTblOrgLicenseDtlDAO;
        }
        #region Selection

        public List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId)
        {
           return _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgId);
        }

        public List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId,SqlConnection conn,SqlTransaction tran)
        {
            return _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgId,conn,tran);
        }

        public TblOrgLicenseDtlTO SelectTblOrgLicenseDtlTO(Int32 idOrgLicense)
        {
           return  _iTblOrgLicenseDtlDAO.SelectTblOrgLicenseDtl(idOrgLicense);
        }

        public List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtlList(Int32 orgId,Int32 licenseId,String licenseVal)
        {
            return _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgId,licenseId,licenseVal);
        }

        #endregion

        #region Insertion
        public int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            return _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(tblOrgLicenseDtlTO);
        }

        public int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(tblOrgLicenseDtlTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO)
        {
            return _iTblOrgLicenseDtlDAO.UpdateTblOrgLicenseDtl(tblOrgLicenseDtlTO);
        }

        public int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgLicenseDtlDAO.UpdateTblOrgLicenseDtl(tblOrgLicenseDtlTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblOrgLicenseDtl(Int32 idOrgLicense)
        {
            return _iTblOrgLicenseDtlDAO.DeleteTblOrgLicenseDtl(idOrgLicense);
        }

        public int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgLicenseDtlDAO.DeleteTblOrgLicenseDtl(idOrgLicense, conn, tran);
        }

        #endregion
        
    }
}
