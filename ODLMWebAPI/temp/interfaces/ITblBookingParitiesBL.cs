using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingParitiesBL
    {
        List<TblBookingParitiesTO> SelectAllTblBookingParitiesList();
        TblBookingParitiesTO SelectTblBookingParitiesTO(Int32 idBookingParity);
        List<TblBookingParitiesTO> SelectTblBookingParitiesTOListByBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
        int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO);
        int InsertTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO);
        int UpdateTblBookingParities(TblBookingParitiesTO tblBookingParitiesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingParities(Int32 idBookingParity);
        int DeleteTblBookingParities(Int32 idBookingParity, SqlConnection conn, SqlTransaction tran);
    }
}
