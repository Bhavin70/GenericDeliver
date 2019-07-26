using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVisitFollowUpRolesBL
    {
        List<TblVisitFollowUpRolesTO> SelectAllTblVisitFollowUpRolesList();
        List<DropDownTO> SelectFollowUpUserRoleListForDropDown();
        List<DropDownTO> SelectFollowUpRoleListForDropDown();
        TblVisitFollowUpRolesTO SelectTblVisitFollowUpRolesTO(Int32 idVisitFollowUpRole);
        List<TblVisitFollowUpRolesTO> ConvertDTToList(DataTable tblVisitFollowUpRolesTODT);
        List<DropDownTO> SelectVisitRoleListForDropDown();
        int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO);
        int InsertTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO);
        int UpdateTblVisitFollowUpRoles(TblVisitFollowUpRolesTO tblVisitFollowUpRolesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole);
        int DeleteTblVisitFollowUpRoles(Int32 idVisitFollowUpRole, SqlConnection conn, SqlTransaction tran);
    }
}