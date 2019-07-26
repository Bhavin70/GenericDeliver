using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipDtlDAO
    { 
        String SqlSelectQuery();
        List<TblLoadingSlipDtlTO> SelectAllTblLoadingSlipDtl();
        TblLoadingSlipDtlTO SelectTblLoadingSlipDtl(Int32 idLoadSlipDtl);
        TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId);
        TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromLoadingId(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipDtlTO> ConvertDTToList(SqlDataReader tblLoadingSlipDtlTODT);
        Dictionary<int, Dictionary<int, Double>> SelectCnfAndDealerWiseLoadingQtyDCT(Int32 cnfId, DateTime loadingDate);
        Dictionary<int, Dictionary<int, Double>> SelectCnfAndDealerWiseLoadedAndBookingDeleteQtyDCT(Int32 cnfId, DateTime loadingDate);
        Dictionary<int, Double> SelectBookingWiseLoadingQtyDCT(DateTime loadingDate, Boolean isDeletedOnly, Int32 brandId);
        List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO);
        int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlCommand cmdInsert);
        int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO);
        int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl);
        int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipDtlNew(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadSlipDtl, SqlCommand cmdDelete);

    }
}