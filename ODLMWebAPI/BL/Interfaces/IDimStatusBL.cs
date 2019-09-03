using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimStatusBL
    {
        List<DimStatusTO> SelectAllDimStatusList();
        List<DimStatusTO> SelectAllDimStatusList(Int32 txnTypeId);
        DimStatusTO SelectDimStatusTO(Int32 idStatus);
        int InsertDimStatus(DimStatusTO dimStatusTO);
        int InsertDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimStatus(DimStatusTO dimStatusTO);
        int UpdateDimStatus(DimStatusTO dimStatusTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimStatus(Int32 idStatus);
        DimStatusTO SelectDimStatusOnOrgId(int orgId);
        int DeleteDimStatus(Int32 idStatus, SqlConnection conn, SqlTransaction tran);
    }
}
