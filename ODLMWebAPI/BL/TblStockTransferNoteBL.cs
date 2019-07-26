using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblStockTransferNoteBL : ITblStockTransferNoteBL
    {
        private readonly ITblStockTransferNoteDAO _iTblStockTransferNoteDAO;
        public TblStockTransferNoteBL(ITblStockTransferNoteDAO iTblStockTransferNoteDAO)
        {
            _iTblStockTransferNoteDAO = iTblStockTransferNoteDAO;
        }
        #region Selection

        public List<TblStockTransferNoteTO> SelectAllTblStockTransferNoteList()
        {
            return  _iTblStockTransferNoteDAO.SelectAllTblStockTransferNote();
        } 

        public TblStockTransferNoteTO SelectTblStockTransferNoteTO(Int32 idStkTransferNote)
        {
            return  _iTblStockTransferNoteDAO.SelectTblStockTransferNote(idStkTransferNote);
        }

        #endregion
        
        #region Insertion
        public int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO)
        {
            return _iTblStockTransferNoteDAO.InsertTblStockTransferNote(tblStockTransferNoteTO);
        }

        public int InsertTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockTransferNoteDAO.InsertTblStockTransferNote(tblStockTransferNoteTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO)
        {
            return _iTblStockTransferNoteDAO.UpdateTblStockTransferNote(tblStockTransferNoteTO);
        }

        public int UpdateTblStockTransferNote(TblStockTransferNoteTO tblStockTransferNoteTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockTransferNoteDAO.UpdateTblStockTransferNote(tblStockTransferNoteTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblStockTransferNote(Int32 idStkTransferNote)
        {
            return _iTblStockTransferNoteDAO.DeleteTblStockTransferNote(idStkTransferNote);
        }

        public int DeleteTblStockTransferNote(Int32 idStkTransferNote, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStockTransferNoteDAO.DeleteTblStockTransferNote(idStkTransferNote, conn, tran);
        }

        #endregion
        
    }
}
