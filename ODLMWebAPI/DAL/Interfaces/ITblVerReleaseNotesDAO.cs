using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using TO;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVerReleaseNotesDAO
    {
        String SqlSelectQuery();
        List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes();
        List<TblVerReleaseNotesTO> SelectTblVerReleaseNotes(Int32 idReleaseNote);
        List<TblVerReleaseNotesTO> SelectTblVerReleaseNotesByVerId(Int32 idVersion);
        List<TblVerReleaseNotesTO> SelectAllTblVerReleaseNotes(SqlConnection conn, SqlTransaction tran);
        List<TblVerReleaseNotesTO> ConvertDTToList(SqlDataReader tblVerReleaseNotesTODT);
        int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO);
        int InsertTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlCommand cmdInsert);
        int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO);
        int UpdateTblVerReleaseNotes(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVerReleaseNotesTO tblVerReleaseNotesTO, SqlCommand cmdUpdate);
        int DeleteTblVerReleaseNotes(Int32 idReleaseNote);
        int DeleteTblVerReleaseNotes(Int32 idReleaseNote, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idReleaseNote, SqlCommand cmdDelete);

    }
}