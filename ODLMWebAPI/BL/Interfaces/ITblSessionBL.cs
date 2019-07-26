using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblSessionBL
    {
        TblSessionTO SelectAllTblSession();
        List<TblSessionTO> SelectAllTblSessionList();
        TblSessionTO SelectTblSessionTO(int idsession);
        TblSessionTO getSessionAllreadyExist(Int32 idUser, Int32 ConversionUserId);
        int InsertTblSession(TblSessionTO tblSessionTO);
        int InsertTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblSession(int idsession);
        int UpdateTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran);
        int DeleteTblSession(int idsession);
        int deleteAllMsgData();
        int DeleteTblSession(int idsession, SqlConnection conn, SqlTransaction tran);
    }
}