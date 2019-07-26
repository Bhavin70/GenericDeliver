using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimOrgTypeBL
    {
        List<DimOrgTypeTO> SelectAllDimOrgTypeList();
        DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType, SqlConnection conn, SqlTransaction tran);
        DimOrgTypeTO SelectDimOrgTypeTO(Int32 idOrgType);
        int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO);
        int InsertDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO);
        int UpdateDimOrgType(DimOrgTypeTO dimOrgTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimOrgType(Int32 idOrgType);
        int DeleteDimOrgType(Int32 idOrgType, SqlConnection conn, SqlTransaction tran);
    }
}
