using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ODLMWebAPI.BL
{
    public interface IDimAddonsFunDAO
    {
        String SqlSelectQuery();
        List<DimAddonsFunTO> SelectAllDimAddonsFun();
        DimAddonsFunTO SelectDimAddonsFun(Int32 idAddonsFun);
        List<DimAddonsFunTO> ConvertDTToList(SqlDataReader dimAddonsFunTODT);
        List<DimAddonsFunTO> SelectAllDimAddonsFun(SqlConnection conn, SqlTransaction tran);
        int InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO);
        int InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimAddonsFunTO dimAddonsFunTO, SqlCommand cmdInsert);
        int UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO);
        int UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimAddonsFunTO dimAddonsFunTO, SqlCommand cmdUpdate);
        int DeleteDimAddonsFun(Int32 idAddonsFun);
        int DeleteDimAddonsFun(Int32 idAddonsFun, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idAddonsFun, SqlCommand cmdDelete);
    } 
}