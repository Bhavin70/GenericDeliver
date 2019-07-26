using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimPageElementTypesBL
    {
        List<DimPageElementTypesTO> SelectAllDimPageElementTypesList();
        DimPageElementTypesTO SelectDimPageElementTypesTO(Int32 idPageEleType);
        int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO);
        int InsertDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO);
        int UpdateDimPageElementTypes(DimPageElementTypesTO dimPageElementTypesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimPageElementTypes(Int32 idPageEleType);
        int DeleteDimPageElementTypes(Int32 idPageEleType, SqlConnection conn, SqlTransaction tran);
    }
}
