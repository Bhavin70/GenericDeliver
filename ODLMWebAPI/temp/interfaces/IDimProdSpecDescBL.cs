using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimProdSpecDescBL
    {
        List<DimProdSpecDescTO> SelectAllDimProdSpecDescList();
        DimProdSpecDescTO SelectDimPRodSpecDescTO(Int32 idCodeType);
        int SelectAllDimProdSpecDescriptionList();
        int InsertDimProdSpecDesc(DimProdSpecDescTO ProSpecDesc);
        int InsertDimProdSpecDesc(DimProdSpecDescTO dimProSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO);
        int UpdateDimProSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO);
        int DeleteDimProSpecDesc(DimProdSpecDescTO DimProdSpecDescTO, SqlConnection conn, SqlTransaction tran);
    }
}
