using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblConfigParamsBL
    {
        List<TblConfigParamsTO> SelectAllTblConfigParamsList();
        TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName);
        TblConfigParamsTO SelectTblConfigParamsTO(Int32 idConfigParam);
        TblConfigParamsTO SelectTblConfigParamsTO(String configParamName);
        TblConfigParamsTO SelectTblConfigParamsTO(string configParamName, SqlConnection conn, SqlTransaction tran);
        Int32 GetStockConfigIsConsolidate();
        int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO);
        int InsertTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO);
        int UpdateTblConfigParams(TblConfigParamsTO tblConfigParamsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblConfigParams(Int32 idConfigParam);
        int DeleteTblConfigParams(Int32 idConfigParam, SqlConnection conn, SqlTransaction tran);
    }
}
