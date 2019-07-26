using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimMstDeptDAO
    {
        String SqlSelectQuery();
        List<DimMstDeptTO> SelectAllDimMstDept();
        DimMstDeptTO SelectDimMstDept(Int32 idDept);
        List<DimMstDeptTO> SelectAllDimMstDept(SqlConnection conn, SqlTransaction tran);
        DimMstDeptTO SelectDimMstDept(Int32 idDept, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllDepartmentMasterForDropDown(Int32 DeptTypeId);
        List<DropDownTO> SelectDepartmentDropDownListByDivision(Int32 DivisionId);
        DropDownTO SelectBOMDepartmentTO();
        List<DropDownTO> SelectSubDepartmentDropDownListByDepartment(Int32 DepartmentId);
        List<DimMstDeptTO> ConvertDTToList(SqlDataReader dimMstDeptTODT);
        Int32 SelectLastDeptId(SqlConnection conn, SqlTransaction tran);
        int InsertDimMstDept(DimMstDeptTO dimMstDeptTO);
        int InsertDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimMstDeptTO dimMstDeptTO, SqlCommand cmdInsert);
        int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO);
        int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimMstDeptTO dimMstDeptTO, SqlCommand cmdUpdate);
        int DeleteDimMstDept(Int32 idDept);
        int DeleteDimMstDept(Int32 idDept, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idDept, SqlCommand cmdDelete);

    }
}