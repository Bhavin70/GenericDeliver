using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblStockTransferNoteDAO
    {
        String SqlSelectQuery();
        List<TblStockTransferNoteTO> SelectAllTblStockTransferNote();
        TblStockTransferNoteTO SelectTblStockTransferNote(Int32 idStkTransferNote);
        List<TblStockTransferNoteTO> ConvertDTToList(SqlDataReader tblStockTransferNoteTODT);
        int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO);
        int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblStockTransferNoteTO tblStockTransferNoteTO, SqlCommand cmdInsert);
        int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO);
        int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblStockTransferNoteTO tblStockTransferNoteTO, SqlCommand cmdUpdate);
        int DeleteTblStockTransferNote(Int32 idStkTransferNote);
        int DeleteTblStockTransferNote(Int32 idStkTransferNote, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idStkTransferNote, SqlCommand cmdDelete);

    }
}