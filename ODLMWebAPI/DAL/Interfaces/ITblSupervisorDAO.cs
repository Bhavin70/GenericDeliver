using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSupervisorDAO
    {
        String SqlSelectQuery();
        List<TblSupervisorTO> SelectAllTblSupervisor();
        TblSupervisorTO SelectTblSupervisor(Int32 idSupervisor);
        List<TblSupervisorTO> ConvertDTToList(SqlDataReader tblSupervisorTODT);
        int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO);
        int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSupervisorTO tblSupervisorTO, SqlCommand cmdInsert);
        int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO);
        int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSupervisorTO tblSupervisorTO, SqlCommand cmdUpdate);
        int DeleteTblSupervisor(Int32 idSupervisor);
        int DeleteTblSupervisor(Int32 idSupervisor, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idSupervisor, SqlCommand cmdDelete);

    }
}