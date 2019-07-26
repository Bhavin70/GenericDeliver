using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimBrandDAO
    {
        String SqlSelectQuery();
        List<DimBrandTO> SelectAllDimBrand();
        List<DimBrandTO> SelectAllDimBrand(DimBrandTO dimBrandTO);
        DimBrandTO SelectDimBrand(Int32 idBrand);
        DimBrandTO SelectDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran);
        List<DimBrandTO> SelectAllDimBrand(SqlConnection conn, SqlTransaction tran);
        List<DimBrandTO> ConvertDTToList(SqlDataReader dimBrandTODT);
        int InsertDimBrand(DimBrandTO dimBrandTO);
        int InsertDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimBrandTO dimBrandTO, SqlCommand cmdInsert);
        int UpdateDimBrand(DimBrandTO dimBrandTO);
        int UpdateDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimBrandTO dimBrandTO, SqlCommand cmdUpdate);
        int DeleteDimBrand(Int32 idBrand);
        int DeleteDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBrand, SqlCommand cmdDelete);

    }
}