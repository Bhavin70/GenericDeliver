using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblRoleDAO
    {
        String SqlSelectQuery();
        String SqlGetDepartmentFromUserId();
        TblRoleTO SelectAllTblRole();
        TblRoleTO SelectTblRole(Int32 idRole);
        TblRoleTO SelectAllTblRole(SqlConnection conn, SqlTransaction tran);
        TblRoleTO SelectTblRoleOnOrgStructureId(Int32 OrgStructureId);
        List<TblRoleTO> ConvertDTToList(SqlDataReader tblRoleTODT);
        List<TblRoleTO> ConvertDepartmentDTToList(SqlDataReader tblRoleTODT);
        TblRoleTO SelectTblRoleOnOrgStructureId(Int32 OrgStructureId, SqlConnection conn, SqlTransaction tran);
        TblRoleTO getDepartmentIdFromUserId(Int32 userId);
        int InsertTblRole(TblRoleTO tblRoleTO);
        int InsertTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblRoleTO tblRoleTO, SqlCommand cmdInsert);
        int UpdateTblRole(TblRoleTO tblRoleTO);
        int UpdateTblRole(TblRoleTO tblRoleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblRoleTO tblRoleTO, SqlCommand cmdUpdate);
        int DeleteTblRole(Int32 idRole);
        int DeleteTblRole(Int32 idRole, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRole, SqlCommand cmdDelete);

    }
}