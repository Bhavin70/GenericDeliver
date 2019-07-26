using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimProdSpecBL
    {
        List<DimProdSpecTO> SelectAllDimProdSpecList();
        DimProdSpecTO SelectDimProdSpecTO(Int32 idProdSpec);
        int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO);
        int InsertDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO);
        int UpdateDimProdSpec(DimProdSpecTO dimProdSpecTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimProdSpec(Int32 idProdSpec);
        int DeleteDimProdSpec(Int32 idProdSpec, SqlConnection conn, SqlTransaction tran);
    }
}
