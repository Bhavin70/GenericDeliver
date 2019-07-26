using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingBeyondQuotaBL
    {
        List<TblBookingBeyondQuotaTO> SelectAllTblBookingBeyondQuotaList();
        TblBookingBeyondQuotaTO SelectTblBookingBeyondQuotaTO(Int32 idBookingAuth);
        List<TblBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId);
        int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO);
        int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO);
        int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingBeyondQuota(Int32 idBookingAuth);
        int DeleteTblBookingBeyondQuota(Int32 idBookingAuth, SqlConnection conn, SqlTransaction tran);
    }
}
