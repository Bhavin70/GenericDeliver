using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblTranActionsDAO
    {
        String SqlSelectQuery();
        List<TblTranActionsTO> SelectAllTblTranActions();
        List<TblTranActionsTO> SelectAllTblTranActionsList(TblTranActionsTO tblTranActionsTO);
        TblTranActionsTO SelectTblTranActions(Int32 idTranActions);
        List<TblTranActionsTO> ConvertDTToList(SqlDataReader tblTranActionsTODT);
        List<TblTranActionsTO> SelectAllTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran);
        int InsertTblTranActions(TblTranActionsTO tblTranActionsTO);
        int InsertTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblTranActionsTO tblTranActionsTO, SqlCommand cmdInsert);
        int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO);
        int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblTranActionsTO tblTranActionsTO, SqlCommand cmdUpdate);
        int DeleteTblTranActions(Int32 idTranActions);
        int DeleteTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTranActions, SqlCommand cmdDelete);

    }
}