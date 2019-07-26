using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimVehDocTypeBL
    {
        List<DimVehDocTypeTO> SelectAllDimVehDocType();
        List<DimVehDocTypeTO> SelectAllDimVehDocTypeList();
        DimVehDocTypeTO SelectDimVehDocTypeTO(Int32 idVehDocType);
        int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO);
        int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO);
        int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimVehDocType(Int32 idVehDocType);
        int DeleteDimVehDocType(Int32 idVehDocType, SqlConnection conn, SqlTransaction tran);
    }
}
