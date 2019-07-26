using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingQuotaDeclarationBL
    {
        List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclarationList();
        List<TblLoadingQuotaDeclarationTO> SelectAllTblLoadingQuotaDeclarationList(DateTime declarationDate);
        Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, Int32 prodCatId, Int32 prodSpecId);
        Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate);
        Boolean IsLoadingQuotaDeclaredForTheDate(DateTime declarationDate, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectAvailableLoadingQuotaForCnf(int cnfId, DateTime declarationDate);
        List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 prodCatId, Int32 prodSpecId);
        List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 cnfOrgId, SqlConnection conn, SqlTransaction tran);
        TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 idLoadingQuota);
        TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran);
        TblLoadingQuotaDeclarationTO SelectTblLoadingQuotaDeclarationTO(Int32 cnfId, Int32 prodCatId, Int32 prodSpecId, Int32 materialId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectLoadingQuotaListForCnfAndDate(Int32 cnfOrgId, DateTime quotaDate, SqlConnection conn, SqlTransaction tran);
        List<TblLoadingQuotaDeclarationTO> SelectAllLoadingQuotaDeclListFromLoadingExt(String loadingSlipExtId, SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO);
        int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO);
        int UpdateTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota);
        int DeleteTblLoadingQuotaDeclaration(Int32 idLoadingQuota, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveLoadingQuotaDeclaration(List<TblLoadingQuotaDeclarationTO> loadingQuotaDeclarationTOList);
    }
}