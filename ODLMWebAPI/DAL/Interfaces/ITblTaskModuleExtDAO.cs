using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblTaskModuleExtDAO
    {
        String SqlSelectQuery();
        List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt();
        TblTaskModuleExtTO SelectTblTaskModuleExt(Int32 idTaskModuleExt);
        List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt(SqlConnection conn, SqlTransaction tran);
        List<TblTaskModuleExtTO> SelectTaskModuleDetailsByEntityId(Int32 EntityId);
        int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO);
        int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblTaskModuleExtTO tblTaskModuleExtTO, SqlCommand cmdInsert);
        int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO);
        int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblTaskModuleExtTO tblTaskModuleExtTO, SqlCommand cmdUpdate);
        int DeleteTblTaskModuleExt(Int32 idTaskModuleExt);
        int DeleteTblTaskModuleExt(Int32 idTaskModuleExt, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idTaskModuleExt, SqlCommand cmdDelete);
        List<TblTaskModuleExtTO> ConvertDTToList(SqlDataReader tblTaskModuleExtTODT);

    }
}