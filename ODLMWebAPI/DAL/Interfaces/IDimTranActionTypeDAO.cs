using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimTranActionTypeDAO
    {
        String SqlSelectQuery();
        List<DimTranActionTypeTO> SelectAllDimTranActionType();
        DimTranActionTypeTO SelectDimTranActionType(Int32 idTranActionType);
        List<DimTranActionTypeTO> SelectAllDimTranActionType(SqlConnection conn, SqlTransaction tran);
        List<DimTranActionTypeTO> ConvertDTToList(SqlDataReader dimTranActionTypeTODT);
        int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO);
        int InsertDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimTranActionTypeTO dimTranActionTypeTO, SqlCommand cmdInsert);
        int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO);
        int UpdateDimTranActionType(DimTranActionTypeTO dimTranActionTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimTranActionTypeTO dimTranActionTypeTO, SqlCommand cmdUpdate);
        int DeleteDimTranActionType(Int32 idTranActionType);
        int DeleteDimTranActionType(Int32 idTranActionType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTranActionType, SqlCommand cmdDelete);

    }
}