using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimProdCatBL
    {
        List<DimProdCatTO> SelectAllDimProdCatList();
        DimProdCatTO SelectDimProdCatTO(Int32 idProdCat);
        int InsertDimProdCat(DimProdCatTO dimProdCatTO);
        int InsertDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimProdCat(DimProdCatTO dimProdCatTO);
        int UpdateDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimProdCat(Int32 idProdCat);
        int DeleteDimProdCat(Int32 idProdCat, SqlConnection conn, SqlTransaction tran);
    }
}
