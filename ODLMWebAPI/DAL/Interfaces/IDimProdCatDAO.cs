using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimProdCatDAO
    {
        String SqlSelectQuery();
        List<DimProdCatTO> SelectAllDimProdCat();
        DimProdCatTO SelectDimProdCat(Int32 idProdCat);
        List<DimProdCatTO> ConvertDTToList(SqlDataReader dimProdCatTODT);
        int InsertDimProdCat(DimProdCatTO dimProdCatTO);
        int InsertDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimProdCatTO dimProdCatTO, SqlCommand cmdInsert);
        int UpdateDimProdCat(DimProdCatTO dimProdCatTO);
        int UpdateDimProdCat(DimProdCatTO dimProdCatTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimProdCatTO dimProdCatTO, SqlCommand cmdUpdate);
        int DeleteDimProdCat(Int32 idProdCat);
        int DeleteDimProdCat(Int32 idProdCat, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdCat, SqlCommand cmdDelete);

    }
}