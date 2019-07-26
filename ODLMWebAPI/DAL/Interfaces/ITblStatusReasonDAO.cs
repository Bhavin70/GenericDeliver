using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStatusReasonDAO
    {
        String SqlSelectQuery();
        List<TblStatusReasonTO> SelectAllTblStatusReason();
        List<TblStatusReasonTO> SelectAllTblStatusReason(int statusId);
        TblStatusReasonTO SelectTblStatusReason(Int32 idStatusReason);
        List<TblStatusReasonTO> ConvertDTToList(SqlDataReader tblStatusReasonTODT);
        int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO);
        int InsertTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStatusReasonTO tblStatusReasonTO, SqlCommand cmdInsert);
        int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO);
        int UpdateTblStatusReason(TblStatusReasonTO tblStatusReasonTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStatusReasonTO tblStatusReasonTO, SqlCommand cmdUpdate);
        int DeleteTblStatusReason(Int32 idStatusReason);
        int DeleteTblStatusReason(Int32 idStatusReason, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStatusReason, SqlCommand cmdDelete);

    }
}