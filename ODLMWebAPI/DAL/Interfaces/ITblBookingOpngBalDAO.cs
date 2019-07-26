using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingOpngBalDAO
    {
        String SqlSelectQuery();
        List<TblBookingOpngBalTO> SelectAllTblBookingOpngBal(DateTime asOnDate);
        TblBookingOpngBalTO SelectTblBookingOpngBal(Int32 idOpeningBal);
        List<TblBookingOpngBalTO> ConvertDTToList(SqlDataReader tblBookingOpngBalTODT);
        int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO);
        int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingOpngBalTO tblBookingOpngBalTO, SqlCommand cmdInsert);
        int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO);
        int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingOpngBalTO tblBookingOpngBalTO, SqlCommand cmdUpdate);
        int DeleteTblBookingOpngBal(Int32 idOpeningBal);
        int DeleteTblBookingOpngBal(Int32 idOpeningBal, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOpeningBal, SqlCommand cmdDelete);

    }
}