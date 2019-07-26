using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using TO;
using ODLMWebAPI.DAL;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblVerReleaseNotesBL : ITblVerReleaseNotesBL
    {
        private readonly ITblVerReleaseNotesDAO _iTblVerReleaseNotesDAO;
        public TblVerReleaseNotesBL(ITblVerReleaseNotesDAO iTblVerReleaseNotesDAO)
        {
            _iTblVerReleaseNotesDAO = iTblVerReleaseNotesDAO;
        }
        #region Selection
        public List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes()
        {
            return _iTblVerReleaseNotesDAO.SelectAllTblVerReleaseNotes();
        }

        public List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotesList()
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTOList = _iTblVerReleaseNotesDAO.SelectAllTblVerReleaseNotes();
            return tblVerReleaseNotesTOList;
        }

        public TblVerReleaseNotesTO SelectTblVerReleaseNotesTO(Int32 idReleaseNote)
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTODT = _iTblVerReleaseNotesDAO.SelectTblVerReleaseNotes(idReleaseNote);
            if(tblVerReleaseNotesTODT != null && tblVerReleaseNotesTODT.Count == 1)
                return tblVerReleaseNotesTODT[0];
            else
                return null;
        }

        public List<TblVerReleaseNotesTO> SelectTblVerReleaseNotesTOByVerId(Int32 idVersion)
        {
            List<TblVerReleaseNotesTO> tblVerReleaseNotesTODT = _iTblVerReleaseNotesDAO.SelectTblVerReleaseNotesByVerId(idVersion);
            if (tblVerReleaseNotesTODT != null && tblVerReleaseNotesTODT.Count > 0)
                return tblVerReleaseNotesTODT;
            else
                return null;
        }


        #endregion

        #region Insertion
        public int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO)
        {
            return _iTblVerReleaseNotesDAO.InsertTblVerReleaseNotes(tblVerReleaseNotesTO);
        }

        public int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVerReleaseNotesDAO.InsertTblVerReleaseNotes(tblVerReleaseNotesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO)
        {
            return _iTblVerReleaseNotesDAO.UpdateTblVerReleaseNotes(tblVerReleaseNotesTO);
        }

        public int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVerReleaseNotesDAO.UpdateTblVerReleaseNotes(tblVerReleaseNotesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblVerReleaseNotes(Int32 idReleaseNote)
        {
            return _iTblVerReleaseNotesDAO.DeleteTblVerReleaseNotes(idReleaseNote);
        }

        public int DeleteTblVerReleaseNotes(Int32 idReleaseNote, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblVerReleaseNotesDAO.DeleteTblVerReleaseNotes(idReleaseNote, conn, tran);
        }

        #endregion
        
    }
}
