using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingSlipBL
    {
        List<TblLoadingSlipTO> SelectAllTblLoadingSlipList();
        List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId);
        //List<TblLoadingSlipTO> SelectAllLoadingSlipListWithDetails(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipTO SelectAllLoadingSlipWithDetails(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipTO SelectTblLoadingSlipTO(Int32 idLoadingSlip);
        Dictionary<int, string> SelectRegMobileNoDCTForLoadingDealers(String loadingIds, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipTO SelectAllLoadingSlipWithDetails(Int32 loadingSlipId);
        List<TblLoadingSlipTO> SelectAllLoadingSlipList(List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm, Int32 brandId, Int32 superwisorId);
        TblLoadingSlipTO SelectAllLoadingSlipWithDetailsByInvoice(Int32 invoiceId);
        List<TblLoadingSlipTO> SelectAllTblLoadingSlipByDate(DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId);
        List<TblLoadingSlipTO> SelectAllLoadingCycleList(DateTime startDate, DateTime endDate, List<TblUserRoleTO> tblUserRoleTOList, Int32 cnfId, Int32 vehicleStatus);
        List<TblLoadingSlipTO> SelectAllLoadingListByVehicleNo(string vehicleNo);
        List<TblLoadingSlipTO> SelectLoadingTOWithDetailsByLoadingSlipIdForSupport(String loadingSlipNo);
        List<TblLoadingSlipTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime startDate, DateTime endDate);
        List<TblLoadingSlipTO> SelectAllTblLoadingSlip(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipTO> SelectAllNotifiedTblLoadingList(DateTime fromDate, DateTime toDate, Int32 callFlag);
        List<TblORCReportTO> SelectORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag);
        int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO);
        int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO);
        int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlip(TblLoadingTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId);
        ResultMessage ChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId, SqlConnection conn, SqlTransaction tran);
        ResultMessage OldChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId);
        ResultMessage OldChangeLoadingSlipConfirmationStatus(TblLoadingSlipTO tblLoadingSlipTO, Int32 loginUserId, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlip(Int32 idLoadingSlip);
        int DeleteTblLoadingSlip(Int32 idLoadingSlip, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipTO SelectAllLoadingSlipWithDetailsForExtract(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipTO SelectAllLoadingSlipWithDetailsForExtract(Int32 loadingSlipId);
    }
}