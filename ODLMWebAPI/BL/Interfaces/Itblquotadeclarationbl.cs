using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblQuotaDeclarationBL
    {
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList();
        List<TblQuotaDeclarationTO> SelectAllTblQuotaDeclarationList(Int32 globalRateId);
        TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration);
        TblQuotaDeclarationTO SelectPreviousTblQuotaDeclarationTO(Int32 idQuotaDeclaration, Int32 cnfOrgId);
        TblQuotaDeclarationTO SelectTblQuotaDeclarationTO(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        TblQuotaDeclarationTO SelectLatestQuotaDeclarationTO(SqlConnection conn, SqlTransaction tran);
        List<TblQuotaDeclarationTO> SelectLatestQuotaDeclaration(Int32 cnfId, DateTime date, Boolean isQuotaDeclaration, Int32 categoryType);
        List<TblQuotaDeclarationTO> SelectLatestQuotaDeclarationTOList(Int32 cnfId, DateTime date,Int32 categoryType = 1);
        ODLMWebAPI.DashboardModels.QuotaAndRateInfo SelectQuotaAndRateDashboardInfo(Int32 roletypeId, Int32 orgId, DateTime sysDate);
        List<ODLMWebAPI.DashboardModels.QuotaAndRateInfo> SelectQuotaAndRateDashboardInfoList(Int32 roletypeId, Int32 orgId, DateTime sysDate,Int32 categoryType);
        Boolean CheckForValidityAndReset(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int InsertTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int SaveDeclaredRateAndAllocatedQuota(List<TblQuotaDeclarationTO> quotaExtList, List<TblQuotaDeclarationTO> quotaList, TblGlobalRateTO tblGlobalRateTO);
        ResultMessage SaveDeclaredRateAndAllocatedBand(List<TblGlobalRateTO> tblGlobalRateTOList);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO);
        int UpdateTblQuotaDeclaration(TblQuotaDeclarationTO tblQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration);
        int DeleteTblQuotaDeclaration(Int32 idQuotaDeclaration, SqlConnection conn, SqlTransaction tran);
        //Aniket [30-9-2019]
        TblQuotaDeclarationTO GetBookingQuotaAgainstCNF(Int32 cnfOrgId, Int32 brandId);
        List<TblQuotaDeclarationTO> GetLatestRateInfo(Int32 cnfId, DateTime date, Boolean isQuotaDeclaration, Int32 categoryType);


    }
}