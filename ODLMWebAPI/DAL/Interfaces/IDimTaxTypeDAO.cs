using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimTaxTypeDAO
    {
        String SqlSelectQuery();
        List<DimTaxTypeTO> SelectAllDimTaxType();
        DimTaxTypeTO SelectDimTaxType(Int32 idTaxType);
        List<DimTaxTypeTO> ConvertDTToList(SqlDataReader dimTaxTypeTODT);
        int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO);
        int InsertDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimTaxTypeTO dimTaxTypeTO, SqlCommand cmdInsert);
        int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO);
        int UpdateDimTaxType(DimTaxTypeTO dimTaxTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimTaxTypeTO dimTaxTypeTO, SqlCommand cmdUpdate);
        int DeleteDimTaxType(Int32 idTaxType);
        int DeleteDimTaxType(Int32 idTaxType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTaxType, SqlCommand cmdDelete);

    }
}