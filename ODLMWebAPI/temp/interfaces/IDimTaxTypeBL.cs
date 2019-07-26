using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimTaxTypeBL
    {
        List<DimTaxTypeTO> SelectAllDimTaxTypeList();
        DimTaxTypeTO SelectDimTaxTypeTO(Int32 idTaxType);
        int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO);
        int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO);
        int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimTaxType(Int32 idTaxType);
        int DeleteDimTaxType(Int32 idTaxType, SqlConnection conn, SqlTransaction tran);
    }
}
