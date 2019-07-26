using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingQuotaConfigBL
    {
        List<TblLoadingQuotaConfigTO> SelectAllTblLoadingQuotaConfigList();
        TblLoadingQuotaConfigTO SelectTblLoadingQuotaConfigTO(Int32 idLoadQuotaConfig);
        List<TblLoadingQuotaConfigTO> SelectLatestLoadingQuotaConfigList(Int32 prodCatId, Int32 prodSpecId);
        List<TblLoadingQuotaConfigTO> SelectEmptyLoadingQuotaConfig(SqlConnection conn, SqlTransaction tran);
        int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO);
        int InsertTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewLoadingQuotaConfiguration(List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList);
        ResultMessage SaveNewLoadingQuotaConfiguration(List<TblLoadingQuotaConfigTO> loadingQuotaConfigTOList, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO);
        int UpdateTblLoadingQuotaConfig(TblLoadingQuotaConfigTO tblLoadingQuotaConfigTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig);
        int DeleteTblLoadingQuotaConfig(Int32 idLoadQuotaConfig, SqlConnection conn, SqlTransaction tran);
    }
}