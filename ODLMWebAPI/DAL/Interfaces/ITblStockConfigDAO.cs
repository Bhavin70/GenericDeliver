using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStockConfigDAO
    {
        String SqlSelectQuery();
        List<TblStockConfigTO> SelectAllTblStockConfigTOList();
        TblStockConfigTO SelectTblStockConfigTO(Int32 idStockConfig);
        List<TblStockConfigTO> SelectAllTblStockConfigTOList(SqlConnection conn, SqlTransaction tran);
        List<TblStockConfigTO> ConvertDTToList(SqlDataReader tblStockConfigTODT);
        int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO);
        int InsertTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockConfigTO tblStockConfigTO, SqlCommand cmdInsert);
        int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO);
        int UpdateTblStockConfig(TblStockConfigTO tblStockConfigTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockConfigTO tblStockConfigTO, SqlCommand cmdUpdate);
        int DeleteTblStockConfig(Int32 idStockConfig);
        int DeleteTblStockConfig(Int32 idStockConfig, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStockConfig, SqlCommand cmdDelete);

    }
}