using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUnLoadingItemDetDAO
    {
        String SqlSelectQuery();
        List<TblUnLoadingItemDetTO> SelectAllTblUnLoadingItemDetails(int unLoadingId = 0);
        TblUnLoadingItemDetTO SelectTblUnLoadingItemDet(Int32 idUnloadingItemDet);
        List<TblUnLoadingItemDetTO> SelectAllUnLoadingItemDetails(Int32 unLoadingId);
        List<TblUnLoadingItemDetTO> ConvertDTToList(SqlDataReader tblUnLoadingItemDetTODT);
        int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO);
        int InsertTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlCommand cmdInsert);
        int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO);
        int UpdateTblUnLoadingItemDet(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUnLoadingItemDetTO tblUnLoadingItemDetTO, SqlCommand cmdUpdate);
        int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet);
        int DeleteTblUnLoadingItemDet(Int32 idUnloadingItemDet, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUnloadingItemDet, SqlCommand cmdDelete);

    }
}