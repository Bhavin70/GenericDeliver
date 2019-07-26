using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSiteRequirementsDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblSiteRequirements();
        DataTable SelectTblSiteRequirements(Int32 idSiteRequirement);
        DataTable SelectAllTblSiteRequirements(SqlConnection conn, SqlTransaction tran);
        TblSiteRequirementsTO SelectSiteRequirements(Int32 visitId);
        List<TblSiteRequirementsTO> ConvertDTToList(SqlDataReader siteRequirementsDT);
        int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO);
        int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSiteRequirementsTO tblSiteRequirementsTO, SqlCommand cmdInsert);
        int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO);
        int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSiteRequirementsTO tblSiteRequirementsTO, SqlCommand cmdUpdate);
        int DeleteTblSiteRequirements(Int32 idSiteRequirement);
        int DeleteTblSiteRequirements(Int32 idSiteRequirement, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSiteRequirement, SqlCommand cmdDelete);

    }
}