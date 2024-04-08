using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{  
    public interface ITblStockSummaryBL
    { 
        List<TblStockSummaryTO> SelectAllTblStockSummaryList();
        String SelectLastStockUpdatedDateTime(Int32 compartmentId, Int32 prodCatId);
        TblStockSummaryTO SelectTblStockSummaryTO(Int32 idStockSummary);
        TblStockSummaryTO SelectTblStockSummaryTO(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran);
        TblStockSummaryTO SelectTblStockSummaryTO(DateTime stocDate);
        ODLMWebAPI.DashboardModels.StockUpdateInfo SelectDashboardStockUpdateInfo(DateTime sysDate,Int32 categoryType);
        int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO);
        int InsertTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage ResetStockDetails();
        ResultMessage UpdateDailyStock(TblStockSummaryTO tblStockSummaryTO);
        int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO);
        int UpdateTblStockSummary(TblStockSummaryTO tblStockSummaryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStockSummary(Int32 idStockSummary);
        int DeleteTblStockSummary(Int32 idStockSummary, SqlConnection conn, SqlTransaction tran);
        ResultMessage ConfirmStockSummary(List<SizeSpecWiseStockTO> sizeSpecWiseStockTOList);
        StockSummaryTO GetLastStockSummaryDetails();
        StockSummaryTO GetTodaysStockSummaryDetails();
        
    }
}