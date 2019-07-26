using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingScheduleDAO
    {
        String SqlSelectQuery();
        List<TblBookingScheduleTO> SelectAllTblBookingSchedule();
        TblBookingScheduleTO SelectTblBookingSchedule(Int32 idSchedule);
        TblBookingScheduleTO SelectAllTblBookingSchedule(SqlConnection conn, SqlTransaction tran);
        List<TblBookingScheduleTO> ConvertDTToList(SqlDataReader tblBookingScheduleTODT);
        List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId);
        List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
        int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO);
        int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingScheduleTO tblBookingScheduleTO, SqlCommand cmdInsert);
        int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO);
        int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingScheduleTO tblBookingScheduleTO, SqlCommand cmdUpdate);
        int DeleteTblBookingSchedule(Int32 idSchedule);
        int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSchedule, SqlCommand cmdDelete);
    }
}