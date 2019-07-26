using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimGstCodeTypeBL
    {
        List<DimGstCodeTypeTO> SelectAllDimGstCodeTypeList();
        DimGstCodeTypeTO SelectDimGstCodeTypeTO(Int32 idCodeType);
        int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO);
        int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimGstCodeType(Int32 idCodeType);
        int DeleteDimGstCodeType(Int32 idCodeType, SqlConnection conn, SqlTransaction tran);
    }
}
