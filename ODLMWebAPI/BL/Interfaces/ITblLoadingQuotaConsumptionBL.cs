using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingQuotaConsumptionBL
    {
        List<TblLoadingQuotaConsumptionTO> SelectAllTblLoadingQuotaConsumptionList();
        TblLoadingQuotaConsumptionTO SelectTblLoadingQuotaConsumptionTO();
        int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO);
        int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO);
        int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingQuotaConsumption();
        int DeleteTblLoadingQuotaConsumption(SqlConnection conn, SqlTransaction tran);
    }
}