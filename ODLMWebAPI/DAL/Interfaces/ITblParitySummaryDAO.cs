using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblParitySummaryDAO
    {
        String SqlSelectQuery();
        List<TblParitySummaryTO> SelectAllTblParitySummary();
        TblParitySummaryTO SelectTblParitySummary(Int32 idParity, SqlConnection conn, SqlTransaction tran);
        TblParitySummaryTO SelectParitySummaryFromParityDtlId(Int32 parityDtlId, SqlConnection conn, SqlTransaction tran);
        TblParitySummaryTO SelectStatesActiveParitySummary(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblParitySummaryTO> SelectActiveParitySummaryTOList(int dealerId, SqlConnection conn, SqlTransaction tran);
        List<TblParitySummaryTO> ConvertDTToList(SqlDataReader tblParitySummaryTODT);
        int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO);
        int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblParitySummaryTO tblParitySummaryTO, SqlCommand cmdInsert);
        int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO);
        int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllParitySummary(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblParitySummaryTO tblParitySummaryTO, SqlCommand cmdUpdate);
        int DeleteTblParitySummary(Int32 idParity);
        int DeleteTblParitySummary(Int32 idParity, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idParity, SqlCommand cmdDelete);

    }
}