using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimVehDocTypeDAO
    {
        String SqlSelectQuery();
        List<DimVehDocTypeTO> SelectAllDimVehDocType();
        DimVehDocTypeTO SelectDimVehDocType(Int32 idVehDocType);
        List<DimVehDocTypeTO> SelectAllDimVehDocType(SqlConnection conn, SqlTransaction tran);
        List<DimVehDocTypeTO> ConvertDTToList(SqlDataReader dimVehDocTypeTODT);
        int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO);
        int InsertDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimVehDocTypeTO dimVehDocTypeTO, SqlCommand cmdInsert);
        int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO);
        int UpdateDimVehDocType(DimVehDocTypeTO dimVehDocTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimVehDocTypeTO dimVehDocTypeTO, SqlCommand cmdUpdate);
        int DeleteDimVehDocType(Int32 idVehDocType);
        int DeleteDimVehDocType(Int32 idVehDocType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVehDocType, SqlCommand cmdDelete);

    }
}