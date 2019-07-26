using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimRoleTypeBL
    {
        List<DimRoleTypeTO> SelectAllDimRoleTypeList();
        DimRoleTypeTO SelectDimRoleTypeTO(Int32 idRoleType);
        int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO);
        int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO);
        int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimRoleType(Int32 idRoleType);
        int DeleteDimRoleType(Int32 idRoleType, SqlConnection conn, SqlTransaction tran);
    }
}
