using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblRoleOrgSettingDAO
    {
        int CheckIfExists(RoleOrgTO roleOrgTO);
        List<DropDownTO> SelectSavedRoles(int visitTypeId, int personTypeId);
        int UpdateRolesAndOrg(RoleOrgTO roleOrgTO, SqlConnection conn);
        int SaveRolesAndOrg(RoleOrgTO roleOrgTO, SqlConnection conn);
        List<DropDownTO> SelectAllSystemRoleOrgListForDropDown(int visitTypeId, int personTypeId);
        List<DropDownTO> SelectSavedOrg(int visitTypeId, int personTypeId);
    }
}