using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblVersionBL : ITblVersionBL
    {
        #region Selection
        private readonly ITblVersionDAO _iTblVersionDAO;
        public TblVersionBL(ITblVersionDAO iTblVersionDAO)
        {
            _iTblVersionDAO = iTblVersionDAO;
        }

        public List<TblVersionTO> SelectAllTblVersionList()
        {
            return _iTblVersionDAO.SelectAllTblVersion();
            //return ConvertDTToList(tblVersionTODT);
        }

        public TblVersionTO SelectTblVersionTO(Int32 idVersion)
        {
            return _iTblVersionDAO.SelectTblVersion(idVersion);

        }
        public TblVersionTO SelectLatestVersionTO()
        {
            return _iTblVersionDAO.SelectLatestVersionTO();

        }




        #endregion

        #region Insertion
        public int InsertTblVersion(TblVersionTO tblVersionTO)
        {
            return _iTblVersionDAO.InsertTblVersion(tblVersionTO);
        }

        public int InsertTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVersionDAO.InsertTblVersion(tblVersionTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblVersion(TblVersionTO tblVersionTO)
        {
            return _iTblVersionDAO.UpdateTblVersion(tblVersionTO);
        }

        public int UpdateTblVersion(TblVersionTO tblVersionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVersionDAO.UpdateTblVersion(tblVersionTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblVersion(Int32 idVersion)
        {
            return _iTblVersionDAO.DeleteTblVersion(idVersion);
        }

        public int DeleteTblVersion(Int32 idVersion, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVersionDAO.DeleteTblVersion(idVersion, conn, tran);
        }

        #endregion
        
    }
}
