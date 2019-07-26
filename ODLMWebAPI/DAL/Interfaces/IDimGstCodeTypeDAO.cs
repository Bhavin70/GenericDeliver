using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimGstCodeTypeDAO
    {
        String SqlSelectQuery();
        List<DimGstCodeTypeTO> SelectAllDimGstCodeType();
        DimGstCodeTypeTO SelectDimGstCodeType(Int32 idCodeType);
        List<DimGstCodeTypeTO> ConvertDTToList(SqlDataReader dimGstCodeTypeTODT);
        int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO);
        int InsertDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimGstCodeTypeTO dimGstCodeTypeTO, SqlCommand cmdInsert);
        int UpdateDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO);
        int UpdateDimGstCodeType(DimGstCodeTypeTO dimGstCodeTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimGstCodeTypeTO dimGstCodeTypeTO, SqlCommand cmdUpdate);
        int DeleteDimGstCodeType(Int32 idCodeType);
        int DeleteDimGstCodeType(Int32 idCodeType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idCodeType, SqlCommand cmdDelete);

    }
}