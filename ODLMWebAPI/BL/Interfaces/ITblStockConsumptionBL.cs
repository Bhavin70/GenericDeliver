using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStockConsumptionBL
    {
        List<TblStockConsumptionTO> SelectAllTblStockConsumptionList();
        TblStockConsumptionTO SelectTblStockConsumptionTO(Int32 idStockConsumption);
        List<TblStockConsumptionTO> SelectAllStockConsumptionList(Int32 loadingSlipExtId, Int32 txnOpTypeId, SqlConnection conn, SqlTransaction tran);
        int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO);
        int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO);
        int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStockConsumption(Int32 idStockConsumption);
        int DeleteTblStockConsumption(Int32 idStockConsumption, SqlConnection conn, SqlTransaction tran);
    }
}