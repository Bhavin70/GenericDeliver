using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblSessionDAO
    {
        String SqlSelectQuery();
        TblSessionTO SelectAllTblSession();
        List<TblSessionTO> SelectAllTblSessionData();
        TblSessionTO SelectTblSession(int idsession);
        TblSessionTO getSessionAllreadyExist(Int32 CreateUserId, Int32 ConversionUserId);
        TblSessionTO SelectAllTblSession(SqlConnection conn, SqlTransaction tran);
        int InsertTblSession(TblSessionTO tblSessionTO);
        int InsertTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblSessionTO tblSessionTO, SqlCommand cmdInsert);
        int UpdateTblSession(TblSessionTO tblSessionTO);
        int UpdateTblSession(TblSessionTO tblSessionTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblSessionTO tblSessionTO, SqlCommand cmdUpdate);
        int DeleteTblSession(int idsession);
        int DeleteTblSession();
        int DeleteTblSession(int idsession, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(int idsession, SqlCommand cmdDelete);
        int ExecuteDeletionCommand(SqlCommand cmdDelete);
        List<TblSessionTO> ConvertDTToList(SqlDataReader tblSessionTODT);

    }
}