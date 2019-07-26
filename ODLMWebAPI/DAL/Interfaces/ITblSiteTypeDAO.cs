using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSiteTypeDAO
    {
        String SqlSelectQuery();
        List<TblSiteTypeTO> SelectAllTblSiteTypeList();
        DataTable SelectTblSiteType(Int32 idSiteType);
        DataTable SelectAllTblSiteType(SqlConnection conn, SqlTransaction tran);
        List<TblSiteTypeTO> ConvertDTToList(SqlDataReader tblSiteTypeTODT);
        int InsertTblSiteType(TblSiteTypeTO tblSiteTypeTO);
        int InsertTblSiteType(ref TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(ref TblSiteTypeTO tblSiteTypeTO, SqlCommand cmdInsert);
        int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO);
        int UpdateTblSiteType(TblSiteTypeTO tblSiteTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSiteTypeTO tblSiteTypeTO, SqlCommand cmdUpdate);
        int DeleteTblSiteType(Int32 idSiteType);
        int DeleteTblSiteType(Int32 idSiteType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSiteType, SqlCommand cmdDelete);

    }
}