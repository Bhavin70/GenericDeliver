using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimBrandBL
    {
        List<DimBrandTO> SelectAllDimBrand();
        List<DimBrandTO> SelectAllDimBrandList();
        DimBrandTO SelectDimBrandTO(Int32 idBrand);
        DimBrandTO SelectDimBrandTO(Int32 idBrand, SqlConnection conn, SqlTransaction tran);
        List<DimBrandTO> SelectAllDimBrandList(DimBrandTO dimBrandTO);
        int InsertDimBrand(DimBrandTO dimBrandTO);
        int InsertDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimBrand(DimBrandTO dimBrandTO);
        int UpdateDimBrand(DimBrandTO dimBrandTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimBrand(Int32 idBrand);
        int DeleteDimBrand(Int32 idBrand, SqlConnection conn, SqlTransaction tran);
    }
}
