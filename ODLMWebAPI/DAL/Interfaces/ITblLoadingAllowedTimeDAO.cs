using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingAllowedTimeDAO
    {
        String SqlSelectQuery();
        List<TblLoadingAllowedTimeTO> SelectAllTblLoadingAllowedTime();
        TblLoadingAllowedTimeTO SelectTblLoadingAllowedTime(Int32 idLoadingTime);
        TblLoadingAllowedTimeTO SelectTblLoadingAllowedTime(DateTime date);
        List<TblLoadingAllowedTimeTO> ConvertDTToList(SqlDataReader tblLoadingAllowedTimeTODT);
        int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO);
        int InsertTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlCommand cmdInsert);
        int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO);
        int UpdateTblLoadingAllowedTime(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingAllowedTimeTO tblLoadingAllowedTimeTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingAllowedTime(Int32 idLoadingTime);
        int DeleteTblLoadingAllowedTime(Int32 idLoadingTime, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingTime, SqlCommand cmdDelete);

    }
}