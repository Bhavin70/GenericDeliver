using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblFreightUpdateDAO
    {
        String SqlSelectQuery();
        List<TblFreightUpdateTO> SelectAllTblFreightUpdate();
        TblFreightUpdateTO SelectTblFreightUpdate(Int32 idFreightUpdate);
        List<TblFreightUpdateTO> SelectAllTblFreightUpdate(DateTime frmDt, DateTime toDt, int distrinctId, int talukaId);
        List<TblFreightUpdateTO> ConvertDTToList(SqlDataReader tblFreightUpdateTODT);
        int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO);
        int InsertTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblFreightUpdateTO tblFreightUpdateTO, SqlCommand cmdInsert);
        int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO);
        int UpdateTblFreightUpdate(TblFreightUpdateTO tblFreightUpdateTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblFreightUpdateTO tblFreightUpdateTO, SqlCommand cmdUpdate);
        int DeleteTblFreightUpdate(Int32 idFreightUpdate);
        int DeleteTblFreightUpdate(Int32 idFreightUpdate, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idFreightUpdate, SqlCommand cmdDelete);

    }
}