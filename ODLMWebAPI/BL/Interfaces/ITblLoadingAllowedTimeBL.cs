using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingAllowedTimeBL
    {
        List<TblLoadingAllowedTimeTO> SelectAllTblLoadingAllowedTimeList();
        TblLoadingAllowedTimeTO SelectTblLoadingAllowedTimeTO(Int32 idLoadingTime);
        TblLoadingAllowedTimeTO SelectAllowedLoadingTimeTO(DateTime date);
        int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO);
        int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO);
        int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingAllowedTime(Int32 idLoadingTime);
        int DeleteTblLoadingAllowedTime(Int32 idLoadingTime, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveAllowedLoadingTime(TblLoadingAllowedTimeTO loadingAllowedTimeTO);
    }
}
