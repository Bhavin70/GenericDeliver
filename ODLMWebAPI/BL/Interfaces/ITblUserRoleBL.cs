using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserRoleBL
    {
        List<TblUserRoleTO> SelectAllTblUserRoleList();
        List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId);
        TblUserRoleTO SelectTblUserRoleTO(Int32 idUserRole);
        Int32 IsAreaConfigurationEnabled(Int32 userId);
        List<TblUserRoleTO> SelectAllActiveUserRoleList(Int32 userId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectUsersFromRoleForDropDown(Int32 roleId);
        List<DropDownTO> SelectUsersFromRoleIdsForDropDown(string roleId);
        List<DropDownTO> SelectUsersFromRoleTypeForDropDown(Int32 roleTypeId);
        TblUserRoleTO SelectUserRoleTOAccToPriority(List<TblUserRoleTO> userRoleTOList);
        Boolean selectRolePriorityForOther(List<TblUserRoleTO> userRoleToList);
        int InsertTblUserRole(TblUserRoleTO tblUserRoleTO);
        int InsertTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO);
        int UpdateTblUserRole(TblUserRoleTO tblUserRoleTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserRole(Int32 idUserRole);
        int DeleteTblUserRole(Int32 idUserRole, SqlConnection conn, SqlTransaction tran);
    }
}