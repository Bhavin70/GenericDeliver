using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSysEleUserEntitlementsBL
    {
        List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId);

        List<TblSysEleUserEntitlementsTO> SelectAllTblSysEleUserEntitlementsList(int userId, int? moduleId);
        TblSysEleUserEntitlementsTO SelectTblSysEleUserEntitlementsTO(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission);
        int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO);
        int InsertTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO);
        int UpdateTblSysEleUserEntitlements(TblSysEleUserEntitlementsTO tblSysEleUserEntitlementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission);
        int DeleteTblSysEleUserEntitlements(Int32 idUserEntitlement, Int32 userId, Int32 sysEleId, String permission, SqlConnection conn, SqlTransaction tran);
    }
}