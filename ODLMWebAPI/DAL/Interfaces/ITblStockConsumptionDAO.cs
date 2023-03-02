using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStockConsumptionDAO
    {
        String SqlSelectQuery();
        List<TblStockConsumptionTO> SelectAllTblStockConsumption();
        List<TblStockConsumptionTO> SelectAllStockConsumptionList(Int32 loadingSlipExtId, Int32 txnOpTypeId, SqlConnection conn, SqlTransaction tran);
        TblStockConsumptionTO SelectTblStockConsumption(Int32 idStockConsumption);
        List<TblStockConsumptionTO> ConvertDTToList(SqlDataReader tblStockConsumptionTODT);
        int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO);
        int InsertTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockConsumptionTO tblStockConsumptionTO, SqlCommand cmdInsert);
        int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO);
        int UpdateTblStockConsumption(TblStockConsumptionTO tblStockConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockConsumptionTO tblStockConsumptionTO, SqlCommand cmdUpdate);
        int DeleteTblStockConsumption(Int32 idStockConsumption);
        int DeleteTblStockConsumption(Int32 idStockConsumption, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStockConsumption, SqlCommand cmdDelete);
        Int64 GetLastIdStockConsumption( SqlConnection conn, SqlTransaction tran);

    }
}