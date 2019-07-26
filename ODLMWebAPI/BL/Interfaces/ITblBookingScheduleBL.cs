using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingScheduleBL
    {
        List<TblBookingScheduleTO> SelectAllTblBookingScheduleList();
        TblBookingScheduleTO SelectTblBookingScheduleTO(Int32 idSchedule);
        List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId);
        List<TblBookingScheduleTO> SelectAllTblBookingScheduleList(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
        //List<TblBookingScheduleTO> SelectBookingScheduleByBookingId(Int32 bookingId);
        int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO);
        int InsertTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO);
        int UpdateTblBookingSchedule(TblBookingScheduleTO tblBookingScheduleTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingSchedule(Int32 idSchedule);
        int DeleteTblBookingSchedule(Int32 idSchedule, SqlConnection conn, SqlTransaction tran);
    }
}
