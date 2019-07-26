using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingQuotaConfigDAO
    {
        String SqlSelectQuery();
        List<TblLoadingQuotaConfigTO> SelectAllTblLoadingQuotaConfig();
        List<TblLoadingQuotaConfigTO> SelectLatestLoadingQuotaConfig(Int32 prodCatId, Int32 prodSpecId);
        List<TblLoadingQuotaConfigTO> SelectEmptyLoadingQuotaConfig(SqlConnection conn, SqlTransaction tran);
        TblLoadingQuotaConfigTO SelectTblLoadingQuotaConfig(Int32 idLoadQuotaConfig);
        List<TblLoadingQuotaConfigTO> ConvertDTToList(SqlDataReader tblLoadingQuotaConfigTODT);
        int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO);
        int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlCommand cmdInsert);
        int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO);
        int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran);
        int DeactivateLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig);
        int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idLoadQuotaConfig, SqlCommand cmdDelete);

    }
}