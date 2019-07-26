using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUnLoadingBL
    {
        List<TblUnLoadingTO> SelectAllTblUnLoadingList(DateTime startDate, DateTime endDate);
        TblUnLoadingTO SelectTblUnLoadingTO(Int32 idUnLoading);
        int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO);
        int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewUnLoadingSlipDetails(TblUnLoadingTO tblUnLoadingTO);
        int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO);
        ResultMessage UpdateUnLoadingSlipDetails(TblUnLoadingTO tblUnLoadingTO);
        int UpdateUnloadingQuantity(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage DeactivateUnLoadingSlip(TblUnLoadingTO tblUnLoadingTO);
        int DeleteTblUnLoading(Int32 idUnLoading);
        int DeleteTblUnLoading(Int32 idUnLoading, SqlConnection conn, SqlTransaction tran);
    }
}