using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSupervisorBL
    { 
        List<TblSupervisorTO> SelectAllTblSupervisorList();
        TblSupervisorTO SelectTblSupervisorTO(Int32 idSupervisor);
        int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO);
        int InsertTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO);
        int UpdateTblSupervisor(TblSupervisorTO tblSupervisorTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSupervisor(Int32 idSupervisor);
        int DeleteTblSupervisor(Int32 idSupervisor, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewSuperwisor(TblSupervisorTO supervisorTO);
        ResultMessage UpdateSuperwisor(TblSupervisorTO supervisorTO);
    }
}