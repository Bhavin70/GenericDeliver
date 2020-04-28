using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingSlipExtBL
    {
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList();
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList(int loadingSlipId);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtList(int loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtListByWeighingMeasureId(int WeighingMeasureId, SqlConnection conn, SqlTransaction tran);
        TblLoadingSlipExtTO SelectTblLoadingSlipExtTO(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExtList(Int32 prodCatId, int bookingId);
        List<TblBookingScheduleTO> SelectEmptyLoadingSlipExtListAgainstSch(Int32 prodCatId, Int32 prodSpecId, int bookingId, int brandId);
        List<TblLoadingSlipExtTO> SelectEmptyLoadingSlipExtList(Int32 prodCatId, Int32 prodSpecId, int bookingId, int brandId);
        List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(Int32 loadingId);
        List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromBookingId(Int32 bookingId);
        List<TblLoadingSlipExtTO> SelectAllLoadingSlipExtListFromLoadingId(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        Dictionary<Int32, Double> SelectLoadingQuotaWiseApprovedLoadingQtyDCT(String loadingQuotaIds, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectCnfWiseLoadingMaterialToPostPoneList(SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipExtTO> SelectAllTblLoadingSlipExtByDate(DateTime frmDt, DateTime toDt, String statusStr,string selectedOrgStr);
        List<TblLoadingSlipExtTO> SelectTblLoadingSlipExtByIds(String idLoadingSlipExt, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO);
        int InsertTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO);
        int UpdateTblLoadingSlipExt(TblLoadingSlipExtTO tblLoadingSlipExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt);
        int DeleteTblLoadingSlipExt(Int32 idLoadingSlipExt, SqlConnection conn, SqlTransaction tran);
    }
}