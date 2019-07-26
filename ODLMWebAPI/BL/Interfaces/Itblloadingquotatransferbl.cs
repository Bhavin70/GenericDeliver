using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblLoadingQuotaTransferBL
    {
        List<TblLoadingQuotaTransferTO> SelectAllTblLoadingQuotaTransferList();
        TblLoadingQuotaTransferTO SelectTblLoadingQuotaTransferTO(Int32 idTransferNote);
        int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO);
        int InsertTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO);
        int UpdateTblLoadingQuotaTransfer(TblLoadingQuotaTransferTO tblLoadingQuotaTransferTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote);
        int DeleteTblLoadingQuotaTransfer(Int32 idTransferNote, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveLoadingQuotaTransferNotes(List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList);
        ResultMessage SaveMaterialTransferNotes(List<TblLoadingQuotaTransferTO> loadingQuotaTransferTOList);
    }
}
