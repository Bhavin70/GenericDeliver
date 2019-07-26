using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblUserExtBL
    {
        List<TblUserExtTO> SelectAllTblUserExtList();
        TblUserExtTO SelectTblUserExtTO(Int32 userId);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO);
        int InsertTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO);
        int UpdateTblUserExt(TblUserExtTO tblUserExtTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblUserExt();
        int DeleteTblUserExt(SqlConnection conn, SqlTransaction tran);
    }
}