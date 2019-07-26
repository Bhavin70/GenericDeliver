using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblLoadingSlipDtlBL
    {
        List<TblLoadingSlipDtlTO> SelectAllTblLoadingSlipDtlList();
        TblLoadingSlipDtlTO SelectTblLoadingSlipDtlTO(Int32 idLoadSlipDtl);
        TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId);
        TblLoadingSlipDtlTO SelectLoadingSlipDtlTO(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromLoadingId(Int32 loadingId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingSlipDtlTO> SelectAllLoadingSlipDtlListFromBookingId(Int32 bookingId, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO);
        int InsertTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO);
        int UpdateTblLoadingSlipDtl(TblLoadingSlipDtlTO tblLoadingSlipDtlTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl);
        int DeleteTblLoadingSlipDtl(Int32 idLoadSlipDtl, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingSlipDtlNew(Int32 loadingSlipId, SqlConnection conn, SqlTransaction tran);
    }
}