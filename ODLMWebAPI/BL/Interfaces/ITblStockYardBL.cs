using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStockYardBL
    {
        List<TblStockYardTO> SelectAllTblStockYardList();
        TblStockYardTO SelectTblStockYardTO(Int32 idStockYard);
        int InsertTblStockYard(TblStockYardTO tblStockYardTO);
        int InsertTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStockYard(TblStockYardTO tblStockYardTO);
        int UpdateTblStockYard(TblStockYardTO tblStockYardTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStockYard(Int32 idStockYard);
        int DeleteTblStockYard(Int32 idStockYard, SqlConnection conn, SqlTransaction tran);
    }
}