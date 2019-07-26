using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface IDimAddonsFunBL
    {
        List<DimAddonsFunTO> SelectAllDimAddonsFunList();
        DimAddonsFunTO SelectDimAddonsFunTO(Int32 idAddonsFun);
        ResultMessage InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO);
        ResultMessage InsertDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO);
        ResultMessage UpdateDimAddonsFun(DimAddonsFunTO dimAddonsFunTO, SqlConnection conn, SqlTransaction tran);
        int DeleteDimAddonsFun(Int32 idAddonsFun);
        int DeleteDimAddonsFun(Int32 idAddonsFun, SqlConnection conn, SqlTransaction tran);
    }
}
