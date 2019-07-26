using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitDetailsDAO
    {
        String SqlSelectQuery();
        List<TblVisitDetailsTO> SelectAllTblVisitDetails();
        TblVisitDetailsTO SelectTblVisitDetails(int idVisit);
        DataTable SelectAllTblVisitDetails(SqlConnection conn, SqlTransaction tran);
        List<VisitFirmDetailsTO> SelectVisitFirmDetailsList();
        Int32 SelectLastVisitId();
        List<TblVisitDetailsTO> ConvertDTToList(SqlDataReader visitDetailsDT);
        int InsertTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO);
        int InsertTblVisitDetails(ref TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(ref TblVisitDetailsTO tblVisitDetailsTO, SqlCommand cmdInsert);
        int UpdateTblVisitDetails(TblVisitDetailsTO tblVisitDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitDetailsTO tblVisitDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblVisitDetails(int idVisit);
        int DeleteTblVisitDetails(int idVisit, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(int idVisit, SqlCommand cmdDelete);

    }
}