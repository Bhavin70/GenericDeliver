using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static ODLMWebAPI.StaticStuff.Constants;

namespace ODLMWebAPI.BL.Interfaces
{  

    public interface ITblBookingsBL
    { 
        List<TblBookingsTO> SelectAllTblBookingsList();
        List<TblBookingsTO> SelectAllBookingsListFromLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblBookingsTO> SelectAllBookingsListForApproval(Int32 isConfirmed, Int32 idBrand);
        List<PendingQtyOrderTypeTo> SelectTotalPendingBookingQty(DateTime sysDate);
        void AssignOverDueAmount(List<TblBookingsTO> tblBookingsTOList);
        ResultMessage GetBookingAvgQtyDetailsStatus(int dealerOrgId, Int32 bookingId);

        List<TblBookingsTO> SelectAllBookingsListForAcceptance(Int32 cnfId, List<TblUserRoleTO> userRoleTOList, Int32 isConfirmed);
        List<TblBookingsTO> SelectAllLatestBookingOfDealer(Int32 dealerId, Int32 lastNRecords, Int32 bookingId);
        List<TblBookingsTO> SelectAllBookingList(Int32 cnfId, Int32 dealerId, List<TblUserRoleTO> tblUserRoleTOList);
        List<TblBookingsTO> GetOrderwiseDealerList();
        List<TblBookingsTO> SelectBookingList(Int32 cnfId, Int32 dealerId, Int32 statusId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 confirm, Int32 isPendingQty, Int32 bookingId, Int32 isViewAllPendingEnq, Int32 RMId,Int32 orderTypeId=0,Boolean isFromEnquiryReport=false,Int32 categoryType=1);
        List<TblBookingSummaryTO> SelectBookingSummaryList(Int32 typeId, Int32 masterId, DateTime fromDate, DateTime toDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId);
        TblBookingsTO SelectTblBookingsTO(Int32 idBooking);
        List<TblBookingsTO> SelectUserwiseBookingList(DateTime fromDate, DateTime toDate, Int32 statusId, Int32 activeUserId);
        //TblBookingsTO SelectBookingsTOWithDetails(Int32 idBooking);
        List<ODLMWebAPI.DashboardModels.BookingInfo> SelectBookingDashboardInfo(List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, Int32 dealerId, DateTime date,Int32 categoryType);
        TblBookingsTO SelectTblBookingsTO(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        List<PendingBookingRptTO> SelectAllPendingBookingsForReport(Int32 cnfId, Int32 dealerOrgId, List<TblUserRoleTO> tblUserRoleTOList, int isTransporterScopeYn, int isConfirmed, DateTime fromDate, DateTime toDate, Boolean isDateFilter, Int32 brandId);
        List<BookingGraphRptTO> SelectBookingListForGraph(Int32 OrganizationId, List<TblUserRoleTO> userRoleTOList, Int32 dealerId);
        int InsertTblBookings(TblBookingsTO tblBookingsTO);
        int InsertTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewBooking(TblBookingsTO tblBookingsTO);
        int UpdateTblBookings(TblBookingsTO tblBookingsTO);
        int UpdateTblBookings(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateBookingPendingQty(TblBookingsTO tblBookingsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateBookingConfirmations(TblBookingsTO tblBookingsTO);
        ResultMessage SendNotification(TblBookingsTO tblBookingsTO, Boolean isCnfAcceptDirectly, Boolean isFromNewBooking, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateBooking(TblBookingsTO tblBookingsTO);
        ResultMessage PostUpdateOverdueExistOrNotFromBooking(TblBookingsTO bookingTo, Int32 loginUserId);
        Int32 MigrateBookingSizesQty();
        int DeleteTblBookings(Int32 idBooking);
        int DeleteTblBookings(Int32 idBooking, SqlConnection conn, SqlTransaction tran);
        List<CnFWiseReportTO> SelectCnfCNCBookingReport(DateTime fromDate, DateTime toDate);
        List<TblOrganizationTO> SelectSalesAgentListWithBrandAndRate();
        List<TblBookingPendingRptTO> SelectBookingPendingQryRpt(DateTime fromDate, DateTime toDate,int reportType);
        List<TblBookingPendingRptTO> SelectBookingPendingOrderQtyRpt(DateTime fromDate, DateTime toDate, int reportType);

        TblBookingsTO SelectBookingsDetailsFromInVoiceId(Int32 inInvoice, SqlConnection conn, SqlTransaction tran);

        TblBookingsTO SelectBookingsDetailsFromInVoiceId(Int32 inInvoice);
        ResultMessage UpdatePendingQuantity(TblBookingQtyConsumptionTO tblBookingQtyConsumptionTO);

        ResultMessage SendBookingDueNotification();
        List<TblBookingAnalysisReportTO> GetBookingAnalysisReport(DateTime startDate, DateTime endDate, int distributorId, int cOrNcId, int brandId, int skipDate, int isFromProject, int stateId , int districtId );

        //ResultMessage DeleteAllBookings(List<Int32> bookingsIdList);
        //ResultMessage DeleteAllBookings(List<int> bookingsIdsList, SqlConnection conn, SqlTransaction tran);
        //int DeleteDispatchBookingData(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
    }
}
