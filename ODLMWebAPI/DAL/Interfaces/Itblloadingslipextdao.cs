using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingSlipExtDAO
    {
        String SqlSelectQuery();
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt();
        List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(String loadingIds, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromBookingId(String BookingIds, SqlConnection conn, SqlTransaction tran);
        Dictionary<Int32, Double> SelectLoadingQuotaWiseApprovedLoadingQtyDCT(String loadingQuotaIds, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExt(Int32 prodCatId);
        List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExt(Int32 prodCatId, int prodSpecId);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt(Int32 loadingSlipId);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExt(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtByWeighingMeasureId(Int32 WeighingMeasureId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipExtTO SelectTblLoadingSlipExt(Int32 idLoadingSlipExt);
        TblLoadingSlipExtTO SelectTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> ConvertDTToList(SqlDataReader tblLoadingSlipExtTODT);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtByDate(DateTime frmDt, DateTime toDt, String statusStr,string selectedOrgStr);
        List<TblLoadingSlipExtTO> SelectTblLoadingSlipExtByIds(String idLoadingSlipExt, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO);
        int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdInsert);
        int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO);
        int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt);
        int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingSlipExt, SqlCommand cmdDelete);
       
        //Aniket [22-3-2019] added to get loadingSlipExt details against 
        List<TblLoadingSlipExtTO> GetAllLoadingExtByBookingId(int bookingId,string configval);
        //Aniket [13-8-2019]
        int UpdateLoadingSlipExtSeqNumber(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran);

    }
}