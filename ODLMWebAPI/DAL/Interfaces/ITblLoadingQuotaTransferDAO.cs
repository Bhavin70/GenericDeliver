using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblLoadingQuotaTransferDAO
    {
        String SqlSelectQuery();
        List<TblLoadingQuotaTransferTO> SelectAllTblLoadingQuotaTransfer();
        TblLoadingQuotaTransferTO SelectTblLoadingQuotaTransfer(Int32 idTransferNote);
        List<TblLoadingQuotaTransferTO> ConvertDTToList(SqlDataReader tblLoadingQuotaTransferTODT);
        int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO);
        int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlCommand cmdInsert);
        int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO);
        int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlCommand cmdUpdate);
        int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote);
        int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTransferNote, SqlCommand cmdDelete);

    }
}