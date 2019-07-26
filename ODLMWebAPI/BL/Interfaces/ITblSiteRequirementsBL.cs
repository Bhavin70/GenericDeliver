using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSiteRequirementsBL
    {
        DataTable SelectAllTblSiteRequirements();
        List<TblSiteRequirementsTO> SelectAllTblSiteRequirementsList();
        TblSiteRequirementsTO SelectTblSiteRequirementsTO(Int32 idSiteRequirement);
        List<TblSiteRequirementsTO> ConvertDTToList(DataTable tblSiteRequirementsTODT);
        TblSiteRequirementsTO SelectSiteRequirementsTO(Int32 visitId);
        int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO);
        int InsertTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO);
        int UpdateTblSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateSiteRequirements(TblSiteRequirementsTO tblSiteRequirementsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSiteRequirements(Int32 idSiteRequirement);
        int DeleteTblSiteRequirements(Int32 idSiteRequirement, SqlConnection conn, SqlTransaction tran);
    }
}