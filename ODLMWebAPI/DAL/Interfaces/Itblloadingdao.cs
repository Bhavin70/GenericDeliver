using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface ITblLoadingDAO
    {
        String SqlSelectQuery();
        List<TblLoadingTO> SelectAllTblLoading();
        List<TblLoadingTO> SelectAllTblLoadingListForConvertNCToC();
        List<TblLoadingTO> SelectAllTblloadingList(DateTime fromDate, DateTime toDate,string selectedOrgStr);
        List<TblLoadingTO> SelectAllLoadingsFromParentLoadingId(Int32 parentLoadingId);
        List<TblLoadingTO> SelectAllLoadingListByStatus(string statusId, SqlConnection conn, SqlTransaction tran,Int32 gateId);
        List<TblLoadingTO> SelectAllTblLoading(TblUserRoleTO tblUserRoleTO, int cnfId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate, Int32 loadingTypeId, Int32 dealerId, string selectedOrgStr, Int32 isConfirm, Int32 brandId, Int32 loadingNavigateId, Int32 superwisorId);
        List<TblLoadingTO> SelectAllTblLoadingLinkList(TblUserRoleTO tblUserRoleTO, int dearlerOrgId, Int32 loadingStatusId, DateTime fromDate, DateTime toDate);
        List<TblLoadingTO> SelectAllTblLoading(int cnfId, String loadingStatusIdIn, DateTime loadingDate);
        TblLoadingTO SelectTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran);
        TblLoadingTO SelectTblLoadingByLoadingSlipId(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        Int64 SelectCountOfLoadingSlips(DateTime date, SqlConnection conn, SqlTransaction tran);
        ODLMWebAPI.DashboardModels.LoadingInfo SelectDashboardLoadingInfo(TblUserRoleTO tblUserRoleTO, Int32 orgId, DateTime sysDate,Int32 categoryType, Int32 loadingType);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, DateTime loadingDate);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo);
        List<TblLoadingTO> SelectLoadingTOWithDetailsByLoadingNoForSupport(string loadingSlipNo);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNo(string vehicleNo, bool isAllowNxtLoading, int loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(string vehicleNo, SqlConnection conn, SqlTransaction tran);
        //Aniket [19-8-2019]
        List<TblLoadingTO> SelectAllLoadingListByVehicleNoForDelOut(int loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> SelectAllInLoadingListByVehicleNo(string vehicleNo);
        Dictionary<Int32, Int32> SelectCountOfLoadingsOfSuperwisor(DateTime date, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingTO> ConvertDTToList(SqlDataReader tblLoadingTODT);
        List<VehicleNumber> SelectAllVehicles();
        List<DropDownTO> SelectAllVehiclesListByStatus(int statusId);
        List<TblLoadingTO> SelectAllTempLoading(SqlConnection conn, SqlTransaction tran, DateTime migrateBeforeDate);
        List<TblLoadingTO> SelectAllTempLoadingOnStatus(SqlConnection conn, SqlTransaction tran, DateTime migrateBeforeDate);
        List<TblLoadingTO> SelectLoadingListByVehicleNo(string vehicleNo);
        int InsertTblLoading(TblLoadingTO tblLoadingTO);
        int UpdateTblLoadingIgnoreGrossWTFlag(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdInsert);
        int UpdateTblLoading(TblLoadingTO tblLoadingTO);
        int UpdateTblLoadingWeighingDetails(int loadingId, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoading(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        int updateLaodingToCallFlag(TblLoadingTO tblLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingTO tblLoadingTO, SqlCommand cmdUpdate);
        int DeleteTblLoading(Int32 idLoading);
        int DeleteTblLoading(Int32 idLoading, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoading, SqlCommand cmdDelete);

        //Aniket[30-7-2019] added for IOT
        List<int> GeModRefMaxData();
        tblUserMachineMappingTo SelectUserMachineTo(int userId, SqlConnection conn, SqlTransaction tran);
        TblLoadingTO SelectTblLoading(Int32 idLoading);
        TblLoadingTO SelectTblLoadingTOByModBusRefId(Int32 modBusRefId, SqlConnection conn, SqlTransaction tran);

        List<TblLoadingTO> GetPendingBookingQtyList(DateTime startDate);

    }
}