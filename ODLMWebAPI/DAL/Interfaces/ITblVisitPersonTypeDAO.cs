using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblVisitPersonTypeDAO
    {
        String SqlSelectQuery();
        DataTable SelectAllTblVisitPersonType();
        DataTable SelectTblVisitPersonType(Int32 idPersonType);
        DataTable SelectAllTblVisitPersonType(SqlConnection conn, SqlTransaction tran);
        int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO);
        int InsertTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlCommand cmdInsert);
        int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO);
        int UpdateTblVisitPersonType(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblVisitPersonTypeTO tblVisitPersonTypeTO, SqlCommand cmdUpdate);
        int DeleteTblVisitPersonType(Int32 idPersonType);
        int DeleteTblVisitPersonType(Int32 idPersonType, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idPersonType, SqlCommand cmdDelete);

    }
}