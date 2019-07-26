using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSiteStatusDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblSiteStatus();
        List<DropDownTO> SelectSiteStatusForDropDown();
        DataTable SelectTblSiteStatus(Int32 idSiteStatus);
        DataTable SelectAllTblSiteStatus(SqlConnection conn, SqlTransaction tran);
        int InsertTblSiteStatus(TblSiteStatusTO tblSiteStatusTO);
        int InsertTblSiteStatus(ref TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(ref TblSiteStatusTO tblSiteStatusTO, SqlCommand cmdInsert);
        int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO);
        int UpdateTblSiteStatus(TblSiteStatusTO tblSiteStatusTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSiteStatusTO tblSiteStatusTO, SqlCommand cmdUpdate);
        int DeleteTblSiteStatus(Int32 idSiteStatus);
        int DeleteTblSiteStatus(Int32 idSiteStatus, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSiteStatus, SqlCommand cmdDelete);

    }
}