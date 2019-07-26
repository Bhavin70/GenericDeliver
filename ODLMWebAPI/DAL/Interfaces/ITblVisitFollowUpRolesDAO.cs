using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitFollowUpRolesDAO
    {
        String SqlSelectQuery();
        List<TblVisitFollowUpRolesTO> SelectAllTblVisitFollowUpRoles();
        DataTable SelectTblVisitFollowUpRoles(Int32 idVisitFollowUpRole);
        DataTable SelectAllTblVisitFollowUpRoles(SqlConnection conn, SqlTransaction tran);
        List<TblVisitFollowUpRolesTO> ConvertDTToList(SqlDataReader visitFollowUpRoleDT);
        List<DropDownTO> SelectFollowUpUserRoleListForDropDown();
        List<DropDownTO> SelectFollowUpRoleListForDropDown();
        List<DropDownTO> SelectVisitRoleForDropDown();
        int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO);
        int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlCommand cmdInsert);
        int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO);
        int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlCommand cmdUpdate);
        int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole);
        int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVisitFollowUpRole, SqlCommand cmdDelete);

    }
}