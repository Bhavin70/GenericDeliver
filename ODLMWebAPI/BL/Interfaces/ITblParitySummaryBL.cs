using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblParitySummaryBL
    {
        List<TblParitySummaryTO> SelectAllTblParitySummaryList();
        TblParitySummaryTO SelectTblParitySummaryTO(Int32 idParity, SqlConnection conn, SqlTransaction tran);
        TblParitySummaryTO SelectParitySummaryTOFromParityDtlId(Int32 parityDtlId, SqlConnection conn, SqlTransaction tran);
        TblParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId, Int32 brandId);
        TblParitySummaryTO SelectStatesActiveParitySummaryTO(Int32 stateId, Int32 brandId, SqlConnection conn, SqlTransaction tran);
        List<TblParitySummaryTO> SelectActiveParitySummaryTOList(int dealerId, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveParitySettings(TblParitySummaryTO tblParitySummaryTO);
        int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO);
        int InsertTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);
        Int32 MigrateOldParityWithAllSpecifications();
        int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO);
        int UpdateTblParitySummary(TblParitySummaryTO tblParitySummaryTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblParitySummary(Int32 idParity);
        int DeleteTblParitySummary(Int32 idParity, SqlConnection conn, SqlTransaction tran);
    }
}