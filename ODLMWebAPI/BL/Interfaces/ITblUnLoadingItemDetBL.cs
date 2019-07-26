using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUnLoadingItemDetBL
    {
        List<TblUnLoadingItemDetTO> SelectAllUnLoadingItemDetailsList(int unLoadingId = 0);
        TblUnLoadingItemDetTO SelectTblUnLoadingItemDetTO(Int32 idUnloadingItemDet);
        int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO);
        int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO);
        int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet);
        int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet, SqlConnection conn, SqlTransaction tran);
    }
}