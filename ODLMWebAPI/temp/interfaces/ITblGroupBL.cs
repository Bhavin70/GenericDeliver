using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblGroupBL
    {
        List<TblGroupTO> SelectAllTblGroupList();
        TblGroupTO SelectTblGroupTO(Int32 idGroup);
        List<TblGroupTO> SelectAllGroupList(TblGroupTO tblGroupTO);
        List<TblGroupTO> SelectAllActiveGroupList();
        List<TblGroupTO> SelectTblGroupTOWithRate();
        int InsertTblGroup(TblGroupTO tblGroupTO);
        int InsertTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateTblGroup(TblGroupTO tblGroupTO);
        int UpdateTblGroup(TblGroupTO tblGroupTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblGroup(Int32 idGroup);
        int DeleteTblGroup(Int32 idGroup, SqlConnection conn, SqlTransaction tran);
    }
}
