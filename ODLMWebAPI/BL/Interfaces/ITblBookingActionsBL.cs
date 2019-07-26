using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblBookingActionsBL
    {
        List<TblBookingActionsTO> SelectAllTblBookingActionsList();
        TblBookingActionsTO SelectTblBookingActionsTO(Int32 idBookingAction);
        TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran);
        TblBookingActionsTO SelectLatestBookingActionTO();
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO);
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveBookingActions(TblBookingActionsTO tblBookingActionsTO);
        int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO);
        int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblBookingActions(Int32 idBookingAction);
        int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran);
    }
}
