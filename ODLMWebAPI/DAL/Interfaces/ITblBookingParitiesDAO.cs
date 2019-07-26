using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingParitiesDAO
    {
        String SqlSelectQuery();
        List<TblBookingParitiesTO> SelectAllTblBookingParities();
        TblBookingParitiesTO SelectTblBookingParities(Int32 idBookingParity);
        List<TblBookingParitiesTO> SelectTblBookingParitiesByBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran);

        List<TblBookingParitiesTO> SelectTblBookingParitiesByBookingId(Int32 bookingId);
        List<TblBookingParitiesTO> ConvertDTToList(SqlDataReader tblBookingParitiesTODT);
        int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO);
        int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingParitiesTO tblBookingParitiesTO, SqlCommand cmdInsert);
        int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO);
        int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingParitiesTO tblBookingParitiesTO, SqlCommand cmdUpdate);
        int DeleteTblBookingParities(Int32 idBookingParity);
        int DeleteTblBookingParities(Int32 idBookingParity, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBookingParity, SqlCommand cmdDelete);

    }
}