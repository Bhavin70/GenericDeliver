using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUnloadingStandDescBL
    {
        List<TblUnloadingStandDescTO> SelectAllTblUnloadingStandDescList();
        TblUnloadingStandDescTO SelectTblUnloadingStandDescTO(Int32 idUnloadingStandDesc);
        List<DropDownTO> SelectAllUnloadingStandDescForDropDown();
        int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO UnloadingStandDescTO);
        int InsertTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO);
        int UpdateTblUnloadingStandDesc(TblUnloadingStandDescTO tblUnloadingStandDescTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc);
        int DeleteTblUnloadingStandDesc(Int32 idUnloadingStandDesc, SqlConnection conn, SqlTransaction tran);
    }
}