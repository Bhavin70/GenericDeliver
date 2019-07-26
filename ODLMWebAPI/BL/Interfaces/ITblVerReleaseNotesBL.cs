using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TO;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblVerReleaseNotesBL
    {
        List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes();
        List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotesList();
        TblVerReleaseNotesTO SelectTblVerReleaseNotesTO(Int32 idReleaseNote);
        List<TblVerReleaseNotesTO> SelectTblVerReleaseNotesTOByVerId(Int32 idVersion);
        int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO);
        int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO);
        int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblVerReleaseNotes(Int32 idReleaseNote);
        int DeleteTblVerReleaseNotes(Int32 idReleaseNote, SqlConnection conn, SqlTransaction tran);
    }
}