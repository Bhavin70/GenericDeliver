using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStockYardDAO
    {
        String SqlSelectQuery();
        List<TblStockYardTO> SelectAllTblStockYard();
        TblStockYardTO SelectTblStockYard(Int32 idStockYard);
        List<TblStockYardTO> ConvertDTToList(SqlDataReader tblStockYardTODT);
        int InsertTblStockYard(TblStockYardTO tblStockYardTO);
        int InsertTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockYardTO tblStockYardTO, SqlCommand cmdInsert);
        int UpdateTblStockYard(TblStockYardTO tblStockYardTO);
        int UpdateTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockYardTO tblStockYardTO, SqlCommand cmdUpdate);
        int DeleteTblStockYard(Int32 idStockYard);
        int DeleteTblStockYard(Int32 idStockYard, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStockYard, SqlCommand cmdDelete);

    }
}