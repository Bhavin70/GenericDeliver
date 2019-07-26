using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingBeyondQuotaDAO
    {
        String SqlSelectQuery();
        List<TblBookingBeyondQuotaTO> SelectAllTblBookingBeyondQuota();
        List<TblBookingBeyondQuotaTO> SelectAllStatusHistoryOfBooking(Int32 bookingId);
        TblBookingBeyondQuotaTO SelectTblBookingBeyondQuota(Int32 idBookingAuth);
        List<TblBookingBeyondQuotaTO> ConvertDTToList(SqlDataReader tblBookingBeyondQuotaTODT);
        int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO);
        int InsertTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlCommand cmdInsert);
        int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO);
        int UpdateTblBookingBeyondQuota(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingBeyondQuotaTO tblBookingBeyondQuotaTO, SqlCommand cmdUpdate);
        int DeleteTblBookingBeyondQuota(Int32 idBookingAuth);
        int DeleteTblBookingBeyondQuota(Int32 idBookingAuth, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBookingAuth, SqlCommand cmdDelete);

    }
}