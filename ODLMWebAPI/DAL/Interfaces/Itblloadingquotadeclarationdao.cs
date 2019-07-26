using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingQuotaDeclarationDAO
    {
        String SqlSelectQuery();
        List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclaration();
        List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclaration(DateTime declarationDate);
        Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, Int32 prodCatId, Int32 prodSpecId);
        Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclaration(int cnfId, DateTime declarationDate);
        List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId);
        List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 cnfId, SqlConnection conn, SqlTransaction tran);
        TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran);
        TblLoadingQuotaDeclarationTO SelectLoadingQuotaDeclarationTO(Int32 cnfId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectLoadingQuotaDeclaredForCnfAndDate(Int32 cnfOrgId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectAllLoadingQuotaDeclListFromLoadingExt(String loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> ConvertDTToList(SqlDataReader tblLoadingQuotaDeclarationTODT);
        int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO);
        int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlCommand cmdInsert);
        int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO);
        int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllPrevLoadingQuota(Int32 updatedBy, int prodCatId, Int32 prodSpecId, SqlConnection conn, SqlTransaction tran);
        int DeactivateAllPrevLoadingQuota(Int32 updatedBy, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota);
        int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadingQuota, SqlCommand cmdDelete);

    }
}