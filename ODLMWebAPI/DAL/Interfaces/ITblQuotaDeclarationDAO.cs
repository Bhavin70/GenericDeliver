using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblQuotaDeclarationDAO
    {
        String SqlSelectQuery();
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclaration();
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclaration(Int32 globalRateId);
        List<TblQuotaDeclarationTO> ConvertDTToList(SqlDataReader tblQuotaDeclarationTODT);
        TblQuotaDeclarationTO SelectTblQuotaDeclaration(Int32 idQuotaDeclaration);
        TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration, Int32 cnfOrgId);
        TblQuotaDeclarationTO SelectTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran);
        List<TblQuotaDeclarationTO> SelectLatestQuotaDeclaration(Int32 orgId, DateTime date, Boolean isQuotaDeclaration);
        ODLMWebAPI.DashboardModels.QuotaAndRateInfo SelectDashboardQuotaAndRateInfo(Int32 roletypeId, Int32 orgId, DateTime sysDate);
        List<ODLMWebAPI.DashboardModels.QuotaAndRateInfo> SelectDashboardQuotaAndRateInfoList(Int32 roleId, Int32 orgId, DateTime sysDate);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlCommand cmdInsert);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllDeclaredQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran);
        int UpdateQuotaDeclarationValidity(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlCommand cmdUpdate);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idQuotaDeclaration, SqlCommand cmdDelete);
        TblQuotaDeclarationTO GetBookingQuotaAgainstCNF(Int32 cnfOrgId, Int32 brandId);

    }
}