using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitProjectDetailsDAO
    {
        String SqlSelectQuery();
        List<TblVisitProjectDetailsTO> SelectAllTblVisitProjectDetails(Int32 visitId);
        DataTable SelectTblVisitProjectDetails(Int32 idProject);
        DataTable SelectAllTblVisitProjectDetails(SqlConnection conn, SqlTransaction tran);
        List<TblVisitProjectDetailsTO> ConvertDTToList(SqlDataReader projectDetailsDT);
        int InsertTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO);
        int InsertTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlCommand cmdInsert);
        int UpdateTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO);
        int UpdateTblVisitProjectDetails(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitProjectDetailsTO tblVisitProjectDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblVisitProjectDetails(Int32 idProject);
        int DeleteTblVisitProjectDetails(Int32 idProject, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idProject, SqlCommand cmdDelete);

    }
}