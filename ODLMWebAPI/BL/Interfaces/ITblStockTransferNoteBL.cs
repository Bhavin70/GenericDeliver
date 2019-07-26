using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblStockTransferNoteBL
    {
        List<TblStockTransferNoteTO> SelectAllTblStockTransferNoteList();
        TblStockTransferNoteTO SelectTblStockTransferNoteTO(Int32 idStkTransferNote);
        int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO);
        int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO);
        int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblStockTransferNote(Int32 idStkTransferNote);
        int DeleteTblStockTransferNote(Int32 idStkTransferNote, SqlConnection conn, SqlTransaction tran);
    }
}