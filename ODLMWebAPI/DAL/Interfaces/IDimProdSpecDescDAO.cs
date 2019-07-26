using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimProdSpecDescDAO
    {
        String SqlSelectQuery();
        List<DimProdSpecDescTO> SelectAllDimProdSpecDesc();
        DimProdSpecDescTO SelectDimProdSpecDesc(Int32 idProdSpec);
        List<DimProdSpecDescTO> ConvertDTToList(SqlDataReader dimDimProdSpecTODT);
        int SelectDimProdSpecDescription();
        int InsertDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO);
        int InsertDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdInsert);
        int ExecuteSelectCommand(SqlCommand cmdInsert);
        int UpdateDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO);
        int UpdateDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdUpdate);
        int UpdateDimProdSpecDescription(DimProdSpecDescTO dimProdSpecDescTO);
        int UpdateDimProdSpecDescription(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdateCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdUpdate);

    }
}