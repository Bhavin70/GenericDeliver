using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipDAO
    {
        String SqlSelectQuery();
        List<TblLoadingSlipTO> SelectAllTblLoadingSlip();
        List<TblLoadingSlipTO> SelectAllTblLoadingSlip(int loadingId);
        List<TblLoadingSlipTO> SelectAllTblLoadingSlip(int loadingId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipTO SelectTblLoadingSlip(Int32 idLoadingSlip);
        TblLoadingSlipTO SelectTblLoadingSlip(int idLoadingSlip, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectRegMobileNoDCTForLoadingDealers(String loadingIds, SqlConnection conn, SqlTransaction tran);
        Int64 SelectCountOfLoadingSlips(DateTime date, Int32 isConfirmed, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipTO> SelectAllTblLoadingSlipByDate(DateTime fromDate, DateTime toDate, TblUserRoleTO tblUserRoleTO, Int32 cnfId);
        List<TblLoadingSlipTO> SelectAllLoadingSlipListByVehicleNo(string vehicleNo);
        List<TblLoadingSlipTO> SelectTblLoadingTOByLoadingSlipIdForSupport(String loadingSlipNo);
        List<TblLoadingSlipTO> ConvertDTToList(SqlDataReader tblLoadingSlipTODT);
        List<TblLoadingSlipTO> SelectAllTblLoadingSlipListByloadingId(string idLoadingSlip);
        List<TblLoadingSlipTO> SelectAllLoadingSlipListByVehicleNo(string vehicleNo, DateTime fromDate, DateTime toDate);
        List<TblLoadingSlipTO> SelectAllNotifiedTblLoadingList(DateTime fromDate, DateTime toDate, Int32 callFlag);
        List<TblLoadingSlipTO> SelectAllTblLoadingSlipList(TblUserRoleTO tblUserRoleTO, int cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, Int32 isConfirm, Int32 brandId, Int32 superwisorId);
        List<TblORCReportTO> SelectORCReportDetailsList(DateTime fromDate, DateTime toDate, Int32 flag);
        List<TblORCReportTO> ConvertDTToListForORC(SqlDataReader tblORCReportTODT);
        int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO);
        int InsertTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdInsert);
        int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO);
        int UpdateTblLoadingSlip(TblLoadingSlipTO tblLoadingSlipTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlip(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingSlipTO tblLoadingSlipTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingSlip(Int32 idLoadingSlip);
        int DeleteTblLoadingSlip(Int32 idLoadingSlip, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingSlip, SqlCommand cmdDelete);

        

    }
}