using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblRunningSizesBL
    {  
        List<TblRunningSizesTO> SelectAllTblRunningSizesList();
        TblRunningSizesTO SelectTblRunningSizesTO(Int32 idRunningSize);
        int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO);
        int InsertTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO);
        int UpdateTblRunningSizes(TblRunningSizesTO tblRunningSizesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblRunningSizes(Int32 idRunningSize);
        int DeleteTblRunningSizes(Int32 idRunningSize, SqlConnection conn, SqlTransaction tran);
        List<TblRunningSizesTO> SelectAllTblRunningSizesList(DateTime stockDate);
        ResultMessage SaveDailyRunningSizeInfo(List<TblRunningSizesTO> runningSizesTOList, DateTime stockDate);
        ResultMessage RemoveRunningSizeDtls(TblRunningSizesTO runningSizesTO, TblStockSummaryTO tblStockSummaryTO, Int32 userId);
    }
}