using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimMstDeptBL
    {
        List<DimMstDeptTO> SelectAllDimMstDeptList();
        DimMstDeptTO SelectDimMstDeptTO(Int32 idDept);
        List<DropDownTO> SelectDivisionDropDownList(Int32 DeptTypeId);
        List<DropDownTO> SelectDepartmentDropDownListByDivision(Int32 DivisionId);
        DropDownTO SelectBOMDepartmentTO();
        List<DimMstDeptTO> SelectAllDimMstDeptList(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectSubDepartmentDropDownListByDepartment(Int32 DepartmentId);
        int InsertDimMstDept(DimMstDeptTO dimMstDeptTO);
        int InsertDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveDepartment(DimMstDeptTO dimMstDeptTO);
        int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO);
        int UpdateDimMstDept(DimMstDeptTO dimMstDeptTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateDepartment(DimMstDeptTO dimMstDeptTO);
        int DeleteDimMstDept(Int32 idDept);
        int DeleteDimMstDept(Int32 idDept, SqlConnection conn, SqlTransaction tran);
    }
}
