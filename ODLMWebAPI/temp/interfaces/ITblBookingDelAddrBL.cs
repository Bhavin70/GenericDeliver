using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingDelAddrBL
    {
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList();
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrList(int bookingId);
        TblBookingDelAddrTO SelectTblBookingDelAddrTO(Int32 idBookingDelAddr);
        List<TblBookingDelAddrTO> SelectDeliveryAddrListFromDealer(Int32 addrSourceTypeId, Int32 entityId);
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId);
        List<TblBookingDelAddrTO> SelectAllTblBookingDelAddrListBySchedule(int scheduleId, SqlConnection conn, SqlTransaction tran);
        List<TblBookingDelAddrTO> SelectExistingBookingAddrListByDealerId(Int32 dealerOrgId, String txnAddrTypeId);
        int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO);
        int InsertTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO);
        int UpdateTblBookingDelAddr(TblBookingDelAddrTO tblBookingDelAddrTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingDelAddr(Int32 idBookingDelAddr);
        int DeleteTblBookingDelAddr(Int32 idBookingDelAddr, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingDelAddrByScheduleId(Int32 scheduleId, SqlConnection conn, SqlTransaction tran);
    }
}
