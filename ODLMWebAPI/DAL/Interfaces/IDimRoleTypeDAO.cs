using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimRoleTypeDAO
    {
        String SqlSelectQuery();
        List<DimRoleTypeTO> SelectAllDimRoleTypeList();
        DimRoleTypeTO SelectDimRoleType(Int32 idRoleType);
        List<DimRoleTypeTO> ConvertDTToList(SqlDataReader dimRoleTypeTODT);
        int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO);
        int InsertDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimRoleTypeTO dimRoleTypeTO, SqlCommand cmdInsert);
        int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO);
        int UpdateDimRoleType(DimRoleTypeTO dimRoleTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimRoleTypeTO dimRoleTypeTO, SqlCommand cmdUpdate);
        int DeleteDimRoleType(Int32 idRoleType);
        int DeleteDimRoleType(Int32 idRoleType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idRoleType, SqlCommand cmdDelete);

    }
}