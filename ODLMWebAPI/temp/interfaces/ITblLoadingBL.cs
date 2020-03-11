﻿using ODLMWebAPI.DashboardModels;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingBL
    {
        List<TblLoadingTO> SelectAllTblLoadingList();
        List<TblLoadingTO> SelectAllTblLoadingListForConvertNCToC();
        List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId(int parentLoadingId);
        List<TblLoadingTO> SelectAllTblloadingList(DateTime fromDate, DateTime toDate);
        List<TblLoadingTO> SelectAllTblLoadingList(List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId, Int32 superwisorId);
        List<TblLoadingTO> GetLoadingDetailsForReport(DateTime fromDate, DateTime toDate);
        List<TblLoadingTO> SelectAllTblLoadingLinkList(List<TblUserRoleTO> tblUserRoleTOList, Int32 dearlerOrgId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate);
        List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId);
        TblLoadingTO SelectTblLoadingTO(Int32 idLoading, SqlConnection conn, SqlTransaction tran);
        TblLoadingTO SelectTblLoadingTO(Int32 idLoading);
        TblLoadingTO SelectTblLoadingTOByLoadingSlipId(Int32 loadingSlipId);
        List<TblLoadingTO> SelectLoadingTOListWithDetails(string idLoadings);
        TblLoadingTO SelectLoadingTOWithDetails(Int32 idLoading);
        TblLoadingTO SelectLoadingTOWithDetailsByInvoiceId(Int32 invoiceId);
        TblLoadingTO SelectLoadingTOWithDetailsByLoadingSlipId(Int32 loadingSlipId);
        TblLoadingTO SelectTblLoadingByLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<VehicleNumber> SelectAllVehicles();
        List<DropDownTO> SelectAllVehiclesByStatus(int statusId);
        LoadingInfo SelectDashboardLoadingInfo(List<TblUserRoleTO> tblUserRoleTOList, Int32 orgId, DateTime sysDate, Int32 loadingType);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime loadingDate);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(string vehicleNo, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> SelectAllInLoadingListByVehicleNo(string vehicleNo);
        Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisorDCT(DateTime date);
        List<TblLoadingTO> SelectAllTblLoading(int cnfId, String loadingStatusIdIn, DateTime loadingDate);
        List<TblLoadingTO> SelectAllTempLoading(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> SelectAllTempLoadingOnStatus(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> SelectLoadingListByVehicleNo(string vehicleNo);
        List<TblLoadingTO> SelectAllTblLoadingByBookingId(Int32 bookingId);
        TblLoadingTO SelectLoadingTOWithDetailsByBooking(String tempBookingsIdsList, String tempScheduleIdsList);
        int InsertTblLoading(TblLoadingTO tblLoadingTO);
        int InsertTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage CalculateLoadingValuesRate(TblLoadingTO tblLoadingTO);
        void CalculateActualPriceInclusiveOfTaxes();
        ResultMessage UpdateNCToCLoadingSlip(Int32 loginUserId);
        ResultMessage SaveNewLoadingSlip(TblLoadingTO tblLoadingTO);
        ResultMessage UpdateStockAndConsumptionHistory(TblLoadingSlipExtTO tblLoadingSlipExtTO, TblLoadingTO tblLoadingTO, int stockDtlId, ref Double totalLoadingQty, TblProductInfoTO prodConfgTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoading(TblLoadingTO tblLoadingTO);
        int UpdateTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateDeliverySlipConfirmations(TblLoadingTO tblLoadingTO);
        ResultMessage UpdateDeliverySlipConfirmations(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage InsertLoadingVehDocExtDetailsAgainstLoading(TblLoadingTO tblLoadingTO);
        ResultMessage RestorePreviousStatusForLoading(TblLoadingTO tblLoadingTO);
        ResultMessage CancelAllNotConfirmedLoadingSlips();
        ResultMessage CanGivenLoadingSlipBeApproved(TblLoadingTO tblLoadingTO);
        ResultMessage CanGivenLoadingSlipBeApproved(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage updateLaodingToCallFlag(TblLoadingTO tblLoadingTO);
        ResultMessage IsVehicleWaitingForGross(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        TblWeighingMeasuresTO getWeighingGrossTo(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage removeIsAllowOneMoreLoading(TblLoadingTO tblLoadingTO, int loginUserId);
        ResultMessage UpdateLoadingTransportDetails(TblTransportSlipTO tblTransportSlipTO);
        ResultMessage UpdateVehicleDetails(TblLoadingTO LoadingTO);
        ResultMessage AllocateSuperwisor(TblLoadingTO tblLoadingTO, string loginUserId);
        int DeleteTblLoading(Int32 idLoading);
        int DeleteTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran);
        ResultMessage RemoveItemFromLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId);
        ResultMessage RemoveItemFromLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 isForUpdate, Int32 txnUserId, SqlConnection conn, SqlTransaction tran);
        ResultMessage AddItemInLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId);
        ResultMessage AddItemInLoadingSlip(TblLoadingSlipExtTO tblLoadingSlipExtTO, Int32 txnUserId, Int32 isForUpdate, SqlConnection conn, SqlTransaction tran);
        ResultMessage ReverseWeighingProcessAgainstLoading(Int32 loadingId, Int32 txnUserId);
        ResultMessage ReverseWeighingProcessAgainstLoading(Int32 loadingId, Int32 txnUserId, SqlConnection conn, SqlTransaction tran);
    }
}