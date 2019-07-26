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
    public class TblOrgAddressBL : ITblOrgAddressBL
    {
        private readonly ITblOrgAddressDAO _iTblOrgAddressDAO;
        public TblOrgAddressBL(ITblOrgAddressDAO iTblOrgAddressDAO)
        {
            _iTblOrgAddressDAO = iTblOrgAddressDAO;
        }
        #region Selection

        public List<TblOrgAddressTO> SelectAllTblOrgAddressList()
        {
          return _iTblOrgAddressDAO.SelectAllTblOrgAddress();
           
        }

        public TblOrgAddressTO SelectTblOrgAddressTO(Int32 idOrgAddr)
        {
            return  _iTblOrgAddressDAO.SelectTblOrgAddress(idOrgAddr);
           
        }

       

        #endregion
        
        #region Insertion
        public int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO)
        {
            return _iTblOrgAddressDAO.InsertTblOrgAddress(tblOrgAddressTO);
        }

        public int InsertTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgAddressDAO.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO)
        {
            return _iTblOrgAddressDAO.UpdateTblOrgAddress(tblOrgAddressTO);
        }

        public int UpdateTblOrgAddress(TblOrgAddressTO tblOrgAddressTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgAddressDAO.UpdateTblOrgAddress(tblOrgAddressTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblOrgAddress(Int32 idOrgAddr)
        {
            return _iTblOrgAddressDAO.DeleteTblOrgAddress(idOrgAddr);
        }

        public int DeleteTblOrgAddress(Int32 idOrgAddr, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrgAddressDAO.DeleteTblOrgAddress(idOrgAddr, conn, tran);
        }

        #endregion
        
    }
}
