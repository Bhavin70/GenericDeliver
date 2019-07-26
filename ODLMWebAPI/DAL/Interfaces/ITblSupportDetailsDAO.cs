using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSupportDetailsDAO
    {
        String SqlSelectQuery();
        List<TblSupportDetailsTO> SelectAllTblSupportDetails();
        TblSupportDetailsTO SelectTblSupportDetails(Int32 idsupport);
        List<TblSupportDetailsTO> SelectAllTblSupportDetails(SqlConnection conn, SqlTransaction tran);
        List<TblSupportDetailsTO> ConvertDTToList(SqlDataReader tblSupportDetailsTODT);
        int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO);
        int InsertTblStopServiceHistory(TblLoginTO tblLoginTO);
        int InsertTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSupportDetailsTO tblSupportDetailsTO, SqlCommand cmdInsert);
        int InsertTblStopService(TblLoginTO tblLoginTO, SqlCommand cmdInsert);
        int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO);
        int UpdateTblSupportDetails(TblSupportDetailsTO tblSupportDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSupportDetailsTO tblSupportDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblSupportDetails(Int32 idsupport);
        int DeleteTblSupportDetails(Int32 idsupport, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idsupport, SqlCommand cmdDelete);

    }
}