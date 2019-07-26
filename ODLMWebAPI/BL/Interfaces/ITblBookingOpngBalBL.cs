using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingOpngBalBL
    {
        List<TblBookingOpngBalTO> SelectAllTblBookingOpngBalList(DateTime asOnDate);
        TblBookingOpngBalTO SelectTblBookingOpngBalTO(Int32 idOpeningBal);
        int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO);
        int InsertTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage CalculateBookingOpeningBalance();
        int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO);
        int UpdateTblBookingOpngBal(TblBookingOpngBalTO tblBookingOpngBalTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingOpngBal(Int32 idOpeningBal);
        int DeleteTblBookingOpngBal(Int32 idOpeningBal, SqlConnection conn, SqlTransaction tran);
    }
}
