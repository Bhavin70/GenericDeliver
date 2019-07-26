using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimProdSpecDAO
    {
        String SqlSelectQuery();
        List<DimProdSpecTO> SelectAllDimProdSpec();
        DimProdSpecTO SelectDimProdSpec(Int32 idProdSpec);
        List<DimProdSpecTO> ConvertDTToList(SqlDataReader dimProdSpecTODT);
        int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO);
        int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(DimProdSpecTO dimProdSpecTO, SqlCommand cmdInsert);
        int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO);
        int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(DimProdSpecTO dimProdSpecTO, SqlCommand cmdUpdate);
        int DeleteDimProdSpec(Int32 idProdSpec);
        int DeleteDimProdSpec(Int32 idProdSpec, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProdSpec, SqlCommand cmdDelete);

    }
}