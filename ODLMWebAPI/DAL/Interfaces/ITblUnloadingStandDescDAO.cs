using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblUnloadingStandDescDAO
    {
        String SqlSelectQuery();
        List<DropDownTO> SelectAllUnloadingStandDescForDropDown();
        List<TblUnloadingStandDescTO> SelectAllTblUnloadingStandDesc();
        TblUnloadingStandDescTO SelectTblUnloadingStandDesc(Int32 idUnloadingStandDesc);
        List<TblUnloadingStandDescTO> ConvertDTToList(SqlDataReader tblUnloadingStandDescTO);
        int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO UnloadingStandDescTO);
        int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlCommand cmdInsert);
        int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO);
        int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlCommand cmdUpdate);
        int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc);
        int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idUnloadingStandDesc, SqlCommand cmdDelete);

    }
}