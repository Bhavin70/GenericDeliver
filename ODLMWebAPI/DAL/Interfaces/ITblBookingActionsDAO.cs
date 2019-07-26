using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblBookingActionsDAO
    {
        String SqlSelectQuery();
        List<TblBookingActionsTO> SelectAllTblBookingActions();
        TblBookingActionsTO SelectTblBookingActions(Int32 idBookingAction);
        TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran);
        List<TblBookingActionsTO> ConvertDTToList(SqlDataReader tblBookingActionsTODT);
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO);
        int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdInsert);
        int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO);
        int UpdateTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdUpdate);
        int DeleteTblBookingActions(Int32 idBookingAction);
        int DeleteTblBookingActions(Int32 idBookingAction, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idBookingAction, SqlCommand cmdDelete);

    }
}