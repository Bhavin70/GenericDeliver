using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingExtDAO
    {
        String SqlSelectQuery();
        List<TblBookingExtTO> SelectAllTblBookingExt();
        List<TblBookingExtTO> SelectAllTblBookingExt(int bookingId);
        List<TblBookingExtTO> SelectAllTblBookingExt(int bookingId, SqlConnection conn, SqlTransaction trans);
        List<TblBookingExtTO> SelectEmptyBookingExtList(int prodCatgId, Int32 prodSpecId);
        List<TblBookingExtTO> SelectBookingDetails(Int32 dealerId);
        TblBookingExtTO SelectTblBookingExt(Int32 idBookingExt);
        List<TblBookingExtTO> ConvertDTToList(SqlDataReader tblBookingExtTODT);
        List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(int scheduleId);
        List<TblBookingExtTO> SelectAllTblBookingExtListBySchedule(int scheduleId, SqlConnection conn, SqlTransaction trans);
        int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO);
        int InsertTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingExtTO tblBookingExtTO, SqlCommand cmdInsert);
        int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO);
        int UpdateTblBookingExt(TblBookingExtTO tblBookingExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingExtTO tblBookingExtTO, SqlCommand cmdUpdate);
        int DeleteTblBookingExt(Int32 idBookingExt);
        int DeleteTblBookingExt(Int32 idBookingExt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBookingExt, SqlCommand cmdDelete);
        int DeleteTblBookingExtBySchedule(Int32 scheduleId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByScheduleId(Int32 scheduleId, SqlCommand cmdDelete);

    }
}