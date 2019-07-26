using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblSysElementsBL
    {
        TblSysElementsTO SelectTblSysElementsTO(Int32 idSysElement);
        Dictionary<int, String> SelectSysElementUserEntitlementDCT(int userId, int roleId);
        Dictionary<int, String> SelectSysElementUserMultiRoleEntitlementDCT(int userId, String roleId,int? moduleId);
        List<PermissionTO> SelectAllPermissionList(int menuPgId, int roleId, int userId, int moduleId);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO);
        int InsertTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveRoleOrUserPermission(PermissionTO permissionTO);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO);
        int UpdateTblSysElements(TblSysElementsTO tblSysElementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSysElements(Int32 idSysElement);
        int DeleteTblSysElements(Int32 idSysElement, SqlConnection conn, SqlTransaction tran);
        List<PermissionTO> getAllUsersWithModulePermission(int moduleId,int role_Id,int DeptId);
          ResultMessage SaveAllUserPermission(PermissionTO permissionTO);
    }
}