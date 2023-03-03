using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblStockSummaryDAO
    {
        String SqlSelectQuery();
        List<TblStockSummaryTO> SelectAllTblStockSummary();
        String SelectLastStockUpdatedDateTime(Int32 compartmentId, Int32 prodCatId);
        TblStockSummaryTO SelectTblStockSummary(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran);
        TblStockSummaryTO SelectTblStockSummary(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null);
        TblStockSummaryTO SelectTblProdStockSummary(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null);
        TblStockSummaryTO SelectTblProdStockSummaryExist(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null);
        TblStockSummaryTO SelectTblStockSummaryExist(DateTime stocDate, SqlConnection conn = null, SqlTransaction tran = null);
        ODLMWebAPI.DashboardModels.StockUpdateInfo SelectDashboardStockUpdateInfo(DateTime sysDate);
        List<TblStockSummaryTO> ConvertDTToList(SqlDataReader tblStockSummaryTODT);
        int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO);
        int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockSummaryTO tblStockSummaryTO, SqlCommand cmdInsert);
        int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO);
        int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockSummaryTO tblStockSummaryTO, SqlCommand cmdUpdate);
        int DeleteTblStockSummary(Int32 idStockSummary);
        int DeleteTblStockSummary(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStockSummary, SqlCommand cmdDelete);
        StockSummaryTO GetLastStockSummaryDetails();

        Int64 GetLastIdStockSummary(SqlConnection conn = null, SqlTransaction tran = null);
        

    }
}