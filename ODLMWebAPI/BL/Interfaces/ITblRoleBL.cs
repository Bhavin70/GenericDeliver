using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblRoleBL
    {
        TblRoleTO SelectAllTblRole();
        List<TblRoleTO> SelectAllTblRoleList();
        TblRoleTO SelectTblRoleTO(Int32 idRole);
        List<TblRoleTO> ConvertDTToList(TblRoleTO tblRoleTODT);
        TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId);
        TblRoleTO SelectTblRoleOnOrgStructureId(Int32 orgStructutreId, SqlConnection conn, SqlTransaction tran);
        TblRoleTO getDepartmentIdFromUserId(Int32 userId);
        int InsertTblRole(TblRoleTO tblRoleTO);
        int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblRole(TblRoleTO tblRoleTO);
        int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateRoleType(TblOrgStructureTO tblOrgStructureTO);
        int DeleteTblRole(Int32 idRole);
        int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran);
    }
}