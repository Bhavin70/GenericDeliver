using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUnLoadingDAO
    {
        String SqlSelectQuery();
        List<TblUnLoadingTO> SelectAllTblUnLoading(DateTime startDate, DateTime endDate);
        TblUnLoadingTO SelectTblUnLoading(Int32 idUnLoading);
        List<TblUnLoadingTO> ConvertDTToList(SqlDataReader tblUnLoadingTODT);
        int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO);
        int InsertTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdInsert);
        int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO);
        int UpdateTblUnLoading(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdUpdate);
        int UpdateUnLoadingQty(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationQtyCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdUpdate);
        int DeactivateUnLoadingSlip(TblUnLoadingTO tblUnLoadingTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeactivateUnLoadingSlipCommand(TblUnLoadingTO tblUnLoadingTO, SqlCommand cmdUpdate);
        int DeleteTblUnLoading(Int32 idUnLoading);
        int DeleteTblUnLoading(Int32 idUnLoading, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUnLoading, SqlCommand cmdDelete);

    }
}