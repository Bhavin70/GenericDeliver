using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{ 
    public class TblOrgPersonDtlsBL : ITblOrgPersonDtlsBL
    {
        private readonly ITblOrgPersonDtlsDAO _iTblOrgPersonDtlsDAO;
        public TblOrgPersonDtlsBL(ITblOrgPersonDtlsDAO iTblOrgPersonDtlsDAO)
        {
            _iTblOrgPersonDtlsDAO = iTblOrgPersonDtlsDAO;
        }
        #region Selection
        public List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtls()
        {
            return _iTblOrgPersonDtlsDAO.SelectAllTblOrgPersonDtls();
        }

        public List<TblOrgPersonDtlsTO> SelectAllTblOrgPersonDtlsList()
        {
            List<TblOrgPersonDtlsTO> tblOrgPersonDtlsTODT = _iTblOrgPersonDtlsDAO.SelectAllTblOrgPersonDtls();
            if (tblOrgPersonDtlsTODT != null && tblOrgPersonDtlsTODT.Count > 0)
                return tblOrgPersonDtlsTODT;
            else
                return null;
        }

        public TblOrgPersonDtlsTO SelectTblOrgPersonDtlsTO(Int32 idOrgPersonDtl)
        {
            TblOrgPersonDtlsTO tblOrgPersonDtlsTODT = _iTblOrgPersonDtlsDAO.SelectTblOrgPersonDtls(idOrgPersonDtl);
            if (tblOrgPersonDtlsTODT != null )
                return tblOrgPersonDtlsTODT;
            else
                return null;
        }
        #endregion

        #region Insertion
        public int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO)
        {
            return _iTblOrgPersonDtlsDAO.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO);
        }

        public int InsertTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgPersonDtlsDAO.InsertTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO)
        {
            return _iTblOrgPersonDtlsDAO.UpdateTblOrgPersonDtls(tblOrgPersonDtlsTO);
        }

        public int UpdateTblOrgPersonDtls(TblOrgPersonDtlsTO tblOrgPersonDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgPersonDtlsDAO.UpdateTblOrgPersonDtls(tblOrgPersonDtlsTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl)
        {
            return _iTblOrgPersonDtlsDAO.DeleteTblOrgPersonDtls(idOrgPersonDtl);
        }

        public int DeleteTblOrgPersonDtls(Int32 idOrgPersonDtl, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgPersonDtlsDAO.DeleteTblOrgPersonDtls(idOrgPersonDtl, conn, tran);
        }

        #endregion

    }
}
