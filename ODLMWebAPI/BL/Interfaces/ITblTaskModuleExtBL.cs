using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblTaskModuleExtBL
    {
        List<TblTaskModuleExtTO> SelectAllTblTaskModuleExt();
        TblTaskModuleExtTO SelectTblTaskModuleExtTO(Int32 idTaskModuleExt);
        List<TblTaskModuleExtTO> SelectTaskModuleDetailsByEntityId(Int32 EntityId);
        int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO);
        int InsertTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO);
        int UpdateTblTaskModuleExt(TblTaskModuleExtTO tblTaskModuleExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblTaskModuleExt(Int32 idTaskModuleExt);
        int DeleteTblTaskModuleExt(Int32 idTaskModuleExt, SqlConnection conn, SqlTransaction tran);
    }
}