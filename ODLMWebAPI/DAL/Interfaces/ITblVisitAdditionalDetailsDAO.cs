using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitAdditionalDetailsDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblVisitAdditionalDetails();
        DataTable SelectTblVisitAdditionalDetails(Int32 idVisitDetails);
        DataTable SelectAllTblVisitAdditionalDetails(SqlConnection conn, SqlTransaction tran);
        TblVisitAdditionalDetailsTO SelectVisitAdditionalDetails(Int32 visitId);
        List<TblVisitAdditionalDetailsTO> ConvertDTToList(SqlDataReader visitAdditionalDetailsDT);
        int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO);
        int InsertTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlCommand cmdInsert);
        int InsertTblVisitAdditionalDetailsAboutCompany(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO);
        int UpdateTblVisitAdditionalDetails(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitAdditionalDetailsTO tblVisitAdditionalDetailsTO, SqlCommand cmdUpdate);
        int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails);
        int DeleteTblVisitAdditionalDetails(Int32 idVisitDetails, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idVisitDetails, SqlCommand cmdDelete);

    }
}