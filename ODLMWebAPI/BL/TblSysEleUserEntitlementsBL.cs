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
    public class TblSysEleUserEntitlementsBL : ITblSysEleUserEntitlementsBL
    {
        private readonly ITblSysEleUserEntitlementsDAO _iTblSysEleUserEntitlementsDAO;
        public TblSysEleUserEntitlementsBL(ITblSysEleUserEntitlementsDAO iTblSysEleUserEntitlementsDAO)
        {
            _iTblSysEleUserEntitlementsDAO = iTblSysEleUserEntitlementsDAO;
        }
        #region Selection

        public List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId)
        {
            return  _iTblSysEleUserEntitlementsDAO.SelectAllTblSysEleUserEntitlements(userId,null);
        }
        public List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId,int? moduleId)
        {
            return _iTblSysEleUserEntitlementsDAO.SelectAllTblSysEleUserEntitlements(userId,moduleId);
        }


        public TblSysEleUserEntitlementsTO SelectTblSysEleUserEntitlementsTO(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission)
        {
           return  _iTblSysEleUserEntitlementsDAO.SelectTblSysEleUserEntitlements(idUserEntitlement, userId, sysEleId, permission);
        }

        

        #endregion
        
        #region Insertion
        public int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO)
        {
            return _iTblSysEleUserEntitlementsDAO.InsertTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO);
        }

        public int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysEleUserEntitlementsDAO.InsertTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO)
        {
            return _iTblSysEleUserEntitlementsDAO.UpdateTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO);
        }

        public int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysEleUserEntitlementsDAO.UpdateTblSysEleUserEntitlements(tblSysEleUserEntitlementsTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission)
        {
            return _iTblSysEleUserEntitlementsDAO.DeleteTblSysEleUserEntitlements(idUserEntitlement, userId, sysEleId, permission);
        }

        public int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblSysEleUserEntitlementsDAO.DeleteTblSysEleUserEntitlements(idUserEntitlement, userId, sysEleId, permission, conn, tran);
        }

        #endregion
         
    }
}
