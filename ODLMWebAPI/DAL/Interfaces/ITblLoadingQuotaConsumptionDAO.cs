using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingQuotaConsumptionDAO
    {
        String SqlSelectQuery();
        List<TblLoadingQuotaConsumptionTO> SelectAllTblLoadingQuotaConsumption();
        TblLoadingQuotaConsumptionTO SelectTblLoadingQuotaConsumption();
        List<TblLoadingQuotaConsumptionTO> ConvertDTToList(SqlDataReader tblLoadingQuotaConsumptionTODT);
        int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO);
        int InsertTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlCommand cmdInsert);
        int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO);
        int UpdateTblLoadingQuotaConsumption(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingQuotaConsumptionTO tblLoadingQuotaConsumptionTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingQuotaConsumption();
        int DeleteTblLoadingQuotaConsumption(SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(SqlCommand cmdDelete);

    }
}