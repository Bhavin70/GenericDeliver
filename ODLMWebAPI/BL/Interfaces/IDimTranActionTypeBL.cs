using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimTranActionTypeBL
    {
        List<DimTranActionTypeTO> SelectAllDimTranActionType();
        DimTranActionTypeTO SelectDimTranActionTypeTO(Int32 idTranActionType);
        int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO);
        int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO);
        int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimTranActionType(Int32 idTranActionType);
        int DeleteDimTranActionType(Int32 idTranActionType, SqlConnection conn, SqlTransaction tran);
    }
}
