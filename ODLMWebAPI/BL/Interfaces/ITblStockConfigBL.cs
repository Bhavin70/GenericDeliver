using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStockConfigBL
    { 
        List<TblStockConfigTO> SelectAllTblStockConfigTOList();
        TblStockConfigTO SelectTblStockConfigTO(Int32 idStockConfig);
        List<TblStockConfigTO> SelectAllTblStockConfigTOList(SqlConnection conn, SqlTransaction tran);
        int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO);
        int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO);
        int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeactivateTblStockConfig(TblStockConfigTO tblStockConfigTO);
        int DeleteTblStockConfig(Int32 idStockConfig);
        int DeleteTblStockConfig(Int32 idStockConfig, SqlConnection conn, SqlTransaction tran);
    }
}