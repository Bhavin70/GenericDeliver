using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingDelAddrDAO
    {
        String SqlSelectQuery();
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddr();
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddr(int bookingId);
        TblBookingDelAddrTO SelectTblBookingDelAddr(Int32 idBookingDelAddr);
        List<TblBookingDelAddrTO> ConvertDTToList(SqlDataReader tblBookingDelAddrTODT);
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId);
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblBookingDelAddrTO> SelectTblBookingsByDealerOrgId(Int32 dealerOrgId, String txnAddrTypeIdtemp);
        int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO);
        int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingDelAddrTO tblBookingDelAddrTO, SqlCommand cmdInsert);
        int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO);
        int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingDelAddrTO tblBookingDelAddrTO, SqlCommand cmdUpdate);
        int DeleteTblBookingDelAddr(Int32 idBookingDelAddr);
        int DeleteTblBookingDelAddr(Int32 idBookingDelAddr, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBookingDelAddr, SqlCommand cmdDelete);
        int DeleteTblBookingDelAddrByScheduleId(Int32 scheduleId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommandByScheduleId(Int32 scheduleId, SqlCommand cmdDelete);

    }
}