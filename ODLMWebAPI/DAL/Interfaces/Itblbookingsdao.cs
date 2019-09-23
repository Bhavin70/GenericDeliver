using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL.Interfaces
{  
    public interface ITblBookingsDAO
    {
        String SqlSelectQuery(Int32 loginUserId = 0);
        List<TblBookingsTO> SelectAllTblBookings();
        Double SelectTotalPendingBookingQty(DateTime sysDate);
        List<TblBookingsTO> SelectAllBookingsListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblBookingsTO> SelectAllBookingsListForApproval(Int32 isConfirmed, Int32 idBrand);
        List<TblBookingsTO> SelectAllBookingsListForAcceptance(Int32 cnfId, TblUserRoleTO tblUserRoleTO, Int32 isConfirmed);
        List<TblBookingsTO> SelectAllPendingBookingsList(Int32 cnfId, string dateCondOper, Boolean onlyPendingYn, int isTransporterScopeYn, int isConfirmed, DateTime asOnDate, int brandId, TblUserRoleTO tblUserRoleTO);
        List<TblBookingsTO> SelectTodayLoadedAndDeletedBookingsList(Int32 cnfId, DateTime asOnDate);
        List<TblBookingsTO> SelectAllTodaysBookingsWithOpeningBalance(Int32 cnfId, DateTime asOnDate, int isTransporterScopeYn, int isConfirmed, int brandId);
        List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords, Boolean pendingYn);
        List<TblBookingsTO> SelectAllBookingList(Int32 cnfId, Int32 dealerId, TblUserRoleTO tblUserRoleTO);
        List<TblBookingsTO> GetOrderwiseDealerList();
        List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, Int32 statusId, DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 confirm, Int32 isPendingQty, Int32 bookingId, Int32 isViewAllPendingEnq, Int32 RMId);
        List<TblBookingSummaryTO> SelectBookingSummaryList(Int32 typeId, Int32 masterId, DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId);
        List<TblBookingsTO> SelectUserwiseBookingList(DateTime fromDate, DateTime toDate, Int32 statusId, Int32 activeUserId);
        TblBookingsTO SelectTblBookings(Int32 idBooking);
        TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking);
        TblBookingsTO SelectTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        List<ODLMWebAPI.DashboardModels.BookingInfo> SelectBookingDashboardInfo(TblUserRoleTO tblUserRoleTO, int orgId, Int32 dealerId, DateTime date, string ids, Int32 isHideCorNC);
        List<TblBookingsTO> ConvertDTToList(SqlDataReader tblBookingsTODT);
        List<TblBookingSummaryTO> ConvertDTToListForBookingSummaryRpt(SqlDataReader tblBookingsSummaryTODT);
        Dictionary<Int32, Double> SelectBookingsPendingQtyDCT(SqlConnection conn, SqlTransaction tran);
        List<BookingGraphRptTO> SelectBookingListForGraph(Int32 OrganizationId, TblUserRoleTO tblUserRoleTO, Int32 dealerId);
        List<BookingGraphRptTO> ConvertDTToListForGraph(SqlDataReader tblBookingsGraphRptTODT);
        int InsertTblBookings(TblBookingsTO tblBookingsTO);
        int InsertTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingsTO tblBookingsTO, SqlCommand cmdInsert);
        int UpdateTblBookings(TblBookingsTO tblBookingsTO);
        int UpdateTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateBookingPendingQty(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingsTO tblBookingsTO, SqlCommand cmdUpdate);
        int DeleteTblBookings(Int32 idBooking);
        int DeleteTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBooking, SqlCommand cmdDelete);
        List<CnFWiseReportTO> SelectCnfCNCBookingReport(DateTime fromDate, DateTime toDate);
        List<TblBookingsTO> SelectAllBookingDateWise(DateTime fromDate, DateTime toDate);

        List<TblBookingsTO> SelectTblBookingsRef(Int32 BookingRefId, SqlConnection conn, SqlTransaction tran);
        TblBookingsTO SelectBookingsDetailsFromInVoiceId(Int32 invoiceId, SqlConnection conn, SqlTransaction tran);


    }
}