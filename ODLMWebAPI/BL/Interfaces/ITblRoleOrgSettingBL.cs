using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblRoleOrgSettingBL
    {  
        List<DropDownTO> SelectSavedRoles(int visitTypeId, int personTypeId);
        List<DropDownTO> SelectSavedOrg(int visitTypeId, int personTypeId);
        List<DropDownTO> SelectAllSystemRoleOrgListForDropDown(int visitTypeId, int personTypeId);
        ResultMessage SaveRolesAndOrg(List<RoleOrgTO> roleOrgTOList);
    }
}