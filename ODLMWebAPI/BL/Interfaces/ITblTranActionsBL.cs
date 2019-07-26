using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblTranActionsBL
    {
        List<TblTranActionsTO> SelectAllTblTranActions();
        List<TblTranActionsTO> SelectAllTblTranActionsList(TblTranActionsTO tblTranActionsTO);
        TblTranActionsTO SelectTblTranActionsTO(Int32 idTranActions);
        int InsertTblTranActions(TblTranActionsTO tblTranActionsTO);
        int InsertTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO);
        int UpdateTblTranActions(TblTranActionsTO tblTranActionsTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblTranActions(Int32 idTranActions);
        int DeleteTblTranActions(Int32 idTranActions, SqlConnection conn, SqlTransaction tran);
    }
}